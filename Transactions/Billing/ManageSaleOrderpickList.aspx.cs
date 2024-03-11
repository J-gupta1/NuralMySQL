#region Copyright(c) 2010 Zed-Axis Technologies All rights are reserved
/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2010 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Created By : Shashikant Singh
* Role : SSE
* Module : Sale Order PickList
* Description :.
* ====================================================================================================
* Reviewed By : Balram Jha
 ====================================================================================================
Change Log:
-------------------------------------------------------------------------------------------------------

 * ====================================================================================================
*/

#endregion
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Resources;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using ExportExcelOpenXML;
using ZedService;
using BussinessLogic;
using BusinessLogics;


public partial class Order_Common_ManageSaleOrderPickList : PageBase
{
    char[] separator = new char[] { ',' };

    string strUploadedFileName = string.Empty;

    string strDownloadPath = PageBase.strExcelPhysicalDownloadPathSB;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            fillOrderTo();
            div2.Visible = false;          
            ShowHideAutoAllocate();
             HyperLinkTempDownload.NavigateUrl = "../../../Excel/Templates/SalesOrderPickList.xlsx"; 
        }
    }

    public void fillOrderTo()
    {
        using (SalesChannelData objSaleschannel = new SalesChannelData())
        {
            try
            {
                cmbOrderTo.Items.Clear();
                DataTable dt;
                if (BaseEntityTypeID == 1)
                    objSaleschannel.SalesChannelID = PageBase.SalesChanelID;
                if (BaseEntityTypeID == 3)
                    objSaleschannel.RetailerID = 0;
                objSaleschannel.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
                objSaleschannel.UserID = PageBase.UserId;
                objSaleschannel.BaseEntityTypeID = Convert.ToInt16(PageBase.BaseEntityTypeID);
                objSaleschannel.GetParentOrChild = 1;

                dt = objSaleschannel.GetParentSalesChannel();
                cmbOrderTo.DataSource = dt;
                cmbOrderTo.DataTextField = "SalesChannelName";
                cmbOrderTo.DataValueField = "SalesChannelID";
                cmbOrderTo.DataBind();
                cmbOrderTo.Items.Insert(0, new ListItem("Select", "0"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbOrderTo.Attributes.Add("SalesChannelTypeID" + dt.Rows[i]["SalesChannelID"].ToString(), dt.Rows[i]["SalesChannelTypeID"].ToString());
                }
            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }

    public void  fillOrderFrom()
    {
        using (SalesChannelData objSaleschannel = new SalesChannelData())
        {
            try
            {
                if (Convert.ToInt32(cmbOrderTo.SelectedValue) > 0)
                {
                    cmbOrderFrom.Items.Clear();
                    DataTable dt;
                    if (BaseEntityTypeID == 1)
                        objSaleschannel.SalesChannelID = Convert.ToInt32(cmbOrderTo.SelectedValue);
                    if (BaseEntityTypeID == 3)
                        objSaleschannel.RetailerID = Convert.ToInt32(cmbOrderTo.SelectedValue);
                    objSaleschannel.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
                    objSaleschannel.UserID = PageBase.UserId;
                    objSaleschannel.BaseEntityTypeID = Convert.ToInt16(PageBase.BaseEntityTypeID);
                    objSaleschannel.GetParentOrChild = 0;
                    dt = objSaleschannel.GetParentSalesChannel();
                    cmbOrderFrom.DataSource = dt;
                    cmbOrderFrom.DataTextField = "SalesChannelName";
                    cmbOrderFrom.DataValueField = "SalesChannelID";
                    cmbOrderFrom.DataBind();
                    cmbOrderFrom.Items.Insert(0, new ListItem("Select", "0"));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cmbOrderFrom.Attributes.Add("SalesChannelTypeID" + dt.Rows[i]["SalesChannelID"].ToString(), dt.Rows[i]["SalesChannelTypeID"].ToString());
                    }

                }
                else
                {
                    cmbOrderFrom.Items.Clear();
                    cmbOrderFrom.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }

    protected void cmbOrderTo_SelectedIndexChanged1(object sender, EventArgs e)
    {
        fillOrderFrom();
    }
   
    # region common
  
    public void BlankSearch()
    {

        txtOrderNumber.Text = "";
        txtPartCode.Text = "";
        ucFromDate.Date = "";
        ucToDate.Date = "";
        cmbOrderTo.SelectedIndex=0;
        cmbOrderFrom.SelectedIndex=0;
        updSearch.Update();
        updpnlSaveData.Update();       
        div2.Visible = false;
        dvChkAll.Attributes.Add("style", "display:none;");
        grdvList.DataSource = null;
        grdvList.DataBind(); 
        ScriptManager.RegisterStartupScript(this.Page, GetType(), "hide Bulkupload div", "SetBulkUploadOnChange(3)", true);
    }

    #endregion


    #region packinglistdetails

    public bool ValidateSearch()
    {
        if (ucToDate.Date == "" && ucFromDate.Date == "" && txtPartCode.Text == "" && txtOrderNumber.Text == "" )
        {
            ucMessage1.ShowWarning(WarningMessages.EnterSearchCriteria);
            ucMessage1.Visible = true;           
            return false;
        }
        return true;
    }

    public bool ValidateSearchingParameters()
    {
        if (ucFromDate.Date != "")
        {
            if (ucToDate.Date == "")
            {
                ucMessage1.ShowError(ErrorMessages.InValidDateRange);
                return false;
            }
            if (Convert.ToDateTime(ucToDate.Date) < Convert.ToDateTime(ucFromDate.Date))
            {
                ucMessage1.ShowError(ErrorMessages.InValidDateRange);               
                ucMessage1.Visible = true;
                return false;
            }
        }
        if (ucToDate.Date != "")
        {
            if (ucFromDate.Date == "")
            {
                ucMessage1.ShowError(ErrorMessages.InValidDateRange);              
                ucMessage1.Visible = true;
                return false;
            }
        }
        return true;

    }

    void BindList(int index)
    {
        try
        {
            index = index == 0 ? 1 : index;
            using (clsSalesOrder obj = new clsSalesOrder())
            {
                obj.PageSize = 10;
                obj.PageIndex = index;
                if (BaseEntityTypeID == 1)
                    obj.EntityId = PageBase.SalesChanelID;
                if (BaseEntityTypeID == 2)
                   obj.EntityId =0;                 
                obj.FromID = Convert.ToInt32(cmbOrderFrom.SelectedValue);
                obj.ToID = Convert.ToInt32(cmbOrderTo.SelectedValue);
                obj.FromDate = ucFromDate.Date;
                obj.ToDate = ucToDate.Date;
                obj.OrderNumber = txtOrderNumber.Text.Trim();
                obj.PartCode = txtPartCode.Text.Trim();
                obj.StockMode = Convert.ToInt16(1);
                obj.CreatedBy = UserId;
                obj.BaseEntityTypeId = Convert.ToInt16(PageBase.BaseEntityTypeID);
                DataTable dt = obj.SelectSaleOrderPickList();
                grdvList.Visible = true;
                grdvList.DataSource = dt;
                grdvList.DataBind();
                if (dt == null || dt.Rows.Count == 0)
                {
                    ucPagingControl1.Visible = false;
                    dvChkAll.Attributes.Add("style", "display:none;");
                    dvButton.Attributes.Add("style", "display:none;");
                }
                else
                {
                    ucPagingControl1.Visible = true;
                    ucPagingControl1.PageSize = 10;
                    ucPagingControl1.TotalRecords = obj.TotalRecords;
                    ucPagingControl1.CurrentPage = index;
                    ucPagingControl1.FillPageInfo();
                    dvChkAll.Attributes.Add("style", "display:block;");
                    dvButton.Attributes.Add("style", "display:block;");
                }
                div2.Visible = true;
                updSearch.Update();

            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
    }

    protected void UCPagingControl1_SetControlRefresh()
    {
        using (clsSalesOrder obj = new clsSalesOrder())
        {
            int intPageNumber = ucPagingControl1.CurrentPage;
            ViewState["PageIndex"] = intPageNumber;
            obj.PageIndex = intPageNumber;
            BindList(ucPagingControl1.CurrentPage);
        }
    }

    protected void btnLoad_Click(object sender, EventArgs e)
    {
        
        if (cmbOrderTo.SelectedValue == "0") 
        {            
            ucMessage1.ShowWarning(WarningMessages.SelectOrderTo);
            ucMessage1.Visible = true;
            return;
        }       
        ucMessage1.Visible = false;         
        if (!ValidateSearch())
        {
            return;
        }
        if (!ValidateSearchingParameters())
        {
            return;
        }
        BindList(1);
        updpnlSaveData.Update();
    }

    #endregion


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        BlankSearch();
        ucMessage1.Visible = false;           
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {

    }
   
    public DataTable GetData()
    {
        DataTable dt = new DataTable();
        DataColumn[] dc = new DataColumn[4];
        dc[0] = new DataColumn("SalesOrderAllocationDetailID", typeof(Int64));
        dc[1] = new DataColumn("Qty", typeof(int));
        dc[2] = new DataColumn("StockMode", typeof(int));
        dc[3] = new DataColumn("PKIDType", typeof(Int64));
        dt.Columns.AddRange(dc);
        foreach (GridViewRow grv in grdvList.Rows)
        {
            Label lblAllocationID = (Label)grv.FindControl("lblAllocationID");
            TextBox txtQty = (TextBox)grv.FindControl("txtQty");
            CheckBox chkBoxModule = (CheckBox)grv.FindControl("chkBoxModule");
            Label lblSalesOrderDetailId = (Label)grv.FindControl("lblSalesOrderDetailId");
           // DropDownList ddlStockMode = (DropDownList)grv.FindControl("ddlStockMode");

            if (chkBoxModule.Checked)
            {
                DataRow dr = dt.NewRow();
               
                if (lblAllocationID.Text != null && lblAllocationID.Text != "" && lblAllocationID.Text != "0")
                {
                    dr["SalesOrderAllocationDetailID"] = lblAllocationID.Text;
                    dr["PKIDType"] = "1";/* 0-SalesOrderDetail  1-SalesOrderAllocationDetail*/
                }
                else if (lblAllocationID.Text == "" || lblAllocationID.Text == "0")
                {
                    dr["SalesOrderAllocationDetailID"] = lblSalesOrderDetailId.Text;
                    dr["PKIDType"] = "0";/* 0-SalesOrderDetail  1-SalesOrderAllocationDetail*/
                }
               
                    dr["StockMode"] = 1;
                
                if (txtQty.Text != "")
                {
                    dr["Qty"] = txtQty.Text;
                }
                
                dt.Rows.Add(dr);
                dt.AcceptChanges();
            }
        }
        return dt;
    }
   
    protected void btnSave_Click(object sender, EventArgs e)
    {
        using (clsSalesOrder objSale = new clsSalesOrder())
        {
            try
            {
                DataTable dt = GetData(); 
               
                if (dt.Rows.Count == 0)
                {                    
                    ucMessage1.ShowWarning("Please Select Some Records");                  
                    ucMessage1.Visible = true;                 
                    return;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["Qty"]) == "")
                    {                        
                        ucMessage1.ShowWarning("Please enter some quantity");                       
                        ucMessage1.Visible = true;                     
                        return;
                    }
                }
               
                Int16 result;
                objSale.strSalesInvoiceXML = "<NewDataSet><Table></Table></NewDataSet>";
                objSale.FromID = Convert.ToInt32(cmbOrderFrom.SelectedValue);
                objSale.ToID = Convert.ToInt32(cmbOrderTo.SelectedValue);
              
                if (Convert.ToString(ucFromDate.GetDate) != "")
                    objSale.SoFromDate = Convert.ToDateTime(ucFromDate.GetDate);
                if (Convert.ToString(ucToDate.GetDate) != "")
                    objSale.SoToDate = Convert.ToDateTime(ucToDate.GetDate);
               
                objSale.Type = 1;
                objSale.CreatedBy = PageBase.UserId;
                objSale.OrderNumber = txtOrderNumber.Text;
                objSale.PartCode = txtPartCode.Text;
                if (BaseEntityTypeID == 1)
                    objSale.EntityId = PageBase.SalesChanelID;
                if (BaseEntityTypeID == 2)
                    objSale.EntityId = 0; 

                result = objSale.UpdateSalesOrderAllocationDetail(dt);
                if (result == 0)
                {                   
                    ucMessage1.Visible = true;                 
                    ucMessage1.ShowSuccess("Save successfylly.");
                    BindList(1);
                    return;
                }

               
                if (objSale.strSalesInvoiceXML != null && objSale.strSalesInvoiceXML != string.Empty && objSale.Error == null)
                {
                    ucMessage1.Visible = true;
                    ucMessage1.XmlErrorSource = objSale.strSalesInvoiceXML;                  
                    return;
                }              
                else if (result == 1)
                {
                    ucMessage1.ShowError(objSale.Error);                   
                    ucMessage1.Visible = true;
                }            
                else if (result == 4)
                {
                    ucMessage1.ShowError("No Record change");                   
                    ucMessage1.Visible = true;
                }
                else
                {
                    ucMessage1.ShowError("Error occure in save.");              
                    ucMessage1.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ucMessage1.ShowError(ex.ToString());            
                ucMessage1.Visible = true;
            }
        }

    }

    protected void btnSubmitCurrent_Click(object sender, EventArgs e)
    {
        using (clsSalesOrder objSale = new clsSalesOrder())
        {
            try
            {
                DataTable dt = GetData();
              
                if (dt.Rows.Count == 0)
                {                   
                    ucMessage1.ShowWarning("Please Select Some Records");                 
                    ucMessage1.Visible = true;                 
                    return;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["Qty"]) == "")
                    {
                      
                        ucMessage1.ShowWarning("Please enter some quantity");                    
                        ucMessage1.Visible = true;                     
                        return;
                    }
                }
              
                Int16 result;
                objSale.strSalesInvoiceXML = "<NewDataSet><Table></Table></NewDataSet>";
                objSale.FromID = Convert.ToInt32(cmbOrderFrom.SelectedValue);
                objSale.ToID = Convert.ToInt32(cmbOrderTo.SelectedValue);

                if (BaseEntityTypeID == 1)
                    objSale.EntityId = PageBase.SalesChanelID;
                if (BaseEntityTypeID == 2)
                    objSale.EntityId = 0; 
              
                if (Convert.ToString(ucFromDate.GetDate) != "")
                    objSale.SoFromDate = Convert.ToDateTime(ucFromDate.GetDate);
                if (Convert.ToString(ucToDate.GetDate) != "")
                    objSale.SoToDate = Convert.ToDateTime(ucToDate.GetDate);
              
                objSale.Type = 2;
                objSale.CreatedBy = PageBase.UserId;
                objSale.OrderNumber = txtOrderNumber.Text;
                objSale.PartCode = txtPartCode.Text;
                result = objSale.UpdateSalesOrderAllocationDetail(dt);
                if (result == 0)
                {                  
                    ucMessage1.Visible = true;
                    ucMessage1.ShowSuccess("Save successfuly and " + "Picklist number is " + objSale.SalesOrderPickListNumber);
                    BindList(1);
                    return;
                }
                if (objSale.strSalesInvoiceXML != null && objSale.strSalesInvoiceXML != string.Empty && objSale.Error == null) 
                {
                    ucMessage1.XmlErrorSource = objSale.strSalesInvoiceXML;                  
                    ucMessage1.Visible = true;
                    return;
                }               
                else if (result == 1)
                {
                    ucMessage1.ShowError(objSale.Error);                   
                    ucMessage1.Visible = true;
                }               
                else if (result == 4)
                {
                    ucMessage1.ShowError("No Record change");                   
                    ucMessage1.Visible = true;
                }
                else
                {
                    ucMessage1.ShowError("Record not save successfully.");                  
                    ucMessage1.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ucMessage1.ShowError(ex.ToString());                
                ucMessage1.Visible = true;
            }
        }
    }

    protected void btnSubmitAll_Click(object sender, EventArgs e)
    {
        using (clsSalesOrder objSale = new clsSalesOrder())
        {
            try
            {
                DataTable dt = new DataTable();
               
                DataColumn[] dc = new DataColumn[4];
                dc[0] = new DataColumn("SalesOrderAllocationDetailID", typeof(Int64));
                dc[1] = new DataColumn("Qty", typeof(int));
                dc[2] = new DataColumn("StockMode", typeof(int));
                dc[3] = new DataColumn("PKIDType", typeof(Int64));/* 0-SalesOrderDetail  1-SalesOrderAllocationDetail*/
                dt.Columns.AddRange(dc);
              
                Int16 result;
                objSale.strSalesInvoiceXML = "<NewDataSet><Table></Table></NewDataSet>";
                objSale.FromID = Convert.ToInt32(cmbOrderFrom.SelectedValue);
                objSale.ToID = Convert.ToInt32(cmbOrderTo.SelectedValue);
                if (BaseEntityTypeID == 1)
                    objSale.EntityId = PageBase.SalesChanelID;
                if (BaseEntityTypeID == 2)
                    objSale.EntityId = 0; 
              
                if (Convert.ToString(ucFromDate.GetDate) != "")
                    objSale.SoFromDate = Convert.ToDateTime(ucFromDate.GetDate);
                if (Convert.ToString(ucToDate.GetDate) != "")
                    objSale.SoToDate = Convert.ToDateTime(ucToDate.GetDate);
             
                objSale.Type = 3;
                objSale.CreatedBy = PageBase.UserId;
                objSale.OrderNumber = txtOrderNumber.Text;
                objSale.PartCode = txtPartCode.Text;
                result = objSale.UpdateSalesOrderAllocationDetail(dt);
                if (result == 0)
                {                   
                    ucMessage1.Visible = true;
                    ucMessage1.ShowSuccess("Save  successfully and "+ "Picklist number is " + objSale.SalesOrderPickListNumber);
                    BindList(1);
                    return;
                }
              
                if (objSale.strSalesInvoiceXML != null && objSale.strSalesInvoiceXML != string.Empty && objSale.Error == null)
                {
                    ucMessage1.XmlErrorSource = objSale.strSalesInvoiceXML;                   
                    ucMessage1.Visible = true;
                    return;
                }              
                else if (objSale.Error != null)
                {
                    ucMessage1.ShowError(objSale.Error);
                    ucMessage1.Visible = true;
                  
                }
                else if (result == 4)
                {
                    ucMessage1.ShowError("No Record change");
                    ucMessage1.Visible = true;                   
                }
                else
                {
                    ucMessage1.ShowError("Record not saved");
                    ucMessage1.Visible = true;
                  
                }
            }
            catch (Exception ex)
            {
                ucMessage1.ShowError(ex.ToString());
                ucMessage1.Visible = true;               
            }
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (!ValidateSearch())
        {
            return;
        }
        if (!ValidateSearchingParameters())
        {
            return;
        }
        using (clsSalesOrder obj = new clsSalesOrder())
        { 
            try
            {
                obj.PageSize = 10;
                obj.PageIndex = -1;
                if (BaseEntityTypeID == 1)
                    obj.EntityId = PageBase.SalesChanelID;
                if (BaseEntityTypeID == 2)
                    obj.EntityId = 0; 
                obj.FromID = Convert.ToInt32(cmbOrderFrom.SelectedValue);
                obj.ToID = Convert.ToInt32(cmbOrderTo.SelectedValue);
                obj.FromDate = ucFromDate.Date;
                obj.ToDate = ucToDate.Date;
                obj.OrderNumber = txtOrderNumber.Text.Trim();
                obj.PartCode = txtPartCode.Text.Trim();
                obj.StockMode = 1;
                DataTable dt = obj.SelectSaleOrderPickList();
                DataSet ds = new DataSet();
                ds.Merge(dt);
                ZedService.Utility.ZedServiceUtil.ExportToExecl(ds, "SaleOrderpickList");
               // LuminousSMS.Utility.LuminousUtil.ExportToExecl(ds, "SaleOrderpickList");
                
            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.ToString());
            }
        }
    }
   
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            using (clsSalesOrder objSale = new clsSalesOrder())
            {
                Int16 result;
                objSale.CreatedBy = PageBase.UserId;
                objSale.FromID = Convert.ToInt32(cmbOrderFrom.SelectedValue);
                objSale.ToID = Convert.ToInt32(cmbOrderTo.SelectedValue);
                result = objSale.AutoAllocatePO();
                if (result == 0)
                {
                    ucMessage1.ShowSuccess("Stock Auto Allocate successfully.");
                    ucMessage1.Visible = true;                  
                    BindList(1);
                    return;
                }
                else if (result == 4)
                {
                    ucMessage1.ShowError("No Record change");
                    ucMessage1.Visible = true;                   
                }
                else if (result == 1)
                {
                    ucMessage1.ShowError(objSale.Error);
                    ucMessage1.Visible = true;                   
                }
                else
                {
                    ucMessage1.ShowError("Error in Auto stock Allocate.");
                    ucMessage1.Visible = true;
                   
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
            ucMessage1.Visible = true;           
        }
    }
  
    protected void grdvList_RowDataBound(object sender, GridViewRowEventArgs e)
    {       
        if (e.Row.RowType == DataControlRowType.DataRow)
        {            
            TextBox txtQuantity = (TextBox)e.Row.FindControl("txtQty");
            Label lblPendingQty = (Label)e.Row.FindControl("lblPendingQty");
            Label lblRequestedQty = (Label)e.Row.FindControl("lblRequestedQty");
            string str = string.Format("CheckData({0},this)", lblPendingQty.Text);           

        }

    }
   
    protected void btnexcelupload_Click(object sender, EventArgs e)
    {
        try
        {
           
            ucMessage1.Visible = false;
            if (cmbOrderTo.SelectedValue == "0")
            {
                ucMessage1.ShowWarning(WarningMessages.SelectOrderTo);               
                ucMessage1.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "Show Bulkupload div", "SetBulkUploadOnChange(2)", true);
                return;
            }
            if (!fileUpdExcel.HasFile)
            {
                ucMessage1.ShowWarning("Please select excelsheet");              
                ucMessage1.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "Show Bulkupload div", "SetBulkUploadOnChange(2)", true);
                return;
            }
            btnexcelupload.Enabled = true;
            hlnkInvalid.Visible = false;
            hlnkDuplicate.Visible = false;
            hlnkBlank.Visible = false;
            hlnkInvalid.Text = string.Empty;
            hlnkDuplicate.Text = string.Empty;
            hlnkBlank.Text = string.Empty;
            hlnkInvalid.NavigateUrl = string.Empty;
            hlnkDuplicate.NavigateUrl = string.Empty;
            hlnkBlank.NavigateUrl = string.Empty;
           
            string InvalidFileName = string.Empty;
            string DuplicateFileName = string.Empty;
            string BlankFileName = string.Empty;
            Int32 InvalidTotalRecord = 0;
            Int32 DuplicateTotalRecord = 0;
            Int32 BlankTotalRecord = 0;

            Int16 Upload = 0;
            Upload = IsExcelFile();
            if (Upload == 1)
            {
                OpenXMLExcel objexcel = new OpenXMLExcel();
                string _strFileName = PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName;
                PageBase objPageBase = new PageBase();
                DataSet DsExcel = objexcel.ImportExcelFile(_strFileName);
                if (DsExcel != null && DsExcel.Tables.Count > 0 && DsExcel.Tables[0].Rows.Count > 0)
                {
                    string strPkColName = "SRNo";
                    if (DsExcel.Tables[0].Rows.Count > 4000)
                    {
                        ucMessage1.ShowInfo("Please upload maximum 4000 records in a single upload!");
                        btnexcelupload.Enabled = true;
                        ScriptManager.RegisterStartupScript(this.Page, GetType(), "Show Bulkupload div", "SetBulkUploadOnChange(2)", true);
                    }
                    else  
                    {
                        ValidateUploadFile objValidateFile = new ValidateUploadFile();
                        {
                            DataSet objDS = DsExcel;
                            SortedList objSL = new SortedList();
                            objValidateFile.UploadedFileName = strUploadedFileName;
                            objValidateFile.ExcelFileNameInTable = "SalesOrderPickList";
                            objValidateFile.PkColumnName = strPkColName;
                            objValidateFile.ValidateFile(true, out objDS, out objSL);
                            if (objValidateFile.Message != null && objValidateFile.Message.Trim() != "")
                                ucMessage1.ShowInfo(objValidateFile.Message);
                            else
                            {
                                bool blnIsUpload = true;
                                string errmsg = string.Empty;
                                if (objSL.Count >= 1 && objSL.Keys.Count >= 1)
                                {
                                    blnIsUpload = false;
                                    objDS.Tables["DtExcelSheet"].Columns.Add(new DataColumn("ReasonForInvalid"));

                                    IDictionaryEnumerator objIDicEnum = objSL.GetEnumerator();
                                    while (objIDicEnum.MoveNext())
                                    {

                                        foreach (DataRow dr in objDS.Tables["DtExcelSheet"].Select("[" + strPkColName + "] ='" + objIDicEnum.Key.ToString() + "'", strPkColName))
                                        {
                                            dr["ReasonForInvalid"] = objIDicEnum.Value.ToString();
                                        }
                                    }

                                    objDS.Tables[0].AcceptChanges();
                                    if (objDS.Tables["DtExcelSheet"] != null && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                                    {
                                        blnIsUpload = false;
                                        errmsg = "'Invalid Data'";
                                        hlnkInvalid.Visible = true;
                                        string strFileName = strUploadedFileName + "_InvalidData";
                                        InvalidTotalRecord = objDS.Tables["DtExcelSheet"].Rows.Count;
                                        ExportInExcel(objDS.Tables["DtExcelSheet"], strFileName);
                                        hlnkInvalid.NavigateUrl = PageBase.strExcelPhysicalDownloadPathSB + strFileName + ".xlsx";                                      
                                        hlnkInvalid.Text = "Invalid Data";
                                        InvalidFileName = strFileName;
                                    }
                                }
                                if (objDS.Tables["DtDuplicateRecord"] != null && objDS.Tables["DtDuplicateRecord"].Rows.Count > 0)
                                {
                                    blnIsUpload = false;
                                    if (errmsg != string.Empty)
                                        errmsg += ", 'Duplicate SRNo'";
                                    else
                                        errmsg = "'Duplicate SRNo'";

                                    hlnkDuplicate.Visible = true;
                                    DuplicateTotalRecord = objDS.Tables["DtDuplicateRecord"].Rows.Count;
                                    string strFileName = "InvalidData" + strUploadedFileName + "_DuplicateData" + DateTime.Now.Ticks;
                                    ExportInExcel(objDS.Tables["DtDuplicateRecord"], strFileName);
                                    hlnkDuplicate.NavigateUrl = PageBase.strExcelPhysicalDownloadPathSB + strFileName + ".xlsx";
                                  //  ViewState["hlnkDuplicate"] = PageBase.strExcelPhysicalDownloadPathSB + strFileName + ".xlsx";
                                    hlnkDuplicate.Text = "Duplicate Data";
                                    DuplicateFileName = strFileName;
                                }
                                if (objDS.Tables["DtBlankData"] != null && objDS.Tables["DtBlankData"].Rows.Count > 0)
                                {
                                    blnIsUpload = false;
                                    hlnkBlank.Visible = true;
                                    if (errmsg != string.Empty)
                                        errmsg += ", 'Blank Data'";
                                    else
                                        errmsg = "Blank Data";

                                    string strFileName = "InvalidData" + strUploadedFileName + "_BlankData" + DateTime.Now.Ticks;
                                    BlankTotalRecord = objDS.Tables["DtBlankData"].Rows.Count;
                                    ExportInExcel(objDS.Tables["DtBlankData"], strFileName);
                                    hlnkBlank.NavigateUrl = PageBase.strExcelPhysicalDownloadPathSB + strFileName + ".xlsx"; 
                                   // ViewState["hlnkBlank"] = PageBase.strExcelPhysicalDownloadPathSB + strFileName + ".xlsx"; 
                                    hlnkBlank.Text = "Blank Data";
                                    BlankFileName = strFileName;
                                }
                                if (blnIsUpload)
                                {
                                    if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                                    {
                                        string strFileNameSchema = "Schema" + DateTime.Now.Ticks.ToString() + ".xml";
                                        ViewState["UploadDataFileNameSchema"] = strFileNameSchema;
                                        string strFileName = DateTime.Now.Ticks.ToString() + ".xml";
                                        ViewState["UploadDataFileName"] = strFileName;                                       

                                        if (objDS.Tables[0].Columns.Contains("Error"))
                                        {
                                            objDS.Tables[0].Columns.Remove("Error");
                                            objDS.Tables[0].AcceptChanges();
                                        }
                                        BulkUploadfromExcel(objDS.Tables[0]);
                                    }
                                    else
                                    {
                                        ucMessage1.ShowInfo("No record found!");
                                        btnexcelupload.Enabled = true;
                                        ScriptManager.RegisterStartupScript(this.Page, GetType(), "Show Bulkupload div", "SetBulkUploadOnChange(2)", true);
                                    }
                                }
                                else
                                {
                                    ucMessage1.ShowInfo(errmsg);
                                    btnexcelupload.Enabled = true;
                                    ScriptManager.RegisterStartupScript(this.Page, GetType(), "Show Bulkupload div", "SetBulkUploadOnChange(2)", true);
                                }
                            }
                        }
                    }
                }
                else
                {
                    string errmsg = "Empty data is not allowed !";
                    ucMessage1.ShowInfo(errmsg);
                    btnexcelupload.Enabled = true;
                    ScriptManager.RegisterStartupScript(this.Page, GetType(), "Show Bulkupload div", "SetBulkUploadOnChange(2)", true);
                }
            }
            else
            {
                string errmsg = "Data upload fail due to some problem!";
                ucMessage1.ShowInfo(errmsg);
                btnexcelupload.Enabled = true;
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "Show Bulkupload div", "SetBulkUploadOnChange(2)", true);
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
            btnexcelupload.Enabled = true;
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "Show Bulkupload div", "SetBulkUploadOnChange(2)", true);
        }
    }

    #region validate upload excel data

    private Int16 IsExcelFile()
    {
        Int16 MessageforValidation = 0;
        try
        {
            if (fileUpdExcel.HasFile)
            {
                if (Path.GetExtension(fileUpdExcel.FileName).ToLower() == ".xlsx")
                {
                    try
                    {
                        strUploadedFileName = PageBase.importExportExcelFileName;
                        fileUpdExcel.SaveAs(PageBase.strExcelPhysicalDownloadPathSB + strUploadedFileName);
                        MessageforValidation = 1;
                        return MessageforValidation;
                    }
                    catch (Exception objEx)
                    {
                        btnexcelupload.Enabled = true;
                        MessageforValidation = 0;
                        return MessageforValidation;
                        throw objEx;
                    }
                }
                else
                {
                    btnexcelupload.Enabled = true;
                    MessageforValidation = 2;
                    return MessageforValidation;
                }
            }
            else
            {
                btnexcelupload.Enabled = true;
                MessageforValidation = 3;
                return MessageforValidation;
            }
        }
        catch (HttpException objHttpException)
        {
            btnexcelupload.Enabled = true;
            return MessageforValidation;
            throw objHttpException;
        }
        catch (Exception ex)
        {
            btnexcelupload.Enabled = true;
            return MessageforValidation;
            throw ex;
        }
    }

    private void ExportInExcel(DataTable objTmpTb, string strFileName)
    {
        if (objTmpTb.Rows.Count > 0)
        {
            string FileName = strFileName;
            string Path = strDownloadPath + "InvalidData\\";
            DataSet ds = new DataSet();

            objTmpTb.TableName = "tblInvalidRecords" + DateTime.Now.Ticks;
            DataTable dt = objTmpTb.Copy();
            ds.Tables.Add(dt);
            PageBase.ExportToExeclV2(ds, strFileName);
        }
    }

    #endregion

    public void BulkUploadfromExcel(DataTable dtDetail)
    {

        try
        {
            if (dtDetail == null) return;
            using (clsSalesOrder objupload = new clsSalesOrder())
            {
                objupload.ToID = Convert.ToInt32(cmbOrderTo.SelectedValue);
                objupload.CreatedBy = PageBase.UserId;

                if (BaseEntityTypeID == 1)
                    objupload.LoggedInEntityID = PageBase.SalesChanelID;
                if (BaseEntityTypeID == 2)
                    objupload.LoggedInEntityID = 0; 
                //objupload.LoggedInEntityID = PageBase.EntityID;
                objupload.strSalesInvoiceXML = "<NewDataSet><Table></Table></NewDataSet>";
                int result = objupload.UploadSalesOrderAllocationDetail(dtDetail);
                if (objupload.strSalesInvoiceXML != null && objupload.strSalesInvoiceXML != string.Empty)
                {
                    ucMessage1.XmlErrorSource = objupload.strSalesInvoiceXML;
                    ucMessage1.ShowError(objupload.Error);                  
                    ucMessage1.Visible = true;
                    btnexcelupload.Enabled = true;
                    ScriptManager.RegisterStartupScript(this.Page, GetType(), "Show Bulkupload div", "SetBulkUploadOnChange(2)", true);
                    return;
                }
                if (objupload.Error != null && objupload.Error != "")
                {
                    ucMessage1.ShowError(objupload.Error);
                    ucMessage1.Visible = true;                   
                    btnexcelupload.Enabled = true;
                    ScriptManager.RegisterStartupScript(this.Page, GetType(), "Show Bulkupload div", "SetBulkUploadOnChange(2)", true);
                    return;
                }
                if (result == 1)
                {
                    ucMessage1.ShowError(objupload.Error);                  
                    ucMessage1.Visible = false;
                    btnexcelupload.Enabled = true;
                    ScriptManager.RegisterStartupScript(this.Page, GetType(), "Show Bulkupload div", "SetBulkUploadOnChange(2)", true);
                    return;
                }
                if (result == 75)
                {                   
                    ucMessage1.Visible = true;
                    btnexcelupload.Enabled = false;
                    ucMessage1.ShowSuccess("Record Save");
                    ScriptManager.RegisterStartupScript(this.Page, GetType(), "Show Bulkupload div", "SetBulkUploadOnChange(2)", true);
                }
                if (result == 0)
                {
                                       ucMessage1.Visible = true;
                    btnexcelupload.Enabled = false;
                    ucMessage1.ShowSuccess("Record Save and " + "Picklist number is " + objupload.SalesOrderPickListNumber);
                    ScriptManager.RegisterStartupScript(this.Page, GetType(), "Show Bulkupload div", "SetBulkUploadOnChange(2)", true);
                   
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
            ucMessage1.Visible = true;
            ucMessage1.Visible = false;
            btnexcelupload.Enabled = true;
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "Show Bulkupload div", "SetBulkUploadOnChange(2)", true);
        }
    }
   
    public void ShowHideAutoAllocate()
    {
        try
        {

            DataTable dt = PageBase.GetEnumByTableName("AppConfig", "ShowHideRunAutoAllocate");
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Int16 intShowHideRunAutoAllocate = Convert.ToInt16(dt.Rows[0]["Value"]);
                    if (intShowHideRunAutoAllocate == 0)
                    {
                        btnRefresh.Visible = false; /* Auto Allocate Button */
                        chkBoxBulkupload.Visible = false;
                    }
                    else
                    {
                        btnRefresh.Visible = true; /* Auto Allocate Button */
                        chkBoxBulkupload.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
        }
    }
   
}
