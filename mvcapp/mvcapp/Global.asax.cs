using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using mvcapp.Filters;
using mvcapp.FiltersApi;
using mvcapp.Handlers;

namespace mvcapp
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalFilters.Filters.Add(new BuggyActionFilter());
            GlobalFilters.Filters.Add(new CSPActionFilter());

            GlobalFilters.Filters.Add(new WebExceptionFilter());

            GlobalConfiguration.Configuration.Filters.Add(new ApiExceptionFilter());
            GlobalConfiguration.Configuration.Filters.Add(new ApiAntiCsrfActionFilter());
            GlobalConfiguration.Configuration.Services.
                Replace(typeof(IExceptionHandler), new ApiExceptionHandler());

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
