using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OMKT.Business;
using OMKT.Context;
using Paging;

namespace OMKT.Controllers
{
    [Authorize]
    public class CommercialProductController : Controller
    {
        private readonly int _defaultPageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DefaultPaginationSize"]);
        private readonly OMKTDB _db = new OMKTDB();

        public ViewResultBase Search(string q, int? page)
        {
            IQueryable<CommercialProduct> products = _db.CommercialProducts;

            if (q.Length == 1)//alphabetical search, first letter
            {
                ViewBag.LetraAlfabetica = q;
                products = products.Where(c => c.ProductName.StartsWith(q));
            }
            else if (q.Length > 1)
            {
                //normal search
                products = products.Where(c => c.ProductName.IndexOf(q, StringComparison.Ordinal) > -1);
            }

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            var customersListPaged = products.OrderBy(i => i.ProductName).ToPagedList(currentPageIndex, _defaultPageSize);

            if (Request.IsAjaxRequest())
                return PartialView("Index", customersListPaged);
            return View("Index", customersListPaged);
        }

        public PartialViewResult ActiveProducts(int? top)
        {
            if (!top.HasValue) top = 5;
            //MembershipUser user = Membership.GetUser(User.Identity.Name);
            //if (user == null)
            //{
            //    throw new InvalidOperationException("User [" +
            //        User.Identity.Name + " ] not found.");
            //}
            // Do whatever u want with the unique identifier.
            //Guid guid = (Guid)user.ProviderUserKey;
            var oUser = (User)Session["User"];
            var catalogs = _db.CommercialProducts.Where(c => c.CustomerId == oUser.CustomerId).OrderByDescending(i => i.ProductName).Take(top.Value);
            if (catalogs.Any())
                return PartialView("CommercialProductListPartial", catalogs.ToList());
            return PartialView("CommercialProductListPartial");
        }

        // GET: /CommercialProduct/
        public ViewResult Index(int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            return View(_db.CommercialProducts.OrderBy(c => c.ProductName).ToPagedList(currentPageIndex, _defaultPageSize));
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
                    var fileName = Path.GetFileName(image.FileName);
                    if (fileName != null)
                    {
                        var path = Path.Combine(Server.MapPath("~/Content/productImages"), fileName);
                        image.SaveAs(path);
                    }
                    var prodType = _db.CommercialProductTypes.Find(commercialProduct.CommercialProductTypeId);
                    var img = new ProductImage
                                  {
                                      Path = Url.Content("~/brand/product-images/" + fileName),
                                      Caption = image.FileName.Substring(0, image.FileName.Length - 4),
                                      Title = image.FileName.Substring(0, image.FileName.Length - 4),
                                      Extension = image.FileName.Substring(image.FileName.Length - 4, 4),
                                      CreatedDate = DateTime.Now,
                                      Size = image.ContentLength.ToString(CultureInfo.InvariantCulture),
                                      ThumbnailPath = Url.Content("~/brand/product-images/" + fileName)
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
                            ViewBag.Success = "El producto fue registrado satisfactorimente.";
                            return RedirectToAction("Edit", new { id = commercialProduct.CommercialProductId });
                        }
                        catch (Exception)
                        {
                            ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud.";
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

        public ActionResult Edit(int id)
        {
            ViewBag.Mode = "Edit";
            CommercialProduct commercialproduct = _db.CommercialProducts.Find(id);
            ViewBag.CommercialProductTypeId = new SelectList(_db.CommercialProductTypes, "CommercialProductTypeId", "Description", commercialproduct.CommercialProductTypeId);
            return View(commercialproduct);
        }

        //
        // POST: /CommercialProduct/Edit/5

        [HttpPost]
        public ActionResult Edit(CommercialProduct commercialProduct, HttpPostedFileBase image)
        {
            ViewBag.Mode = "Edit";
            ViewBag.CommercialProductTypeId = new SelectList(_db.CommercialProductTypes, "CommercialProductTypeId", "Description", commercialProduct.CommercialProductTypeId);
            var img = _db.ProductImages.Find(commercialProduct.ProductImageId);
            if (ModelState.IsValid)
            {
                if (image != null && image.ContentLength > 0)
                {
                    // extract only the fielname
                    var fileName = Path.GetFileName(image.FileName);
                    // store the file inside ~/App_Data/uploads folder
                    if (fileName != null)
                    {
                        var path = Path.Combine(Server.MapPath("~/Content/productImages"), fileName);
                        image.SaveAs(path);
                    }
                    img.Path = Url.Content("~/brand/product-images/" + fileName);
                    img.ThumbnailPath = Url.Content("~/brand/product-images/" + fileName);
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
                    ViewBag.Success = "El producto fue editado satisfactorimente.";
                    return RedirectToAction("Edit", new { id = commercialProduct.CommercialProductId });
                }
                catch (Exception)
                {
                    ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud.";
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
            return View(commercialproduct);
        }

        //
        // POST: /CommercialProduct/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            CommercialProduct commercialproduct = _db.CommercialProducts.Find(id);
            _db.CommercialProducts.Remove(commercialproduct);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}