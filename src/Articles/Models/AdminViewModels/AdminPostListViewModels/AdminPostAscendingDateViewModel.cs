using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.AdminViewModels
{
    public class AdminPostAscendingDateViewModel : AdminPostsListViewModel
    {
        public AdminPostAscendingDateViewModel (IAdminRepository adminRepo)
            : base(adminRepo)
        {
            this.PopulatePostList(adminRepo.ListAllPostsAscendingDate());
            this.SortedBy = "DateDescending"; // used by view logic
        }
    }
}
