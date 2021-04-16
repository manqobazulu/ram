using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMSCustomerPortal.Web.Components;
using WMSCustomerPortal.Business.Models;
using WMSCustomerPortal.Models.Entities;
using WMSCustomerPortal.Models.Common;
using WMSCustomerPortal.Business;
using WMSCustomerPortal.Web.Common;
using OfficeOpenXml;

namespace WMSCustomerPortal.Web.Controllers
{

    [AllowAnonymous]
    public class HomeController : BaseController
    {


        WMSCustomerPortal.Business.MasterDataService _service;
        private WMSCustomerPortal.Business.MasterDataService DataService
        {
            get
            {
                if (_service == null)
                {
                    _service = new WMSCustomerPortal.Business.MasterDataService();
                }
                return _service;
            }
        }

        public ActionResult Index()
        {

            // return RedirectToAction("Index", "Home");
            return RedirectToAction("Login", "Account");
            //  return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Dashboard()
        {
            return RedirectToAction("Dashboard", "Admin");
        }


        [FileTypes("doc,docx,xlsx")]
        public HttpPostedFileBase File { get; set; }

        //for the upload dialog box
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            var lines = new List<string>();
            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            Principal principal = new Principal();
            principal = principal.ReadObject(principalID);
            string FileDelimiter = principal.FileDelimiter;
            ProductStaging productRecord = new ProductStaging();
            SalesOrderUpoad salesOrderUpload = new SalesOrderUpoad();
            CustomerReturnUpload customerReturn = new CustomerReturnUpload();
            AuditRecord auditRecord = new AuditRecord();
            List<ProductStaging> list = new List<ProductStaging>();
            List<string> auditStatus = new List<string>();
            Object[] objects;
            ProductUpload productUpload = new ProductUpload();
            PurchaseOrderUploads POU = new PurchaseOrderUploads();
            customerUpload cs = new customerUpload();
            Customers customerRecord = new Customers();
            AuditMail auditMail = new AuditMail();
            // int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            customerRecord.EmployeeID = this.LoggedOnUserID;
            string empID = customerRecord.EmployeeID;
            customerRecord.TerminalID = this.LoggedOnUser;

            //call precedure to get a delimiter for that principal

            //  Class1 principal = new Class1();

            principal = principal.ReadObject(principalID);
            // string FileDelimiter = principal.FileDelimiter;
            // string FileDelimiter = ",";

            string eventTerminal = this.TerminalID;
            string eventUser = this.LoggedOnUser;
            string ReturnMessage = "";


            string[] fileName = Path.GetFileName(file.FileName).Split('|');


            if (file.FileName.EndsWith("xlsx"))
            {
                var package = new ExcelPackage(file.InputStream);
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                int col = 1;

                string cellValue = "";
               
                for (int row = 1; worksheet.Cells[row, col].Value != null; row++)
                {
                    for (int pcol = 1; worksheet.Cells[row, pcol].Value != null; pcol++)
                    {
                        cellValue += FileDelimiter+worksheet.Cells[row, pcol].Value.ToString();
                        // cellrow=cellValue += FileDelimiter;

                    }
                    // replace the lastest delimeter 
                    lines.Add(cellValue);
                    
                    cellValue ="";
                }

            }
            else
            {

                var reader = new StreamReader(file.InputStream);
                do
                {
                    string textline = reader.ReadLine();


                    lines.Add(textline);

                } while (reader.Peek() != -1);
            }



            if (file.ContentLength > 0)
            {
                //Get the table name
                string name = lines[0].ToUpper().Replace(",","");
                string FileName = fileName[0];
                string TransactionType = name;
                //auditRecord.Transaction = "PRODUCT DETAILS";
                auditRecord.User = eventUser;

                //validation the table/file name
                switch (name)
                {
                    case "PRODUCT":
                        ReturnMessage = productUpload.productFile(lines, principalID, eventTerminal, eventUser, FileName, TransactionType, FileDelimiter);

                        break;
                    case "CUSTOMER":
                        ReturnMessage = cs.customerFile(lines, principalID, eventTerminal, empID, eventUser, FileName, TransactionType, FileDelimiter);
                        break;
                    case "PURCHASE ORDER":
                        ReturnMessage = POU.POImport(lines, principalID, eventTerminal, eventUser, FileName, TransactionType, FileDelimiter);
                        break;
                    case "SALES ORDER":
                        // sales order and lines
                        string PrincipalCode = Session.GetDataFromSession<string>("WMSSession.PrincipalCode");
                        ReturnMessage = salesOrderUpload.SaleOrderUpload(lines, principalID, eventTerminal, eventUser, FileName, TransactionType, FileDelimiter, PrincipalCode);
                        break;
                    case "CUSTOMER RETURN":
                        // customer return
                        ReturnMessage = customerReturn.ReturnsUpload(lines, principalID, eventTerminal, eventUser, FileName, TransactionType, FileDelimiter);
                        break;
                    default:
                        auditRecord.EventStatus = "FAILED";
                        auditRecord.FileName = FileName;
                        auditRecord.Transaction = "Not appicable".ToUpper();
                        auditRecord.TransactionType = "Not Applicable".ToUpper();
                        auditRecord.Message = "0 Row(s) Uploaded Succefully And 0 Row(s) Failed - INVALID FILE DESCRIPTION".ToUpper();
                        ReturnMessage = auditRecord.Message;
                        auditStatus.Add(auditRecord.Message);
                        ReturnMessage = auditRecord.Message;
                        auditMail.Sendmail(auditStatus, eventUser, FileName);
                        auditRecord.Save(principalID);
                        break;

                }
            }
            else
            {
                string FileName = fileName[0];
                auditRecord.EventStatus = "FAILED";
                auditRecord.FileName = FileName;
                auditRecord.Transaction = "Not appicable".ToUpper();
                auditRecord.TransactionType = "Not Applicable".ToUpper();
                auditRecord.User = eventUser;
                auditRecord.Message = "Empty file".ToUpper();
                ReturnMessage = auditRecord.Message;
                auditStatus.Add(auditRecord.Message);
                ReturnMessage = auditRecord.Message;
                auditMail.Sendmail(auditStatus, eventUser, FileName);
                auditRecord.Save(principalID);
            }
        
            



            var tempval = ReturnMessage;
            /// TempData["Failed"] = tempval;

            //   Console.WriteLine(ReturnMessage.Substring(34, 1));

            return RedirectToAction("Dashboard", "Admin", new { id = tempval });

            //return Json(tempval, JsonRequestBehavior.AllowGet);
        }

        private void TransectionType(string name)
        {

        }

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        public ActionResult Audit()
        {
            return View("Audit");
        }

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        public ActionResult Parameters()
        {
            return View("Parameters");
        }

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        public ActionResult EmailNotification()
        {
            return View("EmailNotification");
        }

        public ActionResult GetNotificationList(JQueryDataTableParamModel param, string filter)
        {
            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            List<dynamic> result = DataService.GetEmailNotificationList(principalID);

            var data = new
            {
                sEcho = param.sEcho,
                iTotalRecords = result.Count,
                iTotalDisplayRecords = result.Count,
                aaData = result
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public struct EmailStruct
        {
            public int PrincipalID { get; set; }
            public int EmailNotificationID { get; set; }
            public bool OldSelected { get; set; }
            public bool NewSelected { get; set; }
        }

        public JsonResult UpdateEmailNotifications(List<EmailStruct> EmailNewValues)
        {
            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            foreach (var i in EmailNewValues)
            {
                if(i.OldSelected != i.NewSelected)
                {
                    int result = DataService.UpdateEmailNotification(principalID, i.EmailNotificationID, i.NewSelected, this.LoggedOnUser);
                }
            }

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        public ActionResult DisplayAudit()
        {
            //var searchString = TempData["data"];

            return View("DisplayAudit");
        }

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        public ActionResult DisplayWsAudit()
        {
            //var searchString = TempData["data"];

            return View("DisplayWsAudit");
        }

        public ActionResult Display(JQueryDataTableParamModel param, string filter)
        {
            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            var from_date = TempData["fromDate"] == null ? "" : TempData["fromDate"].ToString();
            var to_date = TempData["to_date"] == null ? "" : TempData["to_date"].ToString();
            var fileName = TempData["fileName"] == null ? "" : TempData["fileName"].ToString();
            var transaction = TempData["transaction"] == null ? "" : TempData["transaction"].ToString();
            var transactionType = TempData["transactionType"] == null ? "" : TempData["transactionType"].ToString();
            var user = TempData["user"] == null ? "" : TempData["user"].ToString();
            var eventStatus = TempData["eventStatus"] == null ? "" : TempData["eventStatus"].ToString();
            var message = TempData["message"] == null? "": TempData["message"].ToString();
            TempData.Keep();

            List<dynamic> result = DataService.readAudits(principalID, from_date, to_date, fileName, transaction, transactionType, user, eventStatus, message);

            var data = new
            {
                sEcho = param.sEcho,
                iTotalRecords = result.Count,
                iTotalDisplayRecords = result.Count,
                aaData = result
            };

            if (result != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("No records", JsonRequestBehavior.AllowGet);
            }


        }

        public ActionResult DisplayWs(JQueryDataTableParamModel param, string filter)
        {
            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            var From_Date = TempData["WsFromDate"].ToString();
            var To_Date = TempData["WsToDate"].ToString();
            var TransactionId = TempData["WsTransactionId"].ToString();
            var TransactionType = TempData["WsTransactionType"].ToString();
            var TrResult = TempData["WsTrResult"].ToString();
            var TrDirection = TempData["WsTrDirection"].ToString();
            TempData.Keep();

            List<dynamic> result = DataService.readWsAudits(principalID, From_Date, To_Date, TransactionId, TransactionType, TrResult, TrDirection);

            var data = new
            {
                sEcho = param.sEcho,
                iTotalRecords = result.Count,
                iTotalDisplayRecords = result.Count,
                aaData = result
            };

            if (result != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("No records", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult getAuditXml(int ID)
        {
            List<dynamic> result = DataService.GetAuditXml(ID);

            var response = new
            {
                Result = result
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult readAudits(string from_date, string to_date, string fileName, string transaction, string transactionType, string user, string eventStatus, string message)
        {
            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            //SAVING VARIABLES FOR DISPLAY IN THE NEXT PAGE
            TempData["fromDate"] = from_date;
            TempData["to_date"] = to_date;
            TempData["fileName"] = fileName;
            TempData["transaction"] = transaction;
            TempData["transactionType"] = transactionType;
            TempData["user"] = user;
            TempData["eventStatus"] = eventStatus;
            TempData["message"] = message;


            List<dynamic> result = DataService.readAudits(principalID ,from_date, to_date, fileName, transaction, transactionType, user, eventStatus, message);

            var response = new
            {
                Result = result
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult readWsAudits(string From_Date, string To_Date, string TransactionId, string TransactionType, string TrResult, string TrDirection)
        {
            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            //SAVING VARIABLES FOR DISPLAY IN THE NEXT PAGE
            TempData["WsFromDate"] = From_Date;
            TempData["WsToDate"] = To_Date;
            TempData["WsTransactionId"] = TransactionId;
            TempData["WsTransactionType"] = TransactionType;
            TempData["WsTrResult"] = TrResult;
            TempData["WsTrDirection"] = TrDirection;

            //List<dynamic> result = DataService.readWsAudits(principalID, From_Date, To_Date, TransactionId, TransactionType, TrResult, TrDirection);

            var response = new
            {
                Result = "OK"
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult readParameters()
        {

            List<dynamic> result = DataService.readParameters();

            var response = new
            {
                Result = result
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult updateParameters(string paramName, string paramDescription, string paramValue)
        {

            int result = DataService.UpdateParameter(paramName, paramDescription, paramValue);

            var response = new
            {
                Result = result
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        private class FileTypesAttribute : Attribute
        {
            private readonly List<string> _types;

            public FileTypesAttribute(string types)
            {
                _types = types.Split(',').ToList();
            }
        }


        //[Authorize]
        //public JsonResult SaveProductUploads()
        //{
        //   // ProductStaging productRecord = new ProductStaging();
        //    //string eventUser = "";
        //    string eventTerminal = this.TerminalID;
        //    string eventUser = this.LoggedOnUser;
        //    int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

        //    try
        //    {
        //        productRecord.PrincipalID = principalID;
        //    }
        //    catch (Exception)
        //    {
        //        productRecord.PrincipalID = 0;
        //    }

        //    productRecord.EventUser = eventUser;
        //    productRecord.EventTerminal = eventTerminal;
        //    if ((productRecord.ProductStagingID < 1))
        //    {

        //        int retVal = productRecord.Save();
        //    }
        //    else
        //    {
        //        int retVal = productRecord.Update();
        //    }

        //    var result = new BaseSaveResponse
        //    {

        //        Details = "OK"
        //    };

        //    var response = new
        //    {
        //        SaveResponse = result,
        //        Result = "SUCCESS"
        //    };

        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}


    }
}