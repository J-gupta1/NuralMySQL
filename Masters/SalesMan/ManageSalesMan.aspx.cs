/*
====================================================================================================================================
 *Change Log: 
 *DD-MMM-YYYY, Name , #CCXX - Description
------------------------------------------------------------------------------------------------------------------------------------
 * 22-Mar-2016, Sumit Maurya, #CC01, Issue of page getting blocked on the execution of event(s) due to update panel resolved.
 * 22-Jul-2016, Sumit Maurya, #CC02, New field EmailID created and supplied to save/update Email of Salesman.
 * 04-Aug-2016, Sumit Maurya, #CC03, Regex added for mobbile number and new check added to check either emailid or mobile number should be given by user. Salesmancode is now auto generated so  not required to pass through user.
 * 19 Aug 2016, Karam Chand Sharma, #CC04, Allow custome paging 
 * 24-Aug-2016, Sumit Maurya, #CC05, New previous method commented to update salesman status and new added to show error msg.
 * 27-Sep-2016, Sumit Maurya, #CC06, New filter controls added and allowed ND to create SalesChannel of its own RDS.
 * 29-Sep-2016, Karam Chand Sharma, #CC07, bind SalesChannelName + SalesChannelCode in SalesChannel dropdown as per client requirement 
 * 10-Nov-2016, Sumit Maurya, #CC08, Value supplied if logged in SaleschannelTypeID is 7.
 * 12-July-2018, Kalpana, #CC09, resolved design issue
 * * 20-Nov-2018,Vijay Kumar Prajapati,#CC10,Add Username and password because salesman login using tsm app.
 * 27-Nov-2018,Vijay Kumar Prajapati,#CC11,Add userid For insert and update in Parameter.
 * 29-Nov-2018,Vijay Kumar Prajapati,#CC12,Active/Inactive Saleman and record updated in entitymapping.
====================================================================================================================================
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
using System.Globalization;
using System.Resources;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;

public partial class Masters_SalesMan_ManageSalesMan : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            ucMsg.Visible = false;
            if (!IsPostBack)
            {
                chkActive.Checked = true;
                if (ViewState["EditSalesManId"] != null)
                { ViewState.Remove("EditSalesManId"); }
                /*#CC04 START COMMENTED FillGrid(); #CC04 END COMMENTED */
                GetSearchData(1);/*#CC04 ADDED*/
                FillSalesChannel();
                /*#CC10 Added Started*/
                if (Convert.ToString(HttpContext.Current.Session["SalesManLOGIN"]) == "1")
                {
                    tdlogindetails.Visible = true;
                }
                else
                {
                    tdlogindetails.Visible = false;
                }
                /*#CC10 Added End*/
                this.Form.DefaultButton = btnShow.UniqueID;
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }

    }
    private void FillSalesChannel()
    {
        ddlSalesChannel.Enabled = true;
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            /* #CC06 Add Start */
            ObjSalesChannel.SalesChannelID = Convert.ToInt32(PageBase.SalesChanelID);
            ObjSalesChannel.BindChild = ((Convert.ToInt16(PageBase.SalesChanelTypeID) == 6) || (Convert.ToInt16(PageBase.SalesChanelTypeID) == 7)) ? 1 : 0; /* #CC08 (Convert.ToInt16(PageBase.SalesChanelTypeID) == 7) Added */

            /* #CC06 Add End */
            ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
            ObjSalesChannel.SearchType = EnumData.eSearchConditions.Active;
            ObjSalesChannel.BilltoRetailer = true;
            ObjSalesChannel.StatusValue = 1;

            String[] StrCol = new String[] { "SalesChannelID", "DisplayName"/*#CC07 START COMMENTED "SalesChannelName"  #CC07 END COMMENTED*/};
            PageBase.DropdownBinding(ref ddlSalesChannel, ObjSalesChannel.GetSalesChannelforSalesMan(), StrCol);
            if (PageBase.SalesChanelTypeID != 6)
                if (Convert.ToInt32(PageBase.SalesChanelID) != 0)             //this will select the login user in combo
                {
                    ddlSalesChannel.SelectedValue = PageBase.SalesChanelID.ToString();
                    ddlSalesChannel.Enabled = false;
                }

        };
    }
    protected void btnCreateUser_Click(object sender, EventArgs e)
    {
        string StrPSalt = "";
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }
            if (txtSalesManName.Text.Trim().Length == 0)
            {
                ucMsg.ShowInfo(Resources.Messages.MandatoryField);
                return;
            }
            /* #CC03 Add Start */
            /*#CC09 Added started*/
            if (Convert.ToString(HttpContext.Current.Session["SalesManLOGIN"]) != "1")
            {
                if (txtMobile.Text.Trim() == "" && txtemail.Text.Trim() == "")
                {
                    ucMsg.ShowInfo("Please enter Mobile number or EmailID.");
                    return;
                }
            }
            else
            {
                if (txtMobile.Text.Trim() == "")
                {
                    ucMsg.ShowInfo("Please enter Mobile number.");
                    return;
                }
                if (txtemail.Text.Trim() == "")
                {
                    ucMsg.ShowInfo("Please enter EmailID.");
                    return;
                }
            }
            /*#CC09 Added End*/
            if (txtMobile.Text.Trim() == "" && txtemail.Text.Trim() == "")
            {
                ucMsg.ShowInfo("Please enter Mobile number or EmailID.");
                return;
            }
            /* #CC03 Add End */

            if (txtSalesManName.Text.Trim().Length > 0)
            {
                Int32 result = 0;
                using (SalesmanData objSales = new SalesmanData())
                {
                    if (Convert.ToInt32(ViewState["EditSalesManId"]) == 0)
                    {
                        objSales.SalesmanID = 0;
                    }
                    else
                    {
                        objSales.SalesmanID = Convert.ToInt32(ViewState["EditSalesManId"]);
                    }
                    if (objSales.SalesmanID == 0)
                    {
                        if /*#CC09 Added started*/(Convert.ToString(HttpContext.Current.Session["SalesManLOGIN"]) == "1")
                        {
                            using (Authenticates ObjAuth = new Authenticates())
                            {
                                StrPSalt = ObjAuth.GenerateSalt(txtpassword.Text.Trim().Length);
                                objSales.Password = ObjAuth.EncryptPassword(txtpassword.Text.Trim(), StrPSalt);

                            };
                            objSales.PasswordSalt = StrPSalt;
                            objSales.UserName = txtUserName.Text.Trim();
                            objSales.PasswordExpiryDays = Convert.ToInt16(Application["ExpiryDays"].ToString());
                        }
                        else
                        {
                            objSales.Password = null;
                            objSales.PasswordSalt = null;
                            objSales.UserName = null;
                            objSales.PasswordExpiryDays = 0;
                        }
                    }
                    /*#CC09 Added End*/
                    objSales.Salesmanname = txtSalesManName.Text.Trim();
                    /* objSales.SalesmanCode = txtSalesManCode.Text.Trim(); #CC03 Commented*/
                    objSales.Address = txtAddress.Text.Trim();
                    objSales.MobileNumber = txtMobile.Text.Trim();
                    objSales.Status = chkActive.Checked;
                    //objSales.SalesChannelID = PageBase.SalesChanelID;
                    objSales.SalesChannelID = Convert.ToInt32(ddlSalesChannel.SelectedValue);
                    objSales.EmailID = txtemail.Text.Trim() == "" ? null : txtemail.Text.Trim(); /* #CC02 Added */
                    objSales.UserID = PageBase.UserId;/*#CC11 Added*/
                    result = objSales.InsertUpdateSalesManInfo();
                    if (objSales.Error != null && objSales.Error != string.Empty)
                    {
                        ucMsg.ShowInfo(objSales.Error);
                        return;
                    }
                };
                if (result > 0)
                {
                    if (ViewState["EditSalesManId"] == null)
                    {
                        ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    }
                    else
                    {
                        ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                    }
                    ClearForm();
                    /*#CC04 START COMMENTED FillGrid(); #CC04 END COMMENTED */
                    GetSearchData(1);/*#CC04 ADDED*/
                    FillSalesChannel();
                    updAddUserMain.Update();
                    chkActive.Checked = true;
                    return;
                }
                else
                {
                    ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    return;
                }
            }
        }

        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }

    }
    protected void btnExprtToExcel_Click(object sender, EventArgs e)
    {
        /*#CC04 START COMMENTED */
        //try
        //{
        //    if (ViewState["Dtexport"] != null)
        //    {
        //        DataTable dt = (DataTable)ViewState["Dtexport"];
        //        string[] DsCol = new string[] { "SalesChannelCode", "SalesChannelName", "SalesmanName", "SalesmanCode", "Address", "MobileNumber", "Email", "StatusText" }; /* #CC02 New column Email Selected */
        //        DataTable DsCopy = new DataTable();
        //        dt = dt.DefaultView.ToTable(true, DsCol);
        //        dt.Columns["StatusText"].ColumnName = "Status";
        //        if (dt.Rows.Count > 0)
        //        {
        //            DataSet dtcopy = new DataSet();
        //            dtcopy.Merge(dt);
        //            dtcopy.Tables[0].AcceptChanges();
        //            String FilePath = Server.MapPath("../../");
        //            string FilenameToexport = "SalesManList";
        //            PageBase.RootFilePath = FilePath;
        //            PageBase.ExportToExecl(dtcopy, FilenameToexport);
        //            ViewState["Dtexport"] = null;
        //        }
        //        else
        //        {
        //            ucMsg.ShowError(Resources.Messages.NoRecord);
        //        }
        //        ViewState["Dtexport"] = null;
        //    }
        //}
        //catch (Exception ex)
        //{
        //    ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        //}
        /*#CC04 END COMMENTED */
        /*#CC04 START ADDED*/
        string Password = string.Empty;   /*#CC10 Added*/
        try
        {
            using (SalesmanData objSalesMan = new SalesmanData())
            
            {
                /* #CC06 Add Start */
                objSalesMan.EmailID = txtEmailIDSearch.Text.Trim();
                objSalesMan.SalesChannelCode = txtSaleChannelCodeSearch.Text.Trim();
                objSalesMan.MobileNumber = txtSalesManMobileNumberSearch.Text.Trim();
                /* #CC06 Add Start */
                objSalesMan.Salesmanname = txtSalesManSearch.Text.Trim();
                objSalesMan.SalesmanCode = txtSalesManCodeSearch.Text.Trim();
                objSalesMan.SalesChannelID = PageBase.SalesChanelID;
                objSalesMan.PageIndex = -1;
                DataTable dtSalesMan = objSalesMan.GetSalesmanInfoV2();
                /*#CC09 Added Started*/
                if (dtSalesMan.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSalesMan.Rows)
                    {
                        using (Authenticates ObjAuth = new Authenticates())
                        {
                            Password = ObjAuth.DecryptPassword(Convert.ToString(dr["Password"]), Convert.ToString(dr["PasswordSalt"]));
                        };
                        dr["Password"] = Password;
                    }

                }
                string[] DsCol = new string[] { "SalesChannelCode", "SalesChannelName", "SalesmanName", "SalesmanCode", "LoginName", "Password", "Address", "MobileNumber", "Email", "StatusText" };
                DataTable DsCopy = new DataTable();
                dtSalesMan = dtSalesMan.DefaultView.ToTable(true, DsCol);
                /*#CC09 Added End*/
                if (dtSalesMan.Rows.Count > 0)
                {
                    DataSet dtcopy = new DataSet();
                    dtcopy.Merge(dtSalesMan);
                    dtcopy.Tables[0].AcceptChanges();
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "SalesManList";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dtcopy, FilenameToexport);
                }
                else
                {
                    ucMsg.ShowError(Resources.Messages.NoRecord);
                }
            };
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
        /*#CC04 END ADDED*/

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearForm();
        ViewState["EditSalesManId"] = null;
        FillSalesChannel();
        if /*#CC10 Added started*/(Convert.ToString(HttpContext.Current.Session["SalesManLOGIN"]) == "1")
        {
            tdlogindetails.Visible = true;
        }
    }
    protected void btnSearchUser_Click(object sender, EventArgs e)
    {
        /*#CC04 START COMMENTED FillGrid(); #CC04 END COMMENTED */
        GetSearchData(1);/*#CC04 ADDED*/
        FillSalesChannel();

    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        ClearForm();
        /*#CC04 START COMMENTED FillGrid(); #CC04 END COMMENTED */
        GetSearchData(1);/*#CC04 ADDED*/
        FillSalesChannel();
    }
    private void FillGrid()
    {
        DataTable dtSalesMan;
        using (SalesmanData objSalesMan = new SalesmanData())
        {
            objSalesMan.Salesmanname = txtSalesManSearch.Text.Trim();
            objSalesMan.SalesmanCode = txtSalesManCodeSearch.Text.Trim();
            objSalesMan.SalesChannelID = PageBase.SalesChanelID;
            dtSalesMan = objSalesMan.GetSalesmanInfo();
            gvSalesMan.Visible = true;
        };
        if (dtSalesMan != null && dtSalesMan.Rows.Count > 0)
        {
            ViewState["Dtexport"] = dtSalesMan;
            gvSalesMan.DataSource = dtSalesMan;
            btnExprtToExcel.Visible = true;

        }
        else
        {
            gvSalesMan.DataSource = null;
            ucPagingControl1.Visible = false;
            btnExprtToExcel.Visible = false;

        }
        gvSalesMan.DataBind();
        UpdSearch.Update();
        updAddUserMain.Update();

    }
    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            tdlogindetails.Visible = false;
            chkActive.Enabled = true;
            ImageButton btnEdit = (ImageButton)sender;
            DataTable dtSales;
            using (SalesmanData objSalesMan = new SalesmanData())
            
            {
                objSalesMan.SalesmanID = Convert.ToInt32(btnEdit.CommandArgument);
                objSalesMan.PageIndex = 1;
                objSalesMan.PageSize = 1;
                dtSales = objSalesMan.GetSalesmanInfo();
            };
            ViewState["EditSalesManId"] = Convert.ToInt32(btnEdit.CommandArgument);
            if (dtSales != null && dtSales.Rows.Count > 0)
            {
                ddlSalesChannel.ClearSelection();
                //ddlSalesChannel.SelectedValue = dtSales.Rows[0]["SalesChannelID"].ToString();
                ddlSalesChannel.Items.FindByValue(dtSales.Rows[0]["SalesChannelID"].ToString()).Selected = true;


                ddlSalesChannel.Enabled = false;
                txtSalesManName.Text = (dtSales.Rows[0]["SalesmanName"].ToString());
                /*txtSalesManCode.Text = (dtSales.Rows[0]["SalesmanCode"].ToString());  #CC03 Commented */
                txtAddress.Text = (dtSales.Rows[0]["Address"].ToString());
                txtMobile.Text = (dtSales.Rows[0]["MobileNumber"].ToString());
                txtemail.Text = (dtSales.Rows[0]["Email"].ToString()); /* #CC02 Added */
                chkActive.Checked = Convert.ToBoolean(dtSales.Rows[0]["Status"]);
                btnCreateUser.Text = "Update";
                /*#CC04 START COMMENTED FillGrid(); #CC04 END COMMENTED */
                GetSearchData(1);/*#CC04 ADDED*/
                updgrid.Update();
                updAddUserMain.Update();
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnActiveDeactive_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton btnActiveDeactive = (ImageButton)sender;
            Int32 Result = 0;
            Int32 SalesManID = Convert.ToInt32(btnActiveDeactive.CommandArgument);
            using (SalesmanData objSalesMan = new SalesmanData())
            {

                objSalesMan.SalesmanID = SalesManID;
                objSalesMan.UserID = PageBase.UserId;/*#CC11 Added*/
                /* #CC05 Add Start */
                Result = objSalesMan.UpdateStatusSalesManInfoV2();
                if (Result == 0)
                {
                    ucMsg.ShowSuccess(Resources.Messages.StatusChanged);
                }
                else if (Result == 1)
                {
                    if (objSalesMan.Error != "")
                    {
                        ucMsg.ShowInfo(objSalesMan.Error);
                    }
                }
                else
                {
                    ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                }
            }
            /* #CC05 Add End */
            /* #CC05 Comment Start Result = objSalesMan.UpdateStatusSalesManInfo();
         };
         if (Result == 1)
         {
             ucMsg.ShowSuccess(Resources.Messages.StatusChanged);
         }
         else
         {
             ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
         }  #CC05 Comment End */

            /*#CC04 START COMMENTED FillGrid(); #CC04 END COMMENTED */
            GetSearchData(1);/*#CC04 ADDED*/
            ClearForm();
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
        updgrid.Update();

    }
    void ClearForm()
    {
        /* #CC06 Add Start */
        txtEmailIDSearch.Text = "";
        txtSaleChannelCodeSearch.Text = "";
        txtSalesManMobileNumberSearch.Text = "";
        /* #CC06 Add Start */
        txtSalesManName.Text = "";
        /* txtSalesManCode.Text = "";#CC03 Commented */
        txtAddress.Text = "";
        txtMobile.Text = "";
        txtSalesManSearch.Text = "";
        txtSalesManCodeSearch.Text = "";
        txtemail.Text = ""; /* #CC02 Added */
        btnCreateUser.Text = "Submit";
        ViewState["EditSalesManId"] = null;
        UpdSearch.Update();
    }

    protected void gvSalesMan_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvSalesMan.PageIndex = e.NewPageIndex;
            FillGrid();
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }

    }
    protected void gvSalesMan_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
           
         
            int CheckResult = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Int32 SalesmanID = Convert.ToInt32(gvSalesMan.DataKeys[e.Row.RowIndex].Value);
                using (SalesmanData ObjSalesman = new SalesmanData())
                {
                    ObjSalesman.SalesmanID = SalesmanID;
                    CheckResult = ObjSalesman.CheckSalesmanExistence();
                };
               GridViewRow GVR = e.Row;

                ImageButton btnStatus = (ImageButton)GVR.FindControl("btnActiveDeactive");
                /*#CC09 Added Started*/
            LinkButton hlPassword = default(LinkButton);
            hlPassword = (LinkButton)GVR.FindControl("hlPassword");
            string strPassword = null;
            Label lblPassword = (Label)GVR.FindControl("lblPassword");
            Label lblPasswordSalt = (Label)GVR.FindControl("lblPasswordSalt");
            strPassword = fncChangePwd(lblPassword.Text, lblPasswordSalt.Text);
            hlPassword.Attributes.Add("Onclick", "javascript:alert('Salesman password is : " + strPassword + "');{return false;}");
                /*#CC09 Added End*/

                /*#CC12 Commented Started*/
                //if (CheckResult > 0)
                //{


                //    if (btnStatus != null)
                //    {
                //        btnStatus.Attributes.Add("Onclick", "javascript:alert('This salesman is linked to existing data.You can not deactivate it.');{return false;}");

                //    }
                //}
                /*#CC12 Commented End*/

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());

        }
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        GetSearchData(ucPagingControl1.CurrentPage);

    }
    /*#CC04 START ADDED*/
    public void GetSearchData(int pageno)
    {
        ViewState["TotalRecords"] = 0;

        DataTable dtSalesMan;
        using (SalesmanData objSalesMan = new SalesmanData())
        
        {
            objSalesMan.Salesmanname = txtSalesManSearch.Text.Trim();
            objSalesMan.SalesmanCode = txtSalesManCodeSearch.Text.Trim();
            objSalesMan.SalesChannelID = PageBase.SalesChanelID;

            objSalesMan.PageIndex = pageno;
            objSalesMan.PageSize = Convert.ToInt32(PageSize);
            /* #CC06 Add Start */
            objSalesMan.EmailID = txtEmailIDSearch.Text.Trim();
            objSalesMan.SalesChannelCode = txtSaleChannelCodeSearch.Text.Trim();
            objSalesMan.MobileNumber = txtSalesManMobileNumberSearch.Text.Trim();
            /* #CC06 Add Start */
            dtSalesMan = objSalesMan.GetSalesmanInfoV2();

            gvSalesMan.Visible = true;

            if (dtSalesMan != null && dtSalesMan.Rows.Count > 0)
            {
                ViewState["Dtexport"] = dtSalesMan;
                gvSalesMan.DataSource = dtSalesMan;
                btnExprtToExcel.Visible = true;

                ucPagingControl1.TotalRecords = objSalesMan.TotalRecords;
                ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                ucPagingControl1.SetCurrentPage = pageno;
                ucPagingControl1.FillPageInfo();

            }
            else
            {
                gvSalesMan.DataSource = null;
                //btnExprtToExcel.Visible = false;
                //btnExprtToExcel.Enabled = false;
                ucPagingControl1.Visible = false;
            }
            gvSalesMan.DataBind();
            UpdSearch.Update();
            updAddUserMain.Update();
        };
        /*#CC04 END ADDED*/
    }
    /*#CC09 Added Started*/
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


            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
        return vMailPassword;
    }
    /*#CC09 Added End*/
}
