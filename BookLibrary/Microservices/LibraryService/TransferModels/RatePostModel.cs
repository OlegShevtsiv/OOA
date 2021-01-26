using System.ComponentModel.DataAnnotations;

namespace LibraryService.TransferModels
{
    public class RatePostModel
    {
        [Required]
        public string UserId { get; set; }
        
        [Required]
        public string RatedEssenceId { get; set; }
        
        [Required]
        public decimal Value { get; set; }
    }
}