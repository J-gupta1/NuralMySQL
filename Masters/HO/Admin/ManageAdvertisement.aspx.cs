using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using BussinessLogic;
using DataAccess;
using System.Data;

public partial class Masters_HO_Admin_ManageAdvertisement : PageBase
{
    int ThumbainlType = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                hdnCondition.Value = "0";
                gvbind();
                if (lblMode.Text == "Image Internet URL")
                {
                    fuImage.Visible = false;
                    txtInternetURL.Visible = true;
                    chkMode.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }
    # region User_Define Function
    private string Upload_Thumbainl()
    {
        string strresult = string.Empty;
        try
        {
            string[] imgextension = { ".bmp", ".dds", ".dng", ".gif", ".jpg", ".jpeg", ".psd", ".pspimage", ".tga", ".thm", ".tif", ".yuv" };
            if (fuImage.HasFile)
            {
                foreach (string ext in imgextension)
                {
                    if (Path.GetExtension(fuImage.PostedFile.FileName).Equals(ext))
                    {
                        string filename = Path.GetFileName(fuImage.FileName);
                        fuImage.SaveAs(Server.MapPath("~/Excel/Upload/Tumbnail/") + filename);
                        strresult = "~/Excel/Upload/Tumbnail/" + filename;
                        ThumbainlType = 1;
                    }
                }
            }
            else
            {
                if (txtInternetURL.Text == "")
                {
                    strresult = txtInternetURL.Text;
                }
                else
                {
                    strresult = txtInternetURL.Text;
                    ThumbainlType = 2;
                }
            }
        }
        catch (IOException exp)
        {
            throw exp;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return strresult;
    }
    public void clear()
    {
        fuImage.Visible = false;
        txtInternetURL.Visible = true;
        txtCaption.Text = "";
        txtInternetURL.Text = "";
        txtVideoLink.Text = "";
        chkMode.Checked = false;
        chkStatus.Checked = true;
        btnSubmit.Text = "Submit";
        ucMsg.Visible = false;
        txtsrcCaption.Text = "";
    }
    public void gvbind()
    {
        try 
        {
            using (Advertisement objadd = new Advertisement())
            {
                DataTable dt;
                if (hdnCondition.Value.Equals("0")&& txtsrcCaption.Text=="")
                {
                    dt = objadd.GetAdvertisement();
                    gvAdvertisement.DataSource = dt;
                }
                if(hdnCondition.Value.Equals("1") && txtsrcCaption.Text!="")
                {
                    objadd.srcCaption = txtsrcCaption.Text.Trim();
                    dt = objadd.GetAdvertisement();
                    gvAdvertisement.DataSource = dt;
                }
                gvAdvertisement.DataBind();
            };
        }
        catch (Exception ex)
        { throw ex; }
    }
    # endregion
    # region Control Function
    protected void chkMode_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkMode.Checked)
            {
                lblMode.Text = "Upload Image";
                fuImage.Visible = true;
                txtInternetURL.Visible = false;
            }
            else
            {
                lblMode.Text = "Image Internet URL";
                fuImage.Visible = false;
                txtInternetURL.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string ThumbainlPath = Upload_Thumbainl();
            using (Advertisement objadd = new Advertisement())
            {
                objadd.Caption = txtCaption.Text;
                objadd.ThumbainlPath = ThumbainlPath;
                objadd.ThumbainlType = ThumbainlType;
                objadd.VideoLink = txtVideoLink.Text;
                objadd.Status = Convert.ToInt16(chkStatus.Checked);
                if (btnSubmit.Text == "Submit")
                {
                    objadd.InsUpdAdvertisement(0);
                    if (objadd.ErrorMsg == "")
                        ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    else
                        ucMsg.ShowError(objadd.ErrorMsg);
                }
                else
                {
                    objadd.AdvertisementID = Convert.ToInt32(hdnAddID.Value);
                    objadd.InsUpdAdvertisement(1);
                    if (objadd.ErrorMsg == "")
                        ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                    else
                        ucMsg.ShowError(objadd.ErrorMsg);
                }
                clear();
                gvbind();
            };
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clear();
    }
    protected void gvAdvertisement_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvAdvertisement.PageIndex = e.NewPageIndex;
            gvbind();
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }
    protected void gvAdvertisement_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            ucMsg.Visible = false;
            if (e.CommandName.Equals("Status"))
            {
                using (Advertisement objadd = new Advertisement())
                {
                    objadd.AdvertisementID = Convert.ToInt32(e.CommandArgument);
                    objadd.InsUpdAdvertisement(2);
                    if (objadd.ErrorMsg == "")
                        ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                    else
                        ucMsg.ShowError(objadd.ErrorMsg);
                };
                gvbind();
            }
            if (e.CommandName.Equals("editcmd"))
            {
                using (Advertisement objadd = new Advertisement())
                {
                    DataTable dt;
                    objadd.AdvertisementID = Convert.ToInt32(e.CommandArgument);
                    dt = objadd.GetAdvertisement();
                    hdnAddID.Value = e.CommandArgument.ToString();
                    txtCaption.Text = dt.Rows[0]["AdCaption"].ToString();
                    if (dt.Rows[0]["ThumbNailType"].ToString().Equals("2"))
                    {
                        chkMode.Checked = false;
                        fuImage.Visible = false;
                        txtInternetURL.Visible = true;
                        txtInternetURL.Text = dt.Rows[0]["ThumbNailPath"].ToString();
                    }
                    if (dt.Rows[0]["ThumbNailType"].ToString().Equals("1"))
                    {
                        chkMode.Checked = true;
                        fuImage.Visible = true;
                        txtInternetURL.Visible = false;
                    }
                    if (dt.Rows[0]["ThumbNailType"].ToString().Equals("0"))
                    {
                        chkMode.Checked = false;
                        fuImage.Visible = false;
                        txtInternetURL.Visible = true;
                    }
                    txtVideoLink.Text = dt.Rows[0]["AdURL"].ToString();
                    chkStatus.Checked = Convert.ToBoolean(dt.Rows[0]["Status"]);
                    btnSubmit.Text = "Update";
                };
            }
        }
        catch (Exception ex)
        {
            ucMsg.Visible = true;
            ucMsg.ShowError(ex.ToString());
        }
    }
    # endregion
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        hdnCondition.Value = "1";
        gvbind();
    }
    protected void btnsrcCancel_Click(object sender, EventArgs e)
    {
        clear();
        hdnCondition.Value = "0";
        gvbind();
    }
}
