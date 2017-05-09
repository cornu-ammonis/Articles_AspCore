using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Articles.Models;
using Articles.Models.AdminViewModels;

namespace Articles.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult ListPostsAdmin()
        {
            AdminPostsListViewModel viewModel = new AdminPostsUnsorted(_adminRepository);
            return View(viewModel);
        }

        public IActionResult ListPostsDescendingDate()
        {
            AdminPostsListViewModel viewModel = new AdminPostDescendingDateViewModel(_adminRepository);
            return View("ListPostsAdmin", viewModel);
        }

        public IActionResult UnpublishPost(int postId)
        {
            _adminRepository.UnpublishPost(postId);
             return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult PublishPost(int postId)
        {
            _adminRepository.PublishPost(postId);
            return Redirect(Request.Headers["Referer"].ToString());

        }
    }
}