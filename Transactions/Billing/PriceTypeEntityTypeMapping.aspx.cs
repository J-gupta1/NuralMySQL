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

public partial class Transactions_Billing_PriceTypeEntityTypeMapping : PageBase
{
    DataTable PriceTypeinfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            fillPriceType();
            fillEntityType();
            binddata();
        }
    }
    protected void btninsert_click(object sender, EventArgs e)
    {
        if (IsPageRefereshed == true)
        {
            return;
        }
        using (ClsPricetypemaster objmaster = new ClsPricetypemaster())
        {
            updAddUserMain.Update();
            if (!insertvalidate())
            {
                return;
            }
            else
            {
                objmaster.Pricetypeid =Convert.ToInt32(ddlPriceType.SelectedValue);
                objmaster.Entitytypeid = Convert.ToInt32(ddlEntityType.SelectedValue);
                if (chkstatus.Checked == true)
                {
                    objmaster.Status = 1;
                }
                else
                {
                    objmaster.Status = 0;
                }
                objmaster.UserId = PageBase.UserId;
                if (ViewState["PriceTypeEntityTypeMappingID"] == null || (int)ViewState["PriceTypeEntityTypeMappingID"] == 0)
                {
                    try
                    {
                        objmaster.InsertPriceTypeEntityMappingType();
                        if (objmaster.error == "")
                        {
                            binddata();
                            ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                            blankinsert();
                            blanksearch();
                            updgrid.Update();
                        }
                        else
                        {

                            ucMessage1.ShowInfo(objmaster.error);
                        }
                    }
                    catch (Exception ex)
                    {
                        ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                        PageBase.Errorhandling(ex);
                    }
                }
                else
                {
                    try
                    {
                        objmaster.PriceTypeEntityTypeMappingID = (int)ViewState["PriceTypeEntityTypeMappingID"];
                        objmaster.InsertPriceTypeEntityMappingType();
                        if (objmaster.error == "")
                        {
                            binddata();
                            ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);
                            ViewState["PriceTypeEntityTypeMappingID"] = null;
                            blankinsert();
                            updAddUserMain.Update();
                        }
                        else
                        {
                            ucMessage1.ShowInfo(objmaster.error);
                        }
                    }

                    catch (Exception ex)
                    {
                        ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                        PageBase.Errorhandling(ex);
                    }

                }
            }
        }
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        blankinsert();
        blanksearch();
        binddata();
        ucMessage1.Visible = false;
    }
    public void fillPriceType()
    {

        using (ClsPricetypemaster objmaster = new ClsPricetypemaster()) 
        {

            try
            {
                DataTable dt;
                dt = objmaster.SelectPriceTypeInfo();
                String[] colArray = { "PriceTypeID", "PriceTypeKeyword" };
                PageBase.DropdownBinding(ref ddlPriceType, dt, colArray);
                PageBase.DropdownBinding(ref ddlSerPriceType, dt, colArray);
                
            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }

        }
    }
    public void fillEntityType()
    {

        using (ClsPricetypemaster objmaster = new ClsPricetypemaster())
        {

            try
            {
                DataTable dt;
                dt = objmaster.SelectEntityTypeInfo();
                String[] colArray = { "EntityTypeID", "EntityType" };
                PageBase.DropdownBinding(ref ddlEntityType, dt, colArray);
                PageBase.DropdownBinding(ref ddlSerEntityType, dt, colArray);
              
            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }

        }
    }
    public void binddata()
    {
        try
        {
            ucMessage1.Visible = false;
            using (ClsPricetypemaster objpricetypemaster = new ClsPricetypemaster())
            {
                objpricetypemaster.Pricetypeid =Convert.ToInt32(ddlSerPriceType.SelectedValue);
                objpricetypemaster.Entitytypeid = Convert.ToInt32(ddlSerEntityType.SelectedValue);
                objpricetypemaster.Status = 2;
                PriceTypeinfo = objpricetypemaster.SelectPriceTypeEntitymappingInfo();
                grdPriceTypeEntityMapping.DataSource = PriceTypeinfo;
                grdPriceTypeEntityMapping.DataBind();
                updgrid.Update();

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }

    }
    public void blankinsert()
    {
        try
        {
            ddlEntityType.SelectedValue= "0";
            ddlPriceType.SelectedValue = "0";
            btnsubmit.Text = "Submit";
            updAddUserMain.Update();
        }
        catch (Exception ex)
        {
            string err = ex.Message.ToString();
            ucMessage1.ShowError(err);
        }
    }
    public void blanksearch()
    {
        try
        {

            ddlSerEntityType.SelectedValue = "0";
            ddlSerPriceType.SelectedValue = "0";
            UpdSearch.Update();
        }
        catch (Exception ex)
        {
            string err = ex.Message.ToString();
            ucMessage1.ShowError(err);
        }
    }
    public bool insertvalidate()
    {
        if (ddlPriceType.SelectedValue == "0" || ddlEntityType.SelectedValue == "0")
        {
            ucMessage1.ShowInfo("Please Select PriceType  and EntityType.");
            return false;
        }
        return true;

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        blankinsert();
        binddata();
    }
    protected void btnGetallData_Click(object sender, EventArgs e)
    {
        blankinsert();
        blanksearch();
        binddata();
        ucMessage1.Visible = false;
    }
    protected void btnExprtToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            binddata();
            DataTable dt = PriceTypeinfo.Copy();
            string[] DsCol = new string[] { "PriceTypeKeyword", "PriceTypeDescription", "CurrentStatus", "EntityType" };
            DataTable DsCopy = new DataTable();
            dt = dt.DefaultView.ToTable(true, DsCol);
            dt.Columns["CurrentStatus"].ColumnName = "Status";

            if (dt.Rows.Count > 0)
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "PriceTypeEntityMappingMasterDetails";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dtcopy, FilenameToexport);
            }
            else
            {
                ucMessage1.ShowError(Resources.Messages.NoRecord);

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
    protected void grdPriceTypeEntityMapping_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (ClsPricetypemaster objmaster = new ClsPricetypemaster())
        {

            if (e.CommandName == "Active")
            {
                try
                {
                    ucMessage1.Visible = false;
                    blankinsert();
                    objmaster.PriceTypeEntityTypeMappingID = Convert.ToInt32(e.CommandArgument);
                    objmaster.Status = 2;
                    PriceTypeinfo = objmaster.SelectPriceTypeEntitymappingInfo();
                    objmaster.Pricetypeid = Convert.ToInt32(PriceTypeinfo.Rows[0]["PriceTypeID"]);
                    objmaster.Entitytypeid = Convert.ToInt32(PriceTypeinfo.Rows[0]["EntityTypeID"]);
                    objmaster.Status = Convert.ToInt16(PriceTypeinfo.Rows[0]["Active"]);
                    objmaster.UserId = PageBase.UserId;
                    if (objmaster.Status == 1)
                    {
                        objmaster.Status = 0;
                    }
                    else
                    {
                        objmaster.Status = 1;
                    }

                    objmaster.InsertPriceTypeEntityMappingType();

                    if (objmaster.error == "")
                    {
                        binddata();
                        ucMessage1.ShowSuccess(Resources.Messages.StatusChanged);
                    }
                    else
                    {
                        ucMessage1.ShowInfo(objmaster.error);
                    }
                }
                catch (Exception ex)
                {
                    ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                    PageBase.Errorhandling(ex);
                }


            }
            if (e.CommandName == "cmdEdit")
            {
                try
                {
                    ucMessage1.Visible = false;
                    ViewState["PriceTypeEntityTypeMappingID"] = objmaster.PriceTypeEntityTypeMappingID = Convert.ToInt32(e.CommandArgument);
                    objmaster.Status = 2;
                    PriceTypeinfo = objmaster.SelectPriceTypeEntitymappingInfo();
                    ddlPriceType.SelectedValue = Convert.ToString(PriceTypeinfo.Rows[0]["PriceTypeID"]);
                    ddlEntityType.SelectedValue = Convert.ToString(PriceTypeinfo.Rows[0]["EntityTypeID"]);
                    if (PriceTypeinfo.Rows[0]["Active"].ToString() == "1")
                    {
                        chkstatus.Checked = true;
                    }
                    else
                    {
                        chkstatus.Checked = false;
                    }
                    btnsubmit.Text = "Update";
                    updAddUserMain.Update();
                    updgrid.Update();
                }
                catch (Exception ex)
                {
                    ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                    PageBase.Errorhandling(ex);
                }


            }
        }
    }
    protected void grdPriceTypeEntityMapping_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPriceTypeEntityMapping.PageIndex = e.NewPageIndex;
        binddata();
    }
}