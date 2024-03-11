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
#region ChangeLog
/* Change Log
 * -------------------------------------------------------------------------------------
 * DD-MMM-YYYY, Name, #CCXX, Description
 *  
 */
#endregion ChangeLog
namespace DataAccess
{
    public class marketingmanagementdata : IDisposable
    {
        private Int64 _MarketingDocumentID;
        private int _SubCategoryId, intUserID;
        private Int32 intSearchType;
        private string _Description, _DocumentPath, _Heading, strLevelIds, strLocationIds, strSalesChannelIds, strSalesChannelTypeIds, strError, _categoryname, _subcategoryname;

        private DateTime dtPublishDate, dtExpiryDate;
        private Int32 intAccessType;
        private bool blnStatus;
        DataTable dtResult;
        SqlParameter[] SqlParam;
        Int32 IntResultCount = 0;
        DataSet dsResult;


        public Int64 MarketingDocumentID
        {
            get { return _MarketingDocumentID; }
            set { _MarketingDocumentID = value; }
        }
        public int UserID
        {
            get { return intUserID; }
            set { intUserID = value; }
        }
        public int SubCategoryId
        {
            get { return _SubCategoryId; }
            set { _SubCategoryId = value; }
        }
        public Int32 SearchType
        {
            get { return intSearchType; }
            set { intSearchType = value; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public string DocumentPath
        {
            get { return _DocumentPath; }
            set { _DocumentPath = value; }
        }
        public string LevelIds
        {
            get { return strLevelIds; }
            set { strLevelIds = value; }
        }
        public string SalesChannelIds
        {
            get { return strSalesChannelIds; }
            set { strSalesChannelIds = value; }
        }
        public string Error
        {
            get { return strError; }
            set { strError = value; }
        }
        public string LocationIds
        {
            get { return strLocationIds; }
            set { strLocationIds = value; }
        }
        public string SalesChannelTypeIds
        {
            get { return strSalesChannelTypeIds; }
            set { strSalesChannelTypeIds = value; }
        }
        public string Heading
        {
            get { return _Heading; }
            set { _Heading = value; }
        }
        public DateTime PublishDate
        {
            get { return dtPublishDate; }
            set { dtPublishDate = value; }
        }
        public DateTime ExpiryDate
        {
            get { return dtExpiryDate; }
            set { dtExpiryDate = value; }
        }
        public Int32 AccessType
        {
            get { return intAccessType; }
            set { intAccessType = value; }
        }
        public bool Status
        {
            get { return blnStatus; }
            set { blnStatus = value; }
        }

        public string categoryname
        {
            get { return _categoryname; }
            set { _categoryname = value; }
        }
        public string subcategoryname
        {
            get { return _subcategoryname; }
            set { _subcategoryname = value; }
        }
        public int IsScheme
        {
            get;
            set;
        }

        public DataTable DTBrandID
        {
            get;
            set;
        }
        public Int16? BrandAccessType
        {
            get;
            set;
        }


        public DataSet GetAllHierarchyLevelwithLocation()
        {
            try
            {
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetAllHierarchyLevelWithLocation", CommandType.StoredProcedure);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetHierarchyLevelTreeByMARKETINGDOCUMENTId()
        {
            try
            {
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@MarketingDocumentID", MarketingDocumentID);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetHierarchyLevelTreeByMARKETINGDOCUMENTId", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetSalesChannelTreeByBulletinId()
        {
            try
            {
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@BulletinId", MarketingDocumentID);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetSalesChannelTreeByBulletinId", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllSalesChannelwithLocation()
        {
            try
            {
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@isscheme", IsScheme);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetAllSalesChannelWithLocation", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Int32 InsMarketingDocument()
        {
            try
            {
                SqlParam = new SqlParameter[17]; /* #CC01 Length increased from 13 to 15 */
                SqlParam[0] = new SqlParameter("@MarketingDocumentID", MarketingDocumentID);
                SqlParam[0].Direction = ParameterDirection.InputOutput;
                SqlParam[1] = new SqlParameter("@Heading", Heading);
                SqlParam[2] = new SqlParameter("@CategoryID", SubCategoryId);
                SqlParam[3] = new SqlParameter("@Description", Description);
                SqlParam[4] = new SqlParameter("@PublishDate", dtPublishDate);
                SqlParam[5] = new SqlParameter("@ExpiryDate", dtExpiryDate);
                SqlParam[6] = new SqlParameter("@AccessType", intAccessType);
                SqlParam[7] = new SqlParameter("@Status", blnStatus);
                SqlParam[8] = new SqlParameter("@LevelIds", strLevelIds);
                SqlParam[9] = new SqlParameter("@LocationIds", strLocationIds);
                SqlParam[10] = new SqlParameter("@SalesChannelIds", strSalesChannelIds);
                SqlParam[11] = new SqlParameter("@SalesChannelTypeIds", strSalesChannelTypeIds);
                SqlParam[12] = new SqlParameter("@OutError", SqlDbType.VarChar, 200);
                SqlParam[12].Direction = ParameterDirection.Output;

                SqlParam[13] = new SqlParameter("@BrandAccessType", BrandAccessType);
                SqlParam[14] = new SqlParameter("@TVPBrandID", SqlDbType.Structured);
                SqlParam[14].Value = DTBrandID;
                SqlParam[15] = new SqlParameter("@DocumentPath", DocumentPath);
                SqlParam[16] = new SqlParameter("@CreatedBy", UserID);

                DataAccess.Instance.DBInsertCommand("PrcInsMarketingDocument", SqlParam);
                IntResultCount = Convert.ToInt32(SqlParam[0].Value);
                Error = Convert.ToString(SqlParam[12].Value);
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAllParentCategory()
        {
            try
            {
               return DataAccess.Instance.GetTableFromDatabase("prcGetAllParentCategory", CommandType.StoredProcedure);
                 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetAllCategoryMasterByParentId(int CategoryID)
        {
            try
            {
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@CategoryID", CategoryID);
                return DataAccess.Instance.GetTableFromDatabase("prcGetAllCategoryMaster", CommandType.StoredProcedure, SqlParam);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertCatSubCategory()
        {

            SqlParam = new SqlParameter[5];
             
            SqlParam[0] = new SqlParameter("@subcategoryid", SqlDbType.Int, 5);
            SqlParam[0].Direction = ParameterDirection.Output;

            SqlParam[1] = new SqlParameter("@categoryname", categoryname);
            SqlParam[2] = new SqlParameter("@subcategoryname", subcategoryname);

            SqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200); ;
            SqlParam[3].Direction = ParameterDirection.Output;


            SqlParam[4] = new SqlParameter("@CreatedBy", UserID);
            int r = DataAccess.Instance.DBInsertCommand("PrcInsertCatSubCategory", SqlParam);
            
            Error = Convert.ToString(SqlParam[3].Value);
            if (Error.Trim().Length == 0)
            {
                SubCategoryId = Convert.ToInt32(SqlParam[0].Value);

            }

        }

        //public Int32 UpdateStatusBulletinInfo()
        //{
        //    try
        //    {

        //        SqlParam = new SqlParameter[1];
        //        SqlParam[0] = new SqlParameter("@BulletinId", intBulletinId);
        //        IntResultCount = DataAccess.Instance.DBInsertCommand("PrcUpdStatusBulletin", SqlParam);
        //        return IntResultCount;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}


        public DataTable GetMmanagementInfo()
        {
            try
            {
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@SubCategoryId", SubCategoryId);
                SqlParam[1] = new SqlParameter("@MarketingDocumentID", MarketingDocumentID);
                SqlParam[2] = new SqlParameter("@Status", blnStatus);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetMmanagementInfoByParameters", CommandType.StoredProcedure, SqlParam);

                return dtResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public DataTable GetBulletinInfoByUserId()
        //{
        //    try
        //    {
        //        SqlParam = new SqlParameter[3];
        //        SqlParam[0] = new SqlParameter("@UserID", UserID);
        //        SqlParam[1] = new SqlParameter("@SubCategoryId", SubCategoryId);
        //        SqlParam[2] = new SqlParameter("@SearchType", SearchType);
        //        dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetBulletinbyUserID", CommandType.StoredProcedure, SqlParam);

        //        return dtResult;
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}



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

        ~marketingmanagementdata()
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
