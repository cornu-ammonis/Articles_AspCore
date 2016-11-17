using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.BlogViewModels.ListViewModels
{
    public class AuthorListViewModel : ListViewModel
    {
        public AuthorListViewModel(IBlogRepository blogRepository, string authorname, int p, string user_name)
            : base (blogRepository, user_name)
        {
            PopulatePostFields(blogRepository.PostsByAuthor(authorname, p - 1, PageSize),
                blogRepository.TotalPostsByAuthor(authorname));
        }
    }
}
