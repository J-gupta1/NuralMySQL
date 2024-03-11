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

public partial class Masters_SalesChannel_ManageSalesChannelType : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillSalesChannelType();
            BindHierarchy();
            fillGrid();
        }
       
    }


    void FillSalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            DataTable dt = new DataTable();
            String[] StrCol = new String[] { "SalesChannelTypeID", "SalesChannelTypeName" };
            dt = ObjSalesChannel.GetSalesChannelType();
            ViewState["Table1"] = dt;
            PageBase.DropdownBinding(ref cmbParentSalesChannelType, dt, StrCol);
            ViewState["SalesType"] = dt;


        }
    }

    private void BindHierarchy()
    {
        try
        {
            DataTable dt = new DataTable();
            using (OrgHierarchyData objuser = new OrgHierarchyData())
            {
               dt = objuser.GetAllHierarchyLevel();
            };
            String[] colArray = { "HierarchyLevelID", "HierarchyLevelName" };
            PageBase.DropdownBinding(ref cmbHierarchyLevel, dt, colArray);
          }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }



    public void fillGrid()
    {
        using (SalesChannelData obj = new SalesChannelData())
        {
            DataTable dt = obj.GetSalesChannelTypeDetails();
            grdSalesChannelType.DataSource = dt;
            grdSalesChannelType.DataBind();
            updgrid.Update();

        }


    }

    protected void btninsert_click(object sender, EventArgs e)
    {
        using (SalesChannelData obj = new SalesChannelData())
        {
            obj.salesChannelTypeName = txtsalesChanneltypeName.Text.Trim();
            obj.SalesChannelTypegroupName = txtgroupName.Text.Trim();
            obj.ParentSalesChannelTypeID = Convert.ToInt32(cmbParentSalesChannelType.SelectedValue);
            obj.HierarchyLevelID = Convert.ToInt16(cmbHierarchyLevel.SelectedValue);
            obj.BilltoRetailer = chkBilToRetailer.Checked;
            obj.IsAutoGenerate = chkIsAutoGenerate.Checked;
            obj.IsPTOAllowed = ChkPTO.Checked;
            if (ViewState["Status"] != null)
            {
                obj.Status = (bool)ViewState["Status"];
            }
            else
            {
                obj.Status = true;
            }
                if (ViewState["TypeID"] == null)
            {
                btnsubmit.CausesValidation = true;
                obj.InsertUpdateChannelSalesChannelType();
                if (obj.Error != null)
                {
                    ucMessage1.ShowError(obj.Error.ToString());

                }
                else
                {
                    ucMessage1.ShowSuccess(Resources.Messages.InsertSuccessfull);
                    fillGrid();
                    blankall();
                }

            }
            else
            {
                obj.SalesChannelTypeID = (Int16)ViewState["TypeID"];
                obj.InsertUpdateChannelSalesChannelType();
                btnsubmit.CausesValidation = true;
                if (obj.Error != null )
                {
                    ucMessage1.ShowError(obj.Error.ToString());

                }
                else
                {
                    ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);
                    fillGrid();
                    ViewState["TypeID"] = null;
                    ViewState["Status"] = null;
                    blankall();
                }

            }


        }


    }


    public void blankall()
    {
        txtgroupName.Text = "";
        txtsalesChanneltypeName.Text = "";
        cmbHierarchyLevel.SelectedValue = "0";
        cmbParentSalesChannelType.SelectedValue = "0";
        chkBilToRetailer.Checked = false;
        chkIsAutoGenerate.Checked = false;
        ChkPTO.Checked = false;
        btnsubmit.Text = "Submit";
        cmbParentSalesChannelType.Enabled = true;
        updAddUserMain.Update();
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        blankall();
    }
    protected void grdSalesChannelType_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        using (SalesChannelData obj = new SalesChannelData())
        {

            if (e.CommandName == "Active")
            {
                try
                {
                    ucMessage1.Visible = false;
                    blankall();

                    obj.SalesChannelTypeID = Convert.ToInt16(e.CommandArgument);
                    DataTable dt = obj.GetSalesChannelTypeDetails();
                    obj.SalesChannelTypeID = Convert.ToInt16(dt.Rows[0]["SalesChannelTypeID"]);
                    obj.salesChannelTypeName = Convert.ToString(dt.Rows[0]["SalesChannelTypeName"]);
                    obj.SalesChannelTypegroupName = Convert.ToString(dt.Rows[0]["SalesChannelTypeGroupName"]);
                    obj.ParentSalesChannelTypeID = Convert.ToInt16(dt.Rows[0]["ParentSalesChannelTypeID"]);
                    obj.HierarchyLevelID = Convert.ToInt16(dt.Rows[0]["HierarchyLevelID"]);
                    obj.BilltoRetailer = Convert.ToBoolean(dt.Rows[0]["BillToRetailer"]);
                    obj.IsAutoGenerate = Convert.ToBoolean(dt.Rows[0]["IsAutoGenerate"]);
                    obj.IsPTOAllowed = Convert.ToBoolean(dt.Rows[0]["ReturnForInvoicePTO"]);
                    obj.Status = Convert.ToBoolean(dt.Rows[0]["Status"]);
                    if (obj.Status == true)
                    {
                        obj.Status = false;
                    }
                    else
                    {
                        obj.Status = true;
                    }

                    obj.InsertUpdateChannelSalesChannelType();
                    if (obj.Error != null)
                    {
                        ucMessage1.ShowError(obj.Error.ToString());

                    }
                    else
                    {
                        ucMessage1.ShowInfo(Resources.Messages.StatusChanged);
                        blankall();
                        fillGrid();
                        updgrid.Update();
                    }


                }

                catch (Exception ex)
                {
                    ucMessage1.ShowInfo(ex.ToString());
                }
            }

            if (e.CommandName == "cmdEdit")
            {
                try
                {
                    ucMessage1.Visible = false;
                    cmbParentSalesChannelType.Enabled = true;
                    obj.SalesChannelTypeID = Convert.ToInt16(e.CommandArgument);
                    DataTable dt = obj.GetSalesChannelTypeDetails();
                    ViewState["TypeID"] = Convert.ToInt16(dt.Rows[0]["SalesChannelTypeID"]);
                    txtsalesChanneltypeName.Text = Convert.ToString(dt.Rows[0]["SalesChannelTypeName"]);
                    txtgroupName.Text = Convert.ToString(dt.Rows[0]["SalesChannelTypeGroupName"]);
                    cmbParentSalesChannelType.ClearSelection();
                    cmbParentSalesChannelType.SelectedValue = Convert.ToString(dt.Rows[0]["ParentSalesChannelTypeID"]);
                    if (Convert.ToInt16(dt.Rows[0]["ParentSalesChannelTypeID"]) == 0)
                    {
                        cmbParentSalesChannelType.Enabled = false;
                        btnsubmit.CausesValidation = false;
                    }
                    cmbHierarchyLevel.ClearSelection();
                    cmbHierarchyLevel.SelectedValue = Convert.ToString(dt.Rows[0]["HierarchyLevelID"]);
                    chkBilToRetailer.Checked = Convert.ToBoolean(dt.Rows[0]["BillToRetailer"]);
                    chkIsAutoGenerate.Checked = Convert.ToBoolean(dt.Rows[0]["IsAutoGenerate"]);
                    ChkPTO.Checked = Convert.ToBoolean(dt.Rows[0]["ReturnForInvoicePTO"]);
                    ViewState["Status"] = Convert.ToBoolean(dt.Rows[0]["Status"]);
                    btnsubmit.Text = "Update";
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
    protected void grdSalesChannelType_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdSalesChannelType.PageIndex = e.NewPageIndex;
            fillGrid();
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
    
}
