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


public partial class Retailer_Specific_RetailerSalesEntry : PageBase
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

          
           //// ucMsg.Visible = false;
            if (!IsPostBack)
            {

                //ViewState["SalesBackDate"]= Convert.ToString(DateTime.Now.AddDays(-Convert.ToDouble(Session["RETAILERBACKDATESALE"])));
                
                ucDtPickerSalesDate.MaxRangeValue = DateTime.Now;
               // ucDtPickerSalesDate.RangeErrorMessage = "Range should between " + Convert.ToDouble(Session["RETAILERBACKDATESALE"]).ToString() + " days back and todays";
                ucDtPickerSalesDate.RangeErrorMessage = "Range should between " + Convert.ToDouble(Session["RETAILERBACKDATESALE"]).ToString() + " days back and todays";

               

                bindSearchDate();
                BindRetailer();

                AutoCompleteExtenderModel5.ContextKey = ddlRetailer.SelectedValue;
                hdnRetailerID.Value = ddlRetailer.SelectedValue;

                BindISP(Convert.ToInt16(hdnRetailerID.Value));

                if (PageBase.BaseEntityTypeID != 4)
                {
                    if (ddlISPByRetailer.Items.Count == 1)
                    {
                        hdnActivationDate.Value = Convert.ToString(DateTime.Now.AddDays(-Convert.ToDouble(Session["RETAILERBACKDATESALE"])).ToShortDateString());
                        hdnDeActivationDate.Value = DateTime.Now.ToShortDateString();
                    }
                    else
                    {
                        hdnActivationDate.Value = "";
                        hdnDeActivationDate.Value = "";
                    }
                }
                else
                {
                    hdnActivationDate.Value = "";
                    hdnDeActivationDate.Value = "";
                }

                //lblPageHeading.Text = "Sales Entry for Retailer";
                //lblPageHeading.Visible = true;
                tblGridPanel.Visible = false;
               
                

            }
            if (PageBase.MultipleBrandName != "")
            {
               // LBSwitchToBrand.Text = "Current Brand " + PageBase.MultipleBrandName + " " + "(Switch To)";
               // LBSwitchToBrand.Visible = true;
            }
            bindAssets();

            if (ViewState["SalesBackDate"] != null)
            {
               // ucDtPickerSalesDate.MinRangeValue = Convert.ToDateTime(ViewState["SalesBackDate"].ToString());
                ucDtPickerSalesDate.MaxRangeValue = DateTime.Now;
                ucDtPickerSalesDate.RangeErrorMessage = "Date should not be less than " + Convert.ToDateTime(ViewState["SalesBackDate"].ToString()).ToShortDateString() + " or greater than todays";
                ucDtPickerSalesDate.RangeErrorMessage = "Date should not be greater than todays";
            }  


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
  

    void BindRetailer()
    {
        ddlRetailer.Items.Insert(0, new ListItem("Select", "0"));

        using (RetailerData ObjRetailer = new RetailerData())
        {
            ObjRetailer.UserID = PageBase.UserId;
            ObjRetailer.CompanyId = PageBase.ClientId;
            ObjRetailer.OtherEntityID = Convert.ToInt16( PageBase.BaseEntityTypeID);
            string[] str = { "RetailerID", "Retailer" };
            DataTable d = ObjRetailer.GetRetailerByOrgHeirarchy();
            PageBase.DropdownBinding(ref ddlRetailer, d, str);

            //if (ddlRetailer.Items.Count == 2)
            //{
            //    ddlRetailer.SelectedIndex = 1;
            //    ddlRetailer.Enabled = false;
            //    //btnReset.Visible = false;
            //    bindGrid();
            //}


           // ViewState["ISPIDLogined"] = d.Rows.Count > 0 ? d.Rows[0]["ISPIDLogined"].ToString() : "-1";

           
        };
    }

    void BindISP(int retailerid)
    {
        if (ddlISPByRetailer.Items.Count > 0)
                ddlISPByRetailer.Items.Clear();

       ddlISPByRetailer.Items.Insert(0, new ListItem("Select", "0"));

        using (RetailerData ObjRetailer = new RetailerData())
        {
            ObjRetailer.RetailerID = retailerid;
            ObjRetailer.UserID = PageBase.UserId;
            ObjRetailer.CompanyId = PageBase.ClientId;
           // ObjRetailer.Status = true;
            string[] str = { "ISPID", "ISPNameWithCode" };
            DataTable dSource = ObjRetailer.GetISPsInfoByRetailer();
           // PageBase.DropdownBinding(ref ddlISPByRetailer, dSource, str);

            ListItem li;


           

            foreach (DataRow dr in dSource.Rows)
            {
                li = new ListItem(dr["ISPNameWithCode"].ToString(), dr["ISPID"].ToString());
               li.Attributes.Add("ActivationDate", Convert.ToDateTime(dr["ActivationDate"].ToString()).ToShortDateString());
               li.Attributes.Add("DeActivationDate", Convert.ToDateTime(dr["DeActivationDate"].ToString()).ToShortDateString());
              
                ddlISPByRetailer.Items.Add(li);
            }


            SetControls();


            //if (ddlISPByRetailer.Items.Count == 2)
            //{
            //    ddlISPByRetailer.SelectedIndex = 1;
            //    ObjRetailer.ISPID = Convert.ToInt32(ddlISPByRetailer.SelectedValue);
            //    ObjRetailer.RetailerID = Convert.ToInt32(ddlRetailer.SelectedValue);
            //    DataTable dtISPinfo = ObjRetailer.GetISPsInfo();

            //    if (PageBase.OtherEntityType == 2)
            //    {
            //        ViewState["SalesBackDate"] = dtISPinfo.Rows[0]["ActivationDate"].ToString();
            //    }
            //    ddlISPByRetailer.Enabled = false;


            //}
            if (ddlISPByRetailer.Items.Count == 1)
            {
                reqISP.Visible = false;
                reqISP.Enabled = false;
                ddlISPByRetailer.Enabled = false;
            }
            else
            {
                reqISP.Visible = true;
                reqISP.Enabled = true;
            }



        };

    }

    private void SetControls()
    {
        if (PageBase.BaseEntityTypeID == 4)/* ISP Login */
        {

            if (ddlISPByRetailer.Items.FindByValue(ViewState["ISPIDLogined"].ToString()) == null)
            {

                lblMsg.Text = "There is no active retailer mapped to the ISP.";
                lblMsg.Visible = true;
                tblGridPanel.Visible = false;
                btnProceed.Visible = false;
                // btnReset.Visible = false;
            }
            else
            {
                btnProceed.Visible = true;
                btnReset.Visible = true;
                lblMsg.Visible = false;
                tblGridPanel.Visible = true;
            }
        }
    }


    void bindAssets()
    {
        //ImgSideLogo.ImageUrl = "~/" + strAssets + "/CSS/Images/zedsaleslogo.gif";
        //hyplogo.ImageUrl = "~/" + strAssets + "/CSS/Images/innerlogo.gif";
        //hypfooterlogo.ImageUrl = "~/" + strAssets + "/CSS/Images/footerimg.gif";
       // hypfooterlogo.NavigateUrl = PageBase.redirectURL;
      //  hypfooterlogo.Target = "_blank";
    }

    
   
    void Clear()
    {

        btnProceed.Visible = true;
        txtMobileNo.Text = "";
        txtFirstName.Text = "";
        txtMiddleName.Text = "";
        txtLastName.Text = "";
        txtIMEI.Text = "";
        txtBatchcode.Text = "";
        txtQuantity.Text = "";
        ucDtPickerSalesDate.TextBoxDate.Text = "";
        txtSkuName.Text = "";
       // ddlISPByRetailer.SelectedIndex = 0;
    }





    protected void ddlRetailer_SelectedIndexChanged(object sender, EventArgs e)
    {
       
        hdnRetailerID.Value = ((DropDownList)sender).SelectedValue;
        BindISP(Convert.ToInt32(hdnRetailerID.Value));
        AutoCompleteExtenderModel5.ContextKey = hdnRetailerID.Value;
        bindGrid();
        ddlRetailer.Enabled = false;
       

    }

    void bindSearchDate()
    {

        ddlSearchDate.Items.Add(new ListItem("For Today", DateTime.Now.ToString()));
        ddlSearchDate.Items.Add(new ListItem("Last 3 days", DateTime.Now.AddDays(-3).ToString()));
        ddlSearchDate.Items.Add(new ListItem("Last 7 days", DateTime.Now.AddDays(-7).ToString()));
        ddlSearchDate.Items.Add(new ListItem("Last 15 days", DateTime.Now.AddDays(-15).ToString()));
 
        
        ddlSearchDate.Items[1].Selected = true;



    }

   

    private void LoadData(DataTable MyData)
    {




        StringBuilder sb = new StringBuilder();
        DataTable dt = MyData;
        int i = 0;
        if (dt != null)
            if (dt.Rows.Count > 0)
                foreach (DataRow dr in dt.Rows)
                {

                    sb.Append("<tr>");
                    sb.Append("<td>");
                    sb.Append(dr["SalesDate"]);
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(dr["IEMI"]);
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(dr["BatchCode"]);
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(dr["SKUName"]);
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(dr["Quantity"]);
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(dr["FirstName"]);
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(dr["MiddleName"]);
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(dr["LastName"]);
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(dr["MobileNo"]);
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(dr["ISPName"]);
                    sb.Append("</td>");
                    sb.Append("<td >");
                    sb.Append("</td>");

                    sb.Append("</tr>");

                }

        ltData.Text = sb.ToString();

        // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sdf", "document.getElementById('spn').innerText='"+sb.ToString()+ "';", true);
        // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sdf", "getElementById('spn').innerText='" + sb.ToString() + "';", true);

    }

    void bindGrid()
    {


        try
        {

            using (TertiarySales obj = new TertiarySales())
            {

                obj.SalesFromID = Convert.ToInt32(ddlRetailer.SelectedValue);
              obj.RecordCreationDate = Convert.ToDateTime(ddlSearchDate.SelectedValue);
              //obj.RecordCreationDate = DateTime.Now;
                DataTable dt = obj.GetTertiarySalesByRetailer();
                LoadData(dt);


            }
        }
        catch (Exception ex)
        {
            
            throw;
        }




    }


    protected void btnInsert_Click(object sender, EventArgs e)
    {

       

        try
        {
           
            if (Convert.ToDateTime(ucDtPickerSalesDate.Date) > System.DateTime.Now)
            {
                ucMsg.ShowError("Future date is not allowed");
                return;
            }
            using (TertiarySales ObjSales = new TertiarySales())
            {

               
                ObjSales.SalesFromID = Convert.ToInt32(ddlRetailer.SelectedValue);
                ObjSales.InvoiceDate = Convert.ToDateTime(ucDtPickerSalesDate.Date);
                ObjSales.IEMI = txtIMEI.Text;
                ObjSales.BatchCode = txtBatchcode.Text;
                ObjSales.Quantity =Convert.ToInt32(txtQuantity.Text);
                ObjSales.ISPID = Convert.ToInt32(ddlISPByRetailer.SelectedValue);
                ObjSales.CreatedBy = PageBase.UserId;
                ObjSales.FirstName = txtFirstName.Text.Trim();
                ObjSales.MiddleName = txtMiddleName.Text.Trim();
                ObjSales.LastName = txtLastName.Text.Trim();
                ObjSales.Mobile = txtMobileNo.Text.Trim();
                ObjSales.SKUName = txtSkuName.Text.Trim();
                ObjSales.CompanyId = PageBase.ClientId;

                short result = ObjSales.InsertTertairySalesForRetailer();
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
                if (result == 255)
                {
                    ucMsg.ShowInfo("Invalid sales date entered.");
                    return;
                }
                else if (result == 254)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "maintainSKUName", "VaidateIEMIServer();", true);
                    ucMsg.ShowInfo("IEMI entered already.");
                    return;
                }

                ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                Clear();
               // ddlRetailer.SelectedIndex = 0;
                bindGrid();

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message);
        }

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
       ddlRetailer.SelectedValue = "0";
       ddlRetailer.Enabled = true;
        tblGridPanel.Visible = false;
        ucMsg.Visible = false;
        SetControls();
        Clear();
        
       
    }
    protected void ddlSearchDate_SelectedIndexChanged(object sender, EventArgs e)
    {

        ddlRetailer_SelectedIndexChanged(ddlRetailer, null);

    }
    protected void btnProceed_Click(object sender, EventArgs e)
    {
      //  updtDataTable.Update();

      
            tblGridPanel.Visible = true;
            hdnRetailerID.Value = ddlRetailer.SelectedValue;
            BindISP(Convert.ToInt32(hdnRetailerID.Value));
            AutoCompleteExtenderModel5.ContextKey = hdnRetailerID.Value;
            AutoCompleteExtender1.ContextKey = hdnRetailerID.Value;
            bindGrid();
            ddlRetailer.Enabled = false;
            ((Button)sender).Visible = false;

    }
  
}
