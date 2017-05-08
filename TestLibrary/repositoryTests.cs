using Articles.Data;
using Articles.Models;
using Articles.Models.Core;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestLibrary
{

   /* [TestFixture]
    public class repositoryTests
    {
        [TestCase]
        public void RepositoryCreatesBlockingRelationship()
        {
            BlogUser user = new BlogUser();
            user.user_name = "blocking user";
            //user.UsersThisUserBlocks = new List<UserBlocksUser>();

            BlogUser blockedUser = new BlogUser();
            blockedUser.user_name = "blockd user";
            // user.UsersBlockingThisUser = new List<UserBlocksUser>();

            //user.UsersThisUserBlocks.

            var mockDb = new Mock<ApplicationDbContext>();
            IBlogRepository repo = new BlogRepository(mockDb.Object);
            repo.BlockUser(user.user_name, blockedUser.user_name);

        }

    }*/
}
