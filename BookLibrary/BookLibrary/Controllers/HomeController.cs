using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BookLibrary.Client;
using Microsoft.AspNetCore.Mvc;
using BookLibrary.Models;
using Services.Interfaces;
using Services.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BookLibrary.ViewModels.Home;
using Newtonsoft.Json;

namespace BookLibrary.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        private readonly IRateService _rateService;
        private readonly ILibraryHttpDataClient _client;

        public HomeController(IBookService bookService, IAuthorService authorService, IRateService rateService, ILibraryHttpDataClient client)
        {
            _bookService = bookService;
            _authorService = authorService;
            _rateService = rateService;
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var books = (await _client.GetData<IEnumerable<BookDTO>>("books/all")).ToList().OrderByDescending(b => b.Rate).ToList();
            return View(books);
        }
      
        [HttpPost]
        public IActionResult Search(string req)
        {
            if (string.IsNullOrEmpty(req))
            {
                return RedirectToAction("Index");
            }
            List<BookDTO> param = new List<BookDTO>();
            List<string> keys = req.Trim().Split(' ').ToList();
            List<BookDTO> allBooks = _bookService.GetAll().ToList();
            for(int i = 0; i < keys.Count; i++)
            {
                keys[i] = keys[i].ToLower().Trim();
                foreach  (BookDTO book  in allBooks)
                {
                    if (book.Title.ToLower().Contains(keys[i]))
                    {
                        if (!param.Exists(b => b.Id == book.Id))
                        {
                            param.Add(book);
                        }
                    }
                    if (book.Year.ToString() == keys[i])
                    {
                        if (!param.Exists(b => b.Id == book.Id))
                        {
                            param.Add(book);
                        }
                    }
                    if (book.Genre.ToLower().ToString().Contains(keys[i]))
                    {
                        if (!param.Exists(b => b.Id == book.Id))
                        {
                            param.Add(book);
                        }
                    }
                    if (_authorService.Get(book.AuthorId).Name.ToLower() == keys[i])
                    {
                        if (!param.Exists(b => b.Id == book.Id))
                        {
                            param.Add(book);
                        }
                    }
                    if (_authorService.Get(book.AuthorId).Surname.ToLower() == keys[i])
                    {
                        if (!param.Exists(b => b.Id == book.Id))
                        {
                            param.Add(book);
                        }
                    }
                }
            }

            return View("~/Views/Home/Index.cshtml", param.ToList());
        }

        [HttpGet]
        public  async Task<IActionResult> GetAuthorInfo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Error");
            }

            var currentAuthor = (await _client.GetData<AuthorDTO>($"authors?id={id}"));
            if (currentAuthor == null)
            {
                return RedirectToAction("Error");
            }
            return View(currentAuthor);
        }

        [HttpGet]
        public  async Task<IActionResult> GetBookInfo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Error");
            }
            var currentBook = (await _client.GetData<BookDTO>($"books?id={id}"));
            if (currentBook == null)
            {
                return RedirectToAction("Error");
            }
            return View(currentBook);
        }

        [Authorize]
        public IActionResult DownloadBook(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                RedirectToAction("Error");
            }
            BookDTO book = _bookService.Get(id);
            if (book == null)
            {
                return RedirectToAction("Error");
            }
            string file_type = "application/pdf";
            string file_name = book.Title + ".pdf";

            return File(book.FileBook, file_type, file_name);
        }


        [Authorize]
        [HttpPost]
        public  async Task<IActionResult> RateBook(RateViewModel rateVM)
        {
            if (string.IsNullOrEmpty(rateVM.UserId) || string.IsNullOrEmpty(rateVM.RatedEssenceId) || rateVM.Value < 1 || rateVM.Value > 5)
            {
                return RedirectToAction("Error");
            }
            
            string postData = JsonConvert.SerializeObject(rateVM);
            await _client.PostData("rates/rateBook", postData);
            
            return RedirectToAction("GetBookInfo", "Home", new { id = rateVM.RatedEssenceId });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
