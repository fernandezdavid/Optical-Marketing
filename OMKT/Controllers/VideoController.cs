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
using System.IO;
using Simple.ImageResizer;
using System.Globalization;

namespace OMKT.Controllers
{ 
    public class VideoController : Controller
    {
        private OMKTDB db = new OMKTDB();

        public PartialViewResult LatestVideos(int? top)
        {
            if (!top.HasValue)
                top = 10;
            var oUser = (User)Session["User"];
            var videos = db.Videos.OrderByDescending(i => i.Name).Take(top.Value);
            return PartialView("VideoListPartial", videos.ToList());
        }

        //
        // GET: /Video/

        public ViewResult Index()
        {
           return View();
        }

        //
        // GET: /Video/Details/5

        public ViewResult Details(int id)
        {
            Video video = db.Videos.Find(id);
            return View(video);
        }

        //
        // GET: /Video/Create

        public ActionResult Create()
        {
            ViewBag.AdvertStateId = new SelectList(db.AdvertStates, "AdvertstateId", "Description", 1);
            var video = new Video {AdvertStateId = 1, StartDatetime = DateTime.Now, EndDatetime = DateTime.Now.AddDays(30) };
            return View(video);
        } 

        //
        // POST: /Video/Create

        [HttpPost]
        public ActionResult Create(Video video, HttpPostedFileBase image, HttpPostedFileBase fvideo)
        {
            if (ModelState.IsValid)
            {
                if ((fvideo != null && fvideo.ContentLength > 0) && (image != null && image.ContentLength > 0))
                {
                    // Thumbnail logic
                    BinaryReader b = new BinaryReader(image.InputStream);
                    var binaryImage = b.ReadBytes(image.ContentLength);

                    var imageResizer = new ImageResizer(binaryImage);
                    // 50 width, 50 height, scaleToFill, encoding
                    imageResizer.Resize(100, 100, true, ImageEncoding.Png);

                    var fileName = image.FileName;
                    var imagePath = "~/Content/videoImages/";
                    imageResizer.SaveToFile(Path.Combine(Server.MapPath(imagePath + "thumbnails"), fileName));
                    
                    if (fileName != null)
                    {
                        var path = Path.Combine(Server.MapPath(imagePath), fileName);
                        image.SaveAs(path);
                    }
                    var img = new ProductImage
                    {
                        Path = Url.Content(imagePath + fileName),
                        Caption = image.FileName.Substring(0, image.FileName.Length - 4),
                        Title = image.FileName.Substring(0, image.FileName.Length - 4),
                        Extension = image.FileName.Substring(image.FileName.Length - 4, 4),
                        CreatedDate = DateTime.Now,
                        Size = image.ContentLength.ToString(CultureInfo.InvariantCulture),
                        ThumbnailPath = Url.Content(imagePath + "thumbnails/" + fileName)
                    };
                    // video save logic
                    var videoPath = "~/Content/videos/";
                    var videoName = fvideo.FileName;
                    if (videoName != null)
                    {
                        var vpath = Path.Combine(Server.MapPath(videoPath), videoName);
                        video.Path = Url.Content(videoPath + videoName);
                        video.Extension = fvideo.FileName.Substring(fvideo.FileName.Length - 4, 4);
                        video.Size = fvideo.ContentLength.ToString(CultureInfo.InvariantCulture);
                        fvideo.SaveAs(vpath);
                    }
                    //

                    var oUser = (User)Session["User"];
                    if (oUser != null)
                    {
                        video.AdvertType = db.AdvertTypes.Find(1);
                        video.CreatedDate = DateTime.Now;
                        video.ProductImage = img;
                        video.StartDatetime = DateTime.Now;
                        db.Videos.Add(video);
                        try
                        {
                            db.SaveChanges();
                            return RedirectToAction("Edit", new { id = video.AdvertId});
                        }
                        catch (Exception)
                        {
                            ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud.";
                            return View(video);
                        }
                    }                    
                }                  
            }
            ViewBag.AdvertStateId = new SelectList(db.AdvertStates, "AdvertstateId", "Description", video.AdvertStateId);
            return View(video);
        }
        
        //
        // GET: /Video/Edit/5
 
        public ActionResult Edit(int id = 0)
        {
            Video video = db.Videos.Find(id);
            if (video == null) return HttpNotFound();
            ViewBag.AdvertStateId = new SelectList(db.AdvertStates, "AdvertstateId", "Description", video.AdvertStateId);
            return View(video);
        }

        //
        // POST: /Video/Edit/5

        [HttpPost]
        public ActionResult Edit(Video video, HttpPostedFileBase image, HttpPostedFileBase fvideo)
        {
            ViewBag.AdvertStateId = new SelectList(db.AdvertStates, "AdvertstateId", "Description", video.AdvertStateId);
            var img = db.ProductImages.Find(video.ProductImageId);
            if (img == null) img = new ProductImage();

            if (ModelState.IsValid)
            {
                if (image != null && image.ContentLength > 0)
                {
                    // Thumbnail logic
                    BinaryReader b = new BinaryReader(image.InputStream);
                    var binaryImage = b.ReadBytes(image.ContentLength);

                    var imageResizer = new ImageResizer(binaryImage);
                    // 50 width, 50 height, scaleToFill, encoding
                    imageResizer.Resize(100, 100, true, ImageEncoding.Png);

                    var fileName = image.FileName;
                    var imagePath = "~/Content/videoImages/";
                    imageResizer.SaveToFile(Path.Combine(Server.MapPath(imagePath + "thumbnails"), fileName));
                    //var fileName = Path.GetFileName(image.FileName);
                    if (fileName != null)
                    {
                        var path = Path.Combine(Server.MapPath(imagePath), fileName);
                        image.SaveAs(path);
                    }
                    img.Path = Url.Content(imagePath + fileName);
                    img.ThumbnailPath = Url.Content(imagePath + "thumbnails/" + fileName);
                    img.Caption = image.FileName.Substring(0, image.FileName.Length - 4);
                    img.Title = image.FileName.Substring(0, image.FileName.Length - 4);
                    img.Extension = image.FileName.Substring(image.FileName.Length - 4, 4);
                    img.CreatedDate = DateTime.Now;
                    img.Size = image.ContentLength.ToString(CultureInfo.InvariantCulture);
                    img.ProductImageId = video.ProductImageId;
                    if(img.ProductImageId == 0) db.ProductImages.Add(img);
                    video.ProductImage = img;
                    //@todo borrar la imagen anterior
                }
                if (fvideo != null && fvideo.ContentLength > 0)
                {
                    // video save logic
                    var videoPath = "~/Content/videos/";
                    var videoName = fvideo.FileName;
                    if (videoName != null)
                    {
                        var vpath = Path.Combine(Server.MapPath(videoPath), videoName);
                        video.Path = Url.Content(videoPath + videoName);
                        video.Extension = fvideo.FileName.Substring(fvideo.FileName.Length - 4, 4);
                        video.Size = fvideo.ContentLength.ToString(CultureInfo.InvariantCulture);
                        fvideo.SaveAs(vpath);
                    }
                    //@todo borrar el video anterior
                }
                db.Entry(video).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    ViewBag.Success = "El video fue editado satisfactoriamente.";
                }
                catch (Exception)
                {
                    ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud.";
                }                
            }
            return View(video);
        }

        //
        // GET: /Video/Delete/5
 
        public ActionResult Delete(int id)
        {
            Video video = db.Videos.Find(id);
            return PartialView(video);
        }

        //
        // POST: /Video/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Video video = db.Videos.Find(id);
            db.Adverts.Remove(video);
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                
                //@TODO
            }
            return Json(new { });
        }

        public ActionResult VideosOverview()
        {
            var oUser = (User)Session["User"];
            var advertDetails = db.AdvertCampaignDetails.Where(c => c.AdvertCampaign.CustomerId == oUser.CustomerId);
            var interactions = new List<AdvertOverview>();
            var views = 0;
            foreach (var cat in advertDetails)
            {
                views = db.AdvertCampaignDetailInteractions.Where(c => c.AdvertID == cat.AdvertID && c.Advert.AdvertTypeId == 3).Count();
                var oCO = new AdvertOverview();
                oCO.Views = views;
                oCO.AdvertName = cat.Advert.Name;
                interactions.Add(oCO);
            }
            return PartialView("VideosOverview", interactions.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}