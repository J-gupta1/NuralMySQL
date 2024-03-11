using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;
using ExportExcelOpenXML;
using System.Text;
using BusinessLogics;
using System.Collections;
using System.Data.SqlClient;

public partial class Masters_SalesChannel_ManageSalesChannelProductCategoryUpload :PageBase //System.Web.UI.Page
{
    DataTable dtNew = new DataTable();
    object objSum;
    int counter = 0;
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    UploadFile UploadFile = new UploadFile();
    List<String> lstDuplicate = new List<String>();
    DataSet dsErrorProne = new DataSet();
    Int32 CallingMode;
    DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        try
        {
            ucMsg.ShowControl = false;
            if (!IsPostBack)
            {
                FillState();
                FillBrand();
                FillSalesChannelType();
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());

        }
    }

    void FillBrand()
    {
        using (ProductData ObjProduct = new ProductData())
        {
            ObjProduct.SearchType = EnumData.eSearchConditions.Active;
            String[] StrCol = new String[] { "ProductCategoryID", "ProductCategoryName" };
            PageBase.DropdownBinding(ref ddlProductCategory, ObjProduct.GetAllProductCategoryByParameters(), StrCol);

        };
    }
    void FillState()
    {
        using (GeographyData ObjState = new GeographyData())
        {
            ObjState.SearchCondition = EnumData.eSearchConditions.Active;
            String[] StrCol = new String[] { "StateID", "StateName" };
            PageBase.DropdownBinding(ref ddlState, ObjState.GetAllStateByParameters(), StrCol);
            PageBase.DropdownBinding(ref ddlRefState, ObjState.GetAllStateByParameters(), StrCol);

        };
    }
    void FillSalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.SalesChannelTypeID = -1;
            String[] StrCol = new String[] { "SalesChannelTypeID", "SalesChannelTypeName" };
            PageBase.DropdownBinding(ref ddlSalesChannelType, ObjSalesChannel.GetSalesChannelTypeForBrand(), StrCol);
            PageBase.DropdownBinding(ref ddlRefSaleschanneltype, ObjSalesChannel.GetSalesChannelTypeForBrand(), StrCol);

        };
    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
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
    protected void DwnldReferenceCodeTemplate_Click(object sender, EventArgs e)
    {


        try
        {
 
            DataSet dsReferenceCode = new DataSet();
            DataSet Ds = new DataSet();
            /*using (SalesChannelData objSalesData = new SalesChannelData())*/
            if (Convert.ToInt32(ddlRefSaleschanneltype.SelectedValue)==0)
            {
                ucMsg.ShowError("Please select saleschannel type");
            }
            if (Convert.ToInt32(ddlRefSaleschanneltype.SelectedValue) > 0)
            {
                using (SalesChannelData objSalesData = new SalesChannelData())
                {
                    objSalesData.SalesChannelTypeID = Convert.ToInt16(ddlRefSaleschanneltype.SelectedValue);
                    objSalesData.SalesChannelID = PageBase.SalesChanelID;
                    objSalesData.StateID = Convert.ToInt16(ddlRefState.SelectedValue);
                    objSalesData.CityID = Convert.ToInt16(ddlRefCity.SelectedValue);





                    dsReferenceCode = objSalesData.GetSaleschannelProductCategoryReferencecode();

                    
                    if (dsReferenceCode != null && dsReferenceCode.Tables[0].Rows.Count > 0)
                    {

                        String FilePath = Server.MapPath("~/");
                        string FilenameToexport = "Reference Code List";
                        PageBase.RootFilePath = FilePath;
                       
                        string[] strExcelSheetName = { "SalesChannel", "ProductCategory"};
                        

                        ZedService.Utility.ZedServiceUtil.ExportToExecl(dsReferenceCode, FilenameToexport, 2, strExcelSheetName);
                      
                    }
                    else
                    {
                        ucMsg.ShowInfo(Resources.Messages.NoRecord);
                    }



                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        grdSalesChannelList.PageIndex = 0;
        if (Convert.ToInt32(ddlSalesChannelType.SelectedValue)==0)
        { 
            ucMsg.ShowError("Please select saleschannel type");
            return;
        }
        if (Convert.ToInt32(ddlProductCategory.SelectedValue) == 0)
        {
            ucMsg.ShowError("Please select Product Category");
            return;
        }
        FillGrid();
    }
        void FillGrid()
            {

        DataTable Dt = new DataTable();
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            if (ddlSalesChannelType.SelectedValue != "0")
            {
                ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(ddlSalesChannelType.SelectedValue);
            }
            ObjSalesChannel.ProductCategoryId = Convert.ToInt32(ddlProductCategory.SelectedValue);
            ObjSalesChannel.StateID = Convert.ToInt16(ddlState.SelectedValue);
            ObjSalesChannel.CityID = Convert.ToInt16(ddlCity.SelectedValue);
           
            Dt = ObjSalesChannel.GetSalesChannelInfoForProductCategory();
        };
        if (Dt != null && Dt.Rows.Count > 0)
        {
            grdSalesChannelList.Visible = true;
            grdSalesChannelList.DataSource = Dt;
            grdSalesChannelList.DataBind();
            pnlHide.Visible = true;
            btnExprtToExcel.Visible = true;

        }
        else
        {
            grdSalesChannelList.Visible = false;
            grdSalesChannelList.DataSource = null;
            grdSalesChannelList.DataBind();
            ucMsg.ShowInfo(Resources.Messages.NoRecord);
            pnlHide.Visible = false;
            btnExprtToExcel.Visible = false;

        }
    }

        protected void btnUpload_Click(object sender, EventArgs e)
        {


            try
            {
                ClearForm();
                DataSet objDS = new DataSet();
                byte isSuccess = 1;
                Int16 UploadCheck = 0;
                String RootPath = Server.MapPath("../../");
                UploadFile.RootFolerPath = RootPath;
              
                UploadCheck = UploadFile.IsExcelFile(FileSalesChannelProductMappingUpload, ref strUploadedFileName);
                ViewState["TobeUploaded"] = strUploadedFileName;
                if (UploadCheck == 1)
                {
                    UploadFile.UploadedFileName = strUploadedFileName;
                    UploadFile.UploadValidationType = EnumData.eUploadExcelValidationType.eRetailerUpload;

                    isSuccess = UploadFile.uploadValidExcelRetailer(ref objDS, "SalesChProductMappUpload");



                    switch (isSuccess)
                    {
                        case 0:
                            ucMsg.ShowInfo("Invalid template kindly download template again and then upload.");
                            break;
                        case 2:
                            hlnkInvalid.Visible = true;
                            string strFileName = "InvalidData" + DateTime.Now.Ticks;

                            ucMsg.ShowInfo("There is error in data. Please refer Invalid Data link. No Data processed.");
                            DataView dvError = objDS.Tables[0].DefaultView;
                            dvError.RowFilter = "Error<>''";
                            DataTable dtError = dvError.ToTable();
                            DataSet dsError = new DataSet();
                            dsError.Tables.Add(dtError);
                            ExportInExcel(dsError, strFileName);

                            /*ExportInExcel(objDS, strFileName);*/
                            hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                            hlnkInvalid.Text = "Invalid Data";
                            break;
                        case 1:
                            InsertData(objDS.Tables[0]);
                            break;
                    }

                }
                else if (UploadCheck == 2)
                {
                    ucMsg.ShowInfo(Resources.Messages.UploadXlxs);

                }
                else if (UploadCheck == 3)
                {
                    ucMsg.ShowInfo(Resources.Messages.SelectFile);


                }

            }
            catch (Exception ex)
            {

                ucMsg.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);

            }
        }
        void ClearForm()
        {

            ucMsg.Visible = false;
            hlnkInvalid.Visible = false;

        }
        protected void btnCancel_Click1(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ManageSalesChannelProductCategoryUpload.aspx", false);
            }
            catch (Exception ex)
            {

            }
        }

        private void ExportInExcel(DataSet DsExport, string strFileName)
        {
            try
            {
                if (DsExport != null && DsExport.Tables.Count > 0)
                {
                    PageBase.ExportToExeclV2(DsExport, strFileName, DsExport.Tables.Count);
                }
            }
            catch (Exception ex)
            {
                ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }
        }
        protected void grdSalesChannelList_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void grdSalesChannelList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdSalesChannelList.PageIndex = e.NewPageIndex;
            FillGrid();

        }
        //protected void grdSalesChannelList_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            GridViewRow GVR = e.Row;
        //            Label lblBrandId = (Label)GVR.FindControl("lblBrandId");
        //            Label lblStatus = (Label)GVR.FindControl("lblStatus");
        //            CheckBox chkBrand = (CheckBox)GVR.FindControl("chkBxSelect");
        //            if (lblStatus.Text.ToLower() != "false")
        //            {
        //                chkBrand.Checked = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        //    }
        //}

        private void InsertData(DataTable DtBulkUpload)
        {
            try
            {
                if ((CreatedBcpData(DtBulkUpload)) == true)
                {

                    SalesChannelData objSales = new SalesChannelData();
                    objSales.SessionID = Convert.ToString(DtBulkUpload.Rows[0]["SessionID"]);
                   
                 
                    //objSales.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
                    objSales.SalesChannelID = PageBase.SalesChanelID;
                   
                   
                    objSales.UserID = Convert.ToInt32(PageBase.UserId);
                    objSales.SalesChannelTypeID = Convert.ToInt16(ddlRefSaleschanneltype.SelectedValue);
                    DataSet dsResult = new DataSet();
                    dsResult = objSales.SaveSalesChannelProductCategoryMapping();

                    int result = objSales.intOutParam;
                    if (result == 0)
                    {
                        ucMsg.ShowSuccess("Data uploaded successfully.");
                      

                    }
                    else if (result == 1 && dsResult != null && dsResult.Tables.Count > 0)
                    {
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            hlnkInvalid.Visible = true;
                            string strFileName = "InvalidData" + DateTime.Now.Ticks;
                            ucMsg.ShowInfo("There is error in data. Please refer Invalid Data link. No Data processed.");
                            ExportInExcel(dsResult, strFileName);
                            hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                            hlnkInvalid.Text = "Invalid Data";
                            //ucMsg.XmlErrorSource = objSales.XMLList;
                        }
                    }
                    else
                    {
                        //ucMsg.ShowError(objSales.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                ucMsg.ShowError(ex.ToString());
            }

        }

        public bool CreatedBcpData(DataTable dtUpload)
        {
            bool result = false;
            try
            {
                string guid = Guid.NewGuid().ToString();
               
                dtUpload.Columns.Add(AddColumn(guid, "SessionID", typeof(System.String)));
                dtUpload.Columns.Add(AddColumn(Convert.ToString(PageBase.UserId), "CreatedBy", typeof(int)));
               dtUpload.AcceptChanges();
                int i = PageBase.UserId;

                if (UploadCurrentOutStandingBcp(dtUpload) == true)
                {
                    /*ucMsg.ShowSuccess("BCP done sucessfully");*/
                    result = true;
                }
                /*else
                {
                    ucMsg.ShowError("Error while doing BCP");
                }*/
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }

        }

        public bool UploadCurrentOutStandingBcp(DataTable dtUpload)
        {
            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
                {
                    bulkCopy.BatchSize = 20000;
                    bulkCopy.DestinationTableName = "BulkUploadSCPCMapping";
                    bulkCopy.ColumnMappings.Add("SalesChannelCode", "SalesChannelCode");
                    bulkCopy.ColumnMappings.Add("ProductCategoryCode", "ProductCategoryCode");
                    bulkCopy.ColumnMappings.Add("Status", "Status");
                     bulkCopy.ColumnMappings.Add("SessionID", "SessionID");
                    bulkCopy.ColumnMappings.Add("CreatedBy", "CreatedBy");
                    bulkCopy.WriteToServer(dtUpload);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected void btnExprtToExcel_Click(object sender, EventArgs e)
        {
            try
             {
            dt = new DataTable();

            using (SalesChannelData ObjSalesChannel = new SalesChannelData())
                {
            if (ddlSalesChannelType.SelectedValue != "0")
            {
                ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(ddlSalesChannelType.SelectedValue);
            }
            ObjSalesChannel.ProductCategoryId = Convert.ToInt32(ddlProductCategory.SelectedValue);
            ObjSalesChannel.StateID = Convert.ToInt16(ddlState.SelectedValue);
            ObjSalesChannel.CityID = Convert.ToInt16(ddlCity.SelectedValue);
           
            dt = ObjSalesChannel.GetSalesChannelInfoForProductCategory();
                 };
           
                if (dt.Rows.Count > 0)
                     {


                DataSet dtcopy = new DataSet();

                dtcopy.Merge(dt);
                //dtcopy.Merge(Ds.Tables[1]);
                dtcopy.AcceptChanges();
                dtcopy.Tables[0].AcceptChanges();
                //dtcopy.Tables[1].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "SalesChannelProductCategoryMappingDetails";
                PageBase.RootFilePath = FilePath;
                //string[] strExcelSheetName = { "Detail", "Total" };
                //PageBase.ExportToExecl(dtcopy, FilenameToexport);
                ZedService.Utility.ZedServiceUtil.ExportToExecl(dtcopy, FilenameToexport);
               



                     }
                else
                {
                    ucMsg.ShowError(Resources.Messages.NoRecord);

                }
               
           
            }
      

                catch (Exception ex)
                {
                    ucMsg.ShowError(ex.Message.ToString());
                }
            }
        protected void ddlRefState_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (GeographyData ObjGeography = new GeographyData())
                {
                    if (ddlRefState.SelectedIndex > 0)
                    {
                        ObjGeography.SearchCondition = EnumData.eSearchConditions.Active;
                        ObjGeography.StateId = Convert.ToInt16(ddlRefState.SelectedValue);
                        String[] StrCol = new String[] { "CityId", "CityName" };
                        PageBase.DropdownBinding(ref ddlRefCity, ObjGeography.GetAllCityByParameters(), StrCol);
                    }
                    else if (ddlRefState.SelectedIndex == 0)
                    {
                        ddlRefCity.Items.Clear();
                        ddlRefCity.Items.Insert(0, new ListItem("Select", "0"));
                    }

                };
            }
            catch (Exception ex)
            {

                ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }
        }
}
