using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WMSCustomerPortal.Models.Entities;
using RAM.Utilities.Common;
using System.Text;
using System.Collections.Generic;

namespace WMSCustomerPortal.Models.Common
{
    public class AuditMail
    {
        public void Sendmail(List<string> auditList, string user, string fileName)
        {
            SMTPMailer mailer = new SMTPMailer();
            System.Net.Mail.MailAddress fromMail = new System.Net.Mail.MailAddress(ConfigSettings.ReadConfigValue("FromWMSEmail", "wmswebadmin@ram.co.za"));

            mailer.From = fromMail;
            System.Collections.ArrayList toMail = new System.Collections.ArrayList();
            toMail.Add(user);
            mailer.To = toMail;

            mailer.Subject = fileName + " Manual Upload - RAM WMS Management Portal";

            string value = "";
            foreach (string Message in auditList)
            {

                value += Message + Environment.NewLine;

            }
            Console.Write(value);
            mailer.MessageBody = value;

            //   mailer.SendMailMessage();// send email

        }
    }
}
