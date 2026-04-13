using Core.Interfaces.Services;
using Core.Models.Role;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace WEB.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        public RoleController(IRoleService roleService, IUserService userService)
        {
            _roleService = roleService;
            _userService = userService;
        }

        [HttpGet]
        [Route("/Role/EditUserRole")]
        public IActionResult EditUserRole()
        {
            var model = new RoleAndUserModel();
            model.RoleAvailable = _roleService.GetAll().Select(t =>
                new RoleAvailable()
                {
                    RoleId = t.Id,
                    RoleName = t.Name.ToString(),
                    IsChecked = false
                }).ToList();

            model.UserAvailables = _userService.GetAllDto()
                .Select(u =>
                    new UserAvailable()
                    {
                            UserId = u.Id,
                            UserName = $"{u.FirstName.Trim().Concat(" " + u.FirstName.Trim())} ({u.UserName})",
                            UserRole = u.Role
                    })
                .ToList();

            return View("RoleAndUserView", model);
        }

        [HttpPost]
        [Route("/Role/EditUserRole")]
        public IActionResult EditUserRole(RoleAndUserModel model)
        {
            var targetUserId = model.UserAvailables.FirstOrDefault(t => t.IsChecked is true).UserId;
            var targetUser = _userService.GetAll().FirstOrDefault(u => u.Id == targetUserId);

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
            var targetUserRoleId = targetUser?.Role;
            // Если выбранная роль не привязана к текущему пользователю..
            var selectedRole = model.RoleAvailable.FirstOrDefault(r => r.IsChecked == true && r.RoleId != targetUserRoleId.Id);

            // Если роль найдена, то её можно установить пользвоателю, если selectedRole = null то ничего не ставим
            if (selectedRole is not null)
            {
                targetUser.RoleId = selectedRole.RoleId;
                _userService.Update(targetUser, null);
                //return View("RoleAndUserView", model);
                return RedirectToAction("EditUserRole", "Role");
            }

            return View("RoleAndUserView", model);
        }
    }
}
