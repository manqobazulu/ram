using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSCustomerPortal.Business.Models
{
    public class ReturnReceiptResponse
    {
        public string RAMOrderNumber { get; set; }

        public string ReturnReferenceNumber { get; set; }

        public string CustomerName { get; set; }

        public string ReturnWaybillNumber { get; set; }

        public List<dynamic> LineItems { get; set; }
    }
}
