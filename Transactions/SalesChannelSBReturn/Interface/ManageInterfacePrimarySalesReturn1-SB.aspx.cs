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
 * 22 Jan 2015, Karam Chand Sharma, #CC01, stock bin type dropdown enables set false.
 * 18 July 2016 , Karam Chand Sharma, #CC02, hide stock bin type dropdown
 */
public partial class Transactions_SalesChannelSBReturn_Interface_ManageInterfacePrimarySalesReturn1_SB : PageBase
{
    DateTime dt = new DateTime();
    string strSalesChannelName, strSalesChannelCode;
    string[] strSplitSerialNumber;

    protected void Page_Load(object sender, EventArgs e)
    {

        strSalesChannelCode = hdnCode.Value;
        strSalesChannelName = hdnName.Value;
        txtSearchedName.Text = strSalesChannelName;
        StockBinTypeTobeAskedOrNot();
        btnsearch.Attributes.Add("onclick", "return popup();");
        try
        {
            dt = System.DateTime.Now.Date;
            ucDatePicker.MaxRangeValue = dt;
            ucDatePicker.RangeErrorMessage = "Invalid Date";
            ucMsg.ShowControl = false;
            StockBinTypeTobeAskedOrNot();
            if (!IsPostBack)
            {


                FillStockBinType();
                lblChange.Text = Resources.Messages.SalesEntity;
                lblChangeto.Text = Resources.Messages.SalesEntity;

                BindWareshouse();
                if (PageBase.SalesChanelID == 0)
                {
                    ddlWarehouse.Enabled = true;
                    ddlWarehouse.SelectedValue = "0";
                }
                else
                {
                    ddlWarehouse.SelectedValue = PageBase.SalesChanelCode;
                    ddlWarehouse.Enabled = false;
                }
                ucDatePicker.Date = dt.ToString();

            }
            PartLookupClientSide1.SalesChannelID = PageBase.SalesChanelID == 0 ? "0" : PageBase.SalesChanelID.ToString();
            PartLookupClientSide1.SalesChannelCode = PageBase.SalesChanelID == 0 ? ddlWarehouse.SelectedValue : PageBase.SalesChanelCode.ToString();
            PartLookupClientSide1.SalesTypeID = 1;
            Session["ReturnSalesType"] = 1;  //Here this is hardcoded will be removed as i will get the option for  the 
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
                drow["ReturnFromSalesChannelCode"] = hdnCode.Value;
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


            /*#CC01 Commented using (SalesData objIntermediary = new SalesData())
            {
                objIntermediary.EntryType = EnumData.eEntryType.eInterface;//useless
                objIntermediary.UserID = PageBase.UserId;
                objIntermediary.ReturnDate = Convert.ToDateTime(ucDatePicker.Date);
                objIntermediary.StockBinType = Convert.ToInt16(ddlStockBinType.SelectedValue);
                intResult = objIntermediary.InsertPrimarySalesReturn1SB(Tvp);
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
                //PartLookupClientSide1.SubmittingTable = new DataTable();
                //PartLookupClientSide1.IsBlankDataTable = true;
                ClearForm();
                ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                pnlGrid.Visible = false;

            }*/
            /*#CC01 START ADDED*/
            Tvp.Columns.Add(AddColumn(guid, "TransUploadSession", typeof(System.String)));
            Tvp.Columns.Add(AddColumn("3", "TransType", typeof(System.Int32)));
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
            using (SalesData objPrimaryReturn = new SalesData())
            {
                objPrimaryReturn.EntryType = EnumData.eEntryType.eUpload;
                objPrimaryReturn.UserID = PageBase.UserId;
                objPrimaryReturn.ReturnDate = Convert.ToDateTime(ucDatePicker.Date);
                objPrimaryReturn.StockBinType = Convert.ToInt16(ddlStockBinType.SelectedValue);
                objPrimaryReturn.TransUploadSession = guid;
                objPrimaryReturn.ComingFrom = 2;

                intResult = objPrimaryReturn.InsertPrimarySalesReturn1SBBCP();

                if (objPrimaryReturn.ErrorDetailXML != null && objPrimaryReturn.ErrorDetailXML != string.Empty)
                {
                    ucMsg.XmlErrorSource = objPrimaryReturn.ErrorDetailXML;
                    EnableDisableDirectSales();
                    return;
                }
                if (objPrimaryReturn.Error != null && objPrimaryReturn.Error != "")
                {
                    ucMsg.ShowError(objPrimaryReturn.Error);
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
                ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                pnlGrid.Visible = false;
            }
            /*#CC01 START END*/

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }




    }
    bool pageValidateSave()
    {

        if (ddlWarehouse.SelectedIndex == 0 || ServerValidation.IsDate(ucDatePicker.Date, true) != 0)
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



    void ClearForm()
    {
        // txtSearchedName.Text = "";
        hdnCode.Value = "";
        hdnName.Value = "";
        ucDatePicker.Date = DateTime.Now.Date.ToString();
        txtSearchedName.Text = "";
        // BindSalesChannel();

        if (PageBase.SalesChanelID != 0)
        {

            ddlWarehouse.SelectedValue = PageBase.SalesChanelCode.ToString();
            ddlWarehouse.Enabled = false;

        }
        else
        {
            ddlWarehouse.ClearSelection();
            ddlWarehouse.SelectedIndex = 0;
            ddlWarehouse.Enabled = true;

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
            ddlWarehouse.Items.Clear();
            using (SalesChannelData ObjSalesChannel = new SalesChannelData())
            {
                ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
                ObjSalesChannel.ActiveStatus = 1;
                string[] str = { "SalesChannelid", "SalesChannelName" };
                PageBase.DropdownBinding(ref ddlWarehouse, ObjSalesChannel.GetSalesChannelList(), str);
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }


    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            PartLookupClientSide1.SubmittingTable = new DataTable();
            PartLookupClientSide1.IsBlankDataTable = true;
            PartLookupClientSide1.ReturnFromSalesChannelID = hdnSalesChannelID.Value.Trim();
            Session["ReturnFromSalesChannelID"] = hdnSalesChannelID.Value.Trim();
            pnlGrid.Visible = true;
            ucDatePicker.imgCal.Enabled = false;
            ucDatePicker.TextBoxDate.Enabled = false;
            EnableDisableDirectSales();
            ddlStockBinType.Enabled = false;/*#CC01 ADDED */
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }

    }

    void BindWareshouse()
    {
        DataSet dsWarehosue = new DataSet();
        try
        {
            using (SalesChannelData objSalesData = new SalesChannelData())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.ePrimary1Sales;
                objSalesData.SalesChannelID = PageBase.SalesChanelID;
                objSalesData.Brand = PageBase.Brand;    //Pankaj Dhingra
                dsWarehosue = objSalesData.GetAllTemplateData();
                if (dsWarehosue != null && dsWarehosue.Tables.Count > 0)
                {
                    string[] str = { "SalesChannelCode", "SalesChannelName" };
                    PageBase.DropdownBinding(ref ddlWarehouse, dsWarehosue.Tables[0], str);
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

    protected void rdModelList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdModelList.SelectedValue == "0")
            /*#CC02 START COMMENTED  Response.Redirect("~/Transactions/SalesChannelSBReturn/Upload/ManageUploadPrimarySalesReturn1-SB.aspx"); #CC02 END COMMENTED*/
            Response.Redirect("~/Transactions/SalesChannelSBReturn/Upload/ManageUploadPrimarySalesReturn1-SB-BCP.aspx");/*#CC02 ADDED*/
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
                reqStockBinType.Enabled = true; #CC02 END COMMENTED  */

                dvStockBinType.Visible = false; /*#CC02 ADDED */
                ddlStockBinType.ValidationGroup = "";/*#CC02 ADDED */
                reqStockBinType.Enabled = false;/*#CC02 ADDED */
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
                bulkCopy.ColumnMappings.Add("BinCode", "StockBinType");/*#CC02 ADDED*/
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
                        drow["ReturnFromSalesChannelCode"] = hdnCode.Value;
                        drow["ReturnToSalesChannelCode"] = PageBase.SalesChanelCode;
                        drow["InvoiceNumber"] = null;
                        drow["InvoiceDate"] = ucDatePicker.Date; ;
                        drow["SKUCode"] = null;
                        drow["Quantity"] = 1;
                        drow["Serial#1"] = strDirectSerialNumber.Trim();
                        drow["BinCode"] = null;/*#CC02 ADDED*/                       
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

                using (SalesData objPrimaryReturn = new SalesData())
                {
                    objPrimaryReturn.EntryType = EnumData.eEntryType.eUpload;
                    objPrimaryReturn.UserID = PageBase.UserId;
                    objPrimaryReturn.ReturnDate = Convert.ToDateTime(ucDatePicker.Date);
                    objPrimaryReturn.StockBinType = Convert.ToInt16(ddlStockBinType.SelectedValue);
                    objPrimaryReturn.TransUploadSession = guid;
                    objPrimaryReturn.TemplateType = "2";//Serial Only
                    objPrimaryReturn.ComingFrom = 2;//1-from upload 2 from interface
                    intResult = objPrimaryReturn.InsertPrimarySalesReturn1SBBCP();

                    if (objPrimaryReturn.ErrorDetailXML != null && objPrimaryReturn.ErrorDetailXML != string.Empty)
                    {
                        ucMsg.XmlErrorSource = objPrimaryReturn.ErrorDetailXML;
                        EnableDisableDirectSales();
                        return;
                    }
                    if (objPrimaryReturn.Error != null && objPrimaryReturn.Error != "")
                    {
                        ucMsg.ShowError(objPrimaryReturn.Error);
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

}
