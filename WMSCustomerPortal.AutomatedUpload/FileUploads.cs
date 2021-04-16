using System;
using System.Collections.Generic;
using System.IO;

using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WMSCustomerPortal.Models.Common;
using WMSCustomerPortal.Models.Entities;
using WMSCustomerPortal.Web;
using ClassLibrary6;
using WMSCustomerPortal.Web.Common;
using OfficeOpenXml;
using System.Configuration;

namespace WMSCustomerPortal.AutomatedUpload
{
	public class FileUploads : Web.Controllers.HomeController
	{
		ProductStaging productRecord = new ProductStaging();
		AuditRecord auditRecord = new AuditRecord();
		ProductUpload productUpload = new ProductUpload();
		CustomerReturnUpload customerReturn = new CustomerReturnUpload();
		PurchaseOrderUploads POU = new PurchaseOrderUploads();
		customerUpload cs = new customerUpload();
		SalesOrderUpoad salesOrderUpload = new SalesOrderUpoad();
		Customers customerRecord = new Customers();
		AuditMail auditMail = new AuditMail();

		


		int PrincipleValue = 0;


		List<string> auditStatus = new List<string>();
		Object[] objects;


		List<Class1> principal = new List<Class1>();
		Class1 t = new Class1();

		public string ReturnMessage = "";


		string Hostname = "10.0.25.11/RAMFTP";
		//string User = "";
		//string pass = "";

		 string Host = ConfigurationManager.AppSettings["FTP_URL"];
		 string User = ConfigurationManager.AppSettings["USER"];
		 string pass = ConfigurationManager.AppSettings["PASS"];

		private NetworkCredential credentials = new NetworkCredential("apex", "@pex2019");

		int res;
		string file = "";

		string FileName;

		public int ListDirectories()
		{
			List<Class1> customerList = t.ReadAllList();


			foreach (Class1 test in customerList)
			{
				Console.WriteLine(test.FilePath);

				// delete file put it in fails folder

				try
				{
					// remoteFtpPath = "Apex1";
					// var urlu = "";
					// if (urlu = $"ftp://{Hostname}/{test.PrincipalCode}"==)
					var url = "";
					try
					{
						url = $"ftp://{Hostname}/{test.PrincipalCode}/NEW/"; // Is this right? Not used C#6 yet...
					}
					catch (Exception esc)
					{
						continue;
					}

					var request = (FtpWebRequest)WebRequest.Create(url);
					request.Method = WebRequestMethods.Ftp.ListDirectory;
					request.Credentials = credentials;
					request.Proxy = null;

					var directories = new List<string>();

					using (var response = (FtpWebResponse)request.GetResponse())
					using (var responseStream = response.GetResponseStream())
					using (var reader = new StreamReader(responseStream))
					{
						string names;
						try
						{
							names = reader.ReadToEnd();
						}
						catch (Exception ex)
						{
							continue; ;
						}


						List<string> readfiles = new List<string>();
						List<string> Directories = names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

						// getting all the files from the directory add to readfiles list 
						for (int i = 0; i < Directories.Count(); i++)
						{
							string input = Directories[i].ToString();
							string output = input.Substring(input.IndexOf("/") + 1);
							readfiles.Add(output);


						}


						// download each file from the ftp server
						for (int listOffiles = 0; listOffiles < readfiles.Count(); listOffiles++)
						{
							file = readfiles[listOffiles].ToString();
							var url2 = $"ftp://{Hostname}/{test.PrincipalCode}/NEW/" + file;
							//FtpWebRequest r = (FtpWebRequest)WebRequest.Create(url2);
							//r.Method = WebRequestMethods.Ftp.Rename;
							//r.Credentials = credentials;
							WebRequest request1 = WebRequest.Create(url2);
							request1.Method = WebRequestMethods.Ftp.DownloadFile;

							request1.Credentials = credentials;
							FtpWebResponse response1 = (FtpWebResponse)request1.GetResponse();

							var Extensionresult = file.Substring(file.LastIndexOf('.') + 1);

							string name = "";
							string TransactionType = "";
							string ReturnMessage = "";
							List<string> lines = new List<string>();

							if (Extensionresult == "xlsx")
							{
								try
								{
									StreamReader reader1 = new StreamReader(response1.GetResponseStream());


									MemoryStream stream = new MemoryStream();
									response1.GetResponseStream().CopyTo(stream);
									var package = new ExcelPackage(stream);
									ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

									int col = 1;

									string cellValue = "";

									for (int row = 1; worksheet.Cells[row, col].Value != null; row++)
									{
										for (int pcol = 1; worksheet.Cells[row, pcol].Value != null; pcol++)
										{
											cellValue += test.FileDelimiter + worksheet.Cells[row, pcol].Value.ToString();
											// cellrow=cellValue += FileDelimiter;

										}
										// replace the lastest delimeter 
										lines.Add(cellValue);

										cellValue = "";
									}
								}
								catch (Exception ex)
								{
									continue;
								}

							}
							else
							{
								try
								{
									StreamReader reader1 = new StreamReader(response1.GetResponseStream());


									do
									{
										string textline = reader1.ReadLine();


										lines.Add(textline);

									} while (reader1.Peek() != -1);

									//   string ReturnMessage = "";
								}
								catch (Exception xc)
								{

									continue;
								}


							}
							//Get the table name
							name = lines[0].ToString().ToUpper();
							string input = Directories[listOffiles].ToString();
							string output = input.Substring(input.IndexOf("/") + 1);
							string FileName = output;
							TransactionType = name;
							name = name.Replace("" + test.FileDelimiter + "", "");

							auditRecord.User = "SYSTEM";


							//validation the table/file name
							switch (name)
							{
								case "PRODUCT":
									ReturnMessage = productUpload.productFile(lines, test.PrincipalID, "SYSTEM", "SYSTEM", FileName, TransactionType, test.FileDelimiter);
									res = 1;
									break;
								case "CUSTOMER":
									ReturnMessage = cs.customerFile(lines, test.PrincipalID, "SYSTEM", "2", "SYSTEM", FileName, TransactionType, test.FileDelimiter);
									res = 1;
									break;
								case "PURCHASE ORDER":
									ReturnMessage = POU.POImport(lines, test.PrincipalID, "SYSTEM", "SYSTEM", FileName, TransactionType, test.FileDelimiter);
									res = 1;
									break;
								case "SALES ORDER":
                                    string PrincipalCode = test.PrincipalCode;
                                    ReturnMessage = salesOrderUpload.SaleOrderUpload(lines, test.PrincipalID, "SYSTEM", "SYSTEM", FileName, TransactionType, test.FileDelimiter, PrincipalCode);
									res = 1;
									break;
								case "CUSTOMER RETURN":
									ReturnMessage = customerReturn.ReturnsUpload(lines, test.PrincipalID, "SYSTEM", "SYSTEM", FileName, TransactionType, test.FileDelimiter);
									res = 1;
									break;
								default:
									auditRecord.EventStatus = "FAILED";
									auditRecord.FileName = FileName;
									auditRecord.Transaction = "Not appicable".ToUpper();
									auditRecord.TransactionType = "Not Applicable".ToUpper();
									auditRecord.Message = "0 Row(s) Uploaded Succefully And 0 Row(s) Failed - INVALID FILE DESCRIPTION ".ToUpper();
									ReturnMessage = auditRecord.Message;
									auditStatus.Add(auditRecord.Message);
									ReturnMessage = auditRecord.Message;
									// auditMail.Sendmail(auditStatus, "SYSTEM", FileName);
									auditRecord.Save(test.PrincipalID);

									ReturnMessage = "0 Row(s) Uploaded Succefully  And 0 Row(s) Failed - INVALID FILE DESCRIPTION".ToUpper();
									//var DelUrl = $"ftp://{Hostname}/{test.PrincipalCode}/NEW/" + file;
									//FtpWebRequest r6 = (FtpWebRequest)WebRequest.Create(DelUrl);
									//r6.Method = WebRequestMethods.Ftp.DeleteFile;
									//r6.Credentials = credentials;
									
									//try
									//{
									//	r6.GetResponse().Close();
									//}
									//catch (Exception appex)
									//{
									//	Console.WriteLine("Append status: {0}", response.StatusDescription);
										

									//}
								
									break;

							}
							if (ReturnMessage.Substring(36, 1) == "0") // FULL PROCCESSED
							{
								
								var uploadUrl = $"ftp://{Hostname}/{test.PrincipalCode}/COLLECTED/" + file;
								FtpWebRequest r6 = (FtpWebRequest)WebRequest.Create(uploadUrl);
								r6.Method = WebRequestMethods.Ftp.UploadFile;
								r6.UsePassive = true;
								r6.UseBinary = true;
								r6.Credentials = credentials;
								try
								{
									r6.GetResponse().Close();
								}
								catch (Exception appex)
								{

									Console.WriteLine("Append status: {0}", response.StatusDescription);

								}
								var deleteUrl = $"ftp://{Hostname}/{test.PrincipalCode}/NEW/" + file;

								r6 = (FtpWebRequest)WebRequest.Create(url2);
								r6.Method = WebRequestMethods.Ftp.DeleteFile;
								r6.Credentials = credentials;
								r6.GetResponse().Close();
							}
							else if (ReturnMessage.Substring(0, 1) == "0" || ReturnMessage.Substring(0, 7) == "INVALID" || ReturnMessage.Substring(0, 1) == "E") // ERROR FILES
							{
								var uploadUrl = $"ftp://{Hostname}/{test.PrincipalCode}/FAILED/" + file;
								FtpWebRequest r6 = (FtpWebRequest)WebRequest.Create(uploadUrl);
								r6.Method = WebRequestMethods.Ftp.UploadFile;
								r6.UsePassive = true;
								r6.UseBinary = true;
								r6.Credentials = credentials;
								try
								{
									r6.GetResponse().Close();
								}
								catch (Exception appex)
								{
									Console.WriteLine("Append status: {0}", response.StatusDescription);

								}

								var deleteUrl = $"ftp://{Hostname}/{test.PrincipalCode}/NEW/" + file;

								r6 = (FtpWebRequest)WebRequest.Create(url2);
								r6.Method = WebRequestMethods.Ftp.DeleteFile;
								r6.Credentials = credentials;
								r6.GetResponse().Close();
							}
							else
							{
								var uploadUrl = $"ftp://{Hostname}/{test.PrincipalCode}/FAILED/" + file;
								FtpWebRequest r6 = (FtpWebRequest)WebRequest.Create(uploadUrl);
								r6.Method = WebRequestMethods.Ftp.UploadFile;
								r6.UsePassive = true;
								r6.UseBinary = true;
								r6.Credentials = credentials;
								try
								{
									r6.GetResponse().Close();
								}
								catch (Exception appex)
								{
									Console.WriteLine("Append status: {0}", response.StatusDescription);

								}
								var deleteUrl = $"ftp://{Hostname}/{test.PrincipalCode}/NEW/" + file;

								r6 = (FtpWebRequest)WebRequest.Create(url2);
								r6.Method = WebRequestMethods.Ftp.DeleteFile;
								r6.Credentials = credentials;
								r6.GetResponse().Close();
							}
						}
					}
				}
				catch (WebException ex)
				{
					FtpWebResponse response = (FtpWebResponse)ex.Response;
					if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
					{
						Console.Write("FILE NOT AVAILABLE");

						continue;
					}
					else
					{
						continue;
					}
				}
			}
			return res;
		}
	}
}