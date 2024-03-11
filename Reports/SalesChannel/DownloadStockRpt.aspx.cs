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
using DataAccess;
#endregion

/*
 * 05 Nov 2015, Karam Chand Sharma, #CC01, In case of retailer serach error not handled properly
 *  * 14-June-16, Karam Chand Sharma, #CC02 - Load retailer option in case on ND check
 *  22-Mar-17, Balram Jha  #CC03- Used TempReport data in place of DataAccess dll
 *  21-Apr-2017, Balram Jha, #CC04- server script time out increased
 *  18-Aug-2017, Sumit Maurya, #CC05, UserID provided to get dropdown value according to login user.
 *  13-Nov-2017, Balram Jha, #CC06- Export to excel used
    *  12-Feb-2018, Rajnish kumar, #CC07- Export to excel used
   *  05-June-2018, Rajnish kumar, #CC08- SalesChannelId and CityId for filter
 *  18 June 2018,Rajnish Kumar,#CC09,Region Level Name According to RSMLevel Configuration Value.
 *  3-July-2018, Rakesh Raj, #CC10, Export to CSV Feature Added
 *  3-July-2018, Rakesh Raj, #CC11, Error throwing in case of "" / blank string value
 *  18-July2018,Vijay Kumar Prajapati,#CC12, saleschannel not bind issues solve on Live.
 *  29-Aug-2018, Rakesh Raj, #CC13, Flat Report Export - Dynamic Header for Hierarchy Column Names  
 * 01-Oct-2018,Vijay Kumar Prajapati,#CC14,Get model according to productcateryId selection. 
 * 22-Nov-2022, Rinku Sharma,#CC15, Set Company ID in all the DropDown when bind.
 */

public partial class Reports_SalesChannel_DownloadStock : PageBase
{
    string strExportFileName = string.Empty;
    //#CC11
    string ExportFileLocation = HttpContext.Current.Server.MapPath("~") + "/Excel/Download/";
    protected void Page_Load(object sender, EventArgs e)
    {
        Server.ScriptTimeout = 700;//#CC04 Added
        ucMsg.ShowControl = false;
        if (!IsPostBack)
        {
            FillsalesChannelType();
            ucDateTo.Date = PageBase.ToDate;
            ddlModelName.Items.Insert(0, new ListItem("Select", "0"));/*#CC14 Added*/
           // BindModel();/*#CC14 Commented*/
            ddlSku.Items.Insert(0, new ListItem("Select", "0"));
            bindProductCategory();
            BindState();
            fillLocations();
            /*#CC12 Added Started*/
            if(ddlType.SelectedValue.ToString()!="0")
            {
                BindSalesChannel();
            }
            /*#CC12 Added End*/
            /*#CC09 start*/
            if (Session["RSMLevelName"] != null)
            {
                lbllocation.Text = HttpContext.Current.Session["RSMLevelName"].ToString();
            } /*#CC09 end*/
        }
    }
    void FillsalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {

            ddlType.Items.Clear();
            string[] str = { "SalesChannelTypeID", "SalesChannelTypeName" };
            ObjSalesChannel.UserID = PageBase.UserId;
            ObjSalesChannel.CompanyId = PageBase.ClientId;/* #CC15 Added */
            PageBase.DropdownBinding(ref ddlType, ObjSalesChannel.GetSalesChannelTypeAndBaseEntityType(), str);
            if (PageBase.SalesChanelID != 0 )
            {
                ddlType.SelectedValue = PageBase.SalesChanelTypeID.ToString() + "#" + PageBase.BaseEntityTypeID.ToString();
                ///*#CC07 START ADDED*/
                //if (PageBase.SalesChanelTypeID != 5 && PageBase.IsRetailerStockTrack == 1)
                //    ddlType.Items.Add(new ListItem("Retailer", "101")); /*#CC07 ENd ADDED*/
                //ddlType.Enabled = false;
                ///*#CC02 START ADDED*/
                //if (PageBase.SalesChanelTypeID == 6 && PageBase.IsRetailerStockTrack == 0)
                //{
                //    ddlType.Items.Add(new ListItem("Retailer", "101"));
                //    ddlType.Enabled = true;
                //}/*#CC02 END ADDED*/
            }
            ///*#CC07 START ADDED*/
            /*else if (PageBase.SalesChanelID == 0 & PageBase.BaseEntityTypeID == 3)
            {
                ddlType.Items.Clear();
                ddlType.Items.Insert(0, new ListItem("Retailer", "101"));
                ddlType.Enabled = false;
            }
            else if (PageBase.IsRetailerStockTrack == 1)
            {
                ddlType.Items.Add(new ListItem("Retailer", "101"));
                ddlType.Enabled = true;
            }*/
            ///*#CC07 START ENd*/
        };
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            PopulateReport(1);
        }
        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }

    }

    //CC10
    protected void ExportToCSV_Click(object sender, EventArgs e)
    {
        try
        {
            PopulateReport(2);
        }
        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    //#CC10
    private void PopulateReport(Int16 Value)
    {
        try
        {

            if (ddlType.SelectedValue == "0")
            {
                ucMsg.ShowInfo(Resources.Messages.MandatoryField);
                return;
            }
            //using (ReportData objRD = new ReportData())//#CC03 Comented
            using (ReportData objRD = new ReportData())//#CC03 added
            {
                Int32 intResult = 1;
                //string[] strFilePath = PageBase.strBCPFilePath.Split(new char[] { '\\' });
                //string path = strFilePath[4].ToString();  Pankaj Dhingra
                strExportFileName = PageBase.importExportCSVFileName;
                objRD.CompanyId = PageBase.ClientId;
                objRD.DateTo = Convert.ToDateTime(ucDateTo.Date);
                objRD.UserId = PageBase.UserId;
                string[] strEntityTypeBaseEntityType = ddlType.SelectedValue.ToString().Split('#');
                objRD.SalesChannelTypeID = Convert.ToInt16(strEntityTypeBaseEntityType[0]);
                objRD.ModelId = Convert.ToInt32(ddlModelName.SelectedValue);
                objRD.CompanyId = PageBase.ClientId;/* ##CC15 Added*/
                objRD.SkuId = Convert.ToInt32(ddlSku.SelectedValue);
                objRD.FilePath = "StockReport" + strExportFileName;   //Pankaj Dhingra
                objRD.OrgHierarchyId = Convert.ToInt32(ddllocation.SelectedValue);
                objRD.intWantZeroQuantity = chkZeroQuantity.Checked ? Convert.ToInt16(1) : Convert.ToInt16(0);
                //objRD.FilePath = PageBase.strBCPFilePath + strExportFileName; Pankaj Dhingra
                objRD.stateid = Convert.ToInt32(ddlState.SelectedValue);
                objRD.ProductCategtoryid = Convert.ToInt32(ddlProductCategory.SelectedValue);
                //#CC11
                if (!string.IsNullOrEmpty(DdlSaleschannel.SelectedValue))
                {
                    if (Convert.ToInt32(DdlSaleschannel.SelectedValue) > 0 )
                    {
                        objRD.SalesChannelID = Convert.ToInt32(DdlSaleschannel.SelectedValue);
                    }
                }
                else
                {
                    objRD.SalesChannelID = 0;/*#CC08*/
                }
                if (Convert.ToInt32(ddlState.SelectedValue) > 0)/*#CC08*/
                {
                    objRD.CityId = Convert.ToInt32(ddlCity.SelectedValue);/*#CC08*/
                }
                else
                    objRD.CityId = 0;/*;/*#CC08*/
                if (chkZeroQuantity.Checked)
                    objRD.intWantZeroQuantity = 1;
                objRD.ComingFrom = 0;
                DataSet ds = new DataSet();

                //#CC10
                if (Value == 2)
                {
                    ds = objRD.GetStockReportCommon();
                    
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        /*#CC13*/// objRD.headerReplacement(ds.Tables[0]);
                        ds.Tables[0].ToCSV("StockReport", ExportFileLocation);

                    }
                    else
                    {
                        ucMsg.ShowInfo(Resources.Messages.NoRecord);
                    }
                }
                else
                {
                    if (strEntityTypeBaseEntityType[1] == "3")
                    {
                        objRD.BaseEntityTypeID = PageBase.BaseEntityTypeID;
                        ds = objRD.GetStockReportExcelbybcpRetailer(out intResult);
                        if (intResult > 0)
                        {
                            ucMsg.ShowInfo(Resources.Messages.NoRecord);
                            return;
                        }
                        else
                        {
                            /*#CC13*/// objRD.headerReplacement(ds.Tables[0]);
                            PageBase.ExportToExecl(ds, "StockRetailer");//#CC06 added
                        }
                    }
                    else
                    {
                        // intResult = objRD.GetStockReportExcelbybcp();

                        ds = objRD.GetStockReportCommon();
                        intResult = 0;
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //DataTable dt;
                            /*#CC13*/// objRD.headerReplacement(ds.Tables[0]);
                            PageBase.ExportToExecl(ds, "StockReport");//#CC06 added
                        }
                        else
                            ucMsg.ShowInfo(Resources.Messages.NoRecord);

                    }
                }
                //if (objRD.Result == 0 /*#CC01 ADDED START*/ && intResult ==0 /*#CC01 ADDED END*/)
                //{
                //    //Response.Redirect((siteURL + strBCPFilePath + objRD.FilePath)); #CC06 comented
                //    //string filePath = "../../" + PageBase.strGlobalDownloadExcelPathRoot + path + "/" + strExportFileName;
                //    //HttpContext.Current.Response.Clear();
                //    //HttpContext.Current.Response.Charset = "";
                //    //HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=StockReport" + strExportFileName);
                //    //HttpContext.Current.Response.ContentType = "application/vnd.csv";
                //    //PageBase.ClearBuffer();
                //    //HttpContext.Current.Response.WriteFile(filePath);
                //    //HttpContext.Current.Response.End();
                //}
                //else if (intResult == 1 || objRD.Result==1)
                //{
                //    ucMsg.ShowInfo(Resources.Messages.NoRecord);

                //}
                //else
                //{
                //    ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);

                //}
            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }
    //private void fncExportToExcel(DataSet DS)
    //{

    //    if (DS.Tables[0].Rows.Count > 0)
    //    {
    //        DataTable DsCopy = new DataTable();
    //        DsCopy = DS.Tables[0];
    //        DS.Tables.Clear();
    //        DsCopy.Columns["HO"].ColumnName = Resources.SalesHierarchy.HierarchyName1;

    //        DsCopy.Columns["RBH"].ColumnName = Resources.SalesHierarchy.HierarchyName2;
    //        DsCopy.Columns["ZBH"].ColumnName = Resources.SalesHierarchy.HierarchyName3;
    //        DsCopy.Columns["SBH"].ColumnName = Resources.SalesHierarchy.HierarchyName4;
    //        DsCopy.Columns["ASO"].ColumnName = Resources.SalesHierarchy.HierarchyName5;
    //        DS.Tables.Add(DsCopy);
    //        String FilePath = Server.MapPath("../../");
    //        string FilenameToexport = "StockReport";
    //        PageBase.RootFilePath = FilePath;
    //        PageBase.ExportToExeclUsingOPENXMLV2(DS.Tables[0], FilenameToexport);

    //    }
    //}
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
            /*#CC14 Commented*/
            objproduct.ModelProdCatId = Convert.ToInt32(ddlProductCategory.SelectedValue);/*#CC14 Added*/
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
            objproduct.CompanyId = PageBase.ClientId;/* #CC15 Added */
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
            obj.CompanyId = PageBase.ClientId;/* #CC15 Added */
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
            obj.CompanyId = PageBase.ClientId;/* #CC15 Added */
            DataTable dt = obj.GetOrgHierarchy();
            String[] colArray = { "OrgnhierarchyID", "LocationName" };
            PageBase.DropdownBinding(ref ddllocation, dt, colArray);
        }
    }

    /*#CC08 start*/ protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
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
                 DdlSaleschannel.Items.Clear();/*#CC12 Added*/
                 PageBase.DropdownBinding(ref DdlSaleschannel, ObjSalesChannel.GetSalesChannelListForPivotandStock(), str);/*#CC12 Uncomment*/
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
     }/*#CC08 end*/
    /*#CC14 Added Started*/
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
     
    /*#CC14 Added End*/
}
