#region Copyright(c) 2017 Zed-Axis Technologies All rights are reserved
/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2017 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Created By : 
* Created On: 
 * Description: This is a copy of business dashboard from DataAccess.
* ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 12-May-2021, kishan singh , #CC01, New properties and methods created for Business Dashboard.
 *  24-Apr-2023, Hema Thapa, #CC02, MySQL connections
 *  15-FEB-2024, JITENDRA, #CC03, MySQL parameters 
 ====================================================================================================
*/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using MySql.Data.MySqlClient;

namespace DataAccess
{
    
   public class ClsBusinessDashboard : IDisposable
   {
       DataTable d1;
       SqlParameter[] SqlParam;
       MySqlParameter[] MySqlParam; //#CC02 added
       DataSet ds;
       private Int32 intUserID;
       private Int16 intUserRoleID;
       public Int16 SelfOrTeam { get; set; }
       public Int32 UserID
       {
           get { return intUserID; }
           set { intUserID = value; }
       }
       private string _strConnectionString;
       public string strConnectionString
       {
           get { return _strConnectionString; }
           set { _strConnectionString = value; }
       }
       private string _authKey;
       public string authKey
       {
           get { return _authKey; }
           set { _authKey = value; }
       }
       public int TotalRecords
       {
           get;
           set;
       }
       private DataSet _dsresult;
       private DataTable _dtResult;
       public DataSet dsresult
       {
           get
           {
               return _dsresult;
           }
           set
           {
               _dsresult = value;
           }
       }
       public DataTable dtresult
       {
           get { return _dtResult; }
           set { _dtResult = value; }
       }
       private string _strError;
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
       private int _intResult;
       public int intResult
       {
           get { return _intResult; }
           set { _intResult = value; }
       }
       private Int32 _accountId = 0;
       public Int32 accountId
       {
           get { return _accountId; }
           set { _accountId = value; }
       }
       public DateTime? SaleFromDate { get; set; }
       public DateTime? SaleToDate { get; set; }
       private Int32 _EntityId = 0;
       public Int32 EntityId
       {
           get { return _EntityId; }
           set { _EntityId = value; }
       }
       private Int32 _EntityTypeId = 0;
       public Int32 EntityTypeId
       {
           get { return _EntityTypeId; }
           set { _EntityTypeId = value; }
       }
       private Int32 _BaseEntityTypeID = 0;
       public Int32 BaseEntityTypeID
       {
           get { return _BaseEntityTypeID; }
           set { _BaseEntityTypeID = value; }
       }
       private Int32 _RoleId = 0;
       public Int32 RoleId
       {
           get { return _RoleId; }
           set { _RoleId = value; }
       }
       private Int32 _TopType = 0;
       public Int32 TopType
       {
           get { return _TopType; }
           set { _TopType = value; }
       }

       
       public DataSet GetUserOrderListData()
       {
           try
           {
                /* #CC02 add start */ /* #CC03 add start */
                MySqlParameter[] objSqlParam = new MySqlParameter[9];
                objSqlParam[0] = new MySqlParameter("@p_Out_Param", MySqlDbType.Int16, 2);
                objSqlParam[0].Direction = ParameterDirection.Output;
                objSqlParam[1] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 3000);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new MySqlParameter("@p_UserId", UserID);
                objSqlParam[3] = new MySqlParameter("@p_TotalRecords", MySqlDbType.Int64, 8);
                objSqlParam[3].Direction = ParameterDirection.Output;
                objSqlParam[4] = new MySqlParameter("@p_AccountIdOut", MySqlDbType.Int32);
                objSqlParam[4].Direction = ParameterDirection.Output;
                objSqlParam[5] = new MySqlParameter("@p_OrderDate", null);
                objSqlParam[6] = new MySqlParameter("@p_debugmode", 0);
                objSqlParam[7] = new MySqlParameter("@p_ParentUserId", 0);
                objSqlParam[8] = new MySqlParameter("@p_authKey", authKey);
                dsresult = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcOrderDashboardDataAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 add end */ /* #CC03 add end */

                /* #CC02 comment start */
                //SqlParameter[] objSqlParam = new SqlParameter[6];
                //objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                //objSqlParam[0].Direction = ParameterDirection.Output;
                //objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 3000);
                //objSqlParam[1].Direction = ParameterDirection.Output;
                //objSqlParam[2] = new SqlParameter("@UserId", UserID);
                //objSqlParam[3] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
                //objSqlParam[3].Direction = ParameterDirection.Output;
                //objSqlParam[4] = new SqlParameter("@AccountIdOut", SqlDbType.Int);
                //objSqlParam[4].Direction = ParameterDirection.Output;
                //objSqlParam[5] = new SqlParameter("@authKey", authKey);
                //dsresult = DataAccess.Instance.GetDataSetFromDatabase("prcOrderDashboardDataAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 comment end */

                intResult = Convert.ToInt16(objSqlParam[0].Value);
               accountId = 0; //Convert.ToInt32(objSqlParam[4].Value);
               if (objSqlParam[1].Value != null && Convert.ToString(objSqlParam[1].Value).Trim() != "")
               {
                   Error = Convert.ToString(objSqlParam[1].Value);
               }
               TotalRecords = Convert.ToInt32(objSqlParam[3].Value);
               return dsresult;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public DataSet GetUserAttandanceDashboardList()
       {
           try
           {
                /* #CC02 add start *///JK//
                MySqlParameter[] objSqlParam = new MySqlParameter[10];
                objSqlParam[0] = new MySqlParameter("@p_Out_Param", MySqlDbType.Int16, 2);
                objSqlParam[0].Direction = ParameterDirection.Output;
                objSqlParam[1] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 3000);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new MySqlParameter("@p_UserId", UserID);
                objSqlParam[3] = new MySqlParameter("@p_TotalRecords", MySqlDbType.Int64, 8);
                objSqlParam[3].Direction = ParameterDirection.Output;
                objSqlParam[5] = new MySqlParameter("@p_AttendanceDate", null);
                objSqlParam[6] = new MySqlParameter("@p_debugmode", 0);
                objSqlParam[7] = new MySqlParameter("@p_authKey", authKey);
                objSqlParam[4] = new MySqlParameter("@p_AccountIdOut", MySqlDbType.Int32);
                objSqlParam[4].Direction = ParameterDirection.Output;
                objSqlParam[8] = new MySqlParameter("@p_SelfOrTeam", SelfOrTeam);
                objSqlParam[9] = new MySqlParameter("@p_ParentUserId", 0);
                dsresult = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcAttendanceDashboardDataAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 add end *///JK//


                /* #CC02 comment start */
                //SqlParameter[] objSqlParam = new SqlParameter[7];
                //objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                //objSqlParam[0].Direction = ParameterDirection.Output;
                //objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 3000);
                //objSqlParam[1].Direction = ParameterDirection.Output;
                //objSqlParam[2] = new SqlParameter("@UserId", UserID);
                //objSqlParam[3] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
                //objSqlParam[3].Direction = ParameterDirection.Output;
                //objSqlParam[4] = new SqlParameter("@AccountIdOut", SqlDbType.Int);
                //objSqlParam[4].Direction = ParameterDirection.Output;
                //objSqlParam[5] = new SqlParameter("@authKey", authKey);
                //objSqlParam[6] = new SqlParameter("@SelfOrTeam", SelfOrTeam);
                //dsresult = DataAccess.Instance.GetDataSetFromDatabase("prcAttendanceDashboardDataAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 comment end */

                intResult = Convert.ToInt16(objSqlParam[0].Value);
               accountId = Convert.ToInt32(objSqlParam[4].Value);
               if (objSqlParam[1].Value != null && Convert.ToString(objSqlParam[1].Value).Trim() != "")
               {
                   Error = Convert.ToString(objSqlParam[1].Value);
               }
               TotalRecords = Convert.ToInt32(objSqlParam[3].Value);
               return dsresult;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public DataSet GetUserLeaveDashboardList()
       {
           try
           {

                /* #CC02 add start */
                MySqlParameter[] objSqlParam = new MySqlParameter[7];
                objSqlParam[0] = new MySqlParameter("@Out_Param", MySqlDbType.Int16, 2);
                objSqlParam[0].Direction = ParameterDirection.Output;
                objSqlParam[1] = new MySqlParameter("@Out_Error", MySqlDbType.VarChar, 3000);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new MySqlParameter("@UserId", UserID);
                objSqlParam[3] = new MySqlParameter("@TotalRecords", MySqlDbType.Int64, 8);
                objSqlParam[3].Direction = ParameterDirection.Output;
                objSqlParam[4] = new MySqlParameter("@AccountIdOut", MySqlDbType.Int32);
                objSqlParam[4].Direction = ParameterDirection.Output;
                objSqlParam[5] = new MySqlParameter("@authKey", authKey);
                objSqlParam[6] = new MySqlParameter("@SelfOrTeam", SelfOrTeam);
                dsresult = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcLeaveDashboardDataAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 add end */

                /* #CC02 comment start */
                //SqlParameter[] objSqlParam = new SqlParameter[7];
                //objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                //objSqlParam[0].Direction = ParameterDirection.Output;
                //objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 3000);
                //objSqlParam[1].Direction = ParameterDirection.Output;
                //objSqlParam[2] = new SqlParameter("@UserId", UserID);
                //objSqlParam[3] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
                //objSqlParam[3].Direction = ParameterDirection.Output;
                //objSqlParam[4] = new SqlParameter("@AccountIdOut", SqlDbType.Int);
                //objSqlParam[4].Direction = ParameterDirection.Output;
                //objSqlParam[5] = new SqlParameter("@authKey", authKey);
                //objSqlParam[6] = new SqlParameter("@SelfOrTeam", SelfOrTeam);
                //dsresult = DataAccess.Instance.GetDataSetFromDatabase("prcLeaveDashboardDataAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 comment end */
                intResult = Convert.ToInt16(objSqlParam[0].Value);
               accountId = Convert.ToInt32(objSqlParam[4].Value);
               if (objSqlParam[1].Value != null && Convert.ToString(objSqlParam[1].Value).Trim() != "")
               {
                   Error = Convert.ToString(objSqlParam[1].Value);
               }
               TotalRecords = Convert.ToInt32(objSqlParam[3].Value);
               return dsresult;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public DataSet GetUserTravelDistanceListData()
       {
           try
           {
                /* #CC02 add start */
                MySqlParameter[] objSqlParam = new MySqlParameter[7];
                objSqlParam[0] = new MySqlParameter("@Out_Param", MySqlDbType.Int16, 2);
                objSqlParam[0].Direction = ParameterDirection.Output;
                objSqlParam[1] = new MySqlParameter("@Out_Error", MySqlDbType.VarChar, 3000);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new MySqlParameter("@UserId", UserID);
                objSqlParam[3] = new MySqlParameter("@TotalRecords", MySqlDbType.Int64, 8);
                objSqlParam[3].Direction = ParameterDirection.Output;
                objSqlParam[4] = new MySqlParameter("@AccountIdOut", MySqlDbType.Int32);
                objSqlParam[4].Direction = ParameterDirection.Output;
                objSqlParam[5] = new MySqlParameter("@authKey", authKey);
                objSqlParam[6] = new MySqlParameter("@SelfOrTeam", SelfOrTeam);
                dsresult = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcTraveledDistanceDashboardDataAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 add end */

                /* #CC02 comment start */
                //SqlParameter[] objSqlParam = new SqlParameter[7];
                //objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                //objSqlParam[0].Direction = ParameterDirection.Output;
                //objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 3000);
                //objSqlParam[1].Direction = ParameterDirection.Output;
                //objSqlParam[2] = new SqlParameter("@UserId", UserID);
                //objSqlParam[3] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
                //objSqlParam[3].Direction = ParameterDirection.Output;
                //objSqlParam[4] = new SqlParameter("@AccountIdOut", SqlDbType.Int);
                //objSqlParam[4].Direction = ParameterDirection.Output;
                //objSqlParam[5] = new SqlParameter("@authKey", authKey);
                //objSqlParam[6] = new SqlParameter("@SelfOrTeam", SelfOrTeam);
                //dsresult = DataAccess.Instance.GetDataSetFromDatabase("prcTraveledDistanceDashboardDataAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 comment end */
                intResult = Convert.ToInt16(objSqlParam[0].Value);
               accountId = Convert.ToInt32(objSqlParam[4].Value);
               if (objSqlParam[1].Value != null && Convert.ToString(objSqlParam[1].Value).Trim() != "")
               {
                   Error = Convert.ToString(objSqlParam[1].Value);
               }
               TotalRecords = Convert.ToInt32(objSqlParam[3].Value);
               return dsresult;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public DataSet GetUserTimeSpendInMarketListData()
       {
           try
           {
                /* #CC02 add start */
                MySqlParameter[] objSqlParam = new MySqlParameter[7];
                objSqlParam[0] = new MySqlParameter("@Out_Param", MySqlDbType.Int16, 2);
                objSqlParam[0].Direction = ParameterDirection.Output;
                objSqlParam[1] = new MySqlParameter("@Out_Error", MySqlDbType.VarChar, 3000);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new MySqlParameter("@UserId", UserID);
                objSqlParam[3] = new MySqlParameter("@TotalRecords", MySqlDbType.Int64, 8);
                objSqlParam[3].Direction = ParameterDirection.Output;
                objSqlParam[4] = new MySqlParameter("@AccountIdOut", MySqlDbType.Int32);
                objSqlParam[4].Direction = ParameterDirection.Output;
                objSqlParam[5] = new MySqlParameter("@authKey", authKey);
                objSqlParam[6] = new MySqlParameter("@SelfOrTeam", SelfOrTeam);
                dsresult = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcTimeSpendInMarketDashboardDataAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 add end */

                /* #CC02 comment start */
                //SqlParameter[] objSqlParam = new SqlParameter[7];
                //objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                //objSqlParam[0].Direction = ParameterDirection.Output;
                //objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 3000);
                //objSqlParam[1].Direction = ParameterDirection.Output;
                //objSqlParam[2] = new SqlParameter("@UserId", UserID);
                //objSqlParam[3] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
                //objSqlParam[3].Direction = ParameterDirection.Output;
                //objSqlParam[4] = new SqlParameter("@AccountIdOut", SqlDbType.Int);
                //objSqlParam[4].Direction = ParameterDirection.Output;
                //objSqlParam[5] = new SqlParameter("@authKey", authKey);
                //objSqlParam[6] = new SqlParameter("@SelfOrTeam", SelfOrTeam);
                //dsresult = DataAccess.Instance.GetDataSetFromDatabase("prcTimeSpendInMarketDashboardDataAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 comment end */
                intResult = Convert.ToInt16(objSqlParam[0].Value);
               accountId = Convert.ToInt32(objSqlParam[4].Value);
               if (objSqlParam[1].Value != null && Convert.ToString(objSqlParam[1].Value).Trim() != "")
               {
                   Error = Convert.ToString(objSqlParam[1].Value);
               }
               TotalRecords = Convert.ToInt32(objSqlParam[3].Value);
               return dsresult;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public DataSet GetUserPropectListData()
       {
           try
           {

                /* #CC02 addstart */
                MySqlParameter[] objSqlParam = new MySqlParameter[7];
                objSqlParam[0] = new MySqlParameter("@Out_Param", MySqlDbType.Int16, 2);
                objSqlParam[0].Direction = ParameterDirection.Output;
                objSqlParam[1] = new MySqlParameter("@Out_Error", MySqlDbType.VarChar, 3000);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new MySqlParameter("@UserId", UserID);
                objSqlParam[3] = new MySqlParameter("@TotalRecords", MySqlDbType.Int64, 8);
                objSqlParam[3].Direction = ParameterDirection.Output;
                objSqlParam[4] = new MySqlParameter("@AccountIdOut", SqlDbType.Int);
                objSqlParam[4].Direction = ParameterDirection.Output;
                objSqlParam[5] = new MySqlParameter("@authKey", authKey);
                objSqlParam[6] = new MySqlParameter("@SelfOrTeam", SelfOrTeam);
                dsresult = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcProsectDashboardDataAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 add end */

                /* #CC02 comment start */
                //SqlParameter[] objSqlParam = new SqlParameter[7];
                //objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                //objSqlParam[0].Direction = ParameterDirection.Output;
                //objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 3000);
                //objSqlParam[1].Direction = ParameterDirection.Output;
                //objSqlParam[2] = new SqlParameter("@UserId", UserID);
                //objSqlParam[3] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
                //objSqlParam[3].Direction = ParameterDirection.Output;
                //objSqlParam[4] = new SqlParameter("@AccountIdOut", SqlDbType.Int);
                //objSqlParam[4].Direction = ParameterDirection.Output;
                //objSqlParam[5] = new SqlParameter("@authKey", authKey);
                //objSqlParam[6] = new SqlParameter("@SelfOrTeam", SelfOrTeam);
                //dsresult = DataAccess.Instance.GetDataSetFromDatabase("prcProsectDashboardDataAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 comment end */
                intResult = Convert.ToInt16(objSqlParam[0].Value);
               accountId = Convert.ToInt32(objSqlParam[4].Value);
               if (objSqlParam[1].Value != null && Convert.ToString(objSqlParam[1].Value).Trim() != "")
               {
                   Error = Convert.ToString(objSqlParam[1].Value);
               }
               TotalRecords = Convert.ToInt32(objSqlParam[3].Value);
               return dsresult;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public DataSet GetUserExpenseListData()
       {
           try
           {
                /* #CC02 addstart */
                MySqlParameter[] objSqlParam = new MySqlParameter[7];
                objSqlParam[0] = new MySqlParameter("@Out_Param", MySqlDbType.Int16, 2);
                objSqlParam[0].Direction = ParameterDirection.Output;
                objSqlParam[1] = new MySqlParameter("@Out_Error", MySqlDbType.VarChar, 3000);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new MySqlParameter("@UserId", UserID);
                objSqlParam[3] = new MySqlParameter("@TotalRecords", MySqlDbType.Int64, 8);
                objSqlParam[3].Direction = ParameterDirection.Output;
                objSqlParam[4] = new MySqlParameter("@AccountIdOut", MySqlDbType.Int32);
                objSqlParam[4].Direction = ParameterDirection.Output;
                objSqlParam[5] = new MySqlParameter("@authKey", authKey);
                objSqlParam[6] = new MySqlParameter("@SelfOrTeam", SelfOrTeam);
                dsresult = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcExpenseDashboardDataAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 add end */

                /* #CC02 comment start */
                //SqlParameter[] objSqlParam = new SqlParameter[7];
                //objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                //objSqlParam[0].Direction = ParameterDirection.Output;
                //objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 3000);
                //objSqlParam[1].Direction = ParameterDirection.Output;
                //objSqlParam[2] = new SqlParameter("@UserId", UserID);
                //objSqlParam[3] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
                //objSqlParam[3].Direction = ParameterDirection.Output;
                //objSqlParam[4] = new SqlParameter("@AccountIdOut", SqlDbType.Int);
                //objSqlParam[4].Direction = ParameterDirection.Output;
                //objSqlParam[5] = new SqlParameter("@authKey", authKey);
                //objSqlParam[6] = new SqlParameter("@SelfOrTeam", SelfOrTeam);
                //dsresult = DataAccess.Instance.GetDataSetFromDatabase("prcExpenseDashboardDataAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 comment end */
                intResult = Convert.ToInt16(objSqlParam[0].Value);
               accountId = Convert.ToInt32(objSqlParam[4].Value);
               if (objSqlParam[1].Value != null && Convert.ToString(objSqlParam[1].Value).Trim() != "")
               {
                   Error = Convert.ToString(objSqlParam[1].Value);
               }
               TotalRecords = Convert.ToInt32(objSqlParam[3].Value);
               return dsresult;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public DataSet GetUserAverageTimeListData()
       {
           try
           {
               SqlParameter[] objSqlParam = new SqlParameter[7];
               objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
               objSqlParam[0].Direction = ParameterDirection.Output;
               objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 3000);
               objSqlParam[1].Direction = ParameterDirection.Output;
               objSqlParam[2] = new SqlParameter("@UserId", UserID);
               objSqlParam[3] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
               objSqlParam[3].Direction = ParameterDirection.Output;
               objSqlParam[4] = new SqlParameter("@AccountIdOut", SqlDbType.Int);
               objSqlParam[4].Direction = ParameterDirection.Output;
               objSqlParam[5] = new SqlParameter("@authKey", authKey);
               objSqlParam[6] = new SqlParameter("@SelfOrTeam", SelfOrTeam);
               dsresult = DataAccess.Instance.GetDataSetFromDatabase("prcAverageTimeDashboardDataAPI", CommandType.StoredProcedure, objSqlParam);
               intResult = Convert.ToInt16(objSqlParam[0].Value);
               accountId = Convert.ToInt32(objSqlParam[4].Value);
               if (objSqlParam[1].Value != null && Convert.ToString(objSqlParam[1].Value).Trim() != "")
               {
                   Error = Convert.ToString(objSqlParam[1].Value);
               }
               TotalRecords = Convert.ToInt32(objSqlParam[3].Value);
               return dsresult;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public DataSet GetTodayTeamAttandanceListData()
       {
           try
           {
                /* #CC02 addstart */
                MySqlParameter[] objSqlParam = new MySqlParameter[7];
                objSqlParam[0] = new MySqlParameter("@Out_Param", MySqlDbType.Int16, 2);
                objSqlParam[0].Direction = ParameterDirection.Output;
                objSqlParam[1] = new MySqlParameter("@Out_Error", MySqlDbType.VarChar, 3000);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new MySqlParameter("@UserId", UserID);
                objSqlParam[3] = new MySqlParameter("@TotalRecords", MySqlDbType.Int64, 8);
                objSqlParam[3].Direction = ParameterDirection.Output;
                objSqlParam[4] = new MySqlParameter("@AccountIdOut", MySqlDbType.Int32);
                objSqlParam[4].Direction = ParameterDirection.Output;
                objSqlParam[5] = new MySqlParameter("@authKey", authKey);
                objSqlParam[6] = new MySqlParameter("@SelfOrTeam", SelfOrTeam);
                dsresult = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcTodayTeamAttandanceDashboardDataAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 add end */

                /* #CC02 comment start */
                //SqlParameter[] objSqlParam = new SqlParameter[7];
                //objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                //objSqlParam[0].Direction = ParameterDirection.Output;
                //objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 3000);
                //objSqlParam[1].Direction = ParameterDirection.Output;
                //objSqlParam[2] = new SqlParameter("@UserId", UserID);
                //objSqlParam[3] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
                //objSqlParam[3].Direction = ParameterDirection.Output;
                //objSqlParam[4] = new SqlParameter("@AccountIdOut", SqlDbType.Int);
                //objSqlParam[4].Direction = ParameterDirection.Output;
                //objSqlParam[5] = new SqlParameter("@authKey", authKey);
                //objSqlParam[6] = new SqlParameter("@SelfOrTeam", SelfOrTeam);
                //dsresult = DataAccess.Instance.GetDataSetFromDatabase("prcTodayTeamAttandanceDashboardDataAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 comment end */
                intResult = Convert.ToInt16(objSqlParam[0].Value);
               accountId = Convert.ToInt32(objSqlParam[4].Value);
               if (objSqlParam[1].Value != null && Convert.ToString(objSqlParam[1].Value).Trim() != "")
               {
                   Error = Convert.ToString(objSqlParam[1].Value);
               }
               TotalRecords = Convert.ToInt32(objSqlParam[3].Value);
               return dsresult;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public DataSet GetUserSaleListData()
       {
           try
           {
                /* #CC02 addstart */
                MySqlParameter[] objSqlParam = new MySqlParameter[7];
                objSqlParam[0] = new MySqlParameter("@Out_Param", MySqlDbType.Int16, 2);
                objSqlParam[0].Direction = ParameterDirection.Output;
                objSqlParam[1] = new MySqlParameter("@Out_Error", MySqlDbType.VarChar, 3000);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new MySqlParameter("@UserId", UserID);
                objSqlParam[3] = new MySqlParameter("@TotalRecords", MySqlDbType.Int64, 8);
                objSqlParam[3].Direction = ParameterDirection.Output;
                objSqlParam[4] = new MySqlParameter("@AccountIdOut", MySqlDbType.Int32);
                objSqlParam[4].Direction = ParameterDirection.Output;
                objSqlParam[5] = new MySqlParameter("@authKey", authKey);
                objSqlParam[6] = new MySqlParameter("@SelfOrTeam", SelfOrTeam);
                dsresult = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcUserSalesDashboardDataAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 add end */

                /* #CC02 comment start */
                //SqlParameter[] objSqlParam = new SqlParameter[7];
                //objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                //objSqlParam[0].Direction = ParameterDirection.Output;
                //objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 3000);
                //objSqlParam[1].Direction = ParameterDirection.Output;
                //objSqlParam[2] = new SqlParameter("@UserId", UserID);
                //objSqlParam[3] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
                //objSqlParam[3].Direction = ParameterDirection.Output;
                //objSqlParam[4] = new SqlParameter("@AccountIdOut", SqlDbType.Int);
                //objSqlParam[4].Direction = ParameterDirection.Output;
                //objSqlParam[5] = new SqlParameter("@authKey", authKey);
                //objSqlParam[6] = new SqlParameter("@SelfOrTeam", SelfOrTeam);
                //dsresult = DataAccess.Instance.GetDataSetFromDatabase("prcUserSalesDashboardDataAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 comment end */
                intResult = Convert.ToInt16(objSqlParam[0].Value);
               accountId = Convert.ToInt32(objSqlParam[4].Value);
               if (objSqlParam[1].Value != null && Convert.ToString(objSqlParam[1].Value).Trim() != "")
               {
                   Error = Convert.ToString(objSqlParam[1].Value);
               }
               TotalRecords = Convert.ToInt32(objSqlParam[3].Value);
               return dsresult;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public DataSet GetUserTopSalesMenListData()
       {
           try
           {

                /* #CC02 addstart */
                MySqlParameter[] objSqlParam = new MySqlParameter[6];
                objSqlParam[0] = new MySqlParameter("@Out_Param", MySqlDbType.Int16, 2);
                objSqlParam[0].Direction = ParameterDirection.Output;
                objSqlParam[1] = new MySqlParameter("@Out_Error", MySqlDbType.VarChar, 3000);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new MySqlParameter("@UserId", UserID);
                objSqlParam[3] = new MySqlParameter("@TotalRecords", MySqlDbType.Int64, 8);
                objSqlParam[3].Direction = ParameterDirection.Output;
                objSqlParam[4] = new MySqlParameter("@Out_ParamEntityId", MySqlDbType.Int32);
                objSqlParam[4].Direction = ParameterDirection.Output;
                objSqlParam[5] = new MySqlParameter("@authKey", authKey);
                dsresult = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcGetTopSalesMenAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 add end */

                /* #CC02 comment start */
                //SqlParameter[] objSqlParam = new SqlParameter[6];
                //objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                //objSqlParam[0].Direction = ParameterDirection.Output;
                //objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 3000);
                //objSqlParam[1].Direction = ParameterDirection.Output;
                //objSqlParam[2] = new SqlParameter("@UserId", UserID);
                //objSqlParam[3] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
                //objSqlParam[3].Direction = ParameterDirection.Output;
                //objSqlParam[4] = new SqlParameter("@Out_ParamEntityId", SqlDbType.Int);
                //objSqlParam[4].Direction = ParameterDirection.Output;
                //objSqlParam[5] = new SqlParameter("@authKey", authKey);
                //dsresult = DataAccess.Instance.GetDataSetFromDatabase("prcGetTopSalesMenAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 comment end */
                intResult = Convert.ToInt16(objSqlParam[0].Value);
               string value = objSqlParam[4].Value.ToString();
               if (value != null && value!="")
               {
                   accountId = Convert.ToInt32(objSqlParam[4].Value);
               }
               
               if (objSqlParam[1].Value != null && Convert.ToString(objSqlParam[1].Value).Trim() != "")
               {
                   Error = Convert.ToString(objSqlParam[1].Value);
               }
               TotalRecords = Convert.ToInt32(objSqlParam[3].Value);
               return dsresult;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public DataSet GetUserBeatPlanListData()
       {
           try
           {

                /* #CC02 addstart */
                MySqlParameter[] objSqlParam = new MySqlParameter[9];
                objSqlParam[0] = new MySqlParameter("@Out_Param", MySqlDbType.Int16, 2);
                objSqlParam[0].Direction = ParameterDirection.Output;
                objSqlParam[1] = new MySqlParameter("@Out_Error", MySqlDbType.VarChar, 3000);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new MySqlParameter("@UserId", UserID);
                objSqlParam[3] = new MySqlParameter("@TotalRecords", MySqlDbType.Int64, 8);
                objSqlParam[3].Direction = ParameterDirection.Output;
                objSqlParam[4] = new MySqlParameter("@AccountIdOut", MySqlDbType.Int32);
                objSqlParam[4].Direction = ParameterDirection.Output;
                objSqlParam[5] = new MySqlParameter("@authKey", authKey);
                objSqlParam[6] = new MySqlParameter("@EntityId", EntityId);
                objSqlParam[7] = new MySqlParameter("@EntityTypeId", EntityTypeId);
                objSqlParam[8] = new MySqlParameter("@BaseEntityTypeID", BaseEntityTypeID);
                dsresult = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcGetBeatPlanSummaryAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 add end */

                /* #CC02 comment start */
                //SqlParameter[] objSqlParam = new SqlParameter[9];
                //objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                //objSqlParam[0].Direction = ParameterDirection.Output;
                //objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 3000);
                //objSqlParam[1].Direction = ParameterDirection.Output;
                //objSqlParam[2] = new SqlParameter("@UserId", UserID);
                //objSqlParam[3] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
                //objSqlParam[3].Direction = ParameterDirection.Output;
                //objSqlParam[4] = new SqlParameter("@AccountIdOut", SqlDbType.Int);
                //objSqlParam[4].Direction = ParameterDirection.Output;
                //objSqlParam[5] = new SqlParameter("@authKey", authKey);
                //objSqlParam[6] = new SqlParameter("@EntityId", EntityId);
                //objSqlParam[7] = new SqlParameter("@EntityTypeId", EntityTypeId);
                //objSqlParam[8] = new SqlParameter("@BaseEntityTypeID", BaseEntityTypeID);
                //dsresult = DataAccess.Instance.GetDataSetFromDatabase("prcGetBeatPlanSummaryAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 comment end */
                intResult = Convert.ToInt16(objSqlParam[0].Value);
               accountId = Convert.ToInt32(objSqlParam[4].Value);
               if (objSqlParam[1].Value != null && Convert.ToString(objSqlParam[1].Value).Trim() != "")
               {
                   Error = Convert.ToString(objSqlParam[1].Value);
               }
               TotalRecords = Convert.ToInt32(objSqlParam[3].Value);
               return dsresult;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public DataSet GetTopModelListData()
       {
           try
           {

                /* #CC02 addstart */
                MySqlParameter[] objSqlParam = new MySqlParameter[7];
                objSqlParam[0] = new MySqlParameter("@OutParam", MySqlDbType.Int16, 2);
                objSqlParam[0].Direction = ParameterDirection.Output;
                objSqlParam[1] = new MySqlParameter("@OutError", MySqlDbType.VarChar, 3000);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new MySqlParameter("@UserId", UserID);
                objSqlParam[3] = new MySqlParameter("@OutUserId", MySqlDbType.Int64, 8);
                objSqlParam[3].Direction = ParameterDirection.Output;
                objSqlParam[4] = new MySqlParameter("@OutaccountId", MySqlDbType.Int32);
                objSqlParam[4].Direction = ParameterDirection.Output;
                objSqlParam[5] = new MySqlParameter("@authKey", authKey);
                objSqlParam[6] = new MySqlParameter("@RoleID", RoleId);
                dsresult = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcGetTopModels", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 add end */

                /* #CC02 comment start */
                //SqlParameter[] objSqlParam = new SqlParameter[7];
                //objSqlParam[0] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
                //objSqlParam[0].Direction = ParameterDirection.Output;
                //objSqlParam[1] = new SqlParameter("@OutError", SqlDbType.VarChar, 3000);
                //objSqlParam[1].Direction = ParameterDirection.Output;
                //objSqlParam[2] = new SqlParameter("@UserId", UserID);
                //objSqlParam[3] = new SqlParameter("@OutUserId", SqlDbType.BigInt, 8);
                //objSqlParam[3].Direction = ParameterDirection.Output;
                //objSqlParam[4] = new SqlParameter("@OutaccountId", SqlDbType.Int);
                //objSqlParam[4].Direction = ParameterDirection.Output;
                //objSqlParam[5] = new SqlParameter("@authKey", authKey);
                //objSqlParam[6] = new SqlParameter("@RoleID", RoleId);
                //dsresult = DataAccess.Instance.GetDataSetFromDatabase("prcGetTopModels", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 comment end */
                intResult = Convert.ToInt16(objSqlParam[0].Value);
               accountId = Convert.ToInt32(objSqlParam[4].Value);
               if (objSqlParam[1].Value != null && Convert.ToString(objSqlParam[1].Value).Trim() != "")
               {
                   Error = Convert.ToString(objSqlParam[1].Value);
               }

               if (objSqlParam[3].Value.ToString() != "" && objSqlParam[3].Value.ToString()!=null)
               {
                   TotalRecords = Convert.ToInt32(objSqlParam[3].Value);
               }
               
               return dsresult;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public DataSet GetTargetVsAchievementListData()
       {
           try
           {
                /* #CC02 addstart */
                MySqlParameter[] objSqlParam = new MySqlParameter[7];
                objSqlParam[0] = new MySqlParameter("@Out_Param", MySqlDbType.Int16, 2);
                objSqlParam[0].Direction = ParameterDirection.Output;
                objSqlParam[1] = new MySqlParameter("@Out_Error", MySqlDbType.VarChar, 3000);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new MySqlParameter("@UserId", UserID);
                objSqlParam[3] = new MySqlParameter("@TotalRecords", MySqlDbType.Int64, 8);
                objSqlParam[3].Direction = ParameterDirection.Output;
                objSqlParam[4] = new MySqlParameter("@OutaccountId", MySqlDbType.Int32);
                objSqlParam[4].Direction = ParameterDirection.Output;
                objSqlParam[5] = new MySqlParameter("@OutUserId", MySqlDbType.Int32);
                objSqlParam[5].Direction = ParameterDirection.Output;
                objSqlParam[6] = new MySqlParameter("@authKey", authKey);
                dsresult = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcGetTargetVsAchievementDashboardAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 add end */

                /* #CC02 comment start */
                //SqlParameter[] objSqlParam = new SqlParameter[7];
                //objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                //objSqlParam[0].Direction = ParameterDirection.Output;
                //objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 3000);
                //objSqlParam[1].Direction = ParameterDirection.Output;
                //objSqlParam[2] = new SqlParameter("@UserId", UserID);
                //objSqlParam[3] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
                //objSqlParam[3].Direction = ParameterDirection.Output;
                //objSqlParam[4] = new SqlParameter("@OutaccountId", SqlDbType.Int);
                //objSqlParam[4].Direction = ParameterDirection.Output;
                //objSqlParam[5] = new SqlParameter("@OutUserId", SqlDbType.Int);
                //objSqlParam[5].Direction = ParameterDirection.Output;
                //objSqlParam[6] = new SqlParameter("@authKey", authKey);
                //dsresult = DataAccess.Instance.GetDataSetFromDatabase("prcGetTargetVsAchievementDashboardAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 comment end */
                intResult = Convert.ToInt16(objSqlParam[0].Value);
               accountId = Convert.ToInt32(objSqlParam[4].Value);
               if (objSqlParam[1].Value != null && Convert.ToString(objSqlParam[1].Value).Trim() != "")
               {
                   Error = Convert.ToString(objSqlParam[1].Value);
               }
               TotalRecords = Convert.ToInt32(objSqlParam[3].Value);
               return dsresult;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public DataSet GetRankingListData()
       {
           try
           {
                /* #CC02 addstart */
                MySqlParameter[] objSqlParam = new MySqlParameter[6];
                objSqlParam[0] = new MySqlParameter("@Out_Param", MySqlDbType.Int16, 2);
                objSqlParam[0].Direction = ParameterDirection.Output;
                objSqlParam[1] = new MySqlParameter("@Out_Error", MySqlDbType.VarChar, 3000);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new MySqlParameter("@UserId", UserID);
                objSqlParam[3] = new MySqlParameter("@TotalRecords", MySqlDbType.Int64, 8);
                objSqlParam[3].Direction = ParameterDirection.Output;
                objSqlParam[4] = new MySqlParameter("@AccountIdOut", MySqlDbType.Int32);
                objSqlParam[4].Direction = ParameterDirection.Output;
                objSqlParam[5] = new MySqlParameter("@authKey", authKey);
                dsresult = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcGetRakingDataAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 add end */

                /* #CC02 comment start */
                //SqlParameter[] objSqlParam = new SqlParameter[6];
                //objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                //objSqlParam[0].Direction = ParameterDirection.Output;
                //objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 3000);
                //objSqlParam[1].Direction = ParameterDirection.Output;
                //objSqlParam[2] = new SqlParameter("@UserId", UserID);
                //objSqlParam[3] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
                //objSqlParam[3].Direction = ParameterDirection.Output;
                //objSqlParam[4] = new SqlParameter("@AccountIdOut", SqlDbType.Int);
                //objSqlParam[4].Direction = ParameterDirection.Output;
                //objSqlParam[5] = new SqlParameter("@authKey", authKey);
                //dsresult = DataAccess.Instance.GetDataSetFromDatabase("prcGetRakingDataAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 comment end */
                intResult = Convert.ToInt16(objSqlParam[0].Value);
               accountId = Convert.ToInt32(objSqlParam[4].Value);
               if (objSqlParam[1].Value != null && Convert.ToString(objSqlParam[1].Value).Trim() != "")
               {
                   Error = Convert.ToString(objSqlParam[1].Value);
               }
               TotalRecords = Convert.ToInt32(objSqlParam[3].Value);
               return dsresult;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public DataSet GetTopDistributorListData()
       {
           try
           {
                /* #CC02 addstart */
                MySqlParameter[] objSqlParam = new MySqlParameter[8];
                objSqlParam[0] = new MySqlParameter("@OutParam", MySqlDbType.Int16, 2);
                objSqlParam[0].Direction = ParameterDirection.Output;
                objSqlParam[1] = new MySqlParameter("@OutError", MySqlDbType.VarChar, 3000);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new MySqlParameter("@UserId", UserID);
                objSqlParam[3] = new MySqlParameter("@OutUserId", MySqlDbType.Int64);
                objSqlParam[3].Direction = ParameterDirection.Output;
                objSqlParam[4] = new MySqlParameter("@OutaccountId", MySqlDbType.Int32);
                objSqlParam[4].Direction = ParameterDirection.Output;
                objSqlParam[5] = new MySqlParameter("@authKey", authKey);
                objSqlParam[6] = new MySqlParameter("@RoleID", RoleId);
                objSqlParam[7] = new MySqlParameter("@TopType", TopType);
                dsresult = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcGetTopDistributor", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 add end */

                /* #CC02 comment start */
                //SqlParameter[] objSqlParam = new SqlParameter[8];
                //objSqlParam[0] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
                //objSqlParam[0].Direction = ParameterDirection.Output;
                //objSqlParam[1] = new SqlParameter("@OutError", SqlDbType.VarChar, 3000);
                //objSqlParam[1].Direction = ParameterDirection.Output;
                //objSqlParam[2] = new SqlParameter("@UserId", UserID);
                //objSqlParam[3] = new SqlParameter("@OutUserId", SqlDbType.Int);
                //objSqlParam[3].Direction = ParameterDirection.Output;
                //objSqlParam[4] = new SqlParameter("@OutaccountId", SqlDbType.Int);
                //objSqlParam[4].Direction = ParameterDirection.Output;
                //objSqlParam[5] = new SqlParameter("@authKey", authKey);
                //objSqlParam[6] = new SqlParameter("@RoleID", RoleId);
                //objSqlParam[7] = new SqlParameter("@TopType", TopType);
                //dsresult = DataAccess.Instance.GetDataSetFromDatabase("prcGetTopDistributor", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 comment end */
                intResult = Convert.ToInt16(objSqlParam[0].Value);
               accountId = Convert.ToInt32(objSqlParam[4].Value);
               if (objSqlParam[1].Value != null && Convert.ToString(objSqlParam[1].Value).Trim() != "")
               {
                   Error = Convert.ToString(objSqlParam[1].Value);
               }
               if (objSqlParam[3].Value.ToString() != "" && objSqlParam[3].Value.ToString()!=null)
               {
                   TotalRecords = Convert.ToInt32(objSqlParam[3].Value);
               }
               return dsresult;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public DataSet GetPaymentCollectionListData()
       {
           try
           {
                /* #CC02 addstart */
                MySqlParameter[] objSqlParam = new MySqlParameter[9];
                objSqlParam[0] = new MySqlParameter("@Out_Param", MySqlDbType.Int16, 2);
                objSqlParam[0].Direction = ParameterDirection.Output;
                objSqlParam[1] = new MySqlParameter("@Out_Error", MySqlDbType.VarChar, 3000);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new MySqlParameter("@UserId", UserID);
                objSqlParam[3] = new MySqlParameter("@TotalRecords", MySqlDbType.Int64, 8);
                objSqlParam[3].Direction = ParameterDirection.Output;
                objSqlParam[4] = new MySqlParameter("@AccountIdOut", MySqlDbType.Int32);
                objSqlParam[4].Direction = ParameterDirection.Output;
                objSqlParam[5] = new MySqlParameter("@authKey", authKey);
                objSqlParam[6] = new MySqlParameter("@EntityId", EntityId);
                objSqlParam[7] = new MySqlParameter("@EntityTypeId", EntityTypeId);
                objSqlParam[8] = new MySqlParameter("@BaseEntityTypeID", BaseEntityTypeID);
                dsresult = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcGetPaymentCollectionDashboardDetailAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 add end */

                /* #CC02 comment start */
                //SqlParameter[] objSqlParam = new SqlParameter[9];
                //objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                //objSqlParam[0].Direction = ParameterDirection.Output;
                //objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 3000);
                //objSqlParam[1].Direction = ParameterDirection.Output;
                //objSqlParam[2] = new SqlParameter("@UserId", UserID);
                //objSqlParam[3] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
                //objSqlParam[3].Direction = ParameterDirection.Output;
                //objSqlParam[4] = new SqlParameter("@AccountIdOut", SqlDbType.Int);
                //objSqlParam[4].Direction = ParameterDirection.Output;
                //objSqlParam[5] = new SqlParameter("@authKey", authKey);
                //objSqlParam[6] = new SqlParameter("@EntityId", EntityId);
                //objSqlParam[7] = new SqlParameter("@EntityTypeId", EntityTypeId);
                //objSqlParam[8] = new SqlParameter("@BaseEntityTypeID", BaseEntityTypeID);
                //dsresult = DataAccess.Instance.GetDataSetFromDatabase("prcGetPaymentCollectionDashboardDetailAPI", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 comment end */
                intResult = Convert.ToInt16(objSqlParam[0].Value);
               accountId = Convert.ToInt32(objSqlParam[4].Value);
               if (objSqlParam[1].Value != null && Convert.ToString(objSqlParam[1].Value).Trim() != "")
               {
                   Error = Convert.ToString(objSqlParam[1].Value);
               }
               TotalRecords = Convert.ToInt32(objSqlParam[3].Value);
               return dsresult;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public DataSet GetDashBoardWidget()
       {
           try
           {
                /* #CC02 add start  */
                MySqlParameter[] objSqlParam = new MySqlParameter[7];
                objSqlParam[0] = new MySqlParameter("@p_Out_Param", MySqlDbType.Int16, 2);
                objSqlParam[0].Direction = ParameterDirection.Output;
                objSqlParam[1] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 3000);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new MySqlParameter("@p_UserId", UserID);
                objSqlParam[3] = new MySqlParameter("@p_TotalRecords", MySqlDbType.Int64, 8);
                objSqlParam[3].Direction = ParameterDirection.Output;
                objSqlParam[4] = new MySqlParameter("@p_AccountIdOut", MySqlDbType.Int32);
                objSqlParam[4].Direction = ParameterDirection.Output;
                objSqlParam[5] = new MySqlParameter("@p_AuthKey", authKey);
                objSqlParam[6] = new MySqlParameter("@p_ParentUserId", 0);
                dsresult = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcAPIGetDashboardWidgetList", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 add end  */


                /* #CC02 comment start  */
                //SqlParameter[] objSqlParam = new SqlParameter[7];
                //objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                //objSqlParam[0].Direction = ParameterDirection.Output;
                //objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 3000);
                //objSqlParam[1].Direction = ParameterDirection.Output;
                //objSqlParam[2] = new SqlParameter("@UserID", UserID);
                //objSqlParam[3] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
                //objSqlParam[3].Direction = ParameterDirection.Output;
                //objSqlParam[4] = new SqlParameter("@AccountIdOut", SqlDbType.Int);
                //objSqlParam[4].Direction = ParameterDirection.Output;
                //objSqlParam[5] = new SqlParameter("@AuthKey", authKey);
                //objSqlParam[6] = new SqlParameter("@ParentUserId", 0);
                //dsresult = DataAccess.Instance.GetDataSetFromDatabase("prcAPIGetDashboardWidgetList", CommandType.StoredProcedure, objSqlParam);
                /* #CC02 comment end  */
                intResult = Convert.ToInt16(objSqlParam[0].Value);


               if (objSqlParam[4].Value != null && Convert.ToString(objSqlParam[4].Value).Trim() != "")
               {
                   accountId = Convert.ToInt32(objSqlParam[4].Value);
               }
               if (objSqlParam[1].Value != null && Convert.ToString(objSqlParam[1].Value).Trim() != "")
               {
                   Error = Convert.ToString(objSqlParam[1].Value);
               }
               TotalRecords = Convert.ToInt32(objSqlParam[3].Value);
               return dsresult;
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

        ~ClsBusinessDashboard()
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
}
