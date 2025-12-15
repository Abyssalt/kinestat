using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KineStat.Filters
{
    /// <summary>
    /// Ensures user is authenticated via session. Redirects to Unauthorized page if not.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new RedirectToActionResult("Unauthorized", "Error", null);
            }
        }
    }

    /// <summary>
    /// Restricts access to physiotherapists only. Redirects to AccessDenied if not a physio.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizePhysioAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.Session.GetString("UserId");
            var userRole = context.HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new RedirectToActionResult("Unauthorized", "Error", null);
            }
            else if (userRole != "Physio")
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Error", null);
            }
        }
    }

    /// <summary>
    /// Restricts access to administrators only. Redirects to AccessDenied if not an admin.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.Session.GetString("UserId");
            var userRole = context.HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new RedirectToActionResult("Unauthorized", "Error", null);
            }
            else if (userRole != "Admin")
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Error", null);
            }
        }
    }
}