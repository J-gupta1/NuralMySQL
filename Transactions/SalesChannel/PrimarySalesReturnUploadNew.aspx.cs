using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;


public partial class Transactions_SalesChannel_PrimarySalesReturnUploadNew : PageBase
{
    DataTable DtSaleschannel;
    DataTable dtNew = new DataTable();
    object objSum;
    string strUploadedFileName = string.Empty;
    int counter = 0;        //Pankaj Kumar
    DateTime dt = new DateTime();
    UploadFile UploadFile = new UploadFile();
    protected void Page_Load(object sender, EventArgs e)
    {

        ucMsg.ShowControl = false;
            lblMainHeading.Text = "Primary Sales 1 Return";
            dt = System.DateTime.Now.Date;
            // As discussed with the pardeep sir ther is not need to check the back days allowed on sales return         
            //ucDatePicker.MinRangeValue = dt.AddDays(PageBase.NumberofBackDaysAllowed);
            ucDatePicker.MaxRangeValue = System.DateTime.Now.Date;
            ucDatePicker.RangeErrorMessage ="Invalid Date Range";
            

       
    }
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
                isSuccess = UploadFile.uploadValidExcel(ref objDS, "PrimarySalesReturnP1");

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
                        GridSalesReturn.DataSource = null;
                        GridSalesReturn.DataBind();
                        updGrid.Update();
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
                            objDS.Tables[0].Rows[i]["Error"] = "Same invoice number can not have muliple " + Resources.Messages.SalesEntity + " Code";
                        }
                        else
                            objDS.Tables[0].Rows[i]["Error"] += "Same invoice number can not have muliple " + Resources.Messages.SalesEntity + "  Code";
                    }
                    string strWhereW = "WarehouseCode<>'" + objDS.Tables[0].Rows[i]["WarehouseCode"].ToString().Trim() + "'and InvoiceNumber ='" + objDS.Tables[0].Rows[i]["InvoiceNumber"].ToString().Trim() + "'";
                    DataRow[] drW = objDS.Tables[0].Select(strWhereW);
                    if (drW.Length > 0)
                    {
                        counter = counter + 1;
                        if (objDS.Tables[0].Rows[i]["Error"]==DBNull.Value  || objDS.Tables[0].Rows[i]["Error"].ToString () == string.Empty)
                        {
                            objDS.Tables[0].Rows[i]["Error"] = "Same Invoice Number has different Warehouse Code";
                        }
                        else
                            objDS.Tables[0].Rows[i]["Error"] += ";Same Invoice Number has different Warehouse Code";
                    }
                    string strWhere5 = "WarehouseCode<>'" + objDS.Tables[0].Rows[i]["WarehouseCode"].ToString().Trim() + "' and SalesChannelCode<>'" + objDS.Tables[0].Rows[i]["SalesChannelCode"].ToString().Trim() + "' and InvoiceNumber='" + objDS.Tables[0].Rows[i]["InvoiceNumber"].ToString().Trim() + "'";
                    DataRow[] dr1 = objDS.Tables[0].Select(strWhere5);
                    if (dr1.Length > 0)
                    {
                        counter = counter + 1;
                        if (objDS.Tables[0].Rows[i]["Error"] == DBNull.Value || objDS.Tables[0].Rows[i]["Error"].ToString ()=="")
                        {
                            objDS.Tables[0].Rows[i]["Error"] = "Same Invoice Number has different Warehouse To Distributor.";
                        }
                        else
                            objDS.Tables[0].Rows[i]["Error"] += ";Same Invoice Number has different Warehouse To Distributor.";
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
                        if (objDS.Tables[0].Rows[i]["Error"] == DBNull.Value || objDS.Tables[0].Rows[i]["Error"].ToString () == string.Empty)
                        {
                            objDS.Tables[0].Rows[i]["Error"] = "Same invoice no with multiple dates!";
                        }
                        else
                            objDS.Tables[0].Rows[i]["Error"] += ";Same invoice no with multiple dates!";
                    }
                }
              
            }
            objDS.Tables[0].AcceptChanges();

        }


        if (objDS.Tables[0].Rows.Count > 0)
        {
            pnlGrid.Visible = true;
            BtnCancel.Visible = true;
            Btnsave.Visible = true;
            Btnsave.Enabled = true;
            if (counter == 0)
            {
                objDS.Tables[0].Columns.Remove("Error");
            }
            GridSalesReturn.DataSource = objDS.Tables[0];
            GridSalesReturn.DataBind();
            updGrid.Update();

            
               
                if (counter == 0)
                {
                     
                    dtNew = objDS.Tables[0].Clone();

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

                        if (ViewState["Detail"] != null)
                            ViewState["Detail"] = null;
                        Btnsave.Enabled = false;

                        return;
                    }
                    ViewState["Detail"] = objDS.Tables[0];

                }
                else
                {
                    Btnsave.Enabled = false;
                }
                updGrid.Update();



            }
            else
            {
                pnlGrid.Visible = false;
                updGrid.Update();
            }


        


    }

    protected void rdoSelectMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoSelectMode.SelectedValue == "2")
        {
            Response.Redirect("PrimarySales1ReturnInterface.aspx");
            //Server.Transfer("PrimarySales1ReturnInterface.aspx");
        }

    }
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

            //if (Convert.ToInt32(PageBase.SalesChanelTypeID) == 6)       //For only SS
            //{
            //    if (Convert.ToDateTime(DateTime.Now.Date).AddDays(PageBase.NumberofBackDaysAllowed) > Convert.ToDateTime(ucDatePicker.Date))
            //    {
            //        ucMsg.ShowInfo("Invalid Date Range");
            //        return;
            //    } //Pankaj Kumar
            //}
            int Result = 0;
            if (ViewState["Detail"] != null)
            {
                DataTable DtDetail = new DataTable();
                DataTable dtSalesReturn = new DataTable();
                using (CommonData ObjCommom = new CommonData())
                {
                    dtSalesReturn = ObjCommom.GettvpTablePrimarySalesReturnNew();
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
                    drow[8] = dr["WarehouseCode"];

                    dtSalesReturn.Rows.Add(drow);
                }
                dtSalesReturn.AcceptChanges();
                using (SalesData ObjSales = new SalesData())
                {

                    ObjSales.Error = "";
                    ObjSales.EntryType = EnumData.eEntryType.eUpload;

                    ObjSales.UserID = PageBase.SalesChanelID;
                    Result = ObjSales.InsertUpdatePrimarySalesReturnInfoNew(dtSalesReturn);

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
                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.ePrimary1Sales;
                objSalesData.SalesChannelID = PageBase.SalesChanelID;
                objSalesData.Brand = PageBase.Brand;            //Pankaj Dhingra
                dsReferenceCode = objSalesData.GetAllTemplateData();
                if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dsReferenceCode, FilenameToexport, EnumData.eTemplateCount.ePrimarysales1 + 1);
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
