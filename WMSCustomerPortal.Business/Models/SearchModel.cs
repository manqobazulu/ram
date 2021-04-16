using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WMSCustomerPortal.Business.Models {
    public class SearchModel {
        public string CustomerName { get; set; }
        public bool ShowZeroInventory { get; set; }
        public int PrincipalID { get; set; }
    }
}
