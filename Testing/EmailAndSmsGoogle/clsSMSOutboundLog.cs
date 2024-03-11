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
Amit Bhardwaj		12/2/2010	Created

*/

namespace LuminousSMS.Data
{

    public class clsSMSOutboundLog : IDisposable
    {
        #region Private Class Variables
        private int _intSMSOutboundLogID;
        private short _shtSMSOutboundTransID;
        private string _strTransText;
        private string _strSMSMobileNumber;
        private short _shtSendType;
        private string _strSendText;
        private string _strSendTransNo;
        private short _shtIsDelivered;
        private DateTime _dtDeliveredDate;
        private DateTime _dtSMSExpiryDate;
        private DateTime _dtRecordCreationDate;
        private DateTime _dtModifiedOn;
        private DateTime? _dtFrom;
        private DateTime? _dtTo;
        private bool _blnActive;
        private Int16 _intSMSTrasNameID;
        private Int16 _intSMSStatus;

        private string _strError;
        private string _strMobileNo;
        private Int32 _intPageIndex;
        private Int32 _intPageSize;
        private Int32 _intTotalRecords;
        private Int32 _intStatus;
        private int _intResult;
        
        public static String ConStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        #endregion

        #region Public Properties
        public Int32 Status
        {
            get { return _intStatus; }
            set { _intStatus = value; }
        }

        public Int32 result
        {
            get { return _intResult; }
            set { _intResult = value; }
        }

        public bool Active
        {
            get
            {
                return _blnActive;
            }
            set
            {
                _blnActive = value;
            }
        }

        public int SMSOutboundLogID
        {
            get
            {
                return _intSMSOutboundLogID;
            }
            set
            {
                _intSMSOutboundLogID = value;
            }
        }
        public short SMSOutboundTransID
        {
            get
            {
                return _shtSMSOutboundTransID;
            }
            set
            {
                _shtSMSOutboundTransID = value;
            }
        }
        public string TransText
        {
            get
            {
                return _strTransText;
            }
            set
            {
                _strTransText = value;
            }
        }
        public string SMSMobileNumber
        {
            get
            {
                return _strSMSMobileNumber;
            }
            set
            {
                _strSMSMobileNumber = value;
            }
        }
        public short SendType
        {
            get
            {
                return _shtSendType;
            }
            set
            {
                _shtSendType = value;
            }
        }
        public string SendText
        {
            get
            {
                return _strSendText;
            }
            set
            {
                _strSendText = value;
            }
        }
        public string SendTransNo
        {
            get
            {
                return _strSendTransNo;
            }
            set
            {
                _strSendTransNo = value;
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
        public DateTime? FromDate
        {
            get
            {
                return _dtFrom;
            }
            set
            {
                _dtFrom = value;
            }
        }
        public DateTime? ToDate
        {
            get
            {
                return _dtTo;
            }
            set
            {
                _dtTo = value;
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
        public DateTime SMSExpiryDate
        {
            get
            {
                return _dtSMSExpiryDate;
            }
            set
            {
                _dtSMSExpiryDate = value;
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
        public string MobileNo
        {
            get
            {
                return _strMobileNo;
            }
            set
            {
                _strMobileNo = value;
            }
        }
        public Int16 SMSTransNameID
        {
            private get
            {
                return _intSMSTrasNameID;
            }
            set
            {
                _intSMSTrasNameID = value;
            }
        }
        public Int16 SMSStatus
        {
            private get
            {
                return _intSMSStatus;
            }
            set
            {
                _intSMSStatus = value;
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
        public clsSMSOutboundLog()
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

        ~clsSMSOutboundLog()
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
        /*public Int16 Save()
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[14];
            objSqlParam[0] = new SqlParameter("@SMSOutboundTransID", SMSOutboundTransID);
            objSqlParam[1] = new SqlParameter("@TransText", TransText);
            objSqlParam[2] = new SqlParameter("@SMSMobileNumber", SMSMobileNumber);
            objSqlParam[3] = new SqlParameter("@SendType", SendType);
            objSqlParam[4] = new SqlParameter("@SendText", SendText);
            objSqlParam[5] = new SqlParameter("@SendTransNo", SendTransNo);
            objSqlParam[6] = new SqlParameter("@IsDelivered", IsDelivered);
            objSqlParam[7] = new SqlParameter("@DeliveredDate", DeliveredDate);
            objSqlParam[8] = new SqlParameter("@SMSExpiryDate", SMSExpiryDate);
            objSqlParam[9] = new SqlParameter("@RecordCreationDate", RecordCreationDate);
            objSqlParam[10] = new SqlParameter("@ModifiedOn", ModifiedOn);
            objSqlParam[11] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[11].Direction = ParameterDirection.Output;
            objSqlParam[12] = new SqlParameter("@PKId", SqlDbType.Int, 4);
            objSqlParam[12].Direction = ParameterDirection.Output;
            objSqlParam[13] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[13].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(DBConnection.ConStr, CommandType.StoredProcedure, "prcSMSOutboundLog_Insert", objSqlParam);
            result = Convert.ToInt16(objSqlParam[11].Value);
            SMSOutboundLogID = Convert.ToInt32(objSqlParam[12].Value);
            Error = Convert.ToString(objSqlParam[13].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return result;
        }*/

        /// <summary>
        /// Update records in database.
        /// </summary>
        /// <results>Int16: 0 if success</results> 
        /*public Int16 Update()
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[14];
            objSqlParam[0] = new SqlParameter("@SMSOutboundLogID", SMSOutboundLogID);
            objSqlParam[1] = new SqlParameter("@SMSOutboundTransID", SMSOutboundTransID);
            objSqlParam[2] = new SqlParameter("@TransText", TransText);
            objSqlParam[3] = new SqlParameter("@SMSMobileNumber", SMSMobileNumber);
            objSqlParam[4] = new SqlParameter("@SendType", SendType);
            objSqlParam[5] = new SqlParameter("@SendText", SendText);
            objSqlParam[6] = new SqlParameter("@SendTransNo", SendTransNo);
            objSqlParam[7] = new SqlParameter("@IsDelivered", IsDelivered);
            objSqlParam[8] = new SqlParameter("@DeliveredDate", DeliveredDate);
            objSqlParam[9] = new SqlParameter("@SMSExpiryDate", SMSExpiryDate);
            objSqlParam[10] = new SqlParameter("@RecordCreationDate", RecordCreationDate);
            objSqlParam[11] = new SqlParameter("@ModifiedOn", ModifiedOn);
            objSqlParam[12] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[12].Direction = ParameterDirection.Output;
            objSqlParam[13] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[13].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(DBConnection.ConStr, CommandType.StoredProcedure, "prcSMSOutboundLog_Update", objSqlParam);
            result = Convert.ToInt16(objSqlParam[12].Value);
            Error = Convert.ToString(objSqlParam[13].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return result;
        }*/
        /// <summary>
        /// Delete selected record.
        /// </summary>
        /// <results>bool: True if success</results> 
        /*public bool Delete()
        {
            bool result = false;
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@SMSOutboundLogID", SMSOutboundLogID);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(DBConnection.ConStr, CommandType.StoredProcedure, "prcSMSOutboundLog_Delete", objSqlParam);
            result = (Convert.ToInt16(objSqlParam[1].Value) == 0);
            Error = Convert.ToString(objSqlParam[2].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return result;
        }*/

        /// <summary>
        /// Get All records from database for selected key
        /// </summary>
        /// <results>DataTable: Collection of records</results> 		
        /*public DataTable SelectAll()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[4];
            objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[3].Direction = ParameterDirection.Output;
            DataSet dsResult = SqlHelper.ExecuteDataset(DBConnection.ConStr, CommandType.StoredProcedure, "prcSMSOutboundLog_Select", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            TotalRecords = Convert.ToInt32(objSqlParam[2].Value);
            Error = Convert.ToString(objSqlParam[3].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return dtResult;
        }*/

        /*public DataTable SelectSMSOutboundTrans()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[4];
            //objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
            //objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[3].Direction = ParameterDirection.Output;
            DataSet dsResult = SqlHelper.ExecuteDataset(DBConnection.ConStr, CommandType.StoredProcedure, "prcSMSOutboundTrans_Select", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            TotalRecords = Convert.ToInt32(objSqlParam[2].Value);
            Error = Convert.ToString(objSqlParam[3].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return dtResult;
        }*/

        /// <summary>
        /// Get All records from database for selected key
        /// </summary>
        /// <results>DataTable: Collection of records</results> 		
        /*public DataTable SelectById()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@SMSOutboundLogID", SMSOutboundLogID);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            DataSet dsResult = SqlHelper.ExecuteDataset(DBConnection.ConStr, CommandType.StoredProcedure, "prcSMSOutboundLog_SelectById", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[2].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return dtResult;
        }*/
        /*public DataTable SelectBySMSConfigId(int SMSConfigID)
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@SMSConfigID", SMSConfigID);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            DataSet dsResult = SqlHelper.ExecuteDataset(DBConnection.ConStr, CommandType.StoredProcedure, "prcSearchSMSConfigID_Select", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[2].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return dtResult;
        }
        public DataTable SelectByOutboundTransName(string TransName)
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@SMSConfigID", TransName);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            DataSet dsResult = SqlHelper.ExecuteDataset(DBConnection.ConStr, CommandType.StoredProcedure, "prcSearchTransName_Select", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[2].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return dtResult;
        }
        public Int16 UpdateActive(int SMSConfigID)
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[4];
            objSqlParam[0] = new SqlParameter("@SMSConfigID", SMSConfigID);
            objSqlParam[1] = new SqlParameter("@Active", Active);
            objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[3].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(DBConnection.ConStr, CommandType.StoredProcedure, "prcSMSConfig_ActionUpdate", objSqlParam);
            result = Convert.ToInt16(objSqlParam[2].Value);
            Error = Convert.ToString(objSqlParam[3].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return result;
        }
        public DataTable SearchOutboundSMSList()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[9];
            objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@FromDate", FromDate);
            objSqlParam[5] = new SqlParameter("@ToDate", ToDate);
            objSqlParam[6] = new SqlParameter("@MobileNo", MobileNo);
            objSqlParam[7] = new SqlParameter("@SMSTransNameID", SMSTransNameID);
            objSqlParam[8] = new SqlParameter("@SMSStausID", SMSStatus);
            DataSet dsResult = SqlHelper.ExecuteDataset(DBConnection.ConStr, CommandType.StoredProcedure, "prcSearchOutBoundSMSList_Select", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            TotalRecords = Convert.ToInt32(objSqlParam[2].Value);
            Error = Convert.ToString(objSqlParam[3].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return dtResult;
        }*/
        /// <summary>
        /// Get All records from database for selected key
        /// </summary>
        /// <results>DataTable: Collection of records</results> 		
        /*public void Load()
        {
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@SMSOutboundLogID", SMSOutboundLogID);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            IDataReader reader = SqlHelper.ExecuteReader(DBConnection.ConStr, CommandType.StoredProcedure, "prcSMSOutboundLog_SelectById", objSqlParam);
            while (reader.Read())
            {
                if (reader["SMSOutboundTransID"] != null)
                    SMSOutboundTransID = Convert.ToInt16(reader["SMSOutboundTransID"]);
                if (reader["TransText"] != null)
                    TransText = Convert.ToString(reader["TransText"]);
                if (reader["SMSMobileNumber"] != null)
                    SMSMobileNumber = Convert.ToString(reader["SMSMobileNumber"]);
                if (reader["SendType"] != null)
                    SendType = Convert.ToInt16(reader["SendType"]);
                if (reader["SendText"] != null)
                    SendText = Convert.ToString(reader["SendText"]);
                if (reader["SendTransNo"] != null)
                    SendTransNo = Convert.ToString(reader["SendTransNo"]);
                if (reader["IsDelivered"] != null)
                    IsDelivered = Convert.ToInt16(reader["IsDelivered"]);
                if (reader["DeliveredDate"] != null)
                    DeliveredDate = Convert.ToDateTime(reader["DeliveredDate"]);
                if (reader["SMSExpiryDate"] != null)
                    SMSExpiryDate = Convert.ToDateTime(reader["SMSExpiryDate"]);
                if (reader["RecordCreationDate"] != null)
                    RecordCreationDate = Convert.ToDateTime(reader["RecordCreationDate"]);
                if (reader["ModifiedOn"] != null)
                    ModifiedOn = Convert.ToDateTime(reader["ModifiedOn"]);
            }
            Error = Convert.ToString(objSqlParam[2].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

        }

        public DataTable SelectAllSMSConfigInfo()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[4];
            objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[3].Direction = ParameterDirection.Output;
            DataSet dsResult = SqlHelper.ExecuteDataset(DBConnection.ConStr, CommandType.StoredProcedure, "prcSMSConfiguration_Select", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            TotalRecords = Convert.ToInt32(objSqlParam[2].Value);
            Error = Convert.ToString(objSqlParam[3].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return dtResult;
        }*/

        /// <summary>
        /// Toggle activation of selected record
        /// </summary>
        /*public bool ToggleActivation()
        {
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0]=new SqlParameter("@SMSOutboundLogID",SMSOutboundLogID);
            objSqlParam[1]=new SqlParameter("@Out_Param",SqlDbType.TinyInt, 2); 
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error",SqlDbType.VarChar, 500); 
            objSqlParam[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(DBConnection.ConStr,CommandType.StoredProcedure,"prcSMSOutboundLog_Toggle",objSqlParam);
            result = (Convert.ToInt16(objSqlParam[1].Value) == 0);
            Error = Convert.ToString(objSqlParam[2].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

        }*/

        public DataTable GetUndeliveredSMSListInbound()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.BigInt, 8);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Status", Status);// This will help in picking the data for IsDelivered=0 for inbound or outbound call
            DataSet dsResult = SqlHelper.ExecuteDataset(ConStr, CommandType.StoredProcedure, "prcGetTransNo_Select", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            result = Convert.ToInt32(objSqlParam[0].Value);
            Error = Convert.ToString(objSqlParam[1].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return dtResult;
        }

        public void SMSProcessedUpdateOutBound(string strTransNo, int OutboundLogID)
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[5];
            objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.BigInt, 8);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@strTransNo", strTransNo);
            objSqlParam[3] = new SqlParameter("@OutboundLogID", OutboundLogID);
            objSqlParam[4] = new SqlParameter("@SuccessStatus", IsDelivered);
            //DataSet dsResult = SqlHelper.ExecuteDataset(DBConnection.ConStr, CommandType.StoredProcedure, "prcSMSDelivery_Update", objSqlParam);
            SqlHelper.ExecuteNonQuery(ConStr, CommandType.StoredProcedure, "prcSMSDelivery_UpdateOutbound", objSqlParam);
            result = Convert.ToInt32(objSqlParam[0].Value);
            Error = Convert.ToString(objSqlParam[1].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

        }
        #endregion
    }
}