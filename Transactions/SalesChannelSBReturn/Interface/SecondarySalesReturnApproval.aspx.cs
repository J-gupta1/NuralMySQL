#region Copyright(c) 2017 Zed-Axis Technologies All rights are reserved
/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2017 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Created By : Sumit Maurya
* Created On: 23-Feb-2017 
 * Description: This interface is used to approve reject Secondary Sales Return.
* ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 29-May-2018,Vijay Kumar Prajapati,#CC01-Added ReturnType in Page.
 ====================================================================================================
*/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using ExportExcelOpenXML;
using System.Text;
using BusinessLogics;
using System.Collections;
using System.Data.SqlClient;


public partial class Transactions_SalesChannelSBReturn_Interface_SecondarySalesReturnApproval : PageBase
{

    DateTime dt = new DateTime();
    DataTable dtNew = new DataTable();
    int intSelectedValue;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMsg.ShowControl = false;
            if (!IsPostBack)
            {
                Session["ID"] = null;
                if (pnldetail.Visible == true)
                {
                    pnldetail.Visible = false;
                }
                ViewState["dtSearch"] = null;
                ViewState["SecondarySalesReturnMainID"] = null;
                ClearForm();
              
                    FillReturnType();/*#CC01 Added*/
                   
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }

    }

    bool ChckDateInput()
    {
        DateTime dateTime;
        if (ucFromDate.Date != "")
        {
            if (!DateTime.TryParse(ucFromDate.Date, out dateTime))
            {
                return false;
            }
        }
        if (ucToDate.Date != "")
        {
            if (!DateTime.TryParse(ucToDate.Date, out dateTime))
            {
                return false;
            }
        }
        return true;
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        ClearForm();
    }

    protected void ExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            using (SalesData objSales = new SalesData())
            {
                objSales.PageIndex = -1;
                //objSales.PageSize = Convert.ToInt32(PageBase.PageSize);
                objSales.SalesChannelID = Convert.ToInt32(ddlSalesChannel.SelectedValue);
                objSales.SalesChannelCode = txtSalesChannelCode.Text.Trim();
                objSales.InvoiceNumber = txtInvoice.Text.Trim();
                objSales.strFromDate = ucFromDate.TextBoxDate.Text;
                objSales.strToDate = ucToDate.TextBoxDate.Text;
                objSales.Status = Convert.ToInt16(ddlStatus.SelectedValue);
                objSales.ReturnType = Convert.ToInt32(ddlReturnType.SelectedValue);/*#CC01 Added*/
                DataSet ds = objSales.dsSecondarySalesReturnApprovalData();
                if (objSales.TotalRecords > 0)
                {
                    DataTable dtExcel = ds.Tables[0];
                    if (dtExcel.Rows.Count > 0)
                    {
                        DataSet dsExcel = new DataSet();
                        dsExcel.Merge(dtExcel);
                        dsExcel.Tables[0].AcceptChanges();
                        String FilePath = Server.MapPath("../../");
                        string FilenameToexport = "SalesReturnData";
                        PageBase.RootFilePath = FilePath;
                        PageBase.ExportToExecl(dsExcel, FilenameToexport);
                    }
                }
                else
                {
                    ucMsg.ShowInfo(Resources.Messages.NoRecord);

                }
            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearForm();

    }
    public void BindSalesChannelType()
    {
        try
        {
            using (SalesData objTemp = new SalesData())
            {
                objTemp.BilltoRetailer = 1;/*#CC01 Added*/
                DataSet ds = objTemp.dsSecondarySalesReturnDropdownData();
                if (objTemp.TotalRecords > 0)
                {
                    String[] StrCol = new String[] { "SalesChannelTypeID", "SalesChannelTypeName" };
                    PageBase.DropdownBinding(ref ddlSalesChannelType, ds.Tables[0], StrCol);
                }

            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }

    protected void ddlSalesChannelType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            using (SalesData objTemp = new SalesData())
            {
                ddlSalesChannel.Items.Clear();
                objTemp.SalesChannelTypeID = Convert.ToInt32(ddlSalesChannelType.SelectedValue);
                DataSet ds = objTemp.dsSecondarySalesReturnDropdownData();
                if (objTemp.TotalRecords > 0 && ddlSalesChannelType.SelectedValue != "0")
                {
                    String[] StrCol = new String[] { "SalesChannelID", "SalesChannelName" };
                    PageBase.DropdownBinding(ref ddlSalesChannel, ds.Tables[0], StrCol);
                }
                else
                {
                    ddlSalesChannel.Items.Clear();
                    ddlSalesChannel.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            ClearOutput();
            dtNew = new DataTable();

            if (!ChckDateInput())
            {
                ucMsg.ShowError("Invalid Date format");
                return;
            }
            if (pageValidateSave())
            {
                GetSearchData(1);
            }
            else
            {
                ucMsg.ShowError("Invalid Searching Parameters");
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    bool pageValidateSave()
    {
        if (ucFromDate.Date == "" && ucToDate.Date == "" && txtInvoice.Text.Trim() == "" && ddlStatus.SelectedValue == "101")
        {
            ucMsg.ShowInfo(Resources.Messages.MandatoryField);
            return false;
        }
        if (ucFromDate.Date != "")
        {
            if (ucToDate.Date == "")
            {
                ucMsg.ShowInfo(Resources.Messages.InvalidDate);
                return false;
            }
        }
        if (ucToDate.Date != "")
        {
            if (ucFromDate.Date == "")
            {
                ucMsg.ShowInfo(Resources.Messages.InvalidDate);
                return false;
            }
        }
        if (ucToDate.Date != "" && ucFromDate.Date != "")
        {
            if (Convert.ToDateTime(ucFromDate.Date) > Convert.ToDateTime(ucToDate.Date))
            {
                ucMsg.ShowInfo(Resources.Messages.InvalidDate);
                return false;
            }
        }


        return true;
    }

    public DataTable GetDistinctRecords(DataTable dt, string[] Columns)
    {
        DataTable dtUniqRecords = new DataTable();
        dtUniqRecords = dt.DefaultView.ToTable(true, Columns);
        return dtUniqRecords;
    }

    protected void gvAck_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            dtNew = new DataTable();
            Int32 id = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Details")
            {
                ViewState["SecondarySalesReturnMainID"] = Convert.ToInt32(e.CommandArgument);
                Button imgbtn = (Button)e.CommandSource;      //Only this code will change the Selected row css
                if(ddlStatus.SelectedValue=="1")
                {
                    btnAccept.Visible = false;
                    btnReject.Visible = false;
                }
                else if(ddlStatus.SelectedValue=="2")
                {
                    btnAccept.Visible = false;
                    btnReject.Visible = false;
                }
                else
                {
                    btnAccept.Visible = true;
                    btnReject.Visible = true;
                }
                using (SalesData objsales = new SalesData())
                {
                    objsales.SecondarySalesReturnMainID = Convert.ToInt32(e.CommandArgument);// Convert.ToInt32(intSelectedValue);
                    objsales.ReturnType = Convert.ToInt32(ddlReturnType.SelectedValue);/*#CC01 Added*/
                    DataSet ds = objsales.dsSecondarySalesReturnSerialData();
                    ViewState["dtSearch"] = ds.Tables[0];
                    string[] Columns = { "SecondarySalesReturnID", "InvoiceNumber", "InvoiceDate", "Quantity", "SKUName", "Mode", "Amount" };
                    DataTable objDtUniqueFile = (ds.Tables[0].DefaultView.ToTable(true, Columns));
                    grdDetails.DataSource = objDtUniqueFile;
                    grdDetails.DataBind();
                    pnldetail.Visible = true;
                    updGrid.Update();
                    upddetails.Update();
                }

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void detailGrid_DataSelect(object sender, EventArgs e)
    {
        try
        {
            dtNew = new DataTable();
            intSelectedValue = Convert.ToInt32((sender as ASPxGridView).GetMasterRowKeyValue());
            ASPxGridView objDetail = (ASPxGridView)(sender as ASPxGridView);
            DataTable dt = (DataTable)ViewState["dtSearch"];
            dt.DefaultView.RowFilter = "SecondarySalesReturnID =" + intSelectedValue;

            objDetail.DataSource = dt;
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }

    }

    void ClearForm()
    {
        ucFromDate.imgCal.Enabled = true;
        ucFromDate.TextBoxDate.Enabled = true;
        ucFromDate.TextBoxDate.Text = "";
        ucToDate.imgCal.Enabled = true;
        ucToDate.TextBoxDate.Enabled = true;
        ucToDate.TextBoxDate.Text = "";
        pnlGrid.Visible = false;
        pnldetail.Visible = false;
        ddlStatus.SelectedValue = "101";
        txtInvoice.Text = "";
        updGrid.Update();
        upddetails.Update();

        ViewState["SecondarySalesReturnMainID"] = null;
        BindSalesChannelType();
        ddlSalesChannel.Items.Clear();
        ddlSalesChannel.Items.Insert(0, new ListItem("Select", "0"));
    }
    void ClearOutput()
    {
        pnlGrid.Visible = false;
        pnldetail.Visible = false;
        updGrid.Update();
        upddetails.Update();
        ViewState["Type"] = null;
    }
    protected void btnAccept_Click(object sender, EventArgs e)
    {
        AcceptorReject(1);
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        AcceptorReject(2);
    }

    public void AcceptorReject(Int16 ApproveStatus)
    {
        try
        {
            using (SalesData objSales = new SalesData())
            {
                objSales.ApproveStatus = ApproveStatus;
                objSales.SecondarySalesReturnMainID = Convert.ToInt64(ViewState["SecondarySalesReturnMainID"]);
                objSales.ReturnType = Convert.ToInt32(ddlReturnType.SelectedValue);
                objSales.UserID = PageBase.UserId;/*#CC01 Added*/
                objSales.ApproveRejectSecondarySalesReturn();
                if (objSales.intOutParam == 0)
                {
                    ucMsg.ShowSuccess("Record saved successfully.");
                    ucPagingControl1.FillPageInfo();
                    // GetSearchData(ucPagingControl1.CurrentPage);

                    DataSet ds = new DataSet();
                    objSales.PageIndex = ucPagingControl1.CurrentPage;
                    objSales.PageSize = Convert.ToInt32(PageBase.PageSize);
                    objSales.Status = Convert.ToInt16(ddlStatus.SelectedValue);
                    ds = objSales.dsSecondarySalesReturnApprovalData();
                    ViewState["dtSearch"] = ds.Tables[0];
                    if (objSales.TotalRecords > 0)
                    {
                        /// ViewState["Type"] = dtNew.Rows[0]["Type"].ToString();
                        dvFooter.Visible = true;
                        ViewState["TotalRecords"] = objSales.TotalRecords;
                        ucPagingControl1.TotalRecords = objSales.TotalRecords;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = ucPagingControl1.CurrentPage;
                        ucPagingControl1.FillPageInfo();
                        gvAck.DataSource = ds.Tables[0];
                        gvAck.DataBind();

                        updGrid.Update();
                        pnldetail.Visible = false;
                        upddetails.Update();

                    }
                    else
                    {
                        pnldetail.Visible = false;
                        upddetails.Update();
                        gvAck.DataSource = null;
                        gvAck.DataBind();
                        pnlGrid.Visible = false;
                    }
                    upddetails.Update();

                }
                else if (objSales.intOutParam == 1)
                {
                    if (objSales.Error != null)
                        ucMsg.ShowInfo(objSales.Error);
                    if (objSales.ErrorDetailXML != null)
                        ucMsg.XmlErrorSource = objSales.ErrorDetailXML;
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            ucMsg.ShowError(ex.ToString());
        }
        finally
        {
            ViewState["SecondarySalesReturnMainID"] = null;

        }
    }

    public void GetSearchData(int pageno)
    {
        try
        {
            using (SalesData objSales = new SalesData())
            {
                DataSet ds = new DataSet();
                objSales.PageIndex = pageno;
                objSales.PageSize = Convert.ToInt32(PageBase.PageSize);
                objSales.Status = Convert.ToInt16(ddlStatus.SelectedValue);
                objSales.InvoiceNumber = txtInvoice.Text.Trim();
                objSales.strFromDate = ucFromDate.TextBoxDate.Text;
                objSales.strToDate = ucToDate.TextBoxDate.Text;
                objSales.SalesChannelID = Convert.ToInt32(ddlSalesChannel.SelectedValue);
                objSales.SalesChannelCode = txtSalesChannelCode.Text.Trim();
                
                objSales.ReturnType = Convert.ToInt32(ddlReturnType.SelectedValue);/*#CC01 Added*/

                ds = objSales.dsSecondarySalesReturnApprovalData();
                ViewState["dtSearch"] = ds.Tables[0];
                if (objSales.TotalRecords > 0)
                {
                    /// ViewState["Type"] = dtNew.Rows[0]["Type"].ToString();
                    dvFooter.Visible = true;
                    ViewState["TotalRecords"] = objSales.TotalRecords;
                    ucPagingControl1.TotalRecords = objSales.TotalRecords;
                    ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                    ucPagingControl1.SetCurrentPage = pageno;
                    ucPagingControl1.FillPageInfo();
                    gvAck.DataSource = ds.Tables[0];
                    gvAck.DataBind();
                    pnlGrid.Visible = true;
                    updGrid.Update();

                }
                else
                {
                    dvFooter.Visible = false;
                    ucMsg.ShowInfo(Resources.Messages.NoRecord);
                }

                upddetails.Update();
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }

    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        GetSearchData(ucPagingControl1.CurrentPage);

    }
    /*#CC01 Added Started*/
    public void FillReturnType()
    {
        List<ListItem> items = new List<ListItem>();
        items.Add(new ListItem("Select", "0"));
        if (Convert.ToString(HttpContext.Current.Session["SecondarySalesReturnApproval"]) == "1")
        {

            items.Add(new ListItem("SecondarySalesReturn", "1"));
        }
        if (Convert.ToString(HttpContext.Current.Session["IntermediarySalesReturnApproval"]) == "1")
        {
            items.Add(new ListItem("IntermediarySalesReturn", "2"));

        }
        ddlReturnType.Items.AddRange(items.ToArray());
    }
    /*#CC01 Added End*/
}

