using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSCustomerPortal.Models.Entities;
using System.Data;
namespace WMSCustomerPortal.Business
{


    public struct GroupsPrincipalsAssoc
    {
        public bool BelongsTo;
        public int PrincipalID;
        public string PrincipalCode;
        public string PrincipalName;

    }
    
    
    public class PrincipalGroupService
    {
        /// <summary>
        /// Returns a list of all the principals that this user is allowed to access
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<ApplicationGroupPrincipals> GetAllPrincipalsForGroups(List<string> groupIDs  )
        {
            try {  
            List<ApplicationGroupPrincipals> retVal = new List<ApplicationGroupPrincipals>();
            
            foreach(string groupID in groupIDs)
            {
                List<ApplicationGroupPrincipals> princ = new List<ApplicationGroupPrincipals>();

                princ = GetApplicationGroupPrincipalsByGroup(groupID);
                
                foreach(ApplicationGroupPrincipals agp in princ)
                {
                    if(retVal.Contains(agp) == false)
                    {
                        retVal.Add(agp);

                    }
                }
            
            }

            return retVal;
            }
            catch { throw; }
        }


        /// <summary>
        /// Sets the Group principal associations per group
        /// </summary>
        /// <param name="principals"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public static bool SetPrincipalsPerGroup(List<int> principals, string groupID)
        {
            try { 
            bool retVal = false;
            //get a list of all the group principal assocs for this group
           List<ApplicationGroupPrincipals>currentAssocs = new List<ApplicationGroupPrincipals>();
           Dictionary<int,ApplicationGroupPrincipals> dictCurrentAssocs = new Dictionary<int,ApplicationGroupPrincipals>();  //the key is the principla id 
           Dictionary<int,ApplicationGroupPrincipals> dictNewAssocs = new Dictionary<int,ApplicationGroupPrincipals>();  //the key is the principla id 
            
            try
            {

                    currentAssocs = GetApplicationGroupPrincipalsByGroup(groupID);
                    //add this to the dict 
                    foreach(ApplicationGroupPrincipals apgp in currentAssocs)
                    {
                        if (!dictCurrentAssocs.ContainsKey(apgp.PrincipalID))
                        dictCurrentAssocs.Add(apgp.PrincipalID, apgp);
                    }


                //lets also build up a (new) list
                  

                   foreach(int princId in principals)
                   {
                       if(!dictNewAssocs.ContainsKey(princId))
                       {
                           ApplicationGroupPrincipals newapgp = new ApplicationGroupPrincipals();
                           newapgp.GroupID = groupID;
                           newapgp.PrincipalID = princId;

                           dictNewAssocs.Add(princId, newapgp);
                       }

                   }

                //now we can loop through the old list ... if there is no match in the new list ... lets delete the record
                foreach(ApplicationGroupPrincipals compare in dictCurrentAssocs.Values)
                {
                         //whack this record
                         ApplicationGroupPrincipals.Delete(compare.Id);
                }
                
                     //after whacking // lets write the associations
                    int saveVal = 0;
                    foreach(int princId in principals)
                    {
                      saveVal = ApplicationGroupPrincipals.Save(0, groupID, princId);

                    }

                    if (saveVal > 0)
                    {
                        retVal = true; //all is well that ends well
                    }
                    
            }
            catch 
            {
                throw;

            }

            return retVal;

            }
            catch { throw; }
        }

       
        /// <summary>
        /// Returns a list of the groups and principal associations per group
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public static List<GroupsPrincipalsAssoc>  GetGroupsAndPrincipalList(string groupID)
        {
            try { 
            List<GroupsPrincipalsAssoc> assocList = new List<GroupsPrincipalsAssoc>();
            //lets get a list of all the principals
            Principal princService = new Principal();
            List<Principal> allPrincipals = new List<Principal>();
            allPrincipals = princService.ReadAllList();


            List<ApplicationGroupPrincipals> appGroupPrincipalsPerGroup = new List<ApplicationGroupPrincipals>();
            //get the currently associated group principals for this group
            appGroupPrincipalsPerGroup = PrincipalGroupService.GetApplicationGroupPrincipalsByGroup(groupID);
            //lets add this to a dictionary so that we can search more effectively
            Dictionary<int, ApplicationGroupPrincipals> appGrpPrincDict = new Dictionary<int,ApplicationGroupPrincipals>();
            foreach(ApplicationGroupPrincipals apgP in appGroupPrincipalsPerGroup)
            {
                //use the principalID as the key
                if(appGrpPrincDict.ContainsKey(apgP.PrincipalID) == false){
                        appGrpPrincDict.Add(apgP.PrincipalID, apgP);
                }
            }


            foreach(Principal princ in allPrincipals)
            {
               GroupsPrincipalsAssoc assocRecord = new GroupsPrincipalsAssoc();
               
                //if the principal association is found, mark it as selected
               if(appGrpPrincDict.ContainsKey(princ.PrincipalID))
               {
                   //we can set it o selected
                   assocRecord.BelongsTo = true;
               }
               else
               {
                   assocRecord.BelongsTo = false;

               }

               assocRecord.PrincipalCode = princ.PrincipalCode;
               assocRecord.PrincipalID = princ.PrincipalID;
               assocRecord.PrincipalName = princ.PrincipalName;

               assocList.Add(assocRecord);

            }

            return assocList;
            }
            catch { throw; }

        }


        /// <summary>
        /// Updates the ApplicationGroupPrincipals record
        /// </summary>
        /// <param name="recordID"></param>
        /// <param name="groupID"></param>
        /// <param name="principalID"></param>
        /// <returns></returns>
        public static int UpdateApplicationGroupPrincipal(int recordID, string groupID, int principalID)
        {
            try { 
            int retVal = 0;
            retVal = ApplicationGroupPrincipals.Update(recordID, groupID, principalID);
            return retVal;
            }
            catch { throw; }
        }

        /// <summary>
        /// Add a new ApplicationGroupPrincipals record.
        /// </summary>
        /// <param name="recordID">May be 0</param>
        /// <param name="groupID"></param>
        /// <param name="principalID"></param>
        /// <returns></returns>
        public static int AddApplicationGroupPrincipal(int recordID, string groupID, int principalID)
        {
            try { 
            int retVal = 0;
           retVal =  ApplicationGroupPrincipals.Save(recordID, groupID, principalID);
           return retVal;
            }
            catch { throw; }
        }


        /// <summary>
        /// Deletes a ApplicationGroupPrincipals record 
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        public static bool DeleteApplicationGroupPrincipal(int recordID)
        {
            try { 
            bool retValue = false;
            retValue = ApplicationGroupPrincipals.Delete(recordID);
            return retValue;
            }
            catch { throw; }

        }

        /// <summary>
        /// Returns a specific ApplicationGroupPrincipals record
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        public static ApplicationGroupPrincipals GetApplicationGroupPrincipals(int recordID)
        {

            try { 
            ApplicationGroupPrincipals retVal = new ApplicationGroupPrincipals();
            retVal = ApplicationGroupPrincipals.ReadObject(recordID);
            return retVal;
            }
            catch { throw; }

        }


        public static List<ApplicationGroupPrincipals>GetApplicationGroupPrincipalsByGroup(string groupID)
        {
            try { 
            List<ApplicationGroupPrincipals> retVal = new List<ApplicationGroupPrincipals>();

            retVal = ApplicationGroupPrincipals.ReadAll_Group_List(groupID);
            return retVal;
            }
            catch { throw; }
        }

    }
}
