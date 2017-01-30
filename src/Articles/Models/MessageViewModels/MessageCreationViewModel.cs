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


        
    }
}
