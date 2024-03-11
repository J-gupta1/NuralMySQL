using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Xml;
using DataAccess;
/*
 * 17-Dec-2014 , Sumit Kumar, #CC01, Create some function for check Invoice No existx or not , Binf NDS and Wahrehouse
 * 23-Apr-2015 , Karam Chand Sharma, #CC02, Create new properties and pass to the function
 * 05-May-2015 , Karam Chand Sharma, #CC03, pass user id into GetSalesChannelChildInfoV2 function
 * 06-Aug-2015 , Karam Chand Sharma, #CC04, Create a new function for salechannel , it will come according to sale channel type
 * 20-May-2016, Sumit Maurya, #CC05, New function created to save SaleschanneltostateMapping.
 * 11-Jul-2016 Sumit Maurya, #CC06, New properties Mobile number2 created and supplied in function to save/update SalesChannel Creation.
 * 18-Jul-2016, Sumit Maurya, #CC07, New property and method created for SalesChannelCurrentOutstanding.
 * 21-Jul-2016, Karam Chand Sharma, #CC08, Create a new function for downlolad bin code
 * 28-Jul-2016, Sumit Maurya, #CC09, New properties methods created for BulkMapping interface.
 * 19-Aug-2016, Sumit Maurya, #CC10, New property created and supplied as parameter to get Nd of loggedin Distributor .
 * 05-Sep-2016 , Sumit Maurya, #CC11, Parameter supplied in methods of Bulkupload mapping  to get details and update for ND.
 * 22-Sep-2016, Sumit Maurya, #CC12, New properties and  method created to get ND & RDS Detail.
 * 27-Sep-2016, Sumit Maurya, #CC13, New property created and value supplied to bind childsaleschannel.
 * 26-Jan-2018,Vijay Kumar Prajapati,#CC14,New Function Created for bind Relationtype.
 * * 29-Jan-2018,Rajnish Kumar ,#CC15,EntitytypeId,EntityMappingRelationTypeId parameter is added to save data.
 * 06-Feb-2018,Vijay Kumar Prajapati,#CC16,Add New Method For Bind Retailer and saleschannel referencecode.
 * 05-Mar-2018, Sumit Maurya,  #CC17 ,New Method created to get multiple tables from database for ref code (Done for ZedsalesV5)
  * 09-Mar-2018, Rajnish Kumar,  #CC18 ,RDS Approval Two Levels
 *  * 11-April-2018, Rajnish Kumar,  #CC19 ,Get Mapped and Unmapped Data
 *   *  * 04-May-2018, Rajnish Kumar,  #CC20 ,Model Filter
 *   08-May-2018, Sumit Maurya, #CC21, UserID passed to get saleschannel data according to userid for  Manage Retailer interface (Done for Motorola).
 *   *   14-May-2018, Rajnish Kumar, #CC22, UserID passed to get saleschannel data according to rsm asm Login.
 *    *   *   18-May-2018, Rajnish Kumar, #CC23, Get Sales Channel Type For Sales Channel State Mapping.
 *   18-May-2018, Rajnish Kumar, #CC24, Get Sales Channel Mapping Data for Sales Channel Type wise.
 *   31-May-2018, Rajnish Kumar, #CC25, Get Reference code Saleschannel and Product Category Mapping and Save Sales Channel ProductCategoty Mapping.
   *   05-June-2018, Rajnish Kumar, #CC26, Get SaleschannelType Based on Sales Type.
   *   14-June-2018, Rajnish Kumar, #CC27, Sales Return back Days.
   *   18-June-2018, Rajnish Kumar, #CC28, Search Parent Retailer according to ASM RSM Login.
 *   25-June-2018, Rajnish Kumar, #CC29, Bind SalesChannel based on saleschanneltype.
 *   25-June-2018, Balram Jha, #CC30, Sales channel type will include retailer based on parameter
 *   24-July-2018, Rakesh Raj, #CC31, GetSalesChannelType for Mobile App API
 * 27-Jul-2018 , Sumit Maurya, #CC32, New porperties added and provided to get Saleschannel data according to provided data accordingly (Done for Karbonn).
 * 31-Jul-2018,Vijay Kumar Prajapati,#CC33,New Parameter Added in Method.
 * 10-Sep-2018,Vijay Kumar Prajapati,#CC34,New Paramater added for return saleschannel code.
 * 15-Nov-2018, Sumit Maurya, #CC35, New method created to orgn saleschannel orgn hierarchy data (Done for Karbonn).
 * 28-Nov-2018,Vijay Kumar Prajapati,#CC36,New method created for bind salechannel.
 * 26-Dec-2018, Sumit Maurya, #CC37, Retailer ID provided (Done for Motorola).
 * 14-April-2020,Vijay Kumar Prajapati,#CC38,Add CompanyId (Done For Zedsales2.0)
 * 22-April-2020,Balram Jha,#CC39,Add CompanyId (Done For Zedsales2.0)
 * 14-sept-2021, Vaibhav Pandey,#CC40,new method created for bind generic BeatPlan
 * 19-Oct-2022, Adnan Mubeen, #CC41, Added CompanyID in function InsertOpeningStockWithZero().
 */

public class SalesChannelData : IDisposable
{
    #region Private Properties

    private string strErrorDetailXML;
    private Int32 intSalesChannelID, intNumberOfBackDaysSC, intUserID, intOrgnhierarchyID, intGroupParentSalesChannelID, intParentSalesChannelID, intBrand, intProducteCategoryid, intSalesChannelTo;
    private Int16 intCityID, intAreaID, intStateID, intSalesChannelTypeID, intType, intcountryid, intMappingTypeId;
    private string strSalesChannelName, strPrimarySales2DetailXML;
    private string strSalesChannelCode;
    private string strLoginname;
    private string strPassword;
    private string strAddress1;
    private string strAddress2;
    private string strMobile;
    private string strPinCode;
    private string strPhone, strPasswordSalt;
    private string strEmail;
    private string strFax;
    private string strTinNumber;
    private string strPanNumber;
    private string strCstNumber;
    private string strContactPerson;
    private bool blnIsOpeningStockEntered;
    private DateTime dtBusinessStartDate;
    private DateTime? dtOpeningStockDate;
    private DateTime? dtDoa;
    private DateTime? dtDob;
    private bool blnStatus, blnBillToRetailer, blnShowDetail, blnBranding;
    private Int32 intPasswordExpiryDays;
    private string strError;
    private string _SuccessMsg;/*#CC34 Added*/
    private string _xmlList;
    private EnumData.eSalesChannelLevel eSalesChanneLevel;
    private EnumData.eSalesChannelType eSalesChanneType;
    private EnumData.eControlRequestTypeForEntry eReqType;
    private EnumData.eSearchConditions eSearchType;
    private Boolean blnIsBrandwise;
    private Boolean blnIsHierarchylevel;
    private Int16 intHierarchyLevelID;
    private Int16 _GetSelectedTextId = 0;/*#CC02 ADDED*/
    private Int16 intSearchType;
    private Int32 intNumberOfBackDaysSCSaleReturns;
    /* #CC09 Add Start */
    public int MappingTypeID
    {
        get;
        set;
    }

    public int intOutParam
    {
        get;
        set;
    }
    /* #CC09 Add End */
    /* #CC10 Add Start */
    public int GetND
    {
        get;
        set;
    }
    /* #CC10 Add Start */
    #endregion
    #region Public Properties
    public string CompanyImageFolder { get; set; }
    public Int16 GetSelectedTextId
    {
        get { return _GetSelectedTextId; }
        set { _GetSelectedTextId = value; }
    }/*#CC02 ADDED*/


    public Int16 BaseEntityTypeID
    {
        get;
        set;
    }
    public Int16 GetParentOrChild
    {
        get;
        set;
    }
    public string OrderNumber
    {
        get;
        set;
    }
    public Int32 OrderID
    {
        get;
        set;
    }
    public Int32 StatusValue
    {
        get;
        set;
    }
    public Int16 Flag   //Pankaj Dhingra  POC For FirstData
    {
        get;
        set;
    }
    public string ErrorDetailXML
    {
        get { return strErrorDetailXML; }
        set { strErrorDetailXML = value; }
    }
    public Int32 LoggedInSalesChannelID   //Pankaj Dhingra  POC For FirstData
    {
        get;
        set;
    }
    public DateTime? OrderFromDate
    {
        get;
        set;
    }
    public DateTime? OrderToDate
    {
        get;
        set;
    }
    public Int32 Brand
    {
        get { return intBrand; }
        set { intBrand = value; }
    }
    public Int32 ProductCategoryId
    {
        get { return intProducteCategoryid; }
        set { intProducteCategoryid = value; }
    }

    public Int16 HierarchyLevelID
    {
        get { return intHierarchyLevelID; }
        set { intHierarchyLevelID = value; }
    }
    public EnumData.eControlRequestTypeForEntry ReqType
    {
        get { return eReqType; }
        set { eReqType = value; }
    }
    public EnumData.eSalesChannelLevel SalesChanneLevel
    {
        get { return eSalesChanneLevel; }
        set { eSalesChanneLevel = value; }
    }
    public EnumData.eSalesChannelType SalesChanneType
    {
        get { return eSalesChanneType; }
        set { eSalesChanneType = value; }
    }
    public EnumData.eSearchConditions SearchType
    {
        get { return eSearchType; }
        set { eSearchType = value; }
    }
    public string XMLList
    {
        get
        {
            return _xmlList;
        }
        set
        {
            _xmlList = value;
        }
    }
    public string SalesChannelTypeName
    {
        get { return saleschanneltypename; }
        set { saleschanneltypename = value; }
    }
    public Int32 PasswordExpiryDays
    {
        get { return intPasswordExpiryDays; }
        set { intPasswordExpiryDays = value; }
    }

    public string SalesChannelGroupName
    {
        get { return saleschannelgroupname; }
        set { saleschannelgroupname = value; }
    }

    public Int32 SalesChannelLevel
    {
        get { return saleschannellevel; }
        set { saleschannellevel = value; }
    }

    public Int32 BackDaysNumber
    {
        get { return backdaysnumber; }
        set { backdaysnumber = value; }
    }

    public Int32 SalesChannelID
    {
        get { return intSalesChannelID; }
        set { intSalesChannelID = value; }
    }
    public Int32 NumberofBackDaysSC
    {
        get { return intNumberOfBackDaysSC; }
        set { intNumberOfBackDaysSC = value; }
    }
    public Int32 NumberofBackDaysSCSaleReturns
    {
        get { return intNumberOfBackDaysSCSaleReturns; }
        set { intNumberOfBackDaysSCSaleReturns = value; }
    }
    public Int32 ToSalesChannelID
    {
        get { return intSalesChannelTo; }
        set { intSalesChannelTo = value; }
    }

    public Int32 UserID
    {
        get { return intUserID; }
        set { intUserID = value; }
    }
    public Int32 GetRetailerType
    {
        get;
        set;
    }

    public Int16 CountryID
    {
        get { return intcountryid; }
        set { intcountryid = value; }
    }



    public Int16 AreaID
    {
        get { return intAreaID; }
        set { intAreaID = value; }
    }
    public Int16 Type
    {
        get { return intType; }
        set { intType = value; }
    }
    public string PrimarySales2DetailXML
    {
        get { return strPrimarySales2DetailXML; }
        set { strPrimarySales2DetailXML = value; }
    }

    public Int16 CityID
    {
        get { return intCityID; }
        set { intCityID = value; }
    }

    public Int16 StateID
    {
        get { return intStateID; }
        set { intStateID = value; }
    }
    public Int32 OrgnhierarchyID
    {
        get { return intOrgnhierarchyID; }
        set { intOrgnhierarchyID = value; }
    }

    public Int32 GroupParentSalesChannelID
    {
        get { return intGroupParentSalesChannelID; }
        set { intGroupParentSalesChannelID = value; }
    }

    public Int32 ParentSalesChannelID
    {
        get { return intParentSalesChannelID; }
        set { intParentSalesChannelID = value; }
    }
    public Int16 SalesChannelTypeID
    {
        get { return intSalesChannelTypeID; }
        set { intSalesChannelTypeID = value; }
    }
    /*#CC14 Added Started*/
    public Int16 SalesChannelMappingTypeID
    {
        get { return intMappingTypeId; }
        set { intMappingTypeId = value; }
    }
    /*#CC14 Added End*/
    public string SalesChannelName
    {
        get { return strSalesChannelName; }
        set { strSalesChannelName = value; }
    }
    public string SalesChannelCode
    {
        get { return strSalesChannelCode; }
        set { strSalesChannelCode = value; }
    }

    public string Error
    {
        get { return strError; }
        set { strError = value; }
    }
    /*#CC34 Added Started*/
    public string SuccessMsg
    {
        get { return _SuccessMsg; }
        set { _SuccessMsg = value; }
    }
    /*#CC34 Added End*/
    public string Address1
    {
        get { return strAddress1; }
        set { strAddress1 = value; }
    }
    public string ContactPerson
    {
        get { return strContactPerson; }
        set { strContactPerson = value; }
    }
    public string Loginname
    {
        get { return strLoginname; }
        set { strLoginname = value; }
    }
    public string Password
    {
        get { return strPassword; }
        set { strPassword = value; }
    }


    public string Address2
    {
        get { return strAddress2; }
        set { strAddress2 = value; }
    }


    public string Mobile
    {
        get { return strMobile; }
        set { strMobile = value; }
    }


    public string PinCode
    {
        get { return strPinCode; }
        set { strPinCode = value; }
    }


    public string Phone
    {
        get { return strPhone; }
        set { strPhone = value; }
    }


    public string Email
    {
        get { return strEmail; }
        set { strEmail = value; }
    }



    public string Fax
    {
        get { return strFax; }
        set { strFax = value; }
    }


    public string TinNumber
    {
        get { return strTinNumber; }
        set { strTinNumber = value; }
    }


    public string CstNumber
    {
        get { return strCstNumber; }
        set { strCstNumber = value; }
    }

    public string PanNumber
    {
        get { return strPanNumber; }
        set { strPanNumber = value; }
    }
    public string PasswordSalt
    {
        get { return strPasswordSalt; }
        set { strPasswordSalt = value; }
    }
    public bool IsOpeningStockEntered
    {
        get { return blnIsOpeningStockEntered; }
        set { blnIsOpeningStockEntered = value; }
    }
    public bool Status
    {
        get { return blnStatus; }
        set { blnStatus = value; }
    }
    public bool BilltoRetailer
    {
        get { return blnBillToRetailer; }
        set { blnBillToRetailer = value; }
    }
    public DateTime BusinessStartDate
    {
        get { return dtBusinessStartDate; }
        set { dtBusinessStartDate = value; }
    }
    public DateTime? DateOfBirth
    {
        get { return dtDob; }
        set { dtDob = value; }
    }
    public DateTime? DateOfAnniversary
    {
        get { return dtDoa; }
        set { dtDoa = value; }
    }
    public DateTime? OpeningStockDate       //Convert This Property into the Nullable property
    {
        get { return dtOpeningStockDate; }
        set { dtOpeningStockDate = value; }
    }
    public Boolean IsBrandwise
    {
        get { return blnIsBrandwise; }
        set { blnIsBrandwise = value; }
    }
    public Boolean IsHierarchylevel
    {
        get { return blnIsHierarchylevel; }
        set { blnIsHierarchylevel = value; }
    }
    public Int16 isHOZSM
    {
        get { return ishozsm; }
        set { ishozsm = value; }
    }

    public Int16 isReport
    {
        get { return isreport; }
        set { isreport = value; }
    }


    public bool BlnShowDetail
    {
        get { return blnShowDetail; }
        set { blnShowDetail = value; }
    }
    public bool ShowBranding
    {
        get { return blnBranding; }
        set { blnBranding = value; }
    }
    private int _intStatus;
    public int intStatus
    {
        get { return _intStatus; }
        set { _intStatus = value; }
    }
    private int _SalesChannelApproval = -1;
    public int SalesChannelApproval
    {
        get { return _SalesChannelApproval; }
        set { _SalesChannelApproval = value; }
    }
    public bool ShowProductCategory//just for future reference it will not be used
    {
        get;
        set;
    }

    public string salesChannelTypeName
    {
        get;
        set;
    }

    public string SalesChannelTypegroupName
    {
        get;
        set;
    }

    public int ParentSalesChannelTypeId
    {
        get;
        set;
    }
    public bool BillToretailer
    {
        get;
        set;
    }
    public bool IsAutoGenerate
    {
        get;
        set;
    }

    public bool IsPTOAllowed
    {
        get;
        set;
    }
    public EnumData.RoleType RoleType
    {
        get;
        set;
    }
    public string RetailerName
    {
        get;
        set;
    }
    public string RetailerCode
    {
        get;
        set;
    }
    public Int16 ActiveStatus
    {
        get;
        set;
    }
    public Int16 StockStatusID
    {
        get;
        set;
    }

    public string SkuName
    {
        get;
        set;
    }
    public string SkuCode
    {
        get;
        set;
    }
    public Int32 SkuCodeDownload
    {
        get;
        set;
    }
    public Int32 StockBinTypeMasterID
    {
        get;
        set;
    }
    public string SerialNumber
    {
        get;
        set;
    }
    public Int16 Condition
    {
        get;
        set;
    }
    public Int32 PageIndex
    {
        get;
        set;
    }
    public Int32 PageSize
    {
        get;
        set;
    }
    public Int32 TotalRecords
    {
        get;
        set;
    }
    public Int32 SkuID
    {
        get;
        set;
    }
    public Int32 ComingFrom
    {
        get;
        set;
    }
    private string _StrImg;
    public string StrImg
    {
        get { return _StrImg; }
        set { _StrImg = value; }
    }
    private string _StrPassword;
    public string StrPassword
    {
        get { return _StrPassword; }
        set { _StrPassword = value; }
    }
    private string _Siteurl;
    public string Siteurl
    {
        get { return _Siteurl; }
        set { _Siteurl = value; }
    }
    private string _WebSite;
    public string WebSite
    {
        get { return _WebSite; }
        set { _WebSite = value; }
    }
    /*#CC01 START ADDED*/
    public string InvoiceNo
    {
        get;
        set;
    }

    public string FileURL
    {
        get;
        set;
    }

    public DateTime Fromdate
    {
        get;
        set;
    }

    public DateTime Todate
    {
        get;
        set;
    }
    public string ApprovalRemarks
    {
        get;
        set;
    }
    /*#CC01 START END*/
    /* #CC05 Add Start */
    public int SalesChannelStateMappingID
    {
        get;
        set;
    }
    /* #CC05 Add End */

    /* #CC06 Add Start */
    public string MobileNo2
    {
        get;
        set;
    }
    /* #CC06 Add End */

    /* #CC07 Add Start */
    public string SessionID
    {
        get;
        set;
    }
    public int OutstandingDetailDownload
    {
        get;
        set;
    }
    /* #CC07 Add End */

    public int NDID
    {
        get;
        set;
    }
    public int RDSID
    {
        get;
        set;
    }

    public DateTime? FromDate
    {
        get;
        set;
    }
    public DateTime? ToDate
    {
        get;
        set;
    }

    /* #CC12 Add End */

    /* #CC13 Add Start */
    public int BindChild
    {
        get;
        set;
    }
    /* #CC13 Add End */
    /* #CC14 Add Start Rajnish */
    public int EntityMappingTypeId
    {
        get;
        set;
    }
    public int EntityMappingRelationId
    {
        get;
        set;
    }
    public Int16 LoadRetailer//#CC30 added
    {
        get;
        set;
    }
    public int RejectionFlag
    {
        get;
        set;
    }
    private int _SalesChannelRDSApproval = 0;
    public int SalesChannelRDSApproval
    {
        get { return _SalesChannelRDSApproval; }
        set { _SalesChannelRDSApproval = value; }
    }
    /* #CC14 Add End Rajnish */
    Int32 backdaysnumberForSaleReturn;
    public Int32 BackDaysNumberSaleReturn
    {
        get { return backdaysnumberForSaleReturn; }
        set { backdaysnumberForSaleReturn = value; }
    }
    /*#CC36 Added*/
    private Int32 selectedFOSTSM;
    public string ErrorMessage { get; set; }
    private DateTime todate; private DateTime fromdate;
    public Int32 SelectedFOSTSM
    {
        get { return selectedFOSTSM; }
        set { selectedFOSTSM = value; }
    }
    public int FosTsmName
    {
        get;
        set;
    }
    public DateTime ToDate1
    {
        get { return todate; }
        set { todate = value; }

    }

    public DateTime FromDate1
    {

        get { return fromdate; }
        set { fromdate = value; }
    }
    public Int32 UserIdapi
    {

        get;
        set;
    }
    /*#CC36 Added End*/
    #endregion

    #region Class Variables
    Int16 ishozsm;
    Int16 isreport;
    DataTable dtResult;
    SqlParameter[] SqlParam;
    MySqlParameter[] MySqlParam;
    Int32 IntResultCount = 0;
    DataSet dsResult;
    Int32 saleschannellevel;
    Int32 backdaysnumber;

    string saleschannelgroupname;
    string saleschanneltypename;
    #endregion

    /* #CC20*/
    public Int32 ModelId
    {
        get;
        set;
    }
    public int SaleTypeID
    {
        get;
        set;
    }

    public int DistrictID
    {
        get;
        set;
    }


    public Int32 CompanyId { get; set; }/*#CC38 Added Start*/
    public Int32 BrandId { get; set; }
    public Int64 AppVisitId { get; set; }
    #region salesChannelType

    public DataTable GetSalesChannelTypeDetails()
    {
        try
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelTypeDetails", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetSalesChannelList()
    {
        try
        {
            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@ActiveStatus", ActiveStatus);
            SqlParam[2] = new SqlParameter("@ComingFrom", ComingFrom);// when it will be zero then old functionality else will work in the webservice
            SqlParam[3] = new SqlParameter("@SalesChannelCode", SalesChannelCode);
            SqlParam[4] = new SqlParameter("@GetSelectedTextId", GetSelectedTextId); /*#CC02 ADDED*/
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelList", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    /* #CC17 Add Start */
    public DataSet GetSalesChannelListDS()
    {
        try
        {
            SqlParam = new SqlParameter[7];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@ActiveStatus", ActiveStatus);
            SqlParam[2] = new SqlParameter("@ComingFrom", ComingFrom);
            SqlParam[3] = new SqlParameter("@SalesChannelCode", SalesChannelCode);
            SqlParam[4] = new SqlParameter("@GetSelectedTextId", GetSelectedTextId);
            SqlParam[5] = new SqlParameter("@IsSkuRequired", 1);
            SqlParam[6] = new SqlParameter("@CompanyID", CompanyId);
            DataSet ds = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetSalesChannelList", CommandType.StoredProcedure, SqlParam);
            return ds;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    /* #CC17 Add End */

    public DataSet GetprcGetSalesChannelInformationList()
    {
        try
        {
            DataSet dsTwo = new DataSet();
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@ActiveStatus", ActiveStatus);
            dsTwo = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetSalesChannelInformationList", CommandType.StoredProcedure, SqlParam);
            return dsTwo;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetSalesChannelListWithRetailer()
    {
        try
        {
            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@ActiveStatus", ActiveStatus);
            SqlParam[2] = new SqlParameter("@OtherEntityTypeID", BaseEntityTypeID);
            SqlParam[3] = new SqlParameter("@UserID", UserID);
            SqlParam[4] = new SqlParameter("@loggedInChannelID", SqlDbType.Int);
            SqlParam[4].Direction = ParameterDirection.Output;
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelListWithRetailer", CommandType.StoredProcedure, SqlParam);
            if (Convert.ToString(SqlParam[4].Value) != "")
                LoggedInSalesChannelID = Convert.ToInt32(SqlParam[4].Value);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    /*#CC23 Start*/
    public DataTable GetSalesChannelTypeForStateMapping()
    {
        try
        {
            SqlParam = new SqlParameter[2];

            SqlParam[1] = new SqlParameter("@UserId", UserID);

            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("BindSaleschannelTypeForSaleschannelStateMapping", CommandType.StoredProcedure, SqlParam);


            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    /*#CC23 end*/
    public DataTable GetSalesChannelListWithRetailerV2()
    {
        try
        {
            SqlParam = new SqlParameter[7]; /* #CC10 length increased from 5-6*/
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@ActiveStatus", ActiveStatus);
            SqlParam[2] = new SqlParameter("@BaseEntityTypeID", BaseEntityTypeID);
            SqlParam[3] = new SqlParameter("@UserID", UserID);
            SqlParam[4] = new SqlParameter("@loggedInChannelID", SqlDbType.Int);
            SqlParam[4].Direction = ParameterDirection.Output;
            SqlParam[5] = new SqlParameter("@GetND", GetND); /* #CC10 Added */
            SqlParam[6] = new SqlParameter("@CompanyId", CompanyId);

            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelListWithRetailer", CommandType.StoredProcedure, SqlParam);
            if (Convert.ToString(SqlParam[4].Value) != "")
                LoggedInSalesChannelID = Convert.ToInt32(SqlParam[4].Value);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void InsertUpdateChannelSalesChannelType()
    {
        try
        {

            SqlParam = new SqlParameter[10];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@saleschanneltypename", salesChannelTypeName);
            SqlParam[2] = new SqlParameter("@hierarchyleveid", HierarchyLevelID);
            SqlParam[3] = new SqlParameter("@saleschanneltypegroupname", SalesChannelTypegroupName);
            SqlParam[4] = new SqlParameter("@parentsaleschanneltypeid", ParentSalesChannelTypeID);
            SqlParam[5] = new SqlParameter("@staus", Status);
            SqlParam[6] = new SqlParameter("@isautogenerate", IsAutoGenerate);
            SqlParam[7] = new SqlParameter("@billtoretailer", BilltoRetailer);
            SqlParam[8] = new SqlParameter("@isPTOallowed", IsPTOAllowed);
            SqlParam[9] = new SqlParameter("@outerror", SqlDbType.NVarChar, 200);
            SqlParam[9].Direction = ParameterDirection.Output;
            int r = DataAccess.DataAccess.Instance.DBInsertCommand("prcInsUpdSalesChannelType", SqlParam);
            if (Convert.ToString(SqlParam[9].Value) != "")
                strError = Convert.ToString(SqlParam[8].Value);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }



    #endregion


    #region GetSalesChannelType


    public int ParentHierarchyID
    {
        get;
        set;
    }

    public int ParentLocation
    {
        get;
        set;
    }

    //public int ParentSalesChannelID
    //{
    //    get;
    //    set;
    //}

    public int ParentSalesChannelTypeID
    {
        get;
        set;
    }

    public Int16 SearchType1
    {
        get { return intSearchType; }
        set { intSearchType = value; }
    }
    public DataTable GetSalesChannelInfoForControl()
    {
        try
        {
            SqlParam = new SqlParameter[4];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@saleschannelparentid", ParentSalesChannelID);
            SqlParam[2] = new SqlParameter("@orghierarchyid", OrgnhierarchyID);
            SqlParam[3] = new SqlParameter("@parentorghierarchyid", ParentLocation);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelControlDetails", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }


    public DataTable GetSalesChannelForRetailers()
    {
        dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetRetailerParentInfo", CommandType.StoredProcedure);
        return dtResult;
    }



    public DataTable GetSalesChannelType()
    {
        try
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetCompleteSalesChannelType", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetSalesChannelTypeV2()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            SqlParam[0] = new SqlParameter("@UserId", UserID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetCompleteSalesChannelTypeV2", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetSalesChannelTypeForReport()
    {
        try
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@UserId", UserID);
            SqlParam[2] = new SqlParameter("@GetRetailerType", GetRetailerType);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetCompleteSalesChannelTypeForReport", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetSalesChannelForStockAdjustment()
    {
        try
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetCompleteSalesChannelStockAdjustment", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetSalesChannelTypeV3()
    {
        try
        {
            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@UserId", UserID);
            SqlParam[2] = new SqlParameter("@ForApproval", SalesChannelRDSApproval);
            SqlParam[3] = new SqlParameter("@LoadRetailer", LoadRetailer);
            SqlParam[4] = new SqlParameter("@CompanyId", CompanyId);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetCompleteSalesChannelTypeV3", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetSalesChannelTypeAndBaseEntityType()
    {
        try
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@UserId", UserID);
            SqlParam[2] = new SqlParameter("@CompanyId", CompanyId);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetCompleteSalesChannelTypeV4", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetSalesChannelTypeV4()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@UserId", UserID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetCompleteSalesChannelTypeApproveSalesChannel", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetSalesChannelTypeV5()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@UserId", UserID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetCompleteSalesChannelTypeForApproval", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetSalesChannelTypeSelfandChild()  //26-Mar-14, Rakesh Goel - new method created to get self and child channel type
    {
        try
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetCompleteSalesChannelTypeSelfandChild", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable FetchSalesChannelTypeSB()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@OtherEntityTypeID", BaseEntityTypeID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcFetchSalesChannelTypeSB", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }


    public DataTable GetSalesChannelTypeForBrand()
    {
        try
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelTypeForBrand", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion


    public DataTable GetDistributerInfo()
    {
        try
        {
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetDistributerInfo", CommandType.StoredProcedure);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetParentForRetailer()
    {
        try
        {
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetRetailerParentInfo", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet GetParentForRetailerTransfer()
    {
        try
        {
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("[prcGetRetailerTransferParentInfo]", CommandType.StoredProcedure, SqlParam);
            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetSalesChannelTypeFromUser()
    {
        try
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", intSalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@ishozsm", ishozsm);
            SqlParam[2] = new SqlParameter("@isreport", isreport);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelTypeByUserInfo", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    #region GetSalesChannelParent
    /* #CC22*/
    public DataTable GetSalesChannelParent()
    {
        try
        {
            SqlParam = new SqlParameter[4];
            SqlParam[0] = new SqlParameter("@saleschannelid", SalesChannelID);
            SqlParam[1] = new SqlParameter("@saleschanneltype", SalesChannelTypeID);
            SqlParam[2] = new SqlParameter("@UserId", UserID);
            SqlParam[3] = new SqlParameter("@CompanyId", CompanyId);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("[prcGetAllParentSalesChannel]", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    //Pankaj Dhingra for Poc would be used at the SaleschannelLedger page
    public DataTable GetSalesChannelBasedOnType()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@SalesChannelID", SalesChannelID);
            SqlParam[1] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGeSalesChannelBasedOnTypeID", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion
    public DataTable GetParentSalesChannelInfo()
    {
        try
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@SalesChannelId", intSalesChannelID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelParentInfo", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    #region Procedures for DownLoad Data
    public DataSet GetAllTemplateData()
    {
        try
        {
            string TargetName = "";
            MySqlParam = new MySqlParameter[8];
            MySqlParam[0] = new MySqlParameter("@p_UserID", UserID);
            MySqlParam[1] = new MySqlParameter("@p_ReqType", eReqType);
            MySqlParam[2] = new MySqlParameter("@p_SecondarySalesChannelID", SalesChannelID);
            MySqlParam[3] = new MySqlParameter("@p_SalesChanneLevel", eSalesChanneLevel);
            MySqlParam[4] = new MySqlParameter("@p_BrandID", Brand);
            MySqlParam[5] = new MySqlParameter("@p_TargetName", TargetName);
            MySqlParam[6] = new MySqlParameter("@p_debugmode", 0);
            MySqlParam[7] = new MySqlParameter("@p_exportpricetype", 0);
            //SqlParam[5] = new SqlParameter("@IsBrandwise", IsBrandwise);
            //SqlParam[6] = new SqlParameter("@IsHierarchylevel", IsHierarchylevel);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcGetAllTemplateData", CommandType.StoredProcedure, MySqlParam);
            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    /*#CC08 START ADDED*/
    public DataSet GetBinCode()
    {
        try
        {
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetStockBinTypeInfo", CommandType.StoredProcedure);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /*#CC08 END ADDED*/
    public DataSet GetAllTemplateDataMicromax()
    {
        try
        {
            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@UserID", UserID);
            SqlParam[1] = new SqlParameter("@ReqType", eReqType);
            SqlParam[2] = new SqlParameter("@SecondarySalesChannelID", SalesChannelID);
            SqlParam[3] = new SqlParameter("@SalesChanneLevel", eSalesChanneLevel);
            SqlParam[4] = new SqlParameter("@BrandID", Brand);
            //SqlParam[5] = new SqlParameter("@IsBrandwise", IsBrandwise);
            //SqlParam[6] = new SqlParameter("@IsHierarchylevel", IsHierarchylevel);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetAllTemplateDataMicromax", CommandType.StoredProcedure, SqlParam);
            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }


    public DataSet GetSchemeTemplateData()
    {
        try
        {
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("[prcGetSchemeTemplateIbfo]", CommandType.StoredProcedure);
            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }



    public DataSet GetSecondaryTemplate()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@SalesChannelID", SalesChannelID);
            SqlParam[1] = new SqlParameter("@brandID", Brand);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetSecondaryTemplate", CommandType.StoredProcedure, SqlParam);
            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet GetAllTemplateDataWithType()
    {
        try
        {
            SqlParam = new SqlParameter[4];
            SqlParam[0] = new SqlParameter("@UserID", UserID);
            SqlParam[1] = new SqlParameter("@ReqType", eReqType);
            SqlParam[2] = new SqlParameter("@SecondarySalesChannelID", SalesChannelID);
            SqlParam[3] = new SqlParameter("@SalesChanneLevel", eSalesChanneLevel);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetAllTemplateDataWithType", CommandType.StoredProcedure, SqlParam);
            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion
    public DataTable GetSalesChannelOrghierarchy()
    {
        try
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@SalesChannelTypeID", intSalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@ParentSalesChannelID", intParentSalesChannelID);
            SqlParam[2] = new SqlParameter("@CompanyId", CompanyId);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetOrghierarchyBySalesType", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetSalesChannelOrghierarchyRetailer()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@SalesChannelID", SalesChannelID);
            SqlParam[1] = new SqlParameter("@LoggedInSalesChannelID", LoggedInSalesChannelID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetOrghierarchyRetailerCreation", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetSalesChannel()
    {
        try
        {

            SqlParam = new SqlParameter[4];
            SqlParam[0] = new SqlParameter("@SalesChannelLevel", saleschannellevel);
            SqlParam[1] = new SqlParameter("@BackDaysNumber", backdaysnumber);
            SqlParam[2] = new SqlParameter("@SalesChannelTypeName", saleschanneltypename);
            SqlParam[3] = new SqlParameter("@SalesChannelTypeID", intSalesChannelTypeID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelInfo", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetSalesChannelTypeName()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@SalesChanneLevel", saleschannellevel);
            SqlParam[1] = new SqlParameter("@SalesChannelTypeID", intSalesChannelTypeID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelTypeName", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void UpdateSalesChannel()
    {
        try
        {

            SqlParam = new SqlParameter[4];

            SqlParam[0] = new SqlParameter("@BackDaysNumber", backdaysnumber);
            SqlParam[1] = new SqlParameter("@SalesChannelTypeID", intSalesChannelTypeID);
            SqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@BackDaysNumberSaleReturn", BackDaysNumberSaleReturn);/*#CC27*/
            int r = DataAccess.DataAccess.Instance.DBInsertCommand("prcUpdateSalesChannelType", SqlParam);
            strError = Convert.ToString(SqlParam[2].Value);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #region GetSalesChannelParentFor Group
    public DataTable GetSalesChannelParentForGroup()
    {
        try
        {
            SqlParam = new SqlParameter[8];/* Size increase to from 5 to 6 and search Parent Retailer according to asm and rsm login*/ /* #CC37 length increased. */
            SqlParam[0] = new SqlParameter("@SalesChanneType", eSalesChanneType);
            SqlParam[1] = new SqlParameter("@RoleType", RoleType);
            SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
            SqlParam[3] = new SqlParameter("@RetailerCode", RetailerCode);
            SqlParam[4] = new SqlParameter("@RetailerName", RetailerName);
            SqlParam[5] = new SqlParameter("@UserId", UserID);
            SqlParam[6] = new SqlParameter("@RetailerID", RetailerID); /* #CC37 Added */
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetParentForSalesChannelForGroup", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion
    #region Insert Update sales Channel
    public Int32 InsertSalesChannelOpeningStock()
    {
        try
        {
            SqlParam = new SqlParameter[4];
            SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesChannelID);
            SqlParam[1] = new SqlParameter("@XMLList", SqlDbType.Xml);
            SqlParam[1].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(XMLList, XmlNodeType.Document, null));
            SqlParam[2] = new SqlParameter("@OpeningStockDate", dtOpeningStockDate);
            SqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[3].Direction = ParameterDirection.Output;
            DataAccess.DataAccess.Instance.DBInsertCommand("prcInsertSalesChannelOpeningStock", SqlParam);
            IntResultCount = Convert.ToInt32(SqlParam[3].Value);
            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public Int32 InsertSalesChannelBrandMapping()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@XMLList", SqlDbType.Xml);
            SqlParam[0].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(XMLList, XmlNodeType.Document, null));
            SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[1].Direction = ParameterDirection.Output;
            DataAccess.DataAccess.Instance.DBInsertCommand("prcInsertUpdateSalesChannelBrandMapping", SqlParam);
            IntResultCount = Convert.ToInt32(SqlParam[1].Value);
            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public Int32 InsertSalesChannelProductCategoryMapping()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@XMLList", SqlDbType.Xml);
            SqlParam[0].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(XMLList, XmlNodeType.Document, null));
            SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[1].Direction = ParameterDirection.Output;
            DataAccess.DataAccess.Instance.DBInsertCommand("prcInsertUpdateSalesChannelProductCategoryMapping", SqlParam);
            IntResultCount = Convert.ToInt32(SqlParam[1].Value);
            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public DataTable GetSalesChannelOpeningStockInfo()
    {
        try
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@SalesChannelID", SalesChannelID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelOpeningStockInfo", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public Int32 InsertUpdateSalesChannel()         //Pankaj Dhingra  08-Feb-2013
    {
        try
        {
            SqlParam = new SqlParameter[38];
            SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesChannelID);
            SqlParam[0].Direction = ParameterDirection.InputOutput;
            SqlParam[1] = new SqlParameter("@SalesChannelCode", strSalesChannelCode);
            SqlParam[2] = new SqlParameter("@SalesChannelName", strSalesChannelName);
            SqlParam[3] = new SqlParameter("@Address1", strAddress1);
            SqlParam[4] = new SqlParameter("@Address2", strAddress2);
            SqlParam[5] = new SqlParameter("@CstNumber", strCstNumber);
            SqlParam[6] = new SqlParameter("@Email", strEmail);
            SqlParam[7] = new SqlParameter("@Fax", strFax);
            SqlParam[8] = new SqlParameter("@Mobile", strMobile);
            SqlParam[9] = new SqlParameter("@Phone", strPhone);
            SqlParam[10] = new SqlParameter("@PinCode", strPinCode);
            SqlParam[11] = new SqlParameter("@TinNumber", strTinNumber);
            SqlParam[12] = new SqlParameter("@ContactPerson", strContactPerson);
            SqlParam[13] = new SqlParameter("@SalesChannelTypeID", intSalesChannelTypeID);
            SqlParam[14] = new SqlParameter("@StateID", intStateID);
            SqlParam[15] = new SqlParameter("@CityID", intCityID);
            SqlParam[16] = new SqlParameter("@AreaID", intAreaID);
            SqlParam[17] = new SqlParameter("@OrgnhierarchyID", intOrgnhierarchyID);
            SqlParam[18] = new SqlParameter("@ParentSalesChannelID", intParentSalesChannelID);
            SqlParam[19] = new SqlParameter("@GroupParentSalesChannelID", intGroupParentSalesChannelID);
            SqlParam[20] = new SqlParameter("@Loginname", strLoginname);
            SqlParam[21] = new SqlParameter("@Password", strPassword);
            SqlParam[22] = new SqlParameter("@PasswordSalt", strPasswordSalt);
            SqlParam[23] = new SqlParameter("@Status", @Status);
            SqlParam[24] = new SqlParameter("@BuisnessStartDate", dtBusinessStartDate);
            SqlParam[25] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 200);
            SqlParam[25].Direction = ParameterDirection.Output;
            SqlParam[26] = new SqlParameter("@PanNumber", strPanNumber);
            SqlParam[27] = new SqlParameter("@DateOfBirth", dtDob);
            SqlParam[28] = new SqlParameter("@DateOfAnniversary", dtDoa);
            SqlParam[29] = new SqlParameter("@PasswordExpiryDays", intPasswordExpiryDays);
            SqlParam[30] = new SqlParameter("@strPassword", StrPassword);
            SqlParam[31] = new SqlParameter("@OpeningStockDate", OpeningStockDate);       //08-Feb-2013
            SqlParam[32] = new SqlParameter("@MobileNo2", MobileNo2); /* #CC06 Added */
            SqlParam[33] = new SqlParameter("@LoginUserId", UserID);/*#CC18 Added*/
            SqlParam[34] = new SqlParameter("@ApprovalRemarks", ApprovalRemarks);/*#CC18 Added*/
            SqlParam[35] = new SqlParameter("@RejectionFlag", RejectionFlag);/*#CC18 Added*/
            SqlParam[36] = new SqlParameter("@Out_SucessMessage", SqlDbType.VarChar, 200);/*#CC34 Added*/
            SqlParam[36].Direction = ParameterDirection.Output;
            SqlParam[37] = new SqlParameter("@CompanyID", CompanyId);
            DataAccess.DataAccess.Instance.DBInsertCommand("prcInsUpdSalesChannel", SqlParam);
            IntResultCount = Convert.ToInt32(SqlParam[0].Value);
            if (SqlParam[25].Value != DBNull.Value && SqlParam[25].Value.ToString() != "")
            {
                Error = (SqlParam[25].Value).ToString();
            }
            /*#CC34 Added Started*/
            if (SqlParam[36].Value != DBNull.Value && SqlParam[36].Value.ToString() != "")
            {
                SuccessMsg = (SqlParam[36].Value).ToString();
            }
            /*#CC34 Added Started*/
            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion
    #region Get Sales Channel Info by Parameters


    public DataTable GetSalesChannelInfoForStockists()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesChannelID);
            SqlParam[1] = new SqlParameter("@SalesChannelTypeID", intSalesChannelTypeID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelForStockists", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }





    public DataTable GetSalesChannelInfo()
    {
        try
        {
            SqlParam = new SqlParameter[17]; /* #CC02 Length increased from 9 to 10 */ /* #CC21 Length increased from 10 to 11 */ /* #CC32 Length increased from 11 to 14 */
            SqlParam[0] = new SqlParameter("@SalesChannelId", intSalesChannelID);
            SqlParam[1] = new SqlParameter("@SalesChannelTypeID", intSalesChannelTypeID);
            SqlParam[2] = new SqlParameter("@SalesChannelName", strSalesChannelName);
            SqlParam[3] = new SqlParameter("@SalesChannelCode", strSalesChannelCode);
            SqlParam[4] = new SqlParameter("@BillToRetailer", BilltoRetailer);
            SqlParam[5] = new SqlParameter("@ShowDetail", blnShowDetail);
            SqlParam[6] = new SqlParameter("@SearchType", eSearchType);
            // SqlParam[7] = new SqlParameter("@BrandId", Brand);
            SqlParam[8] = new SqlParameter("@Status", StatusValue);
            SqlParam[9] = new SqlParameter("@BindChild", BindChild); /* #CC02 Added */
            SqlParam[10] = new SqlParameter("@UserID", UserID); /* #CC21 Added */
            /* #CC32 Add Start */
            SqlParam[11] = new SqlParameter("@PageIndex", PageIndex);
            SqlParam[12] = new SqlParameter("@PageSize", PageSize);
            SqlParam[13] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 10);
            SqlParam[13].Direction = ParameterDirection.Output;
            SqlParam[14] = new SqlParameter("@CompanyId", CompanyId);
            SqlParam[15] = new SqlParameter("@BrandId", BrandId);
            SqlParam[16] = new SqlParameter("@ProductCategoryId", ProductCategoryId);
            /* #CC32 Add End */
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelInfoByParameters", CommandType.StoredProcedure, SqlParam);

            TotalRecords = Convert.ToInt32(SqlParam[13].Value);  /*#CC32 Added */
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetParentSalesChannel()
    {
        try
        {
            SqlParam = new SqlParameter[6];
            SqlParam[0] = new SqlParameter("@SalesChannelId", SalesChannelID);
            SqlParam[1] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
            SqlParam[2] = new SqlParameter("@UserID", UserID);
            SqlParam[3] = new SqlParameter("@UserBaseEntityTypeId", BaseEntityTypeID);
            SqlParam[4] = new SqlParameter("@GetParentOrChild", GetParentOrChild);
            SqlParam[5] = new SqlParameter("@RetailerId", RetailerID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetParentSaleChannel", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    /*#CC18 Added*/
    public DataSet GetRDSListForApproval()
    {
        try
        {
            SqlParam = new SqlParameter[10];
            SqlParam[0] = new SqlParameter("@SalesChannelId", LoggedInSalesChannelID);
            SqlParam[1] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
            SqlParam[2] = new SqlParameter("@SalesChannelName", SalesChannelName);
            SqlParam[3] = new SqlParameter("@SalesChannelCode", SalesChannelCode);
            SqlParam[4] = new SqlParameter("@SalesChannelApproval", SalesChannelApproval);
            SqlParam[5] = new SqlParameter("@intStatus", intStatus);
            SqlParam[6] = new SqlParameter("@PageIndex", PageIndex);
            SqlParam[7] = new SqlParameter("@PageSize", PageSize);
            SqlParam[8] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 10);
            SqlParam[8].Direction = ParameterDirection.Output;
            SqlParam[9] = new SqlParameter("@UserId", UserID);


            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetRDSListApproval", CommandType.StoredProcedure, SqlParam);
            TotalRecords = Convert.ToInt32(SqlParam[8].Value);
            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetSalesChannelforSalesMan()
    {
        try
        {
            SqlParam = new SqlParameter[1]; /* #CC02 Length increased from 9 to 10 */

            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelInfoforSalesMan", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetChildSalesChannel()
    {
        try
        {
            SqlParam = new SqlParameter[10]; /* #CC02 Length increased from 9 to 10 */
            SqlParam[0] = new SqlParameter("@SalesChannelId", intSalesChannelID);
            SqlParam[1] = new SqlParameter("@SalesChannelTypeID", intSalesChannelTypeID);
            SqlParam[2] = new SqlParameter("@SalesChannelName", strSalesChannelName);
            SqlParam[3] = new SqlParameter("@SalesChannelCode", strSalesChannelCode);
            SqlParam[4] = new SqlParameter("@BillToRetailer", BilltoRetailer);
            SqlParam[5] = new SqlParameter("@ShowDetail", blnShowDetail);
            SqlParam[6] = new SqlParameter("@SearchType", eSearchType);
            SqlParam[7] = new SqlParameter("@BrandId", Brand);
            SqlParam[8] = new SqlParameter("@Status", StatusValue);
            SqlParam[9] = new SqlParameter("@BindChild", BindChild); /* #CC02 Added */
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetChildSalesChannel", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetSalesChannelInfoV2()
    {
        try
        {
            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@SalesChannelId", SalesChannelID);
            SqlParam[1] = new SqlParameter("@SalesChannelTypeID", intSalesChannelTypeID);
            SqlParam[2] = new SqlParameter("@SalesChannelName", strSalesChannelName);
            SqlParam[3] = new SqlParameter("@SalesChannelCode", strSalesChannelCode);
            SqlParam[4] = new SqlParameter("@BillToRetailer", BilltoRetailer);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelInfoByParametersV2", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }


    public DataTable GetSalesChannelInfoForBrand()
    {
        try
        {
            SqlParam = new SqlParameter[8];
            SqlParam[0] = new SqlParameter("@SalesChannelTypeID", intSalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@BrandId", intBrand);
            SqlParam[2] = new SqlParameter("@blnBranding", ShowBranding);
            SqlParam[3] = new SqlParameter("@SalesChannelID", SalesChannelID);
            SqlParam[4] = new SqlParameter("@StateID", StateID);
            SqlParam[5] = new SqlParameter("@CityID", CityID);
            SqlParam[6] = new SqlParameter("@SalesChannelName", SalesChannelName);
            SqlParam[7] = new SqlParameter("@SalesChannelCode", SalesChannelCode);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelInfoForBrand", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }


    public DataTable GetSalesChannelInfoForProductCategory()
    {
        try
        {
            SqlParam = new SqlParameter[8];
            SqlParam[0] = new SqlParameter("@SalesChannelTypeID", intSalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@ProductCategoryId", intProducteCategoryid);
            SqlParam[2] = new SqlParameter("@blnProductCategory", ShowProductCategory);
            SqlParam[3] = new SqlParameter("@SalesChannelID", SalesChannelID);
            SqlParam[4] = new SqlParameter("@StateID", StateID);
            SqlParam[5] = new SqlParameter("@CityID", CityID);
            SqlParam[6] = new SqlParameter("@SalesChannelName", SalesChannelName);
            SqlParam[7] = new SqlParameter("@SalesChannelCode", SalesChannelCode);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelInfoForProductCategory", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetSalesChannelChildInfo()
    {
        try
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@saleschannelid", intSalesChannelID);
            SqlParam[1] = new SqlParameter("@saleschannelname", strSalesChannelName);
            SqlParam[2] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelHierarchy", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetSalesChannelChildInfoV2()
    {
        try
        {
            SqlParam = new SqlParameter[4];
            SqlParam[0] = new SqlParameter("@saleschannelid", intSalesChannelID);
            SqlParam[1] = new SqlParameter("@saleschannelname", strSalesChannelName);
            SqlParam[2] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            SqlParam[3] = new SqlParameter("@UserID", UserID);/*#CC03 ADDED*/
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelHierarchyV2", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion
    #region UpdateStatus


    public Int32 UpdateStatusSalesChannelInfo()
    {
        try
        {

            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesChannelID);

            IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("PrcUpdStatusSalesChannel", SqlParam);
            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public Int32 UpdateNumberofBackdaysofSalesChannel()//Pankaj Kumar
    {

        try
        {

            SqlParam = new SqlParameter[4];/*#CC27*//*Increased 3 to 4 for salereturns */
            SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesChannelID);
            SqlParam[1] = new SqlParameter("@NumberofBackdaysSC", intNumberOfBackDaysSC);
            SqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.Int);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@NumberofBackdaysSCSaleReturns", intNumberOfBackDaysSCSaleReturns);/*#CC27*/
            IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("PrcUpdNumberofBackDaysofSalesChannel", SqlParam);
            return Convert.ToInt32(SqlParam[2].Value);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }


    #endregion
    #region Check SalesChannel Existance
    public Int32 CheckSalesChannelExistence()
    {
        try
        {
            int Result = 0;
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesChannelID);
            SqlParam[1] = new SqlParameter("@ResultOut", SqlDbType.Int);
            SqlParam[1].Direction = ParameterDirection.Output;

            IntResultCount = (DataAccess.DataAccess.Instance.DBInsertCommand("PrcChkSalesChannelExistence", SqlParam));
            Result = Convert.ToInt32(SqlParam[1].Value);
            return Result;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    public Int32 CheckSalesChannelBrandMapping()
    {
        try
        {
            int Result = 0;
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@SalesChannelTypeID", intSalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@ResultOut", SqlDbType.Int);
            SqlParam[1].Direction = ParameterDirection.Output;
            IntResultCount = (DataAccess.DataAccess.Instance.DBInsertCommand("PrcChkSalesChannelBrandMapping", SqlParam));
            Result = Convert.ToInt32(SqlParam[1].Value);
            return Result;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    #endregion
    #region Check Sales Channel Stock Existance
    public bool CheckStockEntryExistance()
    {
        bool Check = false;
        try
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesChannelID);
            Check = Convert.ToBoolean(DataAccess.DataAccess.Instance.getSingleValues("PrcChkStockEntryExistence", SqlParam));
            return Check;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    #endregion

    public DataTable GetSalesChannelOrderInfo()
    {
        SqlParam = new SqlParameter[7];
        SqlParam[0] = new SqlParameter("@SalesChannelId", intSalesChannelID);
        SqlParam[1] = new SqlParameter("@Flag", Flag);
        SqlParam[2] = new SqlParameter("@LoginSaleschannelID", LoggedInSalesChannelID);
        SqlParam[3] = new SqlParameter("@OrderFromDate", OrderFromDate);
        SqlParam[4] = new SqlParameter("@OrderToDate", OrderToDate);
        SqlParam[5] = new SqlParameter("@OrderID", OrderID);
        SqlParam[6] = new SqlParameter("@StatusValue", StatusValue);
        dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelOrderInfo", CommandType.StoredProcedure, SqlParam);
        return dtResult;
    }
    public DataTable GetSalesChannelOrderInfoV1()
    {
        SqlParam = new SqlParameter[7];
        SqlParam[0] = new SqlParameter("@RetailerCode", RetailerCode);
        SqlParam[1] = new SqlParameter("@Flag", Flag);
        SqlParam[2] = new SqlParameter("@LoginSaleschannelID", LoggedInSalesChannelID);
        SqlParam[3] = new SqlParameter("@OrderFromDate", OrderFromDate);
        SqlParam[4] = new SqlParameter("@OrderToDate", OrderToDate);
        SqlParam[5] = new SqlParameter("@OrderID", OrderID);
        SqlParam[6] = new SqlParameter("@SearchType", SearchType1);

        dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSecondaryOrderInfo", CommandType.StoredProcedure, SqlParam);
        return dtResult;
    }

    public DataTable GetSalesChannelOrderDetailInfo()
    {
        SqlParam = new SqlParameter[8];
        SqlParam[0] = new SqlParameter("@SalesChannelId", intSalesChannelID);
        SqlParam[1] = new SqlParameter("@LoginSaleschannelID", LoggedInSalesChannelID);
        SqlParam[2] = new SqlParameter("@OrderFromDate", OrderFromDate);
        SqlParam[3] = new SqlParameter("@OrderToDate", OrderToDate);
        SqlParam[4] = new SqlParameter("@OrderNumber", OrderNumber);
        SqlParam[5] = new SqlParameter("@StatusValue", StatusValue);
        SqlParam[6] = new SqlParameter("@OrderID", OrderID);
        SqlParam[7] = new SqlParameter("@Flag", Flag);
        dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelOrderDetailInfo", CommandType.StoredProcedure, SqlParam);
        return dtResult;
    }
    public DataTable GetSalesChannelOrderDetailInfoV1()
    {
        SqlParam = new SqlParameter[1];
        SqlParam[0] = new SqlParameter("@OrderID", OrderID);

        dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSecondaryOrderDetailInfo", CommandType.StoredProcedure, SqlParam);
        return dtResult;
    }

    public Int16 InsertOrderInformation(DataTable Dt)
    {
        try
        {

            SqlParam = new SqlParameter[7];
            SqlParam[0] = new SqlParameter("@tvpApprovedOrder", SqlDbType.Structured);
            SqlParam[0].Value = Dt;
            SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[3].Direction = ParameterDirection.Output;
            SqlParam[4] = new SqlParameter("@StatusValue", StatusValue);
            SqlParam[5] = new SqlParameter("@Flag", Flag);
            SqlParam[6] = new SqlParameter("@OrderID", OrderID);
            DataAccess.DataAccess.Instance.DBInsertCommand("PrcInsOrderedApprovedQuantity", SqlParam);
            Error = Convert.ToString(SqlParam[1].Value);
            if (SqlParam[2].Value != DBNull.Value)
            {
                strErrorDetailXML = (Convert.ToString(SqlParam[2].Value));
            }
            else
            {
                strErrorDetailXML = null;
            }
            return Convert.ToInt16(SqlParam[3].Value);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public Int16 InsertOrderInformationV1(DataTable Dt)
    {
        try
        {

            SqlParam = new SqlParameter[7];
            SqlParam[0] = new SqlParameter("@tvpApprovedOrder", SqlDbType.Structured);
            SqlParam[0].Value = Dt;
            SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[3].Direction = ParameterDirection.Output;
            SqlParam[4] = new SqlParameter("@StatusValue", StatusValue);
            SqlParam[5] = new SqlParameter("@Flag", Flag);
            SqlParam[6] = new SqlParameter("@OrderID", OrderID);
            DataAccess.DataAccess.Instance.DBInsertCommand("PrcInsSecondaryOrderedApprovedQuantity", SqlParam);
            Error = Convert.ToString(SqlParam[1].Value);
            if (SqlParam[2].Value != DBNull.Value)
            {
                strErrorDetailXML = (Convert.ToString(SqlParam[2].Value));
            }
            else
            {
                strErrorDetailXML = null;
            }
            return Convert.ToInt16(SqlParam[3].Value);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetSalesChannelStockInfo()
    {
        SqlParam = new SqlParameter[3];
        SqlParam[0] = new SqlParameter("@SalesChannelId", intSalesChannelID);
        SqlParam[1] = new SqlParameter("@Brand", intBrand);
        SqlParam[2] = new SqlParameter("@SalesChannelToID", intSalesChannelTo);
        dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetDailyStockForSalesChannel", CommandType.StoredProcedure, SqlParam);
        return dtResult;
    }
    public DataTable GetSalesChannelListForP1Sales()
    {
        SqlParam = new SqlParameter[3];
        SqlParam[0] = new SqlParameter("@SalesChannelId", intSalesChannelID);
        SqlParam[1] = new SqlParameter("@SalesChannelCode", SalesChannelCode);
        SqlParam[2] = new SqlParameter("@SalesChannelName", SalesChannelName);
        dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelListForP1Sales", CommandType.StoredProcedure, SqlParam);
        return dtResult;
    }
    //Added By Renuka Choudhary
    public Int32 InsertInfoSDSwap()
    {
        try
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@FromSDSalesChannelId", SalesChannelID);
            SqlParam[1] = new SqlParameter("@ToSDSalesChannelId", intSalesChannelTo);
            SqlParam[2] = new SqlParameter("@Out_Param", 0);
            SqlParam[2].Direction = ParameterDirection.Output;
            DataAccess.DataAccess.Instance.DBInsertCommand("prcTDTransfer", SqlParam);
            IntResultCount = Convert.ToInt16(SqlParam[2].Value);
            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //Renuka Choudhary
    public Int16 GetSalesChannelBackDays()
    {
        Int16 BackDaysAllowed = 0;
        try
        {

            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
            BackDaysAllowed = Convert.ToInt16(DataAccess.DataAccess.Instance.getSingleValues("prcGetBackDaysSales", SqlParam));

            return BackDaysAllowed;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //Pankaj Dhingra  New for opening stock Serial or batchwise
    //public Int16 InsertOpeningStock(DataTable Tvp)
    public Int16 InsertOpeningStock()
    {
        Int16 Result;
        SqlParam = new SqlParameter[9];
        SqlParam[0] = new SqlParameter("@SalesChannelId", intSalesChannelID);
        SqlParam[1] = new SqlParameter("@SessionId", SessionID);
        //SqlParam[1].Value = Tvp;
        SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
        SqlParam[2].Direction = ParameterDirection.Output;
        SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
        SqlParam[3].Direction = ParameterDirection.Output;
        SqlParam[4] = new SqlParameter("@OpeningStockDate", OpeningStockDate);
        SqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.Int);
        SqlParam[5].Direction = ParameterDirection.Output;
        SqlParam[6] = new SqlParameter("@UserId", UserID);
        SqlParam[7] = new SqlParameter("@CompanyId", CompanyId);
        SqlParam[8] = new SqlParameter("@OpeningStockFor", "0");/*0=For REtailer, 1= Sales Channel*/

        DataAccess.DataAccess.Instance.DBInsertCommand("prcInsOpeningStockSB", SqlParam);
        if (SqlParam[3].Value.ToString() != "")
        {
            XMLList = SqlParam[3].Value.ToString();
        }
        else
        {
            XMLList = null;
        }
        Error = Convert.ToString(SqlParam[2].Value);
        Result = Convert.ToInt16(SqlParam[5].Value);
        return Result;
    }

    public Int16 InsertCurrentStock()
    {
        Int16 Result;
        SqlParam = new SqlParameter[9];
        SqlParam[0] = new SqlParameter("@SalesChannelId", intSalesChannelID);
        SqlParam[1] = new SqlParameter("@SessionId", SessionID);
        //SqlParam[1].Value = Tvp;
        SqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 1000);
        SqlParam[2].Direction = ParameterDirection.Output;
        SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
        SqlParam[3].Direction = ParameterDirection.Output;
        SqlParam[4] = new SqlParameter("@OpeningStockDate", OpeningStockDate);
        SqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.Int);
        SqlParam[5].Direction = ParameterDirection.Output;
        SqlParam[6] = new SqlParameter("@UserId", UserID);
        SqlParam[7] = new SqlParameter("@CompanyId", CompanyId);
        SqlParam[8] = new SqlParameter("@OpeningStockFor", "1");/*0=For REtailer, 1= Sales Channel*/

        DataAccess.DataAccess.Instance.DBInsertCommand("prcUpdateCurrentStock", SqlParam);
        if (SqlParam[3].Value.ToString() != "")
        {
            XMLList = SqlParam[3].Value.ToString();
        }
        else
        {
            XMLList = null;
        }
        Error = Convert.ToString(SqlParam[2].Value);
        Result = Convert.ToInt16(SqlParam[5].Value);
        return Result;
    }
    public Int16 InsertOpeningStockWithZero()
    {
        Int16 Result;
        SqlParam = new SqlParameter[5];
        SqlParam[0] = new SqlParameter("@SalesChannelId", intSalesChannelID);
        SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
        SqlParam[1].Direction = ParameterDirection.Output;
        SqlParam[2] = new SqlParameter("@OpeningStockDate", OpeningStockDate);
        SqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.Int);
        SqlParam[3].Direction = ParameterDirection.Output;
        SqlParam[4] = new SqlParameter("@CompanyID", CompanyId);/* #CC41 Added */
        DataAccess.DataAccess.Instance.DBInsertCommand("prcInsOpeningStockSBWithZero", SqlParam);
        Error = Convert.ToString(SqlParam[1].Value);
        Result = Convert.ToInt16(SqlParam[3].Value);
        return Result;
    }
    public DataTable GetRetailers(int RetailerID, string RetailerName, string RetailerCode)
    {
        SqlParam = new SqlParameter[3];
        SqlParam[0] = new SqlParameter("@RetailerId", RetailerID);
        SqlParam[1] = new SqlParameter("@RetailerCode", RetailerCode);
        SqlParam[2] = new SqlParameter("@RetailerName", RetailerName);
        dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetRetailer", CommandType.StoredProcedure, SqlParam);
        return dtResult;
    }


    int _retailerID = 0;

    public int RetailerID
    {
        get
        {
            return _retailerID;
        }
        set { _retailerID = value; }
    }

    public Int16 InsertOpeningStockForRetailer(DataTable Tvp)
    {
        Int16 Result;
        SqlParam = new SqlParameter[6];
        SqlParam[0] = new SqlParameter("@RetailerID", _retailerID);
        SqlParam[1] = new SqlParameter("@TvpStock", SqlDbType.Structured);
        SqlParam[1].Value = Tvp;
        SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
        SqlParam[2].Direction = ParameterDirection.Output;
        SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
        SqlParam[3].Direction = ParameterDirection.Output;
        SqlParam[4] = new SqlParameter("@OpeningStockDate", OpeningStockDate);
        SqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.Int);
        SqlParam[5].Direction = ParameterDirection.Output;
        DataAccess.DataAccess.Instance.DBInsertCommand("prcInsOpeningStockForRetailer", SqlParam);
        if (SqlParam[3].Value.ToString() != "")
        {
            XMLList = SqlParam[3].Value.ToString();
        }
        else
        {
            XMLList = null;
        }
        Error = Convert.ToString(SqlParam[2].Value);
        Result = Convert.ToInt16(SqlParam[5].Value);
        return Result;
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

    ~SalesChannelData()
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


    public DataTable GetSalesChannelAndRetailer()
    {
        try
        {
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelWithRetailers", CommandType.StoredProcedure);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable SelectBinTypeByStockStatusId()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[1];
        objSqlParam[0] = new SqlParameter("@StockStatusID", StockStatusID);
        dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcStockBinTypeMaster_SelectByStatusId", CommandType.StoredProcedure, objSqlParam);
        if (dsResult != null && dsResult.Tables.Count > 0)
            dtResult = dsResult.Tables[0];
        return dtResult;
    }
    public DataSet CurrentSalesChannelStockStatus()
    {
        DataSet dsResult = new DataSet();
        SqlParameter[] objSqlParam = new SqlParameter[16];
        objSqlParam[0] = new SqlParameter("@SerialNumber", SerialNumber);
        objSqlParam[1] = new SqlParameter("@StockStatusID", StockStatusID);
        objSqlParam[2] = new SqlParameter("@StockBinTypeMasterID", StockBinTypeMasterID);
        objSqlParam[3] = new SqlParameter("@SkuName", SkuName);
        objSqlParam[4] = new SqlParameter("@SkuCode", SkuCode);
        objSqlParam[5] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
        objSqlParam[6] = new SqlParameter("@SelectedSalesChannelID", SalesChannelID);
        objSqlParam[7] = new SqlParameter("@UserID", UserID);
        objSqlParam[8] = new SqlParameter("@Condition", Condition);
        objSqlParam[9] = new SqlParameter("@PageIndex", PageIndex);
        objSqlParam[10] = new SqlParameter("@PageSize", PageSize);
        objSqlParam[11] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
        objSqlParam[11].Direction = ParameterDirection.Output;
        objSqlParam[12] = new SqlParameter("@OtherEntityTypeID", BaseEntityTypeID);//
        objSqlParam[13] = new SqlParameter("@SalesChannelName", SalesChannelCode);
        objSqlParam[14] = new SqlParameter("@ModelId", ModelId);/*#CC20  added*/
        objSqlParam[15] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);/*#CC33 Added*/
        objSqlParam[15].Direction = ParameterDirection.Output;/*#CC33 Added*/
        TotalRecords = Convert.ToInt32(objSqlParam[11].Value);
        //dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcCurrentChannelStockStatus", CommandType.StoredProcedure, objSqlParam);
        dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcCurrentChannelStockStatus", CommandType.StoredProcedure, objSqlParam);/*#CC07 ADDED*/
        //if (dsResult != null && dsResult.Tables.Count > 0)
        //dtResult = dsResult.Tables[0];
        if (Convert.ToString(objSqlParam[15].Value) != "")
        {
            Error = Convert.ToString(objSqlParam[15].Value);/*#CC33 Added*/
        }

        return dsResult;
    }
    public DataTable CurrentSalesChannel_DetailSB()
    {
        DataTable dtData = new DataTable("Record");
        SqlParameter[] objSqlParam = new SqlParameter[10];
        try
        {
            objSqlParam[0] = new SqlParameter("@SkuID", SkuID);
            objSqlParam[1] = new SqlParameter("@SalesChannelID", SalesChannelID);
            objSqlParam[2] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[3] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[4] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[4].Direction = ParameterDirection.Output;
            objSqlParam[5] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[5].Direction = ParameterDirection.Output;
            objSqlParam[6] = new SqlParameter("@StockStatusID", StockStatusID);
            objSqlParam[7] = new SqlParameter("@StockBinTypeMasterID", StockBinTypeMasterID);
            objSqlParam[8] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
            objSqlParam[9] = new SqlParameter("@OtherEntityTypeID", BaseEntityTypeID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcMain_DetailSB", CommandType.StoredProcedure, objSqlParam);
            TotalRecords = Convert.ToInt32(objSqlParam[4].Value);
            Error = Convert.ToString(objSqlParam[5].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }
            return dtResult;
        }
        catch { return dtResult; }
    }

    //Pankaj Dhingra
    public void ChangedExcelSheetNames(ref DataSet dsExcel, string[] strExcelSheetName, Int16 intSheetCount)
    {
        Int16 index;
        for (index = 0; index < intSheetCount; index++)
            dsExcel.Tables[index].TableName = strExcelSheetName[index];
        dsExcel.AcceptChanges();

    }
    #region MCC Code
    public Int32 MCCMasterId
    {
        get;
        set;
    }
    public string MCCCode
    {
        get;
        set;
    }
    public string OperatorName
    {
        get;
        set;
    }

    public string CircleName
    {
        get;
        set;
    }
    public DataTable GetMCCCodeInfo()
    {
        try
        {
            SqlParam = new SqlParameter[4];
            SqlParam[0] = new SqlParameter("@MCCMasterId", MCCMasterId);
            SqlParam[1] = new SqlParameter("@MCCCode", MCCCode);
            SqlParam[2] = new SqlParameter("@OperatorName", OperatorName);
            SqlParam[3] = new SqlParameter("@CircleName", CircleName);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetMCCCodeInfo", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public Int32 InsertUpdateMCCCodeInfo()
    {
        try
        {
            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@MCCMasterId", MCCMasterId);
            SqlParam[0].Direction = ParameterDirection.InputOutput;
            SqlParam[1] = new SqlParameter("@MCCCode", MCCCode);
            SqlParam[2] = new SqlParameter("@OperatorName", OperatorName);
            SqlParam[3] = new SqlParameter("@CircleName", CircleName);
            SqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 200);
            SqlParam[4].Direction = ParameterDirection.Output;
            DataAccess.DataAccess.Instance.DBInsertCommand("prcInsUpdMCCCodeInfo", SqlParam);
            IntResultCount = Convert.ToInt32(SqlParam[0].Value);

            if (SqlParam[4].Value != DBNull.Value && SqlParam[4].Value.ToString() != "")
            {
                Error = (SqlParam[4].Value).ToString();
            }

            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion


    /*#CC01 START ADDED*/
    public DataSet GetWarehouseAndNDS()
    {
        try
        {
            DataSet dsRes = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetWarehouseAndNDS", CommandType.StoredProcedure, SqlParam);
            return dsRes;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataSet GetAllBulkFile()
    {
        try
        {
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@FromDate", Fromdate);
            objSqlParam[1] = new SqlParameter("@ToDate", Todate);
            objSqlParam[2] = new SqlParameter("@FileURL", FileURL);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetAllBulkFile", CommandType.StoredProcedure, objSqlParam);
            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    public bool ChkDuplicateInvoiceNo()
    {
        try
        {
            SqlParameter[] objSqlParam = new SqlParameter[1];
            objSqlParam[0] = new SqlParameter("@InvoiceNo", InvoiceNo);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcChkDuplicateInvoiceNo", CommandType.StoredProcedure, objSqlParam);
            if (dtResult.Rows.Count > 0)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /*#CC01 START END*/
    /*#CC04 ADDED START*/
    public DataTable SalesChannelList()
    {
        try
        {
            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@SaleChanneltype", SalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@UserId", UserID);
            SqlParam[2] = new SqlParameter("@SalesChannelCode", SalesChannelCode);
            SqlParam[3] = new SqlParameter("@ComingFrom", ComingFrom);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcSaleChannelList", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    /*#CC04 ADDED END*/

    /* #CC05 Add Start */
    public Int16 SaveSalechanneltostatemapping()
    {
        Int16 Result;
        SqlParam = new SqlParameter[6];
        SqlParam[0] = new SqlParameter("@SalesChannelID", SalesChannelID);
        SqlParam[1] = new SqlParameter("@StateID", StateID);
        SqlParam[2] = new SqlParameter("@Status", Status);
        SqlParam[3] = new SqlParameter("@UserId", UserID);
        SqlParam[4] = new SqlParameter("@OutParam", SqlDbType.Int);
        SqlParam[4].Direction = ParameterDirection.Output;
        DataAccess.DataAccess.Instance.DBInsertCommand("prcSaveSalesChannelStateMapping", SqlParam);
        Result = Convert.ToInt16(SqlParam[4].Value);
        return Result;
    }


    /* #CC09 Add Start */

    public DataTable GetChannelStateMapping()
    {
        DataSet ds = new DataSet();
        SqlParam = new SqlParameter[6];
        SqlParam[0] = new SqlParameter("@SalesChannelID", SalesChannelID);
        SqlParam[1] = new SqlParameter("@OutParam", SqlDbType.Int);
        SqlParam[1].Direction = ParameterDirection.Output;
        SqlParam[2] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
        SqlParam[2].Direction = ParameterDirection.Output;
        SqlParam[3] = new SqlParameter("@PageIndex", PageIndex);
        SqlParam[4] = new SqlParameter("@PageSize", PageSize);
        SqlParam[5] = new SqlParameter("@SaleschannelTypeId", SalesChannelTypeID);/* #CC24*/
        dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetChannelStateMapping", CommandType.StoredProcedure, SqlParam);
        TotalRecords = Convert.ToInt32(SqlParam[2].Value);
        return dtResult;
    }
    public Int16 UpdateSalechanneltostatemapping()
    {
        Int16 Result;
        SqlParam = new SqlParameter[4];
        SqlParam[0] = new SqlParameter("@SalesChannelStateMappingID", SalesChannelStateMappingID);
        SqlParam[1] = new SqlParameter("@OutParam", SqlDbType.Int);
        SqlParam[1].Direction = ParameterDirection.Output;
        SqlParam[2] = new SqlParameter("@UserId", UserID);
        SqlParam[3] = new SqlParameter("@ParentSalesChannelID", ParentSalesChannelID);
        DataAccess.DataAccess.Instance.DBInsertCommand("prcUpdateChannelStateMapping", SqlParam);
        Result = Convert.ToInt16(SqlParam[1].Value);
        return Result;
    }


    /* #CC05 Add End */

    /* #CC07 Add Start */
    public int SaveSaleChannelOutStandingAmount()
    {
        Int16 Result;
        SqlParam = new SqlParameter[4];
        SqlParam[0] = new SqlParameter("@SessionID", SessionID);
        SqlParam[1] = new SqlParameter("@OutParam", SqlDbType.Int);
        SqlParam[1].Direction = ParameterDirection.Output;
        SqlParam[2] = new SqlParameter("@OutError", SqlDbType.NVarChar, 200);
        SqlParam[2].Direction = ParameterDirection.Output;
        SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
        SqlParam[3].Direction = ParameterDirection.Output;
        DataAccess.DataAccess.Instance.DBInsertCommand("prcSaveSaleChannelOutstandingAmount", SqlParam);
        if (SqlParam[3].Value.ToString() != "")
        {
            XMLList = SqlParam[3].Value.ToString();
        }
        else
        {
            XMLList = null;
        }
        Result = Convert.ToInt16(SqlParam[1].Value);
        Error = Convert.ToString(SqlParam[2].Value);

        return Result;
    }
    public DataSet GetTotalOutstandingDetail()
    {
        try
        {
            Int16 Result;
            SqlParameter[] SqlParam = new SqlParameter[4];
            SqlParam[0] = new SqlParameter("@SalesChannelID", SalesChannelID);
            SqlParam[1] = new SqlParameter("@OutParam", SqlDbType.Int);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@OutError", SqlDbType.NVarChar, 200);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@OutstandingDetailDownload", OutstandingDetailDownload);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetSaleChannelOutstandingAmount", CommandType.StoredProcedure, SqlParam);
            Result = Convert.ToInt16(SqlParam[1].Value);
            Error = Convert.ToString(SqlParam[2].Value);
            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    /* #CC07 Add End */
    /* #CC09 Add Start */
    public DataSet GetBulkUploadMappingData()
    {
        try
        {
            /* #CC01 Add Start */
            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@SalesChannelID", SalesChannelID);
            SqlParam[2] = new SqlParameter("@EntityMappingTypeId", EntityMappingTypeId);
            SqlParam[3] = new SqlParameter("@EntityMappingRelationId", EntityMappingRelationId);
            SqlParam[4] = new SqlParameter("@CompanyId", CompanyId);/*#CC38 Added*/
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetBulkUploadMappingData", CommandType.StoredProcedure, SqlParam);
            /* #CC01 Add End */
            /*dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetBulkUploadMappingData", CommandType.StoredProcedure, SqlParam);  #CC01 Commented */
            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    /* #CC14 Add Start */
    public DataSet GetBulkUploadMappingDataForRelationType()
    {
        try
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@MappingTypeId", SalesChannelMappingTypeID);
            SqlParam[1] = new SqlParameter("@SalesChannelID", SalesChannelID);
            SqlParam[2] = new SqlParameter("@CompanyId", CompanyId);/*#CC38 Added*/
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetRelationTypeBulkUploadMappingData", CommandType.StoredProcedure, SqlParam);
            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    /* #CC14 Add End */
    public DataSet SaveBulkUploadMapping()
    {
        Int16 Result;
        SqlParam = new SqlParameter[10]; /* #CC01 Length increased */
        SqlParam[0] = new SqlParameter("@SessionID", SessionID);
        SqlParam[1] = new SqlParameter("@OutParam", SqlDbType.Int);
        SqlParam[1].Direction = ParameterDirection.Output;
        SqlParam[2] = new SqlParameter("@OutError", SqlDbType.NVarChar, 200);
        SqlParam[2].Direction = ParameterDirection.Output;
        SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
        SqlParam[3].Direction = ParameterDirection.Output;
        //SqlParam[4] = new SqlParameter("@MappingTypeID", MappingTypeID);
        /* #CC01 Add Start */
        SqlParam[4] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
        SqlParam[5] = new SqlParameter("@SaleChannelID", SalesChannelID); /* #CC01 Add End */
        /* #CC02 Add Start */
        SqlParam[6] = new SqlParameter("@EntityMappingTypeID", EntityMappingTypeId);
        SqlParam[7] = new SqlParameter("@EntityMappingTypeRelationID", EntityMappingRelationId);
        SqlParam[8] = new SqlParameter("@UserId", UserID); /* #CC02 Add End */
        SqlParam[9] = new SqlParameter("@CompanyId", CompanyId);/*#CC38 Added*/
        dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcBulkMappingUpload", CommandType.StoredProcedure, SqlParam);
        /*  DataAccess.Instance.DBInsertCommand("prcBulkMappingUpload", SqlParam);*/
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
        /* return Result;*/
    }

    /* #CC09 Add End */


    public DataSet GetNDRDS()
    {
        try
        {
            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@SalesChannelID", SalesChannelID);
            SqlParam[1] = new SqlParameter("@SalesChannelTypeID", intSalesChannelTypeID);
            SqlParam[2] = new SqlParameter("@OutError", SqlDbType.NVarChar, 200);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@ParentSalesChannelID", ParentSalesChannelID);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetSalesChannel", CommandType.StoredProcedure, SqlParam);
            if (SqlParam[2].Value != DBNull.Value && SqlParam[2].Value.ToString() != "")
            {
                Error = (SqlParam[2].Value).ToString();
            }
            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }

    }



    public DataSet GetImeiAccExchangeReportData()
    {
        try
        {
            SqlParam = new SqlParameter[6];
            SqlParam[0] = new SqlParameter("@NDID", NDID);
            SqlParam[1] = new SqlParameter("@RDSID", RDSID);
            SqlParam[2] = new SqlParameter("@OutError", SqlDbType.NVarChar, 200);
            SqlParam[2].Direction = ParameterDirection.Output;
            //SqlParam[3] = new SqlParameter("@OutParam", SqlDbType.Int);
            //SqlParam[3].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@FromDate", Fromdate);
            SqlParam[4] = new SqlParameter("@ToDate", Todate);
            SqlParam[5] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            SqlParam[5].Direction = ParameterDirection.Output;

            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcIMEIUpdateThroughAccXchange", CommandType.StoredProcedure, SqlParam);
            if (SqlParam[2].Value != DBNull.Value && SqlParam[2].Value.ToString() != "")
            {
                Error = (SqlParam[2].Value).ToString();
            }
            TotalRecords = Convert.ToInt32(SqlParam[5].Value);
            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }



    }

    public DataSet GetEntityMappingType()
    {
        try
        {
            SqlParam = new SqlParameter[5];

            SqlParam[0] = new SqlParameter("@CompanyId", CompanyId);/*#CC38 Added*/
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcBindEntityMappingType", CommandType.StoredProcedure, SqlParam);

            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    /*#CC16 Added Started*/
    public DataSet GetReferenceCodeRetailer()
    {
        try
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@SalesChannelId", SalesChannelID);
            SqlParam[1] = new SqlParameter("@UserId", UserID);
            SqlParam[2] = new SqlParameter("@DownloadSKUCode", SkuCodeDownload);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcDoaReferenceCode", CommandType.StoredProcedure, SqlParam);
            return dsResult;
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
    public DataSet GetDOASalesChannel()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@SalesChannelId", SalesChannelID);
            SqlParam[1] = new SqlParameter("@UserId", UserID);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcDoaGetSalesChannel", CommandType.StoredProcedure, SqlParam);
            return dsResult;

        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    public DataSet SaveDOAUpload()
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
        dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcUploadDOA", CommandType.StoredProcedure, SqlParam);
        intOutParam = Convert.ToInt16(SqlParam[1].Value);
        Error = Convert.ToString(SqlParam[2].Value);
        return dsResult;
    }

    /*#CC16 Added End*/

    /*CC19 start*/
    public DataSet GetBulkMappedUnmappedData()
    {
        try
        {

            SqlParam = new SqlParameter[9];
            SqlParam[0] = new SqlParameter("@OutParam", SqlDbType.Int);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@OutError", SqlDbType.NVarChar, 200);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@EntityMappingTypeId", EntityMappingTypeId);
            SqlParam[3] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
            SqlParam[4] = new SqlParameter("@SaleChannelID", SalesChannelID);
            SqlParam[5] = new SqlParameter("@EntityMappingTypeRelationID", EntityMappingRelationId);
            SqlParam[6] = new SqlParameter("@UserID", UserID);
            SqlParam[7] = new SqlParameter("@CallingMode", ComingFrom);
            SqlParam[8] = new SqlParameter("@CompanyId", CompanyId);/*#CC38 Added*/

            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcMappedUnmappedData", CommandType.StoredProcedure, SqlParam);

            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }/*CC19 END*/
    /*#CC23 Start*/
    //    public DataTable GetSalesChannelTypeForStateMapping()
    //    {
    //        try
    //        {
    //            SqlParam = new SqlParameter[2];

    //            SqlParam[1] = new SqlParameter("@UserId", UserID);

    //            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("BindSaleschannelTypeForSaleschannelStateMapping",

    //CommandType.StoredProcedure, SqlParam);


    //            return dtResult;
    //        }

    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }
    /*#CC23 end*
    /*CC25 start*/
    public DataSet GetSaleschannelProductCategoryReferencecode()
    {
        try
        {

            SqlParam = new SqlParameter[3];

            SqlParam[0] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@StateId", StateID);
            SqlParam[2] = new SqlParameter("@CityId", CityID);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetReferenceDataForSalesChannelProductCategory", CommandType.StoredProcedure, SqlParam);

            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }


    public DataSet SaveSalesChannelProductCategoryMapping()
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
        SqlParam[4] = new SqlParameter("@UserId", UserID);
        SqlParam[5] = new SqlParameter("@SalesChannelTypeId", SalesChannelTypeID);
        dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcSaveProductCategorySaleschannelMapping", CommandType.StoredProcedure, SqlParam);

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

    }/*CC25 END*/



    /*#CC26*/
    public DataTable GetSaleschannelTypeBasedOnSaleType()
    {
        try
        {

            SqlParam = new SqlParameter[2];

            SqlParam[0] = new SqlParameter("@SalesTypeID", SaleTypeID);
            SqlParam[1] = new SqlParameter("@CompanyId", CompanyId);
            dtResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetSalesChannelTypeBasedonSalesType", CommandType.StoredProcedure, SqlParam).Tables[0];

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet GetReportBeatPlanData()
    {
        try
        {

            SqlParameter[] prm = new SqlParameter[14];
            prm[0] = new SqlParameter("@UserId", UserID);
            prm[1] = new SqlParameter("@SalesChannelTypeId", SalesChannelTypeID);
            prm[2] = new SqlParameter("@SalesChannelId", SalesChannelID);
            if (Fromdate.Year >= 1900)
            {
                prm[3] = new SqlParameter("@FromDate", Fromdate);
            }
            else
            {
                prm[3] = new SqlParameter("@FromDate", DBNull.Value);
            }
            if (Todate.Year >= 1900)
            {
                prm[4] = new SqlParameter("@Todate", Todate);
            }
            else
            {
                prm[4] = new SqlParameter("@Todate", DBNull.Value);
            }


            prm[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[5].Direction = ParameterDirection.Output;
            prm[6] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[6].Direction = ParameterDirection.Output;
            prm[7] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            prm[7].Direction = ParameterDirection.Output;
            prm[8] = new SqlParameter("@PageSize", PageSize);
            prm[9] = new SqlParameter("@PageIndex", PageIndex);
            prm[10] = new SqlParameter("@selectedFOSTSM", SelectedFOSTSM);
            prm[11] = new SqlParameter("@Status", ActiveStatus);
            prm[12] = new SqlParameter("@CompanyId", CompanyId);
            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetBeatPlanDetailV5", CommandType.StoredProcedure, prm);
            ErrorMessage = Convert.ToString(prm[6].Value);
            TotalRecords = Convert.ToInt32(prm[7].Value);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /*#CC29*/
    public DataTable GetSalesChannelListForPivotandStock()
    {
        try
        {
            SqlParam = new SqlParameter[4];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@ActiveStatus", ActiveStatus);
            SqlParam[2] = new SqlParameter("@UserID", UserID);



            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelListForPivotandStockReport", CommandType.StoredProcedure, SqlParam);


            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    /*#CC31*/
    public DataTable GetSalesChannelTypeV5API()
    {
        try
        {
            SqlParam = new SqlParameter[3];//#CC39
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@userId", UserID);
            SqlParam[2] = new SqlParameter("@CompanyId", CompanyId);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelTypeDatabaseV5", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    /*#CC31*/

    /* #CC35 Add Start */

    public DataSet GetOrghierarchyRetailerCreationV2()
    {
        try
        {

            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 2000);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.Int);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@UserId", UserID);
            SqlParam[3] = new SqlParameter("@SalesChannelID", SalesChannelID);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetOrghierarchyRetailerCreationV2", CommandType.StoredProcedure, SqlParam);
            intOutParam = Convert.ToInt16(SqlParam[2].Value);
            if (SqlParam[2].Value != null && Convert.ToString(SqlParam[2].Value) != "")
            {
                Error = Convert.ToString(SqlParam[2].Value);
            }
            if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
            {
                TotalRecords = dsResult.Tables[0].Rows.Count;
            }
            else
            {
                TotalRecords = 0;
            }

            return dsResult;

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    /* #CC35 Add End */

    /*#CC36 Added Started*/
    public DataTable BindSalesChannelName()
    {
        try
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@UserId", UserID);
            SqlParam[2] = new SqlParameter("@CompanyId", CompanyId);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelTsmApiReports", CommandType.StoredProcedure, SqlParam);
            return dtResult;
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
    /*#CC40 Added Starts*/
    public DataSet GetReportGenericBeatPlan()
    {
        try
        {

            SqlParameter[] prm = new SqlParameter[11];
            prm[0] = new SqlParameter("@UserId", UserID);
            prm[1] = new SqlParameter("@StateId", StateID);
            prm[2] = new SqlParameter("@DistrictId", DistrictID);
            prm[3] = new SqlParameter("@CityID", CityID);
            prm[4] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[4].Direction = ParameterDirection.Output;
            prm[5] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[5].Direction = ParameterDirection.Output;
            prm[6] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            prm[6].Direction = ParameterDirection.Output;
            prm[7] = new SqlParameter("@PageSize", PageSize);
            prm[8] = new SqlParameter("@PageIndex", PageIndex);
            // prm[10] = new SqlParameter("@selectedFOSTSM", SelectedFOSTSM);
            prm[9] = new SqlParameter("@Status", ActiveStatus);
            prm[10] = new SqlParameter("@CompanyId", CompanyId);
            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetBeatGenericPlanDetailV5", CommandType.StoredProcedure, prm);
            ErrorMessage = Convert.ToString(prm[5].Value);
            TotalRecords = Convert.ToInt32(prm[6].Value);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /*#CC40 Added End*/

    public DataSet GetReportPhysicalData()
    {
        try
        {

            SqlParameter[] prm = new SqlParameter[12];
            prm[0] = new SqlParameter("@UserId", UserID);
            prm[1] = new SqlParameter("@SalesChannelTypeId", SalesChannelTypeID);
            prm[2] = new SqlParameter("@SalesChannelId", SalesChannelID);
            if (Fromdate.Year >= 1900)
            {
                prm[3] = new SqlParameter("@FromDate", Fromdate);
            }
            else
            {
                prm[3] = new SqlParameter("@FromDate", DBNull.Value);
            }
            if (Todate.Year >= 1900)
            {
                prm[4] = new SqlParameter("@Todate", Todate);
            }
            else
            {
                prm[4] = new SqlParameter("@Todate", DBNull.Value);
            }


            prm[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
            prm[5].Direction = ParameterDirection.Output;
            prm[6] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            prm[6].Direction = ParameterDirection.Output;
            prm[7] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            prm[7].Direction = ParameterDirection.Output;
            prm[8] = new SqlParameter("@PageSize", PageSize);
            prm[9] = new SqlParameter("@PageIndex", PageIndex);
            prm[10] = new SqlParameter("@selectedFOSTSM", SelectedFOSTSM);
            prm[11] = new SqlParameter("@CompanyId", CompanyId);

            DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetPhysicalReport", CommandType.StoredProcedure, prm);
            ErrorMessage = Convert.ToString(prm[6].Value);
            TotalRecords = Convert.ToInt32(prm[7].Value);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet GetCheckInCheckOutDetails()
    {
        try
        {
            SqlParam = new SqlParameter[16];
            SqlParam[0] = new SqlParameter("@UserId", UserIdapi);
            SqlParam[1] = new SqlParameter("@SalesChannelTypeId", SalesChannelTypeID);
            SqlParam[2] = new SqlParameter("@SalesChannelId", SalesChannelID);
            SqlParam[3] = new SqlParameter("@FosTsmName", FosTsmName);
            if (FromDate1.Year >= 1900)
            {
                SqlParam[4] = new SqlParameter("@FromDate", FromDate1);
            }
            else
            {
                SqlParam[4] = new SqlParameter("@FromDate", DBNull.Value);
            }
            if (ToDate1.Year >= 1900)
            {
                SqlParam[5] = new SqlParameter("@ToDate", ToDate1);
            }
            else
            {
                SqlParam[5] = new SqlParameter("@ToDate", DBNull.Value);
            }
            SqlParam[6] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            SqlParam[6].Direction = ParameterDirection.Output;
            SqlParam[7] = new SqlParameter("@PageIndex", PageIndex);
            SqlParam[8] = new SqlParameter("@PageSize", PageSize);

            SqlParam[9] = new SqlParameter("@Out_Param", SqlDbType.Int);
            SqlParam[9].Direction = ParameterDirection.Output;
            SqlParam[10] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
            SqlParam[10].Direction = ParameterDirection.Output;
            SqlParam[11] = new SqlParameter("@selectedFOSTSM", FosTsmName);
            SqlParam[12] = new SqlParameter("@CompanyId", CompanyId);
            SqlParam[13] = new SqlParameter("@ImageUrlPath", SqlDbType.NVarChar, 200);
            SqlParam[13].Direction = ParameterDirection.Output;
            SqlParam[14] = new SqlParameter("@BrandId", BrandId);
            SqlParam[15] = new SqlParameter("@ProductCategoryId", ProductCategoryId);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetCheckInCheckOutDetailV4", CommandType.StoredProcedure, SqlParam);
            if (SqlParam[10].Value != DBNull.Value && SqlParam[10].Value.ToString() != "")
            {
                Error = Convert.ToString(SqlParam[10].Value);
            }
            TotalRecords = Convert.ToInt32(SqlParam[6].Value);
            CompanyImageFolder = Convert.ToString(SqlParam[13].Value);
            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }

    }
    /*#CC36 Added End*/
    public DataTable GetSerialTransactions(Int64 EntityId, int EntityTypeID, int SKUID, int CreatedBy, DateTime CreationOn)
    {
        try
        {
            dtResult = new DataTable();
            SqlParam = new SqlParameter[7];
            SqlParam[0] = new SqlParameter("@EntityId", EntityId);
            SqlParam[1] = new SqlParameter("@EntityTypeID", EntityTypeID);
            SqlParam[2] = new SqlParameter("@SKUID", SKUID);
            SqlParam[3] = new SqlParameter("@CreatedBy", CreatedBy);
            SqlParam[4] = new SqlParameter("@CreationOn", CreationOn);

            SqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[5].Direction = ParameterDirection.Output;
            SqlParam[6] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            SqlParam[6].Direction = ParameterDirection.Output;
            //SqlParam[7] = new SqlParameter("@PageIndex", Pageno);
            //SqlParam[8] = new SqlParameter("@PageSize", PageSize);

            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcSerialNumberForPhysicalStock", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetUserCheckOutImageInfo()
    {
        try
        {
            DataTable dtRetailerImage;
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@AppVisitId", AppVisitId);
            SqlParam[1] = new SqlParameter("@UserID", UserID);
            SqlParam[2] = new SqlParameter("@CompanyId", CompanyId);
            dtRetailerImage = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetUserCheckoutImagePath", CommandType.StoredProcedure, SqlParam);
            return dtRetailerImage;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }



}



