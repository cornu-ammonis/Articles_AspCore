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

namespace Articles.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        private readonly IBlogRepository _blogRepository;

        //constructer for ninject dependency injection 
        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }


        public ViewResult Posts(int p = 1)
        {
            // var posts = _blogRepository.Posts(p - 1, 10);
            // var total_posts = _blogRepository.TotalPosts();
            var viewModel = new ListViewModel(_blogRepository, p);
            ViewBag.Title = "Latest Posts";
            return View("List", viewModel);
        }

        public IActionResult CustomPosts(int p = 1)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
              string user_name = User.Identity.Name;
               
              var viewModel = new ListViewModel(_blogRepository, p, user_name);
                ViewBag.Title = String.Format(@"{0} posts found for user {1} ", viewModel.TotalPosts, user_name);
              return View("List", viewModel);
            }
        }

        [HttpGet]
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
        public IActionResult Customize([Bind(include: "categories")] CustomizeViewModel ViewModel)
        {
            if (ModelState.IsValid) {  
                if (ViewModel.categories.Keys.Count < 3)
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
            var viewModel = new ListViewModel(_blogRepository, category, "Category", p);

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
            var viewModel = new ListViewModel(_blogRepository, tag, "Tag", p);
            if (viewModel.Tag == null)
                return new StatusCodeResult(400);

            ViewBag.Title = String.Format(@"{0} posts tagged ""{1}""", viewModel.TotalPosts, viewModel.Tag.Name);

            return View("List", viewModel);
        }

        public ViewResult Search(string s, int p = 1)
        {
            var viewModel = new ListViewModel(_blogRepository, s, "Search", p);

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

            if( Request.Headers["Referer"].ToString().Contains("Custom") == true || Request.Headers["Referer"].ToString().Contains("custom") == true)
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

                // Parse the query string variables into a NameValueCollection.
                IDictionary<string, StringValues> qsdictionary = QueryHelpers.ParseQuery(query_string);

                foreach (PostTag ptag in post.PostTags)
                {
                    if(qsdictionary["tag"].ToString().Equals(ptag.Tag.UrlSlug, StringComparison.OrdinalIgnoreCase))
                    {
                        ViewBag.Name = ptag.Tag.Name;
                        ViewBag.Slug = ptag.Tag.UrlSlug;
                        return View(post);
                    }
                }
            }

                return View(post);
        }

    }
}