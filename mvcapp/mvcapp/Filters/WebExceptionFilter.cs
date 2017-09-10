﻿using System;
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
            throw new NotImplementedException("Not sure if we need this right now.");
        }

        private static void AdjustResponse(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }

    public class GeneralError
    {
        public Exception Exception { get; set; }
        public bool IsDebug => HttpContext.Current.IsDebuggingEnabled;

        public GeneralError(Exception exception)
        {
            Exception = exception;
        }
    }
}