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
    /**
     * Manejador de vistas de Juegos
     */

    [Authorize]
    public class GameController : Controller
    {
        private readonly OMKTDB db = new OMKTDB();

        /**
         * Vista del índice de la sección Juegos
         *
         * @since 04/04/2013
         * @return Vista principal de juegos
         */

        public ViewResult Index()
        {
            return View();
        }

        /**
         * Vista de últimos juegos ordernadas por nombre
         *
         * @since 04/04/2013
         * @param int top
         * @return Vista parcial de listado de catálogos
         */

        public PartialViewResult LatestGames(int? top)
        {
            if (!top.HasValue)
                top = 10;
            var oUser = (User)Session["User"];
            var games = db.Games.OrderByDescending(i => i.Name).Take(top.Value);
            return PartialView("GameListPartial", games.ToList());
        }

        /**
         * Vista de últimos juegos ordernadas por nombre
         *
         * @since 04/04/2013
         * @param int top
         * @return Vista parcial de listado de juegos
         */

        public PartialViewResult DashboardGames(int? top)
        {
            if (!top.HasValue)
                top = 10;
            var oUser = (User)Session["User"];
            var games = db.Games.OrderByDescending(i => i.Name).Take(top.Value);
            return PartialView("GameListSlimPartial", games.ToList());
        }

        /**
         * Vista del detalle de juego
         *
         * @since 04/04/2013
         * @param int id
         * @return Vista de detalle de juego
         */

        public ViewResult Details(int id)
        {
            Game game = db.Games.Find(id);
            return View(game);
        }

        /**
         * Vista de creación de juegos
         *
         * @since 04/04/2013
         * @return Vista de creación de juegos
         */

        public ActionResult Create()
        {
            ViewBag.AdvertStateId = new SelectList(db.AdvertStates.OrderBy(x => x.Description), "AdvertStateId", "Description");
            var oGame = new Game { StartDatetime = DateTime.Now, EndDatetime = DateTime.Now.AddDays(30), Oportunities = 3 };
            return View(oGame);
        } 

        /**
        * Crea una juego
        *
        * @since 04/04/2013
        * @param Modelo Juego
        * @return Vista de edición de Juego
        */

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
        
        /**
        * Vista de edición de juego
        *
        * @since 04/04/2013
        * @param int id
        * @return Vista de edición de juego
        */
 
        public ActionResult Edit(int id)
        {
            Game game = db.Games.Find(id);
            ViewBag.AdvertStateId = new SelectList(db.AdvertStates, "AdvertstateId", "Description", game.AdvertStateId);            
            return View(game);
        }

        /**
        * Edita un juego
        *
        * @since 04/04/2013
        * @param Modelo Juego
        * @return Vista de edición de juego
        */

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

        /**
         * Vista de borrado lógico de juego
         *
         * @since 04/04/2013
         * @param int id
         * @return Vista de borrado de juego
         */
 
        public ActionResult Delete(int id)
        {
            Game game = db.Games.Find(id);
            return PartialView(game);
        }

        /**
        * Vista del rendimiento de juegos a través de interacciones
        * 
        * @since 14/07/2013 
        * @return Vista de rendimiento de catálogos
        */

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

        /**
        * Borra lógicamente una juego
        *
        * @since 04/04/2013
        * @param int id
        * @return Mensaje de confirmación
        */

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