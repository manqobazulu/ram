using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using RAM.Utilities.Common;

using WMSCustomerPortal.Models.DataAccess;

namespace WMSCustomerPortal.Models.Entities {
    public partial class Principal {        

        #region Stored Procedures

        private const string STP_SAVE = "usp_Principal_Save";
        private const string STP_READ = "usp_Principal_Read";
        //private const string STP_READ_ALL = "usp_Principal_Read_All";
        private const string STP_DELETE = "usp_Principal_Delete";
        private const string STP_UPDATE = "usp_Principal_Update";

        #endregion

        #region Properties

        public int PrincipalID { get; set; }
        public string PrincipalCode { get; set; }
        public string PrincipalName { get; set; }
        public string EventUser { get; set; }
        public string EventTerminal { get; set; }
        public string Prefix { get; set; }
        public string FileDelimiter { get; set; }
        public bool WebServiceIntegration { get; set; }
        public System.DateTime EventDate { get; set; }
        public bool Deleted { get; set; }
        public string CustomerGroupID { get; set; }

        #endregion

        #region Constructors

        public Principal() {

        }        

        public Principal(
            Principal stagingRecord,
            string pEventUser,
                string pEventTerminal) {

            PrincipalID = stagingRecord.PrincipalID;
            PrincipalName = stagingRecord.PrincipalName.ToUpper();
            PrincipalCode = stagingRecord.PrincipalCode.ToUpper();
            CustomerGroupID = stagingRecord.CustomerGroupID;
            Prefix = stagingRecord.Prefix.ToUpper();
            FileDelimiter = stagingRecord.FileDelimiter;
            WebServiceIntegration = stagingRecord.WebServiceIntegration;

            EventUser = pEventUser;
            EventTerminal = pEventTerminal;
        }

        /// <summary>
        /// Constructor for Customer_Principal Class with Initialization
        /// </summary>
        public Principal(int pCustomerPrincipalID, string pCustomerPrincipalCode, string pCustomerGroupID, string pPrincipalName, string pEventUser, string pEventTerminal, string pFileDelimiter, bool pWebServiceIntegration) {
            PrincipalID = pCustomerPrincipalID;
            PrincipalName = pPrincipalName.ToUpper();
            CustomerGroupID = pCustomerGroupID;
            PrincipalCode = pCustomerPrincipalCode.ToUpper();
            EventUser = pEventUser;
            EventTerminal = pEventTerminal;
            FileDelimiter = pFileDelimiter;
            WebServiceIntegration = pWebServiceIntegration;
        }

        #endregion

        #region Data Access - Private Methods

        private ArrayList ReturnCollection(DataTable pdtTable) {
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

        private List<Principal> ReturnList(DataTable pdtTable) {
            List<Principal> list = new List<Principal>();
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

        private Principal ReturnObject(DataRow loRow) {
            return new Principal(
                 Functions.SafeInt(loRow["PrincipalID"], 0),
                 Functions.SafeString(loRow["PrincipalCode"]),
                 Functions.SafeString(loRow["CustomerGroupID"]),
                 Functions.SafeString(loRow["PrincipalName"]),
                 Functions.SafeString(loRow["EventUser"]),
                 Functions.SafeString(loRow["EventTerminal"]),
                 Functions.SafeString(loRow["FileDelimiter"]),
                 Functions.SafeBool(loRow["WebServiceIntegration"])
            );
        }
        #endregion

        #region Data Access

        public Principal ReadObject(int principalID) {
            DataRow loRow;
            try {

                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); 
                string sqlCommand = STP_READ;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);

                loRow = db.ExecuteDataSet(dbCommand).Tables[0].Rows[0];

                if (loRow.Equals(null)) {
                    return null;
                }
                else {
                    return ReturnObject(loRow);
                }
            }
            catch (Exception ex) {
                return null;
            }
        }


        public int Save() {
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); 
                string sqlCommand = STP_SAVE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddOutParameter(dbCommand, "@PrincipalID", DbType.Int32, 4);
                db.AddInParameter(dbCommand, "@PrincipalCode", DbType.String, this.PrincipalCode);
                db.AddInParameter(dbCommand, "@CustomerGroupID", DbType.String, this.CustomerGroupID);
                db.AddInParameter(dbCommand, "@PrincipalName", DbType.String, this.PrincipalName);
                db.AddInParameter(dbCommand, "@Prefix", DbType.String, this.Prefix);
                db.AddInParameter(dbCommand, "@EventUser", DbType.String, this.EventUser);
                db.AddInParameter(dbCommand, "@EventTerminal", DbType.String, this.EventTerminal);
                db.AddInParameter(dbCommand, "@FileDelimiter", DbType.String, this.FileDelimiter);
                db.AddInParameter(dbCommand, "@WebServiceIntegration", DbType.Boolean, this.WebServiceIntegration);

                db.ExecuteNonQuery(dbCommand);

                return (int)db.GetParameterValue(dbCommand, "@PrincipalID");
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public int Update() {
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); 
                string sqlCommand = STP_UPDATE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, this.PrincipalID);
                db.AddInParameter(dbCommand, "@PrincipalCode", DbType.String, this.PrincipalCode);
                db.AddInParameter(dbCommand, "@PrincipalName", DbType.String, this.PrincipalName);

                db.AddInParameter(dbCommand, "@EventUser", DbType.String, this.EventUser);
                db.AddInParameter(dbCommand, "@EventTerminal", DbType.String, this.EventTerminal);
                db.AddInParameter(dbCommand, "@FileDelimiter", DbType.String, this.FileDelimiter);
                db.AddInParameter(dbCommand, "@WebServiceIntegration", DbType.Boolean, this.WebServiceIntegration);
                //db.AddInParameter(dbCommand, "@Deleted", DbType.Boolean, this.Deleted);

                db.ExecuteNonQuery(dbCommand);
                return (int)db.GetParameterValue(dbCommand, "@PrincipalID");


            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// Marks a record as deleted within the database
        /// </summary>
        /// <param name="detailID">The GUID detailID of the customer</param>
        /// <returns></returns>
        public bool Delete(int principalID, string eventTerminal, string eventUser) {
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); 
                string sqlCommand = STP_DELETE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, principalID, eventTerminal, eventUser);

                //db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);
                //db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);
                //db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);

                db.ExecuteNonQuery(dbCommand);

                return true;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="detailID">The detail ID of the record to read </param>
        /// <returns></returns>
        public IDataReader Read(int principalID) {
            IDataReader dr = null;
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); 
                string sqlCommand = STP_READ;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);

                dr = db.ExecuteReader(dbCommand);

                return dr;
            }
            catch (Exception ex) {
                throw ex;
            }
        }        

        /// <summary>
        /// Reads ALL records ... only for a specified Principal
        /// </summary>
        /// <param name="principalCode">The principal code</param>
        /// <returns>A Dataset containing the records for the principalCode</returns>
        public DataSet ReadAllDS() {
            DataSet dr = null;
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); 
                string sqlCommand = "usp_Principal_Read_All";

                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex) {
                throw ex;
            }
        }
        
        public List<Principal> ReadAllList() {
            List<Principal> customers = new List<Principal>();
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); 

                // Create an output row mapper that maps all properties based on the column names
                IRowMapper<Principal> mapper = MapBuilder<Principal>.BuildAllProperties();

                // Create a stored procedure accessor that uses this output mapper
                var accessor = db.CreateSprocAccessor("usp_Principal_Read_All", mapper);

                var data = accessor.Execute();

                customers = data.ToList();
            }
            catch (Exception ex) {
                throw ex;
            }
            return customers;
        }

        public string GetSetPrincipalIDByCode(string PrincipalCode, string UserName) {

            string dr = null;
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_Principal_SetPrincipal";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, PrincipalCode, UserName);

                dr = db.ExecuteScalar(dbCommand).ToString();

                return dr;
            }
            catch (Exception ex) {
                throw ex;
            }

        }


        #endregion
    }
}

