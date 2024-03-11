/* 
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 21-Feb-2017, Sumit Maurya, #CC01, New method added for PSI report (copied from DataAccess created by Karam with change log #CC12 ).
 * 21-Apr-2017, Balram Jha, #CC02 - increased time out for procedure
 * 22-May-2017, Sumit Maurya, #CC03, New properties method created to get tertiary trend report (copy from ReportData frile from DataAccess).
 * 06-Nov-2017, Balram Jha, #CC04 Added Last Sale data metohd
 * 28-Nov-2017,Vijay Kumar Prajapati,#CC05, Add new method for circle wise reports.
 * 15-Nov-2016, Sumit Maurya, #CC11, New properties method created to get tertiary trend report.
 * 03-Feb-2018, Sumit Maurya, #CC13, Property created and privided in method to get Target and achivement report (Done for Comio).
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DataAccess
{
    public class TempReportData : IDisposable
    {
        DataTable dtResult;
        DataSet dsResult;
        SqlParameter[] SqlParam;
        public string error, strSalesChannelName, StrMobileNumber, StrIMEINo;
        public string ConString = ConfigurationManager.ConnectionStrings["AppConString"].ConnectionString;//#CC02 added
        private Int32 intSalesChanelTypeID, intSalesChannelID, intHierarchyLevelId, intModuleID, intOrgHierarchyId;
        string tablename, filePath;
        private Int32 intSalesType; Int16 companytype, intTagetBasedOn;



        # region BTM Report

        string todate; string fromdate; int salestypeid; string stnno; int ishozsm; int saleschannelid;

        public Int16 CompanyType
        {
            get { return companytype; }
            set { companytype = value; }
        }

        public Int16 SMSDateType
        {
            get;
            set;
        }
        public Int16 intWantZeroQuantity
        {
            get;
            set;
        }
        public DataTable dtSerialNumber
        {
            get;
            set;
        }
        public string TableName
        {
            get { return tablename; }
            set { tablename = value; }

        }


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

        public string STNNo
        {
            get { return stnno; }
            set { stnno = value; }
        }
        public string MobileNumber
        {
            get { return StrMobileNumber; }
            set { StrMobileNumber = value; }
        }
        public string IMEINo
        {
            get { return StrIMEINo; }
            set { StrIMEINo = value; }
        }
        public int SalesTypeID
        {
            get { return salestypeid; }
            set { salestypeid = value; }

        }
        public int SalesChannelId
        {
            get { return saleschannelid; }
            set { saleschannelid = value; }

        }
        public string SalesChannelName
        {
            get { return strSalesChannelName; }
            set { strSalesChannelName = value; }

        }
        public int ModuleID
        {
            get { return intModuleID; }
            set { intModuleID = value; }
        }

        # endregion
        public int Status { get; set; }

        public int ActiveStatus { get; set; }
        public Int32 OrgHierarchyId
        {
            get { return intOrgHierarchyId; }
            set { intOrgHierarchyId = value; }
        }
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
        public Int32 OtherEntityType
        {
            get;
            set;
        }
        public Int32 ModelId
        {
            get;
            set;
        }
        public Int32 SkuId
        {
            get;
            set;
        }
        public Int32 ProductCategtoryid
        {
            get;
            set;
        }
        public Int32 stateid
        {
            get;
            set;
        }
        public string SalesChannelCode
        {
            get;
            set;
        }
        public Int32 ISPId
        {
            get;
            set;
        }
        public string ISPCode
        {
            get;
            set;
        }

        public Int16 WithOrWithoutSerialBatch
        {
            get;
            set;
        }

        public Int16 ComingFrom
        {
            get;
            set;
        }
        public int Result
        {
            get;
            set;
        }

        /* #CC11 Add Start */
        public Int32 TotalRecords
        {
            get;
            set;
        }
        /* #CC11 Add End */
        /* #CC13 Add Start  */
        public Int16 HierarchyTypeID
        {
            get;
            set;
        }

        /* #CC13 Add End */

        #region user tracking report
        DateTime? datefrom; DateTime? dateto; int roleid; int userid;
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
        public int RoleId
        {
            get { return roleid; }
            set { roleid = value; }
        }
        public Int32 SalesChannelTypeID
        {
            get { return intSalesChanelTypeID; }
            set { intSalesChanelTypeID = value; }
        }
        public Int32 HierarchyLevelId
        {
            get { return intHierarchyLevelId; }
            set { intHierarchyLevelId = value; }
        }
        public Int32 SalesChannelID
        {
            get { return intSalesChannelID; }
            set { intSalesChannelID = value; }
        }
        public Int32 SalesType
        {
            get { return intSalesType; }
            set { intSalesType = value; }
        }
        public int UserId
        {
            get { return userid; }
            set { userid = value; }
        }
        public int ForSearchOrForCount
        {
            get;
            set;
        }

        public Int16 TagetBasedOn
        {
            get { return intTagetBasedOn; }
            set { intTagetBasedOn = value; }
        }
        #endregion

        # region GRN

        string grnnumber; int reporttype;

        public string GRNNumber
        {
            get { return grnnumber; }
            set { grnnumber = value; }

        }

        public int ReportType
        {
            get { return reporttype; }
            set { reporttype = value; }
        }


        public string BatchNumber
        {
            get;
            set;
        }

        public string SkuCode
        {
            get;
            set;

        }


        # region Batch Data









        # endregion




        # endregion

        public DataSet GetMonthRegionModelWiseReport()          //Pankaj Dhingra
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                SqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
                SqlParam[4].Direction = ParameterDirection.Output;
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetMonthRegionModelWiseRpt", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCircleWiseModelWiseReport()          //Pankaj Dhingra
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                SqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
                SqlParam[4].Direction = ParameterDirection.Output;
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetCircleWiseModelWiseReport", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region Public Method
        public DataTable GetGRNInfo()
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@dateto", todate);
                SqlParam[1] = new SqlParameter("@datefrom", fromdate);
                SqlParam[2] = new SqlParameter("@grnnumber", grnnumber);
                SqlParam[3] = new SqlParameter("@saleschannelid", SalesChannelID);
                SqlParam[4] = new SqlParameter("@UserID", userid);
                SqlParam[5] = new SqlParameter("@resulttype", reporttype);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetGRNDetails", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetTeritoryInfo()
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@dateto", todate);
                SqlParam[1] = new SqlParameter("@datefrom", fromdate);
                SqlParam[2] = new SqlParameter("@saleschannelid", saleschannelid);
                SqlParam[3] = new SqlParameter("@UserID", userid);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetTeritoryDetails", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetModelwiseInfo()
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@dateto", todate);
                SqlParam[1] = new SqlParameter("@datefrom", fromdate);
                SqlParam[2] = new SqlParameter("@saleschannelid", saleschannelid);
                SqlParam[3] = new SqlParameter("@UserID", userid);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetModelWiseDetails", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetSapInfo()
        {
            try
            {
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@dateto", todate);
                SqlParam[1] = new SqlParameter("@datefrom", fromdate);
                SqlParam[2] = new SqlParameter("@ModuleID", ModuleID);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetSapInfo", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetUploadSchemaTable()
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@tablename", tablename);
                SqlParam[1] = new SqlParameter("@companytype", companytype);
                dtResult = DataAccess.Instance.GetTableFromDatabase("PrcTableSchema", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataTable GetModuleNameInfoForSap()
        {
            try
            {
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetModuleNameInfoForSap", CommandType.StoredProcedure);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetRetailerReachInfo()
        {
            try
            {
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@dateto", todate);
                SqlParam[1] = new SqlParameter("@datefrom", fromdate);
                SqlParam[2] = new SqlParameter("@UserId", UserId);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetRetailerReach", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetSalesTypeforReport()
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
                SqlParam[1] = new SqlParameter("@HierarchyLevelID", HierarchyLevelId);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetSalesType", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public DataTable GetBTMInfo()
        {
            try
            {
                SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@todate", todate);
                SqlParam[1] = new SqlParameter("@fromdate", fromdate);
                SqlParam[2] = new SqlParameter("@stnno", stnno);
                SqlParam[3] = new SqlParameter("@saleschanneltypeid", salestypeid);
                SqlParam[4] = new SqlParameter("@saleschannelid", saleschannelid);
                SqlParam[5] = new SqlParameter("@reporttype", reporttype);
                SqlParam[6] = new SqlParameter("@UserId", userid);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetStockTransferDetails", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetUserTrackingInfo()
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@roleid", roleid);
                SqlParam[1] = new SqlParameter("@userid", userid);
                SqlParam[2] = new SqlParameter("@datefrom", datefrom);
                SqlParam[3] = new SqlParameter("@dateto", dateto);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetUserTrackingDetails", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetSalesReport()
        {
            try
            {
                SqlParam = new SqlParameter[11];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                SqlParam[4] = new SqlParameter("@SalesType", SalesType);
                SqlParam[5] = new SqlParameter("@Modelid", ModelId);
                SqlParam[6] = new SqlParameter("@Skuid", SkuId);
                SqlParam[7] = new SqlParameter("@Stateid", stateid);
                SqlParam[8] = new SqlParameter("@ProductCategtoryid", ProductCategtoryid);
                SqlParam[9] = new SqlParameter("@OrgnHierarchyId", OrgHierarchyId);
                SqlParam[10] = new SqlParameter("@WantZeroQuantity", intWantZeroQuantity);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetSalesRpt", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetSalesReportRetailer()
        {
            try
            {
                SqlParam = new SqlParameter[8];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                SqlParam[4] = new SqlParameter("@SalesType", SalesType);
                SqlParam[5] = new SqlParameter("@OtherEntityType", OtherEntityType);
                SqlParam[6] = new SqlParameter("@Modelid", ModelId);
                SqlParam[7] = new SqlParameter("@Skuid", SkuId);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetSalesRptRetailer", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetOFWiseSalesReportOnida()
        {
            try
            {
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[1] = new SqlParameter("@UserId", UserId);
                SqlParam[2] = new SqlParameter("@SalesType", SalesType);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("[prcSalesReportOFWise]", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSalesChannalForAutoMailer()
        {
            try
            {
                //SqlParam = new SqlParameter[5];
                //SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                //SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                //SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                //SqlParam[3] = new SqlParameter("@UserId", UserId);
                //SqlParam[4] = new SqlParameter("@SalesType", SalesType);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetSalesChannelforAutoMailScheduler", CommandType.StoredProcedure);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTertioryReport()
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetTertioryRpt", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetSalesContributionReport()
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                SqlParam[4] = new SqlParameter("@SalesType", SalesType);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetSalesContributionRpt", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetFlatSalesReport()
        {
            try
            {
                SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                SqlParam[4] = new SqlParameter("@SalesType", SalesType);
                SqlParam[5] = new SqlParameter("@ModelID", ModelId);
                SqlParam[6] = new SqlParameter("@SKUid", SkuId);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetFlatSalesRpt", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetSMSLogReport()
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@MobileNumber", MobileNumber);
                SqlParam[3] = new SqlParameter("@IMEINo", IMEINo);
                SqlParam[4] = new SqlParameter("@UserId", UserId);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetSMSLogReport", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPurchaseReport()
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                SqlParam[4] = new SqlParameter("@OtherEntityType", OtherEntityType);
                //SqlParam[4] = new SqlParameter("@Type", SalesType);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetPurchaseRpt", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetStockingUserInfo()
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@UserId", UserId);
                SqlParam[3] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetStockingUserInfo", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetFlatPurchaseReport()
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                //SqlParam[4] = new SqlParameter("@Type", SalesType);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetFlatPurchaseRpt", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetOPSIReportRetailer()
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@RetailerID", saleschannelid);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                SqlParam[4] = new SqlParameter("@OtherEntityType", OtherEntityType);
                SqlParam[5] = new SqlParameter("@SalesChannelCode", SalesChannelCode);
                // dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetOPSIRptRetailer", CommandType.StoredProcedure, SqlParam);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetOPSIRptRetailerV5", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetOPSIReport()//Pankaj Kumar
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                SqlParam[4] = new SqlParameter("@SalesChannelCode", SalesChannelCode);
                SqlParam[5] = new SqlParameter("@SalesChannelTypeid", SalesChannelTypeID);
                //dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetOPSIRpt", CommandType.StoredProcedure, SqlParam);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetOPSIRptV5", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetStockReport()
        {
            try
            {
                SqlParam = new SqlParameter[9];

                SqlParam[0] = new SqlParameter("@DateTo", DateTo);
                SqlParam[1] = new SqlParameter("@UserId", UserId);
                SqlParam[2] = new SqlParameter("@SalesChannelTypeId", intSalesChanelTypeID);

                SqlParam[3] = new SqlParameter("@Modelid", ModelId);
                SqlParam[4] = new SqlParameter("@Skuid", SkuId);
                SqlParam[5] = new SqlParameter("@Stateid", stateid);
                SqlParam[6] = new SqlParameter("@ProductCategtoryid", ProductCategtoryid);
                SqlParam[7] = new SqlParameter("@OrgnHierarchyId", OrgHierarchyId);
                SqlParam[8] = new SqlParameter("@WantZeroQuantity", intWantZeroQuantity);

                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetStockRpt", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetStockReportRetailer()
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@DateTo", DateTo);
                SqlParam[1] = new SqlParameter("@UserId", UserId);
                SqlParam[2] = new SqlParameter("@SalesChannelTypeId", intSalesChanelTypeID);
                SqlParam[3] = new SqlParameter("@otherEntityType", OtherEntityType);

                SqlParam[4] = new SqlParameter("@Modelid", ModelId);
                SqlParam[5] = new SqlParameter("@Skuid", SkuId);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetStockRptRetailer", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPriceProtectionReport()
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@FromDate", DateFrom);
                SqlParam[1] = new SqlParameter("@UserId", UserId);

                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetPriceProtectionRpt", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetStockAdjustmentReport()
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@SalesChannelTypeID", intSalesChanelTypeID);
                SqlParam[1] = new SqlParameter("@SalesChannelName", strSalesChannelName);
                SqlParam[2] = new SqlParameter("@Datefrom", datefrom);
                SqlParam[3] = new SqlParameter("@DateTo", dateto);
                SqlParam[4] = new SqlParameter("@SalesChannelID", intSalesChannelID);
                SqlParam[5] = new SqlParameter("@UserId", userid);
                dtResult = DataAccess.Instance.GetTableFromDatabase("PrcGetStockAdjustmentRpt", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetLaggardReport()
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@OrgnhierarchyID", OrgHierarchyId);
                SqlParam[1] = new SqlParameter("@UserId", UserId);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetLaggardRpt", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetStockReportCommon()
        {
            SqlConnection objCon = new SqlConnection(ConString);//#CC02 added
            objCon.Open();
            try
            {
                SqlParam = new SqlParameter[12];

                SqlParam[0] = new SqlParameter("@DateTo", DateTo);
                SqlParam[1] = new SqlParameter("@UserId", UserId);
                SqlParam[2] = new SqlParameter("@SalesChannelTypeId", intSalesChanelTypeID);

                SqlParam[3] = new SqlParameter("@Modelid", ModelId);
                SqlParam[4] = new SqlParameter("@Skuid", SkuId);
                SqlParam[5] = new SqlParameter("@Stateid", stateid);
                SqlParam[6] = new SqlParameter("@ProductCategtoryid", ProductCategtoryid);
                SqlParam[7] = new SqlParameter("@OrgnHierarchyId", OrgHierarchyId);
                SqlParam[8] = new SqlParameter("@WantZeroQuantity", intWantZeroQuantity);
                SqlParam[9] = new SqlParameter("@ComingFrom", ComingFrom);
                SqlParam[10] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[10].Direction = ParameterDirection.Output;
                SqlParam[11] = new SqlParameter("@filepath", FilePath);
                //dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetStockRptV4", CommandType.StoredProcedure, SqlParam);#CC02 comented
                //dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetFlatSalesRptV4", CommandType.StoredProcedure, SqlParam);
                /*#CC02 add start*/
                dsResult = new DataSet();
                SqlCommand objComm = new SqlCommand("PrcGetStockRptV4", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 600;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }

                /*#CC02 end*/
                Result = Convert.ToInt32(SqlParam[10].Value);
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


        #endregion
        //For Orchid
        public DataSet GetPrimaryOrderReport()
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetPrimaryOrderRpt", CommandType.StoredProcedure, SqlParam);
                return dsResult;
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

        ~TempReportData()
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


        /*
        ZedSales Version-2 (BCP Reports)
     */
        public Int32 GetStockReportExcelbybcp()
        {
            try
            {
                Int32 intResult = 1;
                SqlParam = new SqlParameter[11];
                SqlParam[0] = new SqlParameter("@DateTo", DateTo);
                SqlParam[1] = new SqlParameter("@UserId", UserId);
                SqlParam[2] = new SqlParameter("@SalesChannelTypeId", intSalesChanelTypeID);
                SqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@filepath", FilePath);
                SqlParam[5] = new SqlParameter("@Modelid", ModelId);
                SqlParam[6] = new SqlParameter("@Skuid", SkuId);
                SqlParam[7] = new SqlParameter("@stateid", stateid);
                SqlParam[8] = new SqlParameter("@ProductCategtoryid", ProductCategtoryid);
                SqlParam[9] = new SqlParameter("@OrgnHierarchyId", OrgHierarchyId);
                SqlParam[10] = new SqlParameter("@WantZeroQuantity", intWantZeroQuantity);

                DataAccess.Instance.DBInsertCommand("PrcGetStockRptExcelbybcp", SqlParam);
                intResult = Convert.ToInt32(SqlParam[3].Value);
                return intResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Int32 GetStockReportDateWiseExcelbybcp()
        {
            try
            {
                Int32 intResult = 1;
                SqlParam = new SqlParameter[14];
                SqlParam[0] = new SqlParameter("@UserId", UserId);
                SqlParam[1] = new SqlParameter("@FromDate", DateFrom);
                SqlParam[2] = new SqlParameter("@ToDate", DateTo);
                SqlParam[3] = new SqlParameter("@SalesChannelID", SalesChannelID);

                SqlParam[4] = new SqlParameter("@SalesChannelTypeId", intSalesChanelTypeID);
                SqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[5].Direction = ParameterDirection.Output;
                SqlParam[6] = new SqlParameter("@filepath", FilePath);
                SqlParam[7] = new SqlParameter("@Modelid", ModelId);
                SqlParam[8] = new SqlParameter("@Skuid", SkuId);
                SqlParam[9] = new SqlParameter("@stateid", stateid);
                SqlParam[10] = new SqlParameter("@ProductCategtoryid", ProductCategtoryid);
                SqlParam[11] = new SqlParameter("@OrgnHierarchyId", OrgHierarchyId);
                SqlParam[12] = new SqlParameter("@WantZeroQuantity", intWantZeroQuantity);
                SqlParam[13] = new SqlParameter("@OutError", SqlDbType.VarChar, 200);
                SqlParam[13].Direction = ParameterDirection.Output;
                DataAccess.Instance.DBInsertCommand("PrcGetStockRptDateWiseExcelbybcp", SqlParam);
                intResult = Convert.ToInt32(SqlParam[5].Value);
                error = Convert.ToString(SqlParam[13].Value);
                return intResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Int32 GetStockReportExcelbybcpRetailer()
        {
            SqlConnection objCon = new SqlConnection(ConString);//#CC02 added
            objCon.Open();
            try
            {
                Int32 intResult = 1;
                SqlParam = new SqlParameter[8];
                SqlParam[0] = new SqlParameter("@DateTo", DateTo);
                SqlParam[1] = new SqlParameter("@UserId", UserId);
                SqlParam[2] = new SqlParameter("@SalesChannelTypeId", intSalesChanelTypeID);
                SqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@filepath", FilePath);
                SqlParam[5] = new SqlParameter("@OtherEntityType", OtherEntityType);
                SqlParam[6] = new SqlParameter("@Modelid", ModelId);
                SqlParam[7] = new SqlParameter("@Skuid", SkuId);
                //DataAccess.Instance.DBInsertCommand("PrcGetStockRptExcelbybcpRetailer", SqlParam);#CC02 comented
                /*#CC02 add start*/

                SqlCommand objComm = new SqlCommand("PrcGetStockRptExcelbybcpRetailer", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 600;
                objComm.ExecuteNonQuery();

                /*#CC02 end*/
                intResult = Convert.ToInt32(SqlParam[3].Value);
                return intResult;
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

        public Int32 GetRSPDSRReport()
        {
            try
            {
                Int32 intResult = 1;
                SqlParam = new SqlParameter[8];
                SqlParam[0] = new SqlParameter("@DateFrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@UserId", UserId);
                SqlParam[3] = new SqlParameter("@SalesChannelTypeId", SalesChannelTypeID);
                SqlParam[4] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[4].Direction = ParameterDirection.Output;
                SqlParam[5] = new SqlParameter("@filepath", FilePath);
                SqlParam[6] = new SqlParameter("@SalesChannelid", saleschannelid);
                SqlParam[7] = new SqlParameter("@ISPCode", ISPCode);
                DataAccess.Instance.DBInsertCommand("PrcGetRSPDSRReport", SqlParam);
                intResult = Convert.ToInt32(SqlParam[4].Value);
                return intResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Int32 GetRSPStockReport()
        {
            try
            {
                Int32 intResult = 1;
                SqlParam = new SqlParameter[8];
                SqlParam[0] = new SqlParameter("@DateFrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@UserId", UserId);
                SqlParam[3] = new SqlParameter("@SalesChannelTypeId", SalesChannelTypeID);
                SqlParam[4] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[4].Direction = ParameterDirection.Output;
                SqlParam[5] = new SqlParameter("@filepath", FilePath);
                SqlParam[6] = new SqlParameter("@SalesChannelid", SalesChannelID);
                SqlParam[7] = new SqlParameter("@ISPCode", ISPCode);
                DataAccess.Instance.DBInsertCommand("PrcGetRSPStockReport", SqlParam);
                intResult = Convert.ToInt32(SqlParam[4].Value);
                return intResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Int32 GetRSPAttendanceReport()
        {
            try
            {
                Int32 intResult = 1;
                SqlParam = new SqlParameter[8];
                SqlParam[0] = new SqlParameter("@DateFrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@UserId", UserId);
                SqlParam[3] = new SqlParameter("@SalesChannelTypeId", SalesChannelTypeID);
                SqlParam[4] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[4].Direction = ParameterDirection.Output;
                SqlParam[5] = new SqlParameter("@filepath", FilePath);
                SqlParam[6] = new SqlParameter("@SalesChannelid", SalesChannelID);
                SqlParam[7] = new SqlParameter("@ISPCode", ISPCode);
                DataAccess.Instance.DBInsertCommand("PrcGetRSPAttendanceReport", SqlParam);
                intResult = Convert.ToInt32(SqlParam[4].Value);
                return intResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }





        public DataSet GetFlatBulkTertioryReport()        //Pankaj Dhingra
        {
            try
            {
                Int32 intResult = 1;
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                SqlParam[4] = new SqlParameter("@filepath", FilePath);
                SqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[5].Direction = ParameterDirection.Output;
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetFlatBulkTertioryRpt", CommandType.StoredProcedure, SqlParam);
                intResult = Convert.ToInt32(SqlParam[5].Value);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Int32 GetFlatPurchaseReportbybcp()
        {
            try
            {
                Int32 intResult = 1;
                SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                SqlParam[4] = new SqlParameter("@filepath", FilePath);
                SqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[5].Direction = ParameterDirection.Output;
                SqlParam[6] = new SqlParameter("@OtherEntityType", OtherEntityType);
                DataAccess.Instance.DBInsertCommand("PrcGetFlatPurchaseRpt", SqlParam);
                intResult = Convert.ToInt32(SqlParam[5].Value);
                return intResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Int32 GetFlatPurchaseReportbybcpSB()       //1-Feb-2013
        {
            try
            {
                Int32 intResult = 1;
                SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                SqlParam[4] = new SqlParameter("@filepath", FilePath);
                SqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[5].Direction = ParameterDirection.Output;
                SqlParam[6] = new SqlParameter("@OtherEntityType", OtherEntityType);
                DataAccess.Instance.DBInsertCommand("PrcGetFlatPurchaseRptSB", SqlParam);
                intResult = Convert.ToInt32(SqlParam[5].Value);
                return intResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int32 GetFlatSalesReportbybcp()
        {
            try
            {
                Int32 intResult = 1;
                SqlParam = new SqlParameter[13];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                SqlParam[4] = new SqlParameter("@SalesType", SalesType);
                SqlParam[5] = new SqlParameter("@filepath", FilePath);
                SqlParam[6] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[6].Direction = ParameterDirection.Output;
                SqlParam[7] = new SqlParameter("@ModelID", ModelId);
                SqlParam[8] = new SqlParameter("@SKUid", SkuId);
                SqlParam[9] = new SqlParameter("@ProductCategtoryid", ProductCategtoryid);
                SqlParam[10] = new SqlParameter("@stateid", stateid);
                SqlParam[11] = new SqlParameter("@OrgnHierarchyId", OrgHierarchyId);
                SqlParam[12] = new SqlParameter("@WantZeroQuantity", intWantZeroQuantity);
                DataAccess.Instance.DBInsertCommand("PrcGetFlatSalesRpt", SqlParam);
                intResult = Convert.ToInt32(SqlParam[6].Value);
                return intResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Int32 GetFlatSalesReportbybcpSB()    //01-Feb-2013
        {
            try
            {
                Int32 intResult = 1;
                SqlParam = new SqlParameter[13];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                SqlParam[4] = new SqlParameter("@SalesType", SalesType);
                SqlParam[5] = new SqlParameter("@filepath", FilePath);
                SqlParam[6] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[6].Direction = ParameterDirection.Output;
                SqlParam[7] = new SqlParameter("@ModelID", ModelId);
                SqlParam[8] = new SqlParameter("@SKUid", SkuId);
                SqlParam[9] = new SqlParameter("@ProductCategtoryid", ProductCategtoryid);
                SqlParam[10] = new SqlParameter("@stateid", stateid);
                SqlParam[11] = new SqlParameter("@OrgnHierarchyId", OrgHierarchyId);
                SqlParam[12] = new SqlParameter("@WantZeroQuantity", intWantZeroQuantity);
                DataAccess.Instance.DBInsertCommand("PrcGetFlatSalesRptSB", SqlParam);
                intResult = Convert.ToInt32(SqlParam[6].Value);
                return intResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Int32 GetFlatSalesReportbybcpRetailer()
        {
            try
            {
                Int32 intResult = 1;
                SqlParam = new SqlParameter[10];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                SqlParam[4] = new SqlParameter("@SalesType", SalesType);
                SqlParam[5] = new SqlParameter("@filepath", FilePath);
                SqlParam[6] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[6].Direction = ParameterDirection.Output;
                SqlParam[7] = new SqlParameter("@OtherEntityType", OtherEntityType);
                SqlParam[8] = new SqlParameter("@Modelid", ModelId);
                SqlParam[9] = new SqlParameter("@Skuid", SkuId);
                DataAccess.Instance.DBInsertCommand("PrcGetFlatSalesRptRetailer", SqlParam);
                intResult = Convert.ToInt32(SqlParam[6].Value);
                return intResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetFlatSalesReportCommonForAll()    //01-Feb-2013
        {
            try
            {
                Int32 intResult = 1;
                SqlParam = new SqlParameter[15];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                SqlParam[4] = new SqlParameter("@SalesType", SalesType);
                SqlParam[5] = new SqlParameter("@filepath", FilePath);
                SqlParam[6] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[6].Direction = ParameterDirection.Output;
                SqlParam[7] = new SqlParameter("@ModelID", ModelId);
                SqlParam[8] = new SqlParameter("@SKUid", SkuId);
                SqlParam[9] = new SqlParameter("@ProductCategtoryid", ProductCategtoryid);
                SqlParam[10] = new SqlParameter("@stateid", stateid);
                SqlParam[11] = new SqlParameter("@OrgnHierarchyId", OrgHierarchyId);
                SqlParam[12] = new SqlParameter("@WantZeroQuantity", intWantZeroQuantity);
                SqlParam[13] = new SqlParameter("@WithOrWithoutSerialBatch", WithOrWithoutSerialBatch);
                SqlParam[14] = new SqlParameter("@ComingFrom", ComingFrom);
                // DataAccess.Instance.DBInsertCommand("PrcGetFlatSalesRptV4", SqlParam);
                /* #CC07 START COMMENTED dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetFlatSalesRptV4", CommandType.StoredProcedure, SqlParam);  #CC07 END COMMENTED */
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetFlatSalesRptV6", CommandType.StoredProcedure, SqlParam);/*#CC07 ADDED*/
                Result = Convert.ToInt32(SqlParam[6].Value);
                //return intResult;
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetFlatLastSalesReportCommonForAll() //#CC04 Added
        {
            try
            {
                Int32 intResult = 1;
                SqlParam = new SqlParameter[15];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                SqlParam[4] = new SqlParameter("@SalesType", SalesType);
                SqlParam[5] = new SqlParameter("@filepath", FilePath);
                SqlParam[6] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[6].Direction = ParameterDirection.Output;
                SqlParam[7] = new SqlParameter("@ModelID", ModelId);
                SqlParam[8] = new SqlParameter("@SKUid", SkuId);
                SqlParam[9] = new SqlParameter("@ProductCategtoryid", ProductCategtoryid);
                SqlParam[10] = new SqlParameter("@stateid", stateid);
                SqlParam[11] = new SqlParameter("@OrgnHierarchyId", OrgHierarchyId);
                SqlParam[12] = new SqlParameter("@WantZeroQuantity", intWantZeroQuantity);
                SqlParam[13] = new SqlParameter("@WithOrWithoutSerialBatch", WithOrWithoutSerialBatch);
                SqlParam[14] = new SqlParameter("@ComingFrom", ComingFrom);
                
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetFlatLastSales", CommandType.StoredProcedure, SqlParam);/*#CC07 ADDED*/
                Result = Convert.ToInt32(SqlParam[6].Value);
                
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTargetVsAchivementRpt()
        {
            try
            {
                SqlParam = new SqlParameter[7]; /* #CC13 Length increased from 6 to 7 */
                SqlParam[0] = new SqlParameter("@datefrom", datefrom);
                SqlParam[1] = new SqlParameter("@dateto", dateto);
                SqlParam[2] = new SqlParameter("@HierarchyLevelId", HierarchyLevelId);
                SqlParam[3] = new SqlParameter("@userid", userid);
                SqlParam[4] = new SqlParameter("@TargetBasedOn", intTagetBasedOn);
                SqlParam[5] = new SqlParameter("@LoggedInSalesChannelID", SalesChannelId);
                SqlParam[6] = new SqlParameter("@HierarchyTypeID", HierarchyTypeID); /* #CC13 Added */
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetTargetVsAcheivementRptVer2", CommandType.StoredProcedure, SqlParam);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResult;
        }
        public DataSet SelectSalesChannelLedgerInformation()
        {
            try
            {

                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@Saleschannelid", SalesChannelID);
                SqlParam[1] = new SqlParameter("@ToDate", DateTo);
                SqlParam[2] = new SqlParameter("@FromDate", DateFrom);
                SqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
                SqlParam[3].Direction = ParameterDirection.Output;
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcSalesChannelLedgerReport", CommandType.StoredProcedure, SqlParam);
                if (Convert.ToString(SqlParam[3].Value) != "")
                    error = Convert.ToString(SqlParam[3].Value);
                return dsResult;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable ReportToPendingDSR()
        {
            dtResult = new DataTable();
            dtResult = DataAccess.Instance.GetTableFromDatabase("prcget_MissingDSR", CommandType.StoredProcedure);
            return dtResult;
        }


        public DataSet GetViewSerialNumberMovementWithTransactionExcel(DataTable dtSN)
        {
            try
            {

                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@dtserialNumberExcel", dtSN);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetViewSerialNumberMovementWithTransactionExcel", CommandType.StoredProcedure, SqlParam);
                error = Convert.ToString(SqlParam[1].Value);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetViewSerialNumberMovement(DataTable dtSN)
        {
            try
            {
                dtResult = new DataTable();
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@dtserialNumber", dtSN);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetViewSerialNumberMovement", CommandType.StoredProcedure, SqlParam);
                error = Convert.ToString(SqlParam[1].Value);
                return dsResult.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetSerialTrackGetTransactions(Int64 SerialMasterID)
        {
            try
            {
                dtResult = new DataTable();
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@SerialMasterID", SerialMasterID);
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcSerialTrackGetTransactions", CommandType.StoredProcedure, SqlParam);
                error = Convert.ToString(SqlParam[2].Value);
                return dsResult.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetGRNInfoSB()
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@dateto", todate);
                SqlParam[1] = new SqlParameter("@datefrom", fromdate);
                SqlParam[2] = new SqlParameter("@grnnumber", grnnumber);
                SqlParam[3] = new SqlParameter("@saleschannelid", SalesChannelID);
                SqlParam[4] = new SqlParameter("@UserID", userid);
                SqlParam[5] = new SqlParameter("@resulttype", reporttype);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetGRNDetailsSB", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Int32 GetFlatSalesReportbybcpRetailerSB()
        {
            try
            {
                Int32 intResult = 1;
                SqlParam = new SqlParameter[10];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                SqlParam[4] = new SqlParameter("@SalesType", SalesType);
                SqlParam[5] = new SqlParameter("@filepath", FilePath);
                SqlParam[6] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[6].Direction = ParameterDirection.Output;
                SqlParam[7] = new SqlParameter("@OtherEntityType", OtherEntityType);
                SqlParam[8] = new SqlParameter("@Modelid", ModelId);
                SqlParam[9] = new SqlParameter("@Skuid", SkuId);
                DataAccess.Instance.DBInsertCommand("PrcGetFlatSalesRptRetailerSB", SqlParam);
                intResult = Convert.ToInt32(SqlParam[6].Value);
                return intResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetSMSTertiorySalesData()
        {
            try
            {
                SqlParam = new SqlParameter[8];
                SqlParam[0] = new SqlParameter("@dateto", todate);
                SqlParam[1] = new SqlParameter("@datefrom", fromdate);
                SqlParam[2] = new SqlParameter("@saleschannelid", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserID", userid);
                SqlParam[4] = new SqlParameter("@SMSDateType", SMSDateType);
                SqlParam[5] = new SqlParameter("@dtserialNumber", dtSerialNumber);
                SqlParam[6] = new SqlParameter("@ForSearchOrForCount", ForSearchOrForCount);
                SqlParam[7] = new SqlParameter("@ModalId", ModelId);

                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetSMSTertiorySalesData", CommandType.StoredProcedure, SqlParam);
                //if(ForSearchOrForCount==1)
                //dtResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetSMSTertiorySalesData", CommandType.StoredProcedure, SqlParam).Tables[1];
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetSMSSystemPISTertiory()
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@dateto", todate);
                SqlParam[1] = new SqlParameter("@datefrom", fromdate);
                SqlParam[2] = new SqlParameter("@saleschannelid", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserID", userid);
                SqlParam[4] = new SqlParameter("@SMSDateType", SMSDateType);
                SqlParam[5] = new SqlParameter("@dtserialNumber", dtSerialNumber);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetSMSSystemPISTertiory", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //This will fetch all the SMS whether they are fail or ok
        public DataTable GetFullSMSList()
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@dateto", todate);
                SqlParam[1] = new SqlParameter("@datefrom", fromdate);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetFullSMSList", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetUserTrackingInfoV2()
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@roleid", roleid);
                SqlParam[1] = new SqlParameter("@userid", userid);
                SqlParam[2] = new SqlParameter("@datefrom", datefrom);
                SqlParam[3] = new SqlParameter("@dateto", dateto);
                SqlParam[4] = new SqlParameter("@Status", Status);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetUserTrackingDetailsV2", CommandType.StoredProcedure, SqlParam);
                return dtResult;
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
                SqlParam[0] = new SqlParameter("@roleid", RoleId);
                SqlParam[1] = new SqlParameter("@Status", ActiveStatus);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetUserFromRole", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /* #CC01 Add Start */
        public DataSet PSIReport()
        {
            try
            {
                SqlParam = new SqlParameter[15];
                SqlParam[0] = new SqlParameter("@FromDate", FromDate);
                SqlParam[1] = new SqlParameter("@ToDate", ToDate);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcRptPSIReport", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /* #CC01 Add End */

        /* #CC03 Add Start */
        public DataSet GetTertiaryTrendReport()
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@ToDate", ToDate);
                SqlParam[2] = new SqlParameter("@FromDate", FromDate);
                SqlParam[3] = new SqlParameter("@WebUserId", UserId);
                SqlParam[4] = new SqlParameter("@OutError", SqlDbType.VarChar, 200);
                SqlParam[4].Direction = ParameterDirection.Output;
                SqlParam[5] = new SqlParameter("@TotalRecord", SqlDbType.BigInt);
                SqlParam[5].Direction = ParameterDirection.Output;
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcTertiaryTrendSelect", CommandType.StoredProcedure, SqlParam);
                Result = Convert.ToInt32(SqlParam[0].Value);
                if (SqlParam[4].Value != null)
                {
                    if (Convert.ToString(SqlParam[4].Value) != "")
                        error = Convert.ToString(SqlParam[4].Value);
                }
                TotalRecords = Convert.ToInt32(SqlParam[5].Value);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /* #CC03 Add End */
        /*#CC05 Added Started*/
        public DataSet GetCircleWiseStateWiseReport()         
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                SqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
                SqlParam[4].Direction = ParameterDirection.Output;
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetCircleWiseTertioryReport", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*#CC05 Added End*/
        public DataTable GetRetailerReachInfoV2()
        {
            try
            {
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@dateto", todate);
                SqlParam[1] = new SqlParameter("@datefrom", fromdate);
                SqlParam[2] = new SqlParameter("@UserId", UserId);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetRetailerReachV2", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*#CC05 Added End*/
    }
}
