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
using Microsoft.AspNet.Identity;
using WMSCustomerPortal.Web.Components;
using Functions = RAM.Utilities.Common.Functions;


namespace WMSCustomerPortal.Web.Controllers {

   [Authorize(Roles = "Backdoor, Outbound")]
    public class OutboundController : BaseController {

       [Authorize]
       public ActionResult ItemLineEdit(int outboundOrderID=0 ) {
           //pass through a model for the new page
           var outboundAddModel = new OutboundOrder { OutboundOrderID = outboundOrderID };
           return View(outboundAddModel);
       }

        WMSCustomerPortal.Models.DataAccess.SQLDataManager _sqlManager;

        private WMSCustomerPortal.Models.DataAccess.SQLDataManager SqlManager {
            get {
                if (_sqlManager == null) {
                    _sqlManager = new WMSCustomerPortal.Models.DataAccess.SQLDataManager();
                }
                return _sqlManager;
            }
        }

        WMSCustomerPortal.Business.OutboundService _outboundService;

        private WMSCustomerPortal.Business.OutboundService OutboundDataService
        {
            get
            {
                if (_outboundService == null)
                {
                    _outboundService = new WMSCustomerPortal.Business.OutboundService();
                }
                return _outboundService;
            }
        }

        WMSCustomerPortal.Business.CommonService _commonservice;
        private WMSCustomerPortal.Business.CommonService CommonDataService {
            get {
                if (_commonservice == null) {
                    _commonservice = new WMSCustomerPortal.Business.CommonService();
                }
                return _commonservice;
            }
        }

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


        // GET: Outbound

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        public ActionResult Index() {
            return View();
        }

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        public ActionResult OutboundMain(bool? userLevel)
        {
            if (userLevel.Equals(null))
            {
                try
                {
                    userLevel = Session.GetDataFromSession<bool>("WMSSession.UserLevel");
                }
                catch
                {
                    userLevel = false;
                }
            }

            var model = new UserLevelMainModel
            {
                UserLevel = userLevel
            };

            return View(model);
        }

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        public ActionResult OutboundMultipleReceiver(bool? userLevel)
        {
            if (userLevel.Equals(null))
            {
                try
                {
                    userLevel = Session.GetDataFromSession<bool>("WMSSession.UserLevel");
                }
                catch
                {
                    userLevel = false;
                }
            }

            var model = new UserLevelMainModel
            {
                UserLevel = userLevel
            };

            return View(model);
        }

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        public ActionResult SingleReceiver(bool? userLevel)
        {

            if (userLevel.Equals(null))
            {
                try
                {
                    userLevel = Session.GetDataFromSession<bool>("WMSSession.UserLevel");
                }
                catch
                {
                    userLevel = false;
                }
            }

            var model = new UserLevelMainModel
            {
                UserLevel = userLevel
            };

            return View(model);
        }

        public JsonResult OrderCustomerName_Lookup(string startsWith)
        {

            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            List<CustomerOrdersLookUpItem> data = new List<CustomerOrdersLookUpItem>();

            string result = "";

            try {

                string EventUser = this.LoggedOnUser;
                data = SqlManager.OrderCustomerName_Lookup(startsWith.Trim(), principalId, EventUser);

                result = "SUCCESS";
            }
            catch (Exception ex) {
                // Logging
                DefaultLogger.LogEvent(eventSource: this.ToString(),
                        message: ex.Message, level: RAM.Logging.LogLevel.ERROR, ex: ex,
                        context: "OrderCustomerName_Lookup");

                result = string.Format("ERROR: {0}", ex.Message);
            }

            return Json(data);
        }

        public JsonResult CustomerName_Lookup(string startsWith) {

            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            var data = SqlManager.CustomerName_Lookup(startsWith.Trim(), principalId);

            return Json(data);
        }

        public JsonResult ProductCode_Lookup(string startsWith)
        {

            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            var data = SqlManager.ProductCode_Lookup(startsWith.Trim(), principalId);

            return Json(data);
        }

        public JsonResult GetCustomerDetail(string CustomerID) {

            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            var result = OutboundDataService.GetCustomerDetail(CustomerID.Trim(), principalId);

            var data = new {
                Result = "SUCCESS",
                RecordCount = result.Count,
                Data = result
            };

            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;

        }

        public JsonResult GetOrderDetail(int OrderID) {

            var result = OutboundDataService.GetOrderDetail(OrderID);

            var data = new {
                Result = "SUCCESS",
                RecordCount = result.Count,
                Data = result
            };

            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;

        }

        public JsonResult GetOrderStatus(int OrderID)
        {

            var result = OutboundDataService.GetOrderStatus(OrderID);

            var data = new
            {
                Result = "SUCCESS",
                RecordCount = result.Count,
                Data = result
            };

            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;

        }

        public JsonResult GetOutboundOrderFilter(JQueryDataTableParamModel param, string formData) {

            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            string result = "";
            List<dynamic> outboundOrderData = new List<dynamic>();

            try {

                string EventUser = this.LoggedOnUser;
                principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
                outboundOrderData = OutboundDataService.SearchOrdersDS(principalID, EventUser);
                result = "SUCCESS";
              }
              catch (Exception ex) {

                DefaultLogger.LogEvent(eventSource: this.ToString(),
                        message: ex.Message, level: RAM.Logging.LogLevel.ERROR, ex: ex,
                        context: "GetOutboundOrderFilter");
                result = string.Format("ERROR: {0}", ex.Message);
                principalID = 0;
              }

                var data = new {
                    sEcho = param.sEcho,
                    iTotalRecords = outboundOrderData.Count,
                    iTotalDisplayRecords = outboundOrderData.Count,
                    aaData = outboundOrderData,
                    Result = result
                };

                return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCompleteOutboundOrderFilter(JQueryDataTableParamModel param, string formData) {

            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            string result = "";
            List<dynamic> CompleteOutboundOrder = new List<dynamic>();

            try {

                string EventUser = this.LoggedOnUser;
                principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
                CompleteOutboundOrder = OutboundDataService.SearchCompleteOrdersDS(principalID, EventUser);//;
                result = "SUCCESS";
            }
            catch (Exception ex) {

                DefaultLogger.LogEvent(eventSource: this.ToString(),
                        message: ex.Message, level: RAM.Logging.LogLevel.ERROR, ex: ex,
                        context: "GetCompleteOutboundOrderFilter");
                result = string.Format("ERROR: {0}", ex.Message);
                principalID = 0;
            }

            //Set my data
            var data = new {
                sEcho = param.sEcho,
                iTotalRecords = CompleteOutboundOrder.Count,
                iTotalDisplayRecords = CompleteOutboundOrder.Count,
                aaData = CompleteOutboundOrder,
                Result = result
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOutboundOrderLineItems(WMSCustomerPortal.Business.Models.JQueryDataTableParamModel param, string formData)
        {
            JavaScriptSerializer js = new JavaScriptSerializer(new SimpleTypeResolver());
            OrderLineItemsModel olModel = js.Deserialize<OrderLineItemsModel>(formData);

            int principalID = olModel.OrderID;

            var outboundOrderLineItems = OutboundDataService.GetAllOrdersForPrincipal(principalID);

            var data = new
            {
                sEcho = param.sEcho,
                iTotalRecords = outboundOrderLineItems.Count,
                iTotalDisplayRecords = outboundOrderLineItems.Count,
                aaData = outboundOrderLineItems
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetsProducts(JQueryDataTableParamModel param, string formData)
        {

            int principalID = Int32.Parse(formData);

            var sessionData = ProductStaging.ReadAllList(principalID);
                               
            var data = new
            {
                sEcho = param.sEcho,
                iTotalRecords = sessionData.Count(),
                iTotalDisplayRecords = sessionData.Count(),
                aaData = sessionData
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveOrderSingleReceiver(
            string pCustomerID, 
            string pCustomerName,
            int pCustomerDetailID,
            string pInvoiceCustomerID,
            string pInvoiceCustomerName,
            int pInvoiceCustomerDetailID,
            string pCustomerOrderNumber,
            bool pSubmitted,
            string pTable)
         {


            string pEventTerminal = this.TerminalID;
            string pEventUser = this.LoggedOnUser;
            int pPrincipalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            DateTime pDateCreated = DateTime.Now;


             var result = OutboundDataService.AddOrderSingleReceiver(new Orders {
                 CustomerDetailID = pCustomerDetailID,
                 BillingCustomerDetailID = pInvoiceCustomerDetailID,
                 PrincipalID = pPrincipalID,
                 DateCreated = pDateCreated,
                 EventUser = pEventUser,
                 EventTerminal = pEventTerminal,
                 RAMCustomerID = pCustomerID,
                 ReceiverRAMCustomerID = pCustomerID,
                 BillingRAMCustomerID = pInvoiceCustomerID,
                 CustomerOrderNumber = pCustomerOrderNumber
            }, pEventTerminal, pEventUser, pSubmitted, pTable);

            var response = new {
                OrderID = result,
                Result = "SUCCESS"
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveMultipleReceiverOrder(string pCustomerOrderNumber, bool pSubmitted, string pReceiver, string pProduct) {

            string pEventTerminal = this.TerminalID;
            string pEventUser = this.LoggedOnUser;
            int pPrincipalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            DateTime pDateCreated = DateTime.Now;


            var result = OutboundDataService.AddOrderMultipleReceiver(new Orders {
                CustomerOrderNumber = pCustomerOrderNumber,
                PrincipalID = pPrincipalID,
                DateCreated = pDateCreated,
                EventUser = pEventUser,
                EventTerminal = pEventTerminal,
                RAMCustomerID = "",
            }, pEventTerminal, pEventUser, pSubmitted, pProduct, pReceiver);

            var response = new {
                OrderID = result,
                Result = "SUCCESS"
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveOrder(string pBillingCustomerID, string pCustomerID, string pCustomerName)
        {

            string pEventTerminal = this.TerminalID;
            string pEventUser = this.LoggedOnUser;
            int pPrincipalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

	        string pPriority = "";
	        bool pValueAddedPackaging = false;
            string pSalesPerson = "";
            string pSalesCategory = "";
            string pProcessor = "";
	        int pOrderDiscount = 0;
            int pOrderVAT = 0;
            DateTime pDateCreated = DateTime.Now;
            int pCustomerDetailID = 0;
            string pRAMOrderNumber = "";
            string pCustomerOrderNumber = "";
            string pStoreCode = "";
            string pCustomerGroupID = "";
            bool pSubmitted = false;

            var result = OutboundDataService.AddOrder(new Orders {
            CustomerDetailID = pCustomerDetailID,
            BillingRAMCustomerID = pBillingCustomerID,
            ReceiverRAMCustomerID  = pCustomerID,
            PrincipalID = pPrincipalID,
			RAMOrderNumber = pRAMOrderNumber,
            DateCreated = pDateCreated,
            CustomerOrderNumber = pCustomerOrderNumber,
			Priority = pPriority,
			ValueAddedPackaging = pValueAddedPackaging,
			SalesPerson = pSalesPerson,
			SalesCategory = pSalesCategory,
			Processor = pProcessor,
			OrderDiscount = pOrderDiscount,
			OrderVAT = pOrderVAT,
			EventUser = pEventUser,
			EventTerminal = pEventTerminal,
            RAMCustomerID = pCustomerID,
            StoreCode = pStoreCode,
            CustomerName = pCustomerName,
            CustomerGroupID = pCustomerGroupID
            }, pEventTerminal, pEventUser, pCustomerID, pSubmitted);

            var response = new
            {
                OrderID = result,
                Result = "SUCCESS"
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteOrder(string pOrderID)
        {
            int OrderID = int.Parse(pOrderID);
            string pEventTerminal = this.TerminalID;
            string pEventUser = this.LoggedOnUser;
            int pPrincipalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            string errorResult = "";

            var result = OutboundDataService.DeleteOrder(OrderID, pEventTerminal, pEventUser, out errorResult);
            var response = new
            {
                Result = result
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

       
       /// <summary>
       /// Submits a completed order to WMS for processing
       /// </summary>
       /// <param name="orderID"></param>
       /// <returns></returns>
       public JsonResult SubmitOrderWMS(int orderID)
        {

            string pEventTerminal = this.TerminalID;
            string pEventUser = this.LoggedOnUser;
            int pPrincipalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");


            var result = OutboundDataService.ProcessOrderToWMS(orderID, pEventTerminal, pEventUser);
            var response = new
            {
                Result = result
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

       public JsonResult CheckStockQuantity(string ProductCode, int Quantity) {
             
           int PrincipalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            string PrincipalCode = Session.GetDataFromSession<string>("WMSSession.PrincipalCode");
           string ResultOut = "";

            //var result = OutboundDataService.CheckStockQuantity(PrincipalID, ProductCode, Quantity, ResultOut);
            //var response = new {
            //    Result = result
            //};

           int AvQty = 0;
           var result = OutboundDataService.CheckStockQuantityNew(PrincipalID, PrincipalCode, ProductCode, Quantity, out AvQty);

           var response = new {
               Result = result
           };           

           return Json(response, JsonRequestBehavior.AllowGet);
       }


        #region Editable grid

        Dictionary<int, SaveOutboundLine> mOrderLineItems = null;

        public JsonResult UpdateOrderLine(int pOrderID, int pCustomerDetailID, string pCustomerID, string pTable, string viewSession) {

            string pEventTerminal = this.TerminalID;
            string pEventUser = this.LoggedOnUser;
            int pPrincipalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            var result = OutboundDataService.UpdateOrderLineItem(pOrderID, pCustomerDetailID, pCustomerID, pPrincipalID, pEventUser, pEventTerminal, pTable);

            return Json(new {
                Result = "OK",
                Data = result,
            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DeleteOrderLine(int pLineNumber, int pLineItemID, string viewSession) {

            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            string pEventTerminal = this.TerminalID;
            string pEventUser = this.LoggedOnUser;

            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveOutboundLine>>(viewSession);

            if (mOrderLineItems.ContainsKey(pLineNumber)) {
                mOrderLineItems.Remove(pLineNumber);
                if (pLineItemID != 0) {
                    var result = OrderLineItems.Delete(pLineItemID, pEventUser, pEventTerminal);
                }

            }

            List<SaveOutboundLine> lineItems = this.ReorderLineItems(mOrderLineItems, viewSession);

            return Json(new {
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult EditableGrid_GetLineItems(string viewSession) {

            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveOutboundLine>>(viewSession);

            if (mOrderLineItems == null) {
                mOrderLineItems = new Dictionary<int, SaveOutboundLine>();
            }

            List<SaveOutboundLine> lineItems = this.ReorderLineItems(mOrderLineItems, viewSession);

            return Json(new {
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditableGrid_AddLineItem_Pre(SaveOutboundLine lineItem, string viewSession) {
           
            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveOutboundLine>>(viewSession);


            if (mOrderLineItems == null) {
                mOrderLineItems = new Dictionary<int, SaveOutboundLine>();
            }

            if (!mOrderLineItems.ContainsKey(lineItem.LineNumber)) {
                mOrderLineItems.Add(lineItem.LineNumber, lineItem);
            }
            else {
                foreach (var item in mOrderLineItems) {
                    SaveOutboundLine request = (SaveOutboundLine)item.Value;
                    if (request.ProductStagingID == lineItem.ProductStagingID) {
                        return Json(new {
                            Result = "ERROR: Product already added"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            List<SaveOutboundLine> lineItems = this.ReorderLineItems(mOrderLineItems, viewSession);

            mOrderLineItems.Clear();

            return Json(new {
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditableGrid_RemoveLineItem(int lineNumber, string viewSession) {

            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveOutboundLine>>(viewSession);

            if (mOrderLineItems.ContainsKey(lineNumber)) {
                mOrderLineItems.Remove(lineNumber);
            }

            List<SaveOutboundLine> lineItems = this.ReorderLineItems(mOrderLineItems, viewSession);

            return Json(new {
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ClearLineItem(string viewSession) {

            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveOutboundLine>>(viewSession);

            mOrderLineItems = new Dictionary<int, SaveOutboundLine>();;

            List<SaveOutboundLine> lineItems = this.ReorderLineItems(mOrderLineItems, viewSession);

            return Json(new {
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditableGrid_AddLineItem(SaveOutboundLine lineItem, string viewSession) {

            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveOutboundLine>>(viewSession);


            if (mOrderLineItems == null) {
                mOrderLineItems = new Dictionary<int, SaveOutboundLine>();
            }

            if (!mOrderLineItems.ContainsKey(lineItem.LineNumber)) {
                mOrderLineItems.Add(lineItem.LineNumber, lineItem);
            }
            else {
                foreach (var item in mOrderLineItems) {
                    SaveOutboundLine request = (SaveOutboundLine)item.Value;
                    if (request.ProductStagingID == lineItem.ProductStagingID) {
                        return Json(new {
                            Result = "ERROR: Product already added"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            List<SaveOutboundLine> lineItems = this.ReorderLineItems(mOrderLineItems, viewSession);

            return Json(new {
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditableGrid_PrepopulateLineItems(int OrderID, string viewSession) {

           var result = OrderLineItems.ReadAll_List(OrderID);

          try {

              Session.SetDataToSession<Dictionary<int, SaveOutboundLine>>(viewSession, mOrderLineItems);
             
           if (mOrderLineItems == null) {
                mOrderLineItems = new Dictionary<int, SaveOutboundLine>();
            }

           var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
           var json = serializer.Serialize(result);
           var c = serializer.Deserialize<List<OrderLineItems>>(json);
           SaveOutboundLine saveibl = new SaveOutboundLine();
       
           for (int i = 0; i < c.Count; i++) 
	         {

               saveibl.LineNumber = int.Parse(c[i].LineNumber);
               saveibl.LineItemID = c[i].LineItemID;
               saveibl.ProductStagingID = c[i].ProductStagingID;
               saveibl.ProductCode = c[i].ProductCode;
               saveibl.EANCode = c[i].EANCode;
               saveibl.ShortDescription = c[i].ShortDescription;
               saveibl.LongDescription = c[i].LongDescription;
               saveibl.UnitCost = Convert.ToDouble(c[i].SubmittedUnitCost); 
               saveibl.Quantity = Convert.ToInt32(c[i].Quantity);

               if (!mOrderLineItems.ContainsKey(int.Parse(c[i].LineNumber)))
               {
                   mOrderLineItems.Add(int.Parse(c[i].LineNumber), saveibl);
               }
        
             }

           Session.SetDataToSession<Dictionary<int, SaveOutboundLine>>(viewSession, mOrderLineItems);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return Json(new
            {
                Result = "OK",
                Data = mOrderLineItems
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditableGrid_UpdateLineItem(SaveOutboundLine lineItem, string viewSession) {

            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveOutboundLine>>(viewSession);

            if (mOrderLineItems.ContainsKey(lineItem.LineNumber)) {
                SaveOutboundLine request = (SaveOutboundLine)mOrderLineItems[lineItem.LineNumber];
                request.Quantity = lineItem.Quantity;

                mOrderLineItems[lineItem.LineNumber] = lineItem;
            }

            List<SaveOutboundLine> lineItems = this.ReorderLineItems(mOrderLineItems, viewSession);

            return Json(new {
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditableGrid_SaveLineItems(string viewSession) {

            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveOutboundLine>>(viewSession);

            if (mOrderLineItems == null || mOrderLineItems.Count == 0) {
                return Json(new {
                    Result = "ERROR: No products have been added"
                }, JsonRequestBehavior.AllowGet);
            }

            // TODO: Save Line Items


            //mOrderLineItems.Clear();

            List<SaveOutboundLine> lineItems = mOrderLineItems.Values.ToList<SaveOutboundLine>();

            // Clear Session
            Session.SetDataToSession<string>(viewSession, mOrderLineItems);

            return Json(new {
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }

        private List<SaveOutboundLine> ReorderLineItems(Dictionary<int, SaveOutboundLine> dictLineItems, string viewSession) {

            Dictionary<int, SaveOutboundLine> newDictLineItems = new Dictionary<int, SaveOutboundLine>();

            int index = 0;
            foreach (var item in dictLineItems) {
                SaveOutboundLine request = (SaveOutboundLine)item.Value;
                request.LineNumber = index + 1;

                newDictLineItems.Add(request.LineNumber, request);
                index++;
            }

            List<SaveOutboundLine> lineItems = newDictLineItems.Values.ToList<SaveOutboundLine>();

            Session.SetDataToSession<string>(viewSession, newDictLineItems);

            return lineItems;
        }

        [AllowAnonymous]
        public JsonResult EditableGrid_ProductLookup(string startsWith, string searchType) {

            int principalId =  Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            var data = CommonDataService.Product_Lookup(principalId, startsWith, searchType);

            return Json(data);
        }

        public JsonResult DoesCustomerOrderNumberExist(string txtCustomerOrderNumber) {

            bool retVal = false;
            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            retVal = OutboundDataService.DoesCustomerOrderNumberExist(txtCustomerOrderNumber, principalId);

            var response = Json(retVal, JsonRequestBehavior.AllowGet);
            return response;
        }

        public JsonResult Customer_Lookup(string startsWith, string searchType) {

            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            var data = CommonDataService.Customer_Lookup(principalId, startsWith, searchType);

            return Json(data);
        }
        #endregion

    }

   public struct SaveOutboundLine {
       public int LineNumber { get; set; }
       public int ProductStagingID { get; set; }
       public string ProductCode { get; set; }
       public string EANCode { get; set; }
       public string ShortDescription { get; set; }
       public string LongDescription { get; set; }
       public double UnitCost { get; set; }
       public int Quantity { get; set; }
       public int LineItemID { get; set; }
       public int OrderID { get; set; }
   }

}