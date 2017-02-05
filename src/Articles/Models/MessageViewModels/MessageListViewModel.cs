using Articles.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.MessageViewModels
{
    public class MessageListViewModel
    {

        private List<Message> messageList { get; set; }


        public void populateMessageList(List<Message> _messages)
        {
            messageList = _messages;
        }

        public List<Message> getMessages()
        {
            return messageList;
        }

    }
}
