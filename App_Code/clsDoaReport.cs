#region Copyright(c) 2017 Zed-Axis Technologies All rights are reserved
/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2017 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Created By : Vijay Kumar Prajapati
* Created On: 15-Sept-2017 
 * Description: This is  DOA  Reports  Page Class Page.
* ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 28-May-2018, Sumit Maurya, #CC01, New methods created for DOA CreditNoteNumber interface (Done for motorola).
 * 11-June-2018, Rajnish Kumar, #CC02, New methods  for Upload and View Price Drop (Done for comio).
 * 10-Jul-2018, Sumit Maurya, #CC03, Properties and method copied from ZedSalesV4 for UploadDOAwithSTN interface (Done for Comio)
 * 17-Jul-2018, Sumit Maurya, #CC04, New method created for ViewDOAReport data (Done for ComioV5).
 * 18-Jul-2018, Sumit Maurya, #CC05, userId passed to get data according to logeed in user (Done for ComioV5).
 ====================================================================================================
*/
#endregion
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data.SqlClient;
using System.Xml;
using System.Configuration;
using DataAccess;




/// <summary>
/// Summary description for clsDoaReport
/// </summary>
public class clsDoaReport : IDisposable
{
    private Int32 _intPageSize;
    private DateTime _RequestDateFrom;
    private DateTime _RequestDateTo;
    private string _IMEINumber;
    private string _DoaCertificateNumber;
    private Int32 _DOAStatus;
    private Int32 _intTotalRecords;
    private Int32 _SalesChannelId;
    private Int32 _LoginUserId;
    private Int16 _DispatchMode;
    private string _Remarks;
    private string _DocketNo;
    private string _GCNNo;
    public Int16 Receivestatus { get; set; }
    public Int32 ReceiveCount { get; set; }
    public Int16 OutParam { get; set; }
    public Int64 StockDispatchIDforPrint { get; set; }
    public Int64 Dispatchid { get; set; }
    public Int16 StockReceiveType { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string WAGuid { get; set; }
    public int Export { get; set; }
    public DataTable dtDispatchItem = new DataTable();
    public DataSet dsReceive = new DataSet();
    private string _xmlList;
    public string XMLList
    {
        get
        {
            return _xmlList;
        }
        set
        {
            _xmlList = value;
        }
    }
    public string ErrorMessage
    {
        get;
        set;
    }
    public Int64 TotalRecord
    {
        get;
        set;
    }
    public Int32 PageIndex
    {
        get;
        set;
    }
    public Int32 PageSize
    {
        get
        {
            return _intPageSize;
        }
        set
        {
            _intPageSize = value;
        }
    }
    public DateTime RequestDateTo
    {
        get
        {
            return _RequestDateTo;
        }
        set
        {
            _RequestDateTo = value;
        }
    }
    public Int32 SalesChannelId
    {
        get
        {
            return _SalesChannelId;
        }
        set
        {
            _SalesChannelId = value;
        }
    }
    public Int32 LoginUserId
    {
        get
        {
            return _LoginUserId;
        }
        set
        {
            _LoginUserId = value;
        }
    }
    public Int16 DispatchMode
    {
        get
        {
            return _DispatchMode;
        }
        set
        {
            _DispatchMode = value;
        }
    }
    public string Remarks
    {
        get { return _Remarks; }
        set { _Remarks = value; }
    }
    public string DocketNo
    {
        get { return _DocketNo; }
        set { _DocketNo = value; }
    }
    public string GCNNo
    {
        get { return _GCNNo; }
        set { _GCNNo = value; }
    }
    public Int64 DoaId { get; set; }
    public DataTable DOAIDForDispatch;
    public Int32 Acknowledgetype { get; set; }
    public string CreditNote { get; set; }
    public string SwapIMEI { get; set; }
    public Int16 DispatchTOid { get; set; }
    public string CourierName { get; set; }
    public string STNNumber { get; set; }
    public string GRNNumber { get; set; }
    public string InvoiceNo { get; set; }
    public DataTable StockReceiveId;
    public Int64 StockDispatchID;
    public Int64 StockReceiveIdForPrint;
    public string ReceiveRemark { get; set; }
    public Int32 Receivedcountprint { get; set; }
    public decimal CGST { get; set; }
    public decimal SGST { get; set; }
    public decimal IGST { get; set; }
    public decimal UTGST { get; set; }
    public Int16 Stockreceivestatus { get; set; }
    public Int16 PriceDropType { get; set; }/*#CC02*/

    /* #CC03 Add Start  */
    public string SessionID
    {
        get;
        set;
    }
    public DataTable dtTvp = new DataTable();
    public int intOutParam;
    public string Error;
    /* #CC03 Add End */

    public DateTime RequestDateFrom
    {
        get
        {
            return _RequestDateFrom;
        }

        set
        {
            _RequestDateFrom = value;
        }
    }
    public string IMEINumber
    {
        get
        {
            return _IMEINumber;
        }
        set
        {
            _IMEINumber = value;
        }
    }
    public string DOACertificateNumber
    {
        get
        {
            return _DoaCertificateNumber;
        }
        set
        {
            _DoaCertificateNumber = value;
        }
    }
    public int DOAStatus
    {
        get
        {
            return _DOAStatus;
        }
        set
        {
            _DOAStatus = value;
        }
    }
    public Int32 TotalRecords
    {
        get
        {
            return _intTotalRecords;
        }
        set
        {
            _intTotalRecords = value;
        }
    }

    public clsDoaReport()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static DataTable GetEnumbyTableName(string Filename, string TableName)
    {
        DataTable dt = new DataTable();
        using (DataSet ds = new DataSet())
        {
            string filename = HttpContext.Current.Server.MapPath("~/Assets/XML/" + Filename + ".xml");
            ds.ReadXml(filename);
            dt = ds.Tables[TableName];
            if (dt == null || dt.Rows.Count == 0)
                return null;
        }
        try
        {
            dt = dt.Select("Active=1").CopyToDataTable();
            return dt;
        }
        catch (Exception)
        {
            return null;
        }
    }
    public DataSet GetReportDOAData()
    {
        try
        {

            SqlParameter[] prm = new SqlParameter[12];
            prm[0] = new SqlParameter("@IMEINumber", IMEINumber);
            prm[1] = new SqlParameter("@DOACertificateNumber", DOACertificateNumber);
            prm[2] = new SqlParameter("@DOAStatus", DOAStatus);
            if (_RequestDateFrom.Year >= 1900)
            {
                prm[3] = new SqlParameter("@RequestDateFrom", RequestDateFrom);
            }
            else
            {
                prm[3] = new SqlParameter("@RequestDateFrom", DBNull.Value);
            }
            if (_RequestDateTo.Year >= 1900)
            {
                prm[4] = new SqlParameter("@RequestDateTo", RequestDateTo);
            }
            else
            {
                prm[4] = new SqlParameter("@RequestDateTo", DBNull.Value);
            }

            prm[5] = new SqlParameter("@SalesChannelId", SalesChannelId);
            prm[6] = new SqlParameter("@LoginUserid", LoginUserId);
            prm[7] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[7].Direction = ParameterDirection.Output;
            prm[8] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[8].Direction = ParameterDirection.Output;
            prm[9] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            prm[9].Direction = ParameterDirection.Output;
            prm[10] = new SqlParameter("@PageSize", PageSize);
            prm[11] = new SqlParameter("@PageIndex", PageIndex);
            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetDOA_Record", CommandType.StoredProcedure, prm);
            ErrorMessage = Convert.ToString(prm[8].Value);
            TotalRecords = Convert.ToInt32(prm[9].Value);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet GetReportDOAMotoData()
    {
        try
        {

            SqlParameter[] prm = new SqlParameter[12];
            prm[0] = new SqlParameter("@IMEINumber", IMEINumber);
            prm[1] = new SqlParameter("@DOACertificateNumber", DOACertificateNumber);
            prm[2] = new SqlParameter("@DOAStatus", DOAStatus);
            if (_RequestDateFrom.Year >= 1900)
            {
                prm[3] = new SqlParameter("@RequestDateFrom", RequestDateFrom);
            }
            else
            {
                prm[3] = new SqlParameter("@RequestDateFrom", DBNull.Value);
            }
            if (_RequestDateTo.Year >= 1900)
            {
                prm[4] = new SqlParameter("@RequestDateTo", RequestDateTo);
            }
            else
            {
                prm[4] = new SqlParameter("@RequestDateTo", DBNull.Value);
            }

            prm[5] = new SqlParameter("@SalesChannelId", SalesChannelId);
            prm[6] = new SqlParameter("@LoginUserid", LoginUserId);
            prm[7] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[7].Direction = ParameterDirection.Output;
            prm[8] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[8].Direction = ParameterDirection.Output;
            prm[9] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            prm[9].Direction = ParameterDirection.Output;
            prm[10] = new SqlParameter("@PageSize", PageSize);
            prm[11] = new SqlParameter("@PageIndex", PageIndex);
            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetDOAUploadDetail", CommandType.StoredProcedure, prm);
            ErrorMessage = Convert.ToString(prm[8].Value);
            TotalRecords = Convert.ToInt32(prm[9].Value);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet GetReportDOAMotoDataExporttoExcel()
    {
        try
        {

            SqlParameter[] prm = new SqlParameter[9];
            prm[0] = new SqlParameter("@IMEINumber", IMEINumber);
            prm[1] = new SqlParameter("@DOACertificateNumber", DOACertificateNumber);
            prm[2] = new SqlParameter("@DOAStatus", DOAStatus);
            if (_RequestDateFrom.Year >= 1900)
            {
                prm[3] = new SqlParameter("@RequestDateFrom", RequestDateFrom);
            }
            else
            {
                prm[3] = new SqlParameter("@RequestDateFrom", DBNull.Value);
            }
            if (_RequestDateTo.Year >= 1900)
            {
                prm[4] = new SqlParameter("@RequestDateTo", RequestDateTo);
            }
            else
            {
                prm[4] = new SqlParameter("@RequestDateTo", DBNull.Value);
            }

            prm[5] = new SqlParameter("@SalesChannelId", SalesChannelId);
            prm[6] = new SqlParameter("@LoginUserid", LoginUserId);
            prm[7] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[7].Direction = ParameterDirection.Output;
            prm[8] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[8].Direction = ParameterDirection.Output;

            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetDOAUploadDetailReports", CommandType.StoredProcedure, prm);
            ErrorMessage = Convert.ToString(prm[8].Value);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet GetExcelReportDOAData()
    {
        try
        {

            SqlParameter[] prm = new SqlParameter[10];
            prm[0] = new SqlParameter("@IMEINumber", IMEINumber);
            prm[1] = new SqlParameter("@DOACertificateNumber", DOACertificateNumber);
            prm[2] = new SqlParameter("@DOAStatus", DOAStatus);
            if (_RequestDateFrom.Year >= 1900)
            {
                prm[3] = new SqlParameter("@RequestDateFrom", RequestDateFrom);
            }
            else
            {
                prm[3] = new SqlParameter("@RequestDateFrom", DBNull.Value);
            }
            if (_RequestDateTo.Year >= 1900)
            {
                prm[4] = new SqlParameter("@RequestDateTo", RequestDateTo);
            }
            else
            {
                prm[4] = new SqlParameter("@RequestDateTo", DBNull.Value);
            }
            prm[5] = new SqlParameter("@SalesChannelId", SalesChannelId);
            prm[6] = new SqlParameter("@LoginUserid", LoginUserId);
            prm[7] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[7].Direction = ParameterDirection.Output;
            prm[8] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[8].Direction = ParameterDirection.Output;
            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetDOA_RecordExcel", CommandType.StoredProcedure, prm);
            if (dsResult != null && dsResult.Tables.Count > 0)
            {
                ErrorMessage = Convert.ToString(prm[8].Value);
            }

            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet UpdateAcknowledgement()
    {
        try
        {
            SqlParameter[] prm = new SqlParameter[7];
            prm[0] = new SqlParameter("@SalesChannelid", SalesChannelId);
            prm[1] = new SqlParameter("@Doaid", DoaId);
            prm[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[2].Direction = ParameterDirection.Output;
            prm[3] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[3].Direction = ParameterDirection.Output;
            prm[4] = new SqlParameter("@Loginuserid", LoginUserId);
            prm[5] = new SqlParameter("@CreditNote", CreditNote);
            prm[6] = new SqlParameter("@SwapIMEI", SwapIMEI);

            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcDOAUpdateAcknowledgementStatus", CommandType.StoredProcedure, prm);
            OutParam = Convert.ToInt16(prm[2].Value);
            ErrorMessage = Convert.ToString(prm[3].Value);
            return dsResult;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataSet ReceiveAcknowledgement()
    {
        try
        {
            SqlParameter[] prm = new SqlParameter[5];
            prm[0] = new SqlParameter("@SalesChannelid", SalesChannelId);

            prm[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[1].Direction = ParameterDirection.Output;
            prm[2] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[2].Direction = ParameterDirection.Output;
            prm[3] = new SqlParameter("@Loginuserid", LoginUserId);
            prm[4] = new SqlParameter("@TVP_PKID", DOAIDForDispatch);
            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcDOAReceiveAcknowledgement", CommandType.StoredProcedure, prm);
            OutParam = Convert.ToInt16(prm[1].Value);
            ErrorMessage = Convert.ToString(prm[2].Value);
            return dsResult;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet ReceiveAcknowledgementV1()
    {
        try
        {
            SqlParameter[] prm = new SqlParameter[6];
            prm[0] = new SqlParameter("@SalesChannelid", SalesChannelId);

            prm[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[1].Direction = ParameterDirection.Output;
            prm[2] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[2].Direction = ParameterDirection.Output;
            prm[3] = new SqlParameter("@Loginuserid", LoginUserId);
            prm[4] = new SqlParameter("@TVP_PKID", DOAIDForDispatch);
            prm[5] = new SqlParameter("@XML_PartError", SqlDbType.Xml, 500);
            prm[5].Direction = ParameterDirection.Output;
            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcDOAReceiveAcknowledgementMotorola", CommandType.StoredProcedure, prm);
            OutParam = Convert.ToInt16(prm[1].Value);
            ErrorMessage = Convert.ToString(prm[2].Value);
            if (prm[5].Value.ToString() != "")
            {
                XMLList = prm[5].Value.ToString();
            }
            else
            {
                XMLList = null;
            }
            return dsResult;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetDispatchTo()
    {
        try
        {
            SqlParameter[] prm = new SqlParameter[1]; /* #CC05 length increased from 0 to 1*/
            prm[0] = new SqlParameter("@UserID", LoginUserId); /* #CC05 Added */
            DataTable dsResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcDOAGetWarehouseName", CommandType.StoredProcedure, prm);
            return dsResult;
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataSet SaveDispatch()
    {
        try
        {
            SqlParameter[] prm = new SqlParameter[13];
            prm[0] = new SqlParameter("@SalesChannelid", SalesChannelId);
            prm[1] = new SqlParameter("@LoginUserId", LoginUserId);
            prm[2] = new SqlParameter("@DispatchMode", DispatchMode);
            prm[3] = new SqlParameter("@Remarks", Remarks);
            prm[4] = new SqlParameter("@DocketNo", DocketNo);
            prm[5] = new SqlParameter("@GCNNo", GCNNo);
            prm[6] = new SqlParameter("@CourierName", CourierName);
            prm[7] = new SqlParameter("@DispatchToID", DispatchTOid);
            prm[8] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[8].Direction = ParameterDirection.Output;
            prm[9] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[9].Direction = ParameterDirection.Output;
            prm[10] = new SqlParameter("@TVP_PKID", DOAIDForDispatch);
            prm[11] = new SqlParameter("@StockDispatchIDforPrint", SqlDbType.BigInt);
            prm[11].Direction = ParameterDirection.Output;
            prm[12] = new SqlParameter("@InvoiceDate", InvoiceDate);

            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcDOADispatchDefective", CommandType.StoredProcedure, prm);
            OutParam = Convert.ToInt16(prm[8].Value);
            ErrorMessage = Convert.ToString(prm[9].Value);
            if (OutParam == 0)
            {
                StockDispatchIDforPrint = Convert.ToInt64(prm[11].Value);
            }


            return dsResult;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet GetDoaDispatchPrint()
    {
        try
        {
            SqlParameter[] prm = new SqlParameter[4];
            prm[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[0].Direction = ParameterDirection.Output;
            prm[1] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 1000);
            prm[1].Direction = ParameterDirection.Output;
            prm[2] = new SqlParameter("@DispatchId", Dispatchid);
            prm[3] = new SqlParameter("@Saleschannelid", SalesChannelId);
            DataSet dsresult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcDoaDispatchPrint", CommandType.StoredProcedure, prm);
            OutParam = Convert.ToInt16(prm[0].Value);
            ErrorMessage = Convert.ToString(prm[1].Value);
            return dsresult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet GetDoaGRNPrint()
    {
        try
        {
            SqlParameter[] prm = new SqlParameter[5];
            prm[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[0].Direction = ParameterDirection.Output;
            prm[1] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 1000);
            prm[1].Direction = ParameterDirection.Output;
            prm[2] = new SqlParameter("@StockReceiveId", StockReceiveIdForPrint);
            prm[3] = new SqlParameter("@Saleschannelid", SalesChannelId);
            prm[4] = new SqlParameter("@ReceiveCountoutput", SqlDbType.Int);
            prm[4].Direction = ParameterDirection.Output;
            DataSet dsresult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcDoaGRNPrint", CommandType.StoredProcedure, prm);
            OutParam = Convert.ToInt16(prm[0].Value);
            ErrorMessage = Convert.ToString(prm[1].Value);
            if (dsresult.Tables[1].Rows.Count > 0)
            {
                Receivedcountprint = Convert.ToInt32(prm[4].Value);
            }
            else
            {
                Receivedcountprint = 0;
            }

            return dsresult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet GetSTNDEtails()
    {
        try
        {
            SqlParameter[] prm = new SqlParameter[3];
            prm[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[0].Direction = ParameterDirection.Output;
            prm[1] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 1000);
            prm[1].Direction = ParameterDirection.Output;
            prm[2] = new SqlParameter("@StockDispatchID", StockDispatchID);
            DataSet dsresult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcDoaViewStockDispatchSTN", CommandType.StoredProcedure, prm);
            OutParam = Convert.ToInt16(prm[0].Value);
            ErrorMessage = Convert.ToString(prm[1].Value);
            return dsresult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet BindSTNData()
    {
        try
        {
            SqlParameter[] prm = new SqlParameter[8];
            prm[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[0].Direction = ParameterDirection.Output;
            prm[1] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 1000);
            prm[1].Direction = ParameterDirection.Output;
            prm[2] = new SqlParameter("@STNNo", STNNumber);
            prm[3] = new SqlParameter("@ImeiNumber", IMEINumber);
            prm[4] = new SqlParameter("@CertificateNo", DOACertificateNumber);
            if (RequestDateFrom.Year >= 1900)
            {
                prm[5] = new SqlParameter("@DispatchFromData", RequestDateFrom);
            }
            else
            {
                prm[5] = new SqlParameter("@DispatchFromData", DBNull.Value);
            }
            if (RequestDateTo.Year >= 1900)
            {
                prm[6] = new SqlParameter("@DispatchToDate", RequestDateTo);
            }
            else
            {
                prm[6] = new SqlParameter("@DispatchToDate", DBNull.Value);
            }
            prm[7] = new SqlParameter("@Loginuserid", LoginUserId);
            DataSet dsresult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcDoaSearchSTN_Detail", CommandType.StoredProcedure, prm);
            OutParam = Convert.ToInt16(prm[0].Value);
            ErrorMessage = Convert.ToString(prm[1].Value);
            return dsresult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataSet BindSTNDataR()
    {
        try
        {
            SqlParameter[] prm = new SqlParameter[13];
            prm[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[0].Direction = ParameterDirection.Output;
            prm[1] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 1000);
            prm[1].Direction = ParameterDirection.Output;
            prm[2] = new SqlParameter("@STNNo", STNNumber);
            prm[3] = new SqlParameter("@ImeiNumber", IMEINumber);
            prm[4] = new SqlParameter("@CertificateNo", DOACertificateNumber);
            if (RequestDateFrom.Year >= 1900)
            {
                prm[5] = new SqlParameter("@DispatchFromData", RequestDateFrom);
            }
            else
            {
                prm[5] = new SqlParameter("@DispatchFromData", DBNull.Value);
            }
            if (RequestDateTo.Year >= 1900)
            {
                prm[6] = new SqlParameter("@DispatchToDate", RequestDateTo);
            }
            else
            {
                prm[6] = new SqlParameter("@DispatchToDate", DBNull.Value);
            }
            prm[7] = new SqlParameter("@Loginuserid", LoginUserId);
            prm[8] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            prm[8].Direction = ParameterDirection.Output;
            prm[9] = new SqlParameter("@PageSize", PageSize);
            prm[10] = new SqlParameter("@PageIndex", PageIndex);
            prm[11] = new SqlParameter("@Invoiceno", InvoiceNo);
            prm[12] = new SqlParameter("@Receivestatus", Receivestatus);
            DataSet dsresult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcDoaSearchSTN_Detail", CommandType.StoredProcedure, prm);
            OutParam = Convert.ToInt16(prm[0].Value);
            ErrorMessage = Convert.ToString(prm[1].Value);
            TotalRecords = Convert.ToInt32(prm[8].Value);
            return dsresult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataSet BindViewSTNData()
    {
        try
        {
            SqlParameter[] prm = new SqlParameter[14];
            prm[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[0].Direction = ParameterDirection.Output;
            prm[1] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 1000);
            prm[1].Direction = ParameterDirection.Output;
            prm[2] = new SqlParameter("@STNNo", STNNumber);
            prm[3] = new SqlParameter("@ImeiNumber", IMEINumber);
            prm[4] = new SqlParameter("@CertificateNo", DOACertificateNumber);
            if (RequestDateFrom.Year >= 1900)
            {
                prm[5] = new SqlParameter("@DispatchFromData", RequestDateFrom);
            }
            else
            {
                prm[5] = new SqlParameter("@DispatchFromData", DBNull.Value);
            }
            if (RequestDateTo.Year >= 1900)
            {
                prm[6] = new SqlParameter("@DispatchToDate", RequestDateTo);
            }
            else
            {
                prm[6] = new SqlParameter("@DispatchToDate", DBNull.Value);
            }
            prm[7] = new SqlParameter("@Loginuserid", LoginUserId);
            prm[8] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            prm[8].Direction = ParameterDirection.Output;
            prm[9] = new SqlParameter("@PageSize", PageSize);
            prm[10] = new SqlParameter("@PageIndex", PageIndex);
            prm[11] = new SqlParameter("@ReceiveStatus", Receivestatus);
            prm[12] = new SqlParameter("@SalesChannelid", SalesChannelId);
            prm[13] = new SqlParameter("@GRNNumber", GRNNumber);
            DataSet dsresult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcDoaViewSTNDetail", CommandType.StoredProcedure, prm);
            OutParam = Convert.ToInt16(prm[0].Value);
            ErrorMessage = Convert.ToString(prm[1].Value);
            TotalRecords = Convert.ToInt32(prm[8].Value);
            return dsresult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet SaveUpdateDispatchRecord()
    {
        try
        {
            SqlParameter[] prm = new SqlParameter[11];
            prm[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[0].Direction = ParameterDirection.Output;
            prm[1] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 1000);
            prm[1].Direction = ParameterDirection.Output;
            prm[2] = new SqlParameter("@TVP_DOAReceiveID", StockReceiveId);
            prm[3] = new SqlParameter("@Loginuserid", LoginUserId);
            prm[4] = new SqlParameter("@ReceiveRemark", ReceiveRemark);
            prm[5] = new SqlParameter("@stockdispatchid", StockDispatchID);
            prm[6] = new SqlParameter("@StockReceiveIdForPrintGRN", SqlDbType.BigInt);
            prm[6].Direction = ParameterDirection.Output;
            prm[7] = new SqlParameter("@CGST", CGST);
            prm[8] = new SqlParameter("@SGST", SGST);
            prm[9] = new SqlParameter("@IGST", IGST);
            prm[10] = new SqlParameter("@UTGST", UTGST);
            DataSet dsresult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcDoaReceiveGRN", CommandType.StoredProcedure, prm);
            OutParam = Convert.ToInt16(prm[0].Value);
            ErrorMessage = Convert.ToString(prm[1].Value);
            if (OutParam == 0)
            {
                StockReceiveIdForPrint = Convert.ToInt64(prm[6].Value);
            }
            return dsresult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataSet uploadDOANote()
    {
        try
        {
            SqlParameter[] prm = new SqlParameter[5];
            prm[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[0].Direction = ParameterDirection.Output;
            prm[1] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 1000);
            prm[1].Direction = ParameterDirection.Output;
            prm[2] = new SqlParameter("@Loginuserid", LoginUserId);
            prm[3] = new SqlParameter("@SalesChannelId", SalesChannelId);
            prm[4] = new SqlParameter("@WAGuid", WAGuid);
            DataSet dsresult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("UploadDOACreditNotes", CommandType.StoredProcedure, prm);
            OutParam = Convert.ToInt16(prm[0].Value);
            ErrorMessage = Convert.ToString(prm[1].Value);
            return dsresult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet GetReportDOA()
    {
        try
        {

            SqlParameter[] prm = new SqlParameter[10];
            prm[0] = new SqlParameter("@IMEINumber", IMEINumber);
            prm[1] = new SqlParameter("@DOACertificateNumber", DOACertificateNumber);
            prm[2] = new SqlParameter("@DOAStatus", DOAStatus);
            if (_RequestDateFrom.Year >= 1900)
            {
                prm[3] = new SqlParameter("@RequestDateFrom", RequestDateFrom);
            }
            else
            {
                prm[3] = new SqlParameter("@RequestDateFrom", DBNull.Value);
            }
            if (_RequestDateTo.Year >= 1900)
            {
                prm[4] = new SqlParameter("@RequestDateTo", RequestDateTo);
            }
            else
            {
                prm[4] = new SqlParameter("@RequestDateTo", DBNull.Value);
            }

            prm[5] = new SqlParameter("@SalesChannelId", SalesChannelId);
            prm[6] = new SqlParameter("@LoginUserid", LoginUserId);
            prm[7] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[7].Direction = ParameterDirection.Output;
            prm[8] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[8].Direction = ParameterDirection.Output;
            prm[9] = new SqlParameter("@ReceiveStatus", Stockreceivestatus);

            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetDOAReports", CommandType.StoredProcedure, prm);
            if (dsResult != null && dsResult.Tables.Count > 0)
            {
                ErrorMessage = Convert.ToString(prm[8].Value);
            }

            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public DataSet ViewSTNReports()
    {
        try
        {

            SqlParameter[] prm = new SqlParameter[9];
            prm[0] = new SqlParameter("@ImeiNumber", IMEINumber);
            prm[1] = new SqlParameter("@CertificateNo", DOACertificateNumber);
            prm[2] = new SqlParameter("@ReceiveStatus", Receivestatus);
            if (_RequestDateFrom.Year >= 1900)
            {
                prm[3] = new SqlParameter("@DispatchFromData", RequestDateFrom);
            }
            else
            {
                prm[3] = new SqlParameter("@DispatchFromData", DBNull.Value);
            }
            if (_RequestDateTo.Year >= 1900)
            {
                prm[4] = new SqlParameter("@DispatchToDate", RequestDateTo);
            }
            else
            {
                prm[4] = new SqlParameter("@DispatchToDate", DBNull.Value);
            }

            prm[5] = new SqlParameter("@SalesChannelId", SalesChannelId);
            prm[6] = new SqlParameter("@Loginuserid", LoginUserId);
            prm[7] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[7].Direction = ParameterDirection.Output;
            prm[8] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[8].Direction = ParameterDirection.Output;
            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcViewSTNReports", CommandType.StoredProcedure, prm);
            if (dsResult != null && dsResult.Tables.Count > 0)
            {
                ErrorMessage = Convert.ToString(prm[8].Value);
            }

            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet uploadDOAApprovedReject()
    {
        try
        {
            SqlParameter[] prm = new SqlParameter[5];
            prm[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[0].Direction = ParameterDirection.Output;
            prm[1] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 1000);
            prm[1].Direction = ParameterDirection.Output;
            prm[2] = new SqlParameter("@Loginuserid", LoginUserId);
            prm[3] = new SqlParameter("@SalesChannelId", SalesChannelId);
            prm[4] = new SqlParameter("@WAGuid", WAGuid);
            DataSet dsresult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("UploadDOAApprovedReject", CommandType.StoredProcedure, prm);
            OutParam = Convert.ToInt16(prm[0].Value);
            ErrorMessage = Convert.ToString(prm[1].Value);
            return dsresult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet GetSerialNumberLength()
    {
        try
        {
            SqlParameter[] prm = new SqlParameter[0];
            DataSet dsresult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("PrcAPIGetSerialLengthforprimarySale", CommandType.StoredProcedure, prm);
            return dsresult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }



    /* #CC03 Add  Start */
    public DataSet SaveDOAUpload()
    {
        Int16 Result;
        DataSet dsResult = new DataSet();

        SqlParameter[] SqlParam = new SqlParameter[6];
        SqlParam[0] = new SqlParameter("@SessionId", SessionID);
        SqlParam[1] = new SqlParameter("@OutParam", SqlDbType.Int);
        SqlParam[1].Direction = ParameterDirection.Output;
        SqlParam[2] = new SqlParameter("@outError", SqlDbType.NVarChar, 200);
        SqlParam[2].Direction = ParameterDirection.Output;
        SqlParam[3] = new SqlParameter("@tvpDoaReuestData", dtTvp);
        dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcApiDOADataRequestSave", CommandType.StoredProcedure, SqlParam);
        intOutParam = Convert.ToInt16(SqlParam[1].Value);
        Error = Convert.ToString(SqlParam[2].Value);
        return dsResult;
    }
    /* #CC03 Add End*/

    /* #CC01 Add Start */

    public DataSet UploadDOACreditNote()
    {
        try
        {
            SqlParameter[] prm = new SqlParameter[5];
            prm[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[0].Direction = ParameterDirection.Output;
            prm[1] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 1000);
            prm[1].Direction = ParameterDirection.Output;
            prm[2] = new SqlParameter("@UserID", LoginUserId);
            prm[3] = new SqlParameter("@SessionID", WAGuid);
            DataSet dsresult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("DOACreditNoteNumberUpload", CommandType.StoredProcedure, prm);
            OutParam = Convert.ToInt16(prm[0].Value);
            ErrorMessage = Convert.ToString(prm[1].Value);
            return dsresult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

         /* #C01 Add End */

    
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

    ~clsDoaReport()
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
   /*#CC02 start*/ public DataSet uploadPriceDrop()
    {
        try
        {
            SqlParameter[] prm = new SqlParameter[4];
            prm[0] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
            prm[0].Direction = ParameterDirection.Output;
            prm[1] = new SqlParameter("@OutError", SqlDbType.NVarChar, 1000);
            prm[1].Direction = ParameterDirection.Output;
            prm[2] = new SqlParameter("@WebUserId", LoginUserId);
            prm[3] = new SqlParameter("@sessionId", WAGuid);
            DataSet dsresult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("PrcSavePriceDrop", CommandType.StoredProcedure, prm);
            OutParam = Convert.ToInt16(prm[0].Value);
            ErrorMessage = Convert.ToString(prm[1].Value);
            return dsresult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet ViewPriceDropReports()
    {
        try
        {

            SqlParameter[] prm = new SqlParameter[8];
            prm[0] = new SqlParameter("@SerialNumber1", IMEINumber);
            prm[1] = new SqlParameter("@PriceDropType", PriceDropType);
            if (_RequestDateFrom.Year >= 1900)
            {
                prm[2] = new SqlParameter("@PriceDropFromDate", RequestDateFrom);
            }
            else
            {
                prm[2] = new SqlParameter("@PriceDropFromDate", DBNull.Value);
            }
            if (_RequestDateTo.Year >= 1900)
            {
                prm[3] = new SqlParameter("@PriceDropTODate", RequestDateTo);
            }
            else
            {
                prm[3] = new SqlParameter("@PriceDropTODate", DBNull.Value);
            }

            prm[4] = new SqlParameter("@SalesChannelId", SalesChannelId);
            prm[5] = new SqlParameter("@Loginuserid", LoginUserId);
            prm[6] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[6].Direction = ParameterDirection.Output;
            prm[7] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[7].Direction = ParameterDirection.Output;
            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcViewPriceDropReports", CommandType.StoredProcedure, prm);
            if (dsResult != null && dsResult.Tables.Count > 0)
            {
                ErrorMessage = Convert.ToString(prm[7].Value);
            }

            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    } /*#CC02 end*/
    
    /* #CC04 Add Start */
    public DataSet GetReportDOAWithSTN()
    {
        try
        {
            SqlParameter[] prm = new SqlParameter[10];
            prm[0] = new SqlParameter("@IMEINumber", IMEINumber);
            prm[1] = new SqlParameter("@DOACertificateNumber", DOACertificateNumber);
            prm[2] = new SqlParameter("@DOAStatus", DOAStatus);
            if (_RequestDateFrom.Year >= 1900)
            {
                prm[3] = new SqlParameter("@RequestDateFrom", RequestDateFrom);
            }
            else
            {
                prm[3] = new SqlParameter("@RequestDateFrom", DBNull.Value);
            }
            if (_RequestDateTo.Year >= 1900)
            {
                prm[4] = new SqlParameter("@RequestDateTo", RequestDateTo);
            }
            else
            {
                prm[4] = new SqlParameter("@RequestDateTo", DBNull.Value);
            }
            prm[5] = new SqlParameter("@SalesChannelId", SalesChannelId);
            prm[6] = new SqlParameter("@LoginUserid", LoginUserId);
            prm[7] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[7].Direction = ParameterDirection.Output;
            prm[8] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[8].Direction = ParameterDirection.Output;
            prm[9] = new SqlParameter("@ReceiveStatus", Stockreceivestatus);
            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetDOAReportsWithSTN", CommandType.StoredProcedure, prm);
            if (dsResult != null && dsResult.Tables.Count > 0)
            {
                ErrorMessage = Convert.ToString(prm[8].Value);
            }
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /* #CC04 Add End */

}
