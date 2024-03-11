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
* Created By : Vijay Kumar Prajapati
* Created On: 03-May-2019 
 * Description: This is  EntityAddressInfo upload  Page Class.
* ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 ====================================================================================================
*/
#endregion
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
/// Summary description for ClsEntityAddressInfo
/// </summary>
public class ClsEntityAddressInfo : IDisposable
{
    private Int32 intUserID, intSalesChannelID, _SalesChannelTypeID ,pvt_stateid,pvt_cityid,pvt_status ;
    SqlParameter[] SqlParam;
    DataSet dsResult;
    DataTable dtResult;
    Int32 IntResultCount = 0;
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
    public Int32 SalesChannelTypeID
    {
        private get
        {
            return _SalesChannelTypeID;
        }
        set
        {
            _SalesChannelTypeID = value;
        }
    }
    public int STateid { get; set; }
    public Int64 EntityAddressID { get; set; } 
    public int pageindex { get; set; }
    public int pagesize { get; set; }
    public int Cityid { get; set; }
    public Int16 Status { get; set; }
    public string TransUploadSession { get; set; }
    public string Error { get; set; }
    public string ErrorDetailXML { get; set; }
    public int Totalrecords { get; set; }
    public DataSet GetAllTemplateData()
    {
        try
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@UserID", UserID);
            SqlParam[1] = new SqlParameter("@SalesChannelID", SalesChannelID);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetAllEntityInfoData", CommandType.StoredProcedure, SqlParam);
            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet InsertEntityAddressInfoSBBCP()
    {
        try
        {
            SqlParam = new SqlParameter[6];
            SqlParam[0] = new SqlParameter("@TransUploadSession", TransUploadSession);
            SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
            SqlParam[3].Direction = ParameterDirection.Output;
            SqlParam[4] = new SqlParameter("@userid", UserID);
            SqlParam[5] = new SqlParameter("@SalesChannelId", SalesChannelID);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("PrcInsertEntityAddressInfo", CommandType.StoredProcedure, SqlParam);
            IntResultCount = Convert.ToInt16(SqlParam[1].Value);
            if (SqlParam[3].Value.ToString() != "")
            {
                ErrorDetailXML = SqlParam[3].Value.ToString();
            }
            else
            {
                ErrorDetailXML = null;
            }
            Error = Convert.ToString(SqlParam[2].Value);

            return dsResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetSalesChannelTypeV5API()
    {
        try
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelTypeDatabaseV5", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable BindSalesChannelName()
    {
        try
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@saleschanneltypeid", SalesChannelTypeID);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelname", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GETstatevisecity(int stateid)
    { 
        try
        {
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@stateid",stateid);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGETstatevisecity", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
        
    }

    public DataSet GETADDRESSDETAILS()
    {
        try
        {
            SqlParam = new SqlParameter[10];
            SqlParam[0] = new SqlParameter("@stateid",   STateid);
            SqlParam[1] = new SqlParameter("@Cityid", Cityid);
            SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
            SqlParam[3] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
            SqlParam[4] = new SqlParameter("@Status", Status);
            
            SqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.Int);
            SqlParam[5].Direction = ParameterDirection.Output;
            SqlParam[6] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 2000);
            SqlParam[6].Direction = ParameterDirection.Output;
            SqlParam[7] = new SqlParameter("@PageIndex", pageindex);
            SqlParam[8] = new SqlParameter("@PageSize", pagesize);
            SqlParam[9] = new SqlParameter("@Totalrecords", SqlDbType.BigInt, 7);
            SqlParam[9].Direction = ParameterDirection.Output;
           
            DataSet ds = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("PRCGETADDRESSDETAILS", CommandType.StoredProcedure, SqlParam);
            Totalrecords = Convert.ToInt32(SqlParam[9].Value);
            Error = SqlParam[6].Value.ToString();
            return ds;
        }

        catch (Exception ex)
        {
            throw ex;
        }
        

    }
    public void EntityAddressInfo()
    {
        SqlParam = new SqlParameter[4];
        SqlParam[0] = new SqlParameter("@EntityAddressID", EntityAddressID);
        SqlParam[1] = new SqlParameter("@Userid", UserID);
         

        SqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.Int);
        SqlParam[2].Direction = ParameterDirection.Output;
        SqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 2000);
        SqlParam[3].Direction = ParameterDirection.Output;
        IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("PrcEntityAddressInfo", SqlParam);
                Error = Convert.ToString(SqlParam[3].Value);
         

       

    }
    public ClsEntityAddressInfo()
    {
        //
        // TODO: Add constructor logic here
        //
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

    ~ClsEntityAddressInfo()
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