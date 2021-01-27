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
    public class BookProvider :Provider<Book, BookDTO, IFilter>, IBookProvider
    {
        public BookProvider(IUnitOfWork unitOfWork) :
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
                throw new NotFoundException();
            }

            return MapToDto(entity);
        }

        public override IEnumerable<BookDTO> Get(IFilter filter)
        {
            Func<Book, bool> predicate = GetFilter(filter);
            List<Book> entities = Repository.Get().ToList()
                .Where(p => predicate(p))
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
