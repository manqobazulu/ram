using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using WMSCustomerPortal.Web.Models;
using System.Collections;
using System.Collections.Generic;
using RAM.Utilities.Common;

using System.Data.Entity;
using System.Net;
using WMSCustomerPortal.Business;
using WMSCustomerPortal.Business.Models;
using WMSCustomerPortal.Models;
using WMSCustomerPortal.Models.Common;
using WMSCustomerPortal.Web.Components;

//Remove



namespace WMSCustomerPortal.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public AccountController()
        {
        }
        private ApplicationGroupManager _groupManager;
        public ApplicationGroupManager GroupManager
        {
            get
            {
                return _groupManager ?? new ApplicationGroupManager();
            }
            private set
            {
                _groupManager = value;
            }
        }
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
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetCurrentPrincipal()
        {

             int currentprincipalID = Session.GetDataFromSession<int>("WMSSession.PrincipalID");

             string currentPrincipal = "";
            try
            {
                currentPrincipal = MasterDataService.GetPrincipalCodeByID(currentprincipalID);
            }
            catch
            {
                currentPrincipal = "";
            }
            
           

            var data = new
            {
                iTotalRecords = 1,
                iTotalDisplayRecords = 1,
                aaData = currentPrincipal

            };

            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;
        }

        [Authorize] 
        public bool GetPrincipalDetailForUser(string userEmail, out int principalID, out string principalCode, out List<SelectListItem> selectListItems, out bool userLevel)
        {

            bool retVal = false;
            principalID = 0;
            principalCode = "UNDEFINED";
            userLevel = false;

            List<SelectListItem> principalSelectList = new List<SelectListItem>();

            selectListItems = principalSelectList;

            string loggedInUserID = this.LoggedOnUserID;
            
            var user = UserManager.FindById(loggedInUserID);
            int principalId = 0;
            try
            { 
            principalId = Session.GetDataFromSession<int>("WMSSession.PrincipalID");
            }
            catch(Exception xx){
                principalID = 0;
            }

            if (user == null )
            {
                user = UserManager.FindByEmail(userEmail);
                if (user == null)
                {
                    return false;
                }
                
            }

            var userGroups = this.GroupManager.GetUserGroups(user.Id);

            List<string> striungGroups = new System.Collections.Generic.List<string>();
            foreach (ApplicationGroup appGroup in userGroups)
            {
                striungGroups.Add(appGroup.Id);
            }

            PrincipalGroupService pgService = new PrincipalGroupService();
            List<WMSCustomerPortal.Models.Entities.ApplicationGroupPrincipals> grpPrincipals = new List<WMSCustomerPortal.Models.Entities.ApplicationGroupPrincipals>();

            int currentprincipalID = 0;

            grpPrincipals = pgService.GetAllPrincipalsForGroups(striungGroups);

            List<SelectListItem> alRetVal = new List<SelectListItem>();
            Dictionary<int, SelectListItem> dictItems = new Dictionary<int, SelectListItem>();

            foreach (WMSCustomerPortal.Models.Entities.ApplicationGroupPrincipals apgP in grpPrincipals)
            {
                if (dictItems.ContainsKey(apgP.PrincipalID) == false)
                {
                    if (apgP.PrincipalID == currentprincipalID)
                    {
                        dictItems.Add(apgP.PrincipalID, new SelectListItem
                        {
                            Text = apgP.PrincipalCode,
                            Value = apgP.PrincipalID.ToString(),
                            Selected = true
                        });
                    }
                    else
                    {
                        dictItems.Add(apgP.PrincipalID, new SelectListItem
                        {
                            Text = apgP.PrincipalCode,
                           
                           Value = apgP.PrincipalID.ToString(),
                            Selected = false
                        });

                    }
                }
            }

            foreach (SelectListItem itms in dictItems.Values)
            {              
                principalSelectList.Add(itms); 
            }

            bool UserLevel = false;
            UserLevel = DataService.CheckUserLevel(userEmail, Functions.SafeInt(principalSelectList[0].Value));

            if (principalSelectList.Count > 0)
            {

                selectListItems = principalSelectList;
                principalID = Functions.SafeInt(principalSelectList[0].Value, 0);
                principalCode = Functions.SafeString(principalSelectList[0].Text, "UNDEFINED");
                userLevel = UserLevel;

                return true;
            
            }
            else
            {
                return false;

            }
        }
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(WMSCustomerPortal.Web.Models.LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result2 = SignInManager.PasswordSignIn(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result2)
            {
                case SignInStatus.Success:
                 
                   List<SelectListItem> principalList = new List<SelectListItem>();
                   Session.SetDataToSession<string>("WMSSession.LoggedInUser", model.Email);

                   bool getDetails = false;
                   int firstPrincipalID = 0;
                   string firstPrincipalCode = "";
                   bool userLevel = false;
                   getDetails = GetPrincipalDetailForUser(model.Email, out firstPrincipalID, out firstPrincipalCode, out principalList, out userLevel);

                   if (!getDetails)
                   {     
                        return RedirectToAction("Index", "Home");
                   }
                   else
                   {
                       Session.SetDataToSession<List<SelectListItem>>("WMSSession.PrincipalList", principalList);
                       Session.SetDataToSession<int>("WMSSession.PrincipalID", firstPrincipalID);
                       Session.SetDataToSession<string>("WMSSession.PrincipalCode", firstPrincipalCode);
                       Session.SetDataToSession<bool>("WMSSession.UserLevel", userLevel);
                    }

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        Session.SetDataToSession<List<SelectListItem>>("WMSSession.PrincipalList", principalList);
                        Session.SetDataToSession<int>("WMSSession.PrincipalID", firstPrincipalID);
                        Session.SetDataToSession<string>("WMSSession.PrincipalCode", firstPrincipalCode);
                        Session.SetDataToSession<bool>("WMSSession.UserLevel", userLevel);
                        return RedirectToAction("Dashboard", "Admin");
                    }

                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                // if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id))) {return View("ForgotPasswordConfirmation");}
                if (user == null)
                {
                    return View("ForgotPasswordConfirmation");
                }

                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

                string addOnEmail = ("&userEmail=" + Server.UrlEncode(user.Email));

                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password - RAM WMS Management Portal", "Please reset your password by clicking " + callbackUrl + addOnEmail + " here.");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            string emailAddress = "";
                if(Request.QueryString["userEmail"] != null)
                {
                  emailAddress = Server.UrlDecode(Request.QueryString["userEmail"]);
                                   
                }

                ResetPasswordViewModel resetViewModel = new ResetPasswordViewModel();
                resetViewModel.Email = emailAddress;
            try
            {
                if(code == null)
                {
                    return View("Error");
                }
                else
                {
                    return View(resetViewModel);
                }

            }
            catch
            {
                return View("Error"); 
            }       
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        [AllowAnonymous]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            Session.SetDataToSession<int>("WMSSession.PrincipalID", 0);  
             return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}