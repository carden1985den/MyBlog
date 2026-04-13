
using Core.Entity;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Models;
using System.Diagnostics;

namespace MyBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostService _postService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IPostService postService, ILogger<HomeController> logger)
        {
            _postService = postService;
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
            if (User.Identity.IsAuthenticated == true )
            {
                _logger.LogInformation($"{User.Identity.Name} searching tags by name ({HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()})");
            }
            else
            {
                _logger.LogInformation($"Anonymous user searching tags by name ({HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()})");
            }
            

            if (!string.IsNullOrEmpty(search))
            {
                IEnumerable<Post> serachedPost = _postService.GetAll().Where(n => n.Title.Contains(search)).ToList();

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
