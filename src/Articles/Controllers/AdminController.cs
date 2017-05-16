using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Articles.Models;
using Articles.Models.AdminViewModels;
using Articles.Models.AdminViewModels.AdminPostListViewModels;
using myExtensions;
using Articles.Models.AdminViewModels.AdminUserListViewModels;

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

        public IActionResult ExpandPost(int postId)
        {
            Post post = _adminRepository.RetrievePostById(postId);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AdminPostExpanded", post);
            }
            else
            {
                return View("_AdminPostExpanded", post);
            }
            
        }

        // NOTE: does not check if is ajaxrequest because this should not be accessible 
        // by a client not using ajax. 
        public IActionResult UnexpandPost (int postId)
        {
            Post post = _adminRepository.RetrievePostById(postId);
            return PartialView("_AdminPost", post);
        }

        // displays posts which match search string @param s in some way 
        public IActionResult Search(string s)
        {
            AdminPostsListViewModel viewModel = new AdminPostsSearch(_adminRepository, s);
            return View("ListPostsAdmin", viewModel);
        }

        
        /* *************
         * CATEGORIES
         * *************/

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

        [HttpGet]
        public IActionResult CreateCategory()
        {
            CategoryCreationViewModel vm = new CategoryCreationViewModel();
            return View(vm);
        }


        [HttpPost]
        public IActionResult CreateCategory([Bind(include: "CategoryName, Description")] CategoryCreationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // succeeded, go back to list
                if (viewModel.CreateAndPersistCategory(_adminRepository))
                    return RedirectToAction("ListCategories");

                // failed, display message
                else
                {
                    ModelState.AddModelError(string.Empty, "a category with that name already exists");
                    return View(viewModel);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "something went wrong with model binding");
                return View(viewModel);
            }

        }



        // displays a list of users sorted alphabetically
        public IActionResult ListUsers()
        {
            AdminUserListViewModel viewModel = new AdminUserListAlphabetical(_adminRepository);
            return View("ListUsersAdmin", viewModel);
        }

        // displays a list of users for search @param s , sorted alphabetically
        public IActionResult SearchUsers(string s)
        {
            AdminUserListViewModel viewModel = new AdminUserListSearch(_adminRepository, s);
            return View("ListUsersAdmin", viewModel);
        }


        public IActionResult BanUser (string username)
        {
            if (username == null || username.Length < 1)
                throw new InvalidOperationException("username parameter empty");
            _adminRepository.BanUser(username);

            // redirects to referring list view 
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult UnbanUser (string username)
        {
            if (username == null || username.Length < 1)
                throw new InvalidOperationException("username parameter empty");

            _adminRepository.UnbanUser(username);

            // redirects to referring list view 
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> MakeAdmin (string username)
        {
            await _adminRepository.MakeAdminAsync(username);

            // redirect to page from which user was made admin 
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> RevokeAdmin (string username)
        {
            // user attempted to revoke own admin privileges
            if (User.Identity.Name.Equals(username))
                throw new InvalidOperationException("You tried to revoke your own admin status!");

            await _adminRepository.RevokeAdminAsync(username);

            // redirect to page from which user revoked admin 
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}