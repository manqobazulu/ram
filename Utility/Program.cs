using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;

namespace Utility
{
    class Program
    {

        static void ResendOrdersToWMSByDate()
        {
            string FromDate;
            string ToDate;

            Console.Write("Enter the From Date - ");
            FromDate = Console.ReadLine();
            Console.WriteLine("From Date: '{0}'", FromDate);

            Console.Write("\nEnter the To Date - ");
            ToDate = Console.ReadLine();
            Console.WriteLine("To Date: '{0}'", ToDate);

            string Query = String.Format("SELECT [OrderID], [EventTerminal], [EventUser] FROM [WMSHostCustomerPortal].[dbo].[Orders] WHERE [DateCreated] > '{0}' AND [DateCreated] < '{1}' AND Submitted = 1", FromDate, ToDate);
            Console.WriteLine("\nQuery: '{0}'", Query);

            string Conn = ConfigurationManager.ConnectionStrings["WMSHostCustomerPortal"]?.ConnectionString;
            Console.WriteLine("\nConn: '{0}'", Conn);

            DataTable dt = new DataTable();
            int rows_returned;

            using (SqlConnection connection = new SqlConnection(Conn))
            using (SqlCommand cmd = connection.CreateCommand())
            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            {
                cmd.CommandText = Query;
                cmd.CommandType = CommandType.Text;
                connection.Open();
                rows_returned = sda.Fill(dt);
                connection.Close();
            }

            if (dt.Rows.Count == 0)
            {
                Console.WriteLine("\nQuery Returned No Rows");
            }
            else
            {
                WMSCustomerPortal.Business.OutboundService OutboundDataService = new WMSCustomerPortal.Business.OutboundService();

                Console.WriteLine("\n");
                foreach (DataRow Row in dt.Rows)
                {
                    Console.WriteLine("\nRow [OrderID]: '{0}'", Row["OrderID"].ToString());

                    int OrderID = Convert.ToInt32(Row["OrderID"]);
                    string EventTerminal = Row["EventTerminal"].ToString();
                    string EventUser = Row["EventUser"].ToString();

                    if(OutboundDataService.ProcessOrderToWMS(OrderID, EventTerminal, EventUser))
                    {
                        Console.WriteLine("Order Submitted Successfully");
                    }
                    else
                    {
                        Console.WriteLine("Failed To Submit Order");
                    }
                }
            }

            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            ResendOrdersToWMSByDate();
            
        }
    }
}
