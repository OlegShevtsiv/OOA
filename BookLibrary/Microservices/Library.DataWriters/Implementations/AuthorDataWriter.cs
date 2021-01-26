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
    internal class AuthorDataWriter : DataWriter<Author, AuthorDTO>, IAuthorDataWriter
    {
        public AuthorDataWriter(WriteContext context) :
            base(context)
        {
        }
        
        public override void Add(AuthorDTO dto)
        {
            Author checkEntity = Context.Authors.FirstOrDefault(r => r.Id == dto.Id);
            
            if (checkEntity != null)
            {
                throw new DuplicateNameException();
            }
        
            Author entity = MapToEntity(dto);
            Context.Add(entity);
            Context.SaveChanges();
        }
        
        public override void Remove(string id)
        {
            Author entity = Context.Authors.FirstOrDefault(r => r.Id == id);
            
            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }
        
            Context.Remove(entity);
            Context.SaveChanges();
        }
        public override void Update(AuthorDTO dto)
        {
            Author entity = Context.Authors.FirstOrDefault(r => r.Id == dto.Id);
            
            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }
        
            entity.Name = dto.Name;
            entity.Surname = dto.Surname;
            entity.Description = dto.Description;
            entity.Image = dto.Image;
        
            Context.Update(entity);
            Context.SaveChanges();
        }

        protected override AuthorDTO MapToDto(Author entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            AuthorDTO dto = new AuthorDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Surname = entity.Surname,
                Description = entity.Description,
                Image = entity.Image
            };

            return dto;
        }

        protected override Author MapToEntity(AuthorDTO dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException();
            }

            Author entity = new Author
            {
                Id = dto.Id,
                Name = dto.Name,
                Surname = dto.Surname,
                Description = dto.Description,
                Image = dto.Image
            };

            return entity;
        }
    }
}
