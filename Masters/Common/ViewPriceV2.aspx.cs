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
using System.Text;
using System;
/*
 * 14 Jan 2016, Karam Chand Sharma, #CC01, Export model name also
 * 07-Jul-2016, Sumit Maurya, #CC02, Grid binding and pagination changed for fast processing.
 */
public partial class Masters_HO_Admin_ViewPriceV2 : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.Visible = false;
            if (!IsPostBack)
            {
                BindPriceList();
                BindSKUList();
                /* #CC02 Add Start  */
                BindGridRow();
                HideColumns();
                /* #CC02 Add End  */
            }
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.Message.ToString());
            updMsg.Update();
            PageBase.Errorhandling(ex);
        }
    }
    private void BindPriceList()
    {
        try
        {
            DataTable dt = new DataTable();
            cmbPriceList.Items.Clear();
            using (ProductData ObjProduct = new ProductData())
            {
                if (PageBase.BaseEntityTypeID == 3 && PageBase.SalesChanelID == 0)
                    ObjProduct.Condition = 1;
                if ( PageBase.SalesChanelID == 0)
                    ObjProduct.Condition = 0;
                if ( PageBase.SalesChanelID > 0)
                    ObjProduct.Condition = 2;
                ObjProduct.CompanyId = PageBase.ClientId;
                ObjProduct.UserId = PageBase.UserId;
                ObjProduct.Status = true;
                dt = ObjProduct.GetPriceListInfoV2();
                if (ObjProduct.error != null)
                {
                    ucMessage1.ShowError(ObjProduct.error);
                    return;
                }
            };
            String[] colArray = { "PriceListID", "PriceListName" };
            PageBase.DropdownBinding(ref cmbPriceList, dt, colArray);
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.Message.ToString());
            updMsg.Update();
            PageBase.Errorhandling(ex);

        }
    }//ok
    private void BindSKUList()
    {
        try
        {
            DataTable dt = new DataTable();
            cmbSKUName.Items.Clear();

            using (ProductData ObjProduct = new ProductData())
            {
                ObjProduct.Status = true;
                ObjProduct.CompanyId = PageBase.ClientId;
                dt = ObjProduct.GetSKUInfo();
            };
            String[] colArray = { "SKUID", "SKUCode" };
            PageBase.DropdownBinding(ref cmbSKUName, dt, colArray);
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
            updMsg.Update();
            PageBase.Errorhandling(ex);

        }
    }//ok
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (cmbPriceList.SelectedValue == "0" && cmbSKUName.SelectedValue == "0" && ucDateFrom.Date == "" && ucDateTo.Date == "")
        {
            ucMessage1.ShowInfo("Enter searching parameter(s)");
            updMsg.Update();
            /* pnlsearch.Visible = false;  #CC02 Commented */
            return;
        }
        else
        {

            GridPrice.PageIndex = 0;
            FillGrid();
        }
    }
    void FillGrid()
    {
        DataTable Dt = new DataTable();
        using (ProductData ObjProduct = new ProductData())
        {
            ObjProduct.PriceListID = Convert.ToInt16(cmbPriceList.SelectedValue);
            ObjProduct.SKUId = Convert.ToInt16(cmbSKUName.SelectedValue);
            if (ucDateFrom.TextBoxDate.Text != "")
            {
                ObjProduct.EffectiveDate = Convert.ToDateTime(ucDateFrom.Date);
            }
            if (ucDateTo.TextBoxDate.Text != "")
            {
                ObjProduct.DateRange = Convert.ToDateTime(ucDateTo.Date);
            }
            if (PageBase.BaseEntityTypeID == 3 && PageBase.SalesChanelID == 0)
                ObjProduct.Condition = 1;
            if ( PageBase.SalesChanelID == 0)
                ObjProduct.Condition = 0;
            if  (PageBase.SalesChanelID > 0)
                ObjProduct.Condition = 2;
            ObjProduct.UserId = PageBase.UserId;
            Dt = ObjProduct.GetPriceInfoV2();
        };
        if (Dt != null && Dt.Rows.Count > 0)
        {
            ViewState["Table"] = Dt;
            /*ExportToExcel.Visible = true; #CC02 Commented */
            GridPrice.Visible = true;
            GridPrice.DataSource = Dt;
            GridPrice.DataBind();
            /* pnlsearch.Visible = true;  #CC02 Commented */
            if (PageBase.BaseEntityTypeID == 3 && PageBase.SalesChanelID == 0)
            {
                GridPrice.Columns[2].Visible = false;
                GridPrice.Columns[4].Visible = false;
                GridPrice.Columns[5].Visible = false;
            }
            if ( PageBase.SalesChanelID > 0)
            {
                if (PageBase.SalesChannelLevel == 2 || PageBase.SalesChannelLevel == 3)
                {
                    GridPrice.Columns[2].Visible = false;
                }
                if (PageBase.SalesChannelLevel == 4)
                {
                    GridPrice.Columns[2].Visible = false;
                    GridPrice.Columns[4].Visible = false;
                }
            }
            GridPrice.Columns[11].Visible = false;
        }
        else
        {
            GridPrice.DataSource = null;
            GridPrice.DataBind();
            ucMessage1.ShowInfo(Resources.Messages.NoRecord);
            /*  pnlsearch.Visible = false;  #CC02 Commented */
        }
        /*UpdGrid.Update();  #CC02 Commented */
    }//ok

    protected void btnActiveDeactive_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton btnActiveDeactive = (ImageButton)sender;
            Int32 Result = 0;
            Int16 PriceMasterID = Convert.ToInt16(btnActiveDeactive.CommandArgument);
            using (ProductData ObjProduct = new ProductData())
            {
                ObjProduct.PriceMasterID = PriceMasterID;
                Result = ObjProduct.UpdateStatusPriceInfo();
            };
            if (Result == 1)
            {
                ucMessage1.ShowSuccess(Resources.Messages.StatusChanged);
            }
            else
            {
                ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            }
            updMsg.Update();
            FillGrid();
        }
        catch (Exception ex)
        {
            ucMessage1.ShowInfo(ex.Message.ToString());
            updMsg.Update();

        }

    }
    protected void btnDelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton btnDelete = (ImageButton)sender;
            Int32 Result = 0;
            Int16 PriceMasterID = Convert.ToInt16(btnDelete.CommandArgument);
            using (ProductData ObjProduct = new ProductData())
            {
                ObjProduct.PriceMasterID = PriceMasterID;
                Result = ObjProduct.DeletePriceInfo();
                if (Result == 0)
                {
                    ucMessage1.ShowSuccess(Resources.Messages.Delete);
                }
                else
                {
                    ucMessage1.ShowError(ObjProduct.error);
                }
            }
            updMsg.Update();
            FillGrid();
        }
        catch (Exception ex)
        {
            ucMessage1.ShowInfo(ex.Message.ToString());
            updMsg.Update();

        }

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
    }
    void Clear()
    {
        cmbPriceList.SelectedValue = "0";
        cmbSKUName.SelectedValue = "0";
        ucDateFrom.Date = "";
        ucDateTo.Date = "";
        FillGrid();
    }
    protected void ExportToExcel_Click(object sender, EventArgs e)
    {
        if (ViewState["Table"] != null)
        {
            DataTable dt = (DataTable)ViewState["Table"];
            string[] DsCol = null;
            if (PageBase.BaseEntityTypeID == 3 && PageBase.SalesChanelID == 0)
            {
                dt.Columns.Remove("WHPrice");
                dt.Columns.Remove("SDPrice");
                dt.Columns.Remove("MDPrice");
                DsCol = new string[] { "PriceListName", /*#CC01 ADDED START*/ "Model" /*#CC01 ADDED END*/, "SKUCode", "RetailerPrice", "MOP", "MRP", "EffectiveDateCheck", "ValidTill" };
            }
            else if ( PageBase.SalesChanelID > 0)
            {
                if (PageBase.SalesChannelLevel == 2 || PageBase.SalesChannelLevel == 3)
                {
                    dt.Columns.Remove("WHPrice");
                    DsCol = new string[] { "PriceListName", /*#CC01 ADDED START*/"Model"/*#CC01 ADDED END*/, "SKUCode", "SDPrice", "MDPrice", "RetailerPrice", "MOP", "MRP", "EffectiveDateCheck", "ValidTill" };
                }
                if (PageBase.SalesChannelLevel == 4)
                {
                    dt.Columns.Remove("WHPrice");
                    dt.Columns.Remove("SDPrice");
                    DsCol = new string[] { "PriceListName", /*#CC01 ADDED START*/"Model"/*#CC01 ADDED END*/, "SKUCode", "MDPrice", "RetailerPrice", "MOP", "MRP", "EffectiveDateCheck", "ValidTill" };
                }
            }
            else
            {
                DsCol = new string[] { "PriceListName",/*#CC01 ADDED START*/ "Model"/*#CC01 ADDED END*/, "SKUCode", "WHPrice", "SDPrice", "MDPrice", "RetailerPrice", "MOP", "MRP", "EffectiveDateCheck", "ValidTill" };
            }
            DataTable DsCopy = new DataTable();
            dt = dt.DefaultView.ToTable(true, DsCol);
            if (dt.Rows.Count > 0)
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "PriceList";
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
    }//ok
    protected void LBViewPrice_Click(object sender, EventArgs e)
    {
        //Response.Redirect(Resources.Messages.PriceMaster, false);

    }
    protected void GridPrice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridPrice.PageIndex = e.NewPageIndex;
        FillGrid();
    }
    protected void GridPrice_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                Label lblEffectiveDate = (Label)e.Row.FindControl("lblEffectiveDateCheck");

                ImageButton imgbtnDelete = (ImageButton)e.Row.FindControl("btnDelete");

                if (lblEffectiveDate.Text.Trim() != string.Empty)
                {
                    DateTime dt_Today = DateTime.Now;
                    DateTime dt_validfrom = Convert.ToDateTime(lblEffectiveDate.Text.Trim());
                    if (dt_validfrom.CompareTo(dt_Today) > 0)
                    {
                        //imgbtnDelete.Visible = true;
                    }
                    else
                    {
                        imgbtnDelete.Visible = false;
                    }
                }


                //    if (objlblActive.Text == "False")
                //    {
                //        objBtnActive.ImageUrl = "~/Assets/images/decative.png";//objBtnActive.Text = "Activate";
                //        objBtnActive.ToolTip = "Inactive";
                //    }
                //    else
                //    {
                //        objBtnActive.ImageUrl = "~/Assets/images/active.png";//objBtnActive.Text = "InActivate";*/
                //        objBtnActive.ToolTip = "Active";
                //    }
                //    ImageButton objDeleteConfirm = (ImageButton)e.Row.FindControl("imgbtnDelete");
                //    if (objDeleteConfirm != null)
                //    {
                //        objDeleteConfirm.Attributes.Add("Onclick", "if(!confirm('Are you sure you want to delete this record?')){return false;}");
                //    }
            }
            catch (Exception ex)
            {
                ucMessage1.ShowError(ex.ToString());
            }
        }

    }

    /* #CC02 Add Start */
    private void BindGridRow()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PriceListName");
        dt.Columns.Add("SKUCode");
        dt.Columns.Add("WHPrice");
        dt.Columns.Add("SDPrice");
        dt.Columns.Add("MDPrice");
        dt.Columns.Add("RetailerPrice");
        dt.Columns.Add("MOP");
        dt.Columns.Add("MRP");
        dt.Columns.Add("EffectiveDate");
        dt.Columns.Add("ValidTill");
        dt.Rows.Add();
        GridPrice.DataSource = dt;
        GridPrice.DataBind();
    }
    protected void ExportToExcel2_Click(object sender, EventArgs e)
    {
        try
        {
            string[] DsCol = null;
            using (ProductData obj = new ProductData())
            {
                DataTable dt = new DataTable();
                obj.PriceListID = Convert.ToInt32(cmbPriceList.SelectedValue);
                obj.SKUId = Convert.ToInt32(cmbSKUName.SelectedValue);
                obj.PageIndex = -1;
                obj.PageSize = Convert.ToInt32(PageBase.PageSize);
                if (ucDateFrom.TextBoxDate.Text != "")
                {
                    obj.FromDate = Convert.ToDateTime(ucDateFrom.TextBoxDate.Text);
                }
                if (ucDateTo.TextBoxDate.Text != "")
                {
                    obj.ToDate = Convert.ToDateTime(ucDateTo.TextBoxDate.Text);
                }
                if (PageBase.BaseEntityTypeID == 3 && PageBase.SalesChanelID == 0)
                    obj.Condition = 1;
                if ( PageBase.SalesChanelID == 0)
                    obj.Condition = 0;
                if ( PageBase.SalesChanelID > 0)
                    obj.Condition = 2;
                obj.UserId = PageBase.UserId;
                dt = obj.GetPriceInfoV3();

                if (PageBase.BaseEntityTypeID == 3 && PageBase.SalesChanelID == 0)
                {
                    dt.Columns.Remove("WHPrice");
                    dt.Columns.Remove("SDPrice");
                    dt.Columns.Remove("MDPrice");
                    DsCol = new string[] { "PriceListName", "Model", "SKUCode", "RetailerPrice", "MOP", "MRP", "EffectiveDateCheck", "ValidTill" };
                }
                else if ( PageBase.SalesChanelID > 0)
                {
                    if (PageBase.SalesChannelLevel == 2 || PageBase.SalesChannelLevel == 3)
                    {
                        dt.Columns.Remove("WHPrice");
                        DsCol = new string[] { "PriceListName", "Model", "SKUCode", "SDPrice", "MDPrice", "RetailerPrice", "MOP", "MRP", "EffectiveDateCheck", "ValidTill" };
                    }
                    if (PageBase.SalesChannelLevel == 4)
                    {
                        dt.Columns.Remove("WHPrice");
                        dt.Columns.Remove("SDPrice");
                        DsCol = new string[] { "PriceListName", "Model", "SKUCode", "MDPrice", "RetailerPrice", "MOP", "MRP", "EffectiveDateCheck", "ValidTill" };
                    }
                }
                else
                {
                    DsCol = new string[] { "PriceListName", "Model", "SKUCode", "WHPrice", "SDPrice", "MDPrice", "RetailerPrice", "MOP", "MRP", "EffectiveDateCheck", "ValidTill" };
                } DataTable DsCopy = new DataTable();
                dt = dt.DefaultView.ToTable(true, DsCol);
                if (dt.Rows.Count > 0)
                {
                    DataSet dtcopy = new DataSet();
                    dtcopy.Merge(dt);
                    dtcopy.Tables[0].AcceptChanges();
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "PriceList";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dtcopy, FilenameToexport);
                }
                else
                {
                    ucMessage1.ShowError(Resources.Messages.NoRecord);
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowInfo(ex.Message.ToString());
            updMsg.Update();
        }
    }

    public void HideColumns()
    {
        /* Columns of gridview which needed to be hide must be in Ascending order.*/
        if (PageBase.BaseEntityTypeID == 3 && PageBase.SalesChanelID == 0)
        {
            hdnHideColumns.Value = "2,4,5";
        }
        if ( PageBase.SalesChanelID > 0)
        {
            if (PageBase.SalesChannelLevel == 2 || PageBase.SalesChannelLevel == 3)
            {
                hdnHideColumns.Value = "2";
            }
            if (PageBase.SalesChannelLevel == 4)
            {
                hdnHideColumns.Value = "2,4";
            }
        }
    }
    /* #CC02 Add End */
   
}
