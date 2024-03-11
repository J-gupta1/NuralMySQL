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
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Linq.Expressions;
using ZedService;
using System.IO; /* #CC07 Added */
//using System.Linq.Dynamic;
/*
 * 28 Jan 2015, Karam Chand Sharma, #CC01, Option open for full template upload and clear some control on redet button
 * 17 June 2016, Karam Chand Sharma, #CC02, Add tolower() function in comparing
 * 18 July 2016 , Karam Chand Sharma, #CC03, hide stock bin type dropdown
 * 21 July 2016, Karam Chand Sharma, #CC04, Download bin code as an reference
 * 17-Jan-2018, Sumit Maurya, #CC05, New namespace added and method ImportExcelFileV2 used (Done For ZedSalesV5).
 *04-July-2018,Vijay Kumar Prajapati,#CC06,Add NumberofBackdayssalesreturnallow.
 ** 19 Sep 2018,Vijay Kumar Prajapati,#CC07-Add two Parameter in Uploadprimarysale.
 */

public partial class Transactions_SalesChannelSBReturn_Upload_ManageUploadPrimarySalesReturn1_SB_BCP : PageBase
{

    object objSum;
    string strPrimaryReturnSessionName = "PrimaryReturnUploadSession";
    string strOriginalFileName = string.Empty;/*#CC07 Added*/
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    DateTime dt = new DateTime();
    UploadFile UploadFile = new UploadFile();
    DataSet dsErrorProne = new DataSet();

    string ErrorColumnName = "Error";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            dt = System.DateTime.Now.Date;
            StockBinTypeTobeAskedOrNot();
            if (PageBase.SalesChanelID != 0)
            {
                /*#CC06 Added Started*/
                if (Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening) >= Convert.ToDateTime(dt.AddDays(PageBase.BackDaysAllowedForSaleReturn)))
                {

                    ucSalesReturnDate.MinRangeValue = Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-PageBase.BackDaysAllowedBeforeOpening);
                    lblInfo.Text = Resources.Messages.ValidationSalesReturnDays.ToString().Replace("Number", (Convert.ToDateTime(dt.ToShortDateString()) - Convert.ToDateTime(Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedForSaleReturn).ToShortDateString())).TotalDays.ToString());
                }
                else
                {
                    ucSalesReturnDate.MinRangeValue = dt.AddDays(PageBase.BackDaysAllowedForSaleReturn);
                    lblInfo.Text = Resources.Messages.ValidationSalesReturnDays.ToString().Replace("Number", PageBase.BackDaysAllowedForSaleReturn.ToString().Replace("-", ""));
                }
                /*#CC06 Added Started*/
                /*#CC06 Commented Started
                if (Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening) >= Convert.ToDateTime(dt.AddDays(PageBase.NumberofBackDaysAllowed)))
                {

                    ucSalesReturnDate.MinRangeValue = Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-PageBase.BackDaysAllowedBeforeOpening);
                    lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", (Convert.ToDateTime(dt.ToShortDateString()) - Convert.ToDateTime(Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening).ToShortDateString())).TotalDays.ToString());
                }
                else
                {
                    ucSalesReturnDate.MinRangeValue = dt.AddDays(PageBase.NumberofBackDaysAllowed);
                    lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
                }*#CC06 Commented End*/
            }
            ucMsg.ShowControl = false;
            ucSalesReturnDate.MaxRangeValue = dt;
            ucSalesReturnDate.RangeErrorMessage = "Invalid Date Range";
            if (!IsPostBack)
            {
                FillStockBinType();
                pnlGrid.Visible = false;
                ddlTemplate_SelectedIndexChanged(null, null);
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

            string strTemplate = ddlTemplate.SelectedValue;
            DataTable dtErrorTable = GetBlankTableError();
            DataTable dtError = new DataTable();
            HttpContext.Current.Session["PkeyColumns"] = null;
            string strKey = string.Empty;
            ClearForm();
            hlnkInvalid.Visible = false;
            Int16 Upload = 0;
            Upload = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            strOriginalFileName = FileUpload1.FileName;/*#CC07 Added*/
            ViewState["TobeUploaded"] = strUploadedFileName;
            if (Upload == 1)
            {
                DataTable dtExcelData = new DataTable();
                OpenXMLExcel objexcel = new OpenXMLExcel();
                /* DataSet DsExcel = objexcel.ImportExcelFile(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName); #CC05 Commented */
                DataSet DsExcel = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);/* #CC05 Added */
                if (DsExcel != null && DsExcel.Tables.Count > 0 && DsExcel.Tables[0].Rows.Count > 0)
                {

                    if (DsExcel.Tables[0].Rows.Count > Convert.ToInt32(PageBase.ValidExcelRows))
                        ucMsg.ShowInfo("Limit Crossed");
                    else
                    {
                        ValidateUploadFile objValidateFile = new ValidateUploadFile();
                        {
                            string varToSalesChannelCode = string.Empty;    /*#CC01 ADDED Replace with "ToSalesChannelCode"*/
                           
                            objValidateFile.UploadedFileName = strUploadedFileName;
                            if (strTemplate == "1")
                            {
                                objValidateFile.ExcelFileNameInTable = "PrimarySalesReturn1-SB";
                                varToSalesChannelCode = "ToWarehouseCode";/*#CC01 ADDED*/
                            }
                            else
                            {
                                objValidateFile.ExcelFileNameInTable = "GenericSalesReturn";
                                varToSalesChannelCode = "ToSalesChannelCode";/*#CC01 ADDED*/
                            }
                            objValidateFile.ValidateFileBCPV5(out dtExcelData, strUploadedFileName);
                            if (objValidateFile.Message != null && objValidateFile.Message.Trim() != "")
                                ucMsg.ShowInfo(objValidateFile.Message);
                            else
                            {
                                ucMsg.Visible = false;
                                bool blnIsUpload = true;

                                if (dtExcelData != null && dtExcelData.Rows.Count > 0)
                                {
                                    if (dtExcelData.Columns.Contains("id"))
                                        dtExcelData.Columns.Remove("id");
                                    if (dtExcelData.Columns.Contains(ErrorColumnName))
                                    {
                                        hlnkInvalid.Visible = true;
                                        dsErrorProne.Merge(dtExcelData);
                                        hlnkInvalid.Text = "Invalid Data";

                                        blnIsUpload = false;
                                    }

                                }
                                if (!dtExcelData.Columns.Contains(ErrorColumnName))
                                {
                                    int counter = 0;
                                    if (!dtExcelData.Columns.Contains(ErrorColumnName))
                                        dtExcelData.Columns.Add(new DataColumn(ErrorColumnName));
                                    if (Session["PRIMARYINGRNMODE"] != null)
                                    {
                                        if (Convert.ToInt32(Session["PRIMARYINGRNMODE"]) == 0)
                                        {
                                            if (PageBase.SalesChanelID != 0)
                                            {
                                                var query = from r in dtExcelData.AsEnumerable()
                                                            /*#CC02 START COMMENTED where ((Convert.ToString(r[varToSalesChannelCode]) != PageBase.SalesChanelCode)) #CC02 END COMMENTED  */
                                                            where ((Convert.ToString(r[varToSalesChannelCode]).ToLower() != PageBase.SalesChanelCode.ToLower()))/*#CC02 ADDED*/
                                                            select new
                                                            {
                                                                InvoiceNumber = strTemplate == "1" ? Convert.ToString(r["InvoiceNumber"]) : "Not Required",
                                                                WarehouseCode = Convert.ToString(r[varToSalesChannelCode]),
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
                                                            drow[ErrorColumnName] = "Can't Upload the Sales of other SalesChannel";
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
                                                var query = from r in dtExcelData.AsEnumerable()
                                                            where ((Convert.ToString(r["FromSalesChannelCode"]) != PageBase.SalesChanelCode))
                                                            select new
                                                            {
                                                                InvoiceNumber = strTemplate == "1" ? Convert.ToString(r["InvoiceNumber"]) : "Not Required",
                                                                WarehouseCode = Convert.ToString(r[varToSalesChannelCode]),
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
                                                            drow[ErrorColumnName] = "Can't Upload the Sales of other SalesChannel";
                                                            dtErrorTable.Rows.Add(drow);
                                                        }
                                                        dtErrorTable.AcceptChanges();
                                                    }
                                                }
                                            }
                                        }
                                    }


                                    if (strTemplate != "2")
                                    {
                                        var differentToPlantCode = from row in dtExcelData.AsEnumerable()
                                                                   group row by new
                                                                   {
                                                                       InvoiceNumber = Convert.ToString(row["InvoiceNumber"]),
                                                                       WarehouseCode = Convert.ToString(row["FromSalesChannelCode"])

                                                                   } into grp
                                                                   where (from p in grp
                                                                          select Convert.ToString(p[varToSalesChannelCode])).Distinct().Count() > 1
                                                                   orderby grp.Key.InvoiceNumber
                                                                   select new
                                                                   {
                                                                       Key = grp.Key,
                                                                       InvoiceNumber = grp.Key.InvoiceNumber,
                                                                       WarehouseCode = grp.Key.WarehouseCode,
                                                                       UniqueRows = (from p in grp
                                                                                     select Convert.ToString(p[varToSalesChannelCode])).Distinct().Count()
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
                                                    drow[ErrorColumnName] = "Same Invoice Number has different ToPlantCode";
                                                    dtErrorTable.Rows.Add(drow);
                                                }
                                                dtErrorTable.AcceptChanges();
                                            }
                                        }

                                        //when a invoice number has different dates
                                        var differentDates = from row in dtExcelData.AsEnumerable()
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
                                                    drow[ErrorColumnName] = "Same Invoice Number has different InvoiceDates";
                                                    dtErrorTable.Rows.Add(drow);

                                                }
                                                dtErrorTable.AcceptChanges();
                                            }
                                        }



                                        var qryNegativeQty = from row in dtExcelData.AsEnumerable()
                                                             where ((Convert.ToInt32(row["Quantity"]) <= 0))
                                                             select new
                                                             {
                                                                 InvoiceNumber = Convert.ToString(row["InvoiceNumber"]),
                                                                 WarehouseCode = Convert.ToString(row["FromSalesChannelCode"]),
                                                                 SalesChannelCode = Convert.ToString(row[varToSalesChannelCode])
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
                                                    drow[ErrorColumnName] = "Negative Quantity is Not Allowed";
                                                    dtErrorTable.Rows.Add(drow);
                                                }
                                                dtErrorTable.AcceptChanges();

                                            }
                                        }

                                    }
                                    //sending stock to the same saleschannelcode means (FromSalesChannelCode=TosalesChannelCode)
                                    var SameData = from r in dtExcelData.AsEnumerable()
                                                   where ((Convert.ToString(r["FromSalesChannelCode"]) == (Convert.ToString(r[varToSalesChannelCode]))))
                                                   select new
                                                   {
                                                       InvoiceNumber = strTemplate == "1" ? Convert.ToString(r["InvoiceNumber"]) : "Not Required",
                                                       WarehouseCode = Convert.ToString(r["FromSalesChannelCode"]),
                                                       SalesChannelCode = Convert.ToString(r[varToSalesChannelCode])
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
                                                drow[ErrorColumnName] = "Sending stock to the same SalesChannel";
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
                                        dtExcelData.Columns.Remove(ErrorColumnName);
                                    }
                                }

                                //if (objDS.Tables["DtDuplicateRecord"] != null && objDS.Tables["DtDuplicateRecord"].Rows.Count > 0)
                                //{
                                //    dsErrorProne.Merge(objDS.Tables["DtDuplicateRecord"]);
                                //    blnIsUpload = false;
                                //}
                                //if (objDS.Tables["DtBlankData"] != null && objDS.Tables["DtBlankData"].Rows.Count > 0)
                                //{
                                //    dsErrorProne.Merge(objDS.Tables["DtBlankData"]);
                                //    blnIsUpload = false;
                                //}
                                if (blnIsUpload)
                                {
                                    if (dtExcelData != null && dtExcelData.Rows.Count > 0)
                                    {
                                        InsertData(dtExcelData, strTemplate);
                                        ddlTemplate.Enabled = false;
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



    private void InsertData(DataTable dtGRN, string strTemplateType)
    {
        try
        {
            int result = 0;
            if (dtGRN.Rows.Count > 0)
            {
                dvUploadPreview.Visible = true;
                Btnsave.Enabled = true;
                if (strTemplateType != "2")
                {
                    dtGRN.Columns.Add("QuantityNew", typeof(Int32), "Quantity");
                    dtGRN.AcceptChanges();
                    objSum = dtGRN.Compute("sum(QuantityNew)", "");
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
                                    //SkuCode = grp.Key.SkuCode,
                                    SkuCode = (grp.Key.SkuCode.Trim() == string.Empty | grp.Key.SkuCode.Trim() == null) ? Resources.Messages.SkuNotMentioned : grp.Key.SkuCode.Trim(),
                                    Quantity = grp.Sum(r => r.Field<Int32>("QuantityNew"))
                                };

                    result = (from r in dtGRN.AsEnumerable() select r.Field<Int32>("QuantityNew")).Sum();

                    lblTotal.Visible = true;
                    lblTotal.Text = "Total Quantity: " + Convert.ToString(result);
                    GridGRN.DataSource = query;
                    GridGRN.DataBind();

                }
                else
                {
                    dvGrid.Visible = false;
                    result = dtGRN.AsEnumerable().Count();
                    lblTotal.Visible = true;
                    lblTotal.Text = "Total Quantity: " + Convert.ToString(result);

                }

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
            if (Convert.ToDateTime(ucSalesReturnDate.Date) > System.DateTime.Now)
            {
                ucMsg.ShowWarning("Future Date is not allowed");
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
                /* DsExcelDetail = objexcel.ImportExcelFile(PageBase.strExcelPhysicalUploadPathSB + ViewState["TobeUploaded"].ToString()); #CC05 Commented */
                DsExcelDetail = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + ViewState["TobeUploaded"].ToString()); /* #CC05 Added */
                //foreach (DataRow dr in DsExcelDetail.Tables[0].Rows)
                //{
                //    DataRow drow = Tvp.NewRow();
                //    drow["ReturnFromSalesChannelCode"] = dr["FromSalesChannelCode"].ToString();
                //    drow["ReturnToSalesChannelCode"] = dr["ToWarehouseCode"].ToString();
                //    drow["InvoiceNumber"] = dr["InvoiceNumber"].ToString();
                //    drow["InvoiceDate"] = dr["InvoiceDate"];
                //    drow["SKUCode"] = dr["SKUCode"].ToString();
                //    drow["Quantity"] = Convert.ToInt32(dr["Quantity"]);
                //    drow["Serial#1"] = dr["Serial#1"].ToString();
                //    drow["BatchNo"] = dr["BatchNo"].ToString();
                //    Tvp.Rows.Add(drow);
                //}
                //Tvp.AcceptChanges();

                string guid = Guid.NewGuid().ToString();
                ViewState[strPrimaryReturnSessionName] = guid;
                /*#CC01 START ADDED*/
                if (ddlTemplate.SelectedValue != "1")
                {
                    DsExcelDetail.Tables[0].Columns.Add(AddColumn(null, "InvoiceNumber", typeof(System.String)));
                    DsExcelDetail.Tables[0].Columns.Add(AddColumn(null, "InvoiceDate", typeof(System.String)));
                    DsExcelDetail.Tables[0].Columns.Add(AddColumn(null, "SKUCode", typeof(System.String)));
                    DsExcelDetail.Tables[0].Columns.Add(AddColumn("1", "Quantity", typeof(System.Int32)));
                    DsExcelDetail.Tables[0].Columns.Add(AddColumn(null, "BatchNo", typeof(System.String)));
                }
                else
                {
                    DsExcelDetail.Tables[0].Columns[1].ColumnName = "ToSalesChannelCode";
                }
                /*#CC01 START END*/

                /*#CC01 Comented DsExcelDetail.Tables[0].Columns.Add(AddColumn(null, "InvoiceNumber", typeof(System.String)));
                DsExcelDetail.Tables[0].Columns.Add(AddColumn(null, "InvoiceDate", typeof(System.String)));
                DsExcelDetail.Tables[0].Columns.Add(AddColumn(null, "SKUCode", typeof(System.String)));
                DsExcelDetail.Tables[0].Columns.Add(AddColumn("1", "Quantity", typeof(System.Int32)));
                DsExcelDetail.Tables[0].Columns.Add(AddColumn(null, "BatchNo", typeof(System.String)));
                */
                DsExcelDetail.Tables[0].Columns.Add(AddColumn(guid, "TransUploadSession", typeof(System.String)));
                DsExcelDetail.Tables[0].Columns.Add(AddColumn("3", "TransType", typeof(System.Int32)));




                DsExcelDetail.Tables[0].AcceptChanges();
                if (DsExcelDetail.Tables[0].Rows.Count > 0)
                {
                    if (!BulkCopyUpload(DsExcelDetail.Tables[0]))
                    {
                        ucMsg.ShowError("Error Occured While transferring the data to the server");
                        return;
                    }

                }

                using (SalesData objP1 = new SalesData())
                {
                    objP1.EntryType = EnumData.eEntryType.eUpload;
                    objP1.UserID = PageBase.UserId;
                    objP1.ReturnDate = Convert.ToDateTime(ucSalesReturnDate.Date);
                    objP1.StockBinType = Convert.ToInt16(ddlStockBinType.SelectedValue);
                    objP1.TransUploadSession = Convert.ToString(ViewState[strPrimaryReturnSessionName]);
                    objP1.TemplateType = ddlTemplate.SelectedValue;
                    objP1.ComingFrom = 1;
                    objP1.strOriginalFileName = strOriginalFileName;
                    objP1.strUploadedFileName = strUploadedFileName;
                    intResult = objP1.InsertPrimarySalesReturn1SBBCP();

                    if (objP1.ErrorDetailXML != null && objP1.ErrorDetailXML != string.Empty)
                    {
                        /*#CC07 Added*/
                        hlnkInvalid.Visible = true;
                        string strFileName = "InvalidData" + DateTime.Now.Ticks;
                        StringReader theReader = new StringReader(objP1.ErrorDetailXML);
                        dsErrorProne.ReadXml(theReader);
                        ExportInExcel(dsErrorProne, strFileName);
                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                        hlnkInvalid.Text = "Invalid Data";
                        /*#CC07 Added End*/
                        //ucMsg.XmlErrorSource = objP1.ErrorDetailXML; /*#CC07 Commented*/
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
        /*#CC01 START ADDED*/
        ddlTemplate.Enabled = true;
        ddlStockBinType.SelectedIndex = 0;
        ddlTemplate.SelectedIndex = 0;
        ddlTemplate_SelectedIndexChanged(null, null);
        ucSalesReturnDate.TextBoxDate.Text = "";
        hlnkInvalid.Visible = false;
        /*#CC01 START END*/
    }

    void ClearForm()
    {
        dvUploadPreview.Visible = false;
        GridGRN.DataSource = null;
        GridGRN.DataBind();
        pnlGrid.Visible = false;
        ucMsg.Visible = false;
        lblTotal.Text = "";
        //ddlTemplate.Enabled = true;        
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
        Detail.Columns.Add(ErrorColumnName);
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
                /*#CC03 START ADDED dvStockBinType.Visible = true;
                ddlStockBinType.ValidationGroup = "EntryValidation";
                reqStockBinType.Enabled = true; #CC03 END ADDED */

                dvStockBinType.Visible = false;/*#CC03 ADDED*/
                ddlStockBinType.ValidationGroup = "";/*#CC03 ADDED*/
                reqStockBinType.Enabled = false;/*#CC03 ADDED*/
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

    DataColumn AddColumn(string columnValue, string ColumnName, Type ColumnType)
    {
        DataColumn dcSession = new DataColumn();
        dcSession.ColumnName = ColumnName;

        if (ColumnType == typeof(int))
        {
            dcSession.DataType = typeof(System.Int32);
            dcSession.DefaultValue = Convert.ToInt32(columnValue);
        }
        if (ColumnType == typeof(System.String))
        {
            dcSession.DataType = typeof(System.String);
            dcSession.DefaultValue = columnValue;
        }
        return dcSession;
    }
    public bool BulkCopyUpload(DataTable dtTempTable)
    {
        try
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "TransactionUploadBulk";
                bulkCopy.ColumnMappings.Add("FromSalesChannelCode", "ToCode");
                bulkCopy.ColumnMappings.Add("ToSalesChannelCode", "FromCode");
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
    /*#CC01 START ADDED*/
    protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTemplate.SelectedIndex == 0)
        {
            lnkFullTemplate.Visible = false;
            lnkTemplateSystemExist.Visible = false;
        }
        else if (ddlTemplate.SelectedValue == "1")
        {
            lnkFullTemplate.Visible = true;
            lnkTemplateSystemExist.Visible = false;
        }
        else if (ddlTemplate.SelectedValue == "2")
        {
            lnkFullTemplate.Visible = false;
            lnkTemplateSystemExist.Visible = true;
        }
    }
    /*#CC01 START END*/
    /*#CC04 START ADDED*/
    protected void DwnldBindCode_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsReferenceCode = new DataSet();
            using (SalesChannelData objSalesData = new SalesChannelData())
            {

                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.ePrice;

                dsReferenceCode = objSalesData.GetBinCode();
                if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
                {

                    string FilenameToexport = "Reference Code List";
                    PageBase.ExportToExecl(dsReferenceCode, FilenameToexport, EnumData.eTemplateCount.ePrimarysales1);
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
        }
        /*#CC04 END ADDED*/

    }
}





