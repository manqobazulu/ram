using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using WMSCustomerPortal.Business.Models;
using WMSCustomerPortal.Models.Entities;
using WMSCustomerPortal.Web.Components;


namespace WMSCustomerPortal.Web.Controllers {
     [Authorize(Roles = "Backdoor, Reporting")]
    public class InventoryController : BaseController {

        WMSCustomerPortal.Models.Entities.ProductStaging _productStaging;
        private WMSCustomerPortal.Models.Entities.ProductStaging ProductStaging
        {
            get
            {
                if (_productStaging == null)
                {
                    _productStaging = new WMSCustomerPortal.Models.Entities.ProductStaging();
                }
                return _productStaging;
            }
        }

        public ActionResult Index() {
            return View();
        }

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        public ActionResult StockLevelAdjustment() {

            return View();
        }

        struct DateRange
        {
            public DateTime DateFrom;
            public DateTime DateTo;
        }

        public ActionResult GetStockLevelAdjustment(JQueryDataTableParamModel param, string formData) {

            JavaScriptSerializer js = new JavaScriptSerializer();
            var dateRange = js.Deserialize<DateRange>(formData);

            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            string result = "";
            List<dynamic> StockLevelAdjustmentData = new List<dynamic>();

            try {

                principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
                string EventUser = this.LoggedOnUser;

                StockLevelAdjustmentData = ProductStaging.GetStockLevelAdjustment(principalID, dateRange.DateFrom, dateRange.DateTo, EventUser);
                result = "SUCCESS";
            }
            catch (Exception ex) {

                DefaultLogger.LogEvent(eventSource: this.ToString(),
                        message: ex.Message, level: RAM.Logging.LogLevel.ERROR, ex: ex,
                        context: "GetStockLevelAdjustmentDataFilter");
                result = string.Format("ERROR: {0}", ex.Message);
                principalID = 0;
            }

            var data = new {
                sEcho = param.sEcho,
                iTotalRecords = StockLevelAdjustmentData.Count,
                iTotalDisplayRecords = StockLevelAdjustmentData.Count,
                aaData = StockLevelAdjustmentData,
                Result = result
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}