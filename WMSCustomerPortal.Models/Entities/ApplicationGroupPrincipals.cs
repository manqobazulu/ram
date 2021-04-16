using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using RAM.Utilities.Common;

namespace WMSCustomerPortal.Models.Entities
{

     [Serializable]
    public class ApplicationGroupPrincipals
    {

        #region Constants
		
	
		
		#endregion
		
		#region Stored Procedures
		
		private static readonly string STP_SAVE = "usp_ApplicationGroupPrincipals_Save";
		private static readonly string STP_READ = "usp_ApplicationGroupPrincipals_Read";
		private static readonly string STP_READ_ALL = "usp_ApplicationGroupPrincipals_Read_All";
		private static readonly string STP_DELETE = "usp_ApplicationGroupPrincipals_Delete";
        private static readonly string STP_READ_ALL_GROUP = "usp_ApplicationGroupPrincipals_Read_All_Group";
			
		#endregion

		#region Properties
		
		private int mId = 0 ;
		public int Id { get { return mId; } set { mId = value; } } 

		private string mGroupID = "";
		public string GroupID { get { return mGroupID; } set { mGroupID = value; } } 

		private int mPrincipalID = 0;
		public int PrincipalID { get { return mPrincipalID; } set { mPrincipalID = value; } } 

		private string mPrincipalCode = "";
		public string PrincipalCode { get { return mPrincipalCode; } set { mPrincipalCode = value; } } 

		private string mPrincipalName = "";
		public string PrincipalName { get { return mPrincipalName; } set { mPrincipalName = value; } } 

			
		#endregion
		
		#region Constructors
		
		/// <summary>
		/// Default Constructor for vwApplicationGroupPrincipals Class
		/// </summary>
		public ApplicationGroupPrincipals()
		{

		}
		
		/// <summary>
		/// Constructor for vwApplicationGroupPrincipals Class with Initialization
		/// </summary>
		public ApplicationGroupPrincipals(int pId, string pGroupID, int pPrincipalID, string pPrincipalCode, string pPrincipalName)
		{
			Id = pId;
			GroupID = pGroupID;
			PrincipalID = pPrincipalID;
			PrincipalCode = pPrincipalCode;
			PrincipalName = pPrincipalName;
		}

		/// <summary>
		/// Constructor for vwApplicationGroupPrincipals Class with Initialization
		/// </summary>
        public ApplicationGroupPrincipals(DataRow loRow)
		{	
						Id = Functions.SafeInt(loRow["Id"]);
			GroupID = Functions.SafeString(loRow["GroupID"]);
			PrincipalID = Functions.SafeInt(loRow["PrincipalID"], 0);
			PrincipalCode = Functions.SafeString(loRow["PrincipalCode"]);
			PrincipalName = Functions.SafeString(loRow["PrincipalName"]);
		}
		
		#endregion
		
		#region Data Access - Private Methods

        private static List<ApplicationGroupPrincipals> ReturnList(DataTable pdtTable)
		{
            List<ApplicationGroupPrincipals> list = new List<ApplicationGroupPrincipals>();
			try
			{
				foreach (DataRow loRow in pdtTable.Rows)
				{
					list.Add(ReturnObject(loRow));
				}
				return list;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        private static ApplicationGroupPrincipals ReturnObject(DataRow loRow)
		{
            return new ApplicationGroupPrincipals(Functions.SafeInt(loRow["Id"]),
				Functions.SafeString(loRow["GroupID"]),
				Functions.SafeInt(loRow["PrincipalID"], 0),
				Functions.SafeString(loRow["PrincipalCode"]),
				Functions.SafeString(loRow["PrincipalName"]));
		}		
		
		#endregion

		#region Data Access

        public static ApplicationGroupPrincipals ReadObject(int pId)
		{
			DataRow loRow;          
		    try
		    {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
				string sqlCommand = STP_READ;
				DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pId);
				
				loRow = db.ExecuteDataSet(dbCommand).Tables[0].Rows[0];
    				
				if (loRow.Equals(null))
				{
					return null;
				}
				else
				{
				    return ReturnObject(loRow);
				}	
		    }        
		    catch (Exception ex)
		    {
                throw ex;
		    } 	
	    }
	
		public static int Save(int pId, string pGroupID, int pPrincipalID)
		{
			try
			{
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
				string sqlCommand = STP_SAVE;
				DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pId, pGroupID, pPrincipalID);

                db.ExecuteScalar(dbCommand);
                return 1; // (int)db.ExecuteScalar(dbCommand);
			}        
			catch (Exception ex)
			{
				throw ex;
			} 	
		}

		public static int Update(int pId, string pGroupID, int pPrincipalID)
		{
			try
			{
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
				string sqlCommand = STP_SAVE;
				DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pId, pGroupID, pPrincipalID);
				
				return (int)db.ExecuteNonQuery(dbCommand);
			}        
			catch (Exception ex)
			{
				throw ex;
			} 	
		}

		public static bool Delete(int pintID)
		{
			try
			{
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
				string sqlCommand = STP_DELETE;
				DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pintID);
				
				db.ExecuteNonQuery(dbCommand);
				
				return true;
			}        
			catch (Exception ex)
			{
                throw ex;
			} 	
		}

		public static IDataReader Read(int pintID)
        {
			IDataReader dr = null;
			try
		    {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
				string sqlCommand = STP_READ;
				DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pintID);
				
				dr = db.ExecuteReader(dbCommand);

				return dr;				
		    }        
		    catch (Exception ex)
		    {
                throw ex;
		    } 	
		}	
    
		public static IDataReader ReadAll()
		{
		    IDataReader dr = null;
			try
		    {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
				string sqlCommand = STP_READ_ALL;
				DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
				
				dr = db.ExecuteReader(dbCommand);

				return dr;				
		    }        
		    catch (Exception ex)
		    {
                throw ex;
		    }	
		}	
    
		public static DataSet ReadAllDS()
        {
			DataSet dr = null;
			try
		    {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
				string sqlCommand = STP_READ_ALL;
				DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
				
				dr = db.ExecuteDataSet(dbCommand);

				return dr;				
		    }        
		    catch (Exception ex)
		    {
                throw ex;
		    }	
		}

        public static List<ApplicationGroupPrincipals> ReadAll_List()
        {
			try
			{
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
				string sqlCommand = STP_READ_ALL;
				DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
				
				return ReturnList(db.ExecuteDataSet(dbCommand).Tables[0]);
			}        
			catch (Exception ex)
			{
                throw ex;
			} 	
		}


        public static List<ApplicationGroupPrincipals> ReadAll_Group_List(string groupID)
        {
            try
            {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));
                string sqlCommand = STP_READ_ALL_GROUP;
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, groupID);

                return ReturnList(db.ExecuteDataSet(dbCommand).Tables[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		#endregion


    }
}
