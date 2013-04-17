using System.Web.Mvc;
using System;
using OMKT.Business;
using OMKT.Context;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using OMKT.Models;

namespace OMKT.Controllers
{
    [Authorize]
    public class StatsController : Controller
    {
        private readonly OMKTDB _db = new OMKTDB();

        public ActionResult SummaryBoard()
        {
            return PartialView("SummaryBoard");
        }

        //private IEnumerable<CampaignPerformance> GetPerformance(int? advertCampaignId)
        //{
           
           //for (int i = 6; i < 13; i++)
           //{
           //    var rend = new CampaignPerformance();


           //    rend.Traffic = _db.Interactions.Include( x =>
           //        x.AdvertCampaignDetail.AdvertCampaign.CustomerId == oUser.CustomerId &&
           //        x.StartDateTime.Day == i).Sum(a => a.Traffic);
           //    rend.Month = i;
           //    rend.Impressions = _db.Interactions.Where(it => it.AdvertCampaignDetail.AdvertCampaign.CustomerId == oUser.CustomerId && it.StartDateTime.Day == i).
           //            Sum(s => s.Impressions);
           //    perfList.Add(rend);
           //}           
           //return camp;
        //}

        //private Summary GetSummary()
        //{
        //    return new Summary();
        //}

        //private List<ProductPerformance> GetProductsPerformance()
        //{
        //    var oUser = (User)Session["User"];
        //    var performanceList = new List<ProductPerformance>();
        //    var products = _db.CommercialProducts.Where(x => x.CustomerId == oUser.CustomerId);
        //    foreach (var pro in products)
        //    {
        //        var likes = _db.CatalogDetails.Where(
        //            p => p.CommercialProductId == pro.CommercialProductId);
        //        var views = _db.CatalogDetails.Where(
        //            p => p.CommercialProductId == pro.CommercialProductId);
        //        var prodPerformance = new ProductPerformance
        //                       {
        //                           Likes = (likes.Any()) ? likes.Sum(s=>s.Likes) : 0,
        //                           Views = (views.Any()) ? views.Sum(s => s.Views) : 0,
        //                           Name = pro.ProductName
        //                       };
        //        performanceList.Add(prodPerformance);
        //    }
        //    return performanceList.ToList();

        //}

        ////
        //// GET: /Reports/
        //[OutputCache(Duration = 30)]
        //public ActionResult AdvertCampaignsPerformance()
        //{
        //    int quarter = 0;
        //    int year = 0;
        //    DateTime fromDate = DateTime.Now;
        //    DateTime end;

        //    return PeriodSummary();
        //}

        //public ActionResult CommercialProductsViews()
        //{
        //    return PartialView("CommercialProductsViews", GetProductsPerformance());
        //}
        //public ActionResult CommercialProductsLikes()
        //{
        //    return PartialView("CommercialProductsLikes", GetProductsPerformance());
        //}
        //public ActionResult AdvertsPerformance()
        //{
        //    return PartialView("AdvertsPerformance", GetProductsPerformance());
        //}

        public ActionResult AdvertCampaignsPerformance(int? period)
        {
            var oUser = (User)Session["User"];
            var detailList = new List<AdvertCampaignDetail>();

            var camp = new List<CampaignPerformance>();
            if (period.HasValue && period != 0)
            {
                detailList = _db.AdvertCampaignDetails.Where(a => a.AdvertCampaignId == period).ToList();

            }
            else
            {

                detailList = _db.AdvertCampaignDetails.Where(a => a.AdvertCampaign.CustomerId == oUser.CustomerId).ToList();
            }
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