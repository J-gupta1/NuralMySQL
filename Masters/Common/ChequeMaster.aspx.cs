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

public partial class Masters_Common_ChequeMaster : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (SalesChanelTypeID == 6)
            {
                pnlRDO.Visible = false;
                pnlMain.Visible = false;
                pnlSearch.Visible = true;
                pnlGrid.Visible = true;
                updsearch.Update();
                updView.Update();
                fillviewgrid();
            }
            fillsaleschannel();
            ucDateTo.Date = ToDate;
            UcDateFrom.Date = Fromdate;
            rdoSelect.ClearSelection();
            
        }

    }

    public void fillsaleschannel()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            DataTable dt = ObjSalesChannel.GetDistributerInfo();
            String[] colArray = { "SalesChannelID", "DisplayName" };
            PageBase.DropdownBinding(ref cmbSalesChannel, dt, colArray);
        }

    }

    public void fillviewgrid()
    {
        using (ProductData obj = new ProductData())
        {
            if (SalesChanelTypeID == 6)
            {
                obj.SalesChannelID = SalesChanelID;
            }
            else
            {
                obj.SalesChannelID = Convert.ToInt32(cmbSalesChannel.SelectedValue);
            }
                if (txtSerChkNo.Text == "")
            {
                obj.ChequeNumber = 0;
            }
            else
            {
                obj.ChequeNumber = Convert.ToInt32(txtSerChkNo.Text);
            }
            obj.ChequeStatus = Convert.ToInt32(cmbChequeStatus.SelectedValue);
            obj.DateFrom = UcDateFrom.Date;
            obj.DateTo = ucDateTo.Date;
            DataTable dt = obj.SelectChequeInfo();
            grdView.DataSource = dt;
            grdView.DataBind();
            updView.Update();

        }

    }

    protected void cmbSalesChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbSalesChannel.SelectedValue == "0")
        {
            rdoSelect.ClearSelection();
            pnlSubmitAmt.Visible = false;
            pnlEnterChq.Visible = false;
            pnlGrid.Visible = false;
            pnlSearch.Visible = false;
            updEnterCheque.Update();
            updSubmitAmt.Update();
            updView.Update();
            updsearch.Update();
            ucMessage1.Visible = false;
            return;
        }
        rdoSelect_SelectedIndexChanged(new object(), new EventArgs());
        pnlSearch.Visible = true;
        updsearch.Update();
        fillviewgrid();
        pnlGrid.Visible = true;
        updView.Update();


    }
    protected void rdoSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;

        if (cmbSalesChannel.SelectedValue == "0")
        {
            ucMessage1.ShowInfo("please select a Distributer");
            return;
        }
        if (rdoSelect.SelectedValue == "1")
        {
            pnlSubmitAmt.Visible = false;
            updSubmitAmt.Update();
            updView.Update();
            pnlEnterChq.Visible = true;
            updEnterCheque.Update();

        }
        if (rdoSelect.SelectedValue == "2")
        {
            updView.Update();
            pnlEnterChq.Visible = false;
            updEnterCheque.Update();
            pnlSubmitAmt.Visible = true;
            updSubmitAmt.Update();
        }





    }
    protected void btnInsertChk_Click(object sender, EventArgs e)
    {
        
        using (ProductData obj = new ProductData())
        {
            if (txtChequeFrom.Text != "")
            {
                obj.Checkfrom = Convert.ToInt32(txtChequeFrom.Text);
            }
            if (txtChequeTo.Text != "")
            {
                obj.CheckTo = Convert.ToInt32(txtChequeTo.Text);
            }
            if (txtChequeFrom.Text != "" && txtChequeTo.Text != "")
            {
                if (Convert.ToInt32(txtChequeFrom.Text) > Convert.ToInt32(txtChequeTo.Text))
                {
                    ucMessage1.ShowInfo("From Cheque sequence cant be greater than To cheque ");
                    return;
                }
            }
            obj.BankName = txtBank.Text;
            obj.AccountNo = txtAccountNo.Text;
            obj.SalesChannelID = Convert.ToInt32(cmbSalesChannel.SelectedValue);
            obj.InsUpdChequeDetails();
            if (obj.error != "")
            {
                ucMessage1.ShowInfo(obj.error);
                return;
            }
            ucMessage1.ShowSuccess("Cheques Created Sucessfully");
            txtAccountNo.Text = "";
            txtBank.Text = "";
            txtChequeFrom.Text = "";
            txtChequeTo.Text = "";

            updEnterCheque.Update();
            cmbChequeStatus.SelectedValue = "1";
            updsearch.Update();
            fillviewgrid();
            updView.Update();


        }

    }
    protected void btInsertCancel_Click(object sender, EventArgs e)
    {
        txtAccountNo.Text = "";
        txtBank.Text = "";
        txtChequeFrom.Text = "";
        txtChequeTo.Text = "";
        pnlEnterChq.Visible = false;
        updEnterCheque.Update();
        ucMessage1.Visible = false;
        rdoSelect.ClearSelection();
    }


    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        using (ProductData obj = new ProductData())
        {
            obj.SalesChannelID = Convert.ToInt32(cmbSalesChannel.SelectedValue);
            obj.Amount = Convert.ToDecimal(txtAmount.Text);
            obj.OrderNumber = txtOrderNumber.Text;
            obj.DepositionDate = ucDipdate.Date;
            obj.ChequeNumber = Convert.ToInt32(txtChqNo.Text);
            obj.InsUpdChequeDetails();
            if (obj.error != "")
            {
                ucMessage1.ShowInfo(obj.error);
                return;
            }
            ucMessage1.ShowSuccess("Cheques Deposited Sucessfully");
            txtAmount.Text = "";
            txtChqNo.Text = "";
            txtOrderNumber.Text = "";
            ucDipdate.Date = Convert.ToString(DateTime.Now);
            updSubmitAmt.Update();
            cmbChequeStatus.SelectedValue = "2";
            updsearch.Update();
            fillviewgrid();
            updView.Update();

        }


    }
    protected void btncancel_click(object sender, EventArgs e)
    {
        btnCancelUpdate_Click(new object(), new EventArgs());
        btInsertCancel_Click(new object(), new EventArgs());
        btnCancelSerch_Click(new object(), new EventArgs());
        pnlSubmitAmt.Visible = false;
        pnlEnterChq.Visible = false;
        pnlGrid.Visible = false;
        pnlSearch.Visible = false;
        updSubmitAmt.Update();
        updView.Update();
        updsearch.Update();
        updEnterCheque.Update();
        cmbSalesChannel.SelectedValue = "0";
        rdoSelect.ClearSelection();
        ucMessage1.Visible = false;



    }
    protected void btnCancelUpdate_Click(object sender, EventArgs e)
    {
        txtAmount.Text = "";
        ucDipdate.Date = Convert.ToString(DateTime.Now);
        pnlSubmitAmt.Visible = false;
        updSubmitAmt.Update();
        ucMessage1.Visible = false;
        rdoSelect.ClearSelection();

    }



    protected void grdView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdView.PageIndex = e.NewPageIndex;
        fillviewgrid();
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {

        fillviewgrid();

    }
    protected void btnCancelSerch_Click(object sender, EventArgs e)
    {
        txtSerChkNo.Text = "";
        cmbChequeStatus.SelectedValue = "0";
        ucDateTo.Date = ToDate;
        UcDateFrom.Date = Fromdate;
        updsearch.Update();

    }
}