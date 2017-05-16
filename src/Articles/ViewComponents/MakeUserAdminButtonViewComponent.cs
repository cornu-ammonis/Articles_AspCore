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
            if (await _adminRepo.CheckIfAdminAsync(username))
                return View("MakeNotAdminButton", username);

            else
                return View("RevokeAdminButton", username);           
        }
    }
}
