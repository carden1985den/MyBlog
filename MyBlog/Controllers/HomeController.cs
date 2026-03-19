using AutoMapper;
using BLL.Entity;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Models;
using System.Collections;
using System.Diagnostics;
using WEB.Models.Post;

namespace MyBlog.Controllers
{
    public class HomeController : Controller
    {
        private UnitOfWork _unitOfWork;
        public HomeController(ApplicationDbContext context)
        {
            _unitOfWork = new UnitOfWork(context);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                IEnumerable<Post> serachedPost = _unitOfWork.Posts.GetAll().Where(n => n.Title.Contains(search)).ToList();

                if (serachedPost is not null)
                {
                    return View("Index", serachedPost);
                }
            }
            return View("Index");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
