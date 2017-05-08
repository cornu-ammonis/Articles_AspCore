using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.AdminViewModels
{
    public class AdminPostsListViewModel
    {
        public IList<Post> posts { get; set; }


        protected virtual void PopulatePostList(IList<Post> posts)
        {
            this.posts = posts;
        }

    }
}
