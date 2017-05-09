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

        void UnpublishPost(int postId);
        void PublishPost(int postId);
    }
}
