using Core;
using Core.Entity;
using Core.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;

        public TagService(IUnitOfWork unitOfWork, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public void Create(Tag item)
        {
            _unitOfWork.Tags.Create(item);
        }

        public void Delete(Tag item)
        {
            _unitOfWork.Tags.Delete(item);
        }

        public IEnumerable<Tag> GetAll()
        {
            return _unitOfWork.Tags.GetAll();

        }

        public Tag? GetById(Guid id)
        {
            return _unitOfWork.Tags.GetById(id);
        }

        public void Update(Tag item)
        {
            _unitOfWork.Tags.Update(item);
        }
    }
}
