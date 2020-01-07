using AnalyzeFiles.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnalyzeFiles.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index(ErrorModel model)
        {           
            return View(model);
        }
       
    }
}