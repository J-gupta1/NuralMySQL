using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data.SqlClient;
using System.Xml;
using System.Configuration;
using DataAccess;
/// <summary>
/// Summary description for ClsSAPIntegrationDNPSI
/// </summary>
public class ClsSAPIntegrationDNPSI : IDisposable
{
    private Int32 _LoginUserId;
    private string _SessionId;
    private Int32 _OutParam;
    private string _OutError;
    private EnumData.eControlRequestTypeForEntry eReqType;
    private Int32 intSalesChannelID,  intUserID, intOrgnhierarchyID, intGroupParentSalesChannelID, intParentSalesChannelID, intBrand, intProducteCategoryid, intSalesChannelTo;
    private EnumData.eSalesChannelLevel eSalesChanneLevel;
    private Int64 _ReferenceType_id = 0;
	public ClsSAPIntegrationDNPSI()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public Int32 LoginUserId
    {
        get
        {
            return _LoginUserId;
        }
        set
        {
            _LoginUserId = value;
        }
    }
    public string SessionId
    {
        get
        {
            return _SessionId;
        }
        set
        {
            _SessionId = value;
        }
    }
    public Int32 OutParam
    {
        get
        {
            return _OutParam;
        }
        set
        {
            _OutParam = value;
        }
    }
    public string OutError
    {
        get
        {
            return _OutError;
        }
        set
        {
            _OutError = value;
        }
    }
    public string strOriginalFileName { get; set; }
    public string strUploadedFileName { get; set; }
    public int SalesChannelId { get; set; }
    public string WOFileXML { set; get; }
    public DataTable UploadImageData { get; set; }
    public Int64 PSIID { get; set; }
    public Int64 ReferenceType_id
    {
        get
        {
            return _ReferenceType_id;
        }
        set
        {
            _ReferenceType_id = value;
        }
    }
    public EnumData.eControlRequestTypeForEntry ReqType
    {
        get { return eReqType; }
        set { eReqType = value; }
    }
    public Int32 UserID
    {
        get { return intUserID; }
        set { intUserID = value; }
    }
    public Int32 SalesChannelID
    {
        get { return intSalesChannelID; }
        set { intSalesChannelID = value; }
    }
    public Int32 Brand
    {
        get { return intBrand; }
        set { intBrand = value; }
    }
    public string FromCode { get; set; }
    public string ToCode { get; set; }
    public string DNNumber { get; set; }
    public Int16 DataType { get; set; }
    public string InvoiceNumber { get; set; }
    public int DateType { get; set; }
    public Int32 TotalRecords { get; set; }
    
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public string FromDate { get; set; }
    public string ToDate { get; set; }
    public DataSet Insert()
    {
        DataSet dsResult = new DataSet();

        SqlParameter[] objSqlParam = new SqlParameter[6];
        objSqlParam[0] = new SqlParameter("@UserId", LoginUserId);
        objSqlParam[1] = new SqlParameter("@SessionID", SessionId);
        objSqlParam[2] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
        objSqlParam[2].Direction = ParameterDirection.Output;
        objSqlParam[3] = new SqlParameter("@OutError", SqlDbType.VarChar, 500);
        objSqlParam[3].Direction = ParameterDirection.Output;
        objSqlParam[4] = new SqlParameter("@OriginalFileName", strOriginalFileName);
        objSqlParam[5] = new SqlParameter("@UniqueFileName", strUploadedFileName);
        dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("PrcUploadSAPIntegrationPSI", CommandType.StoredProcedure, objSqlParam);
        OutParam = Convert.ToInt16(objSqlParam[2].Value);
        OutError = Convert.ToString(objSqlParam[3].Value);

        return dsResult;
    }
    public DataSet GetBinCode()
    {
        DataSet dsResult = new DataSet();
        try
        {
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetStockBinTypeInfo", CommandType.StoredProcedure);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet GetAllTemplateData()
    {
        DataSet dsResult = new DataSet();
        try
        {
            SqlParameter[] SqlParam = new SqlParameter[5];
            SqlParam[0] = new SqlParameter("@UserID", UserID);
            SqlParam[1] = new SqlParameter("@ReqType", eReqType);
            SqlParam[2] = new SqlParameter("@SecondarySalesChannelID", SalesChannelID);
            SqlParam[3] = new SqlParameter("@SalesChanneLevel", eSalesChanneLevel);
            SqlParam[4] = new SqlParameter("@BrandID", Brand);
           
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetAllTemplateData", CommandType.StoredProcedure, SqlParam);
            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataSet GetAllDNDetailData()
    {
        DataSet dsResult = new DataSet();
        try
        {
            SqlParameter[] SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@UserID", UserID);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetAllDNNumberData", CommandType.StoredProcedure, SqlParam);
            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet InsertInvoiceInfo()
    {
        DataSet dsResult = new DataSet();

        SqlParameter[] objSqlParam = new SqlParameter[6];
        objSqlParam[0] = new SqlParameter("@UserId", LoginUserId);
        objSqlParam[1] = new SqlParameter("@SessionID", SessionId);
        objSqlParam[2] = new SqlParameter("@OutParam", SqlDbType.TinyInt, 2);
        objSqlParam[2].Direction = ParameterDirection.Output;
        objSqlParam[3] = new SqlParameter("@OutError", SqlDbType.VarChar, 500);
        objSqlParam[3].Direction = ParameterDirection.Output;
        objSqlParam[4] = new SqlParameter("@OriginalFileName", strOriginalFileName);
        objSqlParam[5] = new SqlParameter("@UniqueFileName", strUploadedFileName);
        dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("PrcUploadSAPIntegrationInvoiceDetail", CommandType.StoredProcedure, objSqlParam);
        OutParam = Convert.ToInt16(objSqlParam[2].Value);
        OutError = Convert.ToString(objSqlParam[3].Value);

        return dsResult;
    }
    public int SaveImgSaperateByProcess()
    {
        try
        {
            int result;
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@ReferenceType_id", ReferenceType_id);
            param[1] = new SqlParameter("@webUserId", LoginUserId);
            param[2] = new SqlParameter("@TvpFile", UploadImageData);
            param[3] = new SqlParameter("@SalesChannelId", SalesChannelId);
            param[4] = new SqlParameter("@Out_Param", SqlDbType.NVarChar, 50);
            param[4].Direction = ParameterDirection.Output;
            result = DataAccess.DataAccess.Instance.DBInsertCommand("prcUploadInvoicePDfSaperateByProcess", param);

            return result = Convert.ToInt32(param[4].Value);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public DataSet GetPSIInvoiceData()
    {
        try
        {   
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[13];
            param[0] = new SqlParameter("@FromSalesChannelCode", FromCode);
            param[1] = new SqlParameter("@UserID", UserID);
            param[2] = new SqlParameter("@ToSalesChannelCode", ToCode);
            param[3] = new SqlParameter("@DNNumber", DNNumber);
            param[4] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
            param[4].Direction = ParameterDirection.Output;
            param[5] = new SqlParameter("@OutError", SqlDbType.NVarChar, 1000);
            param[5].Direction = ParameterDirection.Output;
            param[6] = new SqlParameter("@InvoiceNumber", InvoiceNumber);
            param[7] = new SqlParameter("@DateType", DateType);
            param[8] = new SqlParameter("@FromDate", FromDate);
            param[9] = new SqlParameter("@ToDate", ToDate);
            param[10] = new SqlParameter("@PageIndex", PageIndex);
            param[11] = new SqlParameter("@PageSize", PageSize);
            param[12] = new SqlParameter("@TotalRecord", SqlDbType.Int);
            param[12].Direction = ParameterDirection.Output;
            ds = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetSAPDNData",CommandType.StoredProcedure, param);
            OutParam = Convert.ToInt32(param[4].Value);
            OutError = Convert.ToString(param[5].Value);
            TotalRecords = Convert.ToInt32(param[12].Value);

            return ds;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public int CancelPsi()
    {
        try
        {
            
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@SAPDNPSIId", PSIID);
            param[1] = new SqlParameter("@UserId", UserID);
            param[2] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@OutError", SqlDbType.NVarChar, 1000);
            param[3].Direction = ParameterDirection.Output;

            DataAccess.DataAccess.Instance.DBInsertCommand("prcCancelSAPPSI", param);
            OutParam = Convert.ToInt32(param[2].Value);
            OutError = Convert.ToString(param[3].Value);


            return OutParam;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public DataSet GetAnnuxerPrint(Int64 StockDispatchID)
    {
        try
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@PSIID", StockDispatchID);
            param[1] = new SqlParameter("@UserID", UserID);
            param[2] = new SqlParameter("@OutParam", SqlDbType.TinyInt);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@OutError", SqlDbType.NVarChar, 1000);
            param[3].Direction = ParameterDirection.Output;
            ds = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetSAPDetailData", CommandType.StoredProcedure, param);
            
            return ds;
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

    ~ClsSAPIntegrationDNPSI()
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