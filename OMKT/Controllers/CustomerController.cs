using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using OMKT.Business;
using OMKT.Context;
using Paging;

namespace OMKT.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class CustomerController : Controller
    {
        private readonly int _defaultPageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DefaultPaginationSize"]);
        private readonly OMKTDB _db = new OMKTDB();

        /*CUSTOM*/

        public ViewResultBase Search(string q, int? page)
        {
            IQueryable<Customer> customers = _db.Customers;

            if (q.Length == 1)//alphabetical search, first letter
            {
                ViewBag.LetraAlfabetica = q;
                customers = customers.Where(c => c.Name.StartsWith(q));
            }
            else if (q.Length > 1)
            {
                //normal search
                customers = customers.Where(c => c.Name.IndexOf(q, StringComparison.Ordinal) > -1);
            }

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            var customersListPaged = customers.OrderBy(i => i.Name).ToPagedList(currentPageIndex, _defaultPageSize);

            if (Request.IsAjaxRequest())
                return PartialView("Index", customersListPaged);
            return View("Index", customersListPaged);
        }

        /*END CUSTOM*/

        //
        // GET: /Customer/

        public ViewResult Index(int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            return View(_db.Customers.OrderBy(c => c.Name).ToPagedList(currentPageIndex, _defaultPageSize));
        }

        //
        // GET: /Customer/Details/5

        public ViewResult Details(int id)
        {
            Customer customer = _db.Customers.Find(id);
            return View(customer);
        }

        //
        // GET: /Customer/Create

        public ActionResult Create()
        {
            return PartialView();
        }

        //
        // POST: /Customer/Create

        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _db.Customers.Add(customer);
                _db.SaveChanges();
                //return list of customers as it is ajax request
                return PartialView("CustomerListPartial", _db.Customers.OrderBy(c => c.Name).ToPagedList(0, _defaultPageSize));
                //return RedirectToAction("Index");
            }
            Response.StatusCode = 400;
            return PartialView(customer);
        }

        //
        // GET: /Customer/Edit/5

        public ActionResult Edit(int id)
        {
            Customer customer = _db.Customers.Find(id);
            return PartialView(customer);
        }

        //
        // POST: /Customer/Edit/5

        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(customer).State = EntityState.Modified;
                _db.SaveChanges();
                //return RedirectToAction("Index");
                //return list of customers as it is ajax request
                return PartialView("CustomerListPartial", _db.Customers.OrderBy(c => c.Name).ToPagedList(0, _defaultPageSize));
            }
            Response.StatusCode = 400;
            return PartialView(customer);
        }

        //
        // GET: /Customer/Delete/5

        public ActionResult Delete(int id)
        {
            Customer customer = _db.Customers.Find(id);
            return View(customer);
        }

        //
        // POST: /Customer/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = _db.Customers.Find(id);
            _db.Customers.Remove(customer);
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