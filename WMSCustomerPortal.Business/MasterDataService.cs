using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using WMSCustomerPortal.Models.DataAccess;
using WMSCustomerPortal.Models.Common;
using WMSCustomerPortal.Models.Entities;

using System.Threading.Tasks;

namespace WMSCustomerPortal.Business {
    public class MasterDataService {

        #region Customers

        /// <summary>
        /// Retrieves a dataset of all the Customers for a principal ID
        /// </summary>
        /// <param name="principalID"></param>
        /// <returns></returns>
        public List<dynamic> GetAllCustomers(int principalID, bool isActive) {
            try {
                DataSet ds = LinkedPrincipalCustomers.ReadAllDS(principalID, isActive);

            // Test dynamic conversion using extension method
            var result = ds.Tables[0].ToDynamicList("LinkedPrincipalCustomers");

            return result;
            }
            catch { throw; }
        }

        /// <summary>
        /// Returns a LinkedPrincipalCustomer Object by a supplied ID
        /// </summary>
        /// <param name="customerDetailID"></param>
        /// <returns></returns>
        public static LinkedPrincipalCustomers GetLinkedCustomerByID(int customerDetailID) {
            try { 
            return LinkedPrincipalCustomers.ReadObject(customerDetailID);
            }
            catch { throw; }
        }


        /// <summary>
        /// Business logic for the persisting of a customer object to the database
        /// </summary>
        /// <param name="stagingRecord"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public static int AddCustomer(LinkedPrincipalCustomers stagingRecord, string eventUser, string eventTerminal) {

            try { 
            int retVal = 0;

            //make sure these have been added
            stagingRecord.EventTerminal = eventTerminal;
            stagingRecord.EventUser = eventUser;

            retVal = LinkedPrincipalCustomers.Save(stagingRecord);

            return retVal;
            }
            catch { throw; }
        }        

        /// <summary>
        /// Creates a new customer in the RAM Management System
        /// </summary>
        /// <param name="staging"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <param name="checkForDups"></param>
        /// <returns>WMSCustomerPortal.Models.Common.CustomerSaveResponse</returns>
        public CustomerSaveResponse CreateNewCustomer(int principalId, Customers customer, string hubID, bool isAddingReceiver) {
            try
            { 
            if (principalId < 1)
                throw new System.ArgumentException("Invalid parameter value", "PrincipalId");

            CustomerSaveResponse response = new SQLDataManager().Customer_Save(principalId, customer, hubID, isAddingReceiver);

            return response;
            }
            catch { throw; }
        }

        /// <summary>
        /// Updates the links for a LinkedPrincipalCustomer 
        /// </summary>
        /// <param name="stagingRecord"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public int UpdateCustomer(LinkedPrincipalCustomers stagingRecord, string eventUser, string eventTerminal) {
            try { 
            int retVal = 0;

            //make sure these have been added
            stagingRecord.EventTerminal = eventTerminal;
            stagingRecord.EventUser = eventUser;

            retVal = LinkedPrincipalCustomers.Save(stagingRecord);

            return retVal;
            }
            catch { throw; }
        }

        /// <summary>
        /// Updates the links for a LinkedPrincipalCustomer 
        /// </summary>
        /// <param name="stagingRecord"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public int SaveBillingCustomerID(string customerID, string pBillingCustomerID, int principalID, string eventTerminal, string eventUser) {
            try {
                int retVal = 0;

                //make sure these have been added

                retVal = LinkedPrincipalCustomers.SaveBillingCustomerID(customerID, pBillingCustomerID, principalID, eventTerminal, eventUser);

                return retVal;
            }
            catch { throw; }
        }

        /// <summary>
        /// Deletes the link for a LinkedPrincipalCustomer
        /// </summary>
        /// <param name="customerDetailID"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public static bool DeleteCustomer(int customerDetailID, string eventUser, string eventTerminal) {

            try { 
            bool retVal = false;

            retVal = LinkedPrincipalCustomers.Delete(customerDetailID, eventUser, eventTerminal);

            return retVal;
            }
            catch { throw; }
        }

        /// <summary>
        /// Returns ArrayList of partially completed ram customers
        /// </summary>
        /// <param name="prefixValue"></param>
        /// <param name="contextKey"></param>
        /// <returns></returns>
        public static System.Collections.ArrayList SearchRAMCustomersList(string prefixValue, int countRecords, string contextKey, string searchParameter) {
            try
            { 
            System.Collections.ArrayList retVal = new System.Collections.ArrayList();

            System.Data.DataTable dt = new DataTable();

            dt = LinkedPrincipalCustomers.GetRAMCustomersDT(prefixValue, countRecords, contextKey, searchParameter);

            foreach (System.Data.DataRow rw in dt.Rows) {
                //now add the values to the list 
                retVal.Add(rw["CustomerName"].ToString());
            }

            return retVal;
            }
            catch { throw; }
        }

        /// <summary>
        /// Returns Datatable of partially completed ram customers
        /// </summary>
        /// <param name="prefixValue"></param>
        /// <param name="contextKey"></param>
        /// <returns></returns>
        public static System.Data.DataTable SearchRAMCustomersDT(string prefixValue, int countRecords, string contextKey, string searchField) {
            try { 
            System.Data.DataTable dt = new DataTable();

            dt = LinkedPrincipalCustomers.GetRAMCustomersDT(prefixValue, countRecords, contextKey, searchField);

            return dt;
            }
            catch { throw; }
        }

        /// <summary>
        /// Returns a Zone Object from  the RAM management system
        /// </summary>
        /// <param name="zoneID"></param>
        /// <returns></returns>
        public static Zones LookupZone(string zoneID) {

            try { 
            Zones retVal = new Zones();
            retVal = Zones.ReadObject(zoneID);
            return retVal;
            }
            catch { throw; }
        }

        /// <summary>
        /// Looks up Zone details for an autoocomplete for example
        /// </summary>
        /// <param name="prefixText"></param>
        /// <param name="count"></param>
        /// <param name="contextKey"></param>
        /// <returns></returns>
        public static System.Data.DataTable LookupZoneDetail(string prefixText, int count, string contextKey) {
            // lookup from the entities
            try { 
            System.Data.DataTable retVal = new DataTable();

            retVal = LinkedPrincipalCustomers.LookupZoneDetail(prefixText, count, contextKey);

            return retVal;
            }
            catch { throw; }
        }
        #endregion

        #region Principals

        /// <summary>
        /// Returns a Dataset of all the WMS Principals
        /// </summary>
        /// <returns></returns>
        public List<Principal> GetAllPrincipals() {
            try { 
            var data = new Principal().ReadAllList();

            return data;
            }
            catch { throw; }
        }

        public List<dynamic> GetAllPrincipals_Dynamic() {

            try { 
            var data = new Principal().ReadAllDS();

            return data.Tables[0].ToDynamicList("Principals");
            }
            catch { throw; }
        }

        /// <summary>
        /// Returns the Principal Code when an ID is supplied
        /// </summary>
        /// <param name="principalID"></param>
        /// <returns></returns>
        public static string GetPrincipalCodeByID(int principalID) {

            try { 
            Principal principal = new Principal();
            principal = principal.ReadObject(principalID);

            return principal.PrincipalCode;
            }
            catch { throw; }
        }

        ///by the ID supplied <summary>
        /// Returns a Principal Object 
        /// </summary>
        /// <param name="principalID"></param>
        /// <returns></returns>
        public static Principal GetPrincipalByID(int principalID) {
            try
            {
                Principal principal = new Principal();

                principal = principal.ReadObject(principalID);

                return principal;
            }
            catch { throw; }
        }

        /// <summary>
        /// Checks to see if the Principal Code Exists
        /// </summary>
        /// <param name="principalCode"></param>
        /// <returns></returns>
        public bool DoesPrincipalExist(string principalCode) {

            try { 
            bool doesExist = false;

            List<Principal> principals = new Principal().ReadAllList();

            foreach (Principal pr in principals) {
                if (pr.PrincipalCode.ToUpper() == principalCode.ToUpper()) {
                    doesExist = true;
                    break;
                }
            }
            return doesExist;
            }
            catch { throw; }
        }

        public bool DoesPrefixExist(string principalPrefix) {

            try {
                bool doesExist = false;

                List<Principal> principals = new Principal().ReadAllList();

                foreach (Principal pr in principals) {
                    if (pr.Prefix.ToUpper() == principalPrefix.ToUpper()) {
                        doesExist = true;
                        break;
                    }
                }
                return doesExist;
            }
            catch { throw; }
        }

        public int GetFirstPrincipalIDByCode(string principalCode)
        {
            try { 
            int retVal = 0;

            List<Principal> principals = new Principal().ReadAllList();

            foreach (Principal pr in principals)
            {
                if (pr.PrincipalCode.ToUpper() == principalCode.ToUpper())
                {
                    retVal = pr.PrincipalID;
                    break;
                }
            }
            return retVal;
            }
            catch { throw; }
        }


        public string GetSetPrincipalIDByCode(string PrincipalCode, string UserName) {

            string retVal = "";
            Principal staging = new Principal();

            retVal = staging.GetSetPrincipalIDByCode(PrincipalCode, UserName);

            return retVal;

        }



        /// <summary>
        /// Adds a new Principal Object to the Database
        /// </summary>
        /// <param name="stagingRecord"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public int AddPrincipal(Principal stagingRecord, string eventUser, string eventTerminal) {
            try { 
            int retVal = 0;

            Principal principalDetail = new Principal(stagingRecord, eventUser, eventTerminal);

            retVal = principalDetail.Save();

            return retVal;
            }
            catch { throw; }
        }

        /// <summary>
        /// Updates the Principal Object
        /// </summary>
        /// <param name="stagingRecord"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public int UpdatePrincipal(Principal stagingRecord, string eventUser, string eventTerminal) {
            try { 
            int retVal = 0;
            Principal igdStaging = new Principal(stagingRecord, eventUser, eventTerminal);

            retVal = igdStaging.Update();

            return retVal;
            }
            catch { throw; }
        }

        /// <summary>
        /// Deletes the Principal Object by supplying the whole Principal Object
        /// </summary>
        /// <param name="stagingRecord"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public static bool DeletePrincipal(Principal stagingRecord, string eventUser, string eventTerminal) {
            try { 
            bool retVal = false;
            Principal igdStaging = new Principal(stagingRecord, eventUser, eventTerminal);
            retVal = igdStaging.Delete(stagingRecord.PrincipalID, eventTerminal, eventUser);
            return retVal;
            }
            catch { throw; }
        }

        /// <summary>
        /// Deletes the Principal Object from the supplied ID
        /// </summary>
        /// <param name="principalID"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public static bool DeletePrincipal(int principalID, string eventUser, string eventTerminal) {
            try { 
            bool retVal = false;

            Principal igdStaging = new Principal();
            igdStaging.EventTerminal = eventTerminal;
            igdStaging.EventUser = eventUser;

            retVal = igdStaging.Delete((principalID), eventTerminal, eventUser);

            return retVal;
            }
            catch { throw; }
        }

        #endregion

        #region Linked BilledTo

        public List<LinkedBilledTo> GetLinkedBilledTo_by_PrincipalID(int principalId) {
            try { 
            if (principalId < 1)
                throw new System.ArgumentException("Invalid parameter value", "PrincipalId");

            var data = new LinkedBilledTo().ReadAll_List(principalId);

            return data;
            }
            catch { throw; }
        }

        public int SaveLinkedBilledTo(int principalId, string billedTo, string eventUserId, string terminalId) {

            try { 
            if (principalId < 1)
                throw new System.ArgumentException("Invalid parameter value", "PrincipalId");
            if (string.IsNullOrEmpty(billedTo))
                throw new System.ArgumentException("Invalid parameter value", "BilledTo");

            var recordId = new LinkedBilledTo().Save(-1, principalId, billedTo, DateTime.Now, eventUserId, terminalId, false);

            return recordId;
            }
            catch { throw; }
        }

        public int SaveLinkedReceiver(int principalId, string ReceiverID, string UserName, string EventUser, string EventTerminal, bool ReceiverFlag)
        {

            var retVal = new LinkedBilledTo().SaveLinkedUserToReceiver(principalId, ReceiverID, UserName, EventTerminal, EventUser, ReceiverFlag);

            return retVal;

        }

        public int SaveAddNewCustomerAccess(int principalId, string UserName, string EventUser, string EventTerminal)
        {

            var retVal = new LinkedBilledTo().SaveAddNewCustomerAccess(principalId, UserName, EventTerminal, EventUser);

            return retVal;

        }

        public bool DeleteAddNewCustomerAccess(int principalId, string UserName, string EventUser, string EventTerminal)
        {

            var retVal = new LinkedBilledTo().DeleteAddNewCustomerAccess(principalId, UserName, EventTerminal, EventUser);

            return retVal;

        }

        public bool DeleteLinkedReceiver(int PrincipalID, int RecordID, string EventUser, string EventTerminal)
        {

            var retVal = new LinkedBilledTo().DeleteLinkedReceiver(PrincipalID, RecordID, EventTerminal, EventUser);

            return retVal;

        }

        public bool CheckUserLevel(string UserName, int PrincipalID) {
          try 
              {
                 bool retVal = false;
                 retVal = new LinkedBilledTo().CheckUserLevel(PrincipalID, UserName);
                 return retVal;
              }
            catch { throw; }
        }

        public List<dynamic> GetReceivers(int PrincipalID, string UserName)
        {
            try
            {
                DataSet ds = LinkedBilledTo.GetReceivers(PrincipalID, UserName);

                // Test dynamic conversion using extension method
                var result = ds.Tables[0].ToDynamicList("LinkedReceivers");

                return result;
            }
            catch { throw; }
        }

        public List<dynamic> GetAddNewCustomerAcess(int PrincipalID, string UserName)
        {
            try
            {
                DataSet ds = LinkedBilledTo.GetAddNewCustomerAcess(PrincipalID, UserName);

                // Test dynamic conversion using extension method
                var result = ds.Tables[0].ToDynamicList("GetAddNewCustomerAcess");

                return result;
            }
            catch { throw; }
        }

        public List<dynamic> CheckAllReceiversAccess(int PrincipalID, string UserName) {
            try {
                DataSet ds = LinkedBilledTo.CheckAllReceiversAccess(PrincipalID, UserName);

                // Test dynamic conversion using extension method
                var result = ds.Tables[0].ToDynamicList("CheckAllReceiversAccess");

                return result;
            }
            catch { throw; }
        }
      
        public List<string> GetAllowedPermissionsList(string userEmail)
        {

            DataSet ds = LinkedBilledTo.GetAllowedPermissionsList(userEmail);

            List<string> resultList = new List<string>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                resultList.Add(row["Id"].ToString());
            }
            return resultList;
        }

        #endregion

        #region Products

        /// <summary>
        /// Validates if a product with this code already exists somewhere in the db
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="findPartial"></param>
        /// <param name="principalID"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public bool DoesProductExist(string productCode, bool findPartial, int principalID, string eventUser, string eventTerminal) {

            try { 
            bool retVal = true;
            ProductStaging staging = new ProductStaging(principalID, eventUser, eventTerminal);
            List<ProductStaging> stagingList = new List<ProductStaging>();


            stagingList = staging.FindProductsList(productCode, "", "", "", principalID);

            if (stagingList.Count > 0) {

                retVal = true;
            }
            else {

                retVal = false;
            }
            return retVal;
            }
            catch { throw; }

        }

        /// <summary>
        /// Validates if a product with this code already exists somewhere in the db
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="findPartial"></param>
        /// <param name="principalID"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public bool DoesProductExist(string productCode, int principalID)
        {
            try { 
            bool retVal = true;

            ProductStaging staging = new ProductStaging();
           // ProductStaging staging = new ProductStaging(principalID, eventUser, eventTerminal);
            List<ProductStaging> stagingList = new List<ProductStaging>();


            stagingList = staging.FindProductsList(productCode, "", "", "", principalID);

            if (stagingList.Count > 0)
            {

                retVal = true;
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
        /// Validates if a ean with this code already exists somewhere in the db
        /// </summary>
        /// <param name="eanCode"></param>
        /// <param name="principalID"></param>
        /// <returns></returns>
         public bool DoesEANExist(string eanCode, int principalId)
        {
            try
            {
                bool retVal = true;

                ProductStaging staging = new ProductStaging();
                // ProductStaging staging = new ProductStaging(principalID, eventUser, eventTerminal);
                List<ProductStaging> stagingList = new List<ProductStaging>();


                stagingList = staging.DoesEANExist(eanCode, principalId);
                
                if (stagingList.Count > 0)
                {

                    retVal = true;
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
        /// Retrieves a list of products according to criteria
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="eanCode"></param>
        /// <param name="shortDescription"></param>
        /// <param name="longDescrption"></param>
        /// <param name="principalID"></param>
        /// <returns></returns>
        public  List<ProductStaging> SearchProductsList(string productCode, string eanCode, string shortDescription, string longDescrption, int principalID) {

            // bool retVal = true;
            try { 
            List<ProductStaging> dt = new List<ProductStaging>();
            ProductStaging stage = new ProductStaging();
            dt = stage.FindProductsList(productCode, eanCode, shortDescription, longDescrption, principalID);

            return dt;
            }
            catch { throw; }


        }

        /// <summary>
        /// Retrieves a list of products according to criteria
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="eanCode"></param>
        /// <param name="shortDescription"></param>
        /// <param name="longDescrption"></param>
        /// <param name="principalID"></param>
        /// <returns></returns>
        public static System.Data.DataTable SearchProductsDT(string productCode, string eanCode, string shortDescription, string longDescrption, int principalID) {

            // bool retVal = true;

            try { 
            ProductStaging staging = new ProductStaging();
            staging.PrincipalID = principalID;

            System.Data.DataTable stagingDT = new DataTable();

            stagingDT = staging.FindProductsDT(productCode, eanCode, shortDescription, longDescrption, principalID);

            return stagingDT;
            }
            catch { throw; }


        }

        /// <summary>
        /// Updates the Product object in the database
        /// </summary>
        /// <param name="stagingRecord"></param>
        /// <param name="principalID"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public int UpdateProductStaging(ProductStaging stagingRecord, int principalID, string eventUser, string eventTerminal) {
            try { 
            int retVal = 0;
            ProductStaging igdStaging = new ProductStaging(stagingRecord, principalID, eventUser, eventTerminal);
            retVal = igdStaging.Update();
            return retVal;
            }
            catch { throw; }
        }

        /// <summary>
        /// Deletes the Product Object In the database
        /// </summary>
        /// <param name="productStagingID"></param>
        /// <param name="principalID"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public static bool DeleteProductStaging(int productStagingID, int principalID, string eventUser, string eventTerminal) {
            try { 
            bool retVal = false;
            ProductStaging igdStaging = new ProductStaging();
            retVal = igdStaging.Delete(productStagingID, eventTerminal, eventUser);
            return retVal;
            }
            catch { throw; }
        }

        /// <summary>
        /// Adds a new Product Object to the database
        /// </summary>
        /// <param name="stagingRecord"></param>
        /// <param name="principalID"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public static int AddProductStaging(ProductStaging stagingRecord, int principalID, string eventUser, string eventTerminal) {
            try
            { 
            int retVal = 0;
            ProductStaging igdStaging = new ProductStaging(stagingRecord, principalID, eventUser, eventTerminal);
            igdStaging.Save();

            return retVal;
            }
            catch { throw; }

        }


        /// <summary>
        /// Returns a Product Object
        /// </summary>
        /// <param name="productStagingID"></param>
        /// <returns></returns>
        public static ProductStaging GetProductStagingFromID(int productStagingID) {
            try { 
            ProductStaging staging = new ProductStaging();
            //  staging.PrincipalID = principalID;
            staging = staging.ReadObject(productStagingID);
            return staging;
            }
            catch { throw; }

        }


        /// <summary>
        /// Finds a product object for which the products code and principal matches
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="principalID"></param>
        /// <returns></returns>
        public static ProductStaging GetProductStagingFromProdCode(string productCode, int principalID) {

            try { 
            List<ProductStaging> stageList = new List<ProductStaging>();
            ProductStaging staging = new ProductStaging();
            staging.PrincipalID = principalID;

            stageList = staging.FindProductsList(productCode, "", "", "", principalID);

            if (stageList.Count > 0) {
                staging = stageList[0];
            }
            else {
                staging = null;
            }

            return staging;
            }
            catch { throw; }

        }

        /// <summary>
        /// Returns all the Products for a specific principal ID
        /// </summary>
        /// <param name="principalID"></param>
        /// <returns></returns>
        public static DataSet GetAllStagingProducts(int principalID) {

            try
            { 
            DataSet retval = new DataSet();
            ProductStaging staging = new ProductStaging();

            retval = staging.ReadAllDS(principalID);

            return retval;
            }
            catch { throw; }
        }


        /// <summary>
        /// Returns a list of products which have been linked to a principal id and the client's purchase order reference
        /// </summary>
        /// <param name="purcharseOrderReference"></param>
        /// <param name="principalID"></param>
        /// <returns></returns>
        public static List<ProductStaging> SearchPOList(string purcharseOrderReference, int principalID) {

            try { 
            DataSet dt = new DataSet();
            ProductStaging stage = new ProductStaging();
            List<ProductStaging> list = new List<ProductStaging>();
            dt = InboundMaster.ReadAllPO(principalID, purcharseOrderReference);

            ProductStaging p = null;
            foreach (DataRow item in dt.Tables[0].Rows) {
                p = new ProductStaging();
                p.PrincipalID = int.Parse(item["PrincipalID"].ToString());
                p.ProdCode = item["PORef"].ToString();
                list.Add(p);
            }

            return list;
            }
            catch { throw; }
        }

        /// <summary>
        /// Retrieves the Products and performs a filter os aspecified ... 
        /// </summary>
        /// <param name="rowFilter"></param>
        /// <param name="principalID"></param>
        /// <returns></returns>
        public static DataSet GetAllStagingProducts(string rowFilter, int principalID) {

            try { 
            DataSet retval = new DataSet();
            ProductStaging staging = new ProductStaging();
            staging.PrincipalID = principalID;
            retval = staging.ReadAllDS(principalID);

            retval.Tables[0].DefaultView.RowFilter = rowFilter;

            int rows = retval.Tables[0].Rows.Count;

            return retval;
            }
            catch { throw; }
        }




        #endregion

        #region XProducts


        /// <summary>
        /// Returns all the products as stored in MATFLO for a principal Code
        /// </summary>
        /// <param name="principalCode"></param>
        /// <returns></returns>
        public static System.Data.DataSet GetAllXProducts(string principalCode) {
            try { 
            System.Data.DataSet retValue = new DataSet();
            X_PRODUCT xProduct = new X_PRODUCT();

            retValue = X_PRODUCT.ReadAllDS(principalCode);

            return retValue;
            }
            catch { throw; }

        }


        /// <summary>
        /// Retrieves a XProduct object (stored in MATFLO)
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static X_PRODUCT GetXProductFromID(int productID) {
            try { 
            X_PRODUCT retVal = new X_PRODUCT();

            retVal = X_PRODUCT.ReadObject(productID);

            return retVal;
            }
            catch { throw; }
        }
        #endregion

        #region Product Availability - provides the stock counts

        /// <summary>
        /// Returns stock counts for a specific prod code
        /// 
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="principalID"></param>
        /// <returns></returns>
        public static int GetProductAvailability(string productCode, int principalID) {
            try { 
            int retVal = 0;
            //ok ... we need to get the number of the product which is available - we have to check the xproduct db as well as pending orders
            throw new NotImplementedException("Not implemented.");
            //return retVal;
            }
            catch { throw; }
        }

        #endregion

        #region Audit
        public Audit getxAudit(DateTime date, string fileName, string transaction, string transactionType, string user, string eventStatus, string message)
        {

            try
            {
                var data = new Audit().ReadObject(date, fileName, transaction, transactionType, user, eventStatus, message);

                return data;
            }
            catch { throw; }

        }

        public List<dynamic> GetAuditXml(int ID)
        {

            Audit audit = new Audit();

            try
            {
                DataSet ds = audit.GetAuditXml(ID);

                // Test dynamic conversion using extension method
                var result = ds.Tables[0].ToDynamicList("Audit_Records");

                return result;
            }
            catch (Exception ex) { throw; }
        }

        public List<dynamic> readAudits(int PrincipalID , string from_date, string to_date, string audit_fileName, string audit_transaction, string audit_transactionType, string audit_user, string audit_eventStatus, string audit_message)
        {

            Audit audit = new Audit();

            try
            {
                DataSet ds = audit.ReadAudits(PrincipalID,from_date, to_date, audit_fileName, audit_transaction, audit_transactionType, audit_user, audit_eventStatus, audit_message);

                // Test dynamic conversion using extension method
                var result = ds.Tables[0].ToDynamicList("Audit_Records");

                return result;
            }
            catch( Exception ex) { throw; }
        }

        public List<dynamic> readWsAudits(int PrincipalID, string from_date, string to_date, string TransactionId, string TransactionType, string TrResult, string TrDirection)
        {

            Audit audit = new Audit();

            try
            {
                DataSet ds = audit.ReadWsAudits(PrincipalID, from_date, to_date, TransactionId, TransactionType, TrResult, TrDirection);

                // Test dynamic conversion using extension method
                var result = ds.Tables[0].ToDynamicList("Audit_Records");

                return result;
            }
            catch (Exception ex) { throw; }
        }

        public List<dynamic> readParameters()
        {

            Audit audit = new Audit();

            try
            {
                DataSet ds = audit.ReadParameters();

                // Test dynamic conversion using extension method
                var result = ds.Tables[0].ToDynamicList("SysParameters");

                return result;
            }
            catch(Exception exc) { throw; }
        }

        public int UpdateParameter(string paramName, string paramDescription, string paramValue)
        {
            try
            {
                int retVal = 0;
                Audit audit = new Audit(paramName, paramDescription, paramValue);

                retVal = audit.Update(paramName, paramDescription, paramValue);

                return retVal;
            }
            catch { throw; }
        }

        public List<dynamic> GetEmailNotificationList(int PrincipalID)
        {
            Audit audit = new Audit();

            try
            {
                DataSet ds = audit.GetEmailNotificationList(PrincipalID);

                var result = ds.Tables[0].ToDynamicList("NotfList");

                return result;
            }
            catch (Exception) { throw; }
        }

        public int UpdateEmailNotification(int PrincipalID, int EmailNotificationID, bool Enabled, string User)
        {
            try
            {
                int retVal = 0;
                Audit audit = new Audit();

                retVal = audit.UpdateEmailNotification(PrincipalID, EmailNotificationID, Enabled, User);

                return retVal;
            }
            catch { throw; }
        }

        #endregion

    }
}
