using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DataAccess;
using BussinessLogic;
using ZedService.Utility;

/*
 * 18-Dec-2018, Rakesh Raj, #CC01, Added Top 1 Buletting in Marquee
 */

public partial class Reports_TargetStockSalePurchDsbrd : PageBase//System.Web.UI.Page
{
    DataTable dt;
    DataSet Ds;
    public string strLastestBulletin = ""; //#CC01

    protected void Page_Load(object sender, EventArgs e)
    {
        Server.ScriptTimeout = 900;
        ucMsg.Visible = false;
        if (!IsPostBack)
        {
            try
            {
            
                UsertrackingAndRequestValidate();
                ShowDashBoard();
                ShowLatestBulletin();
            }
            catch (Exception ex)
            {
                ucMsg.ShowError(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }
    private void ShowLatestBulletin()
    {   
        try 
	        {	        
		                /*#CC01 Start*/
                        DataTable Dt = new DataTable();
                        using (BulletinData ObjBulletin = new BulletinData())
                        {
                            ObjBulletin.SubCategoryId = 0;
                            ObjBulletin.UserID = PageBase.UserId;
                            Dt = ObjBulletin.GetBulletinInfoByUserId();

                            if (Dt.Rows.Count > 0)
                            {
                                tdBulletin.Visible = true;
                               // strLastestBulletin = Dt.Rows[Dt.Rows.Count - 1]["BulletinSubject"].ToString();
                                strLastestBulletin = Dt.Rows[0]["BulletinSubject"].ToString();
                            }
                            else
                            {
                                tdBulletin.Visible = false;
                            }

                        };
                        /*#CC01 End*/
        
	        }
        catch(Exception ex)
        {
            ucMsg.ShowError(ex.Message);
        }
	  
    }
    private void ShowDashBoard()
    {
        try
        {
            //if ((ddlDashBoardType.SelectedValue == "1" || ddlDashBoardType.SelectedValue == "3")
            //    && ddlTargetType.SelectedIndex==0)
            //{
            //    ucMsg.ShowWarning("Please select target type.");
            //    return;
            //}
            using (ReportData obDashboard = new ReportData())
            {
                obDashboard.UserId = PageBase.UserId;
                obDashboard.BaseEntityTypeID = PageBase.BaseEntityTypeID;
                obDashboard.DashBoardMonth = 0;
                obDashboard.DashBoardYear = 0;
                
                DataSet ds = obDashboard.getStockSalePurchaseTopRetTopModelDashBoard();
                if (ds.Tables[0].Rows.Count>0)//Target
                {
                    grdTarget.DataSource = ds.Tables[0];
                    grdTarget.DataBind();
                    

                }
                if (ds.Tables[1].Rows.Count > 0)//Stock and Sale
                {
                    grdStockDashBoard.DataSource = ds.Tables[1];
                    grdStockDashBoard.DataBind();
                    

                    
                }
                if (ds.Tables[2].Rows.Count > 0)//Purchase and stock
                {
                    grdPurchaseDashBoard.DataSource = ds.Tables[2];
                    grdPurchaseDashBoard.DataBind();
                    
                }
                if (ds.Tables[3].Rows.Count > 0)//Top 5 Retailer
                {
                    grdTopRetailer.DataSource = ds.Tables[3];
                    grdTopRetailer.DataBind();

                }
                if (ds.Tables[4].Rows.Count > 0)//Bottom 5 Retailer
                {
                    grdBottomRetailer.DataSource = ds.Tables[4];
                    grdBottomRetailer.DataBind();

                }
                if (ds.Tables[5].Rows.Count > 0)//Top 5 model SP
                {
                    grdTopModelSP.DataSource = ds.Tables[5];
                    grdTopModelSP.DataBind();

                }
                if (ds.Tables[6].Rows.Count > 0)//Top 5 model SP
                {
                    grdTopModelFP.DataSource = ds.Tables[6];
                    grdTopModelFP.DataBind();

                }
                if (ds.Tables[7].Rows.Count > 0)//Last update On
                {
                    lblLastUpdate.Text =Convert.ToString( ds.Tables[7].Rows[0][0]);

                }
                
            }
        }
        catch(Exception ex)
        {
            ucMsg.ShowError(ex.Message);
        }
    }
    
}