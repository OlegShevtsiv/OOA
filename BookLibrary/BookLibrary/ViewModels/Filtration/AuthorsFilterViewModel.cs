using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.ViewModels.Filtration
{
    public class AuthorsFilterViewModel
    {
        public AuthorsFilterViewModel(string searchReq)
        {
            this.SearchReq = searchReq;
        }
        public string SearchReq { get; set; }
    }
}
