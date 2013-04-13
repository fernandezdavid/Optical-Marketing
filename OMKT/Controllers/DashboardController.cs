using System.Web.Mvc;

namespace OMKT.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Help()
        {
            return View();
        }
    }
}