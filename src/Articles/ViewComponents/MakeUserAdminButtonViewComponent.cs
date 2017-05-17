using Articles.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.ViewComponents
{
    public class MakeUserAdminButtonViewComponent : ViewComponent
    {
        IAdminRepository _adminRepo;
        public MakeUserAdminButtonViewComponent(IAdminRepository adminRepo)
        {
            _adminRepo = adminRepo;
        }

        public async Task<IViewComponentResult> InvokeAsync(string username)
        {
            // don't display "revoke admin" to the admin accessing this page 
            if (User.Identity.Name == username)
                return View("EmptySelf");
           
            try
            {
                if (await _adminRepo.CheckIfAdminAsync(username))
                    return View("RevokeAdminButton", username);

                else
                    return View("MakeAdminButton", username);
            }
            catch 
            {
                return View("Empty");
            }
                    
        }
    }
}
