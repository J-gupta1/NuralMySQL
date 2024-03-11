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
/*
 * 18 July 2016, Karam Chand Sharma, #CC01, Pass stock bin type in datatable
 * 21 July 2016, Karam Chand Sharma, #CC02, Download bin code as an reference
 * 31-May-2019,Vijay Kumar Prajapati,#CC03,Added VoucherExpiryDate in Excel done for shivalik.
 */

public partial class Transactions_SalesChannelSB_ManageSalesChannelGRN_SB : PageBase
{
    DataTable dtNew = new DataTable();
    object objSum;
    int counter = 0;
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    UploadFile UploadFile = new UploadFile();
   // List<String> lstDuplicate = new List<String>();
    DataSet dsErrorProne = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMsg.ShowControl = false;
            if (!IsPostBack)
            {
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
                //DataSet DsExcel = objexcel.ImportExcelFile(RootPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
                DataSet DsExcel = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);
                
                if (DsExcel != null && DsExcel.Tables.Count > 0 && DsExcel.Tables[0].Rows.Count > 0)
                {

                    if (DsExcel.Tables[0].Rows.Count > Convert.ToInt32(PageBase.ValidExcelRows))
                        ucMsg.ShowInfo("Limit Crossed");
                    else
                    {
                        ValidateUploadFile objValidateFile = new ValidateUploadFile();
                        {
                            //string strPkColName = "";
                            DataSet objDS = DsExcel;
                            DataTable dt1 = DsExcel.Tables[0];
                            SortedList objSL = new SortedList();
                            SortedList objSLCorrData = new SortedList();
                            objValidateFile.UploadedFileName = strUploadedFileName;
                            objValidateFile.ExcelFileNameInTable = "GRN-SB";
                            // objValidateFile.PkColumnName = strPkColName;
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
                                        //ExportInExcel(objDS.Tables["DtExcelSheet"], strFileName);
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
                                    objDS.Tables[0].Columns.Add("GRNNumber1", typeof(string), "GRNNumber");
                                    objDS.Tables[0].AcceptChanges();
                                    if (PageBase.SalesChanelID != 0)
                                    {
                                        var query = from r in objDS.Tables[0].AsEnumerable()
                                                    where ((Convert.ToString(r["WarehouseCode"]) != PageBase.SalesChanelCode))
                                                    select new
                                                    {
                                                        GRNNumber = Convert.ToString(r["GRNNumber1"]),
                                                        WarehouseCode = Convert.ToString(r["WarehouseCode"])
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
                                                    drow["GRNNumber"] = dr["GRNNumber"];
                                                    drow["WarehouseCode"] = dr["WarehouseCode"];
                                                    drow["ReasonForInvalid"] = "Can't Upload the GRN of other SalesChannel";
                                                    dtErrorTable.Rows.Add(drow);
                                                }
                                                dtErrorTable.AcceptChanges();
                                            }
                                        }
                                    }

                                    var SameGrnMultipleWarehouse = from row in objDS.Tables[0].AsEnumerable()
                                                               group row by new
                                                               {
                                                                   GRNNumber = Convert.ToString(row["GRNNumber1"])

                                                               } into grp
                                                               where (from p in grp
                                                                      select Convert.ToString(p["WarehouseCode"])).Distinct().Count() > 1
                                                                   orderby grp.Key.GRNNumber
                                                               select new
                                                               {
                                                                   Key = grp.Key,
                                                                   GRNNumber = grp.Key.GRNNumber,
                                                                   UniqueRows = (from p in grp
                                                                                 select Convert.ToString(p["WarehouseCode"])).Distinct().Count()
                                                               };
                                    //differentToPlantCode = differentToPlantCode.Where(r => ((string)r["Firstname"]).StartsWith("J")); 

                                    if (SameGrnMultipleWarehouse != null)
                                    {

                                        if (SameGrnMultipleWarehouse.Count() > 0)
                                        {
                                            dtError = new DataTable();
                                            counter = 1;
                                            dtError = PageBase.LINQToDataTable(SameGrnMultipleWarehouse);
                                            foreach (DataRow dr in dtError.Rows)
                                            {
                                                DataRow drow = dtErrorTable.NewRow();
                                                drow["GRNNumber"] = dr["GRNNumber"];
                                                drow["WarehouseCode"] = "";
                                                drow["ReasonForInvalid"] = "Same GRN Number has different WarehouseCode";
                                                dtErrorTable.Rows.Add(drow);
                                            }
                                            dtErrorTable.AcceptChanges();
                                        }
                                    }

                                    //when a invoice number has different dates
                                    var differentDates = from row in objDS.Tables[0].AsEnumerable()
                                                         group row by new
                                                         {
                                                             GRNNumber = Convert.ToString(row["GRNNumber1"])
                                                         } into grp
                                                         where (from p in grp
                                                                select Convert.ToString(p["GRNDate"])).Distinct().Count() > 1
                                                         orderby grp.Key.GRNNumber
                                                         select new
                                                         {
                                                             Key = grp.Key,
                                                             GRNNumber = grp.Key.GRNNumber,
                                                             UniqueRows = (from p in grp
                                                                           select Convert.ToString(p["GRNDate"])).Distinct().Count()
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
                                                drow["GRNNumber"] = dr["GRNNumber"];
                                                drow["WarehouseCode"] = "";
                                                drow["ReasonForInvalid"] = "Same GRN Number has different Dates";
                                                dtErrorTable.Rows.Add(drow);

                                            }
                                            dtErrorTable.AcceptChanges();
                                        }
                                    }



                                   // for (int i = 0; i < objDS.Tables[0].Rows.Count; i++)
                                    //{
                                        //if (PageBase.SalesChanelID != 0)
                                        //{
                                        //    string srtwe = "WarehouseCode <> '" + PageBase.SalesChanelCode + "'";
                                        //    DataRow[] dr1 = objDS.Tables[0].Select(srtwe);
                                        //    if (dr1.Length > 0)
                                        //    {
                                        //        counter = counter + 1;
                                        //        if (objDS.Tables[0].Rows[i]["ReasonForInvalid"] == "" && objDS.Tables[0].Rows[i]["ReasonForInvalid"] == string.Empty)
                                        //        {
                                        //            objDS.Tables[0].Rows[i]["ReasonForInvalid"] = "Can't Upload the stock of other warehouse";
                                        //        }
                                        //        else
                                        //            objDS.Tables[0].Rows[i]["ReasonForInvalid"] += ";Can't Upload the stock of other warehouse";
                                        //    }
                                        //}
                                        //string strWhere = "WarehouseCode <>'" + objDS.Tables[0].Rows[i]["WarehouseCode"].ToString().Trim() +
                                        //    "'and GRNNumber ='" + objDS.Tables[0].Rows[i]["GRNNumber"].ToString().Trim() + "'";
                                        //DataRow[] dr = objDS.Tables[0].Select(strWhere);

                                        //if (dr.Length > 0)
                                        //{
                                        //    counter = counter + 1;
                                        //    if (objDS.Tables[0].Rows[i]["ReasonForInvalid"] == "" && objDS.Tables[0].Rows[i]["ReasonForInvalid"] == string.Empty)
                                        //    {
                                        //        objDS.Tables[0].Rows[i]["ReasonForInvalid"] = "Same GRN is assigned to different Warehouses";
                                        //    }
                                        //    else
                                        //        objDS.Tables[0].Rows[i]["ReasonForInvalid"] += ";Same GRN is assigned to different Warehouses";
                                        //}
                                        //string strWhere1 = "GRNNumber='" + objDS.Tables[0].Rows[i]["GRNNumber"].ToString().Trim() + "'and GRNDate <>'" + objDS.Tables[0].Rows[i]["GRNDate"].ToString().Trim() + "'";
                                        //if (objDS.Tables[0].Rows[i]["GRNNumber"] != DBNull.Value)
                                        //{

                                        //    DataRow[] dr1 = objDS.Tables[0].Select(strWhere1);
                                        //    if (dr.Length > 0)
                                        //    {
                                        //        counter = counter + 1;
                                        //        if (objDS.Tables[0].Rows[i]["ReasonForInvalid"] == DBNull.Value && objDS.Tables[0].Rows[i]["ReasonForInvalid"] == string.Empty)
                                        //        {
                                        //            objDS.Tables[0].Rows[i]["ReasonForInvalid"] = "Same GRN No with multiple dates!";
                                        //        }
                                        //        else
                                        //        {
                                        //            objDS.Tables[0].Rows[i]["ReasonForInvalid"] += ";Same GRN no with multiple dates!";
                                        //        }

                                        //    }

                                        //}
                                        //if (lstDuplicate.Count > 0 && lstDuplicate != null)
                                        //{
                                        //    foreach (var g in duplicates)
                                        //    {
                                        //        if (objDS.Tables[0].Rows[i]["Serial#1"].ToString() == g.serial.ToString())
                                        //        {
                                        //            counter = counter + 1;
                                        //            if (objDS.Tables[0].Rows[i]["ReasonForInvalid"] == DBNull.Value && objDS.Tables[0].Rows[i]["ReasonForInvalid"].ToString() == string.Empty)
                                        //            {
                                        //                objDS.Tables[0].Rows[i]["ReasonForInvalid"] = g.serial.ToString() + " Duplicate SerialNumber1 exists.";
                                        //            }
                                        //            else
                                        //                objDS.Tables[0].Rows[i]["ReasonForInvalid"] += ";" + g.serial.ToString() + " Duplicate SerialNumber1 exists.";
                                        //        }
                                        //    }
                                        //}
                                        //if (objDS.Tables[0].Rows[i]["skuCode"] != DBNull.Value)
                                        //{
                                        //    string strWhere2 = "skuCode ='" + objDS.Tables[0].Rows[i]["skuCode"].ToString().Trim() + "' and BatchNo ='" + objDS.Tables[0].Rows[i]["BatchNo"].ToString().Trim() + "'and [Serial#1] ='" + objDS.Tables[0].Rows[i]["Serial#1"].ToString().Trim() + "'";
                                        //    DataRow[] dr2 = objDS.Tables[0].Select(strWhere);
                                        //    if (dr.Length > 0)
                                        //    {
                                        //        counter = counter + 1;
                                        //        if (objDS.Tables[0].Rows[i]["ReasonForInvalid"] == DBNull.Value || objDS.Tables[0].Rows[i]["ReasonForInvalid"].ToString() == string.Empty)
                                        //        {
                                        //            objDS.Tables[0].Rows[i]["ReasonForInvalid"] = "Same sku Can not be defined in the Serialized or Batchwise mode ";
                                        //        }
                                        //        else
                                        //            objDS.Tables[0].Rows[i]["ReasonForInvalid"] += "Same sku Can not be defined in the Serialized or Batchwise mode";
                                        //    }
                                        //}

                                    //}
                                    if (counter > 0)
                                    {
                                        ucMsg.ShowInfo("Invalid Records");
                                        //hlnkInvalid.Visible = true;
                                        //string strFileName = "invalidData" + DateTime.Now.Ticks;
                                        //ExportInExcel(objDS.Tables["DtExcelSheet"], strFileName);
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
                                    //string strFileName = "DuplicateData" + DateTime.Now.Ticks;
                                    //ExportInExcel(objDS.Tables["DtDuplicateRecord"], strFileName);
                                    //hlnkDuplicate.NavigateUrl = strExcelVirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                    //hlnkDuplicate.Text = "Duplicate Data";
                                    dsErrorProne.Merge(objDS.Tables["DtDuplicateRecord"]);
                                    blnIsUpload = false;
                                }
                                if (objDS.Tables["DtBlankData"] != null && objDS.Tables["DtBlankData"].Rows.Count > 0)
                                {
                                   // hlnkBlank.Visible = true;
                                    //string strFileName = "BlankData" + DateTime.Now.Ticks;
                                    //ExportInExcel(objDS.Tables["DtBlankData"], strFileName);
                                    //hlnkBlank.NavigateUrl = strExcelVirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                    //hlnkBlank.Text = "Blank Data";
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
            //GridGRN.Visible = false;
            //try
            //{
            //    DataSet dsGRN = null;
            //    byte isSuccess = 1;
            //    Int16 UploadCheck = 0;
            //    String RootPath = Server.MapPath("../../");
            //    UploadFile.RootFolerPath = RootPath;
            //    UploadCheck = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            //    if (UploadCheck == 1)
            //    {
            //        UploadFile.UploadedFileName = strUploadedFileName;
            //        UploadFile.UploadValidationType = EnumData.eUploadExcelValidationType.eSales;
            //        isSuccess = UploadFile.uploadValidExcel(ref dsGRN, "GRN-SB");

            //        switch (isSuccess)
            //        {
            //            case 0:
            //                ucMsg.ShowInfo(UploadFile.Message);
            //                pnlGrid.Visible = false;
            //                break;
            //            case 2:
            //                ucMsg.ShowInfo(Resources.Messages.CheckErrorGrid);
            //                Btnsave.Enabled = false;
            //                pnlGrid.Visible = true;
            //                GridGRN.Visible = true;

            //                GridGRN.Columns[10].Visible = true;
            //                GridGRN.DataSource = dsGRN;
            //                GridGRN.DataBind();
            //                updGrid.Update();
            //                break;
            //            case 1:
            //                InsertData(dsGRN);
            //                break;
            //            case 3:
            //                ucMsg.ShowInfo(UploadFile.Message);
            //                break;


            //        }

            //    }
            //    else if (UploadCheck == 2)
            //    {
            //        pnlGrid.Visible = false;
            //        ucMsg.ShowInfo(Resources.Messages.UploadXlxs);
            //    }
            //    else if (UploadCheck == 3)
            //    {
            //        pnlGrid.Visible = false;
            //        ucMsg.ShowInfo(Resources.Messages.SelectFile);

            //    }
            //    else if (UploadCheck == 4)
            //    {
            //        pnlGrid.Visible = false;
            //        ucMsg.ShowInfo("File size should be less than " + PageBase.ValidExcelLength + " KB");
            //    }
            //    else
            //    {
            //        pnlGrid.Visible = false;
            //        ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            //    }


            //    updGrid.Update();
            //}
            //catch (Exception ex)
            //{
            //    pnlGrid.Visible = false;
            //    ucMsg.ShowError(ex.Message.ToString());
            //    //clsException.clsHandleException.fncHandleException(ex, "");
            //}
        }
        catch (Exception ex)
        {

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
               // updGrid.Update();
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
         if(ViewState["TobeUploaded"]!=null)
            {
                int intResult = 0;
                DataTable Tvp = new DataTable();
                DataSet DsExcelDetail = new DataSet();
                using (CommonData ObjCommom = new CommonData())
                {
                    Tvp = ObjCommom.GettvpTableGRNSB();
                }
                OpenXMLExcel objexcel = new OpenXMLExcel();
                DsExcelDetail = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + ViewState["TobeUploaded"].ToString());

                foreach (DataRow dr in DsExcelDetail.Tables[0].Rows)
                {
                    DataRow drow = Tvp.NewRow();

                    drow["WareHouseCode"] = dr["WareHouseCode"].ToString().Trim();
                    drow["GRNNumber"] = dr["GRNNumber"].ToString().Trim();
                    drow["GRNDate"] = dr["GRNDate"];
                    drow["SKUCode"] = dr["SKUCode"].ToString().Trim();
                    drow["Quantity"] = Convert.ToInt32(dr["Quantity"]);
                    drow["Serial#1"] = dr["Serial#1"].ToString().Trim();
                    drow["Serial#2"] = dr["Serial#2"].ToString().Trim();
                    drow["Serial#3"] = dr["Serial#3"].ToString().Trim();
                    drow["Serial#4"] = dr["Serial#4"].ToString().Trim();
                    drow["BatchNo"] = dr["BatchNo"].ToString().Trim();
                    drow["StockBinType"] = dr["BinCode"].ToString().Trim();/*#CC01 ADDED*/
                    drow["VoucherExpiryDate"] = dr["VoucherExpiryDate"].ToString().Trim();/*#CC03 ADDED*/
                    Tvp.Rows.Add(drow);
                }
                Tvp.AcceptChanges();
                using (SalesData objGRN = new SalesData())
                {
                    objGRN.EntryType = EnumData.eEntryType.eUpload;
                    objGRN.UserID = PageBase.UserId;
                    intResult = objGRN.InsertInfoGRNUploadSB(Tvp);

                    if (objGRN.ErrorDetailXML != null && objGRN.ErrorDetailXML != string.Empty)
                    {
                        ucMsg.XmlErrorSource = objGRN.ErrorDetailXML;
                        //ucMsg1.XmlErrorSource = objGRN.ErrorDetailXML;
                        return;
                    }
                    if (objGRN.Error != null && objGRN.Error != "")
                    {
                        ucMsg.ShowError(objGRN.Error);
                        return;
                    }
                    if (intResult == 2)
                    {
                        ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                        return;
                    }


                    ClearForm();
                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    pnlGrid.Visible = false;
                  //  updGrid.Update();

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
        lblTotal.Text = "";
        ucMsg.Visible = false;
        //updGrid.Update();
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
        //try
        //{
        //    DataTable dt;
        //    DataSet ds = new DataSet();
        //    DataSet dsReferenceCode = new DataSet();
        //    using (ProductData objProductData = new ProductData())
        //    {
        //        dt = objProductData.SelectAllSKUInfo();
        //        dt.DefaultView.RowFilter = "Status = True";
        //        dt = dt.DefaultView.ToTable();
        //        string[] strCode = new string[] { "SKUCode", "SKUName" };
        //        dt = dt.DefaultView.ToTable(true, strCode);
        //        ds.Tables.Add(dt);
        //        dsReferenceCode = ds;
        //        if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
        //        {
        //            String FilePath = Server.MapPath("../../");
        //            string FilenameToexport = "Reference Code List";
        //            PageBase.RootFilePath = FilePath;
        //            PageBase.ExportToExecl(dsReferenceCode, FilenameToexport, EnumData.eTemplateCount.ePrimarysales1);
        //        }
        //        else
        //        {
        //            ucMsg.ShowInfo(Resources.Messages.NoRecord);
        //        }
        //    }
        //}
        try
        {
            DataSet dsReferenceCode = new DataSet();
            using (SalesChannelData objSalesData = new SalesChannelData())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.ePrice;

                dsReferenceCode = objSalesData.GetAllTemplateData();
                if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
                {

                   // String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                   // PageBase.RootFilePath = FilePath;
                    //PageBase.ExportToExecl(dsReferenceCode, FilenameToexport, EnumData.eTemplateCount.ePrimarysales1);

                    string[] strExcelSheetName = {  "SkuCodeList", "BatchCodeList" };
                    ChangedExcelSheetNames(ref dsReferenceCode, strExcelSheetName, 2);
                    //PageBase.ExportToExecl(DsReferenceCode, FilenameToexport, EnumData.eTemplateCount.ePrimarysales2 + 1);
                    //if (dsReferenceCode.Tables.Count > 2)
                    //    dsReferenceCode.Tables.RemoveAt(2);
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsReferenceCode, FilenameToexport, 2, strExcelSheetName);
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
    protected void DwnldWarehouseTemplate_Click(object sender, EventArgs e)
    {
        DataTable dt;
        DataSet ds = new DataSet();
        DataSet dsReferenceCode = new DataSet();
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.SalesChannelTypeID = 14;
            ObjSalesChannel.SalesChannelID = PageBase.SalesChanelID;
            ObjSalesChannel.BlnShowDetail = true;
            ObjSalesChannel.StatusValue = 2;
            dt = ObjSalesChannel.GetSalesChannelInfo();

            dt.DefaultView.RowFilter = "Status = True";
            dt = dt.DefaultView.ToTable();
            string[] strCode = new string[] { "SalesChannelCode" };
            dt = dt.DefaultView.ToTable(true, strCode);
            dt.Columns["SalesChannelCode"].ColumnName = "WareHouseCode";

            ds.Tables.Add(dt);
            dsReferenceCode = ds;
            if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
            {
                //String FilePath = Server.MapPath("../../");
                string FilenameToexport = "/2 List";
                PageBase.RootFilePath = PageBase.strExcelPhysicalBlankTemplatePathSB;
                PageBase.ExportToExecl(dsReferenceCode, FilenameToexport, EnumData.eTemplateCount.ePrimarysales1);
            }
            else
            {
                ucMsg.ShowInfo(Resources.Messages.NoRecord);
            }

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
            Response.Redirect("~/Transactions/SalesChannelSB/Interface/ManageWarehouseGRN.aspx");
    }
    public DataTable GetBlankTableError()
    {
        DataTable Detail = new DataTable();
        Detail.Columns.Add("GRNNumber");
        Detail.Columns.Add("WarehouseCode");
        Detail.Columns.Add("ReasonForInvalid");
        return Detail;
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
        /*#CC02 END ADDED*/
    }
}
