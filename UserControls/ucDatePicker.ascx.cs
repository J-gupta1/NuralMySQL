using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Controls
{

    public partial class ucDatePicker : System.Web.UI.UserControl
    {
        #region Private Class Variable

        private bool _blIsRequired = true;
        private string _strErrorMessage = string.Empty;
        private string _strRangeErrorMessage = string.Empty;
        private string _strValidationGroup = string.Empty;
        private DateTime _dtMinRange = DateTime.MinValue;
        private DateTime _dtMaxRange = DateTime.MaxValue;
        private bool _IsEnabled = true;

        #region Delegate Handling
        public event EventHandler OnCalenderTextChange;

        //    public event delCalender OnCalenderTextChange;
        //    public delegate void delCalender(string CalenderVal);
        
        #endregion
        #endregion

        #region Public Properties

        public bool defaultDateRange
        {
            get{
                //if(ViewState["defaultDateRange"] != null) 
                    return Convert.ToBoolean(ViewState["defaultDateRange"]);
                 }
            set
                {
                    ViewState["defaultDateRange"]=value;
               }
        }

        public string Date
        {
            get
            {
                if (txtDate != null)
                {
                    return txtDate.Text;
                }
                return string.Empty;
            }
            set
            {
                if (txtDate != null)
                {
                    txtDate.Text = value;
                }
            }
        }
        public DateTime? GetDate
        {
            get
            {
                if (txtDate != null && txtDate.Text != string.Empty)
                {
                    return Convert.ToDateTime(txtDate.Text);
                }
                return null;
            }
        }

        public bool IsEnabled
        {
            set
            {
                _IsEnabled = value;
                txtDate.Enabled = value;
                imgCalender.Enabled = value;
            }
            get { return _IsEnabled; }
        }

        public TextBox TextBoxDate
        {
            get { return txtDate; }
        }
        public bool IsRequired
        {
            set { _blIsRequired = value; }
        }
        public string ErrorMessage
        {
            set { _strErrorMessage = value; }
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
        public Button imgCal {

            get { return imgCalender; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            RequiredFieldValidator1.Enabled = _blIsRequired;
            RequiredFieldValidator1.ValidationGroup = _strValidationGroup;
            RequiredFieldValidator1.ErrorMessage = _strErrorMessage;

            RangeValidator1.ValidationGroup = _strValidationGroup;
            RangeValidator1.ErrorMessage = _strRangeErrorMessage;

            RangeValidator1.MinimumValue = _dtMinRange.Date.ToShortDateString();

            if (ViewState["defaultDateRange"] != null)
            {
                RangeValidator1.MaximumValue = DateTime.Now.Date.ToShortDateString();
            }
            else
            {
                RangeValidator1.MaximumValue = _dtMaxRange.Date.ToShortDateString();
            }
            
            //}
        }

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            if (OnCalenderTextChange != null)
            {
                OnCalenderTextChange.Invoke(sender, e);
            }
        }
}
}
