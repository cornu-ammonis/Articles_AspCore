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
           

            // this catches an exception which arises if a "BlogUser" entry exists without a
            // corresponding entry in the .net identity tables. 
            // this *should* only happen in the case where the seed method creates mock users 
            // to author posts for the purposes of developing the site. this should never happen
            // in production, but I'll leave the try/catch so that it's clear whats happening if
            // this does arise in production, and to handle the case where we forget and leave 
            // some seed method users around. 
            try
            {
                if (await _adminRepo.CheckIfAdminAsync(username))
                    return View("RevokeAdminButton", username);

                else
                    return View("MakeAdminButton", username);
            }
            catch 
            {
                // display message that this "BlogUser" isnt fully registered
                return View("EmptyNoIdentity");
            }
                    
        }
    }
}
