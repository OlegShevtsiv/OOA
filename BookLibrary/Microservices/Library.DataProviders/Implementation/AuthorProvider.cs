using System;
using System.Collections.Generic;
using System.Linq;
using Library.DataAccess.DTO;
using Library.DataAccess.Interfaces;
using Library.DataAccess.Models;
using Library.DataProviders.Filters;
using Library.DataProviders.Interfaces;

namespace Library.DataProviders.Implementation
{
    public class AuthorProvider : Provider<Author, AuthorDTO, IFilter>, IAuthorProvider
    {
        public AuthorProvider(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
        }

        public override AuthorDTO Get(string id)
        {
            Author entity = Repository
              .Get(e => e.Id == id)
              .SingleOrDefault();

            if (entity == null)
            {
                return new AuthorDTO();
            }

            return MapToDto(entity);
        }

        public override IEnumerable<AuthorDTO> Get(IFilter filter)
        {
            Func<Author, bool> predicate = GetFilter(filter);
            List<Author> entities = Repository
              .Get(p => predicate(p))
              .ToList();

            if (!entities.Any())
            {
                return new List<AuthorDTO>();
            }

            return entities.Select(e => MapToDto(e));
        }

        public override IEnumerable<AuthorDTO> GetAll()
        {
            List<Author> entities = Repository
              .Get(p => p != null)
              .ToList();

            if (!entities.Any())
            {
                return new List<AuthorDTO>();
            }

            return entities.Select(e => MapToDto(e));
        }

        // public override void Add(AuthorDTO dto)
        // {
        //     Author checkEntity = Repository
        //         .Get(e => e.Id == dto.Id)
        //         .SingleOrDefault();
        //
        //     if (checkEntity != null)
        //     {
        //         throw new DuplicateNameException();
        //     }
        //
        //     Author entity = MapToEntity(dto);
        //     Repository.Add(entity);
        //     _unitOfWork.SaveChanges();
        // }
        //
        // public override void Remove(string id)
        // {
        //     Author entity = Repository
        //      .Get(e => e.Id == id)
        //      .SingleOrDefault();
        //
        //     if (entity == null)
        //     {
        //         throw new ObjectNotFoundException();
        //     }
        //
        //     Repository.Remove(entity);
        //     _unitOfWork.SaveChanges();
        // }
        // public override void Update(AuthorDTO dto)
        // {
        //     Author entity = Repository
        //      .Get(e => e.Id == dto.Id)
        //      .SingleOrDefault();
        //
        //     if (entity == null)
        //     {
        //         throw new ObjectNotFoundException();
        //     }
        //
        //     entity.Name = dto.Name;
        //     entity.Surname = dto.Surname;
        //     entity.Description = dto.Description;
        //     entity.Image = dto.Image;
        //
        //     Repository.Update(entity);
        //     _unitOfWork.SaveChanges();
        // }

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

        private Func<Author, bool> GetFilter(IFilter filter)
        {
            Func<Author, bool> result = e => true;
            if (filter is AuthorFilter)
            {
                if (!String.IsNullOrEmpty((filter as AuthorFilter)?.Name))
                {
                    result += e => e.Name == (filter as AuthorFilter).Name;
                }
            }

            return result;
        }
    }
}
