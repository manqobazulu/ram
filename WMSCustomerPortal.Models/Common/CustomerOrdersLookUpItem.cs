using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSCustomerPortal.Models.Common
{
    public class CustomerOrdersLookUpItem
    {

        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string BillingCustomerID { get; set; }
        public int CustomerDetailID { get; set; }
        public string StoreCode { get; set; }
        public string DefaultContactPerson { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string TelephoneNo { get; set; }
        public string ZoneID { get; set; }
        public Boolean isActive { get; set; }
        public string EmailAddress { get; set; }
        public string Suburb { get; set; }
        public string HubID { get; set; }
        public string International { get; set; }
        public string CellNo { get; set; }
        public string FaxNo { get; set; }
        public string PostalCode { get; set; }



        public CustomerOrdersLookUpItem()
        {

        }

        public CustomerOrdersLookUpItem(string _CustomerName, string _CustomerID, string _BillingCustomerID, int _CustomerDetailID, string _StoreCode, string _DefaultContactPerson, string _StreetAddress1, string _StreetAddress2, string _TelephoneNo, string _ZoneID, Boolean _isActive, string _EmailAddress, string _Suburb, string _HubID, string _International, string _CellNo, string _FaxNo, string _PostalCode )
        {
            CustomerID = _CustomerID;
            CustomerName = _CustomerName;
            BillingCustomerID = _BillingCustomerID;
            CustomerDetailID = _CustomerDetailID;
            StoreCode = _StoreCode;
            DefaultContactPerson = _DefaultContactPerson;
            StreetAddress1 = _StreetAddress1;
            StreetAddress2 = _StreetAddress2;
            TelephoneNo = _TelephoneNo;
            ZoneID = _ZoneID;
            isActive = _isActive;
            EmailAddress = _EmailAddress;
            Suburb = _Suburb;
            HubID = _HubID;
            International = _International;
            CellNo = _CellNo;
            FaxNo = _FaxNo;
            PostalCode = _PostalCode;
        }
    }
}
