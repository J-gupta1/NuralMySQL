using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using System.Data;

public partial class Masters_HO_Admin_ManagerEmailMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string filename = Request.QueryString["FileName"].ToString();
        if (Request.QueryString["EmailKeyword"] != "" && Request.QueryString["EmailKeyword"] != null)
        {
            string emailkey = Request.QueryString["EmailKeyword"].ToString();
            btnSubmit.Text = "Update";
            btnCancel.Text = "Go Back";
            if (!IsPostBack)
            {
                BindToUpdate();
            }

        }
    }
    private void BindToUpdate()
    {
        DataTable dt = new DataTable();
        using (clsMailer objmailer = new clsMailer())
        {
            try
            {
                objmailer.Emailkeyword = Request.QueryString["EmailKeyword"].ToString();
                dt = objmailer.getViewEmailMaster(2);
                txtEmailKeyword.Text = dt.Rows[0][0].ToString();
                txtEmailKeyword.Enabled = false;
                txtEmailDescription.Text = dt.Rows[0][1].ToString();
                txtEmailFrom.Text = dt.Rows[0][2].ToString();
                txtSubjectLine.Text = dt.Rows[0][3].ToString();
                txtHrs.Text=dt.Rows[0][6].ToString();
                FCKBody.Value = dt.Rows[0][4].ToString();
                if (dt.Rows[0][5].Equals(true))
                {
                    chkstatus.Checked = true;
                }
                else { chkstatus.Checked = false; }
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
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["EmailKeyword"] != "" && Request.QueryString["EmailKeyword"] != null)
        {
            string emailkey = Request.QueryString["EmailKeyword"].ToString();
            if (FCKBody.Value != "" && FCKBody.Value != null && FCKBody.Value != string.Empty)
            {
                using (clsMailer objmailer = new clsMailer())
                {
                    try
                    {
                        objmailer.Emailkeyword = Request.QueryString["EmailKeyword"].ToString();
                        objmailer.Emaildesc = txtEmailDescription.Text;
                        objmailer.Emailfrom = txtEmailFrom.Text;
                        objmailer.Subjectline = txtSubjectLine.Text;
                        objmailer.Bodycontent = FCKBody.Value.Replace(">\r\n<", "><");
                        objmailer.Status = Convert.ToInt32(chkstatus.Checked);
                        objmailer.EmailExpiryHrs = Convert.ToInt32(txtHrs.Text);
                        objmailer.UpdateManageEmailMaster(0);
                        //Response.Redirect("ViewEmailMaster.aspx");
                        ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                        txtEmailKeyword.Text = "";
                        txtEmailDescription.Text = "";
                        txtEmailFrom.Text = "";
                        txtSubjectLine.Text = "";
                        txtHrs.Text = "";
                        FCKBody.Value = "";

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
            else
            {
                ucMsg.ShowWarning("Write text in body");
            }
        }
        else
        if (FCKBody.Value != "" && FCKBody.Value != null && FCKBody.Value != string.Empty)
        {
            using (clsMailer objmailer = new clsMailer())
            {
                try
                {
                    objmailer.Emailkeyword = txtEmailKeyword.Text;
                    objmailer.Emaildesc = txtEmailDescription.Text;
                    objmailer.Emailfrom = txtEmailFrom.Text;
                    objmailer.Subjectline = txtSubjectLine.Text;
                    objmailer.Bodycontent = FCKBody.Value.Replace(">\r\n<", "><");
                    objmailer.Status = Convert.ToInt32(chkstatus.Checked);
                    objmailer.EmailExpiryHrs = Convert.ToInt32(txtHrs.Text);
                    objmailer.InsertManageEmailMaster();
                    if (objmailer.Emailkeyinfo != null)
                    {
                        ucMsg.ShowError(objmailer.Emailkeyinfo);
                    }
                    else
                    {
                        ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                        txtEmailKeyword.Text = "";
                        txtEmailDescription.Text = "";
                        txtEmailFrom.Text = "";
                        txtSubjectLine.Text = "";
                        txtHrs.Text = "";
                        FCKBody.Value = "";
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
        else
        {
            ucMsg.ShowWarning("Write text in body");
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtEmailKeyword.Text = "";
        txtEmailDescription.Text = "";
        txtEmailFrom.Text = "";
        txtSubjectLine.Text = "";
        FCKBody.Value = "";
        txtHrs.Text = "";
        ucMsg.Visible = false;
        if (Request.QueryString["EmailKeyword"] != "" && Request.QueryString["EmailKeyword"] != null)
        {
            Response.Redirect("ViewEmailMaster.aspx");
        }
    }
}
