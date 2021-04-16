
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
    /// X_PRODUCT Class
    /// </summary>
    public class X_PRODUCT
    {
        #region Constants

        public static readonly string FLD_REC_NO = "rec_no";
        public static readonly string FLD_SITECODE = "SiteCode";
        public static readonly string FLD_PRINCIPALCODE = "PrincipalCode";
        public static readonly string FLD_PRODCODE = "ProdCode";
        public static readonly string FLD_SHORTDESC = "ShortDesc";
        public static readonly string FLD_LONGDESC = "LongDesc";
        public static readonly string FLD_PRODEAN = "ProdEAN";
        public static readonly string FLD_VOLUME = "volume";
        public static readonly string FLD_WEIGHT = "weight";
        public static readonly string FLD_PRODSTATE = "ProdState";
        public static readonly string FLD_PRODCLASS = "ProdClass";
        public static readonly string FLD_ANALYSIS_A = "Analysis_a";
        public static readonly string FLD_ANALYSIS_B = "Analysis_b";
        public static readonly string FLD_PACKAGINGWEIGHT = "PackagingWeight";
        public static readonly string FLD_PACKQTY = "PackQty";
        public static readonly string FLD_VALIDATECHECKDIGIT = "ValidateCheckDigit";
        public static readonly string FLD_MAXCARTONREPLEN = "MaxCartonReplen";
        public static readonly string FLD_OP_VALID = "OP_Valid";
        public static readonly string FLD_BULKORDERQTY = "BulkOrderQty";
        public static readonly string FLD_OVERSIZE = "OverSize";
        public static readonly string FLD_TRANSFERSERIALS = "TransferSerials";
        public static readonly string FLD_FORBULK = "ForBulk";
        public static readonly string FLD_TEMPLATE = "Template";
        public static readonly string FLD_TMPRODSERIALHOLDER = "TmProdSerialHolder";
        public static readonly string FLD_INHOWMANYBUNDLES = "InHowManyBundles";


        #endregion

        #region Stored Procedures

        //private static readonly string STP_SAVE = "usp_X_PRODUCT_Save";
        private static readonly string STP_READ = "usp_X_PRODUCT_Read";
        private static readonly string STP_READ_ALL = "usp_X_PRODUCT_Read_All";
        
        
        //private static readonly string STP_DELETE = "usp_X_PRODUCT_Delete";

        #endregion

        #region Properties

        private int mrec_no = 0;
        public int rec_no { get { return mrec_no; } set { mrec_no = value; } }

        private string mSiteCode = "";
        public string SiteCode { get { return mSiteCode; } set { mSiteCode = value; } }

        private string mPrincipalCode = "";
        public string PrincipalCode { get { return mPrincipalCode; } set { mPrincipalCode = value; } }

        private string mProdCode = "";
        public string ProdCode { get { return mProdCode; } set { mProdCode = value; } }

        private string mShortDesc = "";
        public string ShortDesc { get { return mShortDesc; } set { mShortDesc = value; } }

        private string mLongDesc = "";
        public string LongDesc { get { return mLongDesc; } set { mLongDesc = value; } }

        private string mProdEAN = "";
        public string ProdEAN { get { return mProdEAN; } set { mProdEAN = value; } }

        private int? mvolume = 0;
        public int? volume { get { return mvolume; } set { mvolume = value; } }

        private int? mweight = 0;
        public int? weight { get { return mweight; } set { mweight = value; } }

        private string mProdState = "";
        public string ProdState { get { return mProdState; } set { mProdState = value; } }

        private string mProdClass = "";
        public string ProdClass { get { return mProdClass; } set { mProdClass = value; } }

        private string mAnalysis_a = "";
        public string Analysis_a { get { return mAnalysis_a; } set { mAnalysis_a = value; } }

        private string mAnalysis_b = "";
        public string Analysis_b { get { return mAnalysis_b; } set { mAnalysis_b = value; } }

        private int? mPackagingWeight = 0;
        public int? PackagingWeight { get { return mPackagingWeight; } set { mPackagingWeight = value; } }

        private int? mPackQty = 0;
        public int? PackQty { get { return mPackQty; } set { mPackQty = value; } }

        private string mValidateCheckDigit = "";
        public string ValidateCheckDigit { get { return mValidateCheckDigit; } set { mValidateCheckDigit = value; } }

        private int? mMaxCartonReplen = 0;
        public int? MaxCartonReplen { get { return mMaxCartonReplen; } set { mMaxCartonReplen = value; } }

        private string mOP_Valid = "";
        public string OP_Valid { get { return mOP_Valid; } set { mOP_Valid = value; } }

        private int? mBulkOrderQty = 0;
        public int? BulkOrderQty { get { return mBulkOrderQty; } set { mBulkOrderQty = value; } }

        private string mOverSize = "";
        public string OverSize { get { return mOverSize; } set { mOverSize = value; } }

        private string mTransferSerials = "";
        public string TransferSerials { get { return mTransferSerials; } set { mTransferSerials = value; } }

        private string mForBulk = "";
        public string ForBulk { get { return mForBulk; } set { mForBulk = value; } }

        private string mTemplate = "";
        public string Template { get { return mTemplate; } set { mTemplate = value; } }

        private int? mTmProdSerialHolder = 0;
        public int? TmProdSerialHolder { get { return mTmProdSerialHolder; } set { mTmProdSerialHolder = value; } }

        private int? mInHowManyBundles = 0;
        public int? InHowManyBundles { get { return mInHowManyBundles; } set { mInHowManyBundles = value; } }


        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor for X_PRODUCT Class
        /// </summary>
        public X_PRODUCT()
        {

        }

        /// <summary>
        /// Constructor for X_PRODUCT Class with Initialization
        /// </summary>
        public X_PRODUCT(int prec_no, string pSiteCode, string pPrincipalCode, string pProdCode, string pShortDesc, string pLongDesc, string pProdEAN, int? pvolume, int? pweight, string pProdState, string pProdClass, string pAnalysis_a, string pAnalysis_b, int? pPackagingWeight, int? pPackQty, string pValidateCheckDigit, int? pMaxCartonReplen, string pOP_Valid, int? pBulkOrderQty, string pOverSize, string pTransferSerials, string pForBulk, string pTemplate, int? pTmProdSerialHolder, int? pInHowManyBundles)
        {
            rec_no = prec_no;
            SiteCode = pSiteCode;
            PrincipalCode = pPrincipalCode;
            ProdCode = pProdCode;
            ShortDesc = pShortDesc;
            LongDesc = pLongDesc;
            ProdEAN = pProdEAN;
            volume = pvolume;
            weight = pweight;
            ProdState = pProdState;
            ProdClass = pProdClass;
            Analysis_a = pAnalysis_a;
            Analysis_b = pAnalysis_b;
            PackagingWeight = pPackagingWeight;
            PackQty = pPackQty;
            ValidateCheckDigit = pValidateCheckDigit;
            MaxCartonReplen = pMaxCartonReplen;
            OP_Valid = pOP_Valid;
            BulkOrderQty = pBulkOrderQty;
            OverSize = pOverSize;
            TransferSerials = pTransferSerials;
            ForBulk = pForBulk;
            Template = pTemplate;
            TmProdSerialHolder = pTmProdSerialHolder;
            InHowManyBundles = pInHowManyBundles;
        }

        /// <summary>
        /// Constructor for X_PRODUCT Class with Initialization
        /// </summary>
        public X_PRODUCT(DataRow loRow)
        {
            rec_no = Functions.SafeInt(loRow["rec_no"], 0);
            SiteCode = Functions.SafeString(loRow["SiteCode"]);
            PrincipalCode = Functions.SafeString(loRow["PrincipalCode"]);
            ProdCode = Functions.SafeString(loRow["ProdCode"]);
            ShortDesc = Functions.SafeString(loRow["ShortDesc"]);
            LongDesc = Functions.SafeString(loRow["LongDesc"]);
            ProdEAN = Functions.SafeString(loRow["ProdEAN"]);
            volume = Functions.SafeInt(loRow["volume"], 0);
            weight = Functions.SafeInt(loRow["weight"], 0);
            ProdState = Functions.SafeString(loRow["ProdState"]);
            ProdClass = Functions.SafeString(loRow["ProdClass"]);
            Analysis_a = Functions.SafeString(loRow["Analysis_a"]);
            Analysis_b = Functions.SafeString(loRow["Analysis_b"]);
            PackagingWeight = Functions.SafeInt(loRow["PackagingWeight"], 0);
            PackQty = Functions.SafeInt(loRow["PackQty"], 0);
            ValidateCheckDigit = Functions.SafeString(loRow["ValidateCheckDigit"]);
            MaxCartonReplen = Functions.SafeInt(loRow["MaxCartonReplen"], 0);
            OP_Valid = Functions.SafeString(loRow["OP_Valid"]);
            BulkOrderQty = Functions.SafeInt(loRow["BulkOrderQty"], 0);
            OverSize = Functions.SafeString(loRow["OverSize"]);
            TransferSerials = Functions.SafeString(loRow["TransferSerials"]);
            ForBulk = Functions.SafeString(loRow["ForBulk"]);
            Template = Functions.SafeString(loRow["Template"]);
            TmProdSerialHolder = Functions.SafeInt(loRow["TmProdSerialHolder"], 0);
            InHowManyBundles = Functions.SafeInt(loRow["InHowManyBundles"], 0);
        }

        #endregion

        #region Data Access - Private Methods

        private static List<X_PRODUCT> ReturnList(DataTable pdtTable)
        {
            List<X_PRODUCT> list = new List<X_PRODUCT>();
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

        private static X_PRODUCT ReturnObject(DataRow loRow)
        {
            return new X_PRODUCT(Functions.SafeInt(loRow["rec_no"], 0),
                Functions.SafeString(loRow["SiteCode"]),
                Functions.SafeString(loRow["PrincipalCode"]),
                Functions.SafeString(loRow["ProdCode"]),
                Functions.SafeString(loRow["ShortDesc"]),
                Functions.SafeString(loRow["LongDesc"]),
                Functions.SafeString(loRow["ProdEAN"]),
                Functions.SafeInt(loRow["volume"], 0),
                Functions.SafeInt(loRow["weight"], 0),
                Functions.SafeString(loRow["ProdState"]),
                Functions.SafeString(loRow["ProdClass"]),
                Functions.SafeString(loRow["Analysis_a"]),
                Functions.SafeString(loRow["Analysis_b"]),
                Functions.SafeInt(loRow["PackagingWeight"], 0),
                Functions.SafeInt(loRow["PackQty"], 0),
                Functions.SafeString(loRow["ValidateCheckDigit"]),
                Functions.SafeInt(loRow["MaxCartonReplen"], 0),
                Functions.SafeString(loRow["OP_Valid"]),
                Functions.SafeInt(loRow["BulkOrderQty"], 0),
                Functions.SafeString(loRow["OverSize"]),
                Functions.SafeString(loRow["TransferSerials"]),
                Functions.SafeString(loRow["ForBulk"]),
                Functions.SafeString(loRow["Template"]),
                Functions.SafeInt(loRow["TmProdSerialHolder"], 0),
                Functions.SafeInt(loRow["InHowManyBundles"], 0));
        }

        #endregion

        #region Data Access

        public static X_PRODUCT ReadObject(int prec_no)
        {
            DataRow loRow;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                string sqlCommand = STP_READ;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, prec_no);

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

        //public static int Save(int prec_no, string pSiteCode, string pPrincipalCode, string pProdCode, string pShortDesc, string pLongDesc, string pProdEAN, int? pvolume, int? pweight, string pProdState, string pProdClass, string pAnalysis_a, string pAnalysis_b, int? pPackagingWeight, int? pPackQty, string pValidateCheckDigit, int? pMaxCartonReplen, string pOP_Valid, int? pBulkOrderQty, string pOverSize, string pTransferSerials, string pForBulk, string pTemplate, int? pTmProdSerialHolder, int? pInHowManyBundles)
        //{
        //    try
        //    {
        //        Database db = DatabaseFactory.CreateDatabase();
        //        string sqlCommand = STP_SAVE;
        //        DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, prec_no, pSiteCode, pPrincipalCode, pProdCode, pShortDesc, pLongDesc, pProdEAN, pvolume, pweight, pProdState, pProdClass, pAnalysis_a, pAnalysis_b, pPackagingWeight, pPackQty, pValidateCheckDigit, pMaxCartonReplen, pOP_Valid, pBulkOrderQty, pOverSize, pTransferSerials, pForBulk, pTemplate, pTmProdSerialHolder, pInHowManyBundles);

        //        return (int)db.ExecuteScalar(dbCommand);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public static int Update(int prec_no, string pSiteCode, string pPrincipalCode, string pProdCode, string pShortDesc, string pLongDesc, string pProdEAN, int? pvolume, int? pweight, string pProdState, string pProdClass, string pAnalysis_a, string pAnalysis_b, int? pPackagingWeight, int? pPackQty, string pValidateCheckDigit, int? pMaxCartonReplen, string pOP_Valid, int? pBulkOrderQty, string pOverSize, string pTransferSerials, string pForBulk, string pTemplate, int? pTmProdSerialHolder, int? pInHowManyBundles)
        //{
        //    try
        //    {
        //        Database db = DatabaseFactory.CreateDatabase();
        //        string sqlCommand = STP_SAVE;
        //        DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, prec_no, pSiteCode, pPrincipalCode, pProdCode, pShortDesc, pLongDesc, pProdEAN, pvolume, pweight, pProdState, pProdClass, pAnalysis_a, pAnalysis_b, pPackagingWeight, pPackQty, pValidateCheckDigit, pMaxCartonReplen, pOP_Valid, pBulkOrderQty, pOverSize, pTransferSerials, pForBulk, pTemplate, pTmProdSerialHolder, pInHowManyBundles);

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

        public static IDataReader Read(int pintID)
        {
            IDataReader dr = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
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

        public static IDataReader ReadAll(string principalCode)
        {
            IDataReader dr = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
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
                Database db = DatabaseFactory.CreateDatabase();
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

        public static List<X_PRODUCT> ReadAll_List(string principalCode)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
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
