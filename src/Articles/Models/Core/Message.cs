using Articles.Models.MessageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.Core
{
    public class Message
    {
        public int MessageId { get; set; }
        public string Subject { get; set; }
        public string Contents { get; set; }
        public DateTime SentTime { get; set; }
        public DateTime ModifiedTime { get; set; }

        public BlogUser Sender { get; set; }
        public BlogUser Recipient { get; set; }

        public bool Read { get; set; }
        public DateTime ReadTime { get; set; }


        public Message(MessageCreationViewModel viewModel, IMessageRepository messageRepository)
        {
            SentTime = DateTime.Now;
            Sender = messageRepository.RetrieveUserForMessaging(viewModel.AuthorName);
            Recipient = messageRepository.RetrieveUserForMessaging(viewModel.RecipientName);
            Contents = viewModel.Contents;
            Subject = viewModel.Subject;
        }

        public Message()
        {
        }


        public void markAsRead()
        {
            Read = true;
        }

        public void markAsUnread()
        {
            Read = false;
        }

        public string MessageDivId()
        {
            return "message" + this.MessageId;
        }
    }
}
