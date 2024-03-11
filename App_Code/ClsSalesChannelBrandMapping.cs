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
* Created On: 10-May-2019 
 * Description: This is  SaleschannelBrand Mapping upload  Page Class.
* ====================================================================================================
 * Change Log
 * 09-Jul-2019, Balram Jha, #CC01, Brand category load Method
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
/// Summary description for ClsSalesChannelBrandMapping
/// </summary>
public class ClsSalesChannelBrandMapping : IDisposable
{
	private Int32 intUserID, intSalesChannelID, _SalesChannelTypeID;
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
	public Int32 ProductCategoryId { get; set; }
	public Int32 BrandId { get; set; }
	public string TransUploadSession { get; set; }
	public string Error { get; set; }
	public string ErrorDetailXML { get; set; }

	public DataSet GetAllTemplateData()
	{
		try
		{
			SqlParam = new SqlParameter[2];
			SqlParam[0] = new SqlParameter("@UserID", UserID);
			SqlParam[1] = new SqlParameter("@SalesChannelID", SalesChannelID);
			dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetAllEntityBrandMapping", CommandType.StoredProcedure, SqlParam);
			return dsResult;
		}

		catch (Exception ex)
		{
			throw ex;
		}
	}
	public DataSet InsertSalesChannelBrandInfoSBBCP()
	{
		try
		{
			SqlParam = new SqlParameter[5];
			SqlParam[0] = new SqlParameter("@TransUploadSession", TransUploadSession);
			SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
			SqlParam[1].Direction = ParameterDirection.Output;
			SqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
			SqlParam[2].Direction = ParameterDirection.Output;
			SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
			SqlParam[3].Direction = ParameterDirection.Output;
			SqlParam[4] = new SqlParameter("@userid", UserID);
			dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("PrcInsertBrandSalesChannelMappingInfo", CommandType.StoredProcedure, SqlParam);
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

	public DataSet GetAllBrandandProductCategory()
	{
		try
		{
			DataSet dsResult = new DataSet();
			SqlParam = new SqlParameter[0];
			dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetAllProductCategoryandbrand", CommandType.StoredProcedure, SqlParam);
			return dsResult;
		}

		catch (Exception ex)
		{
			throw ex;
		}
	}
	public DataTable GetSalesChannelTypeForBrand()
	{
		try
		{
			SqlParam = new SqlParameter[1];
			SqlParam[0] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
			dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelTypeForBrandMapping", CommandType.StoredProcedure, SqlParam);
			return dtResult;
		}

		catch (Exception ex)
		{
			throw ex;
		}
	}
	public DataTable GetSalesChannelInfoForProductCategory()
	{
		try
		{
			SqlParam = new SqlParameter[3];
			SqlParam[0] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
			SqlParam[1] = new SqlParameter("@ProductCategoryId", ProductCategoryId);
			SqlParam[2] = new SqlParameter("@BrandId", BrandId);
			dtResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetSalesChannelInfoForProductCategoryandBrand", CommandType.StoredProcedure, SqlParam);

			return dtResult;
		}

		catch (Exception ex)
		{
			throw ex;
		}
	}

    public DataSet GetAllBrandandCategory()//#CC01 added
	{
		try
		{
			DataSet dsResult = new DataSet();
			SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@EntityTypeId", SalesChannelTypeID);
            SqlParam[1] = new SqlParameter("@EntityId", SalesChannelID);
            dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetBrandCategory", CommandType.StoredProcedure, SqlParam);
			return dsResult;
		}

		catch (Exception ex)
		{
			throw ex;
		}
	}

	public ClsSalesChannelBrandMapping()
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

	~ClsSalesChannelBrandMapping()
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