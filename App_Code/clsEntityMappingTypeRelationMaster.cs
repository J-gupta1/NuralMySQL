﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;

/*        
============================================================================================================================================        
Copyright : Zed-Axis Technologies, 2011        
Author  : Pankaj Mittal       
Create date : 16-nov-2011        
Description : Select Relation of primary and secondary entity        
============================================================================================================================================        
Change Log:        
dd-MMM-yy, Name , #CCx - Description        
--------------------------------------------------------------------------------------------------------------------------------------------        
10-feb-2012,Pankaj Mittal , #cc01 - change from short to int because its entityid not type (variable name make confusion)
06-Jun-2013,Shilpi Sharma,  #CC02- added a parameter and a function to update active status.
07-Jun-2013, Shilpi Sharma,#CC03: New Version is Created of this Function as old interface is using this fucntion
                                  and a new fuction is created to bind jq Grid in edit table mode.
17-Jun-2013, Shilpi Sharma, #CC04: Function Changed From Datatable To Dataset.
11-Nov-2013, Shilpi Sharma, #CC05: Function Created To Provide Searching.
03-Sep-2014, Shilpi Sharma, #CC06: Change from int16 to int64
06-Nov-2014, Rajesh Upadhyay, #CC07: add private variable and property for secondary party
14-Jan-2015, Rajesh Upadhyay, #CC08:add private variable and property for LoginEntityTypeId and LoginEntityId
11-Mar-2015, Rajesh Upadhyay, #CC09:add property and sqlparameters in methods
27-Nov-2015, Vijay Katiyar,   #CC10 Added method SelectPartGroup()

 */

public class clsEntityMappingTypeRelationMaster : IDisposable
{
    public static string _ConnectionString = ConfigurationManager.ConnectionStrings["AppConString"].ConnectionString;
    #region Private Class Variables
    private int _intEntityMappingTypeRelationID;
    private int _intEntityMappingTypeID;
    private short _shtPrimaryEntityTypeID;
    private short _shtSecondaryEntityTypeID;
    private DateTime _dtCreatedOn;
    private int _intCreatedBy;
    private DateTime _dtModifiedOn;
    private int _intModifiedBy;
    private short _shtActive;
    private string _SecondaryParty;/*CC07:Added*/
    private string _strError;
    private Int32 _intPageIndex;
    private Int32 _intPageSize;
    private Int32 _intTotalRecords;
    private int _intEntityMappingID;/*#CC02: added*/
    private short _shtSecondaryEntityID;/*#CC02: added*/
    private string _strCity;/*#CC05:added*/
    private string _strState;/*#CC05:added*/
    private int _intLoginEntityTypeId;/*#CC08:added*/
    private int _intLoginEntityId;/*#CC08:added*/
    private string _strEntityMappingXMLError;
    private Int16 _intPartGroupType;/* #CC10 Added */
    #endregion
    #region Public Properties
    /*#CC10:added (start)*/

    public DataTable dtPartGroup { get; set; }
    public Int16 PartGroupType
    {
        get { return _intPartGroupType; }
        set { _intPartGroupType = value; }
    } /*#CC10:added (end)*/

    /*#CC09:added (start)*/
    public string EntityMappingXMLError
    {
        get { return _strEntityMappingXMLError; }
        private set { _strEntityMappingXMLError = value; }
    } /*#CC09:added (end)*/
    /*#CC08:added (start)*/
    public int LoginEntityTypeId
    {
        get
        {
            return _intLoginEntityTypeId;
        }
        set
        {
            _intLoginEntityTypeId = value;
        }
    }
    public int LoginEntityId
    {
        get
        {
            return _intLoginEntityId;
        }
        set
        {
            _intLoginEntityId = value;
        }
    }
    /*#CC08:added (end)*/
    public string SecondaryParty/*CC07:Added*/
    {
        get
        {
            return _SecondaryParty;
        }
        set
        {
            _SecondaryParty = value;
        }
    }
    public int EntityMappingTypeRelationID
    {
        get
        {
            return _intEntityMappingTypeRelationID;
        }
        set
        {
            _intEntityMappingTypeRelationID = value;
        }
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
    public short PrimaryEntityTypeID
    {
        get
        {
            return _shtPrimaryEntityTypeID;
        }
        set
        {
            _shtPrimaryEntityTypeID = value;
        }
    }
    public short SecondaryEntityTypeID
    {
        get
        {
            return _shtSecondaryEntityTypeID;
        }
        set
        {
            _shtSecondaryEntityTypeID = value;
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
    public Int16 Out_Param { get; set; }
    public int UserID { get; set; }
    public Int16 MappingMode { get; set; }
    public string Remarks { get; set; }
    public int Condition { get; set; }
    public Int32 CompanyId { get; set; }
    public Int32 PageNumber { get; set; }
    public Int32 ParentCategoryId { get; set; }
    public Int32 CategoryForId { get; set; }
    public String CategoryName { get; set; }
    public String ParentCategoryName { get; set; }
    public Int32 CategoryId { get; set; }
    public Int32 MenuId { get; set; }
    public Int32 RoleId { get; set; }
    public Int16 MenuTypeId { get; set; }
    public Int16 DisplayOrder { get; set; }
    public Int32 AppMenuRoleId { get; set; }
    public Int32 PaymentModeId { get; set; }
    public String PaymentModeName { get; set; }
    public String PaymentModeCode { get; set; }
    public DataTable dtSaveManulist;
    /*#CC02: added (start) */
    public int EntityMappingID
    {
        get
        {
            return _intEntityMappingID;
        }
        set
        {
            _intEntityMappingID = value;
        }
    }
    public short SecondaryEntityID
    {
        get
        {
            return _shtSecondaryEntityID;
        }
        set
        {
            _shtSecondaryEntityID = value;
        }
    }
    /*#CC02: added (End) */
    /*#CC05:added (start)*/
    public string City
    {
        get
        {
            return _strCity;
        }
        set
        {
            _strCity = value;
        }
    }
    public string State
    {
        get
        {
            return _strState;
        }
        set
        {
            _strState = value;
        }
    }
    /*#CC05:added (end)*/
    public bool? Status { get; set; }
    public Int16 SearchType { get; set; }
   
    #endregion
    #region Constructors
    public clsEntityMappingTypeRelationMaster()
    {

    }
    #endregion
    #region Dispose
    private bool IsDisposed = false;

    //Call Dispose to free resources explicitly
    public void Dispose()
    {
        //Pass true in dispose method to clean managed resources too and say GC to skip finalize 
        // in next line.
        Dispose(true);
        //If dispose is called already then say GC to skip finalize on this instance.
        GC.SuppressFinalize(this);
    }

    ~clsEntityMappingTypeRelationMaster()
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
    #region Public Method Section 
    #endregion
   
    public string CompanyNameSecondary { get; set; }
    public byte DefaultParent { get; set; }
   
    public DataSet getEntityMappingTypeRelationMasterDropdowns()
    {
        SqlParameter[] objSqlParam = new SqlParameter[4];
        objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
        objSqlParam[0].Direction = ParameterDirection.Output;
        objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.SmallInt, 2);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@CompanyId", CompanyId);
        objSqlParam[3] = new SqlParameter("@EntityMappingTypeID", EntityMappingID);
        DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetEntityMappingTypeRelationMasterDropdowns", CommandType.StoredProcedure, objSqlParam);
        Out_Param = Convert.ToInt16(objSqlParam[1].Value);
        if (Out_Param == 1)
            Error = objSqlParam[0].Value.ToString();
        return dsResult;
    }
    public void SaveEntityMappingTypeRelationMaster()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[12];
        objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
        objSqlParam[0].Direction = ParameterDirection.Output;
        objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.SmallInt, 2);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@EntityMappingTypeID", EntityMappingTypeID);
        objSqlParam[3] = new SqlParameter("@PrimaryEntityTypeID", PrimaryEntityTypeID);
        objSqlParam[4] = new SqlParameter("@SecondaryEntityTypeID", SecondaryEntityTypeID);
        objSqlParam[5] = new SqlParameter("@MappingMode", MappingMode);
        objSqlParam[6] = new SqlParameter("@UserID", UserID);
        objSqlParam[7] = new SqlParameter("@Active", Active);
        objSqlParam[8] = new SqlParameter("@Remarks", Remarks);
        objSqlParam[9] = new SqlParameter("@EntityMappingTypeRelationID", EntityMappingTypeRelationID);
        objSqlParam[10] = new SqlParameter("@Condition", Condition);
        objSqlParam[11] = new SqlParameter("@CompanyId", CompanyId);
        SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, "prcSaveEntityMappingTypeRelationMaster", objSqlParam);
        Out_Param = Convert.ToInt16(objSqlParam[1].Value);
        if (Out_Param == 1)
            Error = Convert.ToString(objSqlParam[0].Value);
    }
    public DataTable GetEntityMappingTypeRelationMaste()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[9];
        objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
        objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
        objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
        objSqlParam[2].Direction = ParameterDirection.Output;
        objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[3].Direction = ParameterDirection.Output;
        objSqlParam[4] = new SqlParameter("@EntityMappingTypeID", EntityMappingTypeID);
        objSqlParam[5] = new SqlParameter("@PrimaryEntityType", PrimaryEntityTypeID);
        objSqlParam[6] = new SqlParameter("@SecondaryEntityType", SecondaryEntityTypeID);
        objSqlParam[7] = new SqlParameter("@EntityMappingTypeRelationID", EntityMappingTypeRelationID);
        objSqlParam[8] = new SqlParameter("@CompanyId", CompanyId);
        DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetEntityMappingTypeRelationMaster", CommandType.StoredProcedure, objSqlParam);
        if (dsResult != null && dsResult.Tables.Count > 0)
            dtResult = dsResult.Tables[0];
        TotalRecords = Convert.ToInt32(objSqlParam[2].Value);
        Error = Convert.ToString(objSqlParam[3].Value);

        return dtResult;
    }
    public DataTable Select(short EntityMappingTypeID)
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[3];
        objSqlParam[0] = new SqlParameter("@EntityMappingTypeID", EntityMappingTypeID);
        objSqlParam[1] = new SqlParameter("@EntityMappingTypeRelationID", EntityMappingTypeRelationID);
        objSqlParam[2] = new SqlParameter("@CompanyId", CompanyId);
        DataSet dsResult = SqlHelper.ExecuteDataset(_ConnectionString, CommandType.StoredProcedure, "prc_EntityMappingTypeRelationMaster_Select", objSqlParam);
        if (dsResult != null && dsResult.Tables.Count > 0)
            dtResult = dsResult.Tables[0];
        return dtResult;
    }
    public DataTable SelectByEntityMapping(short EntityMappingTypeID, int PrimaryEntityID)
    {

        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[10];
        objSqlParam[0] = new SqlParameter("@EntityMappingTypeID", EntityMappingTypeID);
        objSqlParam[1] = new SqlParameter("@EntityMappingTypeRelationID", EntityMappingTypeRelationID);
        objSqlParam[2] = new SqlParameter("@PrimaryEntityID", PrimaryEntityID);
        objSqlParam[3] = new SqlParameter("@UserId", UserID);
        objSqlParam[4] = new SqlParameter("@CompanyId", CompanyId);
        DataSet dsResult = SqlHelper.ExecuteDataset(_ConnectionString, CommandType.StoredProcedure, "prc_EntityMapping_Select", objSqlParam);
        if (dsResult != null && dsResult.Tables.Count > 0)
            dtResult = dsResult.Tables[0];
       

        return dtResult;
    }
    public DataTable SelectEntityMappingSearch(short EntityMappingTypeID, Int64 PrimaryEntityID)
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[9];
        objSqlParam[0] = new SqlParameter("@EntityMappingTypeID", EntityMappingTypeID);
        objSqlParam[1] = new SqlParameter("@EntityMappingTypeRelationID", EntityMappingTypeRelationID);
        objSqlParam[2] = new SqlParameter("@SecondaryEntity", SecondaryParty);/*CC08:Added*/
        objSqlParam[3] = new SqlParameter("@PrimaryEntityID", PrimaryEntityID);
        objSqlParam[4] = new SqlParameter("@PageIndex", PageIndex);
        objSqlParam[5] = new SqlParameter("@PageSize", PageSize);
        objSqlParam[6] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
        objSqlParam[6].Direction = ParameterDirection.Output;
        objSqlParam[7] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[7].Direction = ParameterDirection.Output;
        objSqlParam[8] = new SqlParameter("@CompanyId", CompanyId);
        DataSet dsResult = SqlHelper.ExecuteDataset(_ConnectionString, CommandType.StoredProcedure, "prc_EntityMapping_Search", objSqlParam);
        if (dsResult != null && dsResult.Tables.Count > 0)
            dtResult = dsResult.Tables[0];

        TotalRecords = Convert.ToInt32(objSqlParam[6].Value);

        return dtResult;
    }

    public DataTable getParentCategoryNameDropdowns()
    {
        SqlParameter[] objSqlParam = new SqlParameter[4];
        objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
        objSqlParam[0].Direction = ParameterDirection.Output;
        objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.SmallInt, 2);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@CompanyId", CompanyId);
        objSqlParam[3] = new SqlParameter("@UserID", UserID);
        DataTable dsResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetParentCategoryName", CommandType.StoredProcedure, objSqlParam);
        Out_Param = Convert.ToInt16(objSqlParam[1].Value);
        if (Out_Param == 1)
            Error = objSqlParam[0].Value.ToString();
        return dsResult;
    }

    public DataTable getCategoryForBindDropdowns()
    {
        SqlParameter[] objSqlParam = new SqlParameter[4];
        objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
        objSqlParam[0].Direction = ParameterDirection.Output;
        objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.SmallInt, 2);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@CompanyId", CompanyId);
        objSqlParam[3] = new SqlParameter("@UserID", UserID);
        DataTable dsResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetParentCategoryFor", CommandType.StoredProcedure, objSqlParam);
        Out_Param = Convert.ToInt16(objSqlParam[1].Value);
        if (Out_Param == 1)
            Error = objSqlParam[0].Value.ToString();
        return dsResult;
    }
    public void SaveCategoryMaster()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[12];
        objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
        objSqlParam[0].Direction = ParameterDirection.Output;
        objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.SmallInt, 2);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@ParentCategoryId", ParentCategoryId);
        objSqlParam[3] = new SqlParameter("@CategoryForId", CategoryForId);
        objSqlParam[4] = new SqlParameter("@CategoryName", CategoryName);
        objSqlParam[5] = new SqlParameter("@UserID", UserID);
        objSqlParam[6] = new SqlParameter("@Active", Active);
        objSqlParam[7] = new SqlParameter("@CategoryId", CategoryId);
        objSqlParam[8] = new SqlParameter("@Condition", Condition);
        objSqlParam[9] = new SqlParameter("@CompanyId", CompanyId);
        objSqlParam[10] = new SqlParameter("@ParentCategoryName", ParentCategoryName);
        SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, "prcSaveCategoryMaster", objSqlParam);
        Out_Param = Convert.ToInt16(objSqlParam[1].Value);
        if (Out_Param == 1)
            Error = Convert.ToString(objSqlParam[0].Value);
    }
    public DataTable GetCategoryMasterData()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[9];
        objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
        objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
        objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
        objSqlParam[2].Direction = ParameterDirection.Output;
        objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[3].Direction = ParameterDirection.Output;
        objSqlParam[4] = new SqlParameter("@CategoryName", CategoryName);
        objSqlParam[5] = new SqlParameter("@ParentCategoryId", ParentCategoryId);
        objSqlParam[6] = new SqlParameter("@CategoryForId", CategoryForId);
        objSqlParam[7] = new SqlParameter("@CompanyId", CompanyId);
        DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetcategoryMasterData", CommandType.StoredProcedure, objSqlParam);
        if (dsResult != null && dsResult.Tables.Count > 0)
            dtResult = dsResult.Tables[0];
        TotalRecords = Convert.ToInt32(objSqlParam[2].Value);
        Error = Convert.ToString(objSqlParam[3].Value);
        return dtResult;
    }
    public DataTable SelectCategoryMasterBYID()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[6];
        objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
        objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
        objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
        objSqlParam[2].Direction = ParameterDirection.Output;
        objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[3].Direction = ParameterDirection.Output;
        objSqlParam[4] = new SqlParameter("@CategoryId", CategoryId);
        objSqlParam[5] = new SqlParameter("@CompanyId", CompanyId);
        DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcSelectcategoryMasterDataByID", CommandType.StoredProcedure, objSqlParam);
        if (dsResult != null && dsResult.Tables.Count > 0)
            dtResult = dsResult.Tables[0];
        TotalRecords = Convert.ToInt32(objSqlParam[2].Value);
        Error = Convert.ToString(objSqlParam[3].Value);

        return dtResult;
    }
    public DataTable GetParentCategoryMasterData()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[9];
        objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
        objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
        objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
        objSqlParam[2].Direction = ParameterDirection.Output;
        objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[3].Direction = ParameterDirection.Output;
        objSqlParam[4] = new SqlParameter("@ParentCategoryName", ParentCategoryName);
        objSqlParam[5] = new SqlParameter("@CategoryForId", CategoryForId);
        objSqlParam[6] = new SqlParameter("@CompanyId", CompanyId);
        DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetParentCategoryMasterData", CommandType.StoredProcedure, objSqlParam);
        if (dsResult != null && dsResult.Tables.Count > 0)
            dtResult = dsResult.Tables[0];
        TotalRecords = Convert.ToInt32(objSqlParam[2].Value);
        Error = Convert.ToString(objSqlParam[3].Value);
        return dtResult;
    }
    public void SaveParentCategoryMaster()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[12];
        objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
        objSqlParam[0].Direction = ParameterDirection.Output;
        objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.SmallInt, 2);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@CategoryForId", CategoryForId);
        objSqlParam[3] = new SqlParameter("@ParentCategoryName", ParentCategoryName);
        objSqlParam[4] = new SqlParameter("@UserID", UserID);
        objSqlParam[5] = new SqlParameter("@Active", Active);
        objSqlParam[6] = new SqlParameter("@ParentCategoryId", ParentCategoryId);
        objSqlParam[7] = new SqlParameter("@Condition", Condition);
        objSqlParam[8] = new SqlParameter("@CompanyId", CompanyId);
        SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, "prcSaveParentCategoryMaster", objSqlParam);
        Out_Param = Convert.ToInt16(objSqlParam[1].Value);
        if (Out_Param == 1)
            Error = Convert.ToString(objSqlParam[0].Value);
    }
    public DataTable SelectParentCategoryMasterBYID()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[6];
        objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
        objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
        objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
        objSqlParam[2].Direction = ParameterDirection.Output;
        objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[3].Direction = ParameterDirection.Output;
        objSqlParam[4] = new SqlParameter("@ParentCategoryId", ParentCategoryId);
        objSqlParam[5] = new SqlParameter("@CompanyId", CompanyId);
        DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcSelectParentcategoryMasterDataByID", CommandType.StoredProcedure, objSqlParam);
        if (dsResult != null && dsResult.Tables.Count > 0)
            dtResult = dsResult.Tables[0];
        TotalRecords = Convert.ToInt32(objSqlParam[2].Value);
        Error = Convert.ToString(objSqlParam[3].Value);

        return dtResult;
    }
    public DataTable getMenuNameDropdowns()
    {
        SqlParameter[] objSqlParam = new SqlParameter[5];
        objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
        objSqlParam[0].Direction = ParameterDirection.Output;
        objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.SmallInt, 2);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@CompanyId", CompanyId);
        objSqlParam[3] = new SqlParameter("@UserID", UserID);
        objSqlParam[4] = new SqlParameter("@RoleId", RoleId);
        DataTable dsResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetAPPMenuName", CommandType.StoredProcedure, objSqlParam);
        Out_Param = Convert.ToInt16(objSqlParam[1].Value);
        if (Out_Param == 1)
            Error = objSqlParam[0].Value.ToString();
        return dsResult;
    }
    public DataTable GetUserRoleUserMaster()
    {
        try
        {
            SqlParameter[] SqlParam = new SqlParameter[6]; 
            SqlParam[0] = new SqlParameter("@CompanyID", CompanyId);
            DataTable dtUserInfo = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetAPPRoleMenuDetails", CommandType.StoredProcedure, SqlParam);
            return dtUserInfo;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetAppMenuMasterData()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[9];
        objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
        objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
        objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
        objSqlParam[2].Direction = ParameterDirection.Output;
        objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[3].Direction = ParameterDirection.Output;
        objSqlParam[4] = new SqlParameter("@MenuId", MenuId);
        objSqlParam[5] = new SqlParameter("@RoleId", RoleId);
        objSqlParam[6] = new SqlParameter("@Active", Active);
        objSqlParam[7] = new SqlParameter("@CompanyId", CompanyId);
        DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetAPPMenuRoleMasterData", CommandType.StoredProcedure, objSqlParam);
        if (dsResult != null && dsResult.Tables.Count > 0)
            dtResult = dsResult.Tables[0];
        TotalRecords = Convert.ToInt32(objSqlParam[2].Value);
        Error = Convert.ToString(objSqlParam[3].Value);
        return dtResult;
    }
    public void SaveAPPMenuRoleMaster()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[12];
        objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
        objSqlParam[0].Direction = ParameterDirection.Output;
        objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.SmallInt, 2);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@MenuId", MenuId);
        objSqlParam[3] = new SqlParameter("@RoleId", RoleId);
        objSqlParam[4] = new SqlParameter("@MenuTypeId", MenuTypeId);
        objSqlParam[5] = new SqlParameter("@UserID", UserID);
        objSqlParam[6] = new SqlParameter("@Active", Active);
        objSqlParam[7] = new SqlParameter("@DisplayOrder", DisplayOrder);
        objSqlParam[8] = new SqlParameter("@Condition", Condition);
        objSqlParam[9] = new SqlParameter("@CompanyId", CompanyId);
        objSqlParam[10] = new SqlParameter("@AppMenuRoleId", AppMenuRoleId);
        SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, "prcSaveAppMenuRoleMaster", objSqlParam);
        Out_Param = Convert.ToInt16(objSqlParam[1].Value);
        if (Out_Param == 1)
            Error = Convert.ToString(objSqlParam[0].Value);
    }
    public DataTable SelectAPPMenuRoleBYID()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[6];
        objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
        objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
        objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
        objSqlParam[2].Direction = ParameterDirection.Output;
        objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[3].Direction = ParameterDirection.Output;
        objSqlParam[4] = new SqlParameter("@AppMenuRoleId", AppMenuRoleId);
        objSqlParam[5] = new SqlParameter("@CompanyId", CompanyId);
        DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcSelectAPPMenuRoleMasterDataByID", CommandType.StoredProcedure, objSqlParam);
        if (dsResult != null && dsResult.Tables.Count > 0)
            dtResult = dsResult.Tables[0];
        TotalRecords = Convert.ToInt32(objSqlParam[2].Value);
        Error = Convert.ToString(objSqlParam[3].Value);

        return dtResult;
    }
    public DataTable GetPaymentModeMasterData()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[9];
        objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
        objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
        objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
        objSqlParam[2].Direction = ParameterDirection.Output;
        objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[3].Direction = ParameterDirection.Output;
        objSqlParam[4] = new SqlParameter("@PaymentModeName", PaymentModeName);
        objSqlParam[5] = new SqlParameter("@PaymentModeCode", PaymentModeCode);
        objSqlParam[6] = new SqlParameter("@Active", Active);
        objSqlParam[7] = new SqlParameter("@CompanyId", CompanyId);
        objSqlParam[8] = new SqlParameter("@PaymentModeId", PaymentModeId);
        DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetPaymentModeMasterData", CommandType.StoredProcedure, objSqlParam);
        if (dsResult != null && dsResult.Tables.Count > 0)
            dtResult = dsResult.Tables[0];
        TotalRecords = Convert.ToInt32(objSqlParam[2].Value);
        Error = Convert.ToString(objSqlParam[3].Value);
        return dtResult;
    }
    public void SavePaymentModeMaster()
    {
        DataTable dtResult = new DataTable();
        try
        {
            SqlParameter[] objSqlParam = new SqlParameter[12];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.SmallInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@PaymentModeName", PaymentModeName);
            objSqlParam[3] = new SqlParameter("@PaymentModeCode", PaymentModeCode);
            objSqlParam[4] = new SqlParameter("@UserID", UserID);
            objSqlParam[5] = new SqlParameter("@Active", Active);
            objSqlParam[6] = new SqlParameter("@PaymentModeId", PaymentModeId);
            objSqlParam[7] = new SqlParameter("@Condition", Condition);
            objSqlParam[8] = new SqlParameter("@CompanyId", CompanyId);
            SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, "prcSavePaymentModeMaster", objSqlParam);
            Out_Param = Convert.ToInt16(objSqlParam[1].Value);
            if (Out_Param == 1)
                Error = Convert.ToString(objSqlParam[0].Value);
        }
        catch (Exception ex)
        {
            
            throw ex;
        }
        
    }
    public DataTable getCompanyNameDropdowns()
    {
        SqlParameter[] objSqlParam = new SqlParameter[4];
        objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
        objSqlParam[0].Direction = ParameterDirection.Output;
        objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.SmallInt, 2);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@CompanyId", CompanyId);
        objSqlParam[3] = new SqlParameter("@UserID", UserID);
        DataTable dsResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetCompanyName", CommandType.StoredProcedure, objSqlParam);
        Out_Param = Convert.ToInt16(objSqlParam[1].Value);
        if (Out_Param == 1)
            Error = objSqlParam[0].Value.ToString();
        return dsResult;
    }
    public void SaveAPPMenuRoleMasterList()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[12];
        objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
        objSqlParam[0].Direction = ParameterDirection.Output;
        objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.SmallInt, 2);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@MenuId", MenuId);
        objSqlParam[3] = new SqlParameter("@RoleId", RoleId);
        objSqlParam[4] = new SqlParameter("@UserID", UserID);
        objSqlParam[5] = new SqlParameter("@CompanyId", CompanyId);
        objSqlParam[6] = new SqlParameter("@TVP_APPMenuDetail", dtSaveManulist);
        SqlHelper.ExecuteNonQuery(_ConnectionString, CommandType.StoredProcedure, "prcSaveAppMenuRoleMasterList", objSqlParam);
        Out_Param = Convert.ToInt16(objSqlParam[1].Value);
        if (Out_Param == 1)
            Error = Convert.ToString(objSqlParam[0].Value);
    }
   
}

