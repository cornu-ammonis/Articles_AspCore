using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.AdminViewModels.AdminUserListViewModels
{

    // viewmodel holding lits of BlogUsers sorted alphabetically ascending
    public class AdminUserListAlphabetical : AdminUserListViewModel
    {
        public AdminUserListAlphabetical(IAdminRepository adminRepo)
        {
            PopulateUsersList(adminRepo.ListUsersAlphabetically());
        }
    }
}
