#region Copyright(c) 2012 Zed-Axis Technologies All rights are reserved
/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2010 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Created By    : 
* Module        : 
* Description   : 
* Table Name    : 
* ====================================================================================================
* Reviewed By :
 ====================================================================================================
* Change Log :
 * 16-Jun-2015, Vijay Katiyar, #CC01 - Added to pick Customer Code From Global resources file.
 * 19-DEC-2018, Rakesh Raj,#CC02 - Imported From ZEDEBS 
 ====================================================================================================
 */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using LuminousSMS;#CC02 
//using ZedAxis.ZedEBS.Enums;#CC02 
//using ZedEBS.Data;#CC02 
//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Data;
//using ZedEBS.ZedService;#CC02 
//using ZedEBS.Admin;
using DataAccess;
using BussinessLogic;


    public partial class UserControls_ucUserDetail : System.Web.UI.UserControl
    {
        #region Delegate & Events

        public delegate void delegCityId(int cityid);
        public event delegCityId Citychanged;

        #endregion

        #region Private Class Variables

        private int _intDetailId = 0;
        private Int64 _intJobsheetId = 0;
        private string _strCustomerCode = string.Empty;
        private EnumData.EnumUserDetailType _enumUserDetailType = EnumData.EnumUserDetailType.NONE;
        private string _strValidationGroup = string.Empty;
        private bool _blPartialEditable = false;

        #endregion

        #region Public Properties

        public int DetailId
        {
            set
            {
                _intDetailId = value;
                ViewState["DetailId"] = _intDetailId;
            }
            get
            {
                if (ViewState["DetailId"] != null)
                    return Convert.ToInt32(ViewState["DetailId"]);

                return 0;
            }
        }
        public Int64 JobsheetID
        {
            set
            {
                _intJobsheetId = value;
                ViewState["JobsheetId"] = _intJobsheetId;
            }
            get
            {
                if (ViewState["JobsheetId"] != null)
                    return Convert.ToInt32(ViewState["JobsheetId"]);

                return 0;
            }
        }
        public string CustomerCode
        {
            set { _strCustomerCode = value; }
        }
        public EnumData.EnumUserDetailType UserDetailType
        {
            get
            {
                return _enumUserDetailType;
            }
            set
            {
                _enumUserDetailType = value;
            }
        }
        public string ValidationGroup
        {
            set { _strValidationGroup = value; }
        }
        public bool Enabled
        {
            set
            {
                ToggleControlsAccessbility(value);
            }
        }
        public bool PartialEditable
        {
            set
            {
                _blPartialEditable = value;
            }
        }

        #endregion

        #region Page Control Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["_ucUserDetail"] == null)
                {
                    BindStatesList();
                    BindPageControls();

                    //rfvCompanyName.ValidationGroup = _strValidationGroup;
                    //rfvCity.ValidationGroup = _strValidationGroup;
                    ////rfvEmailID.ValidationGroup = _strValidationGroup;
                    //rfvFirstName.ValidationGroup = _strValidationGroup;
                    //rfvLastName.ValidationGroup = _strValidationGroup;
                    //rfvAddress.ValidationGroup = _strValidationGroup;
                    //rfvOtherCity.ValidationGroup = _strValidationGroup;
                    ////rfvLocality.ValidationGroup = _strValidationGroup;

                    rfvAddress.ValidationGroup = _strValidationGroup;
                    rfvCity.ValidationGroup = _strValidationGroup;
                    rfvCompanyName.ValidationGroup = _strValidationGroup;
                    rfvFirstName.ValidationGroup = _strValidationGroup;
                    rfvLastName.ValidationGroup = _strValidationGroup;
                    rfvOtherCity.ValidationGroup = _strValidationGroup;

                    regexAddress.ValidationGroup = _strValidationGroup;
                    regexAddress2.ValidationGroup = _strValidationGroup;
                    regexCompanyName.ValidationGroup = _strValidationGroup;
                    regexEmail.ValidationGroup = _strValidationGroup;
                    regexFirstName.ValidationGroup = _strValidationGroup;
                    regexLandMark.ValidationGroup = _strValidationGroup;
                    regexLastName.ValidationGroup = _strValidationGroup;
                    regexMiddleName.ValidationGroup = _strValidationGroup;
                    regexPincode.ValidationGroup = _strValidationGroup;
                    regexpOtherCity.ValidationGroup = _strValidationGroup;
                    regexRegion.ValidationGroup = _strValidationGroup;


                    if (UserDetailType == EnumData.EnumUserDetailType.DEALER || UserDetailType == EnumData.EnumUserDetailType.ENTITY)
                    {
                        lblCompanyManadatory.Visible = true;
                        rfvCompanyName.Enabled = true;
                        rfvCompanyName.ValidationGroup = _strValidationGroup;
                    }

                    if (ucAltMobile.IsRequired)
                        ucAltMobile.ValidationGroup = _strValidationGroup;
                    //if (ucDateOfBirth.IsRequired)
                   //rakesh
                    //if (ucMarraigeAnniversary.IsRequired)
                    //    ucMarraigeAnniversary.ValidationGroup = _strValidationGroup;
                    if (ucMobileNo.IsRequired)
                        ucMobileNo.ValidationGroup = _strValidationGroup;
                    if (UCPhoneAlt.IsRequired)
                        UCPhoneAlt.ValidationGroup = _strValidationGroup;
                    if (ucPhoneNo.IsRequired)
                        ucPhoneNo.ValidationGroup = _strValidationGroup;

                    if (_blPartialEditable)
                    {
                        txtFirstName.Enabled = false;
                        txtMiddleName.Enabled = false;
                        txtLastName.Enabled = false;
                        //ucDateOfBirth.Enabled = false;
                    }
                    ViewState["_ucUserDetail"] = "Loaded";

                    //if (!IsPostBack)
                    //    dvCustomerCode.Visible = false;
                }
                ucDateOfBirth.MinRangeValue = DateTime.Today.AddYears(-120).Date;
                ucDateOfBirth.MaxRangeValue = DateTime.Today;
                ucDateOfBirth.RangeErrorMessage = "Invalid DOB";
                ucDateOfBirth.ValidationGroup = _strValidationGroup;
                //  ucMarraigeAnniversary.MinRangeValue= 
            }
            catch (Exception ex)
            {
                PageBase.Errorhandling(ex);/*#CC02*/
            }
        }
        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int16 intStateId;
            Int16.TryParse(ddlState.SelectedValue, out intStateId);
            BindCityList(intStateId);
            hdnfldLocalityID.Value = "0";
        }
        protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Citychanged != null)
            {
                Citychanged(Convert.ToInt32(ddlCity.SelectedValue));
            }

            DataTable dtcity = new DataTable();
            dtcity = (DataTable)ViewState["Cityinfo"];
            DataRow dr = dtcity.Select("CityID=" + ddlCity.SelectedValue).SingleOrDefault();
            if (dr == null)
                lblStdCode.Text = "";
            else
                lblStdCode.Text = dr["STDCode"].ToString();
            AutoCompleteExtenderModel.ContextKey = ddlCity.SelectedValue;
            txtRegion.Text = string.Empty; // Locality
            hdnfldLocalityID.Value = "0";
            dvOtherCity.Visible = (ddlCity.SelectedValue == "-2");
            //Session["CityId"] = ddlCity.SelectedValue;
            //updpnlSTDCode_Locality.Update();
        }
        protected void ddlCity_DataBound(object sender, EventArgs e)
        {
            ddlCity.Items.Insert(0, new ListItem("Select City", "0"));
            if (UserDetailType == EnumData.EnumUserDetailType.CUSTOMER)
                ddlCity.Items.Add(new ListItem("OTHER", "-2"));
        }
        protected void txtRegion_TextChanged(object sender, EventArgs e)
        {
            GetLocalityID();
        }

        protected void ucMA_TextBoxDate_TextChanged(string date)
        {
            try
            {
                Compare_DOB_and_MAdate(false);

            }
            catch (Exception ex)
            {

               PageBase.Errorhandling(ex);/*#CC02*/
            }
        }
        protected void ucDOB_TextBoxDate_TextChanged(string date)
        {
            try
            {
                Compare_DOB_and_MAdate(true);

            }
            catch (Exception ex)
            {

                PageBase.Errorhandling(ex);/*#CC02*/
            }
        }
        private void Compare_DOB_and_MAdate(bool isDOB)
        {
            if (ucDateOfBirth.Date != "" && ucMarraigeAnniversary.Date != "")
            {
                if (Convert.ToDateTime(ucDateOfBirth.Date) < Convert.ToDateTime(ucMarraigeAnniversary.Date))
                {

                    lblmsgAniversary.Text = "";
                }
                else
                {
                    if (isDOB)
                    {
                        ucDateOfBirth.Date = "";

                    }
                    else
                    {
                        ucMarraigeAnniversary.Date = "";
                    }
                    lblmsgAniversary.Text = "Should be greater than DOB";
                }

            }
            else
            {
                lblmsgAniversary.Text = "";
            }
        }


        #endregion

        #region Public Methods

        public string GetContextFld()
        {
            return Convert.ToString(ddlCity.SelectedValue);
        }

        public void GetUserDetail(clsDetailMaster detail)
        {
            try
            {
                //detail.UserDetailID
                int.TryParse(Convert.ToString(ViewState["DetailId"]), out _intDetailId);
                detail.DetailId = _intDetailId;
                detail.LandMark = txtArea.Text;
                detail.CompanyName = txtCompanyName.Text.Trim();
                detail.Address = txtAddress.Text.Trim();
                detail.FirstName = txtFirstName.Text.Trim();
                detail.MiddleName = txtMiddleName.Text.Trim();
                detail.LastName = txtLastName.Text.Trim();
                detail.StateId = Convert.ToInt16(ddlState.SelectedValue);
                detail.StateName = Convert.ToString(ddlState.SelectedItem.Text);
                detail.CityId = Convert.ToInt32(ddlCity.SelectedValue);
                if (detail.CityId > 0)
                    detail.CityName = Convert.ToString(ddlCity.SelectedItem.Text);
                else
                    detail.CityName = txtOtherCity.Text;

                detail.EmailID = txtEmailID.Text.Trim();
                GetLocalityID();
                if (!string.IsNullOrEmpty(hdnfldLocalityID.Value) && Convert.ToInt32(hdnfldLocalityID.Value) > 0)
                    detail.LocalityID = Convert.ToInt32(hdnfldLocalityID.Value.Trim());

                detail.LocalityName = txtRegion.Text.Trim();
                detail.AltMobileNo = ucAltMobile.Text.Trim();
                detail.AltPhoneNo = UCPhoneAlt.Text.Trim();
                detail.MobileNo = ucMobileNo.Text.Trim();
                detail.PhoneNo = ucPhoneNo.Text.Trim();
                detail.Pincode = txtPinCode.Text.Trim();
                detail.Street = txtStreet.Text.Trim();
                detail.DOB = DateTime.Now; //raj ucDateOfBirth.GetDate;

                if (ucDateOfBirth.Date.Trim().Length > 0)
                    detail.Age = (Int16)(DateTime.Now.Subtract(Convert.ToDateTime(ucDateOfBirth.Date)).Days / 365);
                if (ucMarraigeAnniversary.Date.Trim().Length > 0)
                {
                    detail.MarriageAnniversary = Convert.ToDateTime(ucMarraigeAnniversary.Date);
                }
                else
                {
                    detail.MarriageAnniversary = null;
                }
            }
            catch (Exception ex)
            {
               PageBase.Errorhandling(ex);/*#CC02*/
            }
        }
        public void GetUserDetail(clsUserDetail detail)
        {
            clsDetailMaster detailMaster = detail;
            GetUserDetail(detailMaster);
        }
        public void GetUserDetail(clsCustomerDetail detail)
        {
            clsDetailMaster detailMaster = detail;
            GetUserDetail(detailMaster);
            detail.CustomerType = (Int16)EnumData.EnumCustomerType.CUSTOMER;
        }
        public void GetUserDetail(clsJobSheetCustomerDetail detail)
        {
            clsDetailMaster detailMaster = detail;
            GetUserDetail(detailMaster);
        }

        public void BindPageControls()
        {
            try
            {
                if (UserDetailType == EnumData.EnumUserDetailType.NONE || DetailId == 0)
                {
                    lblCodeHeader.Text = string.Empty;
                    dvCustomerCode.Visible = false;
                    return;
                }
                //if (DetailId == 0) return; //Do not interchange location of this code

                clsDetailMaster detail = null;

                if (UserDetailType == EnumData.EnumUserDetailType.APPLICATIONUSER || UserDetailType == EnumData.EnumUserDetailType.ENTITY)
                {
                    detail = new clsUserDetail();
                    if (UserDetailType == EnumData.EnumUserDetailType.ENTITY)
                    {
                        lblCodeHeader.Text = "Enity Code:";
                        dvCustomerCode.Visible = true;
                    }
                    else
                    {
                        lblCodeHeader.Text = string.Empty;
                        dvCustomerCode.Visible = false;
                    }
                }
                else
                {
                    if (JobsheetID == 0)
                        detail = new clsCustomerDetail();
                    else
                        detail = new clsJobSheetCustomerDetail();

                    //trCustomerCode.Visible = true;
                    //lblCodeHeader.Text = "Customer Code:";//#CC01 Commented
                    lblCodeHeader.Text = "Resources.ApplicationKeyword.CustomerCode" + ":";//#CC01 Added
                    lblCustomerCode.Text = _strCustomerCode;
                    dvCustomerCode.Visible = true;
                }

                if (detail != null)
                {
                    using (detail)
                    {
                        detail.JobSheetID = JobsheetID;
                        detail.DetailId = DetailId;
                        detail.Load();
                        txtCompanyName.Text = detail.CompanyName;
                        txtArea.Text = detail.LandMark;
                        txtAddress.Text = detail.Address;
                        txtEmailID.Text = detail.EmailID;
                        txtFirstName.Text = detail.FirstName;
                        txtLastName.Text = detail.LastName;
                        txtMiddleName.Text = detail.MiddleName;
                        ucMobileNo.Text = detail.MobileNo;
                        ucAltMobile.Text = detail.AltMobileNo;
                        ucPhoneNo.Text = detail.PhoneNo;
                        UCPhoneAlt.Text = detail.AltPhoneNo;
                        txtPinCode.Text = detail.Pincode.Trim();
                        txtStreet.Text = detail.Street;

                        if (UserDetailType == EnumData.EnumUserDetailType.ENTITY)
                            lblCustomerCode.Text = detail.EntityCode;

                        ucDateOfBirth.Date = Convert.ToString(detail.DOB);

                        ucDateOfBirth.Date = string.Format("{0:dd/MM/yyyy}", detail.DOB);
                        ucMarraigeAnniversary.Date = string.Format("{0:dd/MM/yyyy}", detail.MarriageAnniversary);
                        ddlState.SelectedValue = Convert.ToString(detail.StateId);
                        ddlCity.DataBound += delegate(object sender1, EventArgs e1)
                        {
                            if (detail.CityId > 0)
                            {
                                ddlCity.SelectedValue = Convert.ToString(detail.CityId);
                            }
                            else if (!string.IsNullOrEmpty(detail.CityName))
                            {
                                ddlCity.SelectedValue = "-2";//value for other city
                                txtOtherCity.Text = detail.CityName;
                                if (Citychanged != null)
                                    Citychanged(-2);
                            }
                            ddlCity_SelectedIndexChanged(ddlCity, null);
                            txtRegion.Text = detail.LocalityName;
                        };
                        if (!string.IsNullOrEmpty(ddlState.SelectedValue))
                            BindCityList(Convert.ToInt16(ddlState.SelectedValue));
                    }
                }
            }
            catch (Exception ex)
            {
             //   ExceptionPolicy.HandleException(ex, Pagebase.ExceptionPolicyName);
            }
        }
        public void ClearPageControls()
        {
            txtCompanyName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtEmailID.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtMiddleName.Text = string.Empty;
            txtRegion.Text = string.Empty;
            ucMobileNo.Text = string.Empty;
            ucAltMobile.Text = string.Empty;
            ucPhoneNo.Text = string.Empty;
            UCPhoneAlt.Text = string.Empty;
            txtPinCode.Text = string.Empty;
            txtStreet.Text = string.Empty;
            txtArea.Text = string.Empty;
            ucDateOfBirth.Date = string.Empty;
            hdnfldLocalityID.Value = "0";
            lblmsgAniversary.Text = string.Empty;

            #region COMMENTED: Not sure why applied here
            //clsDetailMaster detail = null;
            //if (UserDetailType == EnumData.EnumUserDetailType.APPLICATIONUSER)
            //{
            //    detail = new clsUserDetail();
            //    dvCustomerCode.Visible = false;
            //}
            //else
            //{
            //    detail = new clsCustomerDetail();
            //    dvCustomerCode.Visible = true;
            //    lblCustomerCode.Text = _strCustomerCode;
            //}
            //if (detail != null)
            //{
            //    using (detail)
            //    {
            //        ddlState.SelectedValue = Convert.ToString(detail.StateId);
            //        ddlCity.DataBound += delegate(object sender1, EventArgs e1)
            //        {
            //            ddlCity.SelectedValue = Convert.ToString(detail.CityId);
            //        };
            //        BindCityList(Convert.ToInt16(ddlState.SelectedValue));
            //    }
            //} 
            #endregion
            ViewState["Cityinfo"] = null;
            ddlState.SelectedIndex = -1;
            ddlState_SelectedIndexChanged(ddlState, null);
            lblStdCode.Text = string.Empty;

            //ucDateOfBirth.Date = string.Empty;
            ucMarraigeAnniversary.Date = string.Empty;
            lblCustomerCode.Text = string.Empty;
            dvCustomerCode.Visible = false;
        }

        #endregion

        #region Private Methods

        private void BindStatesList()
        {
            try
            {
                //ddlState.Items.Clear();
                using (MastersData objState = new MastersData())
                {
                    //objState.PageIndex = 1;
                    //objState.PageSize = 500;
                    ddlState.DataSource = objState.SelectStateInfo();
                    ddlState.DataTextField = "StateName";
                    ddlState.DataValueField = "StateID";
                    ddlState.DataBind();
                }
                ddlState.Items.Insert(0, new ListItem("Select State", "0"));
                ViewState["Cityinfo"] = null;
            }
            catch (Exception ex)
            {
               // ExceptionPolicy.HandleException(ex, Pagebase.ExceptionPolicyName);
            }
        }
        private void BindCityList(Int16 intStateId)
        {
            try
            {
                //ddlCity.Items.Clear();
                ViewState["Cityinfo"] = null;
                using (MastersData objCity = new MastersData())
                {
             //       objCity.PageIndex = 1;
               //     objCity.PageSize = 500;
                    objCity.Active= 1;
                    DataTable dt = new DataTable();
                    objCity.State_Id= intStateId;
                    dt = objCity.SelectCityInfo();
                    ddlCity.DataSource = dt;
                    ViewState["Cityinfo"] = dt;
                    ddlCity.DataTextField = "CityName";
                    ddlCity.DataValueField = "CityID";
                    ddlCity.DataBind();
                }
            }
            catch (Exception ex)
            {
             //   ExceptionPolicy.HandleException(ex, Pagebase.ExceptionPolicyName);
            }
        }
        private void ToggleControlsAccessbility(bool value)
        {
            try
            {
                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl.GetType() == typeof(TextBox))
                    {
                        ((TextBox)ctrl).Enabled = value;
                    }
                    else if (ctrl.GetType() == typeof(DropDownList))
                    {
                        ((DropDownList)ctrl).Enabled = value;
                    }
                    else if (ctrl.GetType() == typeof(UserControls_ucContactNo))
                    {
                        ((UserControls_ucContactNo)ctrl).Enabled = value;
                    }
                    //else if (ctrl.GetType() == typeof(ZedEBS.Controls.ucDatePicker))
                    //{
                    //    ((ucDatePicker)ctrl).Enabled = value;
                    //}
                    else if (ctrl.GetType() == typeof(UpdatePanel))
                    {
                        DropDownList ddlCity = (DropDownList)((UpdatePanel)ctrl).FindControl("ddlCity");
                        ddlCity.Enabled = value;
                    }
                }
                txtRegion.Enabled = value;
            }
            catch (Exception ex)
            {
             //   ExceptionPolicy.HandleException(ex, Pagebase.ExceptionPolicyName);
            }
        }
        private void GetLocalityID()
        {
            try
            {
                using (MastersData objLocalityMaster = new MastersData())
                {
                    objLocalityMaster.CityId= Convert.ToInt32(ddlCity.SelectedValue.Trim());
                    objLocalityMaster.RegionName = txtRegion.Text.Trim();
                    DataTable dtLocality = objLocalityMaster.SelectRegionInfo();
                    if (dtLocality.Rows.Count > 0)
                        hdnfldLocalityID.Value = Convert.ToString(dtLocality.Rows[0]["LocalityID"]);
                    else
                        hdnfldLocalityID.Value = "0";
                }
            }
            catch (Exception ex)
            {
               PageBase.Errorhandling(ex);/*#CC02*/
            }
        }

        #endregion

    }
