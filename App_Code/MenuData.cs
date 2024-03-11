using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
/*
 * 27-May-2015, Karam Chand Sharma,#CC01, Add some properties and some function for rites (zedcontrol button)
 * 22-Jan-2018, Sumit Maurya, #CC02, New properties and method created to for user role module.
 *  24-Apr-2023, Hema Thapa, #CC03, MySQL connections
 */
namespace DataAccess
{
    public class MenuData : IDisposable
    {
        DataTable dtmenu;
        DataSet dsMenu;
        SqlParameter[] SqlPara;
        MySqlParameter[] MySqlPara;
        DataRelation dsMenuRelation;
        Int32 IntResultCount = 0;
        public Int32 CompanyId { get; set; }
        # region Property
        /*#CC01 START ADDED*/
        private Int16 _intActiveStatus;
        public Int16 ActiveStatus
        {
            get { return _intActiveStatus; }
            set { _intActiveStatus = value; }
        }
        /*#CC01 START END*/
        private string _Error;
        public string Error
        {
            get { return _Error; }
            set { _Error = value; }
        }
        private string _InsError;
        public string InsError
        {
            get { return _InsError; }
            set { _InsError = value; }
        }
        private string _MenuName;
        public string MenuName
        {
            get { return _MenuName; }
            set { _MenuName = value; }
        }
        private string _MenuDescription;
        public string MenuDescription
        {
            get { return _MenuDescription; }
            set { _MenuDescription = value; }
        }
        private int _DisplayOrderNumber;
        public int DisplayOrderNumber
        {
            get { return _DisplayOrderNumber; }
            set { _DisplayOrderNumber = value; }
        }
        private string _NavigationURL;
        public string NavigationURL
        {
            get { return _NavigationURL; }
            set { _NavigationURL = value; }
        }
        private int _ParentMenuID;
        public int ParentMenuID
        {
            get { return _ParentMenuID; }
            set { _ParentMenuID = value; }
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
        private int _AccessFor;
        public int AccessFor
        {
            get { return _AccessFor; }
            set { _AccessFor = value; }
        }
        private int _MenuID;
        public int MenuID
        {
            get { return _MenuID; }
            set { _MenuID = value; }
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
        private int _RoleID;
        public int RoleID
        {
            get { return _RoleID; }
            set { _RoleID = value; }
        }
        private int _UserID;
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        private int _MenuType;
        public int MenuType
        {
            get { return _MenuType; }
            set { _MenuType = value; }
        }
        private int _AccessRole;
        public int AccessRole
        {
            get { return _AccessRole; }
            set { _AccessRole = value; }
        }
        # endregion


        private byte _MenuTypeID = 1  /* Menu for web */;

        public byte MenuTypeID
        {
            get { return _MenuTypeID; }
            set { _MenuTypeID = value; }
        }
        /*#CC01 START ADDED*/

        /* #CC02 Add Start */

        public DataTable dtExclude
        {
            get;
            set;
        }


        public Int16 UpdateAll
        {
            get;
            set;
        }

        public Int32 intOutParam
        {
            get;
            set;
        }

        public Int32 PageIndex
        {
            get;
            set;
        }
        public Int32 PageSize
        {
            get;
            set;
        }
        public Int32 TotalRecords
        {
            get;
            set;
        }
        public string LoginName
        {
            get;
            set;
        }
        /* #CC02 Add Start */
        public DataTable Select()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@ActiveStatus", ActiveStatus);
            objSqlParam[1] = new SqlParameter("@UserId", UserID);
            dtResult = DataAccess.Instance.GetTableFromDatabase("prcMenu_Select", CommandType.StoredProcedure, objSqlParam);
            return dtResult;
        }

        public DataSet SearchByEntityTypeRoleModuleMappingByMenuID()
        {
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@MenuID", MenuID);
            objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[1].Direction = ParameterDirection.Output;
            DataSet dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcUserMenuMapping_SelectMudulewise", CommandType.StoredProcedure, objSqlParam);
            Error = Convert.ToString(objSqlParam[1].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }
            return dsResult;
        }


        public Int16 UpdateMapping_Permission_ModuleWise(DataTable dtModulePermission)
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[5];
            objSqlParam[0] = new SqlParameter("@MenuID", MenuID);
            objSqlParam[1] = new SqlParameter("@tvpEntityRoleModulePermission", SqlDbType.Structured);
            objSqlParam[1].Value = dtModulePermission;
            objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@ModifiedBy", UserID);
            DataAccess.Instance.DBInsertCommand("prcInsUpdEntityTypeRoleModulePermission", objSqlParam);
            result = Convert.ToInt16(objSqlParam[2].Value);
            Error = Convert.ToString(objSqlParam[3].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return result;
        }
        /*#CC01 START END*/
        public DataSet getMenuHirechyByUserID(int UserID)
        {
            try
            {
                dsMenu = new DataSet();

                /* #CC03 add start*/
                MySqlPara = new MySqlParameter[2];
                MySqlPara[0] = new MySqlParameter("@p_UserID", UserID);
                MySqlPara[1] = new MySqlParameter("@p_MenuTypeID", MenuTypeID);
                dsMenu = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("PrcGetRecursiveMenubyUserID", CommandType.StoredProcedure, MySqlPara);

                /* #CC03 add end*/

                /* #CC03 comment start*/
                //SqlPara = new SqlParameter[2];
                //SqlPara[0] = new SqlParameter("@UserID", UserID);
                //SqlPara[1] = new SqlParameter("@MenuTypeID", MenuTypeID);
                //dsMenu = DataAccess.Instance.GetDataSetFromDatabase("PrcGetRecursiveMenubyUserID", CommandType.StoredProcedure, SqlPara);

                /* #CC03 comment end*/

                dsMenu.DataSetName = "Menus";
                dsMenu.Tables[0].TableName = "Menu";
                
                
                   dsMenuRelation = new DataRelation("ParentChild",
                        dsMenu.Tables["Menu"].Columns["MenuID"],
                        dsMenu.Tables["Menu"].Columns["ParentMenuID"],
                        false);
                    dsMenuRelation.Nested = true;
                    dsMenu.Relations.Add(dsMenuRelation);
                    return dsMenu;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable GetMenuInfo()
        {
            try
            {
                dtmenu = new DataTable();
                dtmenu = DataAccess.Instance.GetTableFromDatabase("PrcGetMenuInfo", CommandType.StoredProcedure);
                return dtmenu;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public DataTable GetMenuInfo(string keyword)
        {
            try
            {
                dtmenu = new DataTable();
                SqlPara = new SqlParameter[1];
                SqlPara[0] = new SqlParameter("@keyword", keyword);
                dtmenu = DataAccess.Instance.GetTableFromDatabase("PrcGetMenuInfo", CommandType.StoredProcedure, SqlPara);
                return dtmenu;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public Int32 InsertUpdateMenuInfo(Int32 MenuID, string MenuName, string Description, int ParentID, string NavigationURL, bool status, int DisplayOrder, int MenuType, bool AllowInMenu)
        {
            try
            {
                SqlPara = new SqlParameter[9];
                SqlPara[0] = new SqlParameter("@MenuID", MenuID);
                SqlPara[0].Direction = ParameterDirection.InputOutput;
                SqlPara[1] = new SqlParameter("@MenuName", MenuName);
                SqlPara[2] = new SqlParameter("@Description", Description);
                SqlPara[3] = new SqlParameter("@status", status);
                SqlPara[4] = new SqlParameter("@DisplayOrder", DisplayOrder);
                if (ParentID != 0)
                    SqlPara[5] = new SqlParameter("@ParentID", ParentID);
                else
                    SqlPara[5] = new SqlParameter("@ParentID", null);
                SqlPara[6] = new SqlParameter("@NavigationURL", NavigationURL);
                SqlPara[7] = new SqlParameter("@MenuType", MenuType);
                SqlPara[8] = new SqlParameter("@AllowInMenu", AllowInMenu);

                DataAccess.Instance.DBInsertCommand("PrcInsertUpdateMenuInfo", SqlPara);
                IntResultCount = Convert.ToInt32(SqlPara[0].Value);
                return IntResultCount;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public Int32 DeleteMenuInfo(Int32 MenuID, int MenuType)
        {
            try
            {
                SqlPara = new SqlParameter[2];
                SqlPara[0] = new SqlParameter("@MenuID", MenuID);
                SqlPara[1] = new SqlParameter("@MenuType", MenuType);
                IntResultCount = DataAccess.Instance.DBInsertCommand("prcDeleteMenuInfo", SqlPara);
                return IntResultCount;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public Int32 GetNextDisplayOrder(Int32 MenuID)
        {
            try
            {
                SqlPara = new SqlParameter[1];
                SqlPara[0] = new SqlParameter("@MenuID", MenuID);
                IntResultCount = Convert.ToInt16(DataAccess.Instance.getSingleValues("PrcGetMenuDisplayOrder", SqlPara));
                return IntResultCount;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public int ToggleOtherStatus(int ID, bool status, int IDTYPE)
        {
            SqlPara = new SqlParameter[3];
            SqlPara[0] = new SqlParameter("@ID", ID);
            SqlPara[1] = new SqlParameter("@status", status);
            SqlPara[2] = new SqlParameter("@IDTYPE", IDTYPE);

            IntResultCount = DataAccess.Instance.DBInsertCommand("prcToggleOthersStatus", SqlPara);

            return IntResultCount;
        }

        #region ManageMenuTree_For_test

        //public bool checkNavigat(int NodeKye)
        //{
        //    SqlPara = new SqlParameter[2];
        //    SqlPara[0] = new SqlParameter("@getCheck", SqlDbType.Bit);
        //    SqlPara[0].Direction = ParameterDirection.Output;
        //    SqlPara[1] = new SqlParameter("@ID", NodeKye);
        //    DataAccess.Instance.DBInsertCommand("checkNavigation", CommandType.StoredProcedure);
        //    Error = Convert.ToString(SqlPara[0].Value);
        //    return Convert.ToBoolean(Error);
        //}
        public DataTable GetTreeMenuInfo()
        {
            try
            {
                dtmenu = new DataTable();
                SqlPara = new SqlParameter[2];
                SqlPara[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlPara[0].Direction = ParameterDirection.Output;
                SqlPara[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlPara[1].Direction = ParameterDirection.Output;
                dtmenu = DataAccess.Instance.GetTableFromDatabase("prcGetTreeMenu", CommandType.StoredProcedure, SqlPara);
                //dtmenu = DataAccess.Instance.GetTableFromDatabase("prcGetTreeMenu", CommandType.StoredProcedure);
                Error = Convert.ToString(SqlPara[1].Value);
                return dtmenu;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetRoleMenuMapping()
        {
            try
            {
                dtmenu = new DataTable();
                SqlPara = new SqlParameter[4];
                SqlPara[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlPara[0].Direction = ParameterDirection.Output;
                SqlPara[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlPara[1].Direction = ParameterDirection.Output;
                SqlPara[2] = new SqlParameter("@RoleId", RoleID);
                SqlPara[3] = new SqlParameter("@CompanyId", CompanyId);
                dtmenu = DataAccess.Instance.GetTableFromDatabase("prcGetRoleMenuMapping", CommandType.StoredProcedure, SqlPara);
                Error = Convert.ToString(SqlPara[1].Value);
                return dtmenu;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetTreeMenuMapping()
        {
            try
            {
                dtmenu = new DataTable();
                SqlPara = new SqlParameter[5];
                SqlPara[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlPara[0].Direction = ParameterDirection.Output;
                SqlPara[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlPara[1].Direction = ParameterDirection.Output;
                SqlPara[2] = new SqlParameter("@UserId", UserID);
                SqlPara[3] = new SqlParameter("@RoleId", RoleID);
                SqlPara[4] = new SqlParameter("@CompanyId", CompanyId);
                dtmenu = DataAccess.Instance.GetTableFromDatabase("prcGetMenuMappingTabe", CommandType.StoredProcedure, SqlPara);
                Error = Convert.ToString(SqlPara[1].Value);
                return dtmenu;
            }
            catch (Exception ex)
            { throw ex; }
        }
        public void FillTreeMenuInfo()
        {
            try
            {

                SqlPara = new SqlParameter[13];
                SqlPara[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlPara[0].Direction = ParameterDirection.Output;
                SqlPara[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlPara[1].Direction = ParameterDirection.Output;
                SqlPara[2] = new SqlParameter("@InsError", SqlDbType.VarChar, 200);
                SqlPara[2].Direction = ParameterDirection.Output;
                SqlPara[3] = new SqlParameter("@MenuName", MenuName);
                SqlPara[4] = new SqlParameter("@MenuDescription", MenuDescription);
                SqlPara[5] = new SqlParameter("@DisplayOrderNumber", DisplayOrderNumber);
                SqlPara[6] = new SqlParameter("@NavigationURL", NavigationURL);
                SqlPara[7] = new SqlParameter("@ParentMenuID", ParentMenuID);
                SqlPara[8] = new SqlParameter("@Status", Status);
                SqlPara[9] = new SqlParameter("@AllowInMenu", AllowInMenu);
                SqlPara[10] = new SqlParameter("@AccessFor", AccessFor);
                SqlPara[11] = new SqlParameter("@MenuType", MenuType);
                SqlPara[12] = new SqlParameter("@AccessRole", AccessRole);
                DataAccess.Instance.DBInsertCommand("prcInsTreeMenu", SqlPara);
                if (SqlPara[2].Value.ToString() != "")
                {
                    InsError = SqlPara[2].Value.ToString();
                }
                else { InsError = null; }
                Error = Convert.ToString(SqlPara[1].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void TreeMenuMapping(DataTable dt)
        {
            try
            {
                SqlPara = new SqlParameter[5];
                SqlPara[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlPara[0].Direction = ParameterDirection.Output;
                SqlPara[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlPara[1].Direction = ParameterDirection.Output;
                SqlPara[2] = new SqlParameter("@RoleID", RoleID);
                SqlPara[3] = new SqlParameter("@dtMenu", dt);
                SqlPara[4] = new SqlParameter("@CompanyId", CompanyId);
                DataAccess.Instance.DBInsertCommand("prcDELInsertMenuMapping", SqlPara);
                Error = Convert.ToString(SqlPara[1].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTreeMenuInfo()
        {
            try
            {

                SqlPara = new SqlParameter[13];
                SqlPara[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlPara[0].Direction = ParameterDirection.Output;
                SqlPara[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlPara[1].Direction = ParameterDirection.Output;
                SqlPara[2] = new SqlParameter("@InsError", SqlDbType.VarChar, 200);
                SqlPara[2].Direction = ParameterDirection.Output;
                SqlPara[3] = new SqlParameter("@MenuName", MenuName);
                SqlPara[4] = new SqlParameter("@MenuDescription", MenuDescription);
                //SqlPara[5] = new SqlParameter("@DisplayOrderNumber", DisplayOrderNumber);
                SqlPara[5] = new SqlParameter("@NavigationURL", NavigationURL);
                SqlPara[6] = new SqlParameter("@ParentMenuID", ParentMenuID);
                SqlPara[7] = new SqlParameter("@Status", Status);
                SqlPara[8] = new SqlParameter("@AllowInMenu", AllowInMenu);
                SqlPara[9] = new SqlParameter("@AccessFor", AccessFor);
                SqlPara[10] = new SqlParameter("@MenuID", MenuID);
                SqlPara[11] = new SqlParameter("@MenuType", MenuType);
                SqlPara[12] = new SqlParameter("@AccessRole", AccessRole);
                DataAccess.Instance.DBInsertCommand("prcUpdTreeMenu", SqlPara);
                if (SqlPara[2].Value.ToString() != "")
                {
                    InsError = SqlPara[2].Value.ToString();
                }
                else { InsError = null; }
                Error = Convert.ToString(SqlPara[1].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DelTreeMenu()
        {
            try
            {
                SqlPara = new SqlParameter[1];
                SqlPara[0] = new SqlParameter("@MenuId", MenuID);
                DataAccess.Instance.DBInsertCommand("prcDelTreeMenu", SqlPara);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void DragNode()
        {
            try
            {
                SqlPara = new SqlParameter[5];
                SqlPara[0] = new SqlParameter("@MenuId", MenuID);
                SqlPara[1] = new SqlParameter("@DisplayOrderNumber", DisplayOrderNumber);
                SqlPara[2] = new SqlParameter("@NewParent", NewParent);
                SqlPara[3] = new SqlParameter("@OldParent", OldParent);
                SqlPara[4] = new SqlParameter("@InsError", SqlDbType.VarChar, 200);
                SqlPara[4].Direction = ParameterDirection.Output;
                DataAccess.Instance.DBInsertCommand("prcDragNode", SqlPara);
                if (SqlPara[4].Value.ToString() != "")
                {
                    InsError = SqlPara[4].Value.ToString();
                }
                else { InsError = null; }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void ChengePositionTreeMenu(string Condition)
        {
            try
            {
                SqlPara = new SqlParameter[4];
                SqlPara[0] = new SqlParameter("@MenuId", MenuID);
                SqlPara[1] = new SqlParameter("@ParentMenuID", ParentMenuID);
                SqlPara[2] = new SqlParameter("@Condition", Condition);
                SqlPara[3] = new SqlParameter("@InsError", SqlDbType.VarChar, 200);
                SqlPara[3].Direction = ParameterDirection.Output;
                DataAccess.Instance.DBInsertCommand("prcChengeOrderTreeMenu", SqlPara);
                if (SqlPara[3].Value.ToString() != "")
                {
                    InsError = SqlPara[3].Value.ToString();
                }
                else { InsError = null; }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion


        /* #CC02 Add Start */

        public DataSet GetRoleWiseModuleData()
        {
            try
            {
                SqlParameter[] SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 2000);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.Int);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@UserID", UserID);
                SqlParam[3] = new SqlParameter("@RoleID", RoleID);
                SqlParam[4] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
                SqlParam[4].Direction = ParameterDirection.Output;
                DataSet dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetRoleWiseMenuModules", CommandType.StoredProcedure, SqlParam);
                if (SqlParam[0].Value != DBNull.Value && SqlParam[0].Value.ToString() != "")
                {
                    Error = (SqlParam[0].Value).ToString();
                }
                intOutParam = Convert.ToInt32(SqlParam[1].Value);
                TotalRecords = Convert.ToInt32(SqlParam[4].Value);
                return dsResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetUserByMenuAndRoleData()
        {
            try
            {
                SqlParameter[] SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 2000);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.Int);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@UserID", UserID);
                SqlParam[3] = new SqlParameter("@RoleID", RoleID);                
                SqlParam[4] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
                SqlParam[4].Direction = ParameterDirection.Output;
                SqlParam[5] = new SqlParameter("@MenuID", MenuID);
                SqlParam[6]= new SqlParameter("@LoginName",LoginName);
                DataSet dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetUserByMenuAndRole", CommandType.StoredProcedure, SqlParam);
                if (SqlParam[0].Value != DBNull.Value && SqlParam[0].Value.ToString() != "")
                {
                    Error = (SqlParam[0].Value).ToString();
                }
                intOutParam = Convert.ToInt32(SqlParam[1].Value);
                TotalRecords = Convert.ToInt32(SqlParam[4].Value);
                return dsResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int32 ExcludeMenu()
        {
            try
            {


                SqlParameter[] SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 2000);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@UserModuleData", SqlDbType.Structured);
                SqlParam[2].Value = dtExclude;
                SqlParam[3] = new SqlParameter("@UpdateAll", UpdateAll);
                SqlParam[4] = new SqlParameter("@UserID", UserID);
                SqlParam[5] = new SqlParameter("@UserName",LoginName );
                IntResultCount = Convert.ToInt16(DataAccess.Instance.getSingleValues("prcSaveUserModuleExcludeData", SqlParam));
                if (SqlParam[0].Value != DBNull.Value && SqlParam[0].Value.ToString() != "")
                {
                    Error = (SqlParam[0].Value).ToString();
                }
                intOutParam = Convert.ToInt32(SqlParam[1].Value);

                return IntResultCount;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


/* #CC02 Add end */


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

        ~MenuData()
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

