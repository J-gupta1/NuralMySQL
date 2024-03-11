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
Amit Bhardwaj	01/12/2010		Created
Dhiraj Kumar    28-Feb/2013     #CC01 - set Add country id and country name in Load() function
Rajesh Upadhyay 05-jan-2015 #CC02 -Line added For Binding STDCode
Rajesh Upadhyay 09-july-2015 #CC03 -Property Added CallProjectType
22-Aug-2016, Vijay Katiyar, #CC04 - Added CustomerTypeId property and pass in update function
31-Jan-2017, Vijay Katiyar, #CC05 - Added ModelType and datatable dtBankName 
07-Feb-2017, Vijay Katiyar, #CC06 - Added Customer Category in load() function
 * 12-Feb-2018, Sumit Maurya, #CC07, New value provided in parameter to update GSTINNO (Done for Amararaja).

*/

    public class clsJobSheetCustomerDetail : clsDetailMaster, IDisposable
    {
        public static string _ConnectionString = ConfigurationManager.ConnectionStrings["AppConString"].ConnectionString;

        #region Private Class Variables

        private Int64 _intJobSheetCustomerDetailID;/*#CC02: Added */
        //private Int64 _intJobSheetID;
        //private Int64 _intCustomerDetailId;
        
        private Int64 _intCustomerAMCID;
        private string _strAMCNo;
        private short _shtRepeatJobSheet;
        private short _shtClashJobsheet;
        private Int64 _intStockOfEntityReceiveDetailID;
        private short _shtReasonID;
        private int _intNotReadyToPayReasonMasterId;
        private string _strCallProjectType;

        #endregion

        #region Public Properties

        public string CallProjectType
        {
            get
            {
                return _strCallProjectType;
            }
            set
            {
                _strCallProjectType = value;
            }
        }
        /*#CC02: Added (Start)*/
        public Int64 JobSheetCustomerDetailID
        {
            get
            {
                return _intJobSheetCustomerDetailID;
            }
            set
            {
                _intJobSheetCustomerDetailID = value;
            }
        }
        /*#CC02: Added (End)*/
        //public Int64 JobSheetID
        //{
        //    get
        //    {
        //        return _intJobSheetID;
        //    }
        //    set
        //    {
        //        _intJobSheetID = value;
        //    }
        //}
        //public Int64 CustomerDetailId
        //{
        //    get
        //    {
        //        return _intCustomerDetailId;
        //    }
        //    set
        //    {
        //        _intCustomerDetailId = value;
        //    }
        //}
        public Int64 CustomerAMCID
        {
            get
            {
                return _intCustomerAMCID;
            }
            set
            {
                _intCustomerAMCID = value;
            }
        }
        public string AMCNo
        {
            get
            {
                return _strAMCNo;
            }
            set
            {
                _strAMCNo = value;
            }
        }
        public short RepeatJobSheet
        {
            get
            {
                return _shtRepeatJobSheet;
            }
            set
            {
                _shtRepeatJobSheet = value;
            }
        }
        public short ClashJobsheet
        {
            get
            {
                return _shtClashJobsheet;
            }
            set
            {
                _shtClashJobsheet = value;
            }
        }
        public Int64 StockOfEntityReceiveDetailID
        {
            get
            {
                return _intStockOfEntityReceiveDetailID;
            }
            set
            {
                _intStockOfEntityReceiveDetailID = value;
            }
        }
        public short ReasonID
        {
            get
            {
                return _shtReasonID;
            }
            set
            {
                _shtReasonID = value;
            }
        }
        public int NotReadyToPayReasonMasterId
        {
            get
            {
                return _intNotReadyToPayReasonMasterId;
            }
            set
            {
                _intNotReadyToPayReasonMasterId = value;
            }
        }

        #endregion

        private string _Address2 = string.Empty;

        public string Address2
        {
            get { return _Address2; }
            set { _Address2 = value; }
        }


        #region Constructors
        public clsJobSheetCustomerDetail()
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

        ~clsJobSheetCustomerDetail()
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
        override public Int16 Save()
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[27];
            objSqlParam[0] = new SqlParameter("@JobSheetID", JobSheetID);
            objSqlParam[1] = new SqlParameter("@CustomerDetailId", DetailId);
            objSqlParam[2] = new SqlParameter("@StateId", StateId);
            objSqlParam[3] = new SqlParameter("@CityId", CityId);
            objSqlParam[4] = new SqlParameter("@LocalityID", LocalityID);
            objSqlParam[5] = new SqlParameter("@StateName", StateName);
            objSqlParam[6] = new SqlParameter("@CityName", CityName);
            objSqlParam[7] = new SqlParameter("@LocalityName", LocalityName);
            objSqlParam[8] = new SqlParameter("@Street", Street);
            objSqlParam[9] = new SqlParameter("@Address", Address);
            objSqlParam[10] = new SqlParameter("@LandMark", LandMark);
            objSqlParam[11] = new SqlParameter("@Pincode", Pincode);
            objSqlParam[12] = new SqlParameter("@MobileNo", MobileNo);
            objSqlParam[13] = new SqlParameter("@AltMobileNo", AltMobileNo);
            objSqlParam[14] = new SqlParameter("@PhoneNo", PhoneNo);
            objSqlParam[15] = new SqlParameter("@AltPhoneNo", AltPhoneNo);
            objSqlParam[16] = new SqlParameter("@EmailID", EmailID);
            objSqlParam[17] = new SqlParameter("@CustomerAMCID", CustomerAMCID);
            objSqlParam[18] = new SqlParameter("@AMCNo", AMCNo);
            objSqlParam[19] = new SqlParameter("@RepeatJobSheet", RepeatJobSheet);
            objSqlParam[20] = new SqlParameter("@ClashJobsheet", ClashJobsheet);
            objSqlParam[21] = new SqlParameter("@StockOfEntityReceiveDetailID", StockOfEntityReceiveDetailID);
            objSqlParam[22] = new SqlParameter("@ReasonID", ReasonID);
            objSqlParam[23] = new SqlParameter("@NotReadyToPayReasonMasterId", NotReadyToPayReasonMasterId);
            objSqlParam[24] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[24].Direction = ParameterDirection.Output;
            objSqlParam[25] = new SqlParameter("@PKId", SqlDbType.BigInt, 8);
            objSqlParam[25].Direction = ParameterDirection.Output;
            objSqlParam[26] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[26].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, "prcJobSheetCustomerDetail_Insert", objSqlParam);
            result = Convert.ToInt16(objSqlParam[24].Value);
            JobSheetCustomerDetailID = Convert.ToInt64(objSqlParam[25].Value);
            Error = Convert.ToString(objSqlParam[26].Value);
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
        override public Int16 Update()
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[26];
            objSqlParam[0] = new SqlParameter("@JobSheetID", JobSheetID);
            objSqlParam[1] = new SqlParameter("@CustomerDetailId", DetailId);
            objSqlParam[2] = new SqlParameter("@FirstName", FirstName);
            objSqlParam[3] = new SqlParameter("@MiddleName", MiddleName);
            objSqlParam[4] = new SqlParameter("@LastName", LastName);
            objSqlParam[5] = new SqlParameter("@CompanyName", CompanyName);
            objSqlParam[6] = new SqlParameter("@Age", Age);
            objSqlParam[7] = new SqlParameter("@DOB", DOB);
            objSqlParam[8] = new SqlParameter("@MarriageAnniversary", MarriageAnniversary);
            objSqlParam[9] = new SqlParameter("@StateId", StateId);
            objSqlParam[10] = new SqlParameter("@CityId", CityId);
            objSqlParam[11] = new SqlParameter("@LocalityID", LocalityID);
            objSqlParam[12] = new SqlParameter("@StateName", StateName);
            objSqlParam[13] = new SqlParameter("@CityName", CityName);
            objSqlParam[14] = new SqlParameter("@LocalityName", LocalityName);
            objSqlParam[15] = new SqlParameter("@Street", Street);
            objSqlParam[16] = new SqlParameter("@Address", Address);
            objSqlParam[17] = new SqlParameter("@Pincode", Pincode);
            objSqlParam[18] = new SqlParameter("@MobileNo", MobileNo);
            objSqlParam[19] = new SqlParameter("@AltMobileNo", AltMobileNo);
            objSqlParam[20] = new SqlParameter("@PhoneNo", PhoneNo);
            objSqlParam[21] = new SqlParameter("@AltPhoneNo", AltPhoneNo);
            objSqlParam[22] = new SqlParameter("@EmailID", EmailID);
            objSqlParam[23] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[23].Direction = ParameterDirection.Output;
            objSqlParam[24] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[24].Direction = ParameterDirection.Output;
            objSqlParam[25] = new SqlParameter("@LandMark", LandMark);      //Changed Paankaj Kumar
            //objSqlParam[23] = new SqlParameter("@CustomerAMCID", CustomerAMCID);
            //objSqlParam[24] = new SqlParameter("@AMCNo", AMCNo);
            //objSqlParam[25] = new SqlParameter("@RepeatJobSheet", RepeatJobSheet);
            //objSqlParam[26] = new SqlParameter("@ClashJobsheet", ClashJobsheet);
            //objSqlParam[27] = new SqlParameter("@StockOfEntityReceiveDetailID", StockOfEntityReceiveDetailID);
            //objSqlParam[28] = new SqlParameter("@ReasonID", ReasonID);
            //objSqlParam[29] = new SqlParameter("@NotReadyToPayReasonMasterId", NotReadyToPayReasonMasterId);

            SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, "prcJobSheetCustomerDetail_Update", objSqlParam);

            result = Convert.ToInt16(objSqlParam[23].Value);
            Error = Convert.ToString(objSqlParam[24].Value);

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
        override public bool Delete()
        {
            bool result = false;
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@JobSheetCustomerDetailID", JobSheetCustomerDetailID);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, "prcJobSheetCustomerDetail_Delete", objSqlParam);
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
        override public DataTable SelectAll()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[4];
            objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[3].Direction = ParameterDirection.Output;
            DataSet dsResult = SqlHelper.ExecuteDataset(_ConnectionString, CommandType.StoredProcedure, "prcJobSheetCustomerDetail_Select", objSqlParam);
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
        /// <summary>
        /// Get All records from database for selected key
        /// </summary>
        /// <results>DataTable: Collection of records</results> 		
        override public DataTable SelectById()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@JobsheetID", JobSheetID);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            DataSet dsResult = SqlHelper.ExecuteDataset(_ConnectionString, CommandType.StoredProcedure, "prcJobSheetCustomerDetail_SelectById", objSqlParam);
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
        override public void Load()
        {
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@JobsheetID", JobSheetID);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            IDataReader reader = SqlHelper.ExecuteReader(_ConnectionString, CommandType.StoredProcedure, "prcJobSheetCustomerDetail_SelectById", objSqlParam);
            
            while (reader.Read())
            {
                if (reader["JobSheetID"] != DBNull.Value)
                    JobSheetID = Convert.ToInt64(reader["JobSheetID"]);
                if (reader["CustomerDetailId"] != DBNull.Value)
                    DetailId = Convert.ToInt64(reader["CustomerDetailId"]);
                if (reader["FirstName"] != DBNull.Value)
                    FirstName = Convert.ToString(reader["FirstName"]);
                if (reader["MiddleName"] != DBNull.Value)
                    MiddleName = Convert.ToString(reader["MiddleName"]);
                if (reader["LastName"] != DBNull.Value)
                    LastName = Convert.ToString(reader["LastName"]);
                if (reader["CompanyName"] != DBNull.Value)
                    CompanyName = Convert.ToString(reader["CompanyName"]);
                if (reader["Age"] != DBNull.Value)
                    Age = Convert.ToInt16(reader["Age"]);
                if (reader["DOB"] != DBNull.Value)
                    DOB = Convert.ToDateTime(reader["DOB"]);
                if (reader["MarriageAnniversary"] != DBNull.Value)
                    MarriageAnniversary = Convert.ToDateTime(reader["MarriageAnniversary"]);
                if (reader["StateId"] != DBNull.Value)
                    StateId = Convert.ToInt16(reader["StateId"]);
                if (reader["CityId"] != DBNull.Value)
                    CityId = Convert.ToInt32(reader["CityId"]);
                if (reader["LocalityID"] != DBNull.Value)
                    LocalityID = Convert.ToInt32(reader["LocalityID"]);
                if (reader["StateName"] != DBNull.Value)
                    StateName = Convert.ToString(reader["StateName"]);
                if (reader["CityName"] != DBNull.Value)
                    CityName = Convert.ToString(reader["CityName"]);
                if (reader["LocalityName"] != DBNull.Value)
                    LocalityName = Convert.ToString(reader["LocalityName"]);
                if (reader["Street"] != DBNull.Value)
                    Street = Convert.ToString(reader["Street"]);
                if (reader["Address"] != DBNull.Value)
                    Address = Convert.ToString(reader["Address"]);
                if (reader["Pincode"] != DBNull.Value)
                    Pincode = Convert.ToString(reader["Pincode"]);
                if (reader["MobileNo"] != DBNull.Value)
                    MobileNo = Convert.ToString(reader["MobileNo"]);
                if (reader["AltMobileNo"] != DBNull.Value)
                    AltMobileNo = Convert.ToString(reader["AltMobileNo"]);
                if (reader["PhoneNo"] != DBNull.Value)
                    PhoneNo = Convert.ToString(reader["PhoneNo"]);
                if (reader["AltPhoneNo"] != DBNull.Value)
                    AltPhoneNo = Convert.ToString(reader["AltPhoneNo"]);
                if (reader["EmailID"] != DBNull.Value)
                    EmailID = Convert.ToString(reader["EmailID"]);
                if (reader["CustomerAMCID"] != DBNull.Value)
                    CustomerAMCID = Convert.ToInt64(reader["CustomerAMCID"]);
                if (reader["AMCNo"] != DBNull.Value)
                    AMCNo = Convert.ToString(reader["AMCNo"]);
                if (reader["RepeatJobSheet"] != DBNull.Value)
                    RepeatJobSheet = Convert.ToInt16(reader["RepeatJobSheet"]);
                if (reader["ClashJobsheet"] != DBNull.Value)
                    ClashJobsheet = Convert.ToInt16(reader["ClashJobsheet"]);
                if (reader["StockOfEntityReceiveDetailID"] != DBNull.Value)
                    StockOfEntityReceiveDetailID = Convert.ToInt64(reader["StockOfEntityReceiveDetailID"]);
                if (reader["NotReadyToPayReasonMasterId"] != DBNull.Value)
                    NotReadyToPayReasonMasterId = Convert.ToInt32(reader["NotReadyToPayReasonMasterId"]);
                if (reader["LandMark"] != DBNull.Value)
                    LandMark = Convert.ToString(reader["LandMark"]);    //Paankaj Kumar
                /*#CC01:Start - Added*/
                if (reader["CountryID"] != DBNull.Value)
                    CountryId = Convert.ToInt16(reader["CountryID"]);    
                if (reader["CountryName"] != DBNull.Value)
                    CountryName = Convert.ToString(reader["CountryName"]);
                /*#CC01:Start - Added*/
                /*#CC02: Added (start)*/
                if (reader["STDCode"] != DBNull.Value)
                    STDCode = Convert.ToString(reader["STDCode"]);
                /*#CC02: Added (end)*/
                if (reader["CallProjectType"] != DBNull.Value)
                    CallProjectType = Convert.ToString(reader["CallProjectType"]);
                /* #CC04 Added start*/
                if (reader["CustomerTypeID"] != DBNull.Value)
                    CustomerTypeId = Convert.ToInt16(reader["CustomerTypeID"]);
                if (reader["EditMode"] != DBNull.Value)
                    CustomerEditMode = Convert.ToInt16(reader["EditMode"]);
                /* #CC04 Added end*/
                /* #CC05 Added start*/
                if (reader["ModelType"] != DBNull.Value)
                    ModelType = Convert.ToString(reader["ModelType"]);
                /* #CC05 Added end*/
                /* #CC06 Added start*/
                if (reader["CustomerCategory"] != DBNull.Value)
                    CustomerCategory = Convert.ToString(reader["CustomerCategory"]);
                /* #CC06 Added end*/
                /* #CC07 Add Start */
                if (reader["GSTINNO"] != DBNull.Value)
                    GSTINNo = Convert.ToString(reader["GSTINNo"]);
                /* #CC07 Add End */
            }
            /* #CC05 Added start*/
             
            try
            {
                DataTable dt = new DataTable();
                reader.NextResult();
                dt.Load(reader);
                dtBankName = dt.Copy();
                dt = null;
            }
            catch (Exception ex) { }
            /* #CC05 Added end*/
            Error = Convert.ToString(objSqlParam[2].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

        }
        /// <summary>
        /// Toggle activation of selected record
        /// </summary>
        override public bool ToggleActivation()
        {
            //SqlParameter[] objSqlParam = new SqlParameter[3];
            //objSqlParam[0] = new SqlParameter("@JobSheetCustomerDetailID", JobSheetCustomerDetailID);
            //objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            //objSqlParam[1].Direction = ParameterDirection.Output;
            //objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            //objSqlParam[2].Direction = ParameterDirection.Output;
            //SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, "prcJobSheetCustomerDetail_Toggle", objSqlParam);
            //result = (Convert.ToInt16(objSqlParam[1].Value) == 0);
            //Error = Convert.ToString(objSqlParam[2].Value);
            //if (Error != string.Empty)
            //{
            //    throw new ArgumentException(Error);
            //}
            return false;
        }

        #endregion
    }

