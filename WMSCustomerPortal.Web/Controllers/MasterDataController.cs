using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using WMSCustomerPortal.Business;
using WMSCustomerPortal.Business.Models;
using WMSCustomerPortal.Models;
using WMSCustomerPortal.Models.Common;
using WMSCustomerPortal.Models.Entities;
using WMSCustomerPortal.Web.Components;

namespace WMSCustomerPortal.Web.Controllers {

    public class MasterDataController : BaseController {

        WMSCustomerPortal.Business.MasterDataService _service;
        private WMSCustomerPortal.Business.MasterDataService DataService {
            get {
                if (_service == null) {
                    _service = new WMSCustomerPortal.Business.MasterDataService();
                }
                return _service;
            }
        }

        WMSCustomerPortal.Models.Entities.ProductStaging _productStaging;
        private WMSCustomerPortal.Models.Entities.ProductStaging ProductStaging {
            get {
                if (_productStaging == null) {
                    _productStaging = new WMSCustomerPortal.Models.Entities.ProductStaging();
                }
                return _productStaging;
            }
        }

        WMSCustomerPortal.Models.DataAccess.SQLDataManager _sqlManager;
        private WMSCustomerPortal.Models.DataAccess.SQLDataManager SqlManager {
            get {
                if (_sqlManager == null) {
                    _sqlManager = new WMSCustomerPortal.Models.DataAccess.SQLDataManager();
                }
                return _sqlManager;
            }
        }

        #region Customers

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]

        //[Authorize(Roles = "Backdoor, MasterData")]

        public ActionResult CustomerMain(int? id)
        {

            if (id.Equals(null))
            {
                try
                {
                    id = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
                }
                catch
                {
                    id = 1;
                }
            }

            var model = new CustomerMainModel
            {
                PrincipalID = id
            };

            return View(model);
        }

        public JsonResult GetCustomerList(JQueryDataTableParamModel param, string formData) {

            JavaScriptSerializer js = new JavaScriptSerializer(new SimpleTypeResolver());
            SearchModel searchModel = js.Deserialize<SearchModel>(formData);
            ShowActive showActive = js.Deserialize<ShowActive>(formData);

            var sessionData = DataService.GetAllCustomers(searchModel.PrincipalID, showActive.isActive);

            var data = new {
                sEcho = param.sEcho,
                iTotalRecords = sessionData.Count,
                iTotalDisplayRecords = sessionData.Count,
                aaData = sessionData
            };

            // Test serialization using overriden method
            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;
        }

        //[Authorize(Roles = "Backdoor, MasterData")]
        public JsonResult SaveCustomer(Customers customer) {
            try {
                // Get PrincipalID
                int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

                customer.EmployeeID = this.LoggedOnUserID;
                customer.TerminalID = this.TerminalID; 

                var result = DataService.CreateNewCustomer(principalId, customer, "", true);

                var response = new {
                    SaveResponse = result,
                    Result = result.Details
                };

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                return Json(new {
                    Result = string.Format("ERROR: {0}", ex.Message)
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveBillingCustomerID(string pCustomerID, string pBillingCustomerID) {

            string pEventTerminal = this.TerminalID;
            string pEventUser = this.LoggedOnUser;
            int pPrincipalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            var result = DataService.SaveBillingCustomerID(pCustomerID, pBillingCustomerID, pPrincipalID, pEventTerminal, pEventUser);

            return Json(new {
                Result = "OK",
                Data = result,
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Customer_Lookup_by_ReceiverID(string startsWith)
        {

            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            var data = SqlManager.Customer_Lookup_by_ReceiverID(startsWith, principalId);

            return Json(data);
        }


        #endregion


        #region Principals

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]
        public ActionResult PrincipalMain() {

            return View();
        }

        public ActionResult GetPrincipalList(JQueryDataTableParamModel param, string filter) {

            var result = DataService.GetAllPrincipals_Dynamic();

            var data = new {
                sEcho = param.sEcho,
                iTotalRecords = result.Count,
                iTotalDisplayRecords = result.Count,
                aaData = result
            };

            // Test serialization using overriden method
            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;
        }

        public JsonResult DoesPrincipalExist(string principalCode) {

            bool result = DataService.DoesPrincipalExist(principalCode);

            var response = new {
                Result = result
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DoesPrefixExist(string principalPrefix) {

            bool result = DataService.DoesPrefixExist(principalPrefix);

            var response = new {
                Result = result
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //[Authorize(Roles = "Backdoor, MasterData")]
        public JsonResult SavePrincipal(string principalCode, string principalName, string customerGroupId, string principalPrefix, string filedelimiter, bool WebServiceIntegration) {

            var result = DataService.AddPrincipal(new Principal {
                PrincipalCode = principalCode,
                PrincipalName = principalName,
                CustomerGroupID = customerGroupId,
                Prefix = principalPrefix,
                FileDelimiter = filedelimiter,
                WebServiceIntegration = WebServiceIntegration
            },LoggedOnUserID, TerminalID);

            var response = new {
                PrincipalID = result,
                Result = "SUCCESS"
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdatePrincipal(int principalID, string principalCode, string principalName, string customerGroupId, string principalPrefix, string filedelimiter, bool WebServiceIntegration)
        {

            var result = DataService.UpdatePrincipal(new Principal
            {
                PrincipalID = principalID,
                PrincipalCode = principalCode,
                PrincipalName = principalName,
                CustomerGroupID = customerGroupId,
                Prefix = principalPrefix,
                FileDelimiter = filedelimiter,
                WebServiceIntegration = WebServiceIntegration
            }, LoggedOnUserID, TerminalID);

            var response = new
            {
                PrincipalID = result,
                Result = "SUCCESS"
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Linked BilledTo

        public JsonResult GetLinkedBilledTo_by_PrincipalID(int principalId) {

            var result = DataService.GetLinkedBilledTo_by_PrincipalID(principalId);

            var data = new {
                Result = "SUCCESS",
                RecordCount = result.Count,
                Data = result
            };

            // Test serialization using overriden method
            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;
        }

        public JsonResult SaveLinkedBilledTo(int principalId, string billedTo) {

            var result = DataService.SaveLinkedBilledTo(principalId, billedTo, "LoggedOnUserID", TerminalID);

            var response = new {
                RecordID = result,
                Result = "SUCCESS"
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveLinkedReceiver(string UserName, string ReceiverID, bool ReceiverFlag)
        {
            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            string EventTerminal = this.TerminalID;
            string EventUser = this.LoggedOnUser;

            var result = DataService.SaveLinkedReceiver(principalId, ReceiverID, UserName, EventUser, EventTerminal, ReceiverFlag);

            if ((result > 0))
            {
               Session.SetDataToSession<bool>("WMSSession.UserLevel", true);
            }

            var response = new
            {
                RecordID = result,
                Result = "SUCCESS"
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveLR(string UserName, string ReceiverID, bool ReceiverFlag)
        {
            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            string EventTerminal = this.TerminalID;
            string EventUser = this.LoggedOnUser;

            var result = DataService.SaveLinkedReceiver(principalId, ReceiverID, UserName, EventUser, EventTerminal, ReceiverFlag);

            if ((result > 0))
            {
                Session.SetDataToSession<bool>("WMSSession.UserLevel", true);
            }

            var response = new
            {
                RecordID = result,
                Result = "SUCCESS"
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteAddNewCustomerAccess(string UserName)
        {
            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            string EventTerminal = this.TerminalID;
            string EventUser = this.LoggedOnUser;

            var result = DataService.DeleteAddNewCustomerAccess(principalId, UserName, EventUser, EventTerminal);

            var response = new
            {
                Result = result
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveAddNewCustomerAccess(string UserName)
        {

            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            string EventTerminal = this.TerminalID;
            string EventUser = this.LoggedOnUser;

            var result = DataService.SaveAddNewCustomerAccess(principalId, UserName, EventUser, EventTerminal);

            var response = new
            {
                RecordID = result,
                Result = "SUCCESS"
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        struct ReceiversOptions
        {
            public string UserName;
        }

        public JsonResult GetReceiverList(string UserName)
        {

            //JavaScriptSerializer js = new JavaScriptSerializer();
            //var UserName = js.Deserialize<ReceiversOptions>(formData);

            int PrincipalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            var sessionData = DataService.GetReceivers(PrincipalID, UserName);

            var data = new
            {
                iTotalRecords = sessionData.Count,
                iTotalDisplayRecords = sessionData.Count,
                aaData = sessionData
            };

            // Test serialization using overriden method
            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;
        }

        public JsonResult GetAddNewCustomerAcess(string UserName)
        {

            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            var result = DataService.GetAddNewCustomerAcess(principalId, UserName.Trim());

            var data = new
            {
                Result = "SUCCESS",
                RecordCount = result.Count,
                Data = result
            };

            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;

        }

        public JsonResult CheckAllReceiversAccess(string UserName) {

            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            var result = DataService.CheckAllReceiversAccess(principalId, UserName.Trim());

            var data = new {
                Result = "SUCCESS",
                RecordCount = result.Count,
                Data = result
            };

            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;

        }

        public JsonResult DeleteLinkedReceiver(int pRecordID)
        {
            string pEventTerminal = this.TerminalID;
            string pEventUser = this.LoggedOnUser;
            int pPrincipalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            var result = DataService.DeleteLinkedReceiver(pPrincipalID, pRecordID, pEventTerminal, pEventUser);
            var response = new
            {
                Result = result
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Products

        [Authorize]

        [CustomExceptionAttribute]
        //[CustomAuthorizationAttribute]

        public ActionResult ProductMain() {
            return View();
        }

        struct ProdListOptions {
            public bool showInactive;
        }

        [Authorize]
        public JsonResult GetProductListWithOption(JQueryDataTableParamModel param, string formData) {

            JavaScriptSerializer js = new JavaScriptSerializer();
            var inactiveFlag = js.Deserialize<ProdListOptions>(formData);

            int pPrincipalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            var sessionData = ProductStaging.ReadAllList(pPrincipalID, inactiveFlag.showInactive); //return the opposite ... we only want active

            // var sessionData = ProductStaging.ReadAllList(pPrincipalID);

            var data = new {
                sEcho = param.sEcho,
                iTotalRecords = sessionData.Count,
                iTotalDisplayRecords = sessionData.Count,
                aaData = sessionData
            };

            // Test serialization using overriden method
            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;
        }

        [Authorize]
        public JsonResult GetProductList(JQueryDataTableParamModel param, string formData) {

            int pPrincipalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            var sessionData = ProductStaging.ReadAllList(pPrincipalID);

            var data = new {
                sEcho = param.sEcho,
                iTotalRecords = sessionData.Count,
                iTotalDisplayRecords = sessionData.Count,
                aaData = sessionData
            };

            // Test serialization using overriden method
            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;
        }

        /// <summary>
        /// Call to the save the Product
        /// </summary>
        /// <param name="productStaging"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult SaveProduct(ProductStaging productRecord) {
            //string eventUser = "";
            string eventTerminal = this.TerminalID;
            string eventUser = this.LoggedOnUser;
            int principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            try {
                productRecord.PrincipalID = principalID;
            }
            catch (Exception) {
                productRecord.PrincipalID = 0;
            }

            productRecord.EventUser = eventUser;
            productRecord.EventTerminal = eventTerminal;
            if ((productRecord.ProductStagingID < 1)) {

                //int retVal = productRecord.Save();
                productRecord.Save();
            }
            else {
                int retVal = productRecord.Update();
            }

            var result = new BaseSaveResponse {

                Details = "OK"
            };

            var response = new {
                SaveResponse = result,
                Result = "SUCCESS"
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lookup for the product code - useful in autocomplete situations
        /// </summary>
        /// <param name="startsWith"></param>
        /// <returns></returns>
        public JsonResult ProductCode_Lookup(string startsWith) {

            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            var data = SqlManager.ProductCode_Lookup(startsWith, principalId);
            var response = Json(data, JsonRequestBehavior.AllowGet);
            return response;
        }

        /// <summary>
        /// Checks whether a product code exists
        /// </summary>
        /// <param name="prodCode"></param>
        /// <returns></returns>
        public JsonResult IsProductCodeDuplicate(string txtProductCode) {
            //get the logged in principal id

            bool retVal = false;
            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            bool doesExist = DataService.DoesProductExist(txtProductCode, principalId);

            if (doesExist) {
                retVal = false;
            }
            else {
                retVal = true;
            }
            var response = Json(retVal, JsonRequestBehavior.AllowGet);
            return response;
        }

        
             /// <summary>
        /// Checks whether a ean code exists
        /// </summary>
        /// <param name="prodCode"></param>
        /// <returns></returns>
        public JsonResult IsEANCodeDuplicate(string txtEANCode)
        {
            //get the logged in principal id

            bool retVal = false;
            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            bool doesExist = DataService.DoesEANExist(txtEANCode, principalId);

            if (doesExist) {
                retVal = false;
            }
            else {
                retVal = true;
            }
            var response = Json(retVal, JsonRequestBehavior.AllowGet);
            return response;
        }

        public JsonResult UpdateProduct(int pProductID, string pProductCode, string pEANCode, string pShortDescription, string pLongDescription, bool pSerialised, bool pExpiryProduct, double pSalesPrice, bool pProdActive, int pLeadTimeDays) {

            string pEventTerminal = this.TerminalID;
            string pEventUser = this.LoggedOnUser;
            int pPrincipalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            var result = DataService.UpdateProductStaging(new ProductStaging { ProductStagingID = pProductID, PrincipalID = pPrincipalID, ProdCode = pProductCode, EANCode = pEANCode, ShortDesc = pShortDescription, LongDesc = pLongDescription, Serialised = pSerialised, SalesPrice = pSalesPrice, ExpiryProduct = pExpiryProduct, LeadTimeDays = pLeadTimeDays, ProdActive = pProdActive,EventTerminal = pEventTerminal, EventUser = pEventUser },
            pPrincipalID, pEventTerminal, pEventUser);

            var response = new {
                ProductID = result,
                Result = "SUCCESS"
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Lookup product code - only returns the product code - temp use for handsontable 
        /// </summary>
        /// <param name="startsWith"></param>
        /// <returns></returns>
        public JsonResult ProductCode_LookupList(string startsWith) {

            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            // var data = SqlManager.ProductCode_Lookup(startsWith, principalId);
            List<WMSCustomerPortal.Models.Common.ProductLookUpItem> data = SqlManager.ProductCode_Lookup(startsWith, principalId);

            List<string> prodCodeList = new List<string>();
            foreach (WMSCustomerPortal.Models.Common.ProductLookUpItem itm in data) {
                string displayVal = itm.ProductCode.ToString(); // +itm.ProductStagingID.ToString();
                prodCodeList.Add(displayVal);//+ "" + itm.ProductStagingID.ToString());
            }

            var response = Json(prodCodeList, JsonRequestBehavior.AllowGet);
            // var response = Json(data, JsonRequestBehavior.AllowGet);
            return response;
        }

        /// <summary>
        /// Lookup product code - only returns the product code - temp use for handsontable (lookup editor)
        /// </summary>
        /// <param name="startsWith"></param>
        /// <returns></returns>
        public JsonResult ProductCode_LookupList_XX(string startsWith) {

            int principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            // var data = SqlManager.ProductCode_Lookup(startsWith, principalId);
            List<WMSCustomerPortal.Models.Common.ProductLookUpItem> data = SqlManager.ProductCode_Lookup(startsWith, principalId);

            List<CodeList> prodCodeList = new List<CodeList>();
            foreach (WMSCustomerPortal.Models.Common.ProductLookUpItem itm in data) {
                CodeList xxx = new CodeList();
                xxx.code = itm.ProductStagingID.ToString();
                xxx.label = itm.ProductCode.ToString();

                prodCodeList.Add(xxx);//+ "" + itm.ProductStagingID.ToString());
            }

            var response = Json(prodCodeList, JsonRequestBehavior.AllowGet);
            // var response = Json(data, JsonRequestBehavior.AllowGet);
            return response;
        }
        #endregion
    }

    public struct CodeList {
        public string code;
        public string label;
    }

    //public class CustomerMainModel : ViewModelBase {
    //    [HiddenInput(DisplayValue = false)]
    //    public int? PrincipalID { get; set; }
    //}
}
