using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.AdminViewModels.AdminPostListViewModels
{
    public class AdminPostsSearch : AdminPostsListViewModel
    {
        public AdminPostsSearch(IAdminRepository adminRepo, string search)
            :base(adminRepo)
        {
            this.PopulatePostList(adminRepo.ListPostsForSearch(search));
            this.SortedBy = "DateDescendingSearch";
        }
    }
}
