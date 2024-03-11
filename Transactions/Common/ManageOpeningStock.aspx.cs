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


public partial class Transactions_Common_ManageOpeningStock : PageBase
    {
        protected string strSiteUrl = PageBase.siteURL;
        protected string strAssets = PageBase.strAssets;
        private bool IsOpeningdateEnable = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                GetvalidSession();
                Page.Header.DataBind();
                string strLogInUserName = Convert.ToString(Session["DisplayName"]) + "(" + Convert.ToString(Session["RoleName"]) + ")";
                lblUserNameDesc.Text = strLogInUserName;
                ucMsg.Visible = false;
                if (!IsPostBack)
                {
                    BindGrid();
                }
                bindAssets();
                FillDate();

            }
            catch (Exception ex)
            {
                PageBase.Errorhandling(ex);
                ucMsg.ShowError(ex.Message, GlobalErrorDisplay());
            }

              
            }
        void FillDate()
        {
            if (PageBase.BackDaysAllowedOpeningStock == 0)
            {
                ucDatePicker.Date = DateTime.Now.Date.ToString();
                ucDatePicker.TextBoxDate.Enabled = false;
                ucDatePicker.imgCal.Enabled = false;
            }
            else
            {

                ucDatePicker.TextBoxDate.Enabled = true;
                ucDatePicker.imgCal.Enabled = true;
                ucDatePicker.MinRangeValue = DateTime.Now.Date.AddDays(-PageBase.BackDaysAllowedOpeningStock);
                ucDatePicker.MaxRangeValue = DateTime.Now.Date;
                ucDatePicker.RangeErrorMessage = "Only " + PageBase.BackDaysAllowedOpeningStock + " Back days allowed";
            }
        }
        
        void bindAssets()
        {
            ImgSideLogo.ImageUrl = "~/" + strAssets + "/CSS/Images/zedsaleslogo.gif";
            hyplogo.ImageUrl = "~/" + strAssets + "/CSS/Images/innerlogo.gif";
            hypfooterlogo.ImageUrl = "~/" + strAssets + "/CSS/Images/footerimg.gif";
            hypfooterlogo.NavigateUrl = PageBase.redirectURL;
            hypfooterlogo.Target = "_blank";
        }
         
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsPageRefereshed == true)
                {
                    return;
                }
                if (!pageValidate())
                {
                    return;
                }
                using (SalesChannelData ObjSales = new SalesChannelData())
                {
                    ObjSales.SalesChannelID = PageBase.SalesChanelID;
                    ObjSales.OpeningStockDate =Convert.ToDateTime(ucDatePicker.Date);
                    ObjSales.XMLList = GenerateXML(gvStockEntry).ToString();
                    Int32 result = ObjSales.InsertSalesChannelOpeningStock();
                    switch (result)
                    {
                        case 0: ucMsg.ShowSuccess(Resources.Messages.InsertSuccessfull);
                                Clear();
                                //Response.Redirect("~/Default.aspx",false);    Pankaj Dhingra
                                Response.Redirect("~/Masters/SalesChannel/SalesChannelBranding.aspx", false);
                          
                            break;
                       
                        case 1:
                            ucMsg.ShowInfo(Resources.Messages.ErrorMsgTryAfterSometime);
                            break;
                        case 2:
                            ucMsg.ShowInfo(Resources.Messages.OpeningStockAlreadyEnter);
                            break;
                    }
                   
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
 

        void BindGrid()
        {
            DataTable DtSKU = null;
            ProductData ObjProduct = new ProductData();
            ObjProduct.Status = true ;
            ObjProduct.SalesChannelID = PageBase.SalesChanelID;
            ObjProduct.RequestType = 1;
            DtSKU = ObjProduct.GetSKUInfo();

            if (DtSKU.Rows.Count > 0 || DtSKU != null)
            {
               
                gvStockEntry.DataSource = DtSKU;
                gvStockEntry.DataBind();
            
            
          
     
            
            }
                
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
                StockDetailXML.AppendFormat("<SKUID>{0}</SKUID>{1}",objSKUID.Text.Trim(), Environment.NewLine);
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
            BindGrid();
            ucDatePicker.Date = "";
            FillDate();
        }
        void Clear()
        {
            BindGrid();
        }
}
