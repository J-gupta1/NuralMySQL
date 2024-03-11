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
 * Created By : Sumit Maurya 
 * Created On: 28-May-2018
 * Module :  DOA Credit Note Upload.
 * ====================================================================================================
 * Change Log :
 * DD-MMM-YYYY, Name, #CC01, Description. 
 *  ====================================================================================================
*/

#endregion

using BussinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZedService;
using DataAccess;
using BusinessLogics;
using System.Collections;

public partial class DOA_DOACreditNoteUpload : PageBase
    {
        OpenXMLExcel objexcel = new OpenXMLExcel();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {

            try
            {
            DataTable dtError = new DataTable();

            UploadFile UploadFile = new UploadFile();
            string strUploadedFileName = string.Empty;
            HttpContext.Current.Session["PkeyColumns"] = null;
            string strKey = string.Empty;
            hlnkInvalid.Visible = false;
            byte isSuccess = 1;
            Int16 Upload = 0;
            Upload = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            ViewState["TobeUploaded"] = strUploadedFileName;
            if (Upload == 1)
            {
                OpenXMLExcel objexcel = new OpenXMLExcel();
                DataSet DsExcel = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);
                if (DsExcel != null && DsExcel.Tables.Count > 0 && DsExcel.Tables[0].Rows.Count > 0)
                {
                    if (DsExcel.Tables[0].Rows.Count > Convert.ToInt32(PageBase.ValidExcelRows))
                        uclblMessage.ShowInfo("Limit Crossed");
                    else
                    {
                        ValidateUploadFile objValidateFile = new ValidateUploadFile();
                        {
                            DataSet objDS = DsExcel;
                            DataTable dt1 = DsExcel.Tables[0];
                            SortedList objSL = new SortedList();
                            SortedList objSLCorrData = new SortedList();
                            objValidateFile.UploadedFileName = strUploadedFileName;
                            objValidateFile.ExcelFileNameInTable = "UploadCreditNoteNumber";
                            string strExcelName = "UploadCreditNoteNumber";
                            String RootPath = Server.MapPath("../");
                            UploadFile.RootFolerPath = RootPath;

                            isSuccess = UploadFile.uploadValidExcel(ref objDS, strExcelName);
                            switch (isSuccess)
                            {
                                case 0:
                                    uclblMessage.ShowInfo(UploadFile.Message);
                                    
                                    break;
                                case 2:
                                   /* uclblMessage.ShowInfo(Resources.Messages.CheckErrorGrid);*/
                                    hlnkInvalid.Visible = true;
                                    string strFileName = "InvalidData" + DateTime.Now.Ticks;
                                    ExportInExcel(objDS, strFileName);
                                    hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                    hlnkInvalid.Text = "Invalid Data";
                                    
                                    break;
                                case 1:
                                     string guid = Guid.NewGuid().ToString();               
                                    objDS.Tables[0].Columns.Add(AddColumn(guid, "WAGuid", typeof(System.String)));
                                    InsertData(objDS);
                                    break;
                                case 3:
                                    uclblMessage.ShowInfo(UploadFile.Message);
                                    break;


                            }

                        }
                    }
                }
            }

               
                else if (Upload == 2)
                {
                    uclblMessage.ShowInfo(Resources.Messages.UploadXlxs);

                }
                else if (Upload == 3)
                {
                    uclblMessage.ShowInfo(Resources.Messages.SelectFile);
                }

                

            }
            catch (Exception ex)
            {

                uclblMessage.ShowInfo(ex.Message.ToString());
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
                uclblMessage.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }
        }

        public void InsertData(DataSet ds)
        {
            try
            {
                if (UploadBCPData(ds.Tables[0]))
                {
                    using (clsDoaReport objreport = new clsDoaReport())
                    {
                        objreport.LoginUserId = PageBase.UserId;
                        objreport.SalesChannelId = PageBase.SalesChanelID;
                        objreport.WAGuid = Convert.ToString(ds.Tables[0].Rows[0]["WAGuid"]);
                        ds = objreport.UploadDOACreditNote();
                        if (ds != null && objreport.OutParam == 1)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    uclblMessage.XmlErrorSource = ds.GetXml();
                                    return;
                                }
                            }
                        }
                        else if (objreport.OutParam == 0)
                        {
                            uclblMessage.ShowSuccess("Records uploaded successfully.");
                            return;
                        }
                    }
                }
                else
                {
                    uclblMessage.ShowInfo("Unable to upload data!! Please try after some time.");
                }
            }
            catch (Exception ex )
            {
                
                throw;
            }
        }

        public bool UploadBCPData(DataTable dtUpload)
        {
            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
                {
                    bulkCopy.BatchSize = 10000;
                    bulkCopy.DestinationTableName = "DumpDOAUploadApproveReject";
                    bulkCopy.BulkCopyTimeout = 300;
                    bulkCopy.ColumnMappings.Add("IMEI", "IMEI");
                    bulkCopy.ColumnMappings.Add("DOACertificateNumber", "DOACertificateNumber");
                    bulkCopy.ColumnMappings.Add("CreditNoteNumber", "CreditNoteNumber");
                    bulkCopy.ColumnMappings.Add("WAGuid", "SessionId");
                    bulkCopy.WriteToServer(dtUpload);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
       
}
