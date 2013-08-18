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

            //var advertCamp = _db.AdvertCampaigns.Include(a => a.AdvertCampaignDetails).Where(a => a.CustomerId == oUser.CustomerId);
            var oSummary = new SummaryBoard();

            //foreach (var camp in advertCamp)
            //{
            //    var inter = 0;
            //    var elapsedTime = 0;
            //    var height = 0m;
            //    foreach (var campDetail in camp.AdvertCampaignDetails)
            //    {
            //        DateTime check_date = DateTime.Today.Subtract(TimeSpan.FromDays(i));
            //        inter += _db.AdvertCampaignDetailInteractions
            //                .Where(c => c.AdvertID == campDetail.AdvertID && c.StartDatetime.Year == check_date.Year && c.StartDatetime.Month == check_date.Month && c.StartDatetime.Day == check_date.Day)
            //                .Count();
            //        elapsedTime += (from c in _db.AdvertCampaignDetailInteractions
            //                        let dt = c.StartDatetime
            //                        where c.StartDatetime.Year == check_date.Year && c.StartDatetime.Month == check_date.Month && c.StartDatetime.Day == check_date.Day
            //                        && c.TimeElapsed.HasValue && c.AdvertID == campDetail.AdvertID
            //                        group c by new { y = dt.Year, m = dt.Month, d = dt.Day } into g
            //                        select new { elapsed = g.Sum(c => c.TimeElapsed) }).First().elapsed ?? 0;
            //        height += (from h in _db.AdvertCampaignDetailInteractions
            //                   let ds = h.StartDatetime
            //                   where h.StartDatetime.Year == check_date.Year && h.StartDatetime.Month == check_date.Month && h.StartDatetime.Day == check_date.Day
            //                 && h.AdvertID == campDetail.AdvertID
            //                   group h by new { y = ds.Year, m = ds.Month, d = ds.Day } into a
            //                   select new { totalHeight = a.Sum(x => x.Height) }).First().totalHeight;
            //    }
            //    var oCP = new CampaignPerformance();
            //    oCP.Month = i;
            //    oCP.CampaignName = camp.Name;
            //    oCP.Impressions = inter;
            //    oCP.TimeAverage = ((inter != 0) ? elapsedTime / inter : 0).ToString();
            //    oCP.HeightAverage = ((height != 0) ? height / inter : 0).ToString().Replace(',', '.');
            //    interactions.Add(oCP);
            //}

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