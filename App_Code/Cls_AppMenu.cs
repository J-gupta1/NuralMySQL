#region Copyright(c) 2017 Zed-Axis Technologies All rights are reserved
/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2017 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Created By : Gaurav Tyagi
* Created On: 10-May-2019
 * Description:  To manage the questions and html link for App this interface required to build.
* ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 
 ====================================================================================================
*/
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.IO;
/// <summary>
/// Summary description for Cls
/// </summary>
/// 
namespace DataAccess
{
    public class Cls_AppMenu : IDisposable
    {
        Int64 pvt_AppMenuId, pvt_AppMenuHelpId;
        string pvt_QuestionText, pvt_HelpLink, pvt_Out_Error;
        Int16 pvt_DisplayOrder, pvt_Satusupdate;
        int pvt_PageIndex, pvt_PageSize,pvt_TotalRecords;

        public int PageIndex
        {
            get { return pvt_PageIndex; }
            set { pvt_PageIndex = value; }
        }
        public int PageSize
        {
            get { return pvt_PageSize; }
            set { pvt_PageSize = value; }
        }
        public int TotalRecords
        {
            get { return pvt_TotalRecords; }
            set { pvt_TotalRecords = value; }
        }
        public Int64 AppMenuId
        {
            get { return pvt_AppMenuId; }
            set { pvt_AppMenuId = value; }
        }
        public Int64 AppMenuHelpId
        {
            get { return pvt_AppMenuHelpId; }
            set { pvt_AppMenuHelpId = value; }
        }
        public Int16 DisplayOrder
        {
            get { return pvt_DisplayOrder; }
            set { pvt_DisplayOrder = value; }
        }
        public Int16  Satusupdate
        {
            get { return pvt_Satusupdate; }
            set { pvt_Satusupdate = value; }
        }
        
        public string QuestionText
        {
            get { return pvt_QuestionText; }
            set { pvt_QuestionText = value; }
        }
        public string HelpLink
        {
            get { return pvt_HelpLink; }
            set { pvt_HelpLink = value; }
        }
        public string Out_Error
        {
            get { return pvt_Out_Error; }
            set { pvt_Out_Error = value; }
        }

        SqlParameter[] SqlParam;
        DataTable dtResult;
        DataSet ds;
        public DataTable GetAppMenu()
        {

            try
            {
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetAppMenu", CommandType.StoredProcedure);
                return dtResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getAppmenuhelpdetails()
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.Int);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 2000);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@PageIndex", PageIndex);
                SqlParam[3] = new SqlParameter("@PageSize", PageSize);
                SqlParam[4] = new SqlParameter("@Totalrecords", SqlDbType.BigInt ,12);
                SqlParam[4].Direction = ParameterDirection.Output;
                SqlParam[5] = new SqlParameter("@AppMenuHelpId", AppMenuHelpId);
                 ds= DataAccess.Instance.GetDataSetFromDatabase("prcgetAppmenuhelpdetails", CommandType.StoredProcedure, SqlParam);
                Out_Error = SqlParam[1].Value.ToString();
                TotalRecords = Convert.ToInt32(SqlParam[4].Value.ToString());
                return ds;
            }

            catch (Exception ex)
            {
                throw ex;
            }

             
        }

        public void InsertAppMenuHelp(ref string ERROR)
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@HelpLink", HelpLink);
                SqlParam[1] = new SqlParameter("@AppMenuId", AppMenuId);
                SqlParam[2] = new SqlParameter("@QuestionText", QuestionText);
                SqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200); ;
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@DisplayOrder", DisplayOrder);
                int r = DataAccess.Instance.DBInsertCommand("prcInsertAppmenuhelp", SqlParam);
                ERROR = Convert.ToString(SqlParam[3].Value);
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        public void updateAppMenuHelp(ref string ERROR)
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@satusupdate", Satusupdate);
                SqlParam[1] = new SqlParameter("@AppMenuHelpId", AppMenuHelpId);
                SqlParam[2] = new SqlParameter("@QuestionText", QuestionText);
                SqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200); ;
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@DisplayOrder", DisplayOrder);
                SqlParam[5] = new SqlParameter("@HelpLink", HelpLink);
                int r = DataAccess.Instance.DBInsertCommand("prcupdatestatus", SqlParam);
                ERROR = Convert.ToString(SqlParam[3].Value);
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

        ~Cls_AppMenu()
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