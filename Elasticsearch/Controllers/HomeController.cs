using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Elasticsearch.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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
        public ActionResult InsertContent()
        {
            new EsClient().InsertContent();
            return View();
        }
        public void UpdateContent(string _id)
        {
            new EsClient().UpdateContent(_id);
        }
        public void DeleteContent(string _id)
        {
            new EsClient().DeleteContent(_id);
        }
        public JsonResult GetContent(string _id)
        {
            return Json(new EsClient().GetContent(_id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchAll(string search)
        {
            return Json(new EsClient().SearchAll(search), JsonRequestBehavior.AllowGet);
        }
    }
}