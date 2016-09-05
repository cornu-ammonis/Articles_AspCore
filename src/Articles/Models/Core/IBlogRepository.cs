using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Articles.Models;
using Articles.Models.BlogViewModels;

namespace Articles.Models
{
   public interface IBlogRepository
    {
        //returns posts ordered by PostedOn descending and selected by pagination values
        IList<Post> Posts(int pageNo, int pageSize);
        int TotalPosts( bool checkIsPublished = true);

        //returns posts within a category specified by slug, ordered by PostedOn descending, skip/take by pagination values
        IList<Post> PostsForCategory(string categorySlug, int pageNo, int pageSize);
        int TotalPostsForCategory(string categorySlug);
        Category Category(string categorySlug);

        //returns posts with tag specified by slug, ordered by PostedOn descending, skip/take by pagination values
        IList<Post> PostsForTag(string tagSlug, int pageNo, int pageSize);
        int TotalPostsForTag(string tagSlug);
        Tag Tag(string tagSlug);

        //returns posts with title containing or category/tag matching a given search string - ordered by PostedOn descending, skip/take by pagination values
        IList<Post> PostsForSearch(string search, int pageNo, int pageSize);
        int TotalPostsForSearch(string search);

        IList<Post> PostsForUser(string user_name, int pageNo, int pageSize);
        int TotalPostsForUser(string user_name);
        void UpdateCustomization(CustomizeViewModel viewModel, string user_name);
        int UserPageSize(string user_name);

        //returns a post for full display identified via year/month posted and title slug 
        Post Post(int year, int month, string titleSlug);

        //returns all categories or tags -- this is used for widget
        IList<Category> Categories();
        IList<Tag> Tags();

        IList<Post> Posts(int pageNo, int pageSize, string sortColumn, bool sortByAscending);
    }
}
