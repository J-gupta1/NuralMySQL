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
 * 20 Jan 2014, Karam Chand Sharma, #CC01, Correct the flow according to config value.
 */
public partial class Transactions_SalesChannel_PrimarySalesInterfaceForAddV1 : PageBase
{

    DateTime dt = new DateTime();
    string strSalesChannelName, strSalesChannelCode;

    string[] strSplitSerialNumber;
    protected void Page_Load(object sender, EventArgs e)
    {


        //1-Allowed 0-is not allowed
        if (Session["DirctSralSaleIntefce"] != null)
            hdnDirectSalesOfSerialAllowed.Value = Convert.ToString(Session["DirctSralSaleIntefce"]);

        if (hdnDirectSalesOfSerialAllowed.Value == "0")
        {
            tblDirectSerialPanel.Attributes.Add("style", "display:none");
            tblGrid.Attributes.Add("style", "display:block");
        }
        else
        {
            tblDirectSerialPanel.Attributes.Add("style", "display:block");
            tblGrid.Attributes.Add("style", "display:none");
        }
        //  hdnCode.Value = PageBase.SalesChanelCode;





        strSalesChannelCode = hdnCode.Value;
        strSalesChannelName = hdnName.Value;
        txtSearchedName.Text = strSalesChannelName;

        btnsearch.Attributes.Add("onclick", "return popup();");
        try
        {
            dt = System.DateTime.Now.Date;
            ucDatePicker.MaxRangeValue = dt;
            ucDatePicker.RangeErrorMessage = "Invalid Date";
            ucMsg.ShowControl = false;
            if (!IsPostBack)
            {

                lblChange.Text = Resources.Messages.SalesEntity;
                lblChangeTo.Text = Resources.Messages.SalesEntity;
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

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void rdoSelectMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoSelectMode.SelectedValue == "1")
        {
            Response.Redirect("../Upload/ManageUploadPrimarySales1-SB.aspx");
        }
    }

    protected void Page_PreRender(object s, EventArgs args)
    {
        if (hdnDirectSalesOfSerialAllowed.Value == "0")
            BtnSubmit_Click(btnSave, null);
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




        // PnlHide.Visible = true;
        txtSearchedName.Text = strSalesChannelName;
        // btnsearch.Enabled = false;
        try
        {
            //if (!pageValidateGo())
            //{
            //    return;
            //}

            if (hdnDirectSalesOfSerialAllowed.Value == "0")
            {

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
                /*#CC01 START ADDED */
                if (txtInvoiceNo.Text == "")
                {
                    ucMsg.ShowInfo("Please enter invoice no.");
                    txtInvoiceNo.Focus();
                    return;
                }
                /*#CC01 START END */
                DataTable dtSource = getOpeningStockTable(Dt);

                DataTable Tvp = new DataTable();
                using (CommonData ObjCommom = new CommonData())
                {
                    Tvp = ObjCommom.GettvpTablePrimarySales1SB();
                }

                foreach (DataRow dr in dtSource.Rows)
                {
                    DataRow drow = Tvp.NewRow();
                    drow["WareHouseCode"] = ddlWarehouse.SelectedValue;
                    drow["SalesChannelCode"] = strSalesChannelCode;
                    drow["OrderNumber"] = "";
                    drow["InvoiceNumber"] = txtInvoiceNo.Text.Trim();
                    drow["InvoiceDate"] = ucDatePicker.Date; ;
                    drow["SKUCode"] = dr["SKUCode"].ToString().Trim();
                    drow["Quantity"] = dr["Quantity"];
                    drow["Serial#1"] = dr["Serial#1"].ToString().Trim();
                    drow["BatchNo"] = dr["BatchNo"].ToString().Trim();
                    Tvp.Rows.Add(drow);
                }
                Tvp.AcceptChanges();
                string guid = Guid.NewGuid().ToString();
                Tvp.Columns.Add(AddColumn(guid, "TransUploadSession", typeof(System.String)));
                Tvp.Columns.Add(AddColumn("2", "TransType", typeof(System.Int32)));
                Tvp.AcceptChanges();
                if (Tvp.Rows.Count > 0)
                {
                    if (!BulkCopyUpload(Tvp))
                    {
                        ucMsg.ShowError("Error Occured While transferring the data to the server");
                        return;
                    }

                }


                using (SalesData objP1 = new SalesData())
                {
                    objP1.EntryType = EnumData.eEntryType.eUpload;
                    objP1.UserID = PageBase.UserId;
                    objP1.TransUploadSession = guid;
                    /*#CC01  COMMENTED intResult = objP1.InsertPrimarySales1SB(Tvp);

                    if (objP1.ErrorDetailXML != null && objP1.ErrorDetailXML != string.Empty)
                    {
                        ucMsg.XmlErrorSource = objP1.ErrorDetailXML;
                        return;
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
                    }*/
                    /*#CC01 START ADDED */
                    intResult = objP1.InsertPrimarySales1SBBCP();

                    if (objP1.ErrorDetailXML != null && objP1.ErrorDetailXML != string.Empty)
                    {
                        ucMsg.XmlErrorSource = objP1.ErrorDetailXML;
                        return;
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
                    /*#CC01 START END */
                    PartLookupClientSide1.SubmittingTable = new DataTable();
                    PartLookupClientSide1.IsBlankDataTable = true;
                    ClearForm();
                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);

                }
            }


        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    bool pageValidateSave()
    {

        if (ddlWarehouse.SelectedIndex == 0 || txtInvoiceNo.Text.Trim() == string.Empty || ServerValidation.IsDate(ucDatePicker.Date, true) != 0)
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

        if (txtInvoiceNo.Text.Trim() == string.Empty)
        {
            ucMsg.ShowWarning(Resources.Messages.EnterInvoiceNo);
            return false;
        }
        return true;
    }


    void ClearForm()
    {
        txtSearchedName.Text = "";
        hdnCode.Value = "";
        hdnName.Value = "";
        ucDatePicker.Date = DateTime.Now.Date.ToString();
        BindWareshouse();

        if (PageBase.SalesChanelID != 0)
        {

            ddlWarehouse.SelectedValue = PageBase.SalesChanelCode.ToString();
            ddlWarehouse.Enabled = false;

        }
        else
        {
            ddlWarehouse.ClearSelection();
            ddlWarehouse.SelectedValue = "0";
            ddlWarehouse.Enabled = true;

        }

        txtInvoiceNo.Text = "";
        //ucDatePicker.Date = "";
        ucDatePicker.imgCal.Enabled = true;

        ucDatePicker.TextBoxDate.Enabled = true;
        // pnlGrid.Visible = false;
        //  PnlHide.Visible = false;
        txtInvoiceNo.Enabled = true;
        txtDirectSerialNumber.Text = string.Empty;

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        ClearForm();
        // btnsearch.Enabled = true;
        PartLookupClientSide1.SubmittingTable = new DataTable();
        PartLookupClientSide1.IsBlankDataTable = true;
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
    protected void ddlWarehouse_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlWarehouse.SelectedIndex != 0)
        {
            // pnlGrid.Visible = false;

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
                bulkCopy.ColumnMappings.Add("WareHouseCode", "FromCode");
                bulkCopy.ColumnMappings.Add("SalesChannelCode", "ToCode");
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

    protected void btnSubmitDirectSerialSale_Click(object sender, EventArgs e)
    {

        try
        {
            int intResult;
            string guid = Guid.NewGuid().ToString();
            txtSearchedName.Text = strSalesChannelName;
            if (pageValidateSave())
            {
                DataTable DsDetail = new DataTable();
                using (CommonData ObjCommom = new CommonData())
                {
                    DsDetail = ObjCommom.GettvpTablePrimarySales1SB();
                }
                foreach (string strDirectSerialNumber in strSplitSerialNumber)
                {
                    if (strDirectSerialNumber != string.Empty)
                    {
                        DataRow drow = DsDetail.NewRow();
                        drow["WareHouseCode"] = ddlWarehouse.SelectedValue;
                        drow["SalesChannelCode"] = strSalesChannelCode;
                        drow["OrderNumber"] = "";
                        drow["InvoiceNumber"] = txtInvoiceNo.Text.Trim();
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
                DsDetail.Columns.Add(AddColumn("2", "TransType", typeof(System.Int32)));
                DsDetail.AcceptChanges();
                if (DsDetail.Rows.Count > 0)
                {
                    if (!BulkCopyUpload(DsDetail))
                    {
                        ucMsg.ShowError("Error Occured While transferring the data to the server");
                        return;
                    }

                }

                using (SalesData objP1 = new SalesData())
                {
                    objP1.EntryType = EnumData.eEntryType.eUpload;
                    objP1.UserID = PageBase.UserId;
                    objP1.TransUploadSession = guid;
                    objP1.ComingFrom = 2;//1-from upload 2 from interface
                    intResult = objP1.InsertPrimarySales1SBBCP();

                    if (objP1.ErrorDetailXML != null && objP1.ErrorDetailXML != string.Empty)
                    {
                        ucMsg.XmlErrorSource = objP1.ErrorDetailXML;
                        return;
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
                    ClearForm();
                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);

                }
            }




        }

        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }


}



