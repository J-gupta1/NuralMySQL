﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DataAccess;
using BussinessLogic;
/*Change Log:
 * 08-May-2014, Rakesh Goel, #CC01 - Changed export to excel call to EPP method instead of OpenXML.
 * Also added exception display in catch block
 * 19 Jan 2017, Karam Chand Sharma, #CC02, Remove mandetory for user name
 */


public partial class Reports_Common_UserTrackingRpt : PageBase
{

   


    DataTable roleinfo;

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (!IsPostBack)
            {
               // pnlGrid.Visible = false;
                fillrole();
                fnchidecontrols();
                ucDatePicker1.Date = PageBase.Fromdate;
                ucDatePicker2.Date = PageBase.ToDate;
            }
            }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    void fnchidecontrols()
    {
        ucMessage1.Visible  = false;
       // pnlGrid.Visible = false;
        //updgrid.Update();

    }


    private void fncBindData()
    {
        try
        {
            DataTable DsUserActivityInfo;
            using (ReportData objRD = new ReportData())
            {
                objRD.DateFrom = Convert.ToDateTime(ucDatePicker1.Date);
                objRD.DateTo = Convert.ToDateTime(ucDatePicker2.Date);
               
                objRD .RoleId = Convert.ToInt16(cmbRoleType.SelectedValue.ToString());
                objRD.UserId = Convert.ToInt32(cmbUsername.SelectedValue.ToString());
                DsUserActivityInfo = objRD.GetUserTrackingInfo() ;
                if (DsUserActivityInfo != null && DsUserActivityInfo.Rows.Count > 0)
                {
                    if (hfSearch.Value == "0")
                    {
                        // ViewState["Table"] = DsUserActivityInfo;
                        grdUserTrackRpt.DataSource = DsUserActivityInfo;
                        grdUserTrackRpt.DataBind();
                        ucMessage1.Visible = false;
                        pnlGrid.Visible = true;
                       // updgrid.Update();
                    }
                    else
                    {
                        string[] DsCol = new string[] { "RoleName", "UserName", "MenuName", "UserActivityDate", "LastLoginOn", "UserIP", "ServerIP" };
                        DataTable DsCopy = new DataTable();
                        DsUserActivityInfo = DsUserActivityInfo.DefaultView.ToTable(true, DsCol);
                        if (DsUserActivityInfo.Rows.Count > 0)
                        {
                            try
                            {
                                DataSet dtcopy = new DataSet();
                                dtcopy.Merge(DsUserActivityInfo);
                                dtcopy.Tables[0].AcceptChanges();
                                String FilePath = Server.MapPath("../../");
                                string FilenameToexport = "UserDetails";
                                PageBase.RootFilePath = FilePath;
                                //PageBase.ExportToExeclUsingOPENXMLV2(dtcopy.Tables[0], FilenameToexport);  //#CC01 commented
                                PageBase.ExportToExecl(dtcopy, FilenameToexport);  //#CC01 added
                            }
                            catch (Exception ex)
                            {
                                ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                            }
                        }
                        else
                        {
                            ucMessage1.ShowError(Resources.Messages.NoRecord);
                            
                        }
 
                    }
                    
                }
                else
                {
                   // pnlGrid.Visible = false;
                    
                    grdUserTrackRpt.DataSource = null;
                    grdUserTrackRpt.DataBind();
                    ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                    //updgrid.Update(); 
                }
                
                
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

    bool PageValidate()
    {
        try
        {
              
                if (Convert.ToDateTime(ucDatePicker1.Date) > Convert.ToDateTime(ucDatePicker2.Date))
                {
                    ucMessage1.ShowInfo("From Date must be smaller than To date");
                    return false;
                }
            
            return true;
        }
        catch (Exception ex)
        {

            throw ex ;
        }
        }



    private void fillrole()
    {
        
        using (UserData objuser = new UserData())
        {
            objuser.RoleId = PageBase.RoleID;
            objuser.EntitytypeId = Convert.ToInt32(PageBase.EntityTypeID);
            objuser.BaseEntitytypeId =Convert.ToInt32(PageBase.BaseEntityTypeID);

            objuser.saleschannelLevel = Convert.ToInt16(PageBase.SalesChannelLevel);
            objuser.HierarchyLevelID = Convert.ToInt16(PageBase.HierarchyLevelID);
           
            roleinfo = objuser.GetUserRoleHierarchy();
            cmbRoleType.DataSource = roleinfo;
            cmbRoleType.DataTextField = "RoleName";
            cmbRoleType.DataValueField = "RoleID";
            cmbRoleType.DataBind();
            cmbRoleType.Items.Insert(0, new ListItem("Select", "0"));
            cmbUsername.Items.Insert(0, new ListItem("Select", "0")); 
        }
    }

    protected void ddrSerRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        pnlGrid.Visible = false;
        //updgrid.Update();
        ucMessage1.Visible = false;
        if (cmbRoleType.SelectedValue == "0")
        {
            cmbUsername.Items.Clear();
            cmbUsername.Items.Insert(0, new ListItem("Select", "0"));
        }
        else 
        {
            using (UserData objuser = new UserData())
            {
                objuser.RoleId = Convert.ToInt16(cmbRoleType.SelectedValue.ToString());
                objuser.ActiveStatus = 255;
                DataTable dt = objuser.GetUserFromRole();
                cmbUsername.DataSource = dt;
                cmbUsername.DataTextField = "UserName";
                cmbUsername.DataValueField = "UserID";
                cmbUsername.DataBind();
                cmbUsername.Items.Insert(0, new ListItem("Select", "0"));

            }
        }
    }
    protected void btnSerch_Click(object sender, EventArgs e)
    {
        try
        {
            if (!PageValidate())
            {
                return;
            }
            
            hfSearch.Value = "0";
            fncBindData();
            //updMsg.Update();
        }

        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void exportToExel_Click(object sender, EventArgs e)
    {
        hfSearch.Value = "1";
        fncBindData();

    }

    protected void grdArea_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdUserTrackRpt .PageIndex  = e.NewPageIndex;
        fncBindData();
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        cmbRoleType.SelectedIndex = 0;
        cmbUsername.SelectedIndex = 0;
        ucDatePicker1.Date = PageBase.Fromdate;
        ucDatePicker2.Date = PageBase.ToDate;
        pnlGrid.Visible = false;
        //updgrid.Update();

    }
}
