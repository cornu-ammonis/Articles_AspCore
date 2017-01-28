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

        public UserMessageUser userMessageUser { get; set; }


    }
}
