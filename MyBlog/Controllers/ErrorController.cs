using Microsoft.AspNetCore.Mvc;

namespace WEB.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult ResourceNotFound()
        {
            Response.StatusCode = 404; // Устанавливаем статус код 404 для страницы "Ресурс не найден"
            return View();
        }

        public IActionResult AcceessDenied()
        {
            Response.StatusCode = 403; // Устанавливаем статус код 403 для страницы "Доступ запрещен"
            return View();
        }
    }
}
