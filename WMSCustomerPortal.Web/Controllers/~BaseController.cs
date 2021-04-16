using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using WMSCustomerPortal.Web.Models;

using Newtonsoft.Json;

using RAM.Utilities.Common;

namespace WMSCustomerPortal.Web.Controllers {

    public abstract class BaseController : Controller {

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager {
            get {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set {
                _userManager = value;
            }
        }

        WMSCustomerPortal.Business.CommonService _service;
        private WMSCustomerPortal.Business.CommonService DataService {
            get {
                if (_service == null) {
                    _service = new WMSCustomerPortal.Business.CommonService();
                }
                return _service;
            }
        }

        #region Public Members

        private string GetUserNameFromID(string userID) {
            string retVal = "";
            try {
                var user = UserManager.FindById(userID);

                //right ... lets return the username
                return user.UserName;
            }
            catch {
                retVal = "";
            }
            return retVal;
        }

        public string LoggedOnUser {
            get {
                try {
                    string userID = User.Identity.GetUserId();

                    //lets get the name for this id
                    string userName = GetUserNameFromID(userID);

                    return userName;
                }
                catch { return string.Empty; }
            }
        }

        public string LoggedOnUserID {
            get {
                try {
                    string userID = User.Identity.GetUserId();
                    
                    if(userID == string.Empty || userID == null)
                    {
                        //redirect back to the logon page
                       // RedirectToAction("Home", "Index");

                    }
                    return userID;
                }
                catch { 
                    //redirect to the login page
                    // RedirectToAction("Home", "Index");
                   return string.Empty;
                
                }
            }
        }

        public string TerminalID {
            get {
                try {
                    return RAM.Utilities.Common.HostNames.CachedHostnameFromIPAddress(System.Web.HttpContext.Current.Request.UserHostAddress);
                }
                catch { return string.Empty; }
            }
        }
        #endregion

        #region Controller Overrides

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            //stuff the viewbag with the principal list 
            //ViewBag.selectedPrincipal = "Hello World";


            //check to see if there is any  selected principalcode and principalid

           
            
            try
            {


                //bool getDetails = false;
                //int firstPrincipalID = 0;
                //string firstPrincipalCode = "";
                //getDetails = GetPrincipalDetailForUser(out firstPrincipalID, out firstPrincipalCode, out principalList);

                //if (!getDetails)
                //{
                //    //return RedirectToAction("Index", "Home");

                //}
                //else
                //{

                //    Session.SetDataToSession<List<SelectListItem>>("WMSSession.PrincipalList", principalList);
                //    Session.SetDataToSession<int>("WMSSession.PrincipalID", firstPrincipalID);
                //    Session.SetDataToSession<string>("WMSSession.PrincipalCode", firstPrincipalCode);

                //}

                //lets also check to see if the session variables are set

                //if there aren't any meaningful session variables, we will 


            CheckLoggedInPrincipals();
            ViewBag.selectedPrincipal = Session.GetDataFromSession<string>("WMSSession.PrincipalCode");
            ViewBag.availablePrincipalList = Session.GetDataFromSession<List<SelectListItem>>("WMSSession.PrincipalList");

            }
            catch(Exception ex)
            {
                ViewBag.selectedPrincipal = "UNDEFINED";
                ViewBag.availableList = new List<SelectListItem>(); //empty list

            }
           // int currentprincipalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            
            base.OnActionExecuting(filterContext);
        }

        public bool CheckLoggedInPrincipals()
        {
            bool retVal = false;

            WMSCustomerPortal.Web.Controllers.AccountController acctCtrl = new AccountController();

           try
           {
               //try to get the principalCode and principalList from the sessions
               string sessionPrincipalCode = "";
               string sessionLoggedInUserEmail = "";
                   try
                   {
                       sessionPrincipalCode = Session.GetDataFromSession<string>("WMSSession.PrincipalCode");
                       sessionLoggedInUserEmail = Session.GetDataFromSession<string>("WMSSession.LoggedInUser");

               }
               catch(Exception ex)
                   {

                       sessionPrincipalCode = "UNDEFINED";
                    }
               if(sessionPrincipalCode == "UNDEFINED")
               {
                    //if not repopulate 

                    bool userLevel;
                   int firstPrincipalID = 0;
                   string firstPrincipalCode = "UNDEFINED";
                   List<SelectListItem> principalList = new List<SelectListItem>();
                  // Session.SetDataToSession<string>("WMSSession.LoggedInUser", model.Email);

                   //gets the first principal for the logged in user
                   acctCtrl.GetPrincipalDetailForUser(sessionLoggedInUserEmail, out firstPrincipalID, out firstPrincipalCode, out principalList, out userLevel);


                   Session.SetDataToSession<List<SelectListItem>>("WMSSession.PrincipalList", principalList);
                   Session.SetDataToSession<int>("WMSSession.PrincipalID", firstPrincipalID);
                   Session.SetDataToSession<string>("WMSSession.PrincipalCode", firstPrincipalCode);
                   Session.SetDataToSession<bool>("WMSSession.UserLevel", userLevel);


                }
           }
            catch
           {

               retVal = false;
           }


            return retVal;
        }


        #endregion


        #region Shared Methods

        [AllowAnonymous]
        public JsonResult Suburb_Lookup(string startsWith) {

            var data = DataService.Suburb_Lookup(startsWith);

            return Json(data);
        }

        [AllowAnonymous]
        public JsonResult CustomerGroup_Lookup(string startsWith) {

            var data = DataService.CustomerGroup_Lookup(startsWith);

            return Json(data);
        }

        [AllowAnonymous]
        public JsonResult Customer_Lookup_NoPrincipals(string startsWith) {

            var data = DataService.Customer_Lookup_NoPrincipals(startsWith);

            return Json(data);
        }

    
        [AllowAnonymous]
        public JsonResult Customer_Lookup_by_PrincipalID(string startsWith) {

            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            var data = DataService.Customer_Lookup_by_PrincipalID(startsWith, principalId);

            return Json(data);
        }
        #endregion

        #region Overrides
        protected override JsonResult Json(object data, string contentType,
            Encoding contentEncoding, JsonRequestBehavior behavior) {
            return new JsonNetResult {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }
        #endregion
    }

    public class JsonNetResult : JsonResult {
        public JsonNetResult() {
            Settings = new JsonSerializerSettings {
                ReferenceLoopHandling = ReferenceLoopHandling.Error
            };
        }
        public JsonSerializerSettings Settings { get; private set; }

        public override void ExecuteResult(ControllerContext context) {
            if (context == null)
                throw new ArgumentNullException("context");
            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("JSON GET is not allowed");

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;

            if (this.ContentEncoding != null)
                response.ContentEncoding = this.ContentEncoding;
            if (this.Data == null)
                return;

            var scriptSerializer = JsonSerializer.Create(this.Settings);

            using (var sw = new StringWriter()) {
                scriptSerializer.Serialize(sw, this.Data);
                response.Write(sw.ToString());
            }
        }
    }
}