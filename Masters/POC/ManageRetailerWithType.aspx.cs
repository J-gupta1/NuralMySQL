using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using Cryptography;
using System.Data;

public partial class Masters_HO_POC_ManageRetailerWithType : PageBase
{
    int RetailerId = 0;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        
        try
        {
            if (!IsPostBack)
            {
                ddlSalesman.Items.Clear();
                ddlSalesman.Items.Insert(0,new ListItem("Select","0"));
                FillSalesChannelType();
               
               cmbsaleschannel_SelectedIndexChanged(cmbsaleschannel,new EventArgs());

                FillState();
                FillRetailerType();

                if ((Request.QueryString["RetailerId"] != null) && (Request.QueryString["RetailerId"] != ""))
                {
                    RetailerId = Convert.ToInt32(Convert.ToString(Crypto.Decrypt(Request.QueryString["RetailerId"], PageBase.KeyStr)));
                    if (RetailerId != 0)
                    {
                        PouplateRetailerDetail(RetailerId);
                        btnSubmit.Text = "Update";
                        //lblHeading.Text = "Edit Retailer Details";

                    }
                }
                else
                {


                }

            }
        }

        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);

        }
    }
    void FillState()
    {
        using (GeographyData ObjGeography = new GeographyData())
        {
            ObjGeography.SearchCondition = EnumData.eSearchConditions.Active;
            String[] StrCol = new String[] { "StateID", "StateName" };
            PageBase.DropdownBinding(ref cmbstate, ObjGeography.GetAllStateByParameters(), StrCol);

        };
    }
    void FillRetailerType()
    {
        using (RetailerData ObjRetaileType = new RetailerData())
        {
            ObjRetaileType.SearchCondition = EnumData.eSearchConditions.Active;
            String[] StrCol = new String[] { "ReatilerTypeID", "RetailerTypeName" };
            PageBase.DropdownBinding(ref cmbRetailerType, ObjRetaileType.GetAllRetaileType(), StrCol);

        };
    }
    void FillSalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {

            ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
            ObjSalesChannel.SearchType = EnumData.eSearchConditions.Active;
            ObjSalesChannel.BilltoRetailer = true;
            String[] StrCol = new String[] { "SalesChannelID", "SalesChannelName" };
            PageBase.DropdownBinding(ref cmbsaleschannel, ObjSalesChannel.GetSalesChannelInfo(), StrCol);
            if (Convert.ToInt32(PageBase.SalesChanelID) != 0)             //this will select the login user in combo
            {
                cmbsaleschannel.SelectedValue = PageBase.SalesChanelID.ToString();
                cmbsaleschannel.Enabled = false;
            }

        };
    }
   
    private bool ValidateControl (ref string ErrMessage)
    {


       
        if ((cmbsaleschannel.SelectedValue == "Select") ||
            (cmbstate.SelectedValue == "Select") || (cmbcity.SelectedValue == "Select")
             || (txtpincode.Text.Trim() == "")
            || (txtretailercode.Text.Trim() == "") || (txtretailername.Text.Trim() == "") || (txtAddress1.Text.Trim() == "")  || (txtmobile.Text.Trim() == "")
            || (txtcontactperson.Text.Trim() == "") || (txtcountersize.Text.Trim() == "")||ddlSalesman.SelectedValue=="0")
        {
            ErrMessage = Resources.Messages.MandatoryField;
            return false;

        }

        if (ServerValidation.IsMobileNo(txtmobile.Text.Trim(), true) > 0)
        {
            ErrMessage = Resources.Messages.Mobilevalidate;
            return false;

        }
        if (ServerValidation.IsValidEmail(txtemail.Text.Trim(),false) > 0)
        {
            ErrMessage = ErrMessage + Resources.Messages.Emailvalidate;

            return false;

        }

        if (ServerValidation.IsPinCode(txtpincode.Text.Trim(), true) > 0)
        {
            ErrMessage = ErrMessage + Resources.Messages.PinCodevalidate;
            return false;

        }
       
       
        return true;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }       //Pankaj Dhingra
            int Result = 0;
            string ErrorMsg="";
            if (!ValidateControl(ref ErrorMsg))
            {
                ucMessage1.ShowInfo(ErrorMsg);
                return;
            }

            using (RetailerData objRetailer = new RetailerData())
            {
                if ((Request.QueryString["RetailerId"] != null) && (Request.QueryString["RetailerId"] != ""))
                {
                    RetailerId = Convert.ToInt32(Crypto.Decrypt(Request.QueryString["RetailerId"], PageBase.KeyStr));
                    objRetailer.RetailerID = RetailerId;
                }
                else
                {
                    objRetailer.RetailerID = 0;
                   
                }
                objRetailer.RetailerTypeID = Convert.ToInt32(cmbRetailerType.SelectedValue);
                objRetailer.RetailerCode = txtretailercode.Text.Trim();
                objRetailer.RetailerName = txtretailername.Text.Trim();
                objRetailer.SalesChannelID = Convert.ToInt32(cmbsaleschannel.SelectedValue);
                objRetailer.Address1 = txtAddress1.Text.Trim();
                objRetailer.Address2 = txtAddress2.Text.Trim();
                objRetailer.StateID = Convert.ToInt16(cmbstate.SelectedValue);
                objRetailer.CityID = Convert.ToInt16(cmbcity.SelectedValue);
                objRetailer.AreaID = Convert.ToInt16(cmbArea.SelectedValue);
                if (txtcountersize.Text != "")
                {
                    objRetailer.CounterSize = txtcountersize.Text.Trim();
                }
                objRetailer.ContactPerson = txtcontactperson.Text.Trim();
                objRetailer.MobileNumber = txtmobile.Text.Trim();
                objRetailer.PhoneNumber = txtphone.Text.Trim();
                objRetailer.PinCode = txtpincode.Text.Trim();
                objRetailer.TinNumber = txttinno.Text.Trim();
                objRetailer.Status = chkactive.Checked;            
                objRetailer.Email = txtemail.Text.Trim();
               
                objRetailer.SalesmanID = Convert.ToInt32(ddlSalesman.SelectedValue);
                Result = objRetailer.InsertUpdateRetailerWithType();
                if (Result > 0 && (objRetailer.Error == null || objRetailer.Error == ""))
                {
                    if (RetailerId == 0)
                    {
                        ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    }
                    else
                    {
                        ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);
                    }
                   
                    ClearForm();
                    txtretailercode.Text = "autogenerated";
                }
                else
                {
                    if (objRetailer.Error != null && objRetailer.Error != "")
                    {
                        ucMessage1.ShowInfo(objRetailer.Error);
                    }
                    else
                    {
                        ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    }


                }

                if (Convert.ToInt32(PageBase.SalesChanelID) != 0)             //this will select the login user in combo
                {
                    cmbsaleschannel.SelectedValue = PageBase.SalesChanelID.ToString();
                    cmbsaleschannel.Enabled = false;
                }

            };
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    void ClearForm()
    {
        cmbRetailerType.SelectedValue = "0";
        cmbsaleschannel.SelectedValue = "0";
        cmbstate.SelectedValue = "0";
        cmbcity.SelectedValue = "0";
        cmbArea.SelectedValue = "0"; 
        txtAddress1.Text = "";
        txtAddress2.Text = "";
        txtemail.Text = "";
        txtphone.Text = "";
        txtpincode.Text = "";     
        txtretailercode.Text = "";
        txtretailername.Text = "";
        txtcontactperson.Text = "";
        btnSubmit.Text = "Submit";
        chkactive.Checked = true;
        //chkactive.Enabled = false;
        txtcountersize.Text="";
        cmbArea.Items.Clear();
        cmbArea.Items.Insert(0, new ListItem("Select", "0"));
        cmbcity.Items.Clear();
        cmbcity.Items.Insert(0, new ListItem("Select", "0"));
        //lblHeading.Text = "Manage Retailer";
        txtmobile.Text = "";
        txttinno.Text = "";
        ddlSalesman.SelectedValue = "0";
       

     
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ucMessage1.ShowControl = false;
        ClearForm();
        cmbsaleschannel.ClearSelection();
        if (Convert.ToInt32(PageBase.SalesChanelID) != 0)             //this will select the login user in combo
        {
            cmbsaleschannel.Items.FindByValue(Convert.ToString(PageBase.SalesChanelID)).Selected = true;
            cmbsaleschannel.Enabled = false;
        }

        txtretailercode.Text = "autogenerated";

    }
    protected void LBViewRetailer_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewRetailerWithType.aspx");
    }
    private void PouplateRetailerDetail(int RetailerID)
    {
        DataTable DtSalesChannelDetail;
        try
        {
            using (RetailerData ObjRetailer = new RetailerData())
            {

                ObjRetailer.RetailerID = RetailerID;
                DtSalesChannelDetail = ObjRetailer.GetRetailerInfoWithType();
            };
            if (DtSalesChannelDetail.Rows.Count > 0 && DtSalesChannelDetail != null)
            {
                txtAddress1.Text = DtSalesChannelDetail.Rows[0]["Address1"].ToString();
                txtAddress2.Text = DtSalesChannelDetail.Rows[0]["Address2"].ToString();
                cmbRetailerType.SelectedItem.Selected = false;
                cmbRetailerType.Items.FindByValue(DtSalesChannelDetail.Rows[0]["RetailerTypeID"].ToString()).Selected = true;
                cmbsaleschannel.SelectedItem.Selected = false;
                cmbsaleschannel.Items.FindByValue(DtSalesChannelDetail.Rows[0]["SalesChannelID"].ToString()).Selected = true;
                cmbsaleschannel.Enabled = false;
                cmbstate.SelectedItem.Selected = false;
                cmbstate.Items.FindByValue(DtSalesChannelDetail.Rows[0]["StateID"].ToString()).Selected = true;
                cmbstate_SelectedIndexChanged(cmbstate, new EventArgs());
                cmbcity.SelectedItem.Selected = false;
                cmbcity.Items.FindByValue(DtSalesChannelDetail.Rows[0]["CityID"].ToString()).Selected = true;
                cmbcity_SelectedIndexChanged(cmbstate, new EventArgs());
                cmbArea.SelectedItem.Selected = false;
                cmbArea.Items.FindByValue(DtSalesChannelDetail.Rows[0]["AreaId"].ToString()).Selected = true;
                txtcontactperson.Text = DtSalesChannelDetail.Rows[0]["contactperson"].ToString();
                txtpincode.Text = DtSalesChannelDetail.Rows[0]["PinCode"].ToString();
                txtphone.Text = DtSalesChannelDetail.Rows[0]["PhoneNumber"].ToString();
                txtmobile.Text = DtSalesChannelDetail.Rows[0]["MobileNumber"].ToString();
                txtcountersize.Text = DtSalesChannelDetail.Rows[0]["countersize"].ToString();
                txtemail.Text = DtSalesChannelDetail.Rows[0]["Email"].ToString();
                txtretailercode.Text = DtSalesChannelDetail.Rows[0]["retailercode"].ToString();
                txtretailername.Text = DtSalesChannelDetail.Rows[0]["retailername"].ToString();
                txttinno.Text = DtSalesChannelDetail.Rows[0]["tinnumber"].ToString();
                chkactive.Checked = Convert.ToBoolean(DtSalesChannelDetail.Rows[0]["Status"].ToString());
                cmbsaleschannel_SelectedIndexChanged(cmbsaleschannel, new EventArgs());
                ddlSalesman.Items.FindByValue(DtSalesChannelDetail.Rows[0]["SalesmanID"].ToString()).Selected = true; ;

            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void cmbstate_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            using (GeographyData ObjGeography = new GeographyData())
            {
                if (cmbstate.SelectedIndex > 0)
                {
                    ObjGeography.SearchCondition = EnumData.eSearchConditions.Active;
                    ObjGeography.StateId = Convert.ToInt16(cmbstate.SelectedValue);
                    cmbcity.Items.Clear();
                    cmbArea.Items.Clear();
                    cmbArea.Items.Insert(0, new ListItem("Select", "0"));
                    String[] StrCol = new String[] { "CityId", "CityName" };
                    PageBase.DropdownBinding(ref cmbcity, ObjGeography.GetAllCityByParameters(), StrCol);
                }
                else if (cmbstate.SelectedIndex == 0)
                {
                    cmbcity.Items.Clear();
                    cmbArea.Items.Clear();
                    cmbArea.Items.Insert(0, new ListItem("Select", "0"));
                    cmbcity.Items.Insert(0, new ListItem("Select", "0"));
                }

            };
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
  
    protected void cmbcity_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            using (GeographyData ObjGeography = new GeographyData())
            {
                if (cmbcity.SelectedIndex > 0)
                {
                    ObjGeography.SearchCondition = EnumData.eSearchConditions.Active;
                    ObjGeography.CityId = Convert.ToInt16(cmbcity.SelectedValue);
                    cmbArea.Items.Clear();
                    String[] StrCol = new String[] { "AreaID", "AreaName" };
                    PageBase.DropdownBinding(ref cmbArea, ObjGeography.GetAllAreaByParameters(), StrCol);
                }
                else if (cmbcity.SelectedIndex == 0)
                {
                    cmbArea.Items.Clear();
                    cmbArea.Items.Insert(0, new ListItem("Select", "0"));
                }

            };
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    //protected void rdoSelectMode_SelectedIndexChanged(object sender, EventArgs e)

    //{
    //    if (rdoSelectMode.SelectedValue == "1")
    //    {
    //        Response.Redirect("MangeRetailerUpload.aspx");
    //    }
    //}
    protected void cmbsaleschannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbsaleschannel.SelectedIndex !=0)
        {
             using (SalesmanData ObjSalesman = new SalesmanData())
        {
            ObjSalesman.Type = EnumData.eSearchConditions.Active;
            ObjSalesman.SalesChannelID = Convert.ToInt32(cmbsaleschannel.SelectedValue);
            String[] StrCol = new String[] { "SalesmanID", "Salesman" };
            PageBase.DropdownBinding(ref ddlSalesman, ObjSalesman.GetSalesmanInfo(), StrCol);

        };
        }
        else
        {
            ddlSalesman.Items.Clear();
            ddlSalesman.Items.Insert(0,new ListItem("Select","0"));
        }
    }
}
