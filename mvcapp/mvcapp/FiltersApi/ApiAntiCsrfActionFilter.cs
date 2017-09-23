using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;
using mvcapp.Filters;
using NLog;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

namespace mvcapp.FiltersApi
{
    public class ApiAntiCsrfActionFilter : ActionFilterAttribute
    {
        const string XXsrfToken = "X-XSRF-TOKEN";
        const string XsrfToken = "XSRF-TOKEN";
        const string XsrfCookie = "XSRF-COOKIE";
        const double CookieMaxAgeMinutes = 30D;
        const string Path = "/";

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext?.Response == null || 
                actionExecutedContext.Request.Method != HttpMethod.Get)
            {
                base.OnActionExecuted(actionExecutedContext);
                return;
            }

            string xsrfCookie = GetXsrfCookie(actionExecutedContext.Request.
                Headers.GetCookies(XsrfCookie));
            string newXsrfCookie;
            string newXsrfToken;

            AntiForgery.GetTokens(xsrfCookie, out newXsrfCookie, out newXsrfToken);

            var cookieWithXsrfToken = new CookieHeaderValue(XsrfToken, newXsrfToken)
            {
                Domain = actionExecutedContext.Request.RequestUri.Host,
                MaxAge = TimeSpan.FromMinutes(CookieMaxAgeMinutes),
                Path = Path,
                HttpOnly = false,
            };

            var cookieWithXsrfCookie = new CookieHeaderValue(XsrfCookie, newXsrfCookie ?? xsrfCookie)
            {
                Domain = actionExecutedContext.Request.RequestUri.Host,
                MaxAge = TimeSpan.FromMinutes(CookieMaxAgeMinutes),
                Path = Path,
                HttpOnly = true,
            };

            actionExecutedContext.Response.Headers.
                AddCookies(new[] {cookieWithXsrfToken, cookieWithXsrfCookie});
            base.OnActionExecuted(actionExecutedContext);
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext?.Request == null)
            {
                base.OnActionExecuting(actionContext);
                return;
            }

            if (actionContext.Request.Method == HttpMethod.Get)
            {
                base.OnActionExecuting(actionContext);
                return;
            }

            if (actionContext.Request.Method == HttpMethod.Post ||
                actionContext.Request.Method == HttpMethod.Put ||
                actionContext.Request.Method == HttpMethod.Delete)
            {
                string xsrfCookie = GetXsrfCookie(actionContext.
                    Request.Headers.GetCookies(XsrfCookie));
                string xsrfToken = GetXsrfToken(actionContext);

                if (IsTokenValid(xsrfCookie, xsrfToken))
                {
                    base.OnActionExecuting(actionContext);
                    return;
                }
            }

            SendErrorMessage(actionContext);
        }

        private void SendErrorMessage(HttpActionContext actionContext)
        {
            var generalErrorDto = new GeneralError();
            #if DEBUG
            generalErrorDto.Message = "Anti XSRF token is invalid.";
            #endif
            actionContext.Response = actionContext.
                Request.CreateResponse(HttpStatusCode.BadRequest, generalErrorDto);
            base.OnActionExecuting(actionContext);
        }

        private static string GetXsrfToken(HttpActionContext actionContext)
        {
            KeyValuePair<string, IEnumerable<string>> xsrfHeader =
                actionContext.Request.Headers.FirstOrDefault(x => x.Key == XXsrfToken);

            if (xsrfHeader.Key == XXsrfToken && xsrfHeader.Value.Any())
            {
                return xsrfHeader.Value.First();
            }

            return string.Empty;
        }

        private static string GetXsrfCookie(Collection<CookieHeaderValue> xsrfCookieHeader)
        {
            if (xsrfCookieHeader.Any() && xsrfCookieHeader[0].Cookies.Any())
            {
                CookieState xsrfCookieState = xsrfCookieHeader[0].Cookies.FirstOrDefault(c => c.Name == XsrfCookie);
                return xsrfCookieState?.Value;
            }

            return string.Empty;
        }

        private static bool IsTokenValid(string cookieValue, string tokenValue)
        {
            try
            {
                AntiForgery.Validate(cookieValue, tokenValue);
            }
            catch (HttpAntiForgeryException exception)
            {
                var xsrfLogger = LogManager.GetLogger("XSRFLogger");
                xsrfLogger.Fatal(exception,
                    $"Anti XSRF token is invalid. " +
                    $"{HttpContext.Current.User.Identity.Name} has been probably attacked.");
                return false;
            }

            return true;
        }
    }
}