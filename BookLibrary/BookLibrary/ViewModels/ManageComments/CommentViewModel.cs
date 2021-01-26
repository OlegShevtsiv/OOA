using System;
namespace BookLibrary.ViewModels.ManageComments
{
    public class CommentViewModel
    {
        public string Id { get; set; }
        public DateTime Time { get; set; }
        public string OwnerId { get; set; }
        public string EssenceId { get; set; }
        public bool IsBook { get; set; }
        public string Text { get; set; }
    }
}
