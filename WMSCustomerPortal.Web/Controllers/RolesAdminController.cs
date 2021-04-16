using System.Web.Mvc;
using System.Collections.Generic;



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
using WMSCustomerPortal.Web.Models;
using System.Web.Script.Serialization;
using WMSCustomerPortal.Models.Entities;
using WMSCustomerPortal.Web.Models;
using WMSCustomerPortal.Web.Components;


namespace WMSCustomerPortal.Web.Controllers
{
    [Authorize(Roles = "Backdoor, Admin")]
    public class RolesAdminController : BaseController
    {
        public RolesAdminController()
        {
        }

        public RolesAdminController(ApplicationUserManager userManager,
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
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }


        /// <summary>
        /// returns adefault view for the roles admin
        /// </summary>
        /// <returns></returns>

        [CustomExceptionAttribute]
        [CustomAuthorizationAttribute]

        public ActionResult RolesAdmin()
        {
            return View();

        }

        //
        // GET: /Roles/
        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }


        /// <summary>
        /// Returns json list of all the Users
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRolesItemList(string filter)
        {

            List<ApplicationRole> roles = new List<ApplicationRole>();
            foreach (ApplicationRole rle in RoleManager.Roles)
            {
                roles.Add(rle);

            }

            var data = new
            {

                iTotalRecords = roles.Count,
                iTotalDisplayRecords = roles.Count,
                aaData = roles
            };

            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;
        }



    
        /// <summary>
        /// call to save the group details
        /// </summary>
        /// <param name="userAss"></param>
        /// <returns></returns>
        public JsonResult SaveRoleDetails(string roleName, string roleID, string roleDescription)
        {

            //string eventUser = "";
            string eventTerminal = this.TerminalID;
            string eventUser = this.LoggedOnUser;


            ApplicationRole rle = new Models.ApplicationRole();



            rle = this.RoleManager.FindById(roleID);

            rle.Name = roleName;
            rle.Description = roleDescription;

           

            IdentityResult idRes = new IdentityResult();

            idRes = this.RoleManager.Update(rle); 
           


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
        /// Call to the delete the role
        /// </summary>
        /// <param name="inboundMaster"></param>
        /// <returns></returns>
        public JsonResult DeleteRole(string roleID)
        {

            if (roleID == null)
            {

                var response = new
                {
                    SaveResponse = new BaseSaveResponse { Details = "FAIL" },
                    Result = "FAILURE"
                };

                return Json(response, JsonRequestBehavior.AllowGet); ;
            }

            var role = RoleManager.FindById(roleID);
            if (role == null)
            {
                var response = new
                {
                    SaveResponse = new BaseSaveResponse { Details = "FAIL" },
                    Result = "FAILURE"
                };

                return Json(response, JsonRequestBehavior.AllowGet); ;
            }

            // Remove all the User Group references:
            // this.GroupManager.ClearUserGroups(userID);

            // Then Delete the User:
            var result = RoleManager.Delete(role);
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


        //
        // GET: /Roles/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManager.FindByIdAsync(id);
            // Get the list of Users in this Role
            var users = new List<ApplicationUser>();

            // Get the list of Users in this Role
            foreach (var user in UserManager.Users.ToList())
            {
                if (await UserManager.IsInRoleAsync(user.Id, role.Name))
                {
                    users.Add(user);
                }
            }

            ViewBag.Users = users;
            ViewBag.UserCount = users.Count();
            return View(role);
        }

        //
        // GET: /Roles/Create
        public ActionResult CreateRole()
        {
            return View();
        }

        //
        // POST: /Roles/Create
        [HttpPost]
        public async Task<ActionResult> CreateRole(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = new ApplicationRole(roleViewModel.Name, roleViewModel.Description);
                var roleresult = await RoleManager.CreateAsync(role);
                if (!roleresult.Succeeded)
                {
                    ModelState.AddModelError("", roleresult.Errors.First());
                    return View();
                }

                //return RedirectToAction("Index");
                return RedirectToAction("RolesAdmin");
                //return RedirectToAction("Index");
            }
            return View();
        }

        //
        // GET: /Roles/Edit/Admin
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            RoleViewModel roleModel = new RoleViewModel { Id = role.Id, Name = role.Name };
            return View(roleModel);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Name,Id")] RoleViewModel roleModel)
        {
            if (ModelState.IsValid)
            {
                var role = await RoleManager.FindByIdAsync(roleModel.Id);
                role.Name = roleModel.Name;
                await RoleManager.UpdateAsync(role);
               // return RedirectToAction("Index");

                //return RedirectToAction("Index");
                return RedirectToAction("RolesAdmin");
            }
            return View();
        }

        //
        // GET: /Roles/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        //
        // POST: /Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id, string deleteUser)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var role = await RoleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return HttpNotFound();
                }
                IdentityResult result;
                if (deleteUser != null)
                {
                    result = await RoleManager.DeleteAsync(role);
                }
                else
                {
                    result = await RoleManager.DeleteAsync(role);
                }
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                //return RedirectToAction("Index");
                //return RedirectToAction("Index");
                return RedirectToAction("RolesAdmin");
            }
            return View();
        }
    }
}