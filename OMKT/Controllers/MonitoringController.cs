using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OMKT.Business;
using OMKT.Context;
using OMKT.Models;

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

        public PartialViewResult AdvertHostMonitoring()
        {
            var performance = new List<MonitoringOverview>();
            var now = DateTime.Today;
            var monitoring = (from m in db.Monitoring
                              let dt = m.Timestamp
                              where m.Timestamp.Year == now.Year && m.Timestamp.Month == now.Month && m.Timestamp.Day == now.Day
                              group m by new { y = dt.Year, m = dt.Month, d = dt.Day, h = dt.Hour } into g
                              select new {  monitoring = g,
                                  total = g.Sum(c => c.Average) 
                              });

            var count = 8;
            foreach (var item in monitoring)
            {                
                var oMO = new MonitoringOverview();
                oMO.Timestamp = count.ToString() + ".00";
                oMO.Average = item.total.ToString().Replace(',', '.');
                performance.Add(oMO);
                count++;
            }

            return PartialView("AdvertHostMonitoring", performance);
        }

        public PartialViewResult AdvertHostMonitoringDaily(int? top)
        {
            var performance = new List<MonitoringOverview>();
            var now = DateTime.Today;
            int limit = top.HasValue ? Convert.ToInt16(top) : 7;
            var monitoring = (from m in db.Monitoring
                              let dt = m.Timestamp
                              group m by new { y = dt.Year, m = dt.Month, d = dt.Day } into g
                              select new
                              {
                                  monitoring = g,
                                  total = g.Sum(c => c.Average)
                              }).Take(limit);

            var count = 0;
            foreach (var item in monitoring)
            {
                var oMO = new MonitoringOverview();
                oMO.Timestamp = count.ToString();
                oMO.Average = item.total.ToString().Replace(',', '.');
                performance.Add(oMO);
                count++;
            }

            return PartialView("AdvertHostMonitoringDaily", performance);
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