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
* Created On: 01-Aug-2017 
 * Description: This is a copy of Salesdata from DataAccess.
* ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 ====================================================================================================
*/
#endregion
#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Xml;
#endregion

/// <summary>
/// Summary description for AccountStatementRequest
/// </summary>
public class AccountStatementRequest : IDisposable
{
    #region Private Varible
    private DateTime? _FromDate;
    private DateTime? _Todate;
    SqlParameter[] SqlParam;
    private Int64 _userid;
    private Int32 _intPageIndex;
    private Int32 _intPageSize;
    private int _AccountStatementReqId;
    #endregion

    #region Public Varible
    public DateTime? FromDate
    {
        get { return _FromDate; }
        set { _FromDate = value; }
    }
    public DateTime? Todate
    {
        get { return _Todate; }
        set { _Todate = value; }
    }
    public Int64 UserId
    {
        get { return _userid; }
        set { _userid = value; }
    }
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
    public int AccountStatementReqId
    {
        get
        {

            return _AccountStatementReqId;
        }
        set
        {
            _AccountStatementReqId = value;
        }
    }
    #endregion
    public AccountStatementRequest()
    {

    }
    public Int16 SaveAccountStatementRequest()
    {

        try
        {
            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@UserID", UserId);
            SqlParam[1] = new SqlParameter("@FromDate", FromDate);

            SqlParam[2] = new SqlParameter("@ToDate", Todate);
            SqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.NVarChar);

            SqlParam[4] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 1);
            SqlParam[4].Direction = ParameterDirection.Output;
            Int32 IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("prcAccountStatementRequest", SqlParam);
            return Convert.ToInt16(SqlParam[4].Value);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable SelectAllAccountRequestStatement()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[3];
        objSqlParam[0] = new SqlParameter("@DateFrom", FromDate);
        objSqlParam[1] = new SqlParameter("@DateTo", Todate);
        objSqlParam[2] = new SqlParameter("@userId", UserId);


        dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcAccountStatementRequest_Search", CommandType.StoredProcedure, objSqlParam);

        return dtResult;
    }
    public DataSet GetAccountRequestStatementPrintDetails()
    {

        DataSet ds = new DataSet();
        SqlParameter[] objSqlParam = new SqlParameter[3];
        objSqlParam[0] = new SqlParameter("@AccountStatementReqId", AccountStatementReqId);
        objSqlParam[1] = new SqlParameter("@OutParam", SqlDbType.BigInt, 1);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@outError", SqlDbType.NVarChar);

        ds = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("PrcPrintAccountStatement", CommandType.StoredProcedure, objSqlParam);
        return ds;
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

    ~AccountStatementRequest()
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