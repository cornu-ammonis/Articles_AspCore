using System;
using System.Collections.Generic;
using System.Linq;

using Articles.Models;
using Articles.Models.Core;
using Articles.Data;
using Microsoft.EntityFrameworkCore;

namespace Articles.Models

{
    class BlogRepository : IBlogRepository
    {

        public ApplicationDbContext db;

        // ninject constructor 
        public BlogRepository(ApplicationDbContext database)
        {
            db = database;
        }

        public IList<Post> Posts(int pageNo, int pageSize) {

            List<Post> posts = new List<Post>();

            IEnumerable<Post> post_query =
                (from p in db.Posts
                 where p.Published == true
                 orderby p.PostedOn descending
                 select p)
                .Skip(pageNo * pageSize).Take(pageSize)
                .Include<Post, Category>(p => p.Category)
                .Include<Post, List<PostTag>>(p => p.PostTags)
                .ThenInclude(posttag => posttag.Tag);

            foreach (Post post in post_query)
            {
                posts.Add(post);
            }

            return posts;


            }

        public IList<Post> PostsForCategory(string categorySlug, int pageNo, int pageSize)
        {
            List<Post> posts = new List<Post>();

            IEnumerable<Post> post_query =
                (from p in db.Posts
                 where p.Published == true
                 where p.Category.UrlSlug.Equals(categorySlug)
                 orderby p.PostedOn descending
                 select p)
                 .Skip(pageNo * pageSize).Take(pageSize)
                 .Include<Post, Category>(p => p.Category)
                .Include<Post, List<PostTag>>(p => p.PostTags)
                .ThenInclude(posttag => posttag.Tag); 
            
            foreach (Post post in post_query)
            {
                posts.Add(post);
            }

            return posts;
        }

        public IList<Post> PostsForTag(string tagSlug, int pageNo, int pageSize)
        {
            List<Post> posts = new List<Post>();

            IEnumerable<Post> post_query =
                (from p in db.Posts
                 where p.Published == true
                 where p.PostTags.Any(t => t.Tag.UrlSlug.Equals(tagSlug))
                 orderby p.PostedOn descending
                 select p)
                 .Skip(pageNo * pageSize).Take(pageSize)
                  .Include<Post, Category>(p => p.Category)
                .Include<Post, List<PostTag>>(p => p.PostTags)
                .ThenInclude(posttag => posttag.Tag);

            foreach (Post post in post_query)
            {
                posts.Add(post);
            }

            return posts;
        }

        public IList<Post> PostsForSearch(string search, int pageNo, int pageSize)
        {
            List<Post> posts = new List<Post>();

            IEnumerable<Post> post_query =
                (from p in db.Posts
                 where p.Published == true &&
                 ( p.Title.Contains(search) || p.Category.Name.Equals(search) || p.PostTags.Any(t => t.Tag.Name.Equals(search)))
                 orderby p.PostedOn descending
                 select p)
                 .Skip(pageNo * pageSize).Take(pageSize)
                 .Include<Post, Category>(p => p.Category)
                .Include<Post, List<PostTag>>(p => p.PostTags)
                .ThenInclude(posttag => posttag.Tag);

            foreach (Post post in post_query)
            {
                posts.Add(post);
            }

            return posts;
        }

        public int TotalPosts(bool checkIsPublished = true)
        {
            int total = 0;
            IEnumerable<Post> p_query =
                from p in db.Posts
                where !checkIsPublished || p.Published == true
                select p;

            foreach (Post post in p_query)
            {
                total = total + 1;
            }

            return total;

        }

        public int TotalPostsForCategory(string categorySlug)
        {
            int total = 0;
            IEnumerable<Post> p_query =
                from p in db.Posts
                where p.Published == true && p.Category.UrlSlug.Equals(categorySlug)
                select p;

            

            foreach (Post post in p_query)
            {
                total = total + 1;
            }

            return total;
               
        }

        public int TotalPostsForTag(string tagSlug)
        {
            int total = 0;
            IEnumerable<Post> p_query =
                from p in db.Posts
                where p.Published == true && p.PostTags.Any(t => t.Tag.UrlSlug.Equals(tagSlug))
                select p;

            foreach (Post post in p_query)
            {
                total = total + 1;
            }

            return total;
        }

        public int TotalPostsForSearch(string search)
        {
            int total = 0;

            IEnumerable<Post> p_query =
                from p in db.Posts
                where p.Published == true
                where p.Title.Contains(search) || p.Category.Name.Equals(search) || p.PostTags.Any(t => t.Tag.Name.Equals(search))
                select p;

            foreach (Post post in p_query)
            {
                total = total + 1;
            }

            return total;
        }

        //theres probably a better way to implement this without the unecessary loop -- maybe dont use LINQ
        public Category Category(string categorySlug)
        {
            Category category_instance = null;
            /*
            IEnumerable<Category> c_query =
                from c in db.Categories
                where c.UrlSlug.Equals(categorySlug)
                select c;

            foreach (Category category in c_query)
            {
                category_instance = category;
            } */

            // ** implementation without loop 
            category_instance = db.Categories.FirstOrDefault(c => c.UrlSlug.Equals(categorySlug));

            return category_instance;
        }

        public Tag Tag(string tagSlug)
        {
            Tag tag_instance = null;

            tag_instance = db.Tags.FirstOrDefault(t => t.UrlSlug.Equals(tagSlug));

           
            
            return tag_instance;
        }

        public Post Post(int year, int month, string titleSlug)
        {
             Post post = null;
            try
            {
                //post = db.Posts.SingleOrDefault(p => p.PostedOn.Year == year && p.PostedOn.Month == month && p.UrlSlug.Equals(titleSlug));

               post = (from p in db.Posts
                                  where p.PostedOn.Year == year && p.PostedOn.Month == month && p.UrlSlug.Equals(titleSlug)
                                  select p)
                                 .Include<Post, Category>(p => p.Category)
                                 .Include<Post, List<PostTag>>(p => p.PostTags)
                                 .ThenInclude(posttag => posttag.Tag)
                                 .SingleOrDefault();

            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Post selection failed due to title and date-published duplication");
            }
            return post;

           
        }


        public IList<Category> Categories()
        {
            List<Category> categories = new List<Category>();
            IEnumerable<Category> c_query =
                from c in db.Categories
                orderby c.Name
                select c;

            foreach (Category category in c_query)
            {
                categories.Add(category);
            }

            return categories;
        }

        public IList<Tag> Tags()
        {
            List<Tag> tags = new List<Tag>();
            IEnumerable<Tag> t_query =
                from t in db.Tags
                orderby t.Name
                select t;

            foreach (Tag tag in t_query)
            {
                tags.Add(tag);
            }

            return tags;
        }


        public IList<Post> Posts(int pageNo, int pageSize, string sortColumn, bool sortByAscending)
        {
            IList<Post> posts = new List<Post>();
           

            switch(sortColumn)
            {
                case "Title":
                    if (sortByAscending)
                    {
                        IEnumerable<Post> q_posts = (from p in db.Posts
                                                    orderby p.Title
                                                    select p) .Skip(pageNo * pageSize)
                                                   .Take(pageSize);
                        foreach(Post post in q_posts)
                        {
                            posts.Add(post);
                        }

                    }
                    else
                    {
                        IEnumerable<Post> q_posts = (from p in db.Posts
                                                    orderby p.Title descending
                                                    select p)
                                                   .Skip(pageNo * pageSize)
                                                   .Take(pageSize);
                        foreach (Post post in q_posts)
                        {
                            posts.Add(post);
                        }
                    }
                    break;
                case "Published":
                    if(sortByAscending)
                    {
                        IEnumerable<Post> q_posts = (from p in db.Posts
                                                    orderby p.Published
                                                    select p)
                                                   .Skip(pageNo * pageSize)
                                                   .Take(pageSize);
                        foreach (Post post in q_posts)
                        {
                            posts.Add(post);
                        }
                    }
                    else
                    {
                        IEnumerable<Post> q_posts = (from p in db.Posts
                                                    orderby p.Published descending
                                                    select p)
                                                   .Skip(pageNo * pageSize)
                                                   .Take(pageSize);
                        foreach (Post post in q_posts)
                        {
                            posts.Add(post);
                        }
                    }
                    break;
                case "PostedOn":
                    
                        if(sortByAscending)
                        {
                            IEnumerable<Post> q_posts = (from p in db.Posts
                                                        orderby p.PostedOn
                                                        select p)
                                                   .Skip(pageNo * pageSize)
                                                   .Take(pageSize);
                            foreach (Post post in q_posts)
                            {
                                posts.Add(post);
                            }
                        }
                        else
                        {
                            IEnumerable<Post> q_posts = (from p in db.Posts
                                                        orderby p.PostedOn descending
                                                        select p)
                                                   .Skip(pageNo * pageSize)
                                                   .Take(pageSize);
                            foreach (Post post in q_posts)
                            {
                                posts.Add(post);
                            }
                        }
                    break;
                case "Modified": 
                    if(sortByAscending)
                    {
                        IEnumerable<Post> q_posts = (from p in db.Posts
                                                    orderby p.Modified
                                                    select p)
                                                   .Skip(pageNo * pageSize)
                                                   .Take(pageSize);
                        foreach (Post post in q_posts)
                        {
                            posts.Add(post);
                        }
                    }
                    else
                    {
                        IEnumerable<Post> q_posts = (from p in db.Posts
                                                    orderby p.Modified descending
                                                    select p)
                                                   .Skip(pageNo * pageSize)
                                                   .Take(pageSize);
                        foreach (Post post in q_posts)
                        {
                            posts.Add(post);
                        }
                    }
                    break;
                case "Category":
                    if(sortByAscending)
                    {
                        IEnumerable<Post> q_posts = (from p in db.Posts
                                                    orderby p.Category.Name
                                                    select p)
                                                   .Skip(pageNo * pageSize)
                                                   .Take(pageSize);
                        foreach (Post post in q_posts)
                        {
                            posts.Add(post);
                        }
                    }
                    else
                    {
                        IEnumerable<Post> q_posts = (from p in db.Posts
                                                    orderby p.Category.Name descending
                                                    select p)
                                                   .Skip(pageNo * pageSize)
                                                   .Take(pageSize);
                        foreach (Post post in q_posts)
                        {
                            posts.Add(post);
                        }
                    }
                    break;
                default:
                    IEnumerable<Post> qposts = (from p in db.Posts
                                                orderby p.PostedOn descending
                                                select p)
                                                   .Skip(pageNo * pageSize)
                                                   .Take(pageSize);
                    foreach (Post post in qposts)
                    {
                        posts.Add(post);
                    }
                   break; 
            }
            return posts;
        }

    }
}
