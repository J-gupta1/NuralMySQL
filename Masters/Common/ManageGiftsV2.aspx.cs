#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*/
* ================================================================================================
* ================================================================================================
*
* COPYRIGHT (c) 2016 Zed Axis Technologies (P) Ltd.
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART,
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* ================================================================================================
* Created By : Sumit Maurya
* Created On : 07-Apr-2016
* Module : Manage Gifts
* Description : This module is copy of Manage Gifts.
* ================================================================================================
* Change Log:
* ------------- 
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
using System.Data;
using System.Data.SqlClient;
using DataAccess;
using BussinessLogic;

public partial class Masters_Common_ManageGiftsV2 : PageBase
{
    DataTable giftinfo;


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            BindScheme();
            databind();
            chkActive.Checked = true;
        }

    }


    # region user functions

    public void blankinsert()
    {
        txtGiftName.Text = "";
        txtEligiblity.Text = "";
        btnCreate.Text = "Submit";
        ddlScheme.SelectedValue = "0";
        updAddUserMain.Update();
    }

    public void blankserch()
    {
        txtSerName.Text = "";
        ddlSchemeSearch.SelectedValue = "0";
        UpdSearch.Update();
    }

    public void databind()
    {
        using (SchemeData obj = new SchemeData())
        {
            try
            {
                ucMessage1.Visible = false;
                obj.GiftName = txtSerName.Text.ToString();
                obj.GiftID = 0;
                obj.SchemeID = Convert.ToInt32(ddlSchemeSearch.SelectedValue);
                DataTable dt = obj.GetGiftInfo();
                ViewState["Table"] = dt;
                grdGift.DataSource = dt;
                grdGift.DataBind();
                blankinsert();
                updgrid.Update();
            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }

    }

    # endregion

    #region control functions

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        if (IsPageRefereshed == true)
        {
            return;
        }       //Pankaj Dhingra
        if (ddlScheme.SelectedValue == "0")
        {
            ucMessage1.ShowInfo("Please select Scheme");
            return;
        }
        using (SchemeData obj = new SchemeData())
        {
            obj.GiftName = txtGiftName.Text.Trim();
            obj.EligiblityPoints = Convert.ToInt32(txtEligiblity.Text.Trim());
            if (chkActive.Checked == true)
            {
                obj.GiftStatus = 1;
            }
            else
            {
                obj.GiftStatus = 0;
            }
            obj.GiftID = (ViewState["GiftID"] == null) ? 0 : (int)ViewState["GiftID"];
            obj.SchemeID = Convert.ToInt32(ddlScheme.SelectedValue);
            int result = obj.InsUpdGiftInfoV2();
            if (obj.Error != "")
            {
                ucMessage1.ShowInfo(obj.Error);
            }
            if (result == 0)
            {

                databind();
                blankinsert();
                blankserch();
                if (ViewState["GiftID"] == null)
                    ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                else
                    ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);
                ViewState["GiftID"] = null;
                UpdSearch.Update();
            }

            /*            
            if (ViewState["GiftID"] == null || (int)ViewState["GiftID"] == 0)
            {
                obj.InsUpdGiftInfo();

                if (obj.Error == "")
                {
                    databind();
                    ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    blankinsert();
                    blankserch();
                }
                else
                {
                    ucMessage1.ShowInfo(obj.Error);
                }
            }
            else
            {
                obj.GiftID = (int)ViewState["GiftID"];
                obj.InsUpdGiftInfo();
                if (obj.Error == "")
                {
                    databind();
                    blankinsert();
                    ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);
                    ViewState["GiftID"] = null;
                }
                else
                {
                    ucMessage1.ShowInfo(obj.Error);
                }
            }*/

        }

    }



    protected void btncancel_click(object sender, EventArgs e)
    {
        blankinsert();
        blankserch();
        databind();


    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        blankinsert();
        if (txtSerName.Text == "" && ddlSchemeSearch.SelectedValue == "")
        {
            ucMessage1.ShowInfo("Please enter the search parameter");
            return;
        }

        databind();
    }
    protected void grdColor_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (SchemeData obj = new SchemeData())
        {
            if (e.CommandName == "Active")
            {
                try
                {
                    obj.GiftID = Convert.ToInt32(e.CommandArgument);
                    DataTable dt = obj.GetGiftInfo();
                    obj.GiftName = Convert.ToString(dt.Rows[0]["GiftName"]);
                    obj.EligiblityPoints = Convert.ToInt32(dt.Rows[0]["EligiblityPoint"]);
                    obj.GiftStatus = Convert.ToInt16(dt.Rows[0]["Active"]);
                    obj.SchemeID = Convert.ToInt32(dt.Rows[0]["SchemeID"]);
                    if (obj.GiftStatus == 1)
                    {
                        obj.GiftStatus = 0;
                    }
                    else
                    {
                        obj.GiftStatus = 1;
                    }
                    obj.InsUpdGiftInfoV2();
                    if (obj.Error == "")
                    {
                        databind();
                        ucMessage1.ShowSuccess(Resources.Messages.StatusChanged);

                    }
                    else
                    {
                        ucMessage1.ShowInfo(obj.Error);
                    }

                }
                catch (Exception ex)
                {
                    ucMessage1.ShowInfo(ex.Message.ToString());
                    PageBase.Errorhandling(ex);
                }


            }

            if (e.CommandName == "cmdEdit")
            {
                try
                {
                    obj.GiftID = Convert.ToInt32(e.CommandArgument);
                    ViewState["GiftID"] = obj.GiftID;
                    DataTable dt = obj.GetGiftInfo();

                    txtGiftName.Text = Convert.ToString(dt.Rows[0]["GiftName"]);
                    txtEligiblity.Text = Convert.ToString(dt.Rows[0]["EligiblityPoint"]);
                    obj.GiftStatus = Convert.ToInt16(dt.Rows[0]["Active"]);
                    ddlScheme.SelectedValue = (Convert.ToString(dt.Rows[0]["SchemeID"]) == "" || Convert.ToString(dt.Rows[0]["SchemeID"]) == "0") ? "0" : Convert.ToString(dt.Rows[0]["SchemeID"]);

                    if (obj.GiftStatus == 1)
                    {
                        chkActive.Checked = true;
                    }
                    else
                    {
                        chkActive.Checked = false;
                    }
                    btnCreate.Text = "Update";
                    ucMessage1.Visible = false;
                    updAddUserMain.Update();
                }
                catch (Exception ex)
                {
                    ucMessage1.ShowInfo(ex.Message.ToString());
                    PageBase.Errorhandling(ex);
                }
            }
        }

    }

    protected void btngetalldta_Click(object sender, EventArgs e)
    {
        //ddlSchemeSearch.SelectedValue = "0";
        blankinsert();
        blankserch();
        databind();
    }

    protected void grdColor_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdGift.PageIndex = e.NewPageIndex;
        databind();
    }

    # endregion


    # region export to excel

    protected void Exporttoexcel_Click(object sender, EventArgs e)
    {
        if (ViewState["Table"] != null)
        {

            DataTable dt = (DataTable)ViewState["Table"];
            string[] DsCol = new string[] { "GiftName", "EligiblityPoint", "SchemeName", "GiftStatus" };
            DataTable DsCopy = new DataTable();
            dt = dt.DefaultView.ToTable(true, DsCol);
            dt.Columns["GiftStatus"].ColumnName = "Status";

            if (dt.Rows.Count > 0)
            {
                ucMessage1.Visible = false;
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "GiftDetails";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dtcopy, FilenameToexport);
                ViewState["Table"] = null;
            }
            else
            {
                ucMessage1.ShowError(Resources.Messages.NoRecord);

            }
            ViewState["Table"] = null;
        }


    }

    #endregion


    protected void grdColor_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    public void BindScheme()
    {
        try
        {
            SchemeData objScheme = new SchemeData();
            DataSet ds = objScheme.dsGetSchemeDetail();
            ddlScheme.DataSource = ds.Tables[0];
            ddlScheme.DataTextField = "SchemeName";
            ddlScheme.DataValueField = "SchemeID";
            ddlScheme.DataBind();
            ddlScheme.Items.Insert(0, new ListItem("Select", "0"));



            ddlSchemeSearch.DataSource = ds.Tables[0];
            ddlSchemeSearch.DataTextField = "SchemeName";
            ddlSchemeSearch.DataValueField = "SchemeID";
            ddlSchemeSearch.DataBind();
            ddlSchemeSearch.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {

        }
    }
}
