using System.Web.Mvc;
using OMKT.Business;
using OMKT.Context;
using OMKT.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
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
        public ActionResult DashboardStats(int? period)
        {
            int interval = (period.HasValue) ? Convert.ToInt32(period) : 15;
            var oUser = (User)Session["User"];

            var advertCamp = _db.AdvertCampaigns.Include(c => c.AdvertCampaignDetails).Where(c => c.CustomerId == oUser.CustomerId);
            var interactions = new List<CampaignPerformance>();
            var inter = 0;
            var likes = 0m;
            var wins = 0m;
            var bounce = 0m;
            var elapsedTime = 0;
            var height = 0m;
            for (int i = 0; i < interval; i++)
            {
                foreach (var camp in advertCamp)
                {
                    foreach (var campDetail in camp.AdvertCampaignDetails)
                    {
                        DateTime check_date = DateTime.Today.Subtract(TimeSpan.FromDays(i));
                        inter += _db.AdvertCampaignDetailInteractions
                                .Where(c => c.AdvertID == campDetail.AdvertID && c.StartDatetime.Year == check_date.Year && c.StartDatetime.Month == check_date.Month && c.StartDatetime.Day == check_date.Day)
                                .Count();
                        
                        if (campDetail.Advert.AdvertTypeId == 2)
                        {
                            likes += _db.CatalogDetailInteractions
                                    .Where(c => c.CatalogDetail.AdvertId == campDetail.AdvertID && c.Like == true && c.StartDatetime.Year == check_date.Year && c.StartDatetime.Month == check_date.Month && c.StartDatetime.Day == check_date.Day)
                                    .Count();
                            var views = _db.CatalogDetailInteractions
                                    .Where(c => c.CatalogDetail.AdvertId == campDetail.AdvertID && c.StartDatetime.Year == check_date.Year && c.StartDatetime.Month == check_date.Month && c.StartDatetime.Day == check_date.Day)
                                    .Count();
                            var countDetails = _db.CatalogDetails.Where(ca => ca.AdvertId == campDetail.AdvertID).Count();
                            bounce += views / countDetails;

                        }
                        if (campDetail.Advert.AdvertTypeId == 1)
                        {
                            wins += _db.GameDetailInteractions
                                    .Where(c => c.GameDetail.AdvertId == campDetail.AdvertID && c.Win == true && c.StartDatetime.Year == check_date.Year && c.StartDatetime.Month == check_date.Month && c.StartDatetime.Day == check_date.Day)
                                    .Count();
                            //var views = _db.GameDetailInteractions
                            //        .Where(c => c.GameDetail.AdvertId == campDetail.AdvertID && c.StartDatetime.Year == check_date.Year && c.StartDatetime.Month == check_date.Month && c.StartDatetime.Day == check_date.Day)
                            //        .Count();
                            //var countDetails = _db.CatalogDetails.Where(ca => ca.AdvertId == campDetail.AdvertID).Count();
                            //bounce += views / countDetails;

                        }

                        
                        var result = (from c in _db.AdvertCampaignDetailInteractions
                                        let dt = c.StartDatetime
                                        where c.StartDatetime.Year == check_date.Year && c.StartDatetime.Month == check_date.Month && c.StartDatetime.Day == check_date.Day
                                        && c.TimeElapsed.HasValue && c.AdvertID == campDetail.AdvertID
                                        group c by new { y = dt.Year, m = dt.Month, d = dt.Day } into g
                                        select new { elapsed = g.Sum(c => c.TimeElapsed) }).FirstOrDefault();
                        if (result != null)
                        {
                            elapsedTime += result.elapsed ?? 0;
                        }
                            
                    }

                }

            }

            var oSummary = new SummaryBoard();
            oSummary.Impressions = inter;
            oSummary.LikesPercentage = decimal.Truncate(((likes / inter) * 100)).ToString();
            oSummary.TimeAverage = ((inter != 0) ? elapsedTime / inter : 0).ToString();
            oSummary.Bounce = wins.ToString();
            return PartialView("SummaryBoard", oSummary);
        }


    }
}