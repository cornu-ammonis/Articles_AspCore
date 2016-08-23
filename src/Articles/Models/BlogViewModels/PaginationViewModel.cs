using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.BlogViewModels
{
    public class PaginationViewModel
    {

        public int current_page { get; set; }
        public double total_pages { get; set; }
        public string previous_page { get; set; }
        public string next_page { get; set; }
        public bool p_visible { get; set; }
        public bool n_visible { get; set; }
    }
}
