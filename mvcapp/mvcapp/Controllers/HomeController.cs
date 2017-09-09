﻿using System.Web.Mvc;
using NLog;

namespace mvcapp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var showCaseLogger = LogManager.GetLogger("ShowCaseLogger");

            showCaseLogger.Debug("This is debug log.");
            showCaseLogger.Info("This is info log.");
            showCaseLogger.Warn("This is warning log.");
            showCaseLogger.Error("This is error log.");
            showCaseLogger.Fatal("This is fatal log.");

#if DEBUG
            // development code here only
#elif STAGE
            // stage related code here only
#else
            // probably production related code here only
#endif

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}