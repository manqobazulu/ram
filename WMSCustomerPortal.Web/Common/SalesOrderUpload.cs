using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSCustomerPortal.Models.Entities;
using WMSCustomerPortal.Business;
using WMSCustomerPortal.Web.Controllers;
//using WMSCustomerPortal.Web.Controllers;

namespace WMSCustomerPortal.Models.Common
{
    public class SalesOrderUpoad
    {
        WMSCustomerPortal.Business.OutboundService _outboundService;

        private WMSCustomerPortal.Business.OutboundService OutboundDataService
        {
            get
            {
                if (_outboundService == null)
                {
                    _outboundService = new WMSCustomerPortal.Business.OutboundService();
                }
                return _outboundService;
            }
        }

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

        private void WriteToAudit(int principalID, string Status, int RecordNo, string Message, string Transaction, string FileName, string EventUser, string TransactionType)
        {
            AuditRecord auditRecord = new AuditRecord();

            auditRecord.FileName = FileName;
            auditRecord.User = EventUser;
            auditRecord.TransactionType = TransactionType;
            auditRecord.EventStatus = Status.ToUpper();
            auditRecord.Message = Message;
            auditRecord.Transaction = Transaction;
            auditRecord.Save(principalID);
        }

        private string LineValid(int PrincipalID, string line, int Line, string FileName, string eventUser, string transactionType)
        {
            if (String.IsNullOrEmpty(line))
            {
                WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " IS EMPTY", "NOT APPLICABLE", FileName, eventUser, transactionType);
                return "FAILED";
            }

            return "";
        }

        private string LineTypeValid(string CurrLineType, string PrevLineType, int Line, int PrincipalID, string FileName, string eventUser, string transactionType)
        {
            if(CurrLineType != "H" && CurrLineType != "D")
            {
                WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " INCORRECT LINE TYPE (SHOULD BE EITHER 'H' OR 'D')", "NOT APPLICABLE", FileName, eventUser, transactionType);
                return "FAILED";
            }

            if(PrevLineType == CurrLineType && PrevLineType == "H")
            {
                WriteToAudit(PrincipalID, "FAILED", Line, "HEADER ROW AT LINE " + Line + " IS NOT FOLLOWED BY DETAIL LINE", "NOT APPLICABLE", FileName, eventUser, transactionType);
                return "FAILED";
            }

            if(CurrLineType == "D" && String.IsNullOrEmpty(PrevLineType))
            {
                return "PART";
            }

            return "";
        }

        private string FieldCountValid(int PrincipalID, string[] Fields, int Line, string FileName, string eventUser, string transactionType, string LineType)
        {
            int NoOfFields = Fields.Length;

            if (LineType == "H")
            {
                if (NoOfFields < 9)
                {
                    if(NoOfFields != 3)
                    {
                        WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " INCORRECT NUMBER OF HEADER FILEDS", "NOT APPLICABLE", FileName, eventUser, transactionType);
                        return "FAILED";
                    }
                }
            }
            else
            {
                if (NoOfFields < 3)
                {
                    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " INCORRECT NUMBER OF DETAIL FIELDS", "NOT APPLICABLE", FileName, eventUser, transactionType);
                    return "FAILED";
                }
            }

            return "";
        }

        private string FieldValidData(int PrincipalID, string[] Fields, int Line, string FileName, string eventUser, string transactionType, string LineType, string CustOrdNo, string PrincipalCode)
        {
            int NoOfFields = Fields.Length;

            if (LineType == "H")
            {
                if (String.IsNullOrEmpty(Fields[1]))
                {
                    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " CUSTOMER ORDER NUMBER NOT PROVIDED", "NOT APPLICABLE", FileName, eventUser, transactionType);
                    return "FAILED";
                }

                string CustOrd = Fields[1].ToUpper();
                if (CustOrd.Length > 20)
                {
                    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | ORDER: " + CustOrd + " | CUSTOMER ORDER NUMBER MAXIMUM LENGTH (20) EXCEEDED", CustOrd, FileName, eventUser, transactionType);
                    return "FAILED";
                }

                OutboundService OServ = new OutboundService();
                if(!OServ.DoesCustomerOrderNumberExist(CustOrd, PrincipalID))
                {
                    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | ORDER: " + CustOrd + " | DUPLICATE ORDER PROVIDED", CustOrd, FileName, eventUser, transactionType);
                    return "FAILED";
                }

                if (String.IsNullOrEmpty(Fields[2]))
                {
                    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | ORDER: " + CustOrd + " | CUSTOMER NAME NOT PROVIDED", CustOrd, FileName, eventUser, transactionType);
                    return "FAILED";
                }

                string Cust = Fields[2].ToUpper();
                List<CustomerOrdersLookUpItem> CustList = SqlManager.OrderCustomerName_Lookup(Cust, PrincipalID, eventUser, "");

                if (CustList.Count <= 0)
                {
                    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | ORDER: " + CustOrd + " | CUSTOMER [" + Cust + "] DOES NOT EXITS", CustOrd, FileName, eventUser, transactionType);
                    return "FAILED";
                }

                if(Fields.Length > 3)
                {
                    if (!String.IsNullOrEmpty(Fields[3]) && Fields[3].Length > 13)
                    {
                        WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | ORDER: " + CustOrd + " | CUSTOMER: " + Cust + " | ERROR: ID NO MAXIMUM LENGTH (13) EXCEEDED", CustOrd, FileName, eventUser, transactionType);
                        return "FAILED";
                    }

                    if (!String.IsNullOrEmpty(Fields[4]) && Fields[4].Length > 3)
                    {
                        WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | ORDER: " + CustOrd + " | CUSTOMER: " + Cust + " | ERROR: KYC MAXIMUM LENGTH (3) EXCEEDED", CustOrd, FileName, eventUser, transactionType);
                        return "FAILED";
                    }

                    if (!String.IsNullOrEmpty(Fields[5]) && Fields[5].Length > 30)
                    {
                        WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | ORDER: " + CustOrd + " | CUSTOMER: " + Cust + " | ERROR: COURIER NAME MAXIMUM LENGTH (30) EXCEEDED", CustOrd, FileName, eventUser, transactionType);
                        return "FAILED";
                    }

                    if (!String.IsNullOrEmpty(Fields[6]) && Fields[6].Length > 3)
                    {
                        WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | ORDER: " + CustOrd + " | CUSTOMER: " + Cust + " | ERROR: COURIER SERVICE MAXIMUM LENGTH (3) EXCEEDED", CustOrd, FileName, eventUser, transactionType);
                        return "FAILED";
                    }

                    string Eval = Fields[7];
                    bool ValidBool = true;
                    if (Eval != "0" && Eval != "1")
                    {
                        ValidBool = bool.TryParse(Fields[7], out bool InsureReq);
                    }

                    if (String.IsNullOrEmpty(Fields[7]) || !ValidBool)
                    {
                        WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | ORDER: " + CustOrd + " | CUSTOMER: " + Cust + " | ERROR: INVALID INSURANCE REQUIRED FIELD VALUE PROVIDED", CustOrd, FileName, eventUser, transactionType);
                        return "FAILED";
                    }

                    Eval = Fields[8];
                    ValidBool = true;
                    if (Eval != "0" && Eval != "1")
                    {
                        ValidBool = bool.TryParse(Fields[8], out bool ValidateDel);
                    }

                    if (String.IsNullOrEmpty(Fields[8]) || !ValidBool)
                    {
                        WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | ORDER: " + CustOrd + " | CUSTOMER: " + Cust + " | ERROR: INVALID VALIDATE DELIVERY FIELD VALUE PROVIDED", CustOrd, FileName, eventUser, transactionType);
                        return "FAILED";
                    }

                    if (Fields.Length == 10 && !String.IsNullOrEmpty(Fields[9]) && Fields[9].Length > 200)
                    {
                        WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | ORDER: " + CustOrd + " | CUSTOMER: " + Cust + " | ERROR: HEADER COMMENT MAXIMUM LENGTH (200) EXCEEDED", CustOrd, FileName, eventUser, transactionType);
                        return "FAILED";
                    }
                }
            }
            else
            {
                ProductStaging staging = new ProductStaging();

                if (String.IsNullOrEmpty(Fields[1]))
                {
                    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | ORDER: " + CustOrdNo + " | ERROR: INVALID PRODUCT [" + Fields[1] + "] SUPPLIED", CustOrdNo, FileName, eventUser, transactionType);
                    return "FAILED";
                }

                List<ProductAutocomplete> ProdList = SqlManager.Product_Lookup(PrincipalID, Fields[1], 1, "ProdCode");

                if (ProdList.Count <= 0)
                {
                    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | ORDER: " + CustOrdNo + " | ERROR: INVALID PRODUCT [" + Fields[1] + "] SUPPLIED", CustOrdNo, FileName, eventUser, transactionType);
                    return "FAILED";
                }

                int LineQty = 0;
                bool ValidQty = int.TryParse(Fields[2], out LineQty);

                if (!ValidQty)
                {
                    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | ORDER: " + CustOrdNo + " | PRODUCT: " + Fields[1] + " | QTY: " + Fields[2] + " | ERROR: QUANTITY HAS TO BE A NUMERIC VALUE", CustOrdNo, FileName, eventUser, transactionType);
                    return "FAILED";
                }

                if (LineQty <= 0)
                {
                    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | ORDER: " + CustOrdNo + " | PRODUCT: " + Fields[1] + " | QTY: " + LineQty + " | ERROR: QUANTITY HAS TO BE POSITIVE AND GREATER THAN 0", CustOrdNo, FileName, eventUser, transactionType);
                    return "FAILED";
                }

                //string retVal = staging.CheckStockQuantity(PrincipalID, Fields[1], LineQty, "");
                int AvQty = 0;

                string retVal = OutboundDataService.CheckStockQuantityNew(PrincipalID, PrincipalCode, Fields[1], LineQty, out AvQty);

                if (retVal != "OK")
                {
                    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | ORDER: " + CustOrdNo + " | PRODUCT: " + Fields[1] + " | QTY: " + LineQty + " | ERROR: " + retVal.ToUpper(), CustOrdNo, FileName, eventUser, transactionType);
                    return "FAILED";
                }
            }

            return "";
        }

        private void CreateOrder(OrderHeader TempOrd, int PrincipalID, string FileName, string eventUser, string transactionType, string eventTerminal)
        {
            List<string> MsgList = new List<string>();
            List<int> MsgListRows = new List<int>();
            string pTable = "";
            int NoOfLines = 0;
            foreach(var i in TempOrd.Lines)
            {
                List<ProductAutocomplete> ProdList = SqlManager.Product_Lookup(PrincipalID, i.ProdCode, 1, "ProdCode");
                if(ProdList.Count > 0)
                {
                    pTable += ProdList[0].ProductStagingID + "|" + i.Quantity;
                    NoOfLines++;

                    MsgList.Add("ROW " + i.LineRow + " | ORDER: " + TempOrd.CustomerOrderNo + " | LINE : " + NoOfLines + " | PRODUCT = " + i.ProdCode + " | QTY: " + i.Quantity + " SUCCESSFULLY UPLOADED");
                    MsgListRows.Add(i.LineRow);

                    if (NoOfLines != TempOrd.Lines.Count)
                    {
                        pTable += ",";
                    }
                }
            }

            OutboundService OServ = new OutboundService();

            List<CustomerOrdersLookUpItem> CustList = SqlManager.OrderCustomerName_Lookup(TempOrd.CustomerName, PrincipalID, eventUser, "");

            if(CustList.Count > 0)
            {
                int Res = OServ.AddOrderSingleReceiver(new Orders
                {
                    CustomerDetailID = CustList[0].CustomerDetailID,
                    BillingCustomerDetailID = CustList[0].CustomerDetailID,
                    PrincipalID = PrincipalID,
                    CustomerOrderNumber = TempOrd.CustomerOrderNo,
                    DateCreated = DateTime.Today,
                    EventUser = eventUser,
                    EventTerminal = eventTerminal,
                    RAMCustomerID = CustList[0].CustomerID,
                    ReceiverRAMCustomerID = CustList[0].BillingCustomerID,
                    BillingRAMCustomerID = CustList[0].BillingCustomerID,
                    CustomerGroupID = CustList[0].CustomerID
                }, TempOrd.IDNo, TempOrd.KYC, TempOrd.CourierName, TempOrd.CourierService, TempOrd.InsuranceRequired, TempOrd.ValidateDelivery, eventTerminal, eventUser, true, TempOrd.HeaderComment, pTable);

                WriteToAudit(PrincipalID, "SUCCESS", TempOrd.LineRow, "ROW " + TempOrd.LineRow + " - ORDER WITH " + NoOfLines + " LINE(S) UPLOADED SUCCESSFULLY", TempOrd.CustomerOrderNo, FileName, eventUser, transactionType);

                int i = 0; 
                foreach(var m in MsgList)
                {
                    WriteToAudit(PrincipalID, "SUCCESS", MsgListRows[i++], m, TempOrd.CustomerOrderNo, FileName, eventUser, transactionType);
                }
            }
        }

        class OrderLine
        {
            public string ProdCode { get; set; }
            public int Quantity { get; set; }
            public int LineRow { get; set; }
        }

        class OrderHeader
        {
            public int LineRow { get; set; }
            public string CustomerOrderNo { get; set; }
            public string CustomerName { get; set; }
            public string IDNo { get; set; }
            public string KYC { get; set; }
            public string CourierName { get; set; }
            public string CourierService { get; set; }
            public bool InsuranceRequired { get; set; }
            public bool ValidateDelivery { get; set; }
            public string HeaderComment { get; set; }
            public List<OrderLine> Lines { get; set; }

            public OrderHeader()
            {
                Lines = new List<OrderLine>();
            }
        }

        public string SaleOrderUpload(List<string> lines, int principalID, string eventTerminal, string eventUser, string FileName , string transactionType, string fileDelimiter, string PrincipalCode)
        {
            AuditRecord audit = new AuditRecord();
            AuditMail auditMail = new AuditMail();

            audit.Transaction = transactionType;
            audit.FileName = FileName;
            audit.User = eventUser;

            int row = 0;// files rows start from row 1
            int TotalOrders = 0;
            int TotalOrdersSuccessful = 0;

            lines.RemoveAt(0);

            try
            {
                string Status = "";

                OrderHeader TempOrd = new OrderHeader();

                string PrevLineType = "";
                string CurrLineType = "";
                string FD = fileDelimiter;
                if (String.IsNullOrEmpty(FD)) FD = ",";

                foreach (string line in lines)
                {
                    Status = "";
                    row++;

                    Status = LineValid(principalID, line, row, FileName, eventUser, transactionType);
                    if (!String.IsNullOrEmpty(Status))
                    {
                        break;
                    }

                    string[] Fields = line.Split(Convert.ToChar(FD));
                    CurrLineType = Fields[0].ToUpper();
                    Status = LineTypeValid(CurrLineType, PrevLineType, row, principalID, FileName, eventUser, transactionType);
                    if (!String.IsNullOrEmpty(Status))
                    {
                        if(Status == "PART")
                        {
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }

                    Status = FieldCountValid(principalID, Fields, row, FileName, eventUser, transactionType, CurrLineType);
                    if (!String.IsNullOrEmpty(Status))
                    {
                        PrevLineType = "";
                        TempOrd = new OrderHeader();
                        continue;
                    }

                    string key = CurrLineType == "D" ? TempOrd.CustomerOrderNo : "";
                    Status = FieldValidData(principalID, Fields, row, FileName, eventUser, transactionType, CurrLineType, key, PrincipalCode);
                    if (!String.IsNullOrEmpty(Status))
                    {
                        if(CurrLineType == "H") TotalOrders += 1;

                        PrevLineType = "";
                        TempOrd = new OrderHeader();
                        continue;
                    }

                    if (CurrLineType == "H")
                    {
                        if(TempOrd.Lines.Count > 0 && !String.IsNullOrEmpty(PrevLineType))
                        {
                            CreateOrder(TempOrd, principalID, FileName, eventUser, transactionType, eventTerminal);
                            TotalOrdersSuccessful += 1;
                        }

                        TotalOrders += 1;
                        bool InsuranceReq = false;
                        bool ValidateDelivery = false;

                        if(Fields.Length > 3)
                        {
                            if (Fields[7] == "0" || Fields[7] == "1")
                                InsuranceReq = Convert.ToBoolean(Convert.ToInt32(Fields[7]));
                            else
                                InsuranceReq = Convert.ToBoolean(Fields[7]);

                            if (Fields[8] == "0" || Fields[8] == "1")
                                ValidateDelivery = Convert.ToBoolean(Convert.ToInt32(Fields[8]));
                            else
                                ValidateDelivery = Convert.ToBoolean(Fields[8]);
                        }

                        TempOrd = new OrderHeader
                        {
                            LineRow = row,
                            CustomerOrderNo = Fields[1].ToUpper(),
                            CustomerName = Fields[2].ToUpper(),
                            IDNo = Fields.Length > 3? Fields[3].ToUpper() : "",
                            KYC = Fields.Length > 3? Fields[4].ToUpper() : "",
                            CourierName = Fields.Length > 3? Fields[5].ToUpper(): "",
                            CourierService = Fields.Length > 3? Fields[6].ToUpper() : "",
                            InsuranceRequired = InsuranceReq,
                            ValidateDelivery = ValidateDelivery
                        };

                        if (Fields.Length < 10)
                        {
                            TempOrd.HeaderComment = "";
                        }
                        else
                        {
                            TempOrd.HeaderComment = Fields[9];
                        }
                    }
                    else
                    {
                        OrderLine ln = new OrderLine();
                        ln.ProdCode = Fields[1].ToUpper();
                        ln.Quantity = Convert.ToInt32(Fields[2]);
                        ln.LineRow = row;
                        TempOrd.Lines.Add(ln);
                    }

                    PrevLineType = CurrLineType;
                }

                if (TempOrd.Lines.Count > 0 && !String.IsNullOrEmpty(PrevLineType))
                {
                    CreateOrder(TempOrd, principalID, FileName, eventUser, transactionType, eventTerminal);
                    TotalOrdersSuccessful += 1;
                }
            }
            catch(Exception Ex)
            {
                WriteToAudit(principalID, "FAILED", row, Ex.Message.ToUpper(), "NOT APPLICABLE", FileName, eventUser, transactionType);
            }

            if (TotalOrders == 0)
            {
                return TotalOrders + " ROW(S) UPLOADED AND " + lines.Count + " ROW(S) FAILED";
            }

            int Diff = TotalOrders - TotalOrdersSuccessful;
            return TotalOrdersSuccessful + " ORDER(S) UPLOADED AND " + Diff + " ORDER(S) FAILED";
        }
    }
}