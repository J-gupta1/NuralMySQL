using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
/*
 ============================================================================================================================================                                    
Change Log:                                    
dd-MMM-yy, Name, #CCx - Description                                    
--------------------------------------------------------------------------------------------------------------------------------------------                                    
20-Dec-13,Pankaj Mittal,#cc01 - Provide unlock ISP facility
 * 22 Jan 2015, Karam Chand Sharma, #CC02, create InValidSerialsReturn function for serail no check according to the invoice no
 * 27-Jul-2016, Sumit Maurya, #CC03, New paramenter with value supplied to validate serial.
 * 28-Apr-2018, Sumit Maurya, #CC04, Changes according to ZedSalesV5.
 * 02-Oct-2018, Sumit Maurya, #CC05, Search parameter provided in ISD search function (Done for motorola).
 * 26-Dec-2018, Sumit Maurya, #CC06, New functions created to for ISD (Done for ZedsalesV5).
 * 27-Dec-2018, Sumit Maurya, #CC07, New functions and properties created to for ISD (Done for ZedsalesV5).
 * 21-April-2020,Vijay Kumar Prajapati,#CC08,Added CompanyId in Query.
 */
namespace DataAccess
{
    public class BeautyAdvisorData : IDisposable
    {
        #region Private Class Variables
        private int intRetailerID, intAreaPositionID, intISPID;
        private string strISPName, strISPCode, strMobile;
        private Int32 intCompanyID;
        private bool blnStatus;



        #endregion
        #region Public Properties
        public int ISPID
        {
            get { return intISPID; }
            set { intISPID = value; }
        }
        public string ISPName
        {
            get { return strISPName; }
            set { strISPName = value; }
        }
        public string ISPCode
        {
            get { return strISPCode; }
            set { strISPCode = value; }
        }
        public int RetailerID
        {
            get { return intRetailerID; }
            set { intRetailerID = value; }
        }
        public string Mobile
        {
            get { return strMobile; }
            set { strMobile = value; }
        }

        public Int32 CompanyID
        {
            get { return intCompanyID; }
            set { intCompanyID = value; }
        }
        public bool Status
        {
            get { return blnStatus; }
            set { blnStatus = value; }
        }
        public int AreaPositionID
        {
            get { return intAreaPositionID; }
            set { intAreaPositionID = value; }
        }
        public Int32 ActiveStatus { get; set; }
        #endregion
        SqlParameter[] SqlParam;
        Int32 IntResultCount = 0;
        DataTable dtISPInfo;
        private DateTime _FromDate = DateTime.Now.Date;
        public DateTime FromDate
        {
            get
            {
                return _FromDate;
            }
            set
            {
                _FromDate = value;
            }
        }
        private DateTime _EndDate = DateTime.Now.Date;
        public DateTime EndDate
        {
            get
            {
                return _EndDate;
            }
            set
            {
                _EndDate = value;
            }
        }


        public DateTime? EffectiveFromDate
        {
            get;
            set;
        }
        public DateTime? EffectiveEndDate
        {
            get;
            set;
        }

        private string _UserName = string.Empty;
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;
            }
        }

        private string _Password = string.Empty;
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
            }
        }
        public int CreateLoginOrNot
        {
            get;
            set;
        }
        public int PasswordExpiryDays
        {
            get;
            set;
        }
        public string PasswordSalt
        {
            get;
            set;
        }
        private string strEmail = string.Empty;
        public string Email
        {
            get { return strEmail; }
            set { strEmail = value; }
        }
        private Int32 _LoginId = 0;
        public Int32 LoginId
        {
            get { return _LoginId; }
            set { _LoginId = value; }
        }

        public int Userid
        {
            get;
            set;
        }
        public Int16 Result
        {
            get;
            set;
        }
        #region "ISP InsertUpdate Statement"

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        public Int32 ISPSalary
        {
            get;
            set;
        }
        public int ComingFrom
        {
            get;
            set;
        }
        public int ModeISPSalaryInfo
        {
            get;
            set;
        }

        public string ErrorDetailXML
        {
            get;
            set;
        }
        public string TransUploadSession
        {
            get;
            set;
        }
        public int ISPSalaryid
        {
            get;
            set;
        }
        //    public string ErrorMessage { get; set; }


        /* #CC07 Add Start */
        DataSet dsResult;
        public Int16 UploadType
        {
            get;
            set;
        }
        public int intOutParam
        {
            get;
            set;
        }
        public string StoreCode
        {
            get;
            set;
        }

        public string SessionID
        {
            get;
            set;
        }
        /* #CC07 Add End */
        public Int32 InsertUpdateISPinfo()
        {

            try
            {
                SqlParam = new SqlParameter[16]; /* #CC04 Length increased from 14-15 */ /* #CC07 length increased from 15-16*/
                SqlParam[0] = new SqlParameter("@ISPID", intISPID);
                SqlParam[0].Direction = ParameterDirection.InputOutput;
                SqlParam[1] = new SqlParameter("@ISPCode", strISPCode);
                SqlParam[2] = new SqlParameter("@ISPName", strISPName);
                SqlParam[3] = new SqlParameter("@Mobile", strMobile);
                SqlParam[4] = new SqlParameter("@CompanyID", intCompanyID);
                SqlParam[5] = new SqlParameter("@RetailerID", intRetailerID);
                SqlParam[6] = new SqlParameter("@FromDate", _FromDate);
                SqlParam[7] = new SqlParameter("@UserName", _UserName);
                SqlParam[8] = new SqlParameter("@Password", _Password);
                SqlParam[9] = new SqlParameter("@CreateLoginOrNot", CreateLoginOrNot);
                SqlParam[10] = new SqlParameter("@PasswordExpiryDays", PasswordExpiryDays);
                SqlParam[11] = new SqlParameter("@PasswordSalt", PasswordSalt);
                SqlParam[12] = new SqlParameter("@Email", strEmail);

                SqlParam[13] = new SqlParameter("@OutMassege", SqlDbType.NVarChar, 200, ErrorMessage);
                SqlParam[13].Direction = ParameterDirection.Output;
                SqlParam[14] = new SqlParameter("@UserID", Userid); /* #CC04 Added */
                SqlParam[15] = new SqlParameter("@StoreCode", StoreCode); /* #CC07 Added */
                DataAccess.Instance.DBInsertCommand("prcInsUpdISPInfo", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[0].Value);
                ErrorMessage = Convert.ToString(SqlParam[13].Value);

                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet InsertISPSalary()//this method will do the insertion/updation/Modification and insertion from the interface as well
        {
            try
            {
                DataSet ds;
                SqlParam = new SqlParameter[9];
                SqlParam[0] = new SqlParameter("@TransUploadSession", TransUploadSession);
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;//this is just for future
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", Userid);
                SqlParam[5] = new SqlParameter("@Mode", ModeISPSalaryInfo);
                SqlParam[6] = new SqlParameter("@ComingFrom", ComingFrom);
                SqlParam[7] = new SqlParameter("@ActivationDate", EffectiveFromDate);
                SqlParam[8] = new SqlParameter("@ISPSalaryid", ISPSalaryid);
                // DataAccess.Instance.DBInsertCommand("prcInsertISPSalary", SqlParam);
                ds = DataAccess.Instance.GetDataSetFromDatabase("prcInsertISPSalary", CommandType.StoredProcedure, SqlParam);
                Result = Convert.ToInt16(SqlParam[1].Value);
                if (SqlParam[3].Value.ToString() != "")
                {
                    ErrorDetailXML = SqlParam[3].Value.ToString();
                }
                else
                {
                    ErrorDetailXML = null;
                }
                Error = Convert.ToString(SqlParam[2].Value);

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectISPSalaryList()
        {
            try
            {
                DataTable dt;
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@ISPName", ISPName);
                SqlParam[1] = new SqlParameter("@ISPCode", ISPCode);
                SqlParam[2] = new SqlParameter("@EffectiveFromDate", EffectiveFromDate);
                SqlParam[3] = new SqlParameter("@EffectiveEndDate", EffectiveEndDate);
                SqlParam[4] = new SqlParameter("@ISPSalaryId", ISPSalaryid);
                dt = DataAccess.Instance.GetTableFromDatabase("prcGetISPSalaryDetails", CommandType.StoredProcedure, SqlParam);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion
        #region Fetch ISP Information
        public DataTable GetISPInformation()
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@CompanyID", intCompanyID);
                dtISPInfo = DataAccess.Instance.GetDataSetFromDatabase("prcGetISPInfo", CommandType.StoredProcedure, SqlParam).Tables[0];
                return dtISPInfo;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataSet GetISPList()
        {
            try
            {
                DataSet ds;
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@ISPID", ISPID);
                SqlParam[1] = new SqlParameter("@ISPName", ISPName);
                SqlParam[2] = new SqlParameter("@UserId", LoginId);
                ds = DataAccess.Instance.GetDataSetFromDatabase("prcGetISPList", CommandType.StoredProcedure, SqlParam);
                return ds;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable GetISPInformation(Int16 intCompanyID, int intAreaPositionID)
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@CompanyID", intCompanyID);
                SqlParam[1] = new SqlParameter("@AreaPositionID", intAreaPositionID);
                dtISPInfo = DataAccess.Instance.GetDataSetFromDatabase("prcGetISPInfo", CommandType.StoredProcedure, SqlParam).Tables[0];
                return dtISPInfo;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable GetISPInformation(Int32 intISPID)
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@ISPID", intISPID);
                SqlParam[1] = new SqlParameter("@CompanyID", intCompanyID);
                dtISPInfo = DataAccess.Instance.GetDataSetFromDatabase("prcGetISPInfo", CommandType.StoredProcedure, SqlParam).Tables[0];
                return dtISPInfo;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable GetISPInformation(Int32 RetailerID, string ISPName)
        {
            try
            {
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@CompanyID", intCompanyID);
                SqlParam[1] = new SqlParameter("@RetailerID", RetailerID);
                SqlParam[2] = new SqlParameter("@ISPName", ISPName);

                dtISPInfo = DataAccess.Instance.GetDataSetFromDatabase("prcGetISPInfo", CommandType.StoredProcedure, SqlParam).Tables[0];
                return dtISPInfo;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion
        #region Delete UpdateStatus


        public Int32 DeleteUpdateStatusISPInfo(Int16 DeleteUpdate)
        {
            try
            {

                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@ISPID", intISPID);
                SqlParam[1] = new SqlParameter("@DeleteUpdate", DeleteUpdate);
                IntResultCount = DataAccess.Instance.DBInsertCommand("PrcDelUpdStatusISP", SqlParam);
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion
        /*#cc01 unlock ISP added (ISPMasterInterface) : Code start*/

        #region Unlock ISP

        public Int32 UnlockISP()
        {
            try
            {

                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@ISPID", intISPID);
                IntResultCount = DataAccess.Instance.DBInsertCommand("PrcISPUnlock", SqlParam);
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion
        /*#cc01 unlock ISP added : Code END*/


        #region Check ISP Existance
        public Int32 CheckISPMasterExistence()
        {
            try
            {
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@ISPID", intISPID);

                IntResultCount = Convert.ToInt32(DataAccess.Instance.getSingleValues("PrcChkISPMasterExistence", SqlParam));
                return IntResultCount;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion
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

        ~BeautyAdvisorData()
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

        private byte _RecordStatus = 2;
        public byte RecordStatus
        {
            get
            {
                return _RecordStatus;
            }
            set
            {
                _RecordStatus = value;
            }
        }

        private Int32 _TotalRecords = 0;
        public Int32 TotalRecords
        {
            get
            {
                return _TotalRecords;
            }
            set
            {
                _TotalRecords = value;
            }
        }
        private int _PageIndex = 0;
        public int PageIndex
        {
            get
            {
                return _PageIndex;
            }
            set
            {
                _PageIndex = value;
            }
        }
        private int _PageSize = 10;
        public int PageSize
        {
            get
            {
                return _PageSize;
            }
            set
            {
                _PageSize = value;
            }
        }

        public DataTable ISP_Select()
        {
            try
            {
                SqlParam = new SqlParameter[9]; /* #CC05 Length increased from 7 to 8 */ /* #CC07 length increased from 8 to 9 */
                SqlParam[0] = new SqlParameter("@ISPID", intISPID);
                SqlParam[1] = new SqlParameter("@Status", _RecordStatus);
                SqlParam[2] = new SqlParameter("@ISPName", ISPName);
                SqlParam[3] = new SqlParameter("@RetailerID", RetailerID);
                SqlParam[4] = new SqlParameter("@PageIndex", _PageIndex);
                SqlParam[5] = new SqlParameter("@PageSize", PageSize);
                SqlParam[6] = new SqlParameter("@TotalRecord", SqlDbType.BigInt);
                SqlParam[6].Direction = ParameterDirection.Output;
                SqlParam[7] = new SqlParameter("@ISPCode", ISPCode);
                SqlParam[8] = new SqlParameter("@StoreCode", StoreCode); /* #CC07 Added */
                dtISPInfo = DataAccess.Instance.GetDataSetFromDatabase("prcISP_Select", CommandType.StoredProcedure, SqlParam).Tables[0];
                TotalRecords = Convert.ToInt32(SqlParam[6].Value);
                return dtISPInfo;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public DataTable ISP_SelectMapping()
        {
            try
            {
                SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@ISPID", intISPID);
                SqlParam[1] = new SqlParameter("@Status", _RecordStatus);
                SqlParam[2] = new SqlParameter("@ISPName", ISPName);
                SqlParam[3] = new SqlParameter("@RetailerID", RetailerID);
                SqlParam[4] = new SqlParameter("@PageIndex", _PageIndex);
                SqlParam[5] = new SqlParameter("@PageSize", PageSize);
                SqlParam[6] = new SqlParameter("@TotalRecord", SqlDbType.BigInt);
                SqlParam[6].Direction = ParameterDirection.Output;

                dtISPInfo = DataAccess.Instance.GetDataSetFromDatabase("prcISP_SelectMapping", CommandType.StoredProcedure, SqlParam).Tables[0];
                TotalRecords = Convert.ToInt32(SqlParam[6].Value);
                return dtISPInfo;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        private string _Error = string.Empty;
        public string Error
        {
            get
            {
                return _Error;
            }
            set
            {
                _Error = value;
            }
        }
        private byte _IsExitISP = 0;
        public byte IsExitISP
        {
            get
            {
                return _IsExitISP;
            }
            set
            {
                _IsExitISP = value;
            }
        }

        private int _RetailerISPMappingID = 0;
        public int RetailerISPMappingID
        {
            get
            {
                return _RetailerISPMappingID;
            }
            set
            {
                _RetailerISPMappingID = value;
            }
        }

        public Int32 UpdateExitISP()
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@Out_Param", IntResultCount);
                SqlParam[0].Direction = ParameterDirection.InputOutput;
                SqlParam[1] = new SqlParameter("@RetailerISPMappingID", _RetailerISPMappingID);
                SqlParam[2] = new SqlParameter("@IsExitISP", _IsExitISP);
                SqlParam[3] = new SqlParameter("@FromDate", _FromDate);
                SqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 2000);
                SqlParam[4].Direction = ParameterDirection.InputOutput;
                SqlParam[5] = new SqlParameter("@ISPID", ISPID);
                DataAccess.Instance.DBInsertCommand("prcExitISP", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[0].Value);
                _Error = SqlParam[4].Value.ToString();
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public byte Mode { get; set; }
        public Int32 UpdateISPMapingToNewRetialer()
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@Out_Param", IntResultCount);
                SqlParam[0].Direction = ParameterDirection.InputOutput;
                SqlParam[1] = new SqlParameter("@RetailerID", RetailerID);
                SqlParam[2] = new SqlParameter("@FromDate", _FromDate);
                SqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 2000);
                SqlParam[3].Direction = ParameterDirection.InputOutput;
                SqlParam[4] = new SqlParameter("@ISPID", ISPID);
                SqlParam[5] = new SqlParameter("@Mode", Mode);
                DataAccess.Instance.DBInsertCommand("prcISPMapToRetailer", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[0].Value);
                _Error = SqlParam[3].Value.ToString();
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


        public Int32 deleteISPMapingOrRetaining()
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@Out_Param", IntResultCount);
                SqlParam[0].Direction = ParameterDirection.InputOutput;
                SqlParam[1] = new SqlParameter("@RetailerISPMappingID", RetailerISPMappingID);
                //  SqlParam[2] = new SqlParameter("@FromDate", _FromDate);
                SqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 2000);
                SqlParam[2].Direction = ParameterDirection.InputOutput;
                //  SqlParam[4] = new SqlParameter("@ISPID", ISPID);
                //   SqlParam[5] = new SqlParameter("@EndDate", _EndDate);
                SqlParam[3] = new SqlParameter("@Mode", Mode);
                SqlParam[4] = new SqlParameter("@EndDate", _EndDate);
                DataAccess.Instance.DBInsertCommand("prcISPDeleteMapping", SqlParam);

                IntResultCount = Convert.ToInt16(SqlParam[0].Value);
                _Error = SqlParam[2].Value.ToString();
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        /* #CC06 Add Start */

        public Int32 UpdateISPMapingToNewRetialerV2()
        {
            try
            {
                SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@Out_Param", IntResultCount);
                SqlParam[0].Direction = ParameterDirection.InputOutput;
                SqlParam[1] = new SqlParameter("@RetailerID", RetailerID);
                SqlParam[2] = new SqlParameter("@FromDate", _FromDate);
                SqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 2000);
                SqlParam[3].Direction = ParameterDirection.InputOutput;
                SqlParam[4] = new SqlParameter("@ISPID", ISPID);
                SqlParam[5] = new SqlParameter("@Mode", Mode);
                SqlParam[6] = new SqlParameter("@UserID", Userid);
                DataAccess.Instance.DBInsertCommand("prcISPMapToRetailerV2", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[0].Value);
                _Error = SqlParam[3].Value.ToString();
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public Int32 deleteISPMapingOrRetainingV2()
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@Out_Param", IntResultCount);
                SqlParam[0].Direction = ParameterDirection.InputOutput;
                SqlParam[1] = new SqlParameter("@RetailerISPMappingID", RetailerISPMappingID);

                SqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 2000);
                SqlParam[2].Direction = ParameterDirection.InputOutput;
                SqlParam[3] = new SqlParameter("@Mode", Mode);
                SqlParam[4] = new SqlParameter("@EndDate", _EndDate);
                SqlParam[5] = new SqlParameter("@UserID", Userid);
                DataAccess.Instance.DBInsertCommand("prcISPDeleteMappingV2", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[0].Value);
                _Error = SqlParam[2].Value.ToString();
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        /* #CC06 Add End */
        /* #CC07 Add Start */

        public DataSet SaveISPBulkUpload()
        {
            try
            {
                SqlParam = new SqlParameter[10];
                SqlParam[0] = new SqlParameter("@SessionID", SessionID);
                SqlParam[1] = new SqlParameter("@OutParam", SqlDbType.Int);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@OutError", SqlDbType.NVarChar, 2000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@UserId", Userid);
                SqlParam[4] = new SqlParameter("@UploadType", UploadType);
                SqlParam[5] = new SqlParameter("@CompanyID", CompanyID);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcISPBulkUpload", CommandType.StoredProcedure, SqlParam);


                intOutParam = Convert.ToInt16(SqlParam[1].Value);
                Error = Convert.ToString(SqlParam[2].Value);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            /* return Result;*/
        }

        /* #CC07 Add End */
        public DataTable ISPExport()
        {
            try
            {
                SqlParam = new SqlParameter[4]; /* #CC07 Length increased from 1 to 3 */
                SqlParam[0] = new SqlParameter("@ISPName", ISPName);
                /*#CC07 Add Start */
                SqlParam[1] = new SqlParameter("@ISPCode", ISPCode);
                SqlParam[2] = new SqlParameter("@StoreCode", StoreCode);/*#CC07 Add End */
                SqlParam[3] = new SqlParameter("@CompanyID", CompanyID);/*#CC08 Added*/
                dtISPInfo = DataAccess.Instance.GetDataSetFromDatabase("prcISP_Export", CommandType.StoredProcedure, SqlParam).Tables[0];
                return dtISPInfo;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable GetISPInfo()
        {
            try
            {
                SqlParam = new SqlParameter[7]; 
                SqlParam[0] = new SqlParameter("@ISPName", ISPName);
                SqlParam[1] = new SqlParameter("@ISPCode", ISPCode);
                SqlParam[2] = new SqlParameter("@Userid", Userid);
                SqlParam[3] = new SqlParameter("@CompanyID", CompanyID);
                SqlParam[4] = new SqlParameter("@Email", Email);
                SqlParam[5] = new SqlParameter("@Mobile", Mobile);
                SqlParam[6] = new SqlParameter("@ActiveStatus", ActiveStatus);
                dtISPInfo = DataAccess.Instance.GetDataSetFromDatabase("prcISP_GridData", CommandType.StoredProcedure, SqlParam).Tables[0];
                return dtISPInfo;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public DataTable InValidSerials(Int32 SalesChannelID, string SalesChannelCode, string SkuCode, string StockSerialNo, string TypeID, string StockBinTypeID) /* #C003 StockBinTypeID Added */
        {
            DataTable result = new DataTable();
            try
            {

                SqlParam = new SqlParameter[7]; /* #CC03 length increased from 6 to 7*/
                SqlParam[0] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[1] = new SqlParameter("@SalesChannelCode", SalesChannelCode);
                SqlParam[2] = new SqlParameter("@SkuCode", SkuCode);
                SqlParam[3] = new SqlParameter("@Out_Error", Error);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@SerialNos", SqlDbType.Xml);
                SqlParam[4].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(StockSerialNo, XmlNodeType.Document, null));
                SqlParam[4].Direction = ParameterDirection.Input;
                SqlParam[5] = new SqlParameter("@TypeID", Convert.ToByte(TypeID));
                SqlParam[6] = new SqlParameter("@StockBinTypeId", Convert.ToInt16(StockBinTypeID)); /* #CC03 Added */
                result = DataAccess.Instance.GetDataSetFromDatabase("prcInvalidSerials", CommandType.StoredProcedure, SqlParam).Tables[0];
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                throw ex;

            }
            return result;
        }

        /*#CC02 START ADDED*/
        public DataTable InValidSerialsReturn(Int32 SalesChannelID, string SalesChannelCode, string SkuCode, string StockSerialNo, string TypeID, int StockBinType, string InvoiceNo)
        {
            DataTable result = new DataTable();
            try
            {

                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[1] = new SqlParameter("@StockBinType", StockBinType);
                SqlParam[2] = new SqlParameter("@SkuCode", SkuCode);
                SqlParam[3] = new SqlParameter("@Out_Error", Error);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@SerialNos", SqlDbType.Xml);
                SqlParam[4].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(StockSerialNo, XmlNodeType.Document, null));
                SqlParam[4].Direction = ParameterDirection.Input;
                SqlParam[5] = new SqlParameter("@InvoiceNo", InvoiceNo); 
                result = DataAccess.Instance.GetDataSetFromDatabase("prcInvalidSerialsReturn", CommandType.StoredProcedure, SqlParam).Tables[0];
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                throw ex;

            }
            return result;
        }
        /*#CC02 START END*/

    }
}
