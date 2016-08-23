using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.Core
{
    public class BlogUser
    {
        public int BlogUserId { get; set; }
        public string user_name { get; set; }
        
        public List<CategoryBlogUser> CategoryBlogUsers { get; set; }
    }
}
