using System.Data;
using System.Linq;
using System.Web.Mvc;
using OMKT.Business;
using OMKT.Context;

namespace OMKT.Controllers
{
    [Authorize]
    public class InboxController : Controller
    {
        private readonly OMKTDB _db = new OMKTDB();


        public ActionResult Messages()
        {
            var oUser = (User)Session["User"];
            var msj = _db.Inboxes.Where(c => c.ToId == oUser.CustomerId);
            return Content("You have "+ msj +"  messages!");

        }
        //
        // GET: /Inbox/

        public ActionResult Index()
        {
            var oUser = (User)Session["User"];
            var msj = _db.Inboxes.Where(c => c.ToId == oUser.CustomerId);
            return PartialView("Index", msj.ToList());
        }

        //
        // GET: /Inbox/Details/5

        public ViewResult Details(int id)
        {
            Inbox inbox = _db.Inboxes.Find(id);
            return View(inbox);
        }

        //
        // GET: /Inbox/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Inbox/Create

        [HttpPost]
        public ActionResult Create(Inbox inbox)
        {
            if (ModelState.IsValid)
            {
                _db.Inboxes.Add(inbox);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(inbox);
        }

        //
        // GET: /Inbox/Edit/5

        public ActionResult Edit(int id)
        {
            Inbox inbox = _db.Inboxes.Find(id);
            return View(inbox);
        }

        //
        // POST: /Inbox/Edit/5

        [HttpPost]
        public ActionResult Edit(Inbox inbox)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(inbox).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(inbox);
        }

        //
        // GET: /Inbox/Delete/5

        public ActionResult Delete(int id)
        {
            Inbox inbox = _db.Inboxes.Find(id);
            return View(inbox);
        }

        //
        // POST: /Inbox/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Inbox inbox = _db.Inboxes.Find(id);
            _db.Inboxes.Remove(inbox);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}