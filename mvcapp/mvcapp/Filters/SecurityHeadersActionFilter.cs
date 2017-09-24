using System.Web.Mvc;

namespace mvcapp.Filters
{
    public class SecurityHeadersActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var headers = filterContext?.RequestContext?.HttpContext?.Response?.Headers;

            headers?.Add("Content-Security-Policy", 
                "script-src 'self'; style-src 'self'; img-src 'self';");
            headers?.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
            headers?.Add("X-Frame-Option", "deny");
            headers?.Add("X-XSS-Protection", "1; mode=block");
            headers?.Add("X-Permitted-Cross-Domain-Policies", "none");
            headers?.Add("Referrer-Policy", "no-referrer");
            
            base.OnActionExecuted(filterContext);
        }
    }
}

