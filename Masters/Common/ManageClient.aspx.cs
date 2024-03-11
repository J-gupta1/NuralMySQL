/*
 Change Log
 * 19-Dec-2020, Balram Jha #CC01- Addition of Company setting
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;
using System.IO;

public partial class Masters_Common_ManageClient : PageBase
{
    string LogoPath = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewPanel();
        }
    }
    #region UserDifine Method

    private void ClearAll()
    {
        txtClientNm.Text = "";
        txtClientFolderNm.Text = "";
        txtApplicationTitle.Text = "";
        txtSiteUrl.Text = "";
        txtCopyRight.Text = "";
        txtEmailSignature.Text = "";
        chkSUM.Checked = false;
    }
    private void Clearucmsg()
    {
        ucMsg.Visible = false;
    }
    private void Showucmsg()
    {
        ucMsg.Visible = true;
    }
    private void AddPanel()
    {
        
        using (CommonMaster objInsertMC = new CommonMaster())
        {
            try
            {
                
                objInsertMC.ClientName = txtClientNm.Text;
                objInsertMC.ClientFolderName = txtClientFolderNm.Text;
                objInsertMC.ApplicationTitle = txtApplicationTitle.Text;
                objInsertMC.SiteUrl = txtSiteUrl.Text;
                objInsertMC.SUM = Convert.ToInt32(chkSUM.Checked);
                objInsertMC.Active = Convert.ToInt32(chkStatus.Checked);
                objInsertMC.EmailSignature = txtEmailSignature.Text;
                objInsertMC.CopyRightText = txtCopyRight.Text;
                //#CC01 start
                DataTable dtSetting = GetSettingTable();

                objInsertMC.TVPClientSetting = dtSetting;
                objInsertMC.UserID = PageBase.UserId;
                //#CC01 end
                objInsertMC.InsertManageClient();
                if (objInsertMC.InsError != null)
                {
                    ucMsg.ShowError(objInsertMC.InsError);
                }
                else
                {
                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                }
            }
            catch (Exception ex)
            {
                if (objInsertMC.Error != null && objInsertMC.Error != "" && objInsertMC.Error != "0")
                {
                    ucMsg.ShowError(objInsertMC.Error);
                }
                else
                    ucMsg.ShowError(ex.Message);
            }
        }
    }
    private DataTable GetSettingTable()//#CC01 Added
    {
        DataTable dtSetting = (DataTable)ViewState["ClientSetting"];


        for (int counter = 0; counter < gvSettings.Rows.Count; counter++)
        {
            CheckBox chk = (CheckBox)gvSettings.Rows[counter].FindControl("chkSameAsDefault");
            CheckBox chkgvStatus = (CheckBox)gvSettings.Rows[counter].FindControl("chkStatus");
            Label lbl = (Label)gvSettings.Rows[counter].FindControl("lblDefaultValue");

            if (!chk.Checked)
                dtSetting.Rows[counter]["NewPropertyValue"] = ((TextBox)gvSettings.Rows[counter].FindControl("txtNewPropertyValue")).Text.Trim();
            else
                dtSetting.Rows[counter]["NewPropertyValue"] = lbl.Text.Trim();
            if (chkgvStatus.Checked)
                dtSetting.Rows[counter]["Status"] = 1;
            else
                dtSetting.Rows[counter]["Status"] = 0;

        }
        if (LogoPath.Length > 0)
        {
            DataRow[] drLogo = dtSetting.Select("PropertyName='ClientLogo'");
            drLogo[0]["NewPropertyValue"] = LogoPath;
            drLogo[0]["Status"] = 1;
        }
        dtSetting.Columns.Remove("PropertyName");
        dtSetting.Columns.Remove("PropertyDescription");
        dtSetting.Columns.Remove("PropertyValue");
        dtSetting.AcceptChanges();
        return dtSetting;
    }
    private void ViewPanel()
    {
        using (CommonMaster objGetMC = new CommonMaster())
        {
            try
            {
                DataSet dsClient = objGetMC.GetManageClient(0);
                gvManageClient.DataSource = dsClient.Tables[0];
                gvManageClient.DataBind();

                //#CC01 start 
                ViewState["ClientSetting"] = dsClient.Tables[1];
                gvSettings.DataSource = dsClient.Tables[1];
                gvSettings.DataBind();
                //#CC01 edn
            }
            catch (Exception ex)
            {
                if (objGetMC.Error != null && objGetMC.Error != "" && objGetMC.Error != "0")
                {
                    ucMsg.ShowError(objGetMC.Error);
                }
            }
        }
    }
    private void UpdateAll()
    {
        using (CommonMaster objUpdateMC = new CommonMaster())
        {
            try
            {
                objUpdateMC.ClientName = txtClientNm.Text;
                objUpdateMC.ClientFolderName = txtClientFolderNm.Text;
                objUpdateMC.ApplicationTitle = txtApplicationTitle.Text;
                objUpdateMC.SiteUrl = txtSiteUrl.Text;
                objUpdateMC.SUM = Convert.ToInt32(chkSUM.Checked);
                objUpdateMC.Active = Convert.ToInt32(chkStatus.Checked);
                objUpdateMC.ClientId = Convert.ToInt32(hdnManageClient.Value);
                objUpdateMC.EmailSignature = txtEmailSignature.Text;
                objUpdateMC.CopyRightText = txtCopyRight.Text;
                //#CC01 start
                DataTable dtSetting = GetSettingTable();

                objUpdateMC.TVPClientSetting = dtSetting;
                objUpdateMC.UserID = PageBase.UserId;
                //#CC01 end
                objUpdateMC.UpdateManageClient(0);
                if (objUpdateMC.InsError != null)
                {
                    ucMsg.ShowError(objUpdateMC.InsError);
                }
                else
                {
                    ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                    //btnSubmit.Text = "Submit";
                    //btnClear.Text = "Cancel";   
                }
            }
            catch (Exception ex)
            {
                if (objUpdateMC.Error != null && objUpdateMC.Error != "" && objUpdateMC.Error != "0")
                {
                    ucMsg.ShowError(objUpdateMC.Error);
                }
            }
        }
    }

    #endregion
    #region Control Method

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //#CC01 start
        if (!(FileUploadCompanyLogo.HasFile && ((Path.GetExtension(FileUploadCompanyLogo.FileName).ToLower() == ".jpg")
            || (Path.GetExtension(FileUploadCompanyLogo.FileName).ToLower() == ".jpeg")
            || (Path.GetExtension(FileUploadCompanyLogo.FileName).ToLower() == ".png")))
            )
        {
            ucMsg.ShowError("Please upload jpg or png file.");
            return;
        }
        else if (FileUploadCompanyLogo.HasFile && FileUploadCompanyLogo.PostedFile.ContentLength > 5120000)
        {
            ucMsg.ShowError("Size of Logo image can not exceed 5 MB.");
            return;
        }
        if (FileUploadCompanyLogo.HasFile)
        {
            string NewFileName=Guid.NewGuid().ToString("N") + Path.GetExtension(FileUploadCompanyLogo.FileName).ToLower();
            LogoPath = siteURL + "UploadDownload/clientlogo/" + NewFileName;
            string savePath = Server.MapPath("~") + "\\UploadDownload\\clientlogo\\"+NewFileName;
            FileUploadCompanyLogo.SaveAs(savePath);
        }
        //#CC01 end
        if (hdnManageClient.Value.Equals(""))
        {
            AddPanel();
            ClearAll();
            ViewPanel();
        }
        else 
        {
            UpdateAll();
            ClearAll();
            hdnManageClient.Value = "";
            ViewPanel();
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearAll();
        ucMsg.Visible = false;
        hdnManageClient.Value = "";
    }
    protected void gvManageClient_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvManageClient.PageIndex = e.NewPageIndex;
        ViewPanel();
    }
    protected void gvManageClient_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ucMsg.Visible = false;
        if (e.CommandName.Equals("editcmd"))
        {
            using (CommonMaster objcmdMC = new CommonMaster())
            {
                try
                {
                    objcmdMC.ClientId = Convert.ToInt32(e.CommandArgument);
                    DataSet ds = objcmdMC.GetManageClient(1);
                    txtClientNm.Text = ds.Tables[0].Rows[0]["ClientName"].ToString();
                    txtClientFolderNm.Text = ds.Tables[0].Rows[0]["ClientFolderName"].ToString();
                    txtApplicationTitle.Text = ds.Tables[0].Rows[0]["ApplicationTitle"].ToString();
                    txtSiteUrl.Text = ds.Tables[0].Rows[0]["SiteURL"].ToString();
                    txtEmailSignature.Text = ds.Tables[0].Rows[0]["EmailSignature"].ToString();
                    txtCopyRight.Text = ds.Tables[0].Rows[0]["CopyRightText"].ToString();
                    if (ds.Tables[0].Rows[0]["SiteUnderMaintenance"].ToString().Equals("1")) { chkSUM.Checked = true; }
                    if (ds.Tables[0].Rows[0]["SiteUnderMaintenance"].ToString().Equals("0")) { chkSUM.Checked = false; }
                    if (ds.Tables[0].Rows[0]["Status"].ToString().Equals("1")) { chkStatus.Checked = true; }
                    if (ds.Tables[0].Rows[0]["Status"].ToString().Equals("0")) { chkStatus.Checked = false; }
                    hdnManageClient.Value = e.CommandArgument.ToString();

                    //#CC01 start 
                    ViewState["ClientSetting"] = ds.Tables[1];
                    gvSettings.DataSource = ds.Tables[1];
                    gvSettings.DataBind();
                    //#CC01 edn


                }
                catch (Exception ex)
                {
                    if (objcmdMC.Error != null && objcmdMC.Error != "" && objcmdMC.Error != "0")
                    {
                        ucMsg.Visible = true;
                        ucMsg.ShowError(objcmdMC.Error);
                    }
                }
            }
        }
        if (e.CommandName.Equals("togglecmdStatus"))
        {
            using (CommonMaster objUpdatecmdMC = new CommonMaster())
            {
                    try
                    {
                        objUpdatecmdMC.ClientId = Convert.ToInt32(e.CommandArgument);
                        //#CC01 start
                        DataTable dtSetting = GetSettingTable();

                        objUpdatecmdMC.TVPClientSetting = dtSetting;
                        objUpdatecmdMC.UserID = PageBase.UserId;
                        //#CC01 end
                        objUpdatecmdMC.UpdateManageClient(2);
                        ViewPanel();
                        ucMsg.Visible = true;
                        ucMsg.ShowSuccess(Resources.Messages.StatusChanged);
                    }
                    catch (Exception ex)
                    {
                        if (objUpdatecmdMC.Error != null && objUpdatecmdMC.Error != "" && objUpdatecmdMC.Error != "0")
                        {
                            ucMsg.Visible = true;
                            ucMsg.ShowError(objUpdatecmdMC.Error);
                        }
                    }
            }
        }
        if (e.CommandName.Equals("togglecmdSUM"))
        {
            using (CommonMaster objUpdatecmdMC = new CommonMaster())
            {
                try
                {
                    objUpdatecmdMC.ClientId = Convert.ToInt32(e.CommandArgument);
                    //#CC01 start
                    DataTable dtSetting = GetSettingTable();

                    objUpdatecmdMC.TVPClientSetting = dtSetting;
                    objUpdatecmdMC.UserID = PageBase.UserId;
                    //#CC01 end
                    objUpdatecmdMC.UpdateManageClient(1);
                    ViewPanel();
                    ucMsg.Visible = true;
                    ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                }
                catch (Exception ex)
                {
                    if (objUpdatecmdMC.Error != null && objUpdatecmdMC.Error != "" && objUpdatecmdMC.Error != "0")
                    {
                        ucMsg.Visible = true;
                        ucMsg.ShowError(objUpdatecmdMC.Error);
                    }
                }
            }
        }
    }

    #endregion
}
