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
* Created On: 04-Dec-2018 
 * Description: This is  Upload Beat Plan   Page Class Page.
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
/// Summary description for clsBeatPlan
/// </summary>
public class clsBeatPlan : IDisposable
{
    private Int32 _LoginUserId;
    private string _SessionId;
    private Int32 _OutParam;
    private string _OutError;
	public clsBeatPlan()
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
    public Int64 CompanyId { get; set; }
    public Int16 TemplateType { get; set; }
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
        objSqlParam[4] = new SqlParameter("@CompanyId", CompanyId);
        dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("PrcUploadBeatPlan", CommandType.StoredProcedure, objSqlParam);
        OutParam = Convert.ToInt16(objSqlParam[2].Value);
        OutError = Convert.ToString(objSqlParam[3].Value);

        return dsResult;
    }
    public DataSet GetAllTemplateData()
    {
        DataSet dsResult = new DataSet();
        try
        {
            SqlParameter[] SqlParam = new SqlParameter[4];
            SqlParam[0] = new SqlParameter("@Userid", LoginUserId);
            SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt,2);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.NVarChar,500);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@CompanyId", CompanyId);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("PrcGetEntityTypeForBeatPlanTSMAPP", CommandType.StoredProcedure, SqlParam);
            return dsResult;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataSet GetBeatPlanUploadTemplate()
    {
        DataSet dsResult = new DataSet();
        try
        {
            SqlParameter[] SqlParam = new SqlParameter[6];
            SqlParam[0] = new SqlParameter("@Userid", LoginUserId);
            SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@CompanyID", CompanyId);
            SqlParam[4] = new SqlParameter("@TemplateType", TemplateType);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("PrcGetISPTemplate", CommandType.StoredProcedure, SqlParam);
            OutError = Convert.ToString(SqlParam[2].Value);
            return dsResult;
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

    ~clsBeatPlan()
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