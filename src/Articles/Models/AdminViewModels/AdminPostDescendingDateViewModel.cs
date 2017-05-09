using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.AdminViewModels
{
    public class AdminPostDescendingDateViewModel : AdminPostsListViewModel
    {
        public AdminPostDescendingDateViewModel (IAdminRepository adminRepo)
            : base(adminRepo)
        {
            this.PopulatePostList(adminRepo.ListAllPostsDescendingDate());
        }
    }
}
