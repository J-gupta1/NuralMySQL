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

/*
Modified By		Modified On		Modification
-----------		-----------		------------
Amit Bhardwaj	16-Apr-11	    Created
*/

namespace LuminousSMS.Data
{
    public class clsEmailOutboundLog : IDisposable
    {
        #region Private Class Variables
        private int _intEmailOutboundLogID;
        private short _shtEmailOutboundTransID;
        private string _strEmailTo;
        private string _strEmailCC;
        private string _strEmailBody;
        private string _strEmailAttachmentNames;
        private short _shtIsDelivered;
        private DateTime _dtDeliveredDate;
        private DateTime _dtEmailExpiryDate;
        private DateTime _dtRecordCreationDate;

        private string _strError;
        private Int32 _intPageIndex;
        private Int32 _intPageSize;
        private Int32 _intTotalRecords;

        private string _strEmailOutboundCallIdsAll;
        public static String ConStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        #endregion

        #region Public Properties
        public string EmailOutboundCallIdsAll
        {
            get { return _strEmailOutboundCallIdsAll; }
            set { _strEmailOutboundCallIdsAll = value; }
        }

        public int EmailOutboundLogID
        {
            get
            {
                return _intEmailOutboundLogID;
            }
            set
            {
                _intEmailOutboundLogID = value;
            }
        }
        public short EmailOutboundTransID
        {
            get
            {
                return _shtEmailOutboundTransID;
            }
            set
            {
                _shtEmailOutboundTransID = value;
            }
        }
        public string EmailTo
        {
            get
            {
                return _strEmailTo;
            }
            set
            {
                _strEmailTo = value;
            }
        }
        public string EmailCC
        {
            get
            {
                return _strEmailCC;
            }
            set
            {
                _strEmailCC = value;
            }
        }
        public string EmailBody
        {
            get
            {
                return _strEmailBody;
            }
            set
            {
                _strEmailBody = value;
            }
        }
        public string EmailAttachmentNames
        {
            get
            {
                return _strEmailAttachmentNames;
            }
            set
            {
                _strEmailAttachmentNames = value;
            }
        }
        public short IsDelivered
        {
            get
            {
                return _shtIsDelivered;
            }
            set
            {
                _shtIsDelivered = value;
            }
        }
        public DateTime DeliveredDate
        {
            get
            {
                return _dtDeliveredDate;
            }
            set
            {
                _dtDeliveredDate = value;
            }
        }
        public DateTime EmailExpiryDate
        {
            get
            {
                return _dtEmailExpiryDate;
            }
            set
            {
                _dtEmailExpiryDate = value;
            }
        }
        public DateTime RecordCreationDate
        {
            get
            {
                return _dtRecordCreationDate;
            }
            set
            {
                _dtRecordCreationDate = value;
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
        public clsEmailOutboundLog()
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

        ~clsEmailOutboundLog()
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
            objSqlParam[0] = new SqlParameter("@EmailOutboundTransID", EmailOutboundTransID);
            objSqlParam[1] = new SqlParameter("@EmailTo", EmailTo);
            objSqlParam[2] = new SqlParameter("@EmailCC", EmailCC);
            objSqlParam[3] = new SqlParameter("@EmailBody", EmailBody);
            objSqlParam[4] = new SqlParameter("@EmailAttachmentNames", EmailAttachmentNames);
            objSqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[5].Direction = ParameterDirection.Output;
            objSqlParam[6] = new SqlParameter("@EmailOutboundLogID", SqlDbType.Int, 4);
            objSqlParam[6].Direction = ParameterDirection.Output;
            objSqlParam[7] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[7].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(ConStr, CommandType.StoredProcedure, "prcEmailOutboundLog_Insert", objSqlParam);
            result = Convert.ToInt16(objSqlParam[5].Value);
            EmailOutboundLogID = Convert.ToInt32(objSqlParam[6].Value);
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
        public Int16 Update(string EmailOutboundLogIDs)
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[12];
            objSqlParam[0] = new SqlParameter("@EmailOutboundLogIDs", EmailOutboundLogIDs);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@EmailOutboundCallIdsAll", EmailOutboundCallIdsAll);

            SqlHelper.ExecuteNonQuery(ConStr, CommandType.StoredProcedure, "prcEmailOutboundLog_UpdateSent", objSqlParam);
            result = Convert.ToInt16(objSqlParam[1].Value);
            Error = Convert.ToString(objSqlParam[2].Value);

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
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@EmailOutboundLogID", EmailOutboundLogID);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(ConStr, CommandType.StoredProcedure, "prcEmailOutboundLog_Delete", objSqlParam);
            result = (Convert.ToInt16(objSqlParam[1].Value) == 0);
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
        public DataTable SelectPending()
        {
            DataTable dtResult = new DataTable();
            DataSet dsResult = SqlHelper.ExecuteDataset(ConStr, CommandType.StoredProcedure, "prcEmailOutboundLog_SelectPending", null);

            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];

            return dtResult;
        }

        /// <summary>
        /// Get All records from database for selected key
        /// </summary>
        /// <results>DataTable: Collection of records</results> 		
        public DataTable SelectById()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@EmailOutboundLogID", EmailOutboundLogID);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            DataSet dsResult = SqlHelper.ExecuteDataset(ConStr, CommandType.StoredProcedure, "prcEmailOutboundLog_SelectById", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[2].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return dtResult;
        }

        /// <summary>
        /// Get All records from database for selected key
        /// </summary>
        /// <results>DataTable: Collection of records</results> 		
        public void Load()
        {
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@EmailOutboundLogID", EmailOutboundLogID);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            IDataReader reader = SqlHelper.ExecuteReader(ConStr, CommandType.StoredProcedure, "prcEmailOutboundLog_SelectById", objSqlParam);
            while (reader.Read())
            {
                if (reader["EmailOutboundTransID"] != null)
                    EmailOutboundTransID = Convert.ToInt16(reader["EmailOutboundTransID"]);
                if (reader["EmailTo"] != null)
                    EmailTo = Convert.ToString(reader["EmailTo"]);
                if (reader["EmailCC"] != null)
                    EmailCC = Convert.ToString(reader["EmailCC"]);
                if (reader["EmailBody"] != null)
                    EmailBody = Convert.ToString(reader["EmailBody"]);
                if (reader["EmailAttachmentNames"] != null)
                    EmailAttachmentNames = Convert.ToString(reader["EmailAttachmentNames"]);
                if (reader["IsDelivered"] != null)
                    IsDelivered = Convert.ToInt16(reader["IsDelivered"]);
                if (reader["DeliveredDate"] != null)
                    DeliveredDate = Convert.ToDateTime(reader["DeliveredDate"]);
                if (reader["EmailExpiryDate"] != null)
                    EmailExpiryDate = Convert.ToDateTime(reader["EmailExpiryDate"]);
                if (reader["RecordCreationDate"] != null)
                    RecordCreationDate = Convert.ToDateTime(reader["RecordCreationDate"]);
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
            objSqlParam[0]=new SqlParameter("@EmailOutboundLogID",EmailOutboundLogID);
            objSqlParam[1]=new SqlParameter("@Out_Param",SqlDbType.TinyInt, 2); 
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error",SqlDbType.VarChar, 500); 
            objSqlParam[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(DBConnection.ConStr,CommandType.StoredProcedure,"prcEmailOutboundLog_Toggle",objSqlParam);
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
