using Core;
using Core.Entity;
using Core.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;

        public RoleService(IUnitOfWork unitOfWork, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public void Create(Role item)
        {
            _unitOfWork.Roles.Create(item);
        }

        public void Delete(Role item)
        {
            _unitOfWork.Roles.Delete(item);
        }

        public IEnumerable<Role> GetAll()
        {
            return _unitOfWork.Roles.GetAll();
        }

        public Role? GetById(Guid id)
        {
            return _unitOfWork.Roles.GetById(id);
        }

        public void Update(Role item)
        {
            _unitOfWork.Roles.Update(item);
        }
    }
}
