using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;
using System.Data.SqlClient;
/*
 * 29 Jan 2015, Karam Chand Sharma, #CC01, stock bin type dropdown enables set false.And Correct submit button with BCD process
 * 18 July 2016 , Karam Chand Sharma, #CC02, hide stock bin type dropdown
 * * 09-AUg-2018,Vijay Kumar Prajapati,#CC06,Add NumberofBackdayssalesreturnallow.(Done For ComioV5)
 */
public partial class Transactions_SalesChannelSBReturn_Interface_ManageInterfaceIntermediarySalesReturnSB : PageBase
{
    DateTime dt = new DateTime();
    string strSalesChannelName, strSalesChannelCode;
    string[] strSplitSerialNumber;
    protected void Page_Load(object sender, EventArgs e)
    {

        strSalesChannelCode = hdnCode.Value;
        strSalesChannelName = hdnName.Value;

        DateTime dt = System.DateTime.Now.Date;

        StockBinTypeTobeAskedOrNot();
        //Added By Mamta Singh for checking back date before opening date
        //if (Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening) >= Convert.ToDateTime(dt.AddDays(PageBase.NumberofBackDaysAllowed)))
        //{

        //    ucDatePicker.MinRangeValue = Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-PageBase.BackDaysAllowedBeforeOpening);
        //    lblValidationDays.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", (Convert.ToDateTime(dt.ToShortDateString()) - Convert.ToDateTime(Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening).ToShortDateString())).TotalDays.ToString());
        //}
        //else
        //{
        //    ucDatePicker.MinRangeValue = dt.AddDays(PageBase.NumberofBackDaysAllowed);
        //    lblValidationDays.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
        //}
        if (Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening) >= Convert.ToDateTime(dt.AddDays(PageBase.BackDaysAllowedForSaleReturn)))
        {

            ucDatePicker.MinRangeValue = Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-PageBase.BackDaysAllowedBeforeOpening);
            lblValidationDays.Text = Resources.Messages.ValidationSalesReturnDays.ToString().Replace("Number", (Convert.ToDateTime(dt.ToShortDateString()) - Convert.ToDateTime(Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedForSaleReturn).ToShortDateString())).TotalDays.ToString());
        }
        else
        {
            ucDatePicker.MinRangeValue = dt.AddDays(PageBase.BackDaysAllowedForSaleReturn);
            lblValidationDays.Text = Resources.Messages.ValidationSalesReturnDays.ToString().Replace("Number", PageBase.BackDaysAllowedForSaleReturn.ToString().Replace("-", ""));
        }
        ucDatePicker.MaxRangeValue = dt;
        ucDatePicker.RangeErrorMessage = "Invalid Date Range";

        btnsearch.Attributes.Add("onclick", "return popup();");
        try
        {
            dt = System.DateTime.Now.Date;
            //            ucDatePicker.MaxRangeValue = dt;
            //          ucDatePicker.RangeErrorMessage = "Invalid Date";
            ucMsg.ShowControl = false;
            if (!IsPostBack)
            {
                FillsalesChannel();
                FillStockBinType();// Due to shortage of time we can make one procedure for this instead of two time hitting of the database
                lblChange.Text = Resources.Messages.SalesEntity;
                lblChangeTo.Text = Resources.Messages.SalesEntity;
                BindSalesChannel();
                if (PageBase.SalesChanelID == 0)
                {
                    ddlSalesChannel.Enabled = true;
                    ddlSalesChannel.SelectedValue = "0";
                }
                else
                {
                    ddlSalesChannel.SelectedValue = PageBase.SalesChanelID.ToString();
                    ddlSalesChannel.Enabled = false;
                }
                ucDatePicker.Date = dt.ToString();

            }
            PartLookupClientSide1.SalesChannelID = PageBase.SalesChanelID == 0 ? "0" : PageBase.SalesChanelID.ToString();
            PartLookupClientSide1.SalesChannelCode = PageBase.SalesChanelID == 0 ? ddlSalesChannel.SelectedValue : PageBase.SalesChanelCode.ToString();
            PartLookupClientSide1.SalesTypeID = 2;
            Session["ReturnSalesType"] = 2;  //Here this is hardcoded will be removed as i will get the option for  the 
            //how to send the Pararmetervalue in the autocomplete service

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void rdoSelectMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (rdoSelectMode.SelectedValue == "1")
        //{
        //    Response.Redirect("../Upload/ManageUploadPrimarySales1-SB.aspx");
        //}
    }

    protected void Page_PreRender(object s, EventArgs args)
    {

        BtnSubmit_Click(btnSave1, null);
    }


    DataTable getOpeningStockTable(DataTable dtSource)
    {

        DataTable Detail = new DataTable();
        using (CommonData ObjCommom = new CommonData())
        {
            Detail = ObjCommom.GettvpTableOpeningStockSB();
        }


        foreach (DataRow dr in dtSource.Rows)
        {
            DataRow drow = Detail.NewRow();
            drow["SKUCode"] = dr["SKUCode"].ToString().Trim();
            drow["Serial#1"] = dr["serialno"].ToString().Trim();
            drow["BatchNo"] = dr["BatchNo"].ToString().Trim();
            drow["Quantity"] = Convert.ToInt32(dr["Quantity"]);
            Detail.Rows.Add(drow);
        }
        Detail.AcceptChanges();

        return Detail;

    }


    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string guid = Guid.NewGuid().ToString();
            int intResult = 0;
            DataTable Dt = new DataTable();
            if (PartLookupClientSide1.SubmittingTable.Rows.Count > 0)
            {
                Dt = PartLookupClientSide1.SubmittingTable;
            }
            else
            {
                return;
            }
            //DataTable dtSource = getOpeningStockTable(Dt);
            DataTable Tvp = new DataTable();
            using (CommonData ObjCommom = new CommonData())
            {
                Tvp = ObjCommom.GettvpTableReturnSalesSB();
            }
            foreach (DataRow dr in Dt.Rows)
            {
                DataRow drow = Tvp.NewRow();
                drow["ReturnFromSalesChannelCode"] = ddlTDCode.SelectedValue;
                drow["ReturnToSalesChannelCode"] = PageBase.SalesChanelCode;
                drow["InvoiceNumber"] = dr["InvoiceNumber"].ToString();
                drow["InvoiceDate"] = dr["InvoiceDate"];
                drow["SKUCode"] = dr["SKUCode"].ToString();
                drow["Quantity"] = Convert.ToInt32(dr["Quantity"]);
                drow["Serial#1"] = dr["Serialno"].ToString();
                drow["BatchNo"] = dr["BatchNo"].ToString();
                Tvp.Rows.Add(drow);
            }
            Tvp.AcceptChanges();

            //using (SalesData objP1 = new SalesData())
            //{
            //    objP1.EntryType = EnumData.eEntryType.eUpload;
            //    objP1.UserID = PageBase.UserId;
            //    intResult = objP1.InsertPrimarySales1SB(Tvp);

            //    if (objP1.ErrorDetailXML != null && objP1.ErrorDetailXML != string.Empty)
            //    {
            //        ucMsg.XmlErrorSource = objP1.ErrorDetailXML;
            //        return;
            //    }
            //    if (objP1.Error != null && objP1.Error != "")
            //    {
            //        ucMsg.ShowError(objP1.Error);
            //        return;
            //    }
            //    if (intResult == 2)
            //    {
            //        ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            //        return;
            //    }
            //    PartLookupClientSide1.SubmittingTable = new DataTable();
            //    PartLookupClientSide1.IsBlankDataTable = true;
            //    ClearForm();
            //    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
            //}
            /*#CC01 START ADDED*/
            Tvp.Columns.Add(AddColumn(guid, "TransUploadSession", typeof(System.String)));
            Tvp.Columns.Add(AddColumn("5", "TransType", typeof(System.Int32)));
            Tvp.AcceptChanges();

            if (Tvp.Rows.Count > 0)
            {
                if (!BulkCopyUpload(Tvp))
                {
                    ucMsg.ShowError("Error Occured While transferring the data to the server");
                    return;
                }

            }
            ((TextBox)PartLookupClientSide1.FindControl("txtInvoiceNo")).Text = "";
            ((TextBox)PartLookupClientSide1.FindControl("ucInvoiceDate").FindControl("txtDate")).Text = "";
            ((TextBox)PartLookupClientSide1.FindControl("txtPartCode")).Text = "";
            using (SalesData objSecondary = new SalesData())
            {
                objSecondary.EntryType = EnumData.eEntryType.eUpload;
                objSecondary.UserID = PageBase.UserId;
                objSecondary.ReturnDate = Convert.ToDateTime(ucDatePicker.Date);
                objSecondary.StockBinType = Convert.ToInt16(ddlStockBinType.SelectedValue);
                objSecondary.TransUploadSession = guid;
                objSecondary.ComingFrom = 2;

                intResult = objSecondary.InsertIntermediarySalesReturnSBBCP();

                if (objSecondary.ErrorDetailXML != null && objSecondary.ErrorDetailXML != string.Empty)
                {
                    ucMsg.XmlErrorSource = objSecondary.ErrorDetailXML;
                    return;
                }
                if (objSecondary.Error != null && objSecondary.Error != "")
                {
                    ucMsg.ShowError(objSecondary.Error);
                    return;
                }
                if (intResult == 2)
                {
                    ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    return;
                }
                ClearForm();
                ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                pnlGrid.Visible = false;
            }
            /*#CC01 START END*/
            /*#CC01 COMMENTED using (SalesData objIntermediary = new SalesData())
            {
                objIntermediary.EntryType = EnumData.eEntryType.eUpload;
                objIntermediary.UserID = PageBase.UserId;
                objIntermediary.ReturnDate = Convert.ToDateTime(ucDatePicker.Date);
                objIntermediary.StockBinType = Convert.ToInt16(ddlStockBinType.SelectedValue);
                intResult = objIntermediary.InsertIntermediarySalesReturnSB(Tvp);
                if (objIntermediary.ErrorDetailXML != null && objIntermediary.ErrorDetailXML != string.Empty)
                {
                    ucMsg.XmlErrorSource = objIntermediary.ErrorDetailXML;
                    return;
                }
                if (objIntermediary.Error != null && objIntermediary.Error != "")
                {
                    ucMsg.ShowError(objIntermediary.Error);
                    return;
                }
                if (intResult == 2)
                {
                    ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    return;
                }
               // PartLookupClientSide1.SubmittingTable = new DataTable();
               // PartLookupClientSide1.IsBlankDataTable = true;
                ClearForm();
                ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                pnlGrid.Visible = false;

            }
            */

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    bool pageValidateSave()
    {

        if (ddlSalesChannel.SelectedIndex == 0 || ServerValidation.IsDate(ucDatePicker.Date, true) != 0)
        {
            ucMsg.ShowWarning(Resources.Messages.MandatoryField);
            return false;
        }

        if (Convert.ToDateTime(ucDatePicker.Date) > DateTime.Now.Date)
        {
            ucMsg.ShowWarning(Resources.Messages.DateRangeValidation);
            return false;
        }
        if (hdnDirectSalesOfSerialAllowed.Value == "1")//means allowed
        {
            if (txtDirectSerialNumber.Text.Trim() == string.Empty)
            {
                ucMsg.ShowWarning("Please fill required Serial Numbers");
                return false;
            }
            string Serialnumber = txtDirectSerialNumber.Text.Trim();
            Serialnumber = Serialnumber.Replace("\r\n", ",");
            strSplitSerialNumber = Serialnumber.Split(',');

            DataTable dtFullError = new DataTable();
            DataSet ds = new DataSet();
            var DuplicateSerialNumber = strSplitSerialNumber.GroupBy(x => x).Where(g => g.Count() > 1 & g.Key.Length > 1).Select(x => new { Error = x.Key, ErrorData = "Duplicate Serial Number" });
            if (DuplicateSerialNumber != null)
            {
                if (DuplicateSerialNumber.Count() > 0)
                {
                    dtFullError.Merge(PageBase.LINQToDataTable(DuplicateSerialNumber));
                    ds.Merge(dtFullError);
                }
            }





            var SerialNumberNotAccordingToConfig = from r in strSplitSerialNumber.AsEnumerable()
                                                   where (r.ToString().Length > SerialNoLength_Max | r.ToString().Length < SerialNoLength_Min) & (r.ToString().Length > 1)
                                                   select new
                                                   {
                                                       Error = r.ToString(),
                                                       ErrorData = "SerialNumber is not according to Min length:" + SerialNoLength_Min + " or Max length:" + SerialNoLength_Max

                                                   };

            if (SerialNumberNotAccordingToConfig != null)
            {
                if (SerialNumberNotAccordingToConfig.Count() > 0)
                {
                    dtFullError.Merge(PageBase.LINQToDataTable(SerialNumberNotAccordingToConfig));
                    ds.Merge(dtFullError);
                }
            }


            if (dtFullError.Rows.Count > 0)
            {
                ucMsg.XmlErrorSource = ds.GetXml();
                return false;
            }
        }

        return true;
    }
    bool pageValidateGo()
    {

        //if (txtInvoiceNo.Text.Trim() == string.Empty)
        //{
        //    ucMsg.ShowWarning(Resources.Messages.EnterInvoiceNo);
        //    return false;
        //}
        return true;
    }


    void ClearForm()
    {
        // txtSearchedName.Text = "";
        hdnCode.Value = "";
        hdnName.Value = "";
        ucDatePicker.Date = DateTime.Now.Date.ToString();
        BindSalesChannel();

        if (PageBase.SalesChanelID != 0)
        {

            ddlSalesChannel.SelectedValue = PageBase.SalesChanelID.ToString();
            ddlSalesChannel.Enabled = false;

        }
        else
        {
            ddlSalesChannel.ClearSelection();
            ddlSalesChannel.SelectedIndex = 0;
            ddlSalesChannel.Enabled = true;

        }

        ucDatePicker.imgCal.Enabled = true;

        ucDatePicker.TextBoxDate.Enabled = true;
        EnableDisableDirectSales();
        txtDirectSerialNumber.Text = string.Empty;

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        ClearForm();
        PartLookupClientSide1.SubmittingTable = new DataTable();
        PartLookupClientSide1.IsBlankDataTable = true;
        ddlStockBinType.Enabled = true;/*#CC01 Added*/
        pnlGrid.Visible = false;
    }
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {

        }
    }
    void BindSalesChannel()
    {
        try
        {
            ddlSalesChannel.Items.Clear();
            using (SalesChannelData ObjSalesChannel = new SalesChannelData())
            {
                ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
                ObjSalesChannel.ActiveStatus = 1;
                string[] str = { "SalesChannelid", "SalesChannelName" };
                PageBase.DropdownBinding(ref ddlSalesChannel, ObjSalesChannel.GetSalesChannelList(), str);
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }
    protected void ddlWarehouse_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlWarehouse.SelectedIndex != 0)
        //{
        //}

    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            PartLookupClientSide1.SubmittingTable = new DataTable();
            PartLookupClientSide1.IsBlankDataTable = true;
            PartLookupClientSide1.ReturnFromSalesChannelID = Convert.ToString(ddlTD.SelectedValue);
            Session["ReturnFromSalesChannelID"] = ddlTD.SelectedValue;
            ddlTDCode.SelectedIndex = ddlTD.SelectedIndex;
            pnlGrid.Visible = true;
            ddlTD.Enabled = false;
            ucDatePicker.imgCal.Enabled = false;
            ucDatePicker.TextBoxDate.Enabled = false;
            EnableDisableDirectSales();
            ddlStockBinType.Enabled = false;/*#CC01 ADDED */
        }
        catch (Exception ex)
        {

        }

    }
    void FillsalesChannel()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            //ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(EnumData.eSalesChannelType.TD);
            ObjSalesChannel.SalesChannelID = PageBase.SalesChanelID;
            ObjSalesChannel.SearchType = EnumData.eSearchConditions.Active;
            ObjSalesChannel.BlnShowDetail = false;
            ObjSalesChannel.StatusValue = 1;
            string[] str = { "SalesChannelID", "DisplayName" };
            //PageBase.DropdownBinding(ref ddlTD, ObjSalesChannel.GetSalesChannelInfo(), str);
            PageBase.DropdownBinding(ref ddlTD, ObjSalesChannel.GetChildSalesChannel(), str);

            string[] str1 = { "SalesChannelCode", "DisplayName" };
            PageBase.DropdownBinding(ref ddlTDCode, ObjSalesChannel.GetChildSalesChannel(), str1);

        };

    }
    protected void rdModelList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdModelList.SelectedValue == "0")
            /*#CC02 START COMMENTED Response.Redirect("~/Transactions/SalesChannelSBReturn/Upload/ManageIntermediarySalesReturnSB.aspx"); #CC02 END COMMENTED*/
            Response.Redirect("~/Transactions/SalesChannelSBReturn/Upload/ManageIntermediarySalesReturnSB-BCP.aspx");/*#CC02 ADDED*/
    }
    void StockBinTypeTobeAskedOrNot()
    {
        if (Session["SalesReturnBinAsking"] != null)
        {
            if (Convert.ToInt16(Session["SalesReturnBinAsking"]) == 0)
            {
                dvStockBinType.Visible = false;
                ddlStockBinType.Items.Clear();
                ddlStockBinType.Items.Insert(0, new ListItem("Select", "0"));
                ddlStockBinType.ValidationGroup = "";
                reqStockBinType.Enabled = false;

            }
            else
            {
                /*#CC02 START COMMENTED dvStockBinType.Visible = true;
                ddlStockBinType.ValidationGroup = "EntryValidation";
                reqStockBinType.Enabled = true; #CC02 END COMMENTED */
                dvStockBinType.Visible = false;/*#CC02 ADDED*/
                ddlStockBinType.ValidationGroup = "";/*#CC02 ADDED*/
                reqStockBinType.Enabled = false;/*#CC02 ADDED*/
            }


        }
    }
    void FillStockBinType()
    {
        String[] StrCol;
        DataSet dsStockBinType = new DataSet();
        using (SalesmanData ObjSalesman = new SalesmanData())
        {
            ObjSalesman.Type = EnumData.eSearchConditions.Active;
            ObjSalesman.SalesChannelID = PageBase.SalesChanelID;
            ObjSalesman.MapwithRetailer = 0;
            dsStockBinType = ObjSalesman.GetSalesmanAndStockBinTypeInfo();
            StrCol = new String[] { "StockBinTypeMasterID", "StockBinTypeDesc" };
            PageBase.DropdownBinding(ref ddlStockBinType, dsStockBinType.Tables[1], StrCol);

        };
    }
    void EnableDisableDirectSales()
    {
        //1-Allowed 0-is not allowed
        /*#CC01 Commented 
        if (Session["DirctSralSaleIntefce"] != null)
            hdnDirectSalesOfSerialAllowed.Value = Convert.ToString(Session["DirctSralSaleIntefce"]);*/
        /*#CC01 START ADDED*/
        if (Session["DirectSaleReturn"] != null)
            hdnDirectSalesOfSerialAllowed.Value = Convert.ToString(Session["DirectSaleReturn"]);
        /*#CC01 START END*/
        if (hdnDirectSalesOfSerialAllowed.Value == "0")
        {
            tblDirectSerialPanel.Attributes.Add("style", "display:none");
            tblGrid.Attributes.Add("style", "display:block");
            if (PartLookupClientSide1.FindControl("ReqInvoice") != null)
                ((RequiredFieldValidator)PartLookupClientSide1.FindControl("ReqInvoice")).ValidationGroup = "EntryValidation";
            if (((RequiredFieldValidator)PartLookupClientSide1.FindControl("ucInvoiceDate").FindControl("RequiredFieldValidator1")) != null)
                ((RequiredFieldValidator)PartLookupClientSide1.FindControl("ucInvoiceDate").FindControl("RequiredFieldValidator1")).ValidationGroup = "EntryValidation";

        }
        else
        {
            tblDirectSerialPanel.Attributes.Add("style", "display:block");
            tblGrid.Attributes.Add("style", "display:none");
            if (PartLookupClientSide1.FindControl("ReqInvoice") != null)
                ((RequiredFieldValidator)PartLookupClientSide1.FindControl("ReqInvoice")).ValidationGroup = "Dummy";
            if (((RequiredFieldValidator)PartLookupClientSide1.FindControl("ucInvoiceDate").FindControl("RequiredFieldValidator1")) != null)
                ((RequiredFieldValidator)PartLookupClientSide1.FindControl("ucInvoiceDate").FindControl("RequiredFieldValidator1")).ValidationGroup = "Dummy";

        }


    }

    protected void btnSubmitDirectSerialSale_Click(object sender, EventArgs e)
    {

        try
        {
            int intResult;
            string guid = Guid.NewGuid().ToString();

            if (pageValidateSave())
            {
                DataTable DsDetail = new DataTable();
                using (CommonData ObjCommom = new CommonData())
                {
                    DsDetail = ObjCommom.GettvpTableReturnSalesSB();
                }
                foreach (string strDirectSerialNumber in strSplitSerialNumber)
                {
                    if (strDirectSerialNumber != string.Empty)
                    {
                        DataRow drow = DsDetail.NewRow();
                        drow["ReturnFromSalesChannelCode"] = ddlTDCode.SelectedValue;
                        drow["ReturnToSalesChannelCode"] = PageBase.SalesChanelCode;
                        drow["InvoiceNumber"] = null;
                        drow["InvoiceDate"] = ucDatePicker.Date; ;
                        drow["SKUCode"] = null;
                        drow["Quantity"] = 1;
                        drow["Serial#1"] = strDirectSerialNumber.Trim();
                        drow["BatchNo"] = null;
                        DsDetail.Rows.Add(drow);
                    }
                }
                DsDetail.AcceptChanges();
                DsDetail.Columns.Add(AddColumn(guid, "TransUploadSession", typeof(System.String)));
                DsDetail.Columns.Add(AddColumn("3", "TransType", typeof(System.Int32)));
                DsDetail.AcceptChanges();
                DsDetail.AcceptChanges();
                if (DsDetail.Rows.Count > 0)
                {
                    if (!BulkCopyUpload(DsDetail))
                    {
                        ucMsg.ShowError("Error Occured While transferring the data to the server");
                        return;
                    }

                }

                using (SalesData objIntermediary = new SalesData())
                {
                    objIntermediary.EntryType = EnumData.eEntryType.eUpload;
                    objIntermediary.UserID = PageBase.UserId;
                    objIntermediary.ReturnDate = Convert.ToDateTime(ucDatePicker.Date);
                    objIntermediary.StockBinType = Convert.ToInt16(ddlStockBinType.SelectedValue);
                    objIntermediary.TransUploadSession = guid;
                    objIntermediary.TemplateType = "2";//Serial Only
                    objIntermediary.ComingFrom = 2;//1-from upload 2 from interface
                    intResult = objIntermediary.InsertIntermediarySalesReturnSBBCP();

                    if (objIntermediary.ErrorDetailXML != null && objIntermediary.ErrorDetailXML != string.Empty)
                    {
                        ucMsg.XmlErrorSource = objIntermediary.ErrorDetailXML;
                        EnableDisableDirectSales();
                        return;
                    }
                    if (objIntermediary.Error != null && objIntermediary.Error != "")
                    {
                        ucMsg.ShowError(objIntermediary.Error);
                        EnableDisableDirectSales();
                        return;
                    }
                    if (intResult == 2)
                    {
                        ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                        EnableDisableDirectSales();
                        return;
                    }
                    ClearForm();
                    EnableDisableDirectSales();
                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);

                }
            }
            else
            {
                EnableDisableDirectSales();
            }

        }

        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    public bool BulkCopyUpload(DataTable dtTempTable)
    {
        try
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "TransactionUploadBulk";
                bulkCopy.ColumnMappings.Add("ReturnFromSalesChannelCode", "ToCode");
                bulkCopy.ColumnMappings.Add("ReturnToSalesChannelCode", "FromCode");
                bulkCopy.ColumnMappings.Add("InvoiceNumber", "TransNumber");
                bulkCopy.ColumnMappings.Add("InvoiceDate", "TransDate");
                bulkCopy.ColumnMappings.Add("SKUCode", "SKUCode");
                bulkCopy.ColumnMappings.Add("Quantity", "Quantity");
                bulkCopy.ColumnMappings.Add("Serial#1", "Serial#1");
                bulkCopy.ColumnMappings.Add("BatchNo", "BatchNo");
                bulkCopy.ColumnMappings.Add("TransUploadSession", "TransUploadSession");
                bulkCopy.ColumnMappings.Add("TransType", "TransType");
                bulkCopy.WriteToServer(dtTempTable);
                return true;
            }

        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
