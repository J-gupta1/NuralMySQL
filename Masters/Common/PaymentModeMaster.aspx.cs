#region Copyright(c) 2020 Zed-Axis Technologies All rights are reserved
/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2017 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Created By : Vijay Kumar Prajapati
* Created On: 15-June-2020
 * Description: This is a Payment Mode Master Interface.
* ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 
 ====================================================================================================
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DataAccess;
using BussinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Masters_Common_PaymentModeMaster : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            BindList(1);
        }
    }
    protected void btnSubmit_Click(object sender, System.EventArgs e)
    {
        using (clsEntityMappingTypeRelationMaster objSave = new clsEntityMappingTypeRelationMaster())
        {

            objSave.PaymentModeId = 0;
            objSave.PaymentModeName = txtPaymentModeName.Text.Trim();
            objSave.PaymentModeCode = txtPaymentModeCode.Text.Trim();
            objSave.UserID = UserId;
            objSave.CompanyId = PageBase.ClientId;
            objSave.Active = Convert.ToInt16(chkActive.Checked);
            objSave.Condition = 0;
            if (btnSubmit.Text == "Update")
            {
                objSave.PaymentModeId = Convert.ToInt32(ViewState["PaymentModeId"]);
                objSave.Condition = 1;
            }
            objSave.SavePaymentModeMaster();
            if (objSave.Out_Param == 0)
            {
                if (btnSubmit.Text == "Submit")
                    ucMsg.ShowSuccess(Resources.SuccessMessages.SaveSuccess);
                else
                    ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                BindList(Convert.ToInt32(ViewState["PageIndex"]));
                CancelSubmit();
            }
            else
            {
                ucMsg.ShowError(objSave.Error);
            }
            dvMsg.Style.Add("display", "block");
            updMessage.Update();
        }
    }
    protected void btnCancel_Click(object sender, System.EventArgs e)
    {
        CancelSubmit();
        ucMsg.Visible = false;
        updMessage.Update();
    }
    protected void btnSearch_Click(object sender, System.EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlStatusSearch.SelectedValue) == 255 && txtPaymentModeNameSearch.Text.Trim() == "" && txtPaymentModeCodeSearch.Text.Trim() == "")
            {
                ucMsg.ShowWarning("Please select at least one search parameter..!!");
                dvMsg.Style.Add("display", "block");
                updMessage.Update();
                return;
            }
            ucPagingControl1.SetCurrentPage = 1;
            BindList(1);
        }
        catch (Exception ex)
        {
            ucMsg.Visible = true;
            ucMsg.ShowError(ex.ToString());
        }
    }
    protected void btnShowAll_Click(object sender, System.EventArgs e)
    {
        try
        {
            ddlStatusSearch.SelectedValue = "255";
            txtPaymentModeCodeSearch.Text = "";
            txtPaymentModeNameSearch.Text = "";
            updSearch.Update();
            ucPagingControl1.SetCurrentPage = 1;
            BindList(1);
        }
        catch (Exception ex)
        {
            ucMsg.Visible = true;
            ucMsg.ShowError(ex.ToString());
        }
    }
    protected void Exporttoexcel_Click(object sender, System.EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            using (clsEntityMappingTypeRelationMaster objGet = new clsEntityMappingTypeRelationMaster())
            {
                objGet.PageSize = Convert.ToInt32(ViewState["TotalRecords"]);
                objGet.PageIndex = -1;
                objGet.PaymentModeName = txtPaymentModeNameSearch.Text.Trim();
                objGet.PaymentModeCode = txtPaymentModeCodeSearch.Text.Trim();
                objGet.Active = Convert.ToInt16(ddlStatusSearch.SelectedValue);
                objGet.CompanyId = PageBase.ClientId;
                dt = objGet.GetPaymentModeMasterData();
            }
            DataSet ds = new DataSet();
            ds.Merge(dt);
            string FilenameToexport = "PaymentModeMasterDetails";
            PageBase.ExportToExecl(ds, FilenameToexport);

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message);
            dvMsg.Style.Add("display", "block");

        }
    }
    protected void grdvList_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "cmdEdit")
            {
                ucMsg.Visible = false;
                using (clsEntityMappingTypeRelationMaster objGet = new clsEntityMappingTypeRelationMaster())
                {
                    objGet.PageSize = Convert.ToInt32(PageBase.PageSize);
                    objGet.PageIndex = -2;
                    objGet.CompanyId = PageBase.ClientId;
                    objGet.PaymentModeId = Convert.ToInt32(e.CommandArgument);
                    objGet.Active = 255;
                    ViewState["PaymentModeId"] = Convert.ToInt32(e.CommandArgument);
                    DataTable dt = objGet.GetPaymentModeMasterData();
                    txtPaymentModeName.Text = dt.Rows[0]["PaymentModeName"].ToString();
                    txtPaymentModeCode.Text = dt.Rows[0]["PaymentModeCode"].ToString();
                    chkActive.Checked = dt.Rows[0]["Status"].ToString() == "1" ? true : false;
                    btnSubmit.Text = "Update";
                    updSave.Update();
                    updpnlGrid.Update();
                }
            }
            if (e.CommandName == "Active")
            {
                using (clsEntityMappingTypeRelationMaster objActive = new clsEntityMappingTypeRelationMaster())
                {
                    objActive.UserID = UserId;
                    objActive.PaymentModeId = Convert.ToInt32(e.CommandArgument);
                    objActive.Condition = 2;
                    objActive.CompanyId = PageBase.ClientId;
                    objActive.SavePaymentModeMaster();
                    if (objActive.Out_Param == 0)
                    {
                        ucMsg.ShowSuccess(Resources.SuccessMessages.ActiveInActive);
                        BindList(Convert.ToInt32(ViewState["PageIndex"]));
                        CancelSubmit();
                    }
                    else
                    {
                        ucMsg.ShowError(objActive.Error);
                    }
                    dvMsg.Style.Add("display", "block");
                    updMessage.Update();
                    updpnlGrid.Update();
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message);
            dvMsg.Style.Add("display", "block");
            updMessage.Update();
        }
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        try
        {

            BindList(ucPagingControl1.CurrentPage);

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message);
            dvMsg.Style.Add("display", "block");
            updMessage.Update();
        }
    }
    private void BindList(int index)
    {
        try
        {
            index = index == 0 ? 1 : index;
            using (clsEntityMappingTypeRelationMaster objGet = new clsEntityMappingTypeRelationMaster())
            {
                objGet.PageSize = Convert.ToInt32(PageBase.PageSize);
                objGet.PageIndex = index;
                objGet.PaymentModeName = txtPaymentModeNameSearch.Text.Trim();
                objGet.PaymentModeCode = txtPaymentModeCodeSearch.Text.Trim();
                objGet.Active = Convert.ToInt16(ddlStatusSearch.SelectedValue);
                objGet.CompanyId = PageBase.ClientId;
                DataTable dt = objGet.GetPaymentModeMasterData();
                ViewState["TotalRecords"] = objGet.TotalRecords;
                dvMsg.Style.Add("display", "none");
                divgrd.Style.Add("display", "bolck");
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
                    ucPagingControl1.TotalRecords = objGet.TotalRecords;
                    ucPagingControl1.FillPageInfo();
                    ViewState["PageIndex"] = index;
                }
                updpnlGrid.Update();
                updMessage.Update();
            }
        }
        catch (Exception ex)
        {
            ucMsg.Visible = true;
            ucMsg.ShowError(ex.ToString());
        }
    }
    private void CancelSubmit()
    { 
        chkActive.Checked = true;
        txtPaymentModeCode.Text = "";
        btnSubmit.Text = "Submit";
        txtPaymentModeName.Text = "";
        updSave.Update();
    }
}