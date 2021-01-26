using System.ComponentModel.DataAnnotations;

namespace LibraryService.TransferModels
{
    public class CommentPostModel
    {
        [Required]
        public string OwnerId { get; set; }
        [Required]
        public string EssenceId { get; set; }
        [Required]
        public string Text { get; set; }
    }
}