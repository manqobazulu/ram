
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
	/// Orders Class
	/// </summary>
	public class Orders
	{
		#region Constants
		
		public static readonly string FLD_ORDERID = "OrderID";
		public static readonly string FLD_CUSTOMERDETAILID = "CustomerDetailID";
        public static readonly string FLD_PRINCIPALID = "PrincipalID";
		public static readonly string FLD_RAMORDERNUMBER = "RAMOrderNumber";
		public static readonly string FLD_DATECREATED = "DateCreated";
		public static readonly string FLD_CUSTOMERORDERNUMBER = "CustomerOrderNumber";
		public static readonly string FLD_PRIORITY = "Priority";
		public static readonly string FLD_VALUEADDEDPACKAGING = "ValueAddedPackaging";
		public static readonly string FLD_SALESPERSON = "SalesPerson";
		public static readonly string FLD_SALESCATEGORY = "SalesCategory";
		public static readonly string FLD_PROCESSOR = "Processor";
		public static readonly string FLD_ORDERDISCOUNT = "OrderDiscount";
		public static readonly string FLD_ORDERVAT = "OrderVAT";
		public static readonly string FLD_EVENTUSER = "EventUser";
		public static readonly string FLD_EVENTTERMINAL = "EventTerminal";
		public static readonly string FLD_EVENTDATE = "EventDate";
		public static readonly string FLD_DELETED = "Deleted";
	
		
		#endregion
		
		#region Stored Procedures

        private static readonly string STP_SAVE = "usp_Order_OrderLineItems_Update";
        private static readonly string STP_SAVE_ORDER_MULTIPLE_RECEIVER = "usp_Order_Save_Multiple_Receiver";
        private static readonly string STP_SAVE_ORDER_SINGLE_RECEIVER = "usp_Order_Save_Single_Receiver";
        private static readonly string STP_SAVE_ORDER_NUMBER = "usp_Orders_SaveWithRamOrderNumber";
		private static readonly string STP_READ = "usp_Orders_Read";
		private static readonly string STP_READ_ALL = "usp_Orders_Read_All";
        private static readonly string STP_READ_ALL_CUSTOMER_ORDER_NUMBER = "usp_Orders_Read_All_Customer_Order_Number";
        private static readonly string STP_READ_ALL_COMPLETE = "usp_Orders_Read_All_Complete";
		private static readonly string STP_DELETE = "usp_Orders_Delete";
        private static readonly string STP_SUBMITTED = "usp_Orders_Complete";
        private static readonly string STP_FILTER = "usp_Orders_Filter";
        private static readonly string Outbound_RPT_READ_ALL = "usp_RPT_Activity_Outbound_Detail"; //Change This


		#endregion

		#region Properties

		private int mOrderID = 0;
		public int OrderID { get { return mOrderID; } set { mOrderID = value; } } 

		private int mCustomerDetailID = 0;
		public int CustomerDetailID { get { return mCustomerDetailID; } set { mCustomerDetailID = value; } }

        
        private int mBillingCustomerDetailID = 0;
		public int BillingCustomerDetailID { get { return mBillingCustomerDetailID; } set { mBillingCustomerDetailID = value; } }

        private string mReceiverRAMCustomerID = "";
        public string ReceiverRAMCustomerID {get{return mReceiverRAMCustomerID;} set {mReceiverRAMCustomerID = value;}}

         private string mBillingRAMCustomerID= "";
        public string BillingRAMCustomerID {get{return mBillingRAMCustomerID;} set {mBillingRAMCustomerID = value;}}
     

        private int mPrincipalID = 0;
        public int PrincipalID { get { return mPrincipalID; } set { mPrincipalID = value; } } 

		private string mRAMOrderNumber = "";
        public string RAMOrderNumber { get { return mRAMOrderNumber.ToUpper(); } set { mRAMOrderNumber = value.ToUpper(); } } 

		private DateTime? mDateCreated = new DateTime(1900, 1, 1);
		public DateTime? DateCreated { get { return mDateCreated; } set { mDateCreated = value; } } 

		private string mCustomerOrderNumber = "";
        public string CustomerOrderNumber { get { return mCustomerOrderNumber.ToUpper(); } set { mCustomerOrderNumber = value.ToUpper(); } } 

		private string mPriority = "";
		public string Priority { get { return mPriority; } set { mPriority = value; } } 

		private bool? mValueAddedPackaging = false;
		public bool? ValueAddedPackaging { get { return mValueAddedPackaging; } set { mValueAddedPackaging = value; } } 

		private string mSalesPerson = "";
        public string SalesPerson { get { return mSalesPerson.ToUpper(); } set { mSalesPerson = value.ToUpper(); } } 

		private string mSalesCategory = "";
        public string SalesCategory { get { return mSalesCategory.ToUpper(); } set { mSalesCategory = value.ToUpper(); } } 

		private string mProcessor = "";
		public string Processor { get { return mProcessor; } set { mProcessor = value; } } 

		private double? mOrderDiscount = 0;
		public double? OrderDiscount { get { return mOrderDiscount; } set { mOrderDiscount = value; } } 

		private double? mOrderVAT = 0;
		public double? OrderVAT { get { return mOrderVAT; } set { mOrderVAT = value; } } 

		private string mEventUser = "";
		public string EventUser { get { return mEventUser; } set { mEventUser = value; } }

        private string mIDNumber = "";
        public string IDNumber { get { return mIDNumber; } set { mIDNumber = value; } }

        private string mKYC = "";
        public string KYC { get { return mKYC; } set { mKYC = value; } }

        private string mCourierName = "";
        public string CourierName { get { return mCourierName; } set { mCourierName = value; } }


        private string mCourierService = "";
        public string CourierService { get { return mCourierService; } set { mCourierService = value; } }

        private bool mInsuranceRequired = false;
        public bool InsuranceRequired { get { return mInsuranceRequired; } set { mInsuranceRequired = value; } }

        private bool mValidateDelivery = false;
        public bool  ValidateDelivery { get { return mValidateDelivery; } set { mValidateDelivery = value; } }

        private string mEventTerminal = "";
		public string EventTerminal { get { return mEventTerminal; } set { mEventTerminal = value; } }

        private string mRAMCustomerID = "";
        public string RAMCustomerID { get { return mRAMCustomerID.ToUpper(); } set { mRAMCustomerID = value.ToUpper(); } }

        private string mStoreCode = "";
        public string StoreCode { get { return mStoreCode.ToUpper(); } set { mStoreCode = value.ToUpper(); } }
        
        private string mCustomerName = "";
        public string CustomerName { get { return mCustomerName.ToUpper(); } set { mCustomerName = value.ToUpper(); } }

        private string mCustomerGroupID = "";
        public string CustomerGroupID { get { return mCustomerGroupID; } set { mCustomerGroupID = value; } } 
			
       
		#endregion
		
		#region Constructors
		
		/// <summary>
		/// Default Constructor for Orders Class
		/// </summary>
		public Orders()
		{

		}
		
		/// <summary>
		/// Constructor for Orders Class with Initialization
		/// </summary>
        /// 

        public Orders(int pOrderID, int pCustomerDetailID, int pBillingCustomerDetailID, string pReceiverRAMCustomerID, string pBillingRAMCustomerID, int pPrincipalID, string pRAMOrderNumber, DateTime? pDateCreated, 
            string pCustomerOrderNumber, string pPriority, bool? pValueAddedPackaging, string pSalesPerson, 
            string pSalesCategory, string pProcessor, double? pOrderDiscount, double? pOrderVAT, string pEventUser, string pEventTerminal,
            string pRAMCustomerID, string pStoreCode, string pCustomerName, string pCustomerGroupID
            )
		{
			OrderID = pOrderID;
			CustomerDetailID = pCustomerDetailID;
            BillingCustomerDetailID = pBillingCustomerDetailID;
            ReceiverRAMCustomerID = pReceiverRAMCustomerID;
            BillingRAMCustomerID = pBillingRAMCustomerID;
            PrincipalID = pPrincipalID;
			RAMOrderNumber = pRAMOrderNumber;
			DateCreated = pDateCreated;
			CustomerOrderNumber = pCustomerOrderNumber;
			Priority = pPriority;
			ValueAddedPackaging = pValueAddedPackaging;
			SalesPerson = pSalesPerson;
			SalesCategory = pSalesCategory;
			Processor = pProcessor;
			OrderDiscount = pOrderDiscount;
			OrderVAT = pOrderVAT;
			EventUser = pEventUser;
			EventTerminal = pEventTerminal;

            RAMCustomerID =pRAMCustomerID;
            StoreCode= pStoreCode;
            CustomerName=pCustomerName;
            CustomerGroupID = pCustomerGroupID;
            //EventDate = pEventDate;
            //Deleted = pDeleted;
		}

		/// <summary>
		/// Constructor for Orders Class with Initialization
		/// </summary>
		public Orders(DataRow loRow)
		{	
			OrderID = Functions.SafeInt(loRow["OrderID"], 0);
			CustomerDetailID = Functions.SafeInt(loRow["CustomerDetailID"], 0);
            BillingCustomerDetailID = Functions.SafeInt(loRow["BillingCustomerDetailID"], 0);
            ReceiverRAMCustomerID = Functions.SafeString(loRow["ReceiverRAMCustomerID"]);
            BillingRAMCustomerID = Functions.SafeString(loRow["BillingRAMCustomerID"]);
            PrincipalID = Functions.SafeInt(loRow["PrincipalID"], 0);
			RAMOrderNumber = Functions.SafeString(loRow["RAMOrderNumber"]);
			DateCreated = Functions.SafeDateTime(loRow["DateCreated"]);
			CustomerOrderNumber = Functions.SafeString(loRow["CustomerOrderNumber"]);
			Priority = Functions.SafeString(loRow["Priority"]);
			ValueAddedPackaging = Functions.SafeBool(loRow["ValueAddedPackaging"]);
			SalesPerson = Functions.SafeString(loRow["SalesPerson"]);
			SalesCategory = Functions.SafeString(loRow["SalesCategory"]);
			Processor = Functions.SafeString(loRow["Processor"]);
			OrderDiscount = Functions.SafeDouble(loRow["OrderDiscount"], 0);
			OrderVAT = Functions.SafeDouble(loRow["OrderVAT"], 0);
			EventUser = Functions.SafeString(loRow["EventUser"]);
			EventTerminal = Functions.SafeString(loRow["EventTerminal"]);
            //EventDate = Functions.SafeDateTime(loRow["EventDate"]);
            //Deleted = Functions.SafeBool(loRow["Deleted"]);
            RAMCustomerID = Functions.SafeString(loRow["RAMCustomerID"]);
            StoreCode = Functions.SafeString(loRow["StoreCode"]);
            CustomerName = Functions.SafeString(loRow["CustomerName"]);
            CustomerGroupID = Functions.SafeString(loRow["CustomerGroupID"]);
		}
		
		#endregion
		
		#region Data Access - Private Methods
	
		private static List<Orders> ReturnList(DataTable pdtTable)
		{
			List<Orders> list = new List<Orders>();
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
				throw new Exception(ex.Message);
			}
		}
    

		private static Orders ReturnObject(DataRow loRow)
		{
            return new Orders(Functions.SafeInt(loRow["OrderID"], 0),
                Functions.SafeInt(loRow["CustomerDetailID"], 0),
                Functions.SafeInt(loRow["BillingCustomerDetailID"], 0),
                Functions.SafeString(loRow["ReceiverRAMCustomerID"]),
                Functions.SafeString(loRow["BillingRAMCustomerID"]),
                Functions.SafeInt(loRow["PrincipalID"], 0),
                Functions.SafeString(loRow["RAMOrderNumber"]).ToUpper(),
                Functions.SafeDateTime(loRow["DateCreated"]),
                Functions.SafeString(loRow["CustomerOrderNumber"]).ToUpper(),
                Functions.SafeString(loRow["Priority"]),
                Functions.SafeBool(loRow["ValueAddedPackaging"]),
                Functions.SafeString(loRow["SalesPerson"]).ToUpper(),
                Functions.SafeString(loRow["SalesCategory"]),
                Functions.SafeString(loRow["Processor"]),
                Functions.SafeDouble(loRow["OrderDiscount"], 0),
                Functions.SafeDouble(loRow["OrderVAT"], 0),
                Functions.SafeString(loRow["EventUser"]),
                Functions.SafeString(loRow["EventTerminal"]),
                Functions.SafeString(loRow["RAMCustomerID"]).ToUpper(),
                Functions.SafeString(loRow["StoreCode"]).ToUpper(),
                Functions.SafeString(loRow["CustomerName"]).ToUpper(),
                Functions.SafeString(loRow["CustomerGroupID"]).ToUpper()
                
                );

           
              
		}		
		
		#endregion

		#region Data Access
		
		public static Orders ReadObject(int pOrderID)
		{
			DataRow loRow;          
		    try
		    {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
				string sqlCommand = STP_READ;
				DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pOrderID);
				
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
	
        
      
		public static int Save(int pOrderID, int pCustomerDetailID, int pBillingCustomerDetailID, string pReceiverRAMCustomerID, string pBillingRAMCustomerID, int pPrincipalID, string pRAMOrderNumber, DateTime? pDateCreated, string pCustomerOrderNumber, string pPriority, bool? pValueAddedPackaging, string pSalesPerson, string pSalesCategory, string pProcessor, double? pOrderDiscount, double? pOrderVAT, string pEventUser, string pEventTerminal, string pCustomerID, bool pSubmitted)
		{
			try
			{
                int count;

                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_SAVE_ORDER_NUMBER;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pOrderID, pCustomerDetailID, pBillingCustomerDetailID, pReceiverRAMCustomerID, pBillingRAMCustomerID, pPrincipalID, pRAMOrderNumber, pDateCreated, pCustomerOrderNumber, pPriority, pValueAddedPackaging, pSalesPerson, pSalesCategory, pProcessor, pOrderDiscount, pOrderVAT, pEventUser, pEventTerminal, pCustomerID, pSubmitted);
				
                //count = (int)db.ExecuteScalar(dbCommand);

                count = Convert.ToInt32(db.ExecuteScalar(dbCommand));
                return count;
			}        
			catch (Exception ex)
			{
				throw ex;
			} 	
		}

        public static int SaveOrderSingleReceiver(int pOrderID, int pCustomerDetailID, int pBillingCustomerDetailID, string pReceiverRAMCustomerID, string pBillingRAMCustomerID, int pPrincipalID, string pCustomerOrderNumber, DateTime? pDateCreated, string pEventUser, string pEventTerminal, bool pSubmitted, string Table)
        {
            try {
                int count;

                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = STP_SAVE_ORDER_SINGLE_RECEIVER;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pOrderID, pCustomerDetailID, pBillingCustomerDetailID, pReceiverRAMCustomerID, pBillingRAMCustomerID, pPrincipalID, pCustomerOrderNumber, pEventUser, pEventTerminal, pSubmitted, Table); 
               
                //count = (int)db.ExecuteScalar(dbCommand);

                count = Convert.ToInt32(db.ExecuteScalar(dbCommand));

                //count =  int.Parse(db.ExecuteScalar(dbCommand));
                return count;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        //bulk 
        public static int SaveOrderSingleReceiver(int pOrderID, int pCustomerDetailID, int pBillingCustomerDetailID, string pReceiverRAMCustomerID, string pBillingRAMCustomerID, int pPrincipalID, string pCustomerOrderNumber, DateTime? pDateCreated, string pEventUser, string IDNumber, string KYC, string CourierName, string CourierService, bool InsuranceRequired,bool ValidateDelivery, string pEventTerminal, bool pSubmitted, string HeaderComment, string Table)
        {
            try
            {
                int count;

                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = "usp_Order_Save_Single_ReceiverBulk";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pOrderID, pCustomerDetailID, pBillingCustomerDetailID, pReceiverRAMCustomerID, pBillingRAMCustomerID, pPrincipalID, pCustomerOrderNumber, IDNumber, KYC, CourierName, CourierService, InsuranceRequired, ValidateDelivery, pEventUser, pEventTerminal, pSubmitted, HeaderComment, Table);

                //count = (int)db.ExecuteScalar(dbCommand);

                count = Convert.ToInt32(db.ExecuteScalar(dbCommand));

                //count =  int.Parse(db.ExecuteScalar(dbCommand));
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet SaveOrderMultipleReceiver(string pCustomerOrderNumber, int pOrderID, int pPrincipalID, DateTime? pDateCreated, string pEventUser, string pEventTerminal, bool pSubmitted, string pProduct, string pReceiver) {
            try {

                DataSet dr = null;

                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = STP_SAVE_ORDER_MULTIPLE_RECEIVER;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pOrderID, pPrincipalID, pCustomerOrderNumber, pEventUser, pEventTerminal, pSubmitted, pProduct, pReceiver);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public static int Update(int pOrderID, int pCustomerDetailID, int pBillingCustomerDetailID, string pReceiverRAMCustomerID, string pBillingRAMCustomerID, int pPrincipalID, string pRAMOrderNumber, DateTime? pDateCreated, string pCustomerOrderNumber, string pPriority, bool? pValueAddedPackaging, string pSalesPerson, string pSalesCategory, string pProcessor, double? pOrderDiscount, double? pOrderVAT, string pEventUser, string pEventTerminal)
		{
			try
			{
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
				string sqlCommand = STP_SAVE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pOrderID, pCustomerDetailID, pBillingCustomerDetailID, pReceiverRAMCustomerID, pBillingRAMCustomerID, pPrincipalID, pRAMOrderNumber, pDateCreated, pCustomerOrderNumber, pPriority, pValueAddedPackaging, pSalesPerson, pSalesCategory, pProcessor, pOrderDiscount, pOrderVAT, pEventUser, pEventTerminal);
				
				return (int)db.ExecuteNonQuery(dbCommand);
			}        
			catch (Exception ex)
			{
				throw ex;
			} 	
		}


        public static int UpdateOrderAndOrderline(int pOrderID, int pCustomerDetailID, string pCustomerID, int pPrincipalID, string pEventUser, string pEventTerminal, string Table) {
  
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = STP_SAVE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pOrderID, pCustomerDetailID, pCustomerID, pPrincipalID, pEventUser, pEventTerminal, Table);

                return (int)db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex) {
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

        public static bool Complete(int pintID, string eventTerminal, string eventUser) {
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = STP_SUBMITTED;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pintID, eventTerminal, eventUser);

                db.ExecuteNonQuery(dbCommand);
                return true;
            }
            catch (Exception ex) {
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

    
		public static DataSet ReadAllDS(int principalID, string EventUser)
        {
			DataSet dr = null;
			try
		    {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
				string sqlCommand = STP_READ_ALL;
				DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, principalID, EventUser);
				
				dr = db.ExecuteDataSet(dbCommand);

				return dr;				
		    }        
		    catch (Exception ex)
		    {
			    throw ex;
		    }	
		}

        public static DataSet ReadAllCompleteDS(int principalID, string EventUser)
        {
			DataSet dr = null;
			try
		    {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ_ALL_COMPLETE;
				DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, principalID, EventUser);
				
				dr = db.ExecuteDataSet(dbCommand);

				return dr;				
		    }        
		    catch (Exception ex)
		    {
			    throw ex;
		    }	
		}
    
		public static List<Orders> ReadAll_List(int principalID)
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

        public static List<Orders> ReadAll_ListByCustomerOrderNumber(string CustomerOrderNumber, int PrincipalID) {
            try {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", "")); ;
                string sqlCommand = STP_READ_ALL_CUSTOMER_ORDER_NUMBER;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, CustomerOrderNumber, PrincipalID);

                return ReturnList(db.ExecuteDataSet(dbCommand).Tables[0]);
            }
            catch (Exception ex) {
                throw ex;
            }
        }


        public static List<Orders> SearchList(int principalID, bool listCompleted, string clientPORef, string ramPORef)
        {
            return ReturnList(SearchDS(principalID, listCompleted, clientPORef, ramPORef).Tables[0]);

        }

        public static System.Data.DataSet SearchDS(int principalID, bool listCompleted, string clientPORef, string ramPORef)
        {

            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_FILTER;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, principalID, listCompleted, clientPORef, ramPORef) ;

                return db.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            } 	

        }

		public List<dynamic> GetOutbound(int principalID, DateTime DateFrom, DateTime DateTo, string UserName)
		{
			DataSet dr = null;
			try
			{
				Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
				string sqlCommand = Outbound_RPT_READ_ALL;

				DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
				db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);
				db.AddInParameter(dbCommand, "@DateFrom", DbType.Date, DateFrom);
				db.AddInParameter(dbCommand, "@DateTo", DbType.Date, DateTo);
				db.AddInParameter(dbCommand, "@UserName", DbType.String, UserName);

				//Set my timeout to 5 minutes
				dbCommand.CommandTimeout = 420;

				dr = db.ExecuteDataSet(dbCommand);
				
				return dr.Tables[0].ToDynamicList("Outbound");
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		#endregion
	}
}
