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
using System.Collections;

namespace WMSCustomerPortal.Web.Controllers
{

    public class ReturnsController : BaseController
    {

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

        WMSCustomerPortal.Business.ReturnService _returnService;
        private WMSCustomerPortal.Business.ReturnService DataService
        {
            get
            {
                if (_returnService == null)
                {
                    _returnService = new WMSCustomerPortal.Business.ReturnService();
                }
                return _returnService;
            }
        }

        #region Returns
        #region Returns

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        public ActionResult AddReturn()
        {
            return View();
        }

        public JsonResult ReturnOrderNumberAutoComplete(string startsWith)
        {
            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            List<ReturnOrdersLookUpItem> data = new List<ReturnOrdersLookUpItem>();

            string result = "";
            try
            {
                string EventUser = this.LoggedOnUser;
                data = SqlManager.ReturnOrderNumber_Lookup(startsWith.Trim(), principalId, EventUser);
                result = "SUCCESS";
            }
            catch (Exception ex)
            {
                // Logging
                DefaultLogger.LogEvent(eventSource: this.ToString(),
                        message: ex.Message, level: RAM.Logging.LogLevel.ERROR, ex: ex,
                        context: "ReturnOrderNumberAutoComplete");
                result = string.Format("ERROR: {0}", ex.Message);
            }
            return Json(data);
        }

        public JsonResult SaveNewReturnOrder(string pReturnOrderNumber, string pReturnType, string pOrderLineItems, string pViewSession, string pReturnPrincipalID)
        {
            string pEventTerminal = this.TerminalID;
            string pEventUser = this.LoggedOnUser;

            int pPrincipalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            if (!string.IsNullOrEmpty(pReturnPrincipalID))
            {
                if(!int.TryParse(pReturnPrincipalID, out pPrincipalID))
                {
                    pPrincipalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
                }
            }

            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveReturnsLine>>(pViewSession);

            mOrderLineItems = new Dictionary<int, SaveReturnsLine>();

            List<SaveReturnsLine> lineItems = this.ReorderLineItemsWithSession(mOrderLineItems, pViewSession);

            Session.SetDataToSession<string>(pViewSession, mOrderLineItems);

            var result = DataService.SaveNewReturnOrder(new Returns
            {
                PrincipalID = pPrincipalID,
                EventTerminal = pEventTerminal,
                RAMOrderNumber = pReturnOrderNumber,
                ReturnType = pReturnType,
                EventUser = pEventUser,
            }, pEventTerminal, pEventUser, pOrderLineItems);

            return Json(new
            {
                Result = "OK",
                Data = result,
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateReturnReceiptLine(List<LineItem> lineItems)
        {
            string pEventTerminal = this.TerminalID;
            string pEventUser = this.LoggedOnUser;
            var result = 0;

            foreach (var lines in lineItems)
            {
                int lineItemID = lines.LineItemID;
                int quantity = lines.Quantity;
                var returnWaybill = lines.ReturnWaybill;

                result = DataService.UpdateReturnReceiptLine(lineItemID, quantity, returnWaybill, pEventUser, pEventTerminal);
               
            }
            return Json(new
            {
                Result = "OK",
                Data = result,
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Editable grid

        Dictionary<int, SaveReturnsLine> mOrderLineItems = null;

        private List<SaveReturnsLine> ReorderLineItemsWithSession(Dictionary<int, SaveReturnsLine> dictLineItems, string viewSession)
        {

            Dictionary<int, SaveReturnsLine> newDictLineItems = new Dictionary<int, SaveReturnsLine>();

            int index = 0;

            var list = dictLineItems.Keys.ToList();
            list.Sort();

            List<SaveReturnsLine> lineItems = new List<SaveReturnsLine>();
            foreach (var key in list)
            {
                SaveReturnsLine request = (SaveReturnsLine)dictLineItems[key];
                request.LineNumber = index + 1;
                lineItems.Add(request);
                newDictLineItems.Add(key, request);
                index++;
            }

            Session.SetDataToSession<string>(viewSession, newDictLineItems);

            return lineItems;
        }

        public JsonResult EditableGrid_PrepopulateLineItems(int OrderID, string ReturnType)
        {
            var result = OrderLineItems.ReturnReadAll_List(OrderID);
            try
            {
                Session.SetDataToSession<Dictionary<int, SaveReturnsLine>>("WMSSession.TempReturnOrder", mOrderLineItems);

                if (mOrderLineItems == null)
                {
                    mOrderLineItems = new Dictionary<int, SaveReturnsLine>();
                }

                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(result);
                var c = serializer.Deserialize<List<OrderLineItems>>(json);
                SaveReturnsLine saveibl = new SaveReturnsLine();

                for (int i = 0; i < c.Count; i++)
                {

                    saveibl.LineNumber = int.Parse(c[i].LineNumber);
                    saveibl.LineItemID = c[i].LineItemID;
                    saveibl.ProductStagingID = c[i].ProductStagingID;
                    saveibl.ProductCode = c[i].ProductCode;
                    saveibl.EANCode = c[i].EANCode;
                    saveibl.ShortDescription = c[i].ShortDescription;
                    saveibl.LongDescription = c[i].LongDescription;
                    saveibl.Quantity = Convert.ToInt32(c[i].Quantity);

                    if (ReturnType == "Part Order")
                    {
                        saveibl.OldQuantity = 0;
                    }
                    if (ReturnType == "Full Order")
                    {
                        saveibl.OldQuantity = Convert.ToInt32(c[i].Quantity);
                    }

                    if (!mOrderLineItems.ContainsKey(int.Parse(c[i].LineNumber)))
                    {
                        mOrderLineItems.Add(int.Parse(c[i].LineNumber), saveibl);
                    }

                }

                Session.SetDataToSession<Dictionary<int, SaveReturnsLine>>("WMSSession.TempReturnOrder", mOrderLineItems);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new
            {
                Result = "OK",
                Data = mOrderLineItems
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ClearLineItem(string viewSession)
        {

            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveReturnsLine>>(viewSession);

            mOrderLineItems = new Dictionary<int, SaveReturnsLine>(); ;

            List<SaveReturnsLine> lineItems = this.ReorderLineItemsWithSession(mOrderLineItems, viewSession);

            return Json(new
            {
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Returns

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        public ActionResult ReturnReceipt()
        {
            return View();
        }

        [Authorize]
        public ActionResult ReturnReceiptList()
        {
            //pass through a model for the new page
            //var returnReceiptModel = new InboundMasterItem { InboundMasterID = inboundMasterID };
            return View();
        }

        public JsonResult GetGroupPrincipals(string Data)
        {
            int PrincipalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            var result = DataService.GetPrincipalsByGroup(PrincipalID);

            return Json(new
            {
                Result = "OK",
                Data = result
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReturnReceiptList(JQueryDataTableParamModel param, string filter)
        {
            int PrincipalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            var result = DataService.GetReturnReceiptList(PrincipalID);

            var data = new
            {
                sEcho = param.sEcho,
                iTotalRecords = result.Count,
                iTotalDisplayRecords = result.Count,
                aaData = result
            };

            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;
        }

        public ActionResult GetReturnReceiptLineItems(string RAMOrderNumber)
        {
            var result = DataService.GetReturnReceiptLineItems(RAMOrderNumber);

            var data = new
            {
                Data = result,
                Result = "SUCCESS"
            };

            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;
        }

        public ActionResult ReturnReceiptLineItems(JQueryDataTableParamModel param, string filter)
        {
            int PrincipalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            var result = DataService.GetReturnReceiptLineItems_All(PrincipalID);

            //var result = DataService.GetReturnReceiptLineItems_by_IGD(193);
            var data = new
            {
                sEcho = param.sEcho,
                iTotalRecords = result.Count,
                iTotalDisplayRecords = result.Count,
                aaData = result
            };

            ArrayList arryList1 = new ArrayList();
            foreach (var x in data.aaData)
            {
                SaveReturnsLine1 lines = new SaveReturnsLine1();

                int IGDStagingID = x.IGDStagingID;
                String moveRef = x.RAMOrderNumber;
                bool SubmittedToWMS = x.SubmittedToWMS;
                int ReceivedInWHQuantity = x.ReceivedInWHQuantity;

                lines.IGDStagingID = IGDStagingID;
                lines.MoveRef = moveRef;
                lines.submitted = SubmittedToWMS;
                lines.ReceivedQuantity = ReceivedInWHQuantity;

            
               arryList1.Add(lines);
            }
            var response = Json(arryList1, JsonRequestBehavior.AllowGet);
            return response;
        }

        public ActionResult GetReturnReceiptLineItems_All(JQueryDataTableParamModel param, string filter)
        {
            int PrincipalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            var result= DataService.GetReturnReceiptLineItems_All(PrincipalID);
              
            if (result !=null)
            {
                var data = new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = result.Count,
                    iTotalDisplayRecords = result.Count,
                    aaData = result
                };

                var response = Json(data, JsonRequestBehavior.AllowGet);


                return response;
            }
            else
            {
         

                var data = new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = 0,
                    iTotalDisplayRecords =0,
                    aaData = 0
                };
                var response=Json(data, JsonRequestBehavior.AllowGet);
                return response; 
            }
           
        }

        #endregion
    }

    public struct SaveReturnsLine1
    {
        public int IGDStagingID { get; set; }
        public String MoveRef { get; set; }
        public bool submitted { get; set; }
        public int ReceivedQuantity { get; set; }
    }
        public struct SaveReturnsLine
    {
        public int LineNumber { get; set; }
        public int ProductStagingID { get; set; }
        public string ProductCode { get; set; }
        public string EANCode { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public int OldQuantity { get; set; }
        public int Quantity { get; set; }
        public int LineItemID { get; set; }
        public int OrderID { get; set; }
    }

    public struct UpdateReturnReceiptLineItem
    {
        public string ProductCode { get; set; }
        public string EANCode { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public int OldQuantity { get; set; }
        public int Quantity { get; set; }
        public int LineItemID { get; set; }
    }

    public class LineItem {
        public int LineItemID { get; set; }
        public int Quantity { get; set; }
        public string ReturnWaybill { get; set; }
    }
}
#endregion