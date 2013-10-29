using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OMKT.Business;
using OMKT.Context;
using OMKT.Models;
using Simple.ImageResizer;

namespace OMKT.Controllers
{
    /**
     * Manejador de vistas de Productos
     */

    [Authorize]
    public class CommercialProductController : Controller
    {
        private readonly int _defaultPageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DefaultPaginationSize"]);
        private readonly OMKTDB _db = new OMKTDB();

        /**
         * Vista de últimos productos activos ordernadas por nombre
         *
         * @since 04/04/2013
         * @param int top
         * @return Vista parcial de listado de productos
         */

        public PartialViewResult ActiveProducts(int? top)
        {
            if (!top.HasValue) top = 10;
            var oUser = (User)Session["User"];
            var catalogs = _db.CommercialProducts.Where(c => c.CustomerId == oUser.CustomerId && c.Status == "OK").OrderByDescending(i => i.ProductName).Take(top.Value);
            return PartialView("CommercialProductListPartial", catalogs.ToList());
        }

        /**
         * Vista de últimos productos activos ordernadas por nombre
         *
         * @since 04/04/2013
         * @param int top
         * @return Vista parcial de listado de productos
         */

        public PartialViewResult DashboardProducts(int? top)
        {
            if (!top.HasValue) top = 10;
            var oUser = (User)Session["User"];
            var catalogs = _db.CommercialProducts.Where(c => c.CustomerId == oUser.CustomerId && c.Status == "OK").OrderByDescending(i => i.ProductName).Take(top.Value);
            return PartialView("CommercialProductListSlimPartial", catalogs.ToList());
        }

        /**
         * Vista del índice de la sección Productos
         *
         * @since 04/04/2013
         * @return Vista principal de productos
         */

        public ViewResult Index()
        {
            return View();
        }

         /**
         * Vista del detalle de producto
         *
         * @since 04/04/2013
         * @param int id
         * @return Vista de detalle de producto
         */

        public ViewResult Details(int id)
        {
            var commercialproduct = _db.CommercialProducts.FirstOrDefault(c => c.CommercialProductId == id && c.Status == "OK");
            if (commercialproduct == null) throw new HttpException(404, "The resource cannot be found");
            return View(commercialproduct);
        }

         /**
         * Vista de creación de producto
         *
         * @since 04/04/2013
         * @return Vista de creación de producto
         */

        public ActionResult Create()
        {
            ViewBag.CommercialProductTypeId = new SelectList(_db.CommercialProductTypes.OrderBy(c => c.Description), "CommercialProductTypeId", "Description");
            return View();
        }

        /**
        * Crea una producto
        *
        * @since 04/04/2013
        * @param Modelo Producto
        * @return Vista de edición de producto
        */

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
                        commercialProduct.Status = "OK";
                        _db.CommercialProducts.Add(commercialProduct);
                        try
                        {
                            _db.SaveChanges();
                            ViewBag.Success = "El product fue agregado exitosamente!";
                            return RedirectToAction("Edit", new { id = commercialProduct.CommercialProductId });
                        }
                        catch (Exception)
                        {
                            ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud.";
                            return View(commercialProduct);
                        }
                    }
                    else return RedirectToAction("LogOff", "Account");
                }
            }
            return View(commercialProduct);
        }

        /**
        * Vista de edición de producto
        *
        * @since 04/04/2013
        * @param int id
        * @return Vista de edición de producto
        */

        public ActionResult Edit(int id)
        {
            var commercialproduct = _db.CommercialProducts.FirstOrDefault(c => c.CommercialProductId == id && c.Status == "OK");
            if (commercialproduct == null) throw new HttpException(404, "The resource cannot be found");
            ViewBag.CommercialProductTypeId = new SelectList(_db.CommercialProductTypes, "CommercialProductTypeId", "Description", commercialproduct.CommercialProductTypeId);
            commercialproduct.Price = Convert.ToInt16(commercialproduct.Price);
            return View(commercialproduct);
        }

        /**
        * Edita un producto
        *
        * @since 04/04/2013
        * @param Modelo Producto
        * @return Vista de edición de producto
        */

        [HttpPost]
        public ActionResult Edit(CommercialProduct commercialProduct, HttpPostedFileBase image)
        {
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
                    ViewBag.Success = "El producto fue editado exitosamente!";
                }
                catch (Exception)
                {
                    ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud.";
                }
                return View(commercialProduct);
            }
            return View(commercialProduct);
        }

        /**
         * Vista de borrado lógico de producto
         *
         * @since 04/04/2013
         * @param int id
         * @return Vista de borrado de producto
         */

        public ActionResult Delete(int id)
        {
            var commercialproduct = _db.CommercialProducts.FirstOrDefault(c => c.CommercialProductId == id && c.Status == "OK");
            if (commercialproduct == null) throw new HttpException(404, "The resource cannot be found");
            return PartialView(commercialproduct);
        }

        /**
        * Borra lógicamente una producto
        *
        * @since 04/04/2013
        * @param int id
        * @return Mensaje de confirmación
        */

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var commercialproduct = _db.CommercialProducts.FirstOrDefault(c => c.CommercialProductId == id && c.Status == "OK");
            if (commercialproduct != null)
            {
                commercialproduct.Status = "DELETED";
                commercialproduct.LastUpdate = DateTime.Now;
                _db.Entry(commercialproduct).State = EntityState.Modified;
                try
                {
                    ViewBag.Success = "El producto fue quitado exitosamente!";
                    _db.SaveChanges();
                }
                catch (Exception)
                {
                    ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud.";
                }
            }
            else { ViewBag.Error = "Lo sentimos, no pudimos encontrar el producto"; }
            return RedirectToAction("ActiveProducts");
        }

        public ActionResult CommercialProductsOverview()
        {
            var oUser = (User)Session["User"];
            var products = _db.CommercialProducts.Where(c => c.CustomerId == oUser.CustomerId);
            var interactions = new List<ProductOverview>();
            var likes = 0;
            foreach (var pro in products)
            {
                likes = _db.CatalogDetailInteractions.Where(c => c.CatalogDetail.CommercialProductId == pro.CommercialProductId && c.Like == true).Count();
                var oPO = new ProductOverview();
                oPO.Likes = likes;
                oPO.ProductName = pro.ProductName;
                interactions.Add(oPO);
            }
            return PartialView("CommercialProductsOverview", interactions.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}