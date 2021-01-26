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
    public class AuthorServiceTests
    {
        private readonly List<Author> authors;

        public AuthorServiceTests()
        {
            authors = new List<Author>
            {
                new Author{ Id ="a1",Name="Name1",Surname="Surname1",Description="Description1",Image=null },
                new Author{ Id ="a2",Name="Name2",Surname="Surname2",Description="Description2",Image=null },
                new Author{ Id ="a3",Name="Name3",Surname="Surname3",Description="Description3",Image=null },
                new Author{ Id ="a4",Name="Name4",Surname="Surname4",Description="Description4",Image=null },
                new Author{ Id ="a5",Name="Name5",Surname="Surname5",Description="Description5",Image=null }
            };
        }
        [Fact]
        public void GetByFilterTest()
        {
            //Arrange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Author>> repositoryMock = new Mock<IRepository<Author>>();
            unitOfWorkMock.Setup(getRepository => getRepository.GetRepository<Author>()).Returns(repositoryMock.Object);
            repositoryMock.Setup(rep => rep.Get(It.IsAny<Expression<Func<Author, bool>>>())).Returns(authors.AsQueryable);
            AuthorService authorService = new AuthorService(unitOfWorkMock.Object);
            AuthorFilter authorfilter = new AuthorFilter();


            //Act

            IEnumerable<AuthorDTO> authorsDto = authorService.Get(authorfilter);

            //Assert
            Assert.NotNull(authorsDto);
            Assert.NotEmpty(authorsDto);
            Assert.Equal(5, authorsDto.Count());

        }

        [Fact]
        public void GetAllTest()
        {
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Author>> repositoryMock = new Mock<IRepository<Author>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Author, bool>>>())).Returns(authors.AsQueryable);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Author>()).Returns(repositoryMock.Object);
            AuthorService authorService = new AuthorService(unitOfWorkMock.Object);

            //Act
            IEnumerable<AuthorDTO> autohrsDto = authorService.GetAll();

            //Assert
            Assert.NotNull(autohrsDto);
            Assert.NotEmpty(autohrsDto);
            Assert.Equal(5, autohrsDto.Count());
        }

        [Fact]
        public void GetByIdTest()
        {
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Author>> repositoryMock = new Mock<IRepository<Author>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Author, bool>>>()))
                 .Returns<Expression<Func<Author, bool>>>(predicate =>
                     authors.Where(predicate.Compile()).AsQueryable());
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Author>()).Returns(repositoryMock.Object);
            AuthorService authorService = new AuthorService(unitOfWorkMock.Object);

            //Act
            AuthorDTO authorDto = authorService.Get("a2");

            //Assert
            Assert.NotNull(authorDto);
            Assert.Equal("Surname2", authorDto.Surname);
        }

        [Fact]
        public void AddTest()
        {
            //Arange
            AuthorDTO newAutohorDTO = new AuthorDTO()
            {
                Id = "a0",
                Name = "Name0"
            };
            bool isAdded = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Author>> repositoryMock = new Mock<IRepository<Author>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Author, bool>>>()))
                .Returns<Expression<Func<Author, bool>>>(predicate =>
                    authors.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Add(It.IsAny<Author>())).Callback(() => isAdded = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Author>()).Returns(repositoryMock.Object);
            AuthorService authorService = new AuthorService(unitOfWorkMock.Object);

            //Act
            authorService.Add(newAutohorDTO);

            //Assert
            Assert.True(isAdded);
        }

        [Fact]
        public void RemoveTest()
        {
            //Arange
            bool isRemoved = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Author>> repositoryMock = new Mock<IRepository<Author>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Author, bool>>>()))
                .Returns<Expression<Func<Author, bool>>>(predicate =>
                   authors.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Remove(It.IsAny<Author>())).Callback(() => isRemoved = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Author>()).Returns(repositoryMock.Object);
            AuthorService authorsService = new AuthorService(unitOfWorkMock.Object);

            //Act
            authorsService.Remove(authors[0].Id);

            //Assert
            Assert.True(isRemoved);
        }

        [Fact]
        public void UpdateTest()
        {
            //Arange
            AuthorDTO updateDTO = new AuthorDTO()
            {
                Id = "a1",
                Name = "NameUpdated"
            };
            bool isUpdate = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Author>> repositoryMock = new Mock<IRepository<Author>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Author, bool>>>()))
                .Returns<Expression<Func<Author, bool>>>(predicate =>
                    authors.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Update(It.Is<Author>(entity =>
                    (entity.Id == updateDTO.Id) &&
                    (entity.Name == updateDTO.Name))))
                .Callback(() => isUpdate = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Author>()).Returns(repositoryMock.Object);
            AuthorService authorsService = new AuthorService(unitOfWorkMock.Object);

            //Act
            authorsService.Update(updateDTO);

            //Assert
            Assert.True(isUpdate);
        }
    }
}


