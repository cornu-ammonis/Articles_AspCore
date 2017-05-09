using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.AdminViewModels
{
    public abstract class AdminPostsListViewModel
    {
        public AdminPostsListViewModel(IAdminRepository adminRepo)
        {
       
        }

        public IList<Post> posts { get; set; }

        // used by view logic to determine how current page is sorted
        // "Unsorted", "DateDescending", "DateAscending", "TitleDescending"
        public string SortedBy { get; set; }


        protected virtual void PopulatePostList(IList<Post> posts)
        {
            this.posts = posts;
        }

    }
}
