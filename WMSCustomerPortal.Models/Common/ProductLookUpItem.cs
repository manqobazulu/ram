using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSCustomerPortal.Models.Common
{
    public class ProductLookUpItem
    {
        public string ProductStagingID { get; set; }
        public string ProductCode { get; set; }

        public ProductLookUpItem()
        {

        }

        public ProductLookUpItem(string _ProductCode, string _ProductStagingID)
        {
            ProductStagingID = _ProductStagingID;
            ProductCode = _ProductCode;
        }
    }
}
