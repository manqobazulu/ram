using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;


namespace WMSCustomerPortal.Web.Components {
    public class SessionExpire : ActionFilterAttribute
    {
        private bool _timeoutFlag;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            // The following code is used for checking if a session has timed out. The default timeout value for ASP.NET is 20mins.
            // The timeout value can be overriden in the Web.config file using the sessionState tag's timeout attribute
            // <sessionState timeout="5"></sessionState>
            // Check for an existing session
            if (null != filterContext.HttpContext.Session)
            {
                // Check if we have a new session
                //if (filterContext.HttpContext.Session.IsNewSession)
                //{
                    string cookie = filterContext.HttpContext.Request.Headers["Cookie"];
                    // Check if session has timed out
                    if ((null != cookie) && (cookie.IndexOf("ASP.NET_SessionId", StringComparison.Ordinal) == -1))
                    {
                        _timeoutFlag = true;
                        // Logout the user
                        FormsAuthentication.SignOut();
                        //redirect to login
                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary(new { controller = "Account", action = "Login" }));
                        return;
                    }
                //}
            }
            // else continue with action as usual
            base.OnActionExecuting(filterContext);
        }
        
    }  
}