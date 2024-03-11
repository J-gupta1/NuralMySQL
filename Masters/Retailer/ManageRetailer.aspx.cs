using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using Cryptography;
using System.Data;

using DevExpress.Web.ASPxUploadControl;
using DevExpress.Web.ASPxClasses.Internal;
using System.IO;
using System.Web.Caching;
using System.Text;

using System.Configuration;
using System.Net;
using System.Collections.Specialized;

/*Change Log:
 * 20-Jun-15, Sumit Kumar, #CC01 - Set  Salesman option or Mandatory according to Config Value. 
  Retailer
 * 10-Mar-15, Karam Chand Sharma, #CC02 - Set  area option or Mandatory according to Config Value. 
 * 02-Jun-15, Karam Chand Sharma, #CC03 - Comment code for opening stock date (Ho case we can not able to see date)
 * 09-Jun-15, Karam Chand Sharma, #CC04 - Change config value RetailerOpStockDate instesd of DefaultOpStockDate
 * 15-Oct-15, Karam Chand Sharma, #CC05 - please refer url for more details https://zed-axis.basecamphq.com/projects/5476690-zed-salestrack/todo_items/202983804/comments#326492235
 * 16-Oct-2015, Sumit Maurya, #CC06, Record creation success message changed if Non-HO create Retailer,Mobile Number and phone number validation changed both shouldnot be start with 0 and min & max digits must be 10,Cancel button will not be displayed in the edit mode .
 * 27-Oct-2015, Sumit Maurya, #CC07, New Panel added to allow user to save Bank Details. This panel will be displayed according to ApplicationConfigurationMaster "0= Hide ,1=Show"
 * 28-Oct-2015, Karam Chand Sharma, #CC08, Add functionality for retailer bank detail . It also handled from configuration 
 * 29-Oct-2015, Sumit Maurya, #CC09, Bank detail textboxes will remain enabled or disabled according to condition. Validation of Password textbox visiblity sets off as it was visible when password textbox was hidden .
 * 30-Oct-2015, Sumit Maurya, #CC10, New Column PAN No. and textbox added under bank detail section with same validations.
 * 10-Nov-2015, Sumit Maurya, #CC11, RetailerId supplied so that its own record does not gets displayed while Edit/Approve.
 * 16-Nov-2015, Karam Chand Sharma, #CC12, Poimnt done mention below
 *              When approval is done, please show the new retailer code generated to user for his reference. This will help him.
 *               Also remarks need to be made mandatory in case of rejection 
 * 17-Nov-2015, Sumit Maurya, #CC13, Approval Reamrks string is trimmed, If user pass space or nothing then Null will be passed in in Approval Remarks.
 * 23-Dec-2015, Sumit Maurya, #CC14, New Back buttons added which gets displayed when user Edit/Approve Retailer. If user click on Back button then it will redirect to the page from which Retailer detail has been sent.
 * 29-Dec-2015, Sumit Maurya, #CC15,  Retailer detail will be binded for updation if query string with value "update" passed in type.
 * 11-Jan-2016, Karam Chand Sharma, #CC16, in this case ,  Retailer created from the SD retailer created succ it was incorrect .
 * 17-Mar-2016, Sumit Maurya, #CC18, New row added which gets displayed according to config value its value is also checked mandatory according to config value and its value will be send if it is visible.
 * 30-Mar-2016, Karam Chand Sharma, #CC19, Implement Tehsil after state and it will work according to configuration.
 * 23-May-2016, Sumit Maurya, #CC20, New Check added to disable Retailer name , State and city accordinessfully and auto approved with non-active status It should create with  active status.
 * 18-Jan-2016, Sumit Maurya, #CC17, Bank Detail(Account holder name, Account Number) binding changed as it was incorrect .
 * 17-Mar-2016, Sumit Maurya, #CC18, New row added which gets displayed according to config value its value is also checked mandatory according to config value and its value will be send if it is visible.
 * 30-Mar-2016, Karam Chand Sharma, #CC19, Implement Tehsil after state and it will work according to configuration.
 * 23-May-2016, Sumit Maurya, #CC20, New Check added to disable Retailer name , State and city according to config.
 *                                  SalesChannelID passed in method to get state detail according to parameter value supplied.
 * 26-May-2016, Sumit Maurya, #CC21, Tehsil gets displayed after city as per client requirement. Previous flow was Country>> State>>Tehsil>>City>>Area. Current flow is Country>>State>>City>>Tehsil>>Area.
 * 07-Jun-2016, Sumit Maurya, #CC22, New Label added for Tin No to dispaly as VAT Number according to config.
 * 07-Jun-2016, Sumit Maurya, #CC23, New check added that if config value of "ALLOWEDITRETAILER" is 0 and user try to edit retailer after it is approved (retailer detail coming from View retailer) then State, City  and name gets disabled / noneditable.
 * 13-Jun-2016, Sumit Maurya, #CC24,  Tehisil binding code updated. And Message displayed if config value of LocationMapping is 1 and records for state binding dropdown is 0.
 * 24-Jun-2016, Sumit Maurya, #CC25, Phone number Validation, Maxlength and Error msg is set by AppConfig.regex.
 * 08-Aug-2016, Sumit Maurya, #CC26, New code added to show radiobutton optoins of Retailer creation according to config value.
 * 26-Aug-2016, Sumit Maurya, #CC27, New field (Reference code) added.
 * 03-Oct-2016, Karam Chand Sharma, #CC28, Check Apply only number will not allow in Retailer Name and Contact Person  textarea. Validation raised by Shivalik
 * 25-Jan-2017, Sumit Maurya, #CC29, As per discussion with Karam "Status" check box was getting visible for those retailer who was not approved Now it will not get displayed except for HO. and until it is approved .
 * 10-Aug-2017, Sumit Maurya, #CC30, Instead of getting approved from HO only now those who have access of Approve retailer interface can approve the retailer. Zedbutton is used instead of normal asp button to prevent creation of retailer only from authorized User. (implemted for comio)
 * 30-Oct-2017,Vijay Kumar Prajapati,#CC31,Check Address1 minimum Length 25.(For Comio)
 * 20-Dec-2017,Vijay Kumar Prajapati,#CC32, Used user Creation Cap.
 * 24-Jan-2018, Sumit Maurya, #CC33, Modified interface according to ZedSalesV5. SalesChannel removed from interface.
 * 30-Mar-2018, Sumit Maurya, #CC34, Sales Channel dropdown wil onlybe visible to HO only and reporting hierarchy is visble and mandatory according to config value.
 * 04-Mar-2018, Sumit Maurya, #CC35, Changes implemented according to V5.
 * 11-April-2018,Vijay Kumar Prajapati,#CC36,saleman,area , countervalue optional according to application configuration master table.
 * 24-Apr-2018, Sumit Maurya, #CC37, Saleschannel ID supplied to save saleschannelID for retailer (Done for Motorola).
 * 04-May-2018, Sumit Maurya, #CC38, Approval value provided in case if approval is not required in reatiler (Done for Infocus).
 * 08-May-2018, Sumit Maurya, #CC39, Status will always be checked while reatiler creation and is not dsplayed (as per discussion with Rakesh Rawat sir). 
 *                                   Reporting hierarchy ddl was getting hidded if user was selecting "Interface" option of Select Mode. Salesman gets binded according to selected saleschannel.
 * 21-May-2018, Rajnish Kumar, #CC40, Retailer Whatsapp Number configurable
 * * 21-May-2018, Rajnish Kumar, #CC41, Retailer shop and Visiting Card Images Upload Based On configuration
30-May-2018, Rajnish Kumar, #CC42, Session Table for image is contain Previous record.
  12-June-2018, Rajnish Kumar, #CC43, In Case of edit Parent Saleschannel and Organisation Hierarchy enabled false.
 *  09-July-2018, Rajnish Kumar, #CC44, ParentRetailer is bind according Saleschannel dropdown.
 *  09-July-2018,Vijay Kumar Prajapati,#CC45,Hide saleschannelname,salesmanname,hierarchylabel for retailer multiple mapping.
 *  13-July-2018,Vijay Kumar Prajapati,#CC46,Add check filesize and filetype for upload functionality.
 *  19-July-2018, Kalpana, #CC47, design issue resolved. 
 *  21-July-2018,Vijay Kumar Prajapati,#CC48,Add AddressLength1 Check length from AppConfig.resx file.
 *  26-Jul-2018, Sumit Maurya, #CC49, Code added to create unique file name (Done for Karbonn).
 *  02-Aug-2018, Sumit Maurya, #CC50, Validation should not get fired if retailer is in editable mode (Done for Karbonn).
 *  14-Nov-2018, Sumit Maurya, #CC51, Reatiler name is editable according to session value of "EDITRETAILERNAME" (Done for Motorola).
 *  15-Nov-2018, Sumit Maurya, #CC52, function changed to bind report orgn hierarchy data  (Done for karbonn).
 *  26-Dec-2018, Sumit Maurya, #CC53, retailer Id provided to exclude getting self retailer data as parent (Done for Motorola).
 */

public partial class Masters_HO_Retailer_ManageRetailer : PageBase
{
    int RetailerId = 0, SelectedCountryID = 0, CountryCount;
    string strRetailerName, strParentRetailerID;
    DataTable dtImageData; /*#CC40*/
    DataTable dtImageDataActual; /*#CC40*/

    DataTable dtImageData1; /*#CC40*/
    DataTable dtImageDataActual1; /*#CC40*/
   
    public string XMLImage
    {
        get;
        set;

    }
    public string XMLImageVisiting
    {
        get;
        set;

    }
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            /* #CC34 Add Start */
            if (/*PageBase.BaseEntityTypeID == 2 &&*/ (Convert.ToString(Session["RETHERARCHYPARENTMANDATORY"]) == "1" || Convert.ToString(Session["RETHERARCHYPARENTMANDATORY"]) == "2"))
            {
                liSaleschannelHeading.Visible = true;
                liSaleschannelddl.Visible = true;

                if (Convert.ToString(Session["RETHERARCHYPARENTMANDATORY"]) == "1")
                {
                    /* saleschannelMandatorySign.Visible=true; */
                    OrgnHierarchayMandatorySign.Visible = true;
                }
                else
                {
                    /* saleschannelMandatorySign.Visible=false; */
                    OrgnHierarchayMandatorySign.Visible = false;
                }

            }
            else
            {
                /* liSaleschannelHeading.Visible = false;
                liSaleschannelddl.Visible = false; */

                tdLabel.Visible = false;
            }
            /* #CC39 Add Start */
            if (Session["RETAILERHIERLVLID"] != null && Convert.ToString(Session["RETHERARCHYPARENTMANDATORY"]) != "0")
            {
                if (Convert.ToInt32(Session["RETAILERHIERLVLID"]) > 0)
                {
                    if (RetailerId != 0)
                    {
                        if (Convert.ToString(HttpContext.Current.Session["RetailerMultipleMapping"]) != "1")
                        {
                            liSaleschannelHeading.Visible = false;
                            liSaleschannelddl.Visible = false;
                            tdLabel.Visible = false;
                        }
                    }
                    else
                    {
                        tdLabel.Visible = true;
                    }
                    
                }
            }/* #CC39 Add End */

            /* #CC34 Add End */


            /* #CC25 Add Start */
            regexPhoneNumber.ValidationExpression = Resources.AppConfig.PhoneNumberRegex.ToString().Replace("@", "");
            regexPhoneNumber.ErrorMessage = Resources.AppConfig.RetailerPhoneNoErrorMsg.ToString();
            txtphone.MaxLength = Convert.ToInt32(Resources.AppConfig.RetailerPhoneNoMaxLength.ToString());
            /* #CC25 Add End */
            CounterPotentialVolume(); /* #CC18 Added */
            SalesManAndArea();/*#CC36 Added*/
            /* #CC14 Add Start */
            if (Session["GoBack"] != null)
            {
                if (Session["GoBack"].ToString() == "ViewRetailer")
                {
                    btnReset.Attributes.CssStyle.Add("display", "block");
                    btnReset.PostBackUrl = "~/Masters/Retailer/ViewRetailer.aspx";                    

                }
                if (Session["GoBack"].ToString() == "ApproveRetailer")
                {
                    btnReset.Attributes.CssStyle.Add("display", "block");
                    btnReset.PostBackUrl = "~/Masters/Retailer/ApproveRetailer.aspx";

                }
            }
            /* #CC14 Add End */
              strParentRetailerID = hdnID.Value;
           // strParentRetailerID = hdnID.Text.ToString();
            //strRetailerName = hdnName.Text;
              strRetailerName = hdnName.Value;

            txtSearchedName.Text = strRetailerName;
            ucMessage1.Visible = false; /* #CC06 Added */
             /* btnsearch.Attributes.Add("onclick", "return popup();"); #CC53 Commented */
            /* #CC54 Add Start  */
            if (Request.QueryString["RetailerId"] !=null && Convert.ToString(Request.QueryString["RetailerId"]) != "")
            {
                btnsearch.Attributes.Add("onclick", "return popup('" + Convert.ToString(Request.QueryString["RetailerId"]).Replace("+", "%2") + "');");
            }
            else
            {
                btnsearch.Attributes.Add("onclick", "return popup();");
            }
            /* #CC54 Add End  */

            if ((Request.QueryString["RetailerId"] != null) && (Request.QueryString["RetailerId"] != ""))
            {
                RetailerId = Convert.ToInt32(Convert.ToString(Crypto.Decrypt(Request.QueryString["RetailerId"], PageBase.KeyStr)));
                trApproval.Attributes.CssStyle.Add("display", "none");
                btnCancel.Attributes.CssStyle.Add("display", "none");
                /* #CC15 Add Start */
                if ((Request.QueryString["type"] != null) && (Request.QueryString["type"] == "update"))
                {
                    Session["GoBack"] = "ApproveRetailer";
                    btnReset.Attributes.CssStyle.Add("display", "block");
                    BtnBack.Attributes.CssStyle.Add("display", "none");
                    btnReset.PostBackUrl = "~/Masters/Retailer/ApproveRetailer.aspx";
                }
                else
                {
                    /* #CC15 Add End */
                    /* #CC14 Add Start */
                    Session["GoBack"] = "ViewRetailer";
                    btnReset.Attributes.CssStyle.Add("display", "none");
                    BtnBack.Attributes.CssStyle.Add("display", "block");
                    BtnBack.PostBackUrl = "~/Masters/Retailer/ViewRetailer.aspx";
                    /* #CC14 Add End */
                } /* #CC15 Added */

            }
            /*#CC05 START ADDED*/
            if ((Request.QueryString["RetailerIdFromApproval"] != null) && (Request.QueryString["RetailerIdFromApproval"] != ""))
            {
                RetailerId = Convert.ToInt32(Convert.ToString(Crypto.Decrypt(Request.QueryString["RetailerIdFromApproval"], PageBase.KeyStr)));
                trApproval.Attributes.CssStyle.Add("display", "block");
                btnCancel.Attributes.CssStyle.Add("display", "none");
                /* #CC14 Add Start */
                Session["GoBack"] = "ApproveRetailer";
                btnReset.Attributes.CssStyle.Add("display", "block");
                BtnBack.Attributes.CssStyle.Add("display", "none");
                btnReset.PostBackUrl = "~/Masters/Retailer/ApproveRetailer.aspx";
                /* #CC14 Add End */

            }
            if (!IsPostBack)
            {
                Session["Table"] = null;/* #CC49 Added */
                if (Session["RetailerApproval"] != null)
                {
                    /* if ((Convert.ToString(Session["RetailerApproval"]) == "1" || Convert.ToString(Session["RetailerApproval"]) == "0") && (PageBase.HierarchyLevelID != 2)) #CC34 Commented  */
                    if ((PageBase.HierarchyLevelID != 2) && Convert.ToString(Session["RetailerApproval"]) != "")/* #CC34 Added  */
                    {
                        trProceedAction.Attributes.CssStyle.Add("display", "block");
                        trAction.Attributes.CssStyle.Add("display", "none");
                        /*  tdStatusLable.Attributes.CssStyle.Add("display", "none");
                         tdStatus.Attributes.CssStyle.Add("display", "none"); #CC30 Commented */
                        /* #CC30 Add Start */
                        btnSubmit.ActionTag = "Add";
                        tdStatusLable.Attributes.CssStyle.Add("display", "block");
                        tdStatus.Attributes.CssStyle.Add("display", "block");
                        /* #CC30 Add End */

                        /* #CC26 Add Start */
                        if ((PageBase.RetailerExcelUpload == 1))
                        {
                            rdoSelectMode.Items.Add(new ListItem("Excel Template", "1"));
                            rdoSelectMode.Items.Add(new ListItem("Interface", "2"));
                        }
                        else  /*#CC26 Add End*/
                            rdoSelectMode.Items.Add(new ListItem("Interface", "2", true));

                    }
                    else if /* (Convert.ToString(Session["RetailerApproval"]) == "1" || Convert.ToString(Session["RetailerApproval"]) == "0") #CC34 Commented  */
                        (Convert.ToString(Session["RetailerApproval"]) != "") /* #CC34 Added */
                    {
                        trProceedAction.Attributes.CssStyle.Add("display", "none");
                        trAction.Attributes.CssStyle.Add("display", "block");
                        tdStatusLable.Attributes.CssStyle.Add("display", "block");
                        tdStatus.Attributes.CssStyle.Add("display", "block");

                        rdoSelectMode.Items.Add(new ListItem("Excel Template", "1"));
                        rdoSelectMode.Items.Add(new ListItem("Interface", "2"));
                    }



                    /*#CC16 START ADDED*/
                    if (Convert.ToString(Session["RetailerApproval"]) == "0")
                    {
                        chkactive.Checked = true;
                    }
                    /*#CC16 START END*/
                    //else if (Convert.ToString(Session["RetailerApproval"]) == "0")
                    //{
                    //    trProceedAction.Attributes.CssStyle.Add("display", "block");
                    //    trAction.Attributes.CssStyle.Add("display", "none");
                    //    tdStatusLable.Attributes.CssStyle.Add("display", "none");
                    //    tdStatus.Attributes.CssStyle.Add("display", "none");
                    //    rdoSelectMode.Items.Add(new ListItem("Interface", "2"));

                    //}
                    /* #CC22 Add Start */
                    if (PageBase.ChangeTinLabel == 1)
                    {
                        txtTinNoHeading.Text = /*"VAT No:"  #CC30 Commented */ "GST No:";
                    }
                    /* #CC22 Add End */

                }
            }



            /*#CC05 START END*/
            if (Session["RetailerOpStockDate"] != null) /*#CC04 Change DefaultOpStockDate*/
            {
                if (Convert.ToString(Session["RetailerOpStockDate"]) != "0")/*#CC04 Change DefaultOpStockDate*/
                {
                    ucOpeningStock.imgCal.Enabled = false;
                    ucOpeningStock.TextBoxDate.Enabled = false;
                    ucOpeningStock.Date = Convert.ToDateTime(Session["RetailerOpStockDate"].ToString()).ToString("MM/dd/yyyy");
                    //ucOpeningStock.Date = Session["RetailerOpStockDate"].ToString();
                }
            }

            //if (Session["DefaultOpStockDate"] != null)
            //    ucOpeningStock.Date = Session["DefaultOpStockDate"].ToString();
            if (!IsPostBack)
            {
                txtApproalRemarks.Text = "";/*#CC05 ADDED*/
                // hdnApproveReject.Value = "0"; /*#CC05 ADDED*/
                hdnApproveReject.Text = "0";
                UserPanel();
                Retailerchequevalid.Visible = false;/*#CC46 Added*/
                RetailerShopImageRequired.Visible = false;/*#CC46 Added*/
                VisitingCardRequired.Visible = false;/*#CC46 Added*/
   
                WhatsappnumberValidation();/*#CC40*/
                FillRetailerType();
                ddlSalesman.Items.Clear();
                ddlSalesman.Items.Insert(0, new ListItem("Select", "0"));
                /* if (Convert.ToString(Session["RETHERARCHYPARENTMANDATORY"]) != "0")  #CC34 Added */
                FillSalesChannelType();    /*#CC33 Commented */ /* #CC34 uncommented */
                // ddlParentRetailer.Items.Insert(0, new ListItem("Select", "0"));

                if (Session["MessageType"] != null)
                {
                    ucMessage1.ShowSuccess(Session["MessageType"].ToString());
                    Session["MessageType"] = null;
                }

                if (RetailerId == 0)
                {
                    cmbCountry.Items.Insert(0, new ListItem("Select", "0"));
                    cmbstate.Items.Insert(0, new ListItem("Select", "0"));
                }
                if (Convert.ToString(Session["RETHERARCHYPARENTMANDATORY"]) != "0") /* #CC34 Added */
                    cmbsaleschannel_SelectedIndexChanged(cmbsaleschannel, new EventArgs()); /* #CC33 Commented */ /* #CC34 uncommented */
                //cmbsaleschannel.Items.Insert(0, new ListItem("Select", "0"));/* #CC33 Added */
                req1.Enabled = false;

                fillcountry(); /* #CC33 Uncommented */

                /*#CC03 COMMENTED START if (SalesChanelTypeID != 0)
                {
                    FillDate();
                    lblSubmitOpeningStock.Visible = true;
                    ucOpeningStock.Visible = true;
                    ucOpeningStock.ValidationGroup = "Add";
                    ucOpeningStock.IsRequired = true;
                }
                else
                {
                    lblSubmitOpeningStock.Visible = false;
                    ucOpeningStock.Visible = false;
                }#CC03 COMMENTED END */
                /*#CC03 ADDED START */
                FillDate();
                lblSubmitOpeningStock.Visible = true;
                ucOpeningStock.Visible = true;
                ucOpeningStock.ValidationGroup = "Add";
                ucOpeningStock.IsRequired = true;
                /*#CC03 ADDED END */

                ucOpeningStock.TextBoxDate.Enabled = true;
                ucOpeningStock.imgCal.Enabled = true;
                if (RetailerId != 0)
                {
                    fillcountry();
                    if ((Session["RetailerCheckUpload"] != null) || (Session["RetailerVisitingCardandshopImageUpload"] != null))
                    {
                        gvAttachedImages.Visible = true;
                    }
                    PouplateRetailerDetail(RetailerId);

                    /* #CC20 Add Start */

                    /* #CC51 Add Start */

                    if (PageBase.EDITRETAILERNAME != "" && (PageBase.ALLOWEDITRETAILER == "2" || PageBase.ALLOWEDITRETAILER == "3") && Convert.ToString(Session["GoBack"]) == "ViewRetailer")
                    {
                        string[] strRole = PageBase.EDITRETAILERNAME.Split(',');
                        foreach (string strrole in strRole)
                        {
                            if (strrole == PageBase.RoleID.ToString())
                            {
                                txtretailername.Enabled = true;
                                break;
                            }
                        }
                    }
                    else
                    { 
                        txtretailername.Enabled = false;
                    }
                    /* #CC51 Add End */

                    if (PageBase.ALLOWEDITRETAILER == "0" && Convert.ToString(Session["GoBack"]) == "ViewRetailer") /* #CC23 New check of data from View reatiler added */
                    {
                            txtretailername.Enabled = false;
                       
                        cmbstate.Enabled = false;
                        cmbcity.Enabled = false;
                    }
                    /* #CC20 Add End */
                    /*#CC05 Commented btnSubmit.Text = "Update";*/
                    /*#CC05 START ADDED*/
                    if ((Request.QueryString["RetailerIdFromApproval"] != null) && (Request.QueryString["RetailerIdFromApproval"] != ""))
                    {
                        if (
                            (Convert.ToString(Session["RetailerApproval"]) == "1"
                            || Convert.ToString(Session["RetailerApproval"]) == "0"
                             || Convert.ToString(Session["RetailerApproval"]) == "2"
                            ) /* && (PageBase.HierarchyLevelID == 2) #CC30 Commented  */ ) /* #CC34 Approval Level 2 Added */
                        {
                            btnReject.Visible = true;
                            btnSubmit.Text = "Approve";
                            btnSubmit.ActionTag = "Edit"; /* #CC30 Added */
                        }
                    }
                    else
                    {
                        btnReject.Visible = false;
                        btnSubmit.Text = "Update";
                    }/*#CC05 START END*/
                    //cmbsaleschannel.Enabled = false;

                    int NotFireOpeningStockValidationOnUpdate = 1;/*0-Not Fire 1-TobeFired*/
                    if (RetailerId != 0 & ucOpeningStock.Date != "")
                    {
                        NotFireOpeningStockValidationOnUpdate = 0;
                    }
                    if (NotFireOpeningStockValidationOnUpdate == 0)
                        ucOpeningStock.ValidationGroup = "Dummy";

                }
                else
                {
                    if (SalesChanelTypeID != 0 | SalesChanelTypeID == 0)
                    {
                        /*if (Convert.ToString(Session["DefaultOpStockDate"]) != "0" & Convert.ToString(Session["DefaultOpStockDate"]) != "-1")*/
                        if (Convert.ToString(Session["RetailerOpStockDate"]) != "0")  /*#CC04 Change DefaultOpStockDate*/
                        {
                            // ucOpeningStock.Date = Session["RetailerOpStockDate"].ToString();/*#CC04 Change DefaultOpStockDate*/
                            ucOpeningStock.Date = Convert.ToDateTime(Session["RetailerOpStockDate"].ToString()).ToString("MM/dd/yyyy");
                            ucOpeningStock.TextBoxDate.Enabled = false;
                            ucOpeningStock.imgCal.Enabled = false;
                        }
                    }
                }

                if (PageBase.SalesChanelID != 0)
                {
                    if (RetailerId == 0)
                    {
                        ddlOrghierarchy.Items.Clear();
                        FillHierarchy();
                    }
                }
                else
                {
                    if (ddlOrghierarchy.Items.FindByValue("0") != null)
                    {
                        if (ddlOrghierarchy.Items.FindByValue("0").Value != "0")
                        {
                            ddlOrghierarchy.Items.Insert(0, new ListItem("Select", "0"));
                        }
                    }
                    else
                    {
                        ddlOrghierarchy.Items.Insert(0, new ListItem("Select", "0"));
                    }
                    ddlOrghierarchy.Visible = Convert.ToInt32(Session["RETAILERHIERLVLID"]) == 0 ? false : true;
                    tdLabel.Visible = ddlOrghierarchy.Visible;
                }
                if (RetailerId == 0)       //Pankaj Dhingra
                {
                    if (Convert.ToInt32(Session["RETAILERCODEAUTO"]) == 1)
                    {
                        txtretailercode.Text = "autogenerated";
                        rqRetailerCode.Enabled = false;
                    }
                    else
                    {
                        txtretailercode.Text = "";
                        txtretailercode.Enabled = true;
                        rqRetailerCode.Enabled = true;
                    }
                }
                else
                {
                    if (Convert.ToInt32(Session["RETAILERCODEAUTO"]) == 0)
                    {
                        txtretailercode.Enabled = true;
                        rqRetailerCode.Enabled = true;
                    }

                }

                /*#CC08 ADDED START*/
                if (Session["RetailerBankDetail"] != null)
                {
                    if (Convert.ToString(Session["RetailerBankDetail"]) == "1")
                    {
                        if (RetailerId == 0)
                        {
                            ClearBankDetail();
                            tblUpdateBankDetail.Attributes.CssStyle.Add("display", "none");
                            BankDetailsNOnMandatory();/*#CC46 Added*/
                        }
                        else
                        {
                            tblUpdateBankDetail.Attributes.CssStyle.Add("display", "block");
                            txtBankName.Enabled = false;
                            txtBranchLocation.Enabled = false;
                            txtBankAccountNumber.Enabled = false;
                            txtAccountHolder.Enabled = false;
                            txtIfscCode.Enabled = false;
                            txtPANNo.Enabled = false; /* #CC10 Added */
                           
                        }

                        dvBankDetail.Attributes.CssStyle.Add("display", "block");
                       

                    }
                    else if (Convert.ToString(Session["RetailerBankDetail"]) == "0")
                    {
                        ClearBankDetail();

                        dvBankDetail.Attributes.CssStyle.Add("display", "none");
                    }
                }
                /*#CC40 start*/
                if (Session["WhatsAppMobileNumber"] != null)
                {
                    if (Convert.ToString(Session["WhatsAppMobileNumber"]) == "1")
                    {

                        if (RetailerId == 0)
                        {
                            tblUpdateWhatsNumber.Attributes.CssStyle.Add("display", "none");

                        }
                        else
                        {
                            tblUpdateWhatsNumber.Attributes.CssStyle.Add("display", "block");
                            txtWhatsNumber.Enabled = false;
                        }
                        dvWhatsNumber.Attributes.CssStyle.Add("display", "block");

                    }
                    else if (Convert.ToString(Session["WhatsAppMobileNumber"]) == "0")
                    {
                        dvWhatsNumber.Attributes.CssStyle.Add("display", "none");
                    }
                }
                /*#CC40 end*/
                /*#CC41 start*/
                if (Session["RetailerCheckUpload"] != null)
                {
                    if (Convert.ToString(Session["RetailerCheckUpload"]) == "1")
                    {

                        if (RetailerId == 0)
                        {
                            tblRetailerCheckUpload.Attributes.CssStyle.Add("display", "none");

                        }
                        else
                        {
                            tblRetailerCheckUpload.Attributes.CssStyle.Add("display", "block");

                        }
                        dvRetailerCheckUpload.Attributes.CssStyle.Add("display", "block");

                    }
                    else if (Convert.ToString(Session["RetailerCheckUpload"]) == "0")
                    {
                        dvRetailerCheckUpload.Attributes.CssStyle.Add("display", "none");
                    }
                }

                if (Session["RetailerVisitingCardandshopImageUpload"] != null)
                {
                    if (Convert.ToString(Session["RetailerVisitingCardandshopImageUpload"]) == "1")
                    {

                        if (RetailerId == 0)
                        {
                            tblShopImageVisitingCard.Attributes.CssStyle.Add("display", "none");

                        }
                        else
                        {
                            tblShopImageVisitingCard.Attributes.CssStyle.Add("display", "block");

                        }
                        dvShopImagesVisitingCard.Attributes.CssStyle.Add("display", "block");

                    }
                    else if (Convert.ToString(Session["RetailerVisitingCardandshopImageUpload"]) == "0")
                    {
                        dvShopImagesVisitingCard.Attributes.CssStyle.Add("display", "none");
                    }
                }
                /*#CC41 end*/



                /*#CC38 end*/
                /*#CC08 ADDED END*/
                /*#CC01 Added  Start*/
                //if (PageBase.SALESMANOPTIONAL == "1")
                //{
                //    divRetailerMandatory.Visible = false;
                //    RequiredFieldValidator4.Enabled = false;
                //}
                /*#CC01 Added  End*/
                /*#CC02 START ADDED*/
                if (PageBase.AREAOPTIONAL == "1")
                {
                    dvAreaMandotery.Visible = false;
                    RequiredArea.Enabled = false;
                }
                /*#CC02 START END*/
                /*#CC19 START ADDED*/
                if (PageBase.TehsillDisplayMode == "1")
                {
                    tdTehsil.Style.Add("display", "block");
                    rfvTehsil.Enabled = false;
                    if (cmbTehsil.Items.Count < 1) /* #CC24 Added */
                        cmbTehsil.Items.Insert(0, new ListItem("Select", "0"));
                }
                else
                {
                    tdTehsil.Style.Add("display", "none");
                    rfvTehsil.Enabled = false;
                    rfvTehsil.ValidationGroup = "";
                }
                /*#CC19 START END*/
            }
            UserPanel();
            WhatsappnumberValidation();/*CC40*/

            /* #CC30 Add Start */
            if (Resources.AppConfig.ShowHideRetailerReferanceCode.ToString() == "0")
            {
                liReferanceCodeHeading.Visible = false;
                liReferanceCode.Visible = false;
            }

            if (PageBase.HierarchyLevelID == 1 || (Session["GoBack"] != null && Session["GoBack"].ToString() == "ApproveRetailer"))
            {
                chkactive.Enabled = true;
                /* #CC39 Comment Start
                 liStatusHeading.Visible = true;                
                liStatus.Visible = true;  #CC39 Comment End*/
                chkactive.Checked = true;
                /* #CC39 Add Start */
                liStatusHeading.Visible = false;
                liStatus.Visible = false;
                /* #CC39 Add End */
            }
            else
            {
                chkactive.Enabled = false;
                liStatusHeading.Visible = false;
                liStatus.Visible = false;
                chkactive.Checked = true;
            }
            /* #CC30 Add End */
            /* #temp Add Start */
            if ((Request.QueryString["RetailerIdFromApproval"] != null) && (Request.QueryString["RetailerIdFromApproval"] != ""))
            {
                cmbsaleschannel.Enabled = false;
                ddlSalesman.Enabled = false;
            }/* #temp Add End */
        }

        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);

        }
    }
    /*#CC08 ADDED START*/
    void ClearBankDetail()
    {
        txtBankName.Text = "";
        txtBranchLocation.Text = "";
        txtBankAccountNumber.Text = "";
        txtAccountHolder.Text = "";
        txtIfscCode.Text = "";
        txtPANNo.Text = ""; /* #CC10 Added*/
        ChkBankDetail.Checked = false;
    }
    /*#CC08 ADDED END*/
    void FillRetailerType()
    {
        using (RetailerData ObjRetaileType = new RetailerData())
        {
            DataTable dt = new DataTable(); /*#CC46 Added*/
            ObjRetaileType.SearchCondition = EnumData.eSearchConditions.Active;
            // String[] StrCol = new String[] { "ReatilerTypeID", "RetailerTypeName" };/*#CC46 Commented*/
            /*#CC46 Added Started*/
            dt = ObjRetaileType.GetAllRetaileType();
            cmbRetailerType.DataSource = dt;
            cmbRetailerType.DataTextField = "RetailerTypeName";
            cmbRetailerType.DataValueField = "ReatilerTypeID";
            cmbRetailerType.DataBind();
            cmbRetailerType.Items.Insert(0, new ListItem("Select", "0"));
            /*CC36:added (start)*/
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbRetailerType.Attributes.Add("BankDetailRequired" + dt.Rows[i]["ReatilerTypeID"].ToString(), dt.Rows[i]["BankDetailRequired"].ToString());
            }
            /*#CC46 Added Started*/
            // PageBase.DropdownBinding(ref cmbRetailerType, ObjRetaileType.GetAllRetaileType(), StrCol);/*#CC46 Commented*/

        };
    }
    //public void fillcountry()
    //{
    //    using (MastersData objmaster = new MastersData())
    //    {

    //        try
    //        {
    //            DataTable dt;
    //            objmaster.CountrySelectionMode = 1;
    //            dt = objmaster.SelectCountryInfo();
    //            String[] colArray = { "CountryID", "CountryName" };
    //            PageBase.DropdownBinding(ref cmbCountry, dt, colArray);
    //            cmbstate.Items.Insert(0, new ListItem("Select", "0"));

    //        }
    //        catch (Exception ex)
    //        {
    //            ucMessage1.ShowInfo(ex.Message.ToString());
    //            PageBase.Errorhandling(ex);
    //        }

    //    }
    //}
    public void fillcountry()
    {
        using (MastersData obj = new MastersData())
        {
            DataTable dt;
            cmbCountry.Items.Clear();
            obj.CountrySelectionMode = 1;
            dt = obj.SelectCountryInfo();
            String[] colArray = { "CountryID", "CountryName" };
            PageBase.DropdownBinding(ref cmbCountry, dt, colArray);
            SelectedCountryID = Convert.ToInt32(dt.Rows[0]["CountryID"]);
            CountryCount = Convert.ToInt32(dt.Rows[0]["CountryCount"]);
            //cmbstate.Items.Insert(0, new ListItem("Select", "0"));
        }

    }

    void FillSalesChannelType()
    {
        /* #temp Add Start */
        Int16 isApproval = 0;
        if ((Request.QueryString["RetailerIdFromApproval"] != null) && (Request.QueryString["RetailerIdFromApproval"] != ""))
        {
            isApproval = 1;
        }/* #temp Add End */

        cmbsaleschannel.Enabled = true;
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {

            ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
            ObjSalesChannel.SearchType = EnumData.eSearchConditions.Active;
            ObjSalesChannel.BilltoRetailer = true;
            ObjSalesChannel.StatusValue = 1;
            ObjSalesChannel.UserID = PageBase.UserId;/* #CC39 Added */

            ObjSalesChannel.BindChild = isApproval == 1 ? 0 : 1;/* #CC39 Added */ /* #temp isApproval == 1 ? 0 : Added */
            String[] StrCol = new String[] { "SalesChannelID", "DisplayName" };
            PageBase.DropdownBinding(ref cmbsaleschannel, ObjSalesChannel.GetSalesChannelInfo(), StrCol);
            if (Convert.ToInt32(PageBase.SalesChanelID) != 0)             //this will select the login user in combo
            {
                cmbsaleschannel.SelectedValue = PageBase.SalesChanelID.ToString();
                cmbsaleschannel.Enabled = false;
            }
            cmbsaleschannel.Enabled = (Convert.ToInt32(isApproval) == 1 || BaseEntityTypeID == 1) ? false : true; /* #temp Added */
        };
    }

    private bool ValidateControl(ref string ErrMessage)
    {

        if ((cmbsaleschannel.SelectedValue == "Select") ||
            (cmbstate.SelectedValue == "Select") || (cmbcity.SelectedValue == "Select")

            || (txtretailercode.Text.Trim() == "") || (txtretailername.Text.Trim() == "") || (txtAddress1.Text.Trim() == "") || (txtmobile.Text.Trim() == "")
            || (txtcontactperson.Text.Trim() == ""))
        {
            ErrMessage = Resources.Messages.MandatoryField;
            return false;

        }
        /*#CC01 Added  Start*/
        //if (PageBase.SALESMANOPTIONAL == "0")
        //{
        //    if (ddlSalesman.SelectedValue == "0")
        //    {
        //        ErrMessage = Resources.Messages.MandatoryField;
        //        return false;
        //    }
        //}
        /*#CC01 Added  End*/
        /*#CC36 Added Started*/
        if (Convert.ToInt32(Session["PotentialVolMandatry"]) == 1)
        {
            if (txtcountersize.Text.Trim() == "")
            {
                ErrMessage = Resources.Messages.MandatoryField;
                return false;
            }
        }
        /*#CC36 Added End*/
        if (ServerValidation.IsMobileNo(txtmobile.Text.Trim(), true) > 0)
        {
            ErrMessage = Resources.Messages.Mobilevalidate;
            return false;

        }
        if (ServerValidation.IsValidEmail(txtemail.Text.Trim(), false) > 0)
        {
            ErrMessage = ErrMessage + Resources.Messages.Emailvalidate;
            return false;

        }

        if (ServerValidation.IsPinCode(txtpincode.Text.Trim(), true) > 0)
        {
            ErrMessage = ErrMessage + Resources.Messages.PinCodevalidate;
            return false;

        }
        ///*#CC31 Started*/
        //if (txtAddress1.Text.Trim().Length < 25)
        //{
        //    ErrMessage = ErrMessage + "Address1 should be 25 character.";
        //    return false;
        //}

        ///*#CC31 Started End*/


        return true;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {/*#CC46 Added Started*/
            if (RetailerId != 0)
            {
                if (Convert.ToString(HttpContext.Current.Session["RetailerMultipleMapping"]) != "1")
                {
                    liSaleschannelHeading.Visible = true;
                    liSaleschannelddl.Visible = true;
                    tdLabel.Visible = true;
                    tdsalesman.Visible = true;
                }
                else
                {
                    liSaleschannelHeading.Visible = false;
                    liSaleschannelddl.Visible = false;
                    tdLabel.Visible = false;
                    tdsalesman.Visible = false;
                    req1.ValidationGroup = "";
                    req1.Attributes.CssStyle.Add("display", "none");
                    reqOrgnhierarchy.ValidationGroup = "";
                    reqOrgnhierarchy.Attributes.CssStyle.Add("display", "none");
                }
            }
            else
            {
                liSaleschannelHeading.Visible = true;
                liSaleschannelddl.Visible = true;
                tdLabel.Visible = true;
                tdsalesman.Visible = true;
                req1.ValidationGroup = "Add";
                req1.Attributes.CssStyle.Add("display", "block");
                reqOrgnhierarchy.ValidationGroup = "Add";
                reqOrgnhierarchy.Attributes.CssStyle.Add("display", "block");
            }
            /*#CC46 Added End*/
            if (IsPageRefereshed == true)
            {
                return;
            }       //Pankaj Dhingra
            int Result = 0;
            string ErrorMsg = "";
            if (!ValidateControl(ref ErrorMsg))
            {
                ucMessage1.ShowInfo(ErrorMsg);
                return;
            }
            /* #CC07 Add Start */
            if(ChkBankDetail.Checked==true)
            {
                if (CheckBankDetailText() == 1)
                {
                    return;
                }
            }
           
            /* #CC07 Add End */

            using (RetailerData objRetailer = new RetailerData())
            {
                if ((Request.QueryString["RetailerId"] != null) && (Request.QueryString["RetailerId"] != ""))
                {
                    RetailerId = Convert.ToInt32(Crypto.Decrypt(Request.QueryString["RetailerId"], PageBase.KeyStr));
                    objRetailer.RetailerID = RetailerId;
                }
                else if ((Request.QueryString["RetailerIdFromApproval"] != null) && (Request.QueryString["RetailerIdFromApproval"] != ""))
                {
                    RetailerId = Convert.ToInt32(Crypto.Decrypt(Request.QueryString["RetailerIdFromApproval"], PageBase.KeyStr));
                    objRetailer.RetailerID = RetailerId;
                }
                else
                {
                    objRetailer.RetailerID = 0;
                    string StrPSalt = "";
                    if (Convert.ToInt16(Session["RetailerLogin"]) == 1)
                    {
                        using (Authenticates ObjAuth = new Authenticates())
                        {
                            StrPSalt = ObjAuth.GenerateSalt(txtpassword.Text.Trim().Length);
                            objRetailer.Password = ObjAuth.EncryptPassword(txtpassword.Text.Trim(), StrPSalt);

                        };
                        objRetailer.PasswordSalt = StrPSalt;
                    }


                }
                objRetailer.RetailerTypeID = Convert.ToInt32(cmbRetailerType.SelectedValue);
                if (rblCounterIsp.SelectedValue == "1")
                    objRetailer.ISP = true;
                else
                    objRetailer.ISP = false;
                objRetailer.RetailerCode = txtretailercode.Text.Trim();
                objRetailer.RetailerName = txtretailername.Text.Trim();
                /* #CC33 Add Start */
                //if (cmbsaleschannel.Items.Count == 0 && cmbsaleschannel.Items[0].Value!="0")
                //{
                //    objRetailer.SalesChannelID = 0;
                //}
                //else  /* #CC33 Add End */
                objRetailer.SalesChannelID = Convert.ToInt32(cmbsaleschannel.SelectedValue); /* #CC37 Uncommented */
                /* objRetailer.SalesChannelID = 0 #CC37 Commented ;*/
                objRetailer.Address1 = txtAddress1.Text.Trim();
                objRetailer.Address2 = txtAddress2.Text.Trim();
                objRetailer.StateID = Convert.ToInt16(cmbstate.SelectedValue);
                objRetailer.CityID = Convert.ToInt16(cmbcity.SelectedValue);
                objRetailer.AreaID = Convert.ToInt16(cmbArea.SelectedValue);
                /*#CC36 Commented Started*/
                //if (txtcountersize.Text != "")
                //{
                //    objRetailer.CounterSize = txtcountersize.Text.Trim();
                //}
                /*#CC36 Commented End*/
                objRetailer.ContactPerson = txtcontactperson.Text.Trim();
                objRetailer.MobileNumber = txtmobile.Text.Trim();
                objRetailer.PhoneNumber = txtphone.Text.Trim();
                objRetailer.PinCode = txtpincode.Text.Trim();
                objRetailer.TinNumber = txttinno.Text.Trim();
                objRetailer.Status = chkactive.Checked;
                objRetailer.Email = txtemail.Text.Trim();
                objRetailer.UserID = PageBase.UserId;
                objRetailer.SalesmanID = Convert.ToInt32(ddlSalesman.SelectedValue);
                objRetailer.UserName = txtUserName.Text.Trim();
                //objRetailer.PasswordExpiryDays = Convert.ToInt16(Application["ExpiryDays"].ToString());
                objRetailer.PasswordExpiryDays = Convert.ToInt16(PageBase.PasswordExp);
                 objRetailer.GroupParentID = hdnID.Value == "" ? 0 : Convert.ToInt32(hdnID.Value);    //Convert.ToInt32(ddlParentRetailer.SelectedValue);
                //objRetailer.GroupParentID = hdnID.Text == "" ? 0 : Convert.ToInt32(hdnID.Text);
                objRetailer.CreateLoginOrNot = Convert.ToInt16(Session["RetailerLogin"]);
                objRetailer.RetailerHierarchyLevelID = Convert.ToInt16(Session["RETAILERHIERLVLID"]);
                objRetailer.RetailerOrgnHierarchyID = ddlOrghierarchy.SelectedValue == "" ? 0 : Convert.ToInt32(ddlOrghierarchy.SelectedValue);
                objRetailer.OpeningStockDate = ucOpeningStock.Date == "" ? objRetailer.OpeningStockDate : Convert.ToDateTime(ucOpeningStock.Date);
                /*#CC08 ADDED START*/
                objRetailer.BankName = txtBankName.Text;
                objRetailer.AccountHolder = txtAccountHolder.Text;
                objRetailer.AccountNumber = txtBankAccountNumber.Text;
                objRetailer.BranchLocation = txtBranchLocation.Text;
                objRetailer.IFSCCode = txtIfscCode.Text;
                objRetailer.PANNo = txtPANNo.Text.Trim(); /* #CC10 Added */
                objRetailer.UpdateBankDetail = ChkBankDetail.Checked;
                /*#CC19 START ADDED*/
                if (PageBase.TehsillDisplayMode == "1")
                    objRetailer.TehsilId = Convert.ToInt16(cmbTehsil.SelectedValue);
                else
                    objRetailer.TehsilId = 0;
                /*#CC19 START END*/
                /*#CC08 ADDED END*/

                if (ucDOB.Date != "")
                    objRetailer.DateOfBirth = Convert.ToDateTime(ucDOB.Date);
                else
                    objRetailer.DateOfBirth = null;
                /*#CC05 START ADDED*/
                if (Convert.ToString(Session["RetailerApproval"]) == "1" || Convert.ToString(Session["RetailerApproval"]) == "2") /* #CC38 Added */
                {
                    if (hdnApproveReject.Text == "1")
                        objRetailer.ApprovalStatus = 2;
                    else if (hdnApproveReject.Text == "0" && btnSubmit.Text == "Approve")
                        objRetailer.ApprovalStatus = 1;
                    /* #CC38 Add Start  */
                }
                else
                {
                    objRetailer.ApprovalStatus = 1;
                }/* #CC38 Add End  */

                /* objRetailer.ApprovalRemarks = txtApproalRemarks.Text.Trim(); */
                /* #CC13 Commented*/
                /* #CC13 Add Start */
                if (txtApproalRemarks.Text.Trim() == "")
                {
                    objRetailer.ApprovalRemarks = null;
                }
                else
                {
                    objRetailer.ApprovalRemarks = txtApproalRemarks.Text.Trim();
                }
                /* #CC13 Add End */
                /*#CC05 START END*/

                /* #CC18 Add Start */
                //if (Convert.ToInt32(Session["PotentialVolMandatry"]) == 1 && Convert.ToInt32(Session["PotentialVolDisplay"]) == 1)

                if (Convert.ToInt32(Session["PotentialVolMandatry"]) == 1)
                {
                    if (txtcountersize.Text.Trim() == "" || txtcountersize.Text.Trim() == null)
                    {
                        ucMessage1.ShowInfo("Please enter counter potential value.");
                        return;
                    }
                    else
                    {
                        if (Convert.ToInt32(Session["PotentialVolMandatry"]) == 1)
                        {
                            objRetailer.CounterSize = txtcountersize.Text.Trim();
                        }
                        else
                        {
                            objRetailer.CounterSize = "0";
                        }

                    }


                }
                /*#CC36 Added Started*/
                else
                {
                    objRetailer.CounterSize = "0";
                }
                /*#CC36 Added End*/
                if (Convert.ToInt32(Session["PotentialVolDisplay"]) == 1)
                {
                    if (txtCounterValue.Text.Trim() == "" || txtCounterValue.Text.Trim() == null)
                    {
                        objRetailer.CounterPotentialValue = 0;
                    }
                    else
                    {
                        objRetailer.CounterPotentialValue = Convert.ToInt64(txtCounterValue.Text.Trim());
                    }
                }
                /* #CC18 Add End */
                objRetailer.ReferanceCode = txtReferanceCode.Text.Trim(); /* #CC27 Added */
                objRetailer.whatsAppNumber = txtWhatsNumber.Text.Trim(); /*#CC40 added*/
                Result = objRetailer.InsertUpdateRetailer();
                saveRetailerImage(objRetailer.RetailerID);/*#CC41 added*/
                Session["Table"] = null; /*CC42*/
                if (Result > 0 && (objRetailer.Error == null || objRetailer.Error == ""))
                {

                    if (RetailerId == 0)
                    {


                        /* Session["MessageType"] = Resources.Messages.CreateSuccessfull;*/
                        /* #CC06 Commented */
                        /* #CC06 Add Start */
                        if (PageBase.HierarchyLevelID == 1 || Convert.ToString(Session["RetailerApproval"]) == "0")
                        {
                            Session["MessageType"] = (Resources.Messages.CreateSuccessfull).Replace("Records", "Record");


                        }
                        else
                        {
                            Session["MessageType"] = (Resources.Messages.CreateSuccessfull).Replace("Records", "Record") + ". It has gone for approval.";
                        }
                        /* #CC06 Add End */
                    }
                    else
                    {
                        Session["MessageType"] = (Resources.Messages.EditSuccessfull).Replace("Records", "Record"); /* #CC06 Success message changed from "Records Updated successfully" to "Record updated successfully" */
                        /*#CC12 ADDED START*/
                        if (objRetailer.ApprovalStatus == 1)
                            Session["MessageType"] = (Resources.Messages.EditSuccessfull).Replace("Records", "Record") + " Retailer Code : " + objRetailer.RetailerCode;
                        /*#CC12 ADDED END*/
                    }
                    hdnApproveReject.Text = "0";

                    Response.Redirect("ManageRetailer.aspx", false);
                }
                else
                {
                    if (objRetailer.Error != null && objRetailer.Error != "")
                    {
                        ucMessage1.ShowInfo(objRetailer.Error);
                    }
                    else
                    {
                        ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    }
                }

                if (Convert.ToInt32(PageBase.SalesChanelID) != 0)             //this will select the login user in combo
                {
                    cmbsaleschannel.SelectedValue = PageBase.SalesChanelID.ToString();
                    cmbsaleschannel.Enabled = false;
                }
                else
                {
                    cmbsaleschannel.Enabled = true;
                }
                UserPanel();

            };
        }
        catch (Exception ex)
        {   /*#CC32 Added Started*/
            if (ex.Message.ToLower().Contains("trgusercount"))
            {
                ucMessage1.ShowError("Active user count is exceeding the limit defined. Please Contact administrator.");
            }
            else/*#CC32 Added End*/
                ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    void ClearForm()
    {

        // cmbsaleschannel.SelectedValue = "0";
        cmbstate.SelectedValue = "0";
        cmbcity.SelectedValue = "0";
        cmbArea.SelectedValue = "0";
        txtAddress1.Text = "";
        txtAddress2.Text = "";
        txtemail.Text = "";
        txtphone.Text = "";
        txtpincode.Text = "";
        txtretailercode.Text = "";
        txtretailername.Text = "";
        txtcontactperson.Text = "";
        btnSubmit.Text = "Submit";
        chkactive.Checked = true;
        //chkactive.Enabled = false;
        txtcountersize.Text = "";
        cmbArea.Items.Clear();
        cmbArea.Items.Insert(0, new ListItem("Select", "0"));
        cmbcity.Items.Clear();
        cmbcity.Items.Insert(0, new ListItem("Select", "0"));
        //lblHeading.Text = "Manage Retailer";
        txtmobile.Text = "";
        txttinno.Text = "";
        ddlSalesman.SelectedValue = "0";
        ddlOrghierarchy.Items.Clear();
        if (PageBase.SalesChanelID != 0)
            FillHierarchy();
        else
            ddlOrghierarchy.Items.Insert(0, new ListItem("Select", "0"));



    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageRetailer.aspx");
        Session["MessageType"] = null;

    }
    protected void LBViewRetailer_Click(object sender, EventArgs e)
    {
        /*   Response.Redirect("ViewRetailer.aspx"); #CC35 Commented */
        /* #CC35 Add Start */
        try
        {
            if (Convert.ToString(Session["GoBack"]) == "ViewRetailer")
            {
                Response.Redirect("ViewRetailer.aspx");
            }
            else if (Convert.ToString(Session["GoBack"]) == "ApproveRetailer")
            {
                Response.Redirect("ApproveRetailer.aspx");
            }
            else
            {
                Response.Redirect("ViewRetailer.aspx");
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
        /* #CC35 Add End */

    }
    private void PouplateRetailerDetail(int RetailerID)
    {
        DataTable DtSalesChannelDetail;
        DataTable DtRetailerImage;
        try
        {
            using (RetailerData ObjRetailer = new RetailerData())
            {

                ObjRetailer.RetailerID = RetailerID;
                ObjRetailer.UserID = PageBase.UserId;
                DtSalesChannelDetail = ObjRetailer.GetRetailerInfo();
                DtRetailerImage = ObjRetailer.GetRetailerImageInfo();/*CC41*/
            };
            /*CC41 start*/
            if (DtRetailerImage != null && DtRetailerImage.Rows.Count > 0)
            {
                //gvAttachedImages.Visible = true;
                gvAttachedImages.DataSource = DtRetailerImage;
                gvAttachedImages.DataBind();
            } /*CC41 end*/
            if (DtSalesChannelDetail.Rows.Count > 0 && DtSalesChannelDetail != null)
            {
                txtAddress1.Text = DtSalesChannelDetail.Rows[0]["Address1"].ToString();
                txtAddress2.Text = DtSalesChannelDetail.Rows[0]["Address2"].ToString();
                cmbsaleschannel.SelectedItem.Selected = false;
                if /*#CC45 Added*/ (Convert.ToString(HttpContext.Current.Session["RetailerMultipleMapping"]) != "1")
                {
                    if (cmbsaleschannel.Items.FindByValue(DtSalesChannelDetail.Rows[0]["SalesChannelID"].ToString()) != null)
                    {
                        cmbsaleschannel.Items.FindByValue(DtSalesChannelDetail.Rows[0]["SalesChannelID"].ToString()).Selected = true;
                        /*#CC43 start*/
                        if ((SalesChannelLevel == 1) && (BaseEntityTypeID == 2))
                            cmbsaleschannel.Enabled = true;
                        else
                            cmbsaleschannel.Enabled = false;
                        //if (Convert.ToInt32(PageBase.SalesChanelID) != 0)             //this will select the login user in combo
                        //{
                        //    cmbsaleschannel.SelectedValue = PageBase.SalesChanelID.ToString();
                        //    cmbsaleschannel.Enabled = false;
                        //}
                        //else
                        //{
                        //    if(SalesChannelLevel==1)
                        //    cmbsaleschannel.Enabled = true;
                        //}    /*#CC43 end*/ 
                    }
                    else
                    {
                        cmbsaleschannel.SelectedIndex = 0;
                        cmbsaleschannel.Enabled = true;

                    }
                }
                else/*#CC45 Added End*/
                {
                    cmbsaleschannel.SelectedIndex = 0;
                    liSaleschannelHeading.Visible = false;
                    liSaleschannelddl.Visible = false;
                }

                cmbCountry.SelectedValue = DtSalesChannelDetail.Rows[0]["CountryID"].ToString();
                cmbCountry_SelectedIndexChanged(cmbCountry, new EventArgs());
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
                /*#CC19 START ADDED*/
                if (PageBase.TehsillDisplayMode == "1")
                { /* #CC24 Add Start */
                    cmbcity.ClearSelection();
                    if (cmbcity.Items.FindByValue(DtSalesChannelDetail.Rows[0]["CityID"].ToString()) == null)
                    {
                        cmbcity.SelectedValue = "0";
                    }
                    else
                    {
                        cmbcity.SelectedValue = DtSalesChannelDetail.Rows[0]["CityID"].ToString();
                    }

                    cmbcity_SelectedIndexChanged(cmbcity, new EventArgs());
                    /* #CC24 Add End */
                    cmbTehsil.ClearSelection();
                    if (cmbTehsil.Items.FindByValue(DtSalesChannelDetail.Rows[0]["TehsilID"].ToString()) == null)
                    {
                        cmbTehsil.SelectedValue = "0";
                    }
                    else
                    {
                        cmbTehsil.SelectedValue = DtSalesChannelDetail.Rows[0]["TehsilID"].ToString();
                    }
                    cmbTehsil_SelectedIndexChanged(null, null);
                    /* #CC24 Comment Start  cmbcity.ClearSelection();
                     if (cmbcity.Items.FindByValue(DtSalesChannelDetail.Rows[0]["CityID"].ToString()) == null)
                     {
                         cmbcity.SelectedValue = "0";
                     }
                     else
                     {
                         cmbcity.SelectedValue = DtSalesChannelDetail.Rows[0]["CityID"].ToString();
                     }
                     cmbcity_SelectedIndexChanged(null, null); #CC24 Comment End  */
                }
                else
                {
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
                }
                /*#CC19 START ADDED*/
                /*#CC19 START COMMENTED cmbcity.ClearSelection();
                if (cmbcity.Items.FindByValue(DtSalesChannelDetail.Rows[0]["CityID"].ToString()) == null)
                {
                    cmbcity.SelectedValue = "0";
                }
                else
                {
                    cmbcity.SelectedValue = DtSalesChannelDetail.Rows[0]["CityID"].ToString();
                }
                cmbcity_SelectedIndexChanged(cmbstate, new EventArgs()); #CC19 END COMMENTED */
                cmbArea.ClearSelection();
                if (cmbArea.Items.FindByValue(DtSalesChannelDetail.Rows[0]["AreaId"].ToString()) == null)
                {
                    cmbArea.SelectedValue = "0";
                }
                else
                {
                    cmbArea.SelectedValue = DtSalesChannelDetail.Rows[0]["AreaId"].ToString();
                }
                if (cmbRetailerType.Items.FindByValue(DtSalesChannelDetail.Rows[0]["RetailerTypeID"].ToString()) == null)
                    cmbRetailerType.SelectedValue = "0";
                else
                    cmbRetailerType.SelectedValue = DtSalesChannelDetail.Rows[0]["RetailerTypeID"].ToString();
                /*#CC46 Added Started*/
               Int64 value=Convert.ToInt64(cmbRetailerType.Attributes["BankDetailRequired" + cmbRetailerType.SelectedValue]);
                if(value==1)
                {
                    BankDetailsMandatory();
                }
                else
                {
                    BankDetailsNOnMandatory();
                }
                /*#CC46 Added End*/
                txtcontactperson.Text = DtSalesChannelDetail.Rows[0]["contactperson"].ToString();
                txtpincode.Text = DtSalesChannelDetail.Rows[0]["PinCode"] == System.DBNull.Value ? string.Empty : DtSalesChannelDetail.Rows[0]["PinCode"].ToString();
                txtphone.Text = DtSalesChannelDetail.Rows[0]["PhoneNumber"].ToString();
                txtmobile.Text = DtSalesChannelDetail.Rows[0]["MobileNumber"].ToString();
                txtcountersize.Text = DtSalesChannelDetail.Rows[0]["countersize"].ToString();
                txtCounterValue.Text = DtSalesChannelDetail.Rows[0]["CounterValue"].ToString(); /* #CC18 Added */

                txtemail.Text = DtSalesChannelDetail.Rows[0]["Email"].ToString();
                txtretailercode.Text = DtSalesChannelDetail.Rows[0]["retailercode"].ToString();
                txtretailername.Text = DtSalesChannelDetail.Rows[0]["retailername"].ToString();
                txttinno.Text = DtSalesChannelDetail.Rows[0]["tinnumber"] == System.DBNull.Value ? string.Empty : DtSalesChannelDetail.Rows[0]["tinnumber"].ToString();
                chkactive.Checked = Convert.ToBoolean(DtSalesChannelDetail.Rows[0]["Status"].ToString());
                ucDOB.Date = DtSalesChannelDetail.Rows[0]["DOB"].ToString();
                if (Convert.ToBoolean(DtSalesChannelDetail.Rows[0]["IsISPOnCounter"]) == true)
                    rblCounterIsp.SelectedValue = "1";
                else
                    rblCounterIsp.SelectedValue = "0";
                if (Convert.ToString(Session["RETHERARCHYPARENTMANDATORY"]) != "0") /* #CC34 Added */
                    cmbsaleschannel_SelectedIndexChanged(cmbsaleschannel, new EventArgs());  /* #CC33 commented  */ /* #CC34 Uncommented */
                if/*#CC45 Added*/ (Convert.ToString(HttpContext.Current.Session["RetailerMultipleMapping"]) != "1")
                {
                    if (ddlSalesman.Items.FindByValue(DtSalesChannelDetail.Rows[0]["SalesmanID"].ToString()) != null)
                    {
                        ddlSalesman.Items.FindByValue(DtSalesChannelDetail.Rows[0]["SalesmanID"].ToString()).Selected = true;
                    }
                    else
                    {
                        ddlSalesman.SelectedIndex = 0;
                    }
                }
                else /*#CC45 Added End*/
                {
                    ddlSalesman.SelectedIndex = 0;
                    tdsalesman.Visible = false;
                }
                txtpassword.Visible = false;
                reqpassword.Visible = false; /* #CC09 Added */
                tdPassword.Visible = false;
                txtUserName.Enabled = false;
                txtUserName.Text = DtSalesChannelDetail.Rows[0]["LoginName"].ToString();
                if (Convert.ToInt32(DtSalesChannelDetail.Rows[0]["GroupParentRetailerID"]) > 0)
                {
                    rdoChild.Visible = true;
                    rdoChild.SelectedIndex = 1;
                    rdoChild_SelectedIndexChanged(rdoChild, new EventArgs());
                    // ddlParentRetailer.Visible = true;
                    txtSearchedName.Visible = true;
                }
                //ddlParentRetailer.SelectedValue = DtSalesChannelDetail.Rows[0]["GroupParentRetailerID"].ToString();
                txtSearchedName.Text = DtSalesChannelDetail.Rows[0]["ParentRetailerName"].ToString();
                 hdnID.Value = DtSalesChannelDetail.Rows[0]["GroupParentRetailerID"].ToString();
                //hdnID.Text = DtSalesChannelDetail.Rows[0]["GroupParentRetailerID"].ToString();
                //hdnName.Text = DtSalesChannelDetail.Rows[0]["ParentRetailerName"].ToString();
                 hdnName.Value = DtSalesChannelDetail.Rows[0]["ParentRetailerName"].ToString();
                reqpassword.Enabled = false;
                if /*#CC45 Added*/(Convert.ToString(HttpContext.Current.Session["RetailerMultipleMapping"]) != "1")
                {
                    if (ddlOrghierarchy.Items.FindByValue(DtSalesChannelDetail.Rows[0]["OrgnhierarchyID"].ToString()) != null)
                    {
                        ddlOrghierarchy.Items.FindByValue(DtSalesChannelDetail.Rows[0]["OrgnhierarchyID"].ToString()).Selected = true;
                        /*#CC43 */
                        ddlOrghierarchy.Enabled = false;
                    }
                    else
                    {
                        ddlOrghierarchy.SelectedIndex = 0;
                    }
                }
                else/*#CC45 Added End*/
                {
                    ddlOrghierarchy.SelectedIndex = 0;
                    tdLabel.Visible = false;
                }
                //ddlOrghierarchy.SelectedValue = DtSalesChannelDetail.Rows[0]["OrgnhierarchyID"].ToString() == "" ? "0" : DtSalesChannelDetail.Rows[0]["OrgnhierarchyID"].ToString();
                //08-Feb-2013

                //ChkWantToSubmitOpeningStock.Visible = false;
                //ucOpeningStock.IsRequired = false;
                //ucOpeningStock.TextBoxDate.Enabled = false;
                //ucOpeningStock.imgCal.Enabled = false;
                if (DtSalesChannelDetail.Rows[0]["IsOpeningStockEnteredForRetailer"].ToString() == "True")
                {
                    //ucOpeningStock.Visible = true;
                    // ucOpeningStock.Date = DtSalesChannelDetail.Rows[0]["OpeningStockDate"].ToString();  //08-Feb-2013
                    ucOpeningStock.Date = Convert.ToDateTime(Session["RetailerOpStockDate"].ToString()).ToString("MM/dd/yyyy");
                    ucOpeningStock.imgCal.Enabled = false;
                    ucOpeningStock.TextBoxDate.Enabled = false;
                    //else
                }

                //    ucOpeningStock.Visible = false;
                /*#CC05 START ADDED*/
                if ((Request.QueryString["RetailerIdFromApproval"] != null) && (Request.QueryString["RetailerIdFromApproval"] != ""))
                {
                    if ((Convert.ToString(Session["RetailerApproval"]) == "1" || Convert.ToString(Session["RetailerApproval"]) == "0")
                        /* && (PageBase.HierarchyLevelID == 2)  #CC30 Commented  */ || (Convert.ToString(Session["RetailerApproval"]) == "2") /* #CC34 Approval Level 2 Added */)
                    {

                        btnProceed_Click(null, null);
                    }
                }



                if ((Request.QueryString["RetailerId"] != null) && (Request.QueryString["RetailerId"] != ""))
                {
                    trProceedAction.Attributes.CssStyle.Add("display", "block");
                    trAction.Attributes.CssStyle.Add("display", "none");
                    tdStatusLable.Attributes.CssStyle.Add("display", "none");
                    tdStatus.Attributes.CssStyle.Add("display", "none");




                }
                /*else if ((Request.QueryString["RetailerIdFromApproval"] != null) && (Request.QueryString["RetailerIdFromApproval"] != ""))
                 {
                     trProceedAction.Attributes.CssStyle.Add("display", "block");
                     trAction.Attributes.CssStyle.Add("display", "none");
                     tdStatusLable.Attributes.CssStyle.Add("display", "none");
                     tdStatus.Attributes.CssStyle.Add("display", "none");


                     rdoSelectMode.Items.Add(new ListItem("Interface", "2"));



                 }*/
                else if (Convert.ToString(Session["RetailerApproval"]) == "1")
                {
                    trProceedAction.Attributes.CssStyle.Add("display", "none");
                    trAction.Attributes.CssStyle.Add("display", "block");
                    /*
                     #CC29 Comment Start
                     tdStatusLable.Attributes.CssStyle.Add("display", "block");
                     tdStatus.Attributes.CssStyle.Add("display", "block");
                      #CC29 Comment End
                     */


                }

                else if ((Request.QueryString["RetailerIdFromApproval"] != null) && (Request.QueryString["RetailerIdFromApproval"] != ""))
                {
                    if (Convert.ToString(Session["RetailerApproval"]) == "0")
                    {

                        btnProceed_Click(null, null);
                    }
                }

                /*#CC05 START END*/

                /*#CC08 ADDED START*/

                txtBankName.Text = DtSalesChannelDetail.Rows[0]["BankName"].ToString();
                /* #CC05 Comment Start */
                /*
                txtBankAccountNumber.Text = DtSalesChannelDetail.Rows[0]["AccountNumber"].ToString();
                txtAccountHolder.Text = DtSalesChannelDetail.Rows[0]["AccountHolder"].ToString();
                 */
                /* #CC05 Comment End */
                /* #CC05 Add Start */
                txtBankAccountNumber.Text = DtSalesChannelDetail.Rows[0]["AccountNumber"].ToString(); /* #CC05 */
                txtAccountHolder.Text = DtSalesChannelDetail.Rows[0]["AccountHolder"].ToString();
                /* #CC05 Add End */

                txtIfscCode.Text = DtSalesChannelDetail.Rows[0]["IFSCCode"].ToString();
                txtBranchLocation.Text = DtSalesChannelDetail.Rows[0]["BranchLocation"].ToString();
                /*#CC08 ADDED END*/
                txtPANNo.Text = DtSalesChannelDetail.Rows[0]["PANNo"].ToString(); /* #CC10 Added */
                txtReferanceCode.Text = DtSalesChannelDetail.Rows[0]["ReferanceCode"].ToString(); /* #CC27 Added */
                txtWhatsNumber.Text = DtSalesChannelDetail.Rows[0]["WhatsAppNo"].ToString();/*CC40*/
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void cmbstate_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            using (GeographyData ObjGeography = new GeographyData())
            {
                if (cmbstate.SelectedIndex > 0)
                {
                    ObjGeography.SearchCondition = EnumData.eSearchConditions.Active;
                    ObjGeography.StateId = Convert.ToInt16(cmbstate.SelectedValue);
                    cmbcity.Items.Clear();
                    cmbArea.Items.Clear();
                    cmbArea.Items.Insert(0, new ListItem("Select", "0"));
                    /*#CC19 START ADDED*/
                    /* #CC21 Comment Start
                    if (PageBase.TehsillDisplayMode == "1")
                    {
                        cmbcity.Items.Insert(0, new ListItem("Select", "0"));
                        String[] StrCol = new String[] { "TehsilID", "TehsilName" };
                        PageBase.DropdownBinding(ref cmbTehsil, ObjGeography.GetAllActiveTehsil(), StrCol);
                    }
                    else
                    {
                        String[] StrCol = new String[] { "CityId", "CityName" };
                        PageBase.DropdownBinding(ref cmbcity, ObjGeography.GetAllCityByParameters(), StrCol);
                    }
                     #CC21 Comment End */
                    /*#CC19 START END*/
                    /*#CC19 START COMMENTED String[] StrCol = new String[] { "CityId", "CityName" };
                  PageBase.DropdownBinding(ref cmbcity, ObjGeography.GetAllCityByParameters(), StrCol); #CC19 START COMMENTED */
                    /* #CC21 Add Start */
                    String[] StrCol = new String[] { "CityId", "CityName" };
                    PageBase.DropdownBinding(ref cmbcity, ObjGeography.GetAllCityByParameters(), StrCol);

                    /* #CC21 Add End */
                }
                else if (cmbstate.SelectedIndex == 0)
                {
                    cmbcity.Items.Clear();
                    cmbArea.Items.Clear();
                    cmbArea.Items.Insert(0, new ListItem("Select", "0"));
                    cmbcity.Items.Insert(0, new ListItem("Select", "0"));
                    cmbTehsil.Items.Insert(0, new ListItem("Select", "0"));
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
                if (cmbcity.SelectedIndex > 0)
                {
                    ObjGeography.SearchCondition = EnumData.eSearchConditions.Active;
                    ObjGeography.CityId = Convert.ToInt16(cmbcity.SelectedValue);
                    //  cmbArea.Items.Clear();


                    /* #CC21 Add Start */
                    if (PageBase.TehsillDisplayMode == "1")
                    {
                        String[] StrCol = new String[] { "TehsilID", "TehsilName" };
                        PageBase.DropdownBinding(ref cmbTehsil, ObjGeography.GetAllActiveTehsil(), StrCol);
                    }
                    else
                    {
                        /* #CC21 Add End */
                        String[] StrCol = new String[] { "AreaID", "AreaName" };
                        PageBase.DropdownBinding(ref cmbArea, ObjGeography.GetAllAreaByParameters(), StrCol);
                    }
                }
                else if (cmbcity.SelectedIndex == 0)
                {
                    cmbArea.Items.Clear();
                    cmbArea.Items.Insert(0, new ListItem("Select", "0"));
                    /* #CC21 Add Start */
                    cmbTehsil.Items.Clear();
                    cmbTehsil.Items.Insert(0, new ListItem("Select", "0"));
                    /* #CC21 Add End */
                }

            };
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void rdoSelectMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (rdoSelectMode.SelectedValue == "1")
        //{
             /*#CC45 Added Started*/
            if(RetailerId!=0)
            {
                if (Convert.ToString(HttpContext.Current.Session["RetailerMultipleMapping"]) != "1")
                {
                    liSaleschannelHeading.Visible = true;
                    liSaleschannelddl.Visible = true;
                    tdLabel.Visible = true;
                    tdsalesman.Visible = true;
                }
                else
                {
                    liSaleschannelHeading.Visible = false;
                    liSaleschannelddl.Visible = false;
                    tdLabel.Visible = false;
                    tdsalesman.Visible = false;
                }
            }
            else
            {
                //Response.Redirect("MangeRetailerUpload.aspx");
                liSaleschannelHeading.Visible = true;
                liSaleschannelddl.Visible = true;
                tdLabel.Visible = true;
                tdsalesman.Visible = true;
            }
        /*#CC45 Added End*/

        //}
    }
    protected void cmbTehsil_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            using (GeographyData ObjGeography = new GeographyData())
            {
                if (cmbTehsil.SelectedIndex > 0)
                {
                    ObjGeography.SearchCondition = EnumData.eSearchConditions.Active;
                    ObjGeography.StateId = Convert.ToInt16(cmbstate.SelectedValue);
                    /*#CC19 START ADDED*/
                    if (PageBase.TehsillDisplayMode == "1")
                        ObjGeography.TehsilId = Convert.ToInt16(cmbTehsil.SelectedValue);
                    else
                        ObjGeography.TehsilId = 0;
                    /*#CC19 START END*/
                    /* cmbcity.Items.Clear(); #CC21 Commented */
                    cmbArea.Items.Clear();
                    cmbArea.Items.Insert(0, new ListItem("Select", "0"));
                    /* #CC21 Comment Start
                     String[] StrCol = new String[] { "CityId", "CityName" };
                    PageBase.DropdownBinding(ref cmbcity, ObjGeography.GetAllCityByParameters(), StrCol);
                     #CC21 Comment End
                     */
                    /* #CC21 Add Start */
                    String[] StrCol = new String[] { "AreaID", "AreaName" };
                    PageBase.DropdownBinding(ref cmbArea, ObjGeography.GetAllAreaByParameters(), StrCol);
                    /* #CC21 Add End */
                }
                else if (cmbstate.SelectedIndex == 0)
                {
                    /* cmbcity.Items.Clear();  #CC21 Commented */
                    cmbArea.Items.Clear();
                    cmbArea.Items.Insert(0, new ListItem("Select", "0"));
                    /* cmbcity.Items.Insert(0, new ListItem("Select", "0"));  #CC21 Commented */
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void cmbsaleschannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        //cmbstate.Items.Clear();
        if (cmbsaleschannel.SelectedIndex != 0)
        {
            using (SalesmanData ObjSalesman = new SalesmanData())
            {
                ObjSalesman.Type = EnumData.eSearchConditions.Active;
                //ObjSalesman.SalesChannelID = Convert.ToInt32(cmbsaleschannel.SelectedValue);
                /* ObjSalesman.SalesChannelID = PageBase.SalesChanelID; #CC39 Commented */
                Session["SaleschannelIdForretailer"] = Convert.ToInt32(cmbsaleschannel.SelectedValue);/*#CC44*/
                ObjSalesman.SalesChannelID = Convert.ToInt32(cmbsaleschannel.SelectedValue); /*#CC39 Added */
                String[] StrCol = new String[] { "SalesmanID", "Salesman" };
                PageBase.DropdownBinding(ref ddlSalesman, ObjSalesman.GetSalesmanInfo(), StrCol);
                ddlOrghierarchy.Items.Clear();
                FillHierarchy();
                if (RetailerId == 0)
                {
                    FillParentSalesChannelAreainformation();
                }
            };
            /* #Temp Add Start */
            if ((Request.QueryString["RetailerIdFromApproval"] != null) && (Request.QueryString["RetailerIdFromApproval"] != ""))
            {
                ddlSalesman.Enabled = false;
            }/* #Temp Add End */

        }
        else
        {
            ddlOrghierarchy.Items.Clear();
            //ddlSalesman.Items.Clear();
            //ddlSalesman.Items.Insert(0, new ListItem("Select", "0"));
            cmbstate.Items.Insert(0, new ListItem("Select", "0"));
        }
    }
    protected void cmbCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        cmbcity.Items.Clear();
        cmbcity.Items.Insert(0, new ListItem("Select", "0"));
        cmbArea.Items.Clear();
        cmbArea.Items.Insert(0, new ListItem("Select", "0"));
        if (cmbCountry.SelectedValue == "0")
        {
            cmbstate.Items.Clear();
            cmbstate.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            cmbstate.Items.Clear();
            using (MastersData obj = new MastersData())
            {
                DataTable dt;
                obj.StateSelectionMode = 1;
                obj.StateCountryid = Convert.ToInt32(cmbCountry.SelectedValue);
                /* #CC33 Add Start */
                //if (cmbsaleschannel.Items.Count == 0 && cmbsaleschannel.Items[0].Value != "0")
                //{
                //    obj.SalesChannelID = 0;
                //}
                //else  /* #CC33 Add End */
                obj.SalesChannelID = Convert.ToInt32(cmbsaleschannel.SelectedValue); /* #CC20 Added */
                /*obj.SalesChannelID = 0;*/
                dt = obj.SelectStateInfo();
                String[] colArray = { "StateID", "StateName" };
                PageBase.DropdownBinding(ref cmbstate, dt, colArray);

                /* #CC24 Add Start */
                if (PageBase.LOCATIONMAPPING == 1 && dt.Rows.Count == 0)
                {
                    ucMessage1.ShowInfo("ND-State Mapping doesnot exists for the sale channel.");
                }
                /* #CC24 Add End */


            }
        }



    }
    protected void rdoChild_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            if (rdoChild.SelectedValue == "1")
            {
                // ddlParentRetailer.Visible = true;
                lblParentRetailer.Visible = true;
                btnsearch.Visible = true;
                txtSearchedName.Visible = true;
                //using (SalesChannelData objSalesChannel = new SalesChannelData())
                //{
                // objSalesChannel.RoleType = EnumData.RoleType.Retailer;
                //objSalesChannel.SalesChannelID = PageBase.SalesChanelID;
                // String[] StrCol = new String[] { "RetailerID", "RetailerName" };
                //   PageBase.DropdownBinding(ref ddlParentRetailer, objSalesChannel.GetSalesChannelParentForGroup(), StrCol);
                ReqParent.Enabled = true;
                ReqParent.ValidationGroup = "Add";
                //};
                /*#CC45 Added Started*/
                if (RetailerId != 0)
                {
                    if (Convert.ToString(HttpContext.Current.Session["RetailerMultipleMapping"]) != "1")
                    {
                        liSaleschannelHeading.Visible = true;
                        liSaleschannelddl.Visible = true;
                        tdLabel.Visible = true;
                        tdsalesman.Visible = true;
                    }
                    else
                    {
                        liSaleschannelHeading.Visible = false;
                        liSaleschannelddl.Visible = false;
                        tdLabel.Visible = false;
                        tdsalesman.Visible = false;
                    }
                }
                else
                {
                    //Response.Redirect("MangeRetailerUpload.aspx");
                    liSaleschannelHeading.Visible = true;
                    liSaleschannelddl.Visible = true;
                    tdLabel.Visible = true;
                    tdsalesman.Visible = true;
                }
                /*#CC45 Added End*/
            }
            else
            {
                // ddlParentRetailer.Visible = false;
                txtSearchedName.Visible = false;
                btnsearch.Visible = false;
                lblParentRetailer.Visible = false;
                ReqParent.Enabled = false;
                ReqParent.ValidationGroup = "";
                  hdnID.Value = "0";/* #CC39 Added */
                //hdnID.Text = "0";
                /*#CC45 Added Started*/
                if (RetailerId != 0)
                {
                    if (Convert.ToString(HttpContext.Current.Session["RetailerMultipleMapping"]) != "1")
                    {
                        liSaleschannelHeading.Visible = true;
                        liSaleschannelddl.Visible = true;
                        tdLabel.Visible = true;
                        tdsalesman.Visible = true;
                    }
                    else
                    {
                        liSaleschannelHeading.Visible = false;
                        liSaleschannelddl.Visible = false;
                        tdLabel.Visible = false;
                        tdsalesman.Visible = false;
                    }
                }
                else
                {
                    //Response.Redirect("MangeRetailerUpload.aspx");
                    liSaleschannelHeading.Visible = true;
                    liSaleschannelddl.Visible = true;
                    tdLabel.Visible = true;
                    tdsalesman.Visible = true;
                }
                /*#CC45 Added End*/
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void lnkCreateLogin_Click(object sender, EventArgs e)
    {
        try
        {
            //if (hdnvalue.Value == "0")
            //{
            //      pnlGrid.Visible = false;
            //    hdnvalue.Value = "1";
            //    reqpassword.Enabled = false;
            //    reqUserName.Enabled = false;
            //    reqpassword.ValidationGroup = "";
            //    reqUserName.ValidationGroup = "";
            //}
            //else
            //{
            //    pnlGrid.Visible = true;
            //    hdnvalue.Value = "0";
            //    reqpassword.Enabled = true;
            //    reqUserName.Enabled = true;
            //    reqpassword.ValidationGroup = "Add";
            //    reqUserName.ValidationGroup = "Add";


            //}
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    void UserPanel()
    {
        try
        {
            if (Convert.ToString(Session["RetailerLogin"]) == "0")
            {
                pnlGrid.Visible = false;
                reqpassword.Enabled = false;
                reqUserName.Enabled = false;
                reqpassword.ValidationGroup = "";
                reqUserName.ValidationGroup = "";
            }
            else
            {
                pnlGrid.Visible = true;
                reqpassword.Enabled = true;
                reqUserName.Enabled = true;
                reqpassword.ValidationGroup = "Add";
                reqUserName.ValidationGroup = "Add";
                ucOpeningStock.IsRequired = false;/*#CC04 ADDED*/
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }

    /*CC40 start*/
    void WhatsappnumberValidation()
    {
        try
        {
            
            if (Convert.ToString(Session["WhatsAppMobileNumber"]) == "0")
            {

                rfvWhatsNumber.Enabled = false;
                RevWhatsAppNumber.Enabled = false;
                rfvWhatsNumber.ValidationGroup = "";
                RevWhatsAppNumber.ValidationGroup = "";
            }
            else
            {

                rfvWhatsNumber.Enabled = true;
                RevWhatsAppNumber.Enabled = true;
                rfvWhatsNumber.ValidationGroup = "Add";
                RevWhatsAppNumber.ValidationGroup = "Add";

            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    } /*CC40 end*/

    protected override void OnPreRender(EventArgs e)
    {
        txtpassword.Attributes.Add("value", txtpassword.Text);
        base.OnPreRender(e);
    }
    void FillHierarchy()
    {

        if (Session["RETAILERHIERLVLID"] != null && Convert.ToString(Session["RETHERARCHYPARENTMANDATORY"]) != "0")/* #CC39 RETHERARCHYPARENTMANDATORY Added */
        {
            if (Convert.ToInt32(Session["RETAILERHIERLVLID"]) > 0)
            {
                using (SalesChannelData ObjSalesChannel = new SalesChannelData())
                {
                    ddlOrghierarchy.Items.Clear();
                    ObjSalesChannel.SalesChannelID = Convert.ToInt32(cmbsaleschannel.SelectedValue);
                    //ObjSalesChannel.SalesChannelID = PageBase.SalesChanelID;
                    String[] StrCol1 = new String[] { "OrgnhierarchyID", "LocationName" };
                  /*  PageBase.DropdownBinding(ref ddlOrghierarchy, ObjSalesChannel.GetSalesChannelOrghierarchyRetailer(), StrCol1); #CC52 Commented */
                    /* #CC52 Add Start */
                    DataSet dsResult = ObjSalesChannel.GetOrghierarchyRetailerCreationV2();
                    PageBase.DropdownBinding(ref ddlOrghierarchy, dsResult.Tables[0], StrCol1);
                    /* #CC52 Add End */
                }
                reqOrgnhierarchy.ValidationGroup = "Add";
                ddlOrghierarchy.Visible = true;
                ReqParent.Enabled = true;
                tdLabel.Visible = true;

            }
            else
            {
                reqOrgnhierarchy.ValidationGroup = "";
                ddlOrghierarchy.Visible = false;
                ReqParent.Enabled = false;
                tdLabel.Visible = false;
            }

            /* #CC34 Add Start */
            if (Convert.ToString(Session["RETHERARCHYPARENTMANDATORY"]) == "1")
            {
                reqOrgnhierarchy.ValidationGroup = "Add";
            }
            else
            {
                reqOrgnhierarchy.ValidationGroup = "";
            }
            /* #CC34 Add End */
            /* #temp Add Start */
            if ((Request.QueryString["RetailerIdFromApproval"] != null) && (Request.QueryString["RetailerIdFromApproval"] != ""))
            {
                ddlOrghierarchy.Enabled = false;
            }/* #temp Add End */


        }



    }
    void FillParentSalesChannelAreainformation()
    {
        try
        {
            DataSet ds = new DataSet();
            using (MastersData obj = new MastersData())
            {
                cmbCountry.Items.Clear();
                cmbstate.Items.Clear();
                obj.CountrySelectionMode = 1;
                obj.SalesChannelID = Convert.ToInt32(cmbsaleschannel.SelectedValue);
                ds = obj.SelectSalesChannelAreainformation();
                if (ds != null && ds.Tables.Count > 0)
                {
                    String[] colArray = { "CountryID", "CountryName" };
                    PageBase.DropdownBinding(ref cmbCountry, ds.Tables[0], colArray);
                    // cmbstate.Items.Insert(0, new ListItem("Select", "0"));Pankaj
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    SelectedCountryID = Convert.ToInt32(ds.Tables[1].Rows[0]["CountryID"]);
                    if (cmbCountry.Items.FindByValue(SelectedCountryID.ToString()) != null)
                    {
                        cmbCountry.SelectedValue = SelectedCountryID.ToString();
                        cmbCountry_SelectedIndexChanged(cmbCountry, new EventArgs());
                        /* #CC24 Add Start */
                        if (cmbstate.Items.Count > 1)
                        {
                            /* #CC24 Add End */
                            cmbstate.SelectedValue = (ds.Tables[1].Rows[0]["StateID"]).ToString();
                            cmbstate_SelectedIndexChanged(cmbstate, new EventArgs());
                        } /* #CC24 Added */
                    }
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
        //    if (ChkWantToSubmitOpeningStock.Checked)
        //    {
        //        ucOpeningStock.Visible = true;

        //        FillDate();
        //    }
        //    else
        //    {
        //        ucOpeningStock.Visible = false;
        //        ucOpeningStock.Date = "";
        //        ucOpeningStock.IsRequired = false;
        //        ucOpeningStock.ValidationGroup = "NotRequired";
        //    }
    }
    void FillDate()
    {
        try
        {
            if (PageBase.BackDaysAllowedOpeningStock == 0 && Session["RetailerOpStockDate"].ToString() != "-1")/*#CC04 Change DefaultOpStockDate*/
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
                ucOpeningStock.IsRequired = true;
                // ucOpeningStock.TextBoxDate.Text = Session["RetailerOpStockDate"].ToString();
                ucOpeningStock.Date = Convert.ToDateTime(Session["RetailerOpStockDate"].ToString()).ToString("MM/dd/yyyy");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /*#CC05 START ADDED*/

    protected void btnProceed_Click(object sender, EventArgs e)
    {

        /*#CC46 Added Started*/
        int value = Convert.ToInt16(cmbRetailerType.Attributes["BankDetailRequired" + cmbRetailerType.SelectedValue]);
        if(value==1 && RetailerId ==0)/* #CC50 RetailerId ==0 Added */
        {
            if (Session["Tableuploadretailercheque"]==null)
            {
                lblRetailerCheck.ForeColor = System.Drawing.Color.Red;
                lblRetailerCheck.Visible = true;
                lblRetailerCheck.Text = "Please select Retailer cheque & PAN upload";
                return;
            }
            if(Session["RetailerShopImageuPLoad"]==null)
            {
                lblRetailerShop.ForeColor = System.Drawing.Color.Red;
                lblRetailerShop.Visible = true;
                lblRetailerShop.Text = "Please select Retailer Shop Image Upload";
                return;

            }
            if (Session["VisitingCardUpload"] == null)
            {
                lblVisiting.ForeColor = System.Drawing.Color.Red;
                lblVisiting.Visible = true;
                lblVisiting.Text = "Please select Visiting Card/Stationary Having Postal Address.";
                return;

            }
           
        }
        if (RetailerId != 0)
        {
            if (Convert.ToString(HttpContext.Current.Session["RetailerMultipleMapping"]) != "1")
            {
                liSaleschannelHeading.Visible = true;
                liSaleschannelddl.Visible = true;
                tdLabel.Visible = true;
                tdsalesman.Visible = true;
            }
            else
            {
                liSaleschannelHeading.Visible = false;
                liSaleschannelddl.Visible = false;
                tdLabel.Visible = false;
                tdsalesman.Visible = false;
                req1.ValidationGroup = "";
                req1.Attributes.CssStyle.Add("display", "none");
                reqOrgnhierarchy.ValidationGroup = "";
                reqOrgnhierarchy.Attributes.CssStyle.Add("display", "none");
            }
        }
        else
        {
            liSaleschannelHeading.Visible = true;
            liSaleschannelddl.Visible = true;
            tdLabel.Visible = true;
            tdsalesman.Visible = true;
            req1.ValidationGroup = "Add";
            req1.Attributes.CssStyle.Add("display", "block");
            reqOrgnhierarchy.ValidationGroup = "Add";
            reqOrgnhierarchy.Attributes.CssStyle.Add("display", "block");
        }
        /*#CC46 Added End*/
        /* #CC09 Add Start */
        string BankName = txtBankName.Text;

        if (ChkBankDetail.Checked == true)
        {
            txtBankName.Enabled = true;
            txtBankAccountNumber.Enabled = true;
            txtBranchLocation.Enabled = true;
            txtAccountHolder.Enabled = true;
            txtIfscCode.Enabled = true;
            txtPANNo.Enabled = true; /* #CC10 Added */

        }
        /* #CC09 Add End */

        /*#CC31 Started*/
        string cleanedString = System.Text.RegularExpressions.Regex.Replace(txtAddress1.Text.Trim(), @"\s+", "");
        int Address1Length = Convert.ToInt32(Resources.AppConfig.RetailerAddress1MinLength);/*#CC48 Added*/
        if (cleanedString.Length < Address1Length)
        {
            ucMessage1.ShowWarning("Address1 should be 25 character.");
        } /*#CC31 Started End*/
        else
        {
            using (RetailerData objRetailer = new RetailerData())
            {
                DataTable dt = new DataTable();
                objRetailer.RetailerName = txtretailername.Text.Trim();
                objRetailer.Address1 = txtAddress1.Text.Trim();
                objRetailer.CityID = Convert.ToInt16(cmbcity.SelectedValue);
                objRetailer.MobileNumber = txtmobile.Text.Trim();
                objRetailer.PhoneNumber = txtphone.Text.Trim();
                objRetailer.RetailerID = RetailerId;
                dt = objRetailer.CheckRetailer();
                if (objRetailer.OutPutResult == 0)
                {
                    lblIsRetailerFound.Text = "No similar retailer matched.";
                    GridRetailer.DataSource = null;
                    GridRetailer.DataBind();
                    trAction.Attributes.CssStyle.Add("display", "none");
                    trRetailerGrid.Attributes.CssStyle.Add("display", "none");
                    trAction.Attributes.CssStyle.Add("display", "block");
                }
                else
                {
                    lblIsRetailerFound.Text = "Similar retailer matched.";
                    trAction.Attributes.CssStyle.Add("display", "block");
                    trRetailerGrid.Attributes.CssStyle.Add("display", "block");
                    GridRetailer.DataSource = dt;
                    GridRetailer.DataBind();
                }

            }
        }
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        hdnApproveReject.Text = "1";
        /*#CC12 ADDED START*/
        if (txtApproalRemarks.Text.Trim() == "") /* #CC13 Trim Added */
        {
            txtApproalRemarks.Text = ""; /* #CC13 Added */
            ucMessage1.ShowInfo("Please enter rejection remarks.");
            return;
        }
        /*#CC12 ADDED END*/
        btnSubmit_Click(null, null);
    }
    /*#CC05 START END*/

    /* #CC07 Add Start */
    public int CheckBankDetailText()
    {
        int result = 0;
        if (txtBankName.Text == "" && txtAccountHolder.Text == "" && txtBankAccountNumber.Text == "" && txtBranchLocation.Text == "" && txtIfscCode.Text == "" && txtPANNo.Text == "")  /* #CC10 PanNumber Validation Added */
        {
            ucMessage1.Visible = false;
            result = 0;

        }
        else if (txtBankName.Text != "" && txtAccountHolder.Text != "" && txtBankAccountNumber.Text != "" && txtBranchLocation.Text != "" && txtIfscCode.Text != "" && txtPANNo.Text != "") /* #CC10 PanNumber Validation Added */
        {
            ucMessage1.Visible = false;
            result = 0;
        }

        else if (txtBankName.Text != "" || txtAccountHolder.Text != "" || txtBankAccountNumber.Text != "" || txtBranchLocation.Text != "" || txtIfscCode.Text != "" || txtPANNo.Text != "") /* #CC10 PanNumber Validation Added */
        {
            if (txtBankName.Text == "")
            {
                ucMessage1.ShowInfo("Please provide complete bank details.");
                result = 1;
            }
            else if (txtAccountHolder.Text == "")
            {
                ucMessage1.ShowInfo("Please provide complete bank details.");
                result = 1;
            }
            else if (txtBankAccountNumber.Text == "")
            {
                ucMessage1.ShowInfo("Please provide complete bank details.");
                result = 1;
            }
            else if (txtBranchLocation.Text == "")
            {
                ucMessage1.ShowInfo("Please provide complete bank details.");
                result = 1;
            }
            else if (txtIfscCode.Text == "")
            {
                ucMessage1.ShowInfo("Please provide complete bank details.");
                result = 1;
            }
            /* #CC10 Add Start */
            else if (txtPANNo.Text == "")
            {
                ucMessage1.ShowInfo("Please provide complete bank details.");
                result = 1;
            }
            /* #CC10 Add End */

        }
        return result;
    }
    /* #CC07 Add End */

    /* #CC18 Add Start */
    public void CounterPotentialVolume()
    {
        try
        {
            if (Convert.ToInt32(Session["PotentialVolDisplay"]) == 1)
            {
                trPotentalValue.Visible = true;
                regCounterValue.ValidationGroup = "Add";
                regCounterValue.Enabled = true;
            }
            else
            {
                trPotentalValue.Visible = false;
                regCounterValue.ValidationGroup = "";
                regCounterValue.Enabled = false;
            }

            if (Convert.ToInt32(Session["PotentialVolMandatry"]) == 1)
            {
                /*#CC36 Commented Started*/
                //counterpotentialmandatsign.Visible = true;
                //rqValidateCounterValue.ValidationGroup = "Add";
                //rqValidateCounterValue.Enabled = true;
                /*#CC36 Commented End*/

                /*#CC36 Added Started*/
                counterpotentialvolumnmandatsign.Visible = true;
                RequiredFieldValidator3.ValidationGroup = "Add";
                RequiredFieldValidator3.Enabled = true;
                /*#CC36 Added End*/
            }
            else
            {
                /*#CC36 Commented Started*/
                //counterpotentialmandatsign.Visible = false;
                //rqValidateCounterValue.ValidationGroup = "";
                //rqValidateCounterValue.Enabled = false;
                /*#CC36 Commented End*/
                /*#CC36 Added Started*/
                counterpotentialvolumnmandatsign.Visible = false;
                RequiredFieldValidator3.ValidationGroup = "";
                RequiredFieldValidator3.Enabled = false;
                /*#CC36 Added End*/
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    /* #CC18 Add End */
    /*#CC36 Added Started*/
    public void SalesManAndArea()
    {
        try
        {
            if (Convert.ToInt32(Session["SALESMANOPTIONAL"]) == 0)
            {
                divRetailerMandatory.Visible = true;
                RequiredFieldValidator4.ValidationGroup = "Add";
                RequiredFieldValidator4.Enabled = true;
            }
            else
            {
                divRetailerMandatory.Visible = false;
                RequiredFieldValidator4.ValidationGroup = "";
                RequiredFieldValidator4.Enabled = false;
            }

            if (Convert.ToInt32(Session["AREAOPTIONAL"]) == 0)
            {
                dvAreaMandotery.Visible = true;
                RequiredArea.ValidationGroup = "Add";
                RequiredArea.Enabled = true;

            }
            else
            {

                dvAreaMandotery.Visible = false;
                RequiredArea.ValidationGroup = "";
                RequiredArea.Enabled = false;

            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    /*#CC36 Added End*/
    /*protected void UploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
    {
        try
        {
            e.CallbackData = SavePostedFiles(e.UploadedFile);
        }
        catch (Exception ex)
        {
            e.IsValid = false;
            e.ErrorText = ex.Message;
        }
    }*/

    /*CC41 start*/
    protected void RetailerCheck_FileUploadComplete(object sender, EventArgs e)
    {

        if (RetailerId != 0)
        {
            if (Convert.ToString(HttpContext.Current.Session["RetailerMultipleMapping"]) != "1")
            {
                liSaleschannelHeading.Visible = true;
                liSaleschannelddl.Visible = true;
                tdLabel.Visible = true;
                tdsalesman.Visible = true;
            }
            else
            {
                liSaleschannelHeading.Visible = false;
                liSaleschannelddl.Visible = false;
                tdLabel.Visible = false;
                tdsalesman.Visible = false;
            }
        }
        else
        {
            //Response.Redirect("MangeRetailerUpload.aspx");
            liSaleschannelHeading.Visible = true;
            liSaleschannelddl.Visible = true;
            tdLabel.Visible = true;
            tdsalesman.Visible = true;
        }



        /*#CC46 Added Started*/
        /*
        dtImageDataActual = null;
        Session["Table"] = dtImageDataActual; #CC49 Commented   */
        /*#CC46 Added End*/
        /* #CC49 Add Start */
        if (Session["Table"] != null)
        {
            for (int i = ((DataTable)Session["Table"]).Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = ((DataTable)Session["Table"]).Rows[i];
                if (Convert.ToString(dr["ImageTypeId"]) == "1")
                {
                    dr.Delete();
                    ((DataTable)Session["Table"]).AcceptChanges();
                }
            }
        }
        /* #CC49 Add End */
        StringBuilder sb = new StringBuilder();
        string[] validFileTypes = { "bmp", "gif", "png", "jpg", "jpeg", "PNG", "JPG", "JPEG", "BMP" };
        string strFilePath;
        strFilePath = ZedService.Utility.ZedServiceUtil.GetUploadFilePath(Convert.ToDateTime(System.DateTime.Now), "../../UploadDownload/UploadPersistent/Retailer/");
        if (FileUploadRetailerCheck.HasFile)
        {
            try
            {
                /*#CC46 Added Started*/
                FileInfo fileInfo = new FileInfo(FileUploadRetailerCheck.FileName);
                string ext = System.IO.Path.GetExtension(FileUploadRetailerCheck.FileName);
                var fileLength = FileUploadRetailerCheck.PostedFile.ContentLength;
                int validatefilesize = Convert.ToInt32(Resources.AppConfig.UploadedImageSize);
                int ActualSize = 1024 * validatefilesize;
                if (fileLength > ActualSize)
                {
                    lblVisiting.Visible = true;
                    lblVisiting.Text = "File size must not exceed 2 MB.";
                    return;
                }
                bool isValidFile = false;

                for (int i = 0; i < validFileTypes.Length; i++)
                {

                    if (ext == "." + validFileTypes[i])
                    {

                        isValidFile = true;

                        break;

                    }

                }

                if (!isValidFile)
                {
                    lblRetailerCheck.Visible = true;
                    lblRetailerCheck.ForeColor = System.Drawing.Color.Red;

                    lblRetailerCheck.Text = "Invalid File. Please upload a File with extension " +

                                   string.Join(",", validFileTypes);
                    return;

                }
                /*#CC46 Added End*/
                string strTicks = System.DateTime.Now.Ticks.ToString();

                // string strFileUploadedName = strTicks + fileInfo.Name;
                string strFileUploadedName = Path.GetFileNameWithoutExtension(fileInfo.Name) + DateTime.Now.Ticks + fileInfo.Extension;/* #CC49 Added */
                /* string strFileUploadedName = fileInfo.Name; #CC49 Commented */
                string strTempPath = Server.MapPath("../../UploadDownload/UploadPersistent/Retailer/");
                string str1Path = "../../UploadDownload/UploadPersistent/Retailer/";


                if (!Directory.Exists(Server.MapPath(strFilePath)))
                    Directory.CreateDirectory(Server.MapPath(strFilePath));


                FileUploadRetailerCheck.PostedFile.SaveAs(strTempPath + strFileUploadedName);
                lblRetailerCheck.ForeColor = System.Drawing.Color.Black;
                lblRetailerCheck.Visible = true;
                lblRetailerCheck.Text = strFileUploadedName/* #CC49 Added */; /*fileInfo.Name; #CC49 Commented */


                if (Session["Table"] == null)
                {
                    dtImageDataActual = new DataTable();
                    dtImageDataActual = CreateImageDataTable();

                    DataRow dr = dtImageDataActual.NewRow();

                    //dr["ImageTypeId"] = Convert.ToInt32(ddlImagesType.SelectedValue);
                    //dr["ImageRelativePath"] = strFilePath.Replace("../../", "/") + "/" + strFileUploadedName;
                    dr["ImageRelativePath"] = str1Path + strFileUploadedName;


                    dr["FileLocation"] = strTempPath + strFileUploadedName;
                    dr["ImageTypeId"] = 1;
                    //dr["BinaryChangedImage"] = ImageToBinary(strTempPath + strFileUploadedName);
                    //dr["UploadDocTypeId"] = Convert.ToInt16(Session["DocTypeId"]) == 0 ? 2 : Convert.ToInt16(Session["DocTypeId"]);
                    dtImageDataActual.Rows.Add(dr);
                    dtImageDataActual.AcceptChanges();
                    Session["Table"] = dtImageDataActual;
                    Session["Tableuploadretailercheque"] = dtImageDataActual;

                }
                else
                {
                    //dtImageDataActual = (DataTable)Session["Table"];
                    //DataColumn[] columns = dtImageDataActual.Columns.Cast<DataColumn>().ToArray();
                    //bool imagetype = dtImageDataActual.AsEnumerable()
                    //    .Any(row => columns.Any(col => row[col].ToString() == "1"));
                    Int16 IsExists = 0;
                    foreach (DataRow dr in ((DataTable)Session["Table"]).Rows)
                    {
                        if (Convert.ToInt32(dr["ImageTypeID"]) == 1)
                        {
                            IsExists = 1;
                            break;
                        }
                    }

                    if (IsExists == 1)
                    {


                        foreach (DataRow dr in dtImageDataActual.Rows)
                        {
                            if (dr["ImageTypeId"].ToString() == "1") // getting the row to edit , change it as you need
                            {
                                dr["ImageTypeId"] = 1;
                                //dr["ImageRelativePath"] = strFilePath.Replace("../../", "/") + "/" + strFileUploadedName;
                                dr["ImageRelativePath"] = str1Path + strFileUploadedName;
                                dr["FileLocation"] = strTempPath + strFileUploadedName;
                            }
                            //dtImageDataActual.Rows.Add(dr);
                            dtImageDataActual.AcceptChanges();
                            Session["Table"] = dtImageDataActual;
                        }
                    }
                    else
                    {
                        dtImageDataActual = (DataTable)Session["Table"];
                        DataRow dr = dtImageDataActual.NewRow();
                        dr["ImageTypeId"] = 1;
                        //dr["ImageRelativePath"] = strFilePath.Replace("../../", "/") + "/" + strFileUploadedName;
                        dr["ImageRelativePath"] = str1Path + strFileUploadedName;
                        dr["FileLocation"] = strTempPath + strFileUploadedName;
                        //dr["ImageTypeName"] = ddlImageType.SelectedItem;
                        dtImageDataActual.Rows.Add(dr);
                        dtImageDataActual.AcceptChanges();
                        Session["Table"] = dtImageDataActual;
                    }

                }

                string fileLabel = strFileUploadedName/* #CC49 Added */; /*fileInfo.Name; #CC49 Commented */
                // string fileLength = FileUploadRetailerCheck.FileBytes.Length / 1024 + "K";

                //return string.Format("{0} ({1})|{2}", fileLabel, fileLength, VirtualPath + "/UploadDownload/UploadPersistent/" + strFileUploadedName);


            }
            catch (Exception ex)
            {
                sb.Append("<br/> Error <br/>");
                sb.AppendFormat("Unable to save file <br/> {0}", ex.Message);
            }
        }
       
    }
    protected void UploadControl_RetailerShopImageuPLoad(object sender, EventArgs e)
    {
        /*#CC45 Added Started*/
        if (RetailerId != 0)
        {
            if (Convert.ToString(HttpContext.Current.Session["RetailerMultipleMapping"]) != "1")
            {
                liSaleschannelHeading.Visible = true;
                liSaleschannelddl.Visible = true;
                tdLabel.Visible = true;
                tdsalesman.Visible = true;
            }
            else
            {
                liSaleschannelHeading.Visible = false;
                liSaleschannelddl.Visible = false;
                tdLabel.Visible = false;
                tdsalesman.Visible = false;
            }
        }
        else
        {
            //Response.Redirect("MangeRetailerUpload.aspx");
            liSaleschannelHeading.Visible = true;
            liSaleschannelddl.Visible = true;
            tdLabel.Visible = true;
            tdsalesman.Visible = true;
        }
        /*#CC45 Added End*/
        /*#CC46 Added Started*/
       /* dtImageDataActual = null;
        Session["Table"] = dtImageDataActual;#CC49 Commented   */
        /*#CC46 Added End*/
        /* #CC49 Add Start */
        if (Session["Table"] != null)
        {
            for (int i = ((DataTable)Session["Table"]).Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = ((DataTable)Session["Table"]).Rows[i];
                if (Convert.ToString(dr["ImageTypeId"]) == "2")
                {
                    dr.Delete();
                    ((DataTable)Session["Table"]).AcceptChanges();
                }
            }
        }
        /* #CC49 Add End */
        StringBuilder sb = new StringBuilder();
        string[] validFileTypes = { "bmp", "gif", "png", "jpg", "jpeg", "PNG", "JPG", "JPEG", "BMP" };
        string strFilePath;
        strFilePath = ZedService.Utility.ZedServiceUtil.GetUploadFilePath(Convert.ToDateTime(System.DateTime.Now), "../../UploadDownload/UploadPersistent/Retailer/");
        if (FileUploadShopImage.HasFile)
        {
            try
            {
                /*#CC46 Added Started*/
                FileInfo fileInfo = new FileInfo(FileUploadShopImage.FileName);
                string ext = System.IO.Path.GetExtension(FileUploadShopImage.FileName);
                var fileLength = FileUploadShopImage.PostedFile.ContentLength;
                int validatefilesize = Convert.ToInt32(Resources.AppConfig.UploadedImageSize);
                int ActualSize = 1024 * validatefilesize;
                if (fileLength > ActualSize)
                {
                    lblVisiting.Visible = true;
                    lblVisiting.Text = "File size must not exceed 2 MB.";
                    return;
                }
                bool isValidFile = false;

                for (int i = 0; i < validFileTypes.Length; i++)
                {
                    if (ext == "." + validFileTypes[i])
                    {
                        isValidFile = true;
                        break;
                    }
                }

                if (!isValidFile)
                {
                    lblRetailerShop.Visible = true;
                    lblRetailerShop.ForeColor = System.Drawing.Color.Red;
                    lblRetailerShop.Text = "Invalid File. Please upload a File with extension " + string.Join(",", validFileTypes);
                    return;

                }
                /*#CC46 Added Started*/
                string strTicks = System.DateTime.Now.Ticks.ToString();
                string strFileUploadedName = Path.GetFileNameWithoutExtension(fileInfo.Name) + DateTime.Now.Ticks + fileInfo.Extension;/* #CC49 Added */
               /*  string strFileUploadedName = fileInfo.Name; #CC49 Commented */
                //string strFileUploadedName = fileInfo.Name;
                string strTempPath = Server.MapPath("../../UploadDownload/UploadPersistent/Retailer/");
                string str1Path = "../../UploadDownload/UploadPersistent/Retailer/";

                if (!Directory.Exists(Server.MapPath(strFilePath)))
                    Directory.CreateDirectory(Server.MapPath(strFilePath));


                FileUploadShopImage.SaveAs(strTempPath + strFileUploadedName);
                lblRetailerShop.ForeColor = System.Drawing.Color.Black;
                lblRetailerShop.Visible = true;
                lblRetailerShop.Text = strFileUploadedName;
                //dtImageDataActual = (DataTable)Session["Table"];
                //DataColumn[] columns = dtImageDataActual.Columns.Cast<DataColumn>().ToArray();
                if ((Session["Table"] == null))
                {
                    dtImageDataActual = CreateImageDataTable();
                    //dtImageDataActual=(DataTable)Session["Table"];
                    DataRow dr = dtImageDataActual.NewRow();

                    dr["ImageTypeId"] = 2;
                    //dr["ImageRelativePath"] = strFilePath.Replace("../../", "/") + "/" + strFileUploadedName;
                    //dr["ImageRelativePath"] = strFilePath.Replace("../../", "/") + "/" + strFileUploadedName;
                    dr["ImageRelativePath"] = str1Path + strFileUploadedName;


                    dr["FileLocation"] = strTempPath + strFileUploadedName;

                    dtImageDataActual.Rows.Add(dr);
                    dtImageDataActual.AcceptChanges();
                    Session["Table"] = dtImageDataActual;
                    Session["RetailerShopImageuPLoad"] = dtImageDataActual;

                }
                else
                {
                    //       dtImageDataActual = (DataTable)Session["Table"];
                    //DataColumn[] columns1 = dtImageDataActual.Columns.Cast<DataColumn>().ToArray();
                    //bool imagetype = dtImageDataActual.AsEnumerable()
                    //    .Any(row => columns1.Any(col => row[col].ToString() == "2"));
                    Int16 IsExists = 0;
                    foreach (DataRow dr in ((DataTable)Session["Table"]).Rows)
                    {
                        if (Convert.ToInt32(dr["ImageTypeID"]) == 2)
                        {
                            IsExists = 1;
                            break;
                        }
                    }



                    if (IsExists == 1)
                    {
                        dtImageDataActual = (DataTable)Session["Table"];
                        foreach (DataRow dr in dtImageDataActual.Rows)
                        {
                            if (Convert.ToInt32(dr["ImageTypeID"]) == 2) // getting the row to edit , change it as you need
                            {
                                dr["ImageTypeId"] = 2;
                                //dr["ImageRelativePath"] = strFilePath.Replace("../../", "/") + "/" + strFileUploadedName;
                                dr["ImageRelativePath"] = str1Path + strFileUploadedName;

                                dr["FileLocation"] = strTempPath + strFileUploadedName;
                            }
                            dtImageDataActual.AcceptChanges();
                            Session["Table"] = dtImageDataActual;
                            Session["RetailerShopImageuPLoad"] = dtImageDataActual;
                        }
                    }
                    else
                    {
                        dtImageDataActual = (DataTable)Session["Table"];
                        DataRow dr = dtImageDataActual.NewRow();
                        dr["ImageTypeId"] = 2;
                        //dr["ImageRelativePath"] = strFilePath.Replace("../../", "/") + "/" + strFileUploadedName;
                        dr["ImageRelativePath"] = str1Path + strFileUploadedName;

                        dr["FileLocation"] = strTempPath + strFileUploadedName;
                        //dr["ImageTypeName"] = ddlImageType.SelectedItem;
                        dtImageDataActual.Rows.Add(dr);
                        dtImageDataActual.AcceptChanges();
                        Session["Table"] = dtImageDataActual;
                        Session["RetailerShopImageuPLoad"] = dtImageDataActual;
                    }

                }
                string fileLabel = strFileUploadedName/* #CC49 Added */; /*fileInfo.Name; #CC49 Commented */
                // string fileLength = FileUploadShopImage.FileBytes.Length / 1024 + "K";

                //}






                //return string.Format("{0} ({1})|{2}", fileLabel, fileLength, VirtualPath + "/UploadDownload/UploadPersistent/Retailer/" + strFileUploadedName);

            }

            catch (Exception ex)
            {
                sb.Append("<br/> Error <br/>");
                sb.AppendFormat("Unable to save file <br/> {0}", ex.Message);
            }
            //return string.Format("{0} ({1})|{2}", fileLabel, fileLength, VirtualPath + "/UploadDownload/UploadPersistent/" + strFileUploadedName);

        }
    }

    protected void UploadControl_VisitingCardUpload(object sender, EventArgs e)
    {

        /*#CC45 Added Started*/
        if (RetailerId != 0)
        {
            if (Convert.ToString(HttpContext.Current.Session["RetailerMultipleMapping"]) != "1")
            {
                liSaleschannelHeading.Visible = true;
                liSaleschannelddl.Visible = true;
                tdLabel.Visible = true;
                tdsalesman.Visible = true;
            }
            else
            {
                liSaleschannelHeading.Visible = false;
                liSaleschannelddl.Visible = false;
                tdLabel.Visible = false;
                tdsalesman.Visible = false;
            }
        }
        else
        {
            //Response.Redirect("MangeRetailerUpload.aspx");
            liSaleschannelHeading.Visible = true;
            liSaleschannelddl.Visible = true;
            tdLabel.Visible = true;
            tdsalesman.Visible = true;
        }
        /*#CC45 Added End*/

        /*#CC46 Added Started*/
       /* dtImageDataActual = null;
        Session["Table"] = dtImageDataActual;#CC49 Commented   */
        /*#CC46 Added End*/
        /* #CC49 Add Start */
        if (Session["Table"] != null)
        {
            for (int i = ((DataTable)Session["Table"]).Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = ((DataTable)Session["Table"]).Rows[i];
                if (Convert.ToString(dr["ImageTypeId"]) == "3")
                {
                    dr.Delete();
                    ((DataTable)Session["Table"]).AcceptChanges();
                }
            }
        }
        /* #CC49 Add End */

        StringBuilder sb = new StringBuilder();
        string[] validFileTypes = { "bmp", "gif", "png", "jpg", "jpeg", "PNG", "JPG", "JPEG", "BMP" };
        string strFilePath;
        strFilePath = ZedService.Utility.ZedServiceUtil.GetUploadFilePath(Convert.ToDateTime(System.DateTime.Now), "../../UploadDownload/UploadPersistent/Retailer/");
        if (FileUploadVisiting.HasFile)
        {
            try
            {

                FileInfo fileInfo = new FileInfo(FileUploadVisiting.FileName);
                /*#CC46 Added Started*/
                string ext = System.IO.Path.GetExtension(FileUploadVisiting.FileName);
                var fileLength = FileUploadVisiting.PostedFile.ContentLength;
                int validatefilesize = Convert.ToInt32(Resources.AppConfig.UploadedImageSize);
                int ActualSize = 1024 * validatefilesize;
                if (fileLength > ActualSize)
                {
                    lblVisiting.Visible = true;
                    lblVisiting.Text = "File size must not exceed 2 MB.";
                    return;
                }
                bool isValidFile = false;

                for (int i = 0; i < validFileTypes.Length; i++)
                {
                    if (ext == "." + validFileTypes[i])
                    {
                        isValidFile = true;
                        break;
                    }
                }
                if (!isValidFile)
                {
                    lblVisiting.Visible = true;
                    lblVisiting.ForeColor = System.Drawing.Color.Red;
                    lblVisiting.Text = "Invalid File. Please upload a File with extension " + string.Join(",", validFileTypes);
                    return;

                }
                /*#CC46 Added End*/
                string strTicks = System.DateTime.Now.Ticks.ToString();
                string strFileUploadedName = Path.GetFileNameWithoutExtension(fileInfo.Name) + DateTime.Now.Ticks + fileInfo.Extension;/* #CC49 Added */
                /*string strFileUploadedName = fileInfo.Name;  #CC49 Commented */
                //string strFileUploadedName = fileInfo.Name;
                string strTempPath = Server.MapPath("../../UploadDownload/UploadPersistent/Retailer/");
                string str1Path = "../../UploadDownload/UploadPersistent/Retailer/";

                if (!Directory.Exists(Server.MapPath(strFilePath)))
                    Directory.CreateDirectory(Server.MapPath(strFilePath));


                FileUploadVisiting.SaveAs(strTempPath + strFileUploadedName);
                lblVisiting.ForeColor = System.Drawing.Color.Black;
                lblVisiting.Visible = true;
                lblVisiting.Text = strFileUploadedName;
                //dtImageDataActual = (DataTable)Session["Table"];
                //DataColumn[] columns = dtImageDataActual.Columns.Cast<DataColumn>().ToArray();
                if ((Session["Table"] == null))
                {
                    dtImageDataActual = new DataTable();
                    dtImageDataActual = CreateImageDataTable();
                    //dtImageDataActual = (DataTable)Session["Table"];
                    DataRow dr = dtImageDataActual.NewRow();

                    dr["ImageTypeId"] = 3;
                    dr["ImageRelativePath"] = str1Path + strFileUploadedName;


                    dr["FileLocation"] = strTempPath + strFileUploadedName;
                    //dr["ImageTypeName"] = ddlImageType.SelectedItem;
                    //dr["BinaryChangedImage"] = ImageToBinary(strTempPath + strFileUploadedName);
                    //dr["UploadDocTypeId"] = Convert.ToInt16(Session["DocTypeId"]) == 0 ? 2 : Convert.ToInt16(Session["DocTypeId"]);
                    dtImageDataActual.Rows.Add(dr);
                    dtImageDataActual.AcceptChanges();
                    Session["Table"] = dtImageDataActual;
                    Session["VisitingCardUpload"] = dtImageDataActual;

                }
                else
                {
                    //       dtImageDataActual = (DataTable)Session["Table"];
                    //DataColumn[] columns1 = dtImageDataActual.Columns.Cast<DataColumn>().ToArray();
                    //bool imagetype = dtImageDataActual.AsEnumerable()
                    //    .Any(row => columns1.Any(col => row[col].ToString() == "2"));
                    Int16 IsExists = 0;
                    foreach (DataRow dr in ((DataTable)Session["Table"]).Rows)
                    {
                        if (Convert.ToInt32(dr["ImageTypeID"]) == 3)
                        {
                            IsExists = 1;
                            break;
                        }
                    }



                    if (IsExists == 1)
                    {
                        dtImageDataActual = (DataTable)Session["Table"];
                        foreach (DataRow dr in dtImageDataActual.Rows)
                        {
                            if (Convert.ToInt32(dr["ImageTypeID"]) == 3) // getting the row to edit , change it as you need
                            {
                                dr["ImageTypeId"] = 3;
                                dr["ImageRelativePath"] = str1Path + strFileUploadedName;

                                dr["FileLocation"] = strTempPath + strFileUploadedName;
                            }
                            dtImageDataActual.AcceptChanges();
                            Session["Table"] = dtImageDataActual;
                            Session["VisitingCardUpload"] = dtImageDataActual;
                        }
                    }
                    else
                    {
                        dtImageDataActual = (DataTable)Session["Table"];
                        DataRow dr = dtImageDataActual.NewRow();
                        dr["ImageTypeId"] = 3;
                        dr["ImageRelativePath"] = str1Path + strFileUploadedName;

                        dr["FileLocation"] = strTempPath + strFileUploadedName;
                        //dr["ImageTypeName"] = ddlImageType.SelectedItem;
                        dtImageDataActual.Rows.Add(dr);
                        dtImageDataActual.AcceptChanges();
                        Session["Table"] = dtImageDataActual;
                        Session["VisitingCardUpload"] = dtImageDataActual;
                    }





                }
                string fileLabel = strFileUploadedName/* #CC49 Added */; /*fileInfo.Name; #CC49 Commented */

            }
                

                //return string.Format("{0} ({1})|{2}", fileLabel, fileLength, VirtualPath + "/UploadDownload/UploadPersistent/" + strFileUploadedName);



            catch (Exception ex)
            {
                sb.Append("<br/> Error <br/>");
                sb.AppendFormat("Unable to save file <br/> {0}", ex.Message);
            }
        }
    }
    //            else
    //            {
    //                dtImageDataActual = (DataTable)Session["Table"];

    //                DataColumn[] columns1 = dtImageDataActual.Columns.Cast<DataColumn>().ToArray();
    //                //bool imagetype = dtImageDataActual.AsEnumerable()
    //                //    .Any(row => columns1.Any(col => row[col].ToString() == "3"));


    //                //if (imagetype == true)
    //                    if (dtImageDataActual.AsEnumerable()
    //                    .Any(row => columns1.Any(col => row[col].ToString() == "3")))
    //                {
    //                    foreach (DataRow dr in dtImageDataActual.Rows)
    //                    {
    //                        if (dr["ImageTypeId"].ToString() == "3") // getting the row to edit , change it as you need
    //                        {
    //                            dr["ImageTypeId"] = 3;
    //                            dr["ImageRelativePath"] = strFilePath.Replace("../../", "/") + "/" + strFileUploadedName;

    //                            dr["FileLocation"] = strTempPath + strFileUploadedName;
    //                        }



    //                        //dtImageDataActual.Rows.Add(dr);
    //                        dtImageDataActual.AcceptChanges();
    //                        Session["Table"] = dtImageDataActual;
    //                    }
    //                }
    //                dtImageDataActual = (DataTable)Session["Table"];


    //                if (dtImageDataActual.AsEnumerable()
    //                    .Any(row => columns1.Any(col => row[col].ToString() == "3")))
    //                {

    //                    DataRow[] dupresults = dtImageDataActual.Select("ImageTypeId");
    //                    DataTable dtTemp = dtImageDataActual.DefaultView.ToTable(true, "ImageTypeId");
    //                    DataRow dr = dtImageDataActual.NewRow();
    //                    dr["ImageTypeId"] = 3;
    //                    dr["ImageRelativePath"] = strFilePath.Replace("../../", "/") + "/" + strFileUploadedName;

    //                    dr["FileLocation"] = strTempPath + strFileUploadedName;
    //                    //dr["ImageTypeName"] = ddlImageType.SelectedItem;
    //                    dtImageDataActual.Rows.Add(dr);
    //                    dtImageDataActual.AcceptChanges();
    //                    Session["Table"] = dtImageDataActual;

    //                }
    //            }

    //            string fileLabel = fileInfo.Name;
    //            string fileLength = FileUploadVisiting.FileBytes.Length / 1024 + "K";

    //            //return string.Format("{0} ({1})|{2}", fileLabel, fileLength, VirtualPath + "/UploadDownload/UploadPersistent/" + strFileUploadedName);


    //        }
    //        catch (Exception ex)
    //        {
    //            sb.Append("<br/> Error <br/>");
    //            sb.AppendFormat("Unable to save file <br/> {0}", ex.Message);
    //        }
    //    }
    //}



    DataTable CreateImageDataTable()
    {


        dtImageData = new DataTable();
        DataColumn dc = new DataColumn("CtrlID");
        dc.DataType = System.Type.GetType("System.Int32");
        dc.AutoIncrement = true;
        dc.AutoIncrementSeed = 1;
        dtImageData.Columns.Add(dc);
        dtImageData.Columns.Add("ImageTypeId", typeof(System.Int16));
        dtImageData.Columns.Add("FileLocation", typeof(string));
        dtImageData.Columns.Add("ImageRelativePath", typeof(string));

        return dtImageData;
    }
    public void saveRetailerImage(int RetailerId)
    {
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();

        try
        {
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataTable FinalFileData = new DataTable();
            //FinalFileData = (DataTable)dtImageDataActual.Copy();
            DataTable FinalFileData1 = new DataTable();
            if (Session["Table"] != null)
            {
                FinalFileData = (DataTable)Session["Table"];
                FinalFileData.TableName = "Table";
                dt = FinalFileData.Copy();
                ds.Tables.Add(dt);
            }
            XMLImage = ds.GetXml();


            using (RetailerData obj = new RetailerData())
            {
                obj.RetailerID = RetailerId;
                obj.UserID = PageBase.UserId;
                obj.WOFileXML = XMLImage;

                obj.SaveImgSaperateByProcess();
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());

        }


    }
    protected void DownloadFile(object sender, EventArgs e)
    {

        try
        {
            string filePath = (sender as LinkButton).CommandArgument;
            //Response.ContentType = ContentType;
            Response.ContentType = "image/jpg";

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

    }
    /*CC41 end*/
    /*#CC46 Added Started*/
    public void GetsampleImage(string imagename)
    {
        string filePath = "../../UploadDownload/UploadPersistent/DownloadSampleImage/" + imagename;
        if (imagename != string.Empty)
        {
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            response.AddHeader("Content-Disposition", "attachment;filename=\"" + imagename + "\"");
            byte[] data = req.DownloadData(Server.MapPath(filePath));
            response.BinaryWrite(data);
            response.End();
        }
    }
    protected void lnkretailerchequeimage_Click(object sender, EventArgs e)
    {
        GetsampleImage("Cheque&PanCard.jpg");
    }
    protected void Lnkdownloadshopimage_Click(object sender, EventArgs e)
    {
        GetsampleImage("ShopImage.jpg");
    }
    protected void lnkdownloadvisitingcard_Click(object sender, EventArgs e)
    {
        GetsampleImage("VisitingCard_OR_Bill.jpg");
    }
  
    protected void cmbRetailerType_SelectedIndexChanged(object sender, EventArgs e)
    {
        int value = Convert.ToInt16(cmbRetailerType.Attributes["BankDetailRequired" + cmbRetailerType.SelectedValue]);
        try
        {
            if (value == 1)
            {
               
                BankDetailsMandatory();
            }
            else
            {
                BankDetailsNOnMandatory();
               
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }



    }
    private void BankDetailsMandatory()
    {
        Retailerchequevalid.Visible = true;
        RetailerShopImageRequired.Visible = true;
        VisitingCardRequired.Visible = true;
        if (Convert.ToString(Session["RetailerBankDetail"]) == "1")
        {
            nameofbank.Visible = true;
            Accountholdername.Visible = true;
            BankAccountnumber.Visible = true;
            Branchlocation.Visible = true;
            IFSCcode.Visible = true;
            Pannumber.Visible = true;
            RQFNameofBank.Enabled = true;
            RQFNameofBank.ValidationGroup = "Add";
            RQFAccountholderName.Enabled = true;
            RQFAccountholderName.ValidationGroup = "Add";
            RQFbankaccountnumber.Enabled = true;
            RQFbankaccountnumber.ValidationGroup = "Add";


            RQFBranchLocation.Enabled = true;
            RQFBranchLocation.ValidationGroup = "Add";

            RQFIFSCCode.Enabled = true;
            RQFIFSCCode.ValidationGroup = "Add";

            RQFPannumber.Enabled = true;
            RQFPannumber.ValidationGroup = "Add";
        }
    }
    private void BankDetailsNOnMandatory()
    {
        Retailerchequevalid.Visible = false;
        if (Convert.ToString(Session["RetailerBankDetail"]) == "1")
        {
            nameofbank.Visible = false;
            Accountholdername.Visible = false;
            BankAccountnumber.Visible = false;
            Branchlocation.Visible = false;
            IFSCcode.Visible = false;
            Pannumber.Visible = false;
            RQFNameofBank.Enabled = false;
            RQFNameofBank.ValidationGroup = "";
            RQFAccountholderName.Enabled = false;
            RQFAccountholderName.ValidationGroup = "";
            RQFbankaccountnumber.Enabled = false;
            RQFbankaccountnumber.ValidationGroup = "";
            RQFBranchLocation.Enabled = false;
            RQFBranchLocation.ValidationGroup = "";
            RQFIFSCCode.Enabled = false;
            RQFIFSCCode.ValidationGroup = "";
            RQFPannumber.Enabled = false;
            RQFPannumber.ValidationGroup = "";
        }
        else
        {
            nameofbank.Visible = false;
            Accountholdername.Visible = false;
            BankAccountnumber.Visible = false;
            Branchlocation.Visible = false;
            IFSCcode.Visible = false;
            Pannumber.Visible = false;
            RQFNameofBank.Enabled = false;
            RQFNameofBank.ValidationGroup = "";
            RQFAccountholderName.Enabled = false;
            RQFAccountholderName.ValidationGroup = "";
            RQFbankaccountnumber.Enabled = false;
            RQFbankaccountnumber.ValidationGroup = "";
            RQFBranchLocation.Enabled = false;
            RQFBranchLocation.ValidationGroup = "";
            RQFIFSCCode.Enabled = false;
            RQFIFSCCode.ValidationGroup = "";
            RQFPannumber.Enabled = false;
            RQFPannumber.ValidationGroup = "";
        }
    }
    /*#CC46 Added End*/
}
