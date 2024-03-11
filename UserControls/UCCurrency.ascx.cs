#region Copyright(c) 2010 Zed-Axis Technologies All rights are reserved
/*
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2010 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Created By    : Dhiraj Kumar
* Role          : SE
* Module        : User Control
* Description   : This control provides drop down of currency selection.
* Table Name    : CurrencyMaster
* ====================================================================================================
* Reviewed By :
* ====================================================================================================
* Change Log :
* ==================================================================================================== * 
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZedAxis.ZedEBS;
using System.Data;
using BussinessLogic;

public partial class UserControls_UCCurrency : System.Web.UI.UserControl
{
    #region Private Variables
    private bool _blIsRequired = false;
    private AutoPostPack _blAutoPostBack = AutoPostPack.False;
    private string _strValidationGroup = string.Empty;
    private string _strSelectedValue = string.Empty;
    private bool _blEnabled = true;
    
    #endregion Private Variables

    #region Public Properties
    public bool IsRequired
    {
        get { return _blIsRequired; }
        set
        {
            _blIsRequired = value;
            if (reqddlCurrency!= null)
                reqddlCurrency.Enabled = _blIsRequired;
        }
    }

    public bool Enabled
    {
        set 
        {
            _blEnabled = value;
            if (_blEnabled == true)
            {
                ddlCurrency.Attributes.Add("disabled", "false");
            }
            else
            {
                ddlCurrency.Attributes.Add("disabled", "true");
            }
        }
    }
    public AutoPostPack AutoPostBack
    {
        get
        {

            return _blAutoPostBack;
        }
        set
        {
            _blAutoPostBack = value;
            //  ddlServiceCentre.AutoPostBack = _blAutoPostBack;
        }
    }
    public string ValidationGroup
    {
        set
        {
            _strValidationGroup = value;
            reqddlCurrency.ValidationGroup = value;
        }
    }
    public string SelectedValue
    {
        get { return ddlCurrency.SelectedValue; }
        set 
        { 
            _strSelectedValue = value;
            if (_strSelectedValue != "" && _strSelectedValue != null)
            {
                ddlCurrency.SelectedValue = _strSelectedValue;
            }
        }
    }
    #endregion Public Properties

    protected void Page_Load(object sender, EventArgs e)
    {
        ddlCurrency.AutoPostBack = Convert.ToBoolean(_blAutoPostBack);
        if (!IsPostBack)
        {
            using (UC_CurrencyList objCurrency = new UC_CurrencyList())
            {
                DataTable dt = objCurrency.GetCurrency(1);
                ddlCurrency.DataSource = dt;
                ddlCurrency.DataTextField = "CurrencyName";
                ddlCurrency.DataValueField = "CurrencyID";
                ddlCurrency.DataBind();
            }
        }
    }

    public enum AutoPostPack
    {
        True = 1,
        False = 0
    }
}
