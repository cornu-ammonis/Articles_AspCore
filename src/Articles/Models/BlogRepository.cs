using System;
using System.Collections.Generic;
using System.Linq;

using Articles.Models;
using Articles.Models.Core;
using Articles.Data;
using Microsoft.EntityFrameworkCore;
using Articles.Models.BlogViewModels;
using System.Collections;

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

        public void UpdateCustomization(CustomizeViewModel viewModel, string user_name)
        {
            IList<Category> all_categories = this.Categories();

            BlogUser user;
            if (!db.BlogUser.Any(c => c.user_name == user_name))
            {
                 user = new BlogUser();
                user.user_name = user_name;
                user.page_size = viewModel.user_page_size;
                user.CategoryBlogUsers = new List<CategoryBlogUser>();
                db.BlogUser.Add(user);
            }
            else
            {
                user = db.BlogUser.Include<BlogUser, List<CategoryBlogUser>>(c => c.CategoryBlogUsers).Single(c => c.user_name == user_name);
                db.BlogUser.Update(user);
                user.page_size = viewModel.user_page_size;
                db.SaveChanges();
                user = db.BlogUser.Include<BlogUser, List<CategoryBlogUser>>(c => c.CategoryBlogUsers).Single(c => c.user_name == user_name);
            }

            
            
            foreach(var key in viewModel.categories.Keys)
            {
                Category category = db.Categories.Single(c => c.UrlSlug == key);
                if (viewModel.categories[key] == false)
                {
                    if(category.CategoryBlogUsers.Any(c => c.BlogUser.user_name == user_name))
                    {
                        CategoryBlogUser to_remove = category.CategoryBlogUsers.Find(c => c.BlogUser.user_name == user_name);
                        category.CategoryBlogUsers.Remove(to_remove);
                        db.CategoryBlogUser.Remove(to_remove);
                        
                    }
                }
                else  if(viewModel.categories[key] == true)
                {
                    if(category.CategoryBlogUsers.Any(c => c.BlogUser.user_name == user_name) == false)
                    {
                        //throw new InvalidOperationException();
                        /*
                         CategoryBlogUser to_add = new CategoryBlogUser();

                         to_add.BlogUser = db.BlogUser.Single(c => c.user_name == user_name);
                         to_add.Category = db.Categories.Single(c => c.Name == category.Name);
                         to_add.CategoryId = db.Categories.Single(c => c.Name == category.Name).CategoryId;
                         to_add.BlogUserId = db.BlogUser.Single(c => c.user_name == user_name).BlogUserId;
                         db.BlogUser.Single(c => c.user_name == user_name).CategoryBlogUsers.Add(to_add);
                         db.Categories.Single(c => c.Name == key);
                         db.CategoryBlogUser.Add(to_add);

                         db.SaveChanges(); */

                        CategoryBlogUser cbu1 = new CategoryBlogUser();
                        //cbu1.BlogUser = db.BlogUser.Single(c => c.user_name == user_name);
                        cbu1.Category = db.Categories.FirstOrDefault(c => c.UrlSlug == category.UrlSlug);
                        user.CategoryBlogUsers.Add(cbu1);
                        db.CategoryBlogUser.Add(cbu1);
                        
                    }
                }
            }
            db.SaveChanges();
            /*
            

            foreach (Category category in all_categories)
            {
                if (viewModel.categories.Keys.Contains(category.Name))
                {
                    if (viewModel.categories[category.Name] == true && !category.CategoryBlogUsers.Any(c => c.BlogUser.user_name == user_name))
                    {

                        CategoryBlogUser category_user = new CategoryBlogUser();
                        category_user.BlogUser = user;
                        category_user.BlogUserId = user.BlogUserId;
                        category_user.Category = category;
                        category_user.CategoryId = category.CategoryId;
                        user.CategoryBlogUsers.Add(category_user);
                        category.CategoryBlogUsers.Add(category_user);
                        db.CategoryBlogUser.Add(category_user);
                        db.SaveChanges();
                    }
                }
            } */

            //db.BlogUser.Update(user);
            //db.SaveChanges();

        }

        public IList<Post> PostsForUser(string user_name, int pageNo, int pageSize)
        {
          /* BlogUser bloguser = new BlogUser();
            bloguser.user_name = user_name;
            bloguser.CategoryBlogUsers = new List<CategoryBlogUser>();

            CategoryBlogUser cbu1 = new CategoryBlogUser();
            cbu1.BlogUser = bloguser;
            cbu1.Category = db.Categories.FirstOrDefault(c => c.UrlSlug == "seed_category_three");

            CategoryBlogUser cbu2 = new CategoryBlogUser();
            cbu2.BlogUser = bloguser;
            cbu2.Category = db.Categories.FirstOrDefault(c => c.UrlSlug == "slug_one");

            bloguser.CategoryBlogUsers.Add(cbu1);
            bloguser.CategoryBlogUsers.Add(cbu2);
            db.CategoryBlogUser.Add(cbu1);
            db.CategoryBlogUser.Add(cbu2);

            db.BlogUser.Add(bloguser);
            db.SaveChanges();   */
            
            


            List<Post> posts = new List<Post>();

            IEnumerable<Post> post_query = 
                (from p in db.Posts
                 where p.Published == true &&
                 p.Category.CategoryBlogUsers.Any(c => c.BlogUser.user_name == user_name)
                 orderby p.PostedOn descending
                 select p).Skip(pageNo * pageSize).Take(pageSize)
                 .Include<Post, Category>(p => p.Category)
                .Include<Post, List<PostTag>>(p => p.PostTags)
                .ThenInclude(posttag => posttag.Tag);

            foreach(Post post in post_query)
            {
                posts.Add(post);
            }

            return posts;

        }

        public int UserPageSize(string user_name)
        {
            BlogUser user;
            if (!db.BlogUser.Any(c => c.user_name == user_name))
            {
                return 10;
            }
            else
            {
                user = db.BlogUser.Single(c => c.user_name == user_name);
                return user.page_size;
            }
        }

        public int TotalPostsForUser(string user_name)
        {
            int totalPosts = 0;
            IEnumerable<Post> post_query =
                (from p in db.Posts
                 where p.Published == true &&
                 p.Category.CategoryBlogUsers.Any(c => c.BlogUser.user_name == user_name)
                 select p);

            foreach (Post post in post_query)
            {
                totalPosts = totalPosts + 1;
            }

            return totalPosts;
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
                (from c in db.Categories
                 orderby c.Name
                 select c)
                .Include<Category, List<CategoryBlogUser>>(c => c.CategoryBlogUsers)
                .ThenInclude<Category, CategoryBlogUser, BlogUser>(c => c.BlogUser);
                
                

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

        public IDictionary<string, string> CategoryCounts()
        {
            IList<Category> AllCategories = new List<Category>();
            IDictionary<string, string> counts = new Dictionary<string, string>();
            AllCategories = this.Categories();

            foreach (Category category in AllCategories)
            {
                int count = this.TotalPostsForCategory(category.UrlSlug);
                string counter = String.Format("({0} posts)", count);
                counts[category.UrlSlug] = counter;
            }
            return counts;

        }

        public IDictionary<string, string> CategoryCounts(IList<Category> AllCategories)
        {
            IDictionary<string, string> counts = new Dictionary<string, string>();

            foreach (Category category in AllCategories)
            {
                int count = this.TotalPostsForCategory(category.UrlSlug);
                string counter = String.Format("({0} posts)", count);
                counts[category.UrlSlug] = counter;
            }
            return counts;
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
