#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*
     * ================================================================================================
     *
     * COPYRIGHT (c) 2008 Zed Axis Technologies (P) Ltd.
     * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART,
     * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE,
     * WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
     *
     * ================================================================================================
     * Created By : Sumit Maurya
     * Created On : 01-March-2016
     * Modified BY :
     * Description : 
     * ================================================================================================
     * Change Log :
     * DD-MMM-YYY, Name, #CCXX, Description.
     * ================================================================================================
     * Reviewed By :
     * ================================================================================================
       */

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Data.SqlClient;
using System.Xml;

namespace DataAccess
{
    public class Broadcast : IDisposable
    {
        #region Properties Defined
        public DataTable dtResult;
        public DataSet dsResult;
        public DataTable dtID
        {
            get;
            set;
        }
        public int UserID
        {
            get;
            set;
        }
        public int BroadcastTypeId
        {
            get;
            set;
        }
        public string BroadcastText
        {
            get;
            set;
        }

        #endregion Properties Defined

        #region Public methods
        public DataSet GetStateandBroadcastTypeDetails()
        {
            try
            {
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetStateandBroadcastTypeDetails", CommandType.StoredProcedure);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SaveBroadcastMessage()
        {
            try
            {
                int result;
                SqlParameter[] SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@BroadcastTypeId", BroadcastTypeId);
                SqlParam[1] = new SqlParameter("@BroadcastText", BroadcastText);
                SqlParam[2] = new SqlParameter("@BroadcastTo", dtID);
                SqlParam[3] = new SqlParameter("@UserId", UserID);
                SqlParam[4] = new SqlParameter("@Out_Param", SqlDbType.Int);
                SqlParam[4].Direction = ParameterDirection.Output;
                DataAccess.Instance.DBInsertCommand("prcSaveBroadcastText", SqlParam);
                result = Convert.ToInt16(SqlParam[4].Value);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion Public methods

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

        ~Broadcast()
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
