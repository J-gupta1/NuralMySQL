using BussinessLogic;
using Cryptography;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DOA_ViewDoaSTN : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            BindReceiveStatus();
           // ucFromDate.Date = PageBase.Fromdate;
           // ucToDate.Date = PageBase.ToDate;
            dvhidegrid.Visible = false;
        }
    }
   
    protected void UCPagingControl1_SetControlRefresh()
    {
       
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        bindGrid(ucPagingControl1.CurrentPage);
       
    }
    protected void ChkViewDetails_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow gv = (GridViewRow)chk.NamingContainer;
            int rownumber = gv.RowIndex;
            string stockdispatchid = "";
            if (chk.Checked)
            {
                int i;
                for (i = 0; i <= gvSTNDetail.Rows.Count - 1; i++)
                {
                    if (i != rownumber)
                    {
                        CheckBox chkcheckbox = ((CheckBox)(gvSTNDetail.Rows[i].FindControl("ChkViewDetails")));
                        chkcheckbox.Checked = false;
                    }
                }
                stockdispatchid = gvSTNDetail.DataKeys[gv.RowIndex].Value.ToString();
               
                if (stockdispatchid != "0")
                {
                    using (clsDoaReport objreport = new clsDoaReport())
                    {
                        objreport.StockDispatchID = Convert.ToInt64(stockdispatchid);
                       DataSet dsResult = objreport.GetSTNDEtails();
                       
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            dvhidegrid.Visible = true;
                            ucMessage1.Visible = false;
                            GridDOA.DataSource = dsResult;
                            GridDOA.DataBind();
                        }
                        else
                        {
                            dsResult = null;
                            ucMessage1.Visible = true;
                            ucMessage1.ShowInfo("No Record Found");
                            GridDOA.DataSource = null;
                            GridDOA.DataBind();
                        }
                    }
                }

            }
            for (int j = 0; j < gvSTNDetail.Rows.Count; j++)
            {
                if (((CheckBox)gvSTNDetail.Rows[j].FindControl("ChkViewDetails")).Checked == true)
                {
                    gvSTNDetail.Rows[j].BackColor = System.Drawing.Color.YellowGreen;
                }
                else
                {
                    gvSTNDetail.Rows[j].BackColor = System.Drawing.Color.White;
                }
            }
        }
        catch (Exception ex)
        {

            ucMessage1.Visible = true;
            ucMessage1.ShowError(ex.Message);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.Visible = false;
            if (ucFromDate.Date == "" && ucToDate.Date == "" && txtSTNNo.Text.Trim() == "" && txtDoaCertificatenumber.Text.Trim() == "" && txtIMEINo.Text.Trim() == "" && ddlReceivestatus.SelectedValue == "255" && txtGRNNO.Text.Trim() == "")
            {
                ucMessage1.Visible = true;
                dvhidegrid.Visible = false;
               
                ucMessage1.ShowWarning("Please enter at least one field.");
                return;
            }
           else if (ucFromDate.Date != "" && ucToDate.Date == "")
            {
                ucMessage1.ShowWarning("Please enter Dispatch To Date.");
                return;
            }
            else if (ucFromDate.Date == "" && ucToDate.Date != "")
            {
                ucMessage1.ShowWarning("Please enter Dispatch From Date.");
                return;
            }
            else
            {
                
                bindGrid(1);
            }
            
        }
        catch (Exception ex)
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowError(ex.Message);
        }
       
    }
    protected void btnExportexcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (ucFromDate.Date == "" && ucToDate.Date == "" && txtSTNNo.Text.Trim() == "" && txtDoaCertificatenumber.Text.Trim() == "" && txtIMEINo.Text.Trim() == "" && ddlReceivestatus.SelectedValue == "255" && txtGRNNO.Text.Trim() == "")
            {
                ucMessage1.Visible = true;
                dvhidegrid.Visible = false;
                ucMessage1.ShowWarning("Please enter at least one field.");
                return;
            }
            else
            {

                ExportSTNData();
            }

        }
        catch (Exception ex)
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowError(ex.Message);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
    }
    private void BindReceiveStatus()
    {
        DataTable dtresult = new DataTable();
        try
        {
            dtresult = clsDoaReport.GetEnumbyTableName("XML_Enum", "ReceiveType");
            if (dtresult.Rows.Count > 0)
            {
                ddlReceivestatus.DataSource = dtresult;
                ddlReceivestatus.DataTextField = "Description";
                ddlReceivestatus.DataValueField = "Value";
                ddlReceivestatus.DataBind();
                ddlReceivestatus.Items.Insert(0, new ListItem("Select", "255"));
            }
            else
            {
                ddlReceivestatus.Items.Insert(0, new ListItem("Select", "255"));
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
    public void bindGrid(int pageno)
    {
        clsDoaReport objreport = new clsDoaReport();
        DataSet dsResult = new DataSet();
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
            if (txtDoaCertificatenumber.Text != "" && txtDoaCertificatenumber.Text != null)
            {
                objreport.DOACertificateNumber = txtDoaCertificatenumber.Text.Trim();
            }
            if (txtIMEINo.Text != "" && txtIMEINo.Text != null)
            {
                objreport.IMEINumber = txtIMEINo.Text.Trim();
            }
            if (txtSTNNo.Text != "" && txtSTNNo.Text != null)
            {
                objreport.STNNumber = txtSTNNo.Text.Trim();
            }
            if (txtGRNNO.Text != "" && txtGRNNO.Text != null)
            {
                objreport.GRNNumber = txtGRNNO.Text.Trim();
            }
            objreport.Receivestatus =Convert.ToInt16(ddlReceivestatus.SelectedValue.ToString());
            objreport.LoginUserId = PageBase.UserId;
            objreport.PageIndex = pageno;
            objreport.PageSize = Convert.ToInt32(PageBase.PageSize);
            objreport.SalesChannelId = PageBase.SalesChanelID;
            dsResult = objreport.BindViewSTNData();
            if (objreport.TotalRecords > 0)
            {
                gvSTNDetail.DataSource = dsResult;
                gvSTNDetail.DataBind();
                PnlViewStnGrid.Visible = true;
                dvhidegrid.Visible = false;
                ucPagingControl1.Visible = true;
                ViewState["TotalRecords"] = objreport.TotalRecords;
                ucPagingControl1.TotalRecords = objreport.TotalRecords;
                ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                ucPagingControl1.SetCurrentPage = pageno;
                ucPagingControl1.FillPageInfo();
                
              
            }
            else
            {
                dsResult = null;
                gvSTNDetail.DataSource = null;
                gvSTNDetail.DataBind();
                ucPagingControl1.Visible = false;
                dvhidegrid.Visible = false;
                ucMessage1.Visible = true;
                ucMessage1.ShowInfo("NO Record Found.");
               
            }
        }
        catch (Exception ex)
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void gvSTNDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridViewRow GVR = e.Row;
            Int32 stockdispatchID = Convert.ToInt32(gvSTNDetail.DataKeys[e.Row.RowIndex].Value);
            Button debitnote = default(Button);
            debitnote = (Button)GVR.FindControl("btndebitprint");
            debitnote.Attributes.Add("OnClick", "return Popup('" + Cryptography.Crypto.Encrypt(stockdispatchID.ToString().Replace(" ", "+"), PageBase.KeyStr) + "');");
            if (PageBase.SalesChanelTypeID==14)
            {
                int ReceiveID = 0;
                Label strTmp = ((Label)e.Row.FindControl("lblGrnprint"));
                ReceiveID = Convert.ToInt32(strTmp.Text);
                Button btnGrnprint = default(Button);
                btnGrnprint = (Button)GVR.FindControl("btnGrnprint");
                btnGrnprint.Attributes.Add("OnClick", "return Popupgrn('" + Cryptography.Crypto.Encrypt(ReceiveID.ToString().Replace(" ", "+"), PageBase.KeyStr) + "');");
            }
            else if(PageBase.EntityTypeID==1 && PageBase.BaseEntityTypeID==2)
            {
                int ReceiveID = 0;
                Label strTmp = ((Label)e.Row.FindControl("lblGrnprint"));
                ReceiveID = Convert.ToInt32(strTmp.Text);
                Button btnGrnprint = default(Button);
                btnGrnprint = (Button)GVR.FindControl("btnGrnprint");
                btnGrnprint.Attributes.Add("OnClick", "return Popupgrn('" + Cryptography.Crypto.Encrypt(ReceiveID.ToString().Replace(" ", "+"), PageBase.KeyStr) + "');");
            }
            else
            {
                gvSTNDetail.Columns[2].Visible = false;
            }
        }  
    }

    private void Clear()
    {
        txtDoaCertificatenumber.Text = string.Empty;
        txtIMEINo.Text = string.Empty;
        txtSTNNo.Text = string.Empty;
        ddlReceivestatus.SelectedValue = "255";
        ucFromDate.Date = "";
        ucToDate.Date = "";
        PnlViewStnGrid.Visible = false;
        dvhidegrid.Visible = false;
        ucMessage1.Visible = false;
    }

    private DataSet ExportSTNData()
    {
        DataSet ds = new DataSet();
        clsDoaReport objreport = new clsDoaReport();
        try
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
            if (txtDoaCertificatenumber.Text.Trim() != "" && txtDoaCertificatenumber.Text.Trim() != null)
            {
                objreport.DOACertificateNumber = txtDoaCertificatenumber.Text.Trim();
            }
            objreport.Receivestatus = Convert.ToInt16(ddlReceivestatus.SelectedValue);
            objreport.SalesChannelId = PageBase.SalesChanelID;
            objreport.LoginUserId = PageBase.UserId;
            ds = objreport.ViewSTNReports();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
               
                string FilenameToexport = "STNReports";
                PageBase.ExportToExecl(ds, FilenameToexport);
            }
            else
            {
                ucMessage1.ShowInfo("NO Record Found.");

            }
            return ds;
        }
        catch (Exception ex)
        {
            return ds;
        }

    }
   
}