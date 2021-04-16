using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSCustomerPortal.Models.Common {
    public class CustomerAutocomplete {
        public string CustomerID { get; set; }
        public string Suburb { get; set; }
        public string CustomerName { get; set; }
        public string StoreCode { get; set; }
        public string TelephoneNo { get; set; }
        public Boolean IsActive { get; set; }

        public CustomerAutocomplete() {

        }

        public CustomerAutocomplete(string pCustomerID, string pSuburb, string pCustomerName, string pStoreCode, string pTelephoneNo, Boolean pIsActive) {
            CustomerID = pCustomerID;
            Suburb = pSuburb;
            CustomerName = pCustomerName;
            StoreCode = pStoreCode;
            TelephoneNo = pTelephoneNo;
            IsActive = pIsActive;   
        }
    }
}
