using BLL.Enum;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using WEB.Models.Role;

namespace WEB.Controllers
{
    public class RoleController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public RoleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("/Role/EditUserRole")]
        public IActionResult EditUserRole()
        {
            //var allUsers = _unitOfWork.Users.GetAll();
            //var allRoles = _unitOfWork.Roles.GetAll();

            var model = new RoleAndUserModel();
            model.RoleAvailable = _unitOfWork.Roles.GetAll().Select(t =>
                new RoleAvailable()
                {
                    RoleId = t.Id,
                    RoleName = t.Name.ToString(),
                    IsChecked = false
                }).ToList();

            model.UserAvailables = _unitOfWork.Users.GetAll()
                .Include(u => u.Profile)
                .Include(u => u.Role)
                .Select(u =>
                    new UserAvailable()
                    {
                            UserId = u.Id,
                            UserName = $"{u.Profile.FullName().Trim()} ({u.Login})",
                            UserRole = u.Role.Name.ToString()
                    })
                .ToList();

            return View("RoleAndUserView", model);
        }

        [HttpPost]
        [Route("/Role/EditUserRole")]
        public IActionResult EditUserRole(RoleAndUserModel model)
        {
            var targetUserId = model.UserAvailables.FirstOrDefault(t => t.IsChecked is true).UserId;
            var targetUser = _unitOfWork.Users.GetAll().FirstOrDefault(u => u.Id == targetUserId);

            if (model.UserAvailables.Where(t => t.IsChecked is true).Count() > 1)
            {
                ModelState.AddModelError("", "Выбрано более одного пользователя для редактирования ролей");
                return View("RoleAndUserView", model);
            }

            if (model.RoleAvailable.Where(r => r.IsChecked is true).Count() == 0)
            {
                ModelState.AddModelError("", "Не выбрана роль пользователя");
                return View("RoleAndUserView", model);
            }

            if (model.RoleAvailable.Where(r => r.IsChecked is true).Count() > 1)
            {
                ModelState.AddModelError("", "Выбрано более одной роли пользователя");
                return View("RoleAndUserView", model);
            }

            // Получаем текущую роль пользователя
            var targetUserRoleId = targetUser.RoleId;
            // Если выбранная роль не привязана к текущему пользователю..
            var selectedRole = model.RoleAvailable.FirstOrDefault(r => r.IsChecked == true && r.RoleId != targetUserRoleId);

            // Если роль найдена, то её можно установить пользвоателю, если selectedRole = null то ничего не ставим
            if (selectedRole is not null)
            {
                targetUser.RoleId = selectedRole.RoleId;
                _unitOfWork.Users.Update(targetUser);
                //return View("RoleAndUserView", model);
                return RedirectToAction("EditUserRole", "Role");
            }

            return View("RoleAndUserView", model);
        }
    }
}
