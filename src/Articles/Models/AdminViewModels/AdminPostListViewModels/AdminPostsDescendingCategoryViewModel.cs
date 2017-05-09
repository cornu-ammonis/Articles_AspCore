using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.AdminViewModels.AdminPostListViewModels
{
    public class AdminPostsDescendingCategoryViewModel : AdminPostsListViewModel
    {
        public AdminPostsDescendingCategoryViewModel(IAdminRepository adminRepo)
            :base(adminRepo)
        {
            this.PopulatePostList(adminRepo.ListAllPostsDescendingCategory());
            this.SortedBy = "CategoryDescending";
        }
    }
}
