using System;
using System.Diagnostics;
using System.Web;

namespace mvcapp.HttpModules
{
    public class LogfulHttpModule : IHttpModule
    {
        public void Init(HttpApplication application)
        {
            application.BeginRequest += Application_BeginRequest;
            application.EndRequest += Application_EndRequest;
        }

        private void Application_BeginRequest(Object source, EventArgs e)
        {
            var url = GetUrl(source);
            Debug.WriteLine($"Begining of request: {url}");
        }

        private void Application_EndRequest(Object source, EventArgs e)
        {
            var url = GetUrl(source);
            Debug.WriteLine($"End of request: {url}");
        }

        private static string GetUrl(object source)
        {
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
            return context.Request.Url.AbsoluteUri;
        }

        public void Dispose()
        {
        }
    }
}
