using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMSCustomerPortal.Models.Common;
using WMSCustomerPortal.Models.Entities;
using WMSCustomerPortal.Web.Controllers;

namespace WMSCustomerPortal.Web.Common
{
    public class CustomerReturnUpload
    {

        WMSCustomerPortal.Models.DataAccess.SQLDataManager _sqlManager;

        private WMSCustomerPortal.Models.DataAccess.SQLDataManager SqlManager
        {
            get
            {
                if (_sqlManager == null)
                {
                    _sqlManager = new WMSCustomerPortal.Models.DataAccess.SQLDataManager();
                }
                return _sqlManager;
            }
        }

        WMSCustomerPortal.Business.ReturnService _returnService;
        private WMSCustomerPortal.Business.ReturnService DataService
        {
            get
            {
                if (_returnService == null)
                {
                    _returnService = new WMSCustomerPortal.Business.ReturnService();
                }
                return _returnService;
            }
        }

        public string ReturnsUpload(List<string> lines, int principalID, string eventTerminal, string eventUser, string FileName , string transectionType, string fileDelimiter)
        {
            List<ReturnOrdersLookUpItem> data = new List<ReturnOrdersLookUpItem>();
            AuditRecord audit = new AuditRecord();
            AuditMail auditMail = new AuditMail();
            List<int> successLog = new List<int>();
            Object[] objects = new object[lines.Count];
            List<string> auditStatus = new List<string>();
            List<SaveReturnsLine> stringprod = new List<SaveReturnsLine>();
            List<SaveReturnsLine> stringprod1 = new List<SaveReturnsLine>();

            audit.Transaction = transectionType;
            audit.FileName = FileName;
            audit.User = eventUser;
            int headerSuccess = 0;
            int lineSuccess = 0;
            int detailsfail = 0;
            int SuccessStatus = 0;
            int failers = 0;
            bool result = false;
            bool sendReturn = false;
            int output = 0;
            int orderId = 0;
            string pTable = "";
            String OrderHeader="";
            int row = 0;// files rows start from row 1
            string mname = lines[0].ToString();
            Dictionary<int, SaveReturnsLine> mOrderLineItems = null;
            List<OrderLineItems> listResult = null;
            SaveReturnsLine saveibl = new SaveReturnsLine();
            SaveReturnsLine saveProd = new SaveReturnsLine();

            try
            {


                if (lines[0].ToString().Equals(""))
                {
                    audit.EventStatus = "FAILED".ToUpper();
                    audit.Message = "ROW ".ToUpper() + row + " IS EMPTY".ToUpper();
                    auditStatus.Add(audit.Message);
                    audit.TransactionType = "NOT APPLICABLE";
                    audit.Save(principalID);
                }
                else
                {
                    lines.RemoveAt(0);  //Remove the first index
                }

                for (int x = 0; x < lines.Count; x++)
                {
                    row++;
                    if (lines[x].ToString().Equals(""))// checking for empty row
                    {

                        audit.EventStatus = "FAILED".ToUpper();
                        audit.Message = "ROW ".ToUpper() + row + " IS EMPTY".ToUpper();
                        auditStatus.Add(audit.Message);
                        audit.TransactionType = "NOT APPLICABLE";
                        audit.Save(principalID);
                    }
                    else
                    {
                        if (fileDelimiter == null)
                        {
                            objects = lines[x].Split(',');// default delimiter 
                        }
                        else
                        {
                            objects = lines[x].Split(Convert.ToChar(fileDelimiter));// return string
                        }

                        if (objects.Count() != 3)
                        {
                            // this will check if the object got splitted by the delimiter, am using if trying to avoid ArrayOutOfBoundException throw

                            audit.EventStatus = "FAILED";
                            audit.Message = "ROW " + row + " INCORRECT NUMBER OF FIELDS.".ToUpper();
                            auditStatus.Add(audit.Message);
                            audit.TransactionType = "NOT APPLICABLE";
                            audit.Save(principalID);
                            failers++;
                        }
                        else
                        {
                            if (Convert.ToString(objects[0]).ToUpper() == "H" || Convert.ToString(objects[0]).ToUpper() == "D")
                            {
                                if (Convert.ToString(objects[0]).ToUpper() == "H" )
                                {
                                    
                                    headerSuccess++;
                                    lineSuccess = 0;
                                    

                                    String orderID = objects[1].ToString();
                                    //Check if customer details exist
                                    data = SqlManager.ReturnOrderNumber_Lookup(orderID, principalID, eventUser);

                                    if (data.Count != 0)
                                    {
                                        SuccessStatus++;
                                        audit.EventStatus = "SUCCESS";
                                        audit.Message = "ROW " + row + " Consigment ID " + Convert.ToString(objects[1]).ToUpper();
                                        OrderHeader= Convert.ToString(objects[1]).ToUpper();

                                        auditStatus.Add(audit.Message);
                                        audit.TransactionType = Convert.ToString(data[0].OrderNumber);
                                        audit.Save(principalID);

                                        //save the order ID to search for customer information
                                        orderId = Convert.ToInt32(data[0].OrderID);

                                        //use the order id to get the order line items
                                        listResult = OrderLineItems.ReadAll_List(orderId);

                                        if (mOrderLineItems == null)
                                        {
                                            mOrderLineItems = new Dictionary<int, SaveReturnsLine>();
                                        }

                                        //for (int i = 0; i < listResult.Count; i++)
                                        //{
                                        //    // row++;

                                        //    saveibl.LineNumber = int.Parse(listResult[i].LineNumber);
                                        //    saveibl.LineItemID = listResult[i].LineItemID;
                                        //    saveibl.ProductStagingID = listResult[i].ProductStagingID;
                                        //    saveibl.ProductCode = listResult[i].ProductCode;
                                        //    saveibl.EANCode = listResult[i].EANCode;
                                        //    saveibl.ShortDescription = listResult[i].ShortDescription;
                                        //    saveibl.LongDescription = listResult[i].LongDescription;
                                        //    saveibl.Quantity = Convert.ToInt32(listResult[i].Quantity);
                                        //}

                                            if (objects[2].ToString().ToUpper() == "PART ORDER")
                                            {
                                                saveibl.OldQuantity = 0;
                                                result = true;

                                            }
                                        if (objects[2].ToString().ToUpper() == "FULL ORDER")
                                        {
                                            if (listResult.Count() != 0) {
                                                successLog.Add(row);

                                                for (int J = 0; J < listResult.Count; J++)
                                                {
                                                    // row++;

                                                    saveibl.LineNumber = int.Parse(listResult[J].LineNumber);
                                                    saveibl.LineItemID = listResult[J].LineItemID;
                                                    saveibl.ProductStagingID = listResult[J].ProductStagingID;
                                                    saveibl.ProductCode = listResult[J].ProductCode;
                                                    saveibl.EANCode = listResult[J].EANCode;
                                                    saveibl.ShortDescription = listResult[J].ShortDescription;
                                                    saveibl.LongDescription = listResult[J].LongDescription;
                                                    saveibl.Quantity = Convert.ToInt32(listResult[J].Quantity);
                                                    stringprod.Add(saveibl);
                                                    if (!mOrderLineItems.ContainsKey(int.Parse(listResult[J].LineNumber)))
                                                    {
                                                        mOrderLineItems.Add(int.Parse(listResult[J].LineNumber), saveibl);
                                                    }

                                                }
                                            }
                                           
                                                
                                                //if (objects[2].ToString().ToUpper() == "FULL ORDER")
                                                //{
                                                //    audit.EventStatus = "SUCCESS";
                                                //    audit.Message = "ROW " + row + " PRODUCT IMPORT SUCCESSFUL - " + listResult[i].ProductCode;

                                                //    auditStatus.Add(audit.Message);
                                                //    audit.TransactionType = Convert.ToString(data[0].OrderNumber);
                                                //    audit.Save();
                                                //}
                                                SuccessStatus++;
                                                //saveibl.OldQuantity = Convert.ToInt32(listResult[i].Quantity);

                                                sendReturn = true;
                                            }

                                            //if (!mOrderLineItems.ContainsKey(int.Parse(listResult[i].LineNumber)))
                                            //{
                                            //    mOrderLineItems.Add(int.Parse(listResult[i].LineNumber), saveibl);
                                            //}

                                            //save products in an array because we have to use a PIPE to concatinate them before persisting them into the DB
                                            //stringprod.Add(saveibl);
                                            //if (objects[2].ToString().ToUpper() == "FULL ORDER")
                                            //{
                                            //    audit.EventStatus = "SUCCESS";
                                            //    audit.Message = "ROW " + row + " PRODUCT IMPORT SUCCESSFUL - " + listResult[i].ProductCode;

                                            //    auditStatus.Add(audit.Message);
                                            //    audit.TransactionType = Convert.ToString(data[0].OrderNumber);
                                            //    audit.Save();
                                            //}
                                           // SuccessStatus++;
                                        
                                    }
                                    else
                                    {
                                        audit.EventStatus = "FAILED";
                                        audit.Message = "ROW " + row + " Consigment ID/ORDER NUMBER DOES NOT EXIST.".ToUpper();
                                        auditStatus.Add(audit.Message);
                                        audit.TransactionType = "NOT APPLICABLE";
                                        audit.Save(principalID);

                                        failers++;
                                    }
                                }
                                
                            }
                            else
                            {
                                audit.EventStatus = "FAILED";
                                audit.Message = "ROW " + row + " - EACH ROW SHOULD START WITH EITHER A HEADER (H) OR DETAIL (D).".ToUpper();

                                auditStatus.Add(audit.Message);
                                audit.TransactionType = "NOT APPLICABLE";
                                audit.Save(principalID);

                                failers++;
                            }

                            if (result == true && Convert.ToString(objects[0]).ToUpper() == "D")
                            {
                              //  row++;
                                lineSuccess++;
                                headerSuccess = 0;

                                sendReturn = true;


                                for (int i = 0; i < listResult.Count; i++)
                                {
                                   // row++;

                                    saveProd.LineNumber = int.Parse(listResult[i].LineNumber);
                                    saveProd.LineItemID = listResult[i].LineItemID;
                                    saveProd.ProductStagingID = listResult[i].ProductStagingID;
                                    saveProd.ProductCode = listResult[i].ProductCode;
                                    saveProd.EANCode = listResult[i].EANCode;
                                    saveProd.ShortDescription = listResult[i].ShortDescription;
                                    saveProd.LongDescription = listResult[i].LongDescription;
                                    saveProd.Quantity = Convert.ToInt32(listResult[i].Quantity);

                                    if (listResult[i].ProductCode.Equals(objects[1].ToString()))
                                    {
                                        int n = 0;
                                        bool ValidaQuntity = int.TryParse(objects[2].ToString(), out n);
                                        if (ValidaQuntity == true)
                                        {
                                            if( Convert.ToInt32(objects[2])<=Convert.ToInt32(listResult[i].Quantity))
                                            {
                                                saveProd.Quantity = Convert.ToInt32(objects[2]);
                                                stringprod1.Add(saveProd);
                                                SuccessStatus++;
                                                successLog.Add(row);
                                            }
                                            else
                                            {
                                                audit.EventStatus = "FAILED";
                                                audit.Message = "ROW " + row + " Consigmnet ID/ ORDER NUMBER = " + OrderHeader + " PRODUCT = " + objects[1].ToString().ToUpper() +" QUANTITY EXCEEDED.".ToUpper();

                                                auditStatus.Add(audit.Message);
                                                audit.TransactionType = objects[1].ToString().ToUpper();
                                                audit.Save(principalID);

                                                failers++;

                                            }



                                        }
                                        else
                                        {
                                            audit.EventStatus = "FAILED";
                                            audit.Message = "ROW " + row + " Consigmnet ID/ ORDER NUMBER = "+OrderHeader+" PRODUCT = "+ objects[1].ToString().ToUpper()  +" QUNTITY, INCORRECT DATATYPE NUMERICAL VALUE EXPECTED.".ToUpper();

                                            auditStatus.Add(audit.Message);
                                            audit.TransactionType = objects[1].ToString().ToUpper();
                                            audit.Save(principalID);

                                            failers++;

                                        }


                                    }

                                 //   stringprod1.Add(saveProd);

                                    //SuccessStatus++;
                                }
                                if (stringprod1.Count() == 0)
                                {
                                    audit.EventStatus = "FAILED";
                                    audit.Message = "ROW " + row + " " +"CONSIGNMENT ID = " + OrderHeader  +" PRODUCT = " + objects[1].ToString().ToUpper() + "INVALID";

                                    auditStatus.Add(audit.Message);
                                    audit.TransactionType = objects[1].ToString().ToUpper();
                                    audit.Save(principalID);

                                    failers++;
                                }
                                //if (stringprod1.Count() == 0)
                                //{

                                //}

                            }
                            if(result == false && Convert.ToString(objects[0]).ToUpper() == "D")
                            {
                                audit.EventStatus = "FAILED";
                                audit.Message = "ROW " + row + " HEADER NOT PROVIDED".ToUpper();
                                auditStatus.Add(audit.Message);
                                audit.TransactionType = objects[1].ToString().ToUpper();
                                audit.Save(principalID);
                                failers++;
                                SuccessStatus--;

                            }
                            //else
                            //{
                            //    audit.EventStatus = "FAILED";
                            //    audit.Message = "ROW " + row + " HEADER NOT PROVIDED".ToUpper();
                            //    auditStatus.Add(audit.Message);
                            //    audit.TransactionType = objects[1].ToString().ToUpper();
                            //    audit.Save();
                            //    failers++;
                            //}
                            if (detailsfail != 0 && OrderHeader!="") {
                                stringprod1.Clear();
                            }

                            if (sendReturn)
                            {
                                string productTable = "";
                                if (headerSuccess == 2) { 
                                if (stringprod1.Count != 0)
                                {

                                    foreach (var i in stringprod1)
                                    {
                                        int y = 0;
                                        pTable += i.ProductStagingID + "|" + i.Quantity + ",";
                                        audit.EventStatus = "SUCCESS";
                                        audit.Message = Convert.ToString("ROW " + successLog[y] + " " + OrderHeader + ", " + i.ProductCode + " UPLOAD sucessfully".ToUpper());

                                        auditStatus.Add(audit.Message);
                                        audit.TransactionType = i.ProductCode;
                                        audit.Save(principalID);

                                    }
                                    //Substring the product tables string removing the last comma
                                    productTable = pTable.Remove(pTable.Length - 1);
                                    }
                                    else
                                    {
                                        audit.EventStatus = "FAILED";
                                        audit.Message = Convert.ToString("CONSIGNMENT ID/ORDER = " + OrderHeader + " NO DETAILS, FAILED UPLOADS".ToUpper());
                                        SuccessStatus--;
                                        auditStatus.Add(audit.Message);
                                        audit.TransactionType = OrderHeader;
                                        failers++;
                                        audit.Save(principalID);
                                    }

                                    stringprod1.Clear();

                            }

                                // full part 
                                if (stringprod.Count != 0)
                                {

                                    foreach (var i in stringprod)
                                    {
                                        int y = 0;
                                        pTable += i.ProductStagingID + "|" + i.Quantity + ",";
                                        audit.EventStatus = "SUCCESS";
                                        audit.Message = Convert.ToString(OrderHeader + ", " + i.ProductCode + " UPLOAD sucessfully".ToUpper());

                                        auditStatus.Add(audit.Message);
                                        audit.TransactionType = i.ProductCode;
                                        audit.Save(principalID);

                                    }
                                    stringprod.Clear();
                                    //Substring the product tables string removing the last comma
                                    productTable = pTable.Remove(pTable.Length - 1);
                                }

                                output = DataService.SaveNewReturnOrder(new Returns
                                {
                                    PrincipalID = principalID,
                                    EventTerminal = eventTerminal,
                                    RAMOrderNumber = data[0].OrderNumber,
                                    ReturnType = objects[2].ToString(),
                                    EventUser = eventUser,
                                }, eventTerminal, eventUser, productTable);

                                if (output ==0)
                                {
                                    audit.EventStatus = "FAILED";
                                    audit.Message = Convert.ToString(OrderHeader +" FAILED UPLOADS".ToUpper());

                                    auditStatus.Add(audit.Message);
                                    audit.TransactionType = OrderHeader;
                                    audit.Save(principalID);

                                  //  failers++;
                                  ////  failers++;
                                    SuccessStatus--;
                                }

                                sendReturn = false;
                                result = false;
                                pTable = "";
                                stringprod = new List<SaveReturnsLine>();

                            }
                        }


                    }
                }
                if (lineSuccess == 0 && OrderHeader != "")
                {
                    audit.EventStatus = "FAILED";
                    audit.Message = Convert.ToString("CONSIGNMENT ID/ORDER = "+OrderHeader + " NO DETAILS, FAILED UPLOADS".ToUpper());
                    SuccessStatus--;
                    auditStatus.Add(audit.Message);
                    audit.TransactionType = OrderHeader;
                    failers++;
                    audit.Save(principalID);
                }
            }
            catch (Exception ex)
            {
                audit.EventStatus = "FAILED";
                audit.Message = ex.Message.ToUpper();
                auditStatus.Add(audit.Message);
                audit.TransactionType = "NOT APPLICABLE";
                audit.Save(principalID);
            }

            //call the Email send;
            // auditMail.Sendmail(auditStatus, eventUser, FileName);

            if (SuccessStatus == row - 1)// return all successful rows
            {
                return SuccessStatus + " Row(s) Uploaded Successfully ".ToUpper() + " And ".ToUpper() + failers + " Row(s) Failed".ToUpper();
            }
            if (SuccessStatus == 0)// return all failed rows
            {

                return SuccessStatus + " Row(s) Uploaded Successfully ".ToUpper() + " And ".ToUpper() + failers + " Row(s) Failed".ToUpper();
            }
            return SuccessStatus + " Row(s) Uploaded Successfully".ToUpper() + " And ".ToUpper() + failers + " Row(s) Failed".ToUpper();
        }

    }
}