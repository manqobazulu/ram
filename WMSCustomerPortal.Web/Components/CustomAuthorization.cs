using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using System.Xml.Linq;

namespace WMSCustomerPortal.Web.Components {

    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class CustomAuthorizationAttribute : AuthorizeAttribute {

        //Custom named parameters for annotation
        public string ResourceKey { get; set; }
        public string OperationKey { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext) {
            

            base.OnAuthorization(filterContext);
        }

        //Called when access is denied
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext) {            

            // TEMP solution
            filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Account", action = "Login" })
                //new RouteValueDictionary(new { controller = "Error", action = "SessionExpired" })
                );

            
            //if (!filterContext.HttpContext.User.Identity.IsAuthenticated) {
            //    //User isn't logged in
            //    filterContext.Result = new RedirectToRouteResult(
            //            new RouteValueDictionary(new { controller = "Account", action = "Login" })
            //    );
            //    //base.HandleUnauthorizedRequest(filterContext);
            //}            
            //else {
            //    // User is logged in but has no access
            //    filterContext.Result = new RedirectToRouteResult(
            //            new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" })
            //    );
            //}
        }

        /// <summary>
        /// When overridden, provides an entry point for custom authorization checks.
        /// </summary>
        /// <remarks>Core authentication, called before each action</remarks>
        /// <param name="httpContext">The HTTP context, which encapsulates all HTTP-specific information about an individual HTTP request.</param>
        /// <returns>
        /// true if the user is authorized; otherwise, false.
        /// </returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext) {

            var isUserAuthenticated = false;
            try {

                var model = httpContext.Session.GetDataFromSession<string>("WMSSession.LoggedInUser");

                if (model != null) {
                    //ValidUser user = ValidUser.GetValidUser(model.EmployeeID);
                    ValidUser user = new XmlDataFileReader().GetValidUser(model);
                    if (user != null)
                        isUserAuthenticated = true;
                }
            }
            catch {

            }
            // Returns true or false, meaning allow or deny. False will call HandleUnauthorizedRequest above
            return isUserAuthenticated;
        }
    }

    #region XMLReader

    public class XmlDataFileReader {
        public XmlDataFileReader() {

        }

        public ValidUser GetValidUser(string User) {
            ValidUser validUser = new ValidUser();


            //string dataFile = string.Empty;

            //dataFile = HttpContext.Current.Server.MapPath("~/App_Data/ValidUsers.xml");
            
            //XElement data = XElement.Load(dataFile);

            //if (File.Exists(dataFile)) {
            //    IEnumerable<XElement> records = from d in data.Descendants("dbo.ValidUser")
            //                                    where d.Attribute("Email").Value == User
            //                                    select d;

            //    var count = records.Count();

            //    XElement xe = records.FirstOrDefault();

            //    validUser = new ValidUser( xe.Attribute("Email").Value);

            //}
            return validUser;
        }
    }
    #endregion

    #region ValidUser

    public class ValidUser {
        public string Email { get; set; }

        public ValidUser() {

        }

        public ValidUser(string pEmail) {
            Email = pEmail;
        }        
    } 
    #endregion

    

    public class RequiresAuthorization : AuthorizeAttribute {
        /// <summary>
        /// Processes HTTP requests that fail authorization.
        /// </summary>        
        /// <param name="filterContext">Encapsulates the information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />. The <paramref name="filterContext" /> object contains the controller, HTTP context, request context, action result, and route data.</param>
        /// <remarks>Customizing Authorize attribute</remarks>
        /// <see cref="http://www.prideparrot.com/blog/archive/2012/6/customizing_authorize_attribute"/>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext) {

            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated) {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else {
                //filterContext.Result = new RedirectToRouteResult(
                //    new RouteValueDictionary(
                //        new { 
                //            controller = "Error", 
                //            action = "AccessDenied" 
                //        }));

                filterContext.Result = new ViewResult {
                    ViewName = "~/Views/Shared/AccessDenied.cshtml",
                    TempData = filterContext.Controller.TempData
                };
            }
        }

        /// <summary>
        /// Called when a process requests authorization.
        /// </summary>        
        /// <param name="filterContext">The filter context, which encapsulates information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />.</param>
        /// <remarks>Custom ASP.NET MVC Authorization Attribute For Ajax Requests</remarks>
        /// <see cref="https://gist.github.com/beckelmw/1487699"/>
        public override void OnAuthorization(AuthorizationContext filterContext) {

            base.OnAuthorization(filterContext);

            if (filterContext.Result == null || (filterContext.Result.GetType() != typeof(HttpUnauthorizedResult)
                || !filterContext.HttpContext.Request.IsAjaxRequest()))
                return;

            var redirectToUrl = "/login?returnUrl=" + filterContext.HttpContext.Request.UrlReferrer.PathAndQuery;

            filterContext.Result = filterContext.HttpContext.Request.ContentType == "application/json"
                ? (ActionResult)
                  new JsonResult {
                      Data = new { RedirectTo = redirectToUrl },
                      ContentEncoding = System.Text.Encoding.UTF8,
                      JsonRequestBehavior = JsonRequestBehavior.DenyGet
                  }
                : new ContentResult {
                    Content = redirectToUrl,
                    ContentEncoding = System.Text.Encoding.UTF8,
                    ContentType = "text/html"
                };

            //Important: Cannot set 401 as asp.net intercepts and returns login page
            //so instead set 530 User access denied            
            filterContext.HttpContext.Response.StatusCode = 530; //User Access Denied
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }    
}