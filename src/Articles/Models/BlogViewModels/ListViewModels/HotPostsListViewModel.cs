using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.BlogViewModels.ListViewModels
{
    public class HotPostsListViewModel : ListViewModel
    {
        public HotPostsListViewModel(IBlogRepository blogRepository, int p, string user_name = null)
            : base (blogRepository, user_name)
        {
            PopulatePostFields(blogRepository.PostsByLikesPerDay(p - 1, PageSize),
                blogRepository.TotalPostsByLikesPerDay());
        }
    }
}
