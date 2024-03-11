/*
 27-Oct-2017, Balram Jha, #CC01, Displayed outerror from procedure call
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
using BusinessLogics;
using System.Collections;
using System.Data.SqlClient;
using ZedService;

public partial class Masters_SalesMan_BulkRetailerTransferUpload : PageBase
{
    #region Upload Variables
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    UploadFile UploadFile = new UploadFile();
    OpenXMLExcel objexcel = new OpenXMLExcel();
    string strPSalt, strPassword;
    #endregion
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
            #region uploadTableSchema
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
                isSuccess = UploadFile.uploadValidExcelRetailer(ref objDS, "BulkRetailerTransferUpload");
                int intError = 0;
                if (objDS.Tables[0].Columns.Contains("Error"))
                {
                    intError = 1;
                }
                if (!objDS.Tables[0].Columns.Contains("Error"))
                {
                    objDS.Tables[0].Columns.Add("Error", typeof(string));
                }

                if (intError == 1)
                {
                    isSuccess = 2;
                }
                if (intError == 0)
                {
                    objDS.Tables[0].Columns.Remove("Error");
                    ///Btnsave.Visible = true;
                }
                /* #CC02 Add End */
                switch (isSuccess)
                {
                    case 0:
                        ucMessage.ShowInfo(UploadFile.Message);
                        break;
                    case 2:

                        string strFileName = "InvalidData" + DateTime.Now.Ticks;
                        DataView dvError = objDS.Tables[0].DefaultView;
                        dvError.RowFilter = "Error<>''";
                        DataTable dtError = dvError.ToTable();
                        DataSet dsError = new DataSet();
                        dsError.Tables.Add(dtError);
                        ExportInExcel(dsError, strFileName);
                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                        hlnkInvalid.Text = "Invalid Data";
                        ucMessage.ShowInfo("There is error in data. Please refer Invalid Data link. No Data processed.");
                        break;
                    case 1:
                        SaveData(objDS.Tables[0]);
                        ///InsertData(objDS);
                        break;
                }
            }
            else if (UploadCheck == 2)
            {
                ucMessage.ShowInfo(Resources.Messages.UploadXlxs);
            }
            else if (UploadCheck == 3)
            {
                ucMessage.ShowInfo(Resources.Messages.SelectFile);
            }
            #endregion uploadTableSchema

        }
        catch (Exception ex)
        {
            ucMessage.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
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
            ucMessage.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }

    protected void DwnldReferenceCode_Click(object sender, EventArgs e)
    {
        try
        {
            //using (SalesmanData objSalesMan = new SalesmanData())
            using (SalesmanData objSalesMan = new SalesmanData())
            {
                objSalesMan.SalesChannelID = Convert.ToInt32(PageBase.SalesChanelID);
                DataSet ds = objSalesMan.GetRetailerBulkUploadTransfer();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "RetailerData";
                        if (ds.Tables[1] != null)
                            ds.Tables[1].TableName = "SalesChannelData";
                        if (ds.Tables[2] != null)
                            ds.Tables[2].TableName = "TSMData";
                        string FilePath = Server.MapPath("~/");
                        string FilenameToexport = "ReferenceCodeList";
                        PageBase.RootFilePath = FilePath;
                        PageBase.ExportToExecl(ds, FilenameToexport, EnumData.eTemplateCount.eRetailer + 2);
                    }
                }
            }

        }
        catch (Exception ex)
        {
            ucMessage.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            //PageBase.Errorhandling(ex);
        }
    }
    public void SaveData(DataTable dt)
    {
        try
        {
            string guid = Guid.NewGuid().ToString();
            dt.Columns.Add(AddColumn(guid, "SessionID", typeof(System.String)));
            dt.Columns.Add(AddColumn(Convert.ToString(PageBase.UserId), "CreatedBy", typeof(int)));

            if (UploadRetailerTransferBCP(dt) == true)
            {
                SalesmanData obj = new SalesmanData();
                
                obj.SessionID = Convert.ToString(dt.Rows[0]["SessionID"]);
                obj.UserID = PageBase.UserId;
                obj.SalesChannelID = Convert.ToInt32(PageBase.SalesChanelID);
                DataSet dsResult = new DataSet();
                dsResult = obj.SaveRetailerBulkTransfer();
                int result = obj.intOutParam;
                if (result == 0)
                {
                    hlnkInvalid.Text = "";
                    hlnkInvalid.Visible = false;
                    ucMessage.ShowSuccess("Data uploaded successfully.");

                }
                else if (result == 1 && dsResult != null && dsResult.Tables.Count > 0)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        hlnkInvalid.Visible = true;
                        string strFileName = "InvalidData" + DateTime.Now.Ticks;
                        ucMessage.ShowInfo("There is error in data. Please refer Invalid Data link. No Data processed.");
                        ExportInExcel(dsResult, strFileName);
                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                        hlnkInvalid.Text = "Invalid Data";
                        //ucMsg.XmlErrorSource = objSales.XMLList;
                    }
                }
                else if (!string.IsNullOrEmpty(obj.Error))//#CC01 added
                    ucMessage.ShowError(obj.Error);

            }
            else
            {
                ucMessage.ShowInfo("Error in saving data.");
            }
        }
        catch (Exception ex)
        {
            ucMessage.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }


    public bool UploadRetailerTransferBCP(DataTable dtUpload)
    {
        try
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "BulkRetailerTransfer";
                bulkCopy.ColumnMappings.Add("RetailerCode", "RetailerCode");
                bulkCopy.ColumnMappings.Add("NewSalesChannelCode", "NewSalesChannelCode");
                bulkCopy.ColumnMappings.Add("NewTSMCode", "NewTSMCode");
                bulkCopy.ColumnMappings.Add("NewSalesManCode", "NewSalesManCode");
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

    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("BulkRetailerTransferUpload.aspx", false);
        }
        catch (Exception ex)
        {
            ucMessage.ShowInfo(ex.Message.ToString());
        }
    }
}
