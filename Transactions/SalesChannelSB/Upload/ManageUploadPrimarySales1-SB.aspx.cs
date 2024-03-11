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

public partial class Transactions_SalesChannelSB_ManageUploadPrimarySales1_SB : PageBase
{
    DataTable dtNew = new DataTable();
    object objSum;
    int counter = 0;
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    UploadFile UploadFile = new UploadFile();
    List<String> lstDuplicate = new List<String>();
    DataSet dsErrorProne = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
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
                //if (PageBase.SalesChanelID != 0)
                //{
                //    btnwarehousecode.Visible = false;

                //}
                
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
                            objValidateFile.ExcelFileNameInTable = "PrimarySales1-SB";
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
                                        //ExportInExcel(objDS.Tables["DtExcelSheet"], strFileName);
                                        //hlnkInvalid.NavigateUrl = strExcelVirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
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
                                    if (Session["PRIMARYINGRNMODE"] != null)
                                    {
                                        if (Convert.ToInt32(Session["PRIMARYINGRNMODE"]) == 0)
                                        {
                                            if (PageBase.SalesChanelID != 0)
                                            {
                                                var query = from r in objDS.Tables[0].AsEnumerable()
                                                            where ((Convert.ToString(r["WarehouseCode"]) != PageBase.SalesChanelCode))
                                                            select new
                                                            {
                                                                InvoiceNumber = Convert.ToString(r["InvoiceNumber"]),
                                                                //InvoiceNumber = Convert.ToString(r["InvoiceNumber"]),
                                                                WarehouseCode = Convert.ToString(r["WarehouseCode"]),
                                                                SalesChannelCode = Convert.ToString(r["SalesChannelCode"])
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
                                                            where ((Convert.ToString(r["SalesChannelCode"]) != PageBase.SalesChanelCode))
                                                            select new
                                                            {
                                                                InvoiceNumber = Convert.ToString(r["InvoiceNumber"]),
                                                                WarehouseCode = Convert.ToString(r["WarehouseCode"]),
                                                                SalesChannelCode = Convert.ToString(r["SalesChannelCode"])
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
                                                                   WarehouseCode = Convert.ToString(row["WarehouseCode"])

                                                               } into grp
                                                               where (from p in grp
                                                                      select Convert.ToString(p["SalesChannelCode"])).Distinct().Count() > 1
                                                               orderby grp.Key.InvoiceNumber
                                                               select new
                                                               {
                                                                   Key = grp.Key,
                                                                   InvoiceNumber = grp.Key.InvoiceNumber,
                                                                   WarehouseCode = grp.Key.WarehouseCode,
                                                                   UniqueRows = (from p in grp
                                                                                 select Convert.ToString(p["SalesChannelCode"])).Distinct().Count()
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
                                                drow["WarehouseCode"] = dr["WarehouseCode"];
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
                                                             WarehouseCode = Convert.ToString(row["WarehouseCode"])
                                                         } into grp
                                                         where (from p in grp
                                                                select Convert.ToString(p["InvoiceDate"])).Distinct().Count() > 1
                                                         orderby grp.Key.InvoiceNumber
                                                         select new
                                                         {
                                                             Key = grp.Key,
                                                             InvoiceNumber = grp.Key.InvoiceNumber,
                                                             WarehouseCode=grp.Key.WarehouseCode,
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
                                                drow["WarehouseCode"] = dr["WarehouseCode"];
                                                drow["SalesChannelCode"] = "";
                                                drow["ReasonForInvalid"] = "Same Invoice Number has different InvoiceDates";
                                                dtErrorTable.Rows.Add(drow);

                                            }
                                            dtErrorTable.AcceptChanges();
                                        }
                                    }
                                    //sending stock to the same saleschannelcode means (FromSalesChannelCode=TosalesChannelCode)
                                    var SameData = from r in objDS.Tables[0].AsEnumerable()
                                                   where ((Convert.ToString(r["WarehouseCode"]) == (Convert.ToString(r["SalesChannelCode"]))))
                                                   select new
                                                   {
                                                       InvoiceNumber =Convert.ToString(r["InvoiceNumber"]),
                                                       WarehouseCode =Convert.ToString(r["WarehouseCode"]),
                                                       SalesChannelCode = Convert.ToString(r["SalesChannelCode"])
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
                                                             WarehouseCode = Convert.ToString(row["WarehouseCode"]),
                                                             SalesChannelCode = Convert.ToString(row["SalesChannelCode"])
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
                                    //for (int i = 0; i < objDS.Tables[0].Rows.Count; i++)
                                    //{
                                    //    if (PageBase.SalesChanelID != 0)
                                    //    {
                                    //        string srtwe = "WarehouseCode <> '" + PageBase.SalesChanelCode + "'";
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

                                    //    if (objDS.Tables[0].Rows[i]["SalesChannelCode"] != DBNull.Value)
                                    //    {
                                    //        string strWhere = "SalesChannelCode<>'" + objDS.Tables[0].Rows[i]["SalesChannelCode"].ToString().Trim() + "' and InvoiceNumber='" + objDS.Tables[0].Rows[i]["InvoiceNumber"].ToString().Trim() + "'";
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
                                       
                                    //        string strWhere1 = "WarehouseCode<>'" + objDS.Tables[0].Rows[i]["WarehouseCode"].ToString().Trim() + "' and InvoiceNumber='" + objDS.Tables[0].Rows[i]["InvoiceNumber"].ToString().Trim() + "'";

                                    //        if (objDS.Tables[0].Rows[i]["WarehouseCode"] != DBNull.Value)
                                    //        {

                                    //            DataRow[] dr1 = objDS.Tables[0].Select(strWhere1);
                                    //            if (dr1.Length > 0)
                                    //            {
                                    //                counter = counter + 1;
                                    //                if (objDS.Tables[0].Rows[i]["ReasonForInvalid"] == DBNull.Value && objDS.Tables[0].Rows[i]["ReasonForInvalid"] == string.Empty)
                                    //                {
                                    //                    objDS.Tables[0].Rows[i]["ReasonForInvalid"] = "Same Invoice Number has different FromPlantCode";
                                    //                }
                                    //                else
                                    //                {
                                    //                    objDS.Tables[0].Rows[i]["ReasonForInvalid"] += ";Same Invoice Number has different FromPlantCode";
                                    //                }

                                    //            }

                                    //        }

                                    //       string strWhere3 = "WarehouseCode<>'" + objDS.Tables[0].Rows[i]["WarehouseCode"].ToString().Trim() + "' and SalesChannelCode<>'" + objDS.Tables[0].Rows[i]["SalesChannelCode"].ToString().Trim() + "' and InvoiceNumber='" + objDS.Tables[0].Rows[i]["InvoiceNumber"].ToString().Trim() + "'";
                                    //        DataRow[] dr3 = objDS.Tables[0].Select(strWhere3);
                                    //        if (dr3.Length > 0)
                                    //        {
                                    //            counter = counter + 1;
                                    //            if (objDS.Tables[0].Rows[i]["ReasonForInvalid"] == DBNull.Value && objDS.Tables[0].Rows[i]["ReasonForInvalid"] == string.Empty)
                                    //            {
                                    //                objDS.Tables[0].Rows[i]["ReasonForInvalid"] = "Same Invoice Number has different FromPlantCode To ToPlantCode.<br/>";
                                    //            }
                                    //            else
                                    //                objDS.Tables[0].Rows[i]["ReasonForInvalid"] = ";Same Invoice Number has different FromPlantCode To ToPlantCode.<br/>";
                                    //        }

                                    //     if (lstDuplicate.Count > 0 && lstDuplicate != null)
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
                                    //     string strWhere4 = "InvoiceNumber='" + objDS.Tables[0].Rows[i]["InvoiceNumber"].ToString().Trim() + "'and InvoiceDate <>'" + objDS.Tables[0].Rows[i]["InvoiceDate"].ToString().Trim() + "'";
                                    //     if (objDS.Tables[0].Rows[i]["InvoiceNumber"] != DBNull.Value)
                                    //     {
                                    //         DataRow[] dr = objDS.Tables[0].Select(strWhere4);
                                    //         if (dr.Length > 0)
                                    //         {
                                    //             counter = counter + 1;
                                    //             if (objDS.Tables[0].Rows[i]["ReasonForInvalid"] == DBNull.Value && objDS.Tables[0].Rows[i]["ReasonForInvalid"] == string.Empty)
                                    //             {
                                    //                 objDS.Tables[0].Rows[i]["ReasonForInvalid"] = "Same invoice no with multiple dates!<br/>";
                                    //             }
                                    //             else
                                    //                 objDS.Tables[0].Rows[i]["ReasonForInvalid"] += ";Same invoice no with multiple dates!<br/>";
                                    //         }
                                    //         TimeSpan ts = Convert.ToDateTime(objDS.Tables[0].Rows[i]["invoiceDate"]).Subtract(System.DateTime.Now.Date);
                                    //         if (ts.Days > 0)
                                    //         {
                                    //             counter = counter + 1;
                                    //             if (objDS.Tables[0].Rows[i]["ReasonForInvalid"] != DBNull.Value)
                                    //                 objDS.Tables[0].Rows[i]["ReasonForInvalid"] = " Invoice date should not be greater than current date!<br/>";
                                    //             else
                                    //                 objDS.Tables[0].Rows[i]["ReasonForInvalid"] += ";Invoice date should not be greater than current date!<br/>";
                                    //         }
                                    //     }

                                    //     string strWhere5 = "'" + objDS.Tables[0].Rows[i]["SalesChannelCode"].ToString().Trim() + "'='" + objDS.Tables[0].Rows[i]["WarehouseCode"].ToString().Trim() + "'";
                                    //     if (objDS.Tables[0].Rows[i]["SalesChannelCode"] != DBNull.Value && objDS.Tables[0].Rows[i]["WarehouseCode"] != DBNull.Value)
                                    //     {
                                    //         DataRow[] dr = objDS.Tables[0].Select(strWhere5);
                                    //         if (dr.Length > 0)
                                    //         {
                                    //             counter = counter + 1;
                                    //             if (objDS.Tables[0].Rows[i]["ReasonForInvalid"] == DBNull.Value && objDS.Tables[0].Rows[i]["ReasonForInvalid"] == string.Empty)
                                    //             {
                                    //                 objDS.Tables[0].Rows[i]["ReasonForInvalid"] = "Sending Stock to the same plant!<br/>";
                                    //             }
                                    //             else
                                    //                 objDS.Tables[0].Rows[i]["ReasonForInvalid"] += ";Sending Stock to the same plant!<br/>";
                                    //         }
                                    //     }

                                    //}
                                    if (counter > 0)
                                    {
                                        ucMsg.ShowInfo("Invalid Records");
                                        //hlnkInvalid.Visible = true;
                                        //string strFileName = "invalidData" + DateTime.Now.Ticks;
                                       // ExportInExcel(objDS.Tables["DtExcelSheet"], strFileName);
                                        //ExportInExcel(dtErrorTable, strFileName);
                                        //hlnkInvalid.NavigateUrl = strExcelVirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                        //hlnkInvalid.Text = "Invalid Data";
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
                                    //hlnkDuplicate.Visible = true;
                                   // string strFileName = "DuplicateData" + DateTime.Now.Ticks;
                                   // ExportInExcel(objDS.Tables["DtDuplicateRecord"], strFileName);
                                    dsErrorProne.Merge(objDS.Tables["DtDuplicateRecord"]);
                                    //hlnkDuplicate.NavigateUrl = strExcelVirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                    //hlnkDuplicate.Text = "Duplicate Data";
                                    blnIsUpload = false;
                                }
                                if (objDS.Tables["DtBlankData"] != null && objDS.Tables["DtBlankData"].Rows.Count > 0)
                                {
                                    //hlnkBlank.Visible = true;
                                    //string strFileName = "BlankData" + DateTime.Now.Ticks;
                                    //ExportInExcel(objDS.Tables["DtBlankData"], strFileName);
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
                    Tvp = ObjCommom.GettvpTablePrimarySales1SB();
                }
                OpenXMLExcel objexcel = new OpenXMLExcel();
                DsExcelDetail = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + ViewState["TobeUploaded"].ToString());

                foreach (DataRow dr in DsExcelDetail.Tables[0].Rows)
                {
                    DataRow drow = Tvp.NewRow();
                    drow["WareHouseCode"] = dr["WareHouseCode"].ToString().Trim();
                    drow["SalesChannelCode"] = dr["SalesChannelCode"].ToString().Trim();
                    drow["OrderNumber"] = dr["OrderNumber"];
                    drow["InvoiceNumber"] = dr["InvoiceNumber"].ToString().Trim();
                    drow["InvoiceDate"] = dr["InvoiceDate"];
                    drow["SKUCode"] = dr["SKUCode"].ToString().Trim();
                    drow["Quantity"] = Convert.ToInt32(dr["Quantity"]);
                    drow["Serial#1"] = dr["Serial#1"].ToString().Trim();
                    drow["BatchNo"] = dr["BatchNo"].ToString().Trim();
                    Tvp.Rows.Add(drow);
                }
                Tvp.AcceptChanges();
                using (SalesData objP1 = new SalesData())
                {
                    objP1.EntryType = EnumData.eEntryType.eUpload;
                    objP1.UserID = PageBase.UserId;
                    intResult = objP1.InsertPrimarySales1SB(Tvp);

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
                        if (Session["PRIMARYINGRNMODE"] != null)
                        {
                            if (Convert.ToInt32(Session["PRIMARYINGRNMODE"]) == 0)
                            {


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

                            }
                            else
                            {
                                Ds = dsReferenceCode;
                            }
                        }
                        
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
            Response.Redirect("~/Transactions/SalesChannelSB/Interface/PrimarySalesInterfaceForAddV1.aspx");
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
   
   
    
}
