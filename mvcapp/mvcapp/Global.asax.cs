using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using mvcapp.Filters;

namespace mvcapp
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalFilters.Filters.Add(new BuggyActionFilter());
            GlobalFilters.Filters.Add(new CSPActionFilter());

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}