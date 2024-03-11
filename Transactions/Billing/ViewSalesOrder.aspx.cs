#region Copyright(c) 2010 Zed-Axis Technologies All rights are reserved
/*/
* ==============================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2010 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ==============================================================================================================
* Created By : Vijay Kumar Prajapati 18-april-2019
* Module : 
* Description : 
 * Table Name: 
* ==============================================================================================================
* Reviewed By :
 ===============================================================================================================
 * Change Log:
   ----------
 * =============================================================================================================
*/
#endregion


using Resources;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;

public partial class Order_Common_ViewSalesOrder : PageBase
{

    string strPrintPatch = string.Empty;
    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            grdItemDetails.Columns[0].HeaderText = "SKU Code";
            if (!IsPostBack)
            {
                BindOrderTypes();
                CreditCheckText();
                reqOrderType.ErrorMessage = "Please select an option.";
            }
            divCancel.Visible = false;
            updCancelRemarks.Update();
        }
        catch (Exception ex)
        {
            ucMessage1.ShowWarning(ex.Message);
        }
    }
    protected void ddlOrderType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cmbOrderFrom.BusinessEventKeyword = Enum.GetName(typeof(ZedAxis.ZedEBS.Enums.BusinessEventsKeyword), ZedAxis.ZedEBS.Enums.BusinessEventsKeyword.SOGENERATE).ToString();
            cmbOrderFrom.EntityTypeDescription = Convert.ToInt32(ZedAxis.ZedEBS.Enums.EntityTypeDescription.IncludingEntityType);
            cmbOrderFrom.SelectionMode = ZedAxis.ZedEBS.Enums.EnumSelectionMode.Single;
            cmbOrderFrom.Keyword = ZedAxis.ZedEBS.Enums.EntityTypeKeyword.SALECHANNEL;
            cmbOrderFrom.IsParent = ucEntityList_ver2.Parent.True;

            if (BaseEntityTypeID == 1)
                cmbOrderFrom.LoggedInEntityID = PageBase.SalesChanelID;
            else
                cmbOrderFrom.LoggedInEntityID = 0;
            cmbOrderFrom.SelectionMode = ZedAxis.ZedEBS.Enums.EnumSelectionMode.CheckColumn;

            cmbOrderFrom.Type = (ddlOrderType.SelectedValue.Trim() == "1") ? ucEntityList_ver2.EntityType.SECONDARY : ucEntityList_ver2.EntityType.PRIMARY;
            cmbOrderFrom.ForceTypeMatching = 1;
            cmbOrderFrom.Clear();
            cmbOrderFrom.BindServiceCentre();
            LoadChild();

            liParentHeader.InnerText = (ddlOrderType.SelectedValue.Trim() == "1") ? "Order From:" : "Order To:";
            liChildHeader.InnerText = (ddlOrderType.SelectedValue.Trim() == "1") ? "Order To:" : "Order From:";

        }
        catch (Exception ex)
        {
            ucMessage1.ShowWarning(ex.Message);
        }
    }

    protected void UCPagingControl1_SetControlRefresh()
    {
        try
        {
            using (clsSalesOrder obj = new clsSalesOrder())
            {
                int intPageNumber = ucPagingControl1.CurrentPage;
                ViewState["PageIndex"] = intPageNumber;
                obj.PageIndex = intPageNumber;
                BindList(ucPagingControl1.CurrentPage);
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowWarning(ex.Message);
        }
    }

    protected void grdvList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Int32 id = 0;
            if (e.CommandName != "PrintInvoice" && e.CommandName != "PrintDispatch")
            {
                id = Convert.ToInt32(e.CommandArgument);
                ViewState["OrderId"] = id;


            }

            if (e.CommandName == "AdditionalDetails")
            {
                try
                {
                    using (clsSalesOrder obj = new clsSalesOrder())
                    {

                        obj.OrderId = id;
                        DataTable dt = obj.SelectOrderAdditionalDetails();
                        grdDetail.DataSource = dt;
                        grdDetail.DataBind();
                        divAdditionalDetails.Visible = true;
                        updAdditionalDetails.Update();
                        divItemDetails.Visible = false;
                        updItemDetails.Update();
                    }
                }
                catch (Exception ex)
                {

                }
            }

            if (e.CommandName == "ItemDetails")
            {
                try
                {
                    int i = 1;

                    using (clsSalesOrder obj = new clsSalesOrder())
                    {
                        foreach (GridViewRow grv in grdvList.Rows)
                        {

                            Label lblOrderId = (Label)grv.FindControl("lblOrderId");
                            if (lblOrderId.Text == id.ToString())
                            {
                                grv.BackColor = System.Drawing.Color.LightYellow;
                                grv.ForeColor = System.Drawing.Color.Red;
                                i++;
                            }
                            else
                            {
                                if (i % 2 != 1)
                                {
                                    grv.BackColor = System.Drawing.Color.LightGray;
                                    grv.ForeColor = System.Drawing.Color.Black;
                                }
                                else
                                {
                                    grv.BackColor = System.Drawing.Color.White;
                                    grv.ForeColor = System.Drawing.Color.Black;
                                }
                                i++;
                            }
                        }

                        obj.OrderId = id;
                        DataTable dt = obj.SelectOrderItemDetails();
                        grdItemDetails.DataSource = dt;
                        grdItemDetails.DataBind();
                        divItemDetails.Visible = true;
                        updItemDetails.Update();
                        divAdditionalDetails.Visible = false;
                        updAdditionalDetails.Update();
                    }
                }
                catch (Exception ex)
                {
                    ucMessage1.ShowError(ex.Message);

                }
            }

            if (e.CommandName == "Cancel")
            {
                try
                {
                    using (clsSalesOrder obj = new clsSalesOrder())
                    {
                        ucMessage1.Visible = false;
                        divCancel.Visible = true;
                        txtCancelAllocation.TextBoxText = "";
                        HideShowDiv(divCancel.ID);

                    }
                }
                catch (Exception ex)
                {
                    ucMessage1.ShowError(ex.Message);
                }
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowWarning(ex.Message);
        }
    }

    protected void grdvList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {

    }

    private void HideShowDiv(string DivID)
    {

        if (DivID == divCancel.ID)
        {
            grdDetail.DataSource = null;
            grdDetail.DataBind();
        }
        updAdditionalDetails.Update();
        updCancelRemarks.Update();
        updItemDetails.Update();
    }

    protected void BtnCancelProcess_Click(object sender, EventArgs e)
    {
        using (clsSalesOrder obj = new clsSalesOrder())
        {
            ucMessage1.Visible = false;
            obj.OrderId = (Int32)ViewState["OrderId"];
            obj.Remarks = txtCancelAllocation.TextBoxText;
            obj.CreatedBy = PageBase.UserId;
            obj.OrderCancellation();
            if (obj.Error == "")
            {
                updpnlSaveData.Update();
                if (obj.Result == 2)
                {
                    ucMessage1.ShowError(obj.Error);
                    ucMessage1.Visible = true;
                    return;
                }

                if (obj.IsApplyCancel == 1)
                {
                    ucMessage1.ShowSuccess(SuccessMessages.OrderCancel);
                    ucMessage1.Visible = true;
                    txtCancelAllocation.TextBoxText = string.Empty;
                }
                if (obj.IsApplyCancel == 2)
                {
                    ucMessage1.ShowSuccess(SuccessMessages.OrderCancelledForApproval);
                    ucMessage1.Visible = true;
                    txtCancelAllocation.TextBoxText = string.Empty;
                }
                updCancelRemarks.Update();
                BindList(ucPagingControl1.CurrentPage);
            }
            else if (obj.Error != "")
            {
                ucMessage1.ShowError(obj.Error);
                ucMessage1.Visible = true;
                return;
            }
        }
    }

    protected void grdvList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblOrderId = (Label)e.Row.FindControl("lblOrderId");

                ImageButton btnPrint = (ImageButton)e.Row.FindControl("btnPrint");

                Label lblApplyCancel = (Label)e.Row.FindControl("lblApplyCancel");
                Label lblCancelSOVisibility = (Label)e.Row.FindControl("lblCancelSOVisibility");

                ImageButton btncancelInv = (ImageButton)e.Row.FindControl("imgCancel");

                btnPrint.Attributes.Add("OnClick", string.Format("return popup({0})", lblOrderId.Text));
                if ((e.Row.RowIndex % 2) == 1)
                {
                    e.Row.BackColor = System.Drawing.Color.LightGray;
                    e.Row.ForeColor = System.Drawing.Color.Black;
                }
                else
                {
                    e.Row.BackColor = System.Drawing.Color.White;
                    e.Row.ForeColor = System.Drawing.Color.Black;
                }


                ImageButton btnPrintInvoice = (ImageButton)e.Row.FindControl("btnPrintInvoice");
                if (btnPrintInvoice.CommandArgument == "")
                {
                    btnPrintInvoice.Visible = false;
                }
                else
                {
                    Label lblSoToId = (Label)e.Row.FindControl("lblSOTOID");

                    btnPrintInvoice.Visible = false;

                    if (ddlOrderType.SelectedValue.Trim() == "2")
                    {
                        btnPrintInvoice.Visible = true;
                        btnPrintInvoice.Attributes.Add("OnClick", "return PopupInvoice('" + lblSoToId.Text + "','" + btnPrintInvoice.CommandArgument + "')");
                    }

                }


                ImageButton btnPrintDispatch = (ImageButton)e.Row.FindControl("btnPrintDispatch");
                if (btnPrintDispatch.CommandArgument == "")
                {
                    btnPrintDispatch.Visible = false;
                }
                else
                {

                    btnPrintDispatch.Visible = false;
                    if (ddlOrderType.SelectedValue.Trim() == "2")
                    {
                        btnPrintDispatch.Visible = true;

                        if (strPrintPatch == string.Empty)
                        {
                            DataTable dtWhichDispatch = PageBase.GetEnumByTableName("AppConfig", "DispatchPrint");
                            DataRow[] drWhichprint = dtWhichDispatch.Select("Active=1");
                            strPrintPatch = drWhichprint[0]["Description"].ToString();
                        }


                        string _strPrint = string.Format("return PopupViewDeliveryChallanPrint({0},{1})", "'" + Server.UrlEncode(Cryptography.Crypto.Encrypt(btnPrintDispatch.CommandArgument, KeyStr)) + "'", "'" + VirtualPath + strPrintPatch + "'");


                        btnPrintDispatch.Attributes.Add("OnClick", _strPrint);
                    }
                }
                ImageButton btnPrintGrn = (ImageButton)e.Row.FindControl("btnGrnPrint");
                if (btnPrintGrn.CommandArgument == "")
                {
                    btnPrintGrn.Visible = false;
                }
                else
                {

                    btnPrintGrn.Visible = true;
                    string _strPrint = string.Format("return PopupGRNPrint({0},{1})", "'" + Server.UrlEncode(Cryptography.Crypto.Encrypt(btnPrintGrn.CommandArgument.ToString(), KeyStr)) + "'", "'" + VirtualPath + "'");
                    btnPrintGrn.Attributes.Add("OnClick", _strPrint);

                }

                if (lblApplyCancel.Text == "1" || lblApplyCancel.Text == "2")
                {
                    if (lblCancelSOVisibility.Text == "1")
                        btncancelInv.Visible = true;
                    else
                        btncancelInv.Visible = false;
                }
                else
                {
                    btncancelInv.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowWarning(ex.Message);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (cmbOrderFrom.IsParentWithSingleItem)
                {
                    LoadChild();
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowWarning(ex.Message);
        }
    }



    protected void ucOrderFrom_SelectedEntity(DataTable dtList)
    {
        try
        {
            ucMessage1.Visible = false;
            ucMessage2.Visible = false;
            cmbOrderTo1.Clear();
            cmbOrderTo1.BusinessEventKeyword = Enum.GetName(typeof(ZedAxis.ZedEBS.Enums.BusinessEventsKeyword), ZedAxis.ZedEBS.Enums.BusinessEventsKeyword.SOGENERATE).ToString();
            cmbOrderTo1.EntityTypeDescription = Convert.ToInt32(ZedAxis.ZedEBS.Enums.EntityTypeDescription.IncludingEntityType);
            cmbOrderTo1.SelectionMode = ZedAxis.ZedEBS.Enums.EnumSelectionMode.CheckColumn;
            cmbOrderTo1.Keyword = ZedAxis.ZedEBS.Enums.EntityTypeKeyword.SALECHANNEL;
            cmbOrderTo1.IsParent = ucEntityList_ver2.Parent.False;
            cmbOrderTo1.IsRequired = true;

            if (BaseEntityTypeID == 1)
                cmbOrderTo1.LoggedInEntityID = PageBase.SalesChanelID;
            else
                cmbOrderTo1.LoggedInEntityID = 0;
            cmbOrderTo1.Type = (ddlOrderType.SelectedValue.Trim() == "1") ? ucEntityList_ver2.EntityType.PRIMARY : ucEntityList_ver2.EntityType.SECONDARY;/*#CC07:added*/
            cmbOrderTo1.CheckedEntityList = cmbOrderFrom.GetSelectedValues;
            cmbOrderTo1.BindChild();

        }
        catch (Exception ex)
        {
            ucMessage1.ShowWarning(ex.Message);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.Visible = false;
            ucMessage2.Visible = false;
            if (!ValidateSearch())
            {
                return;
            }
            if (!validDateFormat())
            {
                return;
            }

            ddlOrderType.Enabled = false;
            BindList(1);
        }
        catch (Exception ex)
        {
            ucMessage1.ShowWarning(ex.Message);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ddlOrderType.Enabled = true;
            ddlOrderType.SelectedValue = "0";
            ucMessage1.Visible = false;
            ucMessage2.Visible = false;
            BlankSearch();
        }
        catch (Exception ex)
        {
            ucMessage1.ShowWarning(ex.Message);
        }
    }
    #endregion

    #region Methods
    private void LoadChild()
    {
        cmbOrderTo1.Clear();
        cmbOrderTo1.BusinessEventKeyword = Enum.GetName(typeof(ZedAxis.ZedEBS.Enums.BusinessEventsKeyword), ZedAxis.ZedEBS.Enums.BusinessEventsKeyword.SOGENERATE).ToString();
        cmbOrderTo1.EntityTypeDescription = Convert.ToInt32(ZedAxis.ZedEBS.Enums.EntityTypeDescription.IncludingEntityType);
        cmbOrderTo1.SelectionMode = ZedAxis.ZedEBS.Enums.EnumSelectionMode.Single;
        cmbOrderTo1.Keyword = ZedAxis.ZedEBS.Enums.EntityTypeKeyword.SALECHANNEL;
        cmbOrderTo1.IsParent = ucEntityList_ver2.Parent.False;

        cmbOrderTo1.Type = (ddlOrderType.SelectedValue.Trim() == "1") ? ucEntityList_ver2.EntityType.PRIMARY : ucEntityList_ver2.EntityType.SECONDARY;


        cmbOrderTo1.SelectionMode = ZedAxis.ZedEBS.Enums.EnumSelectionMode.CheckColumn;

        cmbOrderTo1.CallingMode = 2;
        cmbOrderTo1.CheckedEntityList = cmbOrderFrom.GetSelectedValues;


        cmbOrderTo1.BindChild();
    }

    public bool ValidateSearch()
    {

        if (string.IsNullOrEmpty(txtPoNumber.Text.Trim()) && cmbOrderFrom.GetSelectedValues.Rows.Count == 0)
        {
            ucMessage1.ShowWarning(WarningMessages.FillRequiredInformation);
            return false;
        }
        if (ddlOrderType.SelectedValue.Trim() == "0")
        {
            ucMessage1.ShowWarning(WarningMessages.FillRequiredInformation);
            return false;
        }


        if (txtPoNumber.Text == "" && ucToDate.Date == "" && ucFromDate.Date == ""
            && cmbAllocStatus.SelectedIndex == 0 && ddlCreditCheck.SelectedIndex == 0)
        {
            ucMessage1.ShowWarning(WarningMessages.EnterSearchCriteria);
            return false;
        }
        return true;
    }

    public bool validDateFormat()
    {
        if (ucFromDate.Date != "" || ucToDate.Date != "")
        {
            if (ucToDate.Date == "" || ucFromDate.Date == "")
            {
                ucMessage1.ShowError(ErrorMessages.InValidDateRange);
                return false;
            }
            if (Convert.ToDateTime(ucFromDate.Date) > Convert.ToDateTime(ucToDate.Date))
            {
                ucMessage1.ShowError(ErrorMessages.InValidDateRange);
                return false;
            }
        }
        return true;
    }

    public void BlankSearch()
    {

        divCancel.Visible = false;
        txtCancelAllocation.TextBoxText = "";
        txtPoNumber.Text = "";
        ucFromDate.Date = "";
        ucToDate.Date = "";
        ucPagingControl1.SetCurrentPage = 1;
        cmbAllocStatus.SelectedIndex = 0;
        ddlCreditCheck.SelectedIndex = 0; /*#CC04:Added*/
        cmbOrderTo1.Select_DeSelect_All(true);
        cmbOrderFrom.Select_DeSelect_All(true);
        div2.Visible = false;
        updSearch.Update();
        updpnlSaveData.Update();
        divAdditionalDetails.Visible = false;
        updAdditionalDetails.Update();
        divItemDetails.Visible = false;
        updItemDetails.Update();
        updCancelRemarks.Update();
    }


    void BindList(int index)
    {

        divAdditionalDetails.Visible = false;
        divItemDetails.Visible = false;
        updAdditionalDetails.Update();
        updItemDetails.Update();
        index = index == 0 ? 1 : index;
        using (clsSalesOrder obj = new clsSalesOrder())
        {

            obj.OrderType = Convert.ToInt16(ddlOrderType.SelectedValue);
            if (cmbOrderFrom.GetAllValues.Rows.Count > 0)
                obj.FromEntities = cmbOrderFrom.GetSelectedValues;
            else
                obj.FromEntities = cmbOrderFrom.GetAllValues;

            if (cmbOrderTo1.GetAllValues.Rows.Count > 0)
                if (cmbOrderTo1.GetSelectedValues.Rows.Count > 0)
                {
                    obj.ToEntities = cmbOrderTo1.GetSelectedValues;
                }
                else
                {
                    obj.ToEntities = cmbOrderTo1.GetAllValues;
                }
            else
                obj.ToEntities = cmbOrderTo1.GetAllValues;

            obj.SoFromDate = ucFromDate.GetDate;
            obj.SoToDate = ucToDate.GetDate;
            obj.OrderNumber = txtPoNumber.Text;
            obj.OrderAllocationStatus = Convert.ToInt16(cmbAllocStatus.SelectedValue);

            obj.PageIndex = index;
            obj.PageSize = Convert.ToInt32(PageSize);
            ucPagingControl1.CurrentPage = index;
            obj.SoCreaditCheckStatus = Convert.ToInt16(ddlCreditCheck.SelectedValue);

            DataTable dt = obj.SelectOrderForView_Ver2();
            grdvList.Visible = true;
            grdvList.DataSource = dt;
            grdvList.DataBind();
            if (dt == null || dt.Rows.Count == 0)
            {
                ucPagingControl1.Visible = false;
            }
            else
            {
                ucPagingControl1.Visible = true;
                ucPagingControl1.PageSize = Convert.ToInt32(PageBase.PageSize);
                ucPagingControl1.TotalRecords = obj.TotalRecords;
                ucPagingControl1.FillPageInfo();
            }
            div2.Visible = true;
            updSearch.Update();
        }

    }


    private void BindOrderTypes()
    {
        DataTable dtOrderTypeds = PageBase.GetEnumByTableName("XML_Enum", "OrderTypes");
        ddlOrderType.DataSource = dtOrderTypeds;
        ddlOrderType.DataValueField = "ID";
        ddlOrderType.DataTextField = "Description";
        ddlOrderType.DataBind();

        if (dtOrderTypeds.Rows.Count > 1)
        {
            ddlOrderType.Items.Insert(0, new ListItem("Select", "0"));
        }
    }
    private void CreditCheckText()
    {
        DataTable dtCreditcheck = PageBase.GetEnumByTableName("XML_Enum", "CreditCheck");
        ddlCreditCheck.DataSource = dtCreditcheck;
        ddlCreditCheck.DataTextField = "Description";
        ddlCreditCheck.DataValueField = "ID";
        ddlCreditCheck.DataBind();
        if (dtCreditcheck.Rows.Count > 1)
        {
            ddlCreditCheck.Items.Insert(0, new ListItem("Select", "0"));
        }

    }
    #endregion
    protected void Exporttoexcel_Click(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.Visible = false;

            using (clsSalesOrder obj = new clsSalesOrder())
            {

                obj.OrderType = Convert.ToInt16(ddlOrderType.SelectedValue);
                if (cmbOrderFrom.GetAllValues.Rows.Count > 0)
                    obj.FromEntities = cmbOrderFrom.GetSelectedValues;
                else
                    obj.FromEntities = cmbOrderFrom.GetAllValues;

                if (cmbOrderTo1.GetAllValues.Rows.Count > 0)
                    if (cmbOrderTo1.GetSelectedValues.Rows.Count > 0)
                    {
                        obj.ToEntities = cmbOrderTo1.GetSelectedValues;
                    }
                    else
                    {
                        obj.ToEntities = cmbOrderTo1.GetAllValues;
                    }
                else
                    obj.ToEntities = cmbOrderTo1.GetAllValues;

                obj.SoFromDate = ucFromDate.GetDate;
                obj.SoToDate = ucToDate.GetDate;
                obj.OrderNumber = txtPoNumber.Text;
                obj.OrderAllocationStatus = Convert.ToInt16(cmbAllocStatus.SelectedValue);

                obj.PageIndex = -1;
                obj.PageSize = Convert.ToInt32(PageSize);

                obj.SoCreaditCheckStatus = Convert.ToInt16(ddlCreditCheck.SelectedValue);

                DataTable dtexcel = obj.SelectOrderForView_Ver2();


                if (dtexcel.Rows.Count - 1 > 0)
                {
                    DataSet dsexcel = new DataSet();
                    dsexcel.Merge(dtexcel);
                    PageBase.ExportToExecl(dsexcel, "ViewSaleExcel");
                }
                else
                {
                    ucMessage1.ShowInfo("No View Sale Record Found");
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);

        }
    }
}
