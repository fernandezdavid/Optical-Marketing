﻿using System;
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
    /**
     * Manejador de vistas de Detalle de Juego
     */

    [Authorize]
    public class GameDetailController : Controller
    {
        private OMKTDB db = new OMKTDB();

        /**
         * Vista de detalle de juego activo
         *
         * @since 04/04/2013
         * @param int GameID
         * @return Vista de detalle de juego
         */

        public PartialViewResult IndexByGame(int id)
        {
            ViewBag.AdvertId = id;
            var oGame = db.Games.Find(id);
            ViewBag.Game = oGame;
            var oGameDetails = db.GameDetails.Include(i => i.Game).Where(i => i.AdvertId == id && i.Status == "OK");
            return PartialView("GameDetailPartialList", oGameDetails.ToList());
        }

        /**
         * Vista de detalle de juego activo
         *
         * @since 04/04/2013
         * @param int GameID
         * @return Vista de detalle de juego
         */

        public PartialViewResult IndexByMemoryGame(int id)
        {
            ViewBag.AdvertId = id;
            var oGame = db.Games.FirstOrDefault(i => i.AdvertId == id);
            ViewBag.Game = oGame;

            var oGameDetails = db.GameDetails.Include(i => i.Game).Where(i => i.AdvertId == id && i.Status == "OK");
            return PartialView("MemoryGamePartialList", oGameDetails.ToList());

        }

        /**
         * Vista sección detalle de juegos
         *
         * @since 04/04/2013
         * @return Vista principal de detalle de juegos
         * @deprecated
         */
        
        public ViewResult Index()
        {
            var gamedetails = db.GameDetails.Include(g => g.Game);
            return View(gamedetails.ToList());
        }

        /**
         * Vista del detalle de un detalle de juego
         *
         * @since 04/04/2013
         * @param int id
         * @return Vista de detalle de un detalle de juego
         */

        public ViewResult Details(int id)
        {
            var gamedetail = db.GameDetails.Where(g=>g.GameDetailId == id && g.Status == "OK").FirstOrDefault();
            if (gamedetail == null) throw new HttpException(404, "The resource cannot be found");
            return View(gamedetail);
        }

         /**
         * Vista de creación de detalle de juego
         *
         * @since 04/04/2013
         * @return Vista de creación de detalle de juego
         */

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

        /**
         * Crea una detalle de juego
         *
         * @since 04/04/2013
         * @param Modelo Detalle de Juego
         * @return Vista de detalle de juego
         */

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
                    gamedetail.Status = "OK";
                    db.GameDetails.Add(gamedetail);
                    try
                    {
                        db.SaveChanges();
                        ViewBag.Success = "El producto fue agregado exitosamente";   
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud";                        
                    }
                }else if(check.Status == "DELETED"){
                    check.LastUpdate = DateTime.Now;
                    check.Status = "OK";
                    check.Discount = gamedetail.Discount;
                    check.QRCode = gamedetail.QRCode;
                    db.Entry(check).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                        ViewBag.Success = "El producto fue restaurado exitosamente";
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud";
                    }  
                }
                else
                {
                    ViewBag.Error = "Parece que este producto ya fue agregado!";   
                }
                var oGame = db.Games.Find(gamedetail.AdvertId);
                ViewBag.Game = oGame;

                return PartialView("GameDetailPartialList", db.GameDetails.Where(cd => cd.AdvertId == gamedetail.AdvertId && cd.Status == "OK").Include(i => i.CommercialProduct));
            }
            return PartialView("Create", gamedetail);
        }

        /**
        * Vista de edición de detalle de juego
        *
        * @since 04/04/2013
        * @param int id
        * @return Vista de edición de detalle de juego
        */

        public ActionResult Edit(int id)
        {
            var gamedetail = db.GameDetails.FirstOrDefault(g=>g.GameDetailId == id && g.Status == "OK");
            if (gamedetail == null) throw new HttpException(404, "The resource cannot be found");
            ViewBag.CommercialProductId = new SelectList(db.CommercialProducts.Where(c=>c.Status == "OK"), "CommercialProductId", "ProductName", gamedetail.CommercialProductId);
            return PartialView(gamedetail);
        }

        /**
        * Edita un detalle de juego
        *
        * @since 04/04/2013
        * @param Modelo Detalle de Juego
        * @return Vista de edición de detalle de juego
        */

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
                    ViewBag.Success = "El producto fue editado exitosamente"; 
                }
                catch (Exception)
                {
                    ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud";                      
                }
                return PartialView("GameDetailPartialList", db.GameDetails.Where(cd => cd.AdvertId == gamedetail.AdvertId && cd.Status =="OK").Include(i => i.CommercialProduct).ToList());
            }
            return PartialView("Edit", gamedetail);
        }

         /**
         * Vista de borrado lógico de detalle de juego
         *
         * @since 04/04/2013
         * @param int id
         * @return Vista de borrado de detalle de juego
         */

        public ActionResult Delete(int id)
        {
            var gamedetail = db.GameDetails.FirstOrDefault(g=>g.GameDetailId == id && g.Status == "OK");
            if (gamedetail == null) throw new HttpException(404, "The resource cannot be found");
            return PartialView(gamedetail);
        }

        /**
        * Borra lógicamente un detalle de juego
        *
        * @since 04/04/2013
        * @param int id
        * @return Mensaje de confirmación
        */

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            GameDetail gamedetail = db.GameDetails.Find(id);
            if (gamedetail != null)
            {
                gamedetail.Status = "DELETED";
                gamedetail.LastUpdate = DateTime.Now;
                db.Entry(gamedetail).State = EntityState.Modified;
                try
                {
                    ViewBag.Success = "El producto fue quitado exitosamente!";
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud.";                    
                }                
            }
            else { ViewBag.Error = "Lo sentimos, no pudimos encontrar el detalle."; }
            return PartialView("GameDetailPartialList", db.GameDetails.Where(cd => cd.AdvertId == gamedetail.AdvertId && cd.Status == "OK").Include(i => i.CommercialProduct));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}