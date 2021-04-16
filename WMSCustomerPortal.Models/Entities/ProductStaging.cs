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
    public class ProductStaging
    {
        #region Constants

        public static readonly string FLD_PRODCODE = "ProdCode";
        //public static readonly string FLD_EANCODE = "EANCode";



        public struct ProductQuantities
        {




        }



        #endregion

        #region Stored Procedures

        private const string STP_SAVE = "usp_ProductStaging_Save";
        private const string STP_READ = "usp_ProductStaging_Read";
        private const string STP_READ_ALL = "usp_ProductStaging_Read_All";
        private const string STP_DELETE = "usp_ProductStaging_Delete";
        private const string STP_UPDATE = "usp_ProductStaging_Update";
        private const string STP_READ_MASTER_ALL = "usp_ProductStaging_MasterID_Read";
        private const string STP_FIND_PRODUCT = "usp_ProductStaging_SearchProduct";
        private const string STP_FIND_PRODUCT_GENERIC = "usp_ProductStaging_Search";
        private const string STP_FIND_PRODUCT_FILTER = "usp_ProductStaging_SearchFilter";
        private const string STP_FIND_PRODUCT_EXISTS = "usp_ProductStaging_Exists";
        private const string STP_FIND_EANCODE_EXISTS = "usp_ProductStaging_EANCode_Exists";
        private const string STP_RETRIEVE_PRODUCT_QUANTITIES = "usp_ProductStaging_GetProductQuantities";
        private const string STP_READ_ALL_SHOWINACTIVEFILTER = "usp_ProductStaging_Read_All_ShowInactiveFilter";
        private const string STP_CHECK_SOCK_AVAILABILITY = "usp_X_STOCK_Available";
        private const string SOH_READ_ALL = "usp_RPT_Stock_Available_Qry";
        private const string SOH_READ_ALLaging = "usp_RPT_Stock_Available_Qry1";
        private const string IGD_READ_ALL = "usp_RPT_IGD_Qry";
        private const string RCG_READ_ALL = "usp_RPT_Receiving_Qry";
        private const string DSP_READ_ALL = "usp_RPT_Dispatch_Qry";
        private const string SLA_READ_ALL = "usp_RPT_StockLevelAdjustment_WMS";
        private const string RR_READ_ALL = "usp_RPT_ReturnReceipt_Qry";

        #endregion



        #region Properties


        public int ProductStagingID { get; set; }
        public int PrincipalID { get; set; }

        public string ProdCode { get; set; }
        public string EANCode { get; set; }
        public string ShortDesc { get; set; }
        public string LongDesc { get; set; }
        public bool Serialised { get; set; }
        //public double UnitCost { get; set; }
        public double SalesPrice { get; set; }
        public bool ExpiryProduct { get; set; }
        public int LeadTimeDays { get; set; }
        public bool ProdActive { get; set; }
        public string EventTerminal { get; set; }
        public System.DateTime EventDate { get; set; }
        public string EventUser { get; set; }
        public bool Deleted { get; set; }
        public int XProductQuantity { get; set; }
        public int IGDExpectedQuantity { get; set; }
        public int OrdersInProgressQuantity { get; set; }

        #endregion

        #region Constructors

        public ProductStaging()
        {


        }

        /// <summary>
        /// Constructor for Product_ProductStaging class
        /// </summary>
        /// <param name="pPrincipalCode">The principal for which to query on</param>
        public ProductStaging(int principalID, string eventUser, string eventTerminal)
        {

            PrincipalID = principalID;
            EventTerminal = eventTerminal;
            EventUser = eventUser;
        }


        /// <summary>
        /// Constructor for Product_ProductStaging class
        /// </summary>
        /// <param name="pPrincipalCode">The principal for which to query on</param>
        public ProductStaging(ProductStaging stagingRecord, int principalID, string eventUser, string eventTerminal)
        {

            //we will be overwriting these values with new ones

            PrincipalID = principalID;
            EventTerminal = eventTerminal;
            EventUser = eventUser;

            //Init the new values
            ProductStagingID = stagingRecord.ProductStagingID;
            PrincipalID = principalID;

            ProdCode = stagingRecord.ProdCode.ToUpper();
            EANCode = stagingRecord.EANCode.ToUpper();
            ShortDesc = stagingRecord.ShortDesc.ToUpper();
            LongDesc = stagingRecord.LongDesc.ToUpper();
            Serialised = stagingRecord.Serialised;

            //UnitCost = stagingRecord.UnitCost;
            SalesPrice = stagingRecord.SalesPrice;
            ExpiryProduct = stagingRecord.ExpiryProduct;
            LeadTimeDays = stagingRecord.LeadTimeDays;
            ProdActive = stagingRecord.ProdActive;
            EventTerminal = eventTerminal;
            EventDate = System.DateTime.Now;
            EventUser = eventUser;
            Deleted = false;



        }


        /////////// <summary>
        /////////// Constructor for Product_ProductStaging Class with Initialization
        /////////// </summary>
        public ProductStaging(


        int productStagingID,
        int principalID,
        string pProdCode,
        string pEANCode,
        string pShortDesc,
        string pLongDesc,
        bool serialised,
        //double unitCost,
        double salesPrice,
        bool expiryProduct,
        int leadTimeDays,
        bool prodActive,
        int xProductQuantity,
        int igdExpectedQuantity,
        int ordersInProgressQuantity,
        string pEventTerminal,
        //System.DateTime pEventDate,
        string pEventUser
    //bool pDeleted 

    )
        {


            ProductStagingID = productStagingID;
            PrincipalID = principalID;

            ProdCode = pProdCode.ToUpper();
            EANCode = pEANCode.ToUpper();
            ShortDesc = pShortDesc.ToUpper();
            LongDesc = pLongDesc.ToUpper();

            Serialised = serialised;

            // UnitCost = unitCost;
            SalesPrice = salesPrice;
            ExpiryProduct = expiryProduct;
            LeadTimeDays = leadTimeDays;
            ProdActive = prodActive;

            XProductQuantity = xProductQuantity;
            IGDExpectedQuantity = igdExpectedQuantity;
            OrdersInProgressQuantity = ordersInProgressQuantity;

            EventTerminal = pEventTerminal;
            //EventDate = pEventDate ;
            EventUser = pEventUser;
            //Deleted = pDeleted;
        }



        #endregion

        #region Data Access - Private Methods

        private ArrayList ReturnCollection(DataTable pdtTable)
        {
            ArrayList palCollection = new ArrayList();
            try
            {
                foreach (DataRow loRow in pdtTable.Rows)
                {
                    palCollection.Add(ReturnObject(loRow));
                }
                return palCollection;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<ProductStaging> ReturnList(DataTable pdtTable)
        {
            List<ProductStaging> list = new List<ProductStaging>();
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

        private ProductStaging ReturnObject(DataRow loRow)
        {
            return new ProductStaging(
                Functions.SafeInt(loRow["ProductStagingID"], 0),
                Functions.SafeInt(loRow["PrincipalID"], 0),

                Functions.SafeString(loRow["ProdCode"]),
                Functions.SafeString(loRow["EANCode"]),
                Functions.SafeString(loRow["ShortDesc"]),
                Functions.SafeString(loRow["LongDesc"]),
                Functions.SafeBool(loRow["Serialised"]),
                // Functions.SafeDouble(loRow["UnitCost"],0.00), 
                Functions.SafeDouble(loRow["SalesPrice"], 0.00),
                Functions.SafeBool(loRow["ExpiryProduct"]),
                Functions.SafeInt(loRow["LeadTimeDays"], 0),
                Functions.SafeBool(loRow["ProdActive"], false),


                Functions.SafeInt(loRow["XProductQuantity"], 0),
                Functions.SafeInt(loRow["IGDExpectedQuantity"], 0),
                Functions.SafeInt(loRow["OrdersInProgressQuantity"], 0),


                Functions.SafeString(loRow["EventTerminal"]),
                Functions.SafeString(loRow["EventUser"])

            );


        }

        #endregion

        #region Data Access



        public List<ProductStaging> FindProductsList(string productCode, bool findPartial, int count, bool searchStartOnly, int principalID)
        {

            DataTable dt = new DataTable();
            List<ProductStaging> retValue = new List<ProductStaging>();
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = STP_FIND_PRODUCT;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddInParameter(dbCommand, "@ProductCode", DbType.String, productCode);
                db.AddInParameter(dbCommand, "@FindPartial", DbType.Boolean, findPartial);
                db.AddInParameter(dbCommand, "@SearchStartOnly", DbType.Boolean, searchStartOnly);

                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);
                db.AddInParameter(dbCommand, "@FindCount", DbType.Int32, count);

                dt = db.ExecuteDataSet(dbCommand).Tables[0];



                retValue = this.ReturnList(dt);


                return retValue;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public List<ProductStaging> DoesEANExist(string eanCode, int principalID)
        {
            return ReturnList(DoesEANExistDT(eanCode, principalID));


        }


        public System.Data.DataTable DoesEANExistDT(string eanCode, int principalID)
        {

            DataTable dt = new DataTable();

            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = STP_FIND_EANCODE_EXISTS;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddInParameter(dbCommand, "@EANCode", DbType.String, eanCode);

                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);


                dt = db.ExecuteDataSet(dbCommand).Tables[0];

                return dt;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public List<ProductStaging> FindProductsList(string productCode, string eanCode, string shortDescription, string longDescrption, int principalID)
        {
            return ReturnList(FindProductsDT(productCode, eanCode, shortDescription, longDescrption, principalID));


        }


        public System.Data.DataTable FindProductsDT(string productCode, string eanCode, string shortDescription, string longDescrption, int principalID)
        {

            DataTable dt = new DataTable();

            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = STP_FIND_PRODUCT_EXISTS;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddInParameter(dbCommand, "@ProductCode", DbType.String, productCode);

                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);


                dt = db.ExecuteDataSet(dbCommand).Tables[0];



                return dt;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ProductStaging ReadObject(int productStagingID)
        {
            DataRow loRow;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = STP_READ;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddInParameter(dbCommand, "@ProductStagingID", DbType.Int32, productStagingID);
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


        public void Save()
        {

            //this will insert a new record into the database
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = STP_SAVE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                // db.AddOutParameter(dbCommand, "@ProductStagingID", DbType.Int32, 4);
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, this.PrincipalID);

                db.AddInParameter(dbCommand, "@ProdCode", DbType.String, this.ProdCode);
                db.AddInParameter(dbCommand, "@EANCode", DbType.String, this.EANCode);
                db.AddInParameter(dbCommand, "@ShortDesc", DbType.String, this.ShortDesc);
                db.AddInParameter(dbCommand, "@LongDesc", DbType.String, this.LongDesc);


                db.AddInParameter(dbCommand, "@Serialised", DbType.Boolean, this.Serialised);

                //  db.AddInParameter(dbCommand, "@UnitCost", DbType.Double, this.UnitCost);
                db.AddInParameter(dbCommand, "@SalesPrice", DbType.Double, this.SalesPrice);

                db.AddInParameter(dbCommand, "@ExpiryProduct", DbType.Boolean, this.ExpiryProduct);
                db.AddInParameter(dbCommand, "@LeadTimeDays", DbType.Int32, this.LeadTimeDays);
                db.AddInParameter(dbCommand, "@ProdActive", DbType.Boolean, this.ProdActive);


                db.AddInParameter(dbCommand, "@EventTerminal", DbType.String, this.EventTerminal);
                //  db.AddInParameter(dbCommand, "@EventDate",DbType.DateTime ,this.EventDate );
                db.AddInParameter(dbCommand, "@EventUser", DbType.String, this.EventUser);
                //  db.AddInParameter(dbCommand, "@Deleted", DbType.Boolean, this.Deleted);



                db.ExecuteNonQuery(dbCommand);


                //return (int)db.GetParameterValue(dbCommand, "@ProductStagingID");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update()
        {

            //this will insert a new record into the database
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = STP_UPDATE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddInParameter(dbCommand, "@ProductStagingID", DbType.Int32, this.ProductStagingID);
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, this.PrincipalID);

                db.AddInParameter(dbCommand, "@ProdCode", DbType.String, this.ProdCode);
                db.AddInParameter(dbCommand, "@EANCode", DbType.String, this.EANCode);
                db.AddInParameter(dbCommand, "@ShortDesc", DbType.String, this.ShortDesc);
                db.AddInParameter(dbCommand, "@LongDesc", DbType.String, this.LongDesc);


                db.AddInParameter(dbCommand, "@Serialised", DbType.Boolean, this.Serialised);

                // db.AddInParameter(dbCommand, "@UnitCost", DbType.Double, this.UnitCost);
                db.AddInParameter(dbCommand, "@SalesPrice", DbType.Double, this.SalesPrice);

                db.AddInParameter(dbCommand, "@ExpiryProduct", DbType.Boolean, this.ExpiryProduct);
                db.AddInParameter(dbCommand, "@LeadTimeDays", DbType.Int32, this.LeadTimeDays);
                db.AddInParameter(dbCommand, "@ProdActive", DbType.Boolean, this.ProdActive);

                db.AddInParameter(dbCommand, "@EventTerminal", DbType.String, this.EventTerminal);
                //  db.AddInParameter(dbCommand, "@EventDate",DbType.DateTime ,this.EventDate );
                db.AddInParameter(dbCommand, "@EventUser", DbType.String, this.EventUser);
                //  db.AddInParameter(dbCommand, "@Deleted", DbType.Boolean, this.Deleted);



                db.ExecuteNonQuery(dbCommand);


                return (int)db.GetParameterValue(dbCommand, "@ProductStagingID");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// Marks a record as deleted within the database
        /// </summary>
        /// <param name="detailID">The GUID detailID of the customer</param>
        /// <returns></returns>
        public bool Delete(int productStagingID, string eventTerminal, string eventUser)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = STP_DELETE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, productStagingID, eventTerminal, eventUser);

                // db.AddInParameter(dbCommand, "@ProductStagingID", DbType.String, productStagingID);



                db.ExecuteNonQuery(dbCommand);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="detailID">The detail ID of the record to read </param>
        /// <returns></returns>
        public IDataReader Read(int productStagingID)
        {
            IDataReader dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = STP_READ;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddInParameter(dbCommand, "@ProductStagingID", DbType.Int32, productStagingID);
                dr = db.ExecuteReader(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Reads ALL records ... only for a specified Principal
        /// </summary>
        /// <param name="principalCode">The principal code</param>
        /// <returns>A datareader object</returns>
        public IDataReader ReadAll(int principalID)
        {
            IDataReader dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = STP_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);

                dr = db.ExecuteReader(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Reads ALL records ... only for a specified Principal
        /// </summary>
        /// <param name="principalCode">The principal code</param>
        /// <returns>A Dataset containing the records for the principalCode</returns>
        public DataSet ReadAllDS(int principalID)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = STP_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<dynamic> ReadAllList(int principalID)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = STP_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);

                dr = db.ExecuteDataSet(dbCommand);

                return dr.Tables[0].ToDynamicList("Products");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Retrieves a product list filtering on active / inactive
        /// </summary>
        /// <param name="principalID"></param>
        /// <param name="showInactive"></param>
        /// <returns></returns>
        public List<dynamic> ReadAllList(int principalID, bool showInactive)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = STP_READ_ALL_SHOWINACTIVEFILTER;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);
                db.AddInParameter(dbCommand, "@ProdActive", DbType.Boolean, showInactive);
                dr = db.ExecuteDataSet(dbCommand);

                return dr.Tables[0].ToDynamicList("Products");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string CheckStockQuantity(int PrincipalID, string ProductCode, int Quantity, string ResultOut)
        {

            string dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = STP_CHECK_SOCK_AVAILABILITY;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, PrincipalID, ProductCode, Quantity, ResultOut);

                dr = db.ExecuteScalar(dbCommand).ToString();

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int CheckUnsubmittedQuantity(int PrincipalID, string PrincipalCode, string ProductCode)
        {
            try
            {
                string dr = null;
                int ResultOut = 0;
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_ORD_UnsubmittedQty";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, PrincipalID);
                db.AddInParameter(dbCommand, "@ProdCode", DbType.String, ProductCode);
                db.AddInParameter(dbCommand, "@PrincipalCode", DbType.String, PrincipalCode);
                db.AddOutParameter(dbCommand, "@Result", DbType.Int32, ResultOut);

                dr = db.ExecuteScalar(dbCommand).ToString();

                return Convert.ToInt32(dr);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<dynamic> GetStockOnHand(int principalID, bool showZeroInventory)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = SOH_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);
                db.AddInParameter(dbCommand, "@ShowZeroInventory", DbType.Boolean, showZeroInventory);

                dr = db.ExecuteDataSet(dbCommand);

                return dr.Tables[0].ToDynamicList("StockOnHand");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<dynamic> GetStockOnHandSerials(int principalID, bool showZeroInventory)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_RPT_Stock_Available_Serial_Qry";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);
                db.AddInParameter(dbCommand, "@ShowZeroInventory", DbType.Boolean, showZeroInventory);

                dr = db.ExecuteDataSet(dbCommand);

                return dr.Tables[0].ToDynamicList("StockOnHand");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<dynamic> GetStockAging(int principalID, DateTime DateFrom, DateTime DateTo)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_RPT_Stock_Aging";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);
                db.AddInParameter(dbCommand, "@DateFrom", DbType.Date, DateFrom);
                db.AddInParameter(dbCommand, "@DateTo", DbType.Date, DateTo);

                dr = db.ExecuteDataSet(dbCommand);


                return dr.Tables[0].ToDynamicList("StockOnHand").Distinct().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<dynamic> GetStockAgeData(int principalID)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_RPT_Stock_Age_Qry";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);

                dr = db.ExecuteDataSet(dbCommand);


                return dr.Tables[0].ToDynamicList("StockOnHand").Distinct().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<dynamic> GetStockAgeDetailData(int principalID)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_RPT_Stock_Age_Details_Raw";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);

                dr = db.ExecuteDataSet(dbCommand);


                return dr.Tables[0].ToDynamicList("StockOnHand");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<dynamic> GetNewStockAgeDetailData(int principalID, DateTime DateFrom, DateTime DateTo)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "USP_RPT_StockAgeDetail_QRY";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);
                db.AddInParameter(dbCommand, "@DateFrom", DbType.Date, DateFrom);
                db.AddInParameter(dbCommand, "@DateTo", DbType.Date, DateTo);

                //Set my timeout to 5 minutes
                dbCommand.CommandTimeout = 540;

                dr = db.ExecuteDataSet(dbCommand);
                
                return dr.Tables[0].ToDynamicList("StockOnHand");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<dynamic> GetIGDs(int principalID, DateTime DateFrom, DateTime DateTo)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = IGD_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);
                db.AddInParameter(dbCommand, "@DateFrom", DbType.Date, DateFrom);
                db.AddInParameter(dbCommand, "@DateTo", DbType.Date, DateTo);

                dr = db.ExecuteDataSet(dbCommand);

                return dr.Tables[0].ToDynamicList("IGD");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<dynamic> GetInboundMaster(int principalID, DateTime DateFrom, DateTime DateTo)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = RCG_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);
                db.AddInParameter(dbCommand, "@DateFrom", DbType.Date, DateFrom);
                db.AddInParameter(dbCommand, "@DateTo", DbType.Date, DateTo);

                dr = db.ExecuteDataSet(dbCommand);

                return dr.Tables[0].ToDynamicList("InboundMaster");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<dynamic> GetDispatched(int principalID, DateTime DateFrom, DateTime DateTo, string UserName)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = DSP_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);
                db.AddInParameter(dbCommand, "@DateFrom", DbType.Date, DateFrom);
                db.AddInParameter(dbCommand, "@DateTo", DbType.Date, DateTo);
                db.AddInParameter(dbCommand, "@UserName", DbType.String, UserName);

                dr = db.ExecuteDataSet(dbCommand);

                return dr.Tables[0].ToDynamicList("Dispatched");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 

        public List<dynamic> GetDispatchedSerials(int principalID, DateTime DateFrom, DateTime DateTo, string UserName)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_RPT_Dispatch_Serial_Qry";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);
                db.AddInParameter(dbCommand, "@DateFrom", DbType.Date, DateFrom);
                db.AddInParameter(dbCommand, "@DateTo", DbType.Date, DateTo);
                db.AddInParameter(dbCommand, "@UserName", DbType.String, UserName);

                dr = db.ExecuteDataSet(dbCommand);

                return dr.Tables[0].ToDynamicList("Dispatched");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<dynamic> GetReturnReceipt(int principalID, DateTime DateFrom, DateTime DateTo)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = RR_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);
                db.AddInParameter(dbCommand, "@DateFrom", DbType.Date, DateFrom);
                db.AddInParameter(dbCommand, "@DateTo", DbType.Date, DateTo);

                dr = db.ExecuteDataSet(dbCommand);

                return dr.Tables[0].ToDynamicList("ReturnReceipt");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<dynamic> GetStockLevelAdjustment(int principalID, DateTime DateFrom, DateTime DateTo, string UserName)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = SLA_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);
                db.AddInParameter(dbCommand, "@DateFrom", DbType.Date, DateFrom);
                db.AddInParameter(dbCommand, "@DateTo", DbType.Date, DateTo);
                db.AddInParameter(dbCommand, "@UserName", DbType.String, UserName);

                dr = db.ExecuteDataSet(dbCommand);

                return dr.Tables[0].ToDynamicList("StockLevelAdjustment");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<dynamic> GetIGDVariance(int principalID, DateTime DateFrom, DateTime DateTo, string UserName)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_RPT_IGD_Variance_Qry";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);
                db.AddInParameter(dbCommand, "@DateFrom", DbType.Date, DateFrom);
                db.AddInParameter(dbCommand, "@DateTo", DbType.Date, DateTo);
                db.AddInParameter(dbCommand, "@UserName", DbType.String, UserName);

                dr = db.ExecuteDataSet(dbCommand);

                return dr.Tables[0].ToDynamicList("Dispatched");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
