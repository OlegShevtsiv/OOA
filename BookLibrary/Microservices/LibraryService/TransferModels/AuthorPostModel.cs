using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LibraryService.TransferModels
{
    public class AuthorPostModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public string Description { get; set; }
        public  byte[] Image { get; set; }
    }
}