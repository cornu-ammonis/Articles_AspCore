using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.AdminViewModels
{
    public class AdminPostsAscendingDateViewModel : AdminPostsListViewModel
    {
        public AdminPostsAscendingDateViewModel (IAdminRepository adminRepo)
            : base(adminRepo)
        {
            this.PopulatePostList(adminRepo.ListAllPostsAscendingDate());
            this.SortedBy = "DateAscending"; // used by view logic
        }
    }
}
