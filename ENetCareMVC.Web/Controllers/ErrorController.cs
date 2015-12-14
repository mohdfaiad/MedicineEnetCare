using System.Web.Mvc;

namespace ENetCareMVC.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Generic()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Generic(string action)
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult MissingResource()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MissingResource(string action)
        {
            return RedirectToAction("Index", "Home");
        }
    }
}