using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for clsReportQueue
/// </summary>
public class clsReportQueue : IDisposable
{
    //public String ConStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

    #region Private Class Variables
    private int _intEntityMappingID;
    private string _strError;
    private int _intModifiedBy;
    private int _intEntityMappingTypeRelationID;
    private Int32 _intPageIndex;
    private Int32 _intPageSize;
    private Int32 _intTotalRecords;
    private int _intEntityMappingTypeID;

    private Int16 _intApplicableForClaim;
    private Int16 _intApplicableForLedger;
    private string _strCompanyName;
    private string _strCompanyDisplayName;
    private short _shtVerified;
    private short _shtWeeklyOffDay;
    private string _strEntityCode;
    private string _strEntitySAPCode;
    private int? _intGroupEntityID;
    private short _shtApplicationWorkingMode;
    private Int64 _intCreatedBy;
    private int? _intDisplayOrder;
    private string _strRemarks;
    private short _shtServiceLevel; 
    private short _shtActive;
    private Int16 _intDefectiveStockTakenInDOA;

    private Int64 _UserId;
 
    private Int64 _intDownloadRequestId;
    private string _strRequestForReport;
    private string _strRequestedProcedure;
    private DateTime _dtCreatedOn;
   
    private string _strFileName;
    private short _shtProcessStatus;
    private string _strProcessRemarks;
    private DateTime _dtProcessOn;
    private string _SecondaryParty;
 
    private string _SerialNo;
    private string _Remark;
 
    private string _strRemovePermanentVoidXML;
    private DateTime? _DateFrom;
    private DateTime? _DateTo;
    private string _ActionType;
    private int _intSubCategoryID; 
    private int _intCategoryID;  
    private int _VoucherType;

    private int _intStockAdjustmentID, _intStockAdjustmentForID, _intReasonID;
    private string _strstockAdjustmentDetail, _strStockAdjustmentNo;
    private DateTime? _dtStockAdjustmentDate, datestockAdjustFromdate, datestockAdjustTodate;
    private Int16 intApprovalStatus, intInterMediateApprovalStatus, _TransactionStatus;
    private string _strConfigKey;
    private string _straDisposalTo;
    private string _strVehicalNo;
    private int _intCarrierID;
    private string _strCarrierName;
    private string _strCarrierPersonName;
    private string _strCarrierPersonMobile;
    private short _AdjustmentTypeID;
    private short _SelectionMode;
    private short _ApprovalProces;
    private short _ApprovalRejection;
    private string _strMobileNo;
    private Int16 _intSMSTrasNameID;
    private Int16 _intSMSStatus;
    private bool? _blnActive;
    private Int16 _OrderTypeAllowed;
    private Int16 _DOA_Allowed;
    private int _intLoginEntityTypeId;
    private int _intLoginEntityId;
    private Int64 _intProductPartId;
    private short _shtSparesQty;
    private int _modifiedby;
    private string _strEntityTypeModeFieldName;
    private Int16 _shtEntityTypeModeFieldValue;
    private Int64 _lngProductId;
    #endregion

    #region Public Properties
    public Int64 ProductId
    {
        get
        {
            return _lngProductId;
        }
        set
        {
            _lngProductId = value;
        }
    }
    public string EntityTypeModeFieldName1
    {
        get
        {
            return _strEntityTypeModeFieldName;
        }
        set
        {
            _strEntityTypeModeFieldName = value;
        }
    }
    public Int16 EntityTypeModeFieldValue1
    {
        get
        {
            return _shtEntityTypeModeFieldValue;
        }
        set
        {
            _shtEntityTypeModeFieldValue = value;
        }
    }
    public int Modifiedby
    {
        get
        {
            return _modifiedby;
        }
        set
        {
            _modifiedby = value;
        }
    }
    public short SparesQty
    {
        get
        {
            return _shtSparesQty;
        }
        set
        {
            _shtSparesQty = value;
        }
    }
    public Int64 ProductPartId
    {
        get
        {
            return _intProductPartId;
        }
        set
        {
            _intProductPartId = value;
        }
    }
    public Int16 PartBomType { get; set; }
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
    public DataTable dtEntityInformation
    {
        get;
        set;
    }
    public Int16 OrderTypeAllowed
    {
        get
        {
            return _OrderTypeAllowed;
        }
        set
        {
            _OrderTypeAllowed = value;
        }
    }

    public Int16 DOA_Allowed
    {
        get
        {
            return _DOA_Allowed;
        }
        set
        {
            _DOA_Allowed = value;
        }
    }
    public Int16 SMSStatus
    {
        private get
        {
            return _intSMSStatus;
        }
        set
        {
            _intSMSStatus = value;
        }
    }

    public Int16 SMSTransNameID
    {
        private get
        {
            return _intSMSTrasNameID;
        }
        set
        {
            _intSMSTrasNameID = value;
        }
    }
    public string MobileNo
    {
        get
        {
            return _strMobileNo;
        }
        set
        {
            _strMobileNo = value;
        }
    }
    public int createdBy
    {
        get;
        set;
    }
    public bool? ActiveSMS
    {
        get
        {
            return _blnActive;
        }
        set
        {
            _blnActive = value;
        }
    }
    public Int16 StockMode { get; set; }
    public short ApprovalRejection
    {
        get
        {
            return _ApprovalRejection;
        }
        set
        {
            _ApprovalRejection = value;
        }
    }


    public short ApprovalProces
    {
        get
        {
            return _ApprovalProces;
        }
        set
        {
            _ApprovalProces = value;
        }
    }


    public short SelectionMode
    {
        get
        {
            return _SelectionMode;
        }
        set
        {
            _SelectionMode = value;
        }
    }

    public short AdjustmentTypeID
    {
        get
        {
            return _AdjustmentTypeID;
        }
        set
        {
            _AdjustmentTypeID = value;
        }
    }

    public int IntStockAdjustmentForID
    {
        get { return _intStockAdjustmentForID; }
        set { _intStockAdjustmentForID = value; }
    }
    public int ErrorValue
    {
        get;
        set;
    }

    public int IntStockAdjustmentID
    {
        get { return _intStockAdjustmentID; }
        set { _intStockAdjustmentID = value; }
    }
    public string ConfigKey
    {
        get { return _strConfigKey; }
        set { _strConfigKey = value; }
    }
    private Int16 _shtEntityTypeID;
    private EnumUploadPageName ePageName;
    public Int16 ShtEntityTypeID
    {
        get { return _shtEntityTypeID; }
        set { _shtEntityTypeID = value; }
    }
    public EnumUploadPageName ePageNameUpload
    {
        get { return ePageName; }
        set { ePageName = value; }
    }


    public DateTime? DtStockAdjustmentDate
    {
        get { return _dtStockAdjustmentDate; }
        set { _dtStockAdjustmentDate = value; }
    }

    public int IsFromOpeningStock
    {
        get;
        set;
    }
    public string StrError
    {
        get { return _strError; }
        set { _strError = value; }
    }

    public string StrRemarks
    {
        get { return _strRemarks; }
        set { _strRemarks = value; }
    }

    public string StrStockAdjustmentNo
    {
        get { return _strStockAdjustmentNo; }
        set { _strStockAdjustmentNo = value; }
    }

    public string StrstockAdjustmentDetail
    {
        get { return _strstockAdjustmentDetail; }
        set { _strstockAdjustmentDetail = value; }
    }
    public int ReasonID
    {
        get { return _intReasonID; }
        set { _intReasonID = value; }
    }
    public DateTime? stockAdjustFromdate
    {
        get { return datestockAdjustFromdate; }
        set { datestockAdjustFromdate = value; }
    }
    public DateTime? stockAdjustTodate
    {
        get { return datestockAdjustTodate; }
        set { datestockAdjustTodate = value; }
    }
    public Int16 ApprovalStatus
    {
        get { return intApprovalStatus; }
        set { intApprovalStatus = value; }
    }
    public Int16 InterMediateApprovalStatus
    {
        get { return intInterMediateApprovalStatus; }
        set { intInterMediateApprovalStatus = value; }
    }


    public Int16 TransactionStatus
    {
        get
        {
            return _TransactionStatus;
        }
        set
        {
            _TransactionStatus = value;
        }
    }

    public DataTable PartInfo;
    #endregion

    public enum EnumUploadPageName
    {
        StockAdjustment = 0

    }
    public string CarrierPersonMobile
    {
        get { return _strCarrierPersonMobile; }
        set { _strCarrierPersonMobile = value; }
    }

    public string VehicalNo
    {
        get { return _strVehicalNo; }
        set { _strVehicalNo = value; }
    }
    public int CarrierID
    {
        get { return _intCarrierID; }
        set { _intCarrierID = value; }
    }
    public string CarrierName
    {
        get { return _strCarrierName; }
        set { _strCarrierName = value; }
    }
    public string CarrierPersonName
    {
        get { return _strCarrierPersonName; }
        set { _strCarrierPersonName = value; }
    }

    public string DisposalTo
    {
        get
        {
            return _straDisposalTo;
        }
        set
        {
            _straDisposalTo = value;
        }

    }
    public string PartCode
    {
        get;
        set;
    }

    public int PriceGroupID
    {
        get;
        set;

    }
    public short PriceTypeId
    {
        get;
        set;
    }
    public Int32 EntityTypeId
    {
        get;
        set;
    }
    public string SearchDate
    {
        get;
        set;
    }
    public int PriceGroupId
    {
        get;
        set;
    }
    public short PriceTypeID
    {
        get;
        set;
    }
    public int VoucherType
    {
        get
        {
            return _VoucherType;
        }
        set
        {
            _VoucherType = value;
        }
    }
    public int CategoryID
    {
        get
        {
            return _intCategoryID;
        }
        set
        {
            _intCategoryID = value;
        }
    }

    public int SubCategoryID
    {
        get
        {
            return _intSubCategoryID;
        }
        set
        {
            _intSubCategoryID = value;
        }
    }
    public string RemovePermanentVoidXML
    {
        get { return _strRemovePermanentVoidXML; }
        private set { _strRemovePermanentVoidXML = value; }
    }

    public string SerialNo
    {
        get { return _SerialNo; }
        set { _SerialNo = value; }
    }

    public string Remark
    {
        get { return _Remark; }
        set { _Remark = value; }
    }

    public DateTime? DateFrom
    {
        get
        {
            return _DateFrom;
        }
        set
        {
            _DateFrom = value;
        }
    }

    public DateTime? DateTo
    {
        get
        {
            return _DateTo;
        }
        set
        {
            _DateTo = value;
        }
    }

    public string ActionType
    {
        get
        {
            return _ActionType;
        }
        set
        {
            _ActionType = value;
        }
    }
   

    public string SecondaryParty
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
    public Int64 UserId
    {
        get
        {
            return _UserId;
        }
        set
        {
            _UserId = value;
        }
    }
    public Int64 DownloadRequestId
    {
        get
        {
            return _intDownloadRequestId;
        }
        set
        {
            _intDownloadRequestId = value;
        }
    }
    public string RequestForReport
    {
        get
        {
            return _strRequestForReport;
        }
        set
        {
            _strRequestForReport = value;
        }
    }
    public string RequestedProcedure
    {
        get
        {
            return _strRequestedProcedure;
        }
        set
        {
            _strRequestedProcedure = value;
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

    public string FileName
    {
        get
        {
            return _strFileName;
        }
        set
        {
            _strFileName = value;
        }
    }
    public short ProcessStatus
    {
        get
        {
            return _shtProcessStatus;
        }
        set
        {
            _shtProcessStatus = value;
        }
    }

    public string ProcessRemarks
    {
        get
        {
            return _strProcessRemarks;
        }
        set
        {
            _strProcessRemarks = value;
        }
    }
    public DateTime ProcessOn
    {
        get
        {
            return _dtProcessOn;
        }
        set
        {
            _dtProcessOn = value;
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
    public DataTable dtEntityValidation
    {
        get;
        set;
    }
    public short ServiceLevel
    {
        get
        {
            return _shtServiceLevel;
        }
        set
        {
            _shtServiceLevel = value;
        }
    }
    public short DefectiveStockTakenInDOA
    {
        get
        {
            return _intDefectiveStockTakenInDOA;
        }
        set
        {
            _intDefectiveStockTakenInDOA = value;
        }
    }
    public int? DisplayOrder
    {
        get
        {
            return _intDisplayOrder;
        }
        set
        {
            _intDisplayOrder = value;
        }
    }
    public string Remarks
    {
        get
        {
            return _strRemarks;
        }
        set
        {
            _strRemarks = value;
        }
    }
    public Int64 CreatedBy
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
    public int? GroupEntityID
    {
        get
        {
            return _intGroupEntityID;
        }
        set
        {
            _intGroupEntityID = value;
        }
    }
    public short ApplicationWorkingMode
    {
        get
        {
            return _shtApplicationWorkingMode;
        }
        set
        {
            _shtApplicationWorkingMode = value;
        }
    }
    public string CompanyName
    {
        get
        {
            return _strCompanyName;
        }
        set
        {
            _strCompanyName = value;
        }
    }
    public string CompanyDisplayName
    {
        get
        {
            return _strCompanyDisplayName;
        }
        set
        {
            _strCompanyDisplayName = value;
        }
    }
    public short Verified
    {
        get
        {
            return _shtVerified;
        }
        set
        {
            _shtVerified = value;
        }
    }
    public short WeeklyOffDay
    {
        get
        {
            return _shtWeeklyOffDay;
        }
        set
        {
            _shtWeeklyOffDay = value;
        }
    }
    public string EntityCode
    {
        get
        {
            return _strEntityCode;
        }
        set
        {
            _strEntityCode = value;
        }
    }

    public string EntitySAPCode
    {
        get
        {
            return _strEntitySAPCode;
        }
        set
        {
            _strEntitySAPCode = value;
        }
    }
    public short ApplicableForClaim
    {
        get
        {
            return _intApplicableForClaim;
        }
        set
        {
            _intApplicableForClaim = value;
        }
    }
    public short ApplicableForLedger
    {
        get
        {
            return _intApplicableForLedger;
        }
        set
        {
            _intApplicableForLedger = value;
        }
    }
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
    public string CompanyNameSecondary { get; set; }
    public byte DefaultParent { get; set; }
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
    public DateTime? FromDate
    {
        get { return _dtFromDate; }
        set { _dtFromDate = value; }
    }

    public DateTime? ToDate
    {
        get { return _dtToDate; }
        set { _dtToDate = value; }
    }
    public Int32 EntityID
    {
        get { return _intEntityID; }
        set { _intEntityID = value; }
    }

    public Int16 EntityTypeID
    {
        get { return _intEntityTypeID; }
        set { _intEntityTypeID = value; }
    }
    public Int64 ServiceEntityID
    {
        get;
        set;
    }
    public int ClaimGoupMasterID
    {
        get;
        set;
    }
    public Int64 EntityClaimGroupMappingId
    {
        get;
        set;
    }

    private Int16 _shtDisplayBehaviour;
    public Int16 DisplayBehaviour
    {
        get
        {
            return _shtDisplayBehaviour;
        }
        set
        {
            _shtDisplayBehaviour = value;
        }
    }
    #region Private Class Variables
    DateTime? _dtFromDate;
    DateTime? _dtToDate;
    private Int16 _intEntityTypeID = 0;
    private Int32 _intEntityID;





    #endregion
	public clsReportQueue()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public DataTable SelectAll()
    {
        DataTable dtResult = new DataTable();
        SqlParameter[] objSqlParam = new SqlParameter[6];

        objSqlParam[4] = new SqlParameter("@userId", UserId);
        objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
        objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
        objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
        objSqlParam[2].Direction = ParameterDirection.Output;
        objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        objSqlParam[3].Direction = ParameterDirection.Output;
        objSqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        objSqlParam[5].Direction = ParameterDirection.Output;
        DataSet dsResult = DataAccess.DataAccess.Instance.GetDataSetFromDatabase("prcDownloadRequest_Select",CommandType.StoredProcedure, objSqlParam);
        if (dsResult != null && dsResult.Tables.Count > 0)
            dtResult = dsResult.Tables[0];
        TotalRecords = Convert.ToInt32(objSqlParam[2].Value);
        Error = Convert.ToString(objSqlParam[3].Value);

        return dtResult;
    }
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

        ~clsReportQueue()
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