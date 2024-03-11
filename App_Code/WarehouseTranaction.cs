using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using DataAccess;
using System.Data.SqlClient;
using System.Xml;

/*
 * 17-Dec-2014, Sumit Kumar, #CC01, Add some properties and function for bulk upload file.
 * 02-May-2016, Sumit Maurya, #CC02, New property created and supplied to method UploadBulkGRNandPrimaryFile()
 * 01-Jul-2016, Sumit Maurya, #CC03, New method created to get Active SKUDetails.
 * 12-Aug-2016, Sumit Maurya, #CC04, New properties and method created to Update Carton Number.
 */



public class WarehouseTranaction : IDisposable
{
    private string strAddress1, strBTMSapDetailXML, Error;
    DataTable dtResult;
    SqlParameter[] SqlParam;
    Int32 IntResultCount = 0;
    DataSet dsResult;
    SapService objSapService = new SapService();

    public string BTMSapDetailXML
    {
        get { return strBTMSapDetailXML; }
        set { strBTMSapDetailXML = value; }
    }
    public string ErrorMessage
    {
        get;
        set;
    }

    #region Insert Sap Integration for warehouse
    public void UploadBTMSapData()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@BTMDetailXML", SqlDbType.Xml);
            SqlParam[0].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(strBTMSapDetailXML, XmlNodeType.Document, null));
            SqlParam[0].Direction = ParameterDirection.InputOutput;
            SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 100);
            SqlParam[1].Direction = ParameterDirection.InputOutput;
            IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("prcSapBMTSaleUpload", SqlParam);
            if (((System.Data.SqlTypes.SqlXml)SqlParam[0].Value).IsNull != true)
            {
                strBTMSapDetailXML = ((System.Data.SqlTypes.SqlXml)SqlParam[0].Value).Value;
            }
            else
            {
                strBTMSapDetailXML = null;
            }
            //ErrorMessage = SqlParam[1].Value.ToString();
            //objSapService.ModuleName = EnumData.EnumSAPModuleName.BTMDataUpload;
            //objSapService.LogType = (int)EnumData.EnumSAPLogType.SuccessWithData;
            //objSapService.StatusValue = "Successfully inserted/Updated.";
            //objSapService.MessageDetail = "Successfully inserted/Updated.";
            //objSapService.SapServiceMethodName = "BTMUpload";
            //objSapService.SapFileName = objSapService.SapFileName;
            //objSapService.XMLData = "No Error";


        }
        catch (Exception ex)
        {
            objSapService.ModuleName = EnumData.EnumSAPModuleName.BTMDataUpload;
            objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
            objSapService.StatusValue = ex.Message;
            objSapService.MessageDetail = ex.Source + " :: " + ex.StackTrace;
            objSapService.SapServiceMethodName = "BTMUpload";
            objSapService.SapFileName = objSapService.SapFileName;
            objSapService.XMLData = strBTMSapDetailXML;
            throw ex;

        }

    }
    public void UploadBTMSapDataOnida()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@BTMDetailXML", SqlDbType.Xml);
            SqlParam[0].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(strBTMSapDetailXML, XmlNodeType.Document, null));
            SqlParam[0].Direction = ParameterDirection.InputOutput;
            SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 100);
            SqlParam[1].Direction = ParameterDirection.InputOutput;
            IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("prcSapBMTSaleUploadOnida", SqlParam);
            if (((System.Data.SqlTypes.SqlXml)SqlParam[0].Value).IsNull != true)
            {
                strBTMSapDetailXML = ((System.Data.SqlTypes.SqlXml)SqlParam[0].Value).Value;
            }
            else
            {
                strBTMSapDetailXML = null;
            }
        }
        catch (Exception ex)
        {
            objSapService.ModuleName = EnumData.EnumSAPModuleName.BTMDataUpload;
            objSapService.LogType = (int)EnumData.EnumSAPLogType.Error;
            objSapService.StatusValue = ex.Message;
            objSapService.MessageDetail = ex.Source + " :: " + ex.StackTrace;
            objSapService.SapServiceMethodName = "BTMUpload";
            objSapService.SapFileName = objSapService.SapFileName;
            objSapService.XMLData = strBTMSapDetailXML;
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

    ~WarehouseTranaction()
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
    /*#CC01 START ADDED*/
    public DataTable Upload_dt
    {
        set;
        get;
    }

    public string Upload_date
    {
        set;
        get;
    }

    public string Upload_Warehouse
    {
        set;
        get;
    }

    public string Upload_NDS
    {
        set;
        get;
    }

    public string Upload_InvoiceNo
    {
        set;
        get;
    }

    public int Upload_CreatedBy
    {
        set;
        get;
    }
    public int Upload_TotalFile { get; set; }
    
    /* #CC04 Add Start */
    public DataTable dtIMEIs
    {
        get;
        set;
    }
    public int intOutParam
    {
        get;
        set;
    }
    public string strCartonNumber
    {
        get;
        set;
    }
    public string SessionID
    {
        get;
        set;
    }
    /* #CC04 Add End */
        /* #CC02 Add Start */
        public Int16 Result { get; set; }
        private string strCartonSkUXML;
        public string CartonSkUXML
        {
            get { return strCartonSkUXML; }
            set { strCartonSkUXML = value; }
        }

        public int VendorID
        {
            set;
            get;
        }
        /* #CC02 Add End */

    public int UploadBulkGRNandPrimaryFile()
    {
        int rowAffected = 0;
        try
        {
            Dictionary<string, object> objDicPara = new Dictionary<string, object>();
            objDicPara.Add("@GrnInvoiceDate", Upload_date);
            objDicPara.Add("@Warehouse", Upload_Warehouse);
            objDicPara.Add("@NDS", Upload_NDS);
            objDicPara.Add("@InvoiceNo", Upload_InvoiceNo);
            objDicPara.Add("@CreatedBy", Upload_CreatedBy);
            objDicPara.Add("@TotalFiles", Upload_TotalFile);
            objDicPara.Add("@VendorID", VendorID);/* #CC02 Added */

            rowAffected = TempDataAccess.Instance.DBInsertBulkData("prcInsertBulkGRNPrimaryData", objDicPara, Upload_dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rowAffected;
        }
        /*#CC01 START END*/
        /* #CC02 Add Start */
        public void GetCartonSKUDetail()
        {
            try
            {

                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@BulkCartonSKUDetail", SqlDbType.Xml);
                SqlParam[0].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(strCartonSkUXML, XmlNodeType.Document, null));
                SqlParam[0].Direction = ParameterDirection.InputOutput;
                SqlParam[1] = new SqlParameter("@OutError", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
                SqlParam[2].Direction = ParameterDirection.Output;

                IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("prcValidateSKUCartonQty", SqlParam);
                Result = Convert.ToInt16(SqlParam[2].Value);
                if (((System.Data.SqlTypes.SqlXml)SqlParam[0].Value).IsNull != true)
                {
                    strCartonSkUXML = ((System.Data.SqlTypes.SqlXml)SqlParam[0].Value).Value;
                }
                else
                {
                    strCartonSkUXML = null;
                }
                if (SqlParam[1].Value != DBNull.Value && SqlParam[1].Value.ToString() != "")
                {
                    Error = (SqlParam[1].Value).ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /* #CC02 Add End */

        /* #CC03 Add Start */

    public DataSet GetManageRetailer()
    {
        try
        {
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetAllActiveSKU", CommandType.StoredProcedure);
            return dsResult;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /* #CC03 Add Start */

        /* #CC04 Add Start */
        public DataSet UpdateCartonNumber()
        {
            try
            {
                SqlParam = new SqlParameter[4];
                /* SqlParam[0] = new SqlParameter("@TVPSerials", SqlDbType.Structured);
                 SqlParam[0].Value = dtIMEIs;
                 SqlParam[0].Direction = ParameterDirection.Input;*/
                SqlParam[0] = new SqlParameter("@OutError", SqlDbType.VarChar, 500);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@SessionID", SessionID);
                dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcUpdateCartonNumber", CommandType.StoredProcedure, SqlParam);
                intOutParam = Convert.ToInt16(SqlParam[1].Value);
                if (SqlParam[0].Value != DBNull.Value && SqlParam[0].Value.ToString() != "")
                {
                    Error = (SqlParam[0].Value).ToString();
                }
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    /* #CC04 Add End */
}



