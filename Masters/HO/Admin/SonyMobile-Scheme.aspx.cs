using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using System.IO;
using System.Data;
using BussinessLogic;

public partial class Masters_HO_Admin_SonyMobile_Scheme : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMsg.Visible = false;
            if (!IsPostBack)
            {
                btnSubmit.Text = "Submit";
                hdnCase.Value = "0";
                gvBind();
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }
    # region UserDefine Function
    private void CreateOfflineScheme()
    {
        if (Convert.ToDateTime(ucdpStartDate.Date) > Convert.ToDateTime(ucdpEndDate.Date))
        {
            ucMsg.ShowError("Start Date is must be less than End Date.");
            return;
        }
        using (SchemeData objOfflineScheme = new SchemeData())
        {
            objOfflineScheme.SchemeName = txtSchemeName.Text;
            objOfflineScheme.SchemeDecription = txtDescripption.Text;
            if (fuDocument.HasFile)
            {
                if (fuDocument.FileBytes.Length > 10485760)
                {
                    ucMsg.ShowError("Uploaded file size is exceeding the limit.");
                    return;
                }
                if (Path.GetExtension(fuDocument.PostedFile.FileName).Equals(".pdf"))
                {
                    string filename = Path.GetFileName(fuDocument.FileName);
                    fuDocument.SaveAs(Server.MapPath("~/Excel/Upload/OffLineScheme/") + filename);
                    objOfflineScheme.SchemeDocumentFileName = "~/Excel/Upload/OffLineScheme/" + filename;
                }
                else
                {
                    ucMsg.Visible = true;
                    ucMsg.ShowInfo("Incorrect File Format");
                    return;
                }
            }
            else 
            {
                //objOfflineScheme.SchemeDocumentFileName = "File Not Found";
                 ucMsg.Visible = true;
                 ucMsg.ShowWarning("File not found, Please browes a pdf document");
                 return;
            }
            objOfflineScheme.SchemeStartDate = Convert.ToDateTime(ucdpStartDate.Date);
            objOfflineScheme.SchemeEndDate = Convert.ToDateTime(ucdpEndDate.Date);
            objOfflineScheme.OfflineStatus = Convert.ToInt16(chkStatus.Checked);
            objOfflineScheme.InsUpdOfflineScheme(0);
            if (objOfflineScheme.ErrorMessage != "")
            {
                ucMsg.Visible = true;
                ucMsg.ShowError(objOfflineScheme.ErrorMessage);
                Cancel();
            }
            else
            {
                ucMsg.Visible = true;
                ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                hdnCase.Value = "0";
                gvBind();
                Cancel();
            }
        };
    }
    private void UpdateOfflineScheme()
    {
        using (SchemeData objOfflineScheme = new SchemeData())
        {
            objOfflineScheme.SchemeID = Convert.ToInt16(hdnSchemeID.Value);
            objOfflineScheme.SchemeName = txtSchemeName.Text;
            objOfflineScheme.SchemeDecription = txtDescripption.Text;
            if (fuDocument.HasFile)
            {
                if (Path.GetExtension(fuDocument.PostedFile.FileName).Equals(".pdf"))
                {
                    string filename = Path.GetFileName(fuDocument.FileName);
                    fuDocument.SaveAs(Server.MapPath("~/Excel/Upload/OffLineScheme/") + filename);
                    objOfflineScheme.SchemeDocumentFileName = "~/Excel/Upload/OffLineScheme/" + filename;
                }
                else
                {
                    ucMsg.Visible = true;
                    ucMsg.ShowInfo("Incorrect File Format");
                    return;
                }
            }
            else
            {
                objOfflineScheme.SchemeDocumentFileName = hdnFileName.Value;
            }
            objOfflineScheme.SchemeStartDate = Convert.ToDateTime(ucdpStartDate.Date);
            objOfflineScheme.SchemeEndDate = Convert.ToDateTime(ucdpEndDate.Date);
            objOfflineScheme.OfflineStatus = Convert.ToInt16(chkStatus.Checked);
            objOfflineScheme.InsUpdOfflineScheme(2);
            if (objOfflineScheme.ErrorMessage != "")
            {
                ucMsg.Visible = true;
                ucMsg.ShowError(objOfflineScheme.ErrorMessage);
                Cancel();
            }
            else
            {
                ucMsg.Visible = true;
                ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                gvBind();
                Cancel();
            }
        };
    }
    private void Cancel()
    {
        txtSchemeName.Text = "";
        txtDescripption.Text = "";
        ucdpStartDate.Date = "";
        ucdpEndDate.Date = "";
        chkStatus.Checked = true;
        txtsrcSchemeCode.Text = "";
        txtsrcSchememName.Text = "";
        btnSubmit.Text = "Submit";
    }
    private void gvBind()
    {
        try
        {
            DataTable dt = new DataTable();
            using (SchemeData objOfflineScheme = new SchemeData())
            {
                if (hdnCase.Value == "0")
                {
                    dt = objOfflineScheme.GetOfflineScheme(0);
                }
                else if (hdnCase.Value == "1")
                {
                    if (txtsrcSchememName.Text != "")
                        objOfflineScheme.SchemeName = txtsrcSchememName.Text;
                    if (txtsrcSchemeCode.Text != "")
                        objOfflineScheme.OfflineSchemeCode = txtsrcSchemeCode.Text;
                    dt = objOfflineScheme.GetOfflineScheme(1);
                }
                gvViewOfflineScheme.DataSource = dt;
                gvViewOfflineScheme.DataBind();
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    # endregion
    # region Control Function
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if(btnSubmit.Text=="Submit")
            CreateOfflineScheme();
        if (btnSubmit.Text == "Update")
            UpdateOfflineScheme();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Cancel();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtsrcSchememName.Text != "" || txtsrcSchemeCode.Text != "")
            hdnCase.Value = "1";
        else
            hdnCase.Value = "0";
        gvBind();
    }
    protected void btnCancelSearch_Click(object sender, EventArgs e)
    {
        hdnCase.Value = "0";
        gvBind();
        Cancel();
    }
    protected void gvViewOfflineScheme_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvViewOfflineScheme.PageIndex = e.NewPageIndex;
        gvBind();
    }
    protected void gvViewOfflineScheme_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Status"))
        {
            using (SchemeData objOfflineScheme = new SchemeData())
            {
                objOfflineScheme.SchemeID = Convert.ToInt16(e.CommandArgument);
                objOfflineScheme.InsUpdOfflineScheme(1);
                gvBind();
                //Cancel();
                ucMsg.Visible = true;
                ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
            };
        }
        if (e.CommandName.Equals("editcmd"))
        {
            DataTable dtresutt = new DataTable();
            using (SchemeData objOfflineScheme = new SchemeData())
            {
                objOfflineScheme.SchemeID = Convert.ToInt16(e.CommandArgument);
                dtresutt=objOfflineScheme.GetOfflineScheme(2);
                txtSchemeName.Text = dtresutt.Rows[0]["SchemeName"].ToString();
                txtDescripption.Text = dtresutt.Rows[0]["SchemeDescription"].ToString();
                ucdpStartDate.Date = dtresutt.Rows[0]["SchemeStartDate"].ToString();
                ucdpEndDate.Date = dtresutt.Rows[0]["SchemeEndDate"].ToString();
                chkStatus.Checked = Convert.ToBoolean(dtresutt.Rows[0]["Status"]);
                hdnFileName.Value = dtresutt.Rows[0]["SchemeDocumentFileName"].ToString();
                hdnSchemeID.Value = Convert.ToString(e.CommandArgument);
                btnSubmit.Text = "Update";
            };
        }
    }
    # endregion
}
