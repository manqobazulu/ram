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

namespace WMSCustomerPortal.Web.Controllers
{
//     [Authorize(Roles = "Backdoor, MasterData")]
//    public class ProductsController : BaseController
//    {

//        //The products resides in the masterdata
//        WMSCustomerPortal.Business.MasterDataService _service;

//        private WMSCustomerPortal.Business.MasterDataService DataService
//        {
//            get
//            {
//                if (_service == null)
//                {
//                    _service = new WMSCustomerPortal.Business.MasterDataService();
//                }
//                return _service;
//            }
//        }


      

//        /// <summary>
//        /// Returns alist of all the products for the warehouse 
//        /// </summary>
//        /// <param name="param"></param>
//        /// <param name="filter"></param>
//        /// <returns></returns>
//        [Authorize]
//         public JsonResult GetProductsList(JQueryDataTableParamModel param, string filter)
//        {

//            //TODO: Get PrincipalID
//            int principalID = 0;
//            try
//            {
//                principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

//            }
//            catch
//            {
//                principalID = 0;
//            }

//            var sessionData = DataService.SearchProductsList("", "", "", "", principalID);  


//            var data = new
//            {
//                sEcho = param.sEcho,
//                iTotalRecords = sessionData.Count,
//                iTotalDisplayRecords = sessionData.Count,
//                aaData = sessionData
//            };

//            // Test serialization using overriden method
//            var response = Json(data, JsonRequestBehavior.AllowGet);

//            return response;
//        }

        
//    }
}