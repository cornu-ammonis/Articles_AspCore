using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Articles.Models;
using Microsoft.AspNetCore.WebUtilities;
using Articles.Models.Core;
using Articles.Models.BlogViewModels;

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

            return View(post);
        }

    }
}