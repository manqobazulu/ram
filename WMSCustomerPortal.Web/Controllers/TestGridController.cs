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

using Microsoft.AspNet.Identity;
namespace WMSCustomerPortal.Web.Controllers
{
    public class TestGridController : BaseController
    {

        WMSCustomerPortal.Business.InboundService _service;

        private WMSCustomerPortal.Business.InboundService DataService
        {
            get
            {
                if (_service == null)
                {
                    _service = new WMSCustomerPortal.Business.InboundService();
                }
                return _service;
            }
        }
        
        // GET: TestGrid
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TestGrid()
        {
            return View("TestGrid");

        }

        /// <summary>
        /// Gets the inbound list
        /// </summary>
        /// <param name="param"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public JsonResult GetInboundList(JQueryDataTableParamModel param, string filter)
        {

            int principalID = 2;
            //try
            //{
            //    principalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

            //}
            //catch
            //{
            //    principalID = 0;
            //}


            //TODO: Dates that are to be filtered
            System.DateTime dtFrom = new DateTime();
            System.DateTime dtTo = new DateTime();
            dtFrom = DateTime.Now.AddYears(-2);
            dtTo = DateTime.Now;

            //TODO: Purcase Order Reference
            string poRef = "";

            var sessionData = DataService.GetAllInboundMasterFilter(principalID, poRef, dtFrom, dtTo); //.GetAllCustomers(principalID);

            var data = new
            {
                sEcho = param.sEcho,
                iTotalRecords = sessionData.Count,
                iTotalDisplayRecords = sessionData.Count,
                aaData = sessionData
            };

            // Test serialization using overriden method
            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;
        }






    }
}