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
* Created By    : Amit Agarwal
* Role          : SE
* Module        : User Control
* Description   : This control provides drop down of multiple selection or single selection for Entities basis on the access.
* Table Name    : 
* ====================================================================================================
* Reviewed By :
* ====================================================================================================
 * Change Log :
 * 02-Dec-2011, Prashant Chitransh, #Ch01: RequiredFieldValidator added.
 * 03-Dec-2011, Prashant Chitransh, #Ch02: Change in binding because previous procedure was showing only service centres.
 * 14-mar-2012, Amit Agarwal , #ch03:  DevExpress controls validations are not compatible with standard .net validations so
                                this file need to be add a refrence on UserControl if any page containing both devexpress
                                controls with validation setting  and standard .net framework vaidation then submitting button
                                need to call a method (i.e OnLinkButtonClick()) explicitly on the Event named (onClientClick) 
                                for validating the page.  
                                Exp: OnClientClick="return OnLinkButtonClick();"
                                Points to Remember
                                1. Add this property on submitting Button: OnClientClick="return OnLinkButtonClick();"
                                2. add this Line of Code to UserControl on PageLoad Event if UserControl has any of devExpress Control with 
                                Validation Setting:   ScriptManager.GetCurrent(this.Page).Scripts.Add(new ScriptReference("~/Assets/Scripts/DevExValidationCampatibility.js"));
                                3. Never Set any of Page Validator's ValidationGroup property on page otherwise validations will not fire.  
 *01-Oct-2012,  Amit Agarwal,   #CH04: Required field validator not getting active on postback, add code to make it active
 *01-Oct-2012,  Amit Agarwal,   #CH05: Create clear function to clear all items.
 *15-May-2013,  Dhiraj Kumar,   #CH06: User tryparse function before setting _long_SelectedValue.
 *25-May-2013,  Dhiraj Kumar,   #CH07: Create new property for setting selected value.
 *27-june-2014, Shashikant Singh #CC08 : Added Enalbe and SelectedIndex Property.
*17-oct-2014, SHilpi Sharma, #CH09:added new Property to activeMode
*25-June-2015,Shashikant Singh,#CH10 :Added new property LoginEntityRequired as told by mittal sir
*25-Jan-2016, Sumit Maurya, #CC10, Client side validation was not working as path of "DevExValidationCampatibility.js" was hardcoded. now path is configurable.
*27-Jun-2016, Vijay Katiyar, #CC11, Added Display="Dynamic" in required field due to Error message show on page load and after selection entity
*12-Nov-2016, Shashikant Singh, #CC12, Also added activemode condintion when load parrent entity List.
*12-May-2017, Kalpana, #CC13, hardcoded style removed and applied responsive css
* ==================================================================================================== * 
*/
#endregion

using System;
using System.Data;
using System.ComponentModel;
using DevExpress.Web.ASPxEditors;
using ZedAxis.ZedEBS;
using ZedAxis.ZedEBS.Enums;
using System.Web.UI;
using BusinessLogics;
using BussinessLogic;


public partial class UserControls_ucEntityList : System.Web.UI.UserControl
{
    #region Delegate & Events

    public delegate void DelegateSelectedEntity(Int32 EntityID); /* #Ch02: added */
    public event DelegateSelectedEntity SelectedEntity;          /* #Ch02: added */
    #endregion

    #region Private Class Variable

    public enum EnumIncrementalFilteringMode
    {
        Contains = 0,
        StartsWith = 1,
        None = 2,

    }
    private Int16 _intActiveMode = 1;/*CH08:added0-Both type of records (ACtive+inactive),1-means only active records*/
    private DataTable _dataTableEntityTypeIDs;
    private Int32 _intEntityID = 0;
    private Int32 _intEntityTypeID = 0;
    private byte _intGetEntityTypeID = 2; // Service Center By Default
    private bool _blIsRequired = false;
    private AutoPostPack _blAutoPostBack = AutoPostPack.False;               /* #Ch02: added */
    private string _strValidationGroup = string.Empty;
    private string _strErrorMessage = string.Empty;
    private string _strInitialValue = "Please Choose";     /* #Ch02: added */
    private Int16 _intShowBehaviour = 1;                //#CC04 added
    private EnumSelectionMode _byteSelectionMode = EnumSelectionMode.Single;
    private int _long_SelectedValue = 0;
    private string _strInitialValue_req;
    private int _byteEntityTypeDescription = 1;
    public EnumIncrementalFilteringMode _IncrementalFilteringMode = EnumIncrementalFilteringMode.Contains;
    private int _int_ParentSelectedValue = 0;
    private int _intSetSelectedValue; //#CH07: Added

    private Int16 _ForceTypeMatching = 0;
    private Int16 _LoginEntityRequired = 0; /*#CH10:added*/
    
    
    #endregion

    #region Public Properties

    /*#CC08:Added Start*/
    public int SelectedIndex
    {
        get
        {
            return ZedComboBoxDevEx.SelectedIndex;
        }
        set
        {
            ZedComboBoxDevEx.SelectedIndex = value;
        }
    }

    [DefaultValue(true)]
    public bool Enable
    {
        set
        {
            ZedComboBoxDevEx.Enabled = value;
        }
    }

    /*#CC08:Added End*/

    [DefaultValue(true)]


    public EnumIncrementalFilteringMode IncrementalFilteringMode
    {
        set
        {
            _IncrementalFilteringMode = value;

        }

    }
    public AutoPostPack AutoPostBack
    {
        get {

            return _blAutoPostBack;
        }
        set
        {
            _blAutoPostBack = value;
            //  ddlServiceCentre.AutoPostBack = _blAutoPostBack;
        }
    }

    public string InitialValue
    {
        get
        { return _strInitialValue_req; }
        set
        {
            _strInitialValue_req = value;
            reqddlSC.InitialValue = value;
        }
    }
   
    public int ParentSelectedValue
    {
        get {

            return _int_ParentSelectedValue;
        }
        set
        {
            _int_ParentSelectedValue = value;
        }
    }

    public long SelectedValue {
        get {
            return _long_SelectedValue;
        }
        set
        {
            if (IsSingleSelect())

                ZedComboBoxDevEx.Value = value;
        }
        
    }
    public string SetInitialValue
    {
        get {
            return _strInitialValue;
        }
        set
        {
            _strInitialValue = value;

          //  _strInitialValue = value;
            //   ddlServiceCentre.SelectedValue = _strInitialValue;
        }
    }



    public Int32 EntityID
    {
        get
        {
            return _intEntityID;
        }
        set
        {
            _intEntityID = value;
        }
    }

    public bool IsRequired
    {
        get { return _blIsRequired; }
        set
        {
            _blIsRequired = value;
            if (IsSingleSelect() == false)
            {
                if (reqddlSC != null)
                    reqddlSC.Enabled = _blIsRequired;
            }
            else
            {
                ((DevExpress.Web.ASPxEditors.ValidationSettings)((ASPxEdit)ZedComboBoxDevEx).ValidationSettings).ErrorDisplayMode = ErrorDisplayMode.Text;
                ((RequiredFieldValidationPattern)((DevExpress.Web.ASPxEditors.ValidationSettings)((ASPxEdit)ZedComboBoxDevEx).ValidationSettings).RequiredField).IsRequired = _blIsRequired;

            }
        }
    }

    public string ErrorMessage
    {
        set
        {
            _strErrorMessage = value;
            reqddlSC.ErrorMessage = value;
        }
    }

    public Int32 EntityTypeID
    {
        get
        {
            return _intEntityTypeID;
        }
        set
        {
            _intEntityTypeID = value;
        }
    }


    public byte GetEntityTypeID
    {
        get
        {
            return _intGetEntityTypeID;
        }
        set
        {
            _intGetEntityTypeID = value;
        }
    }


  

    public string ValidationGroup
    {
        set
        {
            _strValidationGroup = value;
            reqddlSC.ValidationGroup = value;
        }
    }

    
    public enum EntityType
    {
        PRIMARY = 1,
        SECONDARY = 2
    }


    public EnumSelectionMode SelectionMode
    {
        get
        {
            return _byteSelectionMode;
        }
        set
        {
            _byteSelectionMode = value;

        }
    }
    public EntityType EntityType_entityType = EntityType.SECONDARY;

    public string buis_keyword;

    /// <summary>
    ///  Provide values to be set in dropdown
    /// </summary>
    /// <param name="dtSource">Source Datatable to be set in dropdown</param>



    public void Reset()
    {
        ASPxListBox objlst = (ASPxListBox)ddlSC.FindControl("lstSC");
        if (SelectionMode != EnumSelectionMode.Single)
        {
            

            //if (objlst.SelectionMode != ListEditSelectionMode.Single)
            //{


            foreach (ListEditItem lstItm in objlst.Items)
            {
                lstItm.Selected = false;

            }
            ddlSC.Text = _strInitialValue;

        }
        else
        {
            ZedComboBoxDevEx.SelectedIndex = 0;
            if (ddlSC.Enabled == false)
            {
                if (ViewState["EntityName"] != null)
                {
                    ddlSC.Text = ViewState["EntityName"].ToString();
                }
                else
                {
                    ddlSC.Text = _strInitialValue;
                }

            }
            else
            {
                ddlSC.Text = _strInitialValue;
            }



            foreach (ListEditItem lstItm in ZedComboBoxDevEx.Items)
            {
                lstItm.Selected = false;

            }
            

        }
    }

    /*#CH05:Start - Added*/
    public void Clear()
    {
        ASPxListBox objlst = (ASPxListBox)ddlSC.FindControl("lstSC");
        if (SelectionMode != EnumSelectionMode.Single)
        {
            objlst.Items.Clear();
            ddlSC.Text = _strInitialValue;
        }
        else
        {
          //  ZedComboBoxDevEx.SelectedIndex = 0;
            ZedComboBoxDevEx.Text = _strInitialValue;            
            ZedComboBoxDevEx.Items.Clear();            
            
        }
    }
    /*#CH05:End - Added*/

    public void SetValues(DataTable dtSource)
    {
        if (dtSource.Columns.Count == 1 && dtSource.Columns[0].ColumnName == "SecondaryEntityID")
        {
            if (dtSource.Rows.Count > 0)
            {
                if (IsSingleSelect() == false)
                {
                    ASPxListBox objlst = (ASPxListBox)ddlSC.FindControl("lstSC");

                    foreach (ListEditItem lstItm in objlst.Items)
                    {
                        foreach (DataRow dr in dtSource.Rows)
                        {
                            if (lstItm.Value == dr["SecondaryEntityID"])
                            {
                                lstItm.Selected = true;
                            }
                        }
                    }
                }

            }
        }

    }

    /// <summary>
    ///  List of the selected items in Dropdown   
    /// </summary>
    public DataTable GetSelectedValues
    {
        get
        {
            try
            {
                using (DataTable dt = new DataTable())
                {
                    DataColumn dc = new DataColumn("SecondaryEntityID");
                    dc.DataType = typeof(int);
                    dt.Columns.Add(dc);
                    if (IsSingleSelect() == false)
                    {

                        ASPxListBox objlst = (ASPxListBox)ddlSC.FindControl("lstSC");
                        foreach (ListEditItem lstItm in objlst.Items)
                        {
                            DataRow dr = dt.NewRow();
                            if (lstItm.Selected)
                            {
                                dr["SecondaryEntityID"] = lstItm.Value;
                                dt.Rows.Add(dr);
                            }

                            dt.AcceptChanges();
                        }
                    }
                    else
                    {
                        DataRow dr = dt.NewRow();
                        dr["SecondaryEntityID"] = ZedComboBoxDevEx.Value;
                        dt.Rows.Add(dr);
                        dt.AcceptChanges();

                    }
                    return dt;
                }
            }
            catch (Exception ex)
            {
                return new DataTable();
            }
        }
    }





    public Int16 ShowBehaviour  //#CC04 added
    {
        get
        {
            return _intShowBehaviour;
        }
        set
        {
            _intShowBehaviour = value;
        }
    }

    public int SetSelectedValue       //#CH07: Added
    {
        get { return _intSetSelectedValue; }
        set { _intSetSelectedValue = value; }
    }
    #endregion
    public Int16 ForceTypeMatching
    {
        get { return _ForceTypeMatching; }
        set { _ForceTypeMatching = value; }
    }
    /*#CH10:added start*/
    public Int16 LoginEntityRequired
    {
        get { return _LoginEntityRequired; }
        set { _LoginEntityRequired = value; }
    }
    /*#CH10:added end*/

    public enum EntityTypeList
    {
        True = 1,
        False = 0
    }

    public enum Parent
    {
        True = 1,
        False = 0
    }




    public void Select_DeSelect_All(bool isSelect)
    {
        if (IsSingleSelect() == false)
        {
            ASPxListBox objlst = (ASPxListBox)ddlSC.FindControl("lstSC");
            foreach (ListEditItem lstItm in objlst.Items)
            {
                lstItm.Selected = isSelect;

            }
            ddlSC.Text = "Please Choose";
        }
        else
        {
            ZedComboBoxDevEx.Text = "";
        }
    }
    /*CH09:added (start)*/
    public Int16 ActiveMode
    {
        get
        {
            return _intActiveMode;
        }
        set
        {
            _intActiveMode = value;
        }
    }
    /*CH09:added (end)*/
    private bool _enabled;

    public bool Enabled
    {
        get
        {
            return _enabled;
        }
        set
        {
            _enabled = value;
            if (IsSingleSelect() == false)
                ddlSC.Enabled = _enabled;
            else
                ZedComboBoxDevEx.Enabled = _enabled;
        }
    }


    public int ItemCount
    {
        get
        {
            if (IsSingleSelect() == false)
            {
                ASPxListBox objlst = (ASPxListBox)ddlSC.FindControl("lstSC");
                if (objlst != null)
                {
                    return objlst.Items.Count;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return ZedComboBoxDevEx.Items.Count;
            }

        }
    }



    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {

            /* #cc05  added start*/
            if (EntityID == 0)
            {
                //EntityID = ZedEBS.Pagebase.EntityID;
                EntityID = BussinessLogic.PageBase.SalesChanelID;
            }
            if (EntityTypeID == 0)
            {
                //EntityTypeID = ZedEBS.Pagebase.EntityTypeID;
                EntityTypeID = BussinessLogic.PageBase.SalesChanelTypeID;
            }
            /* #cc05  added END*/
            //  RequiredFieldValidator1.Enabled = _blIsRequired;
            //  RequiredFieldValidator1.ValidationGroup = _strValidationGroup;
            //  RequiredFieldValidator1.ErrorMessage = _strErrorMessage;



            reqddlSC.ControlToValidate = IsSingleSelect() ? ZedComboBoxDevEx.ID : ddlSC.ID;

            ASPxListBox Lst = (ASPxListBox)ddlSC.FindControl("lstSC");
           
            if (IsSingleSelect())
            {
                ZedComboBoxDevEx.AutoPostBack = Convert.ToBoolean(_blAutoPostBack);
                bool isNumeric;
                int i;
                isNumeric = int.TryParse(ZedComboBoxDevEx.Value != null ? ZedComboBoxDevEx.Value.ToString() : "", out i);
                _long_SelectedValue = isNumeric ? i : 0;
            }
            else
            {
                if (Lst != null)
                {

                    Lst.SelectionMode = (ListEditSelectionMode)SelectionMode;
                    Lst.ClientSideEvents.SelectedIndexChanged = "function(s,e){ItemSelected(s,e," + ddlSC.ClientID + ");}";
                    if (Convert.ToBoolean(_blAutoPostBack))
                    {
                        Lst.ClientSideEvents.SelectedIndexChanged = "function(s,e){FirePostBack(s,e," + ddlSC.ClientID + ");}";
                        Lst.ClientSideEvents.ValueChanged = "function(s,e){FirePostBack(s,e," + ddlSC.ClientID + ");}";


                    }

                    if (Lst.SelectionMode == ListEditSelectionMode.Single)
                    {
                        if (Lst.SelectedItem != null)
                        {
                            _long_SelectedValue = Convert.ToInt32(Lst.SelectedItem.Value);
                        }
                    }
                }

                if (Convert.ToBoolean(_blAutoPostBack))
                {
                    ZedComboBoxDevEx.AutoPostBack = Convert.ToBoolean(_blAutoPostBack);
                }

            }
            /*#CH04:Start - Added*/
            if (IsSingleSelect() == false)
            {
                if (reqddlSC != null)
                    reqddlSC.Enabled = _blIsRequired;
            }
            else
            {
                /*#CH04: Start - Added*/
                if (ZedComboBoxDevEx.Text != _strInitialValue)
                {
                    if (ZedComboBoxDevEx.Text == "Please Select")
                        ZedComboBoxDevEx.Text = _strInitialValue;
                }
                /*#CH04: End - Added*/
              //  ZedComboBoxDevEx.Text = _strInitialValue;
                reqddlSC.Enabled = _blIsRequired;
                ((DevExpress.Web.ASPxEditors.ValidationSettings)((ASPxEdit)ZedComboBoxDevEx).ValidationSettings).ErrorDisplayMode = ErrorDisplayMode.Text;
                ((RequiredFieldValidationPattern)((DevExpress.Web.ASPxEditors.ValidationSettings)((ASPxEdit)ZedComboBoxDevEx).ValidationSettings).RequiredField).IsRequired = _blIsRequired;
                ((DevExpress.Web.ASPxEditors.ValidationSettings)ZedComboBoxDevEx.ValidationSettings).ValidationGroup = _strValidationGroup;

            }
            /*CH04:End - Added*/

            if (!IsPostBack)
            {
                /* ScriptManager.GetCurrent(this.Page).Scripts.Add(new ScriptReference("/zedebs/Assets/Scripts/DevExValidationCampatibility.js"));*/
                /* #ch03 added */
                /* #CC10 Commenetd*/
                //ScriptManager.GetCurrent(this.Page).Scripts.Add(new ScriptReference(ZedEBS.Pagebase.SiteUrl + "Assets/Scripts/DevExValidationCampatibility.js")); /* #CC10 Added */
                ScriptManager.GetCurrent(this.Page).Scripts.Add(new ScriptReference(PageBase.siteURL + "Assets/jscript/DevExValidationCampatibility.js")); /* #CC10 Added */
                if ((ListEditSelectionMode)SelectionMode == ListEditSelectionMode.Single)
                {
                    ddlSC.Visible = false;
                    ZedComboBoxDevEx.Visible = true;
                    ZedComboBoxDevEx.IncrementalFilteringMode = (IncrementalFilteringMode)_IncrementalFilteringMode;
                    ((RequiredFieldValidationPattern)((DevExpress.Web.ASPxEditors.ValidationSettings)((ASPxEdit)ZedComboBoxDevEx).ValidationSettings).RequiredField).ErrorText = _strErrorMessage;
         
                }
                else
                {
                    ddlSC.Text = _strInitialValue;
                    
                    ddlSC.Visible = true;
                    ZedComboBoxDevEx.Visible = false;
                }

                BindServiceCentre();

            }
        }
        catch (Exception ex)
        { 
        }
    }

    public enum AutoPostPack
    {
        True = 1,
        False = 0
    }
    

   /* protected void Page_PreRender()
    {
        //try
        //{
        //    if (!IsPostBack)
        //    {
        //        BindServiceCentre();
        //    }
            
        //}
        //catch (Exception ex)
        //{
        
        //}
    }*/

    public string SelectedText
    {
        get
        {
            if (IsSingleSelect() == false)
                return ddlSC.Text;
            else
                return ZedComboBoxDevEx.Text;
        }

        set
        {
            if (IsSingleSelect() == false)
                ddlSC.Text = value;
            else
                ZedComboBoxDevEx.SelectedItem.Text = value;
        }
    }



    private int int_LoggedInEntityID = 0;

    private Parent IsParent_IsParent = Parent.False;

    public Parent IsParent
    {
        get
        {
            return IsParent_IsParent;
        }
        set
        {
            IsParent_IsParent = value;
        }
    }

    public int LoggedInEntityID { get { return int_LoggedInEntityID; } set { int_LoggedInEntityID = value; } }
    

    public EntityTypeList bool_IsEntityTypeCollection = 0;
   
    public EntityTypeList IsEntityTypeList
    {
        get
        {
            return (EntityTypeList)bool_IsEntityTypeCollection;
        }
        set
        {
            bool_IsEntityTypeCollection = value;
        }
    }

    public EntityType Type
    {

        get {
            return EntityType_entityType;
        }
        set
        {
            EntityType_entityType = value;
        }
    
    }

    public String BusinessEventKeyword
    {
        get
        {
            return buis_keyword;
        }
        set
        {
            buis_keyword = value; 
        }
    }



    private ZedAxis.ZedEBS.Enums.EntityTypeKeyword _EntityTypeKeyword = ZedAxis.ZedEBS.Enums.EntityTypeKeyword.ORGTREE;

  
        
        //

 
    public ZedAxis.ZedEBS.Enums.EntityTypeKeyword Keyword
    {

        get
        {
            return _EntityTypeKeyword;
        }
        set
        {
            _EntityTypeKeyword = value;

        }

    }


    public bool IsEnabled
    {
        get
        {
            if (IsSingleSelect() == false)
                return ddlSC.Enabled;
            else
                return ZedComboBoxDevEx.Enabled;
        }
    }

    public int EntityTypeDescription
    {

        get {

            return _byteEntityTypeDescription;
        }
        set
        {
            _byteEntityTypeDescription = value;
        }
    }

    bool IsSingleSelect()
    {
        return (ListEditSelectionMode)SelectionMode == ListEditSelectionMode.Single;
    }

    bool _boolIsParentWithSingleItem = false;

    public bool IsParentWithSingleItem 
    {
        set {

            _boolIsParentWithSingleItem = value;
        
        }
        get
        {
            return _boolIsParentWithSingleItem;
        }
    
    }
    

    public void BindServiceCentre()
    {
        //#cc07 added
        string _strHeadingText = string.Empty;
        if (IsRequired)
        { _strHeadingText = "Please Select"; }
        else
        { _strHeadingText = "Select All"; }


        //#cc07 added end

        if (Convert.ToBoolean(IsParent_IsParent))
        {

            using (UC_EntityList objClsReport = new UC_EntityList())
            {

                DataTable objdt = new DataTable();
                objClsReport.EntityID = int_LoggedInEntityID;
                objClsReport.ForceTypeMatching = _ForceTypeMatching;
                //objClsReport.LoginEntityRequired = _LoginEntityRequired; /*#CH10:added*/
                objdt = objClsReport.GetEntityAccessWise(Convert.ToBoolean(IsParent_IsParent), _int_ParentSelectedValue, Convert.ToByte(EntityType_entityType), _EntityTypeKeyword.ToString(), buis_keyword, _byteEntityTypeDescription, _intActiveMode);/*#CC12:added Active Mode*/
                ASPxListBox Lst = (ASPxListBox)ddlSC.FindControl("lstSC");
                Lst.ClientSideEvents.SelectedIndexChanged = "function(s,e){ItemSelected(s,e," + ddlSC.ClientID + ");}";

                if (objdt != null && objdt.Rows.Count > 0)
                {

                    if (IsSingleSelect() == false)
                    {

                        Lst.DataSource = objdt;
                        Lst.DataBind();

                        /*(Start:#CH07 - Added)*/
                        if (SetSelectedValue > 0)
                        {
                            foreach (ListEditItem lstitem in Lst.Items)
                            {
                                if (lstitem.Value.ToString() == Convert.ToString(SetSelectedValue))
                                {
                                    Lst.SelectedIndex = Lst.Items.IndexOf(lstitem);
                                    ddlSC.Text = lstitem.Text;
                                    _long_SelectedValue = SetSelectedValue;
                                    ViewState["EntityName"] = lstitem.Text;
                                }

                            }
                        }
                        /*(End:#CH07 - Added)*/
                        if (objdt.Rows.Count == 1)
                        {
                            ddlSC.Enabled = false;
                            ddlSC.Text = objdt.Rows[0]["CompanyDisplayName"].ToString();
                            _long_SelectedValue = Convert.ToInt32(objdt.Rows[0]["EntityID"].ToString());
                            ViewState["EntityName"] = objdt.Rows[0]["CompanyDisplayName"].ToString();

                        }
                        else
                        {
                            ddlSC.Enabled = true;

                        }
                    }
                    else
                    {
                        ZedComboBoxDevEx.DataSource = objdt;
                        ZedComboBoxDevEx.DataBind();


                        /*(Start:#CH07 - Added)*/
                        if (SetSelectedValue > 0)
                        {
                            foreach (ListEditItem lstitem in ZedComboBoxDevEx.Items)
                            {
                                if (lstitem.Value.ToString() == Convert.ToString(SetSelectedValue))
                                {
                                    ZedComboBoxDevEx.SelectedIndex = ZedComboBoxDevEx.Items.IndexOf(lstitem);
                                    _long_SelectedValue = SetSelectedValue;
                                    ViewState["EntityName"] = lstitem.Text;
                                }

                            }
                        }
                        /*(End:#CH07 - Added)*/
                    }

                    if (IsSingleSelect() && objdt.Rows.Count == 1)
                    {
                        ZedComboBoxDevEx.SelectedIndex = 1;
                    }
                    //(Start:#CH06 - if condition added)
                    try
                    {
                        int.TryParse(ZedComboBoxDevEx.Value.ToString(), out _long_SelectedValue);
                    }
                    catch (Exception ex)
                    {
                    }
                    //(End:#CH06 - if condition added)
                    //_long_SelectedValue = Convert.ToInt32(ZedComboBoxDevEx.Value);    //#CH06: Commented
                    IsParentWithSingleItem = objdt.Rows.Count == 1 && IsSingleSelect();


                }
            }
        }
    }



    public void BindChild()
    {
        //#cc07 added
        string _strHeadingText = string.Empty;
        if (IsRequired)
        { _strHeadingText = "Please Select"; }
        else
        { _strHeadingText = "Select All"; }


        //#cc07 added end


        using (UC_EntityList objClsReport = new UC_EntityList())
        {

            DataTable objdt = new DataTable();
            objClsReport.EntityID = int_LoggedInEntityID;
            objClsReport.ForceTypeMatching = _ForceTypeMatching;
            //objClsReport.LoginEntityRequired = _LoginEntityRequired; /*#CH10:added*/
            objdt = objClsReport.GetEntityAccessWise(Convert.ToBoolean(IsParent_IsParent), _int_ParentSelectedValue, Convert.ToByte(EntityType_entityType), _EntityTypeKeyword.ToString(), buis_keyword, _byteEntityTypeDescription,_intActiveMode);/*#Ch09:added Active Mode*/
            ASPxListBox Lst = (ASPxListBox)ddlSC.FindControl("lstSC");
            Lst.ClientSideEvents.SelectedIndexChanged = "function(s,e){ItemSelected(s,e," + ddlSC.ClientID + ");}";


            if (objdt != null && objdt.Rows.Count > 0)
            {

                if (IsSingleSelect() == false)
                {

                    Lst.DataSource = objdt;
                    Lst.DataBind();
                    if (objdt.Rows.Count == 1)
                    {
                        ddlSC.Enabled = false;
                        ddlSC.Text = objdt.Rows[0]["CompanyDisplayName"].ToString();
                        _long_SelectedValue = Convert.ToInt32(objdt.Rows[0]["EntityID"].ToString());
                        ViewState["EntityName"] = objdt.Rows[0]["CompanyDisplayName"].ToString();

                    }
                    else
                    {
                        ddlSC.Enabled = true;
                    }
                }
                else
                {
                    ZedComboBoxDevEx.DataSource = objdt;
                    ZedComboBoxDevEx.DataBind();
                }

                if (IsSingleSelect() && objdt.Rows.Count == 1)
                {
                    ZedComboBoxDevEx.SelectedIndex = 1;

                }
                else
                {
                    ZedComboBoxDevEx.Text = "";
                }
                _long_SelectedValue = Convert.ToInt32(ZedComboBoxDevEx.Value);
                _boolIsParentWithSingleItem = objdt.Rows.Count == 1 && IsSingleSelect();


            }
            
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

        if (SelectedEntity != null)
        {
            if (IsSingleSelect() == false)
            {
                ASPxListBox Lst = (ASPxListBox)ddlSC.FindControl("lstSC");

                int _intEntityID = 0;
                bool isNumeric;
                int i;
                isNumeric = int.TryParse(Lst.SelectedItem.Value != null ? Lst.SelectedItem.Value.ToString() : "", out i);
                _intEntityID = isNumeric ? i : 0;
                SelectedEntity(_intEntityID);
            }
            else
            {
                int _intEntityID = 0;
                bool isNumeric;
                int i;
                isNumeric = int.TryParse(ZedComboBoxDevEx.Value != null ? ZedComboBoxDevEx.Value.ToString() : "", out i);
                _intEntityID = isNumeric ? i : 0;
                SelectedEntity(_intEntityID);
            }
        }

    }
    protected void ZedComboBoxDevEx_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (SelectedEntity != null)
        {
            int _intEntityID = 0;
            bool isNumeric;
            int i;
            isNumeric = int.TryParse(ZedComboBoxDevEx.Value != null ? ZedComboBoxDevEx.Value.ToString() : "", out i);
            _intEntityID = isNumeric ? i : 0;
            SelectedEntity(_intEntityID);
        }

    }
}