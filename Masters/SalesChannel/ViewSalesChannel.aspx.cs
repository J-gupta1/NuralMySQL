using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;
using Cryptography;
/*Change Log:
 * 14-Apr-14, Rakesh Goel, #CC01 - Make Sales Channel Type selection non-mandatory
 * 23-Jun-14, Rakesh Goel, #CC02 - Add Product Category mapped to Channel in Excel output
 * 29-Dec-14, Rakesh Goel, #CC03 - Proper message not coming on user unlocking. Fixed the condition
 * 20 Sep 2014, Karam Chand Sharma, #CC04,  Change the label Address 1 and Address 2 to Address Line 1 and Address Line 2 respectively
 * 10-Aug-2017, Sumit Maurya, #CC05, Column name changed from CSTNumber To GSTNumber. (Implemeted for Comio)
 * 14-June-2018, Rajnish Kumar, #CC06, Sales Returns Back Days
 * 27-Jul-2018, Sumit Maurya, #CC07, UCPaging Added instead of Gridview inbuilt paging (Done for Karbonn).
 * 17-Aug-2018, Balram Jha, #CC08, Checked TableRow Count in place of total records parameter as it is not initialized in class method
 * 06-Nov-2018, Rakesh Raj, #CC09, Inactive/Active Sales channel TicketId#32895
 */

public partial class Masters_HO_SalesChannel_ViewSalesChannel : PageBase
{
    DataTable DtSalesChannelDetail = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.ShowControl = false;
            if (!IsPostBack)
            {
                pageInfo();
                fillBrandCategoryDDL();
                HideControls();
                FillsalesChannelType();
                FillBrand();
                txtNumberofbackdays.Value = string.Empty;
                if (PageBase.BaseEntityTypeID == 2 && PageBase.SalesChannelLevel == 1)
                {
                    LBAddSalesChannel.Visible = true;
                   
                }
                else
                {
                    LBAddSalesChannel.Visible = false;
                   
                }

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
    void FillBrand()
    {
        using (ProductData ObjProduct = new ProductData())
        {
            ObjProduct.SearchType = EnumData.eSearchConditions.Active;
            String[] StrCol = new String[] { "BrandID", "BrandName" };
            PageBase.DropdownBinding(ref cmbBrandName, ObjProduct.GetAllBrandByParameters(), StrCol);

        };
    }

    void FillsalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.Type = 5;           //For Mapping
            if (Convert.ToInt32(PageBase.SalesChanelTypeID) != 0)
            {
                ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
            }
            ObjSalesChannel.UserID = PageBase.UserId;
            ObjSalesChannel.CompanyId = PageBase.ClientId;
            string[] str = { "SalesChannelTypeID", "SalesChannelTypeName" };
            PageBase.DropdownBinding(ref cmbsaleschanneltype, ObjSalesChannel.GetSalesChannelTypeV3(), str);
        };
    }
    void HideControls()
    {
        ExportToExcel.Visible = false;
        GridSalesChannel.Visible = false;
        // UpdGrid.Update();

    }
    public string fncChangePwd(string vPassword, string vPasswordSalt)
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


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            /*#CC01 comment start
            if (cmbsaleschanneltype.SelectedIndex == 0)
            {
                ucMessage1.ShowInfo("Please select Sales Channel Type");
                //GridSalesChannel.DataSource = null;
                //GridSalesChannel.DataBind();

                HideControls();
                return;
            } #CC01 comment end*/
            BindGrid(1);/* #CC07 Added */
            /* FillGrid(); #CC07 Commented  */

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    void FillGrid()
    {
        //  ViewState["Table"] = null;

        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {

            ObjSalesChannel.SalesChannelName = txtsaleschannelname.Text.Trim();
            ObjSalesChannel.SalesChannelCode = txtsaleschannelcode.Text.Trim();
            ObjSalesChannel.StatusValue = Convert.ToInt32(ddlactive.SelectedValue);
            if (cmbsaleschanneltype.SelectedValue != "0")
            {

                ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(cmbsaleschanneltype.SelectedValue);
            }
            if (cmbBrandName.SelectedValue != "0")
            {
                ObjSalesChannel.Brand = Convert.ToInt16(cmbBrandName.SelectedValue);
            }
            DtSalesChannelDetail = ObjSalesChannel.GetSalesChannelInfo();
        };
        if (DtSalesChannelDetail != null && DtSalesChannelDetail.Rows.Count > 0)
        {
            //  ViewState["Table"] = DtSalesChannelDetail;
            ExportToExcel.Visible = true;
            GridSalesChannel.Visible = true;
            GridSalesChannel.DataSource = DtSalesChannelDetail;
            GridSalesChannel.DataBind();
            dvhide.Visible = true;
        }
        else
        {
            HideControls();
            GridSalesChannel.Visible = false;
            GridSalesChannel.DataSource = null;
            GridSalesChannel.DataBind();
            ucMessage1.ShowInfo(Resources.Messages.NoRecord);
            dvhide.Visible = false;
        }
    }

    public string LoginStatus(Int16 IsActive)
    {
        string imgUrl = PageBase.siteURL + "/" + strAssets + "/CSS/images/status_offline.png";
        if (IsActive == 1)
        { imgUrl = PageBase.siteURL + "/" + strAssets + "/CSS/images/status_online.png"; }
        return imgUrl;
    }
    public string LoginToolTip(Int16 IsActive)
    {
        string ToolTip = "Online";
        if (IsActive == 0)
        { ToolTip = "Offline"; }
        return ToolTip;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

        ucMessage1.ShowControl = false;
        cmbsaleschanneltype.SelectedValue = "0";
        txtsaleschannelname.Text = "";
        txtsaleschannelcode.Text = "";
        pnlBrand.Visible = false;
        cmbBrandName.SelectedValue = "0";
        HideControls();
        dvhide.Visible = false;
    }
    protected void ExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string Password = string.Empty;
            FillGrid();
            //if (ViewState["Table"] != null)
            //{
            DataTable dt = DtSalesChannelDetail.Copy();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    using (Authenticates ObjAuth = new Authenticates())
                    {
                        Password = ObjAuth.DecryptPassword(Convert.ToString(dr["Password"]), Convert.ToString(dr["PasswordSalt"]));
                    };
                    dr["Password"] = Password;
                }

            }
            string[] DsCol = new string[] { "sequence", "SalesChannelName", "LoginName", "Password", "SalesChannelCode","ND", "SalesChannelTypeName", "ParentCode", "ParentName", "LocationName", "ContactPerson", "Address1", "Address2", "StateName", "CityName", "DistrictName", "AreaName", "Fax", "PinCode", "CstNumber", "TinNumber", "MobileNumber", "StatusValue", "PhoneNumber", "Email", "BussinessStartDate", "OpeningStockEntered", "GroupParentName", "Multilocation", "ZSMName", "SBHName", "BackDaysForListing", "ChannelProductCategory" };
            DataTable DsCopy = new DataTable();
            dt = dt.DefaultView.ToTable(true, DsCol);
            dt.Columns["StatusValue"].ColumnName = "Status";
            dt.Columns["LocationName"].ColumnName = "Repo.Hierarchy Name";
            dt.Columns["SBHName"].ColumnName = "ASMName";
            dt.Columns["BackDaysForListing"].ColumnName = "Back Days";
            dt.Columns["ChannelProductCategory"].ColumnName = "Mapped Product Categories";  //#CC02 added
            dt.Columns["Address1"].ColumnName = "Address Line 1";/*#CC04 ADDED*/
            dt.Columns["Address2"].ColumnName = "Address Line 2";/*#CC04 ADDED*/
            dt.Columns["CstNumber"].ColumnName = "GSTNumber"; /* #CC05 Added */

            if (dt.Rows.Count > 0)
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "SalesChannelList";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dtcopy, FilenameToexport);
                //  ViewState["Table"] = null;
            }
            else
            {
                ucMessage1.ShowError(Resources.Messages.NoRecord);

            }
            // ViewState["Table"] = null;
            //   }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
    protected void GridSalesChannel_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int Result = 0;
        Int32 SalesChannelId = Convert.ToInt32(e.CommandArgument);
        //GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
        Label lblNumberofBackdays = new Label();
        lblNumberofBackdays = (((Control)e.CommandSource).NamingContainer).FindControl("lblNumberofBackDaysSC") as Label;

        if (lblNumberofBackdays != null)
            //lblNumberofBackdays=row.FindControl("lblNumberofBackDaysSC") as Label;
            lblSelectedSalesChannelNumberofbackdays.Text = lblNumberofBackdays.Text;
        try
        {
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
                    //FillGrid();
                    BindGrid(1);
                }
            }
            if (e.CommandName == "Online")
            {
                if (SalesChannelId > 0)
                {
                    using (UserData ObjUser = new UserData())
                    {
                        Int32 UserId = Convert.ToInt32(e.CommandArgument);
                        ObjUser.UserID = UserId;
                        Result = ObjUser.UpdateUserLoginStatus();
                    };
                    if (Result > 0)
                    {
                        ucMessage1.ShowSuccess(Resources.Messages.LogOff);
                    }
                    else
                    {
                        ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    }
                    //FillGrid();
                    BindGrid(1);
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
                    if (Result == 1 | Result == -1)   /*#CC03 added Result == 1 as well*/
                    {
                        ucMessage1.ShowSuccess(Resources.Messages.LockedOut);
                    }
                    else
                    {
                        ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    }
                    //FillGrid();
                    BindGrid(1);
                }

            }
            if (e.CommandName.ToLower() == "cmdeditnumberofbackdays")
            {
                lblSelectedSalesChannelId.Text = Convert.ToString(SalesChannelId);
                txtNumberofbackdays.Value = string.Empty;
                txtNumberofbackdays.Value = lblSelectedSalesChannelNumberofbackdays.Text == "-101" ? string.Empty : lblSelectedSalesChannelNumberofbackdays.Text;

                ModelPopJustConfirmation.Show();

            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
        if (e.CommandName == "cmdEdit")
        {
            //#CC09
            Response.Redirect("UpdateSalesChannel.aspx?SalesChannelId=" +  Server.UrlEncode(Crypto.Encrypt(Convert.ToString(SalesChannelId), PageBase.KeyStr)) + "&ChangeStatus=1");
            //Response.Redirect("ManageSalesChannel.aspx?SalesChannelId=" + Server.UrlEncode(Crypto.Encrypt(Convert.ToString(SalesChannelId), PageBase.KeyStr)));
        }
    }

    void pageInfo()
    {
        this.GridSalesChannel.Columns[0].HeaderText = Resources.Messages.SalesEntity + " Name";
        this.GridSalesChannel.Columns[3].HeaderText = Resources.Messages.SalesEntity + " Code";
        this.GridSalesChannel.Columns[7].HeaderText = Resources.Messages.SalesEntity + " Type";
    }

    protected void GridSalesChannel_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int CheckResult = 0;
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
               //#CC09 ImageButton btnStatus = (ImageButton)GVR.FindControl("imgActive");
                //Pankaj Dhingra   09/04/2012
                HiddenField hdnStatus = (HiddenField)GVR.FindControl("hdnStatus");
                ImageButton btnEdit = (ImageButton)GVR.FindControl("img1");

                /*#CC09*/
                if (hdnStatus.Value == "0")
                {
                 //  btnStatus.PostBackUrl = "ManageSalesChannel.aspx?SalesChannelId=" + Server.UrlEncode(Crypto.Encrypt(Convert.ToString(SalesChannelID), PageBase.KeyStr)) + "&ChangeStatus=1";
                    btnEdit.Visible = false;
                }
               
                //Pankaj Dhingra  09/04/2012
                HyperLink HLDetails = default(HyperLink);
                HLDetails = (HyperLink)GVR.FindControl("HLDetails");
                string strViewDBranchDtlURL = null;
                LinkButton hlPassword = default(LinkButton);
                hlPassword = (LinkButton)GVR.FindControl("hlPassword");
                string strPassword = null;
                Label lblPassword = (Label)GVR.FindControl("lblPassword");
                Label lblPasswordSalt = (Label)GVR.FindControl("lblPasswordSalt");
                strPassword = fncChangePwd(lblPassword.Text, lblPasswordSalt.Text);
                hlPassword.Attributes.Add("Onclick", "javascript:alert('Sales channel password is : " + strPassword + "');{return false;}");
                strViewDBranchDtlURL = Server.UrlEncode(Crypto.Encrypt(Convert.ToString(SalesChannelID), PageBase.KeyStr)).ToString().Replace("+", " ");
                {
                    HLDetails.Text = "Details";
                    HLDetails.Attributes.Add("onClick", string.Format("return popup('" + strViewDBranchDtlURL + "')"));
                }
                Label lblOnline = (Label)GVR.FindControl("lblOnline");
                ImageButton Online = (ImageButton)GVR.FindControl("imgOnline");
                if (lblOnline.Text.ToLower() == "false")
                {
                    Online.Attributes.Add("Onclick", "javascript:alert('User already logged off.');{return false;}");
                }

                /*#CC09 
                if (CheckResult > 0)
                {
                    if ((btnStatus != null) && (hdnStatus.Value == "1"))
                    {
                        btnStatus.Attributes.Add("Onclick", "javascript:alert('This sales channel is linked to existing data.You can not deactivate it.');{return false;}");

                    }

                }*/

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
    protected void LBAddSalesChannel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageSalesChannel.aspx");
    }
    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            cmbsaleschanneltype.SelectedValue = "0";
            txtsaleschannelcode.Text = "";
            txtsaleschannelname.Text = "";
            cmbBrandName.SelectedValue = "0";
            ddlactive.SelectedValue = "2";
           /* FillGrid(); #CC07 Commented */
            BindGrid(1); /* #CC01 Added */
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }

    protected void cmbsaleschanneltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        CheckBrandSalesChannelMapping();
    }
    void CheckBrandSalesChannelMapping()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            Int32 Result = 0;
            ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(cmbsaleschanneltype.SelectedValue);
            Result = ObjSalesChannel.CheckSalesChannelBrandMapping();
            if (Result > 0)
            {
                if (cmbsaleschanneltype.SelectedValue == "5")
                    pnlBrand.Visible = false;
                else
                    pnlBrand.Visible = true;
            }
            else
                pnlBrand.Visible = false;
            cmbBrandName.SelectedValue = "0";
        };
    }
    protected void btnSubmitNumberofBackDays_Click(object sender, EventArgs e)
    {
        int Saleschannelid = Convert.ToInt32(lblSelectedSalesChannelId.Text);
        //int intnumberofbackdays = Convert.ToInt32(lblSelectedSalesChannelNumberofbackdays.Text);
        string strNumberofBackDays = txtNumberofbackdays.Value.Trim();
        string strNumberofBackDaysSaleReturn = txtNumberofbackdaysSaleReturns.Value.Trim(); /*#CC06*/
        if (strNumberofBackDays != string.Empty)
        {
            if (strNumberofBackDays.Length > 4)
            {
                ucMessage1.ShowError("Please Enter valid number of days");
                ModelPopJustConfirmation.Hide();
                return;
            }
            Int32 result;
            Int32.TryParse(strNumberofBackDays, out result);
            if (result > 0)
            {
                strNumberofBackDays = result.ToString();
            }
        }
        /*#CC06 start*/
        if (strNumberofBackDaysSaleReturn != string.Empty)
        {
            if (strNumberofBackDaysSaleReturn.Length > 4)
            {
                ucMessage1.ShowError("Please Enter valid number of days");
                ModelPopJustConfirmation.Hide();
                return;
            }
            Int32 result;
            Int32.TryParse(strNumberofBackDaysSaleReturn, out result);
            if (result > 0)
            {
                strNumberofBackDaysSaleReturn = result.ToString();
            }
        } /*#CC06 start*/

        if (SubmitNumberOfBackDays(Saleschannelid, strNumberofBackDays, strNumberofBackDaysSaleReturn) == 0)  /*#CC06 start*/
        {


            ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);
        }
        else
        {
            ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
        }
        ModelPopJustConfirmation.Hide();
        txtNumberofbackdays.Value = string.Empty;
        FillGrid();
    }

    Int32 SubmitNumberOfBackDays(int SalesChannelId, string intnumberofbackdays, string intnumberofbackdaysSaleReturns)  /*#CC06 added*/
    {
        Int32 SubmissionResult;
        if (SalesChannelId > 0)
        {
            using (SalesChannelData ObjSalesChannel = new SalesChannelData())
            {
                ObjSalesChannel.SalesChannelID = SalesChannelId;
                ObjSalesChannel.NumberofBackDaysSC = intnumberofbackdays == string.Empty ? -101 : Convert.ToInt32(intnumberofbackdays);
                /*#CC06 added*/
                ObjSalesChannel.NumberofBackDaysSCSaleReturns = intnumberofbackdaysSaleReturns == string.Empty ? -101 : Convert.ToInt32(intnumberofbackdaysSaleReturns);
                SubmissionResult = ObjSalesChannel.UpdateNumberofBackdaysofSalesChannel();
                return SubmissionResult;
            }


        }
        else
        {
            return 1;/*some error*/
        }

    }


    /* #CC07 Add Start */



    void BindGrid(int pageno)
    {
        try
        {
            ViewState["TotalRecords"] = 0;
            using (SalesChannelData ObjSalesChannel = new SalesChannelData())
            {
                ObjSalesChannel.PageIndex = pageno;
                ObjSalesChannel.PageSize = Convert.ToInt32(PageSize);
                ObjSalesChannel.SalesChannelName = txtsaleschannelname.Text.Trim();
                ObjSalesChannel.SalesChannelCode = txtsaleschannelcode.Text.Trim();
                ObjSalesChannel.StatusValue = Convert.ToInt32(ddlactive.SelectedValue);
                ObjSalesChannel.UserID = PageBase.UserId;
                ObjSalesChannel.CompanyId = PageBase.ClientId;
                ObjSalesChannel.BrandId = Convert.ToInt32(ddlBrand.SelectedValue);
                ObjSalesChannel.ProductCategoryId = Convert.ToInt32(ddlproductcategory.SelectedValue);
                    if (cmbsaleschanneltype.SelectedValue != "0")
                    {
                        ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(cmbsaleschanneltype.SelectedValue);
                    }
                    if (cmbBrandName.SelectedValue != "0")
                    {
                        ObjSalesChannel.Brand = Convert.ToInt16(cmbBrandName.SelectedValue);
                    }
                DtSalesChannelDetail = ObjSalesChannel.GetSalesChannelInfo();
                   //if(ObjSalesChannel.TotalRecords>0) #CC08 comented
                if (DtSalesChannelDetail != null && DtSalesChannelDetail.Rows.Count>0)
                   {

                       dvFooter.Visible = true;
                       ViewState["TotalRecords"] = ObjSalesChannel.TotalRecords;
                       ucPagingControl1.TotalRecords = ObjSalesChannel.TotalRecords;
                       ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                       ucPagingControl1.SetCurrentPage = pageno;
                       ucPagingControl1.FillPageInfo();

                        ExportToExcel.Visible = true;
                        GridSalesChannel.Visible = true;
                        GridSalesChannel.DataSource = DtSalesChannelDetail;
                        GridSalesChannel.DataBind();
                        dvhide.Visible = true;
                    }
                    else
                    {
                        dvFooter.Visible = false;
                        HideControls();
                        GridSalesChannel.Visible = false;
                        GridSalesChannel.DataSource = null;
                        GridSalesChannel.DataBind();
                        ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                        dvhide.Visible = false;
                    }
            }
        }
        catch (Exception ex )
        {
            ucMessage1.ShowError(ex.Message);
        }
    }

    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
       // GetSearchData(ucPagingControl1.CurrentPage);
        BindGrid(ucPagingControl1.CurrentPage);

    }
    protected void ExportToExcel_Click2(object sender, EventArgs e)
    {
        try
        {
            string Password = string.Empty;


            using (SalesChannelData ObjSalesChannel = new SalesChannelData())
            {
                ObjSalesChannel.PageIndex = -1;
                ObjSalesChannel.PageSize = Convert.ToInt32(PageSize);
                ObjSalesChannel.SalesChannelName = txtsaleschannelname.Text.Trim();
                ObjSalesChannel.SalesChannelCode = txtsaleschannelcode.Text.Trim();
                ObjSalesChannel.StatusValue = Convert.ToInt32(ddlactive.SelectedValue);
                ObjSalesChannel.CompanyId = PageBase.ClientId;
                ObjSalesChannel.UserID = PageBase.UserId;
                if (cmbsaleschanneltype.SelectedValue != "0")
                {
                    ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(cmbsaleschanneltype.SelectedValue);
                }
                if (ddlBrand.SelectedValue != "0")
                {
                    ObjSalesChannel.BrandId = Convert.ToInt32(cmbBrandName.SelectedValue);
                }
                if(ddlproductcategory.SelectedValue!="0")
                {
                    ObjSalesChannel.ProductCategoryId = Convert.ToInt32(ddlproductcategory.SelectedValue);
                }
                DtSalesChannelDetail = ObjSalesChannel.GetSalesChannelInfo();
                if (ObjSalesChannel.TotalRecords > 0)
                {

                    DataTable dt = DtSalesChannelDetail.Copy();
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            using (Authenticates ObjAuth = new Authenticates())
                            {
                                Password = ObjAuth.DecryptPassword(Convert.ToString(dr["Password"]), Convert.ToString(dr["PasswordSalt"]));
                            };
                            dr["Password"] = Password;
                        }
                        if (dt.Columns.Contains("PasswordSalt"))
                        {
                            dt.Columns.Remove("PasswordSalt");
                            dt.AcceptChanges();
                        }
                        DataSet dtcopy = new DataSet();
                        dtcopy.Merge(dt);
                        dtcopy.Tables[0].AcceptChanges();
                        String FilePath = Server.MapPath("../../");
                        string FilenameToexport = "SalesChannelList";
                        PageBase.RootFilePath = FilePath;
                        PageBase.ExportToExecl(dtcopy, FilenameToexport);

                    }
                    else
                    {
                        ucMessage1.ShowError(Resources.Messages.NoRecord);

                    }
                    // ViewState["Table"] = null;
                    //   }
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
    public void fillBrandCategoryDDL()
    {

        using (ProductData objproduct = new ProductData())
        {

            try
            {
                objproduct.CompanyId = PageBase.ClientId;
                DataTable dtbrandfil = objproduct.SelectAllBrandInfo();
                String[] colArray = { "BrandID", "BrandName" };
                PageBase.DropdownBinding(ref ddlBrand, dtbrandfil, colArray);
                ddlBrand.SelectedValue = "0";

                DataTable dtprodcatfil = objproduct.SelectAllProdCatInfo();
                String[] colArray1 = { "ProductCategoryID", "ProductCategoryName" };
                PageBase.DropdownBinding(ref ddlproductcategory, dtprodcatfil, colArray1);
                ddlproductcategory.SelectedValue = "0";

            }
            catch (Exception ex)
            {
                ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);

            }
        }
    }
    /* #CC07 Add End */

}
