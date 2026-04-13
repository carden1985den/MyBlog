using Core;
using Core.Entity;
using Core.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;

        public CommentService(IUnitOfWork unitOfWork, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public void Create(Comment item)
        {
            _unitOfWork.Comments.Create(item);
        }

        public void Delete(Comment item)
        {
            _unitOfWork.Comments.Delete(item);
        }

        public IQueryable<Comment> GetAll()
        {
            return _unitOfWork.Comments.GetAll();
             
        }

        public Comment? GetById(Guid id)
        {
            return _unitOfWork.Comments.GetById(id);
        }

        public void Update(Comment item)
        {
            _unitOfWork.Comments.Update(item);
        }
    }

}
