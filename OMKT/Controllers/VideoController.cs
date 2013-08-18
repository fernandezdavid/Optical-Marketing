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
    public class VideoController : Controller
    {
        private OMKTDB db = new OMKTDB();

        //
        // GET: /Video/

        public ViewResult Index()
        {
            var videos = db.Videos.Include(v => v.AdvertType).Include(v => v.AdvertState);
            return View(videos.ToList());
        }

        //
        // GET: /Video/Details/5

        public ViewResult Details(int id)
        {
            Video video = db.Videos.Find(id);
            return View(video);
        }

        //
        // GET: /Video/Create

        public ActionResult Create()
        {
            ViewBag.AdvertTypeId = new SelectList(db.AdvertTypes, "AdvertTypeId", "Description");
            ViewBag.AdvertStateId = new SelectList(db.AdvertStates, "AdvertstateId", "Description");
            return View();
        } 

        //
        // POST: /Video/Create

        [HttpPost]
        public ActionResult Create(Video video)
        {
            if (ModelState.IsValid)
            {
                db.Videos.Add(video);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.AdvertTypeId = new SelectList(db.AdvertTypes, "AdvertTypeId", "Description", video.AdvertTypeId);
            ViewBag.AdvertStateId = new SelectList(db.AdvertStates, "AdvertstateId", "Description", video.AdvertStateId);
            return View(video);
        }
        
        //
        // GET: /Video/Edit/5
 
        public ActionResult Edit(int id)
        {
            Video video = db.Videos.Find(id);
            ViewBag.AdvertTypeId = new SelectList(db.AdvertTypes, "AdvertTypeId", "Description", video.AdvertTypeId);
            ViewBag.AdvertStateId = new SelectList(db.AdvertStates, "AdvertstateId", "Description", video.AdvertStateId);
            return View(video);
        }

        //
        // POST: /Video/Edit/5

        [HttpPost]
        public ActionResult Edit(Video video)
        {
            if (ModelState.IsValid)
            {
                db.Entry(video).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdvertTypeId = new SelectList(db.AdvertTypes, "AdvertTypeId", "Description", video.AdvertTypeId);
            ViewBag.AdvertStateId = new SelectList(db.AdvertStates, "AdvertstateId", "Description", video.AdvertStateId);
            return View(video);
        }

        //
        // GET: /Video/Delete/5
 
        public ActionResult Delete(int id)
        {
            Video video = db.Videos.Find(id);
            return View(video);
        }

        //
        // POST: /Video/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Video video = db.Videos.Find(id);
            db.Adverts.Remove(video);
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