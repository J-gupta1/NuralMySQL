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
 *21-Mar-2013, Prashant Chitransh, #Ch06: Changes to implement Multiselect parent functionality.
 *15-may-2014, Shilpi Sharma, #Ch07: Code commented as when only one record in from then it is giving error and not binding to
 *15-may-2014, Shilpi Sharma, #Ch07: Code commented as when only one record in from then it is giving error and not binding to
 *07-Jul-2015, Vijay Katiyar, #Ch08: Change DevExpress.Web.ValidationSettings to DevExpress.Web.ASPxEditors.ValidationSettings
  26-may-2016, vivek kumar, #Ch09: added condition if ZedComboBoxDevEx.Value is please select then skip else block condition..
 *  08-Jun-2017, Sumit Maurya, #CC10, Path corrected (done for firstdata).
 *  06-July-2017, Kalpana, #CC11: hardcoded style removed and applied responsive css
* ==================================================================================================== * 
*/
#endregion

using System;
using System.Data;
using System.ComponentModel;
using DevExpress.Web;
using ZedAxis.ZedEBS;
using DevExpress.Web.ASPxEditors;/* #Ch08 Added */
using ZedAxis.ZedEBS.Enums;
using System.Web.UI;
using BussinessLogic;

public partial class ucEntityList_ver2 : System.Web.UI.UserControl
{
    #region Delegate & Events
    public delegate void DelegateSelectedEntity(Int32 EntityID); /* #Ch02: added */
    public event DelegateSelectedEntity SelectedEntity;          /* #Ch02: added */

    public delegate void DelegateAllCheckedEntities(DataTable dtEntities);  /* #Ch06: added */
    public event DelegateAllCheckedEntities CheckedEntities;                /* #Ch06: added */
    #endregion

    #region Class Enum
    public enum EnumIncrementalFilteringMode
    {
        Contains = 0,
        StartsWith = 1,
        None = 2,
    }

    public enum EntityType
    {
        PRIMARY = 1,
        SECONDARY = 2
    }

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

    public enum AutoPostPack
    {
        True = 1,
        False = 0
    }
    #endregion

    #region Private Class Variable
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
    private bool _enabled;
    private int int_LoggedInEntityID = 0;
    private Parent IsParent_IsParent = Parent.False;
    public EntityTypeList bool_IsEntityTypeCollection = 0;
    private ZedAxis.ZedEBS.Enums.EntityTypeKeyword _EntityTypeKeyword = ZedAxis.ZedEBS.Enums.EntityTypeKeyword.ORGTREE;
    bool _boolIsParentWithSingleItem = false;
    public EntityType EntityType_entityType = EntityType.SECONDARY;
    public string buis_keyword;
    private DataTable _dtCheckedEntityList;     // #Ch06: added.
    private Int16 _intCallingMode;              // #Ch06: added.

    private Int16 _ForceTypeMatching = 0;
    #endregion

    #region Public Properties
    [DefaultValue(true)]
    public EnumIncrementalFilteringMode IncrementalFilteringMode
    {
        set { _IncrementalFilteringMode = value; }
    }

    // #Ch06: property created.
    public DataTable CheckedEntityList
    {
        get { return _dtCheckedEntityList; }
        set { _dtCheckedEntityList = value; }
    }

    // #Ch06: property created.
    public Int16 CallingMode
    {
        get { return _intCallingMode; }
        set { _intCallingMode = value; }
    }

    public AutoPostPack AutoPostBack
    {
        get { return _blAutoPostBack; }
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
        get
        {
            return _int_ParentSelectedValue;
        }
        set
        {
            _int_ParentSelectedValue = value;
        }
    }

    public long SelectedValue
    {
        get { return _long_SelectedValue; }
        set
        {
            if (IsSingleSelect())
                ZedComboBoxDevEx.Value = value;
        }
    }
    public Int16 ForceTypeMatching
    {
        get { return _ForceTypeMatching; }
        set { _ForceTypeMatching = value; }
    }
    //public DataTable CheckedValues
    //{
    //    get
    //    {
    //        if (!IsSingleSelect())
    //            return returnCheckedEntities();
    //        else
    //            return CreateTable();
    //    }
    //}

    public string SetInitialValue
    {
        get { return _strInitialValue; }
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
                /* #Ch08 Added (start) */
                ((DevExpress.Web.ASPxEditors.ValidationSettings)((ASPxEdit)ZedComboBoxDevEx).ValidationSettings).ErrorDisplayMode = ErrorDisplayMode.Text;
                ((RequiredFieldValidationPattern)((DevExpress.Web.ASPxEditors.ValidationSettings)((ASPxEdit)ZedComboBoxDevEx).ValidationSettings).RequiredField).IsRequired = _blIsRequired;
                /* #Ch08 Added (End) */
                /* #Ch08 Commented (start) */
                //((DevExpress.Web.ValidationSettings)((ASPxEdit)ZedComboBoxDevEx).ValidationSettings).ErrorDisplayMode = ErrorDisplayMode.Text;
                //((RequiredFieldValidationPattern)((DevExpress.Web.ValidationSettings)((ASPxEdit)ZedComboBoxDevEx).ValidationSettings).RequiredField).IsRequired = _blIsRequired;
                /* #Ch08 Commented (End) */
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
    /// 

    // #Ch06: property created.
    public DataTable GetAllValues
    {
        get
        {
            if (!IsSingleSelect())
                return returnAllEntities();
            else
                return CreateTable();
        }
    }

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
                    DataColumn dcType = new DataColumn("SecondaryEntityTypeId", typeof(int));
                    dt.Columns.Add(dcType);
                    if (IsSingleSelect() == false)
                    {
                        ASPxListBox objlst = (ASPxListBox)ddlSC.FindControl("lstSC");
                        foreach (ListEditItem lstItm in objlst.Items)
                        {
                            DataRow dr = dt.NewRow();
                            if (lstItm.Selected)
                            {
                                dr["SecondaryEntityID"] = lstItm.Value.ToString().Split('-')[0];
                                dr["SecondaryEntityTypeId"] = lstItm.Value.ToString().Split('-')[1];
                                dt.Rows.Add(dr);
                            }

                            dt.AcceptChanges();
                        }
                    }
                    else
                    {
                        /*#Ch09:start(add)*/
                        if ( !ZedComboBoxDevEx.Value.ToString().ToLower().Contains("please select."))
                        {
                            /*#Ch09:start(end)*/
                        DataRow dr = dt.NewRow();
                        dr["SecondaryEntityID"] = ZedComboBoxDevEx.Value;
                        dt.Rows.Add(dr);
                        dt.AcceptChanges();

                        }/*#Ch09:added*/

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

    //#CC04 added
    public Int16 ShowBehaviour
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

    public int LoggedInEntityID
    {
        get { return int_LoggedInEntityID; }
        set { int_LoggedInEntityID = value; }
    }

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

        get
        {
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

        get
        {

            return _byteEntityTypeDescription;
        }
        set
        {
            _byteEntityTypeDescription = value;
        }
    }

    public bool IsParentWithSingleItem
    {
        set
        {

            _boolIsParentWithSingleItem = value;

        }
        get
        {
            return _boolIsParentWithSingleItem;
        }

    }
    #endregion

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            /* #cc05  added start*/
            if (EntityTypeID == 0)
            {
                EntityTypeID = PageBase.EntityTypeID;
            }
            //if (EntityID == 0)
            //{
                
            //    EntityID = PageBase.EntityID;
            //}
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

                /* #Ch08 Added (Start) */
                ((DevExpress.Web.ASPxEditors.ValidationSettings)((ASPxEdit)ZedComboBoxDevEx).ValidationSettings).ErrorDisplayMode = ErrorDisplayMode.Text;
                ((RequiredFieldValidationPattern)((DevExpress.Web.ASPxEditors.ValidationSettings)((ASPxEdit)ZedComboBoxDevEx).ValidationSettings).RequiredField).IsRequired = _blIsRequired;
                ((DevExpress.Web.ASPxEditors.ValidationSettings)ZedComboBoxDevEx.ValidationSettings).ValidationGroup = _strValidationGroup;
                /* #Ch08 Added (End) */
                /* #Ch08 Commented (start) */
                /*((DevExpress.Web.ValidationSettings)((ASPxEdit)ZedComboBoxDevEx).ValidationSettings).ErrorDisplayMode = ErrorDisplayMode.Text;
                ((RequiredFieldValidationPattern)((DevExpress.Web.ValidationSettings)((ASPxEdit)ZedComboBoxDevEx).ValidationSettings).RequiredField).IsRequired = _blIsRequired;
                ((DevExpress.Web.ValidationSettings)ZedComboBoxDevEx.ValidationSettings).ValidationGroup = _strValidationGroup;
                */
                 /* #Ch08 Commented (End) */

            }
            /*CH04:End - Added*/

            if (!IsPostBack)
            {
                /* ScriptManager.GetCurrent(this.Page).Scripts.Add(new ScriptReference("/zedebs/Assets/Scripts/DevExValidationCampatibility.js")); #CC10 Commented */ /* #ch03 added */
                ScriptManager.GetCurrent(this.Page).Scripts.Add(new ScriptReference(PageBase.siteURL + "/Assets/jscripts/DevExValidationCampatibility.js")); /* #CC10 Added */
                if ((ListEditSelectionMode)SelectionMode == ListEditSelectionMode.Single)
                {
                    ddlSC.Visible = false;
                    ZedComboBoxDevEx.Visible = true;
                    ZedComboBoxDevEx.IncrementalFilteringMode = (IncrementalFilteringMode)_IncrementalFilteringMode;
                    //((RequiredFieldValidationPattern)((DevExpress.Web.ValidationSettings)((ASPxEdit)ZedComboBoxDevEx).ValidationSettings).RequiredField).ErrorText = _strErrorMessage;//#Ch08 Commented
                    ((RequiredFieldValidationPattern)((DevExpress.Web.ASPxEditors.ValidationSettings)((ASPxEdit)ZedComboBoxDevEx).ValidationSettings).RequiredField).ErrorText = _strErrorMessage;//#Ch08 Added
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

    /*protected void Page_PreRender()
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

    // #Ch06: Event created.
    protected void ddlSC_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (CheckedEntities != null)
        {
            _dtCheckedEntityList = GetSelectedValues;
            CheckedEntities(_dtCheckedEntityList);
        }
    }
    #endregion

    #region Methods
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

    bool IsSingleSelect()
    {
        return (ListEditSelectionMode)SelectionMode == ListEditSelectionMode.Single;
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
                if (!IsSingleSelect())
                {
                    objClsReport.SelectionMode = 2;
                    objClsReport.EntityList = GetSelectedValues;
                }
                else
                    objClsReport.EntityList = GetSelectedValues;/*#Ch09:added*/
                    objClsReport.SelectionMode = 1;
                objClsReport.ForceTypeMatching = 1;
                objdt = objClsReport.GetEntityAccessWise_ver2(Convert.ToBoolean(IsParent_IsParent), _int_ParentSelectedValue, Convert.ToByte(EntityType_entityType), _EntityTypeKeyword.ToString(), buis_keyword, _byteEntityTypeDescription);
                ASPxListBox Lst = (ASPxListBox)ddlSC.FindControl("lstSC");
                Lst.ClientSideEvents.SelectedIndexChanged = "function(s,e){ItemSelected(s,e," + ddlSC.ClientID + ");}";

                if (objdt != null && objdt.Rows.Count > 0)
                {
                    if (IsSingleSelect() == false)
                    {
                        Lst.DataSource = objdt;
                        Lst.DataBind();
                        ddlSC.Enabled = true;
                        /*Ch07:commnted (start)*/
                        //if (objdt.Rows.Count == 1)
                        //{
                        //    ddlSC.Enabled = false;
                        //    ddlSC.Text = objdt.Rows[0]["CompanyDisplayName"].ToString();
                        //    _long_SelectedValue = Convert.ToInt32(objdt.Rows[0]["EntityID"].ToString());

                        //    ViewState["EntityName"] = objdt.Rows[0]["CompanyDisplayName"].ToString();                            
                        //}
                        //else
                        //{
                        //    ddlSC.Enabled = true;
                        //}
                        /*Ch07:commnted (end)*/
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
                    _long_SelectedValue = Convert.ToInt32(ZedComboBoxDevEx.Value);
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
            objClsReport.SelectionMode = _intCallingMode;
            objClsReport.EntityList = CheckedEntityList;
            //if (CheckedEntityList.Rows.Count > 0)
            //    objClsReport.EntityList = CheckedEntityList; //returnCheckedEntities();
            //else
            //    objClsReport.EntityList = CreateTable();

            objdt = objClsReport.GetEntityAccessWise_ver2(Convert.ToBoolean(IsParent_IsParent), _int_ParentSelectedValue, Convert.ToByte(EntityType_entityType), _EntityTypeKeyword.ToString(), buis_keyword, _byteEntityTypeDescription);
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
                        //ddlSC.Enabled = false;        // #Ch06: commented.
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

    // #Ch06: Method created.
    private DataTable CreateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("EntityID", typeof(int));
        dt.Columns.Add("EntityTypeID", typeof(int));
        dt.AcceptChanges();
        return dt;
    }

    // #Ch06: Method created.
    private DataTable returnAllEntities()
    {
        DataTable dtList = CreateTable();
        ASPxListBox lstSC = (ASPxListBox)ddlSC.FindControl("lstSC");
        foreach (ListEditItem item in lstSC.Items)
        {
            string EntityIDTypeId = Convert.ToString(item.Value);
            DataRow dr = dtList.NewRow(); //new DataRow();
            dr["EntityID"] = Convert.ToInt32(EntityIDTypeId.Split('-')[0]);
            dr["EntityTypeID"] = Convert.ToInt32(EntityIDTypeId.Split('-')[1]);
            dtList.Rows.Add(dr);
        }
        return dtList;
    }
    #endregion
}