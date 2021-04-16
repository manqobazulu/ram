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
    /// OrderLineItems Class
    /// </summary>
    public class OrderLineItems
    {
        #region Constants

        public static readonly string FLD_LINEITEMID = "LineItemID";
        public static readonly string FLD_ORDERID = "OrderID";
        public static readonly string FLD_WMSHOSTORDERID = "WMSHostOrderID";
        public static readonly string FLD_PRODUCTSTAGINGID = "ProductStagingID";
        public static readonly string FLD_LINENUMBER = "LineNumber";
        public static readonly string FLD_LINETYPE = "LineType";
        public static readonly string FLD_SUBMITTEDPRODUCTCODE = "SubmittedProductCode";
        public static readonly string FLD_LINETEXT = "LineText";
        public static readonly string FLD_QUANTITY = "Quantity";
        public static readonly string FLD_SUBMITTEDUNITCOST = "SubmittedUnitCost";
        public static readonly string FLD_SUBMITTEDRAMORDERNUMBER = "SubmittedRAMOrderNumber";
        public static readonly string FLD_SUBMITTEDVAT = "SubmittedVAT";
        public static readonly string FLD_SUBMITTEDUNITDISCOUNTAMOUNT = "SubmittedUnitDiscountAmount";
        public static readonly string FLD_SUBMITTEDTOWMS = "SubmittedToWMS";
        public static readonly string FLD_EVENTUSER = "EventUser";
        public static readonly string FLD_EVENTTERMINAL = "EventTerminal";
        public static readonly string FLD_EVENTDATE = "EventDate";
        public static readonly string FLD_DELETED = "Deleted";


        #endregion

        #region Stored Procedures

        private static readonly string STP_SAVE = "usp_OrderLineItems_Save";
        private static readonly string STP_READ = "usp_OrderLineItems_Read";
        private static readonly string STP_READ_ALL = "usp_OrderLineItems_Read_All";
        private static readonly string STP_DELETE = "usp_OrderLineItems_Delete";
        private static readonly string STP_SETWMS = "usp_OrderLineItems_SetWMSSubmitted";
        private static readonly string STP_READ_ALL_LINKED_PRINCIPALID = "usp_LinkedPrincipalID_Read_All";
        private static readonly string STP_READ_ALL_CUSTOMER = "usp_Customer_Filter";
        private static readonly string STP_READ__CUSTOMER_DETAIL = "usp_CustomerDetail_Lookup";
        private static readonly string STP_READ__ORDER_DETAIL = "usp_OrderDetail_Lookup";
      

        #endregion

        #region Properties

        private int mLineItemID = 0;
        public int LineItemID { get { return mLineItemID; } set { mLineItemID = value; } }

        private int mOrderID = 0;
        public int OrderID { get { return mOrderID; } set { mOrderID = value; } }

        private int mWMSHostOrderID = 0;
        public int WMSHostOrderID { get { return mWMSHostOrderID; } set { mWMSHostOrderID = value; } }

        private int mProductStagingID = 0;
        public int ProductStagingID { get { return mProductStagingID; } set { mProductStagingID = value; } }

        private string mLineNumber = "";
        public string LineNumber { get { return mLineNumber; } set { mLineNumber = value; } }

        private string mLineType = "";
        public string LineType { get { return mLineType; } set { mLineType = value; } }

        private string mSubmittedProductCode = "";
        public string SubmittedProductCode { get { return mSubmittedProductCode.ToUpper(); } set { mSubmittedProductCode = value.ToUpper(); } }

        private string mSubmittedRAMOrderNumber = "";
        public string SubmittedRAMOrderNumber { get { return mSubmittedRAMOrderNumber.ToUpper(); } set { mSubmittedRAMOrderNumber = value.ToUpper(); } }

        private int? mQuantity = 0;
        public int? Quantity { get { return mQuantity; } set { mQuantity = value; } }

        private double? mSubmittedUnitCost = 0;
        public double? SubmittedUnitCost { get { return mSubmittedUnitCost; } set { mSubmittedUnitCost = value; } }
        
        private string mLineText = "";
        public string LineText { get { return mLineText.ToUpper(); } set { mLineText = value.ToUpper(); } }

        private double? mSubmittedVAT = 0;
        public double? SubmittedVAT { get { return mSubmittedVAT; } set { mSubmittedVAT = value; } }

        private double? mSubmittedUnitDiscountAmount = 0;
        public double? SubmittedUnitDiscountAmount { get { return mSubmittedUnitDiscountAmount; } set { mSubmittedUnitDiscountAmount = value; } }

        private bool mSubmittedToWMS = false;
        public bool SubmittedToWMS { get { return mSubmittedToWMS; } set { mSubmittedToWMS = value; } }

        private string mEventUser = "";
        public string EventUser { get { return mEventUser; } set { mEventUser = value; } }

        private string mEventTerminal = "";
        public string EventTerminal { get { return mEventTerminal; } set { mEventTerminal = value; } }



        private string mProductCode = "";
        public string ProductCode { get { return mProductCode.ToUpper(); } set { mProductCode = value.ToUpper(); } }

        private string mEANCode = "";
        public string EANCode { get { return mEANCode.ToUpper(); } set { mEANCode = value.ToUpper(); } }

        private string mShortDescription = "";
        public string ShortDescription { get { return mShortDescription.ToUpper(); } set { mShortDescription = value.ToUpper(); } }

        private string mLongDescription = "";
        public string LongDescription { get { return mLongDescription.ToUpper(); } set { mLongDescription = value.ToUpper(); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor for OrderLineItems Class
        /// </summary>
        public OrderLineItems()
        {

        }

        /// <summary>
        /// Constructor for OrderLineItems Class with Initialization
        /// </summary>
        public OrderLineItems(int pLineItemID, int pOrderID,
            int pWMSHostOrderID,
            int pProductStagingID, string pLineNumber, string pLineType, 
            string pSubmittedProductCode, string pLineText, int? pQuantity, double? pSubmittedUnitCost, 
            string pSubmittedRAMOrderNumber,
            double? pSubmittedVAT, 
            double? pSubmittedUnitDiscountAmount, bool pSubmittedToWMS, string pEventUser, string pEventTerminal,
            string pProductCode, string pEANCode, string pShortDescription, string pLongDescription)
        {
            LineItemID = pLineItemID;
            OrderID = pOrderID;
            WMSHostOrderID = pWMSHostOrderID;
            ProductStagingID = pProductStagingID;
            LineNumber = pLineNumber;
            LineType = pLineType;
            SubmittedProductCode = pSubmittedProductCode;
            LineText = pLineText;
            Quantity = pQuantity;
            SubmittedUnitCost = pSubmittedUnitCost;
            SubmittedRAMOrderNumber = pSubmittedRAMOrderNumber;
            SubmittedVAT = pSubmittedVAT;
            SubmittedUnitDiscountAmount = pSubmittedUnitDiscountAmount;
            SubmittedToWMS = pSubmittedToWMS;
            EventUser = pEventUser;
            EventTerminal = pEventTerminal;
            //EventDate = pEventDate;
            //Deleted = pDeleted;

            ProductCode = pProductCode;
            EANCode = pEANCode;
            ShortDescription = pShortDescription;
            LongDescription = pLongDescription;
        
        }

        /// <summary>
        /// Constructor for OrderLineItems Class with Initialization
        /// </summary>
        public OrderLineItems(DataRow loRow)
        {
            LineItemID = Functions.SafeInt(loRow["LineItemID"], 0);
            OrderID = Functions.SafeInt(loRow["OrderID"], 0);
            WMSHostOrderID = Functions.SafeInt(loRow["WMSHostOrderID"], 0);
            ProductStagingID = Functions.SafeInt(loRow["ProductStagingID"], 0);
            LineNumber = Functions.SafeString(loRow["LineNumber"]);
            LineType = Functions.SafeString(loRow["LineType"]);
            SubmittedProductCode = Functions.SafeString(loRow["SubmittedProductCode"]);
            LineText = Functions.SafeString(loRow["LineText"]);
            Quantity = Functions.SafeInt(loRow["Quantity"], 0);
            SubmittedUnitCost = Functions.SafeDouble(loRow["SubmittedUnitCost"], 0);
            SubmittedRAMOrderNumber = Functions.SafeString(loRow["SubmittedRAMOrderNumber"]);
            SubmittedVAT = Functions.SafeDouble(loRow["SubmittedVAT"], 0);
            SubmittedUnitDiscountAmount = Functions.SafeDouble(loRow["SubmittedUnitDiscountAmount"], 0);
            SubmittedToWMS = Functions.SafeBool(loRow["SubmittedToWMS"]);
            EventUser = Functions.SafeString(loRow["EventUser"]);
            EventTerminal = Functions.SafeString(loRow["EventTerminal"]);
            //EventDate = Functions.SafeDateTime(loRow["EventDate"]);
            //Deleted = Functions.SafeBool(loRow["Deleted"]);

            ProductCode = Functions.SafeString(loRow["ProductCode"]);
            EANCode = Functions.SafeString(loRow["EANCode"]);
            ShortDescription = Functions.SafeString(loRow["ShortDescription"]);
            LongDescription = Functions.SafeString(loRow["LongDescription"]);
        }

        #endregion

        #region Data Access - Private Methods

        private static List<OrderLineItems> ReturnList(DataTable pdtTable)
        {
            List<OrderLineItems> list = new List<OrderLineItems>();
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

        private static OrderLineItems ReturnObject(DataRow loRow)
        {
            return new OrderLineItems(Functions.SafeInt(loRow["LineItemID"], 0),
                Functions.SafeInt(loRow["OrderID"], 0),
                Functions.SafeInt(loRow["WMSHostOrderID"],0),
                Functions.SafeInt(loRow["ProductStagingID"], 0),
                Functions.SafeString(loRow["LineNumber"]),
                Functions.SafeString(loRow["LineType"]),
                Functions.SafeString(loRow["SubmittedProductCode"]).ToUpper(),
                Functions.SafeString(loRow["LineText"]).ToUpper(),
                Functions.SafeInt(loRow["Quantity"], 0),
                Functions.SafeDouble(loRow["SubmittedUnitCost"], 0),
                Functions.SafeString(loRow["SubmittedRAMOrderNumber"]),
                Functions.SafeDouble(loRow["SubmittedVAT"], 0),
                Functions.SafeDouble(loRow["SubmittedUnitDiscountAmount"], 0),
                Functions.SafeBool(loRow["SubmittedToWMS"]),
                Functions.SafeString(loRow["EventUser"]),
                Functions.SafeString(loRow["EventTerminal"]),

                Functions.SafeString(loRow["ProductCode"]).ToUpper(),
                Functions.SafeString(loRow["EANCode"]).ToUpper(),
                Functions.SafeString(loRow["ShortDescription"]).ToUpper(),
                Functions.SafeString(loRow["LongDescription"]).ToUpper()
                             
                );
        }

        #endregion

        #region Data Access

        public static OrderLineItems ReadObject(int pLineItemID)
        {
            DataRow loRow;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pLineItemID);

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

        public static int Save(int pLineItemID, int pOrderID, int pWMSHostOrderID, int pProductStagingID, string pLineNumber, string pLineType, string pSubmittedProductCode, string pLineText, int? pQuantity, double? pSubmittedUnitCost, string pSubmittedRAMOrderNumber, double? pSubmittedVAT, double? pSubmittedUnitDiscountAmount, bool pSubmittedToWMS, string pEventUser, string pEventTerminal)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_SAVE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pLineItemID, pOrderID, pWMSHostOrderID, pProductStagingID, pLineNumber, pLineType, pSubmittedProductCode, pLineText, pQuantity, pSubmittedUnitCost, pSubmittedRAMOrderNumber, pSubmittedVAT, pSubmittedUnitDiscountAmount, pSubmittedToWMS, pEventUser, pEventTerminal);

                db.ExecuteScalar(dbCommand);

                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       public static int Update(int pOrderID, int pCustomerDetailID, int pPrincipalID, DateTime? pDateCreated, string pEventUser, string pEventTerminal, string Table)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_SAVE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pOrderID, pCustomerDetailID, pPrincipalID, pDateCreated, pEventUser, pEventTerminal, Table);

                return (int)db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        public static IDataReader ReadAll(int orderID)
        {
            IDataReader dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, orderID);

                dr = db.ExecuteReader(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet ReadAllDS(int orderID)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, orderID);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet GetCustomerDetail(string customerID, int principalID) {
            DataSet dr = null;
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = STP_READ__CUSTOMER_DETAIL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, customerID, principalID);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public static DataSet GetOrderDetail(int OrderID) {
            DataSet dr = null;
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = STP_READ__ORDER_DETAIL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, OrderID);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public static DataSet GetOrderStatus(int OrderID)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = "usp_OrderStatus_Lookup";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, OrderID);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet SearchCustomerDS(string customerName)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = STP_READ_ALL_CUSTOMER;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, customerName);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet ReadAllLinkedPrincipalIDDS(int PrincipalID, string filter)
        {
            DataSet dr = null;
            try
            {
              Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
              string sqlCommand = STP_READ_ALL_LINKED_PRINCIPALID;
              DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, PrincipalID, filter);

              dr = db.ExecuteDataSet(dbCommand);

              return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public static List<OrderLineItems> ReadAll_List(int orderID)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, orderID);

                return ReturnList(db.ExecuteDataSet(dbCommand).Tables[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<OrderLineItems> ReturnReadAll_List(int orderID)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = "usp_GetReturnOrderLineItems"; 
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, orderID);

                return ReturnList(db.ExecuteDataSet(dbCommand).Tables[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //  private static readonly string STP_SETWMS = "usp_OrderLineItems_SetWMSSubmitted";
        public static int SetWMSSubmitted(int lineItemID,
                                            bool submitted,
                                            string eventTerminal,
                                            string  eventUser)
         {

             try
             {
                 Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                 string sqlCommand = STP_SETWMS;
                 DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, lineItemID, submitted, eventTerminal, eventUser);

                 return db.ExecuteNonQuery(dbCommand);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

        #endregion
    }
}