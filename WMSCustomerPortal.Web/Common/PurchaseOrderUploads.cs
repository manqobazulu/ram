using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSCustomerPortal.Business;
using WMSCustomerPortal.Models.DataAccess;
using WMSCustomerPortal.Models.Entities;

namespace WMSCustomerPortal.Models.Common
{
    public class PurchaseOrderUploads
    {
        class ILine
        {
            public string ProdCode { get; set; }
            public int Quantity { get; set; }
            public float UnitCost { get; set; }
            public int LineRow { get; set; }
        }

        class IBHeader
        {
            public int LineRow { get; set; }
            public string PORef { get; set; }
            public string SupplierName { get; set; }
            public DateTime PODate { get; set; }
            public DateTime ExpectedDeliveryDateTime { get; set; }
            public List<ILine> Lines { get; set; }

            public IBHeader()
            {
                Lines = new List<ILine>();
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
            if (CurrLineType != "H" && CurrLineType != "D")
            {
                WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " INCORRECT LINE TYPE (SHOULD BE EITHER 'H' OR 'D')", "NOT APPLICABLE", FileName, eventUser, transactionType);
                return "FAILED";
            }

            if (PrevLineType == CurrLineType && PrevLineType == "H")
            {
                WriteToAudit(PrincipalID, "FAILED", Line, "HEADER ROW AT LINE " + Line + " IS NOT FOLLOWED BY DETAIL LINE", "NOT APPLICABLE", FileName, eventUser, transactionType);
                return "FAILED";
            }

            if (CurrLineType == "D" && String.IsNullOrEmpty(PrevLineType))
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
                if (NoOfFields < 5)
                {
                    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " INCORRECT NUMBER OF HEADER FILEDS", "NOT APPLICABLE", FileName, eventUser, transactionType);
                    return "FAILED";
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

        private string FieldValidData(int PrincipalID, string[] Fields, int Line, string FileName, string eventUser, string transactionType, string LineType, string dPORef)
        {
            int NoOfFields = Fields.Length;

            if (LineType == "H")
            {
                if (String.IsNullOrEmpty(Fields[1]))
                {
                    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " PO REFERENCE NOT PROVIDED", "NOT APPLICABLE", FileName, eventUser, transactionType);
                    return "FAILED";
                }

                string PORef = Fields[1].ToUpper();
                if (PORef.Length > 20)
                {
                    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | PO: " + PORef + " | PO REFERENCE MAXIMUM LENGTH (20) EXCEEDED", PORef, FileName, eventUser, transactionType);
                    return "FAILED";
                }

                if (String.IsNullOrEmpty(Fields[2]))
                {
                    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | PO: " + PORef + " | SUPPLIER NAME NOT PROVIDED", PORef, FileName, eventUser, transactionType);
                    return "FAILED";
                }

                string SupplierName = Fields[2].ToUpper();

                if (String.IsNullOrEmpty(Fields[3]))
                {
                    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | PO: " + PORef + " | SUPPLIER: " + SupplierName + " | ERROR: PO DATE NOT SUPPLIED", PORef, FileName, eventUser, transactionType);
                    return "FAILED";
                }

                DateTime PODate;
                //bool pPODate = DateTime.TryParse(Fields[3], out PODate);              
                //if (!pPODate)
                //{
                //    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | PO: " + PORef + " | SUPPLIER: " + SupplierName + " | ERROR: INCORRECT PO DATE FORMAT.", PORef, FileName, eventUser, transactionType);
                //    return "FAILED";
                //}

                PODate = DateTime.ParseExact(Fields[3], "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime Today = DateTime.Today;
                if (PODate < Today)
                {
                    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | PO: " + PORef + " | SUPPLIER: " + SupplierName + " | ERROR: PO DATE CANNOT BE IN THE PAST.", PORef, FileName, eventUser, transactionType);
                    return "FAILED";
                }

                if (String.IsNullOrEmpty(Fields[4]))
                {
                    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | PO: " + PORef + " | SUPPLIER: " + SupplierName + " | ERROR: EXPECTED DELIVERY DATE NOT SUPPLIED", PORef, FileName, eventUser, transactionType);
                    return "FAILED";
                }

                DateTime ExpDate;
                //bool pExpectedDeliveryDateTime = DateTime.TryParse(Fields[4], out ExpDate);
                //if (!pExpectedDeliveryDateTime)
                //{
                //    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | PO: " + PORef + " | SUPPLIER: " + SupplierName + " | ERROR: INCORRECT EXPECTED DATE FORMAT.", PORef, FileName, eventUser, transactionType);
                //    return "FAILED";
                //}

                ExpDate = DateTime.ParseExact(Fields[4], "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                if (ExpDate < Today)
                {
                    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | PO: " + PORef + " | SUPPLIER: " + SupplierName + " | ERROR: EXPECTED DELIVERY DATE CANNOT BE IN THE PAST.", PORef, FileName, eventUser, transactionType);
                    return "FAILED";
                }
            }
            else
            {
                ProductStaging staging = new ProductStaging();

                if (String.IsNullOrEmpty(Fields[1]))
                {
                    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | PO: " + dPORef + " | ERROR: INVALID PRODUCT [" + Fields[1] + "] SUPPLIED", dPORef, FileName, eventUser, transactionType);
                    return "FAILED";
                }

                SQLDataManager SqlManager = new SQLDataManager();
                List<ProductAutocomplete> ProdList = SqlManager.Product_Lookup(PrincipalID, Fields[1], 1, "ProdCode");

                if (ProdList.Count <= 0)
                {
                    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | PO: " + dPORef + " | ERROR: INVALID PRODUCT [" + Fields[1] + "] SUPPLIED", dPORef, FileName, eventUser, transactionType);
                    return "FAILED";
                }

                int LineQty = 0;
                bool ValidQty = int.TryParse(Fields[2], out LineQty);

                if (!ValidQty)
                {
                    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | PO: " + dPORef + " | PRODUCT: " + Fields[1] + " | QTY: " + Fields[2] + " | ERROR: QUANTITY HAS TO BE A NUMERIC VALUE", dPORef, FileName, eventUser, transactionType);
                    return "FAILED";
                }

                if (LineQty <= 0)
                {
                    WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | PO: " + dPORef + " | PRODUCT: " + Fields[1] + " | QTY: " + LineQty + " | ERROR: QUANTITY HAS TO BE POSITIVE AND GREATER THAN 0", dPORef, FileName, eventUser, transactionType);
                    return "FAILED";
                }

                if (Fields.Length > 3)
                {
                    if (!String.IsNullOrEmpty(Fields[3]))
                    {
                        float UnitCost = 0;
                        ValidQty = float.TryParse(Fields[3], out UnitCost);

                        if (!ValidQty)
                        {
                            WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " | PO: " + dPORef + " | ERROR: INVALID UNIT COST VALUE", dPORef, FileName, eventUser, transactionType);
                            return "FAILED";
                        }
                    }
                }
            }

            return "";
        }

        private void CreateOrder(IBHeader TempOrd, int PrincipalID, string FileName, string eventUser, string transactionType, string eventTerminal)
        {
            SQLDataManager SqlManager = new SQLDataManager();
            List<string> MsgList = new List<string>();
            List<int> MsgListRows = new List<int>();
            string pTable = "";
            int NoOfLines = 0;
            foreach (var i in TempOrd.Lines)
            {
                List<ProductAutocomplete> ProdList = SqlManager.Product_Lookup(PrincipalID, i.ProdCode, 1, "ProdCode");
                if (ProdList.Count > 0)
                {
                    pTable += ProdList[0].ProductStagingID + "|" + i.UnitCost + "|" + i.Quantity;
                    NoOfLines++;

                    MsgList.Add("ROW " + i.LineRow + " | PO: " + TempOrd.PORef + " | LINE : " + NoOfLines + " | PRODUCT = " + i.ProdCode + " | QTY: " + i.Quantity + " SUCCESSFULLY UPLOADED");
                    MsgListRows.Add(i.LineRow);

                    if (NoOfLines != TempOrd.Lines.Count)
                    {
                        pTable += ",";
                    }
                }
            }

            InboundService inboundService = new InboundService();
            InboundMaster stagingRecord = new InboundMaster();

            string pRecordSource = RAM.Utilities.Common.ConfigSettings.ReadConfigValue("WMSRecordSource", "UNKNOWN");
            string pSiteCode = RAM.Utilities.Common.ConfigSettings.ReadConfigValue("WMSSiteCode", "001");

            stagingRecord.PORef = TempOrd.PORef;
            stagingRecord.SupplierName = TempOrd.SupplierName;
            stagingRecord.PODate = TempOrd.PODate;

            stagingRecord.ExpectedDeliveryDateTime = TempOrd.ExpectedDeliveryDateTime;
            stagingRecord.SiteCode = pSiteCode;
            stagingRecord.RecordSource = pRecordSource;
            stagingRecord.PrincipalID = PrincipalID;

            inboundService.SaveNewOrderWithOrderLine(stagingRecord, eventTerminal, eventUser, pTable);

            WriteToAudit(PrincipalID, "SUCCESS", TempOrd.LineRow, "ROW " + TempOrd.LineRow + " - PO WITH " + NoOfLines + " LINE(S) UPLOADED SUCCESSFULLY", TempOrd.PORef, FileName, eventUser, transactionType);

            int d = 0;
            foreach (var m in MsgList)
            {
                WriteToAudit(PrincipalID, "SUCCESS", MsgListRows[d++], m, TempOrd.PORef, FileName, eventUser, transactionType);
            }
        }

        public string POImport(List<string> lines, int principalID, string eventTerminal, string eventUser, string FileName, string transactionType, string fileDelimiter)
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

                IBHeader TempOrd = new IBHeader();

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
                        if (Status == "PART")
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
                        TempOrd = new IBHeader();
                        continue;
                    }

                    string key = CurrLineType == "D" ? TempOrd.PORef : "";
                    Status = FieldValidData(principalID, Fields, row, FileName, eventUser, transactionType, CurrLineType, key);
                    if (!String.IsNullOrEmpty(Status))
                    {
                        if (CurrLineType == "H") TotalOrders += 1;

                        PrevLineType = "";
                        TempOrd = new IBHeader();
                        continue;
                    }

                    if (CurrLineType == "H")
                    {
                        if (TempOrd.Lines.Count > 0 && !String.IsNullOrEmpty(PrevLineType))
                        {
                            CreateOrder(TempOrd, principalID, FileName, eventUser, transactionType, eventTerminal);
                            TotalOrdersSuccessful += 1;
                        }

                        TotalOrders += 1;

                        TempOrd = new IBHeader
                        {
                            LineRow = row,
                            PORef = Fields[1].ToUpper(),
                            SupplierName = Fields[2].ToUpper(),
                            PODate = DateTime.ParseExact(Fields[3], "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture), //Convert.ToDateTime(Fields[3]),
                            ExpectedDeliveryDateTime = DateTime.ParseExact(Fields[4], "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) //; Convert.ToDateTime(Fields[4])
                        };
                    }
                    else
                    {
                        ILine ln = new ILine();
                        ln.ProdCode = Fields[1].ToUpper();
                        ln.Quantity = Convert.ToInt32(Fields[2]);
                        ln.LineRow = row;
                        if(Fields.Length > 3 && !String.IsNullOrEmpty(Fields[3]))
                        {
                            ln.UnitCost = Convert.ToSingle(Fields[3]);
                        }
                        else
                        {
                            ln.UnitCost = 0;
                        }
                        
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
            return TotalOrdersSuccessful + " PO(S) UPLOADED AND " + Diff + " PO(S) FAILED";
        }

        public string POupload(List<string> lines, int principalID, string eventTerminal, string eventUser, string FileName, string transectionType, string fileDelimiter)
        {
            InboundService inboundService = new InboundService();
            InboundMaster stagingRecord = new InboundMaster();
            InboundMasterLineItem items = new InboundMasterLineItem();

            AuditRecord auditRecord = new AuditRecord();
            AuditMail auditMail = new AuditMail();
            SQLDataManager qLDataManager = new SQLDataManager();


            List<string> auditStatus = new List<string>();
            List<InboundMaster> inboundheader = new List<InboundMaster>();
            List<InboundMasterLineItem> inboundMasterLineItems = new List<InboundMasterLineItem>();
            List<int> successRow = new List<int>(); // couting number of rows
            List<int> failsRow = new List<int>(); // couting number of rows
            List<int> header = new List<int>(); // couting number of rows
            List<string> err = new List<string>();


            string pRecordSource = RAM.Utilities.Common.ConfigSettings.ReadConfigValue("WMSRecordSource", "UNKNOWN");
            string pSiteCode = RAM.Utilities.Common.ConfigSettings.ReadConfigValue("WMSSiteCode", "001");

            auditRecord.TransactionType = transectionType;
            auditRecord.FileName = FileName;
            auditRecord.User = eventUser;

            DateTime date;
            int NumericalValue;
            string Status;
            var prodstaging = 0;
            var quantity = 0;
            string pTable = "";
            string combined = "";
            string POHeader="";
            int x = 1;
            // double UnitPrice;


            int i = 0;
            int row = 0;// files rows start from row 1
            int SuccessStatus = 0;
            int FailsStatus = 0;

          
            //remove tablename from list
            lines.RemoveAt(0);
            int loopingHeader = 0;

            try
            {
                foreach (var obj in lines)
                {
                    row++;
                    Object[] objects;
                    if (obj.ToString().Equals(""))// checking for empty row
                    {
                        auditRecord.EventStatus = "Failed".ToUpper();
                        auditRecord.Message = "Row ".ToUpper() + row + " is empty".ToUpper();
                        auditStatus.Add(auditRecord.Message);
                        Status = "Failed";
                        auditRecord.Save(principalID);
                    }
                    else
                    {
                        //splitting
                        if (fileDelimiter == null)
                        {
                            objects = obj.Split(',');// default delimiter 
                        }
                        else
                        {
                            objects = obj.Split(Convert.ToChar(fileDelimiter));// return string
                           // objects = objects.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                            //objects = objects.Except(new List<string> { string.Empty }).ToArray();




                        }
                        if (objects[0].ToString().ToUpper() == "H")
                        {

                            if (inboundMasterLineItems.Count() != 0)
                            {
                                //FailsStatus = FailsStatus + failsRow.Count();// successfull product rollback
                                if (stagingRecord.PORef != "")
                                {
                                    //SuccessStatus = 0;
                                    //    SuccessStatus++;
                                    //    auditRecord.EventStatus = "SUCCESS";
                                    //auditRecord.Message = " " +POHeader + " SUCCESSFUL SUCCESSFULLY IMPORTED".ToUpper();
                                    //auditStatus.Add(auditRecord.Message);
                                    //auditRecord.Transaction = POHeader;
                                    //auditRecord.Save();
                                    SuccessStatus++;

                                foreach (var s in inboundMasterLineItems)
                                {


                                    string tbl = s.ProductStagingID + "|" + s.UnitCost + "|" + s.ExpectedQuantity + ",";

                                    if (successRow.Count() <= row)
                                    {

                                        auditRecord.EventStatus = "SUCCESS";
                                       // auditRecord.Message = "ROW " + successRow[x] + " , " + s.ProdCode + " SUCCESSFULLY IMPORTED".ToUpper();
                                        auditRecord.Message = "ROW " + successRow[x] + " PO REF = " + POHeader + " ,  PRODUCT = " + s.ProdCode + "  SUCCESSFULLY IMPORTED ".ToUpper();
                                        auditStatus.Add(auditRecord.Message);
                                        auditRecord.Transaction = s.ProdCode;
                                        auditRecord.Save(principalID);
                                        x++;
                                    }

                                    combined += tbl;
                                    SuccessStatus++;
                                }
                                  //  successRow.Clear();
                                loopingHeader = 0;
                                if (combined != "")
                                {
                                    combined = combined.Substring(0, combined.Length - 1);
                                    inboundService.SaveNewOrderWithOrderLine(stagingRecord, eventTerminal, eventUser, combined);
                                        combined = "";
                                }
                                }
                                else
                                {

                                    //SuccessStatus=0;
                                    FailsStatus = FailsStatus + successRow.Count();
                                    auditRecord.EventStatus = "FAILED";
                                    auditRecord.Message = POHeader + " FAILED UPLOADS".ToUpper();
                                    auditStatus.Add(auditRecord.Message);
                                    auditRecord.Transaction = POHeader;
                                    auditRecord.Save(principalID);
                                    loopingHeader = 0;
                                    FailsStatus++;
                                    successRow.Clear();
                                }

                            }

                            inboundMasterLineItems.Clear();
                            //else
                            //{

                            //    auditRecord.EventStatus = "FAILED";
                            //    auditRecord.Message = POHeader + " FAILED UPLOADS".ToUpper();
                            //    auditStatus.Add(auditRecord.Message);
                            //    auditRecord.Transaction = POHeader;
                            //    auditRecord.Save();
                            //    SuccessStatus--;
                            //    FailsStatus++; ;
                            //}

                        }
                        if (objects[0].ToString().ToUpper() == "H" || objects[0].ToString().ToUpper() == "D")
                        {
                            if (loopingHeader == 0)
                            {
                                if (objects.Count() == 5)
                                {
                                    if (objects[0].ToString().ToUpper() == "H")
                                    {
                                        bool PODate = DateTime.TryParse(objects[3].ToString(), out date);
                                        bool ExpectedDeliveryDateTime = DateTime.TryParse(objects[4].ToString(), out date);

                                        if (objects[1].ToString().ToUpper() != "")
                                        {
                                            if (objects[2].ToString().ToUpper() != "") { 
                                            if (PODate == true && ExpectedDeliveryDateTime == true)
                                            {
                                                DateTime Today = new DateTime();
                                                DateTime cPODate = Convert.ToDateTime(objects[3]);
                                                DateTime cExpDate = Convert.ToDateTime(objects[4]);
                                                if (cPODate.AddDays(1) < Today)
                                                {
                                                    if (cExpDate.AddDays(1) < Today)
                                                    {
                                                        stagingRecord.PORef = objects[1].ToString().ToUpper();
                                                        stagingRecord.SupplierName = objects[2].ToString().ToUpper();
                                                        stagingRecord.PODate = Convert.ToDateTime(objects[3]);

                                                        stagingRecord.ExpectedDeliveryDateTime = Convert.ToDateTime(objects[4]);
                                                        stagingRecord.SiteCode = pSiteCode;
                                                        stagingRecord.RecordSource = pRecordSource;
                                                        stagingRecord.PrincipalID = principalID;

                                                        inboundheader.Add(stagingRecord);
                                                        loopingHeader++;
                                                        //SuccessStatus++;

                                                        if (stagingRecord != null)
                                                        {
                                                            auditRecord.EventStatus = "SUCCESS";
                                                            successRow.Add(row);
                                                            header.Add(row);
                                                            POHeader = objects[1].ToString().ToUpper();
                                                            auditRecord.Message = "PURCHASE ORDER REFERENCE " + POHeader.ToUpper();
                                                            auditStatus.Add(auditRecord.Message);
                                                            auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                            auditRecord.Save(principalID);
                                                        }
                                                        else
                                                        {
                                                            auditRecord.EventStatus = "FAILED";
                                                            auditRecord.Message = "ROW " + row + " PURCHASE ORDER HEADER NOT SUCCESSFUL".ToUpper();
                                                            auditStatus.Add(auditRecord.Message);
                                                            auditRecord.Transaction = "NOT APPLICABLE";
                                                            FailsStatus++;
                                                            auditRecord.Save(principalID);
                                                            continue;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        FailsStatus++;
                                                        auditRecord.EventStatus = "FAILED";
                                                        auditRecord.Message = "ROW " + row + " EXPECTED DELIVERY DATE CANNOT BE IN THE PAST.".ToUpper();
                                                        auditStatus.Add(auditRecord.Message);
                                                        auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                        auditRecord.Save(principalID);
                                                        continue;
                                                    }
                                                }
                                                else
                                                {
                                                    FailsStatus++;
                                                    auditRecord.EventStatus = "FAILED";
                                                    auditRecord.Message = "ROW " + row + " PO DATE CANNOT BE IN THE PAST.".ToUpper();
                                                    auditStatus.Add(auditRecord.Message);
                                                    auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                    auditRecord.Save(principalID);
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                FailsStatus++;
                                                auditRecord.EventStatus = "FAILED";
                                                auditRecord.Message = "ROW " + row + " INCORRECT DATE FORMAT.".ToUpper();
                                                auditStatus.Add(auditRecord.Message);
                                                auditRecord.Transaction = "NOT APPLICABLE";
                                                auditRecord.Save(principalID);
                                                continue;
                                            }
                                        }
                                            else
                                            {
                                               FailsStatus++;
                                                auditRecord.EventStatus = "FAILED";
                                                auditRecord.Message = "ROW " + row + " SUPPLIER NAME NOT PROVIDED.".ToUpper();
                                                auditStatus.Add(auditRecord.Message);
                                                auditRecord.Transaction = "NOT APPLICABLE";
                                                auditRecord.Save(principalID);
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                           FailsStatus++;
                                            auditRecord.EventStatus = "FAILED";
                                            auditRecord.Message = "ROW " + row + " POREF NOT PROVIDED.".ToUpper();
                                            auditStatus.Add(auditRecord.Message);
                                            auditRecord.Transaction = "NOT APPLICABLE";
                                            auditRecord.Save(principalID);
                                            continue;
                                        }
                                    }
                                }
                                else
                                {
                                    FailsStatus++;
                                    auditRecord.EventStatus = "FAILED";
                                    auditRecord.Message = "ROW " + row + " PURCHASE ORDER HEADER NOT PROVIDED".ToUpper();
                                    auditStatus.Add(auditRecord.Message);
                                    auditRecord.Transaction =objects[1].ToString().ToUpper();
                                    auditRecord.TransactionType = "PURCHASE ORDER";
                                    auditRecord.Save(principalID);
                                    continue;
                                }
                            }
                            if (loopingHeader >= 1 && objects[0].ToString().ToUpper() == "D")
                            {

                                List<ProductAutocomplete> results = qLDataManager.Product_Lookup(principalID, objects[1].ToString(), 20, "ProdCode"); //looking up for the product code 
                                bool Quantit = Int32.TryParse(objects[2].ToString(), out NumericalValue);
                                if (objects.Count() == 3)
                                {
                                    if (objects[2].ToString() != "" && Quantit == true)
                                    {
                                        if (results.Count() == 0)
                                        {
                                            // invalid product code
                                            auditRecord.EventStatus = "FAILED";
                                            FailsStatus++;
                                            err.Add("ROW " + row + objects[1].ToString().ToUpper() + ", INVALID PRODUCT CODE".ToUpper());
                                            auditRecord.Message = "ROW " + row + " PO REF = "+POHeader+" ,  PRODUCT = " + objects[1].ToString().ToUpper() +" INVALID ".ToUpper();
                                            auditStatus.Add(auditRecord.Message);
                                            auditRecord.Transaction = objects[1].ToString().ToUpper();
                                            failsRow.Add(row);
                                            auditRecord.Save(principalID);
                                            stagingRecord.PORef = "";
                                            
                                            combined = null;
                                            continue;
                                        }
                                        else
                                        {
                                            items.ProductStagingID = results[0].ProductStagingID;
                                            items.ExpectedQuantity = Convert.ToInt16(objects[2].ToString());
                                            items.UnitCost = Convert.ToDouble(results[0].UnitCost);
                                            //pTable = prodstaging + "|" + UnitPrice + "|" + quantity;

                                            inboundMasterLineItems.Add(new InboundMasterLineItem(items.ProductStagingID, items.ExpectedQuantity, items.UnitCost, objects[1].ToString()));
                                            List<string> prodlines = new List<string>();

                                            //foreach (var s in inboundMasterLineItems)
                                            //{

                                            //    string tbl = s.ProductStagingID + "|" + s.SalesPrice + "|" + s.ExpectedQuantity +",";

                                            //   // prodlines.Add(tbl);

                                            //        combined += tbl;
                                            //}
                                            // combined += prodlines[0]+",";


                                            // inboundService.SaveNewOrderWithOrderLine(stagingRecord, eventTerminal, eventUser, combined); // send the header to the database
                                            // SuccessStatus++;
                                            successRow.Add(row);
                                            //  auditRecord.Transaction = objects[1].ToString().ToUpper();
                                            //  auditRecord.EventStatus = "SUCCESS";
                                            //  auditRecord.Message = "ROW " + row + " IMPORT SUCCESS";
                                            //  auditStatus.Add(auditRecord.Message);
                                            //  auditRecord.Save();// save to auditRecord
                                            // inboundMasterLineItems.RemoveAt(0);
                                        }
                                    }
                                    else
                                    {
                                        // quantity cannot be null
                                        auditRecord.EventStatus = "FAILED";
                                        FailsStatus++;
                                        auditRecord.Message = "ROW " + row + " PO REF = " + POHeader + " ,  PRODUCT = " + objects[1].ToString().ToUpper() + "  Non Nagative number only required ".ToUpper();
                                        auditStatus.Add(auditRecord.Message);
                                        auditRecord.Transaction = objects[1].ToString().ToUpper();
                                        auditRecord.Save(principalID);
                                        failsRow.Add(row);
                                        stagingRecord.PORef = "";
                                        combined = null;
                                        continue;
                                    }
                                }
                                else
                                {
                                    auditRecord.EventStatus = "FAILED";
                                    FailsStatus++;
                                    auditRecord.Message = "ROW " + row + "Incorrect Number of fileds".ToUpper();
                                    auditStatus.Add(auditRecord.Message);
                                    auditRecord.Transaction = POHeader;
                                    auditRecord.Save(principalID);
                                    stagingRecord.PORef = "";
                                    failsRow.Add(row);
                                    combined = null;
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            auditRecord.EventStatus = "FAILED";
                            auditRecord.Message = "ROW " + row + " - EACH ROW SHOULD START WITH EITHER A HEADER (H) OR DETAIL (D).".ToUpper();
                            auditRecord.Transaction = "NOT APPLICABLE";
                            auditStatus.Add(auditRecord.Message);
                            auditRecord.TransactionType = "PURCHASE ORDER";
                            auditRecord.Save(principalID);
                           // inboundMasterLineItems = null;
                            combined = null;
                            stagingRecord.PORef ="";
                            continue;
                        }
                    }
                }
                if (inboundMasterLineItems.Count() != 0)
                {
                 // FailsStatus= FailsStatus+ failsRow.Count();// successfull product rollback
                    if (stagingRecord.PORef != "")
                    {
                       //// SuccessStatus = 0;

                       // auditRecord.EventStatus = "SUCCESS";
                       // auditRecord.Message = "ROW " + POHeader + " SUCCESSFUL SUCCESSFULLY UPLOADED".ToUpper();
                       // auditStatus.Add(auditRecord.Message);
                       // auditRecord.Transaction = POHeader;
                       // auditRecord.Save();
                        

                        foreach (var s in inboundMasterLineItems)
                        {
                            string tbl = s.ProductStagingID + "|" + s.SalesPrice + "|" + s.ExpectedQuantity + ",";

                            if (successRow.Count() <= row)
                            {
                                auditRecord.EventStatus = "SUCCESS";
                                auditRecord.Message = "ROW " + successRow[x] + " PO REF = " + POHeader + " ,  PRODUCT = " + s.ProdCode + "  SUCCESSFULLY IMPORTED ".ToUpper();
                                auditStatus.Add(auditRecord.Message);
                                auditRecord.Transaction = s.ProdCode;
                                auditRecord.Save(principalID);
                                SuccessStatus++;
                                x++;
                            }

                            combined += tbl;
                            SuccessStatus++;
                        }
                        // SuccessStatus = 0;

                        //auditRecord.EventStatus = "SUCCESS";
                        //auditRecord.Message = "ROW " + POHeader + " SUCCESSFUL SUCCESSFULLY UPLOADED".ToUpper();
                        //auditStatus.Add(auditRecord.Message);
                        //auditRecord.Transaction = POHeader;
                        //auditRecord.Save();


                        loopingHeader = 0;
                        if (combined != "")
                        {
                            combined = combined.Substring(0, combined.Length - 1);
                            inboundService.SaveNewOrderWithOrderLine(stagingRecord, eventTerminal, eventUser, combined);
                        }
                        SuccessStatus = successRow.Count();
                    }
                    else
                    {
                        //SuccessStatus--;
                        FailsStatus++;
                        auditRecord.EventStatus = "FAILED";
                        auditRecord.Message = POHeader + " FAILED UPLOADS".ToUpper();
                        auditStatus.Add(auditRecord.Message);
                        auditRecord.Transaction = POHeader;
                        auditRecord.Save(principalID);
                        loopingHeader = 0;
                    }

                }
                else
                {
                    auditRecord.EventStatus = "FAILED";
                    auditRecord.Message = POHeader + " FAILED UPLOADS".ToUpper();
                    auditStatus.Add(auditRecord.Message);
                    auditRecord.Transaction = POHeader;
                    auditRecord.Save(principalID);
                   // SuccessStatus=0;
                     FailsStatus++; ;
                }

               
            }
            catch (Exception Ex)
            {
                // Reading error to the database table Audit Table 
                auditRecord.EventStatus = "FAILED";
                auditRecord.Message = Ex.Message.ToUpper();
                auditStatus.Add(Ex.Message.ToUpper());
                Status = "Failed";
                auditRecord.Save(principalID);
                Console.WriteLine("Error  details" + Ex);
            }          
            // call the Email send;
            //auditMail.Sendmail(auditStatus, eventUser, FileName);


            if (FailsStatus ==0)// return all successful rows
            {
                return SuccessStatus + " Row(s) Uploaded Successfully ".ToUpper() + " And ".ToUpper() + FailsStatus + " Row(s) Failed".ToUpper();
            }
            if (SuccessStatus == 0)// return all failed rows
            {
                --row;
                return SuccessStatus + " Row(s) Uploaded Successfully ".ToUpper() + " And ".ToUpper() + FailsStatus + " Row(s) Failed".ToUpper();
            }
            // Partialy successfully , failed rows and succeeded rows 
            return SuccessStatus + " Row(s) Uploaded Successfully ".ToUpper() + " And ".ToUpper() + FailsStatus + " Row(s) Failed".ToUpper();

        }
        
    }
}