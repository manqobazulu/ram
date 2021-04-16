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

   [Authorize(Roles = "Backdoor, Inbound")]
    public class InboundController : BaseController
    {

        #region Service Instances

        WMSCustomerPortal.Business.InboundService _inboundService;
       
        private WMSCustomerPortal.Business.InboundService DataService
        {
            get
            {
                if (_inboundService == null)
                {
                    _inboundService = new WMSCustomerPortal.Business.InboundService();
                }
                return _inboundService;
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

        WMSCustomerPortal.Business.CommonService _commonservice;
        private WMSCustomerPortal.Business.CommonService CommonDataService
        {
            get
            {
                if (_commonservice == null)
                {
                    _commonservice = new WMSCustomerPortal.Business.CommonService();
                }
                return _commonservice;
            }
        }


        #endregion

        [Authorize]

        
        [CustomAuthorizationAttribute]
        public ActionResult InboundList()
        {
            return View("InboundList");

        }

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        public ActionResult InboundAddNew() {
            return View("InboundAddNew");

        }

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        public ActionResult AddReturn()
        {
            return View();
        }

        [Authorize]

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
       public ActionResult InboundAddEdit(int inboundMasterID = 0)
       {
          //pass through a model for the new page
           var inboundAddModel = new InboundMasterItem { InboundMasterID = inboundMasterID };
           return View(inboundAddModel);
       }

       /// <summary>
       /// Prepopulates the grid dictionary with the values 
       /// </summary>
       /// <param name="inboundMasterID"></param>
       /// <returns></returns>
        public JsonResult EditableGrid_PrepopulateLineItems(int inboundMasterID)
        {
            //load all the inbound master line items for the master id specified
             var sessionData = DataService.GetInboundMasterLineItemsList(inboundMasterID);
           
              if (mOrderLineItems == null) {
                mOrderLineItems = new Dictionary<int, SaveInboundLine>();
            }

            //clear the session data first with the line items
            mOrderLineItems.Clear();

            Session.SetDataToSession<Dictionary<int, SaveInboundLine>>("WMSSession.TempInboundOrder",  mOrderLineItems);
            int lineNumber = 0;

            try { 
            //we need to convert this dynamic list to a InboundMasterLineItem list
            //quick and dirty ... lets serialize and then deserialize
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(sessionData);
            var c = serializer.Deserialize<List<InboundMasterLineItem>>(json);
            
            foreach (WMSCustomerPortal.Models.Entities.InboundMasterLineItem ibl in c)
            {
                SaveInboundLine saveibl = new SaveInboundLine();
                saveibl.InboundMasterID = inboundMasterID;
                saveibl.InboundMasterLineItemID = Functions.SafeInt(ibl.InboundMasterLineItemID);
                saveibl.EANCode = Functions.SafeString(   ibl.EANCode, "");
                saveibl.LineNumber = lineNumber;
                saveibl.LongDesc =  Functions.SafeString(   ibl.LongDesc, "");
                saveibl.ProdCode =  Functions.SafeString(   ibl.ProdCode, "");
                saveibl.ProductStagingID = Functions.SafeInt( ibl.ProductStagingID, 0);
                saveibl.Qty =  Functions.SafeInt(ibl.ExpectedQuantity, 0);
                saveibl.ShortDesc = Functions.SafeString(ibl.ShortDesc, "");
                saveibl.UnitCost = Functions.SafeDouble(ibl.UnitCost, 0.00);

                lineNumber++;
                //add this value to the dictionary
                if (!mOrderLineItems.ContainsKey(lineNumber))
                {
                    mOrderLineItems.Add(lineNumber, saveibl);
                }
            }

            }
            catch(Exception ex)
            {
                throw ex;
            }

            //now save this to the session variable
            // List<SaveInboundLine> lineItems = newDictLineItems.Values.ToList<SaveInboundLine>();

            Session.SetDataToSession<Dictionary<int, SaveInboundLine>>("WMSSession.TempInboundOrder", mOrderLineItems);
            
            return Json(new
            {
                Result = "OK"
               
            }, JsonRequestBehavior.AllowGet);
        }

       

       //checks to see whether the item is deleteable
        private bool IsInboundLineItemDeleatable(SaveInboundLine inboundlineItem)
        {
            bool retVal = true;
            //check to see if deleteable
            retVal =  InboundService.CheckDeletable(inboundlineItem.InboundMasterLineItemID);

            return retVal;
        }



        #region Editable grid

        Dictionary<int, SaveInboundLine> mOrderLineItems = null;

       /// <summary>
       /// Clears all the temp line items
       /// </summary>
       /// <returns></returns>
        public JsonResult EditableGrid_ClearAllLineItems()
        {
            //mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveInboundLine>>("WMSSession.TempInboundOrder");

            if (mOrderLineItems == null)
            {
                mOrderLineItems = new Dictionary<int, SaveInboundLine>();
            }

            //Just clear the values
            Session.SetDataToSession<string>("WMSSession.TempInboundOrder", mOrderLineItems);
            return Json(new
            {
                Result = "OK"
               
            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ClearLineItem(string viewSession) {

            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveInboundLine>>(viewSession);

            mOrderLineItems = new Dictionary<int, SaveInboundLine>(); ;

            List<SaveInboundLine> lineItems = this.ReorderLineItemsWithSession(mOrderLineItems, viewSession);

            return Json(new {
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult EditableGrid_GetLineItems(SaveInboundLine lineItem)
        {
            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveInboundLine>>("WMSSession.TempInboundOrder");

            if (mOrderLineItems == null) {
                mOrderLineItems = new Dictionary<int, SaveInboundLine>();
            }

            List<SaveInboundLine> lineItems = this.ReorderLineItems(mOrderLineItems);

            return Json(new {
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditableGrid_AddLineItem(SaveInboundLine lineItem)
        {
           
            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveInboundLine>>("WMSSession.TempInboundOrder");

            lineItem.InboundMasterLineItemID = Functions.SafeInt(lineItem.InboundMasterLineItemID, 0);
           

           if (mOrderLineItems == null) {
                mOrderLineItems = new Dictionary<int, SaveInboundLine>();
            }

           if (!mOrderLineItems.ContainsKey(lineItem.LineNumber))
           {   //also check to see whether the product is already in a different line item ///
               bool productExistsInDict = false;
               bool productValid = false;
               //check for 0 expected values
               if(Functions.SafeInt(lineItem.Qty , 0) < 1)
               {
                   //return with error
                   List<SaveInboundLine> linexItems = this.ReorderLineItems(mOrderLineItems);
                   return Json(new
                   {
                       Result = "ERROR: Quantity cannot be zero. Cannot add a new line.",
                       Data = linexItems
                   }, JsonRequestBehavior.AllowGet);

               }
               
               if (EditableGrid_DoesProductExistInDict(lineItem.ProductStagingID)) {
                   productExistsInDict = true;
                   List<SaveInboundLine> linexItems = this.ReorderLineItems(mOrderLineItems); 
                   return Json(new
                   {
                       Result = "ERROR: Product already added. Cannot add a new line.",
                       Data = linexItems
                   }, JsonRequestBehavior.AllowGet);
               }
               else
               {
                   productExistsInDict = false;
               }


               if ((lineItem.ProductStagingID == 0) || ((lineItem.ProductStagingID == null)))
               {
                   productValid = false;
                   List<SaveInboundLine> linexItems = this.ReorderLineItems(mOrderLineItems);
                   return Json(new
                   {
                       Result = "ERROR: Product not valid. Cannot add a new line.",
                       Data = linexItems
                   }, JsonRequestBehavior.AllowGet);
               }
               else
               {
                   productValid = true;
               }

               if(!productExistsInDict && productValid )
               {
                   //lets persist this lineitem to the database before adding to the dictionary
                  
                   try{
                       //get into entity structure
                        InboundMasterLineItem ibli = new InboundMasterLineItem();
                        ibli.ExpectedQuantity = lineItem.Qty;
                        //ibli.InboundMasterID = inboundMasterID;
                        ibli.InboundMasterLineItemID = Functions.SafeInt(lineItem.InboundMasterLineItemID, 0);
                        ibli.ProductStagingID = Functions.SafeInt(lineItem.ProductStagingID, 0);
                        ibli.UnitCost = Functions.SafeDouble( lineItem.UnitCost, 0.00);
                        ibli.InboundMasterID = lineItem.InboundMasterID;
                      
                        //ok ... now we save 
                        int saveRecord = SaveInboundMasterLineItem(ibli);
                       //update the lineitem for this record
                        lineItem.InboundMasterLineItemID = saveRecord;

                        if (saveRecord > 0)
                            { mOrderLineItems.Add(lineItem.LineNumber, lineItem); }
                        else
                            { 
                             throw new System.Exception("Cannot persist the LineItem to the Database."); 
                            }
                   
                   }
                   catch(Exception ex)
                   {
                       throw ex;
                   }
               
               }
           }
           else
           {
               foreach (var item in mOrderLineItems)
               {
                   SaveInboundLine request = (SaveInboundLine)item.Value;
                   if (request.ProductStagingID == lineItem.ProductStagingID)
                   {
                       return Json(new
                       {
                           Result = "ERROR: Product already added."
                       }, JsonRequestBehavior.AllowGet);
                   }
               }
           }

            List<SaveInboundLine> lineItems = this.ReorderLineItems(mOrderLineItems);            

            return Json(new { 
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }


        private bool EditableGrid_DoesProductExistInDict(int productStagingID)
        {
            //get the dict and see whether the list exists
            Dictionary<int, SaveInboundLine> tempDict = new Dictionary<int, SaveInboundLine>();
            bool retVal = false;
            try
            {
              
                tempDict = Session.GetDataFromSession<Dictionary<int, SaveInboundLine>>("WMSSession.TempInboundOrder");
            }
            catch (Exception ex)
            {
                return false; // there are no items in the dict
            }


            foreach (var item in tempDict)
            {
                SaveInboundLine request = (SaveInboundLine)item.Value;
                if (request.ProductStagingID == productStagingID)
                {
                    retVal = true; //it exists
                }
            }

            return retVal;

        }


        public JsonResult EditableGrid_RemoveLineItem(int lineNumber) {

            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveInboundLine>>("WMSSession.TempInboundOrder");
            string retVal = "";
           
            string eventTerminal = this.TerminalID;
            string eventUser = this.LoggedOnUser;
           
            if (mOrderLineItems.ContainsKey(lineNumber)) {
                
                
                if (IsInboundLineItemDeleatable(mOrderLineItems[lineNumber]))
                {
                    //whack it in the database
                    try
                    {
                        int ibmlid = 0;
                        SaveInboundLine tmp = new SaveInboundLine();
                        mOrderLineItems.TryGetValue(lineNumber, out tmp);
                        if (tmp.InboundMasterLineItemID != 0) { 
                        //delete from database
                        //InboundService.DeleteInboundMasterLineItem(mOrderLineItems[lineNumber].InboundMasterLineItemID , eventTerminal, eventUser);
                            InboundService.DeleteInboundMasterLineItem(tmp.InboundMasterLineItemID, eventTerminal, eventUser);
                        }
                        //after a successful delete 
                        mOrderLineItems.Remove(lineNumber);
                        retVal = "OK";
                    }
                    catch(Exception ex)
                    {
                        retVal = "Record cannot be deleted. May have additional dependencies.";
                    }
                   
                }
                else
                {
                    retVal = "Record cannot be deleted. May have additional dependencies.";
                }

               
            }

            List<SaveInboundLine> lineItems = this.ReorderLineItems(mOrderLineItems);

            return Json(new {
                Result = retVal, //"OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditableGrid_RemoveLineItemAddNew(int lineNumber, string viewSession) {

            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveInboundLine>>(viewSession);

            if (mOrderLineItems.ContainsKey(lineNumber)) {
                mOrderLineItems.Remove(lineNumber);
            }

            List<SaveInboundLine> lineItems = this.ReorderLineItemsAddNew(mOrderLineItems, viewSession);

            return Json(new {
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }

        private List<SaveInboundLine> ReorderLineItemsAddNew(Dictionary<int, SaveInboundLine> dictLineItems, string viewSession) {

            Dictionary<int, SaveInboundLine> newDictLineItems = new Dictionary<int, SaveInboundLine>();

            int index = 0;
            foreach (var item in dictLineItems) {
                SaveInboundLine request = (SaveInboundLine)item.Value;
                request.LineNumber = index + 1;

                newDictLineItems.Add(request.LineNumber, request);
                index++;
            }

            List<SaveInboundLine> lineItems = newDictLineItems.Values.ToList<SaveInboundLine>();

            Session.SetDataToSession<string>(viewSession, newDictLineItems);

            return lineItems;
        }

        public JsonResult EditableGrid_AddLineItemAddNew(SaveInboundLine lineItem, string viewSession) {

            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveInboundLine>>(viewSession);


            if (mOrderLineItems == null) {
                mOrderLineItems = new Dictionary<int, SaveInboundLine>();
            }

            if (!mOrderLineItems.ContainsKey(lineItem.LineNumber)) {
                mOrderLineItems.Add(lineItem.LineNumber, lineItem);
            }
            //else {
            //    foreach (var item in mOrderLineItems) {
            //        SaveInboundLine request = (SaveInboundLine)item.Value;
            //        if (request.ProductStagingID == lineItem.ProductStagingID) {


            //            return Json(new {
            //                Result = "ERROR: Product already added"
            //            }, JsonRequestBehavior.AllowGet);
            //        }
            //    }
            //}

            List<SaveInboundLine> lineItems = this.ReorderLineItemsAddNew(mOrderLineItems, viewSession);

            return Json(new {
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Call to the save the inbound item including the line items
        /// </summary>
        /// <returns></returns>

        public JsonResult SaveNewOrderWithOrderLine(string pPurchaseOrderRef, string pSupplierName, DateTime? pPurchaseOrderDate, DateTime? pExpectedDeliveryDateTime, string pTable) {
          
            string pEventTerminal = this.TerminalID;
            string pEventUser = this.LoggedOnUser;
            int pPrincipalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            string pRecordSource = RAM.Utilities.Common.ConfigSettings.ReadConfigValue("WMSRecordSource", "UNKNOWN");
            string pSiteCode = RAM.Utilities.Common.ConfigSettings.ReadConfigValue("WMSSiteCode", "000");

            var result = DataService.SaveNewOrderWithOrderLine(new InboundMaster {
                RecordSource = pRecordSource,
                SiteCode = pSiteCode,
                PrincipalID = pPrincipalID,
                PrincipalCode = "",
                SupplierName = pSupplierName,
                PODate = pPurchaseOrderDate,
                PORef = pPurchaseOrderRef,
                ExpectedDeliveryDateTime = pExpectedDeliveryDateTime,
                EventTerminal = pEventTerminal,
                EventUser = pEventUser,
            }, pEventTerminal, pEventUser, pTable);

            return Json(new {
                Result = "OK",
                Data = result,
            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult EditableGrid_UpdateLineItemAddNew(SaveInboundLine lineItem, string viewSession) {

            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveInboundLine>>(viewSession);

            if (mOrderLineItems.ContainsKey(lineItem.LineNumber)) {
                SaveInboundLine request = (SaveInboundLine)mOrderLineItems[lineItem.LineNumber];
                request.Qty = lineItem.Qty;

                mOrderLineItems[lineItem.LineNumber] = lineItem;
            }

            List<SaveInboundLine> lineItems = this.ReorderLineItemsAddNew(mOrderLineItems, viewSession);

            return Json(new {
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditableGrid_SaveLineItemsAddNew(string viewSession) {

            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveInboundLine>>(viewSession);

            if (mOrderLineItems == null || mOrderLineItems.Count == 0) {
                return Json(new {
                    Result = "ERROR: No products have been added"
                }, JsonRequestBehavior.AllowGet);
            }

            // TODO: Save Line Items


            //mOrderLineItems.Clear();

            List<SaveInboundLine> lineItems = mOrderLineItems.Values.ToList<SaveInboundLine>();

            // Clear Session
            Session.SetDataToSession<string>(viewSession, mOrderLineItems);

            return Json(new {
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditableGrid_UpdateLineItem(SaveInboundLine lineItem)
        {
           
            
            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveInboundLine>>("WMSSession.TempInboundOrder");

            //only go through this routine if the lineitem's value is DEFINTELY different
            //becuase this method is fired multiple times

            //do compare
            if(mOrderLineItems.ContainsKey(lineItem.LineNumber))
            {
                // SaveInboundLine originalValue = (SaveInboundLine)mOrderLineItems[lineItem.LineNumber];
               
                SaveInboundLine originalValue; // = (SaveInboundLine)mOrderLineItems[lineItem.LineNumber];

                mOrderLineItems.TryGetValue(lineItem.LineNumber, out originalValue);
               
                if(
                    (originalValue.InboundMasterID != lineItem.InboundMasterID) ||
                    (originalValue.InboundMasterLineItemID != lineItem.InboundMasterLineItemID) ||
                    (originalValue.ProductStagingID != lineItem.ProductStagingID) ||
                    (originalValue.Qty != lineItem.Qty) ||
                    (originalValue.UnitCost != lineItem.UnitCost)
                )
                {
                    //we may continue

                }
                else
                {
                    // just return a success message
                    return Json(new
                    {
                        Result = "OK",
                        Data = mOrderLineItems
                    }, JsonRequestBehavior.AllowGet);

                }
            }
                



            if (mOrderLineItems.ContainsKey(lineItem.LineNumber)) {
               
                
                //SaveInboundLine request = (SaveInboundLine)mOrderLineItems[lineItem.LineNumber];
                SaveInboundLine request = new SaveInboundLine();

                mOrderLineItems.TryGetValue(lineItem.LineNumber, out request);

                request.Qty = lineItem.Qty;
                request.ProductStagingID = lineItem.ProductStagingID;
                request.UnitCost = lineItem.UnitCost;
                request.InboundMasterLineItemID = lineItem.InboundMasterLineItemID;
                request.InboundMasterID = lineItem.InboundMasterID;

                //replace the current value in the dict
                mOrderLineItems[lineItem.LineNumber] = request;
              
                 


                //also ... we cannot have 0 
                if (Functions.SafeInt(lineItem.Qty, 0) < 1)
                {
                    //return with error
                    List<SaveInboundLine> linexItems = this.ReorderLineItems(mOrderLineItems);
                    return Json(new
                    {
                        Result = "ERROR: Quantity cannot be zero. Cannot add a new line.",
                        Data = linexItems
                    }, JsonRequestBehavior.AllowGet);

                }


                try
                {
                    //persist this change to the database

                    InboundMasterLineItem ibli = new InboundMasterLineItem();
                    ibli.ExpectedQuantity = Functions.SafeInt(request.Qty,0);
                    ibli.InboundMasterID = Functions.SafeInt(request.InboundMasterID,0);
                    ibli.InboundMasterLineItemID = Functions.SafeInt(request.InboundMasterLineItemID, 0);
                    ibli.ProductStagingID = Functions.SafeInt(request.ProductStagingID, 0);
                    ibli.UnitCost = Functions.SafeDouble(request.UnitCost, 0.00);
                   
                    //ok ... now we save 
                    int saveRecord = SaveInboundMasterLineItem(ibli);

                    if (saveRecord < 1)
                    { 
                        throw new System.Exception("Cannot persist the LineItem to the Database.");
                    }
                    Session.SetDataToSession < Dictionary<int, SaveInboundLine>>("WMSSession.TempInboundOrder", mOrderLineItems);
 
                }
                catch(Exception ex)
                {
                    List<SaveInboundLine> lineItemsfail = this.ReorderLineItems(mOrderLineItems);
                    return Json(new
                    {
                        Result = "FAILURE",
                        Data = lineItemsfail
                    }, JsonRequestBehavior.AllowGet);
 
                }
            

            
            }
              List<SaveInboundLine> lineItems = this.ReorderLineItems(mOrderLineItems);

              return Json(new
              {
                  Result = "OK",
                  Data = lineItems
              }, JsonRequestBehavior.AllowGet);
 
        }

        public JsonResult EditableGrid_SaveLineItems() {

            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveInboundLine>>("WMSSession.TempInboundOrder");

            if (mOrderLineItems == null || mOrderLineItems.Count == 0) {
                return Json(new {
                    Result = "ERROR: No products have been added"
                }, JsonRequestBehavior.AllowGet);
            }

            // TODO: Save Line Items


            mOrderLineItems.Clear();

            List<SaveInboundLine> lineItems = mOrderLineItems.Values.ToList<SaveInboundLine>();

            // Clear Session
            Session.SetDataToSession<string>("WMSSession.TempInboundOrder", mOrderLineItems);

            return Json(new {
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }

        private List<SaveInboundLine> ReorderLineItems(Dictionary<int, SaveInboundLine> dictLineItems)
        {

            Dictionary<int, SaveInboundLine> newDictLineItems = new Dictionary<int, SaveInboundLine>();

            int index = 0;

            //get all the keys in the dictionary

            var list = dictLineItems.Keys.ToList();
            list.Sort();

            List<SaveInboundLine> lineItems = new List<SaveInboundLine>();
            // Loop through keys.
            foreach (var key in list)
            {
                SaveInboundLine request = (SaveInboundLine)dictLineItems[key];
                request.LineNumber = index + 1;
                lineItems.Add(request);
                newDictLineItems.Add(key, request);
                index++;
                //Console.WriteLine("{0}: {1}", key, dictionary[key]);
            }
           

            Session.SetDataToSession<string>("WMSSession.TempInboundOrder", newDictLineItems);

            return lineItems;
        }


        private List<SaveInboundLine> ReorderLineItemsWithSession(Dictionary<int, SaveInboundLine> dictLineItems, string viewSession) {

            Dictionary<int, SaveInboundLine> newDictLineItems = new Dictionary<int, SaveInboundLine>();

            int index = 0;

            //get all the keys in the dictionary

            var list = dictLineItems.Keys.ToList();
            list.Sort();

            List<SaveInboundLine> lineItems = new List<SaveInboundLine>();
            // Loop through keys.
            foreach (var key in list) {
                SaveInboundLine request = (SaveInboundLine)dictLineItems[key];
                request.LineNumber = index + 1;
                lineItems.Add(request);
                newDictLineItems.Add(key, request);
                index++;
                //Console.WriteLine("{0}: {1}", key, dictionary[key]);
            }


            Session.SetDataToSession<string>(viewSession, newDictLineItems);

            return lineItems;
        }




        [AllowAnonymous]
        public JsonResult EditableGrid_ProductLookup(string startsWith, string searchType)  {

            int principalId =  Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            var data = CommonDataService.Product_Lookup(principalId, startsWith, searchType);

            return Json(data);
        }

        #endregion

       


        #region Inbound Master


        /// <summary>
        /// Gets the inbound list
        /// </summary>
        /// <param name="param"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public JsonResult GetInboundList(JQueryDataTableParamModel param, string filter)
        {

            int principalID = 0;
            try
            {
                principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            }
            catch
            {
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

            var data = new
            {
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
        /// Gets the inbound list for the completed items
        /// </summary>
        /// <param name="param"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public JsonResult GetInboundListCompleted(JQueryDataTableParamModel param, string filter)
        {

           
            int principalID = 0;
            try
            {
                principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            }
            catch
            {
                principalID = 0;
            }


            //TODO: Dates that are to be filtered
            System.DateTime dtFrom = new DateTime();
            System.DateTime dtTo = new DateTime();
            dtFrom = DateTime.Now.AddYears(-2);
            dtTo = DateTime.Now;

            //TODO: Purcase Order Reference
            string poRef = "";

            var sessionData = DataService.GetAllInboundMasterFilterCompleted(principalID, poRef, dtFrom, dtTo); //.GetAllCustomers(principalID);

            var data = new
            {
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
      /// Retrieves an Inbound master record 
      /// </summary>
      /// <param name="inboundMasterID"></param>
      /// <returns></returns>
       public JsonResult GetInboundMasterRecord(int inboundMasterID)
        {

            InboundMaster ibItem = new InboundMaster();

            if (inboundMasterID == 0)
            {
                //just return an empty item
                var data = new
                {
                    InboundMaster = ibItem
                };
                var response = Json(data, JsonRequestBehavior.AllowGet);
                return response;
            }
            else
            {

                ibItem = DataService.GetInboundMaster(inboundMasterID);

                var data = new
                {
                    InboundMaster = ibItem
                };
                var response = Json(data, JsonRequestBehavior.AllowGet);
                return response;
            }
        }


 
        /// <summary>
        /// Call to the save the inbound item including the line items
        /// </summary>
        /// <param name="inboundMaster"></param>
        /// <returns></returns>
        public JsonResult SaveInbound(InboundMaster inboundMaster)
        {
           
            //string eventUser = "";
            string eventTerminal = this.TerminalID;
            string eventUser = this.LoggedOnUser;
            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            string recordSource = RAM.Utilities.Common.ConfigSettings.ReadConfigValue("WMSRecordSource","UNKNOWN");
            string siteCode = RAM.Utilities.Common.ConfigSettings.ReadConfigValue("WMSSiteCode","000");
            inboundMaster.RecordSource = recordSource;
            inboundMaster.SiteCode = siteCode;

            try
            {
                inboundMaster.PrincipalID = principalID;
            }
            catch (Exception)
            {
                inboundMaster.PrincipalID = 0;
            }

           
            //we would like to save this record somewhere
            int saveRetVal = 0;
            bool updateRetVal = false;
            if((inboundMaster.InboundMasterID < 1))
            {
                saveRetVal = InboundService.AddInboundMaster(inboundMaster, eventUser, eventTerminal);

            }
            else
            {
                updateRetVal = InboundService.UpdateInboundMaster(inboundMaster, eventUser, eventTerminal);
                saveRetVal = inboundMaster.InboundMasterID;

             }

            //ok ... we have the master id ... lets loop through the line items and then save accordingly

            bool saveLineItemResult = false;
            saveLineItemResult = CommitInboundMasterLineItems(saveRetVal); //pass through the master id
          
            if (saveLineItemResult == false)
            {
                var result = new BaseSaveResponse
                {

                    Details = "Failed to save the line items."
                };

                var response = new
                {

                    InboundMasterID = saveRetVal,
                    SaveResponse = result,
                    Result = "FAILURE"
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new BaseSaveResponse
                {

                    Details = "OK"
                };

                var response = new
                {

                    InboundMasterID = saveRetVal,
                    SaveResponse = result,
                    Result = "SUCCESS"
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }

           

           
        }

        /// <summary>
        /// Deletes an inbound master  item
        /// </summary>
        /// <param name="inboundMasterLineItemID"></param>
        /// <returns></returns>
        public JsonResult DeleteInboundMaster(int inboundMasterID)
        {

            string eventTerminal = this.TerminalID;
            string eventUser = this.LoggedOnUser;

            //we would like to delete this record 

            bool success = false;

            success = InboundService.DeleteInboundMaster(inboundMasterID, eventTerminal, eventUser);
            
            if (success)
            {
                var response = new
                {
                    SaveResponse = new BaseSaveResponse { Details = "Record Deleted." },
                    Result = "SUCCESS"
                };

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var response = new
                {
                    SaveResponse = new BaseSaveResponse { Details = "Record Not Deleted." },
                    Result = "FAILURE"
                };

                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region InboundMasterLineItem

        
        private bool CommitInboundMasterLineItems(int inboundMasterID) //pass through the master id
        {
            bool retVal = true;

            //get the dictionary
            

            if (mOrderLineItems == null)
            {
                mOrderLineItems = new Dictionary<int, SaveInboundLine>();
            }
            
            try
            {
                mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveInboundLine>>("WMSSession.TempInboundOrder");
            }
            catch(Exception ex)
            {
                // we cannot get to the dictionary ... do nothing because we dont have any lines possibly
            }
            List<SaveInboundLine> lineItems = this.ReorderLineItems(mOrderLineItems);
            //loop through each of the line items and construct the lineitems and save
            foreach (SaveInboundLine lineitem in mOrderLineItems.Values)
            {
                InboundMasterLineItem ibli = new InboundMasterLineItem();
                ibli.ExpectedQuantity = lineitem.Qty;
                ibli.InboundMasterID = inboundMasterID;
                ibli.InboundMasterLineItemID = Functions.SafeInt(lineitem.InboundMasterLineItemID, 0);
                ibli.ProductStagingID = Functions.SafeInt(lineitem.ProductStagingID, 0);
                ibli.UnitCost = Functions.SafeDouble( lineitem.UnitCost, 0.00);

                //ok ... now we save 
                int saveRecord = SaveInboundMasterLineItem(ibli);

                if (saveRecord < 1)
                {
                    //there is an issue ...
                    retVal = false;
                    return retVal;
                }
                else
                {
                    retVal = true;
                }
            }


            return retVal;

        }
       
       
        /// <summary>
        /// call to save the inbound master line item
        /// </summary>
        /// <param name="inboundMasterLineItem"></param>
        /// <returns></returns>
        private int SaveInboundMasterLineItem(InboundMasterLineItem inboundMasterLineItem)
        {
            
            string eventTerminal = this.TerminalID;
            string eventUser = this.LoggedOnUser;
           
            //we would like to save this record somewhere
            int recordsAffected = 0;
            if ( (inboundMasterLineItem.InboundMasterLineItemID < 1))
            {
                recordsAffected = InboundService.AddInboundMasterLineItem(inboundMasterLineItem, eventUser, eventTerminal);

            }
            else
            {
                recordsAffected = InboundService.UpdateInboundMasterLineItem(inboundMasterLineItem, eventUser, eventTerminal);
            }

            return recordsAffected;
        }



        /// <summary>
        /// Deletes an inbound master line item
        /// </summary>
        /// <param name="inboundMasterLineItemID"></param>
        /// <returns></returns>
        private bool DeleteInboundMasterLineItem(int inboundMasterLineItemID)
        {
           
            string eventTerminal = this.TerminalID;
            string eventUser = this.LoggedOnUser;

            //we would like to delete this record 

            bool success = InboundService.DeleteInboundMasterLineItem(inboundMasterLineItemID, eventTerminal, eventUser);

            return success;
        }

        #endregion

        public JsonResult GetInboundLineItemList(JQueryDataTableParamModel param, int filter)
        {

            var sessionData = DataService.GetInboundMasterLineItemsList(filter); 

            var data = new
            {
                sEcho = param.sEcho,
                iTotalRecords = sessionData.Count,
                iTotalDisplayRecords = sessionData.Count,
                aaData = sessionData
            };

            // Test serialization using overriden method
            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;
        }



    }

   public struct SaveInboundLine
   {
      // public string ItemGUID { get; set; } //this will be used as the unique key in the dictionary and database so we know which ... 
       public int InboundMasterID { get; set; }
       public int InboundMasterLineItemID { get; set; }
       public int LineNumber { get; set; }
       public int ProductStagingID { get; set; }
       public string ProdCode { get; set; }
       public string EANCode { get; set; }
       public string ShortDesc { get; set; }
       public string LongDesc { get; set; }
       public double UnitCost { get; set; }
       // public double SalesPrice { get; set; }
       public int Qty { get; set; }
   }
}