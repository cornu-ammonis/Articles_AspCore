using Articles.Controllers;
using Articles.Models;
using Articles.Models.Core;
using Articles.Models.MessageViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [TestCase]
        public void AllMessageListViewModelCallsAllMessagesRepositoryMethodAndPopulatesInternalProperty()
        {
            string user_name = "test";
            Message testm1 = new Message();
            testm1.Subject = "testm1";
            Message testm2 = new Message();
            testm2.Subject = "testm2";

            List<Message> testList = new List<Message>();
            testList.Add(testm1);
            testList.Add(testm2);

            var mRepo = new Mock<IMessageRepository>();
            mRepo.Setup(r => r.RetrieveAllMessages(user_name)).Returns(testList);

            MessageListViewModel testAllMessageViewModel = new AllMessageListViewModel(mRepo.Object, user_name);

            Assert.That(testAllMessageViewModel.getMessages(), Is.EquivalentTo(testList));
        }

        [TestCase]
        public void AuthorizedMessageListViewModelCallsAuthorizedMessagesRepositoryMethodAndPopulatesInternalProperty()
        {
            string user_name = "test";
            Message testm1 = new Message();
            testm1.Subject = "testm1";
            Message testm2 = new Message();
            testm2.Subject = "testm2";

            List<Message> testList = new List<Message>();
            testList.Add(testm1);
            testList.Add(testm2);

            var mRepo = new Mock<IMessageRepository>();
            mRepo.Setup(r => r.RetrieveAuthorizedMessages(user_name)).Returns(testList);

            MessageListViewModel viewModel = new AuthorizedMessageListViewModel(mRepo.Object, user_name);

            

            Assert.That(viewModel.getMessages(), Is.EquivalentTo(testList));
        }

        [TestCase]
        public void UnauthorizedMessageListViewModelCallsUnauthorizedMessageRepositoryMethodAndPopulatesInternalProperty()
        {
            string user_name = "test";
            Message testm1 = new Message();
            testm1.Subject = "testm1";
            Message testm2 = new Message();
            testm2.Subject = "testm2";

            List<Message> testList = new List<Message>();
            testList.Add(testm1);
            testList.Add(testm2);

            var mRepo = new Mock<IMessageRepository>();
            mRepo.Setup(r => r.RetrieveUnauthorizedMessages(user_name)).Returns(testList);

            MessageListViewModel viewModel = new UnauthorizedMessageListViewModel(mRepo.Object, user_name);

            Assert.That(viewModel.getMessages(), Is.EquivalentTo(testList));
        }


        [TestCase]
        public void SendMessageActionReturnsErrorIfUnauthorized()
        {
            var mockMessageRepo = new Mock<IMessageRepository>();
            var mockBlogRepo = new Mock<IBlogRepository>();

            string user_name = "testUser@test.net";
            string recipient_name = "testRecipient@test.net";

            mockMessageRepo.Setup(r => r.CanMessage(user_name, recipient_name)).Returns(false);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(c => c.User.Identity.Name).Returns(user_name);
            

            BlogController _testController = new BlogController(mockBlogRepo.Object, mockMessageRepo.Object);
            _testController.ControllerContext = new ControllerContext();
            _testController.ControllerContext.HttpContext = mockHttpContext.Object;
            MessageCreationViewModel viewModel = new MessageCreationViewModel();
            viewModel.RecipientName = recipient_name;

            ViewResult testResult = (ViewResult) _testController.SendMessage(viewModel);
            Assert.That(testResult.ViewData.ModelState.ErrorCount, Is.EqualTo(1));
            

            
        }



    }
}
