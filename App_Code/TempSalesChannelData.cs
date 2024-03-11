using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using DataAccess;
/*
 * 17-Dec-2014 , Sumit Kumar, #CC01, Create some function for check Invoice No existx or not , Binf NDS and Wahrehouse
 * 27-Sep-2016, Sumit Maurya, #CC02, New property created and value supplied to bind childsaleschannel.
 * 14-feb-2018, Sumit Maurya, #CC14, New property Created and value provided to bind referance code accrdong to targetID (done for Comio).
 *  * 13-July-2018, Rajnish Kumar, #CC15, View Check-in Check-out Details
 * 
 */

public class TempSalesChannelData : IDisposable
{
    #region Private Properties

    private string strErrorDetailXML;
    private Int32 intSalesChannelID, intNumberOfBackDaysSC, intUserID, intOrgnhierarchyID, intGroupParentSalesChannelID, intParentSalesChannelID, intBrand, intProducteCategoryid, intSalesChannelTo;
    private Int16 intCityID, intAreaID, intStateID, intSalesChannelTypeID, intType, intcountryid;
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
    private string _xmlList;
    private EnumData.eSalesChannelLevel eSalesChanneLevel;
    private EnumData.eSalesChannelType eSalesChanneType;
    private EnumData.eControlRequestTypeForEntry eReqType;
    private EnumData.eSearchConditions eSearchType;
    private Boolean blnIsBrandwise;
    private Boolean blnIsHierarchylevel;
    private Int16 intHierarchyLevelID;
   /*#CC15*/ string todate; string fromdate;
    #endregion
    #region Public Properties
    public Int16 OtherEntityTypeID
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

    /*#CC15 start*/
    public Int32 SaleschannelIdFos  
    {
        get;
        set;
    }
    public Int32 SaleschannelTypeIdFos
    {
        get;
        set;
    }
    public string FosTsmName
    {
        get;
        set;
    }

    public string ToDate1
    {
        get { return todate; }
        set { todate = value; }

    }

    public string FromDate1
    {

        get { return fromdate; }
        set { fromdate = value; }
    }
    public Int32 UserIdapi
    {

        get;
        set;
    }
    /*#CC15 end*/
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
    public int GetND
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
    /*#CC01 START END*/
    /* #CC02 Add Start */
    public int BindChild
    {
        get;
        set;
    }
    /* #CC02 Add End */
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
    /* #CC14 Add Start */
    public string TargetName
    {
        get;
        set;
    }
    /* #CC14 Add End */
  

    #endregion

    #region Class Variables
    Int16 ishozsm;
    Int16 isreport;
    DataTable dtResult;
    SqlParameter[] SqlParam;
    Int32 IntResultCount = 0;
    DataSet dsResult;
    Int32 saleschannellevel;
    Int32 backdaysnumber;

    string saleschannelgroupname;
    string saleschanneltypename;
    #endregion

    

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
            SqlParam = new SqlParameter[4];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@ActiveStatus", ActiveStatus);
            SqlParam[2] = new SqlParameter("@ComingFrom", ComingFrom);// when it will be zero then old functionality else will work in the webservice
            SqlParam[3] = new SqlParameter("@SalesChannelCode", SalesChannelCode);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelList", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
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
            SqlParam[2] = new SqlParameter("@OtherEntityTypeID", OtherEntityTypeID);
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

    public DataTable GetSalesChannelListWithRetailerV2()
    {
        try
        {
            SqlParam = new SqlParameter[6]; /* #CC10 length increased from 5-6*/
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@ActiveStatus", ActiveStatus);
            SqlParam[2] = new SqlParameter("@OtherEntityTypeID", OtherEntityTypeID);
            SqlParam[3] = new SqlParameter("@UserID", UserID);
            SqlParam[4] = new SqlParameter("@loggedInChannelID", SqlDbType.Int);
            SqlParam[4].Direction = ParameterDirection.Output;
            SqlParam[5] = new SqlParameter("@GetND", GetND); /* #CC10 Added */
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
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetCompleteSalesChannelTypeV2", CommandType.StoredProcedure, SqlParam);
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
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetCompleteSalesChannelTypeV3", CommandType.StoredProcedure, SqlParam);
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
            SqlParam[1] = new SqlParameter("@OtherEntityTypeID", OtherEntityTypeID);
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
    public DataTable GetSalesChannelParent()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@saleschannelid", SalesChannelID);
            SqlParam[1] = new SqlParameter("@saleschanneltype", SalesChannelTypeID);
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
            SqlParam = new SqlParameter[6]; /* #CC14 length increased from 5 to 6 */
            SqlParam[0] = new SqlParameter("@UserID", UserID);
            SqlParam[1] = new SqlParameter("@ReqType", eReqType);
            SqlParam[2] = new SqlParameter("@SecondarySalesChannelID", SalesChannelID);
            SqlParam[3] = new SqlParameter("@SalesChanneLevel", eSalesChanneLevel);
            SqlParam[4] = new SqlParameter("@BrandID", Brand);
            SqlParam[5] = new SqlParameter("@TargetName", TargetName); /* #CC14 Added */
            //SqlParam[5] = new SqlParameter("@IsBrandwise", IsBrandwise);
            //SqlParam[6] = new SqlParameter("@IsHierarchylevel", IsHierarchylevel);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetAllTemplateData", CommandType.StoredProcedure, SqlParam);
            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
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
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@SalesChannelTypeID", intSalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@ParentSalesChannelID", intParentSalesChannelID);
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
            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@SalesChanneType", eSalesChanneType);
            SqlParam[1] = new SqlParameter("@RoleType", RoleType);
            SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
            SqlParam[3] = new SqlParameter("@RetailerCode", RetailerCode);
            SqlParam[4] = new SqlParameter("@RetailerName", RetailerName);
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
            SqlParam = new SqlParameter[32];
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
            DataAccess.DataAccess.Instance.DBInsertCommand("prcInsUpdSalesChannel", SqlParam);
            IntResultCount = Convert.ToInt32(SqlParam[0].Value);
            if (SqlParam[25].Value != DBNull.Value && SqlParam[25].Value.ToString() != "")
            {
                Error = (SqlParam[25].Value).ToString();
            }
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
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelInfoByParameters", CommandType.StoredProcedure, SqlParam);

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

            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesChannelID);
            SqlParam[1] = new SqlParameter("@NumberofBackdaysSC", intNumberOfBackDaysSC);
            SqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.Int);
            SqlParam[2].Direction = ParameterDirection.Output;
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
    public Int16 InsertOpeningStock(DataTable Tvp)
    {
        Int16 Result;
        SqlParam = new SqlParameter[6];
        SqlParam[0] = new SqlParameter("@SalesChannelId", intSalesChannelID);
        SqlParam[1] = new SqlParameter("@TvpStock", SqlDbType.Structured);
        SqlParam[1].Value = Tvp;
        SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
        SqlParam[2].Direction = ParameterDirection.Output;
        SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
        SqlParam[3].Direction = ParameterDirection.Output;
        SqlParam[4] = new SqlParameter("@OpeningStockDate", OpeningStockDate);
        SqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.Int);
        SqlParam[5].Direction = ParameterDirection.Output;
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
    public Int16 InsertOpeningStockWithZero()
    {
        Int16 Result;
        SqlParam = new SqlParameter[4];
        SqlParam[0] = new SqlParameter("@SalesChannelId", intSalesChannelID);
        SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
        SqlParam[1].Direction = ParameterDirection.Output;
        SqlParam[2] = new SqlParameter("@OpeningStockDate", OpeningStockDate);
        SqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.Int);
        SqlParam[3].Direction = ParameterDirection.Output;
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

    ~TempSalesChannelData()
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
    public DataTable CurrentSalesChannelStockStatus()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[13];
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
        objSqlParam[12] = new SqlParameter("@OtherEntityTypeID", OtherEntityTypeID);
        TotalRecords = Convert.ToInt32(objSqlParam[11].Value);
        dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcCurrentChannelStockStatus", CommandType.StoredProcedure, objSqlParam);
        if (dsResult != null && dsResult.Tables.Count > 0)
            dtResult = dsResult.Tables[0];
        return dtResult;
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
            objSqlParam[9] = new SqlParameter("@OtherEntityTypeID", OtherEntityTypeID);
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
    public string SessionID
    {
        get;
        set;
    }
    /* #CC09 Add End */

    /* #CC09 Add Start */
    public DataSet GetBulkUploadMappingData()
    {
        try
        {
            /* #CC01 Add Start */
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@SalesChannelID", SalesChannelID);
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



    public DataSet SaveBulkUploadMapping()
    {
        Int16 Result;
        SqlParam = new SqlParameter[7]; /* #CC01 Length increased */
        SqlParam[0] = new SqlParameter("@SessionID", SessionID);
        SqlParam[1] = new SqlParameter("@OutParam", SqlDbType.Int);
        SqlParam[1].Direction = ParameterDirection.Output;
        SqlParam[2] = new SqlParameter("@OutError", SqlDbType.NVarChar, 200);
        SqlParam[2].Direction = ParameterDirection.Output;
        SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
        SqlParam[3].Direction = ParameterDirection.Output;
        SqlParam[4] = new SqlParameter("@MappingTypeID", MappingTypeID);
        /* #CC01 Add Start */
        SqlParam[5] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
        SqlParam[6] = new SqlParameter("@SaleChannelID", SalesChannelID); /* #CC01 Add End */
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

   /*#CC15 start*/ public DataTable GetSalesChannelTypeTsm()
    {
        try
        {
            SqlParam = new SqlParameter[2];
           
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelTypeTsmApiReports", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetSalesChannelapiTsm()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelTsmApiReports", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetFosTsmName()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@userId", UserID);

            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalemanV5", CommandType.StoredProcedure, SqlParam);
            return dtResult;
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
            SqlParam = new SqlParameter[11];
            SqlParam[0] = new SqlParameter("@UserId", UserIdapi);
            SqlParam[1] = new SqlParameter("@SalesChannelTypeId", SalesChannelTypeID);
            SqlParam[2] = new SqlParameter("@SalesChannelId", SalesChannelID);
            SqlParam[3] = new SqlParameter("@FosTsmName", FosTsmName);
            SqlParam[4] = new SqlParameter("@FromDate", FromDate1);
            SqlParam[5] = new SqlParameter("@ToDate", ToDate1);
            SqlParam[6] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            SqlParam[6].Direction = ParameterDirection.Output;
            SqlParam[7] = new SqlParameter("@PageIndex", PageIndex);
            SqlParam[8] = new SqlParameter("@PageSize", PageSize);

            SqlParam[9] = new SqlParameter("@Out_Param", SqlDbType.Int);
            SqlParam[9].Direction = ParameterDirection.Output;
            SqlParam[10] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
            SqlParam[10].Direction = ParameterDirection.Output;
            
            

            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetCheckInCheckOutDetailV4", CommandType.StoredProcedure, SqlParam);
            if (SqlParam[2].Value != DBNull.Value && SqlParam[2].Value.ToString() != "")
            {
                Error = (SqlParam[2].Value).ToString();
            }
            TotalRecords = Convert.ToInt32(SqlParam[6].Value);
            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }

    }

    /*#CC15 end*/
}



