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
/*
================================================================================================================================
Copyright	: Zed-Axis Technologies, 2016
Created By	: Sumit Maurya
Create date	: 20-Aug-2016
Description	: .
Module      : Report
================================================================================================================================
Change Log:
DD-MMM-YYYY, Name , #CCXX - Description
30-Aug-2018,Rakesh Raj,#CC01 - Flat Report Export - Dynamic Header for Hierarchy Column Names 
--------------------------------------------------------------------------------------------------------------------------------
 */

public partial class Reports_SalesChannel_OrganHierarchyMappingReport : PageBase //System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        if (!IsPostBack)
        {
            // ddllocation.Enabled = false;
            BindHierarchy();
            ddllocation.Items.Insert(0, new ListItem("All", "0"));


        }
    }



    private void BindHierarchy()
    {
        try
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            ddlHierarchyLevel.Items.Clear();
            using (OrgHierarchyData objuser = new OrgHierarchyData())
            {
                objuser.AllownotOwn = false;
                objuser.HierarchyLevelID = 0;
                objuser.CompanyId = PageBase.ClientId;
                ds = objuser.GetHierarchyandOrgnHierarchyData();
                if (objuser.TotalRecords > 0)
                {
                    String[] colArray = { "HierarchyLevelID", "HierarchyLevelName" };
                    PageBase.DropdownBinding(ref ddlHierarchyLevel, ds.Tables[0], colArray);

                }


            };


        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

    protected void ddlHierarchyLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();

        using (OrgHierarchyData objuser = new OrgHierarchyData())
        {
            objuser.AllownotOwn = false;
            objuser.HierarchyLevelID = Convert.ToInt16(ddlHierarchyLevel.SelectedValue);
            objuser.CompanyId = PageBase.ClientId;
            ds = objuser.GetHierarchyandOrgnHierarchyData();
            if (objuser.TotalRecords > 0)
            {
                ddllocation.Items.Clear();
                String[] colArray = { "OrgnhierarchyID", "LocationCode" };
                PageBase.DropdownBinding(ref ddllocation, ds.Tables[0], colArray);
            }
            else
            {
                ddllocation.Items.Insert(0, new ListItem("All", "0"));
            }
        };
    }


    protected void ExportToExcel_Click(object sender, EventArgs e)
    {

        DataTable dt = new DataTable();
        try
        {
            using (OrgHierarchyData objuser = new OrgHierarchyData())
            {
                objuser.HierarchyLevelID = Convert.ToInt16(ddlHierarchyLevel.SelectedValue);
                objuser.OrgnhierarchyID = Convert.ToInt16(ddllocation.SelectedValue);
                objuser.CompanyId = PageBase.ClientId;
                dt = objuser.GetReportOrgnHierarchyMappingData();
                /*#CC01 Start
                using (ReportData objRD = new ReportData())
                {
                    objRD.headerReplacement(dt);
                } #CC01 End*/

                if (objuser.TotalRecords > 0)
                {
                    DataSet dtcopy = new DataSet();
                    dtcopy.Merge(dt);
                    dtcopy.Tables[0].AcceptChanges();
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "OrganisationHierarchyMappingReport";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dtcopy, FilenameToexport);

                }
                else
                {
                    ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
}
