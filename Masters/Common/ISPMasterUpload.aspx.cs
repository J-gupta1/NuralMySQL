/*
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX,Description.
 * 30-Oct-2018, Sumit Maurya, #CC01, UserID provided in procedure (Done for Motorola).
 * 02-Oct-2018, Sumit Maurya, #CC02, New Code added to generate dynamic template (Done for motorola).
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using BussinessLogic;
using DataAccess;
using System.Data;
using System.Collections;
using BusinessLogics;
using ZedService;

public partial class Masters_Common_ISPMasterUpload : PageBase
{
    Int16 CompanyID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        lblUploadMsg.Text = "";
      //  CompanyID = PageBase.CompanyID;
    }
    #region Upload Variables
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    bool blnIsValid;
    UploadFile UploadFile = new UploadFile();
    List<String> lstDuplicate = new List<String>();
    DataSet dsErrorProne = new DataSet();
    #endregion

    protected void LnkDownloadRefCode_Click(object sender, EventArgs e)
    {

        DataSet DsSku = new DataSet();
        try
        {
            using (RetailerData ObjSales = new RetailerData())
            {
                DsSku = ObjSales.RetailersCodeDownload();
            };
            if (DsSku.Tables.Count > 0)
            {
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "Retailer Code list";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(DsSku, FilenameToexport, EnumData.eTemplateCount.eTarget);
            }
        }
        catch (Exception ex)
        {

            PageBase.Errorhandling(ex);
            ucMsg.ShowError(ex.ToString(), GlobalErrorDisplay());

        }

    }
    public Int16 IsExcelFile(System.Web.UI.WebControls.FileUpload UploadControl, ref string FileName)
    {
        Int16 MessageforValidation = 0;
        try
        {

            if (UploadControl.HasFile)
            {
                if ((Path.GetExtension(UploadControl.FileName).ToLower() == ".xlsx") || (Path.GetExtension(UploadControl.FileName).ToLower() == ".xls"))
                {
                    try
                    {
                        double dblMaxFileSize = Convert.ToDouble(PageBase.ValidExcelLength);
                        int intFileSize = UploadControl.PostedFile.ContentLength;  //file size is obtained in bytes
                        double dblFileSizeinKB = intFileSize / 1024.0; //convert the file size into kilobytes
                        if ((dblFileSizeinKB > dblMaxFileSize))
                        {
                            MessageforValidation = 4;
                            return MessageforValidation;
                        }
                        strUploadedFileName = PageBase.importExportExcelFileName;
                        //UploadControl.SaveAs(RootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
                        UploadControl.SaveAs(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);

                        MessageforValidation = 1;
                        FileName = strUploadedFileName;
                        return MessageforValidation;
                    }
                    catch (Exception objEx)
                    {

                        MessageforValidation = 0;
                        throw new Exception(objEx.Message.ToString());
                        return MessageforValidation;

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
            //return MessageforValidation;
            throw objHttpException;
        }
        catch (Exception ex)
        {
            //return MessageforValidation;
            throw ex;

        }


    }
    public DataTable GetBlankTableError()
    {
        DataTable Detail = new DataTable();
        Detail.Columns.Add("RetailerCode");
        Detail.Columns.Add("ISPCode");
        Detail.Columns.Add("ISPName");
        Detail.Columns.Add("ISPMobile");
        return Detail;
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {

            GridISP.Visible = false;
            DataTable dtErrorTable = GetBlankTableError();
            DataTable dtError = new DataTable();
            HttpContext.Current.Session["PkeyColumns"] = null;
            string strKey = string.Empty;
           // ClearForm();
            hlnkInvalid.Visible = false;
            //hlnkDuplicate.Visible = false;
            //hlnkBlank.Visible = false;
            // String RootPath = Server.MapPath("../../");
            // UploadFile.RootFolerPath = RootPath;
            Int16 Upload = 0;
            Upload = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            ViewState["TobeUploaded"] = strUploadedFileName;
            if (Upload == 1)
            {
                OpenXMLExcel objexcel = new OpenXMLExcel();
                DataSet DsExcel = objexcel.ImportExcelFile(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);

                //if (DsExcel.Tables[0].Columns.Contains("RetailerCode"))
                //{
                //    DsExcel.Tables[0].Columns["RetailerCode"].ColumnName = "ToSalesChannelCode";
                //    DsExcel.Tables[0].AcceptChanges();
                //}

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
                            objValidateFile.ExcelFileNameInTable = "ISPUpload";
                            //objValidateFile.RootFolerPath = RootPath;
                            objValidateFile.ValidateFile(false, out objDS, out objSL);
                            if (objValidateFile.Message != null && objValidateFile.Message.Trim() != "")
                                ucMsg.ShowInfo(objValidateFile.Message);
                            else
                            {
                                ucMsg.Visible = false;
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
                                        //string strFileName = "invalidData" + DateTime.Now.Ticks;
                                        //ExportInExcel(objDS.Tables["DtExcelSheet"], strFileName);
                                        //hlnkInvalid.NavigateUrl = strExcelVirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                        dsErrorProne.Merge(objDS.Tables["DtExcelSheet"]);
                                        hlnkInvalid.Text = "Invalid Data";
                                        ucMsg.ShowInfo("There is error in data. Please refer Invalid Data link.");

                                        blnIsUpload = false;
                                    }
                                    blnIsUpload = false;
                                }


                                if (objDS.Tables["DtDuplicateRecord"] != null && objDS.Tables["DtDuplicateRecord"].Rows.Count > 0)
                                {
                                    //hlnkDuplicate.Visible = true;
                                    dsErrorProne.Merge(objDS.Tables["DtDuplicateRecord"]);
                                    //string strFileName = "DuplicateData" + DateTime.Now.Ticks;
                                    //ExportInExcel(objDS.Tables["DtDuplicateRecord"], strFileName);
                                    //hlnkDuplicate.NavigateUrl = strExcelVirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                    //hlnkDuplicate.Text = "Duplicate Data";
                                    blnIsUpload = false;
                                }
                                if (objDS.Tables["DtBlankData"] != null && objDS.Tables["DtBlankData"].Rows.Count > 0)
                                {
                                    // hlnkBlank.Visible = true;
                                    dsErrorProne.Merge(objDS.Tables["DtBlankData"]);
                                    // string strFileName = "BlankData" + DateTime.Now.Ticks;
                                    // ExportInExcel(objDS.Tables["DtBlankData"], strFileName);
                                    //hlnkBlank.NavigateUrl = strExcelVirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                    //hlnkBlank.Text = "Blank Data";
                                    blnIsUpload = false;
                                }
                                if (blnIsUpload)
                                {
                                    if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                                    {
                                        InsertData(objDS);
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
                                    ucMsg.ShowInfo("There is error in data. Please refer Invalid Data link.");
                                }



                            }
                        }
                    }
                }
                /* #CC02 Add Start */else
                {
                    ucMsg.ShowInfo(Resources.Messages.BlankFileMessage);
                }
            }
            else if (Upload == 2)
            {
                ucMsg.ShowInfo(Resources.Messages.UploadXlxs);
            }
            else if (Upload == 3)
            {
                ucMsg.ShowInfo(Resources.Messages.SelectFile);
            } /* #CC02 Add End */
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }
    private void ExportInExcel(DataSet DsError, string strFileName)
    {
        if (DsError != null && DsError.Tables.Count > 0)
        {
            PageBase.ExportToExeclV2(DsError, strFileName, DsError.Tables.Count);
        }
    }

    private void InsertData(DataSet objds)
    {

      

        DataTable dt = new DataTable();
        dt = objds.Tables[0];
        /* #CC02 Add Start */
        if (Convert.ToString(Session["ISDLogin"]) != "0")
        {/* #CC02 Add End */
            if (Convert.ToInt16(Session["ISDLogin"]) == 0)
            {

                string expression;
                expression = "ISPUsername is null or ISPPassword is null";
                DataRow[] foundRows;

                // Use the Select method to find all rows matching the filter.
                foundRows = dt.Select(expression);




                //   int TypeCheck = (from v in dt.AsEnumerable() where Convert.ToDouble(v.Field<double>("ISPUsername")).ToString() == "" | Convert.ToDouble(v.Field<double>("ISPPassword")).ToString() == "" select v).Count();
                if (foundRows != null)
                {
                    if (foundRows.Length > 0)
                    {
                        ucMsg.ShowInfo("ISPUsername or ISPPassword can not be blank.");
                        return;
                    }


                }

            }

            DataColumn dcPasswordSalt = new DataColumn("ISPPasswordSalt", typeof(string));
            dcPasswordSalt.DefaultValue = string.Empty;
            dt.Columns.Add(dcPasswordSalt);


            using (Authenticates ObjAuth = new Authenticates())
            {
                foreach (DataRow item in dt.Rows)
                {
                    string pws = string.Empty;
                    if (item["ISPUsername"].ToString() != "" & item["ISPPassword"].ToString() != "")
                    {
                        pws = ObjAuth.GenerateSalt(item["ISPPassword"].ToString().Length);
                        item["ISPPasswordSalt"] = pws;
                        item["ISPPassword"] = ObjAuth.EncryptPassword(item["ISPPassword"].ToString(), pws);
                    }

                }
            };
        }/* #CC02 Added */
          dt.AcceptChanges();
          objds = new DataSet();

          objds.Tables.Add(dt.Copy());

          dt = null;
        //  objds.Tables.Add(objdt.Copy());
        if (objds != null && objds.Tables.Count > 0 && objds.Tables[0].Rows.Count > 0)
        {

            string strXML = string.Empty;
            int intResult = 0;
            strXML = objds.GetXml();
            strXML = strXML.Replace("T00:00:00+05:30", "");


            using (CommonMaster ObjCommon = new CommonMaster())
            {
                ObjCommon.DocXML = strXML;
                ObjCommon.CompanyID = CompanyID;
                ObjCommon.UserID = PageBase.UserId;/* #CC01 Added */
                ObjCommon.SPName = "PrcUploadISP";
                ObjCommon.PasswordExpiryDays = Convert.ToInt16(Application["ExpiryDays"].ToString());
                objds = ObjCommon.InsertThroughUpload(ref intResult);
            };
            if (intResult == 0)
            {
               // lblUploadMsg.Text = Resources.GlobalMessages.ErrorMsgTryAfterSometime;
                ucMsg.ShowError(Resources.GlobalMessages.ErrorMsgTryAfterSometime);
            }
            else if (intResult == 1)
            {
              //  lblUploadMsg.Text = Resources.GlobalMessages.DataUploadSuccess;
                ucMsg.ShowSuccess(Resources.GlobalMessages.DataUploadSuccess);
                GridISP.Visible = false;
                GridISP.DataSource = null;
                GridISP.DataBind();

            }


            else if (intResult == 2)
            {
                GridISP.Visible = true;
                GridISP.DataSource = objds.Tables[0];
                GridISP.DataBind();
                ucMsg.ShowInfo(Resources.GlobalMessages.CheckErrorGrid);

            }


        }

    }
    protected void rdoSelectMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoSelectMode.SelectedValue == "2")
        {
            Response.Redirect("ISPMasterInterface.aspx");
        }
    }
    /* #CC02 Add Start */
    protected void lnkDownloadTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            DataTable dt = ISPTemplateExcelFile();
            ds.Merge(dt);
            String FilePath = Server.MapPath("../../");
            string FilenameToexport = "ISP Template";
            PageBase.RootFilePath = FilePath;
            PageBase.ExportToExecl(ds, FilenameToexport);

        }
        catch (Exception ex)
        {
            
        }
    }

    private DataTable ISPTemplateExcelFile()
    {
        try
        {
            OpenXMLExcel objexcel = new OpenXMLExcel();
            DataSet dsTemplate = new DataSet();
            string strTemplatePath = Server.MapPath(PageBase.strExcelTemplatePathSB);
            DirectoryInfo dirXLS = new DirectoryInfo(strTemplatePath);
            FileInfo[] drFilesXLS = dirXLS.GetFiles("*.xlsx");
            foreach (FileInfo fi in drFilesXLS)
            {
                if (fi.Name.ToLower().Contains("ispmaster") == true)
                    dsTemplate = objexcel.ImportExcelFile(strTemplatePath + "ISPMaster.xlsx");
            }
            if (Convert.ToString(Session["ISDLogin"]) == "0")
            {
                if(dsTemplate.Tables[0].Columns.Contains("ISPUsername"))
                {
                    dsTemplate.Tables[0].Columns.Remove("ISPUsername");
                }
                 if (dsTemplate.Tables[0].Columns.Contains("ISPPassword"))
                 {
                     dsTemplate.Tables[0].Columns.Remove("ISPPassword");
                 }
                 if (dsTemplate.Tables[0].Columns.Contains("Email"))
                 {
                     dsTemplate.Tables[0].Columns.Remove("Email");
                 }
            }

            dsTemplate.Tables[0].AcceptChanges();
            return dsTemplate.Tables[0];
        }
        catch( Exception ex)
        {
            throw ex;
        }

    }
    /* #CC02 Add End */
}
