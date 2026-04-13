using API.Controllers;
using Core;
using Core.DTOs;
using Core.Entity;
using Core.Interfaces.Services;
using Core.Models.User;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;
        public UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public UserDto? Authenticate(string login, string password)
        {
            return _unitOfWork.Users.GetAll()
                .Where(u => u.Login == login && u.Password == password)
                .Select(u => new UserDto()
                {
                    Id = u.Id,
                    UserName = u.Login,
                    FirstName = u.Profile.FirstName,
                    LastName = u.Profile.LastName,
                    Role = u.Role.Name.ToString()
                }).FirstOrDefault();
        }

        public IEnumerable<User> GetAll()
        {
            return _unitOfWork.Users.GetAll().ToList() ;
        }

        public IEnumerable<UserDto> GetAllDto()
        {
            return _unitOfWork.Users.GetAll()
               .Select(u => new UserDto()
               {
                   Id = u.Id,
                   UserName = u.Login,
                   FirstName = u.Profile.FirstName,
                   LastName = u.Profile.LastName,
                   Role = u.Role.Name.ToString()
               }).ToList();
        }

        // This method retrieves a User by their ID and maps it to a UserDto.
        public UserDto? GetByIdDto(Guid id)
        {
            return _unitOfWork.Users.GetAll()
                .Where(u => u.Id == id)
                .Select(u => new UserDto()
                {
                    Id = u.Id,
                    UserName = u.Login,
                    FirstName = u.Profile.FirstName,
                    LastName = u.Profile.LastName,
                    Role = u.Role.Name.ToString()
                }).FirstOrDefault();
        }

        public User? GetById(Guid id)
        {
            return _unitOfWork.Users.GetAll().FirstOrDefault(u => u.Id == id);
        }

        public UserProfile? GetProfileByUserId(Guid userId)
        {
            return _unitOfWork.UserProfiles.GetAll().FirstOrDefault(p => p.UserId == userId);
        }

        public void Update(User item, UserProfile item2)
        {
            _unitOfWork.Users.Update(item);

            if (item2 != null)
                _unitOfWork.UserProfiles.Update(item2);
        }

        public bool Create(RegistrationViewModel model)
        {

            var existingUser = _unitOfWork.Users.GetAll().FirstOrDefault(u => u.Login == model.Username);

            if (existingUser == null)
            {
                var newUserId = Guid.NewGuid();

                var user = new User()
                {
                    Id = newUserId,
                    Login = model.Username,
                    Password = model.Password,
                    RoleId = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                };

                var userProfile = new UserProfile()
                {
                    FirstName = model.Firstname,
                    LastName = model.Lastname,
                    UserId = newUserId
                };

                _unitOfWork.Users.Create(user);
                _unitOfWork.UserProfiles.Create(userProfile);

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid userId))
                return false;

            User result = _unitOfWork.Users.GetById(userId);

            if (result == null)
                return false;

            _unitOfWork.Users.Delete(result);
            return true;
        }
    }
}
