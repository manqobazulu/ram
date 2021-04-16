using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace WMSWebSandbox.Controllers {
    public class HomeController : BaseController {

        #region Service Instances

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

        WMSCustomerPortal.Business.CommonService _commonservice;
        private WMSCustomerPortal.Business.CommonService CommonDataService {
            get {
                if (_commonservice == null) {
                    _commonservice = new WMSCustomerPortal.Business.CommonService();
                }
                return _commonservice;
            }
        }
        #endregion

        public ActionResult Index() {
            return View();
        }

        public ActionResult Orders2() {

            //Session.SetDataToSession<string>("WMSSession.TempOrder", mOrderLineItems);

            return View();
        }

        #region Orders - Editable Grid

        public ActionResult Orders1() {

            return View();
        }        

        Dictionary<int, SaveOrderLineRequest> mOrderLineItems = null;

        public JsonResult EditableGrid_GetLineItems(SaveOrderLineRequest lineItem) {

            int principalId = 2;

            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveOrderLineRequest>>("WMSSession.TempOrder");

            if (mOrderLineItems == null) {
                mOrderLineItems = new Dictionary<int, SaveOrderLineRequest>();
            }

            List<SaveOrderLineRequest> lineItems = this.ReorderLineItems(mOrderLineItems);

            return Json(new {
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditableGrid_AddLineItem(SaveOrderLineRequest lineItem) {

            int principalId = 3;

            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveOrderLineRequest>>("WMSSession.TempOrder");

            if (mOrderLineItems == null) {
                mOrderLineItems = new Dictionary<int, SaveOrderLineRequest>();
            }

            if (!mOrderLineItems.ContainsKey(lineItem.LineNumber)) {
                mOrderLineItems.Add(lineItem.LineNumber, lineItem);
            }
            else {
                foreach (var item in mOrderLineItems) {
                    SaveOrderLineRequest request = (SaveOrderLineRequest)item.Value;
                    if (request.ProductStagingID == lineItem.ProductStagingID) {
                        return Json(new {
                            Result = "ERROR: Product already added"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            List<SaveOrderLineRequest> lineItems = this.ReorderLineItems(mOrderLineItems);            

            return Json(new { 
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditableGrid_RemoveLineItem(int lineNumber) {

            int principalId = 3;

            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveOrderLineRequest>>("WMSSession.TempOrder");

            if (mOrderLineItems.ContainsKey(lineNumber)) {
                mOrderLineItems.Remove(lineNumber);
            }

            List<SaveOrderLineRequest> lineItems = this.ReorderLineItems(mOrderLineItems);

            return Json(new {
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditableGrid_UpdateLineItem(SaveOrderLineRequest lineItem) {

            int principalId = 3;

            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveOrderLineRequest>>("WMSSession.TempOrder");

            if (mOrderLineItems.ContainsKey(lineItem.LineNumber)) {
                SaveOrderLineRequest request = (SaveOrderLineRequest)mOrderLineItems[lineItem.LineNumber];
                request.Qty = lineItem.Qty;

                mOrderLineItems[lineItem.LineNumber] = lineItem;
            }            

            List<SaveOrderLineRequest> lineItems = this.ReorderLineItems(mOrderLineItems);

            return Json(new {
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditableGrid_SaveLineItems() {

            int principalId = 3;

            mOrderLineItems = Session.GetDataFromSession<Dictionary<int, SaveOrderLineRequest>>("WMSSession.TempOrder");

            if (mOrderLineItems == null || mOrderLineItems.Count == 0) {
                return Json(new {
                    Result = "ERROR: No products have been added"
                }, JsonRequestBehavior.AllowGet);
            }

            // TODO: Save Line Items


            mOrderLineItems.Clear();

            List<SaveOrderLineRequest> lineItems = mOrderLineItems.Values.ToList<SaveOrderLineRequest>();

            // Clear Session
            Session.SetDataToSession<string>("WMSSession.TempOrder", mOrderLineItems);

            return Json(new {
                Result = "OK",
                Data = lineItems
            }, JsonRequestBehavior.AllowGet);
        }        

        private List<SaveOrderLineRequest> ReorderLineItems(Dictionary<int, SaveOrderLineRequest> dictLineItems) {

            Dictionary<int, SaveOrderLineRequest> newDictLineItems = new Dictionary<int, SaveOrderLineRequest>();

            int index = 0;
            foreach (var item in dictLineItems) {
                SaveOrderLineRequest request = (SaveOrderLineRequest)item.Value;
                request.LineNumber = index + 1;

                newDictLineItems.Add(request.LineNumber, request);
                index++;
            }            

            List<SaveOrderLineRequest> lineItems = newDictLineItems.Values.ToList<SaveOrderLineRequest>();

            Session.SetDataToSession<string>("WMSSession.TempOrder", newDictLineItems);

            return lineItems;
        }

        [AllowAnonymous]
        public JsonResult EditableGrid_ProductLookup(string startsWith, string searchType) {

            int principalId = 2; // Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            var data = CommonDataService.Product_Lookup(principalId, startsWith, searchType);

            return Json(data);
        }
        #endregion

        #region ProductMain

        public ActionResult ProductMain() {

            return View();
        }

        public JsonResult GetProductList(string formData) {

            int pPrincipalID = 2; //Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            var sessionData = ProductStaging.ReadAllList(pPrincipalID);

            var data = new {
                iTotalRecords = sessionData.Count,
                iTotalDisplayRecords = sessionData.Count,
                aaData = sessionData
            };

            // Test serialization using overriden method
            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;
        }

        public JsonResult GetProductListWithOption(bool showInactive) {

            int pPrincipalID = 2; //Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            var sessionData = ProductStaging.ReadAllList(pPrincipalID, !showInactive);

            var data = new {
                iTotalRecords = sessionData.Count,
                iTotalDisplayRecords = sessionData.Count,
                aaData = sessionData
            };

            // Test serialization using overriden method
            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;
        } 
        #endregion

        #region PrincipalMain
        public ActionResult PrincipalMain() {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult GetPrincipalList(string filter) {

            var result = DataService.GetAllPrincipals_Dynamic();

            var data = new {
                iTotalRecords = result.Count,
                iTotalDisplayRecords = result.Count,
                aaData = result
            };

            // Test serialization using overriden method
            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;
        } 
        #endregion
    }

    public struct SaveOrderLineRequest {
        public int LineNumber { get; set; }
        public int ProductStagingID { get; set; }
        public string ProdCode { get; set; }
        public string EANCode { get; set; }
        public string ShortDesc { get; set; }
        public string LongDesc { get; set; }
        public double UnitCost { get; set; }
        public double SalesPrice { get; set; }
        public int Qty { get; set; }
    }
}