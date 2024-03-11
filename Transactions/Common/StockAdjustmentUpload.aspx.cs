#region Copyright(c) 2010 Zed-Axis Technologies All rights are reserved
/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2010 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Created By : 
* Module : 
* Description : 
 * Table Name: 
* ====================================================================================================
* Reviewed By :
 ====================================================================================================
Modification On       Modified By          Modification    
---------------      -----------          -------------------------------------------------------------  
 * 12-Apr-11            AmitB            #CC01: Allow serial number between 6 to 18 digits across the application
 * 28-Sep-11            Rakesh Goel      #CC02: Changes for Access Tag and Dynamic location.
 * 29-Sep-11            Rakesh Goel      #CC03: Removed usage of Viewstate to store uploaded file data and other fixing done.
 * 10-Jun-2016          Sumit Maurya     #CC04: Textxbox "Adjustment for" commented from interface and added in template to 
 *                                              create adjustment for multiple entities .New link added to download referance 
 *                                              code according to selected saleschannel type. And  New save function added for                                                   multiple Entity stock upload.
 * 17-Jun-2016          Sumit Maurya     #CC05: code aletered to get referance details for retailer.
 * 20 July 2016, Karam Chand Sharma, #CC06, Pass dyanamic stock bin type
 * 21 July 2016, Karam Chand Sharma, #CC07, Download bin code as an reference
 * 17-May-2018, Sumit Maurya, #CC08, Implementation according to bcp, earler page was getting stuck if user is trying to upload 10K+ data (Implemented for Infocus).
 * 02-Jun-2018, Sumit Maurya, #CC09, Change log #CC08 further implemented (Done for Infocus).
 * 18-Jun-2018, Sumit Maurya, #CC10, detail showed in label for user to inform about maximum number of rows in upload excel (Done for Infocus).
 * 26-Jun-2018, Balram Jha, #CC11- Added flag to load retailer in sales channel type drop down.
 * 24-Sept-2018,Vijay Kumar Prajapati,#CC12-Get Error in Export to Excel done for Karbonn.
 ====================================================================================================
*/

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using BusinessLogics;
using ZedService;
using System.Data.SqlClient; /* #CC08 Added*/




public partial class Transactions_Common_StockAdjustmentUpload : BussinessLogic.PageBase
{
    #region VariableDeclaration
    string strUploadedFileName = string.Empty;
    //    string strDownloadPath = Pagebase.strDownloadExcelPath;
    string strDownloadPath = BussinessLogic.PageBase.strExcelVirtualPath;
    List<string> lstDuplicate = new List<string>();   //#CC03 defined as global variable so that it is accessible in Databound event.
    DataSet dsErrorProne = new DataSet();
    UploadFile UploadFile = new UploadFile();

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            hdnAdjustmentForSalesChannelid.Text = "0";
            ucDatePicker1.TextBoxDate.Text = DateTime.Now.ToShortDateString();
            HyperLink1.NavigateUrl = "../../Excel/Templates/StockAdjustmentTemplate.xlsx";
            pnlGrid.Visible = false;
            DaysLoad();
            FillsalesChannelType();
            btnUploadData.ValidationGroup = "Save"; /* #CC08 Added */
            lblExcelRowsLimitMsg.Text = "Maximum " + String.Format("{0:n0}", Convert.ToInt64(PageBase.ValidExcelRows)) + " rows allowed in upload."; /* #CC10 Added */

        }
        bindDays();
        ChangeValidationGroup();

    }
    void bindDays()
    {
        ucDatePicker1.MaxRangeValue = DateTime.Now.Date;
        ucDatePicker1.MinRangeValue = DateTime.Now.Date.AddDays(-Convert.ToInt16(ViewState["backdays"]));
        ucDatePicker1.RangeErrorMessage = "Date shlould be between " + Convert.ToInt16(ViewState["backdays"]).ToString() + " days back and todays only.";
        ucDatePicker1.ValidationGroup = "grpupld";
    }

    private void DaysLoad()
    {
        using (clsStockAdjustment obj = new clsStockAdjustment())
        {
            int b = 0;
            obj.CompanyID = PageBase.ClientId;
            DataSet ds = obj.StockAdjustmentLoad(ref b);
            ViewState["backdays"] = b.ToString();
            FillReason(ds.Tables[0]);
            //            BindAdjustmentFor();
        }
    }

    #region User Defined Function


    private void FillReason(DataTable dt)
    {
        DataTable DtReason;
        DtReason = dt;
        if (DtReason.Rows.Count > 0 && DtReason != null)
        {
            cmbReason.Items.Clear();
            cmbReason.DataSource = DtReason;
            cmbReason.DataTextField = "ReasonName";
            cmbReason.DataValueField = "ReasonID";
            cmbReason.DataBind();
        }
        cmbReason.Items.Insert(0, new ListItem("Select", "0"));
    }
    private void BindAdjustmentFor()
    {
        DataTable DtAdjustment;

    }

    private bool ValidatePage()
    {
        if (ucDatePicker1.TextBoxDate.Text == "" || Convert.ToInt32(hdnAdjustmentForSalesChannelid.Text) == 0 || cmbReason.SelectedIndex == 0)
        {
            ucMessage1.ShowWarning(Resources.Messages.MandatoryField);
            return false;
        }
        if (Convert.ToDateTime(ucDatePicker1.TextBoxDate.Text) > DateTime.Now.Date)
        {
            ucMessage1.ShowWarning(Resources.Messages.DateRangeValidation);
            return false;
        }
        return true;
    }
    private void ClearFields()
    {
        clear();
        DaysLoad();
        cmbReason.SelectedValue = "0";
        hlnkBlank.Visible = false;
        hlnkDuplicate.Visible = false;
        hlnkInvalid.Visible = false;

    }
    private Int16 IsExcelFile()
    {
        Int16 MessageforValidation = 0;
        try
        {
            if (Fileupload1.HasFile)
            {
                if (Path.GetExtension(Fileupload1.FileName).ToLower() == ".xlsx")
                {
                    try
                    {
                        strUploadedFileName = PageBase.importExportExcelFileName;
                        // Fileupload1.SaveAs(Server.MapPath(Pagebase.strUploadExcelPath + strUploadedFileName));
                        Fileupload1.SaveAs(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);
                        MessageforValidation = 1;
                        return MessageforValidation;
                    }
                    catch (Exception objEx)
                    {

                        MessageforValidation = 0;
                        return MessageforValidation;
                        throw objEx;
                    }
                }
                else
                {
                    MessageforValidation = 2;
                    return MessageforValidation;
                }
            }
            else
            {
                MessageforValidation = 3;
                return MessageforValidation;
            }
        }
        catch (HttpException objHttpException)
        {
            return MessageforValidation;
            throw objHttpException;
        }
        catch (Exception ex)
        {
            return MessageforValidation;
            throw ex;
        }
    }

    #endregion

    #region Button Events


    public DataTable GetBlankTableError()
    {
        DataTable Detail = new DataTable();
        Detail.Columns.Add("SKUCode", typeof(System.Double));
        Detail.Columns.Add("Quantity", typeof(System.Int32));
        Detail.Columns.Add("SerialNo", typeof(System.Double));
        Detail.Columns.Add("BatchNo", typeof(System.Double));
        return Detail;
    }


    protected void btnUploadData_Click(object sender, EventArgs e)
    {

        DataTable dtErrorTable = GetBlankTableError();
        DataTable dtError = new DataTable();
        HttpContext.Current.Session["PkeyColumns"] = null;
        string strKey = string.Empty;
        hlnkInvalid.Visible = false;
        Int16 Upload = 0;
        Upload = UploadFile.IsExcelFile(Fileupload1, ref strUploadedFileName);
        ViewState["TobeUploaded"] = strUploadedFileName;
        if (Upload == 1)
        {
            OpenXMLExcel objexcel = new OpenXMLExcel();
            DataSet DsExcel = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);
            if (DsExcel != null && DsExcel.Tables.Count > 0 && DsExcel.Tables[0].Rows.Count > 0)
            {

                if (DsExcel.Tables[0].Rows.Count > Convert.ToInt32(PageBase.ValidExcelRows))
                    ucMessage1.ShowInfo("Limit Crossed");
                else
                {
                    ValidateUploadFile objValidateFile = new ValidateUploadFile();
                    {
                        DataSet objDS = DsExcel;
                        DataTable dt1 = DsExcel.Tables[0];
                        SortedList objSL = new SortedList();
                        SortedList objSLCorrData = new SortedList();
                        objValidateFile.UploadedFileName = strUploadedFileName;
                        objValidateFile.ExcelFileNameInTable = "StockAdjustmentTemplate";
                        // objValidateFile.RootFolerPath = RootPath;
                        objValidateFile.CompanyId = PageBase.ClientId;
                        objValidateFile.ValidateFile(false, out objDS, out objSL);
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
                                            if (strKey == objIDicEnum.Key.ToString())
                                            {
                                                objDS.Tables["DtExcelSheet"].Rows[i]["ReasonForInvalid"] = objIDicEnum.Value.ToString();
                                            }
                                        }
                                    }
                                }

                                objDS.Tables[0].AcceptChanges();
                                if (objDS.Tables["DtExcelSheet"] != null && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                                {
                                    hlnkInvalid.Visible = true;
                                    dsErrorProne.Merge(objDS.Tables["DtExcelSheet"]);
                                    hlnkInvalid.Text = "Invalid Data";

                                    blnIsUpload = false;
                                }
                                blnIsUpload = false;
                            }
                            if (!objDS.Tables[0].Columns.Contains("ReasonForInvalid"))
                            {
                                int counter = 0;
                                if (!objDS.Tables[0].Columns.Contains("ReasonForInvalid"))
                                    objDS.Tables[0].Columns.Add(new DataColumn("ReasonForInvalid"));

                                if (counter > 0)
                                {
                                    ucMessage1.ShowInfo("Invalid Records");
                                    dsErrorProne.Merge(dtErrorTable);
                                    blnIsUpload = false;
                                }
                                else
                                {
                                    objDS.Tables[0].Columns.Remove("ReasonForInvalid");
                                }




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

                                    ucDatePicker1.IsEnabled = false;
                                    btnSave.Visible = true;
                                    ucMessage1.ShowSuccess("File upload successfully, please click on save button.");
                                    return;
                                    // InsertData(objDS.Tables[0]);
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
                            btnSave.Visible = false;

                        }
                    }
                }
            }
        }

    }

    /* #CC08 Add Start */
    protected void btnUploadData_Click2(object sender, EventArgs e)
    {

        try
        {
            if (ddlType.SelectedValue == "0")
            {
                ucMessage1.ShowInfo("Please select Sales Channel Type.");
                ddlType.Focus();
                return;
            }

            if (txtRemarks.TextBoxText.Trim() == "")
            {
                ucMessage1.ShowInfo("Please enter Remarks.");
                return;
            }

            DataTable dtErrorTable = GetBlankTableError();
            DataTable dtError = new DataTable();
            HttpContext.Current.Session["PkeyColumns"] = null;
            string strKey = string.Empty;
            hlnkInvalid.Visible = false;
            Int16 Upload = 0;
            Upload = UploadFile.IsExcelFile(Fileupload1, ref strUploadedFileName);
            ViewState["TobeUploaded"] = strUploadedFileName;
            if (Upload == 1)
            {
                OpenXMLExcel objexcel = new OpenXMLExcel();
                DataSet DsExcel = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);
                if (DsExcel != null && DsExcel.Tables.Count > 0 && DsExcel.Tables[0].Rows.Count > 0)
                {
                    if (DsExcel.Tables[0].Rows.Count > Convert.ToInt32(PageBase.ValidExcelRows))
                        ucMessage1.ShowInfo("Limit Crossed");
                    else
                    {
                        ValidateUploadFile objValidateFile = new ValidateUploadFile();
                        {
                            DataSet objDS = DsExcel;
                            DataTable dt1 = DsExcel.Tables[0];
                            SortedList objSL = new SortedList();
                            SortedList objSLCorrData = new SortedList();
                            objValidateFile.UploadedFileName = strUploadedFileName;
                            objValidateFile.CompanyId = PageBase.ClientId;
                            objValidateFile.ExcelFileNameInTable = "StockAdjustmentTemplate";
                            objValidateFile.ValidateFileWithCompanyId(false, out objDS, out objSL);
                            if (objValidateFile.Message != null && objValidateFile.Message.Trim() != "")
                                ucMessage1.ShowInfo(objValidateFile.Message);
                            else
                            {
                                ucMessage1.Visible = false;
                                bool blnIsUpload = true;
                                #region ValidationComment
                                /*
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
                                            if (strKey == objIDicEnum.Key.ToString())
                                            {
                                                objDS.Tables["DtExcelSheet"].Rows[i]["ReasonForInvalid"] = objIDicEnum.Value.ToString();
                                            }
                                        }
                                    }
                                }

                                objDS.Tables[0].AcceptChanges();
                                if (objDS.Tables["DtExcelSheet"] != null && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                                {
                                    hlnkInvalid.Visible = true;
                                    dsErrorProne.Merge(objDS.Tables["DtExcelSheet"]);
                                    hlnkInvalid.Text = "Invalid Data";

                                    blnIsUpload = false;
                                }
                                blnIsUpload = false;
                            }
                            if (!objDS.Tables[0].Columns.Contains("ReasonForInvalid"))
                            {
                                int counter = 0;
                                if (!objDS.Tables[0].Columns.Contains("ReasonForInvalid"))
                                    objDS.Tables[0].Columns.Add(new DataColumn("ReasonForInvalid"));

                                if (counter > 0)
                                {
                                    ucMessage1.ShowInfo("Invalid Records");
                                    dsErrorProne.Merge(dtErrorTable);
                                    blnIsUpload = false;
                                }
                                else
                                {
                                    objDS.Tables[0].Columns.Remove("ReasonForInvalid");
                                }

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
                            } */
                                #endregion ValidationComment
                                if (blnIsUpload)
                                {
                                    if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                                    {
                                        string guid = Guid.NewGuid().ToString();
                                        if (CreatedBcpData(objDS.Tables[0], guid))
                                        {
                                            using (clsStockAdjustment objstockAdjustment = new clsStockAdjustment())
                                            {
                                                objstockAdjustment.UserId = PageBase.UserId;
                                                objstockAdjustment.Remarks = txtRemarks.TextBoxText.Trim();
                                                objstockAdjustment.ReasonID = Convert.ToInt32(cmbReason.SelectedValue);
                                                objstockAdjustment.StockAdjustmentDate = Convert.ToDateTime(ucDatePicker1.TextBoxDate.Text);
                                                objstockAdjustment.SalesChannelTypeID = Convert.ToInt32(ddlType.SelectedValue);
                                                objstockAdjustment.SessionID = guid;
                                                /* #CC09 Add Start */
                                                objstockAdjustment.RefType = 0;
                                                objstockAdjustment.OriginalFileName = Fileupload1.FileName;
                                                objstockAdjustment.UniqueFileName = strUploadedFileName;
                                                objstockAdjustment.CompanyID = PageBase.ClientId;
                                                /* #CC09 Add End */

                                                DataSet dsResult = objstockAdjustment.SaveStockAdjustmentV2();
                                                if (objstockAdjustment.OutParam == 0)
                                                {
                                                    ucMessage1.ShowSuccess(Resources.Messages.InsertSuccessfull);
                                                }
                                                else if (objstockAdjustment.OutParam == 1)
                                                {
                                                    if (dsResult == null)
                                                    {
                                                        ucMessage1.ShowInfo(objstockAdjustment.Error);
                                                    }
                                                    else if (dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                                                    {
                                                        /* ucMessage1.XmlErrorSource = dsResult.GetXml(); */
                                                        ucMessage1.ShowInfo(objstockAdjustment.Error);
                                                        hlnkInvalid.Visible = true;
                                                        string strFileName = "InvalidData" + DateTime.Now.Ticks;
                                                        ExportInExcel(dsResult, strFileName);
                                                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                                        hlnkInvalid.Text = "Invalid Data";
                                                    }
                                                    else
                                                    {
                                                        ucMessage1.ShowError(objstockAdjustment.Error);
                                                    }
                                                }
                                                else if (objstockAdjustment.OutParam == 2)
                                                {
                                                    ucMessage1.ShowInfo(objstockAdjustment.Error);
                                                    hlnkInvalid.Visible = true;
                                                    string strFileName = "InvalidData" + DateTime.Now.Ticks;


                                                    StringReader strRdr = new StringReader(dsResult.Tables[0].Rows[0][0].ToString());
                                                    DataSet dsError = new DataSet();
                                                    dsError.ReadXml(strRdr);

                                                    ExportInExcel(dsError, strFileName);
                                                    hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                                    hlnkInvalid.Text = "Invalid Data";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ucMessage1.ShowInfo("Unable to upload data!! Please try after some time.");
                                        }
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
                                btnSave.Visible = false;

                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowInfo(ex.Message);
        }

    }

    public bool CreatedBcpData(DataTable dtUpload, string guid)
    {
        bool result = false;
        try
        {

            dtUpload.Columns.Add(AddColumn(guid, "SessionID", typeof(System.String)));
            dtUpload.Columns.Add(AddColumn(Convert.ToString(PageBase.UserId), "CreatedBy", typeof(int)));
            dtUpload.Columns.Add(AddColumn(null, "GRNDate_From", typeof(System.DateTime)));
            dtUpload.Columns.Add(AddColumn(ucDatePicker1.TextBoxDate.Text, "GRNDate_To", typeof(System.String)));
            dtUpload.AcceptChanges();
            int i = PageBase.UserId;

            if (UploadCurrentOutStandingBcp(dtUpload) == true)
            {
                result = true;
            }

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
                bulkCopy.DestinationTableName = "BulkStockAdjustment";
                bulkCopy.ColumnMappings.Add("SKUCODE", "SKUCODE");
                bulkCopy.ColumnMappings.Add("Quantity", "Quantity");
                bulkCopy.ColumnMappings.Add("SerialNo", "SerialNo");
                //bulkCopy.ColumnMappings.Add("SerialNo2", "SerialNo2");
                bulkCopy.ColumnMappings.Add("BatchNo", "BatchNo");
                bulkCopy.ColumnMappings.Add("BinCode", "StockBinTypeCode_From");
                bulkCopy.ColumnMappings.Add("BinCode", "StockBinTypeCode_To");
                bulkCopy.ColumnMappings.Add("GRNDate_From", "GRNDate_From");
                bulkCopy.ColumnMappings.Add("GRNDate_To", "GRNDate_To");
                /* bulkCopy.ColumnMappings.Add("", "");*/
                bulkCopy.ColumnMappings.Add("AdjustmentFor", "AdjustmentFor");
                bulkCopy.ColumnMappings.Add("BinCode", "BinCode");
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
    public void ChangeValidationGroup()
    {
        try
        {
            //grpupld
            /*ddlType.ValidationGroup = "grpupld";
            reqSales.ValidationGroup = "grpupld";
            cmbReason.ValidationGroup = "grpupld";
            txtRemarks.ValidationGroup = "grpupld";*/
            btnUploadData.ValidationGroup = "Save";
            regExFileUpload.ValidationGroup = "Save";
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.Message);
        }
    }


    /* #CC08 Add End */

    private void ExportInExcel(DataSet DsError, string strFileName)
    {
        if (DsError != null && DsError.Tables.Count > 0)
        {
            PageBase.ExportToExeclV2(DsError, strFileName, DsError.Tables.Count);
        }
    }

    private void ExportInExcel(DataTable objTmpTb, string strFileName)
    {
        if (objTmpTb.Rows.Count > 0)
        {
            string FileName = strFileName;
            //string Path = Server.MapPath(strDownloadPath);
            string Path = strDownloadPath;
            DataSet ds = new DataSet();

            objTmpTb.TableName = "tblInvalidRecords" + DateTime.Now.Ticks;
            DataTable dt = objTmpTb.Copy();
            ds.Tables.Add(dt);
            string FilenameToexport = "tblInvalidRecords" + DateTime.Now.Ticks;
            PageBase.ExportToExecl(ds, FilenameToexport, EnumData.eTemplateCount.ePrimarysales1 + 1);
            // LuminousSMS.Utility.LuminousUtil.SaveToExecl(ds, strFileName);
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        ClearFields();



    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        OpenXMLExcel objexcel = new OpenXMLExcel();

        DataSet DsExcel = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + Convert.ToString(ViewState["TobeUploaded"]));
        DataTable dtUploadedFile = DsExcel.Tables[0];

        DataTable dtSubmit = new DataTable();
        dtSubmit = PageBase.GetStockUpdateTable();
        dtSubmit.Columns.Add("Adjustmentfor", typeof(string)); /* #CC04 Added */
        dtSubmit.Columns.Add("SerialNo2", typeof(string));
        using (clsStockAdjustment ObjStockAdjust = new clsStockAdjustment())
        {

            //DataTable dtSubmit = DsXML.Tables[0].Copy();


            foreach (DataRow row in dtUploadedFile.Rows)
            {
                DataRow NewRow = dtSubmit.NewRow();
                NewRow["SKUCode"] = row["skucode"];
                NewRow["Quantity"] = row["Quantity"];
                /*#CC06 START COMMENTED NewRow["StockBinTypeCode_From"] = "COFGD";
                NewRow["StockBinTypeCode_To"] = "COFGD";#CC06 END COMMENTED */
                NewRow["StockBinTypeCode_From"] = row["BinCode"];/*#CC06 ADDED*/
                NewRow["StockBinTypeCode_To"] = row["BinCode"];/*#CC06 ADDED*/
                NewRow["SerialNo"] = row["SerialNo"];
                NewRow["SerialNo2"] = row["SerialNo2"];
                NewRow["BatchNo"] = row["BatchNo"];
                NewRow["GRNDate_From"] = null;
                NewRow["GRNDate_To"] = Convert.ToDateTime(ucDatePicker1.TextBoxDate.Text);
                NewRow["Adjustmentfor"] = row["Adjustmentfor"]; /* #CC04 Added */

                dtSubmit.Rows.Add(NewRow);

                if (Convert.ToInt16(row["quantity"]) < 0)
                {
                    NewRow["StockBinTypeCode_To"] = null;
                }
                else
                {
                    NewRow["StockBinTypeCode_From"] = null;

                }

                NewRow["quantity"] = Math.Abs(Convert.ToInt32(row["quantity"]));
            }

            dtSubmit.AcceptChanges();



            byte SCTypeID = 0;

            // string[] s = ddlAdjustmentFor.SelectedValue.Split('/');

            //SCTypeID = Convert.ToByte(s[1]);
            SCTypeID = Convert.ToByte(ddlType.SelectedValue);
            ObjStockAdjust.XML_PartDetails = dtSubmit;
            ObjStockAdjust.SalesChannelTypeID = SCTypeID;

            ObjStockAdjust.StrstockAdjustmentDetail = "<NewDataSet><Table></Table></NewDataSet>";
            ObjStockAdjust.IntStockAdjustmentID = 0;
            ObjStockAdjust.IntStockAdjustmentForID = Convert.ToInt32(hdnAdjustmentForSalesChannelid.Text);//Convert.ToInt32(s[0]);
            ObjStockAdjust.DtStockAdjustmentDate = DateTime.Parse(ucDatePicker1.Date);
            ObjStockAdjust.StrRemarks = txtRemarks.TextBoxText.Trim();
            ObjStockAdjust.StrError = "";
            ObjStockAdjust.IntCreatedBy = PageBase.UserId;
            ObjStockAdjust.ReasonID = Convert.ToInt16(cmbReason.SelectedValue);  //This would be used in the opening stock case
            ObjStockAdjust.CompanyID = PageBase.ClientId;
            /*
             #CC04 Comment Start
             ObjStockAdjust.Save();   

            if (ObjStockAdjust.StrstockAdjustmentDetail != null && ObjStockAdjust.StrstockAdjustmentDetail != string.Empty)
            {
                ucMessage1.XmlErrorSource = ObjStockAdjust.StrstockAdjustmentDetail;
                //  dvSave.Attributes.Add("Style", "display:none");
                return;
            }
            if (ObjStockAdjust.StrError != null && ObjStockAdjust.StrError != "")
            {
                ucMessage1.ShowError(ObjStockAdjust.StrError);
                //  dvSave.Attributes.Add("Style", "display:none");
                return;
            }
            if (ObjStockAdjust.ErrorValue == 1)
            {
                // ucMessage1.ShowError(Resources.WarningMessages.BackDaysIssue);
                // dvSave.Attributes.Add("Style", "display:none");
                return;
            }
            else if (ObjStockAdjust.ErrorValue == 2)
            {
                ucMessage1.ShowError("Stock adjustment date can not be less than from opening stock date.");
                return;
            }
             #CC04 Comment End
            */
            /* #CC04 Add Start */
            DataSet dsResult = ObjStockAdjust.SaveStockAdjustment();
            int result = ObjStockAdjust.OutParam;
            if (result == 1 && dsResult.Tables.Count > 0)
            {
                if (dsResult.Tables[0].Rows.Count > 0)
                {

                    /* ucMessage1.XmlErrorSource = dsResult.GetXml();*/ /*#CC12 Commented*/
                                                                        /*#CC12 Added Started*/
                    ucMessage1.ShowInfo(ObjStockAdjust.Error);
                    hlnkInvalid.Visible = true;
                    string strFileName = "InvalidData" + DateTime.Now.Ticks;
                    ExportInExcel(dsResult, strFileName);
                    hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                    hlnkInvalid.Text = "Invalid Data";
                    /*#CC12 Added End*/
                }
            }
            else if (result == 0)
                /* #CC04 Add End */
                ucMessage1.ShowSuccess(Resources.Messages.InsertSuccessfull);
            else if (result == 2)
                ucMessage1.ShowError("Stock maintain mode is Zero for this Sales Channel Type.");

            pnlGrid.Visible = false;
            //ddlAdjustmentFor.SelectedIndex = 0;
            /* txtAdjustmentFor.Text = string.Empty;  #CC04 Commented */
            cmbReason.SelectedIndex = 0;
            txtRemarks.TextBoxText = "";
            hdnAdjustmentForSalesChannelid.Text = "0";
            //ddlType.SelectedValue = "0";
            ddlType.Enabled = true;

        };

        //}
        //catch (Exception ex)
        //{
        //    ucMessage1.ShowAppError(ex);
        //    cmbReason.Enabled = true;
        //}
    }
    void clear()
    {
        // ddlAdjustmentFor.SelectedIndex = 0;
        cmbReason.SelectedIndex = 0;
        txtRemarks.TextBoxText = "";
        ucMessage1.Visible = false;
        // ucDatePicker1.TextBoxDate.Text = DateTime.Now.ToShortDateString();
        btnSave.Visible = false;
        ucDatePicker1.IsEnabled = true;
        hdnAdjustmentForSalesChannelid.Text = "0";
        ddlType.SelectedValue = "0";
        /*txtAdjustmentFor.Text = string.Empty; #CC04 Commented */
        ddlType.Enabled = true;
        //cmbReason.SelectedValue = "0";
    }

    protected void DownloadTemplatePartCode_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(hdnAdjustmentForSalesChannelid.Text) != 0)
            {
                using (clsStockAdjustment objStockAdjust = new clsStockAdjustment())
                {
                    objStockAdjust.EntityId = Convert.ToInt32(hdnAdjustmentForSalesChannelid.Text);
                    DataSet ds = objStockAdjust.GetPartCodeTemplate();
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        PageBase.ExportToExecl(ds, "PartCodeTemplate", EnumData.eTemplateCount.ePrimarysales1 + 1);
                        // LuminousSMS.Utility.LuminousUtil.ExportToExecl(ds, "PartCodeTemplate");
                    }
                    else
                    {
                        ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                    }
                }
            }
            else
            {
                ucMessage1.ShowError("Select plant first before download parts");
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);

        }
    }
    protected void DownloadStockBinType_Click(object sender, EventArgs e)
    {
        try
        {
            using (clsStockAdjustment objStockAdjust = new clsStockAdjustment())
            {
                DataSet dsStockBinType = objStockAdjust.GetStockBinType();
                if (dsStockBinType != null && dsStockBinType.Tables[0].Rows.Count > 0)
                {
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "StockBinTypeCodeTemplate";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dsStockBinType, FilenameToexport, EnumData.eTemplateCount.ePrice);

                    //LuminousSMS.Utility.LuminousUtil.ExportToExecl(dsStockBinType, "StockBinTypeCodeTemplate");
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
            PageBase.Errorhandling(ex);
        }
    }

    #endregion

    protected void gridStockAdjustment_DataBound(object sender, EventArgs e)
    {

        bool hasDuplicateRecords = false;
        bool hasInvalidSerialNo = false;
        //List<string> lstDuplicate = (List<string>)ViewState["duplicateSerial"];
        Regex serialregex = new Regex(Resources.GlobalMessages.SerialNoExpression);    // #CC01 : Changed

        if (lstDuplicate != null && lstDuplicate.Count > 0)
        {
            foreach (GridViewRow row in gridStockAdjustment.Rows)
            {
                if (lstDuplicate != null && lstDuplicate.Contains(row.Cells[3].Text))
                {
                    row.BackColor = System.Drawing.Color.FromName("#FFBABA");
                    row.ToolTip = "Duplicate serial no. Please correct data before uploading.";
                    hasDuplicateRecords = true;
                }
                if (!string.IsNullOrEmpty(row.Cells[3].Text.Replace("&nbsp;", "")))
                {
                    if (!serialregex.IsMatch(row.Cells[3].Text))
                    {
                        row.BackColor = System.Drawing.Color.FromName("#FEEFB3");
                        hasInvalidSerialNo = true;
                        row.ToolTip = Resources.GlobalMessages.SerialNoErrorMessage;   // #CC01 : Changed
                    }
                }
            }
        }

        if (hasDuplicateRecords || hasInvalidSerialNo)
        {
            btnSave.Visible = false;
            //btnSave.Enabled = false;
            btnSave.ToolTip = "Excel file contains error. Please correct data before uploading.";
            throw new ArgumentException(btnSave.ToolTip);
        }
        else
        {
            btnSave.Visible = true;
            //btnSave.Enabled = true;
            btnSave.ToolTip = string.Empty;
        }
    }
    protected void rdModelList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdModelList.SelectedValue == "1")
            Response.Redirect("~/Transactions/Common/StockAdjustmentInterface.aspx");
    }
    void FillsalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
            ObjSalesChannel.UserID = PageBase.UserId;
            ObjSalesChannel.CompanyId = PageBase.ClientId;
            ObjSalesChannel.LoadRetailer = 1;//#CC11 added
            ddlType.Items.Clear();

            string[] str = { "SalesChannelTypeID", "SalesChannelTypeName" };
            PageBase.DropdownBinding(ref ddlType, ObjSalesChannel.GetSalesChannelTypeV3(), str);
            //if (PageBase.SalesChanelID != 0)
            //{
            //    ddlType.SelectedValue = PageBase.SalesChanelTypeID.ToString();
            //    if (PageBase.SalesChanelTypeID != 5 && PageBase.IsRetailerStockTrack == 1)
            //        ddlType.Items.Add(new ListItem("Retailer", "101"));
            //}
            //else if (PageBase.SalesChanelID == 0)
            //{
            //    ddlType.Items.Clear();
            //    ddlType.Items.Insert(0, new ListItem("Retailer", "101"));
            //    ddlType.Enabled = false;
            //}
            //else if (PageBase.IsRetailerStockTrack == 1)
            //{
            //    ddlType.Items.Add(new ListItem("Retailer", "101"));
            //    ddlType.Enabled = true;
            //}
        };
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        /* AutoCompleteExtender1.ContextKey = ddlType.SelectedValue; #CC04 Commented */
        hdnAdjustmentForSalesChannelid.Text = "0";
        ucMessage1.Visible = false;
        ddlType.Enabled = false;
    }

    /* #CC04 Add Start */
    protected void lnkRefCode_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlType.SelectedValue == "0")
            {
                ucMessage1.ShowInfo("Select salechannel type");
                return;
            }
            else
            {
                ucMessage1.Visible = false;
                DataTable dtReferenceCode = new DataTable();
                DataSet dsResult = new DataSet();
                SalesChannelData obj = new SalesChannelData();
                obj.SalesChannelTypeID = Convert.ToInt16(ddlType.SelectedValue);
                obj.ActiveStatus = 1;
                obj.CompanyId = PageBase.ClientId;
                /* #CC05 Add Start */
                if (ddlType.SelectedValue == "12")
                {
                    obj.SalesChannelCode = "";
                    obj.ComingFrom = 1;
                }
                /* #CC05 Add End */
                //dtReferenceCode = obj.GetSalesChannelList();                 
                // /* #CC05 Add Start */
                // if (dtReferenceCode.Rows.Count > 0)
                // {
                //     /* #CC05 Add End */
                //     string[] strRemoveColumn = { "SalesChannelID", "RetailerID" };
                //     for (int i = 0; i < strRemoveColumn.Length; i++)
                //     {
                //         if (dtReferenceCode.Columns.Contains(strRemoveColumn[i]))
                //             dtReferenceCode.Columns.Remove(strRemoveColumn[i]);

                //     }
                //     dtReferenceCode.AcceptChanges();
                //     DataTable dtclone = new DataTable();
                //     dtclone = dtReferenceCode.Copy();

                //     dsResult.Tables.Add(dtclone);
                //     if (dtReferenceCode.Rows.Count > 0)
                //     {
                //         String FilePath = Server.MapPath("../../");
                //         string FilenameToexport = "Reference Code List";
                //         PageBase.RootFilePath = FilePath;
                //         PageBase.ExportToExecl(dsResult, FilenameToexport, EnumData.eTemplateCount.eRetailer + 2);
                //     }

                // }



                DataSet ds = obj.GetSalesChannelListDS();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string[] strRemoveColumn = { "SalesChannelID", "RetailerID" };
                    for (int i = 0; i < strRemoveColumn.Length; i++)
                    {
                        if (ds.Tables[0].Columns.Contains(strRemoveColumn[i]))
                            ds.Tables[0].Columns.Remove(strRemoveColumn[i]);

                    }
                    ds.Tables[0].AcceptChanges();

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(ds, FilenameToexport, EnumData.eTemplateCount.eRetailer + 2);


                }

                /* #CC05 Add Start */

                else
                {
                    ucMessage1.ShowInfo("No record found");
                }
                /* #CC05 Add End */
            }
        }
        catch (Exception ex)
        {

        }

    }
    /* #CC04 Add End */
    /*#CC07 START ADDED*/
    protected void DwnldBindCode_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsReferenceCode = new DataSet();
            using (SalesChannelData objSalesData = new SalesChannelData())
            {

                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.ePrice;

                dsReferenceCode = objSalesData.GetBinCode();
                if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
                {

                    string FilenameToexport = "Reference Code List";
                    PageBase.ExportToExecl(dsReferenceCode, FilenameToexport, EnumData.eTemplateCount.ePrimarysales1);
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
        /*#CC07 END ADDED*/

    }
}


