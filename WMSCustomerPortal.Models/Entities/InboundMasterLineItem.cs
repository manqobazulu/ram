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
   [Serializable]
    public class InboundMasterLineItem
    {
        #region Constants

      
        #endregion

        #region Stored Procedures

        private static readonly string STP_SAVE = "usp_InboundMasterLineItem_Save";
        private static readonly string STP_READ = "usp_InboundMasterLineItem_Read";
        private static readonly string STP_READ_ALL = "usp_InboundMasterLineItem_Read_All";
        private static readonly string STP_READ_ALL_WAREHOUSE = "usp_InboundMasterLineItem_Read_All_Warehouse";
        private static readonly string STP_READ_ALL_WAREHOUSE_MASTER = "usp_InboundMasterLineItem_Read_All_Warehouse_MasterID";
        private static readonly string STP_DELETE = "usp_InboundMasterLineItem_Delete";
        private static readonly string STP_GET_RECEIVEDINWAREHOUSEQUANTITY = "usp_InboundMasterLineItem_WHQuantity";
        private static readonly string STP_CHECKDELETEABLE = "usp_InboundMasterLineItem_CheckDeletable";

        #endregion

        #region Properties

        private int mInboundMasterLineItemID = 0;
        public int InboundMasterLineItemID { get { return mInboundMasterLineItemID; } set { mInboundMasterLineItemID = value; } }

        private int mInboundMasterID = 0;
        public int InboundMasterID { get { return mInboundMasterID; } set { mInboundMasterID = value; } }

        private int mProductStagingID = 0;
        public int ProductStagingID { get { return mProductStagingID; } set { mProductStagingID = value; } }

        private int mExpectedQuantity = 0;
        public int ExpectedQuantity { get { return mExpectedQuantity; } set { mExpectedQuantity = value; } }

        

        private string mEventTerminal = "";
        public string EventTerminal { get { return mEventTerminal; } set { mEventTerminal = value; } }

        private DateTime mEventDate = new DateTime(1900, 1, 1);
        public DateTime EventDate { get { return mEventDate; } set { mEventDate = value; } }

        private string mEventUser = "";
        public string EventUser { get { return mEventUser; } set { mEventUser = value; } }

        private bool mDeleted = false;
        public bool Deleted { get { return mDeleted; } set { mDeleted = value; } }

        //Product staging details merged in ...
        
       // ProductData.ProductStagingID = Functions.SafeInt(loRow["ProductStagingID"],0);
        private int mPrincipalID = 0;
            public int PrincipalID  { get { return mPrincipalID; } set { mPrincipalID = value; } }  //Functions.SafeInt(loRow["PrincipalID"],0);
        private string mProdCode  = "";
        public string ProdCode { get { return mProdCode; } set { mProdCode = value; } } //Functions.SafeString(loRow["ProdCode"]);
        private string mEANCode = "";
        public string EANCode { get { return mEANCode; } set { mEANCode = value; } } //Functions.SafeString(loRow["EANCode"]);
        private string mShortDesc = "";
        public string ShortDesc { get { return mShortDesc; } set { mShortDesc = value; } } //Functions.SafeString(loRow["ShortDesc"]);
        private string mLongDesc = "";
        public string LongDesc { get { return mLongDesc; } set { mLongDesc = value; } } //Functions.SafeString(loRow["LongDesc"]);
        private bool mSerialised = false;
        public bool Serialised  { get { return mSerialised; } set {mSerialised  = value; } } //Functions.SafeBool(loRow["Serialised"]);
        private double mUnitCost = 0;
        public double UnitCost  { get { return mUnitCost; } set { mUnitCost = value; } }  //Functions.SafeDouble(loRow["UnitCost"]);

            //private string mItemGuid = "";
            //public string ItemGuid { get { return mItemGuid; } set { mItemGuid = value; } } 
       
       private double mSalesPrice = 0.00;
       public double SalesPrice   { get { return mSalesPrice; } set { mSalesPrice = value; } } //Functions.SafeDouble(loRow["SalesPrice"]);
       private bool mExpiryProduct = false;
       public bool ExpiryProduct  { get { return mExpiryProduct; } set {mExpiryProduct  = value; } } // Functions.SafeBool(loRow["ExpiryProduct"]);
       private int mLeadTimeDays = 0;
       public int LeadTimeDays  { get { return mLeadTimeDays; } set { mLeadTimeDays = value; } } //Functions.SafeInt(loRow["LeadTimeDays"],0);
       private int mReceivedInWarehouseQuantity = 0;
       public int ReceivedInWarehouseQuantity { get { return mReceivedInWarehouseQuantity; } set { mReceivedInWarehouseQuantity = value; } }


        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor for InboundMasterLineItem Class
        /// </summary>
        public InboundMasterLineItem()
        {

        }
        public InboundMasterLineItem(int pProductStagingID, int pExpectedQuantity, 
                                
                                    double pUnitCost
                                    

    )
        {
           
            ProductStagingID = pProductStagingID;
            ExpectedQuantity = pExpectedQuantity;
                       //ProductData = productData;
                 UnitCost = pUnitCost;
            

        }


        public InboundMasterLineItem(int pProductStagingID, int pExpectedQuantity,

                                   double pUnitCost,string productData


   )
        {

            ProductStagingID = pProductStagingID;
            ExpectedQuantity = pExpectedQuantity;
            ProdCode = productData;
            UnitCost = pUnitCost;


        }


        /// <summary>
        /// Constructor for InboundMasterLineItem Class with Initialization
        /// </summary>
        public InboundMasterLineItem(int pInboundMasterLineItemID, int pInboundMasterID, int pProductStagingID, int pExpectedQuantity, string pEventTerminal, DateTime pEventDate, string pEventUser, bool pDeleted,
                                            int pPrincipalID,
                                            string pProdCode,
                                            string pEANCode,
                                            string pShortDesc,
                                            string pLongDesc,
                                            bool pSerialised,
                                            double pUnitCost,
                                            double pSalesPrice,
                                            bool pExpiryProduct,
                                            int pLeadTimeDays,
                                            int receivedInWarehouseQuantity

            )
        {
            InboundMasterLineItemID = pInboundMasterLineItemID;
            InboundMasterID = pInboundMasterID;
            ProductStagingID = pProductStagingID;
            ExpectedQuantity = pExpectedQuantity;
            EventTerminal = pEventTerminal;
            EventDate = pEventDate;
            EventUser = pEventUser;
            Deleted = pDeleted;
            //ProductData = productData;
            PrincipalID = pPrincipalID;
            ProdCode = pProdCode;
            EANCode = pEANCode;
            ShortDesc = pShortDesc;
            LongDesc = pLongDesc;
            Serialised = pSerialised;
            UnitCost = pUnitCost;
            SalesPrice = pSalesPrice;
            ExpiryProduct = pExpiryProduct;
            LeadTimeDays = pLeadTimeDays;
            ReceivedInWarehouseQuantity = receivedInWarehouseQuantity;

        }

        /// <summary>
        /// Constructor for InboundMasterLineItem Class with Initialization
        /// </summary>
        public InboundMasterLineItem(DataRow loRow)
        {
            InboundMasterLineItemID = Functions.SafeInt(loRow["InboundMasterLineItemID"], 0);
            InboundMasterID = Functions.SafeInt(loRow["InboundMasterID"], 0);
            ProductStagingID = Functions.SafeInt(loRow["ProductStagingID"], 0);
            ExpectedQuantity = Functions.SafeInt(loRow["ExpectedQuantity"], 0);
            EventTerminal = Functions.SafeString(loRow["EventTerminal"]);
            EventDate = Functions.SafeDateTime(loRow["EventDate"]);
            EventUser = Functions.SafeString(loRow["EventUser"]);
            Deleted = Functions.SafeBool(loRow["Deleted"]);
            PrincipalID =Functions.SafeInt(loRow["PrincipalID"],0);
            ProdCode =Functions.SafeString(loRow["ProdCode"]);
            EANCode =Functions.SafeString(loRow["EANCode"]);
            ShortDesc =Functions.SafeString(loRow["ShortDesc"]);
            LongDesc = Functions.SafeString(loRow["LongDesc"]);
            Serialised =Functions.SafeBool(loRow["Serialised"]);
            UnitCost = Functions.SafeDouble(loRow["UnitCost"],0.00);
            SalesPrice = Functions.SafeDouble(loRow["SalesPrice"],0.00);
            ExpiryProduct = Functions.SafeBool(loRow["ExpiryProduct"]);
            LeadTimeDays =Functions.SafeInt(loRow["LeadTimeDays"],0);
            ReceivedInWarehouseQuantity = Functions.SafeInt(loRow["ReceivedInWarehouseQuantity"], 0);   
           
        }

        #endregion

        #region Data Access - Private Methods

        private static List<InboundMasterLineItem> ReturnList(DataTable pdtTable)
        {
            List<InboundMasterLineItem> list = new List<InboundMasterLineItem>();
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

        private static InboundMasterLineItem ReturnObject(DataRow loRow)
        {
                      
            
            return new InboundMasterLineItem(Functions.SafeInt(loRow["InboundMasterLineItemID"], 0),
                Functions.SafeInt(loRow["InboundMasterID"], 0),
                Functions.SafeInt(loRow["ProductStagingID"], 0),
                Functions.SafeInt(loRow["ExpectedQuantity"], 0),
                Functions.SafeString(loRow["EventTerminal"]),
                Functions.SafeDateTime(loRow["EventDate"]),
                Functions.SafeString(loRow["EventUser"]),
                Functions.SafeBool(loRow["Deleted"]),
                Functions.SafeInt(loRow["PrincipalID"], 0),
                Functions.SafeString(loRow["ProdCode"]).ToUpper(),
                Functions.SafeString(loRow["EANCode"]).ToUpper(),
                Functions.SafeString(loRow["ShortDesc"]).ToUpper(),
                Functions.SafeString(loRow["LongDesc"]).ToUpper(),
                Functions.SafeBool(loRow["Serialised"]),
                Functions.SafeDouble(loRow["UnitCost"],0.00),
                Functions.SafeDouble(loRow["SalesPrice"],0.00),
                Functions.SafeBool(loRow["ExpiryProduct"]),
                Functions.SafeInt(loRow["LeadTimeDays"], 0),
                Functions.SafeInt(loRow["ReceivedInWarehouseQuantity"],0)    
                );
        
        
        }

        #endregion

        #region Data Access

        public static InboundMasterLineItem ReadObject(int pInboundMasterLineItemID)
        {
            DataRow loRow;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pInboundMasterLineItemID);

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

        public static int Save(int pInboundMasterLineItemID, int pInboundMasterID, int pProductStagingID, int pExpectedQuantity, float pUnitCost, string pEventTerminal, DateTime pEventDate, string pEventUser, bool pDeleted)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_SAVE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pInboundMasterLineItemID, pInboundMasterID, pProductStagingID, pExpectedQuantity, pUnitCost, pEventTerminal, pEventDate, pEventUser, pDeleted);

                return (int)db.ExecuteScalar(dbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int Save(InboundMasterLineItem stagingRecord, string eventTerminal, string eventUser)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_SAVE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand,
                    stagingRecord.InboundMasterLineItemID,
                    stagingRecord.InboundMasterID,
                    stagingRecord.ProductStagingID,
                    stagingRecord.ExpectedQuantity,
                    stagingRecord.UnitCost,
                    eventTerminal,
                    eventUser
                    );
               // return (int)db.ExecuteScalar(dbCommand);

                db.ExecuteNonQuery(dbCommand);
                return (int)db.GetParameterValue(dbCommand, "@InboundMasterLineItemID");
           
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int Update(int pInboundMasterLineItemID, int pInboundMasterID, int pProductStagingID, int pExpectedQuantity, float pUnitCost, string pEventTerminal,  string pEventUser)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_SAVE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pInboundMasterLineItemID, pInboundMasterID, pProductStagingID, pExpectedQuantity, pUnitCost, pEventTerminal, pEventUser);

                return (int)db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int Update(InboundMasterLineItem stagingRecord, string eventUser, string eventTerminal)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_SAVE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand,
                    stagingRecord.InboundMasterLineItemID,
                    stagingRecord.InboundMasterID,
                    stagingRecord.ProductStagingID,
                    stagingRecord.ExpectedQuantity,
                    stagingRecord.UnitCost,
                    eventTerminal,
                    eventUser
                    );

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


                db.AddInParameter(dbCommand, "@InboundMasterLineItemID", DbType.Int32, recordID);
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


        public static bool Delete(int pintID, string eventTerminal, string eventUser )
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

        public static IDataReader ReadAll(int inboundMasterID)
        {
            IDataReader dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, inboundMasterID);

                dr = db.ExecuteReader(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet ReadAllDS(int inboundMasterID)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, inboundMasterID);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


       public static DataSet ReadAllDSWarehouse(int masterID)
        {
            DataSet dr = null;
            try
            {


                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ_ALL_WAREHOUSE_MASTER;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, masterID);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static List<InboundMasterLineItem> ReadAll_List(int inboundMasterID)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, inboundMasterID);

                return ReturnList(db.ExecuteDataSet(dbCommand).Tables[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


       public static int GetReceivedInWarehouseQuantity(int inboundMasterLineItemID)
        {

            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_GET_RECEIVEDINWAREHOUSEQUANTITY;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);


                db.AddInParameter(dbCommand, "@InboundMasterLineItemID", DbType.Int32, inboundMasterLineItemID);
                db.AddOutParameter(dbCommand,  "@ReceivedInWarehouseQuantity", DbType.Int32, 4);

                db.ExecuteNonQuery(dbCommand);


                return Functions.SafeInt(db.GetParameterValue(dbCommand, "@ReceivedInWarehouseQuantity"),0);
                
            
            
            
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    


        #endregion
    }

}
