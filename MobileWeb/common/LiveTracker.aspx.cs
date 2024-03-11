using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using BussinessLogic;
using DataAccess;
using System.Net;


public partial class MobileWeb_common_LiveTracker : PageBase
{
    public string ConStr = ConfigurationManager.ConnectionStrings["AppConString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           // ddlUser.Items.Insert(0, new ListItem("Select", "0"));
            ucSearchDate.Date = PageBase.ToDate;
            BindDefaultfields();
            BindData();
           // BindddlType();
            BindLiveTracker();
            GoogleAPIKeyHitCountSave();
        }
    }
    public DataSet GetData(int EntityID, DateTime DateRange)
    {
        DataSet dsResult = new DataSet();
        SqlParameter[] objSqlParam = new SqlParameter[4];
        objSqlParam[0] = new SqlParameter("@EntityID", EntityID);
        objSqlParam[1] = new SqlParameter("@DateRange", DateRange);
        objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.BigInt, 8);
        objSqlParam[2].Direction = ParameterDirection.Output;
        objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[3].Direction = ParameterDirection.Output;
        dsResult = SqlHelper.ExecuteDataset(ConStr, CommandType.StoredProcedure, "prcSupervisorDashboardV1", objSqlParam);
        return dsResult;
    }


    private void BindServiceCentre()
    {
        try
        {

        }
        catch (Exception ex)
        {

        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindLiveTracker();
    }

    private void BindLiveTracker()
    {
        try
        {
            ucMessage1.Visible = false;
            DataSet ds = new DataSet();
            using (DataAccess.dashBoard objdashboard = new DataAccess.dashBoard())
            {
                objdashboard.ComingFor = 2;
                objdashboard.UserId = PageBase.UserId;
                objdashboard.Date = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                //if (ddlEntityType.SelectedIndex > 0)
                //    objdashboard.EntityTypeID = Convert.ToInt32(ddlEntityType.SelectedValue);
                //if (ddlUser.SelectedIndex > 0)
                //    objdashboard.EntityID = Convert.ToInt32(ddlUser.SelectedValue);
                objdashboard.EntityTypeID = 0;
                objdashboard.EntityID = 0;
                ds = objdashboard.GetSupervisorDashboardDataV1();
                if (objdashboard.Result == 0)
                {
                    if (objdashboard.TotalRecords > 0)
                    {
                        ViewState["Date"] = DateTime.Now.ToShortDateString();
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            DatalistEngineerAbsent.DataSource = ds.Tables[0];
                            DatalistEngineerAbsent.DataBind();
                        }
                        else
                        {
                            DatalistEngineerAbsent.DataSource = null;
                            DatalistEngineerAbsent.DataBind();
                        }
                        
                        if (ds != null && ds.Tables[1].Rows.Count > 0)
                        {

                            gvEngDetail.DataSource = ds.Tables[1];
                            gvEngDetail.DataBind();
                           divgvEngDetail.Attributes.Add("style", "display:block");
                        }
                        else
                        {
                            gvEngDetail.DataSource = null;
                            gvEngDetail.DataBind();
                           divgvEngDetail.Attributes.Add("style", "display:none");
                        }
                        
                        if (ds != null && ds.Tables[2].Rows.Count > 0)
                        {

                            rptMarkers.DataSource = ds.Tables[2];
                            rptMarkers.DataBind();
                        }
                        else
                        {
                            ucMessage1.ShowInfo("No Record Found.");
                            rptMarkers.DataSource = null;
                            rptMarkers.DataBind();
                        }
                        
                        if (ds != null && ds.Tables[0].Rows.Count == 0 && ds.Tables[1].Rows.Count == 0 && ds.Tables[1].Rows.Count == 0)
                        {
                           divPresentAbsent.Attributes.Add("style", "display:none");
                            
                            return;
                        }
                        else
                        {
                            divPresentAbsent.Attributes.Add("style", "display:block");

                        }
                    }
                    else
                    {
                        ucMessage1.ShowInfo(objdashboard.Error);
                        rptMarkers.DataSource = null;
                        rptMarkers.DataBind();
                    }
                }
                else if (objdashboard.Result == 1)
                {
                    ucMessage1.ShowInfo(objdashboard.Error);
                }
                else if (objdashboard.Result == 2)
                {
                    ucMessage1.ShowError(objdashboard.Error);
                }
            }



        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }

    protected void gvEngDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image imgAbsent = (Image)e.Row.FindControl("imggrdPresentGreen");
                {
                    imgAbsent.ImageUrl = PageBase.siteURL + "/" + PageBase.strAssets + "/Css/Images/PresentGreen.png";
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
        }
    }

    private void BindDefaultfields()
    {
        try
        {
            imgabsentred.Src = PageBase.siteURL + "/" + PageBase.strAssets + "/Css/Images/AbsentRed.png";
            imgPresentGreen.Src = PageBase.siteURL + "/" + PageBase.strAssets + "/Css/Images/PresentGreen.png";
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
        }
    }
    protected void DatalistEngineerAbsent_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Image imgAbsent = (Image)e.Item.FindControl("imgdtListEngineerAbsent");
                {
                    imgAbsent.ImageUrl = PageBase.siteURL + "/" + PageBase.strAssets + "/Css/Images/AbsentRed.png";
                }
                Label LblthUserNameHeading = (Label)e.Item.FindControl("lblthUserNameHeading");

                {
                    LblthUserNameHeading.Text = "Display Name";
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message + "line no : 227");
        }
    }

    public void BindData()
    {
        try
        {
            ucMessage1.Visible = false;
            DataSet ds = new DataSet();
            using (DataAccess.dashBoard objdashboard = new DataAccess.dashBoard())
            {
                objdashboard.ComingFor = 2;
                objdashboard.UserId = PageBase.UserId;
                objdashboard.CompanyId = PageBase.ClientId;
                objdashboard.Date = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                ds = objdashboard.GetSupervisorDashboardDataV1();
                if (objdashboard.Result == 0)
                {
                    if (objdashboard.TotalRecords > 0)
                    {
                        ViewState["Date"] = DateTime.Now.ToShortDateString();
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            DatalistEngineerAbsent.DataSource = ds.Tables[0];
                            DatalistEngineerAbsent.DataBind();
                        }
                        else
                        {
                            DatalistEngineerAbsent.DataSource = null;
                            DatalistEngineerAbsent.DataBind();
                        }
                        if (ds != null && ds.Tables[1].Rows.Count > 0)
                        {

                            gvEngDetail.DataSource = ds.Tables[1];
                            gvEngDetail.DataBind();
                           divgvEngDetail.Attributes.Add("style", "display:block");
                        }
                        else
                        {
                            gvEngDetail.DataSource = null;
                            gvEngDetail.DataBind();
                            divgvEngDetail.Attributes.Add("style", "display:none");
                        }
                        if (ds != null && ds.Tables[2].Rows.Count > 0)
                        {

                            rptMarkers.DataSource = ds.Tables[2];
                            rptMarkers.DataBind();
                        }
                        if (ds != null && ds.Tables[0].Rows.Count == 0 && ds.Tables[1].Rows.Count == 0 && ds.Tables[1].Rows.Count == 0)
                        {
                            divPresentAbsent.Attributes.Add("style", "display:none");
                            
                            return;
                        }
                        else
                        {
                            divPresentAbsent.Attributes.Add("style", "display:block");
                        }
                    }
                    else
                    {
                        ucMessage1.ShowInfo(objdashboard.Error);
                    }
                }
                else if (objdashboard.Result == 1)
                {
                    ucMessage1.ShowInfo(objdashboard.Error);
                }
                else if (objdashboard.Result == 2)
                {
                    ucMessage1.ShowError(objdashboard.Error);
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    //protected void ddlEntityType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        using (dashBoard ObjdashBoard = new dashBoard())
    //        {

    //            ObjdashBoard.UserId = Convert.ToInt32(PageBase.UserId);
    //            ObjdashBoard.EntityTypeID = Convert.ToInt32(ddlEntityType.SelectedValue);
    //            String[] StrCol1 = new String[] { "EntityID", "DisplaynameWithRole" };
    //            DataSet dsResult = ObjdashBoard.GetSupervisorDashboardEntity();
    //            PageBase.DropdownBinding(ref ddlUser, dsResult.Tables[0], StrCol1);

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucMessage1.ShowError(ex.ToString());
    //    }
    //}

    //public void BindddlType()
    //{
    //    try
    //    {
    //        using (dashBoard ObjdashBoard = new dashBoard())
    //        {
    //            ddlEntityType.Items.Clear();
    //            ObjdashBoard.UserId = PageBase.UserId;
    //            String[] StrCol1 = new String[] { "EntityTypeID", "EntityType" };
    //            DataSet dsResult = ObjdashBoard.GetSupervisorDashboardType();
    //            PageBase.DropdownBinding(ref ddlEntityType, dsResult.Tables[0], StrCol1);

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucMessage1.ShowError(ex.ToString());
    //    }
    //}

    public void GoogleAPIKeyHitCountSave()
    {
        try
        {
            ucMessage1.Visible = false;
            DataSet ds = new DataSet();
            using (DataAccess.dashBoard objdashboard = new DataAccess.dashBoard())
            {

                objdashboard.UserId = PageBase.UserId;
                objdashboard.CompanyId = PageBase.ClientId;
                string hostName = Dns.GetHostName();
                string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
                objdashboard.IPAddress = myIP;
                objdashboard.InterfaceName = "Live Tracker";

                Int32 Result = objdashboard.SaveGoogleAPICountHit();
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
}