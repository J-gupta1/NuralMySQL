#region Copyright(c) 2012 Zed-Axis Technologies All rights are reserved
/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2012 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Created By : Amit Agarwal
* Module : User Conrols 
* Description : Multiline Textbox having chars count with javascript.
 * Table Name: 
* ====================================================================================================
* Reviewed By :
 ====================================================================================================
* 27-Feb-2018, Kalpana, #CC01: hardcoded style removed and applied responsive css

 ====================================================================================================
*/

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class UserControls_ucTextboxMultiline : System.Web.UI.UserControl
{
    #region Delegate Events

    public delegate void DelegateTextChanged(string text);
    public event DelegateTextChanged TextChanged;

    #endregion

    #region Private Class Variable

    private bool _blIsRequired = true;
    private string _strErrorMessage = string.Empty;
    private string _strRangeErrorMessage = string.Empty;
    private string _strValidationGroup = string.Empty;
    private int _intMaxCharsLength = 50;

    #endregion

    #region Public Properties



    public int CharsLength
    {
        get { return _intMaxCharsLength; }
        set
        {
            _intMaxCharsLength = value;
        }
    }



    public string TextBoxText
    {
        get { return txtMultiline.Text; }
        set
        {
            txtMultiline.Text = value;
            hdnCharsLeft.Value = _intMaxCharsLength.ToString();
        }
    }


    public string TextBoxWatermarkText
    {
        get { return tbweMultiline.WatermarkText; }
        set
        {
            tbweMultiline.WatermarkText = value;
        }
    }


    public RequiredFieldValidator RequiredDealer
    {
        get { return rfvMultiline; }
    }


    public bool IsRequired
    {
        get { return _blIsRequired; }
        set
        {
            _blIsRequired = value;
            if (rfvMultiline != null)
                rfvMultiline.Enabled = _blIsRequired;
        }
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
        set
        {
            txtMultiline.Enabled = value;

        }
    }
    public bool AutoPostBack
    {
        set
        {
            txtMultiline.AutoPostBack = value;
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        rfvMultiline.Visible = _blIsRequired;
        rfvMultiline.Enabled = _blIsRequired;
        rfvMultiline.ValidationGroup = _strValidationGroup;
        rfvMultiline.ErrorMessage = _strErrorMessage;
        txtMultiline.MaxLength = _intMaxCharsLength;
        txtMultiline.Attributes.Add("CharsLength", _intMaxCharsLength.ToString());
        remLen1.Text = _intMaxCharsLength.ToString();
        txtMultiline.Attributes.Add("OnKeyDown", "textCounter('" + this.ClientID + "','" + txtMultiline.ID + "','" + remLen1.ID + "','" + hdnCharsLeft.ID + "');");
        txtMultiline.Attributes.Add("OnKeyUp", "textCounter('" + this.ClientID + "','" + txtMultiline.ID + "','" + remLen1.ID + "','" + hdnCharsLeft.ID + "');");
        if (!IsPostBack)
        {
            hdnCharsLeft.Value = _intMaxCharsLength.ToString(); ;
        }
        if (!IsPostBack == false)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), this.ClientID, "SetValueOnPostback('" + this.ClientID + "','" + this.remLen1.ID + "','" + this.hdnCharsLeft.ID + "');", true);
        }
    }

    protected void txtMultiline_TextChanged(object sender, EventArgs e)
    {
        if (TextChanged != null)
        {
            TextChanged(txtMultiline.Text);
        }
    }

    public void SetMonthStartDate()
    {
        int _intdate = DateTime.Today.Day - 1;
        txtMultiline.Text = DateTime.Now.AddDays(-_intdate).ToShortDateString();
    }

    public void SetMonthEndDate()
    {
        DateTime dtLastDateofMonth = DateTime.Today.AddMonths(1);
        int _intdate = DateTime.Now.Day;
        txtMultiline.Text = dtLastDateofMonth.AddDays(-_intdate).ToShortDateString();
    }
}
