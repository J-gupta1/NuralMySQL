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
public partial class Transactions_POC_MaterialIssueOutwardBasedonType : PageBase
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
            bindWarehouse();
            
            ddlUser.Items.Insert(0, new ListItem("Select","0"));
        }
    }
    protected void DwnldReferenceCodeTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsReferenceCode = new DataSet();
            using (ProductData objProductData = new ProductData())
            {
                objProductData.SalesChannelID = PageBase.SalesChanelID;
                dsReferenceCode = objProductData.SelectSkuDataBasedonType();
                if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
                {
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dsReferenceCode, FilenameToexport);

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
            GridGRN.DataSource = (DataTable)ViewState["MaterialIssue"];

            GridGRN.DataBind();
        }

        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    private void InsertData(DataSet objDS)
    {
        ViewState["MaterialIssue"] = null;
        try
        {
            
            DataColumn dc = new DataColumn();
            dc.ColumnName = "ERRORMessage";
            dc.DataType = typeof(string);

            if (objDS.Tables[0].Columns.Contains("ERRORMessage") == false)
                objDS.Tables[0].Columns.Add(dc);
            pnlGrid.Visible = true;
            if ((objDS.Tables[0].Select("isnull(BatchNumber,'')<>''").Length == 0))
                GridGRN.Columns[2].Visible = false;
            else
                GridGRN.Columns[2].Visible = true;
            if ((objDS.Tables[0].Select("isnull(SerialNumber,'')<>''").Length == 0))
                GridGRN.Columns[3].Visible = false;
            else
                GridGRN.Columns[3].Visible = true;

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
                    ViewState["MaterialIssue"] = objDS.Tables[0];
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

        if (ucDateForGRN.Date != "")
        {
            if (Convert.ToDateTime(ucDateForGRN.Date) > Convert.ToDateTime(ucDateForGRN.Date))
            {
                ucMsg.ShowInfo("Invoice Date can't be greater than Grn Date");
                return false;
            }
        }
        if (txtDocketNo.Text != "")
        {
            if (ucDateForGRN.Date == "")
            {
                ucMsg.ShowInfo("Please insert Invoice Date for invoice Number");
                return false;
            }
        }

        if (ucDateForGRN.Date != "")
        {
            if (txtDocketNo.Text == "")
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
        txtDocketNo.Text = "";
        ddlmodeofReceipt.SelectedValue = "0";
        ddlUser.SelectedValue = "0";
        ddlUser.Enabled = false;
        txtCourierName.Text = "";
        ucDateForGRN.Date = "";
        //ucDateForGCN.Date = "";
        txtRemarks.Text = "";
        ddlWarehouse.SelectedIndex = 0;
        GridGRN.DataSource = null;
        GridGRN.DataBind();
        updGrid.Update();
        pnlGrid.Visible = false;
        ViewState["MaterialIssue"] = null;
        txtCourierName.Enabled = false;
    }


    protected void BtnUpload_Click(object sender, EventArgs e)
    {
        if (!PageValidatesave())
        {
            return;
        }
        if (Convert.ToInt32(ddlmodeofReceipt.SelectedValue) == 2 && txtCourierName.Text.Trim() == "")
        {
            pnlGrid.Visible = false;
            ucMsg.ShowInfo("Please insert the courier Name");
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
                isSuccess = UploadFile.uploadValidExcel(ref dsGRN, "GRNBatchOrSerial");

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
            if (Validation()==false)
            {
                ucMsg.ShowInfo("Please fill required information");
                return;
            }
            if (IsPageRefereshed == true)
            {
                return;
            }
            if (ViewState["MaterialIssue"] != null)
            {
                int intResult = 0;
                DataTable Tvp = new DataTable();

                dtGrnUpload = (DataTable)ViewState["MaterialIssue"];
                using (POC ObjCommom = new POC())
                {
                    Tvp = ObjCommom.GettvpTableMaterialIssueBatchOrSerialWise();
                }

                foreach (DataRow dr in dtGrnUpload.Rows)
                {
                    DataRow drow = Tvp.NewRow();
                    drow[0] = Convert.ToInt32(ddlWarehouse.SelectedValue);
                    drow[1] = txtDocketNo.Text.Trim();
                    drow[2] = Convert.ToDateTime(ucDateForGRN.Date);
                    drow[3] = Convert.ToInt32(ddlUser.SelectedValue);
                    drow[4] = txtRemarks.Text.Trim();
                    drow[5] = dr["Quantity"].ToString();
                    drow[6] = ddlmodeofReceipt.SelectedValue;
                    drow[7] = txtCourierName.Text.Trim();
                    drow[8] = dr["skuCode"].ToString();
                    drow[9] = dr["SerialNumber"].ToString();
                    drow[10] = dr["BatchNumber"].ToString();
                    drow[11] = PageBase.UserId;
                    Tvp.Rows.Add(drow);
                }
                Tvp.AcceptChanges();
                using (POC objGrnUpload = new POC())
                {
                    intResult = objGrnUpload.InsertInfoMaterialIssueBatchWiseOrSerialWise(Tvp);

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
    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlWarehouse_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            using (ProductData objProductData = new ProductData())
            {
                objProductData.SalesChannelID = Convert.ToInt32(ddlWarehouse.SelectedValue);
                dt = objProductData.SelectUserBasedonSalesChannelID();
                String[] StrCol = new String[] { "UserID", "UserName" };
                if (dt != null && dt.Rows.Count > 0)
                {
                    PageBase.DropdownBinding(ref ddlUser, dt, StrCol);
                    ddlUser.Enabled = true;
                }
                else
                    ddlUser.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    private bool Validation()
    {
        if (txtDocketNo.Text.Trim() == "" || ucDateForGRN.Date == "" || ddlUser.SelectedValue == "0" || (Convert.ToInt32(ddlmodeofReceipt.SelectedValue) == 2 && txtCourierName.Text.Trim() == ""))
        {
            return false;
        }
        return true;
    }
}

