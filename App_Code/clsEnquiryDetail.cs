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
* Created On: 04-Sept-2017 
 * Description: This is a Distributor Query and Admin Query Page Class Page.
* ====================================================================================================
 * Change Log
 * 07-05-2015, Rajnish Kumar , #CC01, EnquiryTypeRoleMapping
 ====================================================================================================
*/
#endregion
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Text;
using System.Xml;

/// <summary>
/// Summary description for clsEnquiryDetail
/// </summary>
public class clsEnquiryDetail : IDisposable
{
    #region Private Class Variables
    private string _strError;
    private string _strEnquiryNumber;
    private DateTime? _FromDate;
    private DateTime? _Todate;
    private Int32 _intTotalRecords;
    private Int32 _intPageIndex;
    private Int32 _intPageSize;
    #endregion

    #region Public Properties
    public int LoginUserId { get; set; }
    public int CategoryID { get; set; }
    public int SubCategoryId { get; set;}
    public string Description { get; set; }
    public string name { get; set; }
    public string contactnumber { get; set; }
    public string Emailid { get; set; }
    public Int64 RetailerRoleId { get; set; }
    public string ImagePath { get; set; }
    public Int16 QueryType { get; set; }
    public string Distributorcode { get; set; }
    public Int64 EnquiryDetailid { get; set; }
    public int SalesChannelId { get; set; }
    public Int16 DownLoadReport { get; set; }
    public Int16 Result { get; set; }
    public DataTable dtDecider { get; set; }
    public string WOFileXML { set; get; }
    private Int64 _ReferenceType_id = 0;
    public Int64 ReferenceType_id
    {
        get
        {
            return _ReferenceType_id;
        }
        set
        {
            _ReferenceType_id = value;
        }
    }
    
    
    private int _ImageLoadType = 0;
   
    public int ImageLoadType
    {
        get
        {
            return _ImageLoadType;
        }
        set
        {
            _ImageLoadType = value;
        }
    }
    public DateTime? FromDate
    {
        get { return _FromDate; }
        set { _FromDate = value; }
    }
    public DateTime? Todate
    {
        get { return _Todate; }
        set { _Todate = value; }
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
    public string EnquiryNumber
    {
        get { return _strEnquiryNumber; }
        set { _strEnquiryNumber = value; }
    }
  /*CC01 start*/  public Int64 GetEnquirydetailid { get; set; }
    public int CategoryTypeId { get; set; }
    private Int64 _shtEnquiryTypeMasterID;
    public string XML_Error { get; set; }
    private Int64 _intEnquiryTypeRoleMappingID;
    private int _shtEntityTypeID;
    #endregion
    public DataTable DtRoleID
    {
        get;
        set;
    }

    public Int64 EnquiryTypeMasterID
    {
        get
        {
            return _shtEnquiryTypeMasterID;
        }
        set
        {
            _shtEnquiryTypeMasterID = value;
        }
    }
    public Int64 EnquiryTypeRoleMappingID
    {
        get
        {
            return _intEnquiryTypeRoleMappingID;
        }
        set
        {
            _intEnquiryTypeRoleMappingID = value;
        }
    }
    public int EntityTypeID
    {
        get { return _shtEntityTypeID; }
        set { _shtEntityTypeID = value; }
    }
    private DateTime _dtValidFrom;
    private DateTime _dtValidTill;
    private DateTime _dtCreatedOn;
    public DateTime ValidFrom
    {
        get
        {
            return _dtValidFrom;
        }
        set
        {
            _dtValidFrom = value;
        }
    }
    public DateTime ValidTill
    {
        get
        {
            return _dtValidTill;
        }
        set
        {
            _dtValidTill = value;
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
    }/*CC01 end*/
    #region Constructors
    public clsEnquiryDetail()
    {

    }
    #endregion
    #region Public Method Section
    /// <summary>
    /// Save records in database.
    /// </summary>
    /// <results>Int16: 0 if success</results> 
    public Int16 Insert()
    {
        Int16 result = 1;
        SqlParameter[] objSqlParam = new SqlParameter[14];
        objSqlParam[0] = new SqlParameter("@LoginUserId", LoginUserId);
        objSqlParam[1] = new SqlParameter("@SubCategory", SubCategoryId);
        objSqlParam[2] = new SqlParameter("@Description", Description);

        objSqlParam[3] = new SqlParameter("@EnquiryCustomerName", name);
        objSqlParam[4] = new SqlParameter("@EnquiryCustomerContact", contactnumber);
       // objSqlParam[5] = new SqlParameter("@ImagePath", ImagePath);
        
        objSqlParam[5] = new SqlParameter("@EnquiryNumber", SqlDbType.NVarChar, 20);
        objSqlParam[5].Direction = ParameterDirection.Output;
        objSqlParam[6] = new SqlParameter("@Out_Param", SqlDbType.TinyInt,2);
        objSqlParam[6].Direction = ParameterDirection.Output;
        objSqlParam[7] = new SqlParameter("@Out_Error", SqlDbType.VarChar,500);
        objSqlParam[7].Direction = ParameterDirection.Output;
        objSqlParam[8] = new SqlParameter("@EnquiryDetailID", EnquiryDetailid);
        objSqlParam[9] = new SqlParameter("@QueryStatus", QueryType);
        objSqlParam[10] = new SqlParameter("@SalesChannelId", SalesChannelId);
        objSqlParam[11] = new SqlParameter("@Emailid", Emailid);
        objSqlParam[12] = new SqlParameter("@SetEnquiryDetailedId", SqlDbType.BigInt);
        objSqlParam[12].Direction = ParameterDirection.Output;
        objSqlParam[13] = new SqlParameter("@RetailerRoleId", RetailerRoleId);
        Int32 IntResultCount = DataAccess.DataAccess.Instance.DBInsertCommand("prcEnquiryDetails_Insert", objSqlParam);

        result = Convert.ToInt16(objSqlParam[6].Value);
        EnquiryNumber = Convert.ToString(objSqlParam[5].Value);
        Error = Convert.ToString(objSqlParam[7].Value);
        if (Error=="")
        GetEnquirydetailid = Convert.ToInt64(objSqlParam[12].Value);


        return result;
    }
    public DataSet SelectCategoryType()
    {
        SqlParameter[] objSqlParam = new SqlParameter[2];
        objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
        objSqlParam[0].Direction = ParameterDirection.Output;
        objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[1].Direction = ParameterDirection.Output;
        DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcEnquiryCategory_Select", CommandType.StoredProcedure, objSqlParam);
       

        Error = Convert.ToString(objSqlParam[1].Value);
        return dsResult;
    }
    public DataSet SelectDistributorCode()
    {
        SqlParameter[] objSqlParam = new SqlParameter[4];
        objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
        objSqlParam[0].Direction = ParameterDirection.Output;
        objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelId);
        objSqlParam[3] = new SqlParameter("@LoginUserId", LoginUserId);
        DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetDistributorCode", CommandType.StoredProcedure, objSqlParam);


        Error = Convert.ToString(objSqlParam[1].Value);
        return dsResult;
    }
    public DataSet SelectSubCategory()
    {
        SqlParameter[] objSqlParam = new SqlParameter[3];
        objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
        objSqlParam[0].Direction = ParameterDirection.Output;
        objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@CategoryTypeID", CategoryTypeId);
        DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcEnquiryTypeMaster_Select", CommandType.StoredProcedure, objSqlParam);
       

        Error = Convert.ToString(objSqlParam[1].Value);
        return dsResult;
    }
    public static DataTable GetEnumByTableName(string FileName, string TableName)
    {
        DataTable dt = new DataTable();
        using (DataSet theDataSet = new DataSet())
        {
            string strPath = HttpContext.Current.Server.MapPath("~/Assets/XML/" + FileName + ".xml");
            theDataSet.ReadXml(strPath);
            dt = theDataSet.Tables[TableName];
            if (dt == null || dt.Rows.Count == 0)
                return null;
        }
        try
        {
            dt = dt.Select("Active = 1").CopyToDataTable();
            return dt;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public DataSet SelectQueryResult()
    {
        
        SqlParameter[] objSqlParam = new SqlParameter[10];
        objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
        objSqlParam[0].Direction = ParameterDirection.Output;
        objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@DateFrom", FromDate);
        objSqlParam[3] = new SqlParameter("@DateTo", Todate);
        objSqlParam[4] = new SqlParameter("@QueryType", QueryType);
        objSqlParam[5] = new SqlParameter("@LoginUserId", LoginUserId);
        objSqlParam[6] = new SqlParameter("@SalesChannelId", SalesChannelId);
        objSqlParam[7] = new SqlParameter("@DistributorCode", Distributorcode);
        objSqlParam[8] = new SqlParameter("@DownLoadReport", DownLoadReport);
        objSqlParam[9] = new SqlParameter("@RetailerRoleId", RetailerRoleId);


        DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcEnquiryDetails_Search", CommandType.StoredProcedure, objSqlParam);
        Result = Convert.ToInt16(objSqlParam[0].Value);
        Error = Convert.ToString(objSqlParam[1].Value);
        
        return dsResult;
    }
    public DataTable GetAllQuery()
    {
        SqlParameter[] objSqlParam = new SqlParameter[5];
        objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
        objSqlParam[0].Direction = ParameterDirection.Output;
        objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@LoginUserId", LoginUserId);
        objSqlParam[3] = new SqlParameter("@SalesChannelId", SalesChannelId);
        objSqlParam[4] = new SqlParameter("@EnquiryDetailID", EnquiryDetailid);

        DataTable dsResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetAllDetails", CommandType.StoredProcedure, objSqlParam);
        Error = Convert.ToString(objSqlParam[1].Value);
        return dsResult;
    }
    public DataTable ViewReplyFillData()
    {
        SqlParameter[] objSqlParam = new SqlParameter[3];
        objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
        objSqlParam[0].Direction = ParameterDirection.Output;
        objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@EnquiryDetailID", EnquiryDetailid);

        DataTable dsResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcFIllAllDetails", CommandType.StoredProcedure, objSqlParam);
        Error = Convert.ToString(objSqlParam[1].Value);
        return dsResult;
    }
    public DataTable GetImagePath()
    {
        SqlParameter[] objSqlParam = new SqlParameter[7];
        objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
        objSqlParam[0].Direction = ParameterDirection.Output;
        objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@LoginUserId", LoginUserId);
        objSqlParam[3] = new SqlParameter("@SalesChannelId", SalesChannelId);
        objSqlParam[4] = new SqlParameter("@FromDate", FromDate);
        objSqlParam[5] = new SqlParameter("@ToDate", Todate);
        objSqlParam[6] = new SqlParameter("@EnquiryStatus", QueryType);

        DataTable dsResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetImagePath", CommandType.StoredProcedure, objSqlParam);
        Error = Convert.ToString(objSqlParam[1].Value);
        return dsResult;
    }
    public DataTable GetRemarksImagePath()
    {
        SqlParameter[] objSqlParam = new SqlParameter[5];
        objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt);
        objSqlParam[0].Direction = ParameterDirection.Output;
        objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[2] = new SqlParameter("@LoginUserId", LoginUserId);
        objSqlParam[3] = new SqlParameter("@SalesChannelId", SalesChannelId);
        objSqlParam[4] = new SqlParameter("@EnquiryDetailID", EnquiryDetailid);

        DataTable dsResult = DataAccess.DataAccess.Instance.GetTableFromDatabase("prcGetImagePathRemarks", CommandType.StoredProcedure, objSqlParam);
        Error = Convert.ToString(objSqlParam[1].Value);
        return dsResult;
    }
    public static bool IsDecimal(string str)
    {
        bool flag = false;
        try
        {
            decimal xyz = Convert.ToDecimal(str);
            flag = true;
        }
        catch
        {
            flag = false;
        }
        return flag;
    }
    public DataSet GetImageLoadType()
    {
        SqlParameter[] objSqlParam = new SqlParameter[1];
        objSqlParam[0] = new SqlParameter("@ImageReferenceType", ImageLoadType);
        DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetImageTypeLoad", CommandType.StoredProcedure, objSqlParam);
        return dsResult;
    }
    public DataSet GetImageTypes(Int16 intDecider)     
    {
        SqlParameter[] objSqlParam = new SqlParameter[4];
        objSqlParam[0] = new SqlParameter("@SalesChannelId", SalesChannelId);
        objSqlParam[1] = new SqlParameter("@WebUserId", LoginUserId);
        objSqlParam[2] = new SqlParameter("@Decider", intDecider);
        objSqlParam[3] = new SqlParameter("@TVPData", dtDecider);
        DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetImageType", CommandType.StoredProcedure, objSqlParam);
        return dsResult;

    }
    public DataSet GetImageTypesV1(Int16 intDecider)
    {
        SqlParameter[] objSqlParam = new SqlParameter[4];
        objSqlParam[0] = new SqlParameter("@SalesChannelId", SalesChannelId);
        objSqlParam[1] = new SqlParameter("@WebUserId", LoginUserId);
        objSqlParam[2] = new SqlParameter("@Decider", intDecider);

        DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcGetImageTypeNew", CommandType.StoredProcedure, objSqlParam);
        return dsResult;

    }
    public int SaveImgSaperateByProcess()
    {
        try
        {
            int result;
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@ReferenceType_id", ReferenceType_id);
            param[1] = new SqlParameter("@webUserId", LoginUserId);
            param[2] = new SqlParameter("@XMLFile", WOFileXML);
            param[3] = new SqlParameter("@SalesChannelId", SalesChannelId);
            param[4] = new SqlParameter("@Out_Param", SqlDbType.NVarChar, 50);
            param[4].Direction = ParameterDirection.Output;
            result= DataAccess.DataAccess.Instance.DBInsertCommand("prcImageUploadSaperateByProcess", param);
            
            return result = Convert.ToInt32(param[4].Value);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    #endregion
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

    ~clsEnquiryDetail()
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
    /*#CC01 start*/ 
   # region EnquiryTypeRoleMapping
    public DataTable SelectAll()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[6];
        objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
        objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
        objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
        objSqlParam[2].Direction = ParameterDirection.Output;
        objSqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[3].Direction = ParameterDirection.Output;
        objSqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[4].Direction = ParameterDirection.Output;
        //DataSet dsResult = DataAccess.DataAccess.ExecuteDataset(DBConnection.ConStr, CommandType.StoredProcedure, "prcEnquiryTypeRoleMapping_Select", objSqlParam);
       DataSet dsResult= DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcEnquiryTypeRoleMapping_Select", CommandType.StoredProcedure, objSqlParam);
        if (dsResult != null && dsResult.Tables.Count > 0)
            dtResult = dsResult.Tables[0];
        TotalRecords = Convert.ToInt32(objSqlParam[2].Value);
        Error = Convert.ToString(objSqlParam[4].Value);

        return dtResult;
    }
    public Int16 ToggleActivation()
    {
        Int16 result = 1;
        SqlParameter[] objSqlParam = new SqlParameter[4];
        objSqlParam[0] = new SqlParameter("@EnquiryTypeRoleMappingID", EnquiryTypeRoleMappingID);
        objSqlParam[1] = new SqlParameter("@modifiedby", LoginUserId);
        objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[2].Direction = ParameterDirection.Output;
        objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[3].Direction = ParameterDirection.Output;
        //SqlHelper.ExecuteNonQuery(DBConnection.ConStr, CommandType.StoredProcedure, "prcEnquiryTypeMapping_TActive", objSqlParam);
        DataAccess.DataAccess.Instance.DBInsertCommand("prcEnquiryTypeMapping_TActive", objSqlParam);
        result = Convert.ToInt16(objSqlParam[2].Value);
        Error = Convert.ToString(objSqlParam[3].Value);

        return result;
    }
    public DataSet SelectEnquiryTypeAndRole()
    {
        SqlParameter[] objSqlParam = new SqlParameter[8];
        //objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
        //objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
        //objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
        //objSqlParam[2].Direction = ParameterDirection.Output;
        objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[1].Direction = ParameterDirection.Output;
        objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[3].Direction = ParameterDirection.Output;
        objSqlParam[4] = new SqlParameter("@CategoryTypeID", EntityTypeID);
        //objSqlParam[6] = new SqlParameter("@IntentType", IntentType);
        //objSqlParam[7] = new SqlParameter("@EnquiryTypeID", EnquiryNature);

        DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcEnquiryTypeMaster_Select", CommandType.StoredProcedure, objSqlParam);
            //SqlHelper.ExecuteDataset(DBConnection.ConStr, CommandType.StoredProcedure, "prcEnquiryTypeMaster_Select", objSqlParam);

        Error = Convert.ToString(objSqlParam[4].Value);
        return dsResult;
    }
    public Int16 InsertRoleSubCategoryMapping()
    {
        XML_Error = "<NewDataSet><Table></Table></NewDataSet>";
        Int16 result = 1;
        SqlParameter[] objSqlParam = new SqlParameter[12];
        objSqlParam[0] = new SqlParameter("@EnquiryTypeMasterID", EnquiryTypeMasterID);
        objSqlParam[1] = new SqlParameter("@tvp_RoleID", SqlDbType.Structured);
        objSqlParam[1].Value = DtRoleID;
        objSqlParam[2] = new SqlParameter("@ValidFrom", ValidFrom);
        objSqlParam[3] = new SqlParameter("@CreatedBy", LoginUserId);
        objSqlParam[4] = new SqlParameter("@XML_Duplicate", SqlDbType.Xml);
        objSqlParam[4].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(XML_Error, XmlNodeType.Document, null));
        objSqlParam[4].Direction = ParameterDirection.Output;
        objSqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[5].Direction = ParameterDirection.Output;
        objSqlParam[6] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[6].Direction = ParameterDirection.Output;
        //SqlHelper.ExecuteNonQuery(DBConnection.ConStr, CommandType.StoredProcedure, "prcEnquiryTypeRoleMapping_Insert", objSqlParam);
        DataAccess.DataAccess.Instance.DBInsertCommand("prcEnquiryTypeRoleMapping_Insert", objSqlParam);
        //DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcEnquiryTypeRoleMapping_Insert", CommandType.StoredProcedure, objSqlParam);
        result = Convert.ToInt16(objSqlParam[5].Value);

        Error = Convert.ToString(objSqlParam[6].Value);

        if (((System.Data.SqlTypes.SqlXml)objSqlParam[4].Value).IsNull != true)
        {
            XML_Error = ((System.Data.SqlTypes.SqlXml)objSqlParam[4].Value).Value;

        }
        else
        {
            XML_Error = null;
        }
        return result;
    }
    public DataTable SelectEnquiryType()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[6];
        objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
        objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
        objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
        objSqlParam[2].Direction = ParameterDirection.Output;
        objSqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[3].Direction = ParameterDirection.Output;
        objSqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[4].Direction = ParameterDirection.Output;
        //DataSet dsResult = DataAccess.DataAccess.ExecuteDataset(DBConnection.ConStr, CommandType.StoredProcedure, "prcEnquiryTypeRoleMapping_Select", objSqlParam);
        DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcEnquiryTypeRoleMapping_Select", CommandType.StoredProcedure, objSqlParam);
        if (dsResult != null && dsResult.Tables.Count > 0)
            dtResult = dsResult.Tables[0];
        TotalRecords = Convert.ToInt32(objSqlParam[2].Value);
        Error = Convert.ToString(objSqlParam[4].Value);

        return dtResult;

    }
        /*#CC01 end*/ 
    #endregion
}