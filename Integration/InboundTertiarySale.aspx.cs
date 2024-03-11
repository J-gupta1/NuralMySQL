#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*/
 * ================================================================================================
 * ================================================================================================
 * COPYRIGHT (c) 2016 Zed Axis Technologies (P) Ltd.
 * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART,
 * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
 * ================================================================================================
 * Created By : Rakesh Raj
 * Created On : 30 Oct 2018
 * Description : This is used to manage Inbound Tertiary Sale from URL
 * ================================================================================================
 * Change Log: 
 * ------------- 
 * DD-MMM-YYYY, Name, #CCXX, Description
  
 * ====================================================================================================
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using System.Data.SqlClient;
using System.Data;

public partial class Integration_InboundTertiarySale : System.Web.UI.Page
{   
    string strRetailerCode, IMEICommaSeperated;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["rc"] != null && Request.QueryString["rc"].ToString() != string.Empty)
                        strRetailerCode = HttpUtility.HtmlDecode(Request.QueryString["rc"].ToString());

                    if (Request.QueryString["ms"] != null && Request.QueryString["ms"].ToString() != string.Empty)
                        IMEICommaSeperated = HttpUtility.HtmlDecode(Request.QueryString["ms"].ToString());

                                    
                    //Data will come in the string like this
                    //http://za-nb-138/ZedSalesV5/integration/InboundTertiarySale.aspx?rc=HARARYJULRT000043&ms=868750010052612,868750011052611
                    using (SMSData ObjSMS = new SMSData())
                    {
                        ObjSMS.MobileNumber = strRetailerCode;
                        ObjSMS.SerialNumberWithModelContent = IMEICommaSeperated;
                        ObjSMS.SMSContent = Request.QueryString.ToString();
                        ObjSMS.InboundTertiarySale();
                        
                        Response.Write(ObjSMS.Error);
                        
                    }


                }
                else
                    Response.Write("No query string found.");

            }

        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }

    }
}
