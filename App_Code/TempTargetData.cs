using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DataAccess;
using System.Data.SqlTypes;
using System.Xml;
/*
 * ------------------------------------------------------------------------------------------------------------
 * Change Log
 * ------------------------------------------------------------------------------------------------------------
 * DD-MMM-YYYY, Name, #CCXX, Description.
 * 01-Feb-2018, Sumit Maurya, #CC01. New properties and method created to update target.
 * 05-Feb-2018, Sumit Maurya, #CC02, new property created and target upload method modified.
 * 14-Feb-2018, Sumit Maurya, #CC03, New method created to get targetName (Done for Comio).
 * 22-May-2018, Sumit Maurya, #CC04, Userid provided to get data according to user and try catch block added .(Done for motorola).
 * ------------------------------------------------------------------------------------------------------------
 
 */
namespace DataAccess
{
    public class TempTargetData : IDisposable
    {
        #region Private Class Variables
        private int _TargetForID, _TargetID, _UserId;
        private DateTime? _TargetFrom,_TargetTo;
        private Int16 _UserTypeID,_UserType,_OwnLevel;
        SqlParameter[] SqlParam;
        DataSet dsTarget;
        DataTable dtTarget;
        Boolean _showLevel;
        Int16 _TargetPeriod;

        #endregion
        #region Public Properties
        public Boolean showLevel
        {
            get { return _showLevel; }
            set { _showLevel = value; }

        }
        public Int16 OwnLevel
        {
            get { return _OwnLevel; }
            set { _OwnLevel = value; }

        }
       
        public Int16 UserType
        {
            get { return _UserType; }
            set { _UserType = value; }
        }

        public Int16 UserTypeID
        {
            get { return _UserTypeID; }
            set { _UserTypeID = value; }
        }
      
        public int TargetForID
        {
            get { return _TargetForID; }
            set { _TargetForID = value; }
        }

        public DateTime? TargetFrom
        {
            get { return _TargetFrom; }
            set { _TargetFrom = value; }
        }
        public DateTime? TargetTo
        {
            get { return _TargetTo; }
            set { _TargetTo = value; }
        }
        public Int16  TargetPeriod
        {
            get { return _TargetPeriod; }
            set { _TargetPeriod = value; }
        }
        public int TargetID
        {
            get { return _TargetID; }
            set { _TargetID = value; }
        }
        public string ErrorMessage
        {
            get;
            set;

        }
        public string ErrorDetailXML
        {
            get;
            set;
        }
        public EnumData.eTargetTemplateType TemplateType
        {
            get;
            set;
        }

        public int UserSelectedTypeID
        {
            get;
            set;
        }

        public int SelectedTypeID
        {
            get;
            set;
        }


       
        public int UserId
        {
            get { return _UserId;}
            set { _UserId = value; }
        }
        /* #CC01 Add Start */

        public DataTable Dt
        {
            get;
            set;
        }
        /* #CC01 Add End */
        /* #CC02 Add Start */
        public Int16 InsUpdTarget
        {
            get;
            set;
        }
        /* #CC02 Add End */

        /* #CC03 Add Start */
        public Int32 result
        {
            get;
            set;
        }
        public Int64 TotalRecords
        {
            get;
            set;
        }
        public string strError
        {
            get;
            set;
        }

        public string TargetName
        {
            get;
            set;
        }
        public Int16? TargetStatus
        {
            get;
            set;
        }

        public Int16 TargetUserTypeID;
                
        public Int16 TargetUserType
        {
             get;
            set;
        }

        /* #CC03 Add End */

        #endregion
        #region Public Methods
        public DataSet GetTargetTemplate()
        {
            try
            {
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@TargetType", TemplateType);
                dsTarget = DataAccess.Instance.GetDataSetFromDatabase("prcgetTargetTemplate", CommandType.StoredProcedure, SqlParam);
                return dsTarget;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void UploadTarget(DataTable Dt)
        {
            try
            {
                    SqlParam = new SqlParameter[5]; /* #CC02 Length increased from 4 - 5 */
                    SqlParam[0] = new SqlParameter("@tvpTarget", SqlDbType.Structured);
                    SqlParam[0].Value = Dt;
                    SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                    SqlParam[1].Direction = ParameterDirection.Output;
                    SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
                    SqlParam[2].Direction = ParameterDirection.Output;
                    SqlParam[3] = new SqlParameter("@UserId", _UserId);
                    SqlParam[4] = new SqlParameter("@InsUpdTarget", InsUpdTarget); /* #CC02 Added */
                    DataAccess.Instance.DBInsertCommand("PrcUploadTargetInfo", SqlParam);
                    if (SqlParam[2].Value != DBNull.Value)
                    {
                        ErrorDetailXML = SqlParam[2].Value.ToString();
                    }
                    else
                    {
                        ErrorDetailXML = null;
                    }
                /* #CC02 Add Start */
                if(SqlParam[1].Value!=null && Convert.ToString(SqlParam[1].Value)!="")
                {
                    ErrorMessage = Convert.ToString(SqlParam[1].Value);
                }
                /* #CC02 Add End */
                   
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        
        public DataTable GetTargetInfo()
        {
            try
            {
                SqlParam = new SqlParameter[8]; /* #CC03 length increased From 6 to 7 */
                SqlParam[0] = new SqlParameter("@TargetID", _TargetID);
                SqlParam[1] = new SqlParameter("@TargetForID", _TargetForID);
                SqlParam[2] = new SqlParameter("@FromDate", _TargetFrom);
                SqlParam[3] = new SqlParameter("@ToDate", _TargetTo);
                SqlParam[4] = new SqlParameter("@UserTypeiD", _UserTypeID);
                SqlParam[5] = new SqlParameter("@UserType", _UserType);
                SqlParam[6] = new SqlParameter("@TargetName", TargetName); /* #CC03 Added */
             
                dtTarget = DataAccess.Instance.GetTableFromDatabase("prcGetTargetInfo", CommandType.StoredProcedure, SqlParam);
                return dtTarget;

                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /* #CC01 Add Start */
        public int UpdateTarget()
        {
            try
            {
                Int16 Result;
                SqlParam = new SqlParameter[4];

                SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.Int);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@UserId", UserId);
                SqlParam[3] = new SqlParameter("@TargetDetail", SqlDbType.Structured);
                SqlParam[3].Value = Dt;
                DataAccess.Instance.DBInsertCommand("prcUpdateTarget", SqlParam);

                Result = Convert.ToInt16(SqlParam[0].Value);
                if (SqlParam[1].Value != null && Convert.ToString(SqlParam[1].Value) != "")
                    ErrorMessage = Convert.ToString(SqlParam[1].Value);

                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /* #CC01 Add End */


        /* #CC03 Add Start */
        public DataSet GetTargetName()
        {
            try
            {
                SqlParam = new SqlParameter[8];
                SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@UserId", UserId);
                SqlParam[3] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@TargetName", TargetName);
                SqlParam[5] = new SqlParameter("@TargetUserTypeID", TargetUserTypeID);
                SqlParam[6] = new SqlParameter("@TargetUserType", TargetUserType);
                SqlParam[7] = new SqlParameter("@TargetStatus", TargetStatus); 
                DataSet ds = DataAccess.Instance.GetDataSetFromDatabase("prcGetTargetName", CommandType.StoredProcedure, SqlParam);
                result = Convert.ToInt32(SqlParam[0].Value);
                if (SqlParam[1].Value != null && Convert.ToString(SqlParam[1].Value) != "")
                {
                    strError = Convert.ToString(SqlParam[2].Value);
                }
                TotalRecords = Convert.ToInt32(SqlParam[2].Value);
                return ds;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /* #CC03 Add End */
        #endregion
        #region Delete Credit Note
        public Int32 DeleteTargetInfo()
        {
            try
            {
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@TargetID", _TargetID);
                SqlParam[1] = new SqlParameter("@ErrorMsg", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.Int);
                SqlParam[2].Direction = ParameterDirection.Output;
                DataAccess.Instance.DBInsertCommand("prcDelTargetInfo", SqlParam);
                ErrorMessage = Convert.ToString(SqlParam[1].Value);
                return Convert.ToInt32(SqlParam[2].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable GetUsersFortarget()
        {
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@userid",UserId);
            SqlParam[1] = new SqlParameter("@usertypeid",UserTypeID);
            SqlParam[2] = new SqlParameter("@selectedtypeid",SelectedTypeID);
            dtTarget = DataAccess.Instance.GetTableFromDatabase("[prcGetUserTypesForTarget]", CommandType.StoredProcedure, SqlParam);
            return dtTarget;
        }
        


        public DataTable GetTargetLevelUser()
        {
            SqlParam = new SqlParameter[5]; /* #CC04 Length increased from 4 to 5  */
            SqlParam[0] = new SqlParameter("@TargetUserType", _UserType);
            SqlParam[1] = new SqlParameter("@TargetUserTypeId", _UserTypeID);
            SqlParam[2] = new SqlParameter("@TargetUserLevel", _OwnLevel);
            SqlParam[3] = new SqlParameter("@ShowLevel", _showLevel);
            SqlParam[4] = new SqlParameter("@Userid", UserId); /* #CC04 Added */
            dtTarget = DataAccess.Instance.GetTableFromDatabase("prcGetTargetLeveluser", CommandType.StoredProcedure, SqlParam);
            return dtTarget;
        }
        public DataTable GetTimePeriod()
        {
            dtTarget = DataAccess.Instance.GetTableFromDatabase("prcGetTargetPeriod", CommandType.StoredProcedure, SqlParam);
            return dtTarget;
        }
        #endregion
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

        ~TempTargetData()
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
                    if (SqlParam != null)
                    {
                        SqlParam = null;
                    }
                    // Released managed Resources
                }
            }
        }

        #endregion
    }
}
