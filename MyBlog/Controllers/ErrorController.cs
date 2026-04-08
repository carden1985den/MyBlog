using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WEB.Controllers
{
    public class ErrorController : Controller
    {
        private static ILogger<ErrorController> _logger;
        public ErrorController(ILogger<ErrorController> logger)
        {

        }
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

        public IActionResult SomthingWrong()
        {
            // Получаем информацию об исключении
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature?.Error != null)
            {
                // Логируем ошибку для вашего NLog
                _logger.LogError(exceptionHandlerPathFeature.Error,
                    "Непредвиденная ошибка на пути: {Path}", exceptionHandlerPathFeature.Path);
            }

            return View();
        }
    }
}
