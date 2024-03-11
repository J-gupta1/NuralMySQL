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



public partial class Masters_HO_Admin_ViewPriceDrop : PageBase
{
    DataTable DtSalesChannelDetail = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.ShowControl = false;
            if (!IsPostBack)
            {
                pageInfo();
                HideControls();

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
   

   
    void HideControls()
    {
        dvhide.Visible = false;
        ExportToExcel.Visible = false;
        //GridPriceDrop.Visible = false;
         UpdGrid.Update();

    }
  

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {   
            FillGrid(1);
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    void FillGrid(int PageNo)
    {
        //  ViewState["Table"] = null;

        if (ucDateFrom.Date!="" &&  ucDateTo.Date!="" && Convert.ToDateTime(ucDateFrom.Date) > Convert.ToDateTime(ucDateTo.Date))
        {
            ucMessage1.ShowInfo("From Date can't be gretaer then the To Date.");
            return;
        }

        if (txtSalesChannelCodeSearch.Text.Trim() == "" && txtSerialNumberSearch.Text.Trim() == "" && ddlStatus.SelectedValue == "" && ucDateFrom.Date == "" && ucDateTo.Date=="")
        {
            ucMessage1.ShowInfo("Please provide atlease one search criteria. ");
        }
        else
        {
            using (PriceDrop obj = new PriceDrop())
            {
                obj.PageSize = Convert.ToInt32(PageBase.PageSize);
                obj.PageIndex = PageNo;
                obj.UserID = PageBase.UserId;
                obj.SalesChannelCode = txtSalesChannelCodeSearch.Text.Trim();
                obj.SerialNumber = txtSerialNumberSearch.Text.Trim();
                obj.strStatus = ddlStatus.SelectedValue;
                DataSet ds = obj.GetPriceDropData();
                if (obj.OutParam == 0)
                {
                    if (obj.TotalRecords > 0)
                    {
                        dvhide.Visible = true;
                        ExportToExcel.Visible = true;
                        GridPriceDrop.DataSource = ds;
                        GridPriceDrop.DataBind();

                        dvFooter.Visible = true;
                        ViewState["TotalRecords"] = obj.TotalRecords;
                        ucPagingControl1.TotalRecords = Convert.ToInt32(obj.TotalRecords);
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = PageNo;
                        ucPagingControl1.FillPageInfo();
                        UpdGrid.Update();
                    }
                    else
                    {
                        ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                        dvhide.Visible = false;
                        ExportToExcel.Visible = false;
                    }
                }
                else
                {
                    if (obj.OutParam == 1)
                    {
                        ucMessage1.ShowInfo(obj.OutError);
                    }
                    else if(obj.OutParam==2)
                    {
                        ucMessage1.ShowError(obj.OutError);
                    }
                }
            }
        }       
    }

  

    protected void btnCancel_Click(object sender, EventArgs e)
    {

        ucMessage1.ShowControl = false;        
        txtSalesChannelCodeSearch.Text = "";
        txtSalesChannelCodeSearch.Text = "";        
        HideControls();
        dvhide.Visible = false;
    }
    protected void ExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {


            if (ucDateFrom.Date != "" && ucDateTo.Date != "" && Convert.ToDateTime(ucDateFrom.Date) > Convert.ToDateTime(ucDateTo.Date))
            {
                ucMessage1.ShowInfo("From Date can't be gretaer then the To Date.");
                return;
            }

            if (txtSalesChannelCodeSearch.Text.Trim() == "" && txtSerialNumberSearch.Text.Trim() == "" && ddlStatus.SelectedValue == "" && ucDateFrom.Date == "" && ucDateTo.Date == "")
            {
                ucMessage1.ShowInfo("Please provide atlease one search criteria. ");
            }
            else
            {
                using (PriceDrop obj = new PriceDrop())
                {
                    obj.PageSize = Convert.ToInt32(PageBase.PageSize);
                    obj.PageIndex = -1;
                    obj.UserID = PageBase.UserId;
                    obj.SalesChannelCode = txtSalesChannelCodeSearch.Text.Trim();
                    obj.SerialNumber = txtSerialNumberSearch.Text.Trim();
                    obj.strStatus = ddlStatus.SelectedValue;
                    DataSet ds = obj.GetPriceDropData();
                    if (obj.OutParam == 0)
                    {
                        if (obj.TotalRecords > 0)
                        {
                           // PageBase.ExportToExeclV2(ds, "PriceDropData");
                            PageBase.ExportToExecl(ds, "PriceDropData");
                            UpdGrid.Update();
                        }
                        else
                        {
                            ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                            dvhide.Visible = false;
                        }
                    }
                    else
                    {
                        if (obj.OutParam == 1)
                        {
                            ucMessage1.ShowInfo(obj.OutError);
                        }
                        else if (obj.OutParam == 2)
                        {
                            ucMessage1.ShowError(obj.OutError);
                        }
                    }
                }
            }    

            //if (dt.Rows.Count > 0)
            //{
            //    DataSet dtcopy = new DataSet();
            //    dtcopy.Merge(dt);
            //    dtcopy.Tables[0].AcceptChanges();
            //    String FilePath = Server.MapPath("../../");
            //    string FilenameToexport = "SalesChannelList";
            //    PageBase.RootFilePath = FilePath;
            //    PageBase.ExportToExecl(dtcopy, FilenameToexport);
            //    //  ViewState["Table"] = null;
            //}
            //else
            //{
            //    ucMessage1.ShowError(Resources.Messages.NoRecord);

            //}
            // ViewState["Table"] = null;
            //   }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
 

    void pageInfo()
    {
        //this.GridPriceDrop.Columns[0].HeaderText = Resources.Messages.SalesEntity + " Name";
        //this.GridPriceDrop.Columns[3].HeaderText = Resources.Messages.SalesEntity + " Code";
        //this.GridPriceDrop.Columns[7].HeaderText = Resources.Messages.SalesEntity + " Type";
    }

 
    
 

    protected void UCPagingControl1_SetControlRefresh()
    {
        try
        {
            ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
            FillGrid(ucPagingControl1.CurrentPage);
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }


    }
}
