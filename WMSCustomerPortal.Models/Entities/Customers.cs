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
using WMSCustomerPortal.Models.Common;

namespace WMSCustomerPortal.Models.Entities {
    /// <summary>
    /// Customers Class
    /// </summary>
    [Serializable]
    public class Customers {        

        #region Properties

        public string CustomerID { get; set; }

        public string StoreCode { get; set; }

        public string CustomerGroupID { get; set; }

        private string customerName = string.Empty;
        public string CustomerName { get { return customerName.Unescape(); } set { customerName = value; } }

        public string TelephoneNo { get; set; }

        public string FaxNo { get; set; }

        public string EmailAddress { get; set; }

        public string StreetAddress1 { get; set; }

        public string StreetAddress2 { get; set; }

        public string ZoneID { get; set; }

        public int? isShipper { get; set; }

        public int? isActive { get; set; }

        public string EventDateTime { get; set; }

        public string EmployeeID { get; set; }

        public string TerminalID { get; set; }

        public string LastUpdate { get; set; }

        public string isDirty { get; set; }

        public string CustomerType { get; set; }

        public string CellNo { get; set; }

        public string DefaultContactPerson { get; set; }

        public string GEOID { get; set; }

        public int? POIID { get; set; }

        public double? Lat { get; set; }

        public double? Lon { get; set; }

        public string StreetNo { get; set; }

        public string BuildingName { get; set; }

        public string SuiteFloorNo { get; set; }

        public bool? International { get; set; }

        public DateTime? RecordCreatedDT { get; set; }

        public string RecordCreateHubID { get; set; }

        public DateTime? UpdateDT { get; set; }

        public string UpdatedBy { get; set; }

        public string UpdateTerminalID { get; set; }

        public string UpdateHubID { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor for Customers Class
        /// </summary>
        public Customers() {

        }

        /// <summary>
        /// Constructor for Customers Class with Initialization
        /// </summary>
        public Customers(string pCustomerID, string pStoreCode, string pCustomerGroupID, string pCustomerName, string pTelephoneNo, string pFaxNo, string pEmailAddress, string pStreetAddress1, string pStreetAddress2, string pZoneID, int? pisShipper, int? pisActive, string pEventDateTime, string pEmployeeID, string pTerminalID, string pLastUpdate, string pisDirty, string pCustomerType, string pCellNo, string pDefaultContactPerson, string pGEOID, int? pPOIID, double? pLat, double? pLon, string pStreetNo, string pBuildingName, string pSuiteFloorNo, bool? pInternational, DateTime? pRecordCreatedDT, string pRecordCreateHubID, DateTime? pUpdateDT, string pUpdatedBy, string pUpdateTerminalID, string pUpdateHubID) {
            CustomerID = pCustomerID;
            StoreCode = pStoreCode;
            CustomerGroupID = pCustomerGroupID;
            CustomerName = pCustomerName;
            TelephoneNo = pTelephoneNo;
            FaxNo = pFaxNo;
            EmailAddress = pEmailAddress;
            StreetAddress1 = pStreetAddress1;
            StreetAddress2 = pStreetAddress2;
            ZoneID = pZoneID;
            isShipper = pisShipper;
            isActive = pisActive;
            EventDateTime = pEventDateTime;
            EmployeeID = pEmployeeID;
            TerminalID = pTerminalID;
            LastUpdate = pLastUpdate;
            isDirty = pisDirty;
            CustomerType = pCustomerType;
            CellNo = pCellNo;
            DefaultContactPerson = pDefaultContactPerson;
            GEOID = pGEOID;
            POIID = pPOIID;
            Lat = pLat;
            Lon = pLon;
            StreetNo = pStreetNo;
            BuildingName = pBuildingName;
            SuiteFloorNo = pSuiteFloorNo;
            International = pInternational;
            RecordCreatedDT = pRecordCreatedDT;
            RecordCreateHubID = pRecordCreateHubID;
            UpdateDT = pUpdateDT;
            UpdatedBy = pUpdatedBy;
            UpdateTerminalID = pUpdateTerminalID;
            UpdateHubID = pUpdateHubID;
        }

        #endregion

        #region Data Access - Private Methods

        private static ArrayList ReturnCollection(DataTable pdtTable) {
            ArrayList palCollection = new ArrayList();
            try {
                foreach (DataRow loRow in pdtTable.Rows) {
                    palCollection.Add(ReturnObject(loRow));
                }
                return palCollection;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        private static List<Customers> ReturnList(DataTable pdtTable) {
            List<Customers> list = new List<Customers>();
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

        private static Customers ReturnObject(DataRow loRow) {
            return new Customers(Functions.SafeString(loRow["CustomerID"]),
                Functions.SafeString(loRow["StoreCode"]),
                Functions.SafeString(loRow["CustomerGroupID"]),
                Functions.SafeString(loRow["CustomerName"]),
                Functions.SafeString(loRow["TelephoneNo"]),
                Functions.SafeString(loRow["FaxNo"]),
                Functions.SafeString(loRow["EmailAddress"]),
                Functions.SafeString(loRow["StreetAddress1"]),
                Functions.SafeString(loRow["StreetAddress2"]),
                Functions.SafeString(loRow["ZoneID"]),
                Functions.SafeInt(loRow["isShipper"], 0),
                Functions.SafeInt(loRow["isActive"], 0),
                Functions.SafeString(loRow["EventDateTime"]),
                Functions.SafeString(loRow["EmployeeID"]),
                Functions.SafeString(loRow["TerminalID"]),
                Functions.SafeString(loRow["LastUpdate"]),
                Functions.SafeString(loRow["isDirty"]),
                Functions.SafeString(loRow["CustomerType"]),
                Functions.SafeString(loRow["CellNo"]),
                Functions.SafeString(loRow["DefaultContactPerson"]),
                Functions.SafeString(loRow["GEOID"]),
                Functions.SafeInt(loRow["POIID"], 0),
                Functions.SafeDouble(loRow["Lat"], 0),
                Functions.SafeDouble(loRow["Lon"], 0),
                Functions.SafeString(loRow["StreetNo"]),
                Functions.SafeString(loRow["BuildingName"]),
                Functions.SafeString(loRow["SuiteFloorNo"]),
                Functions.SafeBool(loRow["International"]),
                Functions.SafeDateTime(loRow["RecordCreatedDT"]),
                Functions.SafeString(loRow["RecordCreateHubID"]),
                Functions.SafeDateTime(loRow["UpdateDT"]),
                Functions.SafeString(loRow["UpdatedBy"]),
                Functions.SafeString(loRow["UpdateTerminalID"]),
                Functions.SafeString(loRow["UpdateHubID"]));
        }
        #endregion

        #region Data Access

        public Customers ReadObject(string pCustomerID) {
            DataRow loRow;
            try {
                Database db = DatabaseFactory.CreateDatabase();
                string sqlCommand = "";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pCustomerID);

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

        public static int Save(string pCustomerID, string pStoreCode, string pCustomerGroupID, string pCustomerName, string pTelephoneNo, string pFaxNo, string pEmailAddress, string pStreetAddress1, string pStreetAddress2, string pZoneID, int? pisShipper, int? pisActive, string pEventDateTime, string pEmployeeID, string pTerminalID, string pLastUpdate, string pisDirty, string pCustomerType, string pCellNo, string pDefaultContactPerson, string pGEOID, int? pPOIID, double? pLat, double? pLon, string pStreetNo, string pBuildingName, string pSuiteFloorNo, bool? pInternational, DateTime? pRecordCreatedDT, string pRecordCreateHubID, DateTime? pUpdateDT, string pUpdatedBy, string pUpdateTerminalID, string pUpdateHubID) {
            try {
                Database db = DatabaseFactory.CreateDatabase();
                string sqlCommand = "";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pCustomerID, pStoreCode, pCustomerGroupID, pCustomerName, pTelephoneNo, pFaxNo, pEmailAddress, pStreetAddress1, pStreetAddress2, pZoneID, pisShipper, pisActive, pEventDateTime, pEmployeeID, pTerminalID, pLastUpdate, pisDirty, pCustomerType, pCellNo, pDefaultContactPerson, pGEOID, pPOIID, pLat, pLon, pStreetNo, pBuildingName, pSuiteFloorNo, pInternational, pRecordCreatedDT, pRecordCreateHubID, pUpdateDT, pUpdatedBy, pUpdateTerminalID, pUpdateHubID);

                return (int)db.ExecuteScalar(dbCommand);
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public static int Update(string pCustomerID, string pStoreCode, string pCustomerGroupID, string pCustomerName, string pTelephoneNo, string pFaxNo, string pEmailAddress, string pStreetAddress1, string pStreetAddress2, string pZoneID, int? pisShipper, int? pisActive, string pEventDateTime, string pEmployeeID, string pTerminalID, string pLastUpdate, string pisDirty, string pCustomerType, string pCellNo, string pDefaultContactPerson, string pGEOID, int? pPOIID, double? pLat, double? pLon, string pStreetNo, string pBuildingName, string pSuiteFloorNo, bool? pInternational, DateTime? pRecordCreatedDT, string pRecordCreateHubID, DateTime? pUpdateDT, string pUpdatedBy, string pUpdateTerminalID, string pUpdateHubID) {
            try {
                Database db = DatabaseFactory.CreateDatabase();
                string sqlCommand = "";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pCustomerID, pStoreCode, pCustomerGroupID, pCustomerName, pTelephoneNo, pFaxNo, pEmailAddress, pStreetAddress1, pStreetAddress2, pZoneID, pisShipper, pisActive, pEventDateTime, pEmployeeID, pTerminalID, pLastUpdate, pisDirty, pCustomerType, pCellNo, pDefaultContactPerson, pGEOID, pPOIID, pLat, pLon, pStreetNo, pBuildingName, pSuiteFloorNo, pInternational, pRecordCreatedDT, pRecordCreateHubID, pUpdateDT, pUpdatedBy, pUpdateTerminalID, pUpdateHubID);

                return (int)db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex) {
                throw ex;
            }
        }



        public CustomerSaveResponse Customer_Save(int principalId, Customers customer, string hubID = "", bool isAddingReceiver = true)
        {
            CustomerSaveResponse response = new CustomerSaveResponse();
            string result = string.Empty;

            Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
            string sqlCommand = "usp_Customer_Save";
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
            dbCommand.CommandTimeout = 0;

            db.AddInParameter(dbCommand, "IsAddingReceiver", DbType.Boolean, isAddingReceiver);
            db.AddInParameter(dbCommand, "PrincipalID", DbType.Int32, principalId);
            db.AddInParameter(dbCommand, "CustomerID", DbType.String, customer.CustomerID);
            db.AddInParameter(dbCommand, "StoreCode", DbType.String, customer.StoreCode);
            db.AddInParameter(dbCommand, "CustomerGroupID", DbType.String, customer.CustomerGroupID);
            db.AddInParameter(dbCommand, "CustomerName", DbType.String, customer.CustomerName);
            db.AddInParameter(dbCommand, "TelephoneNo", DbType.String, customer.TelephoneNo);
            db.AddInParameter(dbCommand, "FaxNo", DbType.String, customer.FaxNo);
            db.AddInParameter(dbCommand, "CellNo", DbType.String, customer.CellNo);
            db.AddInParameter(dbCommand, "DefaultContactPerson", DbType.String, customer.DefaultContactPerson);
            db.AddInParameter(dbCommand, "EmailAddress", DbType.String, customer.EmailAddress);
            db.AddInParameter(dbCommand, "StreetAddress1", DbType.String, customer.StreetAddress1);
            db.AddInParameter(dbCommand, "StreetAddress2", DbType.String, customer.StreetAddress2);
            db.AddInParameter(dbCommand, "ZoneID", DbType.String, customer.ZoneID);
            db.AddInParameter(dbCommand, "IsShipper", DbType.Int32, customer.isShipper);
            db.AddInParameter(dbCommand, "IsActive", DbType.Int32, customer.isActive);
            db.AddInParameter(dbCommand, "HubID", DbType.String, hubID);
            db.AddInParameter(dbCommand, "EmployeeID", DbType.String, customer.EmployeeID);
            db.AddInParameter(dbCommand, "TerminalID", DbType.String, customer.TerminalID);
            db.AddInParameter(dbCommand, "CheckForDups", DbType.Boolean, true);

            db.AddOutParameter(dbCommand, "Result", DbType.String, 50);

            try
            {
                IDataReader reader = db.ExecuteReader(dbCommand);

                //result = db.GetParameterValue(dbCommand, "Result").ToString();

                while (reader.Read())
                {
                    response.CustomerID = reader["NewCustomerID"].ToString();
                    response.Saved = Convert.ToBoolean(reader["Success"]);
                    response.Details = reader["Result"].ToString();
                }
            }
            catch (Exception ex)
            {
                response.Details = "ERROR: " + ex.Message;
            }
            return response;
        }





        #endregion
    }
}
