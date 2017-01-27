using System;
using System.Collections.Generic;
using System.Linq;

using Articles.Models;
using Articles.Models.Core;
using Articles.Data;
using Microsoft.EntityFrameworkCore;
using Articles.Models.BlogViewModels;
using System.Collections;
using System.Threading.Tasks;

namespace Articles.Models

{
    //unless indicated otherwise, all methods which return IList<Post> and which contain pageNo and pageSize
    // parameters return [pageSize] posts, sorted by date posted (descending; recent first)
    //after skipping [pageNo]*[pageSize] posts such that only posts for the current page are returned.

   public class BlogRepository : IBlogRepository
    {

        public ApplicationDbContext db;

        // ninject constructor 
        public BlogRepository(ApplicationDbContext database)
        {
            db = database;
        }

        //used to return some posts from all published posts for a "recent posts" feed
        public IList<Post> Posts(int pageNo, int pageSize) {

            List<Post> posts = new List<Post>();

            IEnumerable<Post> post_query =
                (from p in db.Posts
                 where p.Published == true
                 orderby p.PostedOn descending
                 select p)
                .Skip(pageNo * pageSize).Take(pageSize)
                .Include<Post, BlogUser>(p => p.Author)
                .Include<Post, Category>(p => p.Category)
                .Include<Post, List<PostTag>>(p => p.PostTags)
                .ThenInclude(posttag => posttag.Tag);

            foreach (Post post in post_query)
            {
                posts.Add(post);
            }

            return posts;


            }

        //used to return some posts from a particlar category, identified by that category's UrlSlug
        //which is given as the first parameter. 
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
                 .Include<Post, BlogUser>(p => p.Author)
                 .Include<Post, Category>(p => p.Category)
                .Include<Post, List<PostTag>>(p => p.PostTags)
                .ThenInclude(posttag => posttag.Tag); 
            
            foreach (Post post in post_query)
            {
                posts.Add(post);
            }

            return posts;
        }

        //used to return some posts with a particular tag, identified by that tag's UrlSlug which
        //is given as the first parameter.
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
                 .Include<Post, BlogUser>(p => p.Author)
                  .Include<Post, Category>(p => p.Category)
                .Include<Post, List<PostTag>>(p => p.PostTags)
                .ThenInclude(posttag => posttag.Tag);

            foreach (Post post in post_query)
            {
                posts.Add(post);
            }

            return posts;
        }

        //used to return some posts matching a particular search term, which is given as a string as the
        //first parameter. queries for posts whose title contains the search term, or whose category's name
        //equals the search term, or whose list of tags contains a tag whose name equals the search term.
        public IList<Post> PostsForSearch(string search, int pageNo, int pageSize)
        {
            List<Post> posts = new List<Post>();

            IEnumerable<Post> post_query =
                (from p in db.Posts
                 where p.Published == true &&
                 //if the post's title contains the search term, or the post's category's name or any of its 
                 //tags' names equal the search term , return that post 
                
                 (p.Title.Contains(search) || p.Category.Name.Equals(search) || p.PostTags.Any(t => t.Tag.Name.Equals(search)))
                 orderby p.PostedOn descending
                 select p)
                 .Skip(pageNo * pageSize).Take(pageSize)
                 .Include<Post, BlogUser>(p => p.Author)
                 .Include<Post, Category>(p => p.Category)
                .Include<Post, List<PostTag>>(p => p.PostTags)
                .ThenInclude(posttag => posttag.Tag);

            foreach (Post post in post_query)
            {
                posts.Add(post);
            }

            return posts;
        }

        //takes viewmodel from HttpPost action and commits values returned from customize page to database. 
        public void UpdateCustomization(CustomizeViewModel viewModel, string user_name)
        {
          

            BlogUser user;
            if (!db.BlogUser.Any(c => c.user_name == user_name))
            {
                user = this.GenerateUser(user_name);
            }
            else
            {
                user = db.BlogUser
                    .Include<BlogUser, List<CategoryBlogUser>>(c => c.CategoryBlogUsers)
                    .Include<BlogUser, List<BlogUser>>(u => u.SubscribedAuthors)
                    .Single(c => c.user_name == user_name);
                db.BlogUser.Update(user);
                user.page_size = viewModel.user_page_size;
                db.SaveChanges();
                user = db.BlogUser
                    .Include<BlogUser, List<CategoryBlogUser>>(c => c.CategoryBlogUsers)
                    .Include<BlogUser, List<UserAuthorSubscribe>>(ua => ua.UserAuthorSubscribes)
                    .Single(c => c.user_name == user_name);
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
                
                        CategoryBlogUser cbu1 = new CategoryBlogUser();
                        //cbu1.BlogUser = db.BlogUser.Single(c => c.user_name == user_name);
                        cbu1.Category = db.Categories.FirstOrDefault(c => c.UrlSlug == category.UrlSlug);
                        user.CategoryBlogUsers.Add(cbu1);
                        db.CategoryBlogUser.Add(cbu1);
                        
                    }
                }
            }

            db.Update(user);
            foreach(var key in viewModel.subscribed_authors.Keys)
            {
                BlogUser author = db.BlogUser.Include<BlogUser, List<UserAuthorSubscribe>>(bu => bu.AuthorUserSubscribes)
                    .Single(c => c.user_name == key);
                db.Update(author);
                if (viewModel.subscribed_authors[key] == false)
                {
                    if(db.UserAuthorSubscribes.Any(ua => ua.user.user_name == user_name && ua.author.user_name == key))
                    {
                        db.Update(user);
                        UserAuthorSubscribe to_remove = db.UserAuthorSubscribes.Single(ua => ua.user.user_name == user_name
                        && ua.author.user_name == key);
                        user.UserAuthorSubscribes.Remove(to_remove);
                        author.AuthorUserSubscribes.Remove(to_remove);
                        author.subscribers_count = author.subscribers_count - 1;
                        db.UserAuthorSubscribes.Remove(to_remove);
                    }
                }

                if (viewModel.subscribed_authors[key] == true)
                {
                    if(db.UserAuthorSubscribes.Any(ua => ua.user.user_name == user_name && ua.author.user_name == key)
                        == false)
                    {
                        UserAuthorSubscribe nsubscription = new UserAuthorSubscribe();
                        nsubscription.authorId = author.BlogUserId;
                        nsubscription.userId = user.BlogUserId;
                        nsubscription.author = author;
                        nsubscription.user = user;
                        author.AuthorUserSubscribes.Add(nsubscription);
                        author.subscribers_count = author.subscribers_count + 1;
                        user.UserAuthorSubscribes.Add(nsubscription);
                        db.UserAuthorSubscribes.Add(nsubscription);
                    }
                }
            }
            db.SaveChanges();
           
        }

        //returns only posts in a category for which the junction table link between that category
        //and the current BlogUser exists 
        public IList<Post> CustomPostsForUser(string user_name, int pageNo, int pageSize)
        {
         

            List<Post> posts = new List<Post>();

            IEnumerable<Post> post_query = 
                (from p in db.Posts
                 where p.Published == true &&
                 p.Category.CategoryBlogUsers.Any(c => c.BlogUser.user_name == user_name)
                 orderby p.PostedOn descending
                 select p).Skip(pageNo * pageSize).Take(pageSize)
                 .Include<Post, BlogUser>(p => p.Author)
                 .Include<Post, Category>(p => p.Category)
                .Include<Post, List<PostTag>>(p => p.PostTags)
                .ThenInclude(posttag => posttag.Tag);

            foreach(Post post in post_query)
            {
                posts.Add(post);
            }

            return posts;

        }

        //used to return some posts where the current user has chosen to subscribe to the author of those 
        //posts. 
        public IList<Post> SubscribedPostsForUser(string user_name, int pageNo, int pageSize)
        {
            List<Post> posts = new List<Post>();
            BlogUser user = this.RetrieveUser(user_name);

            IEnumerable<Post> post_query =
                (from p in db.Posts
                 where p.Published == true &&
                user.UserAuthorSubscribes.Any(ua => ua.authorId == p.Author.BlogUserId)
                //p.Author.AuthorUserSubscribes.Any(au => au.userId == user.BlogUserId)
                 orderby p.PostedOn descending
                 select p).Skip(pageNo * pageSize).Take(pageSize)
                 .Include<Post, BlogUser>(p => p.Author)
                 .Include<Post, Category>(p => p.Category)
                .Include<Post, List<PostTag>>(p => p.PostTags)
                .ThenInclude(posttag => posttag.Tag);

            foreach (Post post in post_query)
            {
                
                posts.Add(post);
            }

            return posts;
        }

        //returns the total number of posts which the SubSubscribedPostsForUser method could return; 
        //i.e., counts the number of posts in the database where the current user has chosen to subscribe 
        //to the author of those posts 
        public int TotalSubscribedPostsForUser(string user_name)
        {
            int total = 0;
            BlogUser user = this.RetrieveUser(user_name);

            IEnumerable<Post> post_query =
                (from p in db.Posts
                 where p.Published == true &&
                 user.UserAuthorSubscribes.Any(ua => ua.authorId == p.Author.BlogUserId)
                //p.Author.AuthorUserSubscribes.Any(au => au.userId == user.BlogUserId)
                 orderby p.PostedOn descending
                 select p);

            foreach (Post post in post_query)
            {
                total = total + 1;
            }
            return total;
        }

        //queries database for pagesize value associated with current BlogUser, identified by string
        // user name -- returns 10 if not found 
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

        //counts the total number of posts which the PostsForUser method could access, i.e. counts the 
        //number of posts whose category is included in the current user's custom feed
        public int TotalCustomPostsForUser(string user_name)
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

        public void SavePostForUser(int year, int month, string titleSlug, string user_name)
        {
          
           
            BlogUser user;
            if (!db.BlogUser.Any(c => c.user_name == user_name))
            {
               user = this.GenerateUser(user_name);
            }
            Post post_tosave = this.Post(year, month, titleSlug);
            user = db.BlogUser.Include<BlogUser, IList<PostUserSave>>(u => u.PostUserSaves).Single(c => c.user_name == user_name);
            db.BlogUser.Update(user);
            db.Posts.Update(post_tosave);
            PostUserSave toSave = new PostUserSave();
            toSave.Post = post_tosave;
            user.PostUserSaves.Add(toSave);
            db.SaveChanges();

        }

        public void UnsavePostForUser(int year, int month, string titleSlug, string user_name)
        {
            BlogUser user = db.BlogUser.Include<BlogUser, IList<PostUserSave>>(u => u.PostUserSaves)
                .Single(u => u.user_name == user_name);
               
            db.BlogUser.Update(user);
            /* Post post_to_remove = this.Post(year, month, titleSlug);
            db.Posts.Update(post_to_remove);
            user.BlogUserPosts.Remove(post_to_remove); */
           PostUserSave toRemove = db.PostUserSaves.Single(u => u.BlogUserId == user.BlogUserId && u.Post.UrlSlug == titleSlug);
            user.PostUserSaves.Remove(toRemove);
            db.PostUserSaves.Remove(toRemove);
            db.SaveChanges();
        }

        public bool CheckIfSaved(Post post, string username)
        {
            BlogUser user = this.RetrieveUser(username);
            if (user.PostUserSaves.Any(p => p.PostId == post.PostId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CheckIfSavedAsync(Post post, string user_name)
        {
            bool IsSaved = await db.BlogUser.AnyAsync(u => u.user_name == user_name && u.PostUserSaves.Any(p => p.PostId == post.PostId));
            return IsSaved;
        }


        //reuturns some posts which the current user has saved.
        public IList<Post> PostsUserSaved(string username, int pageNo, int pageSize)
        {
            List<Post> posts = new List<Post>();
            BlogUser user = db.BlogUser.Include<BlogUser, List<PostUserSave>>(u => u.PostUserSaves)
                .SingleOrDefault(u => u.user_name == username);
            if(user == null)
            {
                return posts;
            }

            IEnumerable<Post> post_query =
                (from p in db.Posts
                 where p.Published == true &&
                 user.PostUserSaves.Any(c => c.PostId == p.PostId)
                 orderby p.PostedOn descending
                 select p).Skip(pageNo * pageSize).Take(pageSize)
                 .Include<Post, BlogUser>(p => p.Author)
                 .Include<Post, Category>(p => p.Category)
                  .Include(p => p.PostTags)
                 .ThenInclude(posttag => posttag.Tag);




            foreach (Post post in post_query)
            {
                posts.Add(post);
            }

            return posts;
        }

        //counts the number of posts which the current user has saved
        public int TotalPostsUserSaved(string username)
        {
            List<Post> posts = new List<Post>();
            int total = 0;
            BlogUser user = db.BlogUser.SingleOrDefault(u => u.user_name == username);
            if (user == null)
            {
                return total;
            }

            IEnumerable<Post> post_query =
                (from p in db.Posts
                 where p.Published == true &&
                 user.PostUserSaves.Any(c => c.PostId == p.PostId)
                 orderby p.PostedOn descending
                 select p);




            foreach (Post post in post_query)
            {
                total = total + 1;
            }
            return total;
        }

        public void LikePostForUser(int year, int month, string titleSlug, string user_name)
        {
            BlogUser user;
            if (!db.BlogUser.Any(c => c.user_name == user_name))
            {
                user = this.GenerateUser(user_name);
            }
            Post post_tolike = this.Post(year, month, titleSlug);
            user = db.BlogUser.Include<BlogUser, IList<PostUserLike>>(u => u.PostUserLikes).Single(c => c.user_name == user_name);

            if(user.PostUserLikes.Any(p => p.PostId == post_tolike.PostId) == false)
            {
                db.BlogUser.Update(user);
                db.Posts.Update(post_tolike);
                PostUserLike to_add = new PostUserLike();
                to_add.Post = post_tolike;
                to_add.BlogUser = user;
                user.PostUserLikes.Add(to_add);
                
                post_tolike.LikeCount = post_tolike.LikeCount + 1;
                db.SaveChanges();
            }
            
        }

        public void UnlikePostForUser(int year, int month, string titleSlug, string user_name)
        {
            BlogUser user = this.RetrieveUser(user_name);
            db.BlogUser.Update(user);
            Post post_to_unlike = this.Post(year, month, titleSlug);
            db.Posts.Update(post_to_unlike);
            PostUserLike to_remove = db.PostUserLikes.Single(pu => pu.BlogUser.user_name == user_name
            && pu.PostId == post_to_unlike.PostId);
            user.PostUserLikes.Remove(to_remove);
            db.PostUserLikes.Remove(to_remove);
            post_to_unlike.LikeCount = post_to_unlike.LikeCount - 1;
            db.SaveChanges();
        }

        public bool CheckIfLiked(Post post, string user_name)
        {
            BlogUser user = this.RetrieveUser(user_name);

            if(user.PostUserLikes.Any(p => p.PostId == post.PostId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CheckIfLikedAsync(Post post, string user_name)
        {
            bool IsLiked = await db.BlogUser.AnyAsync(u => u.user_name == user_name && u.PostUserLikes.Any(p => p.PostId == post.PostId));
            return IsLiked;
                
        }

        //initializes all relevent lists to avoid errors
        //
        public BlogUser GenerateUser(string user_name)
        {
            //checks if current user already exists -- should this throw an error or not?
            if(db.BlogUser.Any(u => u.user_name == user_name))
            {
                throw new InvalidOperationException("user already exists");
            }
            else
            {
               
                BlogUser fresh_user = new BlogUser();
                fresh_user.user_name = user_name;
                fresh_user.AuthoredPosts = new List<Post>();
                fresh_user.AuthorUserSubscribes = new List<UserAuthorSubscribe>();
                fresh_user.UserAuthorSubscribes = new List<UserAuthorSubscribe>();
                fresh_user.PostUserSaves = new List<PostUserSave>();
                fresh_user.PostUserLikes = new List<PostUserLike>();
                fresh_user.CategoryBlogUsers = new List<CategoryBlogUser>();
                fresh_user.UsersThisUserBlocks = new List<UserBlocksUser>();
                fresh_user.UsersBlockingThisUser = new List<UserBlocksUser>();
                fresh_user.UsersThisUserAuthorizes = new List<UserAuthorizesUser>();
                fresh_user.UsersAuthorizingThisUser = new List<UserAuthorizesUser>();
                db.BlogUser.Add(fresh_user);
                db.SaveChanges();
                fresh_user = db.BlogUser.Single(u => u.user_name == user_name);
                return fresh_user;
               
            }
        }

        public void BlockUser(string user_name, string user_to_block)
        {
            //checks that there is no user matching current user who already blocks target user 
            if(db.BlogUser.Any(bu => bu.user_name == user_name && 
            bu.UsersThisUserBlocks.Any(ub => ub.userBlocked.user_name == user_to_block )) == false)
            {
                BlogUser user;
                if (db.BlogUser.Any(u => u.user_name == user_name) == false)
                {
                    user = this.GenerateUser(user_name);

                }
                else
                {
                    user = db.BlogUser.Include<BlogUser, List<UserBlocksUser>>(u => u.UsersThisUserBlocks)
                        .Single(u => u.user_name == user_name);
                }
                db.BlogUser.Update(user);

                BlogUser blocked_user;
                if (db.BlogUser.Any(u => u.user_name == user_to_block) == false)
                {
                    blocked_user = this.GenerateUser(user_to_block);
                }
                else
                {
                    blocked_user = db.BlogUser.Include<BlogUser, List<UserBlocksUser>>(u => u.UsersBlockingThisUser)
                        .Single(u => u.user_name == user_to_block);
                }
                db.BlogUser.Update(blocked_user);

                UserBlocksUser blockingRelationship = new UserBlocksUser();
                blockingRelationship.blockingUserId = user.BlogUserId;
                blockingRelationship.userBlockedId = blocked_user.BlogUserId;
                blockingRelationship.blockingUser = user;
                blockingRelationship.userBlocked = blocked_user;

                user.UsersThisUserBlocks.Add(blockingRelationship);
                blocked_user.UsersBlockingThisUser.Add(blockingRelationship);
                db.UserBlocksUsers.Add(blockingRelationship);
                db.SaveChanges();

                //unsubscribes  if user  is subscribed to blocked user 
                if (db.UserAuthorSubscribes.Any(ua => ua.user.user_name == user_name && ua.author.user_name == user_to_block))
                {
                    this.UnsubscribeAuthor(user_name, user_to_block);
                }
            }
        }

        public void UnblockUser(string user_name, string user_to_unblock)
        {
            //checks current user blocks target user
            if (db.UserBlocksUsers.Any(ub => ub.blockingUser.user_name == user_name && 
            ub.userBlocked.user_name == user_to_unblock))
            {
                BlogUser user = this.RetrieveUser(user_name);
                BlogUser blockedUser = this.RetrieveUser(user_to_unblock);
                db.BlogUser.Update(user);
                db.BlogUser.Update(blockedUser);

                UserBlocksUser toUnblock = db.UserBlocksUsers.Single(ub => ub.blockingUser.user_name == user_name
                && ub.userBlocked.user_name == user_to_unblock);

                user.UsersThisUserBlocks.Remove(toUnblock);
                blockedUser.UsersBlockingThisUser.Remove(toUnblock);
                db.UserBlocksUsers.Remove(toUnblock);
                db.SaveChanges();

            }
        }

        public async Task<bool> CheckIfBlockedAsync(string user_name, string author_name)
        {
            return await db.BlogUser.AnyAsync(bu => bu.user_name == user_name
            && bu.UsersThisUserBlocks.Any(ua => ua.userBlocked.user_name == author_name));
        }

        public void SubscribeAuthor(string user_name, string author_name)
        {

            if(db.BlogUser.Any(bu => bu.user_name == user_name 
            && bu.UserAuthorSubscribes.Any(ua => ua.author.user_name == author_name)) == false)
            {
                BlogUser user;
                if (db.BlogUser.Any(u => u.user_name == user_name) == false)
                {
                     user = this.GenerateUser(user_name);
                    
                }
             else
                {
                     user = db.BlogUser.Include<BlogUser, List<UserAuthorSubscribe>>(u => u.UserAuthorSubscribes)
                        .Single(u => u.user_name == user_name);
                    
                    
                }
                db.BlogUser.Update(user);

                BlogUser author;
              if (db.BlogUser.Any(u => u.user_name == author_name))
                {
                    author = db.BlogUser.Include<BlogUser, List<UserAuthorSubscribe>>(u => u.AuthorUserSubscribes)
                                        .Single(u => u.user_name == author_name);
                }
              else
                {
                    
                    throw new InvalidOperationException("Author not found");
                }
                db.BlogUser.Update(author);

                UserAuthorSubscribe nsubscription = new UserAuthorSubscribe();
                nsubscription.authorId = author.BlogUserId;
                nsubscription.userId = user.BlogUserId;
                nsubscription.author = author;
                nsubscription.user = user;
                
               
                user.UserAuthorSubscribes.Add(nsubscription);
                author.AuthorUserSubscribes.Add(nsubscription);
                author.subscribers_count = author.subscribers_count + 1;
                db.UserAuthorSubscribes.Add(nsubscription);
                db.SaveChanges();
                
            }

            /*
            BlogUser user = this.RetrieveUser(user_name);
            BlogUser author = this.RetrieveUser(author_name);
           
            if(user.UserAuthorSubscribes.Any(c => c.author == author) == false)
            {
                db.Update(user);
                db.Update(author);
                user.SubscribedAuthors.Add(author);
                db.SaveChanges();
            } */
        }
        public void UnsubscribeAuthor(string user_name, string author_name)
        {
           
            if(db.UserAuthorSubscribes.Any(ua => ua.user.user_name == user_name && ua.author.user_name == author_name))
            {
                BlogUser user = this.RetrieveUser(user_name);
                BlogUser author = this.RetrieveUser(author_name);
                UserAuthorSubscribe to_remove = db.UserAuthorSubscribes.Single(ua => ua.user.user_name == user_name
                && ua.author.user_name == author_name);
                db.Update(user);
                db.Update(author);
                user.UserAuthorSubscribes.Remove(to_remove);
                author.AuthorUserSubscribes.Remove(to_remove);
                author.subscribers_count = author.subscribers_count - 1;
                db.UserAuthorSubscribes.Remove(to_remove);
                db.SaveChanges();
            }
            
        }

        public bool CheckIfSubscribed(string user_name, string author_name)
        {
           if (db.UserAuthorSubscribes.Any(u => u.user.user_name == user_name && u.author.user_name == author_name))
            {
                return true;
            }
           else
            {
                return false;
            }


        }

        public async Task<bool> CheckIfSubscribedAsync(string user_name, string author_name)
        {
            bool IsSubscribed = await db.UserAuthorSubscribes.AnyAsync(ua => ua.user.user_name == user_name
            && ua.author.user_name == author_name);
            
            return IsSubscribed;
        }

        public BlogUser RetrieveUser(string username)
        {
            //user doesnt exist; generate a new user to avoid errors
            if(db.BlogUser.Any(u => u.user_name == username) == false)
            {
                return this.GenerateUser(username);
            }

            BlogUser user = db.BlogUser
             .Include<BlogUser, List<UserAuthorSubscribe>>(c => c.UserAuthorSubscribes)
             .Include<BlogUser, List<PostUserSave>>(u => u.PostUserSaves)
             .Include<BlogUser, List<PostUserLike>>(u => u.PostUserLikes)
             .Include<BlogUser, List<UserBlocksUser>>(u => u.UsersThisUserBlocks)
             .Include<BlogUser, List<UserBlocksUser>>(u => u.UsersBlockingThisUser)
             .SingleOrDefault(u => u.user_name == username);
            return user;
        }

        public IList<BlogUser> AllAuthors()
        {
            List<BlogUser> authors = new List<BlogUser>();
            IEnumerable<BlogUser> u_query =
                (from u in db.BlogUser
                 where u.AuthoredPosts.Any(p => p.Published == true)
                 orderby u.user_name
                 select u)
                 .Include<BlogUser, List<UserAuthorSubscribe>>(u => u.UserAuthorSubscribes)
                 .Include<BlogUser, List<UserAuthorSubscribe>>(u => u.AuthorUserSubscribes)
                 .Include<BlogUser, List<PostUserSave>>(u => u.PostUserSaves);

            foreach (BlogUser author in u_query)
            {
                authors.Add(author);
            }
            return authors;
        }

       public IDictionary<string, string> AuthorPostCounts()
        {
            int count;
            IList<BlogUser> AllAuthors = this.AllAuthors();
            Dictionary<string, string> AuthorCounts = new Dictionary<string, string>();
            foreach (BlogUser author in AllAuthors)
            {
                count = this.TotalPostsByAuthor(author.user_name);
                AuthorCounts[author.user_name] = String.Format("({0} posts)", count);

            }
            return AuthorCounts;
        }

        public IDictionary<string, string> AuthorPostCounts(IList<BlogUser> AllAuthors)
        {
            int count;
            Dictionary<string, string> AuthorCounts = new Dictionary<string, string>();
            foreach (BlogUser author in AllAuthors)
            {
                count = this.TotalPostsByAuthor(author.user_name);
                AuthorCounts[author.user_name] = String.Format("({0} posts)", count);

            }
            return AuthorCounts;
        }

        public IList<Post> PostsByLikesPerDay(int pageNo, int pageSize)
        {
            List<Post> posts = new List<Post>();

            IEnumerable<Post> p_query =
                (from p in db.Posts
                 where p.Published == true &&
                 p.LikeCount > 0
                 orderby p.LikesPerDay() descending
                 select p).Skip(pageNo * pageSize).Take(pageSize)
                 .Include<Post, BlogUser>(p => p.Author)
                 .Include<Post, Category>(p => p.Category)
                 .Include(p => p.PostTags)
                 .ThenInclude(posttag => posttag.Tag);

            foreach (Post post in p_query)
            {
                posts.Add(post);
            }
            return posts;
        }
        
        public int TotalPostsByLikesPerDay()
        {
            int count = 0;
            IEnumerable<Post> p_query =
                (from p in db.Posts
                 where p.Published == true &&
                 p.LikeCount > 0
                 orderby p.LikesPerDay() descending
                 select p);

            foreach (Post post in p_query)
            {
                count = count + 1;
            }
            return count;
        }

        public IList<Post> PostsByAuthor(string user_name, int pageNo, int pageSize)
        {
            List<Post> return_posts = new List<Post>();
            IEnumerable<Post> p_query = 
                (from p in db.Posts
                where p.Published == true &&
                p.Author.user_name == user_name
                orderby p.PostedOn descending
                select p).Skip(pageNo * pageSize).Take(pageSize)
                .Include<Post, BlogUser>(p => p.Author)
                .Include<Post, Category>(p => p.Category)
                  .Include(p => p.PostTags)
                 .ThenInclude(posttag => posttag.Tag);

            foreach(Post post in p_query)
            {
                return_posts.Add(post);
            }
            return return_posts;
        }

        public int TotalPostsByAuthor(string user_name)
        {
            int total = 0;
            IEnumerable<Post> p_query =
                (from p in db.Posts
                 where p.Published == true &&
                 p.Author.user_name == user_name
                 select p);

            foreach(Post post in p_query)
            {
                total = total + 1;
            }
            return total;
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
                                  .Include<Post, BlogUser>(p => p.Author)
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
        public Post IncrementViews(Post post)
        {
            db.Posts.Update(post);
            post.ViewCount = post.ViewCount + 1;
            db.SaveChanges();
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

        //used to generate string representing current number of posts in a certain category for display 
        //format of dictionary for return: key is category.UrlSlug, entry is "(x posts)" 
        //this version generates its own copy of AllCategories, meaning it queries the databse once per call 
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

        //used to generate string representing current number of posts in a certain category for display 
        //format of dictionary for return: key is category.UrlSlug, entry is "(x posts)" 
        //this version takes a premade list of all categories as an argument, meaning it avoids a redundant database query
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
