using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LuminousSMS.Data;
using System.Threading;
using System.Configuration;
using System.IO;
using System.Net;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

/*
==================================================================================================================
Copyright	: Zed-Axis Technologies, 2011
Author		: Pankaj Mittal
Create date : 
Description : 
==================================================================================================================
Change Log: 
----------
DD-MM-YYYY, Name, Code: Description.
------------------------------------------------------------------------------------------------------------------
07-feb-2012, Pankaj Mittal, #CC01: add attempt count so if error occure at email sending then send mail till attempt count
15-Oct-2012, Prashant Chitransh, #CC02: changes to implement message sending with new templates for ZedServiceLWT.
15-Oct-2012, Prashant Chitransh, #CC03: changes to implement message sending with new templates for ZedServiceLWT.
26-Dec-2014, Shashikant Singh ,  #CC04: Added condition for missing dynamic Configuration
21-July-2015,Shashikant Singh ,  #CC05: Added condition for missing dynamic Configuration such as EmailFrom
28-Nov-2015,Pankaj Mittal,#cc06: add a configurable check to bypass Gmail certificate issue.
*/
namespace EmailSMSApp
{
    public class Program
    {
        System.Resources.ResourceManager objRM;
        System.Threading.Thread t = null;
        System.Threading.Thread t1 = null;
        Int32 SleepDuration;
        string strConnectionString, strServiceUserID, strServicePassword, strSMSErrorLogPath;
        static void Main(string[] args)
        {
            Program objprogram = new Program();
            if (ConfigurationManager.AppSettings["SMS"] == "1") /*#CC04:Added*/
            {
                try
                {
                    WriteLogToTextFile("SMS START" + DateTime.Now.ToString());
                    objprogram.Start();
                    WriteLogToTextFile("SMS END" + DateTime.Now.ToString());
                }
                catch (Exception ex)
                { WriteLogToTextFile(ex.ToString()); }
            }
            if (ConfigurationManager.AppSettings["Email"] == "1") /*#CC04:Added*/
            {
                try
                {
                    WriteLogToTextFile("Email start");
                    List<string> lstLog = objprogram.SendEmails();
                    if (lstLog != null)
                    {
                        StringBuilder sbLog = new StringBuilder();
                        foreach (string log in lstLog)
                        {
                            sbLog.AppendLine(log);
                        }
                        Program.WriteLogToTextFile(sbLog.ToString());
                    }
                    else
                    {
                        WriteLogToTextFile("No Email found");
                    }
                }
                catch (Exception ex)
                { WriteLogToTextFile(ex.ToString()); }
            }
        }

        public void Start()
        {
            try
            {
                SleepDuration = Convert.ToInt32(ConfigurationManager.AppSettings["SleepDuration"]);
                strConnectionString = ConfigurationManager.ConnectionStrings["constr"].ToString();
                strServiceUserID = ConfigurationManager.AppSettings["SMSServiceUserID"].ToString();
                strServicePassword = ConfigurationManager.AppSettings["SMSServicePassword"].ToString();
                strSMSErrorLogPath = ConfigurationManager.AppSettings["SMSErrorLogPath"].ToString();
                // Thread.Sleep(SleepDuration);

                Console.WriteLine("Service Started......");
                StartAction();

                //System.Threading.ThreadStart ts = new System.Threading.ThreadStart(StartAction);
                Console.WriteLine("Thread Started......");
                //   t = new System.Threading.Thread(ts);
                //   t.Start();
            }
            catch (Exception ex)
            {
                WriteLogToTextFile(ex.Message);
            }
        }

        void errorLog(string exMessage)
        {
            Console.WriteLine(exMessage + DateTime.Now + "\n");
            //FileStream fs = new FileStream(strSMSErrorLogPath, FileMode.OpenOrCreate, FileAccess.Write);
            //StreamWriter m_streamWriter = new StreamWriter(fs);
            //m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            //m_streamWriter.WriteLine(exMessage + DateTime.Now + "\n");
            //m_streamWriter.Flush();
            //m_streamWriter.Close();
        }

        private void StartAction()
        {
            string str = string.Empty;
            //using (clsSMSTest objSMS = new clsSMSTest())
            {
                try
                {
                    WriteLogToTextFile("Function StartAction() starts.at " + DateTime.Now);
                    //str = objSMS.SMSsenderOutBoundNew();          // #CC02: commented.
                    SMSsenderOutBoundNew();                         // #CC02: added.
                    //  WriteLogToTextFile("Function StartAction() ends. at " + DateTime.Now + " " + str);
                }

                catch (Exception ex)
                {
                    str = str + ex.Message;
                    errorLog(str);
                }
            }
        }

        // #CC02: method createded.
        private bool SendSMS(string strRecip, string strMsgText)
        {
            Uri objURI = GetSMSURL(strRecip, strMsgText); WebRequest objWebRequest = WebRequest.Create(objURI);
            WebResponse objWebResponse;
            Stream objStream;
            string finalStatus = ConfigurationManager.AppSettings["SMSResultValue"]; /*#CC04:Added*/
            Boolean result = false; /*#CC04:Added*/
        
            try
            {
                objWebResponse = objWebRequest.GetResponse();
                 objStream = objWebResponse.GetResponseStream();
                 StreamReader objStreamReader = new StreamReader(objStream);
                 string strHTML = objStreamReader.ReadToEnd();
                 // #CC03: added (start).
                 try
                 {
                     if (ConfigurationManager.AppSettings["SMSResultValueType"] == "1")/*#CC04:Added*/
                     {

                         string[] arrRes = System.Text.RegularExpressions.Regex.Split(strHTML, finalStatus);
                         if (arrRes.Length > 1 && !string.IsNullOrEmpty(arrRes[1].Substring(0, 1)) && Convert.ToInt16(arrRes[1].Substring(0, 1)) > 0)
                         {
                             result = true;
                         }
                         result = false;
                     }
                     /*#CC04:Added start here*/
                     else if (ConfigurationManager.AppSettings["SMSResultValueType"] == "0")
                     {
                         result = strHTML.Contains(finalStatus);

                     }

                     return result;
                     /*#CC04:Added end here*/
                 }
                 catch (Exception ex)
                 {
                     return false;
                 }
                // #CC03: added (end).
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)wex.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string error = reader.ReadToEnd();
                            //TODO: use JSON.net to parse this string and look at the error message
                            result = false;
                        }
                    }

                }
            }


            return result;
              //return true;
            //return strHTML.Contains("Credits Consumed: 1");           // #CC03: commented.

            //string strtest = "Message Submitted<br>Credits Consumed: 2<br>Balance Credits: 4206<br>Mobile=919990182470 MsgId=1116491069";

            
        }

        private static Uri GetSMSURL(string strRecip, string strMsgText)
        {
            /*string strSender = "SenderID";

            //NOW CHECK IF TARGET NUMBER IS RELIANCE CDMA THEN CHANGE THE SENDER ID TO FIXED ID, AS RELIANCE CDMA DOES NOT SUPPORT ALPHANUMERIC SENDER ID
            if ((strRecip.Substring(0, 4) == "9193"))
            {
                strSender = "919860609000";
            }*/

            //Uri objURI = new Uri("http://sms.sms2india.info/sendsms.asp?user=USERNAME&password=PASSWORD&sender=" + strSender + "&" + "PhoneNumber=" + strRecip + "&Text=" + HttpUtility.UrlEncode(strMsgText) + "&track=1)");
            //Uri objURI = new Uri("http://sms.sms2india.info/sendsms.asp?user=LWTPL&password=123456&sender=" + strSender + "&" + "PhoneNumber=" + strRecip + "&Text=" + strMsgText + "&track=1)");
            Uri objURI = null;
            if (ConfigurationManager.AppSettings["URIConfig"] == "0")
            {
                string strSender = "SenderID";

                //NOW CHECK IF TARGET NUMBER IS RELIANCE CDMA THEN CHANGE THE SENDER ID TO FIXED ID, AS RELIANCE CDMA DOES NOT SUPPORT ALPHANUMERIC SENDER ID 
                if ((strRecip.Substring(0, 4) == "9193"))
                {
                    strSender = "919860609000";
                }

                objURI = new Uri("http://sms.sms2india.info/sendsms.asp?user=stpl&password=stpl123&sender=" + strSender + "&countrycode=91&" + "PhoneNumber=" + strRecip + "&Text=" + strMsgText + "&track=1)");
            }
            else
            {
                //#CC04:Commented objURI = new Uri(ConfigurationManager.AppSettings["MainURI"] + ConfigurationManager.AppSettings["URIUserIDPRM"] + "&" + ConfigurationManager.AppSettings["URIPassPRM"] + "&" + ConfigurationManager.AppSettings["URISenderPRM"] + "&countrycode=91&" + "PhoneNumber=" + strRecip + "&text=" + strMsgText + "&gateway=UES3B2ZX");

                /*#CC04:Added start here*/
                string URIPassPRM, Other1, Other2, Other3, FinalString = null, URISendToPRM;
                URIPassPRM = ConfigurationManager.AppSettings["URIPassPRM"];
                Other1 = ConfigurationManager.AppSettings["Other1"];
                Other2 = ConfigurationManager.AppSettings["Other2"];
                Other3 = ConfigurationManager.AppSettings["Other3"];
                URISendToPRM = ConfigurationManager.AppSettings["URISendToPRM"];
                if (URIPassPRM.Length > 0)
                {
                    FinalString = "&" + URIPassPRM;

                }
                if (Other1.Length > 0)
                {
                    FinalString = FinalString + "&" + Other1;
                }
                if (Other3.Length > 0)
                {
                    FinalString = FinalString + "&" + Other3;

                }
                if (Other2.Length > 0)
                {
                    FinalString = FinalString + "&" + Other2;

                }
                 if (URISendToPRM.Length > 0)
                {
                    FinalString = FinalString + "&" + URISendToPRM +strRecip;

                }
              

                objURI = new Uri(ConfigurationManager.AppSettings["MainURI"] + ConfigurationManager.AppSettings["URIUserIDPRM"] + "&" + ConfigurationManager.AppSettings["URISenderPRM"] +"&text="+strMsgText+ FinalString);

                /*#CC04:Added end here*/

            }
            return objURI;
        }

        /// <summary>
        /// Created By AmitB on 21-Apr-2011 To send mails
        /// </summary>
        /// <returns>List of log</returns>

        public List<string> SendEmails()
        {
            List<string> lstSendLog = null; // Mail Error
            try
            {
                using (clsEmailOutboundLog outboundEmail = new clsEmailOutboundLog())
                {
                    List<string> lstLogoutboundID = null; // Mail sent to id's
                    List<string> lstAttachments = null; // File Attachment list
                    List<string> lstLogoutboundIDAll = null; //#CC01 added
                    WriteLogToTextFile("Fatch Email list");
                    System.Data.DataTable dtResult = outboundEmail.SelectPending();
                    WriteLogToTextFile("Email data fatched");
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        WriteLogToTextFile("Email data found" + dtResult.Rows.Count.ToString());

                        string strAttachmentPath = Convert.ToString(ConfigurationManager.AppSettings["AttachmentFilePath"]);
                        foreach (System.Data.DataRow dr in dtResult.Rows)
                        {
                            using (EmailUtility email = new EmailUtility())
                            {
                                email.ToEmailID = Convert.ToString(dr["EmailTo"]);
                                email.FromEmailID = Convert.ToString(ConfigurationManager.AppSettings["mailfrom"]);
                                email.CcEmailID = Convert.ToString(dr["EmailCC"]);
                                email.FromName = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);/*"Gionee" #CC05:Added/Commented */ 
                                email.IsBodyHtml = true;
                                //  Convert.ToString(ConfigurationManager.AppSettings["EmailSubject"]);
                                email.Subject = Convert.ToString(dr["EmailSubject"]);
                                email.Body = Convert.ToString(dr["EmailBody"]);
                                if (dr["EmailAttachmentNames"] != DBNull.Value && Convert.ToString(dr["EmailAttachmentNames"]) != string.Empty)
                                {
                                    foreach (string strAttachment in Convert.ToString(dr["EmailAttachmentNames"]).Split(','))
                                    {
                                        if (lstAttachments == null)
                                            lstAttachments = new List<string>();

                                        lstAttachments.Add(strAttachmentPath + strAttachment);
                                    }
                                    email.AttachmentFiles = lstAttachments;
                                }

                                if (lstLogoutboundIDAll == null)
                                    lstLogoutboundIDAll = new List<string>();
                                lstLogoutboundIDAll.Add(Convert.ToString(dr["EmailOutboundLogID"]));
                             /*   #cc06 added start*/
                                if (Convert.ToString(ConfigurationManager.AppSettings["bypassGmailCertificate"]) == "1")
                                {
                                    ServicePointManager.ServerCertificateValidationCallback =
        delegate(object s, X509Certificate certificate,
                 X509Chain chain, SslPolicyErrors sslPolicyErrors)
        { return true; };
                                }
                                /*   #cc06 added end*/
                                //#cc01 added end
                                if (email.SendMail())
                                {
                                    if (lstLogoutboundID == null)
                                        lstLogoutboundID = new List<string>();

                                    lstLogoutboundID.Add(Convert.ToString(dr["EmailOutboundLogID"]));

                                    if (lstSendLog == null)
                                        lstSendLog = new List<string>();

                                    lstSendLog.Add(string.Format("Email sent successfully to {0} for LogID {1}", dr["EmailTo"], dr["EmailOutboundLogID"]));
                                }
                                else
                                {
                                    if (lstSendLog == null)
                                        lstSendLog = new List<string>();

                                    lstSendLog.Add(email.Error);
                                }
                            }
                        }

                        if (lstLogoutboundID != null && lstLogoutboundID.Count > 0)
                        {
                            string strEmailOutboundCallIds = string.Join(",", lstLogoutboundID.ToArray());
                            //string strEmailOutboundCallIdsAll = string.Join(",", lstLogoutboundIDAll.ToArray());
                            outboundEmail.EmailOutboundCallIdsAll = string.Join(",", lstLogoutboundIDAll.ToArray());
                            //int result = outboundEmail.Update(strEmailOutboundCallIds, strEmailOutboundCallIdsAll);
                            int result = outboundEmail.Update(strEmailOutboundCallIds);
                            if (result == 0)
                            {
                                lstSendLog.Add(string.Format("Records successfully updated for ({0})", strEmailOutboundCallIds));
                            }
                            else
                            {
                                lstSendLog.Add(string.Format("Unable to update email sent records for ({0})", strEmailOutboundCallIds));
                            }
                        }
                    }
                    else
                    {
                        WriteLogToTextFile("No data found");
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogToTextFile(ex.Message);
                if (lstSendLog == null)
                {
                    lstSendLog = new List<string>();
                    lstSendLog.Add(ex.Message);
                }
            }
            return lstSendLog;
        }

        public static void WriteLogToTextFile(string Message)
        {
            string strLogFileName = "SMSLog.txt";
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + strLogFileName))
            {
                File.Create(AppDomain.CurrentDomain.BaseDirectory + strLogFileName);
            }

            using (StreamWriter sWriter = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + strLogFileName))
            {
                sWriter.Write("\r\n{0}: ", DateTime.Now);
                sWriter.WriteLine("\r\n{0}", Message);
                sWriter.WriteLine("--------------------------------------------------------------");
                sWriter.Flush();
            }
        }

        public void SMSsenderOutBoundNew()
        {
            try
            {
                string strTestMessage = string.Empty;
                /* #CC02: commented.   strToken = objSMSService.login_authentication(strServiceUserID, strServicePassword);
                   strTestMessage = strTestMessage + "..Token.. " + strToken;
                   */

                // #CC02: added (start).
                using (clsSMSOutboundLog objSMSOutbound = new clsSMSOutboundLog())
                {
                    objSMSOutbound.Status = 0;  //For getting all the outbound undelivered calls
                    int counter = 0;
                    strTestMessage = strTestMessage + "Fatching data.. ";
                    DataTable dt = objSMSOutbound.GetUndeliveredSMSListInbound();
                    strTestMessage = strTestMessage + "data Fatch.. ";
                    if (dt.Rows.Count > 0)
                    {
                        strTestMessage = strTestMessage + "Record found.. " + dt.Rows.Count;
                        foreach (DataRow dr in dt.Rows)
                        {
                            int intOutboundLogID = Convert.ToInt32(dr["SMSOutBoundLogID"]);
                            if (Convert.ToString(dr["SMSMobileNumber"]) != "")
                            {
                                bool blTransStatus = SendSMS(Convert.ToString(dr["SMSMobileNumber"]), Convert.ToString(dr["SendText"]));
                                //bool blTransStatus = SendSMS("9990182470", Convert.ToString(dr["SendText"]));

                                strTestMessage = strTestMessage + "..mobile:" + Convert.ToString(dr["SMSMobileNumber"]) + " status :" + blTransStatus.ToString();

                                //if (blTransStatus)                // #CC03: if condition commented.
                                //{
                                //strTestMessage = strTestMessage + "..intOutboundLogID" + Convert.ToString(intOutboundLogID);
                                //objSMSOutbound.SMSProcessedUpdateOutBound(strTransNo, intOutboundLogID);

                                objSMSOutbound.IsDelivered = Convert.ToInt16((blTransStatus == true) ? 1 : 2);      // #CC03: added.

                                objSMSOutbound.SMSProcessedUpdateOutBound(string.Empty, intOutboundLogID);
                                counter = counter + 1;
                                //}                                 // #CC03: if condition commented.
                            }
                        }
                        //return strTestMessage; //+"SMS Send Succefully";
                        WriteLogToTextFile(strTestMessage);
                    }
                    else
                    {
                        WriteLogToTextFile("No Data To send SMS");
                        //return strTestMessage;// +"No Data To send SMS";
                    }
                }
                // #CC02: added (end).
            }
            catch (Exception ex)
            {
                WriteLogToTextFile(ex.Message);
                //strTestMessage = strTestMessage + "Error:" + ex.Message;
                //return strTestMessage;
            }
            //return strTestMessage;
        }
    }
}