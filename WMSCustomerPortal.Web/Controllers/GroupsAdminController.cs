using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;


using WMSCustomerPortal.Models.Entities;
using Microsoft.AspNet.Identity;

using WMSCustomerPortal.Models.Common;
using WMSCustomerPortal.Web.Models;
using WMSCustomerPortal.Business;
using WMSCustomerPortal.Web.Components;




namespace WMSCustomerPortal.Web.Controllers
{
    [Authorize(Roles = "Admin, Backdoor")]
    public class GroupsAdminController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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

       
        WMSCustomerPortal.Business.PrincipalGroupService _service;

        private WMSCustomerPortal.Business.PrincipalGroupService DataService
        {
            get
            {
                if (_service == null)
                {
                    _service = new WMSCustomerPortal.Business.PrincipalGroupService();
                }
                return _service;
            }
        }



        public ActionResult Index()
        {
            return View(this.GroupManager.Groups.ToList());
        }


        /// <summary>
        /// returns adefault view for the groups admin
        /// </summary>
        /// <returns></returns>
        public ActionResult GroupsAdmin()
        {
            return View();

        }

        /// <summary>
        /// returns adefault view for the groups admin
        /// </summary>
        /// <returns></returns>
        public ActionResult PrincipalGroupAdmin()
        {
            return View();

        }

       

        /// <summary>
        /// Returns json list of all the Groups
        /// </summary>
        /// <returns></returns>
        public JsonResult GetGroupsItemList(string filter)
        {

            List<ApplicationGroup> roles = new List<ApplicationGroup>();
            foreach (ApplicationGroup rle in GroupManager.Groups)
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



        struct GroupsRolesAssoc
        {
            public bool BelongsTo;
            public string RoleId;
            public string RoleName;
            public string RoleDescription;

        }

      

            
        /// <summary>
        /// call to save the group details
        /// </summary>
        /// <param name="userAss"></param>
        /// <returns></returns>
        public JsonResult SaveGroupDetails(string groupName, string groupID, string groupDescription)
        {

            //string eventUser = "";
            string eventTerminal = this.TerminalID;
            string eventUser = this.LoggedOnUser;


            ApplicationGroup grp = new Models.ApplicationGroup(); 
            grp = this.GroupManager.FindById(groupID);

            grp.Name = groupName;
            grp.Description = groupDescription;

           

            IdentityResult idRes = new IdentityResult();

            idRes = this.GroupManager.UpdateGroup(grp);
           


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
        /// call to save the group roles associations
       /// </summary>
       /// <param name="userAss"></param>
       /// <returns></returns>
        public JsonResult SaveGroupRoleAssociations(List<string> groupRoles, string groupID)
        {

            //string eventUser = "";
            string eventTerminal = this.TerminalID;
            string eventUser = this.LoggedOnUser;

            //ok ... lets save the group associations for this user
            string[] selectedRoles;
            selectedRoles = new string[groupRoles.Count];
            
            
            
            if (groupRoles == null) // we have NO checkboxes selected
            {
               

            }

            int count = 0;
           
            foreach (string rleID in groupRoles)
            {
               
              ApplicationRole appRole = new Models.ApplicationRole();
              appRole = this.RoleManager.FindById(rleID);

              string roleName = appRole.Name;

              selectedRoles[count] = roleName;
                count++;

            }


            //now set the roles for this group

            IdentityResult idRes = new IdentityResult();
            //get the user id

            idRes = this.GroupManager.SetGroupRoles(groupID, selectedRoles);

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
        /// Saves the groups principals associations
        /// </summary>
        /// <param name="groupPrincipals"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public JsonResult SaveGroupPrincipalAssociations(List<int> groupPrincipals, string groupID)
        {

            //string eventUser = "";
            string eventTerminal = this.TerminalID;
            string eventUser = this.LoggedOnUser;


            bool success = PrincipalGroupService.SetPrincipalsPerGroup(groupPrincipals, groupID);
           

            var result = new BaseSaveResponse
            {
              
                Details = ""
            };


            if (success)
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



        public JsonResult GetGroupsPrincipalsList(string groupID)
        {
            //get a list of all the roles assigned to this group


            List<GroupsPrincipalsAssoc> assocList = new List<GroupsPrincipalsAssoc>();

            assocList = PrincipalGroupService.GetGroupsAndPrincipalList(groupID);
           var data = new 
            {

                iTotalRecords = assocList.Count,
                iTotalDisplayRecords = assocList.Count,
                aaData = assocList
            };

            var response = Json(data, JsonRequestBehavior.AllowGet);

            return response;


        }



        /// <summary>
        /// get a list of all the roles assigned to this group
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public JsonResult GetGroupsRolesList(string groupID)
        {
            //get a list of all the roles assigned to this group


            List<GroupsRolesAssoc> assoc = new List<GroupsRolesAssoc>();

            //lets get the user
            var group = GroupManager.FindById(groupID);

            //lets  get all the roles associated 
            Dictionary<string, ApplicationRole> rolebelongstogroup = new Dictionary<string, ApplicationRole>();
           
            //first lets get the roles that are associated with this group
           
            foreach (ApplicationRole appRole in GroupManager.GetGroupRoles(groupID))
            {
                rolebelongstogroup.Add(appRole.Id, appRole);
            }



            //loop through all the roles
            foreach (ApplicationRole rle in RoleManager.Roles)
            {
                GroupsRolesAssoc rec = new GroupsRolesAssoc();


                if (rolebelongstogroup.ContainsKey(rle.Id))

                //if (grp.ApplicationUsers.Contains(appUserGroup))
                {

                    rec.BelongsTo = true;
                }
                else
                {
                    rec.BelongsTo = false;

                }

                rec.RoleId = rle.Id;
                rec.RoleDescription = rle.Description;
                rec.RoleName = rle.Name;

                //add the users to this collection
                assoc.Add(rec);


            }


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
        /// Call to the delete the group
        /// </summary>
        /// <param name="inboundMaster"></param>
        /// <returns></returns>

        [Authorize(Roles = "Admin")]
        public JsonResult DeleteGroup(string groupID)
        {

            if (groupID == null)
                {
                    
                  var response  = new
                    {
                        SaveResponse = new BaseSaveResponse{ Details = "FAIL"},
                        Result = "FAILURE"
                    };

                  return Json(response, JsonRequestBehavior.AllowGet); ;
                }

            var group = GroupManager.FindById(groupID);
            if (group == null)
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
            var result = GroupManager.DeleteGroup(group.Id);
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
            ApplicationGroup applicationgroup =
                await this.GroupManager.Groups.FirstOrDefaultAsync(g => g.Id == id);
            if (applicationgroup == null)
            {
                return HttpNotFound();
            }
            var groupRoles = this.GroupManager.GetGroupRoles(applicationgroup.Id);
            string[] RoleNames = groupRoles.Select(p => p.Name).ToArray();
            ViewBag.RolesList = RoleNames;
            ViewBag.RolesCount = RoleNames.Count();
            return View(applicationgroup);
        }


        public ActionResult CreateGroup()
        {
            //Get a SelectList of Roles to choose from in the View:
            ViewBag.RolesList = new SelectList(
                this.RoleManager.Roles.ToList(), "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateGroup(
            [Bind(Include = "Name,Description")] ApplicationGroup applicationgroup, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                // Create the new Group:
                var result = await this.GroupManager.CreateGroupAsync(applicationgroup);
                if (result.Succeeded)
                {
                    selectedRoles = selectedRoles ?? new string[] { };

                    // Add the roles selected:
                    await this.GroupManager.SetGroupRolesAsync(applicationgroup.Id, selectedRoles);
                }
                //return RedirectToAction("Index");
                return RedirectToAction("GroupsAdmin");
            }

            // Otherwise, start over:
            ViewBag.RoleId = new SelectList(
                this.RoleManager.Roles.ToList(), "Id", "Name");
            return View(applicationgroup);
        }


        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationGroup applicationgroup = await this.GroupManager.FindByIdAsync(id);
            if (applicationgroup == null)
            {
                return HttpNotFound();
            }

            // Get a list, not a DbSet or queryable:
            var allRoles = await this.RoleManager.Roles.ToListAsync();
            var groupRoles = await this.GroupManager.GetGroupRolesAsync(id);

            var model = new GroupViewModel()
            {
                Id = applicationgroup.Id,
                Name = applicationgroup.Name,
                Description = applicationgroup.Description
            };

            // load the roles/Roles for selection in the form:
            foreach (var Role in allRoles)
            {
                var listItem = new SelectListItem()
                {
                    Text = Role.Name,
                    Value = Role.Id,
                    Selected = groupRoles.Any(g => g.Id == Role.Id)
                };
                model.RolesList.Add(listItem);
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include = "Id,Name,Description")] GroupViewModel model, params string[] selectedRoles)
        {
            var group = await this.GroupManager.FindByIdAsync(model.Id);
            if (group == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                group.Name = model.Name;
                group.Description = model.Description;
                await this.GroupManager.UpdateGroupAsync(group);

                selectedRoles = selectedRoles ?? new string[] { };
                await this.GroupManager.SetGroupRolesAsync(group.Id, selectedRoles);
               // return RedirectToAction("Index");

                //return RedirectToAction("Index");
                return RedirectToAction("GroupsAdmin");
            }
            return View(model);
        }


        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationGroup applicationgroup = await this.GroupManager.FindByIdAsync(id);
            if (applicationgroup == null)
            {
                return HttpNotFound();
            }
            return View(applicationgroup);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationGroup applicationgroup = await this.GroupManager.FindByIdAsync(id);
            await this.GroupManager.DeleteGroupAsync(id);
           // return RedirectToAction("Index");
            //return RedirectToAction("Index");
            return RedirectToAction("GroupsAdmin");
        }






        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}