using Articles.Models;
using Articles.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Articles.Models
{
    public class ListViewModel
    {
      /*  public ListViewModel(IBlogRepository _blogRepository, int p)
        {
            Posts = _blogRepository.Posts(p - 1, 10);
            TotalPosts = _blogRepository.TotalPosts();
        }

      /*  public ListViewModel(IBlogRepository blogRepository, string categorySlug, int p)
        {
            Posts = blogRepository.PostsForCategory(categorySlug, p - 1, 10);
            TotalPosts = blogRepository.TotalPostsForCategory(categorySlug);
            Category = blogRepository.Category(categorySlug);
        } */

            //constructor used for two types of list view generation -- all posts, and custom posts. string "type" determines which.
            // if user name exists, attempts to return their customized page size, otherwise defaults to 10.
        public ListViewModel(IBlogRepository blogRepository, int p, string type, string user_name = null)
        {
            PageSize = 10;
            if (user_name != null)
            {
                PageSize = blogRepository.UserPageSize(user_name);

                //avoids error where page size gets set to zero and various SQL fetch errors are generated and 
                // pagination says there are  infinite pages
                if (PageSize < 1)
                {
                    PageSize = 10;
                }
            }
            switch(type)
            {
                case "Custom":
                    Posts = blogRepository.PostsForUser(user_name, p - 1, PageSize);
                    TotalPosts = blogRepository.TotalPostsForUser(user_name);
                    break;
                case "All":
                    Posts = blogRepository.Posts(p - 1, PageSize);
                    TotalPosts = blogRepository.TotalPosts();
                    break;
                case "Saved":
                    Posts = blogRepository.PostsUserSaved(user_name, p - 1, PageSize);
                    TotalPosts = blogRepository.TotalPostsUserSaved(user_name);
                    break;
            }
           
            //this code creates a dictionary of <post.urlslug, bool> representing whether each post 
            // is saved by the current user. 
           if (user_name != null)
            {
                SaveUnsave = true;
                BlogUser user = blogRepository.RetrieveUser(user_name);
                IsSaved = new Dictionary<string, bool>();
                foreach (Post post in Posts)
                {
                    if (user.BlogUserPosts.Any(c => c.PostId == post.PostId))
                    {
                        IsSaved[post.UrlSlug] = true;
                    }
                    else
                    {
                        IsSaved[post.UrlSlug] = false;
                    }
                }
            }
           else
            {
                SaveUnsave = false;
            }
        }


        //used for generating a list of posts either by tag, category, or search term, the instance of which is specified by 
        // string "text" and which case specified by string "type" 
        public ListViewModel(IBlogRepository blogRepository, string text, string type, int p, string user_name = null)
        {
            PageSize = 10;
            if (user_name != null)
            {
               
                PageSize = blogRepository.UserPageSize(user_name);

                if (PageSize < 1)
                {
                    PageSize = 10;
                }
            }

            switch (type)
            {
                case "Tag":
                    Posts = blogRepository.PostsForTag(text, p - 1, PageSize);
                    TotalPosts = blogRepository.TotalPostsForTag(text);
                    Tag = blogRepository.Tag(text);
                    break;
                case "Category":
                    Posts = blogRepository.PostsForCategory(text, p - 1, PageSize);
                    TotalPosts = blogRepository.TotalPostsForCategory(text);
                    Category = blogRepository.Category(text);
                    break;
                case "Search":
                    Posts = blogRepository.PostsForSearch(text, p - 1, PageSize);
                    TotalPosts = blogRepository.TotalPostsForSearch(text);
                    Search = text;
                break;
               
            }

            if (user_name != null)
            {
                SaveUnsave = true;
                BlogUser user = blogRepository.RetrieveUser(user_name);
                IsSaved = new Dictionary<string, bool>();
                foreach (Post post in Posts)
                {
                    if (user.BlogUserPosts.Any(c => c.PostId == post.PostId))
                    {
                        IsSaved[post.UrlSlug] = true;
                    }
                    else
                    {
                        IsSaved[post.UrlSlug] = false;
                    }
                }
            }
            else
            {
                SaveUnsave = false;
            }
        }


        public IList<Post>  Posts { get; private set; }
        public int TotalPosts { get; private set; }
        public int PageSize { get; private set; } = 10;
        public Category Category { get; private set; }
        public Tag Tag { get; private set; }
        public string Search { get; private set; }
        public IDictionary<string, bool> IsSaved { get; private set; }
        public bool SaveUnsave { get; private set; }


    }
}