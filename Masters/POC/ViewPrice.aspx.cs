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
 * 11 Feb 2016, Karam Chand Sharma, #CC02, Export to excel was not working if data if very heavy , and data was expot from view start so fixed it and export from datatable
 */
public partial class Masters_HO_Admin_ViewPrice : PageBase
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
                ObjProduct.Status = true;
                dt = ObjProduct.GetPriceListInfo();
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
    }
    private void BindSKUList()
    {
        try
        {
            DataTable dt = new DataTable();
            cmbSKUName.Items.Clear();
            using (ProductData ObjProduct = new ProductData())
            {
                ObjProduct.Status = true;
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
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (cmbPriceList.SelectedValue == "0" && cmbSKUName.SelectedValue == "0" && ucDateFrom.Date == "" && ucDateTo.Date == "")
        {
            ucMessage1.ShowInfo("Enter searching parameter(s)");
            updMsg.Update();
            pnlsearch.Visible = false;
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
            Dt = ObjProduct.GetPriceInfo();
        };
        if (Dt != null && Dt.Rows.Count > 0)
        {
            // ViewState["Table"] = Dt;/*#CC02 COMMENTED*/
            ExportToExcel.Visible = true;
            GridPrice.Visible = true;
            GridPrice.DataSource = Dt;
            GridPrice.DataBind();
            pnlsearch.Visible = true;

        }
        else
        {
            GridPrice.DataSource = null;
            GridPrice.DataBind();
            ucMessage1.ShowInfo(Resources.Messages.NoRecord);
            pnlsearch.Visible = false;
        }
        UpdGrid.Update();
    }

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
        /*#CC02 START COMMENTED if (ViewState["Table"] != null)
        {
             DataTable dt = (DataTable)ViewState["Table"];#CC02 END COMMENTED*/
        // #CC02 START COMMENTED string[] DsCol = new string[] { "PriceListName", /*#CC01 ADDED START*/ "Model" /*#CC01 ADDED END*/, "SKUCode", "WHPrice", "SDPrice", "MDPrice", "RetailerPrice", "MOP", "MRP", "EffectiveDateCheck", "ValidTill" };#CC02 END COMMENTED 
        /*#CC02 START COMMENTED  DataTable DsCopy = new DataTable();
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

         }#CC02 END COMMENTED */
        /*#CC02 START COMMENTED */
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
            Dt = ObjProduct.GetPriceInfo();
        };
        if (Dt.Rows.Count > 0)
        {
            DataSet dtcopy = new DataSet();
            dtcopy.Merge(Dt);
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
        /*#CC02 END COMMENTED */
        /*#CC02 START COMMENTEDViewState["Table"] = null;
    }#CC02 END COMMENTED*/
    }
    protected void LBViewPrice_Click(object sender, EventArgs e)
    {
        Response.Redirect(Resources.Messages.PriceMaster, false);

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
                        imgbtnDelete.Visible = true;
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

}
