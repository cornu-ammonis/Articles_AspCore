using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.BlogViewModels.ListViewModels
{
    public class SubscribedListViewModel : ListViewModel
    {
        public SubscribedListViewModel(IBlogRepository blogRepository, int p, string user_name)
            : base(blogRepository, user_name)
            //base constructur retrieves user's custom page size or defaults it to 10
        {
            //passes list of subscribed posts and total number of subscribed posts to base method for 
            //value assignation
            PopulatePostFields(blogRepository.SubscribedPostsForUser(user_name, p - 1, PageSize),
                blogRepository.TotalSubscribedPostsForUser(user_name));
        }
    }
}
