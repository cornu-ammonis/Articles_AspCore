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

        // returns list of all posts ordered by title ascending with
        // associated navigation properties included
        public IList<Post> ListAllPostsAscendingTitle()
        {
            IList<Post> postq = 
                (from p in db.Posts
                 orderby p.Title ascending
                 select p)
                .Include<Post, Category>(p => p.Category)
                .Include<Post, BlogUser>(p => p.Author).Include<Post, List<PostTag>>(p => p.PostTags)
                .ThenInclude(posttag => posttag.Tag)
                .ToList();

            return postq;

        }

        // returns list of all posts ordered by category name descending with
        // associated navigation properties included
        public IList<Post> ListAllPostsDescendingCategory()
        {
            IList<Post> postq =
                (from p in db.Posts
                 orderby p.Category.Name descending
                 select p)
                .Include<Post, Category>(p => p.Category)
                .Include<Post, BlogUser>(p => p.Author).Include<Post, List<PostTag>>(p => p.PostTags)
                .ThenInclude(posttag => posttag.Tag)
                .ToList();

            return postq;
        }

        // returns all posts ordered by category name ascending with 
        // associated navigation properties included
        public IList<Post> ListAllPostsAscendingCategory()
        {
            IList<Post> postq = 
                (from p in db.Posts
                 orderby p.Category.Name ascending
                 select p)
                .Include<Post, Category>(p => p.Category)
                .Include<Post, BlogUser>(p => p.Author).Include<Post, List<PostTag>>(p => p.PostTags)
                .ThenInclude(posttag => posttag.Tag)
                .ToList();

            return postq;
        }


        // lists all posts which in some way match the search string.
        // NOTE: this defaults to sorting by date, but future implementations
        // should permit for the selection of alternative sorting methods 
        public IList<Post> ListPostsForSearch(string search)
        {
            IList<Post> postq =
                (from p in db.Posts
                 where p.Title.Contains(search) ||   // title contains search
                 search.Contains(p.Title) ||         // search contains title
                 p.Category.Name.Contains(search) || // category name contains search
                 search.Contains(p.Category.Name) || // search contains category name
                 // any of the tag names contain the search or search contains any tag names
                 p.PostTags.Any(pt => pt.Tag.Name.Contains(search) || search.Contains(pt.Tag.Name))
                 || search.Contains(p.Author.user_name) // search contains author name
                 orderby p.PostedOn descending          // DEFAULT - sort by date
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





        // CATEGORIES

        public IList<Category> ListCategoriesForSearch(string search)
        {
            IList<Category> cquery =
                (from c in db.Categories
                 where c.Name.Contains(search) ||
                 search.Contains(c.Name)
                 orderby c.Name ascending
                 select c)
                 .Include<Category, IList<Post>>(c => c.Posts)
                 .ToList();

            return cquery;
        }

        public IList<Category> ListAllCategories()
        {
            IList<Category> cquery =
                (from c in db.Categories
                 orderby c.Name ascending
                 select c)
                 .Include<Category, IList<Post>>(c => c.Posts)
                 .ToList();

            return cquery;
        }
    }
}
