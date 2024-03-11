using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Data;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Resources;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;
using System.Text;

/*
 *17 Mar 2015, Karam Chand Sharma, #CC01, Correct download template path
 *26-Jul-2016, Sumit Maurya, #CC02, New row "StockType" added.
 *28-Jul-2016, Sumit Maurya, #CC03, New check added which prevent to call button click event multiple times. UCMsg display is set to block. 
 */
public partial class Transactions_SalesChannelSB_ManageWarehouseGRN : PageBase
{
    protected string strSiteUrl = PageBase.siteURL;
    protected string strAssets = PageBase.strAssets;
    private bool IsOpeningdateEnable = false;
    protected void Page_Load(object sender, EventArgs e)
    {



        try
        {
            // GetvalidSession();
            Page.Header.DataBind();
            // string strLogInUserName = Convert.ToString(Session["DisplayName"]) + "(" + Convert.ToString(Session["RoleName"]) + ")";
            //lblUserNameDesc.Text = strLogInUserName;
            ucMsg.Visible = false;
            if (!IsPostBack)
            {
                //BindGrid();
                BindWarehouse();


            }
            bindAssets();
            FillDate();
            PartLookupClientSide1.SalesChannelID = PageBase.SalesChanelID == 0 ? "0" : PageBase.SalesChanelID.ToString();
            PartLookupClientSide1.SalesChannelCode = PageBase.SalesChanelID == 0 ? ddlSalesChannel.SelectedValue : PageBase.SalesChanelCode.ToString();
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            ucMsg.ShowError(ex.Message, GlobalErrorDisplay());
        }


    }
    void FillDate()
    {
        //if (PageBase.BackDaysAllowedOpeningStock == 0)
        //{
        //    ucDatePicker.Date = DateTime.Now.Date.ToString();
        //    ucDatePicker.TextBoxDate.Enabled = false;
        //    ucDatePicker.imgCal.Enabled = false;
        //}
        //else
        //{

        ucDatePicker.TextBoxDate.Enabled = true;
        ucDatePicker.imgCal.Enabled = true;
        ucDatePicker.MinRangeValue = DateTime.MinValue;
        ucDatePicker.MaxRangeValue = DateTime.Now.Date;
        ucDatePicker.RangeErrorMessage = "Only Back days allowed";
        //}
    }

    protected void rdModelList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdModelList.SelectedValue == "0")
            /*#CC01 COMMENTED */
            Response.Redirect("~/Transactions/SalesChannelSB/Upload/ManageSalesChannelGRN-SB.aspx");
        /* Response.Redirect("~/Transactions/SalesChannelSB/Upload/ManageSalesChannelGRN-SB-BCP.aspx");#CC01 ADDED*/
    }

    void bindAssets()
    {
        // ImgSideLogo.ImageUrl = "~/" + strAssets + "/CSS/Images/zedsaleslogo.gif";
        //  hyplogo.ImageUrl = "~/" + strAssets + "/CSS/Images/innerlogo.gif";
        //  hypfooterlogo.ImageUrl = "~/" + strAssets + "/CSS/Images/footerimg.gif";
        //  hypfooterlogo.NavigateUrl = PageBase.redirectURL;
        // hypfooterlogo.Target = "_blank";
    }


    DataTable getOpeningStockTable(DataTable dtSource)
    {

        DataTable Detail = new DataTable();
        using (CommonData ObjCommom = new CommonData())
        {
            Detail = ObjCommom.GettvpTableOpeningStockSB();
        }


        foreach (DataRow dr in dtSource.Rows)
        {
            DataRow drow = Detail.NewRow();
            drow["SKUCode"] = dr["SKUCode"].ToString().Trim();
            drow["Serial#1"] = dr["serialno"].ToString().Trim();
            drow["BatchNo"] = dr["BatchNo"].ToString().Trim();
            drow["Quantity"] = Convert.ToInt32(dr["Quantity"]);
            drow["StockBinType"] = dr["StockType"].ToString().Trim(); /* #CC02 Added */

            Detail.Rows.Add(drow);
        }
        Detail.AcceptChanges();

        return Detail;

    }

    protected void Page_PreRender(object s, EventArgs args)
    {
        if (!IsPostBack)/* #CC03 Added */
            btnSave_Click(btnSave, null);
    }

    void BindWarehouse()
    {
        DataSet DsScheme;
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.SalesChannelTypeID = 14;
            ObjSalesChannel.ActiveStatus = 1;
            string[] str = { "SalesChannelCode", "SalesChannelName" };
            PageBase.DropdownBinding(ref ddlSalesChannel, ObjSalesChannel.GetSalesChannelList(), str);
        }



        //void BindSalesChannel()
        // {
        //     try
        //     {
        //         ddlSalesChannelName.Items.Clear();
        //         using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        //         {
        //             ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(cmbsaleschanneltype.SelectedValue);
        //             ObjSalesChannel.ActiveStatus = 255;
        //             string[] str={"SalesChannelid","SalesChannelName"};
        //             PageBase.DropdownBinding(ref ddlSalesChannelName, ObjSalesChannel.GetSalesChannelList(), str);
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         ucMessage1.ShowError(ex.ToString());
        //     }
        // }          




        //String[] colArray = { "SalesChannelCode", "SalesChannelName" };
        //PageBase.DropdownBinding(ref ddlSalesChannel, DsScheme.Tables[0], colArray);
        ddlSalesChannel.Enabled = true;

        if (ddlSalesChannel.Items.FindByValue(PageBase.SalesChanelCode) != null)
        {
            ddlSalesChannel.Items.FindByValue(PageBase.SalesChanelCode).Selected = true;
            ddlSalesChannel.Enabled = false;
        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int intResult = 0;
            //if (IsPageRefereshed == true)
            //{
            //    return;
            //}
            // if (!pageValidate())
            // {
            //   return;
            // }


            DataTable Dt = new DataTable();
            if (PartLookupClientSide1.SubmittingTable.Rows.Count > 0)
            {
                Dt = PartLookupClientSide1.SubmittingTable;
            }
            else
            {
                return;
            }


            DataTable dtSource = getOpeningStockTable(Dt);

            DataTable Tvp = new DataTable();
            using (CommonData ObjCommom = new CommonData())
            {
                Tvp = ObjCommom.GettvpTableGRNSB();
            }

            foreach (DataRow dr in dtSource.Rows)
            {
                DataRow drow = Tvp.NewRow();

                drow["WareHouseCode"] = ddlSalesChannel.SelectedValue;
                drow["GRNNumber"] = txtGRNNo.Text.Trim();
                drow["GRNDate"] = ucDatePicker.Date;
                drow["SKUCode"] = dr["SKUCode"].ToString().Trim();
                drow["Quantity"] = dr["Quantity"];
                drow["Serial#1"] = dr["Serial#1"].ToString().Trim();
                //drow["Serial#2"] = dr["Serial#2"];
                //drow["Serial#3"] = dr["Serial#3"].ToString();
                //drow["Serial#4"] = dr["Serial#4"].ToString();
                drow["BatchNo"] = dr["BatchNo"].ToString().Trim();

                drow["StockBinType"] = dr["StockBinType"].ToString().Trim(); /* #CC02 Added */

                Tvp.Rows.Add(drow);
            }
            Tvp.AcceptChanges();
            using (SalesData objGRN = new SalesData())
            {
                ucMsg.Attributes.CssStyle.Add("display", "block"); /* #CC03 Added */
                objGRN.EntryType = EnumData.eEntryType.eUpload;
                objGRN.UserID = PageBase.UserId;
                intResult = objGRN.InsertInfoGRNUploadSB(Tvp);

                if (objGRN.ErrorDetailXML != null && objGRN.ErrorDetailXML != string.Empty)
                {
                    ucMsg.XmlErrorSource = objGRN.ErrorDetailXML;
                    return;
                }
                if (objGRN.Error != null && objGRN.Error != "")
                {
                    ucMsg.ShowError(objGRN.Error);
                    return;
                }
                if (intResult == 2)
                {
                    ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    return;
                }
                PartLookupClientSide1.IsBlankDataTable = true;
                PartLookupClientSide1.SubmittingTable = new DataTable();
                ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                Clear();

            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);

        }
    }



    bool pageValidate()
    {
        if (ServerValidation.IsDate(ucDatePicker.Date, true) != 0)
        {
            ucMsg.ShowInfo(Resources.Messages.MandatoryField);
            return false;
        }

        if (Convert.ToDateTime(ucDatePicker.Date) > DateTime.Now.Date)
        {
            ucMsg.ShowInfo("Date should be less than or equal to current date.");
            return false;
        }

        return true;
    }



    public StringBuilder GenerateXML(GridView gv)
    {

        StringBuilder StockDetailXML = new StringBuilder();
        StockDetailXML.AppendLine("<table>");
        foreach (GridViewRow gvRow in gv.Rows)
        {
            Label objSKUID = ((Label)gvRow.FindControl("lblSKUID"));
            TextBox objOpeningStock = ((TextBox)gvRow.FindControl("txtOpeningStock"));

            StockDetailXML.AppendLine("<rowse>");
            StockDetailXML.AppendFormat("<SKUID>{0}</SKUID>{1}", objSKUID.Text.Trim(), Environment.NewLine);
            StockDetailXML.AppendFormat("<QUANTITY>{0}</QUANTITY>{1}", objOpeningStock.Text.Trim(), Environment.NewLine);
            StockDetailXML.AppendLine("</rowse>");
        }
        StockDetailXML.AppendLine("</table>");
        return StockDetailXML;
    }

    protected void gvStockEntry_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {

        ucDatePicker.Date = "";
        FillDate();
        PartLookupClientSide1.SubmittingTable = new DataTable();
        PartLookupClientSide1.IsBlankDataTable = true;
        Clear();
    }
    void Clear()
    {
        txtGRNNo.Text = "";
        //ddlSalesChannel.SelectedIndex = 0;
    }


}
