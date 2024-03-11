using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;

public partial class Transactions_SalesChannel_GRNUpload : PageBase 
{
    DataTable dtNew = new DataTable();
    object objSum;
    int counter = 0;
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    string abc = "";
    UploadFile UploadFile = new UploadFile();
   // string saleschannelcode;


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMsg.ShowControl = false;
            if (!IsPostBack)
            {
                pnlGrid.Visible = false;
                if (PageBase.SalesChanelID != 0)
                {
                    btnwarehousecode.Visible = false;
                    
                }
            }
           

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            // PageBase.Errorhandling(ex);
        }

    }
    public string Getusercode()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            string saleschannelcode1;
            ObjSalesChannel.SalesChannelID = PageBase.SalesChanelID;
            DataTable dtcode = ObjSalesChannel.GetSalesChannelInfoForStockists();
            saleschannelcode1 = Convert.ToString(dtcode.Rows[0]["SalesChannelName"]);
            return saleschannelcode1;
            
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        GridGRN.Visible = false;
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
                if (PageBase.SalesChanelID != 0)
                {
                    UploadFile.issaleschannel = true;
                }
                isSuccess = UploadFile.uploadValidExcel(ref dsGRN, "GRN");

                switch (isSuccess)
                {
                    case 0:
                        ucMsg.ShowInfo(UploadFile.Message);
                        pnlGrid.Visible = false;
                        break;
                    case 2:
                        ucMsg.ShowInfo(Resources.Messages.CheckErrorGrid);
                        Btnsave.Enabled = false;
                        pnlGrid.Visible = true;
                        GridGRN.Visible = true;

                        GridGRN.Columns[9].Visible = true;
                        GridGRN.DataSource = dsGRN;
                        GridGRN.DataBind();
                        updGrid.Update();
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
            //clsException.clsHandleException.fncHandleException(ex, "");
        }
   }



    private void InsertData(DataSet dsGRN)
    {
       
        DataTable dtGRN = dsGRN.Tables[0];
        DataColumn dcError = new DataColumn();
        dcError.DataType = System.Type.GetType("System.String");
        dcError.ColumnName = "Error";

     //   saleschannelcode = Getusercode();
          
       if (dtGRN.Columns.Contains("Error") == false)
       {
           dtGRN.Columns.Add(dcError);
       
       }
       for (int i = 0; i <= dtGRN.Rows.Count - 1; i++)
       {
           if (dtGRN != null && dtGRN.Rows.Count > 0)
           {
               if (PageBase.SalesChanelID != 0)
               {
                   string srtwe = "SalesChannelCode <> '" + PageBase.SalesChanelCode + "'";
                   DataRow[] dr1 = dtGRN.Select(srtwe);
                   if (dr1.Length > 0)
                   {
                       counter = counter + 1;
                       if (dtGRN.Rows[i]["Error"] == "" && dtGRN.Rows[i]["Error"] == string.Empty)
                       {
                           dtGRN.Rows[i]["Error"] = "Can't Upload the stock of other warehouse";
                       }
                       else
                           dtGRN.Rows[i]["Error"] += ";Can't Upload the stock of other warehouse";
                   }
               }

               string strWhere5 = "InvoiceNumber ='" + dtGRN.Rows[i]["InvoiceNumber"].ToString().Trim() +
                  "'and SalesChannelCode <>'" + dtGRN.Rows[i]["SalesChannelCode"].ToString().Trim() + "'";
               DataRow[] dr5 = dtGRN.Select(strWhere5);

               if (dr5.Length > 0)
               {
                   counter = counter + 1;
                   if (dtGRN.Rows[i]["Error"] == "" && dtGRN.Rows[i]["Error"] == string.Empty)
                   {
                       dtGRN.Rows[i]["Error"] = "Same invoice number is assigned to different Warehouses";
                   }
                   else
                       dtGRN.Rows[i]["Error"] += ";Same Invoice Number is assigned to different Warehouses";
               }

               if (Convert.ToDateTime(dtGRN.Rows[i]["GRNDate"]) < Convert.ToDateTime(dtGRN.Rows[i]["InvoiceDate"]) ||
                   Convert.ToDateTime(dtGRN.Rows[i]["GRNDate"]) < Convert.ToDateTime(dtGRN.Rows[i]["PODate"]))
               {
                   counter = counter + 1;
                   if (dtGRN.Rows[i]["Error"] == "" && dtGRN.Rows[i]["Error"] == string.Empty)
                   {
                       dtGRN.Rows[i]["Error"] = "GRN date cant be less than the Po date or Invoice Date";
                   }
                   else
                       dtGRN.Rows[i]["Error"] += ";GRN date cant be less than the Po date or Invoice Date";

               }

               if (Convert.ToDateTime(dtGRN.Rows[i]["PODate"]) > Convert.ToDateTime(dtGRN.Rows[i]["InvoiceDate"]))
               {
                   counter = counter + 1;
                   if (dtGRN.Rows[i]["Error"] == "" && dtGRN.Rows[i]["Error"] == string.Empty)
                   {
                       dtGRN.Rows[i]["Error"] = "Invoice date  cant be less than the Po date ";
                   }
                   else
                       dtGRN.Rows[i]["Error"] += ";Invoice date  cant be less than the Po date ";

               }

            
               string strWhere = "SalesChannelCode <>'" + dtGRN.Rows[i]["SalesChannelCode"].ToString().Trim() +
                   "'and GRNNumber ='" + dtGRN.Rows[i]["GRNNumber"].ToString().Trim() + "'";
               DataRow[] dr = dtGRN.Select(strWhere);

               if (dr.Length > 0)
               {
                   counter = counter + 1;
                   if (dtGRN.Rows[i]["Error"] == "" && dtGRN.Rows[i]["Error"] == string.Empty)
                   {
                       dtGRN.Rows[i]["Error"] = "Same GRN is assigned to different Warehouses";
                   }
                   else
                       dtGRN.Rows[i]["Error"] += ";Same GRN is assigned to different Warehouses";
               }
               DateTime dtGRNDate = Convert.ToDateTime(dtGRN.Rows[i]["GRNDate"]);
               TimeSpan ts = dtGRNDate.Subtract(System.DateTime.Now.Date);
               if (ts.Days > 0)
               {
                   counter = counter + 1;
                   if (dtGRN.Rows[i]["Error"] != DBNull.Value)
                       dtGRN.Rows[i]["Error"] += " GRN Date should not be greater than current date!";
                   else
                       dtGRN.Rows[i]["Error"] += ";GRN Date should not be greater than current date!";
               }

               DateTime dtPODate = Convert.ToDateTime(dtGRN.Rows[i]["PODate"]);
               TimeSpan ts1 = dtPODate.Subtract(System.DateTime.Now.Date);
               if (ts1.Days > 0)
               {
                   counter = counter + 1;
                   if (dtGRN.Rows[i]["Error"] != DBNull.Value || dtGRN.Rows[i]["Error"] != "" )
                   {

                       dtGRN.Rows[i]["Error"] += "PO Date should not be greater than current date!";
                   }
                   else
                   {
                       dtGRN.Rows[i]["Error"] += ";PO Date should not be greater than current date!";
                   }
               }

               DateTime dtInvoiceDate = Convert.ToDateTime(dtGRN.Rows[i]["InvoiceDate"]);
               TimeSpan ts2 = dtInvoiceDate.Subtract(System.DateTime.Now.Date);
               if (ts2.Days > 0)
               {
                   counter = counter + 1;
                   if (dtGRN.Rows[i]["Error"] != DBNull.Value)
                   {
                       dtGRN.Rows[i]["Error"] += " Invoice Date should not be greater than current date!";
                   }
                   else
                   {
                       dtGRN.Rows[i]["Error"] += ";Invoice Date should not be greater than current date!";
                   }
               }


               string strWhere1 = "GRNNumber='" + dtGRN.Rows[i]["GRNNumber"].ToString().Trim() + "'and GRNDate <>'" + dtGRN.Rows[i]["GRNDate"].ToString().Trim() + "'";
               if (dtGRN.Rows[i]["GRNNumber"] != DBNull.Value)
               {

                   DataRow[] dr1 = dtGRN.Select(strWhere1);
                   if (dr.Length > 0)
                   {
                       counter = counter + 1;
                       if (dsGRN.Tables[0].Rows[i]["Error"] == DBNull.Value && dtGRN.Rows[i]["Error"] == string.Empty)
                       {
                           dtGRN.Rows[i]["Error"] = "Same GRN No with multiple dates!";
                       }
                       else
                       {
                           dtGRN.Rows[i]["Error"] += ";Same GRN no with multiple dates!";
                       }

                   }

               }
           }
       }
        hideUnhideControls(dtGRN);
               
    }
           
    private void hideUnhideControls(DataTable dtGRN)
    {
        dtNew = dtGRN.Clone();
        foreach (DataColumn dc in dtNew.Columns)
        {
            if (dc.DataType == typeof(string) && dc.ColumnName == "Quantity")
            {
                dc.DataType = typeof(System.Int32);
                break;
            }
        }
        foreach (DataRow dr in dtGRN.Rows)
        {
            dtNew.ImportRow(dr);
        }
        objSum = dtNew.Compute("sum(Quantity)", "");
        if (counter == 0)
        {
            Btnsave.Enabled = true;
            GridGRN.Columns[9].Visible = false;
            if (Convert.ToInt32(objSum) <= 0)
            {
                Btnsave.Enabled = false;
                ucMsg.ShowInfo("Please Insert right Quantity");
            }
        }
        else
        {
            GridGRN.Columns[9].Visible = true;
            Btnsave.Enabled = false;
        }
        if (dtGRN.Rows.Count > 0)
        {
            dvUploadPreview.Visible = true;
            pnlGrid.Visible = true;
            Btnsave.Visible = true;
            BtnCancel.Visible = true;
            GridGRN.Visible = true;

            GridGRN.DataSource = dtGRN;
            ViewState["GRN"] = dtGRN;
            GridGRN.DataBind();
            updGrid.Update();

        }
        else
            pnlGrid.Visible = false;
    }


    protected void Btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["GRN"] != null)
            {
                int intResult = 0;
                DataTable dtGRN = new DataTable();
                DataTable Tvp = new DataTable();

                dtGRN = (DataTable)ViewState["GRN"];
                using (CommonData ObjCommom = new CommonData())
                {
                    Tvp = ObjCommom.GettvpTableGRNUpload();
                }

                foreach (DataRow dr in dtGRN.Rows)
                {
                    DataRow drow = Tvp.NewRow();

                    drow[0] = dr["SalesChannelCode"].ToString();
                    drow[1] = dr["GRNNumber"].ToString();
                    drow[2] = dr["GRNDate"];
                    drow[3] = dr["PONumber"].ToString();
                    drow[4] = dr["PODate"];
                    drow[5] = dr["InvoiceNumber"].ToString();
                    drow[6] = dr["InvoiceDate"];
                    drow[7] = dr["SKUCode"].ToString();
                    drow[8] = dr["Quantity"].ToString();
                    drow[9] = 0;
                    Tvp.Rows.Add(drow);
                }
                Tvp.AcceptChanges();
                using (SalesData objPrimarySales = new SalesData())
                {
                    objPrimarySales.EntryType = EnumData.eEntryType.eUpload;
                    objPrimarySales.UserID = PageBase.UserId;
                    intResult = objPrimarySales.InsertInfoGRNUpload(Tvp);

                    if (objPrimarySales.ErrorDetailXML != null && objPrimarySales.ErrorDetailXML != string.Empty)
                    {
                        ucMsg.XmlErrorSource = objPrimarySales.ErrorDetailXML;
                        return;
                    }
                    if (objPrimarySales.Error != null && objPrimarySales.Error != "")
                    {
                        ucMsg.ShowError(objPrimarySales.Error);
                        return;
                    }
                    if (intResult == 2)
                    {
                        ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                        return;
                    }

                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    ClearForm();


                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }

    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        ClearForm();

    }

    void ClearForm()
    {
        pnlGrid.Visible = false;
        updGrid.Update();
    }

    protected void GridGRN_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridGRN.PageIndex = e.NewPageIndex;
            DataTable dt = new DataTable();
            GridGRN.DataSource = (DataTable)ViewState["GRN"];
            GridGRN.DataBind();
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
            DataTable dt;
            DataSet ds = new DataSet(); 
            DataSet dsReferenceCode = new DataSet();
            using (ProductData objProductData = new ProductData())
            {
                dt = objProductData.SelectAllSKUInfo();
                dt.DefaultView.RowFilter = "Status = True";
                dt = dt.DefaultView.ToTable();
                string[] strCode = new string[] { "SKUCode","SKUName"};
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
            // PageBase.Errorhandling(ex);
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
             dt.DefaultView.RowFilter = "Status = True";
             dt = dt.DefaultView.ToTable();
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
}
