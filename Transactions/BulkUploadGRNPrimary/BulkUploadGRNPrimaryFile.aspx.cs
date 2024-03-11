#region Copyright© 2012 Zed-Axis Technologies All rights are reserved
/*
================================================================================================
COPYRIGHT (c) 2012 Zed Axis Technologies (P) Ltd.
ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART,
ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE,
WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
================================================================================================
Created By :  Sumit Kumar
Modified BY : Karam Chand Sharma
Role : Software Engineer
Module : Bulh Upload
Description : User upload file with respect to NDS and Warehouse
Reviewed By :
================================================================================================
 * 28 Apr 2015, Karam Chand Sharma, #CC01, Reset Session value and reset grid binding
 * 01-Jun-2016, Sumit Maurya, #CC02 , New Code added to show Error msg while uploading data from excel.
 * 06-Jun-2016, Sumit Maurya, #CC03, New link button added to download Active SKU details.
 * 07-Jun-2016, Sumit Mauyra, #CC04, ViewState of usercontrol is set null when records gets saved.
*/

#endregion
#region Using
using System.Data;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Resources;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;
using System.Text;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

public partial class Transactions_SalesChannelSB_Interface_BulkUploadGRNPrimaryFile : PageBase
{
    protected string strSiteUrl = PageBase.siteURL;
    protected string strAssets = PageBase.strAssets;
    private bool IsOpeningdateEnable = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Page.Header.DataBind();
            ucMsg.Visible = false;
            if (!IsPostBack)
            {
                BindWarehouseNDS();
                TextBox txtDate = (TextBox)ucDatePicker.FindControl("txtDate");
                txtDate.Text = DateTime.Now.Date.ToString();
            }
            FillDate();
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            ucMsg.ShowError(ex.Message, GlobalErrorDisplay());
        }
    }

    void BindWarehouseNDS()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            string[] strWarehouse = { "SalesChannelID", "SalesChannelName" };
            DataSet dsData = ObjSalesChannel.GetWarehouseAndNDS();
            PageBase.DropdownBinding(ref ddlWarehouse, dsData.Tables[0], strWarehouse);

            string[] strNDS = { "SalesChannelID", "SalesChannelName" };
            PageBase.DropdownBinding(ref ddlNDS, dsData.Tables[1], strNDS);

            string[] strVendor = { "VendorID", "VendorName" };
            PageBase.DropdownBinding(ref drpVender, dsData.Tables[2], strVendor);
        }
    }

    void FillDate()
    {
        ucDatePicker.TextBoxDate.Enabled = true;
        ucDatePicker.imgCal.Enabled = true;
        ucDatePicker.MinRangeValue = DateTime.MinValue;
        ucDatePicker.MaxRangeValue = DateTime.Now.Date;
        ucDatePicker.RangeErrorMessage = "Only Back days allowed";
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Response.Redirect("BulkUploadGRNPrimaryFile.aspx");
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            ucMsg.Visible = false;
            GridView grdvwFile = (GridView)ucUploadMultipleExcelFile1.FindControl("grdvwFile");
            if (grdvwFile.Rows.Count > 0)
            {
                if (txtInvoiceNo.Text.Length == 0)
                {
                    ucMsg.ShowInfo("Please Enter Invoice Number.");
                    return;
                }
                using (SalesChannelData ObjSalesChannel = new SalesChannelData())
                {
                    ObjSalesChannel.InvoiceNo = txtInvoiceNo.Text.Trim();
                    if (ObjSalesChannel.ChkDuplicateInvoiceNo())
                    {
                        txtInvoiceNo.Text = "";
                        txtInvoiceNo.Focus();
                        ucMsg.ShowWarning("Invoice number already exists.");
                        return;
                    }
                }
                WarehouseTranaction objWarehouse = new WarehouseTranaction();
                objWarehouse.Upload_date = ((TextBox)ucDatePicker.FindControl("txtDate")).Text;
                objWarehouse.Upload_Warehouse = ddlWarehouse.SelectedValue.Trim();
                objWarehouse.Upload_NDS = ddlNDS.SelectedValue.Trim();
                objWarehouse.Upload_InvoiceNo = txtInvoiceNo.Text.Trim();
                objWarehouse.Upload_CreatedBy = PageBase.UserId;
                objWarehouse.Upload_TotalFile = grdvwFile.Rows.Count;
                /* objWarehouse.VendorID = Convert.ToInt32(ddlNDS.SelectedValue); #CC03 Commented */
                /*#CC02 Added */
                objWarehouse.VendorID = Convert.ToInt32(drpVender.SelectedValue); /* #CC03 Added */
                DataTable dtType = new DataTable();
                dtType.Columns.Add("FileName", typeof(string));
                dtType.Columns.Add("SystemFileName", typeof(string));

                for (int i = 0; i < grdvwFile.Rows.Count; i++)
                {
                    string PhysicalPath = ((HiddenField)grdvwFile.Rows[i].FindControl("hdnPath")).Value.Trim();
                    string lblFileName = ((Label)grdvwFile.Rows[i].FindControl("lblFileName")).Text.Trim();
                    dtType.Rows.Add(lblFileName, PhysicalPath);
                }

                objWarehouse.Upload_dt = dtType;

                int Result = objWarehouse.UploadBulkGRNandPrimaryFile();

                if (Result == 0)
                {
                    TextBox txtDate = (TextBox)ucDatePicker.FindControl("txtDate");
                    txtDate.Text = DateTime.Now.Date.ToString();
                    /*#CC01 COMMENTED grdvwFile.DataSource = null;
                    grdvwFile.DataBind();
                    Session["FileInfo"] = null;*/
                    ddlNDS.SelectedIndex = 0;
                    ddlWarehouse.SelectedIndex = 0;
                    txtInvoiceNo.Text = "";
                    ucMsg.ShowSuccess("Files has been uploaded successfully.");

                    /* #CC04 Add Start */
                    drpVender.SelectedIndex = 0;
                    Session["DTCheckSKUCartonTemp"] = null;
                    Session["DTCheckSKUCarton"] = null;
                    /* #CC04 Add End */
                }
                else
                {
                    ucMsg.ShowError("Files not uploaded.");
                }
            }
            else
            {
                ucMsg.ShowError("Please Add atleast one file.");
            }
            /*#CC01 START ADDED*/
            grdvwFile.DataSource = null;
            grdvwFile.DataBind();
            Session["FileInfo"] = null;
            /*#CC01 START END*/
        }
        catch (Exception ex)
        {
            Session["FileInfo"] = null;/*#CC01 ADDED*/
            PageBase.Errorhandling(ex);
            ucMsg.ShowError(ex.Message, GlobalErrorDisplay());
        }
    }

    /* #CC02 Add Start */
    public void DisplayMessage(string msgtype, string message)
    {
        try
        {

            if (msgtype.ToLower() == "showxmlerror")
            {
                ucMsg.XmlErrorSource = message;
            }
            if (msgtype.ToLower() == "showxmlinfo")
            {
                ucMsg.XmlErrorSource = message;
            }

        }
        catch (Exception ex)
        {

        }
    }
    /* #CC02 Add End */

    /* #CC03 Add Start */
    protected void lnkReferanceCode_Click(object sender, EventArgs e)
    {
        try
        {
            //WarehouseTranaction objWareHouse = new WarehouseTranaction();
            WarehouseTranaction objWareHouse = new WarehouseTranaction();
            DataSet ds = new DataSet();
            ds = objWareHouse.GetManageRetailer();
            ds.Tables[0].Columns.Remove("SKUID");
            if (ds.Tables[0].Rows.Count > 0)
            {
                PageBase.ExportToExecl(ds, "SKU Referance Details.");
            }
            else
            {
                ucMsg.ShowInfo("No Record Found");
            }
        }
        catch (Exception ex)
        {
        }
    }
    /* #CC03 Add End */




}
