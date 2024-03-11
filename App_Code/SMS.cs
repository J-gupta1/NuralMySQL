using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Threading;
using System.Configuration;
using System.IO;
using System.Net;
using Microsoft.ApplicationBlocks.Data;


public class SMS : IDisposable
{

    DataTable dtResult;
    DataSet dsResult;
    SqlParameter[] SqlParam;
    string proxyHost;
    Int32 proxyPort;
    DateTime? datefrom; DateTime? dateto; int roleid; int userid;

    private string _includeProxy;
    public string includeProxy
    {
        get { return GetAppSettingsKey("IncludeProxy"); }
        set { _includeProxy = value; }
    }
    private string _UID;
    public string UID
    {
        get { return GetAppSettingsKey("uid"); ; }
        set { _UID = value; }
    }
    private string _PWD;
    public string PWD
    {
        get { return GetAppSettingsKey("pwd"); ; }
        set { _PWD = value; }
    }
    private string _SMSURL;
    public string SMSURL
    {
        get { return GetAppSettingsKey("smsURL"); }
        set { _SMSURL = value; }
    }
    private string _MobNumber;
    public string MobNumber
    {
        get { return _MobNumber; }
        set { _MobNumber = value; }
    }
    private string _SMSContent;
    public string SMSContent
    {
        get { return _SMSContent; }
        set { _SMSContent = value; }
    }
    private string _strSMSResponse;
    public string strSMSResponse
    {
        get { return _strSMSResponse; }
        set { _strSMSResponse = value; }
    }
    private int _IsSent = 0;/*Pending*/
    public int IsSent
    {
        get { return _IsSent; }
        set { _IsSent = value; }
    }
    private string _SendTransNo;
    public string SendTransNo
    {
        get { return _SendTransNo; }
        set { _SendTransNo = value; }
    }
    private Int32 _TransactionID = 0;
    public Int32 TransactionID
    {
        get { return _TransactionID; }
        set { _TransactionID = value; }
    }

    public string SendSMS()
    {
        string strSMSURL;
        try
        {
            SMSContent = System.Web.HttpUtility.UrlEncode(SMSContent.Trim());
            strSMSURL = SMSURL.Replace("#uid#", UID).Replace("#pwd#", PWD).Replace("#sendto#", MobNumber).Replace("#smsmsg#", SMSContent);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(strSMSURL);
            req.Method = "GET";
            StreamReader stIn = new StreamReader(req.GetResponse().GetResponseStream());
            strSMSResponse = stIn.ReadToEnd();
        }
        catch (Exception ex)
        {
            IsSent = 0;
        }
        return strSMSResponse;
    }

    public int UpdateSMSOutboundLog()
    {
        try
        {
            DataSet dtResult = new DataSet();
            SqlParameter[] objSqlParam = new SqlParameter[12];
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@SMSMobileNumber", MobNumber);
            objSqlParam[4] = new SqlParameter("@SendText", SMSContent);
            objSqlParam[5] = new SqlParameter("@IsSent", IsSent);
            objSqlParam[6] = new SqlParameter("@APIResponse", strSMSResponse);
            objSqlParam[7] = new SqlParameter("@SendTransNo", SendTransNo);
            objSqlParam[8] = new SqlParameter("@TransactionID ", TransactionID);
            DataAccess.DataAccess.Instance.DBInsertCommand("prcInsertSMSOutboundLogAPI", objSqlParam);
            return Convert.ToInt32(objSqlParam[1].Value);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    private static string GetAppSettingsKey(string Key)
    {
        return ConfigurationManager.AppSettings[Key].ToString();
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

    ~SMS()
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
