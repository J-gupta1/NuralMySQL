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
#endregion


/* ==============================================================================================================
 * Change Log
 * ==============================================================================================================
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 17-Aug-2017, Sumit Maurya, #CC01, UserID provided to get dropdown value according to login user.
  * 05-June-2018, Rajnish Kumar, #CC02, SalesChannelId and CityId for Filter
 * 18-June-2018, Rajnish Kumar, #CC03, Region Level Name According to RSMLevel Configuration Value.
 * 27-Aug-2018, Rakesh Raj, #CC04, Display Column Dynamically from database 
 * 01-Oct-2018,Vijay Kumar Prajapati,#CC05,Get model according to productcateryId selection.
 * 22-Nov-2022, Rinku Sharma,#CC06, Set Company ID in all the DropDown when bind.
 * ==============================================================================================================
 */

public partial class Reports_SalesChannel_DownloadSalesRpt :PageBase
{
    string strExportFileName = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        fncHide();
        if (!IsPostBack)
        {

            ucDateFrom.Date = PageBase.Fromdate;
            ucDateTo.Date = PageBase.ToDate;
            SalesType();
            ddlModelName.Items.Insert(0, new ListItem("Select", "0"));/*#CC05 Added*/
            ddlCity.Items.Insert(0, new ListItem("Select", "0"));
            ddlSalesChanneltype.Items.Insert(0, new ListItem("Select", "0"));
            DdlSaleschannel.Items.Insert(0, new ListItem("Select", "0"));
            // BindModel(); /*#CC05 Commented*/
            ddlSku.Items.Insert(0, new ListItem("Select", "0"));
            bindProductCategory();
            BindState();
            fillLocations();
             /*#CC03 start*/
            if (Session["RSMLevelName"] != null)
            {
                lbllocation.Text = HttpContext.Current.Session["RSMLevelName"].ToString();
            } /*#CC03 end*/
        }

    }
    void SalesType()
    {
        try
        {
            DataTable dt = new DataTable();
            ddlSalesType.Items.Clear();
            using (ReportData objSalesType = new ReportData())
            {
                objSalesType.SalesChannelTypeID = PageBase.SalesChanelTypeID;
                objSalesType.HierarchyLevelId = PageBase.HierarchyLevelID;
                objSalesType.CompanyId = PageBase.ClientId; /* #CC06 Added */
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

    /*##CC02 start*/
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
            ObjSalesChannel.CompanyId = PageBase.ClientId;/* #CC06 Added */
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
    }/*##CC02 end*/
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {

            using (ReportData objRD = new ReportData())
            {
                if (pageValidate())
                {

                    Int32 intResult = 1;
                    //string[] strFilePath = PageBase.strBCPFilePath.Split(new char[] { '\\' });
                    //string path = strFilePath[4].ToString(); Pankaj Dhingra
                    strExportFileName = PageBase.importExportCSVFileName;
                    objRD.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                    objRD.DateTo = Convert.ToDateTime(ucDateTo.Date);
                    objRD.SalesChannelID = PageBase.SalesChanelID;
                    objRD.UserId = PageBase.UserId;
                    objRD.CompanyId = PageBase.ClientId;/* #CC06 Added */
                    objRD.SalesType = Convert.ToInt16(ddlSalesType.SelectedValue);
                    objRD.ModelId = Convert.ToInt32(ddlModelName.SelectedValue);
                    objRD.SkuId = Convert.ToInt32(ddlSku.SelectedValue);
                    objRD.stateid = Convert.ToInt32(ddlState.SelectedValue);
                    objRD.ProductCategtoryid = Convert.ToInt32(ddlProductCategory.SelectedValue);
                    objRD.OrgHierarchyId = Convert.ToInt32(ddllocation.SelectedValue);
                    objRD.intWantZeroQuantity = chkZeroQuantity.Checked ? Convert.ToInt16(1) : Convert.ToInt16(0);
                    //objRD.FilePath =PageBase.strBCPFilePath + strExportFileName;      Pankaj Dhingra
                    objRD.FilePath = "SalesReport"+strExportFileName;
                    objRD.WithOrWithoutSerialBatch = chkSB.Checked ? Convert.ToInt16(1) : Convert.ToInt16(0);
                    if (Convert.ToInt32(ddlSalesChanneltype.SelectedValue) != 12)
                    {
                        if (Convert.ToInt32(ddlSalesChanneltype.SelectedValue) > 0)/*#CC02*/
                        {
                            objRD.SalesChannelID = Convert.ToInt32(DdlSaleschannel.SelectedValue);/*#CC02*/
                        }
                    }
                    
                    objRD.SalesChannelID = 0;/*#CC02*/
                    if (Convert.ToInt32(ddlState.SelectedValue) > 0)/*#CC02*/
                    {
                        objRD.CityId = Convert.ToInt32(ddlCity.SelectedValue);/*#CC02*/
                    }
                    else
                        objRD.CityId = 0; ;/*#CC02*/
                    objRD.ComingFrom = 0;//excel
                    DataSet ds= new DataSet();
                    ds = objRD.GetFlatSalesReportCommonForAll();

                    //if (chkSB.Checked)
                    //    intResult = objRD.GetFlatSalesReportbybcpSB();
                    //else
                    //    intResult = objRD.GetFlatSalesReportbybcp();
                    //if (objRD.Result == 0)
                    if (ds.Tables.Count > 0)
                    {
                        //Response.Redirect((siteURL + strBCPFilePath + objRD.FilePath));
                        if(ds.Tables[0].Rows.Count>0)
                        {
                            //  objRD.headerReplacement(ds.Tables[0]); /*CC04*/
                            PageBase.ExportToExecl(ds, "SalesReport");
                        }
                        else 
                        {
                            ucMsg.ShowInfo(Resources.Messages.NoRecord);

                        }
                        //Old one Block Pankaj Dhingra
                        //string filePath = "../../" + PageBase.strGlobalDownloadExcelPathRoot + path + "/" + strExportFileName;
                        //HttpContext.Current.Response.Clear();
                        //HttpContext.Current.Response.Charset = "";
                        //HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=SalesReport" + strExportFileName);
                        //HttpContext.Current.Response.ContentType = "application/vnd.csv";
                        //PageBase.ClearBuffer();
                        //HttpContext.Current.Response.WriteFile(filePath);
                        //HttpContext.Current.Response.End();
                    }
                    else if (objRD.Result == 1)
                    {
                        ucMsg.ShowInfo(Resources.Messages.NoRecord);

                    }
                    else
                    {
                        ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);

                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }


    }
    //private void fncExportToExcel(DataSet DS)
    //{

    //    if (DS.Tables[0].Rows.Count > 0)
    //    {
    //        DataTable DsCopy = new DataTable();
    //        DsCopy = DS.Tables[0];
    //        DS.Tables.Clear();
    //        DS.Tables.Add(DsCopy);
    //        DsCopy.Columns["HO"].ColumnName = Resources.SalesHierarchy.HierarchyName1;
    //        DsCopy.Columns["RBH"].ColumnName = Resources.SalesHierarchy.HierarchyName2;
    //        DsCopy.Columns["ZBH"].ColumnName = Resources.SalesHierarchy.HierarchyName3;
    //        DsCopy.Columns["SBH"].ColumnName = Resources.SalesHierarchy.HierarchyName4;
    //        DsCopy.Columns["ASO"].ColumnName = Resources.SalesHierarchy.HierarchyName5;
    //        String FilePath = Server.MapPath("../../");
    //        string FilenameToexport = "SalesReport";
    //        PageBase.RootFilePath = FilePath;
    //        PageBase.ExportToExeclUsingOPENXMLV2(DS.Tables[0], FilenameToexport);
    //    }
    //}
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ucDateFrom.Date = PageBase.Fromdate;
        ucDateTo.Date = PageBase.ToDate;
        ddlSalesType.SelectedIndex = 0;
        ddlModelName.SelectedValue = "0";
        ddlSku.ClearSelection();
        ddlSku.Items.Clear();
        ddlSku.Items.Insert(0, new ListItem("Select", "0"));
    }
    private void fncHide()
    {
        ucMsg.ShowControl = false;

    }
    bool pageValidate()
    {

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
    void BindModel()
    {
        using (ProductData objproduct = new ProductData())
        {
           /* objproduct.ModelProdCatId = 0;*/ /*#CC05 Commented*/
            objproduct.ModelProdCatId = Convert.ToInt32(ddlProductCategory.SelectedValue);/*#CC05 Added*/
            objproduct.ModelSelectionMode = 1;
            objproduct.CompanyId = PageBase.ClientId;/* #CC06 Added */
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
            objsku.CompanyId = PageBase.ClientId;/* #CC06 Added */
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
            objproduct.CompanyId = PageBase.ClientId;/* #CC06 Added */
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
            obj.CompanyId = PageBase.ClientId;/* #CC06 Added */
            obj.UserId = PageBase.UserId;
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
            obj.CompanyId = PageBase.ClientId;/* #CC06 Added */
            DataTable dt = obj.GetOrgHierarchy();
            String[] colArray = { "OrgnhierarchyID", "LocationName" };
            PageBase.DropdownBinding(ref ddllocation, dt, colArray);
        }
    }
    /*#CC02 start*/
    protected void ddlSalesChanneltype_SelectedIndexChanged(object sender, EventArgs e)
    {
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
   
   
    /*#CC02 start*/
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
   
    protected void ddlState_SelectedIndexChanged1(object sender, EventArgs e)
    {
        BindCity();
    }
    protected void ddlSalesType_SelectedIndexChanged1(object sender, EventArgs e)
    {
        DdlSaleschannel.Items.Clear();
        FillsalesChannelType();
    } /*#CC02 end*/
    /*#CC05 Added Started*/
    protected void ddlProductCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlProductCategory.SelectedValue == "0")
            {
                ddlModelName.Items.Clear();
                ddlModelName.Items.Insert(0, new ListItem("Select", "0"));
                ddlModelName.SelectedValue = "0";
                if(ddlModelName.SelectedValue=="0")
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
    /*#CC05 Added End*/
}
