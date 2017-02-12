using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Articles.Models;
using Microsoft.AspNetCore.WebUtilities;
using Articles.Models.Core;
using Articles.Models.BlogViewModels;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel;
using Articles.ViewComponents;
using myExtensions;
using Articles.Models.BlogViewModels.ListViewModels;
using Microsoft.Extensions.Options;
using Articles.Models.MessageViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Articles.Controllers
{
    [RequireHttps]
    public class BlogController : Controller
    {
        public IActionResult Index()
        {


            return View();
        }

        private readonly IBlogRepository _blogRepository;
        private readonly IMessageRepository _messageRepository;

        //constructer for  dependency injection, registered in the startup.cs service. repository DI is configured her to use a 
        //scoped lifetime, which means one instance is used in all cases within one request, and a new instance is created each request 
        public BlogController(IBlogRepository blogRepository, IMessageRepository messageRepository)
        {

            _messageRepository = messageRepository;
            _blogRepository = blogRepository;
        }

        //returns a list of all posts. parameter p is provided as a query string and represents the current page, used by 
        // pagination / repository methods to pull the correct posts from the database. if the user is authenticated, their customized page 
        //size will be used, defaulting to 10 either if user is not authenticaed or user has not submitted custom page size.
        public IActionResult Posts(int p = 1)
        {
            ListViewModel viewModel = new AllPostsListViewModel(_blogRepository, p,
                User.Identity.IsAuthenticated ? User.Identity.Name : null);
            
            ViewBag.Title = "Latest Posts";
            
            return View("List", viewModel);
        }

        [Authorize]
        public IActionResult CustomPosts(int p = 1)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                string user_name = User.Identity.Name;

                ListViewModel viewModel = new CustomListViewModel(_blogRepository, p, user_name);
                ViewBag.Title = String.Format(@"{0} posts found for user {1} ", viewModel.TotalPosts, user_name);
                return View("List", viewModel);
            }
        }

        [Authorize]
        public IActionResult YourAuthoredPosts(int p =1)
        {
            ListViewModel viewModel = new AuthorListViewModel(_blogRepository, User.Identity.Name, p,
                User.Identity.Name);
            ViewBag.SaveUnsave = false;
            return View("List", viewModel);
        }

        public IActionResult PostsByAuthor( string author, int p = 1)
        {
            ListViewModel viewModel = new AuthorListViewModel(_blogRepository, author, p,
                 User.Identity.IsAuthenticated ? User.Identity.Name : null);
            ViewBag.Title = String.Format("{0} posts found by author {1}", viewModel.TotalPosts, author);
                return View("List", viewModel);
          
        }

        public IActionResult HotPosts(int p = 1)
        {
            ListViewModel viewModel = new HotPostsListViewModel(_blogRepository, p, User.Identity.IsAuthenticated ?
                User.Identity.Name : null);
            return View("List", viewModel);
        }

        [Authorize]
        public IActionResult Subscribed(int p = 1)
        {

            string current_username = User.Identity.Name;
            ListViewModel viewModel = new SubscribedListViewModel(_blogRepository, p, current_username);
            ViewBag.Title = String.Format("{0} posts by authors to which user {1} subscribes", 
                viewModel.TotalPosts, current_username);
            return View("List", viewModel);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Customize()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                CustomizeViewModel ViewModel = new CustomizeViewModel(_blogRepository, User.Identity.Name);
                return View(ViewModel);
            }
        }

        [HttpPost]
        public IActionResult Customize([Bind(include: "categories, category_counts, user_page_size, subscribed_authors, publicMessaging")] CustomizeViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                //returns error if there are somehow fewer categories in the ViewModel than in the databse 
                if (ViewModel.categories.Keys.Count < _blogRepository.Categories().Count)
                {
                    return new StatusCodeResult(406);
                }


                _blogRepository.UpdateCustomization(ViewModel, User.Identity.Name);
                return RedirectToAction("CustomPosts");
            }

            else
            {
                return new StatusCodeResult(406);
            }
        }

        public IActionResult Category(string category, int p = 1)
        {
            ListViewModel viewModel = new CategoryListViewModel(_blogRepository, category, p,
                User.Identity.IsAuthenticated ? User.Identity.Name : null);

            if (viewModel.Category == null)
                return new StatusCodeResult(400);

            ViewBag.Title = String.Format(@"{0} posts on category ""{1}""", viewModel.TotalPosts,
                        viewModel.Category.Name);
            ViewBag.ByCategory = true;
            ViewBag.Category = viewModel.Category;

            return View("List", viewModel);
        }

        public IActionResult Tag(string tag, int p = 1)
        {
            ListViewModel viewModel = new TagListViewModel(_blogRepository, tag, p, User.Identity.IsAuthenticated ?
                User.Identity.Name : null);


            if (viewModel.Tag == null)
                return new StatusCodeResult(400);

            ViewBag.Title = String.Format(@"{0} posts tagged ""{1}""", viewModel.TotalPosts, viewModel.Tag.Name);
            ViewBag.ByTag = true;
            ViewBag.Tag = viewModel.Tag;

            return View("List", viewModel);
        }

       

        //returns a partial view of search results if ajax request; else, returns full List View
        public IActionResult Search(string s, int p = 1)
        {
            ListViewModel viewModel = new SearchListViewModel(_blogRepository, s, p,
                User.Identity.IsAuthenticated ? User.Identity.Name : null);

            ViewBag.Title = String.Format(@"{0} posts found for search ""{1}""", viewModel.TotalPosts, 
                viewModel.Search);
            if (Request.IsAjaxRequest())
            {
                ViewBag.s = String.Format("<a href=\"/Blog/partialSearch?s={0}\">Load Full Results</a>", s);
                return PartialView("List", viewModel);
            }
            else
            {
                return View("List", viewModel);
            }
           
        }
        public IActionResult AjaxSearch()
        {
            return View("_AjaxSearch");
        }

        public IActionResult Post(int year, int month, string ti)
        {
            var post = _blogRepository.Post(year, month, ti);
            ViewBag.RefererTag = false;

            if (post == null)
                return new StatusCodeResult(402);

            if (post.Published == false && User.Identity.IsAuthenticated == false)
                return new StatusCodeResult(401);

            //checks whether user came from "all posts" or "custom posts" [this will not persist...need better implementation, perhaps with qstring]
            if (Request.Headers["Referer"].ToString().Contains("Custom") == true || Request.Headers["Referer"].ToString().Contains("custom") == true)
            {
                ViewBag.Type = "Custom";
            }
            else
            {
                ViewBag.Type = "All Posts";
            }


            //checks if user came from a tag page in order to render tag breadcrumb
            if (Request.Headers["Referer"].ToString().Contains("tag") || Request.Headers["Referer"].ToString().Contains("Tag"))
            {
                ViewBag.RefererTag = true;

                string reference_url = Request.Headers["Referer"].ToString();
                string query_string = null;

                //query string index
                int qsi = reference_url.IndexOf('?');

                //if query string doesnt exist, return view with category breadcrumb instead
                if (qsi == -1)
                {
                    ViewBag.RefererTag = false;
                    return View(post);
                }
                else if (qsi >= 0)
                {
                    query_string = (qsi < reference_url.Length - 1) ? reference_url.Substring(qsi + 1) : String.Empty;
                }

                // Parse the query string variables into a dictionary.
                IDictionary<string, StringValues> qsdictionary = QueryHelpers.ParseQuery(query_string);

                foreach (PostTag ptag in post.PostTags)
                {
                    if (qsdictionary["tag"].ToString().Equals(ptag.Tag.UrlSlug, StringComparison.OrdinalIgnoreCase))
                    {
                        ViewBag.Name = ptag.Tag.Name;
                        ViewBag.Slug = ptag.Tag.UrlSlug;
                        post = _blogRepository.IncrementViews(post);
                        return View(post);
                    }
                }
            }

            post = _blogRepository.IncrementViews(post);
            return View(post);
        }

        //this causes a bug where if a user presses save and is not logged in, they will be redirected to 
        //the login page due to authorize decorator, which will redirect them back to this action upon login
        // -- this action will save the post, but then redirect the user back to the login page with no indication
        //that they have either logged in successfully or saved the post, when in fact both have happened.
        //ultimately this functionality should be implemented with AJAX, which will eliminate the need for 
        //redirects 
        [Authorize]
        public IActionResult SavePost(int year, int month, string ti)
        {
            _blogRepository.SavePostForUser(year, month, ti, User.Identity.Name);
            if (Request.IsAjaxRequest())
            {
                Post post = _blogRepository.Post(year, month, ti);
                return PartialView("_Unsave", post);
            }
            else
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }
            
        }

        [Authorize]
        public IActionResult UnsavePost(int year, int month, string ti)
        {
            _blogRepository.UnsavePostForUser(year, month, ti, User.Identity.Name);
            if(Request.IsAjaxRequest())
            {
                Post post = _blogRepository.Post(year, month, ti);
                return PartialView("_Save", post);
            }
            else
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }
            
        }

        //called via AJAX from the list of saved posts, which will hide the post in question, and then 
        //insert a link to undo the unsave which is returned by this method as a partial view
        public IActionResult UnsaveAndHide(int year, int month, string ti)
        {
            _blogRepository.UnsavePostForUser(year, month, ti, User.Identity.Name);
            Post post = _blogRepository.Post(year, month, ti);
            return PartialView("_UndoUnsave", post);
        }

        public IActionResult UndoUnsave(int year, int month, string ti)
        {
            _blogRepository.SavePostForUser(year, month, ti, User.Identity.Name);
            Post post = _blogRepository.Post(year, month, ti);
            return PartialView("_PostTemplate", post);
        }
        public IActionResult SaveViewComponent()
        {
            return ViewComponent("SaveViewComponent");
        }

        [Authorize]
        public IActionResult SavedPosts(int p = 1)
        {
            ListViewModel viewModel = new SavedListViewModel(_blogRepository, p, User.Identity.Name);
            ViewBag.Title = String.Format(@"{0} posts saved for user {1} ", viewModel.TotalPosts, User.Identity.Name);

            if (Request.IsAjaxRequest())
            {
                return PartialView("List", viewModel);
            }
            else
            {
                return View("List", viewModel);
            }
        }

        [Authorize]
        public IActionResult BlockUser (string blockUserName)
        {
            string user_name = User.Identity.Name;
            _blogRepository.BlockUser(user_name, blockUserName);

            try { return Redirect(Request.Headers["Referer"].ToString()); }
            catch
            {
                return RedirectToAction("Posts"); 
            }
            
        }

        [Authorize]
        public IActionResult UnBlockUser(string blockUserName)
        {
            string user_name = User.Identity.Name;
            _blogRepository.UnblockUser(user_name, blockUserName);

            try { return Redirect(Request.Headers["Referer"].ToString()); }
            catch
            {
                return RedirectToAction("Posts");
            }

        }

        [Authorize]
        public IActionResult AuthorizeUser(string authorizeUserName)
        {
            _blogRepository.AuthorizeUser(User.Identity.Name, authorizeUserName);

            try { return Redirect(Request.Headers["Referer"].ToString()); }
            catch
            {
                return RedirectToAction("Posts");
            }
        }

        [Authorize]
        public IActionResult UnAuthorizeUser(string unAuthorizeUserName)
        {
            _blogRepository.UnAuthorizeUser(User.Identity.Name, unAuthorizeUserName);

            try { return Redirect(Request.Headers["Referer"].ToString()); }
            catch
            {
                return RedirectToAction("Posts");
            }
        }

        [Authorize]
        public IActionResult Subscribe(string authorname)
        {
            string user_name = User.Identity.Name;
            _blogRepository.SubscribeAuthor(user_name, authorname);
            if (Request.IsAjaxRequest())
            {
                BlogUser author = _blogRepository.RetrieveUser(authorname);
                return PartialView("~/Views/Shared/Components/Subscribe/Unsubscribe.cshtml", author);
            }
           else
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }
            
        }

        [Authorize]
        public IActionResult Unsubscribe(string authorname)
        {
            string user_name = User.Identity.Name;
            _blogRepository.UnsubscribeAuthor(user_name, authorname);
            if(Request.IsAjaxRequest())
            {
                BlogUser author = _blogRepository.RetrieveUser(authorname);
                return PartialView("~/Views/Shared/Components/Subscribe/Subscribe.cshtml", author);
            }
            
            else
            {
                return RedirectToAction("Posts");
            }
        }

        public IActionResult UnsubscribeUpdate(string authorname)
        {
            string user_name = User.Identity.Name;
            _blogRepository.UnsubscribeAuthor(user_name, authorname);

            //****need to add way to retrieve page number****
            ListViewModel viewModel = new SubscribedListViewModel(_blogRepository, 1, user_name);
            ViewBag.Title = String.Format("{0} posts by authors to which user {1} subscribes",
                viewModel.TotalPosts, user_name);
            return PartialView("List", viewModel);
        }

        public IActionResult UndoUnsubscribe(string authorname)
        {
            string user_name = User.Identity.Name;
            _blogRepository.SubscribeAuthor(user_name, authorname);

            //****need to add way to retrieve page number****
            ListViewModel viewModel = new SubscribedListViewModel(_blogRepository, 1, user_name);
            ViewBag.Title = String.Format("{0} posts by authors to which user {1} subscribes",
                viewModel.TotalPosts, user_name);
            return PartialView("List", viewModel);
        }

        [Authorize]
        public IActionResult LikePost(int year, int month, string ti)
        {
            _blogRepository.LikePostForUser( year, month, ti, User.Identity.Name);
            if(Request.IsAjaxRequest())
            {
                Post post = _blogRepository.Post(year, month, ti);
                return PartialView("_Unlike", post);
            }
            else
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }
            
        }

        [Authorize]
        public IActionResult UnlikePost(int year, int month, string ti)
        {
            _blogRepository.UnlikePostForUser(year, month, ti, User.Identity.Name);
            if(Request.IsAjaxRequest())
            {
                Post post = _blogRepository.Post(year, month, ti);
                return PartialView("_Like", post);
            }
            else
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }

        }


        [Authorize]
        public IActionResult YourMessages() 
        {
            MessageListViewModel viewModel = new AllMessageListViewModel(_messageRepository, User.Identity.Name);
            return View("Messages", viewModel);
        }

        [Authorize]
        public IActionResult YourUnauthorizedMessages()
        {
            MessageListViewModel viewModel = new UnauthorizedMessageListViewModel(_messageRepository, User.Identity.Name);
            return View("Messages", viewModel);
        }

        [Authorize]
        public IActionResult YourAuthorizedMessages()
        {
            MessageListViewModel viewModel = new AuthorizedMessageListViewModel(_messageRepository, User.Identity.Name);
            return View("Messages", viewModel);
        }

        [Authorize]
        public IActionResult YourSentMessages()
        {
            MessageListViewModel viewModel = new SentMessageListViewModel(_messageRepository, User.Identity.Name);
            return View("Messages", viewModel);
        }

        [Authorize]
        public IActionResult YourUnreadMessages()
        {
            MessageListViewModel viewModel = new UnreadMessageListViewModel(_messageRepository, User.Identity.Name);
            return View("Messages", viewModel);
        }

        
        [Authorize]
        public IActionResult MessagesBetweenUsers(string user_name, string user_name2)
        {
            string otherUserName;
            otherUserName = (user_name == User.Identity.Name) ? user_name2 : user_name;
            MessageListViewModel viewModel = new MessagesBetweenUsersViewModel(_messageRepository, User.Identity.Name, otherUserName);
            return View("Messages", viewModel);
        }

        [HttpGet]
        [Authorize]
        public IActionResult SendMessage()
        {
            MessageCreationViewModel viewModel = new MessageCreationViewModel();

            if(Request.IsAjaxRequest())
            {
                return PartialView(viewModel);
            }
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SendMessage([Bind(include: "RecipientName, Subject, Contents")] MessageCreationViewModel viewModel)
        {
            viewModel.AuthorName = User.Identity.Name;
            if(_messageRepository.CanMessage(User.Identity.Name, viewModel.RecipientName))
            {
                viewModel.sendMessage(_messageRepository);
                return RedirectToAction("YourMessages");
            }
            else
            {
                ModelState.AddModelError(String.Empty, "you arent allowed to message this person");
                return View(viewModel);
            }
           

           
        }

       // public IActionResult SendMessages 

        public IActionResult MarkAsRead(int MessageId)
        {
            bool succeeded = _messageRepository.MarkAsRead(MessageId);

            if(succeeded)
            {
                try
                {
                   return Redirect(Request.Headers["Referer"].ToString());
                }
                catch
                {
                    return RedirectToAction("YourMessages");
                }
            }

            else
            {
                throw new InvalidOperationException("Message not found or more than one message found");
            }
        }

        public IActionResult MarkAsUnread(int MessageId)
        {
            bool succeeded = _messageRepository.MarkAsUnread(MessageId);

            if(succeeded)
            {
                try
                {
                    return Redirect(Request.Headers["Referer"].ToString());
                }
                catch
                {
                    return RedirectToAction("YourMessages");
                }
            }
            else
            {
                throw new InvalidOperationException("Message not found or more than one message found");
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult MessageSpecifiedUser(string userName)
        {
            if(!_messageRepository.CanMessage(User.Identity.Name, userName))
            {
                MessageCreationViewModel vm = new MessageCreationViewModel();
                ModelState.AddModelError(String.Empty, "you arent allowed to message this person!");
                return View("SendMessage", vm);
            }
            MessageCreationViewModel viewModel = new MessageCreationUserSpecifiedViewModel(userName);

            return View("SendMessage", viewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult MessageSpecifiedUser([Bind(include: "RecipientName, Subject, Contents")] MessageCreationViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                if (_messageRepository.CanMessage(User.Identity.Name, viewModel.RecipientName))
                {
                    viewModel.sendMessage(_messageRepository);
                    return RedirectToAction("YourMessages");
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "you arent allowed to message this person");
                    return View(viewModel);
                }
            }
            else
            {
                viewModel = new MessageCreationViewModel();
                ModelState.AddModelError(String.Empty, "something went wrong with model binding");
                return RedirectToAction("SendMesssage");
            }
        }


        [Authorize]
        public IActionResult ReadMessageViewMore(int messageId)
        {
            Message toDisplay = _messageRepository.retrieveSpecifiedMessage(messageId);
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Shared/Components/ReadOrUnread/ReadMessage.cshtml", toDisplay);
            } 

            else
            {
                return View("~/Views/Shared/Components/ReadOrUnread/ReadMessage.cshtml", toDisplay);
            }
        }
    }
}