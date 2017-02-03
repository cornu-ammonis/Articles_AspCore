using Articles.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.MessageViewModels
{
    public class UnauthorizedMessageListViewModel : MessageListViewModel
    {

       public UnauthorizedMessageListViewModel(IMessageRepository _messageRepository, string user_name)
        {
            populateMessageList(_messageRepository.RetrieveUnauthorizedMessages(user_name));
        }
    }
}
