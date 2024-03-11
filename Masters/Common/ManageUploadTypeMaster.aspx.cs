using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;

public partial class Masters_Common_ManageUploadTypeMaster : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindgvUploadDocs();
        }
        if (hdnAdjReasonID.Value.Equals(""))
        {
            btnSubmit.Text = "Submit";
            btnClear.Text = "Cancel";
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (hdnAdjReasonID.Value.Equals(""))
        {
            using (clsUploadDocs obj = new clsUploadDocs())
            {
                try
                {
                    obj.UploadType = txtUploadType.Text.Trim();
                    obj.Active = Convert.ToInt32(chkActive.Checked);
                    obj.CreatedBy = PageBase.UserId;
                    obj.InsertUploadDocs();
                    BindgvUploadDocs();
                    if (obj.InsError != null)
                    {
                        ucMsg.Visible = true;
                        ucMsg.ShowError(obj.InsError);
                    }
                    else
                    {
                        ucMsg.Visible = true;
                        ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                        txtUploadType.Text = "";
                    }

                }
                catch (Exception ex)
                {
                    if (obj.Error != null && obj.Error != "" && obj.Error != "0")
                    {
                        ucMsg.ShowError(obj.Error);
                    }
                }
            }
        }
        else
        {
            using (clsUploadDocs obj = new clsUploadDocs())
            {
                try
                {
                    obj.UploadTypeID = Convert.ToInt32(hdnAdjReasonID.Value);
                    obj.UploadType = txtUploadType.Text;
                    obj.Active = Convert.ToInt32(chkActive.Checked);
                    obj.condition = 0;
                    obj.UpdateUploadDocs();
                    BindgvUploadDocs();
                    if (obj.InsError != null)
                    {
                        ucMsg.Visible = true;
                        ucMsg.ShowError(obj.InsError);
                    }
                    else
                    {   
                        ucMsg.Visible = true;
                        ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                        txtUploadType.Text = "";

                    }
                }
                catch (Exception ex)
                {
                    if (obj.Error != null && obj.Error != "" && obj.Error != "0")
                    {
                        ucMsg.ShowError(obj.Error);
                    }
                }
            }
            hdnAdjReasonID.Value = "";
            txtUploadType.Text = "";
        }

    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtUploadType.Text = "";
        ucMsg.Visible = false;
        BindgvUploadDocs();
        chkActive.Checked = true;
        btnSubmit.Text = "Submit";
        btnClear.Text = "Cancel";

    }
    private void BindgvUploadDocs()
    {
        using (clsUploadDocs obj = new clsUploadDocs())
        {
            try
            {
                obj.condition = 1;
                gvViewUploadType.DataSource = obj.GetUploadDocs();
                gvViewUploadType.DataBind();
            }
            catch (Exception ex)
            {

                if (obj.Error != null && obj.Error != "" && obj.Error != "0")
                {
                    ucMsg.ShowError(obj.Error);
                }
            }
        }
    }
    protected void gvViewUploadType_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvViewUploadType.PageIndex = e.NewPageIndex;
        BindgvUploadDocs();
    }
    protected void gvViewUploadType_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ucMsg.Visible = false;
        if (e.CommandName.Equals("editcmd"))
        {
            using (clsUploadDocs obj = new clsUploadDocs())
            {
                try
                {
                    obj.UploadTypeID = Convert.ToInt32(e.CommandArgument);
                    obj.condition = 0;
                    DataTable dt = obj.GetUploadDocs();
                    txtUploadType.Text = dt.Rows[0]["UploadDocReferenceType"].ToString();
                    if (dt.Rows[0]["Active"].ToString().Equals("1")) { chkActive.Checked = true; }
                    if (dt.Rows[0]["Active"].ToString().Equals("0")) { chkActive.Checked = false; }
                    hdnAdjReasonID.Value = e.CommandArgument.ToString();
                    btnSubmit.Text = "Update";
                    btnClear.Text = "Clear";
                    BindgvUploadDocs();

                }
                catch (Exception ex)
                {
                    if (obj.Error != null && obj.Error != "" && obj.Error != "0")
                    {
                        ucMsg.Visible = true;
                        ucMsg.ShowError(obj.Error);
                    }
                }
            }
        }
        if (e.CommandName.Equals("togglecmd"))
        {
            using (clsUploadDocs obj = new clsUploadDocs())
            {
                try
                {
                    obj.UploadTypeID = Convert.ToInt32(e.CommandArgument);
                    obj.condition = 1;
                    obj.UpdateUploadDocs();
                    BindgvUploadDocs();
                    ucMsg.Visible = true;
                    ucMsg.ShowSuccess(Resources.Messages.StatusChanged);
                    txtUploadType.Text = "";
                }
                catch (Exception ex)
                {
                    if (obj.Error != null && obj.Error != "" && obj.Error != "0")
                    {
                        ucMsg.Visible = true;
                        ucMsg.ShowError(obj.Error);
                    }
                }
            }

        }

    }
}
