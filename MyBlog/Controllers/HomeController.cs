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
        private IUnitOfWork _unitOfWork;
        private readonly ILogger<HomeController> _logger;
        public HomeController(IUnitOfWork unitOfWork, ILogger<HomeController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            var user = User;
            if (user.Identity.IsAuthenticated == true )
            {
                _logger.LogInformation($"{user.Identity.Name} searching tags by name ({HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()})");
            }
            else
            {
                _logger.LogInformation($"Anonymous user searching tags by name ({HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()})");
            }
            

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
