using System.Linq;
using System.Web.Mvc;
using OMKT.Context;
using OMKT.Models;

namespace OMKT.Controllers
{
    public class HomeController : Controller
    {
        private readonly OMKTDB _db = new OMKTDB();

        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
                Session["User"] = _db.Users.FirstOrDefault(x => x.Username == User.Identity.Name);
            return View();
        }

        public ActionResult Editores()
        {
            return View();
        }

        public ActionResult Empresa()
        {
            return View();
        }

        public ActionResult Contacto()
        {
            return View();
        }

        public ActionResult Anunciantes()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contacto(ContactUsModel model)
        {
            if (ModelState.IsValid)
            {
            }
            return View();
        }
    }
}