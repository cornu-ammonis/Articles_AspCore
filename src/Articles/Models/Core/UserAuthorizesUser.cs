using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.Core
{
    public class UserAuthorizesUser
    {
        public int? authorizingUserId { get; set; }
        public BlogUser authorizingUser { get; set; }

        public int? userAuthorizedId { get; set; }
        public BlogUser userAuthorized { get; set; }
    }
}
