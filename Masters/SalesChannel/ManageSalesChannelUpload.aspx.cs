#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*/
 * ================================================================================================
 * ================================================================================================
 * COPYRIGHT (c) 2016 Zed Axis Technologies (P) Ltd.
 * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART,
 * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
 * ================================================================================================
 * Created By : Rakesh Raj
 * Created On : 15 June 2018
 * Description : Uploading(Add/Edit) Sales Channel Records from Excel Sheet.
 * ================================================================================================
 * Change Log: 
 * ------------- 
 * DD-MMM-YYYY, Name, #CCXX, Description
  * * 6-July-2018, #CC01, Rakesh Raj, InvalidData Link Added and Hide Grid
 * ====================================================================================================
 */
#endregion

using System;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;
using System.Data.SqlClient;
using System.Globalization;


public partial class Masters_SalesChannel_ManageSalesChannelUpload : PageBase
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
            Response.Redirect("ManageSalesChannel.aspx");
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

                isSuccess = UploadFile.uploadValidExcel(ref dsUpload, "SalesChannelUpload");

                if (isSuccess == 1)
                {
                    InsertData(dsUpload);
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

                        //ucMsg.ShowInfo(Resources.Messages.CheckErrorGrid);
                        //Btnsave.Enabled = false;
                        //pnlGrid.Visible = true;
                        ////GridUpload.Visible = true;
                        //GridUpload.Columns[27].Visible = true;
                        //GridUpload.DataSource = dsUpload;
                        //GridUpload.DataBind();
                        //updGrid.Update();
                        break;
                    case 1:
                        UpdateData(dsUpload.Tables[0]);
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
                ucMsg.ShowInfo("File size should be less than " + PageBase.ValidExcelLength + " KB");
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



    //protected void GridUpload_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    try
    //    {
    //        //GridUpload.PageIndex = e.NewPageIndex;
    //        DataTable dt = new DataTable();
    //        //GridUpload.DataSource = (DataTable)ViewState["UploadData"];

    //        //GridUpload.DataBind();
    //    }

    //    catch (Exception ex)
    //    {
    //        ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
    //    }
    //}
    //}
    //#CC01

    protected void DwnldReferenceCode_Click(object sender, EventArgs e)
    {
        using (CommonData obj = new CommonData())
        {
            DataSet dsReferenceCode = obj.DownloadRefCodeExcel(7);
            if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
            {
                try
                {
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "RefCodeSalesChannel";
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
        try
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
                    //validate SalesChannelType 

                    string strColName1 = "'" + dtUpload.Rows[0]["SalesChannelType"].ToString().Trim() + "'";
                  //  DataRow[] dr1 = dtUpload.Select(strColName1);

                    if (dtUpload.Rows.Count > 1 && i >0)
                    {
                      
                        string strColNameDup = "'" + dtUpload.Rows[1]["SalesChannelType"].ToString().Trim() + "'";

                        if (strColName1 != strColNameDup)
                        {
                            counter = counter + 1;

                            if (dtUpload.Rows[i]["Error"].ToString() == "" && dtUpload.Rows[i]["Error"].ToString() == string.Empty)
                            {
                                dtUpload.Rows[i]["Error"] = "Same Type of SalesChannel can be only uploaded at once!";
                            }
                            else
                                dtUpload.Rows[i]["Error"] += ";Same Type of SalesChannel can be only uploaded at once!";
                        }
                    }


                    //In case of Sales Channel Type = 'Ware House' then ParentSalesChannelCode is optional else mendatory


                    if (!string.IsNullOrEmpty(dtUpload.Rows[i]["SalesChannelType"].ToString()))
                    {
                        string strColNameType = "'" + dtUpload.Rows[i]["SalesChannelType"].ToString().Trim() + "'";

                        if (dtUpload.Rows[i]["SalesChannelType"].ToString().ToLower().Trim() != "warehouse" && string.IsNullOrEmpty(dtUpload.Rows[i]["ParentSalesChannelCode"].ToString()))
                        {
                            counter = counter + 1;

                            if (dtUpload.Rows[i]["Error"].ToString() == "" && dtUpload.Rows[i]["Error"].ToString() == string.Empty)
                            {
                                dtUpload.Rows[i]["Error"] = "ParentSalesChannelCode required!";
                            }
                            else
                                dtUpload.Rows[i]["Error"] += ";ParentSalesChannelCode required!";

                        }
                        
                    }


                    //Validate SalesChannelCode

                    string strColName2 = "SalesChannelCode = '" + dtUpload.Rows[i]["SalesChannelCode"].ToString().Trim() + "'";
                    DataRow[] dr2 = dtUpload.Select(strColName2);

                    if (dr2.Length > 1)
                    {
                        counter = counter + 1;
                        if (dtUpload.Rows[i]["Error"].ToString() == "" && dtUpload.Rows[i]["Error"].ToString() == string.Empty)
                        {
                            dtUpload.Rows[i]["Error"] = "SalesChannelCode " + Resources.Messages.UniqeMsg;
                        }
                        else
                            dtUpload.Rows[i]["Error"] += ";SalesChannelCode " + Resources.Messages.UniqeMsg;
                    }


                    if (!string.IsNullOrEmpty(dtUpload.Rows[i]["BusinessStartDate"].ToString()))
                    {
                       DateTime Temp;

                        if (DateTime.TryParseExact(dtUpload.Rows[i]["BusinessStartDate"].ToString().Trim(), "MM/dd/yyyy", null, DateTimeStyles.None, out Temp) == false)
                       // if (DateTime.TryParse(dtUpload.Rows[i]["BusinessStartDate"].ToString().Trim(), out Temp) == false)
                            {
                                counter = counter + 1;
                                dtUpload.Rows[i]["Error"] = "BusinessStartDate is Invalid!(Valid: MM/DD/YYYY)";
                            }
                    }

                    if (!string.IsNullOrEmpty(dtUpload.Rows[i]["OpeningStockDate"].ToString()))
                    {
                        DateTime Temp;

                         if (DateTime.TryParseExact(dtUpload.Rows[i]["OpeningStockDate"].ToString().Trim(), "MM/dd/yyyy", null, DateTimeStyles.None, out Temp) == false)
                       // if (DateTime.TryParse(dtUpload.Rows[i]["OpeningStockDate"].ToString().Trim(), out Temp) == false)
                        {
                            counter = counter + 1;
                            dtUpload.Rows[i]["Error"] += ";OpeningStockDate is Invalid!(Valid: MM/DD/YYYY)";
                        }
                       
                    }

                    string StrPSalt = "";
                    string strPassword = dtUpload.Rows[i]["Password"].ToString().Trim();
                    if (!string.IsNullOrEmpty(dtUpload.Rows[i]["Password"].ToString()))
                    {
                        using (Authenticates ObjAuth = new Authenticates())
                        {
                            StrPSalt = ObjAuth.GenerateSalt(strPassword.Trim().Length);
                           // objSalesChannel.Password = ObjAuth.EncryptPassword(txtpassword.Text.Trim(), StrPSalt);
                            dtUpload.Rows[i]["Password"] = ObjAuth.EncryptPassword(strPassword.Trim(), StrPSalt) + "$" + StrPSalt +  "$"+ PasswordExp;

                        }
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
            
           //#CC01
          //  hideUnhideControls(dtUpload);

        }
        catch (Exception)
        {

            throw;
        }
    }

    //#CC01

    //private void hideUnhideControls(DataTable dtUpload)
    //{
        
    //    if (counter == 0)
    //    {
    //        //Btnsave.Enabled = true;
    //        //GridUpload.Columns[27].Visible = false;
    //        ucMsg.Visible = false;
    //    }

    //    else
    //    {
    //        ucMsg.ShowInfo(Resources.Messages.CheckErrorGrid);
    //        ////GridUpload.Columns[4].Visible = true;
    //        //GridUpload.Columns[27].Visible = true;
    //        ucMsg.Visible = true;

    //        //Btnsave.Enabled = false;
    //    }
    //    if (dtUpload.Rows.Count > 0)
    //    {
    //        dvUploadPreview.Visible = true;
    //        //pnlGrid.Visible = true;
    //        //Btnsave.Visible = true;
    //        BtnCancel.Visible = true;
         
    //        //GridUpload.DataSource = dtUpload;
    //        ViewState["UploadData"] = dtUpload;
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
                    DataSet dsResult = obj.BulkUploadExcel("prcBulkUploadSalesChannel");
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
                        string strFileName = "InvalidData" + DateTime.Now.Ticks;
                        PageBase.ExportInExcel(dsResult, strFileName);
                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
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

    public bool UpdateBCP(DataTable dtUpload)
    {
        try
        {

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "BulkUploadSalesChannel";
                bulkCopy.ColumnMappings.Add("SalesChannelType", "SalesChannelType");
                bulkCopy.ColumnMappings.Add("SalesChannelCode", "SalesChannelCode");
                bulkCopy.ColumnMappings.Add("SalesChannelName", "SalesChannelName");
                bulkCopy.ColumnMappings.Add("ContactPerson", "ContactPerson");
                bulkCopy.ColumnMappings.Add("BusinessStartDate", "BusinessStartDate");
                bulkCopy.ColumnMappings.Add("LoginName", "LoginName");
                bulkCopy.ColumnMappings.Add("Password", "Password");
                bulkCopy.ColumnMappings.Add("StateName", "StateName");
                bulkCopy.ColumnMappings.Add("CityName", "CityName");
                bulkCopy.ColumnMappings.Add("AreaName", "AreaName");
                bulkCopy.ColumnMappings.Add("AddressLine1", "AddressLine1");
                bulkCopy.ColumnMappings.Add("AddressLine2", "AddressLine2");
                bulkCopy.ColumnMappings.Add("PinCode", "PinCode");
                bulkCopy.ColumnMappings.Add("PhoneNo", "PhoneNo");
                bulkCopy.ColumnMappings.Add("MobileNo1", "MobileNo1");
                bulkCopy.ColumnMappings.Add("MobileNo2", "MobileNo2");
                bulkCopy.ColumnMappings.Add("Fax", "Fax");
                bulkCopy.ColumnMappings.Add("PanNo", "PanNo");
                bulkCopy.ColumnMappings.Add("Email", "Email");
                bulkCopy.ColumnMappings.Add("GSTNo", "GSTNo");
                bulkCopy.ColumnMappings.Add("TINNo", "TINNo");
                bulkCopy.ColumnMappings.Add("DOB", "DOB");
                bulkCopy.ColumnMappings.Add("DateofAnniversary", "DOA");
                bulkCopy.ColumnMappings.Add("ReportHierarchyCode", "ReportHierarchyCode");
                bulkCopy.ColumnMappings.Add("ParentSalesChannelCode", "ParentSalesChannelCode");
                bulkCopy.ColumnMappings.Add("OpeningStockDate", "OpeningStockDate");
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
