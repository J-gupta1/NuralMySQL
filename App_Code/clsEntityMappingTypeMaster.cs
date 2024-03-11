#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2010 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Created By : Vijay Kumar Prajapati
* Created On: 22-April-2019
* ====================================================================================================
* Reviewed By :
* DD-MMM-YYYY, Name, #CCXX, Description
 ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 ====================================================================================================
*/

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Xml;

/// <summary>
/// Summary description for clsEntityMappingTypeMaster
/// </summary>
public class clsEntityMappingTypeMaster : IDisposable
{
    #region Private Class Variables
    private int _intEntityMappingTypeID;
    private string _strKeyword;
    private string _strDescription;
    private DateTime _dtCreatedOn;
    private int _intCreatedBy;
    private DateTime _dtModifiedOn;
    private int _intModifiedBy;
    private short _shtActive;

    private string _strError;
    private Int32 _intPageIndex;
    private Int32 _intPageSize;
    private Int32 _intTotalRecords;
  
    private Int16 _intOutParam;
    private string _strEntityMappingXMLError;
  
    #endregion
    #region Public Properties
 
    public DataTable UploadEntityMappingDataTable { get; set; }
    public Int16 OutParam
    {
        get
        {
            return _intOutParam;
        }
        private set
        {
            _intOutParam = value;
        }
    }
    public string EntityMappingXMLError
    {
        get { return _strEntityMappingXMLError; }
        private set { _strEntityMappingXMLError = value; }
    }
   
    public int EntityMappingTypeID
    {
        get
        {
            return _intEntityMappingTypeID;
        }
        set
        {
            _intEntityMappingTypeID = value;
        }
    }
    public string Keyword
    {
        get
        {
            return _strKeyword;
        }
        set
        {
            _strKeyword = value;
        }
    }
    public string Description
    {
        get
        {
            return _strDescription;
        }
        set
        {
            _strDescription = value;
        }
    }
    public DateTime CreatedOn
    {
        get
        {
            return _dtCreatedOn;
        }
        set
        {
            _dtCreatedOn = value;
        }
    }
    public int CreatedBy
    {
        get
        {
            return _intCreatedBy;
        }
        set
        {
            _intCreatedBy = value;
        }
    }
    public DateTime ModifiedOn
    {
        get
        {
            return _dtModifiedOn;
        }
        set
        {
            _dtModifiedOn = value;
        }
    }
    public int ModifiedBy
    {
        get
        {
            return _intModifiedBy;
        }
        set
        {
            _intModifiedBy = value;
        }
    }
    public short Active
    {
        get
        {
            return _shtActive;
        }
        set
        {
            _shtActive = value;
        }
    }

    public string Error
    {
        get
        {
            return _strError;
        }
        private set
        {
            _strError = value;
        }
    }
    public Int32 PageIndex
    {
        private get
        {
            return _intPageIndex;
        }
        set
        {
            _intPageIndex = value;
        }
    }
    public Int32 PageSize
    {
        private get
        {
            return _intPageSize;
        }
        set
        {
            _intPageSize = value;
        }
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
    public Int32 CompanyId { get; set; }
    #endregion
    #region Constructors
    public clsEntityMappingTypeMaster()
    {

    }
    #endregion
    #region Public Method Section
    /// <summary>
    /// Save records in database.
    /// </summary>
    /// <param name="pk_Id">Returns the Pk Id of last inserted record. </param>
    /// <results>Int16: 0 if success</results> 
    public Int16 Save()
    {
        Int16 result = 1;
        SqlParameter[] objSqlParam = new SqlParameter[10];
        objSqlParam[0] = new SqlParameter("@Keyword", Keyword);
        objSqlParam[1] = new SqlParameter("@Description", Description);
        objSqlParam[2] = new SqlParameter("@CreatedOn", CreatedOn);
        objSqlParam[3] = new SqlParameter("@CreatedBy", CreatedBy);
        objSqlParam[4] = new SqlParameter("@ModifiedOn", ModifiedOn);
        objSqlParam[5] = new SqlParameter("@ModifiedBy", ModifiedBy);
        objSqlParam[6] = new SqlParameter("@Active", Active);
        objSqlParam[7] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[7].Direction = ParameterDirection.Output;
        objSqlParam[8] = new SqlParameter("@PKId", SqlDbType.Int, 4);
        objSqlParam[8].Direction = ParameterDirection.Output;
        objSqlParam[9] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[9].Direction = ParameterDirection.Output;
        int save = DataAccess.DataAccess.Instance.DBInsertCommand("prcEntityMappingTypeMaster_Insert", objSqlParam);   
        result = Convert.ToInt16(objSqlParam[7].Value);
        EntityMappingTypeID = Convert.ToInt32(objSqlParam[8].Value);
        Error = Convert.ToString(objSqlParam[9].Value);
        if (Error != string.Empty)
        {
            throw new ArgumentException(Error);
        }

        return result;
    }

    /// <summary>
    /// Update records in database.
    /// </summary>
    /// <results>Int16: 0 if success</results> 
    public Int16 Update()
    {
        Int16 result = 1;
        SqlParameter[] objSqlParam = new SqlParameter[10];
        objSqlParam[0] = new SqlParameter("@EntityMappingTypeID", EntityMappingTypeID);
        objSqlParam[1] = new SqlParameter("@Keyword", Keyword);
        objSqlParam[2] = new SqlParameter("@Description", Description);
        objSqlParam[3] = new SqlParameter("@CreatedOn", CreatedOn);
        objSqlParam[4] = new SqlParameter("@CreatedBy", CreatedBy);
        objSqlParam[5] = new SqlParameter("@ModifiedOn", ModifiedOn);
        objSqlParam[6] = new SqlParameter("@ModifiedBy", ModifiedBy);
        objSqlParam[7] = new SqlParameter("@Active", Active);
        objSqlParam[8] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[8].Direction = ParameterDirection.Output;
        objSqlParam[9] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[9].Direction = ParameterDirection.Output;
        int save = DataAccess.DataAccess.Instance.DBInsertCommand("prcEntityMappingTypeMaster_Update", objSqlParam);   
        result = Convert.ToInt16(objSqlParam[8].Value);
        Error = Convert.ToString(objSqlParam[9].Value);
        if (Error != string.Empty)
        {
            throw new ArgumentException(Error);
        }

        return result;
    }
    /// <summary>
    /// Delete selected record.
    /// </summary>
    /// <results>bool: True if success</results> 
    public bool Delete()
    {
        bool result = false;
        SqlParameter[] objSqlParam = new SqlParameter[3];
        objSqlParam[0] = new SqlParameter("@EntityMappingTypeID", EntityMappingTypeID);
        objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[2].Direction = ParameterDirection.Output;
        int save = DataAccess.DataAccess.Instance.DBInsertCommand("prcEntityMappingTypeMaster_Delete", objSqlParam);  
        result = (Convert.ToInt16(objSqlParam[1].Value) == 0);
        Error = Convert.ToString(objSqlParam[2].Value);
        if (Error != string.Empty)
        {
            throw new ArgumentException(Error);
        }

        return result;
    }

    /// <summary>
    /// Get All records from database for selected key
    /// </summary>
    /// <results>DataTable: Collection of records</results> 		
    public DataTable SelectAll()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[4];
        objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
        objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
        objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
        objSqlParam[2].Direction = ParameterDirection.Output;
        objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[3].Direction = ParameterDirection.Output;
        DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcEntityMappingTypeMaster_Select",CommandType.StoredProcedure, objSqlParam);
        
        if (dsResult != null && dsResult.Tables.Count > 0)
            dtResult = dsResult.Tables[0];
        TotalRecords = Convert.ToInt32(objSqlParam[2].Value);
        Error = Convert.ToString(objSqlParam[3].Value);
        if (Error != string.Empty)
        {
            throw new ArgumentException(Error);
        }

        return dtResult;
    }

    /// <summary>
    /// Get All records from database for selected key
    /// </summary>
    /// <results>DataTable: Collection of records</results> 		
    public DataTable SelectById()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[3];
        objSqlParam[0] = new SqlParameter("@EntityMappingTypeID", EntityMappingTypeID);
        objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[2].Direction = ParameterDirection.Output;
        DataSet dsResult = new DataSet();
           //SqlHelper.ExecuteDataset(DBConnection.ConStr, CommandType.StoredProcedure, "prcEntityMappingTypeMaster_SelectById", objSqlParam);
        if (dsResult != null && dsResult.Tables.Count > 0)
            dtResult = dsResult.Tables[0];
        Error = Convert.ToString(objSqlParam[2].Value);
        if (Error != string.Empty)
        {
            throw new ArgumentException(Error);
        }

        return dtResult;
    }


    public DataTable Select()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[2];
        objSqlParam[0] = new SqlParameter("@EntityMappingTypeID", EntityMappingTypeID);
        objSqlParam[1] = new SqlParameter("@CompanyId", CompanyId);
        DataSet dsResult = new DataSet();
        dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prc_EntityMappingTypeMaster_Select", CommandType.StoredProcedure, objSqlParam);
        if (dsResult != null && dsResult.Tables.Count > 0)
            dtResult = dsResult.Tables[0];
        return dtResult;
    }

    /// <summary>
    /// Get All records from database for selected key
    /// </summary>
    /// <results>DataTable: Collection of records</results> 		
    public void Load()
    {
        //SqlParameter[] objSqlParam = new SqlParameter[3];
        //objSqlParam[0] = new SqlParameter("@EntityMappingTypeID", EntityMappingTypeID);
        //objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        //objSqlParam[1].Direction = ParameterDirection.Output;
        //objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        //objSqlParam[2].Direction = ParameterDirection.Output;
        //IDataReader reader = "";
        ////SqlHelper.ExecuteReader(DBConnection.ConStr, CommandType.StoredProcedure, "prcEntityMappingTypeMaster_SelectById", objSqlParam);
        //while (reader.Read())
        //{
        //    if (reader["Keyword"] != null)
        //        Keyword = Convert.ToString(reader["Keyword"]);
        //    if (reader["Description"] != null)
        //        Description = Convert.ToString(reader["Description"]);
        //    if (reader["CreatedOn"] != null)
        //        CreatedOn = Convert.ToDateTime(reader["CreatedOn"]);
        //    if (reader["CreatedBy"] != null)
        //        CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
        //    if (reader["ModifiedOn"] != null)
        //        ModifiedOn = Convert.ToDateTime(reader["ModifiedOn"]);
        //    if (reader["ModifiedBy"] != null)
        //        ModifiedBy = Convert.ToInt32(reader["ModifiedBy"]);
        //    if (reader["Active"] != null)
        //        Active = Convert.ToInt16(reader["Active"]);
        //}
      //  Error = Convert.ToString(objSqlParam[2].Value);
        Error = "";
        if (Error != string.Empty)
        {
            throw new ArgumentException(Error);
        }

    }

   
    #endregion
   
    public DataSet GetAllEntityMappingTypeMasterData()
    {
        DataSet dsResult = new DataSet();
       // dsResult = 
        //SqlHelper.ExecuteDataset(DBConnection.ConStr, CommandType.StoredProcedure, "prc_EntityMappingTypeMasterData");
       
        return dsResult;
    }

    public String ConStr1 = ConfigurationManager.ConnectionStrings["AppConString"].ConnectionString;
    public DataTable BulkUploadEntityMapping()
    {


        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        SqlConnection conStr = new SqlConnection(ConStr1);

        DataTable dtResult = new DataTable();

        SqlParameter[] objSqlParam = new SqlParameter[4];

        objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[0].Direction = ParameterDirection.Output;
        objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@CreatedBy", CreatedBy);
        objSqlParam[3] = new SqlParameter("@UploadEntityMappingDataTable", SqlDbType.Structured);
        objSqlParam[3].Value = UploadEntityMappingDataTable;

        cmd = new SqlCommand("prcBulkEntityMappingType_Insert", conStr);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddRange(objSqlParam);
        da.SelectCommand = cmd;

        DataSet dsResult = new DataSet();
        da.SelectCommand.CommandTimeout = 180;
        da.Fill(dsResult);

        if (dsResult != null && dsResult.Tables.Count > 0)
            dtResult = dsResult.Tables[0];
        Error = Convert.ToString(objSqlParam[1].Value);
        OutParam = Convert.ToInt16(objSqlParam[0].Value);
      
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

    ~clsEntityMappingTypeMaster()
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