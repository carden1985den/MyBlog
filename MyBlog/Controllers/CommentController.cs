using BLL.Entity;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB.Models.Comement;

namespace WEB.Controllers
{
    public class CommentController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private readonly ILogger<CommentController> _logger;

        public CommentController(IUnitOfWork unitOfWork, ILogger<CommentController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        [Route("Add")]
        public IActionResult Add(CommentViewModel model)
        {
            _logger.LogInformation($"Пользователь {User.Identity.Name} создал комментарий");

            // Получить идентификатор текущего поста из модели
            var currentPostId = model.PostId;

            // Создать новый комментарий на основе данных из модели
            var comment = new Comment
            {
                Text = model.Text,
                PostId = Guid.Parse(currentPostId),
                UserId = Guid.Parse(User.FindFirst("UserId").Value)

            };

            // Сохранить комментарий в базе данных
            _unitOfWork.Comments.Create(comment);

            // Перенаправить пользователя обратно на страницу поста, чтобы увидеть новый комментарий
            return RedirectToAction("PostComment", "Post", new { id = currentPostId });
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var currentComment = _unitOfWork.Comments.GetById(Guid.Parse(id));

            var model = new CommentViewModel()
            {
                Id = currentComment.Id.ToString(),
                Text = currentComment.Text,
                PostId = currentComment.PostId.ToString(),
            };

            return View("Edit", model);
        }


        [Authorize]
        [HttpPost]
        public IActionResult Edit(CommentViewModel model)
        {
            _logger.LogInformation($"Пользователь {User.Identity.Name} изменил комментарий");

            var currentComment = _unitOfWork.Comments.GetById(Guid.Parse(model.Id));
            var currentPostId = _unitOfWork.Comments.GetById(currentComment.Id).PostId;

            if (currentComment.Text != model.Text)
                currentComment.Text = model.Text;

            _unitOfWork.Comments.Update(currentComment);

            return RedirectToAction("PostComment", "Post", new { id = currentPostId });
        }


        [Authorize]
        [HttpGet]
        public IActionResult Delete(string id)
        {
            _logger.LogInformation($"Пользователь {User.Identity.Name} удалил комментарий");
            // Получить комментарий по идентификатору
            var curretComment = _unitOfWork.Comments.GetById(Guid.Parse(id));
            // Получить идентификатор текущего поста, к которому принадлежит комментарий
            var currentPostId = _unitOfWork.Comments.GetById(Guid.Parse(id)).PostId;

            // Проверить, является ли текущий пользователь автором комментария
            if (Guid.Parse(User.FindFirst("UserId").Value) != curretComment.UserId)
                return RedirectToAction("PostComment", "Post", new { id = currentPostId });
            // Удалить комментарий из базы данных
            _unitOfWork.Comments.Delete(curretComment);
            return RedirectToAction("PostComment", "Post", new { id = currentPostId });
        }
    }
}
