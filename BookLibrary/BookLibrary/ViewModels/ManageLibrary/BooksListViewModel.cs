using BookLibrary.ViewModels.Filtration;
using BookLibrary.ViewModels.Pagination;
using BookLibrary.ViewModels.Sorting;
using Services.DTO;
using System.Collections.Generic;

namespace BookLibrary.ViewModels.ManageLibrary
{
    public class BooksListViewModel
    {
        public IEnumerable<BookDTO> Books { get; set; }
        public BooksFilterViewModel BooksFilterVM { get; set; }
        public PageViewModel BooksPageVM { get; set; }
        public BooksSortViewModel BooksSortVM { get; set; }
    }
}
