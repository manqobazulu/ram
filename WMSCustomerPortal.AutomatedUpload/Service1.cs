using ClassLibrary6;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WMSCustomerPortal.Web;


namespace WMSCustomerPortal.AutomatedUpload
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer = new Timer();
        FileUploads fileUploads = new FileUploads();
        Class1 principal = new Class1();
        public Service1()
        {
            InitializeComponent();
           
        }


      
        protected override void OnStart(string[] args)
        {
            WriteToFile("Service is started at " + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 10000; //number in milisecinds  equivalent to 10sec
            timer.Enabled = true;
        }
        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            fileUploads.ListDirectories();

            if (fileUploads.ListDirectories() == 1)
                WriteToFile("File processed " + DateTime.Now);
            else
                WriteToFile("nothing processed " + DateTime.Now);
           // fileUploads.ListDirectories();


        }
        public void onDebug()
        {
            OnStart(null);

        }
        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}
