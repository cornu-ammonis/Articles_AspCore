using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.BlogViewModels
{
    public class PagerViewModel
    {

        public int CurrentPage { get; set; }
        public double TotalPages { get; set; }
        public string p { get; set; }
        public string n { get; set; }
        public bool p_visible { get; set; }
        public bool n_visible { get; set; }
    }
}
