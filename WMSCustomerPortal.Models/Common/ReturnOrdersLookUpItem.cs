using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSCustomerPortal.Models.Common
{
    public class ReturnOrdersLookUpItem
    {
        public string OrderNumber { get; set; }
        public string OrderID { get; set; }
        public string ReceiverRAMCustomerID { get; set; }
        public string BillingRAMCustomerID { get; set; }
        public int PrincipalID { get; set; }
        public string ReceivedDate { get; set; }
        public string CompanyName { get; set; }
        public string InvName { get; set; }
        public string InvAdd1 { get; set; }
        public string InvAdd2 { get; set; }
        public string InvAdd3 { get; set; }
        public string InvAdd4 { get; set; }
        public string InvAdd5 { get; set; }
        public string InvAdd6 { get; set; }
        public string DeliveryAdd1 { get; set; }
        public string DeliveryAdd2 { get; set; }
        public string DeliveryAdd3 { get; set; }
        public string DeliveryAdd4 { get; set; }
        public string DeliveryAdd5 { get; set; }
        public string DeliveryAdd6 { get; set; }
        public string WMSPostCode { get; set; }

        public ReturnOrdersLookUpItem()
        {

        }

        public ReturnOrdersLookUpItem(string _OrderNumber, string _OrderID, string _ReceiverRAMCustomerID, string _BillingRAMCustomerID, int _PrincipalID, string _ReceivedDate, string _CompanyName, string _InvName, string _InvAdd1, string _InvAdd2, string _InvAdd3, string _InvAdd4, string _InvAdd5, string _InvAdd6, string _DeliveryAdd1, string _DeliveryAdd2, string _DeliveryAdd3, string _DeliveryAdd4, string _DeliveryAdd5, string _DeliveryAdd6, string _WMSPostCode)
        {
            OrderNumber = _OrderNumber;
            OrderID = _OrderID;
            ReceiverRAMCustomerID = _ReceiverRAMCustomerID;
            BillingRAMCustomerID = _BillingRAMCustomerID;
            PrincipalID = _PrincipalID;
            ReceivedDate = _ReceivedDate;
            CompanyName = _CompanyName;
            InvName =_InvName;
            InvAdd1 = _InvAdd1;
            InvAdd2 = _InvAdd2;
            InvAdd3 = _InvAdd3;
            InvAdd4 = _InvAdd4;
            InvAdd5 = _InvAdd5;
            InvAdd6 = _InvAdd6;
            DeliveryAdd1 = _DeliveryAdd1;
            DeliveryAdd2 = _DeliveryAdd2;
            DeliveryAdd3 = _DeliveryAdd3;
            DeliveryAdd4 = _DeliveryAdd4;
            DeliveryAdd5 = _DeliveryAdd5;
            DeliveryAdd6 = _DeliveryAdd6;
            WMSPostCode = _WMSPostCode;

        }
    }
}
