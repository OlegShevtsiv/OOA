using BookLibrary.Controllers;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.DTO;
using Services.Filters;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using Xunit;


namespace BookLibrary.Tests
{
    public class HomeControllerTests
    {
        private readonly List<BookDTO> _books;
        private readonly List<RateDTO> _rates;
        private readonly List<AuthorDTO> _authors;


        public HomeControllerTests()
        {
            _books = new List<BookDTO>
            {
                new BookDTO { Id = "b1", Title = "First", AuthorId = "a1", Year = 2019, Genre = "g1", Description = "d1", Rate = 4, RatesAmount = 2 },
                new BookDTO { Id = "b2", Title = "Second", AuthorId = "a2", Year = 2014, Genre = "g2", Description = "d2", Rate = 2, RatesAmount = 2 },
                new BookDTO { Id = "b3", Title = "Third", AuthorId = "a3", Year = 2012, Genre = "g3", Description = "d3", Rate = 5, RatesAmount = 1 }
            };

            _authors = new List<AuthorDTO>
            {
                new AuthorDTO { Id = "a1", Name = "N_111", Surname = "SN_111"},
                new AuthorDTO { Id = "a2", Name = "N_222", Surname = "SN_222"},
                new AuthorDTO { Id = "a3", Name = "N_333", Surname = "SN_333"}
            };

            _rates = new List<RateDTO>
            {
                new RateDTO{ Id = "r1", UserId = "u1", BookId = "b1", Value = 3 },
                new RateDTO{ Id = "r2", UserId = "u2", BookId = "b2", Value = 1 },
                new RateDTO{ Id = "r3", UserId = "u3", BookId = "b3", Value = 5 },
                new RateDTO{ Id = "r4", UserId = "u1", BookId = "b2", Value = 3 },
                new RateDTO{ Id = "r5", UserId = "u3", BookId = "b1", Value = 5 }
            };
        }

        [Fact]
        public void IndexReturnsAViewResultWithABooksList()
        {
            // Arrange

            var BookServiceMoq = new Mock<IBookService>();
            var AuthorServiceMoq = new Mock<IAuthorService>();
            var RateServiceMoq = new Mock<IRateService>();

            BookServiceMoq.Setup(service => service.GetAll()).Returns(_books.AsQueryable);
            AuthorServiceMoq.Setup(service => service.GetAll()).Returns(_authors.AsQueryable);
            RateServiceMoq.Setup(service => service.GetAll()).Returns(_rates.AsQueryable);

            HomeController controller = new HomeController(BookServiceMoq.Object, AuthorServiceMoq.Object, RateServiceMoq.Object);


            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<BookDTO>>(viewResult.Model);
            Assert.Equal(3, model.Count());
        }

        [Fact]
        public void SearchTest()
        {
            // Arrange
            var BookServiceMoq = new Mock<IBookService>();
            var AuthorServiceMoq = new Mock<IAuthorService>();
            var RateServiceMoq = new Mock<IRateService>();

            BookServiceMoq.Setup(service => service.GetAll()).Returns(_books.AsQueryable);
            AuthorServiceMoq.Setup(service => service.GetAll()).Returns(_authors.AsQueryable);
            RateServiceMoq.Setup(service => service.GetAll()).Returns(_rates.AsQueryable);

            HomeController controller = new HomeController(BookServiceMoq.Object, AuthorServiceMoq.Object, RateServiceMoq.Object);

            // Act
            RedirectToActionResult result = (RedirectToActionResult)controller.Search("");
            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void RateBookTest()
        {
            //Arrange
            var BookServiceMoq = new Mock<IBookService>();
            var AuthorServiceMoq = new Mock<IAuthorService>();
            var RateServiceMoq = new Mock<IRateService>();

            BookServiceMoq.Setup(service => service.GetAll()).Returns(_books.AsQueryable);
            AuthorServiceMoq.Setup(service => service.GetAll()).Returns(_authors.AsQueryable);
            RateServiceMoq.Setup(service => service.GetAll()).Returns(_rates.AsQueryable);

            HomeController controller = new HomeController(BookServiceMoq.Object, AuthorServiceMoq.Object, RateServiceMoq.Object);
            //Act

            RedirectToActionResult result1 = (RedirectToActionResult)controller.RateBook(new ViewModels.Home.RateViewModel { UserId = "u1", RatedEssenceId = "b1", Value = 1 });
            //Assert

            var viewResult1 = Assert.IsType<RedirectToActionResult>(result1);
        }





    }
}
