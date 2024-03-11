using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Collections;
using System.Web;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace BussinessLogic
{
    public class Mailer
    {

        #region Class Public Properties, Members and Global Variables

        private static string _smtpHostServer = "";
        public static string smtpHostServer
        {
            get
            { return _smtpHostServer; }
            set
            { _smtpHostServer = value; }
        }
        private static int _smtpPort = 25;
        public static string Errorexception { get; set; }
        public static int smtpPort
        {
            get
            { return _smtpPort; }
            set
            { _smtpPort = value; }
        }
        private static string _smtpUserName = "";
        public static string smtpUserName
        {
            get
            { return _smtpUserName; }
            set
            { _smtpUserName = value; }
        }
        private static string _smtpPassword = "";
        public static string smtpPassword
        {
            get
            { return _smtpPassword; }
            set
            { _smtpPassword = value; }
        }
        private static int _smtpAuthentication = 0;
        public static int smtpAuthentication
        {
            get
            { return _smtpAuthentication; }
            set
            { _smtpAuthentication = value; }
        }
        private static string strPassword;
        public static string Password
        {
            get
            { return strPassword; }
            set
            { strPassword = value; }
        }
        private static string strLoginName;
        public static string LoginName
        {
            get
            { return strLoginName; }
            set
            { strLoginName = value; }
        }
        private static string strUserName;
        public static string UserName
        {
            get
            { return strUserName; }
            set
            { strUserName = value; }
        }
        private static string StrEmailID;
        public static string EmailID
        {
            get
            { return StrEmailID; }
            set
            { StrEmailID = value; }
        }
        #endregion
        public static String readHtmlPage(string url)
        {
            //string FileName = Server.MapPath(url);
            string FileName = (url);
            StreamReader ObjStrmReader;
            ObjStrmReader = File.OpenText(FileName);
            string strMail = ObjStrmReader.ReadToEnd();
            ObjStrmReader = null;
            return strMail;
        }
        public static bool fnc_SendMailerViaSMTP(string mailFrom, string mailFromName, string mailTo, string mailToName, string mailSubject, string mailFileName, bool mailHTML, Hashtable paramReplaceToWith, out string sErrorDesc)
        {
            string sError = ""; bool _FncExecute = false;
            try
            {
                string smailFrom = mailFrom.Trim();
                string MailBody = readHtmlPage(HttpContext.Current.Server.MapPath(mailFileName));

                if (paramReplaceToWith.Count >= 1)
                {
                    IDictionaryEnumerator myEnumerator = paramReplaceToWith.GetEnumerator();
                    while (myEnumerator.MoveNext())
                    { MailBody = MailBody.Replace(Convert.ToString(myEnumerator.Key), Convert.ToString(myEnumerator.Value)); }
                }

                MailBody = MailBody.Replace("'", "''");
                _FncExecute = fncSendContentMail(mailFrom, mailFromName, mailTo, mailToName, mailSubject, MailBody, mailHTML, out sErrorDesc);
            }
            catch (Exception ex)
            { sError = ex.Message; _FncExecute = false; }
            sErrorDesc = sError; return _FncExecute;
        }
        public static bool fncSendContentMail(string emailFrom, string emailFromName, string emailTo, string emailToName, string emailSubject, string msgBody, bool IsHTML, out string sErrorDesc)
        {
            string sServer = smtpHostServer.Trim(); int sPort = smtpPort;
            int sAuthentication = smtpAuthentication;
            string sUsername = smtpUserName; string sPassword = smtpPassword;
               string sError = ""; bool _FncExecute = false;
            try
            {
                SmtpClient MailSender = new SmtpClient();
                MailAddress mailto; MailAddress mailfrom;

                if (emailToName.Trim().Length >= 1)
                { mailto = new MailAddress(emailTo.Trim(), emailToName.Trim()); }
                else
                { mailto = new MailAddress(emailTo.Trim()); }
                if (emailFromName.Trim().Length >= 1)
                { mailfrom = new MailAddress(emailFrom.Trim(), emailFromName.Trim()); }
                else
                { mailfrom = new MailAddress(emailFrom.Trim()); }

                MailMessage MailMessage = new MailMessage(mailfrom, mailto);
                MailMessage.Subject = emailSubject;
                MailMessage.Body = msgBody.Trim();
                MailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                MailMessage.Priority = MailPriority.Normal;
                MailMessage.IsBodyHtml = IsHTML;
                MailSender.EnableSsl = true;
                if (sAuthentication == 1)
                {
                    MailSender.UseDefaultCredentials = false;
                    NetworkCredential info = new NetworkCredential(sUsername, sPassword);
                    MailSender.Credentials = info; MailSender.Host = sServer; MailSender.Port = sPort;
                }
                else
                { MailSender.UseDefaultCredentials = true; }
              
                ServicePointManager.ServerCertificateValidationCallback =
    delegate(object s, X509Certificate certificate,
             X509Chain chain, SslPolicyErrors sslPolicyErrors)
    { return true; };

                MailSender.Send(MailMessage);
                _FncExecute=true;
            }
            catch (Exception ex)
            { sError = ex.Message; _FncExecute = false; }
            sErrorDesc = sError; Errorexception = sErrorDesc; return _FncExecute;
        }
        public static bool sendmail(string MailerFileName)
        {
            bool sendStatus = false;
            try
            {
                string[] strAssets = PageBase.strAssets.Split(new char[] { '/' });
                string ErrDesc = string.Empty;
                Hashtable hasher = new Hashtable();
                hasher.Add("!!~image~!!", PageBase.siteURL + PageBase.strAssets);
                hasher.Add("!!~UserName~!!", strUserName);
                hasher.Add("!!~LoginName~!!", strLoginName);
                hasher.Add("!!~Password~!!", strPassword);
                hasher.Add("!!~siteurl~!!", PageBase.siteURL);
                hasher.Add("!!~website~!!", PageBase.siteURL + "Login/" + strAssets[1] + "/Login.aspx");
                sendStatus = fnc_SendMailerViaSMTP(PageBase.EmailIDFrom, PageBase.MailFrom, StrEmailID, strLoginName, PageBase.ForgotPasswordSubject, MailerFileName, true, hasher, out ErrDesc);
                return sendStatus;
            }
            catch
            {
                return false;
            }
        }
    }
}
