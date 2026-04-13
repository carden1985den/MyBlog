using API.Models;
using Core.DTOs;
using Core.Entity;
using Core.Interfaces.Services;
using Core.Models.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, IRoleService roleService, ILogger<UserController> logger)
        {
            _userService = userService;
            _roleService = roleService;
            _logger = logger;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModelApi model)
        {
            if (ModelState.IsValid == false)
                return Unauthorized();

            if (User.Identity?.IsAuthenticated == true)
                return Ok();

            UserDto? authUser = _userService.Authenticate(model.Username, model.Password);

            if (authUser is null)
                return Unauthorized();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, authUser.UserName),
                new Claim("UserId", authUser.Id.ToString()),
                new Claim(ClaimTypes.Role, authUser.Role)
            };

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimIdentity));

            return Ok("Вы вошли в систему");
        }

        [Authorize]
        [HttpGet]
        [Route("GetAll")]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            var result = _userService.GetAllDto();
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        [Route("GetById")]
        public ActionResult<IEnumerable<User>> GetById([FromQuery] string id)
        {
            var result = _userService.GetByIdDto(Guid.Parse(id));
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromBody] RegistrationViewModel model)
        {
            if (ModelState.IsValid == false)
                return BadRequest("Ошибка введенных данных");

            _userService.Create(model);

            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("Create")]
        public IActionResult Create(RegistrationViewModel model)
        {
            _logger.LogInformation($"Попытка создания пользоватнеля: {model.Username}");

            if (ModelState.IsValid == false)
                return BadRequest("Ошибка введенных данных");

            bool status = _userService.Create(model);

            if (status == false)
                return BadRequest("Ошибка при создании пользователя");

            return Ok("Пользователь создан");
        }

        [Authorize]
        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete([FromBody] string id)
        {
            _logger.LogInformation($"Попытка удаления пользоватнеля по ID: {id}");

            if (ModelState.IsValid == false)
                return BadRequest("Ошибка введенных данных");

            bool status = _userService.Delete(id);
            if (status == false)
                return BadRequest("Ошибка при удалении пользователя");

            return Ok("Пользователь удален");
        }
    }
}
