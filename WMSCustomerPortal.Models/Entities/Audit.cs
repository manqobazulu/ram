using Microsoft.Practices.EnterpriseLibrary.Data;
using RAM.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace WMSCustomerPortal.Models.Entities
{
    public class Audit
    {
        #region Stored Procedures

        private const string STP_Audit = "usp_SearchAudits";
        private const string STP_Parameters = "usp_GetSysParameters";
        private const string STP_UpdateParameters = "usp_UpdateSysParameters";
        #endregion
        #region Properties


        public System.DateTime auditDate { get; set; }
        public string fileName { get; set; }
        public string transaction { get; set; }
        public string transactionType { get; set; }
        public string user { get; set; }
        public string eventStatus { get; set; }
        public string message { get; set; }

        public string paramName { get; set; }
        public string paramDescription { get; set; }
        public string paramValue { get; set; }


        #endregion

        #region Constructors

        public Audit()
        {


        }


        public Audit(DateTime p_date, string p_fileName, string p_transaction, string p_TransactionType, string p_user, string p_eventStatus, string p_message)
        {
            auditDate = p_date;
            fileName = p_fileName;
            transaction = p_transaction;
            transactionType = p_transaction;
            user = p_user;
            eventStatus = p_eventStatus;
            message = p_message;
        }
        public Audit(string paramName, string paramDescription, string paramValue)
        {
            paramName = this.paramName;
            paramDescription = this.paramDescription;
            paramValue = this.paramValue;
        }

        #endregion

        #region Data Access - Private Methods
        private Audit ReturnObject(DataRow loRow)
        {
            return new Audit(
                Functions.SafeDateTime(loRow["Date"]),
                Functions.SafeString(loRow["FileName"]),

                Functions.SafeString(loRow["Transaction"]),
                Functions.SafeString(loRow["TransactionType"]),
                Functions.SafeString(loRow["User"]),
                Functions.SafeString(loRow["EventStatus"]),
                Functions.SafeString(loRow["Message"])

            );


        }

        private List<Audit> ReturnList(DataTable pdtTable)
        {
            List<Audit> list = new List<Audit>();
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

        #endregion

        #region Data access
        public Audit ReadObject(DateTime date, string fileName, string transaction, string transactionType, string user, string eventStatus, string message)
        {
            DataRow loRow;
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = STP_Audit;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddInParameter(dbCommand, "@Audit_Date", DbType.DateTime, date);
                db.AddInParameter(dbCommand, "@FileName", DbType.String, fileName);
                db.AddInParameter(dbCommand, "@Transaction", DbType.String, transaction);
                db.AddInParameter(dbCommand, "@TransactionType", DbType.String, transactionType);
                db.AddInParameter(dbCommand, "@User", DbType.String, user);
                db.AddInParameter(dbCommand, "@EventStatus", DbType.String, eventStatus);
                db.AddInParameter(dbCommand, "@Message", DbType.String, message);

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

        public DataSet ReadAudits(int PrincipalID,string from_date, string to_date, string fileName, string transaction, string transactionType, string user, string eventStatus, string message)
        {

            DataSet dr = null;

            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = STP_Audit;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, PrincipalID);

                if (from_date != "" && to_date != "")
                {
                    db.AddInParameter(dbCommand, "@From_date", DbType.String, from_date);
                    db.AddInParameter(dbCommand, "@To_date", DbType.String, to_date);
                }
                else if (fileName != "")
                {
                    db.AddInParameter(dbCommand, "@FileName", DbType.String, fileName);
                }
                else if (transaction != "")
                {
                    db.AddInParameter(dbCommand, "@Transaction", DbType.String, transaction);
                }
                else if (transactionType != "")
                {
                    db.AddInParameter(dbCommand, "@TransactionType", DbType.String, transactionType);
                }
                else if (user != "")
                {
                    db.AddInParameter(dbCommand, "@User", DbType.String, user);
                }
                else if (eventStatus != "")
                {
                    db.AddInParameter(dbCommand, "@EventStatus", DbType.String, eventStatus);
                }
                else if (message != "")
                {
                    db.AddInParameter(dbCommand, "@Message", DbType.String, message);
                }

                
                //db.AddInParameter(dbCommand, "@Audit_Date", DbType.String, audit_date);
                //db.AddInParameter(dbCommand, "@FileName", DbType.String, fileName);
                //db.AddInParameter(dbCommand, "@Transaction", DbType.String, transaction);
                //db.AddInParameter(dbCommand, "@TransactionType", DbType.String, transactionType);
                //db.AddInParameter(dbCommand, "@User", DbType.String, user);
                //db.AddInParameter(dbCommand, "@EventStatus", DbType.String, eventStatus);
                //db.AddInParameter(dbCommand, "@Message", DbType.String, message);


                dr = db.ExecuteDataSet(dbCommand);
                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet ReadWsAudits(int PrincipalID, string from_date, string to_date, string TransactionId, string TransactionType, string TrResult, string TrDirection)
        {

            DataSet dr = null;

            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_SearchWsAudits";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, PrincipalID);

                if (from_date != "" && to_date != "")
                {
                    db.AddInParameter(dbCommand, "@From_date", DbType.String, from_date);
                    db.AddInParameter(dbCommand, "@To_date", DbType.String, to_date);
                }
                else if (TransactionId != "")
                {
                    db.AddInParameter(dbCommand, "@TransactionId", DbType.String, TransactionId);
                }
                else if (TransactionType != "")
                {
                    db.AddInParameter(dbCommand, "@TransactionType", DbType.String, TransactionType);
                }
                else if (TrResult != "")
                {
                    db.AddInParameter(dbCommand, "@TrResult", DbType.String, TrResult);
                }
                else if (TrDirection != "")
                {
                    db.AddInParameter(dbCommand, "@TrDirection", DbType.String, TrDirection);
                }

                dr = db.ExecuteDataSet(dbCommand);
                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAuditXml(int ID)
        {

            DataSet dr = null;

            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_GetWsAuditXml";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddInParameter(dbCommand, "@ID", DbType.Int32, ID);

                dr = db.ExecuteDataSet(dbCommand);
                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Parameter
        public DataSet ReadParameters()
        {

            DataSet dr = null;

            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = STP_Parameters;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                dr = db.ExecuteDataSet(dbCommand);
                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(string paramName, string paramDescription, string paramValue)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = STP_UpdateParameters;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddInParameter(dbCommand, "@ParamName", DbType.String, paramName);
                db.AddInParameter(dbCommand, "@ParamDescription", DbType.String, paramDescription);
                db.AddInParameter(dbCommand, "@ParamValue", DbType.String, paramValue);

                db.ExecuteNonQuery(dbCommand);
                return 0;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region EmailNotification

        public DataSet GetEmailNotificationList(int PrincipalID)
        {
            DataSet dr = null;

            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_Principal_Notification_Read";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, PrincipalID);

                dr = db.ExecuteDataSet(dbCommand);
                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateEmailNotification(int PrincipalID, int EmailNotificationID, bool Enabled, string User)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = "usp_Principal_Notification_Update";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, PrincipalID);
                db.AddInParameter(dbCommand, "@EmailNotificationID", DbType.Int32, EmailNotificationID);
                db.AddInParameter(dbCommand, "@Enabled", DbType.Boolean, Enabled);
                db.AddInParameter(dbCommand, "@User", DbType.String, User);

                db.ExecuteNonQuery(dbCommand);
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #endregion
    }

}
