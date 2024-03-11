using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using System.Data;
/*Change Log:
 * 30-May-2014, Rakesh Goel, #CC01 - Replace back ~# and #~ to square brackets
 */
public partial class Transactions_SalesChannel_UploadSecondarySalesMMX : PageBase
{
    object objSum;
    string strUploadedFileName = string.Empty;
    int counter = 0;
    UploadFile UploadFile = new UploadFile();
    DataSet dsSecondarySales = new DataSet();
    DataSet dsTemplateCode = new DataSet();
    DateTime dt = new DateTime();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            dt = System.DateTime.Now.Date;
           // ucSalesDate.MinRangeValue = dt.AddDays(PageBase.NumberofBackDaysAllowed);
            ucSalesDate.MaxRangeValue = dt;
            ucSalesDate.RangeErrorMessage = "Invalid Date Range";
            ucMsg.ShowControl = false;
            //Added By Mamta Singh for checking back date before opening date
            if (Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening) >= Convert.ToDateTime(dt.AddDays(PageBase.NumberofBackDaysAllowed)))
            {
                ucSalesDate.MinRangeValue = Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-PageBase.BackDaysAllowedBeforeOpening);
                lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", (Convert.ToDateTime(dt.ToShortDateString()) - Convert.ToDateTime(Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening).ToShortDateString())).TotalDays.ToString());
            }
            else
            {
                ucSalesDate.MinRangeValue = dt.AddDays(PageBase.NumberofBackDaysAllowed);
                lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
            }
           
            if (!IsPostBack)
            {

                
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }

    }
    protected void lnkDownLoadTemplate_Click(object sender, EventArgs e)
    {
          try
        {
            using (SalesChannelData objSalesData = new SalesChannelData())
            {
                objSalesData.SalesChannelID = PageBase.SalesChanelID;
                objSalesData.Brand = PageBase.Brand;
                dsTemplateCode = objSalesData.GetSecondaryTemplate();
                if (dsTemplateCode!=null && dsTemplateCode.Tables.Count>0)
                {
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Secondary Template";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dsTemplateCode, FilenameToexport,EnumData.eTemplateCount.ePrice);
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
    protected void DwnldReferenceCodeTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            using (SalesChannelData objSalesData = new SalesChannelData())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.eSecondarySales;
                objSalesData.SalesChannelID = PageBase.SalesChanelID;
                objSalesData.Brand = PageBase.Brand;
                dsTemplateCode = objSalesData.GetAllTemplateDataMicromax();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dsTemplateCode, FilenameToexport, EnumData.eTemplateCount.eSecondary);
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
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
           // if (ViewState["SecondarySales"] != null)
             //   ViewState["SecondarySales"] = null;
            ucMsg.Visible = false;
            byte isSuccess = 1;
            Int16 UploadCheck = 0;
            String RootPath = Server.MapPath("../../");
            UploadFile.RootFolerPath = RootPath;
            Btnsave.Visible = true ;
            UploadCheck = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            ViewState["TobeUploaded"] = strUploadedFileName;/*Pkd 29-Nov-2014*/
            if (UploadCheck == 1)
            {
                UploadFile.UploadedFileName = strUploadedFileName;
              
                isSuccess = UploadFile.uploadValidExcelForSecondarySales(ref dsSecondarySales);
               
                switch (isSuccess)
                {
                    case 0:
                        ucMsg.ShowInfo(UploadFile.Message);
                        GridSales.Visible = true;
                        Btnsave.Visible = false;
                        GridSales.DataSource = dsSecondarySales;
                        GridSales.DataBind();
                       // if (ViewState["SecondarySales"] != null)
                         //   ViewState["SecondarySales"] = null;
                        break;
                    case 2:
                        ucMsg.ShowInfo(Resources.Messages.CheckErrorGrid);
                        pnlGrid.Visible = true;
                        Btnsave.Visible = false;
                        GridSales.Visible = true;
                        GridSales.DataSource = dsSecondarySales;
                        GridSales.DataBind();
                        //if (ViewState["SecondarySales"] != null)
                          //  ViewState["SecondarySales"] = null;
                        break;
                    case 1:
                        InsertData(dsSecondarySales);

                        break;
                }
            }
            else if (UploadCheck == 2)
            {
                ucMsg.ShowInfo(Resources.Messages.UploadXlxs);
                GridSales.DataSource = null;
                GridSales.DataBind();
                pnlGrid.Visible = false;

            }
            else if (UploadCheck == 3)
            {
                ucMsg.ShowInfo(Resources.Messages.SelectFile);
                GridSales.DataSource = null;
                GridSales.DataBind();
                pnlGrid.Visible = false;

            }
            else
            {
                ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                GridSales.DataSource = null;
                GridSales.DataBind();
                pnlGrid.Visible = false;

            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
        }
    }
    private void InsertData(DataSet dsSecondarySales)
    {
        pnlGrid.Visible = true;
        Btnsave.Visible = true;
        Btnsave.Enabled = true;
        BtnCancel.Visible = true;

        if (dsSecondarySales.Tables[0].Select("Error <>'' and Error is not null").Length > 0)
        {

        }
        else
        {
            dsSecondarySales.Tables[0].Columns.Remove("Error");
        }
        dsSecondarySales.AcceptChanges();
        //ViewState["SecondarySales"] = dsSecondarySales.Tables[0]; 19-Nov No more required
        /*Start 29-Nov*/
        var columnList = dsSecondarySales.Tables[0].AsEnumerable().ToList();
        int columnCount = dsSecondarySales.Tables[0].Columns.Count;
        int rowCount = dsSecondarySales.Tables[0].Rows.Count;

        var rowList = Enumerable.Range(0, columnCount)
                                 .Select(x => Enumerable.Range(0, rowCount)
                                                         .Select(y => columnList[y][x])
                                                         .ToList())
                                 .ToList();

        DataTable dtInfo = new DataTable();
        dtInfo.Columns.Add("RetailerName", typeof(System.String));
        dtInfo.Columns.Add("Quantity", typeof(System.Int32));
        dtInfo.AcceptChanges();

        int intRetailerIndex = 0;
        int Quantity=0;
        foreach (string strRetailer in rowList[1])
        {
            
            for (int index = 3; index <= columnCount-1; index++)
               Quantity = Quantity+Convert.ToInt32(rowList[index][intRetailerIndex]);

            DataRow drData = dtInfo.NewRow();
            drData["RetailerName"] = strRetailer;
            drData["Quantity"] = Quantity;
            dtInfo.Rows.Add(drData);
            Quantity = 0;
            intRetailerIndex = intRetailerIndex + 1;
        }
        /*End 29-Nov*/
        GridSales.DataSource = dtInfo;
        GridSales.DataBind();
    }
    private void ClearForm()
    {
        ucMsg.Visible = false;
        Btnsave.Visible = false;
        BtnCancel.Visible = false;
        pnlGrid.Visible = false;
        ucSalesDate.Date = "";
        //if (ViewState["SecondarySales"] != null)
//            ViewState["SecondarySales"] = null;
    }
    protected void Btnsave_Click(object sender, EventArgs e)
    {
        try
        {

            //if (ViewState["SecondarySales"] != null)/*PKD 29-Nov Commented*/
           // {
            if (ViewState["TobeUploaded"] != null)/*PKD 29-Nov Added*/
            {
                //DataTable DtDetail = (DataTable)ViewState["SecondarySales"];/*PKD 29-Nov Commented*/
                DataSet DsExcelDetail = new DataSet();/*PKD 29-Nov Added*/
                OpenXMLExcel objexcel = new OpenXMLExcel();
                String RootPath = Server.MapPath("../../");
                UploadFile.RootFolerPath = RootPath;
                UploadFile.UploadedFileName = ViewState["TobeUploaded"].ToString();
                DsExcelDetail = UploadFile.ImportExcelFileMMX();//by oldedb it was not fetching the all column of excel it was fetching the 254 column only in datatable instead of all
                DataTable DtDetail = DsExcelDetail.Tables[0];
                

                DataTable DtNew = new DataTable();
                DtNew.Columns.Add("RetailerCode");
                DtNew.Columns.Add("SKUName");
                DataColumn Dc1 = new DataColumn("Quantity");
                Dc1.DataType = System.Type.GetType("System.Int32");
                DtNew.Columns.Add(Dc1);
                foreach (DataRow dr in DtDetail.Rows)
                {

                    for (int i = 3; i <= DtDetail.Columns.Count - 1; i++)
                    {

                        DataRow drow = DtNew.NewRow();
                        drow[0] = dr["RetailerCode"];
                        drow[1] = DtDetail.Columns[i].ColumnName.Replace("~#", "[").Replace("#~", "]");  /*#CC01 added replace*/
                        drow[2] = dr[i];
                        DtNew.Rows.Add(drow);

                    }
                    DtNew.AcceptChanges();
                }

                DtNew.DefaultView.RowFilter = "Quantity<>0";
                DtNew = DtNew.DefaultView.ToTable();
                if (DtNew != null && DtNew.Rows.Count == 0)
                {
                    ucMsg.ShowInfo("Please insert quantity.");
                    return;
                }
                using (SalesData ObjSales = new SalesData())
                {

                    ObjSales.Error = "";
                    ObjSales.SalesChannelID = PageBase.SalesChanelID;
                    ObjSales.InvoiceDate = Convert.ToDateTime(ucSalesDate.Date);
                    ObjSales.InsertSecondarySalesMicromax(DtNew);

                    if (ObjSales.ErrorDetailXML != null && ObjSales.ErrorDetailXML != string.Empty)
                    {
                        ucMsg.XmlErrorSource = ObjSales.ErrorDetailXML;

                        return;
                    }

                    else if (ObjSales.Error != null && ObjSales.Error != "")
                    {
                        ucMsg.ShowInfo(ObjSales.Error);

                        return;
                    }

                    ClearForm();

                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    //if (ViewState["SecondarySales"] != null)         //Clearing the viewstate data
                      //  ViewState["SecondarySales"] = null;

                }


            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }

    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        ClearForm();
        pnlGrid.Visible = false;
    }
    protected void rdModelList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdModelList.SelectedValue == "1")
            Response.Redirect("SecondarySalesInterface.aspx");
    }
}
