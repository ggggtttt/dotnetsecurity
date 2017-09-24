using System.Diagnostics;
using System.Web.Mvc;

namespace mvcapp.Filters
{
    public class CSPActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            string url = filterContext?.HttpContext?.Request?.Url?.AbsoluteUri;
            Debug.WriteLine($"CSPActionFilter, begining of request: {url}");

            //filterContext?.RequestContext?.HttpContext?.Response?.Headers.Add("Content-Security-Policy", "script-src 'self'; style-src 'self'; img-src 'self';");
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string url = filterContext?.HttpContext?.Request?.Url?.AbsoluteUri;
            Debug.WriteLine($"CSPActionFilter, end of request: {url}");
            base.OnActionExecuted(filterContext);
        }
    }
}