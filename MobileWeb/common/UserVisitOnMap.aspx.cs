﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using System.Data;
using System.IO;
using System.Net;

//======================================================================================
//* Developed By : Vijay Prajapati 
//* Role         : Software Engineer
//* Module       : Reports(User Visit on google Map)  
//* Description  :  This page is used for View use visit on google map
//* ====================================================================================
/* Change Log
 * -------------------------------------------------------------------------------------
 * DD-MMM-YYYY, Name, #CCXX, Description
 01-may-2019 , Gaurav Tyagi,#CC01 Imagepath added in grid to download image
 
 */

public partial class MobileWeb_common_UserVisitOnMap : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            FillEntityType();
            
            ucFromDate.Date = PageBase.Fromdate;
            ucToDate.Date = PageBase.ToDate;
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        SearchVisitMapDetailData();
        GoogleAPIKeyHitCountSave();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ddlEntityType.SelectedValue = "0";
        if(ddlEntityType.SelectedValue=="0")
        {
            ddlEntityTypeName.Items.Clear();
            ddlEntityTypeName.Items.Insert(0, new ListItem("Select", "0"));
            ucFromDate.Date = PageBase.Fromdate;
            ucToDate.Date = PageBase.ToDate;
            ucInTime.TextBoxTime.Text = "";
            ucOutTime.TextBoxTime.Text = "";
            rptMarkers.DataSource = null;
            rptMarkers.DataBind();
        }
    }
    protected void ddlEntityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillEntityTypeName(Convert.ToInt32(ddlEntityType.SelectedValue));
    }
    void FillEntityType()
    {
        using (ClsPaymentReport ObjEntityType = new ClsPaymentReport())
        {

            ddlEntityType.Items.Clear();
            string[] str = { "EntityTypeID", "EntityType" };
            ObjEntityType.CompanyId = PageBase.ClientId;
            ObjEntityType.UserId = PageBase.UserId;
            PageBase.DropdownBinding(ref ddlEntityType, ObjEntityType.GetEntityTypeV5API(), str);

        };
    }
    void FillEntityTypeName(int EntityTypeID)
    {
        using (ClsPaymentReport ObjEntityTypeName = new ClsPaymentReport())
        {

            ddlEntityTypeName.Items.Clear();
            ObjEntityTypeName.EntityTypeId = EntityTypeID;
            ObjEntityTypeName.UserId = PageBase.UserId;
            ObjEntityTypeName.CompanyId = PageBase.ClientId;
            string[] str = { "UserID", "EntityTypeName" };
            PageBase.DropdownBinding(ref ddlEntityTypeName, ObjEntityTypeName.GetEntityTypeName(), str);

        };
    }
    public void SearchVisitMapDetailData()
    {
        ucMessage1.Visible = false;
        ClsPaymentReport objMapvisit;
        try
        {
            using (objMapvisit = new ClsPaymentReport())
            {
                if (ucFromDate.Date == "" && ucToDate.Date == "")
                { ;}
                else
                {
                    objMapvisit.FromDate = Convert.ToDateTime(ucFromDate.Date);
                    objMapvisit.Todate = Convert.ToDateTime(ucToDate.Date);
                }
                objMapvisit.UserId = PageBase.UserId;
                objMapvisit.CompanyId = PageBase.ClientId;
                objMapvisit.EntityTypeId = Convert.ToInt32(ddlEntityType.SelectedValue);
                objMapvisit.EntitytypeUserId = Convert.ToInt16(ddlEntityTypeName.SelectedValue);
                objMapvisit.InTime = ucInTime.GetTime; ;
                objMapvisit.OutTime = ucOutTime.GetTime;
                DataSet ds = objMapvisit.GetReportONmapvisitData();
                if (objMapvisit.TotalRecords > 0)
                {
                    rptMarkers.DataSource = ds;
                    rptMarkers.DataBind();
                   
                }
                else
                {
                    ucMessage1.Visible = true;
                    ucMessage1.ShowInfo("No Record Found.");
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
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
                objdashboard.InterfaceName = "History Tracker";
                Int32 Result = objdashboard.SaveGoogleAPICountHit();
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
}