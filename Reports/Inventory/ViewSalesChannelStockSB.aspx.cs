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
using ZedService;
using ActiveXLS;

/*
 * 06-Aug-2015,Karam Chand Sharma,#CC01, Implement salechannle type as us and give provission to type sale channel in textbox
 * 06-Aug-2015,Balram Jha,#CC02, hide search button and disable viewstate for grid to improve server performance
 * * 05-April-2018,Rajnish kumar,#CC03, Model Drop Down In filter
 *  3-July-2018, Rakesh Raj, #CC04, Export to CSV Feature Added
 *  31-July-2018,Vijay Kumar Prajapati,#CC05, Display Message in Exportto Excel button.
 */
public partial class Reports_Inventory_ViewSalesChannelStockSB : PageBase
{
    DataTable DtSalesChannelDetail = new DataTable();
    DataTable dtNew;
    string ExportFileLocation = HttpContext.Current.Server.MapPath("~") + "/Excel/Download/";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {   
            ucMessage1.ShowControl = false;
            Server.ScriptTimeout = 2000; //#CC04
            if (!IsPostBack)
            {
                //#CC02 start
                grdvResult.EnableViewState = false;
                btnSearch.Visible = false;
                //#CC02 end
                BindModel();/*#CC03*/
                FillSalesChannelType();
                if (PageBase.BaseEntityTypeID == 3)
                {
                    cmbsaleschanneltype.SelectedValue = "101";
                }
                else
                {
                    cmbsaleschanneltype.SelectedValue = Convert.ToString(PageBase.SalesChanelTypeID);
                }

                cmbsaleschanneltype.Enabled = (PageBase.SalesChanelID > 0 || PageBase.BaseEntityTypeID == 3) ? false : true;

                ddlSalesChannelName.Items.Insert(0, new ListItem("Select", "0"));
                if (PageBase.SalesChanelID > 0 || PageBase.BaseEntityTypeID == 3)
                {
                    cmbsaleschanneltype_SelectedIndexChanged(cmbsaleschanneltype, null);
                    ddlSalesChannelName.SelectedValue = PageBase.BaseEntityTypeID == 3 ? Convert.ToString(ViewState["LoggedIn"]) : Convert.ToString(PageBase.SalesChanelID);
                    ddlSalesChannelName.Enabled = false;

                }

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }





    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            //if (cmbsaleschanneltype.SelectedValue != "0")
            //{
            //    if (ddlSalesChannelName.SelectedValue == "0")
            //    {
            //        ucMessage1.ShowInfo("Please Select any SalesChannel Name");
            //        return;
            //    }
            //}
            BindGrid(1,false,2);
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

        ucMessage1.ShowControl = false;
        cmbsaleschanneltype.Enabled = (PageBase.SalesChanelID > 0 || PageBase.BaseEntityTypeID == 3) ? false : true;
        ddlSalesChannelName.Enabled = (PageBase.SalesChanelID > 0 || PageBase.BaseEntityTypeID == 3) ? false : true;
        if (PageBase.SalesChanelID == 0 && PageBase.BaseEntityTypeID != 3)
        {
            cmbsaleschanneltype.SelectedIndex = 0;
            ddlSalesChannelName.Items.Clear();
            ddlSalesChannelName.Items.Insert(0, new ListItem("Select", "0"));

        }
        ddlStockStatus.SelectedIndex = 0;
        ddlStockBinType.Items.Clear();
        ddlStockBinType.Items.Insert(0, new ListItem("Select", "0"));
        txtSerialNo.Text = "";
        txtSkuCode.Text = "";
        txtSkuName.Text = "";
        dvDetail.Visible = false;
        dvhide.Visible = false;
        updDetail.Update();

    }
    protected void ExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            BindGrid(2, false,2);
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }

    //#CC04
    //protected void ExportToCSV_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        BindGrid(2, true);
    //    }
    //    catch (Exception ex)
    //    {
    //        ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
    //    }

    //}

    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            if (cmbsaleschanneltype.SelectedValue != "0")
            {
                if (ddlSalesChannelName.SelectedValue == "0")
                {
                    ucMessage1.ShowInfo("Please Select any SalesChannel Name");
                    return;
                }
            }
            ddlStockBinType.SelectedValue = "0";
            ddlStockStatus.SelectedValue = "0";
            txtSkuName.Text = "";
            txtSkuCode.Text = "";
            if (PageBase.SalesChanelID == 0 && PageBase.BaseEntityTypeID != 3)
            {
                cmbsaleschanneltype.SelectedValue = "0";
                ddlSalesChannelName.SelectedValue = "0";
            }
            BindGrid(1, false,2);
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }

    protected void cmbsaleschanneltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearPreviouseInformation();
        BindSalesChannel();
    }
    protected void ddlStockStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlStockStatus.SelectedValue == "0")
        {
            ddlStockBinType.Items.Clear();
            ddlStockBinType.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            BindStockBinType(Convert.ToInt16(ddlStockStatus.SelectedValue));
        }
    }
    private void BindStockBinType(Int16 StockStatusID)
    {
        try
        {
            using (SalesChannelData binType = new SalesChannelData())
            {
                ddlStockBinType.Items.Clear();
                binType.StockStatusID = StockStatusID;
                string[] str = { "StockBinTypeMasterID", "StockBinTypeDesc" };
                PageBase.DropdownBinding(ref ddlStockBinType, binType.SelectBinTypeByStockStatusId(), str);
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    protected void grdvResult_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToLower() == "bindserialdetail")
            {
                LinkButton imgbtnPayment = (LinkButton)e.CommandSource;
                GridViewRow grdrow = (GridViewRow)imgbtnPayment.NamingContainer;
                grdvResult.SelectedIndex = grdrow.RowIndex;

                Int32 intIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow grow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                Int32 intSkuID = Convert.ToInt32(grdvResult.DataKeys[grow.RowIndex]["SkuID"]);
                Int32 intSaleschannelID = Convert.ToInt32(grdvResult.DataKeys[grow.RowIndex]["Saleschannelid"]);
                Int16 intStockStatusID = Convert.ToInt16(grdvResult.DataKeys[grow.RowIndex]["StockStatusID"]);
                Int32 intStockbinType = Convert.ToInt32(grdvResult.DataKeys[grow.RowIndex]["StockBinTypeMasterID"]);
                Int16 intSalesChannelTypeID = Convert.ToInt16(grdvResult.DataKeys[grow.RowIndex]["SaleschanneltypeID"]);

                ViewState["SkuID"] = intSkuID;
                ViewState["SaleschannelID"] = intSaleschannelID;
                ViewState["StockBinTypeMasterID"] = intStockbinType;
                ViewState["StockStatusID"] = intStockStatusID;
                ViewState["SalesChannelTypeID"] = intSalesChannelTypeID;
                bindSerialGrid(intSkuID, intSaleschannelID, intStockbinType, intStockStatusID, intSalesChannelTypeID);
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowWarning(ex.Message);

        }
    }
    void bindSerialGrid(Int32 intSkuID, Int32 intSalesChannelid, Int32 intStockBinType, Int16 intStockStatus, Int16 SaleschannelTypeID)
    {
        try
        {
            using (SalesChannelData objSales = new SalesChannelData())
            {
                objSales.SkuID = intSkuID;
                objSales.SalesChannelID = intSalesChannelid;
                objSales.StockBinTypeMasterID = intStockBinType;
                objSales.StockStatusID = intStockStatus;
                objSales.PageIndex = 0;
                objSales.PageSize = 0;
                objSales.SalesChannelTypeID = SaleschannelTypeID;
                objSales.BaseEntityTypeID = Convert.ToInt16(PageBase.BaseEntityTypeID);
                DataTable dt = objSales.CurrentSalesChannel_DetailSB();
                if (dt.Rows[0]["SerialisedMode"].ToString() == "3")
                    GrdSerialDetail.Columns[5].Visible = false;
                GrdSerialDetail.DataSource = (dt == null || dt.Rows.Count == 0 ? null : dt);
                GrdSerialDetail.DataBind();
                GrdSerialDetail.Visible = true;
                dvDetail.Visible = true;
                updDetail.Update();

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    protected void grdvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkl = (LinkButton)e.Row.FindControl("lnkSerial");
                Label lbStatus = (Label)e.Row.FindControl("Label1");
                if (lbStatus.Text == "1")
                {
                    lnkl.Visible = false;
                }
                if (lbStatus.Text == "3")
                {
                    lnkl.Text = "View Serial";
                }

                if (lbStatus.Text == "2")
                {
                    lnkl.Text = "View Batch";
                }

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowInfo("Pankaj");
        }
    }
    void BindSalesChannel()
    {
        try
        {
            ddlSalesChannelName.Items.Clear();
            if (cmbsaleschanneltype.SelectedValue == "0")
            {
                ddlSalesChannelName.Items.Clear();
                ddlSalesChannelName.Items.Insert(0, new ListItem("Select", "0"));
                return;
            }
            using (SalesChannelData ObjSalesChannel = new SalesChannelData())
            {
                ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(cmbsaleschanneltype.SelectedValue);
                ObjSalesChannel.ActiveStatus = 255;
                ObjSalesChannel.BaseEntityTypeID = Convert.ToInt16(PageBase.BaseEntityTypeID);
                ObjSalesChannel.UserID = PageBase.UserId;
                string[] str = { "SalesChannelid", "SalesChannelName" };
                PageBase.DropdownBinding(ref ddlSalesChannelName, ObjSalesChannel.GetSalesChannelListWithRetailer(), str);
                ViewState["LoggedIn"] = ObjSalesChannel.LoggedInSalesChannelID;
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    void FillSalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            DataTable dt = new DataTable();
            String[] StrCol = new String[] { "EntityTypeID", "EntityType" };
            ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16( PageBase.EntityTypeID);
            ObjSalesChannel.BaseEntityTypeID = Convert.ToInt16(PageBase.BaseEntityTypeID);
            dt = ObjSalesChannel.FetchSalesChannelTypeSB();
            PageBase.DropdownBinding(ref cmbsaleschanneltype, dt, StrCol);
            //ViewState["SalesType"] = dt;


        };
    }
    /*#CC03 start*/
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
    }  /*#CC03 End*/
    void BindGrid(Int16 Value, bool IsCSV,int RequestType)
    {

        try
        {
            dvDetail.Visible = false;
            updDetail.Update();
            using (SalesChannelData ObjStock = new SalesChannelData())
            {
                //dtNew = new DataTable();
                
                ObjStock.SerialNumber = txtSerialNo.Text;
                ObjStock.StockStatusID = Convert.ToInt16(ddlStockStatus.SelectedValue);
                ObjStock.StockBinTypeMasterID = Convert.ToInt16(ddlStockBinType.SelectedValue);
                ObjStock.SkuName = txtSkuName.Text;
                ObjStock.SkuCode = txtSkuCode.Text;
                ObjStock.SalesChannelTypeID = Convert.ToInt16(cmbsaleschanneltype.SelectedValue);
                ObjStock.SalesChannelTypeID = Convert.ToInt16(ddlEntityType.SelectedValue);
                /*ObjStock.SalesChannelID = Convert.ToInt32(ddlSalesChannelName.SelectedValue);#CC01 COMMENTED*/
                ObjStock.SalesChannelID = Convert.ToInt32(hdnAdjustmentForSalesChannelid.Text);/*#CC01 ADDED*/
                ObjStock.PageIndex = 0; //Will not be used any where for future aspect
                ObjStock.PageSize = 0;//Will not be used any where for future aspect
                ObjStock.UserID = PageBase.UserId;
                ObjStock.Condition = Value;
                ObjStock.BaseEntityTypeID = Convert.ToInt16(PageBase.BaseEntityTypeID);
                ObjStock.SalesChannelCode = txtSalesChannelName.Text.Trim();
                //ObjStock.SalesChannelTypeID = Convert.ToInt16(cmbsaleschanneltype.SelectedValue);
                ObjStock.ModelId = Convert.ToInt32(ddlModelName.SelectedValue);/*#CC03*/
                ObjStock.RequestType = RequestType;
                DataSet dsNew = new DataSet();
                dsNew = ObjStock.CurrentSalesChannelStockStatus();
                /*#CC05 Added Started*/
                if (ObjStock.Error != "" && ObjStock.Error!=null)
                {
                    ucMessage1.ShowSuccess(ObjStock.Error);
                    return;
                }
                /*#CC05 Added End*/
                if (Value == 1)
                {
                    if (dsNew.Tables[0].Rows.Count > 0)
                        ExportToExcel.Visible = true;
                    else
                        ExportToExcel.Visible = false;
                    grdvResult.DataSource = dsNew.Tables[0];
                    grdvResult.DataBind();
                    GrdSerialDetail.Visible = false;
                    dvhide.Visible = true;
                }
                //#CC04
                else if (IsCSV == true)
                {
                    if (dsNew.Tables.Count > 0 && dsNew.Tables[0].Rows.Count > 0)
                    {      
                        string FilenameToexport = "SalesChannelStock";
                        dsNew.Tables[0].ToCSV(FilenameToexport, ExportFileLocation);
                        
                    }
                    else
                    {
                        ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                    }
                    
                }
                else
                {
                    if (dsNew.Tables[0].Rows.Count > 0)
                    {
                        //DataSet dtcopy = new DataSet();
                        //dtcopy.Merge(dtNew);
                        //dtcopy.Tables[0].AcceptChanges();
                        String FilePath = Server.MapPath("../../");
                        string FilenameToexport = "SalesChannelStock";
                        PageBase.RootFilePath = FilePath;
                        //PageBase.ExportToExecl(dsNew, FilenameToexport);
                        int tbCount = dsNew.Tables.Count;
                        string[] Sheetname = new string[tbCount];// { "Sheet1", "Sheet2" };
                        for (int a = 0; a < tbCount;a++ )
                        {
                            Sheetname[a] = "Sheet" + (a + 1).ToString();
                        }
                            ZedService.Utility.ZedServiceUtil.ExportToExecl(dsNew, FilenameToexport, tbCount, Sheetname);
                       // FilenameToexport = FilenameToexport + System.DateTime.Now.Ticks + ".xlsb";
                        //string FilenameWithPath = HttpContext.Current.Server.MapPath("~") + "\\Excel\\Download\\" + FilenameToexport;
                        //hlnkInvalid.Visible = fncExportInExcelAcitveXLS(dsNew, FilenameWithPath);

                        //hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + FilenameToexport;
                        //hlnkInvalid.Text = "Download File";

                    }
                    else
                    {
                        ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    protected void grdvResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvResult.PageIndex = e.NewPageIndex;
        BindGrid(1, false,2);
    }
    protected void GrdSerialDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdSerialDetail.PageIndex = e.NewPageIndex;
        bindSerialGrid(Convert.ToInt32(ViewState["SkuID"]), Convert.ToInt32(ViewState["SaleschannelID"]), Convert.ToInt32(ViewState["StockBinTypeMasterID"]), Convert.ToInt16(ViewState["StockStatusID"]), Convert.ToInt16(ViewState["SalesChannelTypeID"]));
    }
    void ClearPreviouseInformation()
    {
        ucMessage1.ShowControl = false;
        cmbsaleschanneltype.Enabled = (PageBase.SalesChanelID > 0 || PageBase.BaseEntityTypeID == 1) ? false : true;
        ddlSalesChannelName.Enabled = PageBase.SalesChanelID > 0 ? false : true;
        if (PageBase.SalesChanelID == 0)
        {
            ddlSalesChannelName.Items.Clear();
            ddlSalesChannelName.Items.Insert(0, new ListItem("Select", "0"));
        }
        ddlStockStatus.SelectedIndex = 0;
        ddlStockBinType.Items.Clear();
        ddlStockBinType.Items.Insert(0, new ListItem("Select", "0"));
        dvDetail.Visible = false;
        dvhide.Visible = false;
        updDetail.Update();
    }
    public bool fncExportInExcelAcitveXLS(DataSet ds, string ReportName)
    {
        bool returnVal = false;
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ActiveXLS.ExcelDocument xlsdoc = new ActiveXLS.ExcelDocument(1);
                int sheetCount = 0;
                ExcelWorksheet xlsSheet = new ExcelWorksheet();
                ExcelStyle xlsEvenStyle = new ExcelStyle();


                xlsEvenStyle.setBackground(System.Drawing.Color.Khaki);
                xlsEvenStyle.setBold(true);

                ExcelStyle xlsOddStyle = new ExcelStyle();
                xlsOddStyle.setBackground(System.Drawing.Color.DarkKhaki);
                xlsOddStyle.setBold(true);

                ExcelStyle xlsHeadStyle = new ExcelStyle();
                xlsHeadStyle.setBackground(System.Drawing.Color.MistyRose);
                xlsHeadStyle.setBold(true);

                ExcelAutoFormat xlsAutoFormat = new ExcelAutoFormat();
                xlsAutoFormat.setHeaderRowStyle(xlsHeadStyle);

                //if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows.Count <= 1048500)
                //{
                //    sheetCount = xlsdoc.SheetCount();
                //    if (sheetCount > 1)
                //    {
                //        for (int i = 2; i <= sheetCount; i++)
                //        {
                //            xlsdoc.esd_removeSheet("Sheet" + i);
                //        }
                //    }
                //    xlsSheet = (ExcelWorksheet)xlsdoc.esd_getSheet("Sheet1");
                //    //   xlsSheet.esd_insertDataSet(ds, 0, 0, xlsAutoFormat, true);
                //}


                //Response.AppendHeader("content-disposition", "attachment; filename=" + ReportName + ".xls");
                //Response.ContentType = "application/octetstream";
                //Response.Clear();
                ExcelAutoFormat Ab = new ExcelAutoFormat();
                
                
                
                try
                {
                    xlsdoc.esd_removeSheet("Sheet1" );
                    xlsdoc.esd_WriteXLSBFile_FromDataSet(ReportName, ds, Ab, "StockSB");
                    returnVal = true;
                    
                }
                catch (Exception ex)
                {
                    //lblMessage.Text = ex.Message;
                    Response.ClearHeaders();
                    Response.ClearContent();
                }

                finally
                {
                    if (xlsdoc != null)
                    { xlsdoc.Dispose(); }
                }

            }
        }
        catch (Exception ex)
        {
            clsException.clsHandleException.fncHandleException(ex, "User Id- '" + Session["webuserid"] + "' User - '" + Session["person_name"] + "'");
            //  lblMessage.Text = ex.Message;
        }
        return returnVal;
    }

    protected void RequestForData_Click(object sender, EventArgs e)
    {
        try
        {
            BindGrid(2, false, 1);
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
}
