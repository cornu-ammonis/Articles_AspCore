using Articles.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.MessageViewModels
{
    public class AuthorizedMessageListViewModel : MessageListViewModel
    {
        public AuthorizedMessageListViewModel(IMessageRepository repo, string user_name)
        {
            populateMessageList(repo.RetrieveAuthorizedMessages(user_name));
        }

    }
}
