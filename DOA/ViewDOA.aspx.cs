using BussinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DOA_ViewDOA : PageBase
{
    clsDoaReport objreport = new clsDoaReport();
    public DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            btnAcknowledgement.Visible = false;
            BindDOAStatus();
            dvhide.Visible = false;
        }
    }
    protected void Search_Click(object sender, EventArgs e)
    {
        try
        {
            if ((txtDoaCertificateno.Text.Trim() == "" || txtDoaCertificateno.Text.Trim() == null) && (txtIMEINo.Text.Trim() == "" || txtIMEINo.Text.Trim() == null) && (ddldoastatus.SelectedValue == "0") && (ucFromDate.Date == "") && (ucToDate.Date == ""))
            {
                ucMessage1.Visible = true;
                ucMessage1.ShowWarning("Select OR Enter any search criteria!!!");
                dvhide.Visible = false;
                return;
            }
            else
            {
                bindSearchDOAData(1);
                dvhide.Visible = true;
                ucMessage1.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
    protected void Cancel_Click(object sender, EventArgs e)
    {

    }
    protected void btnAcknowledgement_Click(object sender, EventArgs e)
    {
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
                    DataSet dsResult = objreport.ReceiveAcknowledgementV1();
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


    
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if ((txtDoaCertificateno.Text.Trim() == "" || txtIMEINo.Text.Trim() == null) && (txtIMEINo.Text.Trim() == "" || txtIMEINo.Text.Trim() == null) && (ddldoastatus.SelectedValue == "0") && (ucFromDate.Date == "") && (ucToDate.Date == ""))
            {
                ucMessage1.ShowWarning("Select OR Enter any search criteria!!!");
                return;

            }
            else
            {
                ucMessage1.Visible = false;

                bindDOADataForExcel();
            }
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.Message.ToString());
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
        ds = objreport.GetReportDOAMotoDataExporttoExcel();
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            string FilenameToexport = "DOAData";
            
            PageBase.ExportToExecl(ds, FilenameToexport);
        }
        else
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowInfo("No Record Found.");
           
            
        }
        return ds;
    }  
   
    protected void checkAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow row = GridDOA.HeaderRow;
            for (int i = 0; i < GridDOA.Rows.Count; i++)
            {
                if (((CheckBox)row.FindControl("checkAll")).Checked == true)
                {
                    GridDOA.Rows[i].BackColor = System.Drawing.Color.YellowGreen;
                    //((CheckBox)row.FindControl("checkAllDispatch")).Checked = false;
                    foreach (GridViewRow grv in GridDOA.Rows)
                    {
                        if (grv.RowType == DataControlRowType.DataRow)
                        {
                            /*CheckBox chk = (CheckBox)(grv.FindControl("chkDispatchRow"));
                            if (chk.Checked)
                            {
                                chk.Checked = false;
                            }*/
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
            GridViewRow row = GridDOA.HeaderRow;
            //((CheckBox)row.FindControl("checkAllDispatch")).Checked = false;
            for (int i = 0; i < GridDOA.Rows.Count; i++)
            {
                if (((CheckBox)GridDOA.Rows[i].FindControl("chkRow")).Checked == true)
                {
                    GridDOA.Rows[i].BackColor = System.Drawing.Color.YellowGreen;
                    foreach (GridViewRow grv in GridDOA.Rows)
                    {
                        if (grv.RowType == DataControlRowType.DataRow)
                        {
                            /*CheckBox chk = (CheckBox)(grv.FindControl("chkDispatchRow"));
                            if (chk.Checked)
                            {
                                chk.Checked = false;
                            }*/
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
            DataSet ds = objreport.GetReportDOAMotoData();
            if (objreport.TotalRecords > 0)
            {
                GridDOA.DataSource = ds;
                GridDOA.DataBind();
                if(SalesChanelTypeID==7)
                {
                    GridDOA.Columns[0].Visible = true;
                }
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
            dtresult = clsDoaReport.GetEnumbyTableName("XML_Enum", "DOAMotoStatus");
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
}