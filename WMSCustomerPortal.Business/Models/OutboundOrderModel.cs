using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSCustomerPortal.Business.Models
{
    public class OutboundOrderModel
    {
        public string RAMOrderNumber { get; set; }
        public string CustomerOrderNumber { get; set; }
    }
    public class OrderLineItemsModel
    {
        public int OrderID { get; set; }
    }

}
