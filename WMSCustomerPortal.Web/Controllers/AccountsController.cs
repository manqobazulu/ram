using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using WMSCustomerPortal.Business.Models;

namespace WMSCustomerPortal.Web.Controllers {
    public class AccountsController : BaseController {

        WMSCustomerPortal.Business.AccountsService _service;
        private WMSCustomerPortal.Business.AccountsService DataService {
            get {
                if (_service == null) {
                    _service = new WMSCustomerPortal.Business.AccountsService();
                }
                return _service;
            }
        }


        public ActionResult Index() {
            return View();
        }

        [AllowAnonymous]
        public JsonResult GetEmployeeListByID(string startsWith) {

            var data = DataService.GetEmployeeListByID(startsWith);

            return Json(data);
        }

        [AllowAnonymous]
        public JsonResult GetEmployeeListByName(string startsWith) {

            var data = DataService.GetEmployeeListByName(startsWith);

            return Json(data);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult EmployeeLogin(LoginViewModel model, string returnUrl) {
            if (ModelState.IsValid) {

                if (DataService.EmployeeLogin(model.EmployeeID, model.Password)) {

                    FormsAuthentication.SetAuthCookie(model.EmployeeID, false);

                    if (Url.IsLocalUrl(returnUrl)) {
                        return Redirect(returnUrl);
                    }
                    else {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                }
                else {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult LogOff() {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}