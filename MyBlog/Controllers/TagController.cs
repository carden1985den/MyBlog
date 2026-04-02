using BLL.Entity;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Models;
using WEB.Models.Post;
using WEB.Models.Tag;

namespace WEB.Controllers
{
    public class TagController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public TagController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /*
        [Authorize]
        [HttpPost]
        public IActionResult Manage(TagsViewModel model)
        {

            // Получаем все теги отдной строкой со страницы через модель и разбиваем их на отдельные теги, удаляя лишние пробелы
            var curretTag = model.Tag.Split(';').ToList();

            // Получаем все теги из базы данных для данного поста
            var tagFromDB = _unitOfWork.Tags.GetAll().Where(t => t.PostId == Guid.Parse(model.PostId)).ToList();

            // 
            var deleteTag = tagFromDB.Where(t => !curretTag.Contains(t.Name)).ToList();

            foreach (var tag in deleteTag)
            {
                if (deleteTag != null)
                {
                    _unitOfWork.Tags.Delete(tag);
                }
            }


            // Проходим по каждому тегу и проверяем, существует ли он уже в базе данных для данного поста. Если нет - создаем новый тег
            foreach (var tagName in curretTag)
            {
                // Проверяем, существует ли тег с таким именем для данного поста в базе данных
                var existingTag = _unitOfWork.Tags.GetAll().FirstOrDefault(t => t.Name == tagName && t.PostId == Guid.Parse(model.PostId));

                if (existingTag is null && !string.IsNullOrEmpty(tagName))
                {
                    // Если тег не существует, создаем новый тег и сохраняем его в базе данных
                    _unitOfWork.Tags.Create(new Tag { PostId = Guid.Parse(model.PostId), Name = tagName });
                }
            }

            return RedirectToAction("PostComment", "Post", new { id = model.PostId });
        }
        */

        [HttpGet]
        [Authorize]
        public IActionResult ShowAll()
        {
            var model = new TagsViewModel();
            var allTegs = _unitOfWork.Tags.GetAll();

            // 
            model.Tags = allTegs.Select(t => new TagChekBox()
            {
                Id = t.Id,
                Name = t.Name,
                IsChecked = false
            }).ToList();

            return View("ShowAllTegs", model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(TagsViewModel model)
        {
            if (ModelState.IsValid == true && !string.IsNullOrEmpty(model.NewTagName))
            {
                var currentTag = _unitOfWork.Tags.GetAll().Where(t => t.Name == model.NewTagName.ToLower()).FirstOrDefault();

                if (currentTag is null)
                {
                    var tag = new Tag() {Name = model.NewTagName};

                    _unitOfWork.Tags.Create(tag);
                    return RedirectToAction("ShowAll", "Tag");
                }
                else
                {
                    ModelState.AddModelError("", "Указанный Тэг существует");
                    return View("ShowAllTegs", model);
                }
            }
            
            //ModelState.AddModelError("", "Не указано имя тэга");
            return View("ShowAllTegs", model);
        }


    }
}
