using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.AdminViewModels.AdminPostListViewModels
{
    public class AdminPostsAscendingTitleViewModel : AdminPostsListViewModel
    {
        public AdminPostsAscendingTitleViewModel(IAdminRepository adminRepo)
            :base(adminRepo)
        {
            this.PopulatePostList(adminRepo.ListAllPostsAscendingTitle());
            this.SortedBy = "TitleAscending";
        }
    }
}
