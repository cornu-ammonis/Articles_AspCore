using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.AdminViewModels
{
    public class AdminCategoriesListViewModel
    {
        public IList<Category> categories { get; set; }

        public AdminCategoriesListViewModel(IAdminRepository adminRepository)
        {
            PopulateCategoriesList(adminRepository.ListAllCategories());
        }

        // populate the viewModel's categories list
        // @param _categories - list of categories to display
        protected virtual void PopulateCategoriesList(IList<Category> _categories)
        {
            this.categories = _categories;
        }
    }
}
