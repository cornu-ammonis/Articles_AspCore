using Articles.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models
{
    public interface IAdminRepository
    {
        IList<Post> ListAllPosts();
        IList<Post> ListAllPostsDescendingDate();
        IList<Post> ListAllPostsAscendingDate();
        IList<Post> ListAllPostsDescendingAuthorName();
        IList<Post> ListAllPostsAscendingAuthorName();
        IList<Post> ListAllPostsDescendingTitle();
        IList<Post> ListAllPostsAscendingTitle();
        IList<Post> ListAllPostsDescendingCategory();
        IList<Post> ListAllPostsAscendingCategory();

        IList<Post> ListPostsForSearch(string search);

        IList<Category> ListCategoriesForSearch(string search);

        IList<Category> ListAllCategories();

        void AddNewCategoryToDatabase(Category category);

        void UnpublishPost(int postId);
        void PublishPost(int postId);

        // returns post specified by postId, must include relevant navigation properties by default
        Post RetrievePostById(int postId);


        // returns a list of blog users sorted alphabetically ascending. 
        // includes list of their posts and those who subscribe to them for display in 
        // console.
        IList<BlogUser> ListUsersAlphabetically();

        // returns a list of blog users sorted alphabetically 
        // where the user name matches the search string in some way.
        // must include list of their posts and who subscribes to them for display 
        // in admin console
        IList<BlogUser> ListUsersForSearch(string search);

        void BanUser(string username);
        void UnbanUser(string username);

        Task MakeAdminAsync(string username);
    }
}
