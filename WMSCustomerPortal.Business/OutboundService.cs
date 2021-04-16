using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSCustomerPortal.Models.Entities;
using RAM.Utilities.Common;
using System.Data;
using WMSCustomerPortal.Business.WS_WMS_ATO;


namespace WMSCustomerPortal.Business
{
    public class OutboundService
    {
        #region Orders

        /// <summary>
        /// Retrieves a specific Order object from an ID
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static Orders GetOrder(int orderID)
        {

            Orders staging = new Orders();

            staging = Orders.ReadObject(orderID);

            return staging;

        }


        public List<dynamic> GetOutboundOrdersFilter(int principalID, bool listCompleted, string clientPORef, string ramPORef) 
        {

            var outboundOrders = Orders.SearchDS(principalID, listCompleted, clientPORef, ramPORef);

            return outboundOrders.Tables[0].ToDynamicList("");
        }

        /// <summary>
        /// Retrieves a Dataset of Orders for a Principal
        /// </summary>
        /// <param name="principalID"></param>
        /// <returns></returns>
        public List<dynamic> GetAllOrdersForPrincipal(int principalID)
        {
            System.Data.DataSet retVal = new DataSet();
            retVal = OrderLineItems.ReadAllDS(principalID);

            return retVal.Tables[0].ToDynamicList("OrderLineItems");

        }

      
        /// <summary>
        /// Search for Orders based  on the various PO reference numbers
        /// </summary>
        /// <param name="principalID"></param>
        /// <param name="listCompleted"></param>
        /// <param name="clientPORef"></param>
        /// <param name="ramPORef"></param>
        /// <returns></returns>
        public List<dynamic> SearchOrdersDS(int principalID, string EventUser)
        {
            System.Data.DataSet retVal = new DataSet();
            retVal = Orders.ReadAllDS(principalID, EventUser);

            return retVal.Tables[0].ToDynamicList("OutboundOrders");


        }

        public List<dynamic> SearchCompleteOrdersDS(int principalID, string EventUser) {
            System.Data.DataSet retVal = new DataSet();
            retVal = Orders.ReadAllCompleteDS(principalID, EventUser);

            return retVal.Tables[0].ToDynamicList("OutboundOrders");
        }

        public static List<dynamic> SearchCustomer(string customerName)
        {

            System.Data.DataSet retVal = new DataSet();
            retVal = OrderLineItems.SearchCustomerDS(customerName);

            return retVal.Tables[0].ToDynamicList("Customer");

        }



        /// <summary>
        /// Retrieves a list of orders based  on the various PO reference numbers
        /// </summary>
        /// <param name="principalID"></param>
        /// <param name="listCompleted"></param>
        /// <param name="clientPORef"></param>
        /// <param name="ramPORef"></param>
        /// <returns></returns>
        public static List<Orders> SearchOrdersList(int principalID, bool listCompleted, string clientPORef, string ramPORef)
        {

            List<Orders> retValue = new List<Orders>();
            retValue = Orders.SearchList(principalID, listCompleted, clientPORef, ramPORef);

            return retValue;

        }

        /// <summary>
        /// Updates a specific Order object
        /// </summary>
        /// <param name="stagingRecord"></param>
        /// <param name="eventTerminal"></param>
        /// <param name="eventUser"></param>
        /// <returns></returns>
        public static int UpdateOrder(Orders stagingRecord, string eventTerminal, string eventUser)
        {
            int retVal = 0;
            Orders staging = new Orders();

            retVal = Orders.Update(
                                    stagingRecord.OrderID,
                                    stagingRecord.CustomerDetailID,
                                    stagingRecord.BillingCustomerDetailID,
                                    stagingRecord.ReceiverRAMCustomerID,
                                    stagingRecord.BillingRAMCustomerID,
                                    stagingRecord.PrincipalID,
                                    stagingRecord.RAMOrderNumber,
                                    stagingRecord.DateCreated,
                                    stagingRecord.CustomerOrderNumber,
                                    stagingRecord.Priority,
                                    stagingRecord.ValueAddedPackaging,
                                    stagingRecord.SalesPerson,
                                    stagingRecord.SalesCategory,
                                    stagingRecord.Processor,
                                    stagingRecord.OrderDiscount,
                                    stagingRecord.OrderVAT,
                                    eventUser,
                                    eventTerminal);
            return retVal;
        }

        /// <summary>
        /// Deletes a specific Order object
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="eventTerminal"></param>
        /// <param name="eventUser"></param>
        /// <param name="errorResult"></param>
        /// <returns></returns>
        public bool DeleteOrder(int orderID, string eventTerminal, string eventUser, out string errorResult)
        {

            errorResult = "";
            bool retVal = false;

            //check to see if there are any order lines attached to this record
            System.Data.DataSet dsOrderLines = new DataSet();
            dsOrderLines = GetAllOrderLineItems(orderID);
            if (dsOrderLines.Tables[0].Rows.Count > 0)
            {
                //we cannot delete
                errorResult = "There are Order Line Items bound to this order. Cannot delete.";
            }
            else
            {
                retVal = Orders.Delete(orderID, eventTerminal, eventUser);
                if (retVal)
                {
                    errorResult = "Order deleted.";
                }
            }


            return retVal;
        }

        /// <summary>
        /// Just completes an order - Does NOT submit to WMS
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="eventTerminal"></param>
        /// <param name="eventUser"></param>
        /// <returns></returns>
        public bool SubmitOrder(int orderID, string eventTerminal, string eventUser) {

            bool retVal = false;

            //check to see if there are any order lines attached to this record
            System.Data.DataSet dsOrderLines = new DataSet();
            dsOrderLines = GetAllOrderLineItems(orderID);
            if (dsOrderLines.Tables[0].Rows.Count > 0) {
               
                //mark this order as submitted to WMS
                retVal = Orders.Complete(orderID, eventTerminal, eventUser);
            }

            return retVal;
        }

        /// <summary>
        /// Sets the Submitted Flahg on the order
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="eventTerminal"></param>
        /// <param name="eventUser"></param>
        /// <returns></returns>
        public bool SetSubmittedOrder(int orderID, string eventTerminal, string eventUser)
        {

            bool retVal = false;

            //check to see if there are any order lines attached to this record
            System.Data.DataSet dsOrderLines = new DataSet();
            dsOrderLines = GetAllOrderLineItems(orderID);
            if (dsOrderLines.Tables[0].Rows.Count > 0)
            {

                //mark this order as submitted to WMS
                retVal = Orders.Complete(orderID, eventTerminal, eventUser);
            }

            return retVal;
        }

       
        /// <summary>
        /// Adds a new Order object
        /// </summary>
        /// <param name="stagingRecord"></param>
        /// <param name="eventTerminal"></param>
        /// <param name="eventUser"></param>
        /// <returns></returns>
        public int AddOrder(Orders stagingRecord, string eventTerminal, string eventUser, string customerID, bool submitted)
        {
            int retVal = 0;
            Orders staging = new Orders();

            //lets create a new RAM order number 
            //string newRAMOrder = "";
            //newRAMOrder = WMSCustomerPortal.Models.DataAccess.SQLDataManager.GetNextBaseRAMOrder(stagingRecord.PrincipalID);


            retVal = Orders.Save(
                                    stagingRecord.OrderID,
                                    stagingRecord.CustomerDetailID,
                                    stagingRecord.BillingCustomerDetailID,
                                    stagingRecord.ReceiverRAMCustomerID,
                                    stagingRecord.BillingRAMCustomerID,
                                    stagingRecord.PrincipalID,
                                    stagingRecord.RAMOrderNumber,
                                    stagingRecord.DateCreated,
                                    stagingRecord.CustomerOrderNumber,
                                    stagingRecord.Priority,
                                    stagingRecord.ValueAddedPackaging,
                                    stagingRecord.SalesPerson,
                                    stagingRecord.SalesCategory,
                                    stagingRecord.Processor,
                                    stagingRecord.OrderDiscount,
                                    stagingRecord.OrderVAT,
                                    eventUser,
                                    eventTerminal, customerID, submitted);



            return retVal;

        }

        public int UpdateOrderLineItem(int OrderID, int CustomerDetailID, string CustomerID,  int PrincipalID, string eventUser, string eventTerminal, string Table)
        {
                                 
            int retVal = 0;
            Orders staging = new Orders();

            retVal = Orders.UpdateOrderAndOrderline(OrderID, CustomerDetailID,  CustomerID, PrincipalID, eventUser, eventTerminal, Table);

            return retVal;

        }

        public string CheckStockQuantity(int PrincipalID, string ProductCode, int Quantity, string ResultOut) {

            string retVal = "";
            ProductStaging staging = new ProductStaging();

            retVal = staging.CheckStockQuantity(PrincipalID, ProductCode, Quantity, ResultOut);

            return retVal;

        }

        public string CheckStockQuantityNew(int PrincipalID, string PrincipalCode, string ProductCode, int QuantityRequired, out int AvailableQty)
        {
            string retVal = "";

            try
            {
                WS_WMS_ATO.TypeATORequest Req = new TypeATORequest();

                Req.PrincipalCode = PrincipalCode;
                Req.ProdCode = ProductCode;
                Req.SiteCode = RAM.Utilities.Common.ConfigSettings.ReadConfigValue("WMSSiteCode", "001");

                WS_WMS_ATO.ATOWsPortClient Cl = new ATOWsPortClient();

                Cl.Open();
                WS_WMS_ATO.TypeATOResponse Res = Cl.ATORequest(Req);
                Cl.Close();

                if (Res.Success)
                {
                    int Qty = 0;
                    retVal = "OK";
                    AvailableQty = Res.AvailableQty;

                    if(QuantityRequired > AvailableQty)
                    {
                        retVal = "Insufficient Stock. Only " + AvailableQty + "  Available For Order For Product Code " + ProductCode;
                    }
                    else
                    {
                        try
                        {
                            ProductStaging staging = new ProductStaging();
                            Qty = staging.CheckUnsubmittedQuantity(PrincipalID, PrincipalCode, ProductCode);
                        }
                        catch(Exception){ }
                    }

                    AvailableQty -= Qty;
                    if(QuantityRequired > AvailableQty)
                    {
                        retVal = "Insufficient Stock. Only " + AvailableQty + "  Available For Order For Product Code " + ProductCode;
                    }
                }
                else
                {
                    retVal = Res.ReasonText;
                    AvailableQty = Res.AvailableQty;
                }
            }
            catch(Exception Ex)
            {
                retVal = Ex.Message;
                AvailableQty = 0;
            }


            return retVal;

        }

        public bool DoesCustomerOrderNumberExist(string CustomerOrderNumber, int PrincipalID)
        {
            try {
            bool retVal = true;

            List<Orders> retValue = new List<Orders>();
            retValue = Orders.ReadAll_ListByCustomerOrderNumber(CustomerOrderNumber, PrincipalID);

            if (retValue.Count > 0)
            {
                retVal = false;
            }

            return retVal;
            }
            catch { throw; }
        }

        public int AddOrderSingleReceiver(Orders stagingRecord, string eventTerminal, string eventUser, bool submitted, string Table) {
            int retVal = 0;
            Orders staging = new Orders();

            retVal = Orders.SaveOrderSingleReceiver(
                                    stagingRecord.OrderID,
                                    stagingRecord.CustomerDetailID,
                                    stagingRecord.BillingCustomerDetailID,
                                    stagingRecord.ReceiverRAMCustomerID,
                                    stagingRecord.BillingRAMCustomerID,
                                    stagingRecord.PrincipalID,
                                    stagingRecord.CustomerOrderNumber,
                                    stagingRecord.DateCreated,
                                    eventUser,
                                    eventTerminal, submitted, Table);

            if (submitted == true) {
                bool SendToWMS = ProcessOrderToWMS(retVal, eventTerminal, eventUser);
            }

            return retVal;

        }

        public int AddOrderSingleReceiver(Orders stagingRecord,string IDNumber, string KYC,string CourierName,string CourierService,bool InsuranceRequired,bool ValidateDelivery, string eventTerminal, string eventUser, bool submitted, string HeaderComment, string Table)
        {
            int retVal = 0;
            Orders staging = new Orders();

            retVal = Orders.SaveOrderSingleReceiver(
                                    stagingRecord.OrderID,
                                    stagingRecord.CustomerDetailID,
                                    stagingRecord.BillingCustomerDetailID,
                                    stagingRecord.ReceiverRAMCustomerID,
                                    stagingRecord.BillingRAMCustomerID,
                                    stagingRecord.PrincipalID,
                                    stagingRecord.CustomerOrderNumber,
                                    stagingRecord.DateCreated,
                                    eventUser,
                                    IDNumber,
                                    KYC,
                                    CourierName,
                                    CourierService,
                                    InsuranceRequired,
                                    ValidateDelivery,
                                    eventTerminal, submitted, HeaderComment, Table);

            if (submitted == true)
            {
                bool SendToWMS = ProcessOrderToWMS(retVal, eventTerminal, eventUser, IDNumber, KYC, CourierName, CourierService, InsuranceRequired, ValidateDelivery, HeaderComment);
            }

            return retVal;

        }

        public int AddOrderMultipleReceiver(Orders stagingRecord, string eventTerminal, string eventUser, bool submitted, string pProduct, string pReceiver) {
            int retVal = 0;
            Orders staging = new Orders();

            DataSet mOrders = new DataSet();
            mOrders = Orders.SaveOrderMultipleReceiver(
                                    stagingRecord.CustomerOrderNumber,
                                    stagingRecord.OrderID,
                                    stagingRecord.PrincipalID,
                                    stagingRecord.DateCreated,
                                    eventUser,
                                    eventTerminal, submitted, pProduct, pReceiver);
            if (mOrders.Tables[0].Rows.Count > 0) {
                retVal++;
            }

            if (submitted == true) {

             foreach (DataRow dr in mOrders.Tables[0].Rows) {
                      string strOrderID = dr["ORDERID"].ToString();
                      bool SendToWMS = ProcessOrderToWMS(int.Parse(strOrderID), eventTerminal, eventUser);
                      retVal++;
                     }
              }

            return retVal;

        }

        /// <summary>
        /// Retrieves all the Order records for a specifc linked  customer ID
        /// </summary>
        /// <param name="linkedCustomerID"></param>
        /// <returns></returns>
        public static System.Data.DataSet GetAllOrdersForCustomer(int linkedCustomerID)
        {
            //retrieves all the order for a specific linkedcustomerid
            System.Data.DataSet retVal = new DataSet();


            return retVal;
        }

        public List<dynamic> GetCustomerDetail(string CustomerID, int principalID) {

            //retrieves all the order for a specific linkedcustomerid
            System.Data.DataSet retVal = new DataSet();

            retVal = OrderLineItems.GetCustomerDetail(CustomerID, principalID);

            return retVal.Tables[0].ToDynamicList("customerDetail");
        }

        public List<dynamic> GetOrderDetail(int OrderID) {

            //retrieves all the order for a specific linkedcustomerid
            System.Data.DataSet retVal = new DataSet();

            retVal = OrderLineItems.GetOrderDetail(OrderID);

            return retVal.Tables[0].ToDynamicList("OrderDetail");
        }

        public List<dynamic> GetOrderStatus(int OrderID)
        {
            System.Data.DataSet retVal = new DataSet();

            retVal = OrderLineItems.GetOrderStatus(OrderID);

            return retVal.Tables[0].ToDynamicList("OrderDetail");
        }
        #endregion


        #region Orders Line Items

        /// <summary>
        /// Retrieves a Order Line Item Object by ID
        /// </summary>
        /// <param name="lineItemID"></param>
        /// <returns></returns>
        public static OrderLineItems GetOrderLineItem(int lineItemID)
        {

            OrderLineItems staging = new OrderLineItems();

            staging = OrderLineItems.ReadObject(lineItemID);

            return staging;

        }

        /// <summary>
        /// Retrieves all the Order Line Items for a order id
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static System.Data.DataSet GetAllOrderLineItems(int orderID)
        {

            System.Data.DataSet retValue = new DataSet();
            retValue = OrderLineItems.ReadAllDS(orderID);

            return retValue;

        }

        /// <summary>
        /// Retrieves a list of all the Order Line Items for a order id
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public List<OrderLineItems> GetAllOrderLineItemsList(int orderID)
        {

            List<OrderLineItems> lineItems = new List<OrderLineItems>();
            lineItems = OrderLineItems.ReadAll_List(orderID);
            return lineItems;

        }

        public static List<dynamic> GetAllLinkedPrincipalID(int LinkedPrincipalID, string filter)
        {
            System.Data.DataSet retVal = new DataSet();

            retVal = OrderLineItems.ReadAllLinkedPrincipalIDDS(LinkedPrincipalID, filter);

            return retVal.Tables[0].ToDynamicList("OutboundOrders");

        }

        /// <summary>
        /// Deletes an ordr line item
        /// </summary>
        /// <param name="lineItemID"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public bool DeleteOrderLineItem(int lineItemID, string eventUser, string eventTerminal)
        {
            bool retVal = false;

            retVal = OrderLineItems.Delete(lineItemID, eventTerminal, eventUser);
            return retVal;
        }

        /// <summary>
        /// Adds a order line item
        /// </summary>
        /// <param name="stagingRecord"></param>
        /// <param name="eventUser"></param>
        /// <param name="eventTerminal"></param>
        /// <returns></returns>
        public int AddOrderLineItem(OrderLineItems stagingRecord, string eventUser, string eventTerminal)
        {
            int retVal = 0;


            retVal = OrderLineItems.Save(stagingRecord.LineItemID,
                                            stagingRecord.OrderID,
                                            stagingRecord.WMSHostOrderID,
                                            stagingRecord.ProductStagingID,
                                            stagingRecord.LineNumber,
                                            stagingRecord.LineType,
                                            stagingRecord.SubmittedProductCode,
                                            stagingRecord.LineText,
                                            stagingRecord.Quantity,
                                            stagingRecord.SubmittedUnitCost,
                                            stagingRecord.SubmittedRAMOrderNumber,
                                            stagingRecord.SubmittedVAT,
                                            stagingRecord.SubmittedUnitDiscountAmount,
                                            stagingRecord.SubmittedToWMS,
                                            eventUser,
                                            eventTerminal);


            return retVal;

        }

        #endregion

        #region Stock Validation

        /// <summary>
        /// Checks to see if there is enough stock for a specific product (this is done against the MATFLO system)
        /// </summary>
        /// <param name="productStagingID"></param>
        /// <param name="requiredQuantity"></param>
        /// <param name="stockAvailable"></param>
        /// <param name="principalID"></param>
        /// <returns></returns>
        public static bool ValidateStock(int productStagingID, int requiredQuantity, out int stockAvailable, int principalID)
        {
            bool retVal = false;
            stockAvailable = 0;

            //get the available stock
            string principalCode = MasterDataService.GetPrincipalCodeByID(principalID);


            ProductStaging stage = new ProductStaging();
            stage = MasterDataService.GetProductStagingFromID(productStagingID);
            // we now have the productcode
            string productCode = stage.ProdCode;


            X_STOCK stock = new X_STOCK();


            try
            {
                stock = X_STOCK.ReadObject(principalCode.ToUpper(), productCode);
                stockAvailable = Functions.SafeInt(stock.Qty, 0);
            }
            catch (Exception ex)
            {
                stockAvailable = 0;
            }




            if (requiredQuantity > stockAvailable)
            {
                //not enough stock
                retVal = false;

            }
            else
            {
                retVal = true; // enough stock

            }


            return retVal;
        }


        #endregion


        #region Submit to WMS

        

        /// <summary>
        /// Processes order to automatically complete the missing parts and submit the order to WMS
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="eventTerminal"></param>
        /// <param name="eventUser"></param>
        /// <returns>True if successfully submitted to WMS</returns>
        public bool ProcessOrderToWMS(int orderID, string eventTerminal, string eventUser)
        {
        

            //retrieve and complete the required fields for submission to the WMS Host
            bool retVal = false;
            bool submittedSuccess = false;
            //the following setail is needed
            Orders stagingOrder = new Orders();
            List<OrderLineItems> stagingOrderLineItems = new List<OrderLineItems>();
            LinkedPrincipalCustomers customerReceiverDetail = new LinkedPrincipalCustomers();
            LinkedPrincipalCustomers customerBillingDetail = new LinkedPrincipalCustomers();
            Zones billingZone = new Zones();
            Zones receiverZone = new Zones();
            try
            {

                   //3. The order details
            stagingOrder = GetOrder(orderID);

            int principalID = stagingOrder.PrincipalID;
            string principalCode = MasterDataService.GetPrincipalCodeByID(principalID);

             string recordSource =  RAM.Utilities.Common.ConfigSettings.ReadConfigValue("WMSRecordSource", "WMSCustomerPortal");
             string siteCode = RAM.Utilities.Common.ConfigSettings.ReadConfigValue("WMSSiteCode", "001");
             string IDNumber = stagingOrder.IDNumber;
             string KYC = stagingOrder.KYC;
             string CourierName = stagingOrder.CourierName;
             string CourierService = stagingOrder.CourierService;
             bool InsuranceRequired = stagingOrder.InsuranceRequired;
             bool ValidateDelivery = stagingOrder.ValidateDelivery;




                //1. The billing address
                customerBillingDetail = MasterDataService.GetLinkedCustomerByID(stagingOrder.BillingCustomerDetailID);
              //customerBillingDetail = MasterDataService.Ge+tLinkedBillingCustomerByID(stagingOrder.CustomerDetailID);xxx
            //2. The receiver address 
            customerReceiverDetail = MasterDataService.GetLinkedCustomerByID(stagingOrder.CustomerDetailID);
            //4. The Order Line Items
            stagingOrderLineItems = GetAllOrderLineItemsList(orderID);
            //5.submit this data to Matflo 

            billingZone = MasterDataService.LookupZone(customerBillingDetail.ZoneID);
            receiverZone = MasterDataService.LookupZone(customerReceiverDetail.ZoneID); 

            WriteOrderLineItemsToHost(IDNumber, KYC, CourierName, CourierService, InsuranceRequired, ValidateDelivery,customerReceiverDetail, customerBillingDetail, receiverZone, billingZone, stagingOrder, stagingOrderLineItems, recordSource, siteCode, principalCode);

            //6.mark the record as completed.
            submittedSuccess = SetSubmittedOrder( orderID, eventTerminal,  eventUser);
            if(submittedSuccess == true)
                {
                retVal = true;
                }

            }
            catch(Exception ex)
            {
                throw ex;

            }

            return retVal;
        }
        public bool ProcessOrderToWMS(int orderID, string eventTerminal, string eventUser, string IDNumber,string KYC, string CourierName, string CourierService, bool InsuranceRequired,bool ValidateDelivery, string HeaderComment = "")
        {


            //retrieve and complete the required fields for submission to the WMS Host
            bool retVal = false;
            bool submittedSuccess = false;
            //the following setail is needed
            Orders stagingOrder = new Orders();
            List<OrderLineItems> stagingOrderLineItems = new List<OrderLineItems>();
            LinkedPrincipalCustomers customerReceiverDetail = new LinkedPrincipalCustomers();
            LinkedPrincipalCustomers customerBillingDetail = new LinkedPrincipalCustomers();
            Zones billingZone = new Zones();
            Zones receiverZone = new Zones();
            try
            {

                //3. The order details
                stagingOrder = GetOrder(orderID);

                int principalID = stagingOrder.PrincipalID;
                string principalCode = MasterDataService.GetPrincipalCodeByID(principalID);

                string recordSource = RAM.Utilities.Common.ConfigSettings.ReadConfigValue("WMSRecordSource", "WMSCustomerPortal");
                string siteCode = RAM.Utilities.Common.ConfigSettings.ReadConfigValue("WMSSiteCode", "001");
               
                //1. The billing address
                customerBillingDetail = MasterDataService.GetLinkedCustomerByID(stagingOrder.BillingCustomerDetailID);
                //customerBillingDetail = MasterDataService.Ge+tLinkedBillingCustomerByID(stagingOrder.CustomerDetailID);xxx
                //2. The receiver address 
                customerReceiverDetail = MasterDataService.GetLinkedCustomerByID(stagingOrder.CustomerDetailID);
                //4. The Order Line Items
                stagingOrderLineItems = GetAllOrderLineItemsList(orderID);
                //5.submit this data to Matflo 

                billingZone = MasterDataService.LookupZone(customerBillingDetail.ZoneID);
                receiverZone = MasterDataService.LookupZone(customerReceiverDetail.ZoneID);

                WriteOrderLineItemsToHost(IDNumber, KYC, CourierName, CourierService, InsuranceRequired, ValidateDelivery, customerReceiverDetail, customerBillingDetail, receiverZone, billingZone, stagingOrder, stagingOrderLineItems, recordSource, siteCode, principalCode, HeaderComment);

                //6.mark the record as completed.
                submittedSuccess = SetSubmittedOrder(orderID, eventTerminal, eventUser);
                if (submittedSuccess == true)
                {
                    retVal = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;

            }

            return retVal;
        }

        /// <summary>
        /// Marks the order lkines as WMS Submitted- Deprecated ... we are marking the orderr as submitted
        /// </summary>
        /// <param name="submittedItem"></param>
        /// <param name="submitted"></param>
        /// <param name="eventTerminal"></param>
        /// <param name="eventUser"></param>
        /// <returns></returns>
        public static bool MarkAsWMSSubmitted(OrderLineItems submittedItem, bool submitted, string eventTerminal, string eventUser)
        {
            bool retVal = false;
            submittedItem.SubmittedToWMS = true;

            try
            {

                OrderLineItems.SetWMSSubmitted(submittedItem.LineItemID,
                                               submitted,
                                               eventTerminal,
                                               eventUser);

                retVal = true;
            }
            catch (Exception ex)
            {
                retVal = false;

            }
            return retVal;
        }

     
        #endregion

        #region Host Web Service Calls

        /// <summary>
        /// Call the host WebService to write an ORD message
        /// </summary>
        /// WriteOrderLineItemToHost(orderStaging, orderLineItemStaging);
        public static int WriteOrderLineItemsToHost(
                                                    string IDNumber,
                                                    string KYC,
                                                    string CourierName,
                                                    string CourierService,
                                                    bool InsuranceRequired,
                                                    bool ValidateDelivery,
                                                    LinkedPrincipalCustomers customerReceiverStaging,
                                                    LinkedPrincipalCustomers customerBillingStaging,
                                                    Zones receiverZone,
                                                    Zones billingZone,
                                                    Orders orderStaging,
                                                    List<OrderLineItems> lineItems,
                                                    string recordSource,
                                                    string siteCode,
                                                    string principalCode, 
                                                    string HeaderComment = "")
        {

            WS_RAM_Host.Result resultValue = new WS_RAM_Host.Result();
            int retVal = 0;
            try
            {

                WS_RAM_Host.HostWS hostService = new WS_RAM_Host.HostWS();
                
                
              //  hostService.Url = WMSCustomerPortal.Library.Configuration.GetRAMHostWSPath();
                hostService.Url = ConfigSettings.ReadConfigValue("RAMHostWSPath", ""); 

                hostService.Credentials = System.Net.CredentialCache.DefaultCredentials;

                WS_RAM_Host.ORDLine[] orderLine = new WS_RAM_Host.ORDLine[lineItems.Count];
                int iPos = 0;


                foreach (OrderLineItems itm in lineItems)
                {
                    //also look up the product details
                    ProductStaging product = new ProductStaging();
                    product = MasterDataService.GetProductStagingFromID(itm.ProductStagingID);

                    orderLine[iPos] = new WS_RAM_Host.ORDLine();
                    orderLine[iPos].LineNumber = Functions.SafeInt(iPos + 1, 0);
                    orderLine[iPos].LineText = itm.LineText;
                    orderLine[iPos].LineType = "P";//itm.LineType;
                    orderLine[iPos].ProductCode = itm.ProductCode;
                    orderLine[iPos].Quantity = Functions.SafeInt(itm.Quantity, 0);
                    orderLine[iPos].UnitCost = product.SalesPrice;
                    orderLine[iPos].UnitDiscountAmount = 0.00;

                    orderLine[iPos].VAT = 0.00;
                    iPos++;
                }

                resultValue = hostService.ORDAdd(recordSource
                                   , siteCode
                                   , principalCode
                                   , orderStaging.RAMOrderNumber
                                   , lineItems.Count
                                   , Functions.SafeDateTime(orderStaging.DateCreated, DateTime.Now)
                                   , Functions.Truncate(customerBillingStaging.CustomerName, 10)
                                   , Functions.Truncate(HeaderComment, 200)
                                   , orderStaging.CustomerOrderNumber
                                   , Functions.Truncate(customerBillingStaging.CustomerID, 20)
                                   , Functions.Truncate(customerBillingStaging.CustomerName, 32) //InvoiceToName
                                   , Functions.Truncate(customerBillingStaging.StreetAddress1, 32) //InvoiceAddress1
                                   , Functions.Truncate(customerBillingStaging.StreetAddress2, 32)  //InvoiceAddress2
                                   , Functions.Truncate(billingZone.Suburb, 32)//InvoiceSuburb //InvoiceAddress3
                                   , billingZone.HubID//customerStaging.InvoicePostalCode//InvoiceAddress4
                                   , billingZone.PostalCode //customerStaging.InvoiceAddress5
                                   , customerBillingStaging.TelephoneNo //customerStaging.InvoiceAddress6
                                   , false
                                   , "" //customerStaging.VATNumber
                                   , false // printPrice
                                   , orderStaging.Priority
                                   , Functions.SafeBool(orderStaging.ValueAddedPackaging, false) //orderStaging.ValueAddedPackaging
                                   , Functions.Truncate(orderStaging.SalesPerson, 6)
                                   , Functions.Truncate(orderStaging.SalesCategory, 6)
                                   , Functions.Truncate(orderStaging.Processor, 10)
                                   , Functions.Truncate(customerReceiverStaging.CustomerName, 32)  //DeliverStreetAddress1
                                   , Functions.Truncate(customerReceiverStaging.StreetAddress1, 32)  //DeliverStreetAddress2
                                   , Functions.Truncate(customerReceiverStaging.StreetAddress2, 32)  //DeliverStreetAddress3
                                   , Functions.Truncate(receiverZone.Suburb, 32)                 //customerStaging.DeliverSuburb            
                                   , receiverZone.PostalCode //deliver adrress 5
                                   , customerReceiverStaging.TelephoneNo //deliver addres 6
                                   , receiverZone.PostalCode                  //customerStaging.DeliverPostalCode
                                   , Functions.SafeDouble(orderStaging.OrderDiscount, 0.00)
                                   , Functions.SafeDouble(orderStaging.OrderVAT, 0.00)
                                   , 0
                                   , orderLine
                                   , IDNumber
                                   , KYC
                                   , CourierName
                                   , CourierService
                                   , InsuranceRequired
                                   , ValidateDelivery
                                   , Functions.Truncate(Functions.SafeString(customerReceiverStaging.StoreCode), 20));

            }

            catch (Exception ex)
            {
                retVal = 0;
                throw new System.Exception(ex.Message.ToString());


            }
            if (resultValue.Success == true)
                retVal = 1;

            return retVal;

        }


        #endregion


    }
}
