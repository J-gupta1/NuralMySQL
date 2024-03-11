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
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;
using ZedService;
using System.Xml;
using System.IO;
/*
 * DD MMM YYYY , Name, #CCXX, Desc
 */
public partial class Transactions_StockTransferBCP : PageBase
{
    DataTable dtNew = new DataTable();
    string strPrimarySessionName = "StockTransferSession";
    string strUploadedFileName = string.Empty;
    string strOriginalFileName = string.Empty;
    string guid = string.Empty;
    string strMsg = string.Empty;
    UploadFile UploadFile = new UploadFile();
    

    protected void Page_Load(object sender, EventArgs e)
    {
        Server.ScriptTimeout = 1000;
        try
        {
            ucMsg.ShowControl = false;
            if (!IsPostBack)
            {

            }
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
            //UploadFile.RootFolerPath = PageBase.strExcelPhysicalUploadPathSB;
            Upload = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            strOriginalFileName = FileUpload1.FileName;
            if (Upload == 1)
            {
                OpenXMLExcel objexcel = new OpenXMLExcel();
                DataSet DsExcel = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);
                if (DsExcel != null && DsExcel.Tables.Count > 0 && DsExcel.Tables[0].Rows.Count > 0)
                {

                    if (DsExcel.Tables[0].Rows.Count > Convert.ToInt32(PageBase.ValidExcelRows))
                        ucMsg.ShowInfo("Number of rows in excel template should not be more than " + PageBase.ValidExcelRows);
                    else
                    {
                        ValidateUploadFile objValidateFile = new ValidateUploadFile(); 
                        {
                            DataSet objDS = DsExcel;
                            //DataTable dt1 = DsExcel.Tables[0];
                            //SortedList objSL = new SortedList();
                            //SortedList objSLCorrData = new SortedList();
                            objValidateFile.UploadedFileName = strUploadedFileName;
                            objValidateFile.ExcelFileNameInTable = "StockTransferSB";
                            

                            if (!objValidateFile.IsValidFileSchema(out objDS))
                            {
                                if (objValidateFile.Message != null && objValidateFile.Message.Trim() != "")
                                    ucMsg.ShowInfo(objValidateFile.Message);
                                else
                                    ucMsg.ShowInfo("Some error occured, please contact administrator.");
                            }
                            else
                            {

                                //byte isSuccess = UploadFile.uploadValidExcel(ref DsExcel, "StockTransferSB");
                                //if (isSuccess==1)
                                SaveTransfer(DsExcel);
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

    private void SaveTransfer(DataSet DsExcelDetail)
    {
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }

            int intResult = 0;
            DataTable Tvp = new DataTable();

            Tvp = GettvpTableStockTransferSB();

            guid = Guid.NewGuid().ToString();
            
            DsExcelDetail.Tables[0].Columns.Add(AddColumn(guid, "TransUploadSession", typeof(System.String)));
            DsExcelDetail.Tables[0].Columns.Add(AddColumn("8", "TransType", typeof(System.Int32)));
            DsExcelDetail.Tables[0].AcceptChanges();
            if (DsExcelDetail.Tables[0].Rows.Count > 0)
            {
                if (!BulkCopyUpload(DsExcelDetail.Tables[0]))
                {
                    ucMsg.ShowError("Please check the help worksheet of template file and change upload data accordingly.");
                    return;
                }

            }


            string Error;
            DataSet DsError;
            intResult = InsertStockTransferSBBCP(
                PageBase.UserId, 8, 0, 1,  out DsError, out Error);

            if (DsError != null && DsError.Tables.Count > 0)
            {
                CreateInvalidDataLink(DsError);
                ucMsg.ShowError(Error);
                return;
            }
            else if (Error != null && Error != "")
            {
                ucMsg.ShowError(Error);
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
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    private DataTable GettvpTableStockTransferSB()
    {
        DataTable Detail = new DataTable();
        Detail.Columns.Add("FromCode");
        Detail.Columns.Add("ToCode");
        Detail.Columns.Add("STNNo");
        Detail.Columns.Add("DocketNo");
        Detail.Columns.Add("TransferDate");
        Detail.Columns.Add("SKUCode");
        Detail.Columns.Add("Quantity");
        Detail.Columns.Add("Serial#1");
        Detail.Columns.Add("Serial#2");
        Detail.Columns.Add("Serial#3");
        Detail.Columns.Add("Serial#4");
        Detail.Columns.Add("BatchNo");
        Detail.Columns.Add("BinCode");
        return Detail;
    }
    private Int32 InsertStockTransferSBBCP( int UserID, int eEntryType,
        int SalesChannelID, int ComingFrom, out DataSet dsError , out string Error)
    {
        dsError = new DataSet();
        Error = "";
        string strConnectionString = ConfigurationManager.ConnectionStrings["AppConString"].ConnectionString;
        SqlConnection objCon = new SqlConnection(strConnectionString);
        objCon.Open();
        try
        {
            SqlParameter[] SqlParam = new SqlParameter[10];
            SqlParam[0] = new SqlParameter("@TransUploadSession", guid);
            SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
            SqlParam[3].Direction = ParameterDirection.Output;
            SqlParam[4] = new SqlParameter("@userid", UserID);
            SqlParam[5] = new SqlParameter("@EntryType", eEntryType);
            SqlParam[6] = new SqlParameter("@SalesChannelId", SalesChannelID);
            SqlParam[7] = new SqlParameter("@ComingFrom", ComingFrom);
            SqlParam[8] = new SqlParameter("@OriginalFileName", strOriginalFileName);
            SqlParam[9] = new SqlParameter("@UniqueFileName", strUploadedFileName);

            SqlCommand objComm = new SqlCommand("PrcStockTransferSB", objCon);
            objComm.CommandType = CommandType.StoredProcedure;
            objComm.Parameters.AddRange(SqlParam);
            objComm.CommandTimeout = 900;
            objComm.ExecuteNonQuery();

            string ErrorDetailXML;
            Int16 IntResultCount = Convert.ToInt16(SqlParam[1].Value);
            if (SqlParam[3].Value.ToString() != "")
            {
                ErrorDetailXML = SqlParam[3].Value.ToString();
                dsError.ReadXml(new XmlTextReader(new StringReader(ErrorDetailXML)));

            }
            else
            {
                ErrorDetailXML = null;
            }
            Error = Convert.ToString(SqlParam[2].Value);

            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (objCon.State != ConnectionState.Closed)
            {
                objCon.Close();
                objCon.Dispose();
            }
        }
    }
    private void CreateInvalidDataLink(DataSet dsResult)
    {
        hlnkInvalid.Visible = true;
        strUploadedFileName = "InvalidData" + DateTime.Now.Ticks;
        PageBase.ExportInExcel(dsResult, strUploadedFileName);
        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strUploadedFileName + ".xlsx";
        hlnkInvalid.Text = "Invalid Data";

    }

    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        ClearForm();

    }

    void ClearForm()
    {
       
        ucMsg.Visible = false;
        
    }

    

    protected void DwnldReferenceCodeTemplate_Click(object sender, EventArgs e)
    {

        try
        {

            DataSet dsReferenceCode = new DataSet();
            DataSet Ds = new DataSet();
            using (SalesChannelData objSalesData = new SalesChannelData())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.eStockTransfer;
                objSalesData.SalesChannelID = PageBase.SalesChanelID;
                objSalesData.Brand = PageBase.Brand;
                dsReferenceCode = objSalesData.GetAllTemplateData();

                if (PageBase.SalesChanelID != 0)
                {
                    dsReferenceCode.Tables[0].DefaultView.RowFilter = "SalesChannelCode='" + PageBase.SalesChanelCode + "'";
                }

                Ds.Merge(dsReferenceCode.Tables[0].DefaultView.ToTable());
                if (PageBase.SalesChanelID != 0)
                {
                    dsReferenceCode.Tables[1].DefaultView.RowFilter = "ParentSalesChannelID=" + PageBase.SalesChanelID;
                }
                Ds.Merge(dsReferenceCode.Tables[1].DefaultView.ToTable());
                Ds.Tables[1].Columns.Remove("ParentSalesChannelID");

                Ds.Merge(dsReferenceCode.Tables[2]);






                if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "TransferFromCode", "TransferToCode", "SkuCodeList" };

                    ChangedExcelSheetNames(ref Ds, strExcelSheetName, Convert.ToInt16(EnumData.eTemplateCount.ePrimarysales1 + 1));
                    if (Ds.Tables.Count > 3)
                        Ds.Tables.Remove(Ds.Tables[3]);
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(Ds, FilenameToexport, 3, strExcelSheetName);
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
            PageBase.Errorhandling(ex);
        }
    }


    private void ExportInExcel(DataSet DsError, string strFileName)
    {
        if (DsError != null && DsError.Tables.Count > 0)
        {
            PageBase.ExportToExeclV2(DsError, strFileName, DsError.Tables.Count);
        }
    }
    protected void rdModelList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdModelList.SelectedValue == "1")
            Response.Redirect("~/Transactions/SalesChannelSB/Interface/PrimarySalesInterfaceForAddV1.aspx");
    }
    public DataTable GetBlankTableError()
    {
        DataTable Detail = new DataTable();
        Detail.Columns.Add("InvoiceNumber");
        Detail.Columns.Add("WarehouseCode");
        Detail.Columns.Add("SalesChannelCode");
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
                bulkCopy.BulkCopyTimeout = 500;
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "TransactionUploadBulk";
                bulkCopy.ColumnMappings.Add("FromCode", "FromCode");
                bulkCopy.ColumnMappings.Add("ToCode", "ToCode");
                bulkCopy.ColumnMappings.Add("STNNo", "TransNumber");
                bulkCopy.ColumnMappings.Add("DocketNo", "OrderNumber");
                bulkCopy.ColumnMappings.Add("TransferDate", "TransDate");
                bulkCopy.ColumnMappings.Add("SKUCode", "SKUCode");
                bulkCopy.ColumnMappings.Add("Quantity", "Quantity");
                bulkCopy.ColumnMappings.Add("Serial#1", "Serial#1");
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
            WriteLogToTextFile(ex.Message);
            return false;
        }
    }
    //#CC02 START ADDED
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
                    //PageBase.ExportToExecl(dsReferenceCode, FilenameToexport, EnumData.eTemplateCount.ePrimarysales1);
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


    }
    //#CC02 END ADDED

    public static void WriteLogToTextFile(string Message)
    {
        try
        {
            string FilePath = ConfigurationManager.AppSettings["PhysicalPath"].ToString() + "\\LogFile\\" + DateTime.Today.ToString("yyyy") + "\\" + DateTime.Today.ToString("MMM"); //AppDomain.CurrentDomain.BaseDirectory + "LogFile\\";

            System.IO.DirectoryInfo dr = new System.IO.DirectoryInfo(FilePath);
            if (!dr.Exists)
            {
                dr.Create();
            }
            if (!System.IO.File.Exists(FilePath + "\\Log" + DateTime.Today.ToString("dd-MMM-yyy") + ".txt"))
            {
                System.IO.File.Create(FilePath + "\\Log" + DateTime.Today.ToString("dd-MMM-yyy") + ".txt").Dispose();
            }
            using (System.IO.StreamWriter sWriter = System.IO.File.AppendText(FilePath + "\\Log" + DateTime.Today.ToString("dd-MMM-yyy") + ".txt"))
            {
                sWriter.WriteLine("\r\n{0}", Message);
                sWriter.WriteLine("--------------------------------------------------------------");
                sWriter.Flush();
            }
        }
        catch (Exception ex)
        { }
    }
}
