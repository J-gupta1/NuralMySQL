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


namespace DataAccess
{
    public class SapService
    {
        
        private string strError,strServiceDocNo,strErrorDetailXML,
            strModuleName, strSapServiceMethodName, strServiceDocNumber,strSapFileName, strStatusValue, strXMLData, strMessageDetail;
        public static string NoFileExistDownLoad = "There is no file Exist for Downloading.", ExceptionOccured = "Exception has Occured During the Process.", DownLoadFailed = "Downloading of File Failed.",
            NoFileExistUploading = "There is no File Exist For uploading.", ConnectionFailed = "Connection Could not be Established.";
        private int intLogType;
        DataTable dtResult;
        SqlParameter[] objSqlParam;
        Int32 IntResultCount = 0;
        public string Error
        {
            get { return strError; }
            set { strError = value; }
        }
        #region Service Trace private properties
        public string ErrorDetailXML
        {
            get { return strErrorDetailXML; }
            set { strErrorDetailXML = value; }
        }
        public string SapServiceMethodName
        {
            get { return strSapServiceMethodName; }
            set { strSapServiceMethodName = value; }
        }
        public string SapFileName
        {
            get { return strSapFileName; }
            set { strSapFileName = value; }
        }
        public string ServiceDocNumber
        {
            get { return strServiceDocNumber; }
            set { strServiceDocNumber = value; }
        }
        public string StatusValue
        {
            get { return strStatusValue; }
            set { strStatusValue = value; }
        }
        public string MessageDetail
        {
            get { return strMessageDetail; }
            set { strMessageDetail = value; }
        }
        public string ServiceDocNo
        {
            get { return strServiceDocNo; }
            set { strServiceDocNo = value; }
        }
        public string XMLData
        {
            get { return strXMLData; }
            set { strXMLData = value; }
        }

        public EnumData.EnumSAPModuleName ModuleName
        {
            get;
            set;
        }
        public int LogType
        {
            get { return intLogType; }
            set { intLogType = value; }
        }
        #endregion
        //# region dispose
        ////Call Dispose to free resources explicitly
        //private bool IsDisposed = false;
        //public void Dispose()
        //{
        //    //Pass true in dispose method to clean managed resources too and say GC to skip finalize 
        //    // in next line.
        //    Dispose(true);
        //    //If dispose is called already then say GC to skip finalize on this instance.
        //    GC.SuppressFinalize(this);
        //}

        //~SapService()
        //{
        //    //Pass false as param because no need to free managed resources when you call finalize it
        //    //  will be done
        //    //by GC itself as its work of finalize to manage managed resources.
        //    Dispose(false);
        //}

        ////Implement dispose to free resources
        //protected virtual void Dispose(bool disposedStatus)
        //{
        //    if (!IsDisposed)
        //    {
        //        IsDisposed = true;
        //        // Released unmanaged Resources
        //        if (disposedStatus)
        //        {
        //            // Released managed Resources
        //        }
        //    }
        //}

        //#endregion
        #region Trace Service Log
       public void insertServiceTraceLog()
        {
            try
            {
                objSqlParam = new SqlParameter[8];
                objSqlParam[0] = new SqlParameter("@ModuleName", ModuleName.ToString());
                objSqlParam[1] = new SqlParameter("@SapServiceMethodName", SapServiceMethodName);
                objSqlParam[2] = new SqlParameter("@StatusValue", StatusValue);
                objSqlParam[3] = new SqlParameter("@MessageDetail", MessageDetail);
                objSqlParam[4] = new SqlParameter("@XMLDATA", XMLData);
                objSqlParam[5] = new SqlParameter("@LogType", LogType);
                objSqlParam[6] = new SqlParameter("@ServiceDocNo", ServiceDocNumber);
                objSqlParam[7] = new SqlParameter("@SapFileName", SapFileName);
                IntResultCount = DataAccess.Instance.DBInsertCommand("prcTraceSapServiceLog", objSqlParam);

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
       public DataTable GetServiceTraceLogForMail()
       {
           try
           {
               dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetServiceTraceLogForMailOrupdate", CommandType.StoredProcedure);
               return dtResult;

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
       public void UpdateServiceTraceLogForMail(string ID)
       {
           try
           {
               objSqlParam = new SqlParameter[1];
               objSqlParam[0] = new SqlParameter("@ID", ID);
               IntResultCount = DataAccess.Instance.DBInsertCommand("prcGetServiceTraceLogForMailOrupdate", objSqlParam);

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
       public Int16 InsertVourcherInfoUpload(DataTable Dt)
       {
           Int16 IntResultCount = 0;
           try
           {
               objSqlParam = new SqlParameter[4];
               objSqlParam[0] = new SqlParameter("@tvpVoucher", SqlDbType.Structured);
               objSqlParam[0].Value = Dt;
               objSqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
               objSqlParam[1].Direction = ParameterDirection.Output;
               objSqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
               objSqlParam[2].Direction = ParameterDirection.Output;
               objSqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
               objSqlParam[3].Direction = ParameterDirection.Output;
               DataAccess.Instance.DBInsertCommand("PrcInsVoucherInfoUpload", objSqlParam);
               IntResultCount = Convert.ToInt16(objSqlParam[3].Value);
               if (objSqlParam[2].Value != DBNull.Value)
               {
                   ErrorDetailXML = objSqlParam[2].Value.ToString();
               }
               else
               {
                   ErrorDetailXML = null;
               }
               if (objSqlParam[1].Value != DBNull.Value)
               {
                   Error = objSqlParam[1].Value.ToString();
               }
               return IntResultCount;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
        #endregion
    }
}
