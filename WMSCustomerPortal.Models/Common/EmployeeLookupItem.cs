
namespace WMSCustomerPortal.Models.Common {

    public class EmployeeLookupItem {
        public string EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public EmployeeLookupItem() {

        }

        public EmployeeLookupItem(string _EmployeeID, string _FirstName, string _LastName) {
            EmployeeID = _EmployeeID;
            FirstName = _FirstName;
            LastName = _LastName;
        }
    }
}
