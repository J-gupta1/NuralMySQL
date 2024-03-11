﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using DataAccess;
using System.Web.UI;
using DocumentFormat;
using BussinessLogic;
using System.Data.OleDb;
using Microsoft.SqlServer;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Web.SessionState;
using System.Linq;
using ZedService;
//using System.Data.Linq;
/*
 * 17-Dec-2014, Sumit Kumar, #CC01, Check table schema table
 * 31-May-2016, Sumit Maurya, #CC02, New code added to return error message if value of mandatory fields are blank.
 * 28-July-2016, Karam Chand Sharma, #CC03, datetime check bypass due to only check date fix it
 * 29-July-2016, Karam Chand Sharma, #CC04, Validate If serial no is blank
 * 19-Sep-2016, Sumit Maurya, #CC05, New check added to validate numbers of columns in Excel data.
 * 10-Nov-2017, Balram Jha, #CC06 - for Primary sale upload File data validation will be done in Procedure and all Error should display in upload.
 * 31-Jan-2020,Vijay Kumar Prajapati,#CC08-Added New Method for file validate done for inone.
 * 15-April-2020,Vijay Kumar Prajapati,#CC09--Added CompanyId 
 */

namespace BusinessLogics
{
    public class ValidateUploadFile
    {
        #region Variables and objects
        string strMSg;
        bool blnIsValid;
        private DataTable dtUpload;
        List<string> lstPrimary;

        List<clsErrorData> lstErrorData = new List<clsErrorData>();
        string strKey = string.Empty;
        private string strRootFolerPath;
        private string strUploadedFileName = string.Empty;
        private string strExcelFileNameInTable = string.Empty;
        private string strPkColumnName = string.Empty;

        SortedList objSLInvalidRecords = new SortedList();
        ArrayList objArrBlankList = new ArrayList();
        DataTable objDtExcelSheet = new DataTable();
        DataTable objDtBlankData = new DataTable();
        DataTable objDtDuplicateRecord = new DataTable();

        #endregion

        #region Properties

        public string RootFolerPath
        {
            get { return strRootFolerPath; }
            set { strRootFolerPath = value; }
        }
        public string UploadedFileName
        {
            get { return strUploadedFileName; }
            set { strUploadedFileName = value; }
        }
        public string ExcelFileNameInTable
        {
            get { return strExcelFileNameInTable; }
            set
            {
                strExcelFileNameInTable = value;
            }
        }
        public string Message
        {
            get { return strMSg; }
            set
            {
                strMSg = value;
            }
        }
        public string PkColumnName
        {
            get { return strPkColumnName; }
            set
            {
                strPkColumnName = value;
            }
        }
        public Int32 CompanyId { set; get; }/*#CC09 Added*/
        #endregion

        #region Error Messages

        string strErrMsgMandatory = "Null/Blank values not allowed with mandatory field ";
        string strErrMsgInt16 = "Please define numeric values less than 32768 for this field ";
        string strErrMsgInt32 = "Please define numeric values less than 2147483648 for this field ";
        string strErrMsgInt64 = "Please define numeric values less than 9223372036854775808 for this field ";
        string strErrMsgDouble = "Please define numeric values for this field ";
        string strErrMsgDecimal = "Please define numeric values for this field ";
        string strErrMsgAlphaNumeric = "Please define AlphaNumeric value for this field ";
        string strErrMsgBool = "Please define boolean (0|1) values for this field ";
        string strErrMsgEmail = "Please define valid Email for this field ";
        string strErrMsgDate = "Please define valid Date for this field ";
        string strErrMsgPincode = "PIN Code should be 6 digits numeric.";
        string strErrMsgPanNo = "PAN No should be 10 Characters.";
        string strMinLength = "Please define Min ";
        string strMaxLength = "Please define Max ";
        string strMSgComplete = " characters for this field ";
        string strErrMsgPositive = "Only positive value allowed";
        #endregion

        /// <summary>
        /// Description: Checking that the provided Excel file fields match with Actual Excel Format and that file have records
        /// </summary>
        /// <param name="strExcelField"></param>
        /// <param name="strArCols"></param>
        /// <returns></returns>
        public void ValidateFile(bool blnChkDuplicate, out DataSet objDs, out SortedList objSL)
        {
            DataSet ds;
            Boolean blnValidaData;
            objDs = new DataSet();
            objSL = new SortedList();
            try
            {
                OpenXMLExcel objexcel = new OpenXMLExcel();
                using (CommonData objSchema = new CommonData())
                {
                    objSchema.CompanyId = CompanyId;
                    DataSet objdsSchema = objSchema.GetUploadFileSchema(ExcelFileNameInTable);
                    string strColName = string.Empty;
                    if (objdsSchema != null && objdsSchema.Tables.Count > 0)
                    {
                        string strColumnNames = string.Empty;
                        //ds = objexcel.ImportExcelFile(strRootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
                        ds = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);
                        if (objdsSchema.Tables[0].Rows.Count > 0)
                        {
                            #region Get Excel data from sheet1
                            //GetExcelData();
                            objDtExcelSheet = ds.Tables[0].Copy();
                            //DataTable objdtTmp = objDtExcelSheet.Clone();
                            ArrayList objArrBlankCol = new ArrayList();
                            foreach (DataColumn objDC in objDtExcelSheet.Columns)
                            {
                                if (objDC.ColumnName.Trim() == "F1" || objDC.ColumnName.Trim() == "F2" || objDC.ColumnName.Trim() == "F3" ||
                                    objDC.ColumnName.Trim() == "F4" || objDC.ColumnName.Trim() == "F5" || objDC.ColumnName.Trim() == "F6" ||
                                    objDC.ColumnName.Trim() == "F7" || objDC.ColumnName.Trim() == "F8" || objDC.ColumnName.Trim() == "F9" ||
                                    objDC.ColumnName.Trim() == "F10" || objDC.ColumnName.Trim() == "F11" || objDC.ColumnName.Trim() == "F12" ||
                                    objDC.ColumnName.Trim() == "F13" || objDC.ColumnName.Trim() == "F14" || objDC.ColumnName.Trim() == "F15" ||
                                    objDC.ColumnName.Trim() == "F16" || objDC.ColumnName.Trim() == "F17" || objDC.ColumnName.Trim() == "F18" ||
                                    objDC.ColumnName.Trim() == "F19" || objDC.ColumnName.Trim() == "F20" || objDC.ColumnName.Trim() == "F21" ||
                                    objDC.ColumnName.Trim() == "F22" || objDC.ColumnName.Trim() == "F23" || objDC.ColumnName.Trim() == "F24" ||
                                    objDC.ColumnName.Trim() == "F25" || objDC.ColumnName.Trim() == "F26" || objDC.ColumnName.Trim() == "F27" ||
                                    objDC.ColumnName.Trim() == "F28" || objDC.ColumnName.Trim() == "F29" || objDC.ColumnName.Trim() == "F30"
                                    )
                                {
                                    objArrBlankCol.Add(objDC);
                                }
                            }
                            foreach (DataColumn objDC1 in objArrBlankCol)
                                objDtExcelSheet.Columns.Remove(objDC1);
                            objDtExcelSheet.AcceptChanges();
                            #endregion

                            #region Validate schema is match or not
                            for (int sc = 0; sc < objdsSchema.Tables[0].Rows.Count; sc++)
                            {
                                if (objdsSchema.Tables[0].Rows.Count == objDtExcelSheet.Columns.Count)
                                {
                                    if (Convert.ToString(objdsSchema.Tables[0].Rows[sc]["ColumnName"]).ToUpper() != objDtExcelSheet.Columns[sc].ColumnName.ToUpper())
                                    {
                                        Message = "Invalid file format";
                                        return;
                                    }
                                }
                                else
                                {
                                    Message = "Number of columns not match";
                                    return;
                                }
                            }
                            #endregion

                            #region CreateDataViews

                            strColName = string.Empty;
                            DataView objDV = objdsSchema.Tables[0].DefaultView;

                            #endregion

                            #region Delete the rows which not having Base Column
                            objDtBlankData = objDtExcelSheet.Clone();
                            objDtExcelSheet.AcceptChanges();
                            objDtBlankData.AcceptChanges();
                            objDV.RowFilter = " [BaseColumn]=1 ";
                            if (objDtExcelSheet.Rows.Count > 0)
                            {
                                ArrayList objArrBlankList = new ArrayList();
                                if (objDV.Count > 0)
                                {
                                    foreach (DataRow objDR in objDtExcelSheet.Rows)
                                    {
                                        foreach (DataRowView drv in objDV)
                                        {
                                            //strColName = objDV.Table.Columns[intTmpArrCount].ColumnName.Trim();
                                            strColName = Convert.ToString(drv["ColumnName"]);

                                            if (objDR[strColName] == DBNull.Value || objDR[strColName] == "")
                                            {
                                                bool blnIsData = false;
                                                objArrBlankList.Add(objDR);

                                                /*#CC23082014 --looks useless */
                                                for (int sc = 0; sc < objdsSchema.Tables[0].Rows.Count; sc++)
                                                {
                                                    if (objDR[Convert.ToString(objdsSchema.Tables[0].Rows[sc]["ColumnName"])].ToString().Trim().Length > 0)
                                                    {
                                                        blnIsData = true;
                                                        break;
                                                    }
                                                }
                                                if (blnIsData)
                                                {
                                                    objDtBlankData.ImportRow(objDR);
                                                }
                                                break;

                                            }
                                        }
                                    }
                                    foreach (DataRow objDrTmp in objArrBlankList)
                                        objDtExcelSheet.Rows.Remove(objDrTmp);
                                    objDtExcelSheet.AcceptChanges();
                                }
                            }
                            #endregion

                            #region Check file is empty or not
                            if (objDtExcelSheet.Rows.Count == 0)
                            {
                                //Message = Resources.Message.EmptyFile;
                                //objDtBlankData.TableName = "DtBlankData";
                                //objDs.Tables.Add(objDtBlankData);//Blank Records
                                Message = "File is empty! Some Mandatory columns has no required data!";
                                return;
                            }
                            #endregion

                            #region Remove Duplicate Records
                            DataSet DsValidate = new DataSet();
                            DsValidate = ds;
                            if (DsValidate.Tables[0].Columns.Contains("Error") == false)
                            {
                                DataColumn dcError = new DataColumn();
                                dcError = new DataColumn("Error", typeof(string));
                                DsValidate.Tables[0].Columns.Add(dcError);
                                DsValidate.Tables[0].AcceptChanges();
                            }

                            DataTable dt = objdsSchema.Tables[0];
                            CreateSchema(ref dt);
                            dtUpload = dt;
                            blnValidaData = ValidateExcel(ref DsValidate);
                            if (blnValidaData == false)
                            {
                                DataView dvComposite = DsValidate.Tables[0].DefaultView;
                                dvComposite.RowFilter = "Error<>''";
                                objDtDuplicateRecord = dvComposite.ToTable();
                            }
                            //if (blnChkDuplicate)
                            //{
                            //    objDtDuplicateRecord = objDtExcelSheet.Clone();
                            //    if (objDtExcelSheet.Rows.Count > 0)
                            //    {
                            //        Hashtable objHT = new Hashtable();
                            //        ArrayList objArrDuplicateList = new ArrayList();

                            //        foreach (DataRow objDR in objDtExcelSheet.Rows)
                            //        {
                            //            if (objHT.Contains(objDR[PkColumnName]))
                            //            {
                            //                objArrDuplicateList.Add(objDR);
                            //                objDtDuplicateRecord.ImportRow(objDR);
                            //            }
                            //            else
                            //            {
                            //                objHT.Add(objDR[PkColumnName], string.Empty);
                            //            }
                            //        }
                            //        foreach (DataRow objDrTmp in objArrDuplicateList)
                            //            objDtExcelSheet.Rows.Remove(objDrTmp);
                            //        objDtExcelSheet.AcceptChanges();
                            //    }
                            //}
                            #endregion

                            #region validate Data Type of excel file

                            for (int intTmpVar = 0; intTmpVar < objDtExcelSheet.Rows.Count; intTmpVar++)
                            {
                                #region ChkMandatory
                                objDV.RowFilter = " [Mandatory]=1";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if ((objDtExcelSheet.Rows[intTmpVar][strColName] == DBNull.Value) || (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]).Trim() == ""))
                                    {
                                        string strErrorMsg = strErrMsgMandatory + "[ " + strColName + " ]";
                                        InvalidRecords(intTmpVar, strErrorMsg);
                                    }
                                }
                                #endregion
                                #region Check Positive value
                                objDV.RowFilter = "  DataType like 'positive' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                    {
                                        if (!IsPositive(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                        {
                                            string strErrorMsg = strErrMsgPositive + "[" + strColName + "]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check small Integer
                                objDV.RowFilter = "  DataType like 'Int16' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                    {
                                        if (!IsInt16(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                        {
                                            string strErrorMsg = strErrMsgInt16 + "[" + strColName + "]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check Integer
                                objDV.RowFilter = "  DataType like 'Int32' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                    {
                                        if (!IsInt32(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                        {
                                            string strErrorMsg = strErrMsgInt32 + "[" + strColName + "]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check BigInteger
                                objDV.RowFilter = "  DataType like 'Int64' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                    {
                                        if (!IsInt64(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                        {
                                            string strErrorMsg = strErrMsgInt64 + "[" + strColName + "]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check decimal
                                objDV.RowFilter = "  DataType like 'Decimal' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                    {
                                        if (!IsDecimal(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                        {
                                            string strErrorMsg = strErrMsgDecimal + "[" + strColName + "]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check double
                                objDV.RowFilter = "  DataType like 'Double' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                    {
                                        if (!IsDouble(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                        {
                                            string strErrorMsg = strErrMsgDouble + "[" + strColName + "]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check date
                                objDV.RowFilter = "  DataType like 'Date' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                    {
                                        if (!IsDate(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                        {
                                            string strErrorMsg = strErrMsgDate + "[" + strColName + "]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check the fields contain AlphaNumeric value
                                objDV.RowFilter = "  DataType like 'AlphaNumeric' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                    {
                                        string strErrorMsg = strErrMsgAlphaNumeric + "[" + strColName + "]";
                                        string strPattern = @"[a-zA-Z0-9]*";
                                        Regex objRegEx = new Regex(strPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                                        if (objRegEx.IsMatch(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                        {
                                        }
                                        else
                                        {
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check Email
                                objDV.RowFilter = "  DataType like 'Email' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    string strTmp = Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]);
                                    strTmp = strTmp.Trim();
                                    if (strTmp.Length > 0)
                                    {
                                        string strPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
                                        Regex objRegEx = new Regex(strPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                                        if (objRegEx.IsMatch(strTmp))
                                        {
                                        }
                                        else
                                        {
                                            string strErrorMsg = strErrMsgEmail + "[ " + strColName + " ]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check Pincode
                                objDV.RowFilter = "  DataType like 'Pincode' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    string strTmp = Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]);
                                    strTmp = strTmp.Trim();
                                    if (strTmp.Length != 0)
                                    {
                                        if (strTmp.Length > 0)
                                        {
                                            // string strPattern = @"^\d{5}$";
                                            string strPattern = @"^([0-9]{5})$";
                                            //string strPattern = @"^([012346789][0-9]{5})$";

                                            //Regex objRegEx = new Regex(strPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                                            //if (objRegEx.IsMatch(strTmp))
                                            //{
                                            //}
                                            //else
                                            //{
                                            //    string strErrorMsg = strErrMsgPincode + "[ " + strColName + " ]";
                                            //    InvalidRecords(intTmpVar, strErrorMsg);
                                            //}
                                            bool blnchk = IsInt64(strTmp);
                                            if (blnchk == false)
                                            {
                                                string strErrorMsg = strErrMsgPincode + "[ " + strColName + " ]";
                                                InvalidRecords(intTmpVar, strErrorMsg);
                                            }
                                            else
                                                if ((strTmp.Length != 6) || (Convert.ToInt64(strTmp) == 0))
                                            {
                                                string strErrorMsg = strErrMsgPincode + "[ " + strColName + " ]";
                                                InvalidRecords(intTmpVar, strErrorMsg);
                                            }
                                        }
                                    }
                                }
                                #endregion
                                #region Check PanNo
                                objDV.RowFilter = "  DataType like 'PanNo' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    string strTmp = Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]);
                                    strTmp = strTmp.Trim();
                                    if (strTmp.Length != 0)
                                    {
                                        if (strTmp.Length != 10)
                                        {
                                            string strErrorMsg = strErrMsgPanNo + "[ " + strColName + " ]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }

                                }
                                #endregion
                                #region ChkBit
                                objDV.RowFilter = "  DataType like 'Bool' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if ((objDtExcelSheet.Rows[intTmpVar][strColName] == DBNull.Value) || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString() == "") || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString() == "1") || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString() == "0"))
                                    {
                                    }
                                    else
                                    {
                                        string strErrorMsg = strErrMsgBool + "[ " + strColName + " ]";
                                        InvalidRecords(intTmpVar, strErrorMsg);
                                    }
                                }
                                #endregion
                                #region Chk Minimum length
                                objDV.RowFilter = "  MinLength > 0 ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if ((objDtExcelSheet.Rows[intTmpVar][strColName] == DBNull.Value)
                                        || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length == 0)
                                        || (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length) == Convert.ToString(drv.Row["MinLength"]))
                                        )
                                    {
                                    }
                                    else if (objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length < Convert.ToInt32(drv.Row["MinLength"]))
                                    {
                                        string strErrorMsg = strMinLength + Convert.ToString(drv.Row["MinLength"]) + strMSgComplete + "[ " + strColName + " ]";
                                        InvalidRecords(intTmpVar, strErrorMsg);
                                    }
                                }
                                #endregion
                                #region Chk Maximum length
                                objDV.RowFilter = "  MaxLength > 0 ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if ((objDtExcelSheet.Rows[intTmpVar][strColName] == DBNull.Value)
                                        || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length == 0)
                                        || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length <= Convert.ToInt32(drv.Row["MaxLength"]))
                                        )
                                    {
                                    }
                                    else
                                    {
                                        string strErrorMsg = strMaxLength + Convert.ToString(drv.Row["MaxLength"]) + strMSgComplete + "[ " + strColName + " ]";
                                        InvalidRecords(intTmpVar, strErrorMsg);
                                    }
                                }



                                #endregion
                            }
                            #endregion
                            //objDs = null;
                            //objDs = new DataSet();
                            objDtExcelSheet.TableName = "DtExcelSheet";
                            objDtDuplicateRecord.TableName = "DtDuplicateRecord";
                            objDtBlankData.TableName = "DtBlankData";

                            objDs.Tables.Add(objDtExcelSheet); //Complete DataTable excluding blank and duplicate records                        
                            objDs.Tables.Add(objDtDuplicateRecord);//Duplicate Records
                            objDs.Tables.Add(objDtBlankData);//Blank Records
                            objSL = objSLInvalidRecords;//Errors found in diffrent columns
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        /*#CC06 Add start*/
        public bool IsValidFileSchema(out DataSet ds)
        {
            ds = new DataSet();
            Boolean blnValidaData = false;

            try
            {
                OpenXMLExcel objexcel = new OpenXMLExcel();
                using (CommonData objSchema = new CommonData())
                {
                    DataSet objdsSchema = objSchema.GetUploadFileSchema(ExcelFileNameInTable);
                    string strColName = string.Empty;
                    if (objdsSchema != null && objdsSchema.Tables.Count > 0)
                    {
                        string strColumnNames = string.Empty;
                        //ds = objexcel.ImportExcelFile(strRootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
                        ds = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);
                        if (objdsSchema.Tables[0].Rows.Count > 0)
                        {
                            #region Get Excel data from sheet1
                            //GetExcelData();
                            objDtExcelSheet = ds.Tables[0].Copy();
                            //DataTable objdtTmp = objDtExcelSheet.Clone();
                            ArrayList objArrBlankCol = new ArrayList();
                            foreach (DataColumn objDC in objDtExcelSheet.Columns)
                            {
                                if (objDC.ColumnName.Trim() == "F1" || objDC.ColumnName.Trim() == "F2" || objDC.ColumnName.Trim() == "F3" ||
                                    objDC.ColumnName.Trim() == "F4" || objDC.ColumnName.Trim() == "F5" || objDC.ColumnName.Trim() == "F6" ||
                                    objDC.ColumnName.Trim() == "F7" || objDC.ColumnName.Trim() == "F8" || objDC.ColumnName.Trim() == "F9" ||
                                    objDC.ColumnName.Trim() == "F10" || objDC.ColumnName.Trim() == "F11" || objDC.ColumnName.Trim() == "F12" ||
                                    objDC.ColumnName.Trim() == "F13" || objDC.ColumnName.Trim() == "F14" || objDC.ColumnName.Trim() == "F15" ||
                                    objDC.ColumnName.Trim() == "F16" || objDC.ColumnName.Trim() == "F17" || objDC.ColumnName.Trim() == "F18" ||
                                    objDC.ColumnName.Trim() == "F19" || objDC.ColumnName.Trim() == "F20" || objDC.ColumnName.Trim() == "F21" ||
                                    objDC.ColumnName.Trim() == "F22" || objDC.ColumnName.Trim() == "F23" || objDC.ColumnName.Trim() == "F24" ||
                                    objDC.ColumnName.Trim() == "F25" || objDC.ColumnName.Trim() == "F26" || objDC.ColumnName.Trim() == "F27" ||
                                    objDC.ColumnName.Trim() == "F28" || objDC.ColumnName.Trim() == "F29" || objDC.ColumnName.Trim() == "F30"
                                    )
                                {
                                    objArrBlankCol.Add(objDC);
                                }
                            }
                            foreach (DataColumn objDC1 in objArrBlankCol)
                                objDtExcelSheet.Columns.Remove(objDC1);
                            objDtExcelSheet.AcceptChanges();
                            #endregion

                            #region Validate schema is match or not
                            for (int sc = 0; sc < objdsSchema.Tables[0].Rows.Count; sc++)
                            {
                                if (objdsSchema.Tables[0].Rows.Count == objDtExcelSheet.Columns.Count)
                                {
                                    if (Convert.ToString(objdsSchema.Tables[0].Rows[sc]["ColumnName"]).ToUpper() != objDtExcelSheet.Columns[sc].ColumnName.ToUpper())
                                    {
                                        Message = "Invalid file format";
                                        return blnValidaData;
                                    }
                                }
                                else
                                {
                                    Message = "Number of columns not match";
                                    return blnValidaData;
                                }
                            }
                            #endregion


                        }
                        blnValidaData = true;
                    }
                    else
                    {
                        Message = "Invalid file format";

                    }

                }
                return blnValidaData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        } /*#CC06 end*/
        /*#CC01 START ADDED*/

        public DataSet GetExcelFileIntoDataset(string filelocation)
        {
            string excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filelocation + ";Extended Properties=\"Excel 12.0;IMEX=1;HDR=YES;\"";
            OleDbConnection dbCon = new OleDbConnection(excelConnectionString);
            dbCon.Open();
            // Get All Sheets Name
            DataTable dtSheetName = dbCon.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            // Retrive the Data by Sheetwise
            DataSet dsOutput = new DataSet();
            for (int nCount = 0; nCount < dtSheetName.Rows.Count; nCount++)
            {
                string sSheetName = dtSheetName.Rows[nCount]["TABLE_NAME"].ToString();
                if (sSheetName.ToLower() != "help$")
                {
                    string sQuery = "Select * From [" + sSheetName + "]";
                    OleDbCommand dbCmd = new OleDbCommand(sQuery, dbCon);
                    OleDbDataAdapter dbDa = new OleDbDataAdapter(dbCmd);
                    DataTable dtData = new DataTable();
                    dbDa.Fill(dtData);
                    dsOutput.Tables.Add(dtData);
                }
            }
            dbCon.Close();
            return dsOutput;
        }





        public void ValidateSchemaFile(bool blnChkDuplicate, out DataSet objDs, out SortedList objSL, DataSet ds)
        {
            Boolean blnValidaData;
            objDs = new DataSet();
            objSL = new SortedList();
            try
            {
                OpenXMLExcel objexcel = new OpenXMLExcel();
                using (CommonData objSchema = new CommonData())
                {
                    DataSet objdsSchema = objSchema.GetUploadFileSchema(ExcelFileNameInTable);
                    string strColName = string.Empty;
                    if (objdsSchema != null && objdsSchema.Tables.Count > 0)
                    {
                        string strColumnNames = string.Empty;
                        if (objdsSchema.Tables[0].Rows.Count > 0)
                        {
                            #region Get Excel data from sheet1
                            objDtExcelSheet = ds.Tables[0].Copy();
                            /* #CC05 Add Start */
                            #region ValidateNoOFcolumns
                            if (objDtExcelSheet != null)
                                if (objDtExcelSheet.Rows.Count > 0)
                                {
                                    if (objdsSchema.Tables[0].Rows.Count != objDtExcelSheet.Columns.Count)
                                    {
                                        Message = "Invalid file.";
                                        return;
                                    }
                                    #endregion ValidateNoOFcolumns

                                    #region ValidationForMatchingColumnsName
                                    for (int i = 0; i < objdsSchema.Tables[0].Rows.Count; i++)
                                    {
                                        if (objDtExcelSheet.Columns[i].ToString().Trim() != objdsSchema.Tables[0].Rows[i]["ColumnName"].ToString().Trim())
                                        {
                                            Message = "Columns doesn't match. Please download fresh Template to upload data.";
                                            return;
                                        }
                                    }
                                }
                            #endregion ValidationForMatchingColumnsName
                            /* #CC05 Add End */


                            ArrayList objArrBlankCol = new ArrayList();
                            foreach (DataColumn objDC in objDtExcelSheet.Columns)
                            {
                                if (objDC.ColumnName.Trim() == "F1" || objDC.ColumnName.Trim() == "F2" || objDC.ColumnName.Trim() == "F3" ||
                                    objDC.ColumnName.Trim() == "F4" || objDC.ColumnName.Trim() == "F5" || objDC.ColumnName.Trim() == "F6" ||
                                    objDC.ColumnName.Trim() == "F7" || objDC.ColumnName.Trim() == "F8" || objDC.ColumnName.Trim() == "F9" ||
                                    objDC.ColumnName.Trim() == "F10" || objDC.ColumnName.Trim() == "F11" || objDC.ColumnName.Trim() == "F12" ||
                                    objDC.ColumnName.Trim() == "F13" || objDC.ColumnName.Trim() == "F14" || objDC.ColumnName.Trim() == "F15" ||
                                    objDC.ColumnName.Trim() == "F16" || objDC.ColumnName.Trim() == "F17" || objDC.ColumnName.Trim() == "F18" ||
                                    objDC.ColumnName.Trim() == "F19" || objDC.ColumnName.Trim() == "F20" || objDC.ColumnName.Trim() == "F21" ||
                                    objDC.ColumnName.Trim() == "F22" || objDC.ColumnName.Trim() == "F23" || objDC.ColumnName.Trim() == "F24" ||
                                    objDC.ColumnName.Trim() == "F25" || objDC.ColumnName.Trim() == "F26" || objDC.ColumnName.Trim() == "F27" ||
                                    objDC.ColumnName.Trim() == "F28" || objDC.ColumnName.Trim() == "F29" || objDC.ColumnName.Trim() == "F30"
                                    )
                                {
                                    objArrBlankCol.Add(objDC);
                                }
                            }
                            foreach (DataColumn objDC1 in objArrBlankCol)
                                objDtExcelSheet.Columns.Remove(objDC1);
                            objDtExcelSheet.AcceptChanges();
                            #endregion

                            #region Validate schema is match or not
                            int Matchedschema = 0;
                            for (int sc = 0; sc < objdsSchema.Tables[0].Rows.Count; sc++)
                            {
                                for (int exlsc = 0; exlsc < objDtExcelSheet.Columns.Count; exlsc++)
                                {
                                    if (Convert.ToString(objdsSchema.Tables[0].Rows[sc]["ColumnName"]).ToUpper() == objDtExcelSheet.Columns[exlsc].ColumnName.ToUpper())
                                    {
                                        Matchedschema = Matchedschema + 1;
                                    }
                                }

                            }

                            if (Matchedschema != objdsSchema.Tables[0].Rows.Count)
                            {
                                Message = "Invalid file.";
                                return;
                            }

                            #endregion
                            /* #CC02 Add Start */
                            string colnm = string.Empty;
                            DataRow[] DrCheckColumn = objdsSchema.Tables[0].Select("Mandatory=1");

                            foreach (DataRow drCheck in DrCheckColumn)
                            {
                                foreach (DataColumn dcExcel in objDtExcelSheet.Columns)
                                {
                                    if (drCheck["ColumnName"].ToString().ToLower() == dcExcel.ToString().ToLower())
                                    {
                                        foreach (DataRow drExcelData in objDtExcelSheet.Rows)
                                        {
                                            if (drExcelData[dcExcel].ToString().Trim() == "")
                                            {
                                                Message = "Value in column <b>" + dcExcel.ToString().Trim() + "</b> cannot be left blank.";
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                            /* #CC02 Add End */
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        /*#CC01 START END*/
        public void ValidateFileBCPV5(out DataTable dtOutData, string strUploadedFileName)
        {

            //string ExcelFileNameInTable = "PrimarySalesReturnSerialExists";
            DataTable dtExcelData = new DataTable();
            DataTable dtSchema;
            lstPrimary = new List<string>();
            DataSet ds;
            string strColName;

            try
            {
                OpenXMLExcel objexcel = new OpenXMLExcel();
                using (CommonData objSchema = new CommonData())
                {
                    DataSet objdsSchema = objSchema.GetUploadFileSchema(ExcelFileNameInTable);
                    strColName = string.Empty;
                    if (objdsSchema != null && objdsSchema.Tables.Count > 0)
                    {
                        string strColumnNames = string.Empty;
                        ds = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);
                        if (objdsSchema.Tables[0].Rows.Count > 0)
                        {
                            #region Get Excel data from sheet1
                            objDtExcelSheet = ds.Tables[0].Clone();
                            ArrayList objArrBlankCol = new ArrayList();
                            foreach (DataColumn objDC in objDtExcelSheet.Columns)
                            {
                                if (objDC.ColumnName.Trim() == "F1" || objDC.ColumnName.Trim() == "F2" || objDC.ColumnName.Trim() == "F3" ||
                                    objDC.ColumnName.Trim() == "F4" || objDC.ColumnName.Trim() == "F5" || objDC.ColumnName.Trim() == "F6" ||
                                    objDC.ColumnName.Trim() == "F7" || objDC.ColumnName.Trim() == "F8" || objDC.ColumnName.Trim() == "F9" ||
                                    objDC.ColumnName.Trim() == "F10" || objDC.ColumnName.Trim() == "F11" || objDC.ColumnName.Trim() == "F12" ||
                                    objDC.ColumnName.Trim() == "F13" || objDC.ColumnName.Trim() == "F14" || objDC.ColumnName.Trim() == "F15" ||
                                    objDC.ColumnName.Trim() == "F16" || objDC.ColumnName.Trim() == "F17" || objDC.ColumnName.Trim() == "F18" ||
                                    objDC.ColumnName.Trim() == "F19" || objDC.ColumnName.Trim() == "F20" || objDC.ColumnName.Trim() == "F21" ||
                                    objDC.ColumnName.Trim() == "F22" || objDC.ColumnName.Trim() == "F23" || objDC.ColumnName.Trim() == "F24" ||
                                    objDC.ColumnName.Trim() == "F25" || objDC.ColumnName.Trim() == "F26" || objDC.ColumnName.Trim() == "F27" ||
                                    objDC.ColumnName.Trim() == "F28" || objDC.ColumnName.Trim() == "F29" || objDC.ColumnName.Trim() == "F30"
                                    )
                                {
                                    objArrBlankCol.Add(objDC);
                                }
                            }
                            foreach (DataColumn objDC1 in objArrBlankCol)
                                ds.Tables[0].Columns.Remove(objDC1);
                            ds.AcceptChanges();
                            #endregion

                            #region Validate schema is match or not
                            for (int sc = 0; sc < objdsSchema.Tables[0].Rows.Count; sc++)
                            {
                                if (objdsSchema.Tables[0].Rows.Count == objDtExcelSheet.Columns.Count)
                                {
                                    if (Convert.ToString(objdsSchema.Tables[0].Rows[sc]["ColumnName"]).ToUpper() != ds.Tables[0].Columns[sc].ColumnName.ToUpper())
                                    {
                                        Message = "Invalid file format";
                                        dtOutData = dtExcelData;
                                        return;
                                    }
                                }
                                else
                                {
                                    Message = "Number of columns not match";
                                    dtOutData = dtExcelData;
                                    return;
                                }
                            }

                            #endregion
                            #region EmptySheet
                            if (ds.Tables[0].Rows.Count == 0)
                            {
                                Message = "File is empty! Some Mandatory columns has no required data!";
                                dtOutData = dtExcelData;
                                return;
                            }
                            #endregion EmptySheet


                            /*Shift the dataset into Datatable and blank the dataset*/
                            #region AddingColumn
                            dtExcelData = ds.Tables[0];//this is the data
                            dtSchema = objdsSchema.Tables[0];//this is for the schema
                            ds = null;
                            objdsSchema = null;
                            dtExcelData.Columns.Add(AddColumn(null, "Error", typeof(System.String), 0));//adding the new column for the Error
                            dtExcelData.Columns.Add(AddColumn(null, "Id", typeof(System.Int32), 1));//adding the new column for the Error
                            for (int ij = 0; ij <= dtExcelData.Rows.Count - 1; ij++)//can be seen another way here including the 
                            {
                                dtExcelData.Rows[ij]["Id"] = Convert.ToInt32(ij + 1);
                            }
                            dtExcelData.AcceptChanges();
                            #endregion

                            #region BaseColumn
                            foreach (DataRow dr in dtSchema.Rows)
                            {
                                if (dr["BaseColumn"].ToString().ToLower().Contains("true"))
                                {

                                    List<clsErrorData> lstErrorBaseColumn = (from r in dtExcelData.AsEnumerable()
                                                                             where (r[dr["ColumnName"].ToString()] == DBNull.Value || r[dr["ColumnName"].ToString()] == "")
                                                                             select new clsErrorData
                                                                             {
                                                                                 id = Convert.ToInt32(r["Id"]),
                                                                                 Error = dr["ColumnName"].ToString() + " can not be blank"
                                                                             }).ToList<clsErrorData>();

                                    if (lstErrorBaseColumn.Count() > 0)
                                        lstErrorData.AddRange(lstErrorBaseColumn);
                                }

                                if (dr["Mandatory"].ToString().ToLower().Contains("true"))
                                {
                                    List<clsErrorData> lstErrorMandatoryColumn = (from r in dtExcelData.AsEnumerable()
                                                                                  where (r[dr["ColumnName"].ToString()] == DBNull.Value || r[dr["ColumnName"].ToString()] == "")
                                                                                  select new clsErrorData
                                                                                  {
                                                                                      id = Convert.ToInt32(r["Id"]),
                                                                                      Error = dr["ColumnName"].ToString() + " Is Mandatory Column"
                                                                                  }).ToList<clsErrorData>();
                                    if (lstErrorMandatoryColumn.Count() > 0)
                                        lstErrorData.AddRange(lstErrorMandatoryColumn);


                                }

                                if (dr["DataType"].ToString().ToLower().Equals("date") /*#CC03 START ADDED*/ || dr["DataType"].ToString().ToLower().Equals("datetime")/*#CC03 END ADDED*/ )
                                {

                                    List<clsErrorData> lstErrorDate = (from r in dtExcelData.AsEnumerable()
                                                                       where (!IsDate(r[dr["ColumnName"].ToString()]))
                                                                       select new clsErrorData
                                                                       {
                                                                           id = Convert.ToInt32(r["Id"]),
                                                                           Error = dr["ColumnName"].ToString() + " is not valid Date"
                                                                       }).ToList<clsErrorData>();
                                    if (lstErrorDate.Count() > 0)
                                        lstErrorData.AddRange(lstErrorDate);

                                }

                                if (dr["DataType"].ToString().ToLower().Equals("int"))
                                {

                                    List<clsErrorData> lstErrorInt = (from r in dtExcelData.AsEnumerable()
                                                                      where (!IsInt32(r[dr["ColumnName"].ToString()]))
                                                                      select new clsErrorData
                                                                      {
                                                                          id = Convert.ToInt32(r["Id"]),
                                                                          Error = dr["ColumnName"].ToString() + " is not valid Numeric Value"
                                                                      }).ToList<clsErrorData>();
                                    if (lstErrorInt.Count() > 0)
                                        lstErrorData.AddRange(lstErrorInt);

                                }

                                if (dr["MinLength"].ToString() != string.Empty)
                                {
                                    List<clsErrorData> lstErrorDataType = (from r in dtExcelData.AsEnumerable()
                                                                           where r[dr["ColumnName"].ToString()].ToString().Length < Convert.ToInt32(dr["MinLength"])
                                                                           select new clsErrorData
                                                                           {
                                                                               id = Convert.ToInt32(r["Id"]),
                                                                               Error = dr["ColumnName"].ToString() + " value can not be less than the given min value"
                                                                           }).ToList<clsErrorData>();
                                    if (lstErrorDataType.Count() > 0)
                                        lstErrorData.AddRange(lstErrorDataType);
                                }

                                if (dr["MaxLength"].ToString() != string.Empty)
                                {
                                    List<clsErrorData> lstErrorDataType = (from r in dtExcelData.AsEnumerable()
                                                                           where r[dr["ColumnName"].ToString()].ToString().Length > Convert.ToInt32(dr["MaxLength"])
                                                                           select new clsErrorData
                                                                           {
                                                                               id = Convert.ToInt32(r["Id"]),
                                                                               Error = dr["ColumnName"].ToString() + " value can not be greater than the given max value"
                                                                           }).ToList<clsErrorData>();
                                    if (lstErrorDataType.Count() > 0)
                                        lstErrorData.AddRange(lstErrorDataType);
                                }

                                if (dr["ColumnConstraint"].ToString().ToLower() == "primary")
                                {
                                    lstPrimary.Add("key." + dr["ColumnName"].ToString().Replace("#", ""));

                                    //var query = from row in dtGRN.AsEnumerable()
                                    //            group row by new
                                    //            {
                                    //                SkuCode = r[dr["ColumnName"].ToString()]Convert.ToString(row["SkuCode"])
                                    //            } into grp
                                    //            orderby grp.Key.SkuCode
                                    //            select new
                                    //            {
                                    //                Key = grp.Key,
                                    //                SkuCode = (grp.Key.SkuCode.Trim() == string.Empty | grp.Key.SkuCode.Trim() == null) ? Resources.Messages.SkuNotMentioned : grp.Key.SkuCode.Trim(),
                                    //                Quantity = grp.Sum(r => r.Field<Int32>("QuantityNew"))
                                    //            };
                                }
                                // var query = source.GroupBy(x => new { x.Column1, x.Column2 });
                                /*To be asked for validate constraint no need to do anything on this*/
                                /*for primary constraints*/

                            }

                            /*can be removed*/
                            foreach (DataColumn dc in dtExcelData.Columns)
                            {
                                if (dc.ColumnName.Contains("#"))
                                {
                                    dc.ColumnName = dc.ColumnName.Replace("#", "");
                                }
                            }
                            dtExcelData.AcceptChanges();

                            string strgroupby = "new(" + string.Join(",", lstPrimary.ToArray()) + ")";// "new(Str1,Str2)";
                            string strFiltergroupby = strgroupby.Replace("key.", "");
                            //string strkey = "Key.Str1,Key.Str2";

                            //To be done
                            //var lstDuplicateRecords = dtData.AsEnumerable().AsQueryable().GroupBy(strFiltergroupby, "new(Serial1)").Select(strgroupby.Replace(")", "") + ", Count() as Total)"); 


                            #endregion

                            //here we are groupting the data because Error data can have the duplicate ids 
                            var lstFullErrorDataAfterGrouping =
         from tt in
             ((from i in lstErrorData.AsEnumerable()
               group i by Convert.ToString(i.id) into g
               select new { Id = g.Key, count = g.Count(), ErrorDescription = string.Join(",", g.Select(xx => xx.Error).ToArray()) }))
         select tt;

                            if (lstFullErrorDataAfterGrouping.Count() > 0)
                            {
                                /*Populating the error with the list*/
                                foreach (DataRow drData in dtExcelData.Rows)
                                {
                                    foreach (var error in lstFullErrorDataAfterGrouping)
                                    {
                                        if (Convert.ToInt32(drData["id"]) == Convert.ToInt32(error.Id))
                                        {
                                            drData["Error"] = Convert.ToString(error.ErrorDescription);
                                            break;
                                        }
                                    }
                                }

                            }
                            else
                            {
                                if (dtExcelData.Columns.Contains("Error"))
                                    dtExcelData.Columns.Remove("Error");
                                if (dtExcelData.Columns.Contains("id"))
                                    dtExcelData.Columns.Remove("id");
                                dtExcelData.AcceptChanges();

                            }


                            /*To be done
                                                        var lstDataWithErrorDescription = (from data in dtData.AsEnumerable()
                                                                                           join errordata in lstFullErrorDataAfterGrouping.AsEnumerable()
                                                                                          on Convert.ToInt32(data["Id"]) equals Convert.ToInt32(errordata.Id)
                                                                                           select new { data,errordata.ErrorDescription });

                                        */
                            //select data.ItemArray.Concat(.Concat(errordata.ErrorDescription).ToArray();


                        }
                        dtOutData = dtExcelData;
                    }
                    dtOutData = dtExcelData;
                }
            }
            catch (Exception ex)
            {
                dtOutData = dtExcelData;
                throw new Exception(ex.Message.ToString());

            }
        }


        /// <summary>
        /// Read excel file and get data in datatable
        /// </summary>
        private void GetExcelData()
        {
            try
            {
                //   Extended Properties=HTML Import

                //  OleDbConnection objOleDbCon = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + HttpContext.Current.Server.MapPath(strUploadPath) + strUploadedFileName + ";Extended Properties=HTML Import;");

                //OleDbConnection objOleDbCon = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + HttpContext.Current.Server.MapPath(Pagebase.strUploadExcelPath) + strUploadedFileName + ";Extended Properties=Excel 8.0;");
                //OleDbConnection objOleDbCon = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Pagebase.strUploadExcelFullPath + strUploadedFileName + ";Extended Properties=Excel 8.0;");
                OleDbConnection objOleDbCon = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strUploadedFileName + ";Extended Properties=Excel 8.0;");
                OleDbDataAdapter objOleDbDA = new OleDbDataAdapter();
                OleDbCommand objOleDbCom = new OleDbCommand();
                objOleDbCom.Connection = objOleDbCon;
                objOleDbCom.CommandTimeout = 1200;

                //Server.ScriptTimeout = 1200;
                objOleDbCom.Connection.Open();                  //connection Open
                objOleDbCom.CommandText = "select * from  [Sheet1$]";
                objOleDbDA.SelectCommand = objOleDbCom;
                objOleDbDA.Fill(objDtExcelSheet);
                objOleDbCon.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Description : If any record found Invalid then it insert or append that record in sorted list
        /// </summary>
        /// <param name="intTmpVar"></param>
        /// <param name="strErrorMsg"></param>
        protected void InvalidRecords(int intTmpVar, string strErrorMsg)
        {
            try
            {
                strKey = string.Empty;
                string[] strpkeyColumnName;
                //'Pankaj'
                // if (PkColumnName != "")

                if (HttpContext.Current.Session["PkeyColumns"] != null)
                {
                    strpkeyColumnName = Convert.ToString(HttpContext.Current.Session["PkeyColumns"]).Split(',');
                    for (int i = 0; i <= strpkeyColumnName.Length - 1; i++)
                    {
                        if (strKey == string.Empty)
                            strKey = objDtExcelSheet.Rows[intTmpVar][strpkeyColumnName[i]].ToString();
                        else
                            strKey = strKey + objDtExcelSheet.Rows[intTmpVar][strpkeyColumnName[i]].ToString();
                    }

                    if (objSLInvalidRecords.ContainsKey(strKey))
                    {
                        string strAppendValue = Convert.ToString(objSLInvalidRecords.GetByIndex(objSLInvalidRecords.IndexOfKey(strKey))) + ",  " + strErrorMsg;
                        objSLInvalidRecords.SetByIndex(objSLInvalidRecords.IndexOfKey(strKey), strAppendValue);
                        //string strAppendValue = Convert.ToString(objSLInvalidRecords.GetByIndex(objSLInvalidRecords.IndexOfKey(objDtExcelSheet.Rows[intTmpVar][PkColumnName])))+ ",  " + strErrorMsg;
                        //objSLInvalidRecords.SetByIndex(objSLInvalidRecords.IndexOfKey(objDtExcelSheet.Rows[intTmpVar][PkColumnName]),strAppendValue);
                    }
                    else
                    {
                        objSLInvalidRecords.Add(strKey, strErrorMsg);
                    }
                }
                else
                {
                    Message = "Please define Primary Key.";
                    return;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }

        public void CreateDataTable(DataSet objdsSchema)
        {

            // DataTable Table1 = new DataTable();
            DataColumn myDataColumn;
            if (objdsSchema != null && objdsSchema.Tables.Count > 0 && objdsSchema.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < objdsSchema.Tables[0].Rows.Count; i++)
                {
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = Type.GetType(Convert.ToString(objdsSchema.Tables[0].Rows[i]["TableDataType"]));
                    myDataColumn.ColumnName = Convert.ToString(objdsSchema.Tables[0].Rows[i]["ColumnName"]);
                    objDtExcelSheet.Columns.Add(myDataColumn);
                }
            }
        }

        /*#CC09 Added Started*/
        public void ValidateFileWithCompanyId(bool blnChkDuplicate, out DataSet objDs, out SortedList objSL)
        {
            DataSet ds;
            Boolean blnValidaData;
            objDs = new DataSet();
            objSL = new SortedList();
            try
            {
                OpenXMLExcel objexcel = new OpenXMLExcel();
                using (CommonData objSchema = new CommonData())
                {
                    DataSet objdsSchema = objSchema.GetUploadFileSchemaWithCompanyId(ExcelFileNameInTable, CompanyId);
                    string strColName = string.Empty;
                    if (objdsSchema != null && objdsSchema.Tables.Count > 0)
                    {
                        string strColumnNames = string.Empty;
                        //ds = objexcel.ImportExcelFile(strRootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
                        ds = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);
                        if (objdsSchema.Tables[0].Rows.Count > 0)
                        {
                            #region Get Excel data from sheet1
                            //GetExcelData();
                            objDtExcelSheet = ds.Tables[0].Copy();
                            //DataTable objdtTmp = objDtExcelSheet.Clone();
                            ArrayList objArrBlankCol = new ArrayList();
                            foreach (DataColumn objDC in objDtExcelSheet.Columns)
                            {
                                if (objDC.ColumnName.Trim() == "F1" || objDC.ColumnName.Trim() == "F2" || objDC.ColumnName.Trim() == "F3" ||
                                    objDC.ColumnName.Trim() == "F4" || objDC.ColumnName.Trim() == "F5" || objDC.ColumnName.Trim() == "F6" ||
                                    objDC.ColumnName.Trim() == "F7" || objDC.ColumnName.Trim() == "F8" || objDC.ColumnName.Trim() == "F9" ||
                                    objDC.ColumnName.Trim() == "F10" || objDC.ColumnName.Trim() == "F11" || objDC.ColumnName.Trim() == "F12" ||
                                    objDC.ColumnName.Trim() == "F13" || objDC.ColumnName.Trim() == "F14" || objDC.ColumnName.Trim() == "F15" ||
                                    objDC.ColumnName.Trim() == "F16" || objDC.ColumnName.Trim() == "F17" || objDC.ColumnName.Trim() == "F18" ||
                                    objDC.ColumnName.Trim() == "F19" || objDC.ColumnName.Trim() == "F20" || objDC.ColumnName.Trim() == "F21" ||
                                    objDC.ColumnName.Trim() == "F22" || objDC.ColumnName.Trim() == "F23" || objDC.ColumnName.Trim() == "F24" ||
                                    objDC.ColumnName.Trim() == "F25" || objDC.ColumnName.Trim() == "F26" || objDC.ColumnName.Trim() == "F27" ||
                                    objDC.ColumnName.Trim() == "F28" || objDC.ColumnName.Trim() == "F29" || objDC.ColumnName.Trim() == "F30"
                                    )
                                {
                                    objArrBlankCol.Add(objDC);
                                }
                            }
                            foreach (DataColumn objDC1 in objArrBlankCol)
                                objDtExcelSheet.Columns.Remove(objDC1);
                            objDtExcelSheet.AcceptChanges();
                            #endregion

                            #region Validate schema is match or not
                            for (int sc = 0; sc < objdsSchema.Tables[0].Rows.Count; sc++)
                            {
                                if (objdsSchema.Tables[0].Rows.Count == objDtExcelSheet.Columns.Count)
                                {
                                    if (Convert.ToString(objdsSchema.Tables[0].Rows[sc]["ColumnName"]).ToUpper() != objDtExcelSheet.Columns[sc].ColumnName.ToUpper())
                                    {
                                        Message = "Invalid file format";
                                        return;
                                    }
                                }
                                else
                                {
                                    Message = "Number of columns not match";
                                    return;
                                }
                            }
                            #endregion

                            #region CreateDataViews

                            strColName = string.Empty;
                            DataView objDV = objdsSchema.Tables[0].DefaultView;

                            #endregion

                            #region Delete the rows which not having Base Column
                            objDtBlankData = objDtExcelSheet.Clone();
                            objDtExcelSheet.AcceptChanges();
                            objDtBlankData.AcceptChanges();
                            objDV.RowFilter = " [BaseColumn]=1 ";
                            if (objDtExcelSheet.Rows.Count > 0)
                            {
                                ArrayList objArrBlankList = new ArrayList();
                                if (objDV.Count > 0)
                                {
                                    foreach (DataRow objDR in objDtExcelSheet.Rows)
                                    {
                                        foreach (DataRowView drv in objDV)
                                        {
                                            //strColName = objDV.Table.Columns[intTmpArrCount].ColumnName.Trim();
                                            strColName = Convert.ToString(drv["ColumnName"]);

                                            if (objDR[strColName] == DBNull.Value || objDR[strColName] == "")
                                            {
                                                bool blnIsData = false;
                                                objArrBlankList.Add(objDR);

                                                /*#CC23082014 --looks useless */
                                                for (int sc = 0; sc < objdsSchema.Tables[0].Rows.Count; sc++)
                                                {
                                                    if (objDR[Convert.ToString(objdsSchema.Tables[0].Rows[sc]["ColumnName"])].ToString().Trim().Length > 0)
                                                    {
                                                        blnIsData = true;
                                                        break;
                                                    }
                                                }
                                                if (blnIsData)
                                                {
                                                    objDtBlankData.ImportRow(objDR);
                                                }
                                                break;

                                            }
                                        }
                                    }
                                    foreach (DataRow objDrTmp in objArrBlankList)
                                        objDtExcelSheet.Rows.Remove(objDrTmp);
                                    objDtExcelSheet.AcceptChanges();
                                }
                            }
                            #endregion

                            #region Check file is empty or not
                            if (objDtExcelSheet.Rows.Count == 0)
                            {
                                //Message = Resources.Message.EmptyFile;
                                //objDtBlankData.TableName = "DtBlankData";
                                //objDs.Tables.Add(objDtBlankData);//Blank Records
                                Message = "File is empty! Some Mandatory columns has no required data!";
                                return;
                            }
                            #endregion

                            #region Remove Duplicate Records
                            DataSet DsValidate = new DataSet();
                            DsValidate = ds;
                            if (DsValidate.Tables[0].Columns.Contains("Error") == false)
                            {
                                DataColumn dcError = new DataColumn();
                                dcError = new DataColumn("Error", typeof(string));
                                DsValidate.Tables[0].Columns.Add(dcError);
                                DsValidate.Tables[0].AcceptChanges();
                            }

                            DataTable dt = objdsSchema.Tables[0];
                            CreateSchema(ref dt);
                            dtUpload = dt;
                            blnValidaData = ValidateExcel(ref DsValidate);
                            if (blnValidaData == false)
                            {
                                DataView dvComposite = DsValidate.Tables[0].DefaultView;
                                dvComposite.RowFilter = "Error<>''";
                                objDtDuplicateRecord = dvComposite.ToTable();
                            }
                            //if (blnChkDuplicate)
                            //{
                            //    objDtDuplicateRecord = objDtExcelSheet.Clone();
                            //    if (objDtExcelSheet.Rows.Count > 0)
                            //    {
                            //        Hashtable objHT = new Hashtable();
                            //        ArrayList objArrDuplicateList = new ArrayList();

                            //        foreach (DataRow objDR in objDtExcelSheet.Rows)
                            //        {
                            //            if (objHT.Contains(objDR[PkColumnName]))
                            //            {
                            //                objArrDuplicateList.Add(objDR);
                            //                objDtDuplicateRecord.ImportRow(objDR);
                            //            }
                            //            else
                            //            {
                            //                objHT.Add(objDR[PkColumnName], string.Empty);
                            //            }
                            //        }
                            //        foreach (DataRow objDrTmp in objArrDuplicateList)
                            //            objDtExcelSheet.Rows.Remove(objDrTmp);
                            //        objDtExcelSheet.AcceptChanges();
                            //    }
                            //}
                            #endregion

                            #region validate Data Type of excel file

                            for (int intTmpVar = 0; intTmpVar < objDtExcelSheet.Rows.Count; intTmpVar++)
                            {
                                #region ChkMandatory
                                objDV.RowFilter = " [Mandatory]=1";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if ((objDtExcelSheet.Rows[intTmpVar][strColName] == DBNull.Value) || (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]).Trim() == ""))
                                    {
                                        string strErrorMsg = strErrMsgMandatory + "[ " + strColName + " ]";
                                        InvalidRecords(intTmpVar, strErrorMsg);
                                    }
                                }
                                #endregion
                                #region Check Positive value
                                objDV.RowFilter = "  DataType like 'positive' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                    {
                                        if (!IsPositive(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                        {
                                            string strErrorMsg = strErrMsgPositive + "[" + strColName + "]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check small Integer
                                objDV.RowFilter = "  DataType like 'Int16' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                    {
                                        if (!IsInt16(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                        {
                                            string strErrorMsg = strErrMsgInt16 + "[" + strColName + "]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check Integer
                                objDV.RowFilter = "  DataType like 'Int32' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                    {
                                        if (!IsInt32(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                        {
                                            string strErrorMsg = strErrMsgInt32 + "[" + strColName + "]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check BigInteger
                                objDV.RowFilter = "  DataType like 'Int64' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                    {
                                        if (!IsInt64(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                        {
                                            string strErrorMsg = strErrMsgInt64 + "[" + strColName + "]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check decimal
                                objDV.RowFilter = "  DataType like 'Decimal' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                    {
                                        if (!IsDecimal(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                        {
                                            string strErrorMsg = strErrMsgDecimal + "[" + strColName + "]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check double
                                objDV.RowFilter = "  DataType like 'Double' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                    {
                                        if (!IsDouble(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                        {
                                            string strErrorMsg = strErrMsgDouble + "[" + strColName + "]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check date
                                objDV.RowFilter = "  DataType like 'Date' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                    {
                                        if (!IsDate(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                        {
                                            string strErrorMsg = strErrMsgDate + "[" + strColName + "]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check the fields contain AlphaNumeric value
                                objDV.RowFilter = "  DataType like 'AlphaNumeric' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                    {
                                        string strErrorMsg = strErrMsgAlphaNumeric + "[" + strColName + "]";
                                        string strPattern = @"[a-zA-Z0-9]*";
                                        Regex objRegEx = new Regex(strPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                                        if (objRegEx.IsMatch(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                        {
                                        }
                                        else
                                        {
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check Email
                                objDV.RowFilter = "  DataType like 'Email' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    string strTmp = Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]);
                                    strTmp = strTmp.Trim();
                                    if (strTmp.Length > 0)
                                    {
                                        string strPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
                                        Regex objRegEx = new Regex(strPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                                        if (objRegEx.IsMatch(strTmp))
                                        {
                                        }
                                        else
                                        {
                                            string strErrorMsg = strErrMsgEmail + "[ " + strColName + " ]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check Pincode
                                objDV.RowFilter = "  DataType like 'Pincode' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    string strTmp = Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]);
                                    strTmp = strTmp.Trim();
                                    if (strTmp.Length != 0)
                                    {
                                        if (strTmp.Length > 0)
                                        {
                                            // string strPattern = @"^\d{5}$";
                                            string strPattern = @"^([0-9]{5})$";
                                            //string strPattern = @"^([012346789][0-9]{5})$";

                                            //Regex objRegEx = new Regex(strPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                                            //if (objRegEx.IsMatch(strTmp))
                                            //{
                                            //}
                                            //else
                                            //{
                                            //    string strErrorMsg = strErrMsgPincode + "[ " + strColName + " ]";
                                            //    InvalidRecords(intTmpVar, strErrorMsg);
                                            //}
                                            bool blnchk = IsInt64(strTmp);
                                            if (blnchk == false)
                                            {
                                                string strErrorMsg = strErrMsgPincode + "[ " + strColName + " ]";
                                                InvalidRecords(intTmpVar, strErrorMsg);
                                            }
                                            else
                                                if ((strTmp.Length != 6) || (Convert.ToInt64(strTmp) == 0))
                                            {
                                                string strErrorMsg = strErrMsgPincode + "[ " + strColName + " ]";
                                                InvalidRecords(intTmpVar, strErrorMsg);
                                            }
                                        }
                                    }
                                }
                                #endregion
                                #region Check PanNo
                                objDV.RowFilter = "  DataType like 'PanNo' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    string strTmp = Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]);
                                    strTmp = strTmp.Trim();
                                    if (strTmp.Length != 0)
                                    {
                                        if (strTmp.Length != 10)
                                        {
                                            string strErrorMsg = strErrMsgPanNo + "[ " + strColName + " ]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }

                                }
                                #endregion
                                #region ChkBit
                                objDV.RowFilter = "  DataType like 'Bool' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if ((objDtExcelSheet.Rows[intTmpVar][strColName] == DBNull.Value) || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString() == "") || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString() == "1") || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString() == "0"))
                                    {
                                    }
                                    else
                                    {
                                        string strErrorMsg = strErrMsgBool + "[ " + strColName + " ]";
                                        InvalidRecords(intTmpVar, strErrorMsg);
                                    }
                                }
                                #endregion
                                #region Chk Minimum length
                                objDV.RowFilter = "  MinLength > 0 ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if ((objDtExcelSheet.Rows[intTmpVar][strColName] == DBNull.Value)
                                        || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length == 0)
                                        || (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length) == Convert.ToString(drv.Row["MinLength"]))
                                        )
                                    {
                                    }
                                    else if (objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length < Convert.ToInt32(drv.Row["MinLength"]))
                                    {
                                        string strErrorMsg = strMinLength + Convert.ToString(drv.Row["MinLength"]) + strMSgComplete + "[ " + strColName + " ]";
                                        InvalidRecords(intTmpVar, strErrorMsg);
                                    }
                                }
                                #endregion
                                #region Chk Maximum length
                                objDV.RowFilter = "  MaxLength > 0 ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if ((objDtExcelSheet.Rows[intTmpVar][strColName] == DBNull.Value)
                                        || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length == 0)
                                        || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length <= Convert.ToInt32(drv.Row["MaxLength"]))
                                        )
                                    {
                                    }
                                    else
                                    {
                                        string strErrorMsg = strMaxLength + Convert.ToString(drv.Row["MaxLength"]) + strMSgComplete + "[ " + strColName + " ]";
                                        InvalidRecords(intTmpVar, strErrorMsg);
                                    }
                                }



                                #endregion
                            }
                            #endregion
                            //objDs = null;
                            //objDs = new DataSet();
                            objDtExcelSheet.TableName = "DtExcelSheet";
                            objDtDuplicateRecord.TableName = "DtDuplicateRecord";
                            objDtBlankData.TableName = "DtBlankData";

                            objDs.Tables.Add(objDtExcelSheet); //Complete DataTable excluding blank and duplicate records                        
                            objDs.Tables.Add(objDtDuplicateRecord);//Duplicate Records
                            objDs.Tables.Add(objDtBlankData);//Blank Records
                            objSL = objSLInvalidRecords;//Errors found in diffrent columns
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        /*#CC09 Added End*/
        #region ValidationMethods
        public virtual bool IsDate(object theValue)
        {
            try
            { Convert.ToDateTime(theValue); return true; }
            catch
            { return false; }
        }
        public bool IsDate(string strDateToValidate, bool getCurrDateIFNotValid, out string strGetValidDate)
        {
            strGetValidDate = string.Empty; bool returnValue = false;
            try
            { strGetValidDate = Convert.ToDateTime(strDateToValidate).ToShortDateString(); returnValue = true; }
            catch
            {
                if (getCurrDateIFNotValid)
                { strGetValidDate = DateTime.Now.ToShortDateString(); returnValue = true; }
                else { returnValue = false; }
            }
            return returnValue;
        }
        public bool IsInt16(object objTmp)
        {
            bool flag = false;
            try
            {
                int xyz = Convert.ToInt16(objTmp);
                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
        public bool IsPositive(object objTmp)
        {
            bool flag = false;
            try
            {
                if (Convert.ToDouble(objTmp) > 0)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }

            }
            catch
            {
                flag = false;
            }
            return flag;
        }
        public bool IsInt32(object objTmp)
        {
            bool flag = false;
            try
            {
                int xyz = Convert.ToInt32(objTmp);
                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
        public bool IsDecimal(object objTmp)
        {
            bool flag = false;
            try
            {
                decimal xyz = Convert.ToDecimal(objTmp);
                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
        public bool IsDouble(object objTmp)
        {
            bool flag = false;
            try
            {
                double xyz = Convert.ToDouble(objTmp);
                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
        public bool IsInt64(object objTmp)
        {
            bool flag = false;
            try
            {
                long xyz = Convert.ToInt64(objTmp);
                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
        #endregion





        public void ValidateFileFromSourceDataSet(bool blnChkDuplicate, out DataSet objDs, out SortedList objSL, DataSet dsSource)
        {
            objDs = new DataSet();
            objSL = new SortedList();
            try
            {
                //using (clsDashboard objDashboard = new clsDashboard())
                //{
                CommonData objSchema = new CommonData();
                DataSet objdsSchema = objSchema.GetUploadFileSchema(ExcelFileNameInTable);

                string strColName = string.Empty;
                if (objdsSchema != null && objdsSchema.Tables.Count > 0)
                {
                    string strColumnNames = string.Empty;
                    if (objdsSchema.Tables[0].Rows.Count > 0)
                    {
                        //   CreateDataTable(objdsSchema);
                        #region Get Excel data from sheet1
                        //GetExcelData();
                        OpenXMLExcel objexcel = new OpenXMLExcel();
                        DataSet ds = dsSource;
                        objDtExcelSheet = ds.Tables[0].Copy();
                        //DataTable objdtTmp = objDtExcelSheet.Clone();
                        ArrayList objArrBlankCol = new ArrayList();
                        foreach (DataColumn objDC in objDtExcelSheet.Columns)
                        {
                            if (objDC.ColumnName.Trim() == "F1" || objDC.ColumnName.Trim() == "F2" || objDC.ColumnName.Trim() == "F3" ||
                                objDC.ColumnName.Trim() == "F4" || objDC.ColumnName.Trim() == "F5" || objDC.ColumnName.Trim() == "F6" ||
                                objDC.ColumnName.Trim() == "F7" || objDC.ColumnName.Trim() == "F8" || objDC.ColumnName.Trim() == "F9" ||
                                objDC.ColumnName.Trim() == "F10" || objDC.ColumnName.Trim() == "F11" || objDC.ColumnName.Trim() == "F12" ||
                                objDC.ColumnName.Trim() == "F13" || objDC.ColumnName.Trim() == "F14" || objDC.ColumnName.Trim() == "F15" ||
                                objDC.ColumnName.Trim() == "F16" || objDC.ColumnName.Trim() == "F17" || objDC.ColumnName.Trim() == "F18" ||
                                objDC.ColumnName.Trim() == "F19" || objDC.ColumnName.Trim() == "F20" || objDC.ColumnName.Trim() == "F21" ||
                                objDC.ColumnName.Trim() == "F22" || objDC.ColumnName.Trim() == "F23" || objDC.ColumnName.Trim() == "F24" ||
                                objDC.ColumnName.Trim() == "F25" || objDC.ColumnName.Trim() == "F26" || objDC.ColumnName.Trim() == "F27" ||
                                objDC.ColumnName.Trim() == "F28" || objDC.ColumnName.Trim() == "F29" || objDC.ColumnName.Trim() == "F30"
                                )
                            {
                                objArrBlankCol.Add(objDC);
                            }
                        }
                        foreach (DataColumn objDC1 in objArrBlankCol)
                            objDtExcelSheet.Columns.Remove(objDC1);
                        objDtExcelSheet.AcceptChanges();
                        #endregion

                        #region Validate schema is match or not
                        for (int sc = 0; sc < objdsSchema.Tables[0].Rows.Count; sc++)
                        {
                            if (objdsSchema.Tables[0].Rows.Count == objDtExcelSheet.Columns.Count)
                            {
                                if (Convert.ToString(objdsSchema.Tables[0].Rows[sc]["ColumnName"]).ToUpper() != objDtExcelSheet.Columns[sc].ColumnName.ToUpper())
                                {
                                    Message = "Invalid file format";
                                    return;
                                }
                            }
                            else
                            {
                                Message = "Number of columns not match";
                                return;
                            }
                        }
                        #endregion

                        #region CreateDataViews

                        strColName = string.Empty;
                        DataView objDV = objdsSchema.Tables[0].DefaultView;

                        #endregion

                        #region Delete the rows which not having Base Column
                        objDtBlankData = objDtExcelSheet.Clone();
                        objDtExcelSheet.AcceptChanges();
                        objDtBlankData.AcceptChanges();
                        objDV.RowFilter = " [BaseColumn]=1 ";
                        if (objDtExcelSheet.Rows.Count > 0)
                        {
                            ArrayList objArrBlankList = new ArrayList();
                            if (objDV.Count > 0)
                            {
                                foreach (DataRow objDR in objDtExcelSheet.Rows)
                                {
                                    foreach (DataRowView drv in objDV)
                                    {
                                        //strColName = objDV.Table.Columns[intTmpArrCount].ColumnName.Trim();
                                        strColName = Convert.ToString(drv["ColumnName"]);

                                        if (objDR[strColName] == DBNull.Value || objDR[strColName] == "")
                                        {
                                            bool blnIsData = false;
                                            objArrBlankList.Add(objDR);
                                            for (int sc = 0; sc < objdsSchema.Tables[0].Rows.Count; sc++)
                                            {
                                                if (objDR[Convert.ToString(objdsSchema.Tables[0].Rows[sc]["ColumnName"])].ToString().Trim().Length > 0)
                                                {
                                                    blnIsData = true;
                                                    break;
                                                }
                                            }
                                            if (blnIsData)
                                            {
                                                objDtBlankData.ImportRow(objDR);
                                            }
                                            break;

                                        }
                                    }
                                }
                                foreach (DataRow objDrTmp in objArrBlankList)
                                    objDtExcelSheet.Rows.Remove(objDrTmp);
                                objDtExcelSheet.AcceptChanges();
                            }
                        }
                        #endregion

                        #region Check file is empty or not
                        if (objDtExcelSheet.Rows.Count == 0)
                        {
                            //Message = Resources.Message.EmptyFile;
                            Message = "File is empty!";
                            return;
                        }
                        #endregion

                        #region Remove Duplicate Records

                        if (blnChkDuplicate)
                        {
                            objDtDuplicateRecord = objDtExcelSheet.Clone();
                            if (objDtExcelSheet.Rows.Count > 0)
                            {
                                Hashtable objHT = new Hashtable();
                                ArrayList objArrDuplicateList = new ArrayList();

                                foreach (DataRow objDR in objDtExcelSheet.Rows)
                                {
                                    if (objHT.Contains(objDR[PkColumnName]))
                                    {
                                        objArrDuplicateList.Add(objDR);
                                        objDtDuplicateRecord.ImportRow(objDR);
                                    }
                                    else
                                    {
                                        objHT.Add(objDR[PkColumnName], string.Empty);
                                    }
                                }
                                foreach (DataRow objDrTmp in objArrDuplicateList)
                                    objDtExcelSheet.Rows.Remove(objDrTmp);
                                objDtExcelSheet.AcceptChanges();
                            }
                        }
                        #endregion

                        #region validate Data Type of excel file

                        for (int intTmpVar = 0; intTmpVar < objDtExcelSheet.Rows.Count; intTmpVar++)
                        {
                            #region ChkMandatory
                            objDV.RowFilter = " [Mandatory]=1";
                            foreach (DataRowView drv in objDV)
                            {
                                strColName = Convert.ToString(drv["ColumnName"]);
                                if ((objDtExcelSheet.Rows[intTmpVar][strColName] == DBNull.Value) || (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]).Trim() == ""))
                                {
                                    string strErrorMsg = strErrMsgMandatory + "[ " + strColName + " ]";
                                    InvalidRecords(intTmpVar, strErrorMsg);
                                }
                            }
                            #endregion
                            #region Check Positive value
                            objDV.RowFilter = "  DataType like 'positive' ";
                            foreach (DataRowView drv in objDV)
                            {
                                strColName = Convert.ToString(drv["ColumnName"]);
                                if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                {
                                    if (!IsPositive(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                    {
                                        string strErrorMsg = strErrMsgPositive + "[" + strColName + "]";
                                        InvalidRecords(intTmpVar, strErrorMsg);
                                    }
                                }
                            }
                            #endregion
                            #region Check small Integer
                            objDV.RowFilter = "  DataType like 'Int16' ";
                            foreach (DataRowView drv in objDV)
                            {
                                strColName = Convert.ToString(drv["ColumnName"]);
                                if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                {
                                    if (!IsInt16(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                    {
                                        string strErrorMsg = strErrMsgInt16 + "[" + strColName + "]";
                                        InvalidRecords(intTmpVar, strErrorMsg);
                                    }
                                }
                            }
                            #endregion
                            #region Check Integer
                            objDV.RowFilter = "  DataType like 'Int32' ";
                            foreach (DataRowView drv in objDV)
                            {
                                strColName = Convert.ToString(drv["ColumnName"]);
                                if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                {
                                    if (!IsInt32(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                    {
                                        string strErrorMsg = strErrMsgInt32 + "[" + strColName + "]";
                                        InvalidRecords(intTmpVar, strErrorMsg);
                                    }
                                }
                            }
                            #endregion
                            #region Check BigInteger
                            objDV.RowFilter = "  DataType like 'Int64' ";
                            foreach (DataRowView drv in objDV)
                            {
                                strColName = Convert.ToString(drv["ColumnName"]);
                                if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                {
                                    if (!IsInt64(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                    {
                                        string strErrorMsg = strErrMsgInt64 + "[" + strColName + "]";
                                        InvalidRecords(intTmpVar, strErrorMsg);
                                    }
                                }
                            }
                            #endregion
                            #region Check decimal
                            objDV.RowFilter = "  DataType like 'Decimal' ";
                            foreach (DataRowView drv in objDV)
                            {
                                strColName = Convert.ToString(drv["ColumnName"]);
                                if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                {
                                    if (!IsDecimal(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                    {
                                        string strErrorMsg = strErrMsgDecimal + "[" + strColName + "]";
                                        InvalidRecords(intTmpVar, strErrorMsg);
                                    }
                                }
                            }
                            #endregion
                            #region Check double
                            objDV.RowFilter = "  DataType like 'Double' ";
                            foreach (DataRowView drv in objDV)
                            {
                                strColName = Convert.ToString(drv["ColumnName"]);
                                if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                {
                                    if (!IsDouble(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                    {
                                        string strErrorMsg = strErrMsgDouble + "[" + strColName + "]";
                                        InvalidRecords(intTmpVar, strErrorMsg);
                                    }
                                }
                            }
                            #endregion
                            #region Check date
                            objDV.RowFilter = "  DataType like 'Date' ";
                            foreach (DataRowView drv in objDV)
                            {
                                strColName = Convert.ToString(drv["ColumnName"]);
                                if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                {
                                    if (!IsDate(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                    {
                                        string strErrorMsg = strErrMsgDate + "[" + strColName + "]";
                                        InvalidRecords(intTmpVar, strErrorMsg);
                                    }
                                }
                            }
                            #endregion
                            #region Check the fields contain AlphaNumeric value
                            objDV.RowFilter = "  DataType like 'AlphaNumeric' ";
                            foreach (DataRowView drv in objDV)
                            {
                                strColName = Convert.ToString(drv["ColumnName"]);
                                if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                {
                                    string strErrorMsg = strErrMsgAlphaNumeric + "[" + strColName + "]";
                                    string strPattern = @"[a-zA-Z0-9]*";
                                    Regex objRegEx = new Regex(strPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                                    if (objRegEx.IsMatch(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                    {
                                    }
                                    else
                                    {
                                        InvalidRecords(intTmpVar, strErrorMsg);
                                    }
                                }
                            }
                            #endregion
                            #region Check Email
                            objDV.RowFilter = "  DataType like 'Email' ";
                            foreach (DataRowView drv in objDV)
                            {
                                strColName = Convert.ToString(drv["ColumnName"]);
                                string strTmp = Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]);
                                strTmp = strTmp.Trim();
                                if (strTmp.Length > 0)
                                {
                                    string strPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
                                    Regex objRegEx = new Regex(strPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                                    if (objRegEx.IsMatch(strTmp))
                                    {
                                    }
                                    else
                                    {
                                        string strErrorMsg = strErrMsgEmail + "[ " + strColName + " ]";
                                        InvalidRecords(intTmpVar, strErrorMsg);
                                    }
                                }
                            }
                            #endregion
                            #region Check Pincode
                            objDV.RowFilter = "  DataType like 'Pincode' ";
                            foreach (DataRowView drv in objDV)
                            {
                                strColName = Convert.ToString(drv["ColumnName"]);
                                string strTmp = Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]);
                                strTmp = strTmp.Trim();
                                if (strTmp.Length != 0)
                                {
                                    if (strTmp.Length > 0)
                                    {
                                        // string strPattern = @"^\d{5}$";
                                        string strPattern = @"^([0-9]{5})$";
                                        //string strPattern = @"^([012346789][0-9]{5})$";

                                        //Regex objRegEx = new Regex(strPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                                        //if (objRegEx.IsMatch(strTmp))
                                        //{
                                        //}
                                        //else
                                        //{
                                        //    string strErrorMsg = strErrMsgPincode + "[ " + strColName + " ]";
                                        //    InvalidRecords(intTmpVar, strErrorMsg);
                                        //}
                                        bool blnchk = IsInt64(strTmp);
                                        if (blnchk == false)
                                        {
                                            string strErrorMsg = strErrMsgPincode + "[ " + strColName + " ]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                        else
                                            if ((strTmp.Length != 6) || (Convert.ToInt64(strTmp) == 0))
                                        {
                                            string strErrorMsg = strErrMsgPincode + "[ " + strColName + " ]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                            }
                            #endregion
                            #region Check PanNo
                            objDV.RowFilter = "  DataType like 'PanNo' ";
                            foreach (DataRowView drv in objDV)
                            {
                                strColName = Convert.ToString(drv["ColumnName"]);
                                string strTmp = Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]);
                                strTmp = strTmp.Trim();
                                if (strTmp.Length != 0)
                                {
                                    if (strTmp.Length != 10)
                                    {
                                        string strErrorMsg = strErrMsgPanNo + "[ " + strColName + " ]";
                                        InvalidRecords(intTmpVar, strErrorMsg);
                                    }
                                }

                            }
                            #endregion
                            #region ChkBit
                            objDV.RowFilter = "  DataType like 'Bool' ";
                            foreach (DataRowView drv in objDV)
                            {
                                strColName = Convert.ToString(drv["ColumnName"]);
                                if ((objDtExcelSheet.Rows[intTmpVar][strColName] == DBNull.Value) || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString() == "") || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString() == "1") || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString() == "0"))
                                {
                                }
                                else
                                {
                                    string strErrorMsg = strErrMsgBool + "[ " + strColName + " ]";
                                    InvalidRecords(intTmpVar, strErrorMsg);
                                }
                            }
                            #endregion
                            #region Chk Minimum length
                            objDV.RowFilter = "  MinLength > 0 ";
                            foreach (DataRowView drv in objDV)
                            {
                                strColName = Convert.ToString(drv["ColumnName"]);
                                if ((objDtExcelSheet.Rows[intTmpVar][strColName] == DBNull.Value)
                                    || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length == 0)
                                    || (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length) == Convert.ToString(drv.Row["MinLength"]))
                                    )
                                {
                                }
                                else if (objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length < Convert.ToInt32(drv.Row["MinLength"]))
                                {
                                    string strErrorMsg = strMinLength + Convert.ToString(drv.Row["MinLength"]) + strMSgComplete + "[ " + strColName + " ]";
                                    InvalidRecords(intTmpVar, strErrorMsg);
                                }
                            }
                            #endregion
                            #region Chk Maximum length
                            objDV.RowFilter = "  MaxLength > 0 ";
                            foreach (DataRowView drv in objDV)
                            {
                                strColName = Convert.ToString(drv["ColumnName"]);
                                if ((objDtExcelSheet.Rows[intTmpVar][strColName] == DBNull.Value)
                                    || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length == 0)
                                    || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length <= Convert.ToInt32(drv.Row["MaxLength"]))
                                    )
                                {
                                }
                                else
                                {
                                    string strErrorMsg = strMaxLength + Convert.ToString(drv.Row["MaxLength"]) + strMSgComplete + "[ " + strColName + " ]";
                                    InvalidRecords(intTmpVar, strErrorMsg);
                                }
                            }
                            #endregion
                        }
                        #endregion
                        //objDs = null;
                        //objDs = new DataSet();
                        objDtExcelSheet.TableName = "DtExcelSheet";
                        objDtDuplicateRecord.TableName = "DtDuplicateRecord";
                        objDtBlankData.TableName = "DtBlankData";



                        objDs.Tables.Add(objDtExcelSheet); //Complete DataTable excluding blank and duplicate records                        
                        objDs.Tables.Add(objDtDuplicateRecord);//Duplicate Records
                        objDs.Tables.Add(objDtBlankData);//Blank Records
                        objSL = objSLInvalidRecords;//Errors found in diffrent columns
                    }
                }
                //}
            }
            catch (Exception ex)
            {
                //ExceptionPolicy.HandleException(ex, Pagebase.ExceptionPolicyName);
            }

        }

        private void CreateSchema(ref DataTable dt)
        {
            if (dt.Rows.Count == 0)
            {
                throw new NullReferenceException("Schema not defined for table " + ExcelFileNameInTable);
            }

            DataTable dtNewSchema = new DataTable(dt.Rows[0]["ExcelName"].ToString());
            DataColumn dtCol;
            ArrayList PrimaryCol = new ArrayList();
            Int16 iPrimaryCount = 0;
            foreach (DataRow drow in dt.Rows)
            {
                dtCol = new DataColumn(drow["ColumnName"].ToString(), System.Type.GetType(drow["TableDataType"].ToString()));
                //dtCol.AllowDBNull = !Convert.ToBoolean(drow["Validate"].ToString());
                dtCol.AllowDBNull = !Convert.ToBoolean(drow["Validate"]);
                dtCol.ExtendedProperties.Add("Validate", Convert.ToBoolean(drow["Validate"]));
                //dtCol.ExtendedProperties.Add("Validate", Convert.ToBoolean(drow["Validate"].ToString()));

                if (Convert.ToInt32(drow["MinLength"]) != 0)
                {
                    dtCol.ExtendedProperties.Add("MinLength", drow["MinLength"].ToString());
                }
                if (Convert.ToInt32(drow["MaxLength"].ToString()) > 0)
                {
                    dtCol.ExtendedProperties.Add("MaxLength", Convert.ToInt32(drow["MaxLength"].ToString()));
                }

                dtCol.ExtendedProperties.Add("ColumnConstraint", drow["ColumnConstraint"].ToString());



                if (drow["ColumnConstraint"].ToString().ToLower() == "primary")
                {
                    if (HttpContext.Current.Session["PkeyColumns"] == null)
                        HttpContext.Current.Session["PkeyColumns"] = dtCol.ColumnName;
                    else
                        HttpContext.Current.Session["PkeyColumns"] = HttpContext.Current.Session["PkeyColumns"] + "," + dtCol.ColumnName;


                    PrimaryCol.Add(dtCol.ColumnName);
                    dtCol.ExtendedProperties.Add("ColumnName", drow["ColumnName"].ToString());
                    dtCol.ExtendedProperties.Add("TableDataType", drow["TableDataType"].ToString());
                    iPrimaryCount++;
                }
                dtNewSchema.Columns.Add(dtCol);
            }

            if (PrimaryCol.Count > 0)
            {
                DataColumn[] dtc = new DataColumn[PrimaryCol.Count];
                for (int i = 0; i < PrimaryCol.Count; i++)
                {
                    if (PrimaryCol[i] != null)
                    {
                        dtc[i] = dtNewSchema.Columns[PrimaryCol[i].ToString()];
                    }
                }
                UniqueConstraint dcolUnique = new UniqueConstraint(dtc);
                dtNewSchema.Constraints.Add("PkKey", dtc, false);
                PrimaryCol = null;
            }
            dt = null;
            dt = dtNewSchema;
        }
        DataColumn AddColumn(string columnValue, string ColumnName, Type ColumnType, Int16 intAutoIncrement)
        {
            DataColumn dc = new DataColumn();
            dc.ColumnName = ColumnName;

            if (ColumnType == typeof(int))
            {
                dc.DataType = typeof(System.Int32);
                if (intAutoIncrement == 0)
                    dc.DefaultValue = Convert.ToInt32(columnValue);
                else
                {
                    dc.AutoIncrement = true;
                    dc.AutoIncrementSeed = 1;
                    dc.AutoIncrementStep = 1;
                }


            }
            if (ColumnType == typeof(System.String))
            {
                dc.DataType = typeof(System.String);
                dc.DefaultValue = columnValue;
            }
            return dc;
        }
        private bool ValidateExcel(ref DataSet dsExcel)
        {

            DataRow dnew;
            DataTable dt;
            dt = dsExcel.Tables[0].Copy();
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new ArgumentException("No data for upload! ");
            }

            dt = dsExcel.Tables[0].Copy();
            dsExcel.Tables[0].Rows.Clear();
            foreach (DataRow erow in dt.Rows)
            {
                dnew = dtUpload.NewRow();
                try
                {
                    for (int icol = 0; icol < dsExcel.Tables[0].Columns.Count - 1; icol++)
                    {
                        //switch (dtUpload.Columns[icol].DataType.FullName.ToString())
                        //{
                        //    case "System.Int64":
                        //        if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                        //        {
                        //            if (ServerValidation.IsBigInt(erow[icol].ToString(), true) != 0)
                        //            {
                        //                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be numeric format and mandatory.");
                        //            }

                        //        }
                        //        else
                        //        {
                        //            if (ServerValidation.IsBigInt(erow[icol].ToString(), false) != 0)
                        //            {
                        //                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be numeric format ");
                        //            }

                        //        }
                        //        break;
                        //    case "System.Int32":
                        //        if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                        //        {
                        //            if (ServerValidation.IsInteger(erow[icol].ToString(), true) != 0)
                        //            {
                        //                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be numeric format and mandatory.");
                        //            }
                        //        }
                        //        else
                        //        {
                        //            if (ServerValidation.IsInteger(erow[icol].ToString(), false) != 0)
                        //            {
                        //                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be numeric format");
                        //            }
                        //        }
                        //        break;
                        //    case "System.Int16":
                        //        if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                        //        {


                        //            if (ServerValidation.IsSmallint(erow[icol].ToString(), true) != 0)
                        //            {
                        //                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be numeric format and mandatory.");
                        //            }

                        //        }
                        //        else
                        //        {

                        //            if (ServerValidation.IsSmallint(erow[icol].ToString(), false) != 0)
                        //            {
                        //                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be numeric format.");
                        //            }

                        //        }
                        //        break;
                        //    case "System.Decimal":
                        //        if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                        //        {

                        //            if (ServerValidation.IsDecimal(erow[icol].ToString(), true) != 0)
                        //            {
                        //                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be decimal format and mandatory");
                        //            }
                        //            if (erow[icol].ToString().Contains("-") == true)
                        //            {
                        //                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be negative");
                        //            }
                        //        }
                        //        else
                        //            if (ServerValidation.IsDecimal(erow[icol].ToString(), false) != 0)
                        //            {
                        //                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be decimal format ");
                        //            }
                        //        if (erow[icol].ToString() != "")
                        //        {
                        //            if (erow[icol].ToString().Contains("-") == true)
                        //            {
                        //                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be negative");
                        //            }
                        //        }

                        //        break;
                        //    case "System.DateTime":
                        //        if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                        //        {

                        //            if (ServerValidation.IsDate(erow[icol].ToString(), true) != 0)
                        //            {
                        //                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be date format and mandatory");
                        //            }
                        //        }
                        //        else
                        //        {
                        //            if (ServerValidation.IsDate(erow[icol].ToString(), false) != 0)
                        //            {
                        //                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be date format");
                        //            }
                        //        }
                        //        break;

                        //}
                        //switch (dtUpload.Columns[icol].ColumnName.ToString().ToLower())
                        //{


                        //    case "email":
                        //        //Need not any validation on the Email in Excel in Retailer upload
                        //        if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                        //        {

                        //            if (ServerValidation.IsValidEmail(erow[icol].ToString(), true) != 0)
                        //            {
                        //                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be proper email format and mandatory");
                        //            }
                        //        }
                        //        else
                        //        {

                        //            if (ServerValidation.IsValidEmail(erow[icol].ToString(), false) != 0)
                        //            {
                        //                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be proper email format.");
                        //            }
                        //        }
                        //        break;
                        //    case "pincode":
                        //        if (ServerValidation.IsPinCode(erow[icol].ToString(), true) != 0)
                        //        {
                        //            throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be proper pin code format and mandatory");
                        //        }
                        //        break;
                        //    case "mobilenumber":
                        //        if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)      //Pankaj Dhingra
                        //        {
                        //            if (ServerValidation.IsMobileNo(erow[icol].ToString(), true) != 0)
                        //            {
                        //                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be proper mobile number format and mandatory");
                        //            }
                        //        }
                        //        else
                        //        {
                        //            if (ServerValidation.IsMobileNo(erow[icol].ToString(), false) != 0)
                        //            {
                        //                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be proper mobile number format");
                        //            }
                        //        }
                        //        break;
                        //    case "invoicedate":

                        //        if (Convert.ToDateTime(erow[icol].ToString()) > System.DateTime.Now)
                        //        {
                        //            throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be less than current date and mandatory");
                        //        }

                        //        break;
                        //    case "quantity":
                        //        if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)      //Pankaj Dhingra
                        //        {
                        //            if (((Convert.ToInt32(erow[icol].ToString()) < 0)))
                        //            {
                        //                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be negative");
                        //            }
                        //        }
                        //        break;
                        //    case "externaltarget":
                        //        if (dtUpload.Columns[icol].ToString() != null && dtUpload.Columns[icol].ToString() != "")
                        //        {
                        //            if (((Convert.ToInt32(erow[icol].ToString()) < 0)))
                        //            {
                        //                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be negative");
                        //            }
                        //        }
                        //        break;
                        //    case "internaltarget":
                        //        if (dtUpload.Columns[icol].ToString() != null && dtUpload.Columns[icol].ToString() != "")
                        //        {
                        //            if (((Convert.ToInt32(erow[icol].ToString()) < 0)))
                        //            {
                        //                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be negative");
                        //            }
                        //        }
                        //        break;
                        //}
                        //if (dtUpload.Columns[icol].ExtendedProperties["MinLength"] != null)
                        //{
                        //    if (erow[icol].ToString().Length < Convert.ToInt32(dtUpload.Columns[icol].ExtendedProperties["MinLength"]))
                        //    {
                        //        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be minimum " + dtUpload.Columns[icol].ExtendedProperties["MinLength"]);
                        //    }


                        //}
                        //if (dtUpload.Columns[icol].ExtendedProperties["MaxLength"] != null)
                        //{
                        //    if (erow[icol].ToString().Length > Convert.ToInt32(dtUpload.Columns[icol].ExtendedProperties["MaxLength"]))
                        //    {
                        //        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be maximum " + dtUpload.Columns[icol].ExtendedProperties["MaxLength"]);
                        //    }

                        //if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                        //{
                        //    if (erow[icol].ToString() == "")
                        //    {
                        //        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be blank");
                        //    }
                        //}
                        //}
                        /*#CC04 START ADDED*/
                        if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                        {
                            if (erow[icol].ToString() == "")
                            {
                                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be blank");
                            }
                        }
                        /*#CC04 END ADDED*/


                        if (erow[icol] != null && erow[icol].ToString() != "")
                        {
                            dnew[icol] = erow[icol];
                        }
                    }
                    dtUpload.Rows.Add(dnew);


                }
                catch (Exception ex)
                {
                    erow["Error"] = ex.Message;
                }

                dsExcel.Tables[0].LoadDataRow(erow.ItemArray, true);
            }

            dsExcel.Tables[0].AcceptChanges();
            if (dsExcel.Tables[0].Select("isnull(Error,'')<>''").Length > 0) { return false; }
            else
            {

                dsExcel.Tables.Clear();
                dtUpload.AcceptChanges();
                dtUpload.TableName = "Table";
                dsExcel.Tables.Add(dtUpload);
                return true;

            }
        }
        /*#CC08 Added Started*/
        public void ValidatePSIFile(bool blnChkDuplicate, out DataSet objDs, out SortedList objSL, string UploadInterfacename)
        {
            DataSet ds = new DataSet();
            Boolean blnValidaData;
            objDs = new DataSet();
            objSL = new SortedList();
            try
            {
                OpenXMLExcel objexcel = new OpenXMLExcel();
                using (CommonData objSchema = new CommonData())
                {
                    DataSet objdsSchema = objSchema.GetUploadFileSchema(ExcelFileNameInTable);
                    string strColName = string.Empty;
                    if (objdsSchema != null && objdsSchema.Tables.Count > 0)
                    {
                        string strColumnNames = string.Empty;
                        //ds = objexcel.ImportExcelFile(strRootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
                        if (UploadInterfacename == "UploadPSIInfo")
                        {
                            ds = objexcel.ImportExcelFileV2(PageBase.strExcelBulkUploadPSIInfoPath + strUploadedFileName);
                        }
                        else if (UploadInterfacename == "InvoiceInfo")
                        {
                            ds = objexcel.ImportExcelFileV2(PageBase.strExcelBulkUploadPSIInvoiceInfoPath + strUploadedFileName);
                        }

                        if (objdsSchema.Tables[0].Rows.Count > 0)
                        {
                            #region Get Excel data from sheet1
                            //GetExcelData();
                            objDtExcelSheet = ds.Tables[0].Copy();
                            //DataTable objdtTmp = objDtExcelSheet.Clone();
                            ArrayList objArrBlankCol = new ArrayList();
                            foreach (DataColumn objDC in objDtExcelSheet.Columns)
                            {
                                if (objDC.ColumnName.Trim() == "F1" || objDC.ColumnName.Trim() == "F2" || objDC.ColumnName.Trim() == "F3" ||
                                    objDC.ColumnName.Trim() == "F4" || objDC.ColumnName.Trim() == "F5" || objDC.ColumnName.Trim() == "F6" ||
                                    objDC.ColumnName.Trim() == "F7" || objDC.ColumnName.Trim() == "F8" || objDC.ColumnName.Trim() == "F9" ||
                                    objDC.ColumnName.Trim() == "F10" || objDC.ColumnName.Trim() == "F11" || objDC.ColumnName.Trim() == "F12" ||
                                    objDC.ColumnName.Trim() == "F13" || objDC.ColumnName.Trim() == "F14" || objDC.ColumnName.Trim() == "F15" ||
                                    objDC.ColumnName.Trim() == "F16" || objDC.ColumnName.Trim() == "F17" || objDC.ColumnName.Trim() == "F18" ||
                                    objDC.ColumnName.Trim() == "F19" || objDC.ColumnName.Trim() == "F20" || objDC.ColumnName.Trim() == "F21" ||
                                    objDC.ColumnName.Trim() == "F22" || objDC.ColumnName.Trim() == "F23" || objDC.ColumnName.Trim() == "F24" ||
                                    objDC.ColumnName.Trim() == "F25" || objDC.ColumnName.Trim() == "F26" || objDC.ColumnName.Trim() == "F27" ||
                                    objDC.ColumnName.Trim() == "F28" || objDC.ColumnName.Trim() == "F29" || objDC.ColumnName.Trim() == "F30"
                                    )
                                {
                                    objArrBlankCol.Add(objDC);
                                }
                            }
                            foreach (DataColumn objDC1 in objArrBlankCol)
                                objDtExcelSheet.Columns.Remove(objDC1);
                            objDtExcelSheet.AcceptChanges();
                            #endregion

                            #region Validate schema is match or not
                            for (int sc = 0; sc < objdsSchema.Tables[0].Rows.Count; sc++)
                            {
                                if (objdsSchema.Tables[0].Rows.Count == objDtExcelSheet.Columns.Count)
                                {
                                    if (Convert.ToString(objdsSchema.Tables[0].Rows[sc]["ColumnName"]).ToUpper() != objDtExcelSheet.Columns[sc].ColumnName.ToUpper())
                                    {
                                        Message = "Invalid file format";
                                        return;
                                    }
                                }
                                else
                                {
                                    Message = "Number of columns not match";
                                    return;
                                }
                            }
                            #endregion

                            #region CreateDataViews

                            strColName = string.Empty;
                            DataView objDV = objdsSchema.Tables[0].DefaultView;

                            #endregion

                            #region Delete the rows which not having Base Column
                            objDtBlankData = objDtExcelSheet.Clone();
                            objDtExcelSheet.AcceptChanges();
                            objDtBlankData.AcceptChanges();
                            objDV.RowFilter = " [BaseColumn]=1 ";
                            if (objDtExcelSheet.Rows.Count > 0)
                            {
                                ArrayList objArrBlankList = new ArrayList();
                                if (objDV.Count > 0)
                                {
                                    foreach (DataRow objDR in objDtExcelSheet.Rows)
                                    {
                                        foreach (DataRowView drv in objDV)
                                        {
                                            //strColName = objDV.Table.Columns[intTmpArrCount].ColumnName.Trim();
                                            strColName = Convert.ToString(drv["ColumnName"]);

                                            if (objDR[strColName] == DBNull.Value || objDR[strColName] == "")
                                            {
                                                bool blnIsData = false;
                                                objArrBlankList.Add(objDR);

                                                /*#CC23082014 --looks useless */
                                                for (int sc = 0; sc < objdsSchema.Tables[0].Rows.Count; sc++)
                                                {
                                                    if (objDR[Convert.ToString(objdsSchema.Tables[0].Rows[sc]["ColumnName"])].ToString().Trim().Length > 0)
                                                    {
                                                        blnIsData = true;
                                                        break;
                                                    }
                                                }
                                                if (blnIsData)
                                                {
                                                    objDtBlankData.ImportRow(objDR);
                                                }
                                                break;

                                            }
                                        }
                                    }
                                    foreach (DataRow objDrTmp in objArrBlankList)
                                        objDtExcelSheet.Rows.Remove(objDrTmp);
                                    objDtExcelSheet.AcceptChanges();
                                }
                            }
                            #endregion

                            #region Check file is empty or not
                            if (objDtExcelSheet.Rows.Count == 0)
                            {
                                //Message = Resources.Message.EmptyFile;
                                //objDtBlankData.TableName = "DtBlankData";
                                //objDs.Tables.Add(objDtBlankData);//Blank Records
                                Message = "File is empty! Some Mandatory columns has no required data!";
                                return;
                            }
                            #endregion

                            #region Remove Duplicate Records
                            DataSet DsValidate = new DataSet();
                            DsValidate = ds;
                            if (DsValidate.Tables[0].Columns.Contains("Error") == false)
                            {
                                DataColumn dcError = new DataColumn();
                                dcError = new DataColumn("Error", typeof(string));
                                DsValidate.Tables[0].Columns.Add(dcError);
                                DsValidate.Tables[0].AcceptChanges();
                            }

                            DataTable dt = objdsSchema.Tables[0];
                            CreateSchema(ref dt);
                            dtUpload = dt;
                            blnValidaData = ValidateExcel(ref DsValidate);
                            if (blnValidaData == false)
                            {
                                DataView dvComposite = DsValidate.Tables[0].DefaultView;
                                dvComposite.RowFilter = "Error<>''";
                                objDtDuplicateRecord = dvComposite.ToTable();
                            }
                            //if (blnChkDuplicate)
                            //{
                            //    objDtDuplicateRecord = objDtExcelSheet.Clone();
                            //    if (objDtExcelSheet.Rows.Count > 0)
                            //    {
                            //        Hashtable objHT = new Hashtable();
                            //        ArrayList objArrDuplicateList = new ArrayList();

                            //        foreach (DataRow objDR in objDtExcelSheet.Rows)
                            //        {
                            //            if (objHT.Contains(objDR[PkColumnName]))
                            //            {
                            //                objArrDuplicateList.Add(objDR);
                            //                objDtDuplicateRecord.ImportRow(objDR);
                            //            }
                            //            else
                            //            {
                            //                objHT.Add(objDR[PkColumnName], string.Empty);
                            //            }
                            //        }
                            //        foreach (DataRow objDrTmp in objArrDuplicateList)
                            //            objDtExcelSheet.Rows.Remove(objDrTmp);
                            //        objDtExcelSheet.AcceptChanges();
                            //    }
                            //}
                            #endregion

                            #region validate Data Type of excel file

                            for (int intTmpVar = 0; intTmpVar < objDtExcelSheet.Rows.Count; intTmpVar++)
                            {
                                #region ChkMandatory
                                objDV.RowFilter = " [Mandatory]=1";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if ((objDtExcelSheet.Rows[intTmpVar][strColName] == DBNull.Value) || (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]).Trim() == ""))
                                    {
                                        string strErrorMsg = strErrMsgMandatory + "[ " + strColName + " ]";
                                        InvalidRecords(intTmpVar, strErrorMsg);
                                    }
                                }
                                #endregion
                                #region Check Positive value
                                objDV.RowFilter = "  DataType like 'positive' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                    {
                                        if (!IsPositive(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                        {
                                            string strErrorMsg = strErrMsgPositive + "[" + strColName + "]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check small Integer
                                objDV.RowFilter = "  DataType like 'Int16' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                    {
                                        if (!IsInt16(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                        {
                                            string strErrorMsg = strErrMsgInt16 + "[" + strColName + "]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check Integer
                                objDV.RowFilter = "  DataType like 'Int32' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                    {
                                        if (!IsInt32(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                        {
                                            string strErrorMsg = strErrMsgInt32 + "[" + strColName + "]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check BigInteger
                                objDV.RowFilter = "  DataType like 'Int64' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                    {
                                        if (!IsInt64(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                        {
                                            string strErrorMsg = strErrMsgInt64 + "[" + strColName + "]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check decimal
                                objDV.RowFilter = "  DataType like 'Decimal' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                    {
                                        if (!IsDecimal(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                        {
                                            string strErrorMsg = strErrMsgDecimal + "[" + strColName + "]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check double
                                objDV.RowFilter = "  DataType like 'Double' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                    {
                                        if (!IsDouble(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                        {
                                            string strErrorMsg = strErrMsgDouble + "[" + strColName + "]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check date
                                objDV.RowFilter = "  DataType like 'Date' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                    {
                                        if (!IsDate(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                        {
                                            string strErrorMsg = strErrMsgDate + "[" + strColName + "]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check the fields contain AlphaNumeric value
                                objDV.RowFilter = "  DataType like 'AlphaNumeric' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]) != "")
                                    {
                                        string strErrorMsg = strErrMsgAlphaNumeric + "[" + strColName + "]";
                                        string strPattern = @"[a-zA-Z0-9]*";
                                        Regex objRegEx = new Regex(strPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                                        if (objRegEx.IsMatch(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Trim()))
                                        {
                                        }
                                        else
                                        {
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check Email
                                objDV.RowFilter = "  DataType like 'Email' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    string strTmp = Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]);
                                    strTmp = strTmp.Trim();
                                    if (strTmp.Length > 0)
                                    {
                                        string strPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
                                        Regex objRegEx = new Regex(strPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                                        if (objRegEx.IsMatch(strTmp))
                                        {
                                        }
                                        else
                                        {
                                            string strErrorMsg = strErrMsgEmail + "[ " + strColName + " ]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }
                                }
                                #endregion
                                #region Check Pincode
                                objDV.RowFilter = "  DataType like 'Pincode' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    string strTmp = Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]);
                                    strTmp = strTmp.Trim();
                                    if (strTmp.Length != 0)
                                    {
                                        if (strTmp.Length > 0)
                                        {
                                            // string strPattern = @"^\d{5}$";
                                            string strPattern = @"^([0-9]{5})$";
                                            //string strPattern = @"^([012346789][0-9]{5})$";

                                            //Regex objRegEx = new Regex(strPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                                            //if (objRegEx.IsMatch(strTmp))
                                            //{
                                            //}
                                            //else
                                            //{
                                            //    string strErrorMsg = strErrMsgPincode + "[ " + strColName + " ]";
                                            //    InvalidRecords(intTmpVar, strErrorMsg);
                                            //}
                                            bool blnchk = IsInt64(strTmp);
                                            if (blnchk == false)
                                            {
                                                string strErrorMsg = strErrMsgPincode + "[ " + strColName + " ]";
                                                InvalidRecords(intTmpVar, strErrorMsg);
                                            }
                                            else
                                                if ((strTmp.Length != 6) || (Convert.ToInt64(strTmp) == 0))
                                            {
                                                string strErrorMsg = strErrMsgPincode + "[ " + strColName + " ]";
                                                InvalidRecords(intTmpVar, strErrorMsg);
                                            }
                                        }
                                    }
                                }
                                #endregion
                                #region Check PanNo
                                objDV.RowFilter = "  DataType like 'PanNo' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    string strTmp = Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName]);
                                    strTmp = strTmp.Trim();
                                    if (strTmp.Length != 0)
                                    {
                                        if (strTmp.Length != 10)
                                        {
                                            string strErrorMsg = strErrMsgPanNo + "[ " + strColName + " ]";
                                            InvalidRecords(intTmpVar, strErrorMsg);
                                        }
                                    }

                                }
                                #endregion
                                #region ChkBit
                                objDV.RowFilter = "  DataType like 'Bool' ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if ((objDtExcelSheet.Rows[intTmpVar][strColName] == DBNull.Value) || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString() == "") || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString() == "1") || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString() == "0"))
                                    {
                                    }
                                    else
                                    {
                                        string strErrorMsg = strErrMsgBool + "[ " + strColName + " ]";
                                        InvalidRecords(intTmpVar, strErrorMsg);
                                    }
                                }
                                #endregion
                                #region Chk Minimum length
                                objDV.RowFilter = "  MinLength > 0 ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if ((objDtExcelSheet.Rows[intTmpVar][strColName] == DBNull.Value)
                                        || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length == 0)
                                        || (Convert.ToString(objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length) == Convert.ToString(drv.Row["MinLength"]))
                                        )
                                    {
                                    }
                                    else if (objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length < Convert.ToInt32(drv.Row["MinLength"]))
                                    {
                                        string strErrorMsg = strMinLength + Convert.ToString(drv.Row["MinLength"]) + strMSgComplete + "[ " + strColName + " ]";
                                        InvalidRecords(intTmpVar, strErrorMsg);
                                    }
                                }
                                #endregion
                                #region Chk Maximum length
                                objDV.RowFilter = "  MaxLength > 0 ";
                                foreach (DataRowView drv in objDV)
                                {
                                    strColName = Convert.ToString(drv["ColumnName"]);
                                    if ((objDtExcelSheet.Rows[intTmpVar][strColName] == DBNull.Value)
                                        || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length == 0)
                                        || (objDtExcelSheet.Rows[intTmpVar][strColName].ToString().Length <= Convert.ToInt32(drv.Row["MaxLength"]))
                                        )
                                    {
                                    }
                                    else
                                    {
                                        string strErrorMsg = strMaxLength + Convert.ToString(drv.Row["MaxLength"]) + strMSgComplete + "[ " + strColName + " ]";
                                        InvalidRecords(intTmpVar, strErrorMsg);
                                    }
                                }



                                #endregion
                            }
                            #endregion
                            //objDs = null;
                            //objDs = new DataSet();
                            objDtExcelSheet.TableName = "DtExcelSheet";
                            objDtDuplicateRecord.TableName = "DtDuplicateRecord";
                            objDtBlankData.TableName = "DtBlankData";

                            objDs.Tables.Add(objDtExcelSheet); //Complete DataTable excluding blank and duplicate records                        
                            objDs.Tables.Add(objDtDuplicateRecord);//Duplicate Records
                            objDs.Tables.Add(objDtBlankData);//Blank Records
                            objSL = objSLInvalidRecords;//Errors found in diffrent columns
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        /*#CC08 Added End*/
        # region dispose
        //Call Dispose to free resources explicitly
        private bool IsDisposed = false;
        public void Dispose()
        {
            //Pass true in dispose method to clean managed resources too and say GC to skip finalize 
            // in next line.
            Dispose(true);
            //If dispose is called already then say GC to skip finalize on this instance.
            GC.SuppressFinalize(this);
        }

        ~ValidateUploadFile()
        {
            //Pass false as param because no need to free managed resources when you call finalize it
            //  will be done
            //by GC itself as its work of finalize to manage managed resources.
            Dispose(false);
        }

        //Implement dispose to free resources
        protected virtual void Dispose(bool disposedStatus)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                // Released unmanaged Resources
                if (disposedStatus)
                {
                    // Released managed Resources
                }
            }
        }

        #endregion

    }


}
