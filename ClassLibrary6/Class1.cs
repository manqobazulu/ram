using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using RAM.Utilities.Common;
using WMSCustomerPortal.Models.Entities;

namespace ClassLibrary6
{
    public class Class1
    {
        public Class1()
        {

        }

        #region Properties

        public int PrincipalID { get; set; }
        public string PrincipalCode { get; set; }
        public string PrincipalName { get; set; }
        public string EventUser { get; set; }
        public string EventTerminal { get; set; }
        public string Prefix { get; set; }
        public string FileDelimiter { get; set; }
        public System.DateTime EventDate { get; set; }
        public bool Deleted { get; set; }
        public string CustomerGroupID { get; set; }
        public string FilePath { get; set; }

        #endregion


        public Class1(
            Class1 stagingRecord,
            string pEventUser,
                string pEventTerminal)
        {

            PrincipalID = stagingRecord.PrincipalID;
            PrincipalName = stagingRecord.PrincipalName.ToUpper();
            PrincipalCode = stagingRecord.PrincipalCode.ToUpper();
            CustomerGroupID = stagingRecord.CustomerGroupID;
            Prefix = stagingRecord.Prefix.ToUpper();
            FileDelimiter = stagingRecord.FileDelimiter;
            FilePath = stagingRecord.FilePath;

            EventUser = pEventUser;
            EventTerminal = pEventTerminal;
        }
        public Class1(int pCustomerPrincipalID, string pCustomerPrincipalCode, string pCustomerGroupID, string pPrincipalName, string pEventUser, string pEventTerminal, string pFileDelimiter,string pFilePath)
        {
            PrincipalID = pCustomerPrincipalID;
            PrincipalName = pPrincipalName.ToUpper();
            CustomerGroupID = pCustomerGroupID;
            PrincipalCode = pCustomerPrincipalCode.ToUpper();
            EventUser = pEventUser;
            EventTerminal = pEventTerminal;
            FileDelimiter = pFileDelimiter;
            FilePath = pFilePath;
        }


        private Class1 ReturnObject(DataRow loRow)
        {
            return new Class1(
             Functions.SafeInt(loRow["PrincipalID"], 0),
             Functions.SafeString(loRow["PrincipalCode"]),
             Functions.SafeString(loRow["CustomerGroupID"]),
             Functions.SafeString(loRow["PrincipalName"]),
              Functions.SafeString(loRow["EventUser"]),
             Functions.SafeString(loRow["EventTerminal"]),
              Functions.SafeString(loRow["FileDelimiter"]),
              Functions.SafeString(loRow["FilePath"])
            );
        }
        public Class1 ReadObject(int principalID)
        {

            DataRow loRow;
            try
            {

                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));//

                string sqlCommand = "usp_Principal_Read";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);

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
                return null;
            }
        }

        //public Class1 ReadObjectAll()
        //{

        //    DataRow dr = null;
        //    try
        //    {
        //        Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
        //        string sqlCommand = "usp_Principal_Read_All";

        //        DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);


        //        dr = db.ExecuteDataSet(dbCommand).Tables[0].Rows[0];
               


        //        return ReturnObject(dr);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}




        public List<Class1> ReadAllList()
        {
            List<Class1> customers = new List<Class1>();
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));

                // Create an output row mapper that maps all properties based on the column names
                IRowMapper<Class1> mapper = MapBuilder<Class1>.BuildAllProperties();

                // Create a stored procedure accessor that uses this output mapper
                var accessor = db.CreateSprocAccessor("usp_Principal_Read_All", mapper);

                var data = accessor.Execute();

                customers = data.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return customers;
        }

        //public Class1 ReadObjectAll()
        //{

        //    DataRow loRow;
        //    try
        //    {

        //        Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));//

        //        string sqlCommand = "usp_Principal_Read_All";
        //        DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

        //        //db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);

        //        loRow = db.ExecuteDataSet(dbCommand).Tables[0].Rows[0];

        //        if (loRow.Equals(null))
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            return ReturnObject(loRow);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

    }
}
