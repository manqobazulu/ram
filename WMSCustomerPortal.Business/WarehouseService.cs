using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSCustomerPortal.Models.Entities;
using RAM.Utilities.Common;
using System.Data;


namespace WMSCustomerPortal.Business
{
    public class WarehouseService
    {
       

        #region Host Web Service Calls

        /// <summary>
        /// Call the host WebService to write an IGD message
        /// </summary>
        public static int WriteIGDToHost(IGDStaging stagingRecord)
        {
            int retVal = 0;
            try
            {
               
                WS_RAM_Host.HostWS hostService = new WS_RAM_Host.HostWS();
               hostService.Url = ConfigSettings.ReadConfigValue("RAMHostWSPath", "");
                //hostService.Url = "http://10.0.20.34:8085";
               //hostService.Url = "http://10.0.201.34:8085";

                hostService.Credentials = System.Net.CredentialCache.DefaultCredentials;

                hostService.IGDAdd(Functions.SafeString(stagingRecord.RecordSource),
                                      Functions.SafeString(stagingRecord.SiteCode),
                                      Functions.SafeString(stagingRecord.PrincipalCode),
                                      Functions.SafeString(stagingRecord.ProdCode),
                                      Functions.SafeString(stagingRecord.EANCode),
                                      Functions.SafeString(stagingRecord.ShortDesc),
                                      Functions.SafeString(stagingRecord.LongDesc),
                                      Functions.SafeBool(stagingRecord.Serialised),
                                      Functions.SafeString(stagingRecord.AnalysisA),
                                      Functions.SafeString(stagingRecord.AnalysisB),
                                      Functions.SafeString(stagingRecord.OrderLineNo),
                                      Functions.SafeInt(stagingRecord.ReceivedQuantity, 0),
                                      Functions.SafeString(stagingRecord.ReceiptType),
                                      Functions.SafeString(stagingRecord.MoveRef),
                                      Functions.SafeString(stagingRecord.PORef),
                                      Functions.SafeDateTime(stagingRecord.PODate),
                                      Functions.SafeDateTime(stagingRecord.StockDateTime));


                retVal = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //throw ;
            }


            return retVal;

        }


        public static int AddCLCMessage(IGDStaging stagingRecord)
        {
            int retVal = 0;
            try
            {
                WS_RAM_Host.HostWS hostService = new WS_RAM_Host.HostWS();
                hostService.Url = ConfigSettings.ReadConfigValue("RAMHostWSPath", "");

                hostService.Credentials = System.Net.CredentialCache.DefaultCredentials;
                string[] SerialNumbers = new string[0];

                hostService.CLCAdd(Functions.SafeString(stagingRecord.RecordSource),
                                      Functions.SafeString(stagingRecord.SiteCode),
                                      Functions.SafeString(stagingRecord.PrincipalCode),
                                      Functions.SafeString(stagingRecord.MoveRef),
                                      Functions.SafeString(stagingRecord.OrderLineNo),
                                      Functions.SafeString(stagingRecord.ProdCode),
                                      Functions.SafeInt(stagingRecord.ReceivedQuantity, 0),
                                      Functions.SafeString(stagingRecord.ReceiptType),
                                      0,
                                      0,
                                      SerialNumbers);


                retVal = 1;
            }
            catch
            {
                throw;
            }


            return retVal;

        }

        #endregion

        #region Warehouse 

        /// <summary>
        /// Gets all the inbound Line Items for a specified Inbound master ID
        /// </summary>
        /// <param name="masterID"></param>
        /// <returns></returns>
        public System.Data.DataSet GetInboundLineItems(int masterID)
        {
            try { 
            System.Data.DataSet retVal = new System.Data.DataSet();

            retVal =   InboundMasterLineItem.ReadAllDSWarehouse(masterID);

            return retVal;
            }
            catch { throw; }
        }


        

        public List<dynamic> GetInboundLineItemsList(int masterID)
        {
            
            try{
                System.Data.DataSet ds = new DataSet();
            ds = GetInboundLineItems(masterID);

            var result = ds.Tables[0].ToDynamicList("InboundMasterLineItem");

            return result;
            }
            catch { throw; }

        }




        public List<dynamic> GetAllInboundMasterFilter(int principalID, string poRef, System.DateTime fromxDate, System.DateTime toxDate)
        {
            try { 
            System.Data.DataSet retValue = new DataSet();
            retValue = InboundMaster.ReadAllFilter(principalID, poRef, fromxDate, toxDate);

            var result = retValue.Tables[0].ToDynamicList("InboundMaster");

            return result;
            }
            catch { throw; }
        }


        /// <summary>
        /// return a specific Inbound Master Line Item
        /// </summary>
        /// <param name="lineItemID"></param>
        /// <returns></returns>
        public  InboundMasterLineItem GetInboundMasterLineItem(int lineItemID)
        {
            try { 
            InboundMasterLineItem retVal = new InboundMasterLineItem();
            retVal = InboundMasterLineItem.ReadObject(lineItemID);

            return retVal;
            }
            catch { throw; }

        }


        /// <summary>
        /// Returns a pecific Dataset of Inbound Master Items filtered on criteria
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="poFilter"></param>
        /// <param name="principalID"></param>
        /// <returns></returns>
        public static System.Data.DataSet GetInboundMasterItems(DateTime fromDate, DateTime toDate, string poFilter, int principalID)
        {
            try { 
            System.Data.DataSet retVal = new System.Data.DataSet();

            if (poFilter.Trim().Length < 1)
            {
                poFilter = "";

            }

            retVal = InboundMaster.ReadAllDSWarehouse(poFilter, principalID);

            return retVal;
            }
            catch { throw; }
        }

        #endregion


        #region IGD Staging


        /// <summary>
        /// Returns a dataset of all IGD Staging records for a specified Principal ID
        /// </summary>
        /// <param name="principalID"></param>
        /// <returns></returns>
        public static System.Data.DataSet GetAllIGDStaging(int principalID)
        {
            try { 
            System.Data.DataSet retValue = new System.Data.DataSet();
            retValue = IGDStaging.ReadAllDS();

            return retValue;
            }
            catch { throw; }

        }


        /// <summary>
        /// Returns alist of all the staging records for a specified masterlineitem
        /// </summary>
        /// <param name="masterLineID"></param>
        /// <returns></returns>
        public List<dynamic> GetIGDStagingList(int masterLineItemID)
        {
            try { 
            System.Data.DataSet ds = new DataSet();

            ds = IGDStaging.ReadAllDSForMasterID(masterLineItemID);

            var result = ds.Tables[0].ToDynamicList("IGDStaging");

            return result;
            }
            catch { throw; }

        }




        /// <summary>
        /// Return a specific IGD object based on the ID supplied
        /// </summary>
        /// <param name="igdStagingID"></param>
        /// <returns></returns>
        public static IGDStaging GetIGDStaging(int igdStagingID)
        {
            try { 
            IGDStaging retVal = new IGDStaging();
            retVal = IGDStaging.ReadObject(igdStagingID);

            return retVal;
            }
            catch { throw; }
        }

        /// <summary>
        /// Checks to see whether the received quantity is valid
        /// </summary>
        /// <param name="igdReceivedQuantity"></param>
        /// <param name="inboundLineItemID"></param>
        /// <returns></returns>
        public bool IsReceivedQuantityValid(int igdReceivedQuantity, int inboundLineItemID)
        {
            int totalReceived = GetReceivedQuantityForMasterLineItemID(inboundLineItemID);
            //get the lineitem
            InboundMasterLineItem itm = new InboundMasterLineItem();
            itm = InboundMasterLineItem.ReadObject(inboundLineItemID);
            int expectedQuantity = Functions.SafeInt(itm.ExpectedQuantity, 0);
            
             
            if((igdReceivedQuantity + totalReceived) > expectedQuantity)
            {
                return false; 
            }
            else
            {
                return true;
            }


        }


        /// <summary>
        /// Calculates the received quantity for a specific master line item ID. 
        /// This is calculated from the IGD's which have already been created for this record 
        /// </summary>
        /// <param name="inboundMasterLineItemID"></param>
        /// <returns></returns>
        public static int GetReceivedQuantityForMasterLineItemID(int inboundMasterLineItemID)
        {
            try { 
            int retVal = 0;
            retVal = InboundMasterLineItem.GetReceivedInWarehouseQuantity(inboundMasterLineItemID);
            return retVal;
            }
            catch { throw; }

        }

        public string GetNextIGDMoveRef(int principalID) {
            try {
                string retVal = "";
                retVal = IGDStaging.GetNextIGDMoveRef(principalID);
                return retVal;
            }
            catch { throw; }

        }


        /// <summary>
        /// returns a dynamic list of all the igd records for a specified warehouse line item
        /// </summary>
        /// <param name="inboundMasterLineItemID"></param>
        /// <returns></returns>
        public List<dynamic> GetAllIGDForMasterRecordList(int inboundMasterLineItemID)
        {
            try { 
            System.Data.DataSet retValue = new DataSet();
            retValue = IGDStaging.ReadAllDSForMasterID(inboundMasterLineItemID);

            var result = retValue.Tables[0].ToDynamicList("IGDStaging");

            return result;
            }
            catch { throw; }
        }

        public  bool CheckIGDDeletable(int igdStagingID)
        {

            if (igdStagingID == 0)
            {
                return true; //we can delete this, because it has not been submitted to the database
            }

            //if (InboundMasterLineItem.CheckDeletable(inboundMasterLineItemID) == true)
            if (IGDStaging.CheckDeletable(igdStagingID) == true)
            {

                return true;
            }
            else
            {
                return false;

            }
        }

        /// <summary>
        /// Provides a complete IGD Item that can be stored / edited
        /// </summary>
        /// <param name="igdStagingID"></param>
        /// <param name="inboundMasterLineItemID"></param>
        /// <param name="receivedQuantity"></param>
        /// <param name="principalID"></param>
        /// <returns></returns>
        public static IGDStaging CompleteIGDItem(int igdStagingID, int inboundMasterLineItemID, int receivedQuantity, int principalID)
        {
            try { 
            IGDStaging retValue = new IGDStaging();
            if (igdStagingID > 0) //we have a valid record
            {
                retValue = GetIGDStaging(igdStagingID);
                return retValue;

            }

            //product related

            Principal principal = new Principal();
            principal = MasterDataService.GetPrincipalByID(principalID);
            InboundMasterLineItem inboundLineItem = new InboundMasterLineItem();
            inboundLineItem = InboundMasterLineItem.ReadObject(inboundMasterLineItemID);

            ProductStaging product = new ProductStaging();
            product = product.ReadObject(inboundLineItem.ProductStagingID);
            retValue.EANCode = product.EANCode;
            retValue.ProdCode = product.ProdCode;
            retValue.ShortDesc = product.ShortDesc;
            retValue.LongDesc = product.LongDesc;
            retValue.Serialised = product.Serialised;


            retValue.ProductStagingID = inboundLineItem.ProductStagingID;


            //Principal related

            retValue.PrincipalCode = principal.PrincipalCode;
            retValue.PrincipalID = principal.PrincipalID;

            //igd related
            retValue.AnalysisA = "";
            retValue.AnalysisB = "";
            retValue.OrderLineNo = "1";

            retValue.ReceivedQuantity = receivedQuantity;
            retValue.ReceiptType = "0";

            //master incoming record related
            InboundMaster master = new InboundMaster();
            master = InboundMaster.ReadObject(inboundLineItem.InboundMasterID);
            retValue.SiteCode = master.SiteCode;// WMSCustomerPortal.Library.Configuration.GetSiteCode();
            retValue.RecordSource = master.RecordSource;
            retValue.PODate = master.PODate;
            retValue.PORef = master.PORef;
            retValue.StockDateTime = inboundLineItem.EventDate;
            retValue.InboundMasterLineItemID = inboundLineItem.InboundMasterLineItemID;

            retValue.MoveRef =  WMSCustomerPortal.Models.DataAccess.SQLDataManager.GetNextMovementReference(principal.PrincipalCode);

            return retValue;
            }
            catch { throw; }
        }


        /// <summary>
        /// Updates a IGD Record object
        /// </summary>
        /// <param name="stagingRecord"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public int UpdateIGDStaging(IGDStaging stagingRecord, string eventUser, string eventTerminal)
        {
            try { 
            int retVal = 0;



            retVal = IGDStaging.Update(stagingRecord.IGDStagingID, stagingRecord.InboundMasterLineItemID, stagingRecord.ProductStagingID,
                                        stagingRecord.RecordSource, stagingRecord.SiteCode, stagingRecord.PrincipalID,
                                        stagingRecord.PrincipalCode, stagingRecord.AnalysisA, stagingRecord.AnalysisB,
                                        stagingRecord.OrderLineNo, stagingRecord.ReceiptType, stagingRecord.PODate,
                                        stagingRecord.StockDateTime, stagingRecord.MoveRef, stagingRecord.PORef,
                                        stagingRecord.ExpectedDeliveryDateTime, stagingRecord.ReceivedQuantity, stagingRecord.SubmittedToWMS,
                                        eventTerminal, eventUser);


            return retVal;
            }
            catch { throw; }
        }


        /// <summary>
        /// Updates a IGD Record object
        /// </summary>
        /// <param name="stagingRecord"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public int UpdatePartialIGDStaging(IGDStaging stagingRecord, string eventUser, string eventTerminal)
        {
            try
            {
                int retVal = 0;



                retVal = IGDStaging.UpdatePartialIGDForWarehouse(stagingRecord.IGDStagingID, stagingRecord.InboundMasterLineItemID, stagingRecord.ProductStagingID,
                                            stagingRecord.RecordSource, stagingRecord.SiteCode, stagingRecord.PrincipalID,
                                            stagingRecord.PrincipalCode, stagingRecord.AnalysisA, stagingRecord.AnalysisB,
                                            stagingRecord.OrderLineNo, stagingRecord.ReceiptType, stagingRecord.PODate,
                                            stagingRecord.StockDateTime, stagingRecord.MoveRef, stagingRecord.PORef,
                                            stagingRecord.ExpectedDeliveryDateTime, stagingRecord.ReceivedQuantity, stagingRecord.SubmittedToWMS,
                                            eventTerminal, eventUser);


                return retVal;
            }
            catch { throw; }
        }

        /// <summary>
        /// Updates a IGD Record object and return a completed object
        /// </summary>
        /// <param name="stagingRecord"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public IGDStaging UpdateAndRetrieveIGDStaging(IGDStaging stagingRecord, string eventUser, string eventTerminal)
        {
            try
            {
                int retVal = 0;

                //sooooo .... we are getting a partial igdstaging record

                retVal = IGDStaging.Update(stagingRecord.IGDStagingID, stagingRecord.InboundMasterLineItemID, stagingRecord.ProductStagingID,
                                            stagingRecord.RecordSource, stagingRecord.SiteCode, stagingRecord.PrincipalID,
                                            stagingRecord.PrincipalCode, stagingRecord.AnalysisA, stagingRecord.AnalysisB,
                                            stagingRecord.OrderLineNo, stagingRecord.ReceiptType, stagingRecord.PODate,
                                            stagingRecord.StockDateTime, stagingRecord.MoveRef, stagingRecord.PORef,
                                            stagingRecord.ExpectedDeliveryDateTime, stagingRecord.ReceivedQuantity, stagingRecord.SubmittedToWMS,
                                            eventTerminal, eventUser);





                return stagingRecord;
            }
            catch { throw; }
        }


        /// <summary>
        /// Delets an IGD Staging record
        /// </summary>
        /// <param name="igdStagingID"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public  bool DeleteIGDStaging(int igdStagingID, string eventUser, string eventTerminal)
        {
            try { 
            bool retVal = false;
            // errorResult = "";
            bool deleteable = false;
            //check to see whether this record may be deleted

           
            if (IGDStaging.CheckDeletable(igdStagingID) == true)
            {

                deleteable = true;

            }
            else
            {
                //  errorResult = "Cannot delete while there are IGD Messages or Inbound Line Items on record.";
                deleteable = false;
            }

            if (deleteable)
            {

                retVal = IGDStaging.Delete(igdStagingID, eventTerminal, eventUser);
            }
            else
            {
                retVal = false;

            }

            return retVal;

            }
            catch { throw; }

        }


        public bool SubmitSingleIGDStagingLineItem(int lineItemIDForSubmission, string eventTerminal, string eventUser, int principalID)
        {
			int submitted=0;

            try
            {
                bool retVal = false;

                IGDStaging staging = new IGDStaging();
                //we will get the igd staging object
                staging = IGDStaging.ReadObject(lineItemIDForSubmission);
                int InboundMasterLineItemID = Functions.SafeInt(staging.InboundMasterLineItemID);
                int ReceivedQuantity = Functions.SafeInt(staging.ReceivedQuantity, 0);
                //complete the item 
                staging = CompleteIGDItem(lineItemIDForSubmission, InboundMasterLineItemID, ReceivedQuantity, principalID);

                //if it has been submitted ... just return true
                if (staging.SubmittedToWMS == true )
                {
                    return true;
                }

                UpdateIGDStaging(staging, eventUser, eventTerminal);

                //call the web services
                int Res = WriteIGDToHost(staging);

                if (Res == 1)
                    staging.SubmittedToWMS = true;

                int recordUpdated = 0;
                recordUpdated = UpdateIGDStaging(staging, eventUser, eventTerminal);

                //AddCLCMessage(staging); 

                if (recordUpdated > 0)
                { retVal = true; }
                else
                { retVal = false; }

                return retVal;
            }
            catch { throw; }

        }

        public bool ReSubmitSingleIGDStagingLineItem(int lineItemIDForSubmission, string eventTerminal, string eventUser, int principalID)
        {
            
            try
            {
                bool retVal = false;

                IGDStaging staging = new IGDStaging();
                //we will get the igd staging object
                staging = IGDStaging.ReadObject(lineItemIDForSubmission);
                int InboundMasterLineItemID = Functions.SafeInt(staging.InboundMasterLineItemID);
                int ReceivedQuantity = Functions.SafeInt(staging.ReceivedQuantity, 0);
                //complete the item 
                staging = CompleteIGDItem(lineItemIDForSubmission, InboundMasterLineItemID, ReceivedQuantity, principalID);

                //if it has been submitted ... just return true
                staging.SubmittedToWMS = false;
            
                UpdateIGDStaging(staging, eventUser, eventTerminal);

                //call the web services
                if (WriteIGDToHost(staging) == 1)
                {
                    staging.SubmittedToWMS = true;
                }

                int recordUpdated = 0;
                recordUpdated = UpdateIGDStaging(staging, eventUser, eventTerminal);

                //AddCLCMessage(staging); 

                if (recordUpdated > 0)
                { retVal = true; }
                else
                { retVal = false; }

                return retVal;
            }
            catch { throw; }

        }



        public bool SubmitAllIGDStagingLineItem(int inboundlineItemIDForSubmission, string eventTerminal, string eventUser, int principalID)
        {

            try
            {
                bool retVal = false;

                IGDStaging staging = new IGDStaging();
                //we will get the igd staging object
                staging = IGDStaging.ReadObject(inboundlineItemIDForSubmission);
                int InboundMasterLineItemID = Functions.SafeInt(staging.InboundMasterLineItemID);
                int ReceivedQuantity = Functions.SafeInt(staging.ReceivedQuantity, 0);
                //complete the item 
                staging = CompleteIGDItem(inboundlineItemIDForSubmission, InboundMasterLineItemID, ReceivedQuantity, principalID);


                //if it has been submitted ... just return true
                if (staging.SubmittedToWMS == true)
                {
                    return true;
                }

                UpdateIGDStaging(staging, eventUser, eventTerminal);

                //call the web services
                WriteIGDToHost(staging);

                staging.SubmittedToWMS = true;

                int recordUpdated = 0;
                recordUpdated = UpdateIGDStaging(staging, eventUser, eventTerminal);

                if (recordUpdated > 0)
                { retVal = true; }
                else
                { retVal = false; }

                return retVal;
            }
            catch { throw; }

        }





        /// <summary>
        /// Adds an IGD Staging record
        /// </summary>
        /// <param name="stagingRecord"></param>
        /// <param name="principalID"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public int AddIGDStaging(IGDStaging stagingRecord, string eventUser, string eventTerminal, int principalID)
        {

            try { 
            int retVal = 0;

                //just make sure that we have a product id to link this with

              



                  
            retVal = IGDStaging.Save(stagingRecord.IGDStagingID, stagingRecord.InboundMasterLineItemID, stagingRecord.ProductStagingID,
                                        stagingRecord.RecordSource, stagingRecord.SiteCode, stagingRecord.PrincipalID,
                                        stagingRecord.PrincipalCode, stagingRecord.AnalysisA, stagingRecord.AnalysisB,
                                        stagingRecord.OrderLineNo, stagingRecord.ReceiptType, stagingRecord.PODate,
                                        stagingRecord.StockDateTime, stagingRecord.MoveRef, stagingRecord.PORef,
                                        stagingRecord.ExpectedDeliveryDateTime, stagingRecord.ReceivedQuantity, stagingRecord.SubmittedToWMS,
                                        eventTerminal, eventUser);

            //first we save .... then we complete ... then we update
            //IGDStaging staging = new IGDStaging();
            ////we will get the igd staging object
            //staging = IGDStaging.ReadObject(retVal);

            ////get the new id for this record ...
            //int igdStagingID = Functions.SafeInt(staging.IGDStagingID, 0);

            //int InboundMasterLineItemID = Functions.SafeInt(staging.InboundMasterLineItemID);
            //int ReceivedQuantity = Functions.SafeInt(staging.ReceivedQuantity, 0);
            //complete the item 
            //staging = CompleteIGDItem(0, InboundMasterLineItemID, ReceivedQuantity, principalID);
            //staging.IGDStagingID = igdStagingID;
            //int updated = 0;
            //updated = UpdateIGDStaging(staging, eventUser, eventTerminal);
       
            //retVal = updated;
           
            return retVal ;

            }
            catch { throw; }

        }


        /// <summary>
        /// Adds an IGD Staging record
        /// </summary>
        /// <param name="stagingRecord"></param>
        /// <param name="principalID"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public IGDStaging AddAndRetrieveIGDStaging(IGDStaging stagingRecord, string eventUser, string eventTerminal, int principalID)
        {

            try
            {
                IGDStaging retVal = new IGDStaging();
                int retInt = 0;

                retInt = IGDStaging.Save(stagingRecord.IGDStagingID, stagingRecord.InboundMasterLineItemID, stagingRecord.ProductStagingID,
                                            stagingRecord.RecordSource, stagingRecord.SiteCode, stagingRecord.PrincipalID,
                                            stagingRecord.PrincipalCode, stagingRecord.AnalysisA, stagingRecord.AnalysisB,
                                            stagingRecord.OrderLineNo, stagingRecord.ReceiptType, stagingRecord.PODate,
                                            stagingRecord.StockDateTime, stagingRecord.MoveRef, stagingRecord.PORef,
                                            stagingRecord.ExpectedDeliveryDateTime, stagingRecord.ReceivedQuantity, stagingRecord.SubmittedToWMS,
                                            eventTerminal, eventUser);

                //first we save .... then we complete ... then we update
                IGDStaging staging = new IGDStaging();
                //we will get the igd staging object
                staging = IGDStaging.ReadObject(retInt);

                //get the new id for this record ...
                int igdStagingID = Functions.SafeInt(staging.IGDStagingID, 0);

                int InboundMasterLineItemID = Functions.SafeInt(staging.InboundMasterLineItemID);
                int ReceivedQuantity = Functions.SafeInt(staging.ReceivedQuantity, 0);
                //complete the item 
                staging = CompleteIGDItem(0, InboundMasterLineItemID, ReceivedQuantity, principalID);
                staging.IGDStagingID = igdStagingID;
                int updated = 0;
                updated = UpdateIGDStaging(staging, eventUser, eventTerminal);

                retInt = updated;

                return staging;

            }
            catch { throw; }

        }

        #endregion

    }
}
