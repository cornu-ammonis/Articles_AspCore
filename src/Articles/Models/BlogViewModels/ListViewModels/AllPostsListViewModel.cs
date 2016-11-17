using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.BlogViewModels.ListViewModels
{
    public class AllPostsListViewModel : ListViewModel
    {
        public AllPostsListViewModel(IBlogRepository blogRepository, int p, string user_name = null)
            : base(blogRepository, user_name)
        {
            PopulatePostFields(blogRepository.Posts(p - 1, PageSize), blogRepository.TotalPosts());
        }
    }
}
