using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using WMSCustomerPortal.Business;
using WMSCustomerPortal.Business.Models;
using WMSCustomerPortal.Models;
using WMSCustomerPortal.Models.Common;
using WMSCustomerPortal.Models.Entities;
using WMSCustomerPortal.Web.Components;


namespace WMSCustomerPortal.Web.Controllers
{
    [Authorize(Roles = "Backdoor, Reporting")]
    public class ReportsController : BaseController
    {

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

        //These are my references
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

        WMSCustomerPortal.Models.Entities.InboundMaster _inboundMaster;
        private WMSCustomerPortal.Models.Entities.InboundMaster InboundMaster
        {
            get
            {
                if (_inboundMaster == null)
                {
                    _inboundMaster = new WMSCustomerPortal.Models.Entities.InboundMaster();
                }
                return _inboundMaster;
            }
        }

        //Start
        WMSCustomerPortal.Models.Entities.Orders _Orders;
        private WMSCustomerPortal.Models.Entities.Orders Orders
        {
            get
            {
                if (_Orders == null)
                {
                    _Orders = new WMSCustomerPortal.Models.Entities.Orders();
                }
                return _Orders;
            }
        }

        WMSCustomerPortal.Models.DataAccess.SQLDataManager _sqlManager;
        private WMSCustomerPortal.Models.DataAccess.SQLDataManager SqlManager
        {
            get
            {
                if (_sqlManager == null)
                {
                    _sqlManager = new WMSCustomerPortal.Models.DataAccess.SQLDataManager();
                }
                return _sqlManager;
            }
        }


        public ActionResult Index()
        {
            return View();
        }

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        public ActionResult Reports()
        {
            return View();
        }

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        public ActionResult Dispatched()
        {
            return View();
        }

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        public ActionResult WarehouseActivity()
        {
            return View();
        }

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        public ActionResult Receiving()
        {
            return View();
        }

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        public ActionResult StockOnHand()
        {
            return View();
        }

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        public ActionResult StockAge()
        {
            return View();
        }

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        public ActionResult StockAgeDetail()
        {
            return View();
        }

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        public ActionResult ReceivingVariance()
        {
            return View();
        }

        public List<dynamic> RemoveDuplicatesIterative(List<dynamic> items)
        {
            List<dynamic> result = new List<dynamic>();
            for (int i = 0; i < items.Count; i++)
            {
                // Assume not duplicate.
                bool duplicate = false;
                for (int z = 0; z < i; z++)
                {
                    if (items[z].ProdCode == items[i].ProdCode)
                    {
                        // This is a duplicate.
                        duplicate = true;
                        //items[z].StockDateTime = 0;
                        //items[z].ReceivedQuantity = 0;
                        break;
                    }

                }
                // If not duplicate, add to result.
                if (!duplicate)
                {
                    result.Add(items[i]);
                    items[i].LastReturnDate = new DateTime();
                    items[i].ReturnQuantity = 0;
                }

            }
            return result;
        }

        public ActionResult GetStockOnHand(JQueryDataTableParamModel param, string formData)
        {

            JavaScriptSerializer js = new JavaScriptSerializer();
            var showZeroInventory = js.Deserialize<SearchModel>(formData);

            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            string result = "";
            List<dynamic> StockOnHandData = new List<dynamic>();

            try
            {
                principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
                StockOnHandData = ProductStaging.GetStockOnHand(principalID, showZeroInventory.ShowZeroInventory);
                result = "SUCCESS";
            }
            catch (Exception ex)
            {

                DefaultLogger.LogEvent(eventSource: this.ToString(),
                        message: ex.Message, level: RAM.Logging.LogLevel.ERROR, ex: ex,
                        context: "GetStockOnHandFilter");
                result = string.Format("ERROR: {0}", ex.Message);
                principalID = 0;
            }

            var data = new
            {
                iTotalRecords = StockOnHandData.Count,
                iTotalDisplayRecords = StockOnHandData.Count,
                aaData = StockOnHandData,
                Result = result
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStockOnHandSerials(JQueryDataTableParamModel param, string formData)
        {

            JavaScriptSerializer js = new JavaScriptSerializer();
            var showZeroInventory = js.Deserialize<SearchModel>(formData);

            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            string result = "";
            List<dynamic> StockOnHandData = new List<dynamic>();

            try
            {
                principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
                StockOnHandData = ProductStaging.GetStockOnHandSerials(principalID, showZeroInventory.ShowZeroInventory);
                result = "SUCCESS";
            }
            catch (Exception ex)
            {

                DefaultLogger.LogEvent(eventSource: this.ToString(),
                        message: ex.Message, level: RAM.Logging.LogLevel.ERROR, ex: ex,
                        context: "GetStockOnHandFilter");
                result = string.Format("ERROR: {0}", ex.Message);
                principalID = 0;
            }

            var data = new
            {
                iTotalRecords = StockOnHandData.Count,
                iTotalDisplayRecords = StockOnHandData.Count,
                aaData = StockOnHandData,
                Result = result
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStockAging(JQueryDataTableParamModel param,string formData)
        {
            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            string result = "";
            List<dynamic> StockAingData = new List<dynamic>();

            try
            {
                StockAingData = ProductStaging.GetStockAgeData(principalID);
                result = "SUCCESS";
            }
            catch (Exception ex)
            {
                DefaultLogger.LogEvent(eventSource: this.ToString(),
                        message: ex.Message, level: RAM.Logging.LogLevel.ERROR, ex: ex,
                        context: "GetStockOnHandFilter");
                result = string.Format("ERROR: {0}", ex.Message);
                principalID = 0;
            }

            var data = new
            {
                iTotalRecords = StockAingData.Count,
                iTotalDisplayRecords = StockAingData.Count,
                aaData = StockAingData,
                Result = result
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNewStockAgeDetail(JQueryDataTableParamModel param, string formData)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            //Deserialise the data
            var dateRange = js.Deserialize<DateRange>(formData);

            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            string result = "";
            List<dynamic> StockAingDataNew = new List<dynamic>();

            try
            {
                StockAingDataNew = ProductStaging.GetNewStockAgeDetailData(principalID, dateRange.DateFrom, dateRange.DateTo);
                result = "SUCCESS";
            }
            catch (Exception ex)
            {
                DefaultLogger.LogEvent(eventSource: this.ToString(),
                        message: ex.Message, level: RAM.Logging.LogLevel.ERROR, ex: ex,
                        context: "GetStockOnHandFilter");
                result = string.Format("ERROR: {0}", ex.Message);
                principalID = 0;
            }

            var data = new
            {
                iTotalRecords = StockAingDataNew.Count,
                iTotalDisplayRecords = StockAingDataNew.Count,
                aaData = StockAingDataNew,
                Result = result
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStockAgeDetailReport(JQueryDataTableParamModel param, string formData)
        {
            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            string result = "";
            List<dynamic> StockAingData = new List<dynamic>();

            try
            {
                StockAingData = ProductStaging.GetStockAgeDetailData(principalID);
                result = "SUCCESS";
            }
            catch (Exception ex)
            {
                DefaultLogger.LogEvent(eventSource: this.ToString(),
                        message: ex.Message, level: RAM.Logging.LogLevel.ERROR, ex: ex,
                        context: "GetStockOnHandFilter");
                result = string.Format("ERROR: {0}", ex.Message);
                principalID = 0;
            }

            var data = new
            {
                iTotalRecords = StockAingData.Count,
                iTotalDisplayRecords = StockAingData.Count,
                aaData = StockAingData,
                Result = result
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        struct DateRange
        {
            public DateTime DateFrom;
            public DateTime DateTo;
        }
        public ActionResult GetIGDs(JQueryDataTableParamModel param, string formData)
        {

            JavaScriptSerializer js = new JavaScriptSerializer();
            var dateRange = js.Deserialize<DateRange>(formData);


            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            string result = "";
            List<dynamic> IGDsData = new List<dynamic>();

            try
            {

                principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
                IGDsData = ProductStaging.GetIGDs(principalID, dateRange.DateFrom, dateRange.DateTo);
                result = "SUCCESS";
            }
            catch (Exception ex)
            {

                DefaultLogger.LogEvent(eventSource: this.ToString(),
                        message: ex.Message, level: RAM.Logging.LogLevel.ERROR, ex: ex,
                        context: "GetIGDsDataFilter");
                result = string.Format("ERROR: {0}", ex.Message);
                principalID = 0;
            }

            var data = new
            {
                sEcho = param.sEcho,
                iTotalRecords = IGDsData.Count,
                iTotalDisplayRecords = IGDsData.Count,
                aaData = IGDsData,
                Result = result
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInboundMaster(JQueryDataTableParamModel param, string formData)
        {

            JavaScriptSerializer js = new JavaScriptSerializer();
            var dateRange = js.Deserialize<DateRange>(formData);

            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            string result = "";
            List<dynamic> IGDsData = new List<dynamic>();

            try
            {

                principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
                IGDsData = ProductStaging.GetInboundMaster(principalID, dateRange.DateFrom, dateRange.DateTo);
                result = "SUCCESS";
            }
            catch (Exception ex)
            {

                DefaultLogger.LogEvent(eventSource: this.ToString(),
                        message: ex.Message, level: RAM.Logging.LogLevel.ERROR, ex: ex,
                        context: "GetInboundMastersDataFilter");
                result = string.Format("ERROR: {0}", ex.Message);
                principalID = 0;
            }

            var data = new
            {
                sEcho = param.sEcho,
                iTotalRecords = IGDsData.Count,
                iTotalDisplayRecords = IGDsData.Count,
                aaData = IGDsData,
                Result = result
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDispatched(JQueryDataTableParamModel param, string formData)
        {

            JavaScriptSerializer js = new JavaScriptSerializer();
            //Deserialise the data
            var dateRange = js.Deserialize<DateRange>(formData);

            //WMSSession.PrincipalID - IS the selected code HUAW20
            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            string result = "";
            List<dynamic> DispatchedData = new List<dynamic>();

            try
            {
                //LOGGED user
                principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
                string EventUser = this.LoggedOnUser;
                
                DispatchedData = ProductStaging.GetDispatched(principalID, dateRange.DateFrom, dateRange.DateTo, EventUser);
                result = "SUCCESS";
            }
            catch (Exception ex)
            {

                DefaultLogger.LogEvent(eventSource: this.ToString(),
                        message: ex.Message, level: RAM.Logging.LogLevel.ERROR, ex: ex,
                        context: "GetDispatchedDataFilter");
                result = string.Format("ERROR: {0}", ex.Message);
                principalID = 0;
            }

            var data = new
            {
                sEcho = param.sEcho,
                iTotalRecords = DispatchedData.Count,
                iTotalDisplayRecords = DispatchedData.Count,
                aaData = DispatchedData,
                Result = result
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        } 

        public ActionResult GetDispatchedSerials(JQueryDataTableParamModel param, string formData)
        {

            JavaScriptSerializer js = new JavaScriptSerializer();
            var dateRange = js.Deserialize<DateRange>(formData);

            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            string result = "";
            List<dynamic> DispatchedData = new List<dynamic>();

            try
            {

                principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
                string EventUser = this.LoggedOnUser;


                DispatchedData = ProductStaging.GetDispatchedSerials(principalID, dateRange.DateFrom, dateRange.DateTo, EventUser);
                result = "SUCCESS";
            }
            catch (Exception ex)
            {

                DefaultLogger.LogEvent(eventSource: this.ToString(),
                        message: ex.Message, level: RAM.Logging.LogLevel.ERROR, ex: ex,
                        context: "GetDispatchedDataFilter");
                result = string.Format("ERROR: {0}", ex.Message);
                principalID = 0;
            }

            var data = new
            {
                sEcho = param.sEcho,
                iTotalRecords = DispatchedData.Count,
                iTotalDisplayRecords = DispatchedData.Count,
                aaData = DispatchedData,
                Result = result
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInbound(JQueryDataTableParamModel param, string formData)
        {

            JavaScriptSerializer js = new JavaScriptSerializer();
            //Deserialise the data
            var dateRange = js.Deserialize<DateRange>(formData);

            //WMSSession.PrincipalID - IS the selected code HUAW20
            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            string result = "";
            List<dynamic> InboundData = new List<dynamic>();

            try
            {
                //LOGGED user
                principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
                string EventUser = this.LoggedOnUser;

                InboundData = InboundMaster.GetInbound(principalID, dateRange.DateFrom, dateRange.DateTo, EventUser);
                result = "SUCCESS";
            }
            catch (Exception ex)
            {

                DefaultLogger.LogEvent(eventSource: this.ToString(),
                        message: ex.Message, level: RAM.Logging.LogLevel.ERROR, ex: ex,
                        context: "GetDispatchedDataFilter");
                result = string.Format("ERROR: {0}", ex.Message);
                principalID = 0;
            }

            var data = new
            {
                sEcho = param.sEcho,
                iTotalRecords = InboundData.Count,
                iTotalDisplayRecords = InboundData.Count,
                aaData = InboundData,
                Result = result
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReturns(JQueryDataTableParamModel param, string formData)
        {

            JavaScriptSerializer js = new JavaScriptSerializer();
            //Deserialise the data
            var dateRange = js.Deserialize<DateRange>(formData);

            //WMSSession.PrincipalID - IS the selected code HUAW20
            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            string result = "";
            List<dynamic> InboundData = new List<dynamic>();

            try
            {
                //LOGGED user
                principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
                string EventUser = this.LoggedOnUser;

                InboundData = InboundMaster.GetReturns(principalID, dateRange.DateFrom, dateRange.DateTo, EventUser);
                result = "SUCCESS";
            }
            catch (Exception ex)
            {

                DefaultLogger.LogEvent(eventSource: this.ToString(),
                        message: ex.Message, level: RAM.Logging.LogLevel.ERROR, ex: ex,
                        context: "GetDispatchedDataFilter");
                result = string.Format("ERROR: {0}", ex.Message);
                principalID = 0;
            }

            var data = new
            {
                sEcho = param.sEcho,
                iTotalRecords = InboundData.Count,
                iTotalDisplayRecords = InboundData.Count,
                aaData = InboundData,
                Result = result
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOutbound(JQueryDataTableParamModel param, string formData)
        {

            JavaScriptSerializer js = new JavaScriptSerializer();
            //Deserialise the data
            var dateRange = js.Deserialize<DateRange>(formData);

            //WMSSession.PrincipalID - IS the selected code HUAW20
            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            
            //DateTime endDate = startDate.AddMonths(1);
            DateTime dateValue = new DateTime();

            string result = "";
            List<dynamic> outboundData = new List<dynamic>();
            
            try
            { 

                principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
                string EventUser = this.LoggedOnUser;                
                outboundData = Orders.GetOutbound(principalID, dateRange.DateFrom, dateRange.DateTo, EventUser);
                      
            }
            catch (Exception ex)
            {

                DefaultLogger.LogEvent(eventSource: this.ToString(),
                        message: ex.Message, level: RAM.Logging.LogLevel.ERROR, ex: ex,
                        context: "GetDispatchedDataFilter");
                result = string.Format("ERROR: {0}", ex.Message);
                principalID = 0;
            }

            var data = new
            {
                sEcho = param.sEcho,
                iTotalRecords = outboundData.Count,
                iTotalDisplayRecords = outboundData.Count,
                aaData = outboundData,
                Result = result
            };
 
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetReturnReceipt(JQueryDataTableParamModel param, string formData)
        {

            JavaScriptSerializer js = new JavaScriptSerializer();
            var dateRange = js.Deserialize<DateRange>(formData);


            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            string result = "";
            List<dynamic> ReturnData = new List<dynamic>();

            try
            {

                principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
                ReturnData = ProductStaging.GetReturnReceipt(principalID, dateRange.DateFrom, dateRange.DateTo);
                result = "SUCCESS";
            }
            catch (Exception ex)
            {

                DefaultLogger.LogEvent(eventSource: this.ToString(),
                        message: ex.Message, level: RAM.Logging.LogLevel.ERROR, ex: ex,
                        context: "GetReturnDataFilter");
                result = string.Format("ERROR: {0}", ex.Message);
                principalID = 0;
            }

            var data = new
            {
                sEcho = param.sEcho,
                iTotalRecords = ReturnData.Count,
                iTotalDisplayRecords = ReturnData.Count,
                aaData = ReturnData,
                Result = result
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetVarianceReport(JQueryDataTableParamModel param, string formData)
        {

            JavaScriptSerializer js = new JavaScriptSerializer();
            var dateRange = js.Deserialize<DateRange>(formData);

            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            string result = "";
            List<dynamic> ReportData = new List<dynamic>();

            try
            {

                principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
                string EventUser = this.LoggedOnUser;


                ReportData = ProductStaging.GetIGDVariance(principalID, dateRange.DateFrom, dateRange.DateTo, EventUser);
                result = "SUCCESS";
            }
            catch (Exception ex)
            {

                DefaultLogger.LogEvent(eventSource: this.ToString(),
                        message: ex.Message, level: RAM.Logging.LogLevel.ERROR, ex: ex,
                        context: "GetDispatchedDataFilter");
                result = string.Format("ERROR: {0}", ex.Message);
                principalID = 0;
            }

            var data = new
            {
                sEcho = param.sEcho,
                iTotalRecords = ReportData.Count,
                iTotalDisplayRecords = ReportData.Count,
                aaData = ReportData,
                Result = result
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}