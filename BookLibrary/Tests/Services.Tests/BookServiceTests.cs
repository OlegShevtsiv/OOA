using DataAccess.Interfaces;
using DataAccess.Models;
using Moq;
using Services.DTO;
using Services.Filters;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Services.Tests
{
    public class BookServiceTests
    {
        private readonly List<Book> _books;

        public BookServiceTests()
        {
            _books = new List<Book>
            {
                new Book { Id = "b1", Title = "First", AuthorId = "a1"},
                new Book { Id = "b2", Title = "Second", AuthorId = "a2"},
                new Book { Id = "b3", Title = "Third", AuthorId = "a3"}
            };
        }

        [Fact]
        public void GetByFilterTest()
        {
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Book>> repositoryMock = new Mock<IRepository<Book>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Book, bool>>>())).Returns(_books.AsQueryable);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Book>()).Returns(repositoryMock.Object);
            BookService _bookService = new BookService(unitOfWorkMock.Object);
            BookFilter _bookFilter = new BookFilter();

            //Act
            IEnumerable<BookDTO> _booksDto = _bookService.Get(_bookFilter);

            //Assert
            Assert.NotNull(_booksDto);
            Assert.NotEmpty(_booksDto);
            Assert.Equal(3, _booksDto.Count());
        }

        [Fact]

        public void GetAllTest()
        {
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Book>> repositoryMock = new Mock<IRepository<Book>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Book, bool>>>())).Returns(_books.AsQueryable);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Book>()).Returns(repositoryMock.Object);
            BookService _bookService = new BookService(unitOfWorkMock.Object);

            //Act
            IEnumerable<BookDTO> _booksDto = _bookService.GetAll();

            //Assert
            Assert.NotNull(_booksDto);
            Assert.NotEmpty(_booksDto);
            Assert.Equal(3, _booksDto.Count());
        }

        [Fact]
        public void GetByIdTest()
        {
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Book>> repositoryMock = new Mock<IRepository<Book>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Book, bool>>>()))
                .Returns<Expression<Func<Book, bool>>>(predicate =>
                    _books.Where(predicate.Compile()).AsQueryable());
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Book>()).Returns(repositoryMock.Object);
            BookService _bookService = new BookService(unitOfWorkMock.Object);

            //Act
            BookDTO _bookDto = _bookService.Get("b1");

            //Assert
            Assert.NotNull(_bookDto);
            Assert.Equal("First", _bookDto.Title);
        }

        [Fact]
        public void AddTest()
        {
            //Arange
            BookDTO projectDto = new BookDTO()
            {
                Id = "b0",
                Title = "Zero"
            };
            bool isAdded = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Book>> repositoryMock = new Mock<IRepository<Book>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Book, bool>>>()))
                .Returns<Expression<Func<Book, bool>>>(predicate =>
                    _books.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Add(It.IsAny<Book>())).Callback(() => isAdded = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Book>()).Returns(repositoryMock.Object);
            BookService _bookService = new BookService(unitOfWorkMock.Object);

            //Act
            _bookService.Add(projectDto);

            //Assert
            Assert.True(isAdded);
        }

        [Fact]
        public void RemoveTest()
        {
            //Arange
            bool isRemoved = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Book>> repositoryMock = new Mock<IRepository<Book>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Book, bool>>>()))
                .Returns<Expression<Func<Book, bool>>>(predicate =>
                    _books.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Remove(It.IsAny<Book>())).Callback(() => isRemoved = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Book>()).Returns(repositoryMock.Object);
            BookService _bookService = new BookService(unitOfWorkMock.Object);

            //Act
            _bookService.Remove(_books[0].Id);

            //Assert
            Assert.True(isRemoved);
        }

        [Fact]
        public void UpdateTest()
        {
            //Arange
            BookDTO projectDto = new BookDTO()
            {
                Id = "b1",
                Title = "FirstUpdated"
            };
            bool isUpdate = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Book>> repositoryMock = new Mock<IRepository<Book>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Book, bool>>>()))
                .Returns<Expression<Func<Book, bool>>>(predicate =>
                    _books.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Update(It.Is<Book>(entity =>
                    (entity.Id == projectDto.Id) &&
                    (entity.Title == projectDto.Title))))
                .Callback(() => isUpdate = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Book>()).Returns(repositoryMock.Object);
            BookService _bookService = new BookService(unitOfWorkMock.Object);

            //Act
            _bookService.Update(projectDto);

            //Assert
            Assert.True(isUpdate);
        }
    }
}
