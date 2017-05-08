using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models
{
    interface IAdminRepository
    {
        IList<Post> ListAllPosts();
    }
}
