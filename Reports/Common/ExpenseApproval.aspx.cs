//======================================================================================
//* Developed By : Vijay Kumar Prajapati 
//* Role         : Software Developer
//* Module       : Expense Approval Page  
//* Description  :  This page is used for Expense Approval Page  
//* ====================================================================================
/* Change Log
 * -------------------------------------------------------------------------------------
 * DD-MMM-YYYY, Name, #CCXX, Description
 
 
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
using ExportExcelOpenXML;
using System.Text;
using BusinessLogics;
using System.Collections;
using System.Data.SqlClient;
using ZedService;

public partial class Reports_Common_ExpenseApproval : PageBase
{
    DataTable dtNew = new DataTable();
    object objSum;
    int counter = 0;
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    UploadFile UploadFile = new UploadFile();
    List<String> lstDuplicate = new List<String>();
    DataSet dsErrorProne = new DataSet();
    string strPrimarySessionName = "ExpensUploadSession";
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            FillEntityType();
            FillApprovalStatus();
        }
    }
    protected void ddlEntityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillEntityTypeName(Convert.ToInt32(ddlEntityType.SelectedValue));
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (ucFromDate.Date != "" && ucToDate.Date == "")
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowWarning("Please Enter  To Date.");
            return;
        }
        if (ucToDate.Date != "" && ucFromDate.Date == "")
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowWarning("Please Enter  From Date.");
            return;
        }
        ucMessage1.Visible = false;
        if (ddlApprovalStatus.SelectedValue == "2" || ddlApprovalStatus.SelectedValue == "3")
        {
            btnApprove.Visible = false;
            btnReject.Visible = false;
        }
        else
        {
            btnApprove.Visible = true;
            btnReject.Visible = true;
        }

        SearchExpenseDetailData(1);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ddlEntityType.SelectedValue = "0";
        if (ddlEntityType.SelectedValue == "0")
        {
            ddlEntityTypeName.SelectedValue = "0";
            ddlEntityTypeName.Items.Clear();
            ddlEntityTypeName.Items.Insert(0, new ListItem("Select", "0"));
        }
        ddlApprovalStatus.SelectedValue = "0";
        gvExpenseDetail.DataSource = null;
        gvExpenseDetail.DataBind();
        ucFromDate.TextBoxDate.Text = "";
        ucToDate.TextBoxDate.Text = "";
        ucMessage1.Visible = false;
        PnlGrid.Visible = false;
    }
    protected void btnExportexcel_Click(object sender, EventArgs e)
    {
        SearchExpenseDetailData(-1);
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        SearchExpenseDetailData(ucPagingControl1.CurrentPage);
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsPageRefereshed)
            {
                return;

            }
            DataTable dtErrorTable = GetBlankTableError();
            DataTable dtError = new DataTable();
            HttpContext.Current.Session["PkeyColumns"] = null;
            string strKey = string.Empty;
            hlnkInvalid.Visible = false;

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
                            objValidateFile.ExcelFileNameInTable = "UploadApproveExpense";

                            objValidateFile.ValidateFileWithCompanyId(false, out objDS, out objSL);
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
                                        string strFileName = "InvalidData" + DateTime.Now.Ticks;
                                        ExportInExcel(dsErrorProne, strFileName);
                                        hlnkInvalid.Text = "Invalid Data";
                                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
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
            else
            {
                ucMessage1.ShowInfo("File is empty! Some Mandatory columns has no required data!");
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    void FillEntityType()
    {
        using (ClsExpense ObjEntityType = new ClsExpense())
        {

            ddlEntityType.Items.Clear();
            string[] str = { "EntityTypeID", "EntityType" };
            ObjEntityType.CompanyId = PageBase.ClientId;
            ObjEntityType.UserId = PageBase.UserId;
            PageBase.DropdownBinding(ref ddlEntityType, ObjEntityType.GetEntityTypeV5API(), str);

        };
    }
    void FillEntityTypeName(int EntityTypeID)
    {
        using (ClsExpense ObjEntityTypeName = new ClsExpense())
        {

            ddlEntityTypeName.Items.Clear();
            ObjEntityTypeName.EntityTypeId = EntityTypeID;
            ObjEntityTypeName.UserId = PageBase.UserId;
            ObjEntityTypeName.CompanyId = PageBase.ClientId;
            string[] str = { "UserID", "EntityTypeName" };
            PageBase.DropdownBinding(ref ddlEntityTypeName, ObjEntityTypeName.GetEntityTypeName(), str);

        };
    }
    void FillApprovalStatus()
    {
        using (ClsExpense ObjEntityType = new ClsExpense())
        {

            ddlApprovalStatus.Items.Clear();
            string[] str = { "ExpenseStatusId", "ExpenseStatus" };
            ObjEntityType.CompanyId = PageBase.ClientId;
            ObjEntityType.UserId = PageBase.UserId;
            PageBase.DropdownBinding(ref ddlApprovalStatus, ObjEntityType.GetApprovalStatus(), str);

        };
    }
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        ApproveRejectExpense(2);
    }

   
    protected void btnReject_Click(object sender, EventArgs e)
    {
        ApproveRejectExpense(3);
    }
    public void SearchExpenseDetailData(int pageno)
    {
        ClsExpense objExpense;
        try
        {
            ViewState["TotalRecords"] = 0;
            ViewState["CurrentPage"] = pageno;
            using (objExpense = new ClsExpense())
            {
                if (ucFromDate.Date == "" && ucToDate.Date == "")
                { ;}
                else
                {
                    objExpense.FromDate = Convert.ToDateTime(ucFromDate.Date);
                    objExpense.Todate = Convert.ToDateTime(ucToDate.Date);
                }
                objExpense.UserId = PageBase.UserId;
                objExpense.CompanyId = PageBase.ClientId;
                objExpense.EntityTypeId = Convert.ToInt32(ddlEntityType.SelectedValue);
                objExpense.EntitytypeUserId = Convert.ToInt16(ddlEntityTypeName.SelectedValue);
                objExpense.ExpenseStatus = Convert.ToInt32(ddlApprovalStatus.SelectedValue);
                objExpense.PageIndex = pageno;
                objExpense.PageSize = 30; /*Convert.ToInt32(PageBase.PageSize);*/

                DataSet ds = objExpense.GetReportExpenseData();
                if (objExpense.TotalRecords > 0)
                {
                    PnlGrid.Visible = true;
                    if (pageno > 0)
                    {
                        gvExpenseDetail.DataSource = ds;
                        gvExpenseDetail.DataBind();
                        PnlGrid.Visible = true;
                        ViewState["TotalRecords"] = objExpense.TotalRecords;
                        ucPagingControl1.TotalRecords = objExpense.TotalRecords;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = pageno;
                        ucPagingControl1.FillPageInfo();
                    }
                    else
                    {

                        string FilenameToexport = "ExpenseDetailData";
                        PageBase.ExportToExecl(ds, FilenameToexport);
                    }
                }
                else
                {
                    ds = null;
                    gvExpenseDetail.DataSource = null;
                    gvExpenseDetail.DataBind();
                    ucMessage1.Visible = true;
                    PnlGrid.Visible = false;
                    ucMessage1.ShowInfo("No Record Found.");

                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void gvExpenseDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    private void ApproveRejectExpense(int Status)
    {
        try
        {
            DataTable dtAddtolist;
            dtAddtolist = new DataTable();
            dtAddtolist.Columns.Add("ExpenseId", typeof(Int64));
            dtAddtolist.Columns.Add("ExpenseDate", typeof(string));
            dtAddtolist.Columns.Add("EntityTypeName", typeof(string));
            dtAddtolist.Columns.Add("EntityType", typeof(string));
            dtAddtolist.Columns.Add("CategoryName", typeof(string));
            dtAddtolist.Columns.Add("Amount", typeof(decimal));
            dtAddtolist.Columns.Add("ApproveAmount", typeof(decimal));
            dtAddtolist.Columns.Add("Status", typeof(string));
            dtAddtolist.Columns.Add("ApproveRemark", typeof(string));
            dtAddtolist.AcceptChanges();

            CheckBox chkaddlist = new CheckBox();
            string Expenseid = string.Empty;
            foreach (GridViewRow grv in gvExpenseDetail.Rows)
            {
                if (grv.RowType == DataControlRowType.DataRow)
                {
                    chkaddlist = (grv.FindControl("chkboxExpenseID") as CheckBox);

                    if (chkaddlist.Checked)
                    {
                        Label lblExpenseDate = (Label)grv.FindControl("lblExpenseDate") as Label;
                        Label lblEntityTypeName = (Label)grv.FindControl("lblEntityTypeName") as Label;
                        Label lblCategoryName = (Label)grv.FindControl("lblCategoryName") as Label;
                        Label lblEntityType = (Label)grv.FindControl("lblEntityType") as Label;
                        Label lblAmount = (Label)grv.FindControl("lblAmount") as Label;
                        TextBox txtApprovedAmount = (TextBox)grv.FindControl("txtApprovedgamount") as TextBox;
                        TextBox txtApproveRemark = (TextBox)grv.FindControl("txtApproveremarks") as TextBox;
                        Label lblStatus = (Label)grv.FindControl("lblStatus") as Label;
                        Expenseid = gvExpenseDetail.DataKeys[grv.RowIndex].Value.ToString();
                        if (txtApprovedAmount.Text != "")
                        {
                            if (System.Text.RegularExpressions.Regex.IsMatch(txtApprovedAmount.Text, "^[0-9.]"))
                            {
                                DataRow dr = null;
                                dr = dtAddtolist.NewRow();
                                dr["ExpenseId"] = Convert.ToInt64(Expenseid);
                                dr["ExpenseDate"] = lblExpenseDate.Text;
                                dr["EntityTypeName"] = lblEntityTypeName.Text;
                                dr["EntityType"] = lblEntityType.Text;
                                dr["CategoryName"] = lblCategoryName.Text;
                                dr["Amount"] = Convert.ToDecimal(lblAmount.Text);
                                dr["ApproveAmount"] = Convert.ToDecimal(txtApprovedAmount.Text.Trim());
                                dr["Status"] = lblStatus.Text;
                                dr["ApproveRemark"] = txtApproveRemark.Text.Trim();
                                dtAddtolist.Rows.Add(dr);
                                dtAddtolist.AcceptChanges();
                            }
                            else
                            {
                                ucMessage1.Visible = true;
                                ucMessage1.ShowWarning("Approve amount should be numeric value.");
                                return;
                            }
                        }
                        else
                        {
                            ucMessage1.Visible = true;
                            ucMessage1.ShowWarning("Approve amount can not be blank.");
                            return;

                        }
                    }
                    if (Expenseid == string.Empty && Expenseid != null)
                    {
                        ucMessage1.Visible = true;
                        ucMessage1.ShowWarning("No record in list for Approve/Reject.");
                    }
                }
            }
            if (dtAddtolist.Rows.Count > 0)
            {

                using (ClsExpense objSave = new ClsExpense())
                {
                    objSave.ExpenseStatus = Status;
                    objSave.CompanyId = PageBase.ClientId;
                    objSave.UserId = PageBase.UserId;
                    objSave.dtSavelist = dtAddtolist;
                    objSave.CallingFrom = 0;
                    objSave.SaveApproveReject();
                    if (objSave.OutParam == 0)
                    {
                        if (Status==2)
                        {
                            ucMessage1.Visible = true;
                            ucMessage1.ShowSuccess("Expense Approve Successfully.");
                            SearchExpenseDetailData(1);
                            return;
                        }
                        else
                        {
                            ucMessage1.Visible = true;
                            ucMessage1.ShowSuccess("Expense Reject Successfully.");
                            SearchExpenseDetailData(1);
                            return;
                        }
                        
                        
                    }
                    else if (objSave.OutParam == 1 && objSave.ErrorMessage != "")
                    {
                        ucMessage1.Visible = true;
                        ucMessage1.ShowError(objSave.ErrorMessage);
                        return;
                    }
                   
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
    private void CheckState(bool p)
    {
        foreach (GridViewRow row in gvExpenseDetail.Rows)
        {
            CheckBox chkcheck = (CheckBox)row.FindControl("chkboxPartID");
            chkcheck.Checked = p;
        }
    }
    public DataTable GetBlankTableError()
    {
        DataTable Detail = new DataTable();
        Detail.Columns.Add("ExpenseID");
        Detail.Columns.Add("ExpenseDate");
        Detail.Columns.Add("EntityTypeName");
        Detail.Columns.Add("EntityType");
        Detail.Columns.Add("CategoryName");
        Detail.Columns.Add("Amount");
        Detail.Columns.Add("ApproveAmount");
        Detail.Columns.Add("Status");
        Detail.Columns.Add("ReasonForInvalid");
        return Detail;
    }
    private void InsertData(DataTable objdt)
    {
        if (objdt != null)
        {
            DataSet objds = new DataSet();

            if (objdt != null && objdt.Rows.Count > 0)
            {

                try
                {
                    if (ViewState["TobeUploaded"] != null)
                    {
                        OpenXMLExcel objexcel = new OpenXMLExcel();
                        string strUploadedFileNameFromViewState = ViewState["TobeUploaded"].ToString();
                        DataSet DsXML = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + ViewState["TobeUploaded"].ToString());

                        DataTable dtUploadQueue = DsXML.Tables[0];
                        using (ClsExpense objDetail = new ClsExpense())
                        {
                            objDetail.UserId = PageBase.UserId;
                            objDetail.CompanyId = PageBase.ClientId;
                            objDetail.dtSavelist = dtUploadQueue;
                            objDetail.CallingFrom = 1;
                            DataSet dtInvalidRecordSet = objDetail.SaveApproveReject();
                            Int32 result = objDetail.OutParam;
                            if (result == 0)
                            {
                                ucMessage1.ShowSuccess("Data Uploaded Successfully");
                                return;
                            }
                            if (result == 1 && objDetail.ErrorMessage != "")
                            {
                                ucMessage1.ShowError(objDetail.ErrorMessage);
                                return;
                            }
                            if (result == 1 && dtInvalidRecordSet != null && dtInvalidRecordSet.Tables[0].Rows.Count > 0)
                            {
                                DataSet ds = new DataSet();
                                string strFileName = "Invalid Data" + DateTime.Now.Ticks;
                                ExportInExcel(dtInvalidRecordSet, strFileName);
                                hlnkInvalid.Visible = true;
                                hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                hlnkInvalid.Text = "Invalid Data";
                                ucMessage1.Visible = true;
                                ucMessage1.ShowInfo("Please click on Invalid data to check the error obtained");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ucMessage1.ShowError(ex.Message);
                }
            }
            else
            {
                ucMessage1.ShowInfo("File is empty!");
            }
        }

    }
    private void ExportInExcel(DataSet DsError, string strFileName)
    {
        if (DsError != null && DsError.Tables.Count > 0)
        {
            PageBase.ExportToExeclV2(DsError, strFileName, DsError.Tables.Count);
        }
    }
}