using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.AdminViewModels.AdminPostListViewModels
{
    public class AdminPostsDescendingTitleViewModel : AdminPostsListViewModel
    {
        public AdminPostsDescendingTitleViewModel(IAdminRepository adminRepo)
            :base(adminRepo)
        {
            this.PopulatePostList(adminRepo.ListAllPostsDescendingTitle());
            this.SortedBy = "DescendingTitle";
        }
    }
}
