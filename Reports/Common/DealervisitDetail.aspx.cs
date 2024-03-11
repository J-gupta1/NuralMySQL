using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cryptography;
using BussinessLogic;
using DataAccess;
using System.Data;

public partial class Reports_Common_DealervisitDetail : PageBase
{
    DataSet ds = new DataSet();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {  
           
            if (!IsPostBack)
            {
                StyleCss.Attributes.Add("href", "~/" + strAssets + "/CSS/popup.css");
                if (Session["Mydata"] != null && Session["Mydata"]!="")
                {
                   
                    ds =(DataSet)Session["Mydata"];
                    ViewState["Data"] = ds;

                    BindGrid();
                }
                else
                {
                    gvVisitAnalysis.DataSource = null;
                    gvVisitAnalysis.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);

        }
    }
    protected void gvVisitAnalysis_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvVisitAnalysis.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    private void BindGrid()
    {

        ds = (DataSet)ViewState["Data"];
        try
        {
            if(ds.Tables[0].Rows.Count>0)
            {
                gvVisitAnalysis.DataSource = ds;
                gvVisitAnalysis.DataBind();
            }
            else
            {
                gvVisitAnalysis.DataSource = null;
                gvVisitAnalysis.DataBind();
            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
        
    }
    protected void exportToExel_Click(object sender, EventArgs e)
    {

        ds = (DataSet)ViewState["Data"];
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                PageBase.ExportToExecl(ds, "DealerTypeDetail");
            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
       
    }
}