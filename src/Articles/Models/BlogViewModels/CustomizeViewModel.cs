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
            IList<Category> all_categories = _blogRepository.Categories();
            categories = new Dictionary<string, bool>();

            foreach (Category category in all_categories)
            {
                if(category.CategoryBlogUsers.Any(c => c.BlogUser.user_name == user_name))
                {
                    categories[category.Name] = true;
                }
                else
                {
                    categories[category.Name] = false;
                }
            }
        }

       

        public IDictionary<string, bool> categories { get; set; }
        public int user_page_size { get; set; }
    }
}
