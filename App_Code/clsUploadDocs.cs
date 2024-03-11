using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace DataAccess
{
    public class clsUploadDocs : IDisposable
    {
        # region dispose
        private bool IsDisposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~clsUploadDocs()
        {
            Dispose(false);
        }
        protected virtual void Dispose(bool disposedStatus)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                if (disposedStatus)
                {
                }
            }
        }

        #endregion
        # region private And Public Properties

        private int _OutParam;
        public int OutParam
        {
            get { return _OutParam; }
            set { _OutParam = value; }
        }

        private int _PageIndex;
        public int PageIndex
        {
            get { return _PageIndex; }
            set { _PageIndex = value; }
        }

        private int _PageSize;
        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }
        private string _OutError;
        public string OutError
        {
            get { return _OutError; }
            set { _OutError = value; }
        }

        private DataTable _ReturnDataTable;
        public DataTable ReturnDataTable
        {
            get { return _ReturnDataTable; }
            set { _ReturnDataTable = value; }
        }
        private int _TotalCount;
        public int TotalCount
        {
            get { return _TotalCount; }
            set { _TotalCount = value; }
        }

        private int _UploadTypeId;
        public int UploadTypeId
        {
            get { return _UploadTypeId; }
            set { _UploadTypeId = value; }
        }

        private int _ISPID;
        public int ISPID
        {
            get { return _ISPID; }
            set { _ISPID = value; }
        }
        private string _ISPCode;
        public string ISPCode
        {
            get { return _ISPCode; }
            set { _ISPCode = value; }
        }
        DateTime? datefrom; DateTime? dateto; int NotificationId;
        public DateTime? DateFrom
        {
            get { return datefrom; }
            set { datefrom = value; }
        }
        public DateTime? DateTo
        {
            get { return dateto; }
            set { dateto = value; }
        }
        private int _Active;
        public int Active
        {
            get { return _Active; }
            set { _Active = value; }
        }
        private string _UploadType;
        public string UploadType
        {
            get { return _UploadType; }
            set { _UploadType = value; }
        }
        private int _CreatedBy;
        public int CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }
        private string _InsError;
        public string InsError
        {
            get { return _InsError; }
            set { _InsError = value; }
        }
        private string _Error;
        public string Error
        {
            get { return _Error; }
            set { _Error = value; }
        }
        private int _UploadTypeID;
        public int UploadTypeID
        {
            get { return _UploadTypeID; }
            set { _UploadTypeID = value; }
        }
        private string _ErrorDetailXML;
        public string ErrorDetailXML
        {
            get { return _ErrorDetailXML; }
            set { _ErrorDetailXML = value; }
        }
        private int _Condition;
        public int condition
        {
            get { return _Condition; }
            set { _Condition = value; }
        }
        private int _UserId;
        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }
        #endregion
        public DataTable GetUploadType()
        {
            try
            {
                return DataAccess.Instance.GetTableFromDatabase("prcGetUploadType", CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetUploadTypeData()
        {
            try
            {
                SqlParameter[] SqlParam = new SqlParameter[9];
                SqlParam[0] = new SqlParameter("@UploadType", UploadTypeId);
                SqlParam[1] = new SqlParameter("@FromDate", datefrom);
                SqlParam[2] = new SqlParameter("@ToDate", dateto);
                SqlParam[3] = new SqlParameter("@ISPCode", ISPCode);
                SqlParam[4] = new SqlParameter("@PageIndex", PageIndex);
                SqlParam[5] = new SqlParameter("@PageSize", PageSize);
                SqlParam[6] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
                SqlParam[6].Direction = ParameterDirection.Output;
                SqlParam[7] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
                SqlParam[7].Direction = ParameterDirection.Output;
                SqlParam[8] = new SqlParameter("@UserId", UserId);
                ReturnDataTable = ((DataSet)DataAccess.Instance.GetDataSetFromDatabase("prc_ViewRSPUploadDocs", CommandType.StoredProcedure, SqlParam)).Tables[0];
                Error = Convert.ToString(SqlParam[7].Value);
                TotalCount = Convert.ToInt32(SqlParam[6].Value);
                return ReturnDataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsertUploadDocs()
        {
            SqlParameter[] SqlParam = new SqlParameter[7];
            SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@InsError", SqlDbType.NVarChar, 200);
            SqlParam[3].Direction = ParameterDirection.Output;
            SqlParam[4] = new SqlParameter("@UploadDocReferenceType", UploadType);
            SqlParam[5] = new SqlParameter("@CreatedBy", CreatedBy);
            SqlParam[6] = new SqlParameter("@Status", Active);
            DataAccess.Instance.DBInsertCommand("prcInsUploadDoc", SqlParam);
            if (SqlParam[2].Value.ToString() != "")
            {
                ErrorDetailXML = SqlParam[2].Value.ToString();
            }
            else { ErrorDetailXML = null; }
            if (SqlParam[3].Value.ToString() != "")
            {
                InsError = SqlParam[3].Value.ToString();
            }
            else { InsError = null; }
            Error = Convert.ToString(SqlParam[1].Value);
        }

        public void UpdateUploadDocs()
        {
            SqlParameter[] SqlParam = new SqlParameter[8];
            SqlParam[0] = new SqlParameter("@UploadDocReferenceType", UploadType);
            SqlParam[1] = new SqlParameter("@Status", Active);
            SqlParam[2] = new SqlParameter("@UploadDocReferenceTypeId", UploadTypeID);
            SqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[3].Direction = ParameterDirection.Output;
            SqlParam[4] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[4].Direction = ParameterDirection.Output;
            SqlParam[5] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
            SqlParam[5].Direction = ParameterDirection.Output;
            SqlParam[6] = new SqlParameter("@InsError", SqlDbType.NVarChar, 200);
            SqlParam[6].Direction = ParameterDirection.Output;
            SqlParam[7] = new SqlParameter("@condition", condition);
            DataAccess.Instance.DBInsertCommand("prcUpdateUploadDocs", SqlParam);
            if (SqlParam[5].Value.ToString() != "")
            {
                ErrorDetailXML = SqlParam[5].Value.ToString();
            }
            else { ErrorDetailXML = null; }
            if (SqlParam[6].Value.ToString() != "")
            {
                InsError = SqlParam[6].Value.ToString();
            }
            else { InsError = null; }
            Error = Convert.ToString(SqlParam[4].Value);
        }
        public DataTable GetUploadDocs()
        {
            SqlParameter[] SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@UploadDocReferenceTypeId", UploadTypeID);
            SqlParam[4] = new SqlParameter("@condition", condition);
            DataTable dtCommon = DataAccess.Instance.GetTableFromDatabase("prcGetUploadDocsMaster", CommandType.StoredProcedure, SqlParam);
            if (SqlParam[2].Value.ToString() != "")
            {
                ErrorDetailXML = SqlParam[2].Value.ToString();
            }
            else { ErrorDetailXML = null; }
            Error = Convert.ToString(SqlParam[1].Value);
            return dtCommon;
        }
    }
}
