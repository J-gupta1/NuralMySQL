#region NameSpace
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
/*Change Log:
 * 26-Mar-14, Rakesh Goel, #CC01 - Only Channel Type self and child should display. Currently showing all
 *   * 14-June-16, Karam Chand Sharma, #CC02 - Load retailer option in case on ND check
 *   15-Nov-2017,Vijay Kumar Prajapati,#CC03-Display Distributor,MD,Retailar code.
  *   05-June-2018,Rajnish Kumar ,#CC04-SaleschannelId and CityId Filter.
 *  18 June 2018,Rajnish Kumar,#CC05,Region Level Name According to RSMLevel Configuration Value.
 *  01-Oct-2018,Vijay Kumar Prajapati,#CC06,Get model according to productcateryId selection.
*/
public partial class Reports_SalesChannel_StockRpt : PageBase
{
    #region PageLoad Event
    protected void Page_Load(object sender, EventArgs e)
    {

        EnableViewState = false;
        fncHide();
        if (!IsPostBack)
        {
            FillsalesChannelType();
            ShowScrollBar(true);
            ucDateTo.Date = PageBase.ToDate;
            ddlModelName.Items.Insert(0, new ListItem("Select", "0"));/*#CC06 Added*/
            // BindModel();/*#CC06 Commented*/
            ddlSku.Items.Insert(0, new ListItem("Select", "0"));
            ddlCity.Items.Insert(0, new ListItem("Select", "0"));
            DdlSaleschannel.Items.Insert(0, new ListItem("Select", "0"));
            bindProductCategory();
            BindState();
            fillLocations();
            /*#CC05 start*/
            if (Session["RSMLevelName"] != null)
            {
                lbllocation.Text = HttpContext.Current.Session["RSMLevelName"].ToString();
            } /*#CC05 end*/


        }
        //if (hfSearch.Value == "1")
            BindGrid();
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        fncHide();

        FillsalesChannelType();
        ShowScrollBar(true);
        ucDateTo.Date = PageBase.ToDate;
        ddlModelName.Items.Insert(0, new ListItem("Select", "0"));/*#CC06 Added*/
                                                                  // BindModel();/*#CC06 Commented*/
        ddlSku.Items.Insert(0, new ListItem("Select", "0"));
        ddlCity.Items.Insert(0, new ListItem("Select", "0"));
        DdlSaleschannel.Items.Insert(0, new ListItem("Select", "0"));
        bindProductCategory();
        BindState();
        fillLocations();
        /*#CC05 start*/
        if (Session["RSMLevelName"] != null)
        {
            lbllocation.Text = HttpContext.Current.Session["RSMLevelName"].ToString();
        } /*#CC05 end*/

        //if (hfSearch.Value == "1")
            BindGrid();
    }
    #endregion


    #region User defined function
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
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {


            ddlType.Items.Clear();
            //string[] str = { "SalesChannelTypeID", "SalesChannelTypeName" };
            //PageBase.DropdownBinding(ref ddlType, ObjSalesChannel.GetSalesChannelType(), str);

            //DataTable dt = new DataTable();
            //String[] StrCol = new String[] { "SalesChannelTypeID", "SalesChannelTypeName" };
            //ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
            //dt = ObjSalesChannel.GetSalesChannelTypeV2();
            string[] str = { "SalesChannelTypeID", "SalesChannelTypeName" };
            //ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
            //PageBase.DropdownBinding(ref ddlType, ObjSalesChannel.GetSalesChannelType(), str);  //#CC01 commented

            //--#CC01 add start
            //ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
            //PageBase.DropdownBinding(ref ddlType, ObjSalesChannel.prcGetCompleteSalesChannelTypeSelfandChild(), str);   //to be implemented..on hold right now
            //PageBase.DropdownBinding(ref ddlType, ObjSalesChannel.GetSalesChannelType(), str); 
            ObjSalesChannel.UserID = PageBase.UserId;
            ObjSalesChannel.CompanyId = PageBase.ClientId;
            PageBase.DropdownBinding(ref ddlType, ObjSalesChannel.GetSalesChannelTypeAndBaseEntityType(), str);
            //--#CC01 add end

            if (PageBase.SalesChanelID != 0)
            {
                //ddlType.SelectedValue = PageBase.SalesChanelTypeID.ToString();
                ddlType.SelectedValue = PageBase.SalesChanelTypeID.ToString() + "#" + PageBase.BaseEntityTypeID.ToString();
                /* if (PageBase.SalesChanelTypeID != 5 && PageBase.IsRetailerStockTrack==1)
                 ddlType.Items.Add(new ListItem("Retailer", "101"));*/
                //ddlType.Enabled = false;
                ///*#CC02 START ADDED*/
                //if (PageBase.SalesChanelTypeID == 6)
                //{
                //    ddlType.Items.Add(new ListItem("Retailer", "101"));
                //    ddlType.Enabled = true;
                //}/*#CC02 END ADDED*/
            }
            else if (PageBase.SalesChanelID == 0 & PageBase.BaseEntityTypeID == 3)
            {
                ddlType.Items.Clear();
                /*ddlType.Items.Insert(0, new ListItem("Retailer", "101"));*/
                ddlType.Enabled = false;
            }
            else if (PageBase.IsRetailerStockTrack == 1)
            {
                /*ddlType.Items.Add(new ListItem("Retailer", "101"));*/
                ddlType.Enabled = true;
            }
        };
    }
    private void BindGrid()
    {
        try
        {
            DataSet DsStockInfo;
            using (ReportData objRD = new ReportData())
            {
                objRD.DateTo = Convert.ToDateTime(ucDateTo.Date);
                objRD.UserId = PageBase.UserId;
                objRD.CompanyId = PageBase.ClientId;
                //objRD.SalesChannelTypeID =Convert.ToInt16(ddlType.SelectedValue);
                string[] strEntityTypeBaseEntityType = ddlType.SelectedValue.ToString().Split('#');
                objRD.SalesChannelTypeID = Convert.ToInt16(strEntityTypeBaseEntityType[0]);
                objRD.ModelId = Convert.ToInt32(ddlModelName.SelectedValue);
                objRD.SkuId = Convert.ToInt32(ddlSku.SelectedValue);
                objRD.stateid = Convert.ToInt32(ddlState.SelectedValue);
                objRD.ProductCategtoryid = Convert.ToInt32(ddlProductCategory.SelectedValue);
                objRD.OrgHierarchyId = Convert.ToInt32(ddllocation.SelectedValue);
                objRD.intWantZeroQuantity = chkZeroQuantity.Checked ? Convert.ToInt16(1) : Convert.ToInt16(0);
                if ((Convert.ToInt32(strEntityTypeBaseEntityType[0]) > 0) && (DdlSaleschannel.SelectedValue != ""))
                {
                    objRD.SalesChannelID = Convert.ToInt32(DdlSaleschannel.SelectedValue);
                }/*#CC04*/
                else { objRD.SalesChannelID = 0; }

                if (Convert.ToInt32(ddlState.SelectedValue) > 0 && (ddlCity.SelectedValue != ""))/*#CC04*/
                {
                    objRD.CityId = Convert.ToInt32(ddlCity.SelectedValue);/*#CC04*/
                }
                else
                {
                    objRD.CityId = 0;
                }/*#CC04*/
                objRD.ComingFrom = 1;
                if (strEntityTypeBaseEntityType.Count() > 1)
                {
                    if (strEntityTypeBaseEntityType[1] == "3")
                    {
                        objRD.BaseEntityTypeID = PageBase.BaseEntityTypeID;
                        DsStockInfo = objRD.GetStockReportRetailer();
                        /*#CC03 Added Started*/
                        PivotGridField19.Visible = false;
                        PivotGridField7.Visible = false;
                        PivotGridField21.Visible = true;
                        PivotGridField20.Visible = true;
                        /*#CC03 Added End*/
                    }
                    else
                    {
                        //DsStockInfo = objRD.GetStockReport();

                        DsStockInfo = objRD.GetStockReportCommon();
                        /*#CC03 Added Started*/
                        PivotGridField20.Visible = false;
                        PivotGridField21.Visible = false;
                        PivotGridField19.Visible = true;
                        PivotGridField7.Visible = true;
                        /*#CC03 Added End*/

                    }
                    if (DsStockInfo.Tables[0].Rows.Count > 0)
                    {

                        hfSearch.Value = "1";
                        ASPxPvtGrd.DataSource = DsStockInfo;
                        ASPxPvtGrd.DataBind();
                        pnlSearch.Visible = true;
                    }
                    else
                    {
                        ucMsg.ShowInfo(Resources.Messages.NoRecord);
                        pnlSearch.Visible = false;
                    }
                }
                else
                {
                    ASPxPvtGrd.DataSource = null;
                    ASPxPvtGrd.DataBind();
                    pnlSearch.Visible = false;
                }
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

        hfSearch.Value = "0";
        if ((Convert.ToDateTime(ucDateTo.Date) > DateTime.Now.Date))
        {
            ucMsg.ShowInfo("Date should be less than or equal to current date.");
            return false;
        }
        if (ddlType.SelectedValue == "0")
        {
            ucMsg.ShowInfo(Resources.Messages.MandatoryField);
            return false;
        }
        return true;
    }
    private void Export(bool saveAs)
    {


        ASPxPivotGridExporter1.OptionsPrint.PrintHeadersOnEveryPage = checkPrintHeadersOnEveryPage.Checked;
        ASPxPivotGridExporter1.OptionsPrint.PrintFilterHeaders = checkPrintFilterHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintColumnHeaders = checkPrintColumnHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintRowHeaders = checkPrintRowHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintDataHeaders = checkPrintDataHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;

        string fileName = string.Format("StockReport_{0}", DateTime.Today.ToString("dd_MM_yyyy"));
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

    private void fncHide()
    {
        ucMsg.ShowControl = false;
        pnlSearch.Visible = false;
    }



    protected void ShowScrollBar(bool show)
    {
        if (show)
        {

            ASPxPvtGrd.Width = Unit.Parse("950px");
            ASPxPvtGrd.OptionsView.ShowHorizontalScrollBar = true;
        }
        else
        {
            ASPxPvtGrd.Width = Unit.Parse("100%");
            ASPxPvtGrd.OptionsView.ShowHorizontalScrollBar = false;
        }

    }
    #endregion

    #region Page Control Event

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        hfSearch.Value = "0";
        pnlSearch.Visible = false;
        ucDateTo.Date = PageBase.ToDate;
        //ddlType.SelectedValue = "0";
        if (PageBase.SalesChanelID > 0)
        {
            ddlType.SelectedValue = PageBase.SalesChanelTypeID.ToString();
            ddlType.Enabled = false;
        }
        else
        {
            ddlType.Enabled = true;
            ddlType.SelectedIndex = 0;
        }
        DdlSaleschannel.SelectedIndex = 0;
        ucDateTo.Date = "";
        ddllocation.SelectedIndex = 0;
        ddlState.SelectedIndex = 0;
        ddlCity.SelectedIndex = 0;
        ddlProductCategory.SelectedIndex = 0;
        ddlModelName.SelectedIndex = 0;
        ddlSku.SelectedIndex = 0;
        chkZeroQuantity.Checked = false;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (pageValidate())
            BindGrid();
        else
            pnlSearch.Visible = false;
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
    void BindSku()
    {
        using (RetailerData objsku = new RetailerData())
        {
            objsku.ModelId = Convert.ToInt32(ddlModelName.SelectedValue);
            DataTable dtmodelfil = objsku.GetAllActiveSKU();
            String[] colArray1 = { "Skuid", "SkuName" };
            PageBase.DropdownBinding(ref ddlSku, dtmodelfil, colArray1);


        }
    }
    void BindModel()
    {
        using (ProductData objproduct = new ProductData())
        {
            /* objproduct.ModelProdCatId = 0;*/
            /*#CC06 Commented*/
            objproduct.ModelProdCatId = Convert.ToInt32(ddlProductCategory.SelectedValue);/*#CC06 Added*/
            objproduct.ModelSelectionMode = 1;
            DataTable dtmodelfil = objproduct.SelectModelInfo();
            String[] colArray1 = { "ModelID", "ModelName" };
            PageBase.DropdownBinding(ref ddlModelName, dtmodelfil, colArray1);


        }
    }
    void bindProductCategory()
    {


        using (ProductData objproduct = new ProductData())
        {
            objproduct.CompanyId = PageBase.ClientId;
            DataTable dt = objproduct.SelectAllProdCatInfo();
            ddlProductCategory.Items.Clear();

            String[] colArray1 = { "ProductCategoryID", "ProductCategoryName" };
            PageBase.DropdownBinding(ref ddlProductCategory, dt, colArray1);
            ddlProductCategory.Items.Insert(0, new ListItem("Select", "0"));
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
            dt = obj.SelectStateInfo();
            String[] colArray = { "StateID", "StateName" };
            PageBase.DropdownBinding(ref ddlState, dt, colArray);
            //ddlState.Items.Insert(0, new ListItem("Select", "0"));
        }
    }
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
    #endregion

    /*#CC04 start*/
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string[] s1 = ddlType.SelectedValue.ToString().Split('#');
        if (Convert.ToInt32(s1[0]) == 12)
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
    void BindSalesChannel()
    {
        try
        {
            DdlSaleschannel.Items.Clear();
            using (SalesChannelData ObjSalesChannel = new SalesChannelData())
            {
                string[] s1 = ddlType.SelectedValue.ToString().Split('#');
                ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(s1[0]);
                ObjSalesChannel.ActiveStatus = 255;
                ObjSalesChannel.BaseEntityTypeID = Convert.ToInt16(PageBase.BaseEntityTypeID);
                ObjSalesChannel.UserID = PageBase.UserId;
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
    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCity();
    }
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
    }/*#CC04 end*/
    /*#CC06 Added Started*/
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
    /*#CC06 Added End*/
}
