using Articles.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Articles.Data;
using Microsoft.EntityFrameworkCore;

namespace Articles.Models
{
    public class MessageRepository : BlogRepository,  IMessageRepository 
    {
        public MessageRepository(ApplicationDbContext database) : base(database)
        {
        }

        //persists new message to database if sender is permitted to message receiver
        public void SendMessage(Message message)
        {
            if(CanMessage(message.Sender.user_name, message.Recipient.user_name))
            {
                db.Messages.Add(message);
                db.SaveChanges();
            }
        }

        //currently retrieves all messages sent to specified user with no specified ordering 
        //only filter is that sender is still permitted to message recipient
        public List<Message> RetrieveAllMessages(string user_name)
        {
            List<Message> messageList = new List<Message>();
            messageList =
                (from m in db.Messages
                 where m.Recipient.user_name == user_name /*&&
                 CanMessage(m.Sender.user_name, m.Recipient.user_name)*/ //commented out -- causes multiple threads error, try different implementation 
                 orderby m.SentTime descending
                 select m)
                 .Include<Message, BlogUser>(m => m.Sender)
                 .Include<Message, BlogUser>(m => m.Recipient)
                 .ToList();
            return messageList;
        }

        public List<Message> RetrieveUnauthorizedMessages(string user_name)
        {
            List<Message> messageList = new List<Message>();
            messageList = 
                (from m in db.Messages
                 where m.Recipient.user_name == user_name &&
                  !m.Recipient.UsersThisUserAuthorizes.Any(l => l.userAuthorized.user_name == m.Sender.user_name)
                  orderby m.SentTime descending
                 select m).Include<Message, BlogUser>(m => m.Sender)
                 .Include<Message, BlogUser>(m => m.Recipient)
                 .ToList();
            return messageList;
        }

        public List<Message> RetrieveAuthorizedMessages(string user_name)
        {
            List<Message> messageList = new List<Message>();
            messageList =
                (from m in db.Messages
                 where m.Recipient.user_name == user_name &&
                 m.Recipient.UsersThisUserAuthorizes.Any(l => l.userAuthorized.user_name == m.Sender.user_name)
                 orderby m.SentTime descending
                 select m).Include<Message, BlogUser>(m => m.Sender)
                 .Include<Message, BlogUser>(m => m.Recipient)
                 .ToList();

            return messageList;
        }

        public List<Message> RetrieveSentMessages(string user_name)
        {
            List<Message> messageList = new List<Message>();
            messageList = 
                (from m in db.Messages
                 where m.Sender.user_name == user_name
                 orderby m.SentTime descending
                 select m)
                 .Include<Message, BlogUser>(m => m.Sender)
                 .Include<Message, BlogUser>(m => m.Recipient)
                 .ToList();

            return messageList;
        }

        public List<Message> RetrieveMessagesBetweenUsers(string user_name_1, string user_name_2)
        {
            List<Message> messageList = new List<Message>();
            messageList = 
                (from m in db.Messages
                 where m.Sender.user_name == user_name_1 
                    && m.Recipient.user_name == user_name_2
                 where m.Sender.user_name == user_name_2
                    && m.Recipient.user_name == user_name_1
                 orderby m.SentTime descending
                 select m)
                 .Include<Message, BlogUser>(m => m.Sender)
                 .Include<Message, BlogUser>(m => m.Recipient)
                 .ToList();

            return messageList;
        }

        public List<Message> RetrieveUnreadMessages(string user_name)
        {
            List<Message> messageList = new List<Message>();
            messageList =
                (from m in db.Messages
                 where m.Recipient.user_name == user_name &&
                 m.Read == false
                 orderby m.SentTime descending
                 select m).Include<Message, BlogUser>(m => m.Sender)
                    .Include<Message, BlogUser>(m => m.Recipient)
                    .ToList();

            return messageList;
        }

        //checks whether specified sender may message specified recipient, according to user names
        public bool CanMessage(string sender_name, string recipient_name)
        {
            if(CheckIfBlocked(recipient_name, sender_name)) {
                return false;
            }
            else if (CheckIfPublicMessaging(recipient_name))
            {
                return true;
            }
            else if (CheckIfAuthorized(recipient_name, sender_name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

       public BlogUser RetrieveUserForMessaging(string user_name)
        {
            return RetrieveUser(user_name);
        }


        public bool MarkAsRead(int messageId)
        {
            try
            {
                Message toModify = db.Messages.Single(m => m.MessageId == messageId);
                db.Messages.Update(toModify);
                toModify.markAsRead();
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

           
        }

        public async Task<bool> CheckIfReadAsync(int messageId)
        {
            return await db.Messages.AnyAsync(m => m.MessageId == messageId && m.Read);
        }

    }
}
