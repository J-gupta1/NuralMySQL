using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using BussinessLogic;
using DataAccess;




public partial class ClientServices_Common_ManageEnquiryTypeRoleMapping :PageBase //System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
           // blankInsert();
            BindList(1);
            BindEnquiryType();
            //BindEnquiryTypeAndRoles();
            ucValidFrom.MinRangeValue = DateTime.Now;
            ucMessage1.Visible = false;
        }
    }

     void BindList(int index)
        {
            try
            {
                index = index == 0 ? 1 : index;
                using(clsEnquiryDetail ObjMapping = new clsEnquiryDetail())
                {
                    ObjMapping.PageSize = Convert.ToInt32(PageSize);
                    ObjMapping.PageIndex = index;
                    DataTable dt = ObjMapping.SelectAll();
                    grdvList.Visible = true;
                    grdvList.DataSource = dt;
                    grdvList.DataBind();
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        ucPagingControl1.Visible = false;
                    }
                    else
                    {
                        ucPagingControl1.Visible = true;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.TotalRecords = ObjMapping.TotalRecords;
                        ucPagingControl1.FillPageInfo();
                    }
                    divgrd.Visible = true;
                    //updpnlGrid.Update();
                }
            }
            catch (Exception ex)
            {
                ucMessage1.Visible = true;
               ucMessage1.ShowError(ex.ToString());
            }
        }
        public void blankInsert()
        {
            idrole.Visible = false;
            ddlEnquiryType.Items.Clear();
            ddlEnquiryType.Items.Insert(0, new ListItem("Select", "0"));
            ddlEnquiryType.SelectedValue = "0";
            DdlEnquiryCategory.SelectedValue = "0";
            ucValidFrom.Date = null;
            chkAll.Checked = false;
            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "SelectUnselectAll", "CheckBoxListSelect();", true);
        }

        public void BindEnquiryType()
        {
            using (clsEnquiryDetail ObjMapping = new clsEnquiryDetail())
            {
                 try
                    {
                 DataSet ds = ObjMapping.SelectCategoryType();

                DataTable dt = ds.Tables[0];
                DdlEnquiryCategory.DataSource = dt;

                DdlEnquiryCategory.DataTextField = "EnquiryCategoryName";
                DdlEnquiryCategory.DataValueField = "EnquiryCategoryId";


                DdlEnquiryCategory.DataBind();
                DdlEnquiryCategory.Items.Insert(0, new ListItem("Select", "0"));

            }
                 catch (Exception ex)
                 {
                     ucMessage1.Visible = true;
                     ucMessage1.ShowError(ex.Message.ToString());
                 }
            }

        }
        private void BindEnquiryTypeAndRoles()
        {
            using (clsEnquiryDetail ObjMapping = new clsEnquiryDetail())
            {
                ObjMapping.PageIndex = -2;
                ObjMapping.PageSize = 1000;
                ObjMapping.EntityTypeID =Convert.ToInt32(DdlEnquiryCategory.SelectedValue);
                DataSet ds = ObjMapping.SelectEnquiryTypeAndRole();
                ddlEnquiryType.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //Start: Bind Enquiry Type
                   
                    ddlEnquiryType.DataSource = ds.Tables[0];
                    ddlEnquiryType.DataValueField = "EnquiryTypeMasterID";
                    ddlEnquiryType.DataTextField = "EnquiryType";
                    ddlEnquiryType.DataBind();
                    ddlEnquiryType.Items.Insert(0, new ListItem("Select", "0"));
                    //End: Bind Enquiry Type

                    //Start: Bind Roles
                    chkRoles.DataSource = ds.Tables[1];
                    chkRoles.DataValueField = "RoleID";
                    chkRoles.DataTextField = "EntityTypeRoleName";
                    chkRoles.DataBind();
                    //End: Bind Roles
                }
                ds.Dispose();
            }
        }

        
        #region Control functions
        protected void Save_Click(object sender, EventArgs e)
        {
            ListItem ChkSelected = chkRoles.SelectedItem;
            if (ChkSelected != null)
            {
                DataTable dtRoleID = new DataTable();
                dtRoleID.Columns.Add("PKID", typeof(Int64));
                dtRoleID.AcceptChanges();
                foreach (ListItem li in chkRoles.Items)
                {
                    if (li.Selected)
                    {
                        dtRoleID.Rows.Add(li.Value);
                    }
                }
                DateTime datecurrent =Convert.ToDateTime(ucValidFrom.Date);
                DateTime currentdatetime =System.DateTime.Now.Date;
                if (datecurrent < currentdatetime)
                {
                    ucMessage1.Visible = true;
                    ucMessage1.ShowInfo("Only Allow Current Or Future Date.");
                    return;
                }
                try
                {

                    int result = 1;
                    using(clsEnquiryDetail ObjMapping = new clsEnquiryDetail())
                    {
                        //ObjMapping.Active = chkActive.Checked ? Convert.ToInt16(1) : Convert.ToInt16(0);
                        ObjMapping.EnquiryTypeMasterID = Convert.ToInt64(ddlEnquiryType.SelectedValue);
                        ObjMapping.DtRoleID = dtRoleID;
                        ObjMapping.ValidFrom = Convert.ToDateTime(ucValidFrom.Date);
                        ObjMapping.LoginUserId = PageBase.UserId;
                        result = ObjMapping.InsertRoleSubCategoryMapping();

                        if (ObjMapping.XML_Error != null)
                        {
                            ucMessage1.Visible = true;
                            ucMessage1.XmlErrorSource = ObjMapping.XML_Error;
                           // updpnlSaveData.Update();
                            return;
                        }
                        if (result == 0)
                        {
                            ucMessage1.Visible = true;
                            ucMessage1.ShowSuccess("Mapping Done successfully.");
                            blankInsert();
                            BindList(ucPagingControl1.CurrentPage);
                           // updpnlSaveData.Update();
                            return;
                        }
                        if (result == 1)
                        {
                            ucMessage1.Visible = true;
                            ucMessage1.ShowError(ObjMapping.Error);
                           // updpnlSaveData.Update();
                            return;
                        }

                    }
                }
                catch (Exception ex)
                {
                    ucMessage1.Visible = true;
                    ucMessage1.ShowError(ex.Message);
                    //ExceptionPolicy.HandleException(ex, ExceptionPolicyName);
                }
            }
            else
            {
                ucMessage1.Visible = true;
                ucMessage1.ShowInfo("Please select any role first.");
            }
            
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ucMessage1.Visible = false;
          

            blankInsert();
        }
        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (txtSerCountry.Text == "" && ucStatusSer.SelectedIndex == 0)
        //        {
        //            blankSer();
        //            ucMessage1.ShowWarning(WarningMessages.EnterSearchCriteria);
        //            return;
        //        }
        //        ucMessage1.Visible = false;
        //        updpnlSaveData.Update();
        //        ucPagingControl1.SetCurrentPage = 1;
        //        BindList(1, Convert.ToInt16(ucStatusSer.SelectedValue));
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, ExceptionPolicyName);
        //    }
        //}
        //protected void btnReset_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        blankSer();
        //        BindList(1, 2);
        //        divgrd.Visible = false;
        //        updpnlGrid.Update();
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, ExceptionPolicyName);
        //    }
        //}
        //protected void btnShowAll_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ucMessage1.Visible = false;

        //        blankSer();
        //        BindList(1, 255);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, ExceptionPolicyName);
        //    }
        //}
        #endregion
        # region exportfunction
        //protected void Exporttoexcel_Click(object sender, ImageClickEventArgs e)
        //{
        //     using(clsEnquiryDetail ObjMapping = new clsEnquiryDetail())
        //        {
        //        ObjMapping.PageSize = Convert.ToInt32(PageSize);
        //        ObjMapping.PageIndex = -1;
        //        DataTable dt = ObjMapping.SelectAll();
        //        DataSet ds = new DataSet();
        //        ds.Merge(dt);
        //        //LuminousSMS.Utility.LuminousUtil.ExportToExecl(ds, "EnquityTypeMapping");
        //          ZedService.Utility.ZedServiceUtil.ExportToExecl(ds, "EnquityTypeMapping");
        //    }
        //}
        #endregion
        #region gridview functions
        protected void grdvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {

                    ImageButton objBtnActive = (ImageButton)e.Row.FindControl("imgActive");
                    Label objlblActive = (Label)e.Row.FindControl("lblactive");

                    Label lblvalidfrom = (Label)e.Row.FindControl("lblvalidfrom");
                    //if (objlblActive.Text == "0")
                    //{
                    //    objBtnActive.ImageUrl = "~/Assets/images/decative.png";
                    //    objBtnActive.ToolTip = "Inactive";
                    //}
                    //else
                    //{
                    //    objBtnActive.ImageUrl = "~/Assets/images/active.png";
                    //    objBtnActive.ToolTip = "Active";
                    //}

                    if (lblvalidfrom.Text.Trim() != string.Empty)
                    {
                        DateTime dt_Today = DateTime.Now.AddDays(1);
                        DateTime dt_validfrom = Convert.ToDateTime(lblvalidfrom.Text.Trim());
                        //if (dt_validfrom.CompareTo(dt_Today) > 0)
                        //{
                        //    //objBtnActive.Visible = true;
                        //    //objBtnActive.SkipValidation = true;
                        //    objBtnActive.Enabled = true;
                        //}
                        //else
                        //{
                        //    //objBtnActive.Visible = false;
                        //    //objBtnActive.SkipValidation = true;
                        //    objBtnActive.Enabled = false;
                        //}
                    }
                    else
                    {
                        //objBtnActive.Visible = true;
                       // objBtnActive.SkipValidation = true;
                        objBtnActive.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    ucMessage1.Visible = true;
                    ucMessage1.ShowError(ex.Message);
                }
            }
        }
        protected void grdvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int64 id = Convert.ToByte(e.CommandArgument);
            if (e.CommandName == "activeMapping")
            {
                try
                {
                    using(clsEnquiryDetail ObjMapping = new clsEnquiryDetail())
                    {
                        ObjMapping.EnquiryTypeRoleMappingID = id;
                        ObjMapping.LoginUserId = PageBase.UserId;
                        Int16 chkActive = ObjMapping.ToggleActivation();
                        if (chkActive == 0)
                        {
                            //BindList(ucPagingControl1.CurrentPage);
                            ucMessage1.Visible = true;
                            ucMessage1.ShowSuccess("Records Updated Successfully");
                            //BindList(ucPagingControl1.CurrentPage);
                        }
                        else
                        {
                            ucMessage1.Visible = true;
                            ucMessage1.ShowInfo(ObjMapping.Error);
                        }
                        BindList(ucPagingControl1.CurrentPage);
                    }
                }
                catch (Exception ex)
                {
                    ucMessage1.Visible = true;
                    ucMessage1.ShowError(ex.Message);
                }
            }
        }
        protected void UCPagingControl1_SetControlRefresh()
        {
                int intPageNumber = ucPagingControl1.CurrentPage;
                ucMessage1.Visible = false;
                BindList(ucPagingControl1.CurrentPage);
        }
        #endregion
        protected void lnkExportToExcel_Click(object sender, EventArgs e)
        {
            using (clsEnquiryDetail ObjMapping = new clsEnquiryDetail())
            {


                try
                {
                ObjMapping.PageSize = Convert.ToInt32(PageSize);
                ObjMapping.PageIndex = -1;
                DataTable dt = ObjMapping.SelectAll();
                if (dt.Rows.Count > 0)
                {
                    DataSet ds = new DataSet();
                    ds.Merge(dt);
                    ds.AcceptChanges();
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "EnquityTypeMapping";
                    PageBase.RootFilePath = FilePath;
                    //LuminousSMS.Utility.LuminousUtil.ExportToExecl(ds, "EnquityTypeMapping");
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(ds, FilenameToexport);

                }
                else
                {
                    ucMessage1.Visible = true;
                    ucMessage1.ShowInfo("No Record Found.");
                }
                           
                      
                }

                catch (Exception ex)
                {
                    ucMessage1.Visible = true;
                    ucMessage1.ShowError(ex.Message.ToString());
                }
               
            }
        }
        protected void DdlEnquiryCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            idrole.Visible = true;
            BindEnquiryTypeAndRoles();
        }
}
