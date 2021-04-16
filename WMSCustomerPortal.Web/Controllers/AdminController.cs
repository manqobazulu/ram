using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMSCustomerPortal.Web.Components;

namespace WMSCustomerPortal.Web.Controllers {
     [Authorize]
    public class AdminController : BaseController {


        #region Dashboard

        WMSCustomerPortal.Business.MasterDataService _service;
        private WMSCustomerPortal.Business.MasterDataService DataService
        {
            get
            {
                if (_service == null)
                {
                    _service = new WMSCustomerPortal.Business.MasterDataService();
                }
                return _service;
            }
        }

        [HttpGet]
        public ActionResult Dashboard( string id)
        {


            CheckLoggedInPrincipals();

            string userEmail = Session.GetDataFromSession<string>("WMSSession.LoggedInUser");
            var resultPermissions = new List<string>();
            var uniquePermissions = new List<string>();

            if (!String.IsNullOrEmpty(userEmail))
            {

                resultPermissions = DataService.GetAllowedPermissionsList(userEmail.ToUpper());

                foreach (var permission in resultPermissions)
                {
                    if (!uniquePermissions.Contains(permission))
                        uniquePermissions.Add(permission);
                }
            }

            var allowedPermissions = uniquePermissions.ToArray();

            if (allowedPermissions.Any())
            {

                ViewBag.AllowedPermissions = allowedPermissions;
            }
            else
            {
                ViewBag.AllowedPermissions = "You do not have any permissions for this system.";
            }

            TempData["AllowedPermissions"] = allowedPermissions;

            if (id != null)
            {
                string[] separatingStrings = { " AND " };
                string StrRes = id.ToString();
                string[] ResArr = StrRes.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);

                int SuccessCount = Convert.ToInt32(ResArr[0].Substring(0,1));
                int FailedCount = Convert.ToInt32(ResArr[1].Substring(0, 1));

                if (SuccessCount > 0 && FailedCount > 0)
                {
                    TempData["Partially"] = id;
                }
                else if (FailedCount == 0) 
                {
                    TempData["Success"] = id;
                }
                else
                {
                    TempData["Failed"] = id;
                }
            }
            else
            {
                TempData["Partially"] = null;
                TempData["Failed"] = null;
                TempData["Success"] = null;
            }


            // TempData["Status"] = id;

            return View();
        }


        #endregion
    }
}