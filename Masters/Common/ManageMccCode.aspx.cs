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
using System.Globalization;
using System.Resources;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;

public partial class Masters_Common_ManageMccCode : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            ucMsg.Visible = false;
            if (!IsPostBack)
            {
                hdnMCCCodeid.Value = "0";
                FillGrid(1);

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }
            if (txtMCCCode.Text.Trim().Length == 0)
            {
                ucMsg.ShowInfo(Resources.Messages.MandatoryField);
                return;
            }
            Int32 result = 0;
            using (SalesChannelData objMCC = new SalesChannelData())
            {
                if (hdnMCCCodeid.Value == "0")
                    objMCC.MCCMasterId = 0;
                else
                    objMCC.MCCMasterId = Convert.ToInt32(hdnMCCCodeid.Value);
                objMCC.MCCCode = txtMCCCode.Text.Trim();
                objMCC.OperatorName = txtOperatorName.Text.Trim();
                objMCC.CircleName = txtCircleName.Text.Trim();
                result = objMCC.InsertUpdateMCCCodeInfo();
                if (objMCC.Error != null && objMCC.Error != string.Empty)
                {
                    ucMsg.ShowInfo(objMCC.Error);
                    return;
                }
            };
            if (result > 0)
            {
                if (hdnMCCCodeid.Value == "0")
                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                else
                    ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                ClearForm();
                FillGrid(1);
                updAddMCC.Update();
                return;
            }
            else
            {
                ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                return;
            }

        }

        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }

    }
    protected void btnExprtToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            FillGrid(0);
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
    protected void btnSearchMCC_Click(object sender, EventArgs e)
    {
        FillGrid(1);


    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        ClearForm();
        FillGrid(1);

    }
    private void FillGrid(Int16 CallingFrom)
    {
        DataTable dtMCCCode;
        using (SalesChannelData objMCC = new SalesChannelData())
        {
            objMCC.MCCCode = txtMCCCodeSearch.Text.Trim();
            objMCC.CircleName = txtCircleNameSearch.Text.Trim();
            objMCC.OperatorName = txtOperatorNameSearch.Text.Trim();
            dtMCCCode = objMCC.GetMCCCodeInfo();
            gvMCCCode.Visible = true;
        };
        if (CallingFrom == 1)
        {
            if (dtMCCCode != null && dtMCCCode.Rows.Count > 0)
            {
                ViewState["Dtexport"] = dtMCCCode;
                gvMCCCode.DataSource = dtMCCCode;
                btnExprtToExcel.Visible = true;

            }
            else
            {
                gvMCCCode.DataSource = null;
                btnExprtToExcel.Visible = false;

            }
        }
        else
        {
            DataSet ds = new DataSet();
            string[] DsCol = new string[] { "MCCMNCCode", "OperatorName", "CircleName" };
            dtMCCCode = dtMCCCode.DefaultView.ToTable(true, DsCol);
            dtMCCCode.Columns["MCCMNCCode"].ColumnName = "MCCMNC Code";
            dtMCCCode.Columns["OperatorName"].ColumnName = "Operator Name";
            dtMCCCode.Columns["CircleName"].ColumnName = "Circle Name";
            ds.Merge(dtMCCCode);
            String FilePath = Server.MapPath("../../");
            string FilenameToexport = "MCC Code";
            PageBase.RootFilePath = FilePath;
            PageBase.ExportToExecl(ds, FilenameToexport);
        }
        gvMCCCode.DataBind();
        UpdSearch.Update();
        updAddMCC.Update();
        updgrid.Update();

    }
    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {

        ImageButton btnEdit = (ImageButton)sender;
        DataTable dtMCC;
        using (SalesChannelData objMCC = new SalesChannelData())
        {
            objMCC.MCCMasterId = Convert.ToInt32(btnEdit.CommandArgument);
            dtMCC = objMCC.GetMCCCodeInfo();
        };
        hdnMCCCodeid.Value = Convert.ToString(btnEdit.CommandArgument);
        if (dtMCC != null && dtMCC.Rows.Count > 0)
        {

            txtMCCCode.Text = (dtMCC.Rows[0]["MCCMNCCode"].ToString());
            txtOperatorName.Text = (dtMCC.Rows[0]["OperatorName"].ToString());
            txtCircleName.Text = (dtMCC.Rows[0]["CircleName"].ToString());
            btnSubmit.Text = "Update";
            FillGrid(1);
            updgrid.Update();
            updAddMCC.Update();
        }

    }

    void ClearForm()
    {
        txtMCCCode.Text = "";
        txtOperatorName.Text = "";
        txtCircleName.Text = "";
        btnSubmit.Text = "Submit";
        hdnMCCCodeid.Value = "0";
        UpdSearch.Update();
    }

    protected void gvSalesMan_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvMCCCode.PageIndex = e.NewPageIndex;
            FillGrid(1);
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }

    }

}
