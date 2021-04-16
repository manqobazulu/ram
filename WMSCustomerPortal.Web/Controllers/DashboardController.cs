using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RAM.Utilities.Common;
using System.Net;
using WMSCustomerPortal.Web.Components;

namespace WMSCustomerPortal.Web.Controllers {
    // [Authorize]
    public class DashboardController : BaseController {

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

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        public ActionResult Index() {

            // Add session by Extension method
            Session.SetDataToSession<int>("WMSSession.PrincipalID", 1);

            string eventUser = this.LoggedOnUser;
            string eventterminal = this.TerminalID;

            return View();
        }

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        //[HttpPost]
        public ActionResult GetPrincipalSession(string principalCode)
        {

            //the principalcode is a string with no id attached to it

            //retrieve the principalid from the code
            WMSCustomerPortal.Business.MasterDataService masterData = new Business.MasterDataService();
            int principalID = 0;
            string principal = "";
            string rPrincipalCode = "";
            string userName = this.LoggedOnUser;
            bool UserLevel = false;

            principal = masterData.GetSetPrincipalIDByCode(principalCode, userName);
            string[] tokens = principal.Split(':');
            principalID = int.Parse(tokens[0]);
            rPrincipalCode = tokens[1];
            UserLevel  = Convert.ToBoolean(Convert.ToInt16(tokens[2]));

            Session.SetDataToSession<string>("WMSSession.PrincipalCode", Functions.SafeString(rPrincipalCode, "UNDEFINED"));
            Session.SetDataToSession<int>("WMSSession.PrincipalID", Functions.SafeInt(principalID, 0));
            Session.SetDataToSession<bool>("WMSSession.UserLevel", UserLevel);

            var response = new {
                Result = principal
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        
        }


        /// <summary>
        /// Gets the inbound list
        /// </summary>
        /// <param name="param"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public JsonResult GetPrincipalList()
        {

            List<SelectListItem> availPrincipals = new List<SelectListItem>();
            try
            {
             availPrincipals = Session.GetDataFromSession<List<SelectListItem>>("WMSSession.PrincipalList");

            }
            catch(Exception ex)
            {
               
              availPrincipals = new List<SelectListItem>(); //empty list

            }

            var data = new
            {
                aaData = availPrincipals
            };

            // Test serialization using overriden method
            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;
        }

        private static List<SelectListItem> GenerateNumbers()
        {
            var numbers = (from p in Enumerable.Range(0, 20)
                           select new SelectListItem
                           {
                               Text = p.ToString(),
                               Value = p.ToString()
                           });
            return numbers.ToList();
        }


    }
}