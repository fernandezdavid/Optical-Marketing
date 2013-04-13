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
    public class UserController : Controller
    {
        private readonly OMKTDB _db = new OMKTDB();
        private readonly int _defaultPageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DefaultPaginationSize"]);
        /*CUSTOM*/

        public ViewResultBase Search(string q, int? page)
        {
            IQueryable<User> users = _db.Users;

            if (q.Length == 1)//alphabetical search, first letter
            {
                ViewBag.LetraAlfabetica = q;
                users = users.Where(c => c.Username.StartsWith(q));
            }
            else if (q.Length > 1)
            {
                //normal search
                users = users.Where(c => c.Username.IndexOf(q, StringComparison.Ordinal) > -1);
            }

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            var usersListPaged = users.OrderBy(i => i.Username).ToPagedList(currentPageIndex, _defaultPageSize);

            if (Request.IsAjaxRequest())
                return PartialView("Index", usersListPaged);
            return View("Index", usersListPaged);
        }

        /*END CUSTOM*/

        //
        // GET: /User/

        public ViewResult Index(int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            return View(_db.Users.OrderBy(c => c.Username).ToPagedList(currentPageIndex, _defaultPageSize));
        }

        //
        // GET: /User/Details/5

        public ViewResult Details(Guid id)
        {
            var user = _db.Users.Find(id);
            return View(user);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return PartialView();
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                user.UserId = Guid.NewGuid();
                _db.Users.Add(user);
                _db.SaveChanges();
                return PartialView("UserListPartial", _db.Users.OrderBy(c => c.Username).ToPagedList(0, _defaultPageSize));
            }
            Response.StatusCode = 400;
            return PartialView(user);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(Guid id)
        {
            User user = _db.Users.Find(id);
            return PartialView(user);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
                return PartialView("UserListPartial", _db.Users.OrderBy(c => c.Username).ToPagedList(0, _defaultPageSize));
            }
            Response.StatusCode = 400;
            return PartialView(user);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(Guid id)
        {
            User user = _db.Users.Find(id);
            return View(user);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            User user = _db.Users.Find(id);
            _db.Users.Remove(user);
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