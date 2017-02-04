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
        public List<UserAuthorSubscribe> UsersThisUserSubscribesTo { get; set; }
        //junction table from author --> user direction
        public List<UserAuthorSubscribe> UsersSubscribingToThisUser { get; set; }

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
            return this.UsersSubscribingToThisUser.Count.ToString();
        }

        public UserBlocksUser blockUser(BlogUser blocked_user)
        {
            UserBlocksUser blockingRelationship = new UserBlocksUser();
            blockingRelationship.blockingUserId = this.BlogUserId;
            blockingRelationship.userBlockedId = blocked_user.BlogUserId;
            blockingRelationship.blockingUser = this;
            blockingRelationship.userBlocked = blocked_user;

            this.UsersThisUserBlocks.Add(blockingRelationship);
            blocked_user.UsersBlockingThisUser.Add(blockingRelationship);
            return blockingRelationship;
        }

        public UserAuthorizesUser authorizeUser(BlogUser authorizedUser)
        {
            UserAuthorizesUser authorizationRelationship = new UserAuthorizesUser();
            authorizationRelationship.authorizingUser = this;
            authorizationRelationship.authorizingUserId = this.BlogUserId;

            authorizationRelationship.userAuthorized = authorizedUser;
            authorizationRelationship.userAuthorizedId = authorizedUser.BlogUserId;

            this.UsersThisUserAuthorizes.Add(authorizationRelationship);
            authorizedUser.UsersAuthorizingThisUser.Add(authorizationRelationship);
            return authorizationRelationship;
        }

        public UserAuthorSubscribe subscribeUser(BlogUser subscribedUser)
        {
            UserAuthorSubscribe subscriptionRelationship = new UserAuthorSubscribe();
            subscriptionRelationship.subscribingUser = this;
            subscriptionRelationship.subscribingUserId = this.BlogUserId;

            subscriptionRelationship.userSubscribed = subscribedUser;
            subscriptionRelationship.userSubscribedId = subscribedUser.BlogUserId;

            this.UsersThisUserSubscribesTo.Add(subscriptionRelationship);
            subscribedUser.UsersSubscribingToThisUser.Add(subscriptionRelationship);

            return subscriptionRelationship;
        }

    }
}
