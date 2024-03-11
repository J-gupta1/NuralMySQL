using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;
using System.Data.SqlClient;

//POC---Proof of Concept
public partial class Transactions_SalesChannel_GRNUploadBatchWise : PageBase
{
    int counter = 0;
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    string abc = "";
    string saleschannelcode;
    DataTable dtNew = new DataTable();
    object objSum;
    SqlConnection objConnection = new SqlConnection();
    SalesData objGRN = new SalesData();
    UploadFile UploadFile = new UploadFile();
    protected void Page_Load(object sender, EventArgs e)
    {
        ucDateForGRN.MaxRangeValue = System.DateTime.Now;
        ucDateForGRN.RangeErrorMessage = "Date Can't be more than current Date.";
        ucMsg.Visible = false;
        if (!IsPostBack)
        {
            if (Convert.ToInt16(Session["AsForVendor"]) == 1)
            {
                pnlVendor.Visible = true;
            }
            bindWarehouse();

        }
    }
    protected void DwnldReferenceCodeTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt;
            DataSet ds = new DataSet();
            DataSet dsReferenceCode = new DataSet();
            using (ProductData objProductData = new ProductData())
            {
                dt = objProductData.SelectAllSKUInfo();
                string[] strCode = new string[] { "SKUCode", "SKUName" };
                dt = dt.DefaultView.ToTable(true, strCode);

                ds.Tables.Add(dt);

                dsReferenceCode = ds;
                if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
                {
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
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
    }
    protected void DwnldWarehouseTemplate_Click(object sender, EventArgs e)
    {
        DataTable dt;
        DataSet ds = new DataSet();
        DataSet dsReferenceCode = new DataSet();
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.SalesChannelTypeID = 5;

            dt = ObjSalesChannel.GetSalesChannelInfo();
            string[] strCode = new string[] { "SalesChannelCode" };
            dt = dt.DefaultView.ToTable(true, strCode);
            dt.Columns["SalesChannelCode"].ColumnName = "WareHouseCode";
            ds.Tables.Add(dt);

            dsReferenceCode = ds;
            if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
            {
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "/2 List";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dsReferenceCode, FilenameToexport, EnumData.eTemplateCount.ePrimarysales1);
            }
            else
            {
                ucMsg.ShowInfo(Resources.Messages.NoRecord);
            }

        }
    }
    protected void GridGRN_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridGRN.PageIndex = e.NewPageIndex;
            DataTable dt = new DataTable();
            GridGRN.DataSource = (DataTable)ViewState["GrnBatchWise"];

            GridGRN.DataBind();
        }

        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    private void InsertData(DataSet objDS)
    {
        ViewState["GrnBatchWise"] = null;
        try
        {
            DataColumn dc = new DataColumn();
            dc.ColumnName = "ERRORMessage";
            dc.DataType = typeof(string);

            if (objDS.Tables[0].Columns.Contains("ERRORMessage") == false)
                objDS.Tables[0].Columns.Add(dc);
            pnlGrid.Visible = true;
            if (objDS.Tables[0].Rows.Count > 0)
            {
                objSum = objDS.Tables[0].Compute("sum(Quantity)", "");
                if (Convert.ToInt32(objSum) <= 0)
                {
                    Btnsave.Enabled = false;
                    GridGRN.DataSource = objDS.Tables[0];
                    GridGRN.DataBind();
                    ucMsg.ShowInfo("Please Insert right Quantity");

                }
                else
                {
                    Btnsave.Enabled = true;
                    GridGRN.Columns[4].Visible = false;
                    GridGRN.DataSource = objDS.Tables[0];
                    ViewState["GrnBatchWise"] = objDS.Tables[0];
                    GridGRN.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        ClearControls();
        //Btnsave.Enabled = true;

    }
    private bool PageValidatesave()
    {

        if (ucDateForInvoice.Date != "")
        {
            if (Convert.ToDateTime(ucDateForInvoice.Date) > Convert.ToDateTime(ucDateForGRN.Date))
            {
                ucMsg.ShowInfo("Invoice Date can't be greater than Grn Date");
                return false;
            }
        }
        if (txtInvoiceNumber.Text != "")
        {
            if (ucDateForInvoice.Date == "")
            {
                ucMsg.ShowInfo("Please insert Invoice Date for invoice Number");
                return false;
            }
        }

        if (ucDateForInvoice.Date != "")
        {
            if (txtInvoiceNumber.Text == "")
            {
                ucMsg.ShowInfo("Please insert Invoice Number for invoice Date");
                return false;
            }
        }
        return true;
    }

    void bindWarehouse()
    {
        DataTable dt = new DataTable();
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.SalesChannelTypeID = 5;

            dt = ObjSalesChannel.GetSalesChannelInfo();
            string[] colArray = { "SalesChannelID", "SalesChannelName" };
            PageBase.DropdownBinding(ref ddlWarehouse, dt, colArray);
            if (PageBase.SalesChanelID != 0)
            {
                ddlWarehouse.SelectedValue = Convert.ToString(PageBase.SalesChanelID);
                ddlWarehouse.Enabled = false;
            }
        }
    }

    void ClearControls()
    {
        txtInvoiceNumber.Text = "";
        txtGRNNumber.Text = "";
        ucDateForGRN.Date = "";
        ucDateForInvoice.Date = "";
        txtRemarks.Text = "";
        ddlWarehouse.SelectedIndex = 0;
        GridGRN.DataSource = null;
        GridGRN.DataBind();
        updGrid.Update();
        pnlGrid.Visible = false;
        ViewState["GrnBatchWise"] = null;
    }

  
    protected void BtnUpload_Click(object sender, EventArgs e)
    {
        if (!PageValidatesave())
        {
            return;
        }
        try
        {
            DataSet dsGRN = null;
            byte isSuccess = 1;
            Int16 UploadCheck = 0;
            String RootPath = Server.MapPath("../../");
            UploadFile.RootFolerPath = RootPath;
            UploadCheck = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            if (UploadCheck == 1)
            {
                UploadFile.UploadedFileName = strUploadedFileName;
                UploadFile.UploadValidationType = EnumData.eUploadExcelValidationType.eSales;
                isSuccess = UploadFile.uploadValidExcel(ref dsGRN, "GRNUploadBatchWise");

                switch (isSuccess)
                {
                    case 0:
                        ucMsg.ShowInfo(UploadFile.Message);
                        break;
                    case 2:
                        ucMsg.ShowInfo(Resources.Messages.CheckErrorGrid);
                        Btnsave.Enabled = false;
                        dsGRN.Tables[0].Columns["Error"].ColumnName = "ERRORMessage";
                        GridGRN.DataSource = dsGRN;
                        GridGRN.DataBind();
                        GridGRN.Visible = true;
                        pnlGrid.Visible = true;
                        break;
                    case 1:
                        InsertData(dsGRN);
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
            pnlGrid.Visible = false;
            ucMsg.ShowError(ex.Message.ToString());
        }
    }
    protected void Btnsave_Click(object sender, EventArgs e)
    {
        Btnsave.Enabled = true;
        DataTable dtGrnUpload = new DataTable();
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }
            if (ViewState["GrnBatchWise"] != null)
            {
                int intResult = 0;
                DataTable Tvp = new DataTable();

                dtGrnUpload = (DataTable)ViewState["GrnBatchWise"];
                using (POC ObjCommom = new POC())
                {
                    Tvp = ObjCommom.GettvpTableGRNUploadBatchWise();
                }

                foreach (DataRow dr in dtGrnUpload.Rows)
                {
                    DataRow drow = Tvp.NewRow();
                    drow[0] = Convert.ToInt32(ddlWarehouse.SelectedValue);
                    drow[1] = txtGRNNumber.Text.Trim();
                    drow[2] = Convert.ToDateTime(ucDateForGRN.Date);
                    drow[3] = txtInvoiceNumber.Text.Trim();
                    drow[4] = Convert.ToDateTime(ucDateForInvoice.Date);
                    drow[5] = dr["SKUCode"].ToString();
                    drow[6] = dr["Quantity"].ToString();
                    drow[7] = dr["BatchNumber"].ToString();
                    drow[8] = "";
                    drow[9] = DateTime.MinValue;                //Dummy value nothing else
                    drow[10] = txtRemarks.Text.Trim();
                    drow[11] = PageBase.SalesChanelID;
                    drow[12] = dr["SerialNumber"].ToString();
                    Tvp.Rows.Add(drow);
                }
                Tvp.AcceptChanges();
                using (POC objGrnUpload = new POC())
                {
                    intResult = objGrnUpload.InsertInfoGRNUploadBatchWise(Tvp);

                    if (objGrnUpload.ErrorDetailXML != null && objGrnUpload.ErrorDetailXML != string.Empty)
                    {
                        ucMsg.XmlErrorSource = objGrnUpload.ErrorDetailXML;
                        Btnsave.Enabled = false;
                        return;
                    }
                    if (objGrnUpload.Error != null && objGrnUpload.Error != "")
                    {
                        ucMsg.ShowError(objGrnUpload.Error);
                        Btnsave.Enabled = false;
                        return;
                    }
                    if (intResult == 2)
                    {
                        ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                        Btnsave.Enabled = false;
                        return;
                    }
                    ClearControls();
                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearControls();
    }
}

