/*#region Copyright and page info
============================================================================================================================================
Copyright	: Zed-Axis Technologies, 2013
Author		: Shashikant singh
Create date	: 15-July-2013
Description	: Entity Type Master.
============================================================================================================================================
Change Log:
dd-MMM-yy,      Name ,            #CCxx            - Description
15-jul-2013  Shashikant singh    #CC01              Commented Old Code Which Was Simlar as new code
 05-05-2015 rajesh upadhyay #CC02 - property added for entityid and line added in method
 08-05-2015 rajesh upadhyay #CC03 - lines added 
 08-10-2015 Shashikant Singh #CC04 - Added some new property as LoginedEntityTypeId ,LoginEntityId
--------------------------------------------------------------------------------------------------------------------------------------------
#endregion
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data.Common;
using Microsoft.ApplicationBlocks.Data;


public class clsEntityTypeMaster : IDisposable
{
    public static string _ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AppConString"].ConnectionString;
    #region Private Class Variables

    /* #CC01 Commented
        private short _shtEntityTypeID;
        private string _strEntityType;
        private DateTime _dtCreatedOn;
        private int _intCreatedBy;
        private DateTime _dtModifiedOn;
        private int _intModifiedBy;
        private bool _blnActive;

        private string _strError;
        private Int32 _intPageIndex;
        private Int32 _intPageSize;
        private Int32 _intTotalRecords;
         */

    // #CC01 Added    

    private int _shtEntityTypeID;
    private short _shtHolidayCalendarRequired;
    private string _strEntityType;
    private short _shtBaseEntityTypeID;
    private string _strBaseEntityType;
    private bool _blnBaseEntityActive;
    private short _shtBaseEntityActive;
    private DateTime _dtCreatedOn;
    private int _intCreatedBy;
    private DateTime _dtModifiedOn;
    private int _intModifiedBy;
    private bool _blnActive;
    private short _shtActiveStatus;
    private short _shtStockMaintainedBySystem;
    private short _shtBrandCategoryMappingMode;
    private short _shtGroupMappingMode;
    private short _shtWeeklyOffMode;
    private short _shtPriceGroupMode;
    private short _shtEntityAddressMode;
    private short _shtEntityContactMode;
    private short _shtEntityDetailMode;
    private short _shtApplicationWorkingMode;
    private short _shtEntityStatutoryMode;
    private short _shtEntityBankMode;
    private short _shtAccessType;
    private short _shtCreditTermMode;
    private short _shtJournalMode;
    private short _shtTargetMode;
    private short _shtServiceCustomerMode;
    private short _shtServicePJPMode;
    private short _shtSAPCodeMode;
    private short _shtAuthoriseForLeadPass;
    private short _shtCityMappingMode;
    private short _shtEntityMilMode;
    private TimeSpan? _InTime;
    private TimeSpan? _OutTime;
    private string _strError;
    private Int32 _intPageIndex;
    private Int32 _intPageSize;
    private Int32 _intTotalRecords;
    private int _intEntityId;/* #CC02 added */
    private int _LoginedEntityTypeId;/*#CC04:Added*/
    private int _LoginEntityId;/*#CC04:Added*/
    #endregion

    #region Public Properties
    
    public DataTable Dt;
    public int LoginEntityId
    {
        get
        {
            return _LoginEntityId;
        }
        set
        {
            _LoginEntityId = value;
        }
    }
    public int LoginedEntityTypeId
    {
        get
        {
            return _LoginedEntityTypeId;
        }
        set
        {
            _LoginedEntityTypeId = value;
        }
    }
    public int EntityId
    {
        get
        {
            return _intEntityId;
        }
        set
        {
            _intEntityId = value;
        }
    }
    
    public short HolidayCalendarRequired
    {
        get
        {
            return _shtHolidayCalendarRequired;
        }
        set
        {
            _shtHolidayCalendarRequired = value;
        }
    }

    public int EntityTypeID
    {
        get
        {
            return _shtEntityTypeID;
        }
        set
        {
            _shtEntityTypeID = value;
        }
    }
    public string EntityType
    {
        get
        {
            return _strEntityType;
        }
        set
        {
            _strEntityType = value;
        }
    }
    public short BaseEntityTypeID
    {
        get
        {
            return _shtBaseEntityTypeID;
        }
        set
        {
            _shtBaseEntityTypeID = value;
        }
    }

    public string BaseEntityType
    {
        get
        {
            return _strBaseEntityType;
        }
        set
        {
            _strBaseEntityType = value;
        }
    }
    public bool BaseEntityTypeActive
    {
        get
        {
            return _blnBaseEntityActive;
        }
        set
        {
            _blnBaseEntityActive = value;
        }
    }
    public DateTime CreatedOn
    {
        get
        {
            return _dtCreatedOn;
        }
        set
        {
            _dtCreatedOn = value;
        }
    }
    public int CreatedBy
    {
        get
        {
            return _intCreatedBy;
        }
        set
        {
            _intCreatedBy = value;
        }
    }
    public TimeSpan? InTime
    {
        get
        {
            return _InTime;
        }
        set
        {
            _InTime = value;
        }
    }

    public TimeSpan? OutTime
    {
        get
        {
            return _OutTime;
        }
        set
        {
            _OutTime = value;
        }
    }
    public DateTime ModifiedOn
    {
        get
        {
            return _dtModifiedOn;
        }
        set
        {
            _dtModifiedOn = value;
        }
    }
    public int ModifiedBy
    {
        get
        {
            return _intModifiedBy;
        }
        set
        {
            _intModifiedBy = value;
        }
    }
    public bool Active
    {
        get
        {
            return _blnActive;
        }
        set
        {
            _blnActive = value;
        }
    }
    public short ActiveStatus
    {
        get
        {
            return _shtActiveStatus;
        }
        set
        {
            _shtActiveStatus = value;
        }
    }
    public short BaseEntityActiveStatus
    {
        get
        {
            return _shtBaseEntityActive;
        }
        set
        {
            _shtBaseEntityActive = value;
        }
    }
    public short StockMaintainedBySystem
    {
        get
        {
            return _shtStockMaintainedBySystem;
        }
        set
        {
            _shtStockMaintainedBySystem = value;
        }
    }
    public short BrandCategoryMappingMode
    {
        get
        {
            return _shtBrandCategoryMappingMode;
        }
        set
        {
            _shtBrandCategoryMappingMode = value;
        }
    }
    public short GroupMappingMode
    {
        get
        {
            return _shtGroupMappingMode;
        }
        set
        {
            _shtGroupMappingMode = value;
        }
    }
    public short WeeklyOffMode
    {
        get
        {
            return _shtWeeklyOffMode;
        }
        set
        {
            _shtWeeklyOffMode = value;
        }
    }
    public short PriceGroupMode
    {
        get
        {
            return _shtPriceGroupMode;
        }
        set
        {
            _shtPriceGroupMode = value;
        }
    }
    public short EntityAddressMode
    {
        get
        {
            return _shtEntityAddressMode;
        }
        set
        {
            _shtEntityAddressMode = value;
        }
    }
    public short EntityContactMode
    {
        get
        {
            return _shtEntityContactMode;
        }
        set
        {
            _shtEntityContactMode = value;
        }
    }
    public short EntityDetailMode
    {
        get
        {
            return _shtEntityDetailMode;
        }
        set
        {
            _shtEntityDetailMode = value;
        }
    }
    public short ApplicationWorkingMode
    {
        get
        {
            return _shtApplicationWorkingMode;
        }
        set
        {
            _shtApplicationWorkingMode = value;
        }
    }
    public short EntityStatutoryMode
    {
        get
        {
            return _shtEntityStatutoryMode;
        }
        set
        {
            _shtEntityStatutoryMode = value;
        }
    }
    public short EntityBankMode
    {
        get
        {
            return _shtEntityBankMode;
        }
        set
        {
            _shtEntityBankMode = value;
        }
    }
    public short AccessType
    {
        get
        {
            return _shtAccessType;
        }
        set
        {
            _shtAccessType = value;
        }
    }
    public short CreditTermMode
    {
        get
        {
            return _shtCreditTermMode;
        }
        set
        {
            _shtCreditTermMode = value;
        }
    }
    public short JournalMode
    {
        get
        {
            return _shtJournalMode;
        }
        set
        {
            _shtJournalMode = value;
        }
    }
    public short TargetMode
    {
        get
        {
            return _shtTargetMode;
        }
        set
        {
            _shtTargetMode = value;
        }
    }
    public short ServiceCustomerMode
    {
        get
        {
            return _shtServiceCustomerMode;
        }
        set
        {
            _shtServiceCustomerMode = value;
        }
    }
    public short ServicePJPMode
    {
        get
        {
            return _shtServicePJPMode;
        }
        set
        {
            _shtServicePJPMode = value;
        }
    }
    public short SAPCodeMode
    {
        get
        {
            return _shtSAPCodeMode;
        }
        set
        {
            _shtSAPCodeMode = value;
        }
    }
    public short AuthoriseForLeadPass
    {
        get
        {
            return _shtAuthoriseForLeadPass;
        }
        set
        {
            _shtAuthoriseForLeadPass = value;
        }
    }
    public short CityMappingMode
    {
        get
        {
            return _shtCityMappingMode;
        }
        set
        {
            _shtCityMappingMode = value;
        }
    }
    public short EntityMilMode
    {
        get
        {
            return _shtEntityMilMode;
        }
        set
        {
            _shtEntityMilMode = value;
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
    public Int16 SalesChannelLavel { get; set; }
    public Int16 BilltoRetailor { get; set; }
    public Int16 StockTransferMode { get; set; }
    public Int16 ReportHierarchyLavel { get; set; }
    public Int16 IsPanMandatory { get; set; }
    public Int16 ApprolTypeId { get; set; }
    public Int16 StockMantainMode { get; set; }
    public Int16 ShowinApp { get; set; }
    public Int32 BackDayAllowforSale { get; set; }
    public Int32 BackDayAllowforSaleReturn { get; set; }
    public Int32 CompanyId { get; set; }
    public Int32 UserId { get; set; }
    public Int32 ParentHierarchyTypeId { get; set; }
    public string String1 { get; set; }
    public string String2 { get; set; }
    #endregion

    #region Constructors
    public clsEntityTypeMaster()
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

    ~clsEntityTypeMaster()
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
    public DataSet SelectById()
    {
        DataSet dsResult = new DataSet();
        SqlParameter[] objSqlParam = new SqlParameter[3];
        objSqlParam[0] = new SqlParameter("@EntityTypeID", EntityTypeID);
        objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[2].Direction = ParameterDirection.Output;
        dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcEntityTypeMaster_SelectById", CommandType.StoredProcedure, objSqlParam);
           
        if (dsResult != null && dsResult.Tables.Count > 0)
            //dtResult = dsResult.Tables[0];
        Error = Convert.ToString(objSqlParam[2].Value);
        if (Error != string.Empty)
        {
            throw new ArgumentException(Error);
        }

        return dsResult;
    }
   

    //#CC01 Added By Shashikant Singh on 15 July 2013 to gey entity type with base type id
    /// <summary>
    /// Toggle activation of selected record
    /// </summary>
    public bool ToggleActivation()
    {
        bool result = false;
        SqlParameter[] objSqlParam = new SqlParameter[3];
        objSqlParam[0] = new SqlParameter("@EntityTypeID", EntityTypeID);
        objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[2].Direction = ParameterDirection.Output;
        SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, "prcEntityTypeMaster_TActive", objSqlParam);
        if (Convert.ToInt16(objSqlParam[1].Value) == 0)
        {
            result = true;
        }
        Error = Convert.ToString(objSqlParam[2].Value);
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
    public Int16 Insert()
    {
        Int16 result = 1;
        SqlParameter[] objSqlParam = new SqlParameter[47];
        objSqlParam[0] = new SqlParameter("@EntityType", EntityType);
        objSqlParam[1] = new SqlParameter("@BaseEntityTypeID", BaseEntityTypeID);
        objSqlParam[2] = new SqlParameter("@CreatedBy", CreatedBy);
        objSqlParam[3] = new SqlParameter("@StockMaintainedBySystem", StockMaintainedBySystem);
        objSqlParam[4] = new SqlParameter("@BrandCategoryMappingMode", BrandCategoryMappingMode);
        objSqlParam[5] = new SqlParameter("@GroupMappingMode", GroupMappingMode);
        objSqlParam[6] = new SqlParameter("@WeeklyOffMode", WeeklyOffMode);
        objSqlParam[7] = new SqlParameter("@PriceGroupMode", PriceGroupMode);
        objSqlParam[8] = new SqlParameter("@EntityAddressMode", EntityAddressMode);
        objSqlParam[9] = new SqlParameter("@EntityContactMode", EntityContactMode);
        objSqlParam[10] = new SqlParameter("@EntityDetailMode", EntityDetailMode);
        objSqlParam[11] = new SqlParameter("@ApplicationWorkingMode", ApplicationWorkingMode);
        objSqlParam[12] = new SqlParameter("@EntityStatutoryMode", EntityStatutoryMode);
        objSqlParam[13] = new SqlParameter("@EntityBankMode", EntityBankMode);
        objSqlParam[14] = new SqlParameter("@AccessType", AccessType);
        objSqlParam[15] = new SqlParameter("@CreditTermMode", CreditTermMode);
        objSqlParam[16] = new SqlParameter("@JournalMode", JournalMode);
        objSqlParam[17] = new SqlParameter("@TargetMode", TargetMode);
        objSqlParam[18] = new SqlParameter("@ServiceCustomerMode", ServiceCustomerMode);
        objSqlParam[19] = new SqlParameter("@ServicePJPMode", ServicePJPMode);
        objSqlParam[20] = new SqlParameter("@SAPCodeMode", SAPCodeMode);
        objSqlParam[21] = new SqlParameter("@AuthoriseForLeadPass", AuthoriseForLeadPass);
        objSqlParam[22] = new SqlParameter("@CityMappingMode", CityMappingMode);
        objSqlParam[23] = new SqlParameter("@EntityMilMode", EntityMilMode);  
        objSqlParam[24] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[24].Direction = ParameterDirection.Output;
        objSqlParam[25] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[25].Direction = ParameterDirection.Output;
        objSqlParam[26] = new SqlParameter("@HolidayCalendarRequired", HolidayCalendarRequired);
        objSqlParam[27] = new SqlParameter("@SalesChannelLavel", SalesChannelLavel);
        objSqlParam[28] = new SqlParameter("@BilltoRetailor", BilltoRetailor);
        objSqlParam[29] = new SqlParameter("@StockTransferMode", StockTransferMode);
        objSqlParam[30] = new SqlParameter("@ReportHierarchyLavel", ReportHierarchyLavel);
        objSqlParam[31] = new SqlParameter("@IsPanMandatory", IsPanMandatory);
        objSqlParam[32] = new SqlParameter("@ApprolTypeId", ApprolTypeId);
        objSqlParam[33] = new SqlParameter("@StockMantainMode", StockMantainMode);
        objSqlParam[34] = new SqlParameter("@ShowinApp", ShowinApp);
        objSqlParam[35] = new SqlParameter("@BackDayAllowforSale", BackDayAllowforSale);
        objSqlParam[36] = new SqlParameter("@BackDayAllowforSaleReturn", BackDayAllowforSaleReturn);
        objSqlParam[37] = new SqlParameter("@Active", ActiveStatus);
        objSqlParam[38] = new SqlParameter("@CompanyId", CompanyId);

        objSqlParam[39] = new SqlParameter("@ParentHierarchyTypeId", ParentHierarchyTypeId);
        objSqlParam[40] = new SqlParameter("@ParentSalesChannelTypes", String1);
        objSqlParam[41] = new SqlParameter("@StockTransferSCTypes", String2);
        objSqlParam[42] = new SqlParameter("@tvpRole", Dt);  
        objSqlParam[43] = new SqlParameter("@UserID", CreatedBy);
        objSqlParam[44] = new SqlParameter("@InTime", InTime);
        objSqlParam[45] = new SqlParameter("@OutTime", OutTime);
        SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, "prcEntityTypeMaster_Insert", objSqlParam);
        result = Convert.ToInt16(objSqlParam[24].Value);
        Error = Convert.ToString(objSqlParam[25].Value);

        return result;
    }

    /// <summary>
    /// Update records in database.
    /// </summary>
    /// <results>Int16: 0 if success</results> 
    public Int16 Update()
    {
        Int16 result = 1;
        SqlParameter[] objSqlParam = new SqlParameter[47];
        objSqlParam[0] = new SqlParameter("@EntityTypeID", EntityTypeID);
        objSqlParam[1] = new SqlParameter("@EntityType", EntityType);
        objSqlParam[2] = new SqlParameter("@BaseEntityTypeID", BaseEntityTypeID);
        objSqlParam[3] = new SqlParameter("@ModifiedBy", ModifiedBy);
        objSqlParam[4] = new SqlParameter("@StockMaintainedBySystem", StockMaintainedBySystem);
        objSqlParam[5] = new SqlParameter("@BrandCategoryMappingMode", BrandCategoryMappingMode);
        objSqlParam[6] = new SqlParameter("@GroupMappingMode", GroupMappingMode);
        objSqlParam[7] = new SqlParameter("@WeeklyOffMode", WeeklyOffMode);
        objSqlParam[8] = new SqlParameter("@PriceGroupMode", PriceGroupMode);
        objSqlParam[9] = new SqlParameter("@EntityAddressMode", EntityAddressMode);
        objSqlParam[10] = new SqlParameter("@EntityContactMode", EntityContactMode);
        objSqlParam[11] = new SqlParameter("@EntityDetailMode", EntityDetailMode);
        objSqlParam[12] = new SqlParameter("@ApplicationWorkingMode", ApplicationWorkingMode);
        objSqlParam[13] = new SqlParameter("@EntityStatutoryMode", EntityStatutoryMode);
        objSqlParam[14] = new SqlParameter("@EntityBankMode", EntityBankMode);
        objSqlParam[15] = new SqlParameter("@AccessType", AccessType);
        objSqlParam[16] = new SqlParameter("@CreditTermMode", CreditTermMode);
        objSqlParam[17] = new SqlParameter("@JournalMode", JournalMode);
        objSqlParam[18] = new SqlParameter("@TargetMode", TargetMode);
        objSqlParam[19] = new SqlParameter("@ServiceCustomerMode", ServiceCustomerMode);
        objSqlParam[20] = new SqlParameter("@ServicePJPMode", ServicePJPMode);
        objSqlParam[21] = new SqlParameter("@SAPCodeMode", SAPCodeMode);
        objSqlParam[22] = new SqlParameter("@AuthoriseForLeadPass", AuthoriseForLeadPass);
        objSqlParam[23] = new SqlParameter("@CityMappingMode", CityMappingMode);
        objSqlParam[24] = new SqlParameter("@EntityMilMode", EntityMilMode);
        objSqlParam[28] = new SqlParameter("@Active", ActiveStatus);
        objSqlParam[25] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[25].Direction = ParameterDirection.Output;
        objSqlParam[26] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[26].Direction = ParameterDirection.Output;
        objSqlParam[27] = new SqlParameter("@HolidayCalendarRequired", HolidayCalendarRequired);
        objSqlParam[28] = new SqlParameter("@SalesChannelLavel", SalesChannelLavel);
        objSqlParam[29] = new SqlParameter("@BilltoRetailor", BilltoRetailor);
        objSqlParam[30] = new SqlParameter("@StockTransferMode", StockTransferMode);
        objSqlParam[31] = new SqlParameter("@ReportHierarchyLavel", ReportHierarchyLavel);
        objSqlParam[32] = new SqlParameter("@IsPanMandatory", IsPanMandatory);
        objSqlParam[33] = new SqlParameter("@ApprolTypeId", ApprolTypeId);
        objSqlParam[34] = new SqlParameter("@StockMantainMode", StockMantainMode);
        objSqlParam[35] = new SqlParameter("@ShowinApp", ShowinApp);
        objSqlParam[36] = new SqlParameter("@BackDayAllowforSale", BackDayAllowforSale);
        objSqlParam[37] = new SqlParameter("@BackDayAllowforSaleReturn", BackDayAllowforSaleReturn);
        objSqlParam[38] = new SqlParameter("@Active", ActiveStatus);
        objSqlParam[39] = new SqlParameter("@CompanyId", CompanyId);

        objSqlParam[40] = new SqlParameter("@ParentHierarchyTypeId", ParentHierarchyTypeId);
        objSqlParam[41] = new SqlParameter("@ParentSalesChannelTypes", String1);
        objSqlParam[42] = new SqlParameter("@StockTransferSCTypes", String2);
        objSqlParam[43] = new SqlParameter("@tvpRole", Dt);
        objSqlParam[44] = new SqlParameter("@InTime", InTime);
        objSqlParam[45] = new SqlParameter("@OutTime", OutTime);
        SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, "prcEntityTypeMaster_Update", objSqlParam);
        result = Convert.ToInt16(objSqlParam[25].Value);
        Error = Convert.ToString(objSqlParam[26].Value);

        return result;
    }
    /// <summary>
    /// Delete selected record.
    /// </summary>
    /// <results>bool: True if success</results> 
    public bool Delete()
    {
        bool result = false;
        SqlParameter[] objSqlParam = new SqlParameter[3];
        objSqlParam[0] = new SqlParameter("@EntityTypeID", EntityTypeID);
        objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[2].Direction = ParameterDirection.Output;
        SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, "prcEntityTypeMaster_Delete", objSqlParam);
        result = (Convert.ToInt16(objSqlParam[1].Value) == 0);
        Error = Convert.ToString(objSqlParam[2].Value);

        return result;
    }

    /// <summary>
    /// Get All records from database 
    /// send @PageSize as -1 if need all record and @Active=255 
    /// </summary>
    /// <results>DataTable: Collection of records</results> 		
    public DataSet SelectAllEntity()
    {
        DataSet dsResult = new DataSet();
        SqlParameter[] objSqlParam = new SqlParameter[10]; /*#CC04:Added from 7 to 8*/
        objSqlParam[0] = new SqlParameter("@EntityTypeID", EntityTypeID);
        objSqlParam[3] = new SqlParameter("@active", ActiveStatus);
        objSqlParam[1] = new SqlParameter("@PageIndex", PageIndex);
        objSqlParam[2] = new SqlParameter("@PageSize", PageSize);
        objSqlParam[4] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
        objSqlParam[4].Direction = ParameterDirection.Output;
        objSqlParam[5] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[5].Direction = ParameterDirection.Output;
        objSqlParam[6] = new SqlParameter("@EntityType", EntityType);
        objSqlParam[7] = new SqlParameter("@loginEntityTypeId", LoginedEntityTypeId); /*#CC04:Added */
        objSqlParam[8] = new SqlParameter("@loginEntityId", LoginEntityId); /*#CC04:Added */
        objSqlParam[9] = new SqlParameter("@CompanyId", CompanyId); /*#CC04:Added */
        dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcEntityTypeMaster_Select", CommandType.StoredProcedure, objSqlParam);
            
        //if (dsResult != null && dsResult.Tables.Count > 0)
        //    dtResult = dsResult.Tables[0];
        TotalRecords = Convert.ToInt32(objSqlParam[4].Value);
        Error = Convert.ToString(objSqlParam[5].Value);

        return dsResult;
    }


  
    public DataSet SelectAllBaseEntityType()
    {
       
        SqlParameter[] objSqlParam = new SqlParameter[7];
        objSqlParam[0] = new SqlParameter("@BaseEntityTypeID", BaseEntityType);
        objSqlParam[3] = new SqlParameter("@active", BaseEntityActiveStatus);
        objSqlParam[1] = new SqlParameter("@PageIndex", PageIndex);
        objSqlParam[2] = new SqlParameter("@PageSize", PageSize);
        objSqlParam[4] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
        objSqlParam[4].Direction = ParameterDirection.Output;
        objSqlParam[5] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[5].Direction = ParameterDirection.Output;
        objSqlParam[6] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[6].Direction = ParameterDirection.Output;
        DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcBaseEntityTypeMaster_Select", CommandType.StoredProcedure, objSqlParam);
        TotalRecords = Convert.ToInt32(objSqlParam[4].Value);
        Error = Convert.ToString(objSqlParam[5].Value);

        return dsResult;
    }

    public DataSet SelectAapplicationConfiguration()
    {
        DataSet dsResult = new DataSet();
        MySqlParameter[] objSqlParam = new MySqlParameter[6];
        objSqlParam[0] = new MySqlParameter("@p_OutError", MySqlDbType.VarChar, 500);
        objSqlParam[0].Direction = ParameterDirection.Output;
        objSqlParam[1] = new MySqlParameter("@p_OutParam", MySqlDbType.Int16, 2);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new MySqlParameter("@p_UserId", UserId);
        objSqlParam[3] = new MySqlParameter("@p_CompanyId", CompanyId); 
        objSqlParam[4] = new MySqlParameter("@p_active", ActiveStatus);
        objSqlParam[5] = new MySqlParameter("@p_TotalRecord", MySqlDbType.Int64,8);
        objSqlParam[5].Direction = ParameterDirection.Output;
        
        
        dsResult = DataAccess.DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcGetAppConfig", CommandType.StoredProcedure, objSqlParam);

        
        TotalRecords = Convert.ToInt32(objSqlParam[5].Value);
        Error = Convert.ToString(objSqlParam[0].Value);

        return dsResult;
    }
    
public int PWDEXPY {get;set;}
public int SERIALLENGTHMIN  {get;set;}
public int SERIALLENGTHMAX  {get;set;}
public int BATCHLENGTHMIN  {get;set;}
public int BATCHLENGTHMAX  {get;set;}
public int PWDCNT  {get;set;}
public int RetailerUniqueMobile  {get;set;}
public int ISPUniqueMobile  {get;set;}
public int ALLOWRETPARENTCHECK  {get;set;}
public int SecondarySalesReturnApproval  {get;set;}
public int RETAILERAPPROVALLEVELS  {get;set;}
public int SALESCHANNELAPPROVALVALUES  {get;set;}
public int IntermediarySalesReturnApproval  {get;set;}
public int ISDSalePunchStockOutUsingCSAAPI  {get;set;}
public int RETAPPAUTORECDAYS  {get;set;}
public int UploadDateFormat  {get;set;}
public int BeatPlanAutoApprove  {get;set;}
public int PhysicalStockAutoStockAdjustment  {get;set;}
public int APIStockAdjustmentResonID  {get;set;}
public int TopSalesChannel  {get;set;}
public int TopRetailer  {get;set;}
public int BackDateExpense { get; set; }

    public Int16 UpdateAppConfig()
    {
        DataSet dsResult = new DataSet();
        Int16 result = 1;
        MySqlParameter[] objSqlParam = new MySqlParameter[25];
        objSqlParam[0] = new MySqlParameter("@p_OutParam", MySqlDbType.Int16, 2);
        objSqlParam[0].Direction = ParameterDirection.Output;
        objSqlParam[1] = new MySqlParameter("@p_OutError", MySqlDbType.VarChar, 1000);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new MySqlParameter("@p_PWDEXPY", PWDEXPY);
        objSqlParam[3] = new MySqlParameter("@p_SERIALLENGTHMIN", SERIALLENGTHMIN);
        objSqlParam[4] = new MySqlParameter("@p_SERIALLENGTHMAX", SERIALLENGTHMAX);
        objSqlParam[5] = new MySqlParameter("@p_BATCHLENGTHMIN", BATCHLENGTHMIN);
        objSqlParam[6] = new MySqlParameter("@p_BATCHLENGTHMAX", BATCHLENGTHMAX);
        objSqlParam[7] = new MySqlParameter("@p_PWDCNT", @PWDCNT);



        objSqlParam[8] = new MySqlParameter("@p_RetailerUniqueMobile", RetailerUniqueMobile);
        objSqlParam[9] = new MySqlParameter("@p_ISPUniqueMobile", ISPUniqueMobile);
        objSqlParam[10] = new MySqlParameter("@p_ALLOWRETPARENTCHECK", ALLOWRETPARENTCHECK);
        objSqlParam[11] = new MySqlParameter("@p_SecondarySalesReturnApproval", SecondarySalesReturnApproval);
        objSqlParam[12] = new MySqlParameter("@p_RETAILERAPPROVALLEVELS", RETAILERAPPROVALLEVELS);
        objSqlParam[13] = new MySqlParameter("@p_SALESCHANNELAPPROVALVALUES", SALESCHANNELAPPROVALVALUES);
        objSqlParam[14] = new MySqlParameter("@p_IntermediarySalesReturnApproval", IntermediarySalesReturnApproval);
        objSqlParam[15] = new MySqlParameter("@p_ISDSalePunchStockOutUsingCSAAPI", ISDSalePunchStockOutUsingCSAAPI);
        objSqlParam[16] = new MySqlParameter("@p_RETAPPAUTORECDAYS", @RETAPPAUTORECDAYS);
        objSqlParam[17] = new MySqlParameter("@p_UploadDateFormat", UploadDateFormat);
        objSqlParam[18] = new MySqlParameter("@p_BeatPlanAutoApprove", BeatPlanAutoApprove);
        objSqlParam[19] = new MySqlParameter("@p_PhysicalStockAutoStockAdjustment", PhysicalStockAutoStockAdjustment);
        objSqlParam[20] = new MySqlParameter("@p_APIStockAdjustmentResonID", APIStockAdjustmentResonID);
        objSqlParam[21] = new MySqlParameter("@p_TopSalesChannel", TopSalesChannel);
        objSqlParam[22] = new MySqlParameter("@p_TopRetailer", TopRetailer);
        objSqlParam[23] = new MySqlParameter("@p_BackDateExpense", BackDateExpense);
        objSqlParam[24] = new MySqlParameter("@p_CompanyId", CompanyId);
        

        //SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, "pcrSaveApplicationConfiguration", objSqlParam);
        dsResult = DataAccess.DataAccess.Instance.GetDataSetFrom_MySqlDatabase("pcrSaveApplicationConfiguration", CommandType.StoredProcedure, objSqlParam);
        //MySqlHelper.ExecuteNonQuery(_ConnectionString,  "pcrSaveApplicationConfiguration", objSqlParam);
        result = Convert.ToInt16(objSqlParam[0].Value);
        Error = Convert.ToString(objSqlParam[1].Value);

        return result;
    }

    #endregion
}

