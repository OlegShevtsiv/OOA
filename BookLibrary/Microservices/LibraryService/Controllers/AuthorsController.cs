using System.ComponentModel.DataAnnotations;
using System.IO;
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
    [Route("authors/")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorDataWriter _dataWriter;
        private readonly IAuthorProvider _provider;
        private readonly ICommentDataWriter _commentDataWriter;
        private readonly ICommentProvider _commentProvider;
        
        public AuthorsController(IAuthorDataWriter dataWriter, IAuthorProvider provider, ICommentDataWriter commentDataWriter, ICommentProvider commentProvider)
        {
            _dataWriter = dataWriter;
            _provider = provider;
            _commentDataWriter = commentDataWriter;
            _commentProvider = commentProvider;
        }

        [HttpGet("")]
        public IActionResult Get([Required]string id)
        {
            AuthorDTO currentAuthor = _provider.Get(id);
            return new ObjectResult(currentAuthor);
        }
        
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var currentBooks = _provider.GetAll();

            return new ObjectResult(currentBooks);
        }
        
        [HttpPost("")]
        [ProducesResponseType(typeof(AuthorPostModel), (int)HttpStatusCode.OK)]
        public IActionResult Post([Required][FromBody]AuthorPostModel model)
        {
            if (ModelState.IsValid)
            {
                AuthorDTO newAuthor = new AuthorDTO
                {
                    Name = model.Name.Trim(),
                    Description = model.Description,
                    Surname = model.Surname.Trim(),
                    Image = model.Image
                };
                
                _dataWriter.Add(newAuthor);
            }

            return Ok();
        }
        
        [HttpPut("")]
        [ProducesResponseType(typeof(AuthorPostModel), (int)HttpStatusCode.OK)]
        public IActionResult Put([Required]string id, [Required][FromBody]AuthorPostModel model)
        {
            if (ModelState.IsValid)
            {
                AuthorDTO newAuthor = new AuthorDTO
                {
                    Id = id,
                    Name = model.Name.Trim(),
                    Description = model.Description,
                    Surname = model.Surname.Trim(),
                    Image = model.Image
                };
                
                _dataWriter.Update(newAuthor);
            }

            return Ok();
        }
        
        [HttpDelete("")]
        public IActionResult Delete([Required]string id)
        {
            foreach (var c in _commentProvider.Get(new CommentFilter { CommentedEssenceId = id }))
            {
                _commentDataWriter.Remove(c.Id);
            }
            _dataWriter.Remove(id);
            return Ok();
        }
    }
}