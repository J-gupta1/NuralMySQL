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
 * Date : 27 July 2015
 * Description : Sales Channel Type According to Entity
 * ====================================================================================================
 * Reviewed By :
 ====================================================================================================
 * Change Log :
 * Date, Name, #CCxx, Description 
 * 07-April-2018,Vijay Kumar Prajapati,#CC01,Add/Coment new Method for bind entitytypeid.
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

public partial class UserControl_ucEntityType : System.Web.UI.UserControl
{
    private bool _blIsRequired = false;
    private string _strValidationGroup = string.Empty;
    private string _intInitialValue = "0";
    #region Public Properties
    public string SelectedText
    {
        get { return ddlEntityType.SelectedItem.Text; }
        set { ddlEntityType.SelectedItem.Text = value; }
    }
    public int SelectedValue
    {
        get { return Convert.ToInt32(ddlEntityType.SelectedValue); }
        set { ddlEntityType.SelectedValue = value.ToString(); }
    }
    public int SelectedIndex
    {
        get { return ddlEntityType.SelectedIndex; }
        set { ddlEntityType.SelectedIndex = value; }
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
        get { return ddlEntityType.AutoPostBack; }
        set { ddlEntityType.AutoPostBack = value; }
    }
    public bool Enabled
    {
        get { return ddlEntityType.Enabled; }
        set { ddlEntityType.Enabled = value; }
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
        get { return ddlEntityType.Width.Value; }
        set { ddlEntityType.Width = Convert.ToUInt16(value); }
    }
    #endregion
    #region Function

    public void fillServiceEntity()
    {
        /*#CC01 Commented Started*/
       // DataAccess.ReportData ob = new DataAccess.ReportData();
        //ob.UserId = BussinessLogic.PageBase.UserId;
        //ob.SpecificSaleChannelId = BussinessLogic.PageBase.UserId;
        //DataSet ds = ob.getSaleChannelType();
        //fillucDropdownLists(ddlEntityType, ds, "SalesChannelTypeID", "SalesChannelTypeName");
        //if (ds.Tables[0].Rows.Count == 1)
        //{
        //    ddlEntityType.Items.RemoveAt(0);
        //    SelectedIndex = 0;
        //}
        /*#CC01 Commented Started*/
        /*#CC01 Added Started*/
       using(SalesChannelData ObjSalesChannel = new SalesChannelData())
       {
           DataSet ds = new DataSet();
           DataTable dt = new DataTable();
           ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(BussinessLogic.PageBase.EntityTypeID);
           ObjSalesChannel.BaseEntityTypeID = Convert.ToInt16(BussinessLogic.PageBase.BaseEntityTypeID);
           dt = ObjSalesChannel.FetchSalesChannelTypeSB();
           ds.Tables.Add(dt.Copy());
           fillucDropdownLists(ddlEntityType, ds, "EntityTypeID", "EntityType");
           if (ds.Tables[0].Rows.Count == 1)
           {
               ddlEntityType.Items.RemoveAt(0);
               SelectedIndex = 0;
           }
       }
        /*#CC01 Added End*/
    }
    public void clearServiceEntity()
    {
        ddlEntityType.Items.Clear();
        ListItem li = new ListItem();
        li.Text = "--Select--";
        li.Value = "0";
        ddlEntityType.Items.Insert(0, li);
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
            ddlEntityType.ValidationGroup = _strValidationGroup;
        }
        else
        {
            rvServiceEntity.Enabled = false;
        }
        if (!IsPostBack)
        {
            fillServiceEntity();
        }
    }
    public delegate void EventHandler(Object obj, EventArgs e);
    public event EventHandler IndexChage;
    protected void OnIndexChage()
    {
        if (IndexChage != null)
        {
            IndexChage(this, new EventArgs());
        }
    }
    protected void ddlEntityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        OnIndexChage();
        Session["EntityType"] = ddlEntityType.SelectedValue;
    }
    #endregion
}
