using System.Web.Mvc;
using OMKT.Business;
using OMKT.Context;
using OMKT.Models;
using System;
using System.Collections.Generic;

namespace OMKT.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/
        private readonly OMKTDB _db = new OMKTDB();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Help()
        {
            return View();
        }
        public PartialViewResult DashboardStats(int? period)
        {
            if (!period.HasValue) period = 10;
            var oUser = (User)Session["User"];
            //var catalogs = _db
            var oSummary = new SummaryBoard();
            oSummary.Impressions = 1923;
            oSummary.LikesPercentage = 49.7M;
            oSummary.TimeAverage = 32.2M;
            oSummary.Bounce = 13.9M;
            return PartialView("SummaryBoard", oSummary);
        }

        public PartialViewResult AdvertCampaignsPerformance(int? period)
        {
            var oUser = (User)Session["User"];
            var detailList = new List<AdvertCampaignDetail>();

            var camp = new List<CampaignPerformance>();
            period = (period.HasValue) ? period : 12;

            for (int j = 0; j <= period; j++)
            {
                camp.Add(new CampaignPerformance()
                {
                    Impressions = new Random(j).Next(2000, 3000),
                    Traffic = new Random(j).Next(10000, 15000),
                    Month = j
                });
            }
            return PartialView("AdvertCampaignsPerformance", camp);
        }
    }
}