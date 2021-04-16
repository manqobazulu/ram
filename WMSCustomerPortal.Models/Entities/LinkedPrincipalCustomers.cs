using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using System.Configuration;

using RAM.Utilities.Common;
namespace WMSCustomerPortal.Models.Entities {
    /// <summary>
    /// LinkedPrincipalCustomers Class
    /// </summary>
    public class LinkedPrincipalCustomers {
        #region Constants

        public static readonly string FLD_CUSTOMERID = "CustomerID";
        public static readonly string FLD_CUSTOMERDETAILID = "CustomerDetailID";
        public static readonly string FLD_PRINCIPALID = "PrincipalID";
        public static readonly string FLD_STORECODE = "StoreCode";
        public static readonly string FLD_CUSTOMERGROUPID = "CustomerGroupID";
        public static readonly string FLD_CUSTOMERNAME = "CustomerName";
        public static readonly string FLD_TELEPHONENO = "TelephoneNo";
        public static readonly string FLD_FAXNO = "FaxNo";
        public static readonly string FLD_EMAILADDRESS = "EmailAddress";
        public static readonly string FLD_STREETADDRESS1 = "StreetAddress1";
        public static readonly string FLD_ZONEID = "ZoneID";
        public static readonly string FLD_STREETADDRESS2 = "StreetAddress2";
        public static readonly string FLD_ISSHIPPER = "isShipper";
        public static readonly string FLD_ISACTIVE = "isActive";
        public static readonly string FLD_EMPLOYEEID = "EmployeeID";
        public static readonly string FLD_CUSTOMERTYPE = "CustomerType";
        public static readonly string FLD_CELLNO = "CellNo";
        public static readonly string FLD_GEOID = "GEOID";
        public static readonly string FLD_POIID = "POIID";
        public static readonly string FLD_DEFAULTCONTACTPERSON = "DefaultContactPerson";
        public static readonly string FLD_LAT = "Lat";
        public static readonly string FLD_LON = "Lon";
        public static readonly string FLD_STREETNO = "StreetNo";
        public static readonly string FLD_BUILDINGNAME = "BuildingName";
        public static readonly string FLD_SUITEFLOORNO = "SuiteFloorNo";
        public static readonly string FLD_INTERNATIONAL = "International";
        public static readonly string FLD_EVENTDATE = "EventDate";
        public static readonly string FLD_EVENTTERMINAL = "EventTerminal";
        public static readonly string FLD_EVENTUSER = "EventUser";
        public static readonly string FLD_DELETED = "Deleted";


        #endregion

        #region Stored Procedures

        private static readonly string STP_SAVE = "usp_LinkedPrincipalCustomers_Save";
        private static readonly string STP_READ = "usp_LinkedPrincipalCustomers_Read";
        private static readonly string STP_READ_ALL = "usp_LinkedPrincipalCustomers_Read_All";
        private static readonly string STP_SEARCH_RAM = "usp_LinkedPrincipalCustomers_SearchRAMCustomers";
        private static readonly string STP_READ_ALL_PRINCIPAL = "usp_LinkedPrincipalCustomers_Read_All_Principal";
        private static readonly string STP_DELETE = "usp_LinkedPrincipalCustomers_Delete";
        private static readonly string STP_SEARCH_ZONEDETAIL = "usp_Zones_SuburbSearch";
        private static readonly string STP_SAVE_TNT = "usp_CustomerV2_Save";
        private static readonly string STP_BILLING = "usp_Billing_CustomerID_Update";


        #endregion

        #region Properties

        private string mCustomerID = "";
        public string CustomerID { get { return mCustomerID; } set { mCustomerID = value; } }

        private int mCustomerDetailID = 0;
        public int CustomerDetailID { get { return mCustomerDetailID; } set { mCustomerDetailID = value; } }

        private int mPrincipalID = 0;
        public int PrincipalID { get { return mPrincipalID; } set { mPrincipalID = value; } }

        private string mStoreCode = "";
        public string StoreCode { get { return mStoreCode.ToUpper(); } set { mStoreCode = value; } }

        private string mCustomerGroupID = "";
        public string CustomerGroupID { get { return mCustomerGroupID.ToUpper(); } set { mCustomerGroupID = value; } }

        private string mCustomerName = "";
        public string CustomerName { get { return mCustomerName.ToUpper(); } set { mCustomerName = value; } }

        private string mTelephoneNo = "";
        public string TelephoneNo { get { return mTelephoneNo; } set { mTelephoneNo = value; } }

        private string mFaxNo = "";
        public string FaxNo { get { return mFaxNo; } set { mFaxNo = value; } }

        private string mEmailAddress = "";
        public string EmailAddress { get { return mEmailAddress.ToUpper(); } set { mEmailAddress = value; } }

        private string mStreetAddress1 = "";
        public string StreetAddress1 { get { return mStreetAddress1.ToUpper(); } set { mStreetAddress1 = value; } }

        private string mZoneID = "";
        public string ZoneID { get { return mZoneID; } set { mZoneID = value; } }

        private string mStreetAddress2 = "";
        public string StreetAddress2 { get { return mStreetAddress2.ToUpper(); } set { mStreetAddress2 = value; } }

        private int? misShipper = 0;
        public int? isShipper { get { return misShipper; } set { misShipper = value; } }

        private int? misActive = 0;
        public int? isActive { get { return misActive; } set { misActive = value; } }

        private string mEmployeeID = "";
        public string EmployeeID { get { return mEmployeeID; } set { mEmployeeID = value; } }

        private string mCustomerType = "";
        public string CustomerType { get { return mCustomerType; } set { mCustomerType = value; } }

        private string mCellNo = "";
        public string CellNo { get { return mCellNo; } set { mCellNo = value; } }

        private string mGEOID = "";
        public string GEOID { get { return mGEOID; } set { mGEOID = value; } }

        private int? mPOIID = 0;
        public int? POIID { get { return mPOIID; } set { mPOIID = value; } }

        private string mDefaultContactPerson = "";
        public string DefaultContactPerson { get { return mDefaultContactPerson.ToUpper(); } set { mDefaultContactPerson = value; } }

        private double? mLat = 0;
        public double? Lat { get { return mLat; } set { mLat = value; } }

        private double? mLon = 0;
        public double? Lon { get { return mLon; } set { mLon = value; } }

        private string mStreetNo = "";
        public string StreetNo { get { return mStreetNo; } set { mStreetNo = value; } }

        private string mBuildingName = "";
        public string BuildingName { get { return mBuildingName.ToUpper(); } set { mBuildingName = value; } }

        private string mSuiteFloorNo = "";
        public string SuiteFloorNo { get { return mSuiteFloorNo; } set { mSuiteFloorNo = value; } }

        private bool? mInternational = false;
        public bool? International { get { return mInternational; } set { mInternational = value; } }

        private DateTime mEventDate = new DateTime(1900, 1, 1);
        public DateTime EventDate { get { return mEventDate; } set { mEventDate = value; } }

        private string mEventTerminal = "";
        public string EventTerminal { get { return mEventTerminal; } set { mEventTerminal = value; } }

        private string mEventUser = "";
        public string EventUser { get { return mEventUser; } set { mEventUser = value; } }

        private bool mDeleted = false;
        public bool Deleted { get { return mDeleted; } set { mDeleted = value; } }


        public string Suburb { get; set; }
        public string PostalCode { get; set; }
        public string Area { get; set; }        
        public string HubID { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor for LinkedPrincipalCustomers Class
        /// </summary>
        public LinkedPrincipalCustomers() {

        }

        /// <summary>
        /// Constructor for LinkedPrincipalCustomers Class with Initialization
        /// </summary>
        public LinkedPrincipalCustomers(string pCustomerID, int pCustomerDetailID, int pPrincipalID, string pStoreCode, string pCustomerGroupID, string pCustomerName, string pTelephoneNo, string pFaxNo, string pEmailAddress, string pStreetAddress1, string pZoneID, string pStreetAddress2, int? pisShipper, int? pisActive, string pEmployeeID, string pCustomerType, string pCellNo, string pGEOID, int? pPOIID, string pDefaultContactPerson, double? pLat, double? pLon, string pStreetNo, string pBuildingName, string pSuiteFloorNo, bool? pInternational, DateTime pEventDate, string pEventTerminal, string pEventUser, bool pDeleted, string pSuburb, string pPostalCode, string pArea, string pHubID) {
            CustomerID = pCustomerID;
            CustomerDetailID = pCustomerDetailID;
            PrincipalID = pPrincipalID;
            StoreCode = pStoreCode;
            CustomerGroupID = pCustomerGroupID;
            CustomerName = pCustomerName;
            TelephoneNo = pTelephoneNo;
            FaxNo = pFaxNo;
            EmailAddress = pEmailAddress;
            StreetAddress1 = pStreetAddress1;
            ZoneID = pZoneID;
            StreetAddress2 = pStreetAddress2;
            isShipper = pisShipper;
            isActive = pisActive;
            EmployeeID = pEmployeeID;
            CustomerType = pCustomerType;
            CellNo = pCellNo;
            GEOID = pGEOID;
            POIID = pPOIID;
            DefaultContactPerson = pDefaultContactPerson;
            Lat = pLat;
            Lon = pLon;
            StreetNo = pStreetNo;
            BuildingName = pBuildingName;
            SuiteFloorNo = pSuiteFloorNo;
            International = pInternational;
            EventDate = pEventDate;
            EventTerminal = pEventTerminal;
            EventUser = pEventUser;
            Deleted = pDeleted;

            Suburb = pSuburb;
            PostalCode = pPostalCode;
            Area = pArea;
            HubID = pHubID;
        }

        /// <summary>
        /// Constructor for LinkedPrincipalCustomers Class with Initialization
        /// </summary>
        public LinkedPrincipalCustomers(DataRow loRow) {
            CustomerID = Functions.SafeString(loRow["CustomerID"]);
            CustomerDetailID = Functions.SafeInt(loRow["CustomerDetailID"], 0);
            PrincipalID = Functions.SafeInt(loRow["PrincipalID"], 0);
            StoreCode = Functions.SafeString(loRow["StoreCode"]);
            CustomerGroupID = Functions.SafeString(loRow["CustomerGroupID"]);
            CustomerName = Functions.SafeString(loRow["CustomerName"]);
            TelephoneNo = Functions.SafeString(loRow["TelephoneNo"]);
            FaxNo = Functions.SafeString(loRow["FaxNo"]);
            EmailAddress = Functions.SafeString(loRow["EmailAddress"]);
            StreetAddress1 = Functions.SafeString(loRow["StreetAddress1"]);
            ZoneID = Functions.SafeString(loRow["ZoneID"]);
            StreetAddress2 = Functions.SafeString(loRow["StreetAddress2"]);
            isShipper = Functions.SafeInt(loRow["isShipper"], 0);
            isActive = Functions.SafeInt(loRow["isActive"], 0);
            EmployeeID = Functions.SafeString(loRow["EmployeeID"]);
            CustomerType = Functions.SafeString(loRow["CustomerType"]);
            CellNo = Functions.SafeString(loRow["CellNo"]);
            GEOID = Functions.SafeString(loRow["GEOID"]);
            POIID = Functions.SafeInt(loRow["POIID"], 0);
            DefaultContactPerson = Functions.SafeString(loRow["DefaultContactPerson"]);
            Lat = Functions.SafeDouble(loRow["Lat"], 0);
            Lon = Functions.SafeDouble(loRow["Lon"], 0);
            StreetNo = Functions.SafeString(loRow["StreetNo"]);
            BuildingName = Functions.SafeString(loRow["BuildingName"]);
            SuiteFloorNo = Functions.SafeString(loRow["SuiteFloorNo"]);
            International = Functions.SafeBool(loRow["International"]);
            EventDate = Functions.SafeDateTime(loRow["EventDate"]);
            EventTerminal = Functions.SafeString(loRow["EventTerminal"]);
            EventUser = Functions.SafeString(loRow["EventUser"]);
            Deleted = Functions.SafeBool(loRow["Deleted"]);

            Suburb = Functions.SafeString(loRow["Suburb"]);
            PostalCode = Functions.SafeString(loRow["PostalCode"]);
            Area = Functions.SafeString(loRow["Area"]);
            HubID = Functions.SafeString(loRow["HubID"]);
        }

        #endregion

        #region Data Access - Private Methods

        private static List<LinkedPrincipalCustomers> ReturnList(DataTable pdtTable) {
            List<LinkedPrincipalCustomers> list = new List<LinkedPrincipalCustomers>();
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

        private static LinkedPrincipalCustomers ReturnObject(DataRow loRow) {
            return new LinkedPrincipalCustomers(Functions.SafeString(loRow["CustomerID"]),
                Functions.SafeInt(loRow["CustomerDetailID"], 0),
                Functions.SafeInt(loRow["PrincipalID"], 0),
                Functions.SafeString(loRow["StoreCode"]).ToUpper(),
                Functions.SafeString(loRow["CustomerGroupID"]).ToUpper(),
                Functions.SafeString(loRow["CustomerName"]).ToUpper(),
                Functions.SafeString(loRow["TelephoneNo"]),
                Functions.SafeString(loRow["FaxNo"]),
                Functions.SafeString(loRow["EmailAddress"]).ToUpper(),
                Functions.SafeString(loRow["StreetAddress1"]).ToUpper(),
                Functions.SafeString(loRow["ZoneID"]).ToUpper(),
                Functions.SafeString(loRow["StreetAddress2"]).ToUpper(),
                Functions.SafeInt(loRow["isShipper"], 0),
                Functions.SafeInt(loRow["isActive"], 0),
                Functions.SafeString(loRow["EmployeeID"]),
                Functions.SafeString(loRow["CustomerType"]),
                Functions.SafeString(loRow["CellNo"]),
                Functions.SafeString(loRow["GEOID"]),
                Functions.SafeInt(loRow["POIID"], 0),
                Functions.SafeString(loRow["DefaultContactPerson"]).ToUpper(),
                Functions.SafeDouble(loRow["Lat"], 0),
                Functions.SafeDouble(loRow["Lon"], 0),
                Functions.SafeString(loRow["StreetNo"]).ToUpper(),
                Functions.SafeString(loRow["BuildingName"]).ToUpper(),
                Functions.SafeString(loRow["SuiteFloorNo"]).ToUpper(),
                Functions.SafeBool(loRow["International"]),
                Functions.SafeDateTime(loRow["EventDate"]),
                Functions.SafeString(loRow["EventTerminal"]),
                Functions.SafeString(loRow["EventUser"]),
                Functions.SafeBool(loRow["Deleted"]),

                Functions.SafeString(loRow["Suburb"]),
                Functions.SafeString(loRow["PostalCode"]),
                Functions.SafeString(loRow["Area"]),
                Functions.SafeString(loRow["HubID"]));
        }

        #endregion

        #region Data Access

        public static System.Data.DataTable LookupZoneDetail(string prefixText, int count, string contextKey) {
            System.Data.DataTable retVal = new DataTable();

            Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;

            //lets create all the parameters , but not specify a value
            string sqlCommand = STP_SEARCH_ZONEDETAIL;
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, "", prefixText, count, 1);

            retVal = db.ExecuteDataSet(dbCommand).Tables[0];

            return retVal;
        }

        /// <summary>
        /// Returns a DataTable filled with the ram customers
        /// </summary>
        /// <param name="prefixValue"></param>
        /// <param name="countRecords"></param>
        /// <param name="contextKey"></param>
        /// <returns></returns>
        public static System.Data.DataTable GetRAMCustomersDT(string prefixValue, int countRecords, string contextKey, string searchField) {
            System.Data.DataTable retVal = new DataTable();

            Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;

            //lets create all the parameters , but not specify a value
            string sqlCommand = STP_SEARCH_RAM;
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, "", "", "", "", "", countRecords);

            switch (searchField.ToUpper()) {
                case "CUSTOMERGROUP": dbCommand = db.GetStoredProcCommand(sqlCommand, prefixValue, "", "", "", "", countRecords);
                    break;
                case "CUSTOMERID": dbCommand = db.GetStoredProcCommand(sqlCommand, "", prefixValue, "", "", "", countRecords);
                    break;
                case "CUSTOMERSTORECODE": dbCommand = db.GetStoredProcCommand(sqlCommand, "", "", prefixValue, "", "", countRecords);
                    break;
                case "CUSTOMERTELNUMBER": dbCommand = db.GetStoredProcCommand(sqlCommand, "", "", "", prefixValue, "", countRecords);
                    break;
                case "CUSTOMERNAME": dbCommand = db.GetStoredProcCommand(sqlCommand, "", "", "", "", prefixValue, countRecords);
                    break;
            }

            try {
                retVal = db.ExecuteDataSet(dbCommand).Tables[0];
            }
            catch (Exception ex) {
                int i = 0;
                //do nothing
            }
            return retVal;
        }

        public Common.CustomerSaveResponse CreateNewCustomer(LinkedPrincipalCustomers staging, string eventUser, string eventTerminal, string hubId, bool checkForDups, bool overrideEdit) {
            Common.CustomerSaveResponse response = new Common.CustomerSaveResponse();

            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            Database db = factory.Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));

            using (DbCommand cmd = db.DbProviderFactory.CreateCommand()) {
                cmd.CommandText = "usp_Admin_Employee_Login";
                cmd.CommandType = CommandType.StoredProcedure;

                db.AddInParameter(cmd, "@CustomerID", DbType.String, staging.CustomerID);
			    db.AddInParameter(cmd, "@CustomerName", DbType.String, staging.CustomerName);
			    db.AddInParameter(cmd, "@StoreCode", DbType.String, staging.StoreCode);
			    db.AddInParameter(cmd, "@DefaultContactPerson", DbType.String, staging.DefaultContactPerson);
			    db.AddInParameter(cmd, "@CustomerGroupID", DbType.String, staging.CustomerGroupID);
			    db.AddInParameter(cmd, "@TelephoneNo", DbType.String, staging.TelephoneNo);
			    db.AddInParameter(cmd, "@CellNo", DbType.String, staging.CellNo);
			    db.AddInParameter(cmd, "@FaxNo", DbType.String, staging.FaxNo);
			    db.AddInParameter(cmd, "@EmailAddress", DbType.String, staging.EmailAddress);
			    db.AddInParameter(cmd, "@StreetAddress1", DbType.String, staging.StreetAddress1);
			    db.AddInParameter(cmd, "@StreetAddress2", DbType.String, staging.StreetAddress2);
			    db.AddInParameter(cmd, "@StreetNo", DbType.String, staging.StreetNo);
			    db.AddInParameter(cmd, "@BuildingName", DbType.String, staging.BuildingName);
			    db.AddInParameter(cmd, "@SuiteFloorNo", DbType.String, staging.SuiteFloorNo);
			    db.AddInParameter(cmd, "@ZoneID", DbType.String, staging.ZoneID);
			    db.AddInParameter(cmd, "@GEOID", DbType.String, staging.GEOID);
			    db.AddInParameter(cmd, "@POIID", DbType.Int32, staging.POIID);
			    db.AddInParameter(cmd, "@Lat", DbType.Double, staging.Lat);
			    db.AddInParameter(cmd, "@Lon", DbType.Double, staging.Lon);
			    db.AddInParameter(cmd, "@isShipper", DbType.Boolean, staging.isShipper);
			    db.AddInParameter(cmd, "@isActive", DbType.Boolean, staging.isActive);
			    db.AddInParameter(cmd, "@HubID", DbType.String, hubId);
			    db.AddInParameter(cmd, "@EmployeeID", DbType.String, eventUser);
			    db.AddInParameter(cmd, "@TerminalID", DbType.String, eventTerminal);
			    db.AddInParameter(cmd, "@CheckForDups", DbType.Boolean, checkForDups);
                db.AddInParameter(cmd, "@OverrideEditRestrictions", DbType.Boolean, overrideEdit);

                IRowMapper<Common.CustomerSaveResponse> mapper = MapBuilder<Common.CustomerSaveResponse>.BuildAllProperties();

                using (var reader = db.ExecuteReader(cmd)) {
                    while (reader.Read()) {
                        response = mapper.MapRow(reader);
                    }
                }
                return response;
            }
        }

        #region CreateNewCustomerTnT - UNUSED
        private static DataTable CreateNewCustomerTnT(LinkedPrincipalCustomers staging, string eventUser, string eventTerminal, string hubId, bool checkForDups, bool overrideEdit) {

            Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
            System.Data.DataTable retVal = new DataTable();

            string sqlCommand = "usp_CustomerV2_Save";

            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand,
                staging.CustomerID,
                staging.CustomerName,
                staging.StoreCode,
                staging.DefaultContactPerson,
                staging.CustomerGroupID,
                staging.TelephoneNo,
                staging.CellNo,
                staging.FaxNo,
                staging.EmailAddress,
                staging.StreetAddress1,
                staging.StreetAddress2,
                staging.StreetNo,
                staging.BuildingName,
                staging.SuiteFloorNo,
                staging.ZoneID,
                staging.GEOID,
                staging.POIID,
                staging.Lat,
                staging.Lon,
                staging.isShipper,
                staging.isActive,
                hubId,
                eventUser,
                eventTerminal,
                checkForDups,
                overrideEdit);

            retVal = db.ExecuteDataSet(dbCommand).Tables[0];

            return retVal;
        } 
        #endregion

        public static LinkedPrincipalCustomers ReadObject(int pCustomerDetailID) {
            DataRow loRow;
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;

                string sqlCommand = STP_READ;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pCustomerDetailID);

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

        public static int Save(string pCustomerID, int pCustomerDetailID, int pPrincipalID, string pStoreCode, string pCustomerGroupID, string pCustomerName, string pTelephoneNo, string pFaxNo, string pEmailAddress, string pStreetAddress1, string pZoneID, string pStreetAddress2, int? pisShipper, int? pisActive, string pEmployeeID, string pCustomerType, string pCellNo, string pGEOID, int? pPOIID, string pDefaultContactPerson, double? pLat, double? pLon, string pStreetNo, string pBuildingName, string pSuiteFloorNo, bool? pInternational, DateTime pEventDate, string pEventTerminal, string pEventUser, bool pDeleted) {
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = STP_SAVE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pCustomerID, pCustomerDetailID, pPrincipalID, pStoreCode, pCustomerGroupID, pCustomerName, pTelephoneNo, pFaxNo, pEmailAddress, pStreetAddress1, pZoneID, pStreetAddress2, pisShipper, pisActive, pEmployeeID, pCustomerType, pCellNo, pGEOID, pPOIID, pDefaultContactPerson, pLat, pLon, pStreetNo, pBuildingName, pSuiteFloorNo, pInternational, pEventDate, pEventTerminal, pEventUser, pDeleted);

                return (int)db.ExecuteScalar(dbCommand);
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public static int SaveBillingCustomerID(string pCustomerID, string pBillingCustomerID, int pPrincipalID, string pEventTerminal, string pEventUser) {
            try {
                int count;

                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = STP_BILLING;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pCustomerID, pBillingCustomerID, pPrincipalID, pEventTerminal, pEventUser);

                //return (int)db.ExecuteScalar(dbCommand);

                count = Convert.ToInt32(db.ExecuteScalar(dbCommand));
                return count;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public static int Save(LinkedPrincipalCustomers stagingRecord) {
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = STP_SAVE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand,
                    stagingRecord.CustomerDetailID,
                    stagingRecord.PrincipalID,
                    stagingRecord.CustomerID,
                    stagingRecord.mEventTerminal,
                    stagingRecord.EventUser);

                db.ExecuteScalar(dbCommand);
                //now get the out parameter

                return Functions.SafeInt(db.GetParameterValue(dbCommand, "@CustomerDetailID"), 0);
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public static int Update(LinkedPrincipalCustomers stagingRecord) {
            int RetVal = 0;
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = STP_SAVE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand,
                    stagingRecord.CustomerDetailID,
                    stagingRecord.PrincipalID,
                    stagingRecord.CustomerID,
                    stagingRecord.mEventTerminal,
                    stagingRecord.EventUser);

                return (int)db.ExecuteScalar(dbCommand);
                //now get the out parameter

                //return (int)db.GetParameterValue(dbCommand, "@CustomerDetailID");
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public static int Update(string pCustomerID, int pCustomerDetailID, int pPrincipalID, string pStoreCode, string pCustomerGroupID, string pCustomerName, string pTelephoneNo, string pFaxNo, string pEmailAddress, string pStreetAddress1, string pZoneID, string pStreetAddress2, int? pisShipper, int? pisActive, string pEmployeeID, string pCustomerType, string pCellNo, string pGEOID, int? pPOIID, string pDefaultContactPerson, double? pLat, double? pLon, string pStreetNo, string pBuildingName, string pSuiteFloorNo, bool? pInternational, DateTime pEventDate, string pEventTerminal, string pEventUser, bool pDeleted) {
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = STP_SAVE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pCustomerID, pCustomerDetailID, pPrincipalID, pStoreCode, pCustomerGroupID, pCustomerName, pTelephoneNo, pFaxNo, pEmailAddress, pStreetAddress1, pZoneID, pStreetAddress2, pisShipper, pisActive, pEmployeeID, pCustomerType, pCellNo, pGEOID, pPOIID, pDefaultContactPerson, pLat, pLon, pStreetNo, pBuildingName, pSuiteFloorNo, pInternational, pEventDate, pEventTerminal, pEventUser, pDeleted);

                return (int)db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public static bool Delete(int pintID, string eventUser, string eventTerminal) {
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = STP_DELETE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pintID, eventUser, eventTerminal);

                db.ExecuteNonQuery(dbCommand);

                return true;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public static IDataReader Read(int pintID) {
            IDataReader dr = null;
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = STP_READ;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pintID);

                dr = db.ExecuteReader(dbCommand);

                return dr;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public static IDataReader ReadAll() {
            IDataReader dr = null;
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = STP_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                dr = db.ExecuteReader(dbCommand);

                return dr;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public static DataSet ReadAllDS() {
            DataSet dr = null;
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = STP_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public static DataSet ReadAllDS(int principalID, bool isActive) {
            DataSet dr = null;
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = "usp_LinkedPrincipalCustomers_Read_All_Principal";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, principalID, isActive);

                //now .. add the incoming parameter
                //db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);
                //db.AddInParameter(dbCommand, "@isActive", DbType.Boolean, isActive);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public static List<LinkedPrincipalCustomers> ReadAll_List() {
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = STP_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                return ReturnList(db.ExecuteDataSet(dbCommand).Tables[0]);
            }
            catch (Exception ex) {
                throw ex;
            }
        }
        #endregion
    }
}
