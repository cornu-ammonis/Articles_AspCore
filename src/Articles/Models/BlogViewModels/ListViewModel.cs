using Articles.Models;
using Articles.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Articles.Models
{
    public class ListViewModel
    {
        protected ListViewModel(IBlogRepository blogRepository,
            string user_name = null)
        {
            PageSize = 10;
            if(user_name != null)
            {
                PageSize = blogRepository.UserPageSize(user_name);

                //avoids error where page size gets set to zero and various SQL fetch errors are generated and 
                // pagination says there are  infinite pages
                if (PageSize < 1)
                {
                    PageSize = 10;
                }
            }
        }

        protected virtual void PopulatePostFields(IList<Post> _posts, int _totalPosts)
        {
            Posts = _posts;
            TotalPosts = _totalPosts;
        }

        protected virtual void PopulateTag(Tag _tag)
        {
            Tag = _tag;
        }

        protected virtual void PopulateCategory(Category _category)
        {
            Category = _category;
        }

        protected virtual void PopulateSearch(string _search)
        {
            Search = _search;
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
                case "Author":
                    Posts = blogRepository.PostsByAuthor(text, p - 1, PageSize);
                    TotalPosts = blogRepository.TotalPostsByAuthor(text);
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