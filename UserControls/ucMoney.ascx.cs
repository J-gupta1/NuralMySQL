using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZedEBS.Controls
{
    public partial class ucMoney : System.Web.UI.UserControl
    {
        #region Delegate Events

        public delegate void DelegateTextChanged(object sender, EventArgs e);
        public event DelegateTextChanged TextChanged;

        #endregion

        #region Private Class Variable

        private bool _blIsRequired = true;
        private string _strErrorMessage = string.Empty;
        private string _strValidationGroup = string.Empty;
        private string _strRangeErrorMessage = string.Empty;
        private float _fMoney;
        private float _fMinMoney = 0f;
        private float _fMaxMoney = 999999f;


        #endregion

        #region Public Properties

        public string Text
        {
            get
            {
                if (txtAmount != null)
                {
                    return txtAmount.Text;
                }
                return string.Empty;
            }
            set
            {
                if (txtAmount != null)
                {
                    double d = 0;
                    double.TryParse(value, out d);
                    txtAmount.Text = string.Format("{0:0.00}", d);
                }
            }
        }
        public float Money
        {
            get
            {
                if (txtAmount != null)
                {
                    float.TryParse(txtAmount.Text, out _fMoney);
                    return _fMoney;
                }
                return _fMoney;
            }
        }
        public float MinMoney
        {
            get
            {
                if (txtAmount != null)
                {

                    float.TryParse(txtAmount.Text, out _fMinMoney);
                    return _fMinMoney;
                }
                return _fMinMoney;
            }
        }
        public float MaxMoney
        {
            get
            {
                if (txtAmount != null)
                {
                    float.TryParse(txtAmount.Text, out _fMaxMoney);
                    return _fMaxMoney;
                }
                return _fMaxMoney;
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
        public string RangeErrorMessage
        {
            set { _strRangeErrorMessage = value; }
        }
        public string ValidationGroup
        {
            set { _strValidationGroup = value; }
        }
        public bool ReadOnly
        {
            set
            {
                if (txtAmount != null)
                    txtAmount.ReadOnly = value;
            }
        }
        public bool AutoPostBack
        {
            set
            {
                if (txtAmount != null)
                {
                    txtAmount.AutoPostBack = value;
                }
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            rfvAmount.Enabled = _blIsRequired;
            rfvAmount.ValidationGroup = _strValidationGroup;
            rfvAmount.ErrorMessage = _strErrorMessage;

            rvAmount.ValidationGroup = _strValidationGroup;
            rvAmount.ErrorMessage = _strRangeErrorMessage;
            rvAmount.MinimumValue = _fMinMoney.ToString();
            rvAmount.MaximumValue = _fMaxMoney.ToString();

            regexpAmount.ValidationGroup = _strValidationGroup;
        }

        protected void txtAmount_TextChanged(object sender, EventArgs e)
        {
            if (TextChanged != null)
                TextChanged(this, e);
        }
    }
}
