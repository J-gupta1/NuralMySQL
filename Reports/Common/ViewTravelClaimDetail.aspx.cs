#region Copyright(c) 2010 Zed-Axis Technologies All rights are reserved
/*===============================================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2010 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
=================================================================================================================================
* Created By    : Shashikant Singh
* Created On    : 23-Jun-2020
* Role          : SSe
* Module        : Approve travel calim 
* Description   : This page is used for get and upload UploadTravelClaimApproval Data.
=================================================================================================================================
* Reviewed By : 
=================================================================================================================================
Change Log:
-----------

=================================================================================================================================
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.IO;
using System.Data;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;
using Resources;
using System.Security.Cryptography;
using BussinessLogic;
using ExportExcelOpenXML;
using ZedService;
using BusinessLogics;



public partial class ViewTravelClaimDetail : PageBase
{
    string strUploadedFileName = string.Empty;
    DataSet dsErrorProne = new DataSet();
    UploadFile UploadFile = new UploadFile();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillEntityType();

        }

    }
    protected void btnExportinexcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (ucFromDate.TextBoxDate.Text == "" && ucTODate.TextBoxDate.Text == "" && ddlappstatus.SelectedValue == "255" && ddlEntityType.SelectedValue == "0" && ddlEntityTypeName.SelectedValue == "0" && ddlapprovalLevel.SelectedValue == "0")
            {
                ucMessage1.ShowInfo("Please fill any search parameter first.");
                return;
            }
            if (ucFromDate.TextBoxDate.Text != "" && ucTODate.TextBoxDate.Text == "")
            {
                ucMessage1.ShowInfo("Please fill Date To.");
                return;
            }
            if (ucFromDate.TextBoxDate.Text == "" && ucTODate.TextBoxDate.Text != "")
            {
                ucMessage1.ShowInfo("Please fill Date From.");
                return;
            }
            BindGrid(-1);
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.ToString());
        }
    }
    
    private void BindGrid(int index)
    {
        try
        {
            using (ClsUploadTravelClaimApproval objdetails = new ClsUploadTravelClaimApproval())
            {
                if (ucFromDate.Date == "" && ucTODate.Date == "")
                { ;}
                else
                {
                    objdetails.FromDate = Convert.ToDateTime(ucFromDate.Date);
                    objdetails.ToDate = Convert.ToDateTime(ucTODate.Date);
                }
                objdetails.EntityId = Convert.ToInt32(ddlEntityTypeName.SelectedValue);

                objdetails.EntityTypeId = Convert.ToInt32(ddlEntityType.SelectedValue);
                objdetails.Status = Convert.ToInt16(ddlappstatus.SelectedValue);
                objdetails.RoleId = PageBase.RoleID;

                objdetails.EngineerUserDetailId = 0;
                objdetails.SelectedEntityID = 0;
                objdetails.userId = PageBase.UserId;
                objdetails.PageIndex = index;
                objdetails.PageSize = Convert.ToInt32(PageBase.PageSize);
                objdetails.ApprovalLevel = Convert.ToInt16(ddlapprovalLevel.SelectedValue);
                objdetails.CompanyID = PageBase.ClientId;

                DataSet ds = new DataSet();
                ds = objdetails.ViewAllTravelClaim();
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (index == -1)
                        {
                            string FilenameToexport = "TravelClaimReport";
                            PageBase.ExportToExecl(ds, FilenameToexport);
                        }                       
                       
                    }
                    else
                    {
                       
                        ucMessage1.ShowInfo("No Record Found.");
                    }
                }
              
            }
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.ToString());
        }
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
    protected void ddlEntityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillEntityTypeName(Convert.ToInt32(ddlEntityType.SelectedValue));
    }

}
