using Articles.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.AdminViewModels
{

    // abstract base class for displaying list of BlogUsers
    // PopulateUsersList will be used in constructors of derived classes to 
    // assign users list to the property
    public abstract class AdminUserListViewModel
    {
        public IList<BlogUser> Users { get; set; }


        // @param _users -- list of users to hold in this.Users
        protected virtual void PopulateUsersList(IList<BlogUser> _users)
        {
            this.Users = _users;
        }

    }
}
