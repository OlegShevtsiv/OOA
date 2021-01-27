using System;
using System.Data;
using System.Linq;
using Library.DataAccess.DTO;
using Library.DataAccess.EF;
using Library.DataAccess.Exceptions;
using Library.DataAccess.Models;
using Library.DataWriters.Interfaces;

namespace Library.DataWriters.Implementations
{
    internal class CommentDataWriter : DataWriter<Comment, CommentDTO>, ICommentDataWriter
    {
        public CommentDataWriter(WriteContext context) :
            base(context)
        {
        }
        
        public override void Add(CommentDTO dto)
        {
            Comment checkEntity = Context.Comments.FirstOrDefault(r => r.Id == dto.Id);
            
            if (checkEntity != null)
            {
                throw new DuplicateNameException();
            }
        
            Comment entity = MapToEntity(dto);
            entity.Id = Guid.NewGuid().ToString();
            Context.Add(entity);
            Context.SaveChanges();
        }
        
        public override void Remove(string id)
        {
            Comment entity = Context.Comments.FirstOrDefault(r => r.Id == id);

            if (entity == null)
            {
                throw new NotFoundException();
            }
        
            Context.Remove(entity);
            Context.SaveChanges();
        }
        public override void Update(CommentDTO dto)
        {
            Comment entity = Context.Comments.FirstOrDefault(r => r.Id == dto.Id);

            if (entity == null)
            {
                throw new NotFoundException();
            }
        
            entity.OwnerId = dto.OwnerId;
            entity.CommentedEssenceId = dto.CommentedEssenceId;
            entity.Text = dto.Text;
            entity.Time = dto.Time;
        
            Context.Update(entity);
            Context.SaveChanges();
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
    }
}
