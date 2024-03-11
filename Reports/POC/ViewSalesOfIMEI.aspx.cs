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

public partial class Reports_POC_ViewSalesOfIMEI : PageBase
{
    DataTable DtSalesChannelDetail = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.ShowControl = false;
            if (!IsPostBack)
            {
                FillRetailer();
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    void HideControls()
    {
        ExportToExcel.Visible = false;
        GridIMEI.Visible = false;
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
            if (ddlRetailer.SelectedIndex == 0 && txtIMEINo.Text.Trim()=="")
            {
                ucMessage1.ShowInfo("Please Select any Searching parameter");
                HideControls();
                return;
            }
            FillGrid();
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    void FillGrid()
    {
       using (RetailerData ObjRetailer = new RetailerData())
        {

            ObjRetailer.UserID = PageBase.UserId;
            ObjRetailer.IMEINo = txtIMEINo.Text.Trim();
            ObjRetailer.RetailerID = Convert.ToInt32(ddlRetailer.SelectedValue);
            DtSalesChannelDetail = ObjRetailer.GetSalesIMEI();
        };
        if (DtSalesChannelDetail != null && DtSalesChannelDetail.Rows.Count > 0)
        {
            //  ViewState["Table"] = DtSalesChannelDetail;
            ExportToExcel.Visible = true;
            GridIMEI.Visible = true;
            GridIMEI.DataSource = DtSalesChannelDetail;
            GridIMEI.DataBind();
            dvhide.Visible = true;

        }
        else
        {
            HideControls();
            GridIMEI.Visible = false;
            GridIMEI.DataSource = null;
            GridIMEI.DataBind();
            ucMessage1.ShowInfo(Resources.Messages.NoRecord);
            dvhide.Visible = false;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

        ucMessage1.ShowControl = false;
        ddlRetailer.SelectedValue = "0";
        txtIMEINo.Text = "";
        pnlBrand.Visible = false;
        HideControls();
        dvhide.Visible = false;
    }
    protected void ExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string Password = string.Empty;
            FillGrid();
            DataTable dt = DtSalesChannelDetail.Copy();
            string[] DsCol = new string[] { "RetailerName", "RetailerCode", "SalesDate", "ISDName", "StateName", "RegionName", "ZoneName", "DistrictName", "CityName", "Model", "IMEI" };
            DataTable DsCopy = new DataTable();
            dt = dt.DefaultView.ToTable(true, DsCol);
            if (dt.Rows.Count > 0)
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "SalesIMEI";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dtcopy, FilenameToexport);
            }
            else
            {
                ucMessage1.ShowError(Resources.Messages.NoRecord);

            }
         }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
   
    protected void GridSalesChannel_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridIMEI.PageIndex = e.NewPageIndex;
            FillGrid();
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }

    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            txtIMEINo.Text = "";
            ddlRetailer.SelectedValue = "0";
            FillGrid();
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }

  
   
    void FillRetailer()
    {
        using (RetailerData ObjRetaileType = new RetailerData())
        {
            ObjRetaileType.value = 1;
            String[] StrCol = new String[] { "RetailerID", "RetailerName" };
            PageBase.DropdownBinding(ref ddlRetailer, ObjRetaileType.GetAllRetailer(), StrCol);

        };
    }
}
