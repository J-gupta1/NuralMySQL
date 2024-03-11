﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using DataAccess;
/*
 * =============================================================================================================================
 * Change Log:
 * DD-MMM-YYYY, Name , #CCXX - Description
 * 20-Aug-2016, Sumit Maurya, #CC01, New method created to get Hierarchy and OrgnHierarchy Data and Report OrgnHierarchyMapping Data.
 * 23-Aug-2016, Sumit Maurya, #CC02, Properties created and parameter supplied in method GetOrgnHierarchyInfo().
 * 02-Sep-2016, Sumit Maurya, #CC03, New methods created for CBH to state mapping interface.
 * 03-Oct-2016, Karam Chand Sharma, #CC04, New methods created for CBH to ND mapping interface.
 * 12-Oct-2016, Sumit Maurya, #CC05, New properties, created for and parametr supplied in methods..
 * 18-Oct-2016, Sumit Maurya, #CC06, New properties method created for OrgHierarchy user upload.
 * 21-Oct-2016, Sumit Maurya, #CC07, New properties methods created for ParallelOrgnHierarchy.
 * 24-Oct-2016, Sumit Maurya, #CC08, New methods and properties created for ParallelOrgnHierarchyBrandMapping.
 * 08-Aug-2017, Vijay Katiyar, #CC09, Added output parameter due to wrong result
 * 17-Aug-2017, Sumit Maurya, #CC10, Length increased and userid value suplied to get data accordingly.
 * 26-Nov-2018,Vijay Kumar Prajapati,#CC11, Add Userid and Out_Error Parametre in Mathod.
 * 01-April-2020,Vijay Kumar Prajapati,#CC12,Added CompanyId in Method.
 * ------------------------------------------------------------------------------------------------------------------------------
 */


public class OrgHierarchyData : IDisposable
{
    private int intOrgnhierarchyID;
    private Int16 intSalesChanelTypeID;
    private Int32 intHierarchyLevelID;
    private Int16 intParentHierarchyLevelID;
    private string strLocationName;
    private string strParentLocationName;
    private string strLocationCode;
    private int intParentOrgnhierarchyID;
    private bool blnStatus;
    private bool blnAllownotOwn;
    private bool blnallowhierarchy;
    DataTable dtOrgInfo, dtOrgType;
    SqlParameter[] SqlParam;
    Int32 IntResultCount = 0;
    DataSet dsOrgInfo;
    public bool AllownotOwn
    {
        get { return blnAllownotOwn; }
        set { blnAllownotOwn = value; }
    }
    public Int32 UserID
    {
        get;
        set;
    }
    public Int16 SalesChanelTypeID
    {
        get { return intSalesChanelTypeID; }
        set { intSalesChanelTypeID = value; }
    }
    public string LocationName
    {
        get { return strLocationName; }
        set { strLocationName = value; }
    }
    public string ParentLocationName
    {
        get { return strParentLocationName; }
        set { strParentLocationName = value; }
    }
    public string LocationCode
    {
        get { return strLocationCode; }
        set { strLocationCode = value; }
    }
    public int OrgnhierarchyID
    {
        get { return intOrgnhierarchyID; }
        set { intOrgnhierarchyID = value; }
    }
    public int ParentOrgnhierarchyID
    {
        get { return intParentOrgnhierarchyID; }
        set { intParentOrgnhierarchyID = value; }
    }
    public Int32 HierarchyLevelID
    {
        get { return intHierarchyLevelID; }
        set { intHierarchyLevelID = value; }
    }
    public Int16 ParentHierarchyLevelID
    {
        get { return intParentHierarchyLevelID; }
        set { intParentHierarchyLevelID = value; }
    }
    public bool Status
    {
        get { return blnStatus; }
        set { blnStatus = value; }
    }
    public bool allowhierarchy
    {
        get { return blnallowhierarchy; }
        set { blnallowhierarchy = value; }
    }

    public Int16 SearchMode
    {
        get;
        set;
    }
    private int _OrgnhierachyID_DSR;
    public int OrgnhierachyID_DSR
    {
        get { return _OrgnhierachyID_DSR; }
        set { _OrgnhierachyID_DSR = value; }
    }

    private string _txtMonth;
    public string TxtMonth
    {
        get { return _txtMonth; }
        set { _txtMonth = value; }
    }
    private string _txtYear;
    public string TxtYear
    {
        get { return _txtYear; }
        set { _txtYear = value; }
    }
    private string _txtPathLoc;
    public string TxtPathLoc
    {
        get { return _txtPathLoc; }
        set { _txtPathLoc = value; }
    }
    private string _txtFileName;
    public string TxtFileName
    {
        get { return _txtFileName; }
        set { _txtFileName = value; }
    }
    private int _CreatedBy;
    public int CreatedBy
    {
        get { return _CreatedBy; }
        set { _CreatedBy = value; }
    }
    //=======================
    //Proprty for Search DSR
    //=======================
    string _Src_HierarchyLvlNm;
    public string Src_HierarchyLvlNm
    {
        get { return _Src_HierarchyLvlNm; }
        set { _Src_HierarchyLvlNm = value; }
    }
    string _Src_LocatioNm;
    public string Src_LocatioNm
    {
        get { return _Src_LocatioNm; }
        set { _Src_LocatioNm = value; }
    }
    int _Src_month;
    public int Src_month
    {
        get { return _Src_month; }
        set { _Src_month = value; }
    }
    int _Src_year;
    public int Src_year
    {
        get { return _Src_year; }
        set { _Src_year = value; }
    }
    int _PageIndex;
    public int PageIndex
    {
        get { return _PageIndex; }
        set { _PageIndex = value; }
    }
    int _PageSize;
    public int PageSize
    {
        get { return _PageSize; }
        set { _PageSize = value; }
    }
    private int _TotalRecords;
    public int TotalRecords
    {
        get { return _TotalRecords; }
        set { _TotalRecords = value; }
    }

    public int ReturnValue
    {
        get;
        set;
    }
    /*#CC04 START ADDED */
    public int NDID
    {
        get;
        set;
    }
    /*#CC04 END ADDED */

    //public string _Error;
    //public string Error
    //{
    //    get { return Error; }
    //    set { Error = value; }
    //}
    //======================= End Property

    /* #CC02 Add Start */

    public string UserName
    {
        get;
        set;
    }

    public string ParentCode
    {
        get;
        set;
    }

    /* #CC02 Add End */
    /* #CC03 Add Start */
    public int intOrgnHierarchyStateMappingID
    {
        get;
        set;
    }
    public int intOrgnHierarchySaleChannelMappingID
    {
        get;
        set;
    }
    /* #CC05 Add Start */
    public int SalesChannelID
    {
        get;
        set;
    }
    public DataTable DTCBHID;
    DataTable dtResult;
    public int intOrgnCBHNDMappingID
    {
        get;
        set;
    }
    /* #CC05 Add End */

    public string Error
    {
        get;
        set;
    }
    public int StateID
    {
        get;
        set;
    }
    public Int16 intStatus
    {
        get;
        set;
    }
    /* #CC03 Add End */
    /* #CC06 Add Start */
    public string SessionID
    {
        get;
        set;
    }
    public int intOutParam
    {
        get;
        set;
    }
    public int intType
    {
        get;
        set;
    }
    /* #CC06 Add End */
    /* #CC07 Add Start */
    public string FullName
    {
        get;
        set;
    }


    public string PasswordSalt
    {
        get;
        set;
    }
    public string Password
    {
        get;
        set;
    }
    public string EmailID
    {
        get;
        set;
    }
    public string MobileNumber
    {
        get;
        set;
    }
    public int ParallelOrgnHierarchyID
    {
        get;
        set;
    }
    /* #CC07 Add End */
    /* #CC08 Add Start */
    public int ParallelOrgnSalesChannelBrandMappingID
    {
        get;
        set;
    }
    public int BrandID
    {
        get;
        set;
    }

    public string SalesChannelCode
    {
        get;
        set;
    }
    /* #CC08 Add End */

    public Int32 CompanyId { get; set; }/*#CC12 Added*/
    public string Lat { set; get; }
    public string Long { set; get; }
    public string GeoRadius { set; get; }
    public Int32 CityID { get; set; }
    public string BrandCategoryIds { set; get; }
   
    public DataTable GetOrgHierarchy()
    {
        try
        {
            SqlParam = new SqlParameter[4]; /*#CC09 length increased from 2 to 3*/
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChanelTypeID);
            SqlParam[1] = new SqlParameter("@searchmode", SearchMode);
            SqlParam[2] = new SqlParameter("@UserID", UserID); /* #CC09 Added. */
            SqlParam[3] = new SqlParameter("@CompanyId", CompanyId); 

            dtOrgInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetLocationsBySalesChannelTypeID", CommandType.StoredProcedure, SqlParam);
            return dtOrgInfo;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }


    public DataTable GetAllHierarchyLevel()
    {
        try
        {
            /*#CC12 Added Started*/
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@CompanyId", CompanyId);/*#CC12 Added End*/
            dtOrgInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetHierarchyLevelMaster", CommandType.StoredProcedure,/*#CC12 Added */SqlParam);
            return dtOrgInfo;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetHierarchyLevelConditional(Int16 ConditionFlag)
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@condition", ConditionFlag);
            SqlParam[1] = new SqlParameter("@CompanyId", CompanyId);/*#CC12 Added End*/
            dtOrgInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetHierarchyLevelMaster", CommandType.StoredProcedure, SqlParam);
            return dtOrgInfo;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetAllLowerHierarchyLevel()
    {
        try
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@HierarchyLevelId", HierarchyLevelID);
            SqlParam[1] = new SqlParameter("@AllownotOwn", blnAllownotOwn);
            SqlParam[2] = new SqlParameter("@allowhierarchy", blnallowhierarchy);
            dtOrgInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetLowerHierarchyLevelMaster", CommandType.StoredProcedure, SqlParam);
            return dtOrgInfo;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetAllHierarchyLevelLocation()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@HierarchyLevelId", intHierarchyLevelID);
            SqlParam[1] = new SqlParameter("@CompanyId", CompanyId);/*#CC12 Added End*/
            dtOrgInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetHierarchyLevelLocationMaster", CommandType.StoredProcedure, SqlParam);
            return dtOrgInfo;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetSelectedHierachyID_forDSR()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@HierarchyLevelId_FromDSR", intHierarchyLevelID);
            SqlParam[1] = new SqlParameter("@UserID", UserID);
            dtOrgInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGet_SelectedHierarchyLevel_forDSR", CommandType.StoredProcedure, SqlParam);
            return dtOrgInfo;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void InsertdataToUploadDSR()
    {
        string FilePath = TxtPathLoc + TxtFileName;
        SqlParam = new SqlParameter[6];
        SqlParam[0] = new SqlParameter("@OrgnhierachyID", OrgnhierachyID_DSR);
        SqlParam[1] = new SqlParameter("@TxtMomth", TxtMonth);
        SqlParam[2] = new SqlParameter("@TxtYear", TxtYear);
        SqlParam[3] = new SqlParameter("@FilePath", FilePath);
        SqlParam[4] = new SqlParameter("@Status", 1);
        SqlParam[5] = new SqlParameter("@CreatedBy", CreatedBy);
        DataAccess.DataAccess.Instance.DBInsertCommand("PrcInsert_To_DSRUPload", SqlParam);
    }
    public DataTable getDSRDetail(int Condition)
    {
        try
        {
            SqlParam = new SqlParameter[10];
            SqlParam[0] = new SqlParameter("@condition", Condition);
            SqlParam[1] = new SqlParameter("@HierarchyLevelName", Src_HierarchyLvlNm);
            SqlParam[2] = new SqlParameter("@LocationName", Src_LocatioNm);
            SqlParam[3] = new SqlParameter("@DSRMonth", Src_month);
            SqlParam[4] = new SqlParameter("@DSRYear", Src_year);
            SqlParam[5] = new SqlParameter("@PageIndex", PageIndex);
            SqlParam[6] = new SqlParameter("@PageSize", PageSize);
            SqlParam[7] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            SqlParam[7].Direction = ParameterDirection.Output;
            SqlParam[8] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[8].Direction = ParameterDirection.Output;
            SqlParam[9] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            SqlParam[9].Direction = ParameterDirection.Output;
            dtOrgInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("DSR_Details_Select", CommandType.StoredProcedure, SqlParam);
            TotalRecords = Convert.ToInt32(SqlParam[9].Value);
            //Error = Convert.ToString(SqlParam[7].Value);
            return dtOrgInfo;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetOwnHierarchyLevelLocation()
    {
        try
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@HierarchyLevelId", intHierarchyLevelID);
            dtOrgInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetHierarchyLevelLocationMasterLaggard", CommandType.StoredProcedure, SqlParam);
            return dtOrgInfo;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public Int32 InsertUpdateOrgnHierarchyinfo()
    {
        try
        {
            SqlParam = new SqlParameter[16];/*#CC12 Added 9 to 10*/
            SqlParam[0] = new SqlParameter("@OrgnhierarchyID", intOrgnhierarchyID);
            SqlParam[0].Direction = ParameterDirection.InputOutput;
            SqlParam[1] = new SqlParameter("@HierarchyLevelID", intHierarchyLevelID);
            SqlParam[2] = new SqlParameter("@ParentOrgnhierarchyID", intParentOrgnhierarchyID);
            SqlParam[3] = new SqlParameter("@LocationName", strLocationName);
            SqlParam[4] = new SqlParameter("@LocationCode", strLocationCode);
            SqlParam[5] = new SqlParameter("@Status", Status);
            SqlParam[6] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[6].Direction = ParameterDirection.Output;
            SqlParam[7] = new SqlParameter("@UserId", UserID);/*#CC11 Added*/
            SqlParam[8] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);/*#CC11 Added*/
            SqlParam[8].Direction = ParameterDirection.Output;/*#CC11 Added*/
            SqlParam[9] = new SqlParameter("@CompanyId", CompanyId);/*#CC12 Added*/
            SqlParam[10] = new SqlParameter("@Lat", Lat);
            SqlParam[11] = new SqlParameter("@Long", Long);
            SqlParam[12] = new SqlParameter("@GeoRadius", GeoRadius);
            SqlParam[13] = new SqlParameter("@StateID", StateID);
            SqlParam[14] = new SqlParameter("@CityID", CityID);
            SqlParam[15] = new SqlParameter("@selectedBrandCatIDs", BrandCategoryIds);
            DataAccess.DataAccess.Instance.DBInsertCommand("PrcInsUpdOrgnHierarchyInfo", SqlParam);
            IntResultCount = Convert.ToInt32(SqlParam[0].Value);
            ReturnValue = Convert.ToInt32(SqlParam[6].Value);
            Error = Convert.ToString(SqlParam[8].Value);/*#CC11 Added*/
            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public DataTable GetOrgnHierarchyInfo()
    {
        try
        {
            SqlParam = new SqlParameter[11]; /* #CC02 Length increased*/
            SqlParam[0] = new SqlParameter("@HierarchyLevel", HierarchyLevelID);
            SqlParam[1] = new SqlParameter("@LocationName", LocationName);
            SqlParam[2] = new SqlParameter("@LocationCode", LocationCode);
            SqlParam[3] = new SqlParameter("@ParentHierarchyLevelID", ParentHierarchyLevelID);  //Pankaj Dhingra
            SqlParam[4] = new SqlParameter("@ParentLocationName", ParentLocationName); //Pankaj Dhingra
            /* #CC02 Add Start */
            SqlParam[5] = new SqlParameter("@PageSize", PageSize);
            SqlParam[6] = new SqlParameter("@PageIndex", PageIndex);
            SqlParam[7] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            SqlParam[7].Direction = ParameterDirection.Output;
            SqlParam[8] = new SqlParameter("@ParentCode", ParentCode);
            SqlParam[9] = new SqlParameter("@UserName", UserName);
            SqlParam[10] = new SqlParameter("@CompanyId", CompanyId);/*#CC12 Added*/
            /* #CC02 Add End */
            dtOrgInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("PrcGetOrgnHierarchyInfoByParameters", CommandType.StoredProcedure, SqlParam);
            TotalRecords = Convert.ToInt32(SqlParam[7].Value); /* #CC02 Added */
            return dtOrgInfo;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    public DataTable GetOrgnHierarchyInfo(Int32 OrgnhierarchyID)
    {
        try
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@OrgnhierarchyID", OrgnhierarchyID);
            dtOrgInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("PrcGetOrgnHierarchyInfoByOrgnhierarchyID", CommandType.StoredProcedure, SqlParam);
            return dtOrgInfo;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }


        public Int32 UpdateStatusOrgnHierarchyInfo()
        {
            try
            {
                /*#CC09 Commented start*/
                //SqlParam = new SqlParameter[1];
                //SqlParam[0] = new SqlParameter("@OrgnhierarchyID", intOrgnhierarchyID);
                //IntResultCount = DataAccess.Instance.DBInsertCommand("PrcUpdStatusOrgnHierarchy", SqlParam);
                //return IntResultCount;
                /*#CC09 Commented End*/
                /*#CC09 Added start*/
                int Result = 0;
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@OrgnhierarchyID", intOrgnhierarchyID);
                SqlParam[1] = new SqlParameter("@Out_Param",SqlDbType.TinyInt);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.NVarChar,500);/*#CC11 Added*/
                SqlParam[2].Direction = ParameterDirection.Output;/*#CC11 Added*/
                SqlParam[3] = new SqlParameter("@UserId", UserID);/*#CC11 Added*/

                DataAccess.DataAccess.Instance.DBInsertCommand("PrcUpdStatusOrgnHierarchy", SqlParam);
                    Result = Convert.ToInt32(SqlParam[1].Value);
                    Error = Convert.ToString(SqlParam[2].Value);/*#CC11 Added*/
                return Result;
                /*#CC09 Added End*/
            }
            catch (Exception ex)
            {
                throw ex;
            }

    }
    public Int32 CheckStatusForExistLocation()
    {
        try
        {
            int Result = 0;
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@OrgnhierarchyID", intOrgnhierarchyID);
            SqlParam[1] = new SqlParameter("@ResultOut", SqlDbType.Int);
            SqlParam[1].Direction = ParameterDirection.Output;

            IntResultCount = (DataAccess.DataAccess.Instance.DBInsertCommand("PrcChkExistenceLocationStatus", SqlParam));
            Result = Convert.ToInt32(SqlParam[1].Value);
            return Result;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }


    /* #CC01 Add Start */
    public DataSet GetHierarchyandOrgnHierarchyData()
    {
        try
        {
            DataSet dsResult = new DataSet();
            SqlParam = new SqlParameter[4];
            SqlParam[0] = new SqlParameter("@HierarchyLevelID", HierarchyLevelID);
            SqlParam[1] = new SqlParameter("@OutError", SqlDbType.VarChar, 500);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@CompanyId", CompanyId);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetHierarchyandOrgnHierarchyData", CommandType.StoredProcedure, SqlParam);
            TotalRecords = Convert.ToInt32(SqlParam[2].Value);
            return dsResult;
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    public DataTable GetReportOrgnHierarchyMappingData()
    {
        try
        {
            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@HierarchyLevelID", HierarchyLevelID);
            SqlParam[1] = new SqlParameter("@OrgnHierarchyID", OrgnhierarchyID);
            SqlParam[2] = new SqlParameter("@OutError", SqlDbType.VarChar, 500);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
            SqlParam[3].Direction = ParameterDirection.Output;
            SqlParam[4] = new SqlParameter("@CompanyId", CompanyId);
            dtOrgInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcReportOrgnHierarchyMapping", CommandType.StoredProcedure, SqlParam);
            TotalRecords = Convert.ToInt32(SqlParam[3].Value);
            return dtOrgInfo;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }

    /* #CC01 Add End */

    /* #CC03 Add Start */
    public DataTable GetOrgnHierarchyData()
    {
        try
        {
            DataTable dtResult = new DataTable();
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@Status", intStatus);
            SqlParam[1] = new SqlParameter("@HierarchyLevelID", HierarchyLevelID);
            SqlParam[2] = new SqlParameter("@OutError", SqlDbType.VarChar, 500);
            SqlParam[2].Direction = ParameterDirection.Output;
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetOrgnHierarchyData", CommandType.StoredProcedure, SqlParam);
            if (SqlParam[2].Value != DBNull.Value && SqlParam[2].Value.ToString() != "")
            {
                Error = (SqlParam[2].Value).ToString();
            }
            return dtResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet GetOrgnhierarchyStateMappingData()
    {
        try
        {
            DataSet dsResult = new DataSet();
            SqlParam = new SqlParameter[6];
            SqlParam[0] = new SqlParameter("@OrgnhierarchyID", OrgnhierarchyID);
            SqlParam[1] = new SqlParameter("@OutError", SqlDbType.VarChar, 1000);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@PageSize", PageSize);
            SqlParam[4] = new SqlParameter("@PageIndex", PageIndex);

            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetOrgnhierarchyStateMapping", CommandType.StoredProcedure, SqlParam);
            TotalRecords = Convert.ToInt32(SqlParam[2].Value);
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

    public int InsertOrgnStateMapping()
    {
        try
        {
            SqlParam = new SqlParameter[5];

            SqlParam[0] = new SqlParameter("@OrgnhierarchyID", intOrgnhierarchyID);
            SqlParam[1] = new SqlParameter("@StateID", StateID);
            SqlParam[2] = new SqlParameter("@UserId", UserID);
            SqlParam[3] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
            SqlParam[3].Direction = ParameterDirection.Output;
            SqlParam[4] = new SqlParameter("@OutError", SqlDbType.VarChar, 1000);
            SqlParam[4].Direction = ParameterDirection.Output;

            DataAccess.DataAccess.Instance.DBInsertCommand("prcInsertOrgnStateMapping", SqlParam);
            IntResultCount = Convert.ToInt32(SqlParam[3].Value);
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
    /*#CC04 START ADDED*/
    public DataSet GetOrgnhierarchyNDMappingData()
    {
        try
        {
            DataSet dsResult = new DataSet();
            SqlParam = new SqlParameter[6];
            SqlParam[0] = new SqlParameter("@OrgnhierarchyID", OrgnhierarchyID);
            SqlParam[1] = new SqlParameter("@OutError", SqlDbType.VarChar, 1000);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@PageSize", PageSize);
            SqlParam[4] = new SqlParameter("@PageIndex", PageIndex);
            SqlParam[5] = new SqlParameter("@SalesChannelID", SalesChannelID); /* #CC05 Added */
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetOrgnhierarchySalechannelMapping", CommandType.StoredProcedure, SqlParam);
            TotalRecords = Convert.ToInt32(SqlParam[2].Value);
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
    public int InsertOrgnSalechannelMapping()
    {
        try
        {
            SqlParam = new SqlParameter[9]; /* length increased */

            SqlParam[0] = new SqlParameter("@OrgnhierarchyID", intOrgnhierarchyID);
            SqlParam[1] = new SqlParameter("@NDID", NDID);
            SqlParam[2] = new SqlParameter("@UserId", UserID);
            SqlParam[3] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
            SqlParam[3].Direction = ParameterDirection.Output;
            SqlParam[4] = new SqlParameter("@OutError", SqlDbType.VarChar, 1000);
            SqlParam[4].Direction = ParameterDirection.Output;
            /* #CC04 Add Start */
            SqlParam[5] = new SqlParameter("@TVPCBHID", DTCBHID);
            /* #CC04 Add End */
            DataAccess.DataAccess.Instance.DBInsertCommand("prcInsertOrgnSaleChannelMapping", SqlParam);
            IntResultCount = Convert.ToInt32(SqlParam[3].Value);
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
    public int UpdateOrgnSaleChannelMappingStatus()
    {
        try
        {
            SqlParam = new SqlParameter[4];

            SqlParam[0] = new SqlParameter("@OrgnHierarchySaleChannelMappingID", intOrgnHierarchySaleChannelMappingID);
            SqlParam[1] = new SqlParameter("@UserId", UserID);
            SqlParam[2] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@OutError", SqlDbType.VarChar, 1000);
            SqlParam[3].Direction = ParameterDirection.Output;
            DataAccess.DataAccess.Instance.DBInsertCommand("prcUpdOrgnSaleChannelMappingStatus", SqlParam);
            IntResultCount = Convert.ToInt32(SqlParam[2].Value);
            if (SqlParam[3].Value != DBNull.Value && SqlParam[3].Value.ToString() != "")
            {
                Error = (SqlParam[3].Value).ToString();
            }
            return IntResultCount;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /*#CC04 END ADDED*/
    public int UpdateOrgnStateMappingStatus()
    {
        try
        {
            SqlParam = new SqlParameter[4];

                SqlParam[0] = new SqlParameter("@OrgnHierarchyStateMappingID", intOrgnHierarchyStateMappingID);
                SqlParam[1] = new SqlParameter("@UserId", UserID);
                SqlParam[2] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@OutError", SqlDbType.VarChar, 1000);
                SqlParam[3].Direction = ParameterDirection.Output;
                DataAccess.DataAccess.Instance.DBInsertCommand("prcUpdOrgnStateMappingStatus", SqlParam);
                IntResultCount = Convert.ToInt32(SqlParam[2].Value);
                if (SqlParam[3].Value != DBNull.Value && SqlParam[3].Value.ToString() != "")
                {
                    Error = (SqlParam[3].Value).ToString();
                }
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /* #CC03 Add End */


    /* #CC06 Ad Start  */
    public DataSet GetOrgnhierarchyUserUploadData()
    {
        try
        {
            DataSet dsResult = new DataSet();
            SqlParam = new SqlParameter[6];
            SqlParam[0] = new SqlParameter("@UserID", UserID);
            SqlParam[1] = new SqlParameter("@OutError", SqlDbType.VarChar, 1000);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@PageSize", PageSize);
            SqlParam[4] = new SqlParameter("@PageIndex", PageIndex);
            SqlParam[5] = new SqlParameter("@CompanyId", CompanyId);/*#CC12 Added*/
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetOrgnhierarchyUserUpload", CommandType.StoredProcedure, SqlParam);
            TotalRecords = Convert.ToInt32(SqlParam[2].Value);
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


    public DataSet OrgnHierarchyUserUpload()
    {
        Int16 Result;
        DataSet dsResult = new DataSet();
        SqlParam = new SqlParameter[6];
        SqlParam[0] = new SqlParameter("@SessionID", SessionID);
        SqlParam[1] = new SqlParameter("@OutParam", SqlDbType.Int);
        SqlParam[1].Direction = ParameterDirection.Output;
        SqlParam[2] = new SqlParameter("@OutError", SqlDbType.NVarChar, 1000);
        SqlParam[2].Direction = ParameterDirection.Output;
        SqlParam[3] = new SqlParameter("@UserID", UserID);
        SqlParam[4] = new SqlParameter("@Type", intType);
        SqlParam[5] = new SqlParameter("@CompanyId", CompanyId);/*#CC12 Added*/
        dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcBulkOrgnHierarchyUserUpload", CommandType.StoredProcedure, SqlParam);
        intOutParam = Convert.ToInt16(SqlParam[1].Value);
        if (Convert.ToString(SqlParam[2].Value) != null && Convert.ToString(SqlParam[2].Value) != "")
        {
            Error = Convert.ToString(SqlParam[2].Value);
        }
        return dsResult;
    }


    /*#CC06 Add End */
    /* #CC07 Add Start */
    public int InsUpdParallelOrgnHierarchyUser()
    {
        try
        {
            SqlParam = new SqlParameter[15];


            SqlParam[0] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@OutError", SqlDbType.VarChar, 1000);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@HierarchyLevelID", HierarchyLevelID);
            SqlParam[3] = new SqlParameter("@LocationCode", LocationCode);
            SqlParam[4] = new SqlParameter("@LocationName", LocationName);
            SqlParam[5] = new SqlParameter("@FullName", FullName);
            SqlParam[6] = new SqlParameter("@Status", intStatus);
            SqlParam[7] = new SqlParameter("@UserID", UserID);
            SqlParam[8] = new SqlParameter("@UserName", UserName);
            SqlParam[9] = new SqlParameter("@Password", Password);
            SqlParam[10] = new SqlParameter("@PasswordSalt", PasswordSalt);
            SqlParam[11] = new SqlParameter("@EmailID", EmailID);
            SqlParam[12] = new SqlParameter("@MobileNumber", MobileNumber);
            SqlParam[13] = new SqlParameter("@ParallelOrgnHierarchyID", ParallelOrgnHierarchyID);
            DataAccess.DataAccess.Instance.DBInsertCommand("prcAddEditParallelHierarchyUser", SqlParam);
            IntResultCount = Convert.ToInt32(SqlParam[0].Value);
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
    public DataTable GetParallelOrgnHierarchyInfo()
    {
        try
        {
            DataTable dtResult = new DataTable();
            SqlParam = new SqlParameter[8];
            SqlParam[0] = new SqlParameter("@HierarchyLevelID", HierarchyLevelID);
            SqlParam[1] = new SqlParameter("@LocationName", LocationName);
            SqlParam[2] = new SqlParameter("@LocationCode", LocationCode);

            SqlParam[3] = new SqlParameter("@UserName", UserName);
            SqlParam[4] = new SqlParameter("@ParallelOrgnHierarchyID", ParallelOrgnHierarchyID);
            SqlParam[5] = new SqlParameter("@PageSize", PageSize);
            SqlParam[6] = new SqlParameter("@PageIndex", PageIndex);
            SqlParam[7] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            SqlParam[7].Direction = ParameterDirection.Output;
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("PrcGetParallelOrgnHierarchyUserInfo", CommandType.StoredProcedure, SqlParam);
            TotalRecords = Convert.ToInt32(SqlParam[7].Value);
            return dtResult;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    /* #CC07 Add End */

    /* #CC08 Add Start */

    public DataSet GetParallelOrgnHierarchyBrandMappingInfo()
    {
        try
        {
            DataSet dsResult = new DataSet();
            SqlParam = new SqlParameter[4];
            SqlParam[0] = new SqlParameter("@OutParam", SqlDbType.Int);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@OutError", SqlDbType.NVarChar, 1000);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@UserID", UserID);
            SqlParam[3] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            SqlParam[3].Direction = ParameterDirection.Output;
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcParallelOrgnHierarchyBrandMappingData", CommandType.StoredProcedure, SqlParam);
            intOutParam = Convert.ToInt16(SqlParam[0].Value);
            TotalRecords = Convert.ToInt32(SqlParam[3].Value);
            return dsResult;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }



    public DataSet ParallelOrgnHierarchyBrandMappingUpload()
    {
        Int16 Result;
        DataSet dsResult = new DataSet();
        SqlParam = new SqlParameter[5];
        SqlParam[0] = new SqlParameter("@SessionID", SessionID);
        SqlParam[1] = new SqlParameter("@OutParam", SqlDbType.Int);
        SqlParam[1].Direction = ParameterDirection.Output;
        SqlParam[2] = new SqlParameter("@OutError", SqlDbType.NVarChar, 1000);
        SqlParam[2].Direction = ParameterDirection.Output;
        SqlParam[3] = new SqlParameter("@UserID", UserID);
        dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcBulkOrgnHierarchyBrandMappingUpload", CommandType.StoredProcedure, SqlParam);
        intOutParam = Convert.ToInt16(SqlParam[1].Value);
        if (Convert.ToString(SqlParam[2].Value) != null && Convert.ToString(SqlParam[2].Value) != "")
        {
            Error = Convert.ToString(SqlParam[2].Value);
        }
        return dsResult;
    }

    public DataSet GetParallelOrgnHierarchyLocationInfo()
    {
        try
        {
            DataSet dsResult = new DataSet();
            SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@OutParam", SqlDbType.Int);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@OutError", SqlDbType.NVarChar, 1000);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@UserID", UserID);
            SqlParam[3] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            SqlParam[3].Direction = ParameterDirection.Output;
            SqlParam[4] = new SqlParameter("@HierarchyLevelID", HierarchyLevelID);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetParallelOrgnHierarchyData", CommandType.StoredProcedure, SqlParam);
            intOutParam = Convert.ToInt16(SqlParam[0].Value);
            TotalRecords = Convert.ToInt32(SqlParam[3].Value);
            return dsResult;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }


    public int UpdParallelOrgnSalesChannelBrandMappingStatus()
    {
        try
        {
            SqlParam = new SqlParameter[5];

            SqlParam[0] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@OutError", SqlDbType.VarChar, 1000);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@Status", intStatus);
            SqlParam[2] = new SqlParameter("@UserID", UserID);
            SqlParam[3] = new SqlParameter("@ParallelOrgnSalesChannelBrandMappingID", ParallelOrgnSalesChannelBrandMappingID);
            DataAccess.DataAccess.Instance.DBInsertCommand("prcUpdateParallelOrgnHierarchyBrandMappingStatus", SqlParam);
            IntResultCount = Convert.ToInt32(SqlParam[0].Value);
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

    public DataSet GetParallelOrgnHierarchyMappingInfo()
    {
        try
        {
            DataSet dsResult = new DataSet();
            SqlParam = new SqlParameter[9];
            SqlParam[0] = new SqlParameter("@OutParam", SqlDbType.Int);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@OutError", SqlDbType.NVarChar, 1000);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@HierarchyLevelID", HierarchyLevelID);

            SqlParam[4] = new SqlParameter("@ParallelOrgnHierarchyId", ParallelOrgnHierarchyID);
            SqlParam[5] = new SqlParameter("@SalesChannelCode", SalesChannelCode);
            SqlParam[6] = new SqlParameter("@BrandID", BrandID);
            SqlParam[7] = new SqlParameter("@PageIndex", PageIndex);
            SqlParam[8] = new SqlParameter("@PageSize", PageSize);

            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("PrcGetParallelOrgnHierarchyMappingInfo", CommandType.StoredProcedure, SqlParam);
            intOutParam = Convert.ToInt16(SqlParam[0].Value);
            TotalRecords = Convert.ToInt32(SqlParam[2].Value);
            return dsResult;
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }


    public DataTable GetStateCityList(Int16 ConditionFlag)
    {
        try
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@condition", ConditionFlag);
            SqlParam[1] = new SqlParameter("@CompanyId", CompanyId);
            SqlParam[2] = new SqlParameter("@StateID", StateID);
            dtOrgInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetStateCityMaster", CommandType.StoredProcedure, SqlParam);
            return dtOrgInfo;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /* #CC08 Add End */
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

    ~OrgHierarchyData()
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




    public DataTable SelectAll()
    {
        throw new NotImplementedException();
    }
}
