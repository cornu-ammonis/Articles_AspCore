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
            AdminPostsListViewModel viewModel = new AdminPostDescendingDateViewModel(_adminRepository);
            return View("ListPostsAdmin", viewModel);
        }

        // displays list of posts sorted by posted date ascending
        public IActionResult ListPostsAscendingDate()
        {
            AdminPostsListViewModel viewModel = new AdminPostAscendingDateViewModel(_adminRepository);
            return View("ListPostsAdmin", viewModel);
        }

        public IActionResult ListPostsDescendingTitle()
        {
            AdminPostsListViewModel viewModel = new AdminPostsDescendingTitleViewModel(_adminRepository);
            return View("ListPostsAdmin", viewModel);
        }

        public IActionResult ListPostsAscendingTitle()
        {
            AdminPostsListViewModel viewModel = new AdminPostsAscendingTitleViewModel(_adminRepository);
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
    }
}