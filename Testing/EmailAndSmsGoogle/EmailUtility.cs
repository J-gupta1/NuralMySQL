#region Copyright(c) 2008 Zed-Axis Technologies All rights are reserved
/*/
 * ====================================================================================================
 *  <copyright company="Zed Axis Technologies">
 *      COPYRIGHT (c) 2008 Zed Axis Technologies (P) Ltd. 
 *      ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
 *      ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
 *      WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
 *  </copyright>
 * ====================================================================================================
 * Created By   : Amit Bhardwaj
 * Role         : SE
 * Module       : Mail Management Liabrary
 * FileName     : EmailUtility.cs
 * Description  : Common class with the global static functions to manage mailing.
 * ====================================================================================================
     Modification On       Modified By          Modification    
    ---------------      -----------          -------------------------------------------------------------  
 * 21-July-2015,Shashikant Singh ,  #CC01: Added condition for missing dynamic Configuration such as EnableSsl
 * 
 * ====================================================================================================
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Configuration;

namespace EmailSMSApp
{
    public class EmailUtility : IDisposable
    {
        public string FromEmailID { get; set; }
        public string FromName { get; set; }
        public string ToEmailID { get; set; }
        public string CcEmailID { get; set; }
        public string BccEmailID { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
        public List<String> AttachmentFiles { get; set; }
        public string Error { get; set; }

        #region Dispose
        private bool IsDisposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~EmailUtility()
        {
            Dispose(false);
        }

        //Implement dispose to free resources
        protected virtual void Dispose(bool disposedStatus)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                if (disposedStatus)
                {
                    // Released managed Resources
                }
            }
        }
        #endregion
      
        public bool SendMail()
        {
            MailMessage mail = new MailMessage();
            try
            {
                if (!String.IsNullOrEmpty(FromEmailID))
                {
                    mail.From = new MailAddress(FromEmailID, FromName);
                }
                else
                {
                    Error = "The Sender Address can't be empty.";
                    return false;
                }

                if (!String.IsNullOrEmpty(ToEmailID))
                {
                    if (ToEmailID.Contains(";"))
                    {
                        string[] str = ToEmailID.Split(';');
                        for (int i = 0; i < str.Length; i++)
                        {
                            if (str[i] != String.Empty)
                            {
                                mail.To.Add(str[i]);
                            }
                        }
                    }
                    else
                    {
                        mail.To.Add(ToEmailID);
                    }
                }
                else
                {
                    Error = "Recepient address can't be empty.";
                    return false;
                }

                if (!String.IsNullOrEmpty(BccEmailID))
                {
                    if (BccEmailID.Contains(";"))
                    {
                        string[] str = BccEmailID.Split(';');
                        for (int i = 0; i < str.Length; i++)
                        {
                            if (str[i].Trim() != String.Empty)
                            {
                                mail.Bcc.Add(str[i]);
                            }
                        }
                    }
                    else
                    {
                        mail.Bcc.Add(BccEmailID);
                    }
                }

                if (!String.IsNullOrEmpty(CcEmailID))
                {
                    if (CcEmailID.Contains(";"))
                    {
                        string[] str = CcEmailID.Split(';');
                        for (int i = 0; i < str.Length; i++)
                        {
                            if (str[i].Trim() != String.Empty)
                            {
                                mail.CC.Add(str[i]);
                            }
                        }
                    }
                    else
                    {
                        mail.CC.Add(CcEmailID);
                    }
                }

                mail.Subject = Subject;
                mail.Body = Body;
                mail.IsBodyHtml = IsBodyHtml;
                mail.Priority = MailPriority.Normal;
                 if (AttachmentFiles != null)
                {
                    foreach (string attachment in AttachmentFiles)
                    {
                        mail.Attachments.Add(new Attachment(attachment, MediaTypeNames.Application.Octet));
                    }
                }

                SmtpClient client = new SmtpClient();
                client.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]); /*#CC01:Added/commentedtrue*/
                //client.Credentials = new System.Net.NetworkCredential("gcaresupport@gionee.co.in", "Gcaresupport");

                client.Send(mail);
                //mail.Dispose();

                if (AttachmentFiles != null && AttachmentFiles.Count > 0)
                {
                    AttachmentFiles.Clear();
                }

                return true;
            }
            catch (Exception ex)
            {
                Error = ex.ToString();
                //mail.Dispose();
                return false;
            }
        }
    }

}
