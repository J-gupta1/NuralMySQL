/*
====================================================================================================
Modification On       Modified By           Modification    
---------------      -----------           -------------------------------------------------------------  
 * 02-June-2017, Kalpana, #CC01: hardcoded style removed and applied responsive css
 * 21-Feb-2018, Kalpana, #CC02: CssClass added
=====================================================================================================
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LuminousSMS.Controls
{
    public partial class ucTimePicker : System.Web.UI.UserControl
    {
        #region Private Class Variable

        private bool _blIsRequired = true;
        private string _strErrorMessage = string.Empty;
        private string _strRangeErrorMessage = string.Empty;
        private string _strValidationGroup = string.Empty;
        private DateTime _dtMinRange = DateTime.MinValue;
        private DateTime _dtMaxRange = DateTime.MaxValue;

        #endregion

        #region Public Properties

        public string Time
        {
            get
            {
                if (txtTime != null)
                {
                    return txtTime.Text;
                }
                return string.Empty;
            }
            set
            {
                if (txtTime != null)
                {
                    txtTime.Text = value;
                }
            }
        }
        public TimeSpan? GetTime
        {
            get
            {
                if (txtTime != null && txtTime.Text != string.Empty)
                {
                    return Convert.ToDateTime(txtTime.Text).TimeOfDay;
                }
                return null;
            }
        }
        public TextBox TextBoxTime
        {
            get { return txtTime; }
        }
        public bool IsRequired
        {
            get { return _blIsRequired; }
            set 
            { 
                _blIsRequired = value;
                if (RequiredFieldValidator1 != null)
                    RequiredFieldValidator1.Enabled = _blIsRequired;
            }
        }
        public string ErrorMessage
        {
            //set { _strErrorMessage = value; }
            set 
            { 
                _strErrorMessage = value;
                if (RequiredFieldValidator1 != null)
                    RequiredFieldValidator1.ErrorMessage = _strErrorMessage;
            }
        }
        public string RangeErrorMessage
        {
            set { _strRangeErrorMessage = value; }
        }
        public string ValidationGroup
        {
            set { _strValidationGroup = value; }
        }
        public DateTime MinRangeValue
        {
            set { _dtMinRange = value; }
        }
        public DateTime MaxRangeValue
        {
            set { _dtMaxRange = value; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (ViewState["_ucCustomerProductDetail"] == null)
            //{
            RequiredFieldValidator1.Enabled = _blIsRequired;
            RequiredFieldValidator1.ValidationGroup = _strValidationGroup;
            RequiredFieldValidator1.ErrorMessage = _strErrorMessage;

            //RangeValidator1.ValidationGroup = _strValidationGroup;
            //RangeValidator1.ErrorMessage = _strRangeErrorMessage;

            //RangeValidator1.MinimumValue = _dtMinRange.ToString();
            //RangeValidator1.MaximumValue = _dtMaxRange.ToString();

            //    ViewState["_ucCustomerProductDetail"] = "Loaded";
            //}
        }
    }
}
