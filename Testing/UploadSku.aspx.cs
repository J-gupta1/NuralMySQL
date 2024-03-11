using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Collections;
using DataAccess;
using BussinessLogic;
using System.Windows.Forms;
using System.Data;

public partial class Testing_UploadSku : PageBase
{
    UploadFile UploadFile = new UploadFile();
    DataTable DtSaleschannel;
    DataTable dtNew = new DataTable();
    object objSum;
    string strUploadedFileName = string.Empty;
    int counter = 1;        
    DateTime dt = new DateTime();
    Dictionary<string, string> names = new Dictionary<string, string>();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            fillTableName();
        }
    }
    public void fillTableName()
    {

        names.Add("0", "Select");
        names.Add("Retailer", "Upload Retailer");
        names.Add("Price", "Manage Price");
        names.Add("IntermediarySales", "Upload Primary Sales-2");
        names.Add("PrimarySales", "Upload Primary Sales");
        names.Add("SecondarySalesReturn", "Upload Secondary Sales");
        names.Add("PrimarySalesReturn", "Upload Primary Sales Return");
        names.Add("Warehouse Transaction(Sap-BTM)", "SAP-BTM");
        names.Add("SecondarySalesUpload", "Upload Secondary Sales");
        names.Add("Primary Sales(Sap-MOD)", "SAP-Primary Sales");
        names.Add("IMEI(Sap-IMEI)", "SAP-IMEI");
        names.Add("GRN(Sap-GRN)", "SAP-GRN");
        names.Add("GRN", "GRN");
        names.Add("SalesMan", "Upload SalesMan");
        names.Add("CustomPrice", "Manage Price");
        names.Add("Sku", "Manage SKU");
        cmbMaster.DataSource = new BindingSource(names, null);
        cmbMaster.DataValueField = "Key";
        cmbMaster.DataTextField = "Value";
        cmbMaster.DataBind();
        cmbMaster.SelectedValue = "0";
    }
    protected void cmbMaster_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            importdata();
        }
        catch (Exception ex)
        {

        }
    }
    public void importdata()
    {
        using (ReportData obj = new ReportData())
        {
            obj.TableName = cmbMaster.SelectedValue;
            obj.CompanyType = 0;
            DataTable dt = obj.GetUploadSchemaTable();
            grdSku.DataSource = dt;
            grdSku.DataBind();
            Pnlfrom.Visible = true;
       }
    }

   
    private void InsertData(DataSet objDS)
    {

        DataColumn dcError = new DataColumn();
        dcError.DataType = System.Type.GetType("System.String");
        dcError.ColumnName = "Error";

        if (objDS.Tables[0].Columns.Contains("Error") == false)
            objDS.Tables[0].Columns.Add(dcError);
        objDS.Tables[0].Columns["SalesChannelCode"].ColumnName = "SalesChannelCode";
        for (int i = 0; i <= objDS.Tables[0].Rows.Count - 1; i++)
        {
            if (objDS.Tables[0] != null && objDS.Tables[0].Rows.Count > 0)
            {
                //Same TD/SS can have multiple invoice number
                if (objDS.Tables[0].Rows[i]["SalesChannelCode"] != DBNull.Value)
                {
                    string strWhere = "SalesChannelCode <>'" + objDS.Tables[0].Rows[i]["SalesChannelCode"].ToString().Trim() + "' and InvoiceNumber ='" + objDS.Tables[0].Rows[i]["InvoiceNumber"].ToString().Trim() + "'";
                    DataRow[] dr = objDS.Tables[0].Select(strWhere);
                    if (dr.Length > 0)
                    {
                        counter = counter + 1;
                        if (objDS.Tables[0].Rows[i]["Error"] == DBNull.Value && objDS.Tables[0].Rows[i]["Error"].ToString() == string.Empty)
                        {
                            objDS.Tables[0].Rows[i]["Error"] = "Same invoice number can not have muliple " + Resources.Messages.SalesEntity + " Code";
                        }
                        else
                            objDS.Tables[0].Rows[i]["Error"] = "Same invoice number can not have muliple " + Resources.Messages.SalesEntity + "  Code";
                    }

                }
                //Multiple invoice date with same invoiceNumber
                string strWhere1 = "InvoiceNumber='" + objDS.Tables[0].Rows[i]["InvoiceNumber"].ToString().Trim() + "'and InvoiceDate <>'" + objDS.Tables[0].Rows[i]["InvoiceDate"].ToString().Trim() + "'";
                if (objDS.Tables[0].Rows[i]["InvoiceNumber"] != DBNull.Value)
                {

                    DataRow[] dr = objDS.Tables[0].Select(strWhere1);
                    if (dr.Length > 0)
                    {
                        counter = counter + 1;
                        if (objDS.Tables[0].Rows[i]["Error"] == DBNull.Value && objDS.Tables[0].Rows[i]["Error"] == string.Empty)
                        {
                            objDS.Tables[0].Rows[i]["Error"] = "Same invoice no with multiple dates!";
                        }
                        else
                            objDS.Tables[0].Rows[i]["Error"] += ";Same invoice no with multiple dates!";
                    }
                }

            }

        }




        if (objDS.Tables[0].Rows.Count > 0)
        {
            btnUpload.Visible = true;
            grdUploadedSku.DataSource = objDS.Tables[0];
            grdUploadedSku.DataBind();
        }
        else
        {
            
        }
        if (counter == 1)           //Pankaj Kumar Now Default Value for counter=1
        {
            ViewState["Detail"] = objDS.Tables[0];

            dtNew = objDS.Tables[0].Clone();            //Pankaj Kumar
            foreach (DataColumn dc in dtNew.Columns)
            {
                if (dc.DataType == typeof(string) && dc.ColumnName == "Quantity")
                {
                    dc.DataType = typeof(System.Int32);
                    break;
                }
            }
            foreach (DataRow dr in objDS.Tables[0].Rows)
            {
                dtNew.ImportRow(dr);
            }
            objSum = dtNew.Compute("sum(Quantity)", "");
            if (Convert.ToInt32(objSum) <= 0)
            {
                ucMessage1.ShowInfo("Please insert right Quantity");
                grdUploadedSku.DataSource = objDS.Tables[0];
                grdUploadedSku.DataBind();
                if (ViewState["Detail"] != null)
                    ViewState["Detail"] = null;
               return;
            }

        }
        else
        {
           ViewState["Detail"] = null;
        }
        //updGrid.Update();


    }

    protected void btnUpload_Click1(object sender, EventArgs e)
    {
        DataTable DummyTable = new DataTable();
        DataColumn dc = new DataColumn("ColumnName", typeof(System.String));
        DummyTable.Columns.Add(dc);
        
        try
        {
            OpenXMLExcel obj = new OpenXMLExcel();
            DataSet objDS = new DataSet();
            byte isSuccess = 1;
            Int16 UploadCheck = 0;
            String RootPath = Server.MapPath("../");
            UploadFile.RootFolerPath = RootPath;
            DataSet ds = new DataSet();
            UploadCheck = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            if (UploadCheck == 1)
            {
                ds = obj.ImportExcelFile(RootPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
                foreach (DataColumn dcInner in ds.Tables[0].Columns)
                {
                    DataRow dr = DummyTable.NewRow();
                    dr["ColumnName"] = dcInner.ColumnName;
                    DummyTable.Rows.Add(dr);
                    //DummyTable.Columns["ColumnName"] = dcInner.ColumnName;
                }
                DummyTable.AcceptChanges();
            }
            if (DummyTable.Rows.Count > 0)
            {
                grdUploadedSku.DataSource = DummyTable;
                grdUploadedSku.DataBind();
                return;
            }


            if (UploadCheck == 1)
            {
                ds = obj.ImportExcelFile(RootPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);

                UploadFile.UploadedFileName = strUploadedFileName;
                UploadFile.UploadValidationType = EnumData.eUploadExcelValidationType.eSales;
                isSuccess = UploadFile.uploadValidExcel(ref objDS, "Sku");

                switch (isSuccess)
                {
                    case 0:
                        ucMessage1.ShowInfo(UploadFile.Message);
                        break;
                    case 2:
                        ucMessage1.ShowInfo(Resources.Messages.CheckErrorGrid);
                        grdUploadedSku.Columns[5].Visible = true;
                        objDS.Tables[0].Columns["SalesChannelCode"].ColumnName = "SalesChannelCode";
                        grdUploadedSku.DataSource = objDS;
                        grdUploadedSku.DataBind();
                        break;
                    case 1:
                        InsertData(objDS);
                        break;
                }
            }
            else if (UploadCheck == 2)
            {
                ucMessage1.ShowInfo(Resources.Messages.UploadXlxs);
            }
            else if (UploadCheck == 3)
            {
                ucMessage1.ShowInfo(Resources.Messages.SelectFile);
            }
            else
            {
                ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
}
