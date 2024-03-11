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
* Created By : Vijay Kumar Prajapati
* Created On: 12-Sept-2017 
 * Description: This is a DOA Search Report.
* ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 18-Jul-2018, Sumit Maurya, #CC01, UserID provided to get warehouse according to logged in userid(Done for ComioV5).
 ====================================================================================================
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;

public partial class DOA_AcknowledgementDOA : PageBase
{
    clsDoaReport objreport = new clsDoaReport();
    public DataSet ds = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        ucMessage1.XmlErrorSource = "";
        if (!IsPostBack)
        {
            //ucFromDate.Date = PageBase.Fromdate;
           // ucToDate.Date = PageBase.ToDate;
            BindDOAStatus();
            BindDispatchTo();
            dvhide.Visible = false;
            lnkPrintDispatch.Visible = false;
            PnlDoaDispatch.Visible = false;
            btnAcknowledgement.Visible = false;
        }
    }
    private void HideForSearchClick()
    {
        ucMessage1.Visible = false;
        lnkPrintDispatch.Visible = false;
        dvhide.Visible = true;
        PnlMainDoaSellement.Visible = false;
        PnlSaveDoa.Visible = false;
        RbtCreditNote.Checked = false;
        RbtSwapImei.Checked = false;
        txtCreditNumber.Text = string.Empty;
        txtImeiSettlement.Text = string.Empty;
        txtRemarks.Text = "";
        ddlDispatchMode.SelectedValue = "0";
        txtdocketnumber.Text = "";
        txtinvoiceno.Text = "";
        txtcouriername.Text = "";
        ddlDispatchto.SelectedValue = "0";
        btnAcknowledgement.Visible = false;
        PnlDoaDispatch.Visible = false;
    }
    protected void Search_Click(object sender, EventArgs e)
    {
        try
        {
            HideForSearchClick();
            if ((txtDoaCertificateno.Text.Trim() == "" || txtDoaCertificateno.Text.Trim() == null) && (txtIMEINo.Text.Trim() == "" || txtIMEINo.Text.Trim() == null) && (ddldoastatus.SelectedValue == "0") && (ucFromDate.Date == "") && (ucToDate.Date == ""))
            {
                ucMessage1.Visible = true;
                ucMessage1.ShowWarning("Select OR Enter any search criteria!!!");
                dvhide.Visible = false;
                return;
            }
            else if (ucFromDate.Date != "" && ucToDate.Date == "")
            {
                ucMessage1.ShowWarning("Please enter To Date.");
                return;
            }
            else if (ucFromDate.Date == "" && ucToDate.Date != "")
            {
                ucMessage1.ShowWarning("Please enter  From Date.");
                return;
            }
            else
            {
                bindSearchDOAData(1);
            }
        }
        catch (Exception ex)
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if ((txtDoaCertificateno.Text.Trim() == "" || txtIMEINo.Text.Trim() == null) && (txtIMEINo.Text.Trim() == "" || txtIMEINo.Text.Trim() == null) && (ddldoastatus.SelectedValue == "0") && (ucFromDate.Date == "") && (ucToDate.Date == ""))
            {
                ucMessage1.ShowWarning("Select OR Enter any search criteria!!!");
                return;

            }
            else if (ucFromDate.Date != "" && ucToDate.Date == "")
            {
                ucMessage1.ShowWarning("Please enter To Date.");
                return;
            }
            else if (ucFromDate.Date == "" && ucToDate.Date != "")
            {
                ucMessage1.ShowWarning("Please enter  From Date.");
                return;
            }
            else
            {
                ucMessage1.Visible = false;
                lnkPrintDispatch.Visible = false;
                bindDOADataForExcel();
            }
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ucMessage1.Visible = false;
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        bindSearchDOAData(ucPagingControl1.CurrentPage);
        btnAcknowledgement.Visible = false;

    }
    public void bindSearchDOAData(int pageno)
    {
        try
        {
            ViewState["TotalRecords"] = 0;
            if (ViewState["CurrentPage"] == null)
            {
                pageno = 1;
                ViewState["CurrentPage"] = pageno;
            }
            if (ucFromDate.Date == "" && ucToDate.Date == "")
            { ;}
            else
            {
                objreport.RequestDateFrom = Convert.ToDateTime(ucFromDate.Date);
                objreport.RequestDateTo = Convert.ToDateTime(ucToDate.Date);
            }
            if (txtIMEINo.Text.Trim() != "" && txtIMEINo.Text.Trim() != null)
            {
                objreport.IMEINumber = txtIMEINo.Text.Trim();
            }
            if (txtDoaCertificateno.Text.Trim() != "" && txtDoaCertificateno.Text.Trim() != null)
            {
                objreport.DOACertificateNumber = txtDoaCertificateno.Text.Trim();
            }
            objreport.DOAStatus = Convert.ToInt32(ddldoastatus.SelectedValue);
            objreport.SalesChannelId = PageBase.SalesChanelID;
            objreport.LoginUserId = PageBase.UserId;
            objreport.PageIndex = pageno;
            objreport.PageSize = Convert.ToInt32(PageBase.PageSize);
            DataSet ds = objreport.GetReportDOAData();
            if (objreport.TotalRecords > 0)
            {
                GridDOA.DataSource = ds;
                GridDOA.DataBind();
                //lnkPrintDispatch.Visible = false;
                ViewState["TotalRecords"] = objreport.TotalRecords;
                ucPagingControl1.TotalRecords = objreport.TotalRecords;
                ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                ucPagingControl1.SetCurrentPage = pageno;
                ucPagingControl1.FillPageInfo();
            }
            else
            {
                ds = null;
                GridDOA.DataSource = null;
                GridDOA.DataBind();
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    private void BindDOAStatus()
    {
        DataTable dtresult = new DataTable();
        try
        {
            dtresult = clsDoaReport.GetEnumbyTableName("XML_Enum", "DOAStatus");
            if (dtresult.Rows.Count > 0)
            {
                ddldoastatus.DataSource = dtresult;
                ddldoastatus.DataTextField = "Description";
                ddldoastatus.DataValueField = "Value";
                ddldoastatus.DataBind();
                ddldoastatus.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddldoastatus.Items.Insert(0, new ListItem("Select", "0"));
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
    private void BindDispatchTo()
    {
        DataTable dtresult = new DataTable();
        using (clsDoaReport objresult = new clsDoaReport())
        {
            objresult.LoginUserId = PageBase.UserId; /* #CC01 Added */
            dtresult = objresult.GetDispatchTo();
            if (dtresult.Rows.Count > 0)
            {
                ddlDispatchto.DataSource = dtresult;
                ddlDispatchto.DataTextField = "SalesChannelName";
                ddlDispatchto.DataValueField = "SalesChannelID";
                ddlDispatchto.DataBind();
                ddlDispatchto.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddlDispatchto.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
    }
    private DataSet bindDOADataForExcel()
    {
        if (ucFromDate.Date == "" && ucToDate.Date == "")
        { ;}
        else
        {
            objreport.RequestDateFrom = Convert.ToDateTime(ucFromDate.Date);
            objreport.RequestDateTo = Convert.ToDateTime(ucToDate.Date);
        }
        if (txtIMEINo.Text.Trim() != "" && txtIMEINo.Text.Trim() != null)
        {
            objreport.IMEINumber = txtIMEINo.Text.Trim();
        }
        if (txtDoaCertificateno.Text.Trim() != "" && txtDoaCertificateno.Text.Trim() != null)
        {
            objreport.DOACertificateNumber = txtDoaCertificateno.Text.Trim();
        }

        objreport.DOAStatus = Convert.ToInt32(ddldoastatus.SelectedValue);
        objreport.SalesChannelId = PageBase.SalesChanelID;
        objreport.LoginUserId = PageBase.UserId;
        ds = objreport.GetExcelReportDOAData();
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            string FilenameToexport = "DOAData";
            PageBase.ExportToExecl(ds, FilenameToexport);
        }
        else
        {
            ds = null;
            //ucMessage1.Visible = true;
            //ucMessage1.ShowInfo("No Record Found");
            GridDOA.DataSource = null;
            GridDOA.DataBind();
            PnlDoaDispatch.Visible = false;
            PnlMainDoaSellement.Visible = false;
        }
        return ds;
    }
    private void Clear()
    {
        txtDoaCertificateno.Text = "";
        txtIMEINo.Text = "";
        ddldoastatus.SelectedValue = "0";
        ucFromDate.Date = "";
        ucToDate.Date = "";
        PnlMainDoaSellement.Visible = false;
        PnlSaveDoa.Visible = false;
        dvhide.Visible = false;
        RbtCreditNote.Checked = false;
        RbtSwapImei.Checked = false;
        txtCreditNumber.Text = "";
        txtImeiSettlement.Text = "";
        PnlDoaDispatch.Visible = false;
        txtRemarks.Text = "";
        ddlDispatchMode.SelectedValue = "0";
        txtdocketnumber.Text = "";
        txtinvoiceno.Text = "";
        txtcouriername.Text = "";
        ddlDispatchto.SelectedValue = "0";
        lnkPrintDispatch.Visible = false;
        btnAcknowledgement.Visible = false;
       // ucFromDate.Date = PageBase.Fromdate;
        //ucToDate.Date = PageBase.ToDate;


    }
    protected void Cancel_Click(object sender, EventArgs e)
    {
        Clear();
    }
    protected void GridDOA_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "DOASettlement" && e.CommandArgument != "")
        {
            PnlMainDoaSellement.Visible = true;
            btnAcknowledgement.Visible = false;
            Int64 DOAID = Convert.ToInt32(e.CommandArgument);
            hdfDOAID.Value = Convert.ToString(DOAID);

            GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            int RowIndex = oItem.RowIndex;
            for (int i = 0; i < GridDOA.Rows.Count; i++)
            {
                GridDOA.Rows[i].BackColor = System.Drawing.Color.White;
            }
            GridDOA.Rows[RowIndex].BackColor = System.Drawing.Color.YellowGreen;
            RbtCreditNote.Checked = false;
            RbtSwapImei.Checked = false;
            PnlSaveDoa.Visible = false;
            txtImeiSettlement.Text = string.Empty;
            txtCreditNumber.Text = string.Empty;
            PnlDoaDispatch.Visible = false;
            GridViewRow row = GridDOA.HeaderRow;
            ((CheckBox)row.FindControl("checkAll")).Checked = false;
            ((CheckBox)row.FindControl("checkAllDispatch")).Checked = false;
            foreach (GridViewRow grv in GridDOA.Rows)
            {
                if (grv.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk = (CheckBox)(grv.FindControl("chkRow"));
                    CheckBox chkdispatch = (CheckBox)(grv.FindControl("chkDispatchRow"));

                    if (chk.Checked)
                    {
                        chk.Checked = false;
                    }
                    if (chkdispatch.Checked)
                    {
                        chkdispatch.Checked = false;
                    }
                }

            }

        }

    }
    protected void RbtCreditNote_CheckedChanged(object sender, EventArgs e)
    {
        PnlSaveDoa.Visible = true;
        txtImeiSettlement.Enabled = false;
        txtCreditNumber.Enabled = true;
        txtImeiSettlement.Text = "";
        txtCreditNumber.Text = "";
        btnAcknowledgement.Visible = false;
    }
    protected void RbtSwapImei_CheckedChanged(object sender, EventArgs e)
    {
        PnlSaveDoa.Visible = true;
        txtCreditNumber.Enabled = false;
        txtCreditNumber.Text = "";
        txtIMEINo.Text = "";
        txtImeiSettlement.Enabled = true;
        btnAcknowledgement.Visible = false;
    }
    protected void btnSubmitsettlementDoa_Click(object sender, EventArgs e)
    {
        Int64 DoaSettlementid = Convert.ToInt64(hdfDOAID.Value);
        objreport.DoaId = DoaSettlementid;
        objreport.SalesChannelId = PageBase.SalesChanelID;
        objreport.LoginUserId = PageBase.UserId;
        lnkPrintDispatch.Visible = false;
        PnlSaveDoa.Visible = true;
        btnAcknowledgement.Visible = false;
        try
        {
            if (txtImeiSettlement.Enabled == false)
            {
                if (txtCreditNumber.Text.Trim() == "" || txtCreditNumber.Text.Trim() == null)
                {
                    ucMessage1.ShowWarning("Please Enter Credit Note No.");
                    return;
                }
                else
                {
                    objreport.CreditNote = txtCreditNumber.Text.Trim();
                    DataSet dsResult = objreport.UpdateAcknowledgement();
                    Int16 Result = objreport.OutParam;
                    if (Result == 0)
                    {
                        ucMessage1.ShowSuccess("Credit Note Created Successfully.");
                        bindSearchDOAData(1);
                        txtCreditNumber.Text = "";
                        PnlSaveDoa.Visible = false;
                        PnlMainDoaSellement.Visible = false;
                    }
                    else if (Result == 1 && dsResult.Tables.Count > 0)
                    {
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            string Error = dsResult.Tables[0].Rows[0]["Error"].ToString();

                            string ErrorCode = dsResult.Tables[0].Rows[0]["EntityCode"].ToString();

                            string Err = Error.Replace("ERROR rethrown from CATCH block: Number:50000 Mess", "");

                            ucMessage1.ShowError(ErrorCode + ' ' + Err);
                            return;
                        }
                    }
                }
            }
            else if (txtCreditNumber.Enabled == false)
            {
                if (txtImeiSettlement.Text.Trim() == "" || txtImeiSettlement.Text.Trim() == null)
                {
                    ucMessage1.ShowWarning("Please Enter IMEI No.");
                    return;
                }
                else
                {
                    objreport.SwapIMEI = txtImeiSettlement.Text.Trim();
                    DataSet dsResult = objreport.UpdateAcknowledgement();
                    Int16 Result = objreport.OutParam;
                    if (Result == 0)
                    {
                        ucMessage1.ShowSuccess("Model Replacement Created Successfully.");
                        bindSearchDOAData(1);
                        txtImeiSettlement.Text = "";
                        PnlSaveDoa.Visible = false;
                        PnlMainDoaSellement.Visible = false;

                    }
                    else if (Result == 1 && dsResult.Tables.Count == 0)
                    {
                        ucMessage1.ShowError(objreport.ErrorMessage);
                    }
                    else if (Result == 1 && dsResult.Tables.Count > 0)
                    {
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            string Error = dsResult.Tables[0].Rows[0]["Error"].ToString();
                            string ErrorCode = dsResult.Tables[0].Rows[0]["EntityCode"].ToString();
                            string Err = Error.Replace("ERROR rethrown from CATCH block: Number:50000 Mess", "");
                            ucMessage1.ShowError(ErrorCode + ' ' + Err);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
    protected void btnAcknowledgement_Click(object sender, EventArgs e)
    {
        PnlMainDoaSellement.Visible = false;
        lnkPrintDispatch.Visible = false;
        CheckBox chk = new CheckBox();
        DataTable dtdoaid = new DataTable();
        string strdoaid = string.Empty;
        try
        {
            dtdoaid.Columns.Add("DOAID");
            foreach (GridViewRow grv in GridDOA.Rows)
            {
                if (grv.RowType == DataControlRowType.DataRow)
                {
                    chk = (CheckBox)(grv.FindControl("chkRow"));

                    if (chk.Checked)
                    {
                        strdoaid = GridDOA.DataKeys[grv.RowIndex].Value.ToString();
                        DataRow dr = null;
                        dr = dtdoaid.NewRow();
                        dr["DOAID"] = strdoaid;
                        dtdoaid.Rows.Add(dr);
                        dtdoaid.AcceptChanges();

                    }
                    if (strdoaid == string.Empty && strdoaid != null)
                    {
                        ucMessage1.ShowWarning("No record in list for acknowledge.");
                    }
                }

            }
            if (dtdoaid != null && dtdoaid.Rows.Count > 0)
            {
                using (clsDoaReport objreport = new clsDoaReport())
                {
                    objreport.SalesChannelId = PageBase.SalesChanelID;
                    objreport.LoginUserId = PageBase.UserId;
                    objreport.DOAIDForDispatch = dtdoaid;
                    DataSet dsResult = objreport.ReceiveAcknowledgement();
                    Int16 Result = objreport.OutParam;
                    if (Result == 0)
                    {
                        ucMessage1.ShowSuccess("Acknowledgement Receive successfully.");
                        bindSearchDOAData(1);
                    }
                    else if (Result == 1)
                    {
                        ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    }
                }
            }
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.Message.ToString());
        }
    }


    protected void btnDispatch_Click(object sender, EventArgs e)
    {

        CheckBox chk = new CheckBox();
        string strdoaid = string.Empty;
        DataTable dtblIMEIItem = new DataTable();
        DateTime dtime = System.DateTime.Now;
        try
        {
            if (ddlDispatchMode.SelectedValue == "0")
            {
                ucMessage1.ShowError("Please Select Dispatch Mode.");
                return;
            }
            else if (txtdocketnumber.Text.Trim() == "")
            {
                ucMessage1.ShowError("Enter Docket No.");
                return;
            }
            else if (txtcouriername.Text.Trim() == "")
            {
                ucMessage1.ShowError("Enter Courier Name.");
                return;
            }
            else if (txtinvoiceno.Text.Trim() == "")
            {
                ucMessage1.ShowError("Enter Docket Number.");
                return;
            }
            else if (txtRemarks.Text.Trim() == "")
            {
                ucMessage1.ShowError("Enter Remarks.");
                return;
            }
            else if (ddlDispatchto.SelectedValue == "0")
            {
                ucMessage1.ShowError("Please Select Dispatch To.");
                return;
            }
            else if (ucInvoiceDate.Date== "")
            {
                ucMessage1.ShowError("Please Enter Invoice Date.");
                return;
            }
            if ( Convert.ToDateTime(ucInvoiceDate.Date) > dtime)
            {
                ucMessage1.ShowError("Invoice Date Should Not Be Future Date.");
                return;
            }
            dtblIMEIItem.Columns.Add("DOAID");
            foreach (GridViewRow grv in GridDOA.Rows)
            {
                if (grv.RowType == DataControlRowType.DataRow)
                {
                    chk = (CheckBox)(grv.FindControl("chkDispatchRow"));

                    if (chk.Checked)
                    {
                        strdoaid = GridDOA.DataKeys[grv.RowIndex].Value.ToString();
                        DataRow dr = null;
                        dr = dtblIMEIItem.NewRow();
                        dr["DOAID"] = strdoaid;
                        dtblIMEIItem.Rows.Add(dr);
                        dtblIMEIItem.AcceptChanges();

                    }
                    if (strdoaid == string.Empty && strdoaid != null)
                    {
                        ucMessage1.ShowWarning("No record in list for dispatch.");
                        //return;
                    }
                }
            }
            if (dtblIMEIItem != null && dtblIMEIItem.Rows.Count > 0)
            {
                using (clsDoaReport objreport = new clsDoaReport())
                {
                    objreport.SalesChannelId = PageBase.SalesChanelID;
                    objreport.LoginUserId = PageBase.UserId;
                    objreport.DOAIDForDispatch = dtblIMEIItem;
                    objreport.DispatchMode = Convert.ToInt16(ddlDispatchMode.SelectedValue);
                    objreport.Remarks = txtRemarks.Text.Trim();
                    objreport.DocketNo = txtinvoiceno.Text.Trim();
                    objreport.GCNNo = txtdocketnumber.Text.Trim();
                    objreport.DispatchTOid = Convert.ToInt16(ddlDispatchto.SelectedValue);
                    objreport.CourierName = txtcouriername.Text.Trim();
                    objreport.InvoiceDate =Convert.ToDateTime(ucInvoiceDate.Date);
                    DataSet dsResult = objreport.SaveDispatch();

                    Int16 Result = objreport.OutParam;
                    Int64 StockDispatchID = objreport.StockDispatchIDforPrint;
                    if (Result == 0)
                    {
                        ucMessage1.ShowSuccess("Dispatch Created successfully.");

                        lnkPrintDispatch.Visible = true;
                        lnkPrintDispatch.Attributes.Add("OnClick", "return Popup('" + Cryptography.Crypto.Encrypt(StockDispatchID.ToString().Replace(" ", "+"), PageBase.KeyStr) + "');");
                        bindSearchDOAData(1);
                        DispatchCotrolClear();
                        PnlDoaDispatch.Visible = false;

                    }
                    else if (Result == 1)
                    {
                        ucMessage1.ShowError(objreport.ErrorMessage);
                        lnkPrintDispatch.Visible = false;
                    }
                }
            }
            else
            {
                ucMessage1.ShowWarning("No record in list for dispatch.");
                return;
            }
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.Message.ToString());
        }

    }
    private void DispatchCotrolClear()
    {
        txtRemarks.Text = "";
        ddlDispatchMode.SelectedValue = "0";
        txtdocketnumber.Text = "";
        txtinvoiceno.Text = "";
        txtcouriername.Text = "";
        ddlDispatchto.SelectedValue = "0";
        ucInvoiceDate.Date = "";
    }
    protected void checkAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow row = GridDOA.HeaderRow;
            PnlSaveDoa.Visible = false;
            PnlMainDoaSellement.Visible = false;
            PnlDoaDispatch.Visible = false;
            lnkPrintDispatch.Visible = false;
            for (int i = 0; i < GridDOA.Rows.Count; i++)
            {
                if (((CheckBox)row.FindControl("checkAll")).Checked == true)
                {
                    GridDOA.Rows[i].BackColor = System.Drawing.Color.YellowGreen;
                    ((CheckBox)row.FindControl("checkAllDispatch")).Checked = false;
                    foreach (GridViewRow grv in GridDOA.Rows)
                    {
                        if (grv.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chk = (CheckBox)(grv.FindControl("chkDispatchRow"));
                            if (chk.Checked)
                            {
                                chk.Checked = false;
                            }
                        }

                    }
                    btnAcknowledgement.Visible = true;
                }
                else
                {
                    GridDOA.Rows[i].BackColor = System.Drawing.Color.White;
                    btnAcknowledgement.Visible = false;
                }
            }
        }
        catch (Exception exc)
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowError(exc.Message);
        }
    }
    protected void chkRow_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            btnAcknowledgement.Visible = false;
            PnlMainDoaSellement.Visible = false;
            PnlDoaDispatch.Visible = false;
            lnkPrintDispatch.Visible = false;
            PnlSaveDoa.Visible = false;
            GridViewRow row = GridDOA.HeaderRow;
            ((CheckBox)row.FindControl("checkAllDispatch")).Checked = false;
            for (int i = 0; i < GridDOA.Rows.Count; i++)
            {
                if (((CheckBox)GridDOA.Rows[i].FindControl("chkRow")).Checked == true)
                {
                    GridDOA.Rows[i].BackColor = System.Drawing.Color.YellowGreen;
                    foreach (GridViewRow grv in GridDOA.Rows)
                    {
                        if (grv.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chk = (CheckBox)(grv.FindControl("chkDispatchRow"));
                            if (chk.Checked)
                            {
                                chk.Checked = false;
                            }
                        }

                    }
                    btnAcknowledgement.Visible = true;
                }
                else
                {
                    GridDOA.Rows[i].BackColor = System.Drawing.Color.White;
                }
            }
        }
        catch (Exception exc)
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowError(exc.Message);
        }
    }
    protected void checkAllDispatch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow row = GridDOA.HeaderRow;
            PnlSaveDoa.Visible = false;
            btnAcknowledgement.Visible = false;
            PnlMainDoaSellement.Visible = false;
            lnkPrintDispatch.Visible = false;
            DispatchCotrolClear();
            for (int i = 0; i < GridDOA.Rows.Count; i++)
            {
                if (((CheckBox)row.FindControl("checkAllDispatch")).Checked == true)
                {
                    GridDOA.Rows[i].BackColor = System.Drawing.Color.YellowGreen;
                    ((CheckBox)row.FindControl("checkAll")).Checked = false;
                    foreach (GridViewRow grv in GridDOA.Rows)
                    {
                        if (grv.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chk = (CheckBox)(grv.FindControl("chkRow"));
                            if (chk.Checked)
                            {
                                chk.Checked = false;
                            }
                        }

                    }
                    PnlDoaDispatch.Visible = true;
                }
                else
                {
                    GridDOA.Rows[i].BackColor = System.Drawing.Color.White;
                    PnlDoaDispatch.Visible = false;
                }
            }
        }
        catch (Exception exc)
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowError(exc.Message);
        }
    }
    protected void chkDispatchRow_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            PnlDoaDispatch.Visible = false;
            PnlMainDoaSellement.Visible = false;
            btnAcknowledgement.Visible = false;
            PnlSaveDoa.Visible = false;
            lnkPrintDispatch.Visible = false;
            DispatchCotrolClear();
            GridViewRow row = GridDOA.HeaderRow;
            ((CheckBox)row.FindControl("checkAll")).Checked = false;
            for (int i = 0; i < GridDOA.Rows.Count; i++)
            {
                if (((CheckBox)GridDOA.Rows[i].FindControl("chkDispatchRow")).Checked == true)
                {
                    GridDOA.Rows[i].BackColor = System.Drawing.Color.YellowGreen;
                    foreach (GridViewRow grv in GridDOA.Rows)
                    {
                        if (grv.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chk = (CheckBox)(grv.FindControl("chkRow"));
                            if (chk.Checked)
                            {
                                chk.Checked = false;
                            }
                        }

                    }
                    PnlDoaDispatch.Visible = true;
                }
                else
                {
                    GridDOA.Rows[i].BackColor = System.Drawing.Color.White;
                }
            }
        }
        catch (Exception exc)
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowError(exc.Message);
        }
    }
}