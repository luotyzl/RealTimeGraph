using System.Web.Mvc;

namespace Rakon.TCE.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}