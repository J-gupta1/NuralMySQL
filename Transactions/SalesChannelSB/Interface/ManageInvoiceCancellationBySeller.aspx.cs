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
public partial class Transactions_SalesChannelSB_Interface_ManageInvoiceCancellationBySeller : PageBase
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
        }//Pankaj Kumar


        return true;
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


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            gvAck.PageIndex = 0;
            gvAck.DataSource = null;
            gvAck.DataBind();
            updGrid.Update();
      
            ClearOutput();
            dtNew = new DataTable();
            if (!ChckDateInput())
            {
                ucMsg.ShowError("Invalid Date format");
                return;
            }
            if (pageValidateSave())
            {
                dtNew = GetInvoiceListForCancellation(2, 0);
                if (dtNew.Rows.Count > 0)
                {
                    ViewState["Type"] = dtNew.Rows[0]["Type"].ToString();
                    gvAck.PageIndex = 0;
                    gvAck.DataSource = dtNew;
                    gvAck.DataBind();
                    pnlGrid.Visible = true;

                }
                else
                {
                    ucMsg.ShowInfo(Resources.Messages.NoRecord);
                }
                upddetails.Update();
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
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearForm();

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
        ddlStatus.SelectedValue = "0";
        txtInvoice.Text = "";
        updGrid.Update();
        upddetails.Update();
    }

    void ClearOutput()
    {
        pnlGrid.Visible = false;
        pnldetail.Visible = false;
        updGrid.Update();
        upddetails.Update();
        ViewState["Type"] = null;
    }

   

    protected void btnReject_Click(object sender, EventArgs e)
    {
        try
        {
            CancellationAction(4);
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }


    protected void gvAck_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            dtNew = new DataTable();
            Int32 id = Convert.ToInt32(e.CommandArgument);
            Session["ID"] = id;
            if (e.CommandName == "Details")
            {
                Button imgbtn = (Button)e.CommandSource;      //Only this code will change the Selected row css
                GridViewRow grdrow = (GridViewRow)imgbtn.NamingContainer;
                GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                string strValue = ((Label)(row.Cells[0].FindControl("lblReceived"))).Text;
                dtNew = GetInvoiceListForCancellation(1, id);
                if (dtNew.Rows.Count > 0)
                {
                    grdDetails.DataSource = dtNew;
                    grdDetails.DataBind();
                    pnldetail.Visible = true;

                }
                else
                {
                    grdDetails.DataSource = null;
                    grdDetails.DataBind();
                    pnldetail.Visible = true;
                }
                if (Convert.ToInt32(strValue) != 0)
                    btnReject.Visible = false;
                else
                    btnReject.Visible = true;
                upddetails.Update();

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }

    private void CancellationAction(int value)
    {

        using (SalesData obj = new SalesData())
        {
            obj.value = value;
            obj.SalesUniqueID = Convert.ToInt32(Session["ID"]);
            obj.UserID = PageBase.UserId;
            obj.Decider = Convert.ToInt32(ViewState["Type"]);
            obj.SalesChannelID = PageBase.SalesChanelID;
            obj.OtherEntity = PageBase.BaseEntityTypeID;
            int result = obj.InsertCancellationInformationSB();
            if (result == 0)
            {
                if (value == 4)
                    ucMsg.ShowSuccess(Resources.Messages.InvoiceCancelled);

            }
            if (result == 1)
                ucMsg.ShowError(obj.Error.ToString());
            if (result == 5)
                ucMsg.ShowInfo(Resources.GlobalMessages.InvoiceAlreadyAck);
            ClearForm();
            ClearOutput();


        }
    }
    protected void grdDetails_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
    {
        try
        {
            if (!e.Expanded) return;
            grdDetails.DetailRows.CollapseAllRows();
            grdDetails.DetailRows.ExpandRow(e.VisibleIndex);
            grdDetails.DetailRows.IsVisible(e.VisibleIndex);
            ASPxGridView objDetail = (ASPxGridView)(sender as ASPxGridView).FindDetailRowTemplateControl(e.VisibleIndex, "detailGrid");
            objDetail.DataSource = Session["Detail"];
            objDetail.DataBind();


        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }

    }
    protected void detailGrid_DataSelect(object sender, EventArgs e)
    {
        try
        {
            dtNew = new DataTable();
            intSelectedValue = Convert.ToInt32((sender as ASPxGridView).GetMasterRowKeyValue());
            dtNew = GetInvoiceListForCancellation(3, intSelectedValue);
            ASPxGridView objDetail = (ASPxGridView)(sender as ASPxGridView);
            objDetail.DataSource = dtNew;
            Session["Detail"] = dtNew;
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }

    }


    private DataTable GetInvoiceListForCancellation(int value, int uniqueID)
    {
        DataTable dt = new DataTable();
        using (SalesData objSales = new SalesData())
        {
            if (txtInvoice != null)
            {
                objSales.SalesChannelID = PageBase.SalesChanelID;
                objSales.OtherEntity = PageBase.BaseEntityTypeID;
                objSales.UserID = PageBase.UserId;
                objSales.InvoiceNumber = txtInvoice.Text.Trim();
                objSales.FromDate = ucFromDate.Date != "" ? Convert.ToDateTime(ucFromDate.Date) : objSales.InvoiceDate;
                objSales.ToDate = ucToDate.Date != "" ? Convert.ToDateTime(ucToDate.Date) : objSales.InvoiceDate;
                objSales.AckStatus = Convert.ToInt32(ddlStatus.SelectedValue);
                objSales.FlagForTable = value;
                objSales.SalesUniqueID = uniqueID;
                dt = objSales.GetInvoiceListForCancellation();
            }
        }
        return dt;
    }
    protected void gvAck_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dtNew = new DataTable();
            gvAck.PageIndex = e.NewPageIndex;
            dtNew = GetInvoiceListForCancellation(2, 0);
            if (dtNew.Rows.Count > 0)
            {
                gvAck.DataSource = dtNew;
                gvAck.DataBind();
                pnlGrid.Visible = true;

            }
            else
            {
                ucMsg.ShowInfo(Resources.Messages.NoRecord);
            }
            updGrid.Update();
        }
        catch (Exception ex)
        {

            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
    protected void grdDetails_DetailRowGetButtonVisibility(object sender, ASPxGridViewDetailRowButtonEventArgs e)
    {
        try
        {
            ASPxGridView grid = (ASPxGridView)sender;
            if (grid.GetRow(e.VisibleIndex) != null)
            {
                if (Convert.ToInt32(((System.Data.DataRowView)(grid.GetRow(e.VisibleIndex))).Row.ItemArray[7]) == 1)
                {
                    e.ButtonState = GridViewDetailRowButtonState.Hidden;
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }


    protected void page_Init(object sender, EventArgs e)
    {
        try
        {
            if (Session["ID"] != null)
            {
                dtNew = GetInvoiceListForCancellation(1, Convert.ToInt32(Session["ID"]));
                if (grdDetails != null)
                {
                    if (dtNew.Rows.Count > 0)
                    {
                        grdDetails.DataSource = dtNew;
                        grdDetails.DataBind();
                        pnldetail.Visible = true;
                    }
                    else
                    {
                        grdDetails.DataSource = null;
                        grdDetails.DataBind();
                        pnldetail.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }

    }
    protected void rdModelList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdModelList.SelectedValue == "0")
            Response.Redirect("~/Transactions/SalesChannelSB/Upload/ManageInvoiceCancellationBySellerUpload.aspx");
    }
  

}
