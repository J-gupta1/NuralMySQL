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

public partial class Transactions_SalesChannelSBReturn_Upload_ManageUploadSecondarySalesReturnSB : PageBase
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
            ucMsg.ShowControl = false;
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

            ucSalesReturnDate.MaxRangeValue = dt;
            ucSalesReturnDate.RangeErrorMessage = "Invalid Date Range";
            if (!IsPostBack)
            {
                pnlGrid.Visible = false;
                FillStockBinType();
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
                            objValidateFile.ExcelFileNameInTable = "SecondarySalesReturn-SB";
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
                                    if (PageBase.SalesChanelID != 0)
                                    {
                                        var query = from r in objDS.Tables[0].AsEnumerable()
                                                    where ((Convert.ToString(r["ToSalesChannelCode"]) != PageBase.SalesChanelCode))
                                                    select new
                                                    {
                                                        InvoiceNumber = Convert.ToString(r["InvoiceNumber"]),
                                                        ToSalesChannelCode = Convert.ToString(r["ToSalesChannelCode"]),
                                                        FromRetailerCode = Convert.ToString(r["FromRetailerCode"])
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
                                                    drow["FromRetailerCode"] = dr["FromRetailerCode"];
                                                    drow["ToSalesChannelCode"] = dr["ToSalesChannelCode"];
                                                    drow["ReasonForInvalid"] = "Can't Upload the Sales of other SalesChannel";
                                                    dtErrorTable.Rows.Add(drow);
                                                }
                                                dtErrorTable.AcceptChanges();
                                            }
                                        }
                                    }
                                    var differentToPlantCode = from row in objDS.Tables[0].AsEnumerable()
                                                               group row by new
                                                               {
                                                                   InvoiceNumber = Convert.ToString(row["InvoiceNumber"]),
                                                                   SalesChannelCode = Convert.ToString(row["FromRetailerCode"])
                                                               } into grp
                                                               where (from p in grp
                                                                      select Convert.ToString(p["ToSalesChannelCode"])).Distinct().Count() > 1
                                                               orderby grp.Key.InvoiceNumber
                                                               select new
                                                               {
                                                                   Key = grp.Key,
                                                                   InvoiceNumber = grp.Key.InvoiceNumber,
                                                                   SalesChannelCode = grp.Key.SalesChannelCode,
                                                                   UniqueRows = (from p in grp
                                                                                 select Convert.ToString(p["FromRetailerCode"])).Distinct().Count()
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
                                                drow["FromRetailerCode"] = "";
                                                drow["ToSalesChannelCode"] = "";
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
                                                             SalesChannelcode = Convert.ToString(row["FromRetailerCode"])
                                                         } into grp
                                                         where (from p in grp
                                                                select Convert.ToString(p["InvoiceDate"])).Distinct().Count() > 1
                                                         orderby grp.Key.InvoiceNumber
                                                         select new
                                                         {
                                                             Key = grp.Key,
                                                             InvoiceNumber = grp.Key.InvoiceNumber,
                                                             SalesChannelcode = grp.Key.SalesChannelcode,
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
                                                drow["FromRetailerCode"] = dr["SalesChannelcode"];
                                                drow["ToSalesChannelCode"] = "";
                                                drow["ReasonForInvalid"] = "Same Invoice Number has different InvoiceDates";
                                                dtErrorTable.Rows.Add(drow);

                                            }
                                            dtErrorTable.AcceptChanges();
                                        }
                                    }
                                    //sending stock to the same saleschannelcode means (FromSalesChannelCode=TosalesChannelCode)
                                    var SameData = from r in objDS.Tables[0].AsEnumerable()
                                                   where ((Convert.ToString(r["ToSalesChannelCode"]) == (Convert.ToString(r["FromRetailerCode"]))))
                                                   select new
                                                   {
                                                       InvoiceNumber = Convert.ToString(r["InvoiceNumber"]),
                                                       FromSalesChannelCode = Convert.ToString(r["FromRetailerCode"]),
                                                       ToSalesChannelCode = Convert.ToString(r["ToSalesChannelCode"])
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
                                                drow["FromRetailerCode"] = dr["FromSalesChannelCode"];
                                                drow["ToSalesChannelCode"] = dr["ToSalesChannelCode"];
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
                                                             FromSalesChannelCode = Convert.ToString(row["FromRetailerCode"]),
                                                             ToSalesChannelCode = Convert.ToString(row["ToSalesChannelCode"])
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
                                                drow["FromRetailerCode"] = dr["FromSalesChannelCode"];
                                                drow["ToSalesChannelCode"] = dr["ToSalesChannelCode"];
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
                                SkuCode = Convert.ToString(row["SkuCode"])
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
            if (Convert.ToDateTime(ucSalesReturnDate.Date) > DateTime.Now)
            {
                ucMsg.ShowInfo("The return date cant be greater then the current date");
                return;
            }
            if (Convert.ToDateTime(DateTime.Now.Date).AddDays(PageBase.NumberofBackDaysAllowed) > Convert.ToDateTime(ucSalesReturnDate.Date))
            {
                ucMsg.ShowInfo("Invalid Date Range");
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
                    drow["ReturnFromSalesChannelCode"] = dr["FromRetailerCode"].ToString();
                    drow["ReturnToSalesChannelCode"] = dr["ToSalesChannelCode"].ToString();
                    drow["InvoiceNumber"] = dr["InvoiceNumber"].ToString();
                    drow["InvoiceDate"] = dr["InvoiceDate"];
                    drow["SKUCode"] = dr["SKUCode"].ToString();
                    drow["Quantity"] = Convert.ToInt32(dr["Quantity"]);
                    drow["Serial#1"] = dr["Serial#1"].ToString();
                    drow["BatchNo"] = dr["BatchNo"].ToString();
                    Tvp.Rows.Add(drow);
                }
                Tvp.AcceptChanges();
                using (SalesData objSecondary = new SalesData())
                {
                    objSecondary.EntryType = EnumData.eEntryType.eUpload;
                    objSecondary.UserID = PageBase.UserId;
                    objSecondary.ReturnDate = Convert.ToDateTime(ucSalesReturnDate.Date);
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
            DataSet dsTemplateCode = new DataSet();
            using (SalesChannelData objSalesData = new SalesChannelData())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.eSecondarySales;
                objSalesData.SalesChannelID = PageBase.SalesChanelID;
                objSalesData.Brand = PageBase.Brand;
                dsTemplateCode = objSalesData.GetAllTemplateData();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "SalesFromCode", "SalesToCode", "SkuCodeList" };
                    ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, Convert.ToInt16(EnumData.eTemplateCount.eSecondary + 1));
                    if (dsTemplateCode.Tables.Count > 3)
                        dsTemplateCode.Tables.RemoveAt(3);
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsTemplateCode, FilenameToexport, 3, strExcelSheetName);
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
            Response.Redirect("~/Transactions/SalesChannelSBReturn/Interface/ManageSecondarySalesReturnSB.aspx");
    }
    public DataTable GetBlankTableError()
    {
        DataTable Detail = new DataTable();
        Detail.Columns.Add("InvoiceNumber");
        Detail.Columns.Add("ToSalesChannelCode");
        Detail.Columns.Add("FromRetailerCode");
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
                ddlStockBinType.ValidationGroup = "Save";
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
            ObjSalesman.MapwithRetailer = 1;
            dsStockBinType = ObjSalesman.GetSalesmanAndStockBinTypeInfo();
            StrCol = new String[] { "StockBinTypeMasterID", "StockBinTypeDesc" };
            PageBase.DropdownBinding(ref ddlStockBinType, dsStockBinType.Tables[1], StrCol);

        };
    }
}
