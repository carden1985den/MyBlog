using Core;
using Core.Entity;
using Core.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;

        public PostService(IUnitOfWork unitOfWork, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public void Create(Post item)
        {
            _unitOfWork.Posts.Create(item);
        }

        public bool Delete(Post item)
        {
            try
            {
                _unitOfWork.Posts.Delete(item);
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete post with id {item.Id}");
                return false;
            }
        }

        public IEnumerable<Post> GetAll()
        {
            return _unitOfWork.Posts.GetAll();
        }

        public Post? GetById(Guid id)
        {
            return _unitOfWork.Posts.GetById(id);
        }

        public void Update(Post item)
        {
            _unitOfWork.Posts.Update(item);
        }
    }
}
