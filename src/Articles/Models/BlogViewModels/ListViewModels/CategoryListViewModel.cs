using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.BlogViewModels.ListViewModels
{
    public class CategoryListViewModel : ListViewModel
    {
        public CategoryListViewModel(IBlogRepository blogRepository, string catSlug, int p, string user_name = null)
            : base(blogRepository, user_name)
        {
            PopulatePostFields(blogRepository.PostsForCategory(catSlug, p - 1, PageSize),
                blogRepository.TotalPostsForCategory(catSlug));

            PopulateCategory(blogRepository.Category(catSlug));
        }
    }
}
