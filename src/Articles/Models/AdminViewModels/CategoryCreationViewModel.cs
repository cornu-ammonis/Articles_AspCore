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
    }
}
