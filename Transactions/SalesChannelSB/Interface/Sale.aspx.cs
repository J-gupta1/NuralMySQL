/*Change Log:
 * 02-Nov-2022, Adnan Mubeen, #CC01 - Added 'ddlSalesType.Items.Remove' to remove Primary Sale because it has a saperate Page for upload.
 * 15-Nov-2022, Adnan Mubeen, #CC02 - Change SalesChannelID to SalesChannelCode.
 */

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
using ZedService;
using BusinessLogics;


public partial class Transactions_Sale : PageBase
{
    DateTime dt = new DateTime();
    DataSet dsErrorProne = new DataSet();
    string[] strSplitSerialNumber;
    private bool intIsBinTypeAdded = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            GetvalidSession();
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
            dt = System.DateTime.Now.Date;      //Pankaj Kumar
                                                // ucDatePicker.MinRangeValue = dt.AddDays(PageBase.NumberofBackDaysAllowed);
                                                //Added By Mamta Singh for checking back date before opening date
                                                //if (Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening) >= Convert.ToDateTime(dt.AddDays(PageBase.NumberofBackDaysAllowed)))
                                                //{
                                                //    ucDatePicker.MinRangeValue = Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-PageBase.BackDaysAllowedBeforeOpening);
                                                //    lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", (Convert.ToDateTime(dt.ToShortDateString()) - Convert.ToDateTime(Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening).ToShortDateString())).TotalDays.ToString());
                                                //}
                                                //else
                                                //{
            ucDatePicker.MinRangeValue = dt.AddDays(PageBase.NumberofBackDaysAllowed);
            lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
            //}

            if (PageBase.ClientId == 10036)
            {
                ucDatePicker.MinRangeValue = dt.AddDays(-365);
                lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", 365.ToString().Replace("-", ""));
            }

            ucDatePicker.MaxRangeValue = dt;
            ucDatePicker.RangeErrorMessage = "Invalid Date.";

            //lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
            ucMsg.ShowControl = false;
            /* #CC02 START ADDED
            DataTable dtSALESMANOPTIONAL = new DataTable();
            using (Authenticates objAuthenticate = new Authenticates())
            {
                objAuthenticate.ConfigKey = "SALESMANOPTIONAL";
                objAuthenticate.CompanyId = PageBase.ClientId;
                dtSALESMANOPTIONAL = objAuthenticate.GetApplicationConfiguration();
            }
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
            */

            if (!IsPostBack)
            {
                Page.Header.DataBind();
                FillDropDown(0);
                //if (spanSalesManOptional.Visible == false)
                //    ddlSalesman_SelectedIndexChanged(null, null);
                UISale.Visible = false;
                UploadSale.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }


    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        intIsBinTypeAdded = false;
        if (ddlSalesType.SelectedValue == "0")
        {
            ucMsg.ShowInfo("Please select sale type.");
            return;
        }

        if (rdbSalesWith.SelectedValue == "0")//UI sale
        {
            if (ddlSalesType.SelectedValue == "3" && PageBase.SalesChanelID > 0)
                FillDropDown(1);
            FillDropDown(2);
            UISale.Visible = true;
            UploadSale.Visible = false;
        }
        else if (rdbSalesWith.SelectedValue == "1")//Excel Upload
        {
            UISale.Visible = false;
            UploadSale.Visible = true;
        }
        else
        {
            ucMsg.ShowInfo("Please select UI type.");
            return;
        }
        ddlSalesType.Enabled = false;
        rdbSalesWith.Enabled = false;
        btnGo.Enabled = false;
    }
    protected void btnResetSaleType_Click(object sender, EventArgs e)
    {
        UISale.Visible = false;
        UploadSale.Visible = false;
        ddlSalesType.Enabled = true;
        ddlSalesType.SelectedIndex = 0;
        rdbSalesWith.Enabled = true;
        btnGo.Enabled = true;
        rdbSalesWith.Items[0].Selected = false;
        rdbSalesWith.Items[1].Selected = false;
        txtInvoiceNo.Text = "";
        ucDatePicker.TextBoxDate.Text = "";
        ddlRetailer.SelectedValue = "0";
        PartLookupClientSide1.SubmittingTable = new DataTable();
        PartLookupClientSide1.IsBlankDataTable = true;
    }

    void FillDropDown(Int16 Mastertype)
    {
        using (SalesData ObjSales = new SalesData())
        {
            ObjSales.UserID = PageBase.UserId;
            if (ddlFromSC.SelectedIndex > 0)
                ObjSales.SalesChannelID = Convert.ToInt32(ddlFromSC.SelectedValue);
            else
                ObjSales.SalesChannelID = PageBase.SalesChanelID;
            ObjSales.CompanyId = PageBase.ClientId;
            ObjSales.Mastertype = Mastertype;
            if (Mastertype != 0)
                ObjSales.SalestypeId = Convert.ToInt16(ddlSalesType.SelectedValue);
            String[] StrCol;
            if (Mastertype == 0)
            {
                StrCol = new String[] { "SalesTypeID", "SalesTypeName" };
                PageBase.DropdownBinding(ref ddlSalesType, ObjSales.FillSaleDropDown().Tables[0], StrCol);
                ddlSalesType.Items.Remove(new ListItem("Select", "0"));/* #CC01 Added */
                ddlSalesType.Items.Remove(new ListItem("Primary Sales (P1)", "1"));/* #CC01 Added */
            }
            else if (Mastertype == 1)
            {
                StrCol = new String[] { "SalesmanID", "Salesmanname" };
                PageBase.DropdownBinding(ref ddlSalesman, ObjSales.FillSaleDropDown().Tables[0], StrCol);
            }
            else if (Mastertype == 2)
            {
                StrCol = new String[] { "SalesChannelID", "SalesChannelName" };
                PageBase.DropdownBinding(ref ddlFromSC, ObjSales.FillSaleDropDown().Tables[0], StrCol);
                if (PageBase.SalesChanelID > 0 && ddlFromSC.Items.Count > 1)
                {
                    ddlFromSC.SelectedIndex = 1;
                }
            }
            else if (Mastertype == 3)
            {
                StrCol = new String[] { "SalesChannelCode", "SalesChannelName" };/* #CC02 Added */
                PageBase.DropdownBinding(ref ddlRetailer, ObjSales.FillSaleDropDown().Tables[0], StrCol);
            }
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

        if (ddlRetailer.SelectedIndex == 0 || ServerValidation.IsDate(ucDatePicker.Date, true) != 0  /*|| (ddlSalesman.SelectedValue == "0"   
            && spanSalesManOptional.Visible == true )*/
            )
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
    /*protected void rdoSelectMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoSelectMode.SelectedValue == "1")
        {   
            Response.Redirect("../Upload/ManageSecondarySales-SB-BCP.aspx");
        }
        else
        {
            Response.Redirect("../Interface/SecondarySalesInterfaceV1SKUWise.aspx");
        }

    }*/
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        ClearForm();
        PartLookupClientSide1.SubmittingTable = new DataTable();
        PartLookupClientSide1.IsBlankDataTable = true;
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {

            if (hdnDirectSalesOfSerialAllowed.Value == "0")
            {
                DataTable Dt = new DataTable();
                if (PartLookupClientSide1.SubmittingTable.Rows.Count > 0)
                {
                    Dt = PartLookupClientSide1.SubmittingTable;
                    Dt.Columns.Add("StockBinType", typeof(System.String));
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
                    if (Convert.ToString(dr["StockBinType"]).Trim() != null || Convert.ToString(dr["StockBinType"]).Trim() != "")
                    {
                        drow["StockBinType"] = Convert.ToString(dr["StockBinType"]).Trim();
                    }
                    else
                    {
                        drow["StockBinType"] = "";
                    }
                    drow["Rate"] = dr["Rate"].ToString().Trim();
                    drow["Amount"] = dr["Amount"].ToString().Trim();
                    Tvp.Rows.Add(drow);
                    intIsBinTypeAdded = true;
                }
                Tvp.AcceptChanges();


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

                Save(guid, 2, Convert.ToInt32(ddlFromSC.SelectedValue));
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
        ddlFromSC.SelectedValue = "0";
        //PnlHide.Visible = false;

    }
    protected void ddlFromSC_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlRetailer.Items.Clear();
        ddlRetailer.Items.Insert(0, new ListItem("Select", "0"));
        if (ddlFromSC.SelectedIndex > 0)
        {
            FillDropDown(1);
            FillDropDown(3);
            PartLookupClientSide1.SalesChannelID = ddlFromSC.SelectedValue;
            PartLookupClientSide1.CompanyId = PageBase.ClientId.ToString();
        }
    }
    protected void ddlSalesman_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlRetailer.Items.Clear();
        ddlRetailer.Items.Insert(0, new ListItem("Select", "0"));

        FillDropDown(3);
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
                bulkCopy.ColumnMappings.Add("Rate", "Rate");
                bulkCopy.ColumnMappings.Add("Amount", "Amount");

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
            //if (ddlSalesType.SelectedValue != "3")
            //{
            //    ucMsg.ShowInfo("Selected Sales Type is under development");
            //    return;
            //}

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


            }

            Save(guid, 2, Convert.ToInt32(ddlFromSC.SelectedValue));


        }

        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    private void Save(string guid, Int16 ComingFrom, int SelectedFromSalesChannelID)
    {
        int intResult;
        using (SalesData objP1 = new SalesData())
        {
            objP1.EntryType = EnumData.eEntryType.eUpload;
            objP1.UserID = PageBase.UserId;
            objP1.TransUploadSession = guid;
            objP1.ComingFrom = ComingFrom;//1-from upload 2 from interface
            objP1.CompanyId = PageBase.ClientId;
            objP1.SalestypeId = Convert.ToInt16(ddlSalesType.SelectedValue);
            objP1.SalesChannelID = SelectedFromSalesChannelID;
            if (ddlSalesType.SelectedValue == "3")
                intResult = objP1.InsertSecondarySalesSBBCP();
            else if (ddlSalesType.SelectedValue == "1")
                intResult = objP1.InsertPrimarySales1SBBCP();
            else
            {
                ucMsg.ShowInfo("Selected Sales Type is under development");
                return;
            }
            if (ComingFrom == 2)
            {
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
            else
            {
                if (objP1.ErrorDetailXML != null && objP1.ErrorDetailXML != string.Empty)
                {

                    hlnkInvalid.Visible = true;
                    string strFileName = "InvalidData" + DateTime.Now.Ticks;
                    System.IO.StringReader theReader = new System.IO.StringReader(objP1.ErrorDetailXML);
                    dsErrorProne.ReadXml(theReader);
                    ExportInExcel(dsErrorProne, strFileName);
                    hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                    hlnkInvalid.Text = "Invalid Data";
                    ucMsg.Visible = true;
                    ucMsg.ShowInfo("Please click on Invalid data to check the error obtained");

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
    public DataTable GetBlankTableError()
    {
        DataTable Detail = new DataTable();
        Detail.Columns.Add("InvoiceNumber");
        Detail.Columns.Add("FromSalesChannelCode");
        Detail.Columns.Add("ToSalesChannelCode");
        Detail.Columns.Add("ReasonForInvalid");
        return Detail;
    }

    protected void DwnldReferenceCodeTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (SalesChannelData objSalesData = new SalesChannelData())
            {
                objSalesData.UserID = PageBase.UserId;
                if (ddlSalesType.SelectedValue == "3")//Secondary
                    objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.eSecondarySales;
                else if (ddlSalesType.SelectedValue == "1")//Primary
                    objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.ePrimary1Sales;
                else if (ddlSalesType.SelectedValue == "2")//intermediary
                    objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.ePrimary2Sales;
                objSalesData.SalesChannelID = PageBase.SalesChanelID;
                objSalesData.Brand = PageBase.Brand;
                dsTemplateCode = objSalesData.GetAllTemplateData();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "SalesFromCode", "SalesToCode", "SkuCodeList", "Bincode", "BatchCodeList" };
                    ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, 5);

                    //PageBase.ExportToExecl(dsTemplateCode, FilenameToexport, EnumData.eTemplateCount.eSecondary+1);
                    if (dsTemplateCode.Tables.Count > 5)
                        dsTemplateCode.Tables.RemoveAt(5);
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsTemplateCode, FilenameToexport, 5, strExcelSheetName);
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
            // PageBase.Errorhandling(ex);
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            //if (IsPageRefereshed)
            //{
            //    return;

            //}

            UploadFile UploadFile = new UploadFile();
            string strUploadedFileName = string.Empty;

            DataTable dtErrorTable = GetBlankTableError();
            DataTable dtError = new DataTable();
            HttpContext.Current.Session["PkeyColumns"] = null;
            string strKey = string.Empty;
            ClearForm();
            hlnkInvalid.Visible = false;
            //hlnkDuplicate.Visible = false;
            //hlnkBlank.Visible = false;
            // String RootPath = Server.MapPath("../../");
            // UploadFile.RootFolerPath = RootPath;
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
                            System.Collections.SortedList objSL = new System.Collections.SortedList();
                            //System.Collections.SortedList objSLCorrData = new System.Collections.SortedList();
                            objValidateFile.UploadedFileName = strUploadedFileName;
                            objValidateFile.ExcelFileNameInTable = "SecondarySales-SB";
                            objValidateFile.CompanyId = PageBase.ClientId;

                            objValidateFile.ValidateFileWithCompanyId(false, out objDS, out objSL);

                            if (objValidateFile.Message != null && objValidateFile.Message.Trim() != "")
                                ucMsg.ShowInfo(objValidateFile.Message);
                            else
                            {
                                ucMsg.Visible = false;
                                bool blnIsUpload = true;
                                if (objSL.Count >= 1 && objSL.Keys.Count >= 1)
                                {
                                    objDS.Tables["DtExcelSheet"].Columns.Add(new DataColumn("ReasonForInvalid"));
                                    System.Collections.IDictionaryEnumerator objIDicEnum = objSL.GetEnumerator();
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
                                if (objDS.Tables.Count > 0 && !objDS.Tables[0].Columns.Contains("ReasonForInvalid"))
                                {
                                    int counter = 0;
                                    if (!objDS.Tables[0].Columns.Contains("ReasonForInvalid"))
                                        objDS.Tables[0].Columns.Add(new DataColumn("ReasonForInvalid"));

                                    if (counter > 0)
                                    {
                                        ucMsg.ShowInfo("Invalid Records");
                                        dsErrorProne.Merge(objDS.Tables[0]);
                                        blnIsUpload = false;
                                    }
                                    else
                                    {
                                        objDS.Tables[0].Columns.Remove("ReasonForInvalid");
                                    }
                                }
                                /* Commented by Adnan to Upload Duplicate Data */
                                /*if (objDS.Tables.Count > 0 && objDS.Tables["DtDuplicateRecord"] != null && objDS.Tables["DtDuplicateRecord"].Rows.Count > 0)
                                {
                                    dsErrorProne.Merge(objDS.Tables["DtDuplicateRecord"]);
                                    blnIsUpload = false;
                                }*/
                                if (objDS.Tables.Count > 0 && objDS.Tables["DtBlankData"] != null && objDS.Tables["DtBlankData"].Rows.Count > 0)
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
            if (dtGRN.Rows.Count > 0)
            {
                string guid = Guid.NewGuid().ToString();

                dtGRN.Columns.Add(AddColumn(guid, "TransUploadSession", typeof(System.String)));
                dtGRN.Columns.Add(AddColumn("6", "TransType", typeof(System.Int32)));
                dtGRN.AcceptChanges();

                intIsBinTypeAdded = true;
                if (!BulkCopyUpload(dtGRN))
                {
                    ucMsg.ShowError("Error Occured While transferring the data to the server");
                    return;
                }
                else
                {
                    Save(guid, 1, 0);//ComingFrom 1=from upload
                }

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }

    }


}
