﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.DTO;
using BookLibrary.ViewModels.ManageLibrary;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using BookLibrary.Models;
using System.Diagnostics;
using BookLibrary.Client;
using Microsoft.AspNetCore.Http;
using Services.Filters;
using BookLibrary.ViewModels.Sorting.States;
using BookLibrary.ViewModels.Sorting;
using BookLibrary.ViewModels.Pagination;
using BookLibrary.ViewModels.Filtration;
using Newtonsoft.Json;

namespace BookLibrary.Controllers
{
    [Authorize(Roles = "library admin")]
    public class ManageLibraryController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        private readonly IRateService _rateService;
        private readonly ICommentService _commentService;
        private readonly ILibraryHttpDataClient _client;

        public ManageLibraryController(IBookService bookService, IAuthorService authorService, IRateService rateService, ICommentService commentService, ILibraryHttpDataClient client)
        {
            _bookService = bookService;
            _authorService = authorService;
            _commentService = commentService;
            _client = client;
            _rateService = rateService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, SortEnum sortOrder = SortEnum.TITLE_ASC)
        {
            int pageSize = 2;

            List<BookDTO> Books = (await _client.GetData<IEnumerable<BookDTO>>("books/all")).ToList();
            
            switch (sortOrder)
            {
                case SortEnum.TITLE_DESC:
                    Books = Books.OrderByDescending(a => a.Title).ToList();
                    break;
                case SortEnum.YEAR_ASC:
                    Books = Books.OrderBy(a => a.Year).ToList();
                    break;
                case SortEnum.YEAR_DESC:
                    Books = Books.OrderByDescending(a => a.Year).ToList();
                    break;
                case SortEnum.AUTHOR_NAME_ASC:
                    Books = Books.OrderBy(a => _authorService.Get(a.AuthorId).Name).ToList();
                    break;
                case SortEnum.AUTHOR_NAME_DESC:
                    Books = Books.OrderByDescending(a => _authorService.Get(a.AuthorId).Name).ToList();
                    break;
                case SortEnum.AUTHOR_SURNAME_ASC:
                    Books = Books.OrderBy(a => _authorService.Get(a.AuthorId).Surname).ToList();
                    break;
                case SortEnum.AUTHOR_SURNAME_DESC:
                    Books = Books.OrderByDescending(a => _authorService.Get(a.AuthorId).Surname).ToList();
                    break;
                case SortEnum.RATE_ASC:
                    Books = Books.OrderBy(a => a.Rate).ToList();
                    break;
                case SortEnum.RATE_DESC:
                    Books = Books.OrderByDescending(a => a.Rate).ToList();
                    break;
                default:
                    Books = Books.OrderBy(a => a.Title).ToList();
                    break;
            }

            //pagination
            int count = Books.Count;
            var items = Books.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            BooksListViewModel viewModel = new BooksListViewModel
            {
                BooksPageVM = new PageViewModel(count, page, pageSize),
                BooksSortVM = new BooksSortViewModel(sortOrder),
                Books = items
            };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                {
                    return RedirectToAction("Error");
                }
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
                    return RedirectToAction("Error");
                }
                
                string postData = JsonConvert.SerializeObject(newBook);

                await _client.PostData("books", postData);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditBook(string id)
        {
            var getedBook = (await _client.GetData<BookDTO>($"books?id={id}"));
            if (getedBook == null)
            {
                return RedirectToAction("Error");
            }
            BookViewModel model = new BookViewModel {
                Id = getedBook.Id,
                Title = getedBook.Title.Trim(),
                AuthorId = getedBook.AuthorId,
                Rate = getedBook.Rate,
                Year = getedBook.Year,
                Description = getedBook.Description,
                Genre = getedBook.Genre,
                RatesAmount = getedBook.RatesAmount
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditBook(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                {
                    return RedirectToAction("Error");
                }
                BookDTO editedBook = new BookDTO
                {
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
                    return RedirectToAction("Error");
                }

                string postData = JsonConvert.SerializeObject(editedBook);

                await _client.PutData("authors", model.Id, postData);
                
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteBook(string id)
        {
            foreach (var c in _commentService.Get(new CommentFilter { CommentedEssenceId =  id}))
            {
                _commentService.Remove(c.Id);
            }
            foreach (var r in _rateService.Get(new RateFilterByBookId { BookId = id }))
            {
                _rateService.Remove(r.Id);
            }
            _bookService.Remove(id);
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public IActionResult AddAuthor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAuthor(AuthorViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model == null)
                {
                    return RedirectToAction("Error");
                }

                AuthorDTO newAuthor = new AuthorDTO
                {
                    Name = model.Name.Trim(),
                    Description = model.Description,
                    Surname = model.Surname.Trim()
                };
                if (model.Image != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(model.Image.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)model.Image.Length);
                    }
                    newAuthor.Image = imageData;
                }
                else
                {
                    return RedirectToAction("Error");
                }
                string postData = JsonConvert.SerializeObject(newAuthor);

                await _client.PostData("authors", postData);
            }
            
            return RedirectToAction("AuthorsList");
        }

        [HttpGet]
        public async Task<IActionResult> EditAuthor(string id)
        {
            var getedAuthor = (await _client.GetData<AuthorDTO>($"authors?id={id}"));
            if (getedAuthor == null)
            {
                return RedirectToAction("Error");
            }
            AuthorViewModel model = new AuthorViewModel
            {
                Id = getedAuthor.Id.Trim(),
                Name = getedAuthor.Name.Trim(),
                Surname = getedAuthor.Surname.Trim(),
                Description = getedAuthor.Description,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditAuthor(AuthorViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                {
                    throw new Exception();
                }

                AuthorDTO newAuthor = new AuthorDTO
                {
                    Name = model.Name.Trim(),
                    Description = model.Description,
                    Surname = model.Surname.Trim()
                };
                if (model.Image != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(model.Image.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)model.Image.Length);
                    }
                    newAuthor.Image = imageData;
                }
                else
                {
                    return RedirectToAction("Error");
                }

                string postData = JsonConvert.SerializeObject(newAuthor);

                await _client.PutData("authors", model.Id, postData);
            }
            return RedirectToAction("AuthorsList");
        }

        [HttpPost]
        public IActionResult DeleteAuthor(string id)
        {
            foreach (var c in _commentService.Get(new CommentFilter { CommentedEssenceId = id }))
            {
                _commentService.Remove(c.Id);
            }
            _authorService.Remove(id);
            return RedirectToAction("AuthorsList");
        }

        public async Task<IActionResult> AuthorsList(int page = 1, SortEnum sortOrder = SortEnum.NAME_ASC)
        {
            int pageSize = 2;

            List<AuthorDTO> Authors = (await _client.GetData<IEnumerable<AuthorDTO>>("authors/all")).ToList();
            
            switch(sortOrder)
            {
                case SortEnum.NAME_DESC:
                    Authors = Authors.OrderByDescending(a => a.Name).ToList();
                    break;
                case SortEnum.SURNAME_ASC:
                    Authors = Authors.OrderBy(a => a.Surname).ToList();
                    break;
                case SortEnum.SURNAME_DESC:
                    Authors = Authors.OrderByDescending(a => a.Surname).ToList();
                    break;
                default:
                    Authors = Authors.OrderBy(a => a.Name).ToList();
                    break;
            }

            //pagination
            int count = Authors.Count;
            var items = Authors.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            AuthorsListViewModel viewModel = new AuthorsListViewModel
            {
                AuthorsPageVM = new PageViewModel(count, page, pageSize),
                AuthorsSortVM = new AuthorsSortViewModel(sortOrder),
                Authors = items
            };
            return View(viewModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}