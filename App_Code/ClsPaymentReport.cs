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
* Created On: 12-12-2018
 * Description: This is a copy of Salesdata from DataAccess.
* ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 ====================================================================================================
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Xml;

/// <summary>
/// Summary description for ClsPaymentReport
/// </summary>
public class ClsPaymentReport : IDisposable
{
    #region Private Varible
    DataTable dtResult = new DataTable();
    private DateTime _FromDate;
    private DateTime _Todate;
    SqlParameter[] SqlParam;
    private Int64 _userid;
    private Int32 _intPageIndex;
    private Int32 _intPageSize;
    private Int32 _EntityTypeId;
    private Int32 _SalesChannelTypeID;
    private Int64 _EntitytypeUserId;
    private Int32 _SalesChannelRetailerID;
    private TimeSpan? _InTime;
    private TimeSpan? _OutTime;
    #endregion
    #region Public Varible
    public DateTime FromDate
    {
        get { return _FromDate; }
        set { _FromDate = value; }
    }
    public DateTime Todate
    {
        get { return _Todate; }
        set { _Todate = value; }
    }
    public Int32 CompanyId { get; set; }
    public Int64 UserId
    {
        get { return _userid; }
        set { _userid = value; }
    }

    public Int64 EntitytypeUserId
    {
        get { return _EntitytypeUserId; }
        set { _EntitytypeUserId = value; }
    }
    public Int32 AttendanceStatus { get; set; }
    public Int16 LeaveStatus { get;set; }
    public Int32 BrandId { get; set; }
    public Int32 ProductCategoryId { get; set; }
    public Int32 PageIndex
    {
        private get
        {
            return _intPageIndex;
        }
        set
        {
            _intPageIndex = value;
        }
    }
    public Int32 PageSize
    {
        private get
        {
            return _intPageSize;
        }
        set
        {
            _intPageSize = value;
        }
    }
    public Int32 EntityTypeId
    {
        private get
        {
            return _EntityTypeId;
        }
        set
        {
            _EntityTypeId = value;
        }
    }
    public Int32 SalesChannelTypeID
    {
        private get
        {
            return _SalesChannelTypeID;
        }
        set
        {
            _SalesChannelTypeID = value;
        }
    }
    public Int32 SalesChannelRetailerID
    {
        private get
        {
            return _SalesChannelRetailerID;
        }
        set
        {
            _SalesChannelRetailerID = value;
        }
    }
    public Int32 TotalRecords
    {
        get;
        set;
    }
    public string ErrorMessage
    {
        get;
        set;
    }
    public TimeSpan? InTime
    {
        get
        {
            return _InTime;
        }
        set
        {
            _InTime = value;
        }
    }

    public TimeSpan? OutTime
    {
        get
        {
            return _OutTime;
        }
        set
        {
            _OutTime = value;
        }
    }
    #endregion
    public ClsPaymentReport()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public DataTable GetEntityTypeV5API()
    {

        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@CompanyId", CompanyId);
            SqlParam[1] = new SqlParameter("@UserId", UserId);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetEntityTypeForPaymentReport", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetEntityTypeName()
    {
        try
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@EntityTypeId", EntityTypeId);
            SqlParam[1] = new SqlParameter("@UserId", UserId);
            SqlParam[2] = new SqlParameter("@CompanyId", CompanyId);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetEntityTypeNameForPaymentReport", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetSalesChannelTypeV5API()
    {
        try
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@UserId", UserId);
            SqlParam[2] = new SqlParameter("@CompanyId", CompanyId);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelTypeDatabaseV5", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable BindSalesChannelName()
    {
        try
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelTsmApiReports", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet GetReportPaymentData()
    {
        try
        {

            SqlParameter[] prm = new SqlParameter[14];
            prm[0] = new SqlParameter("@UserId", UserId);
            prm[1] = new SqlParameter("@EntityTypeId", EntityTypeId);
            prm[2] = new SqlParameter("@EntitytypeUserId", EntitytypeUserId);
            if (FromDate.Year >= 1900)
            {
                prm[3] = new SqlParameter("@FromDate", FromDate);
            }
            else
            {
                prm[3] = new SqlParameter("@FromDate", DBNull.Value);
            }
            if (Todate.Year >= 1900)
            {
                prm[4] = new SqlParameter("@Todate", Todate);
            }
            else
            {
                prm[4] = new SqlParameter("@Todate", DBNull.Value);
            }


            prm[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[5].Direction = ParameterDirection.Output;
            prm[6] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[6].Direction = ParameterDirection.Output;
            prm[7] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            prm[7].Direction = ParameterDirection.Output;
            prm[8] = new SqlParameter("@PageSize", PageSize);
            prm[9] = new SqlParameter("@PageIndex", PageIndex);
            prm[10] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
            prm[11] = new SqlParameter("@SalesChannelRetailerID", SalesChannelRetailerID);
            prm[12] = new SqlParameter("@CompanyId", CompanyId);
            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetPaymentDetailReport", CommandType.StoredProcedure, prm);
            ErrorMessage = Convert.ToString(prm[6].Value);
            TotalRecords = Convert.ToInt32(prm[7].Value);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataSet GetReportTaskData()
    {
        try
        {

            SqlParameter[] prm = new SqlParameter[14];
            prm[0] = new SqlParameter("@UserId", UserId);
            prm[1] = new SqlParameter("@EntityTypeId", EntityTypeId);
            prm[2] = new SqlParameter("@EntitytypeUserId", EntitytypeUserId);
            if (FromDate.Year >= 1900)
            {
                prm[3] = new SqlParameter("@FromDate", FromDate);
            }
            else
            {
                prm[3] = new SqlParameter("@FromDate", DBNull.Value);
            }
            if (Todate.Year >= 1900)
            {
                prm[4] = new SqlParameter("@Todate", Todate);
            }
            else
            {
                prm[4] = new SqlParameter("@Todate", DBNull.Value);
            }


            prm[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[5].Direction = ParameterDirection.Output;
            prm[6] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[6].Direction = ParameterDirection.Output;
            prm[7] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            prm[7].Direction = ParameterDirection.Output;
            prm[8] = new SqlParameter("@PageSize", PageSize);
            prm[9] = new SqlParameter("@PageIndex", PageIndex);
            prm[10] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
            prm[11] = new SqlParameter("@SalesChannelRetailerID", SalesChannelRetailerID);
            prm[12] = new SqlParameter("@CompanyId", CompanyId);
            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetTaskDetailReport", CommandType.StoredProcedure, prm);
            ErrorMessage = Convert.ToString(prm[6].Value);
            TotalRecords = Convert.ToInt32(prm[7].Value);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }



    public DataSet GetReportAttendanceData()
    {
        try
        {

            SqlParameter[] prm = new SqlParameter[11];
            prm[0] = new SqlParameter("@UserId", UserId);
            prm[1] = new SqlParameter("@EntityTypeId", EntityTypeId);
            prm[2] = new SqlParameter("@EntitytypeUserId", EntitytypeUserId);
            if (FromDate.Year >= 1900)
            {
                prm[3] = new SqlParameter("@FromDate", FromDate);
            }
            else
            {
                prm[3] = new SqlParameter("@FromDate", DBNull.Value);
            }
            if (Todate.Year >= 1900)
            {
                prm[4] = new SqlParameter("@Todate", Todate);
            }
            else
            {
                prm[4] = new SqlParameter("@Todate", DBNull.Value);
            }


            prm[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[5].Direction = ParameterDirection.Output;
            prm[6] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[6].Direction = ParameterDirection.Output;
            prm[7] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            prm[7].Direction = ParameterDirection.Output;
            prm[8] = new SqlParameter("@PageSize", PageSize);
            prm[9] = new SqlParameter("@PageIndex", PageIndex);
            prm[10] = new SqlParameter("@CompanyId", CompanyId);

            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetAttandanceDetailReport", CommandType.StoredProcedure, prm);
            ErrorMessage = Convert.ToString(prm[6].Value);
            TotalRecords = Convert.ToInt32(prm[7].Value);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataSet GetReportUserLogDetailData()
    {
        try
        {

            SqlParameter[] prm = new SqlParameter[11];
            prm[0] = new SqlParameter("@UserId", UserId);
            prm[1] = new SqlParameter("@EntityTypeId", EntityTypeId);
            prm[2] = new SqlParameter("@EntitytypeUserId", EntitytypeUserId);
            if (FromDate.Year >= 1900)
            {
                prm[3] = new SqlParameter("@FromDate", FromDate);
            }
            else
            {
                prm[3] = new SqlParameter("@FromDate", DBNull.Value);
            }
            if (Todate.Year >= 1900)
            {
                prm[4] = new SqlParameter("@Todate", Todate);
            }
            else
            {
                prm[4] = new SqlParameter("@Todate", DBNull.Value);
            }


            prm[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[5].Direction = ParameterDirection.Output;
            prm[6] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[6].Direction = ParameterDirection.Output;
            prm[7] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            prm[7].Direction = ParameterDirection.Output;
            prm[8] = new SqlParameter("@PageSize", PageSize);
            prm[9] = new SqlParameter("@PageIndex", PageIndex);
            prm[10] = new SqlParameter("@CompanyId", CompanyId);

            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetUserLogDetailReport", CommandType.StoredProcedure, prm);
            ErrorMessage = Convert.ToString(prm[6].Value);
            TotalRecords = Convert.ToInt32(prm[7].Value);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /*#CC01 Added End*/

    public DataSet GetReportONmapvisitData()
    {
        try
        {

            SqlParameter[] prm = new SqlParameter[11];
            prm[0] = new SqlParameter("@UserId", UserId);
            prm[1] = new SqlParameter("@EntityTypeId", EntityTypeId);
            prm[2] = new SqlParameter("@EntitytypeUserId", EntitytypeUserId);
            if (FromDate.Year >= 1900)
            {
                prm[3] = new SqlParameter("@FromDate", FromDate);
            }
            else
            {
                prm[3] = new SqlParameter("@FromDate", DBNull.Value);
            }
            if (Todate.Year >= 1900)
            {
                prm[4] = new SqlParameter("@Todate", Todate);
            }
            else
            {
                prm[4] = new SqlParameter("@Todate", DBNull.Value);
            }
            prm[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[5].Direction = ParameterDirection.Output;
            prm[6] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[6].Direction = ParameterDirection.Output;
            prm[7] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            prm[7].Direction = ParameterDirection.Output;
            prm[8] = new SqlParameter("@InTime", InTime);
            prm[9] = new SqlParameter("@OutTime", OutTime);
            prm[10] = new SqlParameter("@CompanyId", CompanyId);
            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetAppvisitongoogleMapReport", CommandType.StoredProcedure, prm);
            ErrorMessage = Convert.ToString(prm[6].Value);
            TotalRecords = Convert.ToInt32(prm[7].Value);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataSet GetReportVrpAttendanceData()
    {
        try
        {
            SqlParameter[] prm = new SqlParameter[11];
            prm[0] = new SqlParameter("@UserId", UserId);
            prm[1] = new SqlParameter("@EntityTypeId", EntityTypeId);
            prm[2] = new SqlParameter("@EntitytypeUserId", EntitytypeUserId);
            if (FromDate.Year >= 1900)
            {
                prm[3] = new SqlParameter("@FromDate", FromDate);
            }
            else
            {
                prm[3] = new SqlParameter("@FromDate", DBNull.Value);
            }
            if (Todate.Year >= 1900)
            {
                prm[4] = new SqlParameter("@Todate", Todate);
            }
            else
            {
                prm[4] = new SqlParameter("@Todate", DBNull.Value);
            }
            prm[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[5].Direction = ParameterDirection.Output;
            prm[6] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[6].Direction = ParameterDirection.Output;
            prm[7] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            prm[7].Direction = ParameterDirection.Output;
            prm[8] = new SqlParameter("@PageSize", PageSize);
            prm[9] = new SqlParameter("@PageIndex", PageIndex);
            prm[10] = new SqlParameter("@CompanyId", CompanyId);
            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetAttandanceDetailReportForVRP", CommandType.StoredProcedure, prm);
            ErrorMessage = Convert.ToString(prm[6].Value);
            TotalRecords = Convert.ToInt32(prm[7].Value);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet GetReportTravelData()
    {
        try
        {

            SqlParameter[] prm = new SqlParameter[13];
            prm[0] = new SqlParameter("@UserId", UserId);
            prm[1] = new SqlParameter("@EntityTypeId", EntityTypeId);
            prm[2] = new SqlParameter("@EntitytypeUserId", EntitytypeUserId);
            if (FromDate.Year >= 1900)
            {
                prm[3] = new SqlParameter("@FromDate", FromDate);
            }
            else
            {
                prm[3] = new SqlParameter("@FromDate", DBNull.Value);
            }
            if (Todate.Year >= 1900)
            {
                prm[4] = new SqlParameter("@Todate", Todate);
            }
            else
            {
                prm[4] = new SqlParameter("@Todate", DBNull.Value);
            }


            prm[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[5].Direction = ParameterDirection.Output;
            prm[6] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[6].Direction = ParameterDirection.Output;
            prm[7] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            prm[7].Direction = ParameterDirection.Output;
            prm[8] = new SqlParameter("@PageSize", PageSize);
            prm[9] = new SqlParameter("@PageIndex", PageIndex);
            prm[10] = new SqlParameter("@CompanyId", CompanyId);
            prm[11] = new SqlParameter("@BrandId", BrandId);
            prm[12] = new SqlParameter("@ProductCategoryId", ProductCategoryId);
            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcTravelClaimDaySummary", CommandType.StoredProcedure, prm);
            ErrorMessage = Convert.ToString(prm[6].Value);
            TotalRecords = Convert.ToInt32(prm[7].Value);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataSet GetReportDealerVisitAnalysis()
    {
        try
        {

            SqlParameter[] prm = new SqlParameter[11];
            prm[0] = new SqlParameter("@UserId", UserId);
            prm[1] = new SqlParameter("@EntityTypeId", EntityTypeId);
            prm[2] = new SqlParameter("@EntitytypeUserId", EntitytypeUserId);
            if (FromDate.Year >= 1900)
            {
                prm[3] = new SqlParameter("@FromDate", FromDate);
            }
            else
            {
                prm[3] = new SqlParameter("@FromDate", DBNull.Value);
            }
            if (Todate.Year >= 1900)
            {
                prm[4] = new SqlParameter("@Todate", Todate);
            }
            else
            {
                prm[4] = new SqlParameter("@Todate", DBNull.Value);
            }


            prm[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[5].Direction = ParameterDirection.Output;
            prm[6] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[6].Direction = ParameterDirection.Output;
            prm[7] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            prm[7].Direction = ParameterDirection.Output;
            prm[8] = new SqlParameter("@PageSize", PageSize);
            prm[9] = new SqlParameter("@PageIndex", PageIndex);
            prm[10] = new SqlParameter("@CompanyId", CompanyId);

            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcDailyVisitAnalysis", CommandType.StoredProcedure, prm);
            ErrorMessage = Convert.ToString(prm[6].Value);
            TotalRecords = Convert.ToInt32(prm[7].Value);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public string DetailDrillType { get; set; }
    
    public DataSet GetReportDealerVisitAnalysisDrill()
    {
        try
        {

            SqlParameter[] prm = new SqlParameter[14];
            prm[0] = new SqlParameter("@UserId", UserId);
            prm[1] = new SqlParameter("@EntityTypeId", EntityTypeId);
            prm[2] = new SqlParameter("@EntitytypeUserId", EntitytypeUserId);
            if (FromDate.Year >= 1900)
            {
                prm[3] = new SqlParameter("@FromDate", FromDate);
            }
            else
            {
                prm[3] = new SqlParameter("@FromDate", DBNull.Value);
            }
            if (Todate.Year >= 1900)
            {
                prm[4] = new SqlParameter("@Todate", Todate);
            }
            else
            {
                prm[4] = new SqlParameter("@Todate", DBNull.Value);
            }


            prm[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[5].Direction = ParameterDirection.Output;
            prm[6] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[6].Direction = ParameterDirection.Output;
            prm[7] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            prm[7].Direction = ParameterDirection.Output;
            prm[8] = new SqlParameter("@PageSize", PageSize);
            prm[9] = new SqlParameter("@PageIndex", PageIndex);
            prm[10] = new SqlParameter("@CompanyId", CompanyId);
            prm[11] = new SqlParameter("@DetailType", DetailDrillType);

            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcDailyVisitAnalysisDRillData", CommandType.StoredProcedure, prm);
            ErrorMessage = Convert.ToString(prm[6].Value);
            TotalRecords = Convert.ToInt32(prm[7].Value);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet GetReportLeaveData()
    {
        try
        {

            SqlParameter[] prm = new SqlParameter[14];
            prm[0] = new SqlParameter("@UserId", UserId);
            prm[1] = new SqlParameter("@EntityTypeId", EntityTypeId);
            prm[2] = new SqlParameter("@EntitytypeUserId", EntitytypeUserId);
            if (FromDate.Year >= 1900)
            {
                prm[3] = new SqlParameter("@FromDate", FromDate);
            }
            else
            {
                prm[3] = new SqlParameter("@FromDate", DBNull.Value);
            }
            if (Todate.Year >= 1900)
            {
                prm[4] = new SqlParameter("@Todate", Todate);
            }
            else
            {
                prm[4] = new SqlParameter("@Todate", DBNull.Value);
            }
            prm[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[5].Direction = ParameterDirection.Output;
            prm[6] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[6].Direction = ParameterDirection.Output;
            prm[7] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            prm[7].Direction = ParameterDirection.Output;
            prm[8] = new SqlParameter("@PageSize", PageSize);
            prm[9] = new SqlParameter("@PageIndex", PageIndex);
            prm[10] = new SqlParameter("@LeaveStatus", LeaveStatus);
            prm[11] = new SqlParameter("@CompanyId", CompanyId);
            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetLeaveDetailSaasReport", CommandType.StoredProcedure, prm);
            ErrorMessage = Convert.ToString(prm[6].Value);
            TotalRecords = Convert.ToInt32(prm[7].Value);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
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

    ~ClsPaymentReport()
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
