/*Change Log:
 * --===================================================================================================--
 * 19-Feb-2016, Sumit Mauyra, #CC01, New properties GRNNumber, IsSerialRequired, Result and method GetGRNReportDataCSV created.
 * 27-Feb-2019,Vijay Kumar Prajapati,#CC02,Add new Method for getbrand.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for TempClass
/// </summary>
public class TempClass : IDisposable
{

    DataTable dtResult;
    DataSet dsResult;
    SqlParameter[] SqlParam;
    public string error, strSalesChannelName, StrMobileNumber, StrIMEINo;
    private Int32 intSalesChanelTypeID, intSalesChannelID, intHierarchyLevelId, intModuleID, intOrgHierarchyId;
    string tablename, filePath;
    private Int32 intSalesType; Int16 companytype, intTagetBasedOn;

    private Int32 _intTotalRecords;

    public string SalesChannelCode
    {
        get;
        set;
    }

    DateTime? datefrom; DateTime? dateto; int roleid; int userid;
    /* #CC01 Add Start */
    string grnnumber;
    public string GRNNumber
    {
        get { return grnnumber; }
        set { grnnumber = value; }

    }
    public int IsSerialRequired { get; set; }
    public int Result
    {
        get;
        set;
    }
    /* #CC01 Add Start */


    public string FilePath
    {
        get { return filePath; }
        set { filePath = value; }
    }

    public DateTime? DateFrom
    {
        get { return datefrom; }
        set { datefrom = value; }
    }
    public DateTime? DateTo
    {
        get { return dateto; }
        set { dateto = value; }
    }
    public int RoleId
    {
        get { return roleid; }
        set { roleid = value; }
    }
    public Int32 SalesChannelTypeID
    {
        get { return intSalesChanelTypeID; }
        set { intSalesChanelTypeID = value; }
    }
    public Int32 HierarchyLevelId
    {
        get { return intHierarchyLevelId; }
        set { intHierarchyLevelId = value; }
    }
    public Int32 SalesChannelID
    {
        get { return intSalesChannelID; }
        set { intSalesChannelID = value; }
    }
    public Int32 SalesType
    {
        get { return intSalesType; }
        set { intSalesType = value; }
    }
    public int UserId
    {
        get { return userid; }
        set { userid = value; }
    }

    public string ISPCode
    {
        get;
        set;
    }



    public Int32 TotalRecords
    {
        get
        {
            return _intTotalRecords;
        }
        private set
        {
            _intTotalRecords = value;
        }
    }
    public int BrandId { get; set; }/*#CC02 Added*/
   

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

    ~TempClass()
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

    public DataSet GetOPSIReport()//Pankaj Kumar
    {
        try
        {
            SqlParam = new SqlParameter[6];
            SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
            SqlParam[1] = new SqlParameter("@DateTo", DateTo);
            SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
            SqlParam[3] = new SqlParameter("@UserId", UserId);
            SqlParam[4] = new SqlParameter("@SalesChannelCode", SalesChannelCode);
            SqlParam[5] = new SqlParameter("@SalesChannelTypeid", SalesChannelTypeID);
            //dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcGetOPSIRpt", CommandType.StoredProcedure, SqlParam);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("PrcGetOPSIRptV5", CommandType.StoredProcedure, SqlParam);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public Int32 GetRSPDSRReport()
    {
        try
        {
            Int32 intResult = 1;
            SqlParam = new SqlParameter[8];
            SqlParam[0] = new SqlParameter("@DateFrom", DateFrom);
            SqlParam[1] = new SqlParameter("@DateTo", DateTo);
            SqlParam[2] = new SqlParameter("@UserId", UserId);
            SqlParam[3] = new SqlParameter("@SalesChannelTypeId", SalesChannelTypeID);
            SqlParam[4] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[4].Direction = ParameterDirection.Output;
            SqlParam[5] = new SqlParameter("@filepath", FilePath);
            SqlParam[6] = new SqlParameter("@SalesChannelid", SalesChannelID);
            SqlParam[7] = new SqlParameter("@ISPCode", ISPCode);
            DataAccess.DataAccess.Instance.DBInsertCommand("PrcGetRSPDSRReport", SqlParam);
            intResult = Convert.ToInt32(SqlParam[4].Value);
            return intResult;
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    public Int32 GetRSPStockReport()
    {
        try
        {
            Int32 intResult = 1;
            SqlParam = new SqlParameter[8];
            SqlParam[0] = new SqlParameter("@DateFrom", DateFrom);
            SqlParam[1] = new SqlParameter("@DateTo", DateTo);
            SqlParam[2] = new SqlParameter("@UserId", UserId);
            SqlParam[3] = new SqlParameter("@SalesChannelTypeId", SalesChannelTypeID);
            SqlParam[4] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[4].Direction = ParameterDirection.Output;
            SqlParam[5] = new SqlParameter("@filepath", FilePath);
            SqlParam[6] = new SqlParameter("@SalesChannelid", SalesChannelID);
            SqlParam[7] = new SqlParameter("@ISPCode", ISPCode);
            DataAccess.DataAccess.Instance.DBInsertCommand("PrcGetRSPStockReport", SqlParam);
            intResult = Convert.ToInt32(SqlParam[4].Value);
            return intResult;
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    public Int32 GetRSPAttendanceReport()
    {
        try
        {
            Int32 intResult = 1;
            SqlParam = new SqlParameter[8];
            SqlParam[0] = new SqlParameter("@DateFrom", DateFrom);
            SqlParam[1] = new SqlParameter("@DateTo", DateTo);
            SqlParam[2] = new SqlParameter("@UserId", UserId);
            SqlParam[3] = new SqlParameter("@SalesChannelTypeId", SalesChannelTypeID);
            SqlParam[4] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[4].Direction = ParameterDirection.Output;
            SqlParam[5] = new SqlParameter("@filepath", FilePath);
            SqlParam[6] = new SqlParameter("@SalesChannelid", SalesChannelID);
            SqlParam[7] = new SqlParameter("@ISPCode", ISPCode);
            DataAccess.DataAccess.Instance.DBInsertCommand("PrcGetRSPAttendanceReport", SqlParam);
            intResult = Convert.ToInt32(SqlParam[4].Value);
            return intResult;
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    public DataSet TertiaryCount()/*Karam */
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@Date", DateFrom);
            SqlParam[1] = new SqlParameter("@BrandId", BrandId);/*#CC02 Added*/
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcTertiaryCountThreeDays", CommandType.StoredProcedure, SqlParam);
            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public DataTable GetNonActivatedIMEI() /* Sumit Maurya */
    {
        try
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[2].Direction = ParameterDirection.Output;
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcReportNonActivatedIMEI", CommandType.StoredProcedure, objSqlParam);
            TotalRecords = Convert.ToInt32(objSqlParam[2].Value);
            return dtResult;
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }


    /* #CC01 Add Start */
    public DataSet GetGRNReportDataCSV()
    {
        SqlParam = new SqlParameter[8];
        SqlParam[0] = new SqlParameter("@saleschannelid", SalesChannelID);
        SqlParam[1] = new SqlParameter("@grnnumber", GRNNumber);
        SqlParam[2] = new SqlParameter("@IsSerialRequired", IsSerialRequired);
        SqlParam[3] = new SqlParameter("@datefrom", DateFrom);
        SqlParam[4] = new SqlParameter("@dateto", DateTo);
        SqlParam[5] = new SqlParameter("@UserID", UserId);
        SqlParam[6] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        SqlParam[6].Direction = ParameterDirection.Output;
        SqlParam[7] = new SqlParameter("@filepath", FilePath);
        dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetGRNDetailsCSV", CommandType.StoredProcedure, SqlParam);
        Result = Convert.ToInt32(SqlParam[6].Value);
        return dsResult;
    }
    /* #CC01 Add Start */
    /*#CC02 Added Started*/
    public DataTable GetBrand()
    {

        try
        {
            SqlParam = new SqlParameter[0];
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetBrand", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    /*#CC02 Added End*/



}
