using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.Core
{
    public class UserAuthorSubscribe
    {
        public int? userId { get; set; }
        public BlogUser user { get; set; }

        public int? authorId { get; set; }
        public BlogUser author { get; set; }

    }
}
