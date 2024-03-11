using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;
using System.Data;
using System.Configuration;

public partial class Transactions_BulkUploadGRNPrimary_BulkFileStatus : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Page.Header.DataBind();
            ucMsg.Visible = false;
            if (!IsPostBack)
            {

                TextBox txtDate = (TextBox)ucDatePicker.FindControl("txtDate");
                txtDate.Text = DateTime.Now.Date.ToString();

                TextBox txtDateto = (TextBox)ucDatePickerTo.FindControl("txtDate");
                txtDateto.Text = DateTime.Now.Date.ToString();


            }

        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            ucMsg.ShowError(ex.Message, GlobalErrorDisplay());
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Response.Redirect("BulkFileStatus.aspx");
    }

    bool isvalidate()
    {
        TextBox txtDate = (TextBox)ucDatePicker.FindControl("txtDate");
        TextBox txtDateto = (TextBox)ucDatePickerTo.FindControl("txtDate");

        if (txtDate.Text != "" && txtDateto.Text != "")
        {
            if (Convert.ToDateTime(txtDate.Text) > Convert.ToDateTime(txtDateto.Text))
            {
                ucMsg.ShowError("The From date should not be greater than To date.");
                return false;
            }
        }


        return true;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            ucMsg.Visible = false;
            grdvwFile.DataSource = null;
            grdvwFile.DataBind();
            if (isvalidate())
            {
                using (SalesChannelData ObjSalesChannel = new SalesChannelData())
                {
                    string BulkFileURL = ConfigurationManager.AppSettings["ExcelBulkUploadPathStatus"].ToString().Trim();
                    string siteurl = ConfigurationManager.AppSettings["siteurl"].ToString().Trim();
                    TextBox txtDate = (TextBox)ucDatePicker.FindControl("txtDate");
                    TextBox txtDateto = (TextBox)ucDatePickerTo.FindControl("txtDate");
                    ObjSalesChannel.Fromdate = Convert.ToDateTime(txtDate.Text);
                    ObjSalesChannel.Todate = Convert.ToDateTime(txtDateto.Text);
                    ObjSalesChannel.FileURL = siteurl+BulkFileURL;
                    DataSet ds = ObjSalesChannel.GetAllBulkFile();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ViewState["BulKFileDetailId"] = null;
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            ViewState["BulKFileDetailId"] = ds.Tables[1];
                        }
                        grdvwFile.DataSource = ds.Tables[0];
                        grdvwFile.DataBind();

                    }
                    else
                    {
                        ucMsg.ShowInfo("Record not Found.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            ucMsg.ShowError(ex.Message, GlobalErrorDisplay());
        }
    }
    protected void grdvwFile_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ViewState["BulKFileDetailId"] != null)
                {
                    DataTable dt = (DataTable)ViewState["BulKFileDetailId"];
                    if (dt.Rows.Count > 0)
                    {
                        DataView dv = new DataView(dt);
                        dv.RowFilter = "BulKFileId=" + grdvwFile.DataKeys[e.Row.RowIndex].Value;
                        GridView gvChild = (GridView)e.Row.FindControl("gvChildGrid");
                        DataTable dtResult = dv.ToTable();
                        if (dtResult.Rows.Count > 0)
                        {
                            gvChild.DataSource = dtResult;
                            gvChild.DataBind();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            ucMsg.ShowError(ex.Message, GlobalErrorDisplay());
        }
    }


}
