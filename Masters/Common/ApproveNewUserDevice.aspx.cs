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
* Created By : Vijay Kumar Prajapati
* Created On: 12-Sept-2017 
 * Description: This is a DOA Search Report.
* ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 18-Jul-2018, Sumit Maurya, #CC01, UserID provided to get warehouse according to logged in userid(Done for ComioV5).
 ====================================================================================================
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;

public partial class MastersApproveNewUserDevice : PageBase
{
    public DataSet ds = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        ucMessage1.XmlErrorSource = "";
        if (!IsPostBack)
        {
            FillEntityType();
        }
    }
    void FillEntityType()
    {
        using (ClsPaymentReport ObjEntityType = new ClsPaymentReport())
        {

            ddlEntityType.Items.Clear();
            string[] str = { "EntityTypeID", "EntityType" };
            ObjEntityType.CompanyId = PageBase.ClientId;
            ObjEntityType.UserId = PageBase.UserId;
            PageBase.DropdownBinding(ref ddlEntityType, ObjEntityType.GetEntityTypeV5API(), str);

        };
    }
    protected void ddlEntityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillEntityTypeName(Convert.ToInt32(ddlEntityType.SelectedValue));
    }
    void FillEntityTypeName(int EntityTypeID)
    {
        using (ClsPaymentReport ObjEntityTypeName = new ClsPaymentReport())
        {

            ddlEntityTypeName.Items.Clear();
            ObjEntityTypeName.EntityTypeId = EntityTypeID;
            ObjEntityTypeName.UserId = PageBase.UserId;
            ObjEntityTypeName.CompanyId = PageBase.ClientId;
            string[] str = { "UserID", "EntityTypeName" };
            PageBase.DropdownBinding(ref ddlEntityTypeName, ObjEntityTypeName.GetEntityTypeName(), str);

        };
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            
            if (ddlEntityType.SelectedIndex == 0  && ddlEntityTypeName.SelectedIndex == 0)
            {
                ucMessage1.Visible = true;
                ucMessage1.ShowWarning("Select any search criteria");
                return;
            }
            else
            {
                bindSearchData(1);
            }
        }
        catch (Exception ex)
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            bindSearchData(-1);
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ucMessage1.Visible = false;
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        bindSearchData(ucPagingControl1.CurrentPage);

    }
    public void bindSearchData(int pageno)
    {
        try
        {
            ViewState["TotalRecords"] = 0;
            if (ViewState["CurrentPage"] == null)
            {
                pageno = 1;
                ViewState["CurrentPage"] = pageno;
            }
            using (UserData objUser = new UserData())
            {
                objUser.UserID = PageBase.UserId;
                objUser.SalesChannelID =Convert.ToInt32( ddlEntityTypeName.SelectedValue);
                objUser.EntitytypeId = Convert.ToInt32(ddlEntityType.SelectedValue);
                objUser.CompanyID = PageBase.ClientId;
                objUser.PageIndex = pageno;
                objUser.PageSize = Convert.ToInt32(PageBase.PageSize);
                DataSet ds = objUser.prcNewDeviceUsers();
                if (objUser.TotalRecords > 0)
                {
                    if (pageno > 0)
                    {
                        GridUserDevice.DataSource = ds;
                        GridUserDevice.DataBind();
                        //PnlGrid.Visible = true;
                        ViewState["TotalRecords"] = objUser.TotalRecords;
                        ucPagingControl1.TotalRecords = objUser.TotalRecords;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = pageno;
                        ucPagingControl1.FillPageInfo();
                        PnlGrid.Visible = true;
                        dvFooter.Visible = true;
                    }
                    else
                    {

                        string FilenameToexport = "ChangedDeviceUsers";
                        PageBase.ExportToExecl(ds, FilenameToexport);
                    }
                }
                else
                {
                    ds = null;
                    GridUserDevice.DataSource = null;
                    GridUserDevice.DataBind();
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
    
    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        bindSearchData(1);
    }
    
    protected void checkAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            
        }
        catch (Exception exc)
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowError(exc.Message);
        }
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            string approvedUsers = "";
            foreach (GridViewRow row in GridUserDevice.Rows)
            {
                if (((CheckBox)row.FindControl("chkRow")).Checked)
                {
                    approvedUsers = GridUserDevice.DataKeys[row.RowIndex].Values[0].ToString() + ",";
                }
            }
            if (!string.IsNullOrEmpty(approvedUsers))
            {
                using (UserData objUser = new UserData())
                {
                    objUser.UserID = PageBase.UserId;
                    objUser.CompanyID = PageBase.ClientId;
                    objUser.UserIds = approvedUsers;
                    int result = objUser.ApproveNewDevice();
                    if (result == 0)
                    {
                        bindSearchData(1);
                        ucMessage1.ShowSuccess("User device updated successfully.");
                    }
                    else if (!string.IsNullOrEmpty(objUser.Error))
                    {
                        ucMessage1.ShowError(objUser.Error);
                    }
                    else
                    {
                        ucMessage1.ShowError("Some Error occured");
                    }
                }
            }
            else
            {
                ucMessage1.ShowInfo("No record selected for device change approval.");
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
        }
    }
}