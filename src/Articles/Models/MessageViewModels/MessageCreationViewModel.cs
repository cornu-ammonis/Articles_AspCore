using Articles.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.MessageViewModels
{
    public class MessageCreationViewModel
    {
        public string Subject { get; set; }
        public string Contents { get; set; }
        public string AuthorName { get; set; }
        public string RecipientName { get; set; }


        public void sendMessage(IMessageRepository messageRepository)
        {
            Message toSend = new Message(this, messageRepository);
            messageRepository.SendMessage(toSend);
        }


        
    }
}
