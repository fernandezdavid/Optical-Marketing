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
    [Authorize]
    public class AdvertCampaignController : Controller
    {
        private readonly OMKTDB _db = new OMKTDB();

        /**
         * method LatestCampaings
         *
         * List a number of campaigns order by created date descending
         *
         * @since 04/04/2013
         * @return advertCampaign collection
         */

        public PartialViewResult LatestCampaigns(int? state, int? period)
        {
            var oUser = (User)Session["User"];
            var campaigns = _db.AdvertCampaigns;

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
         * method DashboardCampaings
         *
         * List a number of campaigns order by created date descending
         *
         * @since 04/04/2013
         * @return advertCampaign collection
         */

        public PartialViewResult DashboardCampaigns(int? top)
        {
            if (!top.HasValue) top = 10;
            var oUser = (User)Session["User"];
            var campaigns = _db.AdvertCampaigns.Where(i => i.CustomerId == oUser.CustomerId).Take(top.Value);
            return PartialView("AdvertCampaignListSlimPartial", campaigns.ToList());
        }

        /**
         * method ActiveCampaigns
         *
         * List a number of active campaigns order by name descending
         *
         * @since 04/04/2013
         * @return advertCampaign collection
         */

        public PartialViewResult ActiveCampaigns(int? top)
        {
            if (!top.HasValue)
                top = 5;
            var oUser = (User)Session["User"];
            var campaigns = _db.AdvertCampaigns.Where(c => c.CustomerId == oUser.CustomerId).OrderByDescending(i => i.Name).Include(a => a.AdvertCampaignDetails).Take(top.Value);
            return (campaigns.Any()) ? PartialView("AdvertCampaignListSlimPartial", campaigns.ToList()) : PartialView("AdvertCampaignListSlimPartial");
        }

        /**
         * method Index
         *
         * Show main page of advertCampaigns
         *
         * @since 04/04/2013
         * @return view
         */

        public ActionResult Index()
        {
            return View();
        }

        /**
         * method Details
         *
         * Show advertCampaign details
         *
         * @since 04/04/2013
         * @return viewmodel
         */

        public ViewResult Details(int id)
        {
            var oUser = (User)Session["User"];
            //@TODO customerId check
            AdvertCampaign advertcampaign = _db.AdvertCampaigns.Find(id);
            return View(advertcampaign);
        }

        /**
         * method Create
         *
         * Show advertCampaign create form
         *
         * @since 04/04/2013
         * @return view form
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
        * method Create (post)
        *
        * Manage advertCampaign create form
        *
        * @since 04/04/2013
        * @return view form
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
        * method Edit
        *
        * Manage advertCampaign edit form
        *
        * @since 04/04/2013
        * @return view form
        */

        public ActionResult Edit(int id)
        {
            AdvertCampaign advertcampaign = _db.AdvertCampaigns.Find(id); //@TODO customer check
            advertcampaign.Estimate = Convert.ToInt32(advertcampaign.Estimate);
            ViewBag.CampaignTypeId = new SelectList(_db.CampaignTypes.OrderBy(c => c.Name), "CampaignTypeId", "Name", advertcampaign.CampaignTypeId);

            return View(advertcampaign);
        }

        /**
        * method Edit (post)
        *
        * Manage advertCampaign edit form
        *
        * @since 04/04/2013
        * @return view form
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
       * method Delete
       *
       * Manage advertCampaign delete form
       *
       * @since 04/04/2013
       * @return view form
       */

        public ActionResult Delete(int id)
        {
            AdvertCampaign advertcampaign = _db.AdvertCampaigns.Find(id); //@TODO customer check
            return PartialView(advertcampaign);
        }

        /**
        * method Delete (post)
        *
        * Manage advertCampaign delete confirm form
        *
        * @since 04/04/2013
        * @return view form
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
        * method AdvertCampaignsPerformance
        *
        * Shows the relation interactions/likes for each advertCampaign
        *
        * @since 14/07/2013
        * @return partial view
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
                        elapsedTime += (from c in _db.AdvertCampaignDetailInteractions
                                     let dt = c.StartDatetime
                                     where c.StartDatetime.Year == check_date.Year && c.StartDatetime.Month == check_date.Month && c.StartDatetime.Day == check_date.Day 
                                     && c.TimeElapsed.HasValue && c.AdvertID == campDetail.AdvertID
                                     group c by new { y = dt.Year, m = dt.Month, d = dt.Day } into g
                                     select new { elapsed = g.Sum(c => c.TimeElapsed) }).First().elapsed ?? 0;
                        height += (from h in _db.AdvertCampaignDetailInteractions
                                   let ds = h.StartDatetime
                                   where h.StartDatetime.Year == check_date.Year && h.StartDatetime.Month == check_date.Month && h.StartDatetime.Day == check_date.Day
                                 && h.AdvertID == campDetail.AdvertID
                                   group h by new { y = ds.Year, m = ds.Month, d = ds.Day } into a
                                   select new { totalHeight = a.Sum(x => x.Height) }).First().totalHeight;                                    
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