#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*/
 * ================================================================================================
 * ================================================================================================
 * COPYRIGHT (c) 2016 Zed Axis Technologies (P) Ltd.
 * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART,
 * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
 * ================================================================================================
 * Created By : Rakesh Raj
 * Created On : 25 July 2018
 * Description : Display Scheme Details added from Mobile App
 * ================================================================================================
 * Change Log: 
 * ------------- 
 * DD-MMM-YYYY, Name, #CCXX, Description
 * ====================================================================================================
 
 */
#endregion

using System;
using System.Data;
using BussinessLogic;

public partial class Masters_SalesMan_ViewSchemeDetailsAPI : PageBase
{
    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            ucMsg.Visible = false;
            if (!IsPostBack)
            {
                FillsalesChannelType();
                FillSalesmanName();
                GetSearchData(1);
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }

    }
    protected void btnExprtToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            GetSearchData(-1);
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }

    }
    protected void btnSearchUser_Click(object sender, EventArgs e)
    {
        if (ddlFOSTSM.SelectedIndex == 0 & ddlSalesChannelType.SelectedIndex == 0
            & txtCompanyName.Text.Trim().Length == 0 & ucToDate.Date == "" & ucFromDate.Date == "")
        { 
            //Please select at least one search criteria!
            ucMsg.Visible = true;
            ucMsg.ShowInfo(Resources.Messages.SearchCriteriaBlank);
            return;
        }

        GetSearchData(1);

    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        ClearForm();
        GetSearchData(1);
    }

    #endregion

    #region Functions

    void ClearForm()
    {
      
        txtCompanyName.Text = ""; //sales channel name
        ViewState["CurrentPage"] = 1;
        ViewState["TotalRecords"] = 0;
        ddlFOSTSM.SelectedIndex = 0;
        ddlSalesChannelType.SelectedIndex = -1;
        ucFromDate.Date = "";
        ucToDate.Date = "";

    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        GetSearchData(ucPagingControl1.CurrentPage);

    }
    void FillSalesmanName()
    {

        using (SalesmanData ObjSalesChannel = new SalesmanData())
        {
            ObjSalesChannel.UserID = PageBase.UserId;
            ddlFOSTSM.Items.Clear();
            string[] str = { "UserID", "Name" };
            PageBase.DropdownBinding(ref ddlFOSTSM, ObjSalesChannel.BindSalesManName(), str);
        };
    }

  
    void FillsalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {

            ddlSalesChannelType.Items.Clear();
            string[] str = { "SalesChannelTypeID", "SalesChannelTypeName" };
            PageBase.DropdownBinding(ref ddlSalesChannelType, ObjSalesChannel.GetSalesChannelTypeV5API(), str);
            //if (Convert.ToString(HttpContext.Current.Session["APIDatabaseVersion"]) == "0")
            //{
            //    if (PageBase.SalesChanelID != 0 & PageBase.OtherEntityType == 0)
            //    {
            //        ddlsaleschanneltype.SelectedValue = PageBase.SalesChanelTypeID.ToString();
            //        if (PageBase.SalesChanelTypeID != 5 && PageBase.IsRetailerStockTrack == 1)
            //            ddlsaleschanneltype.Items.Add(new ListItem("Retailer", "101"));

            //    }
            //    else if (PageBase.SalesChanelID == 0 & PageBase.OtherEntityType == 1)
            //    {
            //        ddlsaleschanneltype.Items.Clear();
            //        ddlsaleschanneltype.Items.Insert(0, new ListItem("Retailer", "101"));
            //        ddlsaleschanneltype.Enabled = false;
            //    }
            //    else if (PageBase.IsRetailerStockTrack == 1)
            //    {
            //        ddlsaleschanneltype.Items.Add(new ListItem("Retailer", "101"));
            //        ddlsaleschanneltype.Enabled = true;
            //    }
            //}

        };
    }
    private void GetSearchData(int pageno)
    {
        ViewState["TotalRecords"] = 0;
        

        if (ViewState["CurrentPage"] == null)
        {
            pageno = 1;
            ViewState["CurrentPage"] = pageno;
        }
        try{
        DataSet dtSalesMan;
        using (SalesmanData objSalesMan = new SalesmanData())
        {
            if (ucFromDate.Date == "" && ucToDate.Date == "")
            { }
            else
            {
                objSalesMan.FromDate= Convert.ToDateTime(ucFromDate.Date);
                objSalesMan.ToDate = Convert.ToDateTime(ucToDate.Date);
            }

            objSalesMan.CompanyName = txtCompanyName.Text.Trim(); // SalesChannelName

            if (ddlFOSTSM.SelectedIndex > 0)
            {
                objSalesMan.Salesmanname = ddlFOSTSM.SelectedItem.Text.Trim();
            }
           
            if (ddlSalesChannelType.SelectedIndex > 0)
            {
                objSalesMan.SalesChannelType = ddlSalesChannelType.SelectedItem.Text.Trim();
            }
           
            objSalesMan.PageIndex = pageno;
            objSalesMan.PageSize = Convert.ToInt32(PageSize);
            objSalesMan.UserID = PageBase.UserId;
            dtSalesMan = objSalesMan.GeSchemeDetailsAPI();

                if (objSalesMan.TotalRecords > 0)
                {
                    if (pageno > 0)
                    {
                        ucPagingControl1.Visible = true;
                        btnExprtToExcel.Visible = true;
                        gvSalesMan.DataSource = dtSalesMan;
                        gvSalesMan.DataBind();
                        ViewState["TotalRecords"] = objSalesMan.TotalRecords;
                        ucPagingControl1.TotalRecords = objSalesMan.TotalRecords;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = pageno;
                        ucPagingControl1.FillPageInfo();
                    }
                    else
                    {
                         ucPagingControl1.Visible = true;
                        btnExprtToExcel.Visible = true;
                        String FilePath = Server.MapPath("../../");
                        string FilenameToexport = "SchemeList";
                        PageBase.RootFilePath = FilePath;
                        PageBase.ExportToExecl(dtSalesMan, FilenameToexport);

                    }

                }
                else
                {
                    dtSalesMan = null;
                    gvSalesMan.DataSource = null;
                    gvSalesMan.DataBind();
                    btnExprtToExcel.Visible = false;
                    ucPagingControl1.Visible = false;
                }
        
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }

    }

    #endregion
}
