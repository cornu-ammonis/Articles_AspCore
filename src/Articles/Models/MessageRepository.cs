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
        public List<Message> RetrieveMessages(string user_name)
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


    }
}
