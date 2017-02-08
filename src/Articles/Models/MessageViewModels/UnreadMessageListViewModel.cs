using Articles.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.MessageViewModels
{
    public class UnreadMessageListViewModel : MessageListViewModel
    {
        public UnreadMessageListViewModel(IMessageRepository _messageRepository, string user_name)
            {
                populateMessageList(_messageRepository.RetrieveUnreadMessages(user_name));
            }
        }
    }
