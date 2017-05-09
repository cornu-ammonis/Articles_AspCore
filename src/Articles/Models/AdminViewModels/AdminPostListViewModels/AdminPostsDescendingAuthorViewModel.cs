using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.AdminViewModels.AdminPostListViewModels
{
    public class AdminPostsDescendingAuthorViewModel : AdminPostsListViewModel
    {
        public AdminPostsDescendingAuthorViewModel(IAdminRepository adminRepo)
            :base(adminRepo)
        {
            this.PopulatePostList(adminRepo.ListAllPostsDescendingAuthorName());
            this.SortedBy = "AuthorDescending";
        }
    }
}
