using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using System.Threading.Tasks;



namespace ObjectsLibrary
{
    public class Class1
    {
        private const string STP_READ = "usp_Principal_Read";
        public Class1 ReadObject(int principalID)
        {

            DataRow loRow;
            try
            {
                string db =  ConfigurationManager.ConnectionStrings["CString"].ConnectionString;
                SqlConnection con = new SqlConnection(db);
                string sqlCommand = STP_READ;
                SqlCommand cmd = new SqlCommand(STP_READ, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("", 3));

                cmd.ExecuteNonQuery();




                //loRow = db.ExecuteDataSet(dbCommand).Tables[0].Rows[0];

                if (loRow.Equals(null))
                {
                    return null;
                }
                else
                {
                    return ReturnObject(loRow);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
