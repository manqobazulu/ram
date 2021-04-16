using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WMSCustomerPortal.Models.Common {

    public class CustomerMainModel : ViewModelBase {
        
        [HiddenInput(DisplayValue = false)]
        public int? PrincipalID { get; set; }
    }
}
