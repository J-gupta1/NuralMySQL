#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
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
* Created On: 29-Jul-2016 
* ====================================================================================================
* Reviewed By :
* DD-MMM-YYYY, Name, #CCXX, Description
 ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 05-Sep-2016, Sumit Maurya, #CC01, SalesChannelType and SalesChannelValue supplied to filter details only for ND.
 * 26-Jan-2018,Vijay Kumar Prajapati,#CC02,Add Relation type for zedsalesv5.
 * 22-Oct-2018, Sumit Maurya, #CC03, new column Added for BCP to resolve issue of same retailer name under different saleschannels (Done for Karbonn)
 ====================================================================================================
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
using System.Data.SqlClient;

 /* 29-Jan-2018,Rajnish Kumar ,#CC02,EntitytypeId,EntityMappingRelationTypeId parameter is added to save data.*/
public partial class Masters_HO_Admin_BulkUploadMapping : PageBase
{
    DataTable dtNew = new DataTable();
    object objSum;
    int counter = 0;
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    UploadFile UploadFile = new UploadFile();
    List<String> lstDuplicate = new List<String>();
    DataSet dsErrorProne = new DataSet();
    Int32 CallingMode;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMsg.ShowControl = false;
            if (!IsPostBack)
            {
                BindddlMappingType();
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());

        }

    }






    void ClearForm()
    {

        ucMsg.Visible = false;
        hlnkInvalid.Visible = false;

    }




    protected void DwnldReferenceCodeTemplate_Click(object sender, EventArgs e)
    {

        try
        {

            DataSet dsReferenceCode = new DataSet();
            DataSet Ds = new DataSet();
            /*using (SalesChannelData objSalesData = new SalesChannelData())*/
            if (Convert.ToInt32(ddlSaleChannelMappingType.SelectedValue) > 0 && Convert.ToInt32(ddlSaleChannelRelationType.SelectedValue) > 0)
            {
                using (SalesChannelData objSalesData = new SalesChannelData())
                {/* #CC01 Add Start */
                    objSalesData.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
                    objSalesData.SalesChannelID = PageBase.SalesChanelID;
                    objSalesData.EntityMappingTypeId = Convert.ToInt32(ddlSaleChannelMappingType.SelectedValue);
                    objSalesData.EntityMappingRelationId = Convert.ToInt32(ddlSaleChannelRelationType.SelectedValue);



                    /* #CC01 Add End */
                    dsReferenceCode = objSalesData.GetBulkUploadMappingData();

                    //dsReferenceCode.Tables.Remove(dsReferenceCode.Tables[0]);
                    //dsReferenceCode.AcceptChanges();
                    if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
                    {

                        String FilePath = Server.MapPath("../../../");
                        string FilenameToexport = "Reference Code List";
                        PageBase.RootFilePath = FilePath;
                        /*#CC02 Added Started*/
                        //string[] strExcelSheetName = { "FOSList", "SalesHierarchyList","MappingTypeName"};
                        string[] s = ddlSaleChannelRelationType.SelectedItem.Text.ToString().Split(new String[] { " to " }, StringSplitOptions.None);
                        string[] strExcelSheetName = { s[0], s[1] };

                        ZedService.Utility.ZedServiceUtil.ExportToExecl(dsReferenceCode, FilenameToexport, 2, strExcelSheetName);
                        /*#CC02 Added End*/

                        /*#CC02 Commented Started
                           dsReferenceCode.Tables[0].TableName = "FOSList";
                          dsReferenceCode.Tables[1].TableName = "SalesHierarchyList";                  
                          PageBase.ExportToExecl(dsReferenceCode, FilenameToexport, EnumData.eTemplateCount.ePrimarysales1 + 1);
                         #CC02 Commented End */
                    }
                    else
                    {
                        ucMsg.ShowInfo(Resources.Messages.NoRecord);
                    }



                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }




    public DataTable GetBlankTableError()
    {
        DataTable Detail = new DataTable();
        Detail.Columns.Add("InvoiceNumber");
        Detail.Columns.Add("WarehouseCode");
        Detail.Columns.Add("SalesChannelCode");
        Detail.Columns.Add("ReasonForInvalid");
        return Detail;
    }



    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("BulkUploadMapping.aspx", false);
        }
        catch (Exception ex)
        {

        }
    }
    public void BindddlMappingType()
    {
        try
        {
            /*SalesChannelData objsales = new SalesChannelData();*/
            SalesChannelData objsales = new SalesChannelData();
            DataSet ds = new DataSet();
            /* #CC01 Add Start */
            objsales.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
            objsales.SalesChannelID = PageBase.SalesChanelID;
            /* #CC01 Add End */
            ds = objsales.GetEntityMappingType();
            if (ds != null)
            {
                /*#CC02 Commented started
                if (ds.Tables.Count > 0)
                {
                    ddlSaleChannelMappingType.Items.Clear();
                    String[] StrCol = new String[] { "ParentToChildMappingTypeID", "ParentToChildMappingType" };
                    PageBase.DropdownBinding(ref ddlSaleChannelMappingType, ds.Tables[0], StrCol);
                }
                  #CC02 Commented End*/
                /*#CC02 Added Started*/
                if (ds.Tables.Count > 0)
                {
                    ddlSaleChannelMappingType.Items.Clear();
                    String[] StrCol = new String[] { "EntityMappingTypeID", "Keyword" };
                    PageBase.DropdownBinding(ref ddlSaleChannelMappingType, ds.Tables[0], StrCol);
                }
                /*#CC02 Added End*/
            }
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


    protected void btnUpload_Click(object sender, EventArgs e)
    {


        try
        {
            ClearForm();
            DataSet objDS = new DataSet();
            byte isSuccess = 1;
            Int16 UploadCheck = 0;
            String RootPath = Server.MapPath("../../../");
            UploadFile.RootFolerPath = RootPath;

            UploadCheck = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            ViewState["TobeUploaded"] = strUploadedFileName;
            if (UploadCheck == 1)
            {
                UploadFile.UploadedFileName = strUploadedFileName;
                UploadFile.UploadValidationType = EnumData.eUploadExcelValidationType.eRetailerUpload;
               
                    isSuccess = UploadFile.uploadValidExcelRetailer(ref objDS, "BulkUploadMappingParentToChild");
                


                switch (isSuccess)
                {
                    case 0:
                        ucMsg.ShowInfo("Invalid template kindly download template again and then upload.");
                        break;
                    case 2:
                        hlnkInvalid.Visible = true;
                        string strFileName = "InvalidData" + DateTime.Now.Ticks;

                        ucMsg.ShowInfo("There is error in data. Please refer Invalid Data link. No Data processed.");
                        DataView dvError = objDS.Tables[0].DefaultView;
                        dvError.RowFilter = "Error<>''";
                        DataTable dtError = dvError.ToTable();
                        DataSet dsError = new DataSet();
                        dsError.Tables.Add(dtError);
                        ExportInExcel(dsError, strFileName);

                        /*ExportInExcel(objDS, strFileName);*/
                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                        hlnkInvalid.Text = "Invalid Data";
                        break;
                    case 1:
                        InsertData(objDS.Tables[0]);
                        break;
                }

            }
            else if (UploadCheck == 2)
            {
                ucMsg.ShowInfo(Resources.Messages.UploadXlxs);

            }
            else if (UploadCheck == 3)
            {
                ucMsg.ShowInfo(Resources.Messages.SelectFile);


            }

        }
        catch (Exception ex)
        {

            ucMsg.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);

        }
    }
    private void InsertData(DataTable DtBulkUpload)
    {
        try
        {
            if ((CreatedBcpData(DtBulkUpload)) == true)
            {
                
                SalesChannelData objSales = new SalesChannelData();
                objSales.SessionID = Convert.ToString(DtBulkUpload.Rows[0]["SessionID"]);
                objSales.MappingTypeID = Convert.ToInt16(DtBulkUpload.Rows[0]["MappingType"]);
                /* #CC01 Add Start */
                objSales.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
                objSales.SalesChannelID = PageBase.SalesChanelID;
                /* #CC01 Add End */
                /* #CC02 Add Start */
                objSales.EntityMappingTypeId = Convert.ToInt32(ddlSaleChannelMappingType.SelectedValue);
                objSales.EntityMappingRelationId = Convert.ToInt32(ddlSaleChannelRelationType.SelectedValue);
                objSales.UserID = Convert.ToInt32(PageBase.UserId);
                /* #CC02 Add End */
                DataSet dsResult = new DataSet();
                dsResult = objSales.SaveBulkUploadMapping();

                int result = objSales.intOutParam;
                if (result == 0)
                {
                    ucMsg.ShowSuccess("Data uploaded successfully.");
                    ddlSaleChannelMappingType.SelectedValue = "0";
                    ddlSaleChannelRelationType.SelectedValue = "0";

                }
                else if (result == 1 && dsResult != null && dsResult.Tables.Count > 0)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        hlnkInvalid.Visible = true;
                        string strFileName = "InvalidData" + DateTime.Now.Ticks;
                        ucMsg.ShowInfo("There is error in data. Please refer Invalid Data link. No Data processed.");
                        ExportInExcel(dsResult, strFileName);
                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                        hlnkInvalid.Text = "Invalid Data";
                        //ucMsg.XmlErrorSource = objSales.XMLList;
                    }
                }
                else
                {
                    //ucMsg.ShowError(objSales.Error);
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }

    }

    public bool CreatedBcpData(DataTable dtUpload)
    {
        bool result = false;
        try
        {
            string guid = Guid.NewGuid().ToString();
            dtUpload.Columns.Add(AddColumn(Convert.ToString(1), "Status2", typeof(int)));
            dtUpload.Columns.Add(AddColumn(guid, "SessionID", typeof(System.String)));
            dtUpload.Columns.Add(AddColumn(Convert.ToString(PageBase.UserId), "CreatedBy", typeof(int)));
            dtUpload.Columns.Add(AddColumn(Convert.ToString(ddlSaleChannelMappingType.SelectedValue), "MappingType", typeof(int)));

            if (dtUpload.Columns.Contains("FOSName"))
                dtUpload.Columns["FOSName"].ColumnName = "ParentCode";
            if (dtUpload.Columns.Contains("RetailerCode"))
                dtUpload.Columns["RetailerCode"].ColumnName = "ChildCode";
            dtUpload.AcceptChanges();
            int i = PageBase.UserId;

            if (UploadCurrentOutStandingBcp(dtUpload) == true)
            {
                /*ucMsg.ShowSuccess("BCP done sucessfully");*/
                result = true;
            }
            /*else
            {
                ucMsg.ShowError("Error while doing BCP");
            }*/
            return result;
        }
        catch (Exception ex)
        {
            return result;
        }

    }

    public bool UploadCurrentOutStandingBcp(DataTable dtUpload)
    {
        try
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "BulkUploadMapping";
                bulkCopy.ColumnMappings.Add("MappingType", "MappingType");
                bulkCopy.ColumnMappings.Add("ParentCode", "ParentCode");
                bulkCopy.ColumnMappings.Add("ChildCode", "ChildCode");
                bulkCopy.ColumnMappings.Add("Status", "Status");
                bulkCopy.ColumnMappings.Add("Status2", "Status2");
                bulkCopy.ColumnMappings.Add("SessionID", "SessionID");
                bulkCopy.ColumnMappings.Add("CreatedBy", "CreatedBy");
                bulkCopy.ColumnMappings.Add("FromParentCode", "FromParentCode"); /* #CC03 Added */
                bulkCopy.WriteToServer(dtUpload);
                return true;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    /*#CC02 Added Started*/
    protected void ddlSaleChannelMappingType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindddlRelationType();
    }
    public void BindddlRelationType()
    {
        try
        {
            SalesChannelData objsales = new SalesChannelData();
            DataSet ds = new DataSet();

            objsales.SalesChannelMappingTypeID = Convert.ToInt16(ddlSaleChannelMappingType.SelectedValue);
            objsales.SalesChannelID = PageBase.SalesChanelID;

            ds = objsales.GetBulkUploadMappingDataForRelationType();
            if (ds != null)
            {
              
                if (ds.Tables.Count > 0)
                {
                    ddlSaleChannelRelationType.Items.Clear();
                    String[] StrCol = new String[] { "EntityMappingTypeRelationID", "RelationType" };
                    PageBase.DropdownBinding(ref ddlSaleChannelRelationType, ds.Tables[0], StrCol);
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }
    /*#CC02 Added End*/
    protected void DownloadMappedData_Click(object sender, EventArgs e)
    {
        
        try
        {

            DataSet dsReferenceCode = new DataSet();
            DataSet Ds = new DataSet();
            /*using (SalesChannelData objSalesData = new SalesChannelData())*/
            if (Convert.ToInt32(ddlSaleChannelMappingType.SelectedValue) > 0 && Convert.ToInt32(ddlSaleChannelRelationType.SelectedValue) > 0)
            {
                using (SalesChannelData objSalesData = new SalesChannelData())
                {/* #CC01 Add Start */
                    objSalesData.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
                    objSalesData.SalesChannelID = PageBase.SalesChanelID;
                    objSalesData.EntityMappingTypeId = Convert.ToInt32(ddlSaleChannelMappingType.SelectedValue);
                    objSalesData.EntityMappingRelationId = Convert.ToInt32(ddlSaleChannelRelationType.SelectedValue);
                    objSalesData.ComingFrom = 1;



                    /* #CC01 Add End */
                    dsReferenceCode = objSalesData.GetBulkMappedUnmappedData();

                    //dsReferenceCode.Tables.Remove(dsReferenceCode.Tables[0]);
                    //dsReferenceCode.AcceptChanges();
                    if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
                    {

                        String FilePath = Server.MapPath("../../../");
                        string FilenameToexport = "Mapped  Data";
                        PageBase.RootFilePath = FilePath;
                        /*#CC02 Added Started*/
                        //string[] strExcelSheetName = { "FOSList", "SalesHierarchyList","MappingTypeName"};
                        //string[] s = ddlSaleChannelRelationType.SelectedItem.Text.ToString().Split(new String[] { "to" }, StringSplitOptions.None);
                        //string[] strExcelSheetName = { s[0], s[1] };
                        string[] strExcelSheetName = {ddlSaleChannelRelationType.SelectedItem.Text.ToString()};
                        ZedService.Utility.ZedServiceUtil.ExportToExecl(dsReferenceCode, FilenameToexport, 1, strExcelSheetName);
                        /*#CC02 Added End*/

                        /*#CC02 Commented Started
                           dsReferenceCode.Tables[0].TableName = "FOSList";
                          dsReferenceCode.Tables[1].TableName = "SalesHierarchyList";                  
                          PageBase.ExportToExecl(dsReferenceCode, FilenameToexport, EnumData.eTemplateCount.ePrimarysales1 + 1);
                         #CC02 Commented End */
                    }
                    else
                    {
                        ucMsg.Visible = true;
                        ucMsg.ShowInfo(Resources.Messages.NoRecord);
                    }



                }
            }
            else
            {
                ucMsg.Visible = true;
                ucMsg.ShowInfo("Please Select MappingType and RelationType");
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void DownloadUnMappedData_Click(object sender, EventArgs e)
    {
        try
        {

            DataSet dsReferenceCode = new DataSet();
            DataSet Ds = new DataSet();
            /*using (SalesChannelData objSalesData = new SalesChannelData())*/
            if (Convert.ToInt32(ddlSaleChannelMappingType.SelectedValue) > 0 && Convert.ToInt32(ddlSaleChannelRelationType.SelectedValue) > 0)
            {
                using (SalesChannelData objSalesData = new SalesChannelData())
                {/* #CC01 Add Start */
                    objSalesData.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
                    objSalesData.SalesChannelID = PageBase.SalesChanelID;
                    objSalesData.EntityMappingTypeId = Convert.ToInt32(ddlSaleChannelMappingType.SelectedValue);
                    objSalesData.EntityMappingRelationId = Convert.ToInt32(ddlSaleChannelRelationType.SelectedValue);

                    objSalesData.ComingFrom = 2;

                    /* #CC01 Add End */
                    dsReferenceCode = objSalesData.GetBulkMappedUnmappedData();

                    //dsReferenceCode.Tables.Remove(dsReferenceCode.Tables[0]);
                    //dsReferenceCode.AcceptChanges();
                    if (dsReferenceCode != null && dsReferenceCode.Tables[0].Rows.Count > 0)
                    {

                        String FilePath = Server.MapPath("../../../");
                        string FilenameToexport = " Unmapped Data";
                        PageBase.RootFilePath = FilePath;
                        /*#CC02 Added Started*/
                        //string[] strExcelSheetName = { "FOSList", "SalesHierarchyList","MappingTypeName"};
                        //string[] s = ddlSaleChannelRelationType.SelectedItem.Text.ToString().Split(new String[] { " to " }, StringSplitOptions.None);
                        //string[] strExcelSheetName = { s[0], s[1] };
                        string[] strExcelSheetName = { ddlSaleChannelRelationType.SelectedItem.Text };
                        
                        ZedService.Utility.ZedServiceUtil.ExportToExecl(dsReferenceCode, FilenameToexport, 1, strExcelSheetName);
                        /*#CC02 Added End*/

                        /*#CC02 Commented Started
                           dsReferenceCode.Tables[0].TableName = "FOSList";
                          dsReferenceCode.Tables[1].TableName = "SalesHierarchyList";                  
                          PageBase.ExportToExecl(dsReferenceCode, FilenameToexport, EnumData.eTemplateCount.ePrimarysales1 + 1);
                         #CC02 Commented End */
                    }
                    else
                    {
                        ucMsg.Visible = true;
                        ucMsg.ShowInfo(Resources.Messages.NoRecord);
                    }



                }
            }
            else
            {
                ucMsg.Visible = true;
                ucMsg.ShowInfo("Please Select MappingType and RelationType");
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
}
