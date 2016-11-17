using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.BlogViewModels.ListViewModels
{
    public class SearchListViewModel : ListViewModel
    {
        public SearchListViewModel(IBlogRepository blogRepository, string search, int p, string user_name = null)
            : base (blogRepository, user_name)
        {
            PopulatePostFields(blogRepository.PostsForSearch(search, p - 1, PageSize),
                blogRepository.TotalPostsForSearch(search));

            PopulateSearch(search);
        }
    }
}
