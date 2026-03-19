using AutoMapper;
using BLL.Entity;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Metadata;
using WEB.Models.Post;
using WEB.Models.Tag;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WEB.Controllers
{
    public class PostController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public PostController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        [Route("/Post/New")]
        public IActionResult Create()
        {
            return View("New");
        }

        [Authorize]
        [HttpPost]
        [Route("/Post/New")]
        public IActionResult Create(PostViewModel model)
        {
            var post = _mapper.Map<Post>(model);

            post.UserId = _unitOfWork.Users.GetAll().FirstOrDefault(u => u.Login == User.Identity.Name).Id;

            _unitOfWork.Posts.Create(post);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        [Route("/Post/Edit")]
        public IActionResult Edit(string id)
        {
            var currentPost = _unitOfWork.Posts.GetById(Guid.Parse(id));

            // Проверяем, является ли текущий пользователь автором поста, если нет - выдаем ошибку и перенаправляем на главную страницу
            if (User.FindFirst("UserId").Value != currentPost.UserId.ToString())
            {
                TempData["EditPostError"] = "Нет прав на редактирование поста";
                return RedirectToAction("Index", "Home");
            }

            var model = _mapper.Map<PostViewModel>(currentPost);

            // Если пользователь является автором поста, отображаем страницу редактирования
            return View("Edit", model);
        }

        [Authorize]
        [HttpPost]
        [Route("/Post/Edit")]
        public IActionResult Edit(PostViewModel model)
        {

            if (model.Id is not null)
            {
                // Получаем текущий пост из базы данных
                var currentPost = _unitOfWork.Posts.GetById(Guid.Parse(model.Id));

                if (currentPost.Title != model.Title)
                    currentPost.Title = model.Title;

                if (currentPost.Text != model.Text)
                    currentPost.Text = model.Text;

                _unitOfWork.Posts.Update(currentPost);
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Delete(string id)
        {
            var currentPost = _unitOfWork.Posts.GetById(Guid.Parse(id));
            if (currentPost is not null)
            {
                // Проверяем, является ли текущий пользователь автором поста, если нет - выдаем ошибку и перенаправляем на главную страницу
                if (User.FindFirst("UserId").Value != currentPost.UserId.ToString())
                {
                    TempData["EditPostError"] = "Нет прав на редактирование поста";
                    return RedirectToAction("Index", "Home");
                }

                _unitOfWork.Posts.Delete(currentPost);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("/Post/PostComment")]
        public IActionResult PostComment(string id)
        {
            var currentPost = _unitOfWork.Posts.GetById(Guid.Parse(id));

            if (currentPost is null)
                return RedirectToAction("Index", "Home");

            var model = _mapper.Map<PostViewModel>(currentPost);
            return View("PostComment", model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult PostComment(TagViewModel model)
        {
            // Получаем все теги отднойстрокой со страницы через модель и разбиваем их на отдельные теги, удаляя лишние пробелы
            var curretTag = model.Tag.Split(';').Select(t => t.Trim()).ToList();

            // Проходим по каждому тегу и проверяем, существует ли он уже в базе данных для данного поста. Если нет - создаем новый тег
            foreach (var tagName in curretTag)
            {
                var existingTag = _unitOfWork.Tags.GetAll().FirstOrDefault(t => t.Name == tagName && t.PostId == Guid.Parse(model.PostId));

                if (existingTag is null)
                {
                    _unitOfWork.Tags.Create(new Tag
                    {
                        PostId = Guid.Parse(model.PostId),
                        Name = tagName
                    });
                }
            }
            //var allTegs = _unitOfWork.Tags.GetAll().Where(t => t.PostId == Guid.Parse(model.PostId));

           
            return RedirectToAction("PostComment", "Post",  new { id = model.PostId });
        }

        /*
        public IActionResult Search()
        {
            return View();
        }
        */
    }
}
