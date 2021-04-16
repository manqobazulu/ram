using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSCustomerPortal.Models.Entities {

    public partial class Employee {
        public string EmployeeID { get; set; }
        public string EmployeeNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TelNo { get; set; }
        public string CellNo { get; set; }
        public string EmailAddress { get; set; }
        public string HubID { get; set; }
        public string Designation { get; set; }
        public Nullable<int> AccessLevelID { get; set; }
        public string Password { get; set; }
        public string PasswordExpires { get; set; }
        public string EmployeeIDNo { get; set; }
        public Nullable<System.DateTime> EmployedDateTime { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string Suburb { get; set; }
        public Nullable<bool> DriversLicence { get; set; }
        public string DriversCode { get; set; }
        public Nullable<bool> Disability { get; set; }
        public string DisabilityDescr { get; set; }
        public string Race { get; set; }
        public string Religion { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> BirthDayDateTime { get; set; }
        public string Spare1 { get; set; }
        public string Spare2 { get; set; }
        public string Spare3 { get; set; }
        public string Spare4 { get; set; }
        public string Spare5 { get; set; }
        public string Spare6 { get; set; }
        public string Spare7 { get; set; }
        public string Spare8 { get; set; }
        public string Spare9 { get; set; }
        public string Spare10 { get; set; }
        public string Spare11 { get; set; }
        public string Spare12 { get; set; }
        public string Spare13 { get; set; }
        public string Spare14 { get; set; }
        public string Spare15 { get; set; }
        public string Spare16 { get; set; }
        public string Spare17 { get; set; }
        public string Spare18 { get; set; }
        public string Spare19 { get; set; }
        public string Spare20 { get; set; }
        public Nullable<int> isActive { get; set; }
        public Nullable<System.DateTime> EventDateTime { get; set; }
        public string TerminalID { get; set; }
        public string LastUpdate { get; set; }
        public string isDirty { get; set; }
        public System.Guid rowguid { get; set; }
    }
}
