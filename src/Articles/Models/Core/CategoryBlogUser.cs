using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.Core
{
    public class CategoryBlogUser
    {
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int BlogUserId { get; set; }
        public BlogUser BlogUser { get; set; }
    }
}
