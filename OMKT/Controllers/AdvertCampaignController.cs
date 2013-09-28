using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using OMKT.Business;
using OMKT.Context;
using OMKT.Models;
using System.Data.Objects;

namespace OMKT.Controllers
{
    /**
     * Manejador de las vistas de la Campaña
     */

    [Authorize]
    public class AdvertCampaignController : Controller
    {
        private readonly OMKTDB _db = new OMKTDB();

        /**
         * Lista las últimas campañas activas ordernadas por fecha
         *
         * @since 04/04/2013
         * @param int state
         * @param int period
         * @return Vista parcial de listado de campañas
         */

        public PartialViewResult LatestCampaigns(int? state, int? period)
        {
            var oUser = (User)Session["User"];
            var campaigns = _db.AdvertCampaigns.Include(i => i.AdvertCampaignDetails);

            campaigns.Where(i => i.CustomerId == oUser.CustomerId);

            if (state.HasValue && state != 0)
            {
                campaigns.Where(i => i.CampaignStateId == state);
            }
            if (period.HasValue && period != 0)
            {
                campaigns.Where(i => i.StartDatetime >= Convert.ToDateTime(i.StartDatetime).AddDays(-(double)period));
            }

            campaigns.OrderByDescending(i => i.CreatedDate);
            return PartialView("AdvertCampaignListPartial", campaigns.ToList());
        }

        /**
         * Lista las campañas activas ordenadas por fecha
         *
         * @since 04/04/2013
         * @param int top
         * @return Vista parcial de listado de campañas
         */

        public PartialViewResult DashboardCampaigns(int? top)
        {
            if (!top.HasValue) top = 10;
            var oUser = (User)Session["User"];
            var campaigns = _db.AdvertCampaigns.Include(i => i.AdvertCampaignDetails).Where(i => i.CustomerId == oUser.CustomerId).Take(top.Value);
            return PartialView("AdvertCampaignListSlimPartial", campaigns.ToList());
        }

        /**
         * Lista campañas activas ordenadas por nombre
         *
         * @since 04/04/2013
         * @param int top
         * @return Vista parcial de listado de campañas
         */

        public PartialViewResult ActiveCampaigns(int? top)
        {
            if (!top.HasValue)
                top = 5;
            var oUser = (User)Session["User"];
            var campaigns = _db.AdvertCampaigns.Include(i => i.AdvertCampaignDetails).Where(c => c.CustomerId == oUser.CustomerId).OrderByDescending(i => i.Name).Include(a => a.AdvertCampaignDetails).Take(top.Value);
            return (campaigns.Any()) ? PartialView("AdvertCampaignListSlimPartial", campaigns.ToList()) : PartialView("AdvertCampaignListSlimPartial");
        }

        /**
         * Muestra el índice de la sección Campañas
         *
         * @since 04/04/2013
         * @return Vista principal de campañas
         */

        public ActionResult Index()
        {
            return View();
        }

        /**
         * Muestra el detalle de campaña
         *
         * @since 04/04/2013
         * @param int id
         * @return Vista de detalle de campaña
         */

        public ViewResult Details(int id)
        {
            var oUser = (User)Session["User"];
            //@TODO customerId check
            AdvertCampaign advertcampaign = _db.AdvertCampaigns.Find(id);
            return View(advertcampaign);
        }

        /**
         * Vista de creación de campaña
         *
         * @since 04/04/2013
         * @return Vista de creación de una campaña
         */

        public ActionResult Create()
        {
            ViewBag.CampaignTypeId = new SelectList(_db.CampaignTypes.OrderBy(c => c.Name), "CampaignTypeId", "Name", 1);
            var oCampaign = new AdvertCampaign
            {
                StartDatetime = DateTime.Now,
                EndDatetime = DateTime.Now.AddDays(30),
                NetworkId = 1 //@TODO do better
            };
            return View(oCampaign);
        }

        /**
        * Crea una campaña
        *
        * @since 04/04/2013
        * @param Modelo Campaña
        * @return Vista de edición de campaña
        */

        [HttpPost]
        public ActionResult Create(AdvertCampaign advertcampaign)
        {
            ViewBag.CampaignTypeId = new SelectList(_db.CampaignTypes.OrderBy(c => c.Name), "CampaignTypeId", "Name");
            if (ModelState.IsValid)
            {
                advertcampaign.CreatedDate = DateTime.Now;
                advertcampaign.StartDatetime = DateTime.Now;
                advertcampaign.CampaignState = _db.CampaignStates.Find(1); //@TODO do better
                advertcampaign.LastUpdate = DateTime.Now; //sacar esta bazofia
                var oUser = (User)Session["User"];
                if (oUser != null)
                {
                    advertcampaign.CustomerId = oUser.CustomerId;
                    _db.AdvertCampaigns.Add(advertcampaign);
                    try
                    {
                        _db.SaveChanges();
                        ViewBag.Success = "La campaña fue registrada satisfactoriamente.";
                        return RedirectToAction("Edit", "AdvertCampaign", new { id = advertcampaign.AdvertCampaignId });
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud.";
                        return View(advertcampaign);
                    }
                }
            }
            return View(advertcampaign);
        }

        /**
        * Vista de edición de campaña
        *
        * @since 04/04/2013
        * @param int id
        * @return Vista de edición de campaña
        */

        public ActionResult Edit(int id)
        {
            AdvertCampaign advertcampaign = _db.AdvertCampaigns.Find(id); //@TODO customer check
            advertcampaign.Estimate = Convert.ToInt32(advertcampaign.Estimate);
            ViewBag.CampaignTypeId = new SelectList(_db.CampaignTypes.OrderBy(c => c.Name), "CampaignTypeId", "Name", advertcampaign.CampaignTypeId);

            return View(advertcampaign);
        }

        /**
        * Edita una campaña
        *
        * @since 04/04/2013
        * @param Modelo Campaña
        * @return Vista de edición de campaña
        */

        [HttpPost]
        public ActionResult Edit(AdvertCampaign advertcampaign)
        {
            ViewBag.CampaignTypeId = new SelectList(_db.CampaignTypes.OrderBy(c => c.Name), "CampaignTypeId", "Name");
            if (ModelState.IsValid)
            {
                advertcampaign.LastUpdate = DateTime.Now;
                _db.Entry(advertcampaign).State = EntityState.Modified;
                try
                {
                    _db.SaveChanges();
                    ViewBag.Success = "La campaña fue editada satisfactoriamente.";
                    return View(advertcampaign);
                }
                catch (Exception)
                {
                    ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud.";
                    return View(advertcampaign);
                }
            }
            return View(advertcampaign);
        }

        /**
         * Vista de borrado lógico de campaña
         *
         * @since 04/04/2013
         * @param int id
         * @return Vista de borrado de campaña
         */

        public ActionResult Delete(int id)
        {
            AdvertCampaign advertcampaign = _db.AdvertCampaigns.Find(id);
            return PartialView(advertcampaign);
        }

        /**
        * Borra lógicamente una campaña
        *
        * @since 04/04/2013
        * @param int id
        * @return Mensaje de confirmación
        */

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            AdvertCampaign advertcampaign = _db.AdvertCampaigns.Find(id);
            _db.AdvertCampaigns.Remove(advertcampaign);
            try
            {
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return PartialView(advertcampaign);
            }
        }

        /**
        * Muestra el rendimiento de una campaña a través de la relación
        * interacciones-valoraciones
        * 
        * @since 14/07/2013
        * @param int period 
        * @return Vista de rendimiento de una campaña
        */

        public ActionResult AdvertCampaignsPerformance(int? period)
        {
            int interval = (period.HasValue) ? Convert.ToInt32(period) : 15;
            var oUser = (User)Session["User"];

            var advertCamp = _db.AdvertCampaigns.Include(c => c.AdvertCampaignDetails).Where(c => c.CustomerId == oUser.CustomerId);
            var interactions = new List<CampaignPerformance>();
            for (int i = 0; i < interval; i++)
            {
                foreach (var camp in advertCamp)
                {
                    var inter = 0;
                    var elapsedTime = 0;
                    var height = 0m;
                    foreach (var campDetail in camp.AdvertCampaignDetails)
                    {
                        DateTime check_date = DateTime.Today.Subtract(TimeSpan.FromDays(i));
                        inter += _db.AdvertCampaignDetailInteractions
                                .Where(c => c.AdvertID == campDetail.AdvertID && c.StartDatetime.Year == check_date.Year && c.StartDatetime.Month == check_date.Month && c.StartDatetime.Day == check_date.Day)
                                .Count();
                        var cresult = (from c in _db.AdvertCampaignDetailInteractions
                                       let dt = c.StartDatetime
                                       where c.StartDatetime.Year == check_date.Year && c.StartDatetime.Month == check_date.Month && c.StartDatetime.Day == check_date.Day
                                       && c.TimeElapsed.HasValue && c.AdvertID == campDetail.AdvertID
                                       group c by new { y = dt.Year, m = dt.Month, d = dt.Day } into g
                                       select new { elapsed = g.Sum(c => c.TimeElapsed) }).FirstOrDefault();
                        if (cresult != null)
                        {
                            elapsedTime += cresult.elapsed ?? 0;
                        }

                        var hresult = (from h in _db.AdvertCampaignDetailInteractions
                                       let ds = h.StartDatetime
                                       where h.StartDatetime.Year == check_date.Year && h.StartDatetime.Month == check_date.Month && h.StartDatetime.Day == check_date.Day
                                     && h.AdvertID == campDetail.AdvertID
                                       group h by new { y = ds.Year, m = ds.Month, d = ds.Day } into a
                                       select new { totalHeight = a.Sum(x => x.Height) }).FirstOrDefault();
                        if (hresult != null)
                        {
                            height += hresult.totalHeight;
                        }
                    }
                    var oCP = new CampaignPerformance();
                    oCP.Month = i;
                    oCP.CampaignName = camp.Name;
                    oCP.Impressions = inter;
                    oCP.TimeAverage = ((inter != 0) ? elapsedTime / inter : 0).ToString();
                    oCP.HeightAverage = ((height != 0) ? height / inter : 0).ToString().Replace(',', '.');
                    interactions.Add(oCP);
                }

            }

            return PartialView("AdvertCampaignsPerformance", interactions.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}