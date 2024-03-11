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
 * ==============================================================================================================
 */

public partial class Reports_SalesChannel_DownloadSalesRptLastSale : PageBase
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
            BindModel();
            ddlSku.Items.Insert(0, new ListItem("Select", "0"));
            bindProductCategory();
            BindState();
            fillLocations();
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
                    objRD.SalesType = Convert.ToInt16(ddlSalesType.SelectedValue);
                    objRD.ModelId = Convert.ToInt32(ddlModelName.SelectedValue);
                    objRD.SkuId = Convert.ToInt32(ddlSku.SelectedValue);
                    objRD.stateid = Convert.ToInt32(ddlState.SelectedValue);
                    objRD.ProductCategtoryid = Convert.ToInt32(ddlProductCategory.SelectedValue);
                    objRD.OrgHierarchyId = Convert.ToInt32(ddllocation.SelectedValue);
                    objRD.intWantZeroQuantity = chkZeroQuantity.Checked ? Convert.ToInt16(1) : Convert.ToInt16(0);
                    //objRD.FilePath =PageBase.strBCPFilePath + strExportFileName;      Pankaj Dhingra
                    objRD.FilePath = "SalesReport"+strExportFileName;
                    objRD.WithOrWithoutSerialBatch =  Convert.ToInt16(2);
                    objRD.ComingFrom = 0;//excel
                    DataSet ds;
                    ds = objRD.GetFlatLastSalesReportCommonForAll();

                    //if (chkSB.Checked)
                    //    intResult = objRD.GetFlatSalesReportbybcpSB();
                    //else
                    //    intResult = objRD.GetFlatSalesReportbybcp();
                    if (ds.Tables.Count > 0)
                    {
                        //Response.Redirect((siteURL + strBCPFilePath + objRD.FilePath));
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            objRD.headerReplacement(ds.Tables[0]);
                            PageBase.ExportToExecl(ds, "UniqueSalesReport");
                        }
                        else
                        {
                            ucMsg.ShowInfo(Resources.Messages.NoRecord);

                        }
                        
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
            objproduct.ModelProdCatId = 0;
            objproduct.ModelSelectionMode = 1;
            DataTable dtmodelfil = objproduct.SelectModelInfo();
            String[] colArray1 = { "ModelID", "ModelName" };
            PageBase.DropdownBinding(ref ddlModelName, dtmodelfil, colArray1);


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
            obj.StateCountryid = 1;/*because there is no multi country concept is there*/
            dt = obj.SelectStateInfo();
            String[] colArray = { "StateID", "StateName" };
            PageBase.DropdownBinding(ref ddlState, dt, colArray);
            ddlState.Items.Insert(0, new ListItem("Select", "0"));
        }
    }
    protected void fillLocations()
    {
        using (OrgHierarchyData obj = new OrgHierarchyData()) 
        {
            obj.SalesChanelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
            obj.SearchMode = 3;
            obj.UserID= PageBase.UserId; /* #CC01 Added */
            DataTable dt = obj.GetOrgHierarchy();
            String[] colArray = { "OrgnhierarchyID", "LocationName" };
            PageBase.DropdownBinding(ref ddllocation, dt, colArray);
        }
    }
}
