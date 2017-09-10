using System.Diagnostics;
using System.Web.Mvc;

namespace mvcapp.Filters
{
    public class BuggyActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string url = filterContext?.HttpContext?.Request?.Url?.AbsoluteUri;
            Debug.WriteLine($"BuggyActionFilter, begining of request: {url}");
            // throw new Exception("Intentionally throw exception.");
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string url = filterContext?.HttpContext?.Request?.Url?.AbsoluteUri;
            Debug.WriteLine($"BuggyActionFilter, end of request: {url}");
            base.OnActionExecuted(filterContext);
        }
    }
}