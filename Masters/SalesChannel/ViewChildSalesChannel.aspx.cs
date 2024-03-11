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
/*Change Log:
 * 26-Dec-14, Rakesh Goel, #CC01 - Add code for pwd display
 * 14-Feb-15, Rakesh Goel, #CC02 - Handled column reference for controlling edit rights as index can change
 * 05-May-15, Karam Chand Sharma, #CC03 - Pass user id into  procedure
 * 24-June-15, Karam Chand Sharma, #CC04 - Add login name and password in export in excel.
 */
public partial class Masters_SalesChannel_ViewChildSalesChannel : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.ShowControl = false;
            if (!IsPostBack)
            {
                HideControls();
                FillSalesChannelType();


            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }

    }

    void HideControls()
    {
        dvhide.Visible = false;

    }

    void FillSalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            DataTable dt = new DataTable();
            String[] StrCol = new String[] { "SalesChannelTypeID", "SalesChannelTypeName" };
            ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(PageBase.EntityTypeID);
            ObjSalesChannel.UserID = Convert.ToInt16(PageBase.UserId);
            dt = ObjSalesChannel.GetSalesChannelTypeV3();
            PageBase.DropdownBinding(ref cmbsaleschanneltype, dt, StrCol);
            ViewState["SalesType"] = dt;


        };
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtsaleschannelname.Text == "" && cmbsaleschanneltype.SelectedValue == "0")
            {
                ucMessage1.ShowInfo("Please enter some searching parameter ");
                HideControls();
                return;
            }
            FillGrid();

        }


        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    void FillGrid()
    {
        ViewState["Table"] = null;
        DataTable Dt = new DataTable();
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        /*using (SalesChannelData ObjSalesChannel = new SalesChannelData())*/
        {

            ObjSalesChannel.SalesChannelName = txtsaleschannelname.Text.Trim();
            ObjSalesChannel.SalesChannelID = PageBase.SalesChanelID;
            ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(cmbsaleschanneltype.SelectedValue);
            ObjSalesChannel.UserID = PageBase.UserId;
            Dt = ObjSalesChannel.GetSalesChannelChildInfoV2();
        };
        if (Dt != null && Dt.Rows.Count > 0)
        {
            ViewState["Table"] = Dt;

            GridSalesChannel.DataSource = Dt;
            GridSalesChannel.DataBind();
            if (Convert.ToInt16(Session["HIERARCHYADMIN"]) == 1 || Convert.ToInt16(Session["HIERARCHYADMIN"]) == 0)
            {
                if (PageBase.SalesChanelID != 0)
                {


                    GridSalesChannel.Columns[GetColumnIndexByName(GridSalesChannel, "Status")].Visible = true;
                    GridSalesChannel.Columns[GetColumnIndexByName(GridSalesChannel, "Edit")].Visible = true;

                    /*#CC02 comment start
                    GridSalesChannel.Columns[9].Visible = true;
                    GridSalesChannel.Columns[8].Visible = true;
                    #CC02 comment end*/
                }
            }
            else
            {
                GridSalesChannel.Columns[GetColumnIndexByName(GridSalesChannel, "Status")].Visible = false;
                GridSalesChannel.Columns[GetColumnIndexByName(GridSalesChannel, "Edit")].Visible = false;

                /*#CC02 comment start
                GridSalesChannel.Columns[9].Visible = false;
                GridSalesChannel.Columns[8].Visible = false;
                #CC02 comment end*/
            }
            UpdGrid.Update();
            dvhide.Visible = true;


        }
        else
        {
            HideControls();
            ucMessage1.ShowInfo(Resources.Messages.NoRecord);

        }
        UpdGrid.Update();
    }

    void FillAllGrid()
    {
        ViewState["Table"] = null;
        DataTable Dt = new DataTable();
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        /*using (SalesChannelData ObjSalesChannel = new SalesChannelData())*/
        {

            ObjSalesChannel.SalesChannelName = "";
            ObjSalesChannel.SalesChannelID = PageBase.SalesChanelID;
            ObjSalesChannel.SalesChannelTypeID = 0;
            ObjSalesChannel.UserID = PageBase.UserId;
            Dt = ObjSalesChannel.GetSalesChannelChildInfoV2();
        };
        if (Dt != null && Dt.Rows.Count > 0)
        {
            ViewState["Table"] = Dt;

            GridSalesChannel.DataSource = Dt;
            GridSalesChannel.DataBind();
            if (Convert.ToInt16(Session["HIERARCHYADMIN"]) == 1 || Convert.ToInt16(Session["HIERARCHYADMIN"]) == 0)
            {
                if (PageBase.SalesChanelID != 0)
                {


                    GridSalesChannel.Columns[GetColumnIndexByName(GridSalesChannel, "Status")].Visible = true;
                    GridSalesChannel.Columns[GetColumnIndexByName(GridSalesChannel, "Edit")].Visible = true;

                    /*#CC02 comment start
                    GridSalesChannel.Columns[9].Visible = true;
                    GridSalesChannel.Columns[8].Visible = true;
                    #CC02 comment end*/
                }
            }
            else
            {
                GridSalesChannel.Columns[GetColumnIndexByName(GridSalesChannel, "Status")].Visible = false;
                GridSalesChannel.Columns[GetColumnIndexByName(GridSalesChannel, "Edit")].Visible = false;

                /*#CC02 comment start
                GridSalesChannel.Columns[9].Visible = false;
                GridSalesChannel.Columns[8].Visible = false;
                #CC02 comment end*/
            }
            UpdGrid.Update();
            dvhide.Visible = true;


        }
        else
        {
            HideControls();
            ucMessage1.ShowInfo(Resources.Messages.NoRecord);

        }
        UpdGrid.Update();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

        ucMessage1.ShowControl = false;

        txtsaleschannelname.Text = "";
        HideControls();

    }
    protected void ExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string Password = string.Empty;/*#CC04 ADDED*/
            if (ViewState["Table"] != null)
            {

                DataTable dt = (DataTable)ViewState["Table"];
                /*#CC04 ADDED START*/
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        using (Authenticates ObjAuth = new Authenticates())
                        {
                            Password = ObjAuth.DecryptPassword(Convert.ToString(dr["Password"]), Convert.ToString(dr["PasswordSalt"]));
                        };
                        dr["Password"] = Password;
                    }
                    /*#CC04 ADDED END*/
                    string[] DsCol = new string[] { "SalesChannelName", /*#CC04 ADDED START*/"LoginName", "Password"  /*#CC04 ADDED END*/,"SalesChannelCode", "SalesChannelTypeName", "ParentName", "ContactPerson", "Address1", "Address2", "StateName", "CityName", "DistrictName", "AreaName", "Fax", "PinCode", "CstNumber", "TinNumber", "MobileNumber", "StatusValue", "PhoneNumber", "Email", "BussinessStartDate", "Multilocation"};
                    DataTable DsCopy = new DataTable();
                    dt = dt.DefaultView.ToTable(true, DsCol);
                    dt.Columns["StatusValue"].ColumnName = "Status";

                    if (dt.Rows.Count > 0)
                    {
                        DataSet dtcopy = new DataSet();
                        dtcopy.Merge(dt);
                        dtcopy.Tables[0].AcceptChanges();
                        String FilePath = Server.MapPath("../../");
                        string FilenameToexport = "SalesChannelList";
                        PageBase.RootFilePath = FilePath;
                        PageBase.ExportToExecl(dtcopy, FilenameToexport);
                        ViewState["Table"] = null;
                    }
                    else
                    {
                        ucMessage1.ShowError(Resources.Messages.NoRecord);

                    }

                } ViewState["Table"] = null;
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }


    protected void GridSalesChannel_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridSalesChannel.PageIndex = e.NewPageIndex;
            FillGrid();
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }

    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        try
        {

            txtsaleschannelname.Text = "";
            FillAllGrid();
            cmbsaleschanneltype.SelectedValue = "0";
            txtsaleschannelname.Text = "";
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }

    protected void GridSalesChannel_RowDataBound1(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int CheckResult = 0;
            int ApprovalLevel2;
            int baseEntityTypeId = PageBase.BaseEntityTypeID;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Int32 SalesChannelID = Convert.ToInt32(GridSalesChannel.DataKeys[e.Row.RowIndex].Value);
                using (SalesChannelData ObjSalesChannel = new SalesChannelData())
                {
                    ObjSalesChannel.SalesChannelID = SalesChannelID;
                    CheckResult = ObjSalesChannel.CheckSalesChannelExistence();
                };
                GridViewRow GVR = e.Row;
                Label lblMapping = (Label)GVR.FindControl("lblMapping");
                ImageButton btnStatus = (ImageButton)GVR.FindControl("imgActive");
                ImageButton btnEdit = (ImageButton)GVR.FindControl("img1");
                /*#CC01 add start*/
                //if (baseEntityTypeId == 1)
                //{
                //    btnEdit.Visible = false;
                //    btnStatus.Visible = false;
                //}
                LinkButton hlPassword = default(LinkButton);
                hlPassword = (LinkButton)GVR.FindControl("hlPassword");
                string strPassword = null;
                Label lblPassword = (Label)GVR.FindControl("lblPassword");
                Label lblPasswordSalt = (Label)GVR.FindControl("lblPasswordSalt");
                strPassword = fncChangePwd(lblPassword.Text, lblPasswordSalt.Text);
                hlPassword.Attributes.Add("Onclick", "javascript:alert('Sales channel password is : " + strPassword + "');{return false;}");
                /*#CC01 add end*/
                Label lblNumberofBackDaysSC = (Label)GVR.FindControl("lblNumberofBackDaysSC");
                Label lblNumberofBackDaysSCDisplay = (Label)GVR.FindControl("lblNumberofBackDaysSCDisplay");
                HiddenField hdnApproval2 = (HiddenField)GVR.FindControl("HiddenApprovelLevel2");
                if (lblNumberofBackDaysSC.Text == "-101")
                    lblNumberofBackDaysSCDisplay.Text = "Default";
                else
                    lblNumberofBackDaysSCDisplay.Text = lblNumberofBackDaysSC.Text;
                //lblNumberofBackDaysSCDisplay
                if (Session["HIERARCHYADMIN"] != null)
                {
                    if (Convert.ToInt16(Session["HIERARCHYADMIN"]) == 0)
                    {
                        if (lblMapping.Text == "1")     //Means it is his immediate child
                        {
                            btnStatus.Visible = true;
                            btnEdit.Visible = true;
                        }
                    }
                    else
                    {
                        btnStatus.Visible = true;
                        btnEdit.Visible = true;
                        //full hierarchy under him
                    }

                }

                if ((baseEntityTypeId == 1) && (Convert.ToInt16(hdnApproval2.Value) ==0))
                {
                    btnEdit.Visible = false;
                    btnStatus.Visible = false;
                    //GVR.Cells[10].Visible = false;
                    //GVR.Cells[11].Visible = false;
                }
                ImageButton btnLock = (ImageButton)GVR.FindControl("imgLocked");
                Label lblLock = (Label)GVR.FindControl("lblLocked");
                if (lblLock.Text.ToLower() == "true")
                {
                    btnLock.Visible = true;
                    btnLock.ImageUrl = PageBase.siteURL + "/" + strAssets + "/CSS/images/Lock.png";
                    btnLock.ToolTip = "unlock user";
                }
                else
                    btnLock.Visible = false;

                HyperLink HLDetails = default(HyperLink);
                HLDetails = (HyperLink)GVR.FindControl("HLDetails");
                string strViewDBranchDtlURL = null;

                //strViewDBranchDtlURL = "ViewSalesChannelDetail.aspx?SalesChannelId=" + Crypto.Encrypt(Convert.ToString(SalesChannelID), PageBase.KeyStr);
                strViewDBranchDtlURL = Server.UrlEncode(Crypto.Encrypt(Convert.ToString(SalesChannelID), PageBase.KeyStr)).ToString().Replace("+", " ");

                //{
                //    HLDetails.Text = "Details";
                //    HLDetails.NavigateUrl = "#";
                //    HLDetails.Attributes.Add("OnClick", "popup('" + strViewDBranchDtlURL + "')");
                //}
                {
                    HLDetails.Text = "Details";

                    HLDetails.Attributes.Add("onClick", string.Format("return popup('" + strViewDBranchDtlURL + "')"));
                }

                if (CheckResult > 0)
                {


                    if (btnStatus != null)
                    {
                        btnStatus.Attributes.Add("Onclick", "javascript:alert('This sales channel is linked to existing data.You can not deactivate it.');{return false;}");

                    }
                }
                //else
                //{

                //    if (btnStatus != null)
                //    {
                //        btnStatus.Attributes.Add("OnClick", "if(!confirm('Are you sure want to deactivate?')){return false;}");
                //    }
                //}
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }


    public string fncChangePwd(string vPassword, string vPasswordSalt)  /*#CC01 added function*/
    {
        string vMailPassword = string.Empty;
        try
        {
            using (Authenticates objAuth = new Authenticates())
            {
                vMailPassword = objAuth.DecryptPassword(vPassword, vPasswordSalt);
            };
        }
        catch (Exception ex)
        {


            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
        return vMailPassword;
    }
    protected void GridSalesChannel_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int Result = 0;
            Int32 SalesChannelId = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
            Label lblNumberofBackdays = row.FindControl("lblNumberofBackDaysSC") as Label;
            lblSelectedSalesChannelNumberofbackdays.Text = lblNumberofBackdays.Text;
            if (e.CommandName == "cmdEdit")
            {

                Response.Redirect("ManageSalesChannel.aspx?SalesChannelId=" + Server.UrlEncode(Crypto.Encrypt(Convert.ToString(SalesChannelId), PageBase.KeyStr)));
            }
            if (e.CommandName == "Active")
            {

                if (SalesChannelId > 0)
                {
                    using (SalesChannelData ObjSalesChannel = new SalesChannelData())
                    {
                        ObjSalesChannel.SalesChannelID = SalesChannelId;
                        Result = ObjSalesChannel.UpdateStatusSalesChannelInfo();
                    };
                    if (Result > 0)
                    {
                        ucMessage1.ShowSuccess(Resources.Messages.StatusChanged);
                    }
                    else
                    {
                        ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    }
                    FillGrid();
                }
            }
            if (e.CommandName.ToLower() == "unlock")
            {

                if (SalesChannelId > 0)
                {
                    using (UserData ObjUser = new UserData())
                    {
                        Int32 UserId = Convert.ToInt32(e.CommandArgument);
                        ObjUser.UserID = UserId;
                        ObjUser.ActionId = 1;
                        Result = ObjUser.UpdateUserLoginStatus();
                    };
                    if (Result == 1 | Result == -1)
                    {
                        ucMessage1.ShowSuccess(Resources.Messages.LockedOut);
                    }
                    else
                    {
                        ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    }
                    FillGrid();

                }

            }
            if (e.CommandName.ToLower() == "cmdeditnumberofbackdays")
            {
                lblSelectedSalesChannelId.Text = Convert.ToString(SalesChannelId);
                txtNumberofbackdays.Text = lblSelectedSalesChannelNumberofbackdays.Text == "-101" ? string.Empty : lblSelectedSalesChannelNumberofbackdays.Text;
                ModelPopJustConfirmation.Show();
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void btnSubmitNumberofBackDays_Click(object sender, EventArgs e)
    {
        int Saleschannelid = Convert.ToInt32(lblSelectedSalesChannelId.Text);
        int intnumberofbackdays = Convert.ToInt32(lblSelectedSalesChannelNumberofbackdays.Text);
        if (SubmitNumberOfBackDays(Saleschannelid, intnumberofbackdays) == 0)
        {


            ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);
        }
        else
        {
            ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
        }
        ModelPopJustConfirmation.Hide();
        FillGrid();
    }

    Int32 SubmitNumberOfBackDays(int SalesChannelId, int numberofbackdays)
    {
        Int32 SubmissionResult;
        if (SalesChannelId > 0)
        {
            using (SalesChannelData ObjSalesChannel = new SalesChannelData())
            {
                ObjSalesChannel.SalesChannelID = SalesChannelId;
                ObjSalesChannel.NumberofBackDaysSC = txtNumberofbackdays.Text.Trim().Replace(",", "") == string.Empty ? -101 : Convert.ToInt32(txtNumberofbackdays.Text.Trim().Replace(",", ""));
                SubmissionResult = ObjSalesChannel.UpdateNumberofBackdaysofSalesChannel();
                return SubmissionResult;
            }


        }
        else
        {
            return 1;/*some error*/
        }

    }

    /*#CC02 add start  --need to take this to common class*/
    private int GetColumnIndexByName(GridView grid, string name)
    {
        foreach (DataControlField col in grid.Columns)
        {
            if (col.HeaderText.ToLower().Trim() == name.ToLower().Trim())
            {
                return grid.Columns.IndexOf(col);
            }
        }

        return -1;
    }

    /*#CC02 add end*/
}
