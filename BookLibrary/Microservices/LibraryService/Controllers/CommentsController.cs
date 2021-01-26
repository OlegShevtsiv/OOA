using System;
using System.ComponentModel.DataAnnotations;
using Library.DataAccess.DTO;
using Library.DataWriters.Interfaces;
using LibraryService.TransferModels;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.Controllers
{
    [ApiController]
    [Route("comments/[action]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentDataWriter _dataWriter;
        
        public CommentsController(ICommentDataWriter dataWriter)
        {
            _dataWriter = dataWriter;
        }

        [HttpPost]
        public IActionResult Post([Required][FromBody]CommentPostModel newComment)
        {
            if (newComment == null)
            {
                return BadRequest();
            }

            CommentDTO comment = new CommentDTO
            {
                OwnerId = newComment.OwnerId,
                CommentedEssenceId = newComment.EssenceId,
                Text = newComment.Text,
                Time = DateTime.Now
            };
            _dataWriter.Add(comment);

            return Ok();
        }

        [HttpPut]
        public IActionResult Put([Required]string id, [Required][FromBody]CommentPostModel commentToEdit)
        {
           
            if (commentToEdit == null)
            {
                return BadRequest();
            }
            if (string.IsNullOrEmpty(commentToEdit.Text))
            {
                _dataWriter.Remove(id);
                return Ok();
            }

            CommentDTO updatedComment = new CommentDTO
            {
                Id = id,
                OwnerId = commentToEdit.OwnerId,
                CommentedEssenceId = commentToEdit.EssenceId,
                Text = commentToEdit.Text,
                Time = DateTime.Now
            };
            _dataWriter.Update(updatedComment);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([Required]string id)
        {
            _dataWriter.Remove(id);
            return Ok();
        }
    }
}