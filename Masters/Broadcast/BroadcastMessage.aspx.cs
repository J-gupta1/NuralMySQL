#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*
     * ================================================================================================
     *
     * COPYRIGHT (c) 2008 Zed Axis Technologies (P) Ltd.
     * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART,
     * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE,
     * WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
     *
     * ================================================================================================
     * Created By : Sumit Maurya
     * Created On : 01-March-2016
     * Modified BY :
     * Role : Software Developer
     * Module : Broadcast Message
     * Description : 
     * ================================================================================================
     * Change Log :
     * DD-MMM-YYY, Name, #CCXX, Description.
     * ================================================================================================
     * Reviewed By :
     * ================================================================================================
       */

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;
using System.Data;

public partial class Masters_Broadcast_BroadcastMessage : PageBase
{
    DataTable dt = new DataTable();
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {

        ucMessage1.ShowControl = false;
        if (!IsPostBack)
        {
            BindDropdownandCheckboxes();

        }
    }

    public void BindDropdownandCheckboxes()
    {
        try
        {
            BroadcastSMS objbrd = new BroadcastSMS();
            ds = objbrd.GetStateandBroadcastTypeDetails();

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chcklistState.DataSource = ds.Tables[0];
                    chcklistState.DataTextField = "StateName";
                    chcklistState.DataValueField = "StateID";
                    chcklistState.DataBind();
                    hdnStateCount.Value = Convert.ToString(ds.Tables[0].Rows.Count);
                    foreach (ListItem lst in chcklistState.Items)
                    {
                        lst.Attributes.Add("onclick", "checkSelection(this.id);");
                    }
                }
            }

            if (ds.Tables.Count > 1)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ddlBroadcastType.DataSource = ds.Tables[1];
                    ddlBroadcastType.DataTextField = "BroadcastType";
                    ddlBroadcastType.DataValueField = "BroadcastTypeId";
                    ddlBroadcastType.DataBind();
                    ddlBroadcastType.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BroadcastSMS objbrd = new BroadcastSMS();
            DataTable dtStateID = new DataTable();
            dtStateID = dtId();
            int result;
            DataRow drID;
            foreach (ListItem lst in chcklistState.Items)
            {
                if (lst.Selected == true)
                {
                    drID = dtStateID.NewRow();
                    drID["Id"] = Convert.ToInt64(lst.Value);
                    dtStateID.Rows.Add(drID);
                    dtStateID.AcceptChanges();
                }
            }
            if (ddlBroadcastType.SelectedValue == "2" && dtStateID.Rows.Count < 1)
            {
                ucMessage1.ShowInfo("Please select atleast one state");
                return;
            }
            objbrd.UserID = PageBase.UserId;
            objbrd.BroadcastTypeId = Convert.ToInt32(ddlBroadcastType.SelectedValue);
            objbrd.BroadcastText = txtMessage.TextBoxText.Trim();
            objbrd.dtID = dtStateID;
            result = objbrd.SaveBroadcastMessage();
            if (result == 0)
            {
                ucMessage1.ShowSuccess("Broadcast SMS saved successfully");
                Clearfields();
            }
            if (result == 1)
            {
                ucMessage1.ShowError("Error in saving Broadcast SMS.");
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }


    public DataTable dtId()
    {
        DataTable DtTemp = new DataTable();
        DtTemp.Columns.Add("Id", typeof(Int64));
        return DtTemp;
    }


    protected void btnReset_Click(object sender, EventArgs e)
    {
        Clearfields();
        ucMessage1.Visible = false;
    }

    public void Clearfields()
    {
        try
        {
            txtMessage.TextBoxText = string.Empty;
            ddlBroadcastType.SelectedValue = "0";
            foreach (ListItem lst in chcklistState.Items)
            {
                lst.Selected = false;
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
}

