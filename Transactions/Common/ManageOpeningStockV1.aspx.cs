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


public partial class Transactions_Common_ManageOpeningStockV1 : PageBase
    {
        protected string strSiteUrl = PageBase.siteURL;
        protected string strAssets = PageBase.strAssets;
        private bool IsOpeningdateEnable = false;
        protected void Page_Load(object sender, EventArgs e)
        {
         
            try
            {
                lblInfoDate.Text = DateTime.Now.AddDays(-2).ToString("dd MMM yyyy");
                GetvalidSession();
                Page.Header.DataBind();
                string strLogInUserName = Convert.ToString(Session["DisplayName"]) + "(" + Convert.ToString(Session["RoleName"]) + ")";
            
                ucMsg.Visible = false;
                if (!IsPostBack)
                {
                    //BindGrid();
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
        protected void rdModelList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdModelList.SelectedValue == "0")
                Response.Redirect("~/Transactions/CommanSerial/ManageOpeningStockSerialWise.aspx");
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
                drow["StockBinType"] = dr["StockType"].ToString().Trim();
                Detail.Rows.Add(drow);
            }
            Detail.AcceptChanges();

            return Detail;

        }

        protected void Page_PreRender(object s, EventArgs args)
        {
        
            btnSave_Click(btnSave, null);
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
                    ObjSales.SalesChannelID = PageBase.SalesChanelID;
                    ObjSales.OpeningStockDate = Convert.ToDateTime(ucDatePicker.Date);
                    ObjSales.InsertOpeningStock(dtSource);
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
                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    Session["DefaultRedirectionFlag"] = "0";
                    Session["OpeningStockdate"] = ucDatePicker.Date;
                    Response.Redirect("~/Masters/SalesChannel/SalesChannelBranding.aspx", false);
                    Clear();
                    
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
            ucDatePicker.Date = "";
            FillDate();
            PartLookupClientSide1.SubmittingTable = new DataTable();
            PartLookupClientSide1.IsBlankDataTable = true;
        }
        void Clear()
        {
            
        }
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
               
                if (!pageValidate())
                {
                    return;
                }

                using (SalesChannelData ObjSales = new SalesChannelData())
                {

                    ObjSales.Error = "";
                    ObjSales.SalesChannelID = PageBase.SalesChanelID;
                    ObjSales.OpeningStockDate = Convert.ToDateTime(ucDatePicker.Date);
                    int result = ObjSales.InsertOpeningStockWithZero();
                    if (result == 0)
                    {
                        ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                        Session["DefaultRedirectionFlag"] = "0";
                        Session["OpeningStockdate"] = ucDatePicker.Date;
                        Response.Redirect("~/Masters/SalesChannel/SalesChannelBranding.aspx", false);
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

        

    

}
