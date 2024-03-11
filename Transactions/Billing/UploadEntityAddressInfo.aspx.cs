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
 * Created By : Vijay Kumar Prajapati
 * Created On: 03-May-2019
 * Module : Upload Address Details.
 * ====================================================================================================
 * Change Log :
 * DD-MMM-YYYY, Name, #CCXX, Description. 
 *  ====================================================================================================
*/

#endregion
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

public partial class Transactions_Billing_UploadEntityAddressInfo : PageBase
{
    DataTable dtNew = new DataTable();
    object objSum;
    int counter = 0;
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    UploadFile UploadFile = new UploadFile();
    List<String> lstDuplicate = new List<String>();
    DataSet dsErrorProne = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        if (!IsPostBack)
        {
            FillsalesChannelType();
            filllist(0);
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
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
                            objValidateFile.ExcelFileNameInTable = "EntityAddressInfo";

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
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }
    protected void DwnldReferenceCodeTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (ClsEntityAddressInfo objSalesData = new ClsEntityAddressInfo())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.SalesChannelID = PageBase.SalesChanelID;
                dsTemplateCode = objSalesData.GetAllTemplateData();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "SalesChannelDetail", "RetailerDetail", "AddressType" };
                    ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, Convert.ToInt16(EnumData.eTemplateCount.eSecondary + 1));
                    if (dsTemplateCode.Tables.Count > 3)
                        dsTemplateCode.Tables.RemoveAt(3);
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsTemplateCode, FilenameToexport, 3, strExcelSheetName);
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
    private void InsertData(DataTable dtGRN)
    {
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }
            if (dtGRN != null)
            {
                if (dtGRN.Rows.Count > 0 && dtGRN != null)
                {

                    if (ViewState["TobeUploaded"] != null)
                    {
                        int intResult = 0;
                        DataTable Tvp = new DataTable();
                        DataSet DsExcelDetail = new DataSet();
                        using (CommonData ObjCommom = new CommonData())
                        {
                            Tvp = ObjCommom.GettvpTableEntityInfoSB();
                        }
                        OpenXMLExcel objexcel = new OpenXMLExcel();
                        DsExcelDetail = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + ViewState["TobeUploaded"].ToString());
                        string guid = Guid.NewGuid().ToString();
                        ViewState["strEntityInfoGUID"] = guid;
                        DsExcelDetail.Tables[0].Columns.Add(AddColumn(guid, "TransUploadSession", typeof(System.String)));
                        DsExcelDetail.Tables[0].AcceptChanges();

                        if (DsExcelDetail.Tables[0].Rows.Count > 0)
                        {
                            if (!BulkCopyUpload(DsExcelDetail.Tables[0]))
                            {
                                ucMsg.ShowError("Error Occured While transferring the data to the server");
                                return;
                            }

                        }

                        using (ClsEntityAddressInfo objP1 = new ClsEntityAddressInfo())
                        {
                            objP1.UserID = PageBase.UserId;
                            objP1.SalesChannelID = PageBase.SalesChanelID;
                            objP1.TransUploadSession = Convert.ToString(ViewState["strEntityInfoGUID"]);
                            DataSet ds = objP1.InsertEntityAddressInfoSBBCP();
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0] != null)
                                {
                                    hlnkInvalid.Visible = true;
                                    string strFileName = "InvalidData" + DateTime.Now.Ticks;
                                    ExportInExcel(ds, strFileName);
                                    hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                    hlnkInvalid.Text = "Invalid Data";
                                    ucMsg.Visible = true;
                                    // ucMsg.XmlErrorSource = ds.GetXml();
                                    ucMsg.ShowInfo("Please click on Invalid data to check the error obtained");
                                    return;
                                }
                            }
                            if (objP1.Error != null && objP1.Error != "")
                            {
                                ucMsg.ShowError(objP1.Error);
                                return;
                            }
                            if (intResult == 2)
                            {
                                ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                                return;
                            }
                            ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    public bool BulkCopyUpload(DataTable dtTempTable)
    {
        try
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "BulkUploadEntityAddress";
                bulkCopy.ColumnMappings.Add("EntityType", "EntityType");
                bulkCopy.ColumnMappings.Add("EntityCode", "EntityCode");
                bulkCopy.ColumnMappings.Add("AddressType", "AddressType");
                bulkCopy.ColumnMappings.Add("Street", "Street");
                bulkCopy.ColumnMappings.Add("AddressLine2", "AddressLine2");
                bulkCopy.ColumnMappings.Add("LandMark", "LandMark");
                bulkCopy.ColumnMappings.Add("Pincode", "Pincode");
                bulkCopy.ColumnMappings.Add("CountryName", "CountryName");
                bulkCopy.ColumnMappings.Add("CityName", "CityName");
                bulkCopy.ColumnMappings.Add("StateName", "StateName");
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
    protected void ddlsaleschannelType_SelectedIndexChanged(object sender, EventArgs e)
    {
        AutoCompleteExtenderPartName.ContextKey = ddlsaleschannelType.SelectedValue;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ucMsg.Visible = false;
     

        SearchEntityAddressinfoData(1);

    }
    public void SearchEntityAddressinfoData(int pageno)
    {
        try
        {
            if (ViewState["PageIndex"] == null)
            {

                ViewState["PageIndex"] = 1;
            }
            using (ClsEntityAddressInfo obj_ClsEntityAddressInfo = new ClsEntityAddressInfo())
            {
                obj_ClsEntityAddressInfo.STateid = Convert.ToInt32(ddlstate.SelectedValue);
                obj_ClsEntityAddressInfo.Cityid = Convert.ToInt32(ddlcity.SelectedValue);
                obj_ClsEntityAddressInfo.SalesChannelID = FillSaleschannelName(); //Convert.ToInt32(ddlsaleschannelname.SelectedValue);
                obj_ClsEntityAddressInfo.SalesChannelTypeID = Convert.ToInt32(ddlsaleschannelType.SelectedValue);
                obj_ClsEntityAddressInfo.Status = Convert.ToInt16(ddlstatus.SelectedValue);
                obj_ClsEntityAddressInfo.pageindex = pageno;
                obj_ClsEntityAddressInfo.pagesize = Convert.ToInt32(PageBase.PageSize);

                DataSet ds = obj_ClsEntityAddressInfo.GETADDRESSDETAILS();
                if (pageno == -1)
                {
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        

                        string FilenameToexport = "EntityAddressInfo";
                        ucMsg.ShowInfo("Exported Successfully");
                        PageBase.ExportToExecl(ds, FilenameToexport);
                       
                    }
                    else
                    {
                        ucMsg.ShowInfo("No Records Found");
                    }
                }
                else
                {
                    gvAddressinfo.DataSource = ds;
                    gvAddressinfo.DataBind();

                    if (obj_ClsEntityAddressInfo.Totalrecords > 0)
                    {
                        int total = obj_ClsEntityAddressInfo.Totalrecords;
                        ucPagingControl1.Visible = true;
                        ViewState["TotalRecords"] = total;
                        ucPagingControl1.TotalRecords = total;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = Convert.ToInt32(ViewState["PageIndex"]);
                        ucPagingControl1.FillPageInfo();
                    }
                }
            }
             
        }
        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message);
            
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }
    protected void btnExportexcel_Click(object sender, EventArgs e)
    {
        SearchEntityAddressinfoData(-1);
    }
    int FillSaleschannelName()
    {
        Int32 salechannelname = 0;
        using (ClsEntityAddressInfo ObjSalesChannel = new ClsEntityAddressInfo())
        {
            ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(ddlsaleschannelType.SelectedValue);
            
            using (DataTable dt= ObjSalesChannel.BindSalesChannelName() )
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if ((dr["SalesChannelName"].ToString().Trim() + '(' + dr["SalesChannelCode"].ToString() + ')').ToLower() == txt_saleschannelname.Text.ToLower())
                    { 
                        salechannelname = Convert.ToInt32(dr["SalesChannelId"]);
                        return salechannelname;
                    }

                }

            }
        };
        return salechannelname;
    }
    void FillsalesChannelType()
    {
        using (ClsEntityAddressInfo ObjSalesChannel = new ClsEntityAddressInfo())
        {

            ddlsaleschannelType.Items.Clear();
            string[] str = { "SalesChannelTypeID", "SalesChannelTypeName" };
            PageBase.DropdownBinding(ref ddlsaleschannelType, ObjSalesChannel.GetSalesChannelTypeV5API(), str);






        };
    }
    //public void SearchEntityAddressInfoData(int pageno)
    //{
    //    ClsEntityAddressInfo objentityaddress;
    //    try
    //    {
    //        ViewState["TotalRecords"] = 0;
    //        ViewState["CurrentPage"] = pageno;
    //        using (objentityaddress = new ClsEntityAddressInfo())
    //        {


    //            if (ucFromDate.Date == "" && ucToDate.Date == "")
    //            { ;}
    //            else
    //            {
    //                objentityaddress.FromDate = Convert.ToDateTime(ucFromDate.Date);
    //                objentityaddress.Todate = Convert.ToDateTime(ucToDate.Date);
    //            }
    //            objentityaddress.UserID = PageBase.UserId;
    //            objentityaddress.SalesChannelTypeID = Convert.ToInt32(ddlsaleschannelType.SelectedValue);
    //            objentityaddress.SalesChannelRetailerID = Convert.ToInt32(ddlsaleschannelname.SelectedValue);
    //            objentityaddress.PageIndex = pageno;
    //            objentityaddress.PageSize = Convert.ToInt32(PageBase.PageSize);

    //            DataSet ds = objentityaddress.GetReportPaymentData();
    //            if (objentityaddress.TotalRecords > 0)
    //            {
    //                PnlGrid.Visible = true;
    //                if (pageno > 0)
    //                {
    //                    gvPaymentDetail.DataSource = ds;
    //                    gvPaymentDetail.DataBind();
    //                    ViewState["TotalRecords"] = objentityaddress.TotalRecords;
    //                    ucPagingControl1.TotalRecords = objentityaddress.TotalRecords;
    //                    ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
    //                    ucPagingControl1.SetCurrentPage = pageno;
    //                    ucPagingControl1.FillPageInfo();
    //                }
    //                else
    //                {
    //                    string FilenameToexport = "PaymentDetailData";
    //                    PageBase.ExportToExecl(ds, FilenameToexport);
    //                }
    //            }
    //            else
    //            {
    //                ds = null;
    //                gvPaymentDetail.DataSource = null;
    //                gvPaymentDetail.DataBind();
    //                ucm.Visible = true;
    //                ucMessage1.ShowInfo("No Record Found.");

    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
    //        PageBase.Errorhandling(ex);
    //    }
    //}
    public void filllist(int stateid)
    {
        using (ClsEntityAddressInfo clsentityaddress = new ClsEntityAddressInfo())
        {
            if (stateid == 0)
            {
                string[] str = { "StateID", "StateName" };
                PageBase.DropdownBinding(ref ddlstate, clsentityaddress.GETstatevisecity(stateid), str);
            }
            else
            {
                string[] str = { "cityId", "CityName" };
                PageBase.DropdownBinding(ref ddlcity, clsentityaddress.GETstatevisecity(stateid), str);

            }
        }
    }
    protected void ddlstate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlstate.SelectedValue != "0")
        {
            ddlcity.Items.Clear();
            filllist(Convert.ToInt32(ddlstate.SelectedValue));
        }
    }
    protected void ucPagingControl1_SetControlRefresh()
    {
        ViewState["PageIndex"] = null;
        ViewState["PageIndex"] = ucPagingControl1.CurrentPage;
        SearchEntityAddressinfoData(ucPagingControl1.CurrentPage);

    }
    protected void gvAddressinfo_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddressActive")
        {
            statuschange(Convert.ToInt64(e.CommandArgument));
        }
    }
    public void statuschange(Int64 EntityAddressID)
    {
        try
        {
            using (ClsEntityAddressInfo obj = new ClsEntityAddressInfo())
            {
                obj.UserID = PageBase.UserId;
                obj.EntityAddressID = EntityAddressID;
                obj.EntityAddressInfo();
                if (obj.Error.Trim().Length > 0) { ucMsg.ShowInfo(obj.Error); }
                else
                { SearchEntityAddressinfoData(1); }
            }
        }
        catch (Exception EX)
        {
            ucMsg.ShowInfo(EX.Message);
        }
    }
}