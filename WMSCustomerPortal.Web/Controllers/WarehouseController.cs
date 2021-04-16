using System;
using System.Web.Mvc;
using System.Collections;
using System.Collections.Generic;
using WMSCustomerPortal.Business;
using WMSCustomerPortal.Business.Models;
using WMSCustomerPortal.Models.Common;
using WMSCustomerPortal.Models;
using WMSCustomerPortal.Models.Entities;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using RAM.Utilities.Common;


namespace WMSCustomerPortal.Web.Controllers {
    [Authorize(Roles = "Backdoor, Warehouse")]
    public class WarehouseController : BaseController {

        WMSCustomerPortal.Business.WarehouseService _service;

        private WMSCustomerPortal.Business.WarehouseService DataService {
            get {
                if (_service == null) {
                    _service = new WMSCustomerPortal.Business.WarehouseService();
                }
                return _service;
            }
        }

        /// <summary>
        /// Return the default warehouse list view 
        /// </summary>
        /// <returns></returns>
        public ActionResult WarehouseList() {
            // return View("WarehouseList");
            return View();
        }
        public ActionResult ReturnReceipt()
        {
            return View();
        }

        public ActionResult ReturnReceiptList(string ramOrderNumber)
        {
            var returnReceiptList = new ReturnMasterItem { RAMOrderNumber = ramOrderNumber };
            return View(returnReceiptList);
        }
        public ActionResult ReturnIGD()
        {           
            return View();
        }

        /// <summary>
        /// Returns alist of all the items for the warehouse 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public JsonResult GetWarehouseList(JQueryDataTableParamModel param, string filter) {
            int principalID = 0;
            try {
                principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            }
            catch {
                principalID = 0;
            }

            //TODO: Dates that are to be filtered
            System.DateTime dtFrom = new DateTime();
            System.DateTime dtTo = new DateTime();
            dtFrom = DateTime.Now.AddYears(-2);
            dtTo = DateTime.Now;

            //TODO: Purcase Order Reference
            string poRef = "";

            var sessionData = DataService.GetAllInboundMasterFilter(principalID, poRef, dtFrom, dtTo); //.GetAllCustomers(principalID);

            var data = new {
                sEcho = param.sEcho,
                iTotalRecords = sessionData.Count,
                iTotalDisplayRecords = sessionData.Count,
                aaData = sessionData
            };

            // Test serialization using overriden method
            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;
        }

        /// <summary>
        /// Return a dynamic list of inbound line items according to the master id specified
        /// </summary>
        /// <param name="param"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public JsonResult GetWarehouseLineItemsList(JQueryDataTableParamModel param, int masterId) {

            var sessionData = DataService.GetInboundLineItemsList(masterId);

            var data = new {
                sEcho = param.sEcho,
                iTotalRecords = sessionData.Count,
                iTotalDisplayRecords = sessionData.Count,
                aaData = sessionData,
                Result = "SUCCESS"
            };

            // Test serialization using overriden method
            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;
        }

        /// <summary>
        /// Return a dynamic list of inbound line items according to the master id specified
        /// </summary>
        /// <param name="param"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public JsonResult GetWarehouseIGDList(JQueryDataTableParamModel param, int lineItemID) {

            var sessionData = DataService.GetIGDStagingList(lineItemID);

            var data = new {
                sEcho = param.sEcho,
                iTotalRecords = sessionData.Count,
                iTotalDisplayRecords = sessionData.Count,
                aaData = sessionData
            };

            // Test serialization using overriden method
            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;
        }

        /// <summary>
        /// Deletes an igd line item
        /// </summary>
        /// <param name="inboundMasterLineItemID"></param>
        /// <returns></returns>
        public JsonResult DeleteIGD(int igdStagingID) {

            string eventTerminal = this.TerminalID;
            string eventUser = this.LoggedOnUser;

            //we would like to delete this record 

            bool success = DataService.DeleteIGDStaging(igdStagingID, eventTerminal, eventUser);


            if (success) {
                var response = new {
                    SaveResponse = new BaseSaveResponse { Details = "Record Deleted." },
                    Result = "SUCCESS"
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else {
                var response = new {
                    SaveResponse = new BaseSaveResponse { Details = "Record Not Deleted." },
                    Result = "FAILURE"
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Submits all the igd records which have been created to WMS for a specified lineItemID 
        /// </summary>
        /// <param name="lineItemIDForSubmission"></param>
        /// <returns></returns>
        public JsonResult SubmitSingleToWMS(int InboundMasterLineItemID) {
            string eventTerminal = this.TerminalID;
            string eventUser = this.LoggedOnUser;

            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
           

           //bool success = DataService.SubmitAllIGDStagingLineItem(InboundMasterLineItemID, eventTerminal, eventUser, principalID);

           bool success = DataService.SubmitSingleIGDStagingLineItem(InboundMasterLineItemID, eventTerminal, eventUser, principalID);

            if (success) {
                var response = new {
                    SaveResponse = new BaseSaveResponse { Details = "Records Submitted." },
                    Result = "SUCCESS"
                };

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else {
                var response = new {
                    SaveResponse = new BaseSaveResponse { Details = "Records Not All Submitted." },
                    Result = "FAILURE"
                };

                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult CommitIGDItems(int inboundMasterLineID) //pass through the master id
        {
            bool retVal = true;

            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            if (mIGDLineItems == null)
            {
                mIGDLineItems = new Dictionary<int, SaveIGDLine>();
            }

            try
            {
                mIGDLineItems = Session.GetDataFromSession<Dictionary<int, SaveIGDLine>>("WMSSession.TempIGDLine");
            }
            catch (Exception ex)
            {
                // we cannot get to the dictionary ... do nothing because we dont have any lines possibly
            }
            List<SaveIGDLine> lineItems = this.ReorderLineItems(mIGDLineItems);
            //loop through each of the line items and construct the lineitems and save
            foreach (SaveIGDLine lineitem in mIGDLineItems.Values)
            {
                IGDStaging igdLi = new IGDStaging();

               igdLi.IGDStagingID = lineitem.IGDStagingID;
               if ((lineitem.InboundMasterLineItemID == 0) || (lineitem.InboundMasterLineItemID == null))
               {
                   igdLi.InboundMasterLineItemID = inboundMasterLineID;
               }
               else
               {
                   igdLi.InboundMasterLineItemID = lineitem.InboundMasterLineItemID;
               }
               //igdLi.InboundMasterLineItemID = lineitem.InboundMasterLineItemID;
               igdLi.MoveRef = lineitem.MoveRef;
               igdLi.ReceivedQuantity = lineitem.ReceivedQuantity;
               igdLi.SubmittedToWMS = lineitem.SubmittedToWMS;
                //ok ... now we save 
               //int saveRecord = SaveIGDItem(igdLi, principalID);
               int saveRecord = SavePartialIGDItem(igdLi, principalID);
                if (saveRecord < 1)
                {
                    //there is an issue ...
                    retVal = false;

                   
                  
                }
                else
                {
                    retVal = true;
                }
            }

            //lets reload the session variables with the new values as saved in the database
            
            //after a sve etc ... lets get the values correct from the db

                PrepopulateSessionItems(inboundMasterLineID);

            if (retVal)
            {
                var response = new
                {
                    SaveResponse = new BaseSaveResponse { Details = "Records Commited." },
                    Result = "SUCCESS"
                };

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var response = new
                {
                    SaveResponse = new BaseSaveResponse { Details = "Records Not All Commited." },
                    Result = "FAILURE"
                };

                return Json(response, JsonRequestBehavior.AllowGet);
            }


        }

        /// <summary>
        /// call to save the inbound master line item
        /// </summary>
        /// <param name="inboundMasterLineItem"></param>
        /// <returns></returns>
        private int SaveIGDItem(IGDStaging igdItem, int principalID)
        {


            string eventTerminal = this.TerminalID;
            string eventUser = this.LoggedOnUser;

            //we would like to save this record somewhere
            int recordsAffected = 0;
            if ((igdItem.IGDStagingID < 1))
            {
                recordsAffected = DataService.AddIGDStaging(igdItem, eventUser, eventTerminal, principalID);

            }
            else
            {
                recordsAffected = DataService.UpdateIGDStaging(igdItem, eventUser, eventTerminal);
            }

            return recordsAffected;
        }





        /// <summary>
        /// call to save the inbound igd line item
        /// </summary>
        /// <param name="inboundMasterLineItem"></param>
        /// <returns></returns>
        private int SavePartialIGDItem(IGDStaging igdItem, int principalID)
        {


            string eventTerminal = this.TerminalID;
            string eventUser = this.LoggedOnUser;

            //we would like to save this record somewhere
            int recordsAffected = 0;
            if ((igdItem.IGDStagingID < 1))
            {
                recordsAffected = DataService.AddIGDStaging(igdItem, eventUser, eventTerminal, principalID);

            }
            else
            {
                recordsAffected = DataService.UpdatePartialIGDStaging(igdItem, eventUser, eventTerminal);
            }

            return recordsAffected;
        }



        /// <summary>
        /// Submits all the igd records which have been created to WMS for a specified lineItemID 
        /// </summary>
        /// <param name="lineItemIDForSubmission"></param>
        /// <returns></returns>
        public JsonResult SubmitAllIGDLineItemsToWMS( int inboundMasterLineItemID)
        {
           
            //ensure that all the lineitems in the list is saved 
            CommitIGDItems(inboundMasterLineItemID);
            
            string eventTerminal = this.TerminalID;
            string eventUser = this.LoggedOnUser;

            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            //get a list of all the igd records for this row, then we will loop through and save
            List<dynamic> lstdynamicIGDStaging = new List<dynamic>();
          
            lstdynamicIGDStaging = DataService.GetIGDStagingList(inboundMasterLineItemID); 
            
              bool success = false;
            //our dirty trick of serializattion / deserialization
             try {
                //we need to convert this dynamic list to a InboundMasterLineItem list
                //quick and dirty ... lets serialize and then deserialize

                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(lstdynamicIGDStaging);
            var lstIGDStaging = serializer.Deserialize<List<IGDStaging>>(json); 

                foreach (IGDStaging stage in lstIGDStaging)
                {
                    success = DataService.SubmitSingleIGDStagingLineItem(stage.IGDStagingID, eventTerminal, eventUser, principalID);

                }

            }
            catch(Exception ex)
             {
               success = false;
            }

             PrepopulateSessionItems(inboundMasterLineItemID);


            if (success)
            {
                var response = new
                {
                    SaveResponse = new BaseSaveResponse { Details = "Records Submitted." },
                    Result = "SUCCESS"
                };

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var response = new
                {
                    SaveResponse = new BaseSaveResponse { Details = "Records Not All Submitted." },
                    Result = "FAILURE"
                };

                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// Checks whether the quantity exceeds the valid number
        /// </summary>
        /// <param name="igdReceivedQuantity"></param>
        /// <returns></returns>
        public bool IsReceivedQuantityValid(int igdReceivedQuantity, int inboundMasterLineItemID)
        {
           
            bool isReceivedQuantityValid = false;//= DataService.DoesProductExist(txtProductCode, principalId);

           
            //ok .. lets get the count from the database and compare
            //DataService.IsReceivedQuantityValid(Functions.SafeInt(txtIGDReceivedQuantity, 999999), Functions.SafeInt(inboundLineItemID,0));

            //lets get the counts of the currently stored items in the dictionary as a temp measure
            if (mIGDLineItems == null)
            {
                mIGDLineItems = new Dictionary<int, SaveIGDLine>();
            }


            try
            {
                mIGDLineItems = Session.GetDataFromSession<Dictionary<int, SaveIGDLine>>("WMSSession.TempIGDLine");
            }
            catch
            {
                //do nothing ... because the dict may be empty
            }

            WMSCustomerPortal.Models.Entities.InboundMasterLineItem ibmLi = new InboundMasterLineItem();
            int expectedQuantity = 0;
            try
            {
                //get a inbound master id record
                ibmLi = DataService.GetInboundMasterLineItem(inboundMasterLineItemID);

                expectedQuantity = ibmLi.ExpectedQuantity;
            }
            catch(Exception ex)
            {

                throw ex;
            }

            List<SaveIGDLine> lineItems = this.ReorderLineItems(mIGDLineItems);

            //ok ... loop through the list and add the numbers
            int totalCount = 0;
            foreach(SaveIGDLine line in lineItems)
            {
                totalCount = totalCount + line.ReceivedQuantity;

            }

            if ((totalCount + igdReceivedQuantity) > expectedQuantity)
            {
                isReceivedQuantityValid = false;

            }
            else
            {

                isReceivedQuantityValid = true;
            }

            return isReceivedQuantityValid;
           
        }


        /// <summary>
        /// Save an iGD staging record
        /// </summary>
        /// <param name="stagingRecord"></param>
        /// <returns></returns>
        public JsonResult SaveIGDStaging(IGDStaging igdRecord) {

            //string eventUser = "";
            string eventTerminal = this.TerminalID;
            string eventUser = this.LoggedOnUser;

            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            //we would like to save this record somewhere

            igdRecord.PrincipalID = principalID;

            if ((igdRecord.IGDStagingID < 1)) {
                int retVal = DataService.AddIGDStaging(igdRecord, eventUser, eventTerminal, principalID);

            }
            else {
                int retVal = DataService.UpdateIGDStaging(igdRecord, eventUser, eventTerminal);
            }

            var result = new BaseSaveResponse {
              
                Details = "OK"
            };

            var response = new {
                SaveResponse = result,
                Result = "SUCCESS"
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Save an iGD staging record
        /// </summary>
        /// <param name="stagingRecord"></param>
        /// <returns></returns>
        public IGDStaging SaveIGDStagingRecord(IGDStaging partialIGDRecord)
        {
            IGDStaging retVal = new IGDStaging();
            //string eventUser = "";
            string eventTerminal = this.TerminalID;
            string eventUser = this.LoggedOnUser;

            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            //we would like to save this record somewhere

            partialIGDRecord.PrincipalID = principalID;

            if ((partialIGDRecord.IGDStagingID < 1))
            {
                retVal = DataService.AddAndRetrieveIGDStaging(partialIGDRecord, eventUser, eventTerminal, principalID);
            }
            else
            {
                //lets find the updated staging record


                retVal = DataService.UpdateAndRetrieveIGDStaging(partialIGDRecord, eventUser, eventTerminal);
            }

            return retVal;
        }


        /// <summary>
        /// Clears all the temp line items
        /// </summary>
        /// <returns></returns>
        public JsonResult EditableGrid_ClearAllLineItems()
        {
            //mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveInboundLine>>("WMSSession.TempInboundOrder");

            if (mIGDLineItems == null)
            {
                mIGDLineItems = new Dictionary<int, SaveIGDLine>();
            }

            //Just clear the values
            Session.SetDataToSession<string>("WMSSession.TempIGDLine", mIGDLineItems);
            // List<SaveInboundLine> lineItems = this.ReorderLineItems(mOrderLineItems);

            return Json(new
            {
                Result = "OK"

            }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Prepopulates the grid dictionary with the values 
        /// </summary>
        /// <param name="inboundMasterID"></param>
        /// <returns></returns>
        public JsonResult EditableGrid_PrepopulateLineItems(int inboundMasterLineitemID)
        {
            //load all the inbound master line items for the master id specified

            //List<InboundMasterLineItem> sessionData = new List<InboundMasterLineItem>();
            // List<dynamic> sessionData = new List<dynamic>();
            var sessionData = DataService.GetIGDStagingList(inboundMasterLineitemID);

            if (mIGDLineItems == null)
            {
                mIGDLineItems = new Dictionary<int, SaveIGDLine>();
            }

            //clear the session data first with the line items
            mIGDLineItems.Clear();

            Session.SetDataToSession<Dictionary<int, SaveIGDLine>>("WMSSession.TempIGDLine", mIGDLineItems);
            int lineNumber = 1;

            try
            {
                //we need to convert this dynamic list to a InboundMasterLineItem list
                //quick and dirty ... lets serialize and then deserialize
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(sessionData);
                var c = serializer.Deserialize<List<IGDStaging>>(json);



                foreach (WMSCustomerPortal.Models.Entities.IGDStaging ibl in c)
                {
                    SaveIGDLine saveibl = new SaveIGDLine();

                    saveibl.LineNumber = lineNumber;
                    saveibl.IGDStagingID = Functions.SafeInt(ibl.IGDStagingID);
                   
                    saveibl.MoveRef = Functions.SafeString(ibl.MoveRef, "");
                    //saveibl.PORef = Functions.SafeString(ibl.ShortDesc, "");
                    saveibl.ReceivedQuantity = Functions.SafeInt(ibl.ReceivedQuantity, 0);
                    saveibl.SubmittedToWMS = Functions.SafeBool(ibl.SubmittedToWMS);
                    lineNumber++;
                    //add this value to the dictionary
                    if (!mIGDLineItems.ContainsKey(lineNumber))
                    {
                        mIGDLineItems.Add(lineNumber, saveibl);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            //now save this to the session variable

            Session.SetDataToSession<Dictionary<int, SaveIGDLine>>("WMSSession.TempIGDLine", mIGDLineItems);

            return Json(new
            {
                Result = "OK"

            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// loads the values back into the session
        /// 
        /// </summary>
        /// <param name="inboundMasterLineitemID"></param>
        /// <returns></returns>
        private bool PrepopulateSessionItems(int inboundMasterLineitemID)
        {
            //load all the inbound master line items for the master id specified

            //List<InboundMasterLineItem> sessionData = new List<InboundMasterLineItem>();
            // List<dynamic> sessionData = new List<dynamic>();
            var sessionData = DataService.GetIGDStagingList(inboundMasterLineitemID);

            if (mIGDLineItems == null)
            {
                mIGDLineItems = new Dictionary<int, SaveIGDLine>();
            }

            //clear the session data first with the line items
            mIGDLineItems.Clear();

            Session.SetDataToSession<Dictionary<int, SaveIGDLine>>("WMSSession.TempIGDLine", mIGDLineItems);
            int lineNumber = 1;

            try
            {
                //we need to convert this dynamic list to a InboundMasterLineItem list
                //quick and dirty ... lets serialize and then deserialize
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(sessionData);
                var c = serializer.Deserialize<List<IGDStaging>>(json);



                foreach (WMSCustomerPortal.Models.Entities.IGDStaging ibl in c)
                {
                    SaveIGDLine saveibl = new SaveIGDLine();

                    saveibl.LineNumber = lineNumber;
                    saveibl.IGDStagingID = Functions.SafeInt(ibl.IGDStagingID);

                    saveibl.MoveRef = Functions.SafeString(ibl.MoveRef, "");
                    //saveibl.PORef = Functions.SafeString(ibl.ShortDesc, "");
                    saveibl.ReceivedQuantity = Functions.SafeInt(ibl.ReceivedQuantity, 0);
                    saveibl.SubmittedToWMS = Functions.SafeBool(ibl.SubmittedToWMS);
                    lineNumber++;
                    //add this value to the dictionary
                    if (!mIGDLineItems.ContainsKey(lineNumber))
                    {
                        mIGDLineItems.Add(lineNumber, saveibl);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            //now save this to the session variable

            Session.SetDataToSession<Dictionary<int, SaveIGDLine>>("WMSSession.TempIGDLine", mIGDLineItems);

            return true;
        }



        //checks to see whether the item is deleteable
        private bool IsIGDLineItemDeleatable(SaveIGDLine inboundlineItem)
        {
            bool retVal = true;
            //TODo: check to see if deleteable
           // retVal = InboundService.CheckDeletable(inboundlineItem.IGDStagingID);
            retVal = DataService.CheckIGDDeletable(inboundlineItem.IGDStagingID);
            return retVal;
        }

        #region Editable grid

        Dictionary<int, SaveIGDLine> mIGDLineItems = null;



        public JsonResult EditableGrid_GetLineItems(SaveIGDLine lineItem)
        {
          

            if (mIGDLineItems == null)
            {
                mIGDLineItems = new Dictionary<int, SaveIGDLine>();
            }


            try
            {
                mIGDLineItems = Session.GetDataFromSession<Dictionary<int, SaveIGDLine>>("WMSSession.TempIGDLine");
            }
            catch
            {
                //do nothing ... because the dict miay be empty
            }

            List<SaveIGDLine> lineItems = this.ReorderLineItems(mIGDLineItems);

            return Json(new
            {
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNextIGDMoveRef() {


            string NextIGDMoveRef = "";
            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            NextIGDMoveRef = DataService.GetNextIGDMoveRef(principalID);

            var response = Json(NextIGDMoveRef, JsonRequestBehavior.AllowGet);
            return response;

        }

        public JsonResult EditableGrid_AddLineItem(SaveIGDLine lineItem, int inboundExpectedQuantity, int ProductStagingID, DateTime PODate, string PORef, DateTime ExpectedDate)
        {
            mIGDLineItems = Session.GetDataFromSession<Dictionary<int, SaveIGDLine>>("WMSSession.TempIGDLine");
            lineItem.IGDStagingID = Functions.SafeInt(lineItem.IGDStagingID, 0);
            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
           
            if (mIGDLineItems == null)
            {
                mIGDLineItems = new Dictionary<int, SaveIGDLine>();
            }


            bool isReceivedQuantityValid = false;
            if (!mIGDLineItems.ContainsKey(lineItem.LineNumber))
            {   //also check to see if the quantities match up to the expected quantities 

                isReceivedQuantityValid = this.IsReceivedQuantityValid(lineItem.ReceivedQuantity, lineItem.InboundMasterLineItemID);

                if (!isReceivedQuantityValid)
                {
                    //return an error stating that the received quantity is not valid

                    List<SaveIGDLine> linexItems = this.ReorderLineItems(mIGDLineItems);
                    return Json(new
                    {
                        Result = "ERROR: The received quantity exceeds the expected quantity, or the quantity is not valid. Cannot add a new line.",
                        Data = linexItems
                    }, JsonRequestBehavior.AllowGet);
                }

             
                try
                {

                    IGDStaging igdLi = new IGDStaging();

                    igdLi.IGDStagingID = lineItem.IGDStagingID;
                    igdLi.InboundMasterLineItemID = lineItem.InboundMasterLineItemID;

                    if ((lineItem.InboundMasterLineItemID == 0) || (lineItem.InboundMasterLineItemID == null))
                    {
                        //cannot save ... throw exception
                        throw new System.Exception("Cannot Commit IGD to Database.");
                    }

                    string MoveRef = DataService.GetNextIGDMoveRef(principalID);

                    igdLi.MoveRef = lineItem.MoveRef;
                    igdLi.ReceivedQuantity = lineItem.ReceivedQuantity;
                    igdLi.SubmittedToWMS = lineItem.SubmittedToWMS;
                    igdLi.InboundMasterLineItemID = lineItem.InboundMasterLineItemID;
                    igdLi.ProductStagingID = ProductStagingID;
                    igdLi.RecordSource = "WMSCustomerPortal";
                    igdLi.SiteCode = "001";
                    igdLi.PrincipalID = principalID;
                    igdLi.PrincipalCode = Session.GetDataFromSession<string>("WMSSession.PrincipalCode");
                    igdLi.AnalysisA = "";
                    igdLi.AnalysisA = "";
                    igdLi.OrderLineNo = lineItem.LineNumber.ToString();
                    igdLi.ReceiptType = "0";
                    igdLi.PODate = PODate;
                    igdLi.StockDateTime = System.DateTime.Now;
                    igdLi.PORef = PORef;
                    igdLi.ExpectedDeliveryDateTime = ExpectedDate;
                    igdLi.EventTerminal = this.TerminalID;
                    igdLi.EventDate = System.DateTime.Now;
                    igdLi.EventUser = this.LoggedOnUser;
                    igdLi.Deleted = false;

                    //ok ... now we save to the database
                    int saveRecord = SavePartialIGDItem(igdLi, principalID);
                    if (saveRecord < 1)
                    {
                        //there is an issue ...
                         throw new System.Exception("Cannot Commit IGD to Database.");
                    }
                    else
                    {
                        lineItem.IGDStagingID = saveRecord;
                    }

                    //add to the dictionary
                    mIGDLineItems.Add(lineItem.LineNumber, lineItem);

                }
                catch(Exception ex)
                {
                    List<SaveIGDLine> linexxItems = this.ReorderLineItems(mIGDLineItems);

                    return Json(new
                    {
                        Result = "FAILURE",
                        Data = linexxItems
                    }, JsonRequestBehavior.AllowGet);
                }
               
            }

            List<SaveIGDLine> lineItems = this.ReorderLineItems(mIGDLineItems);

            return Json(new
            {
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditableGrid_RemoveLineItem(int lineNumber)
        {

            mIGDLineItems = Session.GetDataFromSession<Dictionary<int, SaveIGDLine>>("WMSSession.TempIGDLine");
            string retVal = "";

            string eventTerminal = this.TerminalID;
            string eventUser = this.LoggedOnUser;

            if (mIGDLineItems.ContainsKey(lineNumber))
            {


                if (IsIGDLineItemDeleatable(mIGDLineItems[lineNumber]))
                {
                    bool success = false;
                    //whack from the database
                    if(mIGDLineItems[lineNumber].IGDStagingID != 0)
                    {
                        //only delete if this record exists in the database
                        success = DataService.DeleteIGDStaging(mIGDLineItems[lineNumber].IGDStagingID, eventTerminal, eventUser);
                    }
                    //remove from the dictionary
                    mIGDLineItems.Remove(lineNumber);
                    retVal = "OK";
                }
                else
                {
                    retVal = "Record cannot be deleted. May have additional dependencies.";
                }


            }

            List<SaveIGDLine> lineItems = this.ReorderLineItems(mIGDLineItems);

            return Json(new
            {
                Result = retVal, //"OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditableGrid_UpdateLineItem(SaveIGDLine lineItem)
        {
            mIGDLineItems = Session.GetDataFromSession<Dictionary<int, SaveIGDLine>>("WMSSession.TempIGDLine");

            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            if (mIGDLineItems.ContainsKey(lineItem.LineNumber))
            {
                SaveIGDLine request = (SaveIGDLine)mIGDLineItems[lineItem.LineNumber];
                request.ReceivedQuantity = lineItem.ReceivedQuantity;
                request.IGDStagingID = lineItem.IGDStagingID;
                request.InboundMasterLineItemID = lineItem.InboundMasterLineItemID;
                request.MoveRef = lineItem.MoveRef;
                request.SubmittedToWMS = lineItem.SubmittedToWMS;
                request.LineNumber = lineItem.LineNumber;

                mIGDLineItems[lineItem.LineNumber] = lineItem;
                
                try
                {
                    //persist this change to the database
                    IGDStaging igdLi = new IGDStaging();
                    igdLi.IGDStagingID = lineItem.IGDStagingID;
                    igdLi.InboundMasterLineItemID = lineItem.InboundMasterLineItemID;

                    if ((lineItem.InboundMasterLineItemID == 0) || (lineItem.InboundMasterLineItemID == null))
                    {
                        //cannot save ... throw exception
                        throw new System.Exception("Cannot Commit IGD to Database.");
                    }

                    igdLi.MoveRef = lineItem.MoveRef;
                    igdLi.ReceivedQuantity = lineItem.ReceivedQuantity;
                    igdLi.SubmittedToWMS = lineItem.SubmittedToWMS;
                   
                    //ok ... now we save 
                   int saveRecord = SavePartialIGDItem(igdLi, principalID);

                    if (saveRecord < 1)
                    { 
                        throw new System.Exception("Cannot persist the LineItem to the Database.");
                    }
                    Session.SetDataToSession<string>("WMSSession.TempIGDLine", mIGDLineItems);
 
                }
                catch(Exception ex)
                {
                    List<SaveIGDLine> lineItemsfail = this.ReorderLineItems(mIGDLineItems);
                    return Json(new
                    {
                        Result = "FAILURE",
                        Data = lineItemsfail
                    }, JsonRequestBehavior.AllowGet);
 
                }

                mIGDLineItems[lineItem.LineNumber] = lineItem;
            }

            List<SaveIGDLine> lineItems = this.ReorderLineItems(mIGDLineItems);

            return Json(new
            {
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditableGrid_SaveLineItems()
        {

            mIGDLineItems = Session.GetDataFromSession<Dictionary<int, SaveIGDLine>>("WMSSession.TempIGDLine");

            if (mIGDLineItems == null || mIGDLineItems.Count == 0)
            {
                return Json(new
                {
                    Result = "ERROR: No IGD's have been added."
                }, JsonRequestBehavior.AllowGet);
            }

            // TODO: Save Line Items


            mIGDLineItems.Clear();

            List<SaveIGDLine> lineItems = mIGDLineItems.Values.ToList<SaveIGDLine>();

            // Clear Session
            Session.SetDataToSession<string>("WMSSession.TempIGDLine", mIGDLineItems);

            return Json(new
            {
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }

        private List<SaveIGDLine> ReorderLineItems(Dictionary<int, SaveIGDLine> dictLineItems)
        {

            Dictionary<int, SaveIGDLine> newDictLineItems = new Dictionary<int, SaveIGDLine>();

            int index = 0;
            foreach (var item in dictLineItems)
            {
                SaveIGDLine request = (SaveIGDLine)item.Value;
                request.LineNumber = index + 1;

                newDictLineItems.Add(request.LineNumber, request);
                index++;
            }

            List<SaveIGDLine> lineItems = newDictLineItems.Values.ToList<SaveIGDLine>();

            Session.SetDataToSession<string>("WMSSession.TempIGDLine", newDictLineItems);

            return lineItems;
        }

       
        #endregion


        public struct SaveIGDLine
        {
            public int IGDStagingID { get; set; }
            public int LineNumber { get; set; }
            public string MoveRef { get; set; }
            public int ReceivedQuantity { get; set; }
            public bool SubmittedToWMS { get; set; }
            public int InboundMasterLineItemID { get; set; }
           
        }



        [Authorize]
        public ActionResult WarehouseEdit(int inboundMasterID = 0)
        {
            //pass through a model for the new page
            var warehouseEditModel = new InboundMasterItem { InboundMasterID = inboundMasterID };
            return View(warehouseEditModel);
        }

        

    }
}