using Microsoft.AspNetCore.Mvc;

namespace KineStat.Controllers
{
    public class ErrorController : Controller
    {
        /// <summary>
        /// Displays the access denied (403) error page.
        /// </summary>
        [HttpGet]
        public IActionResult AccessDenied()
        {
            Response.StatusCode = 403;
            return View();
        }

        /// <summary>
        /// Displays the unauthorized (401) error page.
        /// </summary>
        [HttpGet]
        public IActionResult Unauthorized()
        {
            Response.StatusCode = 401;
            return View();
        }

        /// <summary>
        /// Displays the not found (404) error page.
        /// </summary>
        [HttpGet]
        public IActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}