using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSCustomerPortal.Models.Common {

    public class CustomerLookUpItem {

        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerDetail { get; set; }


        public CustomerLookUpItem() {

        }

        public CustomerLookUpItem(string _CustomerID, string _CustomerName, string _CustomerDetail) {
            CustomerID = _CustomerID;
            CustomerName = _CustomerName;
            CustomerDetail = _CustomerDetail;
        }
        public CustomerLookUpItem(string _CustomerID, string _CustomerName)
        {
            CustomerID = _CustomerID;
            CustomerName = _CustomerName;
           
        }

    }
}
