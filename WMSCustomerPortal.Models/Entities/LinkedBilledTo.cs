using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using RAM.Utilities.Common;

using WMSCustomerPortal.Models.DataAccess;

namespace WMSCustomerPortal.Models.Entities {
    /// <summary>
    /// LinkedBilledTo Class
    /// </summary>
    public class LinkedBilledTo {

        #region Properties

        private int mRecordID = 0;
        public int RecordID { get { return mRecordID; } set { mRecordID = value; } }

        private int mPrincipalID = 0;
        public int PrincipalID { get { return mPrincipalID; } set { mPrincipalID = value; } }

        private string mBilledTo = "";
        public string BilledTo { get { return mBilledTo; } set { mBilledTo = value; } }

        private DateTime mEventDateTime = new DateTime(1900, 1, 1);
        public DateTime EventDateTime { get { return mEventDateTime; } set { mEventDateTime = value; } }

        private string mEventUserID = "";
        public string EventUserID { get { return mEventUserID; } set { mEventUserID = value; } }

        private string mTerminalID = "";
        public string TerminalID { get { return mTerminalID; } set { mTerminalID = value; } }

        private bool mDeleted = false;
        public bool Deleted { get { return mDeleted; } set { mDeleted = value; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor for LinkedBilledTo Class
        /// </summary>
        public LinkedBilledTo() {

        }

        /// <summary>
        /// Constructor for LinkedBilledTo Class with Initialization
        /// </summary>
        public LinkedBilledTo(int pRecordID, int pPrincipalID, string pBilledTo, DateTime pEventDateTime, string pEventUserID, string pTerminalID, bool pDeleted) {
            RecordID = pRecordID;
            PrincipalID = pPrincipalID;
            BilledTo = pBilledTo;
            EventDateTime = pEventDateTime;
            EventUserID = pEventUserID;
            TerminalID = pTerminalID;
            Deleted = pDeleted;
        }

        /// <summary>
        /// Constructor for LinkedBilledTo Class with Initialization
        /// </summary>
        public LinkedBilledTo(DataRow loRow) {
            RecordID = Functions.SafeInt(loRow["RecordID"], 0);
            PrincipalID = Functions.SafeInt(loRow["PrincipalID"], 0);
            BilledTo = Functions.SafeString(loRow["BilledTo"]);
            EventDateTime = Functions.SafeDateTime(loRow["EventDateTime"]);
            EventUserID = Functions.SafeString(loRow["EventUserID"]);
            TerminalID = Functions.SafeString(loRow["TerminalID"]);
            Deleted = Functions.SafeBool(loRow["Deleted"]);
        }
        #endregion

        #region Data Access - Private Methods

        private static List<LinkedBilledTo> ReturnList(DataTable pdtTable) {
            List<LinkedBilledTo> list = new List<LinkedBilledTo>();
            try {
                foreach (DataRow loRow in pdtTable.Rows) {
                    list.Add(ReturnObject(loRow));
                }
                return list;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        private static LinkedBilledTo ReturnObject(DataRow loRow) {
            return new LinkedBilledTo(Functions.SafeInt(loRow["RecordID"], 0),
                Functions.SafeInt(loRow["PrincipalID"], 0),
                Functions.SafeString(loRow["BilledTo"]),
                Functions.SafeDateTime(loRow["EventDateTime"]),
                Functions.SafeString(loRow["EventUserID"]),
                Functions.SafeString(loRow["TerminalID"]),
                Functions.SafeBool(loRow["Deleted"]));
        }
        #endregion

        #region Data Access

        public LinkedBilledTo ReadObject(int pRecordID) {
            DataRow loRow;
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_LinkedBilledTo_Read";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pRecordID);

                loRow = db.ExecuteDataSet(dbCommand).Tables[0].Rows[0];

                if (loRow.Equals(null)) {
                    return null;
                }
                else {
                    return ReturnObject(loRow);
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public int Save(int pRecordID, int pPrincipalID, string pBilledTo, DateTime pEventDateTime, string pEventUserID, string pTerminalID, bool pDeleted) {
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_LinkedBilledTo_Save";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pRecordID, pPrincipalID, pBilledTo, pEventDateTime, pEventUserID, pTerminalID, pDeleted);

                return Convert.ToInt32(db.ExecuteScalar(dbCommand));
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public int SaveLinkedUserToReceiver(int PrincipalID, string ReceiverID, string UserName, string EventTerminal, string EventUser, bool ReceiverFlag)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_LinkedUserToReceiver_Save";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, 0, PrincipalID, ReceiverID, UserName, EventTerminal, EventUser, ReceiverFlag);

                if(db.ExecuteScalar(dbCommand) != DBNull.Value)
                    return Convert.ToInt32(db.ExecuteScalar(dbCommand));

                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteAddNewCustomerAccess(int PrincipalID, string UserName, string EventTerminal, string EventUser)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_AddNewCustomerAccess_Delete";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, PrincipalID, UserName, EventTerminal, EventUser);

                db.ExecuteNonQuery(dbCommand);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SaveAddNewCustomerAccess(int PrincipalID, string UserName, string EventTerminal, string EventUser)
        {
            try
            {

                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_AddNewCustomerAccess_Save";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, 0,PrincipalID, UserName, EventTerminal, EventUser);

                return Convert.ToInt32(db.ExecuteScalar(dbCommand));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        


        public bool Delete(int pintID) {
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_LinkedBilledTo_Delete";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pintID);

                db.ExecuteNonQuery(dbCommand);

                return true;
            }
            catch (Exception ex) {
                throw ex;
            }
        }
        

       public bool DeleteLinkedReceiver(int PrincipalID, int RecordID, string EventUser, string EventTerminal)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_LinkedReceiver_Delete";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, PrincipalID, RecordID, EventTerminal, EventUser);

                db.ExecuteNonQuery(dbCommand);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckUserLevel(int PrincipalID, string UserName) {
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_LinkedReceiver_UserLevel_Read";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, PrincipalID, UserName, 0);

                db.ExecuteNonQuery(dbCommand);

                return true;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public List<LinkedBilledTo> ReadAll_List(int pPrincipalID) {
            List<LinkedBilledTo> records = new List<LinkedBilledTo>();
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));

                NamedParameterMapper parameterMapper = new NamedParameterMapper("@PrincipalID");

                var results = db.ExecuteNamedSprocAccessor<LinkedBilledTo>("usp_LinkedBilledTo_Read_All_by_PrincipalID", parameterMapper, pPrincipalID);
                if (results != null)
                    records = results.ToList();
            }
            catch (Exception ex) {
                throw ex;
            }
            return records;
        }

        public static DataSet GetReceivers(int PrincipalID, string UserName)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = "usp_Receiver_Read";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, PrincipalID, UserName);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet GetAddNewCustomerAcess(int PrincipalID, string UserName)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = "usp_AddNewCustomer_Read";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, PrincipalID, UserName);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet CheckAllReceiversAccess(int PrincipalID, string UserName) {
            DataSet dr = null;
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = "usp_WMS_CheckAllReceiversAcess_Read";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, PrincipalID, UserName);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex) {
                throw ex;
            }
        }
        
        public static DataSet GetAllowedPermissionsList(string UserEmail)
        {
            var dr = new DataSet();
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = "usp_ApplicationGroupRoles_Read_All";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, UserEmail);

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
