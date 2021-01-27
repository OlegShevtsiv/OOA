using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Library.DataAccess.DTO;
using Library.DataProviders.Filters;
using Library.DataProviders.Interfaces;
using Library.DataWriters.Interfaces;
using LibraryService.TransferModels;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.Controllers
{
    [ApiController]
    [Route("comments/")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentDataWriter _dataWriter;
        private readonly ICommentProvider _provider;

        
        public CommentsController(ICommentDataWriter dataWriter, ICommentProvider provider)
        {
            _dataWriter = dataWriter;
            _provider = provider;
        }
        
        [HttpGet("byEssence")]
        public IActionResult GetByEssence([Required]string essenceId)
        {
            var currentBooks = _provider.Get(new CommentFilter{CommentedEssenceId = essenceId});

            return new ObjectResult(currentBooks);
        }
        
        [HttpGet("byUser")]
        public IActionResult GetByUser([Required]string userId)
        {
            var currentBooks = _provider.Get(new CommentFilterByOwnerId{OwnerId = userId});

            return new ObjectResult(currentBooks);
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(CommentPostModel), (int)HttpStatusCode.OK)]
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

        [HttpPut("")]
        [ProducesResponseType(typeof(CommentPostModel), (int)HttpStatusCode.OK)]
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

        [HttpDelete("")]
        public IActionResult Delete([Required]string id)
        {
            _dataWriter.Remove(id);
            return Ok();
        }
    }
}