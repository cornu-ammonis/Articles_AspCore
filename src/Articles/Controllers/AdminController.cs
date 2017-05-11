using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Articles.Models;
using Articles.Models.AdminViewModels;
using Articles.Models.AdminViewModels.AdminPostListViewModels;

namespace Articles.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;


        // constructor for dependency injection
        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        // lists all the posts
        public IActionResult ListPostsAdmin()
        {
            AdminPostsListViewModel viewModel = new AdminPostsUnsorted(_adminRepository);
            return View(viewModel);
        }

        // lists posts sorted by posted date descending
        public IActionResult ListPostsDescendingDate()
        {
            AdminPostsListViewModel viewModel = new AdminPostsDescendingDateViewModel(_adminRepository);
            return View("ListPostsAdmin", viewModel);
        }

        // displays list of posts sorted by posted date ascending
        public IActionResult ListPostsAscendingDate()
        {
            AdminPostsListViewModel viewModel = new AdminPostsAscendingDateViewModel(_adminRepository);
            return View("ListPostsAdmin", viewModel);
        }

        // displays list of posts sorted by title in descending order
        public IActionResult ListPostsDescendingTitle()
        {
            AdminPostsListViewModel viewModel = new AdminPostsDescendingTitleViewModel(_adminRepository);
            return View("ListPostsAdmin", viewModel);
        }

        // displays list of posts sorted by title in ascending order
        public IActionResult ListPostsAscendingTitle()
        {
            AdminPostsListViewModel viewModel = new AdminPostsAscendingTitleViewModel(_adminRepository);
            return View("ListPostsAdmin", viewModel);
        }

        // displays list of posts sorted by category name in descending order
        public IActionResult ListPostsDescendingCategory()
        {
            AdminPostsListViewModel viewModel = new AdminPostsDescendingCategoryViewModel(_adminRepository);
            return View("ListPostsAdmin", viewModel);
        }

        // displays list of posts sorted by category name in ascending order
        public IActionResult ListPostsAscendingCategory()
        {
            AdminPostsListViewModel viewModel = new AdminPostsAscendingCategoryViewModel(_adminRepository);
            return View("ListPostsAdmin", viewModel);
        }

        // displays list of posts sorted by author name in descending order
        public IActionResult ListPostsDescendingAuthor()
        {
            AdminPostsListViewModel viewModel = new AdminPostsDescendingAuthorViewModel(_adminRepository);
            return View("ListPostsAdmin", viewModel);
        }

        // displays list of posts sorted by author name in ascending order
        public IActionResult ListPostsAscendingAuthor()
        {
            AdminPostsListViewModel viewModel = new AdminPostsAscendingAuthorViewModel(_adminRepository);
            return View("ListPostsAdmin", viewModel);
        }

        // changes the specified post so that the database properly reflects it as unpublished
        public IActionResult UnpublishPost(int postId)
        {
            _adminRepository.UnpublishPost(postId);
             return Redirect(Request.Headers["Referer"].ToString());
        }

        // changes the specified post so that the database properly reflects it as unpublished
        public IActionResult PublishPost(int postId)
        {
            _adminRepository.PublishPost(postId);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        // displays posts which match search string @param s in some way 
        public IActionResult Search(string s)
        {
            AdminPostsListViewModel viewModel = new AdminPostsSearch(_adminRepository, s);
            return View("ListPostsAdmin", viewModel);
        }

        public IActionResult ListCategories ()
        {
            AdminCategoriesListViewModel viewModel = new AdminCategoriesListViewModel(_adminRepository);
            return View("ListCategoriesAdmin", viewModel);
        }

        public IActionResult SearchCategories(string s)
        {
            AdminCategoriesListViewModel viewModel = new AdminCategoriesSearchListViewModel(_adminRepository, s);
            return View("ListCategoriesAdmin", viewModel);
        }
    }
}