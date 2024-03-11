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
* Created On: 28-Feb-2018
* ====================================================================================================
* Reviewed By :
* DD-MMM-YYYY, Name, #CCXX, Description
 ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 * ====================================================================================================
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


public partial class Masters_HO_Admin_ManagePriceDrop : PageBase
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
        try
        {
            ucMsg.ShowControl = false;
            if (!IsPostBack)
            {
              
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
           // if (Convert.ToInt32(ddlSaleChannelMappingType.SelectedValue) > 0 && Convert.ToInt32(ddlSaleChannelRelationType.SelectedValue) > 0)
            if(1==1)
            {
                using (SalesChannelData objSalesData = new SalesChannelData())
                {/* #CC01 Add Start */
                    objSalesData.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
                    objSalesData.SalesChannelID = PageBase.SalesChanelID;
                   // objSalesData.EntityMappingTypeId = Convert.ToInt32(ddlSaleChannelMappingType.SelectedValue);
                  //  objSalesData.EntityMappingRelationId = Convert.ToInt32(ddlSaleChannelRelationType.SelectedValue);



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
                        //string[] s = ddlSaleChannelRelationType.SelectedItem.Text.ToString().Split(new String[] { "to" }, StringSplitOptions.None);
                      //  string[] strExcelSheetName = { s[0], s[1] };

                        //ZedService.Utility.ZedServiceUtil.ExportToExecl(dsReferenceCode, FilenameToexport, 2, strExcelSheetName);
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
            Response.Redirect("ManagePriceDrop.aspx", false);
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
        DataTable dtError = new DataTable();
        string SerialMinLength = "";
        string SerialMaxLength = "";

        try
        {
            //ClearForm();
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

                isSuccess = UploadFile.uploadValidExcelRetailer(ref objDS, "PriceDrop");
                using (PriceDrop objImeiNumber = new PriceDrop())
                {
                    DataSet dsImeilength = new DataSet();
                    dsImeilength = objImeiNumber.GetSerialNumberLength();
                    if (dsImeilength.Tables[0].Rows.Count > 0)
                    {
                        SerialMinLength = dsImeilength.Tables[0].Rows[0]["MinLength"].ToString();
                        SerialMaxLength = dsImeilength.Tables[0].Rows[0]["MaxLength"].ToString();
                    }
                    else
                    {
                        ucMsg.ShowError("Serial Length Procedure not Found");
                        return;
                    }
                }
                if (objDS.Tables[0].Columns.Contains("Error") == false)
                {
                    objDS.Tables[0].Columns.Add("Error", System.Type.GetType("System.String"));
                }
                foreach (DataRow Drow in objDS.Tables[0].Rows)
                {
                    if (Drow["IMEI"].ToString().Trim() != string.Empty)
                    {
                        if (Convert.ToInt32(Drow["IMEI"].ToString().Trim().Length) > Convert.ToInt32(SerialMaxLength) || Convert.ToInt32(Drow["IMEI"].ToString().Trim().Length) < Convert.ToInt32(SerialMinLength))
                        {
                            Drow["Error"] = "Invalid IMEI.";
                            isSuccess = 2;
                        }
                    }
                }
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
                         dtError = dvError.ToTable();
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

                PriceDrop obj = new PriceDrop();
                obj.SessionID = Convert.ToString(DtBulkUpload.Rows[0]["SessionID"]);
                obj.UserID = Convert.ToInt32(PageBase.UserId);
                DataSet dsResult = new DataSet();
                dsResult = obj.SaveBulkPriceDrop();
                int result = obj.OutParam;
                if (result == 0)
                {
                    hlnkInvalid.Text = "";
                    hlnkInvalid.Visible = false;
                    ucMsg.ShowSuccess("Data uploaded successfully.");
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
                    ucMsg.ShowError(obj.OutError);
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
           // dtUpload.Columns.Add(AddColumn(Convert.ToString(1), "Status2", typeof(int)));
            dtUpload.Columns.Add(AddColumn(guid, "SessionID", typeof(System.String)));
            dtUpload.Columns.Add(AddColumn(Convert.ToString(PageBase.UserId), "CreatedBy", typeof(int)));

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
                bulkCopy.DestinationTableName = "PriceDropDump";

                bulkCopy.ColumnMappings.Add("IMEI","SerialNumber1");
                bulkCopy.ColumnMappings.Add("CreatedBy", "CreatedBy");
                bulkCopy.ColumnMappings.Add("SessionId", "SessionId");
                bulkCopy.ColumnMappings.Add("SalesChannelCode", "SalesChannelCode");
                bulkCopy.ColumnMappings.Add("ModelCode", "ModelCode");
                bulkCopy.ColumnMappings.Add("SKUCode", "SKUCode");
                bulkCopy.ColumnMappings.Add("Qty", "Qty");
                bulkCopy.ColumnMappings.Add("Status", "Status");               
                bulkCopy.ColumnMappings.Add("Remark", "Remarks");    
                bulkCopy.WriteToServer(dtUpload);
                return true;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    
    protected void DwnldReferenceCode_Click(object sender, EventArgs e)
    {
        try
        {
            using(PriceDrop objPriceDrop = new PriceDrop())
            {
                objPriceDrop.UserID = PageBase.UserId;
                DataSet ds = objPriceDrop.GetPriceDropRefData();
                if(objPriceDrop.TotalRecords>0)
                {
                    ds.Tables[0].TableName = "SalesChannelData";
                    if(ds.Tables.Count>1)
                    {
                        ds.Tables[1].TableName = "RetailerData";
                    }

                    if (ds.Tables.Count > 2)
                    {
                        ds.Tables[2].TableName = "ModelAndSKUData";
                    }
                    string[] strExcelSheetName = { "SalesChannelData", "RetailerData", "ModelAndSKUData" };
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(ds, "PriceDropReferanceData", ds.Tables.Count, strExcelSheetName);
                    //PageBase.ExportToExecl(ds, "PriceDropReferanceData");

                }
            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }
}
