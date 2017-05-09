using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.AdminViewModels.AdminPostListViewModels
{
    public class AdminPostsAscendingAuthorViewModel : AdminPostsListViewModel
    {
        public AdminPostsAscendingAuthorViewModel(IAdminRepository adminRepo)
        :base(adminRepo)
        {
            this.PopulatePostList(adminRepo.ListAllPostsAscendingAuthorName());
            this.SortedBy = "AuthorAscending";
        }
    }
}
