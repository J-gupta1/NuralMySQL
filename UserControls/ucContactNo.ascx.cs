
/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2010 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================

* Reviewed By :
 ====================================================================================================
Modification On       Modified By          Modification    
---------------      -----------          -------------------------------------------------------------  
 * 30-june-16            rajesh  upadhyay           #CC01: regular expression validation which was set hard code for phone and mobile no now made it configurable from appconfig
 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

 //Prashant Chitransh (16-May-2011):#Ch01- messange change.
    public partial class UserControls_ucContactNo : System.Web.UI.UserControl
    {
        public enum EnumContactTypes // Made by amit agarwal
        {
            Phone = 1,
            Mobile = 2,
            Other = 3
        }

        private bool _blIsRequired = true;
        private string _strErrorMessage = string.Empty;
        private string _strValidationGroup = string.Empty;
        private string _strValidationExpression = string.Empty;

        /*#CC01:commented start*/
        //const string _ValidationExpPhone = "^[0-9]{6,8}";
        //const string _ValidationExpMobile = "^[0-9]{10,10}";
        /*#CC01:commented end*/


        //const string _strValidationExpressionPhoneMessage = "Enter Minimum 6 digits.";    // #Ch01: Removed
        /*#CC01:commented start*/
        //const string _strValidationExpressionPhoneMessage = "Enter 6-8 digits number.";     // #Ch01: added
        //const string _strValidationExpressionMobileMessage = "Enter minimum 10 digits.";
        /*#CC01:commented start*/

        private EnumContactTypes _enumContactTypes = EnumContactTypes.Other;




        public EnumContactTypes ContactType
        {
            get
            {
                return _enumContactTypes;
               
            }
            set
            {
                _enumContactTypes = value;
            }
            
        }
        public string Text
        {
            get
            {
                if (txtContactNo != null)
                {
                    return txtContactNo.Text;
                }
                return string.Empty;
            }
            set
            {
                if (txtContactNo != null)
                    txtContactNo.Text = value;
            }
        }
        public string ValidationExpression
        {
            get
            {
                if (regexpContact1.ValidationExpression != string.Empty)
                {
                    return regexpContact1.ValidationExpression;
                }
                return string.Empty;
            }
            set
            {
                if (regexpContact1.ValidationExpression != string.Empty)
                  _strValidationExpression = value;
            }
        }
        public string ValidationExpressionMessage
        {
            get
            {
                if (regexpContact1.ValidationExpression != string.Empty)
                {
                    return regexpContact1.ValidationExpression;
                }
                return string.Empty;
            }
            set
            {
                if (regexpContact1.ValidationExpression != string.Empty)
                    _strValidationExpression = value;
            }
        }
        public bool IsRequired
        {
            get { return _blIsRequired; }
            set { _blIsRequired = value; }
        }
        public string ErrorMessage
        {
            set { _strErrorMessage = value; }
        }
        public string ValidationGroup
        {
            set { _strValidationGroup = value; }
        }
        public bool Enabled
        {
            set { txtContactNo.Enabled = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {




            if (ViewState["_ucContactNo"] == null)
            {
                RequiredFieldValidator1.Enabled = _blIsRequired;
                RequiredFieldValidator1.ValidationGroup = _strValidationGroup;
                RequiredFieldValidator1.ErrorMessage = _strErrorMessage;
                /*#CC01:commented start*/
                // regexpContact1.ValidationExpression = _strValidationExpression;
                // regexpContact1.ValidationGroup = _strValidationGroup;
                /*#CC01:commented end*/

                /*#CC01:added start*/
                    var lstMobileno = "";// rakesh PageBase.GetEnumByTableName("AppConfig", "MobilenoExpression");//added by #ECC16(start)//
                    string exprMobileno = "^[0-9]";//lstMobileno.AsEnumerable().Select(x => x.Field<string>("Value")).Single().ToString();

                    var lstPhoneNo = "";//rakesh Pagebase.GetEnumByTableName("AppConfig", "PhoneNoExpression");//added by #ECC16(start)//
                    string exprPhoneNo = "^[0-9]";//lstPhoneNo.AsEnumerable().Select(x => x.Field<string>("Value")).Single().ToString();
                    /*#CC01:added end*/

                    if (ContactType == EnumContactTypes.Mobile)
                    {
                        /*#CC01:added start*/
                        if (exprMobileno != "")
                        {
                            regexpContact1.ValidationExpression = exprMobileno;
                            regexpContact1.ErrorMessage = "Resources.CommonMessages.MobileNo; ";
                            regexpContact1.ValidationGroup = _strValidationGroup;

                        }
                        /*#CC01:added end*/

                        /*#CC01:commented start*/
                        //txtContactNo.MaxLength = 10;
                        //regexpContact1.ValidationExpression = _ValidationExpMobile;
                        //regexpContact1.ErrorMessage = _strValidationExpressionMobileMessage;
                        /*#CC01:commented end*/
                    }
                    else if (ContactType == EnumContactTypes.Phone)
                    {
                        /*#CC01:added start*/
                        if (exprPhoneNo != "")
                        {
                            regexpContact1.ValidationExpression = exprPhoneNo;
                            regexpContact1.ErrorMessage ="Resources.CommonMessages.PhoneNo;"  ; 
                            regexpContact1.ValidationGroup = _strValidationGroup;
                              
                        }
                        /*#CC01:added start*/

                        /*#CC01:commented start*/
                        //txtContactNo.MaxLength = 8;
                        //regexpContact1.ValidationExpression = _ValidationExpPhone;
                        //regexpContact1.ErrorMessage = _strValidationExpressionPhoneMessage;
                        /*#CC01:commented end*/
                    }
                    else
                    {
                        regexpContact1.ValidationExpression = string.Empty;
                        regexpContact1.ErrorMessage = string.Empty;
                    }

                    ViewState["_ucContactNo"] = "Loaded";
                }
            
        }
    }

