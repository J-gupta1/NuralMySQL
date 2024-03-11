using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataAccess;
using BussinessLogic;

public partial class Masters_Retailer_ManageRetailerTypeMapping : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindALL();
        }
    }
    private void BindALL()
    {
        using (RetailerData objRTMapping = new RetailerData())
        {
            try
            {
                ds = objRTMapping.GetReatailerTypeMapping();
            }
            catch (Exception ex)
            {
                ucMsg.ShowError(objRTMapping.Error);
            }
        };
        BindSalesChannelType(ds.Tables[0]);
        BindRetailerType(ds.Tables[1]);
        BindGVRetailerTypeMapping(ds.Tables[2]);
    }
    private void BindSalesChannelType(DataTable dt)
    {
        try
        {
            ddlSaelsChannelType.Items.Clear();
            String[] colArray = { "SalesChannelTypeID", "SalesChannelTypeName" };
            PageBase.DropdownBinding(ref ddlSaelsChannelType, dt, colArray);

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    private void BindRetailerType(DataTable dt)
    {
        try
        {
            ddlRetailerType.Items.Clear();
            String[] colArray = { "ReatilerTypeID", "RetailerTypeName" };
            PageBase.DropdownBinding(ref ddlRetailerType, dt, colArray);

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    private void BindGVRetailerTypeMapping(DataTable dt)
    {
        try
        {
            gvRetailerTypeMapping.DataSource = dt;
            gvRetailerTypeMapping.DataBind();
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        using (RetailerData objRTMapping = new RetailerData())
        {
            try
            {
                objRTMapping.RetailerTypeID = Convert.ToInt32(ddlRetailerType.SelectedValue);
                objRTMapping.SalesChannelTypeID = Convert.ToInt32(ddlSaelsChannelType.SelectedValue);
                objRTMapping.Status = chkStatus.Checked;
                objRTMapping.CreatedBy = PageBase.UserId;
                objRTMapping.InsReatailerTypeMapping();
                if (objRTMapping.InsError != null)
                {
                    ucMsg.ShowError(objRTMapping.InsError);
                }
                else
                {
                    ucMsg.ShowSuccess("Record Create Successfully");
                }
            }
            catch (Exception ex)
            {
                ucMsg.ShowError(objRTMapping.Error);
            }
        }
        BindALL();
    }
    protected void gvRetailerTypeMapping_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("togglecmdStatus"))
        {
            try
            {
                using (RetailerData objRTMapping = new RetailerData())
                {
                    objRTMapping.UPDToggleReatailerTypeMapping(Convert.ToInt32(e.CommandArgument),PageBase.UserId);
                    BindALL();
                    ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                }
            }
            catch (Exception ex)
            {
                ucMsg.ShowError(ex.ToString());
            }
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        BindALL();
        ucMsg.Visible = false;
    }
    protected void ExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {

            //if (ViewState["Table"] != null)
            //{
            // DataTable dt = (DataTable)ViewState["Table"];        //Pankaj Dhingra
            //FillGrid();
            DataTable dtExp;
            using (RetailerData objRTMapping = new RetailerData())
            {
                
                ds = objRTMapping.GetReatailerTypeMapping();
                DataSet dss = new DataSet();
              
                //DataTable dtExp = Dt.Copy();
                string[] DsCol = new string[] { "Retailer Type Name", "Sales Channel Type Name", "Status" };
                DataTable DsCopy = new DataTable();
                dtExp = ds.Tables[3].DefaultView.ToTable(true, DsCol);
                dss.Merge(dtExp);
                if (dtExp.Rows.Count > 0)
                {
                    //DataSet dtcopy = new DataSet();
                    //dtcopy.Merge(dtExp);
                    //dtcopy.Tables[0].AcceptChanges();
                    //String FilePath = Server.MapPath("../../");
                    //string FilenameToexport = "RetailerList";
                    //PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dss, "SalesChannelRelationMapping");
                    //ViewState["Table"] = null;
                }
                else
                {
                    ucMsg.ShowError(Resources.Messages.NoRecord);

                }
                //ViewState["Table"] = null;
                //}
            };
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void gvRetailerTypeMapping_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRetailerTypeMapping.PageIndex = e.NewPageIndex;
        BindALL();
        ucMsg.Visible = false;
    }
}
