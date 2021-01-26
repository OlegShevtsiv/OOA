using System;
using System.Data;
using System.Linq;
using Library.DataAccess.DTO;
using Library.DataAccess.EF;
using Library.DataAccess.Models;
using Library.DataWriters.Exceptions;
using Library.DataWriters.Interfaces;

namespace Library.DataWriters.Implementations
{
    internal class RateDataWriter : DataWriter<Rate, RateDTO>, IRateDataWriter
    {
        public RateDataWriter(WriteContext context) :
            base(context)
        {
        }
        
        public override void Add(RateDTO dto)
        {
            Rate checkEntity = Context.Rates.FirstOrDefault(r => r.Id == dto.Id);

            if (checkEntity != null)
            {
                throw new DuplicateNameException();
            }
        
            Rate entity = MapToEntity(dto);
            Context.Add(entity);
            Context.SaveChanges();
        }
        
        public override void Remove(string id)
        {
            Rate entity = Context.Rates.FirstOrDefault(r => r.Id == id);
            
            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }
        
            Context.Remove(entity);
            Context.SaveChanges();
        }
        public override void Update(RateDTO dto)
        {
            Rate entity = Context.Rates.FirstOrDefault(r => r.Id == dto.Id);

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }
        
            entity.UserId = dto.UserId;
            entity.BookId = dto.BookId;
            entity.Value = dto.Value;
        
            Context.Update(entity);
            Context.SaveChanges();
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
    }
}
