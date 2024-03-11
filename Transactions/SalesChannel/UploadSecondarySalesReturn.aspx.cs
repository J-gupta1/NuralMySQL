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
/// Created By: Pankaj Kumar
/// </summary>

public partial class Transactions_SalesChannel_UploadSecondarySalesReturn : PageBase
{
    DataTable dtRetailerCode = new DataTable();
    DataSet dsSales = new DataSet();
    string strUploadedFileName = string.Empty;
    int counter = 0;
    object objSum;
    DateTime dt = new DateTime();
    UploadFile UploadFile = new UploadFile();
    protected void Page_Load(object sender, EventArgs e)
    {
        dt = System.DateTime.Now.Date;
        //ucSalesReturnDate.MinRangeValue = dt.AddDays(PageBase.NumberofBackDaysAllowed);
        //Added By Mamta Singh for checking back date before opening date
        if (Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening) >= Convert.ToDateTime(dt.AddDays(PageBase.NumberofBackDaysAllowed)))
        {

            ucSalesReturnDate.MinRangeValue = Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-PageBase.BackDaysAllowedBeforeOpening);
            lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", (Convert.ToDateTime(dt.ToShortDateString()) - Convert.ToDateTime(Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening).ToShortDateString())).TotalDays.ToString());
        }
        else
        {
            ucSalesReturnDate.MinRangeValue = dt.AddDays(PageBase.NumberofBackDaysAllowed);
            lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
        }
           
        ucSalesReturnDate.MaxRangeValue = dt;
        ucSalesReturnDate.RangeErrorMessage = "Invalid Date Range";
        if (!IsPostBack)
        {
            pnlGrid.Visible=false;
            pnlGrid1.Visible = false;
            btnCancel.Visible = false;
            btnSave.Visible = false;
            //lblInfo.Text = "(Only " + PageBase.intBackDaysAllowForTD.ToString().Replace("-", "") + " days back Date Sales Return Allowed)";
            //lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
        }

    }
    private void InsertData(DataSet dsPrimarySales)
    {

        DataColumn dcSalesReturnToID = new DataColumn();
        dcSalesReturnToID.DataType = System.Type.GetType("System.Int64");
        dcSalesReturnToID.ColumnName = "SalesReturnToID";
        dcSalesReturnToID.DefaultValue = Convert.ToInt64(PageBase.SalesChanelID);

        DataColumn dcIsAcknowledge = new DataColumn();
        dcIsAcknowledge.DataType = System.Type.GetType("System.Boolean");
        dcIsAcknowledge.ColumnName = "IsAcknowledge";
        dcIsAcknowledge.DefaultValue = false;

        DataColumn dcError = new DataColumn();
        dcError.DataType = System.Type.GetType("System.String");
        dcError.ColumnName = "Error";

        if (dsPrimarySales.Tables[0].Columns.Contains("Error") == false)
            dsPrimarySales.Tables[0].Columns.Add(dcError);

        if (dsPrimarySales.Tables[0].Columns.Contains("SalesReturnToID") == false)
            dsPrimarySales.Tables[0].Columns.Add(dcSalesReturnToID);

        if (dsPrimarySales.Tables[0].Columns.Contains("IsAcknowledge") == false)
            dsPrimarySales.Tables[0].Columns.Add(dcIsAcknowledge);
        if (counter > 0)
        {
            ucMsg.ShowInfo("File could not be uploaded,Please try again");
            btnSave.Enabled = false;
            btnSave.Visible = true;
            btnCancel.Visible = true;
            pnlGrid1.Visible = true;
            pnlGrid.Visible = true;
            gvSalesReturn.DataSource = dsPrimarySales.Tables[0];
            gvSalesReturn.DataBind();
            gvSalesReturn.Columns[3].Visible = true;
            return;

        }
        else
        {
            objSum = dsPrimarySales.Tables[0].Compute("sum(Quantity)", "");
          if (Convert.ToInt32(objSum) <= 0)
            {
                btnSave.Enabled = false;
                ucMsg.ShowInfo("Please insert right Quantity");
                gvSalesReturn.DataSource = dsPrimarySales;
                
                gvSalesReturn.DataBind();
                if (ViewState["SalesReturnSecondary"] != null)
                    ViewState["SalesReturnSecondary"] = null;
                gvSalesReturn.Columns[3].Visible = false;
                pnlGrid1.Visible = true;
                pnlGrid.Visible = true;
                btnCancel.Visible = true;
                btnSave.Visible = true;
                btnSave.Enabled = false;
                return;
            }
            pnlGrid1.Visible = true;
            pnlGrid.Visible = true;
            btnSave.Visible = true;
            btnCancel.Visible = true;
            btnSave.Enabled = true;
            gvSalesReturn.DataSource = dsPrimarySales;
            ViewState["SalesReturnSecondary"] = dsPrimarySales;
            gvSalesReturn.DataBind();
            gvSalesReturn.Columns[3].Visible = false;
        }

    }
    protected void btnUpload_Click1(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["SalesReturnSecondary"] != null)
                ViewState["SalesReturnSecondary"] = null;
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
                isSuccess = UploadFile.uploadValidExcel(ref dsSales, "SecondarySalesReturn");

                switch (isSuccess)
                {
                    case 0:
                        ucMsg.ShowInfo(UploadFile.Message);
                        break;
                    case 2:
                        ucMsg.ShowInfo(Resources.Messages.CheckErrorGrid);
                        btnSave.Enabled = false;
                        btnSave.Visible = true;
                        btnCancel.Visible = true;
                        pnlGrid1.Visible = true;
                        pnlGrid.Visible = true;
                        gvSalesReturn.Visible = true;
                        gvSalesReturn.DataSource = dsSales;
                        gvSalesReturn.DataBind();
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
            ucMsg.ShowInfo(ex.Message.ToString());
        }
    }
    protected void btnSave_Click1(object sender, EventArgs e)
    {
        if (IsPageRefereshed == true)
        {
            return;
        }
        if (Convert.ToDateTime(ucSalesReturnDate.Date) > DateTime.Now)
        {
            ucMsg.ShowInfo("The return date cant be greater then the current date");
            return;
        }

       
            if (Convert.ToDateTime(DateTime.Now.Date).AddDays(PageBase.NumberofBackDaysAllowed) > Convert.ToDateTime(ucSalesReturnDate.Date))
            {
                ucMsg.ShowInfo("Invalid Date Range");
                return;
            } //Pankaj Kumar
        
        try
        {
            if (ViewState["SalesReturnSecondary"] != null)
            {
                
                DataColumn dcSalesReturnDate = new DataColumn();
                dcSalesReturnDate.DataType = System.Type.GetType("System.DateTime");
                dcSalesReturnDate.ColumnName = "SalesReturnDate";
                dcSalesReturnDate.DefaultValue = Convert.ToDateTime(ucSalesReturnDate.Date);

                dsSales = (DataSet)ViewState["SalesReturnSecondary"];
                dsSales.Tables[0].Columns.Remove("Error");
                dsSales.Tables[0].Columns.Remove("IsAcknowledge");
                DataColumn dcSalesmanname = new DataColumn();
                dcSalesmanname.DataType = System.Type.GetType("System.String");
                dcSalesmanname.ColumnName = "Salesman";
                dcSalesmanname.DefaultValue = "";
                dsSales.Tables[0].Columns.Add(dcSalesmanname);
               
                dsSales.Tables[0].Columns.Add(dcSalesReturnDate);
                DataColumn dcSalesmanID = new DataColumn();
                dcSalesmanID.DataType = System.Type.GetType("System.Int32");
                dcSalesmanID.ColumnName = "SalesmanID";
                dcSalesmanID.DefaultValue = 0;
                dsSales.Tables[0].Columns.Add(dcSalesmanID);
                using (SalesData objSecondarySalesReturn = new SalesData())
                {
                    objSecondarySalesReturn.UserID = PageBase.UserId;
                    Int32 intResult = objSecondarySalesReturn.InsertSecondarySalesReturnInfo(dsSales.Tables[0]);
                    if (objSecondarySalesReturn.ErrorDetailXML != null && objSecondarySalesReturn.ErrorDetailXML != string.Empty)
                    {
                        ucMsg.XmlErrorSource = objSecondarySalesReturn.ErrorDetailXML;
                        btnSave.Enabled = false;
                        ucSalesReturnDate.Date = "";
                        return;
                    }
                    else if (objSecondarySalesReturn.Error != null && objSecondarySalesReturn.Error != "" && objSecondarySalesReturn.Error != "0")
                    {
                        ucMsg.ShowError(objSecondarySalesReturn.Error);
                        btnSave.Enabled = false;
                        ucSalesReturnDate.Date = "";
                        return;
                    }
                    if (intResult == 0)
                    {
                        ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                        pnlGrid1.Visible = false;
                        pnlGrid.Visible = false;
                        gvSalesReturn.DataSource = null;
                        gvSalesReturn.DataBind();
                        ucSalesReturnDate.Date = "";

                        if (ViewState["SalesReturnSecondary"] != null)         //Clearing the viewstate data
                            ViewState["SalesReturnSecondary"] = null;
                    }
                    if (intResult == 4)
                    {
                        ucMsg.ShowSuccess(Resources.Messages.NoRecord);
                        if (ViewState["SalesReturnSecondary"] != null)         //Clearing the viewstate data
                            ViewState["SalesReturnSecondary"] = null;
                        btnSave.Enabled = false;

                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
            btnSave.Enabled = false;
        }

    }
    protected void btnCancel_Click1(object sender, EventArgs e)
    {
        if (ViewState["SalesReturnSecondary"] != null)
            ViewState["SalesReturnSecondary"] = null;
        ucMsg.Visible = false;
        pnlGrid1.Visible = false;
        pnlGrid.Visible = false;
        btnSave.Visible = false;
        btnCancel.Visible = false;
        gvSalesReturn.DataSource = null;
        gvSalesReturn.DataBind();
        ucSalesReturnDate.Date = "";
    }
    protected void DwnldReferenceCodeTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsReferenceCode = new DataSet();
            using (SalesChannelData objSalesData = new SalesChannelData())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.eSecondarySales;
                objSalesData.SalesChannelID = PageBase.SalesChanelID;
                objSalesData.Brand = PageBase.Brand;

                dsReferenceCode = objSalesData.GetAllTemplateData();
                if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dsReferenceCode, FilenameToexport, EnumData.eTemplateCount.eSecondary);
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
    protected void rdModelList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdModelList.SelectedValue == "2")
        {
            Response.Redirect("SecondarySalesReturnInterface.aspx");
        }
    }
}
