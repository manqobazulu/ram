using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSCustomerPortal.Models.Common {
    public class ResponseObjects {

    }

    /// <summary>
    /// Customer Response Object
    /// </summary>
    public struct CustomerSaveResponse {
        public bool Saved;
        public string CustomerID;
        public string Details;
        public string AlternateCustomerID;
    }

    /// <summary>
    /// Base Save Response Object
    /// </summary>
    public struct BaseSaveResponse {
        public bool Saved;
        public string Details;
    }
}
