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
        public CustomizeViewModel(IBlogRepository _blogRepository, string user_name)
        {
            user_page_size = _blogRepository.UserPageSize(user_name);
            IList<Category> all_categories = _blogRepository.Categories();

            category_counts = _blogRepository.CategoryCounts(all_categories);

            categories = new Dictionary<string, bool>();
            
            //used for converting url slug to name in view 
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
        }

       

        public IDictionary<string, bool> categories { get; set; }
        public IDictionary<string, string> category_counts { get; set; }
        public int user_page_size { get; set; }
        //used for converting from url slug to name in view 
        public IDictionary<string, string> category_names { get; set; }
    }
}
