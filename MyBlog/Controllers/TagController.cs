using Core.Entity;
using Core.Interfaces.Services;
using Core.Models.Post;
using Core.Models.Tag;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Models;

namespace WEB.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult ShowAll()
        {
            var model = new TagsViewModel();
            var allTegs = _tagService.GetAll();

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
                var currentTag = _tagService.GetAll().Where(t => t.Name == model.NewTagName.ToLower()).FirstOrDefault();

                if (currentTag is null)
                {
                    var tag = new Tag() {Name = model.NewTagName};

                    _tagService.Create(tag);
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
