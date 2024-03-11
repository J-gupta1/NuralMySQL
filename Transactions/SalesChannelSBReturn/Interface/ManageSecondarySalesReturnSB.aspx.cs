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
 * 24 Feb 2015, Karam Chand Sharma, #CC02, Salesman option option according to configuration
 * 27 Apr 2015, Karam Chand Sharma, #CC03, javascript function fail when controle visible false so handled with style:display:None
 * 18 July 2016 , Karam Chand Sharma, #CC04, hide stock bin type dropdown
 * 27-Mar-2017, Sumit Maurya, #CC05, Interface modified to use it for H.O.
 * * 04-July-2018,Vijay Kumar Prajapati,#CC06,Add NumberofBackdayssalesreturnallow.(Done For ComioV5)
 */
public partial class Transactions_SalesChannelSBReturn_Interface_ManageSecondarySalesReturnSB : PageBase
{
    DateTime dt = new DateTime();
    DataSet dsSalesmanAndStockBinType;
    string strSalesChannelName, strSalesChannelCode;
    string[] strSplitSerialNumber;
    protected void Page_Load(object sender, EventArgs e)
    {

        strSalesChannelCode = hdnCode.Value;
        strSalesChannelName = hdnName.Value;

        DateTime dt = System.DateTime.Now.Date;

        StockBinTypeTobeAskedOrNot();
        //Added By Mamta Singh for checking back date before opening date
        /* #CC05 Add Start */
        if ((PageBase.HierarchyLevelID != 2))
        {/* #CC05 Add End */
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
           
        }/* #CC05 Added */
        ucDatePicker.MaxRangeValue = dt;
        ucDatePicker.RangeErrorMessage = "Invalid Date Range";
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
        try
        {
            dt = System.DateTime.Now.Date;
            ucMsg.ShowControl = false;
            if (!IsPostBack)
            {
                ViewState["NumberofBackDaysAllowed"] = 0;/* #CC05 Added */
                if (PageBase.HierarchyLevelID != 2) /* #CC05 Added */
                    FillSalesmanAndStockBinType();
                lblChange.Text = Resources.Messages.SalesEntity;
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
                }/* #CC02 START ADDED*/
                /* #CC05 Add Start */
                if (PageBase.HierarchyLevelID == 2)
                {
                    ddlSalesman.Items.Clear();
                    ddlSalesman.Items.Add(new ListItem("Select", "0"));
                }
                else
                { /* #CC05 Add End */
                    if (spanSalesManOptional.Visible == false)
                        ddlSalesman_SelectedIndexChanged(null, null);/* #CC02 START END*/
                } /* #CC05 Added */
                ucDatePicker.Date = dt.ToString();
                ddlSalesChannel.AutoPostBack = PageBase.HierarchyLevelID == 2 ? true : false; /* #CC05 Added */
            }
            if (PageBase.HierarchyLevelID != 2)
            {
                PartLookupClientSide1.SalesChannelID = PageBase.SalesChanelID == 0 ? "0" : PageBase.SalesChanelID.ToString();
                PartLookupClientSide1.SalesChannelCode = PageBase.SalesChanelID == 0 ? ddlSalesChannel.SelectedValue : PageBase.SalesChanelCode.ToString();
            }
            else
            {
                PartLookupClientSide1.SalesChannelID = ddlSalesChannel.SelectedValue;
                PartLookupClientSide1.SalesChannelCode = ddlSalesChannel.SelectedValue == "0" ? "0" : ddlSalesChannel.SelectedItem.Text.Split('(')[1].ToString().Replace(")", "");
            }
            PartLookupClientSide1.SalesTypeID = 3;
            Session["ReturnSalesType"] = 3;  //Here this is hardcoded will be removed as i will get the option for  the 
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
            if (hdnDirectSalesOfSerialAllowed.Value == "0")
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
                    drow["ReturnFromSalesChannelCode"] = ddlRetailerForCode.SelectedValue;
                    /* drow["ReturnToSalesChannelCode"] = PageBase.SalesChanelCode;  #CC05 Commented */
                    drow["ReturnToSalesChannelCode"] = PageBase.HierarchyLevelID == 2 ? ddlSalesChannel.SelectedItem.Text.Split('(')[1].ToString().Replace(")", "") : PageBase.SalesChanelCode; /* #CC05 Added */
                    drow["InvoiceNumber"] = dr["InvoiceNumber"].ToString();
                    drow["InvoiceDate"] = dr["InvoiceDate"];
                    drow["SKUCode"] = dr["SKUCode"].ToString();
                    drow["Quantity"] = Convert.ToInt32(dr["Quantity"]);
                    drow["Serial#1"] = dr["Serialno"].ToString();
                    drow["BatchNo"] = dr["BatchNo"].ToString();
                    Tvp.Rows.Add(drow);
                }
                Tvp.AcceptChanges();

                Tvp.Columns.Add(AddColumn(guid, "TransUploadSession", typeof(System.String)));
                Tvp.Columns.Add(AddColumn("7", "TransType", typeof(System.Int32)));
                Tvp.AcceptChanges();
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
                    objSecondary.TemplateType = "2";//Serial Only
                    objSecondary.ComingFrom = 2;//1-from upload 2 from interface
                    intResult = objSecondary.InsertSecondarySalesReturnSBBCP();

                    if (objSecondary.ErrorDetailXML != null && objSecondary.ErrorDetailXML != string.Empty)
                    {
                        ucMsg.XmlErrorSource = objSecondary.ErrorDetailXML;
                        EnableDisableDirectSales();
                        return;
                    }
                    if (objSecondary.Error != null && objSecondary.Error != "")
                    {
                        ucMsg.ShowError(objSecondary.Error);
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
                    btnGo.Enabled = true;

                }

                /*#CC01 COMMENTED using (SalesData objSecondary = new SalesData())
                {
                    objSecondary.EntryType = EnumData.eEntryType.eUpload;
                    objSecondary.UserID = PageBase.UserId;
                    objSecondary.ReturnDate = Convert.ToDateTime(ucDatePicker.Date);
                    objSecondary.StockBinType = Convert.ToInt16(ddlStockBinType.SelectedValue);
                    intResult = objSecondary.InsertSecondarySalesReturnSB(Tvp);
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
                    //PartLookupClientSide1.SubmittingTable = new DataTable();
                    //PartLookupClientSide1.IsBlankDataTable = true;
                    ClearForm();
                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    pnlGrid.Visible = false;

                }*/
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
        if ((ddlSalesChannel.SelectedIndex == 0 /* #CC02 START ADDED*/&& spanSalesManOptional.Visible == true)/* #CC02 START END*/ || ServerValidation.IsDate(ucDatePicker.Date, true) != 0)
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
    bool ChckDateInput()
    {
        DateTime dateTime;
        if (!DateTime.TryParse(ucDatePicker.Date, out dateTime))
        {
            return false;
        }
        return true;
    }



    void ClearForm()
    {
        hdnCode.Value = "";
        hdnName.Value = "";
        ucDatePicker.Date = DateTime.Now.Date.ToString("MM/dd/yyyy");
        ddlRetailerForCode.Items.Clear();
        ddlRetailer.Items.Insert(0, new ListItem("Select", "0"));
        ddlRetailer.Items.Clear();
        ddlSalesman.SelectedIndex = 0;
        ddlStockBinType.SelectedIndex = 0;
        ddlStockBinType.Enabled = true;
        ucDatePicker.imgCal.Enabled = true;

        ucDatePicker.TextBoxDate.Enabled = true;
        //ddlStockBinType.Items.Clear();
        //ddlStockBinType.Items.Insert(0, new ListItem("Select", "0"));
        
        BindSalesChannel();
        
        txtDirectSerialNumber.Text = string.Empty;

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
        /* #CC02 START ADDED*/
        if (spanSalesManOptional.Visible == false)
            ddlSalesman_SelectedIndexChanged(null, null);
        /* #CC02 START END*/
       

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        ClearForm();
        PartLookupClientSide1.SubmittingTable = new DataTable();
        PartLookupClientSide1.IsBlankDataTable = true;
        pnlGrid.Visible = false;
        ddlStockBinType.Enabled = true;/*#CC01 Added*/
        hdnDirectSalesOfSerialAllowed.Value = "0";
        btnGo.Enabled = true;
        ucDatePicker.imgCal.Enabled = true;
        ucDatePicker.TextBoxDate.Enabled = true;
        ucDatePicker.IsEnabled = true;
        Response.Redirect("~/Transactions/SalesChannelSBReturn/Interface/ManageSecondarySalesReturnSB.aspx");

        

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
            /* #CC05 Add Start */
            if (PageBase.HierarchyLevelID == 2)
            {
                using (SalesData objSales = new SalesData())
                {
                    objSales.UserID = PageBase.UserId;
                    DataSet ds = objSales.dsSecondarySalesReturnDropdownData();
                    if (objSales.TotalRecords > 0)
                    {
                        String[] StrCol = new String[] { "SalesChannelid", "SalesChannelName" };
                        PageBase.DropdownBinding(ref ddlSalesChannel, ds.Tables[0], StrCol);
                    }

                }
            }
            else
            {/* #CC05 Add End */
                using (SalesChannelData ObjSalesChannel = new SalesChannelData())
                {
                    ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
                    ObjSalesChannel.ActiveStatus = 1;
                    string[] str = { "SalesChannelid", "SalesChannelName" };
                    PageBase.DropdownBinding(ref ddlSalesChannel, ObjSalesChannel.GetSalesChannelList(), str);
                }
            }/* #CC05 Added */
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
        /* #CC05 Add Start */
        try
        {
            if (PageBase.HierarchyLevelID == 2)
            {
                using (SalesData objSales = new SalesData())
                {
                    objSales.UserID = 0;
                    objSales.BilltoRetailer = -1;
                    objSales.SalesChannelCode = ddlSalesChannel.SelectedItem.Text.Split('(')[1].Replace(")", "").ToString();
                    objSales.SalesChannelTypeID = 0;
                    DataSet ds = objSales.dsSecondarySalesReturnDropdownData();
                    if (objSales.TotalRecords > 0)
                    {
                        ViewState["NumberofBackDaysAllowed"] = "-" + ds.Tables[0].Rows[0]["NumberOfBackDays"].ToString();
                        lblValidationDays.Visible = true;
                        lblValidationDays.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", ds.Tables[0].Rows[0]["NumberOfBackDays"].ToString());
                        ucDatePicker.MinRangeValue = System.DateTime.Now.Date.AddDays(Convert.ToInt32(Convert.ToString(ViewState["NumberofBackDaysAllowed"])));
                    }

                }

                if (ddlSalesChannel.SelectedValue == "0")
                {
                    ClearddlListItems();
                }
                else
                {
                    ddlSalesman_SelectedIndexChanged(null, null);
                    FillSalesmanAndStockBinType();
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }/* #CC05 Add End */
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            /* #CC05 Add Start */
            if (PageBase.HierarchyLevelID == 2)
            {
                if (Convert.ToDateTime(DateTime.Now.Date).AddDays(Convert.ToInt32(ViewState["NumberofBackDaysAllowed"].ToString())) > Convert.ToDateTime(ucDatePicker.Date))
                {
                    ucMsg.ShowInfo("Invalid Date Range");
                    return;
                }
            }
            /* #CC05 Add End */

            if (!ChckDateInput())
            {
                ucMsg.ShowError("Invalid Date format");
                return;
            }
            if (!pageValidateSave())
            {
                ucMsg.ShowError("Invalid Parameters Value");
                return;
            }
            btnGo.Enabled = false;
            PartLookupClientSide1.SubmittingTable = new DataTable();
            PartLookupClientSide1.IsBlankDataTable = true;
            PartLookupClientSide1.ReturnFromSalesChannelID = Convert.ToString(ddlRetailer.SelectedValue);
            Session["ReturnFromSalesChannelID"] = ddlRetailer.SelectedValue;
            ddlRetailerForCode.SelectedIndex = ddlRetailer.SelectedIndex;
            pnlGrid.Visible = true;
            ddlRetailer.Enabled = false;
            ddlStockBinType.Enabled = false;
            ucDatePicker.imgCal.Enabled = false;
            ucDatePicker.TextBoxDate.Enabled = false;
            EnableDisableDirectSales();
            ddlStockBinType.Enabled = false;/*#CC01 ADDED */

        }
        catch (Exception ex)
        {

        }

    }

    protected void ddlSalesman_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlRetailer.Items.Clear();
        ddlRetailer.Items.Insert(0, new ListItem("Select", "0"));

        using (RetailerData ObjRetailer = new RetailerData())
        {
            /* #CC05 Add Start  */
            if (PageBase.HierarchyLevelID == 2)
                ObjRetailer.SalesChannelID = Convert.ToInt32(ddlSalesChannel.SelectedValue == "" ? "0" : ddlSalesChannel.SelectedValue);
            else /* #CC05 Add End */
                ObjRetailer.SalesChannelID = PageBase.SalesChanelID;
            ObjRetailer.SalesmanID = Convert.ToInt32(ddlSalesman.SelectedValue);
            ObjRetailer.Type = 1;               //For 1 because we are going to get only active Retailers
            string[] str = { "RetailerID", "Retailer" };
            PageBase.DropdownBinding(ref ddlRetailer, ObjRetailer.GetRetailerInfo(), str);
            string[] str1 = { "RetailerCode", "Retailer" };
            PageBase.DropdownBinding(ref ddlRetailerForCode, ObjRetailer.GetRetailerInfo(), str1);
        };
        ddlRetailer.ValidationGroup = "EntryValidation";
        reqRetailer.Enabled = true;
        ddlRetailer.Enabled = true;
        // PartLookupClientSide1.SubmittingTable = new DataTable();
        //  PartLookupClientSide1.IsBlankDataTable = true;

    }
    void FillSalesmanAndStockBinType()
    {
        String[] StrCol;
        using (SalesmanData ObjSalesman = new SalesmanData())
        {
            ObjSalesman.Type = EnumData.eSearchConditions.Active;
            /* #CC05 Add Start  */
            if (PageBase.HierarchyLevelID == 2)
                ObjSalesman.SalesChannelID = Convert.ToInt32(ddlSalesChannel.SelectedValue == "" ? "0" : ddlSalesChannel.SelectedValue);
            else /* #CC05 Add End */
                ObjSalesman.SalesChannelID = PageBase.SalesChanelID;
            ObjSalesman.MapwithRetailer = 1;
            dsSalesmanAndStockBinType = ObjSalesman.GetSalesmanAndStockBinTypeInfo();
            StrCol = new String[] { "SalesmanID", "Salesmanname" };

            PageBase.DropdownBinding(ref ddlSalesman, dsSalesmanAndStockBinType.Tables[0], StrCol);
            StrCol = new String[] { "StockBinTypeMasterID", "StockBinTypeDesc" };
            PageBase.DropdownBinding(ref ddlStockBinType, dsSalesmanAndStockBinType.Tables[1], StrCol);

        };
    }

    protected void rdModelList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdModelList.SelectedValue == "0")
            Response.Redirect("~/Transactions/SalesChannelSBReturn/Upload/ManageUploadSecondarySalesReturnSB-BCP.aspx"); /*#CC05 Added  */
        /* Response.Redirect("~/Transactions/SalesChannelSBReturn/Upload/ManageUploadSecondarySalesReturnSB.aspx"); #CC05 Commented */
    }

    void StockBinTypeTobeAskedOrNot()
    {
        if (Session["SalesReturnBinAsking"] != null)
        {
            if (Convert.ToInt16(Session["SalesReturnBinAsking"]) == 0)
            {
                /*#CC03 COMMENTED dvStockBinType.Visible = false;*/
                dvStockBinType.Attributes.Add("style", "display:none");/*#CC03 ADDED*/
                ddlStockBinType.Items.Clear();
                ddlStockBinType.Items.Insert(0, new ListItem("Select", "0"));
                ddlStockBinType.ValidationGroup = "";
                reqStockBinType.Enabled = false;

            }
            else
            {
                /*#CC03 COMMENTED dvStockBinType.Visible = true;*/
                /*#CC04 START COMMENTED dvStockBinType.Attributes.Add("style", "display:block");#CC03 ADDED
                ddlStockBinType.ValidationGroup = "EntryValidation"; 
                reqStockBinType.Enabled = true;#CC04 END COMMENTED */
                dvStockBinType.Attributes.Add("style", "display:none");/*#CC04 ADDED*/
                ddlStockBinType.ValidationGroup = "";/*#CC04 ADDED*/
                reqStockBinType.Enabled = false;
            }


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
                        drow["ReturnFromSalesChannelCode"] = ddlRetailerForCode.SelectedValue;
                        /* drow["ReturnToSalesChannelCode"] = PageBase.SalesChanelCode; #CC05 Commented */
                        drow["ReturnToSalesChannelCode"] = PageBase.HierarchyLevelID == 2 ? ddlSalesChannel.SelectedItem.Text.Split('(')[1].ToString().Replace(")", "") : PageBase.SalesChanelCode; /* #CC05 Added */
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
                DsDetail.Columns.Add(AddColumn("7", "TransType", typeof(System.Int32)));
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

                using (SalesData objSecondary = new SalesData())
                {
                    objSecondary.EntryType = EnumData.eEntryType.eUpload;
                    objSecondary.UserID = PageBase.UserId;
                    objSecondary.ReturnDate = Convert.ToDateTime(ucDatePicker.Date);
                    objSecondary.StockBinType = Convert.ToInt16(ddlStockBinType.SelectedValue);
                    objSecondary.TransUploadSession = guid;
                    objSecondary.TemplateType = "2";//Serial Only
                    objSecondary.ComingFrom = 2;//1-from upload 2 from interface
                    intResult = objSecondary.InsertSecondarySalesReturnSBBCP();

                    if (objSecondary.ErrorDetailXML != null && objSecondary.ErrorDetailXML != string.Empty)
                    {
                        ucMsg.XmlErrorSource = objSecondary.ErrorDetailXML;
                        EnableDisableDirectSales();
                        return;
                    }
                    if (objSecondary.Error != null && objSecondary.Error != "")
                    {
                        ucMsg.ShowError(objSecondary.Error);
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
                    btnGo.Enabled = true;
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

    /* #CC05 Add Start */
    public void ClearddlListItems()
    {
        try
        {
            ddlSalesman.Items.Clear();
            ddlSalesman.Items.Insert(0, new ListItem("Select", "0"));
            ddlRetailer.Items.Clear();
            ddlRetailer.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    /* #CC05 Add End */
}
