﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Resources; /* #CC15 Added */
/* 
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 21-Feb-2017, Sumit Maurya, #CC01, New method added for PSI report (copied from DataAccess created by Karam with change log #CC12 ).
 * 21-Apr-2017, Balram Jha, #CC02 - increased time out for procedure
 * 22-May-2017, Sumit Maurya, #CC03, New properties method created to get tertiary trend report (copy from ReportData frile from DataAccess).
 * 06-Nov-2017, Balram Jha, #CC04 Added Last Sale data metohd
 * 28-Nov-2017,Vijay Kumar Prajapati,#CC05, Add new method for circle wise reports.
 * 11 Jun 2015, Karam Chand Sharma, #CC01 , Create some properties nad function for bind sales channel 
 * 14 July 2015, Karam Chand Sharma, #CC02 , pass state in in retailer stock report
 * 27 July 2015, Karam Chand Sharma, #CC03 , Create new function to return sale channel type 
 * 19-Oct-2015, Sumit Maurya, #CC04, New property OutParam and methods GetSalesChannel() and  GetActivatedIMEISalesRecodMissing()  created
 * 19-Feb-2016, Sumit Mauyra, #CC05, New property IsSerialRequired and method GetGRNReportDataCSV created (For Flat GRN Report).
 *                                  New Property Status created and new parameter supplied in method GetLaggardReport() (For Laggard Report)
 * 26 Feb 2015, Karam Chand Sharma, #CC06, Create two new function for valid and raw sms data name with GetTertiarySMSSale ,GetTertiarySMSSaleRawData          
 * 02 May 2016, Karam Chand Sharma, #CC07, Pass new procedure to GetFlatSalesReportCommonForAll()
 * 08 June 2016, Karam Chand Sharma, #CC08, Pass model and sku id into GetSMSSystemPISTertiory function
 * 09-Jun-2016, Sumit Maurya, #CC09, New property and method created to fetch TertiaryReportSummary details.
 * 16-Jun-2016, Karam Chand Sharma, #CC10, add  intWantZeroQuantity properties into GetStockReportExcelbybcpRetailer() function
 * 15-Nov-2016, Sumit Maurya, #CC11, New properties method created to get tertiary trend report.
 * 25-Dec-2017, Karam Chand Sharma, #CC12, Create new function GetUserTrackingInfoV2 which will help to get only last login menu detail and Create a new function name PSI report
 * 05-June-2017,Rajnish Kumar, #CC14, CityId added in Report filter
 * 08-Jun-2018, Sumit Maurya, #CC15, header replace code further modified (done for motorola)
  * 12-Jun-2018, Rajnish Kumar, #CC16, New Retailer Report
 * 21-Jun-2018, Balram Jha, #CC17 Added Month and year(System start year to current year) Methodh
 * 31-Jul-2018, Balram Jha, #CC18 Add Karbon dashBoard
 * 10-Sep-2018, Balram Jha, #CC19 Primary to tertiary track changed return type as dataset
 * 12-Sep-2018, Balram Jha, #CC20 New method for Stock Age Slab report for Karbon
 * 17-Sep-2018, Balram Jha, #CC21 New method for Date wise stock with IMEI
 * 19-Sept-2018, Rakesh Raj, #CC22, New Retailer Wise Sec & Ter Stock Vol Report
 * 24-Oct-2018, Sumit Maurya,#CC23, New method created to get Entitylist/saleschannellist according to login user (Done for mototrola).
 * 26-Oct-2018, Rakesh Raj, #CC24, New Method Added for New Retailer TertioryTrackFlatFinance Report 
 * 27-Feb-2019,Vijay Kumar Prajapati,#CC25,Added New Paramater For ManageFullSMSReportGionee Page.
 * 02-March-2019,Vijay Kumar Prajapati,#CC26, Added new paramater brandId for tertiarytrend report.
 * 22-May-2019,Vijay Kumar Prajapati,#CC27,Added new method for productcategory (done for Brother).
 * 20-Nov-2019, Balram Jha, #CC28, Changes for Patch Health report (for brother)
 * 05-Mar-2020, Balram Jha, #CC29, Changes for Merino Dashboard
 * 22-Nov-2022, Rinku Sharma,#CC30, Set Company ID GetStockReportExcelbybcpRetailer.
 * 23-Nov-2022, Adnan Mubeen, #CC31, Add method to get Target Report.
 */
namespace DataAccess
{
    public class ReportData : IDisposable
    {
        DataTable dtResult;
        DataSet dsResult;
        SqlParameter[] SqlParam;
        public string error, strSalesChannelName, StrMobileNumber, StrIMEINo;
        public string ConString = ConfigurationManager.ConnectionStrings["AppConString"].ConnectionString;//#CC02 added
        private Int32 intSalesChanelTypeID, intSalesChannelID, intHierarchyLevelId, intModuleID, intOrgHierarchyId;
        string tablename, filePath;
        private Int32 intSalesType; Int16 companytype, intTagetBasedOn;
        public Int32 CompanyId { get; set; }
        /* #CC04 Add Start */
        private int _intOutParam;
        public int OutParam
        {
            get
            {
                return _intOutParam;
            }
            private set
            {
                _intOutParam = value;
            }
        }
        /* #CC04 Add End */
        # region BTM Report

        string todate; string fromdate; int salestypeid; string stnno; int ishozsm; int saleschannelid;

        public Int16 CompanyType
        {
            get { return companytype; }
            set { companytype = value; }
        }
        public Int16 RegionId { get; set; }//#CC29
        public Int16 SMSDateType
        {
            get;
            set;
        }
        public int PageIndex//#CC28
        {
            get;
            set;
        }
        public int PageSize//#CC28
        {
            get;
            set;
        }
        /*#CC14*/
        public Int32 CityId
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
        public int BrandId { get; set; }/*#CC26 Added Started*/
        public Int32 CategoryId { get; set; }/*#CC27 Added*/
        /*#CC25 Added Started*/
        public Int16 ActivationFrom
        {
            get;
            set;
        }
        /*#CC25 Added End*/
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
        public Int32 BaseEntityTypeID
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
        /* #CC09 Add Start */
        public int NDID
        {
            get;
            set;
        }
        /* #CC09 Add End */
        public int SpecificSaleChannelId { get; set; }/*#CC01 ADDED*/
        /* #CC05 Add Start */
        public int IsSerialRequired { get; set; }

        /* #CC05 Add End */

        /* #CC11 Add Start */
        public Int32 TotalRecords
        {
            get;
            set;
        }
        /* #CC11 Add End */
        /* #CC17 Add Start */
        public Int32 DashBoardMonth
        {
            get;
            set;
        }
        public Int32 DashBoardYear
        {
            get;
            set;
        }
        public Int16 DashBoardTargetType
        {
            get;
            set;
        }
        public Int16 DashBoardType
        {
            get;
            set;
        }
        public Int16 DashBoardExportType
        {
            get;
            set;
        }

        /* #CC17 Add End */


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
        public Int16 DownloadType { get; set; }
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
        # region DashBoard Report
        public DateTime Fromdate1
        {
            get;
            set;
        }

        public DateTime Todate1
        {
            get;
            set;
        }
        /*CC22 Start*/
        public DataSet getRetailerWiseSecTerStockVolReport()
        {
            SqlConnection objCon = new SqlConnection(ConString);
            objCon.Open();
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@WebUserId", UserId);
                SqlParam[1] = new SqlParameter("@ToDate", ToDate);
                SqlParam[2] = new SqlParameter("@FromDate", FromDate);
                SqlParam[3] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@OutError", SqlDbType.NVarChar, 500);
                SqlParam[4].Direction = ParameterDirection.Output;

                //dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcReportRetailerMTDSale", CommandType.StoredProcedure, SqlParam);
                //if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                //    error = Convert.ToString(SqlParam[4].Value);
                //return dsResult;

                SqlCommand objComm = new SqlCommand("prcReportRetailerFull", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 900;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }

                /*#CC02 end*/
                if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                    error = Convert.ToString(SqlParam[4].Value);
                return dsResult;


            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (objCon.State != ConnectionState.Closed)
                    objCon.Close();
            }
        }

        /*CC22 End*/

        public DataSet getMTDRetailerReport()
        {
            SqlConnection objCon = new SqlConnection(ConString);//#CC02 added
            objCon.Open();
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@WebUserId", UserId);
                SqlParam[1] = new SqlParameter("@ToDate", ToDate);
                SqlParam[2] = new SqlParameter("@FromDate", FromDate);
                SqlParam[3] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@OutError", SqlDbType.NVarChar, 500);
                SqlParam[4].Direction = ParameterDirection.Output;

                //dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcReportRetailerMTDSale", CommandType.StoredProcedure, SqlParam);
                //if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                //    error = Convert.ToString(SqlParam[4].Value);
                //return dsResult;

                SqlCommand objComm = new SqlCommand("prcReportRetailerMTDSale", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 900;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }

                /*#CC02 end*/
                if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                    error = Convert.ToString(SqlParam[4].Value);
                return dsResult;


            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (objCon.State != ConnectionState.Closed)
                    objCon.Close();
            }
        }

        public DataSet getStateWiseMTDReport()
        {
            SqlConnection objCon = new SqlConnection(ConString);//#CC02 added
            objCon.Open();
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@WebUserId", UserId);
                SqlParam[1] = new SqlParameter("@ToDate", ToDate);
                SqlParam[2] = new SqlParameter("@FromDate", FromDate);
                SqlParam[3] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@OutError", SqlDbType.NVarChar, 500);
                SqlParam[4].Direction = ParameterDirection.Output;
                //dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcReportStateWiseMTDSale", CommandType.StoredProcedure, SqlParam);
                //if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                //    error = Convert.ToString(SqlParam[4].Value);
                //return dsResult;

                SqlCommand objComm = new SqlCommand("prcReportStateWiseMTDSale", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 900;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }

                /*#CC02 end*/
                if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                    error = Convert.ToString(SqlParam[4].Value);
                return dsResult;


            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (objCon.State != ConnectionState.Closed)
                    objCon.Close();
            }
        }
        public DataSet getCityWiseMTDReport()
        {
            SqlConnection objCon = new SqlConnection(ConString);//#CC02 added
            objCon.Open();
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@WebUserId", UserId);
                SqlParam[1] = new SqlParameter("@ToDate", ToDate);
                SqlParam[2] = new SqlParameter("@FromDate", FromDate);
                SqlParam[3] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@OutError", SqlDbType.NVarChar, 500);
                SqlParam[4].Direction = ParameterDirection.Output;
                //dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcReportKey47CityMTDSale", CommandType.StoredProcedure, SqlParam);
                //if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                //    error = Convert.ToString(SqlParam[4].Value);
                //return dsResult;

                SqlCommand objComm = new SqlCommand("prcReportKey47CityMTDSale", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 900;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }

                /*#CC02 end*/
                if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                    error = Convert.ToString(SqlParam[4].Value);
                return dsResult;


            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (objCon.State != ConnectionState.Closed)
                    objCon.Close();
            }
        }
        public DataSet getT1PSTMTDReport()
        {
            SqlConnection objCon = new SqlConnection(ConString);//#CC02 added
            objCon.Open();
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@WebUserId", UserId);
                SqlParam[1] = new SqlParameter("@ToDate", ToDate);
                SqlParam[2] = new SqlParameter("@FromDate", FromDate);
                SqlParam[3] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@OutError", SqlDbType.NVarChar, 500);
                SqlParam[4].Direction = ParameterDirection.Output;
                //dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcReportKeyT1PSTMTDSale", CommandType.StoredProcedure, SqlParam);
                //if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                //    error = Convert.ToString(SqlParam[4].Value);
                //return dsResult;

                SqlCommand objComm = new SqlCommand("prcReportKeyT1PSTMTDSale", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 900;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }

                /*#CC02 end*/
                if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                    error = Convert.ToString(SqlParam[4].Value);
                return dsResult;


            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (objCon.State != ConnectionState.Closed)
                    objCon.Close();
            }
        }
        public DataSet getModelWiseMTDReport()
        {
            SqlConnection objCon = new SqlConnection(ConString);//#CC02 added
            objCon.Open();
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@WebUserId", UserId);
                SqlParam[1] = new SqlParameter("@ToDate", ToDate);
                SqlParam[2] = new SqlParameter("@FromDate", FromDate);
                SqlParam[3] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@OutError", SqlDbType.NVarChar, 500);
                SqlParam[4].Direction = ParameterDirection.Output;
                //dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcReportModelWiseMTDSale", CommandType.StoredProcedure, SqlParam);
                //if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                //    error = Convert.ToString(SqlParam[4].Value);
                //return dsResult;

                SqlCommand objComm = new SqlCommand("prcReportModelWiseMTDSale", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 900;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }

                /*#CC02 end*/
                if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                    error = Convert.ToString(SqlParam[4].Value);
                return dsResult;


            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (objCon.State != ConnectionState.Closed)
                    objCon.Close();
            }
        }
        public DataSet getPriceBandWiseMTDReport()
        {
            SqlConnection objCon = new SqlConnection(ConString);//#CC02 added
            objCon.Open();
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@WebUserId", UserId);
                SqlParam[1] = new SqlParameter("@ToDate", ToDate);
                SqlParam[2] = new SqlParameter("@FromDate", FromDate);
                SqlParam[3] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@OutError", SqlDbType.NVarChar, 500);
                SqlParam[4].Direction = ParameterDirection.Output;
                //dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcPriceBandWiseMTDSale", CommandType.StoredProcedure, SqlParam);
                //if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                //    error = Convert.ToString(SqlParam[4].Value);
                //return dsResult;

                SqlCommand objComm = new SqlCommand("prcPriceBandWiseMTDSale", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 900;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }

                /*#CC02 end*/
                if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                    error = Convert.ToString(SqlParam[4].Value);
                return dsResult;


            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (objCon.State != ConnectionState.Closed)
                    objCon.Close();
            }
        }

        public DataSet getASMWiseWiseMTDReport()
        {
            SqlConnection objCon = new SqlConnection(ConString);//#CC02 added
            objCon.Open();
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@WebUserId", UserId);
                SqlParam[1] = new SqlParameter("@ToDate", ToDate);
                SqlParam[2] = new SqlParameter("@FromDate", FromDate);
                SqlParam[3] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@OutError", SqlDbType.NVarChar, 500);
                SqlParam[4].Direction = ParameterDirection.Output;
                //dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcReportASMWiseMTDSale", CommandType.StoredProcedure, SqlParam);
                //if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                //    error = Convert.ToString(SqlParam[4].Value);
                //return dsResult;

                SqlCommand objComm = new SqlCommand("prcReportASMWiseMTDSale", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 900;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }

                /*#CC02 end*/
                if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                    error = Convert.ToString(SqlParam[4].Value);
                return dsResult;


            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (objCon.State != ConnectionState.Closed)
                    objCon.Close();
            }
        }
        public DataSet getNumberOfBillCutsMTDReport()
        {
            SqlConnection objCon = new SqlConnection(ConString);//#CC02 added
            objCon.Open();
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@WebUserId", UserId);
                SqlParam[1] = new SqlParameter("@ToDate", ToDate);
                SqlParam[2] = new SqlParameter("@FromDate", FromDate);
                SqlParam[3] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@OutError", SqlDbType.NVarChar, 500);
                SqlParam[4].Direction = ParameterDirection.Output;
                //dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcReportMTDNoOfBillCutsSale", CommandType.StoredProcedure, SqlParam);
                //if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                //    error = Convert.ToString(SqlParam[4].Value);
                //return dsResult;

                SqlCommand objComm = new SqlCommand("prcReportMTDNoOfBillCutsSale", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 900;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }

                /*#CC02 end*/
                if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                    error = Convert.ToString(SqlParam[4].Value);
                return dsResult;


            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (objCon.State != ConnectionState.Closed)
                    objCon.Close();
            }
        }

        public DataSet getTertiarySecondaryNodReport()
        {
            SqlConnection objCon = new SqlConnection(ConString);//#CC02 added
            objCon.Open();
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@WebUserId", UserId);
                SqlParam[1] = new SqlParameter("@ToDate", ToDate);
                SqlParam[2] = new SqlParameter("@FromDate", FromDate);
                SqlParam[3] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@OutError", SqlDbType.NVarChar, 500);
                SqlParam[4].Direction = ParameterDirection.Output;
                //dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcReportTerSecNodsMTDSale", CommandType.StoredProcedure, SqlParam);
                //if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                //    error = Convert.ToString(SqlParam[4].Value);
                //return dsResult;
                SqlCommand objComm = new SqlCommand("prcReportTerSecNodsMTDSale", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 900;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }

                /*#CC02 end*/
                if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                    error = Convert.ToString(SqlParam[4].Value);
                return dsResult;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (objCon.State != ConnectionState.Closed)
                    objCon.Close();
            }
        }

        public DataSet getRetailerCategoryWiseMTDReport()
        {
            SqlConnection objCon = new SqlConnection(ConString);//#CC02 added
            objCon.Open();
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@WebUserId", UserId);
                SqlParam[1] = new SqlParameter("@ToDate", ToDate);
                SqlParam[2] = new SqlParameter("@FromDate", FromDate);
                SqlParam[3] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@OutError", SqlDbType.NVarChar, 500);
                SqlParam[4].Direction = ParameterDirection.Output;
                //dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcReportRetailerCategoryWiseMTDSale", CommandType.StoredProcedure, SqlParam);
                //if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                //    error = Convert.ToString(SqlParam[4].Value);
                //return dsResult;
                SqlCommand objComm = new SqlCommand("prcReportRetailerCategoryWiseMTDSale", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 900;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }
                if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                    error = Convert.ToString(SqlParam[4].Value);
                return dsResult;
            }

            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (objCon.State != ConnectionState.Closed)
                    objCon.Close();
            }
        }

        public DataSet getRDWiseMTDSaleReport()
        {
            SqlConnection objCon = new SqlConnection(ConString);//#CC02 added
            objCon.Open();
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@WebUserId", UserId);
                SqlParam[1] = new SqlParameter("@ToDate", ToDate);
                SqlParam[2] = new SqlParameter("@FromDate", FromDate);
                SqlParam[3] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@OutError", SqlDbType.NVarChar, 500);
                SqlParam[4].Direction = ParameterDirection.Output;
                //dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcReportRDWiseMTDSale", CommandType.StoredProcedure, SqlParam);
                //if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                //    error = Convert.ToString(SqlParam[4].Value);
                //return dsResult;

                SqlCommand objComm = new SqlCommand("prcReportRDWiseMTDSale", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 900;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }
                if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                    error = Convert.ToString(SqlParam[4].Value);
                return dsResult;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (objCon.State != ConnectionState.Closed)
                    objCon.Close();
            }
        }
        # endregion




        /*#CC01 ADDED START*/

        public DataSet getServiceEntity()
        {
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@UserId", UserId);
                param[1] = new SqlParameter("@SaleChannelId", SpecificSaleChannelId);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcBindEntityList", CommandType.StoredProcedure, param);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*#CC01 ADDED END*/

        /*#CC03 ADDED START*/

        public DataSet getSaleChannelType()
        {
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] param = new SqlParameter[3];
                param[1] = new SqlParameter("@UserId", SpecificSaleChannelId);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcSaleChannelTypeEntityWise", CommandType.StoredProcedure, param);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*#CC03 ADDED END*/

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
        /*#CC27 Addded*/
        public DataTable GetRetailerReachInfoV3()
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@dateto", todate);
                SqlParam[1] = new SqlParameter("@datefrom", fromdate);
                SqlParam[2] = new SqlParameter("@UserId", UserId);
                SqlParam[3] = new SqlParameter("@ProductCategoryId", ProductCategtoryid);
                SqlParam[4] = new SqlParameter("@ModelId", ModelId);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetRetailerReachV3", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*#CC27 Addded End*/
        public DataTable GetSalesTypeforReport()
        {
            try
            {
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
                SqlParam[1] = new SqlParameter("@HierarchyLevelID", HierarchyLevelId);
                SqlParam[2] = new SqlParameter("@CompanyId", CompanyId);
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
        /*#CC12 START ADDED*/
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
        /*#CC12 END ADDED*/
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
                SqlParam[5] = new SqlParameter("@OtherEntityType", BaseEntityTypeID);
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
            SqlConnection objCon = new SqlConnection(ConString);//#CC02 added
            objCon.Open();
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                SqlParam[4] = new SqlParameter("@SalesType", SalesType);

                dsResult = new DataSet();
                SqlCommand objComm = new SqlCommand("PrcGetSalesContributionRpt", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 600;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }


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
                SqlParam[4] = new SqlParameter("@OtherEntityType", BaseEntityTypeID);
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
                SqlParam[4] = new SqlParameter("@OtherEntityType", BaseEntityTypeID);
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
                SqlParam = new SqlParameter[8];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                SqlParam[4] = new SqlParameter("@SalesChannelCode", SalesChannelCode);
                SqlParam[5] = new SqlParameter("@SalesChannelTypeid", SalesChannelTypeID);
                SqlParam[6] = new SqlParameter("@OutError", SqlDbType.NVarChar, 1000);
                SqlParam[6].Direction = ParameterDirection.Output;
                SqlParam[7] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
                SqlParam[7].Direction = ParameterDirection.Output;

                //dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetOPSIRpt", CommandType.StoredProcedure, SqlParam);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetOPSIRptV5", CommandType.StoredProcedure, SqlParam);
                if (SqlParam[6].Value != null)
                    error = Convert.ToString(SqlParam[6].Value);
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
                SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@DateTo", DateTo);
                SqlParam[1] = new SqlParameter("@UserId", UserId);
                SqlParam[2] = new SqlParameter("@SalesChannelTypeId", intSalesChanelTypeID);
                SqlParam[3] = new SqlParameter("@otherEntityType", BaseEntityTypeID);

                SqlParam[4] = new SqlParameter("@Modelid", ModelId);
                SqlParam[5] = new SqlParameter("@Skuid", SkuId);
                SqlParam[6] = new SqlParameter("@CompanyId", CompanyId);
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
                SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@SalesChannelTypeID", intSalesChanelTypeID);
                SqlParam[1] = new SqlParameter("@SalesChannelName", strSalesChannelName);
                SqlParam[2] = new SqlParameter("@Datefrom", datefrom);
                SqlParam[3] = new SqlParameter("@DateTo", dateto);
                SqlParam[4] = new SqlParameter("@SalesChannelID", intSalesChannelID);
                SqlParam[5] = new SqlParameter("@UserId", userid);
                SqlParam[6] = new SqlParameter("@DownloadType", DownloadType);
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
                SqlParam = new SqlParameter[4]; /* #CC05 Length increased from 2 to 4*/
                SqlParam[0] = new SqlParameter("@OrgnhierarchyID", OrgHierarchyId);
                SqlParam[1] = new SqlParameter("@UserId", UserId);
                /* #CC05 Add Start */
                SqlParam[2] = new SqlParameter("@Status", Status);
                SqlParam[3] = new SqlParameter("@SalesChannalID", SalesChannelID);
                /* #CC05 Add End */
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
                SqlParam = new SqlParameter[15];/*#CC14 Increase size from 12 to 14 for cityid and SaleschannelId Filter*/

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
                SqlParam[12] = new SqlParameter("@SalesChannelID1", SalesChannelID);/*#CC14*/
                SqlParam[13] = new SqlParameter("@CityId", CityId);/*#CC14*/
                SqlParam[14] = new SqlParameter("@CompanyId", CompanyId);

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

        ~ReportData()
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

        public DataSet GetStockReportExcelbybcpRetailer(out int intResult)
        {
            SqlConnection objCon = new SqlConnection(ConString);//#CC02 added
            objCon.Open();
            try
            {
                intResult = 1;
                SqlParam = new SqlParameter[11];
                SqlParam[0] = new SqlParameter("@DateTo", DateTo);
                SqlParam[1] = new SqlParameter("@UserId", UserId);
                SqlParam[2] = new SqlParameter("@SalesChannelTypeId", intSalesChanelTypeID);
                SqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@filepath", FilePath);
                SqlParam[5] = new SqlParameter("@OtherEntityType", BaseEntityTypeID);
                SqlParam[6] = new SqlParameter("@Modelid", ModelId);
                SqlParam[7] = new SqlParameter("@Skuid", SkuId);
                SqlParam[8] = new SqlParameter("@StateId", stateid);
                SqlParam[9] = new SqlParameter("@WantZeroQuantity", intWantZeroQuantity);
                SqlParam[10] = new SqlParameter("@CompanyId", CompanyId);/* #CC30 Added*/
                //DataAccess.Instance.DBInsertCommand("PrcGetStockRptExcelbybcpRetailer", SqlParam);#CC02 comented
                /*#CC02 add start*/

                SqlCommand objComm = new SqlCommand("PrcGetStockRptExcelbybcpRetailer", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 600;
                //objComm.ExecuteNonQuery();
                DataSet dsResult = new DataSet();
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }
                /*#CC02 end*/
                intResult = Convert.ToInt32(SqlParam[3].Value);
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
        //#CC21 start
        public DataSet GetStockReportIMEIWise(out string message)
        {
            SqlConnection objCon = new SqlConnection(ConString);
            objCon.Open();

            try
            {
                message = "";
                SqlParam = new SqlParameter[15];

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
                SqlParam[12] = new SqlParameter("@SalesChannelID1", SalesChannelID);
                SqlParam[13] = new SqlParameter("@CityId", CityId);
                SqlParam[14] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 1000);
                SqlParam[14].Direction = ParameterDirection.Output;
                dsResult = new DataSet();
                SqlCommand objComm = new SqlCommand("PrcGetStockRptImeiWise", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 600;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }

                /*#CC02 end*/
                Result = Convert.ToInt32(SqlParam[10].Value);
                message = Convert.ToString(SqlParam[14].Value);


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
        public DataSet GetStockReportRetailerIMEIWise(out int intResult, out string message)
        {
            SqlConnection objCon = new SqlConnection(ConString);
            objCon.Open();
            try
            {
                intResult = 1;
                message = "";
                SqlParam = new SqlParameter[11];
                SqlParam[0] = new SqlParameter("@DateTo", DateTo);
                SqlParam[1] = new SqlParameter("@UserId", UserId);
                SqlParam[2] = new SqlParameter("@SalesChannelTypeId", intSalesChanelTypeID);
                SqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@filepath", FilePath);
                SqlParam[5] = new SqlParameter("@OtherEntityType", BaseEntityTypeID);
                SqlParam[6] = new SqlParameter("@Modelid", ModelId);
                SqlParam[7] = new SqlParameter("@Skuid", SkuId);
                SqlParam[8] = new SqlParameter("@StateId", stateid);
                SqlParam[9] = new SqlParameter("@WantZeroQuantity", intWantZeroQuantity);
                SqlParam[10] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 1000);
                SqlParam[10].Direction = ParameterDirection.Output;

                SqlCommand objComm = new SqlCommand("PrcGetStockRptRetailerIMEIWise", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 600;

                DataSet dsResult = new DataSet();
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }

                intResult = Convert.ToInt32(SqlParam[3].Value);
                message = Convert.ToString(SqlParam[10].Value);

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
        //#CC21 end
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
        public DataSet GetFlatPurchaseReportbybcp(out Int32 intResult)
        {
            try
            {
                intResult = 1;
                SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                SqlParam[4] = new SqlParameter("@filepath", FilePath);
                SqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[5].Direction = ParameterDirection.Output;
                SqlParam[6] = new SqlParameter("@OtherEntityType", BaseEntityTypeID);
                //DataAccess.Instance.DBInsertCommand("PrcGetFlatPurchaseRpt", SqlParam);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetFlatPurchaseRpt", CommandType.StoredProcedure, SqlParam);/*#CC07 ADDED*/
                intResult = Convert.ToInt32(SqlParam[5].Value);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetFlatPurchaseReportbybcpSB(out Int32 intResult)       //1-Feb-2013
        {
            try
            {
                intResult = 1;
                SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[1] = new SqlParameter("@DateTo", DateTo);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@UserId", UserId);
                SqlParam[4] = new SqlParameter("@filepath", FilePath);
                SqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[5].Direction = ParameterDirection.Output;
                SqlParam[6] = new SqlParameter("@OtherEntityType", BaseEntityTypeID);
                //DataAccess.Instance.DBInsertCommand("PrcGetFlatPurchaseRptSB", SqlParam);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetFlatPurchaseRptSB", CommandType.StoredProcedure, SqlParam);/*#CC07 ADDED*/
                intResult = Convert.ToInt32(SqlParam[5].Value);
                return dsResult;
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
                SqlParam[7] = new SqlParameter("@OtherEntityType", BaseEntityTypeID);
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
                SqlParam = new SqlParameter[17];//*#CC14 *For CityId Parameter Length is increased to 15 to 16*/
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
                SqlParam[15] = new SqlParameter("@CityId", CityId);/*#CC14*/
                SqlParam[16] = new SqlParameter("@CompanyId", CompanyId);
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
                SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@datefrom", datefrom);
                SqlParam[1] = new SqlParameter("@dateto", dateto);
                SqlParam[2] = new SqlParameter("@EntityTypeId", HierarchyLevelId);
                SqlParam[3] = new SqlParameter("@userid", userid);
                SqlParam[4] = new SqlParameter("@TargetBasedOn", intTagetBasedOn);
                SqlParam[5] = new SqlParameter("@LoggedInSalesChannelID", SalesChannelId);
                SqlParam[6] = new SqlParameter("@CompanyId", CompanyId);
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
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@dtserialNumber", dtSN);
                SqlParam[3] = new SqlParameter("@UserId", userid);
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
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@SerialMasterID", SerialMasterID);
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@UserId", userid);
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
                SqlParam[7] = new SqlParameter("@OtherEntityType", BaseEntityTypeID);
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
        /* #CC24 Start*/
        public DataTable GetSMSSystemPISTertioryFIN()
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
                SqlParam[6] = new SqlParameter("@ModelId", ModelId);/*#CC08 ADDED */
                SqlParam[7] = new SqlParameter("@SKUId", SkuId);/*#CC08 ADDED */
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetSMSSystemPISTertioryFIN", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /* #CC24 End*/

        public DataTable GetSMSSystemPISTertiory()
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
                SqlParam[6] = new SqlParameter("@ModelId", ModelId);/*#CC08 ADDED */
                SqlParam[7] = new SqlParameter("@SKUId", SkuId);/*#CC08 ADDED */
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetSMSSystemPISTertiory", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDatasetSMSPISTertiory()//#CC19 added
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
                SqlParam[6] = new SqlParameter("@ModelId", ModelId);/*#CC08 ADDED */
                SqlParam[7] = new SqlParameter("@SKUId", SkuId);/*#CC08 ADDED */
                DataSet dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetSMSSystemPISTertiory", CommandType.StoredProcedure, SqlParam);
                return dsResult;
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
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@dateto", todate);
                SqlParam[1] = new SqlParameter("@datefrom", fromdate);
                SqlParam[2] = new SqlParameter("@ActivationType", ActivationFrom);/*#CC25 Added*/
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetFullSMSList", CommandType.StoredProcedure, SqlParam);
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
        /* #CC04 Add Start */
        public DataSet GetSalesChannel()
        {
            try
            {
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@UserId", UserId);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetNDList", CommandType.StoredProcedure, SqlParam);
                OutParam = Convert.ToInt16(SqlParam[0].Value);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetActivatedIMEISalesRecodMissing()
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@SelectedEntityId", SalesChannelID);
                SqlParam[1] = new SqlParameter("@ToDate", DateTo);
                SqlParam[2] = new SqlParameter("@FromDate", DateFrom);
                SqlParam[3] = new SqlParameter("@OutParam", SqlDbType.NVarChar, 200);
                SqlParam[3].Direction = ParameterDirection.Output;
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcReportActivatedIMEISaleRecordMissing", CommandType.StoredProcedure, SqlParam);
                OutParam = Convert.ToInt16(SqlParam[3].Value);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /* #CC04 Add End */
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
        /*#CC06 START ADDED*/
        public DataSet GetTertiarySMSSale()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@ToDate", DateTo);
                SqlParam[1] = new SqlParameter("@FromDate", DateFrom);
                SqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[2].Direction = ParameterDirection.Output;
                ds = DataAccess.Instance.GetDataSetFromDatabase("prcGetDailyTertiarySMSSaleDetail", CommandType.StoredProcedure, SqlParam);
                Result = Convert.ToInt32(SqlParam[2].Value);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTertiarySMSSaleRawData()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@ToDate", DateTo);
                SqlParam[1] = new SqlParameter("@FromDate", DateFrom);
                SqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[2].Direction = ParameterDirection.Output;
                ds = DataAccess.Instance.GetDataSetFromDatabase("prcGetRawDataTertiarySaleSMSDetail", CommandType.StoredProcedure, SqlParam);
                Result = Convert.ToInt32(SqlParam[2].Value);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*#CC06 START END*/

        /* #CC09 Add Start */
        public DataSet GetTertiaryReportSummaryDetail()
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@ModelId", ModelId);
                SqlParam[1] = new SqlParameter("@ToDate", DateTo);
                SqlParam[2] = new SqlParameter("@FromDate", DateFrom);
                SqlParam[3] = new SqlParameter("@SKUId", SkuId);
                SqlParam[4] = new SqlParameter("@NDId", NDID);
                SqlParam[5] = new SqlParameter("@UserId", UserId);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcTertiarySummary", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /* #CC09 Add End */
        /* #CC03 Add Start */
        public DataSet GetTertiaryTrendReport()
        {
            try
            {
                SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@ToDate", ToDate);
                SqlParam[2] = new SqlParameter("@FromDate", FromDate);
                SqlParam[3] = new SqlParameter("@WebUserId", UserId);
                SqlParam[4] = new SqlParameter("@OutError", SqlDbType.VarChar, 200);
                SqlParam[4].Direction = ParameterDirection.Output;
                SqlParam[5] = new SqlParameter("@TotalRecord", SqlDbType.BigInt);
                SqlParam[5].Direction = ParameterDirection.Output;
                SqlParam[6] = new SqlParameter("@BrandId", BrandId);/*#CC26 Added*/
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


        /*#CC16 start*/
        public DataSet getRetailerOpemingStockReport()
        {
            SqlConnection objCon = new SqlConnection(ConString);//#CC02 added
            objCon.Open();
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@WebUserId", UserId);
                SqlParam[1] = new SqlParameter("@ToDate", ToDate);
                SqlParam[2] = new SqlParameter("@FromDate", FromDate);
                SqlParam[3] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@OutError", SqlDbType.NVarChar, 500);
                SqlParam[4].Direction = ParameterDirection.Output;
                //dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcReportTerSecNodsMTDSale", CommandType.StoredProcedure, SqlParam);
                //if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                //    error = Convert.ToString(SqlParam[4].Value);
                //return dsResult;
                SqlCommand objComm = new SqlCommand("getRetailerOpeningStockReport", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 900;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }

                /*#CC02 end*/
                if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                    error = Convert.ToString(SqlParam[4].Value);
                return dsResult;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (objCon.State != ConnectionState.Closed)
                    objCon.Close();
            }
        }
        public DataSet getRetailerLaggardReport()
        {
            SqlConnection objCon = new SqlConnection(ConString);//#CC02 added
            objCon.Open();
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@WebUserId", UserId);
                SqlParam[1] = new SqlParameter("@ToDate", ToDate);
                SqlParam[2] = new SqlParameter("@FromDate", FromDate);
                SqlParam[3] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@OutError", SqlDbType.NVarChar, 500);
                SqlParam[4].Direction = ParameterDirection.Output;
                //dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcReportTerSecNodsMTDSale", CommandType.StoredProcedure, SqlParam);
                //if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                //    error = Convert.ToString(SqlParam[4].Value);
                //return dsResult;
                SqlCommand objComm = new SqlCommand("prcRetailerLaggardReportForWeb", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 900;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }

                /*#CC02 end*/
                if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                    error = Convert.ToString(SqlParam[4].Value);
                return dsResult;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (objCon.State != ConnectionState.Closed)
                    objCon.Close();
            }
        }
        public DataSet getRetailerOrderReport()
        {
            SqlConnection objCon = new SqlConnection(ConString);//#CC02 added
            objCon.Open();
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] SqlParam = new SqlParameter[9];
                SqlParam[0] = new SqlParameter("@UserId", UserId);
                SqlParam[1] = new SqlParameter("@ToDate", ToDate);
                SqlParam[2] = new SqlParameter("@FromDate", FromDate);
                SqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@OutError", SqlDbType.NVarChar, 500);
                SqlParam[4].Direction = ParameterDirection.Output;
                SqlParam[5] = new SqlParameter("@CompanyId", CompanyId);
                SqlParam[6] = new SqlParameter("@PageIndex", PageIndex);
                SqlParam[7] = new SqlParameter("@PageSize", PageSize);
                SqlParam[8] = new SqlParameter("@TotalRecord", SqlDbType.BigInt);
                SqlParam[8].Direction = ParameterDirection.Output;

                SqlCommand objComm = new SqlCommand("PrcAPIGetOrderRptForWeb", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 900;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }

                /*#CC02 end*/
                if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                    error = Convert.ToString(SqlParam[4].Value);
                if (SqlParam[8].Value != null)
                    TotalRecords = Convert.ToInt32(SqlParam[8].Value);
                return dsResult;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (objCon.State != ConnectionState.Closed)
                    objCon.Close();
            }
        }

        public DataSet getRetailerSaleReport()
        {
            SqlConnection objCon = new SqlConnection(ConString);//#CC02 added
            objCon.Open();
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@UserId", UserId);
                SqlParam[1] = new SqlParameter("@ToDate", ToDate);
                SqlParam[2] = new SqlParameter("@FromDate", FromDate);
                SqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@OutError", SqlDbType.NVarChar, 500);
                SqlParam[4].Direction = ParameterDirection.Output;
                //dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcReportTerSecNodsMTDSale", CommandType.StoredProcedure, SqlParam);
                //if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                //    error = Convert.ToString(SqlParam[4].Value);
                //return dsResult;
                SqlCommand objComm = new SqlCommand("PrcAPIGetSaleRptForWeb", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 900;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }

                /*#CC02 end*/
                if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                    error = Convert.ToString(SqlParam[4].Value);
                return dsResult;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (objCon.State != ConnectionState.Closed)
                    objCon.Close();
            }
        }  /*#CC16 end*/

        public DataTable headerReplacement(DataTable DTFinal)
        {
            int MaxReportHierarchyLevel = Convert.ToInt16(Resources.SalesHierarchy.MaxReportHierarchyLevel);
            if (DTFinal.Columns.Contains("HL1Name"))
            {
                DTFinal.Columns["HL1Name"].ColumnName = Resources.SalesHierarchy.HierarchyName1;
            }
            if (DTFinal.Columns.Contains("HL2Name"))
            {
                DTFinal.Columns["HL2Name"].ColumnName = Resources.SalesHierarchy.HierarchyName2;
            }
            if (DTFinal.Columns.Contains("HL3Name"))
            {
                DTFinal.Columns["HL3Name"].ColumnName = Resources.SalesHierarchy.HierarchyName3;
            }
            if (DTFinal.Columns.Contains("HL4Name"))
            {
                DTFinal.Columns["HL4Name"].ColumnName = Resources.SalesHierarchy.HierarchyName4;

            }
            if (DTFinal.Columns.Contains("HL5Name"))
            {
                DTFinal.Columns["HL5Name"].ColumnName = Resources.SalesHierarchy.HierarchyName5;
            }

            if (DTFinal.Columns.Contains("HL1Code"))
            {
                DTFinal.Columns["HL1Code"].ColumnName = Resources.SalesHierarchy.HierarchyName1 + "Code";
            }
            if (DTFinal.Columns.Contains("HL2Code"))
            {
                DTFinal.Columns["HL2Code"].ColumnName = Resources.SalesHierarchy.HierarchyName2 + "Code";
            }
            if (DTFinal.Columns.Contains("HL3Code"))
            {
                DTFinal.Columns["HL3Code"].ColumnName = Resources.SalesHierarchy.HierarchyName3 + "Code";
            }
            if (DTFinal.Columns.Contains("HL4Code"))
            {
                DTFinal.Columns["HL4Code"].ColumnName = Resources.SalesHierarchy.HierarchyName4 + "Code";

            }
            if (DTFinal.Columns.Contains("HL5Code"))
            {
                DTFinal.Columns["HL5Code"].ColumnName = Resources.SalesHierarchy.HierarchyName5 + "Code";
            }
            if (DTFinal.Columns.Contains("HL6Code"))
            {
                DTFinal.Columns["HL6Code"].ColumnName = Resources.SalesHierarchy.HierarchyName6 + "Code";
            }
            if (DTFinal.Columns.Contains("HL7Code"))
            {
                DTFinal.Columns["HL7Code"].ColumnName = Resources.SalesHierarchy.HierarchyName7 + "Code";
            }
            if (DTFinal.Columns.Contains("HL8Code"))
            {
                DTFinal.Columns["HL8Code"].ColumnName = Resources.SalesHierarchy.HierarchyName8 + "Code";
            }
            if (DTFinal.Columns.Contains("HL9Code"))
            {
                DTFinal.Columns["HL9Code"].ColumnName = Resources.SalesHierarchy.HierarchyName9 + "Code";
            }
            DTFinal.AcceptChanges();
            /* #CC15 Add Start */
            #region forHierarchy
            try
            {
                foreach (DataColumn dc in DTFinal.Columns)
                {
                    for (Int16 i = 0; i < DTFinal.Columns.Count; i++) /* loop for DTFinal */
                    {
                        string ColumnName = Convert.ToString(DTFinal.Columns[i].ColumnName);

                        for (int j = 1; j < 30; j++)
                        {
                            string ColumnFiledName1 = string.Empty;
                            string ColumnFiledName2 = string.Empty;
                            string splitValue = "HL" + j.ToString();
                            if (ColumnName.Contains(splitValue))
                            {
                                string[] strColName = ColumnName.Split(new[] { splitValue }, StringSplitOptions.None);/*  Convert.ToChar(splitValue));*/
                                if (strColName.Count() > 0)
                                    ColumnFiledName1 = strColName[0];
                                if (strColName.Count() > 1)
                                    ColumnFiledName2 = strColName[1];
                                /*ColumnFiledName2 = ColumnName.Replace("HL" + j.ToString(), "");*/
                                if (ColumnName == ColumnFiledName1 + "HL" + j.ToString() + ColumnFiledName2)
                                /*(dc.ColumnName.Contains("HL" + j.ToString() + ColumnFiledName))*/
                                {

                                    ResourceManager rm = Resources.SalesHierarchy.ResourceManager;
                                    string someString = rm.GetString("HierarchyName" + j);

                                    if (ColumnName == ColumnFiledName1 + "HL" + j.ToString() + ColumnFiledName2)
                                    /*(dc.ColumnName.Contains("HL" + j.ToString() + ColumnFiledName))*/
                                    {
                                        /*ColumnFiledName = ColumnName.Replace("HL" + i.ToString(), "");*/
                                        DTFinal.Columns[ColumnFiledName1 + "HL" + Convert.ToString(j) + ColumnFiledName2].ColumnName = ColumnFiledName1 + someString + ColumnFiledName2;
                                        DTFinal.AcceptChanges();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            #endregion forHierarchy

            #region forSalesChannel


            try
            {
                foreach (DataColumn dc in DTFinal.Columns)
                {
                    for (Int16 i = 0; i < DTFinal.Columns.Count; i++) /* loop for DTFinal */
                    {
                        string ColumnName = Convert.ToString(DTFinal.Columns[i].ColumnName);
                        for (int j = 1; j < 30; j++)
                        {
                            string ColumnFiledName1 = string.Empty;
                            string ColumnFiledName2 = string.Empty;
                            string splitValue = "SCH" + j.ToString();
                            if (ColumnName.Contains(splitValue))
                            {
                                string[] strColName = ColumnName.Split(new[] { splitValue }, StringSplitOptions.None);
                                if (strColName.Count() > 0)
                                    ColumnFiledName1 = strColName[0];
                                if (strColName.Count() > 1)
                                    ColumnFiledName2 = strColName[1];
                                if (ColumnName == ColumnFiledName1 + "SCH" + j.ToString() + ColumnFiledName2)
                                {
                                    ResourceManager rm = Resources.SalesHierarchy.ResourceManager;
                                    string someString = rm.GetString("SalesChannel" + j);
                                    if (ColumnName == ColumnFiledName1 + "SCH" + j.ToString() + ColumnFiledName2)
                                    {
                                        DTFinal.Columns[ColumnFiledName1 + "SCH" + Convert.ToString(j) + ColumnFiledName2].ColumnName = ColumnFiledName1 + someString + ColumnFiledName2;
                                        DTFinal.AcceptChanges();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }


            #endregion forSalesChannel

            /* #CC15 Add End */




            while (MaxReportHierarchyLevel < 10)
            {
                string colNameToRemove = "HL" + Convert.ToString(MaxReportHierarchyLevel + 1) + "Name";
                if (DTFinal.Columns.Contains(colNameToRemove))
                {
                    DTFinal.Columns.Remove(colNameToRemove);
                    //MaxReportHierarchyLevel = MaxReportHierarchyLevel + 1;
                    DTFinal.AcceptChanges();
                }
                MaxReportHierarchyLevel = MaxReportHierarchyLevel + 1;
            }
            return DTFinal;
        }

        /*#CC17 start*/
        public DataSet getMonthAndYear()
        {
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@WebUserId", UserId);
                SqlParam[1] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@OutError", SqlDbType.NVarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("procGetMonthAndYear", CommandType.StoredProcedure, SqlParam);
                if (Convert.ToString(SqlParam[2].Value) != null && Convert.ToString(SqlParam[2].Value) != "")
                    error = Convert.ToString(SqlParam[2].Value);
                return dsResult;


            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }
        public DataSet getTargetStockDashboardReport()
        {
            SqlConnection objCon = new SqlConnection(ConString);//#CC02 added
            objCon.Open();
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] SqlParam = new SqlParameter[10];
                SqlParam[0] = new SqlParameter("@WebUserId", UserId);
                SqlParam[1] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@OutError", SqlDbType.NVarChar, 1000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@Month", DashBoardMonth);
                SqlParam[4] = new SqlParameter("@year", DashBoardYear);
                /*@TargetType 1=Quantity, 2=Value*/
                SqlParam[5] = new SqlParameter("@TargetType", DashBoardTargetType);
                /* @DashboardType 0=Export action,1= TargetDashboard,2=Stock Dashboard,3=Both Target and stock Dashboard*/
                SqlParam[6] = new SqlParameter("@DashboardType", DashBoardType);
                /*@ExportType 0=Not Export,1= Target dashboard export,2=stock dashboard export, 3= stock detail export*/
                SqlParam[7] = new SqlParameter("@ExportType", DashBoardExportType);
                SqlParam[8] = new SqlParameter("@PageIndex", 0);
                SqlParam[9] = new SqlParameter("@PageSize", 0);

                SqlCommand objComm = new SqlCommand("prcTargetStockDashboardReport", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 600;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }

                if (!string.IsNullOrEmpty(Convert.ToString(SqlParam[2].Value)))
                    error = Convert.ToString(SqlParam[2].Value);
                return dsResult;


            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (objCon.State != ConnectionState.Closed)
                    objCon.Close();
            }

        }
        /*#CC17 end*/
        /*#CC18 start*/
        public DataSet getStockSalePurchaseTopRetTopModelDashBoard()
        {
            SqlConnection objCon = new SqlConnection(ConString);
            objCon.Open();
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@UserId", UserId);
                SqlParam[1] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@OutError", SqlDbType.NVarChar, 2000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@BaseEntityTypeId", BaseEntityTypeID);
                SqlParam[4] = new SqlParameter("@YearNo", "0");
                SqlParam[5] = new SqlParameter("@MonthNo", "0");


                SqlCommand objComm = new SqlCommand("prcShowDashBoard", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 600;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }

                if (!string.IsNullOrEmpty(Convert.ToString(SqlParam[2].Value)))
                    error = Convert.ToString(SqlParam[2].Value);
                return dsResult;


            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (objCon.State != ConnectionState.Closed)
                    objCon.Close();
            }

        }//#CC18 end
        public DataTable GetStockAgeSlabReport()//#CC20 Added
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@UserId", UserId);
                SqlParam[1] = new SqlParameter("@BaseEntityTypeId", BaseEntityTypeID);
                SqlParam[2] = new SqlParameter("@ReportFor", ComingFrom);
                SqlParam[3] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@OutError", SqlDbType.NVarChar, 2000);
                SqlParam[4].Direction = ParameterDirection.Output;
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcReportStockAgeSlab", CommandType.StoredProcedure, SqlParam);
                if (SqlParam[4].Value != null)
                    error = Convert.ToString(SqlParam[4].Value);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /* #CC23 Add Start */

        public DataSet GetSalesChannelList()
        {
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@UserId", UserId);
                param[1] = new SqlParameter("@SaleChannelId", SpecificSaleChannelId);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcBindEntityListRetApproval", CommandType.StoredProcedure, param);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /* #CC23 Add End*/
        /*#CC27 Added Started*/
        public DataTable GetCategoryMaster()
        {
            try
            {
                SqlParameter[] prm = new SqlParameter[0];
                DataTable dsResult = DataAccess.Instance.GetTableFromDatabase("prcGetCategoryName", CommandType.StoredProcedure, prm);
                return dsResult;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetModelMaster()
        {
            try
            {
                SqlParameter[] prm = new SqlParameter[1];
                prm[0] = new SqlParameter("@CategoryId", CategoryId);
                DataTable dsResult = DataAccess.Instance.GetTableFromDatabase("prcGetRetailerreachModelName", CommandType.StoredProcedure, prm);
                return dsResult;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /*#CC27 Added End*/
        /*#CC28 start*/
        public DataSet getPatchHealthReport()
        {
            SqlConnection objCon = new SqlConnection(ConString);
            objCon.Open();
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] SqlParam = new SqlParameter[9];
                SqlParam[0] = new SqlParameter("@UserId", UserId);
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 2000);
                SqlParam[2].Direction = ParameterDirection.Output;
                if (FromDate != null)
                {
                    SqlParam[3] = new SqlParameter("@FromDate", FromDate);
                    SqlParam[4] = new SqlParameter("@ToDate", ToDate);
                }
                else
                {
                    SqlParam[3] = new SqlParameter("@FromDate", DBNull.Value);
                    SqlParam[4] = new SqlParameter("@ToDate", DBNull.Value);
                }

                SqlParam[5] = new SqlParameter("@LastNumberOfDays", ActivationFrom);

                SqlParam[6] = new SqlParameter("@PageIndex", PageIndex);
                SqlParam[7] = new SqlParameter("@PageSize", PageSize);
                SqlParam[8] = new SqlParameter("@TotalRecords", SqlDbType.BigInt);
                SqlParam[8].Direction = ParameterDirection.Output;
                SqlCommand objComm = new SqlCommand("prcInterfaceHealthReport", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 600;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }

                if (!string.IsNullOrEmpty(Convert.ToString(SqlParam[2].Value)))
                    error = Convert.ToString(SqlParam[2].Value);
                if (!string.IsNullOrEmpty(Convert.ToString(SqlParam[8].Value)))
                    TotalRecords = Convert.ToInt32(SqlParam[8].Value);
                return dsResult;


            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (objCon.State != ConnectionState.Closed)
                    objCon.Close();
            }

        }//#CC28 end

        /*#CC29 start*/
        public DataSet getDashBoardTallyPatch()
        {
            SqlConnection objCon = new SqlConnection(ConString);
            objCon.Open();
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] SqlParam = new SqlParameter[12];
                SqlParam[0] = new SqlParameter("@UserId", UserId);
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 2000);
                SqlParam[2].Direction = ParameterDirection.Output;
                if (FromDate != null)
                {
                    SqlParam[3] = new SqlParameter("@FromDate", FromDate);
                    SqlParam[4] = new SqlParameter("@ToDate", ToDate);
                }
                else
                {
                    SqlParam[3] = new SqlParameter("@FromDate", DBNull.Value);
                    SqlParam[4] = new SqlParameter("@ToDate", DBNull.Value);
                }

                SqlParam[5] = new SqlParameter("@LastNumberOfDays", ActivationFrom);

                SqlParam[6] = new SqlParameter("@PageIndex", PageIndex);
                SqlParam[7] = new SqlParameter("@PageSize", PageSize);
                SqlParam[8] = new SqlParameter("@TotalRecords", SqlDbType.BigInt);
                SqlParam[8].Direction = ParameterDirection.Output;
                SqlCommand objComm = new SqlCommand("prcDashBoardMerino", objCon);
                SqlParam[9] = new SqlParameter("@SalesChannelName", SalesChannelName);
                SqlParam[10] = new SqlParameter("@SalesChannelCode", SalesChannelCode);
                SqlParam[11] = new SqlParameter("@RegionID", RegionId);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 600;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }

                if (!string.IsNullOrEmpty(Convert.ToString(SqlParam[2].Value)))
                    error = Convert.ToString(SqlParam[2].Value);
                if (!string.IsNullOrEmpty(Convert.ToString(SqlParam[8].Value)))
                    TotalRecords = Convert.ToInt32(SqlParam[8].Value);
                return dsResult;


            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (objCon.State != ConnectionState.Closed)
                    objCon.Close();
            }

        }//#CC29

        /* #CC31 Added Start */
        public int EntityTypeUserId
        {
            get;
            set;
        }
        public DataSet GetTargetReport()
        {
            try
            {
                SqlParam = new SqlParameter[11];
                SqlParam[0] = new SqlParameter("@UserId", userid);
                SqlParam[1] = new SqlParameter("@EntityTypeUserId", EntityTypeUserId);
                SqlParam[2] = new SqlParameter("@ProductCategoryID", ProductCategtoryid);
                SqlParam[3] = new SqlParameter("@Datefrom", DateFrom);
                SqlParam[4] = new SqlParameter("@DateTo", DateTo);
                SqlParam[5] = new SqlParameter("@PageIndex", PageIndex);
                SqlParam[6] = new SqlParameter("@PageSize", PageSize);
                SqlParam[7] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
                SqlParam[7].Direction = ParameterDirection.Output;
                SqlParam[8] = new SqlParameter("@CompanyID", CompanyId);
                SqlParam[9] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[9].Direction = ParameterDirection.Output;
                SqlParam[10] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
                SqlParam[10].Direction = ParameterDirection.Output;
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcTargetReport", CommandType.StoredProcedure, SqlParam);

                error = Convert.ToString(SqlParam[10].Value);
                TotalRecords = Convert.ToInt32(SqlParam[7].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResult;
        }
        /* #CC31 Added END */
    }


}