using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WMSCustomerPortal.Models.Common
{
    public class ViewModelBase
    {
        public string SelectedXXPrincipal { get; set; } //temporary 
        public IEnumerable<SelectListItem> SelectedXPrincipal { get; set; }
     
    }
}
