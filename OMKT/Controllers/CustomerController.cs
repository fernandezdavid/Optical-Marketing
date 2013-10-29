using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OMKT.Business;
using OMKT.Context;

namespace OMKT.Controllers
{ 
    /**
     * Manejador de vistas de Cliente
     */

    public class CustomerController : Controller
    {
        private OMKTDB db = new OMKTDB();

        /**
         * Vista del índice de la sección Clientes
         *
         * @since 04/04/2013
         * @return Vista principal de clientes
         */

        public ViewResult Index()
        {
            return View(db.Customers.ToList());
        }

        /**
         * Vista del detalle de cliente
         *
         * @since 04/04/2013
         * @param int id
         * @return Vista de detalle de cliente
         */

        public ViewResult Details(int id)
        {
            Customer customer = db.Customers.Find(id);
            return View(customer);
        }

        /**
         * Vista de creación de cliente
         *
         * @since 04/04/2013
         * @return Vista de creación de cliente
         */

        public ActionResult Create()
        {
            return View();
        } 

        /**
        * Crea una cliente
        *
        * @since 04/04/2013
        * @param Modelo Cliente
        * @return Vista de edición de cliente
        */

        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(customer);
        }
        
        /**
        * Vista de edición de Cliente
        *
        * @since 04/04/2013
        * @param int id
        * @return Vista de edición de cliente
        */
 
        public ActionResult Edit(int id)
        {
            Customer customer = db.Customers.Find(id);
            return View(customer);
        }

        /**
        * Edita un cliente
        *
        * @since 04/04/2013
        * @param Modelo Cliente
        * @return Vista de edición de cliente
        */

        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        /**
         * Vista de borrado lógico de cliente
         *
         * @since 04/04/2013
         * @param int id
         * @return Vista de borrado de cliente
         */
 
        public ActionResult Delete(int id)
        {
            Customer customer = db.Customers.Find(id);
            return View(customer);
        }

        /**
        * Borra lógicamente una cliente
        *
        * @since 04/04/2013
        * @param int id
        * @return Mensaje de confirmación
        */

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}