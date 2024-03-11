using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using DataAccess;
using System.Data.SqlClient;
using System.Xml;
/*
* 30-Oct-2018, Rakesh Raj, #CC01,  This is used to manage Inbound Tertiary Sale from URL
*/
namespace DataAccess
{
    public class SMSData : IDisposable
    {

        private string strError, strMobileNumber, strMessageText, strCircleName;
        SqlParameter[] objSqlParam;
        DataTable dtResult;
        Int32 IntResultCount = 0;

        public string Error
        {
            get { return strError; }
            set { strError = value; }
        }
        public string MobileNumber
        {
            get { return strMobileNumber; }
            set { strMobileNumber = value; }
        }
        public string MessageText
        {
            get { return strMessageText; }
            set { strMessageText = value; }
        }
        public string CircleName
        {
            get { return strCircleName; }
            set { strCircleName = value; }
        }
        //Property for SMSMaster
        private string _SMSKeyword;
        public string SMSKeyword
        {
            get { return _SMSKeyword; }
            set { _SMSKeyword = value; }
        }
        private string _SMSDesc;
        public string SMSDesc
        {
            get { return _SMSDesc; }
            set { _SMSDesc = value; }
        }
        private string _SMSFrom;
        public string SMSFrom
        {
            get { return _SMSFrom; }
            set { _SMSFrom = value; }
        }
        private string _SMSContent;
        public string SMSContent
        {
            get { return _SMSContent; }
            set { _SMSContent = value; }
        }
        private int _SMSExpiry;
        public int SMSExpiry
        {
            get { return _SMSExpiry; }
            set { _SMSExpiry = value; }
        }
        private int _Status;
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        private int _SMSID;
        public int SMSID
        {
            get { return _SMSID; }
            set { _SMSID = value; }
        }
        private string _InsError;
        public string InsError
        {
            get { return _InsError; }
            set { _InsError = value; }
        }
        public string SerialNumber
        {
            get;
            set;
        }

        public string SerialNumberWithModelContent
        {
            get;
            set;
        }
        public void insertSMSLog()
        {
            try
            {
                objSqlParam = new SqlParameter[3];
                objSqlParam[0] = new SqlParameter("@MobileNumber", strMobileNumber);
                objSqlParam[1] = new SqlParameter("@MessageText", strMessageText);
                objSqlParam[2] = new SqlParameter("@Circle", strCircleName);
                IntResultCount = DataAccess.Instance.DBInsertCommand("prcInsUpdSMSInfomation", objSqlParam);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (objSqlParam != null)
                {
                    objSqlParam = null;
                }
            }
        }
        public void SmsLogParser()
        {
            try
            {
                IntResultCount = DataAccess.Instance.DBInsertCommand("PrcUpdSmsLogParser", CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (objSqlParam != null)
                {
                    objSqlParam = null;
                }
            }
        }
        //Code for SMSMaster
        public void InsSMSMaster()
        {
            try
            {
                objSqlParam = new SqlParameter[9];
                objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                objSqlParam[0].Direction = ParameterDirection.Output;
                objSqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new SqlParameter("@SMSKeyword", SMSKeyword);
                objSqlParam[3] = new SqlParameter("@SMSDesc", SMSDesc);
                objSqlParam[4] = new SqlParameter("@SMSFrom", SMSFrom);
                objSqlParam[5] = new SqlParameter("@SMSContent", SMSContent);
                objSqlParam[6] = new SqlParameter("@SMSExpiry", SMSExpiry);
                objSqlParam[7] = new SqlParameter("@Status", Status);
                objSqlParam[8] = new SqlParameter("@Insinfo", SqlDbType.VarChar, 50);
                objSqlParam[8].Direction = ParameterDirection.Output;
                DataAccess.Instance.DBInsertCommand("prcInsSMSMaster", objSqlParam);
                if (objSqlParam[8].Value.ToString() != "")
                {
                    InsError = objSqlParam[8].Value.ToString();
                }
                else { InsError = null; }
                Error = Convert.ToString(objSqlParam[1].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetSMSMaster(int condition)
        {
            try
            {
                objSqlParam = new SqlParameter[5];
                objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                objSqlParam[0].Direction = ParameterDirection.Output;
                objSqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new SqlParameter("@SMSDesc", SMSDesc);
                objSqlParam[3] = new SqlParameter("@SMSID", SMSID);
                objSqlParam[4] = new SqlParameter("@Condition", condition);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetSMSMaster", CommandType.StoredProcedure, objSqlParam);
                Error = Convert.ToString(objSqlParam[1].Value);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdSMSMaster(int condition)
        {
            try
            {
                objSqlParam = new SqlParameter[11];
                objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                objSqlParam[0].Direction = ParameterDirection.Output;
                objSqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new SqlParameter("@SMSKeyword", SMSKeyword);
                objSqlParam[3] = new SqlParameter("@SMSDesc", SMSDesc);
                objSqlParam[4] = new SqlParameter("@SMSFrom", SMSFrom);
                objSqlParam[5] = new SqlParameter("@SMSContent", SMSContent);
                objSqlParam[6] = new SqlParameter("@SMSExpiry", SMSExpiry);
                objSqlParam[7] = new SqlParameter("@Status", Status);
                objSqlParam[8] = new SqlParameter("@condition", condition);
                objSqlParam[9] = new SqlParameter("@SMSID", SMSID);
                objSqlParam[10] = new SqlParameter("@Insinfo", SqlDbType.VarChar, 50);
                objSqlParam[10].Direction = ParameterDirection.Output;
                DataAccess.Instance.DBInsertCommand("prcUpdSMSMaster", objSqlParam);
                if (objSqlParam[10].Value.ToString() != "")
                {
                    InsError = objSqlParam[10].Value.ToString();
                }
                else { InsError = null; }
                Error = Convert.ToString(objSqlParam[1].Value);  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Int32 InsertTertiorySalesThruSMS()
        {
            try
            {
                Int32 result;
                objSqlParam = new SqlParameter[5];
                objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                objSqlParam[0].Direction = ParameterDirection.Output;
                objSqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new SqlParameter("@SMSMobileNumber", MobileNumber);
                //objSqlParam[3] = new SqlParameter("@SMSSerialNumber", SerialNumber);
                objSqlParam[3] = new SqlParameter("@SerialNumberWithModelContent", SerialNumberWithModelContent);
                objSqlParam[4] = new SqlParameter("@SMSContent", SMSContent);
                DataAccess.Instance.DBInsertCommand("prcInsertTertiorySalesThruSMS", objSqlParam);
                if (objSqlParam[1].Value != System.DBNull.Value && objSqlParam[1].Value.ToString() != "")
                {
                    Error = Convert.ToString(objSqlParam[1].Value);
                }
                else
                    Error = null;
                result = Convert.ToInt32(objSqlParam[0].Value);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //#CC01 Start
        public Int32 InboundTertiarySale()
        {
            try
            {
                Int32 result;
                objSqlParam = new SqlParameter[4];
                objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                objSqlParam[0].Direction = ParameterDirection.Output;
                objSqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new SqlParameter("@RetailerCode", MobileNumber);
                objSqlParam[3] = new SqlParameter("@IMEICommaSeperated", SerialNumberWithModelContent);

                DataAccess.Instance.DBInsertCommand("prcInsertInboundTertiarySale", objSqlParam);
                if (objSqlParam[1].Value != System.DBNull.Value && objSqlParam[1].Value.ToString() != "")
                {
                    Error = Convert.ToString(objSqlParam[1].Value);
                }
                else
                    Error = null;
                result = Convert.ToInt32(objSqlParam[0].Value);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //#CC01 End

        # region dispose
        //Call Dispose to free resources explicitly
        private bool IsDisposed = false;
        public void Dispose()
        {
            //Pass true in dispose method to clean managed resources too and say GC to skip finalize 
            // in next line.
            Dispose(true);
            //If dispose is called already then say GC to skip finalize on this instance.
            GC.SuppressFinalize(this);
        }

        ~SMSData()
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
