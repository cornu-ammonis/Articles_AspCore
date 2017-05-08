using Articles.Data;
using Articles.Models.Core;
using Microsoft.EntityFrameworkCore;
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
            return db.Posts.Include<Post, Category>(p => p.Category)
                .Include<Post, BlogUser>(p => p.Author).Include<Post, List<PostTag>>(p => p.PostTags)
                .ThenInclude(posttag => posttag.Tag).ToList();
        }

        public IList<Post> ListAllPostsDescendingDate()
        {
            IList<Post> postq = 
                (from p in db.Posts
                 orderby p.PostedOn descending
                 select p)
                .Include<Post, Category>(p => p.Category)
                .Include<Post, BlogUser>(p => p.Author).Include<Post, List<PostTag>>(p => p.PostTags)
                .ThenInclude(posttag => posttag.Tag).ToList();
            return postq;
        }
    }
}
