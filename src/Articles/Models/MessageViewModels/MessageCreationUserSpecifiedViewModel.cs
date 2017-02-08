using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.MessageViewModels
{
    public class MessageCreationUserSpecifiedViewModel : MessageCreationViewModel
    {
        public MessageCreationUserSpecifiedViewModel(string recipientUserName)
        {
            RecipientName = recipientUserName;
        }

        
    }
}
