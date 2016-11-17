using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Articles.Models;
using Articles.Models.BlogViewModels;
using Articles.Models.Core;

namespace Articles.Models
{
   public interface IBlogRepository
    {

        BlogUser GenerateUser(string user_name);
        
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

        IList<Post> CustomPostsForUser(string user_name, int pageNo, int pageSize);
        int TotalCustomPostsForUser(string user_name);
        void UpdateCustomization(CustomizeViewModel viewModel, string user_name);
        int UserPageSize(string user_name);

        void SavePostForUser(int year, int month, string titleSlug, string user_name);
        void UnsavePostForUser(int year, int month, string titleSlug, string user_name);
        bool CheckIfSaved(Post post, string username);
        Task<bool> CheckIfSavedAsync(Post post, string user_name);
        IList<Post> PostsUserSaved(string username, int pageNo, int pageSize);

        void LikePostForUser(int year, int month, string titleSlug, string user_name);
        Task<bool> CheckIfLikedAsync(Post post, string user_name); 
        void UnlikePostForUser(int year, int month, string titleSlug, string user_name);
        bool CheckIfLiked(Post post, string user_name);

        void SubscribeAuthor(string user_name, string author_name);
        void UnsubscribeAuthor(string user_name, string author_name);
        bool CheckIfSubscribed(string user_name, string author_name);
        Task<bool> CheckIfSubscribedAsync(string user_name, string author_name);
        //bool CheckIfSubscribed(string user_name, BlogUser author);

        IList<Post> SubscribedPostsForUser(string user_name, int pageNo, int pageSize);
        int TotalSubscribedPostsForUser(string user_name);
        int TotalPostsUserSaved(string username);

        BlogUser RetrieveUser(string username);
        IList<BlogUser> AllAuthors();

        IList<Post> PostsByLikesPerDay(int pageNo, int pageSize);
        int TotalPostsByLikesPerDay();

        IList<Post> PostsByAuthor(string user_name, int pageNo, int pageSize);
        int TotalPostsByAuthor(string user_name);

        //returns a post for full display identified via year/month posted and title slug 
        Post Post(int year, int month, string titleSlug);
        Post IncrementViews(Post post);

        //returns all categories or tags -- this is used for widget
        IList<Category> Categories();
        IList<Tag> Tags();

        //returns # of posts in each category for display in category links 
        IDictionary<string, string> CategoryCounts();
        //overloaded version which takes list ofall categories to avoid redundant database queries 
        IDictionary<string, string> CategoryCounts(IList<Category> AllCategories);

        IDictionary<string, string> AuthorPostCounts();
        IDictionary<string, string> AuthorPostCounts(IList<BlogUser> AllAuthors);

        IList<Post> Posts(int pageNo, int pageSize, string sortColumn, bool sortByAscending);
    }
}
