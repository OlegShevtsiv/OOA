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
    [Route("books/")]
    public class BooksController : ControllerBase
    {
        private readonly IBookDataWriter _dataWriter;
        private readonly IBookProvider _provider;
        private readonly ICommentDataWriter _commentDataWriter;
        private readonly ICommentProvider _commentProvider;
        private readonly IRateDataWriter _rateDataWriter;
        private readonly IRateProvider _rateProvider;
        
        public BooksController(IBookDataWriter dataWriter, IBookProvider provider, ICommentDataWriter commentDataWriter, ICommentProvider commentProvider, IRateDataWriter rateDataWriter, IRateProvider rateProvider)
        {
            _dataWriter = dataWriter;
            _provider = provider;
            _commentDataWriter = commentDataWriter;
            _commentProvider = commentProvider;
            _rateDataWriter = rateDataWriter;
            _rateProvider = rateProvider;
        }
        
        [HttpGet("")]
        public IActionResult Get([Required]string id)
        {
            BookDTO currentBook = _provider.Get(id);

            return new ObjectResult(currentBook);
        }
        
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var currentBooks = _provider.GetAll();

            return new ObjectResult(currentBooks);
        }
        
        [HttpPost("")]
        [ProducesResponseType(typeof(BookPostModel), (int)HttpStatusCode.OK)]
        public IActionResult Post([Required][FromBody]BookPostModel model)
        {
            if (ModelState.IsValid)
            {
                BookDTO newBook = new BookDTO
                {
                    Title = model.Title.Trim(),
                    AuthorId = model.AuthorId,
                    Genre = model.Genre.Trim(),
                    Rate = model.Rate,
                    Description = model.Description,
                    Year = model.Year,
                    RatesAmount = model.RatesAmount,
                    Image = model.Image,
                    FileBook = model.FileBook
                };

                _dataWriter.Add(newBook);
            }

            return Ok();
        }
        
        [HttpPut("")]
        [ProducesResponseType(typeof(BookPostModel), (int)HttpStatusCode.OK)]
        public IActionResult Put([Required]string id, [Required][FromBody]BookPostModel model)
        {
            if (ModelState.IsValid)
            {
                BookDTO editedBook = new BookDTO
                {
                    Id = id,
                    Title = model.Title.Trim(),
                    AuthorId = model.AuthorId,
                    Genre = model.Genre,
                    Rate = model.Rate,
                    Description = model.Description,
                    Year = model.Year,
                    RatesAmount = model.RatesAmount,
                    Image = model.Image,
                    FileBook = model.FileBook
                };
                
                _dataWriter.Update(editedBook);
            }

            return Ok();
        }
        
        [HttpDelete("")]
        public IActionResult Delete([Required]string id)
        {
            foreach (var c in _commentProvider.Get(new CommentFilter { CommentedEssenceId =  id}))
            {
                _commentDataWriter.Remove(c.Id);
            }
            foreach (var r in _rateProvider.Get(new RateFilterByBookId { BookId = id }))
            {
                _rateDataWriter.Remove(r.Id);
            }
            _dataWriter.Remove(id);
            return Ok();
        }

        [HttpGet("download")]
        public IActionResult DownloadBook([Required]string id)
        {
            BookDTO book = _provider.Get(id);
            if (book == null)
            {
                return BadRequest();
            }
            string file_type = "application/pdf";
            string file_name = book.Title + ".pdf";

            return File(book.FileBook, file_type, file_name);
        }
    }
}