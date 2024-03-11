#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*/
 * ================================================================================================
 * ================================================================================================
 * COPYRIGHT (c) 2016 Zed Axis Technologies (P) Ltd.
 * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART,
 * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
 * ================================================================================================
 * Created By : Rakesh Raj
 * Created On : 11 June 2018
 * Description : Uploading(Add/Edit) SKUr Records from Excel Sheet.
 * ================================================================================================
 * Change Log: 
 * ------------- 
 * DD-MMM-YYYY, Name, #CCXX, Description
 * ====================================================================================================
 * * 6-July-2018, #CC01, Rakesh Raj, InvalidData Link Added and Hide Grid
 * 10-Oct-2019, #CC02, Balram Jha- Addition of SKU Keyword upload section.
 */
#endregion

using System;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;
using System.Data.SqlClient;

public partial class Masters_Common_ManageSKUUpload : PageBase
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
            //pnlGrid.Visible = false;
        }
    }

    protected void rdoSelectMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoSelectMode.SelectedValue == "2")
        {
            Response.Redirect("ManageSKU.aspx");
        }

    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        //pnlGrid.Visible = false;
        
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
            
                isSuccess = UploadFile.uploadValidExcel(ref dsUpload, "SKUUpload");

                if (isSuccess == 1)
                {
                   // InsertData(dsUpload);
                    if (counter > 0)
                        isSuccess = 2;
                }

                switch (isSuccess)
                {
                    case 0:
                        ucMsg.ShowInfo(UploadFile.Message);
                        //pnlGrid.Visible = false;
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

                       // ucMsg.ShowInfo(Resources.Messages.CheckErrorGrid);
                        //Btnsave.Enabled = false;
                        //pnlGrid.Visible = true;
                        //GridUpload.Columns[9].Visible = true;

                        //GridUpload.DataSource = dsUpload;
                        //GridUpload.DataBind();
                        //updGrid.Update();
                        break;
                    case 1:
                        UpdateData(dsUpload.Tables[0],0);
                      //  InsertData(dsUpload);
                        break;
                    case 3:
                        ucMsg.ShowInfo(UploadFile.Message);
                        break;


                }

            }
            else if (UploadCheck == 2)
            {
                //pnlGrid.Visible = false;
                ucMsg.ShowInfo(Resources.Messages.UploadXlxs);
            }
            else if (UploadCheck == 3)
            {
                //pnlGrid.Visible = false;
                ucMsg.ShowInfo(Resources.Messages.SelectFile);

            }
            else if (UploadCheck == 4)
            {
                //pnlGrid.Visible = false;
                ucMsg.ShowInfo(Resources.Messages.FileSize + PageBase.ValidExcelLength + " KB");
            }
            else
            {
                //pnlGrid.Visible = false;
                ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            }


            //updGrid.Update();
        }
        catch (Exception ex)
        {
            //pnlGrid.Visible = false;
            ucMsg.ShowError(ex.Message.ToString());
            //clsException.clsHandleException.fncHandleException(ex, "");
        }
    }

    protected void btnUploadKeyWord_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsUpload = null;
            byte isSuccess = 1;
            Int16 UploadCheck = 0;
            String RootPath = Server.MapPath("../../");
            UploadFile.RootFolerPath = RootPath;

            UploadCheck = UploadFile.IsExcelFile(FileUpload2, ref strUploadedFileName);
            if (UploadCheck == 1)
            {
                UploadFile.UploadedFileName = strUploadedFileName;

                isSuccess = UploadFile.uploadValidExcel(ref dsUpload, "KeywordUpload");

                if (isSuccess == 1)
                {
                    // InsertData(dsUpload);
                    if (counter > 0)
                        isSuccess = 2;
                }

                switch (isSuccess)
                {
                    case 0:
                        ucMsg.ShowInfo(UploadFile.Message);
                        //pnlGrid.Visible = false;
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

                        // ucMsg.ShowInfo(Resources.Messages.CheckErrorGrid);
                        //Btnsave.Enabled = false;
                        //pnlGrid.Visible = true;
                        //GridUpload.Columns[9].Visible = true;

                        //GridUpload.DataSource = dsUpload;
                        //GridUpload.DataBind();
                        //updGrid.Update();
                        break;
                    case 1:
                        UpdateData(dsUpload.Tables[0],1);
                        //  InsertData(dsUpload);
                        break;
                    case 3:
                        ucMsg.ShowInfo(UploadFile.Message);
                        break;


                }

            }
            else if (UploadCheck == 2)
            {
                //pnlGrid.Visible = false;
                ucMsg.ShowInfo(Resources.Messages.UploadXlxs);
            }
            else if (UploadCheck == 3)
            {
                //pnlGrid.Visible = false;
                ucMsg.ShowInfo(Resources.Messages.SelectFile);

            }
            else if (UploadCheck == 4)
            {
                //pnlGrid.Visible = false;
                ucMsg.ShowInfo(Resources.Messages.FileSize + PageBase.ValidExcelLength + " KB");
            }
            else
            {
                //pnlGrid.Visible = false;
                ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            }


            //updGrid.Update();
        }
        catch (Exception ex)
        {
            //pnlGrid.Visible = false;
            ucMsg.ShowError(ex.Message.ToString());
            //clsException.clsHandleException.fncHandleException(ex, "");
        }
    }
    
    //#CC01
    //protected void Btnsave_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        UpdateData((DataTable)//ViewState["UploadData"]);

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
    //        //GridUpload.PageIndex = e.NewPageIndex;
    //        DataTable dt = new DataTable();
    //        //GridUpload.DataSource = (DataTable)//ViewState["UploadData"];

    //        //GridUpload.DataBind();
    //    }

    //    catch (Exception ex)
    //    {
    //        ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
    //    }
    //}
    //#CC01
    protected void DwnldRefCodePC_Click(object sender, EventArgs e)
    {
        using (CommonData obj = new CommonData())
        {

            DataSet dsReferenceCode = obj.DownloadRefCodeExcel(5);

            if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
            {
                try
                {
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "RefCodeModel";
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

    protected void LinkButtonDownLoadTallyItem_Click(object sender, EventArgs e)
    {
        try
        {
            using (CommonData obj = new CommonData())
            {

                DataSet dsReferenceCode = obj.DownloadRefCodeExcel(11);

                if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
                {
                    try
                    {
                        String FilePath = Server.MapPath("../../");
                        string FilenameToexport = "TallyItemSKUMapping";
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
        catch(Exception ex)
        {
            ucMsg.ShowError(ex.Message);
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
                //validate SKUName



                string strColName1 = "SKUName = '" + dtUpload.Rows[i]["SKUName"].ToString().Trim() + "'";
                DataRow[] dr1 = dtUpload.Select(strColName1);

                if (dr1.Length > 1)
                {
                    counter = counter + 1;
                    if (dtUpload.Rows[i]["Error"].ToString() == "" && dtUpload.Rows[i]["Error"].ToString() == string.Empty)
                    {
                        dtUpload.Rows[i]["Error"] = "SKUName " + Resources.Messages.UniqeMsg;
                    }
                    else
                        dtUpload.Rows[i]["Error"] += ";SKUName " + Resources.Messages.UniqeMsg;
                 }

                //Validate SKUCode

                    string strColName2 = "SKUCode = '" + dtUpload.Rows[i]["SKUCode"].ToString().Trim() + "'";
                    DataRow[] dr2 = dtUpload.Select(strColName2);

                    if (dr2.Length > 1)
                    {
                        counter = counter + 1;
                        if (dtUpload.Rows[i]["Error"].ToString() == "" && dtUpload.Rows[i]["Error"].ToString() == string.Empty)
                        {
                            dtUpload.Rows[i]["Error"] = "SKUCode " + Resources.Messages.UniqeMsg;
                        }
                        else
                            dtUpload.Rows[i]["Error"] += ";SKUCode " + Resources.Messages.UniqeMsg;
                    }


                    string strColName3 = "ModelName = '" + dtUpload.Rows[i]["ModelName"].ToString().Trim() + "'";
                    DataRow[] dr3 = dtUpload.Select(strColName3);

                    if (dr2.Length > 1)
                    {
                        counter = counter + 1;
                        if (dtUpload.Rows[i]["Error"].ToString() == "" && dtUpload.Rows[i]["Error"].ToString() == string.Empty)
                        {
                            dtUpload.Rows[i]["Error"] = "ModelName " + Resources.Messages.UniqeMsg;
                        }
                        else
                            dtUpload.Rows[i]["Error"] += ";ModelName " + Resources.Messages.UniqeMsg;
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

       // hideUnhideControls(dtUpload);
    }
    //#CC01
    //private void hideUnhideControls(DataTable dtUpload)
    //{
        
    //    if (counter == 0)
    //    {
    //        //Btnsave.Enabled = true;
    //        //GridUpload.Columns[9].Visible = false;
    //        ucMsg.Visible = false;
    //    }
    //    else
    //    {
    //        //GridUpload.Columns[9].Visible = true;
    //        ucMsg.ShowInfo(Resources.Messages.CheckErrorGrid);
    //        ucMsg.Visible = true;
    //        //Btnsave.Enabled = false;
    //    }
    //    if (dtUpload.Rows.Count > 0)
    //    {
    //        dvUploadPreview.Visible = true;
    //        //pnlGrid.Visible = true;
    //        //Btnsave.Visible = true;
    //        BtnCancel.Visible = true;
    //        ////GridUpload.Visible = true;
            

    //        //GridUpload.DataSource = dtUpload;
    //        //ViewState["UploadData"] = dtUpload;
    //        //GridUpload.DataBind();
    //        //updGrid.Update();

    //    }
    //    else
    //        //pnlGrid.Visible = false;
    //}

    void ClearForm()
    {

        hlnkInvalid.Text = "";
        /* Add End */
        //pnlGrid.Visible = false;
        //updGrid.Update();
    }


    public void UpdateData(DataTable dt, Int16 intTable)//intTable: 0=SKU, 1= Keyword
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
            if ((UpdateBCP(dt, intTable)) == true)
            {
                using (CommonData obj = new CommonData())
                {
                    DataSet dsResult=new DataSet();
                    obj.SessionID = Convert.ToString(dt.Rows[0]["SessionID"]);
                    obj.UserID = UserId;
                    if(intTable==0)
                     dsResult = obj.BulkUploadExcel("prcBulkSKUUpload");
                    else
                        dsResult = obj.BulkUploadExcel("prcBulkKeywordUpload");
                    if (obj.intOutParam == 0)
                    {
                        ucMsg.ShowSuccess(Resources.Messages.DataUploadSuccess);
                        //pnlGrid.Visible = false;
                        //GridUpload.DataSource = null;
                        //GridUpload.DataBind();
                        //ViewState["UploadData"] = null;
                        //updGrid.Update();
                    }
                    else if (obj.intOutParam == 1)
                    {
                        ucMsg.ShowError(obj.Error);
                        //pnlGrid.Visible = false;
                        //GridUpload.DataSource = null;
                        //GridUpload.DataBind();
                        //ViewState["UploadData"] = null;
                        //updGrid.Update();
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
                        //GridUpload.DataSource = null;
                        //GridUpload.DataBind();
                        //ViewState["UploadData"] = null;
                        //updGrid.Update();
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

    public bool UpdateBCP(DataTable dtUpload,Int16 intTable)//0=SKU, 1= Keyword
    {
        try
        {

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                if (intTable == 0)
                {
                    bulkCopy.BatchSize = 20000;
                    bulkCopy.DestinationTableName = "BulkUploadSKU";
                    bulkCopy.ColumnMappings.Add("SKUName", "SKUName");
                    bulkCopy.ColumnMappings.Add("SKUCode", "SKUCode");
                    bulkCopy.ColumnMappings.Add("SKUDesc", "SKUDesc");
                    bulkCopy.ColumnMappings.Add("ModelCode", "ModelCode");
                    bulkCopy.ColumnMappings.Add("ColorName", "ColorName");
                    bulkCopy.ColumnMappings.Add("CartonSize", "CartonSize");
                    bulkCopy.ColumnMappings.Add("Attribute1", "Attribute1");
                    bulkCopy.ColumnMappings.Add("Attribute2", "Attribute2");
                    bulkCopy.ColumnMappings.Add("Active", "Active");
                    bulkCopy.ColumnMappings.Add("CreatedBy", "CreatedBy");
                    bulkCopy.ColumnMappings.Add("SessionID", "SessionID");

                    bulkCopy.WriteToServer(dtUpload);
                }
                if (intTable == 1)
                {
                    bulkCopy.BatchSize = 20000;
                    bulkCopy.DestinationTableName = "BulkUploadSKUKeyword";
                    bulkCopy.ColumnMappings.Add("SKUCode", "SKUCode");
                    bulkCopy.ColumnMappings.Add("Keyword", "Keyword");
                    bulkCopy.ColumnMappings.Add("SalesChannelCode", "SalesChannelCode");
                    bulkCopy.ColumnMappings.Add("Active", "Active");
                    bulkCopy.ColumnMappings.Add("SessionID", "SessionID");

                    bulkCopy.WriteToServer(dtUpload);
                }
                return true;
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            return false;
        }
    }
    //#CC01
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

    #endregion




    
}
