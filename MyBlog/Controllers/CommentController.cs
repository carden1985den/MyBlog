using BLL.Services;
using Core.Entity;
using Core.Interfaces.Services;
using Core.Models.Comement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WEB.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly ILogger<CommentController> _logger;

        public CommentController(ICommentService commentSrevice, ILogger<CommentController> logger)
        {
            _commentService = commentSrevice;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        [Route("Add")]
        public IActionResult Add(CommentViewModel model)
        {
            _logger.LogInformation($"Пользователь {User.Identity?.Name} создал комментарий");

            // Проверяем модель на валидность
            if (ModelState.IsValid != true)
                return RedirectToAction("PostComment", "Post", new { id = model.PostId });

            // Получаем ID текущего пользователя
            var userGuid = User.FindFirst("UserId")?.Value;

            // если сессия истекла или что то пошло не так, Необходима авторизация
            if (userGuid == null)
                return Unauthorized();

            // Получить идентификатор текущего поста из модели
            if (!Guid.TryParse(model.PostId, out Guid currentPostId))
                return BadRequest("Не корректный ID поста");

            // Создать новый комментарий на основе данных из модели
            var comment = new Comment
            {
                Text = model.Text,
                PostId = currentPostId,
                UserId = Guid.Parse(userGuid)

            };

            // Сохранить комментарий в базе данных
            _commentService.Create(comment);

            // Перенаправить пользователя обратно на страницу поста, чтобы увидеть новый комментарий
            return RedirectToAction("PostComment", "Post", new { id = currentPostId });
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var currentComment = _commentService.GetById(Guid.Parse(id));

            if (currentComment == null)
                return BadRequest("Не верный guid поста");

            var model = new CommentViewModel()
            {
                Id = currentComment.Id.ToString(),
                Text = currentComment.Text,
                PostId = currentComment.PostId?.ToString(),
            };

            return View("Edit", model);
        }


        [Authorize]
        [HttpPost]
        public IActionResult Edit(CommentViewModel model)
        {
            _logger.LogInformation($"Пользователь {User.Identity?.Name} изменил комментарий");

            var currentComment = _commentService.GetById(Guid.Parse(model.Id));
            var currentPostId = _commentService.GetById(currentComment.Id).PostId;

            if (currentComment.Text != model.Text)
                currentComment.Text = model.Text;

            _commentService.Update(currentComment);

            return RedirectToAction("PostComment", "Post", new { id = currentPostId });
        }


        [Authorize]
        [HttpGet]
        public IActionResult Delete(string id)
        {
            _logger.LogInformation($"Пользователь {User.Identity?.Name} удалил комментарий");
            // Получить комментарий по идентификатору
            var curretComment = _commentService.GetById(Guid.Parse(id));
            // Получить идентификатор текущего поста, к которому принадлежит комментарий
            var currentPostId = _commentService.GetById(Guid.Parse(id)).PostId;

            // Проверить, является ли текущий пользователь автором комментария
            var claimUserId = User.FindFirst("UserId")?.Value;

            if (Guid.TryParse(claimUserId, out Guid currentUserId))
            {
                if (currentUserId != curretComment?.UserId)
                {
                    return RedirectToAction("PostComment", "Post", new { id = currentPostId });
                }
            }

            // Удалить комментарий из базы данных
            _commentService.Delete(curretComment);
            return RedirectToAction("PostComment", "Post", new { id = currentPostId });
        }
    }
}