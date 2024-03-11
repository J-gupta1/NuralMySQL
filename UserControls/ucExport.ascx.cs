#region Copyright(c) 2015 Zed-Axis Technologies All rights are reserved
/*/
 * ====================================================================================================
 * <copyright company="Zed Axis Technologies">
 * COPYRIGHT (c) 2012 Zed Axis Technologies (P) Ltd. 
 * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
 * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
 * WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
 * </copyright>
 * ====================================================================================================
 * Created By : Rakesh Raj
 * Module : User Conrols 
 * Date : 17 July 2018
 * Description : Export File Type like .xlsx, .csv, .pdf 
 * ====================================================================================================
 * Reviewed By :
 ====================================================================================================
 * Change Log :
 * Date, Name, #CCxx, Description 
 
 ====================================================================================================
*/

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_ucExport : System.Web.UI.UserControl
{
    private bool _blIsRequired = false;
    private string _strValidationGroup = string.Empty;
    private string _intInitialValue = "0";
    
    #region Public Properties
    public string SelectedText
    {
        get { return ddlExportType.SelectedItem.Text; }
        set { ddlExportType.SelectedItem.Text = value; }
    }
    public int SelectedValue
    {
        get { return Convert.ToInt32(ddlExportType.SelectedValue); }
        set { ddlExportType.SelectedValue = value.ToString(); }
    }
    public int SelectedIndex
    {
        get { return ddlExportType.SelectedIndex; }
        set { ddlExportType.SelectedIndex = value; }
    }
    public bool IsRequired
    {
        get { return _blIsRequired; }
        set { _blIsRequired = value; }
    }
    public string ValidationGroup
    {
        set { _strValidationGroup = value; }
    }
    public string InitialValue
    {
        get { return _intInitialValue; }
        set { _intInitialValue = value; }
    }
    public bool AutoPostBack
    {
        get { return ddlExportType.AutoPostBack; }
        set { ddlExportType.AutoPostBack = value; }
    }
    public bool Enabled
    {
        get { return ddlExportType.Enabled; }
        set { ddlExportType.Enabled = value; }
    }
    public string rvErrorMessage
    {
        get { return rvExportType.ErrorMessage; }
        set { rvExportType.ErrorMessage = value; }
    }
   
    public double Width
    {
        get { return ddlExportType.Width.Value; }
        set { ddlExportType.Width = Convert.ToUInt16(value); }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (_blIsRequired)
        {
            rvExportType.Enabled = true;
            rvExportType.ValidationGroup = _strValidationGroup;
            rvExportType.InitialValue = _intInitialValue;
            ddlExportType.ValidationGroup = _strValidationGroup;
        }
        else
        {
            rvExportType.Enabled = false;
        }

    }
   
}