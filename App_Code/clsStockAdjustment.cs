﻿#region Copyright and page info
/*
============================================================================================================================================
Copyright	: Zed-Axis Technologies, 2012
Author		: Amit Agarwal
Create date	: 14-Feb-2012
Description	: Stock Adjustment
Module      : Inventory
============================================================================================================================================
Change Log:
17-Jun-14, Rakesh Goel , #CC01 - @StockAdjustBackDateValue changed from Tinyint to Int as value more than 255 can be defined in 
 * Application configuration master for this.
15-Jun-2016, Sumit Maurya, #CC02- New property, function created to save stock adjustment for multiple entities.
 * 16-May-2018, Sumit Maurya, #CC03, New property and method created for stock Adjustment (Done for Infocus).
 * 02-Jun-2018, Sumit Maurya, #CC04, Modification further implemented for change log #CC03 (Done for Infocus).
--------------------------------------------------------------------------------------------------------------------------------------------

 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Xml;


namespace DataAccess
{

    public class clsStockAdjustment : IDisposable
    {
        #region Private Class Variables
        private int _intStockAdjustmentID, _intStockAdjustmentForID,
            _intCreatedBy, _intEntityId, _intReasonID, _UserId, _RoleID;
        private string _strstockAdjustmentDetail, _strStockAdjustmentNo, _strRemarks, _strError;
        private DateTime? _dtStockAdjustmentDate, datestockAdjustFromdate, datestockAdjustTodate;
        private Int16 intApprovalStatus, intInterMediateApprovalStatus, _TransactionStatus;
        private Int32 _intPageIndex;
        private Int32 _intPageSize;
        private Int32 _intTotalRecords;
        private short _intEntityTypeID;
        private int _intLoginEntityID;
        public int IntCreatedBy
        {
            get { return _intCreatedBy; }
            set { _intCreatedBy = value; }
        }


        public int IntStockAdjustmentForID
        {
            get { return _intStockAdjustmentForID; }
            set { _intStockAdjustmentForID = value; }
        }
        public int ErrorValue
        {
            get;
            set;
        }
        public int EntityIDLogin
        {
            get
            {
                return _intLoginEntityID;
            }
            set
            {
                _intLoginEntityID = value;
            }
        }
        public int IntStockAdjustmentID
        {
            get { return _intStockAdjustmentID; }
            set { _intStockAdjustmentID = value; }
        }
        private Int16 _shtEntityTypeID;
        private EnumUploadPageName ePageName;
        public Int16 ShtEntityTypeID
        {
            get { return _shtEntityTypeID; }
            set { _shtEntityTypeID = value; }
        }
        public EnumUploadPageName ePageNameUpload
        {
            get { return ePageName; }
            set { ePageName = value; }
        }


        public DateTime? DtStockAdjustmentDate
        {
            get { return _dtStockAdjustmentDate; }
            set { _dtStockAdjustmentDate = value; }
        }

        public int IsFromOpeningStock
        {
            get;
            set;
        }
        public string StrError
        {
            get { return _strError; }
            set { _strError = value; }
        }

        public string StrRemarks
        {
            get { return _strRemarks; }
            set { _strRemarks = value; }
        }

        public string StrStockAdjustmentNo
        {
            get { return _strStockAdjustmentNo; }
            set { _strStockAdjustmentNo = value; }
        }

        public string StrstockAdjustmentDetail
        {
            get { return _strstockAdjustmentDetail; }
            set { _strstockAdjustmentDetail = value; }
        }
        public int EntityId
        {
            get { return _intEntityId; }
            set { _intEntityId = value; }
        }
        public int ReasonID
        {
            get { return _intReasonID; }
            set { _intReasonID = value; }
        }
        public DateTime? stockAdjustFromdate
        {
            get { return datestockAdjustFromdate; }
            set { datestockAdjustFromdate = value; }
        }
        public DateTime? stockAdjustTodate
        {
            get { return datestockAdjustTodate; }
            set { datestockAdjustTodate = value; }
        }
        public Int16 ApprovalStatus
        {
            get { return intApprovalStatus; }
            set { intApprovalStatus = value; }
        }
        public Int16 InterMediateApprovalStatus
        {
            get { return intInterMediateApprovalStatus; }
            set { intInterMediateApprovalStatus = value; }
        }
        public Int32 UserId
        {
            get
            {
                return _UserId;
            }
            set
            {
                _UserId = value;
            }
        }
        public Int32 RoleID
        {
            get
            {
                return _RoleID;
            }
            set
            {
                _RoleID = value;
            }
        }
        public Int16 TransactionStatus
        {
            get
            {
                return _TransactionStatus;
            }
            set
            {
                _TransactionStatus = value;
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
        public Int16 EntitytypeID
        {
            get { return _intEntityTypeID; }
            set { _intEntityTypeID = value; }
        }

        #endregion

        public enum EnumUploadPageName
        {
            StockAdjustment = 0

        }

        /* #CC02 Add Start */
        public int OutParam
        {
            get;
            set;
        }
        /* #CC02 Add End */

        /* #CC03 Add Start */
        public string SessionID
        {
            get;
            set;
        }
        public DateTime? StockAdjustmentDate
        {
            get;
            set;
        }
        /* #CC03 Add End */


        /* #CC04 Add Start */
        public string OriginalFileName
        {
            get;
            set;
        }
        public string UniqueFileName
        {
            get;
            set;
        }

        public Int16? RefType /* BCP Type 0= StockAdjustment */
        {
            get;
            set;
        }

        /* #CC04 Add End */
        public int CompanyID
        {
            get;
            set;
        }
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

        ~clsStockAdjustment()
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
        #region Get Stock Template
        public DataSet GetPartCodeTemplate()
        {
            DataSet dsResult = new DataSet();
            SqlParameter[] objSqlParam = new SqlParameter[1];
            objSqlParam[0] = new SqlParameter("@EntityID", _intEntityId);
            dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcPartCodetemplate_Select", CommandType.StoredProcedure, objSqlParam);
            return dsResult;
        }
        #endregion
        public DataTable GetReason()
        {
            DataTable dtResult = new DataTable();
            dtResult = DataAccess.Instance.GetDataSetFromDatabase("prcReason_Select", CommandType.StoredProcedure).Tables[0];
            return dtResult;

        }

        public DataSet StockAdjustmentLoad(ref int IsStockAdjustBackDateAllowed)
        {
            try
            {
                DataSet dtResult = new DataSet();
                SqlParameter[] objSqlParam = new SqlParameter[3];
                objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 200);
                objSqlParam[0].Direction = ParameterDirection.Output;
                //objSqlParam[1] = new SqlParameter("@StockAdjustBackDateValue", SqlDbType.TinyInt);  #CC01 commented
                objSqlParam[1] = new SqlParameter("@StockAdjustBackDateValue", SqlDbType.Int);  /*#CC01 added*/
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new SqlParameter("@CompanyID", CompanyID);
                dtResult = DataAccess.Instance.GetDataSetFromDatabase("prcStockAdjustmentLoad", CommandType.StoredProcedure, objSqlParam);
                //int b = Convert.ToByte(objSqlParam[1].Value);  #CC01 commented
                int b = Convert.ToInt32(objSqlParam[1].Value);  /*#CC01 added*/
                IsStockAdjustBackDateAllowed = b;
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public DataSet StockAdjustmentLoad1(ref bool IsStockAdjustBackDateAllowed)
        {
            try
            {
                DataSet dtResult = new DataSet();
                SqlParameter[] objSqlParam = new SqlParameter[1];
                objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 200);
                objSqlParam[0].Direction = ParameterDirection.Output;
                dtResult = DataAccess.Instance.GetDataSetFromDatabase("prcEntityTypeMaster_SelectByStockMaintained", CommandType.StoredProcedure, objSqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public DataSet GetStockBinType()
        {
            DataSet dsResult = new DataSet();
            dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcStockBinType_Select", CommandType.StoredProcedure);
            return dsResult;

        }
        public DataTable GetStockAdjustmentDetail()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[12];
            objSqlParam[0] = new SqlParameter("@FromDate", datestockAdjustFromdate);
            objSqlParam[1] = new SqlParameter("@Todate", datestockAdjustTodate);
            objSqlParam[2] = new SqlParameter("@stockAdjustmentId", _intStockAdjustmentID);
            objSqlParam[3] = new SqlParameter("@AprovalStatus", ApprovalStatus);
            objSqlParam[6] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[6].Direction = ParameterDirection.Output;
            objSqlParam[7] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[8] = new SqlParameter("@PageSize", PageSize);

            objSqlParam[9] = new SqlParameter("@EntitytypeID", EntitytypeID);
            objSqlParam[10] = new SqlParameter("@EntityID", EntityId);
            objSqlParam[11] = new SqlParameter("@EntityOfLoginID", EntityIDLogin);
            dtResult = DataAccess.Instance.GetDataSetFromDatabase("prcStockAdjustment_Select", CommandType.StoredProcedure, objSqlParam).Tables[0];
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                if (objSqlParam[6].Value != DBNull.Value)
                {
                    TotalRecords = Convert.ToInt32(objSqlParam[6].Value);
                }
            }
            return dtResult;

        }
        public DataSet SelectEntityNameforOpeningStock()
        {
            DataSet dsResult = new DataSet();
            SqlParameter[] objSqlParam = new SqlParameter[4];
            objSqlParam[0] = new SqlParameter("@LoggedEntityID", EntityId);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@LoggedInEntityTypeID", EntitytypeID);
            DataSet DataSet = DataAccess.Instance.GetDataSetFromDatabase("prcEntityNameForOpeningStock_Select", CommandType.StoredProcedure, objSqlParam);
            Error = Convert.ToString(objSqlParam[2].Value);
            return DataSet;
        }
        #region InsertUpdateStockAdjustment

        public DataTable XML_PartDetails { get; set; }

        public int SalesChannelTypeID { get; set; }

        public int Save()
        {
            string StrstockAdjustmentDetailReturn = StrstockAdjustmentDetail;
            try
            {
                int IntResultCount = 0;
                SqlParameter[] SqlParam = new SqlParameter[12];
                SqlParam[0] = new SqlParameter("@StockAdjustmentID", IntStockAdjustmentID);
                SqlParam[0].Direction = ParameterDirection.InputOutput;
                SqlParam[1] = new SqlParameter("@StockAdjustmentForID", IntStockAdjustmentForID);
                SqlParam[2] = new SqlParameter("@StockAdjustmentDate", DtStockAdjustmentDate);
                SqlParam[3] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[3].Direction = ParameterDirection.InputOutput;

                SqlParam[4] = new SqlParameter("@Remarks", StrRemarks);

                SqlParam[5] = new SqlParameter("@CreatedBy", _intCreatedBy);
                SqlParam[6] = new SqlParameter("@ReasonId", _intReasonID);
                SqlParam[7] = new SqlParameter("@StrstockAdjustmentDetailReturn", SqlDbType.Xml);
                SqlParam[7].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(StrstockAdjustmentDetailReturn, XmlNodeType.Document, null));
                SqlParam[7].Direction = ParameterDirection.Output;

                SqlParam[8] = new SqlParameter("@PartInfoExcel", SqlDbType.Structured);
                SqlParam[8].Value = XML_PartDetails;

                SqlParam[9] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[9].Direction = ParameterDirection.Output;

                SqlParam[10] = new SqlParameter("@IsFromOpeningStock", IsFromOpeningStock);

                SqlParam[11] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);

                IntResultCount = DataAccess.Instance.DBInsertCommand("prcStockAdjustmentSave", SqlParam);
                IntStockAdjustmentID = Convert.ToInt32((SqlParam[0].Value == DBNull.Value ? 0 : SqlParam[0].Value));

                if (((System.Data.SqlTypes.SqlXml)SqlParam[7].Value).IsNull != true)
                {
                    StrstockAdjustmentDetail = ((System.Data.SqlTypes.SqlXml)SqlParam[7].Value).Value;

                }
                else
                {
                    StrstockAdjustmentDetail = null;
                }
                StrError = SqlParam[3].Value.ToString();
                ErrorValue = Convert.ToInt32(SqlParam[9].Value);

                return IntResultCount;
            }

            catch (Exception ex)
            {

                throw ex;
            }

        }
        public Int32 UpdateApprovalStatus()
        {
            try
            {
                Int32 Result = 0;
                SqlParameter[] objSqlParam = new SqlParameter[8];
                objSqlParam[0] = new SqlParameter("@StockAdjustmentId", _intStockAdjustmentID);
                objSqlParam[1] = new SqlParameter("@FinalApprovalStatus", intApprovalStatus);
                objSqlParam[2] = new SqlParameter("@IntermediateApprovalStatus", intInterMediateApprovalStatus);
                objSqlParam[3] = new SqlParameter("@UserId", _UserId);
                objSqlParam[4] = new SqlParameter("@RoleID", _RoleID);
                //objSqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                //objSqlParam[5].Direction = ParameterDirection.Output;
                objSqlParam[5] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 200);
                objSqlParam[5].Direction = ParameterDirection.Output;
                objSqlParam[6] = new SqlParameter("@Transactionstatus", _TransactionStatus);
                objSqlParam[7] = new SqlParameter("@StrXMLDetail", SqlDbType.Xml);
                objSqlParam[7].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(_strstockAdjustmentDetail, XmlNodeType.Document, null));
                objSqlParam[7].Direction = ParameterDirection.InputOutput;

                Result = DataAccess.Instance.DBInsertCommand("prcStockAdjustmentApproval_Update", objSqlParam);
                //Result=Convert.ToInt16(objSqlParam[5].Value);
                ////Error = Convert.ToString(objSqlParam[6].Value);

                if (((System.Data.SqlTypes.SqlXml)objSqlParam[7].Value).IsNull != true)
                {
                    StrstockAdjustmentDetail = ((System.Data.SqlTypes.SqlXml)objSqlParam[7].Value).Value;
                }
                else
                {
                    StrstockAdjustmentDetail = null;
                }
                StrError = objSqlParam[5].Value.ToString();
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string CompanyName { get; set; }
        public DateTime? DocketDate { get; set; }
        public string DocketNo { get; set; }
        public byte ModeOfReceipt { get; set; }
        public string DispatchReferenceNo { get; set; }
        public int GrnByEntityID { get; set; }
        public int GrnFromEntityID { get; set; }
        public string Remarks { get; set; }


        public int SaveGRN()
        {
            string StrstockAdjustmentDetailReturn = StrstockAdjustmentDetail;
            try
            {

                int IntResultCount = 0;
                SqlParameter[] SqlParam = new SqlParameter[14];
                SqlParam[0] = new SqlParameter("@StrstockAdjustmentDetail", SqlDbType.Xml);
                SqlParam[0].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(StrstockAdjustmentDetail, XmlNodeType.Document, null));
                SqlParam[0].Direction = ParameterDirection.Input;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.InputOutput;
                SqlParam[2] = new SqlParameter("@CreatedBy", _intCreatedBy);
                SqlParam[3] = new SqlParameter("@StrstockAdjustmentDetailReturn", SqlDbType.Xml);
                SqlParam[3].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(StrstockAdjustmentDetailReturn, XmlNodeType.Document, null));
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@GrnByEntityID", GrnByEntityID);
                SqlParam[5] = new SqlParameter("@GrnFromEntityID", GrnFromEntityID);
                SqlParam[6] = new SqlParameter("@DocketNo", DocketNo);
                SqlParam[7] = new SqlParameter("@DocketDate", DocketDate);
                SqlParam[8] = new SqlParameter("@ModeOfReceipt", ModeOfReceipt);
                SqlParam[9] = new SqlParameter("@DispatchReferenceNo", DispatchReferenceNo);
                SqlParam[10] = new SqlParameter("@CompanyName", CompanyName);
                SqlParam[11] = new SqlParameter("@ReceiveRemarks", Remarks);
                SqlParam[12] = new SqlParameter("@StockAdjustmentForID", IntStockAdjustmentForID);
                SqlParam[13] = new SqlParameter("@StockReceiveNo", SqlDbType.NVarChar, 30);
                SqlParam[13].Direction = ParameterDirection.Output;
                IntResultCount = DataAccess.Instance.DBInsertCommand("prcDirectGRN_Insert", SqlParam);
                if (((System.Data.SqlTypes.SqlXml)SqlParam[3].Value).IsNull != true)
                {
                    StrstockAdjustmentDetail = ((System.Data.SqlTypes.SqlXml)SqlParam[3].Value).Value;

                }
                else
                {
                    StrstockAdjustmentDetail = null;
                }
                StrError = SqlParam[1].Value.ToString();
                StrStockAdjustmentNo = SqlParam[13].Value.ToString();

                return IntResultCount;
            }

            catch (Exception ex)
            {

                throw ex;
            }

        }

        /* #CC02 Add Start */
        public DataSet SaveStockAdjustment()
        {
            try
            {
                SqlParameter[] SqlParam = new SqlParameter[8];
                SqlParam[0] = new SqlParameter("@StockAdjustmentDate", DtStockAdjustmentDate);
                SqlParam[1] = new SqlParameter("@Remarks", StrRemarks);
                SqlParam[2] = new SqlParameter("@CreatedBy", _intCreatedBy);
                SqlParam[3] = new SqlParameter("@ReasonId", ReasonID);
                SqlParam[4] = new SqlParameter("@PartInfoExcelNew", SqlDbType.Structured);
                SqlParam[4].Value = XML_PartDetails;
                SqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[5].Direction = ParameterDirection.Output;
                SqlParam[6] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
                SqlParam[7] = new SqlParameter("@CompanyID", CompanyID);
                DataSet dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcBulkStockAdjustmentSave", CommandType.StoredProcedure, SqlParam);
                OutParam = Convert.ToInt16(SqlParam[5].Value);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /* #CC02 Add End */



        /* #CC03 Add Start */
        public DataSet SaveStockAdjustmentV2()
        {
            try
            {
                string strcon = System.Configuration.ConfigurationManager.ConnectionStrings["AppConString"].ConnectionString;
                SqlConnection objCon = new SqlConnection(strcon);//#CC15 added
                SqlParameter[] SqlParam = new SqlParameter[12];  /* #CC04 length increased from 8 to 11 */
                SqlParam[0] = new SqlParameter("@SessionID", SessionID);
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.Int);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 2000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                SqlParam[4] = new SqlParameter("@Remarks", Remarks);
                SqlParam[5] = new SqlParameter("@StockAdjustmentDate", StockAdjustmentDate);
                SqlParam[6] = new SqlParameter("@ReasonID", ReasonID);
                SqlParam[7] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
                /* #CC04 Add Start */
                SqlParam[8] = new SqlParameter("@OriginalFileName", OriginalFileName);
                SqlParam[9] = new SqlParameter("@UniqueFileName", UniqueFileName);
                SqlParam[10] = new SqlParameter("@RefType", RefType);
                SqlParam[11] = new SqlParameter("@CompanyId", CompanyID);
                /* #CC04 Add End */
                /*
                DataSet dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcBulkStockAdjustmentSaveV2", CommandType.StoredProcedure, SqlParam);
                */
                DataSet dsResult = new DataSet();
                SqlCommand objComm = new SqlCommand("prcBulkStockAdjustmentSaveV2", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 2000;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }
                OutParam = Convert.ToInt16(SqlParam[1].Value);
                Error = Convert.ToString(SqlParam[2].Value);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /* #CC03 Add End */


        #endregion
    }
}