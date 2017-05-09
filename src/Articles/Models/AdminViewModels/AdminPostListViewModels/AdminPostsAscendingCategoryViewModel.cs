using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.AdminViewModels.AdminPostListViewModels
{
    public class AdminPostsAscendingCategoryViewModel : AdminPostsListViewModel
    {
        public AdminPostsAscendingCategoryViewModel(IAdminRepository adminRepo)
            :base(adminRepo)
        {
            this.PopulatePostList(adminRepo.ListAllPostsAscendingCategory());
            this.SortedBy = "CategoryAscending";
        }
    }
}
