using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NLog;

namespace mvcapp.Filters
{
    public class WebExceptionFilter : IExceptionFilter
    {
        const string XmlHttpRequest = "XMLHttpRequest";
        const string XRequestedWith = "X-Requested-With";

        public void OnException(ExceptionContext filterContext)
        {
            var exceptionLogger = LogManager.GetLogger("ExceptionLogger");

            bool isAjaxRequest = filterContext.HttpContext.Request.Headers[XRequestedWith] == XmlHttpRequest;

            if (isAjaxRequest)
            {
                HandleAjaxException(filterContext);
            }
            else
            {
                HandleStandardException(filterContext);
            }

            exceptionLogger.Error(filterContext.Exception);
        }

        private static void HandleStandardException(ExceptionContext filterContext)
        {
            const string errorViewName = "Error";

            var exception = filterContext.Exception;
            var generalErrorDto = new GeneralError(exception);
            var viewData = filterContext.Controller.ViewData;
            viewData.Model = generalErrorDto;
            
            filterContext.Result = new ViewResult
            {
                ViewName = errorViewName,
                ViewData = viewData,
                TempData = filterContext.Controller.TempData,
            };

            AdjustResponse(filterContext);
        }

        private static void HandleAjaxException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception;

#if DEBUG
            var generalErrorDto = new JsonError(exception);
#else
            var generalErrorDto = new JsonError();
#endif
            filterContext.Result = new JsonResult {Data = generalErrorDto,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet};

            AdjustResponse(filterContext);
        }

        private static void AdjustResponse(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }

    public class GeneralError
    {
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public bool IsDebug => HttpContext.Current.IsDebuggingEnabled;

        public GeneralError()
        {
            Message = "Error occured.";
        }

        public GeneralError(Exception exception) : this()
        {
            Exception = exception;
        }
    }

    public class JsonError
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public JsonError()
        {
            Message = "Error occured.";
        }

        public JsonError(string message) 
        {
            Message = message;
        }

        public JsonError(Exception exception) : this(exception.Message)
        {
            StackTrace = exception.StackTrace;
        }

        public override string ToString()
        {
            return Message + Environment.NewLine + StackTrace;
        }
    }
}