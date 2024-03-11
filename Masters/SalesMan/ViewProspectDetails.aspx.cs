#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*/
 * ================================================================================================
 * ================================================================================================
 * COPYRIGHT (c) 2016 Zed Axis Technologies (P) Ltd.
 * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART,
 * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
 * ================================================================================================
 * Created By : Rakesh Raj
 * Created On : 23 July 2018
 * Description : Display Prospect Details added from Mobile App
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

public partial class Masters_SalesMan_ViewProspectDetails : PageBase
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

                GetSearchData(1);/*#CC04 ADDED*/

                //this.Form.DefaultButton = btnShow.UniqueID;
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
          & txtCompanyName.Text.Trim().Length == 0 & txtContactNo.Text == "" & txtPersonName.Text=="" & ucDateFrom.Date==string.Empty)
        {
            //Please select at least one search criteria!
            ucMsg.Visible = true;
            ucMsg.ShowInfo(Resources.Messages.SearchCriteriaBlank);
            return;
        }
        if (ucDateFrom.Date != "" && ucDateTo.Date == "")
        {
            ucMsg.Visible = true;
            ucMsg.ShowWarning("Please Enter To Date.");
            return;
        }
        if (ucDateTo.Date != "" && ucDateFrom.Date == "")
        {
            ucMsg.Visible = true;
            ucMsg.ShowWarning("Please Enter From Date.");
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
        txtPersonName.Text = "";
        txtCompanyName.Text = "";
        txtContactNo.Text = "";
        ddlFOSTSM.SelectedIndex = 0;
        ddlSalesChannelType.SelectedIndex = -1;

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
            ObjSalesChannel.CompanyId = PageBase.ClientId;
            ddlFOSTSM.Items.Clear();
            string[] str = { "UserID", "Name" };
            PageBase.DropdownBinding(ref ddlFOSTSM, ObjSalesChannel.BindSalesManName(), str);
        };
    }
    void FillsalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.CompanyId = PageBase.ClientId;
            ObjSalesChannel.UserID = PageBase.UserId;
            ddlSalesChannelType.Items.Clear();
            string[] str = { "SalesChannelTypeID", "SalesChannelTypeName" };
            PageBase.DropdownBinding(ref ddlSalesChannelType, ObjSalesChannel.GetSalesChannelTypeV5API(), str);
           // ddlSalesChannelType.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Retailer", "101"));


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
            objSalesMan.PersonName = txtPersonName.Text.Trim();
            objSalesMan.CompanyName = txtCompanyName.Text.Trim();
            objSalesMan.ContactNo = txtContactNo.Text.Trim();
            if (ddlFOSTSM.SelectedIndex > 0)
            {
                objSalesMan.Salesmanname = ddlFOSTSM.SelectedValue.ToString();
            }
            else
            {
                objSalesMan.Salesmanname = "";
            }

            if (ddlSalesChannelType.SelectedIndex > 0)
            {
                objSalesMan.SalesChannelType = ddlSalesChannelType.SelectedItem.Text.Trim();
            }
            else
            {
                objSalesMan.SalesChannelType = "";
            }
            //objSalesMan.ProspectId = PageBase.SalesChanelID;
            objSalesMan.UserID = PageBase.UserId;
            objSalesMan.CompanyId = PageBase.ClientId;

            objSalesMan.PageIndex = pageno;
            objSalesMan.PageSize = Convert.ToInt32(PageSize);
            objSalesMan.NullableFromDate = ucDateFrom.GetDate;
            objSalesMan.NullableToDate = ucDateTo.GetDate;
            dtSalesMan = objSalesMan.GetProspectDetails();

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
                        btnExprtToExcel.Visible = true;
                        String FilePath = Server.MapPath("../../");
                        string FilenameToexport = "ProspectList";
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
