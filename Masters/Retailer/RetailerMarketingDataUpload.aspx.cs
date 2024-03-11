using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;
using System.IO;
using System.Text.RegularExpressions;
using ZedService;
using System.Net;
using System.Data.SqlClient;
public partial class Masters_Retailer_RetailerMarketingDataUpload :  PageBase
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
        if (!IsPostBack)
        {
            ucDateTo.Date = PageBase.ToDate;
            ucDateFrom.Date = PageBase.Fromdate;
            BindZone();
        }
    }

    private void BindZone()
    {

        using (RetailerData objRetailer = new RetailerData())
        {
            try
            {
                DataTable dt = objRetailer.getZoneDetail();


                ddlRetailerZone.DataSource = dt;

                ddlRetailerZone.DataTextField = "ZoneName";
                ddlRetailerZone.DataValueField = "ZoneID";


                ddlRetailerZone.DataBind();
                ddlRetailerZone.Items.Insert(0, new ListItem("Select", "0"));

            }
            catch (Exception ex)
            {
                ucmassege1.ShowError(ex.Message.ToString());
            }
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

            UploadCheck = UploadFile.IsExcelFile(FileUploadRetailerMarketingData, ref strUploadedFileName);
            ViewState["TobeUploaded"] = strUploadedFileName;
            if (UploadCheck == 1)
            {
                UploadFile.UploadedFileName = strUploadedFileName;
                UploadFile.UploadValidationType = EnumData.eUploadExcelValidationType.eRetailerUpload;

                isSuccess = UploadFile.uploadValidExcelRetailer(ref objDS, "RetailerMarketingData");



                switch (isSuccess)
                {
                    case 0:
                        ucmassege1.ShowInfo("Invalid or Blank template kindly download template again or Upload with Correct Data.");
                        break;
                    case 2:
                        hlnkInvalid.Visible = true;
                        string strFileName = "InvalidData" + DateTime.Now.Ticks;

                        ucmassege1.ShowInfo("There is error in data. Please refer Invalid Data link. No Data processed.");
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
                        InsertData(objDS.Tables[0],FileUploadRetailerMarketingData.FileName,strUploadedFileName);
                        break;
                }

            }
            else if (UploadCheck == 2)
            {
                ucmassege1.ShowInfo(Resources.Messages.UploadXlxs);

            }
            else if (UploadCheck == 3)
            {
                ucmassege1.ShowInfo(Resources.Messages.SelectFile);


            }

        }
        catch (Exception ex)
        {

            ucmassege1.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);

        }
    }
    void ClearForm()
    {

        ucmassege1.Visible = false;
        hlnkInvalid.Visible = false;

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
            ucmassege1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    private void InsertData(DataTable DtBulkUpload,string OriginalFileName,string UniqueFileName)
    {
        try
        {
            if ((CreatedBcpData(DtBulkUpload)) == true)
            {

                RetailerData objRetailer = new RetailerData();
                objRetailer.SessionID = Convert.ToString(DtBulkUpload.Rows[0]["SessionID"]);
                objRetailer.UserID = Convert.ToInt32(PageBase.UserId);
                objRetailer.OriginalFileName = OriginalFileName;
                objRetailer.UniqueFileName = UniqueFileName;
                DataSet dsResult = new DataSet();
                dsResult = objRetailer.SaveRetailerMarketingData();

                int result = objRetailer.intOutParam;
                //int result = 0;
                if (result == 0)
                {
                    ucmassege1.ShowSuccess("Data uploaded successfully.");


                }
                else if (result == 1 && dsResult != null && dsResult.Tables.Count > 0)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        hlnkInvalid.Visible = true;
                        string strFileName = "InvalidData" + DateTime.Now.Ticks;
                        ucmassege1.ShowInfo("There is error in data. Please refer Invalid Data link. No Data processed.");
                        ExportInExcel(dsResult, strFileName);
                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                        hlnkInvalid.Text = "Invalid Data";
                        ////ucMsg.XmlErrorSource = objSales.XMLList;
                       // ucmassege1.XmlErrorSource = dsResult.GetXml();
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
            ucmassege1.ShowError(ex.ToString());
        }

    }

    public bool CreatedBcpData(DataTable dtUpload)
    {
        bool result = false;
        try
        {
            string guid = Guid.NewGuid().ToString();
            DateTime dt = DateTime.Now;

            string sDate = dt.ToShortDateString();
            dtUpload.Columns.Add(AddColumn(guid, "SessionID", typeof(System.String)));
            dtUpload.Columns.Add(AddColumn(Convert.ToString(PageBase.UserId), "CreatedBy", typeof(int)));
            dtUpload.Columns.Add(AddColumn(sDate, "CreatedOn", typeof(System.String)));
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
                bulkCopy.DestinationTableName = "RetailerMarketingDataBCP";
                bulkCopy.ColumnMappings.Add("RetailerCode", "RetailerCode");
                
                bulkCopy.ColumnMappings.Add("SignageAgency", "SignageAgency");
                bulkCopy.ColumnMappings.Add("BrandingDone", "BrandingDone");
                bulkCopy.ColumnMappings.Add("MediaACPSFT", "MediaACPSFT");
                bulkCopy.ColumnMappings.Add("MediaGsb", "MediaGsb");
                bulkCopy.ColumnMappings.Add("MediaNormalGsb", "MediaNormalGsb");

                bulkCopy.ColumnMappings.Add("MediaIsbSft", "MediaIsbSft");
                bulkCopy.ColumnMappings.Add("Vendor", "Vendor");
               // bulkCopy.ColumnMappings.Add("Sales", "Sales");
                bulkCopy.ColumnMappings.Add("Remarks", "Remarks");
               // bulkCopy.ColumnMappings.Add("CSA1", "CSA1");
                bulkCopy.ColumnMappings.Add("InstallationDate", "InstallationDate");
                bulkCopy.ColumnMappings.Add("Status", "Status");
                bulkCopy.ColumnMappings.Add("SessionID", "SessionId");
                bulkCopy.ColumnMappings.Add("CreatedBy", "CreatedBy");
                bulkCopy.ColumnMappings.Add("CreatedOn", "CreatedOn");
                bulkCopy.WriteToServer(dtUpload);
                return true;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    protected void grdRetailerMarketingData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdRetailerMarketingData.PageIndex = e.NewPageIndex;
        FillGrid();

    }
    protected void grdRetailerMarketingData_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        grdRetailerMarketingData.PageIndex = 0;
        //if (Convert.ToInt32(ddlRetailerZone.SelectedValue) == 0)
        //{
        //    ucmassege1.ShowError("Please select Retailer Zone");
        //    return;
        //}
        //if (Convert.ToInt32(ddlActiveInactive.SelectedValue) == 2)
        //{
        //    ucmassege1.ShowError("Please select Active-InActive Flag");
        //    return;
        //}
        FillGrid();
    }


    void FillGrid()
    {

        DataTable Dt = new DataTable();
        using (RetailerData ObjRetailer = new RetailerData())
        {
            if (ddlRetailerZone.SelectedValue != "0")
            {
                ObjRetailer.ZoneId = Convert.ToInt16(ddlRetailerZone.SelectedValue);
            }
            
            ObjRetailer.ActiveInActiveFlag = Convert.ToInt16(ddlActiveInactive.SelectedValue);

            //if (convert.toint32(ddlcsatype.selectedvalue) == 2)
            //{
            //    objretailer.csatype = 1;
            //}
            //else
            //{
                ObjRetailer.CsaType = Convert.ToInt16(ddlCSAType.SelectedValue);
            //}
            ObjRetailer.ToDate = ucDateTo.Date;
            ObjRetailer.FromDate = ucDateFrom.Date;
            ObjRetailer.UserID = PageBase.UserId;
            ObjRetailer.Flag = 0;
            Dt = ObjRetailer.GetRetailerMarketingData();
        };
        if (Dt != null && Dt.Rows.Count > 0)
        {
            grdRetailerMarketingData.Visible = true;
            grdRetailerMarketingData.DataSource = Dt;
            grdRetailerMarketingData.DataBind();
            pnlHide.Visible = true;
            btnExprtToExcel.Visible = true;
            ucmassege1.Visible = false;

        }
        else
        {
            grdRetailerMarketingData.Visible = false;
            grdRetailerMarketingData.DataSource = null;
            grdRetailerMarketingData.DataBind();
            ucmassege1.ShowInfo(Resources.Messages.NoRecord);
            pnlHide.Visible = false;
            btnExprtToExcel.Visible = false;

        }
    }
    protected void btnExprtToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable Dt = new DataTable();

            using (RetailerData ObjRetailer = new RetailerData())
            {
                if (ddlRetailerZone.SelectedValue != "0")
                {
                    ObjRetailer.ZoneId = Convert.ToInt16(ddlRetailerZone.SelectedValue);
                }
                ObjRetailer.ActiveInActiveFlag = Convert.ToInt16(ddlActiveInactive.SelectedValue);
                //if (convert.toint32(ddlcsatype.selectedvalue) == 2)
                //{
                //    objretailer.csatype = 1;
                //}
                //else
                //{
                ObjRetailer.CsaType = Convert.ToInt16(ddlCSAType.SelectedValue);
                //}
                ObjRetailer.ToDate = ucDateTo.Date;
                ObjRetailer.FromDate = ucDateFrom.Date;
                ObjRetailer.UserID = PageBase.UserId;
                ObjRetailer.Flag = 1;
                Dt = ObjRetailer.GetRetailerMarketingData();
            };

            if (Dt.Rows.Count > 0)
            {


                DataSet dtcopy = new DataSet();

                dtcopy.Merge(Dt);
                //dtcopy.Merge(Ds.Tables[1]);
                dtcopy.AcceptChanges();
                dtcopy.Tables[0].AcceptChanges();
                //dtcopy.Tables[1].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "RetailerMarketingData";
                PageBase.RootFilePath = FilePath;
                //string[] strExcelSheetName = { "Detail", "Total" };
                //PageBase.ExportToExecl(dtcopy, FilenameToexport);
                ZedService.Utility.ZedServiceUtil.ExportToExecl(dtcopy, FilenameToexport);




            }
            else
            {
                ucmassege1.ShowError(Resources.Messages.NoRecord);

            }


        }


        catch (Exception ex)
        {
            ucmassege1.ShowError(ex.Message.ToString());
        }
    }


    protected void DwnldReferenceCodeTemplate_Click(object sender, EventArgs e)
    {
        try
        {
          

            DataSet ds = new DataSet();
            DataSet Ds = new DataSet();
            using (RetailerData objRetailer = new RetailerData())
            {




                ds = objRetailer.GetRetailerReferenceCodeReferencecode();
            }

            if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {

                        String FilePath = Server.MapPath("../../../");
                        string FilenameToexport = "Reference Code List";
                        PageBase.RootFilePath = FilePath;

                        string[] strExcelSheetName = { "Retailer"};


                        ZedService.Utility.ZedServiceUtil.ExportToExecl(ds, FilenameToexport, 1, strExcelSheetName);

                    }
                    else
                    {
                        ucmassege1.ShowInfo(Resources.Messages.NoRecord);
                    }



                }
            
        
        catch (Exception ex)
        {
            ucmassege1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            grdRetailerMarketingData.DataSource = null;
            Response.Redirect("RetailerMarketingDataUpload.aspx", false);

        }
        catch (Exception ex)
        {

        }
    }
}