using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;
using System.IO;
using System.Text.RegularExpressions;
using ZedService;
using System.Net;
using System.Xml;
/*
 * 09 Jun 2015, Karam Chand Sharma, #CC01, Add stock opening date in upload excel sheet
 * 20-Oct-2015, Sumit Maurya, #CC02, New Validation Added in Excel upload for Mobile Number and Phone Number.
 * 28-Oct-2015, Sumit Maurya, #CC03, New Bank Detail Columns Added to download in Excel Template if Session["RetailerBankDetail"] = 1 and validation Added for Bank Detail and PageBase.UserID supplied.
 * 30-Oct-2015, Sumit Maurya, #CC04, new Column PANNo for BankDetail section of Retailer.
 * 07-Jan-2015, Sumit Maurya, #CC05, Band Detail check added as it was missing.
 * 17-Mar-2016, Sumit Maurya, #CC06, New column added in Excel template according to config value PotentialVolDisplay.
 * 29-Mar-2016, Sumit Maurya, #CC07, New check added to validate only numbers are allowed in CounterPotentailValue.
 * 30-Mar-2016, Sumit Maurya, #CC08, New check added to add new column TehsilName when user download template according to config. Adding data in TehsilName column and validation applied on its data/value.
 * 27-May-2016, Sumit Maurya, #CC09, Changed Tehsil column location from after state to after city.
 * 07-Jun-2016, Sumit Maurya, #CC10, Tin Number label changed to VAT No. according to config.
 * 24-Jun-2016, Sumit Maurya, #CC11, Phone Number validation, Maxlength, Error message applied by config (AppConfig.regex).
 * 29-Aug-2016, Sumit Maurya, #CC12, New column "ReferanceCode" added in Excel download template and validation added.
 * 20 Sep 2014, Karam Chand Sharma, #CC13,  Change the label Address Line 1 and Address Line 2 to Address1 and Address2 respectively
 * 10 Aug 2017, Sumit Maurya, #CC14,  Column Name changed from VATNumber to GSTNumber, ISPCounter to ISP/ISDCounter. (Done for Comio)
 * 31-Oct-2017,Vijay Kumar Prajapati,#CC15,Validation For Address1 minlength 25 in Retailor upload Excel For Comio.
 * 09-Jan-2018,Vijay Kumar Prajapati,#CC16,PinCode Manadetory for lemanMobile.
 * 10-Jan-2018,Vijay Kumar Prajapati,#CC17,Add For Download tamplete for Add Heade Color For mandatory Field For LemonMobile.
 * 02-Apr-2018, Sumit Maurya, #CC18, Code implemted according to V5.
 * 15-May-2018, Sumit Maurya, #CC19, Check added to validate excel file columns according to config (Done for Motorola)
  * 22-May-2018, Rajnish Kumar, #CC20, WhatsAppNumber  Columns Added to download in Excel Template if Session["WhatsAppMobileNumber"] = 1 and validation Added for WhatsAppNUMber.
  * 15-June-2018, Rajnish Kumar, #CC21, UserId is Passed in downloadrefesheet of retailer upload
 * 28-Jun-2018, Rakesh Raj, #CC22, Added Retailer Update Feature to Bulk Update rtailer using excel sheet, Add InvalidData Link download
 */
public partial class Masters_HO_Retailer_MangeRetailerUpload : PageBase
{
    #region Upload Variables
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    UploadFile UploadFile = new UploadFile();
    OpenXMLExcel objexcel = new OpenXMLExcel();
    string strPSalt, strPassword;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        ucmassege1.ShowControl = false;
        if (!IsPostBack)
        {
            Session["RetailerUploadData"] = null;
        }

    }
    protected void rdoSelectMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoSelectMode.SelectedValue == "2")
        {
            Response.Redirect("ManageRetailer.aspx");
        }

    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        //#CC22
        if (ddlUploadType.SelectedValue == "0")
        {
            ucmassege1.ShowInfo("Please select Upload Type!");
            return;
        }
        //if (ViewState["Retailer"] != null)
        //    ViewState["Retailer"] = null;
        //pnlGrid.Visible = false;
        DataSet objDS = new DataSet();
        try
        {

            byte isSuccess = 1;
            Int16 UploadCheck = 0;
            String RootPath = Server.MapPath("../../");
            UploadFile.RootFolerPath = RootPath;
            
            UploadCheck = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            ViewState["TobeUploaded"] = strUploadedFileName;
            if (UploadCheck == 1)
            {
                //#CC22
                if (ddlUploadType.SelectedValue == "2")
                {
                    Session["UPDRTLR"] = "1";

                }
                // RemoveColumns();
                UploadFile.UploadedFileName = strUploadedFileName;
                UploadFile.UploadValidationType = EnumData.eUploadExcelValidationType.eRetailerUpload;
                isSuccess = UploadFile.uploadValidExcelRetailer(ref objDS, "Retailer");


                /* #CC19 Add Start */
                if (Convert.ToString(Session["RETHERARCHYPARENTMANDATORY"]) == "0")
                {
                    if (objDS != null && objDS.Tables.Count > 0)
                    {
                        if (objDS.Tables[0] != null && objDS.Tables[0].Columns.Contains("MappedOrgNHierarchy") == true)
                        {
                            ucmassege1.ShowInfo("Invalid excel sheet, columns doesn't match to required feilds");
                            return;
                        }

                    }
                }
                /* #CC19 Add End */



                /* #CC02 Add Start */
                int intError = 0;
                if (objDS.Tables[0].Columns.Contains("Error"))
                {
                    intError = 1;
                }
                if (!objDS.Tables[0].Columns.Contains("Error"))
                {
                    objDS.Tables[0].Columns.Add("Error", typeof(string));
                }

                /* #CC08 Add Start */

                if (PageBase.TehsillDisplayMode == "1")
                {
                    if (!objDS.Tables[0].Columns.Contains("TehsilName"))
                    {
                        objDS.Tables[0].Columns.Add("TehsilName", typeof(string)).SetOrdinal(objDS.Tables[0].Columns.Count - 13);
                    }
                }
                /* #CC08 Add End */

                /* #CC10 Add Start */
                if (PageBase.ChangeTinLabel == 1)
                {
                    if (objDS.Tables[0].Columns.Contains("TinNumber"))
                    {
                        objDS.Tables[0].Columns["TinNumber"].ColumnName = /* "VATNumber" #CC14 Commented */ "GSTNumber" /* #CC14 Added */;
                    }
                }
                /* #CC10 Add End */

                /* #CC03 Add Start */
                if (Session["RetailerBankDetail"] != null)
                {
                    if (Convert.ToInt32(Session["RetailerBankDetail"]) == 1)
                    {
                        int OrdinalCount = 2;
                        /* #CC04 Add Start */
                        objDS.Tables[0].Columns.Add("PANNo", typeof(string)).SetOrdinal(objDS.Tables[0].Columns.Count - OrdinalCount);
                        objDS.Tables[0].AcceptChanges();
                        /* #CC04 Add End */
                        objDS.Tables[0].Columns.Add("IFSCCode", typeof(string)).SetOrdinal(objDS.Tables[0].Columns.Count - 3);
                        objDS.Tables[0].AcceptChanges();
                        objDS.Tables[0].Columns.Add("BranchLocation", typeof(string)).SetOrdinal(objDS.Tables[0].Columns.Count - 4);
                        objDS.Tables[0].AcceptChanges();
                        objDS.Tables[0].Columns.Add("BankAccountNumber", typeof(string)).SetOrdinal(objDS.Tables[0].Columns.Count - 5);
                        objDS.Tables[0].AcceptChanges();
                        objDS.Tables[0].Columns.Add("AccountHolderName", typeof(string)).SetOrdinal(objDS.Tables[0].Columns.Count - 6);
                        objDS.Tables[0].AcceptChanges();
                        objDS.Tables[0].Columns.Add("BankName", typeof(string)).SetOrdinal(objDS.Tables[0].Columns.Count - 7);
                        objDS.Tables[0].AcceptChanges();


                    }
                }
                /*CC20 start*/
                if (Session["WhatsAppMobileNumber"] != null)
                {
                    if (Convert.ToInt32(Session["WhatsAppMobileNumber"]) == 1)
                    {
                        int OrdinalCount = 2;
                        /* #CC04 Add Start */
                        objDS.Tables[0].Columns.Add("WhatsAppNumber", typeof(string)).SetOrdinal(objDS.Tables[0].Columns.Count - 2);
                        objDS.Tables[0].AcceptChanges();



                    }
                } /*CC20 end*/
                /* #CC06 Add Start */
                //if (Convert.ToString(Session["PotentialVolMandatry"]) == "1")
                if (Convert.ToString(Session["PotentialVolDisplay"]) == "1")
                {
                    if (!objDS.Tables[0].Columns.Contains("CounterPotentialValue"))
                    {
                        objDS.Tables[0].Columns.Add("CounterPotentialValue", typeof(string)).SetOrdinal(objDS.Tables[0].Columns.Count - 3);
                    }
                }

                /* #CC06 Add End */

                /*CC22 start*/
                if (Session["UPDRTLR"] != null)
                {
                    if (Convert.ToInt32(Session["UPDRTLR"]) == 1)
                    {
                        objDS.Tables[0].Columns.Add("RetailerCode", typeof(string));
                        objDS.Tables[0].Columns.Add("Active", typeof(string));
                        objDS.Tables[0].AcceptChanges();

                    }
                } /*CC20 end*/



                DataTable dtBankDetail = new DataTable();

                DataSet dsBankDetail = new DataSet();
                dsBankDetail = (DataSet)Session["RetailerUploadData"];

                /* #CC08 Add Start */
                DataSet dsRetailerDetail = new DataSet();
                dsRetailerDetail = (DataSet)Session["RetailerUploadData"];
                if (PageBase.TehsillDisplayMode == "1")
                {
                    if (dsRetailerDetail.Tables[0].Columns.Contains("TehsilName"))
                    {
                        for (int i = 0; i < dsRetailerDetail.Tables[0].Rows.Count; i++)
                        {
                            objDS.Tables[0].Rows[i]["TehsilName"] = dsRetailerDetail.Tables[0].Rows[i]["TehsilName"];
                        }
                    }
                }

                /* #CC08 Add End */

                /* #CC05 Add Start */
                if (dsBankDetail.Tables[0].Columns.Contains("BankName"))
                {
                    /* #CC05 Add End */
                    string BankDetailColumnNames = "BankName,AccountHolderName,BankAccountNumber,BranchLocation,IFSCCode,PANNo"; /* #CC04 "PANNo" Added */
                    string[] BankDetailColumnSplit = BankDetailColumnNames.Split(',');
                    foreach (string BankDetailColumn in BankDetailColumnSplit)
                    {
                        for (int i = 0; i < dsBankDetail.Tables[0].Rows.Count; i++)
                        {
                            objDS.Tables[0].Rows[i][BankDetailColumn] = dsBankDetail.Tables[0].Rows[i][BankDetailColumn];

                        }
                    }
                }/* #CC05 Added */

                /* #CC06 Add Start */
                // if (Convert.ToString(Session["PotentialVolMandatry"]) == "1")
                if (Convert.ToString(Session["PotentialVolDisplay"]) == "1")
                {
                    if (dsBankDetail.Tables[0].Columns.Contains("CounterPotentialValue"))
                    {
                        string CounterPotentialValueColumnNames = "CounterPotentialValue";
                        string[] CounterPotentialValueColumnNamesSplit = CounterPotentialValueColumnNames.Split(',');
                        foreach (string BankDetailColumn in CounterPotentialValueColumnNamesSplit)
                        {
                            for (int i = 0; i < dsBankDetail.Tables[0].Rows.Count; i++)
                            {

                                objDS.Tables[0].Rows[i]["CounterPotentialValue"] = dsBankDetail.Tables[0].Rows[i]["CounterPotentialValue"];
                            }
                        }
                    }
                }

                /*CC20 start*/
                if (Convert.ToString(Session["WhatsAppMobileNumber"]) == "1")
                {
                    if (dsBankDetail.Tables[0].Columns.Contains("WhatsAppNumber"))
                    {
                        string WhatsAppNumber = "WhatsAppNumber";
                        string[] WhatsAppNumberColumnNamesSplit = WhatsAppNumber.Split(',');
                        foreach (string BankDetailColumn in WhatsAppNumberColumnNamesSplit)
                        {
                            for (int i = 0; i < dsBankDetail.Tables[0].Rows.Count; i++)
                            {

                                objDS.Tables[0].Rows[i]["whatsAppnumber"] = dsBankDetail.Tables[0].Rows[i]["whatsAppnumber"];
                            }
                        }
                    }
                }/*CC20 end*/

                /*CC22 start*/
                if (Convert.ToString(Session["UPDRTLR"]) == "1")
                {
                    if (dsBankDetail.Tables[0].Columns.Contains("RetailerCode"))
                    {
                        string RetailerCode = "RetailerCode";
                        string[] RetailerCodeColumnNamesSplit = RetailerCode.Split(',');
                        foreach (string BankDetailColumn in RetailerCodeColumnNamesSplit)
                        {
                            for (int i = 0; i < dsBankDetail.Tables[0].Rows.Count; i++)
                            {

                                objDS.Tables[0].Rows[i]["RetailerCode"] = dsBankDetail.Tables[0].Rows[i]["RetailerCode"];
                            }
                        }

                        string Active = "Active";
                        string[] ActiveColumnNamesSplit = Active.Split(',');
                        foreach (string BankDetailColumn in ActiveColumnNamesSplit)
                        {
                            for (int i = 0; i < dsBankDetail.Tables[0].Rows.Count; i++)
                            {

                                objDS.Tables[0].Rows[i]["Active"] = dsBankDetail.Tables[0].Rows[i]["Active"];
                            }
                        }

                    }
                }/*CC22 end*/




                /* #CC06 Add End */

                /* #CC03 Add End */
                foreach (DataRow dr in objDS.Tables[0].Rows)
                {
                    string Regexpression = @"^[1-9]([0-9]{9})$";
                    string RegexpressionNum = @"^[0-9]*$";

                    /* #CC11 Add Start */
                    string PhoneNumberRegex = Resources.AppConfig.PhoneNumberRegex.ToString().Replace("@", "");
                    int PhoneNumberMaxLength = Convert.ToInt16(Resources.AppConfig.RetailerPhoneNoMaxLength.ToString());
                    string PhoneNumberErrorMsg = Resources.AppConfig.RetailerPhoneNoErrorMsg.ToString();
                    int PhoneNoMinLength = Convert.ToInt16(Resources.AppConfig.RetailerPhoneNoMinLength.ToString());
                    /* #CC11 Add End */
                    /*#CC15 Add Started*/
                    int Address1MinLength = Convert.ToInt32(Resources.AppConfig.RetailerAddress1MinLength.ToString());
                    int Address1MaxLength = Convert.ToInt32(Resources.AppConfig.RetailerAddress1MaxLength.ToString());
                    int pincodelength = Convert.ToInt32(Resources.AppConfig.RetailerPinCodeMaxLength.ToString()); /*#CC16 Added*/
                    if (dr["AddressLine1"].ToString().Trim() != string.Empty || dr["AddressLine1"].ToString().Trim() != "")
                    {
                        if (Convert.ToInt32(dr["AddressLine1"].ToString().Trim().Length) < Address1MinLength)
                        {
                            dr["Error"] = "AddressLine1 Should be " + Address1MinLength + " Character.";
                            intError = 1;
                        }
                    }
                    /*#CC15 Add End*/
                    /*#CC16 Add Started*/
                    if (Convert.ToString(Session["RetailerPinCodeRequired"]) == "1")
                    {
                        if (dr["PinCode"].ToString().Trim() == string.Empty || dr["PinCode"].ToString().Trim() == "")
                        {
                            dr["Error"] = "Please Enter PinCode .";
                            intError = 1;
                        }
                        if (Convert.ToInt32(dr["PinCode"].ToString().Length) > 0 && Convert.ToInt64(dr["PinCode"].ToString().Length) < 6)
                        {
                            dr["Error"] = "Please enter 6 digit PinCode.";
                            intError = 1;
                        }
                    }
                    /*#CC16 Add End*/
                    if (Convert.ToInt64(dr["MobileNumber"].ToString().Length) > 0 && Convert.ToInt64(dr["MobileNumber"].ToString().Length) < 10)
                    {
                        if (!Regex.IsMatch(dr["MobileNumber"].ToString(), Regexpression))
                        {
                            dr["Error"] = "Please enter 10 digit Mobile number without 0 prefix.";
                            intError = 1;
                        }
                        if (!Regex.IsMatch(dr["MobileNumber"].ToString(), RegexpressionNum))
                        {
                            dr["Error"] = "Only numbers allowed in Mobile Number";
                            intError = 1;
                        }

                    }
                    if (dr["PhoneNumber"].ToString() != string.Empty || dr["PhoneNumber"].ToString() != "")
                    {
                        if (Convert.ToInt64(dr["PhoneNumber"].ToString().Length) > PhoneNumberMaxLength) /* #CC11 Maxlength set by Config */
                        {
                            dr["Error"] = "Phone Number cannot be greater than " + PhoneNumberMaxLength + " digits."; /* #CC11 Edited */
                            intError = 1;
                        }
                        if (!Regex.IsMatch(dr["PhoneNumber"].ToString(), RegexpressionNum))
                        {
                            dr["Error"] = "Only numbers allowed in Phone Number";
                            intError = 1;
                        }

                        if (Convert.ToInt64(dr["PhoneNumber"].ToString().Length) > 0 && Convert.ToInt64(dr["PhoneNumber"].ToString().Length) < PhoneNoMinLength) /* #CC11 min length set by Config */
                        {
                            /*dr["Error"] = "Please enter 10 digit Phone number without 0 prefix."; #CC11 Commented */
                            dr["Error"] = "Phone Number cannot be less than " + PhoneNoMinLength + " digits.";  /* #CC11 Added */
                            intError = 1;
                        }

                        /* #CC11 Comment Start 
                        if (Convert.ToInt64(dr["PhoneNumber"].ToString().Length) > 0 && Convert.ToInt64(dr["PhoneNumber"].ToString().Length) == 10) 
                        {
                            if (!Regex.IsMatch(dr["PhoneNumber"].ToString(), Regexpression))
                            {
                                dr["Error"] = "Please enter 10 digit Phone number without 0 prefix.";
                                intError = 1;
                            }
                        }
                         #CC11 Comment End
                         */

                    }

                    /* #CC03 Add Start */
                    if (Session["RetailerBankDetail"] != null)
                    {
                        if (Convert.ToInt32(Session["RetailerBankDetail"]) == 1)
                        {
                            if (dr["BankName"].ToString().Trim() == string.Empty && dr["AccountHolderName"].ToString().Trim() == string.Empty && dr["BankAccountNumber"].ToString().Trim() == string.Empty && dr["BranchLocation"].ToString().Trim() == string.Empty && dr["IFSCCode"].ToString().Trim() == string.Empty && dr["PANNo"].ToString().Trim() == string.Empty) /* #CC04 Validation For PANNo Added*/
                            {

                            }
                            else if (dr["BankName"].ToString().Trim() != string.Empty && dr["AccountHolderName"].ToString().Trim() != string.Empty && dr["BankAccountNumber"].ToString().Trim() != string.Empty && dr["BranchLocation"].ToString().Trim() != string.Empty && dr["IFSCCode"].ToString().Trim() != string.Empty && dr["PANNo"].ToString().Trim() != string.Empty) /* #CC04 Validation For PANNo Added*/
                            {

                            }
                            else if (dr["BankName"].ToString().Trim() != string.Empty || dr["AccountHolderName"].ToString().Trim() != string.Empty || dr["BankAccountNumber"].ToString().Trim() != string.Empty || dr["BranchLocation"].ToString().Trim() != string.Empty || dr["IFSCCode"].ToString().Trim() != string.Empty || dr["PANNo"].ToString().Trim() != string.Empty) /* #CC04 Validation For PANNo Added*/
                            {
                                dr["Error"] = "Please provide complete bank details.";
                                intError = 1;
                            }
                        }
                    }

                    /*CC20 start*/
                    if (Session["WhatsAppMobileNumber"] != null)
                    {
                        if (Convert.ToInt32(Session["WhatsAppMobileNumber"]) == 1)
                        {
                            if (dr["WhatsAppNumber"].ToString().Trim() == "")
                            {
                                dr["Error"] = "Please provide WhatsAppNumber.";
                                intError = 1;
                            }
                        }

                    }/*CC20 end*/
                    /* #CC03 Add End */
                    /* #CC06 Add Start */
                    string RegexForCounterValue = @"^[0-9]{1,10}$";
                    /* #CC07 Add Start */
                    if (Convert.ToString(Session["PotentialVolDisplay"]) == "1")
                    {
                        if (!Regex.IsMatch(dr["CounterPotentialValue"].ToString(), RegexpressionNum))
                        {
                            dr["Error"] = "Only numbers allowed in Counter Potential Value";
                            intError = 1;
                        }
                    }
                    /* #CC07 Add End */
                    if (Convert.ToString(Session["PotentialVolMandatry"]) == "1")
                    {

                        if (!Regex.IsMatch(dr["CounterPotentialValue"].ToString(), RegexpressionNum))
                        {
                            dr["Error"] = "Only numbers allowed in Counter Potential Value";
                            intError = 1;
                        }

                        if (Convert.ToInt64(dr["CounterPotentialValue"].ToString().Length) < 1)
                        {
                            dr["Error"] = "Counter potential value cannot be blank.";
                            intError = 1;
                        }
                    }
                    /* #CC06 Add End */
                    /* #CC08 Add Start */
                    if (PageBase.TehsillDisplayMode == "1")
                    {
                        if (Convert.ToInt64(dr["TehsilName"].ToString().Length) < 1)
                        {
                            dr["Error"] = "Tehsil Name cannot be blank.";
                            intError = 1;
                        }
                    }
                    /* #CC08 Add End */

                    /* #CC12 Add Start */
                    if (Resources.AppConfig.ShowHideRetailerReferanceCode.ToString() == "1")/* #CC14 Add Start */
                    {/* #CC14 Add End */
                        if (Convert.ToInt64(dr["ReferanceCode"].ToString().Trim().Length) > 20)
                        {
                            dr["Error"] = "ReferanceCode length cannot be more than 20.";
                            intError = 1;
                        }
                        if ((Convert.ToInt64(dr["ReferanceCode"].ToString().Trim().Length) > 0) && (Convert.ToInt64(dr["ReferanceCode"].ToString().Trim().Length) < 21))
                        {
                            if (!Regex.IsMatch(dr["ReferanceCode"].ToString(), @"^[a-zA-Z0-9]+$"))
                            {
                                dr["Error"] = "Enter only alphanueric values.";
                                intError = 1;
                            }
                        }
                    } /* #CC14 Added */
                    /* #CC12 Add End*/

                    //#CC22
                    if (ddlUploadType.SelectedValue == "2")
                    {
                        if (string.IsNullOrEmpty(dr["Active"].ToString()))
                        {
                            dr["Error"] = "Column Active cannot be left blank.";
                            intError = 1;
                        }

                        if (!string.IsNullOrEmpty(dr["Active"].ToString()) && dr["Active"].ToString().Trim().ToLower() != "yes")
                        {
                            if (dr["Active"].ToString().Trim().ToLower() != "no")
                            {
                                dr["Error"] = "Only Yes or No allowed in Active.";
                                intError = 1;
                            }
                        }
                    }



                }
                if (intError == 1)
                {
                    isSuccess = 2;
                }
                if (intError == 0)
                {
                    objDS.Tables[0].Columns.Remove("Error");
                   // Btnsave.Visible = true;
                }
                /* #CC02 Add End */
                switch (isSuccess)
                {
                    case 0:
                        ucmassege1.ShowInfo(UploadFile.Message);
                     //  dvhide.Visible = false;

                        break;
                    case 2:
                        //*CC22 Start
                         hlnkInvalid.Visible = true;
                         strUploadedFileName = "InvalidData" + DateTime.Now.Ticks;
                         ucmassege1.ShowInfo(Resources.Messages.PartialDataUpload);
                        DataView dvError = objDS.Tables[0].DefaultView;
                        dvError.RowFilter = "Error<>''";
                        DataTable dtError = dvError.ToTable();
                        DataSet dsError = new DataSet();
                        dsError.Tables.Add(dtError);
                        PageBase.ExportInExcel(dsError, strUploadedFileName);
                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strUploadedFileName + ".xlsx";
                        hlnkInvalid.Text = "Invalid Data";
                        
                        //ucmassege1.ShowInfo(Resources.Messages.CheckErrorGrid);
                        ////pnlGrid.Visible = true;
                        //GridRetailer.DataSource = objDS;
                        //GridRetailer.DataBind();
                        //dvhide.Visible = true;

                        //*CC22 END
                        break;
                    case 1:
                        SaveData(objDS.Tables[0]);
                        //CC22
                        //InsertData(objDS);
                        break;
                }

            }
            else if (UploadCheck == 2)
            {
                ucmassege1.ShowInfo(Resources.Messages.UploadXlxs);
               // dvhide.Visible = false;
            }
            else if (UploadCheck == 3)
            {
                ucmassege1.ShowInfo(Resources.Messages.SelectFile);
               // dvhide.Visible = false;

            }
            //else
            //{
            //    ucmassege1.ShowError (Resources.Messages.ErrorMsgTryAfterSometime);
            //    dvhide.Visible = false;

            //}
        }
        catch (Exception ex)
        {

            ucmassege1.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);

        }
       
    }

      //#CC22 Start
    private void SaveData(DataTable dtSave)
    {
        try
        {
            DataSet Ds = new DataSet();
            if (IsPageRefereshed == true)
            {
                return;
            }
            Ds = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + ViewState["TobeUploaded"].ToString());
            if (!Ds.Tables[0].Columns.Contains("RetailerUserName"))
                Ds.Tables[0].Columns.Add("RetailerUserName");
            if (!Ds.Tables[0].Columns.Contains("MappedOrgnhierarchy"))
                Ds.Tables[0].Columns.Add("MappedOrgnhierarchy");
            if (!Ds.Tables[0].Columns.Contains("RetailerCode"))
                Ds.Tables[0].Columns.Add("RetailerCode");
            if (!Ds.Tables[0].Columns.Contains("StockOpeningDate"))/*#CC01 ADDED*/
                Ds.Tables[0].Columns.Add("StockOpeningDate");  /*#CC01 ADDED*/
            Ds.AcceptChanges();
            //if (ViewState["Retailer"] != null)
            //{


            //Ds = (DataSet)ViewState["Retailer"];

            /* #CC10 Add Start */
            if (PageBase.ChangeTinLabel == 1)
            {
                /* #CC14 Comment Start
                 if (Ds.Tables[0].Columns.Contains("VATNumber"))
               {
                   Ds.Tables[0].Columns["VATNumber"].ColumnName = "TinNumber";
               } 
                 #CC14 Comment End */
                /* #CC14 Add Start */
                if (Ds.Tables[0].Columns.Contains("GSTNumber"))
                {
                    Ds.Tables[0].Columns["GSTNumber"].ColumnName = "TinNumber";
                }
                /* #CC14 Add End */
            }
            /* #CC10 Add End */

            /*#CC13 START ADDED*/
            if (Ds.Tables[0].Columns.Contains("AddressLine1"))
                Ds.Tables[0].Columns["AddressLine1"].ColumnName = "Address1";
            if (Ds.Tables[0].Columns.Contains("AddressLine2"))
                Ds.Tables[0].Columns["AddressLine2"].ColumnName = "Address2";
            /*#CC13 END ADDED*/
            /* #CC14 Add Start */
            if (Ds.Tables[0].Columns.Contains("ISDCounter"))
            {
                Ds.Tables[0].Columns["ISDCounter"].ColumnName = "ISPCounter";
            }
            /* #CC14 Add Start */
            /* #CC18 Add Start */
            if (Ds.Tables[0].Columns.Contains("ISDCounter"))
            {
                Ds.Tables[0].Columns[Resources.GlobalChangeLabel.CSAName.Replace(" ", "").Replace("onCounter", "Counter")].ColumnName = "ISPCounter";
            }
            if (!Ds.Tables[0].Columns.Contains("SalesChannelCode"))
            {
                Ds.Tables[0].Columns.Add("SalesChannelCode", typeof(string));
                foreach (DataRow dr in Ds.Tables[0].Rows)
                {
                    dr["SalesChannelCode"] = PageBase.SalesChanelCode;
                }
                Ds.Tables[0].AcceptChanges();
            }


            /* #CC18 Add End */
            if (Ds.Tables[0].Columns.Contains("SalesChannelCode")) /* #CC18 Added */
                if (SalesChanelID != 0)
                {
                    var query = from r in Ds.Tables[0].AsEnumerable()
                                where ((Convert.ToString(r["SalesChannelCode"]) != PageBase.SalesChanelCode))
                                select new
                                {
                                    SalesChannelCode = Convert.ToString(r["SalesChannelCode"])
                                };
                    if (query != null)
                    {
                        if (query.Count() > 0)
                        {
                            ucmassege1.ShowInfo(Resources.Messages.SalesChannelNotMatched);
                            return;
                        }
                    }
                    //Commented becuase when code was like 71111001 then it was treated this as the double value and
                    // comparision like <> does not work on the string and the double value so it throws an exception

                    //if (Ds.Tables[0].Select("SalesChannelCode <> '" + PageBase.SalesChanelCode + "'").Length > 0)
                    //{
                    //    ucmassege1.ShowInfo(Resources.Messages.SalesChannelNotMatched);
                    //    return;

                    //}
                }

            using (Authenticates ObjAuth = new Authenticates())
            {
                strPSalt = ObjAuth.GenerateSalt(PageBase.strDefaultPassword.Length);
                strPassword = ObjAuth.EncryptPassword(PageBase.strDefaultPassword, strPSalt);

            };
            using (RetailerData ObjRetailer = new RetailerData())
            {
                DataSet dsResult = new DataSet();
                string XmlDetail = "";
                XmlDetail = Ds.GetXml();
                ObjRetailer.RetailerDetailXML = XmlDetail;
                ObjRetailer.SalesChannelID = PageBase.SalesChanelID;
                ObjRetailer.Password = strPassword;
                ObjRetailer.PasswordSalt = strPSalt;
                ObjRetailer.UserID = PageBase.UserId; /* #CC03 Added */
                //#CC22 -- Update sp called 
                if (ddlUploadType.SelectedValue == "1")
                {
                   dsResult =  ObjRetailer.UploadRetailer("prcInsUpdRetailerUpload");
                }
                else if (ddlUploadType.SelectedValue == "2")
                {
                   dsResult= ObjRetailer.UploadRetailer("prcBulkUpdateRetailerExcel");

                }
                if (ObjRetailer.RetailerDetailXML != null && ObjRetailer.RetailerDetailXML != string.Empty)
                {
                    dsResult.ReadXml(new XmlTextReader(new StringReader(ObjRetailer.RetailerDetailXML)));
                    CreateInvalidDataLink(dsResult);
                    return;
                }
                else if (ObjRetailer.Error != null && ObjRetailer.Error != "")
                {
                    CreateInvalidDataLink(dsResult);
                    return;
                }
                if (ddlUploadType.SelectedValue == "2")
                {
                    ucmassege1.ShowSuccess(Resources.Messages.EditSuccessfull);
                }
                else
                {
                    ucmassege1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                }
                ClearForm();


            };
            // }
        }
        catch (Exception ex)
        {
            ucmassege1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);


        }
    }
    //#CC22 Start
    private void CreateInvalidDataLink(DataSet dsResult)
    {
        hlnkInvalid.Visible = true;
        strUploadedFileName = "InvalidData" + DateTime.Now.Ticks;
        ucmassege1.ShowInfo(Resources.Messages.PartialDataUpload);
        PageBase.ExportInExcel(dsResult, strUploadedFileName);
        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strUploadedFileName + ".xlsx";
        hlnkInvalid.Text = "Invalid Data";

    }

    //#CC22
    //protected void Btnsave_Click(object sender, EventArgs e)
    //{
       
    //}
    void ClearForm()
    {
        rdoSelectMode.SelectedValue = "1";
       // pnlGrid.Visible = false;
        //Btnsave.Visible = false;
        //updGrid.Update();

        //#CC22 Start
        hlnkInvalid.Text = "";
        //#CC22 End
        //if (ViewState["Retailer"] != null)
        //    ViewState["Retailer"] = null;
        //dvhide.Visible = false;
        Session["RetailerUploadData"] = null;
        Session["UPDRTLR"] = null;
    }

    //#CC22 Start
    //protected void BtnCancel_Click(object sender, EventArgs e)
    //{
    //    ucmassege1.ShowControl = false;
    //    ClearForm();


    //}
  
    //private void InsertData(DataSet objds)
    //{
    //    if (objds != null && objds.Tables.Count > 0 && objds.Tables[0].Rows.Count > 0)
    //    {
    //        {

    //            pnlGrid.Visible = true;
    //            Btnsave.Visible = true;
    //            if (Session["RetailerUploadData"] != null)
    //            {
    //                GridRetailer.DataSource = (DataSet)Session["RetailerUploadData"];
    //                //ViewState["Retailer"] = objds;
    //                GridRetailer.DataBind();
    //                updGrid.Update();
    //                dvhide.Visible = true;
    //            }


    //        }

    //    }

    //}
//#CC22 End
    protected void DwnldReferenceCodeTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsReferenceCode = new DataSet();
            using (SalesChannelData objSalesData = new SalesChannelData())
            {

                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.eRetailer;

                objSalesData.SalesChanneLevel = EnumData.eSalesChannelLevel.TdLevel;
                if (PageBase.SalesChanelID != 0 || PageBase.SalesChanelID != null)
                {
                    objSalesData.SalesChannelID = PageBase.SalesChanelID;
                }
                objSalesData.UserID = PageBase.UserId; /*#CC21*/
                dsReferenceCode = objSalesData.GetAllTemplateData();
                if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dsReferenceCode, FilenameToexport, EnumData.eTemplateCount.eRetailer + 2);
                }
                else
                {
                    ucmassege1.ShowInfo(Resources.Messages.NoRecord);
                }
            }
        }
        catch (Exception ex)
        {
            ucmassege1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void lnkRetailerTemplateWV_Click(object sender, EventArgs e)
    {
        try
        {//#CC22
            if (ddlUploadType.SelectedValue == "0")
            {
                ucmassege1.ShowInfo("Please select Upload Type!");
                return;
            }

            /*#CC17 Added Started*/
           /* string strTemplatePath = Server.MapPath(PageBase.strExcelTemplatePathSB);
            string strURL = strTemplatePath + "RetailerMaster.xlsx";
            string filename = "Retailer Template";
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            response.AddHeader("content-disposition", "attachment; filename=" + filename + "*.xlsx");
            byte[] data = req.DownloadData(strURL);
            response.BinaryWrite(data);
            response.End();
            */
            /*#CC17 Added Started*/

            /*#CC17 Commented Started*/
            DataSet ds = new DataSet();
            DataTable dt = ReadRetailerTemplateExcelFile();
            ds.Merge(dt);
            String FilePath = Server.MapPath("../../");
            string FilenameToexport = "Retailer Template" + ddlUploadType.SelectedItem.Text; //#CC22
            PageBase.RootFilePath = FilePath;
            PageBase.ExportToExecl(ds, FilenameToexport);
            /*#CC17 Commented End*/
        }
        catch (Exception ex)
        {
            ucmassege1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());

        }

    }
    private DataTable ReadRetailerTemplateExcelFile()
    {
        DataSet dsTemplate = new DataSet();
        string strTemplatePath = Server.MapPath(PageBase.strExcelTemplatePathSB);
        DirectoryInfo dirXLS = new DirectoryInfo(strTemplatePath);
        FileInfo[] drFilesXLS = dirXLS.GetFiles("*.xlsx");
        foreach (FileInfo fi in drFilesXLS)
        {
            if (fi.Name.ToLower().Contains("retailermaster") == true)
                dsTemplate = objexcel.ImportExcelFile(strTemplatePath + "RetailerMaster.xlsx");
        }
        /* #CC08 Add Start */
        if (PageBase.TehsillDisplayMode == "1")
        {
            dsTemplate.Tables[0].Columns.Add("TehsilName", typeof(string)).SetOrdinal(dsTemplate.Tables[0].Columns.Count - 12); ; /* #CC09 Tehsil setordinal changed.*/
        }
        /* #CC08 Add End */
        /* #CC18 Add Start 
         if(dsTemplate.Tables[0].Columns.Contains("SalesmanName"))
         {
             dsTemplate.Tables[0].Columns.Remove("SalesmanName");
         }
        
         if (dsTemplate.Tables[0].Columns.Contains("ParentRetailerCode"))
         {
             dsTemplate.Tables[0].Columns.Remove("ParentRetailerCode");
         }*/

        /* #CC18 Add End */

        if (Session["RetailerLogin"] != null)
            if (Convert.ToInt32(Session["RETAILERLOGIN"]) == 1)
                dsTemplate.Tables[0].Columns.Add("RetailerUserName");
        if (Session["RETAILERHIERLVLID"] != null)
            if (Convert.ToInt32(Session["RETAILERHIERLVLID"]) > 0 &&
                Convert.ToString(Session["RETHERARCHYPARENTMANDATORY"]) == "1")  /* #CC18 "RETHERARCHYPARENTMANDATORY" Added  */
                dsTemplate.Tables[0].Columns.Add("MappedOrgnhierarchy");


        if ((Session["RETAILERCODEAUTO"] != null))
            if (Convert.ToInt32(Session["RETAILERCODEAUTO"]) == 0 && ddlUploadType.SelectedItem.Value == "1") //#CC22 in case of add 
                dsTemplate.Tables[0].Columns.Add("RetailerCode");
        //#CC22
        if (ddlUploadType.SelectedItem.Value == "2")
        {
            dsTemplate.Tables[0].Columns.Add("RetailerCode");
            dsTemplate.Tables[0].Columns.Add("Active");
            dsTemplate.Tables[0].Columns.Remove("SalesChannelCode");
            dsTemplate.Tables[0].Columns.Remove("SalesmanName");
            dsTemplate.Tables[0].Columns.Remove("MappedOrgnhierarchy");
        }
    

        /*#CC01 START ADDED*/
        if (Session["RetailerOpStockDate"] != null)
            if (Session["RetailerOpStockDate"].ToString() == "0")
                dsTemplate.Tables[0].Columns.Add("StockOpeningDate", typeof(string));
        /*#CC01 START END*/




        /* #CC03 ADD START */
        if (Session["RetailerBankDetail"] != null)
        {
            if (Convert.ToInt32(Session["RetailerBankDetail"]) == 1)
            {
                dsTemplate.Tables[0].Columns.Add("BankName", typeof(string));
                dsTemplate.Tables[0].Columns.Add("AccountHolderName", typeof(string));
                dsTemplate.Tables[0].Columns.Add("BankAccountNumber", typeof(string));
                dsTemplate.Tables[0].Columns.Add("BranchLocation", typeof(string));
                dsTemplate.Tables[0].Columns.Add("IFSCCode", typeof(string));
                dsTemplate.Tables[0].Columns.Add("PANNo", typeof(string));  /* #CC04 Added */
            }
        }
        /* #CC03 ADD End */


        /*#CC20 start*/
        if (Session["WhatsAppMobileNumber"] != null)
        {
            if (Convert.ToInt32(Session["WhatsAppMobileNumber"]) == 1)
            {
                dsTemplate.Tables[0].Columns.Add("WhatsAppNumber", typeof(string));
              
            }
        }/*#CC20 end*/
        /* #CC06 Add Start */
        if (Session["PotentialVolDisplay"] != null)
        {
            if (Convert.ToInt32(Session["PotentialVolDisplay"]) == 1)
            {
                dsTemplate.Tables[0].Columns.Add("CounterPotentialValue", typeof(string));
            }
        }
        /* #CC06 Add End */
        /* #CC10 Add Start */
        if (PageBase.ChangeTinLabel == 1)
        {
            dsTemplate.Tables[0].Columns["TinNumber"].ColumnName = /* "VATNumber" #CC14 Commented */ "GSTNumber" /* #CC14 Added */;
        }
        /* #CC10 Add End */


        if (Resources.AppConfig.ShowHideRetailerReferanceCode.ToString() == "1")/* #CC14 Added*/
            dsTemplate.Tables[0].Columns.Add("ReferanceCode", typeof(string)); /* #CC12 Add Start */

        /* #CC18 Add Start */
        if (dsTemplate.Tables[0].Columns.Contains("ISDCounter"))
        {   
            dsTemplate.Tables[0].Columns["ISDCounter"].ColumnName=Resources.GlobalChangeLabel.CSAName.Replace(" ","").Replace("onCounter","Counter");
        }
        /* #CC18 Add End */

        dsTemplate.Tables[0].AcceptChanges();
        return dsTemplate.Tables[0];

    }

  
}
