﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Configuration;
using DataAccess;
/*
 * 06-May-2015, Karam Chand Sharma, #CC01, Create New Function For Bind Grid for retailer transfer
 * 05-June-2015, Karam Chand Sharma, #CC02, Pass user id into GetRetailerInfoV3  function
 * 14-Oct-2015, Sumit Maurya, #CC03, new parameter supplied in method GetRetailerInfoV3() and property "intStatus", "RetailerApproval" created to pass intiger value.
 * 15-Oct-15, Karam Chand Sharma, #CC04 - please refer url for more details https://zed-axis.basecamphq.com/projects/5476690-zed-salestrack/todo_items/202983804/comments#326492235
 * 28 Oct 2015,Karam Chand Shanrma, #CC05, Added some properties for retailer bank detail and pass into retailer save function
 * 28-Oct-2015, Sumit Maurya, #CC06, new parameter supplied in method UploadRetailer();
 * 30-Oct-2015, Sumit Maurya, #CC07, new property PANNo created and supplied to method InsertUpdateRetailer().
 * 31-Oct-2015, Karam Chand Shanrma, #CC08, Only approve retailer will bind in view retailer interface
 * 10-Nov-2015, Sumit Maurya, #CC09, RetailerID supplied to function CheckRetailer().
 * 16-Nov-2015, Sumit Maurya, #CC10, Approval Remarks supplied as it was needed to save remarks (Earlier it was supplied but got removed by mistake).Take retailercode as an output when retailer approve by HO
 * 17-Mar-2016, Sumit Maurya, #CC11, New property created and  parameter supplied to save Counter Potential Value of retailer.
 * 30-Mar-2016, Sumit Maurya, #CC12, new property TehsilID created and parameter supplied in method to save/update TehsilID by interface.
 * 17-Aug-2016, Sumit Maurya, #CC13, New properties and method created get retailer mapping information.
 * 29-Aug-2016, Sumit Maurya, #CC14, New properties supplied in method InsertUpdateRetailer().
 * 20-Apr-2017, Balram Jha, #CC15 - Increase command timeout for report
 * * 28-Nov-2017,Vijay Kumar Prajapati,#CC16-Add Userid for Download Retailer Mapping Info . 
 * 08-March-2018,Vijay Kumar Prajapati,#CC17-Chaeck if dataset null.
 * 02-Apr-2018, Sumit Maurya, #CC18 , New parameter value provided to get Approval data (Done for V5).
 * 08-Apr-2018, Sumit Maurya, #CC19, changed accessblility to provide status from interface (Done for Motorola).
   * 21-May-2018, Rajnish Kumar, #CC20, Whatsapp number is added.It is configurable  (Done for Karbon).
  * 24-May-2018, Rajnish Kumar, #CC21, Save and GetRetailer Image Info (Done For Karbon).
 * 30-May-2018, Rajnish Kumar, #CC22, View Retailer Image Info (Done For Karbon).
 * * 19-June-2018, Rajnish Kumar, #CC23, Save and View RetailerMarketingData.
 * 6-July-2018, Rakesh Raj, #CC24, Added Invalid Data Link feature  
 *  08-Oct-2018, Sumit Maurya, #CC25, New function created to get data for View and approve Retailer (Done for Karbonn).
 *  21-April-2020,Vijay Kumar Prajapati,#CC26 Added CompanyId in Method.
 * 
 */


public class RetailerData : IDisposable
{

    #region Private Class Variables
    private EnumData.eSearchConditions eSearchConditions;
    private int intRetailerID, intSalesChannelID, intSalesmanID;
    private Int32 intRetailerTypeId;
    private string strRetailerCode, strRetailerName, strContactPerson, strAddress1, strAddress2, strPinCode, strMobileNumber, strRetailerDetailXML, strPhoneNumber, strTinNumber, strError, strSalesmanName, strEmail,strWhatsAppNumber;
    private Int16 intAreaID, intCityID, intStateID; string intCounterSize;
    /* private #CC19 Commented */  public /* #CC19 Added */ Boolean blnStatus, blnIsp;
    private int _DisplayMode = 0;/*#CC08 ADDED*/
    public string ConString = ConfigurationManager.ConnectionStrings["AppConString"].ConnectionString;//#CC15 added
    public int DisplayMode /*1: from view retailer interface 0:from approve retailer*/ /*#CC08 ADDED*/
    {
        get
        {
            return _DisplayMode;
        }
        set
        {
            _DisplayMode = value;
        }
    }
    #endregion
    #region Public Properties
    public string BankName
    {
        get;
        set;
    }
    public string AccountHolder
    {
        get;
        set;
    }
    public string AccountNumber
    {
        get;
        set;
    }
    /*CC20*/public string whatsAppNumber
    {
        get;
        set;
    }
    public string BranchLocation
    {
        get;
        set;
    }
    public string IFSCCode
    {
        get;
        set;
    }
    public bool UpdateBankDetail
    {
        get;
        set;
    }
    /* #CC07 Add Start */
    public string PANNo
    {
        get;
        set;
    }
    /* #CC07 Add End*/
    /*#CC04 ADDED START*/
    public int OutPutResult
    {
        get;
        set;
    }
    public int ApprovalStatus
    {
        get;
        set;
    }
    public string ApprovalRemarks
    {
        get;
        set;
    }
    /*#CC04 ADDED END*/
    public EnumData.eSearchConditions SearchCondition
    {
        get { return eSearchConditions; }
        set { eSearchConditions = value; }
    }
    public int value
    {
        get;
        set;
    }
    public int SkuID
    {
        get;
        set;
    }
    public Int32 LoggedInSalesChannelid
    {
        get;
        set;
    }
    public int RetailerID
    {
        get { return intRetailerID; }
        set { intRetailerID = value; }
    }
    public Int32 RetailerTypeID
    {
        get { return intRetailerTypeId; }
        set { intRetailerTypeId = value; }
    }
    public int SalesmanID
    {
        get { return intSalesmanID; }
        set { intSalesmanID = value; }
    }
    public DateTime? SaleDate
    {
        get;
        set;
    }
    public string IMEINo
    {
        get;
        set;
    }
    public Int32 UserID {get; set;}
    public Int32 CompanyId { get; set; }
    public Int16 OtherEntityID
    {
        get;
        set;
    }

    /* #CC03 Add Start */
    private int _intStatus;
    public int intStatus
    {
        get { return _intStatus; }
        set { _intStatus = value; }
    }
    private int _RetailerApproval = -1;
    public int RetailerApproval
    {
        get { return _RetailerApproval; }
        set { _RetailerApproval = value; }
    }
    /* #CC03 Add End */

    public int SalesChannelID
    {
        get { return intSalesChannelID; }
        set { intSalesChannelID = value; }
    }
    public string RetailerDetailXML
    {
        get { return strRetailerDetailXML; }
        set { strRetailerDetailXML = value; }
    }

    public string RetailerCode
    {
        get { return strRetailerCode; }
        set { strRetailerCode = value; }
    }


    public string RetailerName
    {
        get { return strRetailerName; }
        set { strRetailerName = value; }
    }


    public string ContactPerson
    {
        get { return strContactPerson; }
        set { strContactPerson = value; }
    }


    public string Address1
    {
        get { return strAddress1; }
        set { strAddress1 = value; }
    }

    public string Address2
    {
        get { return strAddress2; }
        set { strAddress2 = value; }
    }

    public string PinCode
    {
        get { return strPinCode; }
        set { strPinCode = value; }
    }


    public string MobileNumber
    {
        get { return strMobileNumber; }
        set { strMobileNumber = value; }
    }
    /*#CC20*/
    /*public string WhatsAppNumber
    {
        get { return WhatsAppNumber; }
        set { WhatsAppNumber = value; }
    }*/
    public string PhoneNumber
    {
        get { return strPhoneNumber; }
        set { strPhoneNumber = value; }
    }

    public string TinNumber
    {
        get { return strTinNumber; }
        set { strTinNumber = value; }
    }


    public string Email
    {
        get { return strEmail; }
        set { strEmail = value; }
    }


    public string Error
    {
        get { return strError; }
        set { strError = value; }
    }


    public Int16 AreaID
    {
        get { return intAreaID; }
        set { intAreaID = value; }
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
    public string SalesmanName
    {
        get { return strSalesmanName; }
        set { strSalesmanName = value; }
    }


    public string CounterSize
    {
        get { return intCounterSize; }
        set { intCounterSize = value; }
    }

    public Int16 Type
    {
        get { return intType; }
        set { intType = value; }
    }


    public Boolean Status
    {
        get { return blnStatus; }
        set { blnStatus = value; }
    }
    public Boolean ISP
    {
        get { return blnIsp; }
        set { blnIsp = value; }
    }
    public string Password
    {
        get;
        set;
    }
    public string UserName
    {
        get;
        set;
    }
    public Int32? GroupParentID
    {
        get;
        set;
    }
    public int CreateLoginOrNot
    {
        get;
        set;
    }
    public int PasswordExpiryDays
    {
        get;
        set;
    }
    public string PasswordSalt
    {
        get;
        set;
    }
    public Int16 RetailerHierarchyLevelID
    {
        get;
        set;
    }
    public Int32 RetailerOrgnHierarchyID
    {
        get;
        set;
    }
    public DateTime? DateOfBirth
    {
        get;
        set;
    }
    //property for Manage Retailer Type
    private string _InsError;
    public string InsError
    {
        get { return _InsError; }
        set { _InsError = value; }
    }
    private string _RetailerType;
    public string RetailerType
    {
        get { return _RetailerType; }
        set { _RetailerType = value; }
    }
    public int ModelId
    {
        get;
        set;
    }
    /* #CC11 Add Start */
    public Int64 CounterPotentialValue
    {
        get;
        set;
    }
    /* #CC11 Add End */
    /*  #CC12 Add Start */
    private Int16 intTehsilId = 0;
    public Int32 BrandId { get; set; }
    public Int32 ProductCategoryId { get; set; }
    public Int16 TehsilId
    {
        get { return intTehsilId; }
        set { intTehsilId = value; }
    }
    /*  #CC12 Add End */
    /* #CC13 Add Start */
    public int? NDID
    {
        get;
        set;
    }
    public int? RDSID
    {
        get;
        set;
    }
    public string SalesChannelCode
    {
        get;
        set;
    }
    /* #CC13 Add End */
    /* #CC14 Add Start */
    public string ReferanceCode
    {
        get;
        set;
    }
    /* #CC14 Add End */

    /* #CC18 Add Start */
    public Int16? FetchDataForApproval
    {
        get;
        set;
    }
    /* #CC18 Add End */
    public string WOFileXML { set; get; }/*CC21 added*/
    public string WOFileXML1 { set; get; }
    /*#CC23 start*/
    public string SessionID
    {
        get;
        set;
    }
    public string OriginalFileName
    {
        get;
        set;
    }
    public string UniqueFileName
    {
        get;
        set;
    }
    public Int32 ZoneId
    {
        get;
        set;
    }

    public Int16 ActiveInActiveFlag
    {
        get;
        set;
    }
    public int intOutParam
    {
        get;
        set;
    }
    public Int16 CsaType
    {
        get;
        set;
    }
    string todate; string fromdate;
    public string ToDate
    {
        get { return todate; }
        set { todate = value; }

    }

    public string FromDate
    {

        get { return fromdate; }
        set { fromdate = value; }
    }

    public Int16 Flag
    {

        get;
        set;
    }
    /*#CC23 end*/
    #endregion
    #region Class variables

    DataTable dtResult;
    SqlParameter[] SqlParam;
    Int32 IntResultCount = 0;
    DataSet dsResult;
    short intType;
    #endregion
    #region Insert Update Retailer

    public Int32 InsertUpdateRetailer()
    {
        try
        {
            SqlParam = new SqlParameter[46]; /* #CC11 length increased from 42 to 43 */ /* #CC12 length increased from 43 to 44 */ /* #CC14 length increased from 44 to 45 *//* #CC20 length increased from 45 to 46 */
            SqlParam[0] = new SqlParameter("@RetailerID", intRetailerID);
            SqlParam[0].Direction = ParameterDirection.InputOutput;
            SqlParam[1] = new SqlParameter("@RetailerCode", strRetailerCode);
            SqlParam[2] = new SqlParameter("@RetailerName", strRetailerName);
            SqlParam[3] = new SqlParameter("@ContactPerson", strContactPerson);
            SqlParam[4] = new SqlParameter("@Address1", strAddress1);
            SqlParam[5] = new SqlParameter("@Address2", strAddress2);
            SqlParam[6] = new SqlParameter("@PinCode", strPinCode);
            SqlParam[7] = new SqlParameter("@MobileNumber", strMobileNumber);
            SqlParam[8] = new SqlParameter("@PhoneNumber", strPhoneNumber);
            SqlParam[9] = new SqlParameter("@AreaID", intAreaID);
            SqlParam[10] = new SqlParameter("@CityID", intCityID);
            SqlParam[11] = new SqlParameter("@StateID", intStateID);
            SqlParam[12] = new SqlParameter("@TinNumber", strTinNumber);
            SqlParam[13] = new SqlParameter("@Email", strEmail);
            SqlParam[14] = new SqlParameter("@CounterSize", intCounterSize);
            SqlParam[15] = new SqlParameter("@SalesChannelID", intSalesChannelID);
            SqlParam[16] = new SqlParameter("@Status", blnStatus);
            SqlParam[17] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 200);
            SqlParam[17].Direction = ParameterDirection.Output;
            SqlParam[18] = new SqlParameter("@SalesmanID", intSalesmanID);
            SqlParam[19] = new SqlParameter("@RetailerTypeId", intRetailerTypeId);
            SqlParam[20] = new SqlParameter("@IsIsp", blnIsp);
            SqlParam[21] = new SqlParameter("@UserName", UserName);
            SqlParam[22] = new SqlParameter("@Password", Password);
            SqlParam[23] = new SqlParameter("@GroupParentID", GroupParentID);
            SqlParam[24] = new SqlParameter("@CreateLoginOrNot", CreateLoginOrNot);
            SqlParam[25] = new SqlParameter("@PasswordExpiryDays", PasswordExpiryDays);
            SqlParam[26] = new SqlParameter("@PasswordSalt", PasswordSalt);
            SqlParam[27] = new SqlParameter("@RetailerHierarchyLevelID", RetailerHierarchyLevelID);
            SqlParam[28] = new SqlParameter("@RetailerOrgnHierarchyID", RetailerOrgnHierarchyID);
            SqlParam[29] = new SqlParameter("@OpeningStockDate", OpeningStockDate);
            SqlParam[30] = new SqlParameter("@DateOfBirth", DateOfBirth);
            SqlParam[31] = new SqlParameter("@LoginUserId", UserID);
            SqlParam[32] = new SqlParameter("@ApproveStatus", ApprovalStatus);  /*#CC04 ADDED */
            SqlParam[33] = new SqlParameter("@BankName", BankName);/*#CC05 ADDED*/
            SqlParam[34] = new SqlParameter("@AccountHolder", AccountHolder);/*#CC05 ADDED*/
            SqlParam[35] = new SqlParameter("@AccountNumber", AccountNumber);/*#CC05 ADDED*/
            SqlParam[36] = new SqlParameter("@BranchLocation", BranchLocation);/*#CC05 ADDED*/
            SqlParam[37] = new SqlParameter("@IFSCCode", IFSCCode);/*#CC05 ADDED*/
            SqlParam[38] = new SqlParameter("@UpdateBankDetail", UpdateBankDetail);       /*#CC05 ADDED*/
            SqlParam[39] = new SqlParameter("@PANNo", PANNo);       /*#CC07 ADDED*/
            SqlParam[40] = new SqlParameter("@ApproveRemarks", ApprovalRemarks);      /*#CC10 Added*/
            SqlParam[41] = new SqlParameter("@NewRetailerCode", SqlDbType.VarChar, 20);/*#CC10 ADDED*/
            SqlParam[41].Direction = ParameterDirection.Output;
            SqlParam[42] = new SqlParameter("@CounterPotentialValue", CounterPotentialValue);    /* #CC11 Added */
            SqlParam[43] = new SqlParameter("@TehsilID", TehsilId);    /* #CC12 Added */
            SqlParam[44] = new SqlParameter("@ReferanceCode", ReferanceCode);    /* #CC14 Added */
            SqlParam[45] = new SqlParameter("@WhatsAppNumber", whatsAppNumber);     /*#CC20 Added */
            DataAccess.DataAccess.Instance.DBInsertCommand("prcInsUpdRetailerInfo", SqlParam);
            IntResultCount = Convert.ToInt32(SqlParam[0].Value);
            if (SqlParam[17].Value != DBNull.Value && SqlParam[17].Value.ToString() != "")
            {
                Error = (SqlParam[17].Value).ToString();
            }
            RetailerCode = SqlParam[41].Value.ToString();
            RetailerID = Convert.ToInt32(SqlParam[0].Value);
            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    public Int32 InsertRetailerIMEIInfo()       //Will be used in the POc and on a WAP Page
    {
        try
        {
            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@RetailerID", intRetailerID);
            SqlParam[1] = new SqlParameter("@SkuID", SkuID);
            SqlParam[2] = new SqlParameter("@SalesDate", SaleDate);
            SqlParam[3] = new SqlParameter("@IMEI", IMEINo);
            SqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 200);
            SqlParam[4].Direction = ParameterDirection.Output;
            DataAccess.DataAccess.Instance.DBInsertCommand("prcInsertRetailerIMEI", SqlParam);
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

    public DataTable GetSalesIMEI()
    {
        try
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@RetailerID", RetailerID);
            SqlParam[1] = new SqlParameter("@IMEINo", IMEINo);
            SqlParam[2] = new SqlParameter("@UserID", UserID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesIMEI", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    public Int32 InsertUpdateRetailerWithType()
    {
        try
        {
            SqlParam = new SqlParameter[20];
            SqlParam[0] = new SqlParameter("@RetailerID", intRetailerID);
            SqlParam[0].Direction = ParameterDirection.InputOutput;
            SqlParam[1] = new SqlParameter("@RetailerCode", strRetailerCode);
            SqlParam[2] = new SqlParameter("@RetailerName", strRetailerName);
            SqlParam[3] = new SqlParameter("@ContactPerson", strContactPerson);
            SqlParam[4] = new SqlParameter("@Address1", strAddress1);
            SqlParam[5] = new SqlParameter("@Address2", strAddress2);
            SqlParam[6] = new SqlParameter("@PinCode", strPinCode);
            SqlParam[7] = new SqlParameter("@MobileNumber", strMobileNumber);
            SqlParam[8] = new SqlParameter("@PhoneNumber", strPhoneNumber);
            SqlParam[9] = new SqlParameter("@AreaID", intAreaID);
            SqlParam[10] = new SqlParameter("@CityID", intCityID);
            SqlParam[11] = new SqlParameter("@StateID", intStateID);
            SqlParam[12] = new SqlParameter("@TinNumber", strTinNumber);
            SqlParam[13] = new SqlParameter("@Email", strEmail);
            SqlParam[14] = new SqlParameter("@CounterSize", intCounterSize);
            SqlParam[15] = new SqlParameter("@SalesChannelID", intSalesChannelID);
            SqlParam[16] = new SqlParameter("@Status", blnStatus);
            SqlParam[17] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 200);
            SqlParam[17].Direction = ParameterDirection.Output;
            SqlParam[18] = new SqlParameter("@SalesmanID", intSalesmanID);
            SqlParam[19] = new SqlParameter("@RetailerTypeId", intRetailerTypeId);
            DataAccess.DataAccess.Instance.DBInsertCommand("prcInsUpdRetailerWithTypeInfo", SqlParam);
            IntResultCount = Convert.ToInt32(SqlParam[0].Value);
            if (SqlParam[17].Value != DBNull.Value && SqlParam[17].Value.ToString() != "")
            {
                Error = (SqlParam[17].Value).ToString();
            }
            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }



    }
    //#CC24 Start 
    public DataSet UploadRetailer(string spName)
    {
        try
        {
            SqlParam = new SqlParameter[6]; /* #CC06 length increased */
            SqlParam[0] = new SqlParameter("@RetailerDetailXML", SqlDbType.Xml);
            SqlParam[0].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(strRetailerDetailXML, XmlNodeType.Document, null));
            SqlParam[0].Direction = ParameterDirection.InputOutput;
            SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@SalesChannelID", intSalesChannelID);
            SqlParam[3] = new SqlParameter("@Password", Password);
            SqlParam[4] = new SqlParameter("@PasswordSalt", PasswordSalt);
            SqlParam[5] = new SqlParameter("@LoginUserId", UserID); /* #CC06 Added */
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase(spName, CommandType.StoredProcedure, SqlParam);

            if (((System.Data.SqlTypes.SqlXml)SqlParam[0].Value).IsNull != true)
            {
                strRetailerDetailXML = ((System.Data.SqlTypes.SqlXml)SqlParam[0].Value).Value;
            }

            else
            {
                strRetailerDetailXML = null;
            }
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

//#CC24 END

    // methods for Manage Retailer Type
    public void InsManageRetailer()
    {
        try
        {
            SqlParam = new SqlParameter[6];
            SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@Insinfo", SqlDbType.VarChar, 50);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@RetailerType", RetailerType);
            SqlParam[4] = new SqlParameter("@Status", Convert.ToInt32(Status));
            SqlParam[5] = new SqlParameter("@CompanyId", CompanyId);
            DataAccess.DataAccess.Instance.DBInsertCommand("prcInsManageRetailer", SqlParam);
            if (SqlParam[2].Value.ToString() != "")
            {
                InsError = SqlParam[2].Value.ToString();
            }
            else { InsError = null; }
            Error = Convert.ToString(SqlParam[1].Value);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void UpdManageRetailer(int condition)
    {
        try
        {
            SqlParam = new SqlParameter[8];
            SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@Insinfo", SqlDbType.VarChar, 50);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@RetailerType", RetailerType);
            SqlParam[4] = new SqlParameter("@Status", Convert.ToInt32(Status));
            SqlParam[5] = new SqlParameter("@ReatilerTypeID", RetailerID);
            SqlParam[6] = new SqlParameter("@Condition", condition);
            SqlParam[7] = new SqlParameter("@CompanyId", CompanyId);
            DataAccess.DataAccess.Instance.DBInsertCommand("prcUpdManageRetailer", SqlParam);
            if (SqlParam[2].Value.ToString() != "")
            {
                InsError = SqlParam[2].Value.ToString();
            }
            else { InsError = null; }
            Error = Convert.ToString(SqlParam[1].Value);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetManageRetailer(int condition)
    {
        try
        {
            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@ReatilerTypeID", RetailerID);
            SqlParam[3] = new SqlParameter("@Condition", condition);
            SqlParam[4] = new SqlParameter("@CompanyId", CompanyId);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetManageRetailer", CommandType.StoredProcedure, SqlParam);
            Error = Convert.ToString(SqlParam[1].Value);
            return dtResult;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion
    #region Get Retailer Info by Parameters

    public DataTable GetActivatedRetailerinfo()
    {
        try
        {
            SqlParam = new SqlParameter[1];

            SqlParam[0] = new SqlParameter("@salesmanid", intSalesmanID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetActivatedRetailerDetails", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }

    }
    public DataTable GetAllRetaileType()
    {
        try
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@SearchConditions", eSearchConditions);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetAllRetailerType", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetAllRetailer()          //This would be used in the POC
    {
        try
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@Value", value);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetAllRetailer", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    /*#CC01 STATR ADDED */
    public DataTable GetRetailerInfoNewV1()
    {
        try
        {
            SqlParam = new SqlParameter[10];
            SqlParam[0] = new SqlParameter("@RetailerID", intRetailerID);
            SqlParam[1] = new SqlParameter("@RetailerName", strRetailerName);
            SqlParam[2] = new SqlParameter("@SalesChannelId", intSalesChannelID);
            SqlParam[3] = new SqlParameter("@Type", Type);
            SqlParam[4] = new SqlParameter("@SalesmanName", strSalesmanName);
            SqlParam[4] = new SqlParameter("@SalesmanID", intSalesmanID);
            SqlParam[5] = new SqlParameter("@PageIndex", _PageIndex);
            SqlParam[6] = new SqlParameter("@PageSize", PageSize);
            SqlParam[7] = new SqlParameter("@TotalRecord", SqlDbType.BigInt);
            SqlParam[7].Direction = ParameterDirection.Output;
            SqlParam[8] = new SqlParameter("@RatailerName", RetailerName);
            SqlParam[9] = new SqlParameter("@RatailerCode", RetailerCode);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetRetailerInfo_V2", CommandType.StoredProcedure, SqlParam);
            TotalRecords = Convert.ToInt32(SqlParam[7].Value);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    /*#CC01 STATR END */
    public DataTable GetRetailerInfo()
    {
        try
        {
            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@RetailerID", intRetailerID);
            SqlParam[1] = new SqlParameter("@RetailerName", strRetailerName);
            SqlParam[2] = new SqlParameter("@SalesChannelId", intSalesChannelID);
            SqlParam[3] = new SqlParameter("@Type", Type);
            SqlParam[4] = new SqlParameter("@SalesmanName", strSalesmanName);
            SqlParam[4] = new SqlParameter("@SalesmanID", intSalesmanID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetRetailerInfo", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetRetailerInfoV2()
    {
        try
        {
            SqlParam = new SqlParameter[7];
            SqlParam[0] = new SqlParameter("@RetailerID", intRetailerID);
            SqlParam[1] = new SqlParameter("@RetailerName", strRetailerName);
            SqlParam[2] = new SqlParameter("@SalesChannelId", intSalesChannelID);
            SqlParam[3] = new SqlParameter("@Type", Type);
            SqlParam[4] = new SqlParameter("@SalesmanName", strSalesmanName);
            SqlParam[5] = new SqlParameter("@SalesmanID", intSalesmanID);
            SqlParam[6] = new SqlParameter("@LoggedSalesChannelID", LoggedInSalesChannelid);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetRetailerInfoV2", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetRetailerInfoV3()
    {
        SqlConnection objCon = new SqlConnection(ConString);//#CC15 added
        objCon.Open();
        try
        {
            SqlParam = new SqlParameter[19]; /* #CC03 Length increased */ /* #CC13 length increased from 17 to 18*/ 
            /* #CC18 length increased from 18 to 19 */
            SqlParam[0] = new SqlParameter("@PageIndex", _PageIndex);
            SqlParam[1] = new SqlParameter("@PageSize", _PageSize);
            SqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 10);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@RetailerID", intRetailerID);
            SqlParam[4] = new SqlParameter("@RetailerName", strRetailerName);
            SqlParam[5] = new SqlParameter("@SalesChannelId", intSalesChannelID);
            SqlParam[6] = new SqlParameter("@Type", Type);
            SqlParam[7] = new SqlParameter("@SalesmanName", strSalesmanName);
            SqlParam[8] = new SqlParameter("@SalesmanID", intSalesmanID);
            SqlParam[9] = new SqlParameter("@LoggedSalesChannelID", LoggedInSalesChannelid);
            SqlParam[10] = new SqlParameter("@UserID", UserID);/*#CC02 ADDED*/
            /* #CC03 Add Start */
            SqlParam[11] = new SqlParameter("@RetailerCode", RetailerCode);
            SqlParam[12] = new SqlParameter("@Status", intStatus);
            SqlParam[13] = new SqlParameter("@RetailerApproval", RetailerApproval);
            /* #CC03 Add End */
            SqlParam[14] = new SqlParameter("@DisplayMode", DisplayMode);
            /* #CC13 Add Start */
            SqlParam[15] = new SqlParameter("@StateID", StateID);
            SqlParam[16] = new SqlParameter("@SalesChannelCode", SalesChannelCode);
            SqlParam[17] = new SqlParameter("@NDID", NDID);
            /* #CC13 Add Start */
            SqlParam[18] = new SqlParameter("@FetchDataForApproval", FetchDataForApproval);/* #CC18 Added */
            //dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetRetailerInfoV3", CommandType.StoredProcedure, SqlParam);-#CC15 comented

            /*#CC15 add start*/
            DataSet dtset = new DataSet();

            SqlCommand objComm = new SqlCommand("prcGetRetailerInfoV3", objCon);
            objComm.CommandType = CommandType.StoredProcedure;
            objComm.Parameters.AddRange(SqlParam);
            objComm.CommandTimeout = 600;
            using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
            {
                obAdp.Fill(dtset);
            }
            /*#CC17 Added Started*/
            if(dtset.Tables.Count>0)
            {
                dtResult = dtset.Tables[0];
                
            }
            else
            {
                dtResult = null;
            }
            /*#CC17 Added End*/
           
            /*#CC15 end*/
            _TotalRecords = Convert.ToInt32(SqlParam[2].Value);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (objCon.State != ConnectionState.Closed)
                objCon.Close();
        }
    }

    public DataTable GetRetailerInfoWithType()
    {
        try
        {
            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@RetailerID", intRetailerID);
            SqlParam[1] = new SqlParameter("@RetailerName", strRetailerName);
            SqlParam[2] = new SqlParameter("@SalesChannelId", intSalesChannelID);
            SqlParam[3] = new SqlParameter("@Type", Type);
            SqlParam[4] = new SqlParameter("@SalesmanName", strSalesmanName);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetRetailerWithTypeInfo", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion
    #region UpdateStatus


    public Int32 UpdateStatusRetailerInfo()
    {
        try
        {

            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@RetailerID", intRetailerID);

            IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("prcUpdStatusRetailer", SqlParam);
            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    #region Check Mapped Retailer Existance
    public Int32 CheckRetailerExistence()
    {
        try
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@RetailerID", intRetailerID);

            IntResultCount = Convert.ToInt32(DataAccess.DataAccess.Instance.getSingleValues("PrcChkRetailerExistence", SqlParam));
            return IntResultCount;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    #endregion
    #endregion


    #region Manage Retailer Type Mapping

    private int _CreatedBy;
    public int CreatedBy
    {
        get { return _CreatedBy; }
        set { _CreatedBy = value; }
    }
    private int _SalesChannelTypeID;
    public int SalesChannelTypeID
    {
        get { return _SalesChannelTypeID; }
        set { _SalesChannelTypeID = value; }
    }
    public DataSet GetReatailerTypeMapping()
    {
        try
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@Condition", 0);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetRetailerTypeMapping", CommandType.StoredProcedure, SqlParam);
            Error = Convert.ToString(SqlParam[1].Value);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void InsReatailerTypeMapping()
    {
        try
        {
            SqlParam = new SqlParameter[7];
            SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@InsError", SqlDbType.VarChar, 200);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@ReatilerTypeID", RetailerTypeID);
            SqlParam[4] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
            SqlParam[5] = new SqlParameter("@CreatedBy", CreatedBy);
            SqlParam[6] = new SqlParameter("@Status", Status);
            DataAccess.DataAccess.Instance.DBInsertCommand("prcInsRetailerTypeMapping", SqlParam);
            if (SqlParam[2].Value.ToString() != "")
            {
                InsError = SqlParam[2].Value.ToString();
            }
            else { InsError = null; }
            Error = Convert.ToString(SqlParam[1].Value);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void UPDToggleReatailerTypeMapping(int ID, int UserID)
    {
        SqlParam = new SqlParameter[2];
        SqlParam[0] = new SqlParameter("@ID", ID);
        SqlParam[1] = new SqlParameter("@UserID", UserID);
        DataAccess.DataAccess.Instance.DBInsertCommand("prcUPDToggelRetailerTypeMapping", SqlParam);
    }

    #endregion




    /* #CC25 Add Start */
    public DataSet GetRetailerViewInfo()
    {
        SqlConnection objCon = new SqlConnection(ConString);
        objCon.Open();
        try
        {
            SqlParam = new SqlParameter[22]; 
            SqlParam[0] = new SqlParameter("@PageIndex", _PageIndex);
            SqlParam[1] = new SqlParameter("@PageSize", _PageSize);
            SqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 10);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@RetailerID", intRetailerID);
            SqlParam[4] = new SqlParameter("@RetailerName", strRetailerName);
            SqlParam[5] = new SqlParameter("@SalesChannelId", intSalesChannelID);
            SqlParam[6] = new SqlParameter("@Type", Type);
            SqlParam[7] = new SqlParameter("@SalesmanName", strSalesmanName);
            SqlParam[8] = new SqlParameter("@SalesmanID", intSalesmanID);
            SqlParam[9] = new SqlParameter("@LoggedSalesChannelID", LoggedInSalesChannelid);
            SqlParam[10] = new SqlParameter("@UserID", UserID);
           
            SqlParam[11] = new SqlParameter("@RetailerCode", RetailerCode);
            SqlParam[12] = new SqlParameter("@Status", intStatus);
            SqlParam[13] = new SqlParameter("@RetailerApproval", RetailerApproval);
           
            SqlParam[14] = new SqlParameter("@DisplayMode", DisplayMode);
           
            SqlParam[15] = new SqlParameter("@StateID", StateID);
            SqlParam[16] = new SqlParameter("@SalesChannelCode", SalesChannelCode);
            SqlParam[17] = new SqlParameter("@NDID", NDID);
        
            SqlParam[18] = new SqlParameter("@Out_Param", SqlDbType.Int);
            SqlParam[18].Direction = ParameterDirection.Output;
            SqlParam[19] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 2000);
            SqlParam[19].Direction = ParameterDirection.Output;
            SqlParam[20] = new SqlParameter("@BrandId", BrandId);
            SqlParam[21] = new SqlParameter("@ProductCategoryId", ProductCategoryId);
            DataSet dsResult = new DataSet();

            SqlCommand objComm = new SqlCommand("prcGetRetailerViewInfo", objCon);
            objComm.CommandType = CommandType.StoredProcedure;
            objComm.Parameters.AddRange(SqlParam);
            objComm.CommandTimeout = 600;
            using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
            {
                obAdp.Fill(dsResult);
            }

            /*if (dsResult.Tables.Count > 0)
            {
                dtResult = dsResult.Tables[0];
            }
            else
            {
                dtResult = null;
            }     */      
            _TotalRecords = Convert.ToInt32(SqlParam[2].Value);
            intOutParam = Convert.ToInt16(SqlParam[18].Value);
            Error = Convert.ToString(SqlParam[19].Value);

            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (objCon.State != ConnectionState.Closed)
                objCon.Close();
        }
    }

    public DataSet GetRetailerApprovalInfo()
    {
        SqlConnection objCon = new SqlConnection(ConString);
        objCon.Open();
        try
        {
            SqlParam = new SqlParameter[20];
            SqlParam[0] = new SqlParameter("@PageIndex", _PageIndex);
            SqlParam[1] = new SqlParameter("@PageSize", _PageSize);
            SqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 10);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@RetailerID", intRetailerID);
            SqlParam[4] = new SqlParameter("@RetailerName", strRetailerName);
            SqlParam[5] = new SqlParameter("@SalesChannelId", intSalesChannelID);
            SqlParam[6] = new SqlParameter("@Type", Type);
            SqlParam[7] = new SqlParameter("@SalesmanName", strSalesmanName);
            SqlParam[8] = new SqlParameter("@SalesmanID", intSalesmanID);
            SqlParam[9] = new SqlParameter("@LoggedSalesChannelID", LoggedInSalesChannelid);
            SqlParam[10] = new SqlParameter("@UserID", UserID);

            SqlParam[11] = new SqlParameter("@RetailerCode", RetailerCode);
            SqlParam[12] = new SqlParameter("@Status", intStatus);
            SqlParam[13] = new SqlParameter("@RetailerApproval", RetailerApproval);

            SqlParam[14] = new SqlParameter("@DisplayMode", DisplayMode);

            SqlParam[15] = new SqlParameter("@StateID", StateID);
            SqlParam[16] = new SqlParameter("@SalesChannelCode", SalesChannelCode);
            SqlParam[17] = new SqlParameter("@NDID", NDID);

            SqlParam[18] = new SqlParameter("@Out_Param", SqlDbType.Int);
            SqlParam[18].Direction = ParameterDirection.Output;
            SqlParam[19] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 2000);
            SqlParam[19].Direction = ParameterDirection.Output;

            DataSet dsResult = new DataSet();

            SqlCommand objComm = new SqlCommand("prcGetRetailerApprovalInfo", objCon);
            objComm.CommandType = CommandType.StoredProcedure;
            objComm.Parameters.AddRange(SqlParam);
            objComm.CommandTimeout = 600;
            using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
            {
                obAdp.Fill(dsResult);
            }
            _TotalRecords = Convert.ToInt32(SqlParam[2].Value);
            intOutParam = Convert.ToInt16(SqlParam[18].Value);
            Error = Convert.ToString(SqlParam[19].Value);

            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (objCon.State != ConnectionState.Closed)
                objCon.Close();
        }
    }

    /* #CC25 Add End */
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

    ~RetailerData()
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




    public int OrgnhierarchyID { get; set; }

    private byte _ISOpeningStock = 2;
    public byte ISOpeningStock { get { return _ISOpeningStock; } set { _ISOpeningStock = value; } }
    /*#CC04 ADDED START*/
    public DataTable CheckRetailer()
    {
        try
        {

            SqlParam = new SqlParameter[7];
            SqlParam[0] = new SqlParameter("@CityID", CityID);
            SqlParam[1] = new SqlParameter("@MobileNumber", MobileNumber);
            SqlParam[2] = new SqlParameter("@PhoneNumber", PhoneNumber);
            SqlParam[3] = new SqlParameter("@RetailerName", RetailerName);
            SqlParam[4] = new SqlParameter("@Address1", Address1);
            SqlParam[5] = new SqlParameter("@OutParam", SqlDbType.Int);
            SqlParam[5].Direction = ParameterDirection.Output;
            SqlParam[6] = new SqlParameter("@RetailerID", RetailerID); /* #CC09 Added */
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("PrcCheckRetailer", CommandType.StoredProcedure, SqlParam);
            OutPutResult = Convert.ToInt16(SqlParam[5].Value);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    /*#CC04 ADDED END*/
    public DataTable GetRetailerByOrgHeirarchy()
    {
        try
        {

            SqlParam = new SqlParameter[4];
            SqlParam[0] = new SqlParameter("@UserID", UserID);
            SqlParam[1] = new SqlParameter("@isOpeningStock", _ISOpeningStock);
            SqlParam[2] = new SqlParameter("@LoginBaseEntityType", OtherEntityID);
            SqlParam[3] = new SqlParameter("@CompanyId", CompanyId); 

            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetRetailerByOrgnhierarchyID", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetRetaierListforReport()
    {
        try
        {

            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@UserID", UserID);
            SqlParam[1] = new SqlParameter("@otherEntityID", OtherEntityID);
            SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetRetailerReport", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }


    public DataSet RetailersCodeDownload()
    {
        try
        {
            /*#CC26 Added*/
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@CompanyId", CompanyId); /*#CC26 Added*/
            return DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcRetailersCodeDownload", CommandType.StoredProcedure, /*#CC26 Added*/SqlParam);

        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    private byte _Status = 2;

    public DataTable GetISPByRetailerID()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@RetailerID", RetailerID);
            SqlParam[1] = new SqlParameter("@Status", Status);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetISPByRetailerID", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    private int _ISPID = 0;

    public int ISPID
    {
        get { return _ISPID; }
        set { _ISPID = value; }
    }


    public DataTable GetISPsInfo()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@ISPID", ISPID);
            SqlParam[1] = new SqlParameter("@RetailerID", RetailerID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcISPsInfo", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetISPsInfoByRetailer()
    {
        try
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@RetailerID", RetailerID);
            SqlParam[1] = new SqlParameter("@CompanyId", CompanyId);
            SqlParam[2] = new SqlParameter("@UserID", UserID);

            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetISPInfoByRetailer", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    private byte _IsZeroStock = 1;

    public byte IsZeroStock
    {
        get { return _IsZeroStock; }
        set { _IsZeroStock = value; }
    }
    private DateTime? dtOpeningStockDate;
    public DateTime? OpeningStockDate
    {
        get { return dtOpeningStockDate; }
        set { dtOpeningStockDate = value; }
    }
    private string _xmlList;
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
    public Int16 InsertOpeningStockWithZero(DataTable Tvp)
    {
        Int16 Result;
        SqlParam = new SqlParameter[9];
        SqlParam[0] = new SqlParameter("@RetailerID", RetailerID);
        SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
        SqlParam[1].Direction = ParameterDirection.Output;
        SqlParam[2] = new SqlParameter("@OpeningStockDate", OpeningStockDate);
        SqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.Int);
        SqlParam[3].Direction = ParameterDirection.Output;
        SqlParam[4] = new SqlParameter("@IsZeroStock", IsZeroStock);
        SqlParam[5] = new SqlParameter("@TvpStock", SqlDbType.Structured);
        SqlParam[5].Value = Tvp;
        SqlParam[6] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
        SqlParam[6].Direction = ParameterDirection.Output;
        

        DataAccess.DataAccess.Instance.DBInsertCommand("prcInsOpeningStockForRetailer", SqlParam);
        Error = Convert.ToString(SqlParam[1].Value);
        Result = Convert.ToInt16(SqlParam[3].Value);

        return Result;
    }

    public DataTable GetISPsInfoByParam()
    {
        try
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@ISPID", ISPID);
            SqlParam[1] = new SqlParameter("@RetailerID", RetailerID);
            SqlParam[2] = new SqlParameter("@UserID", UserID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcISPsInfoByParam", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }


    public DataTable ViewSalesEntrySummary()
    {
        try
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@UserID", UserID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetTertiarySalesSummary", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable ViewStockSummary()
    {
        try
        {
            SqlParam = new SqlParameter[7];
            SqlParam[0] = new SqlParameter("@UserID", UserID);
            SqlParam[1] = new SqlParameter("@SelectedRetailerid", RetailerID);
            SqlParam[2] = new SqlParameter("@value", value);
            SqlParam[3] = new SqlParameter("@SkuID", SkuID);
            SqlParam[4] = new SqlParameter("@PageIndex", _PageIndex);
            SqlParam[5] = new SqlParameter("@PageSize", PageSize);
            SqlParam[6] = new SqlParameter("@TotalRecord", SqlDbType.BigInt);
            SqlParam[6].Direction = ParameterDirection.Output;
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetTertiaryStockSummary", CommandType.StoredProcedure, SqlParam);
            if (SqlParam[6].Value != System.DBNull.Value)
                TotalRecords = Convert.ToInt32(SqlParam[6].Value);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }


    private DateTime _DateFrom = Convert.ToDateTime(DateTime.MinValue.Date.ToShortDateString());
    private DateTime _DateTo = Convert.ToDateTime(DateTime.MaxValue.Date.ToShortDateString());


    public DateTime DateFrom { get { return _DateFrom; } set { _DateFrom = value; } }
    public DateTime DateTo { get { return _DateTo; } set { _DateTo = value; } }
    public string SerialNo { get; set; }
    public Byte HeadID { get; set; }


    private Int32 _TotalRecords = 0;
    public Int32 TotalRecords
    {
        get
        {
            return _TotalRecords;
        }
        set
        {
            _TotalRecords = value;
        }
    }
    private int _PageIndex = 0;
    public int PageIndex
    {
        get
        {
            return _PageIndex;
        }
        set
        {
            _PageIndex = value;
        }
    }
    private int _PageSize = 10;
    public int PageSize
    {
        get
        {
            return _PageSize;
        }
        set
        {
            _PageSize = value;
        }
    }

    public DataTable ViewSalesEntryDetail()
    {
        try
        {
            SqlParam = new SqlParameter[8];
            SqlParam[0] = new SqlParameter("@UserID", UserID);
            SqlParam[1] = new SqlParameter("@HeadID", HeadID);
            SqlParam[2] = new SqlParameter("@DateFrom", _DateFrom);
            SqlParam[3] = new SqlParameter("@DateTo", _DateTo);
            SqlParam[4] = new SqlParameter("@SerialNo", SerialNo);
            SqlParam[5] = new SqlParameter("@PageIndex", _PageIndex);
            SqlParam[6] = new SqlParameter("@PageSize", PageSize);
            SqlParam[7] = new SqlParameter("@TotalRecord", SqlDbType.BigInt);
            SqlParam[7].Direction = ParameterDirection.Output;

            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetTertiarySalesDetail", CommandType.StoredProcedure, SqlParam);
            TotalRecords = Convert.ToInt32(SqlParam[7].Value);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }


    public DataTable GetAllActiveSKU()
    {
        try
        {
            DataTable dtSKU;
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@ModelId", ModelId);
            SqlParam[1] = new SqlParameter("@CompanyId", CompanyId);
            dtSKU = DataAccess.DataAccess.Instance.GetTableFromDatabase("PrcGetAllActiveSKU", CommandType.StoredProcedure, SqlParam);
            return dtSKU;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /* #CC13 Add Start */
    public DataSet GetRetailerMappingInfo()
    {
        SqlConnection objCon = new SqlConnection(ConString);//#CC15 added
        objCon.Open();
        try
        {
            DataSet dsResult = new DataSet();
            SqlParam = new SqlParameter[9];
            SqlParam[0] = new SqlParameter("@NDID", NDID);
            SqlParam[1] = new SqlParameter("@RDSID", RDSID);
            SqlParam[2] = new SqlParameter("@StateID", StateID);
            SqlParam[3] = new SqlParameter("@RetailerName", strRetailerName);
            SqlParam[4] = new SqlParameter("@RetailerCode", strRetailerCode);
            SqlParam[5] = new SqlParameter("@Status", _intStatus);
            SqlParam[6] = new SqlParameter("@TotalRecord", SqlDbType.BigInt);
            SqlParam[6].Direction = ParameterDirection.Output;
            SqlParam[7] = new SqlParameter("@SalesChannelCode", SalesChannelCode);
            SqlParam[8] = new SqlParameter("@UserID", UserID);/*#CC16 Added*/
            //dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetRetailerMappingInfo", CommandType.StoredProcedure, SqlParam);#CC15 comented

            /*#CC15 add start*/

            SqlCommand objComm = new SqlCommand("prcGetRetailerMappingInfo", objCon);
            objComm.CommandType = CommandType.StoredProcedure;
            objComm.Parameters.AddRange(SqlParam);
            objComm.CommandTimeout = 600;
            using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
            {
                obAdp.Fill(dsResult);
            }
            /*#CC15 end*/
            TotalRecords = Convert.ToInt32(SqlParam[6].Value);
            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (objCon.State != ConnectionState.Closed)
                objCon.Close();
        }
    }
    /* #CC13 Add End */
    /*#CC21 start*/
    public int SaveImgSaperateByProcess()
    {
        try
        {
            int result;
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@RetailerId", RetailerID);
            param[1] = new SqlParameter("@webUserId", UserID);
            param[2] = new SqlParameter("@XMLFile", WOFileXML);
           
            param[3] = new SqlParameter("@Out_Param", SqlDbType.NVarChar, 50);
            param[3].Direction = ParameterDirection.Output;
            result = DataAccess.DataAccess.Instance.DBInsertCommand("prcImageUploadForRetailer", param);

            return result = Convert.ToInt32(param[3].Value);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public DataTable GetRetailerImageInfo()
    {
        try
        {
            DataTable dtRetailerImage;
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@RetailerId", RetailerID);
            SqlParam[1] = new SqlParameter("@webUserId", UserID);
            dtRetailerImage = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcImageAndPathForRetailer", CommandType.StoredProcedure, SqlParam);
            return dtRetailerImage;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }/*#CC21 end*/
    /*#CC22 start*/
    public DataTable GetRetailerViewImageInfo()
    {
        try
        {
            DataTable dtRetailerImage;
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@RetailerId", RetailerID);
            SqlParam[1] = new SqlParameter("@LoginUserId", UserID);
            dtRetailerImage = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetRetailerImagePath", CommandType.StoredProcedure, SqlParam);
            return dtRetailerImage;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    } /*#CC22 end*/
    /*#CC23 start*/
    public DataTable getZoneDetail()
    {
        try
        {
            SqlParam = new SqlParameter[1];

         
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetZoneMaster", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }

    }
    public DataSet SaveRetailerMarketingData()
    {
        Int16 Result;
        SqlParam = new SqlParameter[7];
        SqlParam[0] = new SqlParameter("@SessionID", SessionID);
        SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.Int);
        SqlParam[1].Direction = ParameterDirection.Output;
        SqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
        SqlParam[2].Direction = ParameterDirection.Output;
        SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
        SqlParam[3].Direction = ParameterDirection.Output;
        SqlParam[4] = new SqlParameter("@UserId", UserID);
        SqlParam[5] = new SqlParameter("@OriginalFileName", OriginalFileName);
        SqlParam[6] = new SqlParameter("@UniqueFileName", UniqueFileName);
        dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcBulkRetailerMarketingDataSave", CommandType.StoredProcedure, SqlParam);

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

    public DataTable GetRetailerMarketingData()
    {
        try
        {
            SqlParam = new SqlParameter[7];
            SqlParam[0] = new SqlParameter("@ZoneId", ZoneId);
            SqlParam[1] = new SqlParameter("@ActiveInActiveFlag", ActiveInActiveFlag);
            SqlParam[2] = new SqlParameter("@CSAType", CsaType);
            SqlParam[3] = new SqlParameter("@FromDate", FromDate);
            SqlParam[4] = new SqlParameter("@ToDate", ToDate);
            SqlParam[5] = new SqlParameter("@UserId", UserID);
            SqlParam[6] = new SqlParameter("@Flag", Flag);

            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetRetailerMarketingData", CommandType.StoredProcedure, SqlParam);

            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }

    }
    public DataSet GetRetailerReferenceCodeReferencecode()
    {
        try
        {
            SqlParam = new SqlParameter[2];


            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetRetailerReferencecode", CommandType.StoredProcedure, SqlParam);

            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }

    }/*CC23 END*/
}