#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*/
 * ================================================================================================
 * ================================================================================================
 * COPYRIGHT (c) 2016 Zed Axis Technologies (P) Ltd.
 * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART,
 * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
 * ================================================================================================
 * Created By : Rakesh Raj
 * Created On : 17 July 2018
 * Description : Uploading OutStandingAmount Records from Excel Sheet.
 * ================================================================================================
 * Change Log: 
 * ------------- 
 * DD-MMM-YYYY, Name, #CCXX, Description
 * ====================================================================================================
 * 24-Aug-2018, Rakesh Raj, #CC01, Added Search Feature same as ZedSalesV4
 * 05-Dec-2018, Sumit Maurya, #CC02, interface further modified for retailer also and to resolve pending issues as well (Done for Inovu).
 */
#endregion

using System;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;
using System.Data.SqlClient;

public partial class Masters_Common_OutStandingAmountUpload : PageBase
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
            GetSearchData(1);
            FillsalesChannelType();
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            if (ucOutStandingDate.Date.ToString().Length == 0)
            {
                ucMsg.ShowInfo(Resources.Messages.InvalidDateEntered);
                return;
            }

            DataSet dsUpload = null;
            byte isSuccess = 1;
            Int16 UploadCheck = 0;
            String RootPath = Server.MapPath("../../");
            UploadFile.RootFolerPath = RootPath;

            UploadCheck = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            if (UploadCheck == 1)
            {
                UploadFile.UploadedFileName = strUploadedFileName;

                isSuccess = UploadFile.uploadValidExcel(ref dsUpload, "UploadOutStandingAmount");

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
                ucMsg.ShowInfo(Resources.Messages.UploadXlxs);
            }
            else if (UploadCheck == 3)
            {
                ucMsg.ShowInfo(Resources.Messages.SelectFile);

            }
            else if (UploadCheck == 4)
            {
                ucMsg.ShowInfo("File size should be less than " + PageBase.ValidExcelLength + " KB");
            }
            else
            {
                ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            //clsException.clsHandleException.fncHandleException(ex, "");
        }
    }

    protected void DwnldReferenceCode_Click(object sender, EventArgs e)
    {
        using (CommonData obj = new CommonData())
        {
            obj.UserID = PageBase.UserId; /*#CC02 Added */
            DataSet dsReferenceCode = obj.DownloadRefCodeExcel(10);
            if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
            {
                try
                {
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "RefCodeOutStandingAmount";
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


    protected void btnExprtToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            GetSearchData(-1);
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }

    }
    protected void btnSearchUser_Click(object sender, EventArgs e)
    {
        if ((ucToDate.Date != "" && ucFromDate.Date == ""))
        {
            ucMsg.Visible = true;
            ucMsg.ShowInfo("Pleae provide Outstanding Date From.");
            return;
        }
        if ((ucToDate.Date == "" && ucFromDate.Date != ""))
        {
            ucMsg.Visible = true;
            ucMsg.ShowInfo("Pleae provide Outstanding Date To.");
            return;
        }
        if (ddlSalesChannelType.SelectedIndex == 0 & txtCompanyName.Text.Trim().Length == 0
            & (ucToDate.Date == "" || ucFromDate.Date == ""))
        {
            //Please select at least one search criteria!
            ucMsg.Visible = true;
            ucMsg.ShowInfo(Resources.Messages.SearchCriteriaBlank);
            return;
        }

        GetSearchData(1);

    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        ClearForm();
        GetSearchData(1);
    }

    #endregion

    #region Methods

    private void InsertData(DataSet dsUpload)
    {
        // int mbCount = 0;
        DataTable dtUpload = dsUpload.Tables[0];

        DataColumn dcDate = new DataColumn();
        dcDate.DataType = System.Type.GetType("System.String");
        dcDate.ColumnName = "OutstandingDate";

        DataColumn dcError = new DataColumn();
        dcError.DataType = System.Type.GetType("System.String");
        dcError.ColumnName = "Error";

        if (dtUpload.Columns.Contains("OutstandingDate") == false)
        {
            dtUpload.Columns.Add(dcDate);

        }

        if (dtUpload.Columns.Contains("Error") == false)
        {
            dtUpload.Columns.Add(dcError);

        }

        for (int i = 0; i <= dtUpload.Rows.Count - 1; i++)
        {
            if (dtUpload != null && dtUpload.Rows.Count > 0)
            {
                // Adding Outstanding date

                if (ucOutStandingDate.Date.ToString().Length > 0)
                {
                    dtUpload.Rows[i]["OutstandingDate"] = ucOutStandingDate.Date;
                }
                //validate SalesChannelCode

                //string strColName1 = "SalesChannelCode = '" + dtUpload.Rows[i]["SalesChannelCode"].ToString().Trim() + "'";
                //DataRow[] dr1 = dtUpload.Select(strColName1);

                //if (dr1.Length > 1)
                //{
                //    counter = counter + 1;
                //    if (dtUpload.Rows[i]["Error"].ToString() == "" && dtUpload.Rows[i]["Error"].ToString() == string.Empty)
                //    {
                //        dtUpload.Rows[i]["Error"] = "SalesChannelCode supplied multiple times.";
                //    }
                //    else
                //        dtUpload.Rows[i]["Error"] += ";SalesChannelCode supplied multiple times.";
                //}



                //Validate Active 
                if (string.IsNullOrEmpty(dtUpload.Rows[i]["Active"].ToString()))
                {
                    counter = counter + 1;
                    if (dtUpload.Rows[i]["Error"].ToString() == "" && dtUpload.Rows[i]["Error"].ToString() == string.Empty)
                    {
                        dtUpload.Rows[i]["Error"] = "Column Active cannot be left blank.";
                    }
                    else
                        dtUpload.Rows[i]["Error"] += ";Column Active cannot be left blank.";
                }

                if (!string.IsNullOrEmpty(dtUpload.Rows[i]["Active"].ToString()) && dtUpload.Rows[i]["Active"].ToString().Trim().ToLower() != "yes")
                {
                    if (dtUpload.Rows[i]["Active"].ToString().Trim().ToLower() != "no")
                    {
                        counter = counter + 1;
                        if (dtUpload.Rows[i]["Error"].ToString() == "" && dtUpload.Rows[i]["Error"].ToString() == string.Empty)
                        {
                            dtUpload.Rows[i]["Error"] = "Only Yes or No allowed in Active.";
                        }
                        else
                            dtUpload.Rows[i]["Error"] += ";Only Yes or No allowed in Active.";
                    }
                }

            }

        }

    }

    void ClearForm()
    {

        hlnkInvalid.Text = "";
        /*#CC02 Add Start */
        if (ddlSalesChannelType.Items.Count > 0)
            ddlSalesChannelType.SelectedValue = "0";
        txtCompanyName.Text = "";
        ucFromDate.TextBoxDate.Text = "";
        ucToDate.TextBoxDate.Text = "";
        /*#CC02 Add End */

    }

    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        GetSearchData(ucPagingControl1.CurrentPage);
    }

    void FillsalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.CompanyId = PageBase.ClientId;
            ObjSalesChannel.UserID = PageBase.UserId;
            ddlSalesChannelType.Items.Clear();
            string[] str = { "SalesChannelTypeID", "SalesChannelTypeName" };
            PageBase.DropdownBinding(ref ddlSalesChannelType, ObjSalesChannel.GetSalesChannelTypeV5API(), str);

        };
    }

    private void GetSearchData(int pageno)
    {
        ViewState["TotalRecords"] = 0;


        if (ViewState["CurrentPage"] == null)
        {
            pageno = 1;
            ViewState["CurrentPage"] = pageno;
        }
        try
        {
            DataSet dtSalesMan;
            using (SalesmanData objSalesMan = new SalesmanData())
            {
                if (ucFromDate.Date == "" && ucToDate.Date == "")
                { }
                else
                {
                    objSalesMan.FromDate = Convert.ToDateTime(ucFromDate.Date);
                    objSalesMan.ToDate = Convert.ToDateTime(ucToDate.Date);
                }

                objSalesMan.CompanyName = txtCompanyName.Text.Trim(); // SalesChannelName

                if (ddlSalesChannelType.SelectedIndex > 0)
                {
                    objSalesMan.SalesChannelType = ddlSalesChannelType.SelectedItem.Text.Trim();
                }
                objSalesMan.UserID = PageBase.UserId;
                objSalesMan.PageIndex = pageno;
                objSalesMan.PageSize = Convert.ToInt32(PageSize);

                dtSalesMan = objSalesMan.GeOutStandingAmountDetails();

                if (objSalesMan.TotalRecords > 0)
                {
                    if (pageno > 0)
                    {
                        ucPagingControl1.Visible = true;
                        btnExprtToExcel.Visible = true;
                        gvSalesMan.DataSource = dtSalesMan;
                        gvSalesMan.DataBind();
                        ViewState["TotalRecords"] = objSalesMan.TotalRecords;
                        ucPagingControl1.TotalRecords = objSalesMan.TotalRecords;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = pageno;
                        ucPagingControl1.FillPageInfo();
                    }
                    else
                    {
                        ucPagingControl1.Visible = true;
                        btnExprtToExcel.Visible = true;
                        String FilePath = Server.MapPath("../../");
                        string FilenameToexport = "OutStandingAmountList";
                        PageBase.RootFilePath = FilePath;
                        PageBase.ExportToExecl(dtSalesMan, FilenameToexport);

                    }

                }
                else
                {
                    dtSalesMan = null;
                    gvSalesMan.DataSource = null;
                    gvSalesMan.DataBind();
                    btnExprtToExcel.Visible = false;
                    ucPagingControl1.Visible = false;
                }

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }

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
                    obj.UserID = PageBase.UserId; /*#CC02 Added */ 
                    obj.SessionID = Convert.ToString(dt.Rows[0]["SessionID"]);
                    DataSet dsResult = obj.BulkUploadOutStandingAmount(); /*#CC02 Added */
                    /*obj.BulkUploadExcel("prcBulkUploadOutStandingAmount"); #CC02 Commented  */
                    if (obj.intOutParam == 0)
                    {
                        ucMsg.ShowSuccess(Resources.Messages.DataUploadSuccess);
                        GetSearchData(Convert.ToInt32(ViewState["CurrentPage"]));

                    }
                    else if (obj.intOutParam == 1)
                    {
                        ucMsg.ShowInfo(obj.Error);
                        /*#CC02 Add Start */
                        hlnkInvalid.Visible = true;
                        strUploadedFileName = "InvalidData" + DateTime.Now.Ticks;
                        PageBase.ExportInExcel(dsResult, strUploadedFileName);
                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strUploadedFileName + ".xlsx";
                        hlnkInvalid.Text = "Invalid Data";
                        /*#CC02 Add End */

                    }
                    else if (obj.intOutParam == 2)
                    {
                        ucMsg.ShowError(obj.Error);
                        /* #CC02 Comment Start 
                          ucMsg.ShowInfo(Resources.Messages.PartialDataUpload);
                        hlnkInvalid.Visible = true;
                        strUploadedFileName = "InvalidData" + DateTime.Now.Ticks;
                        PageBase.ExportInExcel(dsResult, strUploadedFileName);
                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strUploadedFileName + ".xlsx";
                        hlnkInvalid.Text = "Invalid Data";
                      #CC02 Comment End  */
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
                bulkCopy.DestinationTableName = "BulkUploadOutStandingAmount";
                bulkCopy.ColumnMappings.Add("Narration", "Narration");
                bulkCopy.ColumnMappings.Add("Amount", "Amount");
                bulkCopy.ColumnMappings.Add("OutstandingDate", "OutstandingDate");
                bulkCopy.ColumnMappings.Add("Remarks", "Remarks");
                bulkCopy.ColumnMappings.Add("SalesChannelCode", "SalesChannelCode");
                bulkCopy.ColumnMappings.Add("Active", "Active");
                bulkCopy.ColumnMappings.Add("CreatedBy", "CreatedBy");
                bulkCopy.ColumnMappings.Add("SessionID", "SessionID");
                bulkCopy.ColumnMappings.Add("SalesChannelType", "SalesChannelType"); /*#CC02 Added */
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


    #endregion


}
