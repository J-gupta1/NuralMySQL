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
 * Created On: 10-May-2019
 * Module : Upload Saleschannel Brand Mapping.
 * ====================================================================================================
 * Change Log :
 * 22-Jul-2019, Balram Jha, #CC01, Entity type load for brand-category. 
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

public partial class Masters_SalesChannel_UploadBrandSalesChannelMapping : PageBase
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
        if (!IsPostBack)
        {
            FillBrand();
            FillSalesChannelType();
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
                            objValidateFile.ExcelFileNameInTable = "BrandSalesChannelMapping";

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
                        Tvp = GettvpTableBrandSalMappingSB();
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
                        using (ClsSalesChannelBrandMapping objP1 = new ClsSalesChannelBrandMapping())
                        {
                            objP1.UserID = PageBase.UserId;
                            objP1.SalesChannelID = PageBase.SalesChanelID;
                            objP1.TransUploadSession = Convert.ToString(ViewState["strEntityInfoGUID"]);
                            DataSet ds = objP1.InsertSalesChannelBrandInfoSBBCP();
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
                bulkCopy.DestinationTableName = "BulkUploadSaleschannelBrandMapping";
                bulkCopy.ColumnMappings.Add("EntityType", "EntityType");
                bulkCopy.ColumnMappings.Add("EntityCode", "EntityCode");
                bulkCopy.ColumnMappings.Add("BrandCategoryCode", "BrandCategoryCode");
                bulkCopy.ColumnMappings.Add("Status", "Status");
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
            DataSet dsTemplateCode = new DataSet();
            using (ClsSalesChannelBrandMapping objSalesData = new ClsSalesChannelBrandMapping())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.SalesChannelID = PageBase.SalesChanelID;
                dsTemplateCode = objSalesData.GetAllTemplateData();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "EntityDetail", "BrandDetail" };
                    ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, Convert.ToInt16(EnumData.eTemplateCount.ePrice + 1));
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
    public DataTable GettvpTableBrandSalMappingSB()
    {
        DataTable Detail = new DataTable();
        Detail.Columns.Add("EntityType");
        Detail.Columns.Add("EntityCode");
        Detail.Columns.Add("BrandCategoryCode");
        Detail.Columns.Add("Status");
        return Detail;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        grdSalesChannelList.PageIndex = 0;
        if (Convert.ToInt32(ddlSalesChannelType.SelectedValue) == 0)
        {
            ucMsg.Visible = true;
            ucMsg.ShowError("Please select saleschannel type");
            return;
        }
        ucMsg.Visible = false;
        FillGrid();
    }
    protected void btnCancel_Click1(object sender, EventArgs e)
    {
        try
        {
            ddlBrandName.SelectedValue = "0";
            ddlProductCategory.SelectedValue = "0";
            ddlSalesChannelType.SelectedValue = "0";
            ucMsg.Visible = false;
            pnlHide.Visible = false;
            btnExprtToExcel.Visible = false;
            grdSalesChannelList.DataSource = null;
            grdSalesChannelList.DataBind();
        }
        catch (Exception ex)
        {

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
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnExprtToExcel_Click(object sender, EventArgs e)
    {
        try
        {
          DataTable  dt = new DataTable();

          using (ClsSalesChannelBrandMapping ObjSalesChannel = new ClsSalesChannelBrandMapping())
            {
                if (ddlSalesChannelType.SelectedValue != "0")
                {
                    ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(ddlSalesChannelType.SelectedValue);
                }
                ObjSalesChannel.ProductCategoryId = Convert.ToInt32(ddlProductCategory.SelectedValue);
                ObjSalesChannel.BrandId = Convert.ToInt16(ddlBrandName.SelectedValue);
                dt = ObjSalesChannel.GetSalesChannelInfoForProductCategory();
            };

            if (dt.Rows.Count > 0)
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);    
                dtcopy.AcceptChanges();
                dtcopy.Tables[0].AcceptChanges();   
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "SalesChannelBrandMappingDetails";
                PageBase.RootFilePath = FilePath;
                ZedService.Utility.ZedServiceUtil.ExportToExecl(dtcopy, FilenameToexport);
            }
            else
            {
                ucMsg.ShowError(Resources.Messages.NoRecord);

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
        }
    }
    protected void grdSalesChannelList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdSalesChannelList.PageIndex = e.NewPageIndex;
        FillGrid();
    }
    protected void grdSalesChannelList_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    void FillBrand()
    {
        using (ClsSalesChannelBrandMapping ObjProduct = new ClsSalesChannelBrandMapping())
        {

            System.Data.DataSet dsresult = new DataSet();
            dsresult = ObjProduct.GetAllBrandandProductCategory();
            if (dsresult.Tables.Count > 0)
            {
                ddlBrandName.DataSource = dsresult.Tables[1];
                ddlBrandName.DataTextField = "BrandName";
                ddlBrandName.DataValueField = "BrandID";
                ddlBrandName.DataBind();
                ddlBrandName.Items.Insert(0, new ListItem("Select", "0"));

                ddlProductCategory.DataSource = dsresult.Tables[0];
                ddlProductCategory.DataTextField = "ProductCategoryName";
                ddlProductCategory.DataValueField = "ProductCategoryID";
                ddlProductCategory.DataBind();
                ddlProductCategory.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddlBrandName.Items.Insert(0, new ListItem("Select", "0"));
                ddlProductCategory.Items.Insert(0, new ListItem("Select", "0"));
            }

        };
    }
    void FillSalesChannelType()
    {
        using (ClsSalesChannelBrandMapping ObjSalesChannel = new ClsSalesChannelBrandMapping())
        {
            ObjSalesChannel.SalesChannelTypeID = SalesChanelTypeID;//#CC01 added
            String[] StrCol = new String[] { "SalesChannelTypeID", "SalesChannelTypeName" };
            PageBase.DropdownBinding(ref ddlSalesChannelType, ObjSalesChannel.GetSalesChannelTypeForBrand(), StrCol);


        };
    }
    void FillGrid()
    {

        DataTable Dt = new DataTable();
        using (ClsSalesChannelBrandMapping ObjSalesChannel = new ClsSalesChannelBrandMapping())
        {
            if (ddlSalesChannelType.SelectedValue != "0")
            {
                ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(ddlSalesChannelType.SelectedValue);
            }
            ObjSalesChannel.ProductCategoryId = Convert.ToInt32(ddlProductCategory.SelectedValue);
            ObjSalesChannel.BrandId = Convert.ToInt32(ddlBrandName.SelectedValue);
            Dt = ObjSalesChannel.GetSalesChannelInfoForProductCategory();
        };
        if (Dt != null && Dt.Rows.Count > 0)
        {
            grdSalesChannelList.Visible = true;
            grdSalesChannelList.DataSource = Dt;
            grdSalesChannelList.DataBind();
            pnlHide.Visible = true;
            btnExprtToExcel.Visible = true;

        }
        else
        {
            grdSalesChannelList.Visible = false;
            grdSalesChannelList.DataSource = null;
            grdSalesChannelList.DataBind();
            ucMsg.ShowInfo(Resources.Messages.NoRecord);
            pnlHide.Visible = false;
            btnExprtToExcel.Visible = false;

        }
    }
}