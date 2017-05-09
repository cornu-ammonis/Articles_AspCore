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

        void UnpublishPost(int postId);
        void PublishPost(int postId);
    }
}
