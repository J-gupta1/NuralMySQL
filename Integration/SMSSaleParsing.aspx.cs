using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using System.Data.SqlClient;
using System.Data;

public partial class Integration_SMSSaleParsing : System.Web.UI.Page
{
    string strCustomerMobileNumber, strSerialNumberWithModelContent;
    SqlParameter[] objSqlParam;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["mb"] != null && Request.QueryString["mb"].ToString() != string.Empty)
                        strCustomerMobileNumber = HttpUtility.HtmlDecode(Request.QueryString["mb"].ToString());

                    if (Request.QueryString["ms"] != null && Request.QueryString["ms"].ToString() != string.Empty)
                        strSerialNumberWithModelContent = HttpUtility.HtmlDecode(Request.QueryString["ms"].ToString());

                    Int32 result;
                    using (TempClass ObjSMS = new TempClass())
                    {
                        //ObjSMS.MobileNumber = strCustomerMobileNumber.Substring(2,strCustomerMobileNumber.Length - 2);
                        ObjSMS.MobileNumber = strCustomerMobileNumber;
                        ObjSMS.SerialNumberWithModelContent = strSerialNumberWithModelContent;
                        ObjSMS.SMSContent = Request.QueryString.ToString();
                        result = ObjSMS.InsertTertiorySalesPostThruSMS();
                        using (SMS ObjSMSSent = new SMS())
                        {
                            if (result == 2)
                            {
                                ObjSMSSent.SMSContent = ObjSMS.error;
                                ObjSMSSent.MobNumber = strCustomerMobileNumber;
                                ObjSMSSent.SendSMS();
                                if (ObjSMSSent.strSMSResponse != null)
                                {
                                    if (ObjSMSSent.strSMSResponse.Contains("Message Accepted"))
                                        ObjSMSSent.IsSent = 1;
                                    else
                                        ObjSMSSent.IsSent = 2;
                                }
                                else
                                {
                                    ObjSMSSent.IsSent = 0;
                                }
                                ObjSMSSent.MobNumber = strCustomerMobileNumber;
                                ObjSMSSent.SMSContent = ObjSMS.error;
                                ObjSMSSent.TransactionID = ObjSMS.TransactionID;
                                ObjSMSSent.UpdateSMSOutboundLog();
                            }
                        }
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
