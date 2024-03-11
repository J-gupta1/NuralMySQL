using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;

public partial class Transactions_SalesChannel_PrimarySalesReturnUpload : PageBase
{   DataTable DtSaleschannel;
DataTable dtNew = new DataTable();
object objSum;
string strUploadedFileName = string.Empty;
int counter = 0;        //Pankaj Kumar
DateTime dt = new DateTime();
UploadFile UploadFile = new UploadFile();
     
    protected void Page_Load(object sender, EventArgs e)
    {
       
        ucMsg.ShowControl = false;
        //if warehouse then saleChanelTypeid=5 when SS then salesChanelTypeID=6
        if (Convert.ToInt32(PageBase.SalesChanelTypeID) == 5)              //Pankaj Kumar
        {
            lblMainHeading.Text = "Primary Sales 1 Return";
            dt = System.DateTime.Now.Date;
            ucDatePicker.MaxRangeValue = System.DateTime.Now.Date;
            rdoSelectMode.Visible = false;
           
        }
        else if (Convert.ToInt32(PageBase.SalesChanelTypeID) == 6)
        {
            lblMainHeading.Text = "Primary Sales 2 Return";
            dt = System.DateTime.Now.Date;
            //ucDatePicker.MinRangeValue = dt.AddDays(PageBase.NumberofBackDaysAllowed);
            ucDatePicker.MaxRangeValue = dt;
            ucDatePicker.RangeErrorMessage = "Invalid Date Range";
           // lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
            lblInfo.Visible = true;
            rdoSelectMode.Visible = true;
            dvHide.Visible = true;      //Pankaj Dhingra
            //Added By Mamta Singh for checking back date before opening date
            if (Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening) >= Convert.ToDateTime(dt.AddDays(PageBase.NumberofBackDaysAllowed)))
            {


                ucDatePicker.MinRangeValue = Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-PageBase.BackDaysAllowedBeforeOpening);
                lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", (Convert.ToDateTime(dt.ToShortDateString()) - Convert.ToDateTime(Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening).ToShortDateString())).TotalDays.ToString());
            }
            else
            {
                ucDatePicker.MinRangeValue = dt.AddDays(PageBase.NumberofBackDaysAllowed);
                lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
            }
        }
    }
    //protected void DwnldDustributorCodeTemplate_Click(object sender, EventArgs e)
    //{
       
    //    try
    //    {
    //        using (SalesChannelData objSalesData = new SalesChannelData())
    //        {
    //            objSalesData.UserID = PageBase.UserId;
    //            objSalesData.Type = 1;          //1-TD/SS Code download  
    //            DtSaleschannel = objSalesData.GetAllTemplateData();
    //            if (DtSaleschannel.Rows.Count > 0)
    //            {
    //                DataSet dtcopy = new DataSet();
    //                dtcopy.Merge(DtSaleschannel);
    //                String FilePath = Server.MapPath("../../");
    //                string FilenameToexport = "SS/TD Code List";
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
    //      //  PageBase.Errorhandling(ex);
    //    }
    //}
    //protected void DwnldSKUCodeTemplate_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DtSaleschannel = null;
    //        using (SalesChannelData objSalesData = new SalesChannelData())
    //        {
              
    //            objSalesData.Type = 2;          //1-TD Code download  2-SKU Download  3-Download Sales Template 4-Download Salesman 
    //            DtSaleschannel = objSalesData.GetAllTemplateData();
    //            if (DtSaleschannel.Rows.Count > 0)
    //            {
    //                DataSet dtcopy = new DataSet();
    //                dtcopy.Merge(DtSaleschannel);
    //                String FilePath = Server.MapPath("../../");
    //                string FilenameToexport = "SKUCode List";
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
    //       // PageBase.Errorhandling(ex);
    //    }
    //}
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet objDS = new DataSet();
            byte isSuccess = 1;
            Int16 UploadCheck = 0;
            String RootPath = Server.MapPath("../../");
            UploadFile.RootFolerPath = RootPath;

            UploadCheck = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            if (UploadCheck == 1)
            {
                UploadFile.UploadedFileName = strUploadedFileName;
                UploadFile.UploadValidationType = EnumData.eUploadExcelValidationType.eSales;
                isSuccess = UploadFile.uploadValidExcel(ref objDS, "PrimarySalesReturn");

                switch (isSuccess)
                {
                    case 0:
                        ucMsg.ShowInfo(UploadFile.Message);
                        break;
                    case 2:
                        ucMsg.ShowInfo(Resources.Messages.CheckErrorGrid);
                        pnlGrid.Visible = true;
                       
                        objDS.Tables[0].Columns["SalesChannelCode"].ColumnName = "SalesChannelCode";
                        GridSalesReturn.DataSource = objDS;
                        GridSalesReturn.DataBind();
                        updGrid.Update();
                        break;
                    case 1:
                        InsertData(objDS);
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
            ucMsg.ShowError(ex.ToString());
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
                        if (objDS.Tables[0].Rows[i]["Error"] == DBNull.Value || objDS.Tables[0].Rows[i]["Error"].ToString() == string.Empty)
                        {
                            objDS.Tables[0].Rows[i]["Error"] = "Same invoice number can not have muliple " + Resources.Messages.SalesEntity +" Code";
                        }
                        else
                            objDS.Tables[0].Rows[i]["Error"] += "Same invoice number can not have muliple " + Resources.Messages.SalesEntity + "  Code";
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
                        if (objDS.Tables[0].Rows[i]["Error"] == DBNull.Value ||objDS.Tables[0].Rows[i]["Error"] == string.Empty)
                        {
                            objDS.Tables[0].Rows[i]["Error"] = "Same invoice no with multiple dates!";
                        }
                        else
                            objDS.Tables[0].Rows[i]["Error"] += ";Same invoice no with multiple dates!";
                    }
                }
                //if (Convert.ToInt32(PageBase.SalesChanelTypeID) == 6)
                //{
                //    if (Convert.ToDateTime(DateTime.Now.Date).AddDays(PageBase.NumberofBackDaysAllowed) > Convert.ToDateTime(objDS.Tables[0].Rows[i]["InvoiceDate"]))
                //    {
                //        counter = counter + 1;
                //        if (objDS.Tables[0].Rows[i]["Error"] == DBNull.Value || objDS.Tables[0].Rows[i]["Error"] == string.Empty)
                //        {
                //            objDS.Tables[0].Rows[i]["Error"] = "InValid Date Range";
                //        }
                //        else
                //            objDS.Tables[0].Rows[i]["Error"] += ";InValid Date Range!";

                //    } //Pankaj Kumar
                //}
            }
            
        }



       
        if (objDS.Tables[0].Rows.Count > 0)
        {
            pnlGrid.Visible = true;
            Btnsave.Visible = true;
            BtnCancel.Visible = true;
            if (counter == 0)
                objDS.Tables[0].Columns.Remove("Error"); 
          
            GridSalesReturn.DataSource = objDS.Tables[0];
            GridSalesReturn.DataBind();
            

        }
        else
        {
            pnlGrid.Visible = false;
        }
        if (counter == 0)           //Pankaj Kumar Now Default Value for counter=1
        {
            ViewState["Detail"] = objDS.Tables[0];
            Btnsave.Enabled = true;
            Btnsave.Visible = true;

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
                Btnsave.Enabled = false;
                ucMsg.ShowInfo("Please insert right Quantity");
                GridSalesReturn.DataSource = objDS.Tables[0];
                GridSalesReturn.DataBind();
                if (ViewState["Detail"] != null)
                    ViewState["Detail"] = null;

                BtnCancel.Visible = true;
                Btnsave.Visible = true;
                return;
            }

        }
        else
        {
            Btnsave.Visible = true;
           
            Btnsave.Enabled = false;
            ViewState["Detail"] = null;
        }
        updGrid.Update();


    }

    protected void rdoSelectMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoSelectMode.SelectedValue == "2")
        {
            Response.Redirect("PrimarySales2ReturnInterface.aspx");
        }

    }
    //bool isdatevalidate()   //  created by Saurabh , to validate the date 
    //{
    //    int i ;
    //    int max;
    //    DataTable dt = (DataTable)ViewState["Detail"];
    //    max =      0;
    //    if (dt.Rows.Count == 1)
    //    {
    //        max = 0;
    //    }
    //    else
    //    {

    //        for (i = 1; i <= dt.Rows.Count; i++)
    //        {
    //            if (Convert.ToDateTime(dt.Rows[i]["InvoiceDate"]) > Convert.ToDateTime(dt.Rows[max]["InvoiceDate"]))
    //            {
    //                max = i;
    //            }


    //        }
    //    }
    //        if (Convert.ToDateTime(dt.Rows[max]["InvoiceDate"]) > Convert.ToDateTime(ucDatePicker.Date))
    //    {
           
    //        return false;
    //    }
    //    return true;
    
    //}



    protected void Btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsPageRefereshed == true)           //Pankaj Kumar
            {
                return;
            }

            if (Convert.ToDateTime(ucDatePicker.Date) > DateTime.Now)
            {
                ucMsg.ShowInfo("The return date cant be greater then the current date");
                return;
            }

            if (Convert.ToInt32(PageBase.SalesChanelTypeID) == 6)       //For only SS
            {
                if (Convert.ToDateTime(DateTime.Now.Date).AddDays(PageBase.NumberofBackDaysAllowed) > Convert.ToDateTime(ucDatePicker.Date))
                {
                    ucMsg.ShowInfo("Invalid Date Range");
                    return;
                } //Pankaj Kumar
            }
            int Result = 0;
            if (ViewState["Detail"] != null)
            {
                DataTable DtDetail = new DataTable();
                DataTable dtSalesReturn = new DataTable();
                using (CommonData ObjCommom = new CommonData())
                {
                    dtSalesReturn = ObjCommom.GettvpTablePrimarySalesReturn();
                }
                DtDetail = (DataTable)ViewState["Detail"];
                foreach (DataRow dr in DtDetail.Rows)
                {
                    DataRow drow = dtSalesReturn.NewRow();
                    drow[0] = null;
                    drow[1] = dr["SalesChannelCode"].ToString();
                    drow[2] = dr["InvoiceNumber"].ToString();
                    drow[3] = dr["InvoiceDate"];
                    drow[4] = dr["SKUCode"].ToString();
                    drow[5] = dr["Quantity"].ToString();
                    drow[6] = Convert.ToString(PageBase.SalesChanelID);
                    drow[7] = Convert.ToDateTime(ucDatePicker.Date);
                    dtSalesReturn.Rows.Add(drow);
                }
                dtSalesReturn.AcceptChanges();
                using (SalesData ObjSales = new SalesData())
                {

                    ObjSales.Error = "";
                    ObjSales.EntryType = EnumData.eEntryType.eUpload;


                    Result = ObjSales.InsertUpdatePrimarySalesReturnInfo(dtSalesReturn);

                    if (ObjSales.ErrorDetailXML != null && ObjSales.ErrorDetailXML != string.Empty)
                    {
                        ucMsg.XmlErrorSource = ObjSales.ErrorDetailXML;
                        return;
                    }
                    else if (ObjSales.Error != null && ObjSales.Error != "" && ObjSales.Error != "0")
                    {
                        ucMsg.ShowError(ObjSales.Error);
                        return;
                    }


                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    ClearForm();

                    updGrid.Update();
                }

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    void ClearForm()
    {
        ucDatePicker.Date = "";
        ViewState["Detail"] = null;
       // rdoSelectMode.SelectedValue = "1";
        pnlGrid.Visible = false;
        updGrid.Update();

    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        ClearForm();
    }


    protected void DwnldReferenceCodeTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsReferenceCode = new DataSet();
            using (SalesChannelData objSalesData = new SalesChannelData())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.ePrimary2Sales;
                objSalesData.SalesChannelID = PageBase.SalesChanelID;
                objSalesData.Brand = PageBase.Brand;            //Pankaj Dhingra
                dsReferenceCode = objSalesData.GetAllTemplateData();
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
            // PageBase.Errorhandling(ex);
        }
    }
}
