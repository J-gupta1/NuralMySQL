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
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Linq.Expressions;
using ZedService;
//using System.Linq.Dynamic;

public partial class Masters_Common_ManageISPSalary : PageBase
{
    DataTable dtISPSalary;
    UploadFile UploadFile = new UploadFile();
    DataSet dsErrorProne = new DataSet();
    string strUploadedFileName = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        ucEffectiveDAteUpload.MinRangeValue = Convert.ToDateTime(System.DateTime.Now.Date);
         ucEffectiveDAteUpload.MaxRangeValue = Convert.ToDateTime(System.DateTime.MaxValue.Date);
         ucEffectiveFrom.MinRangeValue = DateTime.Now.Date;

         //ucEffectiveFrom.MinRangeValue = Convert.ToDateTime(System.DateTime.Now.Date);
         //ucEffectiveFrom.MaxRangeValue = Convert.ToDateTime(System.DateTime.MaxValue.Date);
         //ucEffectiveFrom.RangeErrorMessage = "Invalid date";
        if (!IsPostBack)
        {

            databind(0);
        }
    }
    protected void btnSubmitSalary_Click(object sender, EventArgs e)
    {
        try
        {

            if (IsPageRefereshed == true)
            {
                return;
            }
            Int32 Result = 0;
            string errorMsg = string.Empty;
            ucMsg.Visible = false;
            if (!ValidateControl())
            {
                return;
            }

            using (CommonData ObjCommom = new CommonData())
            {
                dtISPSalary = ObjCommom.GettvpISPSalary();
            }
            if (hdnISPCode.Value == "0")
            {
                ucMsg.ShowError("ISP Code could not be found");
                return;
            }
            using (BeautyAdvisorData objISPSalary = new BeautyAdvisorData())
            {

                string strGuid = Guid.NewGuid().ToString();
                DataRow drISpSalary = dtISPSalary.NewRow();
                drISpSalary["ISPCode"] = ISPCode();
                drISpSalary["ISPID"] = 0;//will be updated in the Database
                drISpSalary["SalaryAmt"] = txtISPSalary.Text.Trim();
                drISpSalary["TransUploadSession"] = strGuid;
                dtISPSalary.Rows.Add(drISpSalary);
                dtISPSalary.AcceptChanges();
                if (dtISPSalary.Rows.Count > 0)
                {
                    if (!BulkCopyUpload(dtISPSalary))
                    {
                        
                        ucMsg.ShowError("Error Occured While transferring the data to the server");
                        return;
                    }

                }
               int ISPSalaryid=Convert.ToInt32(hdnISPSalaryid.Value);
                objISPSalary.ISPSalaryid = ISPSalaryid;
                objISPSalary.EffectiveFromDate = Convert.ToDateTime(ucEffectiveFrom.Date);
                objISPSalary.ComingFrom = 0;
                objISPSalary.ModeISPSalaryInfo = btnSubmitSalary.Text.ToLower() == "update" ? Convert.ToInt16(2) : Convert.ToInt16(1);
                objISPSalary.TransUploadSession = strGuid;
                objISPSalary.Userid = PageBase.UserId;
                DataSet ds = objISPSalary.InsertISPSalary();
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ucMsg.XmlErrorSource = ds.GetXml();
                        //lblMessage.Text=""
                    }
                    else
                    {
                        ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                        databind(0);
                        clearForm();
                        btnSubmitSalary.Text = "Submit";
                    }
                }
                else
                {
                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    databind(0);
                    clearForm();
                    btnSubmitSalary.Text = "Submit";
                }
             
              
               
                
            };

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        
        clearForm();
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            if (ucEffectiveDAteUpload.Date == "")
            {
                ucMsg.ShowError("Effective Date is mandatory");
                return;
            }
            if (ddlMode.SelectedValue == "3" & Convert.ToDateTime(ucEffectiveDAteUpload.Date)<=System.DateTime.Now)
            {
                  ucMsg.ShowError("Future Date is allowed only for delete");
                  return;
            }
            DataTable dtError = new DataTable();
            string strMode = ddlMode.SelectedValue;
            DataTable dtErrorTable = GetBlankTableError();

            HttpContext.Current.Session["PkeyColumns"] = null;
            string strKey = string.Empty;

            hlnkInvalid.Visible = false;
            Int16 Upload = 0;
            Upload = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            ViewState["TobeUploaded"] = strUploadedFileName;
            if (Upload == 1)
            {
                DataTable dtExcelData = new DataTable();
                OpenXMLExcel objexcel = new OpenXMLExcel();
                DataSet DsExcel = objexcel.ImportExcelFile(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);
                string guid = Guid.NewGuid().ToString();
                if (DsExcel != null && DsExcel.Tables.Count > 0 && DsExcel.Tables[0].Rows.Count > 0)
                {
                    ValidateUploadFile objValidateFile = new ValidateUploadFile();
                    {
                        DataSet objDS = DsExcel;
                        DataTable dt1 = DsExcel.Tables[0];
                        SortedList objSL = new SortedList();
                        objValidateFile.UploadedFileName = strUploadedFileName;
                        if(ddlMode.SelectedValue!="3")
                        objValidateFile.ExcelFileNameInTable = "UploadISPSalary";
                        else
                            objValidateFile.ExcelFileNameInTable = "DeleteISPSalary";
                            
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


                                var DuplicateISPCode = objDS.Tables[0].AsEnumerable().GroupBy(x => x["IspCode"].ToString().Trim()).Where(g => g.Count() > 1).Select(x => new { Error = x.Key, ErrorData = "Duplicate ISP Code" });



                                if (DuplicateISPCode != null)
                                {

                                    if (DuplicateISPCode.Count() > 0)
                                    {
                                        dtError = new DataTable();
                                        counter = 1;
                                        dtError = PageBase.LINQToDataTable(DuplicateISPCode);
                                        foreach (DataRow dr in dtError.Rows)
                                        {
                                            DataRow drow = dtErrorTable.NewRow();
                                            drow["ISPCode"] = dr["Error"];
                                            drow["ReasonForInvalid"] = dr["ErrorData"];
                                            dtErrorTable.Rows.Add(drow);
                                        }
                                        dtErrorTable.AcceptChanges();
                                    }
                                }


                                if (counter > 0)
                                {
                                    ucMsg.ShowInfo("Invalid Records");
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
                                    objDS.Tables[0].Columns.Add(AddColumn(guid, "TransUploadSession", typeof(System.String)));
                                    objDS.Tables[0].Columns.Add(AddColumn("0", "ISPId", typeof(System.Int32)));
                                    if (objDS.Tables[0].Columns.Contains("ISPSalary"))
                                        objDS.Tables[0].Columns["ISPSalary"].ColumnName = "SalaryAmt";
                                       if (ddlMode.SelectedValue=="3")
                                       objDS.Tables[0].Columns.Add(AddColumn("0", "SalaryAmt", typeof(System.Int32)));
                                    if (!BulkCopyUpload(objDS.Tables[0]))
                                    {
                                        ucMsg.ShowError("Error Occured While transferring the data to the server");
                                        return;
                                    }
                                    else
                                    {
                                        SubmitData(guid);
                                    }
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
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }
    protected void btnUploadCancel_Click(object sender, EventArgs e)
    {
        ddlMode.SelectedValue = "0";
        ucEffectiveDAteUpload.Date = "";
    }
    protected void btnExprtToExcel_Click(object sender, EventArgs e)
    {
        databind(1);
    }
    protected void btnSearchISP_Click(object sender, EventArgs e)
    {
        databind(0);
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        txtSearchISPCode.Text = string.Empty;
        txtSearchISPName.Text = string.Empty;
        databind(0);
    }
    bool ValidateControl()
    {
        if (txtISPCode.Text == string.Empty | txtISPSalary.Text == string.Empty | ucEffectiveFrom.Date == "")
        {
            ucMsg.ShowError("Please fill required information");
            return false;
        }
        if (ucEffectiveFrom.Date != "")
        {
            if (Convert.ToDateTime(ucEffectiveFrom.Date) < Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy")))
            {
                ucMsg.ShowError("Date can not be past date");
                return false;
            }
        }
        if (hdnISPCode.Value.IndexOf('-') <= 0)
        {
            ucMsg.ShowError("Please Select ISP name from list or place name in right Format");
            return false;
        }

        if (Convert.ToInt32(txtISPSalary.Text) <= 0)
        {
            ucMsg.ShowError("Negative/Zero Amount salary is not allowed");
            return false;
        }
        return true;
    }
    public bool BulkCopyUpload(DataTable dtTempTable)
    {
        try
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "ISPSalaryBulk";
                bulkCopy.ColumnMappings.Add("ISPCode", "ISPCode");
                bulkCopy.ColumnMappings.Add("SalaryAmt", "SalaryAmt");
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
            DataSet DsReferenceCode = new DataSet();
            using (BeautyAdvisorData objISP = new BeautyAdvisorData())
            {
                DsReferenceCode = objISP.GetISPList();
                if (DsReferenceCode != null && DsReferenceCode.Tables.Count > 0)
                {
                 String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(DsReferenceCode, FilenameToexport, EnumData.eTemplateCount.ePrice);
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


    protected void grdISPList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdISPList.PageIndex = e.NewPageIndex;
        databind(0);
    }
    protected void grdvwUserList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }
    protected void grdISPList_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        DataTable dtSalary;
        if (e.CommandName == "cmdEdit")
        {
            try
            {

                ucMsg.Visible = false;

                using (BeautyAdvisorData objISPSalary = new BeautyAdvisorData())
                {
                    hdnISPSalaryid.Value = e.CommandArgument.ToString();
                    objISPSalary.ISPSalaryid = Convert.ToInt32(hdnISPSalaryid.Value);
                    dtSalary = objISPSalary.SelectISPSalaryList();
                    if (dtSalary.Rows.Count > 0)
                    {
                        txtISPCode.Text = dtSalary.Rows[0]["ISPDisplayName"].ToString();
                        txtISPSalary.Text = dtSalary.Rows[0]["SalaryAmt"].ToString();
                        ucEffectiveFrom.Date = Convert.ToDateTime(dtSalary.Rows[0]["ActivationDate"]).ToShortDateString();
                        hdnISPCode.Value = txtISPCode.Text;
                        txtISPCode.Enabled = false;
                    }
                    else
                    {

                        ucMsg.ShowError("There is no record to update");
                    }

                }


                btnSubmitSalary.Text = "Update";
                updISP.Update();
            }

            catch (Exception ex)
            {
                ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }

        }


        if (e.CommandName == "cmdDelete")
        {
            try
            {

                ucMsg.Visible = false;

                using (BeautyAdvisorData objISPSalary = new BeautyAdvisorData())
                {
                    hdnISPSalaryid.Value = e.CommandArgument.ToString();
                    objISPSalary.ModeISPSalaryInfo = 3;
                    objISPSalary.ISPSalaryid = Convert.ToInt32(hdnISPSalaryid.Value);
                    objISPSalary.ComingFrom = 0;
                     DataSet ds =  objISPSalary.InsertISPSalary();
                     if (ds != null & ds.Tables.Count>0)
                     {
                         if (ds.Tables[0].Rows.Count > 0)
                         {
                             ucMsg.XmlErrorSource = ds.GetXml();
                         }
                         else
                             ucMsg.ShowSuccess(Resources.Messages.Delete);

                     }
                     else
                     {
                         ucMsg.ShowSuccess(Resources.Messages.Delete);
                     }
                     databind(0);

                }


                btnSubmitSalary.Text = "Update";
                updISP.Update();
            }

            catch (Exception ex)
            {
                ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }

        }
    }

   
    public void databind(Int16 excel)
    {
        using (BeautyAdvisorData objSalary = new BeautyAdvisorData())
        {
            try
            {
              //ucMsg.Visible = false;
                objSalary.ISPName = txtSearchISPName.Text.Trim();
                //if (ucSearchEffectiveDate.Date == "")
                //    objSalary.EffectiveFromDate = null;
                //else objSalary.EffectiveFromDate = Convert.ToDateTime(ucSearchEffectiveDate.Date);
                objSalary.ISPCode = txtSearchISPCode.Text.Trim();
                DataTable dtISPSearch = objSalary.SelectISPSalaryList();
                if (excel == 1)
                {
                    DataSet ds = new DataSet();
                    dtISPSearch.Columns.Remove("ISPDisplayName");
                    dtISPSearch.Columns.Remove("displayModifyButton");
                    dtISPSearch.Columns.Remove("ISPSalaryID");
                    dtISPSearch.Columns.Remove("ISPID");
                    ds.Merge(dtISPSearch);
                    if (ds != null)
                    {
                        
                            
                            
                                
                        ExportInExcel(ds, "ISP Salary");
                    }
                }
                else
                {
                    grdISPList.DataSource = dtISPSearch;
                    grdISPList.DataBind();
                }
                updgrid.Update();
            }



            catch (Exception ex)
            {
                ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }
        }
    }

     public DataTable GetBlankTableError()
    {
        DataTable Detail = new DataTable();
        Detail.Columns.Add("ISPCode");
        Detail.Columns.Add("ReasonForInvalid");
        return Detail;
    }
    void clearForm()
    {
        btnSubmitSalary.Text = "Submit";
        hdnISPSalaryid.Value = "0";
        txtISPSalary.Text = string.Empty;
        txtISPCode.Text = string.Empty;
        ucEffectiveFrom.Date = string.Empty;
        txtISPCode.Enabled = true;
        updISP.Update();

    }

    private void ExportInExcel(DataSet DsError, string strFileName)
    {
        if (DsError != null && DsError.Tables.Count > 0)
        {
            PageBase.ExportToExeclV2(DsError, strFileName, DsError.Tables.Count);
        }
    }
    string ISPCode()
    {
            string[] str = hdnISPCode.Value.Split('-');
            return str[1];

    }

    void SubmitData(string strGuid)
    {
        using (BeautyAdvisorData objISPSalary = new BeautyAdvisorData())
        {
            objISPSalary.ISPSalaryid = 0;
            if (ucEffectiveDAteUpload.Date == "")
                objISPSalary.EffectiveFromDate = null;
            else
                objISPSalary.EffectiveFromDate = Convert.ToDateTime(ucEffectiveDAteUpload.Date);
            objISPSalary.ComingFrom = 1;
            objISPSalary.ModeISPSalaryInfo =Convert.ToInt32(ddlMode.SelectedValue);
            objISPSalary.TransUploadSession = strGuid;
            objISPSalary.Userid = PageBase.UserId;
            DataSet ds = objISPSalary.InsertISPSalary();
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    hlnkInvalidDataFromDB.Visible = true;
                    string strFileName = "InvalidDataExcel" + DateTime.Now.Ticks;
                    ExportInExcel(ds, strFileName);
                    hlnkInvalidDataFromDB.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                    hlnkInvalidDataFromDB.Text = "Invalid Data from Excel";
                    ucMsg.ShowInfo("Partial Record updated , Please click on link to view the Error");
                }
                else
                {
                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                }
            }
            else
            {
                ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
            }
            databind(0);
        }
    }
    protected void ddlMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMode.SelectedValue == "3")
        {
            InsertTemplate.Visible = false;
            DeleteTemplate.Visible = true;
            //ucEffectiveDAteUpload.IsEnabled = false;
            //ucEffectiveDAteUpload.imgCal.Enabled = false;
            //ucEffectiveDAteUpload.Date = "";
            //ucEffectiveDAteUpload.TextBoxDate.Enabled = false;
        }
        else
        {
            DeleteTemplate.Visible = false;
            InsertTemplate.Visible = true;
            //ucEffectiveDAteUpload.IsEnabled = true;
            //ucEffectiveDAteUpload.imgCal.Enabled = true;
            //ucEffectiveDAteUpload.TextBoxDate.Enabled = true;
        }
        updgrid.Update();
        updISP.Update();
    }
}

