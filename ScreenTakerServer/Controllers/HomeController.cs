using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace ScreenTakerServer.Controllers
{
    using ScreenTakerServer.Models;
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            AppConstants.Folder = HttpContext.Request.PhysicalApplicationPath + "Upload";
            return View();
        }
        public ActionResult Abousst()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult getFilesInEachDirectories()
        {
            string[] mouseStatusFilePaths = Directory.GetFiles(AppConstants.Folder + "\\MouseStatus");
            string[] screenshotFilePaths = Directory.GetFiles(AppConstants.Folder + "\\Screenshots");
            string[] MessageFilePaths = Directory.GetFiles(AppConstants.Folder + "\\Messages");
            string rootPath = AppConstants.Folder;
            var jsonData = new
            {
                mouseStatusFilePaths,
                screenshotFilePaths,
                MessageFilePaths
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}