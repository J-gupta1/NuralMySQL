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

public partial class Masters_Common_ViewSchemeFilterDetails : PageBase
{
    DataSet DsScheme;
    int SchemePerformanceCalculationID;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            StyleCss.Attributes.Add("href", "~/" + strAssets + "/CSS/popup.css");
           
            if ((Request.QueryString["SchemePerformanceID"] != null) && (Request.QueryString["SchemePerformanceID"] != ""))
            {

                SchemePerformanceCalculationID = Convert.ToInt32(Convert.ToString(Crypto.Decrypt((Request.QueryString["SchemePerformanceID"]).ToString().Replace(" ", "+"), PageBase.KeyStr)));   //Pankaj Kumar
            }
            if (!IsPostBack)
            {
                FillGrid(SchemePerformanceCalculationID);
            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);

        }
    }


    void FillGrid(Int32 SchemePerformanceCalculationID)
    {

        try
        {
            DsScheme = new DataSet();
            using (SchemeData ObjScheme = new SchemeData())
            {
                ObjScheme.SchemePerformanceCalculationID = SchemePerformanceCalculationID;
                DsScheme = ObjScheme.GetSchemeFilterDetails();
            };
            
               if (DsScheme != null && DsScheme.Tables[0].Rows.Count > 0)
                {
                    if (DsScheme.Tables[0].Rows.Count > 0)
                    {
                        GridScheme.DataSource = DsScheme.Tables[0];
                        GridScheme.DataBind();
                    }
                    if (DsScheme.Tables[1].Rows.Count > 0)
                    {

                        lblCriterialType.Text = DsScheme.Tables[1].Rows[0]["ComponentCriteriaTypeName"].ToString(); ;
                        lblPayoutType.Text = DsScheme.Tables[1].Rows[0]["ComponentPayoutTypeName"].ToString();
                        grdPayout.DataSource = DsScheme.Tables[1];
                        grdPayout.DataBind();
                        pnlPayout.Visible = true;
                    }
                }
                else
                {
                    GridScheme.DataSource = null;
                    GridScheme.DataBind();
                    grdPayout.DataSource = null;
                    grdPayout.DataBind();
                    pnlPayout.Visible = false;

                }
        
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
    }

    protected void grdPayout_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridScheme.PageIndex = e.NewPageIndex;
        FillGrid(SchemePerformanceCalculationID);

    }
}
