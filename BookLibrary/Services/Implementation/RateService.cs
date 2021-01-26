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
    public class RateService : Service<Rate, RateDTO, IFilter>, IRateService
    {
        public RateService(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
        }

        public override RateDTO Get(string id)
        {
            Rate entity = Repository
              .Get(e => e.Id == id)
              .SingleOrDefault();

            if (entity == null)
            {
                return new RateDTO();
            }

            return MapToDto(entity);
        }

        public override IEnumerable<RateDTO> Get(IFilter filter)
        {
            Func<Rate, bool> predicate = GetFilter(filter);
            List<Rate> entities = Repository
              .Get(p => predicate(p))
              .ToList();

            if (!entities.Any())
            {
                return new List<RateDTO>();
            }

            return entities.Select(e => MapToDto(e));
        }

        public override IEnumerable<RateDTO> GetAll()
        {
            List<Rate> entities = Repository
              .Get(p => p != null)
              .ToList();

            if (!entities.Any())
            {
                return new List<RateDTO>();
            }

            return entities.Select(e => MapToDto(e));
        }

        public override void Add(RateDTO dto)
        {
            Rate checkEntity = Repository
                .Get(e => e.Id == dto.Id)
                .SingleOrDefault();

            if (checkEntity != null)
            {
                throw new DuplicateNameException();
            }

            Rate entity = MapToEntity(dto);
            Repository.Add(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Remove(string id)
        {
            Rate entity = Repository
             .Get(e => e.Id == id)
             .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            Repository.Remove(entity);
            _unitOfWork.SaveChanges();
        }
        public override void Update(RateDTO dto)
        {
            Rate entity = Repository
             .Get(e => e.Id == dto.Id)
             .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            entity.UserId = dto.UserId;
            entity.BookId = dto.BookId;
            entity.Value = dto.Value;

            Repository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        protected override RateDTO MapToDto(Rate entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            RateDTO dto = new RateDTO
            {
                Id = entity.Id,
                UserId = entity.UserId,
                BookId = entity.BookId,
                Value = entity.Value
            };

            return dto;
        }

        protected override Rate MapToEntity(RateDTO dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException();
            }

            Rate entity = new Rate
            {
                Id = dto.Id,
                UserId = dto.UserId,
                BookId = dto.BookId,
                Value = dto.Value
            };

            return entity;
        }

        private Func<Rate, bool> GetFilter(IFilter filter)
        {
            Func<Rate, bool> result = e => true;
            if (filter is RateFilterByUserId)
            {
                if (!String.IsNullOrEmpty((filter as RateFilterByUserId)?.UserId))
                {
                    result += e => e.UserId == (filter as RateFilterByUserId).UserId;
                }
            }
            else if (filter is RateFilterByBookId)
            {
                if (!String.IsNullOrEmpty((filter as RateFilterByBookId)?.BookId))
                {
                    result += e => e.BookId == (filter as RateFilterByBookId).BookId;
                }
            }
            return result;
        }


    }
}
