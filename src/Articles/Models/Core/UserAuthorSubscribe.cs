using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.Core
{
    public class UserAuthorSubscribe
    {
        public int? subscribingUserId { get; set; }
        public BlogUser subscribingUser { get; set; }

        public int? userSubscribedId { get; set; }
        public BlogUser userSubscribed { get; set; }

    }
}
