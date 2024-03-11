#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*/
 * ====================================================================================================
 * <copyright company="Zed Axis Technologies">
 * COPYRIGHT (c) 2016 Zed Axis Technologies (P) Ltd. 
 * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
 * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
 * WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
 * </copyright>
 * ====================================================================================================
 * Created By : Sumit Maurya
 * Created On: 24-Oct-2016 
 * Description: This module is used to create bulk Orgn Hierarchy User.
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

public partial class Masters_HO_Admin_ParallelOrgnHierarchyBrandMapping : PageBase
{
    string strUploadedFileName = string.Empty;
    DataTable dtNew = new DataTable();
    int counter = 0;
    UploadFile UploadFile = new UploadFile();
    protected void Page_Load(object sender, EventArgs e)
    {
        hlnkInvalid.Text = "";
        ucMsg.ShowControl = false;
        if (!IsPostBack)
        {

            BindSearchHierarchy();
            BindBrands();
            ddlSerHierarchyLevel_SelectedIndexChanged(null, null);
            FillLocationGrid(1);
            ViewState["CurrentPage"] = 1;
            ucMsg.Visible = false;

        }
    }

    public DataTable dtTemp(DataTable dt)
    {
        try
        {
            DataTable dtreturn = new DataTable();

            /*string[] columnNames = (from dc in dt.Columns.Cast<DataColumn>()
                                    select dc.ColumnName).ToArray();
              //foreach(string clmnNames  in columnNames)
            //{
            //    dtreturn.Columns.Add(
            //}             
             */

            foreach (DataColumn dc in dt.Columns)
            {
                object dctype = dc.DataType.FullName;
                dtreturn.Columns.Add(dc.ColumnName, typeof(string));
                /*dtreturn.Columns[0].DataType = dc.GetType();// dc.DataType.Name*/
                dtreturn.AcceptChanges();
            }
            return dtreturn;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {

            // int intLoginIDMinLength = Convert.ToInt16(Resources.AppConfig.LoginIDMinLength.ToString());
            DataSet objDS = new DataSet();
            byte isSuccess = 1;
            Int16 UploadCheck = 0;
            String RootPath = Server.MapPath("~/");
            UploadFile.RootFolerPath = RootPath;

            UploadCheck = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            ViewState["TobeUploaded"] = strUploadedFileName;
            if (UploadCheck == 1)
            {
                isSuccess = UploadFile.uploadValidExcelRetailer(ref objDS, "ParallelOrgnHierarBrandMapping");
                int intError = 0;
                if (objDS.Tables[0].Columns.Contains("Error"))
                {
                    intError = 1;
                }
                if (!objDS.Tables[0].Columns.Contains("Error"))
                {
                    objDS.Tables[0].Columns.Add("Error", typeof(string));
                }
                #region Validating Records with space
                if (isSuccess != 2)
                {
                    foreach (DataRow dr in objDS.Tables[0].Rows)
                    {
                        Int16 intvalidData = 0;
                        if (dr["LocationCode"].ToString().Trim().Length == 0)
                        {
                            dr["Error"] = dr["Error"].ToString() + " Locationcode cannot be left blank.";
                            isSuccess = 2;
                        }
                        if (dr["SalesChannelCode"].ToString().Trim().Length == 0)
                        {
                            dr["Error"] = dr["Error"].ToString() + " SalesChannelCode cannot be left blank.";
                            isSuccess = 2;
                        }
                        if (dr["BrandCode"].ToString().Trim().Length == 0)
                        {
                            dr["Error"] = dr["Error"].ToString() + " BrandCode cannot be left blank.";
                            isSuccess = 2;
                        }
                        if (dr["BrandCode"].ToString().Contains(","))
                        {
                            string[] spltBrandCode = dr["BrandCode"].ToString().Trim().Split(',');
                            foreach (string strBrand in spltBrandCode)
                            {
                                intvalidData = Convert.ToInt16(intvalidData == 0 ? strBrand.Trim().Length : intvalidData);
                            }

                            if (intvalidData == 0)
                            {
                                dr["Error"] = dr["Error"].ToString() + " BrandCode cannot be left blank.";
                                isSuccess = 2;
                            }

                            /*for (int j = 0; j < spltBrandCode.Count(); j++)
                            {
                                if (spltBrandCode[j].Trim().Length > 1)
                                {
                                    intvalidData = 1;
                                }
                            }*/
                        }
                    }
                }
                #endregion Validating Records with space
                #region Splitting comma saperated Brands
                if (isSuccess == 1)
                {
                    dtNew = dtTemp(objDS.Tables[0]);/* objDS.Tables[0].Clone();*/

                    for (int i = 0; i < objDS.Tables[0].Rows.Count; i++)
                    {
                        if (objDS.Tables[0].Rows[i]["BrandCode"].ToString().Contains(","))
                        {
                            string[] spltBrandCode = objDS.Tables[0].Rows[i]["BrandCode"].ToString().Trim().Split(',');
                            for (int j = 0; j < spltBrandCode.Count(); j++)
                            {
                                if (spltBrandCode[j].Trim().Length > 0)
                                {
                                    DataRow drSplitBrand = dtNew.NewRow();
                                    drSplitBrand["LocationCode"] = objDS.Tables[0].Rows[i]["LocationCode"].ToString().Trim();
                                    drSplitBrand["SalesChannelCode"] = objDS.Tables[0].Rows[i]["SalesChannelCode"].ToString().Trim();
                                    drSplitBrand["BrandCode"] = spltBrandCode[j].Trim();
                                    dtNew.Rows.Add(drSplitBrand);
                                    dtNew.AcceptChanges();
                                }
                            }
                        }
                        else
                        {
                            DataRow drBrand = dtNew.NewRow();
                            drBrand["LocationCode"] = objDS.Tables[0].Rows[i]["LocationCode"].ToString().Trim();
                            drBrand["SalesChannelCode"] = objDS.Tables[0].Rows[i]["SalesChannelCode"].ToString().Trim();
                            drBrand["BrandCode"] = objDS.Tables[0].Rows[i]["BrandCode"].ToString().Trim();
                            dtNew.Rows.Add(drBrand);
                            dtNew.AcceptChanges();
                        }

                    }
                }
                #endregion Splitting comma saperated Brands

                switch (isSuccess)
                {
                    case 0:
                        ucMsg.ShowInfo(UploadFile.Message);
                        break;
                    case 2:

                        DataView dvError = objDS.Tables[0].DefaultView;
                        dvError.RowFilter = "Error<>''";
                        DataTable dtError = dvError.ToTable();

                        DataSet dsError = new DataSet();
                        dsError.Tables.Add(dtError);

                        ucMsg.ShowInfo("Click on Invalid Data link for invalid data.");
                        hlnkInvalid.Visible = true;
                        string strFileName = "InvalidData" + DateTime.Now.Ticks;

                        ExportInExcel(dsError, strFileName);
                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                        hlnkInvalid.Text = "Invalid Data";
                        break;
                    case 1:
                        // SaveOrgnhierarchy(objDS.Tables[0]);
                        SaveOrgnhierarchy(dtNew);
                        break;
                }
                UploadFile.UploadedFileName = strUploadedFileName;
                UploadFile.UploadValidationType = EnumData.eUploadExcelValidationType.eSales;
                if (PageBase.SalesChanelID != 0)
                {
                    UploadFile.issaleschannel = true;
                }

            }
            else if (UploadCheck == 2)
            {
                ucMsg.ShowInfo(Resources.Messages.UploadXlxs);
            }
            else if (UploadCheck == 3)
            {
                ucMsg.ShowInfo(Resources.Messages.SelectFile);

            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            /*clsException.clsHandleException.fncHandleException(ex, "");*/
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


    public void SaveOrgnhierarchy(DataTable dt)
    {
        try
        {
            string guid = Guid.NewGuid().ToString();
            if (!dt.Columns.Contains("SessionID"))
                dt.Columns.Add(AddColumn(guid, "SessionID", typeof(System.String)));
            else
            {
                dt.Columns.Remove("SessionID");
                dt.Columns.Add(AddColumn(guid, "SessionID", typeof(System.String)));
            }
            if (!dt.Columns.Contains("CreatedBy"))
                dt.Columns.Add(AddColumn(Convert.ToString(PageBase.UserId), "CreatedBy", typeof(int)));

            if ((SaveParallelOrgnHierarchyBrandMappingBCP(dt)) == true)
            {

                using (OrgHierarchyData objOrgn = new OrgHierarchyData())
                {
                    objOrgn.SessionID = Convert.ToString(dt.Rows[0]["SessionID"]);
                    objOrgn.UserID = PageBase.UserId;
                    DataSet dsResult = objOrgn.ParallelOrgnHierarchyBrandMappingUpload();
                    if (objOrgn.intOutParam == 0)
                    {
                        ucMsg.ShowSuccess("Records updated successfully.");

                    }
                    else if (objOrgn.intOutParam == 1)
                    {
                        ucMsg.ShowInfo("Click Invalid data link for invalid data. No data processed.");
                        hlnkInvalid.Visible = true;
                        string strFileName = "InvalidData" + DateTime.Now.Ticks;
                        ExportInExcel(dsResult, strFileName);
                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                        hlnkInvalid.Text = "Invalid Data";
                        ViewState["SalesMan"] = null;
                    }
                    else
                    {
                        ucMsg.ShowError(objOrgn.Error);
                    }
                }
            }
            else
            {
                ucMsg.ShowInfo("Unable to update data.");
            }


        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
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

    protected void BtnReset_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("ParallelOrgnHierarchyBrandMapping.aspx", false);
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
            using (OrgHierarchyData objOrg = new OrgHierarchyData())
            {
                objOrg.PageIndex = -1;
                objOrg.PageSize = Convert.ToInt32(PageSize);
                objOrg.SalesChannelCode = txtSalesChannelCodeSearch.Text.Trim();
                objOrg.ParallelOrgnHierarchyID = Convert.ToInt32(ddlLocationSearch.SelectedValue);

                objOrg.BrandID = Convert.ToInt32(ddlBrand.SelectedValue);
                objOrg.HierarchyLevelID = Convert.ToInt16(ddlSerHierarchyLevel.SelectedValue);
                DataSet ds = objOrg.GetParallelOrgnHierarchyMappingInfo();
                if (objOrg.TotalRecords > 0)
                {
                    DataTable dt = ds.Tables[0].Copy();
                    string[] DsCol = new string[] { "HierarchyLevelName", "LocationCode", "LocationName", "SalesChannelCode", "SalesChannelName", "BrandCode", "BrandName", "ValidFrom" };
                    DataTable DsCopy = new DataTable();
                    dt = dt.DefaultView.ToTable(true, DsCol);
                    dt.Columns[0].ColumnName = "Hierarchy Level Type";
                    dt.Columns[1].ColumnName = "Location Code";
                    dt.Columns[2].ColumnName = "Location Name";
                    dt.Columns[3].ColumnName = "Sales Channel Code";
                    dt.Columns[4].ColumnName = "Sales Channel Name";
                    dt.Columns[5].ColumnName = "Brand Code";
                    dt.Columns[6].ColumnName = "Brand Name";
                    dt.Columns[7].ColumnName = "Valid From";



                    if (dt.Rows.Count > 0)
                    {
                        DataSet dtcopy = new DataSet();
                        dtcopy.Merge(dt);
                        dtcopy.Tables[0].AcceptChanges();
                        dtcopy.Tables[0].TableName = "OrgnHierarchyBrandMappingData";
                        String FilePath = Server.MapPath("~/");
                        string FilenameToexport = "ParallelOrgnHierarchyBrandMappingData";
                        PageBase.RootFilePath = FilePath;
                        PageBase.ExportToExecl(dtcopy, FilenameToexport);

                    }
                    else
                    {
                        ucMsg.ShowInfo(Resources.Messages.NoRecord);
                    }
                }
                else
                {
                    ucMsg.ShowInfo("No Record Found");
                }
            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlSerHierarchyLevel.SelectedValue == "0" && ddlLocationSearch.SelectedValue == "0" && txtSalesChannelCodeSearch.Text == "" && ddlBrand.SelectedValue == "0")
            {
                ucMsg.ShowInfo("Please provide atleast one search parameter value.");
                return;
            }
            FillLocationGrid(1);
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());

        }
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {

            ddlBrand.SelectedValue = "0";
            ddlSerHierarchyLevel.SelectedValue = "0";
            txtSalesChannelCodeSearch.Text = "";
            ddlSerHierarchyLevel_SelectedIndexChanged(null, null);
            FillLocationGrid(1);
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
            DataTable dt = new DataTable();
            ddlSerHierarchyLevel.Items.Clear();
            using (OrgHierarchyData objuser = new OrgHierarchyData())
            {
                dt = objuser.GetAllHierarchyLevel();
            };
            String[] colArray = { "HierarchyLevelID", "HierarchyLevelName" };
            PageBase.DropdownBinding(ref ddlSerHierarchyLevel, dt, colArray);

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }




    private void BindBrands()
    {
        try
        {
            DataTable brandserch = new DataTable();
            ddlBrand.Items.Clear();
            using (ProductData objproduct = new ProductData())
            {
                objproduct.BrandId = 0;
                objproduct.BrandSelectionMode = 2;
                brandserch = objproduct.SelectBrandInfo();

            };
            String[] colArray = { "BrandID", "BrandName" };
            PageBase.DropdownBinding(ref ddlBrand, brandserch, colArray);

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

    private void FillLocationGrid(int pageNo)
    {
        ucMsg.Visible = false;
        using (OrgHierarchyData objOrg = new OrgHierarchyData())
        {
            objOrg.PageIndex = pageNo;
            objOrg.PageSize = Convert.ToInt32(PageSize);
            objOrg.SalesChannelCode = txtSalesChannelCodeSearch.Text.Trim();
            objOrg.ParallelOrgnHierarchyID = Convert.ToInt32(ddlLocationSearch.SelectedValue);

            objOrg.BrandID = Convert.ToInt32(ddlBrand.SelectedValue);
            objOrg.HierarchyLevelID = Convert.ToInt16(ddlSerHierarchyLevel.SelectedValue);
            DataSet ds = objOrg.GetParallelOrgnHierarchyMappingInfo();

            /**/
            if (objOrg.TotalRecords > 0)
            {
                dvFooter.Visible = true;
                ViewState["TotalRecords"] = objOrg.TotalRecords;
                ucPagingControl1.TotalRecords = objOrg.TotalRecords;
                ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                ucPagingControl1.SetCurrentPage = pageNo;
                ucPagingControl1.FillPageInfo();
                btnExprtToExcel.Visible = true;
                gvParallelOrgnHierarchyUser.Visible = true;
                gvParallelOrgnHierarchyUser.DataSource = ds.Tables[0];
                gvParallelOrgnHierarchyUser.DataBind();
                //updgrid.Update();

            }
            else
            {
                gvParallelOrgnHierarchyUser.DataSource = null;
                dvFooter.Visible = false;
                btnExprtToExcel.Visible = false;
                gvParallelOrgnHierarchyUser.Visible = false;
                ucMsg.ShowInfo("No Record Found");
            }
            gvParallelOrgnHierarchyUser.DataBind();
            // updgrid.Update();

        }
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        FillLocationGrid(ucPagingControl1.CurrentPage);
    }

    protected void gvParallelOrgnHierarchyUser_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Int32 intParallelOrgnHierarchyID = Convert.ToInt32(e.CommandArgument);
            Int32 RowIndex = Convert.ToInt32(e.CommandArgument) - 1;


            HiddenField hdnparallelOrgnSalesChannelBrandMappingID = gvParallelOrgnHierarchyUser.Rows[RowIndex].FindControl("hdnParallelOrgnSalesChannelBrandMappingID") as HiddenField;

            HiddenField hdnstatus = gvParallelOrgnHierarchyUser.Rows[RowIndex].FindControl("hdnStatus") as HiddenField;



            if (e.CommandName == "Active")
            {
                using (OrgHierarchyData objOrg = new OrgHierarchyData())
                {

                    objOrg.UserID = PageBase.UserId;
                    objOrg.ParallelOrgnSalesChannelBrandMappingID = Convert.ToInt32(hdnparallelOrgnSalesChannelBrandMappingID.Value);
                    objOrg.intStatus = Convert.ToInt16(hdnstatus.Value == "0" ? 1 : 0);
                    int intresult = objOrg.UpdParallelOrgnSalesChannelBrandMappingStatus();
                    if (intresult == 0)
                    {
                        FillLocationGrid(Convert.ToInt32(ViewState["CurrentPage"].ToString()));
                        ucMsg.ShowSuccess(Resources.Messages.StatusChanged);


                    }
                    else if (intresult == 1 && !string.IsNullOrEmpty(objOrg.Error))
                    {
                        ucMsg.ShowInfo(objOrg.Error);
                    }
                    else if (intresult == 2 && !string.IsNullOrEmpty(objOrg.Error))
                    {
                        ucMsg.ShowError(objOrg.Error);
                    }
                    else
                    {
                        ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    }



                }
            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }

    protected void ddlSerHierarchyLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            DataSet ds = new DataSet();
            ddlLocationSearch.Items.Clear();

            using (OrgHierarchyData objOrgn = new OrgHierarchyData())
            {
                objOrgn.HierarchyLevelID = Convert.ToInt16(ddlSerHierarchyLevel.SelectedValue);
                ds = objOrgn.GetParallelOrgnHierarchyLocationInfo();
                if (objOrgn.TotalRecords > 0)
                {
                    String[] colArray = { "ParallelOrgnhierarchyID", "LocationName" };
                    PageBase.DropdownBinding(ref ddlLocationSearch, ds.Tables[0], colArray);
                }
                else
                {
                    ddlLocationSearch.Items.Insert(0, new ListItem("Select", "0"));
                    // ucMsg.ShowInfo("No Record Found.");
                }

            };

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
}
