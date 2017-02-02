using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.Core
{
   public interface IMessageRepository
    {
         void SendMessage(Message message);

         List<Message> RetrieveMessages(string user_name);

        bool CanMessage(string sender_name, string recipient_name);

        BlogUser RetrieveUserForMessaging(string user_name);

    }
}
