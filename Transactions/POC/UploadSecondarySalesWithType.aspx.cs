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

public partial class Transactions_POC_UploadSecondarySalesWithType : PageBase
{
    DataSet dsTemplateCode = new DataSet();
    DataTable dtSecondarySalesData = new DataTable();
    DataSet dsSecondarySales = new DataSet();
    DataTable dtNew = new DataTable();
    object objSum;
    string strUploadedFileName = string.Empty;
    int counter = 0;
    UploadFile UploadFile = new UploadFile();
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        if (!IsPostBack)
        {
            //lblInfo.Text = "(Only " + PageBase.intBackDaysAllowForTD.ToString().Replace("-", "") + " days back date sales Allowed)";
            lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
        }

    }
    //protected void DwnldRetailerCodeTemplate_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        using (SalesChannelData objSalesData = new SalesChannelData())
    //        {
    //            objSalesData.UserID = PageBase.UserId;
    //            objSalesData.SalesChannelID = PageBase.SalesChanelID;
    //            objSalesData.Type = 5;
    //            dtRetailerCode = objSalesData.GetAllTemplateData();
    //            if (dtRetailerCode.Rows.Count > 0)
    //            {
    //                DataSet dtcopy = new DataSet();
    //                dtcopy.Merge(dtRetailerCode);
    //                String FilePath = Server.MapPath("../../");
    //                string FilenameToexport = "Retailer Code List";
    //                PageBase.RootFilePath = FilePath;
    //                PageBase.ExportToExecl(dtcopy, FilenameToexport);
    //            }
    //            else
    //            {
    //                ucMsg.ShowInfo(Resources.Messages.NoRecord);

    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
    //        //PageBase.Errorhandling(ex);
    //    }
    //}
    //protected void DwnldSKUCodeTemplate_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        using (SalesChannelData objSalesData = new SalesChannelData())
    //        {
    //            objSalesData.UserID = PageBase.UserId;
    //            objSalesData.Type = 2;
    //            objSalesData.SalesChannelID = PageBase.SalesChanelID;
    //            dtRetailerCode = objSalesData.GetAllTemplateData();
    //            if (dtRetailerCode.Rows.Count > 0)
    //            {
    //                DataSet dtcopy = new DataSet();
    //                dtcopy.Merge(dtRetailerCode);
    //                String FilePath = Server.MapPath("../../");
    //                string FilenameToexport = "SKU Code List";
    //                PageBase.RootFilePath = FilePath;
    //                PageBase.ExportToExecl(dtcopy, FilenameToexport);
    //            }
    //            else
    //            {
    //                ucMsg.ShowInfo(Resources.Messages.NoRecord);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
    //       // PageBase.Errorhandling(ex);
    //    }
    //}
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["SecondarySales"] != null)
                ViewState["SecondarySales"] = null;
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
                isSuccess = UploadFile.uploadValidExcel(ref dsSecondarySales, "SecondarySalesUpload");

                switch (isSuccess)
                {
                    case 0:
                        ucMsg.ShowInfo(UploadFile.Message);
                        gvSecondarySales.Visible = true;
                        gvSecondarySales.DataSource = dsSecondarySales;
                        gvSecondarySales.DataBind();
                        if (ViewState["SecondarySales"] != null)
                            ViewState["SecondarySales"] = null;
                        break;
                    case 2:
                        ucMsg.ShowInfo(Resources.Messages.CheckErrorGrid);
                        pnlGrid1.Visible = true;
                        gvSecondarySales.Columns[4].Visible = true;
                        gvSecondarySales.Visible = true;
                        gvSecondarySales.DataSource = dsSecondarySales;
                        gvSecondarySales.DataBind();
                        if (ViewState["SecondarySales"] != null)
                            ViewState["SecondarySales"] = null;
                        break;
                    case 1:
                        InsertData(dsSecondarySales);

                        break;
                }
            }
            else if (UploadCheck == 2)
            {
                ucMsg.ShowInfo(Resources.Messages.UploadXlxs);
                gvSecondarySales.DataSource = null;
                gvSecondarySales.DataBind();
                pnlGrid1.Visible = false;

            }
            else if (UploadCheck == 3)
            {
                ucMsg.ShowInfo(Resources.Messages.SelectFile);
                gvSecondarySales.DataSource = null;
                gvSecondarySales.DataBind();
                pnlGrid1.Visible = false;

            }
            else
            {
                ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                gvSecondarySales.DataSource = null;
                gvSecondarySales.DataBind();
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
            if (ViewState["SecondarySales"] != null)
            {
                dtSecondarySalesData = (DataTable)ViewState["SecondarySales"];
                if (dtSecondarySalesData.Columns.Contains("Error") == true)
                    dtSecondarySalesData.Columns.Remove("Error");

                DataTable DtDetail = new DataTable();
                using (CommonData ObjCommom = new CommonData())
                {
                    DtDetail = ObjCommom.GettvpTableSecondarySales();
                }

                foreach (DataRow dr in dtSecondarySalesData.Rows)
                {

                    DataRow drow = DtDetail.NewRow();
                    drow[0] = 0;
                    drow[1] = dr["RetailerCode"].ToString();
                    drow[2] = dr["SalesDate"];
                    drow[3] = dr["SKUCode"];
                    drow[4] = dr["Quantity"].ToString();
                    drow[5] = ""; ;
                    drow[6] = PageBase.SalesChanelID;
                    drow[7] = 0;
                    DtDetail.Rows.Add(drow);
                }
                DtDetail.AcceptChanges();

                using (SalesData ObjSales = new SalesData())
                {

                    ObjSales.Error = "";
                    ObjSales.EntryType = EnumData.eEntryType.eUpload;
                    Result = ObjSales.InsertUpdateSecondarySalesInfoUpload(DtDetail);

                    if (ObjSales.ErrorDetailXML != null && ObjSales.ErrorDetailXML != string.Empty)
                    {
                        ucMsg.XmlErrorSource = ObjSales.ErrorDetailXML;
                        btnSave.Enabled = false;
                        return;
                    }
                    //else if (ObjSales.Error != null && ObjSales.Error != "" && ObjSales.Error != "0")
                    //{
                    //    ucMsg.ShowError(ObjSales.Error);
                    //    return;
                    //}
                    else if (ObjSales.Error != null && ObjSales.Error != "" && (Result == 4 || Result == 5))
                    {
                        ucMsg.ShowInfo(ObjSales.Error);
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
                    if (ViewState["SecondarySales"] != null)         //Clearing the viewstate data
                        ViewState["SecondarySales"] = null;

                }

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
    private void InsertData(DataSet dsSecondarySales)
    {
        counter = 0;
        //DataTable DtDetail;
        //using (CommonData ObjCommom = new CommonData())
        //{
        //    DtDetail = ObjCommom.GettvpTableSecondarySales();
        //}

        //foreach (DataRow dr in dsSecondarySales.Tables[0].Rows)
        //{

        //    DataRow drow = DtDetail.NewRow();
        //    drow[0] = 0;
        //    drow[1] = dr["RetailerCode"].ToString();
        //    drow[2] = dr["SalesDate"];
        //    drow[3] = dr["SKUCode"];
        //    drow[4] = dr["Quantity"].ToString();
        //    drow[5] = ""; ;
        //    drow[6] = PageBase.SalesChanelID;
        //    DtDetail.Rows.Add(drow);
        //}
        //DtDetail.AcceptChanges();
        DataColumn dcError = new DataColumn();
        dcError.DataType = System.Type.GetType("System.String");
        dcError.ColumnName = "Error";

        if (dsSecondarySales.Tables[0].Columns.Contains("Error") == false)
            dsSecondarySales.Tables[0].Columns.Add(dcError);

        for (int i = 0; i <= dsSecondarySales.Tables[0].Rows.Count - 1; i++)
        {
            // string strWhere = "SalesChannelCode='" + DtDetail.Rows[i]["SalesChannelCode"].ToString().Trim() + "'and SalesMan <>'" + DtDetail.Rows[i]["SalesMan"].ToString().Trim() + "'";
            //if (DtDetail.Rows[i]["SalesChannelCode"] != DBNull.Value)
            //{
            //DataRow[] dr = DtDetail.Select(strWhere);
            //if (dr.Length > 0)
            //{
            //    counter = counter + 1;
            //    if (DtDetail.Rows[i]["Error"] == DBNull.Value && Convert.ToString(DtDetail.Rows[i]["Error"]) == string.Empty)
            //    {
            //        DtDetail.Rows[i]["Error"] = "Retailer Can not have multiple salesman!";
            //    }
            //    else
            //        DtDetail.Rows[i]["Error"] += ";Retailer Can not have multiple salesman!";
            //}
            DateTime dtInvoiceDate = Convert.ToDateTime(dsSecondarySales.Tables[0].Rows[i]["SalesDate"]);
            TimeSpan ts = dtInvoiceDate.Subtract(System.DateTime.Now.Date);
            if (ts.Days > 0)
            {
                counter = counter + 1;
                if (dsSecondarySales.Tables[0].Rows[i]["Error"] != DBNull.Value)
                    dsSecondarySales.Tables[0].Rows[i]["Error"] = " Invoice date should not be greater than current date!";
                else
                    dsSecondarySales.Tables[0].Rows[i]["Error"] += ";Invoice date should not be greater than current date!";
            }
            if (Convert.ToDateTime(DateTime.Now.Date).AddDays(PageBase.NumberofBackDaysAllowed) > Convert.ToDateTime(dsSecondarySales.Tables[0].Rows[i]["SalesDate"]))
            {
                counter = counter + 1;
                if (dsSecondarySales.Tables[0].Rows[i]["Error"] == DBNull.Value && dsSecondarySales.Tables[0].Rows[i]["Error"] == string.Empty)
                {
                    dsSecondarySales.Tables[0].Rows[i]["Error"] = "InValid Date!";
                }
                else
                    dsSecondarySales.Tables[0].Rows[i]["Error"] += ";InValid Date!";

            } //Pankaj Kumar

        }
        gvSecondarySales.Columns[4].Visible = true;
        if (counter > 0)
        {
            ucMsg.ShowInfo("File could not be uploaded,Please try again");
            btnSave.Enabled = false;
            btnSave.Visible = true;
            btnCancel.Visible = true;
            pnlGrid1.Visible = true;
            gvSecondarySales.DataSource = dsSecondarySales.Tables[0];
            gvSecondarySales.DataBind();
            return;

        }
        else
        {

            dtNew = dsSecondarySales.Tables[0].Clone();
            foreach (DataColumn dc in dtNew.Columns)
            {
                if (dc.DataType == typeof(string) && dc.ColumnName == "Quantity")
                {
                    dc.DataType = typeof(System.Int32);
                }
                if (dc.DataType == typeof(string) && dc.ColumnName == "SalesDate")        //Pankaj Kumar
                {
                    dc.DataType = typeof(System.DateTime);
                }
            }
            foreach (DataRow dr in dsSecondarySales.Tables[0].Rows)
            {
                dtNew.ImportRow(dr);
            }
            objSum = dtNew.Compute("sum(Quantity)", "");
            if (Convert.ToInt32(objSum) <= 0)
            {
                btnSave.Enabled = false;
                ucMsg.ShowInfo("Please insert right Quantity");
                gvSecondarySales.DataSource = dsSecondarySales.Tables[0];
                gvSecondarySales.DataBind();
                if (ViewState["SecondarySales"] != null)
                    ViewState["SecondarySales"] = null;

                pnlGrid1.Visible = true;
                btnCancel.Visible = true;
                btnSave.Visible = true;
                return;
            }
            pnlGrid1.Visible = true;
            btnSave.Visible = true;
            btnSave.Enabled = true;
            btnCancel.Visible = true;
            ViewState["SecondarySales"] = dsSecondarySales.Tables[0];
            gvSecondarySales.DataSource = dtNew;
            gvSecondarySales.DataBind();
            gvSecondarySales.Columns[4].Visible = false;
        }

    }
    private void ClearForm()
    {
        ucMsg.Visible = false;
        btnSave.Visible = false;
        btnCancel.Visible = false;
        pnlGrid1.Visible = false;

        if (ViewState["SecondarySales"] != null)
            ViewState["SecondarySales"] = null;
    }
    protected void rdModelList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdModelList.SelectedValue == "1")
            Response.Redirect("SecondarySalesInterfaceWithType.aspx");
    }
 

    protected void DwnldReferenceCodeTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            using (SalesChannelData objSalesData = new SalesChannelData())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.eSecondarySales;
                objSalesData.SalesChannelID = PageBase.SalesChanelID;

                dsTemplateCode = objSalesData.GetAllTemplateDataWithType();
                if (dsTemplateCode!=null && dsTemplateCode.Tables.Count>0)
                {
                    
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dsTemplateCode, FilenameToexport,EnumData.eTemplateCount.eSecondary);
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
}
