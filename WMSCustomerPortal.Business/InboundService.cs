using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSCustomerPortal.Models.Entities;
using System.Data;

namespace WMSCustomerPortal.Business
{
    public class InboundService
    {

        #region Inbound Master


        /// <summary>
        /// Returns a Dynamic List  of Inbound Master Objects which have been filtered
        /// </summary>
        /// <param name="principalID"></param>
        /// <param name="poRef"></param>
        /// <param name="fromxDate"></param>
        /// <param name="toxDate"></param>
        /// <returns></returns>
        public List<dynamic>  GetAllInboundMasterFilter(int principalID, string poRef, System.DateTime fromxDate, System.DateTime toxDate)
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
        /// Returns a Dynamic List  of Inbound Master Objects which have been filtered and have been completed
        /// </summary>
        /// <param name="principalID"></param>
        /// <param name="poRef"></param>
        /// <param name="fromxDate"></param>
        /// <param name="toxDate"></param>
        /// <returns></returns>
        public List<dynamic>  GetAllInboundMasterFilterCompleted(int principalID, string poRef, System.DateTime fromxDate, System.DateTime toxDate)
        {
            try { 
            System.Data.DataSet retValue = new DataSet();
            retValue = InboundMaster.ReadAllCompletedFilter(principalID, poRef, fromxDate, toxDate);

            var result = retValue.Tables[0].ToDynamicList("InboundMaster");

            return result;
            }
            catch { throw; }
        }



        /// <summary>
        /// Returns alist of inbound items according to search criteria
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="poRef"></param>
        /// <param name="principalID"></param>
        /// <returns></returns>
        public static List<InboundMaster> SearchInboundList(System.DateTime fromDate, System.DateTime toDate , string poRef, int principalID)
        {
            try { 
            List<InboundMaster> dt = new List<InboundMaster>();
            if( toDate <= new DateTime())
            {
                toDate = System.DateTime.Now;

            }

            if(fromDate <= new DateTime())
            {

                fromDate = new DateTime(1970,1,1);

            }

            dt = InboundMaster.FindInboundList(fromDate, toDate , poRef, principalID);

            return dt;
            }
            catch { throw; }

        }

        /// <summary>
        /// Returns a InboundMaster object based on the ID
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        public  InboundMaster GetInboundMaster(int recordID )
        {
            try { 
            InboundMaster InboundStaging = new InboundMaster();
            InboundStaging = InboundMaster.ReadObject(recordID);
            return InboundStaging;
            }
            catch { throw; }
        }



        /// <summary>
        /// Returns a Dataset of all the Inbound Master objects based on a principal ID
        /// </summary>
        /// <param name="principalID"></param>
        /// <returns></returns>
        public static System.Data.DataSet GetAllInboundMaster(int principalID)
        {
            try { 
            System.Data.DataSet retValue = new DataSet();
            retValue = InboundMaster.ReadAllDS(principalID);

            return retValue;
            }
            catch { throw; }
        }

        /// <summary>
        /// Gets all the Inbound master records based on a Principal ID and Purchase Order Reference
        /// </summary>
        /// <param name="principalID"></param>
        /// <param name="purchaseOrderReference"></param>
        /// <returns></returns>
        public static System.Data.DataSet GetAllInboundMasterPO(int principalID, string purchaseOrderReference)
        {
            try { 
            System.Data.DataSet retValue = new DataSet();
            retValue = InboundMaster.ReadAllPO(principalID, purchaseOrderReference);

            return retValue;
            }
            catch { throw; }
        }


        

        /// <summary>
        /// Returns a Dataset of Inbound Master Objects which have been filtered
        /// </summary>
        /// <param name="principalID"></param>
        /// <param name="poRef"></param>
        /// <param name="fromxDate"></param>
        /// <param name="toxDate"></param>
        /// <returns></returns>
         public static System.Data.DataSet GetAllInboundMasterFilterDS(int principalID, string poRef, System.DateTime fromxDate, System.DateTime toxDate)
        {
             try{
            System.Data.DataSet retValue = new DataSet();
            retValue = InboundMaster.ReadAllFilter(principalID, poRef, fromxDate, toxDate);

            return retValue;
             }
            catch { throw; }
        }

        /// <summary>
        /// Updates the Inbound master object
        /// </summary>
        /// <param name="stagingRecord"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public static bool UpdateInboundMaster(InboundMaster stagingRecord,  string eventUser, string eventTerminal)
        {
            try { 
            bool retVal = false;

            retVal = InboundMaster.Update(stagingRecord, eventTerminal, eventUser);

           return retVal;
            }
            catch { throw; }
        }

        /// <summary>
        /// Deletes the Inbound Master object
        /// </summary>
        /// <param name="recordID"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public static bool DeleteInboundMaster(int recordID, string eventUser, string eventTerminal)
        {
            try { 
            bool retVal = false;
           // errorResult = "";
            bool deleteable = false;
            
            if (InboundMaster.CheckDeletable(recordID) == true)
            {

                deleteable = true;

            }
            else
            {
              //  errorResult = "Cannot delete while there are IGD Messages or Inbound Line Items on record.";
                deleteable = false;
            }
           
            if(deleteable)
            {

             retVal = InboundMaster.Delete(recordID, eventTerminal, eventUser);
            }
            else
            {
                retVal = false;

            }

            return retVal;
            }
            catch { throw; }
        }


        /// <summary>
        /// Adds a new Inbound Master Record
        /// </summary>
        /// <param name="stagingRecord"></param>
        /// <param name="principalID"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public static int AddInboundMaster(InboundMaster stagingRecord, string eventUser, string eventTerminal)
        {
            try { 
            int retVal = 0;
        
            retVal = InboundMaster.Save(stagingRecord, eventTerminal, eventUser);

            return retVal;
            }
            catch { throw; }

        }

        public int SaveNewOrderWithOrderLine(InboundMaster stagingRecord, string pEventTerminal, string pEventUser, string pTable) {

            int retVal = 0;
            InboundMaster staging = new InboundMaster();

            retVal = InboundMaster.SaveNewOrderWithOrderLine(stagingRecord, pEventTerminal, pEventUser, pTable);

            return retVal;

        }

      

        #endregion


        #region Inbound Master Line Items

        public static bool CheckDeletable(int inboundMasterLineItemID)
        {
            
            if(inboundMasterLineItemID == 0)
            {
                return true; //we can delete this, because it has not been submitted to the database
            }
            
            if (InboundMasterLineItem.CheckDeletable(inboundMasterLineItemID) == true)
            {

                return  true;
            }
            else
            {
                return  false;

            }
        }
        
        /// <summary>
        /// Returns a dynamic list of the Line Items specifically for the inbound master id
        /// </summary>
        /// <param name="inboundMasterID"></param>
        /// <returns></returns>
       public List<dynamic>  GetInboundMasterLineItemsList(int inboundMasterID)
        {

            try
            {

            System.Data.DataSet retValue = new DataSet();

            retValue = InboundMasterLineItem.ReadAllDS(inboundMasterID);

            var result = retValue.Tables[0].ToDynamicList("InboundMasterLineItem");

            return result;
            }
            catch { throw; }
           
        }

      
        /// <summary>
        /// Returns a single Inbound Master Line Item List
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        public static InboundMasterLineItem GetInboundMasterLineItem(int recordID)
        {
            try { 
            InboundMasterLineItem InboundStaging = new InboundMasterLineItem();
            InboundStaging = InboundMasterLineItem.ReadObject(recordID);
            return InboundStaging;
            }
            catch { throw; }

        }

        /// <summary>
        /// Retrieves a dataset of all the inbound master Line items for a specific inbound master record
        /// </summary>
        /// <param name="inboundMasterID"></param>
        /// <returns></returns>
        public static System.Data.DataSet GetAllInboundMasterLineItem(int inboundMasterID)
        {
            try { 
            System.Data.DataSet retValue = new DataSet();
            retValue = InboundMasterLineItem.ReadAllDS(inboundMasterID);

            return retValue;
            }
            catch { throw; }

        }


        /// <summary>
        /// Updates a specific InboundMasterLineItem object
        /// </summary>
        /// <param name="stagingRecord"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public static int UpdateInboundMasterLineItem(InboundMasterLineItem stagingRecord, string eventUser, string eventTerminal)
        {
            try { 
            int retVal = 0;

            retVal = InboundMasterLineItem.Update(stagingRecord, eventUser, eventTerminal);

            return retVal;
            }
            catch { throw; }
        }


        /// <summary>
        /// Delets a specific Inbound Master Line Item
        /// </summary>
        /// <param name="recordID"></param>
        /// <param name="eventTerminal"></param>
        /// <param name="eventUser"></param>
        /// <param name="errorResult"></param>
        /// <returns></returns>
        public static bool DeleteInboundMasterLineItem(int recordID, string eventTerminal, string eventUser)
        {
            try { 
            bool retVal = false;
            // errorResult = "";
            bool deleteable = false;
            //check to see whether this record may be deleted

            //get all the 
            //inbound lineitems where warehouse actions have been performed on
            System.Data.DataSet dsInboundLineItems = new DataSet();
            dsInboundLineItems = GetAllInboundMasterLineItem(recordID);
            
            if(InboundMasterLineItem.CheckDeletable(recordID) == true)
            {

                deleteable = true;
            }
            else
            {
                deleteable = false;

            }

            if (deleteable)
            {

                retVal = InboundMasterLineItem.Delete(recordID, eventTerminal, eventUser);
            }
            else
            {
                retVal = false;

            }

            return retVal;
            }
            catch { throw; }
        }


        /// <summary>
        /// Adds a new Inbound Master Line Item
        /// </summary>
        /// <param name="stagingRecord"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public static int AddInboundMasterLineItem(InboundMasterLineItem stagingRecord, string eventUser, string eventTerminal)
        {
            try { 
            int retVal = 0;
            //InboundMaster InboundStaging = new InboundMaster(stagingRecord, principalID, eventUser, eventTerminal);
            retVal = InboundMasterLineItem.Save(stagingRecord, eventTerminal, eventUser);

            return retVal;
            }
            catch { throw; }
        }


        #endregion

    }
}
