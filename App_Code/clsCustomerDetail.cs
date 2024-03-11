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
Rakesh Goel     10-Jun-2011    #CC01, added additional parameter to update method
Dhiraj Kumar,   04-jan-2013     #CC02 - Add country details in customer detail
22-Aug-2016, Vijay Katiyar, #CC03 - Added CustomerTypeId property and pass in update function
31-Jan-2017, Vijay Katiyar, #CC04 - Added ModelType and customerbank detail tvp
06-Feb-2017, Vijay Katiyar, #CC05 - Set CallProjectType &	CustomerCategory 
 * 09-Feb-2018, Sumit Maurya, #CC06, New value provided in parameter to update GSTINNO (Done for Amararaja).

*/
    public class clsCustomerDetail : clsDetailMaster, IDisposable
    {
        public static string _ConnectionString = ConfigurationManager.ConnectionStrings["AppConString"].ConnectionString;

        public Int16 CustomerType
        {
            get;
            set;
        }
    
        #region Constructors
        public clsCustomerDetail()
        {
            
        }
        #endregion

        private string _Address2 = string.Empty;
        private int _EntityTypeId = 5;

        public int EntityTypeId 
        {
            get { return _EntityTypeId; }
            set { _EntityTypeId = value; }
        }


        public string Address2
        {
            get { return _Address2; }
            set { _Address2 = value; }
        }
                
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

		~clsCustomerDetail()
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
			objSqlParam[0] = new SqlParameter("@FirstName",FirstName);
			objSqlParam[1] = new SqlParameter("@MiddleName",MiddleName);
			objSqlParam[2] = new SqlParameter("@LastName",LastName);
			objSqlParam[3] = new SqlParameter("@CompanyName",CompanyName);
			objSqlParam[4] = new SqlParameter("@Age",Age);
			objSqlParam[5] = new SqlParameter("@DOB",DOB);
			objSqlParam[6] = new SqlParameter("@MarriageAnniversary",MarriageAnniversary);
			objSqlParam[7] = new SqlParameter("@StateId",StateId);
			objSqlParam[8] = new SqlParameter("@CityId",CityId);
			objSqlParam[9] = new SqlParameter("@LocalityID",LocalityID);
			objSqlParam[10] = new SqlParameter("@StateName",StateName);
			objSqlParam[11] = new SqlParameter("@CityName",CityName);
			objSqlParam[12] = new SqlParameter("@LocalityName",LocalityName);
			objSqlParam[13] = new SqlParameter("@Street",Street);
			objSqlParam[14] = new SqlParameter("@Address",Address);
			objSqlParam[15] = new SqlParameter("@Pincode",Pincode);
			objSqlParam[16] = new SqlParameter("@MobileNo",MobileNo);
			objSqlParam[17] = new SqlParameter("@AltMobileNo",AltMobileNo);
			objSqlParam[18] = new SqlParameter("@PhoneNo",PhoneNo);
			objSqlParam[19] = new SqlParameter("@AltPhoneNo",AltPhoneNo);
			objSqlParam[20] = new SqlParameter("@EmailID",EmailID);
			objSqlParam[21] = new SqlParameter("@Out_Param",SqlDbType.TinyInt, 2); 
			objSqlParam[21].Direction = ParameterDirection.Output;
			objSqlParam[22] = new SqlParameter("@PKId",SqlDbType.BigInt, 8); 
			objSqlParam[22].Direction = ParameterDirection.Output;
			objSqlParam[23] = new SqlParameter("@Out_Error",SqlDbType.VarChar, 500); 
			objSqlParam[23].Direction = ParameterDirection.Output;
            
            SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, "prcCustomerDetail_Insert", objSqlParam);
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
            SqlParameter[] objSqlParam = new SqlParameter[34]; /* #CC03 Changed 28 to 29*//* #CC04 Changed 29 to 31*//* #CC05 Changed 31 to 33*/ /* ##cc06 Length increased from 33 to 34*/
			objSqlParam[0] = new SqlParameter("@CustomerDetailId",DetailId);
			objSqlParam[1] = new SqlParameter("@FirstName",FirstName);
			objSqlParam[2] = new SqlParameter("@MiddleName",MiddleName);
			objSqlParam[3] = new SqlParameter("@LastName",LastName);
			objSqlParam[4] = new SqlParameter("@CompanyName",CompanyName);
			objSqlParam[5] = new SqlParameter("@Age",Age);
			objSqlParam[6] = new SqlParameter("@DOB",DOB);
			objSqlParam[7] = new SqlParameter("@MarriageAnniversary",MarriageAnniversary);
			objSqlParam[8] = new SqlParameter("@StateId",StateId);
			objSqlParam[9] = new SqlParameter("@CityId",CityId);
			objSqlParam[10] = new SqlParameter("@LocalityID",LocalityID);
			objSqlParam[11] = new SqlParameter("@StateName",StateName);
			objSqlParam[12] = new SqlParameter("@CityName",CityName);
			objSqlParam[13] = new SqlParameter("@LocalityName",LocalityName);
			objSqlParam[14] = new SqlParameter("@Street",Street);
			objSqlParam[15] = new SqlParameter("@Address",Address);
			objSqlParam[16] = new SqlParameter("@Pincode",Pincode);
			objSqlParam[17] = new SqlParameter("@MobileNo",MobileNo);
			objSqlParam[18] = new SqlParameter("@AltMobileNo",AltMobileNo);
			objSqlParam[19] = new SqlParameter("@PhoneNo",PhoneNo);
			objSqlParam[20] = new SqlParameter("@AltPhoneNo",AltPhoneNo);
			objSqlParam[21] = new SqlParameter("@EmailID",EmailID);
			objSqlParam[22] = new SqlParameter("@Out_Param",SqlDbType.TinyInt, 2); 
			objSqlParam[22].Direction = ParameterDirection.Output;
			objSqlParam[23] = new SqlParameter("@Out_Error",SqlDbType.VarChar, 500); 
			objSqlParam[23].Direction = ParameterDirection.Output;
            objSqlParam[24] = new SqlParameter("@LandMark", LandMark);  //Changed add landmark parameter Pankaj Kumar
            objSqlParam[25] = new SqlParameter("@UserID",UserLogIn );  //#CC01 - added logged userID parameter
            /*(Start: #CC02 - Added)*/
            objSqlParam[26] = new SqlParameter("@CountryID", CountryId);
            objSqlParam[27] = new SqlParameter("@CountryName", CountryName);
            /*(End: #CC02 - Added)*/
            objSqlParam[28] = new SqlParameter("@CustomerTypeId", CustomerTypeId); /*#CC03 Added*/
                        

            /*#CC04:added start*/
            objSqlParam[29] = new SqlParameter("@ModelType", ModelType);

            if (dtBankName != null && dtBankName.Rows.Count > 0)
            {
                objSqlParam[30] = new SqlParameter("@TvpBankName", SqlDbType.Structured);
                objSqlParam[30].Value = dtBankName;
            }
            /* #CC04 Added end*/

            /* #CC05 Added start*/
            objSqlParam[31] = new SqlParameter("@CallProjectType", CallProjectType);
            objSqlParam[32] = new SqlParameter("@CustomerCategory", CustomerCategory);
            /* #CC05 Added end*/

            objSqlParam[33] = new SqlParameter("@GSTINNO", GSTINNo); /* #CC06 Added */

			SqlHelper.ExecuteNonQuery(_ConnectionString,CommandType.StoredProcedure,"prcCustomerDetail_Update", objSqlParam);
			result = Convert.ToInt16(objSqlParam[22].Value);
			Error = Convert.ToString(objSqlParam[23].Value);
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
			objSqlParam[0]=new SqlParameter("@CustomerDetailId",DetailId);
			objSqlParam[1]=new SqlParameter("@Out_Param",SqlDbType.TinyInt, 2); 
			objSqlParam[1].Direction = ParameterDirection.Output;
			objSqlParam[2] = new SqlParameter("@Out_Error",SqlDbType.VarChar, 500); 
			objSqlParam[2].Direction = ParameterDirection.Output;
			SqlHelper.ExecuteNonQuery(_ConnectionString,CommandType.StoredProcedure,"prcCustomerDetail_Delete",objSqlParam);
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
			objSqlParam[0]=new SqlParameter("@PageIndex",PageIndex); 
			objSqlParam[1]=new SqlParameter("@PageSize",PageSize); 
			objSqlParam[2]=new SqlParameter("@TotalRecord",SqlDbType.BigInt, 8); 
			objSqlParam[2].Direction = ParameterDirection.Output;
			objSqlParam[3] = new SqlParameter("@Out_Error",SqlDbType.VarChar, 500); 
			objSqlParam[3].Direction = ParameterDirection.Output;
			DataSet dsResult = SqlHelper.ExecuteDataset(_ConnectionString,CommandType.StoredProcedure,"prcCustomerDetail_Select",objSqlParam);
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
			objSqlParam[0]=new SqlParameter("@CustomerDetailId",DetailId);
			objSqlParam[1]=new SqlParameter("@Out_Param",SqlDbType.TinyInt, 2); 
			objSqlParam[1].Direction = ParameterDirection.Output;
			objSqlParam[2] = new SqlParameter("@Out_Error",SqlDbType.VarChar, 500); 
			objSqlParam[2].Direction = ParameterDirection.Output;
			DataSet dsResult = SqlHelper.ExecuteDataset(_ConnectionString,CommandType.StoredProcedure,"prcCustomerDetail_SelectById",objSqlParam);
			if (dsResult != null && dsResult.Tables.Count > 0)
			dtResult = dsResult.Tables[0];
			Error = Convert.ToString(objSqlParam[2].Value);
			if (Error != string.Empty)
			{
				throw new ArgumentException(Error);
			}

			return dtResult;
		}

        public DataTable SearchDuplicateRecords()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[5];
            objSqlParam[0] = new SqlParameter("@Phone", PhoneNo);
            objSqlParam[1] = new SqlParameter("@Mobile", MobileNo);
            objSqlParam[2] = new SqlParameter("@Address", Address + Street);
            objSqlParam[3] = new SqlParameter("@CityID", CityId);
            objSqlParam[4] = new SqlParameter("@CustomerType", CustomerType);

            DataSet dsResult = SqlHelper.ExecuteDataset(_ConnectionString, CommandType.StoredProcedure, "prcCustomer_SearchDuplicateRecords_ver2", objSqlParam);

            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];

            return dtResult;
        }
		
		/// <summary>
        /// Get All records from database for selected key
        /// </summary>
        /// <results>DataTable: Collection of records</results> 		
		override public void Load()
		{
			SqlParameter[] objSqlParam = new SqlParameter[3];
			objSqlParam[0]=new SqlParameter("@CustomerDetailId",DetailId);
			objSqlParam[1]=new SqlParameter("@Out_Param",SqlDbType.TinyInt, 2); 
			objSqlParam[1].Direction = ParameterDirection.Output;
			objSqlParam[2] = new SqlParameter("@Out_Error",SqlDbType.VarChar, 500); 
			objSqlParam[2].Direction = ParameterDirection.Output;
			IDataReader reader = SqlHelper.ExecuteReader(_ConnectionString,CommandType.StoredProcedure,"prcCustomerDetail_SelectById",objSqlParam);
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
                if (reader["Age"] != DBNull.Value)
                    Age = Convert.ToInt16(reader["Age"]);
                if (reader["DOB"] != DBNull.Value)
                    DOB = Convert.ToDateTime(reader["DOB"]);
                if (reader["MarriageAnniversary"] != DBNull.Value)
                    MarriageAnniversary = Convert.ToDateTime(reader["MarriageAnniversary"]);
                if (reader["CountryID"] != DBNull.Value)
                    CountryId = Convert.ToInt16(reader["CountryID"]);
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
                if (reader["LandMark"] != DBNull.Value)
                    LandMark = Convert.ToString(reader["LandMark"]);        //changed Paankaj Kumar
                if (reader["STDCode"] != DBNull.Value)
                    STDCode = Convert.ToString(reader["STDCode"]);
                /* #CC03 Added start*/
                if (reader["CustomerTypeID"] != DBNull.Value)
                    CustomerTypeId = Convert.ToInt16(reader["CustomerTypeID"]);
                if (reader["EditMode"] != DBNull.Value)
                    CustomerEditMode = Convert.ToInt16(reader["EditMode"]);
                /* #CC03 Added end*/
                /* #CC04 Added start*/
                if (reader["ModelType"] != DBNull.Value)
                    ModelType = Convert.ToString(reader["ModelType"]);
                /* #CC04 Added end*/
                /* #CC05 Added start*/
                if (reader["CallProjectType"] != DBNull.Value)
                    CallProjectType = Convert.ToString(reader["CallProjectType"]);
                if (reader["CustomerCategory"] != DBNull.Value)
                    CustomerCategory = Convert.ToString(reader["CustomerCategory"]);
                /* #CC05 Added end*/
                /* #CC06 Add Start */
                if(reader["GSTINNO"]!=DBNull.Value)
                    GSTINNo = Convert.ToString(reader["GSTINNo"]);
                    /* #CC06 Add End */

            }
            /* #CC04 Added start*/
            try
            {
                DataTable dt = new DataTable();
                reader.NextResult();
                dt.Load(reader);
                dtBankName = dt.Copy();
                dt = null;
            }
            catch (Exception ex) { }
            /* #CC04 Added end*/
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
			objSqlParam[0]=new SqlParameter("@CustomerDetailId",CustomerDetailId);
			objSqlParam[1]=new SqlParameter("@Out_Param",SqlDbType.TinyInt, 2); 
			objSqlParam[1].Direction = ParameterDirection.Output;
			objSqlParam[2] = new SqlParameter("@Out_Error",SqlDbType.VarChar, 500); 
			objSqlParam[2].Direction = ParameterDirection.Output;
			SqlHelper.ExecuteNonQuery(_ConnectionString,CommandType.StoredProcedure,"prcCustomerDetail_Toggle",objSqlParam);
			result = (Convert.ToInt16(objSqlParam[1].Value) == 0);
			Error = Convert.ToString(objSqlParam[2].Value);
			if (Error != string.Empty)
			{
				throw new ArgumentException(Error);
			}*/
            return false;
		}
		#endregion

        /// <summary>
        /// Get All records from database for selected key
        /// </summary>
        /// <results>DataTable: Collection of records</results> 		
        public DataTable SelectTransport()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[2];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);            
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@EntityTypeID", EntityTypeId);
            DataSet dsResult = SqlHelper.ExecuteDataset(_ConnectionString, CommandType.StoredProcedure, "prcEntityMaster_SelectTransport", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[0].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return dtResult;
        }
    }

