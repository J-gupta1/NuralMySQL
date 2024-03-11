#region Copyright and page info
/*============================================================================================================================================
Copyright	: Zed-Axis Technologies, 2020
Author		: Shashikant Singh
Create date	: 15-Jun-2020
Description	: 
============================================================================================================================================
Change Log:
dd-MMM-yy, Name , #CCxx - Description
--------------------------------------------------------------------------------------------------------------------------------------------
*/
#endregion

#region Namespace
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

#endregion


    public class clsTravelRate : IDisposable
    {

        #region Private Class Variables

        public static string ConnectionString = ConfigurationManager.ConnectionStrings["AppConString"].ConnectionString;

        private int _intTravelRateID;
        private int _intRoleid;
        private int _intEntityTypeID;
        private short _shtTransportTypeMsterId;
        private decimal _dcmTravelRateAmount;
        private DateTime _dtValidFrom;
        private string _dtvalidFrom_Search;
        private DateTime _dtValidTill;
        private DateTime _dtCreatedOn;
        private int _intCreatedBy;
        private DateTime _dtModifiedOn;
        private int _intModifiedBy;
        private int _IntCurrencyId;                                      

        private string _strError;
        private Int32 _intPageIndex;
        private Int32 _intPageSize;
        private Int32 _intTotalRecords;
        private int? _intTransportType;


        #endregion

        #region Public Properties

        public Int32 CompanyId { get; set; }

        public int TravelRateID
        {
            get
            {
                return _intTravelRateID;
            }
            set
            {
                _intTravelRateID = value;
            }
        }
        public int Roleid
        {
            get
            {
                return _intRoleid;
            }
            set
            {
                _intRoleid = value;
            }
        }
        public int EntityTypeID
        {
            get
            {
                return _intEntityTypeID;
            }
            set
            {
                _intEntityTypeID = value;
            }
        }
        public short TransportTypeMasterId
        {
            get
            {
                return _shtTransportTypeMsterId;
            }
            set
            {
                _shtTransportTypeMsterId = value;
            }
        }
        public decimal TravelRateAmount
        {
            get
            {
                return _dcmTravelRateAmount;
            }
            set
            {
                _dcmTravelRateAmount = value;
            }
        }
        public DateTime ValidFrom
        {
            get
            {
                return _dtValidFrom;
            }
            set
            {
                _dtValidFrom = value;
            }
        }

        public string ValidFrom_Search
        {
            get
            {
                return _dtvalidFrom_Search;
            }
            set
            {
                _dtvalidFrom_Search = value;
            }
        }
        public DateTime ValidTill
        {
            get
            {
                return _dtValidTill;
            }
            set
            {
                _dtValidTill = value;
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
        public int? TransportType
        {
            get
            {
                return _intTransportType;
            }
            set
            {
                _intTransportType = value;
            }
        }
        public int CurrencyId                                    
        {
            get
            {
                return _IntCurrencyId;
            }
            set
            {
                _IntCurrencyId = value;
            }
        }                                                          
        #endregion

        #region Constructors
        public clsTravelRate()
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

        ~clsTravelRate()
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

        #region Public Method Section
        /// <summary>
        /// Save records in database.
        /// </summary>
        /// <results>Int16: 0 if success</results> 
        public Int16 Insert()
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[9];
            objSqlParam[0] = new SqlParameter("@Roleid", Roleid);
            objSqlParam[1] = new SqlParameter("@TransportTypeMasterId", TransportTypeMasterId);
            objSqlParam[2] = new SqlParameter("@TravelRateAmount", TravelRateAmount);
            objSqlParam[3] = new SqlParameter("@ValidFrom", ValidFrom);
            objSqlParam[4] = new SqlParameter("@CreatedBy", CreatedBy);
            objSqlParam[5] = new SqlParameter("@CurrencyId", CurrencyId);
            objSqlParam[6] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[6].Direction = ParameterDirection.Output;
            objSqlParam[7] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[7].Direction = ParameterDirection.Output;
            objSqlParam[8] = new SqlParameter("@CompanyId", CompanyId);

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "prcTravelRate_Insert", objSqlParam);
            result = Convert.ToInt16(objSqlParam[6].Value);
            //TravelRateID = Convert.ToInt32(objSqlParam[10].Value);
            Error = Convert.ToString(objSqlParam[7].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return result;
        }

        /// <summary>
        /// Update records in database.
        /// </summary>
        /// <results>Int16: 0 if success</results> 
        public Int16 Update()
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[11];
            objSqlParam[0] = new SqlParameter("@TravelRateID", TravelRateID);
            objSqlParam[1] = new SqlParameter("@Roleid", Roleid);
            objSqlParam[2] = new SqlParameter("@TransportTypeMasterId", TransportTypeMasterId);
            objSqlParam[3] = new SqlParameter("@TravelRateAmount", TravelRateAmount);
            objSqlParam[4] = new SqlParameter("@ValidFrom", ValidFrom);
            objSqlParam[5] = new SqlParameter("@CurrencyId", CurrencyId);
            objSqlParam[6] = new SqlParameter("@ModifiedBy", ModifiedBy);
            objSqlParam[7] = new SqlParameter("@ModifyOn", ModifiedOn);
            objSqlParam[8] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[8].Direction = ParameterDirection.Output;
            objSqlParam[9] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[9].Direction = ParameterDirection.Output;
            objSqlParam[10] = new SqlParameter("@CompanyId", CompanyId);

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "prcTravelRate_Update", objSqlParam);
            result = Convert.ToInt16(objSqlParam[8].Value);
            Error = Convert.ToString(objSqlParam[9].Value);

            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }
            return result;
        }
        /// <summary>
        /// Delete selected record.
        /// </summary>
        /// <results>bool: True if success</results> 
        public bool Delete()
        {
            bool result = false;
            SqlParameter[] objSqlParam = new SqlParameter[4];
            objSqlParam[0] = new SqlParameter("@TravelRateID", TravelRateID);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@CompanyId", CompanyId);

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "prcTravelRate_Delete", objSqlParam);
            result = (Convert.ToInt16(objSqlParam[1].Value) == 0);
            Error = Convert.ToString(objSqlParam[2].Value);

            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }
            return result;
        }

        /// <summary>
        /// Get All records from database 
        /// send @PageSize as -1 if need all record and @Active=255 
        /// </summary>
        /// <results>DataTable: Collection of records</results> 		
        public DataTable SelectAll()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[9];
            if (ValidFrom_Search == null || ValidFrom_Search == "")
                objSqlParam[0] = new SqlParameter("@ValidFrom_Search", ValidFrom_Search);
            else
                objSqlParam[0] = new SqlParameter("@ValidFrom_Search", Convert.ToDateTime(ValidFrom_Search));

            objSqlParam[1] = new SqlParameter("@TransportTypeMasterId", TransportTypeMasterId);
            objSqlParam[2] = new SqlParameter("@RoleId", Roleid);
            objSqlParam[3] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[4] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[5] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[5].Direction = ParameterDirection.Output;
            objSqlParam[6] = new SqlParameter("@Out_Param", SqlDbType.BigInt, 8);
            objSqlParam[6].Direction = ParameterDirection.Output;
            objSqlParam[7] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[7].Direction = ParameterDirection.Output;
            objSqlParam[8] = new SqlParameter("@CompanyId", CompanyId);

            DataSet dsResult = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "prcTravelRate_Select", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            TotalRecords = Convert.ToInt32(objSqlParam[5].Value);
            Error = Convert.ToString(objSqlParam[7].Value);

            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }
            return dtResult;
        }

        /// <summary>
        /// Get  record(s) from database for selected key
        /// </summary>
        /// <results>DataTable: Collection of records</results> 		
        public DataTable SelectById()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[4];
            objSqlParam[0] = new SqlParameter("@TravelRateID", TravelRateID);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@CompanyId", CompanyId);

            DataSet dsResult = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "prcTravelRate_SelectById", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[2].Value);

            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }
            return dtResult;
        }

        public DataTable SelectUserRole()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[7];
            objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@EntityTypeID", EntityTypeID);
            objSqlParam[5] = new SqlParameter("@Active", 1);
            objSqlParam[6] = new SqlParameter("@CompanyId", CompanyId);

            DataSet dsResult = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "prcUserRoleType_Select", objSqlParam);
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
        public DataTable GetTransportTypeList()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[7];
            objSqlParam[0] = new SqlParameter("@TranportType", TransportType);
            objSqlParam[1] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[2] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[3] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@Out_Param", SqlDbType.BigInt, 8);
            objSqlParam[4].Direction = ParameterDirection.Output;
            objSqlParam[5] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[5].Direction = ParameterDirection.Output;
            objSqlParam[6] = new SqlParameter("@CompanyId", CompanyId);

            DataSet dsResult = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "prcGetTransportList", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            TotalRecords = Convert.ToInt32(objSqlParam[2].Value);
            Error = Convert.ToString(objSqlParam[4].Value);

            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }
            return dtResult;
        }
        /// <summary>
        /// Get single records from database for selected key
        /// </summary>
        /// <results>set record value in properties for direct access</results> 		
        public void Load()
        {
            SqlParameter[] objSqlParam = new SqlParameter[4];
            objSqlParam[0] = new SqlParameter("@TravelRateID", TravelRateID);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@CompanyId", CompanyId);

            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "prcTravelRate_SelectById", objSqlParam);
            while (reader.Read())
            {
                if (reader["Roleid"] != null)
                    Roleid = Convert.ToInt32(reader["Roleid"]);
                if (reader["VehicalType"] != null)
                    TransportTypeMasterId = Convert.ToInt16(reader["TransportTypeMasterId"]);
                if (reader["TravelRateAmount"] != null)
                    TravelRateAmount = Convert.ToDecimal(reader["TravelRateAmount"]);
                if (reader["ValidFrom"] != null)
                    ValidFrom = Convert.ToDateTime(reader["ValidFrom"]);
                if (reader["ValidTill"] != null)
                    ValidTill = Convert.ToDateTime(reader["ValidTill"]);
                if (reader["CreatedOn"] != null)
                    CreatedOn = Convert.ToDateTime(reader["CreatedOn"]);
                if (reader["CreatedBy"] != null)
                    CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                if (reader["ModifiedOn"] != null)
                    ModifiedOn = Convert.ToDateTime(reader["ModifiedOn"]);
                if (reader["ModifiedBy"] != null)
                    ModifiedBy = Convert.ToInt32(reader["ModifiedBy"]);
            }
            Error = Convert.ToString(objSqlParam[2].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }
        }

        /// <summary>
        /// Toggle activation of selected record
        /// </summary>
        /*public bool ToggleActivation()
        {
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0]=new SqlParameter("@TravelRateID",TravelRateID);
            objSqlParam[1]=new SqlParameter("@Out_Param",SqlDbType.TinyInt, 2); 
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error",SqlDbType.VarChar, 500); 
            objSqlParam[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(ConnectionString,CommandType.StoredProcedure,"prcTravelRate_Toggle",objSqlParam);
            result = (Convert.ToInt16(objSqlParam[1].Value) == 0);
            Error = Convert.ToString(objSqlParam[2].Value);

        }*/
        #endregion
    }
