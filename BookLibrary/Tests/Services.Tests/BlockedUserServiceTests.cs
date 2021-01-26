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
    public class BlockedUserServiceTests
    {
        private readonly List<BlockedUser> _blockedUsers;

        public BlockedUserServiceTests()
        {
            _blockedUsers = new List<BlockedUser>
            {
                new BlockedUser {  Id = "id1", UserId = "userId1"},
                new BlockedUser {  Id = "id2", UserId = "userId2"},
                new BlockedUser {  Id = "id3", UserId = "userId3"}
            };
        }

        [Fact]
        public void GetByFilterByUserIdTest()
        {
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<BlockedUser>> repositoryMock = new Mock<IRepository<BlockedUser>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<BlockedUser, bool>>>())).Returns(_blockedUsers.AsQueryable);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<BlockedUser>()).Returns(repositoryMock.Object);
            BlockedUserService _blockedUserService = new BlockedUserService(unitOfWorkMock.Object);
            BlockedUserFilterByUserId _blockedUserFilter = new BlockedUserFilterByUserId();

            //Act
            IEnumerable<BlockedUserDTO> _blockedUsersDto = _blockedUserService.Get(_blockedUserFilter);

            //Assert
            Assert.NotNull(_blockedUsersDto);
            Assert.NotEmpty(_blockedUsersDto);
            Assert.Equal(3, _blockedUsersDto.Count());
        }

        [Fact]
        public void GetAllTest()
        {
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<BlockedUser>> repositoryMock = new Mock<IRepository<BlockedUser>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<BlockedUser, bool>>>())).Returns(_blockedUsers.AsQueryable);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<BlockedUser>()).Returns(repositoryMock.Object);
            BlockedUserService _blockedUserService = new BlockedUserService(unitOfWorkMock.Object);

            //Act
            IEnumerable<BlockedUserDTO> _blockedUsersDto = _blockedUserService.GetAll();

            //Assert
            Assert.NotNull(_blockedUsersDto);
            Assert.NotEmpty(_blockedUsersDto);
            Assert.Equal(3, _blockedUsersDto.Count());
        }

        [Fact]
        public void GetByIdTest()
        {
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<BlockedUser>> repositoryMock = new Mock<IRepository<BlockedUser>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<BlockedUser, bool>>>()))
                .Returns<Expression<Func<BlockedUser, bool>>>(predicate =>
                    _blockedUsers.Where(predicate.Compile()).AsQueryable());
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<BlockedUser>()).Returns(repositoryMock.Object);
            BlockedUserService _blockedUserService = new BlockedUserService(unitOfWorkMock.Object);

            //Act
            BlockedUserDTO _blockedUserDto = _blockedUserService.Get("id1");

            //Assert
            Assert.NotNull(_blockedUserDto);
            Assert.Equal("id1", _blockedUserDto.Id);
        }

        [Fact]
        public void AddTest()
        {
            BlockedUserDTO projectDto = new BlockedUserDTO()
            {
                Id = "userId1",
                UserId = "userId1"
            };
            bool isAdded = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<BlockedUser>> repositoryMock = new Mock<IRepository<BlockedUser>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<BlockedUser, bool>>>()))
                .Returns<Expression<Func<BlockedUser, bool>>>(predicate =>
                    _blockedUsers.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Add(It.IsAny<BlockedUser>())).Callback(() => isAdded = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<BlockedUser>()).Returns(repositoryMock.Object);
            BlockedUserService _blockedUserService = new BlockedUserService(unitOfWorkMock.Object);

            //Act
            _blockedUserService.Add(projectDto);

            //Assert
            Assert.True(isAdded);
        }

        [Fact]
        public void RemoveTest()
        {
            //Arange
            bool isRemoved = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<BlockedUser>> repositoryMock = new Mock<IRepository<BlockedUser>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<BlockedUser, bool>>>()))
                .Returns<Expression<Func<BlockedUser, bool>>>(predicate =>
                    _blockedUsers.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Remove(It.IsAny<BlockedUser>())).Callback(() => isRemoved = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<BlockedUser>()).Returns(repositoryMock.Object);
            BlockedUserService _blockedUserService = new BlockedUserService(unitOfWorkMock.Object);

            //Act
            _blockedUserService.Remove(_blockedUsers[0].Id);

            //Assert
            Assert.True(isRemoved);
        }

        [Fact]
        public void UpdateTest()
        {
            //Arange
            BlockedUserDTO projectDto = new BlockedUserDTO()
            {
                Id = "id1",
                UserId = "userId1"
            };
            bool isUpdate = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<BlockedUser>> repositoryMock = new Mock<IRepository<BlockedUser>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<BlockedUser, bool>>>()))
                .Returns<Expression<Func<BlockedUser, bool>>>(predicate =>
                    _blockedUsers.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Update(It.Is<BlockedUser>(entity =>
                    (entity.Id == projectDto.Id))))
                .Callback(() => isUpdate = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<BlockedUser>()).Returns(repositoryMock.Object);
            BlockedUserService _blockedUserService = new BlockedUserService(unitOfWorkMock.Object);

            //Act
            _blockedUserService.Update(projectDto);

            //Assert
            Assert.True(isUpdate);
        }
    }
}
