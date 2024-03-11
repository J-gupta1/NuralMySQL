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
using System.Collections;
using System.Data.SqlClient;
using ZedService;
using BusinessLogics;
using System.Security.Cryptography;
/*
 * 27 Apr 2015, Karam chand Sharma, #CC01, Check added for opening stock date . Date was assign when sale channel type change so correct them
 * 20-Oct-2015, Sumit Maurya, #CC02, Validation changed in phone number and mobile number both shouldnot be start with 0 and min & max digits must be 10.
 * 11-Jul-2016, Sumit Maurya, #CC03, Alternate Mobile number added in interface which is non mandatory.
 * 04-Nov-2016, Karam Chand Sharma, #CC04, ASM will load according to ND to CBH mapping and it will load according to configuration name NDHIERARCHYMAPPING (0 = Default/previous binding 1 = ASM will load according to ND CBH mapping)
 * 13-Feb-2017, Sumit Maurya, #CC05, parent saleschannel binding changed as it was reflecting extra records on reporting hierarchy. 
 *  20 Dec 2017,Vijay Kumar Prajapati,#CC06,User Cap Creation For Comio.
 *   *  14 May 2018,Rajnish Kumar ,#CC07,User Id Passed as Parameter for Binding Parent SalesChannel and organisation Hierarchy according to login.
 * 17 May 2018,Rajnish Kumar ,#CC08,Enabled false parent saleschannel and organisation hierarchy in case of approve Sales channel.
 *   14 June 2018,Rajnish Kumar ,#CC09,View is visible only for Ho Login.  
 *  04 July 2018,Rajnish Kumar ,#CC10,Pan Mandatory. 
 *  25-Jul-2018, Sumit Maurya, #CC11, On selection of saleschanneltype "Warehouse" only, Reporting hierarchy gets binded (Done for ComioV5)
 *  10-Sep-2018,Vijay Kumar Prajapati,#CC12,Return Saleschannel code with Successfull creation Message.(Done for karbonn)
 *  23-Oct-2018, Sumit Maurya, #CC13, Viewstate conversion of ISPANNo was causing error, same is rectified (Done For Inovu).
 * 28-Nov-2018, Rakesh Raj, #CC16, Inactive/Active Sales channel TicketId#32895
 *12-Dec-2018, Balram Jha, #CC17- View Sales channel page seoarated in menu 
 *07-April-2020,Vijay Kumar Prajapati,#CC18--Remove Country From UI.
 */
public partial class Masters_HO_SalesChannel_ManageSalesChannel : PageBase
{
    int SalesChannelId = 0;
    Int16 FromChildPageStatus;
    int SelectedCountryID, CountryCount, StateID;
    DataTable dtNew = new DataTable();
    object objSum;
    int counter = 0;
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    UploadFile UploadFile = new UploadFile();
    List<String> lstDuplicate = new List<String>();
    DataSet dsErrorProne = new DataSet();
    string strPrimarySessionName = "SaveUploadSession";
    string strPrimarySessionNameupdate = "UploadSessionUpdate";
    string strPSalt, strPassword;
    public static string strDefaultPassword = System.Configuration.ConfigurationManager.AppSettings["DefaultPassword"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMessage1.ShowControl = false;
        try
        {
            //if (SalesChanelTypeID != 0 || SalesChanelID==0)
            //{
            //    LBViewSalesChannel.Visible = true;
            //}
            /*#CC09*/
            if ((SalesChannelLevel == 1) && (BaseEntityTypeID == 2))
            {
                LBViewSalesChannel.Visible = true;
            }
            else
            {
                LBViewSalesChannel.Visible = false;
            }  /*#CC09*/

            if ((Request.QueryString["SalesChannelId"] != null) && (Request.QueryString["SalesChannelId"] != ""))
            {
                SalesChannelId = Convert.ToInt32(Convert.ToString(Crypto.Decrypt(Request.QueryString["SalesChannelId"], PageBase.KeyStr)));
            }
            if ((Request.QueryString["ChangeStatus"] != null) && (Request.QueryString["ChangeStatus"] != ""))
                FromChildPageStatus = Convert.ToInt16(Request.QueryString["ChangeStatus"]);


            //ucOpeningStock.Date = Session["DefaultOpStockDate"].ToString();
            if (SalesChanelTypeID != 0 | SalesChanelTypeID == 0) //All code due to gionee
            {//Now this code will be run for the HO
                FillDate();

                lblSubmitOpeningStock.Visible = true;
                ucOpeningStock.Visible = true;
                ucOpeningStock.IsRequired = true;
                if (Session["DefaultOpStockDate"] != null)
                {
                    if (Convert.ToString(Session["DefaultOpStockDate"]) == "-1")
                        ucOpeningStock.ValidationGroup = "Dummy";
                    else
                        ucOpeningStock.ValidationGroup = "Add";  //Pankaj Kumar 08-March-2014 Added
                }
                //ucOpeningStock.ValidationGroup = "Add";  //Pankaj Kumar 08-March-2014 Commented
                info.Visible = true;

            }
            else
            {
                lblSubmitOpeningStock.Visible = false;
                ucOpeningStock.Visible = false;
                ucOpeningStock.IsRequired = false;
                ucOpeningStock.ValidationGroup = "Dummy";
                info.Visible = false;

            }
            if (Session["DefaultOpStockDate"] != null)
            {
                if (Convert.ToString(Session["DefaultOpStockDate"]) != "0")
                {
                    ucOpeningStock.imgCal.Enabled = false;
                    ucOpeningStock.TextBoxDate.Enabled = false;
                }
            }
            if ((Request.QueryString["SalesChannelId"] != null) && (Request.QueryString["SalesChannelId"] != ""))
            {
                SalesChannelId = Convert.ToInt32(Convert.ToString(Crypto.Decrypt(Request.QueryString["SalesChannelId"], PageBase.KeyStr)));
                //trApproval.Attributes.CssStyle.Add("display", "none");
                btnCancel.Attributes.CssStyle.Add("display", "none");
                /* #CC15 Add Start */
                if ((Request.QueryString["type"] != null) && (Request.QueryString["type"] == "update"))
                {
                    Session["GoBack"] = "ApproveSalesChannel";
                    //btnReset.Attributes.CssStyle.Add("display", "block");
                    //btnReset.PostBackUrl = "~/Masters/Retailer/ApproveSalesChannel.aspx";
                }
                else
                {
                    /* #CC15 Add End */
                    /* #CC14 Add Start */
                    Session["GoBack"] = "ViewSalesChannel";
                    //btnReset.Attributes.CssStyle.Add("display", "none");

                    /* #CC14 Add End */
                } /* #CC15 Added */

            }
            /*#CC05 START ADDED*/
            if ((Request.QueryString["SalesChannelIdFromApproval"] != null) && (Request.QueryString["SalesChannelIdFromApproval"] != ""))
            {
                SalesChannelId = Convert.ToInt32(Convert.ToString(Crypto.Decrypt(Request.QueryString["SalesChannelIdFromApproval"], PageBase.KeyStr)));
                //FillSalesChannelTypeRDApprovel();
                //trApproval.Attributes.CssStyle.Add("display", "block");
                //btnCancel.Attributes.CssStyle.Add("display", "none");
                trApproval.Visible = true;
                txtApprovalRemarks.Visible = true;
                btnCancel.Visible = false;
                chkactive.Enabled = false;
                /* #CC14 Add Start */
                Session["GoBack"] = "ApproveSalesChannel";


                /* #CC14 Add End */

            }
            if (!IsPostBack)
            {
                ViewState["SalesType"] = null;
                PageInfo();
                lblpage.Text = Resources.Messages.SalesEntity;
                //cmborghierarchy.Items.Insert(0, new ListItem("Select","0"));
                if (Session["MessageType"] != null)
                {
                    ucMessage1.ShowSuccess(Session["MessageType"].ToString());
                    Session["MessageType"] = null;
                }

                FillSalesChannelType();

                if (SalesChannelId == 0)
                {
                    //cmbCountry.Items.Insert(0, new ListItem("Select", "0")); /*#CC18 Commented*/
                    cmbstate.Items.Insert(0, new ListItem("Select", "0"));
                }
                // fillcountry(); /*#CC18 Commented*/
                FillState();/*#CC18 Added*/
                //if (ViewState["CountryCount"] != null)
                // {

                //}
                //if (SalesChanelTypeID == 0)
                //    ChkWantToSubmitOpeningStock.Visible = false;
                //else
                //    ChkWantToSubmitOpeningStock.Visible = true;
                ucOpeningStock.TextBoxDate.Enabled = true;
                ucOpeningStock.imgCal.Enabled = true;
                if ((Request.QueryString["SalesChannelIdFromApproval"] != null) && (Request.QueryString["SalesChannelIdFromApproval"] != "") && (SalesChannelId != 0))
                {
                    FillSalesChannelTypeRDApprovel();
                    // fillcountry(); /*#CC18 Commented*/
                    FillState();/*#CC18 Added */
                    PouplateSalesChannelDetail(SalesChannelId);
                    btnReject.Visible = true;
                    trApproval.Visible = true;
                    txtApprovalRemarks.Visible = true;
                    btnReject.Visible = true;
                    btnSubmit.Text = "Approve";
                }
                else if (SalesChannelId != 0)
                {
                    //fillcountry(); /*#CC18 Commented*/
                    FillState();/*#CC18 Added*/
                    PouplateSalesChannelDetail(SalesChannelId);
                    btnReject.Visible = false;
                    btnSubmit.Text = "Update";
                    //if ((Request.QueryString["SalesChannelIdFromApproval"] != null) && (Request.QueryString["SalesChannelIdFromApproval"] != ""))
                    //{
                    //    btnReject.Visible = true;
                    //    trApproval.Visible = true;
                    //    txtApprovalRemarks.Visible = true;
                    //    btnReject.Visible = true;
                    //    btnSubmit.Text = "Approve";

                    //}
                    //else
                    //{
                    //    btnReject.Visible = false;
                    //    btnSubmit.Text = "Update";
                    //}/*#CC05 START END*/

                    //btnSubmit.Text = "Update";
                    int NotFireOpeningStockValidationOnUpdate = 1;/*0-Not Fire 1-TobeFired*/
                    if (SalesChannelId != 0 & ucOpeningStock.Date != "")
                    {
                        NotFireOpeningStockValidationOnUpdate = 0;
                    }
                    if (NotFireOpeningStockValidationOnUpdate == 0)
                        ucOpeningStock.ValidationGroup = "Dummy";

                }
                else
                {
                    if (SalesChanelTypeID != 0 | SalesChanelTypeID == 0)//now this code will be run for the Ho although we can remove this condition
                    {
                        if (Convert.ToString(Session["DefaultOpStockDate"]) != "0")
                        {
                            ucOpeningStock.Date = Session["DefaultOpStockDate"].ToString();
                            ucOpeningStock.TextBoxDate.Enabled = false;
                            ucOpeningStock.imgCal.Enabled = false;
                        }
                    }

                }

                /*#CC18 Added Started*/
                if (Rbtdownloadtemplate.SelectedValue == "1")
                {
                    ForSaveTemplateheading.Visible = true;
                    ForSaveTemplatedownload.Visible = true;
                    ReferenceIdForsaveheading.Visible = true;
                    ReferenceIdForsave.Visible = true;
                }
                else
                {
                    ForSaveTemplateheading.Visible = false;
                    ForSaveTemplatedownload.Visible = false;
                    ReferenceIdForsaveheading.Visible = false;
                    ReferenceIdForsave.Visible = false;
                }
                /*#CC18 Added End*/

            }


        }



        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }



    protected override void OnPreRender(EventArgs e)
    {
        txtpassword.Attributes.Add("value", txtpassword.Text);
        base.OnPreRender(e);
    }
    void PageInfo()
    {
        lblName.Text = Resources.Messages.SalesEntity;
        lblType.Text = Resources.Messages.SalesEntity;
        lblCode.Text = Resources.Messages.SalesEntity;
        lblParentType.Text = Resources.Messages.SalesEntity;
    }



    private void PouplateSalesChannelDetail(int SalesChannelId)
    {
        DataTable DtSalesChannelDetail;
        try
        {
            using (SalesChannelData ObjSalesChannel = new SalesChannelData())
            {

                ObjSalesChannel.SalesChannelID = SalesChannelId;
                ObjSalesChannel.BlnShowDetail = true;
                ObjSalesChannel.StatusValue = 2;
                DtSalesChannelDetail = ObjSalesChannel.GetSalesChannelInfo();
            };
            if (DtSalesChannelDetail.Rows.Count > 0 && DtSalesChannelDetail != null)
            {
                //lblpassword.Visible = false;
                //txtpassword.Visible = false;
                //lblreqpassword.Visible = false;
                txtAddress1.Text = DtSalesChannelDetail.Rows[0]["Address1"].ToString();
                txtAddress2.Text = DtSalesChannelDetail.Rows[0]["Address2"].ToString();
                cmbsaleschanneltype.SelectedItem.Selected = false;

                if (cmbsaleschanneltype.Items.FindByValue(DtSalesChannelDetail.Rows[0]["TYPEIDwithLevel"].ToString()) != null)
                    cmbsaleschanneltype.Items.FindByValue(DtSalesChannelDetail.Rows[0]["TYPEIDwithLevel"].ToString()).Selected = true;
                else
                    cmbsaleschanneltype.SelectedIndex = 0;

                cmbsaleschanneltype.Enabled = false;
                /*#CC08 start*/
                if ((Request.QueryString["SalesChannelIdFromApproval"] != null) && (Request.QueryString["SalesChannelIdFromApproval"] != "") && (SalesChannelId != 0))
                {
                    cmbsaleschanneltype_SelectedIndexChanged(cmbsaleschanneltype, new EventArgs());
                    if (cmbparentsaleschannel.Items.FindByValue(DtSalesChannelDetail.Rows[0]["ParentID"].ToString()) != null)
                    {
                        cmbparentsaleschannel.Items.FindByValue(DtSalesChannelDetail.Rows[0]["ParentID"].ToString()).Selected = true;
                        cmbparentsaleschannel.Enabled = false;
                    }
                    cmbparentsaleschannel_SelectedIndexChanged(cmbparentsaleschannel, new EventArgs());
                    if (cmborghierarchy.Items.FindByValue(DtSalesChannelDetail.Rows[0]["OrgnhierarchyID"].ToString()) != null)
                    {
                        cmborghierarchy.Items.FindByValue(DtSalesChannelDetail.Rows[0]["OrgnhierarchyID"].ToString()).Selected = true;
                        cmborghierarchy.Enabled = false;
                    }
                }
                else
                {
                    cmbsaleschanneltype_SelectedIndexChanged(cmbsaleschanneltype, new EventArgs());/*#CC07 added*/
                    cmborghierarchy.SelectedItem.Selected = false;

                    /* #CC05 Add Start */
                    cmbparentsaleschannel.SelectedItem.Selected = false;
                    if (cmbparentsaleschannel.Items.FindByValue(DtSalesChannelDetail.Rows[0]["ParentID"].ToString()) != null)
                        cmbparentsaleschannel.Items.FindByValue(DtSalesChannelDetail.Rows[0]["ParentID"].ToString()).Selected = true;
                    else
                        cmbparentsaleschannel.SelectedIndex = 0;
                    /* #CC05 Add End */
                    if (cmbsaleschanneltype.SelectedValue != "5")
                    {
                        cmbparentsaleschannel_SelectedIndexChanged(cmbparentsaleschannel, new EventArgs());
                    }


                    if (cmborghierarchy.Items.FindByValue(DtSalesChannelDetail.Rows[0]["OrgnhierarchyID"].ToString()) != null)

                        cmborghierarchy.Items.FindByValue(DtSalesChannelDetail.Rows[0]["OrgnhierarchyID"].ToString()).Selected = true;
                    else
                        cmborghierarchy.SelectedIndex = 0;

                    /* #CC05 Comment Start 
                      cmbparentsaleschannel.SelectedItem.Selected = false;
                    if (cmbparentsaleschannel.Items.FindByValue(DtSalesChannelDetail.Rows[0]["ParentID"].ToString()) != null)
                        cmbparentsaleschannel.Items.FindByValue(DtSalesChannelDetail.Rows[0]["ParentID"].ToString()).Selected = true;
                    else
                        cmbparentsaleschannel.SelectedIndex = 0;
                     #CC05 Comment End */


                    /*#CC04 START ADDED*/
                    if (PageBase.NDHIERARCHYMAPPING == "1" && cmbsaleschanneltype.SelectedValue == "7")
                    {
                        if (cmbsaleschanneltype.SelectedValue != "5")
                        {
                            cmbparentsaleschannel_SelectedIndexChanged(cmbparentsaleschannel, new EventArgs());
                            if (cmborghierarchy.Items.FindByValue(DtSalesChannelDetail.Rows[0]["OrgnhierarchyID"].ToString()) != null)

                                cmborghierarchy.Items.FindByValue(DtSalesChannelDetail.Rows[0]["OrgnhierarchyID"].ToString()).Selected = true;
                            else
                                cmborghierarchy.SelectedIndex = 0;
                        }
                    }  /*#CC08 end*/
                }
                /*#CC04 START END*/
                /*#CC18 Commented Started*/
                //cmbCountry.ClearSelection();
                //cmbCountry.SelectedValue = DtSalesChannelDetail.Rows[0]["CountryID"].ToString();
                //cmbCountry_SelectedIndexChanged(cmbCountry, new EventArgs());
                /*#CC18 Commented End*/
                cmbstate.ClearSelection();
                if (cmbstate.Items.FindByValue(DtSalesChannelDetail.Rows[0]["StateID"].ToString()) == null)
                {
                    cmbstate.SelectedValue = "0";
                }
                else
                {
                    cmbstate.SelectedValue = DtSalesChannelDetail.Rows[0]["StateID"].ToString();
                }
                cmbstate_SelectedIndexChanged(cmbstate, new EventArgs());
                cmbcity.ClearSelection();
                if (cmbcity.Items.FindByValue(DtSalesChannelDetail.Rows[0]["CityID"].ToString()) == null)
                {
                    cmbcity.SelectedValue = "0";
                }
                else
                {
                    cmbcity.SelectedValue = DtSalesChannelDetail.Rows[0]["CityID"].ToString();
                }
                cmbcity_SelectedIndexChanged(cmbstate, new EventArgs());
                cmbArea.ClearSelection();
                if (cmbArea.Items.FindByValue(DtSalesChannelDetail.Rows[0]["AreaId"].ToString()) == null)
                {
                    cmbArea.SelectedValue = "0";
                }
                else
                {
                    cmbArea.SelectedValue = DtSalesChannelDetail.Rows[0]["AreaId"].ToString();
                }
                txtcontactperson.Text = DtSalesChannelDetail.Rows[0]["contactperson"].ToString();
                txtpincode.Text = DtSalesChannelDetail.Rows[0]["PinCode"] == System.DBNull.Value ? string.Empty : DtSalesChannelDetail.Rows[0]["PinCode"].ToString();
                txtphone.Text = DtSalesChannelDetail.Rows[0]["PhoneNumber"].ToString();
                txtloginname.Text = DtSalesChannelDetail.Rows[0]["Loginname"].ToString();
                txtloginname.Enabled = false;
                txtpassword.Visible = false;
                reqpassword.Visible = false;
                lblpassword.Visible = false;
                lblreqpassword.Visible = false;
                reqpassword.ValidationGroup = "Add1";
                txtmobile.Text = DtSalesChannelDetail.Rows[0]["MobileNumber"].ToString();
                txtMobileNumber2.Text = DtSalesChannelDetail.Rows[0]["MobileNumber2"].ToString();/* #CC03 Added */
                txtfax.Text = DtSalesChannelDetail.Rows[0]["Fax"].ToString();
                txtcstno.Text = DtSalesChannelDetail.Rows[0]["CstNumber"].ToString();
                txtemail.Text = DtSalesChannelDetail.Rows[0]["Email"].ToString();
                txtsaleschannelcode.Text = DtSalesChannelDetail.Rows[0]["saleschannelcode"].ToString();
                txtsaleschannelname.Text = DtSalesChannelDetail.Rows[0]["SalesChannelname"].ToString();
                txttinno.Text = DtSalesChannelDetail.Rows[0]["tinnumber"] == System.DBNull.Value ? string.Empty : DtSalesChannelDetail.Rows[0]["tinnumber"].ToString();
                txtpanno.Text = DtSalesChannelDetail.Rows[0]["PanNO"].ToString();
                ucBuisnessstartdate.Date = DtSalesChannelDetail.Rows[0]["BussinessStartDate"].ToString();
                ucDOA.Date = DtSalesChannelDetail.Rows[0]["DOA"].ToString();
                ucDOB.Date = DtSalesChannelDetail.Rows[0]["DOB"].ToString();
                //#CC16
                if (FromChildPageStatus == 0)
                    chkactive.Checked = false; //Convert.ToBoolean(DtSalesChannelDetail.Rows[0]["Status"].ToString());
                else
                    chkactive.Checked = true;
                if (Convert.ToInt64(DtSalesChannelDetail.Rows[0]["GroupParentSalesChannelID"]) > 0) //Pankaj Dhingra
                {
                    rdomaplocation.SelectedValue = "1";
                    rdomaplocation_SelectedIndexChanged(rdomaplocation, new EventArgs());
                    cmbparentsaleschannelgroup.Items.FindByValue(DtSalesChannelDetail.Rows[0]["GroupParentSalesChannelID"].ToString()).Selected = true;
                }
                //08-Feb-2013

                //ChkWantToSubmitOpeningStock.Visible=false;
                if (DtSalesChannelDetail.Rows[0]["IsOpeningStockEntered"].ToString() == "True")
                {
                    ucOpeningStock.Date = DtSalesChannelDetail.Rows[0]["OpeningStockDate"].ToString();  //08-Feb-2013
                    ucOpeningStock.imgCal.Enabled = false;
                    ucOpeningStock.TextBoxDate.Enabled = false;
                }
                //else
                //{
                //    ucOpeningStock.imgCal.Enabled = true;
                //    ucOpeningStock.TextBoxDate.Enabled = true;
                //}
                //else
                //    ucOpeningStock.Visible = false;


            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }

    void FillSalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            DataTable dt = new DataTable();
            String[] StrCol = new String[] { "TYPEIDwithLevel", "SalesChannelTypeName" };
            ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
            ObjSalesChannel.UserID = PageBase.UserId;
            ObjSalesChannel.CompanyId = PageBase.ClientId;
            dt = ObjSalesChannel.GetSalesChannelTypeV3();
            PageBase.DropdownBinding(ref cmbsaleschanneltype, dt, StrCol);
            ViewState["SalesType"] = dt;


        };
    }
    void FillSalesChannelTypeRDApprovel()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            DataTable dt = new DataTable();
            String[] StrCol = new String[] { "TYPEIDwithLevel", "SalesChannelTypeName" };
            ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
            ObjSalesChannel.UserID = PageBase.UserId;
            dt = ObjSalesChannel.GetSalesChannelTypeV4();
            PageBase.DropdownBinding(ref cmbsaleschanneltype, dt, StrCol);
            ViewState["SalesType"] = dt;


        };
    }

    /*#CC18 Commented Started*/
    //void fillcountry()
    //{
    //    using (MastersData obj = new MastersData())
    //    {
    //        DataTable dt;
    //        cmbCountry.Items.Clear();
    //        obj.CountrySelectionMode = 1;
    //        dt = obj.SelectCountryInfo();
    //        String[] colArray = { "CountryID", "CountryName" };
    //        PageBase.DropdownBinding(ref cmbCountry, dt, colArray);
    //        SelectedCountryID = Convert.ToInt32(dt.Rows[0]["CountryID"]);
    //        CountryCount = Convert.ToInt32(dt.Rows[0]["CountryCount"]);
    //        //ViewState["CountryCount"] = dt.Rows[0]["CountryCount"];
    //        cmbstate.Items.Insert(0, new ListItem("Select", "0"));
    //    }

    //}
    /*#CC18 Commented End*/


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string[] s1 = cmbsaleschanneltype.SelectedValue.ToString().Split('/');
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }       //Pankaj Dhingra

            int Result = 0;
            string ErrorString = "";
            if (!ValidateControl(ref ErrorString))
            {
                ucMessage1.ShowInfo(ErrorString);
                return;
            }

            using (SalesChannelData objSalesChannel = new SalesChannelData())
            {
                if ((Request.QueryString["SalesChannelId"] != null) && (Request.QueryString["SalesChannelId"] != ""))
                {
                    SalesChannelId = Convert.ToInt32(Crypto.Decrypt(Request.QueryString["SalesChannelId"], PageBase.KeyStr));
                    objSalesChannel.SalesChannelID = SalesChannelId;
                }
                else if ((Request.QueryString["SalesChannelIdFromApproval"] != null) && (Request.QueryString["SalesChannelIdFromApproval"] != ""))
                {
                    SalesChannelId = Convert.ToInt32(Crypto.Decrypt(Request.QueryString["SalesChannelIdFromApproval"], PageBase.KeyStr));
                    objSalesChannel.SalesChannelID = SalesChannelId;
                }
                else
                {
                    objSalesChannel.SalesChannelID = 0;
                    //string StrPSalt = "";
                    //using (Authenticates ObjAuth = new Authenticates())
                    //{
                    //    StrPSalt = ObjAuth.GenerateSalt(txtpassword.Text.Trim().Length);
                    //    objSalesChannel.Password = ObjAuth.EncryptPassword(txtpassword.Text.Trim(), StrPSalt);

                    //};
                    //objSalesChannel.PasswordSalt = StrPSalt;
                }
                if (cmbparentsaleschannelgroup.Visible == true)
                {
                    objSalesChannel.GroupParentSalesChannelID = Convert.ToInt32(cmbparentsaleschannelgroup.SelectedValue);
                }
                /*New block added*/

                string StrPSalt = "";
                if (txtpassword.Text.Trim() != string.Empty)
                {
                    using (Authenticates ObjAuth = new Authenticates())
                    {
                        StrPSalt = ObjAuth.GenerateSalt(txtpassword.Text.Trim().Length);
                        objSalesChannel.Password = ObjAuth.EncryptPassword(txtpassword.Text.Trim(), StrPSalt);

                    }
                }
                objSalesChannel.PasswordSalt = StrPSalt;

                objSalesChannel.SalesChannelCode = txtsaleschannelcode.Text.Trim();
                objSalesChannel.SalesChannelName = txtsaleschannelname.Text.Trim();
                //objSalesChannel.SalesChannelTypeID = Convert.ToInt16(cmbsaleschanneltype.SelectedValue);
                objSalesChannel.SalesChannelTypeID = Convert.ToInt16(s1[0]);
                objSalesChannel.OrgnhierarchyID = Convert.ToInt16(cmborghierarchy.SelectedValue);
                objSalesChannel.ParentSalesChannelID = Convert.ToInt32(cmbparentsaleschannel.SelectedValue);
                objSalesChannel.Address1 = txtAddress1.Text.Trim();
                objSalesChannel.Address2 = txtAddress2.Text.Trim();
                objSalesChannel.StateID = Convert.ToInt16(cmbstate.SelectedValue);
                objSalesChannel.CityID = Convert.ToInt16(cmbcity.SelectedValue);
                objSalesChannel.AreaID = Convert.ToInt16(cmbArea.SelectedValue);
                objSalesChannel.CstNumber = txtcstno.Text.Trim();
                objSalesChannel.Fax = txtfax.Text.Trim();
                objSalesChannel.ContactPerson = txtcontactperson.Text.Trim();
                objSalesChannel.Mobile = txtmobile.Text.Trim();
                objSalesChannel.MobileNo2 = txtMobileNumber2.Text.Trim(); /* #CC03 Added */
                objSalesChannel.Phone = txtphone.Text.Trim();
                objSalesChannel.PinCode = txtpincode.Text.Trim();
                objSalesChannel.TinNumber = txttinno.Text.Trim();
                objSalesChannel.Status = chkactive.Checked;
                objSalesChannel.BusinessStartDate = Convert.ToDateTime(ucBuisnessstartdate.Date);
                objSalesChannel.Loginname = txtloginname.Text.Trim();
                objSalesChannel.Email = txtemail.Text.Trim();
                objSalesChannel.PanNumber = txtpanno.Text.Trim();
                //objSalesChannel.PasswordExpiryDays = Convert.ToInt16(Application["ExpiryDays"].ToString());
                objSalesChannel.PasswordExpiryDays = PasswordExp;

                objSalesChannel.UserID = PageBase.UserId;
                if (ucDOB.Date != "")
                    objSalesChannel.DateOfBirth = Convert.ToDateTime(ucDOB.Date);
                else
                    objSalesChannel.DateOfBirth = null;
                if (ucDOA.Date != "")
                    objSalesChannel.DateOfAnniversary = Convert.ToDateTime(ucDOA.Date);
                else
                    objSalesChannel.DateOfAnniversary = null;
                objSalesChannel.StrPassword = txtpassword.Text.Trim();
                objSalesChannel.OpeningStockDate = ucOpeningStock.Date == "" ? objSalesChannel.OpeningStockDate : Convert.ToDateTime(ucOpeningStock.Date);
                objSalesChannel.ApprovalRemarks = txtApprovalRemarks.Text.Trim();
                objSalesChannel.RejectionFlag = Convert.ToInt32(hdnApproveReject.Value);
                //int Panmandatory=((int)ViewState["Panmndatory"]);
                /*#CC10 start*/
                byte isPanNo = ((byte[])(ViewState["Panmndatory"]))[0];/* #CC13 Added */ /*((Byte)ViewState["Panmndatory"]); #CC13 Commented  */
                if ((txtpanno.Text.Trim() == "") && (isPanNo == 1))
                {
                    ucMessage1.ShowError("Pan Number Mandatory!");
                    return;
                }/*#CC10 end*/
                Result = objSalesChannel.InsertUpdateSalesChannel();
                if (Result > 0 && (objSalesChannel.Error == null || objSalesChannel.Error == ""))
                {
                    if (SalesChannelId == 0)
                    {
                        Session["MessageType"] = Resources.Messages.CreateSuccessfull + "-" + objSalesChannel.SuccessMsg;/*#CC12 Added*/
                        //SendMailToUser(Result);
                    }
                    else
                    {
                        Session["MessageType"] = Resources.Messages.EditSuccessfull;
                        Response.Redirect("ManageSalesChannel.aspx", false);
                    }
                    if ((Request.QueryString["SalesChannelIdFromApproval"] != null) && (Request.QueryString["SalesChannelIdFromApproval"] != ""))
                    {
                        Session["MessageType"] = Resources.Messages.EditSuccessfull;
                        Response.Redirect("ApproveSalesChannel.aspx?Message=" + Resources.Messages.EditSuccessfull);
                    }
                    else
                    {
                        Response.Redirect("ManageSalesChannel.aspx", false);
                    }
                }
                else
                {
                    if (objSalesChannel.Error != null && objSalesChannel.Error != "")
                    {
                        if (objSalesChannel.Error.ToLower().Contains("active user count"))
                        {
                            ucMessage1.ShowError("Active user count is exceeding the limit defined. Please Contact administrator.");
                        }
                        else
                            ucMessage1.ShowInfo(objSalesChannel.Error);
                    }
                    else
                    {
                        ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    }


                }



            };
        }
        catch (Exception ex)
        {
            /*#CC06 Added Started*/
            if (ex.Message.ToLower().Contains("trgusercount"))
            {
                ucMessage1.ShowError("Active user count is exceeding the limit defined. Please Contact administrator.");
            }
            else/*#CC06 Added End*/
                ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }

    private void SendMailToUser(int ChannelID)
    {
        try
        {
            DataTable dtSalesChannelInfo;
            using (SalesChannelData objSalesChannel = new SalesChannelData())
            {
                objSalesChannel.SalesChannelID = ChannelID;
                objSalesChannel.BlnShowDetail = true;
                dtSalesChannelInfo = objSalesChannel.GetSalesChannelInfo();
            };
            if (dtSalesChannelInfo != null && dtSalesChannelInfo.Rows.Count > 0)
            {
                string ErrDesc = string.Empty;
                string Password = string.Empty;
                using (Authenticates ObjAuth = new Authenticates())
                {
                    Password = ObjAuth.DecryptPassword(Convert.ToString(dtSalesChannelInfo.Rows[0]["Password"]), Convert.ToString(dtSalesChannelInfo.Rows[0]["PasswordSalt"]));
                };
                Mailer.LoginName = Convert.ToString(dtSalesChannelInfo.Rows[0]["LoginName"].ToString());
                Mailer.Password = Password;
                Mailer.EmailID = dtSalesChannelInfo.Rows[0]["Email"].ToString();
                Mailer.UserName = Convert.ToString(dtSalesChannelInfo.Rows[0]["SalesChannelName"].ToString());
                Mailer.sendmail("../../" + strAssets + "/Mailer/CreateUser.htm");
            }
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

        Response.Redirect("ManageSalesChannel.aspx");
        Session["MessageType"] = null;

    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        hdnApproveReject.Value = "1";
        /*#CC12 ADDED START*/
        if (txtApprovalRemarks.Text.Trim() == "") /* #CC13 Trim Added */
        {
            txtApprovalRemarks.Text = ""; /* #CC13 Added */
            ucMessage1.ShowInfo("Please enter rejection remarks.");
            return;
        }
        /*#CC12 ADDED END*/
        btnSubmit_Click(null, null);
    }
    protected void rdomaplocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rdomaplocation.SelectedValue == "1")
            {
                cmbparentsaleschannelgroup.Visible = true;
                lblparentnamegroup.Visible = true;
                lblreqparengroupt.Visible = true;
                using (SalesChannelData objSalesChannel = new SalesChannelData())
                {
                    objSalesChannel.SalesChanneType = EnumData.eSalesChannelType.SS;

                    String[] StrCol = new String[] { "SalesChannelID", "SalesChannelName" };
                    PageBase.DropdownBinding(ref cmbparentsaleschannelgroup, objSalesChannel.GetSalesChannelParentForGroup(), StrCol);

                };
            }
            else
            {
                cmbparentsaleschannelgroup.Visible = false;
                lblparentnamegroup.Visible = false;
                lblreqparengroupt.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    void ClearForm()
    {
        cmbsaleschanneltype.Enabled = true;
        cmbsaleschanneltype.SelectedValue = "0";
        cmbstate.SelectedValue = "0";
        cmbcity.SelectedValue = "0";
        cmbArea.SelectedValue = "0";
        cmborghierarchy.SelectedValue = "0";
        cmbparentsaleschannel.SelectedValue = "0";
        rdomaplocation.SelectedValue = "0";
        txtAddress1.Text = "";
        txtAddress2.Text = "";
        txtemail.Text = "";
        txtfax.Text = "";
        txtloginname.Text = "";
        txtcstno.Text = "";
        lblpassword.Visible = true;
        txtpassword.Visible = true;
        lblreqpassword.Visible = true;
        txtphone.Text = "";
        txtpincode.Text = "";
        txtpanno.Text = "";
        txtloginname.Enabled = true;
        txtsaleschannelcode.Text = "";
        txtsaleschannelname.Text = "";
        txtcontactperson.Text = "";
        btnSubmit.Text = "Submit";
        chkactive.Checked = true;
        ucBuisnessstartdate.Date = "";
        ucDOA.Date = "";
        ucDOB.Date = "";
        cmbparentsaleschannel.Items.Clear();
        cmbparentsaleschannel.Items.Insert(0, new ListItem("Select", "0"));
        cmborghierarchy.Items.Clear();
        cmborghierarchy.Items.Insert(0, new ListItem("Select", "0"));
        cmbArea.Items.Clear();
        cmbArea.Items.Insert(0, new ListItem("Select", "0"));
        cmbcity.Items.Clear();
        cmbcity.Items.Insert(0, new ListItem("Select", "0"));
        rdomaplocation.Visible = false;
        cmbparentsaleschannelgroup.Visible = false;
        lblmapparent.Visible = false;
        lblparentnamegroup.Visible = false;
        lblreqparengroupt.Visible = false;
        //lblHeading.Text = "Manage Sales Channel";

        //pnlpassword.Visible = true;
        //updpassword.Update();
        txtsaleschannelcode.Enabled = true;
        txtsaleschannelcode.Text = "";
        cmbparentsaleschannel.Enabled = true;
        txtmobile.Text = "";
        txttinno.Text = "";
        txtpassword.Text = "";
        ViewState["SalesType"] = null;


    }
    private bool ValidateControl(ref string ErrMessage)
    {


        ErrMessage = "";
        if ((cmbsaleschanneltype.SelectedValue == "0") ||
            (cmbstate.SelectedValue == "0") || (cmbcity.SelectedValue == "0") ||
           (txtloginname.Text.Trim() == "")
            || (txtsaleschannelcode.Text.Trim() == "") || (txtsaleschannelname.Text.Trim() == "") || (txtAddress1.Text.Trim() == "") || (txtemail.Text.Trim() == "") || (txtmobile.Text.Trim() == "")
            || (txtcontactperson.Text.Trim() == ""))
        {
            ErrMessage = Resources.Messages.MandatoryField;
            return false;

        }

        if (ServerValidation.IsMobileNo(txtmobile.Text.Trim(), true) > 0)
        {
            ErrMessage = Resources.Messages.Mobilevalidate;
            return false;

        }
        if (ServerValidation.IsValidEmail(txtemail.Text.Trim(), true) > 0)
        {
            ErrMessage = ErrMessage + Resources.Messages.Emailvalidate;

            return false;

        }

        if (ServerValidation.IsPinCode(txtpincode.Text.Trim(), false) > 0)
        {
            ErrMessage = ErrMessage + Resources.Messages.PinCodevalidate;
            return false;

        }
        if (((Request.QueryString["SalesChannelId"] == null) && (Request.QueryString["SalesChannelIdFromApproval"] == null)) ||
            ((Request.QueryString["SalesChannelId"] == "") && (Request.QueryString["SalesChannelIdFromApproval"] == null)))
        {
            if (txtpassword.Text.Trim() == "")
            {
                ErrMessage = ErrMessage + GetLocalResourceObject("Enterpassword").ToString();
                return false;

            }
        }


        if (txtloginname.Text.Trim() != "")
        {
            if (txtloginname.Text.Trim().Replace(" ", "").Length != txtloginname.Text.Trim().Length)
            {

                ErrMessage = ErrMessage + Resources.Messages.BlankSpaceNotAllow;

                return false;
            }
        }

        if (txtpassword.Text.Trim() != "")
        {
            if (txtpassword.Text.Trim().Replace(" ", "").Length != txtpassword.Text.Trim().Length)
            {
                ErrMessage = ErrMessage + Resources.Messages.BlankSpaceNotAllowPassword;

                return false;
            }
        }

        if (rdomaplocation.Visible == true && rdomaplocation.SelectedValue == "1")
        {
            //if (cmbparentsaleschannelgroup.SelectedValue == "")       //OldCondition
            if (cmbparentsaleschannelgroup.SelectedValue == "0")    //Pankaj Dhingra
            {
                ErrMessage = ErrMessage + GetLocalResourceObject("ParentGroupValidation").ToString();

                return false;
            }

        }
        if (ucBuisnessstartdate.Date != "")
        {
            if (Convert.ToDateTime(ucBuisnessstartdate.Date) > System.DateTime.Now)
            {
                ErrMessage = ErrMessage + Resources.Messages.DateRangeValidation;

                return false;
            }

        }
        if (ucDOA.Date != "")
        {
            if (Convert.ToDateTime(ucDOA.Date) > System.DateTime.Now)
            {
                ErrMessage = ErrMessage + Resources.Messages.DateRangeValidation;

                return false;
            }

        }
        if (ucDOB.Date != "")
        {
            if (Convert.ToDateTime(ucDOB.Date) > System.DateTime.Now)
            {
                ErrMessage = ErrMessage + Resources.Messages.DateRangeValidation;

                return false;
            }

        }
        /*if (cmbparentsaleschannel.Enabled == true)
        {
            if (cmbparentsaleschannel.SelectedValue == "0")
            {
                ErrMessage = ErrMessage + GetLocalResourceObject("ParentValidation").ToString();

                return false;
            }
        }*/
        if (cmborghierarchy.Items.Count > 1)
        {
            if (cmborghierarchy.SelectedValue == "0")
            {
                ErrMessage = ErrMessage + Resources.Messages.MandatoryField;

                return false;
            }
        }

        return true;
    }

    /*#CC07 start*/
    protected void cmbsaleschanneltype_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            if (cmbsaleschanneltype.SelectedValue.ToString() != "0")
            {
                string[] s = cmbsaleschanneltype.SelectedValue.ToString().Split('/');

                if (s[1].ToString() == "1")
                {
                    //fillcountry();/*#CC18 Commented*/
                    FillState();/*#CC18 Added*/
                    if (CountryCount == 4)
                    {
                        /*#CC18 Commented Started*/
                        //cmbCountry.SelectedValue = SelectedCountryID.ToString();
                        //cmbCountry_SelectedIndexChanged(cmbCountry, new EventArgs());
                        /*#CC18 Commented Started*/
                    }
                    else
                    {
                        cmbstate.Items.Clear();
                        cmbcity.Items.Clear();
                        cmbArea.Items.Clear();
                        cmbstate.Items.Insert(0, new ListItem("Select", "0"));
                        cmbcity.Items.Insert(0, new ListItem("Select", "0"));
                        cmbArea.Items.Insert(0, new ListItem("Select", "0"));
                    }
                    FillHierarchy();
                }
                else
                {

                    cmbstate.Items.Clear();
                    //cmbCountry.Items.Clear();  /*#CC18 Commented Started*/
                    cmbArea.Items.Clear();
                    cmbcity.Items.Clear();
                    //cmbCountry.Items.Insert(0, new ListItem("Select", "0"));  /*#CC18 Commented Started*/
                    cmbstate.Items.Insert(0, new ListItem("Select", "0"));
                    cmbArea.Items.Insert(0, new ListItem("Select", "0"));
                    cmbcity.Items.Insert(0, new ListItem("Select", "0"));
                    cmborghierarchy.Items.Clear();
                    cmborghierarchy.Items.Insert(0, new ListItem("Select", "0"));
                    // fillcountry();  /*#CC18 Commented Started*/

                    FillState(); /*#CC18 Added*/
                }

                if (s[1].ToString() == "1") // in case of warehouse vaildation not fired 
                    cmbparentsaleschannel.Enabled = false;
                else
                    cmbparentsaleschannel.Enabled = true;


                FillState();/*#CC18 Added*/

                using (SalesChannelData ObjSalesChannel = new SalesChannelData())
                {

                    String[] StrCol = new String[] { "SalesChannelID", "SalesChannelName" };
                    String[] StrCol1 = new String[] { "SalesChannelID", "NameWithCode" };
                    DataTable Dt = new DataTable();
                    ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(s[0]);
                    ObjSalesChannel.SalesChannelID = PageBase.SalesChanelID;
                    ObjSalesChannel.UserID = PageBase.UserId;/*#CC07*/
                    ObjSalesChannel.CompanyId = PageBase.ClientId;
                    Dt = ObjSalesChannel.GetSalesChannelParent();

                    PageBase.DropdownBinding(ref cmbparentsaleschannel, Dt, StrCol);

                    if (Dt != null && Dt.Rows.Count > 0)
                    {
                        cmbparentsaleschannel.Enabled = true;
                    }
                    else
                    {
                        cmbparentsaleschannel.Enabled = false;
                    }

                    if (Convert.ToInt16(s[0]) == Convert.ToInt16(EnumData.eSalesChannelType.SS))
                    {
                        lblmapparent.Visible = true;
                        rdomaplocation.Visible = true;
                        rdomaplocation.SelectedValue = "0";

                    }
                    else
                    {
                        lblmapparent.Visible = false;
                        rdomaplocation.Visible = false;
                        lblparentnamegroup.Visible = false;
                        cmbparentsaleschannelgroup.Visible = false;
                        lblreqparengroupt.Visible = false;

                    }

                    if (s[1].ToString() == "1") /* #CC11 Added */
                    {
                        String[] StrCol2 = new String[] { "OrgnhierarchyID", "LocationName" };
                        PageBase.DropdownBinding(ref cmborghierarchy, ObjSalesChannel.GetSalesChannelOrghierarchy(), StrCol2);
                    }

                    if (ViewState["SalesType"] != null)
                    {
                        DataTable DtsalesType = (DataTable)(ViewState["SalesType"]);

                        DataRow[] drow = DtsalesType.Select("SalesChannelTypeID=" + cmbsaleschanneltype.SelectedValue);
                        if (drow.Length > 0)
                        {
                            if (((DataRow)drow.GetValue(0))["SalesChannelLevel"] != DBNull.Value && Convert.ToInt32(((DataRow)drow.GetValue(0))["SalesChannelLevel"]) > 2)
                            {
                                txtsaleschannelcode.Enabled = false;
                                txtsaleschannelcode.Text = "Autogenerated";
                            }
                            else
                            {
                                txtsaleschannelcode.Enabled = true;
                                txtsaleschannelcode.Text = "";

                            }
                        }
                    }
                    if (s[0].ToString() != "0")
                    {
                        if (ViewState["SalesType"] != null)
                        {
                            DataTable dt = (DataTable)ViewState["SalesType"];
                            byte[] isautogen = (from dr in dt.AsEnumerable()
                                                where (byte)dr["SalesChannelTypeID"] == Convert.ToByte(s[0])

                                                select ((Byte)dr["IsAutoGenerate"])).ToArray<Byte>();
                            // Provide saleschannel type with dynamic interface for allowing the auto generate machanism or not accoring to the user 


                            /*#CC10 start*/
                            byte[] isPanNo = (from dr in dt.AsEnumerable()
                                              where (byte)dr["SalesChannelTypeID"] == Convert.ToByte(s[0])

                                              select ((Byte)dr["IsPanMandatory"])).ToArray<Byte>();
                            ViewState["Panmndatory"] = isPanNo;
                            //if ((isPanNo[0] == 1) && (txtpanno.Text == ""))
                            //{
                            //    ucMessage1.ShowError("Pan Number Is Mandatory");
                            //    return;
                            //}/*#CC10 end*/

                            if (isautogen[0] == 1)                                                    // By Saurabh Tyagi
                            {
                                txtsaleschannelcode.Enabled = false;
                                txtsaleschannelcode.Text = "Autogenerated";
                            }

                            else
                            {
                                txtsaleschannelcode.Enabled = true;
                                txtsaleschannelcode.Text = "";

                            }
                        }
                    }
                    else
                    {
                        txtsaleschannelcode.Enabled = true;
                        txtsaleschannelcode.Text = "";
                    }


                    if ((Convert.ToInt16(s[1]) == Convert.ToInt16(EnumData.eSalesChannelType.TD)) || (Convert.ToInt16(s[1]) == Convert.ToInt16(EnumData.eSalesChannelType.DirectTD)))
                    {


                    }
                    else
                    {

                    }

                };

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    /*#CC07 End*/

    protected void cmbstate_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            using (GeographyData ObjGeography = new GeographyData())
            {
                if (cmbstate.SelectedIndex > 0)             //Pankaj Kumar
                {
                    ObjGeography.SearchCondition = EnumData.eSearchConditions.Active;
                    ObjGeography.StateId = Convert.ToInt16(cmbstate.SelectedValue);
                    String[] StrCol = new String[] { "CityId", "CityName" };
                    cmbcity.Items.Clear();
                    PageBase.DropdownBinding(ref cmbcity, ObjGeography.GetAllCityByParameters(), StrCol);
                    cmbArea.Items.Clear();
                    cmbArea.Items.Insert(0, new ListItem("Select", "0"));
                    updpnlCity.Update();
                    updpnlArea.Update();
                }
                else if (cmbstate.SelectedIndex == 0)           //Pankaj Kumar
                {
                    cmbcity.Items.Clear();
                    cmbcity.Items.Insert(0, new ListItem("Select", "0"));
                    cmbArea.Items.Clear();
                    cmbArea.Items.Insert(0, new ListItem("Select", "0"));
                    updpnlCity.Update();
                    updpnlArea.Update();
                }

            };
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }

    protected void cmbcity_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            using (GeographyData ObjGeography = new GeographyData())
            {
                if (cmbcity.SelectedIndex > 0)  //Pankaj Kumar
                {
                    ObjGeography.SearchCondition = EnumData.eSearchConditions.Active;
                    ObjGeography.CityId = Convert.ToInt16(cmbcity.SelectedValue);
                    cmbArea.Items.Clear();
                    String[] StrCol = new String[] { "AreaID", "AreaName" };
                    PageBase.DropdownBinding(ref cmbArea, ObjGeography.GetAllAreaByParameters(), StrCol);
                    updpnlArea.Update();
                }
                else if (cmbcity.SelectedIndex == 0)    //Pankaj Kumar
                {
                    cmbArea.Items.Clear();
                    cmbArea.Items.Insert(0, new ListItem("Select", "0"));
                    updpnlArea.Update();
                }

            };
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void LBViewSalesChannel_Click(object sender, EventArgs e)
    {

        if ((Request.QueryString["SalesChannelIdFromApproval"] != null) && (Request.QueryString["SalesChannelIdFromApproval"] != ""))
        {
            Response.Redirect("ApproveSalesChannel.aspx");
        }
        else
        {
            //Server.Transfer("ViewSalesChannel.aspx"); #CC17 comented
            Response.Redirect("ViewSalesChannel.aspx");//#CC17 Added
        }
    }
    void FillHierarchy()
    {


        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            if (cmbsaleschanneltype.SelectedValue.ToString() != "0")
            {
                string[] s = cmbsaleschanneltype.SelectedValue.ToString().Split('/');

                if (s[1].ToString() != "1")
                {
                    ObjSalesChannel.ParentSalesChannelID = Convert.ToInt32(cmbparentsaleschannel.SelectedValue);
                }
                else
                {
                    ObjSalesChannel.ParentSalesChannelID = 0;
                }
                ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(s[0]);
                ObjSalesChannel.CompanyId = PageBase.ClientId;
                String[] StrCol1 = new String[] { "OrgnhierarchyID", "LocationName" };
                PageBase.DropdownBinding(ref cmborghierarchy, ObjSalesChannel.GetSalesChannelOrghierarchy(), StrCol1);

            }
        }



    }
    /*#CC18 Commented Started*/
    //protected void cmbCountry_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    cmbcity.Items.Clear();
    //    cmbcity.Items.Insert(0, new ListItem("Select", "0"));
    //    cmbArea.Items.Clear();
    //    cmbArea.Items.Insert(0, new ListItem("Select", "0"));
    //    
    //    //if (cmbCountry.SelectedValue == "0")
    //    //{
    //    //    cmbstate.Items.Clear();
    //    //    cmbstate.Items.Insert(0, new ListItem("Select", "0"));
    //    //}
    //    //else
    //    //{
    //    
    //        //cmbstate.Items.Clear();
    //        //using (MastersData obj = new MastersData())
    //        //{
    //        //    DataTable dt;
    //        //    obj.StateSelectionMode = 1;
    //        //    obj.StateCountryid = Convert.ToInt32(cmbCountry.SelectedValue);
    //        //    if (cmbparentsaleschannel.Items.Count==0)
    //        //    {
    //        //        obj.SalesChannelID = 0;
    //        //    }
    //        //    obj.SalesChannelID = Convert.ToInt32(cmbparentsaleschannel.SelectedValue); /* temp code added */
    //        //    dt = obj.SelectStateInfo();
    //        //    String[] colArray = { "StateID", "StateName" };
    //        //    PageBase.DropdownBinding(ref cmbstate, dt, colArray);
    //        //    updState.Update();
    //        //}
    //   // }
    //}
    /*#CC18 Commented End*/

    /*#CC18 Added Started*/
    private void FillState()
    {
        cmbcity.Items.Clear();
        cmbcity.Items.Insert(0, new ListItem("Select", "0"));
        cmbArea.Items.Clear();
        cmbArea.Items.Insert(0, new ListItem("Select", "0"));
        cmbstate.Items.Clear();
        using (MastersData obj = new MastersData())
        {
            DataTable dt;
            obj.StateSelectionMode = 1;
            obj.StateCountryid = 0;
            if (cmbparentsaleschannel.Items.Count == 0)
            {
                obj.SalesChannelID = 0;
            }
            obj.SalesChannelID = Convert.ToInt32(cmbparentsaleschannel.SelectedValue); /* temp code added */
            obj.CompanyId = PageBase.ClientId;
            dt = obj.SelectStateInfo();
            String[] colArray = { "StateID", "StateName" };
            PageBase.DropdownBinding(ref cmbstate, dt, colArray);
            updState.Update();
        }

    }
    /*#CC18 Added End*/
    protected void cmbparentsaleschannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillParentSalesChannelAreainformation();
        FillHierarchy();

    }
    void FillParentSalesChannelAreainformation()
    {
        try
        {
            DataSet ds = new DataSet();
            using (MastersData obj = new MastersData())
            {
                //cmbCountry.Items.Clear();  /*#CC18 Commented Started*/
                obj.CountrySelectionMode = 1;
                obj.SalesChannelID = Convert.ToInt32(cmbparentsaleschannel.SelectedValue);
                ds = obj.SelectSalesChannelAreainformation();
                if (ds != null && ds.Tables.Count > 0)
                {
                    String[] colArray = { "CountryID", "CountryName" };
                    // PageBase.DropdownBinding(ref cmbCountry, ds.Tables[0], colArray); /*#CC18 Commented Started*/
                    cmbstate.Items.Insert(0, new ListItem("Select", "0"));
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    /*#CC18 Commented Started*/
                    //SelectedCountryID = Convert.ToInt32(ds.Tables[1].Rows[0]["CountryID"]);

                    //if (cmbCountry.Items.FindByValue(SelectedCountryID.ToString()) != null)
                    //{
                    //    cmbCountry.SelectedValue = SelectedCountryID.ToString();
                    //    cmbCountry_SelectedIndexChanged(cmbCountry, new EventArgs());
                    StateID = Convert.ToInt32(ds.Tables[1].Rows[0]["StateID"]);
                    cmbstate.SelectedValue = StateID.ToString();
                    cmbstate_SelectedIndexChanged(cmbstate, new EventArgs());
                    //} /*#CC18 Commented End*/
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void ChkWantToSubmitOpeningStock_CheckedChanged(object sender, EventArgs e)
    {
        //if (ChkWantToSubmitOpeningStock.Checked)
        //{
        //    ucOpeningStock.Visible = true;

        //    FillDate();
        //}
        //else
        //{
        //    ucOpeningStock.Visible = false;
        //    ucOpeningStock.Date = "";
        //    ucOpeningStock.IsRequired = false;
        //    ucOpeningStock.ValidationGroup = "NotRequired";
        //}
    }
    void FillDate()
    {
        if (PageBase.BackDaysAllowedOpeningStock == 0 /*#CC01 ADDED START*/ && Session["DefaultOpStockDate"].ToString() != "-1" /*#CC01 ADDED END*/)
        {
            ucOpeningStock.Date = DateTime.Now.Date.ToString();
            ucOpeningStock.TextBoxDate.Enabled = false;
            ucOpeningStock.imgCal.Enabled = false;
        }
        else
        {

            ucOpeningStock.TextBoxDate.Enabled = true;
            ucOpeningStock.imgCal.Enabled = true;
            ucOpeningStock.MinRangeValue = DateTime.Now.Date.AddDays(-PageBase.BackDaysAllowedOpeningStock);
            ucOpeningStock.MaxRangeValue = DateTime.Now.Date;
            ucOpeningStock.RangeErrorMessage = "Only " + PageBase.BackDaysAllowedOpeningStock + " Back days allowed";
            ucOpeningStock.ValidationGroup = "Add";
        }
    }
    /*#CC18 Added Started*/
    protected void Rbtdownloadtemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        if (Rbtdownloadtemplate.SelectedValue == "1")
        {
            ForSaveTemplateheading.Visible = true;
            ForSaveTemplatedownload.Visible = true;
            ReferenceIdForsaveheading.Visible = true;
            ReferenceIdForsave.Visible = true;

        }
        else
        {
            ForSaveTemplateheading.Visible = false;
            ForSaveTemplatedownload.Visible = false;
            ReferenceIdForsaveheading.Visible = false;
            ReferenceIdForsave.Visible = false;
        }
        if (Rbtdownloadtemplate.SelectedValue == "2")
        {
            ForUploadTemplateheading.Visible = true;
            ForUpdateTemplatedownload.Visible = true;
            ReferenceIdForupdateheading.Visible = true;
            ReferenceIdForupdate.Visible = true;

        }
        else
        {
            ForUploadTemplateheading.Visible = false;
            ForUpdateTemplatedownload.Visible = false;
            ReferenceIdForupdateheading.Visible = false;
            ReferenceIdForupdate.Visible = false;

        }
    }
    protected void DwnldTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (CommonMaster objSalesData = new CommonMaster())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.CompanyID = PageBase.ClientId;
                objSalesData.TemplateType = 5;
                objSalesData.TemplateName = "SaveSalesChannel";
                dsTemplateCode = objSalesData.GetMaterialMasterTemplate();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "SaveSalesChannel";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "SaveSalesChannel" };
                    ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, 1);
                    if (dsTemplateCode.Tables.Count > 1)
                        dsTemplateCode.Tables.RemoveAt(1);
                    for (int i = dsTemplateCode.Tables[0].Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = dsTemplateCode.Tables[0].Rows[i];

                        dsTemplateCode.Tables[0].Rows.Remove(dr);

                    }

                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsTemplateCode, FilenameToexport, 1, strExcelSheetName);
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

        }
    }
    protected void UpdateTemplateFile_Click(object sender, EventArgs e)
    {


        string Password = string.Empty;
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (CommonMaster objSalesData = new CommonMaster())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.CompanyID = PageBase.ClientId;
                objSalesData.TemplateType = 6;
                objSalesData.TemplateName = "UpdateSalesChannel";
                dsTemplateCode = objSalesData.GetMaterialMasterTemplate();

                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "UpdateSalesChannel";
                PageBase.RootFilePath = FilePath;
                string[] strExcelSheetName = { "UpdateSalesChannel" };
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {  
                    ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, 1);
                    DataTable dt = dsTemplateCode.Tables[0].Copy();
                    if (dt.Rows.Count > 0 && dt.Columns.Contains("PasswordSalt"))
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
                        ZedService.Utility.ZedServiceUtil.ExportToExecl(dtcopy, FilenameToexport, 1, strExcelSheetName);
                    }
                    else
                    {
                        
                        ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, 1);
                        if (dsTemplateCode.Tables.Count > 1)
                            dsTemplateCode.Tables.RemoveAt(1);
                        for (int i = dsTemplateCode.Tables[0].Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = dsTemplateCode.Tables[0].Rows[i];

                            dsTemplateCode.Tables[0].Rows.Remove(dr);

                        }

                        ZedService.Utility.ZedServiceUtil.ExportToExecl(dsTemplateCode, FilenameToexport, 1, strExcelSheetName);
                    }
                   
                }
                else
                {
                    if (!string.IsNullOrEmpty( objSalesData.OutError))
                        ucMessage1.ShowError(Resources.Messages.NoRecord);
                    else
                        ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());

        }
    }
    protected void DownloadReferenceCodeForSave_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (CommonMaster objSalesData = new CommonMaster())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.CompanyID = PageBase.ClientId;
                objSalesData.TemplateType = 6;
                dsTemplateCode = objSalesData.GetMaterialMasterReferenceData();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "ReferenceCode";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "SalesChannelTypeDetail", "StateCityDetail", "ParentSalesChannelDetail", "ParentHierarchyDetail", "EntityMappingTypeRelationDetail" };
                    ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, 5);
                    if (dsTemplateCode.Tables.Count > 5)
                        dsTemplateCode.Tables.RemoveAt(5);
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsTemplateCode, FilenameToexport, 5, strExcelSheetName);
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

        }
    }
    protected void DownloadReferenceCodeForUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (CommonMaster objSalesData = new CommonMaster())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.CompanyID = PageBase.ClientId;
                objSalesData.TemplateType = 6;
                dsTemplateCode = objSalesData.GetMaterialMasterReferenceData();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "ReferenceCode";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "SalesChannelTypeDetail", "StateCityDetail", "ParentSalesChannelDetail", "ParentHierarchyDetail", "EntityMappingTypeRelationDetail" };
                    ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, 5);
                    if (dsTemplateCode.Tables.Count > 5)
                        dsTemplateCode.Tables.RemoveAt(5);
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsTemplateCode, FilenameToexport, 5, strExcelSheetName);
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

        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsPageRefereshed)
            {
                return;

            }
            DataTable dtError = new DataTable();
            HttpContext.Current.Session["PkeyColumns"] = null;
            string strKey = string.Empty;
            hlnkInvalid.Visible = false;

            Int16 Upload = 0;
            Upload = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            ViewState["TobeUploaded"] = strUploadedFileName;
            if (Upload == 1)
            {
                OpenXMLExcel objexcel = new OpenXMLExcel();
                DataSet DsExcel = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);
                if (DsExcel != null && DsExcel.Tables.Count > 0 && DsExcel.Tables[0].Rows.Count > 0)
                {

                    if (DsExcel.Tables[0].Rows.Count > Convert.ToInt32(PageBase.ValidExcelRows))
                    {
                        ucMessage1.ShowInfo("Limit Crossed");
                    }
                    else if (DsExcel.Tables[0].Columns.Contains("SalesChannelID") && Rbtdownloadtemplate.SelectedValue == "1")
                    {
                        ucMessage1.ShowInfo("You are uploading an update template, please upload save template.");
                        return;
                    }
                    else if (!DsExcel.Tables[0].Columns.Contains("SalesChannelID") && Rbtdownloadtemplate.SelectedValue == "2")
                    {
                        ucMessage1.ShowInfo("You are uploading an save template, please upload update template.");
                        return;
                    }


                    if (Rbtdownloadtemplate.SelectedValue == "1")
                    {
                        ValidateUploadFile objValidateFile = new ValidateUploadFile();
                        {
                            DataSet objDS = DsExcel;
                            DataTable dt1 = DsExcel.Tables[0];
                            SortedList objSL = new SortedList();
                            SortedList objSLCorrData = new SortedList();
                            objValidateFile.UploadedFileName = strUploadedFileName;
                            objValidateFile.ExcelFileNameInTable = "SaveSalesChannel";
                            objValidateFile.CompanyId = PageBase.ClientId;
                            objValidateFile.ValidateFileWithCompanyId(false, out objDS, out objSL);
                            if (objValidateFile.Message != null && objValidateFile.Message.Trim() != "")
                                ucMessage1.ShowInfo(objValidateFile.Message);
                            else
                            {
                                ucMessage1.Visible = false;
                                bool blnIsUpload = true;
                                if (objSL.Count >= 1 && objSL.Keys.Count >= 1)
                                {
                                    objDS.Tables["DtExcelSheet"].Columns.Add(new DataColumn("ReasonForInvalid"));
                                    IDictionaryEnumerator objIDicEnum = objSL.GetEnumerator();
                                    while (objIDicEnum.MoveNext())
                                    {
                                        string[] strpkeyColumnName = Convert.ToString(HttpContext.Current.Session["PkeyColumns"]).Split(',');
                                        if (HttpContext.Current.Session["PkeyColumns"] != null)
                                        {
                                            for (int i = 0; i <= objDS.Tables["DtExcelSheet"].Rows.Count - 1; i++)
                                            {
                                                strKey = string.Empty;
                                                for (int j = 0; j <= strpkeyColumnName.Length - 1; j++)
                                                {
                                                    if (strKey == string.Empty)
                                                        strKey = objDS.Tables["DtExcelSheet"].Rows[i][strpkeyColumnName[j]].ToString();
                                                    else
                                                        strKey = strKey + objDS.Tables["DtExcelSheet"].Rows[i][strpkeyColumnName[j]].ToString();
                                                }
                                                if (strKey == objIDicEnum.Key.ToString())
                                                {
                                                    objDS.Tables["DtExcelSheet"].Rows[i]["ReasonForInvalid"] = objIDicEnum.Value.ToString();
                                                }
                                            }
                                        }
                                    }
                                    objDS.Tables[0].AcceptChanges();
                                    if (objDS.Tables["DtExcelSheet"] != null && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                                    {
                                        hlnkInvalid.Visible = true;
                                        dsErrorProne.Merge(objDS.Tables["DtExcelSheet"]);
                                        string strFileName = "InvalidData" + DateTime.Now.Ticks;
                                        ExportInExcel(dsErrorProne, strFileName);
                                        hlnkInvalid.Text = "Invalid Data";
                                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                        blnIsUpload = false;
                                    }
                                    blnIsUpload = false;
                                }
                                if (!objDS.Tables[0].Columns.Contains("ReasonForInvalid"))
                                {
                                    int counter = 0;
                                    if (!objDS.Tables[0].Columns.Contains("ReasonForInvalid"))
                                        objDS.Tables[0].Columns.Add(new DataColumn("ReasonForInvalid"));

                                    if (counter > 0)
                                    {
                                        ucMessage1.ShowInfo("Invalid Records");
                                        dsErrorProne.Merge(objDS.Tables[0]);
                                        blnIsUpload = false;
                                    }
                                    else
                                    {
                                        objDS.Tables[0].Columns.Remove("ReasonForInvalid");
                                    }
                                }

                                if (objDS.Tables["DtDuplicateRecord"] != null && objDS.Tables["DtDuplicateRecord"].Rows.Count > 0)
                                {

                                    dsErrorProne.Merge(objDS.Tables["DtDuplicateRecord"]);
                                    blnIsUpload = false;
                                }
                                if (objDS.Tables["DtBlankData"] != null && objDS.Tables["DtBlankData"].Rows.Count > 0)
                                {

                                    dsErrorProne.Merge(objDS.Tables["DtBlankData"]);

                                    blnIsUpload = false;
                                }
                                if (blnIsUpload)
                                {
                                    if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                                    {
                                        InsertData(objDS.Tables[0]);
                                    }
                                    else
                                        ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                                }
                                else
                                {
                                    hlnkInvalid.Visible = true;
                                    string strFileName = "InvalidData" + DateTime.Now.Ticks;
                                    ExportInExcel(dsErrorProne, strFileName);
                                    hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                    hlnkInvalid.Text = "Invalid Data";
                                }
                            }
                        }
                    }
                    else if (Rbtdownloadtemplate.SelectedValue == "2")
                    {
                        ValidateUploadFile objValidateFile = new ValidateUploadFile();
                        {
                            DataSet objDS = DsExcel;
                            DataTable dt1 = DsExcel.Tables[0];
                            SortedList objSL = new SortedList();
                            SortedList objSLCorrData = new SortedList();
                            objValidateFile.UploadedFileName = strUploadedFileName;
                            objValidateFile.ExcelFileNameInTable = "UpdateSalesChannel";
                            objValidateFile.ValidateFileWithCompanyId(false, out objDS, out objSL);
                            if (objValidateFile.Message != null && objValidateFile.Message.Trim() != "")
                                ucMessage1.ShowInfo(objValidateFile.Message);
                            else
                            {
                                ucMessage1.Visible = false;
                                bool blnIsUpload = true;
                                if (objSL.Count >= 1 && objSL.Keys.Count >= 1)
                                {
                                    objDS.Tables["DtExcelSheet"].Columns.Add(new DataColumn("ReasonForInvalid"));
                                    IDictionaryEnumerator objIDicEnum = objSL.GetEnumerator();
                                    while (objIDicEnum.MoveNext())
                                    {
                                        string[] strpkeyColumnName = Convert.ToString(HttpContext.Current.Session["PkeyColumns"]).Split(',');
                                        if (HttpContext.Current.Session["PkeyColumns"] != null)
                                        {
                                            for (int i = 0; i <= objDS.Tables["DtExcelSheet"].Rows.Count - 1; i++)
                                            {
                                                strKey = string.Empty;
                                                for (int j = 0; j <= strpkeyColumnName.Length - 1; j++)
                                                {
                                                    if (strKey == string.Empty)
                                                        strKey = objDS.Tables["DtExcelSheet"].Rows[i][strpkeyColumnName[j]].ToString();
                                                    else
                                                        strKey = strKey + objDS.Tables["DtExcelSheet"].Rows[i][strpkeyColumnName[j]].ToString();
                                                }
                                                if (strKey == objIDicEnum.Key.ToString())
                                                {
                                                    objDS.Tables["DtExcelSheet"].Rows[i]["ReasonForInvalid"] = objIDicEnum.Value.ToString();
                                                }
                                            }
                                        }
                                    }
                                    objDS.Tables[0].AcceptChanges();
                                    if (objDS.Tables["DtExcelSheet"] != null && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                                    {
                                        hlnkInvalid.Visible = true;
                                        dsErrorProne.Merge(objDS.Tables["DtExcelSheet"]);
                                        string strFileName = "InvalidData" + DateTime.Now.Ticks;
                                        ExportInExcel(dsErrorProne, strFileName);
                                        hlnkInvalid.Text = "Invalid Data";
                                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                        blnIsUpload = false;
                                    }
                                    blnIsUpload = false;
                                }
                                if (!objDS.Tables[0].Columns.Contains("ReasonForInvalid"))
                                {
                                    int counter = 0;
                                    if (!objDS.Tables[0].Columns.Contains("ReasonForInvalid"))
                                        objDS.Tables[0].Columns.Add(new DataColumn("ReasonForInvalid"));

                                    if (counter > 0)
                                    {
                                        ucMessage1.ShowInfo("Invalid Records");
                                        dsErrorProne.Merge(objDS.Tables[0]);
                                        blnIsUpload = false;
                                    }
                                    else
                                    {
                                        objDS.Tables[0].Columns.Remove("ReasonForInvalid");
                                    }
                                }

                                if (objDS.Tables["DtDuplicateRecord"] != null && objDS.Tables["DtDuplicateRecord"].Rows.Count > 0)
                                {

                                    dsErrorProne.Merge(objDS.Tables["DtDuplicateRecord"]);
                                    blnIsUpload = false;
                                }
                                if (objDS.Tables["DtBlankData"] != null && objDS.Tables["DtBlankData"].Rows.Count > 0)
                                {

                                    dsErrorProne.Merge(objDS.Tables["DtBlankData"]);

                                    blnIsUpload = false;
                                }
                                if (blnIsUpload)
                                {
                                    if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                                    {
                                        InsertData(objDS.Tables[0]);
                                    }
                                    else
                                        ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                                }
                                else
                                {
                                    hlnkInvalid.Visible = true;
                                    string strFileName = "InvalidData" + DateTime.Now.Ticks;
                                    ExportInExcel(dsErrorProne, strFileName);
                                    hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                    hlnkInvalid.Text = "Invalid Data";
                                }
                            }
                        }
                    }
                }
                else
                {
                    ucMessage1.ShowInfo("File is empty! Some Mandatory columns has no required data!");
                }
            }
            else
            {
                ucMessage1.Visible = true;
                ucMessage1.ShowInfo("Please Browse File !");
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    private void InsertData(DataTable objdt)
    {
        if (objdt != null)
        {
            DataSet objds = new DataSet();

            if (objdt != null && objdt.Rows.Count > 0)
            {

                try
                {
                    if (Rbtdownloadtemplate.SelectedValue == "1")
                    {
                        if (ViewState["TobeUploaded"] != null)
                        {
                            OpenXMLExcel objexcel = new OpenXMLExcel();
                            string strUploadedFileNameFromViewState = ViewState["TobeUploaded"].ToString();
                            DataSet DsXML = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + ViewState["TobeUploaded"].ToString());
                            string guid = Guid.NewGuid().ToString();
                            ViewState[strPrimarySessionName] = guid;          
                            DsXML.Tables[0].Columns.Add(AddColumn(guid, "TransactionUploadSessionId", typeof(System.String)));
                            DataTable dtUploadQueue = DsXML.Tables[0].Copy();
                            dtUploadQueue.Columns.Add(new DataColumn("PasswordSalt", typeof(System.String)));
                            if (dtUploadQueue.Rows.Count > 0)
                            {
                                
                                foreach (DataRow dr in dtUploadQueue.Rows)
                                {
                                    if (Convert.ToString(dr["Password"]).Length>0)
                                    {
                                        using (Authenticates ObjAuth = new Authenticates())
                                        {
                                            dr["PasswordSalt"] = ObjAuth.GenerateSalt(dr["Password"].ToString().Trim().Length);
                                            dr["Password"] = ObjAuth.EncryptPassword(dr["Password"].ToString(), dr["PasswordSalt"].ToString().Trim());
                                        };
                                    }
                                }
                            }
                            if (dtUploadQueue.Rows.Count > 0)
                            {
                                if (!UserDetailBCP(dtUploadQueue))
                                {
                                    ucMessage1.ShowError("Error Occured While transferring the data to the server");
                                    return;
                                }
                            }

                            using (CommonMaster objDetail = new CommonMaster())
                            {
                                Authenticates ObjAuth = new Authenticates();
                                objDetail.UserID = PageBase.UserId;
                                objDetail.SessionId = guid;
                                objDetail.CompanyID = PageBase.ClientId;
                                strPSalt = ObjAuth.GenerateSalt(strDefaultPassword.Trim().Length);
                                strPassword = ObjAuth.EncryptPassword(strDefaultPassword, strPSalt.Trim());
                                objDetail.Password = strPassword;
                                objDetail.PasswordSalt = strPSalt;
                                objDetail.TemplateType = Convert.ToInt16(Rbtdownloadtemplate.SelectedValue);
                                DataSet dtInvalidRecordSet = objDetail.UploadSalesChannelWithUserSave();
                                Int32 result = objDetail.Out_Param;
                                if (result == 0)
                                {
                                    if (result == 0 && objDetail.OutError != "")
                                    {
                                        ucMessage1.ShowError(objDetail.OutError);
                                        return;
                                    }
                                    else
                                    {
                                        ucMessage1.ShowSuccess("Data Uploaded Successfully");
                                        return;
                                    }
                                }
                                if (result == 1 && objDetail.OutError != "")
                                {
                                    ucMessage1.ShowError(objDetail.OutError);
                                    return;
                                }
                                if (result == 1 && dtInvalidRecordSet != null && dtInvalidRecordSet.Tables[0].Rows.Count > 0)
                                {
                                    DataSet ds = new DataSet();
                                    string strFileName = "Invalid Data" + DateTime.Now.Ticks;
                                    ExportInExcel(dtInvalidRecordSet, strFileName);
                                    hlnkInvalid.Visible = true;
                                    hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                    hlnkInvalid.Text = "Invalid Data";
                                    ucMessage1.Visible = true;
                                    ucMessage1.ShowInfo("Please click on Invalid data to check the error obtained");

                                }
                            }


                        }
                    }
                    else if (Rbtdownloadtemplate.SelectedValue == "2")
                    {
                        if (ViewState["TobeUploaded"] != null)
                        {
                            OpenXMLExcel objexcel = new OpenXMLExcel();
                            string strUploadedFileNameFromViewState = ViewState["TobeUploaded"].ToString();
                            DataSet DsXML = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + ViewState["TobeUploaded"].ToString());
                            string guid = Guid.NewGuid().ToString();
                            ViewState[strPrimarySessionNameupdate] = guid;
                            
                            DsXML.Tables[0].Columns.Add(AddColumn(guid, "TransactionUploadSessionId", typeof(System.String)));
                            DataTable dtUploadQueue = DsXML.Tables[0].Copy();
                            dtUploadQueue.Columns.Add(new DataColumn("PasswordSalt", typeof(System.String)));
                            if (dtUploadQueue.Rows.Count > 0)
                            {

                                foreach (DataRow dr in dtUploadQueue.Rows)
                                {
                                    if (Convert.ToString(dr["Password"]).Length > 0)
                                    {
                                        using (Authenticates ObjAuth = new Authenticates())
                                        {
                                            dr["PasswordSalt"] = ObjAuth.GenerateSalt(dr["Password"].ToString().Trim().Length);
                                            dr["Password"] = ObjAuth.EncryptPassword(dr["Password"].ToString(), dr["PasswordSalt"].ToString().Trim());
                                        };
                                    }
                                }
                            }
                            if (dtUploadQueue.Rows.Count > 0)
                            {
                                if (!UserDetailBCP(dtUploadQueue))
                                {
                                    ucMessage1.ShowError("Error Occured While transferring the data to the server");
                                    return;
                                }
                            }

                            using (CommonMaster objDetail = new CommonMaster())
                            {
                                Authenticates ObjAuth = new Authenticates();
                                objDetail.UserID = PageBase.UserId;
                                objDetail.SessionId = guid;
                                objDetail.CompanyID = PageBase.ClientId;
                                strPSalt = ObjAuth.GenerateSalt(strDefaultPassword.Trim().Length);
                                strPassword = ObjAuth.EncryptPassword(strDefaultPassword, strPSalt.Trim());
                                objDetail.Password = strPassword;
                                objDetail.PasswordSalt = strPSalt;
                                objDetail.TemplateType = Convert.ToInt16(Rbtdownloadtemplate.SelectedValue);
                                DataSet dtInvalidRecordSet = objDetail.UploadSalesChannelWithUserUpdate();
                                Int32 result = objDetail.Out_Param;
                                if (result == 0)
                                {
                                    ucMessage1.ShowSuccess("Data Update Successfully");
                                    return;
                                }
                                if (result == 1 && objDetail.OutError != "")
                                {
                                    ucMessage1.ShowError(objDetail.OutError);
                                    return;
                                }
                                if (result == 1 && dtInvalidRecordSet != null && dtInvalidRecordSet.Tables[0].Rows.Count > 0)
                                {
                                    DataSet ds = new DataSet();
                                    string strFileName = "Invalid Data" + DateTime.Now.Ticks;
                                    ExportInExcel(dtInvalidRecordSet, strFileName);
                                    hlnkInvalid.Visible = true;
                                    hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                    hlnkInvalid.Text = "Invalid Data";
                                    ucMessage1.Visible = true;
                                    ucMessage1.ShowInfo("Please click on Invalid data to check the error obtained");

                                }
                            }


                        }
                    }
                }
                catch (Exception ex)
                {
                    ucMessage1.ShowError(ex.Message);
                }
            }
            else
            {
                ucMessage1.ShowInfo("File is empty!");
            }
        }

    }
    public bool UserDetailBCP(DataTable dtUpload)
    {
        try
        {

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "BulkUploadSalesChannelWithUser";
                if (dtUpload.Columns.Contains("SalesChannelType"))
                {
                    bulkCopy.ColumnMappings.Add("SalesChannelType", "SalesChannelType");
                }
                if (dtUpload.Columns.Contains("SalesChannelCode"))
                {
                    bulkCopy.ColumnMappings.Add("SalesChannelCode", "SalesChannelCode");
                }
                if (dtUpload.Columns.Contains("SalesChannelName"))
                {
                    bulkCopy.ColumnMappings.Add("SalesChannelName", "SalesChannelName");
                }
                if (dtUpload.Columns.Contains("ContactPerson"))
                {
                    bulkCopy.ColumnMappings.Add("ContactPerson", "ContactPerson");
                }
                if (dtUpload.Columns.Contains("LoginName"))
                {
                    bulkCopy.ColumnMappings.Add("LoginName", "LoginName");
                }
                if (dtUpload.Columns.Contains("Password"))
                {
                    bulkCopy.ColumnMappings.Add("Password", "Password");
                }
                if (dtUpload.Columns.Contains("StateName"))
                {
                    bulkCopy.ColumnMappings.Add("StateName", "StateName");
                }
                if (dtUpload.Columns.Contains("CityName"))
                {
                    bulkCopy.ColumnMappings.Add("CityName", "CityName");
                }
                if (dtUpload.Columns.Contains("AreaName"))
                {
                    bulkCopy.ColumnMappings.Add("AreaName", "AreaName");
                }
                if (dtUpload.Columns.Contains("AddressLine1"))
                {
                    bulkCopy.ColumnMappings.Add("AddressLine1", "AddressLine1");
                }
                if (dtUpload.Columns.Contains("AddressLine2"))
                {
                    bulkCopy.ColumnMappings.Add("AddressLine2", "AddressLine2");
                }
                if (dtUpload.Columns.Contains("PinCode"))
                {
                    bulkCopy.ColumnMappings.Add("PinCode", "PinCode");
                }
                if (dtUpload.Columns.Contains("PhoneNo"))
                {
                    bulkCopy.ColumnMappings.Add("PhoneNo", "PhoneNo");
                }
                if (dtUpload.Columns.Contains("MobileNo1"))
                {
                    bulkCopy.ColumnMappings.Add("MobileNo1", "MobileNo1");
                }
                if (dtUpload.Columns.Contains("MobileNo2"))
                {
                    bulkCopy.ColumnMappings.Add("MobileNo2", "MobileNo2");
                }

                if (dtUpload.Columns.Contains("PanNo"))
                {
                    bulkCopy.ColumnMappings.Add("PanNo", "PanNo");
                }
                if (dtUpload.Columns.Contains("Email"))
                {
                    bulkCopy.ColumnMappings.Add("Email", "Email");
                }
                if (dtUpload.Columns.Contains("GSTNo"))
                {
                    bulkCopy.ColumnMappings.Add("GSTNo", "GSTNo");
                }
                if (dtUpload.Columns.Contains("ReportHierarchyCode"))
                {
                    bulkCopy.ColumnMappings.Add("ReportHierarchyCode", "ReportHierarchyCode");
                }
                if (dtUpload.Columns.Contains("ParentSalesChannelCode"))
                {
                    bulkCopy.ColumnMappings.Add("ParentSalesChannelCode", "ParentSalesChannelCode");
                }
                if (dtUpload.Columns.Contains("OpeningStockDate"))
                {
                    bulkCopy.ColumnMappings.Add("OpeningStockDate", "OpeningStockDate");
                }
                if (dtUpload.Columns.Contains("PasswordSalt"))
                {
                    bulkCopy.ColumnMappings.Add("PasswordSalt", "PasswordSalt");
                }
                if (dtUpload.Columns.Contains("SalesChannelId"))
                {
                    bulkCopy.ColumnMappings.Add("SalesChannelId", "SalesChannelId");
                }
                if (dtUpload.Columns.Contains("Status"))
                {
                    bulkCopy.ColumnMappings.Add("Status", "Status");
                }
                bulkCopy.ColumnMappings.Add("TransactionUploadSessionId", "TransactionUploadSessionId");
                bulkCopy.WriteToServer(dtUpload);
                return true;
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
            return false;
        }
    }
    private void ExportInExcel(DataSet DsError, string strFileName)
    {
        if (DsError != null && DsError.Tables.Count > 0)
        {
            PageBase.ExportToExeclV2(DsError, strFileName, DsError.Tables.Count);
        }
    }
    public string GenerateSalt(int length)
    {
        string guidResult = System.Guid.NewGuid().ToString();
        guidResult = guidResult.Replace("-", string.Empty);
        if (length <= 0 || length > guidResult.Length)
        {
            throw new ArgumentException("Length must be between 1 and " + guidResult.Length);
        }
        return guidResult.Substring(0, length);
    }
    public string EncryptPassword(string plainText, string saltValue)
    {
        byte[] initVectorBytes = System.Text.Encoding.ASCII.GetBytes("@1B2c3D4e5F6g7H8");
        // Must be 16 bytes
        byte[] saltValueBytes = System.Text.Encoding.ASCII.GetBytes(saltValue);
        // Encoding Password Salt Value.
        byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        //Encoding the string to be en
        PasswordDeriveBytes password = new PasswordDeriveBytes("zedaxis", saltValueBytes, "SHA1", 2);
        //Rfc2898DeriveBytes("zedaxis", saltValueBytes, 2)

        byte[] keyBytes = password.GetBytes(128 / 8);
        RijndaelManaged symmetricKey = new RijndaelManaged();
        symmetricKey.Mode = CipherMode.CBC;
        ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

        System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
        // Define cryptographic stream (always use Write mode for encryption).
        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        // Start encrypting.
        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
        // Finish encrypting.
        cryptoStream.FlushFinalBlock();

        // Convert our encrypted data from a memory stream into a byte array.
        byte[] cipherTextBytes = memoryStream.ToArray();

        // Close both streams.
        memoryStream.Close();
        cryptoStream.Close();

        // Convert encrypted data into a base64-encoded string.
        string cipherText = Convert.ToBase64String(cipherTextBytes);

        // Return encrypted string.
        return cipherText;
    }
    /*#CC18 Added End*/
}
