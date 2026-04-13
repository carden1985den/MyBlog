using API.Controllers;
using Core.DTOs;
using Core.Entity;
using Core.Models.User;

namespace Core.Interfaces.Services
{
    public interface IUserService
    {
        UserDto? Authenticate(string login, string password);
        IEnumerable<User> GetAll();
        IEnumerable <UserDto> GetAllDto();
        User? GetById(Guid id);
        UserDto? GetByIdDto(Guid id);
        UserProfile? GetProfileByUserId(Guid userId);
        bool Create(RegistrationViewModel item);
        void Update(User item, UserProfile item2);
        bool Delete(string id);
    }
}
