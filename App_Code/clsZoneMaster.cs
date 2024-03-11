//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace ZedAxis.ZedEBS.Admin.Objects
//{
//    class clsZoneMaster
//    {
//    }
//}


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

/*
Modified By		Modified On		Modification
-----------		-----------		------------
Pankaj Mittal		12/13/2011	Created

*/


    public class clsZoneMaster : IDisposable
    {

        #region Private Class Variables
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
        #endregion

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

        #region Constructors
        public clsZoneMaster()
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

        ~clsZoneMaster()
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
        /// <param name="pk_Id">Returns the Pk Id of last inserted record. </param>
        /// <results>Int16: 0 if success</results> 
        public Int16 Save()
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[8];
            objSqlParam[0] = new SqlParameter("@CountryID", CountryID);
            objSqlParam[1] = new SqlParameter("@ZoneName", ZoneName);
            objSqlParam[2] = new SqlParameter("@DisplayOrder", DisplayOrder);
            objSqlParam[3] = new SqlParameter("@Remarks", Remarks);
            objSqlParam[4] = new SqlParameter("@CreatedBy", CreatedBy);
            objSqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[5].Direction = ParameterDirection.Output;
            objSqlParam[6] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[6].Direction = ParameterDirection.Output;
            objSqlParam[7] = new SqlParameter("@active",Active);
            int resultdata = DataAccess.DataAccess.Instance.DBInsertCommand("prcZoneMaster_Insert", objSqlParam);
            result = Convert.ToInt16(objSqlParam[5].Value);
            Error = Convert.ToString(objSqlParam[6].Value);
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
            SqlParameter[] objSqlParam = new SqlParameter[13];
            objSqlParam[0] = new SqlParameter("@ZoneID", ZoneID);
            objSqlParam[1] = new SqlParameter("@CountryID", CountryID);
            objSqlParam[2] = new SqlParameter("@ZoneName", ZoneName);
            objSqlParam[3] = new SqlParameter("@DisplayOrder", DisplayOrder);
            objSqlParam[4] = new SqlParameter("@Remarks", Remarks);
            objSqlParam[5] = new SqlParameter("@ModifiedBy", ModifiedBy);
            objSqlParam[6] = new SqlParameter("@Active", Active);
            objSqlParam[7] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[7].Direction = ParameterDirection.Output;
            objSqlParam[8] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[8].Direction = ParameterDirection.Output;
            int resultupdatedata = DataAccess.DataAccess.Instance.DBInsertCommand("prcZoneMaster_Update", objSqlParam);
          //  SqlHelper.ExecuteNonQuery(DBConnection.ConStr, CommandType.StoredProcedure, "prcZoneMaster_Update", objSqlParam);
            result = Convert.ToInt16(objSqlParam[7].Value);
            Error = Convert.ToString(objSqlParam[8].Value);
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
        //public bool Delete()
        //{
        //    bool result = false;
        //    SqlParameter[] objSqlParam = new SqlParameter[3];
        //    objSqlParam[0] = new SqlParameter("@ZoneID", ZoneID);
        //    objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        //    objSqlParam[1].Direction = ParameterDirection.Output;
        //    objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        //    objSqlParam[2].Direction = ParameterDirection.Output;
        //    SqlHelper.ExecuteNonQuery(DBConnection.ConStr, CommandType.StoredProcedure, "prcZoneMaster_Delete", objSqlParam);
        //    result = (Convert.ToInt16(objSqlParam[1].Value) == 0);
        //    Error = Convert.ToString(objSqlParam[2].Value);
        //    if (Error != string.Empty)
        //    {
        //        throw new ArgumentException(Error);
        //    }

        //    return result;
        //}

        /// <summary>
        /// Get All records from database for selected key
        /// </summary>
        /// <results>DataTable: Collection of records</results> 		
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
            objSqlParam[4] = new SqlParameter("@ZoneID", ZoneID);
            objSqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[5].Direction = ParameterDirection.Output;
            objSqlParam[6] = new SqlParameter("@ZoneName", ZoneName);
            objSqlParam[7] = new SqlParameter("@SrchCountryID", CountryID);
            objSqlParam[8] = new SqlParameter("@active", Active);
            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcZoneMaster_Select", CommandType.StoredProcedure, objSqlParam);
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
            objSqlParam[4] = new SqlParameter("@ZoneID", ZoneID);
            objSqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[5].Direction = ParameterDirection.Output;
            objSqlParam[6] = new SqlParameter("@ZoneName", ZoneName);
            objSqlParam[7] = new SqlParameter("@SrchCountryID", CountryID);
            objSqlParam[8] = new SqlParameter("@active", Active);
            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcZoneMaster_SelectExportinExcel", CommandType.StoredProcedure, objSqlParam);
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
        //public DataTable SelectList()
        //{
        //    DataTable dtResult = new DataTable();
        //    SqlParameter[] objSqlParam = new SqlParameter[4];
        //    objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        //    objSqlParam[0].Direction = ParameterDirection.Output;
        //    objSqlParam[1] = new SqlParameter("@zoneid",ZoneID);
        //    objSqlParam[2] = new SqlParameter("@active", Active);
        //    objSqlParam[3] = new SqlParameter("@parentid",CountryID);
        //    DataSet dsResult = SqlHelper.ExecuteDataset(DBConnection.ConStr, CommandType.StoredProcedure, "prcZoneMaster_SelectList", objSqlParam);
        //    if (dsResult != null && dsResult.Tables.Count > 0)
        //        dtResult = dsResult.Tables[0];
        //    Error = Convert.ToString(objSqlParam[0].Value);
        //    if (Error != string.Empty)
        //    {
        //        throw new ArgumentException(Error);
        //    }

        //    return dtResult;
        //}



        /// <summary>
        /// Get All records from database for selected key
        /// </summary>
        /// <results>DataTable: Collection of records</results> 		
        //public DataTable SelectById()
        //{
        //    DataTable dtResult = new DataTable();
        //    SqlParameter[] objSqlParam = new SqlParameter[3];
        //    objSqlParam[0] = new SqlParameter("@ZoneID", ZoneID);
        //    objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        //    objSqlParam[1].Direction = ParameterDirection.Output;
        //    objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        //    objSqlParam[2].Direction = ParameterDirection.Output;
        //    DataSet dsResult = SqlHelper.ExecuteDataset(DBConnection.ConStr, CommandType.StoredProcedure, "prcZoneMaster_SelectById", objSqlParam);
        //    if (dsResult != null && dsResult.Tables.Count > 0)
        //        dtResult = dsResult.Tables[0];
        //    Error = Convert.ToString(objSqlParam[2].Value);
        //    if (Error != string.Empty)
        //    {
        //        throw new ArgumentException(Error);
        //    }

        //    return dtResult;
        //}
        public DataSet SelectForEdit()
        {
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@ZoneID", ZoneID);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcZoneMaster_SelectForEdit", CommandType.StoredProcedure, objSqlParam); 
            Error = Convert.ToString(objSqlParam[2].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return dsResult;
        }

        /// <summary>
        /// Get All records from database for selected key
        /// </summary>
        /// <results>DataTable: Collection of records</results> 		
        //public void Load()
        //{
        //    SqlParameter[] objSqlParam = new SqlParameter[3];
        //    objSqlParam[0] = new SqlParameter("@ZoneID", ZoneID);
        //    objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        //    objSqlParam[1].Direction = ParameterDirection.Output;
        //    objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        //    objSqlParam[2].Direction = ParameterDirection.Output;
        //    IDataReader reader = SqlHelper.ExecuteReader(DBConnection.ConStr, CommandType.StoredProcedure, "prcZoneMaster_SelectById", objSqlParam);
        //    while (reader.Read())
        //    {
        //        if (reader["CountryID"] != null)
        //            CountryID = Convert.ToInt32(reader["CountryID"]);
        //        if (reader["ZoneName"] != null)
        //            ZoneName = Convert.ToString(reader["ZoneName"]);
        //        if (reader["DisplayOrder"] != null)
        //            DisplayOrder = Convert.ToInt32(reader["DisplayOrder"]);
        //        if (reader["NullOrderReversal"] != null)
        //            NullOrderReversal = Convert.ToInt32(reader["NullOrderReversal"]);
        //        if (reader["Remarks"] != null)
        //            Remarks = Convert.ToString(reader["Remarks"]);
        //        if (reader["CreatedBy"] != null)
        //            CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
        //        if (reader["CreatedOn"] != null)
        //            CreatedOn = Convert.ToDateTime(reader["CreatedOn"]);
        //        if (reader["ModifiedBy"] != null)
        //            ModifiedBy = Convert.ToInt32(reader["ModifiedBy"]);
        //        if (reader["ModifiedOn"] != null)
        //            ModifiedOn = Convert.ToDateTime(reader["ModifiedOn"]);
        //        if (reader["Active"] != null)
        //            Active = Convert.ToInt16(reader["Active"]);
        //    }
        //    Error = Convert.ToString(objSqlParam[2].Value);
        //    if (Error != string.Empty)
        //    {
        //        throw new ArgumentException(Error);
        //    }

        //}

        /// <summary>
        /// Toggle activation of selected record
        /// </summary>
        public Int16 ToggleActivation()
        {
            Int16 result = 0;
            SqlParameter[] objSqlParam = new SqlParameter[4];
            objSqlParam[0] = new SqlParameter("@ZoneID", ZoneID);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@modifiedby", ModifiedBy);
            int updatedat = DataAccess.DataAccess.Instance.DBInsertCommand("prcZoneMaster_TActive", objSqlParam);
            result = (Convert.ToInt16(objSqlParam[1].Value));
            Error = Convert.ToString(objSqlParam[2].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }
            return result;
        }
        #endregion
    }

