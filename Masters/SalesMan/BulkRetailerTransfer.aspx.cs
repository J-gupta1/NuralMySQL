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

/*
 * 11 Mar 2015, Karam Chand Sharma, #CC01, Manage salesman from and to Optional according to ApplicationConfigurationMaster
 */

public partial class Masters_SalesMan_BulkRetailerTransfer : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMessage1.ShowControl = false;
        if (!IsPostBack)
        {
            ucMessage1.Visible = false;
            // fillcmbfrom();
            // fillcmbto();

            cmbSalesManTo.Items.Insert(0, new ListItem("Select", "0"));
            cmbSalesManfrom.Items.Insert(0, new ListItem("Select", "0"));
            ddlOrghierarchy.Items.Insert(0, new ListItem("Select", "0"));
            ddlOrghierarchy.Enabled = false;
            cmbSalesManfrom.Enabled = false;
            cmbSalesManTo.Enabled = false;
            fillsalesChannel();
            if (Session["RETAILERHIERLVLID"] != null)
            {
                if (Convert.ToInt32(Session["RETAILERHIERLVLID"]) == 0)
                {
                    ddlOrghierarchy.Enabled = false;
                    ddlOrghierarchy.Visible = false;
                    tdLabel.Visible = false;
                }
                else
                {
                    ddlOrghierarchy.Enabled = true;
                    ddlOrghierarchy.Visible = true;
                    tdLabel.Visible = true;
                }
            }

            /*This interface will never be used by any Saleschannel(othere wise change the code accordingly)*/
            if (PageBase.SalesChanelID != 0)
            {
                cmbTransferFrom.SelectedValue = Convert.ToString(PageBase.SalesChanelID);
                cmbTransferTo.SelectedValue = Convert.ToString(PageBase.SalesChanelID);
                cmbTransferFrom_SelectedIndexChanged(sender, null);
                cmbTransferTo_SelectedIndexChanged(sender, null);
                cmbTransferFrom.Enabled = false;
                cmbTransferTo.Enabled = false;

            }
            /*#CC01 START ADDED*/
            if (PageBase.SALESMANOPTIONAL == "1")
            {
                saleFrom.Visible = false;
                saleTo.Visible = false;
                RequiredFieldValidator1.Enabled = false;
                RequCombo.Enabled = false;
            }
            /*#CC01 START END*/
        }
    }


    void fillcmbfrom()
    {
        using (SalesmanData obj = new SalesmanData())
        {
            obj.SalesChannelID = PageBase.SalesChanelID;
            DataTable dt = obj.GetSalesmanInfo();
            dt.DefaultView.RowFilter = "Status = True ";
            dt = dt.DefaultView.ToTable();
            String[] colArray = { "SalesmanID", "SalesmanName" };
            PageBase.DropdownBinding(ref cmbSalesManfrom, dt, colArray);

        }

    }


    void fillcmbto()
    {
        using (SalesmanData obj = new SalesmanData())
        {
            obj.SalesChannelID = PageBase.SalesChanelID;
            DataTable dt = obj.GetSalesmanInfo();
            dt.DefaultView.RowFilter = "Status = True ";
            dt = dt.DefaultView.ToTable();
            String[] colArray = { "SalesmanID", "SalesmanName" };
            PageBase.DropdownBinding(ref cmbSalesManTo, dt, colArray);
            cmbSalesManTo.SelectedValue = "0";
        }

    }

    void getretailerfromdata()
    {
        if (cmbSalesManfrom.SelectedValue != "0") /*#CC01 ADDED*/
        {
            using (RetailerData obj = new RetailerData())
            {
                obj.SalesmanID = Convert.ToInt32(cmbSalesManfrom.SelectedValue);
                obj.Type = 1;

                DataTable dt = obj.GetRetailerInfo();
                ViewState["Retailer"] = dt;
                grdRetailerFrom.DataSource = dt;
                grdRetailerFrom.DataBind();
                updFrom.Update();
                Pnlfrom.Visible = true;

            }
        } /*#CC01 START ADDED*/
        else
        {
            cmbTransferFrom_SelectedIndexChanged(null, null);
        } /*#CC01 START END*/
    }


    void getretailerto()
    {
        if (cmbSalesManTo.SelectedValue != "0") /*#CC01 ADDED*/
        {
            using (RetailerData obj = new RetailerData())
            {
                obj.SalesmanID = Convert.ToInt32(cmbSalesManTo.SelectedValue);
                obj.Type = 1;
                DataTable dt2 = obj.GetRetailerInfo();
                grdRetailerTo.DataSource = dt2;
                grdRetailerTo.DataBind();
                updto.Update();
                pnlto.Visible = true;
            }
        } /*#CC01 START ADDED*/
        else
        {
            cmbTransferTo_SelectedIndexChanged(null, null);
        } /*#CC01 START END*/
    }

    protected void cmbSalesManFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbTransferFrom.SelectedIndex == 0)
        {
            pnlto.Visible = false;
            updto.Update();
            return;
        }
        grdRetailerFrom.EmptyDataText = "No data Found";
        ucMessage1.Visible = false;
        if (!isvalidate())
        {
            return;
        } /*#CC01 START ADDED*/
        if (PageBase.SALESMANOPTIONAL == "0")
        {
            if (cmbSalesManfrom.SelectedValue == "0")
            {
                Pnlfrom.Visible = false;
                updFrom.Update();
                return;
            }
        } /*#CC01 START END*/
        getretailerfromdata();
    }
    protected void cmbSalesManTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbTransferTo.SelectedIndex == 0)
        {
            pnlto.Visible = false;
            updto.Update();
            return;
        }
        ucMessage1.Visible = false;

        if (!isvalidate())
        {
            return;
        } /*#CC01 START ADDED*/
        if (PageBase.SALESMANOPTIONAL == "0")
        {
            if (cmbSalesManTo.SelectedValue == "0")
            {
                pnlto.Visible = false;
                updto.Update();
                return;

            }
        } /*#CC01 START END*/
        getretailerto();

    }

    protected void btnTransferselected_Click(object sender, EventArgs e)
    {

        try
        {
            DataTable retailerInfo = new DataTable();
            DataColumn dc1 = new DataColumn("RetailerID", typeof(Int32));

            retailerInfo.Columns.Add(dc1);


            foreach (GridViewRow grv in grdRetailerFrom.Rows)
            {

                CheckBox chk = (CheckBox)grv.FindControl("chkRetailerTransfer");
                if (chk.Checked == true)
                {
                    Label lbId = (Label)grv.FindControl("lblID");


                    DataRow dr = retailerInfo.NewRow();
                    dr["RetailerID"] = Convert.ToInt32(lbId.Text);

                    retailerInfo.Rows.Add(dr);

                }


            }
            gettransferdata(retailerInfo);


        }

        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
    protected void btnTransferAll_Click(object sender, EventArgs e)
    {

        DataTable retailerinfo = (DataTable)ViewState["Retailer"];
        gettransferdata(retailerinfo);



    }


    public void gettransferdata(DataTable retailerInfo)
    {
        int intResult = 0;
        DataTable Tvp = new DataTable();
        using (CommonData ObjCommom = new CommonData())
        {
            Tvp = ObjCommom.GettvpRetailerMap();
        }
        foreach (DataRow dr in retailerInfo.Rows)
        {
            DataRow drow = Tvp.NewRow();
            drow[0] = dr["RetailerID"].ToString();
            Tvp.Rows.Add(drow);

        }
        Tvp.AcceptChanges();

        using (SalesmanData objPrimarySales = new SalesmanData())
        {
            /*objPrimarySales.SalesmanID = Convert.ToInt32(cmbSalesManTo.SelectedValue);*/
            objPrimarySales.SalesmanID = Convert.ToInt32((cmbSalesManTo.SelectedValue == "" ? "0" : cmbSalesManTo.SelectedValue));
            objPrimarySales.SalesChannelID = Convert.ToInt32(cmbTransferTo.SelectedValue);
            objPrimarySales.RetailerOrgnHierarchyID = Convert.ToInt32(ddlOrghierarchy.SelectedValue);
            intResult = objPrimarySales.InsertInfoRetailerMap(Tvp);

            if (objPrimarySales.ErrorDetailXML != null && objPrimarySales.ErrorDetailXML != string.Empty)
            {
                ucMessage1.XmlErrorSource = objPrimarySales.ErrorDetailXML;
                return;
            }
            if (objPrimarySales.Error != null && objPrimarySales.Error != "")
            {
                ucMessage1.ShowError(objPrimarySales.Error);
                return;
            }
            if (intResult == 2)
            {
                ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                return;
            }

            ucMessage1.ShowSuccess("Retailers transferred succesfully");
            /*#CC01 START ADDED*/
            if (PageBase.SALESMANOPTIONAL == "1")
            {
                cmbTransferTo_SelectedIndexChanged(null, null);
            }
            else /*#CC01 START END*/
                getretailerto();
            getretailerfromdata();
            pnlto.Visible = true;
            grdRetailerFrom.EmptyDataText = "All Retailers have been transferred , so no current data ";
        }
    }


    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        ClearData();
    }

    bool isvalidate()
    {
        if (cmbSalesManfrom.SelectedValue != "0" && cmbSalesManTo.SelectedValue != "0")
        {
            if (cmbSalesManTo.SelectedValue == cmbSalesManfrom.SelectedValue)
            {
                ucMessage1.ShowInfo("Cant transfer retailers  between same salesman , please select another salesman");
                return false;
            }
        }
        return true;
    }
    protected void grdfromPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdRetailerFrom.PageIndex = e.NewPageIndex;
        getretailerfromdata();
    }


    protected void grdToPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdRetailerTo.PageIndex = e.NewPageIndex;
        getretailerto();
    }
    protected void cmbTransferFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        RefreshData();

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
        //if (cmbTransferFrom.SelectedValue == "0")
        //{
        //    cmbTransferTo.Items.Clear();
        //    cmbTransferTo.Items.Insert(0, new ListItem("Select", "0"));
        //}
        //else
        //{
        //    using (SalesChannelData obj = new SalesChannelData())
        //    {
        //        DataTable dt = obj.GetParentForRetailer();
        //        dt.DefaultView.RowFilter = "SalesChannelID <> " + cmbTransferFrom.SelectedValue;
        //        dt = dt.DefaultView.ToTable();
        //        String[] colArray = { "SalesChannelID", "SalesChannelName" };
        //        PageBase.DropdownBinding(ref cmbTransferTo, dt, colArray);

        //    }
        //    //        fillgrid();
        //    //      pnlGrid.Visible = true;


        //}
        /*#CC01 START ADDED*/
        if (PageBase.SALESMANOPTIONAL == "1")
        {
            using (RetailerData obj = new RetailerData())
            {
                obj.SalesChannelID = Convert.ToInt32(cmbTransferFrom.SelectedValue);
                obj.Type = 1;

                DataTable dt = obj.GetRetailerInfo();
                ViewState["Retailer"] = dt;
                grdRetailerFrom.DataSource = dt;
                grdRetailerFrom.DataBind();
                updFrom.Update();
                Pnlfrom.Visible = true;

            }
        }
        /*#CC01 START END*/
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
    protected void cmbTransferTo_SelectedIndexChanged(object sender, EventArgs e)
    {
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
                    grdRetailerTo.DataSource = null;
                    grdRetailerTo.DataBind();
                    pnlto.Visible = false;
                }

            };
            FillHierarchy();
        }
        else
        {
            cmbSalesManTo.Items.Clear();
            cmbSalesManTo.Items.Insert(0, new ListItem("Select", "0"));
        }
        /*#CC01 START ADDED*/
        if (PageBase.SALESMANOPTIONAL == "1")
        {
            using (RetailerData obj = new RetailerData())
            {
                obj.SalesChannelID = Convert.ToInt32(cmbTransferTo.SelectedValue);
                obj.Type = 1;
                DataTable dt2 = obj.GetRetailerInfo();
                grdRetailerTo.DataSource = dt2;
                grdRetailerTo.DataBind();
                updto.Update();
                pnlto.Visible = true;

            }
        } /*#CC01 START END*/
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
    void ClearData()
    {
        try
        {
            if (PageBase.SalesChanelID != 0)
            {
                if (cmbTransferFrom.Items.FindByValue(Convert.ToString(PageBase.SalesChanelID)) != null)
                    cmbTransferFrom.SelectedValue = Convert.ToString(PageBase.SalesChanelID);
                //cmbTransferFrom_SelectedIndexChanged(sender, null);
                else
                    cmbTransferFrom.SelectedValue = "0";
                if (cmbTransferTo.Items.FindByValue(Convert.ToString(PageBase.SalesChanelID)) != null)
                    cmbTransferTo.SelectedValue = Convert.ToString(PageBase.SalesChanelID);
                // cmbTransferTo_SelectedIndexChanged(sender, null);
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
            //cmbSalesManTo.Enabled = false;
            //cmbSalesManfrom.Enabled = false;
            cmbSalesManfrom.SelectedValue = "0";
            cmbSalesManTo.SelectedValue = "0";
            ucMessage1.Visible = false;
            cmbSalesManfrom.SelectedValue = "0";
            cmbSalesManTo.SelectedValue = "0";
            Pnlfrom.Visible = false;
            pnlto.Visible = false;
            updFrom.Update();
            updto.Update();
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
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
            pnlto.Visible = false;
            updFrom.Update();
            updto.Update();

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }

}
