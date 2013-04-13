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
    public class AccountController : Controller
    {
        private readonly OMKTDB _db = new OMKTDB();

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

        //GET

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

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

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            return Content("La registración ha sido desactivada de esta aplicación");
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

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

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes

        //private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        //{
        //    // See http://go.microsoft.com/fwlink/?LinkID=177550 for
        //    // a full list of status codes.
        //    switch (createStatus)
        //    {
        //        case MembershipCreateStatus.DuplicateUserName:
        //            return "User name already exists. Please enter a different user name.";

        //        case MembershipCreateStatus.DuplicateEmail:
        //            return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

        //        case MembershipCreateStatus.InvalidPassword:
        //            return "The password provided is invalid. Please enter a valid password value.";

        //        case MembershipCreateStatus.InvalidEmail:
        //            return "The e-mail address provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.InvalidAnswer:
        //            return "The password retrieval answer provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.InvalidQuestion:
        //            return "The password retrieval question provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.InvalidUserName:
        //            return "The user name provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.ProviderError:
        //            return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

        //        case MembershipCreateStatus.UserRejected:
        //            return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

        //        default:
        //            return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
        //    }
        //}

        #endregion Status Codes
    }
}