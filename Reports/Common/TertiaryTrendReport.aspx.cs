#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2010 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Created By : Sumit Maurya
* Created On: 16-Nov-2016 
* ====================================================================================================
* Reviewed By :
* DD-MMM-YYYY, Name, #CCXX, Description
 ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 *  02-March-2019,Vijay Kumar Prajapati,#CC01,Added method For Get Brand.
 ====================================================================================================
*/

#endregion
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


public partial class Reports_Common_TertiaryTrendReport : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ucFromDate.Date = PageBase.Fromdate;
                ucToDate.Date = PageBase.ToDate;
                Brand();/*#CC01 Added*/
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
                ob.DateFrom = Convert.ToDateTime(ucToDate.Date);
                DataSet ds = ob.TertiaryCount();

            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            ucMessage.Visible = false;
            using (ReportData ob = new ReportData())
            {
                ob.FromDate = ucFromDate.Date;
                ob.ToDate = ucToDate.Date;
                ob.UserId = PageBase.UserId;
                ob.BrandId = Convert.ToInt32(ddlBrand.SelectedValue);/*#CC01 Added*/
                DataSet ds = ob.GetTertiaryTrendReport();
                if (ob.Result == 0)
                {
                    if (ob.TotalRecords > 0)
                    {
                        String FilePath = Server.MapPath("~/");
                        string FilenameToexport = "TertiaryTrendReport";
                        PageBase.RootFilePath = FilePath;
                        PageBase.ExportToExecl(ds, FilenameToexport);
                    }
                    else
                    {
                        ucMessage.Visible = true;
                        ucMessage.ShowInfo("No record found.");
                    }
                }
                else if (ob.Result == 1 && ob.error != "")
                {
                    ucMessage.ShowError(ob.error);
                }
                else
                {
                    ucMessage.ShowInfo("Error occured try again later.");
                }

            }
        }
        catch (Exception ex)
        {
            // PageBase.Errorhandling(ex);
        }
    }
    protected void ddlDayRange_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlDayRange.SelectedValue == "0")
                ucFromDate.Date = Convert.ToString(DateTime.Now.AddDays(-Convert.ToInt64(ddlDayRange.SelectedValue)));
            else
                ucFromDate.Date = Convert.ToString(DateTime.Now.AddDays(-Convert.ToInt64(ddlDayRange.SelectedValue) + 1));
        }
        catch (Exception ex)
        {
            ucMessage.ShowError(ex.Message);
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("TertiaryTrendReport.aspx", false);
        }
        catch (Exception ex)
        {
            ucMessage.ShowError(ex.Message);
        }
    }
    /*#CC01 Added*/
    void Brand()
    {
        using (TempClass ObjBrand = new TempClass())
        {

            ddlBrand.Items.Clear();
            string[] str = { "BrandID", "BrandName" };
            PageBase.DropdownBinding(ref ddlBrand, ObjBrand.GetBrand(), str);

        };
    }
    /*#CC01 Added End*/
}
