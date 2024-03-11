﻿#region Copyright(c) 2017 Zed-Axis Technologies All rights are reserved
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
* Created On: 04-June-2020
 * Description: This is a Expense Data get file.
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
/// Summary description for ClsExpense
/// </summary>
public class ClsExpense : IDisposable
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
    private String _SessionId;

    #endregion
    #region Public Varible
   public DataTable dtSavelist;
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
    public Int32 ExpenseStatus { get; set; }
    public Int16 OutParam { get; set; }
    public Int16 CallingFrom { get; set; }
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
    public string SessionId
    {
        get
        {
            return _SessionId;
        }
        set
        {
            _SessionId = value;
        }
    }
    public Int64 ExpenseId { get; set; }
    #endregion
	public ClsExpense()
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
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetEntityTypeForExpenseReport", CommandType.StoredProcedure, SqlParam);
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
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetEntityTypeNameForExpenseReport", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetApprovalStatus()
    {

        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@CompanyId", CompanyId);
            SqlParam[1] = new SqlParameter("@UserId", UserId);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetExpenseStatus", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet GetReportExpenseData()
    {
        try
        {

            SqlParameter[] prm = new SqlParameter[12];
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
            prm[11] = new SqlParameter("@ExpenseStatus", ExpenseStatus);
            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetExpenseDetailReport", CommandType.StoredProcedure, prm);
            ErrorMessage = Convert.ToString(prm[6].Value);
            TotalRecords = Convert.ToInt32(prm[7].Value);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet SaveApproveReject()
    {
        try
        {
            SqlParameter[] prm = new SqlParameter[13];
            prm[0] = new SqlParameter("@ExpenseStatus", ExpenseStatus);
            prm[1] = new SqlParameter("@UserId", UserId);
            prm[2] = new SqlParameter("@CompanyId", CompanyId);
            prm[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[3].Direction = ParameterDirection.Output;
            prm[4] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[4].Direction = ParameterDirection.Output;
            prm[5] = new SqlParameter("@TVP_ExpenseDetail", dtSavelist);
            prm[6] = new SqlParameter("@CallingFrom", CallingFrom);
            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcApproveRejectExpense", CommandType.StoredProcedure, prm);
            OutParam = Convert.ToInt16(prm[3].Value);
            ErrorMessage = Convert.ToString(prm[4].Value);
            return dsResult;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet GetExpenseReportData()
    {
        try
        {

            SqlParameter[] prm = new SqlParameter[12];
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
            prm[11] = new SqlParameter("@ExpenseStatus", ExpenseStatus);
            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetAllExpenseDetailReport", CommandType.StoredProcedure, prm);
            ErrorMessage = Convert.ToString(prm[6].Value);
            TotalRecords = Convert.ToInt32(prm[7].Value);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetExpenswViewImageInfo()
    {
        try
        {
            DataTable dtRetailerImage;
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@ExpenseId", ExpenseId);
            SqlParam[1] = new SqlParameter("@UserID", UserId);
            SqlParam[2] = new SqlParameter("@CompanyId", CompanyId);
            dtRetailerImage = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetExpenseImagePath", CommandType.StoredProcedure, SqlParam);
            return dtRetailerImage;
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

    ~ClsExpense()
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