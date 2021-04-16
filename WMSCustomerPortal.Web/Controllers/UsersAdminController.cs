
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WMSCustomerPortal.Business;
using WMSCustomerPortal.Business.Models;
using WMSCustomerPortal.Models;
using WMSCustomerPortal.Models.Common;
using WMSCustomerPortal.Web.Components;

using WMSCustomerPortal.Web.Models;

using System.Web.Script.Serialization;

using WMSCustomerPortal.Models.Entities;
using RAM.Utilities.Common;
using System.Text;

namespace WMSCustomerPortal.Web.Controllers
{

    [Authorize(Roles = "Backdoor, Admin")]
    public class UsersAdminController : BaseController
    {
        public UsersAdminController()
        {
        
        }

        public UsersAdminController(ApplicationUserManager userManager,
            ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // Add the Group Manager (NOTE: only access through the public
        // Property, not by the instance variable!)
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

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext()
                    .Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }


        public async Task<ActionResult> Index()
        {
            return View(await UserManager.Users.ToListAsync());
        }


        /// <summary>
        /// return a default view for the users admin
        /// </summary>
        /// <returns></returns>

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]

        public ActionResult UsersAdmin()
        {

            // Show a list of available groups:
            ViewBag.GroupsList =
                new SelectList(this.GroupManager.Groups, "Id", "Name");
            return View("UsersAdmin");
           

        }



       /// <summary>
        /// call to save the user and group associations
       /// </summary>
       /// <param name="userAss"></param>
       /// <returns></returns>
        //public JsonResult SaveUserGroupAssociations(List<string> userGroups, List<string> userID)
        public JsonResult SaveUserGroupAssociations(List<string> userGroups, string userID)
       {
    
            //string eventUser = "";
            string eventTerminal = this.TerminalID;
            string eventUser = this.LoggedOnUser;

            //ok ... lets save the group associations for this user
           string[] selectedGroups = new string[userGroups.Count];
            int count = 0; 
            foreach(string grpID in userGroups)
            {
                selectedGroups[count] = grpID;
                count++;

            }


            //now set the groups for this user

            IdentityResult idRes = new IdentityResult();
            //get the user id

            idRes = this.GroupManager.SetUserGroups(userID, selectedGroups);

           


            var result = new BaseSaveResponse
            {
                //CustomerID = "TEST01",
                //AlternateCustomerID = "TEST0101",
                Details = ""
            };

          
           if (idRes.Succeeded)
            {
                result.Details = "OK";
                var response = new
                {
                    SaveResponse = result,
                    Result = "SUCCESS"
                };

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                result.Details = "FAIL";
                var response = new
                {
                    SaveResponse = result,
                    Result = "FAILURE"
                };

                return Json(response, JsonRequestBehavior.AllowGet);
            }
  
        }

        /// <summary>
        /// call to save the user details
        /// </summary>
        /// <param name="userAss"></param>
        /// <returns></returns>
        public JsonResult SaveUserDetails(string userID, string userEmail)
        {

            //string eventUser = "";
            string eventTerminal = this.TerminalID;
            string eventUser = this.LoggedOnUser;


            ApplicationUser user = new ApplicationUser();
            user = this.UserManager.FindById(userID);


            IdentityResult idRes = new IdentityResult();
            //get the user id
            idRes = this.UserManager.SetEmail(userID, userEmail);
           


            var result = new BaseSaveResponse
            {
               Details = ""
            };


            if (idRes.Succeeded)
            {
                result.Details = "OK";
                var response = new
                {
                    SaveResponse = result,
                    Result = "SUCCESS"
                };

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                result.Details = "FAIL";
                var response = new
                {
                    SaveResponse = result,
                    Result = "FAILURE"
                };

                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }




        /// <summary>
        /// Returns json list of all the Users
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUsersItemList(string filter)
        {

             List<ApplicationUser> users = new List<ApplicationUser>();
             foreach (ApplicationUser usr in UserManager.Users)
             {
                 users.Add(usr);
                 
             }
        
            var data = new
            {
            
                iTotalRecords = users.Count,
                iTotalDisplayRecords = users.Count,
                aaData = users
            };

            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;
        }


        struct UsersGroupsAssoc
        {
            public bool BelongsTo;
            public string GroupId;
            public string GroupName;
            public string GroupDescription;

        }

        /// <summary>
        /// returns the groups the user belongs to
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public JsonResult GetUserGroupsList(string userID)
        {



            List<UsersGroupsAssoc> assoc = new List<UsersGroupsAssoc>();
            
            //lets get the user
             var user = UserManager.FindById(userID);

            //lets  get all the groups the user belongs to....
              // var userbelongstogroups = GroupManager.GetUserGroups(user.Id);

             Dictionary<string, ApplicationGroup> userbelongstogroups = new Dictionary<string, ApplicationGroup>();
           foreach (ApplicationGroup appGrrp in  GroupManager.GetUserGroups(user.Id) )
           {
               
               userbelongstogroups.Add(appGrrp.Id, appGrrp);

           }

      

            //loop through all the groups
            foreach (ApplicationGroup grp in GroupManager.Groups)
            {
                  UsersGroupsAssoc rec = new UsersGroupsAssoc();
               

                if(userbelongstogroups.ContainsKey(grp.Id))

                 //if (grp.ApplicationUsers.Contains(appUserGroup))
                {
                  
                     rec.BelongsTo = true;
                }
                else
                {
                    rec.BelongsTo = false;
             
                }

                rec.GroupId = grp.Id;
                rec.GroupDescription = grp.Description;
                rec.GroupName = grp.Name;
                 
                //add the users to this collection
                assoc.Add(rec);


            }

                //// Update the Groups:
                //selectedGroups = selectedGroups ?? new string[] { };
                //await this.GroupManager.SetUserGroupsAsync(user.Id, selectedGroups);
                //return RedirectToAction("Index");



            var data = new
            {

                iTotalRecords = assoc.Count,
                iTotalDisplayRecords = assoc.Count,
                aaData = assoc
            };

            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;


        }


        /// <summary>
        /// Call to the delete the user
        /// </summary>
        /// <param name="inboundMaster"></param>
        /// <returns></returns>
        public JsonResult DeleteUser(string userID)
        {
            
                if (userID == null)
                {
                    
                  var response  = new
                    {
                        SaveResponse = new BaseSaveResponse{ Details = "FAIL"},
                        Result = "FAILURE"
                    };

                  return Json(response, JsonRequestBehavior.AllowGet); ;
                }

                var user = UserManager.FindById(userID);
                if (user == null)
                {
                    var response = new
                    {
                        SaveResponse = new BaseSaveResponse { Details = "FAIL" },
                        Result = "FAILURE"
                    };

                    return Json(response, JsonRequestBehavior.AllowGet); ;
                }

                // Remove all the User Group references:
                 this.GroupManager.ClearUserGroups(userID);

                // Then Delete the User:
                var result = UserManager.Delete(user);
                if (result.Succeeded)
                {
                    var response = new
                    {
                        SaveResponse = new BaseSaveResponse { Details = "OK" },
                        Result = "SUCCEEDED"
                    };

                    return Json(response, JsonRequestBehavior.AllowGet); ;
                }
                else
                {

                    var response = new
                    {
                        SaveResponse = new BaseSaveResponse { Details = "FAIL" },
                        Result = "FAILURE"
                    };

                    return Json(response, JsonRequestBehavior.AllowGet); ;
                
                }
             
         
        }





        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);

            // Show the groups the user belongs to:
            var userGroups = await this.GroupManager.GetUserGroupsAsync(id);
            ViewBag.GroupNames = userGroups.Select(u => u.Name).ToList();
            return View(user);
        }


        public ActionResult CreateUser()
        {
            // Show a list of available groups:
            ViewBag.GroupsList =
                new SelectList(this.GroupManager.Groups, "Id", "Name");
            return View("CreateUser");
        }


        [HttpPost]
        public async Task<ActionResult> CreateUser(CreateUserViewModel userViewModel,
            params string[] selectedGroups)
        {

            //generate password
            const string valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowercase = "abcdefghijklmnopqrstuvwxyz";
            const string numbers = "1234567890";
            const string chars = "@#$%&*";

            StringBuilder res = new StringBuilder();

            Random rnd = new Random();
            for(int x = 0; x < 2; x++)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
                res.Append(numbers[rnd.Next(numbers.Length)]);
                res.Append(chars[rnd.Next(chars.Length)]);
                res.Append(lowercase[rnd.Next(lowercase.Length)]);
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = userViewModel.Email,
                    Email = userViewModel.Email,
                    PasswordHash = res.ToString(),
                    EmailConfirmed = true

                };

                var adminresult = await UserManager
                  .CreateAsync(user, user.PasswordHash); //we are just faking a password because the user will change password on forgot password link

                SMTPMailer mailer = new SMTPMailer();

                System.Net.Mail.MailAddress fromMail = new System.Net.Mail.MailAddress(ConfigSettings.ReadConfigValue("FromWMSEmail", "wmswebadmin@ram.co.za"));

                mailer.From = fromMail;
                System.Collections.ArrayList toMail = new System.Collections.ArrayList();
                toMail.Add(user.Email);
                mailer.To = toMail;

                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                string addOnEmail = ("&userEmail=" + Server.UrlEncode(user.Email));
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                mailer.Subject = "Reset Password - RAM WMS Management Portal";
                mailer.MessageBody = "Password: " + res.ToString() + "\n \n You can reset your password by clicking " + callbackUrl + addOnEmail + " here.";
                

                //Add User to the selected Groups 
                if (adminresult.Succeeded)
                {
                    if (selectedGroups != null)
                    {
                        mailer.SendMailMessage();
                        selectedGroups = selectedGroups ?? new string[] { };
                        await this.GroupManager
                            .SetUserGroupsAsync(user.Id, selectedGroups);
                    }
                   //return RedirectToAction("Index");   @Html.ActionLink("Back to List", "UsersAdmin", "UsersAdmin")
                    return RedirectToAction("UsersAdmin");
                }
                else
                {
                   
                    ViewBag.Message = "Email " + user.Email + " is already taken.";
                    
                
                }


                    //var getUser = await UserManager.FindByNameAsync(user.Email);
                    //if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                    //{
                        //return View("ForgotPasswordConfirmation");
                   // }            
                                                        
                    //await UserManager.SendEmailAsync(user.Id, "Reset Password - RAM WMS Management Portal", );
                    return RedirectToAction("UsersAdmin");
                
            }

            ViewBag.GroupsList =
                new SelectList(this.GroupManager.Groups, "Id", "Name");
            return View("CreateUser");// reload the create user page
        }


        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            // Display a list of available Groups:
            var allGroups = this.GroupManager.Groups;
            var userGroups = await this.GroupManager.GetUserGroupsAsync(id);

            var model = new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email
            };

            foreach (var group in allGroups)
            {
                var listItem = new SelectListItem()
                {
                    Text = group.Name,
                    Value = group.Id,
                    Selected = userGroups.Any(g => g.Id == group.Id)
                };
                model.GroupsList.Add(listItem);
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include = "Email,Id")] EditUserViewModel editUser,
            params string[] selectedGroups)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                // Update the User:
                user.UserName = editUser.Email;
                user.Email = editUser.Email;
                await this.UserManager.UpdateAsync(user);

                // Update the Groups:
                selectedGroups = selectedGroups ?? new string[] { };
                await this.GroupManager.SetUserGroupsAsync(user.Id, selectedGroups);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }


        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                // Remove all the User Group references:
                await this.GroupManager.ClearUserGroupsAsync(id);

                // Then Delete the User:
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}