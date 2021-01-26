using DataAccess.Interfaces;
using DataAccess.Models;
using Services.DTO;
using Services.Exceptions;
using Services.Filters;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Services.Interfaces
{
    public class CommentService : Service<Comment, CommentDTO, IFilter>, ICommentService
    {
        public CommentService(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
        }

        public override CommentDTO Get(string id)
        {
            Comment entity = Repository
              .Get(e => e.Id == id)
              .SingleOrDefault();

            if (entity == null)
            {
                return new CommentDTO();
            }

            return MapToDto(entity);
        }

        public override IEnumerable<CommentDTO> Get(IFilter filter)
        {
            Func<Comment, bool> predicate = GetFilter(filter);
            List<Comment> entities = Repository
              .Get(p => predicate(p))
              .ToList();

            if (!entities.Any())
            {
                return new List<CommentDTO>();
            }

            return entities.Select(e => MapToDto(e));
        }

        public override IEnumerable<CommentDTO> GetAll()
        {
            List<Comment> entities = Repository
              .Get(p => p != null)
              .ToList();

            if (!entities.Any())
            {
                return new List<CommentDTO>();
            }

            return entities.Select(e => MapToDto(e));
        }

        public override void Add(CommentDTO dto)
        {
            Comment checkEntity = Repository
                .Get(e => e.Id == dto.Id)
                .SingleOrDefault();

            if (checkEntity != null)
            {
                throw new DuplicateNameException();
            }

            Comment entity = MapToEntity(dto);
            Repository.Add(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Remove(string id)
        {
            Comment entity = Repository
             .Get(e => e.Id == id)
             .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            Repository.Remove(entity);
            _unitOfWork.SaveChanges();
        }
        public override void Update(CommentDTO dto)
        {
            Comment entity = Repository
             .Get(e => e.Id == dto.Id)
             .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            entity.OwnerId = dto.OwnerId;
            entity.CommentedEssenceId = dto.CommentedEssenceId;
            entity.Text = dto.Text;
            entity.Time = dto.Time;

            Repository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        protected override CommentDTO MapToDto(Comment entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            CommentDTO dto = new CommentDTO
            {
                Id = entity.Id,
                OwnerId = entity.OwnerId,
                CommentedEssenceId = entity.CommentedEssenceId,
                Text = entity.Text,
                Time = entity.Time
            };

            return dto;
        }

        protected override Comment MapToEntity(CommentDTO dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException();
            }

            Comment entity = new Comment
            {
                Id = dto.Id,
                OwnerId = dto.OwnerId,
                CommentedEssenceId = dto.CommentedEssenceId,
                Text = dto.Text,
                Time = dto.Time
                
            };

            return entity;
        }

        private Func<Comment, bool> GetFilter(IFilter filter)
        {
            Func<Comment, bool> result = e => true;
            if (filter is CommentFilter)
            {
                if (!String.IsNullOrEmpty((filter as CommentFilter)?.CommentedEssenceId))
                {
                    result += e => e.CommentedEssenceId == (filter as CommentFilter).CommentedEssenceId;
                }
            }
            else if (filter is CommentFilterByOwnerId)
            {
                if (!String.IsNullOrEmpty((filter as CommentFilterByOwnerId)?.OwnerId))
                {
                    result += e => e.OwnerId == (filter as CommentFilterByOwnerId).OwnerId;
                }
            }
            return result;
        }
    }
}
