using Articles.Models;
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

        public ListViewModel(IBlogRepository blogRepository, int p, string type, string user_name = null)
        {
            PageSize = 10;
            if (user_name != null)
            {
                PageSize = blogRepository.UserPageSize(user_name);
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
            }
            
           

        }

        public ListViewModel(IBlogRepository blogRepository, string text, string type, int p, string user_name = null)
        {
            PageSize = 10;
            if (user_name != null)
            {
                PageSize = blogRepository.UserPageSize(user_name);
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
        }


        public IList<Post>  Posts { get; private set; }
        public int TotalPosts { get; private set; }
        public int PageSize { get; private set; } = 10;
        public Category Category { get; private set; }
        public Tag Tag { get; private set; }
        public string Search { get; private set; }


    }
}