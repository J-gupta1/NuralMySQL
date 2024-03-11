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
using Cryptography;

public partial class Masters_HO_Admin_ViewEmailMaster : BussinessLogic.PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            BindgvViewEmailMaster(1);
        }
    }
    private void BindgvViewEmailMaster(int condition)
    {
        DataTable dt = new DataTable();
        using (clsMailer objmailer = new clsMailer())
        {
            try
            {
                objmailer.Emaildesc = txtEmailDese.Text;
                dt = objmailer.getViewEmailMaster(condition);
                gvViewEmailMaster.DataSource = dt;
                gvViewEmailMaster.DataBind();
                if (dt == null || dt.Rows.Count == 0)
                {
                    ucMsg.ShowError(Resources.Messages.NoRecord);
                }
                else
                {
                    ucMsg.Visible = false;
                }
            }
            catch (Exception ex)
            {
                if (objmailer.ErrorDetailXML != null && objmailer.ErrorDetailXML != string.Empty)
                {
                    ucMsg.XmlErrorSource = objmailer.ErrorDetailXML;
                }
                else if (objmailer.Error != null && objmailer.Error != "" && objmailer.Error != "0")
                {
                    ucMsg.ShowError(objmailer.Error);
                }
            }
        }

    }
    protected void gvViewEmailMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridViewRow GVR = e.Row;
            LinkButton lnkbtnView = default(LinkButton);
            lnkbtnView = (LinkButton)GVR.FindControl("lnkbtnEmailBody");
            //lnkbtnView.Attributes.Add("onclick", "return doWin('" + lnkbtnView.CommandArgument.ToString() + "')");
            lnkbtnView.Attributes.Add("onclick", "return ViewEmailContent('" + lnkbtnView.CommandArgument.ToString() + "')");
        }

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindgvViewEmailMaster(0);
        hdnPagingCommand.Value = "SearchClick";
    }
    protected void btnViewAll_Click(object sender, EventArgs e)
    {
        BindgvViewEmailMaster(1);
        hdnPagingCommand.Value = "ViewAllClick";
        txtEmailDese.Text = "";
    }
    protected void gvViewEmailMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvViewEmailMaster.PageIndex = e.NewPageIndex;
        if (hdnPagingCommand.Value.Equals("SearchClick"))
        { 
            BindgvViewEmailMaster(0);
        }
        else if (hdnPagingCommand.Value.Equals("ViewAllClick"))
        {
            BindgvViewEmailMaster(1);
        }
        else
        {
            BindgvViewEmailMaster(1);
        }
    }
    protected void gvViewEmailMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Status")
        {
            GridViewRow selectedRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
            int intRowIndex = Convert.ToInt32(selectedRow.RowIndex);
            GridViewRow row = gvViewEmailMaster.Rows[intRowIndex];
            Label lblEmailKeyword = (Label)row.FindControl("lblEmailKeyword");
            string EmailKye = lblEmailKeyword.Text;
            using (clsMailer objmailer = new clsMailer())
            {
                try
                {
                    objmailer.Emailkeyword = EmailKye;
                    objmailer.UpdateManageEmailMaster(1);
                    if (hdnPagingCommand.Value.Equals("SearchClick"))
                    {
                        BindgvViewEmailMaster(0);
                        ucMsg.Visible = true;
                        ucMsg.ShowSuccess(Resources.Messages.StatusChanged);
                    }
                    else if (hdnPagingCommand.Value.Equals("ViewAllClick"))
                    {
                        BindgvViewEmailMaster(1);
                        ucMsg.Visible = true;
                        ucMsg.ShowSuccess(Resources.Messages.StatusChanged);
                    }
                    else
                    {
                        BindgvViewEmailMaster(1);
                        ucMsg.Visible = true;
                        ucMsg.ShowSuccess(Resources.Messages.StatusChanged);
                    }
                }
                catch (Exception ex)
                {
                    if (objmailer.ErrorDetailXML != null && objmailer.ErrorDetailXML != string.Empty)
                    {
                        ucMsg.XmlErrorSource = objmailer.ErrorDetailXML;
                    }
                    else if (objmailer.Error != null && objmailer.Error != "" && objmailer.Error != "0")
                    {
                        ucMsg.ShowError(objmailer.Error);
                    }
                }

            }
        }
    }
}
