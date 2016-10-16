using Articles.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.BlogViewModels
{
    public class CustomizeViewModel
    {
        public CustomizeViewModel()
        {
            categories = new Dictionary<string, bool>();
        }

        //used by the customize action to create a page of current settings and bind currend settings for httppost. 
        // identifies user by User.Identity.Name, the string username 
        public CustomizeViewModel(IBlogRepository _blogRepository, string user_name)
        {
            user_page_size = _blogRepository.UserPageSize(user_name);
            IList<Category> all_categories = _blogRepository.Categories();

            //generates <string, string> dictionary where the key is each category's category.UrlSlug and the entry is 
            // "(x posts)" where x is the number of posts in that category
            category_counts = _blogRepository.CategoryCounts(all_categories);

            //used for representing result of exploring junction table as boolean value 
            categories = new Dictionary<string, bool>();
            
            //used for converting url slug to name in view ***
            category_names = new Dictionary<string, string>();

            foreach (Category category in all_categories)
            {
                //checks if the category is linked to the current user via junction table; otherwise assigns false
                if (category.CategoryBlogUsers.Any(c => c.BlogUser.user_name == user_name))
                {
                    categories[category.UrlSlug] = true;
                }
                else
                {
                    categories[category.UrlSlug] = false;
                }

                /* this was moved to the blog repository to avoid duplication 
                //gets total number of posts for each category and creates string for display in view
                int count = _blogRepository.TotalPostsForCategory(category.UrlSlug);
                string counter = String.Format("({0} posts in this category)", count);
                category_counts[category.UrlSlug] = counter;
                */
                category_names[category.UrlSlug] = category.Name; 
            }

            IList<BlogUser> all_authors = _blogRepository.AllAuthors();
            BlogUser current_user = _blogRepository.RetrieveUser(user_name);
            subscribed_authors = new Dictionary<string, bool>();
            author_counts = _blogRepository.AuthorPostCounts(all_authors);
            foreach (BlogUser author in all_authors)
            {
                if(current_user.SubscribedAuthors.Any(c => c.user_name == author.user_name))
                {
                    subscribed_authors[author.user_name] = true;
                }
                else
                {
                    subscribed_authors[author.user_name] = false;
                }
            }
        }

       

        public IDictionary<string, bool> categories { get; set; }
        public IDictionary<string, string> category_counts { get; set; }
        public int user_page_size { get; set; }
        //used for converting from url slug to name in view 
        public IDictionary<string, string> category_names { get; set; }
        public IDictionary<string, bool> subscribed_authors { get; set; }
        public IDictionary<string, string> author_counts { get; set; }
    }
}
