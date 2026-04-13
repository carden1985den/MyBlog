using AutoMapper;
using BLL.Services;
using Core;
using Core.DTOs;
using Core.Entity;
using Core.Enum;
using Core.Interfaces.Services;
using Core.Models.User;
using DAL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WEB.Models.User;

namespace WEB.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, IRoleService roleService, IMapper mapper, ILogger<UserController> logger)
        {
            //_unitOfWork = new UnitOfWork(context);
            _userService = userService;
            _roleService = roleService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Регистрация нового пользователя. При GET-запросе отображается форма регистрации, а при POST-запросе обрабатываются данные формы и создается новый пользователь в базе данных.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Registration")]
        [Route("/User/Registration")]
        public IActionResult Registrations()
        {
            return View("Registration");
        }

        /// <summary>
        /// Обрабатывает запросы на регистрацию пользователей, отображая окно регистрации с использованием предоставленного контекста HTTP.
        /// </summary>
        /// <remarks>This action is intended to be called via HTTP POST. The registration view is rendered
        /// based on the supplied context, which may affect the content or behavior of the view.</remarks>
        /// <param name="context">The HTTP context for the current request, containing information such as user identity, request data, and
        /// session state.</param>
        /// <returns>An IActionResult that renders the registration view to the client.</returns>
        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration(RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Ошибка введенных данных");
                return View("Registration");
            }

            bool result = _userService.Create(model);

            if (result == false)
            {
                _logger.LogError("Попытка регистрации с уже существующим логином: {Username}", model.Username);
                ModelState.AddModelError("", "Указанный логин занят другим пользователем");
                return View("Registration");
            }

            return RedirectToAction("Login");
        }

        /// <summary>
        /// Отображает страницу входа в систему или перенаправляет авторизованных пользователей на главную страницу.
        /// </summary>
        /// <remarks>If the user is already authenticated, this action prevents access to the login page
        /// and redirects to the main application page. This helps avoid unnecessary login prompts for users who are
        /// already signed in.</remarks>
        /// <returns>An <see cref="IActionResult"/> that renders the login view for unauthenticated users, or redirects
        /// authenticated users to the home page.</returns>
        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            // Если пользователь уже аутентифицирован, перенаправляем его на главную страницу
            if (HttpContext.User.Identity?.IsAuthenticated is true)
                return RedirectToAction("Index", "Home");

            return View("Login");
        }

        /// <summary>
        /// запрос на вход в систему. Если пользователь найден и пароль совпадает, выполняется вход и перенаправление на главную страницу. В противном случае отображается сообщение об ошибке.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserDto authUser = _userService.Authenticate(model.Username, model.Password);

                // Проверяем, существует ли пользователь с указанным именем
                if (authUser is null)
                {
                    ModelState.AddModelError("", "Ошибка авторизации");
                    return View(model);
                }

                // Если пользователь найден и пароль совпадает, создаем ClaimsIdentity и выполняем вход
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, authUser.UserName),
                    new Claim(ClaimTypes .Role, authUser.Role.ToString()),
                    new Claim("UserId", authUser.Id.ToString())
                };

                // Добавить в Claim: "ID" авторизованного текущего пользователя
                claims.Add(new Claim("UserId", authUser.Id.ToString()));

                // Создаем ClaimsIdentity с использованием схемы аутентификации "Cookies" и выполняем вход, передавая созданный ClaimsPrincipal
                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    "Cookies",
                    ClaimsIdentity.DefaultNameClaimType, // Какое поле считать за Name
                    ClaimsIdentity.DefaultRoleClaimType  // Какое поле считать за Role
                    );

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Если модель недействительна, добавляем общую ошибку в ModelState и возвращаем представление с сообщением об ошибке
                ModelState.AddModelError(null, "Ошибка заполнения формы");
            }
            return View("Login");
        }

        /// <summary>
        /// Обрабатывает GET-запросы для отображения формы редактирования профиля пользователя для текущего авторизованного пользователя.
        /// </summary>
        /// <remarks>The returned view model contains the current user's data, excluding the password for
        /// security reasons. This action requires the user to be authenticated.</remarks>
        /// <returns>A view result that renders the edit form populated with the current user's profile information.</returns>
        [Authorize]
        [HttpGet]
        [Route("User/Edit")]
        public IActionResult Edit()
        {
            var guid = User.FindFirst("UserId").Value;
            // Получаем текущего пользователя из базы данных, используя его идентификатор, который хранится в клайме "UserId" после аутентификации.
            var currentUser = _userService.GetById(Guid.Parse(User.FindFirst("UserId").Value));

            // Получаем профиль текущего пользователя, используя его идентификатор. Это может включать дополнительную информацию о пользователе, такую как имя, фамилия, и другие данные, которые не хранятся непосредственно в сущности User.
            var currentUserProfile = _userService.GetProfileByUserId(currentUser.Id);

            // Создаем модель представления EditViewModel и заполняем ее данными текущего пользователя и его профиля. Это позволяет передать всю необходимую информацию в представление для отображения и редактирования.
            var model = _mapper.Map<EditViewModel>((currentUser, currentUserProfile));

            // В целях безопасности не передаем пароль в представление, так как он может быть хеширован и не должен быть доступен для отображения или редактирования.
            model.Password = null!;

            return View("Edit", model);
        }

        // Обрабатывает запрос на редактирование профиля пользователя. Получает данные из модели EditViewModel, обновляет информацию о пользователе и его профиле в базе данных, а затем сохраняет изменения.
        [Authorize]
        [HttpPost]
        public IActionResult Edit(EditViewModel model)
        {
            // Получаем текущего пользователя из базы данных, используя его идентификатор, который хранится в клайме "UserId" после аутентификации.
            // Это позволяет нам обновить информацию именно для текущего пользователя, а не для другого.
            var currentUser = _userService.GetById(Guid.Parse(model.Id));

            // Получаем профиль текущего пользователя, используя его идентификатор.
            // Это позволяет нам обновить дополнительную информацию о пользователе, которая может быть хранится в профиле, такую как имя, фамилия и другие данные.
            //var currentUserProfile = _unitOfWork.UserProfiles.GetAll().FirstOrDefault(f => f.UserId == currentUser.Id);
            var currentUserProfile = _userService.GetProfileByUserId(currentUser.Id);

            if (!string.IsNullOrEmpty(model.Password))
                currentUser.Password = model.Password;

            if (!string.IsNullOrEmpty(model.Username))
                currentUser.Login = model.Username;

            if (!string.IsNullOrEmpty(model.FirstName))
                currentUserProfile.FirstName = model.Username;

            if (!string.IsNullOrEmpty(model.LastName))
                currentUserProfile.LastName = model.Username;

            if (!string.IsNullOrEmpty(model.Picture))
                currentUserProfile.Picture = model.Picture;

            _userService.Update(currentUser, currentUserProfile);
            //_unitOfWork.UserProfiles.Update(currentUserProfile);

            return RedirectToAction("Index", "Home");
        }


        // Обрабатывает запрос на выход пользователя из системы. Удаляет аутентификационные куки и перенаправляет пользователя на страницу входа.
        [Authorize]
        [Route("/User/Logout")]
        public void Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Redirect("/Login");
        }

        // Обрабатывает запрос на удаление учетной записи пользователя. Удаляет текущего пользователя из базы данных, используя его идентификатор, который хранится в клайме "UserId" после аутентификации.
        [Authorize]
        [Route("/User/Remove")]
        public void Remove()
        {
            _userService.Delete(User.FindFirst("UserId").Value);
            Response.Redirect("/");
        }

        [Authorize]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAll;
            return View("UserDetails", users);
        }

        [Authorize]
        public IActionResult GetUserById(Guid id)
        {
            var user = _userService.GetByIdDto(id);
            return View("UserDetails", user);
        }
    }
}