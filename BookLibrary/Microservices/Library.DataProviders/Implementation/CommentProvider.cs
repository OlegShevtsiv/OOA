using System;
using System.Collections.Generic;
using System.Linq;
using Library.DataAccess.DTO;
using Library.DataAccess.Exceptions;
using Library.DataAccess.Interfaces;
using Library.DataAccess.Models;
using Library.DataProviders.Filters;
using Library.DataProviders.Interfaces;

namespace Library.DataProviders.Implementation
{
    public class CommentProvider : Provider<Comment, CommentDTO, IFilter>, ICommentProvider
    {
        public CommentProvider(IUnitOfWork unitOfWork) :
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
                throw new NotFoundException();
            }

            return MapToDto(entity);
        }

        public override IEnumerable<CommentDTO> Get(IFilter filter)
        {
            Func<Comment, bool> predicate = GetFilter(filter);
            List<Comment> entities = Repository.Get().ToList()
              .Where(p => predicate(p))
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
