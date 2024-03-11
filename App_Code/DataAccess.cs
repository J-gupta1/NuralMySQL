using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
/*
 * 17-Dec-2014, Sumit Kumar, #CC01, Insert Bulk GRN in table
 * 09-Jan-2014, Sumit Kumar, #CC02, Insert Bulk Excel data for Primary IMEI Ack
 *  24-Apr-2023, Hema Thapa, #CC03, MySQL connections
 */
namespace DataAccess
{
    public class DataAccess : IDisposable
    {


        private static DataAccess _instance;
        Int32 intReturn;
        DataSet _dtset;
        SqlTransaction SqlTrans;
        public static string _ConnectionString = ConfigurationManager.ConnectionStrings["AppConString"].ConnectionString;





        public static DataAccess Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataAccess();
                }
                return _instance;
            }
        }

        private DataAccess()
        {
            SqlHelper.commandTimeOut = 600;
        }


        public SqlTransaction SetTrascation
        {
            get { return SqlTrans; }
            set { SqlTrans = value; }
        }

        #region For Single Select
        public object getSingleValues(string Spname)
        {

            try
            {
                return SqlHelper.ExecuteScalar(_ConnectionString, CommandType.StoredProcedure, Spname);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public object getSingleValues(string SpName, SqlParameter[] param)
        {
            try
            {
                return SqlHelper.ExecuteScalar(_ConnectionString, CommandType.StoredProcedure, SpName, param);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region For Complete table or dataset select
        public DataTable GetTableFromDatabase(string SpName, CommandType CmdType)
        {
            try
            {
                _dtset = new DataSet();

                _dtset = SqlHelper.ExecuteDataset(_ConnectionString, CmdType, SpName);
                return _dtset.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_dtset != null)
                {
                    _dtset.Dispose();
                }
            }
        }

        /* #CC03 add start  */
        public DataTable GetTableFrom_MySqlDatabase(string SpName, CommandType CmdType)
        {
            try
            {
                _dtset = new DataSet();
                _dtset = NuralMySqlHelper.ExecuteDataset(_ConnectionString, SpName, CmdType);
                return _dtset.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_dtset != null)
                {
                    _dtset.Dispose();
                }
            }
        }
        //GetTableFrom_MySqlDatabase
        public DataTable GetTableFrom_MySqlDatabase(string SpName, CommandType CmdType, MySqlParameter[] Sqlpara)
        {
            try
            {
                _dtset = new DataSet();
                _dtset = NuralMySqlHelper.ExecuteDataset(_ConnectionString, SpName, CmdType, Sqlpara);
                return _dtset.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_dtset != null)
                {
                    _dtset.Dispose();
                }
            }
        }

        /* #CC03 add end  */

        public DataTable GetTableFromDatabase(string SpName, CommandType CmdType, SqlParameter[] Sqlpara)
        {
            try
            {
                _dtset = new DataSet();
                _dtset = SqlHelper.ExecuteDataset(_ConnectionString, CmdType, SpName, Sqlpara);
                return _dtset.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_dtset != null)
                {
                    _dtset.Dispose();
                }
            }
        }

        public DataSet GetDataSetFromDatabase(string SpName, CommandType CmdType)
        {
            try
            {
                _dtset = new DataSet();
                _dtset = SqlHelper.ExecuteDataset(_ConnectionString, CmdType, SpName);
                return _dtset;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_dtset != null)
                {
                    _dtset.Dispose();
                }
            }
        }


        public DataSet GetDataSetFromDatabase(string SpName, CommandType CmdType, SqlParameter[] Sqlpara)
        {
            try
            {
                _dtset = new DataSet();
                _dtset = SqlHelper.ExecuteDataset(_ConnectionString, CmdType, SpName, Sqlpara);
                return _dtset;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_dtset != null)
                {
                    _dtset.Dispose();
                }
            }
        }

        /* #CC03 add start */
        public DataSet GetDataSetFrom_MySqlDatabase(string SpName, CommandType CmdType) // replaced with GetDataSetFromDatabase()
        {
            try
            {
                _dtset = new DataSet();
                _dtset = NuralMySqlHelper.ExecuteDataset(_ConnectionString, SpName, CmdType);
                return _dtset;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_dtset != null)
                {
                    _dtset.Dispose();
                }
            }
        }
        public DataSet GetDataSetFrom_MySqlDatabase(string SpName, CommandType CmdType, MySqlParameter[] Sqlpara)// replaced with GetDataSetFromDatabase()
        {
            try
            {
                _dtset = new DataSet();
                _dtset = NuralMySqlHelper.ExecuteDataset(_ConnectionString, SpName, CmdType,  Sqlpara);
                return _dtset;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_dtset != null)
                {
                    _dtset.Dispose();
                }
            }
        }

        /* #CC03 add end */
        #endregion

        public Int32 DBInsertCommand(string SpName, SqlParameter[] SqlParam)
        {

            try
            {
                intReturn = SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, SpName, SqlParam);
                return intReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /* #CC03 add start */
        public Int32 DBInsert_MySqlCommand(string SpName, MySqlParameter[] SqlParam) // replaced with DBInsertCommand()
        {

            try
            {
                intReturn = NuralMySqlHelper.ExecuteNonQuery(_ConnectionString, SpName, CommandType.StoredProcedure, SqlParam);
                return intReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /* #CC03 add end */

        public Int32 DBInsertCommand(string SpName, SqlParameter[] SqlParam, SqlTransaction ObjTrans)
        {
            try
            {
                intReturn = SqlHelper.ExecuteNonQuery(ObjTrans, CommandType.StoredProcedure, SpName, SqlParam);
                return intReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Int32 DBInsertCommand(string SpName, CommandType cmd)
        {

            try
            {
                intReturn = SqlHelper.ExecuteNonQuery(_ConnectionString, cmd, SpName);
                return intReturn;
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

        ~DataAccess()
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

        //Insert Bulk Excel data for GRN and Primary//
        /*#CC01 START ADDED*/
        public Int32 DBInsertBulkData(string SpName, Dictionary<string, object> dicpara, DataTable dt)
        {
            try
            {
                SqlConnection con = new SqlConnection(_ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand(SpName, con);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (KeyValuePair<string, object> getdicpara in dicpara)
                {
                    cmd.Parameters.AddWithValue(getdicpara.Key, getdicpara.Value);
                }
                cmd.Parameters.AddWithValue("@TypeParameter", SqlDbType.Structured).Value = dt;
                cmd.Parameters.AddWithValue("@Out_Param", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                Int32 rowAffected = Convert.ToInt32(cmd.Parameters["@Out_Param"].Value);
                con.Close();
                return rowAffected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*#CC01 START END*/


        //Insert Bulk Excel data for Primary IMEI Ack//
        /*#CC02 START ADDED*/
        public bool BCP_IMEIAck(DataTable dt)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(_ConnectionString, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "TransactionIMEIAckBulk";
                bulkCopy.ColumnMappings.Add("IMEI", "IMEI");
                bulkCopy.ColumnMappings.Add("SalesChannelID", "SalesChannelID");
                bulkCopy.ColumnMappings.Add("AckSessionID", "AckSessionID");             
                bulkCopy.WriteToServer(dt);
                return true;
            }
        }
        /*#CC02 END*/
    }

}
