using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSCustomerPortal.Models.Common {

    public class CustomerGroupLookUpItem {

        public string CustomerGroupID { get; set; }
        public string Description { get; set; }

        public CustomerGroupLookUpItem() {

        }

        public CustomerGroupLookUpItem(string pCustomerGroupID, string pDescription) {
            CustomerGroupID = pCustomerGroupID;
            Description = pDescription;
        }
    }
}
