using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.AdminViewModels
{
    public class AdminPostsUnsorted : AdminPostsListViewModel
    {
        public AdminPostsUnsorted(IAdminRepository adminRepo)
            : base(adminRepo)
        {
            this.PopulatePostList(adminRepo.ListAllPosts());
        }
    }
}
