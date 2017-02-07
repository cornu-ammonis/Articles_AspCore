using Articles.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.MessageViewModels
{
    public class SentMessageListViewModel : MessageListViewModel
    {
        public SentMessageListViewModel(IMessageRepository _messageRepository, string user_name)
        {
            populateMessageList(_messageRepository.RetrieveSentMessages(user_name));
        }
    }
}
