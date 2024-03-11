using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DataAccess
{
    public class Advertisement :IDisposable
    {
        SqlParameter[] SqlParam;
        DataTable dtresult;
        # region Property
        private string _Caption;
        public string Caption
        {
            get { return _Caption; }
            set { _Caption = value; }
        }
        private string _ThumbainlPath;
        public string ThumbainlPath
        {
            get { return _ThumbainlPath; }
            set { _ThumbainlPath = value; }
        }
        private string _VideoLink;
        public string VideoLink
        {
            get { return _VideoLink; }
            set { _VideoLink = value; }
        }
        private string _ErrorMsg;
        public string ErrorMsg
        {
            get { return _ErrorMsg; }
            set { _ErrorMsg = value; }
        }
        private Int16 _Status;
        public Int16 Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        private Int32 _ThumbainlType;
        public Int32 ThumbainlType
        {
            get { return _ThumbainlType; }
            set { _ThumbainlType = value; }
        }
        private int? _AdvertisementID;
        public int? AdvertisementID
        {
            get { return _AdvertisementID; }
            set { _AdvertisementID = value; }
        }
        public Int16? adStatus
        {
            get;
            set;
        }
        public string srcCaption
        {
            get;
            set;
        }
        # endregion
        # region Insert-Update Function
        public void InsUpdAdvertisement(int Condition)
        {
            try
            {
                SqlParam = new SqlParameter[8];
                SqlParam[0] = new SqlParameter("@Caption", Caption);
                SqlParam[1] = new SqlParameter("@ThumbainlPath", ThumbainlPath);
                SqlParam[2] = new SqlParameter("@ThumbNailType", ThumbainlType);
                SqlParam[3] = new SqlParameter("@VideoLink", VideoLink);
                SqlParam[4] = new SqlParameter("@Status", Status);
                SqlParam[5] = new SqlParameter("@Condition", Condition);
                SqlParam[6] = new SqlParameter("@InsUpdError", SqlDbType.NVarChar, 200);
                SqlParam[6].Direction = ParameterDirection.Output;
                SqlParam[7] = new SqlParameter("@AdvertisementID", AdvertisementID);
                DataAccess.Instance.DBInsertCommand("prcInsUpdAdvertisement",SqlParam);
                ErrorMsg = SqlParam[6].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        # endregion
        # region Get Function
        public DataTable GetAdvertisement()
        {
            dtresult = new DataTable();
            SqlParam = new SqlParameter[4];
            SqlParam[0] = new SqlParameter("@InsUpdError", SqlDbType.NVarChar, 200);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@AdvertisementID", AdvertisementID);
            SqlParam[2] = new SqlParameter("@Status", adStatus);
            SqlParam[3] = new SqlParameter("@srcCaption", srcCaption);
            dtresult = DataAccess.Instance.GetTableFromDatabase("prcGetAdvertisement", CommandType.StoredProcedure, SqlParam);
            ErrorMsg = SqlParam[0].Value.ToString();
            return dtresult;
        }
        # endregion
        #region Dispose
        private bool IsDisposed = false;

        //Call Dispose to free resources explicitly
        public void Dispose()
        {
            //Pass true in dispose method to clean managed resources too and say GC to skip finalize 
            // in next line.
            Dispose(true);
            //If dispose is called already then say GC to skip finalize on this instance.
            GC.SuppressFinalize(this);
        }

        ~Advertisement()
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
