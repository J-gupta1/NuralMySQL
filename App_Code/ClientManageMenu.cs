using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace DataAccess
{
    public class ClientManageMenu : IDisposable
    {
        # region declaration
        SqlParameter[] SqlParam;
        DataTable dtResult;
        # endregion
        # region Properties
        private string _Error;
        public string Error
        {
            get { return _Error; }
            set { _Error = value; }
        }
        private string _MenuName;
        public string MenuName
        {
            get { return _MenuName; }
            set { _MenuName = value; }
        }
        private string _MenuDesc;
        public string MenuDesc
        {
            get { return _MenuDesc; }
            set { _MenuDesc = value; }
        }
        private string _InsError;
        public string InsError
        {
            get { return _InsError; }
            set { _InsError = value; }
        }
        private int _Condition;
        public int Condition
        {
            get { return _Condition; }
            set { _Condition = value; }
        }
        private int _UserId;
        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }
        private int _Status;
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        private int _AllowInMenu;
        public int AllowInMenu
        {
            get { return _AllowInMenu; }
            set { _AllowInMenu = value; }
        }
        private int _AccessRole;
        public int AccessRole
        {
            get { return _AccessRole; }
            set { _AccessRole = value; }
        }
        private int _MenuID;
        public int MenuID
        {
            get { return _MenuID; }
            set { _MenuID = value; }
        }
        private int _ParentMenuID;
        public int ParentMenuID
        {
            get { return _ParentMenuID; }
            set { _ParentMenuID = value; }
        }
        private int _DisplayOrderNumber;
        public int DisplayOrderNumber
        {
            get { return _DisplayOrderNumber; }
            set { _DisplayOrderNumber = value; }
        }
        private int _NewParent;
        public int NewParent
        {
            get { return _NewParent; }
            set { _NewParent = value; }
        }
        private int _OldParent;
        public int OldParent
        {
            get { return _OldParent; }
            set { _OldParent = value; }
        }
        private int _ClientId;
        public int ClientId
        {
            get { return _ClientId; }
            set { _ClientId = value; }
        }
        # endregion
        # region Methodes
        public DataTable GetClientName()
        {
            try
            {
                dtResult = new DataTable();
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@Condition", Condition);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetClientMenu", CommandType.StoredProcedure, SqlParam);
                Error = Convert.ToString(SqlParam[1].Value);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetDeflaultMenuData()
        {
            try
            {
                dtResult = new DataTable();
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@UserId", UserId);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetDefaultMenu", CommandType.StoredProcedure, SqlParam);
                Error = Convert.ToString(SqlParam[1].Value);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetClientMenuData(int ClientID)
        {
            try
            {
                dtResult = new DataTable();
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ClientID", ClientID);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetClientMenuData", CommandType.StoredProcedure, SqlParam);
                Error = Convert.ToString(SqlParam[1].Value);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsClientMenuValue(DataTable dt)
        {
            try
            {
                dtResult = new DataTable();
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@InsertAll", dt);
                DataAccess.Instance.DBInsertCommand("prcInsClientMenuValue", SqlParam);
                Error = Convert.ToString(SqlParam[1].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdClientMenuValue()
        {
            try
            {
                dtResult = new DataTable();
                SqlParam = new SqlParameter[10];
                SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@MenuName", MenuName);
                SqlParam[3] = new SqlParameter("@MenuDesc", MenuDesc);
                SqlParam[4] = new SqlParameter("@Status", Status);
                SqlParam[5] = new SqlParameter("@AllowInMenu", AllowInMenu);
                SqlParam[6] = new SqlParameter("@AccessRole", AccessRole);
                SqlParam[7] = new SqlParameter("@MenuID", MenuID);
                SqlParam[8] = new SqlParameter("@ParentMenuID", ParentMenuID);
                SqlParam[9] = new SqlParameter("@ClientID", ClientId);
                DataAccess.Instance.DBInsertCommand("prcUpdClientMenuValue", SqlParam);
                Error = Convert.ToString(SqlParam[1].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //prcUpdClientMenuValue
        }
        public void DelClientMenuValue()
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@MenuId", MenuID);
                SqlParam[1] = new SqlParameter("@ClientId", ClientId);
                DataAccess.Instance.DBInsertCommand("prcDelClientMenuValue", SqlParam);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void ChengePositionClientTreeMenu(string Condition)
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@MenuId", MenuID);
                SqlParam[1] = new SqlParameter("@ParentMenuID", ParentMenuID);
                SqlParam[2] = new SqlParameter("@Condition", Condition);
                SqlParam[3] = new SqlParameter("@InsError", SqlDbType.VarChar, 200);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@ClientID", ClientId);
                DataAccess.Instance.DBInsertCommand("prcChengeOrderClientTreeMenu", SqlParam);
                if (SqlParam[3].Value.ToString() != "")
                {
                    InsError = SqlParam[3].Value.ToString();
                }
                else { InsError = null; }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void ClientMenuDragNode()
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@MenuId", MenuID);
                SqlParam[1] = new SqlParameter("@DisplayOrderNumber", DisplayOrderNumber);
                SqlParam[2] = new SqlParameter("@NewParent", NewParent);
                SqlParam[3] = new SqlParameter("@OldParent", OldParent);
                SqlParam[4] = new SqlParameter("@InsError", SqlDbType.VarChar, 200);
                SqlParam[4].Direction = ParameterDirection.Output;
                SqlParam[5] = new SqlParameter("@ClientID", ClientId);
                DataAccess.Instance.DBInsertCommand("prcClientMenuDragNode", SqlParam);
                if (SqlParam[4].Value.ToString() != "")
                {
                    InsError = SqlParam[4].Value.ToString();
                }
                else { InsError = null; }
            }
            catch (Exception ex)
            {

                throw ex;
            }
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

        ~ClientManageMenu()
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
