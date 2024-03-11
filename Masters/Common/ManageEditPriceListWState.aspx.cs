using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using Cryptography;
using System.Data;
using DataAccess;

public partial class Masters_Common_ManageEditPriceListWState : PageBase
{
    Int32 intPriceListChangeLogID = 0,intStateId=0;
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        ucEffectiveDate.MinRangeValue = System.DateTime.Now.Date;
        ucEffectiveDate.RangeErrorMessage = "Invalid Date";

        if (Request.QueryString["StateID"] != null)
            intStateId = Convert.ToInt32(Request.QueryString["StateID"]);
        if (Request.QueryString["PriceListChangeLogID"] != null)
            intPriceListChangeLogID = Convert.ToInt32(Request.QueryString["PriceListChangeLogID"]);
        StyleCss.Attributes.Add("href", "~/" + strAssets + "/CSS/popup.css");
        if (!IsPostBack)
        {
            FillState();
            fillpricelist();
            if (intPriceListChangeLogID != 0)
                FillDefaultValues();
        }
    }
    protected void btnSubmitPrice_click(object sender, EventArgs e)
    {
        try
        {
            if (ddlInsertPriceList.SelectedValue == "0")
            {
                ucMessage1.ShowInfo("Please Select Price List");
                return;

            }
            using (MastersData ObjStatePricelist = new MastersData())
            {

                ObjStatePricelist.error = "";
                ObjStatePricelist.StateId = intPriceListChangeLogID>0?Convert.ToInt32(ddlStateName.SelectedValue):Convert.ToInt32(intStateId);
                ObjStatePricelist.Purpose = Convert.ToInt16(intPriceListChangeLogID == 0 ? 0 : 1);
                ObjStatePricelist.StatePriceListId = Convert.ToInt32(ddlInsertPriceList.SelectedValue);
                ObjStatePricelist.UniqueID = intPriceListChangeLogID;
                
                //ObjStatePricelist.StatePriceEffDate = ucEffectiveDate.Date;
                ObjStatePricelist.StatePriceEffDate = Convert.ToDateTime(ucEffectiveDate.Date).ToString("yyyy/MM/dd");
                ObjStatePricelist.CompanyId = PageBase.ClientId;    /* #C001 Added */
                int result = ObjStatePricelist.InsertStatePriceLists();
                if (result == 0)
                    ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                else if (result == 2)
                    ucMessage1.ShowError(Resources.GlobalMessages.DateValidate);
                else if (result == 3)
                    ucMessage1.ShowError(Resources.GlobalMessages.PriceListAlreadyExists);
                else if (result == 4)
                    ucMessage1.ShowError(Resources.GlobalMessages.PriceListExistsWSState);
                else if (result == 1)
                    ucMessage1.ShowError(ObjStatePricelist.error);

            }
        }
        catch (Exception ex)
        {

        }
    }

    private void fillpricelist()
    {
        using (ProductData objproduct = new ProductData())
        {
            try
            {
                DataTable dt = new DataTable();
                ddlInsertPriceList.Items.Clear();
                objproduct.CompanyId = PageBase.ClientId;/* #C001 Added */
                dt = objproduct.SelectAllPriceListInfo();
                String[] colArray = { "PriceListID", "PriceListName" };
                PageBase.DropdownBinding(ref ddlInsertPriceList, dt, colArray);
            }
            catch (Exception ex)
            {
                ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);

            }
        }
    }
    private void FillDefaultValues()
    {
        try{
            using (MastersData objmaster = new MastersData())
            {
                objmaster.StateSelectionMode = 4;
                objmaster.CompanyId = PageBase.ClientId;/* #C001 Added */
                objmaster.UniqueID = intPriceListChangeLogID;
                dt = objmaster.SelectStateInfoVer2();
                ddlStateName.SelectedValue = dt.Rows[0]["StateId"].ToString();
                ddlInsertPriceList.SelectedValue = dt.Rows[0]["PriceListID"].ToString();
                ucEffectiveDate.Date = dt.Rows[0]["FromDate"].ToString();
            }

        }
        catch(Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    
    void FillState()
    {
        try
        {
            using (MastersData obj = new MastersData())
            {
                obj.StateSelectionMode = 1;
                obj.StateCountryid = 0;
                obj.CompanyId = PageBase.ClientId;/* #C001 Added */
                dt = obj.SelectStateInfoVer2();
                String[] colArray = { "StateID", "StateName" };
                PageBase.DropdownBinding(ref ddlStateName, dt, colArray);
                ddlStateName.SelectedValue = Convert.ToString(intStateId);
                ddlStateName.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    private void FillPriceListInfo()
    {
        try
        {
            using (MastersData objmaster = new MastersData())
            {
                objmaster.UniqueID = intPriceListChangeLogID;
                objmaster.Condition = 2;
                int result = objmaster.DeleteStatePriceListInfo();
                if (result == 0)
                    ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                else
                    ucMessage1.ShowError(objmaster.error);

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
}
