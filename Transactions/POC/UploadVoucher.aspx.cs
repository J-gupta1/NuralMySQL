using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Data;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Resources;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;
using System.Text;
using System.Collections.Generic;

public partial class Transactions_POC_UploadVoucher : PageBase
{
    DataSet dsTemplateCode = new DataSet();
    DataTable dtVoucher = new DataTable();
    DataSet dsVoucher = new DataSet();
    
    object objSum;
    string strUploadedFileName = string.Empty;
    int counter = 0;
    UploadFile UploadFile = new UploadFile();
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        if (!IsPostBack)
        {
            DateTime dt = System.DateTime.Now.Date;
        }

    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["Voucher"] != null)
                ViewState["Voucher"] = null;
            ucMsg.Visible = false;
            byte isSuccess = 1;
            Int16 UploadCheck = 0;
            String RootPath = Server.MapPath("../../");
            UploadFile.RootFolerPath = RootPath;

            UploadCheck = UploadFile.IsExcelFile(Fileupdload, ref strUploadedFileName);
            if (UploadCheck == 1)
            {
                UploadFile.UploadedFileName = strUploadedFileName;
                UploadFile.UploadValidationType = EnumData.eUploadExcelValidationType.eSales;
                isSuccess = UploadFile.uploadValidExcel(ref dsVoucher, "VoucherUpload");

                switch (isSuccess)
                {
                    case 0:
                        ucMsg.ShowInfo(UploadFile.Message);
                        gvVoucher.Visible = true;
                        gvVoucher.DataSource = dsVoucher;
                        gvVoucher.DataBind();
                        if (ViewState["Voucher"] != null)
                            ViewState["Voucher"] = null;
                        break;
                    case 2:
                        ucMsg.ShowInfo(Resources.Messages.CheckErrorGrid);
                        pnlGrid1.Visible = true;
                        gvVoucher.Columns[7].Visible = true;
                        gvVoucher.Visible = true;
                        gvVoucher.DataSource = dsVoucher;
                        gvVoucher.DataBind();
                        if (ViewState["Voucher"] != null)
                            ViewState["Voucher"] = null;
                        break;
                    case 1:
                        InsertData(dsVoucher);

                        break;
                }
            }
            else if (UploadCheck == 2)
            {
                ucMsg.ShowInfo(Resources.Messages.UploadXlxs);
                gvVoucher.DataSource = null;
                gvVoucher.DataBind();
                pnlGrid1.Visible = false;

            }
            else if (UploadCheck == 3)
            {
                ucMsg.ShowInfo(Resources.Messages.SelectFile);
                gvVoucher.DataSource = null;
                gvVoucher.DataBind();
                pnlGrid1.Visible = false;

            }
            else
            {
                ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                gvVoucher.DataSource = null;
                gvVoucher.DataBind();
                pnlGrid1.Visible = false;

            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearForm();
        pnlGrid1.Visible = false;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (IsPageRefereshed == true)
        {
            return;
        }
        int Result = 0;
        try
        {
            if (ViewState["Voucher"] != null)
            {
                dtVoucher = (DataTable)ViewState["Voucher"];
                if (dtVoucher.Columns.Contains("Error") == true)
                    dtVoucher.Columns.Remove("Error");

                DataTable DtDetail = new DataTable();
                using (CommonData ObjCommom = new CommonData())
                {
                    DtDetail = ObjCommom.GettvpUploadVoucher();
                }

                foreach (DataRow dr in dtVoucher.Rows)
                {

                    DataRow drow = DtDetail.NewRow();
                    drow[0] = Convert.ToDateTime(dr["VoucherDate"]);
                    drow[1] = dr["VoucherType"].ToString();
                    drow[2] = dr["DocNo"];
                    drow[3] = dr["PartyCode"];
                    drow[4] = dr["Amount"].ToString();
                    drow[5] = dr["Narration"].ToString();
                    drow[6] = dr["VoucherNumber"].ToString();
                    drow[7] = PageBase.SalesChanelID;
                    drow[8] = PageBase.UserId;
                    DtDetail.Rows.Add(drow);
                }
                DtDetail.AcceptChanges();

                SapService objVoucher = new SapService();
                

                    objVoucher.Error = "";
                    Result = objVoucher.InsertVourcherInfoUpload(DtDetail);

                    if (objVoucher.ErrorDetailXML != null && objVoucher.ErrorDetailXML != string.Empty)
                    {
                        ucMsg.XmlErrorSource = objVoucher.ErrorDetailXML;
                        btnSave.Enabled = false;
                        return;
                    }
                    //else if (ObjSales.Error != null && ObjSales.Error != "" && ObjSales.Error != "0")
                    //{
                    //    ucMsg.ShowError(ObjSales.Error);
                    //    return;
                    //}
                    else if (objVoucher.Error != null && objVoucher.Error != "" && (Result == 4 || Result == 5))
                    {
                        ucMsg.ShowInfo(objVoucher.Error);
                        btnSave.Enabled = false;
                        return;
                    }
                    //if (Result == 4)
                    //{
                    //    ucMsg.ShowError(ObjSales.Error);
                    //    return;
                    //}
                    ClearForm();

                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    if (ViewState["Voucher"] != null)         //Clearing the viewstate data
                        ViewState["Voucher"] = null;

                

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
    private void InsertData(DataSet dsVoucher)
    {
        counter = 0;
        DataColumn dcError = new DataColumn();
        dcError.DataType = System.Type.GetType("System.String");
        dcError.ColumnName = "Error";

        if (dsVoucher.Tables[0].Columns.Contains("Error") == false)
            dsVoucher.Tables[0].Columns.Add(dcError);

        gvVoucher.Columns[7].Visible = true;
        if (counter > 0)
        {
            ucMsg.ShowInfo("File could not be uploaded,Please try again");
            btnSave.Enabled = false;
            btnSave.Visible = true;
            btnCancel.Visible = true;
            pnlGrid1.Visible = true;
            gvVoucher.DataSource = dsVoucher.Tables[0];
            gvVoucher.DataBind();
            return;

        }
        else
        {

            objSum = dsVoucher.Tables[0].Compute("sum(Amount)", "");
            if (Convert.ToInt32(objSum) <= 0)
            {
                btnSave.Enabled = false;
                ucMsg.ShowInfo("Please insert right Amount");
                gvVoucher.DataSource = dsVoucher.Tables[0];
                gvVoucher.DataBind();
                if (ViewState["Voucher"] != null)
                    ViewState["Voucher"] = null;

                pnlGrid1.Visible = true;
                btnCancel.Visible = true;
                btnSave.Visible = true;
                return;
            }
            pnlGrid1.Visible = true;
            btnSave.Visible = true;
            btnSave.Enabled = true;
            btnCancel.Visible = true;
            ViewState["Voucher"] = dsVoucher.Tables[0];
            gvVoucher.DataSource = dsVoucher.Tables[0];
            gvVoucher.DataBind();
            gvVoucher.Columns[7].Visible = false;
        }

    }
    private void ClearForm()
    {
        ucMsg.Visible = false;
        btnSave.Visible = false;
        btnCancel.Visible = false;
        pnlGrid1.Visible = false;

        if (ViewState["Voucher"] != null)
            ViewState["Voucher"] = null;
    }
  
}