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
    public class CommentServiceTests
    {
        private readonly List<Comment> _comments;

        public CommentServiceTests()
        {
            String _text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";

            _comments = new List<Comment>
            {
                new Comment { Id = "id1", CommentedEssenceId = "ceId1", Time = new DateTime(2019, 01, 01), Text = _text},
                new Comment { Id = "id2", CommentedEssenceId = "ceId2", Time = new DateTime(2019, 02, 02), Text = _text},
                new Comment { Id = "id3", CommentedEssenceId = "ceId3", Time = new DateTime(2019, 03, 03), Text = _text}
            };
        }

        [Fact]
        public void GetByFilterTest()
        {
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Comment>> repositoryMock = new Mock<IRepository<Comment>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Comment, bool>>>())).Returns(_comments.AsQueryable);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Comment>()).Returns(repositoryMock.Object);
            CommentService _commentService = new CommentService(unitOfWorkMock.Object);
            CommentFilter _commentFilter = new CommentFilter();

            //Act
            IEnumerable<CommentDTO> _commentsDto = _commentService.Get(_commentFilter);

            //Assert
            Assert.NotNull(_commentsDto);
            Assert.NotEmpty(_commentsDto);
            Assert.Equal(3, _commentsDto.Count());
        }

        [Fact]

        public void GetAllTest()
        {
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock < IRepository <Comment>> repositoryMock = new Mock<IRepository<Comment>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Comment, bool>>>())).Returns(_comments.AsQueryable);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Comment>()).Returns(repositoryMock.Object);
            CommentService _commentService = new CommentService(unitOfWorkMock.Object);

            //Act
            IEnumerable<CommentDTO> _commentsDto = _commentService.GetAll();

            //Assert
            Assert.NotNull(_commentsDto);
            Assert.NotEmpty(_commentsDto);
            Assert.Equal(3, _commentsDto.Count());
        }

        [Fact]
        public void GetByIdTest()
        {
            String _text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Comment>> repositoryMock = new Mock<IRepository<Comment>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Comment, bool>>>()))
                .Returns<Expression<Func<Comment, bool>>>(predicate =>
                    _comments.Where(predicate.Compile()).AsQueryable());
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Comment>()).Returns(repositoryMock.Object);
            CommentService _commentService = new CommentService(unitOfWorkMock.Object);
            //Act
            CommentDTO _commentDto = _commentService.Get("id1");
            //Assert
            Assert.NotNull(_commentDto);
            Assert.Equal(_text, _commentDto.Text);
        }

        [Fact]
        public void AddTest()
        {
            //Arange
            CommentDTO projectDto = new CommentDTO()
            {
                Id = "b0",
                Text = "Zero"
            };
            bool isAdded = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Comment>> repositoryMock = new Mock<IRepository<Comment>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Comment, bool>>>()))
                .Returns<Expression<Func<Comment, bool>>>(predicate =>
                    _comments.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Add(It.IsAny<Comment>())).Callback(() => isAdded = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Comment>()).Returns(repositoryMock.Object);
            CommentService _commentService = new CommentService(unitOfWorkMock.Object);

            //Act
            _commentService.Add(projectDto);

            //Assert
            Assert.True(isAdded);
        }

        [Fact]
        public void RemoveTest()
        {
            //Arange
            bool isRemoved = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Comment>> repositoryMock = new Mock<IRepository<Comment>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Comment, bool>>>()))
                .Returns<Expression<Func<Comment, bool>>>(predicate =>
                    _comments.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Remove(It.IsAny<Comment>())).Callback(() => isRemoved = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Comment>()).Returns(repositoryMock.Object);
            CommentService _commentService = new CommentService(unitOfWorkMock.Object);

            //Act
            _commentService.Remove(_comments[0].Id);

            //Assert
            Assert.True(isRemoved);
        }

        [Fact]
        public void UpdateTest()
        {
            String _text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
            //Arange
            CommentDTO projectDto = new CommentDTO()
            {
                Id = "id1",
                Text = _text
            };
            bool isUpdate = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Comment>> repositoryMock = new Mock<IRepository<Comment>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Comment, bool>>>()))
                .Returns<Expression<Func<Comment, bool>>>(predicate =>
                    _comments.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Update(It.Is<Comment>(entity =>
                    (entity.Id == projectDto.Id) &&
                    (entity.Text == projectDto.Text))))
                .Callback(() => isUpdate = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Comment>()).Returns(repositoryMock.Object);
            CommentService _commentService = new CommentService(unitOfWorkMock.Object);

            //Act
            _commentService.Update(projectDto);

            //Assert
            Assert.True(isUpdate);
        }
    }
}
