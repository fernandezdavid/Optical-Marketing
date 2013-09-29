using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using OMKT.Business;
using OMKT.Context;
using OMKT.Models;

namespace OMKT.Controllers
{
    /**
     * Manejador de Cuentas de usuario
     */
    public class AccountController : Controller
    {
        private readonly OMKTDB _db = new OMKTDB();

        /**
         * Vista del perfil de usuario
         * 
         * @since 04/04/2013
         * @return Vista de perfil de usuario
         */

        public PartialViewResult Profile()
        {
            var membershipUser = Membership.GetUser(User.Identity.Name);
            if (membershipUser != null)
            {
                var providerUserKey = membershipUser.ProviderUserKey;
                if (providerUserKey != null)
                {
                    var id = (Guid)providerUserKey;
                    //  var customer = db.Customers.FirstOrDefault(x => x.Users.Fi(u => u.UserId == id));
                    var user = _db.Users.FirstOrDefault(i => i.UserId == id);
                    return PartialView("Profile", user);
                }
            }
            return PartialView();
        }

        /**
         * Vista de inicio de sesión
         * 
         * @since 04/04/2013
         * @return Vista de inicio de sesión
         */

        public ActionResult LogOn()
        {
            return View();
        }

        /**
         * Inicia sesión
         * 
         * @since 04/04/2013
         * @return Vista panel de control
         */

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.User, model.Password))
                {
                    Session["User"] = _db.Users.FirstOrDefault(x => x.Username == model.User);
                    FormsAuthentication.SetAuthCookie(model.User, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Dashboard");
                }
                ModelState.AddModelError("", "");
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /**
         * Cierra sesión
         * 
         * @since 04/04/2013
         * @return Vista principal
         */

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        /**
          * Vista de registro de usuario
          * 
          * @since 04/04/2013
          * @return Vista de registro de usuario
          */

        public ActionResult Register()
        {
            return View();
        }

        /**
         * Registra un usuario
         * 
         * @since 04/04/2013
         * @return Sección desactivada
         */

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            return Content("La registración ha sido desactivada de esta aplicación");
        }

        /**
         * Vista de actualizar contraseña
         * 
         * @since 04/04/2013
         * @return Vista de actualizar de contraseña
         */

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        /**
         * Actualiza la contraseña del usuario
         * 
         * @since 04/04/2013
         * @return Mensaje de confirmación
         */

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                var changePasswordSucceeded = false;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);

                    if (currentUser != null)
                        changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                ModelState.AddModelError("", "");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /**
         * Vista de edición de perfil de usuario
         * 
         * @since 04/04/2013
         * @return Vista de edición de perfil
         */

        [Authorize]
        public ActionResult EditProfile()
        {
            ViewBag.Mode = "Edit";
            var oUser = (User)Session["User"];
            var oCustomer = _db.Customers.FirstOrDefault(c => c.CustomerID == oUser.CustomerId);
            var pr = new ProfileModel
                         {
                             Firstname = oUser.FirstName,
                             Lastname = oUser.LastName,
                             Email = oUser.Email,
                             DateOfBirth = oUser.DateOfBirth,
                             CompanyName = oCustomer.Name,
                             CompanyAdress = oCustomer.Address,
                             CompanyCP = oCustomer.CP,
                             CompanyCity = oCustomer.City,
                             CompanyPhone = oCustomer.Phone1,
                             ContactPerson = oCustomer.ContactPerson
                         };
            return View(pr);
        }

        /**
         * Actualiza los datos del perfil
         * 
         * @since 04/04/2013
         * @return Mensaje de confirmación
         */

        [Authorize]
        [HttpPost]
        public ActionResult EditProfile(ProfileModel model)
        {
            if (ModelState.IsValid)
            {
                //var editProfileSucceeded = false;
                MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                var oUser = _db.Users.FirstOrDefault(u => u.Username == User.Identity.Name);
                Customer oCustomer = null;
                if (oUser != null)
                {
                    oCustomer = _db.Customers.FirstOrDefault(c => c.CustomerID == oUser.CustomerId);
                }

                if (!string.IsNullOrEmpty(model.Email) && oUser != null && oCustomer != null)
                {   //user
                    oUser.FirstName = model.Firstname;
                    oUser.LastName = model.Lastname;
                    oUser.Email = model.Email;
                    oUser.DateOfBirth = model.DateOfBirth;
                    //customer
                    oCustomer.Name = model.CompanyName;
                    oCustomer.Address = model.CompanyAdress;
                    oCustomer.CP = model.CompanyCP;
                    oCustomer.City = model.CompanyCity;
                    oCustomer.ContactPerson = model.ContactPerson;
                    oCustomer.Phone1 = model.CompanyPhone;
                    _db.Entry(oCustomer).State = EntityState.Modified;
                    _db.Entry(oUser).State = EntityState.Modified;
                    try
                    {
                        _db.SaveChanges();
                        return RedirectToAction("Index", "Dashboard");
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", "Debes completar todos los campos");
                        return View(model);
                    }
                }
            }
            return View(model);
        }

        /**
         * Mensaje de confirmación
         * 
         * @since 04/04/2013
         * @return Mensaje de confirmación
         */

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

    }
}