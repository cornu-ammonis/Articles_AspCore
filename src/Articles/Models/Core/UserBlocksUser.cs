using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.Core
{
    public class UserBlocksUser
    {
        public int? userBlockingId { get; set; }
        public BlogUser blockingUser { get; set; }

        public int? blockedUserId { get; set; }
        public BlogUser userBlocked { get; set; }
    }
}
