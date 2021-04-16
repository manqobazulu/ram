using System;
using System.Collections.Generic;
using System.Linq;
using WMSCustomerPortal.Models.Entities;
using WMSCustomerPortal.Models.DataAccess;



namespace WMSCustomerPortal.Models.Common
{
    public class CustomerUpload 
    {

        Customers customerecord = new Customers();
        AuditRecord auditRecord = new AuditRecord();
        SQLDataManager dataservice = new SQLDataManager();

        int NumericalValue; // 
        bool BooleanValue;
        Double DecimalValue;
        int i = 0;
        int row = 1;// files rows start from row 1
        string hubID = "";
        bool isAddingReciever = true;


        public string customerFile(List<string> lines, int principalID, string eventTerminal, string empID,string eventUser, string FileName, string transectionType, string fileDelimiter)
        {
            Customers customerRecord = new Customers();
            AuditRecord auditRecord = new AuditRecord();
            AuditMail auditMail = new AuditMail();


            List<ProductStaging> list = new List<ProductStaging>();
            List<string> auditStatus = new List<string>();

         
            customerRecord.EmployeeID = empID;
            customerRecord.TerminalID = eventTerminal;
            auditRecord.FileName = FileName;
            auditRecord.User = eventUser;
            auditRecord.TransactionType = transectionType;

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
                            objects = lines[i].Split(',');// default delimiter 
                        }
                        else
                        {
                            objects = lines[i].Split(Convert.ToChar(fileDelimiter));// return string
                        }

                        if (objects.Count() != 14)
                        {
                            // this will check if the object got splitted by the delimiter, am using if trying to avoid ArrayOutOfBoundException throw

                            auditRecord.EventStatus = "FAILED";
                            auditRecord.Message = "ROW " + row + " INCORRECT NUMEBER OF FILEDS".ToUpper();

                            auditStatus.Add(auditRecord.Message);
                            auditRecord.Transaction = "Not applicable".ToUpper();
                            Status = "Failed".ToUpper();
                            auditRecord.Save(principalID);

                        }
                        else
                        {
                            if (objects.Count() == 14)
                            {
                                //separating details based on the field on the table
                                if (Convert.ToString(objects[0]).ToUpper() == "H")
                                {
                                    if (objects[1].ToString().Length > 0)
                                    {
                                        // Primary keys
                                        //productRecord.PrincipalID = principalID;    //int

                                        if (objects[1].ToString().Length <= 20)
                                        {
                                            if (objects[1] is string)
                                            {
                                                customerRecord.CustomerID = Convert.ToString(objects[1]).ToUpper(); //varchar
                                                auditRecord.Transaction = objects[2].ToString().ToUpper();

                                            }
                                            else
                                            {
                                                auditRecord.EventStatus = "FAILED";

                                                auditRecord.Message = "ROW " + row + " customer name, INCORRECT DATATYPE CHARACTERISTICS VALUES".ToUpper();
                                                auditStatus.Add(auditRecord.Message);
                                                auditRecord.Transaction = objects[2].ToString().ToUpper();
                                                auditRecord.Save(principalID);

                                            }
                                            if (objects[2].ToString().Length < 20)
                                            {
                                                if (objects[2] is string)
                                                {
                                                    customerRecord.CustomerName= Convert.ToString(objects[2]).ToUpper(); //varchar
                                                    
                                                }
                                                else
                                                {
                                                    auditRecord.EventStatus = "FAILED";
                                                    auditRecord.Message = "ROW " + row + " Default Contact person, INCORRECT DATATYPE CHARACTERISTICS VALUES".ToUpper();
                                                    auditStatus.Add(auditRecord.Message);
                                                    auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                    auditRecord.Save(principalID);

                                                }

                                                if (objects[3].ToString().Length < 20)
                                                {
                                                    if (objects[3] is string)
                                                    {
                                                        customerRecord.DefaultContactPerson = Convert.ToString(objects[3]).ToUpper(); //varchar
                                                    }
                                                    else
                                                    {
                                                        auditRecord.EventStatus = "FAILED";
                                                        auditRecord.Message = "ROW " + row + "CELL number, INCORRECT DATATYPE CHARACTERISTICS VALUES".ToUpper();
                                                        auditStatus.Add(auditRecord.Message);
                                                        auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                        auditRecord.Save(principalID);

                                                    }
                                                    if (objects[4].ToString().Length <= 40)
                                                    {
                                                        if (objects[4] is string)
                                                        {
                                                            customerRecord.CellNo = Convert.ToString(objects[4]).ToUpper(); //varchar
                                                        }
                                                        else
                                                        {
                                                            auditRecord.EventStatus = "FAILED";
                                                            auditRecord.Message = "ROW " + row + " Fax no, INCORRECT DATATYPE CHARACTERISTICS VALUES expected".ToUpper();
                                                            auditStatus.Add(auditRecord.Message);
                                                            auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                            auditRecord.Save(principalID);
                                                        }
                                                        if (objects[5].ToString().Length <= 40)
                                                        {
                                                            if (objects[5] is string)
                                                            {
                                                                customerRecord.TelephoneNo = Convert.ToString(objects[5]).ToUpper(); //bit
                                                            }
                                                        }
                                                        else
                                                        {
                                                            auditRecord.EventStatus = "FAILED";
                                                            auditRecord.Message = "ROW " + row + "Email address, INCORRECT DATATYPE EITHER be TRUE OR FALSE EXPECTED";
                                                            auditStatus.Add(auditRecord.Message);
                                                            auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                            auditRecord.Save(principalID);

                                                            //string UnitCost = objects[6]; //Decimal
                                                        }
                                                      
                                                        if (objects[6].ToString().Length <= 40)
                                                        {
                                                            if (objects[6] is string)
                                                            {
                                                                customerecord.FaxNo = Convert.ToString(objects[6]).ToUpper(); //Decimal
                                                            }
                                                        }
                                                        else
                                                        {
                                                            auditRecord.EventStatus = "FAILED";

                                                            auditRecord.Message = "ROW " + row + " , INCORRECT DATATYPE DECIMAL VALUE EXPECTED";
                                                            auditStatus.Add(auditRecord.Message);
                                                            auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                            auditRecord.Save(principalID);

                                                        }
                                                        
                                                        if (objects[7].ToString().Length <= 40)
                                                        {
                                                            if (objects[7] is string)
                                                            {
                                                                customerRecord.EmailAddress = Convert.ToString(objects[7]).ToUpper(); //Bit
                                                            }
                                                        }
                                                        else
                                                        {
                                                            auditRecord.EventStatus = "FAILED";
                                                            auditRecord.Message = "ROW " + row + "store code, INCORRECT DATATYPE REQUIRES EITHER TRUE OR FALSE EXPECTED".ToUpper();
                                                            auditStatus.Add(auditRecord.Message);
                                                            auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                            auditRecord.Save(principalID);
                                                        }
                                                      
                                                        if (objects[8].ToString().Length <= 40)
                                                        {
                                                            if (objects[8] is string)
                                                            {
                                                                customerRecord.StreetAddress1 = Convert.ToString(objects[8]).ToUpper(); //int
                                                            }
                                                        }
                                                        else
                                                        {
                                                            auditRecord.EventStatus = "FAILED";
                                                            auditRecord.Message = "ROW " + row + " street address1, INCORRECT DATATYPE REQUIRES ONLY NUMERIC VALUE";
                                                            auditStatus.Add(auditRecord.Message);
                                                            auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                            auditRecord.Save(principalID);
                                                        }
                                                        if (objects[9].ToString().Length <= 40)
                                                        {
                                                            if (objects[9] is string)
                                                            {
                                                                customerRecord.StreetAddress2 = Convert.ToString(objects[9]).ToUpper(); //int
                                                            }
                                                        }
                                                        else
                                                        {
                                                            auditRecord.EventStatus = "FAILED";
                                                            auditRecord.Message = "ROW " + row + " street address2, INCORRECT DATATYPE REQUIRES ONLY NUMERIC VALUE";
                                                            auditStatus.Add(auditRecord.Message);
                                                            auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                            auditRecord.Save(principalID);
                                                        }
                                                        if (objects[10].ToString().Length <= 40)
                                                        {
                                                            if (objects[10] is string)
                                                            {
                                                                customerRecord.ZoneID = Convert.ToString(objects[10]).ToUpper(); //int
                                                            }
                                                        }
                                                        else
                                                        {
                                                            auditRecord.EventStatus = "FAILED";
                                                            auditRecord.Message = "ROW " + row + " street address2, INCORRECT DATATYPE REQUIRES ONLY NUMERIC VALUE";
                                                            auditStatus.Add(auditRecord.Message);
                                                            auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                            auditRecord.Save(principalID);
                                                        }
                                                        if (objects[11].ToString().Length <= 40)
                                                        {
                                                            if (objects[11] is string)
                                                            {
                                                                customerRecord.StoreCode = Convert.ToString(objects[11]).ToUpper(); //int
                                                            }
                                                        }
                                                        else
                                                        {
                                                            auditRecord.EventStatus = "FAILED";
                                                            auditRecord.Message = "ROW " + row + " street address2, INCORRECT DATATYPE REQUIRES ONLY NUMERIC VALUE";
                                                            auditStatus.Add(auditRecord.Message);
                                                            auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                            auditRecord.Save(principalID);
                                                        }
                                                        if (objects[12].ToString().Length <= 40)
                                                        {
                                                            if (objects[12] is string)
                                                            {
                                                                customerRecord.isActive = Convert.ToInt32(objects[12]); //int
                                                            }
                                                        }
                                                        else
                                                        {
                                                            auditRecord.EventStatus = "FAILED";
                                                            auditRecord.Message = "ROW " + row + " street address2, INCORRECT DATATYPE REQUIRES ONLY NUMERIC VALUE";
                                                            auditStatus.Add(auditRecord.Message);
                                                            auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                            auditRecord.Save(principalID);
                                                        }
                                                        if (objects[13].ToString().Length <= 40)
                                                        {
                                                            if (objects[13] is string)
                                                            {
                                                                customerRecord.isShipper = Convert.ToInt32(objects[13]); //int
                                                            }
                                                        }
                                                        else
                                                        {
                                                            auditRecord.EventStatus = "FAILED";
                                                            auditRecord.Message = "ROW " + row + " street address2, INCORRECT DATATYPE REQUIRES ONLY NUMERIC VALUE";
                                                            auditStatus.Add(auditRecord.Message);
                                                            auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                            auditRecord.Save(principalID);
                                                        }
                                                        objects = null;
                                                        var results= dataservice.Customer_Save(principalID, customerRecord,"",true);// save data to the IGD staging
                                                        if (results.Details.Substring(0,1) == "E")
                                                        {
                                                            auditRecord.Message = "ROW " + row + results.Details;
                                                            auditRecord.EventStatus = "FAILED";
                                                            auditRecord.Save(principalID);// save to auditRecord
                                                        }
                                                        else
                                                        {
                                                            auditRecord.EventStatus = "SUCCESS";
                                                            if (auditRecord.EventStatus == "SUCCESS")
                                                            {

                                                                SuccessStatus++;

                                                            }
                                                            auditRecord.Message = "ROW " + row + " IMPORT SUCCESS";
                                                            auditStatus.Add(auditRecord.Message +" ROW " + row + " IMPORT SUCCESS");
                                                            Status = "Failed";
                                                            auditRecord.Save(principalID);// save to auditRecord

                                                            // list.Add(new ProductStaging());

                                                            objects = null;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //long desc
                                                        auditRecord.EventStatus = "FAILED";
                                                        auditRecord.Message = "ROW " + row + " Long Descprition, Cannot be than 40 Characters".ToUpper();
                                                        auditStatus.Add(auditRecord.Message);
                                                        auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                        Status = "Failed";
                                                        auditRecord.Save(principalID);// save to auditRecord
                                                    }
                                                }
                                                else
                                                {
                                                    // short desc
                                                    auditRecord.EventStatus = "FAILED";
                                                    auditRecord.Message = "ROW " + row + " Short Descprition,  Cannot have more than 20 Characters  ".ToUpper();
                                                    auditStatus.Add(auditRecord.Message);
                                                    auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                    Status = "Failed";
                                                    auditRecord.Save(principalID);// save to auditRecord
                                                }
                                            }
                                            else
                                            {
                                                //EAN CODE
                                                auditRecord.EventStatus = "FAILED";
                                                auditRecord.Message = "ROW " + row + " EANCode ,Cannot have more than 20 Characters ".ToUpper();
                                                auditStatus.Add(auditRecord.Message);
                                                auditRecord.Transaction = objects[1].ToString().ToUpper();
                                                Status = "Failed";
                                                auditRecord.Save(principalID);// save to auditRecord
                                            }
                                        }
                                        else
                                        {
                                            // prod code
                                            auditRecord.EventStatus = "FAILED";
                                            auditRecord.Message = "ROW " + row + " Product code ,cannot have more than 20 Characters".ToUpper();
                                            auditStatus.Add(auditRecord.Message);
                                            auditRecord.Transaction = objects[1].ToString().ToUpper();
                                            Status = "Failed";
                                            auditRecord.Save(principalID);// save to auditRecord
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
                                    // database calls audit table
                                }
                            }
                            else
                            {
                                // call the database Auidt table
                                auditRecord.EventStatus = "FAILED".ToUpper();
                                auditRecord.Message = "Row ".ToUpper() + row + " Product file should contains 10 fields including h".ToUpper();
                                auditStatus.Add(auditRecord.Message);
                                Status = "Failed";
                                auditRecord.Transaction = "Not Applicable".ToUpper();
                                auditRecord.Save(principalID);
                            }
                        }
                    }

                    //increment
                    i++;
                    row++;
                }
                FailsStatus = (row - SuccessStatus) - 1;
            }
            catch (Exception Ex)
            {
                // Reading error to the database table Audit Table 
                auditRecord.EventStatus = "FAILED";
                ;
                auditRecord.Message = "SERVER ERROR -> Server Error".ToUpper();
                auditStatus.Add(auditRecord.Message);
                Status = "Failed";
                auditRecord.Save(principalID);
                Console.WriteLine("Error  details" + Ex);
            }
            // call the Email send;
            auditMail.Sendmail(auditStatus, eventUser, FileName);


            if (SuccessStatus == row - 1)// return all successful rows
            {
                return SuccessStatus + " Row(s) Uploaded Succefully ".ToUpper() + " And ".ToUpper() + FailsStatus + " Row(s) Failed".ToUpper();
            }
            if (SuccessStatus == 0)// return all failed rows
            {
                --row;
                return SuccessStatus + " Row(s) Uploaded Succefully ".ToUpper() + " And ".ToUpper() + row + " Row(s) Failed".ToUpper();
            }
            else
            {    // Partialy successfully , failed rows and succeeded rows 
                return SuccessStatus + " Row(s) Uploaded Succefully ".ToUpper() + " And ".ToUpper() + FailsStatus + " Row(s) Failed".ToUpper();
            }
        }

    }
}
