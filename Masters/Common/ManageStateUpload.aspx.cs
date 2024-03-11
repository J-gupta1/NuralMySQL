#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*/
 * ================================================================================================
 * ================================================================================================
 * COPYRIGHT (c) 2016 Zed Axis Technologies (P) Ltd.
 * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART,
 * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
 * ================================================================================================
 * Created By : Rakesh Raj
 * Created On : 12 June 2018
 * Description : Uploading(Add/Edit) State Records from Excel Sheet.
 * ================================================================================================
 * Change Log: 
 * ------------- 
 * DD-MMM-YYYY, Name, #CCXX, Description
 * * 9-July-2018, Rakesh Raj, #CC01 InvalidData Link Added and Hide Grid
 * ====================================================================================================
 */
#endregion

using System;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;
using System.Data.SqlClient;

public partial class Masters_Common_ManageStateUpload : PageBase
{

    #region Variables/ Instances

    string strUploadedFileName = string.Empty;
    int counter = 0;
    UploadFile UploadFile = new UploadFile();

    #endregion

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        hlnkInvalid.Text = "";
        ucMsg.ShowControl = false;
        if (!IsPostBack)
        {
            ucMsg.Visible = false;
            //#CC01 pnlGrid.Visible = false;
        }
    }

    protected void rdoSelectMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoSelectMode.SelectedValue == "2")
        {
            Response.Redirect("ManageStateMasterVer2.aspx");
        }

    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        //#CC01 pnlGrid.Visible = false;

        try
        {
            DataSet dsUpload = null;
            byte isSuccess = 1;
            Int16 UploadCheck = 0;
            String RootPath = Server.MapPath("../../");
            UploadFile.RootFolerPath = RootPath;

            UploadCheck = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            if (UploadCheck == 1)
            {
                UploadFile.UploadedFileName = strUploadedFileName;
                         
                isSuccess = UploadFile.uploadValidExcel(ref dsUpload, "StateUpload");

                //#CC01 start
                if (isSuccess == 1)
                {
                    InsertData(dsUpload);
                    if (counter > 0)
                        isSuccess = 2;
                }//#CC01 end

                switch (isSuccess)
                {
                    case 0:
                        ucMsg.ShowInfo(UploadFile.Message);
                        //#CC01 pnlGrid.Visible = false;
                        break;
                    case 2:

                        hlnkInvalid.Visible = true;
                        strUploadedFileName = "InvalidData" + DateTime.Now.Ticks;
                        ucMsg.ShowInfo(Resources.Messages.PartialDataUpload);
                        DataView dvError = dsUpload.Tables[0].DefaultView;
                        dvError.RowFilter = "Error<>''";
                        DataTable dtError = dvError.ToTable();
                        DataSet dsError = new DataSet();
                        dsError.Tables.Add(dtError);
                        PageBase.ExportInExcel(dsError, strUploadedFileName);
                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strUploadedFileName + ".xlsx";
                        hlnkInvalid.Text = "Invalid Data";

                        //#CC01 ucMsg.ShowInfo(Resources.Messages.CheckErrorGrid);
                        //#CC01 Btnsave.Enabled = false;
                        //#CC01 pnlGrid.Visible = true;
                        //#CC01 GridUpload.Columns[4].Visible = true;
                        
                        //#CC01 GridUpload.DataSource = dsUpload;
                        //#CC01 GridUpload.DataBind();
                        //#CC01 updGrid.Update();
                        break;
                    case 1:
                     UpdateData(dsUpload.Tables[0]);
                    //#CC01 InsertData(dsUpload);
                        break;
                    case 3:
                        ucMsg.ShowInfo(UploadFile.Message);
                        break;


                }

            }
            else if (UploadCheck == 2)
            {
                //#CC01 pnlGrid.Visible = false;
                ucMsg.ShowInfo(Resources.Messages.UploadXlxs);
            }
            else if (UploadCheck == 3)
            {
                //#CC01 pnlGrid.Visible = false;
                ucMsg.ShowInfo(Resources.Messages.SelectFile);

            }
            else if (UploadCheck == 4)
            {
                //#CC01 pnlGrid.Visible = false;
                ucMsg.ShowInfo(Resources.Messages.FileSize + PageBase.ValidExcelLength + " KB");
            }
            else
            {
                //#CC01 pnlGrid.Visible = false;
                ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            }


            //#CC01 updGrid.Update();
        }
        catch (Exception ex)
        {
            //#CC01 pnlGrid.Visible = false;
            ucMsg.ShowError(ex.Message.ToString());
            //clsException.clsHandleException.fncHandleException(ex, "");
        }
    }

//#CC01 Start
    //protected void Btnsave_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        UpdateData((DataTable)ViewState["UploadData"]);
                        
    //    }
    //    catch (Exception ex)
    //    {
    //        ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
    //    }
    //}

    //protected void BtnCancel_Click(object sender, EventArgs e)
    //{
    //    ucMsg.ShowControl = false;
    //    ClearForm();

    //}

    //protected void GridUpload_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    try
    //    {
    //        //#CC01 GridUpload.PageIndex = e.NewPageIndex;
    //        DataTable dt = new DataTable();
    //        //#CC01 GridUpload.DataSource = (DataTable)ViewState["UploadData"];

    //        //#CC01 GridUpload.DataBind();
    //    }

    //    catch (Exception ex)
    //    {
    //        ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
    //    }
    //}
    //#CC01 End

    protected void DwnldReferenceCode_Click(object sender, EventArgs e)
    {
        using (CommonData obj = new CommonData())
        {
            DataSet dsReferenceCode = obj.DownloadRefCodeExcel(6);
            if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
            {
                try
                {                   
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "RefCodeCountry";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dsReferenceCode, FilenameToexport);
                    // ViewState["Table"] = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                ucMsg.ShowError(Resources.Messages.NoRecord);

            }

        }

    }

    #endregion

    #region Methods

    private void InsertData(DataSet dsUpload)
    {
        // int mbCount = 0;
        DataTable dtUpload = dsUpload.Tables[0];
        DataColumn dcError = new DataColumn();
        dcError.DataType = System.Type.GetType("System.String");
        dcError.ColumnName = "Error";

        if (dtUpload.Columns.Contains("Error") == false)
        {
            dtUpload.Columns.Add(dcError);

        }

        for (int i = 0; i <= dtUpload.Rows.Count - 1; i++)
        {
            if (dtUpload != null && dtUpload.Rows.Count > 0)
            {
                //validate StateName


                string strColName1 = "StateName = '" + dtUpload.Rows[i]["StateName"].ToString().Trim() + "'";
                DataRow[] dr1 = dtUpload.Select(strColName1);

                if (dr1.Length > 1)
                {
                    counter = counter + 1;
                    if (dtUpload.Rows[i]["Error"].ToString() == "" && dtUpload.Rows[i]["Error"].ToString() == string.Empty)
                    {
                        dtUpload.Rows[i]["Error"] = "StateName " + Resources.Messages.UniqeMsg;
                    }
                    else
                        dtUpload.Rows[i]["Error"] += ";StateName " + Resources.Messages.UniqeMsg;
                }

                //Validate StateCode

                string strColName2 = "StateCode = '" + dtUpload.Rows[i]["StateCode"].ToString().Trim() + "'";
                DataRow[] dr2 = dtUpload.Select(strColName2);

                if (dr2.Length > 1)
                {
                    counter = counter + 1;
                    if (dtUpload.Rows[i]["Error"].ToString() == "" && dtUpload.Rows[i]["Error"].ToString() == string.Empty)
                    {
                        dtUpload.Rows[i]["Error"] = "StateCode " + Resources.Messages.UniqeMsg;
                    }
                    else
                        dtUpload.Rows[i]["Error"] += ";StateCode " + Resources.Messages.UniqeMsg;
                }


                //Validate Active 
                if (string.IsNullOrEmpty(dtUpload.Rows[i]["Active"].ToString()))
                {
                    counter = counter + 1;
                    if (dtUpload.Rows[i]["Error"].ToString() == "" && dtUpload.Rows[i]["Error"].ToString() == string.Empty)
                    {
                        dtUpload.Rows[i]["Error"] = "Active " + Resources.Messages.BlankMsg;
                    }
                    else
                        dtUpload.Rows[i]["Error"] += ";Active " + Resources.Messages.BlankMsg;
                }

                if (!string.IsNullOrEmpty(dtUpload.Rows[i]["Active"].ToString()) && dtUpload.Rows[i]["Active"].ToString().Trim().ToLower() != "yes")
                {
                    if (dtUpload.Rows[i]["Active"].ToString().Trim().ToLower() != "no")
                    {
                        counter = counter + 1;
                        if (dtUpload.Rows[i]["Error"].ToString() == "" && dtUpload.Rows[i]["Error"].ToString() == string.Empty)
                        {
                            dtUpload.Rows[i]["Error"] = Resources.Messages.YesNoOnly + " in Active!";
                        }
                        else
                            dtUpload.Rows[i]["Error"] += ";" + Resources.Messages.YesNoOnly + " in Active!";
                    }
                }

            }

        }

        // #CC01 hideUnhideControls(dtUpload);
    }
    // #CC01 Start
    //private void hideUnhideControls(DataTable dtUpload)
    //{
        
    //    // no error
    //    if (counter == 0)
    //    {
    //        //#CC01 Btnsave.Enabled = true;
    //        //#CC01 GridUpload.Columns[4].Visible = false;
    //        ucMsg.Visible = false;
    //    }

    //    else
    //    {
    //        // in case of error
    //        //#CC01 GridUpload.Columns[4].Visible = true;
    //        //#CC01 Btnsave.Enabled = false;
    //        ucMsg.ShowInfo(Resources.Messages.CheckErrorGrid);
    //        ucMsg.Visible = true;

    //    }
    //    if (dtUpload.Rows.Count > 0)
    //    {
    //        dvUploadPreview.Visible = true;
    //        //#CC01 pnlGrid.Visible = true;
    //        //#CC01 Btnsave.Visible = true;
    //        BtnCancel.Visible = true;
    //        ////#CC01 GridUpload.Visible = true;
            

    //        //#CC01 GridUpload.DataSource = dtUpload;
    //        ViewState["UploadData"] = dtUpload;
    //        //#CC01 GridUpload.DataBind();
    //        //#CC01 updGrid.Update();

    //    }
    //    else
    //        //#CC01 pnlGrid.Visible = false;
    //}
    // #CC01 End

    void ClearForm()
    {

        hlnkInvalid.Text = "";
        /* Add End */
        //#CC01 pnlGrid.Visible = false;
        //#CC01 updGrid.Update();
    }


    public void UpdateData(DataTable dt)
    {
        try
        {
            string guid = Guid.NewGuid().ToString();

            if (dt.Columns.Contains("Error"))
            {
                dt.Columns.Remove("Error");
            }

            if (!dt.Columns.Contains("SessionID"))
                dt.Columns.Add(AddColumn(guid, "SessionID", typeof(System.String)));
            else
            {
                dt.Columns.Remove("SessionID");
                dt.Columns.Add(AddColumn(guid, "SessionID", typeof(System.String)));
            }
            if (!dt.Columns.Contains("CreatedBy"))
                dt.Columns.Add(AddColumn(Convert.ToString(PageBase.UserId), "CreatedBy", typeof(int)));
            if ((UpdateBCP(dt)) == true)
            {
                using (CommonData obj = new CommonData())
                {
                    obj.SessionID = Convert.ToString(dt.Rows[0]["SessionID"]);
                    DataSet dsResult = obj.BulkUploadExcel("prcBulkStateUpload");
                    if (obj.intOutParam == 0)
                    {
                        ucMsg.ShowSuccess(Resources.Messages.DataUploadSuccess);
                        //#CC01 pnlGrid.Visible = false;
                        //#CC01 GridUpload.DataSource = null;
                        //#CC01 GridUpload.DataBind();
                        //#CC01 ViewState["UploadData"] = null;
                        //#CC01 updGrid.Update();
                    }
                    else if (obj.intOutParam == 1)
                    {
                        ucMsg.ShowError(obj.Error);
                        //#CC01 pnlGrid.Visible = false;
                        //#CC01 GridUpload.DataSource = null;
                        //#CC01 GridUpload.DataBind();
                        //#CC01 ViewState["UploadData"] = null;
                        //#CC01 updGrid.Update();
                    }
                    else if (obj.intOutParam == 2)
                    {
                        ucMsg.ShowInfo(Resources.Messages.PartialDataUpload);
                        //pnlGrid.Visible = false;
                        hlnkInvalid.Visible = true;
                        strUploadedFileName = "InvalidData" + DateTime.Now.Ticks;
                        PageBase.ExportInExcel(dsResult, strUploadedFileName);
                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strUploadedFileName + ".xlsx";
                        hlnkInvalid.Text = "Invalid Data";
                        //#CC01 GridUpload.DataSource = null;
                        //#CC01 GridUpload.DataBind();
                        //#CC01 ViewState["UploadData"] = null;
                        //#CC01 updGrid.Update();
                    }
                }

            }
            else
            {
                ucMsg.ShowInfo(Resources.Messages.UpLoadFailed);
            }


        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }

    public bool UpdateBCP(DataTable dtUpload)
    {
        try
        {

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "BulkUploadState";
                bulkCopy.ColumnMappings.Add("StateName", "StateName");
                bulkCopy.ColumnMappings.Add("StateCode", "StateCode");
                bulkCopy.ColumnMappings.Add("CountryName", "CountryName");
                bulkCopy.ColumnMappings.Add("Active", "Active");
                bulkCopy.ColumnMappings.Add("CreatedBy", "CreatedBy");
                bulkCopy.ColumnMappings.Add("SessionID", "SessionID");

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

    // #CC01 Start
    //private void ExportInExcel(DataSet DsExport, string strFileName)
    //{
    //    try
    //    {
    //        if (DsExport != null && DsExport.Tables.Count > 0)
    //        {
    //            PageBase.ExportToExeclV2(DsExport, strFileName, DsExport.Tables.Count);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());

    //    }
    //}

    // #CC01 End
    #endregion

   
}
