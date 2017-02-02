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

    }
}
