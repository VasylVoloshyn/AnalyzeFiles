using AnalyzeFiles.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnalyzeFiles.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    var filesLocation = ConfigurationManager.AppSettings["filesLocationFolder"];
                    string path = Path.Combine(filesLocation,
                                               Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    var model = new ErrorModel();
                    model.ErrorText = "ERROR:" + ex.Message.ToString();
                    return RedirectToAction("Index", "Error", model);                    
                }
            else
            {
                var model = new ErrorModel();
                model.ErrorText = "You have not specified a file.";
                return RedirectToAction("Index", "Error", model);
            }
            return View();
        }
    }
}