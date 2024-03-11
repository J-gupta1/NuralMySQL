#region Copyright(c) 2018 Zed-Axis Technologies All rights are reserved
/*/
 * ====================================================================================================
 * <copyright company="Zed Axis Technologies">
 * COPYRIGHT (c) 2018 Zed Axis Technologies (P) Ltd. 
 * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
 * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
 * WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
 * </copyright>
 * ====================================================================================================
 * Created By : Sumit Maurya
 * Created On: 24-Oct-2016 
 * Description: This module is used to exclude Menu user wise.
 * ====================================================================================================
 * Reviewed By :
 * DD-MMM-YYYY, Name, #CCXX, Description
 ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 ====================================================================================================
 */

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

public partial class Masters_HO_Admin_UserRoleModuleMapping : PageBase
{
    string strUploadedFileName = string.Empty;
    
    int counter = 0;
    UploadFile UploadFile = new UploadFile();
    protected void Page_Load(object sender, EventArgs e)
    {
      
        ucMsg.ShowControl = false;
        if (!IsPostBack)
        {
           BindRoles();
            
            ucMsg.Visible = false;

        }
    }

    private void BindRoles()
    {
        try
        {
            DataTable dt = new DataTable();
            ddlRole.Items.Clear();
            
            using (UserData objuser = new UserData()) 
            {
                objuser.SearchType = 4;
                objuser.UserID = PageBase.UserId;
                objuser.Status = true;
                dt = objuser.GetUserRole();
            };
            String[] colArray = { "RoleId", "RoleName" };
            dt.DefaultView.Sort = "RoleName ASC"; 
            PageBase.DropdownBinding(ref ddlRole, dt, colArray);
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
    }




    public DataTable dtTemp(DataTable dt)
    {
        try
        {
            DataTable dtreturn = new DataTable();

            
            foreach (DataColumn dc in dt.Columns)
            {
                object dctype = dc.DataType.FullName;
                dtreturn.Columns.Add(dc.ColumnName, typeof(string));
              
                dtreturn.AcceptChanges();
            }
            return dtreturn;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

  
    protected void DwnldReferenceCode_Click(object sender, EventArgs e)
    {
        try
        {
            ucMsg.Visible = false;
            DataTable dt;
            DataSet ds = new DataSet();
            DataSet dsReferenceCode = new DataSet();
            OrgHierarchyData objOrgn = new OrgHierarchyData();
            objOrgn.UserID = PageBase.UserId;
            objOrgn.PageIndex = -1;
            ds = objOrgn.GetParallelOrgnHierarchyBrandMappingInfo();
            if (objOrgn.TotalRecords > 0)
            {
                ds.Tables[0].TableName = "ParallelOrgnHierarchyData";
                if (ds.Tables.Count > 1)
                    ds.Tables[1].TableName = "SalesChannelData";
                if (ds.Tables.Count > 2)
                    ds.Tables[2].TableName = "BrandData";

                String FilePath = Server.MapPath("~/");
                string FilenameToexport = "OrgnHierarchyBrandMapping";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(ds, FilenameToexport, EnumData.eTemplateCount.ePrimarysales1);
            }
            else
            {
                ucMsg.ShowInfo(Resources.Messages.NoRecord);
            }
        }
        catch (Exception ex)
        {

        }



    }



    public bool SaveParallelOrgnHierarchyBrandMappingBCP(DataTable dtUpload)
    {
        try
        {

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "BulkUploadParallelOrgnHierarchyBrandMapping";
                bulkCopy.ColumnMappings.Add("LocationCode", "LocationCode");
                bulkCopy.ColumnMappings.Add("SalesChannelCode", "SalesChannelCode");
                bulkCopy.ColumnMappings.Add("BrandCode", "BrandCode");
                bulkCopy.ColumnMappings.Add("SessionID", "SessionID");
                bulkCopy.ColumnMappings.Add("CreatedBy", "CreatedBy");
                bulkCopy.WriteToServer(dtUpload);
                return true;
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            return false;
        }
    }
    private void ExportInExcel(DataSet DsExport, string strFileName)
    {
        try
        {
            if (DsExport != null && DsExport.Tables.Count > 0)
            {
                PageBase.ExportToExeclV2(DsExport, strFileName, DsExport.Tables.Count);
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }

   
    protected void btnExprtToExcel_Click(object sender, EventArgs e)
    {
        try
        {
           

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
   
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
        
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());

        }
    }

    private void BindSearchHierarchy()
    {
        try
        {
           

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

  

  
    protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            ddlModule.Items.Clear();
            using (MenuData objMenu = new MenuData())
            {
                objMenu.RoleID = Convert.ToInt32(ddlRole.SelectedValue);
                objMenu.UserID = PageBase.UserId;            
               ds = objMenu.GetRoleWiseModuleData();
              
            }
            String[] colArray = { "MenuID", "MenuName" };
            ds.Tables[0].DefaultView.Sort = "MenuName ASC"; 
            PageBase.DropdownBinding(ref ddlModule, ds.Tables[0], colArray);
            grdUser.Visible = false;
            btnSubmit.Visible = false;
            txtLoginName.Text = string.Empty;
           
        } 
        catch( Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    
    protected void btnGo_Click(object sender, EventArgs e)
    {
         try
         {
             btnSubmit.Visible = false;
            DataSet ds = new DataSet();
            using (MenuData objMenu = new MenuData()) 
            {
                objMenu.RoleID = Convert.ToInt32(ddlRole.SelectedValue);
                objMenu.MenuID = Convert.ToInt32(ddlModule.SelectedValue);
                objMenu.LoginName = txtLoginName.Text.Trim();
                objMenu.UserID = PageBase.UserId; 
                objMenu.PageIndex = -1;
                ds = objMenu.GetUserByMenuAndRoleData();               

                if(objMenu.TotalRecords>0)
                {
                    grdUser.Visible = true;             
                    grdUser.DataSource = ds;
                    grdUser.DataBind();
                    btnSubmit.Visible = true;
                }
                else
                {
                    ucMsg.ShowInfo("No Record Found");
                    grdUser.DataSource = null;
                    grdUser.DataBind();
                }
            }
        }
         catch (Exception ex)
         {
             ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
         }
    }
    
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = dtExcludeMenu();
            using (MenuData objMenu = new MenuData())  
            {
                for (int i = 0; i < grdUser.Rows.Count; i++)
                {
                    CheckBox chckApproval = (CheckBox)grdUser.Rows[i].FindControl("chckSelectItem");

                    if (txtLoginName.Text.Trim() != "")
                    {
                        DataRow dr = dt.NewRow();
                        HiddenField hdnUserID = (HiddenField)grdUser.Rows[i].FindControl("hdnUserID");
                        dr["MenuID"] = Convert.ToInt32(ddlModule.SelectedValue);
                        dr["UserID"] = Convert.ToInt32(hdnUserID.Value);
                        dr["RoleID"] = Convert.ToInt32(ddlRole.SelectedValue);
                        dr["Status"] = chckApproval.Checked == true ? 0 : 1;
                        dt.Rows.Add(dr);
                        dt.AcceptChanges();
                        objMenu.UpdateAll = 1;
                        break;
                    }
                    //if (!chckApproval.Checked)
                    else
                    {

                        DataRow dr = dt.NewRow();
                        HiddenField hdnUserID = (HiddenField)grdUser.Rows[i].FindControl("hdnUserID");
                        dr["MenuID"] = Convert.ToInt32(ddlModule.SelectedValue);
                        dr["UserID"] = Convert.ToInt32(hdnUserID.Value);
                        dr["RoleID"] = Convert.ToInt32(ddlRole.SelectedValue);
                        dr["Status"] = chckApproval.Checked == true ? 0 : 1;
                        dt.Rows.Add(dr);
                        dt.AcceptChanges();
                    }


                }
            if(dt!=null && dt.Rows.Count==0)
            {
                DataRow dr = dt.NewRow();
                dr["MenuID"] = Convert.ToInt32(ddlModule.SelectedValue);
                dr["UserID"] = 0;
                dr["RoleID"] = Convert.ToInt32(ddlRole.SelectedValue);
                dt.Rows.Add(dr);
                dt.AcceptChanges();
                objMenu.UpdateAll = 1;
                objMenu.LoginName = txtLoginName.Text.Trim();

            }
           
                /* objMenu.UpdateAll=  Convert.ToInt16(txtLoginName.Text.Trim()!=""? 1:0); temp commented */
                objMenu.LoginName = txtLoginName.Text.Trim();
                objMenu.UserID = PageBase.UserId; 
                objMenu.dtExclude = dt;
                int intResult = objMenu.ExcludeMenu();

                if(intResult==0)
                {
                    
                    ucMsg.ShowSuccess("Record saved successfully.");
                    grdUser.Visible = false;
                    btnSubmit.Visible = false;
                    txtLoginName.Text = string.Empty;
                    ddlRole.SelectedValue = "0";
                    ddlRole_SelectedIndexChanged(null, null);

                    
                }
                else
                {
                    ucMsg.ShowInfo(objMenu.Error);
                }
            }



        }
        catch (Exception ex)
        {
            
             ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }

    DataTable dtExcludeMenu()
    {
        DataTable dtTemp = new DataTable();      
        
        dtTemp.Columns.Add("MenuID", typeof(int));
        dtTemp.Columns.Add("UserID", typeof(int));
        dtTemp.Columns.Add("RoleID", typeof(int));
        dtTemp.Columns.Add("Status", typeof(int));
        return dtTemp;
    }



    void ClearFields()
    {
        try
        {
            ucMsg.Visible = false;
            ddlRole.SelectedValue = "0";
            ddlRole_SelectedIndexChanged(null, null);
            btnSubmit.Visible = false;
        }
        catch (Exception ex)
        {
            
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
}
