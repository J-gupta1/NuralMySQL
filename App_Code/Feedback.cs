/*
====================================================================================================================================
Copyright	: Zed-Axis Technologies, 2016
Created By	: Sumit Maurya
Create date	: 16-Mar-2016
Description	: This interface Log Feedback.
====================================================================================================================================
Change Log:
DD-MMM-YYYY, Name , #CCXX - Description
------------------------------------------------------------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DataAccess
{
    public class Feedback : IDisposable
    {
        #region private Properties
        private string _ErrorMsg;
        private Int32 _web_user_id;
        private string _FeedbackText;
        private int _FeedbackID;
        private string strError;
        private Int16 _FeedbackStatus;
        private int _intTotalRecords;
        #endregion private Properties

        #region Public Properties
        DataTable dtResult;
        DataSet dsResult;

        public string ErrorMsg
        {
            get { return _ErrorMsg; }
            set { _ErrorMsg = value; }
        }
        public Int32 web_user_id
        {
            get { return _web_user_id; }
            set { _web_user_id = value; }

        }
        public string FeedbackText
        {
            get { return _FeedbackText; }
            set { _FeedbackText = value; }
        }
        public int FeedbackID
        {
            get { return _FeedbackID; }
            set { _FeedbackID = value; }
        }
        public string Error
        {
            get { return strError; }
            set { strError = value; }
        }
        public Int16 FeedbackStatus
        {
            get { return _FeedbackStatus; }
            set { _FeedbackStatus = value; }
        }
        public string FeedbackRevertText
        {
            get;
            set;
        }
        public Int32 TotalRecords
        {
            get
            {
                return _intTotalRecords;
            }
            set
            {
                _intTotalRecords = value;
            }
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
        public Int16 FilterType
        {
            get;
            set;
        }
        public DateTime FromDate
        {
            get;
            set;
        }
        public DateTime ToDate
        {
            get;
            set;
        }
        Int32 IntResultCount = 0;
        #endregion Public Properties

        #region user defined functions


        public DataSet GetFeedback()
        {
            try
            {
                SqlParameter[] SqlParam;

                SqlParam = new SqlParameter[11];
                SqlParam[0] = new SqlParameter("@FeedbackID", FeedbackID);
                SqlParam[1] = new SqlParameter("@FeedbackStatus", FeedbackStatus);
                SqlParam[2] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@OutError", SqlDbType.VarChar, 200);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
                SqlParam[4].Direction = ParameterDirection.Output;
                SqlParam[5] = new SqlParameter("@PageIndex", PageIndex);
                SqlParam[6] = new SqlParameter("@PageSize", PageSize);
                SqlParam[7] = new SqlParameter("@UserID", web_user_id);
                SqlParam[8] = new SqlParameter("@FromDate", FromDate);
                SqlParam[9] = new SqlParameter("@ToDate", ToDate);
                SqlParam[10] = new SqlParameter("@FilterType", FilterType);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetFeedback", CommandType.StoredProcedure, SqlParam);
                TotalRecords = Convert.ToInt32(SqlParam[4].Value);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int32 SaveFeedback()
        {
            try
            {
                SqlParameter[] SqlParam;
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@FeedbackText", FeedbackText);
                SqlParam[1] = new SqlParameter("@web_user_id", web_user_id);
                SqlParam[2] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@OutError", SqlDbType.VarChar, 200);
                SqlParam[3].Direction = ParameterDirection.Output;
                DataAccess.Instance.DBInsertCommand("prcSaveUpdateFeedback", SqlParam);
                if (SqlParam[2].Value.ToString() != "")
                {
                    IntResultCount = Convert.ToInt32(SqlParam[2].Value.ToString());
                }
                if (SqlParam[3].Value != DBNull.Value && SqlParam[3].Value.ToString() != "")
                {
                    Error = (SqlParam[3].Value).ToString();
                }
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public Int32 UpdateFeedback()
        {
            try
            {
                SqlParameter[] SqlParam;
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@FeedbackRevertText", FeedbackRevertText);
                SqlParam[1] = new SqlParameter("@web_user_id", web_user_id);
                SqlParam[2] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@OutError", SqlDbType.VarChar, 200);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@FeedbackID", FeedbackID);
                DataAccess.Instance.DBInsertCommand("prcSaveUpdateFeedback", SqlParam);
                if (SqlParam[2].Value.ToString() != "")
                {
                    IntResultCount = Convert.ToInt32(SqlParam[2].Value.ToString());
                }
                if (SqlParam[3].Value != DBNull.Value && SqlParam[3].Value.ToString() != "")
                {
                    Error = (SqlParam[3].Value).ToString();
                }
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion user defined functions


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

        ~Feedback()
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
