#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Xml;
using MySql.Data.MySqlClient;
#endregion
/*Change Log:
 * 05-Aug-14, Rakesh Goel, #CC01 - Changes for transaction module restriction 
 * 27-May-2015,Karam Chand Sharma, #CC02, Add some properties and set value into function due to zedcontrol apply (Read menu id)
 * 22-Sep-2016, Sumit Maurya, #CC03, New paramter supplied  to filter data , And Mobile number supplied to save Mobile number while creation Mobile Number.
 * 02-Nov-2016, Sumit Maurya, #CC04, OutError supplied to get error message.
 * * 12-July-2017,Vijay Kumar Prajapati,#CC05,New Method For GetUserInfo.
 * 13-July-2017,Vijay kumar Prajapati,#CC06,New Method For Save and Update API User Request Type Mapping.
 * 17-Jul-2017, Sumit Maurya, #CC07, New parameter supplied .
 * 9-Jul-2018, Rakesh Raj, #CC08, Direct hit URL opens the page / Authentication fails/module authenticatoin  
 */


public class UserData : IDisposable
{

    #region Varible
    private Int32 intUserID;
    private string strUserLoginName, strError;
    private string strPassword;
    private string strPasswordSalt;
    private Int16 intUserRoleID, intOfficeLevel;
    private string strFirstName;
    private string strLastName;
    private string strDisplayName;
    private string strEmailID;
    private bool? blnStatus;
    private bool blnAllowHierarchy;
    private string strSelectedRegions;
    private string strMenuDescription;
    private int intCompanyID;
    private Int32 intActionId, intEntityTypeId, intBaseEntityTypeId;
    private Int32 intPasswordExpiryDays;
    private string strUploadSchemaXml;

    DataTable dtUserInfo, dtUserType;
    SqlParameter[] SqlParam;
    MySqlParameter[] MySqlParam;
    Int32 IntResultCount = 0;
    DataSet dsUserInfo;
    private Int16? intSalesChanelTypeID;
    private Int16? intHierarchyLevelID;
    private Int16 intSearchType;
    private string strRoleName;
    private byte byteIsUserMapped;
    private Int16 intsaleschannelLevel;
    private Int32 intSalesChannelID;
    private Int32 intReturnMenuID;/*#CC02 ADDED*/
    private Int32 intAPIUserId;/*#CC06 ADDED*/
    private Int32 BitRequestTypeId1;/*#CC06 ADDED*/
    private Int32 BitRequestTypeId2;/*#CC06 ADDED*/

    #endregion
    #region User Activity tracking and Menu permission Check
    /*#CC02 START ADDED*/
    public Int32 ReturnMenuID
    {
        get { return intReturnMenuID; }
        set { intReturnMenuID = value; }
    }/*#CC02 START END*/
    public string UserIP
    {
        get;
        set;
    }
    public string Error     //Pankaj Dhingra
    {
        get { return strError; }
        set { strError = value; }
    }

    public string UploadSchemaXml       //Pankaj Dhingra
    {
        get { return strUploadSchemaXml; }
        set { strUploadSchemaXml = value; }
    }
    public string UserServerIP
    {
        get;
        set;
    }

    public string MenuDescription
    {
        get { return strMenuDescription; }
        set { strMenuDescription = value; }
    }

    public string ErrorMessage
    {
        get;
        set;
    }

    //public bool isMenuRequestValid  //#CC01 commented
    public Int16 isMenuRequestValid  //#CC01 added
    {
        get;
        set;
    }
    #endregion

    #region Properties
        public int ActiveStatus { get; set; } /*#CC05 ADDED*/
    public Int32 UserID
    {
        get { return intUserID; }
        set { intUserID = value; }
    }
    public Int32 ActionId
    {
        get { return intActionId; }
        set { intActionId = value; }
    }
    public Int32 PasswordExpiryDays
    {
        get { return intPasswordExpiryDays; }
        set { intPasswordExpiryDays = value; }
    }
    public Int16 saleschannelLevel
    {
        get { return intsaleschannelLevel; }
        set { intsaleschannelLevel = value; }
    }
    public Int16? SalesChanelTypeID
    {
        get { return intSalesChanelTypeID; }
        set { intSalesChanelTypeID = value; }
    }
    public Int16? HierarchyLevelID
    {
        get { return intHierarchyLevelID; }
        set { intHierarchyLevelID = value; }
    }
    public string UserLoginName
    {
        get { return strUserLoginName; }
        set { strUserLoginName = value; }
    }
    public string Password
    {
        get { return strPassword; }
        set { strPassword = value; }
    }
    public string PasswordSalt
    {
        get { return strPasswordSalt; }
        set { strPasswordSalt = value; }
    }
    public Int16 UserRoleID
    {
        get { return intUserRoleID; }
        set { intUserRoleID = value; }
    }
    public string RoleName
    {
        get { return strRoleName; }
        set { strRoleName = value; }
    }
    public string FirstName
    {
        get { return strFirstName; }
        set { strFirstName = value; }
    }
    public string LastName
    {
        get { return strLastName; }
        set { strLastName = value; }
    }
    public string DisplayName
    {
        get { return strDisplayName; }
        set { strDisplayName = value; }
    }
    public string EmailID
    {
        get { return strEmailID; }
        set { strEmailID = value; }
    }
    public bool? Status
    {
        get { return blnStatus; }
        set { blnStatus = value; }
    }
    public bool AllowHierarchy
    {
        get { return blnAllowHierarchy; }
        set { blnAllowHierarchy = value; }
    }
    public string SelectedRegions
    {
        get { return strSelectedRegions; }
        set { strSelectedRegions = value; }
    }
    public int CompanyID
    {
        get { return intCompanyID; }
        set { intCompanyID = value; }
    }
    public int  BrandId {get;set;}
    public int ProdCatId { get; set; }
    public byte IsUserMapped
    {
        get { return byteIsUserMapped; }
        set { byteIsUserMapped = value; }
    }
    public Int16 OfficeLevel
    {
        get { return intOfficeLevel; }
        set { intOfficeLevel = value; }
    }
    public Int32 SalesChannelID
    {
        get { return intSalesChannelID; }
        set { intSalesChannelID = value; }
    }
    private int? _OtherEntityType;
    public int? OtherEntityType
    {
        get { return _OtherEntityType; }
        set { _OtherEntityType = value; }
    }
    private int _WAPAccess;
    public int WAPAccess
    {
        get { return _WAPAccess; }
        set { _WAPAccess = value; }
    }

    private string _strPassword;
    public string StrPassword
    {
        get { return _strPassword; }
        set { _strPassword = value; }
    }
    /* #CC03 Add Start */
    public string MobileNumber
    {
        get;
        set;
    }
    /* #CC03 Add End */
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public int TotalRecords { get; set; }
    public string UserIds { get; set; }
    /*#CC06 ADDED Started*/
    public Int32 BITRequestTypeID1
    {
        get { return BitRequestTypeId1; }
        set { BitRequestTypeId1 = value; }
    }
    public Int32 BITRequestTypeID2
    {
        get { return BitRequestTypeId2; }
        set { BitRequestTypeId2 = value; }
    }
    public Int32 APIUserId
    {
        get { return intAPIUserId; }
        set { intAPIUserId = value; }
    }
    /*#CC06 ADDED Started End*/
    public string Lat { get; set; }
    public string Long { get; set; }
    public string GeoRadius { get; set; }
    #endregion

    #region Fetch Other Information
    public DataTable GetAvailedLocations()
    {

        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@UserRoleID", intUserRoleID);
            SqlParam[1] = new SqlParameter("@CompanyID", CompanyID);
            dtUserInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetAvailedLocationsByUserRoleID", CommandType.StoredProcedure, SqlParam);
            return dtUserInfo;
        }
        catch (Exception ex)
        { throw ex; }

    }
    public DataTable prcGetAvailedLocationsByUserId(int UserID)
    {
        try
        {
            SqlParam = new SqlParameter[1];

            SqlParam[0] = new SqlParameter("@UserID", UserID);
            dtUserInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetAvailedLocations", CommandType.StoredProcedure, SqlParam);
            return dtUserInfo;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    public DataTable GetUserTypeForActivityReport()
    {
        try
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@Officelevel", intOfficeLevel);
            dtUserInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("GetUserTypeForActivityReport", CommandType.StoredProcedure, SqlParam);
            return dtUserInfo;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region Role Information

    public Int16 RoleId
    {
        get { return intUserRoleID; }
        set { intUserRoleID = value; }
    }

    public Int32 EntitytypeId
    {
        get { return intEntityTypeId; }
        set { intEntityTypeId = value; }
    }

    public Int32 BaseEntitytypeId
    {
        get { return intBaseEntityTypeId; }
        set { intBaseEntityTypeId = value; }
    }
    public Int16 SearchType
    {
        get { return intSearchType; }
        set { intSearchType = value; }
    }

    private int _RestrictUserID;//for restriction as superadmin userId
    public int RestrictUserID
    {
        get { return _RestrictUserID; }
        set { _RestrictUserID = value; }
    }



    public DataTable GetUserRoleHierarchy()
    {
        try
        {
            SqlParam = new SqlParameter[4];
            SqlParam[0] = new SqlParameter("@roleid", intUserRoleID);
            //SqlParam[1] = new SqlParameter("@hierarchylevelid", intHierarchyLevelID);
            //SqlParam[2] = new SqlParameter("@saleschannelLevelId", saleschannelLevel);
            SqlParam[1] = new SqlParameter("@EntityTypeId", EntitytypeId);
            SqlParam[2] = new SqlParameter("@BaseEntityTypeId", BaseEntitytypeId);
            SqlParam[3] = new SqlParameter("@SalesChannelLevel", saleschannelLevel);

            dtUserInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetUserRoleHierarchy", CommandType.StoredProcedure, SqlParam);
            return dtUserInfo;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

        public DataTable GetUserFromRole()
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@roleid", intUserRoleID);
                SqlParam[1] = new SqlParameter("@Status", ActiveStatus);/*#CC05 ADDED*/
                dtUserInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetUserFromRole", CommandType.StoredProcedure, SqlParam);
                return dtUserInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    public Int32 CheckRoleExistence()
    {
        try
        {
            int Result = 0;
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@RoleID", intUserRoleID);
            SqlParam[1] = new SqlParameter("@ResultOut", SqlDbType.Int);
            SqlParam[1].Direction = ParameterDirection.Output;
            IntResultCount = (DataAccess.DataAccess.Instance.DBInsertCommand("PrcChkUserRoleExistence", SqlParam));
            Result = Convert.ToInt32(SqlParam[1].Value);
            return Result;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }


    public DataTable GetUserRole()
    {
        try
        {
            SqlParam = new SqlParameter[7]; /* #CC07 length increased from 5-6 */
            SqlParam[0] = new SqlParameter("@RoleName", RoleName);
            SqlParam[1] = new SqlParameter("@Status", Status);
            SqlParam[2] = new SqlParameter("@HierarchyLevelID", HierarchyLevelID);
            SqlParam[3] = new SqlParameter("@SalesChanelTypeID", SalesChanelTypeID);
            SqlParam[4] = new SqlParameter("@SearchType", SearchType);
            SqlParam[5] = new SqlParameter("@UserID", UserID);/* #CC07 Added */
            SqlParam[5] = new SqlParameter("@CompanyID", CompanyID);
            dtUserInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetUserRoleInfoByParameter", CommandType.StoredProcedure, SqlParam);
            return dtUserInfo;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetUserRoleUserMaster()
    {
        try
        {
            SqlParam = new SqlParameter[6]; /* #CC07 length increased from 5-6 */
            SqlParam[0] = new SqlParameter("@CompanyID", CompanyID);
            //SqlParam[1] = new SqlParameter("@Status", Status);
            //SqlParam[2] = new SqlParameter("@SearchType", SearchType);
            //SqlParam[3] = new SqlParameter("@UserID", UserID);
            dtUserInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetUserRoleDetails", CommandType.StoredProcedure, SqlParam);
            return dtUserInfo;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    


    public DataTable GetUserRole(int RoleID)
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@RoleID", RoleID);
            SqlParam[1] = new SqlParameter("@CompanyID", CompanyID);
            dtUserInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetUserRoleInfoByParameter", CommandType.StoredProcedure, SqlParam);
            return dtUserInfo;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public Int32 UpdateStatusRoleInfo()
    {
        try
        {

            SqlParam = new SqlParameter[2]; /* #CC04 Length increased */
            SqlParam[0] = new SqlParameter("@RoleID", intUserRoleID);
            /* #CC04 Add Start */
            SqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 200);
            SqlParam[1].Direction = ParameterDirection.Output;
            IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("PrcUpdStatusRole", SqlParam);
            if (SqlParam[1].Value != DBNull.Value && SqlParam[1].Value.ToString() != "")
            {
                ErrorMessage = (SqlParam[1].Value).ToString();
            }/* #CC04 Add End */
            /*IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("PrcUpdStatusRole", SqlParam); #CC04 Commenetd */
            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public Int32 InsertUpdateRoleinfo()
    {
        try
        {
            SqlParam = new SqlParameter[8];
            SqlParam[0] = new SqlParameter("@RoleID", intUserRoleID);
            SqlParam[0].Direction = ParameterDirection.InputOutput;
            SqlParam[1] = new SqlParameter("@RoleName", strRoleName);
            SqlParam[2] = new SqlParameter("@SalesChanelTypeID", intSalesChanelTypeID);
            SqlParam[3] = new SqlParameter("@HierarchyLevelID", intHierarchyLevelID);
            SqlParam[4] = new SqlParameter("@OtherEntityType", OtherEntityType);
            SqlParam[5] = new SqlParameter("@Status", blnStatus);
            SqlParam[6] = new SqlParameter("@WAPAccess", WAPAccess);
            SqlParam[7] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 200);
            SqlParam[7].Direction = ParameterDirection.Output;
            DataAccess.DataAccess.Instance.DBInsertCommand("PrcInsUpdRoleInfo", SqlParam);
            IntResultCount = Convert.ToInt32(SqlParam[0].Value);
            if (SqlParam[7].Value != DBNull.Value && SqlParam[7].Value.ToString() != "")
            {
                ErrorMessage = (SqlParam[7].Value).ToString();
            }
            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }



    }
    public DataTable GetLocationListbyUserRoleID()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@UserRoleID", intUserRoleID);
            SqlParam[1] = new SqlParameter("@CompanyID", CompanyID);
            dtUserInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetLocationListByRole", CommandType.StoredProcedure, SqlParam);
            return dtUserInfo;
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
    #endregion

    #region User Information
    public Int32 InsertUpdateUserinfo()
    {
        try
        {
            SqlParam = new SqlParameter[23]; /* #CC03 Length increased from 18-19*/
            SqlParam[0] = new SqlParameter("@UserID", intUserID);
            SqlParam[0].Direction = ParameterDirection.InputOutput;
            SqlParam[1] = new SqlParameter("@UserLoginName", strUserLoginName);
            SqlParam[2] = new SqlParameter("@Password", strPassword);
            SqlParam[3] = new SqlParameter("@PasswordSalt", strPasswordSalt);
            SqlParam[4] = new SqlParameter("@UserTypeID", intUserRoleID);
            SqlParam[5] = new SqlParameter("@FirstName", strFirstName);
            SqlParam[6] = new SqlParameter("@LastName", strLastName);
            SqlParam[7] = new SqlParameter("@DisplayName", strDisplayName);
            SqlParam[8] = new SqlParameter("@EmailID", strEmailID);
            SqlParam[9] = new SqlParameter("@Status", blnStatus);
            SqlParam[10] = new SqlParameter("@UserRoleID", intUserRoleID);
            SqlParam[11] = new SqlParameter("@SelectedRegions", strSelectedRegions);
            SqlParam[12] = new SqlParameter("@IsUserMappedWithOrgn", byteIsUserMapped);
            SqlParam[13] = new SqlParameter("@AllowHierarchy", blnAllowHierarchy);
            SqlParam[14] = new SqlParameter("@PasswordExpiryDays", intPasswordExpiryDays);
            SqlParam[15] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[15].Direction = ParameterDirection.Output;
            SqlParam[16] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[16].Direction = ParameterDirection.Output;
            SqlParam[17] = new SqlParameter("@strPassword", StrPassword);
            SqlParam[18] = new SqlParameter("@Mobile", MobileNumber); /* #CC03 Added */
            SqlParam[19] = new SqlParameter("@CompanyID", CompanyID); /* #CC03 Added */
            SqlParam[20] = new SqlParameter("@Lat", Lat); /* #CC03 Added */
            SqlParam[21] = new SqlParameter("@Long", Long); /* #CC03 Added */
            SqlParam[22] = new SqlParameter("@GeoRadius", GeoRadius); /* #CC03 Added */
            DataAccess.DataAccess.Instance.DBInsertCommand("PrcInsUpdUserInfo", SqlParam);
            ErrorMessage = SqlParam[16].Value.ToString();
            IntResultCount = Convert.ToInt32(SqlParam[0].Value);
            ReturnValue = Convert.ToInt32(SqlParam[15].Value);
            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public Int32 ReturnValue
    {
        get;
        set;
    }
    public Int32 UpdateStatusUserInfo()
    {
        try
        {

            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@UserID", intUserID);
            SqlParam[1] = new SqlParameter("@CompanyID", CompanyID);
            IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("PrcUpdStatusUser", SqlParam);
            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public Int32 UpdateUserLoginStatus()
    {
        try
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@UserID", intUserID);
            SqlParam[1] = new SqlParameter("@ActionId", ActionId);
            SqlParam[2] = new SqlParameter("@CompanyId", CompanyID);
            IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("PrcUpdUserLoginStatus", SqlParam);
            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public DataTable GetUsersInfo(Int32 UserID, short ChkStatus)
    {
        try
        {
            SqlParam = new SqlParameter[4];
            SqlParam[0] = new SqlParameter("@UserID", UserID);
            SqlParam[1] = new SqlParameter("@ChkStatus", ChkStatus);
            SqlParam[2] = new SqlParameter("@RestrictUserID", RestrictUserID);
            SqlParam[2] = new SqlParameter("@CompanyID", CompanyID);
            dtUserInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("PrcGetUserInfoByParameters", CommandType.StoredProcedure, SqlParam);
            return dtUserInfo;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    public DataTable GetUsersInfo(string UserLoginName)
    {
        try
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@UserLoginName", strUserLoginName);
            dtUserInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("PrcGetUserInfoByParameters", CommandType.StoredProcedure, SqlParam);
            return dtUserInfo;
        }
        catch (Exception ex)
        {

            throw ex;
        }


    }
    public DataTable GetUsersInfo(short UserRoleID, short UserStatus, string DisplayName)
    {
        try
        {
            SqlParam = new SqlParameter[9]; /* #CC03 Length increaded from 4-6*/
            SqlParam[0] = new SqlParameter("@UserRoleID", UserRoleID);
            SqlParam[1] = new SqlParameter("@chkStatus", UserStatus);
            SqlParam[2] = new SqlParameter("@DisplayName", DisplayName);
            SqlParam[3] = new SqlParameter("@RestrictUserID", RestrictUserID);
            /* #CC03 Add Start */
            SqlParam[4] = new SqlParameter("@MobileNumber", MobileNumber);
            SqlParam[5] = new SqlParameter("@EmailId", EmailID); /* #CC03 Add End */
            SqlParam[6] = new SqlParameter("@CompanyID", CompanyID); /* #CC03 Add End */
            SqlParam[7] = new SqlParameter("@BrandId", BrandId); /* #CC03 Add End */
            SqlParam[8] = new SqlParameter("@ProdCatId", ProdCatId); /* #CC03 Add End */
            dtUserInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("PrcGetUserInfoByParameters", CommandType.StoredProcedure, SqlParam);
            return dtUserInfo;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    public DataTable GetUsersInfo(string UserLoginName, short CkhStatus, byte isHQExclude)
    {
        try
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@UserLoginName", strUserLoginName);
            SqlParam[1] = new SqlParameter("@chkStatus", CkhStatus);
            SqlParam[2] = new SqlParameter("@isHQExclude", isHQExclude);
            dtUserInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("PrcGetUserInfoByParameters", CommandType.StoredProcedure, SqlParam);
            return dtUserInfo;
        }
        catch (Exception ex)
        {

            throw ex;
        }


    }
    public DataTable GetUsersInfo(string UserLoginName, short UserStatus, byte isHQExclude, string EmailId)
    {
        try
        {
            SqlParam = new SqlParameter[3];

            SqlParam[0] = new SqlParameter("@UserLoginName", UserLoginName);
            SqlParam[1] = new SqlParameter("@chkStatus", UserStatus);
            SqlParam[2] = new SqlParameter("@EmailId", EmailId);
            dtUserInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("PrcGetUserInfoByParameters", CommandType.StoredProcedure, SqlParam);
            return dtUserInfo;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    public DataTable GetUsersInfoForgotPassword(string UserLoginName, string EmailId)
    {
        try
        {
            MySqlParam = new MySqlParameter[2];

            MySqlParam[0] = new MySqlParameter("@p_UserLoginName", UserLoginName);

            MySqlParam[1] = new MySqlParameter("@p_EmailId", EmailId);
            dtUserInfo = DataAccess.DataAccess.Instance.GetTableFrom_MySqlDatabase("PrcGetUserInfoForForgotPassword", CommandType.StoredProcedure, MySqlParam);
            return dtUserInfo;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    /// <summary>
    /// Fetch user hierarchy by userid
    /// </summary>
    /// <returns>data table</returns>
    public DataTable GetChildHierarchyByUserID()
    {
        try
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@UserID", intUserID);
            dtUserInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetHierarchyByUserID", CommandType.StoredProcedure, SqlParam);
            return dtUserInfo;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    #endregion

    public Int32 InsertUpdateUserTracking()
    {
        try
        {
            MySqlParam = new MySqlParameter[8];
            MySqlParam[0] = new MySqlParameter("@p_UserID", UserID);
            MySqlParam[1] = new MySqlParameter("@p_UserIP", UserIP);
            MySqlParam[2] = new MySqlParameter("@p_UserServerID", UserServerIP);
            MySqlParam[3] = new MySqlParameter("@p_MenuContaningWord", MenuDescription);
            MySqlParam[4] = new MySqlParameter("@p_ErrorMessage", MySqlDbType.VarChar, 200);
            MySqlParam[4].Direction = ParameterDirection.Output;
            MySqlParam[5] = new MySqlParameter("@p_PageHeading", MySqlDbType.VarChar, 200);
            MySqlParam[5].Direction = ParameterDirection.Output;
            //SqlParam[6] = new SqlParameter("@isMenuPermision", SqlDbType.Bit);  //#CC01 commented
            MySqlParam[6] = new MySqlParameter("@p_isMenuPermision", MySqlDbType.Int16);  //#CC01 added
            MySqlParam[6].Direction = ParameterDirection.Output;
            MySqlParam[7] = new MySqlParameter("@p_ReturnMenuID", MySqlDbType.Int32);/*#CC02 ADDED*/
            MySqlParam[7].Direction = ParameterDirection.Output;/*#CC02 ADDED*/
            IntResultCount = DataAccess.DataAccess.Instance.DBInsert_MySqlCommand("prcInsUpdUserTrackingActivity", MySqlParam);
            ErrorMessage = MySqlParam[4].Value.ToString();
            MenuDescription = MySqlParam[5].Value.ToString();

            //#CC08 Start
            if (string.IsNullOrEmpty(MySqlParam[7].Value.ToString()) == true)
            {
                ReturnMenuID = 0;
            }
            else
            {
                ReturnMenuID = Convert.ToInt32(MySqlParam[7].Value.ToString());/*#CC02 ADDED*/
            }
            
            //isMenuRequestValid = Convert.ToBoolean(SqlParam[6].Value.ToString());  //#CC01 commented

            if (string.IsNullOrEmpty(MySqlParam[6].Value.ToString()) == true)
            {
                isMenuRequestValid = 0;
            }
            else
            {
                isMenuRequestValid = Convert.ToInt16(MySqlParam[6].Value.ToString());  //#CC01 added
            }
            //#CC08 End
            return IntResultCount;
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    public string TableName
    {
        get;
        set;
    }

    public DataTable GetTablesNameForSchema()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@tablename", TableName);
            SqlParam[1] = new SqlParameter("@uploadtableid", UploadTableID);
            DataTable dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("getTableNameForSchema", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void UpdSchemaStatus()
    {
        try
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@uploadtableid", UploadTableID);
            SqlParam[1] = new SqlParameter("@status", Status);
            SqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
            SqlParam[2].Direction = ParameterDirection.Output;
            int r = DataAccess.DataAccess.Instance.DBInsertCommand("prcUpdStatusTableSchma", SqlParam);
            error = Convert.ToString(SqlParam[2].Value);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }



    public string error;
    public int UploadTableID
    {
        get;
        set;
    }

    public string TblColumnName
    {
        get;
        set;
    }

    public string TblColumndataType
    {
        get;
        set;
    }

    public string TblExcelColumnName
    {
        get;
        set;
    }

    public string TblExcelColumndataType
    {
        get;
        set;
    }

    public string TblColumnConstraints
    {
        get;
        set;
    }

    public int MaxLength
    {
        get;
        set;
    }

    public int PreviousIndex
    {
        get;
        set;
    }

    public int CurrentIndex
    {
        get;
        set;
    }


    public void UpdTahbleSchema()
    {
        try
        {
            SqlParam = new SqlParameter[10];
            SqlParam[0] = new SqlParameter("@uploadtableid", UploadTableID);
            SqlParam[1] = new SqlParameter("@tablecoumnname", TblColumnName);
            SqlParam[2] = new SqlParameter("@tablecolumndatatype", TblColumndataType);
            SqlParam[3] = new SqlParameter("@excelcolumnname", TblExcelColumnName);
            SqlParam[4] = new SqlParameter("@excelcolumndatatype", TblExcelColumndataType);
            SqlParam[5] = new SqlParameter("@columnconstraints", TblColumnConstraints);
            SqlParam[6] = new SqlParameter("@maxlength", MaxLength);
            SqlParam[7] = new SqlParameter("@previousindex", PreviousIndex);
            SqlParam[8] = new SqlParameter("@currentindex", CurrentIndex);
            //SqlParam[8] = new SqlParameter("@status",Status);
            SqlParam[9] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
            SqlParam[9].Direction = ParameterDirection.Output;
            int r = DataAccess.DataAccess.Instance.DBInsertCommand("prcUpdUplodSchema", SqlParam);
            error = Convert.ToString(SqlParam[9].Value);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public Int32 InsertUpdateUploadSchema()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@UploadSchemaXML", SqlDbType.Xml);
            SqlParam[0].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(UploadSchemaXml, XmlNodeType.Document, null));
            SqlParam[0].Direction = ParameterDirection.InputOutput;
            SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[1].Direction = ParameterDirection.Output;
            IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("prcInsUpdUploadTableSchema", SqlParam);
            if (((System.Data.SqlTypes.SqlXml)SqlParam[0].Value).IsNull != true)
            {
                UploadSchemaXml = ((System.Data.SqlTypes.SqlXml)SqlParam[0].Value).Value;
            }
            else
            {
                UploadSchemaXml = null;
            }
            if (SqlParam[1].Value != DBNull.Value && SqlParam[1].Value.ToString() != "")
            {
                Error = (SqlParam[1].Value).ToString();
            }

            return IntResultCount;
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
    public bool SendForgetPassword()
    {
        bool result = false;
        try
        {
            MySqlParam = new MySqlParameter[13];
            MySqlParam[0] = new MySqlParameter("@p_ErrorMessage", MySqlDbType.VarChar, 200);
            MySqlParam[0].Direction = ParameterDirection.Output;
            MySqlParam[1] = new MySqlParameter("@p_StrUserName", strDisplayName);
            MySqlParam[2] = new MySqlParameter("@p_StrLoginName", strUserLoginName);
            MySqlParam[3] = new MySqlParameter("@p_TxtPassword", StrPassword);
            MySqlParam[4] = new MySqlParameter("@p_EmailTO", strEmailID);
            MySqlParam[5] = new MySqlParameter("@p_EmailKyeWord", "Forget_Pass");
            MySqlParam[6] = new MySqlParameter("@p_EmailCC", null);
            MySqlParam[7] = new MySqlParameter("@p_EmailAttachmentNames",null);
            MySqlParam[8] = new MySqlParameter("@p_InvoiceNO", null);
            MySqlParam[9] = new MySqlParameter("@p_Date", null);
            MySqlParam[10] = new MySqlParameter("@p_EmailBody", null);
            MySqlParam[11] = new MySqlParameter("@p_debugmode", 0);
            MySqlParam[12] = new MySqlParameter("@p_CompanyId", CompanyID);
            DataAccess.DataAccess.Instance.DBInsert_MySqlCommand("prcSendMail", MySqlParam);
            Error = (MySqlParam[0].Value).ToString();
            if (Error == "")
            {
                result = true;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return result;
    }
    /*#CC05 Added Started*/
    public DataTable GetUserInfoForValidateFinanceRequestType()
    {
        DataTable dtResult = new DataTable();
        try
        {
            SqlParam = new SqlParameter[0];
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetuserValidateFinanceRequestType", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }
        catch (Exception ex)
        {
            return dtResult;
        }

    }
    /*#CC05 Added End*/
    /*#CC05 Added*/
    public DataTable prcGetValidateByUserId(int UserID)
    {
        try
        {
            SqlParam = new SqlParameter[1];

            SqlParam[0] = new SqlParameter("@UserID", UserID);
            dtUserInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetUserValidateFinanceRequestTypeMapping", CommandType.StoredProcedure, SqlParam);
            return dtUserInfo;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    /*#CC05 Added End*/
    /*#CC06 Added Started*/
    public Int32 InsertUpdateAPIUserRequestTypeMapping()
    {
        try
        {
            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@UserID", intUserID);
            SqlParam[1] = new SqlParameter("@APIUserId", intAPIUserId);

            SqlParam[2] = new SqlParameter("@RequestTypeId1", BitRequestTypeId1);
            SqlParam[3] = new SqlParameter("@RequestTypeId2", BitRequestTypeId2);

            SqlParam[4] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 1);
            SqlParam[4].Direction = ParameterDirection.Output;
            Int32 IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("prcSaveUpdateAPIUserRequestTypeMapping", SqlParam);
            return Convert.ToInt32(SqlParam[4].Value);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /*#CC06 Added Started End*/
    public DataSet prcNewDeviceUsers()
    {
        try
        {
            SqlParameter[] prm = new SqlParameter[9];
            prm[0] = new SqlParameter("@UserId", UserID);
            prm[1] = new SqlParameter("@EntityTypeId", EntitytypeId);
            prm[2] = new SqlParameter("@EntitytypeUserId", SalesChannelID);//Selected User Id
            
            prm[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[3].Direction = ParameterDirection.Output;
            prm[4] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[4].Direction = ParameterDirection.Output;
            prm[5] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            prm[5].Direction = ParameterDirection.Output;
            prm[6] = new SqlParameter("@PageSize", PageSize);
            prm[7] = new SqlParameter("@PageIndex", PageIndex);
            prm[8] = new SqlParameter("@CompanyId", CompanyID);
            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetChangeDeviceAppUser", CommandType.StoredProcedure, prm);
            ErrorMessage = Convert.ToString(prm[4].Value);
            TotalRecords = Convert.ToInt32(prm[5].Value);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public Int32 ApproveNewDevice()
    {
        try
        {
            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@UserID", UserID);
            SqlParam[1] = new SqlParameter("@UserIdsComaSeparated", UserIds);

            SqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 1);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar,500);
            SqlParam[3].Direction = ParameterDirection.Output;
            SqlParam[4] = new SqlParameter("@CompanyId", CompanyID);
            
            Int32 IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("prcChangeDeviceAppUser", SqlParam);
            Error = Convert.ToString(SqlParam[3].Value);
            return Convert.ToInt32(SqlParam[2].Value);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetBrandCategoryMaster()
    {
        try
        {
            SqlParam = new SqlParameter[6];
            SqlParam[0] = new SqlParameter("@CompanyID", CompanyID);
            dtUserInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetBrandCategoryDetails", CommandType.StoredProcedure, SqlParam);
            return dtUserInfo;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

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

    ~UserData()
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

