using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace BookLibrary.ViewModels.Filtration
{
    public class UsersFilterViewModel
    {
        public UsersFilterViewModel(string searchReq)
        {
            SearchReq = searchReq;
        }
        public string SearchReq { get; private set; } 
    }
}
