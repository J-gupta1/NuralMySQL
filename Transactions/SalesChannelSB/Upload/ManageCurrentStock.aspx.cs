﻿#region Copyright and page info
/*===================================================================
Copyright	: Zed-Axis Technologies, 2011
Author		:
Create date	:
Description	: 
=====================================================================
Review Log:
03-Jul-12, Rakesh Goel - Any validation on dataset should use Linq wherever possible for faster performance rather than row wise loop.
-----------------------------------------------------------------------------------------------------------------
Change Log:
03-Jul-12, Rakesh Goel , #CC01 - Batch number duplicacy validation is buggy. Commented the same.
05-May-15, Karam Chand Sharma , #CC02 - Remove update panal because message was not display
* 31-Jan-2018,Vijay Kumar Prajapati,#CC03- Add Bin Code Download template and bincode added in template.
* 05-Feb-2019,Balram Jha,#CC04- Corrected path for reading file
-----------------------------------------------------------------------------------------------------------------
*/
#endregion

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
using System.Collections.Generic;
using System.Linq;
using BusinessLogics;
using ZedService;
using System.Data.SqlClient;


public partial class ManageCurrentStock : PageBase
{
    protected string strSiteUrl = PageBase.siteURL;
    protected string strAssets = PageBase.strAssets;
    private bool IsOpeningdateEnable = false;
    string strUploadedFileName = string.Empty;
    int counter = 0;
    int Saleschannelid = 0;
    string Imei1 = "";
    UploadFile UploadFile = new UploadFile();
    DataSet dsErrorProne = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            GetvalidSession();
            Page.Header.DataBind();
            string strLogInUserName = Convert.ToString(Session["DisplayName"]) + "(" + Convert.ToString(Session["RoleName"]) + ")";
            
            ucMessage1.Visible = false;
            if (!IsPostBack)
            {
                
                pnlGrid.Visible = false;
            }
            
            FillDate();

        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            ucMessage1.ShowError(ex.Message, GlobalErrorDisplay());
        }


    }

   
    void FillDate()
    {
        
            ucDatePicker.Date = DateTime.Now.Date.ToString();
            ucDatePicker.TextBoxDate.Enabled = true;
            ucDatePicker.imgCal.Enabled = true;
            ucDatePicker.MaxRangeValue = DateTime.Now.Date;
            //ucDatePicker.RangeErrorMessage = "Only " + PageBase.BackDaysAllowedOpeningStock + " Back days allowed";
        
    }

    

    public bool BulkCopy(DataTable dtTempTable)
    {
        try
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "BulkCurrentStock";
                bulkCopy.ColumnMappings.Add("PreviousStock", "PreviousStock");
                bulkCopy.ColumnMappings.Add("CurrentStock", "CurrentStock");
                bulkCopy.ColumnMappings.Add("SKUCode", "SKUCode");
                bulkCopy.ColumnMappings.Add("SKUName", "SKUName");
                bulkCopy.ColumnMappings.Add("BrandName", "BrandName");
                bulkCopy.ColumnMappings.Add("ProductCategoryName", "ProductCategoryName");
                bulkCopy.ColumnMappings.Add("SalesChannelName", "SalesChannelName");
                bulkCopy.ColumnMappings.Add("SalesChannelCode", "SalesChannelCode");
                bulkCopy.ColumnMappings.Add("Address1", "Address1");
                bulkCopy.ColumnMappings.Add("SessionId", "SessionId");
                
                bulkCopy.WriteToServer(dtTempTable);
                return true;
            }

        }
        catch (Exception ex)
        {


            return false;
        }
    }
    private void SaveRecord(DataTable Dt)
    {
        try
        {
            string guid = Guid.NewGuid().ToString();
            Dt.Columns.Add(AddColumn(guid, "SessionId", typeof(System.String)));
            Dt.AcceptChanges();
            if (BulkCopy(Dt) == true)
            {
                using (SalesChannelData ObjSales = new SalesChannelData())
                {

                    ObjSales.Error = "";
                    ObjSales.SalesChannelID = PageBase.SalesChanelID;
                    ObjSales.SessionID = guid;
                    ObjSales.UserID = PageBase.UserId;
                    ObjSales.CompanyId = PageBase.ClientId;
                    ObjSales.OpeningStockDate = Convert.ToDateTime(ucDatePicker.Date);
                    int result = ObjSales.InsertCurrentStock();
                    ucMessage1.Visible = true;
                    if (ObjSales.XMLList != null && ObjSales.XMLList != string.Empty)
                    {
                        ucMessage1.XmlErrorSource = ObjSales.XMLList;
                        return;
                    }
                    else if (ObjSales.Error != null && ObjSales.Error != "")
                    {
                        ucMessage1.ShowError(ObjSales.Error);
                        return;
                    }
                    else if (result == 1)
                    {
                        ucMessage1.ShowError("Some Error has Occured while Processing Data");
                        return;
                    }
                    ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    
                    Clear();
                    /* #CC02 COMMENTED updgrid.Update();*/



                }
            }
            if (!pageValidate())
            {
                return;
            }
            
            
        }

        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }

    bool pageValidate()
    {
        if (ServerValidation.IsDate(ucDatePicker.Date, true) != 0)
        {
            ucMessage1.ShowInfo(Resources.Messages.MandatoryField);
            return false;
        }

        if (Convert.ToDateTime(ucDatePicker.Date) > DateTime.Now.Date)
        {
            ucMessage1.ShowInfo("Date should be less than or equal to current date.");
            return false;
        }

        return true;
    }


    void Clear()
    {
        pnlGrid.Visible = false;
        gvStockEntry.DataSource = null;
        gvStockEntry.DataBind();
        lblTotal.Text = "";
        /* #CC02 COMMENTED updgrid.Update();*/
        ViewState["Detail"] = null;
        

    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            HttpContext.Current.Session["PkeyColumns"] = null;
            string strKey = string.Empty;
            Clear();
            ucMessage1.Visible = false;
            hlnkInvalid.Visible = false;
            //hlnkDuplicate.Visible = false;
            //hlnkBlank.Visible = false;
            String RootPath = Server.MapPath("../../");
            UploadFile.RootFolerPath = RootPath;

            Int16 Upload = 0;
            Upload = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            ViewState["TobeUploaded"] = strUploadedFileName;
            if (Upload == 1)
            {

                OpenXMLExcel objexcel = new OpenXMLExcel();
                
                DataSet DsExcel = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);//#CC04 added
                if (DsExcel != null && DsExcel.Tables.Count > 0 && DsExcel.Tables[0].Rows.Count > 0)
                {
                    if (DsExcel.Tables[0].Rows.Count > Convert.ToInt32(PageBase.ValidExcelRows))
                        ucMessage1.ShowInfo("Limit Crossed");
                    else
                    {
                        ValidateUploadFile objValidateFile = new ValidateUploadFile();
                        {
                            DataSet objDS = DsExcel;
                            
                            SortedList objSL = new SortedList();
                            
                            objValidateFile.CompanyId = PageBase.ClientId;

                            objValidateFile.UploadedFileName = strUploadedFileName;
                            objValidateFile.ExcelFileNameInTable = "CurrentStock";

                            objValidateFile.PkColumnName = "";
                            objValidateFile.RootFolerPath = RootPath;
                            
                            objValidateFile.ValidateFileWithCompanyId(false, out objDS, out objSL);
                            //0 =Complete DataTable excluding blank and duplicate records                        
                            //1=Duplicate Records
                            //2=Blank Records

                            if (objValidateFile.Message != null && objValidateFile.Message.Trim() != "")
                                ucMessage1.ShowInfo(objValidateFile.Message);
                            else
                            {
                                ucMessage1.Visible = false;
                                bool blnIsUpload = true;
                                if (objSL.Count >= 1 && objSL.Keys.Count >= 1)
                                {
                                    objDS.Tables["DtExcelSheet"].Columns.Add(new DataColumn("ReasonForInvalid"));
                                    IDictionaryEnumerator objIDicEnum = objSL.GetEnumerator();
                                    while (objIDicEnum.MoveNext())
                                    {
                                        string[] strpkeyColumnName = Convert.ToString(HttpContext.Current.Session["PkeyColumns"]).Split(',');
                                        if (HttpContext.Current.Session["PkeyColumns"] != null)
                                        {
                                            for (int i = 0; i <= objDS.Tables["DtExcelSheet"].Rows.Count - 1; i++)
                                            {
                                                strKey = string.Empty;
                                                for (int j = 0; j <= strpkeyColumnName.Length - 1; j++)
                                                {
                                                    if (strKey == string.Empty)
                                                        strKey = objDS.Tables["DtExcelSheet"].Rows[i][strpkeyColumnName[j]].ToString();
                                                    else
                                                        strKey = strKey + objDS.Tables["DtExcelSheet"].Rows[i][strpkeyColumnName[j]].ToString();
                                                }
                                                if(strKey==objIDicEnum.Key.ToString())
                                                {
                                                    objDS.Tables["DtExcelSheet"].Rows[i]["ReasonForInvalid"] = objIDicEnum.Value.ToString();
                                                }
                                            }
                                        }
                                        //foreach (DataRow dr in objDS.Tables["DtExcelSheet"].Select(strPkColName + "='" + objIDicEnum.Key.ToString() + "'"))
                                        //{
                                        //    dr["ReasonForInvalid"] = objIDicEnum.Value.ToString();
                                        //}
                                    }

                                    objDS.Tables[0].AcceptChanges();
                                    if (objDS.Tables["DtExcelSheet"] != null && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                                    {
                                        hlnkInvalid.Visible = true;
                                        string strFileName = "invalidData" + DateTime.Now.Ticks;
                                        //ExportInExcel(objDS.Tables["DtExcelSheet"], strFileName);
                                        dsErrorProne.Merge(objDS.Tables["DtExcelSheet"]);
                                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                        hlnkInvalid.Text = "Invalid Data";

                                        blnIsUpload = false;
                                    }
                                    blnIsUpload = false;
                                }

                               
                                if (objDS.Tables["DtDuplicateRecord"] != null && objDS.Tables["DtDuplicateRecord"].Rows.Count > 0)
                                {
                                    
                                    dsErrorProne.Merge(objDS.Tables["DtDuplicateRecord"]);
                                    blnIsUpload = false;
                                }
                                if (objDS.Tables["DtBlankData"] != null && objDS.Tables["DtBlankData"].Rows.Count > 0)
                                {
                                    dsErrorProne.Merge(objDS.Tables["DtBlankData"]);
                                    blnIsUpload = false;
                                }
                                if (blnIsUpload)
                                {
                                    if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                                    {
                                        
                                        SaveRecord(objDS.Tables[0]);
                                    }
                                    else
                                     ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                                    
                                }
                                else
                                {
                                    hlnkInvalid.Visible = true;
                                    string strFileName = "InvalidData" + DateTime.Now.Ticks;
                                    ExportInExcel(dsErrorProne, strFileName);
                                    hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                    hlnkInvalid.Text = "Invalid Data";
                                }
                            }
                        }
                    }

                }
                else
                {
                    ucMessage1.ShowWarning(Resources.Messages.NoRecord);
                }
            }
            else if (Upload == 2)
            {
                ucMessage1.ShowInfo(Resources.Messages.UploadXlxs);
            }
            else if (Upload == 3)
            {
                ucMessage1.ShowInfo(Resources.Messages.SelectFile);
            }
            else
            {
                ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            }


        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
        }
    }
    private void ExportInExcel(DataSet DsError, string strFileName)
    {
        if (DsError != null && DsError.Tables.Count > 0)
        {
           PageBase.ExportToExeclV2(DsError, strFileName,DsError.Tables.Count);
        }
    }
    
    protected void DwnldReferenceCodeTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsReferenceCode = new DataSet();
            using (SalesChannelData objSalesData = new SalesChannelData())
            {

                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.eCurrentStock;
                objSalesData.Brand = PageBase.Brand;
                objSalesData.SalesChannelID = PageBase.SalesChanelID;
                objSalesData.UserID = PageBase.UserId;
                dsReferenceCode = objSalesData.GetAllTemplateData();
                
            
                if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "CurrentStock";
                    PageBase.RootFilePath = FilePath;
                    //string[] strExcelSheetName = { "Retailer", "SalesChannel", "SKU", "BinCode", "Batch" };
                    //ChangedExcelSheetNames(ref dsReferenceCode, strExcelSheetName, 5);
                    //if (dsReferenceCode.Tables.Count > 5)
                    //    dsReferenceCode.Tables.RemoveAt(5);
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsReferenceCode, FilenameToexport);
                }
                else
                {
                    ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());

        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ucMessage1.ShowControl = false;
        Clear();
    }
    bool PageValidatesave()
    {
        //if (cmbSalesChannel.SelectedValue == "0")
        //{
        //    ucMessage1.ShowInfo(Resources.Messages.MandatoryField);
        //    return false;
        //}
        if (ucDatePicker.Date == "")
        {
            ucMessage1.ShowInfo(Resources.Messages.MandatoryField);
            return false;
        }
        if (Convert.ToDateTime(ucDatePicker.Date) > System.DateTime.Now)
        {
            ucMessage1.ShowInfo(Resources.Messages.DateRangeValidation);
            return false;
        }

        return true;
    }

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }
            if (!pageValidate())
            {
                return;
            }

            using (SalesChannelData ObjSales = new SalesChannelData())
            {

                ObjSales.Error = "";
                ObjSales.SalesChannelID = PageBase.SalesChanelID;
                ObjSales.OpeningStockDate = Convert.ToDateTime(ucDatePicker.Date);
                int result = ObjSales.InsertOpeningStockWithZero();
                if (result == 0)
                {
                    ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    Session["DefaultRedirectionFlag"] = "0";
                    Session["OpeningStockdate"] = ucDatePicker.Date;
                    Response.Redirect("~/Masters/SalesChannel/SalesChannelBranding.aspx", false);
                }
                else
                {
                    ucMessage1.ShowError(ObjSales.Error);
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());

        }
    }
    /*#CC03 Added Started*/
    protected void DwnldReferenceBinCodeTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsReferenceCode = new DataSet();
            using (SalesChannelData objSalesData = new SalesChannelData())
            {

                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.eStockTransfer;
                objSalesData.Brand = PageBase.Brand;
                objSalesData.SalesChannelID = PageBase.SalesChanelID;
                objSalesData.UserID = PageBase.UserId;
                if (Client.ToLower().ToString() == "micromax")
                {
                    dsReferenceCode = objSalesData.GetAllTemplateDataMicromax();
                }
                else
                {
                    dsReferenceCode = objSalesData.GetAllTemplateData();
                }

                if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Bin Code List";
                    PageBase.RootFilePath = FilePath;
                    //PageBase.ExportToExecl(dsReferenceCode, FilenameToexport, EnumData.eTemplateCount.ePrimarysales1);/*#CC03 Commented*/
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsReferenceCode, FilenameToexport);/*#CC03 Added*/
                }
                else
                {
                    ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());

        }
    }
    /*#CC03 Added End*/
}
