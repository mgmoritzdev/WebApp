using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp4.Models;

namespace WebApp4.Controllers
{
    public class AngularController : Controller
    {
        MyFileDbContext db = new MyFileDbContext();

        // GET: Angular
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyFilesJson()
        {
            return Json(db.MyFiles.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult MyConfirmationsJson()
        {
            return Json(db.ConfirmMoves.ToList(), JsonRequestBehavior.AllowGet);
        }

        
    }
}