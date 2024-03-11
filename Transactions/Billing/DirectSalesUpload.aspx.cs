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
* Created By : Shashikant Singh
* Created On : 20-July-2015
* Role : SSE
* Module : Dirct Sale
* Description :
* Table Name: .
* ====================================================================================================
* Reviewed By :
 * Mittal Sir
 ====================================================================================================
Modification On       Modified By          Modification    
---------------      -----------          ------------------------------------------------------------- 
*  03-Aug-2016         Kalpana              #CC01: CssClass added
 * 12-Oct-2016         Shashikant Singh     #CC02: Apply check for input lenth for reference no is 20
 * 11-Apr-2017, Sumit Maurya, #CC03, Template gets downloaded through server side code instead of direct download.
   09-May-2017, Vijay Katiyar, #CC04 , Template gets downloaded through server side code instead of direct download also download excel file for invalid ,duplicate and blank from server side. 
 * 05-July-2017, Kalpana, #CC05: hardcoded style removed and applied responsive css
 * 02-aug-2017, Shashikant Singh,#CC06, Added dropdown as Receive Stock Mode.
 ====================================================================================================
*/

#endregion
using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.ApplicationBlocks.Data;
//using ZedEBS;
//using ZedEBS.SaleLib;
using System.IO;
using BussinessLogic;
using ZedService;

public partial class Order_Common_DirectSalesUpload : BussinessLogic.PageBase //Pagebase
{
    int _intEntityId;

    string strUploadedFileName = string.Empty;
    string strUploadedFileName1 = string.Empty;
    //string strDownloadPath = Pagebase.strDownloadExcelVirtualPath;
    string strDownloadPath = PageBase.strExcelPhysicalDownloadPathSB;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!IsPostBack)
            {
                ViewState["Refrece"] = "No";
                cmbOrderFrom.BusinessEventKeyword = Enum.GetName(typeof(ZedAxis.ZedEBS.Enums.BusinessEventsKeyword), ZedAxis.ZedEBS.Enums.BusinessEventsKeyword.SOGENERATE).ToString();
                cmbOrderFrom.EntityTypeDescription = Convert.ToInt32(ZedAxis.ZedEBS.Enums.EntityTypeDescription.IncludingEntityType);
                cmbOrderFrom.SelectionMode = ZedAxis.ZedEBS.Enums.EnumSelectionMode.Single;
                cmbOrderFrom.Keyword = ZedAxis.ZedEBS.Enums.EntityTypeKeyword.SALECHANNEL;
                cmbOrderFrom.IsParent = UserControls_ucEntityList.Parent.True;
                cmbOrderFrom.Type = UserControls_ucEntityList.EntityType.PRIMARY;
                cmbOrderFrom.LoggedInEntityID = PageBase.SalesChanelID;
                FillStockMode();
                /*hplinkDownloadTemplate.NavigateUrl = VirtualPath + "DownloadUpload/Templates/DirectSalesTemplate.xlsx"; #CC03 Commented */
            }
            if(Convert.ToString(ViewState["Refrece"])=="Yes")
            {
                
                ucMessage1.ShowWarning("file already uploaded.");
                return;
            
            }
        }
        catch (Exception ex)
        {
            //ucMessage1.ShowAppError(ex);
            ucMessage1.ShowError(ex.Message);
        }

    }


    private void FillStockMode()
    {
        using (clsSalesOrder obj = new clsSalesOrder())
        {
            DataTable DtStockMode;
            obj.PageIndex = 1;
            obj.PageSize = Convert.ToInt32( PageSize);
            DtStockMode = obj.SelectStockMode();
            if (DtStockMode.Rows.Count > 0 && DtStockMode != null)
            {
                ddlStockMode.Items.Clear();
                ddlStockMode.DataSource = DtStockMode;
                ddlStockMode.DataTextField = "StockModeDesc";
                ddlStockMode.DataValueField = "StockModeMasterID";
                ddlStockMode.DataBind();
                /*#CC06:Added Start*/
                ddlReceiveStockMode.Items.Clear();
                ddlReceiveStockMode.DataSource = DtStockMode;
                ddlReceiveStockMode.DataTextField = "StockModeDesc";
                ddlReceiveStockMode.DataValueField = "StockModeMasterID";
                ddlReceiveStockMode.DataBind();

                /*#CC06:Added End*/
            }
            ddlStockMode.Items.Insert(0, new ListItem("Select", "0"));
            ddlReceiveStockMode.Items.Insert(0, new ListItem("Select", "0")); /*#CC06:Added */
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            if (CheckValidation())
            {
                ucMessage1.ShowWarning("Please select sales entity/stock mode and put ref number.");
                return;            
            }
            ucMessage1.Visible = false;
            //hlnkInvalid.Visible = false;
            //hlnkDuplicate.Visible = false;
            //hlnkBlank.Visible = false;


            if (IsPageRefereshed)
            {
                ucMessage1.ShowWarning("File allready uploaded!");
                return;
                
            }
            Int16 Upload = 0;
            Upload = IsExcelFile();
            if (Upload == 1)
            {
                OpenXMLExcel objexcel = new OpenXMLExcel();
                //DataSet DsExcel = objexcel.ImportExcelFile(Pagebase.strUploadExcelFullPath + "UploadJobExcel/" + strUploadedFileName);
                DataSet DsExcel = objexcel.ImportExcelFile(PageBase.strExcelPhysicalUploadPathSB +  strUploadedFileName);
                
                if (DsExcel != null && DsExcel.Tables.Count > 0 && DsExcel.Tables[0].Rows.Count > 0)
                {
                    string strPkColName = "SR#";
                    if (DsExcel.Tables[0].Rows.Count > 4000)
                        ucMessage1.ShowInfo("Please upload maximum 4000 records in a single upload!");
                    else
                    {
                        BusinessLogics.ValidateUploadFile objValidateFile = new BusinessLogics.ValidateUploadFile();                       
                       // using (ZedEBS.Data1.ValidateUploadFile objValidateFile = new ZedEBS.Data1.ValidateUploadFile())
                        //{
                            DataSet objDS = DsExcel;
                            DataTable dt1 = DsExcel.Tables[0];
                            SortedList objSL = new SortedList();
                            SortedList objSLCorrData = new SortedList();

                            objValidateFile.UploadedFileName =  strUploadedFileName;
                            objValidateFile.ExcelFileNameInTable = "DirectSalesTemplate";
                            objValidateFile.PkColumnName = strPkColName;

                            objValidateFile.ValidateFile(false, out objDS, out objSL);
                            //0 =Complete DataTable excluding blank and duplicate records                        
                            //1=Duplicate Records
                            //2=Blank Records

                            if (objValidateFile.Message != null && objValidateFile.Message.Trim() != "")
                                ucMessage1.ShowInfo(objValidateFile.Message);
                            else
                            {
                                ucMessage1.Visible = false;
                                bool blnIsUpload = true;
                                if (objSL.Count >= 1 && objSL.Keys.Count >= 1)
                                {
                                    objDS.Tables["DtExcelSheet"].Columns.Add(new DataColumn("ReasonForInvalid"));
                                    IDictionaryEnumerator objIDicEnum = objSL.GetEnumerator();
                                    while (objIDicEnum.MoveNext())
                                    {
                                        foreach (DataRow dr in objDS.Tables["DtExcelSheet"].Select("[" + strPkColName + "] ='" + objIDicEnum.Key.ToString() + "'", strPkColName))
                                        {
                                            dr["ReasonForInvalid"] = objIDicEnum.Value.ToString();
                                        }
                                    }

                                    objDS.Tables[0].AcceptChanges();
                                    if (objDS.Tables["DtExcelSheet"] != null && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                                    {
                                        hlnkInvalid.Visible = true;
                                        string strFileName = "invalidData" + DateTime.Now.Ticks;
                                        ExportInExcel(objDS.Tables["DtExcelSheet"], strFileName);
                                        //hlnkInvalid.NavigateUrl = strDownloadPath + "DownloadInvalidJobExcel/" + strFileName + ".xlsx";/* #CC04 Commented */
                                        ViewState["hlnkInvalid"] = strDownloadPath + strFileName + ".xlsx"; /* #CC04 Added */
                                        hlnkInvalid.Text = "Invalid Data";

                                        blnIsUpload = false;
                                    }
                                    blnIsUpload = false;
                                }
                                if (!objDS.Tables[0].Columns.Contains("ReasonForInvalid"))
                                {

                                    DataTable dtRightFormat = new DataTable();
                                    dtRightFormat = objDS.Tables[0];
                                    dtRightFormat.TableName = "DtExcelSheet";
                                    dtRightFormat.AcceptChanges();
                                    objDS.Tables.Remove("DtExcelSheet");
                                    int counter = 0;
                                    for (int intTmpVar = 0; intTmpVar < dtRightFormat.Rows.Count; intTmpVar++)
                                    {
                                        if (!dtRightFormat.Columns.Contains("ReasonForInvalid"))
                                            dtRightFormat.Columns.Add(new DataColumn("ReasonForInvalid"));

                                        if (Convert.ToInt32(dtRightFormat.Rows[intTmpVar]["Quantity"]) < 0)
                                        {
                                            dtRightFormat.Rows[intTmpVar]["ReasonForInvalid"] = Resources.WarningMessages.ApprovedQuantity.ToString();
                                            counter = counter + 1;
                                        }

                                        if (dtRightFormat.Rows[intTmpVar]["SAPPartCode"].ToString().Length > 20)
                                        {
                                            if (dtRightFormat.Rows[intTmpVar]["ReasonForInvalid"] == DBNull.Value || dtRightFormat.Rows[intTmpVar]["ReasonForInvalid"] == string.Empty)
                                                dtRightFormat.Rows[intTmpVar]["ReasonForInvalid"] = Resources.WarningMessages.SAPPartCodeLength.ToString();
                                            else
                                                dtRightFormat.Rows[intTmpVar]["ReasonForInvalid"] += ";" + Resources.WarningMessages.SAPPartCodeLength.ToString();
                                            counter = counter + 1;
                                        }

                                        if (dtRightFormat.Rows[intTmpVar]["BatchNo"].ToString().Length > 20)
                                        {
                                            if (dtRightFormat.Rows[intTmpVar]["ReasonForInvalid"] == DBNull.Value || dtRightFormat.Rows[intTmpVar]["ReasonForInvalid"] == string.Empty)
                                                dtRightFormat.Rows[intTmpVar]["ReasonForInvalid"] = Resources.WarningMessages.BatchNoLength.ToString();
                                            else
                                                dtRightFormat.Rows[intTmpVar]["ReasonForInvalid"] += ";" + Resources.WarningMessages.BatchNoLength.ToString();
                                            counter = counter + 1;
                                        }
                                        if (dtRightFormat.Rows[intTmpVar]["SerialNo"].ToString().Length > 20)
                                        {
                                            if (dtRightFormat.Rows[intTmpVar]["ReasonForInvalid"] == DBNull.Value || dtRightFormat.Rows[intTmpVar]["ReasonForInvalid"] == string.Empty)
                                                dtRightFormat.Rows[intTmpVar]["ReasonForInvalid"] = Resources.WarningMessages.SerialNoLenth.ToString();
                                            else
                                                dtRightFormat.Rows[intTmpVar]["ReasonForInvalid"] += ";" + Resources.WarningMessages.SerialNoLenth.ToString();
                                            counter = counter + 1;
                                        }

                                    }

                                    if (counter > 0)
                                    {
                                        ucMessage1.ShowInfo(Resources.Messages.Invalid);
                                        objDS.Merge(dtRightFormat);
                                        dtRightFormat = null;
                                        hlnkInvalid.Visible = true;
                                        string strFileName = "invalidData" + DateTime.Now.Ticks;
                                        ExportInExcel(objDS.Tables["DtExcelSheet"], strFileName);
                                        //hlnkInvalid.NavigateUrl = strDownloadPath + "DownloadInvalidJobExcel/" + strFileName + ".xlsx"; /* #CC04 Commented */
                                        ViewState["hlnkInvalid"] = strDownloadPath + strFileName + ".xlsx"; /* #CC04 Added */
                                        hlnkInvalid.Text = "Invalid Data";
                                        blnIsUpload = false;
                                    }
                                    else
                                    {
                                        dtRightFormat.Columns.Remove("ReasonForInvalid");
                                        objDS.Merge(dtRightFormat);
                                        dtRightFormat = null;
                                    }
                                }

                                if (objDS.Tables["DtDuplicateRecord"] != null && objDS.Tables["DtDuplicateRecord"].Rows.Count > 0)
                                {
                                    hlnkDuplicate.Visible = true;
                                    string strFileName = "DuplicateData" + DateTime.Now.Ticks;
                                    ExportInExcel(objDS.Tables["DtDuplicateRecord"], strFileName);
                                    //hlnkDuplicate.NavigateUrl = strDownloadPath + "DownloadInvalidJobExcel/" + strFileName + ".xlsx"; /* #CC04 Commented */
                                    ViewState["hlnkDuplicate"] = strDownloadPath + strFileName + ".xlsx"; /* #CC04 Added */
                                    hlnkDuplicate.Text = "Duplicate Data";
                                    blnIsUpload = false;
                                }
                                if (objDS.Tables["DtBlankData"] != null && objDS.Tables["DtBlankData"].Rows.Count > 0)
                                {
                                    hlnkBlank.Visible = true;
                                    string strFileName = "BlankData" + DateTime.Now.Ticks;
                                    ExportInExcel(objDS.Tables["DtBlankData"], strFileName);
                                    //hlnkBlank.NavigateUrl = strDownloadPath + "DownloadInvalidJobExcel/" + strFileName + ".xlsx"; /* #CC04 Commented */
                                    ViewState["hlnkBlank"] = strDownloadPath + strFileName + ".xlsx"; /* #CC04 Added */
                                    hlnkBlank.Text = "Blank Data";
                                    blnIsUpload = false;
                                }
                                if (blnIsUpload)
                                {
                                    if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                                    {
                                        string strFileNameSchema = "Schema" + DateTime.Now.Ticks.ToString() + ".xml";
                                        ViewState["UploadDataFileNameSchema"] = strFileNameSchema;
                                        string strFileName = DateTime.Now.Ticks.ToString() + ".xml";
                                        ViewState["UploadDataFileName"] = strFileName;
                                        objDS.Tables[0].WriteXmlSchema(PageBase.strExcelPhysicalDownloadPathSB + strFileNameSchema);
                                        objDS.Tables[0].WriteXml(PageBase.strExcelPhysicalDownloadPathSB + strFileName);

                                        InsertData(objDS.Tables["DtExcelSheet"]);
                                    }
                                    else
                                        ucMessage1.ShowInfo(Resources.WarningMessages.NoRecord);
                                }
                                else
                                {

                                }


                            }
                        //}
                    }

                }
                else
                {
                    ucMessage1.ShowWarning(Resources.WarningMessages.EmptyFile);
                }
            }
            else if (Upload == 2)
            {
                ucMessage1.ShowWarning(Resources.Messages.UploadXlxs);
            }
            else if (Upload == 3)
            {
                ucMessage1.ShowWarning(Resources.Messages.SelectFile);
            }
            else
            {
                ucMessage1.ShowError(Resources.Messages.ErrorMsg);
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
            //ExceptionPolicy.HandleException(ex, ExceptionPolicyName);
        }

    }

    private void InsertData(DataTable objdt)
    {
        if (objdt != null)
        {
            if (objdt != null && objdt.Rows.Count > 0)
            {
                using (clsSalesOrder objDetail = new clsSalesOrder())
                {
                    DataTable dtInvalidRecordSet;
                    DataTable FinalUploadDetail;
                    if (objdt.Rows.Count > 0)
                    {
                        FinalUploadDetail = objdt.Copy();
                        DataColumnCollection columnname = FinalUploadDetail.Columns;
                        if (columnname.Contains("Error"))
                        {
                            FinalUploadDetail.Columns.Remove("Error");
                        }
                        FinalUploadDetail.AcceptChanges();
                        objDetail.dtSoDetail = FinalUploadDetail;
                    }
                    objDetail.CreatedBy = UserId;
                    objDetail.Remarks = txtRemarks.TextBoxText.Trim();
                    objDetail.ReferenceNo = txtPoNumber.Text.Trim();
                    if (cmbOrderFrom.SelectedIndex>0)
                    objDetail.Salefromid = cmbOrderFrom.SelectedValue;
                    else
                        objDetail.Salefromid = PageBase.SalesChanelID;
                    objDetail.StockModeId = Convert.ToInt16(ddlStockMode.SelectedValue);
                    objDetail.UploadedfileName = strUploadedFileName1;
                    objDetail.ProcessFileName = strUploadedFileName;
                    objDetail.StockMode = Convert.ToInt16(ddlReceiveStockMode.SelectedValue);/*#CC06:Added */
                    dtInvalidRecordSet = objDetail.DirctSaleOrderUploadInsert();
                    if (objDetail.Error != null && objDetail.Error != string.Empty)
                    {
                        ucMessage1.ShowError(objDetail.Error);
                    }
                    else if (dtInvalidRecordSet == null || dtInvalidRecordSet.Rows.Count == 0)
                    {
                        ucMessage1.ShowSuccess("Order created successfully.");
                        ViewState["Refrece"] = "Yes";

                    }

                    if (dtInvalidRecordSet != null && dtInvalidRecordSet.Rows.Count > 0)
                    {
                        string strFileName = "invalidData" + DateTime.Now.Ticks;
                        ExportInExcel(dtInvalidRecordSet, strFileName);
                        hlnkInvalid.Visible = true;
                        //hlnkInvalid.NavigateUrl = strDownloadPath + "DownloadInvalidJobExcel/" + strFileName + ".xlsx"; /* #CC04 Commented */
                        ViewState["hlnkInvalid"] = strDownloadPath + strFileName + ".xlsx";/*#CC04 Added*/
                        hlnkInvalid.Text = "Invalid Data";
                    }
                }
            }
            else
            {
                ucMessage1.ShowInfo("File is empty!");
            }
        }
    }

    private void ExportInExcel(DataTable objTmpTb, string strFileName)
    {
        if (objTmpTb.Rows.Count > 0)
        {
            string FileName = strFileName;
            string Path = strDownloadPath;
            strFileName =  FileName;
            //DataSet ds = new DataSet();

            objTmpTb.TableName = "tblInvalidRecords" + DateTime.Now.Ticks;
            //DataTable dt = objTmpTb.Copy();
            //ds.Tables.Add(dt);
            //LuminousSMS.Utility.LuminousUtil.SaveToExecl(ds, strFileName);
            PageBase.ExportToExecl(objTmpTb.DataSet, strFileName);            
        }
    }

    private Int16 IsExcelFile()
    {
        Int16 MessageforValidation = 0;
        try
        {
            if (uploadExcel.HasFile)
            {
                if (Path.GetExtension(uploadExcel.FileName).ToLower() == ".xlsx")
                {
                    try
                    {
                        strUploadedFileName1 = uploadExcel.FileName;
                        strUploadedFileName = PageBase.importExportExcelFileName;
                        uploadExcel.SaveAs(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);
                        MessageforValidation = 1;
                        return MessageforValidation;
                    }
                    catch (Exception objEx)
                    {
                        MessageforValidation = 0;
                        return MessageforValidation;
                        throw objEx;
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
            return MessageforValidation;
            throw objHttpException;
        }
        catch (Exception ex)
        {
            return MessageforValidation;
            throw ex;
        }
    }

    private bool CheckValidation()
    {

        bool result = false;
        if (ddlStockMode.SelectedValue == "0" || txtPoNumber.Text == string.Empty || (PageBase.BaseEntityTypeID!= 1 && cmbOrderFrom.SelectedValue <= 0))
        {
            result = true;
        }
        return result;  
    
    
    }
    

    #region Access Tag Changes Added
    protected void cmbEntityName_SelectedIndexChanged(Int32 EntityID)
    {
        try
        {
            _intEntityId = Convert.ToInt32(EntityID);

        }
        catch (Exception ex)
        {
            ucMessage1.ShowWarning(ex.Message);
            //ExceptionPolicy.HandleException(ex, ExceptionPolicyName);
        }
    }
    #endregion

    #region dwon lod entitycode and sappartcode

    protected void DownloadSalesToEntityCode_Click(object sender, EventArgs e)
    {
        try
        {
            if (cmbOrderFrom.SelectedValue <= 0)
            {
                ucMessage1.ShowWarning("Please select sales from entity name.");
                return;
            }
            using (clsSalesOrder objEntity = new clsSalesOrder())
            {
                objEntity.BusinessEventKeyword = Enum.GetName(typeof(ZedAxis.ZedEBS.Enums.BusinessEventsKeyword), ZedAxis.ZedEBS.Enums.BusinessEventsKeyword.SOGENERATE).ToString();
                objEntity.EntityTypeDescription = Convert.ToInt32(ZedAxis.ZedEBS.Enums.EntityTypeDescription.IncludingEntityType);
                objEntity.SelectionMode = ZedAxis.ZedEBS.Enums.EnumSelectionMode.Single;
                objEntity.Keyword = "SALECHANNEL";
                objEntity.IsParent = 0;
                objEntity.Type = 2;
                objEntity.ParentSelectedValue = Convert.ToInt32(cmbOrderFrom.SelectedValue);
                objEntity.LoggedInEntityID = PageBase.SalesChanelID;
                DataTable dt = objEntity.FromEntityName();
                DataSet ds = new DataSet();
                ds.Merge(dt);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    //LuminousSMS.Utility.LuminousUtil.ExportToExecl(ds, "SalesToEntityCode");
                    PageBase.ExportToExecl(ds, "SalesToEntityCode");
                }
                else
                {
                    ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
            //ucMessage1.ShowError(Resources.Messages.ErrorMsg);
            //ExceptionPolicy.HandleException(ex, ExceptionPolicyName);
        }
    }

    #endregion



    /* #CC03 Add Start */
    protected void lnkDownloadTemplate_Click(object sender, EventArgs e)
    {
        try
        {
             /*#CC04 :Commented Start*/
			/*
            string path = Server.MapPath("~/DownloadUpload/Templates/DirectSalesTemplate.xlsx");
            string type = "";
            type = "Application/excel";
            Response.AppendHeader("content-disposition", "attachment; filename=DirectSalesTemplate.xlsx");
            if ((type != ""))
            {
                Response.ContentType = type;
                Response.WriteFile(path);
                Response.End();
            }
             */
            /*#CC04 :Commented End*/
            //LuminousSMS.Utility.LuminousUtil.DownloadStaticTemplate("DirectSalesTemplate.xlsx");  /*#CC04 :Added*/
        }
        catch (Exception ex)
        {
        }
    }
    /* #CC03 Add End */
    /* #CC04 Added start*/
    protected void hlnkBlank_Click(object sender, EventArgs e)
    {
        DownloadExportToExcelFile(Convert.ToString(ViewState["hlnkBlank"]));
    }
    protected void hlnkDuplicate_Click(object sender, EventArgs e)
    {
        DownloadExportToExcelFile(Convert.ToString(ViewState["hlnkDuplicate"]));
    }
    protected void hlnkInvalid_Click(object sender, EventArgs e)
    {
        DownloadExportToExcelFile(Convert.ToString(ViewState["hlnkInvalid"]));
    }
    /* #CC04 Added end*/


}
