using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using ExportExcelOpenXML;
using System.Text;
using BusinessLogics;
using System.Collections;
using System.Data.SqlClient;
using ZedService;

/*Change Log:
 * 08-Jan-14, Sumit Kumar, #CC01 - Add UploadFile Control for upload Serial Number for AckIMEI
 */


public partial class Transactions_SalesChannelSB_Interface_ManageSalesAcknowledge : PageBase
{

    DateTime dt = new DateTime();
    DataTable dtNew = new DataTable();
    int intSelectedValue;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMsg.ShowControl = false;
            if (!IsPostBack)
            {
                Session["ID"] = null;
                if (pnldetail.Visible == true)
                {
                    pnldetail.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }

    }


    bool pageValidateSave()
    {
        if (ucFromDate.Date == "" && ucToDate.Date == "" && txtInvoice.Text.Trim() == "" && ddlStatus.SelectedValue == "101")
        {
            ucMsg.ShowInfo(Resources.Messages.MandatoryField);
            return false;
        }
        if (ucFromDate.Date != "")
        {
            if (ucToDate.Date == "")
            {
                ucMsg.ShowInfo(Resources.Messages.InvalidDate);
                return false;
            }
        }
        if (ucToDate.Date != "")
        {
            if (ucFromDate.Date == "")
            {
                ucMsg.ShowInfo(Resources.Messages.InvalidDate);
                return false;
            }
        }
        if (ucToDate.Date != "" && ucFromDate.Date != "")
        {
            if (Convert.ToDateTime(ucFromDate.Date) > Convert.ToDateTime(ucToDate.Date))
            {
                ucMsg.ShowInfo(Resources.Messages.InvalidDate);
                return false;
            }
        }//Pankaj Kumar


        return true;
    }
    bool ChckDateInput()
    {
        DateTime dateTime;
        if (ucFromDate.Date != "")
        {
            if (!DateTime.TryParse(ucFromDate.Date, out dateTime))
            {
                return false;
            }
        }
        if (ucToDate.Date != "")
        {
            if (!DateTime.TryParse(ucToDate.Date, out dateTime))
            {
                return false;
            }
        }
        return true;
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        ClearForm();
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            ClearOutput();
            dtNew = new DataTable();
            if (!ChckDateInput())
            {
                ucMsg.ShowError("Invalid Date format");
                return;
            }
            if (pageValidateSave())
            {
                dtNew = GetInformationForAcknowledge(2, 0, 0);
                if (dtNew.Rows.Count > 0)
                {
                    ViewState["Type"] = dtNew.Rows[0]["Type"].ToString();
                    gvAck.DataSource = dtNew;
                    gvAck.DataBind();
                    pnlGrid.Visible = true;

                }
                else
                {
                    ucMsg.ShowInfo(Resources.Messages.NoRecord);
                }
                upddetails.Update();
            }
            else
            {
                ucMsg.ShowError("Invalid Searching Parameters");
            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void ExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtExcel = GetInformationForAcknowledge(0, 0, 1);
            if (dtExcel.Rows.Count > 0)
            {
                DataSet dsExcel = new DataSet();
                dsExcel.Merge(dtExcel);
                dsExcel.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "PurchaseAcknowledge";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dsExcel, FilenameToexport);
            }
            else
            {
                ucMsg.ShowError(Resources.Messages.NoRecord);

            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearForm();

    }

    void ClearForm()
    {
        ucFromDate.imgCal.Enabled = true;
        ucFromDate.TextBoxDate.Enabled = true;
        ucFromDate.TextBoxDate.Text = "";
        ucToDate.imgCal.Enabled = true;
        ucToDate.TextBoxDate.Enabled = true;
        ucToDate.TextBoxDate.Text = "";
        pnlGrid.Visible = false;
        pnldetail.Visible = false;
        ddlStatus.SelectedValue = "101";
        txtInvoice.Text = "";
        updGrid.Update();
        upddetails.Update();
    }

    void ClearOutput()
    {
        pnlGrid.Visible = false;
        pnldetail.Visible = false;
        updGrid.Update();
        upddetails.Update();
        ViewState["Type"] = null;
    }

    protected void btnAccept_Click(object sender, EventArgs e)
    {
        try
        {
            ucMsg.Visible = false;
            /*#CC01 Added  Start*/

            if (ViewState["IMEIAck"] != null && ViewState["IMEIAck"] != "")
            {
                string IMEIAck = ViewState["IMEIAck"].ToString().Trim();
                if (IMEIAck == "4")
                {
                    if (flUpload.HasFile)
                    {
                        OpenXMLExcel objexcel = new OpenXMLExcel();
                        Int16 Upload = 0;
                        string strUploadedFileName = string.Empty;
                        UploadFile UploadFile = new UploadFile();
                        DataSet DsValidate = new DataSet();
                        Upload = UploadFile.IsExcelFile(flUpload, ref strUploadedFileName);
                        if (Upload == 1)
                        {
                            DataSet DsExcel = objexcel.ImportExcelFile(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);
                            if (DsExcel != null && DsExcel.Tables.Count > 0 && DsExcel.Tables[0].Rows.Count > 0)
                            {
                                using (CommonData objSchema = new CommonData())
                                {
                                    DataSet objdsSchema = objSchema.GetUploadFileSchema("PrimaryIMEIAck");
                                    if (objdsSchema.Tables[0].Rows.Count > 0)
                                    {


                                        #region Validate schema is match or not
                                        int Matchedschema = 0;
                                        for (int sc = 0; sc < objdsSchema.Tables[0].Rows.Count; sc++)
                                        {
                                            for (int exlsc = 0; exlsc < DsExcel.Tables[0].Columns.Count; exlsc++)
                                            {
                                                if (Convert.ToString(objdsSchema.Tables[0].Rows[sc]["ColumnName"]).ToUpper() == DsExcel.Tables[0].Columns[exlsc].ColumnName.ToUpper())
                                                {
                                                    Matchedschema = Matchedschema + 1;
                                                }
                                            }

                                        }

                                        if (Matchedschema != objdsSchema.Tables[0].Rows.Count)
                                        {
                                            ucMsg.ShowError("Column Name mismatched in excel file.");
                                            return;
                                        }

                                        #endregion


                                        DsValidate = DsExcel;
                                        DataTable objDtExcelSheet = DsExcel.Tables[0].Copy();
                                        string strColName = string.Empty;
                                        string strMinLength = "Please define Min ";
                                        string strMaxLength = "Please define Max ";
                                        string strMSgComplete = " characters for this field ";

                                        if (DsValidate.Tables[0].Columns.Contains("Error") == false)
                                        {
                                            DataColumn dcError = new DataColumn();
                                            dcError = new DataColumn("Error", typeof(string));
                                            DsValidate.Tables[0].Columns.Add(dcError);
                                            DsValidate.Tables[0].AcceptChanges();
                                        }


                                        #region validate Data Type of excel file
                                        DataView objDV = objdsSchema.Tables[0].DefaultView;
                                        for (int intTmpVar = 0; intTmpVar < DsExcel.Tables[0].Rows.Count; intTmpVar++)
                                        {
                                            #region Chk Minimum length
                                            objDV.RowFilter = "  MinLength > 0 ";
                                            foreach (DataRowView drv in objDV)
                                            {
                                                strColName = Convert.ToString(drv["ColumnName"]);
                                                if ((objDtExcelSheet.Rows[intTmpVar][strColName] == DBNull.Value)
                                                    || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length == 0)
                                                    || (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length) == Convert.ToString(drv.Row["MinLength"]))
                                                    )
                                                {
                                                }
                                                else if (objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length < Convert.ToInt32(drv.Row["MinLength"]))
                                                {
                                                    string strErrorMsg = strMinLength + Convert.ToString(drv.Row["MinLength"]) + strMSgComplete + "[ " + strColName + " ]";
                                                    if (DsValidate.Tables[0].Rows[intTmpVar]["Error"] == DBNull.Value || DsValidate.Tables[0].Rows[intTmpVar]["Error"] == "")
                                                    {
                                                        DsValidate.Tables[0].Rows[intTmpVar]["Error"] = strErrorMsg;
                                                    }
                                                    else
                                                    {
                                                        DsValidate.Tables[0].Rows[intTmpVar]["Error"] = DsValidate.Tables[0].Rows[intTmpVar]["Error"] + "," + strErrorMsg;
                                                    }
                                                }
                                            }
                                            #endregion
                                            #region Chk Maximum length
                                            objDV.RowFilter = "  MaxLength > 0 ";
                                            foreach (DataRowView drv in objDV)
                                            {
                                                strColName = Convert.ToString(drv["ColumnName"]);
                                                if ((objDtExcelSheet.Rows[intTmpVar][strColName] == DBNull.Value)
                                                    || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length == 0)
                                                    || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length <= Convert.ToInt32(drv.Row["MaxLength"]))
                                                    )
                                                {
                                                }
                                                else
                                                {
                                                    string strErrorMsg = strMaxLength + Convert.ToString(drv.Row["MaxLength"]) + strMSgComplete + "[ " + strColName + " ]";
                                                    if (DsValidate.Tables[0].Rows[intTmpVar]["Error"] == DBNull.Value || DsValidate.Tables[0].Rows[intTmpVar]["Error"] == "")
                                                    {
                                                        DsValidate.Tables[0].Rows[intTmpVar]["Error"] = strErrorMsg;
                                                    }
                                                    else
                                                    {
                                                        DsValidate.Tables[0].Rows[intTmpVar]["Error"] = DsValidate.Tables[0].Rows[intTmpVar]["Error"] + "," + strErrorMsg;
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                        #region Chk Duplicate Record
                                        DataSet dsDup = new DataSet();
                                        dsDup = DsExcel.Copy();
                                        var duplicates = DsExcel.Tables[0].AsEnumerable()
                                                              .GroupBy(r => new { IMIE = r[0].ToString() })//Using Column Index
                                                              .Where(gr => gr.Count() > 1)
                                                              .Select(g => g.Key);



                                        foreach (var d in duplicates)
                                        {
                                            for (int row = 0; row < dsDup.Tables[0].Rows.Count; row++)
                                            {
                                                string IMIE = dsDup.Tables[0].Rows[row][1].ToString();
                                                if ((d.IMIE.ToString() == IMIE && d.IMIE.ToString() != ""))
                                                {
                                                    string strErrorMsg = "IMIE Number already exists in excel file.";
                                                    if (DsValidate.Tables[0].Rows[row]["Error"] == DBNull.Value || DsValidate.Tables[0].Rows[row]["Error"] == "")
                                                    {
                                                        DsValidate.Tables[0].Rows[row]["Error"] = strErrorMsg;
                                                    }
                                                    else
                                                    {
                                                        DsValidate.Tables[0].Rows[row]["Error"] = DsValidate.Tables[0].Rows[row]["Error"] + "," + strErrorMsg;
                                                    }

                                                }

                                            }
                                        }
                                        #endregion
                                        #endregion
                                    }

                                }
                                if (DsValidate.Tables[0].Select("isnull(Error,'')<>''").Length > 0)
                                {
                                    ucMsg.XmlErrorSource = DsValidate.GetXml();
                                    return;
                                }
                                else
                                {
                                    string AckSessionID = Guid.NewGuid().ToString();
                                    DataTable dtExcelData = new DataTable();
                                    dtExcelData.Columns.Add("IMEI", typeof(string));
                                    dtExcelData.Columns.Add("SalesChannelID", typeof(string));
                                    dtExcelData.Columns.Add("AckSessionID", typeof(string));

                                    for (int _row = 0; _row < DsExcel.Tables[0].Rows.Count; _row++)
                                    {
                                        dtExcelData.Rows.Add(DsExcel.Tables[0].Rows[_row]["IMEI"].ToString().Trim(), PageBase.SalesChanelID, AckSessionID);
                                    }
                                    using (SalesData obj = new SalesData())
                                    {
                                        if (obj.Insert_BCP_IMEIAck(dtExcelData))
                                        {
                                            AcknowlegementAction(2, AckSessionID);
                                        }
                                    }

                                }


                            }
                            else
                            {
                                ucMsg.ShowError("please upload valid Serial number..");
                                return;
                            }
                        }
                    }
                    else
                    {
                        ucMsg.ShowError("Please select excel file.");
                        return;
                    }
                } /*#CC01 Added  end*/
                else
                {
                    AcknowlegementAction(2, "");
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        try
        {
            AcknowlegementAction(3, "");
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }


    protected void gvAck_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            dtNew = new DataTable();
            Int32 id = Convert.ToInt32(e.CommandArgument);
            Session["ID"] = id;
            if (e.CommandName == "Details")
            {
                Button imgbtn = (Button)e.CommandSource;      //Only this code will change the Selected row css
                GridViewRow grdrow = (GridViewRow)imgbtn.NamingContainer;
                GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                string strValue = ((Label)(row.Cells[0].FindControl("lblReceived"))).Text;
                dtNew = GetInformationForAcknowledge(1, id, 0);
                HiddenField hdnIMEIAck = (HiddenField)row.FindControl("hdnIMEIAck");
                ViewState["IMEIAck"] = hdnIMEIAck.Value;

                if (Convert.ToInt32(strValue) != 0)
                {
                    btnAccept.Visible = false;
                    btnReject.Visible = false;
                    pnlIMEIAck.Visible = false;
                    ViewState["IMEIAck"] = "2";
                    tdPanel.Visible = false;
                }
                else
                {
                    btnAccept.Visible = true;
                    btnReject.Visible = true;
                    /*#CC01 Added  Start*/
                    
                    if (hdnIMEIAck.Value == "4")
                    {
                        pnlIMEIAck.Visible = true;
                        tdPanel.Visible = true;
                    }
                    else
                    {
                        pnlIMEIAck.Visible = false;
                        tdPanel.Visible = false;
                    }
                    /*#CC01 Added  End*/
                }
                if (dtNew.Rows.Count > 0)
                {
                    grdDetails.DataSource = dtNew;
                    grdDetails.DataBind();
                    pnldetail.Visible = true;

                }
                else
                {
                    grdDetails.DataSource = null;
                    grdDetails.DataBind();
                    pnldetail.Visible = true;
                }
                upddetails.Update();

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }

    private void AcknowlegementAction(int value, string IMEISessionID)
    {

        using (SalesData obj = new SalesData())
        {
            obj.value = value;
            obj.SalesUniqueID = Convert.ToInt32(Session["ID"]);
            obj.UserID = PageBase.UserId;
            obj.Decider = Convert.ToInt32(ViewState["Type"]);
            obj.SalesChannelID = PageBase.SalesChanelID;
            obj.OtherEntity = PageBase.BaseEntityTypeID;
            obj.IMEISessionID = IMEISessionID;
            int result = obj.InsertAcknowlegeInformationSB_V1();
            if (result == 0)
            {
                //Session["ID"] = null;
                if (value == 2)
                    ucMsg.ShowSuccess(Resources.Messages.ApproveSuccess);
                else if (value == 3)
                    ucMsg.ShowSuccess(Resources.Messages.RejectSuccess);

            }
            if (result == 1)
                ucMsg.ShowError(obj.Error.ToString());
            if (result == 4)
                ucMsg.ShowInfo(Resources.GlobalMessages.InvoiceAlreadyCancelled);
            if (obj.ErrorDetailXML != null && obj.ErrorDetailXML != string.Empty)
            {
                ucMsg.XmlErrorSource = obj.ErrorDetailXML;
            }
            ClearForm();
            ClearOutput();


        }
    }
    protected void grdDetails_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
    {
        try
        {
            if (!e.Expanded) return;
            grdDetails.DetailRows.CollapseAllRows();
            grdDetails.DetailRows.ExpandRow(e.VisibleIndex);
            grdDetails.DetailRows.IsVisible(e.VisibleIndex);
            ASPxGridView objDetail = (ASPxGridView)(sender as ASPxGridView).FindDetailRowTemplateControl(e.VisibleIndex, "detailGrid");
            objDetail.DataSource = Session["Detail"];
            objDetail.DataBind();
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }

    }
    protected void detailGrid_DataSelect(object sender, EventArgs e)
    {
        try
        {
            dtNew = new DataTable();
            intSelectedValue = Convert.ToInt32((sender as ASPxGridView).GetMasterRowKeyValue());
            dtNew = GetInformationForAcknowledge(3, intSelectedValue, 0);
            ASPxGridView objDetail = (ASPxGridView)(sender as ASPxGridView);
            objDetail.DataSource = dtNew;
            Session["Detail"] = dtNew;
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }

    }


    private DataTable GetInformationForAcknowledge(int value, int uniqueID, Int16 FromExportToExcel)
    {
        DataTable dt = new DataTable();
        using (SalesData objSales = new SalesData())
        {
            if (txtInvoice != null)
            {
                objSales.SalesChannelID = PageBase.SalesChanelID;
                objSales.OtherEntity = PageBase.BaseEntityTypeID;
                objSales.UserID = PageBase.UserId;
                objSales.InvoiceNumber = txtInvoice.Text.Trim();
                objSales.FromDate = ucFromDate.Date != "" ? Convert.ToDateTime(ucFromDate.Date) : objSales.InvoiceDate;
                objSales.ToDate = ucToDate.Date != "" ? Convert.ToDateTime(ucToDate.Date) : objSales.InvoiceDate;
                objSales.AckStatus = Convert.ToInt32(ddlStatus.SelectedValue);
                objSales.FlagForTable = value;
                objSales.SalesUniqueID = uniqueID;
                if (FromExportToExcel == 1)
                    dt = objSales.GetDataForAcknowlegementExportToExcel();
                else
                    dt = objSales.GetDataForAcknowlegement();
            }
        }
        return dt;
    }
    protected void gvAck_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dtNew = new DataTable();
            gvAck.PageIndex = e.NewPageIndex;
            dtNew = GetInformationForAcknowledge(2, 0, 0);
            if (dtNew.Rows.Count > 0)
            {
                gvAck.DataSource = dtNew;
                gvAck.DataBind();
                pnlGrid.Visible = true;

            }
            else
            {
                ucMsg.ShowInfo(Resources.Messages.NoRecord);
            }
            updGrid.Update();
        }
        catch (Exception ex)
        {

            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
    protected void grdDetails_DetailRowGetButtonVisibility(object sender, ASPxGridViewDetailRowButtonEventArgs e)
    {
        try
        {
            ASPxGridView grid = (ASPxGridView)sender;

            if (grid.GetRow(e.VisibleIndex) != null)
            {
                if (Convert.ToInt32(((System.Data.DataRowView)(grid.GetRow(e.VisibleIndex))).Row.ItemArray[7]) == 1)
                {
                    e.ButtonState = GridViewDetailRowButtonState.Hidden;
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }

    /*#CC01 Added  Start*/
    protected void grdDetails_DataBound(object sender, EventArgs e)
    {

        if (ViewState["IMEIAck"] != null && ViewState["IMEIAck"] != "")
        {
            string IMEIAck = ViewState["IMEIAck"].ToString().Trim();
            if (IMEIAck == "4")
            {
                grdDetails.SettingsDetail.ShowDetailRow = false;
            }
            else
            {
                grdDetails.SettingsDetail.ShowDetailRow = true;
            }
        }

    }
    /*#CC01 Added  end*/



    protected void page_Init(object sender, EventArgs e)
    {
        try
        {
            if (Session["ID"] != null)
            {
                dtNew = GetInformationForAcknowledge(1, Convert.ToInt32(Session["ID"]), 0);
                if (grdDetails != null)
                {
                    if (dtNew.Rows.Count > 0)
                    {
                        grdDetails.DataSource = dtNew;
                        grdDetails.DataBind();
                        pnldetail.Visible = true;
                    }
                    else
                    {
                        grdDetails.DataSource = null;
                        grdDetails.DataBind();
                        pnldetail.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }

    }





}
