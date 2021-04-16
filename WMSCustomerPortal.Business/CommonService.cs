using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data;
using WMSCustomerPortal.Models;
using WMSCustomerPortal.Models.Common;

namespace WMSCustomerPortal.Business {
    public class CommonService {

        WMSCustomerPortal.Models.DataAccess.SQLDataManager _sqlManager;
        private WMSCustomerPortal.Models.DataAccess.SQLDataManager SqlManager {
            get {
                if (_sqlManager == null) {
                    _sqlManager = new WMSCustomerPortal.Models.DataAccess.SQLDataManager();
                }
                return _sqlManager;
            }
        }

        public List<SelectListItem> GetCustomersForSelectList() {

            try {
                var results = SqlManager.GetOrderCustomers();

                // Perform a client-side query on the returned data 
                var customers = (from item in results
                                 select new SelectListItem {
                                     Text = string.Format(item.CustomerName),
                                     Value = item.CustomerDetail
                                 }).ToList();

                customers.Insert(0, (new SelectListItem { Text = "SELECT CUSTOMER", Value = "" }));

                return customers;
            }
            catch { throw; }
        }

        public CustomerLookUpItem[] Customer_Lookup_NoPrincipals(string startsWith) {
            try {
                var data = SqlManager.Customer_Lookup_NoPrincipals(startsWith, 20).ToArray();

                return data;
            }
            catch { throw; }
        }

        public CustomerLookUpItem[] Customer_Lookup_by_PrincipalID(string startsWith, int principalId) {
            try {
                var data = SqlManager.Customer_Lookup_by_PrincipalID(startsWith, 20, principalId).ToArray();

                return data;
            }
            catch { throw; }
        }

        public CustomerGroupLookUpItem[] CustomerGroup_Lookup(string startsWith) {
            try {
                var data = SqlManager.CustomerGroup_Lookup(startsWith, 20).ToArray();

                return data;
            }
            catch { throw; }
        }

        public List<dynamic> Suburb_Lookup(string startsWith) {
            try {
                var data = new SharedServices_LocationsWS.Locations().Suburb_Lookup_Dataset(startsWith, 20);

                return data.Tables[0].ToDynamicList("Suburbs");
            }
            catch { throw; }
        }
        public List<dynamic> Suburb_Lookup1(string startsWith)
        {
            try
            {
                var data = new SharedServices_LocationsWS.Locations().Suburb_Lookup_Dataset(startsWith, 20);

                
                return  data.Tables[0].ToDynamicList("Suburbs");
            }
            catch { throw; }
        }

        public WMSCustomerPortal.Models.Common.ProductAutocomplete[] Product_Lookup(int principalId, string startsWith, string searchType) {
            try {
                var data = SqlManager.Product_Lookup(principalId, startsWith, 20, searchType).ToArray();

                return data;
            }
            catch { throw; }
        }

        public WMSCustomerPortal.Models.Common.CustomerAutocomplete[] Customer_Lookup(int principalId, string startsWith, string searchType) {
            try {
                var data = SqlManager.Customer_Lookup(principalId, startsWith, 20, searchType).ToArray();

                return data;
            }
            catch { throw; }
        }
    }
}
