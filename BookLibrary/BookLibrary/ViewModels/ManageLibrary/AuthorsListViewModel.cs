using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Services.DTO;
using BookLibrary.ViewModels.Sorting;
using BookLibrary.ViewModels.Filtration;
using BookLibrary.ViewModels.Pagination;

namespace BookLibrary.ViewModels.ManageLibrary
{
    public class AuthorsListViewModel
    {
        public IEnumerable<AuthorDTO> Authors { get; set; }
        public AuthorsFilterViewModel AuthorsFilterVM { get; set; }
        public PageViewModel AuthorsPageVM { get; set; }
        public AuthorsSortViewModel AuthorsSortVM { get; set; }
    }
}
