/*
 * ====================================================================================================
 * <copyright company="Zed Axis Technologies">
 * COPYRIGHT (c) 2010 Zed Axis Technologies (P) Ltd. 
 * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
 * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
 * WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
 * </copyright>
 * ====================================================================================================
 * Created By : Sumit Maurya
 * Created On : 04-Oct-2016 
 * Description: This interface is a copy of Manage Orgn Hierarchy (ManageOrgnHierarchy.aspx).
 * ====================================================================================================
 * Change Log:
 * DD-MMM-YYYY, Name , #CCXX - Description
 * ----------------------------------------------------------------------------------------------------
 * 
 * ====================================================================================================
 */
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
using System.Resources;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;

public partial class Masters_HO_Admin_ManageParallelOrgnHierarchy : PageBase
{
    DataTable dtOrg = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {


            updMsg.Update();
            if (!IsPostBack)
            {
                chkActive.Checked = true;
                if (ViewState["EditParallelOrgnhierarchyID"] != null)
                { ViewState.Remove("EditParallelOrgnhierarchyID"); }
                BindHierarchy();

                FillLocationGrid(1);
                ViewState["CurrentPage"] = 1;
                ucMsg.Visible = false;
            }
        }
        catch (Exception ex)
        {

            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }

    }
    private void BindHierarchy()
    {
        try
        {
            DataTable dt = new DataTable();
            ddlHierarchyLevel.Items.Clear();
            ddlSerHierarchyLevel.Items.Clear();
            using (OrgHierarchyData objuser = new OrgHierarchyData())
            {
                dt = objuser.GetAllHierarchyLevel();
            };
            String[] colArray = { "HierarchyLevelID", "HierarchyLevelName" };
            PageBase.DropdownBinding(ref ddlHierarchyLevel, dt, colArray);
            PageBase.DropdownBinding(ref ddlSerHierarchyLevel, dt, colArray);

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }
            if (ddlHierarchyLevel.SelectedIndex == 0 || txtLocationCode.Text.Trim().Length == 0 || txtLocationName.Text.Trim().Length == 0)
            {
                ucMsg.ShowWarning(Resources.Messages.MandatoryField);

                return;
            }
            if (ddlHierarchyLevel.SelectedValue != "2")
            {

            }

            Int32 result = 0;
            using (OrgHierarchyData objOrg = new OrgHierarchyData())
            {

                objOrg.LocationName = txtLocationName.Text.Trim();
                objOrg.LocationCode = txtLocationCode.Text.Trim();
                objOrg.HierarchyLevelID = Convert.ToInt16(ddlHierarchyLevel.SelectedValue);

                objOrg.FullName = txtFullName.Text.Trim();
                objOrg.UserName = txtLoginName.Text.Trim();

                string StrPSalt = "", StrPwd = "";

                if (Convert.ToInt32(ViewState["EditParallelOrgnhierarchyID"]) == 0)
                {
                    using (Authenticates objAuthenticate = new Authenticates())
                    {
                        StrPSalt = objAuthenticate.GenerateSalt(txtPassword.Text.Trim().Length);
                        StrPwd = objAuthenticate.EncryptPassword(txtPassword.Text.Trim(), StrPSalt);
                    }
                    if (txtLoginName.Text.Trim() != "")
                    {
                        if (txtLoginName.Text.Trim().Replace(" ", "").Length != txtLoginName.Text.Trim().Length)
                        {
                            ucMsg.ShowWarning("Blank space not allowed in user name.");
                            return;
                        }
                    }
                    if (txtPassword.Text.Trim() != "")
                    {
                        if (txtPassword.Text.Trim().Replace(" ", "").Length != txtPassword.Text.Trim().Length)
                        {
                            ucMsg.ShowWarning("Blank space not allowed in password.");
                            return;
                        }
                    }
                }
                if (ViewState["EditParallelOrgnhierarchyID"] != null)
                {
                    objOrg.ParallelOrgnHierarchyID = Convert.ToInt32(ViewState["EditParallelOrgnhierarchyID"].ToString());
                }

                objOrg.UserID = PageBase.UserId;
                objOrg.intStatus = Convert.ToInt16(chkActive.Checked == true ? 1 : 0);
                objOrg.Password = StrPwd;
                objOrg.PasswordSalt = StrPSalt;
                objOrg.EmailID = txtEmail.Text.Trim();
                objOrg.MobileNumber = txtMobileNo.Text.Trim();
                result = objOrg.InsUpdParallelOrgnHierarchyUser();
                if (result == 0)
                {
                    if (ViewState["EditParallelOrgnhierarchyID"] == null)
                    {
                        ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    }
                    else
                    {
                        ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                    }
                    ClearForm();
                }
                else if (result == 1 && !string.IsNullOrEmpty(objOrg.Error))
                {
                    ucMsg.ShowInfo(objOrg.Error);
                }
                else if (result == 2 && !string.IsNullOrEmpty(objOrg.Error))
                {
                    ucMsg.ShowError(objOrg.Error);
                }




            }



            updMsg.Update();

        }

        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
            updMsg.Update();
            PageBase.Errorhandling(ex);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtLocationCodeSearch.Text.Trim() != "" || txtLocationNameSearch.Text.Trim() != "" || ddlSerHierarchyLevel.SelectedIndex != 0 || /*txtSerParentLocationName.Text != "" || txtParentCode.Text.Trim() != "" ||*/ txtUserNameSearch.Text.Trim() != "")
        {

            FillLocationGrid(1);

        }
        else
            ucMsg.ShowInfo("Please Enter searching parameter(s).");
        return;

    }

    void ClearForm()
    {
        ddlHierarchyLevel.SelectedIndex = 0;
        // ddlParentHierarchy.SelectedIndex = 0;
        txtMobileNo.Text = "";
        txtEmail.Text = "";
        txtFullName.Text = "";
        txtLoginName.Text = "";
        txtUserNameSearch.Text = "";
        txtPassword.Text = "";

        ddlHierarchyLevel.Enabled = true;
        //  ddlParentHierarchy.Enabled = true;
        txtLocationName.Text = "";
        txtLocationCode.Text = "";
        txtLocationName.Text = "";
        txtLocationCodeSearch.Text = "";
        txtLocationNameSearch.Text = "";
        btnCreate.Text = "Submit";
        ViewState["EditParallelOrgnhierarchyID"] = null;
        ddlSerHierarchyLevel.SelectedIndex = 0;
        //txtSerParentLocationName.Text = "";
        //txtParentCode.Text = "";
        //txtUserName.Text = "";
        FillLocationGrid(Convert.ToInt32(ViewState["CurrentPage"]));
        UpdSearch.Update();
    }

    protected void btnActiveDeactive_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton btnActiveDeactive = (ImageButton)sender;
            Int32 Result = 0;
            Int16 OrgnhierarchyID = Convert.ToInt16(btnActiveDeactive.CommandArgument);
            using (OrgHierarchyData ObjOrgn = new OrgHierarchyData())
            {
                // string lbllocatoinName = gvParallelOrgnHierarchyUser

                ObjOrgn.OrgnhierarchyID = OrgnhierarchyID;
                ObjOrgn.UserID = PageBase.UserId;
                ObjOrgn.intStatus = Convert.ToInt16(chkActive.Checked == true ? 1 : 0);

                Result = ObjOrgn.InsUpdParallelOrgnHierarchyUser(); //1;// ObjOrgn.UpdateStatusOrgnHierarchyInfo();
            };
            if (Result == 0)
            {
                ucMsg.ShowSuccess(Resources.Messages.StatusChanged);
            }
            else
            {
                ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            }
            FillLocationGrid(Convert.ToInt32(ViewState["CurrentPage"]));
            //updgrid.Update();
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        ddlHierarchyLevel.SelectedIndex = -1;
        ImageButton btnEdit = (ImageButton)sender;
        DataTable dtOrgn;
        using (OrgHierarchyData objOrgn = new OrgHierarchyData())
        {
            dtOrgn = objOrgn.GetOrgnHierarchyInfo(Convert.ToInt32(btnEdit.CommandArgument));
        };
        ViewState["EditParallelOrgnhierarchyID"] = Convert.ToInt32(btnEdit.CommandArgument);

        if (dtOrgn != null && dtOrgn.Rows.Count > 0)
        {
            if (ddlHierarchyLevel.Items.FindByValue(dtOrgn.Rows[0]["HierarchyLevelID"].ToString()) != null)
            {
                ddlHierarchyLevel.Items.FindByValue(dtOrgn.Rows[0]["HierarchyLevelID"].ToString()).Selected = true;
                ddlHierarchyLevel.Enabled = false;
            }
            txtLocationName.Text = (dtOrgn.Rows[0]["LocationName"].ToString());
            txtLocationCode.Text = (dtOrgn.Rows[0]["LocationCode"].ToString());
            chkActive.Checked = Convert.ToBoolean(dtOrgn.Rows[0]["Status"]);
            btnCreate.Text = "Update";
            //updAddUserMain.Update();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearForm();
    }


    protected void btnShow_Click(object sender, EventArgs e)
    {
        ClearForm();
        FillLocationGrid(1);

    }
    protected void btnExprtToExcel_Click(object sender, EventArgs e)
    {
        try
        {

            using (OrgHierarchyData objOrg = new OrgHierarchyData())
            {
                objOrg.PageIndex = -1;
                objOrg.PageSize = Convert.ToInt32(PageSize);
                objOrg.UserName = txtUserNameSearch.Text.Trim();

                objOrg.LocationName = txtLocationNameSearch.Text.Trim();
                objOrg.LocationCode = txtLocationCodeSearch.Text.Trim();
                objOrg.HierarchyLevelID = Convert.ToInt16(ddlSerHierarchyLevel.SelectedValue);
                DataTable dtResult = objOrg.GetParallelOrgnHierarchyInfo();
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    DataTable dt = dtResult.Copy();
                    string[] DsCol = new string[] { "LocationName", "LocationCode", "HierarchyLevelName", "LoginName", "Email", "MobileNo", "StatusText" };
                    DataTable DsCopy = new DataTable();
                    dt = dt.DefaultView.ToTable(true, DsCol);
                    dt.Columns["StatusText"].ColumnName = "Status";
                    dt.Columns["LoginName"].ColumnName = "User Name";
                    if (dt.Rows.Count > 0)
                    {
                        DataSet dtcopy = new DataSet();
                        dtcopy.Merge(dt);
                        dtcopy.Tables[0].AcceptChanges();
                        String FilePath = Server.MapPath("~/");
                        string FilenameToexport = "ParallelOrgnHierarchyDetails";
                        PageBase.RootFilePath = FilePath;
                        PageBase.ExportToExecl(dtcopy, FilenameToexport);

                    }
                    else
                    {
                        ucMsg.ShowInfo(Resources.Messages.NoRecord);
                    }
                }
                else
                {

                    ucMsg.ShowInfo("No Record Found");
                }

            }

            //  FillLocationGrid(-1);

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }

    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        FillLocationGrid(ucPagingControl1.CurrentPage);
    }


    private void FillLocationGrid(int pageNo)
    {

        using (OrgHierarchyData objOrg = new OrgHierarchyData())
        {
            objOrg.PageIndex = pageNo;
            objOrg.PageSize = Convert.ToInt32(PageSize);
            objOrg.UserName = txtUserNameSearch.Text.Trim();

            objOrg.LocationName = txtLocationNameSearch.Text.Trim();
            objOrg.LocationCode = txtLocationCodeSearch.Text.Trim();
            objOrg.HierarchyLevelID = Convert.ToInt16(ddlSerHierarchyLevel.SelectedValue);
            DataTable dt = objOrg.GetParallelOrgnHierarchyInfo();
            if (dt != null && dt.Rows.Count > 0)
            {
                dvFooter.Visible = true;
                ViewState["TotalRecords"] = objOrg.TotalRecords;
                ucPagingControl1.TotalRecords = objOrg.TotalRecords;
                ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                ucPagingControl1.SetCurrentPage = pageNo;
                ucPagingControl1.FillPageInfo();
                btnExprtToExcel.Visible = true;
                gvParallelOrgnHierarchyUser.Visible = true;
                gvParallelOrgnHierarchyUser.DataSource = dt;
                gvParallelOrgnHierarchyUser.DataBind();
                //updgrid.Update();

            }
            else
            {
                gvParallelOrgnHierarchyUser.DataSource = null;
                dvFooter.Visible = false;
                btnExprtToExcel.Visible = false;
                gvParallelOrgnHierarchyUser.Visible = false;
                ucMsg.ShowInfo("No Record Found");
            }
            gvParallelOrgnHierarchyUser.DataBind();
            // updgrid.Update();

        }
    }
    protected void gvParallelOrgnHierarchyUser_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Int32 intParallelOrgnHierarchyID = Convert.ToInt32(e.CommandArgument);
            Int32 RowIndex = Convert.ToInt32(e.CommandArgument) - 1;


            HiddenField hdnparallelorgnHierarchyID = gvParallelOrgnHierarchyUser.Rows[RowIndex].FindControl("hdnParallelOrgnHierarchy") as HiddenField;
            HiddenField hdnhierarchyLevelID = gvParallelOrgnHierarchyUser.Rows[RowIndex].FindControl("hdnHierarchyLevelID") as HiddenField;
            Label lblLocationCode = gvParallelOrgnHierarchyUser.Rows[RowIndex].FindControl("lblLocationCode") as Label;
            Label lbllocationName = gvParallelOrgnHierarchyUser.Rows[RowIndex].FindControl("lblLocationName") as Label;
            Label lblloginName = gvParallelOrgnHierarchyUser.Rows[RowIndex].FindControl("lblLoginName") as Label;
            Label lblEmail = gvParallelOrgnHierarchyUser.Rows[RowIndex].FindControl("lblEmail") as Label;
            Label lblmobileNumber = gvParallelOrgnHierarchyUser.Rows[RowIndex].FindControl("lblMobileNumber") as Label;
            HiddenField hdnfirstName = gvParallelOrgnHierarchyUser.Rows[RowIndex].FindControl("hdnFirstName") as HiddenField;
            HiddenField hdnstatus = gvParallelOrgnHierarchyUser.Rows[RowIndex].FindControl("hdnStatus") as HiddenField;

            if (e.CommandName == "Edit")
            {
                ucMsg.Visible = false;
                {
                    ddlHierarchyLevel.SelectedValue = hdnhierarchyLevelID.Value;
                    ViewState["EditParallelOrgnhierarchyID"] = hdnparallelorgnHierarchyID.Value;
                    txtLocationCode.Text = lblLocationCode.Text;
                    txtLocationName.Text = lbllocationName.Text;
                    //txtLocationName.Text = lblloginName.Text;
                    txtEmail.Text = lblEmail.Text;
                    txtMobileNo.Text = lblmobileNumber.Text;
                    txtFullName.Text = hdnfirstName.Value;
                    txtLoginName.Text = lblloginName.Text;
                    if (hdnstatus.Value == "0")
                    {
                        chkActive.Checked = false;
                    }
                    else
                    {
                        chkActive.Checked = true;
                    }
                }
            }

            if (e.CommandName == "Active")
            {
                using (OrgHierarchyData objOrg = new OrgHierarchyData())
                {

                    objOrg.ParallelOrgnHierarchyID = Convert.ToInt32(hdnparallelorgnHierarchyID.Value);
                    objOrg.HierarchyLevelID = Convert.ToInt16(hdnhierarchyLevelID.Value);
                    objOrg.LocationCode = lblLocationCode.Text;
                    objOrg.LocationName = lblloginName.Text;
                    objOrg.UserName = lblloginName.Text;
                    objOrg.FullName = hdnfirstName.Value;
                    objOrg.EmailID = lblEmail.Text;
                    objOrg.MobileNumber = lblmobileNumber.Text;
                    objOrg.UserID = PageBase.UserId;
                    objOrg.intStatus = Convert.ToInt16(hdnstatus.Value == "0" ? 1 : 0);
                    objOrg.EmailID = lblEmail.Text.Trim();
                    objOrg.MobileNumber = lblmobileNumber.Text.Trim();
                    int intresult = objOrg.InsUpdParallelOrgnHierarchyUser();
                    if (intresult == 0)
                    {
                        ClearForm();
                        ucMsg.ShowSuccess(Resources.Messages.StatusChanged);


                    }
                    else if (intresult == 1 && !string.IsNullOrEmpty(objOrg.Error))
                    {
                        ucMsg.ShowInfo(objOrg.Error);
                    }
                    else if (intresult == 2 && !string.IsNullOrEmpty(objOrg.Error))
                    {
                        ucMsg.ShowError(objOrg.Error);
                    }
                    else
                    {
                        ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    }

                    FillLocationGrid(Convert.ToInt32(ViewState["CurrentPage"].ToString()));

                }
            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }


}

