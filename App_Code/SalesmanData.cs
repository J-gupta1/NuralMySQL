﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DataAccess;
/*
 * 06-May-2015, Karam Chand Sharma, #CC01, Add some properties and ceate new function for retailer bulk transfer
 * 21-Jul-2016, Sumit Maurya,  #CC02, New property created and supplied in method to save EmailID.
 * 19 Aug 2016, Karam Chand Sharma, #CC03, Allow custome paging 
 * 24-Aug-2016, Sumit Maurya, #CC04, New function created to Acivate/Deactivate Salesman with output parameters. 
 * 27-Sep-2016, Sumit Maurya, #CC05, New property created and parameter supplied with values to filter data.
 * 28-Sep-2016, Sumit Maurya, #CC06, New properties and methods created for BulkUploadRetailerTransfer.
 * 06-Oct-2016, Sumit Maurya, #CC07, New function created to update salesman. 
 * 10-Oct-2016, Sumit Maurya, #CC08, New parameters supplied to validate data in Add / update Bulk salesman.
 * 24-July-2018, Rakesh Raj, #CC09, GetProspectDetails, GetSalesChannelType for Mobile App API
 * 24-July-2018, Rakesh Raj, #CC10, GetFeebackDetails & GetSchemeDetails for Mobile App API
 * 14-Nov-2018,Vijay Kumar Prajapati,#CC11,Add new parameter in insertmethod.
 * 28-Nov-2018,Vijay Kumar Prajapati,#CC12,New property created and parameter for salesman logindetails.
 */




public class SalesmanData : IDisposable
{


    private int intSalesChannelID, intSalesmanID, /* ##CC09Start*/ intProspectId ;
        private string Strsalesmanname, StrMobileNumber, StrSalesmanCode, strAddress,
            /* ##CC09Start*/ strSalesChannelType, strCompanyName, strPersonName, strContactNo, strEmailId, strRemarks; /* ##CC09End*/
        /* ##CC10 Start*/
        DateTime _FeedbackFrom, _FeedbackTo;/* ##CC10 end*/
        private bool blnStatus;
        private string strError, strErrorDetailXML;
        private Int16 shtMapwithRetailer;
        /*#CC01 START ADDED*/
        private Int32 _SalesManFrom;
        private Int32 _SaleChannelFrom;
        private int _ComingFrom;
        private string _RetailerTransfer;
        /*#CC03 START ADDED*/
        private Int32 _PageIndex;
        private Int32 _PageSize;
        private Int32 _TotalRecords;
        /*#CC12 Added Started*/
        private string _password;
        private string _PasswordSalt;
        private string _UserName;
        private Int32 _PasswordExpiryDays;
        /*#CC12 Added End*/
        /* ##CC10 Start*/
        public DateTime? NullableFromDate { get; set; }
        public DateTime? NullableToDate { get; set; }
        public DateTime FromDate
        {
            get { return _FeedbackFrom; }
            set { _FeedbackFrom = value; }
        }

        public DateTime ToDate
        {
            get { return _FeedbackTo; }
            set { _FeedbackTo = value; }
        }
        /* ##CC10 End*/

        /* ##CC09Start*/
        public string SalesChannelType
        {
            get { return strSalesChannelType; }
            set { strSalesChannelType = value; }
        }

        public string PersonName
        {
            get { return strPersonName; }
            set { strPersonName = value; }
        }

        public string ContactNo
        {
            get { return strContactNo; }
            set { strContactNo = value; }
        }
        
        public string CompanyName
        {
            get { return strCompanyName; }
            set { strCompanyName = value; }
        }

        public string Remarks
        {
            get { return strRemarks; }
            set { strRemarks = value; }
        }

        public int ProspectId
        {
            get { return intProspectId; }
            set { intProspectId = value; }
        }
        
            
        
        /*#CC12 Added Started*/
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        public string PasswordSalt
        {
            get { return _PasswordSalt; }
            set { _PasswordSalt = value; }
        }
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
        public int PasswordExpiryDays
        {
            get { return _PasswordExpiryDays; }
            set { _PasswordExpiryDays = value; }
        }
        /*#CC12 Added End*/
        /* ##CC09End*/

        public Int32 BrandId { get; set; }
        public Int32 ProductCategoryId { get; set; }
        public Int32 PageIndex
        {
            get { return _PageIndex; }
            set { _PageIndex = value; }
        }
        public Int32 PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }
        public Int32 TotalRecords
        {
            get { return _TotalRecords; }
            set { _TotalRecords = value; }
        }
        /*#CC03 END ADDED*/
        /* #CC02 Add Start */
        public string EmailID
        {
            get;
            set;
        }
        /* #CC02 Add End */
        public Int32 SalesManFrom
        {
            get { return _SalesManFrom; }
            set { _SalesManFrom = value; }
        }
        public Int32 SaleChannelFrom
        {
            get { return _SaleChannelFrom; }
            set { _SaleChannelFrom = value; }
        }
        public int ComingFrom
        {
            get { return _ComingFrom; }
            set { _ComingFrom = value; }
        }
        public string RetailerTransfer
        {
            get { return _RetailerTransfer; }
            set { _RetailerTransfer = value; }
        }
        /*#CC01 START END*/
        public Int32 CompanyId { get; set; }

    public string ErrorDetailXML
    {
        get { return strErrorDetailXML; }
        set { strErrorDetailXML = value; }
    }
    public Int16 SalesChannelTypeID
    {
        get;
        set;
    }

    public int SalesmanID
    {
        get { return intSalesmanID; }
        set { intSalesmanID = value; }
    }
    public Int16 MapwithRetailer
    {
        get { return shtMapwithRetailer; }
        set { shtMapwithRetailer = value; }
    }

    private EnumData.eSearchConditions eType;

    public int SalesChannelID
    {
        get { return intSalesChannelID; }
        set { intSalesChannelID = value; }
    }
    public string Salesmanname
    {
        get { return Strsalesmanname; }
        set { Strsalesmanname = value; }
    }
    public string Address
    {
        get { return strAddress; }
        set { strAddress = value; }
    }
    public string Error
    {
        get { return strError; }
        set { strError = value; }
    }
    public bool Status
    {
        get { return blnStatus; }
        set { blnStatus = value; }
    }
    public EnumData.eSearchConditions Type
    {
        get { return eType; }
        set { eType = value; }
    }
    public string MobileNumber
    {
        get { return StrMobileNumber; }
        set { StrMobileNumber = value; }
    }
    public string SalesmanCode
    {
        get { return StrSalesmanCode; }
        set { StrSalesmanCode = value; }
    }
    public Int32 RetailerOrgnHierarchyID
    {
        get;
        set;
    }

    DataTable dtResult;
    SqlParameter[] SqlParam;
    Int32 IntResultCount = 0;
    DataSet dsResult;
    /* #CC05 Add Start */
        public string SalesChannelCode
        {
            get;
            set;
        }
        /* #CC05 Add End */

        /* #CC06 Add Start */
        public string SessionID
        {
            get;
            set;
        }
        public int UserID
        {
            get;
            set;
        }
        public string XMLList
        {
            get;
            set;
        }
        public int intOutParam
        {
            get;
            set;
        }
        /* #CC06 Add End */

        
        #region Get Retailer Info by Parameters
        public DataTable GetSalesmanInfo()
        {
            try
            {
                SqlParam = new SqlParameter[10];
                SqlParam[0] = new SqlParameter("@SalesmanId", intSalesmanID);
                SqlParam[1] = new SqlParameter("@SalesmanName", Strsalesmanname);
                SqlParam[2] = new SqlParameter("@SalesmanCode", StrSalesmanCode);
                SqlParam[3] = new SqlParameter("@SalesChannelID", intSalesChannelID);
                SqlParam[4] = new SqlParameter("@Type", Type);
                SqlParam[5] = new SqlParameter("@MapwithRetailer", shtMapwithRetailer);
                dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesmanInfo", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    /* ##CC09 Starts */
        public DataSet GetProspectDetails()
        {
            try
            {
                SqlParam = new SqlParameter[15]; 
                SqlParam[0] = new SqlParameter("@ProspectId", intProspectId);
                SqlParam[1] = new SqlParameter("@SalesmanName", Strsalesmanname);
                SqlParam[2] = new SqlParameter("@SalesChannelType", strSalesChannelType);
                SqlParam[3] = new SqlParameter("@PersonName", strPersonName);
                SqlParam[4] = new SqlParameter("@CompanyName", strCompanyName);
                SqlParam[5] = new SqlParameter("@ContactNo", strContactNo);
                SqlParam[6] = new SqlParameter("@LoginUserid", UserID);
                SqlParam[7] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
                SqlParam[8] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
                SqlParam[9] = new SqlParameter("@TotalRecord", SqlDbType.BigInt);
                SqlParam[10] = new SqlParameter("@PageIndex", _PageIndex);
                SqlParam[11] = new SqlParameter("@PageSize", _PageSize);
                SqlParam[12] = new SqlParameter("@CompanyId", CompanyId);
                SqlParam[13] = new SqlParameter("@FromDate", NullableFromDate);
                SqlParam[14] = new SqlParameter("@ToDate", NullableToDate);
                
                SqlParam[9].Direction = ParameterDirection.Output;

                dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetProspectDetails", CommandType.StoredProcedure, SqlParam);
                _TotalRecords = Convert.ToInt32(SqlParam[9].Value);
                return dsResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BindSalesManName()
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@userId", UserID);
                SqlParam[1] = new SqlParameter("@CompanyId", CompanyId);
                dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalemanV5", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BindSalesManNameForFeedback()
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@userId", UserID);
                SqlParam[1] = new SqlParameter("@CompanyId", CompanyId);
                dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalemanForFeedbackV5", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        /* ##CC09 End */

        /* ##CC10 Starts */
        /// <summary>
        /// Common Method to Bind Dropdown
        /// </summary>
        /// <param name="DropDownType"></param>
        /// <returns></returns>
        public DataTable BindDropDown(Int16 DropDownType)
        {
            try
            {
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@userId", UserID);
                SqlParam[1] = new SqlParameter("@dropdowntype", DropDownType);
                SqlParam[2] = new SqlParameter("@CompanyId", CompanyId);
                dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcBindMobileAppDropDowns", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GeFeedbackDetails()
        {
            try
            {   SqlParam = new SqlParameter[17]; 
                SqlParam[0] = new SqlParameter("@FeedbackId", intProspectId);
                SqlParam[1] = new SqlParameter("@SalesmanName", Strsalesmanname);
                SqlParam[2] = new SqlParameter("@SalesChannelType", strSalesChannelType);
                SqlParam[3] = new SqlParameter("@SalesChannelName", strCompanyName);
                SqlParam[4] = new SqlParameter("@CategoryName", strContactNo);
                SqlParam[5] = new SqlParameter("@ModelName", strAddress);

                if (_FeedbackFrom.Year >= 1900)
                {
                    SqlParam[6] = new SqlParameter("@RevertDateFrom", _FeedbackFrom);
                }
                else
                {
                    SqlParam[6] = new SqlParameter("@RevertDateFrom", DBNull.Value);
                }

                if (_FeedbackTo.Year >= 1900)
                {
                    SqlParam[7] = new SqlParameter("@RevertDateTo", _FeedbackTo);
                }
                else
                {
                    SqlParam[7] = new SqlParameter("@RevertDateTo", DBNull.Value);
                }

                SqlParam[8] = new SqlParameter("@LoginUserid", UserID);
                SqlParam[9] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
                SqlParam[10] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
                SqlParam[11] = new SqlParameter("@TotalRecord", SqlDbType.BigInt);
                SqlParam[11].Direction = ParameterDirection.Output;
                SqlParam[12] = new SqlParameter("@PageIndex", _PageIndex);
                SqlParam[13] = new SqlParameter("@PageSize", _PageSize);
                SqlParam[14] = new SqlParameter("@CompanyId", CompanyId);
                SqlParam[15] = new SqlParameter("@BrandId", BrandId);
                SqlParam[16] = new SqlParameter("@ProductCategoryId", ProductCategoryId);
                dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetFeedbackDetails", CommandType.StoredProcedure, SqlParam);
                _TotalRecords = Convert.ToInt32(SqlParam[11].Value);
                return dsResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GeSchemeDetailsAPI()
        {
            try
            {
                SqlParam = new SqlParameter[12];
                SqlParam[0] = new SqlParameter("@SchemeId", intProspectId);
                SqlParam[1] = new SqlParameter("@SalesmanName", Strsalesmanname);
                SqlParam[2] = new SqlParameter("@SalesChannelType", strSalesChannelType);
                SqlParam[3] = new SqlParameter("@SalesChannelName", strCompanyName);
                
                if (_FeedbackFrom.Year >= 1900)
                {
                    SqlParam[4] = new SqlParameter("@SchemeDateFrom", _FeedbackFrom);
                }
                else
                {
                    SqlParam[4] = new SqlParameter("@SchemeDateFrom", DBNull.Value);
                }

                if (_FeedbackTo.Year >= 1900)
                {
                    SqlParam[5] = new SqlParameter("@SchemeDateTo", _FeedbackTo);
                }
                else
                {
                    SqlParam[5] = new SqlParameter("@SchemeDateTo", DBNull.Value);
                }

                SqlParam[6] = new SqlParameter("@LoginUserid", UserID);
                SqlParam[7] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
                SqlParam[8] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
                SqlParam[9] = new SqlParameter("@TotalRecord", SqlDbType.BigInt);
                SqlParam[10] = new SqlParameter("@PageIndex", _PageIndex);
                SqlParam[11] = new SqlParameter("@PageSize", _PageSize);

                SqlParam[9].Direction = ParameterDirection.Output;

                dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetSchemeDetailsForAPI", CommandType.StoredProcedure, SqlParam);
                _TotalRecords = Convert.ToInt32(SqlParam[9].Value);
                return dsResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GeOutStandingAmountDetails()
        {
            try
            {
                SqlParam = new SqlParameter[11];
                SqlParam[0] = new SqlParameter("@OutStandingAmountID", intProspectId);
                SqlParam[1] = new SqlParameter("@SalesChannelType", strSalesChannelType);
                SqlParam[2] = new SqlParameter("@SalesChannelName", strCompanyName);

                if (_FeedbackFrom.Year >= 1900)
                {
                    SqlParam[3] = new SqlParameter("@DateFrom", _FeedbackFrom);
                }
                else
                {
                    SqlParam[3] = new SqlParameter("@DateFrom", DBNull.Value);
                }

                if (_FeedbackTo.Year >= 1900)
                {
                    SqlParam[4] = new SqlParameter("@DateTo", _FeedbackTo);
                }
                else
                {
                    SqlParam[4] = new SqlParameter("@DateTo", DBNull.Value);
                }

                SqlParam[5] = new SqlParameter("@LoginUserid", UserID);
                SqlParam[6] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
                SqlParam[7] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
                SqlParam[8] = new SqlParameter("@TotalRecord", SqlDbType.BigInt);
                SqlParam[9] = new SqlParameter("@PageIndex", _PageIndex);
                SqlParam[10] = new SqlParameter("@PageSize", _PageSize);

                SqlParam[8].Direction = ParameterDirection.Output;

                dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetOutStandingAmountDetails", CommandType.StoredProcedure, SqlParam);
                _TotalRecords = Convert.ToInt32(SqlParam[8].Value);
                return dsResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


        /* ##CC10 End */

    /*#CC03 START ADDED */
    public DataTable GetSalesmanInfoV2()
    {
        try
        {
            SqlParam = new SqlParameter[12]; /* #CC05 length increased from 10 to 12 */
            SqlParam[0] = new SqlParameter("@SalesmanId", intSalesmanID);
            SqlParam[1] = new SqlParameter("@SalesmanName", Strsalesmanname);
            SqlParam[2] = new SqlParameter("@SalesmanCode", StrSalesmanCode);
            SqlParam[3] = new SqlParameter("@SalesChannelID", intSalesChannelID);
            SqlParam[4] = new SqlParameter("@Type", Type);
            SqlParam[5] = new SqlParameter("@MapwithRetailer", shtMapwithRetailer);
            SqlParam[6] = new SqlParameter("@PageIndex", _PageIndex);
            SqlParam[7] = new SqlParameter("@PageSize", _PageSize);
            SqlParam[8] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 10);
            SqlParam[8].Direction = ParameterDirection.Output;
             /* #CC05 Add Start */
                SqlParam[9] = new SqlParameter("@SalesChannelCode", SalesChannelCode);
                SqlParam[10] = new SqlParameter("@EmailID", EmailID);
                SqlParam[11] = new SqlParameter("@SalesmanMobileNumber", MobileNumber);
                /* #CC05 Add End */
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesmanInfoV2", CommandType.StoredProcedure, SqlParam);
            _TotalRecords = Convert.ToInt32(SqlParam[8].Value);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    /*#CC03 END ADDED */
    public DataSet GetSalesmanAndStockBinTypeInfo()
    {
        try
        {
            SqlParam = new SqlParameter[7];
            SqlParam[0] = new SqlParameter("@SalesmanId", intSalesmanID);
            SqlParam[1] = new SqlParameter("@SalesmanName", Strsalesmanname);
            SqlParam[2] = new SqlParameter("@SalesmanCode", StrSalesmanCode);
            SqlParam[3] = new SqlParameter("@SalesChannelID", intSalesChannelID);
            SqlParam[4] = new SqlParameter("@Type", Type);
            SqlParam[5] = new SqlParameter("@MapwithRetailer", shtMapwithRetailer);
            SqlParam[6] = new SqlParameter("@CompanyId", CompanyId);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetSalesmanAndStockBinTypeInfo", CommandType.StoredProcedure, SqlParam);

            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion
    public Int32 InsertUpdateSalesManInfo()
    {
        try
        {
            SqlParam = new SqlParameter[14]; /* #CC02 length increased from 8 to 9 */
            SqlParam[0] = new SqlParameter("@SalesmanID", intSalesmanID);
            SqlParam[0].Direction = ParameterDirection.InputOutput;
            SqlParam[1] = new SqlParameter("@SalesManName", Salesmanname);
            SqlParam[2] = new SqlParameter("@SalesManCode", SalesmanCode);
            SqlParam[3] = new SqlParameter("@Address", Address);
            SqlParam[4] = new SqlParameter("@Mobile", MobileNumber);
            SqlParam[5] = new SqlParameter("@Status", Status);
            SqlParam[6] = new SqlParameter("@SalesChannelID", SalesChannelID);
            SqlParam[7] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 200);
            SqlParam[7].Direction = ParameterDirection.Output;
            SqlParam[8] = new SqlParameter("@Email", EmailID); /* #CC02 Added */
            SqlParam[9] = new SqlParameter("@Userid", UserID); /* #CC11 Added */
            SqlParam[10] = new SqlParameter("@UserName", UserName);/*#CC12 Added*/
            SqlParam[11] = new SqlParameter("@Password", Password);/*#CC12 Added*/
            SqlParam[12] = new SqlParameter("@PasswordSalt", PasswordSalt);/*#CC12 Added*/
            SqlParam[13] = new SqlParameter("@PasswordExpiryDays", PasswordExpiryDays);/*#CC12 Added*/
            DataAccess.DataAccess.Instance.DBInsertCommand("prcInsUpdSalesManInfo", SqlParam);
            IntResultCount = Convert.ToInt32(SqlParam[0].Value);
            if (SqlParam[7].Value != DBNull.Value && SqlParam[7].Value.ToString() != "")
            {
                Error = (SqlParam[7].Value).ToString();
            }
            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public Int32 InsertInfoSalesManUpload(DataTable Dt)
    {
        try
        {
            SqlParam = new SqlParameter[6];
            SqlParam[0] = new SqlParameter("@tvpsalesman", SqlDbType.Structured);
            SqlParam[0].Value = Dt;
            SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
            SqlParam[3].Direction = ParameterDirection.Output;
            SqlParam[4] = new SqlParameter("@SalesChannelID", SalesChannelID); /* #CC08 Added */
            SqlParam[5] = new SqlParameter("@UserId", UserID); /* #CC11 Added */
            DataAccess.DataAccess.Instance.DBInsertCommand("PrcSalesManUpload", SqlParam);
            IntResultCount = Convert.ToInt16(SqlParam[1].Value);
            if (SqlParam[3].Value.ToString() != "")
            {
                ErrorDetailXML = SqlParam[3].Value.ToString();
            }
            else
            {
                ErrorDetailXML = null;
            }
            Error = Convert.ToString(SqlParam[2].Value);

            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /*#CC01 START ADDED*/
    public Int32 InsertInfoRetailerMapV2()
    {
        try
        {

            SqlParam = new SqlParameter[11];
            SqlParam[0] = new SqlParameter("@RetailerTransfer", RetailerTransfer);
            SqlParam[1] = new SqlParameter("@tosalesmanid", intSalesmanID);
            SqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
            SqlParam[3].Direction = ParameterDirection.Output;
            SqlParam[4] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
            SqlParam[4].Direction = ParameterDirection.Output;
            SqlParam[5] = new SqlParameter("@tosalesChannelid", SalesChannelID);
            SqlParam[6] = new SqlParameter("@RetailerOrgnHierarchyID", RetailerOrgnHierarchyID);
            SqlParam[7] = new SqlParameter("@FromSalesChannelId", SaleChannelFrom);
            SqlParam[8] = new SqlParameter("@FromsalesmaniD", SalesManFrom);
            SqlParam[9] = new SqlParameter("@ComingFrom", ComingFrom);
            DataAccess.DataAccess.Instance.DBInsertCommand("prcUpdRetailerMapV2", SqlParam);
            IntResultCount = Convert.ToInt16(SqlParam[2].Value);
            if (SqlParam[4].Value.ToString() != "")
            {
                ErrorDetailXML = SqlParam[4].Value.ToString();
            }
            else
            {
                ErrorDetailXML = null;
            }
            Error = Convert.ToString(SqlParam[3].Value);

            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    /*#CC01 START END*/
    //Pankaj Dhingra
    public Int32 InsertInfoRetailerMap(DataTable Dt)
    {
        try
        {
            //Pankaj dhingra Added a new parameter
            SqlParam = new SqlParameter[7];
            SqlParam[0] = new SqlParameter("@tvpRetailerMap", SqlDbType.Structured);
            SqlParam[0].Value = Dt;
            SqlParam[1] = new SqlParameter("@tosalesmanid", intSalesmanID);
            SqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
            SqlParam[3].Direction = ParameterDirection.Output;
            SqlParam[4] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
            SqlParam[4].Direction = ParameterDirection.Output;
            SqlParam[5] = new SqlParameter("@tosalesChannelid", SalesChannelID);
            SqlParam[6] = new SqlParameter("@RetailerOrgnHierarchyID", RetailerOrgnHierarchyID);
            DataAccess.DataAccess.Instance.DBInsertCommand("prcUpdRetailerMap", SqlParam);
            IntResultCount = Convert.ToInt16(SqlParam[1].Value);
            if (SqlParam[3].Value.ToString() != "")
            {
                ErrorDetailXML = SqlParam[4].Value.ToString();
            }
            else
            {
                ErrorDetailXML = null;
            }
            Error = Convert.ToString(SqlParam[3].Value);

            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public Int32 InsertInfoSalesChannelTypeMap(DataTable Dt)
    {
        try
        {
            //Pankaj dhingra Added a new parameter
            SqlParam = new SqlParameter[6];
            SqlParam[0] = new SqlParameter("@tvpSalesChannelMap", SqlDbType.Structured);
            SqlParam[0].Value = Dt;
            SqlParam[1] = new SqlParameter("@SalesChannelTypeIDTo", SalesChannelTypeID);
            SqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
            SqlParam[3].Direction = ParameterDirection.Output;
            SqlParam[4] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
            SqlParam[4].Direction = ParameterDirection.Output;
            SqlParam[5] = new SqlParameter("@LoggedInSalesChannelid", SalesChannelID);
            DataAccess.DataAccess.Instance.DBInsertCommand("prcChangeSalesChannelTypeMap", SqlParam);
            IntResultCount = Convert.ToInt16(SqlParam[1].Value);
            if (SqlParam[3].Value.ToString() != "")
            {
                ErrorDetailXML = SqlParam[4].Value.ToString();
            }
            else
            {
                ErrorDetailXML = null;
            }
            Error = Convert.ToString(SqlParam[3].Value);

            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public Int32 UpdateSalesChannelRetailerMap(DataTable Dt)
    {
        try
        {
            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@tvpRetailer", SqlDbType.Structured);
            SqlParam[0].Value = Dt;
            SqlParam[1] = new SqlParameter("@saleschannelTo", intSalesChannelID);
            SqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
            SqlParam[3].Direction = ParameterDirection.Output;
            SqlParam[4] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
            SqlParam[4].Direction = ParameterDirection.Output;
            DataAccess.DataAccess.Instance.DBInsertCommand("prcUpdRetailerSalesChannelMapping", SqlParam);
            IntResultCount = Convert.ToInt16(SqlParam[1].Value);
            if (SqlParam[3].Value.ToString() != "")
            {
                ErrorDetailXML = SqlParam[4].Value.ToString();
            }
            else
            {
                ErrorDetailXML = null;
            }
            Error = Convert.ToString(SqlParam[3].Value);

            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }






    public Int32 UpdateStatusSalesManInfo()
    {
        try
        {

            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@SalesmanID", SalesmanID);
            IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("PrcUpdStatusSalesMan", SqlParam);
            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    /* #CC04 Add Start */
    public Int32 UpdateStatusSalesManInfoV2()
    {
        try
        {
            SqlParam = new SqlParameter[4];
            SqlParam[0] = new SqlParameter("@SalesmanID", SalesmanID);
            SqlParam[1] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@OutError", SqlDbType.VarChar, 200);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@UserId", UserID);/* #CC11 Added */
            DataAccess.DataAccess.Instance.DBInsertCommand("PrcUpdStatusSalesManV2", SqlParam);
            IntResultCount = Convert.ToInt16(SqlParam[1].Value);
            if (SqlParam[2].Value != DBNull.Value && SqlParam[2].Value.ToString() != "")
            {
                Error = (SqlParam[2].Value).ToString();
            }
            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }

        }
        /* #CC04 Add End */
        /* #CC06 Add Start */
        public DataSet GetRetailerBulkUploadTransfer()
        {
            try
            {
                DataSet dsResult = new DataSet();
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[1] = new SqlParameter("@OutError", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcBulkRetailerTransferUploadData", CommandType.StoredProcedure, SqlParam);
                if (SqlParam[1].Value != DBNull.Value && SqlParam[1].Value.ToString() != "")
                {
                    Error = (SqlParam[1].Value).ToString();
                }
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet SaveRetailerBulkTransfer()
        {
            Int16 Result;
            SqlParam = new SqlParameter[6];
            SqlParam[0] = new SqlParameter("@SessionID", SessionID);
            SqlParam[1] = new SqlParameter("@OutParam", SqlDbType.Int);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@OutError", SqlDbType.NVarChar, 200);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
            SqlParam[3].Direction = ParameterDirection.Output;
            SqlParam[4] = new SqlParameter("@UserID", UserID);
            SqlParam[5] = new SqlParameter("@SalesChannelID", SalesChannelID);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcBulkRetailerTransfer", CommandType.StoredProcedure, SqlParam);
            if (SqlParam[3].Value.ToString() != "")
            {
                XMLList = SqlParam[3].Value.ToString();
            }
            else
            {
                XMLList = null;
            }
            intOutParam = Convert.ToInt16(SqlParam[1].Value);
            Error = Convert.ToString(SqlParam[2].Value);
            return dsResult;
        }



        /* #CC06 Add End */

        /* #CC07 Add Start */
        public DataSet BulkSalesmanUpdate()
        {
            Int16 Result;
            SqlParam = new SqlParameter[5]; 
            SqlParam[0] = new SqlParameter("@SessionID", SessionID);
            SqlParam[1] = new SqlParameter("@OutParam", SqlDbType.Int);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@OutError", SqlDbType.NVarChar, 200);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@SalesChannelID", SalesChannelID);
            SqlParam[4] = new SqlParameter("@UserId", UserID);/*#CC11 Added*/
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcBulkSalesmanUpload", CommandType.StoredProcedure, SqlParam);
            intOutParam = Convert.ToInt16(SqlParam[1].Value);
            if (Convert.ToString(SqlParam[2].Value) != null && Convert.ToString(SqlParam[2].Value) != "")
            {
                Error = Convert.ToString(SqlParam[2].Value);
            }
            return dsResult;
        }
        /* #CC07 Add End */

    #region Check Mapped Retailer Existance
    public Int32 CheckSalesmanExistence()
    {
        try
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@SalesmanID", intSalesmanID);

            IntResultCount = Convert.ToInt32(DataAccess.DataAccess.Instance.getSingleValues("PrcChkSalesmanExistence", SqlParam));
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

    ~SalesmanData()
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

