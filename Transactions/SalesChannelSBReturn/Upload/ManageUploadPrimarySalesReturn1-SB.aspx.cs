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
using ZedService;

public partial class Transactions_SalesChannelSBReturn_ManageUploadPrimarySalesReturn1_SB : PageBase
{
    DataTable dtNew = new DataTable();
    object objSum;
    int counter = 0;
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    DateTime dt = new DateTime();
    UploadFile UploadFile = new UploadFile();
    List<String> lstDuplicate = new List<String>();
    DataSet dsErrorProne = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            dt = System.DateTime.Now.Date;
            StockBinTypeTobeAskedOrNot();
            if (PageBase.SalesChanelID != 0)
            {
                if (Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening) >= Convert.ToDateTime(dt.AddDays(PageBase.NumberofBackDaysAllowed)))
                {

                    ucSalesReturnDate.MinRangeValue = Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-PageBase.BackDaysAllowedBeforeOpening);
                    lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", (Convert.ToDateTime(dt.ToShortDateString()) - Convert.ToDateTime(Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening).ToShortDateString())).TotalDays.ToString());
                }
                else
                {
                    ucSalesReturnDate.MinRangeValue = dt.AddDays(PageBase.NumberofBackDaysAllowed);
                    lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
                }
            }
            ucMsg.ShowControl = false;
            ucSalesReturnDate.MaxRangeValue = dt;
            ucSalesReturnDate.RangeErrorMessage = "Invalid Date Range";
            if (!IsPostBack)
            {
                FillStockBinType();
                pnlGrid.Visible = false;
            }


        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());

        }

    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtErrorTable = GetBlankTableError();
            DataTable dtError = new DataTable();
            HttpContext.Current.Session["PkeyColumns"] = null;
            string strKey = string.Empty;
            ClearForm();
            hlnkInvalid.Visible = false;
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
                            SortedList objSL = new SortedList();
                            SortedList objSLCorrData = new SortedList();
                            objValidateFile.UploadedFileName = strUploadedFileName;
                            objValidateFile.ExcelFileNameInTable = "PrimarySalesReturn1-SB";
                            objValidateFile.ValidateFile(false, out objDS, out objSL);
                            if (objValidateFile.Message != null && objValidateFile.Message.Trim() != "")
                                ucMsg.ShowInfo(objValidateFile.Message);
                            else
                            {
                                ucMsg.Visible = false;
                                bool blnIsUpload = true;
                                if (objSL.Count >= 1 && objSL.Keys.Count >= 1)
                                {
                                    objDS.Tables["DtExcelSheet"].Columns.Add(new DataColumn("ReasonForInvalid"));
                                    IDictionaryEnumerator objIDicEnum = objSL.GetEnumerator();
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
                                if (!objDS.Tables[0].Columns.Contains("ReasonForInvalid"))
                                {
                                    int counter = 0;
                                    if (!objDS.Tables[0].Columns.Contains("ReasonForInvalid"))
                                        objDS.Tables[0].Columns.Add(new DataColumn("ReasonForInvalid"));
                                    if (Session["PRIMARYINGRNMODE"] != null)
                                    {
                                        if (Convert.ToInt32(Session["PRIMARYINGRNMODE"]) == 0)
                                        {
                                            if (PageBase.SalesChanelID != 0)
                                            {
                                                var query = from r in objDS.Tables[0].AsEnumerable()
                                                            where ((Convert.ToString(r["ToWarehouseCode"]) != PageBase.SalesChanelCode))
                                                            select new
                                                            {
                                                                InvoiceNumber = Convert.ToString(r["InvoiceNumber"]),
                                                                WarehouseCode = Convert.ToString(r["ToWarehouseCode"]),
                                                                SalesChannelCode = Convert.ToString(r["FromSalesChannelCode"])
                                                            };
                                                if (query != null)
                                                {
                                                    if (query.Count() > 0)
                                                    {
                                                        counter = 1;
                                                        dtError = PageBase.LINQToDataTable(query);
                                                        foreach (DataRow dr in dtError.Rows)
                                                        {
                                                            DataRow drow = dtErrorTable.NewRow();
                                                            drow["InvoiceNumber"] = dr["InvoiceNumber"];
                                                            drow["WarehouseCode"] = dr["WarehouseCode"];
                                                            drow["SalesChannelCode"] = dr["SalesChannelCode"];
                                                            drow["ReasonForInvalid"] = "Can't Upload the Sales of other SalesChannel";
                                                            dtErrorTable.Rows.Add(drow);
                                                        }
                                                        dtErrorTable.AcceptChanges();
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (PageBase.SalesChanelID != 0)
                                            {
                                                var query = from r in objDS.Tables[0].AsEnumerable()
                                                            where ((Convert.ToString(r["FromSalesChannelCode"]) != PageBase.SalesChanelCode))
                                                            select new
                                                            {
                                                                InvoiceNumber = Convert.ToString(r["InvoiceNumber"]),
                                                                WarehouseCode = Convert.ToString(r["ToWarehouseCode"]),
                                                                SalesChannelCode =Convert.ToString(r["FromSalesChannelCode"])
                                                            };
                                                if (query != null)
                                                {
                                                    if (query.Count() > 0)
                                                    {
                                                        counter = 1;
                                                        dtError = PageBase.LINQToDataTable(query);
                                                        foreach (DataRow dr in dtError.Rows)
                                                        {
                                                            DataRow drow = dtErrorTable.NewRow();
                                                            drow["InvoiceNumber"] = dr["InvoiceNumber"];
                                                            drow["WarehouseCode"] = dr["WarehouseCode"];
                                                            drow["SalesChannelCode"] = dr["SalesChannelCode"];
                                                            drow["ReasonForInvalid"] = "Can't Upload the Sales of other SalesChannel";
                                                            dtErrorTable.Rows.Add(drow);
                                                        }
                                                        dtErrorTable.AcceptChanges();
                                                    }
                                                }
                                            }
                                        }
                                    }



                                    var differentToPlantCode = from row in objDS.Tables[0].AsEnumerable()
                                                               group row by new
                                                               {
                                                                   InvoiceNumber = Convert.ToString(row["InvoiceNumber"]),
                                                                   WarehouseCode = Convert.ToString(row["FromSalesChannelCode"])

                                                               } into grp
                                                               where (from p in grp
                                                                      select Convert.ToString(p["ToWarehouseCode"])).Distinct().Count() > 1
                                                               orderby grp.Key.InvoiceNumber
                                                               select new
                                                               {
                                                                   Key = grp.Key,
                                                                   InvoiceNumber = grp.Key.InvoiceNumber,
                                                                   WarehouseCode = grp.Key.WarehouseCode,
                                                                   UniqueRows = (from p in grp
                                                                                 select Convert.ToString(p["ToWarehouseCode"])).Distinct().Count()
                                                               };
                                    //differentToPlantCode = differentToPlantCode.Where(r => ((string)r["Firstname"]).StartsWith("J")); 

                                    if (differentToPlantCode != null)
                                    {

                                        if (differentToPlantCode.Count() > 0)
                                        {
                                            dtError = new DataTable();
                                            counter = 1;
                                            dtError = PageBase.LINQToDataTable(differentToPlantCode);
                                            foreach (DataRow dr in dtError.Rows)
                                            {
                                                DataRow drow = dtErrorTable.NewRow();
                                                drow["InvoiceNumber"] = dr["InvoiceNumber"];
                                                drow["WarehouseCode"] = "";
                                                drow["SalesChannelCode"] = dr["WarehouseCode"];
                                                drow["ReasonForInvalid"] = "Same Invoice Number has different ToPlantCode";
                                                dtErrorTable.Rows.Add(drow);
                                            }
                                            dtErrorTable.AcceptChanges();
                                        }
                                    }
                                    //when a invoice number has different dates
                                    var differentDates = from row in objDS.Tables[0].AsEnumerable()
                                                         group row by new
                                                         {
                                                             InvoiceNumber = Convert.ToString(row["InvoiceNumber"]),
                                                             WarehouseCode = Convert.ToString(row["FromSalesChannelCode"])
                                                         } into grp
                                                         where (from p in grp
                                                                select Convert.ToString(p["InvoiceDate"])).Distinct().Count() > 1
                                                         orderby grp.Key.InvoiceNumber
                                                         select new
                                                         {
                                                             Key = grp.Key,
                                                             InvoiceNumber = grp.Key.InvoiceNumber,
                                                             WarehouseCode = grp.Key.WarehouseCode,
                                                             UniqueRows = (from p in grp
                                                                           select Convert.ToString(p["InvoiceDate"])).Distinct().Count()
                                                         };
                                    if (differentDates != null)
                                    {
                                        if (differentDates.Count() > 0)
                                        {
                                            counter = 1;
                                            dtError = PageBase.LINQToDataTable(differentDates);
                                            foreach (DataRow dr in dtError.Rows)
                                            {
                                                DataRow drow = dtErrorTable.NewRow();
                                                drow["InvoiceNumber"] = dr["InvoiceNumber"];
                                                drow["WarehouseCode"] = "";
                                                drow["SalesChannelCode"] = dr["WarehouseCode"];
                                                drow["ReasonForInvalid"] = "Same Invoice Number has different InvoiceDates";
                                                dtErrorTable.Rows.Add(drow);

                                            }
                                            dtErrorTable.AcceptChanges();
                                        }
                                    }
                                    //sending stock to the same saleschannelcode means (FromSalesChannelCode=TosalesChannelCode)
                                    var SameData = from r in objDS.Tables[0].AsEnumerable()
                                                   where ((Convert.ToString(r["FromSalesChannelCode"]) == (Convert.ToString(r["ToWarehouseCode"]))))
                                                   select new
                                                   {
                                                       InvoiceNumber = Convert.ToString(r["InvoiceNumber"]),
                                                       WarehouseCode = Convert.ToString(r["FromSalesChannelCode"]),
                                                       SalesChannelCode = Convert.ToString(r["ToWarehouseCode"])
                                                   };
                                    if (SameData != null)
                                    {
                                        if (SameData.Count() > 0)
                                        {
                                            counter = 1;
                                            dtError = PageBase.LINQToDataTable(SameData);
                                            foreach (DataRow dr in dtError.Rows)
                                            {
                                                DataRow drow = dtErrorTable.NewRow();
                                                drow["InvoiceNumber"] = dr["InvoiceNumber"];
                                                drow["WarehouseCode"] = dr["WarehouseCode"];
                                                drow["SalesChannelCode"] = dr["SalesChannelCode"];
                                                drow["ReasonForInvalid"] = "Sending stock to the same SalesChannel";
                                                dtErrorTable.Rows.Add(drow);
                                            }
                                            dtErrorTable.AcceptChanges();

                                        }
                                    }

                                    var qryNegativeQty = from row in objDS.Tables[0].AsEnumerable()
                                                         where ((Convert.ToInt32(row["Quantity"]) <= 0))
                                                         select new
                                                         {
                                                             InvoiceNumber = Convert.ToString(row["InvoiceNumber"]),
                                                             WarehouseCode = Convert.ToString(row["FromSalesChannelCode"]),
                                                             SalesChannelCode = Convert.ToString(row["ToWarehouseCode"])
                                                         };

                                    if (qryNegativeQty != null)
                                    {
                                        if (qryNegativeQty.Count() > 0)
                                        {
                                            counter = 1;
                                            dtError = PageBase.LINQToDataTable(qryNegativeQty);
                                            foreach (DataRow dr in dtError.Rows)
                                            {
                                                DataRow drow = dtErrorTable.NewRow();
                                                drow["InvoiceNumber"] = dr["InvoiceNumber"];
                                                drow["WarehouseCode"] = dr["WarehouseCode"];
                                                drow["SalesChannelCode"] = dr["SalesChannelCode"];
                                                drow["ReasonForInvalid"] = "Negative Quantity is Not Allowed";
                                                dtErrorTable.Rows.Add(drow);
                                            }
                                            dtErrorTable.AcceptChanges();

                                        }
                                    }

                                    if (counter > 0)
                                    {
                                        ucMsg.ShowInfo("Invalid Records");
                                        dsErrorProne.Merge(dtErrorTable);
                                        blnIsUpload = false;
                                    }
                                    else
                                    {
                                        objDS.Tables[0].Columns.Remove("ReasonForInvalid");
                                    }
                                }

                                if (objDS.Tables["DtDuplicateRecord"] != null && objDS.Tables["DtDuplicateRecord"].Rows.Count > 0)
                                {
                                    dsErrorProne.Merge(objDS.Tables["DtDuplicateRecord"]);
                                    blnIsUpload = false;
                                }
                                if (objDS.Tables["DtBlankData"] != null && objDS.Tables["DtBlankData"].Rows.Count > 0)
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
                dvUploadPreview.Visible = true;
                dtGRN.Columns.Add("QuantityNew", typeof(Int32), "Quantity");
                dtGRN.AcceptChanges();
                objSum = dtGRN.Compute("sum(QuantityNew)", "");

                Btnsave.Enabled = true;
                if (Convert.ToInt32(objSum) <= 0)
                {
                    Btnsave.Enabled = false;
                    ucMsg.ShowInfo("Please Insert right Quantity");
                    return;
                }

                var query = from row in dtGRN.AsEnumerable()
                            group row by new
                            {
                                SkuCode =Convert.ToString(row["SkuCode"])
                            } into grp
                            orderby grp.Key.SkuCode
                            select new
                            {
                                Key = grp.Key,
                                SkuCode = grp.Key.SkuCode,
                                Quantity = grp.Sum(r => r.Field<Int32>("QuantityNew"))
                            };
                int result = (from r in dtGRN.AsEnumerable() select r.Field<Int32>("QuantityNew")).Sum();
                lblTotal.Visible = true;
                lblTotal.Text = "Total Quantity: " + Convert.ToString(result);

                GridGRN.DataSource = query;
                GridGRN.DataBind();
                pnlGrid.Visible = true;
                Btnsave.Enabled = true;
                BtnCancel.Visible = true;
                updGrid.Update();
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }

    }


    protected void Btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }
            if (ViewState["TobeUploaded"] != null)
            {
                int intResult = 0;
                DataTable Tvp = new DataTable();
                DataSet DsExcelDetail = new DataSet();
                using (CommonData ObjCommom = new CommonData())
                {
                    Tvp = ObjCommom.GettvpTableReturnSalesSB();
                }
                OpenXMLExcel objexcel = new OpenXMLExcel();
                DsExcelDetail = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + ViewState["TobeUploaded"].ToString());

                foreach (DataRow dr in DsExcelDetail.Tables[0].Rows)
                {
                    DataRow drow = Tvp.NewRow();
                    drow["ReturnFromSalesChannelCode"] = dr["FromSalesChannelCode"].ToString();
                    drow["ReturnToSalesChannelCode"] = dr["ToWarehouseCode"].ToString();
                    drow["InvoiceNumber"] = dr["InvoiceNumber"].ToString();
                    drow["InvoiceDate"] = dr["InvoiceDate"];
                    drow["SKUCode"] = dr["SKUCode"].ToString();
                    drow["Quantity"] = Convert.ToInt32(dr["Quantity"]);
                    drow["Serial#1"] = dr["Serial#1"].ToString();
                    drow["BatchNo"] = dr["BatchNo"].ToString();
                    Tvp.Rows.Add(drow);
                }
                Tvp.AcceptChanges();
                using (SalesData objP1 = new SalesData())
                {
                    objP1.EntryType = EnumData.eEntryType.eUpload;
                    objP1.UserID = PageBase.UserId;
                    objP1.ReturnDate = Convert.ToDateTime(ucSalesReturnDate.Date);
                    objP1.StockBinType = Convert.ToInt16(ddlStockBinType.SelectedValue);
                    intResult = objP1.InsertPrimarySalesReturn1SB(Tvp);

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
        }
    }

    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        ClearForm();

    }

    void ClearForm()
    {
        dvUploadPreview.Visible = false;
        GridGRN.DataSource = null;
        GridGRN.DataBind();
        pnlGrid.Visible = false;
        ucMsg.Visible = false;
        lblTotal.Text = "";
        updGrid.Update();
    }

    protected void GridGRN_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            //GridGRN.PageIndex = e.NewPageIndex;
            //DataTable dt = new DataTable();
            //GridGRN.DataSource = (DataTable)ViewState["GRN"];
            //GridGRN.DataBind();
        }

        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }


    protected void DwnldReferenceCodeTemplate_Click(object sender, EventArgs e)
    {

        try
        {
            DataSet dsReferenceCode = new DataSet();
            DataSet Ds = new DataSet();
            using (SalesChannelData objSalesData = new SalesChannelData())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.ePrimary1Sales;
                objSalesData.SalesChannelID = PageBase.SalesChanelID;
                objSalesData.Brand = PageBase.Brand;
                dsReferenceCode = objSalesData.GetAllTemplateData();

                if (PageBase.SalesChanelID != 0)
                {
                    dsReferenceCode.Tables[0].DefaultView.RowFilter = "SalesChannelCode='" + PageBase.SalesChanelCode + "'";
                }

                Ds.Merge(dsReferenceCode.Tables[0].DefaultView.ToTable());
                if (PageBase.SalesChanelID != 0)
                {
                    dsReferenceCode.Tables[1].DefaultView.RowFilter = "ParentSalesChannelID=" + PageBase.SalesChanelID;
                }
                Ds.Merge(dsReferenceCode.Tables[1].DefaultView.ToTable());
                Ds.Tables[1].Columns.Remove("ParentSalesChannelID");

                Ds.Merge(dsReferenceCode.Tables[2]);

                if (PageBase.SalesChanelID != 0)
                {
                    dsReferenceCode.Tables[3].DefaultView.RowFilter = "ToSalesChannelID=" + PageBase.SalesChanelID;
                }
                Ds.Merge(dsReferenceCode.Tables[3].DefaultView.ToTable());
                Ds.Tables[3].Columns.Remove("ToSalesChannelID");


                if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "WarehouseCode", "SalesToCode", "SkuCodeList" };
                    ChangedExcelSheetNames(ref Ds, strExcelSheetName, Convert.ToInt16(EnumData.eTemplateCount.ePrimarysales1 + 1));
                    PageBase.ExportToExecl(Ds, FilenameToexport, EnumData.eTemplateCount.ePrimarysales1 + 1);
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


    private void ExportInExcel(DataSet DsError, string strFileName)
    {
        if (DsError != null && DsError.Tables.Count > 0)
        {
            PageBase.ExportToExeclV2(DsError, strFileName, DsError.Tables.Count);
        }
    }
    protected void rdModelList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdModelList.SelectedValue == "1")
            Response.Redirect("~/Transactions/SalesChannelSBReturn/Interface/ManageInterfacePrimarySalesReturn1-SB.aspx");
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
                dvStockBinType.Visible = true;
                ddlStockBinType.ValidationGroup = "EntryValidation";
                reqStockBinType.Enabled = true;
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
}
