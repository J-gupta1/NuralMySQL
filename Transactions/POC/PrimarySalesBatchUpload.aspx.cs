using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;

public partial class Transactions_POC_PrimarySalesBatchUpload :PageBase 
{
    DataTable dtNew = new DataTable();
    object objSum;
    int counter = 0;
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    UploadFile UploadFile = new UploadFile();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMsg.ShowControl = false;
            if (!IsPostBack)
                pnlGrid.Visible = false;

            updmsg.Update();

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
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
                    PageBase.ExportToExecl(Ds, FilenameToexport, EnumData.eTemplateCount.ePrimarysales1 + 2);
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
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        ViewState["Sales"] = null;
        GridSales.Visible = false;
        try
        {
            DataSet dsPrimarySales = null;
            byte isSuccess = 1;
            Int16 UploadCheck = 0;
            String RootPath = Server.MapPath("../../");
            UploadFile.RootFolerPath = RootPath;
            UploadCheck = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            if (UploadCheck == 1)
            {
                UploadFile.UploadedFileName = strUploadedFileName;
                UploadFile.UploadValidationType = EnumData.eUploadExcelValidationType.eSales;
                isSuccess = UploadFile.uploadValidExcel(ref dsPrimarySales, "PrimarySalesBatch");

                switch (isSuccess)
                {
                    case 0:
                        ucMsg.ShowInfo(UploadFile.Message);
                        break;
                    case 2:
                        ucMsg.ShowInfo(Resources.Messages.CheckErrorGrid);
                        Btnsave.Enabled = false;
                        pnlGrid.Visible = true;
                        GridSales.Visible = true;
                        GridSales.Columns[7].Visible = true;
                        GridSales.DataSource = dsPrimarySales;
                        GridSales.DataBind();
                        updGrid.Update();
                        break;
                    case 1:
                        InsertData(dsPrimarySales);
                        break;
                    case 3:
                        ucMsg.ShowInfo(UploadFile.Message);
                        break;
                }

            }
            else if (UploadCheck == 2)
            {
                pnlGrid.Visible = false;
                ucMsg.ShowInfo(Resources.Messages.UploadXlxs);
            }
            else if (UploadCheck == 3)
            {
                pnlGrid.Visible = false;
                ucMsg.ShowInfo(Resources.Messages.SelectFile);

            }
            else if (UploadCheck == 4)
            {
                pnlGrid.Visible = false;
                ucMsg.ShowInfo("File size should be less than " + PageBase.ValidExcelLength + " KB");
            }
            else
            {
                pnlGrid.Visible = false;
                ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            }
            updGrid.Update();
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            pnlGrid.Visible = false;
            PageBase.Errorhandling(ex);

        }

    }
    private void InsertData(DataSet dsPrimarySales)
    {
        DataTable dtPrimarySales = dsPrimarySales.Tables[0];
        DataColumn dcError = new DataColumn();
        dcError.DataType = System.Type.GetType("System.String");
        dcError.ColumnName = "Error";

        if (dtPrimarySales.Columns.Contains("Error") == false)
            dtPrimarySales.Columns.Add(dcError);
        for (int i = 0; i <= dtPrimarySales.Rows.Count - 1; i++)
        {
            if (dtPrimarySales != null && dtPrimarySales.Rows.Count > 0)
            {
                if (dtPrimarySales.Rows[i]["SalesChannelCode"] != DBNull.Value)
                {
                    string strWhere = "SalesChannelCode<>'" + dtPrimarySales.Rows[i]["SalesChannelCode"].ToString().Trim() + "'and InvoiceNumber ='" + dtPrimarySales.Rows[i]["InvoiceNumber"].ToString().Trim() + "'";
                    DataRow[] dr = dtPrimarySales.Select(strWhere);
                    if (dr.Length > 0)
                    {
                        counter = counter + 1;
                        if (dtPrimarySales.Rows[i]["Error"].ToString() == "" && dtPrimarySales.Rows[i]["Error"].ToString() == string.Empty)
                        {
                            dtPrimarySales.Rows[i]["Error"] = "Same Invoice Number has different " + Resources.Messages.SalesEntity + " Code";
                        }
                        else
                            dtPrimarySales.Rows[i]["Error"] = ";Same Invoice Number has different " + Resources.Messages.SalesEntity + " Code";
                    }
                    string strWhereW = "WarehouseCode<>'" + dtPrimarySales.Rows[i]["WarehouseCode"].ToString().Trim() + "'and InvoiceNumber ='" + dtPrimarySales.Rows[i]["InvoiceNumber"].ToString().Trim() + "'";
                    DataRow[] drW = dtPrimarySales.Select(strWhereW);
                    if (drW.Length > 0)
                    {
                        counter = counter + 1;
                        if (dtPrimarySales.Rows[i]["Error"].ToString() == "" && dtPrimarySales.Rows[i]["Error"].ToString() == string.Empty)
                        {
                            dtPrimarySales.Rows[i]["Error"] = "Same Invoice Number has different Warehouse Code";
                        }
                        else
                            dtPrimarySales.Rows[i]["Error"] = ";Same Invoice Number has different Warehouse Code";
                    }
                    DateTime dtInvoiceDate = Convert.ToDateTime(dtPrimarySales.Rows[i]["InvoiceDate"]);
                    TimeSpan ts = dtInvoiceDate.Subtract(System.DateTime.Now.Date);
                    if (ts.Days > 0)
                    {
                        counter = counter + 1;
                        if (dtPrimarySales.Rows[i]["Error"] != DBNull.Value)
                            dtPrimarySales.Rows[i]["Error"] = " Invoice date should not be greater than current date!";
                        else
                            dtPrimarySales.Rows[i]["Error"] += ";Invoice date should not be greater than current date!";
                    }
                    string strWhere5 = "WarehouseCode<>'" + dtPrimarySales.Rows[i]["WarehouseCode"].ToString().Trim() + "' and SalesChannelCode<>'" + dtPrimarySales.Rows[i]["SalesChannelCode"].ToString().Trim() + "' and InvoiceNumber='" + dtPrimarySales.Rows[i]["InvoiceNumber"].ToString().Trim() + "'";
                    DataRow[] dr1 = dtPrimarySales.Select(strWhere5);
                    if (dr1.Length > 0)
                    {
                        counter = counter + 1;
                        if (dtPrimarySales.Rows[i]["Error"] == DBNull.Value)
                        {
                            dtPrimarySales.Rows[i]["Error"] = "Same Invoice Number has different Warehouse To Distributor.";
                        }
                        else
                            dtPrimarySales.Rows[i]["Error"] = ";Same Invoice Number has different Warehouse To Distributor.";
                    }

                }
                string strWhere1 = "InvoiceNumber='" + dtPrimarySales.Rows[i]["InvoiceNumber"].ToString().Trim() + "'and InvoiceDate <>'" + dtPrimarySales.Rows[i]["InvoiceDate"].ToString().Trim() + "'";
                if (dtPrimarySales.Rows[i]["InvoiceNumber"] != DBNull.Value)
                {

                    DataRow[] dr = dtPrimarySales.Select(strWhere1);
                    if (dr.Length > 0)
                    {
                        counter = counter + 1;
                        if (dsPrimarySales.Tables[0].Rows[i]["Error"] == DBNull.Value || dtPrimarySales.Rows[i]["Error"].ToString() == string.Empty)
                        {
                            dtPrimarySales.Rows[i]["Error"] = "Same invoice no with multiple dates!";
                        }
                        else
                            dtPrimarySales.Rows[i]["Error"] += ";Same invoice no with multiple dates!";
                    }
                }
                //if (dtPrimarySales.Rows[i]["OrderNumber"] != DBNull.Value || dtPrimarySales.Rows[i]["OrderNumber"].ToString() != string.Empty)
                //{
                //    string strWhere2 = "SalesChannelCode<>'" + dtPrimarySales.Rows[i]["SalesChannelCode"].ToString().Trim() + "'and OrderNumber ='" + dtPrimarySales.Rows[i]["OrderNumber"].ToString().Trim() + "'";

                //    DataRow[] dr = dtPrimarySales.Select(strWhere2);
                //    if (dr.Length > 0)
                //    {
                //        counter = counter + 1;
                //        if (dsPrimarySales.Tables[0].Rows[i]["Error"] == DBNull.Value || dtPrimarySales.Rows[i]["Error"].ToString() == string.Empty)
                //        {
                //            dtPrimarySales.Rows[i]["Error"] = "Same order no with multiple saleschannel!";
                //        }
                //        else
                //            dtPrimarySales.Rows[i]["Error"] += ";Same invoice no with multiple saleschannel!";
                //    }
                //}

            }

        }
        hideUnhideControls(dtPrimarySales);
    }
  private void hideUnhideControls(DataTable dtPrimarySales)
    {
        dtNew = dtPrimarySales.Clone();
        foreach (DataColumn dc in dtNew.Columns)
        {
            if (dc.DataType == typeof(string) && dc.ColumnName == "Quantity")
            {
                dc.DataType = typeof(System.Int32);
                break;
            }
        }
        foreach (DataRow dr in dtPrimarySales.Rows)
        {
            dtNew.ImportRow(dr);
        }
        objSum = dtNew.Compute("sum(Quantity)", "");
        if (counter == 0)
        {
            Btnsave.Enabled=true;
            GridSales.Columns[7].Visible = false;
            if (Convert.ToInt32(objSum) <= 0)
            {
                Btnsave.Enabled = false;
                ucMsg.ShowInfo("Please Insert right Quantity");
            }
        }
        else
        {
            Btnsave.Enabled = false;
        }
        if (dtPrimarySales.Rows.Count > 0)
        {
            dvUploadPreview.Visible = true;
            pnlGrid.Visible = true;
            Btnsave.Visible = true;
            BtnCancel.Visible = true;
            GridSales.Visible = true;
            
            GridSales.DataSource = dtPrimarySales;
            ViewState["Sales"] = dtPrimarySales;
            GridSales.DataBind();
            updGrid.Update();

        }
        else
            pnlGrid.Visible = false;
    }
    protected void Btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }
            if (ViewState["Sales"] != null)
            {
                DataTable dtPrimarySales = new DataTable();
                DataTable Tvp = new DataTable();

                dtPrimarySales = (DataTable)ViewState["Sales"];
                using (CommonData ObjCommom = new CommonData())
                {
                    Tvp = ObjCommom.GettvpTablePrimarySalesBatch ();  
                }
                foreach (DataRow dr in dtPrimarySales.Rows)
                {
                    DataRow drow = Tvp.NewRow();

                    drow[0] = dr["SalesChannelCode"].ToString();
                    drow[1] = dr["InvoiceNumber"].ToString();
                    drow[2] = dr["InvoiceDate"];
                    drow[3] = dr["SKUCode"].ToString();
                    drow[4] = dr["Quantity"].ToString();
                    drow[5] = PageBase.SalesChanelID;
                    drow[6] = dr["WarehouseCode"].ToString();
                    drow[7] = dr["BatchNumber"].ToString();
                    drow[8] = dr["OrderNumber"].ToString(); 
                    Tvp.Rows.Add(drow);
                }
                Tvp.AcceptChanges();
                using (SalesData objPrimarySales = new SalesData())
                {
                    objPrimarySales.EntryType = EnumData.eEntryType.eUpload;
                    objPrimarySales.SalesChannelID = PageBase.SalesChanelID;
                     objPrimarySales.UploadPrimarySalesInfoBatch (Tvp);

                    if (objPrimarySales.ErrorDetailXML != null && objPrimarySales.ErrorDetailXML != string.Empty)
                    {
                        ucMsg.XmlErrorSource = objPrimarySales.ErrorDetailXML;
                        return;
                    }
                    if (objPrimarySales.Error != null && objPrimarySales.Error != "")
                    {
                        ucMsg.ShowError(objPrimarySales.Error);
                        return;
                    }
                    

                    ClearForm();

                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                }
            }
        }
        catch(Exception ex)
            {
                ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }
   
      
        
    }
    void ClearForm()
    {
        pnlGrid.Visible = false;
        ViewState["Sales"] = null;
        GridSales.DataSource = null;
        GridSales.DataBind();
        updGrid.Update();
    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    { ucMsg.ShowControl = false;
        ClearForm();

    }
}
