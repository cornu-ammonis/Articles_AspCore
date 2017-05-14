using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.AdminViewModels
{
    public class CategoryCreationViewModel
    {
        [Required]
        public string CategoryName { get; set; }
        public string Description { get; set; }


        // returns false if a category with that name already exists
        // note: currently creates url slug procedurally by appending the number of 
        // categories in the database + 1 to 'cat'
        public bool CreateAndPersistCategory(IAdminRepository adminRepo)
        {
            IList<Category> ExistingCategories = adminRepo.ListAllCategories();
            if (ExistingCategories.Any(c => c.Name.Equals(CategoryName)))
                return false;

            Category toAdd = new Category()
            {
                Name = CategoryName,
                Description = Description,

                // automatically initializes the url slug based on the count of current categories
                UrlSlug = "cat" + (ExistingCategories.Count + 1).ToString()
            };

            adminRepo.AddNewCategoryToDatabase(toAdd);
            return true;
        }
    }
}
