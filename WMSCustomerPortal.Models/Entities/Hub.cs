using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSCustomerPortal.Models.Entities {

    public partial class Hub {
        public string HubID { get; set; }
        public string Description { get; set; }
        public string TelephoneNo { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string ZoneID { get; set; }
        public Nullable<int> DirtyID { get; set; }
        public string EventDateTime { get; set; }
        public string EmployeeID { get; set; }
        public string TerminalID { get; set; }
        public string LastUpdate { get; set; }
        public string isDirty { get; set; }
        public System.Guid rowguid { get; set; }
        public string HubType { get; set; }
        public string GEOID { get; set; }
        public Nullable<double> Lat { get; set; }
        public Nullable<double> Lon { get; set; }
        public Nullable<bool> CanFly { get; set; }
        public string AirportName { get; set; }
        public string TelephoneNo2 { get; set; }
        public string FaxNo { get; set; }
        public string EmailAddress { get; set; }
        public string BranchManager { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> OpenedDT { get; set; }
        public Nullable<System.DateTime> ClosedDT { get; set; }
    }
}
