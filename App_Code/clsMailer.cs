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
    public class clsMailer : IDisposable
    {
        SqlParameter[] SqlParam;
        DataTable dtMailerInfo;

        private string _Emailkeyword;
        public string Emailkeyword
        {
            get { return _Emailkeyword; }
            set { _Emailkeyword = value; }
        }
        private string _Emaildesc;
        public string Emaildesc
        {
            get { return _Emaildesc; }
            set { _Emaildesc = value; }
        }
        private string _Emailfrom;
        public string Emailfrom
        {
            get { return _Emailfrom; }
            set { _Emailfrom = value; }
        }
        private string _Subjectline;
        public string Subjectline
        {
            get { return _Subjectline; }
            set { _Subjectline = value; }
        }
        private string _Bodycontent;
        public string Bodycontent
        {
            get { return _Bodycontent; }
            set { _Bodycontent = value; }
        }
        private string _ErrorDetailXML;
        public string ErrorDetailXML
        {
            get { return _ErrorDetailXML; }
            set { _ErrorDetailXML = value; }
        }
        private string _Error;
        public string Error
        {
            get { return _Error; }
            set { _Error = value; }
        }
        private string _Emailkeyinfo;
        public string Emailkeyinfo
        {
            get { return _Emailkeyinfo; }
            set { _Emailkeyinfo = value; }
        }
        private int _Status;
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        private int _EmailExpiryHrs;
        public int EmailExpiryHrs
        {
            get { return _EmailExpiryHrs; }
            set { _EmailExpiryHrs = value; }
        }
        public void InsertManageEmailMaster()
        {
            SqlParam = new SqlParameter[11];
            SqlParam[0] = new SqlParameter("@EmailKeyword", Emailkeyword);
            SqlParam[1] = new SqlParameter("@EmailDescription", Emaildesc);
            SqlParam[2] = new SqlParameter("@EmailFrom", Emailfrom);
            SqlParam[3] = new SqlParameter("@SubjectLine", Subjectline);
            SqlParam[4] = new SqlParameter("@Body", Bodycontent);
            SqlParam[5] = new SqlParameter("@Status", Status);
            SqlParam[6] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[6].Direction = ParameterDirection.Output;
            SqlParam[7] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[7].Direction = ParameterDirection.Output;
            SqlParam[8] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
            SqlParam[8].Direction = ParameterDirection.Output;
            SqlParam[9] = new SqlParameter("@EmailKeyInfo", SqlDbType.NVarChar, 100);
            SqlParam[9].Direction = ParameterDirection.Output;
            SqlParam[10] = new SqlParameter("@EmailExpiryHrs", EmailExpiryHrs);
            DataAccess.Instance.DBInsertCommand("prcInsertManageEmailMaster", SqlParam);
            if (SqlParam[8].Value.ToString() != "")
            {
                ErrorDetailXML = SqlParam[8].Value.ToString();
            }
            else { ErrorDetailXML = null; }
            if (SqlParam[9].Value.ToString() != "")
            {
                Emailkeyinfo = SqlParam[9].Value.ToString();
            }
            else { Emailkeyinfo = null; }
            Error = Convert.ToString(SqlParam[7].Value);
        }
        public DataTable getViewEmailMaster(int condition)
        {
            //dtMailerInfo = new DataTable();
            SqlParam = new SqlParameter[6];
            SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@condition", condition);
            SqlParam[4] = new SqlParameter("@emailDese", Emaildesc);
            SqlParam[5] = new SqlParameter("@emailKey", Emailkeyword);
            dtMailerInfo = DataAccess.Instance.GetTableFromDatabase("prcGetViewEmailMaster", CommandType.StoredProcedure, SqlParam);
            if (SqlParam[2].Value.ToString() != "")
            {
                ErrorDetailXML = SqlParam[2].Value.ToString();
            }
            else { ErrorDetailXML = null; }
            Error = Convert.ToString(SqlParam[1].Value);
            return dtMailerInfo;
        }
        public void UpdateManageEmailMaster(int condition)
        {
            SqlParam = new SqlParameter[11];
            SqlParam[0] = new SqlParameter("@EmailKeyword", Emailkeyword);
            SqlParam[1] = new SqlParameter("@EmailDescription", Emaildesc);
            SqlParam[2] = new SqlParameter("@EmailFrom", Emailfrom);
            SqlParam[3] = new SqlParameter("@SubjectLine", Subjectline);
            SqlParam[4] = new SqlParameter("@Body", Bodycontent);
            SqlParam[5] = new SqlParameter("@Status", Status);
            SqlParam[6] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[6].Direction = ParameterDirection.Output;
            SqlParam[7] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[7].Direction = ParameterDirection.Output;
            SqlParam[8] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
            SqlParam[8].Direction = ParameterDirection.Output;
            SqlParam[9] = new SqlParameter("@condition", condition);
            SqlParam[10] = new SqlParameter("@EmailExpiryHrs", EmailExpiryHrs);
            DataAccess.Instance.DBInsertCommand("prcUpdateManageEmailMaster", SqlParam);
            if (SqlParam[8].Value.ToString() != "")
            {
                ErrorDetailXML = SqlParam[8].Value.ToString();
            }
            else { ErrorDetailXML = null; }
            Error = Convert.ToString(SqlParam[7].Value);
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

        ~clsMailer()
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
