﻿using System;
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
using ZedService;
using System.IO;
/*
 * 18 July 2016 , Karam Chand Sharma, #CC01, Save Bin code in BCP
 * 21 July 2016, Karam Chand Sharma, #CC02, Download bin code as an reference
 * 28-Apr-2017, Sumit Maurya, #CC03, Parameter value supplied to validate data.
 */
public partial class Transactions_SalesChannelSB_Upload_ManageUploadIntermediarySales_SB_BCP :  PageBase
{
    DataTable dtNew = new DataTable();
    object objSum;
    int counter = 0;
    string strIntermediarySessionName = "IntermediaryUploadSession";
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    UploadFile UploadFile = new UploadFile();
    List<String> lstDuplicate = new List<String>();
    DataSet dsErrorProne = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //PrcPrimarySales1UploadSB
            ucMsg.ShowControl = false;
            if (!IsPostBack)
            {
                if (PageBase.SalesChanelID != 0)
                {
                    if (Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening) >= Convert.ToDateTime(DateTime.Now.Date.AddDays(PageBase.NumberofBackDaysAllowed)))
                        lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", (Convert.ToDateTime(DateTime.Now.Date.ToShortDateString()) - Convert.ToDateTime(Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening).ToShortDateString())).TotalDays.ToString());
                    else
                        lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
                }
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
                            SortedList objSL = new SortedList();
                            SortedList objSLCorrData = new SortedList();
                            objValidateFile.UploadedFileName = strUploadedFileName;
                            objValidateFile.ExcelFileNameInTable = "IntermediarySales-SB";
                            // objValidateFile.RootFolerPath = RootPath;
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
                                        //string strFileName = "invalidData" + DateTime.Now.Ticks;
                                        // ExportInExcel(objDS.Tables["DtExcelSheet"], strFileName);
                                        // hlnkInvalid.NavigateUrl = strExcelVirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
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
                                                    where ((Convert.ToString(r["FromSalesChannelCode"].ToString().Trim()) != PageBase.SalesChanelCode))
                                                    select new
                                                    {
                                                        InvoiceNumber = Convert.ToString(r["InvoiceNumber"]),
                                                        FromSalesChannelCode = Convert.ToString(r["FromSalesChannelCode"]),
                                                        ToSalesChannelCode = Convert.ToString(r["ToSalesChannelCode"])
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
                                                    drow["WarehouseCode"] = dr["FromSalesChannelCode"];
                                                    drow["SalesChannelCode"] = dr["ToSalesChannelCode"];
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
                                                                   FromSalesChannelCode = Convert.ToString(row["FromSalesChannelCode"])

                                                               } into grp
                                                               where (from p in grp
                                                                      select Convert.ToString(p["ToSalesChannelCode"])).Distinct().Count() > 1
                                                               orderby grp.Key.InvoiceNumber
                                                               select new
                                                               {
                                                                   Key = grp.Key,
                                                                   InvoiceNumber = grp.Key.InvoiceNumber,
                                                                   FromSalesChannelCode = grp.Key.FromSalesChannelCode,
                                                                   UniqueRows = (from p in grp
                                                                                 select Convert.ToString(p["ToSalesChannelCode"])).Distinct().Count()
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
                                                drow["WarehouseCode"] = dr["FromSalesChannelCode"];
                                                drow["SalesChannelCode"] = "";
                                                drow["ReasonForInvalid"] = "Same Invoice Number has different ToSalesChannelCode";
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
                                                             FromSalesChannelCode = Convert.ToString(row["FromSalesChannelCode"])
                                                         } into grp
                                                         where (from p in grp
                                                                select Convert.ToString(p["InvoiceDate"])).Distinct().Count() > 1
                                                         orderby grp.Key.InvoiceNumber
                                                         select new
                                                         {
                                                             Key = grp.Key,
                                                             InvoiceNumber = grp.Key.InvoiceNumber,
                                                             FromSalesChannelCode = grp.Key.FromSalesChannelCode,
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
                                                drow["WarehouseCode"] = dr["FromSalesChannelCode"];
                                                drow["SalesChannelCode"] = "";
                                                drow["ReasonForInvalid"] = "Same Invoice Number has different InvoiceDates";
                                                dtErrorTable.Rows.Add(drow);

                                            }
                                            dtErrorTable.AcceptChanges();
                                        }
                                    }
                                    //sending stock to the same saleschannelcode means (FromSalesChannelCode=TosalesChannelCode)
                                    var SameData = from r in objDS.Tables[0].AsEnumerable()
                                                   where ((Convert.ToString(r["FromSalesChannelCode"]) == (Convert.ToString(r["ToSalesChannelCode"]))))
                                                   select new
                                                   {
                                                       InvoiceNumber = Convert.ToString(r["InvoiceNumber"]),
                                                       FromSalesChannelCode = Convert.ToString(r["FromSalesChannelCode"]),
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
                                                drow["WarehouseCode"] = dr["FromSalesChannelCode"];
                                                drow["SalesChannelCode"] = dr["ToSalesChannelCode"];
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
                                                             FromSalesChannelCode = Convert.ToString(row["FromSalesChannelCode"]),
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
                                                drow["WarehouseCode"] = dr["FromSalesChannelCode"];
                                                drow["SalesChannelCode"] = dr["ToSalesChannelCode"];
                                                drow["ReasonForInvalid"] = "Negative Quantity is Not Allowed";
                                                dtErrorTable.Rows.Add(drow);
                                            }
                                            dtErrorTable.AcceptChanges();

                                        }
                                    }
                                    //DataView dv = new DataView(objDS.Tables[0]);
                                    //DataTable dtSerial = objDS.Tables[0].Clone();
                                    //dv.RowFilter = "[Serial#1] <>''";
                                    //foreach (DataRowView dvr in dv)
                                    //{
                                    //    dtSerial.ImportRow(dvr.Row);
                                    //}
                                    //dtSerial.AcceptChanges();

                                    //var duplicates = from r in dtSerial.AsEnumerable()
                                    //                 group r by r.Field<String>("Serial#1") into gp
                                    //                 where gp.Count() > 1
                                    //                 select new
                                    //                 {
                                    //                     serial = gp.Key
                                    //                 };
                                    //foreach (var g in duplicates)
                                    //{
                                    //    lstDuplicate.Add(g.serial);
                                    //}
                                    //Pankaj
                                    //for (int i = 0; i < objDS.Tables[0].Rows.Count; i++)
                                    //{
                                    //    if (PageBase.SalesChanelID != 0)
                                    //    {
                                    //        string srtwe = "FromSalesChannelCode <> '" + PageBase.SalesChanelCode + "'";
                                    //        DataRow[] dr1 = objDS.Tables[0].Select(srtwe);
                                    //        if (dr1.Length > 0)
                                    //        {
                                    //            counter = counter + 1;
                                    //            if (objDS.Tables[0].Rows[i]["ReasonForInvalid"] == "" && objDS.Tables[0].Rows[i]["ReasonForInvalid"] == string.Empty)
                                    //            {
                                    //                objDS.Tables[0].Rows[i]["ReasonForInvalid"] = "Can't Upload the Sales of other warehouse";
                                    //            }
                                    //            else
                                    //                objDS.Tables[0].Rows[i]["ReasonForInvalid"] += ";Can't Upload the Sales of other warehouse";
                                    //        }
                                    //    }

                                    //    if (objDS.Tables[0].Rows[i]["ToSalesChannelCode"] != DBNull.Value)
                                    //    {
                                    //        string strWhere = "ToSalesChannelCode<>'" + objDS.Tables[0].Rows[i]["ToSalesChannelCode"].ToString().Trim() + "' and InvoiceNumber='" + objDS.Tables[0].Rows[i]["InvoiceNumber"].ToString().Trim() + "'";
                                    //        DataRow[] dr = objDS.Tables[0].Select(strWhere);

                                    //        if (dr.Length > 0)
                                    //        {
                                    //            counter = counter + 1;
                                    //            if (objDS.Tables[0].Rows[i]["ReasonForInvalid"] == "" && objDS.Tables[0].Rows[i]["ReasonForInvalid"] == string.Empty)
                                    //            {
                                    //                objDS.Tables[0].Rows[i]["ReasonForInvalid"] = "Same Invoice Number has different ToPlantCode";
                                    //            }
                                    //            else
                                    //                objDS.Tables[0].Rows[i]["ReasonForInvalid"] += ";Same Invoice Number has different ToPlantCode";
                                    //        }
                                    //    }

                                    //    string strWhere1 = "FromSalesChannelCode<>'" + objDS.Tables[0].Rows[i]["FromSalesChannelCode"].ToString().Trim() + "' and InvoiceNumber='" + objDS.Tables[0].Rows[i]["InvoiceNumber"].ToString().Trim() + "'";

                                    //    if (objDS.Tables[0].Rows[i]["FromSalesChannelCode"] != DBNull.Value)
                                    //    {

                                    //        DataRow[] dr1 = objDS.Tables[0].Select(strWhere1);
                                    //        if (dr1.Length > 0)
                                    //        {
                                    //            counter = counter + 1;
                                    //            if (objDS.Tables[0].Rows[i]["ReasonForInvalid"] == DBNull.Value && objDS.Tables[0].Rows[i]["ReasonForInvalid"] == string.Empty)
                                    //            {
                                    //                objDS.Tables[0].Rows[i]["ReasonForInvalid"] = "Same Invoice Number has different FromPlantCode";
                                    //            }
                                    //            else
                                    //            {
                                    //                objDS.Tables[0].Rows[i]["ReasonForInvalid"] += ";Same Invoice Number has different FromPlantCode";
                                    //            }

                                    //        }

                                    //    }

                                    //    string strWhere3 = "FromSalesChannelCode<>'" + objDS.Tables[0].Rows[i]["FromSalesChannelCode"].ToString().Trim() + "' and ToSalesChannelCode<>'" + objDS.Tables[0].Rows[i]["ToSalesChannelCode"].ToString().Trim() + "' and InvoiceNumber='" + objDS.Tables[0].Rows[i]["InvoiceNumber"].ToString().Trim() + "'";
                                    //    DataRow[] dr3 = objDS.Tables[0].Select(strWhere3);
                                    //    if (dr3.Length > 0)
                                    //    {
                                    //        counter = counter + 1;
                                    //        if (objDS.Tables[0].Rows[i]["ReasonForInvalid"] == DBNull.Value && objDS.Tables[0].Rows[i]["ReasonForInvalid"] == string.Empty)
                                    //        {
                                    //            objDS.Tables[0].Rows[i]["ReasonForInvalid"] = "Same Invoice Number has different FromPlantCode To ToPlantCode.<br/>";
                                    //        }
                                    //        else
                                    //            objDS.Tables[0].Rows[i]["ReasonForInvalid"] = ";Same Invoice Number has different FromPlantCode To ToPlantCode.<br/>";
                                    //    }

                                    //    if (lstDuplicate.Count > 0 && lstDuplicate != null)
                                    //    {
                                    //        foreach (var g in duplicates)
                                    //        {
                                    //            if (objDS.Tables[0].Rows[i]["Serial#1"].ToString() == g.serial.ToString())
                                    //            {
                                    //                counter = counter + 1;
                                    //                if (objDS.Tables[0].Rows[i]["ReasonForInvalid"] == DBNull.Value && objDS.Tables[0].Rows[i]["ReasonForInvalid"].ToString() == string.Empty)
                                    //                {
                                    //                    objDS.Tables[0].Rows[i]["ReasonForInvalid"] = g.serial.ToString() + " Duplicate SerialNumber1 exists.";
                                    //                }
                                    //                else
                                    //                    objDS.Tables[0].Rows[i]["ReasonForInvalid"] += ";" + g.serial.ToString() + " Duplicate SerialNumber1 exists.";
                                    //            }
                                    //        }
                                    //    }
                                    //    string strWhere4 = "InvoiceNumber='" + objDS.Tables[0].Rows[i]["InvoiceNumber"].ToString().Trim() + "'and InvoiceDate <>'" + objDS.Tables[0].Rows[i]["InvoiceDate"].ToString().Trim() + "'";
                                    //    if (objDS.Tables[0].Rows[i]["InvoiceNumber"] != DBNull.Value)
                                    //    {
                                    //        DataRow[] dr = objDS.Tables[0].Select(strWhere4);
                                    //        if (dr.Length > 0)
                                    //        {
                                    //            counter = counter + 1;
                                    //            if (objDS.Tables[0].Rows[i]["ReasonForInvalid"] == DBNull.Value && objDS.Tables[0].Rows[i]["ReasonForInvalid"] == string.Empty)
                                    //            {
                                    //                objDS.Tables[0].Rows[i]["ReasonForInvalid"] = "Same invoice no with multiple dates!<br/>";
                                    //            }
                                    //            else
                                    //                objDS.Tables[0].Rows[i]["ReasonForInvalid"] += ";Same invoice no with multiple dates!<br/>";
                                    //        }
                                    //        TimeSpan ts = Convert.ToDateTime(objDS.Tables[0].Rows[i]["invoiceDate"]).Subtract(System.DateTime.Now.Date);
                                    //        if (ts.Days > 0)
                                    //        {
                                    //            counter = counter + 1;
                                    //            if (objDS.Tables[0].Rows[i]["ReasonForInvalid"] != DBNull.Value)
                                    //                objDS.Tables[0].Rows[i]["ReasonForInvalid"] = " Invoice date should not be greater than current date!<br/>";
                                    //            else
                                    //                objDS.Tables[0].Rows[i]["ReasonForInvalid"] += ";Invoice date should not be greater than current date!<br/>";
                                    //        }
                                    //    }
                                    //    string strWhere5 = "'" + objDS.Tables[0].Rows[i]["ToSalesChannelCode"].ToString().Trim() + "'='" + objDS.Tables[0].Rows[i]["FromSalesChannelCode"].ToString().Trim() + "'";
                                    //    if (objDS.Tables[0].Rows[i]["ToSalesChannelCode"] != DBNull.Value && objDS.Tables[0].Rows[i]["FromSalesChannelCode"] != DBNull.Value)
                                    //    {
                                    //        DataRow[] dr = objDS.Tables[0].Select(strWhere5);
                                    //        if (dr.Length > 0)
                                    //        {
                                    //            counter = counter + 1;
                                    //            if (objDS.Tables[0].Rows[i]["ReasonForInvalid"] == DBNull.Value && objDS.Tables[0].Rows[i]["ReasonForInvalid"] == string.Empty)
                                    //            {
                                    //                objDS.Tables[0].Rows[i]["ReasonForInvalid"] = "Sending Stock to the same plant!<br/>";
                                    //            }
                                    //            else
                                    //                objDS.Tables[0].Rows[i]["ReasonForInvalid"] += ";Sending Stock to the same plant!<br/>";
                                    //        }
                                    //    }
                                    //    if (Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening) >= Convert.ToDateTime(DateTime.Now.Date.AddDays(PageBase.NumberofBackDaysAllowed)))
                                    //    {

                                    //        if (Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-PageBase.BackDaysAllowedBeforeOpening) > Convert.ToDateTime(objDS.Tables[0].Rows[i]["InvoiceDate"]))
                                    //        {
                                    //            counter = counter + 1;
                                    //            if (objDS.Tables[0].Rows[i]["ReasonForInvalid"] == DBNull.Value && objDS.Tables[0].Rows[i]["ReasonForInvalid"] == string.Empty)
                                    //            {
                                    //                objDS.Tables[0].Rows[i]["ReasonForInvalid"] = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", (Convert.ToDateTime(DateTime.Now.Date.ToShortDateString()) - Convert.ToDateTime(Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening).ToShortDateString())).TotalDays.ToString());
                                    //            }
                                    //            else
                                    //                objDS.Tables[0].Rows[i]["ReasonForInvalid"] += ";" + Resources.Messages.ValidationSalesDays.ToString().Replace("Number", (Convert.ToDateTime(DateTime.Now.Date.ToShortDateString()) - Convert.ToDateTime(Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening).ToShortDateString())).TotalDays.ToString());
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        if (Convert.ToDateTime(DateTime.Now.Date).AddDays(PageBase.NumberofBackDaysAllowed) > Convert.ToDateTime(objDS.Tables[0].Rows[i]["InvoiceDate"]))
                                    //        {
                                    //            counter = counter + 1;
                                    //            if (objDS.Tables[0].Rows[i]["ReasonForInvalid"] == DBNull.Value && objDS.Tables[0].Rows[i]["ReasonForInvalid"] == string.Empty)
                                    //            {
                                    //                objDS.Tables[0].Rows[i]["ReasonForInvalid"] = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
                                    //            }
                                    //            else
                                    //                objDS.Tables[0].Rows[i]["ReasonForInvalid"] += ";" + Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));

                                    //        }
                                    //    }

                                    //}
                                    if (counter > 0)
                                    {
                                        ucMsg.ShowInfo("Invalid Records");
                                        // hlnkInvalid.Visible = true;
                                        // string strFileName = "invalidData" + DateTime.Now.Ticks;
                                        // ExportInExcel(objDS.Tables["DtExcelSheet"], strFileName);
                                        // hlnkInvalid.NavigateUrl = strExcelVirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                        // hlnkInvalid.Text = "Invalid Data";
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
                                    // hlnkDuplicate.Visible = true;
                                    // string strFileName = "DuplicateData" + DateTime.Now.Ticks;
                                    // ExportInExcel(objDS.Tables["DtDuplicateRecord"], strFileName);
                                    dsErrorProne.Merge(objDS.Tables["DtDuplicateRecord"]);
                                    // hlnkDuplicate.NavigateUrl = strExcelVirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                    //hlnkDuplicate.Text = "Duplicate Data";
                                    blnIsUpload = false;
                                }
                                if (objDS.Tables["DtBlankData"] != null && objDS.Tables["DtBlankData"].Rows.Count > 0)
                                {
                                    //hlnkBlank.Visible = true;
                                    // string strFileName = "BlankData" + DateTime.Now.Ticks;
                                    // ExportInExcel(objDS.Tables["DtBlankData"], strFileName);
                                    dsErrorProne.Merge(objDS.Tables["DtBlankData"]);
                                    //hlnkBlank.NavigateUrl = strExcelVirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                    //hlnkBlank.Text = "Blank Data";
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
                                    for (int i = 0; i < dsErrorProne.Tables.Count; i++)
                                        if (dsErrorProne.Tables[i].Columns["WarehouseCode"] != null)
                                            dsErrorProne.Tables[i].Columns["WarehouseCode"].ColumnName = "FromSalesChannelCode";
                                    dsErrorProne.AcceptChanges();
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
                    ucMsg.ShowInfo(Resources.Messages.NoRecord);
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
                DataColumn dcQuantity = new DataColumn();
                dcQuantity.DataType = typeof(System.Int32);
                dcQuantity.ColumnName = "QuantityNew";
                dtGRN.Columns.Add(dcQuantity);
                foreach (DataRow dr in dtGRN.Rows)
                {
                    dr["QuantityNew"] = Convert.ToInt32(dr["Quantity"]);
                }
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
                                SkuCode = (grp.Key.SkuCode.Trim() == string.Empty | grp.Key.SkuCode.Trim() == null) ? Resources.Messages.SkuNotMentioned : grp.Key.SkuCode.Trim(),
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
                    Tvp = ObjCommom.GettvpTableIntermediarySalesSB();
                }
                OpenXMLExcel objexcel = new OpenXMLExcel();
                DsExcelDetail = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + ViewState["TobeUploaded"].ToString());

                //foreach (DataRow dr in DsExcelDetail.Tables[0].Rows)
                //{
                //    DataRow drow = Tvp.NewRow();
                //    drow["FromSalesChannelCode"] = dr["FromSalesChannelCode"].ToString().Trim();
                //    drow["ToSalesChannelCode"] = dr["ToSalesChannelCode"].ToString().Trim();
                //    drow["OrderNumber"] = dr["OrderNumber"];
                //    drow["InvoiceNumber"] = dr["InvoiceNumber"].ToString().Trim();
                //    drow["InvoiceDate"] = dr["InvoiceDate"];
                //    drow["SKUCode"] = dr["SKUCode"].ToString().Trim();
                //    drow["Quantity"] = Convert.ToInt32(dr["Quantity"]);
                //    drow["Serial#1"] = dr["Serial#1"].ToString().Trim();
                //    drow["BatchNo"] = dr["BatchNo"].ToString().Trim();
                //    Tvp.Rows.Add(drow);
                //}
                //Tvp.AcceptChanges();
                string guid = Guid.NewGuid().ToString();
                ViewState[strIntermediarySessionName] = guid;
                DsExcelDetail.Tables[0].Columns.Add(AddColumn(guid, "TransUploadSession", typeof(System.String)));
                DsExcelDetail.Tables[0].Columns.Add(AddColumn("4", "TransType", typeof(System.Int32)));
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
                    objP1.TransUploadSession = Convert.ToString(ViewState[strIntermediarySessionName]);
                    objP1.ComingFrom = 1;/* #CC03 Added (coming from bcp)*/
                    intResult = objP1.InsertIntermediarySalesSBBCP();

                    if (objP1.ErrorDetailXML != null && objP1.ErrorDetailXML != string.Empty)
                    {
                        StringReader theReader = new StringReader(objP1.ErrorDetailXML);
                        DataSet theDataSet = new DataSet();
                        theDataSet.ReadXml(theReader);
                        hlnkInvalid.Visible = true;
                        dsErrorProne.Merge(theDataSet.Tables[0]);
                        string strFileName = "InvalidData" + DateTime.Now.Ticks;
                        ExportInExcel(dsErrorProne, strFileName);
                        hlnkInvalid.Text = "Invalid Data";
                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
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
            Response.Redirect("~/Transactions/SalesChannelSB/Interface/PrimarySales2InterfaceAddV1.aspx");
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
    protected void DwnldReferenceCodeTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet DsReferenceCode = new DataSet();
            using (SalesChannelData objSalesData = new SalesChannelData())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.ePrimary2Sales;
                objSalesData.SalesChannelID = PageBase.SalesChanelID;
                objSalesData.Brand = PageBase.Brand;
                DsReferenceCode = objSalesData.GetAllTemplateData();
                if (DsReferenceCode != null && DsReferenceCode.Tables.Count > 0)
                {
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "SDCode", "SalesToCode", "SkuCodeList","BatchCodeList" };
                    ChangedExcelSheetNames(ref DsReferenceCode, strExcelSheetName, 4);
                    //PageBase.ExportToExecl(DsReferenceCode, FilenameToexport, EnumData.eTemplateCount.ePrimarysales2 + 1);
                    if (DsReferenceCode.Tables.Count > 4)
                        DsReferenceCode.Tables.RemoveAt(4);
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(DsReferenceCode, FilenameToexport, 4, strExcelSheetName);
                    
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
                bulkCopy.ColumnMappings.Add("FromSalesChannelCode", "FromCode");
                bulkCopy.ColumnMappings.Add("ToSalesChannelCode", "ToCode");
                bulkCopy.ColumnMappings.Add("InvoiceNumber", "TransNumber");
                bulkCopy.ColumnMappings.Add("InvoiceDate", "TransDate");
                bulkCopy.ColumnMappings.Add("SKUCode", "SKUCode");
                bulkCopy.ColumnMappings.Add("Quantity", "Quantity");
                bulkCopy.ColumnMappings.Add("Serial#1", "Serial#1");
                bulkCopy.ColumnMappings.Add("BatchNo", "BatchNo");
                bulkCopy.ColumnMappings.Add("BinCode", "StockBinType");/*#CC01 ADDED*/
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
    /*#CC02 START ADDED*/
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
                    //PageBase.ExportToExecl(dsReferenceCode, FilenameToexport, EnumData.eTemplateCount.ePrimarysales1);
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsReferenceCode, FilenameToexport);
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
        /*#CC02 END ADDED*/

    }
}
