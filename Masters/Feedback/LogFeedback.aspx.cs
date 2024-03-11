/*
====================================================================================================================================
Copyright	: Zed-Axis Technologies, 2016
Created By	: Sumit Maurya
Create date	: 16-Mar-2016
Description	: This interface Log Feedback.
Module      : Feedback creation.
====================================================================================================================================
Change Log:
DD-MMM-YYYY, Name , #CCXX - Description
------------------------------------------------------------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DataAccess;
using BussinessLogic;


public partial class Masters_Feedback_LogFeedback : PageBase //System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        if (!IsPostBack)
        {



        }
    }

    protected void btnSaveFeedback_Click(object sender, EventArgs e)
    {
        try
        {
            Feedback objfeedback = new Feedback();
            objfeedback.FeedbackText = txtfeedback.TextBoxText.Trim();
            objfeedback.web_user_id = Convert.ToInt32(Session["UserID"]);
            int result = objfeedback.SaveFeedback();
            if (result == 0)
            {
                ucMessage1.ShowSuccess("Feedback created successfully.");
                txtfeedback.TextBoxText = string.Empty;                
            }
            else
            {
                ucMessage1.ShowInfo("Error in creating feedback.");
            }

        }
        catch (Exception ex)
        {
            /*
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
             * */
        }
    }
}
