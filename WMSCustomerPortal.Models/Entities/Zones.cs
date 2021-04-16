using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using System.Configuration;

using RAM.Utilities.Common;

namespace WMSCustomerPortal.Models.Entities
{
	/// <summary>
	/// Zones Class
	/// </summary>
	public class Zones
	{
		#region Constants
		
				public static readonly string FLD_ZONEID = "ZoneID";
		public static readonly string FLD_SUBURB = "Suburb";
		public static readonly string FLD_POSTALCODE = "PostalCode";
		public static readonly string FLD_AREA = "Area";
		public static readonly string FLD_MAINZONE = "MainZone";
		public static readonly string FLD_SUBZONE = "SubZone";
		public static readonly string FLD_ROUTEID = "RouteID";
		public static readonly string FLD_HUBID = "HubID";
		public static readonly string FLD_LOCALAREA = "LocalArea";
		public static readonly string FLD_MAINAREA = "MainArea";
		public static readonly string FLD_REGIONALAREA = "RegionalArea";
		public static readonly string FLD_INTERNATIONAL = "International";
		public static readonly string FLD_ISACTIVE = "isActive";
		public static readonly string FLD_EVENTDATETIME = "EventDateTime";
		public static readonly string FLD_EMPLOYEEID = "EmployeeID";
		public static readonly string FLD_TERMINALID = "TerminalID";
		public static readonly string FLD_LASTUPDATE = "LastUpdate";
		public static readonly string FLD_ISDIRTY = "isDirty";
		
		public static readonly string FLD_NEWZONEID = "NewZoneID";
		public static readonly string FLD_LAT = "Lat";
		public static readonly string FLD_LON = "Lon";
		public static readonly string FLD_RECORDCREATEDDT = "RecordCreatedDT";
		public static readonly string FLD_UPDATEDT = "UpdateDT";
		public static readonly string FLD_UPDATEDBY = "UpdatedBy";
		public static readonly string FLD_UPDATETERMINALID = "UpdateTerminalID";
	
		
		#endregion
		
		#region Stored Procedures
		
		private static readonly string STP_SAVE = "usp_Zones_Save";
		private static readonly string STP_READ = "usp_Zones_Read";
		private static readonly string STP_READ_ALL = "usp_Zones_Read_All";
		private static readonly string STP_DELETE = "usp_Zones_Delete";
			
		#endregion

		#region Properties
		
				private string mZoneID = "";
                public string ZoneID { get { return mZoneID.ToUpper(); } set { mZoneID = value.ToUpper(); } } 

		private string mSuburb = "";
        public string Suburb { get { return mSuburb.ToUpper(); } set { mSuburb = value.ToUpper(); } } 

		private string mPostalCode = "";
        public string PostalCode { get { return mPostalCode.ToUpper(); } set { mPostalCode = value.ToUpper(); } } 

		private string mArea = "";
        public string Area { get { return mArea.ToUpper(); } set { mArea = value.ToUpper(); } } 

		private double? mMainZone = 0;
        public double? MainZone { get { return mMainZone; } set { mMainZone = value; } } 

		private double? mSubZone = 0;
        public double? SubZone { get { return mSubZone; } set { mSubZone = value; } } 

		private int? mRouteID = 0;
		public int? RouteID { get { return mRouteID; } set { mRouteID = value; } } 

		private string mHubID = "";
        public string HubID { get { return mHubID.ToUpper(); } set { mHubID = value.ToUpper(); } } 

		private int? mLocalArea = 0;
		public int? LocalArea { get { return mLocalArea; } set { mLocalArea = value; } } 

		private int? mMainArea = 0;
		public int? MainArea { get { return mMainArea; } set { mMainArea = value; } } 

		private int? mRegionalArea = 0;
		public int? RegionalArea { get { return mRegionalArea; } set { mRegionalArea = value; } } 

		private int? mInternational = 0;
		public int? International { get { return mInternational; } set { mInternational = value; } } 

		private int? misActive = 0;
		public int? isActive { get { return misActive; } set { misActive = value; } } 

		private string mEventDateTime = "";
		public string EventDateTime { get { return mEventDateTime; } set { mEventDateTime = value; } } 

		private string mEmployeeID = "";
		public string EmployeeID { get { return mEmployeeID; } set { mEmployeeID = value; } } 

		private string mTerminalID = "";
		public string TerminalID { get { return mTerminalID; } set { mTerminalID = value; } } 

		private string mLastUpdate = "";
		public string LastUpdate { get { return mLastUpdate; } set { mLastUpdate = value; } } 

		private string misDirty = "";
		public string isDirty { get { return misDirty; } set { misDirty = value; } } 

		

		private string mNewZoneID = "";
        public string NewZoneID { get { return mNewZoneID.ToUpper(); } set { mNewZoneID = value.ToUpper(); } } 

		private double? mLat = 0;
		public double? Lat { get { return mLat; } set { mLat = value; } } 

		private double? mLon = 0;
		public double? Lon { get { return mLon; } set { mLon = value; } } 

		private DateTime? mRecordCreatedDT = new DateTime(1900, 1, 1);
		public DateTime? RecordCreatedDT { get { return mRecordCreatedDT; } set { mRecordCreatedDT = value; } } 

		private DateTime? mUpdateDT = new DateTime(1900, 1, 1);
		public DateTime? UpdateDT { get { return mUpdateDT; } set { mUpdateDT = value; } } 

		private string mUpdatedBy = "";
		public string UpdatedBy { get { return mUpdatedBy; } set { mUpdatedBy = value; } } 

		private string mUpdateTerminalID = "";
		public string UpdateTerminalID { get { return mUpdateTerminalID; } set { mUpdateTerminalID = value; } } 

			
		#endregion
		
		#region Constructors
		
		/// <summary>
		/// Default Constructor for Zones Class
		/// </summary>
		public Zones()
		{

		}
		
		/// <summary>
		/// Constructor for Zones Class with Initialization
		/// </summary>
		public Zones(string pZoneID, string pSuburb, string pPostalCode, string pArea, double? pMainZone, double? pSubZone, int? pRouteID, string pHubID, int? pLocalArea, int? pMainArea, int? pRegionalArea, int? pInternational, int? pisActive, string pEventDateTime, string pEmployeeID, string pTerminalID, string pLastUpdate, string pisDirty, string pNewZoneID, double? pLat, double? pLon, DateTime? pRecordCreatedDT, DateTime? pUpdateDT, string pUpdatedBy, string pUpdateTerminalID)
		{
			ZoneID = pZoneID;
			Suburb = pSuburb;
			PostalCode = pPostalCode;
			Area = pArea;
			MainZone = pMainZone;
			SubZone = pSubZone;
			RouteID = pRouteID;
			HubID = pHubID;
			LocalArea = pLocalArea;
			MainArea = pMainArea;
			RegionalArea = pRegionalArea;
			International = pInternational;
			isActive = pisActive;
			EventDateTime = pEventDateTime;
			EmployeeID = pEmployeeID;
			TerminalID = pTerminalID;
			LastUpdate = pLastUpdate;
			isDirty = pisDirty;
			
			NewZoneID = pNewZoneID;
			Lat = pLat;
			Lon = pLon;
			RecordCreatedDT = pRecordCreatedDT;
			UpdateDT = pUpdateDT;
			UpdatedBy = pUpdatedBy;
			UpdateTerminalID = pUpdateTerminalID;
		}

		/// <summary>
		/// Constructor for Zones Class with Initialization
		/// </summary>
		public Zones(DataRow loRow)
		{	
			ZoneID = Functions.SafeString(loRow["ZoneID"]);
			Suburb = Functions.SafeString(loRow["Suburb"]);
			PostalCode = Functions.SafeString(loRow["PostalCode"]);
			Area = Functions.SafeString(loRow["Area"]);
			MainZone = Functions.SafeDouble(loRow["MainZone"], 0);
			SubZone = Functions.SafeDouble(loRow["SubZone"], 0);
			RouteID = Functions.SafeInt(loRow["RouteID"], 0);
			HubID = Functions.SafeString(loRow["HubID"]);
			LocalArea = Functions.SafeInt(loRow["LocalArea"], 0);
			MainArea = Functions.SafeInt(loRow["MainArea"], 0);
			RegionalArea = Functions.SafeInt(loRow["RegionalArea"], 0);
			International = Functions.SafeInt(loRow["International"], 0);
			isActive = Functions.SafeInt(loRow["isActive"], 0);
			EventDateTime = Functions.SafeString(loRow["EventDateTime"]);
			EmployeeID = Functions.SafeString(loRow["EmployeeID"]);
			TerminalID = Functions.SafeString(loRow["TerminalID"]);
			LastUpdate = Functions.SafeString(loRow["LastUpdate"]);
			isDirty = Functions.SafeString(loRow["isDirty"]);
			
			NewZoneID = Functions.SafeString(loRow["NewZoneID"]);
			Lat = Functions.SafeDouble(loRow["Lat"], 0);
			Lon = Functions.SafeDouble(loRow["Lon"], 0);
			RecordCreatedDT = Functions.SafeDateTime(loRow["RecordCreatedDT"]);
			UpdateDT = Functions.SafeDateTime(loRow["UpdateDT"]);
			UpdatedBy = Functions.SafeString(loRow["UpdatedBy"]);
			UpdateTerminalID = Functions.SafeString(loRow["UpdateTerminalID"]);
		}
		
		#endregion
		
		#region Data Access - Private Methods
	
		private static List<Zones> ReturnList(DataTable pdtTable)
		{
			List<Zones> list = new List<Zones>();
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
    
		private static Zones ReturnObject(DataRow loRow)
		{
            return new Zones(Functions.SafeString(loRow["ZoneID"]).ToUpper(),
                Functions.SafeString(loRow["Suburb"]).ToUpper(),
                Functions.SafeString(loRow["PostalCode"]).ToUpper(),
                Functions.SafeString(loRow["Area"]).ToUpper(),
				Functions.SafeDouble(loRow["MainZone"], 0),
				Functions.SafeDouble(loRow["SubZone"], 0),
				Functions.SafeInt(loRow["RouteID"], 0),
                Functions.SafeString(loRow["HubID"]).ToUpper(),
				Functions.SafeInt(loRow["LocalArea"], 0),
				Functions.SafeInt(loRow["MainArea"], 0),
				Functions.SafeInt(loRow["RegionalArea"], 0),
				Functions.SafeInt(loRow["International"], 0),
				Functions.SafeInt(loRow["isActive"], 0),
                Functions.SafeString(loRow["EventDateTime"]).ToUpper(),
                Functions.SafeString(loRow["EmployeeID"]).ToUpper(),
                Functions.SafeString(loRow["TerminalID"]).ToUpper(),
                Functions.SafeString(loRow["LastUpdate"]).ToUpper(),
                Functions.SafeString(loRow["isDirty"]).ToUpper(),

                Functions.SafeString(loRow["NewZoneID"]).ToUpper(),
				Functions.SafeDouble(loRow["Lat"], 0),
				Functions.SafeDouble(loRow["Lon"], 0),
				Functions.SafeDateTime(loRow["RecordCreatedDT"]),
				Functions.SafeDateTime(loRow["UpdateDT"]),
                Functions.SafeString(loRow["UpdatedBy"]).ToUpper(),
                Functions.SafeString(loRow["UpdateTerminalID"]).ToUpper());
		}		
		
		#endregion

		#region Data Access
		
		public static Zones ReadObject(string pZoneID)
		{
			DataRow loRow;          
		    try
		    {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;

				string sqlCommand = STP_READ;
				DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pZoneID);
				
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
	
        //public static int Save(string pZoneID, string pSuburb, string pPostalCode, string pArea, double? pMainZone, double? pSubZone, int? pRouteID, string pHubID, int? pLocalArea, int? pMainArea, int? pRegionalArea, int? pInternational, int? pisActive, string pEventDateTime, string pEmployeeID, string pTerminalID, string pLastUpdate, string pisDirty, Guid prowguid, string pNewZoneID, double? pLat, double? pLon, DateTime? pRecordCreatedDT, DateTime? pUpdateDT, string pUpdatedBy, string pUpdateTerminalID)
        //{
        //    try
        //    {
        //        Database db = DatabaseFactory.CreateDatabase();
        //        string sqlCommand = STP_SAVE;
        //        DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pZoneID, pSuburb, pPostalCode, pArea, pMainZone, pSubZone, pRouteID, pHubID, pLocalArea, pMainArea, pRegionalArea, pInternational, pisActive, pEventDateTime, pEmployeeID, pTerminalID, pLastUpdate, pisDirty, prowguid, pNewZoneID, pLat, pLon, pRecordCreatedDT, pUpdateDT, pUpdatedBy, pUpdateTerminalID);
				
        //        return (int)db.ExecuteScalar(dbCommand);
        //    }        
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    } 	
        //}

        //public static int Update(string pZoneID, string pSuburb, string pPostalCode, string pArea, double? pMainZone, double? pSubZone, int? pRouteID, string pHubID, int? pLocalArea, int? pMainArea, int? pRegionalArea, int? pInternational, int? pisActive, string pEventDateTime, string pEmployeeID, string pTerminalID, string pLastUpdate, string pisDirty, Guid prowguid, string pNewZoneID, double? pLat, double? pLon, DateTime? pRecordCreatedDT, DateTime? pUpdateDT, string pUpdatedBy, string pUpdateTerminalID)
        //{
        //    try
        //    {				
        //        Database db = DatabaseFactory.CreateDatabase();
        //        string sqlCommand = STP_SAVE;
        //        DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pZoneID, pSuburb, pPostalCode, pArea, pMainZone, pSubZone, pRouteID, pHubID, pLocalArea, pMainArea, pRegionalArea, pInternational, pisActive, pEventDateTime, pEmployeeID, pTerminalID, pLastUpdate, pisDirty, prowguid, pNewZoneID, pLat, pLon, pRecordCreatedDT, pUpdateDT, pUpdatedBy, pUpdateTerminalID);
				
        //        return (int)db.ExecuteNonQuery(dbCommand);
        //    }        
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    } 	
        //}

        //public static bool Delete(int pintID)
        //{
        //    try
        //    {
        //        Database db = DatabaseFactory.CreateDatabase();
        //        string sqlCommand = STP_DELETE;
        //        DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand, pintID);
				
        //        db.ExecuteNonQuery(dbCommand);
				
        //        return true;
        //    }        
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    } 	
        //}

		public static IDataReader Read(int pintID)
        {
			IDataReader dr = null;
			try
		    {
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;

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
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;

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
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;

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
    
		public static List<Zones> ReadAll_List()
        {
			try
			{
                Database db = new DatabaseProviderFactory().Create(ConfigSettings.ReadConfigValue("DefaultWMSConnectionString", ""));;

				string sqlCommand = STP_READ_ALL;
				DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
				
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
