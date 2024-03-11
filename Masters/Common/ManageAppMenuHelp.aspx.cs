#region Copyright(c) 2017 Zed-Axis Technologies All rights are reserved
/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2017 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Created By : Gaurav Tyagi
* Created On: 10-May-2019
 * Description:  To manage the questions and html link for App this interface required to build.
* ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 
 ====================================================================================================
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using System.Data;
using System.IO;


public partial class Master_Common_ManageAppMenuHelp : PageBase
{
    Boolean isedit,isstatusupdate = false;
    
    Dictionary<string, string> menuidtodisplayorder;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillAppMenu();
            // BindStatus();
            AppMenuHelpDetailData(1, 0);

        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {

        ucMessage1.Visible = false;
        PnlGrid.Visible = true;
        AppMenuHelpDetailData(1, 0);
    }
    protected void btnExportexcel_Click(object sender, EventArgs e)
    {
        AppMenuHelpDetailData(-1, 0);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ddlAppMenu.SelectedValue = "0";
        txt_helplink.TextBoxText = string.Empty;
        txt_question.TextBoxText = string.Empty;
        txtDO.Text = string.Empty;

    }
    void FillAppMenu()
    {
        using (Cls_AppMenu Obj_AM = new Cls_AppMenu())
        {
            ddlAppMenu.Items.Clear();
            menuidtodisplayorder = new Dictionary<string, string>();
            DataTable dt = Obj_AM.GetAppMenu();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    menuidtodisplayorder.Add(dr["MenuId"].ToString(), dr["DO"].ToString());
                }
                string[] str = { "MenuId", "MenuName" };
                PageBase.DropdownBinding(ref ddlAppMenu, dt, str);
                ViewState["RECORDS"] = menuidtodisplayorder;
            }
        };
    }


    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        AppMenuHelpDetailData(ucPagingControl1.CurrentPage, 0);
    }
    //private void BindStatus()
    //{
    //    DataTable dtresult = new DataTable();
    //    try
    //    {
    //        dtresult = clsDoaReport.GetEnumbyTableName("XML_Enum", "ViewAttendanceStatus");
    //        if (dtresult.Rows.Count > 0)
    //        {
    //            ddlStatus.DataSource = dtresult;
    //            ddlStatus.DataTextField = "Description";
    //            ddlStatus.DataValueField = "Value";
    //            ddlStatus.DataBind();
    //            ddlStatus.Items.Insert(0, new ListItem("Select", "255"));
    //        }
    //        else
    //        {
    //            ddlStatus.Items.Insert(0, new ListItem("Select", "255"));
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ucMessage1.ShowError(ex.Message.ToString());
    //    }
    //}
    public void AppMenuHelpDetailData(int pageno, Int64 Appmenuhelpid)
    {
        try
        {
            using (Cls_AppMenu obj_cls_appmenu = new Cls_AppMenu())
            {
                ViewState["TotalRecords"] = 0;
                ViewState["CurrentPage"] = pageno;
                obj_cls_appmenu.PageIndex = pageno;
                obj_cls_appmenu.PageSize = Convert.ToInt32(PageBase.PageSize);
                obj_cls_appmenu.AppMenuHelpId = Appmenuhelpid;
                DataSet ds = obj_cls_appmenu.getAppmenuhelpdetails();

                if (obj_cls_appmenu.TotalRecords > 0)
                {
                    PnlGrid.Visible = true;
                    if (pageno > 0)
                    {
                        if (isedit)
                        {
                            ddlAppMenu.SelectedValue = ds.Tables[0].Rows[0]["MenuId"].ToString();
                            txt_helplink.TextBoxText = ds.Tables[0].Rows[0]["HelpLink"].ToString();
                            txt_question.TextBoxText = ds.Tables[0].Rows[0]["QuestionText"].ToString();
                            txtDO.Text = ds.Tables[0].Rows[0]["DisplayOrder"].ToString();
                        }
                        else
                        {
                            gvAttendanceDetail.DataSource = ds;
                            gvAttendanceDetail.DataBind();

                            ViewState["TotalRecords"] = obj_cls_appmenu.TotalRecords;
                            ucPagingControl1.TotalRecords = obj_cls_appmenu.TotalRecords;
                            ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                            ucPagingControl1.SetCurrentPage = pageno;
                            ucPagingControl1.FillPageInfo();
                        }
                    }
                    else
                    {
                        string FilenameToexport = "AppMenuHelpDetailData";
                        PageBase.ExportToExecl(ds, FilenameToexport);
                    }
                }
                else
                {
                    ds = null;
                    gvAttendanceDetail.DataSource = null;
                    gvAttendanceDetail.DataBind();
                    ucMessage1.Visible = true;
                    ucMessage1.ShowInfo("No Record Found.");

                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void DownloadFile(object sender, EventArgs e)
    {
        try
        {
            string filePath = (sender as LinkButton).CommandArgument;
            string fullpath = PageBase.siteURL.Remove(PageBase.siteURL.Length - 1) + filePath;
            Response.ContentType = "image/jpg";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(fullpath));
            Response.WriteFile(fullpath);
            Response.End();
        }
        catch (Exception ex)
        {

            //ucMessage1.Visible = true;
            //ucMessage1.ShowError(ex.Message);
        }

    }
    protected void ddlAppMenu_SelectedIndexChanged(object sender, EventArgs e)
    {
        menuidtodisplayorder = (Dictionary<string, string>)ViewState["RECORDS"];
        if (menuidtodisplayorder.ContainsKey(ddlAppMenu.SelectedValue))
        {
            txtDO.Text = (Convert.ToInt64(menuidtodisplayorder[ddlAppMenu.SelectedValue].ToString()) + 1).ToString();
        }
        txt_question.TextBoxText = string.Empty;
        txt_helplink.TextBoxText = string.Empty;
        btncreate.Text = "Create";
    }
    protected void btncreate_Click(object sender, EventArgs e)
    {
        try
        {
            string ERROR = string.Empty;
            using (Cls_AppMenu OBJ_Cls_AppMenu = new Cls_AppMenu())
            {
                OBJ_Cls_AppMenu.AppMenuHelpId = Convert.ToInt64(ddlAppMenu.SelectedValue);
                OBJ_Cls_AppMenu.QuestionText = txt_question.TextBoxText;
                OBJ_Cls_AppMenu.HelpLink = txt_helplink.TextBoxText;
                OBJ_Cls_AppMenu.DisplayOrder = Convert.ToInt16(txtDO.Text);
                if (btncreate.Text == "Create")
                {
                    OBJ_Cls_AppMenu.InsertAppMenuHelp(ref ERROR);
                }
                else
                {
                    OBJ_Cls_AppMenu.AppMenuHelpId =Convert.ToInt64(ViewState["Appmenuhelpid"]);
                    OBJ_Cls_AppMenu.updateAppMenuHelp(ref ERROR);

                }
                if (ERROR.Trim().Length > 0)
                {
                    ucMessage1.ShowInfo(ERROR);
                }
                else
                {
                    FillAppMenu();
                    if (btncreate.Text == "Create")
                        ucMessage1.ShowInfo("Record Inserted Successfully;");
                    else
                    {
                        ucMessage1.ShowInfo("Record Updated Successfully;");
                        txt_helplink.TextBoxText = string.Empty;
                        txt_question.TextBoxText = string.Empty;
                        txtDO.Text = string.Empty;
                        btncreate.Text = "Create";
                        AppMenuHelpDetailData(1, 0);

                    }
                }
            }
            btncreate.Text = "Create";
        }
        catch (Exception ex)
        {
            ucMessage1.ShowInfo(ex.Message);
            // UpdMain.Update();
        }
    }
    protected void gvAttendanceDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Aphstatus")
            {
                string error = string.Empty;
                using (Cls_AppMenu OBJ_Cls_AppMenu = new Cls_AppMenu())
                {
                    OBJ_Cls_AppMenu.Satusupdate = 1;
                    OBJ_Cls_AppMenu.AppMenuHelpId = Convert.ToInt64(e.CommandArgument);
                    OBJ_Cls_AppMenu.updateAppMenuHelp(ref error);
                    AppMenuHelpDetailData(1, 0);
                    if (error.Trim().Length > 0)
                    {
                        ucMessage1.ShowInfo(error);
                    }
                }
            }
            else if (e.CommandName == "cmdEdit")
            {

                isedit = true;
               ViewState["Appmenuhelpid"] = Convert.ToInt64(e.CommandArgument);
                AppMenuHelpDetailData(1, Convert.ToInt64(e.CommandArgument));
                btncreate.Text = "Update";

            }
        }
        catch (Exception ex)
        { ucMessage1.ShowInfo(ex.Message); }

    }

    

}