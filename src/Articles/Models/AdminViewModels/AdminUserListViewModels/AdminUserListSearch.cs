using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.AdminViewModels.AdminUserListViewModels
{
    public class AdminUserListSearch : AdminUserListViewModel
    {
        public AdminUserListSearch(IAdminRepository adminRepo, string search)
        {
            PopulateUsersList(adminRepo.ListUsersForSearch(search));
        }
    }
}
