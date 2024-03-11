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


/// <summary>
/// Created By : Pankaj Kumar
/// </summary>
public partial class Transactions_SalesChannel_UploadPrimarySales2 : PageBase
{
    DataSet DsReferenceCode = new DataSet();
    DataSet  dsSales= new DataSet();
    DataTable dtSales = new DataTable();
    DataTable dtNew = new DataTable();
    object objSum;
    string strUploadedFileName = string.Empty;
    int counter = 1;
    UploadFile UploadFile = new UploadFile();

    protected void Page_Load(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        if (!IsPostBack)
        {
            if (Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening) >= Convert.ToDateTime(DateTime.Now.Date.AddDays(PageBase.NumberofBackDaysAllowed)))
            {

                lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", (Convert.ToDateTime(DateTime.Now.Date.ToShortDateString()) - Convert.ToDateTime(Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening).ToShortDateString())).TotalDays.ToString());
            }
            else
            {

                lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
            }
            //lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-",""));
            btnSave.Enabled = false;
            pnlGrid.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = false;
        }

    }
    //protected void DwnldTDCodeTemplate_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        using (SalesChannelData objSalesData = new SalesChannelData())
    //        {
    //            objSalesData.UserID = PageBase.UserId;
    //            objSalesData.Type = 1;          //1-TD Code download  2-SKU Download  3-Download Sales Template 4-Download Salesman 
    //            dtTDCode = objSalesData.GetAllTemplateData();
    //            if (dtTDCode.Rows.Count > 0)
    //            {
    //                DataSet dtcopy = new DataSet();
    //                dtcopy.Merge(dtTDCode);
    //                String FilePath = Server.MapPath("../../");
    //                string FilenameToexport = "TDCode List";
    //                PageBase.RootFilePath = FilePath;
    //                PageBase.ExportToExecl(dtcopy, FilenameToexport);
    //            }
    //            else
    //            {
    //                ucMsg.ShowError(Resources.Messages.NoRecord);

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
    //            objSalesData.Type = 2;          //1-TD Code download  2-SKU Download  3-Download Sales Template 4-Download Salesman 
    //            dtTDCode = objSalesData.GetAllTemplateData();
    //            if (dtTDCode.Rows.Count > 0)
    //            {
    //                DataSet dtcopy = new DataSet();
    //                dtcopy.Merge(dtTDCode);
    //                String FilePath = Server.MapPath("../../");
    //                string FilenameToexport = "TDCode List";
    //                PageBase.RootFilePath = FilePath;
    //                PageBase.ExportToExecl(dtcopy, FilenameToexport);

    //            }
    //            else
    //            {
    //                ucMsg.ShowError(Resources.Messages.NoRecord);

    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
    //        //PageBase.Errorhandling(ex);
    //    }

    //}
    protected void DwnldReferenceCodeTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            using (SalesChannelData objSalesData = new SalesChannelData())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.ePrimary2Sales;
                objSalesData.SalesChannelID = PageBase.SalesChanelID;
                objSalesData.Brand = PageBase.Brand;
                DsReferenceCode = objSalesData.GetAllTemplateDataMicromax();
                if (DsReferenceCode != null && DsReferenceCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(DsReferenceCode, FilenameToexport, EnumData.eTemplateCount.ePrimarysales2);
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
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["PrimarySales2"] != null)
                ViewState["PrimarySales2"] = null;
            ucMsg.Visible =false;
            byte isSuccess = 1;
            Int16 UploadCheck = 0;
            String RootPath = Server.MapPath("../../");
            UploadFile.RootFolerPath = RootPath;
       
            UploadCheck = UploadFile.IsExcelFile(Fileupdload, ref strUploadedFileName);
            if (UploadCheck == 1)
            {
                UploadFile.UploadedFileName = strUploadedFileName;
                UploadFile.UploadValidationType = EnumData.eUploadExcelValidationType.eSales;
                isSuccess = UploadFile.uploadValidExcel(ref dsSales, "IntermediarySalesWithoutOrder");

                switch (isSuccess)
                {
                    case 0:
                        ucMsg.ShowInfo(UploadFile.Message);
                        break;
                    case 2:
                        ucMsg.ShowInfo(Resources.Messages.CheckErrorGrid);
                        btnSave.Enabled = false;
                        gvSales.Visible = true;
                        for (int i = 0; i < dsSales.Tables[0].Columns.Count; i++)
                        {
                            if (dsSales.Tables[0].Columns[i].ColumnName == "SalesChannelCode")
                            {
                                dsSales.Tables[0].Columns[i].ColumnName = "SalesChannelCode";
                                break;
                            }
                        }
                        gvSales.DataSource = dsSales;
                        gvSales.Columns[5].Visible = true;
                        gvSales.DataBind();
                        dvUploadPreview.Visible = true;
                        pnlGrid.Visible = true;
                        btnCancel.Visible = true;
                        btnSave.Visible = true;
                        btnSave.Enabled = false;
                        updgrid.Update();
                        break;
                    case 1:
                        InsertData(dsSales);
                        break;
                }
            }
            else if (UploadCheck == 2)
            {
                ucMsg.ShowInfo(Resources.Messages.UploadXlxs);
            }
            else if (UploadCheck == 3)
            {
                ucMsg.ShowInfo(Resources.Messages.SelectFile);
            }
            else
            {
                ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
           // clsException.clsHandleException.fncHandleException(ex, "");
        }
    
    }
    private void InsertData(DataSet dsPrimarySales)
    {
        DataTable dtPrimarySales = dsPrimarySales.Tables[0];
        using (CommonData ObjCommom = new CommonData())
        {
            dtPrimarySales = ObjCommom.GettvpTablePrimarySales2WitoutOrder();
        }

        foreach (DataRow dr in dsPrimarySales.Tables[0].Rows)
        {
            DataRow drow = dtPrimarySales.NewRow();
            drow[0] = 0;
            drow[1] = dr["SalesChannelCode"].ToString();
            drow[2] = dr["InvoiceNumber"].ToString();
            drow[3] = dr["InvoiceDate"];
            drow[4] = dr["SKUCode"].ToString();
            drow[5] = dr["Quantity"].ToString();
            drow[6] = "";
            drow[7] = Convert.ToString(PageBase.SalesChanelID);
            dtPrimarySales.Rows.Add(drow);
        }
        dtPrimarySales.AcceptChanges();

        DataColumn dcIsAcknowledge = new DataColumn();
        dcIsAcknowledge.DataType = System.Type.GetType("System.Boolean");
        dcIsAcknowledge.ColumnName = "IsAcknowledge";
        dcIsAcknowledge.DefaultValue = true;                //By Default we are setting true

        DataColumn dcError= new DataColumn();
        dcError.DataType = System.Type.GetType("System.String");
        dcError.ColumnName = "Error";

        if (dtPrimarySales.Columns.Contains("Error") == false)
            dtPrimarySales.Columns.Add(dcError);

        if (dtPrimarySales.Columns.Contains("IsAcknowledge") == false)
            dtPrimarySales.Columns.Add(dcIsAcknowledge);

        //if (dtPrimarySales.Columns.Contains("TDCode") == true)
            
        
        for (int i = 0; i <= dtPrimarySales.Rows.Count - 1; i++)
        {
            if (dtPrimarySales != null && dtPrimarySales.Rows.Count > 0)
            {
                //Same Invoice Number has different TD Code
                if (dtPrimarySales.Rows[i]["SalesChannelCode"] != DBNull.Value)
                {
                    string strWhere = "SalesChannelCode<>'" + dtPrimarySales.Rows[i]["SalesChannelCode"].ToString().Trim() + "'and InvoiceNumber ='" + dtPrimarySales.Rows[i]["InvoiceNumber"].ToString().Trim() + "'";
                    DataRow[] dr = dtPrimarySales.Select(strWhere);
                    if (dr.Length > 0)
                    {
                        counter = counter+1;
                        if (dtPrimarySales.Rows[i]["Error"] == DBNull.Value && dtPrimarySales.Rows[i]["Error"] == string.Empty)
                        {
                            dtPrimarySales.Rows[i]["Error"] = "Same Invoice Number has different" +Resources.Messages.SalesEntity + " Code";
                        }
                        else
                            dtPrimarySales.Rows[i]["Error"] = ";Same Invoice Number has different " + Resources.Messages.SalesEntity + " Code";
                    }

                    //Invoice Date Should not be greater than today's date
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
                }
                //Multiple invoice date with same invoiceNumber
                string strWhere1 = "InvoiceNumber='" + dtPrimarySales.Rows[i]["InvoiceNumber"].ToString().Trim() + "'and InvoiceDate <>'" + dtPrimarySales.Rows[i]["InvoiceDate"].ToString().Trim() + "'";
                if (dtPrimarySales.Rows[i]["InvoiceNumber"] != DBNull.Value)
                {

                    DataRow[] dr = dtPrimarySales.Select(strWhere1);
                    if (dr.Length > 0)
                    {
                        counter = counter + 1;
                        if (dtPrimarySales.Rows[i]["Error"] == DBNull.Value && dtPrimarySales.Rows[i]["Error"] == string.Empty)
                        {
                            dtPrimarySales.Rows[i]["Error"] = "Same invoice no with multiple dates!";
                        }
                        else
                            dtPrimarySales.Rows[i]["Error"] += ";Same invoice no with multiple dates!";
                    }
                }
            }
            //A salesman can have multiple TDCode but TDCode could have only one Salesman
            //string strWhere2 = "SalesChannelCode='" + dtPrimarySales.Rows[i]["SalesChannelCode"].ToString().Trim() + "'and SalesMan <>'" + dtPrimarySales.Rows[i]["SalesMan"].ToString().Trim() + "'";
            //if (dtPrimarySales.Rows[i]["SalesChannelCode"] != DBNull.Value)
            //{
            //    DataRow[] dr = dtPrimarySales.Select(strWhere2);
            //    if (dr.Length > 0)
            //    {
            //        counter += counter;
            //        if (dtPrimarySales.Rows[i]["Error"] == DBNull.Value && dtPrimarySales.Rows[i]["Error"] == string.Empty)
            //        {
            //            dtPrimarySales.Rows[i]["Error"] = "TD Can not have multiple salesman!";
            //        }
            //        else
            //            dtPrimarySales.Rows[i]["Error"] += ";TD Can not have multiple salesman!";
            //    }
            //}
            //Pankaj Kumar          Check for SS not to allow sale before the defined date
            
            //if (Convert.ToDateTime(DateTime.Now.Date).AddDays(PageBase.NumberofBackDaysAllowed) > Convert.ToDateTime(dtPrimarySales.Rows[i]["InvoiceDate"]))
            //{
            //    counter = counter + 1;
            //    if (dtPrimarySales.Rows[i]["Error"] == DBNull.Value && dtPrimarySales.Rows[i]["Error"] == string.Empty)
            //    {
            //        dtPrimarySales.Rows[i]["Error"] = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
            //    }
            //    else
            //        dtPrimarySales.Rows[i]["Error"] += ";" + Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));

            //} //Pankaj Kumar
            //Added by Mamta Singh to allow back days before opening date 
            if (Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening) >= Convert.ToDateTime(DateTime.Now.Date.AddDays(PageBase.NumberofBackDaysAllowed)))
            {

                if (Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-PageBase.BackDaysAllowedBeforeOpening) > Convert.ToDateTime(dtPrimarySales.Rows[i]["InvoiceDate"]))
                {
                    counter = counter + 1;
                    if (dtPrimarySales.Rows[i]["Error"] == DBNull.Value && dtPrimarySales.Rows[i]["Error"] == string.Empty)
                    {
                        dtPrimarySales.Rows[i]["Error"] = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", (Convert.ToDateTime(DateTime.Now.Date.ToShortDateString()) - Convert.ToDateTime(Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening).ToShortDateString())).TotalDays.ToString());
                    }
                    else
                        dtPrimarySales.Rows[i]["Error"] += ";" + Resources.Messages.ValidationSalesDays.ToString().Replace("Number", (Convert.ToDateTime(DateTime.Now.Date.ToShortDateString()) - Convert.ToDateTime(Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening).ToShortDateString())).TotalDays.ToString());
                }
                }
            else
            {
                if (Convert.ToDateTime(DateTime.Now.Date).AddDays(PageBase.NumberofBackDaysAllowed) > Convert.ToDateTime(dtPrimarySales.Rows[i]["InvoiceDate"]))
                {
                    counter = counter + 1;
                    if (dtPrimarySales.Rows[i]["Error"] == DBNull.Value && dtPrimarySales.Rows[i]["Error"] == string.Empty)
                    {
                        dtPrimarySales.Rows[i]["Error"] = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
                    }
                    else
                        dtPrimarySales.Rows[i]["Error"] += ";" + Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));

                }
            }
        }
        hideUnhideControls(dtPrimarySales);
        
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (IsPageRefereshed == true)
        {
            return;
        }
        try
        {
            if (ViewState["PrimarySales2"] != null)
            {
                dtSales= (DataTable)ViewState["PrimarySales2"];
                if(dtSales.Columns.Contains("Error")==true)
                dtSales.Columns.Remove("Error");
                if (dtSales.Columns.Contains("IsAcknowledge") == true)
                dtSales.Columns.Remove("IsAcknowledge");
                using (SalesData objPrimarySales2 = new SalesData())
                {
                   
                    objPrimarySales2.ErrorDetailXML = dsSales.GetXml();
                    objPrimarySales2.EntryType = EnumData.eEntryType.eUpload;
                    Int32 intResult = objPrimarySales2.InsertPrimarySales2Info(dtSales);
                  
                    if (objPrimarySales2.ErrorDetailXML != null && objPrimarySales2.ErrorDetailXML != string.Empty)
                    {
                        ucMsg.XmlErrorSource = objPrimarySales2.ErrorDetailXML;
                        return;
                    }
                    else if (objPrimarySales2.Error != null && objPrimarySales2.Error != "" )
                    {
                        ucMsg.ShowInfo(objPrimarySales2.Error);
                        return;
                    }
             
                    if (intResult == 4)
                    {
                        ucMsg.ShowSuccess(Resources.Messages.NoRecord);
                        if (ViewState["PrimarySales2"] != null)         
                            ViewState["PrimarySales2"] = null;
                        btnSave.Enabled = false;
                        return;
                    }
                   if (intResult == 2)
                    {
                        ucMsg.ShowInfo("Duplicate Invoice Number");
                        if (ViewState["PrimarySales2"] != null)       
                            ViewState["PrimarySales2"] = null;
                        btnSave.Enabled = false;
                        return;
                    }
                       ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                        clearData();
                        
                        if (ViewState["PrimarySales2"] != null)    
                            ViewState["PrimarySales2"] = null;

                    
                    updgrid.Update();
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
            clsException.clsHandleException.fncHandleException(ex, "");
        }

    }
   
    private void clearData()
    {
        if (ViewState["PrimarySales2"] != null)         //Clearing the viewstate data
            ViewState["PrimarySales2"] = null;
        gvSales.DataSource = null;
        gvSales.DataBind();
        pnlGrid.Visible = false;
        btnSave.Visible = false;
        btnCancel.Visible = false;
        dvUploadPreview.Visible = false;
        updgrid.Update();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clearData();
        btnCancel.Visible = false;
        
        ucMsg.Visible=false;
    }
    private void hideUnhideControls(DataTable dtPrimarySales)
    {
        dtNew = dtPrimarySales.Clone();
        foreach (DataColumn dc in dtNew.Columns)
        {
            if (dc.DataType == typeof(string)&& dc.ColumnName=="Quantity" )
            {
                dc.DataType = typeof(System.Int32);
            }
            if (dc.DataType == typeof(string) && dc.ColumnName == "InvoiceDate")
            {
                dc.DataType = typeof(System.DateTime);
            }
        }
        foreach (DataRow dr in dtPrimarySales.Rows)
        {
            dtNew.ImportRow(dr);
        }
        objSum = dtNew.Compute("sum(Quantity)", "");
        if (counter == 1)
        {
            btnSave.Enabled = true;
            gvSales.Columns[5].Visible = false;
            if (Convert.ToInt32(objSum) <= 0)
            {
                btnSave.Enabled = false;
                ucMsg.ShowInfo("Please insert right Quantity");
            }
        }
        else
        {
            btnSave.Enabled = false;
            gvSales.Columns[5].Visible = true;
        }

        if (dtPrimarySales.Rows.Count > 0)
        {
            dvUploadPreview.Visible = true;
            pnlGrid.Visible = true;
            btnSave.Visible = true;
           
            btnCancel.Visible = true;
            ViewState["PrimarySales2"] = dtPrimarySales;
            gvSales.DataSource = dtNew;
            gvSales.DataBind();
            updgrid.Update();

        }
        else
            pnlGrid.Visible = false;

    }
    protected void rdModeList_SelectedIndexChanged(object sender, EventArgs e)
    {
        
            if (rdModeList.SelectedValue == "1")
            {
                Response.Redirect("PrimarySales2InterfaceWithoutOrder.aspx", false);
            }
        
       

        
    }
}
