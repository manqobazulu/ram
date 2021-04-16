using System;
using System.Collections.Generic;
using System.Linq;
using WMSCustomerPortal.Models.Entities;
using WMSCustomerPortal.Models.DataAccess;
using WMSCustomerPortal.Business;
using System.Text.RegularExpressions;

namespace WMSCustomerPortal.Models.Common
{
    public class customerUpload
    {
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

        private string LineValid(int PrincipalID, string line, int Line, string FileName, string eventUser, string transectionType)
        {
            if (String.IsNullOrEmpty(line))
            {
                WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " IS EMPTY", "NOT APPLICABLE", FileName, eventUser, transectionType);
                return "FAILED";
            }

            return "";
        }

        private string FieldCountValid(int PrincipalID, string[] Fields, int Line, string FileName, string eventUser, string transectionType)
        {
            if (Fields.Length != 23)
            {
                WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " INCORRECT NUMBER OF FILEDS", "NOT APPLICABLE", FileName, eventUser, transectionType);
                return "FAILED";
            }

            return "";
        }

        private string CustomerNameValid(int PrincipalID, string CustName, int Line, string FileName, string eventUser, string transectionType)
        {

            if (CustName.Length <= 0) /* Check Customer Name */
            {
                WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " INVALID CUSTOMER FIELD", "NOT APPLICABLE", FileName, eventUser, transectionType);
                return "FAILED";
            }

            if (CustName.Length > 20) /* Check Customer Name */
            {
                WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " CUSTOMER NAME TOO LONG", CustName.ToUpper(), FileName, eventUser, transectionType);
                return "FAILED";
            }

            return "";
        }

        private string CheckHeaderLine(int PrincipalID, string HeaderField, string CustName, int Line, string FileName, string eventUser, string transectionType)
        {
            if (HeaderField != "H")
            {
                WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " THE FIRST FIELD SHOULD CONTAIN THE HEADER VALUE 'H'", CustName, FileName, eventUser, transectionType);
                return "FAILED";
            }

            return "";
        }

        private string CheckDefaultContact(int PrincipalID, string DefaultContact, string CustName, int Line, string FileName, string eventUser, string transectionType)
        {
            if (DefaultContact.Length <= 0) /* Check Default Contact Person */
            {
                WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " INVALID DEFAULT CONTACT FIELD", CustName, FileName, eventUser, transectionType);
                return "FAILED";
            }

            if (DefaultContact.Length > 20) /* Check Default Contact Person */
            {
                WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " DEFAULT CONTACT TOO LONG", CustName, FileName, eventUser, transectionType);
                return "FAILED";
            }

            return "";
        }

        private string CheckPhoneFaxNo(int PrincipalID, string Number, string CustName, int Line, string Type, string FileName, string eventUser, string transectionType)
        {
            /*if (Number.Length <= 0) 
            {
                WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " INCORRECT " + Type + " NUMBER LENGTH", CustName, FileName, eventUser, transectionType);
                return "FAILED";
            }*/

            if (Number.Length > 10)
            {
                WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " INCORRECT " + Type + " NUMBER LENGTH", CustName, FileName, eventUser, transectionType);
                return "FAILED";
            }

            bool isNumeric = int.TryParse(Number, out int iNo);
            if (!isNumeric)
            {
                WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " INCORRECT " + Type + " NUMBER FORMAT", CustName, FileName, eventUser, transectionType);
                return "FAILED";
            }

            return "";
        }

        private string CheckEmailAddress(int PrincipalID, string EmailAddress, string CustName, int Line, string FileName, string eventUser, string transectionType)
        {
            /*if (Number.Length <= 0) 
            {
                WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " INCORRECT " + Type + " NUMBER LENGTH", CustName);
                return "FAILED";
            }*/

            if (EmailAddress.Length > 40)
            {
                WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " EMAIL ADDRESS CANNOT BE MORE THAN 40 CHARACTERS", CustName, FileName, eventUser, transectionType);
                return "FAILED";
            }

            var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            if (regex.IsMatch(EmailAddress) && !EmailAddress.EndsWith("."))
            {}else
            {
                WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " INCORRECT EMAIL ADDRESS FORMAT", CustName, FileName, eventUser, transectionType);
                return "FAILED";
            }

            return "";
        }

        private string CheckAddressLine(int PrincipalID, string AddressLine, string CustName, int Line, string Type, string FileName, string eventUser, string transectionType)
        {
            if (AddressLine.Length <= 0) 
            {
                WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + ", INVALID " + Type, CustName, FileName, eventUser, transectionType);
                return "FAILED";
            }

            if (AddressLine.Length > 40)
            {
                WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + ", " + Type + " TOO LONG", CustName, FileName, eventUser, transectionType);
                return "FAILED";
            }

            return "";
        }

        private string CheckSuburb(int PrincipalID, string Suburb, string CustName, int Line, string FileName, string eventUser, string transectionType)
        {
            if (Suburb.Length <= 0) /* Check Suburb */
            {
                WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " INVALID SUBURB", CustName, FileName, eventUser, transectionType);
                return "FAILED";
            }

            return "";
        }

        private string CheckPostalCode(int PrincipalID, string PostalCode, string CustName, int Line, out string FilePostalCode, string FileName, string eventUser, string transectionType)
        {
            FilePostalCode = "";
            if (PostalCode.Length <= 0) /* Check PostalCode */
            {
                WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " INVALID POSTALCODE", CustName, FileName, eventUser, transectionType);
                return "FAILED";
            }

            FilePostalCode = PostalCode;
            if (FilePostalCode.Length < 4) FilePostalCode = "0" + FilePostalCode;

            return "";
        }

        private string CheckStoreCode(int PrincipalID, string StoreCode, string CustName, int Line, string FileName, string eventUser, string transectionType)
        {
            if (StoreCode.Length > 10) 
            {
                WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " STORECODE TOO LONG", CustName, FileName, eventUser, transectionType);
                return "FAILED";
            }

            return "";
        }

        private string GetAddressZone(int PrincipalID, string Suburb, string FilePostalCode, string CustName, int Line, out string PostalCode, out string ZoneID, string FileName, string eventUser, string transectionType)
        {
            PostalCode = "";
            ZoneID = "";

            CommonService cs = new CommonService();
            List<dynamic> Suburbs = cs.Suburb_Lookup1(Suburb);

            if (Suburbs.Count() <= 0)
            {
                WriteToAudit(PrincipalID, "FAILED", Line, "ROW " + Line + " INCORRECT POSTALCODE", CustName, FileName, eventUser, transectionType);
                return "FAILED";
            }

            int x = 0;
            while (x < Suburbs.Count())
            {
                string CountStringLength = Suburbs[x].PostalCode;
                if (CountStringLength.Length < 4) CountStringLength = "0" + CountStringLength;

                if (CountStringLength == FilePostalCode)
                {
                    PostalCode = CountStringLength;
                    ZoneID = Suburbs[x].ZoneID;
                    break;
                }

                x++;
            }

            if (String.IsNullOrEmpty(ZoneID))
            {
                ZoneID = Suburbs[0].ZoneID;
            }

            return "";
        }

        private string GetCustomerID(int PrincipalID, string CustName)
        {
            string CustomerID = "NEW";
            SQLDataManager dataservice = new SQLDataManager();

            List<CustomerLookUpName> customerLookUpItems = dataservice.Customer_LookUp_By_CustomerName(PrincipalID, CustName);
            if (customerLookUpItems.Count() > 0) CustomerID = customerLookUpItems.LastOrDefault().CustomerID;

            return CustomerID;
        }

        private CustomerSaveResponse AddUpdateCustomer(string eventTerminal, string empID, int PrincipalID, string ZoneID, string CustomerID, 
                                                       string CustomerName, string DefaultContactPerson, string CellNo, string TelephoneNo,
                                                       string EmailAddress, string StreetAddress1, string StreetAddress2, string StoreCode)
        {
            CustomerSaveResponse Res = new CustomerSaveResponse();
            Customers Rec = new Customers();
            MasterDataService masterDataService = new MasterDataService();

            Rec.EmployeeID = empID;
            Rec.TerminalID = eventTerminal;
            Rec.ZoneID = ZoneID;
            Rec.CustomerID = CustomerID;
            Rec.CustomerName = CustomerName;
            Rec.DefaultContactPerson = DefaultContactPerson;
            Rec.CellNo = CellNo;
            Rec.TelephoneNo = TelephoneNo;
            Rec.EmailAddress = EmailAddress;
            Rec.StreetAddress1 = StreetAddress1;
            Rec.StreetAddress2 = StreetAddress2;
            Rec.StoreCode = StoreCode;
            Rec.isActive = 1;
            Rec.isShipper = 0;

            try
            {
                Res = masterDataService.CreateNewCustomer(PrincipalID, Rec, "", true);
            }
            catch(Exception ex)
            {
                Res.Saved = false;
                Res.CustomerID = "";
                Res.Details = ex.Message;
                Res.AlternateCustomerID = "";
            }

            return Res;
        }

        private string ValidateAddress(int principalID, int row, string HeaderField, string CustomerName, string DefaultContactPerson,
                                       string CellNo, string CellNoDesc, string TelephoneNo, string TelephoneNoDesc,
                                       string FaxNo, string FaxNoDesc, string EmailAddress, string StreetAddress1, string StreetAddress1Desc,
                                       string StreetAddress2, string StreetAddress2Desc, string Suburb, string Postalcode, string StoreCode, out string FilePostalCode, 
                                       string FileName, string eventUser, string transectionType)
        {
            string Status = "";
            FilePostalCode = "";

            Status = CustomerNameValid(principalID, CustomerName, row, FileName, eventUser, transectionType);
            if (!String.IsNullOrEmpty(Status)) return Status;
            
            Status = CheckHeaderLine(principalID, HeaderField, CustomerName, row, FileName, eventUser, transectionType);
            if (!String.IsNullOrEmpty(Status)) return Status;

            Status = CheckDefaultContact(principalID, DefaultContactPerson, CustomerName, row, FileName, eventUser, transectionType);
            if (!String.IsNullOrEmpty(Status)) return Status;

            Status = CheckPhoneFaxNo(principalID, CellNo, CustomerName, row, CellNoDesc, FileName, eventUser, transectionType);
            if (!String.IsNullOrEmpty(Status)) return Status;

            Status = CheckPhoneFaxNo(principalID, TelephoneNo, CustomerName, row, TelephoneNoDesc, FileName, eventUser, transectionType);
            if (!String.IsNullOrEmpty(Status)) return Status;

            Status = CheckPhoneFaxNo(principalID, FaxNo, CustomerName, row, FaxNoDesc, FileName, eventUser, transectionType);
            if (!String.IsNullOrEmpty(Status)) return Status;

            Status = CheckEmailAddress(principalID, EmailAddress, CustomerName, row, FileName, eventUser, transectionType);
            if (!String.IsNullOrEmpty(Status)) return Status;

            Status = CheckAddressLine(principalID, StreetAddress1, CustomerName, row, StreetAddress1Desc, FileName, eventUser, transectionType);
            if (!String.IsNullOrEmpty(Status)) return Status;

            Status = CheckAddressLine(principalID, StreetAddress2, CustomerName, row, StreetAddress2Desc, FileName, eventUser, transectionType);
            if (!String.IsNullOrEmpty(Status)) return Status;

            Status = CheckSuburb(principalID, Suburb, CustomerName, row, FileName, eventUser, transectionType);
            if (!String.IsNullOrEmpty(Status)) return Status;

            Status = CheckPostalCode(principalID, Postalcode, CustomerName, row, out FilePostalCode, FileName, eventUser, transectionType);
            if (!String.IsNullOrEmpty(Status)) return Status;

            Status = CheckStoreCode(principalID, StoreCode, CustomerName, row, FileName, eventUser, transectionType);
            if (!String.IsNullOrEmpty(Status)) return Status;

            return "";
        }

        public string customerFile(List<string> lines, int principalID, string eventTerminal, string empID, string eventUser, string FileName, string transactionType, string fileDelimiter)
        {
            AuditRecord auditRecord = new AuditRecord();
            AuditMail auditMail = new AuditMail();

            List<ProductStaging> list = new List<ProductStaging>();
            List<string> auditStatus = new List<string>();

            auditRecord.FileName = FileName;
            auditRecord.User = eventUser;
            auditRecord.TransactionType = transactionType;

            int row = 0;// files rows start from row 1
            int SuccessStatus = 0;
            int FailsStatus = 0;
            
            lines.RemoveAt(0);

            try
            {
                string Status = "";
                string FD = fileDelimiter;
                if (String.IsNullOrEmpty(FD)) FD = ",";

                foreach (string line in lines)
                {
                    Status = "";
                    row++;

                    Status = LineValid(principalID, line, row, FileName, eventUser, transactionType);
                    if (!String.IsNullOrEmpty(Status))
                    {
                        FailsStatus++;
                        break;
                    }

                    string[] Fields = line.Split(Convert.ToChar(FD));
                    Status = FieldCountValid(principalID, Fields, row, FileName, eventUser, transactionType);
                    if (!String.IsNullOrEmpty(Status))
                    {
                        FailsStatus++;
                        continue;
                    }

                    string CustomerName = Fields[1].ToUpper();
                    string DefaultContactPerson = Fields[2].ToUpper();
                    string CellNo = Fields[3];
                    string TelephoneNo = Fields[4];
                    string FaxNo = Fields[5];
                    string EmailAddress = Fields[6].ToUpper();
                    string StreetAddress1 = Fields[7].ToUpper();
                    string StreetAddress2 = Fields[8].ToUpper();
                    string Suburb = Fields[9].ToUpper();
                    string Postalcode = Fields[10].ToUpper();
                    string StoreCode = Fields[11].ToUpper();
                    string FilePostalCode = "";
                    string ZoneID = "";
                    string CustomerID = "";

                    Status = ValidateAddress(principalID, row, Fields[0].ToUpper(), CustomerName, DefaultContactPerson,
                                             CellNo, "CELLPHONE", TelephoneNo, "TELEPHONE", FaxNo, "FAX", EmailAddress,
                                             StreetAddress1, "STREET ADDRESS1", StreetAddress2, "STREET ADDRESS2", Suburb,
                                             Postalcode, StoreCode, out FilePostalCode, FileName, eventUser, transactionType);
                    if (!String.IsNullOrEmpty(Status))
                    {
                        FailsStatus++;
                        continue;
                    }

                    Status = GetAddressZone(principalID, Suburb, FilePostalCode, CustomerName, row, out Postalcode, out ZoneID, FileName, eventUser, transactionType);
                    if (!String.IsNullOrEmpty(Status))
                    {
                        FailsStatus++;
                        continue;
                    }

                    CustomerID = GetCustomerID(principalID, CustomerName);

                    string bCustomerName = Fields[12].ToUpper();
                    string bDefaultContactPerson = Fields[13].ToUpper();
                    string bCellNo = Fields[14];
                    string bTelephoneNo = Fields[15];
                    string bFaxNo = Fields[16];
                    string bEmailAddress = Fields[17].ToUpper();
                    string bStreetAddress1 = Fields[18].ToUpper();
                    string bStreetAddress2 = Fields[19].ToUpper();
                    string bSuburb = Fields[20].ToUpper();
                    string bPostalcode = Fields[21].ToUpper();
                    string bStoreCode = Fields[22].ToUpper();
                    string bFilePostalCode = "";
                    string bZoneID = "";
                    string bCustomerID = "";

                    Status = ValidateAddress(principalID, row, Fields[0].ToUpper(), bCustomerName, bDefaultContactPerson,
                                             bCellNo, "BILLING CELLPHONE", bTelephoneNo, "BILLING TELEPHONE", bFaxNo, "BILLING FAX", bEmailAddress,
                                             bStreetAddress1, "BILLING STREET ADDRESS1", bStreetAddress2, "BILLING STREET ADDRESS2", bSuburb,
                                             bPostalcode, bStoreCode, out bFilePostalCode, FileName, eventUser, transactionType);
                    if (!String.IsNullOrEmpty(Status))
                    {
                        FailsStatus++;
                        continue;
                    }

                    Status = GetAddressZone(principalID, bSuburb, bFilePostalCode, bCustomerName, row, out bPostalcode, out bZoneID, FileName, eventUser, transactionType);
                    if (!String.IsNullOrEmpty(Status))
                    {
                        FailsStatus++;
                        continue;
                    }

                    bCustomerID = GetCustomerID(principalID, bCustomerName);

                    var results1 = AddUpdateCustomer(eventTerminal, empID, principalID, ZoneID, CustomerID, CustomerName, 
                                                     DefaultContactPerson, CellNo, TelephoneNo, EmailAddress, StreetAddress1, 
                                                     StreetAddress1, StoreCode);

                    string pCustomerID = results1.CustomerID;
                    string output = pCustomerID.Substring(pCustomerID.IndexOf('.') + 1);

                    if (!results1.Saved)
                    {
                        WriteToAudit(principalID, "FAILED", row, "ROW " + row + " " + results1.Details, CustomerName, FileName, eventUser, transactionType);
                        FailsStatus++;
                        continue;
                    }

                    CustomerSaveResponse results2 = new CustomerSaveResponse();
                    results2.Details = "";

                    if (StreetAddress1 != bStreetAddress1 || StreetAddress2 != bStreetAddress2)
                    {
                        results2 = AddUpdateCustomer(eventTerminal, empID, principalID, bZoneID, bCustomerID, bCustomerName,
                                                     bDefaultContactPerson, bCellNo, bTelephoneNo, bEmailAddress, bStreetAddress1,
                                                     bStreetAddress1, bStoreCode);

                        string bpCustomerID = results2.CustomerID;
                        string output2 = bpCustomerID.Substring(bpCustomerID.IndexOf('.') + 1);

                        MasterDataService masterDataService = new MasterDataService();
                        masterDataService.SaveBillingCustomerID(output, output2, principalID, eventTerminal, eventUser);
                    }

                    string LogResults = results1.Details;
                    if (!String.IsNullOrEmpty(results2.Details))
                    {
                        LogResults += " & " + results2.Details;
                    }

                    WriteToAudit(principalID, "SUCCESS", row, "ROW " + row + " " + LogResults + " IMPORT SUCCESS", CustomerName, FileName, eventUser, transactionType);
                    SuccessStatus++;
                }
            }
            catch (Exception Ex)
            {
                WriteToAudit(principalID, "FAILED", row, Ex.Message, "NOT APPLICABLE", FileName, eventUser, transactionType);
            }

            if (SuccessStatus == row - 1)// return all successful rows
            {
                return SuccessStatus + " Row(s) Uploaded Successfully ".ToUpper() + " And ".ToUpper() + FailsStatus + " Row(s) Failed".ToUpper();
            }

            if (SuccessStatus == 0)// return all failed rows
            {
                return SuccessStatus + " Row(s) Uploaded Successfully ".ToUpper() + " And ".ToUpper() + FailsStatus + " Row(s) Failed".ToUpper();
            }
            else
            {    // Partialy successfully , failed rows and succeeded rows 
                return SuccessStatus + " Row(s) Uploaded Successfully ".ToUpper() + " And ".ToUpper() + FailsStatus + " Row(s) Failed".ToUpper();
            }
        }

    }
}