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
using DevExpress.Web.ASPxUploadControl;
using DevExpress.Web.ASPxClasses.Internal;
using System.IO;

public partial class Transactions_SapIntegration_InvoicePdfUpload : PageBase
{
    DataTable dtImageDataActual;
    DataTable dtImageData;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        DataTable FinalFileData = new DataTable();
        // DataSet ds = new DataSet();
        if (Session["Table"] != null)
        {
            FinalFileData = (DataTable)Session["Table"];
            FinalFileData.TableName = "Table1";
            dt = FinalFileData.Copy();
            // ds.Tables.Add(dt);
        }
        else
        {
            ucMsg.Visible = true;
            ucMsg.ShowInfo("Please Upload File!");
        }
        // XMLImage = ds.GetXml();
        using (clsEnquiryDetail ObjDetail = new clsEnquiryDetail())
        {
            if (dt.Rows.Count == 0)
            {
                ucMsg.Visible = true;
                ucMsg.ShowInfo("Please Upload File!");
            }
            else
            {
                using (ClsSAPIntegrationDNPSI objuploadfile = new ClsSAPIntegrationDNPSI())
                {
                    int resultimg = 1;
                    objuploadfile.SalesChannelId = PageBase.SalesChanelID;
                    objuploadfile.LoginUserId = PageBase.UserId;
                    objuploadfile.ReferenceType_id = 0;
                    objuploadfile.UploadImageData = dt;
                    resultimg = objuploadfile.SaveImgSaperateByProcess();
                    if (resultimg == 0)
                    {
                        PageBase objbase = new PageBase();
                        objbase.MoveFileFromTemp(FinalFileData);
                        ucMsg.Visible = true;
                        ucMsg.ShowSuccess("File Uploaded Successfully.!");
                        Session["FinalFileData"] = null;
                    }
                }
            }
            Session["Table"] = null;
        }
    }
    protected void UploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
    {
        try
        {
            e.CallbackData = SavePostedFiles(e.UploadedFile);
        }
        catch (Exception ex)
        {
            e.IsValid = false;
            e.ErrorText = ex.Message;
        }
    }
    string SavePostedFiles(UploadedFile uploadedFile)
    {


        string strFilePath;

        strFilePath = ZedService.Utility.ZedServiceUtil.GetUploadFilePathV2(Convert.ToDateTime(System.DateTime.Now), uploadedFile.FileName);
        if (!uploadedFile.IsValid)
            return string.Empty;
        FileInfo fileInfo = new FileInfo(uploadedFile.FileName);
        string strTicks = System.DateTime.Now.Ticks.ToString();

        string strFileUploadedName = fileInfo.Name;

        string strTempPath = Server.MapPath("~/Excel/Upload/SAPIntegration/UploadedPdfInvoice/");
        if (!Directory.Exists(Server.MapPath(strFilePath)))
            Directory.CreateDirectory(Server.MapPath(strFilePath));


        uploadedFile.SaveAs(strTempPath + strFileUploadedName);
        if (Session["Table"] == null)
        {
            string removeextention = strFileUploadedName.Replace(".pdf", "");
            dtImageDataActual = new DataTable();
            dtImageDataActual = CreateImageDataTable();
            DataRow dr = dtImageDataActual.NewRow();
            dr["FileLocation"] = strFilePath.Replace("../../", "/");
            dr["ImageTypeId"] = 14;
            dr["ImageRelativePath"] = strTempPath + strFileUploadedName;
            dr["TempFileLocation"] = strTempPath + strFileUploadedName;
            dr["ImageTypeName"] = "InvoicePdf";
            dr["ImageName"] = removeextention;
            dtImageDataActual.Rows.Add(dr);
            dtImageDataActual.AcceptChanges();
            Session["Table"] = dtImageDataActual;

        }
        else
        {
            string removeextention = strFileUploadedName.Replace(".pdf", "");
            dtImageDataActual = (DataTable)Session["Table"];
            DataRow dr = dtImageDataActual.NewRow();

            dr["FileLocation"] = strFilePath.Replace("../../", "/");
            dr["ImageTypeId"] = 14;
            dr["ImageRelativePath"] = strTempPath + strFileUploadedName;
            dr["TempFileLocation"] = strTempPath + strFileUploadedName;
            dr["ImageTypeName"] = "InvoicePdf";
            dr["ImageName"] = removeextention;
            dtImageDataActual.Rows.Add(dr);
            dtImageDataActual.AcceptChanges();
            Session["Table"] = dtImageDataActual;


        }

        string fileLabel = fileInfo.Name;
        string fileLength = uploadedFile.FileBytes.Length / 1024 + "K";

        return string.Format("{0} ({1})|{2}", fileLabel, fileLength, VirtualPath + "/Excel/Upload/SAPIntegration/UploadedPdfInvoice/" + strFileUploadedName);

    }
    DataTable CreateImageDataTable()
    {
        dtImageData = new DataTable();
        DataColumn dc = new DataColumn("CtrlID");
        dc.DataType = System.Type.GetType("System.Int32");
        dc.AutoIncrement = true;
        dc.AutoIncrementSeed = 1;
        dtImageData.Columns.Add(dc);
        dtImageData.Columns.Add("FileLocation", typeof(string));
        dtImageData.Columns.Add("TempFileLocation", typeof(string));
        dtImageData.Columns.Add("ImageTypeId", typeof(Int16));
        dtImageData.Columns.Add("ImageRelativePath", typeof(string));
        dtImageData.Columns.Add("ImageName", typeof(string));
        dtImageData.Columns.Add("ImageTypeName", typeof(string));
        return dtImageData;
    }
}