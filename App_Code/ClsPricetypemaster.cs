#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*/
 * ================================================================================================
 * ================================================================================================
 * COPYRIGHT (c) 2016 Zed Axis Technologies (P) Ltd.
 * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART,
 * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
 * ================================================================================================
 * Created By : Vijay Kumar Prajapati
 * Created On : 23-April-2019
 * Description : This is a copy of ClsPricetypemaster.cs
 * ================================================================================================
 * Change Log:
 * ------------- 
 * DD-MMM-YYYY, Name, #CCXX, Description
 
 * ====================================================================================================
 */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Data.SqlClient;
using System.Xml;

/// <summary>
/// Summary description for ClsPricetypemaster
/// </summary>
public class ClsPricetypemaster : IDisposable
{
    SqlParameter[] SqlParam;
    DataTable d1;
    string pricetypekeyword; string pricetypedescription; short status;
    public string error;
    public string PriceTypeKeyword
    {
        get { return pricetypekeyword; }
        set { pricetypekeyword = value; }
    }
    public string PriceTypeDescription
    {
        get { return pricetypedescription; }
        set { pricetypedescription = value; }
    }
    public short Status
    {
        get { return status; }
        set { status = value; }

    }
    public Int32 Pricetypeid { get; set; }
    public Int32 Entitytypeid { get; set; }
    public Int32 UserId { get; set; }
    public Int32 PriceTypeEntityTypeMappingID { get; set; }
    public DataTable SelectPriceTypemasterInfo()
    {
        try
        { 
            SqlParam = new SqlParameter[3];
            SqlParam[0] = new SqlParameter("@PriceTypeKeyword", PriceTypeKeyword);
            SqlParam[1] = new SqlParameter("@Status", Status);
            SqlParam[2] = new SqlParameter("@Pricetypeid", Pricetypeid);
            d1 = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetPriceTypeMaster", CommandType.StoredProcedure, SqlParam);
            return d1;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable SelectPriceTypeEntitymappingInfo()
    {
        try
        {
            SqlParam = new SqlParameter[4];
            SqlParam[0] = new SqlParameter("@Entitytypeid", Entitytypeid);
            SqlParam[1] = new SqlParameter("@Status", Status);
            SqlParam[2] = new SqlParameter("@Pricetypeid", Pricetypeid);
            SqlParam[3] = new SqlParameter("@PriceTypeEntityTypeMappingID", PriceTypeEntityTypeMappingID);
            d1 = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetPriceTypeEntityTypemapping", CommandType.StoredProcedure, SqlParam);
            return d1;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void InsertPriceTypeInfo()
    {
        try
        {
            SqlParam = new SqlParameter[6];
            SqlParam[0] = new SqlParameter("@Pricetypeid", Pricetypeid);
            SqlParam[1] = new SqlParameter("@PriceTypeDescription", PriceTypeDescription);
            SqlParam[2] = new SqlParameter("@PriceTypeKeyword", PriceTypeKeyword);
            SqlParam[3] = new SqlParameter("@status", Status);
            SqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
            SqlParam[4].Direction = ParameterDirection.Output;
            SqlParam[5] = new SqlParameter("@userId", UserId);
            int r = DataAccess.DataAccess.Instance.DBInsertCommand("prcInsUpdPriceTypeMaster", SqlParam);
            error = Convert.ToString(SqlParam[4].Value);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void InsertPriceTypeEntityMappingType()
    {
        try
        {
            SqlParam = new SqlParameter[6];
            SqlParam[0] = new SqlParameter("@PriceTypeEntityTypeMappingID", PriceTypeEntityTypeMappingID);
            SqlParam[1] = new SqlParameter("@PriceTypeId", Pricetypeid);
            SqlParam[2] = new SqlParameter("@EntityTypeId", Entitytypeid);
            SqlParam[3] = new SqlParameter("@status", Status);
            SqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
            SqlParam[4].Direction = ParameterDirection.Output;
            SqlParam[5] = new SqlParameter("@userId", UserId);
            int r = DataAccess.DataAccess.Instance.DBInsertCommand("prcInsUpdEntityMappingType", SqlParam);
            error = Convert.ToString(SqlParam[4].Value);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
	public ClsPricetypemaster()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public DataTable SelectPriceTypeInfo()
    {
        try
        {

            SqlParam = new SqlParameter[0];
            d1 = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetPricemastertype", CommandType.StoredProcedure, SqlParam);
            return d1;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable SelectEntityTypeInfo()
    {
        try
        {

            SqlParam = new SqlParameter[0];
            d1 = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetEntityTypemaster", CommandType.StoredProcedure, SqlParam);
            return d1;

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

        ~ClsPricetypemaster()
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