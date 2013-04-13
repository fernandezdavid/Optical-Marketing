using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using OMKT.Business;
using OMKT.Context;

namespace OMKT.Controllers
{
    public class AlertController : Controller
    {
        private OMKTDB db = new OMKTDB();

        /**
        * method CurrentAlerts()
        *
        * List a alerts to be atended by the customer
        *
        * @since 10/04/2013
        * @return alert collection
        */

        public PartialViewResult CurrentAlerts()
        {
            var oUser = (User)Session["User"];
            var alerts = db.Alerts.Where(c => c.CustomerId == oUser.CustomerId).OrderByDescending(i => i.Date);
            return PartialView("AlertsPartial");
        }

        //
        // GET: /Alert/

        public ViewResult Index()
        {
            var alerts = db.Alerts.Include(a => a.Customer);
            return View(alerts.ToList());
        }

        //
        // GET: /Alert/Details/5

        public ViewResult Details(int id)
        {
            Alert alert = db.Alerts.Find(id);
            return View(alert);
        }

        //
        // GET: /Alert/Create

        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerID", "Name");
            return View();
        }

        //
        // POST: /Alert/Create

        [HttpPost]
        public ActionResult Create(Alert alert)
        {
            if (ModelState.IsValid)
            {
                db.Alerts.Add(alert);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerID", "Name", alert.CustomerId);
            return View(alert);
        }

        //
        // GET: /Alert/Edit/5

        public ActionResult Edit(int id)
        {
            Alert alert = db.Alerts.Find(id);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerID", "Name", alert.CustomerId);
            return View(alert);
        }

        //
        // POST: /Alert/Edit/5

        [HttpPost]
        public ActionResult Edit(Alert alert)
        {
            if (ModelState.IsValid)
            {
                db.Entry(alert).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerID", "Name", alert.CustomerId);
            return View(alert);
        }

        //
        // GET: /Alert/Delete/5

        public ActionResult Delete(int id)
        {
            Alert alert = db.Alerts.Find(id);
            return View(alert);
        }

        //
        // POST: /Alert/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Alert alert = db.Alerts.Find(id);
            db.Alerts.Remove(alert);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}