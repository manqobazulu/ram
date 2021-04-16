using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSCustomerPortal.Models.Entities;

namespace WMSCustomerPortal.Models.Common
{
  public  class ProductUpload
    {

        public string productFile(List<string> lines, int principalID, string eventTerminal, string eventUser, string FileName, string transectionType, string fileDelimiter)
        {
            ProductStaging productRecord = new ProductStaging();
            AuditRecord auditRecord = new AuditRecord();
            AuditMail auditMail = new AuditMail();
            List<ProductStaging> list = new List<ProductStaging>();
            List<string> auditStatus = new List<string>();

           

            auditRecord.TransactionType = transectionType;
            auditRecord.FileName = FileName;
            auditRecord.User = eventUser;
            int NumericalValue; // 
            bool BooleanValue;
            Double DecimalValue;
            string Status;

            int i = 0;
            int row = 1;// files rows start from row 1
            int SuccessStatus = 0;
            int FailsStatus = 0;

            //remove tablename from list
            lines.RemoveAt(0);
            

            try
            {
                foreach (var obj in lines)
                {

                    Object[] objects;
                    Console.WriteLine()
                        ;
                    if (obj.ToString().Equals(""))// checking for empty row
                    {

                        auditRecord.EventStatus = "Failed".ToUpper();
                        auditRecord.Message = "Row ".ToUpper() + row + " is empty".ToUpper();
                        auditStatus.Add(auditRecord.Message);
                        Status = "Failed"; ;
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
                        }

                        if (objects.Count() != 10)
                        {
                            // this will check if the object got splitted by the delimiter, am using if trying to avoid ArrayOutOfBoundException throw

                            auditRecord.EventStatus = "FAILED";
                            auditRecord.Message = "ROW " + row + " INCORRECT NUMBER OF FIELDS".ToUpper();

                            auditStatus.Add(auditRecord.Message);
                            auditRecord.Transaction = "Not applicable".ToUpper();
                            Status = "Failed".ToUpper();
                            auditRecord.Save(principalID);
                            FailsStatus++;
                            continue;

                        }
                        else
                        {
                            if (objects.Count() == 10)
                            {
                                //separating details based on the field on the table
                                if (Convert.ToString(objects[0]).ToUpper() == "H")
                                {
                                    if (objects[6].ToString().Length > 0)
                                    {
                                        // Primary keys
                                        productRecord.PrincipalID = principalID;    //int

                                        if (objects[1].ToString().Length <= 20)
                                        {
                                            if (objects[1] is string)
                                            {
                                                productRecord.ProdCode = Convert.ToString(objects[1]).ToUpper(); //varchar
                                                auditRecord.Transaction = objects[1].ToString().ToUpper();
                                            }
                                            else
                                            {
                                                auditRecord.EventStatus = "FAILED";

                                                auditRecord.Message = "ROW " + row + " Product Code, INCORRECT DATATYPE CHARACTERISTICS VALUES".ToUpper();
                                                auditStatus.Add(auditRecord.Message);
                                                auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                auditRecord.Save(principalID);
                                                FailsStatus++;
                                                continue;

                                            }
                                            if (objects[2].ToString().Length < 20)
                                            {
                                                if (objects[2] is string)
                                                {
                                                    productRecord.EANCode = Convert.ToString(objects[2]).ToUpper(); //varchar
                                                }
                                                else
                                                {
                                                    auditRecord.EventStatus = "FAILED";
                                                    auditRecord.Message = "ROW " + row + " EAN Code, INCORRECT DATATYPE CHARACTERISTICS VALUES".ToUpper();
                                                    auditStatus.Add(auditRecord.Message);
                                                    auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                    auditRecord.Save(principalID);
                                                    FailsStatus++;
                                                    continue;

                                                }

                                                if (objects[3].ToString().Length <= 20)
                                                {
                                                    if (objects[3] is string)
                                                    {
                                                        productRecord.ShortDesc = Convert.ToString(objects[3]).ToUpper(); //varchar
                                                    }
                                                    else
                                                    {
                                                        auditRecord.EventStatus = "FAILED";
                                                        auditRecord.Message = "ROW " + row + "  Short description, INCORRECT DATATYPE CHARACTERISTICS VALUES".ToUpper();
                                                        auditStatus.Add(auditRecord.Message);
                                                        auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                        auditRecord.Save(principalID);
                                                        FailsStatus++;
                                                        continue;
                                                    }
                                                    if (objects[4].ToString().Length <= 40)
                                                    {
                                                        if (objects[4] is string)
                                                        {
                                                            productRecord.LongDesc = Convert.ToString(objects[4]).ToUpper(); //varchar
                                                        }
                                                        else
                                                        {
                                                            auditRecord.EventStatus = "FAILED";
                                                            auditRecord.Message = "ROW " + row + " Long Description, INCORRECT DATATYPE CHARACTERISTICS VALUES expected".ToUpper();
                                                            auditStatus.Add(auditRecord.Message);
                                                            auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                            auditRecord.Save(principalID);
                                                            FailsStatus++;
                                                            continue;
                                                        }

                                                        bool Serislixedresult = bool.TryParse(objects[5].ToString(), out BooleanValue);
                                                        if (Serislixedresult)
                                                        {
                                                            productRecord.Serialised = Convert.ToBoolean(objects[5]); //bit
                                                        }
                                                        else
                                                        {
                                                            auditRecord.EventStatus = "FAILED";
                                                            auditRecord.Message = "ROW " + row + "Serialised, INCORRECT DATATYPE EITHER be TRUE OR FALSE EXPECTED";
                                                            auditStatus.Add(auditRecord.Message);
                                                            auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                            auditRecord.Save(principalID);
                                                            FailsStatus++;
                                                            continue;

                                                            //string UnitCost = objects[6]; //Decimal
                                                        }
                                                        bool SalesPriceresult = Double.TryParse(objects[6].ToString(), out DecimalValue);
                                                        if (SalesPriceresult)
                                                        {
                                                            productRecord.SalesPrice = Convert.ToDouble(objects[6]); //Decimal
                                                        }
                                                        else
                                                        {
                                                            auditRecord.EventStatus = "FAILED";

                                                            auditRecord.Message = "ROW " + row + " Sales price, INCORRECT DATATYPE DECIMAL VALUE EXPECTED";
                                                            auditStatus.Add(auditRecord.Message);
                                                            auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                            auditRecord.Save(principalID);
                                                            FailsStatus++;
                                                            continue;

                                                        }
                                                        bool Expiryresult = bool.TryParse(objects[7].ToString(), out BooleanValue);
                                                        if (Expiryresult)
                                                        {
                                                            productRecord.ExpiryProduct = Convert.ToBoolean(objects[7]); //Bit
                                                        }
                                                        else
                                                        {
                                                            auditRecord.EventStatus = "FAILED";
                                                            auditRecord.Message = "ROW " + row + " Expiry Product, INCORRECT DATATYPE REQUIRES EITHER TRUE OR FALSE EXPECTED".ToUpper();
                                                            auditStatus.Add(auditRecord.Message);
                                                            auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                            auditRecord.Save(principalID);
                                                            FailsStatus++;
                                                            continue;
                                                        }
                                                        bool LeadTimeDaysresult = Int32.TryParse(objects[8].ToString(), out NumericalValue);// passing string from the object
                                                        if (LeadTimeDaysresult)
                                                        {
                                                            productRecord.LeadTimeDays = Convert.ToInt32(objects[8]); //int
                                                        }
                                                        else
                                                        {
                                                            auditRecord.EventStatus = "FAILED";
                                                            auditRecord.Message = "ROW " + row + " LeadTimeDays, INCORRECT DATATYPE REQUIRES ONLY NUMERIC VALUE";
                                                            auditStatus.Add(auditRecord.Message);
                                                            auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                            auditRecord.Save(principalID);
                                                            FailsStatus++;
                                                            continue;
                                                        }

                                                        bool ProdActiveresult = bool.TryParse(objects[9].ToString(), out BooleanValue);

                                                        if (ProdActiveresult)
                                                        {

                                                            productRecord.ProdActive = Convert.ToBoolean(objects[9]); //bit
                                                        }
                                                        else
                                                        {
                                                            auditRecord.EventStatus = "FAILED";
                                                            auditStatus.Add(auditRecord.Message);
                                                            auditRecord.Message = "ROW " + row + " ProductActive, INCORRECT DATATYPE REQUIRES EITHER be TRUE OR FALSE ";
                                                            auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                            auditRecord.Save(principalID);
                                                            FailsStatus++;
                                                            continue;
                                                        }
                                                        productRecord.EventTerminal = eventTerminal;
                                                        productRecord.EventDate = DateTime.UtcNow.Date;
                                                        productRecord.EventUser = eventUser.ToUpper();
                                                        //productRecord.Deleted = false;
                                                        objects = null;
                                                        productRecord.Save();// save data to the IGD staging
                                                        auditRecord.EventStatus = "SUCCESS";
                                                        if (auditRecord.EventStatus == "SUCCESS")
                                                        {
                                                            auditRecord.Message = "ROW " + row + " IMPORT SUCCESS";
                                                            auditStatus.Add(auditRecord.Message = "ROW " + row + " IMPORT SUCCESS");
                                                            SuccessStatus++;

                                                        }
                                                       
                                                        Status = "Failed";
                                                        auditRecord.Save(principalID);// save to auditRecord

                                                        // list.Add(new ProductStaging());

                                                       // objects = null;
                                                    }
                                                    else
                                                    {
                                                        //long desc
                                                        auditRecord.EventStatus = "FAILED";
                                                        auditRecord.Message = "ROW " + row + " Long Descprition, Long Descprition is too long".ToUpper();
                                                        auditStatus.Add(auditRecord.Message);
                                                        auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                        Status = "Failed";
                                                        auditRecord.Save(principalID);// save to auditRecord
                                                        FailsStatus++;
                                                        continue;
                                                    }
                                                }
                                                else
                                                {
                                                    // short desc
                                                    auditRecord.EventStatus = "FAILED";
                                                    auditRecord.Message = "ROW " + row + " Short Descprition, Short Descprition is too long ".ToUpper();
                                                    auditStatus.Add(auditRecord.Message);
                                                    auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                    Status = "Failed";
                                                    auditRecord.Save(principalID);// save to auditRecord
                                                    FailsStatus++;
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                //EAN CODE
                                                auditRecord.EventStatus = "FAILED";
                                                auditRecord.Message = "ROW " + row + " EANCode ,EANCode is too long".ToUpper();
                                                auditStatus.Add(auditRecord.Message);
                                                auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                Status = "Failed";
                                                auditRecord.Save(principalID);// save to auditRecord
                                                FailsStatus++;
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            // prod code
                                            auditRecord.EventStatus = "FAILED";
                                            auditRecord.Message = "ROW " + row + " Product code ,Product code is too long".ToUpper();
                                            auditStatus.Add(auditRecord.Message);
                                            auditRecord.Transaction = objects[1].ToString().ToUpper();
                                            Status = "Failed";
                                            auditRecord.Save(principalID);// save to auditRecord
                                            FailsStatus++;
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        // cannot be null
                                        auditRecord.EventStatus = "FAILED";
                                        auditRecord.Message = "ROW " + row + " Sales Price, Cannot be NULL ".ToUpper();
                                        auditStatus.Add(auditRecord.Message);
                                        auditRecord.Transaction = objects[1].ToString().ToUpper();
                                        Status = "Failed"; ;
                                        auditRecord.Save(principalID);// save to auditRecord
                                        FailsStatus++;
                                        continue;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("The letter does not exist in the contect of the PRODUCT");
                                    auditRecord.EventStatus = "FAILED".ToUpper();
                                    auditRecord.Message = "Row ".ToUpper() + row + " The first line each record on product file should be a letter h".ToUpper();
                                    auditStatus.Add(auditRecord.Message);
                                    Status = "Failed"; ;
                                    auditRecord.Save(principalID);
                                    FailsStatus++;
                                    continue;
                                    // database calls audit table
                                }
                            }
                            else
                            {
                                // call the database Auidt table
                                auditRecord.EventStatus = "FAILED".ToUpper();
                                auditRecord.Message = "Row ".ToUpper() + row + " INCORRECT NUMBER OF FILEDS".ToUpper();
                                auditStatus.Add(auditRecord.Message);
                                Status = "Failed";
                                auditRecord.Transaction = "Not Applicable".ToUpper();
                                auditRecord.Save(principalID);
                                FailsStatus++;
                                continue;
                            }
                        }
                    }

                    //increment
                    i++;
                    row++;
                }
               // FailsStatus = (row - SuccessStatus) - 1;
            }
            catch (Exception Ex)
            {
                // Reading error to the database table Audit Table 
                auditRecord.EventStatus = "FAILED";
                ;
                auditRecord.Message = ""+Ex;
                auditStatus.Add(auditRecord.Message);
                Status = "Failed";
                auditRecord.Save(principalID);
                Console.WriteLine("Error  details" + Ex);
            }
            // call the Email send;
           // auditMail.Sendmail(auditStatus, eventUser, FileName);


            if (SuccessStatus == row - 1)// return all successful rows
            {
                return SuccessStatus + " Row(s) Uploaded Successfully ".ToUpper() + " And ".ToUpper() + FailsStatus + " Row(s) Failed".ToUpper();
            }
            if (SuccessStatus == 0)// return all failed rows
            {
                --row;
                return SuccessStatus + " Row(s) Uploaded Successfully ".ToUpper() + " And ".ToUpper() + row + " Row(s) Failed".ToUpper();
            }
            else
            {    // Partialy successfully , failed rows and succeeded rows 
                return SuccessStatus + " Row(s) Uploaded Successfully ".ToUpper() + " And ".ToUpper() + FailsStatus + " Row(s) Failed".ToUpper();
            }
        }


    }
}
