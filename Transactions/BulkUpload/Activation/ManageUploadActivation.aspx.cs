/*
 20-Jul-2018, Balram Jha, #CC01- on upload direct bulk copy and validate data in back end
 */
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
using ZedService;
using System.IO;

public partial class Transactions_BulkUpload_Activation_ManageUploadActivation : PageBase
{
    DataTable dtNew = new DataTable();
    int counter = 0;
    string strActivationSessionName = "ActivationUploadSession";
    string strUploadedFileNameFromInterface = "TobeUploaded";
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    UploadFile UploadFile = new UploadFile();
    DataSet dsErrorProne = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMsg.ShowControl = false;
            if (!IsPostBack)
               pnlGrid.Visible = false;
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());

        }

    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtErrorTable = GetBlankTableError();
            DataTable dtError = new DataTable();
            HttpContext.Current.Session["PkeyColumns"] = null;
            string strKey = string.Empty;
            ClearForm();
            hlnkInvalid.Visible = false;
            Int16 Upload = 0;
            Upload = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            ViewState[strUploadedFileNameFromInterface] = strUploadedFileName;
            if (Upload == 1)
            {
                OpenXMLExcel objexcel = new OpenXMLExcel();
               // DataSet DsExcel = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName+".xslx");
                DataSet DsExcel = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName );
               
                if (DsExcel != null && DsExcel.Tables.Count > 0 && DsExcel.Tables[0].Rows.Count > 0)
                {

                    if (DsExcel.Tables[0].Rows.Count > Convert.ToInt32(PageBase.ValidExcelRows))
                        ucMsg.ShowInfo("Limit Crossed");
                    else
                    {
                        ValidateUploadFile objValidateFile = new ValidateUploadFile();
                        {
                            DataSet objDS = DsExcel;
                            DataTable dt1 = DsExcel.Tables[0];
                            SortedList objSL = new SortedList();
                            SortedList objSLCorrData = new SortedList();
                            objValidateFile.UploadedFileName = strUploadedFileName;
                            objValidateFile.ExcelFileNameInTable = "UploadActivation";
                            objValidateFile.ValidateFile(false, out objDS, out objSL);
                            if (objValidateFile.Message != null && objValidateFile.Message.Trim() != "")
                                ucMsg.ShowInfo(objValidateFile.Message);
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
                                    string strFileName = "InvalidData" + DateTime.Now.Ticks;
                                    ExportInExcel(dsErrorProne, strFileName);
                                    hlnkInvalid.Text = "Invalid Data";
                                    hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                    return;
                                    
                                }
                               
                            }
                            else
                            {
                                string guid = Guid.NewGuid().ToString();
                                ViewState[strActivationSessionName] = guid;
                                DsExcel.Tables[0].Columns.Add(AddColumn(guid, "TransUploadSession", typeof(System.String)));
                                DsExcel.Tables[0].Columns.Add(AddColumn("1", "TransType", typeof(System.Int32)));//no need here but for future
                                DsExcel.Tables[0].AcceptChanges();
                                if (DsExcel.Tables[0].Rows.Count > 0)
                                {
                                    if (!BulkCopyUpload(DsExcel.Tables[0]))
                                    {
                                        ucMsg.ShowError("Error Occured While transferring the data to the server");
                                        return;
                                    }

                                }
                                using (SalesData objActivation = new SalesData())
                                {
                                    objActivation.EntryType = EnumData.eEntryType.eUpload;
                                    objActivation.UserID = PageBase.UserId;
                                    objActivation.TransUploadSession = Convert.ToString(ViewState[strActivationSessionName]);
                                    int intResult = objActivation.InsertBulkActivationBCP();

                                    if (objActivation.ErrorDetailXML != null && objActivation.ErrorDetailXML != string.Empty)
                                    {
                                       // ucMsg.XmlErrorSource = objActivation.ErrorDetailXML;
                                        //return;
                                        StringReader theReader = new StringReader(objActivation.ErrorDetailXML);
                                        DataSet theDataSet = new DataSet();
                                        theDataSet.ReadXml(theReader);
                                        hlnkInvalid.Visible = true;
                                        dsErrorProne.Merge(theDataSet.Tables[0]);
                                        string strFileName = "InvalidData" + DateTime.Now.Ticks;
                                        ExportInExcel(dsErrorProne, strFileName);
                                        hlnkInvalid.Text = "Invalid Data";
                                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                        return;
                                    }
                                    if (objActivation.Error != null && objActivation.Error != "")
                                    {
                                        ucMsg.ShowError(objActivation.Error);
                                        return;
                                    }
                                    if (intResult == 2)
                                    {
                                        ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                                        return;
                                    }
                                    ClearForm();
                                    ucMsg.Visible = true;
                                    ucMsg.ShowSuccess(Resources.Messages.DataUploadSuccess);

                                }
                            }
                            //#CC01 coment start
                            //{
                            //    ucMsg.Visible = false;
                            //    bool blnIsUpload = true;
                            //    if (objSL.Count >= 1 && objSL.Keys.Count >= 1)
                            //    {
                            //        objDS.Tables["DtExcelSheet"].Columns.Add(new DataColumn("ReasonForInvalid"));
                            //        IDictionaryEnumerator objIDicEnum = objSL.GetEnumerator();
                            //        while (objIDicEnum.MoveNext())
                            //        {
                            //            string[] strpkeyColumnName = Convert.ToString(HttpContext.Current.Session["PkeyColumns"]).Split(',');
                            //            if (HttpContext.Current.Session["PkeyColumns"] != null)
                            //            {
                            //                for (int i = 0; i <= objDS.Tables["DtExcelSheet"].Rows.Count - 1; i++)
                            //                {
                            //                    strKey = string.Empty;
                            //                    for (int j = 0; j <= strpkeyColumnName.Length - 1; j++)
                            //                    {
                            //                        if (strKey == string.Empty)
                            //                            strKey = objDS.Tables["DtExcelSheet"].Rows[i][strpkeyColumnName[j]].ToString();
                            //                        else
                            //                            strKey = strKey + objDS.Tables["DtExcelSheet"].Rows[i][strpkeyColumnName[j]].ToString();
                            //                    }
                            //                    if (strKey == objIDicEnum.Key.ToString())
                            //                    {
                            //                        objDS.Tables["DtExcelSheet"].Rows[i]["ReasonForInvalid"] = objIDicEnum.Value.ToString();
                            //                    }
                            //                }
                            //            }
                            //        }

                            //        objDS.Tables[0].AcceptChanges();
                            //        if (objDS.Tables["DtExcelSheet"] != null && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                            //        {
                            //            hlnkInvalid.Visible = true;
                            //            dsErrorProne.Merge(objDS.Tables["DtExcelSheet"]);
                            //            hlnkInvalid.Text = "Invalid Data";

                            //            blnIsUpload = false;
                            //        }
                            //        blnIsUpload = false;
                            //    }
                            //    if (!objDS.Tables[0].Columns.Contains("ReasonForInvalid"))
                            //    {
                            //        int counter = 0;
                            //        if (!objDS.Tables[0].Columns.Contains("ReasonForInvalid"))
                            //            objDS.Tables[0].Columns.Add(new DataColumn("ReasonForInvalid"));
                                 
                            //            var query = from r in objDS.Tables[0].AsEnumerable()
                            //                        where ((Convert.ToDateTime(r["ActivationDate"].ToString().Trim()) > System.DateTime.Now))
                            //                        select new
                            //                        {
                            //                            IMEINo = Convert.ToString(r["Serial#1"])
                                            
                            //                        };
                            //            if (query != null)
                            //            {
                            //                if (query.Count() > 0)
                            //                {
                            //                    counter = 1;
                            //                    dtError = PageBase.LINQToDataTable(query);
                            //                    foreach (DataRow dr in dtError.Rows)
                            //                    {
                            //                        DataRow drow = dtErrorTable.NewRow();
                            //                        drow["IMEISerialNo"] = dr["IMEINo"];
                            //                        drow["ReasonForInvalid"] = "Activation Date Can not be Future Date.";
                            //                        dtErrorTable.Rows.Add(drow);
                            //                    }
                            //                    dtErrorTable.AcceptChanges();
                            //                }
                            //            }

                            //        if (counter > 0)
                            //        {
                            //            ucMsg.ShowInfo("Invalid Records");
                            //            dsErrorProne.Merge(dtErrorTable);
                            //            blnIsUpload = false;
                            //        }
                            //        else
                            //        {
                            //            objDS.Tables[0].Columns.Remove("ReasonForInvalid");
                            //        }
                            //    }

                            //    if (objDS.Tables["DtDuplicateRecord"] != null && objDS.Tables["DtDuplicateRecord"].Rows.Count > 0)
                            //    {
                            //        dsErrorProne.Merge(objDS.Tables["DtDuplicateRecord"]);
                            //        blnIsUpload = false;
                            //    }
                            //    if (objDS.Tables["DtBlankData"] != null && objDS.Tables["DtBlankData"].Rows.Count > 0)
                            //    {
                            //        dsErrorProne.Merge(objDS.Tables["DtBlankData"]);
                            //        blnIsUpload = false;
                            //    }
                            //    if (blnIsUpload)
                            //    {
                            //        if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                            //        {
                            //            InsertData(objDS.Tables[0]);
                            //        }
                            //        else
                            //            ucMsg.ShowInfo(Resources.Messages.NoRecord);
                            //    }
                            //    else
                            //    {
                            //        hlnkInvalid.Visible = true;
                            //        string strFileName = "InvalidData" + DateTime.Now.Ticks;
                            //        for (int i = 0; i < dsErrorProne.Tables.Count; i++)
                            //            if (dsErrorProne.Tables[i].Columns["WarehouseCode"] != null)
                            //                dsErrorProne.Tables[i].Columns["WarehouseCode"].ColumnName = "FromSalesChannelCode";
                            //        dsErrorProne.AcceptChanges();
                            //        ExportInExcel(dsErrorProne, strFileName);
                            //        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                            //        hlnkInvalid.Text = "Invalid Data";
                            //    }
                            //}
                            ////#CC01 coment end
                        }
                    }
                }
                else
                {
                    ucMsg.ShowInfo(Resources.Messages.NoRecord);
                }
            }
            else if (Upload == 3)
            {
                ucMsg.ShowWarning(Resources.Messages.NoFileExistUploading);
            }
            else if (Upload == 2)
            {
                ucMsg.ShowWarning(Resources.GlobalMessages.Uploadonlyxlsx);
            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }



    private void InsertData(DataTable dtActivation)
    {
        try
        {
            if (dtActivation.Rows.Count > 0)
            {
                int result = (from r in dtActivation.AsEnumerable() select r.Field<string>("Serial#1")).Count();
                lblTotal.Visible = true;
                lblTotal.Text = "Total Activation: " + Convert.ToString(result);
                dvUploadPreview.Visible = true;
                pnlGrid.Visible = true;
                Btnsave.Enabled = false;
                BtnCancel.Visible = false;
                updGrid.Update();
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }

    }
    protected void Btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }
            if (ViewState[strUploadedFileNameFromInterface] != null)
            {
                int intResult = 0;
               DataSet DsExcelDetail = new DataSet();
                OpenXMLExcel objexcel = new OpenXMLExcel();
                DsExcelDetail = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + ViewState[strUploadedFileNameFromInterface].ToString());


                string guid = Guid.NewGuid().ToString();
                ViewState[strActivationSessionName] = guid;
                DsExcelDetail.Tables[0].Columns.Add(AddColumn(guid, "TransUploadSession", typeof(System.String)));
                DsExcelDetail.Tables[0].Columns.Add(AddColumn("1", "TransType", typeof(System.Int32)));//no need here but for future
                DsExcelDetail.Tables[0].AcceptChanges();
                if (DsExcelDetail.Tables[0].Rows.Count > 0)
                {
                    if (!BulkCopyUpload(DsExcelDetail.Tables[0]))
                    {
                        ucMsg.ShowError("Error Occured While transferring the data to the server");
                        return;
                    }

                }
                using (SalesData objActivation = new SalesData())
                {
                    objActivation.EntryType = EnumData.eEntryType.eUpload;
                    objActivation.UserID = PageBase.UserId;
                    objActivation.TransUploadSession = Convert.ToString(ViewState[strActivationSessionName]);
                    intResult = objActivation.InsertBulkActivationBCP();

                    if (objActivation.ErrorDetailXML != null && objActivation.ErrorDetailXML != string.Empty)
                    {
                        ucMsg.XmlErrorSource = objActivation.ErrorDetailXML;
                        return;
                    }
                    if (objActivation.Error != null && objActivation.Error != "")
                    {
                        ucMsg.ShowError(objActivation.Error);
                        return;
                    }
                    if (intResult == 2)
                    {
                        ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                        return;
                    }
                    ClearForm();
                    ucMsg.ShowSuccess(Resources.Messages.DataUploadSuccess);

                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        ClearForm();

    }
    void ClearForm()
    {
        dvUploadPreview.Visible = false;
        pnlGrid.Visible = false;
        ucMsg.Visible = false;
        lblTotal.Text = "";
        updGrid.Update();
    }
   
    private void ExportInExcel(DataSet DsError, string strFileName)
    {
        if (DsError != null && DsError.Tables.Count > 0)
        {
            PageBase.ExportToExeclV2(DsError, strFileName, DsError.Tables.Count);
        }
    }
   
    public DataTable GetBlankTableError()
    {
        DataTable Detail = new DataTable();
        Detail.Columns.Add("IMEISerialNo");
        Detail.Columns.Add("ReasonForInvalid");
        return Detail;
    }
   
    DataColumn AddColumn(string columnValue, string ColumnName, Type ColumnType)
    {
        DataColumn dcSession = new DataColumn();
        dcSession.ColumnName = ColumnName;

        if (ColumnType == typeof(int))
        {
            dcSession.DataType = typeof(System.Int32);
            dcSession.DefaultValue = Convert.ToInt32(columnValue);
        }
        if (ColumnType == typeof(System.String))
        {
            dcSession.DataType = typeof(System.String);
            dcSession.DefaultValue = columnValue;
        }
        return dcSession;
    }
    public bool BulkCopyUpload(DataTable dtTempTable)
    {
        try
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "TertiarySMSBulk";
                bulkCopy.ColumnMappings.Add("MobileNo", "MobileNo");
                bulkCopy.ColumnMappings.Add("Serial#1", "Serial#1");
                bulkCopy.ColumnMappings.Add("ActivationDate", "ActivationDate");
                bulkCopy.ColumnMappings.Add("ModelName", "ModelName");
                bulkCopy.ColumnMappings.Add("Serial#2", "Serial#2");
                bulkCopy.ColumnMappings.Add("Operator", "Operator");
                bulkCopy.ColumnMappings.Add("Circle", "Circle");
                bulkCopy.ColumnMappings.Add("TransUploadSession", "TransUploadSession");
                bulkCopy.ColumnMappings.Add("TransType", "TransType");
                bulkCopy.ColumnMappings.Add("SKUCode", "SKUCode");
                bulkCopy.ColumnMappings.Add("Country", "Country");
                bulkCopy.ColumnMappings.Add("State", "State");
                bulkCopy.ColumnMappings.Add("City", "City");
                bulkCopy.WriteToServer(dtTempTable);
                return true;
            }

        }
        catch (Exception ex)
        {
            return false;
        }
    }
    protected void DwnldReferenceCodeTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (SalesData objSalesData = new SalesData())
            {
                objSalesData.UserID = PageBase.UserId;
                dsTemplateCode = objSalesData.GetAllTemplateData();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "SkuCodeList", "Country_State_City" };
                    ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, Convert.ToInt16(EnumData.eTemplateCount.ePrice + 1));
                    if (dsTemplateCode.Tables.Count > 2)
                        dsTemplateCode.Tables.RemoveAt(2);
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsTemplateCode, FilenameToexport, 2, strExcelSheetName);
                }
                else
                {
                    ucMsg.ShowInfo(Resources.Messages.NoRecord);
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
}
