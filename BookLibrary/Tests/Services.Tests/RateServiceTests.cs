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
    public class RateServiceTests
    {
        private readonly List<Rate> _rates;

        public RateServiceTests()
        {
            _rates = new List<Rate>
            {
                new Rate {  Id = "id1", UserId = "userId1", BookId = "bookId1", Value = 1},
                new Rate {  Id = "id2", UserId = "userId2", BookId = "bookId2", Value = 2},
                new Rate {  Id = "id3", UserId = "userId3", BookId = "bookId3", Value = 3}
            };
        }

        [Fact]
        public void GetByFilterByUserIdTest()
        {
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Rate>> repositoryMock = new Mock<IRepository<Rate>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Rate, bool>>>())).Returns(_rates.AsQueryable);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Rate>()).Returns(repositoryMock.Object);
            RateService _rateService = new RateService(unitOfWorkMock.Object);
            RateFilterByUserId _rateFilter = new RateFilterByUserId();

            //Act
            IEnumerable<RateDTO> _ratesDto = _rateService.Get(_rateFilter);

            //Assert
            Assert.NotNull(_ratesDto);
            Assert.NotEmpty(_ratesDto);
            Assert.Equal(3, _ratesDto.Count());
        }


        [Fact]
        public void GetByFilterByBookIdTest()
        {
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Rate>> repositoryMock = new Mock<IRepository<Rate>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Rate, bool>>>())).Returns(_rates.AsQueryable);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Rate>()).Returns(repositoryMock.Object);
            RateService _rateService = new RateService(unitOfWorkMock.Object);
            RateFilterByBookId _rateFilter = new RateFilterByBookId();

            //Act
            IEnumerable<RateDTO> _ratesDto = _rateService.Get(_rateFilter);

            //Assert
            Assert.NotNull(_ratesDto);
            Assert.NotEmpty(_ratesDto);
            Assert.Equal(3, _ratesDto.Count());
        }

        [Fact]
        public void GetAllTest()
        {
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Rate>> repositoryMock = new Mock<IRepository<Rate>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Rate, bool>>>())).Returns(_rates.AsQueryable);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Rate>()).Returns(repositoryMock.Object);
            RateService _rateService = new RateService(unitOfWorkMock.Object);

            //Act
            IEnumerable<RateDTO> _ratesDto = _rateService.GetAll();

            //Assert
            Assert.NotNull(_ratesDto);
            Assert.NotEmpty(_ratesDto);
            Assert.Equal(3, _ratesDto.Count());
        }

        [Fact]
        public void GetByIdTest()
        {
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Rate>> repositoryMock = new Mock<IRepository<Rate>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Rate, bool>>>()))
                .Returns<Expression<Func<Rate, bool>>>(predicate =>
                    _rates.Where(predicate.Compile()).AsQueryable());
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Rate>()).Returns(repositoryMock.Object);
            RateService _rateService = new RateService(unitOfWorkMock.Object);

            //Act
            RateDTO _rateDto = _rateService.Get("id1");

            //Assert
            Assert.NotNull(_rateDto);
            Assert.Equal(1, _rateDto.Value);
        }

        [Fact]
        public void AddTest()
        {
            RateDTO projectDto = new RateDTO()
            {
                Id = "userId1",
                UserId = "userId1",
                BookId = "bookId1",
                Value = 1
            };
            bool isAdded = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Rate>> repositoryMock = new Mock<IRepository<Rate>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Rate, bool>>>()))
                .Returns<Expression<Func<Rate, bool>>>(predicate =>
                    _rates.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Add(It.IsAny<Rate>())).Callback(() => isAdded = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Rate>()).Returns(repositoryMock.Object);
            RateService _rateService = new RateService(unitOfWorkMock.Object);

            //Act
            _rateService.Add(projectDto);

            //Assert
            Assert.True(isAdded);
        }

        [Fact]
        public void RemoveTest()
        {
            //Arange
            bool isRemoved = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Rate>> repositoryMock = new Mock<IRepository<Rate>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Rate, bool>>>()))
                .Returns<Expression<Func<Rate, bool>>>(predicate =>
                    _rates.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Remove(It.IsAny<Rate>())).Callback(() => isRemoved = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Rate>()).Returns(repositoryMock.Object);
            RateService _rateService = new RateService(unitOfWorkMock.Object);

            //Act
            _rateService.Remove(_rates[0].Id);

            //Assert
            Assert.True(isRemoved);
        }

        [Fact]
        public void UpdateTest()
        {
            //Arange
            RateDTO projectDto = new RateDTO()
            {
                Id = "id1",
                UserId = "userId1",
                BookId = "bookId1",
                Value = 1
            };
            bool isUpdate = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Rate>> repositoryMock = new Mock<IRepository<Rate>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Rate, bool>>>()))
                .Returns<Expression<Func<Rate, bool>>>(predicate =>
                    _rates.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Update(It.Is<Rate>(entity =>
                    (entity.Id == projectDto.Id))))
                .Callback(() => isUpdate = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Rate>()).Returns(repositoryMock.Object);
            RateService _rateService = new RateService(unitOfWorkMock.Object);

            //Act
            _rateService.Update(projectDto);

            //Assert
            Assert.True(isUpdate);
        }
    }
}
