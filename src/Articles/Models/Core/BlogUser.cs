using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.Core
{
    public class BlogUser
    {
        public int BlogUserId { get; set; }
        public string user_name { get; set; }
        public int page_size { get; set; } = 10;
        public int subscribers_count { get; set; } = 0;
        public bool publicMessaging { get; set; } = false;

        public List<CategoryBlogUser> CategoryBlogUsers { get; set; }
        // public List<Post> BlogUserPosts { get; set; }
        public List<Post> AuthoredPosts { get; set; }
        // public List<Post> LikedPosts { get; set; }
        public List<BlogUser> SubscribedAuthors { get; set; }
        public List<PostUserSave> PostUserSaves { get; set; }
        public List<PostUserLike> PostUserLikes { get; set; }
        //junction table from user --> author direction
        public List<UserAuthorSubscribe> UserAuthorSubscribes { get; set; }
        //junction table from author --> user direction
        public List<UserAuthorSubscribe> AuthorUserSubscribes { get; set; }

        //user -- > user to block direction 
        public List<UserBlocksUser> UsersThisUserBlocks { get; set; }

        // blocked user --> blocking user direction
        public List<UserBlocksUser> UsersBlockingThisUser { get; set; }

        //authorizing user --> authorized user direction
        public List<UserAuthorizesUser> UsersThisUserAuthorizes { get; set; }

        //authorized user --> authorizing user direction
        public List<UserAuthorizesUser> UsersAuthorizingThisUser { get; set; }

        public List<Message> ReceivedMessages { get; set; }
        public List<Message> SentMessages { get; set; }


        public string AuthorSubscribeAjaxId()
        {
            return BlogUserId.ToString() + "_sub";
        }
        public string AuthorSubscribeCount()
        {
            return this.AuthorUserSubscribes.Count.ToString();
        }

    }
}
