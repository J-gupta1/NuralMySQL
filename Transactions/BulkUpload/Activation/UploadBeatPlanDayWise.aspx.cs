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
/*
 * 14-sept-2021 Vaibhav Pandey,this page is created for Creating Generic Beat Plan
 */
public partial class Transactions_BulkUpload_Activation_UploadBeatPlanDayWise : PageBase
{
    DataTable dtNew = new DataTable();
    object objSum;
    int counter = 0;
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    UploadFile UploadFile = new UploadFile();
    List<String> lstDuplicate = new List<String>();
    DataSet dsErrorProne = new DataSet();
    string strPrimarySessionName = "BeatPlanUploadSession";
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        { 
        BindStatus();
        fillState();
    }
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
                            objValidateFile.ExcelFileNameInTable = "UploadBeatPlanDayWise";

                            objValidateFile.ValidateFileWithCompanyId(false, out objDS, out objSL);
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
                                        InsertData(objDS.Tables[0]);
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
                else
                {
                    ucMsg.ShowInfo("File is empty! Some Mandatory columns has no required data!");
                }
            }
            else
            {
                ucMsg.ShowInfo("File is empty! Some Mandatory columns has no required data!");
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }
    private void BindStatus()
    {
        DataTable dtresult = new DataTable();
        try
        {
            dtresult = clsDoaReport.GetEnumbyTableName("XML_Enum", "BeatPlanStatus");
            if (dtresult.Rows.Count > 0)
            {
                ddlStatus.DataSource = dtresult;
                ddlStatus.DataTextField = "Description";
                ddlStatus.DataValueField = "Value";
                ddlStatus.DataBind();
                ddlStatus.Items.Insert(0, new ListItem("Select", "255"));
            }
            else
            {
                ddlStatus.Items.Insert(0, new ListItem("Select", "255"));
            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
        }
    }
    public DataTable GetBlankTableError()
    {
        DataTable Detail = new DataTable();
        Detail.Columns.Add("BeatPlanName");
        Detail.Columns.Add("BeatPlanDate");
        Detail.Columns.Add("BeatPlanDay");
        Detail.Columns.Add("SalesTeamMember");
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
                        string guid = Guid.NewGuid().ToString();
                        ViewState[strPrimarySessionName] = guid;
                        DsXML.Tables[0].Columns.Add(AddColumn(guid, "TransactionUploadSessionId", typeof(System.String)));
                        DataTable dtUploadQueue = DsXML.Tables[0].Copy();

                        if (dtUploadQueue.Rows.Count > 0)
                        {
                            if (!UserDetailBCP(dtUploadQueue))
                            {
                                ucMsg.ShowError("Error Occured While transferring the data to the server");
                                return;
                            }
                        }
                     
                        using (clsBeatPlan objDetail = new clsBeatPlan())
                        {
                            objDetail.LoginUserId = PageBase.UserId;

                            objDetail.SessionId = guid;
                            objDetail.CompanyId = PageBase.ClientId;
                            DataSet dtInvalidRecordSet = objDetail.InsertGeneric();
                            Int32 result = objDetail.OutParam;
                            if (result == 0)
                            {
                                ucMsg.ShowSuccess("Data Uploaded Successfully");
                                return;
                            }
                            if (result == 1 && objDetail.OutError != "")
                            {
                                ucMsg.ShowError(objDetail.OutError);
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
                                ucMsg.Visible = true;
                                ucMsg.ShowInfo("Please click on Invalid data to check the error obtained");

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ucMsg.ShowError(ex.Message);
                }
            }
            else
            {
                ucMsg.ShowInfo("File is empty!");
            }
        }

    }
    void fillState()
    {
        using (MastersData obj = new MastersData())
        {
            DataTable dt;

            obj.CountrySelectionMode = 1;
            obj.CompanyId = PageBase.ClientId;
            dt = obj.SelectStateInfo();
            String[] colArray = { "StateID", "StateName" };
            PageBase.DropdownBinding(ref cmbSerState, dt, colArray);
            //cmbSerState.Items.Insert(0, new ListItem("Select", "0"));
            cmbSerDistrict.Items.Insert(0, new ListItem("Select", "0"));
            cmbSerCity.Items.Insert(0, new ListItem("Select", "0"));
            //cmbSerTehsil.Items.Insert(0, new ListItem("Select", "0"));

        }

    }
    protected void cmbSerState_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (MastersData objmaster = new MastersData())
        {
            //cmbSerTehsil.Items.Clear();
            //cmbSerTehsil.Items.Insert(0, new ListItem("Select", "0"));
            cmbSerCity.Items.Clear();
            cmbSerCity.Items.Insert(0, new ListItem("Select", "0"));
            try
            {
                if (cmbSerState.SelectedValue == "0")
                {
                    cmbSerDistrict.Items.Clear();

                    cmbSerDistrict.Items.Insert(0, new ListItem("Select", "0"));


                }
                else
                {

                    cmbSerDistrict.Items.Clear();
                    objmaster.DistrictStateId = Convert.ToInt16(cmbSerState.SelectedValue.ToString());
                    objmaster.DistrictSelectionMode = 1;
                    objmaster.CompanyId = PageBase.ClientId;
                    DataTable dtdistfil = objmaster.SelectDistrictInfo();
                    String[] colArray = { "DistrictID", "DistrictName" };
                    PageBase.DropdownBinding(ref cmbSerDistrict, dtdistfil, colArray);
                    cmbSerDistrict.SelectedValue = "0";
                }
            }
            catch (Exception ex)
            {
                ucMsg.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }
    protected void cmbSerDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (MastersData objmaster = new MastersData())
        {

            try
            {
                if (cmbSerDistrict.SelectedValue == "0")
                {
                    cmbSerCity.Items.Clear();
                    cmbSerCity.Items.Insert(0, new ListItem("Select", "0"));

                }
                else
                {
                    objmaster.CityDistrictId = Convert.ToInt16(cmbSerDistrict.SelectedValue);
                    objmaster.CitySelectionMode = 1;
                    objmaster.CompanyId = PageBase.ClientId;
                    DataTable dtcityfil = objmaster.SelectCityInfo();
                    String[] colArray = { "CityID", "CityName" };
                    PageBase.DropdownBinding(ref cmbSerCity, dtcityfil, colArray);
                    cmbSerCity.SelectedValue = "0";

                }

            }
            catch (Exception ex)
            {
                ucMsg.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        SearchBeatPlanData(ucPagingControl1.CurrentPage);
    }
    public void SearchBeatPlanData(int pageno)
    {
        SalesChannelData objsaleschannel;
        try
        {
            ViewState["TotalRecords"] = 0;
            ViewState["CurrentPage"] = pageno;
            using (objsaleschannel = new SalesChannelData())
            {


                //if (ucFromDate.Date == "" && ucToDate.Date == "")
                //{ ;}
                //else
                //{
                //    objsaleschannel.Fromdate = Convert.ToDateTime(ucFromDate.Date);
                //    objsaleschannel.Todate = Convert.ToDateTime(ucToDate.Date);
                //}
                objsaleschannel.UserID = PageBase.UserId;
                objsaleschannel.StateID = Convert.ToInt16(cmbSerState.SelectedValue);
                objsaleschannel.DistrictID = Convert.ToInt16(cmbSerDistrict.SelectedValue);
                objsaleschannel.CityID = Convert.ToInt16(cmbSerCity.SelectedValue);
                ////objsaleschannel.SelectedFOSTSM = Convert.ToInt32(ddlfosandtsm.SelectedValue);
                //objsaleschannel.SalesChannelTypeID = Convert.ToInt16(ddlsaleschanneltype.SelectedValue);
                //objsaleschannel.SalesChannelID = Convert.ToInt32(ddlsaleschannelname.SelectedValue);
                objsaleschannel.PageIndex = pageno;
                objsaleschannel.PageSize = Convert.ToInt32(PageBase.PageSize);
                objsaleschannel.ActiveStatus = Convert.ToInt16(ddlStatus.SelectedValue);
                objsaleschannel.CompanyId = PageBase.ClientId;

                DataSet ds = objsaleschannel.GetReportGenericBeatPlan();
                if (objsaleschannel.TotalRecords > 0)
                {
                    if (pageno > 0)
                    {
                        gvUploadBeatplan.DataSource = ds;
                        gvUploadBeatplan.DataBind();

                        ViewState["TotalRecords"] = objsaleschannel.TotalRecords;
                        ucPagingControl1.TotalRecords = objsaleschannel.TotalRecords;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = pageno;
                        ucPagingControl1.FillPageInfo();
                    }
                    else
                    {
                        string FilenameToexport = "BeatPlanData";
                        PageBase.ExportToExecl(ds, FilenameToexport);
                    }
                }
                else
                {
                    ds = null;
                    gvUploadBeatplan.DataSource = null;
                    gvUploadBeatplan.DataBind();

                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    public bool UserDetailBCP(DataTable dtUpload)
    {
        try
        {

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "BulkUploadBeatPlandaywise";
                bulkCopy.ColumnMappings.Add("BeatPlanName", "BeatPlanName");
                bulkCopy.ColumnMappings.Add("State", "State");
                bulkCopy.ColumnMappings.Add("District", "District");
                bulkCopy.ColumnMappings.Add("City", "City");
                bulkCopy.ColumnMappings.Add("Area", "Area");
                bulkCopy.ColumnMappings.Add("BeatPlanForCode", "BeatPlanForCode");
                bulkCopy.ColumnMappings.Add("BeatPlanDate", "BeatPlanDate");
                bulkCopy.ColumnMappings.Add("BeatPlanDay", "BeatPlanDay");
                bulkCopy.ColumnMappings.Add("SalechannelType", "SalechannelType");
                bulkCopy.ColumnMappings.Add("SalesChannelCode", "SalesChannelCode");
                bulkCopy.ColumnMappings.Add("ValidFrom", "ValidFrom");
                bulkCopy.ColumnMappings.Add("ValidTill", "ValidTill");
                bulkCopy.ColumnMappings.Add("TransactionUploadSessionId", "TransactionUploadSessionId");
                bulkCopy.WriteToServer(dtUpload);
                return true;
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
            return false;
        }
    }
    protected void btnExportexcel_Click(object sender, EventArgs e)
    {
        SearchBeatPlanData(-1);
    }
    private void ExportInExcel(DataSet DsError, string strFileName)
    {
        if (DsError != null && DsError.Tables.Count > 0)
        {
            PageBase.ExportToExeclV2(DsError, strFileName, DsError.Tables.Count);
        }
    }
    protected void DwnldReferenceCode_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (clsBeatPlan objSalesData = new clsBeatPlan())
            {
                objSalesData.LoginUserId = PageBase.UserId;
                objSalesData.CompanyId = PageBase.ClientId;
                dsTemplateCode = objSalesData.GetAllTemplateDataForGenericBeatPlan();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "ASM-TSM-FOSDetails", "SalesChannel-RetailerDetails"};
                    ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, 2);
                    if (dsTemplateCode.Tables.Count > 2)
                        dsTemplateCode.Tables.RemoveAt(2);
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsTemplateCode, FilenameToexport, 2, strExcelSheetName);
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
    public void ChangedExcelSheetNames(ref DataSet dsExcel, string[] strExcelSheetName, Int16 intSheetCount)
    {
        Int16 index;
        for (index = 0; index < intSheetCount; index++)
            dsExcel.Tables[index].TableName = strExcelSheetName[index];
        dsExcel.AcceptChanges();

    }
    protected void lnkDownloadTemplate_Click(object sender, EventArgs e)
    {
        try
        {
             DataSet dsTemplateCode = new DataSet();
            if (ddlUploadType.SelectedValue == "1")
            {


                using (clsBeatPlan objBeatplanData = new clsBeatPlan())
                {
                    objBeatplanData.LoginUserId = PageBase.UserId;
                    objBeatplanData.CompanyId = PageBase.ClientId;
                    objBeatplanData.TemplateType = 4;
                    dsTemplateCode = objBeatplanData.GetBeatPlanUploadTemplate();
                    if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                    {

                        String FilePath = Server.MapPath("../../");
                        string FilenameToexport = "BeatPlanUploadAdd";
                        PageBase.RootFilePath = FilePath;
                        string[] strExcelSheetName = { "BeatPlanUploadAdd"};
                        ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, 2);
                        if (dsTemplateCode.Tables.Count > 2)
                            dsTemplateCode.Tables.RemoveAt(2);
                        for (int i = dsTemplateCode.Tables[0].Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = dsTemplateCode.Tables[0].Rows[i];

                            dsTemplateCode.Tables[0].Rows.Remove(dr);

                        }

                        ZedService.Utility.ZedServiceUtil.ExportToExecl(dsTemplateCode, FilenameToexport, 2, strExcelSheetName);
                    }
                    else
                    {
                        ucMsg.ShowInfo(objBeatplanData.OutError.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        SearchBeatPlanData(1);
    }

   
}