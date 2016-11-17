using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.BlogViewModels.ListViewModels
{
    public class SavedListViewModel : ListViewModel
    {

        public SavedListViewModel(IBlogRepository blogRepository, int p, string user_name)
            : base(blogRepository, user_name) 
            // base constructor either retrieves user's PageSize or defaults to 10
        {
            //passes list of [PageSize] length saved posts and total # of saved posts for this user 
            PopulatePostFields(blogRepository.PostsUserSaved(user_name, p - 1, PageSize),
                blogRepository.TotalPostsUserSaved(user_name));
        }

        
    }
}
