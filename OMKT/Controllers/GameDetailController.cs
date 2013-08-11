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
    [Authorize]
    public class GameDetailController : Controller
    {
        private OMKTDB db = new OMKTDB();

        public PartialViewResult IndexByGame(int id)
        {
            ViewBag.AdvertId = id;
            var oGame = db.Games.Find(id);
            ViewBag.Game = oGame;
            var oGameDetails = db.GameDetails.Include(i => i.Game).Where(i => i.AdvertId == id);
            return PartialView("GameDetailPartialList", oGameDetails.ToList());
        }

        public PartialViewResult IndexByMemoryGame(int id)
        {
            ViewBag.AdvertId = id;
            var oGame = db.Games.FirstOrDefault(i => i.AdvertId == id);
            ViewBag.Game = oGame;

            var oGameDetails = db.GameDetails.Include(i => i.Game).Where(i => i.AdvertId == id);
            return PartialView("MemoryGamePartialList", oGameDetails.ToList());

        }
        //
        // GET: /GameDetail/

        public ViewResult Index()
        {
            var gamedetails = db.GameDetails.Include(g => g.Game);
            return View(gamedetails.ToList());
        }

        //
        // GET: /GameDetail/Details/5

        public ViewResult Details(int id)
        {
            GameDetail gamedetail = db.GameDetails.Find(id);
            return View(gamedetail);
        }

        //
        // GET: /GameDetail/Create

        public ActionResult Create(int? id)
        {
            ViewBag.CommercialProductId = new SelectList(db.CommercialProducts, "CommercialProductId", "ProductName");
            GameDetail oGameDetail = null;
            if (id.HasValue)
            {
                Game oGame = db.Games.Find(id);
                if (oGame != null)
                {
                    oGameDetail = new GameDetail { AdvertId = id.Value, Game = oGame };
                }
            }
            return PartialView("Create", oGameDetail);
        }

        //
        // POST: /GameDetail/Create

        [HttpPost]
        public ActionResult Create(GameDetail gamedetail)
        {
            ViewBag.CommercialProductId = new SelectList(db.CommercialProducts.OrderBy(c => c.ProductName), "CommercialProductId", "ProductName", gamedetail.CommercialProductId);
            if (ModelState.IsValid)
            {
                var check = db.GameDetails.Where(a => a.CommercialProductId == gamedetail.CommercialProductId && a.AdvertId == gamedetail.AdvertId).FirstOrDefault();
                if (check == null)
                {
                    gamedetail.CreatedDate = DateTime.Now;
                    db.GameDetails.Add(gamedetail);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {

                        //OTOD
                    }
                }
                else
                {
                    //TODO
                }
                var oGame = db.Games.Find(gamedetail.AdvertId);
                ViewBag.Game = oGame;

                return PartialView("GameDetailPartialList", db.GameDetails.Where(cd => cd.AdvertId == gamedetail.AdvertId).Include(i => i.CommercialProduct));
            }
            return PartialView("Create", gamedetail);
        }

        //
        // GET: /GameDetail/Edit/5

        public ActionResult Edit(int id)
        {
            GameDetail gamedetail = db.GameDetails.Find(id);
            ViewBag.CommercialProductId = new SelectList(db.CommercialProducts, "CommercialProductId", "ProductName", gamedetail.CommercialProductId);
            return PartialView(gamedetail);
        }

        //
        // POST: /GameDetail/Edit/5

        [HttpPost]
        public ActionResult Edit(GameDetail gamedetail)
        {
            ViewBag.CommercialProductId = new SelectList(db.CommercialProducts, "CommercialProductId", "ProductName", gamedetail.CommercialProductId);
            if (ModelState.IsValid)
            {
                var oGame = db.Games.Find(gamedetail.GameDetailId);
                gamedetail.LastUpdate = DateTime.Now;
                db.Entry(gamedetail).State = EntityState.Modified;
                ViewBag.Game = oGame;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    //TODO                        
                }
                return PartialView("GameDetailPartialList", db.GameDetails.Where(cd => cd.AdvertId == gamedetail.AdvertId).Include(i => i.CommercialProduct).ToList());
            }
            return PartialView("Edit", gamedetail);
        }

        //
        // GET: /GameDetail/Delete/5

        public ActionResult Delete(int id)
        {
            GameDetail gamedetail = db.GameDetails.Find(id);
            return PartialView(gamedetail);
        }

        //
        // POST: /GameDetail/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            GameDetail gamedetail = db.GameDetails.Find(id);
            if (gamedetail != null)
            {
                db.GameDetails.Remove(gamedetail);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    //foreign key conflict with GameDetailinteractions
                }
                return PartialView("GameDetailPartialList", db.GameDetails.Where(cd => cd.AdvertId == gamedetail.AdvertId).Include(i => i.CommercialProduct));
            }
            return Content("El registro no fue encontrado.");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}