/*
 *==================================================================================================== 
 * Change Log:
 * ----------
 * DD-MMM-YYYY, Name, #CCXX, Description.
 * 30-Nov-2015, Sumit Maurya, #CC01, New Labels added and binded to display "SMSLastDate" and "WebLastDate"
 * 02-Nov-2016, Karam Chand Sharma, #CC02, Tertiary count of two back dates from given date will be displayed in the grid . In grid view data will be display 
 * 27-Feb-2019,Vijay Kumar Prajapati,#CC03,Added method For Get Brand.

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


public partial class Reports_Common_TertiaryCount : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ucDatePicker1.Date = PageBase.ToDate;
                Brand();/*#CC03 Added*/
                btnSerch_Click(null, null);

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnSerch_Click(object sender, EventArgs e)
    {
        try
        {
            using (TempClass ob = new TempClass())
            {
                ob.DateFrom = Convert.ToDateTime(ucDatePicker1.Date);
                ob.BrandId = Convert.ToInt32(ddlBrand.SelectedValue);/*#CC03 Added*/
                DataSet ds = ob.TertiaryCount();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblSMSActivation.Text = ds.Tables[0].Rows[0]["SMSActivationCount"].ToString();
                    lblWebActivation.Text = ds.Tables[0].Rows[0]["WebActivationCount"].ToString();
                    lblTertiaryConsidered.Text = ds.Tables[0].Rows[0]["TertiaryConsideredCount"].ToString();
                    /* #CC01 Add Start*/
                    LblSmsLastDate.Text = String.Format("{0:dd MMM yyyy} {0:T} ", Convert.ToDateTime(ds.Tables[0].Rows[0]["SMSLastDate"].ToString()));
                    LblWebLastDate.Text = String.Format("{0:dd MMM yyyy} {0:T} ", Convert.ToDateTime(ds.Tables[0].Rows[0]["WebLastDate"].ToString()));
                    /* #CC01 Add End*/
                    /*#CC02 ADDED START*/
                    grdvwTertairyCount.DataSource = ds.Tables[1];
                    grdvwTertairyCount.DataBind();
                    /*#CC02 ADDED END*/

                    lblTIme.Text = "Last One Hour Trend ( " + ds.Tables[2].Rows[0]["OneHoueBeforTime"].ToString() + " to " + ds.Tables[2].Rows[0]["CurrentTime"].ToString() + " )";
                   
                }
                else
                {
                    lblSMSActivation.Text = "0";
                    lblWebActivation.Text = "0";
                    lblTertiaryConsidered.Text = "0";
                    /* #CC01 Add Start*/
                    LblSmsLastDate.Text = "0";
                    LblWebLastDate.Text = "0";
                    /* #CC01 Add End*/
                    /*#CC02 ADDED START*/
                    grdvwTertairyCount.DataSource = null;
                    grdvwTertairyCount.DataBind();
                    /*#CC02 ADDED END*/
                    dvTime.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
    }
    /*#CC03 Added*/
    void Brand()
    {
        using (TempClass ObjBrand = new TempClass())
        {

            ddlBrand.Items.Clear();
            string[] str = { "BrandID", "BrandName" };
            PageBase.DropdownBinding(ref ddlBrand, ObjBrand.GetBrand(), str);

        };
    }
    /*#CC03 Added End*/
}
