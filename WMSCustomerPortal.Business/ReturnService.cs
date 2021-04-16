using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSCustomerPortal.Models.Entities;
using System.Data;

using WMSCustomerPortal.Business.Models;

namespace WMSCustomerPortal.Business
{
    public class ReturnService
    {

        #region Returns

        public int SaveNewReturnOrder(Returns stagingRecord, string EventTerminal, string EventUser, string OrderLineItems)
        {
            int retVal = 0;
            Returns staging = new Returns();
            retVal = Returns.SaveNewReturnOrder(stagingRecord, EventTerminal, EventUser, OrderLineItems);    
            return retVal;
        }
        public int UpdateReturnReceiptLine(int lineItemID, int quantity, string returnWaybill, string EventUser, string EventTerminal)
        {
            int retVal = 0;            
            retVal = Returns.UpdateReturnReceiptLine(lineItemID, quantity, returnWaybill, EventUser, EventTerminal);
            return retVal;
        }

        #endregion

        #region ReturnReceipt

        public List<dynamic> GetReturnReceiptList(int PrincipalID)
        {
            try
            {
                var data = new Returns().ReadAllReturnReceipt(PrincipalID);

                return data.Tables[0].ToDynamicList("GetReturnReceiptList");
            }
            catch { throw; }
        }

        public List<dynamic> GetPrincipalsByGroup(int PrincipalID)
        {
            try
            {
                var data = new Returns().GetPrincipalsForPrincipalGroup(PrincipalID);

                return data.Tables[0].ToDynamicList("GetPrincipalsByGroup");
            }
            catch { throw; }
        }

        public ReturnReceiptResponse GetReturnReceiptLineItems(string orderNumber)
        {
            ReturnReceiptResponse response = new ReturnReceiptResponse();
            try
            {
                var data = new Returns().ReadReturnReceiptLineItems(orderNumber);

                if (data != null && data.Tables.Count > 0)
                {
                    response.RAMOrderNumber = data.Tables[0].Rows[0]["RAMOrderNumber"].ToString();
                    response.ReturnReferenceNumber = data.Tables[0].Rows[0]["ReturnReferenceNumber"].ToString();
                    response.CustomerName = data.Tables[0].Rows[0]["CustomerName"].ToString();
                    response.ReturnWaybillNumber = data.Tables[0].Rows[0]["ReturnWaybillNumber"].ToString();

                    if (data.Tables.Count > 1)
                    {
                        response.LineItems = data.Tables[1].ToDynamicList("LineItems");
                    }
                }

                return response;
            }

            catch { throw; }
        }

        public List<dynamic> GetReturnReceiptLineItems_All(int PrincipalID)
        {
            try
            {
                var data = new Returns().ReadReturnReceiptLineItems_All(PrincipalID);

                return data.Tables[0].ToDynamicList("GetReturnReceiptList");
            }
            catch { throw; }
        }

        public List<dynamic> GetReturnReceiptLineItems_by_IGD(int IGDStaging)
        {
            try
            {
                var data = new Returns().ReadReturnReceiptLineItems_by_IGD(IGDStaging);

                return data.Tables[0].ToDynamicList("GetReturnReceiptList");
            }
            catch { throw; }
        }

        #endregion

    }
}