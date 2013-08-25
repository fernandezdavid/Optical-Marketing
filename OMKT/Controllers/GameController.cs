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
    [Authorize]
    public class GameController : Controller
    {
        private readonly OMKTDB db = new OMKTDB();

        //
        // GET: /Game/

        public ViewResult Index()
        {
            return View();
        }

        public PartialViewResult LatestGames(int? top)
        {
            if (!top.HasValue)
                top = 10;
            var oUser = (User)Session["User"];
            var games = db.Games.OrderByDescending(i => i.Name).Take(top.Value);
            return PartialView("GameListPartial", games.ToList());
        }

        public PartialViewResult DashboardGames(int? top)
        {
            if (!top.HasValue)
                top = 10;
            var oUser = (User)Session["User"];
            var games = db.Games.OrderByDescending(i => i.Name).Take(top.Value);
            return PartialView("GameListSlimPartial", games.ToList());
        }

        //
        // GET: /Game/Details/5

        public ViewResult Details(int id)
        {
            Game game = db.Games.Find(id);
            return View(game);
        }

        //
        // GET: /Game/Create

        public ActionResult Create()
        {
            ViewBag.AdvertStateId = new SelectList(db.AdvertStates.OrderBy(x => x.Description), "AdvertStateId", "Description");
            var oGame = new Game { StartDatetime = DateTime.Now, EndDatetime = DateTime.Now.AddDays(30), Oportunities = 3 };
            return View(oGame);
        } 

        //
        // POST: /Game/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Game game)
        {
            ViewBag.AdvertStateId = new SelectList(db.AdvertStates.OrderBy(x => x.Description), "AdvertStateId", "Description", game.AdvertStateId);
           if (ModelState.IsValid)
            {
                game.AdvertType = db.AdvertTypes.Find(1);
                game.CreatedDate = DateTime.Now;
                game.StartDatetime = DateTime.Now;
                db.Games.Add(game);
                try
                {
                    db.SaveChanges();
                    ViewBag.Success = "El juego fue registrado satisfactoriamente.";
                    return RedirectToAction("Edit", "Game", new { id = game.AdvertId });
                }
                catch (Exception)
                {
                    ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud.";
                    return View(game);
                }
            }         
            return View(game);
        }
        
        //
        // GET: /Game/Edit/5
 
        public ActionResult Edit(int id)
        {
            Game game = db.Games.Find(id);
            ViewBag.AdvertStateId = new SelectList(db.AdvertStates, "AdvertstateId", "Description", game.AdvertStateId);            
            return View(game);
        }

        //
        // POST: /Game/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Game game)
        {
            ViewBag.AdvertStateId = new SelectList(db.AdvertStates, "AdvertstateId", "Description", game.AdvertStateId);
            var user = (User)Session["User"];
            if (ModelState.IsValid)
            {
                game.LastUpdate = DateTime.Now;
                db.Entry(game).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    ViewBag.Success = "El juego fue editado satisfactoriamente.";
                    return View(game);
                }
                catch (Exception)
                {
                    ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud.";
                    return View(game);
                }
            }            
            return View(game);
        }

        //
        // GET: /Game/Delete/5
 
        public ActionResult Delete(int id)
        {
            Game game = db.Games.Find(id);
            return PartialView(game);
        }

        public ActionResult GamesOverview()
        {
            var oUser = (User)Session["User"];
            var advertDetails = db.AdvertCampaignDetails.Where(c => c.AdvertCampaign.CustomerId == oUser.CustomerId);
            var interactions = new List<AdvertOverview>();
            var views = 0;
            foreach (var cat in advertDetails)
            {
                views = db.AdvertCampaignDetailInteractions.Where(c => c.AdvertID == cat.AdvertID && c.Advert.AdvertTypeId == 1).Count();
                var oCO = new AdvertOverview();
                oCO.Views = views;
                oCO.AdvertName = cat.Advert.Name;
                interactions.Add(oCO);
            }
            return PartialView("GamesOverview", interactions.ToList());
        }

        //
        // POST: /Game/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Game game = db.Games.Find(id);
            db.Games.Remove(game);
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