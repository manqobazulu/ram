using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Text;
using System.Configuration;

namespace RAM.Utilities.Common
{
    public class SMTPMailer
    {
        MailMessage message;

        public SMTPMailer()
        {
            //creates a default instance
            message = new MailMessage(); 
        }

        [Flags]
        public enum DeliveryNotificationOptions
        {
            None = 0, OnSuccess = 1, OnFailure = 2, Delay = 4, Never = (int)0x08000000
        }


        public string SMTPHost
        {
            get
            {

                return ConfigurationManager.AppSettings["SMTPHost"];
                 }
            


        }

      
        public int SMTPPort
        {
            get
            {

                int retVal = 0;
                int.TryParse(ConfigurationManager.AppSettings["SMTPPort"], out retVal);
                return retVal ;
            }



        }


        public MailAddress From
        {
            get
            {
                return message.From;
            }
            set
            {
                if( (value == null))
                {
                    throw new ArgumentNullException("value");
                }
                message.From = value;
            }
        }

        public MailAddress Sender
        {
            get
            {
                return message.Sender;
            }
            set
            {
                message.Sender = value;
            }
        }


         public MailAddressCollection ReplyToList
        {
            get
            {


                return message.ReplyToList;
            }
           
        }


      



        public string MessageBody
         {
             get { return message.Body; } 
            set { message.Body = value; }


         }


      //  public MailAddressCollection To
        public System.Collections.ArrayList To
        {
            get
            {
                System.Collections.ArrayList alRetVal = new System.Collections.ArrayList();

                foreach(MailAddress addr in message.To)
                { alRetVal.Add(addr.Address); }
                return alRetVal;
            }
            set
            {
                foreach(string addr in value)
                {
                    MailAddress item = new MailAddress(addr);
                    message.To.Add(item);
                }
            
            }
        }

        public MailAddressCollection Bcc
        {
            get
            {
                return message.Bcc;
            }
        }

        public MailAddressCollection CC
        {
            get
            {
                return message.CC;
            }
        }

        public MailPriority Priority
        {
            get
            {
                return message.Priority;
            }
            set
            {
                message.Priority = value;
            }
        }

     

        public string Subject
        {
            get
            {
                return (message.Subject != null ? message.Subject : String.Empty);
            }
            set
            {
                message.Subject = value;
            }
        }

        public System.Text.Encoding SubjectEncoding
        {
            get
            {
                return message.SubjectEncoding;
            }
            set
            {
                message.SubjectEncoding = value;
            }
        }

        public System.Collections.Specialized.NameValueCollection Headers
        {
            get
            {
                return message.Headers;
            }
        }

        /// <summary>
        /// This helper class sends an email message using the System.Net.Mail namespace
        /// </summary>
        /// <param name="fromEmail">Sender email address</param>
        /// <param name="toEmail">Recipient email address</param>
        /// <param name="bcc">Blind carbon copy email address</param>
        /// <param name="cc">Carbon copy email address</param>
        /// <param name="subject">Subject of the email message</param>
        /// <param name="body">Body of the email message</param>
        /// <param name="attachment">File to attach</param>

        #region Members

        public void SendMailMessage()
        {
           

            //create the SmtpClient instance
            SmtpClient mSmtpClient = new SmtpClient();
           
            mSmtpClient.Host = SMTPHost;
            mSmtpClient.Port = SMTPPort;

            try
            {
                //send the mail message
                mSmtpClient.Send(message);
            }catch(Exception exc)
            {
                Console.WriteLine(exc);
            }


        }


        public void SendMailMessage(string toEmail, string fromEmail, string bcc, string cc, string subject, string body, List<string> attachmentFullPath)
        {
            //create the MailMessage object
            MailMessage mMailMessage = new MailMessage();

            //set the sender address of the mail message
            if (!string.IsNullOrEmpty(fromEmail))
            {
                mMailMessage.From = new MailAddress(fromEmail);
            }

            //set the recipient address of the mail message
            mMailMessage.To.Add(new MailAddress(toEmail));

            //set the blind carbon copy address
            if (!string.IsNullOrEmpty(bcc))
            {
                mMailMessage.Bcc.Add(new MailAddress(bcc));
            }

            //set the carbon copy address
            if (!string.IsNullOrEmpty(cc))
            {
                mMailMessage.CC.Add(new MailAddress(cc));
            }

            //set the subject of the mail message
            if (!string.IsNullOrEmpty(subject))
            {
                mMailMessage.Subject = "RAM WMS Web Application Notification";
            }
            else
            {
                mMailMessage.Subject = subject;
            }

            //set the body of the mail message
            mMailMessage.Body = body;

            //set the format of the mail message body
            mMailMessage.IsBodyHtml = false;

            //set the priority
            mMailMessage.Priority = MailPriority.Normal;

            //add any attachments from the filesystem
            foreach (var attachmentPath in attachmentFullPath)
            {
                Attachment mailAttachment = new Attachment(attachmentPath);
                mMailMessage.Attachments.Add(mailAttachment);
            }

            //create the SmtpClient instance
            SmtpClient mSmtpClient = new SmtpClient();

            //send the mail message
            mSmtpClient.Send(mMailMessage);
        }

        /// <summary>
        /// Determines whether an email address is valid.
        /// </summary>
        /// <param name="emailAddress">The email address to validate.</param>
        /// <returns>
        /// 	<c>true</c> if the email address is valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidEmailAddress(string emailAddress)
        {
            // An empty or null string is not valid
            if (String.IsNullOrEmpty(emailAddress))
            {
                return (false);
            }

            // Regular expression to match valid email address
            string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            // Match the email address using a regular expression
            Regex re = new Regex(emailRegex);
            if (re.IsMatch(emailAddress))
                return (true);
            else
                return (false);
        }

        #endregion
    }
}