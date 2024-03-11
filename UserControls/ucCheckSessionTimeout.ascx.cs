/*/
* ===================================================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2016 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ===================================================================================================================================
* Created By    : Sumit Maurya
* Created on    : 19-Apr-2016 
* Description   : This usercontrol is used to check session time , and will redirect to logout page when there is no postback/event execution before session timeout.
* ===================================================================================================================================
* Reviewed By   : 
 ====================================================================================================================================
 * Change Log:
 * DD-MMM-YYYY, Name, #CCXX, Description.
 * 24-05-2018,Vijay Kumar Prajapati,#CC01 -Added session for when login using OTP.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;

public partial class UserControls_ucCheckSessionTimeout : System.Web.UI.UserControl
{

    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            hdnRedirectToPage.Value = PageBase.siteURL + "logout.aspx";
            hdnSessionTimeout.Value = Convert.ToString(SessionTimeOutWithin * 60);
        }
        catch (Exception ex)
        {

        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            /*#CC01 Added Start*/
            if (Convert.ToString(HttpContext.Current.Session["UserLogingUsingOTP"]) == "1" && Convert.ToString(HttpContext.Current.Session["OTPVerified"]) == "0")
            {
                Response.Redirect("~/logout.aspx", false);
            }
            /*#CC01 Added End*/
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "checkinterval();", true);
            hdnSessionTimeout.Value = Convert.ToString(SessionTimeOutWithin * 60);
        }
        catch (Exception ex)
        {

        }

    }

    public int SessionTimeOutWithin
    {
        get { return Session.Timeout; }
    }



}