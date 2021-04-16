using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WMSCustomerPortal.Models;
using WMSCustomerPortal.Models.Common;
using WMSCustomerPortal.Models.Entities;

using System.Data;

namespace WMSCustomerPortal.Business {
    public class AccountsService {

        WMSCustomerPortal.Models.DataAccess.SQLDataManager _sqlManager;
        private WMSCustomerPortal.Models.DataAccess.SQLDataManager SqlManager {
            get {
                if (_sqlManager == null) {
                    _sqlManager = new WMSCustomerPortal.Models.DataAccess.SQLDataManager();
                }
                return _sqlManager;
            }
        }
        
        public EmployeeLookupItem[] GetEmployeeListByID(string startsWith) {
            try { 
            
                    var data = SqlManager.GetEmployeeListByID(startsWith, 20).ToArray();    
                    return data;
            }
            catch { throw; }
        }

        public EmployeeLookupItem[] GetEmployeeListByName(string startsWith) {
            try{
            var data = SqlManager.GetEmployeeListByName(startsWith, 20).ToArray();

            return data;
            }
            catch { throw; }
        }

        public bool EmployeeLogin(string employeeID, string password) {
            try
            {
                           
            return SqlManager.EmployeeLogin(employeeID, password);
            }
            catch { throw; }
        }


        #region Principal Group Links

       
        public static ApplicationGroupPrincipals GetPrincipalGroup(int recordID)
        {
            try { 
            ApplicationGroupPrincipals stage = new ApplicationGroupPrincipals();
            stage = ApplicationGroupPrincipals.ReadObject(recordID);
            return stage;
            }
            catch { throw; }
        }


        public static System.Data.DataSet GetAllPrincipalGroup()
        {
            try { 
            System.Data.DataSet retValue = new DataSet();
            retValue = ApplicationGroupPrincipals.ReadAllDS();
            return retValue;
            }
            catch { throw; }
        }



        #endregion





    }
}
