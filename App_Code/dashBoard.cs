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
 * Description: This is a copy of Salesdata from DataAccess.
* ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 21-Dec-2018, Sumit Maurya, #CC01, New properties and methods created for SupervisorDashboard (Done for Zedsalesv5).
 ====================================================================================================
*/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    
   public class dashBoard : IDisposable
    {
       public int ishierarchyallowed;
        DataTable d1;
        SqlParameter[] SqlParam;
        DataSet ds;
        string todate, fromdate; int userid; int location;
        int brandid;
        public string ToDate
        {
            get { return todate; }
            set { todate = value; }
        }
        public Int32 CompanyId { get; set; }
        public string IPAddress { get; set; }
        public string InterfaceName { get; set; }
        public int BrandId
        {
            get { return brandid; }
            set { brandid = value; }
        }

        public string FromDate
        {
            get { return fromdate; }
            set { fromdate = value; }
        }

        public int UserId
        {
            get { return userid; }
            set { userid = value; }
        }

        public int Location
        {
            get { return location; }
            set { location = value; }
        }


       /* #CC01 Add Start */
        public int EntityID { get; set; }
        public int EntityTypeID { get; set; }
        public Int16 BaseEntityTypeID { get; set; }
        public DateTime Date { get; set; }
        public Int16 ComingFor { get; set; }
        public int TotalRecords { get; set; }
        public int Result { get; set; }
        public string Error { get; set; }
        /* #CC01 Add End */

        public DataTable SelectOrganizationHierarchyinfo()
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@userid", userid);
                SqlParam[1] = new SqlParameter("@allowHierarchy", SqlDbType.Int);
                SqlParam[1].Direction = ParameterDirection.Output;
                d1 = DataAccess.Instance.GetTableFromDatabase("prcDashBoardOrgHierarchies", CommandType.StoredProcedure, SqlParam);
                ishierarchyallowed = Convert.ToInt32(SqlParam[1].Value);
                return d1;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet SelectDashBoardPrimaryData()
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@fromdate",fromdate);
                SqlParam[1] = new SqlParameter("@todate", todate);
                SqlParam[2] = new SqlParameter("@orgHierId",location);
                SqlParam[3] = new SqlParameter("@userid", userid);
                SqlParam[4] = new SqlParameter("@brandid",brandid);
                ds = DataAccess.Instance.GetDataSetFromDatabase("prcGetDashBoardPrimaryInterData", CommandType.StoredProcedure, SqlParam);
                return ds;
             }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet SelectDashBoardSecondaryData()
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@fromdate", fromdate);
                SqlParam[1] = new SqlParameter("@todate", todate);
                SqlParam[2] = new SqlParameter("@orgHierId", location);
                SqlParam[3] = new SqlParameter("@userid", userid);
                SqlParam[4] = new SqlParameter("@brandid", brandid);
                ds = DataAccess.Instance.GetDataSetFromDatabase("prcGetDashBoardSecondaryData", CommandType.StoredProcedure, SqlParam);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataSet SelectDashBoardRetailerData()
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@fromdate", fromdate);
                SqlParam[1] = new SqlParameter("@todate", todate);
                SqlParam[2] = new SqlParameter("@orgHierId", location);
                SqlParam[3] = new SqlParameter("@userid", userid);
                SqlParam[4] = new SqlParameter("@brandid", brandid);
                ds = DataAccess.Instance.GetDataSetFromDatabase("prcGetDashBoardReatailerData", CommandType.StoredProcedure, SqlParam);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


      public DataSet SelectDashBoardStockData()
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@fromdate", fromdate);
                SqlParam[1] = new SqlParameter("@todate", todate);
                SqlParam[2] = new SqlParameter("@orgHierId", location);
                SqlParam[3] = new SqlParameter("@userid", userid);
                SqlParam[4] = new SqlParameter("@brandid", brandid);
                ds = DataAccess.Instance.GetDataSetFromDatabase("prcGetDashBoardStockData", CommandType.StoredProcedure, SqlParam);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       /* #CC01 Add Start */

      public DataSet GetSupervisorDashboardData()
      {
          try
          {
              DataSet dsResult = new DataSet();
              SqlParameter[] objSqlParam = new SqlParameter[9];
              objSqlParam[0] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
              objSqlParam[0].Direction = ParameterDirection.Output;
              objSqlParam[1] = new SqlParameter("@UserID", UserId);
              objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.BigInt, 8);
              objSqlParam[2].Direction = ParameterDirection.Output;
              objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 2000);
              objSqlParam[3].Direction = ParameterDirection.Output;
              objSqlParam[4] = new SqlParameter("@Date", Date);
              objSqlParam[5] = new SqlParameter("@EntityId", EntityID);
              objSqlParam[6] = new SqlParameter("@EntityTypeID", EntityTypeID);
              objSqlParam[7] = new SqlParameter("@BaseEntityTypeID", BaseEntityTypeID);
              objSqlParam[8] = new SqlParameter("@ComingFor", ComingFor);
              dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcSupervisorDashboard", CommandType.StoredProcedure, objSqlParam);

              TotalRecords = Convert.ToInt32(objSqlParam[0].Value);
              Result = Convert.ToInt16(objSqlParam[2].Value);
              if (objSqlParam[3].Value != null && Convert.ToString(objSqlParam[3].Value)!="")
              Error = Convert.ToString(objSqlParam[3].Value);

              return dsResult;
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }


      public DataSet GetSupervisorDashboardType()
      {
          try
          {
              DataSet dsResult = new DataSet();
              SqlParameter[] objSqlParam = new SqlParameter[5];
              objSqlParam[0] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
              objSqlParam[0].Direction = ParameterDirection.Output;
              objSqlParam[1] = new SqlParameter("@UserID", UserId);
              objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.BigInt, 8);
              objSqlParam[2].Direction = ParameterDirection.Output;
              objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 2000);
              objSqlParam[3].Direction = ParameterDirection.Output;
              dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetEntityTypeForSupervisiorDashboard", CommandType.StoredProcedure, objSqlParam);
              TotalRecords = Convert.ToInt32(objSqlParam[0].Value);
              Result = Convert.ToInt16(objSqlParam[2].Value);
              if (objSqlParam[3].Value != null && Convert.ToString(objSqlParam[3].Value) != "")
                  Error = Convert.ToString(objSqlParam[3].Value);

              return dsResult;
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public DataSet GetSupervisorDashboardEntity()
      {
          try
          {
              DataSet dsResult = new DataSet();
              SqlParameter[] objSqlParam = new SqlParameter[5];
              objSqlParam[0] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
              objSqlParam[0].Direction = ParameterDirection.Output;
              objSqlParam[1] = new SqlParameter("@UserID", UserId);
              objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.BigInt, 8);
              objSqlParam[2].Direction = ParameterDirection.Output;
              objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 2000);
              objSqlParam[3].Direction = ParameterDirection.Output;
              objSqlParam[4] = new SqlParameter("@EntityTypeID", EntityTypeID);
              dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetEntityForSupervisiorDashboard", CommandType.StoredProcedure, objSqlParam);
              TotalRecords = Convert.ToInt32(objSqlParam[0].Value);
              Result = Convert.ToInt16(objSqlParam[2].Value);
              if (objSqlParam[3].Value != null && Convert.ToString(objSqlParam[3].Value) != "")
                  Error = Convert.ToString(objSqlParam[3].Value);

              return dsResult;
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      public Int32 SaveGoogleAPICountHit()
      {
          Int32 Result = 1;
          try
          {
             
              SqlParameter[] objSqlParam = new SqlParameter[9];
              objSqlParam[0] = new SqlParameter("@UserID", UserId);
              objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.BigInt, 8);
              objSqlParam[1].Direction = ParameterDirection.Output;
              objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 2000);
              objSqlParam[2].Direction = ParameterDirection.Output;
              objSqlParam[3] = new SqlParameter("@CompanyId", CompanyId);
              objSqlParam[4] = new SqlParameter("@IPAddress", IPAddress);
              objSqlParam[5] = new SqlParameter("@InterfaceName", InterfaceName);
              Result = DataAccess.Instance.DBInsertCommand("prcInsertGoogleAPIKeyLog", objSqlParam);
              return Result;
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      public DataSet GetSupervisorDashboardDataV1()
      {
          try
          {
              DataSet dsResult = new DataSet();
              SqlParameter[] objSqlParam = new SqlParameter[9];
              objSqlParam[0] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
              objSqlParam[0].Direction = ParameterDirection.Output;
              objSqlParam[1] = new SqlParameter("@UserID", UserId);
              objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.BigInt, 8);
              objSqlParam[2].Direction = ParameterDirection.Output;
              objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 2000);
              objSqlParam[3].Direction = ParameterDirection.Output;
              objSqlParam[4] = new SqlParameter("@Date", Date);
              objSqlParam[5] = new SqlParameter("@EntityId", EntityID);
              objSqlParam[6] = new SqlParameter("@EntityTypeID", EntityTypeID);
              objSqlParam[7] = new SqlParameter("@BaseEntityTypeID", BaseEntityTypeID);
              objSqlParam[8] = new SqlParameter("@ComingFor", ComingFor);
              dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcSupervisorDashboardV1", CommandType.StoredProcedure, objSqlParam);

              TotalRecords = Convert.ToInt32(objSqlParam[0].Value);
              Result = Convert.ToInt16(objSqlParam[2].Value);
              if (objSqlParam[3].Value != null && Convert.ToString(objSqlParam[3].Value) != "")
                  Error = Convert.ToString(objSqlParam[3].Value);

              return dsResult;
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      /* #CC01 Add End */



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

        ~dashBoard()
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
