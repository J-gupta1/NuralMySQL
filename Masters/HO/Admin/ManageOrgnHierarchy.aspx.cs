﻿/*
====================================================================================================================================
 *Change Log: 
 *DD-MMM-YYYY, Name , #CCXX - Description
------------------------------------------------------------------------------------------------------------------------------------
 * 22-Mar-2016, Sumit Maurya, #CC01, Issue of page getting blocked on the execution of event(s) due to update panel resolved.
 * 16-Aug-2016, Sumit Maurya, #CC02, Parent dropdown is enabled as per client requirement.
 * 23-Aug-2016, Sumit Maurya, #CC03, Paging implemented and new search parameter suplied to filter data. Parent hierarchy Level dropdown commented as it is not required for search parameter.
 * 25-May-2018, Rajnish Kumar, #CC04, You can't de-activate by change the status by update but when current organisation hierarchy is mapped with sales channel or other organisation hierarchy
 * 29-May-2018,Vijay Kumar Prajapati,#CC05,Change GetAllHierarchyLevel() to GetHierarchyLevelConditional() for karbon mobile.
 * 26-Nov-2018,Vijay Kumar Prajapati,#CC06,Add WebUserId in Save,update,Edit and StatusChange.
 * 27-March-2020,Vijay Kumar Prajapati,#CC07,Create new panel for upload orghierarchy and its use in single panel.
====================================================================================================================================
 */
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Data;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Resources;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;
using System.Collections;
using System.Collections.Generic;
using ExportExcelOpenXML;
using ZedService;
using BusinessLogics;
using System.Data.SqlClient;
using System.Security.Cryptography;
/*
 * 18 Mar 2015, Karam Chand Sharma, #CC01 , User Name also export in Export in Excel "LocationUsername"
 */
public partial class Masters_HO_Admin_ManageOrgnHierarchy : PageBase
{
    DataTable dtOrg = new DataTable();
    /*#CC07 added Started*/
    DataTable dtNew = new DataTable();
    object objSum;
    int counter = 0;
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    UploadFile UploadFile = new UploadFile();
    List<String> lstDuplicate = new List<String>();
    DataSet dsErrorProne = new DataSet();
    string strPrimarySessionName = "OrgnhierarchylUploadSession";
    string strPrimarySessionNameupdate = "OrgnhierarchyUploadSessionUpdate";
    string strPSalt, strPassword;
    public static string strDefaultPassword = ConfigurationManager.AppSettings["DefaultPassword"].ToString();
    /*#CC07 added End*/
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            ucMsg.Visible = false;
            // updMsg.Update();
            //  updgrid.Update();

            if (!IsPostBack)
            {
                chkActive.Checked = true;
                if (ViewState["EditUserId"] != null)
                { ViewState.Remove("EditUserId"); }
                BindHierarchy();
                /*FillLocationGrid(); #CC03 Commented */
                /* #CC03 Add Start */
                FillLocationGrid(1);
                BindState();
                ViewState["CurrentPage"] = 1;
                /* #CC03 Add End */
                BindParentHierarchy();
                /*#CC07 added Started*/
                if (Rbtdownloadtemplate.SelectedValue == "1")
                {
                    ForSaveTemplateheading.Visible = true;
                    ForSaveTemplatedownload.Visible = true;
                    ReferenceIdForsaveheading.Visible = true;
                    ReferenceIdForsave.Visible = true;
                }
                else
                {
                    ForSaveTemplateheading.Visible = false;
                    ForSaveTemplatedownload.Visible = false;
                    ReferenceIdForsaveheading.Visible = false;
                    ReferenceIdForsave.Visible = false;
                }
                /*#CC07 added End*/
            }


        }
        catch (Exception ex)
        {

            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }

    }
    private void BindHierarchy()
    {
        try
        {
            DataTable dt = new DataTable();
            ddlHierarchyLevel.Items.Clear();
            ddlSerHierarchyLevel.Items.Clear();
            using (OrgHierarchyData objuser = new OrgHierarchyData())
            {

                // dt = objuser.GetAllHierarchyLevel();/*#CC05 Commented*/
                objuser.CompanyId = PageBase.ClientId;
                dt = objuser.GetHierarchyLevelConditional(2);/*#CC05 Added*/
            };
            String[] colArray = { "HierarchyLevelID", "HierarchyLevelName" };
            PageBase.DropdownBinding(ref ddlHierarchyLevel, dt, colArray);
            PageBase.DropdownBinding(ref ddlSerHierarchyLevel, dt, colArray);   //Pankaj Dhingra
            /* PageBase.DropdownBinding(ref ddlSerParentHierarchyLevel, dt, colArray); //Pankaj Dhingra #CC03 Commented */

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    private void BindState()
    {
        try
        {
            DataTable dt = new DataTable();
            ddlState.Items.Clear();
            using (OrgHierarchyData objuser = new OrgHierarchyData())
            {
                objuser.CompanyId = PageBase.ClientId;
                dt = objuser.GetStateCityList(0);
            };
            String[] colArray = { "StateID", "StateName" };
            PageBase.DropdownBinding(ref ddlState, dt, colArray);
            ddlCity.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

    private void BindCity()
    {

    }
    private void BindParentHierarchy()
    {
        try
        {
            DataTable dt = new DataTable();
            ddlParentHierarchy.Items.Clear();
            using (OrgHierarchyData objOrg = new OrgHierarchyData())
            {
                objOrg.HierarchyLevelID = Convert.ToInt16(ddlHierarchyLevel.SelectedValue);
                objOrg.CompanyId = PageBase.ClientId;
                dt = objOrg.GetAllHierarchyLevelLocation();
            };
            String[] colArray = { "OrgnhierarchyID", "LocationName" };
            PageBase.DropdownBinding(ref ddlParentHierarchy, dt, colArray);
            ddlParentHierarchy.Enabled = dt.Rows.Count == 0 ? false : true;     //Pankaj Dhingra


        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void ddlHierarchyLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindParentHierarchy();
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        string MappingMessage = "";/*#CC06 Added*/
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }
            if (ddlHierarchyLevel.SelectedIndex == 0 || txtLocationCode.Text.Trim().Length == 0 || txtLocationName.Text.Trim().Length == 0)
            {
                ucMsg.ShowWarning(Resources.Messages.MandatoryField);

                return;
            }

            if (ddlState.SelectedValue != "0")
            {
                if (ddlCity.SelectedValue == "0")
                {
                    ucMsg.ShowWarning("Please Select City.");
                    return;
                }
            }
            //if (ddlHierarchyLevel.SelectedValue != "1")
            //{

            //    if (ddlParentHierarchy.SelectedIndex == 0)
            //    {
            //        ucMsg.ShowInfo("Please select parent hierarchy name ");
            //        return;
            //    }
            //}

            if (Convert.ToInt16(ddlHierarchyLevel.SelectedValue) > 0 && txtLocationCode.Text.Trim().Length >= 0 && txtLocationCode.Text.Trim().Length > 0
                 )
            {
                Int32 result = 0;
                int CheckResult = 0;/*#CC04*/
                using (OrgHierarchyData objOrg = new OrgHierarchyData())
                {
                    if (Convert.ToInt32(ViewState["EditUserId"]) == 0)
                    {
                        objOrg.OrgnhierarchyID = 0;
                    }
                    else
                    {
                        objOrg.OrgnhierarchyID = Convert.ToInt32(ViewState["EditUserId"]);
                    }
                    objOrg.LocationName = txtLocationName.Text.Trim();
                    objOrg.LocationCode = txtLocationCode.Text.Trim();
                    objOrg.HierarchyLevelID = Convert.ToInt16(ddlHierarchyLevel.SelectedValue);
                    objOrg.ParentOrgnhierarchyID = Convert.ToInt32(ddlParentHierarchy.SelectedValue);
                    objOrg.Status = Convert.ToBoolean(chkActive.Checked);
                    CheckResult = objOrg.CheckStatusForExistLocation(); /*#CC04*/
                    objOrg.UserID = PageBase.UserId;/*#CC06 Added*/
                    objOrg.CompanyId = PageBase.ClientId;/*#CC07 Added*/
                                                         // objOrg.Lat = txtLatitude.Text.Trim();
                                                         //objOrg.Long = txtLongitude.Text.Trim();
                                                         //objOrg.GeoRadius = txtGeoFancingRadius.Text.Trim();
                    if (ddlState.SelectedValue != "0")
                    {
                        objOrg.StateID = Convert.ToInt32(ddlState.SelectedValue);
                    }
                    else
                    {
                        objOrg.StateID = 0;
                    }
                    if (ddlCity.SelectedValue != "0")
                    {
                        objOrg.CityID = Convert.ToInt32(ddlCity.SelectedValue);
                    }
                    else
                    {
                        objOrg.CityID = 0;
                    }
                    result = objOrg.InsertUpdateOrgnHierarchyinfo();
                    /*#CC04 start*/
                    if ((CheckResult > 0) && (objOrg.Status == false))
                    {
                        ucMsg.ShowError("This location is map with existing Location or Saleschannel.You can not deactivate it");
                        return;
                    }/*#CC04 end*/
                    if (objOrg.ReturnValue == 1)
                    {
                        ucMsg.ShowError(Resources.Messages.DuplicateLocationName);
                        return;
                    }
                    if (objOrg.ReturnValue == 2)
                    {
                        ucMsg.ShowError(objOrg.Error.ToString());
                        return;
                    }

                    MappingMessage = objOrg.Error;/*#CC06 Added*/
                };


                if (result > 0)
                {
                    if (ViewState["EditUserId"] == null)
                    {
                        ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    }
                    else
                    {
                        ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull + '-' + MappingMessage/*#CC06 Added*/);
                    }

                    ClearForm();
                    //  updAddUserMain.Update();
                    chkActive.Checked = true;
                    return;
                }
                else
                {
                    //ucLblMessage.ShowError(Resources.GlobalMessages.ErrorMsgTryAfterSometime);
                    return;
                }
            }
            updMsg.Update();

        }

        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
            updMsg.Update();
            PageBase.Errorhandling(ex);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtLocationCodeSearch.Text.Trim() != "" || txtLocationNameSearch.Text.Trim() != "" || ddlSerHierarchyLevel.SelectedIndex != 0 ||/* ddlSerParentHierarchyLevel.SelectedIndex != 0  #CC03 Commented ||*/ txtSerParentLocationName.Text != "" || txtParentCode.Text.Trim() != "" || txtUserName.Text.Trim() != "")/* #CC03 New search parameters added  */
        {
            /*  #CC03 Comment Start 
             if ((ddlSerParentHierarchyLevel.SelectedIndex != 0) &&  (txtSerParentLocationName.Text == ""))
{
  ucMsg.ShowInfo("Please Enter Parent Location Name.");
  return;
}
else if (/*(ddlSerParentHierarchyLevel.SelectedIndex == 0)  &&  (txtSerParentLocationName.Text != ""))
{
  ucMsg.ShowInfo("Please Enter Parent Hierarchy Level");
  return;
}
else
  FillLocationGrid(); #CC02 Comment End */
            FillLocationGrid(1);/* #CC02 Added */


        }
        else
            ucMsg.ShowInfo("Please Enter searching parameter(s).");
        return;

    }
    /* #CC03 Comment Start 
    private void FillLocationGrid()
    {
        //DataTable dtOrg;
        using (OrgHierarchyData objOrg = new OrgHierarchyData())
        {
            objOrg.LocationName = txtLocationNameSearch.Text.Trim();
            objOrg.LocationCode = txtLocationCodeSearch.Text.Trim();
            objOrg.HierarchyLevelID = Convert.ToInt16(ddlSerHierarchyLevel.SelectedValue);
            objOrg.ParentHierarchyLevelID = Convert.ToInt16(ddlSerParentHierarchyLevel.SelectedValue);
            objOrg.ParentLocationName = txtSerParentLocationName.Text.Trim();
            dtOrg = objOrg.GetOrgnHierarchyInfo();
        };
        if (dtOrg != null && dtOrg.Rows.Count > 0)
        {
            grdvLocationList.Visible = true;
            grdvLocationList.DataSource = dtOrg;
            //ViewState["Dtexport"] = dtOrg;
        }
        else
        {
            //ViewState["Dtexport"] = null;
            grdvLocationList.DataSource = null;
        }
        grdvLocationList.DataBind();
        updgrid.Update();
    }
    #CC03 Comment End 
    */
    void ClearForm()
    {
        ddlHierarchyLevel.SelectedIndex = 0;
        ddlParentHierarchy.SelectedIndex = 0;
        /*FillLocationGrid(); #CC02 Commented */
        BindParentHierarchy();
        ddlHierarchyLevel.Enabled = true;
        ddlParentHierarchy.Enabled = true;
        txtLocationName.Text = "";
        txtLocationCode.Text = "";
        txtLocationName.Text = "";
        txtLocationCodeSearch.Text = "";
        txtLocationNameSearch.Text = "";
        btnCreate.Text = "Submit";
        ViewState["EditUserId"] = null;
        ddlSerHierarchyLevel.SelectedIndex = 0; //Pankaj Dhingra
        //ViewState["Dtexport"] = null;
        /* ddlSerParentHierarchyLevel.SelectedIndex = 0; #CC03 Commented */
        txtSerParentLocationName.Text = "";
        /*FillLocationGrid(); #CC02 Commented */
        /* #CC02 Add Start */
        txtParentCode.Text = "";
        txtUserName.Text = "";
        FillLocationGrid(Convert.ToInt32(ViewState["CurrentPage"]));
        ddlState.SelectedValue = "0";
        if (ddlState.SelectedValue == "0")
        {
            ddlCity.Items.Clear();
            ddlCity.Items.Insert(0, new ListItem("Select", "0"));
        }

        /* #CC02 Add Start */
        // UpdSearch.Update();
    }
    //public void hideprogressbar()
    //{
    //    Control c = this.Master.FindControl("updateProgressDiv");
    //    c.Visible = false;

    //}
    protected void btnActiveDeactive_Click(object sender, ImageClickEventArgs e)
    {
        string Message = "";/*#CC06 Added*/
        try
        {
            ImageButton btnActiveDeactive = (ImageButton)sender;
            Int32 Result = 0;
            Int16 OrgnhierarchyID = Convert.ToInt16(btnActiveDeactive.CommandArgument);
            using (OrgHierarchyData ObjOrgn = new OrgHierarchyData())
            {
                ObjOrgn.OrgnhierarchyID = OrgnhierarchyID;
                ObjOrgn.UserID = PageBase.UserId;/*#CC06 Added*/
                Result = ObjOrgn.UpdateStatusOrgnHierarchyInfo();
                Message = ObjOrgn.Error;/*#CC06 Added*/
            };
            if (Result == 1)
            {
                ucMsg.ShowSuccess(Resources.Messages.StatusChanged + '-' + Message);
            }
            else
            {
                ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            }
            /*FillLocationGrid(); #CC02 Commented */
            FillLocationGrid(Convert.ToInt32(ViewState["CurrentPage"])); /* #CC02 Added */

            // updgrid.Update();
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        ddlHierarchyLevel.SelectedIndex = -1;
        ImageButton btnEdit = (ImageButton)sender;
        DataTable dtOrgn;
        using (OrgHierarchyData objOrgn = new OrgHierarchyData())
        {
            dtOrgn = objOrgn.GetOrgnHierarchyInfo(Convert.ToInt32(btnEdit.CommandArgument));
        };
        ViewState["EditUserId"] = Convert.ToInt32(btnEdit.CommandArgument);

        if (dtOrgn != null && dtOrgn.Rows.Count > 0)
        {
            if (ddlHierarchyLevel.Items.FindByValue(dtOrgn.Rows[0]["HierarchyLevelID"].ToString()) != null)
            {
                ddlHierarchyLevel.Items.FindByValue(dtOrgn.Rows[0]["HierarchyLevelID"].ToString()).Selected = true;
                ddlHierarchyLevel_SelectedIndexChanged(ddlHierarchyLevel, new EventArgs());
                ddlHierarchyLevel.Enabled = false;
            }
            if (ddlParentHierarchy.Items.FindByValue(dtOrgn.Rows[0]["ParentOrgnhierarchyID"].ToString()) != null)
            {
                ddlParentHierarchy.Items.FindByValue(dtOrgn.Rows[0]["ParentOrgnhierarchyID"].ToString()).Selected = true;
                /*ddlParentHierarchy.Enabled = false; #CC02 Commented*/
            }

            if (ddlState.Items.FindByValue(dtOrgn.Rows[0]["StateID"].ToString()) != null)
            {

                ddlState.SelectedValue = Convert.ToString(dtOrgn.Rows[0]["StateID"]);

                ddlState_SelectedIndexChanged(ddlState, new EventArgs());
                if (ddlCity.Items.FindByValue(dtOrgn.Rows[0]["CityID"].ToString()) != null)
                {
                    ddlCity.SelectedValue = Convert.ToString(dtOrgn.Rows[0]["CityID"]);
                }

            }

            txtLocationName.Text = (dtOrgn.Rows[0]["LocationName"].ToString());
            txtLocationCode.Text = (dtOrgn.Rows[0]["LocationCode"].ToString());
            chkActive.Checked = Convert.ToBoolean(dtOrgn.Rows[0]["Status"]);
            btnCreate.Text = "Update";
            // updAddUserMain.Update();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearForm();
    }

    #region code not required
    /*
    protected void grdvLocationList_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        grdvLocationList.PageIndex = e.NewPageIndex;
        FillLocationGrid();
    }
     * */
    #endregion code not required
    protected void btnShow_Click(object sender, EventArgs e)
    {
        ClearForm();
        FillLocationGrid(1);

    }
    protected void btnExprtToExcel_Click(object sender, EventArgs e)
    {
        try
        {

            //if (ViewState["Dtexport"] != null)
            //{
            //DataTable dt = (DataTable)ViewState["Dtexport"];
            /*FillLocationGrid(); #CC02 Commented */
            //Pankaj Dhingra
            FillLocationGrid(-1); /* #CC02 Added */
            DataTable dt = dtOrg.Copy();
            /*#CC01 COMMENTED string[] DsCol = new string[] { "LocationName", "LocationCode", "ParentLocationName", "ParentLocationCode", "HierarchyLevelName", "StatusText" };*/
            string[] DsCol = new string[] { "LocationName", "LocationCode", "ParentLocationName", "ParentLocationCode", "HierarchyLevelName", "LocationUsername", "StatusText", "StateName", "CityName" };/*#CC01 ADDED*/
            DataTable DsCopy = new DataTable();
            dt = dt.DefaultView.ToTable(true, DsCol);
            dt.Columns["StatusText"].ColumnName = "Status";
            if (dt.Rows.Count > 0)
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../../");
                string FilenameToexport = "LocationDetails";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dtcopy, FilenameToexport);

            }
            else
            {
                ucMsg.ShowInfo(Resources.Messages.NoRecord);
            }
            //ViewState["Dtexport"] = null;
            //}
            //else
            //{
            //    ucMsg.ShowInfo(Resources.Messages.NoRecord); 
            //}
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }

    }
    protected void grdvLocationList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int CheckResult = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Int32 OrgnhierarchyID = Convert.ToInt32(grdvLocationList.DataKeys[e.Row.RowIndex].Value);
            using (OrgHierarchyData ObjHierarchy = new OrgHierarchyData())
            {
                ObjHierarchy.OrgnhierarchyID = OrgnhierarchyID;
                CheckResult = ObjHierarchy.CheckStatusForExistLocation();
            };
            GridViewRow GVR = e.Row;
            ImageButton btnStatus = (ImageButton)GVR.FindControl("btnActiveDeactive");
            if (CheckResult > 0)
            {
                if (btnStatus != null)
                {
                    btnStatus.Attributes.Add("Onclick", "javascript:alert('This location is map with existing Location or Saleschannel.You can not deactivate it.');{return false;}");

                }

            }
        }
    }
    /* #CC03 Add Start */
    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        FillLocationGrid(ucPagingControl1.CurrentPage);
    }


    private void FillLocationGrid(int pageNo)
    {

        using (OrgHierarchyData objOrg = new OrgHierarchyData())
        {
            objOrg.PageIndex = pageNo;
            objOrg.PageSize = Convert.ToInt32(PageSize);
            objOrg.UserName = txtUserName.Text.Trim();

            objOrg.LocationName = txtLocationNameSearch.Text.Trim();
            objOrg.LocationCode = txtLocationCodeSearch.Text.Trim();
            objOrg.HierarchyLevelID = Convert.ToInt16(ddlSerHierarchyLevel.SelectedValue);
            objOrg.ParentCode = txtParentCode.Text.Trim();
            objOrg.ParentLocationName = txtSerParentLocationName.Text.Trim();
            objOrg.CompanyId = PageBase.ClientId;/*#CC07 Added*/
            dtOrg = objOrg.GetOrgnHierarchyInfo();

            if (dtOrg != null && dtOrg.Rows.Count > 0)
            {
                dvFooter.Visible = true;
                ViewState["TotalRecords"] = objOrg.TotalRecords;
                ucPagingControl1.TotalRecords = objOrg.TotalRecords;
                ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                ucPagingControl1.SetCurrentPage = pageNo;
                ucPagingControl1.FillPageInfo();

                //grdvLocationList.Visible = true;
                grdvLocationList.DataSource = dtOrg;
                grdvLocationList.DataBind();
                grdvLocationList.Visible = true;
                // updgrid.Update();

            }
            else
            {
                //viewState["Dtexport"] = null;
                ucPagingControl1.Visible = false;
                grdvLocationList.DataSource = null;
                // ucMsg.ShowInfo("No Record Found");

            }
            grdvLocationList.DataBind();
            // updgrid.Update();

        }
    }

    /* #CC03 Add End */
    /*#CC07 added Started*/
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsPageRefereshed)
            {
                return;

            }
            DataTable dtError = new DataTable();
            HttpContext.Current.Session["PkeyColumns"] = null;
            string strKey = string.Empty;
            hlnkInvalid.Visible = false;

            Int16 Upload = 0;
            Upload = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            ViewState["TobeUploaded"] = strUploadedFileName;
            if (Upload == 1)
            {
                OpenXMLExcel objexcel = new OpenXMLExcel();
                DataSet DsExcel = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);
                if (DsExcel != null && DsExcel.Tables.Count > 0 && DsExcel.Tables[0].Rows.Count > 0)
                {

                    if (DsExcel.Tables[0].Rows.Count > Convert.ToInt32(PageBase.ValidExcelRows))
                    {
                        ucMsg.ShowInfo("Limit Crossed");
                    }
                    else if (DsExcel.Tables[0].Columns.Contains("OrgnhierarchyID") && Rbtdownloadtemplate.SelectedValue == "1")
                    {
                        ucMsg.ShowInfo("You are uploading an update template, please upload save template.");
                        return;
                    }
                    else if (!DsExcel.Tables[0].Columns.Contains("OrgnhierarchyID") && Rbtdownloadtemplate.SelectedValue == "2")
                    {
                        ucMsg.ShowInfo("You are uploading an save template, please upload update template.");
                        return;
                    }


                    if (Rbtdownloadtemplate.SelectedValue == "1")
                    {
                        ValidateUploadFile objValidateFile = new ValidateUploadFile();
                        {
                            DataSet objDS = DsExcel;
                            DataTable dt1 = DsExcel.Tables[0];
                            SortedList objSL = new SortedList();
                            SortedList objSLCorrData = new SortedList();
                            objValidateFile.UploadedFileName = strUploadedFileName;
                            objValidateFile.ExcelFileNameInTable = "SaveOrgHierarchy";
                            objValidateFile.CompanyId = PageBase.ClientId;
                            objValidateFile.ValidateFileWithCompanyId(false, out objDS, out objSL);
                            if (objValidateFile.Message != null && objValidateFile.Message.Trim() != "")
                                ucMsg.ShowInfo(objValidateFile.Message);
                            else
                            {
                                ucMsg.Visible = false;
                                bool blnIsUpload = true;
                                if (objSL.Count >= 1 && objSL.Keys.Count >= 1)
                                {
                                    objDS.Tables["DtExcelSheet"].Columns.Add(new DataColumn("ReasonForInvalid"));
                                    IDictionaryEnumerator objIDicEnum = objSL.GetEnumerator();
                                    while (objIDicEnum.MoveNext())
                                    {
                                        string[] strpkeyColumnName = Convert.ToString(HttpContext.Current.Session["PkeyColumns"]).Split(',');
                                        if (HttpContext.Current.Session["PkeyColumns"] != null)
                                        {
                                            for (int i = 0; i <= objDS.Tables["DtExcelSheet"].Rows.Count - 1; i++)
                                            {
                                                strKey = string.Empty;
                                                for (int j = 0; j <= strpkeyColumnName.Length - 1; j++)
                                                {
                                                    if (strKey == string.Empty)
                                                        strKey = objDS.Tables["DtExcelSheet"].Rows[i][strpkeyColumnName[j]].ToString();
                                                    else
                                                        strKey = strKey + objDS.Tables["DtExcelSheet"].Rows[i][strpkeyColumnName[j]].ToString();
                                                }
                                                if (strKey == objIDicEnum.Key.ToString())
                                                {
                                                    objDS.Tables["DtExcelSheet"].Rows[i]["ReasonForInvalid"] = objIDicEnum.Value.ToString();
                                                }
                                            }
                                        }
                                    }
                                    objDS.Tables[0].AcceptChanges();
                                    if (objDS.Tables["DtExcelSheet"] != null && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                                    {
                                        hlnkInvalid.Visible = true;
                                        dsErrorProne.Merge(objDS.Tables["DtExcelSheet"]);
                                        string strFileName = "InvalidData" + DateTime.Now.Ticks;
                                        ExportInExcel(dsErrorProne, strFileName);
                                        hlnkInvalid.Text = "Invalid Data";
                                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                        blnIsUpload = false;
                                    }
                                    blnIsUpload = false;
                                }
                                if (!objDS.Tables[0].Columns.Contains("ReasonForInvalid"))
                                {
                                    int counter = 0;
                                    if (!objDS.Tables[0].Columns.Contains("ReasonForInvalid"))
                                        objDS.Tables[0].Columns.Add(new DataColumn("ReasonForInvalid"));

                                    if (counter > 0)
                                    {
                                        ucMsg.ShowInfo("Invalid Records");
                                        dsErrorProne.Merge(objDS.Tables[0]);
                                        blnIsUpload = false;
                                    }
                                    else
                                    {
                                        objDS.Tables[0].Columns.Remove("ReasonForInvalid");
                                    }
                                }

                                if (objDS.Tables["DtDuplicateRecord"] != null && objDS.Tables["DtDuplicateRecord"].Rows.Count > 0)
                                {

                                    dsErrorProne.Merge(objDS.Tables["DtDuplicateRecord"]);
                                    blnIsUpload = false;
                                }
                                if (objDS.Tables["DtBlankData"] != null && objDS.Tables["DtBlankData"].Rows.Count > 0)
                                {

                                    dsErrorProne.Merge(objDS.Tables["DtBlankData"]);

                                    blnIsUpload = false;
                                }
                                if (blnIsUpload)
                                {
                                    if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                                    {
                                        InsertData(objDS.Tables[0]);
                                    }
                                    else
                                        ucMsg.ShowInfo(Resources.Messages.NoRecord);
                                }
                                else
                                {
                                    hlnkInvalid.Visible = true;
                                    string strFileName = "InvalidData" + DateTime.Now.Ticks;
                                    ExportInExcel(dsErrorProne, strFileName);
                                    hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                    hlnkInvalid.Text = "Invalid Data";
                                }
                            }
                        }
                    }
                    else if (Rbtdownloadtemplate.SelectedValue == "2")
                    {
                        ValidateUploadFile objValidateFile = new ValidateUploadFile();
                        {
                            DataSet objDS = DsExcel;
                            DataTable dt1 = DsExcel.Tables[0];
                            SortedList objSL = new SortedList();
                            SortedList objSLCorrData = new SortedList();
                            objValidateFile.UploadedFileName = strUploadedFileName;
                            objValidateFile.ExcelFileNameInTable = "UpdateOrgHierarchy";
                            objValidateFile.CompanyId = PageBase.ClientId;
                            objValidateFile.ValidateFileWithCompanyId(false, out objDS, out objSL);
                            if (objValidateFile.Message != null && objValidateFile.Message.Trim() != "")
                                ucMsg.ShowInfo(objValidateFile.Message);
                            else
                            {
                                ucMsg.Visible = false;
                                bool blnIsUpload = true;
                                if (objSL.Count >= 1 && objSL.Keys.Count >= 1)
                                {
                                    objDS.Tables["DtExcelSheet"].Columns.Add(new DataColumn("ReasonForInvalid"));
                                    IDictionaryEnumerator objIDicEnum = objSL.GetEnumerator();
                                    while (objIDicEnum.MoveNext())
                                    {
                                        string[] strpkeyColumnName = Convert.ToString(HttpContext.Current.Session["PkeyColumns"]).Split(',');
                                        if (HttpContext.Current.Session["PkeyColumns"] != null)
                                        {
                                            for (int i = 0; i <= objDS.Tables["DtExcelSheet"].Rows.Count - 1; i++)
                                            {
                                                strKey = string.Empty;
                                                for (int j = 0; j <= strpkeyColumnName.Length - 1; j++)
                                                {
                                                    if (strKey == string.Empty)
                                                        strKey = objDS.Tables["DtExcelSheet"].Rows[i][strpkeyColumnName[j]].ToString();
                                                    else
                                                        strKey = strKey + objDS.Tables["DtExcelSheet"].Rows[i][strpkeyColumnName[j]].ToString();
                                                }
                                                if (strKey == objIDicEnum.Key.ToString())
                                                {
                                                    objDS.Tables["DtExcelSheet"].Rows[i]["ReasonForInvalid"] = objIDicEnum.Value.ToString();
                                                }
                                            }
                                        }
                                    }
                                    objDS.Tables[0].AcceptChanges();
                                    if (objDS.Tables["DtExcelSheet"] != null && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                                    {
                                        hlnkInvalid.Visible = true;
                                        dsErrorProne.Merge(objDS.Tables["DtExcelSheet"]);
                                        string strFileName = "InvalidData" + DateTime.Now.Ticks;
                                        ExportInExcel(dsErrorProne, strFileName);
                                        hlnkInvalid.Text = "Invalid Data";
                                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                        blnIsUpload = false;
                                    }
                                    blnIsUpload = false;
                                }
                                if (!objDS.Tables[0].Columns.Contains("ReasonForInvalid"))
                                {
                                    int counter = 0;
                                    if (!objDS.Tables[0].Columns.Contains("ReasonForInvalid"))
                                        objDS.Tables[0].Columns.Add(new DataColumn("ReasonForInvalid"));

                                    if (counter > 0)
                                    {
                                        ucMsg.ShowInfo("Invalid Records");
                                        dsErrorProne.Merge(objDS.Tables[0]);
                                        blnIsUpload = false;
                                    }
                                    else
                                    {
                                        objDS.Tables[0].Columns.Remove("ReasonForInvalid");
                                    }
                                }

                                if (objDS.Tables["DtDuplicateRecord"] != null && objDS.Tables["DtDuplicateRecord"].Rows.Count > 0)
                                {

                                    dsErrorProne.Merge(objDS.Tables["DtDuplicateRecord"]);
                                    blnIsUpload = false;
                                }
                                if (objDS.Tables["DtBlankData"] != null && objDS.Tables["DtBlankData"].Rows.Count > 0)
                                {

                                    dsErrorProne.Merge(objDS.Tables["DtBlankData"]);

                                    blnIsUpload = false;
                                }
                                if (blnIsUpload)
                                {
                                    if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                                    {
                                        InsertData(objDS.Tables[0]);
                                    }
                                    else
                                        ucMsg.ShowInfo(Resources.Messages.NoRecord);
                                }
                                else
                                {
                                    hlnkInvalid.Visible = true;
                                    string strFileName = "InvalidData" + DateTime.Now.Ticks;
                                    ExportInExcel(dsErrorProne, strFileName);
                                    hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                    hlnkInvalid.Text = "Invalid Data";
                                }
                            }
                        }
                    }
                }
                else
                {
                    ucMsg.ShowInfo("File is empty! Some Mandatory columns has no required data!");
                }
            }
            else
            {
                ucMsg.Visible = true;
                ucMsg.ShowInfo("Please Browse File !");
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }
    private void InsertData(DataTable objdt)
    {
        if (objdt != null)
        {
            DataSet objds = new DataSet();

            if (objdt != null && objdt.Rows.Count > 0)
            {

                try
                {
                    if (Rbtdownloadtemplate.SelectedValue == "1")
                    {
                        if (ViewState["TobeUploaded"] != null)
                        {
                            OpenXMLExcel objexcel = new OpenXMLExcel();
                            string strUploadedFileNameFromViewState = ViewState["TobeUploaded"].ToString();

                            DataSet DsXML = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + ViewState["TobeUploaded"].ToString());
                            string guid = Guid.NewGuid().ToString();
                            ViewState[strPrimarySessionName] = guid;
                            string Radiobuttonid = Rbtdownloadtemplate.SelectedValue;
                            DsXML.Tables[0].Columns.Add(AddColumn(guid, "TransactionUploadSessionId", typeof(System.String)));
                            DsXML.Tables[0].Columns.Add(AddColumn(Radiobuttonid, "ActionType", typeof(System.String)));
                            DataTable dtUploadQueue = DsXML.Tables[0].Copy();
                            dtUploadQueue.Columns.Add(new DataColumn("PasswordSalt", typeof(string)));
                            if (dtUploadQueue.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dtUploadQueue.Rows)
                                {
                                    if (dr["Password"].ToString().Length > 0)
                                    {
                                        using (Authenticates ObjAuth = new Authenticates())
                                        {
                                            dr["PasswordSalt"] = ObjAuth.GenerateSalt(dr["Password"].ToString().Trim().Length);
                                            dr["Password"] = ObjAuth.EncryptPassword(dr["Password"].ToString(), dr["PasswordSalt"].ToString().Trim());
                                        };

                                    }
                                }
                            }
                            if (dtUploadQueue.Rows.Count > 0)
                            {
                                if (!UserDetailBCP(dtUploadQueue))
                                {
                                    ucMsg.ShowError("Error Occured While transferring the data to the server");
                                    return;
                                }
                            }

                            using (CommonMaster objDetail = new CommonMaster())
                            {
                                Authenticates ObjAuth = new Authenticates();
                                objDetail.UserID = PageBase.UserId;
                                objDetail.SessionId = guid;
                                objDetail.CompanyID = PageBase.ClientId;
                                strPSalt = ObjAuth.GenerateSalt(strDefaultPassword.Trim().Length);
                                strPassword = ObjAuth.EncryptPassword(strDefaultPassword, strPSalt.Trim());
                                objDetail.Password = strPassword;
                                objDetail.PasswordSalt = strPSalt;
                                DataSet dtInvalidRecordSet = objDetail.UploadOrgHierarchyWithUserSave();
                                Int32 result = objDetail.Out_Param;
                                if (result == 0)
                                {
                                    ucMsg.ShowSuccess("Data Uploaded Successfully");
                                    return;
                                }
                                if (result == 1 && objDetail.OutError != "")
                                {
                                    ucMsg.ShowError(objDetail.OutError);
                                    return;
                                }
                                if (result == 1 && dtInvalidRecordSet != null && dtInvalidRecordSet.Tables[0].Rows.Count > 0)
                                {
                                    DataSet ds = new DataSet();
                                    string strFileName = "Invalid Data" + DateTime.Now.Ticks;

                                    //if (dtInvalidRecordSet.Tables[0].Rows.Count > 0)
                                    //{
                                    //    foreach (DataRow dr in dtInvalidRecordSet.Tables[0].Rows)
                                    //    {
                                    //        using (Authenticates ObjAuth = new Authenticates())
                                    //        {
                                    //            Password = ObjAuth.DecryptPassword(Convert.ToString(dr["Password"]), Convert.ToString(dr["PasswordSalt"]));
                                    //        };
                                    //        dr["Password"] = Password;
                                    //    }
                                    //    if (dtInvalidRecordSet.Tables[0].Columns.Contains("PasswordSalt"))
                                    //    {
                                    //        dtInvalidRecordSet.Tables[0].Columns.Remove("PasswordSalt");
                                    //        dtInvalidRecordSet.Tables[0].AcceptChanges();
                                    //    }
                                    //    dtInvalidRecordSet.Tables[0].AcceptChanges();
                                    //}


                                    ExportInExcel(dtInvalidRecordSet, strFileName);
                                    hlnkInvalid.Visible = true;
                                    hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                    hlnkInvalid.Text = "Invalid Data";
                                    ucMsg.Visible = true;
                                    ucMsg.ShowInfo("Please click on Invalid data to check the error obtained");

                                }
                            }


                        }
                    }
                    else if (Rbtdownloadtemplate.SelectedValue == "2")
                    {
                        if (ViewState["TobeUploaded"] != null)
                        {
                            OpenXMLExcel objexcel = new OpenXMLExcel();
                            string strUploadedFileNameFromViewState = ViewState["TobeUploaded"].ToString();
                            DataSet DsXML = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + ViewState["TobeUploaded"].ToString());
                            string guid = Guid.NewGuid().ToString();
                            ViewState[strPrimarySessionNameupdate] = guid;
                            DsXML.Tables[0].Columns.Add(AddColumn(guid, "TransactionUploadSessionId", typeof(System.String)));
                            DataTable dtUploadQueue = DsXML.Tables[0].Copy();
                            dtUploadQueue.Columns.Add(new DataColumn("PasswordSalt", typeof(string)));
                            if (dtUploadQueue.Rows.Count > 0)
                            {

                                foreach (DataRow dr in dtUploadQueue.Rows)
                                {
                                    if (Convert.ToString(dr["Password"]).Length > 0)
                                    {
                                        using (Authenticates ObjAuth = new Authenticates())
                                        {
                                            dr["PasswordSalt"] = ObjAuth.GenerateSalt(dr["Password"].ToString().Trim().Length);
                                            dr["Password"] = ObjAuth.EncryptPassword(dr["Password"].ToString(), dr["PasswordSalt"].ToString().Trim());
                                        };

                                    }
                                }
                            }
                            if (dtUploadQueue.Rows.Count > 0)
                            {
                                if (!UserDetailBCP(dtUploadQueue))
                                {
                                    ucMsg.ShowError("Error Occured While transferring the data to the server");
                                    return;
                                }
                            }

                            using (CommonMaster objDetail = new CommonMaster())
                            {
                                objDetail.UserID = PageBase.UserId;
                                objDetail.SessionId = guid;
                                objDetail.CompanyID = PageBase.ClientId;
                                strPSalt = GenerateSalt(strDefaultPassword.Length);
                                strPassword = EncryptPassword(strDefaultPassword, strPSalt);
                                objDetail.Password = strPassword;
                                objDetail.PasswordSalt = strPSalt;
                                DataSet dtInvalidRecordSet = objDetail.UploadOrgHierarchyWithUserUpdate();
                                Int32 result = objDetail.Out_Param;
                                if (result == 0)
                                {
                                    ucMsg.ShowSuccess("Data Update Successfully");
                                    return;
                                }
                                if (result == 1 && objDetail.OutError != "")
                                {
                                    ucMsg.ShowError(objDetail.OutError);
                                    return;
                                }
                                if (result == 1 && dtInvalidRecordSet != null && dtInvalidRecordSet.Tables[0].Rows.Count > 0)
                                {
                                    DataSet ds = new DataSet();
                                    string strFileName = "Invalid Data" + DateTime.Now.Ticks;
                                    ExportInExcel(dtInvalidRecordSet, strFileName);
                                    hlnkInvalid.Visible = true;
                                    hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                    hlnkInvalid.Text = "Invalid Data";
                                    ucMsg.Visible = true;
                                    ucMsg.ShowInfo("Please click on Invalid data to check the error obtained");

                                }
                            }


                        }
                    }
                }
                catch (Exception ex)
                {
                    ucMsg.ShowError(ex.Message);
                }
            }
            else
            {
                ucMsg.ShowInfo("File is empty!");
            }
        }

    }
    public bool UserDetailBCP(DataTable dtUpload)
    {
        try
        {

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "BulkUploadOrgnhierarchyWithUser";
                if (dtUpload.Columns.Contains("OrgnhierarchyID"))
                {
                    bulkCopy.ColumnMappings.Add("OrgnhierarchyID", "OrgnhierarchyID");
                }
                if (dtUpload.Columns.Contains("OrgnHierarchyType"))
                {
                    bulkCopy.ColumnMappings.Add("OrgnHierarchyType", "OrgnHierarchyType");
                }
                if (dtUpload.Columns.Contains("ParentHierarchyCode"))
                {
                    bulkCopy.ColumnMappings.Add("ParentHierarchyCode", "ParentHierarchyCode");
                }
                if (dtUpload.Columns.Contains("LocationName"))
                {
                    bulkCopy.ColumnMappings.Add("LocationName", "LocationName");
                }
                if (dtUpload.Columns.Contains("LocationCode"))
                {
                    bulkCopy.ColumnMappings.Add("LocationCode", "LocationCode");
                }
                if (dtUpload.Columns.Contains("PersonName"))
                {
                    bulkCopy.ColumnMappings.Add("PersonName", "PersonName");
                }
                if (dtUpload.Columns.Contains("Email"))
                {
                    bulkCopy.ColumnMappings.Add("Email", "Email");
                }
                if (dtUpload.Columns.Contains("Mobile"))
                {
                    bulkCopy.ColumnMappings.Add("Mobile", "Mobile");
                }
                if (dtUpload.Columns.Contains("LoginID"))
                {
                    bulkCopy.ColumnMappings.Add("LoginID", "LoginID");
                }
                if (dtUpload.Columns.Contains("Password"))
                {
                    bulkCopy.ColumnMappings.Add("Password", "Password");
                }
                if (dtUpload.Columns.Contains("ActionType"))
                {
                    bulkCopy.ColumnMappings.Add("ActionType", "ActionType");
                }
                if (dtUpload.Columns.Contains("PasswordSalt"))
                {
                    bulkCopy.ColumnMappings.Add("PasswordSalt", "PasswordSalt");
                }
                if (dtUpload.Columns.Contains("Latitude"))
                {
                    bulkCopy.ColumnMappings.Add("Latitude", "Latitude");
                }
                if (dtUpload.Columns.Contains("Longitude"))
                {
                    bulkCopy.ColumnMappings.Add("Longitude", "Longitude");
                }
                if (dtUpload.Columns.Contains("GeoFancingRadius"))
                {
                    bulkCopy.ColumnMappings.Add("GeoFancingRadius", "GeoFancingRadius");
                }
                if (dtUpload.Columns.Contains("StateName"))
                {
                    bulkCopy.ColumnMappings.Add("StateName", "StateName");
                }
                if (dtUpload.Columns.Contains("CityName"))
                {
                    bulkCopy.ColumnMappings.Add("CityName", "CityName");
                }
                if (dtUpload.Columns.Contains("UserRole"))
                {
                    bulkCopy.ColumnMappings.Add("UserRole", "UserRole");
                }
                bulkCopy.ColumnMappings.Add("TransactionUploadSessionId", "TransactionUploadSessionId");
                bulkCopy.WriteToServer(dtUpload);
                return true;
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
            return false;
        }
    }
    private void ExportInExcel(DataSet DsError, string strFileName)
    {
        if (DsError != null && DsError.Tables.Count > 0)
        {
            PageBase.ExportToExeclV2(DsError, strFileName, DsError.Tables.Count);
        }
    }
    protected void DownloadReferenceCodeForSave_Click(object sender, EventArgs e)
    {

        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (CommonMaster objSalesData = new CommonMaster())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.CompanyID = PageBase.ClientId;
                objSalesData.TemplateType = 3;
                dsTemplateCode = objSalesData.GetMaterialMasterReferenceData();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "ReferenceCode";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "OrgnHierarchyType", "OrgnHierarchyTypeDetail", "StateCityDetail" };
                    ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, 3);


                    if (dsTemplateCode.Tables.Count > 3)
                        dsTemplateCode.Tables.RemoveAt(3);
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsTemplateCode, FilenameToexport, 3, strExcelSheetName);
                }
                else
                {
                    ucMsg.ShowInfo(Resources.Messages.NoRecord);
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());

        }
    }
    protected void UpdateTemplateFile_Click(object sender, EventArgs e)
    {

        string Password = string.Empty;
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (CommonMaster objSalesData = new CommonMaster())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.CompanyID = PageBase.ClientId;
                objSalesData.TemplateType = 4;
                dsTemplateCode = objSalesData.GetMaterialMasterTemplate();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "UpdateOrgnHierarchy";
                PageBase.RootFilePath = FilePath;
                string[] strExcelSheetName = { "UpdateOrgnHierarchy" };
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {
                    ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, 1);
                    DataTable dt = dsTemplateCode.Tables[0].Copy();
                    if (dt.Rows.Count > 0 && dt.Columns.Contains("PasswordSalt"))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            using (Authenticates ObjAuth = new Authenticates())
                            {
                                Password = ObjAuth.DecryptPassword(Convert.ToString(dr["Password"]), Convert.ToString(dr["PasswordSalt"]));
                            };
                            dr["Password"] = Password;
                        }
                        if (dt.Columns.Contains("PasswordSalt"))
                        {
                            dt.Columns.Remove("PasswordSalt");
                            dt.AcceptChanges();
                        }
                        DataSet dtcopy = new DataSet();
                        dtcopy.Merge(dt);
                        dtcopy.Tables[0].AcceptChanges();
                        ZedService.Utility.ZedServiceUtil.ExportToExecl(dtcopy, FilenameToexport, 1, strExcelSheetName);
                    }
                    else
                    {
                        DataView dataView = dsTemplateCode.Tables[0].DefaultView;
                        DataTable dt01 = dataView.ToTable(true, "SrNo", "OrgnhierarchyID", "OrgnHierarchyType", "ParentHierarchyCode", "LocationName", "LocationCode", "PersonName", "Email", "Mobile", "LoginID", "Password", "ActionType", "Latitude", "Longitude", "GeoFancingRadius", "StateName", "CityName");
                        DataSet dstemp = new DataSet();
                        dstemp.Tables.Add(dt01);
                        ChangedExcelSheetNames(ref dstemp, strExcelSheetName, 1);
                        if (dstemp.Tables.Count > 1)
                            dstemp.Tables.RemoveAt(1);
                        for (int i = dstemp.Tables[0].Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = dstemp.Tables[0].Rows[i];

                            dstemp.Tables[0].Rows.Remove(dr);

                        }

                        ZedService.Utility.ZedServiceUtil.ExportToExecl(dstemp, FilenameToexport, 1, strExcelSheetName);
                    }

                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());

        }
    }
    protected void DwnldTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (CommonMaster objSalesData = new CommonMaster())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.CompanyID = PageBase.ClientId;
                objSalesData.TemplateType = 3;
                dsTemplateCode = objSalesData.GetMaterialMasterTemplate();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {
                    DataView dataView = dsTemplateCode.Tables[0].DefaultView;
                    DataTable dt01 = dataView.ToTable(true, "SrNo", "OrgnHierarchyType", "ParentHierarchyCode", "LocationName", "LocationCode", "PersonName", "Email", "Mobile", "LoginID", "Password", "Latitude", "Longitude", "GeoFancingRadius", "StateName", "CityName", "UserRole");
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "SaveOrgnHierarchy";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "SaveOrgnHierarchy" };
                    DataSet dstemp = new DataSet();
                    dstemp.Tables.Add(dt01);
                    ChangedExcelSheetNames(ref dstemp, strExcelSheetName, 1);
                    if (dstemp.Tables.Count > 1)
                        dstemp.Tables.RemoveAt(1);
                    for (int i = dstemp.Tables[0].Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = dstemp.Tables[0].Rows[i];

                        dstemp.Tables[0].Rows.Remove(dr);

                    }

                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dstemp, FilenameToexport, 1, strExcelSheetName);
                }
                else
                {
                    ucMsg.ShowInfo(Resources.Messages.NoRecord);
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());

        }
    }
    protected void Rbtdownloadtemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        ucMsg.Visible = false;
        if (Rbtdownloadtemplate.SelectedValue == "1")
        {
            ForSaveTemplateheading.Visible = true;
            ForSaveTemplatedownload.Visible = true;
            ReferenceIdForsaveheading.Visible = true;
            ReferenceIdForsave.Visible = true;

        }
        else
        {
            ForSaveTemplateheading.Visible = false;
            ForSaveTemplatedownload.Visible = false;
            ReferenceIdForsaveheading.Visible = false;
            ReferenceIdForsave.Visible = false;
        }
        if (Rbtdownloadtemplate.SelectedValue == "2")
        {
            ForUploadTemplateheading.Visible = true;
            ForUpdateTemplatedownload.Visible = true;
            ReferenceIdForupdateheading.Visible = true;
            ReferenceIdForupdate.Visible = true;

        }
        else
        {
            ForUploadTemplateheading.Visible = false;
            ForUpdateTemplatedownload.Visible = false;
            ReferenceIdForupdateheading.Visible = false;
            ReferenceIdForupdate.Visible = false;

        }
    }
    /*#CC07 added End*/
    protected void DownloadReferenceCodeForUpdate_Click(object sender, EventArgs e)
    {
        string Password = string.Empty;
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (CommonMaster objSalesData = new CommonMaster())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.CompanyID = PageBase.ClientId;
                objSalesData.TemplateType = 4;
                dsTemplateCode = objSalesData.GetMaterialMasterReferenceData();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {
                    foreach (DataRow dr in dsTemplateCode.Tables[0].Rows)
                    {
                        using (Authenticates ObjAuth = new Authenticates())
                        {
                            Password = ObjAuth.DecryptPassword(Convert.ToString(dr["Password"]), Convert.ToString(dr["PasswordSalt"]));
                        };
                        dr["Password"] = Password;
                    }
                    if (dsTemplateCode.Tables[0].Columns.Contains("PasswordSalt"))
                    {
                        dsTemplateCode.Tables[0].Columns.Remove("PasswordSalt");
                    }
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "ReferenceCode";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "OrgnHierarchyDetail" };
                    ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, 1);
                    if (dsTemplateCode.Tables.Count > 1)
                        dsTemplateCode.Tables.RemoveAt(1);
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsTemplateCode, FilenameToexport, 1, strExcelSheetName);
                }
                else
                {
                    ucMsg.ShowInfo(Resources.Messages.NoRecord);
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());

        }
    }
    public string GenerateSalt(int length)
    {
        string guidResult = System.Guid.NewGuid().ToString();
        guidResult = guidResult.Replace("-", string.Empty);
        if (length <= 0 || length > guidResult.Length)
        {
            throw new ArgumentException("Length must be between 1 and " + guidResult.Length);
        }
        return guidResult.Substring(0, length);
    }
    public string EncryptPassword(string plainText, string saltValue)
    {
        byte[] initVectorBytes = System.Text.Encoding.ASCII.GetBytes("@1B2c3D4e5F6g7H8");
        // Must be 16 bytes
        byte[] saltValueBytes = System.Text.Encoding.ASCII.GetBytes(saltValue);
        // Encoding Password Salt Value.
        byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        //Encoding the string to be en
        PasswordDeriveBytes password = new PasswordDeriveBytes("zedaxis", saltValueBytes, "SHA1", 2);
        //Rfc2898DeriveBytes("zedaxis", saltValueBytes, 2)

        byte[] keyBytes = password.GetBytes(128 / 8);
        RijndaelManaged symmetricKey = new RijndaelManaged();
        symmetricKey.Mode = CipherMode.CBC;
        ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

        System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
        // Define cryptographic stream (always use Write mode for encryption).
        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        // Start encrypting.
        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
        // Finish encrypting.
        cryptoStream.FlushFinalBlock();

        // Convert our encrypted data from a memory stream into a byte array.
        byte[] cipherTextBytes = memoryStream.ToArray();

        // Close both streams.
        memoryStream.Close();
        cryptoStream.Close();

        // Convert encrypted data into a base64-encoded string.
        string cipherText = Convert.ToBase64String(cipherTextBytes);

        // Return encrypted string.
        return cipherText;
    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            ddlCity.Items.Clear();
            using (OrgHierarchyData objuser = new OrgHierarchyData())
            {
                objuser.CompanyId = PageBase.ClientId;
                objuser.StateID = Convert.ToInt32(ddlState.SelectedValue);
                dt = objuser.GetStateCityList(1);
            };
            String[] colArray = { "CityID", "CityName" };
            PageBase.DropdownBinding(ref ddlCity, dt, colArray);
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
}

