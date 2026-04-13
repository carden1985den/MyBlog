using BLL.Services;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [Authorize]
        [Route("Delete")]
        public IActionResult Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid postId))
                return BadRequest(new { message = "Не верный ID" });

            var post = _postService.GetById(postId);

            if (post == null)
                return BadRequest(new { message = "Пост не найден" });

            bool result = _postService.Delete(post);
            
            if (result == false)
                return StatusCode(500,new { message = "Не удалось удалить пост" });

            return Ok(new { message = "Пост удалён" });
        }

        [Authorize]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var posts = _postService.GetAll();
            return Ok(posts);
        }
    }
}
