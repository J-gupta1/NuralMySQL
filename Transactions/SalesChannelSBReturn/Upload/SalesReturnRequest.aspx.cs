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
using System.Linq.Expressions;
using ZedService;
using System.IO;
/*
 * 18 Feb 2020, Balram Jha, Created for Motoroal for sales return request
 */
public partial class Transactions_SalesReturnRequest : PageBase
{
    DataTable dtNew = new DataTable();
    object objSum;
    int counter = 0;
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    
    UploadFile UploadFile = new UploadFile();
    List<String> lstDuplicate = new List<String>();
    DataSet dsErrorProne = new DataSet();
    string strSecondaryReturnSessionName = "SecondaryReturnUploadSession";
    string ErrorColumnName = "Error";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMsg.ShowControl = false;
            
            
          
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
            if (txtRemark.Text.Trim().Length>250)
            {
                ucMsg.ShowInfo("More than 250 characters not allowed in remark.");
                return;
            }
            if (string.IsNullOrEmpty( txtRemark.Text.Trim()))
            {
                ucMsg.ShowInfo("Return request remark is required.");
                return;
            }
            DataTable dtErrorTable = GetBlankTableError();
            DataTable dtError = new DataTable();
            HttpContext.Current.Session["PkeyColumns"] = null;
            string strKey = string.Empty;
            ClearForm();
            hlnkInvalid.Visible = false;
            Int16 Upload = 0;
            Upload = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            //ViewState["TobeUploaded"] = strUploadedFileName;
            if (Upload == 1)
            {
                DataTable dtExcelData = new DataTable();
                OpenXMLExcel objexcel = new OpenXMLExcel();
                //DataSet DsExcel = objexcel.ImportExcelFile(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);
                DataSet DsExcel = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);
                if (DsExcel != null && DsExcel.Tables.Count > 0 && DsExcel.Tables[0].Rows.Count > 0)
                {
                   
                    if (DsExcel.Tables[0].Rows.Count > Convert.ToInt32(PageBase.ValidExcelRows))
                        ucMsg.ShowInfo("Limit Crossed");
                    else
                    {
                        ValidateUploadFile objValidateFile = new ValidateUploadFile();
                        {
                            objValidateFile.UploadedFileName = strUploadedFileName;
                            objValidateFile.ExcelFileNameInTable = "SalesReturnRequest";
                            
                            

                            objValidateFile.ValidateFileBCPV5(out dtExcelData, strUploadedFileName);
                            if (objValidateFile.Message != null && objValidateFile.Message.Trim() != "")
                                ucMsg.ShowInfo(objValidateFile.Message);
                            else
                            {
                                ucMsg.Visible = false;
                                bool blnIsUpload = true;

                                //If Sales channel login- from sales channel/Retailer should be mapped with him

                                //should not be FromSalesChannelCode=TosalesChannelCode)

                                
                                if (blnIsUpload)
                                {
                                    if (dtExcelData != null && dtExcelData.Rows.Count > 0)
                                    {
                                        SaveData(dtExcelData);
                                    }
                                    else
                                        ucMsg.ShowInfo(Resources.Messages.NoRecord);
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
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }



   private void SaveData(DataTable dtGRN)
    {
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }
            
            
            
                int intResult = 0;
                

                
                string guid = Guid.NewGuid().ToString();
                ViewState[strSecondaryReturnSessionName] = guid;


                dtGRN.Columns.Add(AddColumn(guid, "TransUploadSession", typeof(System.String)));
                dtGRN.Columns.Add(AddColumn("7", "TransType", typeof(System.Int32)));
                dtGRN.AcceptChanges();
                if (dtGRN.Rows.Count > 0)
                {
                    if (!BulkCopyUpload(dtGRN))
                    {
                        ucMsg.ShowError("Error Occured While transferring the data to the server");
                        return;
                    }

                }
                using (SalesData objSecondary = new SalesData())
                {
                    objSecondary.UserID = PageBase.UserId;

                    objSecondary.TransUploadSession = guid;
                    objSecondary.Remark = txtRemark.Text.Trim();
                    intResult = objSecondary.SalesReturnRequest();

                    if (objSecondary.ErrorDetailXML != null && objSecondary.ErrorDetailXML != string.Empty)
                    {
                        /*#CC07 Added Started*/
                        hlnkInvalid.Visible = true;
                        string strFileName = "InvalidData" + DateTime.Now.Ticks;
                        StringReader theReader = new StringReader(objSecondary.ErrorDetailXML);
                        dsErrorProne.ReadXml(theReader);
                        ExportInExcel(dsErrorProne, strFileName);
                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                        hlnkInvalid.Text = "Invalid Data";
                        /*#CC07 Added End*/
                       // ucMsg.XmlErrorSource = objSecondary.ErrorDetailXML;
                        return;
                    }
                    if (objSecondary.Error != null && objSecondary.Error != "")
                    {
                        ucMsg.ShowError(objSecondary.Error);
                        return;
                    }
                    if (intResult == 2)
                    {
                        ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                        return;
                    }
                    ClearForm();
                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                  

                }
            
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }

    
    void ClearForm()
    {
        ucMsg.Visible = false;
        
    }

    protected void GridGRN_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            //GridGRN.PageIndex = e.NewPageIndex;
            //DataTable dt = new DataTable();
            //GridGRN.DataSource = (DataTable)ViewState["GRN"];
            //GridGRN.DataBind();
        }

        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }


    protected void DwnldReferenceCodeTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (SalesChannelData objSalesData = new SalesChannelData())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.eSecondarySales;
                objSalesData.SalesChannelID = PageBase.SalesChanelID;
                objSalesData.Brand = PageBase.Brand;
                dsTemplateCode = objSalesData.GetAllTemplateData();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "SalesFromCode", "SalesToCode", "SkuCodeList" };
                    ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, Convert.ToInt16(EnumData.eTemplateCount.eSecondary + 1));
                    if (dsTemplateCode.Tables.Count > 3)
                        dsTemplateCode.Tables.RemoveAt(3);
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsTemplateCode, FilenameToexport, 3, strExcelSheetName);
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
            // PageBase.Errorhandling(ex);
        }
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
        Detail.Columns.Add("InvoiceNumber");
        Detail.Columns.Add("ToSalesChannelCode");
        Detail.Columns.Add("FromRetailerCode");
        Detail.Columns.Add(ErrorColumnName);
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
                bulkCopy.DestinationTableName = "TransactionUploadBulk";
                bulkCopy.ColumnMappings.Add("FromSalesChannelCode", "FromCode");
                bulkCopy.ColumnMappings.Add("ToSalesChannelCode", "ToCode");
                bulkCopy.ColumnMappings.Add("SKUCode", "SKUCode");
                bulkCopy.ColumnMappings.Add("Quantity", "Quantity");
                bulkCopy.ColumnMappings.Add("Serial1", "Serial#1");
                bulkCopy.ColumnMappings.Add("BatchNo", "BatchNo");
                bulkCopy.ColumnMappings.Add("BinCode", "StockBinType");
                bulkCopy.ColumnMappings.Add("TransUploadSession", "TransUploadSession");
                bulkCopy.ColumnMappings.Add("TransType", "TransType");
                bulkCopy.WriteToServer(dtTempTable);
                return true;
            }

        }
        catch (Exception ex)
        {
            return false;
        }
    }
   
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
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsReferenceCode, FilenameToexport);
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
        /*#CC04 END ADDED*/

    }

    

    
}
