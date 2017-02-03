using Articles.Models;
using Articles.Models.Core;
using Articles.Models.MessageViewModels;
using Moq;
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

        [TestCase]
        public void MessageCreationAssignsViewModelPropertiesAsExpected()
        {
            BlogUser sender = new BlogUser();
            sender.user_name = "thisOneSends";

            BlogUser receiver = new BlogUser();
            receiver.user_name = "thisOneReceives";

            MessageCreationViewModel viewModel = new MessageCreationViewModel();
            viewModel.AuthorName = sender.user_name;
            viewModel.RecipientName = receiver.user_name;
            viewModel.Contents = "test contents for message creation";
            viewModel.Subject = "test subject for message creation";

            var mockRepository = new Mock<IMessageRepository>();
            mockRepository.Setup(r => r.RetrieveUserForMessaging(sender.user_name)).Returns(sender);
            mockRepository.Setup(r => r.RetrieveUserForMessaging(receiver.user_name)).Returns(receiver);

            Message testMessage = new Message(viewModel, mockRepository.Object);

            Assert.That(testMessage.Contents, Is.EqualTo(viewModel.Contents));
            Assert.That(testMessage.Subject, Is.EqualTo(viewModel.Subject));


        }

        [TestCase]
        public void MessageCreationRetrievesSenderAndRecipientFromRepository()
        {

            BlogUser sender = new BlogUser();
            sender.user_name = "thisOneSends";

            BlogUser receiver = new BlogUser();
            receiver.user_name = "thisOneReceives";

            MessageCreationViewModel viewModel = new MessageCreationViewModel();
            viewModel.AuthorName = sender.user_name;
            viewModel.RecipientName = receiver.user_name;
            viewModel.Contents = "test contents for message creation";
            viewModel.Subject = "test subject for message creation";

            var mockRepository = new Mock<IMessageRepository>();
            mockRepository.Setup(r => r.RetrieveUserForMessaging(sender.user_name)).Returns(sender);
            mockRepository.Setup(r => r.RetrieveUserForMessaging(receiver.user_name)).Returns(receiver);

            Message testMessage = new Message(viewModel, mockRepository.Object);

            Assert.That(testMessage.Sender, Is.EqualTo(sender));
            Assert.That(testMessage.Recipient, Is.EqualTo(receiver));
        }

    }
}
