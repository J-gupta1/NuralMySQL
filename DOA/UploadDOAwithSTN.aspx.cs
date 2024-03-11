/*
 * ====================================================Note====================================================
 * This Interface is copied from ZedsalesV5 and its name is changed from UploadDOA to UploadDOAwithSTn because same file exists which is created for motorola. In ZedSalesV4 this file is created for Comio client and fucntionality of both the clients are different. 
 * ============================================================================================================
 * Changes Log
 * DD-MMM-YYYY, Name, #CCXX, Description.
 * 
 
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
using System.IO;
using System.Text.RegularExpressions;
using ZedService;
using System.Net;
using System.Data.SqlClient;


public partial class DOA_UploadDOAwithSTN : PageBase
{
    string strUploadedFileName = string.Empty;
    UploadFile UploadFile = new UploadFile();
    ZedService.OpenXMLExcel objexcel = new ZedService.OpenXMLExcel();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {

        DataSet objDS = new DataSet();
        try
        {
            byte isSuccess = 1;
            Int16 UploadCheck = 0;
            String RootPath = Server.MapPath("~/");
            UploadFile.RootFolerPath = RootPath;

            UploadCheck = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            ViewState["TobeUploaded"] = strUploadedFileName;
            if (UploadCheck == 1)
            {
                UploadFile.UploadedFileName = strUploadedFileName;
                UploadFile.UploadValidationType = EnumData.eUploadExcelValidationType.eRetailerUpload;
                isSuccess = UploadFile.uploadValidExceluploadDOA(ref objDS, "UploadDOAWithSTN");
                if (isSuccess == 0)
                {
                    uclblMessage.ShowInfo(UploadFile.Message);
                    return;
                }
                else
                {
                    int intError = 0;
                    if (objDS.Tables[0].Columns.Contains("Error"))
                    {
                        intError = 1;
                    }
                    if (!objDS.Tables[0].Columns.Contains("Error"))
                    {
                        objDS.Tables[0].Columns.Add("Error", typeof(string));
                    }
                    foreach (DataRow dr in objDS.Tables[0].Rows)
                    {
                        if (IsDate(dr["DOACertificateDate"].ToString().Trim()) == false)
                        {
                            dr["Error"] = "Invalid DOA Certificate Date";
                            intError = 1;
                        }
                    }

                    if (intError == 1)
                    {
                        isSuccess = 2;
                    }
                    if (intError == 0)
                    {
                        objDS.Tables[0].Columns.Remove("Error");
                    }
                    switch (isSuccess)
                    {
                        case 0:
                            uclblMessage.ShowInfo(UploadFile.Message);
                            break;
                        case 2:

                            uclblMessage.XmlErrorSource = objDS.GetXml();
                            break;
                        case 1:

                            uclblMessage.Visible = false;
                            InsertData(objDS.Tables[0]);
                            break;
                    }



                }
            }
            else
            {
                uclblMessage.ShowInfo(Resources.Messages.UploadXlxs);
            }
        }
        catch (Exception ex)
        {
            uclblMessage.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    public bool IsDate(string theValue)
    {
        try
        {
            DateTime dateValue;
            if (DateTime.TryParse(theValue, out dateValue))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        catch
        {
            return false;
        }
    }
    public bool BulkCopyDumpDOAUpload(DataTable dtTempTable)
    {
        try
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 10000;
                bulkCopy.DestinationTableName = "DumpDOAUpload";
                bulkCopy.BulkCopyTimeout = 300;
                bulkCopy.ColumnMappings.Add("IMEINumber", "IMEI");
                bulkCopy.ColumnMappings.Add("DOACertificateGenerateState", "DOACertificateGenerateState");
                bulkCopy.ColumnMappings.Add("ServiceCenterCode", "ServiceCenterCode");
                bulkCopy.ColumnMappings.Add("ServiceCenterName", "ServiceCenterName");
                bulkCopy.ColumnMappings.Add("DOACertificateDate", "DOACertificateDate");
                bulkCopy.ColumnMappings.Add("DOACertificateNumber", "DOACertificateNumber");
                bulkCopy.ColumnMappings.Add("SessionID", "WAGuid");
                bulkCopy.WriteToServer(dtTempTable);
                return true;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    public void InsertData(DataTable dt)
    {
        try
        {
            string guid = Guid.NewGuid().ToString();
            dt.Columns.Add(AddColumn(guid, "SessionID", typeof(System.String)));
            DataTable dtTvp = new DataTable();
            dtTvp.Columns.Add("IMEI");
            dtTvp.Columns.Add("DOACertificateGenerateState");
            dtTvp.Columns.Add("SVCCode");
            dtTvp.Columns.Add("SVCName");
            dtTvp.Columns.Add("DOACertificateDate");
            dtTvp.Columns.Add("DOACertificateNumber");
            dtTvp.Columns.Add("RowNum");
            dtTvp.AcceptChanges();
            if (BulkCopyDumpDOAUpload(dt) == true)
            {
                clsDoaReport obj = new clsDoaReport();
                obj.SessionID = Convert.ToString(dt.Rows[0]["SessionID"]);
                obj.dtTvp = dtTvp;
                DataSet dsResult = new DataSet();
                dsResult = obj.SaveDOAUpload();
                int result = obj.intOutParam;
                if (result == 0)
                {

                    uclblMessage.ShowSuccess("DOA Data uploaded successfully.");

                }
                else if (result == 1 && dsResult != null && dsResult.Tables.Count > 0)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        uclblMessage.XmlErrorSource = dsResult.GetXml();

                    }
                }

            }
            else
            {
                uclblMessage.ShowInfo("Error in saving data.");
            }
        }
        catch (Exception ex)
        {
            uclblMessage.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
}