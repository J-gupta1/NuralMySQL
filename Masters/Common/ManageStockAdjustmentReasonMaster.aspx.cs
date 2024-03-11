using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;

public partial class Masters_Common_ManageStockAdjustmentReasonMaster : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindgvAdjReson();
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
            using (CommonMaster objCMInsAdjResson = new CommonMaster())
            {
                try
                {
                    objCMInsAdjResson.AdjReason = txtAdjReason.Text.Trim();
                    objCMInsAdjResson.Active = Convert.ToInt32(chkActive.Checked);
                    objCMInsAdjResson.CreatedBy = PageBase.UserId;
                    objCMInsAdjResson.InsertStockAdjReason();
                    BindgvAdjReson();
                    if (objCMInsAdjResson.InsError != null)
                    {
                        ucMsg.Visible = true;
                        ucMsg.ShowError(objCMInsAdjResson.InsError);
                    }
                    else
                    {
                        ucMsg.Visible = true;
                        ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                        txtAdjReason.Text = "";
                    }

                }
                catch (Exception ex)
                {
                    //if (objCMInsAdjResson.ErrorDetailXML != null && objCMInsAdjResson.ErrorDetailXML != string.Empty)
                    //{
                    //    ucMsg.XmlErrorSource = objCMInsAdjResson.ErrorDetailXML;
                    //}
                    //else 
                    if (objCMInsAdjResson.Error != null && objCMInsAdjResson.Error != "" && objCMInsAdjResson.Error != "0")
                    {
                        ucMsg.ShowError(objCMInsAdjResson.Error);
                    }
                }
            }
        }
        else
        {
            using (CommonMaster objUpdateAdjResson = new CommonMaster())
            {
                try
                {
                    objUpdateAdjResson.AdjustmentReasonID = Convert.ToInt32(hdnAdjReasonID.Value);
                    objUpdateAdjResson.AdjReason = txtAdjReason.Text;
                    objUpdateAdjResson.Active = Convert.ToInt32(chkActive.Checked);
                    objUpdateAdjResson.UpdateStockAdjReason(0);
                    BindgvAdjReson();
                    if (objUpdateAdjResson.InsError != null)
                    {
                        ucMsg.Visible = true;
                        ucMsg.ShowError(objUpdateAdjResson.InsError);
                    }
                    else
                    {
                        ucMsg.Visible = true;
                        ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                        txtAdjReason.Text = "";
                        btnSubmit.Text = "Submit";
                        btnClear.Text = "Cancel";
                    }
                }
                catch (Exception ex)
                {
                    if (objUpdateAdjResson.Error != null && objUpdateAdjResson.Error != "" && objUpdateAdjResson.Error != "0")
                    {
                        ucMsg.ShowError(objUpdateAdjResson.Error);
                    }
                }
            }
            hdnAdjReasonID.Value = "";
            txtAdjReason.Text = "";
        }

    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtAdjReason.Text="";
        ucMsg.Visible = false;
        BindgvAdjReson();
        chkActive.Checked = true;
        btnSubmit.Text = "Submit";
        btnClear.Text = "Cancel";
        
    }
    private void BindgvAdjReson()
    {
        using (CommonMaster objCMGetAdjResson = new CommonMaster())
        {
            try
            {
                gvViewAdjReason.DataSource = objCMGetAdjResson.GetStockAdjReason(1);
                gvViewAdjReason.DataBind();
            }
            catch (Exception ex)
            {
                //if (objCMGetAdjResson.ErrorDetailXML != null && objCMGetAdjResson.ErrorDetailXML != string.Empty)
                //{
                //    ucMsg.XmlErrorSource = objCMGetAdjResson.ErrorDetailXML;
                //}
                //else 
                if (objCMGetAdjResson.Error != null && objCMGetAdjResson.Error != "" && objCMGetAdjResson.Error != "0")
                {
                    ucMsg.ShowError(objCMGetAdjResson.Error);
                }
            }
        }
    }
    protected void gvViewAdjReason_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvViewAdjReason.PageIndex = e.NewPageIndex;
        BindgvAdjReson();
    }
    protected void gvViewAdjReason_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ucMsg.Visible = false;
        if (e.CommandName.Equals("editcmd"))
        {
            using (CommonMaster objCMGetAdjResson = new CommonMaster())
            {
                try
                {
                    objCMGetAdjResson.AdjustmentReasonID = Convert.ToInt32(e.CommandArgument);
                    DataTable dt = objCMGetAdjResson.GetStockAdjReason(0);
                    txtAdjReason.Text = dt.Rows[0]["ReasonDesc"].ToString();
                    if (dt.Rows[0]["Active"].ToString().Equals("1")) { chkActive.Checked = true; }
                    if (dt.Rows[0]["Active"].ToString().Equals("0")) { chkActive.Checked = false; }
                    hdnAdjReasonID.Value = e.CommandArgument.ToString();
                    btnSubmit.Text = "Update";
                    btnClear.Text = "Clear";
                    BindgvAdjReson();
                    
                }
                catch (Exception ex)
                {
                    if (objCMGetAdjResson.Error != null && objCMGetAdjResson.Error != "" && objCMGetAdjResson.Error != "0")
                    {
                        ucMsg.Visible = true;
                        ucMsg.ShowError(objCMGetAdjResson.Error);
                    }
                }
            }
            //GridViewRow selectedRow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            //int intRowIndex = Convert.ToInt32(selectedRow.RowIndex);
            //GridViewRow row = gvViewAdjReason.Rows[intRowIndex];
            //Label lblAdjReason = (Label)row.FindControl("lblAdjReason");
            //txtAdjReason.Text = lblAdjReason.Text;
            //if (e.CommandArgument.Equals(1)) { chkActive.Checked = true; }
            //if (e.CommandArgument.Equals(0)) { chkActive.Checked = false; }
            //else { chkActive.Checked = true; }

        }
        if (e.CommandName.Equals("togglecmd"))
        {
            using (CommonMaster objUpdateAdjResson = new CommonMaster())
            {
                try
                {
                    objUpdateAdjResson.AdjustmentReasonID = Convert.ToInt32(e.CommandArgument);
                    objUpdateAdjResson.UpdateStockAdjReason(1);
                    BindgvAdjReson();
                    ucMsg.Visible = true;
                    ucMsg.ShowSuccess(Resources.Messages.StatusChanged);
                    txtAdjReason.Text = "";
                }
                catch (Exception ex)
                {
                    if (objUpdateAdjResson.Error != null && objUpdateAdjResson.Error != "" && objUpdateAdjResson.Error != "0")
                    {
                        ucMsg.Visible = true;
                        ucMsg.ShowError(objUpdateAdjResson.Error);
                    }
                }
            }

        }

    }
}
