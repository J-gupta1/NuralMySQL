﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;

/*
 * 29-Dec-2014, Sumit Kumar, #CC01, Create properties and add new function to save notification
 * 29-Dec-2014, Karam Chand Sharma, #CC02, Create a new function for view notification
 * 30-Oct-2018, Sumit Maurya, #CC03, UserID provided in procedure (Done for motorola).
 * 20-March-2020,Vijay Kumar Prajapati,#CC04 Upload  all Master using single interface. 
 * 19-Dec-2020, Balram Jha, #CC05 Added ClientMasterDetail field in adding new company
 * 09-May- 2023, Hema Thapa, #CC06, MySql connections
 */

namespace DataAccess
{
    public class CommonMaster : IDisposable
    {
        #region Private Variables
        DataTable dtState, dtCity, dtSKU, dtUploadSchema, dtCommon;
        DataSet dsUploadschema;
        SqlParameter[] SqlParam;
        MySqlParameter[] MySqlParam;
        Int32 intResultCount, intUserMasterId;
        private string strSpName, docXML, strTableName, strAutoCompleteCondition, _OutError;
        private int intCompanyID, intStateID, intBrandID, intAutoCompleteParameter1, intCircleID, intModelID;
        private Int16 intStatus, intOfficeLevel;
        EnumData.eAutoCompleteType eAutoCompleteType;
        private string _Password;
        private string _PasswordSalt;
        string strAutoCompleteParameter2;

        #endregion
        /// <summary>
        /// This will use to check hirechy of user
        /// </summary>
        /// <param name="EntityID">
        /// Zone Master = ZoneID
        /// Circle Manager = CircleID
        /// AreaPosition Master = AreaPositionID
        /// </param>
        /// <param name="UserTypeID">
        /// Usertypeid 
        /// </param>
        /// <returns></returns>
        /// 
        private string strExcelFileNameInTable;

        #region Public Property

        #region User Activity tracking and Menu permission Check
        public Int32 UserID
        {
            get;
            set;
        }

        public string UserIP
        {
            get;
            set;
        }

        public string UserServerIP
        {
            get;
            set;
        }

        public string MenuDescription
        {
            get;
            set;
        }

        public string ErrorMessage
        {
            get;
            set;
        }

        public bool isMenuRequestValid
        {
            get;
            set;
        }
        #endregion

        public string ExcelFileNameInTable
        {
            get
            {
                return strExcelFileNameInTable;
            }
            set
            {
                strExcelFileNameInTable = value;
            }
        }
        public Int32 UserMasterId
        {
            get { return intUserMasterId; }
            set { intUserMasterId = value; }
        }
        public Int16 OfficeLevel
        {
            get { return intOfficeLevel; }
            set { intOfficeLevel = value; }
        }
        public Int32 CircleID
        {
            get { return intCircleID; }
            set { intCircleID = value; }
        }


        public Int16 Status
        {
            get
            {
                return intStatus;
            }
            set
            {
                intStatus = value;
            }
        }

        public EnumData.eSearchCondition FiscalType
        {
            get;
            set;
        }

        #region AutoComplete properties
        public string AutoCompleteCondition
        {
            get
            {
                return strAutoCompleteCondition;
            }
            set
            {
                strAutoCompleteCondition = value;
            }
        }

        public EnumData.eAutoCompleteType AutoCompleteType
        {
            get { return eAutoCompleteType; }
            set { eAutoCompleteType = value; }

        }

        public Int32 AutoCompleteParameter1
        {
            get { return intAutoCompleteParameter1; }
            set { intAutoCompleteParameter1 = value; }
        }
        public string AutoCompleteParameter2
        {
            get { return strAutoCompleteParameter2; }
            set { strAutoCompleteParameter2 = value; }
        }
        #endregion

        public string SPName
        {
            get
            {
                return strSpName;
            }
            set
            {
                strSpName = value;
            }
        }
        public int StateID
        {
            get
            {
                return intStateID;
            }
            set
            {
                intStateID = value;
            }
        }

        public int BrandID
        {
            get
            {
                return intBrandID;
            }
            set
            {
                intBrandID = value;
            }
        }

        public int ModelID
        {
            get
            {
                return intModelID;
            }
            set
            {
                intModelID = value;
            }
        }
        public int CompanyID
        {
            get
            {
                return intCompanyID;
            }
            set
            {
                intCompanyID = value;
            }
        }
        public Int16 TemplateType { get; set; }
        public Int16 ddlFill { get; set; }
        public string TemplateName { get; set; }
        public Int64 RoleId { get; set; }
        public Int32 CityGroupId { get; set; }
        public Int32 ProductCategoryId { get; set; }
        public Int32 TotalRecords { get; set; }
        public Int64 SKUID { get; set; }
        public Int32 RegionId { get; set; }
        public Int32 CityId { get; set; }
        public string DocXML
        {
            get { return docXML; }
            set { docXML = value; }
        }

        public string UploadTableName
        {
            get { return strTableName; }
            set { strTableName = value; }
        }
        //manage stock reason
        private string _ErrorDetailXML;
        public string ErrorDetailXML
        {
            get { return _ErrorDetailXML; }
            set { _ErrorDetailXML = value; }
        }
        private string _Error;
        public string Error
        {
            get { return _Error; }
            set { _Error = value; }
        }
        private string _InsError;
        public string InsError
        {
            get { return _InsError; }
            set { _InsError = value; }
        }
        private string _AdjReason;
        public string AdjReason
        {
            get { return _AdjReason; }
            set { _AdjReason = value; }
        }
        private int _CreatedBy;
        public int CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }
        private int _Active;
        public int Active
        {
            get { return _Active; }
            set { _Active = value; }
        }
        private int _AdjustmentReasonID;
        public int AdjustmentReasonID
        {
            get { return _AdjustmentReasonID; }
            set { _AdjustmentReasonID = value; }
        }
        //property for Manage Client
        private string _ClientName;
        public string ClientName
        {
            get { return _ClientName; }
            set { _ClientName = value; }
        }
        private string _ClientFolderName;
        public string ClientFolderName
        {
            get { return _ClientFolderName; }
            set { _ClientFolderName = value; }
        }
        private string _ApplicationTitle;
        public string ApplicationTitle
        {
            get { return _ApplicationTitle; }
            set { _ApplicationTitle = value; }
        }
        private string _SiteUrl;
        public string SiteUrl
        {
            get { return _SiteUrl; }
            set { _SiteUrl = value; }
        }
        private int _SUM;
        public int SUM
        {
            get { return _SUM; }
            set { _SUM = value; }
        }
        private int _ClientId;
        public int ClientId
        {
            get { return _ClientId; }
            set { _ClientId = value; }
        }
        private string _EmailSignature;
        public string EmailSignature
        {
            get { return _EmailSignature; }
            set { _EmailSignature = value; }
        }
        private string _CopyRightText;


        public string CopyRightText
        {
            get { return _CopyRightText; }
            set { _CopyRightText = value; }
        }
        DateTime? datefrom; DateTime? dateto; int NotificationId;
        public DateTime? DateFrom
        {
            get { return datefrom; }
            set { datefrom = value; }
        }
        public DateTime? DateTo
        {
            get { return dateto; }
            set { dateto = value; }
        }
        public int notificationId
        {
            get { return NotificationId; }
            set { NotificationId = value; }
        }
        /*#CC01 ADDED START*/
        public string NotificationText
        {
            get;
            set;
        }

        public int AccessType
        {
            get;
            set;
        }

        public bool NoteStatus
        {
            get;
            set;
        }

        public int NoteCreatedBy
        {
            get;
            set;
        }

        public string NotificationLevel
        {
            get;
            set;
        }

        public int CallingMode
        {
            get;
            set;
        }

        public int Out_Param
        {
            get;
            set;
        }

        public string XML_Error
        {
            get;
            set;
        }
        /*#CC01 ADDED END*/
        public string SessionId { get; set; }
        public Int64 EntityTypeId { get; set; }
        public Int64 EntityId { get; set; }
        public Int32 PageIndex { get; set; }
        public Int32 PageSize { get; set; }
        public string OutError
        {
            get
            {
                return _OutError;
            }
            set
            {
                _OutError = value;
            }
        }
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
        public string PasswordSalt
        {
            get
            {
                return _PasswordSalt;
            }
            set
            {
                _PasswordSalt = value;
            }
        }

        //#CC05 start

        public DataTable TVPClientSetting;
        //#CC05 end
        #endregion



        #region Data fetch Methods
        public DataTable GetAutoCompleteData()
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@TargetType", eAutoCompleteType);
                SqlParam[1] = new SqlParameter("@CompanyID", intCompanyID);
                SqlParam[2] = new SqlParameter("@ConditionVar", strAutoCompleteCondition);
                SqlParam[3] = new SqlParameter("@AdditionalParamter1", intAutoCompleteParameter1);
                SqlParam[4] = new SqlParameter("@AdditionalParameter2", strAutoCompleteParameter2);

                dtCommon = DataAccess.Instance.GetTableFromDatabase("prcGetAutoCompleteData", CommandType.StoredProcedure, SqlParam);

                return dtCommon;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region GetAllActiveState
        public DataTable GetAllActiveState()
        {
            try
            {
                dtState = DataAccess.Instance.GetTableFromDatabase("PrcAllGetActiveState", CommandType.StoredProcedure);
                return dtState;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        #endregion
        #region GetAllActiveCity
        public DataTable GetAllActiveCity()
        {
            try
            {
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@StateID", intStateID);
                dtCity = DataAccess.Instance.GetTableFromDatabase("PrcAllGetActiveCity", CommandType.StoredProcedure, SqlParam);
                return dtCity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        #region GetAllActiveClubs
        public DataTable GetAllActiveClubs()
        {
            try
            {
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@CompanyID", intCompanyID);

                dtSKU = DataAccess.Instance.GetTableFromDatabase("PrcGetAllActiveClubs", CommandType.StoredProcedure, SqlParam);
                return dtSKU;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region GetAllActiveSKU
        public DataTable GetAllActiveSKU()
        {
            try
            {
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@CompanyID", intCompanyID);
                SqlParam[1] = new SqlParameter("@BrandID", intBrandID);
                SqlParam[2] = new SqlParameter("@ModelId", intModelID);
                dtSKU = DataAccess.Instance.GetTableFromDatabase("PrcGetAllActiveSKU", CommandType.StoredProcedure, SqlParam);
                return dtSKU;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        #region GetAllActiveDistributor
        public DataTable GetAllActiveDistributor()
        {
            try
            {
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@CompanyID", intCompanyID);
                SqlParam[1] = new SqlParameter("@OfficeLevel", intOfficeLevel);
                SqlParam[2] = new SqlParameter("@UserMasterId", intUserMasterId);

                dtSKU = DataAccess.Instance.GetTableFromDatabase("PrcGetAllActiveDistributor", CommandType.StoredProcedure, SqlParam);
                return dtSKU;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region GetAllActiveBrand
        public DataTable GetAllActiveBrand()
        {
            try
            {
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@CompanyID", intCompanyID);

                dtSKU = DataAccess.Instance.GetTableFromDatabase("PrcGetAllActiveBrand", CommandType.StoredProcedure, SqlParam);
                return dtSKU;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region GetAllActiveModel
        public DataTable GetAllActiveModel()
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@CompanyID", intCompanyID);
                SqlParam[1] = new SqlParameter("@BrandID", intBrandID);

                dtSKU = DataAccess.Instance.GetTableFromDatabase("PrcGetAllActiveModel", CommandType.StoredProcedure, SqlParam);
                return dtSKU;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetAllBrandByStatus

        public DataTable GetAllActiveBrand(Int16 intCompanyId, Int16 intStatus)
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@CompanyID", intCompanyID);
                SqlParam[1] = new SqlParameter("@Status", intStatus);

                dtSKU = DataAccess.Instance.GetTableFromDatabase("PrcGetAllActiveBrand", CommandType.StoredProcedure, SqlParam);
                return dtSKU;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public DataTable getAllActiveTIC()
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@CompanyID", intCompanyID);
                SqlParam[1] = new SqlParameter("@CircleID", intCircleID);
                dtCommon = DataAccess.Instance.GetTableFromDatabase("prcGetAreaPositionInfo", CommandType.StoredProcedure, SqlParam);
                return dtCommon;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public DataTable getAllActiveZSM()
        {
            try
            {
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@CompanyID", intCompanyID);
                dtCommon = DataAccess.Instance.GetTableFromDatabase("prcGetAllActiveZSM", CommandType.StoredProcedure, SqlParam);
                return dtCommon;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public DataTable getAllActiveASM(int RegionID)
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@CompanyID", intCompanyID);
                SqlParam[1] = new SqlParameter("@RegionID", RegionID);
                dtCommon = DataAccess.Instance.GetTableFromDatabase("prcGetAllActiveASM", CommandType.StoredProcedure, SqlParam);
                return dtCommon;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public DataTable getAllFiscalQuarter()
        {
            try
            {
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@Type", (int)FiscalType);
                dtCommon = DataAccess.Instance.GetTableFromDatabase("prcGetFiscalQuarterInfo", CommandType.StoredProcedure, SqlParam);
                return dtCommon;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        #region Get Schema By Matched  Excel File Template
        public DataSet InsertThroughUpload(ref int Result)
        {
            try
            {
                SqlParam = new SqlParameter[5]; /* #CC03 length increased from 4-5 */
                SqlParam[0] = new SqlParameter("@doc", docXML);
                SqlParam[1] = new SqlParameter("@CompanyID", CompanyID);
                SqlParam[2] = new SqlParameter("@OutResult", 0);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@PasswordExpiryDays", PasswordExpiryDays);
                SqlParam[4] = new SqlParameter("@UserID", UserID); /* #CC03 Added */
                dsUploadschema = DataAccess.Instance.GetDataSetFromDatabase(strSpName, CommandType.StoredProcedure, SqlParam);
                Result = Convert.ToInt32(SqlParam[2].Value);
                return dsUploadschema;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Get Schema By Matched  Excel File Template
        public DataSet GetMatchedSchemaByExcelFileTemplate()
        {
            try
            {
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@ExcelFileNameInTable", strExcelFileNameInTable);

                dsUploadschema = DataAccess.Instance.GetDataSetFromDatabase("PrcGetMatchedSchemaByExcelFileTemplate", CommandType.StoredProcedure, SqlParam);
                return dsUploadschema;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        public DataTable getSchemaForUpload()
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@UploadTableName", strTableName);

            dtUploadSchema = DataAccess.Instance.GetTableFromDatabase("prcGetSchemaForUpload", CommandType.StoredProcedure, SqlParam);
            CreateSchema(ref dtUploadSchema);
            return dtUploadSchema;
        }

        private void CreateSchema(ref DataTable dt)
        {
            if (dt.Rows.Count == 0)
            {
                throw new NullReferenceException("Scheme not defined for table " + strTableName);
            }

            DataTable dtNewSchema = new DataTable(dt.Rows[0]["tableName"].ToString());
            DataColumn dtCol;
            ArrayList PrimaryCol = new ArrayList();
            Int16 iPrimaryCount = 0;
            foreach (DataRow drow in dt.Rows)
            {
                dtCol = new DataColumn(drow["ExcelSheetColumnName"].ToString(), System.Type.GetType(drow["ExcelSheetDataType"].ToString()));
                dtCol.AllowDBNull = Convert.ToBoolean(drow["Validate"].ToString());

                if (Convert.ToInt32(drow["MinLength"]) != 0)
                {
                    dtCol.ExtendedProperties.Add("MinLength", drow["MinLength"].ToString());

                    if (Convert.ToInt32(drow["MaxLength"].ToString()) > 0)
                    {
                        dtCol.ExtendedProperties.Add("MaxLength", Convert.ToInt32(drow["MaxLength"].ToString()));
                    }

                    dtCol.ExtendedProperties.Add("ColumnConstraint", drow["ColumnConstraint"].ToString());

                }



                if (drow["ColumnConstraint"].ToString().ToLower() == "primary")
                {
                    PrimaryCol.Add(dtCol.ColumnName);
                    dtCol.ExtendedProperties.Add("TableColumnName", drow["TableColumnName"].ToString());
                    dtCol.ExtendedProperties.Add("TableColumnDataType", drow["TableColumnDataType"].ToString());
                    iPrimaryCount++;
                }
                //else
                //{

                //}
                dtNewSchema.Columns.Add(dtCol);

            }
            //dtNewSchema.Columns.Add(dtCol);
            DataColumn[] dtc = new DataColumn[PrimaryCol.Count];
            for (int i = 0; i < PrimaryCol.Count; i++)
            {
                if (PrimaryCol[i] != null)
                {
                    dtc[i] = dtNewSchema.Columns[PrimaryCol[i].ToString()];
                }
            }
            dtNewSchema.Constraints.Add("PkKey", dtc, true);
            PrimaryCol = null;

            dt = null;
            dt = dtNewSchema;
        }
        // code for StockAdjReson
        public void InsertStockAdjReason()
        {
            SqlParam = new SqlParameter[7];
            SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@InsError", SqlDbType.NVarChar, 200);
            SqlParam[3].Direction = ParameterDirection.Output;
            SqlParam[4] = new SqlParameter("@AdjReason", AdjReason);
            SqlParam[5] = new SqlParameter("@CreatedBy", CreatedBy);
            SqlParam[6] = new SqlParameter("@Status", Active);
            DataAccess.Instance.DBInsertCommand("prcInsStockAdjustmentReasonMaster", SqlParam);
            if (SqlParam[2].Value.ToString() != "")
            {
                ErrorDetailXML = SqlParam[2].Value.ToString();
            }
            else { ErrorDetailXML = null; }
            if (SqlParam[3].Value.ToString() != "")
            {
                InsError = SqlParam[3].Value.ToString();
            }
            else { InsError = null; }
            Error = Convert.ToString(SqlParam[1].Value);
        }
        public DataTable GetStockAdjReason(int condition)
        {
            MySqlParam = new MySqlParameter[6];
            MySqlParam[0] = new MySqlParameter("@p_Out_Param", MySqlDbType.Int16, 2);
            MySqlParam[0].Direction = ParameterDirection.Output;
            MySqlParam[1] = new MySqlParameter("@p_ErrorMessage", MySqlDbType.VarChar, 200);
            MySqlParam[1].Direction = ParameterDirection.Output;
            MySqlParam[2] = new MySqlParameter("@p_ErrorXML", MySqlDbType.Text, 2);
            MySqlParam[2].Direction = ParameterDirection.Output;
            MySqlParam[3] = new MySqlParameter("@p_AdjustmentReasonID", AdjustmentReasonID);
            MySqlParam[4] = new MySqlParameter("@p_condition", condition);
            MySqlParam[5] = new MySqlParameter("@p_CompanyId", CompanyID);
            dtCommon = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetStockAdjustmentReasonMaster", CommandType.StoredProcedure, MySqlParam);
            if (MySqlParam[2].Value.ToString() != "")
            {
                ErrorDetailXML = MySqlParam[2].Value.ToString();
            }
            else { ErrorDetailXML = null; }
            Error = Convert.ToString(MySqlParam[1].Value);
            return dtCommon;
        }
        public void UpdateStockAdjReason(int condition)
        {
            SqlParam = new SqlParameter[8];
            SqlParam[0] = new SqlParameter("@AdjReason", AdjReason);
            SqlParam[1] = new SqlParameter("@Status", Active);
            SqlParam[2] = new SqlParameter("@AdjReasonID", AdjustmentReasonID);
            SqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[3].Direction = ParameterDirection.Output;
            SqlParam[4] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[4].Direction = ParameterDirection.Output;
            SqlParam[5] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
            SqlParam[5].Direction = ParameterDirection.Output;
            SqlParam[6] = new SqlParameter("@InsError", SqlDbType.NVarChar, 200);
            SqlParam[6].Direction = ParameterDirection.Output;
            SqlParam[7] = new SqlParameter("@condition", condition);
            DataAccess.Instance.DBInsertCommand("prcUpdateStockAdjustmentReasonMaster", SqlParam);
            if (SqlParam[5].Value.ToString() != "")
            {
                ErrorDetailXML = SqlParam[5].Value.ToString();
            }
            else { ErrorDetailXML = null; }
            if (SqlParam[6].Value.ToString() != "")
            {
                InsError = SqlParam[6].Value.ToString();
            }
            else { InsError = null; }
            Error = Convert.ToString(SqlParam[4].Value);
        }
        // code start for manage client

        public void InsertManageClient()
        {
            SqlParam = new SqlParameter[14];//#CC05
            SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@ClientName", ClientName);
            SqlParam[3] = new SqlParameter("@ClientFolderName", ClientFolderName);
            SqlParam[4] = new SqlParameter("@ApplicationTitle", ApplicationTitle);
            SqlParam[5] = new SqlParameter("@SiteUrl", SiteUrl);
            SqlParam[6] = new SqlParameter("@SUM", SUM);
            SqlParam[7] = new SqlParameter("@Status", Active);
            SqlParam[8] = new SqlParameter("@Insinfo", SqlDbType.VarChar, 50);
            SqlParam[8].Direction = ParameterDirection.Output;
            SqlParam[9] = new SqlParameter("@EmailSignature", EmailSignature);
            SqlParam[10] = new SqlParameter("@CopyRightText", CopyRightText);
            //#CC05 start
            SqlParam[11] = new SqlParameter("@TVPClientSetting", TVPClientSetting);
            
            SqlParam[12] = new SqlParameter("@UserId", UserID);//#CC05 end


            DataAccess.Instance.DBInsertCommand("prcInsManageClient", SqlParam);
            if (SqlParam[8].Value.ToString() != "")
            {
                InsError = SqlParam[8].Value.ToString();
            }
            else { InsError = null; }
            Error = Convert.ToString(SqlParam[1].Value);
        }
        public DataSet GetManageClient(int condition)//#CC05
        {
            DataSet dsClient = new DataSet();
            SqlParam = new SqlParameter[4];
            SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@ClientID", ClientId);
            SqlParam[3] = new SqlParameter("@Condition", condition);
            //dtCommon = DataAccess.Instance.GetTableFromDatabase("prcGetManageClient", CommandType.StoredProcedure, SqlParam);
            dsClient = DataAccess.Instance.GetDataSetFromDatabase("prcGetManageClient", CommandType.StoredProcedure, SqlParam);
            Error = Convert.ToString(SqlParam[1].Value);
            return dsClient;
        }



        public void UpdateManageClient(int condition)
        {
            SqlParam = new SqlParameter[15];
            SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@ClientName", ClientName);
            SqlParam[3] = new SqlParameter("@ClientFolderName", ClientFolderName);
            SqlParam[4] = new SqlParameter("@ApplicationTitle", ApplicationTitle);
            SqlParam[5] = new SqlParameter("@SiteUrl", SiteUrl);
            SqlParam[6] = new SqlParameter("@SUM", SUM);
            SqlParam[7] = new SqlParameter("@Status", Active);
            SqlParam[8] = new SqlParameter("@Insinfo", SqlDbType.VarChar, 50);
            SqlParam[8].Direction = ParameterDirection.Output;
            SqlParam[9] = new SqlParameter("@ClientId", ClientId);
            SqlParam[10] = new SqlParameter("@Condition", condition);
            SqlParam[11] = new SqlParameter("@EmailSignature", EmailSignature);
            SqlParam[12] = new SqlParameter("@CopyRightText", CopyRightText);
            //#CC05 start
            SqlParam[13] = new SqlParameter("@TVPClientSetting", TVPClientSetting);

            SqlParam[14] = new SqlParameter("@UserId", UserID);//#CC05 end
            DataAccess.Instance.DBInsertCommand("prcUpdManageClient", SqlParam);
            if (SqlParam[8].Value.ToString() != "")
            {
                InsError = SqlParam[8].Value.ToString();
            }
            else { InsError = null; }
            Error = Convert.ToString(SqlParam[1].Value);
        }
        #endregion

        #region Check and Toggle Methods
        public Int32 CheckMasterRecordExistence(Int32 EntityID, Byte UserTypeID)
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@EntityID", EntityID);
                SqlParam[1] = new SqlParameter("@UserTypeID", UserTypeID);
                intResultCount = Convert.ToInt32(DataAccess.Instance.getSingleValues("PrcGetEntityExistence", SqlParam));
                return intResultCount;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public int ToggleOtherStatus(int ID, bool status, int IDTYPE)
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@ID", ID);
            SqlParam[1] = new SqlParameter("@status", status);
            SqlParam[2] = new SqlParameter("@IDTYPE", IDTYPE);
            intResultCount = DataAccess.Instance.DBInsertCommand("prcToggleOthersStatus", SqlParam);
            return intResultCount;
        }
        #endregion


        #region Update status Active or inActive

        /// <summary>
        /// This Function used to update status of any record according to their type and master detail
        /// in short used for active or inactive user
        /// </summary>
        /// <param name="MasterType"></param>
        /// <param name="MasterId"></param>
        /// <param name="Status"></param>
        /// <returns></returns>

        public Int32 UpdateMastersRecordStatusByType(Int16 MasterType, Int32 MasterId, Boolean @Status)
        {
            try
            {
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@MasterType", MasterType);
                SqlParam[1] = new SqlParameter("@MasterId", MasterId);
                SqlParam[2] = new SqlParameter("@Status", @Status);
                intResultCount = DataAccess.Instance.DBInsertCommand("PrcActiveDeactivMastersByType", SqlParam);
                return intResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion
        #region Delete Master Records

        /// <summary>
        /// This Function used to Delete master record oaccording to their type and master detail
        /// in short used for delete master records
        /// </summary>
        /// <param name="MasterType"></param>
        /// <param name="MasterId"></param>

        /// <returns></returns>

        public Int32 DeleteMasterRecordsByType(Int16 MasterType, Int32 MasterId)
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@MasterType", MasterType);
                SqlParam[1] = new SqlParameter("@MasterId", MasterId);

                intResultCount = DataAccess.Instance.DBInsertCommand("PrcDeleteMasterRecordsByType", SqlParam);
                return intResultCount;
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
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@UserID", UserID);
                SqlParam[1] = new SqlParameter("@UserIP", UserIP);
                SqlParam[2] = new SqlParameter("@UserServerID", UserServerIP);
                SqlParam[3] = new SqlParameter("@MenuContaningWord", MenuDescription);
                SqlParam[4] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[4].Direction = ParameterDirection.Output;
                SqlParam[5] = new SqlParameter("@isMenuPermision", SqlDbType.Bit);
                SqlParam[5].Direction = ParameterDirection.Output;
                intResultCount = DataAccess.Instance.DBInsertCommand("prcInsUpdUserTrackingActivity", SqlParam);
                ErrorMessage = SqlParam[4].Value.ToString();
                isMenuRequestValid = Convert.ToBoolean(SqlParam[5].Value.ToString());

                return intResultCount;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        #region Methods for Target File Validation

        public DataTable getSchemaForTargetUpload(Int16 TargetCategory)
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@TargetCategory", TargetCategory);
            SqlParam[1] = new SqlParameter("@BrandID", intBrandID);
            dtUploadSchema = DataAccess.Instance.GetTableFromDatabase("prcGetSchemaForTargetUpload", CommandType.StoredProcedure, SqlParam);
            return dtUploadSchema;
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

        ~CommonMaster()
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


        public int PasswordExpiryDays
        {
            get;
            set;
        }


        /*#CC01 ADDED START*/

        public DataSet GetISPForNotification()
        {
            dsUploadschema = DataAccess.Instance.GetDataSetFromDatabase("prcBindISP", CommandType.StoredProcedure, SqlParam);
            return dsUploadschema;
        }

        public int SaveNotification()
        {
            try
            {
                SqlParam = new SqlParameter[9];
                SqlParam[0] = new SqlParameter("@NotificationText", NotificationText);
                SqlParam[1] = new SqlParameter("@AccessType", AccessType);
                SqlParam[2] = new SqlParameter("@Status", NoteStatus);
                SqlParam[3] = new SqlParameter("@NotificationLevel", NotificationLevel);
                SqlParam[4] = new SqlParameter("@CallingMode", CallingMode);
                SqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.Int);
                SqlParam[5].Direction = ParameterDirection.Output;
                SqlParam[6] = new SqlParameter("@XML_Error", SqlDbType.Xml, 2);
                SqlParam[6].Direction = ParameterDirection.Output;
                SqlParam[7] = new SqlParameter("@CreatedBy", NoteCreatedBy);
                intResultCount = DataAccess.Instance.DBInsertCommand("prcSaveNotification", SqlParam);
                Out_Param = Convert.ToInt32(SqlParam[5].Value.ToString());
                XML_Error = SqlParam[6].Value.ToString();

                return intResultCount;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /*#CC01 ADDED END*/
        /*#CC02 ADDED START*/
        public DataTable ViewNotification()
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@FromDate", datefrom);
                SqlParam[1] = new SqlParameter("@ToDate", dateto);
                SqlParam[2] = new SqlParameter("@CallingMode", CallingMode);
                SqlParam[3] = new SqlParameter("@NotificationId", notificationId);
                dtCommon = DataAccess.Instance.GetTableFromDatabase("prcViewNotification", CommandType.StoredProcedure, SqlParam);
                return dtCommon;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*#CC02 ADDED END*/
        /*#CC04 Added Started*/
        public DataSet GetMaterialMasterTemplate()
        {
            DataSet dsResult = new DataSet();
            try
            {
                string TemplateName = "";
                MySqlParameter[] MySqlParam = new MySqlParameter[7];
                MySqlParam[0] = new MySqlParameter("@p_Userid", UserID);
                MySqlParam[1] = new MySqlParameter("@p_Out_Param", MySqlDbType.Int16, 2);
                MySqlParam[1].Direction = ParameterDirection.Output;
                MySqlParam[2] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 500);
                MySqlParam[2].Direction = ParameterDirection.Output;
                MySqlParam[3] = new MySqlParameter("@p_CompanyID", CompanyID);
                MySqlParam[4] = new MySqlParameter("@p_TemplateType", TemplateType);
                MySqlParam[5] = new MySqlParameter("@p_TemplateName", TemplateName);
                MySqlParam[6] = new MySqlParameter("@p_debugmode", 0);
                dsResult = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("PrcGetMaterialMasterTemplate", CommandType.StoredProcedure, MySqlParam);
                OutError = Convert.ToString(MySqlParam[2].Value);
                return dsResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetMaterialMasterReferenceData()
        {
            DataSet dsResult = new DataSet();
            try
            {

                
                /* #CC06 add start */
                MySqlParameter[] MySqlParam = new MySqlParameter[7];
                MySqlParam[0] = new MySqlParameter("@p_Userid", UserID);
                MySqlParam[1] = new MySqlParameter("@p_Out_Param", MySqlDbType.Int16, 2);
                MySqlParam[1].Direction = ParameterDirection.Output;
                MySqlParam[2] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 500);
                MySqlParam[2].Direction = ParameterDirection.Output;
                MySqlParam[3] = new MySqlParameter("@p_CompanyID", CompanyID);
                
                MySqlParam[4] = new MySqlParameter("@p_TemplateType", TemplateType);
                MySqlParam[5] = new MySqlParameter("@p_Dropdownfill", ddlFill);
                MySqlParam[6] = new MySqlParameter("@p_debugmode", 0);
                dsResult = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("PrcGetMaterialReferenceData", CommandType.StoredProcedure, MySqlParam);
                /* #CC06 add end*/

                /* #CC06 comment start */
                //SqlParameter[] SqlParam = new SqlParameter[6];
                //SqlParam[0] = new SqlParameter("@Userid", UserID);
                //SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                //SqlParam[1].Direction = ParameterDirection.Output;
                //SqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
                //SqlParam[2].Direction = ParameterDirection.Output;
                //SqlParam[3] = new SqlParameter("@CompanyID", CompanyID);
                //SqlParam[4] = new SqlParameter("@TemplateType", TemplateType);
                //SqlParam[5] = new SqlParameter("@Dropdownfill", ddlFill);
                //dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetMaterialReferenceData", CommandType.StoredProcedure, SqlParam);
                /* #CC06 comment end*/

                OutError = Convert.ToString(MySqlParam[2].Value);
                return dsResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet UploadMaterialMasterForSaveUpdate()
        {
            DataSet dsResult = new DataSet();

            MySqlParameter[] objSqlParam = new MySqlParameter[8];
            objSqlParam[0] = new MySqlParameter("@p_UserId", UserID);
            objSqlParam[1] = new MySqlParameter("@p_SessionID", SessionId);
            objSqlParam[2] = new MySqlParameter("@p_OutParam", MySqlDbType.Int16, 2);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new MySqlParameter("@p_OutError", MySqlDbType.VarChar, 500);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new MySqlParameter("@p_CompanyID", CompanyID);
            objSqlParam[5] = new MySqlParameter("@p_TemplateType", TemplateType);
            objSqlParam[6] = new MySqlParameter("@p_CreatedBy", 0);
            objSqlParam[7] = new MySqlParameter("@p_Debugmode", 0);
            dsResult = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("PrcUploadMaterialMasterData", CommandType.StoredProcedure, objSqlParam);
            Out_Param = Convert.ToInt16(objSqlParam[2].Value);
            OutError = Convert.ToString(objSqlParam[3].Value);

            return dsResult;
        }



        public DataSet GetLocalityTemplate()
        {
            DataSet dsResult = new DataSet();
            try
            {
                /* #CC06 add start */
                MySqlParameter[] MySqlParam = new MySqlParameter[6];
                MySqlParam[0] = new MySqlParameter("@p_Userid", UserID);
                MySqlParam[1] = new MySqlParameter("@p_Out_Param", MySqlDbType.Int16, 2);
                MySqlParam[1].Direction = ParameterDirection.Output;
                MySqlParam[2] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 500);
                MySqlParam[2].Direction = ParameterDirection.Output;
                MySqlParam[3] = new MySqlParameter("@p_CompanyID", CompanyID);
                MySqlParam[4] = new MySqlParameter("@p_TemplateType", TemplateType);
                MySqlParam[5] = new MySqlParameter("@p_debugmode", 0);
                dsResult = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("PrcGetLocalityMasterTemplate", CommandType.StoredProcedure, MySqlParam);
                /* #CC06 add end*/

                /* #CC06 comment start */
                //SqlParameter[] SqlParam = new SqlParameter[6];
                //SqlParam[0] = new SqlParameter("@Userid", UserID);
                //SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                //SqlParam[1].Direction = ParameterDirection.Output;
                //SqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
                //SqlParam[2].Direction = ParameterDirection.Output;
                //SqlParam[3] = new SqlParameter("@CompanyID", CompanyID);
                //SqlParam[4] = new SqlParameter("@TemplateType", TemplateType);
                //dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetLocalityMasterTemplate", CommandType.StoredProcedure, SqlParam);
                /* #CC06 comment end*/

                OutError = Convert.ToString(MySqlParam[2].Value);
                return dsResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet UploadLocalityMaster()
        {
            DataSet dsResult = new DataSet();

            MySqlParameter[] objSqlParam = new MySqlParameter[8];
            objSqlParam[0] = new MySqlParameter("@p_UserId", UserID);
            objSqlParam[1] = new MySqlParameter("@p_SessionID", SessionId);
            objSqlParam[2] = new MySqlParameter("@p_OutParam", MySqlDbType.Int16, 2);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new MySqlParameter("@p_OutError", MySqlDbType.VarChar, 500);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new MySqlParameter("@p_CompanyID", CompanyID);
            objSqlParam[5] = new MySqlParameter("@p_TemplateType", TemplateType);
            objSqlParam[6] = new MySqlParameter("@p_CreatedBy", 0);
            objSqlParam[7] = new MySqlParameter("@p_Debugmode", 0);
            dsResult = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("PrcUploadLocalityMaster", CommandType.StoredProcedure, objSqlParam);
            Out_Param = Convert.ToInt16(objSqlParam[2].Value);
            OutError = Convert.ToString(objSqlParam[3].Value);

            return dsResult;
        }

        public DataSet UploadOrgHierarchyWithUserSave()
        {
            DataSet dsResult = new DataSet();

            SqlParameter[] objSqlParam = new SqlParameter[7];
            objSqlParam[0] = new SqlParameter("@UserId", UserID);
            objSqlParam[1] = new SqlParameter("@SessionID", SessionId);
            objSqlParam[2] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@OutError", SqlDbType.VarChar, 500);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@CompanyID", CompanyID);
            objSqlParam[5] = new SqlParameter("@Password", Password);
            objSqlParam[6] = new SqlParameter("@PasswordSalt", PasswordSalt);
            dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcUploadOrgnHierarchyWithUserSave", CommandType.StoredProcedure, objSqlParam);
            Out_Param = Convert.ToInt16(objSqlParam[2].Value);
            OutError = Convert.ToString(objSqlParam[3].Value);

            return dsResult;
        }
        public DataSet UploadOrgHierarchyWithUserUpdate()
        {
            DataSet dsResult = new DataSet();

            SqlParameter[] objSqlParam = new SqlParameter[7];
            objSqlParam[0] = new SqlParameter("@UserId", UserID);
            objSqlParam[1] = new SqlParameter("@SessionID", SessionId);
            objSqlParam[2] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@OutError", SqlDbType.VarChar, 500);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@CompanyID", CompanyID);
            objSqlParam[5] = new SqlParameter("@Password", Password);
            objSqlParam[6] = new SqlParameter("@PasswordSalt", PasswordSalt);
            dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcUploadOrgnHierarchyWithUserUpdate", CommandType.StoredProcedure, objSqlParam);
            Out_Param = Convert.ToInt16(objSqlParam[2].Value);
            OutError = Convert.ToString(objSqlParam[3].Value);

            return dsResult;
        }

        public DataSet UploadSalesChannelWithUserSave()
        {
            DataSet dsResult = new DataSet();

            SqlParameter[] objSqlParam = new SqlParameter[8];
            objSqlParam[0] = new SqlParameter("@UserId", UserID);
            objSqlParam[1] = new SqlParameter("@SessionID", SessionId);
            objSqlParam[2] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@OutError", SqlDbType.VarChar, 500);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@CompanyID", CompanyID);
            objSqlParam[5] = new SqlParameter("@Password", Password);
            objSqlParam[6] = new SqlParameter("@PasswordSalt", PasswordSalt);
            objSqlParam[7] = new SqlParameter("@TemplateType", TemplateType);
            dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcUploadSalesChannelWithUserSave", CommandType.StoredProcedure, objSqlParam);
            Out_Param = Convert.ToInt16(objSqlParam[2].Value);
            OutError = Convert.ToString(objSqlParam[3].Value);

            return dsResult;
        }

        public DataSet UploadSalesChannelWithUserUpdate()
        {
            DataSet dsResult = new DataSet();

            SqlParameter[] objSqlParam = new SqlParameter[8];
            objSqlParam[0] = new SqlParameter("@UserId", UserID);
            objSqlParam[1] = new SqlParameter("@SessionID", SessionId);
            objSqlParam[2] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@OutError", SqlDbType.VarChar, 500);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@CompanyID", CompanyID);
            objSqlParam[5] = new SqlParameter("@Password", Password);
            objSqlParam[6] = new SqlParameter("@PasswordSalt", PasswordSalt);
            objSqlParam[7] = new SqlParameter("@TemplateType", TemplateType);
            dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcUploadSalesChannelWithUserUpdated", CommandType.StoredProcedure, objSqlParam);
            Out_Param = Convert.ToInt16(objSqlParam[2].Value);
            OutError = Convert.ToString(objSqlParam[3].Value);

            return dsResult;
        }

        public DataSet GetISPUploadTemplate()
        {
            DataSet dsResult = new DataSet();
            try
            {
                SqlParameter[] SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@Userid", UserID);
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@CompanyID", CompanyID);
                SqlParam[4] = new SqlParameter("@TemplateType", TemplateType);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetISPTemplate", CommandType.StoredProcedure, SqlParam);
                OutError = Convert.ToString(SqlParam[2].Value);
                return dsResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet UploadCityTravelData()
        {
            DataSet dsResult = new DataSet();

            SqlParameter[] objSqlParam = new SqlParameter[6];
            objSqlParam[0] = new SqlParameter("@UserId", UserID);
            objSqlParam[1] = new SqlParameter("@SessionID", SessionId);
            objSqlParam[2] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@OutError", SqlDbType.VarChar, 500);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@CompanyID", CompanyID);
            objSqlParam[5] = new SqlParameter("@TemplateType", TemplateType);
            dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcUploadCityTravelRate", CommandType.StoredProcedure, objSqlParam);
            Out_Param = Convert.ToInt16(objSqlParam[2].Value);
            OutError = Convert.ToString(objSqlParam[3].Value);

            return dsResult;
        }
        public DataTable SelectAllCityTravelRate()
        {
            DataTable d1 = new DataTable();
            try
            {

                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@RoleId", RoleId);
                SqlParam[1] = new SqlParameter("@CityGroupId", CityGroupId);
                SqlParam[2] = new SqlParameter("@CompanyID", CompanyID);
                SqlParam[3] = new SqlParameter("@DateFrom", DateFrom);
                SqlParam[4] = new SqlParameter("@DateTo", DateTo);
                SqlParam[5] = new SqlParameter("@TemplateType", TemplateType);
                d1 = DataAccess.Instance.GetTableFromDatabase("prcGetAllCityTravelRateDetails", CommandType.StoredProcedure, SqlParam);
                return d1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet UploadEntityBrandMappingForSaveUpdate()
        {
            DataSet dsResult = new DataSet();

            SqlParameter[] objSqlParam = new SqlParameter[6];
            objSqlParam[0] = new SqlParameter("@UserId", UserID);
            objSqlParam[1] = new SqlParameter("@SessionID", SessionId);
            objSqlParam[2] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@OutError", SqlDbType.VarChar, 500);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@CompanyID", CompanyID);
            objSqlParam[5] = new SqlParameter("@TemplateType", TemplateType);
            dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcUploadEntityWiseBrandMapping", CommandType.StoredProcedure, objSqlParam);
            Out_Param = Convert.ToInt16(objSqlParam[2].Value);
            OutError = Convert.ToString(objSqlParam[3].Value);

            return dsResult;
        }
        public DataTable BindAllDropdownForEntityBrandMapping()
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@CompanyID", CompanyID);
                SqlParam[1] = new SqlParameter("@UserID", UserID);
                SqlParam[2] = new SqlParameter("@CallingMode", CallingMode);
                SqlParam[3] = new SqlParameter("@EntityTypeId", EntityTypeId);
                SqlParam[4] = new SqlParameter("@StateID", StateID);
                dtCommon = DataAccess.Instance.GetTableFromDatabase("prcGetallMasterForEntityBrandMapping", CommandType.StoredProcedure, SqlParam);
                return dtCommon;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetReportBrandEntityMappingData()
        {
            try
            {
                SqlParameter[] prm = new SqlParameter[12];
                prm[0] = new SqlParameter("@UserId", UserID);
                prm[1] = new SqlParameter("@EntityTypeId", EntityTypeId);
                prm[2] = new SqlParameter("@EntityId", EntityId);
                prm[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
                prm[3].Direction = ParameterDirection.Output;
                prm[4] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
                prm[4].Direction = ParameterDirection.Output;
                prm[5] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
                prm[5].Direction = ParameterDirection.Output;
                prm[6] = new SqlParameter("@PageSize", PageSize);
                prm[7] = new SqlParameter("@PageIndex", PageIndex);
                prm[8] = new SqlParameter("@CompanyId", CompanyID);
                prm[9] = new SqlParameter("@BrandID", BrandID);
                prm[10] = new SqlParameter("@ProductCategoryId", ProductCategoryId);
                prm[11] = new SqlParameter("@Status", Status);
                DataSet dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetEntityBrandMappingReport", CommandType.StoredProcedure, prm);
                ErrorMessage = Convert.ToString(prm[4].Value);
                TotalRecords = Convert.ToInt32(prm[5].Value);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*#CC04 Added Started*/
        public DataSet UploadSKUWisechemeForSaveUpdate()
        {
            DataSet dsResult = new DataSet();

            SqlParameter[] objSqlParam = new SqlParameter[6];
            objSqlParam[0] = new SqlParameter("@UserId", UserID);
            objSqlParam[1] = new SqlParameter("@SessionID", SessionId);
            objSqlParam[2] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@OutError", SqlDbType.VarChar, 500);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@CompanyID", CompanyID);
            objSqlParam[5] = new SqlParameter("@TemplateType", TemplateType);
            dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcUploadSKUWiseScheme", CommandType.StoredProcedure, objSqlParam);
            Out_Param = Convert.ToInt16(objSqlParam[2].Value);
            OutError = Convert.ToString(objSqlParam[3].Value);

            return dsResult;
        }
        public DataSet GetReportSKUSchemeData()
        {
            try
            {
                SqlParameter[] prm = new SqlParameter[15];
                prm[0] = new SqlParameter("@UserId", UserID);
                prm[1] = new SqlParameter("@SKUID", SKUID);
                prm[2] = new SqlParameter("@RegionId", RegionId);
                prm[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
                prm[3].Direction = ParameterDirection.Output;
                prm[4] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
                prm[4].Direction = ParameterDirection.Output;
                prm[5] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
                prm[5].Direction = ParameterDirection.Output;
                prm[6] = new SqlParameter("@PageSize", PageSize);
                prm[7] = new SqlParameter("@PageIndex", PageIndex);
                prm[8] = new SqlParameter("@CompanyId", CompanyID);
                prm[9] = new SqlParameter("@StateID", StateID);
                prm[10] = new SqlParameter("@CityId", CityId);
                prm[11] = new SqlParameter("@Status", Status);
                prm[12] = new SqlParameter("@DateFrom", DateFrom);
                prm[13] = new SqlParameter("@DateTo", DateTo);
                DataSet dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetSKUSchemeReport", CommandType.StoredProcedure, prm);
                ErrorMessage = Convert.ToString(prm[4].Value);
                TotalRecords = Convert.ToInt32(prm[5].Value);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
    }
}

