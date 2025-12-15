using System.Diagnostics;
using KineStat.Models;
using Microsoft.AspNetCore.Mvc;

namespace KineStat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Returns the default view for the Index page.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> that renders the Index view.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Returns the error view containing details about the current request for diagnostic purposes.
        /// </summary>
        /// <remarks>This action disables response caching to ensure that error details are not stored or
        /// reused. The returned view includes a request identifier that can be used for troubleshooting and correlation
        /// in logs.</remarks>
        /// <returns>An <see cref="IActionResult"/> that renders the error view with information about the request.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Used for testing error pages
        [HttpGet]
        public IActionResult TestAccessDenied()
        {
            return RedirectToAction("AccessDenied", "Error");
        }

        [HttpGet]
        public IActionResult TestUnauthorized()
        {
            return RedirectToAction("Unauthorized", "Error");
        }
    }
}
