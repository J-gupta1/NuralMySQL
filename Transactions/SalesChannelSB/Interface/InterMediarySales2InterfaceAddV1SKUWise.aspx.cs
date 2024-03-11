
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

public partial class Transactions_SalesChannel_InterMediarySales2InterfaceAddV1SKUWise : PageBase
{
    string[] strSplitSerialNumber;
    private bool intIsBinTypeAdded = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HiddenField hdnshowSerialCheckBox = (HiddenField)PartLookupClientSide1.FindControl("hdnEnterSerialCheck");
            if (hdnshowSerialCheckBox != null)
            {
                hdnshowSerialCheckBox.Value = "0";
            }
          

            //1-Allowed 0-is not allowed
            //if (Session["DirctSralSaleIntefce"] != null)
            //    hdnDirectSalesOfSerialAllowed.Value = Convert.ToString(Session["DirctSralSaleIntefce"]);
            hdnDirectSalesOfSerialAllowed.Value = "0";

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
            DateTime dt = System.DateTime.Now.Date;


            //Added By Mamta Singh for checking back date before opening date
            if (Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening) >= Convert.ToDateTime(dt.AddDays(PageBase.NumberofBackDaysAllowed)))
            {

                ucDatePicker.MinRangeValue = Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-PageBase.BackDaysAllowedBeforeOpening);
                lblValidationDays.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", (Convert.ToDateTime(dt.ToShortDateString()) - Convert.ToDateTime(Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening).ToShortDateString())).TotalDays.ToString());
            }
            else
            {
                ucDatePicker.MinRangeValue = dt.AddDays(PageBase.NumberofBackDaysAllowed);
                lblValidationDays.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
            }
            ucDatePicker.MaxRangeValue = dt;
            ucDatePicker.RangeErrorMessage = "Invalid Date Range";

            Page.Header.DataBind();
            ucMsg.ShowControl = false;
            if (!IsPostBack)
            {

                lblChange.Text = Resources.Messages.SalesEntity;
                FillsalesChannel();
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    /*protected void rdoSelectMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoSelectMode.SelectedValue == "1")
        {
            /*Server.Transfer("../upload/ManageUploadIntermediarySales-SB.aspx");*/
           /* Response.Redirect("~/Transactions/SalesChannelSB/Upload/ManageUploadIntermediarySales-SB-BCP.aspx");
        }        
    }*/
    void FillsalesChannel()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            //ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(EnumData.eSalesChannelType.TD);
            ObjSalesChannel.SalesChannelID = PageBase.SalesChanelID;
            ObjSalesChannel.SearchType = EnumData.eSearchConditions.Active;
            ObjSalesChannel.StatusValue = 1;//always active
            ObjSalesChannel.BlnShowDetail = false;
            string[] str = { "SalesChannelCode", "DisplayName" };
            PageBase.DropdownBinding(ref ddlTD, ObjSalesChannel.GetSalesChannelInfo(), str);
        };

    }

    bool pageValidateSave()
    {

        if (ddlTD.SelectedIndex == 0 || txtInvoiceNo.Text.Trim() == string.Empty || ServerValidation.IsDate(ucDatePicker.Date, true) != 0)
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //if (IsPageRefereshed == true)
            //{
            //    return;
            //}


            //if (!pageValidateSave())
            //{
            //    return;
            //}
            if (hdnDirectSalesOfSerialAllowed.Value == "0")
            {
                DataTable Dtt = new DataTable();
                if (PartLookupClientSide1.SubmittingTable.Rows.Count > 0)
                {
                    Dtt = PartLookupClientSide1.SubmittingTable;
                    Dtt.Columns.Add("StockBinType", typeof(System.String));
                }
                else
                {
                    return;
                }

                string guid = Guid.NewGuid().ToString();
                int intResult = 0;
                DataTable Tvp = new DataTable();
                DataSet DsExcelDetail = new DataSet();
                using (CommonData ObjCommom = new CommonData())
                {
                    Tvp = ObjCommom.GettvpTableIntermediarySalesSB();
                    Tvp.Columns.Add("StockBinType", typeof(System.String));
                }


                foreach (DataRow dr in Dtt.Rows)
                {
                    DataRow drow = Tvp.NewRow();
                    drow["FromSalesChannelCode"] = PageBase.SalesChanelCode;
                    drow["ToSalesChannelCode"] = ddlTD.SelectedValue;
                    drow["OrderNumber"] = "";
                    drow["InvoiceNumber"] = txtInvoiceNo.Text.Trim();
                    drow["InvoiceDate"] = ucDatePicker.Date;
                    drow["SKUCode"] = dr["SKUCode"].ToString().Trim();
                    drow["Quantity"] = dr["Quantity"];
                    drow["Serial#1"] = dr["SerialNo"].ToString().Trim();
                    drow["BatchNo"] = dr["BatchNo"].ToString().Trim();
                    drow["StockBinType"] = Convert.ToString(dr["StockBinType"]).Trim();
                    Tvp.Rows.Add(drow);
                    intIsBinTypeAdded = true;
                }
                Tvp.AcceptChanges();
                
                Tvp.Columns.Add(AddColumn(guid, "TransUploadSession", typeof(System.String)));
                Tvp.Columns.Add(AddColumn("4", "TransType", typeof(System.Int32)));
                Tvp.AcceptChanges();
                if (Tvp.Rows.Count > 0)
                {
                    if (!BulkCopyUpload(Tvp))
                    {
                        ucMsg.ShowError("Error Occured While transferring the data to the server");
                        return;
                    }

                }
                /*#CC01 START END*/
                using (SalesData objP1 = new SalesData())
                {
                    objP1.EntryType = EnumData.eEntryType.eUpload;
                    objP1.UserID = PageBase.UserId;
                    objP1.ComingFrom = 2;//means from interface
                    objP1.TransUploadSession = guid;
                    intResult = objP1.InsertIntermediarySalesSBBCP();

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
                    PartLookupClientSide1.IsBlankDataTable = true;
                    PartLookupClientSide1.SubmittingTable = new DataTable();
                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    ClearForm();
                }
            }
            intIsBinTypeAdded = false;
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    void ClearForm()
    {

        ddlTD.ClearSelection();
        ddlTD.SelectedValue = "0";
        //txtSalesman.Text = "";
        txtInvoiceNo.Text = "";
        ucDatePicker.Date = "";
        ucDatePicker.imgCal.Enabled = true;
        //txtSalesman.Enabled = true;
        ddlTD.Enabled = true;
        ucDatePicker.TextBoxDate.Enabled = true;

        //PnlHide.Visible = false;
        txtInvoiceNo.Enabled = true;
        txtDirectSerialNumber.Text = string.Empty;

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        ClearForm();
        PartLookupClientSide1.SubmittingTable = new DataTable();
        PartLookupClientSide1.IsBlankDataTable = true;
    }



    protected void Page_PreRender(object sender, EventArgs args)
    {
        /*#CC02 COMMENTD
        btnSave_Click(btnSave, null);*/
    }
    public bool BulkCopyUpload(DataTable dtTempTable)
    {
        try
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "TransactionUploadBulk";
                bulkCopy.ColumnMappings.Add("FromSalesChannelCode", "FromCode");
                bulkCopy.ColumnMappings.Add("ToSalesChannelCode", "ToCode");
                bulkCopy.ColumnMappings.Add("InvoiceNumber", "TransNumber");
                bulkCopy.ColumnMappings.Add("InvoiceDate", "TransDate");
                bulkCopy.ColumnMappings.Add("SKUCode", "SKUCode");
                bulkCopy.ColumnMappings.Add("Quantity", "Quantity");
                bulkCopy.ColumnMappings.Add("Serial#1", "Serial#1");
                bulkCopy.ColumnMappings.Add("BatchNo", "BatchNo");
                bulkCopy.ColumnMappings.Add("TransUploadSession", "TransUploadSession");
                bulkCopy.ColumnMappings.Add("TransType", "TransType");
                if (intIsBinTypeAdded == true)
                {
                    bulkCopy.ColumnMappings.Add("StockBinType", "StockBinType");
                }
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

            if (pageValidateSave())
            {
                DataTable DsDetail = new DataTable();
                using (CommonData ObjCommom = new CommonData())
                {
                    DsDetail = ObjCommom.GettvpTableIntermediarySalesSB();
                }
                foreach (string strDirectSerialNumber in strSplitSerialNumber)
                {
                    if (strDirectSerialNumber != string.Empty)
                    {
                        DataRow drow = DsDetail.NewRow();
                        drow["FromSalesChannelCode"] = PageBase.SalesChanelCode;
                        drow["ToSalesChannelCode"] = ddlTD.SelectedValue;
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
                DsDetail.Columns.Add(AddColumn("4", "TransType", typeof(System.Int32)));
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
                    intResult = objP1.InsertIntermediarySalesSBBCP();

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

