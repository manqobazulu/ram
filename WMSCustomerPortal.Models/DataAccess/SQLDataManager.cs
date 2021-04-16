using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Microsoft.Practices.EnterpriseLibrary;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using WMSCustomerPortal.Models.Common;
using WMSCustomerPortal.Models.Entities;
using RAM.Utilities.Common;

namespace WMSCustomerPortal.Models.DataAccess {

    public class SQLDataManager {

        #region Movement Reference

        /// <summary>
        /// Returns a movement reference for the creation of IGD messages. Unique Movement references have to be generated for each pallet a
        /// assigned to the Warehouse
        /// </summary>
        /// <param name="principalCode"></param>
        /// <returns></returns>
        public static string GetNextMovementReference(string principalCode) {
            try {
                Database db = new DatabaseProviderFactory().Create("WMSHostCustomerPortal");
                string sqlCommand = "usp_GetNextMovementReference";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PrincipalCode", DbType.String, principalCode);
                db.AddOutParameter(dbCommand, "@MovementReference", DbType.String, 10);
                db.ExecuteNonQuery(dbCommand);
                return (string)db.GetParameterValue(dbCommand, "@MovementReference");

            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region RAM Order Reference
        public static string GetNextBaseRAMOrder(int principalID) {

            try {
                Database db = new DatabaseProviderFactory().Create("WMSHostCustomerPortal");
                string sqlCommand = "usp_GetBaseRAMOrderReference";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, principalID);
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.String, principalID);
                db.AddOutParameter(dbCommand, "@RAMOrderReference", DbType.String, 10);
                db.ExecuteNonQuery(dbCommand);
                return (string)db.GetParameterValue(dbCommand, "@RAMOrderReference");

            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public static string GetNextRAMPartialOrder(string baseRAMOrderReference, string principleCode) {

            string retVal = "";

            try {

                System.Data.DataSet dsPartials = new System.Data.DataSet();       
                Database db = new DatabaseProviderFactory().Create("WMSHostCustomerPortal");
                string sqlCommand = "usp_GetRAMPartialOrderReference";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@BaseRAMOrderReference", DbType.String, baseRAMOrderReference);
                db.AddInParameter(dbCommand, "@PrincipalCode", DbType.String, principleCode);
                dsPartials = db.ExecuteDataSet(dbCommand);

                int maxValue = 0;

                if (dsPartials.Tables[0].Rows.Count < 1) {

                    retVal = baseRAMOrderReference.ToUpper() + "-00";
                }
                else {

                    foreach (System.Data.DataRow rw in dsPartials.Tables[0].Rows) {

                        string ordernumber = rw["OrderNumber"].ToString();
                        string[] splitString = ordernumber.Split('-');
                        int splitlength = splitString.Length;
                        if (splitlength == 2) {
                            if (Functions.IsNumeric(splitString[1])) {
                                int val = Functions.SafeInt(splitString[1], 0);
                                if (val > maxValue) {
                                    maxValue = val;
                                }
                            }
                        }
                    }
                    maxValue = maxValue + 1;
                    retVal = baseRAMOrderReference + "-" + maxValue.ToString().PadLeft(2, '0');

                }

            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return retVal;
        }
        #endregion

        #region Employee Login

        public List<EmployeeLookupItem> GetEmployeeListByID(string startsWith, int maxRowCount) {
            List<EmployeeLookupItem> lookupList = new List<EmployeeLookupItem>();
            try {
                DatabaseProviderFactory factory = new DatabaseProviderFactory();
                Database db = factory.Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                NamedParameterMapper parameterMapper = new NamedParameterMapper("@StartsWith", "@MaxRowCount");
                var results = db.ExecuteNamedSprocAccessor<EmployeeLookupItem>("usp_Admin_GetEmployeeIDList", parameterMapper, startsWith, maxRowCount);
                if (results != null)
                    lookupList = results.ToList();
            }
            catch (Exception ex) {
                throw ex;
            }
            return lookupList;
        }

        public List<EmployeeLookupItem> GetEmployeeListByName(string startsWith, int maxRowCount) {
            List<EmployeeLookupItem> lookupList = new List<EmployeeLookupItem>();
            try {
                DatabaseProviderFactory factory = new DatabaseProviderFactory();
                Database db = factory.Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));

                NamedParameterMapper parameterMapper = new NamedParameterMapper("@StartsWith", "@MaxRowCount");
                var results = db.ExecuteNamedSprocAccessor<EmployeeLookupItem>("usp_Admin_GetEmployeeNameList", parameterMapper, startsWith, maxRowCount);
                if (results != null)
                    lookupList = results.ToList();
            }
            catch (Exception ex) {
                throw ex;
            }
            return lookupList;
        }

        public bool EmployeeLogin(string employeeId, string password) {

            Employee employee = new Employee();

            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            Database db = factory.Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));

            using (DbCommand cmd = db.DbProviderFactory.CreateCommand()) {
                cmd.CommandText = "usp_Admin_Employee_Login";
                cmd.CommandType = CommandType.StoredProcedure;
                db.AddInParameter(cmd, "@EmployeeID", DbType.String, employeeId);
                db.AddInParameter(cmd, "@Password", DbType.String, password);

                IRowMapper<Employee> mapper = MapBuilder<Employee>.BuildAllProperties();

                using (var reader = db.ExecuteReader(cmd)) {
                    while (reader.Read()) {
                        employee = mapper.MapRow(reader);
                    }
                }
                return !employee.Equals(null);
            }
        }

        #endregion

        #region Hubs

        public List<HubLookupItem> GetHubsForSelectList() {
            List<HubLookupItem> hubs = new List<HubLookupItem>();
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));

                // Create an output row mapper that maps all properties based on the column names
                IRowMapper<HubLookupItem> mapper = MapBuilder<HubLookupItem>.BuildAllProperties();

                // Create a stored procedure accessor that uses this output mapper
                var accessor = db.CreateSprocAccessor("usp_Hubs_Read_All", mapper);

                // Execute the accessor to obtain the results
                var data = accessor.Execute();

                hubs = data.ToList();
            }
            catch (Exception ex) {
                throw ex;
            }
            return hubs;
        }
        #endregion

        #region Customers

        public List<CustomerGroupLookUpItem> CustomerGroup_Lookup(string startsWith, int maxRowCount) {
            List<CustomerGroupLookUpItem> records = new List<CustomerGroupLookUpItem>();
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));

                NamedParameterMapper parameterMapper = new NamedParameterMapper("@StartsWith", "@MaxRowCount");

                var results = db.ExecuteNamedSprocAccessor<CustomerGroupLookUpItem>("usp_CustomerGroup_Lookup", parameterMapper, startsWith, maxRowCount);
                if (results != null)
                    records = results.ToList();
            }
            catch (Exception ex) {
                throw ex;
            }
            return records;
        }

        public List<CustomerLookUpItem> GetOrderCustomers() {
            List<CustomerLookUpItem> customers = new List<CustomerLookUpItem>();
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));

                // Create an output row mapper that maps all properties based on the column names
                IRowMapper<CustomerLookUpItem> mapper = MapBuilder<CustomerLookUpItem>.BuildAllProperties();

                // Create a stored procedure accessor that uses this output mapper
                var accessor = db.CreateSprocAccessor("usp_LinkedPrincipalID_Read_All", mapper);

                var data = accessor.Execute();

                customers = data.ToList();
            }
            catch (Exception ex) {
                throw ex;
            }
            return customers;
        }

        public List<CustomerOrdersLookUpItem> OrderCustomerName_Lookup(string startsWith, int principalId, string eventUser) {
            List<CustomerOrdersLookUpItem> records = new List<CustomerOrdersLookUpItem>();
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));

                NamedParameterMapper parameterMapper = new NamedParameterMapper("@StartsWith", "@PrincipalID", "@UserName"); 

                var results = db.ExecuteNamedSprocAccessor<CustomerOrdersLookUpItem>("usp_Orders_Customer_Lookup_by_PrincipalID_CustomerName", parameterMapper, startsWith, principalId, eventUser);
                if (results != null)
                    records = results.ToList();
            }
            catch (Exception ex) {
                throw ex;
            }
            return records;
        }

        public List<CustomerOrdersLookUpItem> OrderCustomerName_Lookup(string CustName, int principalId, string eventUser,string BULK)
        {
            List<CustomerOrdersLookUpItem> records = new List<CustomerOrdersLookUpItem>();
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));

                NamedParameterMapper parameterMapper = new NamedParameterMapper("@CustName", "@PrincipalID", "@UserName");

                var results = db.ExecuteNamedSprocAccessor<CustomerOrdersLookUpItem>("usp_Orders_Customer_Lookup_by_Bulk_CustomerName", parameterMapper, CustName, principalId, eventUser);
                if (results != null)
                    records = results.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return records;
        }

        public List<CustomerOrdersLookUpItem> CustomerName_Lookup(string startsWith, int principalId) {
            List<CustomerOrdersLookUpItem> records = new List<CustomerOrdersLookUpItem>();
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));

                NamedParameterMapper parameterMapper = new NamedParameterMapper("@StartsWith", "@PrincipalID");

                var results = db.ExecuteNamedSprocAccessor<CustomerOrdersLookUpItem>("usp_Customer_Lookup_by_PrincipalID_CustomerName", parameterMapper, startsWith, principalId);
                if (results != null)
                    records = results.ToList();
            }
            catch (Exception ex) {
                throw ex;
            }
            return records;
        }

        public List<CustomerLookUpItem> Customer_Lookup_NoPrincipals(string startsWith, int maxRowCount) {
            List<CustomerLookUpItem> customers = new List<CustomerLookUpItem>();
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));

                NamedParameterMapper parameterMapper = new NamedParameterMapper("@StartsWith", "@MaxRowCount");

                var results = db.ExecuteNamedSprocAccessor<CustomerLookUpItem>("usp_Customer_Lookup_NoPrincipals", parameterMapper, startsWith, maxRowCount);
                if (results != null)
                    customers = results.ToList();
            }
            catch (Exception ex) {
                throw ex;
            }
            return customers;
        }

        public List<CustomerLookUpItem> Customer_Lookup_by_PrincipalID(string startsWith, int maxRowCount, int principalId) {
            List<CustomerLookUpItem> customers = new List<CustomerLookUpItem>();
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));

                NamedParameterMapper parameterMapper = new NamedParameterMapper("@StartsWith", "@MaxRowCount", "@PrincipalID");

                var results = db.ExecuteNamedSprocAccessor<CustomerLookUpItem>("usp_Customer_Lookup_by_PrincipalID", parameterMapper, startsWith, maxRowCount, principalId);
                if (results != null)
                    customers = results.ToList();
            }
            catch (Exception ex) {
                throw ex;
            }
            return customers;
        }

        public List<CustomerLookUpItem> Customer_Lookup_by_ReceiverID(string startsWith, int principalId)
        {
            List<CustomerLookUpItem> customers = new List<CustomerLookUpItem>();
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));

                NamedParameterMapper parameterMapper = new NamedParameterMapper("@StartsWith", "@MaxRowCount", "@PrincipalID");

                var results = db.ExecuteNamedSprocAccessor<CustomerLookUpItem>("usp_Customer_Lookup_NewOrder", parameterMapper, startsWith, principalId);
                if (results != null)
                    customers = results.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return customers;
        }

        public List<CustomerLookUpName> Customer_LookUp_By_CustomerName(int principalId, string CustomerName)
        {
            List<CustomerLookUpName> customers = new List<CustomerLookUpName>();

            Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
            NamedParameterMapper parameterMapper = new NamedParameterMapper("@PrincipalID", "@CustomerName");

            var results = db.ExecuteNamedSprocAccessor<CustomerLookUpName>("usp_Customer_Look_Up_By_CustomerName", parameterMapper, principalId, CustomerName);
            try
            {
                if (results != null)
                    customers = results.ToList();
            }catch(Exception ex)
            {
                throw;
            }
            //string sqlCommand = "usp_Customer_Look_Up_By_CustomerName";
            //DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
            //dbCommand.CommandTimeout = 0;
            //db.AddInParameter(dbCommand, "PrincipalID", DbType.Int32, principalId);
            //db.AddInParameter(dbCommand, "CustomerName", DbType.String, CustomerName);
            //try
            //{
            //    IDataReader reader = db.ExecuteReader(dbCommand);

            //    string column = reader["CustomerID"].ToString();
            //    Console.WriteLine(column);

            //    Console.WriteLine(reader.ToString());

            //    if (reader.ToString()=="")
            //    {
            //        Console.WriteLine( reader.Read().ToString());
            //    }
            //}
            //catch(Exception ex)
            //{
            //    throw;
            //}

            return customers;
        }

        /// <summary>
        /// Save the Customer record
        /// </summary>
        /// <param name="principalId">The principal identifier.</param>
        /// <param name="customer">The customer</param>
        /// <param name="hubID">The hub identifier.</param>
        /// <param name="isAddingReceiver">if set to <c>true</c> [is adding receiver].</param>
        /// <returns></returns>
        public CustomerSaveResponse Customer_Save(int principalId, Customers customer, string hubID = "", bool isAddingReceiver = true) {
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

            try {
                IDataReader reader = db.ExecuteReader(dbCommand);

                //result = db.GetParameterValue(dbCommand, "Result").ToString();

                while (reader.Read()) {
                    response.CustomerID = reader["NewCustomerID"].ToString();
                    response.Saved = Convert.ToBoolean(reader["Success"]);
                    response.Details = reader["Result"].ToString();
                }
            }
            catch (Exception ex) {
                response.Details = "ERROR: " + ex.Message;
            }
            return response;
        }
        #endregion

        #region Products

        public List<ProductLookUpItem> ProductCode_Lookup(string startsWith, int principalId) {
            List<ProductLookUpItem> records = new List<ProductLookUpItem>();
            try {

                string safeStartsWith = Functions.SafeString(startsWith, "");
                
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));

                NamedParameterMapper parameterMapper = new NamedParameterMapper("@StartsWith", "@PrincipalID");

             //   var results = db.ExecuteNamedSprocAccessor<ProductLookUpItem>("usp_ProductStaging_SearchProductCode", parameterMapper, startsWith, principalId);
                var results = db.ExecuteNamedSprocAccessor<ProductLookUpItem>("usp_ProductStaging_SearchProductCode", parameterMapper, safeStartsWith, principalId);
                if (results != null)
                    records = results.ToList();
            }
            catch (Exception ex) {
                throw ex;
            }
            return records;
        }

        public List<ProductAutocomplete> Product_Lookup(int principalId, string startsWith, int maxRowCount, string searchType) {
            List<ProductAutocomplete> records = new List<ProductAutocomplete>();
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));

                NamedParameterMapper parameterMapper = new NamedParameterMapper("@PrincipalID", "@StartsWith", "@MaxRowCount", "@SearchType");

                var results = db.ExecuteNamedSprocAccessor<ProductAutocomplete>("usp_ProductStaging_Autocomplete", parameterMapper, principalId, startsWith, maxRowCount, searchType);
                if (results != null)
                    records = results.ToList();
            }
            catch (Exception ex) {
                throw ex;
            }
            return records;
        }

        public List<CustomerAutocomplete> Customer_Lookup(int principalId, string startsWith, int maxRowCount, string searchType) {
            List<CustomerAutocomplete> records = new List<CustomerAutocomplete>();
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));

                NamedParameterMapper parameterMapper = new NamedParameterMapper("@PrincipalID", "@StartsWith", "@MaxRowCount", "@SearchType");

                var results = db.ExecuteNamedSprocAccessor<CustomerAutocomplete>("usp_Customer_Autocomplete", parameterMapper, principalId, startsWith, maxRowCount, searchType);
                if (results != null)
                    records = results.ToList();
            }
            catch (Exception ex) {
                throw ex;
            }
            return records;
        }

        #endregion

        #region Returns

        public List<ReturnOrdersLookUpItem> ReturnOrderNumber_Lookup(string startsWith, int principalId, string eventUser)
        {
            List<ReturnOrdersLookUpItem> records = new List<ReturnOrdersLookUpItem>();
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));

                NamedParameterMapper parameterMapper = new NamedParameterMapper("@StartsWith", "@PrincipalID", "@UserName");

                var results = db.ExecuteNamedSprocAccessor<ReturnOrdersLookUpItem>("usp_ReturnOrders_Lookup_by_PrincipalID_OrderNumber", parameterMapper, startsWith, principalId, eventUser);
                if (results != null)
                    records = results.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return records;
        }


        #endregion


    }
}