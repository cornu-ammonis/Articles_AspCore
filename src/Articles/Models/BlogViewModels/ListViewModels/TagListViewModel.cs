using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.BlogViewModels.ListViewModels
{
    public class TagListViewModel : ListViewModel
    {
        public TagListViewModel(IBlogRepository blogRepository, string tagSlug, int p, string user_name = null)
            : base(blogRepository, user_name)
        {
            //passes list of posts under selected tag and total number of posts under thtat tag
            PopulatePostFields(blogRepository.PostsForTag(tagSlug, p - 1, PageSize), 
                blogRepository.TotalPostsForTag(tagSlug));

            PopulateTag(blogRepository.Tag(tagSlug));
        }
    }
}
