using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Data.SqlClient;
using System.Xml;

/// <summary>
/// Summary description for clsRegionMaster
/// </summary>
public class clsRegionMaster : IDisposable
{
	public clsRegionMaster()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    private int _intZoneID;
    private int _intCountryID;
    private string _strZoneName;
    private int? _intDisplayOrder;
    private int _intNullOrderReversal;
    private string _strRemarks;
    private int _intCreatedBy;
    private DateTime _dtCreatedOn;
    private int _intModifiedBy;
    private DateTime _dtModifiedOn;
    private short _shtActive;

    private string _strError;
    private Int32 _intPageIndex;
    private Int32 _intPageSize;
    private Int32 _intTotalRecords;
    private Int16 _intMode;
    private string _RegionName;
    private string _RegionCode;
    private Int64 _RegionId;
    #region Public Properties
    public int ZoneID
    {
        get
        {
            return _intZoneID;
        }
        set
        {
            _intZoneID = value;
        }
    }
    public int CountryID
    {
        get
        {
            return _intCountryID;
        }
        set
        {
            _intCountryID = value;
        }
    }
    public string ZoneName
    {
        get
        {
            return _strZoneName;
        }
        set
        {
            _strZoneName = value;
        }
    }
    public int? DisplayOrder
    {
        get
        {
            return _intDisplayOrder;
        }
        set
        {
            _intDisplayOrder = value;
        }
    }
    public int NullOrderReversal
    {
        get
        {
            return _intNullOrderReversal;
        }
        set
        {
            _intNullOrderReversal = value;
        }
    }
    public string Remarks
    {
        get
        {
            return _strRemarks;
        }
        set
        {
            _strRemarks = value;
        }
    }
    public Int64 RegionId
    {
        get
        {
            return _RegionId;
        }
        set
        {
            _RegionId = value;
        }
    }
    public string RegionName
    {
        get
        {
            return _RegionName;
        }
        set
        {
            _RegionName = value;
        }
    }
    public string RegionCode
    {
        get
        {
            return _RegionCode;
        }
        set
        {
            _RegionCode = value;
        }
    }
    public Int16 Mode
    {
        get
        {
            return _intMode;
        }
        set
        {
            _intMode = value;
        }
    }
    public int CreatedBy
    {
        get
        {
            return _intCreatedBy;
        }
        set
        {
            _intCreatedBy = value;
        }
    }
    public DateTime CreatedOn
    {
        get
        {
            return _dtCreatedOn;
        }
        set
        {
            _dtCreatedOn = value;
        }
    }
    public int ModifiedBy
    {
        get
        {
            return _intModifiedBy;
        }
        set
        {
            _intModifiedBy = value;
        }
    }
    public DateTime ModifiedOn
    {
        get
        {
            return _dtModifiedOn;
        }
        set
        {
            _dtModifiedOn = value;
        }
    }
    public short Active
    {
        get
        {
            return _shtActive;
        }
        set
        {
            _shtActive = value;
        }
    }

    public string Error
    {
        get
        {
            return _strError;
        }
        private set
        {
            _strError = value;
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
    #endregion
    public Int16 Save()
    {
        Int16 result = 1;
        SqlParameter[] objSqlParam = new SqlParameter[10];
        objSqlParam[0] = new SqlParameter("@countryid", CountryID);
        objSqlParam[1] = new SqlParameter("@zoneId", ZoneID);
        objSqlParam[2] = new SqlParameter("@displayorder", DisplayOrder);
        objSqlParam[3] = new SqlParameter("@remarks", Remarks);
        objSqlParam[4] = new SqlParameter("@CreatedBy", CreatedBy);
        objSqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[5].Direction = ParameterDirection.Output;
        objSqlParam[6] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[6].Direction = ParameterDirection.Output;
        objSqlParam[7] = new SqlParameter("@active", Active);
        objSqlParam[8] = new SqlParameter("@RegionName", RegionName);
        objSqlParam[9] = new SqlParameter("@RegionCode", RegionCode);
        int resultdata = DataAccess.DataAccess.Instance.DBInsertCommand("prcRegionMaster_Insert", objSqlParam);
        result = Convert.ToInt16(objSqlParam[5].Value);
        Error = Convert.ToString(objSqlParam[6].Value);
        if (Error != string.Empty)
        {
            throw new ArgumentException(Error);
        }

        return result;
    }
    public Int16 Update()
    {
        Int16 result = 1;
        SqlParameter[] objSqlParam = new SqlParameter[10];
        objSqlParam[0] = new SqlParameter("@RegionId", RegionId);
        objSqlParam[1] = new SqlParameter("@countryid", CountryID);
        objSqlParam[2] = new SqlParameter("@RegionName", RegionName);
        objSqlParam[3] = new SqlParameter("@displayorder", DisplayOrder);
        objSqlParam[4] = new SqlParameter("@remarks", Remarks);
        objSqlParam[5] = new SqlParameter("@modifiedby", ModifiedBy);
        objSqlParam[6] = new SqlParameter("@Active", Active);
        objSqlParam[7] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[7].Direction = ParameterDirection.Output;
        objSqlParam[8] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[8].Direction = ParameterDirection.Output;
        objSqlParam[9] = new SqlParameter("@RegionCode", RegionCode);
        int resultupdatedata = DataAccess.DataAccess.Instance.DBInsertCommand("prcRegionMaster_Update", objSqlParam);
        result = Convert.ToInt16(objSqlParam[7].Value);
        Error = Convert.ToString(objSqlParam[8].Value);
        if (Error != string.Empty)
        {
            throw new ArgumentException(Error);
        }

        return result;
    }
    public DataTable SelectAll()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[9];
        objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
        objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
        objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
        objSqlParam[2].Direction = ParameterDirection.Output;
        objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[3].Direction = ParameterDirection.Output;
        objSqlParam[4] = new SqlParameter("@RegionID", ZoneID);
        objSqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[5].Direction = ParameterDirection.Output;
        objSqlParam[6] = new SqlParameter("@RegionName", ZoneName);
        objSqlParam[7] = new SqlParameter("@SrchCountryID", CountryID);
        objSqlParam[8] = new SqlParameter("@active", Active);
        DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcRegionMaster_Select", CommandType.StoredProcedure, objSqlParam);
        if (dsResult != null && dsResult.Tables.Count > 0)
            dtResult = dsResult.Tables[0];
        TotalRecords = Convert.ToInt32(objSqlParam[2].Value);
        Error = Convert.ToString(objSqlParam[3].Value);
        if (Error != string.Empty)
        {
            throw new ArgumentException(Error);
        }

        return dtResult;
    }
    public DataTable SelectAllExportinExcel()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[9];
        objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
        objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
        objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
        objSqlParam[2].Direction = ParameterDirection.Output;
        objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[3].Direction = ParameterDirection.Output;
        objSqlParam[4] = new SqlParameter("@RegionId", RegionId);
        objSqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[5].Direction = ParameterDirection.Output;
        objSqlParam[6] = new SqlParameter("@RegionName", RegionName);
        objSqlParam[7] = new SqlParameter("@SrchCountryID", CountryID);
        objSqlParam[8] = new SqlParameter("@active", Active);
        DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcRegionMaster_SelectExportinExcel", CommandType.StoredProcedure, objSqlParam);
        if (dsResult != null && dsResult.Tables.Count > 0)
            dtResult = dsResult.Tables[0];
        TotalRecords = Convert.ToInt32(objSqlParam[2].Value);
        Error = Convert.ToString(objSqlParam[3].Value);
        if (Error != string.Empty)
        {
            throw new ArgumentException(Error);
        }

        return dtResult;
    }
    public Int16 ToggleActivation()
    {
        Int16 result = 0;
        SqlParameter[] objSqlParam = new SqlParameter[4];
        objSqlParam[0] = new SqlParameter("@RegionId", RegionId);
        objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[2].Direction = ParameterDirection.Output;
        objSqlParam[3] = new SqlParameter("@modifiedby", ModifiedBy);
        int updatedat = DataAccess.DataAccess.Instance.DBInsertCommand("prcRegionMaster_TActive", objSqlParam);
        result = (Convert.ToInt16(objSqlParam[1].Value));
        Error = Convert.ToString(objSqlParam[2].Value);
        if (Error != string.Empty)
        {
            throw new ArgumentException(Error);
        }
        return result;
    }
    public DataSet SelectForEdit()
    {
        SqlParameter[] objSqlParam = new SqlParameter[3];
        objSqlParam[0] = new SqlParameter("@RegionID", RegionId);
        objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[2].Direction = ParameterDirection.Output;
        DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcRegionMaster_SelectForEdit", CommandType.StoredProcedure, objSqlParam);
        Error = Convert.ToString(objSqlParam[2].Value);
        if (Error != string.Empty)
        {
            throw new ArgumentException(Error);
        }

        return dsResult;
    }
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

        ~clsRegionMaster()
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