using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.AdminViewModels.AdminPostListViewModels
{
    public class AdminCategoriesSearchListViewModel : AdminCategoriesListViewModel
    {
        public AdminCategoriesSearchListViewModel(IAdminRepository adminRepo, string search) 
            :base()
        {
            this.PopulateCategoriesList(adminRepo.ListCategoriesForSearch(search));
        }
    }
}
