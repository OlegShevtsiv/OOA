using System.ComponentModel.DataAnnotations;
using System.IO;
using Library.DataAccess.DTO;
using Library.DataProviders.Filters;
using Library.DataProviders.Interfaces;
using Library.DataWriters.Interfaces;
using LibraryService.TransferModels;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.Controllers
{
    [ApiController]
    [Route("books/[action]")]
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
        
        [HttpGet]
        public IActionResult Get([Required]string id)
        {
            BookDTO currentBook = _provider.Get(id);
            if (currentBook == null)
            {
                return BadRequest();
            }

            return new ObjectResult(currentBook);
        }
        
        [HttpPost]
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
                    RatesAmount = model.RatesAmount
                };
                if (model.Image != null && model.FileBook != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(model.Image.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)model.Image.Length);
                    }
                    newBook.Image = imageData;

                    byte[] fileData = null;
                    using (var binaryReader = new BinaryReader(model.FileBook.OpenReadStream()))
                    {
                        fileData = binaryReader.ReadBytes((int)model.FileBook.Length);
                    }
                    newBook.FileBook = fileData;
                }
                else
                {
                    return BadRequest();
                }

                _dataWriter.Add(newBook);
            }

            return Ok();
        }
        
        [HttpPut]
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
                    RatesAmount = model.RatesAmount
                };
                if (model.Image != null && model.FileBook != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(model.Image.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)model.Image.Length);
                    }
                    editedBook.Image = imageData;

                    byte[] fileData = null;
                    using (var binaryReader = new BinaryReader(model.FileBook.OpenReadStream()))
                    {
                        fileData = binaryReader.ReadBytes((int)model.FileBook.Length);
                    }
                    editedBook.FileBook = fileData;
                }
                else
                {
                    return BadRequest();
                }

                _dataWriter.Update(editedBook);
            }

            return Ok();
        }
        
        [HttpDelete]
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

        [HttpGet]
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