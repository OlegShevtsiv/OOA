using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Microsoft.EntityFrameworkCore.Extensions;
using DataAccess.Implementation;

namespace DataAccess.Tests
{
    public class BookRepositoryTests
    {
        private readonly List<Book> _books;
        private const int _oneElement = 1;
        private LibraryContext _LibContext;
        private IUnitOfWork _unitOfWork;

        public BookRepositoryTests()
        {
            _books = new List<Book>
            {
                new Book { Id = "b1", Title = "First", AuthorId = "a1"},
                new Book { Id = "b2", Title = "Second", AuthorId = "a2"},
                new Book { Id = "b3", Title = "Third", AuthorId = "a3"}
            };

            _LibContext = GetContext();
            _unitOfWork = new UnitOfWork(_LibContext);
        }

        [Fact]
        public void AddTest()
        {
            // Act
            _unitOfWork.GetRepository<Book>().Add(_books[0]);
            _unitOfWork.SaveChanges();

            // Assert
            Assert.Equal(_oneElement, _LibContext.Books.Count());
            Assert.Equal(_books[0].Title, _LibContext.Books.Single().Title);
            Assert.Equal(_books[0].Id, _LibContext.Books.Single().Id);
            Assert.IsType<Book>(_LibContext.Books.Single());
        }

        [Fact]
        public void AddRangeTest()
        {
            // Act
            _unitOfWork.GetRepository<Book>().Add(_books);
            _unitOfWork.SaveChanges();

            // Assert
            Assert.Equal(_books.Count, _LibContext.Books.Count());
            foreach (var project in _books)
            {
                Assert.Contains(project, _LibContext.Books);
            }
        }

        [Fact]
        public void RemoveTest()
        {
            // Act
            _unitOfWork.GetRepository<Book>().Add(_books[0]);
            _unitOfWork.SaveChanges();

            Assert.Equal(_oneElement, _LibContext.Books.Count());

            _unitOfWork.GetRepository<Book>().Remove(_LibContext.Books.Single());
            _unitOfWork.SaveChanges();

            // Assert
            Assert.Empty(_LibContext.Books);
        }

        [Fact]
        public void RemoveRangeTest()
        {
            // Act
            _unitOfWork.GetRepository<Book>().Add(_books);
            _unitOfWork.SaveChanges();

            Assert.Equal(_books.Count, _LibContext.Books.Count());

            _unitOfWork.GetRepository<Book>().Remove(_books);
            _unitOfWork.SaveChanges();

            // Assert
            Assert.Empty(_LibContext.Books);
        }

        [Fact]
        public void GetTest()
        {
            // Arange
            _unitOfWork.GetRepository<Book>().Add(_books);
            _unitOfWork.SaveChanges();

            // Act
            var result = _unitOfWork.GetRepository<Book>().Get();

            //Assert
            Assert.Equal(_books.Count, result.Count());
            foreach (var project in _books)
            {
                Assert.Contains(project, result);
            }
        }

        [Fact]
        public void GetPredicateTest()
        {
            // Arange
            _unitOfWork.GetRepository<Book>().Add(_books);
            _unitOfWork.SaveChanges();
            Expression<Func<Book, bool>> predicate = a => a.Id.CompareTo("b1") > 0;
            List<Book> filtredProjects = _books.Where(predicate.Compile()).ToList();

            // Act
            var result = _unitOfWork.GetRepository<Book>().Get(predicate);

            //Assert
            Assert.Equal(filtredProjects.Count, result.Count());
            foreach (var project in filtredProjects)
            {
                Assert.Contains(project, result);
            }
        }

        private LibraryContext GetContext()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dataContext = new LibraryContext(options);
            dataContext.Database.EnsureCreated();
            return dataContext;
        }
    }
}
