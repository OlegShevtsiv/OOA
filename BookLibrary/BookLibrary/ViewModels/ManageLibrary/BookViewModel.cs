using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.ViewModels.ManageLibrary
{
    public class BookViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string AuthorId { get; set; }
        public int Year { get; set; }
        public IFormFile Image { get; set; }
        public IFormFile FileBook { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public uint RatesAmount { get; set; }
    }
}
