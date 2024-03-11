using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;
using Cryptography;

public partial class Reports_Retailer_SerialWiseReportRetailer : PageBase
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.ShowControl = false;
            if (!IsPostBack)
            {
                pageInfo();
                HideControls();
                FillsalesChannelType();
                BindRetailer();

                //FillBrand();

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
    //void FillBrand()
    //{
    //    using (ProductData ObjProduct = new ProductData())
    //    {
    //        ObjProduct.SearchType = EnumData.eSearchConditions.Active;
    //        String[] StrCol = new String[] { "BrandID", "BrandName" };
    //        PageBase.DropdownBinding(ref cmbBrandName, ObjProduct.GetAllBrandByParameters(), StrCol);

    //    };
    //}

    void FillsalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.Type = 5;           //For Mapping
            if (Convert.ToInt32(PageBase.SalesChanelTypeID) != 0)
            {
                ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
            }

            string[] str = { "SalesChannelTypeID", "SalesChannelTypeName" };
          //  PageBase.DropdownBinding(ref cmbsaleschanneltype, ObjSalesChannel.GetSalesChannelTypeV3(), str);
        };
    }
    void HideControls()
    {
        ExportToExcel.Visible = false;
        //GridSalesChannel.Visible = false;
        // UpdGrid.Update();

    }
    public string fncChangePwd(string vPassword, string vPasswordSalt)
    {
        string vMailPassword = string.Empty;
        try
        {
            using (Authenticates objAuth = new Authenticates())
            {
                vMailPassword = objAuth.DecryptPassword(vPassword, vPasswordSalt);
            };
        }
        catch (Exception ex)
        {


            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
        return vMailPassword;
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlRetailerName.SelectedIndex == 0)
            {
                ucMessage1.ShowInfo("Please select Retailer");
                HideControls();
                return;
            }
            bindGrid();
            //FillGrid();
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    void FillGrid()
    {
        //  ViewState["Table"] = null;

        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {

            //ObjSalesChannel.SalesChannelName = txtsaleschannelname.Text.Trim();
            //ObjSalesChannel.SalesChannelCode = txtsaleschannelcode.Text.Trim();
            //if (cmbsaleschanneltype.SelectedValue != "0")
            //{

            //    ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(cmbsaleschanneltype.SelectedValue);
            //}
            //if (cmbBrandName.SelectedValue != "0")
            //{
            //    ObjSalesChannel.Brand = Convert.ToInt16(cmbBrandName.SelectedValue);
            //}
            //DtSalesChannelDetail = ObjSalesChannel.GetSalesChannelInfo();
        };
       
    }

    public string LoginStatus(Int16 IsActive)
    {
        string imgUrl = PageBase.siteURL + "/" + strAssets + "/CSS/images/status_offline.png";
        if (IsActive == 1)
        { imgUrl = PageBase.siteURL + "/" + strAssets + "/CSS/images/status_online.png"; }
        return imgUrl;
    }
    public string LoginToolTip(Int16 IsActive)
    {
        string ToolTip = "Online";
        if (IsActive == 0)
        { ToolTip = "Offline"; }
        return ToolTip;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

        ucMessage1.ShowControl = false;
        //cmbsaleschanneltype.SelectedValue = "0";
       // txtsaleschannelname.Text = "";
       // txtsaleschannelcode.Text = "";
       // pnlBrand.Visible = false;
       // cmbBrandName.SelectedValue = "0";
        HideControls();
        dvhide.Visible = false;
    }
    protected void ExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string Password = string.Empty;
            FillGrid();
            //if (ViewState["Table"] != null)
            //{
            DataTable dt = null;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    using (Authenticates ObjAuth = new Authenticates())
                    {
                        Password = ObjAuth.DecryptPassword(Convert.ToString(dr["Password"]), Convert.ToString(dr["PasswordSalt"]));
                    };
                    dr["Password"] = Password;
                }

            }
            string[] DsCol = new string[] { "SalesChannelName", "LoginName", "Password", "SalesChannelCode", "SalesChannelTypeName", "ParentName", "LocationName", "ContactPerson", "Address1", "Address2", "StateName", "CityName", "DistrictName", "AreaName", "Fax", "PinCode", "CstNumber", "TinNumber", "MobileNumber", "StatusValue", "PhoneNumber", "Email", "BussinessStartDate", "OpeningStockEntered", "GroupParentName", "Multilocation" };
            DataTable DsCopy = new DataTable();
            dt = dt.DefaultView.ToTable(true, DsCol);
            dt.Columns["StatusValue"].ColumnName = "Status";
            dt.Columns["LocationName"].ColumnName = "Repo.Hierarchy Name";
            if (dt.Rows.Count > 0)
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "SalesChannelList";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dtcopy, FilenameToexport);
                //  ViewState["Table"] = null;
            }
            else
            {
                ucMessage1.ShowError(Resources.Messages.NoRecord);

            }
            // ViewState["Table"] = null;
            //   }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
    protected void GridSalesChannel_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int Result = 0;
        Int32 SalesChannelId = Convert.ToInt32(e.CommandArgument);
        try
        {
            if (e.CommandName == "Active")
            {

                if (SalesChannelId > 0)
                {
                    using (SalesChannelData ObjSalesChannel = new SalesChannelData())
                    {
                        ObjSalesChannel.SalesChannelID = SalesChannelId;
                        Result = ObjSalesChannel.UpdateStatusSalesChannelInfo();
                    };
                    if (Result > 0)
                    {
                        ucMessage1.ShowSuccess(Resources.Messages.StatusChanged);
                    }
                    else
                    {
                        ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    }
                    FillGrid();
                }
            }
            if (e.CommandName == "Online")
            {
                if (SalesChannelId > 0)
                {
                    using (UserData ObjUser = new UserData())
                    {
                        Int32 UserId = Convert.ToInt32(e.CommandArgument);
                        ObjUser.UserID = UserId;
                        Result = ObjUser.UpdateUserLoginStatus();
                    };
                    if (Result > 0)
                    {
                        ucMessage1.ShowSuccess(Resources.Messages.LogOff);
                    }
                    else
                    {
                        ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    }
                    FillGrid();
                }
            }
            if (e.CommandName.ToLower() == "unlock")
            {

                if (SalesChannelId > 0)
                {
                    using (UserData ObjUser = new UserData())
                    {
                        Int32 UserId = Convert.ToInt32(e.CommandArgument);
                        ObjUser.UserID = UserId;
                        ObjUser.ActionId = 1;
                        Result = ObjUser.UpdateUserLoginStatus();
                    };
                    if (Result > 0)
                    {
                        ucMessage1.ShowSuccess(Resources.Messages.LockedOut);
                    }
                    else
                    {
                        ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    }
                    FillGrid();

                }

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
        if (e.CommandName == "cmdEdit")
        {

            Response.Redirect("ManageSalesChannel.aspx?SalesChannelId=" + Server.UrlEncode(Crypto.Encrypt(Convert.ToString(SalesChannelId), PageBase.KeyStr)));
        }
    }

    void pageInfo()
    {
       // this.GridSalesChannel.Columns[0].HeaderText = Resources.Messages.SalesEntity + " Name";
       // this.GridSalesChannel.Columns[3].HeaderText = Resources.Messages.SalesEntity + " Code";
       // this.GridSalesChannel.Columns[7].HeaderText = Resources.Messages.SalesEntity + " Type";
    }

    
    protected void GridSalesChannel_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
           GridRetailer.PageIndex = e.NewPageIndex;
            bindGrid();
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
    protected void LBAddSalesChannel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageSalesChannel.aspx");
    }
    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            //cmbsaleschanneltype.SelectedValue = "0";
            //txtsaleschannelcode.Text = "";
            //txtsaleschannelname.Text = "";
            //cmbBrandName.SelectedValue = "0";
            bindGrid();
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }

    protected void cmbsaleschanneltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        //CheckBrandSalesChannelMapping();
    }
   
    void BindRetailer()
    {
        ddlRetailerName.Items.Insert(0, new ListItem("Select", "0"));

        using (RetailerData ObjRetailer = new RetailerData())
        {
            ObjRetailer.UserID = PageBase.UserId;
            ObjRetailer.SalesChannelID = PageBase.SalesChanelID;
            ObjRetailer.OtherEntityID = Convert.ToInt16(PageBase.BaseEntityTypeID);
            string[] str = { "RetailerID", "Retailer" };
            DataTable d = ObjRetailer.GetRetaierListforReport();
            PageBase.DropdownBinding(ref ddlRetailerName, d, str);

            //if (ddlRetailerName.Items.Count == 2)
            //{
            //    ddlRetailerName.SelectedIndex = 1;
            //    ddlRetailerName.Enabled = false;
            //    //btnReset.Visible = false;
            //    //bindGrid();
            //}

        };
    }

    
    void bindGrid()
    {
       try
        {
            using (TertiarySales obj = new TertiarySales())
            {
                obj.SalesFromID = Convert.ToInt16(ddlRetailerName.SelectedValue);
                DataTable dt = obj.GetTertiarySalesByRetailerForReport();
                if (dt != null && dt.Rows.Count > 0)
                {
                    //  ViewState["Table"] = DtSalesChannelDetail;
                    ExportToExcel.Visible = false;
                    GridRetailer.Visible = true;
                    GridRetailer.DataSource = dt;
                    GridRetailer.DataBind();
                    dvhide.Visible = true;

                }
                else
                {
                    HideControls();
                    GridRetailer.Visible = false;
                    GridRetailer.DataSource = null;
                    GridRetailer.DataBind();
                    ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                    dvhide.Visible = false;
                }
                //GridRetailer.DataSource = dt;
                //GridRetailer.DataBind();

            }
        }
        catch (Exception ex)
        {

            throw;
        }




    }
}
