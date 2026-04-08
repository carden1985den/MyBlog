using AutoMapper;
using BLL.Entity;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using WEB.Models.Post;
using WEB.Models.Tag;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WEB.Controllers
{
    public class PostController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly ILogger<PostController> _logger;
        public PostController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<PostController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }


        [Authorize(Roles = "Admin, Editor")]
        [HttpGet]
        [Route("/Post/New")]
        public IActionResult Create()
        {
            var model = new PostViewModel();

            model.AvailableTags = _unitOfWork.Tags.GetAll().Select(t =>
                new TagChekBox()
                {
                    Id = t.Id,
                    Name = t.Name,
                    IsChecked = false
                }
            ).ToList();

            return View("New", model);
        }

        [Authorize(Roles = "Admin, Editor")]
        [HttpPost]
        [Route("/Post/New")]
        public IActionResult Create(PostViewModel model)
        {
            _logger.LogInformation($"Пользователь {User.Identity.Name} создал пост");

            if (ModelState.IsValid == true)
            {
                var selectedTagId = model.AvailableTags.Where(t => t.IsChecked == true).Select(t => t.Id).ToList<Guid>();

                var post = new Post()
                {
                    Title = model.Title,
                    Text = model.Text,
                    UserId = _unitOfWork.Users.GetAll().FirstOrDefault(u => u.Login == User.Identity.Name).Id,
                    TagId = selectedTagId?.Count > 0 ? selectedTagId[0] : Guid.Empty,
                };


                //var selectedTags = _unitOfWork.Tags.GetAll().Where(t => model.SelectedTagId.Contains(t.Id.ToString())).ToList();

                //post.Tags = selectedTags;

                _unitOfWork.Posts.Create(post);

                return RedirectToAction("Index", "Home");
            }

            return View("New", model);
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
                _logger.LogError($"Пользователь {User.Identity.Name} попытался отредактировать пост, не являясь его автором");

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
            _logger.LogInformation($"Пользователь {User.Identity.Name} отредактировал пост");

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
            // Берем пост из БД по ID
            var currentPost = _unitOfWork.Posts.GetById(Guid.Parse(id));

            // если поста нет, переходим на домашнюю страницу
            if (currentPost is null)
                return RedirectToAction("Index", "Home");

            // Создаём объект PostViewModel
            var model = new PostViewModel() { 
                Id = id,
                Title = currentPost.Title,
                Text = currentPost.Text,
                
            };

            if (!Guid.TryParse(id, out var postId))
                return NotFound();

            // берем из БД связанный комментарии
            var allComments = _unitOfWork.Comments.GetAll().Where(c =>c.PostId == postId);

            model.AllComments = (from c in _unitOfWork.Comments.GetAll()
                                 join u in _unitOfWork.Users.GetAll() on c.UserId equals u.Id
                                 where c.PostId == postId
                                 select new CurrentComment()
                                 {
                                     Id = c.Id.ToString(),
                                     Text = c.Text,
                                     CreateDate = c.Created,
                                     UserId = c.UserId.ToString(),
                                     PostId = c.PostId.ToString(),
                                     UserName = u.Login
                                 }).ToList(
                );

            //var model = _mapper.Map<PostViewModel>(currentPost);
            return View("PostComment", model);
        }
    }
}
