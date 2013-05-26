using System;
using System.Numerics;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OMKT.Business;
using Simple.ImageResizer;
using OMKT.Context;

namespace OMKT.Controllers
{
    [Authorize]
    public class CommercialProductController : Controller
    {
        private readonly int _defaultPageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DefaultPaginationSize"]);
        private readonly OMKTDB _db = new OMKTDB();

        public PartialViewResult ActiveProducts(int? top)
        {
            if (!top.HasValue) top = 10;
            var oUser = (User)Session["User"];
            var catalogs = _db.CommercialProducts.Where(c => c.CustomerId == oUser.CustomerId).OrderByDescending(i => i.ProductName).Take(top.Value);
            return PartialView("CommercialProductListPartial", catalogs.ToList());
        }

        public PartialViewResult DashboardProducts(int? top)
        {
            if (!top.HasValue) top = 10;
            var oUser = (User)Session["User"];
            var catalogs = _db.CommercialProducts.Where(c => c.CustomerId == oUser.CustomerId).OrderByDescending(i => i.ProductName).Take(top.Value);
            return PartialView("CommercialProductListSlimPartial", catalogs.ToList());

        }

        // GET: /CommercialProduct/
        public ViewResult Index(int? page)
        {
            return View();
        }

        //
        // GET: /CommercialProduct/Details/5

        public ViewResult Details(int id)
        {
            CommercialProduct commercialproduct = _db.CommercialProducts.Find(id);
            return View(commercialproduct);
        }

        //
        // GET: /CommercialProduct/Create

        public ActionResult Create()
        {
            ViewBag.CommercialProductTypeId = new SelectList(_db.CommercialProductTypes.OrderBy(c => c.Description), "CommercialProductTypeId", "Description");
            return View();
        }

        //
        // POST: /CommercialProduct/Create

        [HttpPost]
        public ActionResult Create(CommercialProduct commercialProduct, HttpPostedFileBase image)
        {
            ViewBag.CommercialProductTypeId = new SelectList(_db.CommercialProductTypes.OrderBy(c => c.Description), "CommercialProductTypeId", "Description");

            if (!ModelState.IsValid)
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
                    var imagePath = "~/Content/productImages/";
                    imageResizer.SaveToFile(Path.Combine(Server.MapPath(imagePath + "thumbnails"), fileName));
                    //var fileName = Path.GetFileName(image.FileName);
                    if (fileName != null)
                    {
                        var path = Path.Combine(Server.MapPath(imagePath), fileName);
                        image.SaveAs(path);
                    }
                    var prodType = _db.CommercialProductTypes.Find(commercialProduct.CommercialProductTypeId);
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
                    var oUser = (User)Session["User"];
                    if (oUser != null)
                    {
                        commercialProduct.CustomerId = oUser.CustomerId;
                        commercialProduct.CommercialProductType = prodType;
                        commercialProduct.ProductImage = img;
                        _db.CommercialProducts.Add(commercialProduct);
                        try
                        {
                            _db.SaveChanges();
                            return RedirectToAction("Edit", new { id = commercialProduct.CommercialProductId, result = "success" });
                        }
                        catch (Exception)
                        {
                            return View(commercialProduct);
                        }
                    }
                    else
                        return RedirectToAction("LogOff", "Account");
                }
            }
            return View(commercialProduct);
        }

        //
        // GET: /CommercialProduct/Edit/5

        public ActionResult Edit(int id, string result)
        {
            ViewBag.Mode = "Editar";
            ViewBag.Result = (!string.IsNullOrEmpty(result)) ? "El producto fue guardado con éxito." : result;
            CommercialProduct commercialproduct = _db.CommercialProducts.Find(id);
            ViewBag.CommercialProductTypeId = new SelectList(_db.CommercialProductTypes, "CommercialProductTypeId", "Description", commercialproduct.CommercialProductTypeId);
            commercialproduct.Price = Convert.ToInt16(commercialproduct.Price);
            return View(commercialproduct);
        }

        //
        // POST: /CommercialProduct/Edit/5

        [HttpPost]
        public ActionResult Edit(CommercialProduct commercialProduct, HttpPostedFileBase image)
        {
            ViewBag.Mode = "Editar";
            ViewBag.CommercialProductTypeId = new SelectList(_db.CommercialProductTypes, "CommercialProductTypeId", "Description", commercialProduct.CommercialProductTypeId);
            var img = _db.ProductImages.Find(commercialProduct.ProductImageId);
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
                    var imagePath = "~/Content/productImages/";
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
                    img.ProductImageId = commercialProduct.ProductImageId;
                    commercialProduct.ProductImage = img;
                    //@todo borrar la imagen anterior
                }
                _db.Entry(commercialProduct).State = EntityState.Modified;
                try
                {
                    _db.SaveChanges();
                    //return Json(new { result = true, message = "El producto fue editado satisfactorimente." });
                    return RedirectToAction("Edit", new { id = commercialProduct.CommercialProductId, result = "success" });
                }
                catch (Exception)
                {
                    //return Json(new { result = false, message = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud." });
                    return View(commercialProduct);
                }
            }
            return View(commercialProduct);
        }

        //
        // GET: /CommercialProduct/Delete/5

        public ActionResult Delete(int id)
        {
            CommercialProduct commercialproduct = _db.CommercialProducts.Find(id);
            return PartialView(commercialproduct);
        }

        //
        // POST: /CommercialProduct/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            CommercialProduct commercialproduct = _db.CommercialProducts.Find(id);
            _db.CommercialProducts.Remove(commercialproduct);
            _db.SaveChanges();
            return Json(new { });
        }

        public ActionResult CatalogsOverview()
        {
            return PartialView("CatalogsOverview");
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}