using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;
using System.IO;/*#CC20*/
using Cryptography;
/*Change Log:
 * 07-May-2014, Rakesh Goel, #CC01 - Wrong property was being set in call to CheckRetailerExistence. Same has been fixed.
 * 11-Jul-2014, Rakesh Goel, #CC02 - Viewstate turned off for the dropdown and update panel applied to specific controls
 * instead of whole page.
 * 28-Jul-2014, Rakesh Goel, #CC03 - ShowAll button code not working properly due to saleschannel dropdown now out of update panel and viewstate
 * disabled. Added client side script to handle the same.
 * 27 May 2015, Karam Chand Sharma, #CC04, Use zedcontrol image button and replace button control 
 * 05 Jun 2015, Karam Chand Sharma, #CC05, Pass user id into properties
 * 11 Jun 2015, Karam Chand Sharma, #CC06, Replace sales channel dropdown with usercontrol dropdown
 * 14-Oct-2015, Sumit Maurya, #CC07, New textbox "Retailer Code" and  dropdown "Status" added to supply more filter values.
 * 21-Mar-2016, Sumit Maurya, #CC08, Column "Counter Potential In Size" gets exported according to config value "PotentialVolDisplay".
 * 22-Mar-2016, Sumit Maurya, #CC09, Issue of page getting blocked on the execution of event(s) due to update panel resolved.
 * 23-Mar-2015, Sumit Maurya, #CC10, New check added to add/remove Tehsil data from export to excel according to config.
 * 23-May-2016, Sumit Maurya, #CC11, Nd Name gets displayed in grid. Instead of Image buttton ZedImage button added to change status according to access granted.
 * 07-Jun-2016, Sumit Maurya, #CC12, TinNumber gets dispaly as VATNumber according to config.
 * 14-Jul-2016, Sumit Maurya, #CC13, Incorrect column (Counter Potential in Volume) was getting hide/displayed according to config. It is corrected and correct column (Counter Potential in Value) gets displed/hide.
 * 07-Aug-2016, Sumit Maurya, #CC14, New export added to download "Export Retailer Mapping information and  New filers (State, ND, SalesChannelCode) added to filer data." 
 * 22-Aug-2016, Sumit Maurya, #CC15, Value in parameter supplied to get only Approved retailer.
 * 24-Aug-2016, Sumit Maurya, #CC16, Column "ParentReatilerName " in gridview gets displayed according to config "ShowHideParentRetailerName" of AppConfig.resx. New code added to retain search result when user Search retailer then click on edit icon then on Add retailer interface click on back button or View list link button .
 * 20 Sep 2014, Karam Chand Sharma, #CC17,  Change the label Address 1 and Address 2 to Address Line 1 and Address Line 2 respectively
 * 28 Nov 2017,Vijay Kumar Prajapati,#CC18,Download Retailermapping Info for user wise login.
 * 20 Dec 2017,Vijay Kumar Prajapati,#CC19,User Cap Creation For Comio.
 *  30 May 2018,Rajnish Kumar,#CC20 view Retailer Image path for download.
 *  05-Jun-2018, Sumit Maurya, #CC21,  Header Replacement code added (Done for motorola).
 *    25-Jun-2018, Rajnish Kumar, #CC22, ND drop down is not visible on asm login .
 *    06-July-2018,Vijay Kumar Prajapati,#CC23,Add three column in gridview "Salesman Name","Salesman Mapping Status","SalesChannel Mapping Status" for karbonn mobile when configkey=RetailerMultiplemapping
 *    08-Oct-2018, Sumit Maurya, #CC24, New function called to get data for View Retailer (Done for Karbonn).
 *    22-Oct-2018,Vijay Kumar Prajapati,#CC25, Use GSTNumber instead on VATNumber.
 *    14-Dec-2018, Sumit Maurya, #CC26, Header replacement commented as it is not required now from code it is done by database (done for Inovu). 
 * 
 */


public partial class Masters_HO_Retailer_ViewRetailer : PageBase
{
    DataTable Dt = new DataTable();
    DataTable dtApprovalDetails;/*#CC20*/
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.ShowControl = false;
            if (!IsPostBack)
            {
                HideControls();
                /* #CC06 COMMENTED FillsalesChannel();*/
                if (Convert.ToInt32(PageBase.SalesChanelID) != 0)             //this will select the login user in combo
                {
                    /* #CC06 COMMENTED cmbsaleschannel.Items.FindByValue(Convert.ToString(PageBase.SalesChanelID)).Selected = true;*/
                    //btnShowAll.Visible = false;
                    //cmbsaleschannel.Enabled = false;
                }
                else
                {
                    /*  btnShowAll.Visible = true; #CC14 Commented */
                }
                /* btnShowAll.Visible = true;  #CC14 Commented */
                ViewState["TotalRecords"] = 0;
                /* #CC14 Add Start */
                BindState();
                fillBrandCategoryDDL();
                imgRetailerDetail.ImageUrl = "~/" + strAssets + "/CSS/Images/page_excel.png";
                imgDownloadMappingInfo.ImageUrl = "~/" + strAssets + "/CSS/Images/page_excel.png";
                BindND();
                /* #CC14 Add End */
                ShowHideSearchGridColumns(); /* #CC15 Added */
                /* #CC16 Add Start */
                if (Session["SearchSessionStart"] == null)
                {
                    Session["SearchSessionStart"] = 0;
                }
                /* #CC16 Add End */
                if (PageBase.BaseEntityTypeID == 2 && PageBase.SalesChannelLevel == 1)
                {
                    DwnldRetailerMappingInfo.Visible = true;
                    imgDownloadMappingInfo.Visible = true;
                }
                else
                {
                    DwnldRetailerMappingInfo.Visible = false;
                    imgDownloadMappingInfo.Visible = false;
                }
                /*#CC22*/if(BaseEntityTypeID==2 && SalesChannelLevel==4)
                {
                    ddlND.Visible = false;
                    lblNDHeading.Visible = false;
                }
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }

    }
    /* #CC16 Add Start */
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Session["SearchSessionStart"] != null)
            if (Convert.ToInt32(Session["SearchSessionStart"]) == 2)
            {
                RetailSearch();
            }
    }
    /* #CC16 Add End */

    /* #CC06 COMMENTED START void FillsalesChannel()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
            ObjSalesChannel.SalesChannelID = PageBase.SalesChanelID;
            ObjSalesChannel.BilltoRetailer = true;

            string[] str = { "SalesChannelID", "DisplayName" };
            //PageBase.DropdownBinding(ref cmbsaleschannel, ObjSalesChannel.GetSalesChannelInfo(), str);
            PageBase.DropdownBinding(ref cmbsaleschannel, ObjSalesChannel.GetSalesChannelInfoV2(), str);
        };
    } #CC06 COMMENTED END*/
    void HideControls()
    {
        /* ExportToExcel.Visible = false; #CC14 Commented*/

    }
    protected void LBAddRetailer_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageRetailer.aspx");
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            //if (cmbsaleschannel.SelectedValue == "0" && txtRetailername.Text == "")  //#CC03 commented
            /*#CC06 COMMENTED if (Convert.ToInt32(Request.Form[cmbsaleschannel.UniqueID]) == 0 && txtRetailername.Text == "")  //#CC03 added*/
            if (Convert.ToInt32(ucServiceEntity.SelectedValue) == 0 && txtRetailername.Text == "" && Convert.ToInt32(ddlStatus.SelectedValue) == -1 && txtRetailerCode.Text == "" && Convert.ToInt32(ddlState.SelectedValue) == 0 && Convert.ToInt32(ddlND.SelectedValue) == 0 && txtSalesChannelCode.Text.Trim() == "")  /*#CC06 AddedControl*/ /* #CC07 New Checks added */ /* #CC14 New checks added and previous status check modified (-1 means excluding status check ) */
            {
                ucMessage1.ShowInfo("Please enter atleast one search parameter!");
                return;
            }
            else
            {
                Session["SearchSessionStart"] = 1;
                //FillGrid();
                GetSearchData(1);

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        /* #CC16 Add Start */
        if (Session["SearchSessionStart"] != null)
        {
            Session["SearchSessionStart"] = null;
        }
        /* #CC16 Add End */
        /*#CC06 COMMENTED START ucMessage1.ShowControl = false;
        txtRetailername.Text = "";
        dvhide.Visible = false;#CC06 COMMENTED END*/
        Response.Redirect("ViewRetailer.aspx");
        /*#CC03 comment start
        cmbsaleschannel.ClearSelection();  
        if (Convert.ToInt32(PageBase.SalesChanelID) != 0)             //this will select the login user in combo
        {
            cmbsaleschannel.Items.FindByValue(Convert.ToString(PageBase.SalesChanelID)).Selected = true;

            //cmbsaleschannel.Enabled = false;
        }
        else
        {

            cmbsaleschannel.SelectedValue = "0";
            cmbsaleschannel.Enabled = true;
        }
        #CC03 comment end*/

    }
    #region Not In Use
    /*void FillGrid()
    {
        //DataTable Dt = new DataTable();
        using (RetailerData ObjRetailer = new RetailerData())
        {
            ObjRetailer.RetailerName = txtRetailername.Text.Trim();


            if (cmbsaleschannel.SelectedValue != "0")
            {
                ObjRetailer.SalesChannelID = Convert.ToInt32(cmbsaleschannel.SelectedValue);
            }
            ObjRetailer.LoggedInSalesChannelid = PageBase.SalesChanelID;
            Dt = ObjRetailer.GetRetailerInfoV2();
        };
        if (Dt != null && Dt.Rows.Count > 0)
        {
            //ViewState["Table"] = Dt;      //Pankaj Dhingra
            ExportToExcel.Visible = true;
            GridRetailer.Visible = true;
            GridRetailer.DataSource = Dt;
            GridRetailer.DataBind();
            dvhide.Visible = true;
        }
        else
        {
            dvhide.Visible = false;
            HideControls();
            GridRetailer.DataSource = null;
            GridRetailer.DataBind();
            ucMessage1.ShowInfo(Resources.Messages.NoRecord);
        }
        //UpdGrid.Update();
    }
    protected void GridRetailer_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridRetailer.PageIndex = e.NewPageIndex;
        FillGrid();
    }*/
    #endregion
    protected void GridRetailer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int CheckResult = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                /*#CC23 Added Started*/
                if(Convert.ToString(HttpContext.Current.Session["RetailerMultipleMapping"])=="1")
                {
                    GridRetailer.Columns[10].Visible=true;
                    GridRetailer.Columns[11].Visible = true;
                    GridRetailer.Columns[12].Visible = true;
                }
                else
                {
                    GridRetailer.Columns[10].Visible = false;
                    GridRetailer.Columns[11].Visible = false;
                    GridRetailer.Columns[12].Visible = false;
                }
                /*#CC23 Added End*/
                Int32 RetailerID = Convert.ToInt32(GridRetailer.DataKeys[e.Row.RowIndex].Value);
                /*using (RetailerData ObjRetailer = new RetailerData())
                {
                    //ObjRetailer.SalesChannelID = RetailerID;  #CC01 commented
                    ObjRetailer.RetailerID = RetailerID;   //#CC01 added
                 * 
                    CheckResult = ObjRetailer.CheckRetailerExistence();
                };*/
                /*#CC20 start*/
                if (RetailerID > 0)
                {
                    using (RetailerData objdetail = new RetailerData())
                    {
                        objdetail.UserID = PageBase.UserId;
                        objdetail.RetailerID = RetailerID;
                        dtApprovalDetails = objdetail.GetRetailerViewImageInfo();

                    }
                    DataRow[] drv = dtApprovalDetails.Select("RetailerId=" + RetailerID);
                    if (drv.Length > 0)
                    {
                        DataTable dtTemp = dtApprovalDetails.Clone();

                        for (int cntr = 0; cntr < drv.Length; cntr++)
                        {
                            dtTemp.ImportRow(drv[cntr]);
                        }

                        GridView gvAttachedImages = (GridView)e.Row.FindControl("gvAttachedImages");
                        gvAttachedImages.DataSource = dtTemp;
                        gvAttachedImages.DataBind();
                    }
                }
                /*#CC20 end*/
                CheckResult = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ISRetailerISPMapping"));

                GridViewRow GVR = e.Row;

                ImageButton btnStatus = (ImageButton)GVR.FindControl("imgActive");
                Label lblParentSalesChannelId = (Label)GVR.FindControl("lblParentSalesChannelId");
                Label lblStatus = (Label)GVR.FindControl("lblStatus");
               // ImageButton imgEdit = (ImageButton)GVR.FindControl("img1");/*#CC18 Added*/
                 ZedControlLib.ZedImageButton imgEdit = (ZedControlLib.ZedImageButton)GVR.FindControl("img1");/*#CC04 ADDED*//*#CC18 Commented*/
                if (lblStatus.Text == "True")
                {
                    if (PageBase.SalesChanelID == 0)
                    {
                        imgEdit.Visible = true;
                    }
                    else
                    {
                        if (PageBase.SalesChanelID == Convert.ToInt32(lblParentSalesChannelId.Text))
                        {
                            imgEdit.Visible = true;
                        }
                        else
                        {
                            imgEdit.Visible = false;
                        }
                    }
                }
                else
                {
                    imgEdit.Visible = false;
                }

                HyperLink HLDetails = default(HyperLink);
                HLDetails = (HyperLink)GVR.FindControl("HLDetails");
                string strViewDBranchDtlURL = null;

                //strViewDBranchDtlURL = "ViewRetailerDetail.aspx?RetailerId=" + Crypto.Encrypt(Convert.ToString(RetailerID), PageBase.KeyStr);

                strViewDBranchDtlURL = Server.UrlEncode(Crypto.Encrypt(Convert.ToString(RetailerID), PageBase.KeyStr)).ToString().Replace("+", " ");
                {
                    HLDetails.Text = "Details";
                    //HLDetails.NavigateUrl = "#";
                    //HLDetails.Attributes.Add("OnClick", "popup('" + strViewDBranchDtlURL + "')");
                    HLDetails.Attributes.Add("onClick", string.Format("return popup('" + strViewDBranchDtlURL + "')"));
                }
                Label lblPassword = (Label)GVR.FindControl("lblPassword");
                if (lblPassword.Text != "")
                {
                    LinkButton hlPassword = default(LinkButton);
                    hlPassword = (LinkButton)GVR.FindControl("hlPassword");
                    hlPassword.Visible = true;
                    string strPassword = null;

                    Label lblPasswordSalt = (Label)GVR.FindControl("lblPasswordSalt");
                    strPassword = fncChangePwd(lblPassword.Text, lblPasswordSalt.Text);
                    hlPassword.Attributes.Add("Onclick", "javascript:alert('Password is : " + strPassword + "');{return false;}");

                }


                if (CheckResult > 0)
                {


                    if (btnStatus != null)
                    {
                        btnStatus.Attributes.Add("Onclick", "javascript:alert('This retailer is linked to existing data.You can not deactivate it.');{return false;}");

                    }
                }

                if (PageBase.SalesChanelID != 0)
                {
                }
                else
                {

                }

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }


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
    protected void GridRetailer_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int Result = 0;
        Int32 RetailerId = Convert.ToInt32(e.CommandArgument);
        try
        {

            if (e.CommandName == "Active")
            {

                if (RetailerId > 0)
                {
                    using (RetailerData ObjRetailer = new RetailerData())
                    {

                        ObjRetailer.RetailerID = RetailerId;
                        Result = ObjRetailer.UpdateStatusRetailerInfo();
                        UpdGrid.Update();
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
                    ucPagingControl1.FillPageInfo();
                    GetSearchData(ucPagingControl1.CurrentPage);
                }
            }

        }
        catch (Exception ex)
        {
            /*#CC19 Added Started*/
            if (ex.Message.ToLower().Contains("trgusercount"))
            {
                ucMessage1.ShowError("Active user count is exceeding the limit defined. Please Contact administrator.");
            }
            else /*#CC19 Added End*/
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
        if (e.CommandName == "cmdEdit")
        {
            Session["SearchSessionStart"] = 2; /* #CC17 Added */
            Response.Redirect("ManageRetailer.aspx?RetailerId=" + Server.UrlEncode(Crypto.Encrypt(Convert.ToString(RetailerId), PageBase.KeyStr)));
        }

    }
    #region Not in use
    /*protected void ExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {

            //if (ViewState["Table"] != null)
            //{
            // DataTable dt = (DataTable)ViewState["Table"];        //Pankaj Dhingra
            FillGrid();
            DataTable dtExp = Dt.Copy();
            string[] DsCol = new string[] { "RetailerName", "RetailerCode", "SalesChannelName", "OrgnhierarchyID", "SalesmanName", "ContactPerson", "Address1", "Address2", "StateName", "CityName", "DistrictName", "AreaName", "PinCode", "TinNumber", "MobileNumber", "StatusValue", "PhoneNumber", "Email", "ISPOnCounter", "CounterSize" };
            DataTable DsCopy = new DataTable();
            dtExp = dtExp.DefaultView.ToTable(true, DsCol);
            dtExp.Columns["StatusValue"].ColumnName = "Status";
            if (dtExp.Rows.Count > 0)
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dtExp);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "RetailerList";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dtcopy, FilenameToexport);
                //ViewState["Table"] = null;
            }
            else
            {
                ucMessage1.ShowError(Resources.Messages.NoRecord);

            }
            //ViewState["Table"] = null;
            //}
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }


    }*/
    #endregion
    protected void ExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            using (RetailerData ObjRetailer = new RetailerData())
            {
                ObjRetailer.RetailerName = txtRetailername.Text.Trim();
                /*#CC06 COMMENTED if (cmbsaleschannel.SelectedValue != "0")*/
                if (ucServiceEntity.SelectedValue != 0)/*#CC06 ADDED*/
                {
                    //ObjRetailer.SalesChannelID = Convert.ToInt32(cmbsaleschannel.SelectedValue); //#CC02 commented
                    /*#CC06 COMMENTED ObjRetailer.SalesChannelID = Convert.ToInt32(Request.Form[cmbsaleschannel.UniqueID]);   //#CC02 added*/
                    ObjRetailer.SalesChannelID = Convert.ToInt32(ucServiceEntity.SelectedValue);  /*#CC06 ADDED*/
                }
                ObjRetailer.LoggedInSalesChannelid = PageBase.SalesChanelID;
                ObjRetailer.PageIndex = -1;
                ObjRetailer.PageSize = Convert.ToInt32(ViewState["TotalRecords"]);
                ObjRetailer.UserID = PageBase.UserId;
                /* #CC07 Add Start */
                ObjRetailer.RetailerCode = txtRetailerCode.Text.Trim();
                ObjRetailer.intStatus = Convert.ToInt16(ddlStatus.SelectedValue);
                /* #CC07 Add End */

                ObjRetailer.RetailerApproval = 1; /* #CC15 Added */

                /* #CC14 Add Start */
                ObjRetailer.StateID = Convert.ToInt16(ddlState.SelectedValue);
                ObjRetailer.NDID = Convert.ToInt32(ddlND.SelectedValue);
                ObjRetailer.SalesChannelCode = txtSalesChannelCode.Text.Trim();
                /* #CC14 Add End */
                /* #CC24 Comment Start
                DataTable dt = ObjRetailer.GetRetailerInfoV3();
                DataTable dtExp = dt.Copy();
             #CC24 Comment End   */
                /* #CC24 Add Start */
                DataSet dsData = ObjRetailer.GetRetailerViewInfo();
                DataTable dtExp = new DataTable();
                if (ObjRetailer.TotalRecords > 0)
                {
                    dtExp = dsData.Tables[0].Copy();
                }
                else if (ObjRetailer.intOutParam == 2)
                {
                    dvhide.Visible = false;
                    dvFooter.Visible = false;
                    HideControls();
                    GridRetailer.DataSource = null;
                    GridRetailer.DataBind();
                    ucMessage1.ShowInfo(ObjRetailer.Error);
                }
                /* #CC24 Add End */
                /* #CC12 Add Start */
                if (PageBase.ChangeTinLabel == 1)
                {
                    if (dtExp.Columns.Contains("TinNumber"))
                        //dtExp.Columns["TinNumber"].ColumnName = "VATNnumber:";/*#CC25 Commented*/
                        dtExp.Columns["TinNumber"].ColumnName = "GST Nnumber";/*#CC25 Added*/
                }
                /* #CC12 Add End */
                //string[] DsCol = new string[] { "RetailerName", "RetailerCode", "SalesChannelName", "OrgnhierarchyID", "SalesmanName", "ContactPerson", "Address1", "Address2", "StateName", "CityName", "DistrictName", "AreaName", "PinCode", "TinNumber", "MobileNumber", "StatusValue", "PhoneNumber", "Email", "ISPOnCounter", "CounterSize" };
                //DataTable DsCopy = new DataTable();
                //dtExp = dtExp.DefaultView.ToTable(true, DsCol);
                //dtExp.Columns["StatusValue"].ColumnName = "Status";

                /*dtExp.Columns.Remove("StatusValue");
                dtExp.Columns.Remove("Retailer");
                dtExp.Columns.Remove("LoginName");
                dtExp.Columns.Remove("Password");
                dtExp.Columns.Remove("ParentRetailerName");
                dtExp.Columns.Remove("PasswordSalt");
                dtExp.Columns.Remove("LocationName");
                dtExp.Columns.Remove("IsOpeningStockEnteredForRetailer");
                dtExp.Columns.Remove("OpeningStockDate");
                dtExp.Columns.Remove("OpeningStockEntryDate");
                dtExp.Columns.Remove("ISRetailerISPMapping");
                dtExp.Columns.Remove("RetailerID");
                dtExp.Columns.Remove("SalesChannelID");*/

                /* #CC08 Add Start */
                if (Convert.ToInt32(Session["PotentialVolDisplay"]) != 1)
                {
                    /* #CC13 Comment Start if (dtExp.Columns.Contains("Counter Potential in Volume"))
                     {
                         dtExp.Columns.Remove("Counter Potential in Volume");
                     }
                     #CC13 Comment End */
                    /* #CC13 Add Start */
                    if (dtExp.Columns.Contains("Counter Potential in Value"))
                    {
                        dtExp.Columns.Remove("Counter Potential in Value");
                    }
                    /* #CC13 Add End */

                }
                /* #CC08 Add End */


                /* #CC10 Add Start */
                if (Convert.ToInt32(Session["TehsillDisplayMode"]) != 1)
                {
                    if (dtExp.Columns.Contains("TehsilName"))
                    {
                        dtExp.Columns.Remove("TehsilName");
                    }
                }
                /* #CC10 Add End */

                /*#CC17 START ADDED*/
                if (dtExp.Columns.Contains("Address1"))
                    dtExp.Columns["Address1"].ColumnName = "Address Line 1";
                if (dtExp.Columns.Contains("Address2"))
                    dtExp.Columns["Address2"].ColumnName = "Address Line 2";
                /*#CC17 START ADDED*/
                if (dtExp.Rows.Count > 0)
                {
                    DataSet dtcopy = new DataSet();
                    dtcopy.Merge(dtExp);
                    dtcopy.Tables[0].AcceptChanges();
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "RetailerList";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dtcopy, FilenameToexport);
                    //ViewState["Table"] = null;
                }
                else
                {
                    ucMessage1.ShowError(Resources.Messages.NoRecord);

                }
            };
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        try
        {

            //if (Convert.ToInt32(PageBase.SalesChanelID) != 0)             //this will select the login user in combo
            //{
            //    cmbsaleschannel.Items.FindByValue(Convert.ToString(PageBase.SalesChanelID)).Selected = true;
            //    cmbsaleschannel.Enabled = false;
            //}
            //else
            //{
            //    cmbsaleschannel.SelectedValue = "0";
            //    cmbsaleschannel.Enabled = true;
            //}
            txtRetailername.Text = "";
            ((DropDownList)ucServiceEntity.FindControl("ddlServiceEntity")).SelectedIndex = 0;/*#CC06 ADDED*/
            /*#CC06 COMMENTED START cmbsaleschannel.ClearSelection();
            cmbsaleschannel.SelectedValue = "0";
            cmbsaleschannel.Enabled = true;
            #CC06 COMMENTED END*/
            //FillGrid();
            Clearfields();/* #CC14 Added */
            GetSearchData(1);
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        GetSearchData(ucPagingControl1.CurrentPage);

    }
    public void GetSearchData(int pageno)
    {
        /* #CC16 Add Start */
        if (Session["SearchSessionStart"] != null)
            if (Convert.ToInt32(Session["SearchSessionStart"]) == 1)
                SearchSession();
        /* #CC16 Add End */
        ViewState["TotalRecords"] = 0;
        using (RetailerData ObjRetailer = new RetailerData())
        {
            ObjRetailer.RetailerName = txtRetailername.Text.Trim();
            //if (cmbsaleschannel.SelectedValue != "0")  //#CC03 commented

            {
                //ObjRetailer.SalesChannelID = Convert.ToInt32(cmbsaleschannel.SelectedValue); //#CC02 commented
                /*#CC06 COMMENTED ObjRetailer.SalesChannelID = Convert.ToInt32(Request.Form[cmbsaleschannel.UniqueID]);   //#CC02 added*/
                ObjRetailer.SalesChannelID = Convert.ToInt32(ucServiceEntity.SelectedValue);
            }

            ObjRetailer.LoggedInSalesChannelid = PageBase.SalesChanelID;

            ObjRetailer.PageIndex = pageno;
            ObjRetailer.PageSize = Convert.ToInt32(PageSize);
            ObjRetailer.UserID = PageBase.UserId;/*#CC05 ADDED*/
            /* #CC07 Add Start */
            ObjRetailer.RetailerCode = txtRetailerCode.Text.Trim();
            ObjRetailer.intStatus = Convert.ToInt16(ddlStatus.SelectedValue);
            /* #CC07 Add End */
            ObjRetailer.DisplayMode = 1;
            ObjRetailer.RetailerApproval = 1; /* #CC15 Added */
            /* #CC14 Add Start */
            ObjRetailer.StateID = Convert.ToInt16(ddlState.SelectedValue);
            ObjRetailer.NDID = Convert.ToInt32(ddlND.SelectedValue);
            ObjRetailer.SalesChannelCode = txtSalesChannelCode.Text.Trim();
            ObjRetailer.BrandId = Convert.ToInt32(ddlBrand.SelectedValue);
            ObjRetailer.ProductCategoryId = Convert.ToInt32(ddlproductcategory.SelectedValue);
            /* #CC14 Add End */
            /* #CC24 Comment Start
             * Dt = ObjRetailer.GetRetailerInfoV3();

             if (Dt != null && Dt.Rows.Count > 0)
             {
                 ExportToExcel.Visible = true;
                 GridRetailer.Visible = true;
                 dvFooter.Visible = true;
                 ViewState["TotalRecords"] = ObjRetailer.TotalRecords;
                 ucPagingControl1.TotalRecords = ObjRetailer.TotalRecords;
                 ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                 ucPagingControl1.SetCurrentPage = pageno;
                 ucPagingControl1.FillPageInfo();
                 GridRetailer.DataSource = Dt;
                 GridRetailer.DataBind();
                 dvhide.Visible = true;
                 UpdGrid.Update();

             }  #CC24 Comment End */

            /* #CC24 Add Start */
            DataSet dsData = ObjRetailer.GetRetailerViewInfo();

            if (ObjRetailer.TotalRecords>0)
            {
                ExportToExcel.Visible = true;
                GridRetailer.Visible = true;
                dvFooter.Visible = true;
                ViewState["TotalRecords"] = ObjRetailer.TotalRecords;
                ucPagingControl1.TotalRecords = ObjRetailer.TotalRecords;
                ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                ucPagingControl1.SetCurrentPage = pageno;
                ucPagingControl1.FillPageInfo();
                GridRetailer.DataSource = dsData.Tables[0];
                GridRetailer.DataBind();
                dvhide.Visible = true;
                UpdGrid.Update();
            }
            else if (ObjRetailer.intOutParam==2)
                {
                    dvhide.Visible = false;
                    dvFooter.Visible = false;
                    HideControls();
                    GridRetailer.DataSource = null;
                    GridRetailer.DataBind();
                    ucMessage1.ShowInfo(ObjRetailer.Error);
                }
            /* #CC24 Add End */

            else
            {
                dvhide.Visible = false;
                dvFooter.Visible = false;
                HideControls();
                GridRetailer.DataSource = null;
                GridRetailer.DataBind();
                ucMessage1.ShowInfo(Resources.Messages.NoRecord);
            }
        };
        /* #CC16 Add Start */
        if (Session["SearchSessionStart"] != null && Session["DtSearchParameters"] != null)
        {
            ((DataTable)Session["DtSearchParameters"]).Rows[7]["SearchParameterValue"] = pageno;
        }
        Session["SearchSessionStart"] = 1;
        /* #CC016 Add End */
    }
    /* #CC14 Add Start */
    protected void DwnldRetailerMappingInfo_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsReferenceCode = new DataSet();

            /*using (RetailerData ObjRetailer = new RetailerData())*/
            using (RetailerData ObjRetailer = new RetailerData())
            {
                ObjRetailer.RetailerName = txtRetailername.Text.Trim();
                ObjRetailer.RetailerCode = txtRetailerCode.Text.Trim();
                ObjRetailer.intStatus = Convert.ToInt16(ddlStatus.SelectedValue);
                ObjRetailer.StateID = Convert.ToInt16(ddlState.SelectedValue);
                ObjRetailer.NDID = Convert.ToInt32(ddlND.SelectedValue);
                ObjRetailer.RDSID = Convert.ToInt32(ucServiceEntity.SelectedValue);
                ObjRetailer.SalesChannelCode = txtSalesChannelCode.Text.Trim();
                ObjRetailer.UserID = PageBase.UserId;/*#CC18 Added*/
                dsReferenceCode = ObjRetailer.GetRetailerMappingInfo();
               
                if (ObjRetailer.TotalRecords > 0)
                {
                    /* #CC21 Add Start */
                    /* #CC26 Comment Start
                    using (ReportData objRD = new ReportData())
                    {
                        objRD.headerReplacement(dsReferenceCode.Tables[0]);
                    }  #CC26 Comment End*/
                    /* #CC21 Add End */
                    
                    /*String FilePath = Server.MapPath("../../../");*/
                    String FilePath = Server.MapPath("~/");
                    string FilenameToexport = "Retailer Mapping Info";
                    PageBase.RootFilePath = FilePath;
                    // dsReferenceCode.Tables[0].TableName = "FOSList";                   
                    PageBase.ExportToExecl(dsReferenceCode, FilenameToexport, EnumData.eTemplateCount.ePrimarysales1 + 1);
                }
                else
                {
                    ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                }
            }


        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }

    public void BindState()
    {
        ddlState.Items.Clear();
        using (GeographyData objGeog = new GeographyData())
        {
            DataTable dt = new DataTable();
            objGeog.countryid = 0;
            objGeog.CompanyId = PageBase.ClientId;
            objGeog.UserID = PageBase.UserId;

            dt = objGeog.GetAllActiveStates();
            String[] colArray = { "StateID", "StateName" };
            PageBase.DropdownBinding(ref ddlState, dt, colArray);
        }

    }

    void BindND()
    {
        try
        {
            ddlND.Items.Clear();
            
            using (SalesChannelData ObjSalesChannel = new SalesChannelData())
            {
                DataTable dt = new DataTable();
                ObjSalesChannel.SalesChannelTypeID =0;
                ObjSalesChannel.ActiveStatus = 255;
                ObjSalesChannel.GetND = 1;// PageBase.SalesChanelTypeID == 7 ? 0 : 1;
                ObjSalesChannel.BaseEntityTypeID = Convert.ToInt16(PageBase.BaseEntityTypeID);
                ObjSalesChannel.UserID = PageBase.UserId;
                ObjSalesChannel.CompanyId = PageBase.ClientId;
                /*dt = ObjSalesChannel.GetSalesChannelListWithRetailer();*/
                dt = ObjSalesChannel.GetSalesChannelListWithRetailerV2();
                if (dt != null)
                    if (dt.Rows.Count > 0)
                    {
                        string[] str = { "SalesChannelid", "SalesChannelName" };
                        PageBase.DropdownBinding(ref ddlND, dt, str);
                        if (PageBase.SalesChanelID > 0 || PageBase.BaseEntityTypeID == 3)
                        {
                            ddlND.SelectedValue = PageBase.SalesChannelLevel == 2 ? Convert.ToString(dt.Rows[0]["SalesChannelID"]) : Convert.ToString(PageBase.SalesChanelID);
                            ddlND.Enabled = false;

                        }
                    }
                   else
                    {
                        ddlND.Items.Insert(0, new ListItem("Select", "0"));
                    }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }

    public void Clearfields()
    {
        try
        {
            ddlND.SelectedValue = "0";
            ddlState.SelectedValue = "0";
            txtSalesChannelCode.Text = "";
            txtRetailername.Text = "";
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    /* #CC14 Add End */
    /* #CC15 Add Start */
    public void ShowHideSearchGridColumns()
    {
        try
        {
            int intShowHideParetRetailerName = Convert.ToInt16(Resources.AppConfig.ShowHideParentRetailerName.ToString());
            if (intShowHideParetRetailerName == 0)
            {
                GridRetailer.Columns[2].Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }

    }


    /* #CC15 Add End */
    /* #CC16 Add Start */
    public void SearchSession()
    {
        try
        {

            /* Check session search */
            if (Session["SearchSessionStart"] != null)
            {
                Session["DtSearchParameters"] = null;
                Session["DtSearchParameters"] = dtSearchSession();
                DataRow drSearch = ((DataTable)Session["DtSearchParameters"]).NewRow();
                drSearch["SearchParameterName"] = "SalesChannel";
                drSearch["SearchParameterValue"] = Convert.ToInt32(ucServiceEntity.SelectedValue);
                ((DataTable)Session["DtSearchParameters"]).Rows.Add(drSearch);
                ((DataTable)Session["DtSearchParameters"]).AcceptChanges();

                DataRow drSearch2 = ((DataTable)Session["DtSearchParameters"]).NewRow();
                drSearch2["SearchParameterName"] = "Retailer Name";
                drSearch2["SearchParameterValue"] = txtRetailername.Text.Trim();
                ((DataTable)Session["DtSearchParameters"]).Rows.Add(drSearch2);
                ((DataTable)Session["DtSearchParameters"]).AcceptChanges();

                DataRow drSearch3 = ((DataTable)Session["DtSearchParameters"]).NewRow();
                drSearch3["SearchParameterName"] = "Retailer Code";
                drSearch3["SearchParameterValue"] = txtRetailerCode.Text.Trim();
                ((DataTable)Session["DtSearchParameters"]).Rows.Add(drSearch3);
                ((DataTable)Session["DtSearchParameters"]).AcceptChanges();

                DataRow drSearch4 = ((DataTable)Session["DtSearchParameters"]).NewRow();
                drSearch4["SearchParameterName"] = "Status";
                drSearch4["SearchParameterValue"] = Convert.ToString(ddlStatus.SelectedValue);
                ((DataTable)Session["DtSearchParameters"]).Rows.Add(drSearch4);
                ((DataTable)Session["DtSearchParameters"]).AcceptChanges();

                DataRow drSearch5 = ((DataTable)Session["DtSearchParameters"]).NewRow();
                drSearch5["SearchParameterName"] = "State";
                drSearch5["SearchParameterValue"] = Convert.ToString(ddlState.SelectedValue);
                ((DataTable)Session["DtSearchParameters"]).Rows.Add(drSearch5);
                ((DataTable)Session["DtSearchParameters"]).AcceptChanges();

                DataRow drSearch6 = ((DataTable)Session["DtSearchParameters"]).NewRow();
                drSearch6["SearchParameterName"] = "ND";
                drSearch6["SearchParameterValue"] = Convert.ToString(ddlND.SelectedValue);
                ((DataTable)Session["DtSearchParameters"]).Rows.Add(drSearch6);
                ((DataTable)Session["DtSearchParameters"]).AcceptChanges();

                DataRow drSearch7 = ((DataTable)Session["DtSearchParameters"]).NewRow();
                drSearch7["SearchParameterName"] = "SalesChannel Code";
                drSearch7["SearchParameterValue"] = txtSalesChannelCode.Text.Trim();
                ((DataTable)Session["DtSearchParameters"]).Rows.Add(drSearch7);
                ((DataTable)Session["DtSearchParameters"]).AcceptChanges();

                DataRow drSearch8 = ((DataTable)Session["DtSearchParameters"]).NewRow();
                drSearch8["SearchParameterName"] = "PageNo";
                drSearch8["SearchParameterValue"] = "";
                ((DataTable)Session["DtSearchParameters"]).Rows.Add(drSearch8);
                ((DataTable)Session["DtSearchParameters"]).AcceptChanges();
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }

    }
    public DataTable dtSearchSession()
    {
        DataTable dtSession = new DataTable();
        DataColumn dc = new DataColumn("SRNO");
        dc.DataType = System.Type.GetType("System.Int32");
        dc.AutoIncrement = true;
        dc.AutoIncrementSeed = 1;
        dtSession.Columns.Add(dc);
        dtSession.Columns.Add("SearchParameterName", typeof(string));
        dtSession.Columns.Add("SearchParameterValue", typeof(string));
        return dtSession;
    }

    public void RetailSearch()
    {
        try
        {
            int intPageNo = 0;
            DataTable dtSearch = new DataTable();
            if (Session["DtSearchParameters"] != null)
                if (((DataTable)Session["DtSearchParameters"]).Rows.Count > 0)
                {
                    dtSearch = ((DataTable)Session["DtSearchParameters"]);
                    ucServiceEntity.SelectedValue = Convert.ToInt32(dtSearch.Rows[0]["SearchParameterValue"]);
                    txtRetailername.Text = Convert.ToString(dtSearch.Rows[1]["SearchParameterValue"]);
                    txtRetailerCode.Text = Convert.ToString(dtSearch.Rows[2]["SearchParameterValue"]);
                    ddlStatus.SelectedValue = Convert.ToString(dtSearch.Rows[3]["SearchParameterValue"]);
                    ddlState.SelectedValue = Convert.ToString(dtSearch.Rows[4]["SearchParameterValue"]);
                    ddlND.SelectedValue = Convert.ToString(dtSearch.Rows[5]["SearchParameterValue"]);
                    txtSalesChannelCode.Text = Convert.ToString(dtSearch.Rows[6]["SearchParameterValue"]);
                    intPageNo = Convert.ToInt32(dtSearch.Rows[7]["SearchParameterValue"]);

                    GetSearchData(intPageNo);
                }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    /* #CC16 Add End */

    /*#CC20 start*/
    protected void DownloadFile(object sender, EventArgs e)
    {

        try
        {
            string filePath = (sender as LinkButton).CommandArgument;
            //Response.ContentType = ContentType;
            Response.ContentType="image/jpg";

            //Response.AppendHeader("Content-Disposition", "attachment; filename=" +siteURL+ filePath.Replace("../","/"));
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            //Response.AddHeader("Content-Disposition", "attachment;filename=\"" + filePath + "\"");
            Response.TransmitFile(Server.MapPath(filePath));
            //Response.WriteFile(siteURL + filePath);
            Response.End();
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
        }

    } /*#CC20 end*/
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
}
