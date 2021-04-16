
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Practices.EnterpriseLibrary;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using RAM.Utilities.Common;

namespace WMSCustomerPortal.Models.Entities
{
	public class Returns
    {
	
        #region Properties

        private int mReturnOrderID = 0;
        public int ReturnOrderID { get { return mReturnOrderID; } set { mReturnOrderID = value; } }

        private int mCustomerDetailID = 0;
        public int CustomerDetailID { get { return mCustomerDetailID; } set { mCustomerDetailID = value; } }

        private int mBillingCustomerDetailID = 0;
        public int BillingCustomerDetailID { get { return mBillingCustomerDetailID; } set { mBillingCustomerDetailID = value; } }

        private string mReceiverRAMCustomerID = "";
        public string ReceiverRAMCustomerID { get { return mReceiverRAMCustomerID; } set { mReceiverRAMCustomerID = value; } }

        private string mBillingRAMCustomerID = "";
        public string BillingRAMCustomerID { get { return mBillingRAMCustomerID; } set { mBillingRAMCustomerID = value; } }

        private int mPrincipalID = 0;
        public int PrincipalID { get { return mPrincipalID; } set { mPrincipalID = value; } }

        private string mRAMOrderNumber = "";
        public string RAMOrderNumber { get { return mRAMOrderNumber.ToUpper(); } set { mRAMOrderNumber = value.ToUpper(); } }

        private string mReturnWaybillNumber = "";
        public string ReturnWaybillNumber { get { return mReturnWaybillNumber.ToUpper(); } set { mReturnWaybillNumber = value.ToUpper(); } }

        private string mClientERPStockNumber = "";
        public string ClientERPStockNumber { get { return mClientERPStockNumber.ToUpper(); } set { mClientERPStockNumber = value.ToUpper(); } }

        private DateTime? mDateCreated = new DateTime(1900, 1, 1);
        public DateTime? DateCreated { get { return mDateCreated; } set { mDateCreated = value; } }

        private string mCustomerOrderNumber = "";
        public string CustomerOrderNumber { get { return mCustomerOrderNumber.ToUpper(); } set { mCustomerOrderNumber = value.ToUpper(); } }

        private string mPriority = "";
        public string Priority { get { return mPriority; } set { mPriority = value; } }

        private bool? mValueAddedPackaging = false;
        public bool? ValueAddedPackaging { get { return mValueAddedPackaging; } set { mValueAddedPackaging = value; } }

        private string mSalesPerson = "";
        public string SalesPerson { get { return mSalesPerson.ToUpper(); } set { mSalesPerson = value.ToUpper(); } }

        private string mSalesCategory = "";
        public string SalesCategory { get { return mSalesCategory.ToUpper(); } set { mSalesCategory = value.ToUpper(); } }

        private string mReturnType = "";
        public string ReturnType { get { return mReturnType.ToUpper(); } set { mReturnType = value.ToUpper(); } }

        private string mProcessor = "";
        public string Processor { get { return mProcessor; } set { mProcessor = value; } }

        private double? mOrderDiscount = 0;
        public double? OrderDiscount { get { return mOrderDiscount; } set { mOrderDiscount = value; } }

        private double? mOrderVAT = 0;
        public double? OrderVAT { get { return mOrderVAT; } set { mOrderVAT = value; } }

        private string mEventUser = "";
        public string EventUser { get { return mEventUser; } set { mEventUser = value; } }

        private string mEventTerminal = "";
        public string EventTerminal { get { return mEventTerminal; } set { mEventTerminal = value; } }

        private string mRAMCustomerID = "";
        public string RAMCustomerID { get { return mRAMCustomerID.ToUpper(); } set { mRAMCustomerID = value.ToUpper(); } }

        private string mStoreCode = "";
        public string StoreCode { get { return mStoreCode.ToUpper(); } set { mStoreCode = value.ToUpper(); } }

        private string mCustomerName = "";
        public string CustomerName { get { return mCustomerName.ToUpper(); } set { mCustomerName = value.ToUpper(); } }

        private string mCustomerGroupID = "";
        public string CustomerGroupID { get { return mCustomerGroupID; } set { mCustomerGroupID = value; } }


        #endregion

        #region Constructors

        public Returns()
        {

        }


        public Returns(int pReturnOrderID, int pCustomerDetailID, int pBillingCustomerDetailID, string pReceiverRAMCustomerID, string pBillingRAMCustomerID, int pPrincipalID, string pRAMOrderNumber, string pReturnWaybillNumber, string pClientERPStockNumber, DateTime? pDateCreated,
            string pCustomerOrderNumber, string pPriority, bool? pValueAddedPackaging, string pSalesPerson,
            string pSalesCategory, string pReturnType, string pProcessor, double? pOrderDiscount, double? pOrderVAT, string pEventUser, string pEventTerminal,
            string pRAMCustomerID, string pStoreCode, string pCustomerName, string pCustomerGroupID )
        {
            ReturnOrderID = pReturnOrderID;
            CustomerDetailID = pCustomerDetailID;
            BillingCustomerDetailID = pBillingCustomerDetailID;
            ReceiverRAMCustomerID = pReceiverRAMCustomerID;
            BillingRAMCustomerID = pBillingRAMCustomerID;
            PrincipalID = pPrincipalID;
            RAMOrderNumber = pRAMOrderNumber;
            ReturnWaybillNumber = pReturnWaybillNumber;
            ClientERPStockNumber = pClientERPStockNumber;
            DateCreated = pDateCreated;
            CustomerOrderNumber = pCustomerOrderNumber;
            Priority = pPriority;
            ValueAddedPackaging = pValueAddedPackaging;
            SalesPerson = pSalesPerson;
            SalesCategory = pSalesCategory;
            ReturnType = pReturnType;
            Processor = pProcessor;
            OrderDiscount = pOrderDiscount;
            OrderVAT = pOrderVAT;
            EventUser = pEventUser;
            EventTerminal = pEventTerminal;
            RAMCustomerID = pRAMCustomerID;
            StoreCode = pStoreCode;
            CustomerName = pCustomerName;
            CustomerGroupID = pCustomerGroupID;
        }

        public Returns(DataRow loRow)
        {
            ReturnOrderID = Functions.SafeInt(loRow["ReturnOrderID"], 0);
            CustomerDetailID = Functions.SafeInt(loRow["CustomerDetailID"], 0);
            BillingCustomerDetailID = Functions.SafeInt(loRow["BillingCustomerDetailID"], 0);
            ReceiverRAMCustomerID = Functions.SafeString(loRow["ReceiverRAMCustomerID"]);
            BillingRAMCustomerID = Functions.SafeString(loRow["BillingRAMCustomerID"]);
            PrincipalID = Functions.SafeInt(loRow["PrincipalID"], 0);
            RAMOrderNumber = Functions.SafeString(loRow["RAMOrderNumber"]);
            ReturnWaybillNumber = Functions.SafeString(loRow["ReturnWaybillNumber"]);
            ClientERPStockNumber = Functions.SafeString(loRow["ClientERPStockNumber"]);
            DateCreated = Functions.SafeDateTime(loRow["DateCreated"]);
            CustomerOrderNumber = Functions.SafeString(loRow["CustomerOrderNumber"]);
            Priority = Functions.SafeString(loRow["Priority"]);
            ValueAddedPackaging = Functions.SafeBool(loRow["ValueAddedPackaging"]);
            SalesPerson = Functions.SafeString(loRow["SalesPerson"]);
            SalesCategory = Functions.SafeString(loRow["SalesCategory"]);
            ReturnType = Functions.SafeString(loRow["ReturnType"]);
            Processor = Functions.SafeString(loRow["Processor"]);
            OrderDiscount = Functions.SafeDouble(loRow["OrderDiscount"], 0);
            OrderVAT = Functions.SafeDouble(loRow["OrderVAT"], 0);
            EventUser = Functions.SafeString(loRow["EventUser"]);
            EventTerminal = Functions.SafeString(loRow["EventTerminal"]);
            RAMCustomerID = Functions.SafeString(loRow["RAMCustomerID"]);
            StoreCode = Functions.SafeString(loRow["StoreCode"]);
            CustomerName = Functions.SafeString(loRow["CustomerName"]);
            CustomerGroupID = Functions.SafeString(loRow["CustomerGroupID"]);
        }

        #endregion

        #region Data Access

        public static int SaveNewReturnOrder(Returns stagingRecord, string EventTerminal, string EventUser, string OrderLineItems)
		{
			try
			{
                int count;
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_ReturnOrders_Save";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, stagingRecord.ReturnOrderID, stagingRecord.RAMOrderNumber, stagingRecord.PrincipalID, stagingRecord.ReturnType, EventUser, EventTerminal, OrderLineItems);			        
                count = Convert.ToInt32(db.ExecuteScalar(dbCommand));
                return count;
			}        
			catch (Exception ex)
			{
				throw ex;
			} 	
		}

        public static int UpdateReturnReceiptLine(int lineItemID, int quantity , string returnWaybill, string EventTerminal, string EventUser)
        {
            string recordSource = RAM.Utilities.Common.ConfigSettings.ReadConfigValue("WMSRecordSource", "UNKNOWN");
            string siteCode = RAM.Utilities.Common.ConfigSettings.ReadConfigValue("WMSSiteCode", "000");
            try
            {
                int count;
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_ReturnReceiptLineItems_Update";

                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, lineItemID, quantity, returnWaybill, EventUser, EventTerminal, recordSource, siteCode);
                count = Convert.ToInt32(db.ExecuteScalar(dbCommand));
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet ReadAllReturnReceipt(int PrincipalID)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_ReturnReceipt_Read_All";

                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, PrincipalID);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPrincipalsForPrincipalGroup(int PrincipalID)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_ReturnPrincipalsForGroup";

                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, PrincipalID);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet ReadReturnReceiptLineItems(string orderNumber)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_ReturnReceiptListItems";
               
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, orderNumber);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public DataSet ReadReturnReceiptLineItems_by_IGD(int IGDStaging)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_ReturnReceiptListItems_by_IGD";

                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, IGDStaging);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet ReadReturnReceiptLineItems_All(int PrincipalID)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_ReturnReceiptListItems_Read_All";

                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, PrincipalID);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
