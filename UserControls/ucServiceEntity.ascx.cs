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
 * Created By : Karam Chand Sharma
 * Module : User Conrols 
 * Description : Sales Channel DropdownList Bind.
 * ====================================================================================================
 * Reviewed By :
 ====================================================================================================
 * Change Log :
 * Date, Name, #CCxx, Description 
 * 24-Oct-2018, Sumit Maurya, #CC01, New property created to get data according to new method (done for motorola).
 ====================================================================================================
*/

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class UserControl_ucServiceEntity : System.Web.UI.UserControl
{
    private bool _blIsRequired = false;
    private string _strValidationGroup = string.Empty;
    private string _intInitialValue = "0";
    #region Public Properties
    public string SelectedText
    {
        get { return ddlServiceEntity.SelectedItem.Text; }
        set { ddlServiceEntity.SelectedItem.Text = value; }
    }
    public int SelectedValue
    {
        get { return Convert.ToInt32(ddlServiceEntity.SelectedValue); }
        set { ddlServiceEntity.SelectedValue = value.ToString(); }
    }
    public int SelectedIndex
    {
        get { return ddlServiceEntity.SelectedIndex; }
        set { ddlServiceEntity.SelectedIndex = value; }
    }
    public bool IsValidServiceEntity
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
        get { return ddlServiceEntity.AutoPostBack; }
        set { ddlServiceEntity.AutoPostBack = value; }
    }
    public bool Enabled
    {
        get { return ddlServiceEntity.Enabled; }
        set { ddlServiceEntity.Enabled = value; }
    }
    public string rvErrorMessage
    {
        get { return rvServiceEntity.ErrorMessage; }
        set { rvServiceEntity.ErrorMessage = value; }
    }
    public Int16 UserId { get; set; }
    private int _SpecificSaleChannelId = 0;
    public int SpecificSaleChannelId
    {
        get { return _SpecificSaleChannelId; }
        set { _SpecificSaleChannelId = value; }
    }
    public double Width
    {
        get { return ddlServiceEntity.Width.Value; }
        set { ddlServiceEntity.Width = Convert.ToUInt16(value); }
    }

    /* #CC01 Add Start */
     private Int16 _BindSaleschannel = 0;

public Int16 BindSaleschannel
{
  get { return _BindSaleschannel; }
  set { _BindSaleschannel = value; }
}
    /* #CC01 Add End */

    #endregion
    #region Function

    public void fillServiceEntity()
    {
        DataAccess.ReportData ob = new DataAccess.ReportData();
        ob.UserId = BussinessLogic.PageBase.UserId;
        ob.SpecificSaleChannelId = BussinessLogic.PageBase.SalesChanelID; 
        DataSet ds = ob.getServiceEntity();
        fillucDropdownLists(ddlServiceEntity, ds, "SalesChannelID", "SalesChannelName");
        if (ds.Tables[0].Rows.Count == 1)
        {
            ddlServiceEntity.Items.RemoveAt(0);
            SelectedIndex = 0;
        }
    }
    /* #CC01 Add Start */
    public void fillSalesChannel()
    {
        using (DataAccess.ReportData ob = new DataAccess.ReportData())
        {
            ob.UserId = BussinessLogic.PageBase.UserId;
            ob.SpecificSaleChannelId = BussinessLogic.PageBase.SalesChanelID;
            DataSet ds = ob.GetSalesChannelList();
            fillucDropdownLists(ddlServiceEntity, ds, "SalesChannelID", "SalesChannelName");
            if (ds.Tables[0].Rows.Count == 1)
            {
                ddlServiceEntity.Items.RemoveAt(0);
                SelectedIndex = 0;
            }
        }
    }
    /* #CC01 Add End */

    public void clearServiceEntity()
    {
        ddlServiceEntity.Items.Clear();
        ListItem li = new ListItem();
        li.Text = "--Select--";
        li.Value = "0";
        ddlServiceEntity.Items.Insert(0, li);
    }

    public void fillucDropdownLists(DropDownList ddlToFill, DataSet dsDataToFill, string strValueField, string strTextField)
    {
        try
        {
            if (dsDataToFill != null && dsDataToFill.Tables.Count > 0)
            {
                ddlToFill.DataSource = dsDataToFill.Tables[0].DefaultView;
                ddlToFill.DataValueField = strValueField;
                ddlToFill.DataTextField = strTextField;
                ddlToFill.DataBind();
                ListItem oListItemModel = ddlToFill.Items.FindByValue("0");
                if (oListItemModel == null)
                {
                    ddlToFill.Items.Insert(0, new ListItem("--Select--", "0"));
                }

            }
        }
        catch (Exception ex)
        {

        }
    }


    #endregion
    #region Event
    protected void Page_Load(object sender, EventArgs e)
    {
        if (_blIsRequired)
        {
            rvServiceEntity.Enabled = true;
            rvServiceEntity.ValidationGroup = _strValidationGroup;
            rvServiceEntity.InitialValue = _intInitialValue;
            ddlServiceEntity.ValidationGroup = _strValidationGroup;
        }
        else
        {
            rvServiceEntity.Enabled = false;
        }
        if (!IsPostBack)
        {
            /* #CC01 Add Start */
            if (Convert.ToInt16(BindSaleschannel) == 1)
            {
                fillSalesChannel();
            }
            else
            {/* #CC01 Add End */
                fillServiceEntity();
            }/* #CC01 Added */
        }
    }
    #endregion
    #region DDLEvent
    public delegate void EventHandler(Object obj, EventArgs e);
    public event EventHandler IndexChage;
    protected void OnIndexChage()
    {
        if (IndexChage != null)
        {
            IndexChage(this, new EventArgs());
        }
    }
    protected void ddlServiceEntity_SelectedIndexChanged(object sender, EventArgs e)
    {
        OnIndexChage();
    }
    #endregion
}
