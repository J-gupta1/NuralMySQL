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
 * 19-Dec-2018,   Rakesh Raj, Imported from ZEDERP
 */

namespace DataAccess
{
   public class clsTaxmaster :IDisposable
    {
 
        #region Public Properties

        public int TaxMasterID
        {
            get;
            set;
        }
        public int ModifiedBy
        {
            get;
            set;
        }

        public string TaxName
        {
            get;
            set;
        }
        public short TaxTypeID
        {
            get;
            set;
        }
        public string TaxType
        {
            get;
            set;
        }

        public int CreatedBy
        {
            get;
            set;
        }

        public short TaxGroupID
        {
            get;
            set;
        }

        public int? Status
        {
            get;
            set;
        }

        public bool  Active
        {
            get;
            set;
        }
        public DateTime CreatedOn
        {
            get;
            set;
        }

        public byte TaxTypes
        {
            get;
            set;
        }

        public string Error
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
            get;
            set;
        }
        public Int32 TotalRecords
        {
            get;
            set;
        }
        public int DisplayOrder
        {
            get;
            set;
        }

        public string Remarks
        {
            get;
            set;
        }

        public int CountryId
        {
            get;
            set;
        }

        #endregion

        #region public functions

        public Int16 Save()
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[11];
            objSqlParam[0] = new SqlParameter("@TaxName", TaxName);
            objSqlParam[1] = new SqlParameter("@TaxTypeID", TaxTypeID);
            objSqlParam[2] = new SqlParameter("@Active", Active);
            objSqlParam[3] = new SqlParameter("@createdby",CreatedBy);
            objSqlParam[4] = new SqlParameter("@TaxGroupid", TaxGroupID);       // #Ch01: added
            objSqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[5].Direction = ParameterDirection.Output;
            objSqlParam[6] = new SqlParameter("@PKId", SqlDbType.Int, 4);
            objSqlParam[6].Direction = ParameterDirection.Output;
            objSqlParam[7] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[7].Direction = ParameterDirection.Output;
            objSqlParam[8] = new SqlParameter("@displayorder", DisplayOrder);
            objSqlParam[9] = new SqlParameter("@remarks", Remarks);
            objSqlParam[10] = new SqlParameter("@countryid",CountryId);
            SqlHelper.ExecuteNonQuery(PageBase.ConStr, CommandType.StoredProcedure, "prcTaxMaster_Insert", objSqlParam);
            result = Convert.ToInt16(objSqlParam[5].Value);
            TaxMasterID = Convert.ToInt32(objSqlParam[6].Value);
            Error = Convert.ToString(objSqlParam[7].Value);
            return result;
        }


        public Int16 Update()
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[9];
            objSqlParam[0] = new SqlParameter("@TaxMasterID", TaxMasterID);
            objSqlParam[1] = new SqlParameter("@TaxName", TaxName);
            objSqlParam[2] = new SqlParameter("@TaxTypeID", TaxTypeID);
            objSqlParam[3] = new SqlParameter("@TaxType", TaxType);
            objSqlParam[4] = new SqlParameter("@Active", Active);
            //objSqlParam[5] = new SqlParameter("@CreatedOn",CreatedOn);    // #Ch01: removed
            objSqlParam[5] = new SqlParameter("@TaxGroup", TaxGroupID);       // #Ch01: added
            objSqlParam[6] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[6].Direction = ParameterDirection.Output;
            objSqlParam[7] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[7].Direction = ParameterDirection.Output;
            objSqlParam[8] = new SqlParameter("@modifiedBy",ModifiedBy);
            SqlHelper.ExecuteNonQuery(PageBase.ConStr, CommandType.StoredProcedure, "prcTaxMaster_Update", objSqlParam);
            result = Convert.ToInt16(objSqlParam[6].Value);
            Error = Convert.ToString(objSqlParam[7].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return result;
        }


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
            objSqlParam[4] = new SqlParameter("@taxname",TaxName);
            objSqlParam[5] = new SqlParameter("@active",Status);
            objSqlParam[6] = new SqlParameter("@countryid", CountryId);
            DataSet dsResult = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.StoredProcedure, "prcTaxMaster_Select", objSqlParam);
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
            SqlParameter[] objSqlParam = new SqlParameter[5];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@taxmasterid",TaxMasterID);
            objSqlParam[2] = new SqlParameter("@active",Status);
            objSqlParam[3] = new SqlParameter("@countryid", CountryId);
            objSqlParam[4] = new SqlParameter("@taxtype",TaxTypes);
            DataSet dsResult = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.StoredProcedure, "prcTaxMaster_SelectList", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[0].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return dtResult;
        }

        public DataTable SelectById()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@TaxMasterID", TaxMasterID);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            DataSet dsResult = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.StoredProcedure, "prcTaxMaster_SelectById", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[2].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return dtResult;
        }

        public DataTable SelectTaxGroup()
        {
            DataTable dtResult = new DataTable();
            DataSet dsResult = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.StoredProcedure, "[prcTaxGroup_Select]");
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
             return dtResult;
        }

        public DataTable SelectTaxType()
        {
            DataTable dtResult = new DataTable();
            DataSet dsResult = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.StoredProcedure, "[prcTaxType_Select]");
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            return dtResult;
        }

        public Int16 UpdateActive()
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[4];
            objSqlParam[0] = new SqlParameter("@taxmasterid", TaxMasterID);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@modifiedby", ModifiedBy);
            SqlHelper.ExecuteNonQuery(PageBase.ConStr, CommandType.StoredProcedure, "prcTaxMaster_ActionUpdate", objSqlParam);
            result = Convert.ToInt16(objSqlParam[1].Value);
            Error = Convert.ToString(objSqlParam[2].Value);
            return result;
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

        ~clsTaxmaster()
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
