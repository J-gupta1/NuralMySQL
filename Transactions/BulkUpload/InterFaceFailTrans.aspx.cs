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
using System.Reflection;
using System.Data.SqlClient;
using ZedService;
using System.IO;
/*Change Log:
 * DD-MMM-YYYY, Name, #CCXX - Description
 */


public partial class InterFaceFailTrans : PageBase
{
    DataTable dtNew = new DataTable();
    object objSum;
    int counter = 0;
    string strUploadedFileName = string.Empty;
    string strOriginalFileName = string.Empty;
                    
    string strMsg = string.Empty;
    UploadFile UploadFile = new UploadFile();
    List<String> lstDuplicate = new List<String>();
    DataSet dsErrorProne = new DataSet();
    
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (!IsPostBack)
            {
                ucDatePicker1.Date = PageBase.Fromdate;
                ucDatePicker2.Date = PageBase.ToDate;
            }
            }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    


    private void fncBindData()
    {
        try
        {
            DataSet DsFailRecords;
            using (SalesData objRD = new SalesData())
            {
                objRD.FromDate = Convert.ToDateTime(ucDatePicker1.Date);
                objRD.ToDate = Convert.ToDateTime(ucDatePicker2.Date);
                objRD.UserID = UserId;
                objRD.SalesChannelID = SalesChanelID;

                DsFailRecords = objRD.GetInterfaceSaleFailTrans();
                if (DsFailRecords != null )//&& DsFailRecords.Rows.Count > 0)
                {
                    
                        //string[] DsCol = new string[] { "RoleName", "UserName", "MenuName", "UserActivityDate", "LastLoginOn", "UserIP", "ServerIP" };
                        //DataTable DsCopy = new DataTable();
                        //DsFailRecords = DsFailRecords.DefaultView.ToTable(true, DsCol);
                    if (DsFailRecords.Tables.Count > 0 && DsFailRecords.Tables[0].Rows.Count>0)
                        {
                            try
                            {
                                String FilePath = Server.MapPath("../../");
                                string FilenameToexport = "FailTransactions";
                                PageBase.RootFilePath = FilePath;
                                PageBase.ExportToExecl(DsFailRecords, FilenameToexport);  
                            }
                            catch (Exception ex)
                            {
                                ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                            }
                        }
                        else
                        {
                            ucMessage1.ShowError(Resources.Messages.NoRecord);
                            
                        }
 
                    
                    
                }
                else
                {
                    ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                    //updgrid.Update(); 
                }
                
                
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

    bool PageValidate()
    {
        try
        {
              
                if (Convert.ToDateTime(ucDatePicker1.Date) > Convert.ToDateTime(ucDatePicker2.Date))
                {
                    ucMessage1.ShowInfo("From Date must be smaller than To date");
                    return false;
                }
            
            return true;
        }
        catch (Exception ex)
        {

            throw ex ;
        }
        }



  

    
    protected void btnDownLoad_Click(object sender, EventArgs e)
    {
        try
        {
            if (!PageValidate())
            {
                return;
            }
            
            
            fncBindData();
            //updMsg.Update();
        }

        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

    public DataTable GetBlankTableError()
    {
        DataTable Detail = new DataTable();
        Detail.Columns.Add("InvoiceNumber");
        Detail.Columns.Add("FromSalesChannelCode");
        Detail.Columns.Add("ToSalesChannelCode");
        Detail.Columns.Add("ReasonForInvalid");
        return Detail;
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtErrorTable = GetBlankTableError();
            DataTable dtError = new DataTable();
            HttpContext.Current.Session["PkeyColumns"] = null;
            string strKey = string.Empty;
            strOriginalFileName = FileUpload1.PostedFile.FileName;
            
            Int16 Upload = 0;
            Upload = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            
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
                            objValidateFile.ExcelFileNameInTable = "FailTransApiUpload";
                            objValidateFile.ValidateFile(false, out objDS, out objSL);
                            if (objValidateFile.Message != null && objValidateFile.Message.Trim() != "")
                                ucMessage1.ShowInfo(objValidateFile.Message);
                            else
                            {
                                ucMessage1.Visible = false;
                                bool blnIsUpload = true;
                                /*if (objSL.Count >= 1 && objSL.Keys.Count >= 1)
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
                                        //string strFileName = "invalidData" + DateTime.Now.Ticks;
                                        //ExportInExcel(objDS.Tables["DtExcelSheet"], strFileName);
                                        //hlnkInvalid.NavigateUrl = strExcelVirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
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
                                }*/

                                //if (objDS.Tables["DtDuplicateRecord"] != null && objDS.Tables["DtDuplicateRecord"].Rows.Count > 0)
                                //{
                                //    //hlnkDuplicate.Visible = true;
                                //    dsErrorProne.Merge(objDS.Tables["DtDuplicateRecord"]);
                                //    blnIsUpload = false;
                                //}
                                //if (objDS.Tables["DtBlankData"] != null && objDS.Tables["DtBlankData"].Rows.Count > 0)
                                //{
                                //    dsErrorProne.Merge(objDS.Tables["DtBlankData"]);
                                    
                                //    blnIsUpload = false;
                                //}
                                if (blnIsUpload)
                                {
                                    if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                                    {
                                        InsertData(objDS.Tables[0]);
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
                    ucMessage1.ShowInfo("File is empty! Some Mandatory columns has no required data!");
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }



    private void InsertData(DataTable dtGRN)
    {
        try
        {
            if (dtGRN.Rows.Count > 0)
            {
                DataColumn dcQuantity = new DataColumn();
                dcQuantity.DataType = typeof(System.Int32);
                dcQuantity.ColumnName = "QuantityNew";
                dtGRN.Columns.Add(dcQuantity);
                foreach (DataRow dr in dtGRN.Rows)
                {
                    dr["QuantityNew"] = Convert.ToInt32(dr["SKUQty"]);
                }
                dtGRN.AcceptChanges();
                objSum = dtGRN.Compute("sum(QuantityNew)", "");

                if (Convert.ToInt32(objSum) <= 0)
                {
                    ucMessage1.ShowInfo("Please Insert right Quantity");
                    return;
                }

                SaveRecord(dtGRN);
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }

    }


    private void SaveRecord(DataTable dt )
    {
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }
            if (! string.IsNullOrEmpty( strUploadedFileName) )
            {
                int intResult = 0;
                                

                string guid = Guid.NewGuid().ToString();
                
                dt.Columns.Add(AddColumn(guid, "TransUploadSession", typeof(System.String)));
                dt.AcceptChanges();

                if (dt.Rows.Count > 0)
                {
                    if (!BulkCopyUpload(dt))
                    {
                        ucMessage1.ShowError("Error Occured While transferring the data to the server");
                        return;
                    }

                }

                using (SalesData objP1 = new SalesData())
                {
                    
                    objP1.UserID = PageBase.UserId;
                    objP1.TransUploadSession = guid;
                    objP1.strOriginalFileName = strOriginalFileName;
                    objP1.strUploadedFileName = strUploadedFileName;
                    objP1.SalesChannelID = SalesChanelID;
                    intResult = objP1.ProcessInterFaceFailTrans();

                    if (objP1.ErrorDetailXML != null && objP1.ErrorDetailXML != string.Empty)
                    {
                        hlnkInvalid.Visible = true;
                        string strFileName = "InvalidData" + DateTime.Now.Ticks;
                        StringReader theReader = new StringReader(objP1.ErrorDetailXML);
                        dsErrorProne.ReadXml(theReader);
                        ExportInExcel(dsErrorProne, strFileName);
                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                        hlnkInvalid.Text = "Invalid Data";
                        ucMessage1.Visible = true;
                        ucMessage1.ShowInfo("Please click on Invalid data to check the error obtained");
                        /*#CC04 Added End */
                        // ucMessage1.XmlErrorSource = objP1.ErrorDetailXML;/*#CC04 Commented*/
                        return;
                    }
                    if (objP1.Error != null && objP1.Error != "")
                    {
                        ucMessage1.ShowError(objP1.Error);
                        return;
                    }
                    if (intResult == 2)
                    {
                        ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                        return;
                    }
                    
                    ucMessage1.ShowSuccess("Record uploaded successfuly. It will be processed in next 30 minutes.");

                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    public bool BulkCopyUpload(DataTable dtTempTable)
    {
        try
        {
            dtTempTable.Columns.Remove("CreatedOn");
            dtTempTable.Columns.Remove("SaleFromCode");
            dtTempTable.Columns.Remove("SaleFromName");
            dtTempTable.Columns.Remove("SaleTypeDesc");
            dtTempTable.Columns.Remove("FailReasonSale");
            dtTempTable.Columns.Remove("FailReasonQuantity");
            dtTempTable.Columns.Remove("FailReasonSaleSerial");
            dtTempTable.Columns.Remove("Error");

            //dtTempTable.Columns.Remove("");

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "InterfaceSaleFailReUpload";
                bulkCopy.ColumnMappings.Add("ID1", "ID1");
                bulkCopy.ColumnMappings.Add("ID2", "ID2");
                bulkCopy.ColumnMappings.Add("ID3", "ID3");
                bulkCopy.ColumnMappings.Add("InvoiceNumber", "InvoiceNumber");
                bulkCopy.ColumnMappings.Add("InvoiceDate", "InvoiceDate");
                bulkCopy.ColumnMappings.Add("ProductKeyword", "ProductKeyword");
                bulkCopy.ColumnMappings.Add("SKUCode", "SKUCode");
                bulkCopy.ColumnMappings.Add("SKUQty", "SKUQty");
                bulkCopy.ColumnMappings.Add("SaleType", "SaleType");
                bulkCopy.ColumnMappings.Add("SaleToCode", "RetailerCode");
                bulkCopy.ColumnMappings.Add("InterfaceRetailerName", "InterfaceRetailerName");
                bulkCopy.ColumnMappings.Add("InterfaceRetailerCountry", "InterfaceRetailerCountry");
                bulkCopy.ColumnMappings.Add("InterfaceRetailerState", "InterfaceRetailerState");
                bulkCopy.ColumnMappings.Add("InterfaceRetailerCity", "InterfaceRetailerCity");
                bulkCopy.ColumnMappings.Add("InterfaceRetailerAddress1", "InterfaceRetailerAddress1");
                bulkCopy.ColumnMappings.Add("InterfaceRetailerAddress2", "InterfaceRetailerAddress2");
                bulkCopy.ColumnMappings.Add("InterfaceRetailerPincode", "InterfaceRetailerPincode");
                bulkCopy.ColumnMappings.Add("InterfaceRetailerPhone", "InterfaceRetailerPhone");
                bulkCopy.ColumnMappings.Add("InterfaceRetailerMobile", "InterfaceRetailerMobile");
                bulkCopy.ColumnMappings.Add("InterfaceRetailerEmail", "InterfaceRetailerEmail");
                bulkCopy.ColumnMappings.Add("Serial", "Serial");
                bulkCopy.ColumnMappings.Add("Action", "Action");
                bulkCopy.ColumnMappings.Add("TransUploadSession", "TransUploadSession");

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
            using (SalesChannelData objSalesData = new SalesChannelData())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.eProductKey;
                objSalesData.SalesChannelID = PageBase.SalesChanelID;
                objSalesData.Brand = PageBase.Brand;
                dsTemplateCode = objSalesData.GetAllTemplateData();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
                    //string[] strExcelSheetName = { "SalesFromCode", "SalesToCode", "SkuCodeList" };
                    string[] strExcelSheetName = { "ProductKeyword" };
                    ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, 1);

                    //PageBase.ExportToExecl(dsTemplateCode, FilenameToexport, EnumData.eTemplateCount.eSecondary+1);
                    if (dsTemplateCode.Tables.Count > 3)
                        dsTemplateCode.Tables.RemoveAt(3);
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsTemplateCode, FilenameToexport, 1, strExcelSheetName);
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
            // PageBase.Errorhandling(ex);
        }
    }
   
   
}
