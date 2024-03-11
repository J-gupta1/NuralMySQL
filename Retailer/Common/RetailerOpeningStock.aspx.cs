using System;
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


public partial class Retailer_Common_RetailerOpeningStock : PageBase
{
    protected string strSiteUrl = PageBase.siteURL;
    protected string strAssets = PageBase.strAssets;
    private bool IsOpeningdateEnable = false;
    void SetMsg()
    {
        if (PageBase.BaseEntityTypeID == 3)
        {
            if (ddlRetailer.Items.Count == 1)
            {
                lblMsg.Text = "Opening Stock has been entered already";
                lblMsg.Visible = true;
                btnSave.Enabled = false;
                tblGridPanel.Visible = false;
            }
        }
        else if (PageBase.BaseEntityTypeID == 4)
        {
            if (ddlRetailer.Items.Count == 1)
            {
                lblMsg.Text = "Opening Stock has been entered already";
                lblMsg.Visible = true;
                btnSave.Enabled = false;
                tblGridPanel.Visible = false;
            }
            else if (ddlRetailer.Items.Count == 2)
            {
                ddlRetailer.Items.RemoveAt(0);
                btnSave.Enabled = true;
                tblGridPanel.Visible = true;
            }

        }
    }


    protected void btnInsertOp_Click(object sender, EventArgs e)
    {
      
        try
        {

            using (RetailerData ObjSales = new RetailerData())
            {
                DataTable Detail = new DataTable();
                using (CommonData ObjCommom = new CommonData())
                {
                    Detail = ObjCommom.GettvpTableOpeningStockSB();
                }

               // ObjSales.Error = "";
                ObjSales.RetailerID = Convert.ToInt32(ddlRetailer.SelectedValue);
                ObjSales.OpeningStockDate = Convert.ToDateTime(ucDatePicker.Date);
                int result = ObjSales.InsertOpeningStockWithZero(Detail);

                //int result = 0;
                if (result == 1)
                {
                    Session["DefaultRedirectionFlag"] = "2";
                    Session["OpeningStockdate"] = ucDatePicker.Date;
                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "cnfrm", "javascript:confirmation('../../default.aspx');", true);
                }
                else
                {
                    ucMsg.ShowError(ObjSales.Error);
                   
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());

        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {

            GetvalidSession();
            Page.Header.DataBind();
            string strLogInUserName = Convert.ToString(Session["DisplayName"]) + "(" + Convert.ToString(Session["RoleName"]) + ")";

            ucMsg.Visible = false;
            if (!IsPostBack)
            {
                //BindGrid();
                BindRetailer();

                lblPageHeading.Text = "Opening Stock for Retailer";
                lblPageHeading.Visible = true; 
                FillDate();
                SetMsg();
               

            }
            if (PageBase.MultipleBrandName != "")
            {
                LBSwitchToBrand.Text = "Current Brand " + PageBase.MultipleBrandName + " " + "(Switch To)";
                LBSwitchToBrand.Visible = true;
            }
            bindAssets();

           
        

        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            ucMsg.ShowError(ex.Message, GlobalErrorDisplay());
        }


    }
    protected void LBSwitchToBrand_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/SalesChannel/SalesChannelBranding.aspx", false);
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);

        }
    }
    void FillDate()
    {
        //if (PageBase.BackDaysAllowedOpeningStock == 0)
        //{
        //    ucDatePicker.Date = DateTime.Now.Date.ToString();
        //    ucDatePicker.TextBoxDate.Enabled = false;
        //    ucDatePicker.imgCal.Enabled = false;
        //}
        //else
        //{

        //    ucDatePicker.TextBoxDate.Enabled = true;
        //    ucDatePicker.imgCal.Enabled = true;
        //    ucDatePicker.MinRangeValue = DateTime.Now.Date.AddDays(-PageBase.BackDaysAllowedOpeningStock);
        //    ucDatePicker.MaxRangeValue = DateTime.Now.Date;
        //    ucDatePicker.RangeErrorMessage = "Only " + PageBase.BackDaysAllowedOpeningStock + " Back days allowed";
        //}

        ucDatePicker.Date = DateTime.Now.Date.ToString();
        ucDatePicker.MinRangeValue = DateTime.Now.Date;
        ucDatePicker.MaxRangeValue = DateTime.Now.Date;
        ucDatePicker.RangeErrorMessage = "Date should not be greater than todays.";


    }

    void BindRetailer()
    {
        ddlRetailer.Items.Clear();
        ddlRetailer.Items.Insert(0, new ListItem("Select", "0"));

        using (RetailerData ObjRetailer = new RetailerData())
        {
            ObjRetailer.UserID = PageBase.UserId;
            ObjRetailer.OtherEntityID = Convert.ToInt16( PageBase.BaseEntityTypeID);
            ObjRetailer.CompanyId = PageBase.ClientId;
            ObjRetailer.ISOpeningStock = 0;
            string[] str = { "RetailerID", "Retailer" };
            DataTable dt =ObjRetailer.GetRetailerByOrgHeirarchy();
            PageBase.DropdownBinding(ref ddlRetailer,dt , str);
        };
    }


    void bindAssets()
    {
        //ImgSideLogo.ImageUrl = "~/" + strAssets + "/CSS/Images/zedsaleslogo.gif";
        //hyplogo.ImageUrl = "~/" + strAssets + "/CSS/Images/innerlogo.gif";
        hypfooterlogo.ImageUrl = "~/" + strAssets + "/CSS/Images/footerimg.gif";
        hypfooterlogo.NavigateUrl = PageBase.redirectURL;
        hypfooterlogo.Target = "_blank";
    }


    DataTable getOpeningStockTable(DataTable dtSource)
    {

        DataTable Detail = new DataTable();
        using (CommonData ObjCommom = new CommonData())
        {
            Detail = ObjCommom.GettvpTableOpeningStockSB();
        }


        foreach (DataRow dr in dtSource.Rows)
        {
            DataRow drow = Detail.NewRow();
            drow["SKUCode"] = dr["SKUCode"].ToString().Trim();
            drow["Serial#1"] = dr["serialno"].ToString().Trim();
            drow["BatchNo"] = dr["BatchNo"].ToString().Trim();
            drow["Quantity"] = Convert.ToInt32(dr["Quantity"]);
            drow["StockBinType"] = Convert.ToString(dr["StockType"]);
            Detail.Rows.Add(drow);
        }
        Detail.AcceptChanges();

        return Detail;

    }

    protected void Page_PreRender(object s, EventArgs args)
    {
       

        btnSave_Click(btnSave, null);
       // ddlRetailer.Enabled = ddlRetailer.Items.Count > 2;
        //if (ddlRetailer.Items.Count == 2)
        //{
        //    ddlRetailer.SelectedIndex = 1;
        //}
        

     
    
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //if (IsPageRefereshed == true)
            //{
            //    return;
            //}
            // if (!pageValidate())
            // {
            //   return;
            // }


            DataTable Dt = new DataTable();
            if (PartLookupClientSide1.SubmittingTable.Rows.Count > 0)
            {
                Dt = PartLookupClientSide1.SubmittingTable;
            }
            else
            {
                return;
            }

         

            DataTable dtSource = getOpeningStockTable(Dt);

            using (SalesChannelData ObjSales = new SalesChannelData())
            {

                ObjSales.Error = "";
                ObjSales.RetailerID = Convert.ToInt32(ddlRetailer.SelectedValue);
                ObjSales.OpeningStockDate = Convert.ToDateTime(ucDatePicker.Date);
                ObjSales.InsertOpeningStockForRetailer(dtSource);
                if (ObjSales.XMLList != null && ObjSales.XMLList != string.Empty)
                {
                    ucMsg.XmlErrorSource = ObjSales.XMLList;
                    return;
                }
                else if (ObjSales.Error != null && ObjSales.Error != "")
                {
                    ucMsg.ShowError(ObjSales.Error);
                    return;
                }

                PartLookupClientSide1.IsBlankDataTable = true;
                PartLookupClientSide1.SubmittingTable = new DataTable();

                Session["DefaultRedirectionFlag"] = "2";
                Session["OpeningStockdate"] = ucDatePicker.Date;
                ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "cnfrm", "javascript:confirmation('../../default.aspx');", true);


                BindRetailer();
                SetMsg();
                ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                

                
                Clear();
                ddlRetailer.SelectedIndex = 0;

            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);

        }
    }

    bool pageValidate()
    {
        if (ServerValidation.IsDate(ucDatePicker.Date, true) != 0)
        {
            ucMsg.ShowInfo(Resources.Messages.MandatoryField);
            return false;
        }

        if (Convert.ToDateTime(ucDatePicker.Date) > DateTime.Now.Date)
        {
            ucMsg.ShowInfo("Date should be less than or equal to current date.");
            return false;
        }

        return true;
    }



    public StringBuilder GenerateXML(GridView gv)
    {

        StringBuilder StockDetailXML = new StringBuilder();
        StockDetailXML.AppendLine("<table>");
        foreach (GridViewRow gvRow in gv.Rows)
        {
            Label objSKUID = ((Label)gvRow.FindControl("lblSKUID"));
            TextBox objOpeningStock = ((TextBox)gvRow.FindControl("txtOpeningStock"));

            StockDetailXML.AppendLine("<rowse>");
            StockDetailXML.AppendFormat("<SKUID>{0}</SKUID>{1}", objSKUID.Text.Trim(), Environment.NewLine);
            StockDetailXML.AppendFormat("<QUANTITY>{0}</QUANTITY>{1}", objOpeningStock.Text.Trim(), Environment.NewLine);
            StockDetailXML.AppendLine("</rowse>");
        }
        StockDetailXML.AppendLine("</table>");
        return StockDetailXML;
    }

    protected void gvStockEntry_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ucDatePicker.Date = "";
        FillDate();
        PartLookupClientSide1.SubmittingTable = new DataTable();
        PartLookupClientSide1.IsBlankDataTable = true;
    }
    void Clear()
    {

        FillDate();
    }





    protected void ddlRetailer_SelectedIndexChanged(object sender, EventArgs e)
    {
        PartLookupClientSide1.IsBlankDataTable = true;
        PartLookupClientSide1.SubmittingTable = new DataTable();
    }
}
