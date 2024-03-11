using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for clsSendOTP
/// </summary>
public class clsSendOTP : IDisposable
{
	public clsSendOTP()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public  string GenerateOTPNumber()
    {
        //string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
        string numbers = "1234567890";

        string characters = numbers;
        string otp = string.Empty;
        int length = 6;
        for (int i = 0; i < length; i++)
        {
            string character = string.Empty;
            do
            {
                int index = new Random().Next(0, characters.Length);
                character = characters.ToCharArray()[index].ToString();
            } while (otp.IndexOf(character) != -1);
            otp += character;
        }
       return otp;
    }
    public bool SendSMS(string strRecip, string strMsgText)
    {
        if (ConfigurationManager.AppSettings["HexConversionRequired"] == "1")
        {
            strMsgText = ConvertStringToHex(strMsgText);
        }
        Uri objURI = GetSMSURL(strRecip, strMsgText);
        WebRequest objWebRequest = WebRequest.Create(objURI);


        //if (ConfigurationManager.AppSettings["DebugMode"] == "1")
        //{
        //    WriteLogToTextFile(objURI.OriginalString);
        //}

        WebResponse objWebResponse;
        Stream objStream;
        string finalStatus = ConfigurationManager.AppSettings["SMSResultValue"]; 
        Boolean result = false; 

        try
        {

            objWebResponse = objWebRequest.GetResponse();
            objStream = objWebResponse.GetResponseStream();
            StreamReader objStreamReader = new StreamReader(objStream);
            string strHTML = objStreamReader.ReadToEnd();

            try
            {
                if (ConfigurationManager.AppSettings["SMSResultValueType"] == "1")
                {

                    string[] arrRes = System.Text.RegularExpressions.Regex.Split(strHTML, finalStatus);
                    //if (arrRes.Length > 1 && !string.IsNullOrEmpty(arrRes[1].Substring(0, 1)) && Convert.ToInt16(arrRes[1].Substring(0, 1)) > 0)
                    //{
                    //    result = true;
                    //}
                    if (arrRes.Length > 0)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else if (ConfigurationManager.AppSettings["SMSResultValueType"] == "0")
                {
                    result = strHTML.Contains(finalStatus);
                }
                return result;
                /*#CC04:Added end here*/
            }
            catch (Exception ex)
            {
                // ExceptionEmail(ex, "send sms", "SMS");//#CC07 Added
                return false;
            }
          
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
           // ExceptionEmail(wex, "send sms", "SMS");//#CC07 Added
        }
        return result;
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
            string URIPassPRM, Other1, Other2, Other3, FinalString = null, URISendToPRM, URIOtherURIPart;
            URIPassPRM = ConfigurationManager.AppSettings["URIPassPRM"];
            Other1 = ConfigurationManager.AppSettings["Other1"];
            Other2 = ConfigurationManager.AppSettings["Other2"];
            Other3 = ConfigurationManager.AppSettings["Other3"];
            URISendToPRM = ConfigurationManager.AppSettings["URISendToPRM"];
            URIOtherURIPart = ConfigurationManager.AppSettings["URIOtherURIPart"];
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
                FinalString = FinalString + "&" + URISendToPRM + strRecip;

            }


            objURI = new Uri(ConfigurationManager.AppSettings["MainURI"] + ConfigurationManager.AppSettings["URIUserIDPRM"] + "&" + ConfigurationManager.AppSettings["URISenderPRM"] + "&" + URIOtherURIPart + strMsgText + FinalString);

            /*#CC04:Added end here*/

        }
        return objURI;
    }
    public string ConvertStringToHex(String strMsg)
    {
        //Byte[] stringBytes = System.Text.Encoding.Unicode.GetBytes(strMsg);
        //StringBuilder sbBytes = new StringBuilder(stringBytes.Length * 2);

        //foreach (byte b in stringBytes)
        //{
        //    sbBytes.AppendFormat("{0:X2}", b);
        //}
        //return sbBytes.ToString();
        var hexString = new StringBuilder();
        foreach (var chr in strMsg)
        {
            hexString.Append(Convert.ToString(chr, 16).PadLeft(4, '0'));
        }
        return Convert.ToString(hexString);
    }
      #region Dispose
        private bool IsDisposed = false;

        //Call Dispose to free resources explicitly
        public void Dispose()
        {
            //Pass true in dispose method to clean managed resources too and say GC to skip finalize 
            // in next line.
            Dispose(true);
            //If dispose is called already then say GC to skip finalize on this instance.
            GC.SuppressFinalize(this);
        }

        ~clsSendOTP()
        {
            //Pass false as param because no need to free managed resources when you call finalize it
            //  will be done
            //by GC itself as its work of finalize to manage managed resources.
            Dispose(false);
        }

        //Implement dispose to free resources
        protected virtual void Dispose(bool disposedStatus)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                // Released unmanaged Resources
                if (disposedStatus)
                {
                    // Released managed Resources
                }
            }
        }
        #endregion
}