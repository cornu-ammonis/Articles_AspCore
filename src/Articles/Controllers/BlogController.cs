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

namespace Articles.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        private readonly IBlogRepository _blogRepository;

        //constructer for  dependency injection, registered in the startup.cs service. repository DI is configured her to use a 
        //scoped lifetime, which means one instance is used in all cases within one request, and a new instance is created each request 
        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        //returns a list of all posts. parameter p is provided as a query string and represents the current page, used by 
        // pagination / repository methods to pull the correct posts from the database. if the user is authenticated, their customized page 
        //size will be used, defaulting to 10 either if user is not authenticaed or user has not submitted custom page size.
        public ViewResult Posts(int p = 1)
        {
            //these were replaced by the viewmodel, which now handles generation of posts and total posts 
            // var posts = _blogRepository.Posts(p - 1, 10);
            // var total_posts = _blogRepository.TotalPosts();

            ListViewModel viewModel;
            if (User.Identity.IsAuthenticated)
            {
                viewModel = new ListViewModel(_blogRepository, p, "All", User.Identity.Name);
                ViewBag.SaveUnsaveDict = viewModel.IsSaved;
                ViewBag.SaveUnsave = true;
            }
            else
            {
                viewModel = new ListViewModel(_blogRepository, p, "All");
                ViewBag.SaveUnsave = false;
            }
            
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

                var viewModel = new ListViewModel(_blogRepository, p, "Custom", user_name);
                ViewBag.Title = String.Format(@"{0} posts found for user {1} ", viewModel.TotalPosts, user_name);
                ViewBag.SaveUnsaveDict = viewModel.IsSaved;
                ViewBag.SaveUnsave = true;
                return View("List", viewModel);
            }
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
        public IActionResult Customize([Bind(include: "categories, category_counts, user_page_size")] CustomizeViewModel ViewModel)
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

                //return View("Test", ViewModel);
            }

            else
            {
                return new StatusCodeResult(406);
            }
        }

        public IActionResult Category(string category, int p = 1)
        {

            ListViewModel viewModel;
            if (User.Identity.IsAuthenticated)
            {
                viewModel = new ListViewModel(_blogRepository, category, "Category", p, User.Identity.Name);
                //dictionary of type <string, bool>; key is post.urlSlug, value is whether it has been saved by current user
                ViewBag.SaveUnsaveDict = viewModel.IsSaved;
                //tells the view to toggle save/unsave button; only possible if user is authenticated and 
                // ViewBag.SaveUnsaveDict exists 
                ViewBag.SaveUnsave = true;
            }
            else
            {
                viewModel = new ListViewModel(_blogRepository, category, "Category", p);
                //tells view not to toggle save/unsave button; dict doesnt exist 
                ViewBag.SaveUnsave = false;
            }


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
            ListViewModel viewModel;
            if (User.Identity.IsAuthenticated)
            {
                viewModel = new ListViewModel(_blogRepository, tag, "Tag", p, User.Identity.Name);

                //dictionary of type <string, bool>; key is post.urlSlug, value is whether it has been saved by current user
                ViewBag.SaveUnsaveDict = viewModel.IsSaved;
                //tells view to toggle save unsave; dict exists
                ViewBag.SaveUnsave = true;
            }
            else
            {
                viewModel = new ListViewModel(_blogRepository, tag, "Tag", p);
               
                //tells view not to toggle save/unsave; dict doesnt exist
                ViewBag.SaveUnsave = false;
            }


            if (viewModel.Tag == null)
                return new StatusCodeResult(400);

            ViewBag.Title = String.Format(@"{0} posts tagged ""{1}""", viewModel.TotalPosts, viewModel.Tag.Name);
            ViewBag.ByTag = true;
            ViewBag.Tag = viewModel.Tag;

            return View("List", viewModel);
        }

        public ViewResult Search(string s, int p = 1)
        {
            ListViewModel viewModel;
            if (User.Identity.IsAuthenticated)
            {
                viewModel = new ListViewModel(_blogRepository, s, "Search", p, User.Identity.Name);
               
                //dictionary of type <string, bool>; key is post.urlSlug, value is whether it has been saved by current user
                ViewBag.SaveUnsaveDict = viewModel.IsSaved;
                //tells view to toggle save unsave; dict exists
                ViewBag.SaveUnsave = true;
            }
            else
            {
                viewModel = new ListViewModel(_blogRepository, s, "Search", p);

                //tells view not to toggle save/unsave; dict doesnt exist
                ViewBag.SaveUnsave = false;
            }


            ViewBag.Title = String.Format(@"{0} posts found for search ""{1}""", viewModel.TotalPosts, s);
            return View("List", viewModel);
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
                        return View(post);
                    }
                }
            }

            ViewBag.Saved = false;
            if (User.Identity.IsAuthenticated)
            {
                if(_blogRepository.CheckIfSaved(post, User.Identity.Name))
                {
                    ViewBag.Saved = true;
                }
            }
            return View(post);
        }

        [Authorize]
        public IActionResult SavePost(int year, int month, string ti)
        {
            _blogRepository.SavePostForUser(year, month, ti, User.Identity.Name);
            //return RedirectToAction("Post", new { year, month, ti });
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [Authorize]
        public IActionResult UnsavePost(int year, int month, string ti)
        {
            _blogRepository.UnsavePostForUser(year, month, ti, User.Identity.Name);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [Authorize]
        public IActionResult SavedPosts(int p = 1)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                string user_name = User.Identity.Name;

                var viewModel = new ListViewModel(_blogRepository, p, "Saved", user_name);
                ViewBag.Title = String.Format(@"{0} posts saved for user {1} ", viewModel.TotalPosts, user_name);
                if (viewModel.SaveUnsave)
                {
                    ViewBag.SaveUnsaveDict = viewModel.IsSaved;
                    ViewBag.SaveUnsave = true;
                }
                else
                {
                    ViewBag.SaveUnsave = false;
                }
                return View("List", viewModel);
            }

        }
    }
}