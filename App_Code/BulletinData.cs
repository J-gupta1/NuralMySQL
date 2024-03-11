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
 * 10-Aug-2016, Sumit Maurya, #CC01, New check properties added and passes in functions GetAllSalesChannelwithLocation() and InsertUpdateBulletininfo() . 
 */
#endregion ChangeLog
namespace DataAccess
{
    public class BulletinData : IDisposable
    {
        private int intBulletinId, intSubCategoryId, intUserID;
        private Int32 intSearchType;
        private string strDescription, strBulletinSubject, strLevelIds, strLocationIds, strSalesChannelIds, strSalesChannelTypeIds, strError;

        private DateTime dtPublishDate, dtExpiryDate;
        private Int32 intAccessType;
        private bool blnStatus;
        DataTable dtResult;
        SqlParameter[] SqlParam;
        Int32 IntResultCount = 0;
        DataSet dsResult;


        public int BulletinId
        {
            get { return intBulletinId; }
            set { intBulletinId = value; }
        }
        public int UserID
        {
            get { return intUserID; }
            set { intUserID = value; }
        }
        public int SubCategoryId
        {
            get { return intSubCategoryId; }
            set { intSubCategoryId = value; }
        }
        public Int32 SearchType
        {
            get { return intSearchType; }
            set { intSearchType = value; }
        }
        public string Description
        {
            get { return strDescription; }
            set { strDescription = value; }
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
        public string BulletinSubject
        {
            get { return strBulletinSubject; }
            set { strBulletinSubject = value; }
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


        public int IsScheme
        {
            get;
            set;
        }
        /* #CC01 Add Start */
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

        /* #CC01 Add End */
        public Int32 CompanyId { get; set; }
        public DataSet GetAllHierarchyLevelwithLocation()
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@UserId", UserID);
                SqlParam[1] = new SqlParameter("@CompanyId", CompanyId);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetAllHierarchyLevelWithLocation", CommandType.StoredProcedure,SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetHierarchyLevelTreeByBulletinId()
        {
            try
            {
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@BulletinId", intBulletinId);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetHierarchyLevelTreeByBulletinId", CommandType.StoredProcedure, SqlParam);
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
                SqlParam[0] = new SqlParameter("@BulletinId", intBulletinId);
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
                SqlParam = new SqlParameter[3]; 
                SqlParam[0] = new SqlParameter("@isscheme", IsScheme);
                SqlParam[1] = new SqlParameter("@UserID", UserID);
                SqlParam[2] = new SqlParameter("@CompanyID", CompanyId);   
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetAllSalesChannelWithLocation", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Int32 InsertUpdateBulletininfo()
        {
            try
            {
                SqlParam = new SqlParameter[17]; /* #CC01 Length increased from 13 to 15 */
                SqlParam[0] = new SqlParameter("@BulletinId", intBulletinId);
                SqlParam[0].Direction = ParameterDirection.InputOutput;
                SqlParam[1] = new SqlParameter("@BulletinSubject", strBulletinSubject);
                SqlParam[2] = new SqlParameter("@SubCategoryId", intSubCategoryId);
                SqlParam[3] = new SqlParameter("@Description", strDescription);
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
                /* #CC01 Add Start */
                SqlParam[13] = new SqlParameter("@BrandAccessType", BrandAccessType);
                SqlParam[14] = new SqlParameter("@TVPBrandID", SqlDbType.Structured);
                SqlParam[14].Value = DTBrandID;
                /* #CC01 Add End */
                SqlParam[15] = new SqlParameter("@UserID", UserID);
                SqlParam[16] = new SqlParameter("@CompanyId", CompanyId);
                DataAccess.Instance.DBInsertCommand("PrcInsUpdBulletininfo", SqlParam);
                IntResultCount = Convert.ToInt32(SqlParam[0].Value);
                Error = Convert.ToString(SqlParam[12].Value);
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Int32 UpdateStatusBulletinInfo()
        {
            try
            {

                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@BulletinId", intBulletinId);
                IntResultCount = DataAccess.Instance.DBInsertCommand("PrcUpdStatusBulletin", SqlParam);
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public DataTable GetBulletinInfo()
        {
            try
            {
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@SubCategoryId", intSubCategoryId);
                SqlParam[1] = new SqlParameter("@BulletinId", intBulletinId);
                SqlParam[2] = new SqlParameter("@Status", blnStatus);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetBulletinInfoByParameters", CommandType.StoredProcedure, SqlParam);

                return dtResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetBulletinInfoByUserId()
        {
            try
            {
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@UserID", UserID);
                SqlParam[1] = new SqlParameter("@SubCategoryId", SubCategoryId);
                SqlParam[2] = new SqlParameter("@SearchType", SearchType);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetBulletinbyUserID", CommandType.StoredProcedure, SqlParam);

                return dtResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public  string GetXmlHeadings(string XSLT_File, string XML_File)
        //{
        //    XmlDocument document = new XmlDocument();
        //    XPathNavigator navigator;
        //    XslTransform transformer =new XslTransform();
        //    StringWriter output;
        //    document.Load(XML_File);
        //    output = new StringWriter();
        //    navigator = document.CreateNavigator();
        //    transformer.Load(XSLT_File);
        //    transformer.Transform(navigator, null, output);

        //    if (output.ToString().Length > 39)
        //    {
        //        string retStr = output.ToString().Substring(39, output.ToString().Length - 39);
        //        return "<Marquee height=" + '"' + "150" + '"' + " width=" + '"' + "196" + '"' + " align=" + '"' + "center" + '"' + " OnMouseOver='this.stop();' OnMouseOut='this.start();' direction='up' scrollamount='3'>" + retStr.Replace("&gt;", ">").Replace("&lt;", "<") + "</Marquee>";
        //    }
        //    else
        //        return "<br/><br/><b><u><i>Nothing to display.</b></u></i>";
        //}

        //public  string GetXmlBody(string id, string XSLT_File, string XML_File)
        //{
        //    XmlDocument document = new XmlDocument();
        //    XPathNavigator navigator;
        //    XslTransform transformer;
        //    StringWriter output;
        //    output = new StringWriter();
        //    if (id == "all" || id == "" || id == null)
        //    {
        //        document.Load(XML_File);

        //        navigator = document.CreateNavigator();
        //        transformer = new XslTransform();
        //        transformer.Load(XSLT_File);
        //        transformer.Transform(navigator, null, output);
        //        if (output.ToString().Length > 39)
        //        {
        //            string retStr = output.ToString().Substring(39, output.ToString().Length - 39);
        //            return retStr.Replace("&gt;", ">").Replace("&lt;", "<");
        //        }
        //        else
        //            return "<br/><br/><b><u><i>Nothing to display.</b></u></i>";
        //    }
        //   if (id != null)
        //    {
        //        StringWriter sw = new StringWriter();
        //        SqlParam = new SqlParameter[1];
        //        SqlParam[0] = new SqlParameter("@Bulletinid", id);
        //        DataAccess.Instance.GetDataSetFromDatabase("prcGetBulletinInfoByParameters", CommandType.StoredProcedure, SqlParam).WriteXml(sw);
        //        //DataLayerRNB.BaseClass.GetDataSet("select Heading,Body from tblEvents where id=" + id + " order by id").WriteXml(sw);
        //        document.LoadXml(sw.ToString());
        //        sw.Close();
        //        output = new StringWriter();
        //        navigator = document.CreateNavigator();
        //        transformer = new XslTransform();
        //        transformer.Load(XSLT_File);
        //        transformer.Transform(navigator, null, output);
        //    }
        //    if (output.ToString().Length > 39)
        //    {
        //        string retStr = output.ToString().Substring(39, output.ToString().Length - 39);
        //        return retStr.Replace("&gt;", ">").Replace("&lt;", "<");
        //    }
        //    else
        //        return "<br/><br/><b><u><i>Nothing to display.</b></u></i>";
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

        ~BulletinData()
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
