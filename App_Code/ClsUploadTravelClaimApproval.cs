#region Copyright and page info
/*============================================================================================================================================
Copyright	: Zed-Axis Technologies, 2020
Author		: Shashikant Singh
Create date	: 23-Jun-2020
Description	: This page is used for on Upload travel claim approval. .

============================================================================================================================================
Change Log:
dd-MMM-yy, Name , #CCxx - Description

--------------------------------------------------------------------------------------------------------------------------------------------
*/
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;

public class ClsUploadTravelClaimApproval : IDisposable
{
    public static string ConStr = ConfigurationManager.ConnectionStrings["AppConString"].ConnectionString;
    #region Constructors
    public ClsUploadTravelClaimApproval()
    {

    }
    #endregion
    #region Dispose
    private bool IsDisposed = false;

    //Call Dispose to free resources explicitly
    public void Dispose()
    {
        //Pass true in dispose method to clean managed resources too and say GC to skip finalize 
        // in next line.
        Dispose(true);
        //If dispose is called already then say GC to skip finalize on this instance.
        GC.SuppressFinalize(this);
    }

    ~ClsUploadTravelClaimApproval()
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

    #region Private Class Variables
    private int _UserId;
    private Int32 _intPageIndex;
    private Int32 _intPageSize;
    private Int32 _intTotalRecords;
    private string _SessionId;
    private string _OutError;
    private int _OutParam;
    private DateTime _ToDate;
    private DateTime _FromDate;
    private int _EntityId;
    private int _EntityTypeId;
    private Int16 _Status;
    private Int16 _ApprovalLevel;
    private int _EngineerUserDetailId;
    private int _SelectedEntityID;
    private int _RoleId;
    private int intCompanyID;

    #endregion
    #region Public Properties

    public int CompanyID
    {
        get
        {
            return intCompanyID;
        }
        set
        {
            intCompanyID = value;
        }
    }
    public int userId
    {
        get
        {
            return _UserId;
        }
        set
        {
            _UserId = value;
        }
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

    public string OutError
    {
        get
        {
            return _OutError;
        }
        set
        {
            _OutError = value;
        }
    }
    public int OutParam
    {
        get
        {
            return _OutParam;
        }
        set
        {
            _OutParam = value;
        }
    }
    public DateTime ToDate
    {
        get
        {
            return _ToDate;
        }
        set
        {
            _ToDate = value;
        }
    }

    public DateTime FromDate
    {
        get
        {
            return _FromDate;
        }
        set
        {
            _FromDate = value;
        }
    }
    public int EntityTypeId
    {
        get
        {
            return _EntityTypeId;
        }
        set
        {
            _EntityTypeId = value;
        }
    }
    public int EntityId
    {
        get
        {
            return _EntityId;
        }
        set
        {
            _EntityId = value;
        }
    }

    public Int16 Status
    {
        get
        {
            return _Status;
        }
        set
        {
            _Status = value;
        }
    }
    public Int16 ApprovalLevel
    {
        get
        {
            return _ApprovalLevel;
        }
        set
        {
            _ApprovalLevel = value;
        }
    }
    public int EngineerUserDetailId
    {
        get
        {
            return _EngineerUserDetailId;
        }
        set
        {
            _EngineerUserDetailId = value;
        }
    }
    public int SelectedEntityID
    {
        get
        {
            return _SelectedEntityID;
        }
        set
        {
            _SelectedEntityID = value;
        }
    }
    public int RoleId
    {
        get
        {
            return _RoleId;
        }
        set
        {
            _RoleId = value;
        }
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
    public Int32 TotalRecords
    {
        get
        {
            return _intTotalRecords;
        }
        private set
        {
            _intTotalRecords = value;
        }
    }
    public DataTable TravelClaim { get; set; }

    #endregion

    public DataSet SelectAll()
    {
        DataSet dsResult = new DataSet();
        SqlParameter[] objSqlParam = new SqlParameter[16];

        if (FromDate.Year >= 1900)
        {
            objSqlParam[0] = new SqlParameter("@FromDate", FromDate);
        }
        else
        {
            objSqlParam[0] = new SqlParameter("@FromDate", DBNull.Value);
        }
        if (ToDate.Year >= 1900)
        {
            objSqlParam[1] = new SqlParameter("@ToDate", ToDate);
        }
        else
        {
            objSqlParam[1] = new SqlParameter("@ToDate", DBNull.Value);
        }
        objSqlParam[2] = new SqlParameter("@UserId", userId);
        objSqlParam[3] = new SqlParameter("@EntityId", EntityId);
        objSqlParam[4] = new SqlParameter("@EntityTypeId", EntityTypeId);
        objSqlParam[5] = new SqlParameter("@SelectedEntityID", SelectedEntityID);
        objSqlParam[6] = new SqlParameter("@EngineerUserDetailId", EngineerUserDetailId);
        objSqlParam[7] = new SqlParameter("@ApprovalLeval", ApprovalLevel);
        objSqlParam[8] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[8].Direction = ParameterDirection.Output;
        objSqlParam[9] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 2000);
        objSqlParam[9].Direction = ParameterDirection.Output;
        objSqlParam[10] = new SqlParameter("@RoleId", RoleId);
        objSqlParam[11] = new SqlParameter("@ApprovalStatus", Status);
        objSqlParam[12] = new SqlParameter("@PageIndex", PageIndex);
        objSqlParam[13] = new SqlParameter("@PageSize", PageSize);
        objSqlParam[14] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
        objSqlParam[14].Direction = ParameterDirection.Output;
        objSqlParam[15] = new SqlParameter("@CompanyId", CompanyID);

        dsResult = SqlHelper.ExecuteDataset(ConStr, CommandType.StoredProcedure, "prcGetDatForTravelClaimApproval", objSqlParam);
        if (dsResult != null && dsResult.Tables.Count > 0)
        {
            TotalRecords = Convert.ToInt32(objSqlParam[14].Value);
        }

        return dsResult;
    }
    public DataSet Insert()
    {
        DataSet dsResult = new DataSet();
        SqlParameter[] objSqlParam = new SqlParameter[7];
        objSqlParam[0] = new SqlParameter("@UserId", userId);
        objSqlParam[1] = new SqlParameter("@tvpTravelClaimApproval", SqlDbType.Structured);
        objSqlParam[1].Value = TravelClaim;
        objSqlParam[2] = new SqlParameter("@ApprovalLevel", ApprovalLevel);
        objSqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[3].Direction = ParameterDirection.Output;
        objSqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[4].Direction = ParameterDirection.Output;
        objSqlParam[5] = new SqlParameter("@RoleId", RoleId);
        objSqlParam[6] = new SqlParameter("@CompanyId", CompanyID);
        dsResult = SqlHelper.ExecuteDataset(ConStr, CommandType.StoredProcedure, "prcBulkTravelClaimApproval", objSqlParam);
        OutParam = Convert.ToInt16(objSqlParam[3].Value);
        OutError = Convert.ToString(objSqlParam[4].Value);

        return dsResult;
    }


    public DataSet ViewAllTravelClaim()
    {
        DataSet dsResult = new DataSet();
        SqlParameter[] objSqlParam = new SqlParameter[16];

        if (FromDate.Year >= 1900)
        {
            objSqlParam[0] = new SqlParameter("@FromDate", FromDate);
        }
        else
        {
            objSqlParam[0] = new SqlParameter("@FromDate", DBNull.Value);
        }
        if (ToDate.Year >= 1900)
        {
            objSqlParam[1] = new SqlParameter("@ToDate", ToDate);
        }
        else
        {
            objSqlParam[1] = new SqlParameter("@ToDate", DBNull.Value);
        }
        objSqlParam[2] = new SqlParameter("@UserId", userId);
        objSqlParam[3] = new SqlParameter("@EntityId", EntityId);
        objSqlParam[4] = new SqlParameter("@EntityTypeId", EntityTypeId);
        objSqlParam[5] = new SqlParameter("@SelectedEntityID", SelectedEntityID);
        objSqlParam[6] = new SqlParameter("@EngineerUserDetailId", EngineerUserDetailId);
        objSqlParam[7] = new SqlParameter("@ApprovalLeval", ApprovalLevel);
        objSqlParam[8] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[8].Direction = ParameterDirection.Output;
        objSqlParam[9] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 2000);
        objSqlParam[9].Direction = ParameterDirection.Output;
        objSqlParam[10] = new SqlParameter("@RoleId", RoleId);
        objSqlParam[11] = new SqlParameter("@ApprovalStatus", Status);
        objSqlParam[12] = new SqlParameter("@PageIndex", PageIndex);
        objSqlParam[13] = new SqlParameter("@PageSize", PageSize);
        objSqlParam[14] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
        objSqlParam[14].Direction = ParameterDirection.Output;
        objSqlParam[15] = new SqlParameter("@CompanyId", CompanyID);

        dsResult = SqlHelper.ExecuteDataset(ConStr, CommandType.StoredProcedure, "prcReportTravelClaimDetail", objSqlParam);
        if (dsResult != null && dsResult.Tables.Count > 0)
        {
            TotalRecords = Convert.ToInt32(objSqlParam[14].Value);
        }

        return dsResult;
    }
}
