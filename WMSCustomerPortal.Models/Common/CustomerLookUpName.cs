using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSCustomerPortal.Models.Common
{
 public class CustomerLookUpName
    {
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        


        public CustomerLookUpName()
        {

        }
        public CustomerLookUpName(string _CustomerID, string _CustomerName)
        {
            CustomerID = _CustomerID;
            CustomerName = _CustomerName;

        }
    }
}
