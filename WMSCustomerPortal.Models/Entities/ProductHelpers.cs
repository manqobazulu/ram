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
    class ProductHelpers
    {
        #region Data Access

        public int GetProductCount(string productCode,  string principalCode, int principalID)
        {
            int retVal = 0;
            
            DataRow loRow;
            try
            {
                // Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
               
                
                string sqlCommand = "usp_ProductGetStockCount";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);

                db.AddInParameter(dbCommand, "@ProductCode", DbType.String, productCode);
                db.AddInParameter(dbCommand, "@PrincipalCode", DbType.String, principalCode);
                db.AddInParameter(dbCommand, "@PrincipalID", DbType.Int32, principalID);

                db.AddOutParameter(dbCommand, "@ProductCount", DbType.Int32, 4);

                loRow = db.ExecuteDataSet(dbCommand).Tables[0].Rows[0];

                return (int)db.GetParameterValue(dbCommand, "@ProductStagingID");


            }
            catch (Exception ex)
            {
               throw ex;
            }
        }


        #endregion



    }
}
