using Articles.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models
{
    public class AdminRepository : IAdminRepository
    {
        public ApplicationDbContext db;
        public AdminRepository(ApplicationDbContext context)
        {
            db = context;
        }
        public IList<Post> ListAllPosts()
        {
            return null;
        }
    }
}
