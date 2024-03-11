using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;

public partial class Integration_SMSHandling : System.Web.UI.Page
{
    string strMobileNumber, strMessageText,strCircle;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["mobilenumber"] != null && Request.QueryString["mobilenumber"].ToString() != string.Empty)
                       strMobileNumber= Request.QueryString["mobilenumber"].ToString();
                  
                    if (Request.QueryString["messagetext"] !=null && Request.QueryString["messagetext"].ToString() != string.Empty)
                       strMessageText= Request.QueryString["messagetext"].ToString();

                    if (Request.QueryString["circle"] != null && Request.QueryString["circle"].ToString() != string.Empty)
                        strCircle = Request.QueryString["circle"].ToString();

                    using (SMSData ObjSMS = new SMSData())
                    {
                        ObjSMS.MobileNumber = strMobileNumber;
                        ObjSMS.MessageText = strMessageText;
                        ObjSMS.CircleName = strCircle;
                        ObjSMS.insertSMSLog();
                        ObjSMS.SmsLogParser();
                    }
                    
          
                }
                else
                    Response.Write("No query string found .");

            }

        }
        catch (Exception ex)
        {

            Response.Write(ex.Message);
        }
    }
}
