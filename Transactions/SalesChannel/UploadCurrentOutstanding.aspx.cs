#region Copyright© 2016 Zed-Axis Technologies All rights are reserved
/*
 * ================================================================================================
 * COPYRIGHT (c) 2012 Zed Axis Technologies (P) Ltd.
 * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART,
 * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE,
 * WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
 * ================================================================================================
 * Created By :  Sumit Maurya
 * Role : Software Developer
 * Module : Bulk Upload Current outstanding amount for RDS
 * Description : User upload file with respect to NDS and Warehouse
 * Reviewed By :
 * ================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 28-Jul-2016, Sumit Maurya, #CC01, Referance code link added to download SaleChannel detail of SAlechannel type 7 (RDS/Distributor).
 * ================================================================================================

*/

#endregion
#region Using
using System.Data;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Resources;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;
using System.Text;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using ZedService;
#endregion

public partial class Transactions_Transactions_SalesChannel_UploadCurrentOutstanding : PageBase
{
    protected string strSiteUrl = PageBase.siteURL;
    protected string strAssets = PageBase.strAssets;
    private bool IsOpeningdateEnable = false;


    #region Upload Variables
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    UploadFile UploadFile = new UploadFile();
    OpenXMLExcel objexcel = new OpenXMLExcel();
    string strPSalt, strPassword;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Page.Header.DataBind();
            ucMsg.Visible = false;
            if (!IsPostBack)
            {

            }

        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            ucMsg.ShowError(ex.Message, GlobalErrorDisplay());
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {

    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            byte isSuccess = 1;
            String RootPath = Server.MapPath("../../");
            UploadFile.RootFolerPath = RootPath;
            Int16 UploadCheck = 0;
            UploadCheck = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            DataSet objDS = new DataSet();

            ViewState["TobeUploaded"] = strUploadedFileName;
            if (UploadCheck == 1)
            {
                UploadFile.UploadedFileName = strUploadedFileName;
                UploadFile.UploadValidationType = EnumData.eUploadExcelValidationType.eRetailerUpload;
                isSuccess = UploadFile.uploadValidExcelRetailer(ref objDS, "CurrentOutstandingAmount");

                int intError = 0;
                //if (objDS.Tables[0].Columns.Contains("Error"))
                //{
                //    intError = 1;
                //}
                if (!objDS.Tables[0].Columns.Contains("Error"))
                {
                    objDS.Tables[0].Columns.Add("Error", typeof(string));
                }
                switch (isSuccess)
                {
                    case 0:
                        ucMsg.ShowInfo(UploadFile.Message);
                        dvhide.Visible = true;
                        break;
                    case 2:
                        ucMsg.ShowInfo(Resources.Messages.CheckErrorGrid);
                        pnlGrid.Visible = true;
                        GridCurrentOutstanding.DataSource = objDS;
                        GridCurrentOutstanding.DataBind();
                        dvhide.Visible = true;
                        break;
                    case 1:
                        InsertData(objDS);
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
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            ucMsg.ShowError(ex.Message, GlobalErrorDisplay());
        }
    }

    private void InsertData(DataSet objds)
    {
        if (objds != null && objds.Tables.Count > 0 && objds.Tables[0].Rows.Count > 0)
        {
            {
                if (objds.Tables[0].Columns.Contains("Error"))
                {
                    objds.Tables[0].Columns.Remove("Error");
                }

                pnlGrid.Visible = true;
                Btnsave.Visible = true;
                if ((CreatedBcpData(objds.Tables[0])) == true)
                {

                    SalesChannelData objSales = new SalesChannelData();
                    objSales.SessionID = Convert.ToString(objds.Tables[0].Rows[0]["SessionID"]);
                    int result = objSales.SaveSaleChannelOutStandingAmount();
                    if (result == 0)
                    {
                        ucMsg.ShowSuccess("Data uploaded successfully.");
                        GridCurrentOutstanding.DataSource = null;
                        GridCurrentOutstanding.DataBind();
                        updGrid.Update();
                        dvhide.Visible = false;
                    }
                    else if (result == 1 && objSales.XMLList != null)
                    {
                        ucMsg.XmlErrorSource = objSales.XMLList;
                    }
                    else
                    {
                        ucMsg.ShowError(objSales.Error);
                    }



                }

                /*
                GridCurrentOutstanding.DataSource = objds.Tables[0];

                GridCurrentOutstanding.DataBind();
                updGrid.Update();
                dvhide.Visible = true;*/
            }

        }

    }

    public bool CreatedBcpData(DataTable dtUpload)
    {
        bool result = false;
        try
        {
            string guid = Guid.NewGuid().ToString();
            dtUpload.Columns.Add(AddColumn(guid, "SessionID", typeof(System.String)));
            dtUpload.Columns.Add(AddColumn(Convert.ToString(PageBase.UserId), "CreatedBy", typeof(int)));
            dtUpload.AcceptChanges();

            int i = PageBase.UserId;

            if (UploadCurrentOutStandingBcp(dtUpload) == true)
            {
                ucMsg.ShowSuccess("BCP done sucessfully");
                result = true;
            }
            else
            {
                ucMsg.ShowError("Error while doing BCP");
            }


            /*
            DataTable dtCreateBCP = new DataTable();
            dtCreateBCP = dtTempBCP();


            string[] Columns = { "RDS" };
            DataTable objDtUniqueColumn = dtUpload.DefaultView.ToTable(true, Columns);

            for (int i = 0; i < objDtUniqueColumn.Rows.Count; i++)
            {
                DataRow dr = dtCreateBCP.NewRow();
                DataRow[] drFilterRows;
                drFilterRows = dtUpload.Select("[RDS]='" + objDtUniqueColumn.Rows[i]["RDS"] + "'");
                for(int j=0;j<drFilterRows.Length;j++)
                {

                }
            }
            */

            return result;
        }
        catch (Exception ex)
        {
            return result;
        }

    }

    public bool UploadCurrentOutStandingBcp(DataTable dtUpload)
    {
        try
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "SaleChannelOutstandingBulk";
                bulkCopy.ColumnMappings.Add("RDS", "RDS");
                bulkCopy.ColumnMappings.Add("InvoiceNo", "InvoiceNo");
                bulkCopy.ColumnMappings.Add("Type", "Type");
                bulkCopy.ColumnMappings.Add("Date", "Date");
                bulkCopy.ColumnMappings.Add("PendingAmt", "PendingAmt");
                bulkCopy.ColumnMappings.Add("DueDate", "DueDate");
                bulkCopy.ColumnMappings.Add("Days", "Days");
                bulkCopy.ColumnMappings.Add("SessionID", "SessionID");
                bulkCopy.ColumnMappings.Add("CreatedBy", "CreatedBy");
                bulkCopy.WriteToServer(dtUpload);
                return true;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    /* #CC01 Add Start */
    protected void hlnkDownLoadRefCode_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsReferenceCode = new DataSet();
            DataTable DtSalesChannelDetail = new DataTable();
            using (SalesChannelData objSalesData = new SalesChannelData())
            {
                objSalesData.SalesChannelTypeID = 7;
                objSalesData.StatusValue = 1;

                DtSalesChannelDetail = objSalesData.GetSalesChannelInfo();

                string[] DsCol = new string[] { "SalesChannelName", "SalesChannelCode" };
                // DataTable DtCopy = new DataTable();
                DataTable dt = DtSalesChannelDetail.Copy();

                dt = dt.DefaultView.ToTable(true, DsCol);
                dsReferenceCode.Merge(dt);
                dsReferenceCode.Tables[0].AcceptChanges();
                if (dsReferenceCode != null && dsReferenceCode.Tables.Count > 0)
                {
                    string FilenameToexport = "Reference Code List";
                    PageBase.ExportToExecl(dsReferenceCode, FilenameToexport, EnumData.eTemplateCount.ePrimarysales1);
                }
                else
                {
                    ucMsg.ShowInfo(Resources.Messages.NoRecord);
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    /* #CC01 Add End */
}
