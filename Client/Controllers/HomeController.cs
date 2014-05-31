using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Client.Controllers
{
    [OutputCache(Duration = 1000)]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(NoStore = true,Duration = 0)]
        public ActionResult Hits(string callback)
        {
            return new JavaScriptResult() { Script = GetJsonp(callback) };
        }

        private string GetJsonp(string callback)
        {
            var cqw = (new JavaScriptSerializer()).Serialize(new CustomModule.CustModule().AllHits);
            return string.Format("{0}({1});", callback, cqw);
        }

        public JsonResult JsonHits()
        {
            var ca = new CustomModule.CustModule().AllHits;
            var c = new JsonResult() { Data = ca ,JsonRequestBehavior=JsonRequestBehavior.AllowGet};
            return c;
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