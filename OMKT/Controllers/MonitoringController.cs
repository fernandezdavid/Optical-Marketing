using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OMKT.Business;
using OMKT.Context;

namespace OMKT.Controllers
{ 
    public class MonitoringController : Controller
    {
        private OMKTDB db = new OMKTDB();

        //
        // GET: /Monitoring/

        public ViewResult Index()
        {
            var monitoring = db.Monitoring.Include(m => m.AdvertHost);
            return View(monitoring.ToList());
        }

        //
        // GET: /Monitoring/Details/5

        public ViewResult Details(int id)
        {
            Monitoring monitoring = db.Monitoring.Find(id);
            return View(monitoring);
        }

        //
        // GET: /Monitoring/Create

        public ActionResult Create()
        {
            ViewBag.AdvertHostID = new SelectList(db.AdvertHosts, "AdvertHostId", "AdvertHostName");
            return View();
        } 

        //
        // POST: /Monitoring/Create

        [HttpPost]
        public ActionResult Create(Monitoring monitoring)
        {
            if (ModelState.IsValid)
            {
                db.Monitoring.Add(monitoring);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.AdvertHostID = new SelectList(db.AdvertHosts, "AdvertHostId", "AdvertHostName", monitoring.AdvertHostID);
            return View(monitoring);
        }
        
        //
        // GET: /Monitoring/Edit/5
 
        public ActionResult Edit(int id)
        {
            Monitoring monitoring = db.Monitoring.Find(id);
            ViewBag.AdvertHostID = new SelectList(db.AdvertHosts, "AdvertHostId", "AdvertHostName", monitoring.AdvertHostID);
            return View(monitoring);
        }

        //
        // POST: /Monitoring/Edit/5

        [HttpPost]
        public ActionResult Edit(Monitoring monitoring)
        {
            if (ModelState.IsValid)
            {
                db.Entry(monitoring).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdvertHostID = new SelectList(db.AdvertHosts, "AdvertHostId", "AdvertHostName", monitoring.AdvertHostID);
            return View(monitoring);
        }

        //
        // GET: /Monitoring/Delete/5
 
        public ActionResult Delete(int id)
        {
            Monitoring monitoring = db.Monitoring.Find(id);
            return View(monitoring);
        }

        public PartialViewResult AdvertHostMonitoring(int? hostId)
        {
            if (!hostId.HasValue) hostId = 1;
            //var adverthosts = db.AdvertHosts.OrderByDescending(h => h.AdvertHostName).Include("AdvertHostCategory");
            var monitoring = db.Monitoring.Where(c => c.AdvertHostID == hostId);
            return PartialView("AdvertHostMonitoring", monitoring);
        }

        //
        // POST: /Monitoring/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Monitoring monitoring = db.Monitoring.Find(id);
            db.Monitoring.Remove(monitoring);
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