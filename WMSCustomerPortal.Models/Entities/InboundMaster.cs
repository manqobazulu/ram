using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using RAM.Utilities.Common;

namespace WMSCustomerPortal.Models.Entities
{
    /// <summary>
    /// IncomingMaster Class
    /// </summary>
     [Serializable]
    public class InboundMaster
    {
        #region Constants


        #endregion

        #region Stored Procedures

        private static readonly string STP_SAVE = "usp_InboundMaster_Save";
        private static readonly string STP_READ = "usp_InboundMaster_Read";
        private static readonly string STP_READ_ALL = "usp_InboundMaster_Read_All";
        private static readonly string STP_DELETE = "usp_InboundMaster_Delete";
        private static readonly string STP_FIND_INBOUND_FILTER = "usp_InboundMaster_Filter";
        private static readonly string STP_READ_ALL_WAREHOUSE_FILTER = "usp_InboundMaster_Read_All_Warehouse";
        private static readonly string STP_READ_ALL_PURCHASE_ORDER_FILTER = "usp_InboundMaster_Read_All_PO_Filter";
        private static readonly string STP_READ_ALL_FILTER = "usp_InboundMaster_Read_All_Filter";
        private static readonly string STP_READ_ALL_COMPLETED_FILTER = "usp_InboundMaster_Read_All_Completed_Filter";
        private static readonly string STP_CHECKDELETEABLE = "usp_InboundMaster_CheckDeletable";
        private static readonly string STP_ADD_NEW = "usp_InboundMaster_AddNew_Save";
        private static readonly string INBOUNDRPT_READ_ALL = "usp_RPT_Activity_Inbound_Detail";
        private static readonly string RETURNRPT_READ_ALL = "usp_RPT_Activity_Return_Detail";

        #endregion

        #region Properties

        private int mInboundMasterID = 0;
        public int InboundMasterID { get { return mInboundMasterID; } set { mInboundMasterID = value; } }

        private string mRecordSource = "";
        public string RecordSource { get { return mRecordSource; } set { mRecordSource = value; } }

        private string mSiteCode = "";
        public string SiteCode { get { return mSiteCode; } set { mSiteCode = value; } }

        private int? mPrincipalID = 0;
        public int? PrincipalID { get { return mPrincipalID; } set { mPrincipalID = value; } }

        private string mPrincipalCode = "";
        public string PrincipalCode { get { return mPrincipalCode; } set { mPrincipalCode = value; } }

        private string mSupplierName = "";
        public string SupplierName { get { return mSupplierName; } set { mSupplierName = value; } }

        private DateTime? mPODate = new DateTime(1900, 1, 1);
        public DateTime? PODate { get { return mPODate; } set { mPODate = value; } }

        private string mPORef = "";
        public string PORef { get { return mPORef.ToUpper(); } set { mPORef = value.ToUpper(); } }

        private DateTime? mExpectedDeliveryDateTime = new DateTime(1900, 1, 1);
        public DateTime? ExpectedDeliveryDateTime { get { return mExpectedDeliveryDateTime; } set { mExpectedDeliveryDateTime = value; } }

        private string mEventTerminal = "";
        public string EventTerminal { get { return mEventTerminal; } set { mEventTerminal = value; } }

        private DateTime mEventDate = new DateTime(1900, 1, 1);
        public DateTime EventDate { get { return mEventDate; } set { mEventDate = value; } }

        private string mEventUser = "";
        public string EventUser { get { return mEventUser; } set { mEventUser = value; } }

        private bool mDeleted = false;
        public bool Deleted { get { return mDeleted; } set { mDeleted = value; } }


        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor for IncomingMaster Class
        /// </summary>
        public InboundMaster()
        {

        }

        /// <summary>
        /// Constructor for IncomingMaster Class with Initialization
        /// </summary>
        public InboundMaster(int pInboundMasterID, string pRecordSource, string pSiteCode, int? pPrincipalID, string pPrincipalCode, string pSupplierName, DateTime? pPODate, string pPORef, DateTime? pExpectedDeliveryDateTime, string pEventTerminal, string pEventUser)
        {
            InboundMasterID = pInboundMasterID;
            RecordSource = pRecordSource;
            SiteCode = pSiteCode;
            PrincipalID = pPrincipalID;
            PrincipalCode = pPrincipalCode;
            SupplierName = pSupplierName;
            PODate = pPODate;
            PORef = pPORef;
            ExpectedDeliveryDateTime = pExpectedDeliveryDateTime;
            EventTerminal = pEventTerminal;
          
            EventUser = pEventUser;
         
        }

        /// <summary>
        /// Constructor for IncomingMaster Class with Initialization
        /// </summary>
        public InboundMaster(DataRow loRow)
        {
            InboundMasterID = Functions.SafeInt(loRow["InboundMasterID"], 0);
            RecordSource = Functions.SafeString(loRow["RecordSource"]);
            SiteCode = Functions.SafeString(loRow["SiteCode"]);
            PrincipalID = Functions.SafeInt(loRow["PrincipalID"], 0);
            PrincipalCode = Functions.SafeString(loRow["PrincipalCode"]);
            SupplierName = Functions.SafeString(loRow["SupplierName"]);
            PODate = Functions.SafeDateTime(loRow["PODate"]);
            PORef = Functions.SafeString(loRow["PORef"]);
            ExpectedDeliveryDateTime = Functions.SafeDateTime(loRow["ExpectedDeliveryDateTime"]);
            EventTerminal = Functions.SafeString(loRow["EventTerminal"]);
         
            EventUser = Functions.SafeString(loRow["EventUser"]);
            
        }

       

        #endregion

        #region Data Access - Private Methods

        private static List<InboundMaster> ReturnList(DataTable pdtTable)
        {
            List<InboundMaster> list = new List<InboundMaster>();
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

        private static InboundMaster ReturnObject(DataRow loRow)
        {
            return new InboundMaster(Functions.SafeInt(loRow["InboundMasterID"], 0),
                Functions.SafeString(loRow["RecordSource"]),
                Functions.SafeString(loRow["SiteCode"]),
                Functions.SafeInt(loRow["PrincipalID"], 0),
                Functions.SafeString(loRow["PrincipalCode"]),
                Functions.SafeString(loRow["SupplierName"]),
                Functions.SafeDateTime(loRow["PODate"]),
                Functions.SafeString(loRow["PORef"]).ToUpper(),
                Functions.SafeDateTime(loRow["ExpectedDeliveryDateTime"]),
                Functions.SafeString(loRow["EventTerminal"]),
             
                Functions.SafeString(loRow["EventUser"])
                );
        }

        #endregion

        #region Data Access

        public static InboundMaster ReadObject(int pInboundMasterID)
        {
            DataRow loRow;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pInboundMasterID);

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

        public static int Save(int pInboundMasterID, string pRecordSource, string pSiteCode, int? pPrincipalID, string pSupplierName, DateTime? pPODate, string pPORef, DateTime? pExpectedDeliveryDateTime, string pEventTerminal, DateTime pEventDate, string pEventUser, bool pDeleted)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_SAVE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pInboundMasterID,  pRecordSource, pSiteCode, pPrincipalID, pSupplierName, pPODate, pPORef, pExpectedDeliveryDateTime, pEventTerminal, pEventDate, pEventUser, pDeleted);

                return (int)db.ExecuteScalar(dbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int SaveNewOrderWithOrderLine(InboundMaster stagingRecord, string pEventTerminal, string pEventUser, string pTable) {

            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = STP_ADD_NEW;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand,
                    stagingRecord.InboundMasterID,
                    stagingRecord.RecordSource,
                    stagingRecord.SiteCode,
                    stagingRecord.PrincipalID,
                    stagingRecord.SupplierName,
                    stagingRecord.PODate,
                    stagingRecord.PORef,
                    stagingRecord.ExpectedDeliveryDateTime,
                    pEventTerminal,
                    pEventUser, pTable
                  );

                return (int)db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public static int Save(InboundMaster stagingRecord, string eventUser, string eventTerminal)
        {
            int retVal = 0;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_SAVE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand,
                    stagingRecord.InboundMasterID,
                    stagingRecord.RecordSource,
                    stagingRecord.SiteCode,
                    stagingRecord.PrincipalID,
                    stagingRecord.SupplierName,
                    stagingRecord.PODate,
                    stagingRecord.PORef,
                    stagingRecord.ExpectedDeliveryDateTime,
                    eventTerminal,

                    eventUser
                  );

            
                db.ExecuteNonQuery(dbCommand);
                return (int)db.GetParameterValue(dbCommand, "@InboundMasterID");
               
               
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

         public static bool Update(InboundMaster stagingRecord, string eventUser, string eventTerminal)
        {
            bool retVal = false;

            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_SAVE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand,
                    stagingRecord.InboundMasterID,
                    stagingRecord.RecordSource,
                    stagingRecord.SiteCode,
                    stagingRecord.PrincipalID,
                    stagingRecord.SupplierName,
                    stagingRecord.PODate,
                    stagingRecord.PORef,
                    stagingRecord.ExpectedDeliveryDateTime,
                    eventTerminal,
                   
                    eventUser
                  );

                int recordsAffected =  (int)db.ExecuteNonQuery(dbCommand);

                if(recordsAffected > 0)
                {

                    retVal = true;

                }
                return retVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


        public static int Update(int pInboundMasterID, int pProductStagingID, string pRecordSource, string pSiteCode, int? pPrincipalID, string pSupplierName, DateTime? pPODate, string pPORef, DateTime? pExpectedDeliveryDateTime, string pEventTerminal, string pEventUser)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_SAVE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pInboundMasterID, pProductStagingID, pRecordSource, pSiteCode, pPrincipalID, pSupplierName ,pPODate, pPORef, pExpectedDeliveryDateTime, pEventTerminal, pEventUser );

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


                db.AddInParameter(dbCommand, "@InboundMasterID", DbType.Int32, recordID);
                db.AddOutParameter(dbCommand,  "@Deleteable", DbType.Byte, 1);

                db.ExecuteNonQuery(dbCommand);

                retVal = Functions.SafeBool(db.GetParameterValue(dbCommand, "@Deleteable"),false);

              
                

            }
             catch(Exception ex)
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
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pintID, eventTerminal, eventUser);

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

        public static IDataReader ReadAll(int principalID)
        {
            IDataReader dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, principalID);

                dr = db.ExecuteReader(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet ReadAllPO(int principalID, string purchaseOrderReference)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ_ALL_PURCHASE_ORDER_FILTER;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, principalID, purchaseOrderReference);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


         /// <summary>
         /// Returns the recordsets which have been completed
         /// </summary>
         /// <param name="principalID"></param>
         /// <param name="purchaseOrderReference"></param>
         /// <param name="fromDate"></param>
         /// <param name="toDate"></param>
         /// <returns></returns>
        public static DataSet ReadAllCompletedFilter(int principalID, string purchaseOrderReference, System.DateTime fromDate, System.DateTime toDate)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ_ALL_COMPLETED_FILTER;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, principalID, purchaseOrderReference, fromDate, toDate);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet ReadAllFilter(int principalID, string purchaseOrderReference, System.DateTime fromDate, System.DateTime toDate)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ_ALL_FILTER;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, principalID, purchaseOrderReference, fromDate, toDate);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      public static DataSet ReadAllDSWarehouse(string poFilter ,int principalID)
        {

            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ_ALL_WAREHOUSE_FILTER;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, poFilter, principalID);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }

        public static DataSet ReadAllDS(int principalID)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, principalID);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static List<InboundMaster> FindInboundList(System.DateTime fromDate, System.DateTime toDate ,string  poRef, int principalID)
        {
            return ReturnList(FindInboundDT(fromDate, toDate, poRef, principalID));


        }


        public static System.Data.DataTable FindInboundDT(System.DateTime fromDate, System.DateTime toDate, string poRef, int principalID)
        {

            DataTable dt = new DataTable();



            try
            {
                
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_FIND_INBOUND_FILTER;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddInParameter(dbCommand, "@FromDate", DbType.Date, fromDate);
                db.AddInParameter(dbCommand, "@ToDate", DbType.Date, toDate);
                db.AddInParameter(dbCommand, "@POREf", DbType.String, poRef);
             
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);


                dt = db.ExecuteDataSet(dbCommand).Tables[0];



                return dt;

            }
            catch (Exception ex)
            {

                return null;
            }
        }


        public static List<InboundMaster> ReadAll_List(int principalID)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, principalID);

                return ReturnList(db.ExecuteDataSet(dbCommand).Tables[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<dynamic> GetInbound(int principalID, DateTime DateFrom, DateTime DateTo, string UserName)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = INBOUNDRPT_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);
                db.AddInParameter(dbCommand, "@DateFrom", DbType.Date, DateFrom);
                db.AddInParameter(dbCommand, "@DateTo", DbType.Date, DateTo);
                db.AddInParameter(dbCommand, "@UserName", DbType.String, UserName);

                //Set my timeout to 5 minutes
                dbCommand.CommandTimeout = 420;

                dr = db.ExecuteDataSet(dbCommand);

                return dr.Tables[0].ToDynamicList("Inbound");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public List<dynamic> GetReturns(int principalID, DateTime DateFrom, DateTime DateTo, string UserName)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = RETURNRPT_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);
                db.AddInParameter(dbCommand, "@DateFrom", DbType.Date, DateFrom);
                db.AddInParameter(dbCommand, "@DateTo", DbType.Date, DateTo);
                db.AddInParameter(dbCommand, "@UserName", DbType.String, UserName);

                //Set my timeout to 5 minutes
                dbCommand.CommandTimeout = 420;

                dr = db.ExecuteDataSet(dbCommand);

                return dr.Tables[0].ToDynamicList("Inbound");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }

}
