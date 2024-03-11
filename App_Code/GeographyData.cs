
/*
 * 30-Mar-2016, Sumit Maurya, #CC01, New method created to fetch active Tehsil details and active states according, new property created and parameter supplied in function GetAllCityByParameters() to fetch City By TehsilId
 * 26-May-2016, Sumit Maurya, #CC02, Supplied parameter changed due to the changed flow of Tehsil.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class GeographyData : IDisposable
    {
        public int CompanyId { get; set; }
        public int UserID { get; set; }
        private Int16 intStateId, intDistrictId, intCityId;
        private EnumData.eSearchConditions eSearchConditions;
        public EnumData.eSearchConditions SearchCondition
        {
            get { return eSearchConditions; }
            set { eSearchConditions = value; }
        }
        public Int16 StateId
        {
            get { return intStateId; }
            set { intStateId = value; }
        }
        public Int16 DistrictID
        {
            get { return intDistrictId; }
            set { intDistrictId = value; }
        }
        public Int16 CityId
        {
            get { return intCityId; }
            set { intCityId = value; }
        }
        DataTable dtResult;
        SqlParameter[] SqlParam;
        Int32 IntResultCount = 0;
        DataSet dsResult;
        /*  #CC01 Add Start */
        private Int16 intTehsilId = 0;
        public Int16 TehsilId
        {
            get { return intTehsilId; }
            set { intTehsilId = value; }
        }

        public int countryid
        {
            get;
            set;
        }
        /*  #CC01 Add End */
        #region Get State List
        public DataTable GetAllStateByParameters()
        {
            try
            {
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@SearchConditions", eSearchConditions);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetAllStateByParameters", CommandType.StoredProcedure, SqlParam);

                return dtResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Get Area List
        public DataTable GetAllAreaByParameters()
        {
            try
            {
                SqlParam = new SqlParameter[3]; /* #CC02 length increased from 2 to 3 */
                SqlParam[0] = new SqlParameter("@SearchConditions", eSearchConditions);
                SqlParam[1] = new SqlParameter("@CityID", intCityId);
                SqlParam[2] = new SqlParameter("@TehsilID", TehsilId); /* #CC02 Added */
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetAllAreaByParameters", CommandType.StoredProcedure, SqlParam);

                return dtResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Get City List
        public DataTable GetAllCityByParameters()
        {
            try
            {
                SqlParam = new SqlParameter[3]; /* #CC01 length Increased from 2 to 3 */
                SqlParam[0] = new SqlParameter("@SearchConditions", eSearchConditions);
                SqlParam[1] = new SqlParameter("@StateId", intStateId);
                /* SqlParam[2] = new SqlParameter("@TehsilID", TehsilId); #CC02 Commented */
                /* #CC01 Added */

                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetAllCityByParameters", CommandType.StoredProcedure, SqlParam);

                return dtResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get State List
        public DataTable GetAllTownByParameters()
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@SearchConditions", eSearchConditions);
                SqlParam[1] = new SqlParameter("@CityId", intCityId);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetAllTownByParameters", CommandType.StoredProcedure, SqlParam);

                return dtResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /* #CC01 Add Start */
        public DataTable GetAllActiveTehsil()
        {
            try
            {
                SqlParam = new SqlParameter[2];  /* #CC02 Length increased from 1 to 2 */
                SqlParam[0] = new SqlParameter("@StateID", StateId);
                SqlParam[1] = new SqlParameter("@CityID", CityId); /* #CC02 Added */
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetActiveTehsil", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetAllActiveStates()
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@countryid", countryid);
                SqlParam[1] = new SqlParameter("@CompanyId", CompanyId);
                SqlParam[2] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@OutError", SqlDbType.VarChar,500);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@authKey", "");
                SqlParam[5] = new SqlParameter("@UserId", UserID);


                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetAllStateDetails", CommandType.StoredProcedure, SqlParam);
                return dtResult;
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

        ~GeographyData()
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
