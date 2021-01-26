using BookLibrary.ViewModels.Filtration;
using BookLibrary.ViewModels.Pagination;
using BookLibrary.ViewModels.Sorting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.ViewModels.ManageUsers
{
    public class UsersListViewModel
    {
        public IEnumerable<IdentityUser> Users { get; set; }
        public UsersFilterViewModel UsersFilterVM { get; set; }
        public PageViewModel UsersPageVM { get; set; }
        public UsersSortViewModel UsersSortVM { get; set; }
    }
}
