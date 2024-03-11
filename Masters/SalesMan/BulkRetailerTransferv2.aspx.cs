/*
====================================================================================================================================
 *Change Log: 
 *DD-MMM-YYYY, Name , #CCXX - Description
------------------------------------------------------------------------------------------------------------------------------------
 * 22-Mar-2016, Sumit Maurya, #CC01, Issue of page getting blocked on the execution of event(s) due to update panel resolved.
 * 04-Aug-2016, Karam Chand Sharma, #CC02,Fixing :  If salesman config is 0 than grid was not binding
 * 31-Aug-2016, Sumit Maurya , #CC03, Check added to prevent displaying message (Please select reporting herarchy.) if Config valueof "RETAILERHIERLVLID" is 0.
====================================================================================================================================
 */
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


public partial class Masters_SalesMan_BulkRetailerTransferv2 : PageBase
{
    DataTable ExportDt = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        ucMessage1.Visible = false;
        ucMessage2.Visible = false;

        if (!IsPostBack)
        {


            cmbSalesManTo.Items.Insert(0, new ListItem("Select", "0"));
            cmbSalesManfrom.Items.Insert(0, new ListItem("Select", "0"));
            ddlOrghierarchy.Items.Insert(0, new ListItem("Select", "0"));
            ddlOrghierarchy.Enabled = false;
            cmbSalesManfrom.Enabled = false;
            cmbSalesManTo.Enabled = false;
            fillsalesChannel();
            if (PageBase.SALESMANOPTIONAL == "1")
            {
                RequSalesManFrom.Enabled = false;
                RequCombo.Enabled = false;
                saleFrom.Visible = false;
                saleTo.Visible = false;
            }
        }
    }
    public void fillsalesChannel()
    {
        using (SalesChannelData obj = new SalesChannelData())
        {
            DataSet ds = obj.GetParentForRetailerTransfer();
            String[] colArray = { "SalesChannelID", "SalesChannelName" };
            PageBase.DropdownBinding(ref cmbTransferFrom, ds.Tables[0], colArray);
            PageBase.DropdownBinding(ref cmbTransferTo, ds.Tables[1], colArray);
        }
    }
    protected void cmbTransferFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        RefreshData();
        cmbTransferFrom.Enabled = false;
        UpdatePanel1.Update();
        updLoad.Update();
        DataTable dtNew = new DataTable();
        if (cmbTransferFrom.SelectedIndex != 0)
        {
            cmbSalesManfrom.Items.Clear();
            cmbSalesManfrom.ClearSelection();

            cmbSalesManTo.Items.Clear();
            cmbSalesManTo.Items.Insert(0, new ListItem("Select", "0"));

            using (SalesmanData ObjSalesman = new SalesmanData())
            {
                ObjSalesman.Type = EnumData.eSearchConditions.Active;
                ObjSalesman.SalesChannelID = Convert.ToInt32(cmbTransferFrom.SelectedValue);
                dtNew = ObjSalesman.GetSalesmanInfo();
                if (dtNew.Rows.Count > 0)
                {
                    String[] StrCol = new String[] { "SalesmanID", "Salesman" };
                    PageBase.DropdownBinding(ref cmbSalesManfrom, dtNew, StrCol);
                    cmbSalesManfrom.Enabled = true;
                }


            };
        }
        else
        {
            cmbSalesManfrom.Items.Clear();
            cmbSalesManfrom.Items.Insert(0, new ListItem("Select", "0"));
        }

        /*#CC02 START COMMENTED if (PageBase.SALESMANOPTIONAL == "1")
        {
            using (RetailerData obj = new RetailerData())
            {
                getretailerfromdata(1);
            }
        }#CC02 END COMMENTED */
        getretailerfromdata(1);/*#CC02 ADDED*/
    }
    protected void cmbTransferTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        if (cmbTransferTo.SelectedIndex != 0)
        {
            DataTable dt = new DataTable();
            cmbSalesManTo.Items.Clear();
            using (SalesmanData ObjSalesman = new SalesmanData())
            {
                ObjSalesman.Type = EnumData.eSearchConditions.Active;
                ObjSalesman.SalesChannelID = Convert.ToInt32(cmbTransferTo.SelectedValue);
                String[] StrCol = new String[] { "SalesmanID", "Salesman" };
                dt = ObjSalesman.GetSalesmanInfo();
                if (dt.Rows.Count > 0)
                {
                    PageBase.DropdownBinding(ref cmbSalesManTo, dt, StrCol);
                    cmbSalesManTo.Enabled = true;
                }

            };
            FillHierarchy();
        }
        else
        {
            cmbSalesManTo.Items.Clear();
            cmbSalesManTo.Items.Insert(0, new ListItem("Select", "0"));
        }
        if (PageBase.SALESMANOPTIONAL == "1")
        {
            using (RetailerData obj = new RetailerData())
            {
                obj.SalesChannelID = Convert.ToInt32(cmbTransferTo.SelectedValue);
                obj.Type = 1;
                DataTable dt2 = obj.GetRetailerInfo();
            }
        }
    }

    void FillHierarchy()
    {
        if (Session["RETAILERHIERLVLID"] != null)
        {
            if (Convert.ToInt32(Session["RETAILERHIERLVLID"]) > 0)
            {
                using (SalesChannelData ObjSalesChannel = new SalesChannelData())
                {
                    ddlOrghierarchy.Items.Clear();
                    ObjSalesChannel.SalesChannelID = Convert.ToInt32(cmbTransferTo.SelectedValue);
                    String[] StrCol1 = new String[] { "OrgnhierarchyID", "LocationName" };
                    PageBase.DropdownBinding(ref ddlOrghierarchy, ObjSalesChannel.GetSalesChannelOrghierarchyRetailer(), StrCol1);
                }
                reqOrgnhierarchy.ValidationGroup = "Add";
                ddlOrghierarchy.Visible = true;
                tdLabel.Visible = true;
                ddlOrghierarchy.Enabled = true;
            }
            else
            {
                reqOrgnhierarchy.ValidationGroup = "";
                ddlOrghierarchy.Visible = false;
                tdLabel.Visible = false;
            }
        }
    }
    void RefreshData()
    {
        try
        {
            cmbSalesManTo.SelectedValue = "0";
            ucMessage1.Visible = false;
            ddlOrghierarchy.SelectedValue = "0";
            cmbTransferTo.SelectedValue = "0";
            Pnlfrom.Visible = false;
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
            updLoad.Update();
        }
    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("BulkRetailerTransferv2.aspx", false);
    }
    void ClearData()
    {
        try
        {
            if (PageBase.SalesChanelID != 0)
            {
                if (cmbTransferFrom.Items.FindByValue(Convert.ToString(PageBase.SalesChanelID)) != null)
                    cmbTransferFrom.SelectedValue = Convert.ToString(PageBase.SalesChanelID);

                else
                    cmbTransferFrom.SelectedValue = "0";
                if (cmbTransferTo.Items.FindByValue(Convert.ToString(PageBase.SalesChanelID)) != null)
                    cmbTransferTo.SelectedValue = Convert.ToString(PageBase.SalesChanelID);

                else
                    cmbTransferTo.SelectedValue = "0";
                cmbTransferFrom.Enabled = false;
                cmbTransferTo.Enabled = false;
            }
            else
            {
                cmbTransferTo.SelectedValue = "0";
                cmbTransferFrom.SelectedValue = "0";
                cmbSalesManfrom.Enabled = false;
                cmbSalesManTo.Enabled = false;
            }
            cmbSalesManfrom.SelectedValue = "0";
            cmbSalesManTo.SelectedValue = "0";
            ucMessage1.Visible = false;
            cmbSalesManfrom.SelectedValue = "0";
            cmbSalesManTo.SelectedValue = "0";
            Pnlfrom.Visible = false;
            tblToDetails.Visible = false;
            txtRetailerCode.Text = "";
            txtRetailerName.Text = "";
            cmbTransferFrom.Enabled = true;
            tblToDetails.Visible = false;
            UpdatePanel1.Update();

        }
        catch (Exception ex)
        {
            ucMessage2.ShowError(ex.ToString());

        }
    }
    protected void cmbSalesManFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdRetailerFrom.EmptyDataText = "No data Found";
        ucMessage1.Visible = false;
        if (!isvalidate())
        {
            return;
        }
        if (PageBase.SALESMANOPTIONAL == "0")
        {
            if (cmbSalesManfrom.SelectedValue == "0")
            {
                Pnlfrom.Visible = false;

                return;
            }
        }
        getretailerfromdata(1);
    }
    bool isvalidate()
    {
        if (cmbSalesManfrom.SelectedValue != "0" && cmbSalesManTo.SelectedValue != "0")
        {
            if (cmbSalesManTo.SelectedValue == cmbSalesManfrom.SelectedValue)
            {
                ucMessage2.ShowInfo("Cant transfer retailers  between same salesman , please select another salesman");

                return false;
            }
        }
        return true;
    }

    void getretailerfromdata(int index)
    {

        using (RetailerData obj = new RetailerData())
        {
            if (cmbSalesManfrom.Items.Count > 0 && cmbSalesManfrom.SelectedIndex > 0)
            {
                obj.SalesmanID = Convert.ToInt32(cmbSalesManfrom.SelectedValue);
            }
            else
            {
                obj.SalesChannelID = Convert.ToInt32(cmbTransferFrom.SelectedValue);
                obj.SalesmanID = 0;
            }
            obj.Type = 1;
            obj.PageIndex = index;
            obj.PageSize = 10;
            obj.RetailerName = txtRetailerName.Text;
            obj.RetailerCode = txtRetailerCode.Text;
            DataTable dt = obj.GetRetailerInfoNewV1();
            if (dt.Rows.Count > 0)
            {
                if (index == -1)
                    ExportDt = dt;
                if (index != -1)
                {
                    grdRetailerFrom.DataSource = dt;
                    grdRetailerFrom.DataBind();

                    ucPagingControl1.CurrentPage = index;
                    ucPagingControl1.Visible = true;
                    ucPagingControl1.PageSize = 10;
                    ucPagingControl1.TotalRecords = obj.TotalRecords;
                    ucPagingControl1.FillPageInfo();
                    lblTotalCount.Text = obj.TotalRecords.ToString();
                    Pnlfrom.Visible = true;
                    tblToDetails.Visible = true;
                    //GridClientSide1.IsBlankDataTable = true;
                    //exportToExel.Visible = true;}
                }
                else
                {
                    grdRetailerFrom.DataSource = null;
                    grdRetailerFrom.DataBind();
                    tblToDetails.Visible = false;
                    ucPagingControl1.TotalRecords = obj.TotalRecords;
                    ucPagingControl1.FillPageInfo();
                    lblTotalCount.Text = "0";
                    ucPagingControl1.Visible = false;
                    //exportToExel.Visible = false;
                }
                UpdatePanel1.Update();
                updLoad.Update();
            }
            else
            {
                grdRetailerFrom.DataSource = null;
                grdRetailerFrom.DataBind();
                tblToDetails.Visible = false;
                ucPagingControl1.TotalRecords = obj.TotalRecords;
                ucPagingControl1.FillPageInfo();
                lblTotalCount.Text = "0";
                ucPagingControl1.Visible = false;
                UpdatePanel1.Update();
                updLoad.Update();
                //exportToExel.Visible = false;
            }
        }
    }


    protected void UCPagingControl1_SetControlRefresh()
    {
        hdnIndex.Value = ucPagingControl1.CurrentPage.ToString();
        getretailerfromdata(ucPagingControl1.CurrentPage);
    }
    protected void btnSearchRetailer_Click(object sender, EventArgs e)
    {
        getretailerfromdata(1);
    }
    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        ucMessage1.Visible = true;
        if (PageBase.SALESMANOPTIONAL == "0")
        {
            if (cmbSalesManfrom.Items.Count > 0)
            {
                if (cmbSalesManfrom.SelectedIndex == 0)
                {
                    ucMessage2.ShowInfo("Please select from salesman.");

                    return;
                }
            }
            else
            {
                ucMessage2.ShowInfo("Please select from salesman.");

                return;
            }
            if (cmbSalesManTo.Items.Count > 0)
            {
                if (cmbSalesManTo.SelectedIndex == 0)
                {
                    ucMessage2.ShowInfo("Please select to salesman.");

                    return;
                }
            }
            else
            {
                ucMessage2.ShowInfo("Please select to salesman.");

                return;
            }

        }
        if (Convert.ToInt32(Session["RETAILERHIERLVLID"]) > 0) /* #CC03 Added */
        {
            if (ddlOrghierarchy.Items.Count > 0)
            {
                if (ddlOrghierarchy.SelectedIndex == 0)
                {
                    ucMessage2.ShowInfo("Please select reporting herarchy.");

                    return;
                }
            }
            else
            {
                ucMessage2.ShowInfo("Please select reporting herarchy.");

                return;
            }
        }

        DataTable dtUploadedFile = GridClientSide1.GetDataTable;

        if (hdnComingFrom.Value == "0")
        {
            if (dtUploadedFile.Rows.Count <= 0)
            {
                ucMessage2.ShowInfo("Please add retailer for transfer.");

                return;
            }
        }

        gettransferdata(dtUploadedFile);


    }

    public void gettransferdata(DataTable retailerInfo)
    {
        int intResult = 0;

        DataSet ds = new DataSet();
        ds.Tables.Add(retailerInfo);

        using (SalesmanData objPrimarySales = new SalesmanData())
        {
            objPrimarySales.SalesmanID = Convert.ToInt32((cmbSalesManTo.SelectedValue == "" ? "0" : cmbSalesManTo.SelectedValue));
            objPrimarySales.SalesChannelID = Convert.ToInt32(cmbTransferTo.SelectedValue);
            objPrimarySales.RetailerOrgnHierarchyID = Convert.ToInt32(ddlOrghierarchy.SelectedValue);
            objPrimarySales.ComingFrom = Convert.ToInt16(hdnComingFrom.Value);
            objPrimarySales.SaleChannelFrom = Convert.ToInt32(cmbTransferFrom.SelectedValue);
            objPrimarySales.RetailerTransfer = ds.GetXml();
            if (cmbSalesManfrom.Items.Count > 0)
                objPrimarySales.SalesManFrom = Convert.ToInt32(cmbSalesManfrom.SelectedValue);
            else
                objPrimarySales.SalesManFrom = 0;
            intResult = objPrimarySales.InsertInfoRetailerMapV2();
            if (objPrimarySales.ErrorDetailXML != null && objPrimarySales.ErrorDetailXML != string.Empty)
            {
                ucMessage1.XmlErrorSource = objPrimarySales.ErrorDetailXML;
                if (hdnXMLOccer.Value == "0")
                {
                    ((Panel)ucMessage1.FindControl("pnlUcMessageBox")).Visible = true;
                    ((Panel)ucMessage1.FindControl("pnlUcMessageBox")).Style.Add("display", "block");
                    UpdatePanel2.Update();
                    hdnXMLOccer.Value = "1";
                }
                return;
            }
            if (objPrimarySales.Error != null && objPrimarySales.Error != "")
            {
                ucMessage2.ShowError(objPrimarySales.Error);

                return;
            }
            if (intResult == 2)
            {
                ucMessage2.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);

                return;
            }

            ucMessage1.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alertScript", "document.getElementById('ctl00_contentHolderMain_ucMessage1_pnlUcMessageBox').style.display = 'none'", true);
            ucMessage2.ShowSuccess("Retailers transferred successfully.");
            hdnXMLOccer.Value = "1";
            btnTransfer.Visible = false;
        }
    }
}