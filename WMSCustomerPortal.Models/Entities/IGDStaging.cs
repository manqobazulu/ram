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
    /// <summary>
    /// IGDStaging Class
    /// </summary>
    public class IGDStaging
    {
        #region Constants

    

        #endregion

        #region Stored Procedures

        private static readonly string STP_SAVE = "usp_IGDStaging_Save";
        private static readonly string STP_READ = "usp_IGDStaging_Read";
        private static readonly string STP_READ_ALL = "usp_IGDStaging_Read_All";
        private static readonly string STP_DELETE = "usp_IGDStaging_Delete";
        private static readonly string STP_CHECKDELETEABLE = "usp_IGDStaging_CheckDeletable";
        private static readonly string STP_UPDATE_PARTIAL_WAREHOUSE = "usp_IGDStaging_Warehouse_Partial_Update";
        private static readonly string STP_NEXT_MOVE_REF = "usp_IGDMoveRef_Read";
        private static readonly string  STP_READ_ALL_MASTER_RECORD = "usp_IGDStaging_Read_All_Master_Record";

        #endregion

        #region Properties

        private int mIGDStagingID = 0;
        public int IGDStagingID { get { return mIGDStagingID; } set { mIGDStagingID = value; } }

        private int? mInboundMasterLineItemID = 0;
        public int? InboundMasterLineItemID { get { return mInboundMasterLineItemID; } set { mInboundMasterLineItemID = value; } }

        private int? mProductStagingID = 0;
        public int? ProductStagingID { get { return mProductStagingID; } set { mProductStagingID = value; } }

        private string mRecordSource = "";
        public string RecordSource { get { return mRecordSource; } set { mRecordSource = value; } }

        private string mSiteCode = "";
        public string SiteCode { get { return mSiteCode; } set { mSiteCode = value; } }

        private int? mPrincipalID = 0;
        public int? PrincipalID { get { return mPrincipalID; } set { mPrincipalID = value; } }

        private string mPrincipalCode = "";
        public string PrincipalCode { get { return mPrincipalCode; } set { mPrincipalCode = value; } }

        private string mAnalysisA = "";
        public string AnalysisA { get { return mAnalysisA; } set { mAnalysisA = value; } }

        private string mAnalysisB = "";
        public string AnalysisB { get { return mAnalysisB; } set { mAnalysisB = value; } }

        private string mOrderLineNo = "";
        public string OrderLineNo { get { return mOrderLineNo; } set { mOrderLineNo = value; } }

        private string mReceiptType = "";
        public string ReceiptType { get { return mReceiptType; } set { mReceiptType = value; } }

        private DateTime? mPODate = new DateTime(1900, 1, 1);
        public DateTime? PODate { get { return mPODate; } set { mPODate = value; } }

        private DateTime? mStockDateTime = new DateTime(1900, 1, 1);
        public DateTime? StockDateTime { get { return mStockDateTime; } set { mStockDateTime = value; } }

        private string mMoveRef = "";
        public string MoveRef { get { return mMoveRef; } set { mMoveRef = value; } }

        private string mPORef = "";
        public string PORef { get { return mPORef; } set { mPORef = value; } }

        private DateTime? mExpectedDeliveryDateTime = new DateTime(1900, 1, 1);
        public DateTime? ExpectedDeliveryDateTime { get { return mExpectedDeliveryDateTime; } set { mExpectedDeliveryDateTime = value; } }

        private int? mReceivedQuantity = 0;
        public int? ReceivedQuantity { get { return mReceivedQuantity; } set { mReceivedQuantity = value; } }

        private bool? mSubmittedToWMS = false;
        public bool? SubmittedToWMS { get { return mSubmittedToWMS; } set { mSubmittedToWMS = value; } }

        private string mEventTerminal = "";
        public string EventTerminal { get { return mEventTerminal; } set { mEventTerminal = value; } }

        private DateTime mEventDate = new DateTime(1900, 1, 1);
        public DateTime EventDate { get { return mEventDate; } set { mEventDate = value; } }

        private string mEventUser = "";
        public string EventUser { get { return mEventUser; } set { mEventUser = value; } }

        private bool mDeleted = false;
        public bool Deleted { get { return mDeleted; } set { mDeleted = value; } }


       
        private string    mProdCode = "";
        public string ProdCode { get { return mProdCode.ToUpper(); } set { mProdCode = value.ToUpper(); } }
        private string    mEANCode = "";
        public string EANCode { get { return mEANCode.ToUpper(); } set { mEANCode = value.ToUpper(); } }
        private string	mShortDesc = "";
        public string ShortDesc { get { return mShortDesc.ToUpper(); } set { mShortDesc = value.ToUpper(); } }
        private string	mLongDesc = "";
        public string LongDesc { get { return mLongDesc.ToUpper(); } set { mLongDesc = value.ToUpper(); } }
        private bool	mSerialised = false;
        public bool Serialised { get { return mSerialised; } set { mSerialised = value; } }
        private double mUnitCost = 0.00;
        public double UnitCost { get { return mUnitCost; } set { mUnitCost = value; } }
        private double mSalesPrice = 0.00;
        public double SalesPrice { get { return mSalesPrice; } set { mSalesPrice = value; } }
        private bool	mExpiryProduct = false;
        public bool ExpiryProduct { get { return mExpiryProduct; } set { mExpiryProduct = value; } }
        private int	mLeadTimeDays = 0;
        public int LeadTimeDays { get { return mLeadTimeDays; } set { mLeadTimeDays = value; } }
            

           

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor for IGDStaging Class
        /// </summary>
        public IGDStaging()
        {

        }

        /// <summary>
        /// Constructor for IGDStaging Class with Initialization
        /// </summary>
        public IGDStaging(int pIGDStagingID, int? pInboundMasterLineItemID, int? pProductStagingID, string pRecordSource, string pSiteCode, int? pPrincipalID,
            string pPrincipalCode, string pAnalysisA, string pAnalysisB, string pOrderLineNo, string pReceiptType,
            DateTime? pPODate, DateTime? pStockDateTime, string pMoveRef, string pPORef, DateTime? pExpectedDeliveryDateTime,
            int? pReceivedQuantity, bool? pSubmittedToWMS, string pEventTerminal, string pEventUser,
            string pProdCode, 
            string pEANCode,
			string pShortDesc,
			string pLongDesc,
			bool pSerialised,
			double pUnitCost,
			double pSalesPrice,
			bool pExpiryProduct,
			int pLeadTimeDays
            
            )
        {
            IGDStagingID = pIGDStagingID;
            InboundMasterLineItemID = pInboundMasterLineItemID;
            ProductStagingID = pProductStagingID;
            RecordSource = pRecordSource;
            SiteCode = pSiteCode;
            PrincipalID = pPrincipalID;
            PrincipalCode = pPrincipalCode;
            AnalysisA = pAnalysisA;
            AnalysisB = pAnalysisB;
            OrderLineNo = pOrderLineNo;
            ReceiptType = pReceiptType;
            PODate = pPODate;
            StockDateTime = pStockDateTime;
            MoveRef = pMoveRef;
            PORef = pPORef;
            ExpectedDeliveryDateTime = pExpectedDeliveryDateTime;
            ReceivedQuantity = pReceivedQuantity;
            SubmittedToWMS = pSubmittedToWMS;
            EventTerminal = pEventTerminal;
            //EventDate = pEventDate;
            EventUser = pEventUser;
            //Deleted = pDeleted;


            ProdCode = pProdCode;
            EANCode = pEANCode;
            ShortDesc = pShortDesc;
            LongDesc = pLongDesc;
            Serialised = pSerialised;
            UnitCost = pUnitCost;
            SalesPrice = pSalesPrice;
            ExpiryProduct = pExpiryProduct;
            LeadTimeDays = pLeadTimeDays; 


        }

        /// <summary>
        /// Constructor for IGDStaging Class with Initialization
        /// </summary>
        public IGDStaging(DataRow loRow)
        {
            IGDStagingID = Functions.SafeInt(loRow["IGDStagingID"], 0);
            InboundMasterLineItemID = Functions.SafeInt(loRow["InboundMasterLineItemID"], 0);
            ProductStagingID = Functions.SafeInt(loRow["ProductStagingID"], 0);
            RecordSource = Functions.SafeString(loRow["RecordSource"]);
            SiteCode = Functions.SafeString(loRow["SiteCode"]);
            PrincipalID = Functions.SafeInt(loRow["PrincipalID"], 0);
            PrincipalCode = Functions.SafeString(loRow["PrincipalCode"]);
            AnalysisA = Functions.SafeString(loRow["AnalysisA"]);
            AnalysisB = Functions.SafeString(loRow["AnalysisB"]);
            OrderLineNo = Functions.SafeString(loRow["OrderLineNo"]);
            ReceiptType = Functions.SafeString(loRow["ReceiptType"]);
            PODate = Functions.SafeDateTime(loRow["PODate"]);
            StockDateTime = Functions.SafeDateTime(loRow["StockDateTime"]);
            MoveRef = Functions.SafeString(loRow["MoveRef"]);
            PORef = Functions.SafeString(loRow["PORef"]);
            ExpectedDeliveryDateTime = Functions.SafeDateTime(loRow["ExpectedDeliveryDateTime"]);
            ReceivedQuantity = Functions.SafeInt(loRow["ReceivedQuantity"], 0);
            SubmittedToWMS = Functions.SafeBool(loRow["SubmittedToWMS"]);
            EventTerminal = Functions.SafeString(loRow["EventTerminal"]);
            //EventDate = Functions.SafeDateTime(loRow["EventDate"]);
            EventUser = Functions.SafeString(loRow["EventUser"]);
            //Deleted = Functions.SafeBool(loRow["Deleted"]);


            ProdCode = Functions.SafeString(loRow["ProdCode"]);
            EANCode = Functions.SafeString(loRow["EANCode"]);
            ShortDesc = Functions.SafeString(loRow["ShortDesc"]);
            LongDesc = Functions.SafeString(loRow["LongDesc"]);
            Serialised = Functions.SafeBool(loRow["Serialised"]);
            UnitCost = Functions.SafeDouble(loRow["UnitCost"],0.00);
            SalesPrice = Functions.SafeDouble(loRow["SalesPrice"],0.00);
            ExpiryProduct = Functions.SafeBool(loRow["ExpiryProduct"]);
            LeadTimeDays = Functions.SafeInt(loRow["LeadTimeDays"],0);
        
        
        
        
        }

        #endregion

        #region Data Access - Private Methods

        private static List<IGDStaging> ReturnList(DataTable pdtTable)
        {
            List<IGDStaging> list = new List<IGDStaging>();
            try
            {
                foreach (DataRow loRow in pdtTable.Rows)
                {
                    list.Add(ReturnObject(loRow));
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static IGDStaging ReturnObject(DataRow loRow)
        {
            return new IGDStaging(Functions.SafeInt(loRow["IGDStagingID"], 0),
                Functions.SafeInt(loRow["InboundMasterLineItemID"], 0),
                Functions.SafeInt(loRow["ProductStagingID"], 0),
                Functions.SafeString(loRow["RecordSource"]),
                Functions.SafeString(loRow["SiteCode"]),
                Functions.SafeInt(loRow["PrincipalID"], 0),
                Functions.SafeString(loRow["PrincipalCode"]),
                Functions.SafeString(loRow["AnalysisA"]).ToUpper(),
                Functions.SafeString(loRow["AnalysisB"]).ToUpper(),
                Functions.SafeString(loRow["OrderLineNo"]),
                Functions.SafeString(loRow["ReceiptType"]),
                Functions.SafeDateTime(loRow["PODate"]),
                Functions.SafeDateTime(loRow["StockDateTime"]),
                Functions.SafeString(loRow["MoveRef"]).ToUpper(),
                Functions.SafeString(loRow["PORef"]).ToUpper(),
                Functions.SafeDateTime(loRow["ExpectedDeliveryDateTime"]),
                Functions.SafeInt(loRow["ReceivedQuantity"], 0),
                Functions.SafeBool(loRow["SubmittedToWMS"]),
                Functions.SafeString(loRow["EventTerminal"]),
                //Functions.SafeDateTime(loRow["EventDate"]),
                Functions.SafeString(loRow["EventUser"]),
                //Functions.SafeBool(loRow["Deleted"])

                Functions.SafeString(loRow["ProdCode"]).ToUpper(),
                Functions.SafeString(loRow["EANCode"]).ToUpper(),
                Functions.SafeString(loRow["ShortDesc"]).ToUpper(),
                Functions.SafeString(loRow["LongDesc"]).ToUpper(),
                Functions.SafeBool(loRow["Serialised"]),
                Functions.SafeDouble(loRow["UnitCost"],0.00),
                Functions.SafeDouble(loRow["SalesPrice"],0.00),
                Functions.SafeBool(loRow["ExpiryProduct"]),
                Functions.SafeInt(loRow["LeadTimeDays"],0)
              
                
                );
        }

        #endregion

        #region Data Access

        public static IGDStaging ReadObject(int pIGDStagingID)
        {
            DataRow loRow;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pIGDStagingID);

                loRow = db.ExecuteDataSet(dbCommand).Tables[0].Rows[0];

                if (loRow.Equals(null))
                {
                    return null;
                }
                else
                {
                    return ReturnObject(loRow);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int Save(int pIGDStagingID, int? pInboundMasterLineItemID, int? pProductStagingID, string pRecordSource, string pSiteCode, int? pPrincipalID, string pPrincipalCode, string pAnalysisA, string pAnalysisB, string pOrderLineNo, string pReceiptType, DateTime? pPODate, DateTime? pStockDateTime, string pMoveRef, string pPORef, DateTime? pExpectedDeliveryDateTime, int? pReceivedQuantity, bool? pSubmittedToWMS, string pEventTerminal, string pEventUser)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_SAVE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pIGDStagingID, pInboundMasterLineItemID, pProductStagingID, pRecordSource, pSiteCode, pPrincipalID, pPrincipalCode, pAnalysisA, pAnalysisB, pOrderLineNo, pReceiptType, pPODate, pStockDateTime, pMoveRef, pPORef, pExpectedDeliveryDateTime, pReceivedQuantity, pSubmittedToWMS, pEventTerminal, pEventUser);

                
                    db.ExecuteNonQuery(dbCommand);
                    return (int)db.GetParameterValue(dbCommand, "@IGDStagingID");
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static string GetNextIGDMoveRef(int principalID) {

            string dr = null;
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = STP_NEXT_MOVE_REF;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, principalID);

                dr = db.ExecuteScalar(dbCommand).ToString();

                return dr;
            }
            catch (Exception ex) {
                throw ex;
            }

        }



        public static int UpdatePartialIGDForWarehouse(int pIGDStagingID, int? pInboundMasterLineItemID, int? pProductStagingID, string pRecordSource, string pSiteCode, int? pPrincipalID, string pPrincipalCode, string pAnalysisA, string pAnalysisB, string pOrderLineNo, string pReceiptType, DateTime? pPODate, DateTime? pStockDateTime, string pMoveRef, string pPORef, DateTime? pExpectedDeliveryDateTime, int? pReceivedQuantity, bool? pSubmittedToWMS, string pEventTerminal, string pEventUser)
        
        {

            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = STP_UPDATE_PARTIAL_WAREHOUSE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pIGDStagingID, pInboundMasterLineItemID, pProductStagingID, pRecordSource, pSiteCode, pPrincipalID, pPrincipalCode, pAnalysisA, pAnalysisB, pOrderLineNo, pReceiptType, pPODate, pStockDateTime, pMoveRef, pPORef, pExpectedDeliveryDateTime, pReceivedQuantity, pSubmittedToWMS, pEventTerminal, pEventUser);

                return (int)db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        
        
        }

        public static int Update(int pIGDStagingID, int? pInboundMasterLineItemID, int? pProductStagingID, string pRecordSource, string pSiteCode, int? pPrincipalID, string pPrincipalCode, string pAnalysisA, string pAnalysisB, string pOrderLineNo, string pReceiptType, DateTime? pPODate, DateTime? pStockDateTime, string pMoveRef, string pPORef, DateTime? pExpectedDeliveryDateTime, int? pReceivedQuantity, bool? pSubmittedToWMS, string pEventTerminal, string pEventUser)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_SAVE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pIGDStagingID, pInboundMasterLineItemID, pProductStagingID, pRecordSource, pSiteCode, pPrincipalID, pPrincipalCode, pAnalysisA, pAnalysisB, pOrderLineNo, pReceiptType, pPODate, pStockDateTime, pMoveRef, pPORef, pExpectedDeliveryDateTime, pReceivedQuantity, pSubmittedToWMS, pEventTerminal, pEventUser);

                return (int)db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Checks to see whether this record is deletable
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        public static bool CheckDeletable(int recordID)
        {
            bool retVal = false;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = STP_CHECKDELETEABLE;

                //read the out parameter
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);


                db.AddInParameter(dbCommand, "@IGDStagingID", DbType.Int32, recordID);
                db.AddOutParameter(dbCommand, "@Deleteable", DbType.Byte, 1);

                db.ExecuteNonQuery(dbCommand);

                retVal = Functions.SafeBool(db.GetParameterValue(dbCommand, "@Deleteable"), false);




            }
            catch (Exception ex)
            {

                throw ex;
            }


            return retVal;

        }


        public static bool Delete(int pintID, string eventTerminal, string eventUser)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_DELETE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pintID, eventTerminal,eventUser);

                db.ExecuteNonQuery(dbCommand);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IDataReader Read(int pintID)
        {
            IDataReader dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pintID);

                dr = db.ExecuteReader(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IDataReader ReadAll()
        {
            IDataReader dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                dr = db.ExecuteReader(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet ReadAllDSForMasterID(int inboundMasterLineID)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ_ALL_MASTER_RECORD;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, inboundMasterLineID);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


        public static DataSet ReadAllDS()
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<IGDStaging> ReadAll_List()
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                return ReturnList(db.ExecuteDataSet(dbCommand).Tables[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
