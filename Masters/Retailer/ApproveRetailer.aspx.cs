/*
============================================================================================================================================
Copyright	: Zed-Axis Technologies, 2015
Created By	: Sumit Maurya
Create date	: 14-Oct-2015
Description	: This interface is copy of ViewRetailer.aspx. This will display recods similar to ViewRetailer but who are not approved.
Module      : 
============================================================================================================================================
Change Log:
dd-MMM-yy, Name , #CCxx - Description
21-Oct-2015, Sumit Maurya, #CC01, Approval/Rejection Remarks added in gridview to show Remarks.
16-Nov-2015, Sumit Maurya, #CC02, Approval status displayed.
29-Dec-2015, Sumit Maurya, #CC03, Edit button displayed when retailer is pending.
04-Jan-2016, Sumit Maurya, #CC04, Edit Button visibility condition changed.
21-Mar-2016, Sumit Maurya, #CC05, Column "Counter Potential In Size" gets exported according to config value "PotentialVolDisplay".
22-Mar-2016, Sumit Maurya, #CC06, Issue of page getting blocked on the execution of event(s) due to update panel resolved.
23-Mar-2015, Sumit Maurya, #CC10, New check added to add/remove Tehsil data from export to excel according to config.
23-May-2016, Sumit Maurya, #CC11, Nd Name gets displayed in grid.
07-Jun-2016, Sumit Maurya, #CC12, TinNumber gets dispaly as VATNumber according to config.
 * 14-Jul-2016, Sumit Maurya, #CC13, Incorrect column (Counter Potential in Volume) was getting hide/displayed according to config. It is corrected and correct column (Counter Potential in Value) gets displed/hide.
 * 20 Sep 2014, Karam Chand Sharma, #CC14,  Change the label Address 1 and Address 2 to Address Line 1 and Address Line 2 respectively
 * 10-Aug-2017, Sumit Maurya, #CC15, Column names changed in Export to excel.(Done for Comio)
 * 02-Apr-2018, Sumit Maurya, #CC16, Parameter value provided to get data for approval (done for V5).
 * 22-Aug-2018, Sumit Maurya, #CC17, New columns displayed to provide details according to approval data by user (Done for Karbonn)
 * 04-Oct-2018, Sumit Maurya, #CC18, Approve/reject icon binding columnname is changed (Done for Karbon).
 * 10-Oct-2018, Sumit Maurya, #CC19, new function called to get approved data (Done for Karbonn). 
 * 24-Oct-2018, Sumit Maurya, #CC20, Value in property provided to get data accordingly (Done for Motorola).
--------------------------------------------------------------------------------------------------------------------------------------------

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
using Cryptography;



public partial class Masters_HO_Retailer_ApproveRetailer : PageBase
{
    DataTable Dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.ShowControl = false;
            if (!IsPostBack)
            {
                HideControls();

                if (Convert.ToInt32(PageBase.SalesChanelID) != 0)             //this will select the login user in combo
                {

                    //btnShowAll.Visible = false;
                    //cmbsaleschannel.Enabled = false;
                }
                else
                {
                    btnShowAll.Visible = true;
                }
                btnShowAll.Visible = true;
                ViewState["TotalRecords"] = 0;


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
        ExportToExcel.Visible = false;

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {



            GetSearchData(1);

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {

        Response.Redirect("ApproveRetailer.aspx");
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
                Int32 RetailerID = Convert.ToInt32(GridRetailer.DataKeys[e.Row.RowIndex].Value);
                CheckResult = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ISRetailerISPMapping"));

                GridViewRow GVR = e.Row;

                // ImageButton btnStatus = (ImageButton)GVR.FindControl("imgActive");
                Label lblParentSalesChannelId = (Label)GVR.FindControl("lblParentSalesChannelId");
                Label lblStatus = (Label)GVR.FindControl("lblStatus");
                ZedControlLib.ZedImageButton imgEdit = (ZedControlLib.ZedImageButton)GVR.FindControl("img1");
                //if (lblStatus.Text == "True")
                //{
                //    if (PageBase.SalesChanelID == 0)
                //    {
                //        imgEdit.Visible = true;
                //    }
                //    else
                //    {
                //        if (PageBase.SalesChanelID == Convert.ToInt32(lblParentSalesChannelId.Text))
                //        {
                //            imgEdit.Visible = true;
                //        }
                //        else
                //        {
                //            imgEdit.Visible = false;
                //        }
                //    }
                //}
                //else
                //{
                //    imgEdit.Visible = false;
                //}

                HyperLink HLDetails = default(HyperLink);
                HLDetails = (HyperLink)GVR.FindControl("HLDetails");
                string strViewDBranchDtlURL = null;
                strViewDBranchDtlURL = Server.UrlEncode(Crypto.Encrypt(Convert.ToString(RetailerID), PageBase.KeyStr)).ToString().Replace("+", " ");
                {
                    HLDetails.Text = "Details";
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

                /*
                if (CheckResult > 0)
                {
                    if (btnStatus != null)
                    {
                        btnStatus.Attributes.Add("Onclick", "javascript:alert('This retailer is linked to existing data.You can not deactivate it.');{return false;}");
                    }
                }
                 * */


                /* #CC03 Add Start */
                ZedControlLib.ZedImageButton imageEdit = (ZedControlLib.ZedImageButton)GVR.FindControl("ZedImgEdit");
                /* #CC04 Comment Start */
                /*if (ddlApproveStatus.SelectedValue == "0")
                {
                    imageEdit.Visible = true;
                    //imageEdit.Attributes.CssStyle.Add("display", "block");
                }*/
                /* #CC03 Add End */
                /* #CC04 Comment End */
                /* #CC04 Add Start */
                if (e.Row.Cells[10].Text.ToLower() == "pending")
                {
                    imageEdit.Visible = true;
                }
                else
                {
                    imageEdit.Visible = false;
                }
                /* #CC04 Add End*/

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

            /*  if (e.CommandName == "Active")
              {

                  if (RetailerId > 0)
                  {
                      using (RetailerData ObjRetailer = new RetailerData())
                      {

                          ObjRetailer.RetailerID = RetailerId;
                          Result = ObjRetailer.UpdateStatusRetailerInfo();

                      };
                      if (Result > 0)
                      {
                          ucMessage1.ShowSuccess(Resources.Messages.StatusChanged);
                      }
                      else
                      {
                          ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                      }
                      ucPagingControl1.FillPageInfo();
                      GetSearchData(ucPagingControl1.CurrentPage);
                  }
              }*/
            if (e.CommandName == "cmdEdit")
            {
                Response.Redirect("ManageRetailer.aspx?RetailerIdFromApproval=" + Server.UrlEncode(Crypto.Encrypt(Convert.ToString(RetailerId), PageBase.KeyStr)));
            }
            /*  #CC03 Add Start */
            else if (e.CommandName == "EditRetailer")
            {
                Response.Redirect("ManageRetailer.aspx?RetailerId=" + Server.UrlEncode(Crypto.Encrypt(Convert.ToString(RetailerId), PageBase.KeyStr)) + "&type=update");
            }
            /*  #CC03 Add End */
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }


    }
    protected void ExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            using (RetailerData ObjRetailer = new RetailerData())
            {
                ObjRetailer.RetailerName = txtRetailername.Text.Trim();

                if (ucServiceEntity.SelectedValue != 0)
                {
                    ObjRetailer.SalesChannelID = Convert.ToInt32(ucServiceEntity.SelectedValue);
                }
                ObjRetailer.LoggedInSalesChannelid = PageBase.SalesChanelID;
                ObjRetailer.PageIndex = -1;
                ObjRetailer.PageSize = Convert.ToInt32(ViewState["TotalRecords"]);
                ObjRetailer.UserID = PageBase.UserId;
                ObjRetailer.RetailerCode = txtRetailerCode.Text.Trim();
                ObjRetailer.intStatus = 0;

                // ObjRetailer.RetailerApproval = 0; /* 0 is passed to fetch unapproved retailers. */
                ObjRetailer.RetailerApproval = Convert.ToInt32(ddlApproveStatus.SelectedValue);
                ObjRetailer.FetchDataForApproval = 1; /* #CC16 Added */
                ObjRetailer.RetailerApproval = Convert.ToInt32(ddlApproveStatus.SelectedValue);

                /*   #CC19 Comment Start
                   DataTable dt = ObjRetailer.GetRetailerInfoV3();

                    DataTable dtExp = dt.Copy();
                   #CC19 Comment End */

                /* #CC19 Add Start */
            DataSet dsData = ObjRetailer.GetRetailerApprovalInfo();

            if (ObjRetailer.intOutParam == 2)
            {
                dvhide.Visible = false;
                dvFooter.Visible = false;
                HideControls();
                GridRetailer.DataSource = null;
                GridRetailer.DataBind();
                ucMessage1.ShowInfo(ObjRetailer.Error);
            }
            else
            {
                if (ObjRetailer.TotalRecords == 0)
                {
                    ucMessage1.ShowError(Resources.Messages.NoRecord);
                }
                else
                {
                    DataTable dtExp = dsData.Tables[0].Copy();// ObjRetailer.GetRetailerInfoV3(); //dsData.Tables[0].Copy();
                    /* #CC19 Add End */

                   
                    /* #CC12 Add Start */
                    if (PageBase.ChangeTinLabel == 1)
                    {
                        if (dtExp.Columns.Contains("TinNumber"))
                            dtExp.Columns["TinNumber"].ColumnName = /* "VATNnumber:"; #CC15 Commented */ "GSTNo.";/* #CC15 Added */
                    }
                    /* #CC12 Add End */
                    /* #CC05 Add Start */
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
                    /* #CC05 Add End */
                    /* #CC06 Add Start */
                    if (Convert.ToInt32(Session["TehsillDisplayMode"]) != 1)
                    {
                        if (dtExp.Columns.Contains("TehsilName"))
                        {
                            dtExp.Columns.Remove("TehsilName");
                        }
                    }
                    /* #CC06 Add End */

                    /*#CC14 START ADDED*/
                    if (dtExp.Columns.Contains("Address1"))
                        dtExp.Columns["Address1"].ColumnName = "Address Line 1";
                    if (dtExp.Columns.Contains("Address2"))
                        dtExp.Columns["Address2"].ColumnName = "Address Line 2";
                    /*#CC14 START ADDED*/

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
              /* #CC19 Add Start */  }
            } /* #CC19 Add End */
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

            txtRetailername.Text = "";
            ((DropDownList)ucServiceEntity.FindControl("ddlServiceEntity")).SelectedIndex = 0;
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
        ViewState["TotalRecords"] = 0;
        using (RetailerData ObjRetailer = new RetailerData())
        {
            ObjRetailer.RetailerName = txtRetailername.Text.Trim();

            {
                ObjRetailer.SalesChannelID = Convert.ToInt32(ucServiceEntity.SelectedValue);
            }

            ObjRetailer.LoggedInSalesChannelid = PageBase.SalesChanelID;

            ObjRetailer.PageIndex = pageno;
            ObjRetailer.PageSize = Convert.ToInt32(PageSize);
            ObjRetailer.UserID = PageBase.UserId;
            ObjRetailer.RetailerCode = txtRetailerCode.Text.Trim();
            ObjRetailer.intStatus = 0;
            // ObjRetailer.RetailerApproval = 0; /* 0 is passed to fetch unapproved retailers. */
            ObjRetailer.FetchDataForApproval = 1; /* #CC16 Added */
            ObjRetailer.RetailerApproval = Convert.ToInt32(ddlApproveStatus.SelectedValue);

           /* #CC19 Comment Start 
            Dt = ObjRetailer.GetRetailerInfoV3();

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

            }  #CC19 Comment End */

            /* #CC19 Add Start */
            DataSet dsData = ObjRetailer.GetRetailerApprovalInfo();

            if (ObjRetailer.TotalRecords > 0)
            {
                ExportToExcel.Visible = true;
                GridRetailer.Visible = true;
                dvFooter.Visible = true;
                ViewState["TotalRecords"] = ObjRetailer.TotalRecords;
                ucPagingControl1.TotalRecords = ObjRetailer.TotalRecords;
                ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                ucPagingControl1.SetCurrentPage = pageno;
                ucPagingControl1.FillPageInfo();
                GridRetailer.DataSource = dsData;
                GridRetailer.DataBind();
                dvhide.Visible = true;
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
            /* #CC19 Add End */
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
    }
}
