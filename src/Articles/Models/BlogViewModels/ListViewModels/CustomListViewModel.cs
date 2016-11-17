using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.BlogViewModels.ListViewModels
{
    public class CustomListViewModel : ListViewModel
    {
        public CustomListViewModel(IBlogRepository blogRepository, int p, string user_name) 
            : base(blogRepository, user_name)
        {
            PopulatePostFields(blogRepository.CustomPostsForUser(user_name, p - 1, PageSize), 
                blogRepository.TotalCustomPostsForUser(user_name));
        }
    }
}
