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
Amit Bhardwaj		9/30/2010	Created
Shilpi Sharma       07-May-2013 #CC01: Change Parameter Name From UserDetailId To EntityiD.
Shilpi Sharma       24-Sep-2013 #CC02: added function to check company name uniqueness.
*/


    public class clsUserDetail : clsDetailMaster, IDisposable
    {
        public static string _ConnectionString = ConfigurationManager.ConnectionStrings["AppConString"].ConnectionString;

        //Prashant Chitransh (1st april)new properties added on as new fields added in database.
        private string _strCompanyDisplayName;
        private string _strEntitySapCode;
        private string _strPANNo;
        private string _strServiceTaxNo;
        private string _strUserLoginName;
        private Int16 _intIsLockedOutStatus;
        public Int16 IsLockedOutStatus
        {
            get
            {
                return _intIsLockedOutStatus;
            }
            set
            {
                _intIsLockedOutStatus = value;
            }
        }
        
        public string UserLoginName
        {
            get { return _strUserLoginName; }
            set { _strUserLoginName = value; }
        }

        public string CompanyDisplayName
        {
            get { return _strCompanyDisplayName; }
            set { _strCompanyDisplayName = value; }
        }

        public string EntitySapCode
        {
            get { return _strEntitySapCode; }
            set { _strEntitySapCode = value; }
        }

        public string PANNo
        {
            get { return _strPANNo; }
            set { _strPANNo = value; }
        }

        public string ServiceTaxNo
        {
            get { return _strServiceTaxNo; }
            set { _strServiceTaxNo = value; }
        }


        #region Constructors
        public clsUserDetail()
        {

        }
        #endregion

        #region Dispose
        private bool IsDisposed = false;

        //Call Dispose to free resources explicitly
        new public void Dispose()
        {
            //Pass true in dispose method to clean managed resources too and say GC to skip finalize 
            // in next line.
            Dispose(true);
            //If dispose is called already then say GC to skip finalize on this instance.
            GC.SuppressFinalize(this);
        }

        ~clsUserDetail()
        {
            //Pass false as param because no need to free managed resources when you call finalize it
            //  will be done
            //by GC itself as its work of finalize to manage managed resources.
            Dispose(false);
        }

        //Implement dispose to free resources
        new protected virtual void Dispose(bool disposedStatus)
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
            SqlParameter[] objSqlParam = new SqlParameter[24];
            objSqlParam[0] = new SqlParameter("@FirstName", FirstName);
            objSqlParam[1] = new SqlParameter("@MiddleName", MiddleName);
            objSqlParam[2] = new SqlParameter("@LastName", LastName);
            objSqlParam[3] = new SqlParameter("@CompanyName", CompanyName);
            objSqlParam[4] = new SqlParameter("@Age", Age);
            objSqlParam[5] = new SqlParameter("@DOB", DOB);
            objSqlParam[6] = new SqlParameter("@MarriageAnniversary", MarriageAnniversary);
            objSqlParam[7] = new SqlParameter("@StateId", StateId);
            objSqlParam[8] = new SqlParameter("@CityId", CityId);
            objSqlParam[9] = new SqlParameter("@LocalityID", LocalityID);
            objSqlParam[10] = new SqlParameter("@StateName", StateName);
            objSqlParam[11] = new SqlParameter("@CityName", CityName);
            objSqlParam[12] = new SqlParameter("@LocalityName", LocalityName);
            objSqlParam[13] = new SqlParameter("@Street", Street);
            objSqlParam[14] = new SqlParameter("@Address", Address);
            objSqlParam[15] = new SqlParameter("@Pincode", Pincode);
            objSqlParam[16] = new SqlParameter("@MobileNo", MobileNo);
            objSqlParam[17] = new SqlParameter("@AltMobileNo", AltMobileNo);
            objSqlParam[18] = new SqlParameter("@PhoneNo", PhoneNo);
            objSqlParam[19] = new SqlParameter("@AltPhoneNo", AltPhoneNo);
            objSqlParam[20] = new SqlParameter("@EmailID", EmailID);
            objSqlParam[21] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[21].Direction = ParameterDirection.Output;
            objSqlParam[22] = new SqlParameter("@PKId", SqlDbType.BigInt, 8);
            objSqlParam[22].Direction = ParameterDirection.Output;
            objSqlParam[23] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[23].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, "prcUserDetail_Insert", objSqlParam);
            result = Convert.ToInt16(objSqlParam[21].Value);
            DetailId = Convert.ToInt64(objSqlParam[22].Value);
            Error = Convert.ToString(objSqlParam[23].Value);
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
            SqlParameter[] objSqlParam = new SqlParameter[24];
            objSqlParam[0] = new SqlParameter("@UserDetailId", DetailId);
            objSqlParam[1] = new SqlParameter("@FirstName", FirstName);
            objSqlParam[2] = new SqlParameter("@MiddleName", MiddleName);
            objSqlParam[3] = new SqlParameter("@LastName", LastName);
            objSqlParam[4] = new SqlParameter("@CompanyName", CompanyName);
            objSqlParam[5] = new SqlParameter("@Age", Age);
            objSqlParam[6] = new SqlParameter("@DOB", DOB);
            objSqlParam[7] = new SqlParameter("@MarriageAnniversary", MarriageAnniversary);
            objSqlParam[8] = new SqlParameter("@StateId", StateId);
            objSqlParam[9] = new SqlParameter("@CityId", CityId);
            objSqlParam[10] = new SqlParameter("@LocalityID", LocalityID);
            objSqlParam[11] = new SqlParameter("@StateName", StateName);
            objSqlParam[12] = new SqlParameter("@CityName", CityName);
            objSqlParam[13] = new SqlParameter("@LocalityName", LocalityName);
            objSqlParam[14] = new SqlParameter("@Street", Street);
            objSqlParam[15] = new SqlParameter("@Address", Address);
            objSqlParam[16] = new SqlParameter("@Pincode", Pincode);
            objSqlParam[17] = new SqlParameter("@MobileNo", MobileNo);
            objSqlParam[18] = new SqlParameter("@AltMobileNo", AltMobileNo);
            objSqlParam[19] = new SqlParameter("@PhoneNo", PhoneNo);
            objSqlParam[20] = new SqlParameter("@AltPhoneNo", AltPhoneNo);
            objSqlParam[21] = new SqlParameter("@EmailID", EmailID);
            objSqlParam[22] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[22].Direction = ParameterDirection.Output;
            objSqlParam[23] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[23].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, "prcUserDetail_Update", objSqlParam);
            result = Convert.ToInt16(objSqlParam[22].Value);
            Error = Convert.ToString(objSqlParam[23].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return result;
        }

        /// <summary>
        /// Save records in database.
        /// </summary>
        /// <results>Int16: 0 if success</results> 
        public Int16 SaveUserLogin(int UserRoleID, int LoginID)
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[39];
            objSqlParam[0] = new SqlParameter("@PKId", SqlDbType.SmallInt, 4);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@FirstName", FirstName);
            objSqlParam[2] = new SqlParameter("@MiddleName", MiddleName);
            objSqlParam[3] = new SqlParameter("@LastName", LastName);
            objSqlParam[4] = new SqlParameter("@CompanyName", CompanyName);
            objSqlParam[5] = new SqlParameter("@Age", Age);
            objSqlParam[6] = new SqlParameter("@DOB", DOB);
            objSqlParam[7] = new SqlParameter("@MarriageAnniversary", MarriageAnniversary);
            objSqlParam[8] = new SqlParameter("@StateId", StateId);
            objSqlParam[9] = new SqlParameter("@CityId", CityId);
            objSqlParam[10] = new SqlParameter("@LocalityID", LocalityID);
            objSqlParam[11] = new SqlParameter("@StateName", StateName);
            objSqlParam[12] = new SqlParameter("@CityName", CityName);
            objSqlParam[13] = new SqlParameter("@LocalityName", LocalityName);
            objSqlParam[14] = new SqlParameter("@Street", Street);
            objSqlParam[15] = new SqlParameter("@Address", Address);
            objSqlParam[16] = new SqlParameter("@Pincode", Pincode);
            objSqlParam[17] = new SqlParameter("@MobileNo", MobileNo);
            objSqlParam[18] = new SqlParameter("@AltMobileNo", AltMobileNo);
            objSqlParam[19] = new SqlParameter("@PhoneNo", PhoneNo);
            objSqlParam[20] = new SqlParameter("@AltPhoneNo", AltPhoneNo);
            objSqlParam[21] = new SqlParameter("@EmailID", EmailID);
            objSqlParam[22] = new SqlParameter("@EmployeeID", EmployeeID);
            objSqlParam[23] = new SqlParameter("@Password", Password);
            objSqlParam[24] = new SqlParameter("@PasswordSalt", PasswordSalt);
            //objSqlParam[25] = new SqlParameter("@LastLoginOn", LastLoginOn);
            //objSqlParam[26] = new SqlParameter("@PasswordLastUpdatedOn", PasswordLastUpdatedOn);
            //objSqlParam[27] = new SqlParameter("@LastLockedOn", LastLockedOn);
            objSqlParam[28] = new SqlParameter("@IsLockedOut", IsLockedOut);
            objSqlParam[29] = new SqlParameter("@FailedPasswordAttemptCount", FailedPasswordAttemptCount);
            objSqlParam[30] = new SqlParameter("@LandMark", LandMark);
            objSqlParam[32] = new SqlParameter("@active", Active);
            objSqlParam[33] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[33].Direction = ParameterDirection.Output;
            objSqlParam[34] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[34].Direction = ParameterDirection.Output;
            objSqlParam[35] = new SqlParameter("@RoleDetailID", RoleEntityId);
            objSqlParam[36] = new SqlParameter("@UserRoleID", UserRoleID);
            objSqlParam[37] = new SqlParameter("@LogInUserID", LoginID);
            objSqlParam[38] = new SqlParameter("@UserLoginName", SqlDbType.NVarChar,14);
            objSqlParam[38].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, "prcInsertUser_Login", objSqlParam);
            result = Convert.ToInt16(objSqlParam[33].Value);
            Error = Convert.ToString(objSqlParam[34].Value);
            UserLogIn = Convert.ToString(objSqlParam[38].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }
            return result;
        }
        //Added By Mamta Singh on 14 Sep 2011 to Include Brand mapping for creating user
        public Int16 SaveUserLogin(int UserRoleID, int LoginID,string strXmlBrandMapping)
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[36];
            objSqlParam[0] = new SqlParameter("@PKId", SqlDbType.SmallInt, 4);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@FirstName", FirstName);
            objSqlParam[2] = new SqlParameter("@MiddleName", MiddleName);
            objSqlParam[3] = new SqlParameter("@LastName", LastName);
            objSqlParam[4] = new SqlParameter("@CompanyName", CompanyName);
            objSqlParam[5] = new SqlParameter("@Age", Age);
            objSqlParam[6] = new SqlParameter("@DOB", DOB);
            objSqlParam[7] = new SqlParameter("@MarriageAnniversary", MarriageAnniversary);
            objSqlParam[8] = new SqlParameter("@StateId", StateId);
            objSqlParam[9] = new SqlParameter("@CityId", CityId);
            objSqlParam[10] = new SqlParameter("@LocalityID", LocalityID);
            objSqlParam[11] = new SqlParameter("@StateName", StateName);
            objSqlParam[12] = new SqlParameter("@CityName", CityName);
            objSqlParam[13] = new SqlParameter("@LocalityName", LocalityName);
            objSqlParam[14] = new SqlParameter("@Street", Street);
            objSqlParam[15] = new SqlParameter("@Address", Address);
            objSqlParam[16] = new SqlParameter("@Pincode", Pincode);
            objSqlParam[17] = new SqlParameter("@MobileNo", MobileNo);
            objSqlParam[18] = new SqlParameter("@AltMobileNo", AltMobileNo);
            objSqlParam[19] = new SqlParameter("@PhoneNo", PhoneNo);
            objSqlParam[20] = new SqlParameter("@AltPhoneNo", AltPhoneNo);
            objSqlParam[21] = new SqlParameter("@EmailID", EmailID);
            objSqlParam[22] = new SqlParameter("@EmployeeID", EmployeeID);
            objSqlParam[23] = new SqlParameter("@Password", Password);
            objSqlParam[24] = new SqlParameter("@PasswordSalt", PasswordSalt);
            objSqlParam[25] = new SqlParameter("@IsLockedOut", IsLockedOut);
            objSqlParam[26] = new SqlParameter("@FailedPasswordAttemptCount", FailedPasswordAttemptCount);
            objSqlParam[27] = new SqlParameter("@LandMark", LandMark);
            objSqlParam[28] = new SqlParameter("@active", Active);
            objSqlParam[29] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[29].Direction = ParameterDirection.Output;
            objSqlParam[30] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[30].Direction = ParameterDirection.Output;
            objSqlParam[31] = new SqlParameter("@RoleDetailID", RoleEntityId);
            objSqlParam[32] = new SqlParameter("@UserRoleID", UserRoleID);
            objSqlParam[33] = new SqlParameter("@LogInUserID", LoginID);
            objSqlParam[34] = new SqlParameter("@UserLoginName", SqlDbType.NVarChar, 14);
            objSqlParam[34].Direction = ParameterDirection.Output;
            objSqlParam[35] = new SqlParameter("@strXmlBrandMapping", strXmlBrandMapping);
            SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, "prcInsertUser_LoginVersion2", objSqlParam);
            result = Convert.ToInt16(objSqlParam[29].Value);
            Error = Convert.ToString(objSqlParam[30].Value);
           UserLogIn = Convert.ToString(objSqlParam[34].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }
            return result;
        }
        public Int64 UpdateUserLogin(int EntityID,int intid, int loginUserID, string strXMlBrandMapping)
        {
            Int64 result = 1;
            UserDetailID = intid;
            SqlParameter[] objSqlParam = new SqlParameter[30];
            objSqlParam[0] = new SqlParameter("@PKId", SqlDbType.SmallInt, 4);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@FirstName", FirstName);
            objSqlParam[2] = new SqlParameter("@MiddleName", MiddleName);
            objSqlParam[3] = new SqlParameter("@LastName", LastName);
            objSqlParam[4] = new SqlParameter("@CompanyName", CompanyName);
            objSqlParam[5] = new SqlParameter("@Age", Age);
            objSqlParam[6] = new SqlParameter("@DOB", DOB);
            objSqlParam[7] = new SqlParameter("@MarriageAnniversary", MarriageAnniversary);
            objSqlParam[8] = new SqlParameter("@StateId", StateId);
            objSqlParam[9] = new SqlParameter("@CityId", CityId);
            objSqlParam[10] = new SqlParameter("@LocalityID", LocalityID);
            objSqlParam[11] = new SqlParameter("@StateName", StateName);
            objSqlParam[12] = new SqlParameter("@CityName", CityName);
            objSqlParam[13] = new SqlParameter("@LocalityName", LocalityName);
            objSqlParam[14] = new SqlParameter("@Street", Street);
            objSqlParam[15] = new SqlParameter("@Address", Address);
            objSqlParam[16] = new SqlParameter("@Pincode", Pincode);
            objSqlParam[17] = new SqlParameter("@MobileNo", MobileNo);
            objSqlParam[18] = new SqlParameter("@AltMobileNo", AltMobileNo);
            objSqlParam[19] = new SqlParameter("@PhoneNo", PhoneNo);
            objSqlParam[20] = new SqlParameter("@AltPhoneNo", AltPhoneNo);
            objSqlParam[21] = new SqlParameter("@EmailID", EmailID);
            objSqlParam[22] = new SqlParameter("@EmployeeID", EmployeeID);
            objSqlParam[23] = new SqlParameter("@userdetailid", UserDetailID);
            objSqlParam[24] = new SqlParameter("@LandMark", LandMark);
            objSqlParam[25] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[25].Direction = ParameterDirection.Output;
            objSqlParam[26] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[26].Direction = ParameterDirection.Output;
            objSqlParam[27] = new SqlParameter("@LoginUserID", loginUserID);
            objSqlParam[28] = new SqlParameter("@strXMlBrandMapping", strXMlBrandMapping);
            objSqlParam[29] = new SqlParameter("@EntityID", EntityID);
            SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, "prcUpdateUser_LoginVersion2", objSqlParam);
            result = Convert.ToInt64(objSqlParam[25].Value);
            Error = Convert.ToString(objSqlParam[26].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }
            return result;
        }
        public DataSet SelectUserBrandcategoryMappingByUserDetailID()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[1];
            objSqlParam[0] = new SqlParameter("@UserDetailId", UserDetailID);
            DataSet dsResult = SqlHelper.ExecuteDataset(_ConnectionString, CommandType.StoredProcedure, "prcSelectBrandUserMapping", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            return dsResult;
        }
        public Int16 GenerateUserPassword()
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[6];
            objSqlParam[1] = new SqlParameter("@Password", Password);
            objSqlParam[2] = new SqlParameter("@PasswordSalt", PasswordSalt);
            objSqlParam[3] = new SqlParameter("@UserDetailID", UserDetailID);
            objSqlParam[4] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[4].Direction = ParameterDirection.Output;
            objSqlParam[5] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[5].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, "prcGenerateUserPassword", objSqlParam);
            result = Convert.ToInt16(objSqlParam[4].Value);
            Error = Convert.ToString(objSqlParam[5].Value);
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
        public Int64 UpdateUserLogin(int intid,int loginUserID)
        {
            Int64 result = 1;
            UserDetailID = intid;
            SqlParameter[] objSqlParam = new SqlParameter[28];
            objSqlParam[0] = new SqlParameter("@PKId", SqlDbType.SmallInt, 4);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@FirstName", FirstName);
            objSqlParam[2] = new SqlParameter("@MiddleName", MiddleName);
            objSqlParam[3] = new SqlParameter("@LastName", LastName);
            objSqlParam[4] = new SqlParameter("@CompanyName", CompanyName);
            objSqlParam[5] = new SqlParameter("@Age", Age);
            objSqlParam[6] = new SqlParameter("@DOB", DOB);
            objSqlParam[7] = new SqlParameter("@MarriageAnniversary", MarriageAnniversary);
            objSqlParam[8] = new SqlParameter("@StateId", StateId);
            objSqlParam[9] = new SqlParameter("@CityId", CityId);
            objSqlParam[10] = new SqlParameter("@LocalityID", LocalityID);
            objSqlParam[11] = new SqlParameter("@StateName", StateName);
            objSqlParam[12] = new SqlParameter("@CityName", CityName);
            objSqlParam[13] = new SqlParameter("@LocalityName", LocalityName);
            objSqlParam[14] = new SqlParameter("@Street", Street);
            objSqlParam[15] = new SqlParameter("@Address", Address);
            objSqlParam[16] = new SqlParameter("@Pincode", Pincode);
            objSqlParam[17] = new SqlParameter("@MobileNo", MobileNo);
            objSqlParam[18] = new SqlParameter("@AltMobileNo", AltMobileNo);
            objSqlParam[19] = new SqlParameter("@PhoneNo", PhoneNo);
            objSqlParam[20] = new SqlParameter("@AltPhoneNo", AltPhoneNo);
            objSqlParam[21] = new SqlParameter("@EmailID", EmailID);
            objSqlParam[22] = new SqlParameter("@EmployeeID", EmployeeID);
            objSqlParam[23] = new SqlParameter("@userdetailid", UserDetailID);
            objSqlParam[24] = new SqlParameter("@LandMark", LandMark);
            objSqlParam[25] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[25].Direction = ParameterDirection.Output;
            objSqlParam[26] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[26].Direction = ParameterDirection.Output;
            objSqlParam[27] = new SqlParameter("@LoginUserID", loginUserID);
            SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, "prcUpdateUser_Login", objSqlParam);
            result = Convert.ToInt64(objSqlParam[25].Value);
            Error = Convert.ToString(objSqlParam[26].Value);
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
            objSqlParam[0] = new SqlParameter("@UserDetailId", DetailId);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, "prcUserDetail_Delete", objSqlParam);
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
            DataSet dsResult = SqlHelper.ExecuteDataset(_ConnectionString, CommandType.StoredProcedure, "prcUserDetail_Select", objSqlParam);
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
        public DataTable SelectAllUserVersion2(Int16 EntityTypeId )
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[5];
            objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@EntityTypeId", EntityTypeId);
            DataSet dsResult = SqlHelper.ExecuteDataset(_ConnectionString, CommandType.StoredProcedure, "prcUserDetail_SelectVersion2", objSqlParam);
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
        public DataTable SelectAllRole()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.BigInt, 8);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[1].Direction = ParameterDirection.Output;
            DataSet dsResult = SqlHelper.ExecuteDataset(_ConnectionString, CommandType.StoredProcedure, "prcSearchRoleMaster_Select", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            //TotalRecords = Convert.ToInt32(objSqlParam[1].Value);
            Error = Convert.ToString(objSqlParam[1].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return dtResult;
        }
        public DataTable SelectAllRole(Int16 EntityTypeiD)
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@EntityTypeID", EntityTypeiD);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.BigInt, 8);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            DataSet dsResult = SqlHelper.ExecuteDataset(_ConnectionString, CommandType.StoredProcedure, "prcSearchRoleMaster_SelectForUserMaster", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            //TotalRecords = Convert.ToInt32(objSqlParam[1].Value);
            Error = Convert.ToString(objSqlParam[2].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return dtResult;
        }
       
        public DataTable SelectAllUserdetailIDinfo(int userdetailinfo)
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.BigInt, 8);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@UserDetailId", userdetailinfo);
            DataSet dsResult = SqlHelper.ExecuteDataset(_ConnectionString, CommandType.StoredProcedure, "prcSearchAllEntityInfo_Select", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            //TotalRecords = Convert.ToInt32(objSqlParam[1].Value);
            Error = Convert.ToString(objSqlParam[1].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return dtResult;
        }
        public DataTable SelectEntityOnRole(int RoleID)
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[4];
            objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.BigInt, 8);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@RoleID", RoleID);
            objSqlParam[3] = new SqlParameter("@Active", Active);
            DataSet dsResult = SqlHelper.ExecuteDataset(_ConnectionString, CommandType.StoredProcedure, "prcSearchEntityonRole_Select", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            //TotalRecords = Convert.ToInt32(objSqlParam[1].Value);
            Error = Convert.ToString(objSqlParam[1].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return dtResult;
        }

        public DataTable SelectByName(Int16 EntityTypeID, Int32 EntityID)
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[11];
            objSqlParam[0] = new SqlParameter("@FirstName", FirstName);
            objSqlParam[1] = new SqlParameter("@LastName", LastName);
            objSqlParam[2] = new SqlParameter("@UserRoleId", UserRoleId);
            objSqlParam[3] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[4] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[5] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[5].Direction = ParameterDirection.Output;
            objSqlParam[6] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[6].Direction = ParameterDirection.Output;
            objSqlParam[7] = new SqlParameter("@EntityTypeID", EntityTypeID);
            objSqlParam[8] = new SqlParameter("@EntityID", EntityID);
            objSqlParam[9] = new SqlParameter("@IsLockedOut", IsLockedOutStatus);
            objSqlParam[10] = new SqlParameter("@UserLoginName", UserLoginName);
            DataSet dsResult = SqlHelper.ExecuteDataset(_ConnectionString, CommandType.StoredProcedure, "prcUserDetailByName_Select", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            TotalRecords = Convert.ToInt32(objSqlParam[5].Value);
            Error = Convert.ToString(objSqlParam[6].Value);
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
            objSqlParam[0] = new SqlParameter("@UserDetailId", DetailId);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            DataSet dsResult = SqlHelper.ExecuteDataset(_ConnectionString, CommandType.StoredProcedure, "prcUserDetail_SelectById", objSqlParam);
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
        /// CompanyDisplayName, EntitySapCode, PANNo and ServiceTaxNo added(Prashant Chitransh - 4th Apr' 2011)
        override public void Load()
        {
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@EntityId", DetailId);/*#CC01:Changed*/
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            IDataReader reader = SqlHelper.ExecuteReader(_ConnectionString, CommandType.StoredProcedure, "prcEntityDetail_SelectById", objSqlParam);
            while (reader.Read())
            {
                if (reader["FirstName"] != DBNull.Value)
                    FirstName = Convert.ToString(reader["FirstName"]);
                if (reader["MiddleName"] != DBNull.Value)
                    MiddleName = Convert.ToString(reader["MiddleName"]);
                if (reader["LastName"] != DBNull.Value)
                    LastName = Convert.ToString(reader["LastName"]);
                if (reader["CompanyName"] != DBNull.Value)
                    CompanyName = Convert.ToString(reader["CompanyName"]);
               
                if (reader["DOB"] != DBNull.Value)
                    DOB = Convert.ToDateTime(reader["DOB"]);
                if (reader["MarriageAnniversary"] != DBNull.Value && Convert.ToString(reader["MarriageAnniversary"]) != "")
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
                if (reader["EntityCode"] != DBNull.Value)
                    EntityCode = Convert.ToString(reader["EntityCode"]);
                if (reader["LandMark"] != DBNull.Value)
                    LandMark = Convert.ToString(reader["LandMark"]);             
                if (reader["CompanyDisplayName"] != DBNull.Value)
                    CompanyDisplayName = Convert.ToString(reader["CompanyDisplayName"]);
                if (reader["EntitySapCode"] != DBNull.Value)
                    EntitySapCode = Convert.ToString(reader["EntitySapCode"]);
               
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
        override public bool ToggleActivation()
        {
            /*SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0]=new SqlParameter("@UserDetailId",UserDetailId);
            objSqlParam[1]=new SqlParameter("@Out_Param",SqlDbType.TinyInt, 2); 
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error",SqlDbType.VarChar, 500); 
            objSqlParam[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(_ConnectionString,CommandType.StoredProcedure,"prcUserDetail_Toggle",objSqlParam);
            result = (Convert.ToInt16(objSqlParam[1].Value) == 0);
            Error = Convert.ToString(objSqlParam[2].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }*/
            return false;
        }
        #endregion

        /*#CC02:added (start)*/
        public Boolean CheckCompanyUnique()
        {
            bool blResult=false; 
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@CompanyName", CompanyDisplayName);
            objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteDataset(_ConnectionString, CommandType.StoredProcedure, "[prcCompanyName_Unique]", objSqlParam);
            if (Convert.ToString(objSqlParam[2].Value) != null && Convert.ToString(objSqlParam[2].Value)!="") 
            blResult = Convert.ToBoolean(objSqlParam[2].Value);
            Error = Convert.ToString(objSqlParam[1].Value);
            return blResult;
        }
        /*#CC02:added (end)*/
    }

