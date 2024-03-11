/*
 
 * 04-Apr-2016, Sumit Maurya, #CC01, New Properties and Method created to fetch ModelDetails, SalesChannelDetail according to data supplied in xml
 * 07-Apr-2016, Sumit Maurya, #CC02, New methods created for Manage GiftsV2 interface. And parameter supplied in method to fetch data according to Scheme.
 * */

using System;
using System.Data.SqlTypes;
using System.Xml;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;


public class SchemeData : IDisposable
{
    #region Private Class Variables

    private Int32 intSchemeID;
    private Int16 intSchemePeriod, intSchemeLevel;
    private Int16 intSchemeStatus = 2;
    private string strSchemeName, strSchemeDecription, strLevelIds, strSalesChannelIds, strLocationIds, strSalesChannelTypeIds;
    private Nullable<DateTime> dtSchemeStartDate, dtSchemeEndDate;
    SqlParameter[] SqlParam;
    Int32 IntResultCount = 0;
    DataTable dtSchemeInfo;
    DataSet dsSchemeInfo;
    public Decimal AchievementFrom
    {
        get;
        set;
    }
    public Decimal AchievementTo
    {
        get;
        set;
    }
    public Int32 SchemePerformanceCalculationID
    {
        get;
        set;
    }
    public Decimal PayOutRate
    {
        get;
        set;
    }
    public Int32 SchemeComponentPayoutSlabID
    {
        get;
        set;
    }
    public Int32 Flag
    {
        get;
        set;
    }
    public Int32 SchemeComponentFilterValueID
    {
        get;
        set;
    }
    public string SchemeFilterValue
    {
        get;
        set;
    }
    public string SchemeFilterName
    {
        get;
        set;
    }
    public Int32 SchemeFilterID
    {
        get;
        set;
    }
    /* #CC01 Add Start */
    public string strMobileDetailXML;
    public string strGetSalesChannelDetailXML;
    /* #CC01 Add End */


    #endregion

    #region Public Properties
    public Int32 SalesChannelTypeID
    {
        get;
        set;
    }
    public Int32 ComponentID        //Pankaj Dhingra
    {
        get;
        set;
    }
    public Int16 Counter
    {
        get;
        set;
    }
    public Int32 salesChannelID
    {
        get;
        set;
    }
    public string FromDate
    {
        get;
        set;

    }
    public Int32 HierarchyLevelID
    {
        get;
        set;
    }
    public string ToDate
    {
        get;
        set;
    }
    public string LevelIds
    {
        get { return strLevelIds; }
        set { strLevelIds = value; }
    }
    public string SalesChannelIds
    {
        get { return strSalesChannelIds; }
        set { strSalesChannelIds = value; }
    }
    public string LocationIds
    {
        get { return strLocationIds; }
        set { strLocationIds = value; }
    }
    public string SalesChannelTypeIds
    {
        get { return strSalesChannelTypeIds; }
        set { strSalesChannelTypeIds = value; }
    }

    public DataAccess.EnumData.eSchemeTemplateType SchemeType
    {
        get;
        set;
    }
    public DataAccess.EnumData.SchemeComponentType ComponentType
    {
        get;
        set;
    }
    public DataAccess.EnumData.eRequestforDeleteStatus SchemeUpdateStatus
    {
        get;
        set;
    }

    public Int16 SchemeLevel
    {
        get { return intSchemeLevel; }
        set { intSchemeLevel = value; }
    }
    public Int32 SchemeID
    {
        get
        {
            return intSchemeID;
        }
        set
        {
            intSchemeID = value;
        }
    }
    public string SchemeName
    {
        get
        {
            return strSchemeName;
        }
        set
        {
            strSchemeName = value;
        }
    }
    public int PayOutBase
    {
        get;
        set;
    }
    public string SchemeDecription
    {
        get
        {
            return strSchemeDecription;
        }
        set
        {
            strSchemeDecription = value;
        }
    }
    public int FinancialCalenderId
    {
        get;
        set;
    }
    public Int16 SchemeSelectionId
    {
        get;
        set;
    }
    public int AccessType
    {
        get;
        set;
    }
    public Int16 BaseType
    {
        get;
        set;
    }
    public int UserId
    {
        get;
        set;
    }
    public Int16 BasedOn
    {
        get;
        set;
    }
    public int ComponentTypeID
    {
        get;
        set;
    }
    public Nullable<DateTime> SchemeStartDate
    {
        get { return dtSchemeStartDate; }
        set { dtSchemeStartDate = value; }
    }
    public Nullable<DateTime> SchemeEndDate
    {
        get { return dtSchemeEndDate; }
        set { dtSchemeEndDate = value; }
    }
    public Int16 SchemePeriod
    {
        get { return intSchemePeriod; }
        set { intSchemePeriod = value; }
    }
    public Int16 Status
    {
        get { return intSchemeStatus; }
        set { intSchemeStatus = value; }
    }
    public string ErrorXML
    {
        get;
        set;
    }
       
    public string ErrorMessage
    {
        get;
        set;
    }
    public Int16 IsTarget
    {
        get;
        set;
    }
    public Int16 SelectionMode
    {
        get;
        set;
    }
    public int GiftID
    {
        get;
        set;
    }
    public string GiftName
    {
        get;
        set;
    }
    public int EligiblityPoints
    {
        get;
        set;
    }
    public int GiftStatus
    {
        get;
        set;
    }
    public string SchemeDocumentFileName
    {
        get;
        set;
    }
    public string OfflineSchemeCode
    {
        get;
        set;
    }
    public Int16? OfflineStatus
    {
        get;
        set;
    }
    //public string RetailerCode
    //{
    //    get;
    //    set;
    //}
    public string RetailerName
    {
        get;
        set;
    }
    public int? nullableUserID
    { get; set; }

    public string RetailersIds
    {
        get;
        set;
    }
    public string Error;

    public string ModelDetailXML
    {
        get { return strMobileDetailXML; }
        set { strMobileDetailXML = value; }
    }
    public string GetSalesChannelDetailXML
    {
        get { return strGetSalesChannelDetailXML; }
        set { strGetSalesChannelDetailXML = value; }
    }
    public int OutParam
    {
        get;
        set;
    }


    #endregion

    # region Gifts

    public void InsUpdGiftInfo()
    {
        try
        {
            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@giftname", GiftName);
            SqlParam[1] = new SqlParameter("@giftid", GiftID);
            SqlParam[2] = new SqlParameter("@status", GiftStatus);
            SqlParam[3] = new SqlParameter("@eligiblitypoint", EligiblityPoints);
            SqlParam[4] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[4].Direction = ParameterDirection.Output;
            IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("[prcInsertUpdGifts]", SqlParam);
            if (SqlParam[4].Value != "" && SqlParam[4].Value != null)
            {
                Error = SqlParam[4].Value.ToString();
            }

        }
        catch (Exception ex)
        {

            throw ex;
        }


    }


    public DataTable GetGiftInfo()
    {
        try
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@giftid", GiftID);
            SqlParam[1] = new SqlParameter("@giftname", GiftName);
            SqlParam[2] = new SqlParameter("@SchemeID", SchemeID);
            DataTable ds = DataAccess.DataAccess.Instance.GetTableFromDatabase("[prcGetSchemeItemsDetails]", CommandType.StoredProcedure, SqlParam);
            return ds;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }



    public DataSet dsGetSchemeDetail()
    {
        try
        {
            DataSet ds = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("[prcGetSchemeDetailForGifts]", CommandType.StoredProcedure);
            return ds;
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    public int InsUpdGiftInfoV2()
    {
        try
        {
            int intresult;
            SqlParam = new SqlParameter[7];
            SqlParam[0] = new SqlParameter("@giftname", GiftName);
            SqlParam[1] = new SqlParameter("@giftid", GiftID);
            SqlParam[2] = new SqlParameter("@status", GiftStatus);
            SqlParam[3] = new SqlParameter("@eligiblitypoint", EligiblityPoints);
            SqlParam[4] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
            SqlParam[4].Direction = ParameterDirection.Output;
            SqlParam[5] = new SqlParameter("@OutError", SqlDbType.NVarChar, 500);
            SqlParam[5].Direction = ParameterDirection.Output;

            SqlParam[6] = new SqlParameter("@SchemeID", SchemeID);
            DataAccess.DataAccess.Instance.DBInsertCommand("[prcInsertUpdGiftsV2]", SqlParam);
            IntResultCount = Convert.ToInt32(SqlParam[4].Value);
            if (SqlParam[5].Value != "" && SqlParam[5].Value != null)
            {
                Error = SqlParam[5].Value.ToString();
            }
            return IntResultCount;

        }
        catch (Exception ex)
        {

            throw ex;
        }


    }



    #endregion

    #region Insert Update and Delete

    public Int32 UpdateScheme()
    {
        try
        {
            IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("prcInsUpdSchemeCalculatedPoints", CommandType.StoredProcedure);
            return IntResultCount;
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
    public void UpdateSchemeInfo()
    {
        try
        {
            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@schemeid", intSchemeID);
            SqlParam[1] = new SqlParameter("@dateFrom", FromDate);
            SqlParam[2] = new SqlParameter("@dateTo", ToDate);
            SqlParam[3] = new SqlParameter("@schemename", strSchemeName);
            SqlParam[4] = new SqlParameter("@status", Status);
            IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("prcUpdScheme", SqlParam);

        }
        catch (Exception ex)
        {

            throw ex;
        }


    }
    public void UpdateSchemeMappingInfo()
    {
        try
        {
            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@SchemeID", intSchemeID);
            SqlParam[1] = new SqlParameter("@LevelIds", strLevelIds);
            SqlParam[2] = new SqlParameter("@LocationIds", strLocationIds);
            SqlParam[3] = new SqlParameter("@SalesChannelIds", strSalesChannelIds);
            SqlParam[4] = new SqlParameter("@SalesChannelTypeIds", strSalesChannelTypeIds);
            IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("prcUpdSchemeMappingDetails", SqlParam);
            //intSchemeID = Convert.ToInt32(SqlParam[1].Value);

        }
        catch (Exception ex)
        {

            throw ex;
        }


    }
    public string ErrorDetailXML
    {
        get;
        set;
    }
    public void UpdateSchemeProductInfo(DataTable dt)
    {
        try
        {
            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@tvpScheme", SqlDbType.Structured);
            SqlParam[0].Value = dt;

            SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
            SqlParam[3].Direction = ParameterDirection.Output;
            SqlParam[4] = new SqlParameter("@SchemeID", intSchemeID);
            IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("prcUPDSchemeProductDetails", SqlParam);
            //intSchemeID = Convert.ToInt32(SqlParam[1].Value);
            if (SqlParam[3].Value.ToString() != "")
            {
                ErrorDetailXML = SqlParam[3].Value.ToString();
            }
            else
            {
                ErrorDetailXML = null;
            }
            Error = Convert.ToString(SqlParam[2].Value);

        }
        catch (Exception ex)
        {

            throw ex;
        }


    }
    public Int32 InsertUpdateScheme(DataTable dt, DataTable dt1, DataTable dt2)
    {
        try
        {
            SqlParam = new SqlParameter[20];
            SqlParam[0] = new SqlParameter("@tvpScheme", SqlDbType.Structured);
            SqlParam[0].Value = dt;
            SqlParam[1] = new SqlParameter("@tvpexcludedModels", SqlDbType.Structured);
            SqlParam[1].Value = dt1;
            SqlParam[2] = new SqlParameter("@SchemeID", intSchemeID);
            SqlParam[3] = new SqlParameter("@calenderID", FinancialCalenderId);
            SqlParam[4] = new SqlParameter("@componentTypeId", ComponentTypeID);
            SqlParam[5] = new SqlParameter("@ErrorXMl", SqlDbType.Xml, 8000);
            SqlParam[5].Direction = ParameterDirection.Output;
            SqlParam[6] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[6].Direction = ParameterDirection.Output;
            SqlParam[7] = new SqlParameter("@SchemeName", strSchemeName);
            SqlParam[8] = new SqlParameter("@LevelIds", strLevelIds);
            SqlParam[9] = new SqlParameter("@LocationIds", strLocationIds);
            SqlParam[10] = new SqlParameter("@SalesChannelIds", strSalesChannelIds);
            SqlParam[11] = new SqlParameter("@SalesChannelTypeIds", strSalesChannelTypeIds);
            SqlParam[12] = new SqlParameter("@SchemeLevel", SchemeLevel);
            SqlParam[13] = new SqlParameter("@payoutbase", PayOutBase);
            SqlParam[14] = new SqlParameter("@userID", UserId);
            SqlParam[15] = new SqlParameter("@tvpOF", SqlDbType.Structured);
            SqlParam[15].Value = dt2;
            SqlParam[16] = new SqlParameter("@istarget", IsTarget);
            SqlParam[17] = new SqlParameter("@startdate", FromDate);
            SqlParam[18] = new SqlParameter("@startdate", FromDate);
            SqlParam[18] = new SqlParameter("@enddate", ToDate);
            SqlParam[19] = new SqlParameter("@RetailersIds", RetailersIds);
            IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("prcInsUpdScheme", SqlParam);
            //intSchemeID = Convert.ToInt32(SqlParam[1].Value);
            if (SqlParam[5].Value.ToString() != "")
            {
                ErrorXML = SqlParam[5].Value.ToString();

            }
            else
            {
                ErrorXML = null;
            }
            ErrorMessage = SqlParam[6].Value.ToString();

            return IntResultCount;
        }
        catch (Exception ex)
        {

            throw ex;
        }


    }
    public void UpdateSchemeStatus()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@status", Status);
            SqlParam[1] = new SqlParameter("@SchemeID", intSchemeID);
            IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("[prcSchemeActiveDeActive]", SqlParam);
            //intSchemeID = Convert.ToInt32(SqlParam[1].Value);
        }
        catch (Exception ex)
        {

            throw ex;
        }


    }
    //public Int32 UpdateOrDelete()
    //{ 
    //        try 
    //        {
    //            SqlParam = new SqlParameter[4];
    //            SqlParam[0] = new SqlParameter("@CompanyID", intCompanyID);
    //            SqlParam[1] = new SqlParameter("@SchemeID", intSchemeID);
    //            SqlParam[2] = new SqlParameter("@type", (int)SchemeUpdateStatus);
    //            SqlParam[3] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar,200);
    //            SqlParam[3].Direction=ParameterDirection.Output;
    //            IntResultCount=DataAccess.Instance.DBInsertCommand("prcUpdDelScheme",SqlParam);
    //            ErrorMessage = SqlParam[3].Value.ToString();
    //            return IntResultCount;
    //        }
    //        catch (Exception ex)
    //        {

    //            throw ex;
    //        }


    //}
    public void InsUpdOfflineScheme(int Condition)
    {
        SqlParam = new SqlParameter[9];
        SqlParam[0] = new SqlParameter("@SchemeName", SchemeName);
        SqlParam[1] = new SqlParameter("@SchemeDescription", SchemeDecription);
        SqlParam[2] = new SqlParameter("@SchemeDocumentFileName", SchemeDocumentFileName);
        SqlParam[3] = new SqlParameter("@SchemeStartDate", SchemeStartDate);
        SqlParam[4] = new SqlParameter("@SchemeEndDate", SchemeEndDate);
        SqlParam[5] = new SqlParameter("@Status", OfflineStatus);
        SqlParam[6] = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 200);
        SqlParam[6].Direction = ParameterDirection.Output;
        SqlParam[7] = new SqlParameter("@OfflineSchemeID", SchemeID);
        SqlParam[8] = new SqlParameter("@Condition", Condition);
        DataAccess.DataAccess.Instance.DBInsertCommand("prcInsUpdOfflineScheme", SqlParam);
        ErrorMessage = SqlParam[6].Value.ToString();
    }
    public void InsSchemePerformance(DataTable dt)
    {
        SqlParam = new SqlParameter[3];
        SqlParam[0] = new SqlParameter("@SchemeTable", dt);
        SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 200);
        SqlParam[1].Direction = ParameterDirection.Output;
        SqlParam[2] = new SqlParameter("@ErrorXml", SqlDbType.Xml, 2);
        SqlParam[2].Direction = ParameterDirection.Output;
        DataAccess.DataAccess.Instance.DBInsertCommand("prcInsUpdSchemePerformance", SqlParam);
        if (SqlParam[2].Value.ToString() != "")
        {
            ErrorDetailXML = SqlParam[2].Value.ToString();
        }
        else { ErrorDetailXML = null; }
        ErrorMessage = SqlParam[1].Value.ToString();
    }
    #endregion

    #region Fetch Scheme Information
    public DataTable GetSchemeInformation(DataTable dt2)
    {
        try
        {
            SqlParam = new SqlParameter[8];
            SqlParam[0] = new SqlParameter("@schemeID", intSchemeID);
            SqlParam[1] = new SqlParameter("@schemeName", strSchemeName);
            SqlParam[2] = new SqlParameter("@componenttypeid", ComponentTypeID);
            SqlParam[3] = new SqlParameter("@payoutbase", PayOutBase);
            SqlParam[4] = new SqlParameter("@selectionmode", SelectionMode);
            SqlParam[5] = new SqlParameter("@dateFrom", FromDate);
            SqlParam[6] = new SqlParameter("@dateTo", ToDate);
            SqlParam[7] = new SqlParameter("@tvpOF", SqlDbType.Structured);
            SqlParam[7].Value = dt2;

            dtSchemeInfo = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("[prcGetSchemeDetails]", CommandType.StoredProcedure, SqlParam).Tables[0];
            return dtSchemeInfo;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    public DataSet GetSchemeDetailInformation()
    {
        try
        {
            DataSet dsScheme = new DataSet();
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@schemeID", intSchemeID);
            SqlParam[1] = new SqlParameter("@Flag", Flag);
            dsScheme = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetSchemeDetailsInformation", CommandType.StoredProcedure, SqlParam);
            return dsScheme;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    public Int32 InsertUpdateSchemeFilter()
    {
        try
        {
            SqlParam = new SqlParameter[7];
            SqlParam[0] = new SqlParameter("@SchemeFilterID", SchemeFilterID);
            SqlParam[1] = new SqlParameter("@SchemeFilterName", SchemeFilterName);
            SqlParam[2] = new SqlParameter("@SchemeFilterValue", SchemeFilterValue);
            SqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[3].Direction = ParameterDirection.Output;
            SqlParam[4] = new SqlParameter("@SchemeComponentFilterValueID", SchemeComponentFilterValueID);
            SqlParam[5] = new SqlParameter("@SchemeID", SchemeID);
            SqlParam[6] = new SqlParameter("@Flag", Flag);
            DataAccess.DataAccess.Instance.DBInsertCommand("PrcInsUpdSchemeFilterInfo", SqlParam);
            IntResultCount = Convert.ToInt16(SqlParam[3].Value);
            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public Int32 InsertUpdateSchemePayOut()
    {
        try
        {
            SqlParam = new SqlParameter[7];
            SqlParam[0] = new SqlParameter("@AchievementFrom", AchievementFrom);
            SqlParam[1] = new SqlParameter("@AchievementTo", AchievementTo);
            SqlParam[2] = new SqlParameter("@PayoutRate", PayOutRate);
            SqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[3].Direction = ParameterDirection.Output;
            SqlParam[4] = new SqlParameter("@SchemeComponentPayoutSlabID", SchemeComponentPayoutSlabID);
            SqlParam[5] = new SqlParameter("@SchemeID", SchemeID);
            SqlParam[6] = new SqlParameter("@Flag", Flag);
            DataAccess.DataAccess.Instance.DBInsertCommand("PrcInsUpdSchemePayoutInfo", SqlParam);
            IntResultCount = Convert.ToInt16(SqlParam[3].Value);
            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public Int32 DeleteSchemePayOut()
    {
        try
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@SchemeComponentPayoutSlabID", SchemeComponentPayoutSlabID);
            SqlParam[2] = new SqlParameter("@SchemeID", SchemeID);
            DataAccess.DataAccess.Instance.DBInsertCommand("PrcDeletePayoutInfo", SqlParam);
            IntResultCount = Convert.ToInt16(SqlParam[0].Value);
            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public Int32 ExecuteSchemePerforma()
    {
        try
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@ProcessingSchemeID", SchemeID);
            DataAccess.DataAccess.Instance.DBInsertCommand("prcSchemeCalculationMainPOC", SqlParam);
            IntResultCount = Convert.ToInt16(SqlParam[0].Value);
            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetSchemeComponentFilterMaster()
    {
        try
        {
            DataTable dtScheme = new DataTable();
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@Flag", Flag);
            dtScheme = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSchemeComponentFilterMaster", CommandType.StoredProcedure, SqlParam);
            return dtScheme;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    public DataSet GetSchemeDetailsInformation()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@schemeID", intSchemeID);
            SqlParam[1] = new SqlParameter("@SchemeBased", SqlDbType.Int);
            SqlParam[1].Direction = ParameterDirection.Output;
            DataSet ds = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetSelectedSchemeDetails", CommandType.StoredProcedure, SqlParam);
            BasedOn = Convert.ToInt16(SqlParam[1].Value);
            return ds;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    public int SelectionType
    {
        get;
        set;
    }
    public DataTable GetSchemeComponentsTypeDetails()
    {
        try
        {

            DataSet ds = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("[PrcgetSchemeComponents]", CommandType.StoredProcedure);
            return ds.Tables[0];
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    public DataTable GetPaymentTypeDetails()
    {
        try
        {
            DataSet ds = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("[prcGetPayoutTypeInfo]", CommandType.StoredProcedure);
            return ds.Tables[0];
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    public DataSet GetSchemeInfoDetails()
    {
        try
        {
            SqlParam = new SqlParameter[8];
            SqlParam[0] = new SqlParameter("@SchemeID", intSchemeID);
            SqlParam[1] = new SqlParameter("@SchemeStartDate", SchemeStartDate);
            SqlParam[2] = new SqlParameter("@SchemeEndDate", SchemeEndDate);
            SqlParam[3] = new SqlParameter("@SalesChannelID", salesChannelID);
            SqlParam[4] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
            SqlParam[5] = new SqlParameter("@Counter", Counter);
            SqlParam[6] = new SqlParameter("@Status", Status);
            SqlParam[7] = new SqlParameter("@UserID", UserId);
            DataSet ds = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetSchemeInfo", CommandType.StoredProcedure, SqlParam);
            return ds;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    public DataSet GetSchemeFilterDetails()
    {
        try
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@SchemePerformanceCalculationID", SchemePerformanceCalculationID);
            DataSet ds = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetSchemeFilterInfo", CommandType.StoredProcedure, SqlParam);
            return ds;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    public DataSet GetSchemeTemplate()
    {
        try
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@SchemeType", SchemeType);
            dsSchemeInfo = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcgetSchemeTemplate", CommandType.StoredProcedure, SqlParam);
            return dsSchemeInfo;

        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
    public DataTable GetOfflineScheme(int Condition)
    {
        dtSchemeInfo = new DataTable();
        SqlParam = new SqlParameter[6];
        SqlParam[0] = new SqlParameter("@OfflineSchemeID", SchemeID);
        SqlParam[1] = new SqlParameter("@SchemeName", SchemeName);
        SqlParam[2] = new SqlParameter("@SchemeCode", OfflineSchemeCode);
        SqlParam[3] = new SqlParameter("@Status", OfflineStatus);//send only for user
        SqlParam[4] = new SqlParameter("@Condition", Condition);
        SqlParam[5] = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 200);
        SqlParam[5].Direction = ParameterDirection.Output;
        dtSchemeInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetOfflineScheme", CommandType.StoredProcedure, SqlParam);
        return dtSchemeInfo;
    }
    public DataSet GetSchemeAndRetailer()
    {
        dsSchemeInfo = new DataSet();
        dsSchemeInfo = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetSchemeAndRetailer", CommandType.StoredProcedure);
        return dsSchemeInfo;
    }
    public DataTable GetOfflineSchemePayout()
    {
        dtSchemeInfo = new DataTable();
        SqlParam = new SqlParameter[3];
        SqlParam[0] = new SqlParameter("@SchemeName", SchemeName);
        SqlParam[1] = new SqlParameter("@RetailerName", RetailerName);
        SqlParam[2] = new SqlParameter("@UserID", nullableUserID);
        dtSchemeInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetOfflineSchemePayout", CommandType.StoredProcedure, SqlParam);
        return dtSchemeInfo;
    }


    public DataSet GetModelsDetail()
    {
        dsSchemeInfo = new DataSet();
        SqlParam = new SqlParameter[3];
        SqlParam[0] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
        SqlParam[0].Direction = ParameterDirection.Output;
        SqlParam[1] = new SqlParameter("@ModelDetailXML", SqlDbType.Xml);
        SqlParam[1].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(ModelDetailXML, XmlNodeType.Document, null));
        SqlParam[1].Direction = ParameterDirection.Input;
        dsSchemeInfo = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetExcludedModelDetails", CommandType.StoredProcedure, SqlParam);

        OutParam = Convert.ToInt16(SqlParam[0].Value);
        return dsSchemeInfo;

    }


    public DataSet GetSalesChannelDetailForExclude()
    {
        try
        {
            dsSchemeInfo = new DataSet();
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@OutError", SqlDbType.NVarChar, 500);
            SqlParam[1].Direction = ParameterDirection.Output;
            dsSchemeInfo = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetSalesChannelDetailForScheme", CommandType.StoredProcedure, SqlParam);
            return dsSchemeInfo;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataSet GetSalesChannelDetail()
    {
        dsSchemeInfo = new DataSet();
        SqlParam = new SqlParameter[3];
        SqlParam[0] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
        SqlParam[0].Direction = ParameterDirection.Output;
        SqlParam[1] = new SqlParameter("@SalesChannelDetailDetailXML", SqlDbType.Xml);
        SqlParam[1].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(strGetSalesChannelDetailXML, XmlNodeType.Document, null));
        SqlParam[1].Direction = ParameterDirection.Input;
        dsSchemeInfo = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetSalesChannelDetail", CommandType.StoredProcedure, SqlParam);

        OutParam = Convert.ToInt16(SqlParam[0].Value);
        return dsSchemeInfo;

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

    ~SchemeData()
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
                if (SqlParam != null)
                {
                    SqlParam = null;
                }
                // Released managed Resources
            }
        }
    }

    #endregion
}
