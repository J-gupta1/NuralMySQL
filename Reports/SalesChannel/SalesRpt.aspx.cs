#region  NameSpace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;
using System.Data;
using DevExpress.Web.ASPxPivotGrid;
using System.Configuration;
using DevExpress.XtraPivotGrid;
using DevExpress.Utils;
#endregion
/*
 * 24 July 2016, Karam Chand Sharma, #CC01, Show stock bin type in pivot
 * 15 Nov 2017,Vijay Kumar Prajapati,#CC02,Show distributor,warehouse,retailor code.
 *  05 June 2018,Rajnish Kumar,#CC03,SalesChannelId and CityId Filter.
 * 18 June 2018,Rajnish Kumar,#CC04,Region Level Name According to RSMLevel Configuration Value.
 *  01-Oct-2018,Vijay Kumar Prajapati,#CC05,Get model according to productcateryId selection.
 */
public partial class Reports_SalesChannel_SalesRpt : PageBase
{
    #region PageLoad Event
    protected void Page_Load(object sender, EventArgs e)
    {
        EnableViewState = false;
        fncHide();
        if (hfSearch.Value == "1")
            BindGrid();
        if (!IsPostBack)
        {
            //ShowScrollBar(true);
            ucDateFrom.Date = PageBase.Fromdate;
            ucDateTo.Date = PageBase.ToDate;
            SalesType();
            ddlModelName.Items.Insert(0, new ListItem("Select", "0"));/*#CC05 Added*/
            ddlCity.Items.Insert(0, new ListItem("Select", "0"));
            ddlSalesChanneltype.Items.Insert(0, new ListItem("Select", "0"));
            DdlSaleschannel.Items.Insert(0, new ListItem("Select", "0"));
            // BindModel();/*#CC05 Commented*/
            bindProductCategory();
            ddlSku.Items.Insert(0, new ListItem("Select", "0"));
            BindState();
            fillLocations();
            /*#CC04 start*/
            if (Session["RSMLevelName"] != null)
            {
                lbllocation.Text = HttpContext.Current.Session["RSMLevelName"].ToString();
            } /*#CC04 end*/


        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        fncHide();
        if (hfSearch.Value == "1")
            BindGrid();

        //ShowScrollBar(true);
        ucDateFrom.Date = PageBase.Fromdate;
        ucDateTo.Date = PageBase.ToDate;
        SalesType();
        ddlModelName.Items.Insert(0, new ListItem("Select", "0"));/*#CC05 Added*/
        ddlCity.Items.Insert(0, new ListItem("Select", "0"));
        ddlSalesChanneltype.Items.Insert(0, new ListItem("Select", "0"));
        DdlSaleschannel.Items.Insert(0, new ListItem("Select", "0"));
        // BindModel();/*#CC05 Commented*/
        bindProductCategory();
        ddlSku.Items.Insert(0, new ListItem("Select", "0"));
        BindState();
        fillLocations();
        /*#CC04 start*/
        if (Session["RSMLevelName"] != null)
        {
            lbllocation.Text = HttpContext.Current.Session["RSMLevelName"].ToString();
        } /*#CC04 end*/

    }
    #endregion

    #region User defined function
    private void BindGrid()
    {
        try
        {

            DataSet DsSalesInfo;
            using (ReportData objRD = new ReportData())
            {
                if (ddlSalesType.SelectedIndex == 0)
                {
                    ucMsg.ShowInfo("Select Sales Type");
                    return;
                }

                objRD.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                objRD.DateTo = Convert.ToDateTime(ucDateTo.Date);
                //objRD.SalesChannelID = PageBase.SalesChanelID;
                objRD.UserId = PageBase.UserId;
                objRD.CompanyId = PageBase.ClientId;
                objRD.SalesType = Convert.ToInt16(ddlSalesType.SelectedValue);
                objRD.ModelId = Convert.ToInt32(ddlModelName.SelectedValue);
                objRD.SkuId = Convert.ToInt32(ddlSku.SelectedValue);
                objRD.stateid = Convert.ToInt32(ddlState.SelectedValue);
                objRD.ProductCategtoryid = Convert.ToInt32(ddlProductCategory.SelectedValue);
                objRD.OrgHierarchyId = Convert.ToInt32(ddllocation.SelectedValue);
                objRD.intWantZeroQuantity = chkZeroQuantity.Checked ? Convert.ToInt16(1) : Convert.ToInt16(0);
                objRD.WithOrWithoutSerialBatch = Convert.ToInt16(0);

                if ((Convert.ToInt32(ddlSalesChanneltype.SelectedValue) > 0) && (DdlSaleschannel.SelectedValue != ""))/*#CC03*/
                {
                    //if (DdlSaleschannel.SelectedValue != "")
                    objRD.SalesChannelID = Convert.ToInt32(DdlSaleschannel.SelectedValue);/*#CC03*/
                    //else
                    //    objRD.SalesChannelID = 0;
                }
                else
                {
                    objRD.SalesChannelID = 0;
                }
                //objRD.SalesChannelID = Convert.ToInt32(DdlSaleschannel.SelectedValue);/*#CC03*/

                if ((Convert.ToInt32(ddlState.SelectedValue) > 0) && (ddlCity.SelectedValue != ""))/*#CC03*/
                {
                    //if (ddlCity.SelectedValue!="")
                    objRD.CityId = Convert.ToInt32(ddlCity.SelectedValue);/*#CC03*/
                }
                else
                {
                    objRD.CityId = 0;
                }
                objRD.ComingFrom = 1;//Pivot

                DsSalesInfo = objRD.GetFlatSalesReportCommonForAll();
                //DsSalesInfo = objRD.GetSalesReport();
                hfSearch.Value = "1";

                if (DsSalesInfo.Tables[0].Rows.Count > 0)
                {
                    ASPxPvtGrd.DataSource = DsSalesInfo;
                    ASPxPvtGrd.DataBind();
                    pnlSearch.Visible = true;
                }
                else
                {
                    ucMsg.ShowInfo(Resources.Messages.NoRecord);
                    pnlSearch.Visible = false;

                }
                if (ddlSalesType.SelectedValue == "3")
                    ASPxPvtGrd.Fields["PvtRetailerType"].Visible = true;
                else
                    ASPxPvtGrd.Fields["PvtRetailerType"].Visible = false;
            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    //protected void ShowScrollBar(bool show)
    //{
    //    if (show)
    //    {

    //        ASPxPvtGrd.Width = Unit.Parse("950px");
    //        ASPxPvtGrd.OptionsView.ShowHorizontalScrollBar = true;
    //    }
    //    else
    //    {
    //        ASPxPvtGrd.Width = Unit.Parse("100%");
    //        ASPxPvtGrd.OptionsView.ShowHorizontalScrollBar = false;
    //    }

    //}
    private void Export(bool saveAs)
    {


        ASPxPivotGridExporter1.OptionsPrint.PrintHeadersOnEveryPage = checkPrintHeadersOnEveryPage.Checked;
        ASPxPivotGridExporter1.OptionsPrint.PrintFilterHeaders = checkPrintFilterHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintColumnHeaders = checkPrintColumnHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintRowHeaders = checkPrintRowHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintDataHeaders = checkPrintDataHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;

        string fileName = string.Format("SalesReport_{0}", DateTime.Today.ToString("dd_MM_yyyy"));
        switch (ddlExportFormat.SelectedIndex)
        {
            case 0:
                ASPxPivotGridExporter1.ExportXlsToResponse(fileName, saveAs);
                break;
            case 1:
                ASPxPivotGridExporter1.ExportXlsxToResponse(fileName, saveAs);
                break;
        }
    }
    protected void buttonOpen_Click(object sender, EventArgs e)
    {
        Export(false);
    }
    protected void buttonSaveAs_Click(object sender, EventArgs e)
    {
        Export(true);
    }


    private void SalesType()
    {
        try
        {
            DataTable dt = new DataTable();
            ddlSalesType.Items.Clear();
            using (ReportData objSalesType = new ReportData())
            {
                objSalesType.SalesChannelTypeID = PageBase.SalesChanelTypeID;
                objSalesType.HierarchyLevelId = PageBase.HierarchyLevelID;
                objSalesType.CompanyId = PageBase.ClientId;
                dt = objSalesType.GetSalesTypeforReport();
            };
            String[] colArray = { "SalesTypeID", "SalesTypeName" };
            PageBase.DropdownBinding(ref ddlSalesType, dt, colArray);

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }



    private void fncHide()
    {
        ucMsg.ShowControl = false;
        pnlSearch.Visible = false;
    }
    bool pageValidate()
    {
        hfSearch.Value = "0";
        int _date = 0;
        if ((Convert.ToDateTime(ucDateFrom.Date) > DateTime.Now.Date) || (Convert.ToDateTime(ucDateTo.Date) > DateTime.Now.Date))
        {
            ucMsg.ShowInfo(Resources.Messages.DateRangeValidation);
            return false;
        }

        _date = DateTime.Compare(Convert.ToDateTime(ucDateTo.Date), Convert.ToDateTime(ucDateFrom.Date));
        if (_date < 0)
        {
            ucMsg.ShowInfo(Resources.Messages.InvalidDate);
            return false;
        }

        return true;
    }
    #endregion

    #region Page Control Events

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (pageValidate())
            BindGrid();
        else
            pnlSearch.Visible = false;

    }
    #endregion

    /*#CC03 start*/
    void FillsalesChannelType()
    {
        // using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        //{


        //    string[] str = { "SalesChannelTypeID", "SalesChannelTypeName" };
        //    PageBase.DropdownBinding(ref ddlType, ObjSalesChannel.GetSalesChannelType(), str);
        //    if (PageBase.SalesChanelID != 0)
        //    {
        //        ddlType.SelectedValue = PageBase.SalesChanelTypeID.ToString ();
        //        ddlType.Enabled = false;
        //    }
        //    else
        //    {
        //        ddlType.Enabled = true;
        //    }
        //};
        DataTable dt = new DataTable();
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {


            ddlSalesChanneltype.Items.Clear();

            string[] str = { "SalesChannelTypeID", "SalesChannelTypeName" };
            ObjSalesChannel.SaleTypeID = Convert.ToInt32(ddlSalesType.SelectedValue);
            ObjSalesChannel.CompanyId = PageBase.ClientId;
            PageBase.DropdownBinding(ref ddlSalesChanneltype, ObjSalesChannel.GetSaleschannelTypeBasedOnSaleType(), str);

            //PageBase.DropdownBinding(ref DdlSaleschannel, ObjSalesChannel.GetSalesChannelListWithRetailer(), str);


            //if (PageBase.SalesChanelID != 0)
            //{
            //    ddlSalesChanneltype.SelectedValue = PageBase.SalesChanelTypeID.ToString();
            //    if (PageBase.SalesChanelTypeID != 5 && PageBase.IsRetailerStockTrack == 1)
            //        ddlSalesChanneltype.Items.Add(new ListItem("Retailer", "101"));

            //}
            //else if (PageBase.SalesChanelID == 0 & PageBase.BaseEntityTypeID == 3)
            //{
            //    ddlSalesChanneltype.Items.Clear();
            //    ddlSalesChanneltype.Items.Insert(0, new ListItem("Retailer", "101"));
            //    ddlSalesChanneltype.Enabled = false;
            //}
            //else if (PageBase.IsRetailerStockTrack == 1)
            //{
            //    ddlSalesChanneltype.Items.Add(new ListItem("Retailer", "101"));
            //    ddlSalesChanneltype.Enabled = true;
            //}
        };
    }

    void BindSalesChannel()
    {
        try
        {
            DdlSaleschannel.Items.Clear();
            using (SalesChannelData ObjSalesChannel = new SalesChannelData())
            {
                ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(ddlSalesChanneltype.SelectedValue);
                ObjSalesChannel.ActiveStatus = 255;
                ObjSalesChannel.BaseEntityTypeID = Convert.ToInt16(PageBase.BaseEntityTypeID);
                ObjSalesChannel.UserID = PageBase.UserId;
                ObjSalesChannel.CompanyId = PageBase.ClientId;
                ObjSalesChannel.ActiveStatus = 1;
                string[] str = { "SalesChannelid", "SalesChannelName" };
                PageBase.DropdownBinding(ref DdlSaleschannel, ObjSalesChannel.GetSalesChannelListForPivotandStock(), str);
                ViewState["LoggedIn"] = ObjSalesChannel.LoggedInSalesChannelID;
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }/*#CC03 end*/
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        hfSearch.Value = "0";
        pnlSearch.Visible = false;
        ddlSalesType.SelectedIndex = 0;
        ucDateFrom.Date = PageBase.Fromdate;
        ucDateTo.Date = PageBase.ToDate;
        ddlModelName.SelectedValue = "0";
        ddlSku.Items.Clear();
        ddlSku.ClearSelection();
        ddlSku.Items.Insert(0, new ListItem("Select", "0"));



    }
    void BindModel()
    {
        using (ProductData objproduct = new ProductData())
        {
            /* objproduct.ModelProdCatId = 0;*/
            /*#CC05 Commented*/
            objproduct.ModelProdCatId = Convert.ToInt32(ddlProductCategory.SelectedValue);/*#CC05 Added*/
            objproduct.ModelSelectionMode = 1;
            objproduct.CompanyId = PageBase.ClientId;
            objproduct.UserId = PageBase.UserId;
            DataTable dtmodelfil = objproduct.SelectModelInfo();
            String[] colArray1 = { "ModelID", "ModelName" };
            PageBase.DropdownBinding(ref ddlModelName, dtmodelfil, colArray1);


        }
    }

    void BindSku()
    {
        using (RetailerData objsku = new RetailerData())
        {
            objsku.CompanyId = PageBase.ClientId;
            objsku.UserID = PageBase.UserId;
            objsku.ModelId = Convert.ToInt32(ddlModelName.SelectedValue);
            DataTable dtmodelfil = objsku.GetAllActiveSKU();
            String[] colArray1 = { "Skuid", "SkuName" };
            PageBase.DropdownBinding(ref ddlSku, dtmodelfil, colArray1);


        }
    }
    protected void ddlModelName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlModelName.SelectedValue == "0")
            {
                ddlSku.Items.Clear();
                ddlSku.Items.Insert(0, new ListItem("Select", "0"));
                ddlSku.SelectedValue = "0";
            }
            else
            {
                BindSku();
            }
            ASPxPvtGrd.DataSource = null;
            ASPxPvtGrd.DataBind();
            pnlSearch.Visible = false;
            ucMsg.Visible = false;

        }
        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    void bindProductCategory()
    {


        using (ProductData objproduct = new ProductData())
        {
            objproduct.CompanyId = PageBase.ClientId;
            objproduct.UserId = PageBase.UserId;
            DataTable dt = objproduct.SelectAllProdCatInfo();
            ddlProductCategory.Items.Clear();

            String[] colArray1 = { "ProductCategoryID", "ProductCategoryName" };
            PageBase.DropdownBinding(ref ddlProductCategory, dt, colArray1);
            //ddlProductCategory.Items.Insert(0, new ListItem("Select", "0"));
        }

    }

    void BindState()
    {
        ddlState.Items.Clear();
        using (MastersData obj = new MastersData())
        {
            DataTable dt;
            obj.StateSelectionMode = 1;
            obj.StateCountryid = 0;/*because there is no multi country concept is there*/
            obj.CompanyId = PageBase.ClientId;
            obj.UserId = PageBase.UserId;
            dt = obj.SelectStateInfo();
            String[] colArray = { "StateID", "StateName" };
            PageBase.DropdownBinding(ref ddlState, dt, colArray);
            //ddlState.Items.Insert(0, new ListItem("Select", "0"));
        }
    }
    /*#CC03 start*/
    void BindCity()
    {
        try
        {
            using (GeographyData ObjGeography = new GeographyData())
            {
                if (ddlState.SelectedIndex > 0)
                {
                    ObjGeography.SearchCondition = EnumData.eSearchConditions.Active;
                    ObjGeography.StateId = Convert.ToInt16(ddlState.SelectedValue);
                    String[] StrCol = new String[] { "CityId", "CityName" };
                    ddlCity.Items.Clear();
                    PageBase.DropdownBinding(ref ddlCity, ObjGeography.GetAllCityByParameters(), StrCol);

                }
                else if (ddlState.SelectedIndex == 0)
                {
                    ddlCity.Items.Clear();
                    ddlCity.Items.Insert(0, new ListItem("Select", "0"));

                }

            };
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    /*#CC03 end*/
    protected void fillLocations()
    {
        using (OrgHierarchyData obj = new OrgHierarchyData())
        {
            obj.SalesChanelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
            obj.SearchMode = 3;
            obj.UserID = PageBase.UserId;
            obj.CompanyId = PageBase.ClientId;
            DataTable dt = obj.GetOrgHierarchy();
            String[] colArray = { "OrgnhierarchyID", "LocationName" };
            PageBase.DropdownBinding(ref ddllocation, dt, colArray);
        }
    }
    /*#CC03 start*/
    protected void ddlSalesChanneltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        DdlSaleschannel.Items.Clear();
        if (ddlSalesChanneltype.SelectedValue == "12")
        {
            Label2.Visible = false;
            DdlSaleschannel.Visible = false;
        }
        else
        {
            Label2.Visible = true;
            DdlSaleschannel.Visible = true;
            BindSalesChannel();
        }
    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCity();
    }
    protected void ddlSalesType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillsalesChannelType();
        DdlSaleschannel.Items.Clear();
        DdlSaleschannel.Items.Insert(0, new ListItem("Select", "0"));
        BindGrid();
    } /*#CC03 end*/
    /*#CC05 Added*/
    protected void ddlProductCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlProductCategory.SelectedValue == "0")
            {
                ddlModelName.Items.Clear();
                ddlModelName.Items.Insert(0, new ListItem("Select", "0"));
                ddlModelName.SelectedValue = "0";
                if (ddlModelName.SelectedValue == "0")
                {
                    ddlSku.Items.Clear();
                    ddlSku.Items.Insert(0, new ListItem("Select", "0"));
                    ddlSku.SelectedValue = "0";
                }
            }
            else
            {
                ddlSku.Items.Clear();
                ddlSku.Items.Insert(0, new ListItem("Select", "0"));
                ddlSku.SelectedValue = "0";
                BindModel();
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
}
