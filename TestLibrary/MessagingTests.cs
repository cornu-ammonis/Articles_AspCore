using Articles.Models.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestLibrary
{

    [TestFixture]
    public class MessagingTests
    {

        [TestCase]
        public void BlogUserBlockUserMethodReturnsCorrectBlockingRelationship()
        {
            BlogUser blockingUser = new BlogUser();
            blockingUser.user_name = "thisUserDoesTheBlocking";
            blockingUser.UsersThisUserBlocks = new List<UserBlocksUser>();

            BlogUser blockedUser = new BlogUser();
            blockedUser.user_name = "thisUserGetsBlocked";
            blockedUser.UsersBlockingThisUser = new List<UserBlocksUser>();

            UserBlocksUser result = blockingUser.blockUser(blockedUser);

            Assert.That(result.blockingUser, Is.EqualTo(blockingUser));
            Assert.That(result.userBlocked, Is.EqualTo(blockedUser));
            Assert.That(blockingUser.UsersThisUserBlocks, Is.EquivalentTo(blockedUser.UsersBlockingThisUser));

        }

        [TestCase]
        public void BlogUserAuthorizeUserMethodReturnsCorrectAuthorizationRelationship()
        {
            BlogUser authorizingUser = new BlogUser();
            authorizingUser.user_name = "thisUserDoesTheAuthorizing";
            authorizingUser.UsersThisUserAuthorizes = new List<UserAuthorizesUser>();

            BlogUser authorizedUser = new BlogUser();
            authorizedUser.user_name = "thisUserGetsAuthorized";
            authorizedUser.UsersAuthorizingThisUser = new List<UserAuthorizesUser>();

            UserAuthorizesUser result = authorizingUser.authorizeUser(authorizedUser);

            Assert.That(result.authorizingUser, Is.EqualTo(authorizingUser));
            Assert.That(result.userAuthorized, Is.EqualTo(authorizedUser));
            Assert.That(authorizingUser.UsersThisUserAuthorizes, Is.EquivalentTo(authorizedUser.UsersAuthorizingThisUser));
        }

    }
}
