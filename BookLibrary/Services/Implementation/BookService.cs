using DataAccess.Implementation;
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
    public class BookService :Service<Book, BookDTO, IFilter>, IBookService
    {
        public BookService(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
        }

        public override BookDTO Get(string id)
        {
            Book entity = Repository
              .Get(e => e.Id == id)
              .SingleOrDefault();

            if (entity == null)
            {
                return new BookDTO();
            }

            return MapToDto(entity);
        }

        public override IEnumerable<BookDTO> Get(IFilter filter)
        {
            Func<Book, bool> predicate = GetFilter(filter);
            List<Book> entities = Repository
              .Get(p => predicate(p))
              .ToList();

            if (!entities.Any())
            {
                return new List<BookDTO>();
            }

            return entities.Select(e => MapToDto(e));
        }



        public override IEnumerable<BookDTO> GetAll()
        {
            List<Book> entities = Repository
              .Get(p => p != null)
              .ToList();

            if (!entities.Any())
            {
                return new List<BookDTO>();
            }

            return entities.Select(e => MapToDto(e));
        }

        public override void Add(BookDTO dto)
        {
            Book checkEntity = Repository
                .Get(e => e.Id == dto.Id)
                .SingleOrDefault();

            if (checkEntity != null)
            {
                throw new DuplicateNameException();
            }

            Book entity = MapToEntity(dto);
            Repository.Add(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Remove(string id)
        {
            Book entity = Repository
             .Get(e => e.Id == id)
             .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            Repository.Remove(entity);
            _unitOfWork.SaveChanges();
        }
        public override void Update(BookDTO dto)
        {
            Book entity = Repository
             .Get(e => e.Id == dto.Id)
             .SingleOrDefault();

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
        
            Repository.Update(entity);
            _unitOfWork.SaveChanges();
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

        private Func<Book, bool> GetFilter(IFilter filter)
        {
            Func<Book, bool> result = e => true;
            if (filter is BookFilter)
            {
                if (!String.IsNullOrEmpty((filter as BookFilter)?.Title))
                {
                    result += e => e.Title == (filter as BookFilter).Title;
                }
            }
            else if (filter is BookFilterByAuthor)
            {
                if (!String.IsNullOrEmpty((filter as BookFilterByAuthor)?.AuthorId))
                {
                    result += e => e.AuthorId == (filter as BookFilterByAuthor).AuthorId;
                }
            }
            return result;
        }
    }
}
