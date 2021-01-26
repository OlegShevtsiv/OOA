using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.Models
{
    public class Comment
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }
        public string CommentedEssenceId { get; set; }
        [DataType(DataType.Date)]
        public DateTime Time { get; set; }
        public string Text { get; set; }
        public Comment() { }
        public Comment(string _id, string _ownerId, string _commentedEssenceId, DateTime _time, string _text)
        {
            Id = _id;
            OwnerId = _ownerId;
            CommentedEssenceId = _commentedEssenceId;
            Time = _time;
            Text = _text;
        }
    }
}
