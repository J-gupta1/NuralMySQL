using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using BussinessLogic;

public partial class DOA_ReceiveDispatchDoa : PageBase
{
    clsDoaReport objreport = new clsDoaReport();
    DataSet dsResult = new DataSet();
    public string stockdispatchid = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //ucFromDate.Date = PageBase.Fromdate;
            //ucToDate.Date = PageBase.ToDate;
            dvReceiveRemark.Visible = false;
            ucMessage1.Visible = false;
            dvhidegrid.Visible = false;
            ViewState["GetTable"] = null;
            lnkPrintDispatch.Visible = false;
            BindReceiveStatus();
        }
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
    protected void Search_Click(object sender, EventArgs e)
    {
        try
        {
            if (ucFromDate.Date == "" && ucToDate.Date == "" && txtSTNNo.Text.Trim() == "" && txtDoaCertificatenumber.Text.Trim() == "" && txtIMEINo.Text.Trim() == "")
            {
                ucMessage1.Visible = true;
                lnkPrintDispatch.Visible = false;
              
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
                PnlViewStnGrid.Visible = true;
                ucMessage1.Visible = false;
                lnkPrintDispatch.Visible = false;
                bindGrid(1);
            }
        }
        catch (Exception ex)
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowError(ex.Message);
        }
    }
    protected void Cancel_Click(object sender, EventArgs e)
    {
        dvReceiveRemark.Visible = false;
        PnlViewStnGrid.Visible = false;
        dvhidegrid.Visible = false;
        txtReceiveRemark.Text = string.Empty;
        txtDoaCertificatenumber.Text = string.Empty;
        txtIMEINo.Text = string.Empty;
        txtSTNNo.Text = string.Empty;
        lnkPrintDispatch.Visible = false;
       // ucFromDate.Date = PageBase.Fromdate;
       // ucToDate.Date = PageBase.ToDate;
        ucFromDate.Date = "";
        ucToDate.Date = "";
        ucMessage1.Visible = false;
        hdfstockdispatchid.Value = "";
        txtcgst.Text = string.Empty;
        txtsgst.Text = string.Empty;
        txtigst.Text = string.Empty;
        txtutgst.Text = string.Empty;
    }

    protected void UCPagingControl1_SetControlRefresh()
    {
        ucMessage1.Visible = false;
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        bindGrid(ucPagingControl1.CurrentPage);
        lnkPrintDispatch.Visible = false;

    }
    public void bindGrid(int pageno)
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
            if(txtInvoiceno.Text!="" && txtInvoiceno.Text!=null)
            {
                objreport.InvoiceNo = txtInvoiceno.Text.Trim();
            }
            objreport.LoginUserId = PageBase.UserId;
            objreport.PageIndex = pageno;
            objreport.PageSize = Convert.ToInt32(PageBase.PageSize);
            objreport.Receivestatus = Convert.ToInt16(ddlReceivestatus.SelectedValue);
            dsResult = objreport.BindSTNDataR();
            if (objreport.TotalRecords > 0)
            {
                gvSTNDetail.DataSource = dsResult;
                gvSTNDetail.DataBind();
                PnlViewStnGrid.Visible = true;
                dvhidegrid.Visible = false;
                dvReceiveRemark.Visible = false;
                // lnkPrintDispatch.Visible = false;
                ucPagingControl1.Visible = true;
                ViewState["TotalRecords"] = objreport.TotalRecords;
                ucPagingControl1.TotalRecords = objreport.TotalRecords;
                ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                ucPagingControl1.SetCurrentPage = pageno;
                ucPagingControl1.FillPageInfo();
                //ucMessage1.Visible = false;
            }
            else
            {
                dsResult = null;
                gvSTNDetail.DataSource = null;
                gvSTNDetail.DataBind();
                ucPagingControl1.Visible = false;
                dvhidegrid.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void ChkViewDetails_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow gv = (GridViewRow)chk.NamingContainer;
            dvReceiveRemark.Visible = true;
            txtReceiveRemark.Text = string.Empty;
            lnkPrintDispatch.Visible = false;
            txtcgst.Text = string.Empty;
            txtsgst.Text = string.Empty;
            txtigst.Text = string.Empty;
            txtutgst.Text = string.Empty;
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
                hdfstockdispatchid.Value = stockdispatchid;
                if (stockdispatchid != "0")
                {
                    using (clsDoaReport objreport = new clsDoaReport())
                    {
                        objreport.StockDispatchID = Convert.ToInt64(stockdispatchid);
                        dsResult = objreport.GetSTNDEtails();
                       // ViewState["GetTable"] = dsResult;
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            string lblstatus = dsResult.Tables[0].Rows[0]["receiveAllReady"].ToString();
                            int valuestatus = Convert.ToInt32(lblstatus);
                            if (valuestatus == 1)
                            {
                                dvReceiveRemark.Visible = false;
                            }
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
    protected void ReceiveDispatch_Click(object sender, EventArgs e)
    {
        DataTable dtDispatchItem = new DataTable();
        RadioButtonList Rbtlist = new RadioButtonList();
        string StockStockDispatchItemID = string.Empty;
        string cgst = string.Empty;
        string sgst = string.Empty;
        string igst = string.Empty;
        string utgst = string.Empty;
        //string lblstatus = "";
        //DataSet dsvalue = new DataSet();
        //dsvalue = (DataSet)ViewState["GetTable"];
        //lblstatus = dsvalue.Tables[0].Rows[0]["receiveAllReady"].ToString();
        //int valuestatus = Convert.ToInt32(lblstatus);
        //if (valuestatus == 1)
        //{
        //    ucMessage1.Visible = true;
        //    ucMessage1.ShowWarning("Stock already received.");
        //}
      //  else
      //  {        
            Int64 StockDispatchId = Convert.ToInt64(hdfstockdispatchid.Value);
            try
            {
                dtDispatchItem.Columns.Add("StockDispatchItemID");
                dtDispatchItem.Columns.Add("ReceiveFlag");
                foreach (GridViewRow grv in GridDOA.Rows)
                {
                    if (grv.RowType == DataControlRowType.DataRow)
                    {
                        StockStockDispatchItemID = GridDOA.DataKeys[grv.RowIndex].Value.ToString();
                        Int64 stockid = Convert.ToInt64(StockStockDispatchItemID);
                        Rbtlist = (RadioButtonList)(grv.FindControl("rbtReceive"));
                        if (Rbtlist.SelectedIndex != -1)
                        {
                            DataRow dr = null;
                            dr = dtDispatchItem.NewRow();
                            dr["StockDispatchItemID"] = stockid;
                            dr["ReceiveFlag"] = Rbtlist.SelectedValue;
                            dtDispatchItem.Rows.Add(dr);
                            dtDispatchItem.AcceptChanges();
                        }
                        else
                        {
                            ucMessage1.Visible = true;
                            ucMessage1.ShowWarning("Please select every record in list.");
                            return;
                        }
                        if (StockStockDispatchItemID == string.Empty && StockStockDispatchItemID != null)
                        {
                            ucMessage1.Visible = true;
                            ucMessage1.ShowWarning("Please select all Record in list.");
                            return;
                        }
                    }
                }
                if (txtReceiveRemark.Text == "")
                {
                    ucMessage1.Visible = true;
                    ucMessage1.ShowWarning("Please enter receive remark");
                    return;
                }
                //Start Validate CGST Tax.
                if(txtcgst.Text!="")
                {
                    if(txtutgst.Text=="" && txtsgst.Text=="")
                    {
                        ucMessage1.Visible = true;
                        ucMessage1.ShowWarning("Please enter SGST/UTGST.");
                        return;
                    }
                    if(txtigst.Text!="")
                    {
                        ucMessage1.Visible = true;
                        ucMessage1.ShowWarning("When input CGST/SGST/UTGST  then IGST  not allowed.");
                        return;
                    }
                    if(txtutgst.Text!="")
                    {
                        if(txtsgst.Text!="")
                        {
                            ucMessage1.Visible = true;
                            ucMessage1.ShowWarning("Please enter CGST/SGST.");
                            return;
                        }
                    }
                    if(txtsgst.Text!="")
                    {
                        if(txtutgst.Text!="")
                        {
                            ucMessage1.Visible = true;
                            ucMessage1.ShowWarning("Please enter UTGST/CGST.");
                            return;
                        }
                    }
                }
                //End CGST Tax.

                //Start Validate IGST Tax.
                if(txtigst.Text!="")
                {
                    if(txtcgst.Text!="" || txtsgst.Text!="" || txtutgst.Text!="")
                    {
                        ucMessage1.Visible = true;
                        ucMessage1.ShowWarning("When input IGST  then CGST/SGST/UTGST not allowed.");
                        return;
                    }
                }
                //End IGST.

                //Start validate SGST
                if(txtsgst.Text!="")
                {
                    if(txtcgst.Text=="" && txtutgst.Text=="")
                    {
                        ucMessage1.Visible = true;
                        ucMessage1.ShowWarning("Please enter CGST/UTGST.");
                        return;
                    }
                    if (txtigst.Text != "")
                    {
                        ucMessage1.Visible = true;
                        ucMessage1.ShowWarning("When input CGST/SGST/UTGST then IGST not allowed.");
                        return;
                    }
                    if(txtcgst.Text!="")
                    {
                        if(txtutgst.Text!="")
                        {
                            ucMessage1.Visible = true;
                            ucMessage1.ShowWarning("Please enter SGST/CGST");
                            return;
                        }
                    }
                    if(txtutgst.Text!="")
                    {
                        if(txtcgst.Text!="")
                        {
                            ucMessage1.Visible = true;
                            ucMessage1.ShowWarning("Please enter SGST/CGST");
                            return;
                        }
                    }
                    if(txtutgst.Text!="")
                    {
                        ucMessage1.Visible = true;
                        ucMessage1.ShowWarning("When input SGST then UTGST not allowed.");
                        return;
                    }
                }
                //Start validate UTGST
                if (txtutgst.Text != "")
                {
                    if (txtcgst.Text == "" && txtsgst.Text == "")
                    {
                        ucMessage1.Visible = true;
                        ucMessage1.ShowWarning("Please enter CGST/SGST.");
                        return;
                    }
                    if (txtigst.Text != "")
                    {
                        ucMessage1.Visible = true;
                        ucMessage1.ShowWarning("When input CGST/SGST/UTGST then IGST not allowed.");
                        return;
                    }
                    if(txtcgst.Text!="")
                    {
                        if(txtsgst.Text!="")
                        {
                            ucMessage1.Visible = true;
                            ucMessage1.ShowWarning("Please enter UTGST/SGST.");
                            return;
                        }
                    }
                    if(txtsgst.Text!="")
                    {
                        if(txtcgst.Text!="")
                        {
                            ucMessage1.Visible = true;
                            ucMessage1.ShowWarning("Please enter CGST/UTGST.");
                            return;
                        }
                    }
                    if(txtsgst.Text!="")
                    {
                        ucMessage1.Visible = true;
                        ucMessage1.ShowWarning("When input UTGST then SGST not allowed.");
                        return;
                    }
                }
                
                if(txtcgst.Text=="" || txtcgst.Text==null)
                {
                     cgst = "0.00";
                }
                else
                {
                    cgst = txtcgst.Text.Trim();
                }
                if(txtsgst.Text=="" || txtsgst.Text==null)
                {
                    sgst = "0.00";
                }
                else
                {
                    sgst = txtsgst.Text.Trim();
                }
                if (txtigst.Text == "" || txtigst.Text == null)
                {
                    igst = "0.00";
                }
                else
                {
                    igst = txtigst.Text.Trim();
                }
                if (txtutgst.Text == "" || txtutgst.Text == null)
                {
                    utgst = "0.00";
                }
                else
                {
                    utgst = txtutgst.Text.Trim();
                }
                using (clsDoaReport objreport = new clsDoaReport())
                {
                    objreport.LoginUserId = PageBase.UserId;
                    objreport.StockReceiveId = dtDispatchItem;
                    objreport.ReceiveRemark = txtReceiveRemark.Text.Trim();
                    objreport.CGST = Convert.ToDecimal(cgst);
                    objreport.SGST =Convert.ToDecimal(sgst);
                    objreport.IGST =Convert.ToDecimal(igst);
                    objreport.UTGST =Convert.ToDecimal(utgst);
                    objreport.StockDispatchID = StockDispatchId;
                    DataSet dsResult = objreport.SaveUpdateDispatchRecord();
                    Int16 Result = objreport.OutParam;
                    Int64 stockReceiveId = objreport.StockReceiveIdForPrint;
                    int pageno = Convert.ToInt32(ViewState["CurrentPage"]);
                    if (Result == 0)
                    {
                        ucMessage1.Visible = true;
                        ucMessage1.ShowSuccess("Dispatch Stock Receive successfully.");
                        dvReceiveRemark.Visible = false;
                        bindGrid(pageno);
                        txtReceiveRemark.Text = string.Empty;
                        txtcgst.Text = string.Empty;
                        txtsgst.Text = string.Empty;
                        txtigst.Text = string.Empty;
                        txtutgst.Text = string.Empty;
                        lnkPrintDispatch.Visible = true;
                        lnkPrintDispatch.Attributes.Add("OnClick", "return Popup('" + Cryptography.Crypto.Encrypt(stockReceiveId.ToString().Replace(" ", "+"), PageBase.KeyStr) + "');");
                        hdfstockdispatchid.Value = "";
                        ViewState["GetTable"] = null;
                        dtDispatchItem = null;
                    }
                    else if (dsResult.Tables.Count > 0 && Result == 1)
                    {
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {

                            if (dsResult.Tables[0].Rows.Count > 0)
                            {
                                ucMessage1.Visible = true;
                                ucMessage1.XmlErrorSource = dsResult.GetXml();
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
       // }
    }
}