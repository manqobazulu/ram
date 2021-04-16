using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WMSCustomerPortal.Web.Components {
    public class CustomExceptionAttribute : FilterAttribute, IExceptionFilter {
        public void OnException(ExceptionContext filterContext) {

            filterContext.ExceptionHandled = true;

            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];
            var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

            // Logging
            DefaultLogger.LogEvent(string.Format("{0}Controller", controllerName),
                    message: filterContext.Exception.Message,
                    level: RAM.Logging.LogLevel.ERROR,
                    ex: filterContext.Exception,
                    context: actionName);

            filterContext.Result = new ViewResult {
                ViewName = "~/Views/Shared/Error.cshtml",
                //MasterName = "~/Views/Shared/_Layout.cshtml",
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                TempData = filterContext.Controller.TempData
            };

        }
    }
}