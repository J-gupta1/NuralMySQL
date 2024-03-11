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
using Microsoft.ApplicationBlocks.Data;
using ZedService;
/*
 * 21 July 2016, Karam Chand Sharma, #CC01, Download bin code as an reference
 */
public partial class Transactions_SalesChannelSB_Upload_ManageSalesChannelGRN_SB_BCP :  PageBase
{
    DataTable dtNew = new DataTable();
    object objSum;
    string strGRNSessionName = "GRNUploadSession";
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
            //ViewState["TobeUploaded"] = strUploadedFileName;
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
                                        //InsertData(objDS.Tables[0]);
                                        Save(objDS);
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
            ucMsg.ShowError(ex.Source + " "+ ex.Message);
        }
    }



    //private void InsertData(DataTable dtGRN)
    //{
    //    try
    //    {
    //        if (dtGRN.Rows.Count > 0)
    //        {
    //            dvUploadPreview.Visible = true;
    //            DataColumn dcQuantity = new DataColumn();
    //            dcQuantity.DataType = typeof(System.Int32);
    //            dcQuantity.ColumnName = "QuantityNew";
    //            dtGRN.Columns.Add(dcQuantity);
    //            foreach (DataRow dr in dtGRN.Rows)
    //            {
    //                dr["QuantityNew"] = Convert.ToInt32(dr["Quantity"]);
    //            }
    //            dtGRN.AcceptChanges();
    //            objSum = dtGRN.Compute("sum(QuantityNew)", "");

    //            Btnsave.Enabled = true;
    //            if (Convert.ToInt32(objSum) <= 0)
    //            {
    //                Btnsave.Enabled = false;
    //                ucMsg.ShowInfo("Please Insert right Quantity");
    //                return;
    //            }

    //            var query = from row in dtGRN.AsEnumerable()
    //                        group row by new
    //                        {
    //                            SkuCode = Convert.ToString(row["SkuCode"])
    //                        } into grp
    //                        orderby grp.Key.SkuCode
    //                        select new
    //                        {
    //                            Key = grp.Key,
    //                            SkuCode = grp.Key.SkuCode,
    //                            Quantity = grp.Sum(r => r.Field<Int32>("QuantityNew"))
    //                        };
    //            int result = (from r in dtGRN.AsEnumerable() select r.Field<Int32>("QuantityNew")).Sum();
    //            lblTotal.Visible = true;
    //            lblTotal.Text = "Total Quantity: " + Convert.ToString(result);

    //            GridGRN.DataSource = query;
    //            GridGRN.DataBind();
    //            pnlGrid.Visible = true;
    //            Btnsave.Enabled = true;
    //            Btnsave.Visible = true;
    //            BtnCancel.Visible = true;
    //            updGrid.Update();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucMsg.ShowError(ex.ToString());
    //    }

    //}
    private void Save(DataSet DsExcelDetail)
    {
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }

            //if (ViewState["TobeUploaded"] != null)
            //{
                int intResult = 0;
                //DataTable Tvp = new DataTable();
                //DataSet DsExcelDetail = new DataSet();
                //using (CommonData ObjCommom = new CommonData())
                //{
                //    Tvp = ObjCommom.GettvpTableGRNSB();
                //}
                //OpenXMLExcel objexcel = new OpenXMLExcel();
                //DsExcelDetail = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + ViewState["TobeUploaded"].ToString());

                //foreach (DataRow dr in DsExcelDetail.Tables[0].Rows)
                //{
                //    DataRow drow = Tvp.NewRow();

                //    drow["WareHouseCode"] = dr["WareHouseCode"].ToString().Trim();
                //    drow["GRNNumber"] = dr["GRNNumber"].ToString().Trim();
                //    drow["GRNDate"] = dr["GRNDate"];
                //    drow["SKUCode"] = dr["SKUCode"].ToString().Trim();
                //    drow["Quantity"] = Convert.ToInt32(dr["Quantity"]);
                //    drow["Serial#1"] = dr["Serial#1"].ToString().Trim();
                //    drow["Serial#2"] = dr["Serial#2"].ToString().Trim();
                //    drow["Serial#3"] = dr["Serial#3"].ToString().Trim();
                //    drow["Serial#4"] = dr["Serial#4"].ToString().Trim();
                //    drow["BatchNo"] = dr["BatchNo"].ToString().Trim();
                //    Tvp.Rows.Add(drow);
                //}
                //Tvp.AcceptChanges();

                string guid = Guid.NewGuid().ToString();
                //ViewState[strGRNSessionName] = guid;
                DsExcelDetail.Tables[0].Columns.Add(AddColumn(guid, "TransUploadSession", typeof(System.String)));
                DsExcelDetail.Tables[0].Columns.Add(AddColumn("1", "TransType", typeof(System.Int32)));
                DsExcelDetail.Tables[0].AcceptChanges();
                if (DsExcelDetail.Tables[0].Rows.Count > 0)
                {
                    if (!BulkCopyUploadGRN(DsExcelDetail.Tables[0]))
                    {
                        ucMsg.ShowError("Error Occured While transferring the data to the server");
                        return;
                    }

                }


                using (SalesData objGRN = new SalesData())
                {
                    objGRN.EntryType = EnumData.eEntryType.eUpload;
                    objGRN.UserID = PageBase.UserId;
                    objGRN.TransUploadSession = guid;
                    intResult = objGRN.InsertInfoGRNUploadSBBCP();

                    if (objGRN.ErrorDetailXML != null && objGRN.ErrorDetailXML != string.Empty)
                    {
                        ucMsg.XmlErrorSource = objGRN.ErrorDetailXML;
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
            using (SalesChannelData objSalesData = new SalesChannelData())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.eGRN;

                dsReferenceCode = objSalesData.GetAllTemplateData();
                if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "WarehouseCode",  "SkuCodeList", "Bincode" };
                    ChangedExcelSheetNames(ref dsReferenceCode, strExcelSheetName, 3);


                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsReferenceCode, FilenameToexport, 3, strExcelSheetName);
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
            ObjSalesChannel.SalesChannelTypeID = 5;
            ObjSalesChannel.SalesChannelID = PageBase.SalesChanelID;
            ObjSalesChannel.BlnShowDetail = true;
            ObjSalesChannel.StatusValue = 2;
            ObjSalesChannel.CompanyId = PageBase.ClientId;
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
        //if (rdModelList.SelectedValue == "1")
        //    Response.Redirect("~/Transactions/SalesChannelSB/Interface/ManageWarehouseGRN.aspx");
    }
    public DataTable GetBlankTableError()
    {
        DataTable Detail = new DataTable();
        Detail.Columns.Add("GRNNumber");
        Detail.Columns.Add("WarehouseCode");
        Detail.Columns.Add("ReasonForInvalid");
        return Detail;
    }

    public bool BulkCopyUploadGRN(DataTable dtTempTable)
    {
        try
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "TransactionUploadBulk";
                bulkCopy.ColumnMappings.Add("WareHouseCode", "FromCode");
                bulkCopy.ColumnMappings.Add("GRNNumber", "TransNumber");
                bulkCopy.ColumnMappings.Add("GRNDate", "TransDate");
                bulkCopy.ColumnMappings.Add("SKUCode", "SKUCode");
                bulkCopy.ColumnMappings.Add("Quantity", "Quantity");
                bulkCopy.ColumnMappings.Add("Serial#1", "Serial#1");
                bulkCopy.ColumnMappings.Add("Serial#2", "Serial#2");
                bulkCopy.ColumnMappings.Add("Serial#3", "Serial#3");
                bulkCopy.ColumnMappings.Add("Serial#4", "Serial#4");
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

    DataColumn AddColumn(string columnValue,string ColumnName,Type ColumnType)
    {
        //string guid = Guid.NewGuid().ToString();
        //ViewState[strGRNSessionName] = guid;
        DataColumn dcSession = new DataColumn();
        dcSession.ColumnName = ColumnName;// "GRNUploadSession";
        
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
    /*#CC01 START ADDED*/
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
        /*#CC01 END ADDED*/
    }
}
