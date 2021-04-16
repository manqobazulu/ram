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
    class Audit_Log
    {
        #region Stored Procedures

        private const string STP_SAVE = "usp_Audit_Logs";

        #endregion

        #region Properties

        public DateTime Date { get; set; }
        public string FileName { get; set; }
        public string Transaction { get; set; }
        public string TransactionType { get; set; }
        public string User { get; set; }
        public string EventStatus { get; set; }
        public string Message { get; set; }
        public string EventTerminal { get; set; }
        public string EventUser { get; set; }
        #endregion

        #region Constractors

        public Audit_Log()
        {

        }
        
        public Audit_Log(Audit_Log audit_Log,string eventUser, string eventTerminal, DateTime date, string filename, string transaction,
            string transactiontype,string eventstatus,string message)
        {
            //we will be overwriting these values with new ones
            EventTerminal = eventTerminal;
            EventUser = eventUser;

            //Inti
            Date = audit_Log.Date;
            FileName = audit_Log.FileName;
            Transaction = audit_Log.Transaction;
            TransactionType = audit_Log.TransactionType;
            EventStatus = audit_Log.EventStatus;
            Message = audit_Log.Message;

        }

        public Audit_Log(string aEventUser, DateTime adateTime, string aTransaction, string aMessage, string aTransactionType)
        {
            EventUser = aEventUser;
            Date = adateTime;
            Transaction = aTransaction;
            Message = aMessage;
            Transaction = aTransactionType;
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

        private List<Audit_Log> ReturnList(DataTable pdtTable)
        {
            List<Audit_Log> list = new List<Audit_Log>();
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

        #region Data Access

        public int Save()
        {

            //this will insert a new record into the database
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = STP_SAVE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddInParameter(dbCommand, "@PrincipalID", DbType.DateTime, this.Date);
                db.AddInParameter(dbCommand, "@ProdCode", DbType.String, this.FileName);
                db.AddInParameter(dbCommand, "@EANCode", DbType.String, this.TransactionType);
                db.AddInParameter(dbCommand, "@ShortDesc", DbType.String, this.Transaction);
                db.AddInParameter(dbCommand, "@LongDesc", DbType.String, this.EventUser);
                db.AddInParameter(dbCommand, "@Serialised", DbType.String, this.EventStatus);
                db.AddInParameter(dbCommand, "@SalesPrice", DbType.String, this.Message);

                db.ExecuteNonQuery(dbCommand);


                return (int)db.GetParameterValue(dbCommand, "@ProductStagingID");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion



        private Audit_Log ReturnObject(DataRow loRow)
        {
            return new Audit_Log(
                Functions.SafeString(loRow["EventUser"]),
                Functions.SafeDateTime(loRow["Date"]),
                Functions.SafeString(loRow["Transaction"]),
                Functions.SafeString(loRow["Message"]),
                Functions.SafeString(loRow["TransactionType"])
            );


        }
    }
}
