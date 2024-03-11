using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using System.Data;

public partial class Masters_HO_Admin_ManagerSMSMaster : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["SMSID"] != "" && Request.QueryString["SMSID"] != null)
            {
                btnSubmit.Text = "Update";
                btnCancel.Text = "Go Back";
                using (SMSData objGetSMS = new SMSData())
                {
                    try
                    {
                        objGetSMS.SMSID = Convert.ToInt32(Request.QueryString["SMSID"].ToString());
                        DataTable dt = objGetSMS.GetSMSMaster(1);
                        txtSMSKeyword.Text = dt.Rows[0]["SMSOutboundTransKeyword"].ToString();
                        txtSMSDescription.Text = dt.Rows[0]["SMSOutboundTransDesc"].ToString();
                        txtSMSFrom.Text = dt.Rows[0]["SMSFrom"].ToString();
                        txtSMSContent.Text = dt.Rows[0]["SMSContent"].ToString();
                        txtSMSExpiry.Text = dt.Rows[0]["SMSExpiryHrs"].ToString();
                        if (dt.Rows[0]["Status"].ToString().Equals("1"))
                        { chkStatus.Checked = true; }
                        if (dt.Rows[0]["Status"].ToString().Equals("0"))
                        { chkStatus.Checked = false; }

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
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["SMSID"] != "" && Request.QueryString["SMSID"] != null)
        {
            UpdSMSMaster();
            Clear();
        }
        else 
        {
            InsSMSMaster();
            Clear();
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["SMSID"] != "" && Request.QueryString["SMSID"] != null)
        {
            Response.Redirect("ViewSMSMaster.aspx");
        }
        else
        {
            txtSMSKeyword.Text = "";
            txtSMSDescription.Text = "";
            txtSMSFrom.Text = "";
            txtSMSContent.Text = "";
            txtSMSExpiry.Text = "";
            if (chkStatus.Checked == false) { chkStatus.Checked = true; }
            ucMsg.Visible = false;
        }
    }
    private void InsSMSMaster()
    {
        using (SMSData objInsertSMS = new SMSData())
        {
            try
            {
                objInsertSMS.SMSKeyword = txtSMSKeyword.Text;
                objInsertSMS.SMSDesc = txtSMSDescription.Text;
                objInsertSMS.SMSFrom = txtSMSFrom.Text;
                objInsertSMS.SMSContent = txtSMSContent.Text;
                objInsertSMS.SMSExpiry = Convert.ToInt32(txtSMSExpiry.Text);
                objInsertSMS.Status = Convert.ToInt32(chkStatus.Checked);
                objInsertSMS.InsSMSMaster();
                if (objInsertSMS.InsError != null)
                {
                    ucMsg.ShowError(objInsertSMS.InsError);
                }
                else
                {
                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                }

            }
            catch (Exception ex)
            {
                if (objInsertSMS.Error != null && objInsertSMS.Error != "" && objInsertSMS.Error != "0")
                {
                    ucMsg.ShowError(objInsertSMS.Error);
                }
            }
        }
    }
    private void Clear()
    {
        txtSMSKeyword.Text = "";
        txtSMSDescription.Text = "";
        txtSMSFrom.Text = "";
        txtSMSContent.Text = "";
        txtSMSExpiry.Text = "";
    }
    private void UpdSMSMaster()
    {
        using (SMSData objUpdSMS = new SMSData())
        {
            try
            {
                objUpdSMS.SMSKeyword = txtSMSKeyword.Text;
                objUpdSMS.SMSDesc = txtSMSDescription.Text;
                objUpdSMS.SMSFrom = txtSMSFrom.Text;
                objUpdSMS.SMSContent = txtSMSContent.Text;
                objUpdSMS.SMSExpiry = Convert.ToInt32(txtSMSExpiry.Text);
                objUpdSMS.Status = Convert.ToInt32(chkStatus.Checked);
                objUpdSMS.SMSID = Convert.ToInt32(Request.QueryString["SMSID"].ToString());
                objUpdSMS.UpdSMSMaster(0);
                if (objUpdSMS.InsError != null)
                {
                    ucMsg.ShowError(objUpdSMS.InsError);
                }
                else
                {
                    ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                    //btnSubmit.Text = "Submit";
                    //btnCancel.Text = "Cancel";
                }

            }
            catch (Exception ex)
            {
                if (objUpdSMS.Error != null && objUpdSMS.Error != "" && objUpdSMS.Error != "0")
                {
                    ucMsg.ShowError(objUpdSMS.Error);
                }
            }
        }
    }
}
