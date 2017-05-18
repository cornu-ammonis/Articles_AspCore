using Articles.Data;
using Articles.Models.Core;
using Microsoft.AspNetCore.Identity;
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
        public UserManager<ApplicationUser> _userManager;

        // requests an ApplicationDbContext and UserManager instance 
        public AdminRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            db = context;
            _userManager = userManager;
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

        // unpublishes post specified by postId
        // NOTE: will throw exception if there is no post with
        // corresponding PostId
        public void UnpublishPost(int postId)
        {
            Post post = db.Posts.First(p => p.PostId == postId);
            db.Posts.Update(post);
            post.Published = false;
            db.SaveChanges();
        }

        // publishes post specified by paramter postId
        // NOTE: will throw an exception if there is no post with
        // corresponding PostId
        public void PublishPost(int postId)
        {
            Post post = db.Posts.First(p => p.PostId == postId);
            db.Posts.Update(post);
            post.Published = true;
            db.SaveChanges();
        }


        // retrieves post specified by postId and includes navigation properties.
        // will throw an error if no post is found matching the postId
        public Post RetrievePostById(int postId)
        {
            return db.Posts
                .Include<Post, Category>(p => p.Category)
                .Include<Post, BlogUser>(p => p.Author)
                .Include<Post, List<PostTag>>(p => p.PostTags)
                .ThenInclude(posttag => posttag.Tag)
                .First(p => p.PostId == postId);
        }



        // CATEGORIES


        // lists categories whose whose names match the search string, 
        // ordered by category name ascending. match is defined as the name contains
        // the search string, or the search string contains the name
        //
        // Parameters:
        //    search:
        //       string used to select categories from database
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

        // lists all categories in the database sorted
        // by name ascending
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

        // adds a new category to the database. throws an exception if that category
        // already exists. assumes all needed category properties are initialized elsewhere.
        public void AddNewCategoryToDatabase(Category category)
        {
            if (db.Categories.Contains(category))
                throw new InvalidOperationException("Attempted to add extant category to database");

            db.Categories.Add(category);
            db.SaveChanges();
        }



        // returns a list of blog users sorted alphabetically ascending. 
        // includes list of their posts and those who subscribe to them for display in 
        // console.
        public IList<BlogUser> ListUsersAlphabetically()
        {
            IList<BlogUser> bquery =
                (from u in db.BlogUser
                 orderby u.user_name ascending
                 select u)
                 .Include<BlogUser, IList<Post>>(u => u.AuthoredPosts)
                 .Include<BlogUser, IList<UserAuthorSubscribe>>(u => u.UsersSubscribingToThisUser)
                 .ToList();

            return bquery;
        }
        
        
        // returns a list of blog users sorted alphabetically 
        // where the user name matches the search string in some way.
        // must include list of their posts and who subscribes to them for display 
        // in admin console
        public IList<BlogUser> ListUsersForSearch(string search)
        {
            IList<BlogUser> bquery =
                (from u in db.BlogUser
                 where u.user_name.Equals(search) ||
                 u.user_name.Contains(search) ||
                 search.Contains(u.user_name)
                 orderby u.user_name ascending
                 select u)
                 .Include<BlogUser, IList<Post>>(u => u.AuthoredPosts)
                 .Include<BlogUser, IList<UserAuthorSubscribe>>(u => u.UsersSubscribingToThisUser)
                 .ToList();

            return bquery;
        }


        // bans user specified by username and throws exception if user
        // not found or if already banned
        public void BanUser(string username)
        {
            // user not in database; throw an exception
            if (!db.BlogUser.Any(u => u.user_name == username))
                throw new InvalidOperationException("attempted to ban username which cannot be found in database");

            // retrieve user from database
            BlogUser user = db.BlogUser.First(u => u.user_name == username);

            // they are already banned; throw an exception (something wrong with view logic if this happens)
            if (user.isBanned)
                throw new InvalidOperationException("attempted to ban user who is already banned");

            db.BlogUser.Update(user); // mark user entity for tracking
            user.isBanned = true;     // ban the user
            db.SaveChanges();         // persist to database
        }

        // unbans user specified by username and throws exceptoin of user
        // not found or if not already banned
        public void UnbanUser(string username)
        {
            // user not in database; raise an exception
            if (!db.BlogUser.Any(u => u.user_name == username))
                throw new InvalidOperationException("attempted to unban username which cannot be found in database");

            //retrieve user from database
            BlogUser user = db.BlogUser.First(u => u.user_name == username);
            
            // they aren't already banned; throw an exception (something wrong with view logic if this happens)
            if (!user.isBanned) 
                throw new InvalidOperationException("attempted to unban user who is not banned");

            db.BlogUser.Update(user); // mark user entity for tracking
            user.isBanned = false;    // unban user
            db.SaveChanges();         // persist changes to database
        }


        // checks if user has the role "Administrator"
        // NOTE : will throw an exception if this is called using a BlogUser name
        //    for which there is not a corresponding identity account. this should 
        //    only be a concern in the dev environment, where the seed method creates 
        //    such a scenario
        //
        // Parameters:
        //     username:
        //        the username of the user to check for administrator role. 
        public async Task<bool> CheckIfAdminAsync(string username)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(username);

            if (user == null)
                throw new InvalidOperationException("attempted to CheckIfAdmin a user who could not be found");

            return await _userManager.IsInRoleAsync(user, "Administrator");
        }

        // grants admin privileges to the user specified by their username
        // Parameters:
        //     username:
        //       username of the user to escalate to admin
        public async Task MakeAdminAsync (string username)
        {
            //retrieve user by username
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                // check if user is already an administrator and throw exception if they are 
                bool isAlreadyInRole = await _userManager.IsInRoleAsync(user, "Administrator");
                if (isAlreadyInRole)
                    throw new InvalidOperationException("Attempted to grant role to user who already has that role");

                // add to role
                await _userManager.AddToRoleAsync(user, "Administrator");
            }

            // else user was null, throw exception because user wasnt found
            else
            {
                throw new InvalidOperationException("attempted grant admin to username which cannot be found in database");
            }
        }

        // revokes admin privileges for user specified by username
        // Parameters:
        //    username:
        //      username identifying the user whose admin privileges to revoke
        public async Task RevokeAdminAsync(string username)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(username);

            if (user == null)
                throw new InvalidOperationException("attempted to RevokeAdmin a user who could not be found");

            bool isAlreadyAdmin = await _userManager.IsInRoleAsync(user, "Administrator");

            if (!isAlreadyAdmin)
                throw new InvalidOperationException("attempted to RevokeAdmin a non-admin user");

            await _userManager.RemoveFromRoleAsync(user, "Administrator");
        }
        
    }
}
