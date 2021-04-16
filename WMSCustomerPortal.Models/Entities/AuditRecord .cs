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
    public class AuditRecord
    {
        #region Stored Procedures

        private const string STP_SAVE = "usp_Audit_Save";
        #endregion

        #region Constructor
        public AuditRecord()
        {


        }


        #endregion

        #region Properties
        public string FileName { get; set; }
        public string TransactionType { get; set; }
        public string Transaction { get; set; }
        public string User { get; set; }
        public string EventStatus { get; set; }
        public string Message { get; set; }
        #endregion

        #region Data access method

        public int Save(int PrincipalID)
        {

            //this will insert a new record into the database
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = STP_SAVE;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddOutParameter(dbCommand,"@Audit_Id", DbType.Int32, 4);
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, PrincipalID);
                db.AddInParameter(dbCommand, "@Audit_FileName", DbType.String, this.FileName);
                db.AddInParameter(dbCommand, "@Audit_Transaction", DbType.String, this.Transaction);
                db.AddInParameter(dbCommand, "@Audit_TransactionType", DbType.String, this.TransactionType);
                db.AddInParameter(dbCommand, "@Audit_User", DbType.String, this.User);
                db.AddInParameter(dbCommand, "@Audit_EventStatus", DbType.String, this.EventStatus);
                db.AddInParameter(dbCommand, "@Audit_Message", DbType.String, this.Message);

                db.ExecuteNonQuery(dbCommand);

                return (int)db.GetParameterValue(dbCommand, "@Audit_Id");
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
    }
}

        #endregion
    

