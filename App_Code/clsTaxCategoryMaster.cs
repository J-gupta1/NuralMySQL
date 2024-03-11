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
using BussinessLogic;

/*
Modified By		Modified On		Modification
-----------		-----------		------------
Pankaj Mittal		8/6/2011	Created
19/12/2018, Rakesh Raj, Addition 
*/
namespace DataAccess
{
    public class clsTaxCategoryMaster : IDisposable
    {
        
        #region Private Class Variables
        private Int32 _intTaxCategoryID;
        private string _strTaxCategoryName;
        private DateTime _dtCreatedOn;
        private int _intCreatedBy;
        private short _shtActive;
        private DateTime _dtModifiedOn;
        private int _intModifiedBy;
        private Int16 _intActiveStatus;
        private string _strError;
        private Int32 _intPageIndex;
        private Int32 _intPageSize;
        private Int32 _intTotalRecords;
        #endregion

        #region Public Properties
        public Int16 ActiveStatus
        {
            get { return _intActiveStatus; }
            set { _intActiveStatus = value; }
        }

        public Int32 TaxCategoryID
        {
            get { return _intTaxCategoryID; }
            set { _intTaxCategoryID = value; }
        }

        public string TaxCategoryName
        {
            get { return _strTaxCategoryName; }
            set { _strTaxCategoryName = value; }
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
        #endregion

        #region Constructors
        public clsTaxCategoryMaster()
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

        ~clsTaxCategoryMaster()
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
            SqlParameter[] objSqlParam = new SqlParameter[6];
            objSqlParam[0] = new SqlParameter("@TaxGroupID", TaxCategoryID);
            objSqlParam[1] = new SqlParameter("@TaxGroupName", TaxCategoryName);
            objSqlParam[2] = new SqlParameter("@CreatedBy", CreatedBy);
            objSqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[4].Direction = ParameterDirection.Output;
            //objSqlParam[5] = new SqlParameter("@DuplicateTaxGroupName", SqlDbType.Xml);
            //objSqlParam[5].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(PageBase.ConStr, CommandType.StoredProcedure, "prcTaxGroupMaster_Insert_Update", objSqlParam);
            result = Convert.ToInt16(objSqlParam[3].Value);
            Error = Convert.ToString(objSqlParam[4].Value);
            //DuplicateRecords = Convert.ToString(objSqlParam[5].Value);
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
        public Int16 ActiveInactive()
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[4];
            objSqlParam[0] = new SqlParameter("@TaxCategoryID", TaxCategoryID);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@ModifiedBy", CreatedBy);
            SqlHelper.ExecuteNonQuery(PageBase.ConStr, CommandType.StoredProcedure, "prcTaxGroupMaster_TActive", objSqlParam);
            result = Convert.ToInt16(objSqlParam[1].Value);
            Error = Convert.ToString(objSqlParam[2].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return result;
        }

        /// <summary>
        /// Get All records from database for selected key
        /// </summary>
        /// <results>DataTable: Collection of records</results>  		
        public DataTable SelectAll()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[7];
            objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@ActiveStatus", ActiveStatus);
            objSqlParam[5] = new SqlParameter("@TaxCategoryID", TaxCategoryID);
            objSqlParam[6] = new SqlParameter("@TaxCategoryName", TaxCategoryName);
            DataSet dsResult = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.StoredProcedure, "prcTaxGroupMaster_Select", objSqlParam);
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


        public DataTable SelectList()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@active", ActiveStatus);
            objSqlParam[2] = new SqlParameter("@taxcategoryid",TaxCategoryID);
            DataSet dsResult = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.StoredProcedure, "[prcTaxCategoryMaster_SelectList]", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[0].Value);
            return dtResult;
        }


        /// <summary>
        /// Get All records from database for selected key
        /// </summary>
        /// <results>DataTable: Collection of records</results> 		
        //public DataTable SelectById()
        //{
        //    DataTable dtResult = new DataTable();
        //    SqlParameter[] objSqlParam = new SqlParameter[3];
        //    objSqlParam[0] = new SqlParameter("@PartBrandID", PartBrandID);
        //    objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        //    objSqlParam[1].Direction = ParameterDirection.Output;
        //    objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        //    objSqlParam[2].Direction = ParameterDirection.Output;
        //    DataSet dsResult = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.StoredProcedure, "prcPartBrand_SelectById", objSqlParam);
        //    if (dsResult != null && dsResult.Tables.Count > 0)
        //        dtResult = dsResult.Tables[0];
        //    Error = Convert.ToString(objSqlParam[2].Value);
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
        //public void Load()
        //{
        //SqlParameter[] objSqlParam = new SqlParameter[3];
        //objSqlParam[0] = new SqlParameter("@PartBrandID", PartBrandID);
        //objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        //objSqlParam[1].Direction = ParameterDirection.Output;
        //objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        //objSqlParam[2].Direction = ParameterDirection.Output;
        //IDataReader reader = SqlHelper.ExecuteReader(PageBase.ConStr, CommandType.StoredProcedure, "prcPartBrand_SelectById", objSqlParam);
        //while (reader.Read())
        //{
        //    if (reader["PartID"] != null)
        //        PartID = Convert.ToInt64(reader["PartID"]);
        //    if (reader["BrandID"] != null)
        //        BrandID = Convert.ToInt16(reader["BrandID"]);
        //    if (reader["CreatedOn"] != null)
        //        CreatedOn = Convert.ToDateTime(reader["CreatedOn"]);
        //    if (reader["CreatedBy"] != null)
        //        CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
        //    if (reader["Active"] != null)
        //        Active = Convert.ToInt16(reader["Active"]);
        //    if (reader["ModifiedOn"] != null)
        //        ModifiedOn = Convert.ToDateTime(reader["ModifiedOn"]);
        //    if (reader["ModifiedBy"] != null)
        //        ModifiedBy = Convert.ToInt32(reader["ModifiedBy"]);
        //}
        //Error = Convert.ToString(objSqlParam[2].Value);
        //if (Error != string.Empty)
        //{
        //    throw new ArgumentException(Error);
        //}
        //}

        /// <summary>
        /// Toggle activation of selected record
        /// </summary>
        /*public bool ToggleActivation()
        {
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0]=new SqlParameter("@PartBrandID",PartBrandID);
            objSqlParam[1]=new SqlParameter("@Out_Param",SqlDbType.TinyInt, 2); 
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error",SqlDbType.VarChar, 500); 
            objSqlParam[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(PageBase.ConStr,CommandType.StoredProcedure,"prcPartBrand_Toggle",objSqlParam);
            result = (Convert.ToInt16(objSqlParam[1].Value) == 0);
            Error = Convert.ToString(objSqlParam[2].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

        }*/
        #endregion
    }
}