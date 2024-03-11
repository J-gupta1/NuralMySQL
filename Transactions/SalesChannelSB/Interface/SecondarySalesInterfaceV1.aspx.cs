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
 * 20 Jan 2014, Karam Chand Sharma, #CC01, Correct the flow according to config value.BCP function was not calling so correct them
 * 24 Feb 2015, Karam Chand Sharma, #CC02, Salesman option option according to configuration
 * 08 Jan 2016, Karam Chand Sharma, #CC03, For excel upload it was redirecting wrong page 
 */
public partial class Transactions_SalesChannel_SecondarySalesInterfaceV1 : PageBase
{
    DateTime dt = new DateTime();
    string[] strSplitSerialNumber;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
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
            dt = System.DateTime.Now.Date;      //Pankaj Kumar
            // ucDatePicker.MinRangeValue = dt.AddDays(PageBase.NumberofBackDaysAllowed);
            //Added By Mamta Singh for checking back date before opening date
            if (Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening) >= Convert.ToDateTime(dt.AddDays(PageBase.NumberofBackDaysAllowed)))
            {
                ucDatePicker.MinRangeValue = Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-PageBase.BackDaysAllowedBeforeOpening);
                lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", (Convert.ToDateTime(dt.ToShortDateString()) - Convert.ToDateTime(Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening).ToShortDateString())).TotalDays.ToString());
            }
            else
            {
                ucDatePicker.MinRangeValue = dt.AddDays(PageBase.NumberofBackDaysAllowed);
                lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
            }

            ucDatePicker.MaxRangeValue = dt;
            ucDatePicker.RangeErrorMessage = "Invalid Date.";

            //lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
            ucMsg.ShowControl = false;
            /* #CC02 START ADDED*/
            Authenticates objAuthenticate = new Authenticates();
            objAuthenticate.ConfigKey = "SALESMANOPTIONAL";
            DataTable dtSALESMANOPTIONAL = new DataTable();
            dtSALESMANOPTIONAL = objAuthenticate.GetApplicationConfiguration();
            if (dtSALESMANOPTIONAL.Rows.Count > 0)
            {
                if (dtSALESMANOPTIONAL.Rows[0]["ConfigValue"].ToString() == "1")
                {
                    spanSalesManOptional.Visible = false;
                    RequiredFieldValidator2.InitialValue = "1";
                }
                else
                {
                    spanSalesManOptional.Visible = true;
                    RequiredFieldValidator2.IsValid = true;
                    RequiredFieldValidator2.InitialValue = "0";
                }
            }

            /* #CC02 START END*/
            if (!IsPostBack)
            {
                Page.Header.DataBind();
                FillSalesman();
                /* #CC02 START ADDED*/
                if (spanSalesManOptional.Visible == false)
                    ddlSalesman_SelectedIndexChanged(null, null);
                /* #CC02 START END*/
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }


    }

    void FillSalesman()
    {
        using (SalesmanData ObjSalesman = new SalesmanData())
        {
            ObjSalesman.Type = EnumData.eSearchConditions.Active;
            ObjSalesman.SalesChannelID = PageBase.SalesChanelID;
            ObjSalesman.MapwithRetailer = 1;
            String[] StrCol = new String[] { "SalesmanID", "Salesmanname" };
            PageBase.DropdownBinding(ref ddlSalesman, ObjSalesman.GetSalesmanInfo(), StrCol);

        };
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //PnlHide.Visible = true;
            if (Convert.ToDateTime(ucDatePicker.Date) > DateTime.Now)
            {
                ucMsg.ShowInfo(Resources.Messages.DateRangeValidation);
                return;
            }
            using (SalesData objSales = new SalesData())
            {
                objSales.SalesChannelID = PageBase.SalesChanelID;
                if (ddlRetailer.SelectedIndex != 0)
                {
                    objSales.RetailerCode = ddlRetailer.SelectedValue;
                }
                if (ucDatePicker.Date != "")
                {
                    objSales.InvoiceDate = Convert.ToDateTime(ucDatePicker.Date);
                }
                objSales.Brand = PageBase.Brand;
                DataTable dtSales = objSales.GetSecondarySales();
                if (dtSales.Rows.Count > 0)
                {
                    if (dtSales.Select("SecondarySalesID >0").Length > 0)
                    {
                        DataRow[] drowArray = dtSales.Select("SecondarySalesID > 0");
                        if (drowArray.Length > 0)
                        {
                            //   ddlRetailer.SelectedValue =((DataRow)drowArray.GetValue(0))["SalesToCode"].ToString();
                            ddlRetailer.Enabled = false;
                            ddlSalesman.SelectedValue = ((DataRow)drowArray.GetValue(0))["SalesmanID"].ToString();
                            ddlSalesman.Enabled = false;
                            ucDatePicker.TextBoxDate.Text = ((DataRow)drowArray.GetValue(0))["InvoiceDate"].ToString();
                            ucDatePicker.TextBoxDate.Enabled = false;
                            ucDatePicker.imgCal.Enabled = false;

                        }
                    }
                    else
                    {
                        ucDatePicker.imgCal.Enabled = true;
                        ucDatePicker.TextBoxDate.Enabled = true;

                        ddlSalesman.Enabled = true;
                        ddlRetailer.Enabled = true;
                    }
                }
                else
                {
                    ucDatePicker.imgCal.Enabled = true;
                    ddlSalesman.Enabled = true;
                    ddlRetailer.Enabled = true;
                    ucDatePicker.TextBoxDate.Enabled = true;
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

        if (ddlRetailer.SelectedIndex == 0 || ServerValidation.IsDate(ucDatePicker.Date, true) != 0 || (ddlSalesman.SelectedValue == "0" /* #CC02 START ADDED*/ && spanSalesManOptional.Visible == true /* #CC02 START END*/))
        {
            ucMsg.ShowWarning(Resources.Messages.MandatoryField);
            return false;
        }

        if (Convert.ToDateTime(ucDatePicker.Date) > DateTime.Now.Date)
        {
            ucMsg.ShowWarning(Resources.Messages.DateRangeValidation);
            return false;
        }

        if (Convert.ToDateTime(DateTime.Now.Date).AddDays(PageBase.NumberofBackDaysAllowed) > Convert.ToDateTime(ucDatePicker.Date))
        {
            ucMsg.ShowInfo(Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", "")));
            return false;
        } //Pankaj Kumar
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
    protected void rdoSelectMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoSelectMode.SelectedValue == "1")
        {
            /*#CC03 COMMENTED START Response.Redirect("../Upload/ManageSecondarySales-SB.aspx"); #CC03 COMMENTED END */
            Response.Redirect("../Upload/ManageSecondarySales-SB-BCP.aspx");/*#CC03 ADDED*/
        }
        else
        {
            Response.Redirect("../Interface/SecondarySalesInterfaceV1.aspx");
        }

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        ClearForm();
        PartLookupClientSide1.SubmittingTable = new DataTable();
        PartLookupClientSide1.IsBlankDataTable = true;
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //if (hdnDirectSalesOfSerialAllowed.Value == "0")
        //    btnSave_Click(btnSave, null);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //if (IsPageRefereshed == true)
            //{
            //    return;
            //}//Pankaj Kumar

            if (hdnDirectSalesOfSerialAllowed.Value == "0")
            {
                DataTable Dt = new DataTable();
                if (PartLookupClientSide1.SubmittingTable.Rows.Count > 0)
                {
                    Dt = PartLookupClientSide1.SubmittingTable;
                }
                else
                {
                    return;
                }
                string guid = Guid.NewGuid().ToString();/* #CC01 ADDED */
                int intResult = 0;
                DataTable Tvp = new DataTable();
                DataSet DsExcelDetail = new DataSet();
                using (CommonData ObjCommom = new CommonData())
                {
                    Tvp = ObjCommom.GettvpTableIntermediarySalesSB();
                }

                foreach (DataRow dr in Dt.Rows)
                {
                    DataRow drow = Tvp.NewRow();
                    drow["FromSalesChannelCode"] = PageBase.SalesChanelCode;
                    drow["ToSalesChannelCode"] = ddlRetailer.SelectedValue;
                    drow["OrderNumber"] = "";
                    drow["InvoiceNumber"] = txtInvoiceNo.Text.Trim();
                    drow["InvoiceDate"] = ucDatePicker.Date;
                    drow["SKUCode"] = dr["SKUCode"].ToString().Trim();
                    drow["Quantity"] = dr["Quantity"];
                    drow["Serial#1"] = dr["SerialNo"].ToString().Trim();
                    drow["BatchNo"] = dr["BatchNo"].ToString().Trim();
                    Tvp.Rows.Add(drow);
                }
                Tvp.AcceptChanges();

                /* #CC01 START ADDED */
                Tvp.Columns.Add(AddColumn(guid, "TransUploadSession", typeof(System.String)));
                Tvp.Columns.Add(AddColumn("6", "TransType", typeof(System.Int32)));
                Tvp.AcceptChanges();
                if (Tvp.Rows.Count > 0)
                {
                    if (!BulkCopyUpload(Tvp))
                    {
                        ucMsg.ShowError("Error Occured While transferring the data to the server");
                        return;
                    }

                }
                /* #CC01 START END */
                using (SalesData objP1 = new SalesData())
                {
                    objP1.EntryType = EnumData.eEntryType.eUpload;
                    objP1.UserID = PageBase.UserId;
                    objP1.TransUploadSession = guid; /* #CC01 ADDED */
                    /* #CC01 COMMENTED intResult = objP1.InsertSecondarySalesSB(Tvp);

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
                    /* #CC01 START ADDED */
                    intResult = objP1.InsertSecondarySalesSBBCP();

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
                  
                    /* #CC01 START END */
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
    void ClearForm()
    {
        txtInvoiceNo.Text = "";
        ddlRetailer.Items.Clear();
        ddlRetailer.Items.Insert(0, new ListItem("Select", "0"));
        ddlSalesman.SelectedValue = "0";
        ddlSalesman.Enabled = true;
        ucDatePicker.imgCal.Enabled = true;
        ucDatePicker.TextBoxDate.Enabled = true;
        ucDatePicker.TextBoxDate.Text = "";
        ddlRetailer.Enabled = true;
        txtDirectSerialNumber.Text = string.Empty;
        //PnlHide.Visible = false;
        /* #CC02 START ADDED*/
        if (spanSalesManOptional.Visible == false)
            ddlSalesman_SelectedIndexChanged(null, null);
        /* #CC02 START END*/
    }

    protected void ddlSalesman_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlRetailer.Items.Clear();
        ddlRetailer.Items.Insert(0, new ListItem("Select", "0"));

        using (RetailerData ObjRetailer = new RetailerData())
        {
            ObjRetailer.SalesChannelID = PageBase.SalesChanelID;
            ObjRetailer.SalesmanID = Convert.ToInt32(ddlSalesman.SelectedValue);
            ObjRetailer.Type = 1;               //For 1 because we are going to get only active Retailers
            string[] str = { "RetailerCode", "Retailer" };
            PageBase.DropdownBinding(ref ddlRetailer, ObjRetailer.GetRetailerInfo(), str);
        };
        PartLookupClientSide1.SubmittingTable = new DataTable();
        PartLookupClientSide1.IsBlankDataTable = true;
        updMain.Update();
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
                bulkCopy.ColumnMappings.Add("OrderNumber", "OrderNumber");
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
                        drow["ToSalesChannelCode"] = ddlRetailer.SelectedValue;
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
                DsDetail.Columns.Add(AddColumn("6", "TransType", typeof(System.Int32)));
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
                    intResult = objP1.InsertSecondarySalesSBBCP();

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
                    if (intResult == 10)
                    {
                        ucMsg.ShowError("Selected retailer is in in-active.");
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
