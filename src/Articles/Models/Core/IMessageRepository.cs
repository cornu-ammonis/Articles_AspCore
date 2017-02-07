using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.Core
{
   public interface IMessageRepository
    {
        void SendMessage(Message message);

        List<Message> RetrieveAllMessages(string user_name);
        List<Message> RetrieveUnauthorizedMessages(string user_name);
        List<Message> RetrieveAuthorizedMessages(string user_name);
        List<Message> RetrieveSentMessages(string user_name);
        List<Message> RetrieveMessagesBetweenUsers(string user_name_1, string user_name_2);

        bool CanMessage(string sender_name, string recipient_name);

        BlogUser RetrieveUserForMessaging(string user_name);

    }
}
