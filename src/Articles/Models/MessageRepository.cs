using Articles.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Articles.Data;

namespace Articles.Models
{
    public class MessageRepository : BlogRepository,  IMessageRepository 
    {
        public MessageRepository(ApplicationDbContext database) : base(database)
        {
        }

        public void SendMessage(Message message)
        {
            
        }

        public List<Message> RetrieveMessages(string user_name)
        {
            return new List<Message>();
        }

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


    }
}
