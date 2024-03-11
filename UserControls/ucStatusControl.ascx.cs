/*
 ====================================================================================================
 Change Log 
 --------------- 
 * 21-Aug-2017, Kalpana, #CC01: hardcoded style removed and applied responsive css.
 * 19-Dec-2018, Rakesh Raj, #CC02: Imported from ZEDEBS
 ====================================================================================================
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_ucStatusControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string SelectedValue
    {
        get
        {
            return cmbStatus.SelectedValue;
        }

        set
        {
            cmbStatus.SelectedValue = value;
        }
    }

    public int SelectedIndex
    {
        get
        {
            return cmbStatus.SelectedIndex;
        }
        set
        {
            cmbStatus.SelectedIndex = value;
        }

    }
    public string SelectedText
    {
        get
        {
            return cmbStatus.SelectedItem.ToString();
        }

    }


}
