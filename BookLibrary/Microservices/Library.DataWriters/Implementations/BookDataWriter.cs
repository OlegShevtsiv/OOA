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
    internal class BookDataWriter : DataWriter<Book, BookDTO>, IBookDataWriter
    {
        public BookDataWriter(WriteContext context) :
            base(context)
        {
        }

        public override void Add(BookDTO dto)
        {
            Book checkEntity = Context.Books.FirstOrDefault(r => r.Id == dto.Id);
            
            if (checkEntity != null)
            {
                throw new DuplicateNameException();
            }
        
            Book entity = MapToEntity(dto);
            Context.Add(entity);
            Context.SaveChanges();
        }
        
        public override void Remove(string id)
        {
            Book entity = Context.Books.FirstOrDefault(r => r.Id == id);
            
            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }
        
            Context.Remove(entity);
            Context.SaveChanges();
        }
        public override void Update(BookDTO dto)
        {
            Book entity = Context.Books.FirstOrDefault(r => r.Id == dto.Id);
            
            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }
        
            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.AuthorId = dto.AuthorId;
            entity.Rate = dto.Rate;
            entity.Image = dto.Image;
            entity.FileBook = dto.FileBook;
            entity.Year = dto.Year;
            entity.Genre = dto.Genre;
            entity.RatesAmount = dto.RatesAmount;
        
            Context.Update(entity);
            Context.SaveChanges();
        }

        protected override BookDTO MapToDto(Book entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            BookDTO dto = new BookDTO
            {
                Id = entity.Id,
                Title = entity.Title,
                AuthorId = entity.AuthorId,
                Image = entity.Image,
                FileBook = entity.FileBook,
                Rate = entity.Rate,
                Year = entity.Year,
                Description = entity.Description,
                Genre = entity.Genre,
                RatesAmount = entity.RatesAmount
            };

            return dto;
        }

        protected override Book MapToEntity(BookDTO dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException();
            }

            Book entity = new Book
            {
                Id = dto.Id,
                Title = dto.Title,
                AuthorId = dto.AuthorId,
                Image = dto.Image,
                FileBook = dto.FileBook,    
                Rate = dto.Rate,
                Year = dto.Year,
                Description = dto.Description,
                Genre = dto.Genre,
                RatesAmount = dto.RatesAmount
            };

            return entity;
        }
    }
}
