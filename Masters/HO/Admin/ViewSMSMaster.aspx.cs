using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using System.Data;

public partial class Masters_HO_Admin_ViewSMSMaster : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindgvSMSMaster(0);
        }
    }
    private void BindgvSMSMaster(int condition)
    {
        using (SMSData objGetSMS = new SMSData())
        {
            try
            {

                objGetSMS.SMSDesc = txtSMSDesc.Text.Trim();
                DataTable dt = objGetSMS.GetSMSMaster(condition);
                gvSMSMaster.DataSource = dt;
                gvSMSMaster.DataBind();
                if (dt.Rows.Count == 0)
                {
                    ucMsg.ShowError(Resources.Messages.NoRecord);
                }

            }
            catch (Exception ex)
            {
                if (objGetSMS.Error != null && objGetSMS.Error != "" && objGetSMS.Error != "0")
                {
                    ucMsg.ShowError(objGetSMS.Error);
                }
            }
        }
    }
    protected void gvSMSMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ucMsg.Visible = false;
        gvSMSMaster.PageIndex = e.NewPageIndex;
        BindgvSMSMaster(0); 
    }
    protected void gvSMSMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ucMsg.Visible=false;
        if (e.CommandName.Equals("togglecmd"))
        {
             using (SMSData objcmdSMS = new SMSData())
            {
                try
                {
                    objcmdSMS.SMSID = Convert.ToInt32(e.CommandArgument);
                    objcmdSMS.UpdSMSMaster(1);
                    BindgvSMSMaster(0);
                    ucMsg.Visible=true;
                    ucMsg.ShowSuccess(Resources.Messages.StatusChanged);
                }
                catch (Exception ex)
                {
                    if (objcmdSMS.Error != null && objcmdSMS.Error != "" && objcmdSMS.Error != "0")
                    {
                        ucMsg.ShowError(objcmdSMS.Error);
                    }
                }
            }
        }
        if(e.CommandName.Equals("editcmd"))
        {
            Response.Redirect("ManagerSMSMaster.aspx?SMSID=" + e.CommandArgument.ToString());
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindgvSMSMaster(0);
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSMSDesc.Text = "";
        BindgvSMSMaster(0);
        ucMsg.Visible = false;
    }
}
