using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models
{
    public class AdminRepository : IAdminRepository
    {
        public IList<Post> ListAllPosts()
        {
            return null;
        }
    }
}
