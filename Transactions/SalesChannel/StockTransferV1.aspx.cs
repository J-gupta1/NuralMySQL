/*
 * Change Log:
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 24-May-2016, Sumit Maurya, #CC01, Parameter value supplied according to config value to bind dropdown options. Code commented because it was not implemented according to HO. Update Panel Added to update controls value in ucPartLookupClientSide.ascx
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
using System.Web.UI.HtmlControls;

public partial class Transactions_SalesChannel_StockTransferV1 : PageBase
{


    protected void Page_Load(object sender, EventArgs e)
    {


        try
        {
            /* #CC01 Comment Start
             * if (PageBase.SalesChanelTypeID != 6)
             {
               
                 //Added By Mamta Singh for checking back date before opening  date
                 if (Convert.ToDateTime(DateTime.Now.Date.AddDays(PageBase.NumberofBackDaysAllowed)) < Convert.ToDateTime(SalesChannelOpeningStockDate))
                 {

                     ucDatePicker.MinRangeValue = Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-PageBase.BackDaysAllowedBeforeOpening);
                     lblValidationDays.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", (Convert.ToDateTime(DateTime.Now.Date.ToShortDateString()) - Convert.ToDateTime(Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening).ToShortDateString())).TotalDays.ToString());
                 }
                
                
             }
             * #CC01 Comment End
             */
            /* #CC01 Add Start */
            if (IsPostBack)
            {
                if (cmbFrom.SelectedValue != "0")
                {

                    HtmlInputHidden hdnsalesChannelID = (HtmlInputHidden)PartLookupClientSide1.FindControl("salesChannelID");
                    HtmlInputHidden hdnsalesChannelCode = (HtmlInputHidden)PartLookupClientSide1.FindControl("salesChannelCode");
                    hdnsalesChannelID.Value = cmbFrom.SelectedValue;

                    string[] split = cmbFrom.SelectedItem.ToString().Split('(');
                    hdnsalesChannelCode.Value = split[1].Replace(")", "").Trim();

                }
            }
            /* #CC01 Add End */

            ucDatePicker.MaxRangeValue = DateTime.Now.Date;
            ucDatePicker.RangeErrorMessage = "Invalid Date Range";
            if (!IsPostBack)
            {
                FillsalesChannelType();
                ucMessage1.Visible = false;
                //  pnlGrid.Visible = false;
                fillSalesChannelfrom();
                fillTo();
                cmbTo.Items.Insert(0, new ListItem("Select", "0"));

                if (PageBase.SalesChanelTypeID == 6)
                {
                    lblStockTransfer1.Text = "Inter-Group Stock Transfer";
                    lblStockTransfer2.Text = "Inter-Group Stock Transfer";

                }

            }



        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.Message.ToString(), PageBase.GlobalErrorDisplay());
        }
    }

    void fillTo()
    {
        if (PageBase.SalesChanelID != 0)
        {
            if (PageBase.SalesChanelID != 6)
            {
                using (SalesChannelData ObjSalesChannel = new SalesChannelData())
                {

                    ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(cmbChannelType.SelectedValue.ToString());
                    DataTable dt = ObjSalesChannel.GetSalesChannelInfoForStockists();
                    dt.DefaultView.RowFilter = "Status = True and SalesChannelID <> " + cmbFrom.SelectedValue;
                    dt = dt.DefaultView.ToTable();
                    String[] colArray = { "SalesChannelID", "DisplayName" };
                    PageBase.DropdownBinding(ref cmbTo, dt, colArray);
                }

            }
        }
    }

    void FillsalesChannelType()
    {

        using (SalesChannelData obj = new SalesChannelData())
        {
            cmbFrom.Items.Insert(0, new ListItem("Select", "0"));
            obj.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
            if (obj.SalesChannelTypeID == 0)
            {
                obj.isHOZSM = 1;
            }

            DataTable dt = obj.GetSalesChannelTypeFromUser();
            String[] colArray = { "SalesChannelTypeID", "SalesChannelTypeName" };
            PageBase.DropdownBinding(ref cmbChannelType, dt, colArray);
            cmbChannelType.SelectedValue = Convert.ToString(PageBase.SalesChanelTypeID);
            cmbChannelType.Enabled = false;
            cmbChannelType.AutoPostBack = false;
            if (obj.SalesChannelTypeID == 0)
            {
                cmbChannelType.Enabled = true;
                cmbChannelType.AutoPostBack = true;
            }
        }
    }

    void fillSalesChannelfrom()
    {
        if (PageBase.SalesChanelID != 0)
        {


            using (SalesChannelData ObjSalesChannel = new SalesChannelData())
            {
                ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
                ObjSalesChannel.SalesChannelID = PageBase.SalesChanelID;
                DataTable dt = ObjSalesChannel.GetSalesChannelInfoForStockists();
                String[] colArray = { "SalesChannelID", "DisplayName" };
                PageBase.DropdownBinding(ref cmbFrom, dt, colArray);
                if (PageBase.SalesChanelTypeID != 6)
                {
                    cmbFrom.SelectedValue = Convert.ToString(PageBase.SalesChanelID);
                    cmbFrom.Enabled = false;
                    cmbFrom.AutoPostBack = false;
                }
                else
                {
                    cmbFrom.SelectedValue = "0";
                    cmbFrom.Enabled = true;
                }
                // dt.DefaultView.RowFilter = "Status = True and SalesChannelID <> " + cmbFrom.SelectedValue;
                //dt = dt.DefaultView.ToTable();
                //String[] colArrayTo = { "SalesChannelID", "DisplayName" };
                //PageBase.DropdownBinding(ref cmbTo, dt, colArrayTo);
                //cmbTo.SelectedValue = "0";

            }

        }
    }



    void blankall()
    {

        PartLookupClientSide1.IsBlankDataTable = true;

        txtDocketNo.Text = "";
        txtStnNo.Text = "";
        cmbTo.ClearSelection();
        ucDatePicker.Date = "";
        ucMessage1.Visible = false;
        PnlHide.Visible = false;
        //  pnlGrid.Visible = false;
        //BtnSubmit.Visible = false;

        //  updGrid.Update();

    }


    bool validateselection()
    {
        if (cmbFrom.SelectedItem.Text == cmbTo.SelectedItem.Text)
        {
            ucMessage1.ShowInfo("Cannot set transaction between same user , please select another user ");
            return false;
        }

        using (SalesChannelData ObjSales = new SalesChannelData())
        {
            string dat;


            ObjSales.SalesChannelID = Convert.ToInt16(cmbFrom.SelectedValue.ToString());
            DataTable Dt = ObjSales.GetSalesChannelOpeningStockInfo();
            dat = Dt.Rows[0]["OpeningStockDate"].ToString();
            if (Convert.ToDateTime(ucDatePicker.Date) < Convert.ToDateTime(dat))
            {
                ucMessage1.ShowInfo("The Stock transfer date cant be less then the opening stock date ");
                return false;
            }
            //if (cmbChannelType.SelectedValue != "5")
            //{
            //    ObjSales.SalesChannelTypeID = Convert.ToInt16(cmbChannelType.SelectedValue);
            //    Int16 DaysAllowed = ObjSales.GetSalesChannelBackDays();
            //    if (Convert.ToDateTime(ucDatePicker.Date) < DateTime.Now.Date.AddDays(-DaysAllowed))
            //    {
            //        ucMessage1.ShowInfo("Only " + DaysAllowed.ToString() + " days Back Date Sales Allowed.");
            //        return false;
            //    }

            //}
            return true;


        }




    }

    //protected void BtnSubmit_Click(object sender, EventArgs e)
    //{
    //    PnlHide.Visible = true;
    //    try
    //    {
    //        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
    //        {
    //            ObjSalesChannel.SalesChannelID = Convert.ToInt32(cmbFrom.SelectedValue.ToString());
    //            if (PageBase.SalesChanelID != 0)
    //            {
    //                ObjSalesChannel.Brand = PageBase.Brand;
    //            }
    //            else
    //            {
    //                ObjSalesChannel.Brand = 0;
    //            }
    //            ObjSalesChannel.ToSalesChannelID = Convert.ToInt32(cmbTo.SelectedValue.ToString());
    //            DataTable dt = ObjSalesChannel.GetSalesChannelStockInfo();

    //           //ucDatePicker.Date = PageBase.ToDate;
    //           //ucDatePicker.TextBoxDate.Enabled = false;
    //           //ucDatePicker.imgCal.Enabled = false;
    //            pnlGrid.Visible = true;
    //        //   PnlHide.Visible = true;
    //           // updGrid.Update();l
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //        ucMessage1.ShowError(ex.Message.ToString(), PageBase.GlobalErrorDisplay());
    //    }
    //}



    protected void btnSave_Click(object sender, EventArgs e)
    {


        int Result = 0;
        // updGrid.Update();
        try
        {
            DataTable Dt = new DataTable();
            if (PartLookupClientSide1.SubmittingTable.Rows.Count > 0)
            {
                Dt = PartLookupClientSide1.SubmittingTable;
            }
            else
            {
                return;
            }
            //if (IsPageRefereshed == true)
            //{
            //    return;
            //}
            //if (!validateselection())
            //{
            //    return;
            //}



            DataTable DtDetail = new DataTable();
            using (CommonData ObjCommom = new CommonData())
            {
                //  DtDetail = ObjCommom.GetTVPTableStockTransfer();
                DtDetail = ObjCommom.GetTVPTableStockTransferV1();
            }


            //DataTable Dt = ucSalesEntryGrid1.ReturnGridSource();


            if (Dt.Rows.Count == 0)
            {
                ucMessage1.ShowInfo("Please enter atleast single sku quantity , for stock to transfer");
                return;
            }


            foreach (DataRow dr in Dt.Rows)
            {
                DataRow drow = DtDetail.NewRow();
                drow[0] = Convert.ToInt32(cmbFrom.SelectedValue.ToString());
                drow[1] = Convert.ToInt32(cmbTo.SelectedValue.ToString());
                drow[2] = txtStnNo.Text.ToString();
                drow[3] = txtDocketNo.Text.ToString();
                drow[4] = ucDatePicker.Date;
                drow[5] = dr["Quantity"].ToString();
                drow[6] = dr["SKUCode"].ToString();
                drow[7] = dr["SerialNo"].ToString();
                drow[8] = dr["BatchNo"].ToString();

                DtDetail.Rows.Add(drow);
            }

            DtDetail.AcceptChanges();

            if (DtDetail.Rows.Count == 0)
            {
                return;
            }


            using (SalesData ObjSales = new SalesData())
            {

                ObjSales.Error = "";
                //ObjSales.EntryType = EnumData.eEntryType.eInterface;
                ObjSales.UserID = PageBase.UserId;
                Result = ObjSales.InsertStockTransferInfo_V1(DtDetail);

                if (ObjSales.ErrorDetailXML != null && ObjSales.ErrorDetailXML != string.Empty)
                {
                    ucMessage1.XmlErrorSource = ObjSales.ErrorDetailXML;
                    return;
                }
                else if (ObjSales.Error != null && ObjSales.Error != "")
                {
                    ucMessage1.ShowError(ObjSales.Error);
                    return;
                }

                blankall();
                if (PageBase.SalesChanelID == 0)
                {
                    cmbChannelType.SelectedValue = "0";
                    cmbFrom.Items.Clear();
                    cmbFrom.Items.Insert(0, new ListItem("Select", "0"));
                    cmbFrom.SelectedValue = "0";
                }

                if (PageBase.SalesChanelTypeID == 6)
                {
                    cmbFrom.ClearSelection();
                    cmbFrom.SelectedValue = "0";
                }
                ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                PartLookupClientSide1.SubmittingTable = new DataTable();
                PartLookupClientSide1.IsBlankDataTable = true;

            }
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.Message.ToString(), PageBase.GlobalErrorDisplay());
        }
    }




    protected void btnReset_Click(object sender, EventArgs e)
    {
        blankall();
        PartLookupClientSide1.SubmittingTable = new DataTable();
        PartLookupClientSide1.IsBlankDataTable = true;
        ucMessage1.Visible = false;

        if (PageBase.SalesChanelID == 0)
        {
            cmbChannelType.SelectedValue = "0";
            cmbFrom.Items.Clear();
            cmbFrom.Items.Insert(0, new ListItem("Select", "0"));
            cmbFrom.SelectedValue = "0";
            cmbTo.Items.Clear();

        }
        if (PageBase.SalesChanelTypeID == 6)
        {
            cmbFrom.ClearSelection();
            cmbFrom.SelectedValue = "0";
        }

    }

    protected void cmbFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        blankall();
        PnlHide.Visible = true;
        //   pnlGrid.Visible = false;
        //BtnSubmit.Visible=true;
        // updGrid.Update();
        if (cmbFrom.SelectedValue == "0")
        {
            cmbTo.Items.Clear();
            cmbTo.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            using (SalesChannelData ObjSalesChannel = new SalesChannelData())
            {
                if (PageBase.SalesChanelTypeID == 6)
                {
                    ObjSalesChannel.SalesChannelID = PageBase.SalesChanelID;
                }

                /* #CC01 Add Start */
                if (PageBase.STPARENTCHECK == 1)
                {
                    ObjSalesChannel.SalesChannelID = Convert.ToInt32(cmbFrom.SelectedValue);
                }
                /* #CC01 Add End */
                ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(cmbChannelType.SelectedValue.ToString());
                DataTable dt = ObjSalesChannel.GetSalesChannelInfoForStockists();
                if (dt.Rows.Count == 0)
                {
                    cmbTo.Items.Insert(0, new ListItem("Select", "0"));
                }
                dt.DefaultView.RowFilter = "Status = True and SalesChannelID <> " + cmbFrom.SelectedValue;
                dt = dt.DefaultView.ToTable();
                String[] colArray = { "SalesChannelID", "DisplayName" };
                PageBase.DropdownBinding(ref cmbTo, dt, colArray);
                cmbTo.SelectedValue = "0";

            }
        }
    }
    protected void cmbChannelType_SelectedIndexChanged(object sender, EventArgs e)
    {

        blankall();
        PnlHide.Visible = true;

        //   pnlGrid.Visible = false;

        //updGrid.Update();

        if (cmbChannelType.SelectedValue == "0")
        {
            cmbFrom.Items.Clear();
            cmbFrom.Items.Insert(0, new ListItem("Select", "0"));
            cmbTo.Items.Clear();
            cmbTo.Items.Insert(0, new ListItem("Select", "0"));
            blankall();

        }
        else
        {
            using (SalesChannelData ObjSalesChannel = new SalesChannelData())
            {
                ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(cmbChannelType.SelectedValue.ToString());
                DataTable dt = ObjSalesChannel.GetSalesChannelInfoForStockists();
                dt.DefaultView.RowFilter = "Status = True";
                dt = dt.DefaultView.ToTable();
                String[] colArray = { "SalesChannelID", "DisplayName" };

                cmbTo.Items.Clear();

                PageBase.DropdownBinding(ref cmbFrom, dt, colArray);
                cmbFrom.SelectedValue = "0";
                cmbTo.Items.Insert(0, new ListItem("Select", "0"));

                if (PageBase.SalesChanelID != 0)
                {
                    cmbFrom.Enabled = false;
                }


            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs args)
    {




        btnSave_Click(btnSave, null);



    }

}
