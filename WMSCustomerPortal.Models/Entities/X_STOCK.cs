
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
    /// X_STOCK Class
    /// </summary>
    public class X_STOCK
    {
        #region Constants

        public static readonly string FLD_REC_NO = "rec_no";
        public static readonly string FLD_PRINCIPALCODE = "PrincipalCode";
        public static readonly string FLD_TMID = "TmId";
        public static readonly string FLD_PRODCODE = "ProdCode";
        public static readonly string FLD_QTY = "Qty";
        public static readonly string FLD_FIFODATE = "FiFoDate";
        public static readonly string FLD_MOVEREF = "MoveRef";
        public static readonly string FLD_STOCKSTATE = "StockState";
        public static readonly string FLD_RECEIPTTYPE = "ReceiptType";
        public static readonly string FLD_HOLDSTATE = "HoldState";
        public static readonly string FLD_USERNAME = "UserName";
        public static readonly string FLD_STQTY = "STQty";
        public static readonly string FLD_STREQD = "STReqd";
        public static readonly string FLD_STCOMPLETE = "STComplete";
        public static readonly string FLD_STRECOUNT = "STRecount";
        public static readonly string FLD_QTYRSVD = "QtyRsvd";


        #endregion

        #region Stored Procedures

       // private static readonly string STP_SAVE = "usp_X_STOCK_Save";
        private static readonly string STP_READ = "usp_X_STOCK_Read";
        private static readonly string STP_READ_ALL = "usp_X_STOCK_Read_All";
       // private static readonly string STP_DELETE = "usp_X_STOCK_Delete";

        #endregion

        #region Properties

        private int mrec_no = 0;
        public int rec_no { get { return mrec_no; } set { mrec_no = value; } }

        private string mPrincipalCode = "";
        public string PrincipalCode { get { return mPrincipalCode; } set { mPrincipalCode = value; } }

        private int? mTmId = 0;
        public int? TmId { get { return mTmId; } set { mTmId = value; } }

        private string mProdCode = "";
        public string ProdCode { get { return mProdCode; } set { mProdCode = value; } }

        private int? mQty = 0;
        public int? Qty { get { return mQty; } set { mQty = value; } }

        private DateTime? mFiFoDate = new DateTime(1900, 1, 1);
        public DateTime? FiFoDate { get { return mFiFoDate; } set { mFiFoDate = value; } }

        private string mMoveRef = "";
        public string MoveRef { get { return mMoveRef; } set { mMoveRef = value; } }

        private string mStockState = "";
        public string StockState { get { return mStockState; } set { mStockState = value; } }

        private string mReceiptType = "";
        public string ReceiptType { get { return mReceiptType; } set { mReceiptType = value; } }

        private string mHoldState = "";
        public string HoldState { get { return mHoldState; } set { mHoldState = value; } }

        private string mUserName = "";
        public string UserName { get { return mUserName; } set { mUserName = value; } }

        private int? mSTQty = 0;
        public int? STQty { get { return mSTQty; } set { mSTQty = value; } }

        private string mSTReqd = "";
        public string STReqd { get { return mSTReqd; } set { mSTReqd = value; } }

        private string mSTComplete = "";
        public string STComplete { get { return mSTComplete; } set { mSTComplete = value; } }

        private int? mSTRecount = 0;
        public int? STRecount { get { return mSTRecount; } set { mSTRecount = value; } }

        private int? mQtyRsvd = 0;
        public int? QtyRsvd { get { return mQtyRsvd; } set { mQtyRsvd = value; } }


        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor for X_STOCK Class
        /// </summary>
        public X_STOCK()
        {

        }

        /// <summary>
        /// Constructor for X_STOCK Class with Initialization
        /// </summary>
        public X_STOCK(int prec_no, string pPrincipalCode, int? pTmId, string pProdCode, int? pQty, DateTime? pFiFoDate, string pMoveRef, string pStockState, string pReceiptType, string pHoldState, string pUserName, int? pSTQty, string pSTReqd, string pSTComplete, int? pSTRecount, int? pQtyRsvd)
        {
            rec_no = prec_no;
            PrincipalCode = pPrincipalCode;
            TmId = pTmId;
            ProdCode = pProdCode;
            Qty = pQty;
            FiFoDate = pFiFoDate;
            MoveRef = pMoveRef;
            StockState = pStockState;
            ReceiptType = pReceiptType;
            HoldState = pHoldState;
            UserName = pUserName;
            STQty = pSTQty;
            STReqd = pSTReqd;
            STComplete = pSTComplete;
            STRecount = pSTRecount;
            QtyRsvd = pQtyRsvd;
        }

        /// <summary>
        /// Constructor for X_STOCK Class with Initialization
        /// </summary>
        public X_STOCK(DataRow loRow)
        {
            rec_no = Functions.SafeInt(loRow["rec_no"], 0);
            PrincipalCode = Functions.SafeString(loRow["PrincipalCode"]);
            TmId = Functions.SafeInt(loRow["TmId"], 0);
            ProdCode = Functions.SafeString(loRow["ProdCode"]);
            Qty = Functions.SafeInt(loRow["Qty"], 0);
            FiFoDate = Functions.SafeDateTime(loRow["FiFoDate"]);
            MoveRef = Functions.SafeString(loRow["MoveRef"]);
            StockState = Functions.SafeString(loRow["StockState"]);
            ReceiptType = Functions.SafeString(loRow["ReceiptType"]);
            HoldState = Functions.SafeString(loRow["HoldState"]);
            UserName = Functions.SafeString(loRow["UserName"]);
            STQty = Functions.SafeInt(loRow["STQty"], 0);
            STReqd = Functions.SafeString(loRow["STReqd"]);
            STComplete = Functions.SafeString(loRow["STComplete"]);
            STRecount = Functions.SafeInt(loRow["STRecount"], 0);
            QtyRsvd = Functions.SafeInt(loRow["QtyRsvd"], 0);
        }

        #endregion

        #region Data Access - Private Methods

        private static List<X_STOCK> ReturnList(DataTable pdtTable)
        {
            List<X_STOCK> list = new List<X_STOCK>();
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

        private static X_STOCK ReturnObject(DataRow loRow)
        {
            return new X_STOCK(Functions.SafeInt(loRow["rec_no"], 0),
                Functions.SafeString(loRow["PrincipalCode"]),
                Functions.SafeInt(loRow["TmId"], 0),
                Functions.SafeString(loRow["ProdCode"]),
                Functions.SafeInt(loRow["Qty"], 0),
                Functions.SafeDateTime(loRow["FiFoDate"]),
                Functions.SafeString(loRow["MoveRef"]),
                Functions.SafeString(loRow["StockState"]),
                Functions.SafeString(loRow["ReceiptType"]),
                Functions.SafeString(loRow["HoldState"]),
                Functions.SafeString(loRow["UserName"]),
                Functions.SafeInt(loRow["STQty"], 0),
                Functions.SafeString(loRow["STReqd"]),
                Functions.SafeString(loRow["STComplete"]),
                Functions.SafeInt(loRow["STRecount"], 0),
                Functions.SafeInt(loRow["QtyRsvd"], 0));
        }

        #endregion

        #region Data Access

        public static X_STOCK ReadObject(string principalCode, string prodCode)
        {
            DataRow loRow;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, principalCode, prodCode);

                
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

        //public static int Save(int prec_no, string pPrincipalCode, int? pTmId, string pProdCode, int? pQty, DateTime? pFiFoDate, string pMoveRef, string pStockState, string pReceiptType, string pHoldState, string pUserName, int? pSTQty, string pSTReqd, string pSTComplete, int? pSTRecount, int? pQtyRsvd)
        //{
        //    try
        //    {
        //        Database db = DatabaseFactory.CreateDatabase();
        //        string sqlCommand = STP_SAVE;
        //        DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, prec_no, pPrincipalCode, pTmId, pProdCode, pQty, pFiFoDate, pMoveRef, pStockState, pReceiptType, pHoldState, pUserName, pSTQty, pSTReqd, pSTComplete, pSTRecount, pQtyRsvd);

        //        return (int)db.ExecuteScalar(dbCommand);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static int Update(int prec_no, string pPrincipalCode, int? pTmId, string pProdCode, int? pQty, DateTime? pFiFoDate, string pMoveRef, string pStockState, string pReceiptType, string pHoldState, string pUserName, int? pSTQty, string pSTReqd, string pSTComplete, int? pSTRecount, int? pQtyRsvd)
        //{
        //    try
        //    {
        //        Database db = DatabaseFactory.CreateDatabase();
        //        string sqlCommand = STP_SAVE;
        //        DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, prec_no, pPrincipalCode, pTmId, pProdCode, pQty, pFiFoDate, pMoveRef, pStockState, pReceiptType, pHoldState, pUserName, pSTQty, pSTReqd, pSTComplete, pSTRecount, pQtyRsvd);

        //        return (int)db.ExecuteNonQuery(dbCommand);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public static bool Delete(int pintID)
        //{
        //    try
        //    {
        //        Database db = DatabaseFactory.CreateDatabase();
        //        string sqlCommand = STP_DELETE;
        //        DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pintID);

        //        db.ExecuteNonQuery(dbCommand);

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public static IDataReader Read( string principalCode, string prodCode)
        {
            IDataReader dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, principalCode, prodCode);

                dr = db.ExecuteReader(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IDataReader ReadAll(string principalCode)
        {
            IDataReader dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, principalCode);

                dr = db.ExecuteReader(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet ReadAllDS(string principalCode)
        {
            DataSet dr = null;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, principalCode);

                dr = db.ExecuteDataSet(dbCommand);

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<X_STOCK> ReadAll_List(string principalCode)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                string sqlCommand = STP_READ_ALL;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, principalCode);

                return ReturnList(db.ExecuteDataSet(dbCommand).Tables[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
