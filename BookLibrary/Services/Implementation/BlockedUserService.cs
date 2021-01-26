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
    public class BlockedUserService : Service<BlockedUser, BlockedUserDTO, IFilter>, IBlockedUserService
    {
        public BlockedUserService(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
        }

        public override BlockedUserDTO Get(string id)
        {
            BlockedUser entity = Repository
              .Get(e => e.Id == id)
              .SingleOrDefault();

            if (entity == null)
            {
                return new BlockedUserDTO();
            }

            return MapToDto(entity);
        }

        public override IEnumerable<BlockedUserDTO> Get(IFilter filter)
        {
            Func<BlockedUser, bool> predicate = GetFilter(filter);
            List<BlockedUser> entities = Repository
              .Get(p => predicate(p))
              .ToList();

            if (!entities.Any())
            {
                return new List<BlockedUserDTO>();
            }

            return entities.Select(e => MapToDto(e));
        }

        public override IEnumerable<BlockedUserDTO> GetAll()
        {
            List<BlockedUser> entities = Repository
              .Get(p => p != null)
              .ToList();

            if (!entities.Any())
            {
                return new List<BlockedUserDTO>();
            }

            return entities.Select(e => MapToDto(e));
        }

        public override void Add(BlockedUserDTO dto)
        {
            BlockedUser checkEntity = Repository
                .Get(e => e.Id == dto.Id)
                .SingleOrDefault();

            if (checkEntity != null)
            {
                throw new DuplicateNameException();
            }

            BlockedUser entity = MapToEntity(dto);
            Repository.Add(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Remove(string id)
        {
            BlockedUser entity = Repository
             .Get(e => e.Id == id)
             .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            Repository.Remove(entity);
            _unitOfWork.SaveChanges();
        }
        public override void Update(BlockedUserDTO dto)
        {
            BlockedUser entity = Repository
             .Get(e => e.Id == dto.Id)
             .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            entity.UserId = dto.UserId;

            Repository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        protected override BlockedUserDTO MapToDto(BlockedUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            BlockedUserDTO dto = new BlockedUserDTO
            {
                Id = entity.Id,
                UserId = entity.UserId,
            };

            return dto;
        }

        protected override BlockedUser MapToEntity(BlockedUserDTO dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException();
            }

            BlockedUser entity = new BlockedUser
            {
                Id = dto.Id,
                UserId = dto.UserId,
            };

            return entity;
        }

        private Func<BlockedUser, bool> GetFilter(IFilter filter)
        {
            Func<BlockedUser, bool> result = e => true;
            if (filter is BlockedUserFilterByUserId)
            {
                if (!String.IsNullOrEmpty((filter as BlockedUserFilterByUserId)?.UserId))
                {
                    result += e => e.UserId == (filter as BlockedUserFilterByUserId).UserId;
                }
            }
            return result;
        }
    }
}
