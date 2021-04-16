using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace WMSCustomerPortal.Web.Controllers
{
    public class BulkUploadsController : Controller
    {
        // GET: BulkUploads
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadFile(bool comfrim, string file )
        {

            if (comfrim == true)
            {
                if(file == "")
                {

                }

            }


            return View();

        }
    }
}