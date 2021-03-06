using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LibraryService.TransferModels
{
    public class BookPostModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string AuthorId { get; set; }
        public int Year { get; set; }
        public byte[] Image { get; set; }
        public byte[] FileBook { get; set; }
        [Required]
        public string Genre { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public uint RatesAmount { get; set; }
    }
}