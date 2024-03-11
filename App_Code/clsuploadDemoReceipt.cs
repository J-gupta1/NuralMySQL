using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for clsuploadDemoReceipt
/// </summary>
public class clsuploadDemoReceipt : IDisposable
{
    public string WOFileXML { set; get; }
    public string OutError { set; get; }
    public string POInstruction { set; get; }
    public string RetailerCode { get; set; }
    public string imagePath { get; set; }
    public int WebUserId{get;set;}
	public clsuploadDemoReceipt()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public int SaveUploadDemoReceipt()
    {
        try
        {
            int result;
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@RetailerCode", RetailerCode);
            param[1] = new SqlParameter("@webUserId", WebUserId);
            param[2] = new SqlParameter("@Out_Error", SqlDbType.NVarChar,50);
            param[2].Direction = ParameterDirection.Output;
            param[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            param[3].Direction = ParameterDirection.Output;
            param[4] = new SqlParameter("@ImagePath", imagePath);
            Int32 IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("prcSaveDemoReceipt", param);
            return result = Convert.ToInt32(param[3].Value);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public DataTable SelectDemoReceiptData()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[3];
        objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[0].Direction = ParameterDirection.Output;
        objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 50);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@webUserId", WebUserId);
        dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetDemoReceipt", CommandType.StoredProcedure, objSqlParam);

        return dtResult;
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

    ~clsuploadDemoReceipt()
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