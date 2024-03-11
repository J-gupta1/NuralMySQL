/*
 * ==============================================================================================================================
 * Copyright	: Zed-Axis Technologies, 2016
 * Created By	: Sumit Maurya
 * Create date	: 12-Aug-2016
 * Description	: This interface is used to Update carton number for exiting imei/serial numbers. 
 * Module       : Update Carton  Number for existing data
 * ==============================================================================================================================
 * Change Log:
 * DD-MMM-YYYY, Name , #CCXX - Description
 * ------------------------------------------------------------------------------------------------------------------------------
 * 30-Aug-2016, Sumit Maurya, #CC01, Datatype changed to upload alphanumeric serial numbers.
 * 06-Sep-2016, Sumit Maurya, #CC02, Previous implementation changed, Now Carton number and serial number will be fetched from same multiline textbox.
 * ==============================================================================================================================
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;
using System.IO;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
public partial class Transactions_BulkUploadGRNPrimaryPrimary_UpdateCartonNumber : PageBase
{
    #region Upload Variables

    #endregion Upload Variables
    string strMaxSerialCount = Resources.AppConfig.MaxSerialCountforUpdateCarton.ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMessege.ShowControl = false;

        if (!IsPostBack)
        {
            SetMaxCount();
        }

    }

    public void CreateErrorDataLink(DataSet dsError, string strInvalid)
    {
        try
        {
            hlnkInvalid.Visible = true;
            string strFileName = strInvalid + DateTime.Now.Ticks;
            ExportInExcel(dsError, strFileName);
            hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
            hlnkInvalid.Text = strInvalid;
        }
        catch (Exception ex)
        {
            ucMessege.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    /* #CC02 Comment Start */
    #region  #CC02Comment
    //protected void btnSave_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        hlnkInvalid.Visible = false;
    //        DataSet dsResult = new DataSet();

    //        if (txtSerialNumber.Text.Trim() == "")
    //        {
    //            ucMessege.ShowInfo("Please enter serial number.");
    //            return;
    //        }
    //        DataTable DtSerials = new DataTable();
    //        DtSerials = tempDtSerial();

    //        DataTable DtInvalidSerials = new DataTable();
    //        DtInvalidSerials = tempDtSerial();
    //        DtInvalidSerials.Columns.Add("Error", typeof(System.String));


    //        DataTable DtDuplicateSerials = new DataTable();
    //        DtDuplicateSerials = tempDtSerial();
    //        DtDuplicateSerials.Columns.Add("Error", typeof(System.String));

    //        string[] strSplitSerials = txtSerialNumber.Text.Trim().Split(',');

    //        /* Checking duplicate data Start */
    //        for (int k = 0; k < strSplitSerials.Length; k++)
    //        {
    //            if (strSplitSerials[k].Trim() != "")
    //            {
    //                for (int j = k + 1; j < strSplitSerials.Length; j++)
    //                {
    //                    if (strSplitSerials[k].ToString() == strSplitSerials[j].ToString())
    //                    {
    //                        DataRow drDuplicateSerial = DtDuplicateSerials.NewRow();
    //                        drDuplicateSerial["IMEI"] = strSplitSerials[k].Trim();
    //                        drDuplicateSerial["Error"] = "Duplicate data found";
    //                        DtDuplicateSerials.Rows.Add(drDuplicateSerial);
    //                        DtDuplicateSerials.AcceptChanges();
    //                    }
    //                }
    //            }
    //        }

    //        if (DtDuplicateSerials.Rows.Count > 0)
    //        {

    //            ucMessege.ShowInfo("There is error in data. Please refer Invalid Data link. No Data processed.");
    //            if (DtDuplicateSerials.Columns.Contains("IMEI"))
    //                DtDuplicateSerials.Columns["IMEI"].ColumnName = "Error Data";
    //            DataTable DTDuplicateCols = DtDuplicateSerials.DefaultView.ToTable(true, "Error Data", "Error");
    //            DataSet dsError = new DataSet();
    //            if (DtDuplicateSerials.Rows.Count < 51)
    //            {
    //                dsError.Tables.Add(DTDuplicateCols);
    //                ucMessege.XmlErrorSource = dsError.GetXml();
    //            }
    //            else
    //            {
    //                dsResult.Tables.Add(DTDuplicateCols);
    //                CreateErrorDataLink(dsResult, "Duplicate Data");
    //            }

    //            return;
    //        }

    //        /* Checking duplicate data End */
    //        for (int i = 0; i < strSplitSerials.Length; i++)
    //        {

    //            if (strSplitSerials[i].Trim() != "")
    //            {
    //                DataRow dr = DtSerials.NewRow();
    //                DataRow drInvalidSerial = DtInvalidSerials.NewRow();

    //                if (strSplitSerials[i].Trim().Length < PageBase.SerialNoLength_Min)
    //                {
    //                    // DataRow dr = DtInvalidSerials.NewRow();

    //                    drInvalidSerial["IMEI"] = strSplitSerials[i].Trim();
    //                    drInvalidSerial["Error"] = "Serial Number length cannot be less than " + PageBase.SerialNoLength_Min;
    //                    DtInvalidSerials.Rows.Add(drInvalidSerial);
    //                    DtInvalidSerials.AcceptChanges();
    //                }
    //                else if (strSplitSerials[i].Trim().Length > PageBase.SerialNoLength_Max)
    //                {
    //                    // DataRow dr = DtInvalidSerials.NewRow();

    //                    drInvalidSerial["IMEI"] = strSplitSerials[i].Trim();
    //                    drInvalidSerial["Error"] = "Serial Number length cannot be greater than " + PageBase.SerialNoLength_Max;
    //                    DtInvalidSerials.Rows.Add(drInvalidSerial);
    //                    DtInvalidSerials.AcceptChanges();
    //                }
    //                else if (!Regex.IsMatch(strSplitSerials[i].Trim(), @"^[a-zA-Z0-9]+$"))
    //                {
    //                    drInvalidSerial["IMEI"] = strSplitSerials[i].Trim();
    //                    drInvalidSerial["Error"] = "Special characters not allowed.";
    //                    DtInvalidSerials.Rows.Add(drInvalidSerial);
    //                    DtInvalidSerials.AcceptChanges();
    //                }
    //                else
    //                {
    //                    //DataRow dr = DtSerials.NewRow();
    //                    dr["IMEI"] = strSplitSerials[i].Trim();
    //                    DtSerials.Rows.Add(dr);
    //                    DtSerials.AcceptChanges();
    //                }
    //            }

    //        }
    //        if (DtInvalidSerials.Rows.Count > 0)
    //        {
    //            DataSet dsError = new DataSet();
    //            if (DtInvalidSerials.Rows.Count < 51)
    //            {
    //                dsError.Tables.Add(DtInvalidSerials);
    //                ucMessege.XmlErrorSource = dsError.GetXml();
    //            }
    //            else if (DtInvalidSerials.Rows.Count > 50)
    //            {
    //                ucMessege.ShowInfo("There is error in data. Please refer Invalid Data link. No Data processed.");
    //                if (DtInvalidSerials.Columns.Contains("IMEI"))
    //                    DtInvalidSerials.Columns["IMEI"].ColumnName = "Error Data";
    //                dsResult.Tables.Add(DtInvalidSerials);
    //                CreateErrorDataLink(dsResult, "Invalid Data");

    //            }

    //            return;

    //        }
    //        else
    //        {
    //            string guid = Guid.NewGuid().ToString();
    //            DtSerials.Columns.Add(AddColumn(guid, "SessionID", typeof(System.String)));
    //            DtSerials.Columns.Add(AddColumn(Convert.ToString(PageBase.UserId), "CreatedBy", typeof(int)));
    //            // DtSerials.Columns.Add(AddColumn(txtCartonNumber.Text.Trim(), "CartonNumber", typeof(System.String)));
    //            if (DtSerials.Columns.Contains("IMEI"))
    //                DtSerials.Columns["IMEI"].ColumnName = "SerialNumber";

    //            if (UploadCartonNumberBcp(DtSerials) == true)
    //            {
    //                using (WarehouseTranaction obj = new WarehouseTranaction())
    //                {
    //                    /*obj.strCartonNumber = txtCartonNumber.Text.Trim();
    //                    obj.dtIMEIs = DtSerials;*/
    //                    obj.SessionID = Convert.ToString(DtSerials.Rows[0]["SessionID"]);
    //                    dsResult = obj.UpdateCartonNumber();
    //                    if (obj.intOutParam == 0)
    //                    {
    //                        ucMessege.ShowSuccess("Data updated successfully.");
    //                        clearfields();
    //                    }
    //                    if (dsResult != null)
    //                    {
    //                        if (dsResult.Tables.Count > 0)
    //                        {
    //                            ucMessege.ShowInfo("Valid data uploaded successfully. Some invalid data found, for invalid data please check error error result.");
    //                            if (dsResult.Tables[0].Rows.Count < 51)
    //                            {
    //                                ucMessege.XmlErrorSource = dsResult.GetXml();
    //                            }
    //                            else
    //                            {
    //                                ucMessege.ShowInfo("There is error in data. Please refer Invalid Data link. No Data processed.");
    //                                CreateErrorDataLink(dsResult, "Invalid Data");
    //                                #region not required
    //                                /* hlnkInvalid.Visible = true;
    //                                string strFileName = "InvalidData" + DateTime.Now.Ticks;
    //                                ExportInExcel(dsResult, strFileName);
    //                                hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
    //                                hlnkInvalid.Text = "Invalid Data";*/
    //                                #endregion not required
    //                            }
    //                            // clearfields();
    //                        }
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                ucMessege.ShowError("Error in uploading data.");
    //            }
    //        }



    //    }
    //    catch (Exception ex)
    //    {
    //        ucMessege.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
    //        PageBase.Errorhandling(ex);
    //    }
    //}
    #endregion  #CC02Comment
    /* #CC02 Comment End */


    /* #CC02 Add Start */
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {

            hlnkInvalid.Visible = false;
            DataSet dsResult = new DataSet();

            if (txtCartonAndSerialNumber.Text.Trim() == "")
            {
                ucMessege.ShowInfo("Please enter Carton and Serial number.");
                return;
            }
            DataTable DtCartonWithSerials = new DataTable();
            DtCartonWithSerials = tempDtCartonSerial();


            DataTable DtInvalidCartonWithSerials = new DataTable();
            DtInvalidCartonWithSerials = tempDtCartonSerial();
            DtInvalidCartonWithSerials.Columns.Add("Error", typeof(string));

            string strReplaceSpace = string.Empty;
            strReplaceSpace = txtCartonAndSerialNumber.Text.Trim().Replace("\r\n", ";").Replace("\n", ";").Replace(";;", ";");
            string[] splitQuantity = strReplaceSpace.Split(';');
            for (int i = 0; i < splitQuantity.Length; i++)
            {
                Int16 IsError = 0;
                if (splitQuantity[i].Contains(","))
                {
                    string[] splitCartonAndSerial = splitQuantity[i].Split(',');
                    if (splitCartonAndSerial[0] != "" && splitCartonAndSerial[1] != "")
                    {


                        if (splitCartonAndSerial[0].ToString().Trim().Length > 50)
                        {
                            DataRow drInvalidData = DtInvalidCartonWithSerials.NewRow();
                            drInvalidData["CartonNumber"] = splitCartonAndSerial[0].ToString().Trim();
                            drInvalidData["SerialNumber"] = splitCartonAndSerial[1].ToString().Trim();
                            drInvalidData["Error"] = "Carton number length cannot be greater than 50.";
                            DtInvalidCartonWithSerials.Rows.Add(drInvalidData);
                            DtInvalidCartonWithSerials.AcceptChanges();
                            IsError = 1;
                        }
                        if (splitCartonAndSerial[1].ToString().Trim().Length < PageBase.SerialNoLength_Min)
                        {
                            DataRow drInvalidData = DtInvalidCartonWithSerials.NewRow();
                            drInvalidData["CartonNumber"] = splitCartonAndSerial[0].ToString().Trim();
                            drInvalidData["SerialNumber"] = splitCartonAndSerial[1].ToString().Trim();
                            drInvalidData["Error"] = drInvalidData["Error"].ToString() + "Serial Number length cannot be less than " + PageBase.SerialNoLength_Min + " .";
                            DtInvalidCartonWithSerials.Rows.Add(drInvalidData);
                            DtInvalidCartonWithSerials.AcceptChanges();
                            IsError = 1;
                        }
                        if (splitCartonAndSerial[1].ToString().Trim().Length > PageBase.SerialNoLength_Max)
                        {
                            DataRow drInvalidData = DtInvalidCartonWithSerials.NewRow();
                            drInvalidData["CartonNumber"] = splitCartonAndSerial[0].ToString().Trim();
                            drInvalidData["SerialNumber"] = splitCartonAndSerial[1].ToString().Trim();
                            drInvalidData["Error"] = drInvalidData["Error"].ToString() + "Serial Number length cannot be greater than " + PageBase.SerialNoLength_Max + " .";
                            DtInvalidCartonWithSerials.Rows.Add(drInvalidData);
                            DtInvalidCartonWithSerials.AcceptChanges();
                            IsError = 1;
                        }
                        if (IsError == 0) /* Row without error*/
                        {
                            DataRow dr = DtCartonWithSerials.NewRow();
                            dr["CartonNumber"] = splitCartonAndSerial[0].ToString().Trim();
                            dr["SerialNumber"] = splitCartonAndSerial[1].ToString().Trim();
                            DtCartonWithSerials.Rows.Add(dr);
                            DtCartonWithSerials.AcceptChanges();
                        }


                    }
                    else if (splitCartonAndSerial[0].ToString() == "" || splitCartonAndSerial[1].ToString() == "")
                    {
                        DataRow drInvalidData = DtInvalidCartonWithSerials.NewRow();
                        drInvalidData["CartonNumber"] = splitCartonAndSerial[0].ToString().Trim();
                        drInvalidData["SerialNumber"] = splitCartonAndSerial[1].ToString().Trim();
                        drInvalidData["Error"] = (splitCartonAndSerial[0].ToString() == "") ? "Carton number cannot be blank" : "SerialNumber/IMEI cannot be blank";
                        DtInvalidCartonWithSerials.Rows.Add(drInvalidData);
                        DtInvalidCartonWithSerials.AcceptChanges();
                    }

                }
                else
                {
                    DataRow drInvalidData = DtInvalidCartonWithSerials.NewRow();
                    drInvalidData["CartonNumber"] = splitQuantity[i].ToString().Trim();
                    drInvalidData["Error"] = "Invalid data";
                    DtInvalidCartonWithSerials.Rows.Add(drInvalidData);
                    DtInvalidCartonWithSerials.AcceptChanges();
                }
            }

            if (DtInvalidCartonWithSerials.Rows.Count > 0)
            {
                DataSet dsError = new DataSet();
                if (DtInvalidCartonWithSerials.Rows.Count < 51)
                {
                    dsError.Tables.Add(DtInvalidCartonWithSerials);
                    ucMessege.XmlErrorSource = dsError.GetXml();
                }
                else if (DtInvalidCartonWithSerials.Rows.Count > 50)
                {
                    ucMessege.ShowInfo("There is error in data. Please refer Invalid Data link. No Data processed.");
                    dsResult.Tables.Add(DtInvalidCartonWithSerials);
                    CreateErrorDataLink(dsResult, "Invalid Data");
                }
            }
            else
            {
                if (DtCartonWithSerials.Rows.Count > Convert.ToInt32(Resources.AppConfig.MaxSerialCountforUpdateCarton))
                {
                    ucMessege.ShowInfo("Maximum " + Resources.AppConfig.MaxSerialCountforUpdateCarton.ToString() + " serial numbers are allowed");
                    return;
                }
                else
                {
                    string guid = Guid.NewGuid().ToString();
                    DtCartonWithSerials.Columns.Add(AddColumn(guid, "SessionID", typeof(System.String)));
                    DtCartonWithSerials.Columns.Add(AddColumn(Convert.ToString(PageBase.UserId), "CreatedBy", typeof(int)));
                    if (UploadCartonNumberBcp(DtCartonWithSerials) == true)
                    {
                        using (WarehouseTranaction obj = new WarehouseTranaction())
                        {
                            obj.SessionID = Convert.ToString(DtCartonWithSerials.Rows[0]["SessionID"]);
                            dsResult = obj.UpdateCartonNumber();
                            if (obj.intOutParam == 0)
                            {
                                ucMessege.ShowSuccess("Data updated successfully.");
                                clearfields();
                            }
                            if (dsResult != null)
                            {
                                if (dsResult.Tables.Count > 0)
                                {
                                    ucMessege.ShowInfo("Valid data uploaded successfully. Some invalid data found, for invalid data please check error error result.");
                                    if (dsResult.Tables[0].Rows.Count < 51)
                                    {
                                        ucMessege.XmlErrorSource = dsResult.GetXml();
                                    }
                                    else
                                    {
                                        ucMessege.ShowInfo("There is error in data. Please refer Invalid Data link.");
                                        CreateErrorDataLink(dsResult, "Invalid Data");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ucMessege.ShowError("Error in uploading data.");
                    }
                }
            }



        }
        catch (Exception ex)
        {
            ucMessege.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    /* #CC02 Add End */
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("UpdateCartonNumber.aspx", false);
    }

    public void SetMaxCount()
    {
        try
        {
            hdnSerialCountLimit.Value = strMaxSerialCount;
            lblNote.Text = "Maximum " + hdnSerialCountLimit.Value + " serial numbers are allowed.";
        }
        catch (Exception ex)
        {
            ucMessege.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }

    public DataTable tempDtSerial()
    {
        DataTable dtSerial = new DataTable();
        /* dtbrandid.Columns.Add("IMEI", typeof(System.Int64)); #CC01 Commented */
        dtSerial.Columns.Add("IMEI", typeof(string)); /* #CC01 Added */
        return dtSerial;
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
            ucMessege.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }

    public void clearfields()
    {
        /* #CC02 Comment Statr txtCartonNumber.Text = string.Empty;
         txtSerialNumber.Text = string.Empty; #CC02 Comment End*/
        txtCartonAndSerialNumber.Text = string.Empty; /* #CC02 Added */
    }



    public bool UploadCartonNumberBcp(DataTable dtUpload)
    {
        try
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "BulkUploadCartonNum";
                bulkCopy.ColumnMappings.Add("CartonNumber", "CartonNumber");
                bulkCopy.ColumnMappings.Add("SerialNumber", "SerialNumber");
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

    /* #CC02 Add End*/
    public DataTable tempDtCartonSerial()
    {
        DataTable dtCartonAndSerial = new DataTable();
        dtCartonAndSerial.Columns.Add("CartonNumber", typeof(string));
        dtCartonAndSerial.Columns.Add("SerialNumber", typeof(string));
        return dtCartonAndSerial;
    }
    /* #CC02 Add End*/
}
