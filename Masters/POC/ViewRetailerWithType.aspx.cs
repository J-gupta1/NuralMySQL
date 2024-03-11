using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;
using Cryptography;


public partial class Masters_HO_POC_ViewRetailerWithType : PageBase
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.ShowControl = false;
            if (!IsPostBack)
            {
                HideControls();
                FillsalesChannel();
                if (Convert.ToInt32(PageBase.SalesChanelID) != 0)             //this will select the login user in combo
                {
                    cmbsaleschannel.Items.FindByValue(Convert.ToString(PageBase.SalesChanelID)).Selected = true;
                    cmbsaleschannel.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }

    }
    void FillsalesChannel()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16( PageBase.SalesChanelTypeID);
          
                ObjSalesChannel.BilltoRetailer = true;
          
            string[] str = { "SalesChannelID", "SalesChannelName" };
            PageBase.DropdownBinding(ref cmbsaleschannel, ObjSalesChannel.GetSalesChannelInfo(), str);
        };
    }
    void HideControls()
    {
        ExportToExcel.Visible = false;

    }
    protected void LBAddRetailer_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageRetailerWithType.aspx");
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (cmbsaleschannel.SelectedValue == "0" && txtRetailername.Text == "")
            {
                ucMessage1.ShowInfo("Please Enter Atleast One Searching parameter ");
                return;
            }
            else
            {

                FillGrid();
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ucMessage1.ShowControl = false;
        txtRetailername.Text = "";
        dvhide.Visible = false;
        if (Convert.ToInt32(PageBase.SalesChanelID) != 0)             //this will select the login user in combo
        {
            cmbsaleschannel.Items.FindByValue(Convert.ToString(PageBase.SalesChanelID)).Selected = true;
            cmbsaleschannel.Enabled = false;
        }
        else
        {
            cmbsaleschannel.SelectedValue = "0";
            cmbsaleschannel.Enabled = true;
        }
    }
    void FillGrid()
    {
        DataTable Dt = new DataTable();
        using (RetailerData ObjRetailer = new RetailerData())
        {
            ObjRetailer.RetailerName = txtRetailername.Text.Trim();


            if (cmbsaleschannel.SelectedValue != "0")
            {
                ObjRetailer.SalesChannelID = Convert.ToInt16(cmbsaleschannel.SelectedValue);
            }

            Dt = ObjRetailer.GetRetailerInfoWithType();
        };
        if (Dt != null && Dt.Rows.Count > 0)
        {
            ViewState["Table"] = Dt;
            ExportToExcel.Visible = true;
            GridRetailer.Visible = true;
            GridRetailer.DataSource = Dt;
            GridRetailer.DataBind();
            dvhide.Visible = true;
        }
        else
        {
            dvhide.Visible = false;
            HideControls();
            GridRetailer.DataSource = null;
            GridRetailer.DataBind();
            ucMessage1.ShowInfo(Resources.Messages.NoRecord);
        }
        //UpdGrid.Update();
    }
    protected void GridRetailer_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridRetailer.PageIndex = e.NewPageIndex;
        FillGrid();
    }
    protected void GridRetailer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int CheckResult = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Int32 RetailerID = Convert.ToInt32(GridRetailer.DataKeys[e.Row.RowIndex].Value);
                using (RetailerData ObjRetailer = new RetailerData())
                {
                    ObjRetailer.SalesChannelID = RetailerID;
                    CheckResult = ObjRetailer.CheckRetailerExistence();
                };
                GridViewRow GVR = e.Row;

                ImageButton btnStatus = (ImageButton)GVR.FindControl("imgActive");

                HyperLink HLDetails = default(HyperLink);
                HLDetails = (HyperLink)GVR.FindControl("HLDetails");
                string strViewDBranchDtlURL = null;

                //strViewDBranchDtlURL = "ViewRetailerDetail.aspx?RetailerId=" + Crypto.Encrypt(Convert.ToString(RetailerID), PageBase.KeyStr);

                strViewDBranchDtlURL = Server.UrlEncode(Crypto.Encrypt(Convert.ToString(RetailerID), PageBase.KeyStr)).ToString().Replace("+", " ");
                {
                    HLDetails.Text = "Details";
                    //HLDetails.NavigateUrl = "#";
                    //HLDetails.Attributes.Add("OnClick", "popup('" + strViewDBranchDtlURL + "')");
                    HLDetails.Attributes.Add("onClick", string.Format("return popup('" + strViewDBranchDtlURL + "')"));
                }

                if (CheckResult > 0)
                {


                    if (btnStatus != null)
                    {
                        btnStatus.Attributes.Add("Onclick", "javascript:alert('This retailer is linked to existing data.You can not deactivate it.');{return false;}");

                    }
                }
              
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }


    }
    protected void GridRetailer_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int Result = 0;
        Int32 RetailerId = Convert.ToInt16(e.CommandArgument);
        try
        {

            if (e.CommandName == "Active")
            {

                if (RetailerId > 0)
                {
                    using (RetailerData ObjRetailer = new RetailerData())
                    {

                        ObjRetailer.RetailerID = RetailerId;
                        Result = ObjRetailer.UpdateStatusRetailerInfo();

                    };
                    if (Result > 0)
                    {
                        ucMessage1.ShowSuccess(Resources.Messages.StatusChanged);


                    }
                    else
                    {
                        ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);



                    }
                    FillGrid();
                }
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
        if (e.CommandName == "cmdEdit")
        {

            Response.Redirect("ManageRetailerWithType.aspx?RetailerId=" + Server.UrlEncode(Crypto.Encrypt(Convert.ToString(RetailerId), PageBase.KeyStr)));
        }

    }
    protected void ExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {

            if (ViewState["Table"] != null)
            {

                DataTable dt = (DataTable)ViewState["Table"];
                string[] DsCol = new string[] {"RetailerTypeName", "RetailerName", "RetailerCode", "SalesChannelName", "SalesmanName","ContactPerson", "Address1", "Address2", "StateName", "CityName", "DistrictName", "AreaName", "PinCode", "TinNumber", "MobileNumber", "StatusValue", "PhoneNumber", "Email" };
                DataTable DsCopy = new DataTable();
                dt = dt.DefaultView.ToTable(true, DsCol);
                dt.Columns["StatusValue"].ColumnName = "Status";
                dt.Columns["RetailerTypeName"].ColumnName = "Retailer Type";
                if (dt.Rows.Count > 0)
                {
                    DataSet dtcopy = new DataSet();
                    dtcopy.Merge(dt);
                    dtcopy.Tables[0].AcceptChanges();
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "RetailerList";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dtcopy, FilenameToexport);
                    ViewState["Table"] = null;
                }
                else
                {
                    ucMessage1.ShowError(Resources.Messages.NoRecord);

                }
                ViewState["Table"] = null;
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
            

    }
    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(PageBase.SalesChanelID) != 0)             //this will select the login user in combo
            {
                cmbsaleschannel.Items.FindByValue(Convert.ToString(PageBase.SalesChanelID)).Selected = true;
                cmbsaleschannel.Enabled = false;
            }
            else
            {
                cmbsaleschannel.SelectedValue = "0";
                cmbsaleschannel.Enabled = true;
            }
            txtRetailername.Text = "";
            FillGrid();
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    
}
