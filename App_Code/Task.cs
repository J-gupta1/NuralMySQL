﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Xml;

namespace DataAccess
{
    /// <summary>
    /// Summary description for Task
    /// </summary>

    public class Task : IDisposable
    {
        public DataTable Dt;
        public string ConString = ConfigurationManager.ConnectionStrings["AppConString"].ConnectionString;
        public Int16 OutParam
        {
            get;
            set;
        }
        SqlParameter[] SqlParam;
        public string error { get; set; }
        public int UserID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Int32 TotalRecords { get; set; }
        public Int32 CategoryForId { get; set; }
        public Int32 TaskStatusId { get; set; }
        public Int32 TaskPriorityId { get; set; }
        public Int32 PageIndex { get; set; }
        public Int32 PageSize { get; set; }
        public string TaskDescription { get; set; }
        public string Remark { get; set; }
        public Int64 TaskForUserId {get;set;}
        public Int64 TaskUserID {get;set;}
        public Int64 CompanyId {get;set;}
        public Int32 EntityTypeId { get; set; }
        public string CompanyImageFolder { get; set; }
        public Int32 TaskGroupId { get; set; }
        public Int64 EntitytypeUserId
        {
            get;
            set;
        }
        public Int64 TaskResponseId { get; set; }
        public Task()
        {
            //
            // TODO: Add constructor logic here
            //
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

        ~Task()
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

        public DataTable GetTaskUserList()
        {
            try
            {
                DataTable dsresult = new DataTable();
                SqlParameter[] objSqlParam = new SqlParameter[6];
                objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                objSqlParam[0].Direction = ParameterDirection.Output;
                objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 3000);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new SqlParameter("@UserID", UserID);
                objSqlParam[3] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
                objSqlParam[3].Direction = ParameterDirection.Output;
                objSqlParam[4] = new SqlParameter("@AccountIdOut", SqlDbType.Int);
                objSqlParam[4].Direction = ParameterDirection.Output;
                objSqlParam[5] = new SqlParameter("@AuthKey", "");
                dsresult = DataAccess.Instance.GetTableFromDatabase("prcAPIGetTaskUserList", CommandType.StoredProcedure, objSqlParam);
                OutParam = Convert.ToInt16(objSqlParam[0].Value);
                if (objSqlParam[1].Value != null && Convert.ToString(objSqlParam[1].Value).Trim() != "")
                {
                    error = Convert.ToString(objSqlParam[1].Value);
                }
                TotalRecords = Convert.ToInt32(objSqlParam[3].Value);
                return dsresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCategoryList()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@UserId", UserID);
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 2000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@AccountIdOut", SqlDbType.Int);
                SqlParam[4].Direction = ParameterDirection.Output;
                SqlParam[5] = new SqlParameter("@AuthKey", "");
                SqlParam[6] = new SqlParameter("@CategoryID", CategoryForId);
                ds = DataAccess.Instance.GetDataSetFromDatabase("prcApiCategoryMaster_Select", CommandType.StoredProcedure, SqlParam);
                
                OutParam = Convert.ToInt16(SqlParam[1].Value);
                
                if (SqlParam[0].Value != null)
                {
                    error = Convert.ToString(SqlParam[2].Value);
                }
                TotalRecords = Convert.ToInt32(SqlParam[3].Value);
            }
            catch (Exception ex)
            {
                
                OutParam = 1;
            }
            return ds;

        }
        
        public void InsertTask()
        {
            try
            {
                SqlParameter[] SqlParam = new SqlParameter[14];
                SqlParam[0] = new SqlParameter("@CreatedBy", UserID);
                SqlParam[1] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@OutError", SqlDbType.NVarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@TaskCreationDate", StartDate);
                SqlParam[4] = new SqlParameter("@TaskStartDate", StartDate);
                SqlParam[5] = new SqlParameter("@TaskEndDate", EndDate);
                SqlParam[6] = new SqlParameter("@PriorityType", TaskPriorityId);
                SqlParam[7] = new SqlParameter("@TvpUserList", SqlDbType.Structured);
                SqlParam[7].Value = Dt;
                SqlParam[8] = new SqlParameter("@TaskDescription", TaskDescription);
                SqlParam[9] = new SqlParameter("@Remarks", Remark);
                SqlParam[10] = new SqlParameter("@AccountIdOut", SqlDbType.Int);
                SqlParam[10].Direction = ParameterDirection.Output;
                SqlParam[11] = new SqlParameter("@AuthKey", "");
                SqlParam[12] = new SqlParameter("@TaskStatus", TaskStatusId);
                SqlParam[13] = new SqlParameter("@TaskGroupId", TaskGroupId);
                DataAccess.Instance.DBInsertCommand("prcAPISaveTaskDetail", SqlParam);
                OutParam = Convert.ToInt16(SqlParam[1].Value);
                error = Convert.ToString(SqlParam[2].Value);
                
                
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertTaskResponse()
        {
            try
            {
                SqlParameter[] SqlParam = new SqlParameter[10];
                SqlParam[0] = new SqlParameter("@CreatedBy", UserID);
                SqlParam[1] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@OutError", SqlDbType.NVarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@TaskUserId",TaskUserID);
                SqlParam[4] = new SqlParameter("@NextClosureDate", StartDate);
                SqlParam[5] = new SqlParameter("@TaskStatus", TaskStatusId);
                SqlParam[6] = new SqlParameter("@ResponseRemark", Remark);
                SqlParam[7] = new SqlParameter("@TvpImagelist", SqlDbType.Structured);
                SqlParam[7].Value =Dt;
                

                DataAccess.Instance.DBInsertCommand("prcAPISaveTaskResponseDetail", SqlParam);
                OutParam = Convert.ToInt16(SqlParam[1].Value);
                error = Convert.ToString(SqlParam[2].Value);



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getTaskData()
        {
            SqlConnection objCon = new SqlConnection(ConString);
            objCon.Open();
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] SqlParam = new SqlParameter[15];
                SqlParam[0] = new SqlParameter("@UserId", UserID);
                
                SqlParam[1] = new SqlParameter("@TaskDateFrom", StartDate);
                SqlParam[2] = new SqlParameter("@TaskDateTo", EndDate);
                SqlParam[3] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@OutError", SqlDbType.NVarChar, 500);
                SqlParam[4].Direction = ParameterDirection.Output;
                SqlParam[5] = new SqlParameter("@PriorityType", TaskPriorityId);
                SqlParam[6] = new SqlParameter("@TaskStatus", TaskStatusId);
                SqlParam[7] = new SqlParameter("@TaskForUserId", TaskForUserId);
                SqlParam[8] = new SqlParameter("@TaskUserID", TaskUserID);
                SqlParam[9] = new SqlParameter("@TotalRecord",SqlDbType.Int);
                SqlParam[9].Direction = ParameterDirection.Output;
                SqlParam[10] = new SqlParameter("@CompanyId", CompanyId);
                SqlParam[11] = new SqlParameter("@PageIndex", PageIndex);
                SqlParam[12] = new SqlParameter("@PageSize", PageSize);
                SqlParam[13] = new SqlParameter("@GetUrlCompanyWise", SqlDbType.NVarChar, 200);
                SqlParam[13].Direction = ParameterDirection.Output;
                SqlParam[14] = new SqlParameter("@TaskGroupId", TaskGroupId);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetTaskDetail", CommandType.StoredProcedure, SqlParam);
                
                /*SqlCommand objComm = new SqlCommand("prcGetTaskDetail", objCon);
                objComm.CommandType = CommandType.StoredProcedure;
                objComm.Parameters.AddRange(SqlParam);
                objComm.CommandTimeout = 200;
                using (SqlDataAdapter obAdp = new SqlDataAdapter(objComm))
                {
                    obAdp.Fill(dsResult);
                }*/

                
                if (Convert.ToString(SqlParam[4].Value) != null && Convert.ToString(SqlParam[4].Value) != "")
                    error = Convert.ToString(SqlParam[4].Value);
                TotalRecords = Convert.ToInt32(SqlParam[9].Value);
                CompanyImageFolder = Convert.ToString(SqlParam[13].Value);
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
        public DataTable GetEntityTypeV5API()
        {
            DataTable dtResult = new DataTable();
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@CompanyId", CompanyId);
                SqlParam[1] = new SqlParameter("@UserId",UserID);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetEntityTypeForPaymentReport", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetEntityTypeName()
        {
            DataTable dtResult = new DataTable();
            try
            {
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@EntityTypeId", EntityTypeId);
                SqlParam[1] = new SqlParameter("@UserId", UserID);
                SqlParam[2] = new SqlParameter("@CompanyId", CompanyId);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetEntityTypeNameForPaymentReport", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetReportTaskData()
        {
            try
            {

                SqlParameter[] prm = new SqlParameter[15];
                prm[0] = new SqlParameter("@UserId", UserID);
                prm[1] = new SqlParameter("@EntityTypeId", EntityTypeId);
                prm[2] = new SqlParameter("@EntitytypeUserId", EntitytypeUserId);
                prm[3] = new SqlParameter("@StartDate", StartDate);
                prm[4] = new SqlParameter("@EndDate", EndDate);
                prm[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
                prm[5].Direction = ParameterDirection.Output;
                prm[6] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
                prm[6].Direction = ParameterDirection.Output;
                prm[7] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
                prm[7].Direction = ParameterDirection.Output;
                prm[8] = new SqlParameter("@PageSize", PageSize);
                prm[9] = new SqlParameter("@PageIndex", PageIndex);
                prm[10] = new SqlParameter("@CompanyId", CompanyId);
                prm[11] = new SqlParameter("@TaskPriorityId", TaskPriorityId);
                prm[12] = new SqlParameter("@TaskStatusId", TaskStatusId);
                prm[13] = new SqlParameter("@TaskGroupId", TaskGroupId);
                DataSet dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetTaskDetailReport", CommandType.StoredProcedure, prm);
                error = Convert.ToString(prm[6].Value);
                TotalRecords = Convert.ToInt32(prm[7].Value);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetResponseDetail(Int64 TaskUserId)
        {
            try
            {

                SqlParameter[] prm = new SqlParameter[14];
                prm[0] = new SqlParameter("@UserId", UserID);
                prm[1] = new SqlParameter("@CopmanyId", CompanyId);
                prm[2] = new SqlParameter("@TaskUserId", TaskUserId);
                prm[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
                prm[3].Direction = ParameterDirection.Output;
                prm[4] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
                prm[4].Direction = ParameterDirection.Output;
                DataTable dsResult = DataAccess.Instance.GetTableFromDatabase("prcGetResponseDetailByTaskUserId", CommandType.StoredProcedure, prm);
                error = Convert.ToString(prm[4].Value);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetResponseViewImageInfo()
        {
            try
            {

                SqlParameter[] prm = new SqlParameter[14];
                prm[0] = new SqlParameter("@UserID", UserID);
                prm[1] = new SqlParameter("@CompanyId", CompanyId);
                prm[2] = new SqlParameter("@TaskResponseId", TaskResponseId);
                DataTable dsResult = DataAccess.Instance.GetTableFromDatabase("prcGetResponseImagePath", CommandType.StoredProcedure, prm);
                //error = Convert.ToString(prm[6].Value);
                //TotalRecords = Convert.ToInt32(prm[7].Value);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    
    }
}