using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BookLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Interfaces;
using BookLibrary.ViewModels.ManageComments;

namespace BookLibrary.Controllers
{
    public class ManageCommentsController : Controller
    {
        
        private readonly ICommentService _commentService;

        public ManageCommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddComment(CommentViewModel newComment)
        {
            if (newComment == null)
            {
                return RedirectToAction("Error");
            }

            CommentDTO comment = new CommentDTO
            {
                OwnerId = newComment.OwnerId,
                CommentedEssenceId = newComment.EssenceId,
                Text = newComment.Text,
                Time = DateTime.Now
            };
            _commentService.Add(comment);
            if (newComment.IsBook)
            {
                return RedirectToAction("GetBookInfo", "Home", new { id = newComment.EssenceId });
            }
            else
            {
                return RedirectToAction("GetAuthorInfo", "Home", new { id = newComment.EssenceId });
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditComment(CommentViewModel commentToEdit, string text)
        {
           
            if (commentToEdit == null)
            {
                return RedirectToAction("Error");
            }
            if (string.IsNullOrEmpty(text))
            {
                _commentService.Remove(commentToEdit.Id);
                goto loop;
            }

            CommentDTO updatedComment = new CommentDTO
            {
                Id = commentToEdit.Id,
                OwnerId = commentToEdit.OwnerId,
                CommentedEssenceId = commentToEdit.EssenceId,
                Text = text,
                Time = commentToEdit.Time
            };
            _commentService.Update(updatedComment);
            loop:
            if (commentToEdit.IsBook)
            {
                return RedirectToAction("GetBookInfo", "Home", new { id = commentToEdit.EssenceId });
            }
            else
            {
                return RedirectToAction("GetAuthorInfo", "Home", new { id = commentToEdit.EssenceId });
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteComment(CommentViewModel commentToRemove)
        {
            if (commentToRemove == null)
            {
                return RedirectToAction("Error");
            }
            
            if (_commentService.Get(commentToRemove.Id) == null)
            {
                return RedirectToAction("Error");
            }
            _commentService.Remove(commentToRemove.Id);
            if (commentToRemove.IsBook)
            {
                return RedirectToAction("GetBookInfo", "Home", new { id = commentToRemove.EssenceId });
            }
            else
            {
                return RedirectToAction("GetAuthorInfo", "Home", new { id = commentToRemove.EssenceId });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}