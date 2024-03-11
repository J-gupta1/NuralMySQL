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
* Created By : 
* Module : 
* Description : 
 * Table Name: 
* ====================================================================================================
* Reviewed By :
 ====================================================================================================
Modification On       Modified By          Modification    
---------------      -----------          -------------------------------------------------------------  
 * 12-Apr-11            AmitB            #CC01: Allow serial number between 6 to 18 digits across the application
 * 28-Sep-11            Rakesh Goel      #CC02: Changes for Access Tag and Dynamic location.
 * 29-Sep-11            Rakesh Goel      #CC03: Removed usage of Viewstate to store uploaded file data and other fixing done.
 * 21 June 16, Karam chand sharma, #CC04, show error in result is coming 6 
 * 26 July 2016, Karam Chand Sharma, #CC05, Added stock bin type on interface and bind into to client side grid and pass to save function
 * 27-Jul-2016, Sumit Maurya, #CC06, StockBinTypeID supplied to validate SerialNumber according StockType.
 * 03-Jul-2016, Sumit Maurya, #CC07, instead of "innerText", "value" is used because innerText was not working for Chrome.
 * 13-Jul-2018, Sumit Maurya, #CC08, New columns added to create TVp according to procedure (Done for ComioV5)
 ====================================================================================================
*/

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using BusinessLogics;



public partial class Transactions_Common_StockAdjustmentInterface : BussinessLogic.PageBase
{
    #region VariableDeclaration
    string strUploadedFileName = string.Empty;
    //    string strDownloadPath = Pagebase.strDownloadExcelPath;
    string strDownloadPath = BussinessLogic.PageBase.strExcelVirtualPath;
    List<string> lstDuplicate = new List<string>();   //#CC03 defined as global variable so that it is accessible in Databound event.
    DataSet dsErrorProne = new DataSet();
    UploadFile UploadFile = new UploadFile();

    #endregion

    private string _SalesChannelID = "0";

    public string SalesChannelID
    {
        set
        {
            _SalesChannelID = value;
        }
    }


    private string _SalesChannelCode = string.Empty;

    public string SalesChannelCode
    {
        set
        {
            _SalesChannelCode = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {



        if (!IsPostBack)
        {
            hdnAdjustmentForSalesChannelid.Text = "0";
            ucDatePicker1.TextBoxDate.Text = DateTime.Now.ToShortDateString();
            DaysLoad();
            hdnSerialNoLengthMin.Value = BussinessLogic.PageBase.SerialNoLength_Min.ToString();
            hdnSerialNoLengthMax.Value = BussinessLogic.PageBase.SerialNoLength_Max.ToString();
            hdnBatchNoLengthMin.Value = BussinessLogic.PageBase.BatchNoLength_Min.ToString();
            hdnBatchNoLengthMax.Value = BussinessLogic.PageBase.BatchNoLength_Max.ToString();
            litSerial_MinL.Text = BussinessLogic.PageBase.SerialNoLength_Min.ToString();
            litSerial_MaxL.Text = BussinessLogic.PageBase.SerialNoLength_Max.ToString();
            litBatch_MinL.Text = BussinessLogic.PageBase.BatchNoLength_Min.ToString();
            litBatch_MaxL.Text = BussinessLogic.PageBase.BatchNoLength_Max.ToString();
            FillsalesChannelType();
            ddlType_SelectedIndexChanged(null, null);
            FillStockBinType();/*#CC05 ADDED*/
        }
        // salesChannelID.Value = _SalesChannelID;
        // salesChannelCode.Value = _SalesChannelCode;
        bindDays();





    }
    void bindDays()
    {
        ucDatePicker1.MaxRangeValue = DateTime.Now.Date;
        ucDatePicker1.MinRangeValue = DateTime.Now.Date.AddDays(-Convert.ToInt16(ViewState["backdays"]));
        ucDatePicker1.RangeErrorMessage = "Date shlould be between " + Convert.ToInt16(ViewState["backdays"]).ToString() + " days back and todays only.";
        ucDatePicker1.ValidationGroup = "grpupld";
    }

    private void DaysLoad()
    {
        using (clsStockAdjustment obj = new clsStockAdjustment())
        {
            int b = 0;
            DataSet ds = obj.StockAdjustmentLoad(ref b);
            ViewState["backdays"] = b.ToString();
            FillReason(ds.Tables[0]);
            //            BindAdjustmentFor();
        }
    }

    #region User Defined Function


    private void FillReason(DataTable dt)
    {
        DataTable DtReason;
        DtReason = dt;
        if (DtReason.Rows.Count > 0 && DtReason != null)
        {
            cmbReason.Items.Clear();
            cmbReason.DataSource = DtReason;
            cmbReason.DataTextField = "ReasonName";
            cmbReason.DataValueField = "ReasonID";
            cmbReason.DataBind();
        }
        cmbReason.Items.Insert(0, new ListItem("Select", "0"));
    }
    private void BindAdjustmentFor()
    {
        DataTable DtAdjustment;
        
    }

    private bool ValidatePage()
    {
        if (ucDatePicker1.TextBoxDate.Text == "" || Convert.ToInt32(hdnAdjustmentForSalesChannelid.Text) == 0 || cmbReason.SelectedIndex == 0)
        {
            ucMessage1.ShowWarning(Resources.Messages.MandatoryField);
            return false;
        }
        if (Convert.ToDateTime(ucDatePicker1.TextBoxDate.Text) > DateTime.Now.Date)
        {
            ucMessage1.ShowWarning(Resources.Messages.DateRangeValidation);
            return false;
        }
        return true;
    }
    private void ClearFields()
    {
        clear();
        // DaysLoad();


    }


    #endregion

    #region Button Events


    public DataTable GetBlankTableError()
    {
        DataTable Detail = new DataTable();
        Detail.Columns.Add("SKUCode", typeof(System.Double));
        Detail.Columns.Add("Quantity", typeof(System.Int32));
        Detail.Columns.Add("SerialNo", typeof(System.Double));
        Detail.Columns.Add("BatchNo", typeof(System.Double));
        return Detail;
    }



    private void ExportInExcel(DataSet DsError, string strFileName)
    {
        if (DsError != null && DsError.Tables.Count > 0)
        {
            PageBase.ExportToExeclV2(DsError, strFileName, DsError.Tables.Count);
        }
    }

    private void ExportInExcel(DataTable objTmpTb, string strFileName)
    {
        if (objTmpTb.Rows.Count > 0)
        {
            string FileName = strFileName;
            //string Path = Server.MapPath(strDownloadPath);
            string Path = strDownloadPath;
            DataSet ds = new DataSet();

            objTmpTb.TableName = "tblInvalidRecords" + DateTime.Now.Ticks;
            DataTable dt = objTmpTb.Copy();
            ds.Tables.Add(dt);
            string FilenameToexport = "tblInvalidRecords" + DateTime.Now.Ticks;
            PageBase.ExportToExecl(ds, FilenameToexport, EnumData.eTemplateCount.ePrimarysales1 + 1);
            // LuminousSMS.Utility.LuminousUtil.SaveToExecl(ds, strFileName);
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        ClearFields();



    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (hdnAdjustmentForSalesChannelid.Text == "0")
        {
            ucMessage1.ShowInfo("Error has occured while processing the SalesChannelid");
            return;
        }
        DataTable dtSubmit = new DataTable();
        dtSubmit = PageBase.GetStockUpdateTable();
        DataTable dtUploadedFile = GridClientSide1.GetDataTable;
        

        if (dtUploadedFile.Rows.Count == 0)
        {
            ucMessage1.ShowInfo("There is no row to submit");
            return;
        }

        using (clsStockAdjustment ObjStockAdjust = new clsStockAdjustment())
        {

            //DataTable dtSubmit = DsXML.Tables[0].Copy();
            /* #CC08 Add Start */
            dtUploadedFile.Columns.Add("AdjustmentFor", typeof(string));
            dtUploadedFile.Columns.Add("SerialNo2", typeof(string));
 dtUploadedFile.AcceptChanges();
            dtSubmit.Columns.Add("AdjustmentFor", typeof(string));
            dtSubmit.Columns.Add("SerialNo2", typeof(string));
            dtSubmit.AcceptChanges();
            /* #CC08 Add End */
            foreach (DataRow row in dtUploadedFile.Rows)
            {
                DataRow NewRow = dtSubmit.NewRow();
                NewRow["SKUCode"] = row["skucode"];
                NewRow["Quantity"] = row["qty"];
                /*NewRow["StockBinTypeCode_From"] = "COFGD";
                NewRow["StockBinTypeCode_To"] = "COFGD";*/
                NewRow["StockBinTypeCode_From"] = row["stocktype"];
                NewRow["StockBinTypeCode_To"] = row["stocktype"];
                NewRow["SerialNo"] = row["SerialNo"].ToString().Trim();
                NewRow["BatchNo"] = row["BatchNo"].ToString().Trim();
                NewRow["GRNDate_From"] = null;
                NewRow["GRNDate_To"] = Convert.ToDateTime(ucDatePicker1.TextBoxDate.Text);
                /* #CC08 Add Start */
                if (txtAdjustmentFor.Text.Trim()!="")
                NewRow["AdjustmentFor"] = txtAdjustmentFor.Text.Split('-')[1].ToString();
                NewRow["SerialNo2"] = null;
                /* #CC08 Add End */

                if (Convert.ToInt16(row["qty"]) < 0)
                {
                    NewRow["StockBinTypeCode_To"] = null;
                }
                else
                {
                    NewRow["StockBinTypeCode_From"] = null;

                }
                NewRow["quantity"] = Math.Abs(Convert.ToInt32(row["qty"]));
                dtSubmit.Rows.Add(NewRow);
            }

            dtSubmit.AcceptChanges();


            byte SCTypeID = 0;

            //string[] s = ddlAdjustmentFor.SelectedValue.Split('/');

            // SCTypeID = Convert.ToByte(s[1]);
            SCTypeID = Convert.ToByte(ddlType.SelectedValue);

            ObjStockAdjust.XML_PartDetails = dtSubmit;
            ObjStockAdjust.SalesChannelTypeID = SCTypeID;

            ObjStockAdjust.StrstockAdjustmentDetail = "<NewDataSet><Table></Table></NewDataSet>";
            ObjStockAdjust.IntStockAdjustmentID = 0;
            ObjStockAdjust.IntStockAdjustmentForID = Convert.ToInt32(hdnAdjustmentForSalesChannelid.Text);//Convert.ToInt32(s[0]);
            ObjStockAdjust.DtStockAdjustmentDate = DateTime.Parse(ucDatePicker1.Date);
            ObjStockAdjust.StrRemarks = txtRemarks.TextBoxText.Trim();
            ObjStockAdjust.StrError = "";
            ObjStockAdjust.IntCreatedBy = PageBase.UserId;
            ObjStockAdjust.ReasonID = Convert.ToInt16(cmbReason.SelectedValue);  //This would be used in the opening stock case
            ObjStockAdjust.Save();
            if (ObjStockAdjust.StrstockAdjustmentDetail != null && ObjStockAdjust.StrstockAdjustmentDetail != string.Empty)
            {
                ucMessage1.XmlErrorSource = ObjStockAdjust.StrstockAdjustmentDetail;
                //  dvSave.Attributes.Add("Style", "display:none");
                return;
            }
            if (ObjStockAdjust.StrError != null && ObjStockAdjust.StrError != "")
            {
                ucMessage1.ShowError(ObjStockAdjust.StrError);
                //  dvSave.Attributes.Add("Style", "display:none");
                return;
            }
            if (ObjStockAdjust.ErrorValue == 1)
            {
                // ucMessage1.ShowError(Resources.WarningMessages.BackDaysIssue);
                // dvSave.Attributes.Add("Style", "display:none");
                return;
            }
            /*#CC04 START ADDED*/
            if (ObjStockAdjust.ErrorValue == 6)
            {
                ucMessage1.ShowError("Stock adjustment not allowed for : " + txtAdjustmentFor.Text);
                return;
            }/*#CC04 END ADDED*/
            else if (ObjStockAdjust.ErrorValue == 2)
            {
                ucMessage1.ShowError("Stock adjustment date can not be less than from opening stock date.");
                return;
            }

            ucMessage1.ShowSuccess(Resources.Messages.InsertSuccessfull);
            GridClientSide1.IsBlankDataTable = true;
            //            ddlAdjustmentFor.SelectedIndex = 0;
            cmbReason.SelectedIndex = 0;
            txtRemarks.TextBoxText = "";
            hdnAdjustmentForSalesChannelid.Text = "0";
            ddlType.SelectedValue = "0";
            txtAdjustmentFor.Text = string.Empty;

        };

        //}
        //catch (Exception ex)
        //{
        //    ucMessage1.ShowAppError(ex);
        //    cmbReason.Enabled = true;
        //}
    }
    void clear()
    {

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "RefreshPanel", "VisibleControls('false','');", true);
        txtQuantity.Text = "";
        //        ddlAdjustmentFor.SelectedIndex = 0;
        cmbReason.SelectedIndex = 0;
        txtRemarks.TextBoxText = "";
        ucMessage1.Visible = false;
        // ucDatePicker1.TextBoxDate.Text = DateTime.Now.ToShortDateString();

        ucDatePicker1.IsEnabled = true;
        GridClientSide1.IsBlankDataTable = true;
        hdnAdjustmentForSalesChannelid.Text = "0";
        ddlType.SelectedValue = "0";
        txtAdjustmentFor.Text = string.Empty;
        ddlStockBinType.SelectedIndex = 0;

    }

    protected void DownloadTemplatePartCode_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(hdnAdjustmentForSalesChannelid.Text) != 0)
            {
                using (clsStockAdjustment objStockAdjust = new clsStockAdjustment())
                {
                    objStockAdjust.EntityId = Convert.ToInt32(hdnAdjustmentForSalesChannelid.Text);
                    DataSet ds = objStockAdjust.GetPartCodeTemplate();
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        PageBase.ExportToExecl(ds, "PartCodeTemplate", EnumData.eTemplateCount.ePrimarysales1 + 1);
                        // LuminousSMS.Utility.LuminousUtil.ExportToExecl(ds, "PartCodeTemplate");
                    }
                    else
                    {
                        ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                    }
                }
            }
            else
            {
                ucMessage1.ShowError("Select plant first before download parts");
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);

        }
    }
    protected void DownloadStockBinType_Click(object sender, EventArgs e)
    {
        try
        {
            using (clsStockAdjustment objStockAdjust = new clsStockAdjustment())
            {
                DataSet dsStockBinType = objStockAdjust.GetStockBinType();
                if (dsStockBinType != null && dsStockBinType.Tables[0].Rows.Count > 0)
                {
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "StockBinTypeCodeTemplate";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dsStockBinType, FilenameToexport, EnumData.eTemplateCount.ePrice);

                    //LuminousSMS.Utility.LuminousUtil.ExportToExecl(dsStockBinType, "StockBinTypeCodeTemplate");
                }
                else
                {
                    ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }

    #endregion

    protected void ddlAdjustmentFor_SelectedIndexChanged(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        GridClientSide1.IsBlankDataTable = true;
        txtPartCode.Text = "";
        txtQuantity.Text = "";
        txtBatchNo.Text = "";
        txtSerialNos.Text = "";

    }
    protected void rdModelList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdModelList.SelectedValue == "0")
            Response.Redirect("~/Transactions/Common/StockAdjustmentUpload.aspx");
    }
    void FillsalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {

            ddlType.Items.Clear();
            string[] str = { "SalesChannelTypeID", "SalesChannelTypeName" };
            PageBase.DropdownBinding(ref ddlType, ObjSalesChannel.GetSalesChannelType(), str);
            if (PageBase.SalesChanelID != 0 )
            {
                ddlType.SelectedValue = PageBase.SalesChanelTypeID.ToString();
                if (PageBase.SalesChanelTypeID != 5 && PageBase.IsRetailerStockTrack == 1)
                    ddlType.Items.Add(new ListItem("Retailer", "101"));
            }
            else if (PageBase.SalesChanelID == 0 & PageBase.BaseEntityTypeID == 3)
            {
                ddlType.Items.Clear();
                ddlType.Items.Insert(0, new ListItem("Retailer", "101"));
                ddlType.Enabled = false;
            }
            else if (PageBase.IsRetailerStockTrack == 1)
            {
                ddlType.Items.Add(new ListItem("Retailer", "101"));
                ddlType.Enabled = true;
            }
        };
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        AutoCompleteExtender1.ContextKey = ddlType.SelectedValue;
        hdnAdjustmentForSalesChannelid.Text = "0";
    }
    /*#CC05 START ADDED*/
    
    void FillStockBinType()
    {
        String[] StrCol;
        DataSet dsStockBinType = new DataSet();
        using (SalesmanData ObjSalesman = new SalesmanData())
        {
            ObjSalesman.Type = EnumData.eSearchConditions.Active;
            ObjSalesman.SalesChannelID = PageBase.SalesChanelID;
            ObjSalesman.MapwithRetailer = 1;
            dsStockBinType = ObjSalesman.GetSalesmanAndStockBinTypeInfo();
            StrCol = new String[] { "StockBinTypeMasterID", "StockBinTypeDescWithCode" };
            PageBase.DropdownBinding(ref ddlStockBinType, dsStockBinType.Tables[1], StrCol);

        };
    }
    /*#CC05 END ADDED*/
}


