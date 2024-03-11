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
* Created On: 16-Aug-2019
 * Description: This is  DOA  Reports  Page Class Page.
* ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 ====================================================================================================
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using DataAccess;

public class ClsTravelClaimApproval : IDisposable
{
    #region Private Properties
    SqlParameter[] SqlParam;
    DataTable dtResult;
    private string strErrorDetailXML, strError, _SuccessMsg;
    private Int32 intSalesChannelID, intNumberOfBackDaysSC, intOrgnhierarchyID;
    public int intOutParam
    {
        get;
        set;
    }
    #endregion
    #region Public Properties

    
    public Int16 BaseEntityTypeID
    {
        get;
        set;
    }
    public Int16 EntityTypeID
    {
        get;
        set;
    }
   
    public Int32 ApprovalStatus
    {
        get;
        set;
    }

    public string ClaimNo
    {
        get;
        set;
    }
   
    public Int32 UserId   
    {
        get;
        set;
    }
    public DateTime? FromDate
    {
        get;
        set;
    }
    public DateTime? ToDate
    {
        get;
        set;
    }

    public Int32 TravelClaimCreatedById
    {
        get;
        set;
    }
    public Int32 TravelClaimApprovedById
    {
        get;
        set;
    }
    public Int32 SalesChannelID
    {
        get { return intSalesChannelID; }
        set { intSalesChannelID = value; }
    }
    
    public Int32 OrgnhierarchyID
    {
        get { return intOrgnhierarchyID; }
        set { intOrgnhierarchyID = value; }
    }

   

    public string Error
    {
        get { return strError; }
        set { strError = value; }
    }
  
    public string SuccessMsg
    {
        get { return _SuccessMsg; }
        set { _SuccessMsg = value; }
    }
   
   
    public Int32 PageIndex
    {
        get;
        set;
    }
    public Int32 PageSize
    {
        get;
        set;
    }
    public Int32 TotalRecords
    {
        get;
        set;
    }
   

    public DateTime Fromdate
    {
        get;
        set;
    }

    public DateTime Todate
    {
        get;
        set;
    }
    
    public string ApprovalRemarks
    {
        get;
        set;
    }
   
    #endregion

     public DataTable EntityType()
    {
        try
        {
           
            DataTable dtResult = new DataTable();
            SqlParam = new SqlParameter[1];
            SqlParam[0] = new SqlParameter("@UserId", UserId);
            dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetEntityTypeTravelClaim", CommandType.StoredProcedure, SqlParam);
            return dtResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
     public DataTable BindMappedName()
     {
         try
         {
             SqlParam = new SqlParameter[2];
             SqlParam[0] = new SqlParameter("@UserId", UserId);
             dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetMappedEntityTravelClaimReports", CommandType.StoredProcedure, SqlParam);
             return dtResult;
         }

         catch (Exception ex)
         {
             throw ex;
         }
     }
     public DataSet GetReportTravelClaimData()
     {
         try
         {
             SqlParameter[] prm = new SqlParameter[12];
             prm[0] = new SqlParameter("@PageIndex", PageIndex);
             prm[1] = new SqlParameter("@PageSize", PageSize);
             prm[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
             prm[2].Direction = ParameterDirection.Output;
             prm[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
             prm[3].Direction = ParameterDirection.Output;
             prm[4] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
             prm[4].Direction = ParameterDirection.Output;
             prm[5] = new SqlParameter("@ApprovedByuserid", TravelClaimApprovedById);
             prm[6] = new SqlParameter("@ApprovedTouserid", TravelClaimCreatedById);
             prm[7] = new SqlParameter("@UserDetailId", UserId);
             if (Fromdate.Year >= 1900)
             {
                 prm[8] = new SqlParameter("@DateFrom", Fromdate);
             }
             else
             {
                 prm[8] = new SqlParameter("@DateFrom", DBNull.Value);
             }
             if (Todate.Year >= 1900)
             {
                 prm[9] = new SqlParameter("@DateTo", Todate);
             }
             else
             {
                 prm[9] = new SqlParameter("@DateTo", DBNull.Value);
             }
             prm[10] = new SqlParameter("@ClaimNo", ClaimNo);
             prm[11] = new SqlParameter("@ApproveStauts", ApprovalStatus);

             DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcTravelClaimSubmited_Select", CommandType.StoredProcedure, prm);
             Error = Convert.ToString(prm[4].Value);
             TotalRecords = Convert.ToInt32(prm[2].Value);
             return dsResult;
         }
         catch (Exception ex)
         {
             throw ex;
         }
     }
	public ClsTravelClaimApproval()
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

    ~ClsTravelClaimApproval()
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