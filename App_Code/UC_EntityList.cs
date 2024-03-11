#region Change Log
/*
Change Log:
----------
 * 20-Mar-2013, Prashant Chitransh, #CC01: Changes to implement Multiple Parent IDs.
 * 17-Mar-2014, Karam Chand Sharma, #CC02: Add parameter name with ForceTypeMatching 0 - Default Behaviour, 1 = Force type matching..check notes.
 * 21-May-2014, Shilpi Sharma, #CC03: Add parameter name with ForceTypeMatching 0 - Default Behaviour, 1 = Force type matching..check notes.
 * 17-Oct-2014, Shilpi Sharma, #CC03: Created overload function of GetEntityAccessWise because when existing mapping change and new mapping create old mapping inactive so to process old records new parameter created and need to pass value.
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Microsoft.ApplicationBlocks.Data;

namespace BussinessLogic/*ZedAxis.ZedEBS*/
{
    public class UC_EntityList : UserControlsProperties
    {
        #region Private Variables
        private DataTable _dtEntityList;        // #CC01: added.
        private Int16 _intSelectionMode = 1;    // #CC01: added.
        private Int16 _ForceTypeMatching = 0;/*#CC02*/
        private Int32 _intEntityID = 0;/*#CC02*/
        #endregion
        public String strConStr = System.Configuration.ConfigurationManager.ConnectionStrings["AppConString"].ConnectionString;
        #region Public Properties
        /*#CC02 ADDED START*/
        public Int16 ForceTypeMatching
        {
            get { return _ForceTypeMatching; }
            set { _ForceTypeMatching = value; }
        }
        public Int32 EntityID
        {
            get { return _intEntityID; }
            set { _intEntityID = value; }
        }
        
        private string _strError;
        public string Error
        {
            get
            {
                return _strError;
            }
            set
            {
                _strError = value;
            }
        }
        /*#CC02 ADDED END*/
        // #CC01: added.
        public Int16 SelectionMode
        {
            get { return _intSelectionMode; }
            set { _intSelectionMode = value; }
        }

        // #CC01: added.
        public DataTable EntityList
        {
            get { return _dtEntityList; }
            set { _dtEntityList = value; }
        }
        #endregion


        private Int16 _intActiveMode;
        public Int16 ActiveMode
        {
            get
            {
                return _intActiveMode;
            }
            set
            {
                _intActiveMode = value;
            }
        }
        public DataTable GetEntityAccessWise(bool isParent, int SelectedEntityID, byte Type, string Keyword, string Buisnesseventkeyword, int entitytypedescription)
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[11];
            objSqlParam[0] = new SqlParameter("@IsParent", isParent);
            objSqlParam[1] = new SqlParameter("@LogedInEntityID", EntityID);
            objSqlParam[2] = new SqlParameter("@SelectedEntityID", SelectedEntityID);
            objSqlParam[3] = new SqlParameter("@Type", Type);
            objSqlParam[4] = new SqlParameter("@Keyword", Keyword);
            objSqlParam[5] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[5].Direction = ParameterDirection.Output;
            objSqlParam[6] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 1);
            objSqlParam[6].Direction = ParameterDirection.Output;
            objSqlParam[7] = new SqlParameter("@BusinessEventKeyword", Buisnesseventkeyword);
            objSqlParam[8] = new SqlParameter("@entitytypebit", entitytypedescription);
            objSqlParam[9] = new SqlParameter("@ForceTypeMatching", ForceTypeMatching);/*#CC02 ADDED */
            
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcEntityAccessWise_Select", objSqlParam);

            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];

            Error = Convert.ToString(objSqlParam[5].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }
            return dtResult;
        }
        /*#CC03:added (start)*/
        public DataTable GetEntityAccessWise(bool isParent, int SelectedEntityID, byte Type, string Keyword, string Buisnesseventkeyword, int entitytypedescription, int ActiveMode)
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[11];
            objSqlParam[0] = new SqlParameter("@IsParent", isParent);
            objSqlParam[1] = new SqlParameter("@LogedInEntityID", EntityID);
            objSqlParam[2] = new SqlParameter("@SelectedEntityID", SelectedEntityID);
            objSqlParam[3] = new SqlParameter("@Type", Type);
            objSqlParam[4] = new SqlParameter("@Keyword", Keyword);
            objSqlParam[5] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[5].Direction = ParameterDirection.Output;
            objSqlParam[6] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 1);
            objSqlParam[6].Direction = ParameterDirection.Output;
            objSqlParam[7] = new SqlParameter("@BusinessEventKeyword", Buisnesseventkeyword);
            objSqlParam[8] = new SqlParameter("@entitytypebit", entitytypedescription);
            objSqlParam[9] = new SqlParameter("@ForceTypeMatching", ForceTypeMatching);/*#CC02 ADDED */
            objSqlParam[10] = new SqlParameter("@ActiveMode", ActiveMode);/*#CC02 ADDED */
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcEntityAccessWise_Select", objSqlParam);

            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];

            Error = Convert.ToString(objSqlParam[5].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }
            return dtResult;
        }

        /*#CC03:added (end)*/
        public DataTable GetEntityAccessWise_ver2(bool isParent, int SelectedEntityID, byte Type, string Keyword, string Buisnesseventkeyword, int entitytypedescription)
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[14];
            objSqlParam[0] = new SqlParameter("@IsParent", isParent);
            objSqlParam[1] = new SqlParameter("@LogedInEntityID", EntityID);
            objSqlParam[2] = new SqlParameter("@SelectedEntityID", SelectedEntityID);
            objSqlParam[3] = new SqlParameter("@Type", Type);
            objSqlParam[4] = new SqlParameter("@Keyword", Keyword);
            objSqlParam[5] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[5].Direction = ParameterDirection.Output;
            objSqlParam[6] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 1);
            objSqlParam[6].Direction = ParameterDirection.Output;
            objSqlParam[7] = new SqlParameter("@BusinessEventKeyword", Buisnesseventkeyword);
            objSqlParam[8] = new SqlParameter("@entitytypebit", entitytypedescription);
            objSqlParam[9] = new SqlParameter("@SelectionMode", SelectionMode);
            objSqlParam[10] = new SqlParameter("@SelectedEntityList", EntityList);
            objSqlParam[11] = new SqlParameter("@ForceTypeMatching", ForceTypeMatching);/*#CC03 ADDED */
            objSqlParam[12] = new SqlParameter("@UserId", PageBase.UserId);
            objSqlParam[13] = new SqlParameter("@BaseEntityTypeId",PageBase.BaseEntityTypeID);

            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcEntityAccessWise_Select_ver2", objSqlParam);

            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];

            Error = Convert.ToString(objSqlParam[5].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }
            return dtResult;
        }
    }
}
