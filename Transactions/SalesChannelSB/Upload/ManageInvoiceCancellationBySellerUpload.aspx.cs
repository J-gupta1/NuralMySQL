using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;
using ExportExcelOpenXML;
using System.Text;
using BusinessLogics;
using System.Collections;
using System.Reflection;
using ZedService;

public partial class Transactions_SalesChannelSB_Upload_ManageInvoiceCancellationBySellerUpload : PageBase
{
    DataTable dtNew = new DataTable();
    object objSum;
    int counter = 0;
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    UploadFile UploadFile = new UploadFile();
    List<String> lstDuplicate = new List<String>();
    DataSet dsErrorProne = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMsg.ShowControl = false;
            if (!IsPostBack)
            {
                
                SalesType();
                pnlGrid.Visible = false;
               
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
            DataTable dtErrorTable = GetBlankTableError();
            DataTable dtError = new DataTable();
            HttpContext.Current.Session["PkeyColumns"] = null;
            string strKey = string.Empty;
            ClearForm();
            hlnkInvalid.Visible = false;
            Int16 Upload = 0;
            Upload = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            ViewState["TobeUploaded"] = strUploadedFileName;
            if (Upload == 1)
            {
                OpenXMLExcel objexcel = new OpenXMLExcel();
                DataSet DsExcel = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);

                if (DsExcel != null && DsExcel.Tables.Count > 0 && DsExcel.Tables[0].Rows.Count > 0)
                {

                    if (DsExcel.Tables[0].Rows.Count > Convert.ToInt32(PageBase.ValidExcelRows))
                        ucMsg.ShowInfo("Limit Crossed");
                    else
                    {
                        ValidateUploadFile objValidateFile = new ValidateUploadFile();
                        {
                            DataSet objDS = DsExcel;
                            DataTable dt1 = DsExcel.Tables[0];
                            SortedList objSL = new SortedList();
                            SortedList objSLCorrData = new SortedList();
                            objValidateFile.UploadedFileName = strUploadedFileName;
                            objValidateFile.ExcelFileNameInTable = "InvoiceCancellationSB";
                            objValidateFile.ValidateFile(false, out objDS, out objSL);
                            if (objValidateFile.Message != null && objValidateFile.Message.Trim() != "")
                                ucMsg.ShowInfo(objValidateFile.Message);
                            else
                            {
                                ucMsg.Visible = false;
                                bool blnIsUpload = true;
                                if (objSL.Count >= 1 && objSL.Keys.Count >= 1)
                                {
                                    objDS.Tables["DtExcelSheet"].Columns.Add(new DataColumn("ReasonForInvalid"));
                                    IDictionaryEnumerator objIDicEnum = objSL.GetEnumerator();
                                    while (objIDicEnum.MoveNext())
                                    {
                                        string[] strpkeyColumnName = Convert.ToString(HttpContext.Current.Session["PkeyColumns"]).Split(',');
                                        if (HttpContext.Current.Session["PkeyColumns"] != null)
                                        {
                                            for (int i = 0; i <= objDS.Tables["DtExcelSheet"].Rows.Count - 1; i++)
                                            {
                                                strKey = string.Empty;
                                                for (int j = 0; j <= strpkeyColumnName.Length - 1; j++)
                                                {
                                                    if (strKey == string.Empty)
                                                        strKey = objDS.Tables["DtExcelSheet"].Rows[i][strpkeyColumnName[j]].ToString();
                                                    else
                                                        strKey = strKey + objDS.Tables["DtExcelSheet"].Rows[i][strpkeyColumnName[j]].ToString();
                                                }
                                                if (strKey == objIDicEnum.Key.ToString())
                                                {
                                                    objDS.Tables["DtExcelSheet"].Rows[i]["ReasonForInvalid"] = objIDicEnum.Value.ToString();
                                                }
                                            }
                                        }
                                    }

                                    objDS.Tables[0].AcceptChanges();
                                    if (objDS.Tables["DtExcelSheet"] != null && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                                    {
                                        hlnkInvalid.Visible = true;
                                        dsErrorProne.Merge(objDS.Tables["DtExcelSheet"]);
                                        hlnkInvalid.Text = "Invalid Data";

                                        blnIsUpload = false;
                                    }
                                    blnIsUpload = false;
                                }
                                if (!objDS.Tables[0].Columns.Contains("ReasonForInvalid"))
                                {
                                    int counter = 0;
                                    if (!objDS.Tables[0].Columns.Contains("ReasonForInvalid"))
                                        objDS.Tables[0].Columns.Add(new DataColumn("ReasonForInvalid"));
                                    if (PageBase.SalesChanelID != 0)
                                    {
                                        var query = from r in objDS.Tables[0].AsEnumerable()
                                                    where ((Convert.ToString(r["FromSalesChannelCode"]) != PageBase.SalesChanelCode))
                                                    select new
                                                    {
                                                        InvoiceNumber = Convert.ToString(r["InvoiceNumber"]),
                                                        FromSalesChannelCode = Convert.ToString(r["FromSalesChannelCode"]),
                                                        ToSalesChannelCode = Convert.ToString(r["ToSalesChannelCode"])
                                                    };
                                        if (query != null)
                                        {
                                            if (query.Count() > 0)
                                            {
                                                counter = 1;
                                                dtError = PageBase.LINQToDataTable(query);
                                                foreach (DataRow dr in dtError.Rows)
                                                {
                                                    DataRow drow = dtErrorTable.NewRow();
                                                    drow["InvoiceNumber"] = dr["InvoiceNumber"];
                                                    drow["FromSalesChannelCode"] = dr["FromSalesChannelCode"];
                                                    drow["ToSalesChannelCode"] = dr["ToSalesChannelCode"];
                                                    drow["ReasonForInvalid"] = "Can't Upload the Invoices of other SalesChannel";
                                                    dtErrorTable.Rows.Add(drow);
                                                }
                                                dtErrorTable.AcceptChanges();
                                            }
                                        }
                                    }
                                    //Invoices Information Where SalesChannelFrom and ToSalesChannel is same
                                    var SameData = from r in objDS.Tables[0].AsEnumerable()
                                                   where ((Convert.ToString(r["FromSalesChannelCode"]) == (Convert.ToString(r["ToSalesChannelCode"]))))
                                                   select new
                                                   {
                                                       InvoiceNumber = Convert.ToString(r["InvoiceNumber"]),
                                                       FromSalesChannelCode = Convert.ToString(r["FromSalesChannelCode"]),
                                                       ToSalesChannelCode = Convert.ToString(r["ToSalesChannelCode"])
                                                   };
                                    if (SameData != null)
                                    {
                                        if (SameData.Count() > 0)
                                        {
                                            counter = 1;
                                            dtError = PageBase.LINQToDataTable(SameData);
                                            foreach (DataRow dr in dtError.Rows)
                                            {
                                                DataRow drow = dtErrorTable.NewRow();
                                                drow["InvoiceNumber"] = dr["InvoiceNumber"];
                                                drow["FromSalesChannelCode"] = dr["FromSalesChannelCode"];
                                                drow["ToSalesChannelCode"] = dr["ToSalesChannelCode"];
                                                drow["ReasonForInvalid"] = "Invoices Information is Wrong";
                                                dtErrorTable.Rows.Add(drow);
                                            }
                                            dtErrorTable.AcceptChanges();
                                        }
                                    }
                                   if (counter > 0)
                                    {
                                        ucMsg.ShowInfo("Invalid Records");
                                        dsErrorProne.Merge(dtErrorTable);
                                        blnIsUpload = false;
                                    }
                                    else
                                    {
                                        objDS.Tables[0].Columns.Remove("ReasonForInvalid");
                                    }
                                }

                                if (objDS.Tables["DtDuplicateRecord"] != null && objDS.Tables["DtDuplicateRecord"].Rows.Count > 0)
                                {
                                    dsErrorProne.Merge(objDS.Tables["DtDuplicateRecord"]);
                                    blnIsUpload = false;
                                }
                                if (objDS.Tables["DtBlankData"] != null && objDS.Tables["DtBlankData"].Rows.Count > 0)
                                {
                                    dsErrorProne.Merge(objDS.Tables["DtBlankData"]);
                                    blnIsUpload = false;
                                }
                                if (blnIsUpload)
                                {
                                    if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                                    {
                                        InsertData(objDS.Tables[0]);
                                    }
                                    else
                                        ucMsg.ShowInfo(Resources.Messages.NoRecord);
                                }
                                else
                                {
                                    hlnkInvalid.Visible = true;
                                    string strFileName = "InvalidData" + DateTime.Now.Ticks;
                                    ExportInExcel(dsErrorProne, strFileName);
                                    hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                    hlnkInvalid.Text = "Invalid Data";
                                }



                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }


    private void SalesType()
    {
        try
        {
            DataTable dt = new DataTable();
            ddlSalesType.Items.Clear();
            using (ReportData objSalesType = new ReportData())
            {
                objSalesType.SalesChannelTypeID = PageBase.SalesChanelTypeID;
                objSalesType.HierarchyLevelId = PageBase.HierarchyLevelID;
                dt = objSalesType.GetSalesTypeforReport();
            };
            String[] colArray = { "SalesTypeID", "SalesTypeName" };
            PageBase.DropdownBinding(ref ddlSalesType, dt, colArray);

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

    private void InsertData(DataTable dtGRN)
    {
        try
        {
            if (dtGRN.Rows.Count > 0)
            {
                dvUploadPreview.Visible = true;
              
              
                Btnsave.Enabled = true;
                GridGRN.DataSource = dtGRN;
                GridGRN.DataBind();
                pnlGrid.Visible = true;
                Btnsave.Enabled = true;
                //BtnCancel.Visible = true;
                updGrid.Update();
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }

    }


    protected void Btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }
            if (ddlSalesType.SelectedValue == "0")
            {
                ucMsg.ShowInfo("Please Select Sales Type");
                return;
            }
            if (ViewState["TobeUploaded"] != null)
            {
                int intResult = 0;
                DataTable Tvp = new DataTable();
                DataSet DsExcelDetail = new DataSet();
                using (CommonData ObjCommom = new CommonData())
                {
                    Tvp = ObjCommom.GettvpInvoiceCancellationSB();
                }
                OpenXMLExcel objexcel = new OpenXMLExcel();
                DsExcelDetail = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + ViewState["TobeUploaded"].ToString());

                foreach (DataRow dr in DsExcelDetail.Tables[0].Rows)
                {
                    DataRow drow = Tvp.NewRow();
                    drow["FromSalesChannelCode"] = dr["FromSalesChannelCode"].ToString().Trim();
                    drow["ToSalesChannelCode"] = dr["ToSalesChannelCode"].ToString().Trim();
                    drow["InvoiceNumber"] = dr["InvoiceNumber"].ToString().Trim();
                    Tvp.Rows.Add(drow);
                }
                Tvp.AcceptChanges();
                using (SalesData objP1 = new SalesData())
                {
                    objP1.EntryType = EnumData.eEntryType.eUpload;
                    objP1.UserID = PageBase.UserId;
                    objP1.SalesChannelID = PageBase.SalesChanelID;
                    if(PageBase.SalesChanelID!=0)
                    objP1.Decider = PageBase.SalesChannelLevel == 4 ? 3 : PageBase.SalesChannelLevel == 1 ? 1 : 2;
                    /*Now this Decider variable is useless now it is being fetched in the Procedure*/
                    objP1.OtherEntity = PageBase.BaseEntityTypeID;
                    objP1.SalesTypeCancellation = Convert.ToInt16(ddlSalesType.SelectedValue);
                    
                    intResult = objP1.InsertCancellationInformationBulkSB(Tvp);

                    if (objP1.ErrorDetailXML != null && objP1.ErrorDetailXML != string.Empty)
                    {
                        ucMsg.XmlErrorSource = objP1.ErrorDetailXML;
                        return;
                    }
                    if (objP1.Error != null && objP1.Error != "")
                    {
                        ucMsg.ShowError(objP1.Error);
                        return;
                    }
                    if (intResult == 2)
                    {
                        ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                        return;
                    }
                    ClearForm();
                    ucMsg.ShowSuccess(Resources.Messages.InvoiceCancelled);

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
        ClearForm();

    }

    void ClearForm()
    {
        dvUploadPreview.Visible = false;
        GridGRN.DataSource = null;
        GridGRN.DataBind();
        pnlGrid.Visible = false;
        ucMsg.Visible = false;
        updGrid.Update();
    }

    protected void GridGRN_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            //GridGRN.PageIndex = e.NewPageIndex;
            //DataTable dt = new DataTable();
            //GridGRN.DataSource = (DataTable)ViewState["GRN"];
            //GridGRN.DataBind();
        }

        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }


    
    private void ExportInExcel(DataSet DsError, string strFileName)
    {
        if (DsError != null && DsError.Tables.Count > 0)
        {
            PageBase.ExportToExeclV2(DsError, strFileName, DsError.Tables.Count);
        }
    }
    protected void rdModelList_SelectedIndexChanged(object sender, EventArgs e)
   {
        if (rdModelList.SelectedValue == "1")
            Response.Redirect("~/Transactions/SalesChannelSB/Interface/ManageInvoiceCancellationBySeller.aspx");
    }

    public DataTable GetBlankTableError()
    {
        DataTable Detail = new DataTable();
        Detail.Columns.Add("FromSalesChannelCode");
        Detail.Columns.Add("InvoiceNumber");
        Detail.Columns.Add("ToSalesChannelCode");
        Detail.Columns.Add("ReasonForInvalid");
        return Detail;
    }
    protected void DwnldReferenceCodeTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (SalesData objP1 = new SalesData())
            {
                objP1.UserID = PageBase.UserId;
                objP1.SalesChannelID = PageBase.SalesChanelID;
                objP1.Decider = PageBase.SalesChannelLevel == 4 ? 3 : PageBase.SalesChannelLevel == 1 ? 1 : 2;
                objP1.OtherEntity = PageBase.BaseEntityTypeID;
                dsTemplateCode = objP1.GetInvoiceListTobeCancelled();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Invoice List";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dsTemplateCode, FilenameToexport, EnumData.eTemplateCount.ePrice+2);
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
