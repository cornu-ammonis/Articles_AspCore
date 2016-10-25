using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.Core
{
    public class PostUserLike
    {
        public int PostId { get; set; }
        public Post Post { get; set; }

        public int BlogUserId { get; set; }
        public BlogUser BlogUser { get; set; }
    }
}
