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

        // lists all posts in no guaranteed order
        public IList<Post> ListAllPosts()
        {
            return db.Posts.Include<Post, Category>(p => p.Category)
                .Include<Post, BlogUser>(p => p.Author).Include<Post, List<PostTag>>(p => p.PostTags)
                .ThenInclude(posttag => posttag.Tag).ToList();
        }


        // returns list of all posts ordered by date descending, with associated navigation properties
        // included
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

        // returns list of all posts ordered by date ascending, with associated navigation properties
        // included
        public IList<Post> ListAllPostsAscendingDate()
        {
            IList<Post> postq = 
                (from p in db.Posts
                 orderby p.PostedOn ascending
                 select p)
                .Include<Post, Category>(p => p.Category)
                .Include<Post, BlogUser>(p => p.Author).Include<Post, List<PostTag>>(p => p.PostTags)
                .ThenInclude(posttag => posttag.Tag).ToList();

            return postq;
        }

        // returns list of all posts ordered by author name descending, with associated
        // navigation properties included
        public IList<Post> ListAllPostsDescendingAuthorName()
        {
            IList<Post> postq = 
                (from p in db.Posts
                 orderby p.Author.user_name descending
                 select p)
                .Include<Post, Category>(p => p.Category)
                .Include<Post, BlogUser>(p => p.Author).Include<Post, List<PostTag>>(p => p.PostTags)
                .ThenInclude(posttag => posttag.Tag).ToList();

            return postq;
        }

        // returns list of all posts ordered by author name ascending, with associated
        // navigation properties included
        public IList<Post> ListAllPostsAscendingAuthorName()
        {
            IList<Post> postq = 
                (from p in db.Posts
                 orderby p.Author.user_name ascending
                 select p)
                .Include<Post, Category>(p => p.Category)
                .Include<Post, BlogUser>(p => p.Author).Include<Post, List<PostTag>>(p => p.PostTags)
                .ThenInclude(posttag => posttag.Tag).ToList();

            return postq;
        }

        // returns list of all posts ordered by title descending with 
        // associated navigation properties included
        public IList<Post> ListAllPostsDescendingTitle()
        {
            IList<Post> postq =
                (from p in db.Posts
                 orderby p.Title descending
                 select p)
                .Include<Post, Category>(p => p.Category)
                .Include<Post, BlogUser>(p => p.Author).Include<Post, List<PostTag>>(p => p.PostTags)
                .ThenInclude(posttag => posttag.Tag)
                .ToList();

            return postq;
        }

        public void UnpublishPost(int postId)
        {
            Post post = db.Posts.First(p => p.PostId == postId);
            db.Posts.Update(post);
            post.Published = false;
            db.SaveChanges();
        }

        public void PublishPost(int postId)
        {
            Post post = db.Posts.First(p => p.PostId == postId);
            db.Posts.Update(post);
            post.Published = true;
            db.SaveChanges();
        }
    }
}
