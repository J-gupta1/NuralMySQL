#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Xml;
#endregion
#region Copyright(c) 2018 Zed-Axis Technologies All rights are reserved
/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2010 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Created By : Sumit Maurya
* Created On: 28-Feb-2018
* ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 * ====================================================================================================
*/

#endregion


public class PriceDrop : IDisposable
{

    #region Properties

    public Int16 OutParam
    {
        get;
        set;
    }
    public string OutError
    {
        get;
        set;
    }
    public Int64 TotalRecords
    {
        get;
        set;
    }
    public int Result
    {
        get;
        set;
    }
    public int UserID
    {
        get;
        set;
    }
    public DataTable dtResult
    {
        get;
        set;
    }
    public DataSet dsResult
    {
        get;
        set;
    }
    public string SessionID
    {
        get;
        set;
    }

    public string XMLList
    {
        get;
        set;
    }
    public int PageSize
    {
        get;
        set;
    }
    public int PageNo
    {
        get;
        set;
    }
    public Int32 PageIndex { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }

    public string  SalesChannelCode  { get; set; }
    public string SerialNumber  { get; set; }
    public string strStatus { get; set; }
    string todate; string fromdate;
    public string ToDate1
    {
        get { return todate; }
        set { todate = value; }

    }

    public string FromDate1
    {

        get { return fromdate; }
        set { fromdate = value; }
    }
    #endregion
    


    public DataSet GetPriceDropRefData()
    {
        try
        {
            SqlParameter[] SqlParam = new SqlParameter[4];
            SqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 2000);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.Int);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@UserId", UserID);
            SqlParam[3] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
            SqlParam[3].Direction = ParameterDirection.Output;
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetPriceDropRefData", CommandType.StoredProcedure, SqlParam);
             if (SqlParam[0].Value != null && Convert.ToString(SqlParam[0].Value) != "")
             {
                 OutError = Convert.ToString(SqlParam[0].Value);
             }
             OutParam = Convert.ToInt16(SqlParam[1].Value);
             TotalRecords = Convert.ToInt64(SqlParam[3].Value);
             return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataSet SaveBulkPriceDrop()
    {
       
        SqlParameter[] SqlParam = new SqlParameter[5]; 
        SqlParam[0] = new SqlParameter("@SessionID", SessionID);
        SqlParam[1] = new SqlParameter("@OutParam", SqlDbType.Int);
        SqlParam[1].Direction = ParameterDirection.Output;
        SqlParam[2] = new SqlParameter("@OutError", SqlDbType.NVarChar, 2000);
        SqlParam[2].Direction = ParameterDirection.Output;
        SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
        SqlParam[3].Direction = ParameterDirection.Output;        
        SqlParam[4] = new SqlParameter("@UserId", UserID);
        dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcPriceDropUpload", CommandType.StoredProcedure, SqlParam);
      
        if (Convert.ToString(SqlParam[3].Value) != "")
        {
            XMLList = SqlParam[3].Value.ToString();
        }
        else
        {
            XMLList = null;
        }
        OutParam = Convert.ToInt16(SqlParam[1].Value);
        if (SqlParam[3].Value != null && Convert.ToString(SqlParam[3].Value) != "")
        {
            OutError = Convert.ToString(SqlParam[2].Value);
        }
        return dsResult;
    }

    public DataSet GetPriceDropData()
    {
        try
        {
            SqlParameter[] SqlParam = new SqlParameter[14];
            SqlParam[0] = new SqlParameter("@OutError", SqlDbType.VarChar, 2000);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@OutParam", SqlDbType.Int);
            SqlParam[1].Direction = ParameterDirection.Output;
            
            SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
            SqlParam[2].Direction = ParameterDirection.Output;     

            SqlParam[3] = new SqlParameter("@PageIndex", PageIndex);
            SqlParam[4] = new SqlParameter("@PageSize", PageSize);
            SqlParam[5] = new SqlParameter("@FromDate", FromDate1);
            SqlParam[6] = new SqlParameter("@ToDate", ToDate1);
            SqlParam[7] = new SqlParameter("@SalesChannelCode", SalesChannelCode);
            SqlParam[8] = new SqlParameter("@SerialNumber", SerialNumber);
            SqlParam[9] = new SqlParameter("@Status", strStatus);

            SqlParam[10] = new SqlParameter("@UserId", UserID);
            SqlParam[11] = new SqlParameter("@TotalRecords", SqlDbType.BigInt, 8);
            SqlParam[11].Direction = ParameterDirection.Output;
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetPriceDropData", CommandType.StoredProcedure, SqlParam);
            if (SqlParam[0].Value != null && Convert.ToString(SqlParam[0].Value) != "")
            {
                OutError = Convert.ToString(SqlParam[0].Value);
            }
            if (Convert.ToString(SqlParam[2].Value) != "")
            {
                XMLList = SqlParam[2].Value.ToString();
            }
            else
            {
                XMLList = null;
            }
            OutParam = Convert.ToInt16(SqlParam[1].Value);
            TotalRecords = Convert.ToInt64(SqlParam[11].Value);
            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet GetSerialNumberLength()
    {
        try
        {
            SqlParameter[] prm = new SqlParameter[0];
            DataSet dsresult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("PrcAPIGetSerialLengthforprimarySale", CommandType.StoredProcedure, prm);
            return dsresult;
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

    ~PriceDrop()
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

