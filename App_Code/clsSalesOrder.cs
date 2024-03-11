#region Copyright and page info
/*============================================================================================================================================
Copyright	: Zed-Axis Technologies, 2011
Author		:
Create date	:
Description	: 
============================================================================================================================================
Change Log:
dd-MMM-yy, Name , #CCxx - Description
29-Mar-2013,Shilpi Sharma, #Ch01: added two parameter and a function to update sales order detail.
27-May-2013,Dhiraj Kumar,  #Ch02: Create property SalesOrderTypeMasterID and add as proc parameter in InsertSalesOrderInfo function.
05-Jun-2013,Dhiraj Kumar,  #Ch03: Change return type of function from Datatable to Dataset.
12-Jun-2013,Dhiraj Kumar,  #Ch04: Create funtion and property for scheme functionality.
26-Jun-2013,Dhiraj Kumar,  #Ch05: Send xml file for error output.
08-Jul-2013,Dhiraj Kumar,  #Ch06: Get detail of batch or serial if fifo logic is applied.
18-Jul-2013,Shilpi Sharma, #Ch07: added check to handle when null value coming.
25-Jul-2013,Dhiraj Kumar,  #Ch08: Get available promotion to manipulate data if multiple promotions available.
12-Aug-2013,Dhiraj Kumar,  #Ch09: Add new property with EntityCurrLegBal and use in function SelectPartInfoForOrder.
03-Oct-2013,Shilpi Sharma, #Ch10: Add new method to generate distributor order.
10-mar-2014,Shilpi Sharma, #Ch11: Add new method  to pick order type.
12-mar-2014,Shilpi Sharma, #Ch12: Add new method to get SaleOrder pick list and to update order allocation.
20-mar-2014,Shilpi Sharma, #Ch13: added new method to get StockMode AND Hadle when "" commas come
21-mar-2014,Shilpi Sharma, #Ch14: added new method to AutoAllocate.
24-mar-2014,Shilpi Sharma, #Ch15: Added 2 parameter as told by mittal sir to allocate send orderfrom and toid.
25-mar-2014,Shilpi Sharma, #Ch16: Added to display salesInvoiceXML in parameter.
03-Apr-2014,Shashikant Singh, #Ch17: Added SoCreaditCheckStatus,OrderTypeId property .
22-Apr-14, Sushil Kumar Singh, #CC18: Added a parameter to Store Status of StockOfEntityDispatchInfo when user mar information fill later info 
23-Apr-14, Shilpi Sharma, #CC19: Change parameter name and their value parameter from string to datetime.
                                 While displaying Part Detail in sales Order Interface added parameter named as "OrderType".
24-Apr-2014,Shilpi Sharma, #Ch20: Added new Output parameter
24-Apr-2014, Sushil Kumar Singh, #Ch21: Added a parameter get InvoiceFromID to pass invoice print on creation of invoice
07-May-2014, Shilpi Sharma,#Ch22: added Parmeter to Pass Toid.
12-May-2014, Shilpi Sharma,#Ch23: Wrong Parameter condition commented an dright one added and added parameter to display out_Param value.
16-May-2014, Shilpi Sharma,#Ch24: DispatchType parameter added to save its value.
04-July-2014,Ajeet Mishra, #Ch25: Active Dynamic control for sale order
05-July-2014,shilpi sharma, #Ch25: added function to save data from ProcessSO Upload interface.
05-July-2014,Sushil Kumar singh, #Ch26: added overload function to save data from ProcessSO Upload interface.
05-July-2014,Sushil Kumar singh, #Ch27: added overload function of InsertInvoiceDetails to Genetate invoice.
10-July-2014,Sushil Kumar Singh, #Ch28: Added Function to Get Invoiced data for Invoice dispatch
10-July-2014,Sushil Kumar Singh, #Ch29: added Vresion 2 of function of InsertDispatchInfoForOrder to Save invoice Dispatch
                                        And get Invoice Ditail data
14-July-2014,Sushil Kumar Singh, #Ch30: added Vresion 2 of function of InvoiceCancellation to Cansel invoice
20-Aug-2014,Shilpi sharma,   #Ch31: In salesOrderType function Parameter increased as pass fromid to display ordertype according to allowed order type to from entity id
01-Sept-14, Ajeet Mishra     #Ch32 - comment Sqlhelper and use sqlcommand 
08-Sep-14 , Shashikant Singh #Ch33 - Added ProductName Proprty for searching purpose.
18-Sep-14 , Shilpi sharma #Ch34 - Added parameter to pass loginEntityid 
30-Jan-15, Shashikant Singh #CC35 - Added mettods for check SO creadit Limit from APi and added some property
02-July-15, Shashikant Singh #CC36 - Added smoe prpery to need bind a drop drop down
03-Jul-2015, Vijay Katiyar, #CC37 - Added loginentityid paramenter in update method
22-Jul-2015, Shashikant Singh, #CC38 - Added some property.
21-Jul-2015, Vijay Katiyar, #CC39 - Added function UploadSalesOrderAllocationDetail() and BindStockMode()
23-Sep-2015, Sumit Maurya, #CC40- New property DispatchStaus created and supplied to function GetInvoicedDispatch() to get invoice details according to Stautus supplied.
21-Dec-2015, Sumit Maurya, #CC41, New Parameter supplied @CallingMode in method PackingSlipCancellation() to cancel .
10-May-2016, Vijay Katiyar, #CC42 - Added function GetAllPartGroupName()  and property partgroupid
21-May-2016, Priya Bhatia, #CC43 - Added function to fetch Dealer Details and property SelectedEntityTypeID.
23-May-2016, Vijay Katiyar #CC44 - Added function to fetch all active product part for download excel  
24-May-2016, Priya Bhatia, #CC45 - Added new Property to pass customer details and added new function to generate SalesOrder, PickList, Dispatch and Auto Receive process. 
 * 06-Jun-2016, Sumit Maurya, #CC46, New properties and method created to for BulkSalesorder creation.
23-jun-2016, vivek kumar, #CC47, added new properties and used that property in SelectPartInfoForOrder(),
24-jun-2016, Shashikant, #CC48, added take invoice amount decimal after 2 palces.
03-nov-2016, Vijay Katiyar #CC49 - Added PriceType property and pass it's value in function SelectPartInfoForOrder()
10-nov-2016, Vijay Katiyar #CC50 - Added PartType property and pass it's value in function SelectPartInfoForOrder()
17-Nov-2017, Vijay Katiyar, #CC51 - increase length of @DispatchNo 14 to 20
26-Jul-2017, Shashikant Singh, #CC52 - To remove time out problem and called same as diconnected mode.
02-Aug-2017, Shashikant Singh, #CC53 - Added ReceiveStockMode for directsalesUpload. 
23-Aug-2017, Sumit Maurya, #CC54, New property OriginalInvNo created and value provided.
07-Nov-2017, Shashikant Singh,#CC55, New property PurposeofMaterial created and value provided and binded purpose of materal drop down.
21-Mar-2018, Shashikant Singh,#CC56, Added New property and methods for update salesinvoice .
--------------------------------------------------------------------------------------------------------------------------------------------
 * 
*/
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;
using ZedAxis.ZedEBS.Enums;

namespace BussinessLogic
{
    public class clsSalesOrder : IDisposable
    {
        #region Page Variables
        Int16 _intOrderType;
        DataTable _dtFromEntities;
        DataTable _dtToEntities;
        private DateTime? _dtSoToDate;
        private DateTime? _dtSoFromDate;
        private string _strXMLError;
        private Int16 _shtSalesOrderTypeMasterID; //#Ch02: Added
        private short _shtType;
        private decimal _decOtherDiscount;
        private decimal _decEntityCurrLegBal;    //#Ch09: Added
        private Int16 _SoCreaditCheckStatus; /*CC17:Added*/
        private Int16 _OrderTypeId; /*CC17:Added*/
        private string _strPickListNumber;/*Ch20:added*/
        private Int16 _intDispatchType;/*Ch24:added*/
        private string _strTableName; /*Ch25:added */
        private Int16 _intPvalue; /*Ch25:added */
        private string _strAttribute1;/*Ch25:added */
        private string _strAttribute2;/*Ch25:added */
        private string _strAttribute3;/*Ch25:added */
        /*#ch32 Added Start*/
        public String strConStr = ConfigurationManager.ConnectionStrings["AppConString"].ConnectionString;
        /*#Ch32 Added End*/
        private string _ProductName; /*#Ch33:Added Start Here*/
        /*#CC35:Added start */
        private Decimal _Amount;
        private Decimal _invoiceAmount;
        private string _TokenNo;
        private Int16 _CallingMode;
        private Int16 _CheckStatus;
        private Int16 _InvoicingStatus;
        private string _EntitySAPCode;
        /*#CC35:Added end*/
        /*#CC36:Added start */
        private string _BusinessEventKeyword;
        private int _EntityTypeDescription = 1;
        private EnumSelectionMode _byteSelectionMode = EnumSelectionMode.Single;
        private string _Keyword;
        private Int16 _IsParent;
        private Int32 _ParentSelectedValue;
        private Int16 _ActiveMode;

        /*#CC36:Added end*/
        /*#CC38:Added start*/
        private DataTable _dtSoDetail;
        private string _UploadedfileName;
        private string _ProcessFileName;
        private Int64 _Salefromid;
        private Int16 _StockModeId;
        private string _ReferenceNo;
        /*#CC38:Added end*/
        private Int16 _intStockMode;/*#CC39:Added */
        private int _intPartGroupId;/*#CC42:Added */
        private int _SelectedEntityTypeID; /* #CC43 Added */
        private DataTable _dtCustDetail;  /* #CC45 Added */
        private string _strReceiptNo;  /* #CC45 Added */
        private int _intLedgermaintainedbysystem; /* #CC47 Added */
        private Int16 _intPriceType;/*#CC49 Added*/
        private Int16 _intPartType;/*#CC50 Added*/
        private Int16 _PurposeofMaterial;/*#CC55 Added */
        /*#CC56:Added Start*/
        private DataTable _dtEditSalesInvoice;
        private short _intOutParam;
        private short _intDiscountType=-1;
        /*#CC56:Added End*/


        #endregion

        #region Properties

        /*#CC56:Added Start*/

        public short DiscountType
        {
            get { return _intDiscountType; }
            set { _intDiscountType = value; }
        }

        public short OutParam
        {
            get { return _intOutParam; }
            set { _intOutParam = value; }
        }

        public string OrignalfileName
        {
            get;
            set;
        }

        public DataTable EditSalesInvoice
        {
            get
            {
                return _dtEditSalesInvoice;
            }
            set
            {
                _dtEditSalesInvoice = value;
            }
        }

        /*#CC56:Added End*/

        /* #CC55 Added (start) */
        public Int16 PurposeofMaterial
        {
            get
            {
                return _PurposeofMaterial;
            }
            set
            {
                _PurposeofMaterial = value;
            }
        }
        /* #CC55 Added (end) */


        /* #CC47 Added (start) */
        public int Ledgermaintainedbysystem
        {
            get
            {
                return _intLedgermaintainedbysystem;
            }
            set
            {
                _intLedgermaintainedbysystem = value;
            }
        }
        /* #CC45 Added (end) */


        /*#CC42:Added start */
        public int PartGroupId
        {
            get
            {
                return _intPartGroupId;
            }
            set
            {
                _intPartGroupId = value;
            }
        }
        /*#CC42:Added end */

        /*#CC39:Added start here*/
        public Int16 StockMode
        {
            get
            {
                return _intStockMode;
            }
            set
            {
                _intStockMode = value;
            }
        }
        /*#CC39:Added end here*/
        /*#CC38:Added start here*/
        public DataTable dtSoDetail
        {
            get
            {
                return _dtSoDetail;
            }
            set
            {
                _dtSoDetail = value;
            }
        }
        public string ProcessFileName
        {
            get
            {
                return _ProcessFileName;
            }
            set
            {
                _ProcessFileName = value;
            }
        }
        public string UploadedfileName
        {
            get
            {
                return _UploadedfileName;
            }
            set
            {
                _UploadedfileName = value;
            }
        }
        public Int64 Salefromid
        {
            get
            {
                return _Salefromid;
            }
            set
            {
                _Salefromid = value;
            }
        }
        public Int16 StockModeId
        {
            get
            {
                return _StockModeId;
            }
            set
            {
                _StockModeId = value;
            }
        }
        public string ReferenceNo
        {
            get
            {
                return _ReferenceNo;
            }
            set
            {
                _ReferenceNo = value;
            }
        }

        /*#CC38:Added start end*/
        /*#CC36:Added start here*/
        public String BusinessEventKeyword
        {
            get
            {
                return _BusinessEventKeyword;
            }
            set
            {
                _BusinessEventKeyword = value;
            }
        }
        public int EntityTypeDescription
        {
            get
            {
                return _EntityTypeDescription;
            }
            set
            {
                _EntityTypeDescription = value;
            }
        }

        public String Keyword
        {
            get
            {
                return _Keyword;
            }
            set
            {
                _Keyword = value;
            }
        }
        public EnumSelectionMode SelectionMode
        {
            get
            {
                return _byteSelectionMode;
            }
            set
            {
                _byteSelectionMode = value;

            }
        }
        public Int16 IsParent
        {
            get
            {
                return _IsParent;
            }
            set
            {
                _IsParent = value;
            }
        }
        public Int32 ParentSelectedValue
        {
            get
            {
                return _ParentSelectedValue;
            }
            set
            {
                _ParentSelectedValue = value;
            }
        }
        public Int16 ActiveMode
        {
            get
            {
                return _ActiveMode;
            }
            set
            {
                _ActiveMode = value;
            }
        }

        /*#CC36:Added end here*/

        /*#CC35:Added start here*/
        public String EntitySAPCode
        {
            get
            {
                return _EntitySAPCode;
            }
            set
            {
                _EntitySAPCode = value;
            }
        }
        public Decimal Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                _Amount = value;
            }
        }

        public Decimal invoiceAmount
        {
            get
            {
                return _invoiceAmount;
            }
            set
            {
                _invoiceAmount = value;
            }
        }
        public String TokenNo
        {
            get
            {
                return _TokenNo;
            }
            set
            {
                _TokenNo = value;
            }
        }
        public Int16 CallingMode
        {
            get
            {
                return _CallingMode;
            }
            set
            {
                _CallingMode = value;
            }
        }
        public Int16 CheckStatus
        {
            get
            {
                return _CheckStatus;
            }
            set
            {
                _CheckStatus = value;
            }
        }
        public Int16 InvoicingStatus
        {
            get
            {
                return _InvoicingStatus;
            }
            set
            {
                _InvoicingStatus = value;
            }
        }

        /*#CC35:Added end here*/
        /*Ch25:added */

        /*#Ch33:Added Start Here*/
        public string ProductName
        {
            get
            {
                return _ProductName;
            }
            set
            {
                _ProductName = value;
            }
        }
        /*#Ch33:Added Start End*/
        public string TableName
        {
            get
            {
                return _strTableName;
            }
            set
            {
                _strTableName = value;
            }
        }
        public string Attribute1
        {
            get
            {
                return _strAttribute1;
            }
            set
            {
                _strAttribute1 = value;
            }
        }
        public string Attribute2
        {
            get
            {
                return _strAttribute2;
            }
            set
            {
                _strAttribute2 = value;
            }
        }
        public string Attribute3
        {
            get
            {
                return _strAttribute3;
            }
            set
            {
                _strAttribute3 = value;
            }
        }
        public Int16 Pvalue
        {
            get { return _intPvalue; }
            set { _intPvalue = value; }
        }

        /*Start - CC17:Added*/

        public Int16 SoCreaditCheckStatus
        {
            get { return _SoCreaditCheckStatus; }
            set { _SoCreaditCheckStatus = value; }
        }

        public Int16 OrderTypeId
        {
            get { return _OrderTypeId; }
            set { _OrderTypeId = value; }
        }

        /*CC17:End*/
        public Int16 OrderType
        {
            get { return _intOrderType; }
            set { _intOrderType = value; }
        }

        public DataTable FromEntities
        {
            get { return _dtFromEntities; }
            set { _dtFromEntities = value; }
        }

        public DataTable ToEntities
        {
            get { return _dtToEntities; }
            set { _dtToEntities = value; }
        }

        public int FromID
        {
            get;
            set;
        }

        public int ToID
        {
            get;
            set;
        }

        public DateTime? SoFromDate
        {
            get { return _dtSoFromDate; }
            set { _dtSoFromDate = value; }
        }

        public DateTime? SoToDate
        {
            get { return _dtSoToDate; }
            set { _dtSoToDate = value; }
        }

        public string ToDate
        {
            get;
            set;
        }

        public string FromDate
        {
            get;
            set;
        }

        public Int32 OrderId
        {
            get;
            set;
        }

        public string Orderdate
        {
            get;
            set;
        }

        public string PONumber
        {
            get;
            set;
        }

        public string Remarks
        {
            get;
            set;
        }

        public string PartName
        {
            get;
            set;
        }

        public string PartCode
        {
            get;
            set;
        }

        public string Error
        {
            get;
            set;
        }

        /*#CC01: added (start)*/
        public Int16 Result
        {
            get;
            set;
        }
        /*#CC01: added (End)*/

        public Int32 PageIndex
        {
            get;
            set;
        }

        public Int32 SuggPageIndex
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

        public Int32 TotalRecordsSugg
        {
            get;
            set;
        }

        public Int16 OrderAllocationStatus
        {
            get;
            set;
        }

        public string OrderNumber
        {
            get;
            set;
        }

        public int CreatedBy
        {
            get;
            set;
        }

        public int one
        {
            get;
            set;
        }

        public String DocumentType
        {
            get;
            set;
        }

        public string CancelRemarks
        {
            get;
            set;
        }

        public int TransferTo
        {
            get;
            set;
        }

        public byte IsTransfer
        {
            get;
            set;
        }
        /*#Ch01 added*/
        public byte Status
        {
            get;
            set;
        }

        public string PackingSlipNumber
        {
            get;
            set;
        }

        public string InvoiceNumber
        {
            get;
            set;
        }

        public int AllocationId
        {
            get;
            set;
        }

        public int EntityId
        {
            get;
            set;
        }
        public int OrderFromId
        {
            get;
            set;
        }
        public int OrderFromEntitytypeId
        {
            get;
            set;
        }
        public int OrderToId
        {
            get;
            set;
        }
        public int OrderToEntityTypeId
        {
            get;
            set;
        }
        public int IsSerial
        {
            get;
            set;
        }

        public int TransactionId
        {
            get;
            set;
        }

        public int CancelStatus
        {
            get;
            set;
        }

        public string strSalesInvoiceXML
        {
            get;
            set;
        }

        public int DispatchMode
        {
            get;
            set;
        }

        public DateTime? DocketDate
        {
            get;
            set;
        }

        public int TransporterId
        {
            set;
            get;
        }

        public string TransporterOther
        {
            get;
            set;

        }
        /*#Ch01 added*/
        public string Reason
        {
            get;
            set;
        }

        public string VehicleNo
        {
            get;
            set;
        }

        public string TransporterMobileName
        {
            get;
            set;
        }

        public string TransportPersonName
        {
            get;
            set;
        }

        public string DocketNumber
        {
            get;
            set;
        }

        public string CourierCompanyName
        {
            get;
            set;
        }

        public string DispatchNumber
        {
            get;
            set;
        }

        public int IntError
        {
            get;
            set;
        }

        public Int16 IsApplyCancel
        {
            get;
            set;
        }

        public Int32 Value
        {
            get;
            set;
        }

        public int FrieghtStatus
        {
            get;
            set;
        }

        public string BookingDetails
        {
            get;
            set;
        }

        public string ExciseRegNo
        {
            get;
            set;
        }

        public string cargoToT
        {
            get;
            set;
        }
        public DataTable DtGetApproveSo
        {
            get;
            set;
        }
        public string XMLError
        {
            get { return _strXMLError; }
            set { _strXMLError = value; }
        }
        /*(Start:#Ch02 - Added)*/
        public Int16 SalesOrderTypeMasterID
        {
            get { return _shtSalesOrderTypeMasterID; }
            set { _shtSalesOrderTypeMasterID = value; }
        }
        /*(End:#Ch02 - Added)*/

        /*(Start:#Ch04 - Added)*/
        public DataTable DtPurschasedParts
        {
            get;
            set;
        }
        /*(End:#Ch04 - Added)*/

        public short Type
        {
            get { return _shtType; }
            set { _shtType = value; }
        }
        public int LoggedInEntityID
        {
            get;
            set;
        }

        public int LoggedInEntityTypeID
        {
            get;
            set;
        }
        public int UserDetailId
        {
            get;
            set;
        }

        public DataTable _dtSelectedPromotions
        {
            get;
            set;
        }
        public decimal OtherDiscount
        {
            get { return _decOtherDiscount; }
            set { _decOtherDiscount = value; }
        }

        //(Start:#Ch09 - Added)
        public decimal EntityCurrLegBal
        {
            get { return _decEntityCurrLegBal; }
            set { _decEntityCurrLegBal = value; }
        }
        //(End:#Ch09 - Added)
        /*Ch20:added*/
        public string SalesOrderPickListNumber
        {
            get
            {
                return _strPickListNumber;
            }
            set
            {
                _strPickListNumber = value;
            }
        }
        /*Ch20:added */
        /*Ch24:added start*/
        public Int16 DispatchType
        {
            get { return _intDispatchType; }
            set { _intDispatchType = value; }
        }
        /*Ch24:added end*/
        /* #CC40 Add Start */
        public int DispatchStatus
        {
            get;
            set;
        }
        /* #CC40 Add End */

        /* #CC43 Added Start */
        public int SelectedEntityTypeID
        {
            get { return _SelectedEntityTypeID; }
            set { _SelectedEntityTypeID = value; }
        }
        /* #CC43 Added End */

        /* #CC45 Added Start */
        public DataTable dtCustDetail
        {
            get
            {
                return _dtCustDetail;
            }
            set
            {
                _dtCustDetail = value;
            }
        }

        public string ReceiptNo
        {
            get
            {
                return _strReceiptNo;
            }
            set
            {
                _strReceiptNo = value;
            }
        }

        /* #CC45 Added Start */

        /* #CC46 Add Start */
        public Int64 GroupEntityID
        {
            get;
            set;
        }
        public string SessionId
        {
            get;
            set;
        }

        /* #CC46 Add End */

        /* #CC49 Added (start) */
        public Int16 PriceType
        {
            get
            {
                return _intPriceType;
            }
            set
            {
                _intPriceType = value;
            }
        }
        /* #CC49 Added (end) */

        /* #CC50 Added (start) */
        public Int16 PartType
        {
            get
            {
                return _intPartType;
            }
            set
            {
                _intPartType = value;
            }
        }
        /* #CC50 Added (end) */

        /* #CC54 Add Start */
        public string OrignalInvNo
        {
            get;
            set;
        }

        public Int16 BaseEntityTypeId
        {
            get;
            set;
        }
        public Int16 ToBaseEntityTypeId
        {
            get;
            set;
        }
        /* #CC54 Add End */
        #endregion

        # region Functions

        public DataSet SelectPartInfoForOrder()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[22];/*CC19:increase*/ /*#CC42 changed 15 to 16*//*#CC47 :changes 16 to 17*//*CC49:increase 17 to 18 */ /*#CC50:increased 18 to 19*/ /* #CC54  length increated from 19-20*/
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@toentityid", ToID);
            objSqlParam[2] = new SqlParameter("@fromentityid", FromID);
            objSqlParam[3] = new SqlParameter("@partname", PartName);
            objSqlParam[4] = new SqlParameter("@partcode", PartCode);
            objSqlParam[5] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[6] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[7] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[7].Direction = ParameterDirection.Output;
            objSqlParam[8] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[8].Direction = ParameterDirection.Output;
            objSqlParam[9] = new SqlParameter("@documenttype", DocumentType);
            objSqlParam[10] = new SqlParameter("@TotalRecordSugg", SqlDbType.BigInt, 8);
            objSqlParam[10].Direction = ParameterDirection.Output;
            objSqlParam[11] = new SqlParameter("@SuggPageIndex", SuggPageIndex);
            //(Start:#Ch09 - Added)
            objSqlParam[12] = new SqlParameter("@EntityLedgerCurrBal", SqlDbType.Money, 20);
            objSqlParam[12].Direction = ParameterDirection.Output;
            //(End:#Ch09 - Added)
            objSqlParam[13] = new SqlParameter("@OrderType", OrderType);/*CC19:increase*/
            objSqlParam[14] = new SqlParameter("@Productname", ProductName);/*#Ch33:Added*/
            objSqlParam[15] = new SqlParameter("@PartGroupID", PartGroupId);/*#Ch33:Added*/
            /*Start:#CC47 - Added(start))*/
            objSqlParam[16] = new SqlParameter("@LedgerMaintainedBySystem", SqlDbType.TinyInt, 20);
            objSqlParam[16].Direction = ParameterDirection.Output;
            /*Start:#CC47 - Added(end))*/
            objSqlParam[17] = new SqlParameter("@PriceType", PriceType);/*CC49:Added*/
            objSqlParam[18] = new SqlParameter("@PartType", PartType);/*#CC50:Added*/
            objSqlParam[19] = new SqlParameter("@PurchasedInvoiceNumber", OrignalInvNo);/* #CC54 Added */
            objSqlParam[20] = new SqlParameter("@ToEntityTypeId", OrderToEntityTypeId);/* #CC54 Added */
            objSqlParam[21] = new SqlParameter("@FromEntityTypeId", OrderFromEntitytypeId);/* #CC54 Added */
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcPartInformationForOrder_SelectList", objSqlParam);
            //if (dsResult != null && dsResult.Tables.Count > 0)
            //    dtResult = dsResult.Tables[0];
            if (Convert.ToInt32(objSqlParam[7].Value) == 8)
            {
                IntError = Convert.ToInt32(objSqlParam[7].Value);

            }
            else
            {
                TotalRecords = Convert.ToInt32(objSqlParam[8].Value);
                TotalRecordsSugg = Convert.ToInt32(objSqlParam[10].Value);
                if (objSqlParam[7].Value.ToString() != "")
                {
                    IntError = Convert.ToInt32(objSqlParam[7].Value);
                }
                Error = Convert.ToString(objSqlParam[0].Value);
                EntityCurrLegBal = Convert.ToDecimal(objSqlParam[12].Value);    //#Ch09 - Added
                Ledgermaintainedbysystem = Convert.ToInt16(objSqlParam[16].Value);//#CC47-Added
            }
            return dsResult;

        }

        public void InsertSalesOrderInfo(DataTable Dt)
        {
            try
            {
                Result = 1;
                SqlParameter[] objSqlParam = new SqlParameter[27]; /* #CC54 length increased from 23-24*/ /* #CC55 length increased from 24-25*/
                objSqlParam[0] = new SqlParameter("@tvpSoOrder", SqlDbType.Structured);
                objSqlParam[0].Value = Dt;
                objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                objSqlParam[2].Direction = ParameterDirection.Output;
                objSqlParam[3] = new SqlParameter("@orderdate", Convert.ToDateTime(Orderdate));
                objSqlParam[4] = new SqlParameter("@fromid", FromID);
                objSqlParam[5] = new SqlParameter("@toid", ToID);
                objSqlParam[6] = new SqlParameter("@PONumber", PONumber);
                objSqlParam[7] = new SqlParameter("@remarks", Remarks);
                objSqlParam[8] = new SqlParameter("@createdby", CreatedBy);
                objSqlParam[9] = new SqlParameter("@ordernumber", SqlDbType.VarChar, 500);
                objSqlParam[9].Direction = ParameterDirection.Output;
                objSqlParam[10] = new SqlParameter("@documenttype", DocumentType);
                objSqlParam[11] = new SqlParameter("@istransfer", IsTransfer);
                objSqlParam[12] = new SqlParameter("@StockUpdateError", SqlDbType.Xml);
                objSqlParam[12].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(strSalesInvoiceXML, XmlNodeType.Document, null));
                objSqlParam[12].Direction = ParameterDirection.Output;
                objSqlParam[13] = new SqlParameter("@orderidout", SqlDbType.Int, 2);
                objSqlParam[13].Direction = ParameterDirection.Output;
                objSqlParam[14] = new SqlParameter("@frieghtstatus", FrieghtStatus);
                objSqlParam[15] = new SqlParameter("@bookingDetails", BookingDetails);
                objSqlParam[16] = new SqlParameter("@exciseregNo", ExciseRegNo);
                objSqlParam[17] = new SqlParameter("@cargoTot", cargoToT);
                objSqlParam[18] = new SqlParameter("@SalesOrderTypeMasterID", SalesOrderTypeMasterID);
                objSqlParam[19] = new SqlParameter("@OtherDiscount", OtherDiscount);
                objSqlParam[20] = new SqlParameter("@Attribute1", Attribute1);
                objSqlParam[21] = new SqlParameter("@Attribute2", Attribute2);
                objSqlParam[22] = new SqlParameter("@Attribute3", Attribute3);
                objSqlParam[23] = new SqlParameter("@OrignalInvNo", OrignalInvNo); /* #CC54 Added */
                objSqlParam[24] = new SqlParameter("@SalesOrderPurposeMasterID", PurposeofMaterial); /* #CC55 Added */
                objSqlParam[25] = new SqlParameter("@BaseEntityTypeId", BaseEntityTypeId);
                objSqlParam[26] = new SqlParameter("@ToBaseEntityTypeId", ToBaseEntityTypeId);
                SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcSalesOrder_Insert", objSqlParam);
                /*#CC01: Changed (start)*/
                Error = Convert.ToString(objSqlParam[1].Value);
                //if (Convert.ToString(objSqlParam[1].Value) != "" && Convert.ToInt32(objSqlParam[2].Value) > 0)
                //{
                //    return;
                //}
                /*#CC01: added (start)*/
                Result = Convert.ToInt16(objSqlParam[2].Value);
                /*#CC01: added (End)*/
                OrderNumber = Convert.ToString(objSqlParam[9].Value);
                if (((System.Data.SqlTypes.SqlXml)objSqlParam[12].Value).IsNull != true)
                {
                    strSalesInvoiceXML = ((System.Data.SqlTypes.SqlXml)objSqlParam[12].Value).Value;

                }
                else
                {
                    strSalesInvoiceXML = null;
                    if (Convert.ToString(objSqlParam[13].Value) != null && Convert.ToString(objSqlParam[13].Value) != "")/* #Ch07: added*/
                    {
                        OrderId = Convert.ToInt32(objSqlParam[13].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /*Inserting Van Sales*/
        public void InsertVanSalesInfo(DataTable Dt)
        {
            try
            {
                Result = 1;
                SqlParameter[] objSqlParam = new SqlParameter[20];
                objSqlParam[0] = new SqlParameter("@tvpSoOrderVanSales", SqlDbType.Structured);
                objSqlParam[0].Value = Dt;
                objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                objSqlParam[2].Direction = ParameterDirection.Output;
                objSqlParam[3] = new SqlParameter("@orderdate", Convert.ToDateTime(Orderdate));
                objSqlParam[4] = new SqlParameter("@fromid", FromID);
                objSqlParam[5] = new SqlParameter("@toid", ToID);
                objSqlParam[6] = new SqlParameter("@PONumber", PONumber);
                objSqlParam[7] = new SqlParameter("@remarks", Remarks);
                objSqlParam[8] = new SqlParameter("@createdby", CreatedBy);
                objSqlParam[9] = new SqlParameter("@ordernumber", SqlDbType.VarChar, 500);
                objSqlParam[9].Direction = ParameterDirection.Output;
                objSqlParam[10] = new SqlParameter("@documenttype", DocumentType);
                objSqlParam[11] = new SqlParameter("@istransfer", IsTransfer);
                objSqlParam[12] = new SqlParameter("@StockUpdateError", SqlDbType.Xml);
                objSqlParam[12].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(strSalesInvoiceXML, XmlNodeType.Document, null));
                objSqlParam[12].Direction = ParameterDirection.Output;
                objSqlParam[13] = new SqlParameter("@orderidout", SqlDbType.Int, 2);
                objSqlParam[13].Direction = ParameterDirection.Output;
                objSqlParam[14] = new SqlParameter("@frieghtstatus", FrieghtStatus);
                objSqlParam[15] = new SqlParameter("@bookingDetails", BookingDetails);
                objSqlParam[16] = new SqlParameter("@exciseregNo", ExciseRegNo);
                objSqlParam[17] = new SqlParameter("@cargoTot", cargoToT);
                objSqlParam[18] = new SqlParameter("@SalesOrderTypeMasterID", SalesOrderTypeMasterID);
                objSqlParam[19] = new SqlParameter("@UserDetailID", UserDetailId);
                SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcInsertVanSales", objSqlParam);
                Error = Convert.ToString(objSqlParam[1].Value);
                Result = Convert.ToInt16(objSqlParam[2].Value);
                OrderNumber = Convert.ToString(objSqlParam[9].Value);
                if (((System.Data.SqlTypes.SqlXml)objSqlParam[12].Value).IsNull != true)
                {
                    strSalesInvoiceXML = ((System.Data.SqlTypes.SqlXml)objSqlParam[12].Value).Value;

                }
                else
                {
                    strSalesInvoiceXML = null;
                    if (Convert.ToString(objSqlParam[13].Value) != null && Convert.ToString(objSqlParam[13].Value) != "")/* #Ch07: added*/
                    {
                        OrderId = Convert.ToInt32(objSqlParam[13].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*#Ch10 :added Inserting Distributor (start)*/
        public void InsertDistributorInfo(DataTable Dt)
        {
            try
            {
                Result = 1;
                SqlParameter[] objSqlParam = new SqlParameter[20];
                objSqlParam[0] = new SqlParameter("@tvpSoOrderVanSales", SqlDbType.Structured);
                objSqlParam[0].Value = Dt;
                objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                objSqlParam[2].Direction = ParameterDirection.Output;
                objSqlParam[3] = new SqlParameter("@orderdate", Convert.ToDateTime(Orderdate));
                objSqlParam[4] = new SqlParameter("@fromid", FromID);
                objSqlParam[5] = new SqlParameter("@toid", ToID);
                objSqlParam[6] = new SqlParameter("@PONumber", PONumber);
                objSqlParam[7] = new SqlParameter("@remarks", Remarks);
                objSqlParam[8] = new SqlParameter("@createdby", CreatedBy);
                objSqlParam[9] = new SqlParameter("@ordernumber", SqlDbType.VarChar, 500);
                objSqlParam[9].Direction = ParameterDirection.Output;
                objSqlParam[10] = new SqlParameter("@documenttype", DocumentType);
                objSqlParam[11] = new SqlParameter("@istransfer", IsTransfer);
                objSqlParam[12] = new SqlParameter("@StockUpdateError", SqlDbType.Xml);
                objSqlParam[12].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(strSalesInvoiceXML, XmlNodeType.Document, null));
                objSqlParam[12].Direction = ParameterDirection.Output;
                objSqlParam[13] = new SqlParameter("@orderidout", SqlDbType.Int, 2);
                objSqlParam[13].Direction = ParameterDirection.Output;
                objSqlParam[14] = new SqlParameter("@frieghtstatus", FrieghtStatus);
                objSqlParam[15] = new SqlParameter("@bookingDetails", BookingDetails);
                objSqlParam[16] = new SqlParameter("@exciseregNo", ExciseRegNo);
                objSqlParam[17] = new SqlParameter("@cargoTot", cargoToT);
                objSqlParam[18] = new SqlParameter("@SalesOrderTypeMasterID", SalesOrderTypeMasterID);
                objSqlParam[19] = new SqlParameter("@UserDetailID", UserDetailId);
                SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcInsertDistributorSales", objSqlParam);
                Error = Convert.ToString(objSqlParam[1].Value);
                Result = Convert.ToInt16(objSqlParam[2].Value);
                OrderNumber = Convert.ToString(objSqlParam[9].Value);
                if (((System.Data.SqlTypes.SqlXml)objSqlParam[12].Value).IsNull != true)
                {
                    strSalesInvoiceXML = ((System.Data.SqlTypes.SqlXml)objSqlParam[12].Value).Value;

                }
                else
                {
                    strSalesInvoiceXML = null;
                    if (Convert.ToString(objSqlParam[13].Value) != null && Convert.ToString(objSqlParam[13].Value) != "")/* #Ch07: added*/
                    {
                        OrderId = Convert.ToInt32(objSqlParam[13].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*#Ch10 :added Inserting Distributor (end)*/


        public DataTable GettvpTableOrder()
        {
            DataTable dtPrimaryOrder = new DataTable();
            dtPrimaryOrder.Columns.Add("PartId");
            dtPrimaryOrder.Columns.Add("Quantity");
            dtPrimaryOrder.Columns.Add("GrossAmount");
            dtPrimaryOrder.Columns.Add("NetAmount");
            return dtPrimaryOrder;
        }

        public int EntityTypeId
        {
            get;
            set;
        }

        public DataSet SelectOrderDetailsForManualAllocation()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[2];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@orderid", OrderId);
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcSalesOrderDetails", objSqlParam);
            //if (dsResult != null && dsResult.Tables.Count > 0)
            //    dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[0].Value);


            return dsResult;
        }

        //public DataTable SelectSerializedPartsDetailsForInvoice() //#Ch06: Commented
        public DataSet SelectSerializedPartsDetailsForInvoice()   //#Ch06: Added
        {
            //DataTable dtResult = new DataTable(); //#Ch06: Commented
            SqlParameter[] objSqlParam = new SqlParameter[2];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@allocationId", AllocationId);
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcAllocationDetailForSerializedParts", objSqlParam);

            /*(Start:#Ch06 - Commented)*/
            /*if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];*/
            /*(End:#Ch06 - Commented)*/
            Error = Convert.ToString(objSqlParam[0].Value);

            //return dtResult;  //#Ch06: Commented
            return dsResult;  //#Ch06: Added
        }
        /*#Ch26 Added Start*/
        public DataSet SelectSerializedPartsDetailsForInvoice(DataTable dtAllocationId)
        {
            SqlParameter[] objSqlParam = new SqlParameter[2];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@tvpAllocationId", dtAllocationId);
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcAllocationDetailForSerializedPartsV2", objSqlParam);
            Error = Convert.ToString(objSqlParam[0].Value);
            return dsResult;
        }
        /*#Ch26 Added End*/
        public DataTable ExecuteprocedureforAutomaticAllocation()
        {
            DataTable dtResult = new DataTable();
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcAutoProcessCreditCheckStockAllocation");
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];

            return dtResult;
        }
        public Int16 CallingFrom { get; set; }
        public DataTable SelectPackingSlip()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[14];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@toid", ToID);
            objSqlParam[2] = new SqlParameter("@fromid", FromID);
            if (FromDate != null && FromDate != "")
            {
                objSqlParam[3] = new SqlParameter("@fromdate", Convert.ToDateTime(FromDate));
            }
            else
            {
                objSqlParam[3] = new SqlParameter("@fromdate", FromDate);
            }
            if (ToDate != null && ToDate != "")
            {
                objSqlParam[4] = new SqlParameter("@todate", Convert.ToDateTime(ToDate));
            }
            else
            {
                objSqlParam[4] = new SqlParameter("@todate", ToDate);
            }
            objSqlParam[5] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[6] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[7] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[7].Direction = ParameterDirection.Output;
            objSqlParam[8] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[8].Direction = ParameterDirection.Output;
            objSqlParam[9] = new SqlParameter("@ordernumber", OrderNumber);
            objSqlParam[10] = new SqlParameter("@packingslipnumber", PackingSlipNumber);
            objSqlParam[11] = new SqlParameter("@invoicenumber", InvoiceNumber);
            objSqlParam[12] = new SqlParameter("@loginentityid", EntityId);
            objSqlParam[13] = new SqlParameter("@CallingFrom", CallingFrom);
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcPackingSlipInformation_Select", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            TotalRecords = Convert.ToInt32(objSqlParam[8].Value);
            Error = Convert.ToString(objSqlParam[0].Value);

            return dtResult;
        }
        /*#Ch28 Added Start*/
        public DataTable GetInvoicedDispatch()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[12];/*Ch34:parameter increase*/ /* #CC40 parameter Increased from 11 to 12*/
            objSqlParam[0] = new SqlParameter("@EntityID", ToID);
            if (FromDate != null && FromDate != "")
            {
                objSqlParam[1] = new SqlParameter("@FromDate", Convert.ToDateTime(FromDate));
            }
            else
            {
                objSqlParam[1] = new SqlParameter("@FromDate", FromDate);
            }
            if (ToDate != null && ToDate != "")
            {
                objSqlParam[2] = new SqlParameter("@ToDate", Convert.ToDateTime(ToDate));
            }
            else
            {
                objSqlParam[2] = new SqlParameter("@ToDate", ToDate);
            }
            objSqlParam[3] = new SqlParameter("@InvoiceNumber", InvoiceNumber);
            objSqlParam[4] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[5] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[6] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[6].Direction = ParameterDirection.Output;
            objSqlParam[7] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[7].Direction = ParameterDirection.Output;
            objSqlParam[8] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[8].Direction = ParameterDirection.Output;
            objSqlParam[9] = new SqlParameter("@loginentityid", LoggedInEntityID);/*Ch34:parameter increase*/
            objSqlParam[10] = new SqlParameter("@loginentitytypeid", LoggedInEntityTypeID);/*Ch34:parameter increase*/
            objSqlParam[11] = new SqlParameter("@DispatchStatus", DispatchStatus); /* #CC40 Added*/
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcGetInvoicedDispatch", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            TotalRecords = Convert.ToInt32(objSqlParam[6].Value);
            Error = Convert.ToString(objSqlParam[8].Value);

            return dtResult;
        }
        /*#Ch28 Added End*/
        public DataTable SelectCancelledInvoices()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[11];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@fromid", FromID);
            if (FromDate != null && FromDate != "")
            {
                objSqlParam[2] = new SqlParameter("@fromdate", Convert.ToDateTime(FromDate));
            }
            else
            {
                objSqlParam[2] = new SqlParameter("@fromdate", FromDate);
            }
            if (ToDate != null && ToDate != "")
            {
                objSqlParam[3] = new SqlParameter("@todate", Convert.ToDateTime(ToDate));
            }
            else
            {
                objSqlParam[3] = new SqlParameter("@todate", ToDate);
            }
            objSqlParam[4] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[5] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[6] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[6].Direction = ParameterDirection.Output;
            objSqlParam[7] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[7].Direction = ParameterDirection.Output;
            objSqlParam[8] = new SqlParameter("@invoicenumber", InvoiceNumber);
            objSqlParam[9] = new SqlParameter("@loginentityid", EntityId);
            objSqlParam[10] = new SqlParameter("@status", CancelStatus);
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcInvoiceInformation_Select", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            TotalRecords = Convert.ToInt32(objSqlParam[7].Value);
            Error = Convert.ToString(objSqlParam[0].Value);

            return dtResult;
        }

        public void InsertOrderAllocationInfo(DataTable Dt)
        {
            try
            {
                SqlParameter[] objSqlParam = new SqlParameter[7];
                objSqlParam[0] = new SqlParameter("@tvpOrderAllocate", SqlDbType.Structured);
                objSqlParam[0].Value = Dt;
                objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                objSqlParam[2].Direction = ParameterDirection.Output;
                objSqlParam[3] = new SqlParameter("@createdby", CreatedBy);
                objSqlParam[4] = new SqlParameter("@packingslipnumber", SqlDbType.VarChar, 500);
                objSqlParam[4].Direction = ParameterDirection.Output;
                objSqlParam[5] = new SqlParameter("@allocationidout", SqlDbType.VarChar, 500);
                objSqlParam[5].Direction = ParameterDirection.Output;
                objSqlParam[6] = new SqlParameter("@StockUpdateError", SqlDbType.Xml);
                objSqlParam[6].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(strSalesInvoiceXML, XmlNodeType.Document, null));
                objSqlParam[6].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "[prcOrderAllocation_Manual]", objSqlParam);
                Error = Convert.ToString(objSqlParam[1].Value);
                if (Convert.ToString(objSqlParam[5].Value) != "")/*Ch13:added*/
                    PackingSlipNumber = Convert.ToString(objSqlParam[4].Value);
                if (Convert.ToString(objSqlParam[5].Value) != "")/*Ch13:added*/
                    AllocationId = Convert.ToInt32(objSqlParam[5].Value);
                if (((System.Data.SqlTypes.SqlXml)objSqlParam[6].Value).IsNull != true)
                {
                    strSalesInvoiceXML = ((System.Data.SqlTypes.SqlXml)objSqlParam[6].Value).Value;

                }
                else
                {
                    strSalesInvoiceXML = null;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int16 OrderCancellation()
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[6];
            objSqlParam[0] = new SqlParameter("@cancelRemarks", Remarks);
            objSqlParam[1] = new SqlParameter("@orderid", OrderId);
            objSqlParam[2] = new SqlParameter("@ModifiedBy", CreatedBy);
            objSqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[4].Direction = ParameterDirection.Output;
            objSqlParam[5] = new SqlParameter("@IsApplyCancel", SqlDbType.TinyInt, 2);
            objSqlParam[5].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcOrderAllocation_Cancel", objSqlParam);
            result = Convert.ToInt16(objSqlParam[3].Value);
            Error = Convert.ToString(objSqlParam[4].Value);
            IsApplyCancel = Convert.ToInt16(objSqlParam[5].Value);
            return result;
        }

        public Int16 PackingSlipCancellation()
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[7]; /* #CC41 Length increased from 6 to 7 */
            objSqlParam[0] = new SqlParameter("@cancelRemarks", Remarks);
            objSqlParam[1] = new SqlParameter("@allocationid", AllocationId);
            objSqlParam[2] = new SqlParameter("@ModifiedBy", CreatedBy);
            objSqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[4].Direction = ParameterDirection.Output;
            objSqlParam[5] = new SqlParameter("@StockUpdateError", SqlDbType.Xml);
            objSqlParam[5].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(strSalesInvoiceXML, XmlNodeType.Document, null));
            objSqlParam[5].Direction = ParameterDirection.Output;
            objSqlParam[6] = new SqlParameter("@CallingMode", CallingFrom); /* #CC41 Added*/
            SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "[prcPackingSlip_Cancel]", objSqlParam);
            result = Convert.ToInt16(objSqlParam[3].Value);
            Error = Convert.ToString(objSqlParam[4].Value);
            if (((System.Data.SqlTypes.SqlXml)objSqlParam[5].Value).IsNull != true)
            {
                strSalesInvoiceXML = ((System.Data.SqlTypes.SqlXml)objSqlParam[5].Value).Value;

            }
            else
            {
                strSalesInvoiceXML = null;
                //OrderId = Convert.ToInt32(objSqlParam[5].Value);  Pankaj Dhingra
            }
            return result;
        }

        public Int16 OrderTransfer()
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[8];
            objSqlParam[0] = new SqlParameter("@transferRemarks", Remarks);
            objSqlParam[1] = new SqlParameter("@orderid", OrderId);
            objSqlParam[2] = new SqlParameter("@ModifiedBy", CreatedBy);
            objSqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[4].Direction = ParameterDirection.Output;
            objSqlParam[5] = new SqlParameter("@transferto", TransferTo);
            objSqlParam[6] = new SqlParameter("@ordernumber", SqlDbType.VarChar, 500);
            objSqlParam[6].Direction = ParameterDirection.Output;
            objSqlParam[7] = new SqlParameter("@StockUpdateError", SqlDbType.Xml);
            objSqlParam[7].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(strSalesInvoiceXML, XmlNodeType.Document, null));
            objSqlParam[7].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcOrderAllocation_Transfer", objSqlParam);
            if (((System.Data.SqlTypes.SqlXml)objSqlParam[7].Value).IsNull != true)
            {
                strSalesInvoiceXML = ((System.Data.SqlTypes.SqlXml)objSqlParam[7].Value).Value;

            }
            else
            {
                strSalesInvoiceXML = null;
            }
            result = Convert.ToInt16(objSqlParam[3].Value);
            Error = Convert.ToString(objSqlParam[4].Value);
            OrderNumber = Convert.ToString(objSqlParam[6].Value);

            return result;
        }
        public int InvoiceFromID { get; set; } /*Ch21 Added*/
        public void InsertInvoiceDetails(DataTable Dt)
        {
            try
            {
                SqlParameter[] objSqlParam = new SqlParameter[9];
                objSqlParam[0] = new SqlParameter("@tvpinvoice", SqlDbType.Structured);
                objSqlParam[0].Value = Dt;
                objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                objSqlParam[2].Direction = ParameterDirection.Output;
                objSqlParam[3] = new SqlParameter("@allocationid", AllocationId);
                objSqlParam[4] = new SqlParameter("@createdby", CreatedBy);
                objSqlParam[5] = new SqlParameter("@invoiceNumber", SqlDbType.VarChar, 500);
                objSqlParam[5].Direction = ParameterDirection.Output;
                objSqlParam[6] = new SqlParameter("@isSerial", IsSerial);
                objSqlParam[7] = new SqlParameter("@StockUpdateError", SqlDbType.Xml);
                objSqlParam[7].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(strSalesInvoiceXML, XmlNodeType.Document, null));
                objSqlParam[7].Direction = ParameterDirection.Output;
                /*Ch21 Added Start*/
                objSqlParam[8] = new SqlParameter("@InvoiceFromID", SqlDbType.Int);
                objSqlParam[8].Direction = ParameterDirection.Output;
                /*Ch21 Added End*/
                SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcInvoiceOnPackingSlip", objSqlParam);
                Error = Convert.ToString(objSqlParam[1].Value);
                InvoiceNumber = Convert.ToString(objSqlParam[5].Value);
                /*Ch21 Added Start*/
                if (objSqlParam[8].Value != DBNull.Value && objSqlParam[8].Value != null)
                {
                    InvoiceFromID = Convert.ToInt32(objSqlParam[8].Value);
                }
                /*Ch21 Added end*/
                if (((System.Data.SqlTypes.SqlXml)objSqlParam[7].Value).IsNull != true)
                {
                    strSalesInvoiceXML = ((System.Data.SqlTypes.SqlXml)objSqlParam[7].Value).Value;

                }
                else
                {
                    strSalesInvoiceXML = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*#Ch27 Added Start*/
        public void InsertInvoiceDetails(DataTable Dt, DataTable tvpAllocationID)
        {
            try
            {
                SqlParameter[] objSqlParam = new SqlParameter[15];
                objSqlParam[0] = new SqlParameter("@tvpinvoice", SqlDbType.Structured);
                objSqlParam[0].Value = Dt;
                objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                objSqlParam[2].Direction = ParameterDirection.Output;
                objSqlParam[3] = new SqlParameter("@tvpAllocationid", tvpAllocationID);
                objSqlParam[4] = new SqlParameter("@createdby", CreatedBy);
                objSqlParam[5] = new SqlParameter("@invoiceNumber", SqlDbType.VarChar, 500);
                objSqlParam[5].Direction = ParameterDirection.Output;
                objSqlParam[6] = new SqlParameter("@isSerial", IsSerial);
                objSqlParam[7] = new SqlParameter("@StockUpdateError", SqlDbType.Xml);
                objSqlParam[7].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(strSalesInvoiceXML, XmlNodeType.Document, null));
                objSqlParam[7].Direction = ParameterDirection.Output;
                /*Ch21 Added Start*/
                objSqlParam[8] = new SqlParameter("@InvoiceFromID", SqlDbType.Int);
                objSqlParam[8].Direction = ParameterDirection.Output;
                /*Ch21 Added End*/
                /*#CC35:added starrt*/
                objSqlParam[9] = new SqlParameter("@TokenNo", TokenNo);
                objSqlParam[10] = new SqlParameter("@CallingMode", CallingMode);
                objSqlParam[11] = new SqlParameter("@CurrentInvoiceAmount", SqlDbType.Decimal);
                /*#CC48:added Start*/
                objSqlParam[11].Precision = 29;
                objSqlParam[11].Scale = 2;
                /*#CC48:added end*/
                objSqlParam[11].Direction = ParameterDirection.Output;

                /*#CC35:added starrt*/
                SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcInvoiceOnPackingSlipV2", objSqlParam);
                Error = Convert.ToString(objSqlParam[1].Value);
                invoiceAmount = Convert.ToDecimal(objSqlParam[11].Value); /*#CC35:added*/
                InvoiceNumber = Convert.ToString(objSqlParam[5].Value);
                /*Ch21 Added Start*/
                if (objSqlParam[8].Value != DBNull.Value && objSqlParam[8].Value != null)
                {
                    InvoiceFromID = Convert.ToInt32(objSqlParam[8].Value);
                }
                /*Ch21 Added end*/
                if (((System.Data.SqlTypes.SqlXml)objSqlParam[7].Value).IsNull != true)
                {
                    strSalesInvoiceXML = ((System.Data.SqlTypes.SqlXml)objSqlParam[7].Value).Value;

                }
                else
                {
                    strSalesInvoiceXML = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*#Ch27 Added End*/
        public void InsertInvoiceDetailsUpload(DataTable Dt)
        {
            try
            {
                SqlParameter[] objSqlParam = new SqlParameter[8];
                objSqlParam[0] = new SqlParameter("@tvpinvoice", SqlDbType.Structured);
                objSqlParam[0].Value = Dt;
                objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                objSqlParam[2].Direction = ParameterDirection.Output;
                objSqlParam[3] = new SqlParameter("@allocationid", AllocationId);
                objSqlParam[4] = new SqlParameter("@createdby", CreatedBy);
                objSqlParam[5] = new SqlParameter("@invoiceNumber", SqlDbType.VarChar, 500);
                objSqlParam[5].Direction = ParameterDirection.Output;
                objSqlParam[6] = new SqlParameter("@StockUpdateError", SqlDbType.Xml);
                objSqlParam[6].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(strSalesInvoiceXML, XmlNodeType.Document, null));
                objSqlParam[6].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcInvoiceOnPackingSlipUpload", objSqlParam);
                Error = Convert.ToString(objSqlParam[1].Value);
                InvoiceNumber = Convert.ToString(objSqlParam[5].Value);
                if (((System.Data.SqlTypes.SqlXml)objSqlParam[6].Value).IsNull != true)
                {
                    strSalesInvoiceXML = ((System.Data.SqlTypes.SqlXml)objSqlParam[6].Value).Value;

                }
                else
                {
                    strSalesInvoiceXML = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectCreditCheckfailedOrders()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[10];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@fromid", FromID);
            if (FromDate != null && FromDate != "")
            {
                objSqlParam[2] = new SqlParameter("@fromdate", Convert.ToDateTime(FromDate));
            }
            else
            {
                objSqlParam[2] = new SqlParameter("@fromdate", FromDate);
            }
            if (ToDate != null && ToDate != "")
            {
                objSqlParam[3] = new SqlParameter("@todate", Convert.ToDateTime(ToDate));
            }
            else
            {
                objSqlParam[3] = new SqlParameter("@todate", ToDate);
            }
            objSqlParam[4] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[5] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[6] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[6].Direction = ParameterDirection.Output;
            objSqlParam[7] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[7].Direction = ParameterDirection.Output;
            objSqlParam[8] = new SqlParameter("@ordernumber", OrderNumber);
            objSqlParam[9] = new SqlParameter("@loginentityid", EntityId);
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcSelectOrderForFundApproval", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            TotalRecords = Convert.ToInt32(objSqlParam[7].Value);
            Error = Convert.ToString(objSqlParam[0].Value);

            return dtResult;
        }

        public Int16 InvoiceCancellation()
        {
            XMLError = "<NewDataset><table></table></NewDataset>";  //#Ch05: Added
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[6];
            objSqlParam[0] = new SqlParameter("@cancelRemarks", Remarks);
            objSqlParam[1] = new SqlParameter("@allocationid", AllocationId);
            objSqlParam[2] = new SqlParameter("@ModifiedBy", CreatedBy);
            objSqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[4].Direction = ParameterDirection.Output;
            /*(Start:#Ch05 - Added)*/
            objSqlParam[5] = new SqlParameter("@XML_PartError", SqlDbType.Xml);
            objSqlParam[5].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(XMLError, XmlNodeType.Document, null));
            objSqlParam[5].Direction = ParameterDirection.Output;
            /*(End:#Ch05 - Added)*/
            SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcInvoice_Cancel", objSqlParam);
            result = Convert.ToInt16(objSqlParam[3].Value);
            Error = Convert.ToString(objSqlParam[4].Value);
            /*(Start:#Ch05 - Added)*/
            if (((System.Data.SqlTypes.SqlXml)objSqlParam[5].Value).IsNull != true)
            {
                XMLError = ((System.Data.SqlTypes.SqlXml)objSqlParam[5].Value).Value;
            }
            else
            {
                XMLError = string.Empty;
            }
            /*(End:#Ch05 - Added)*/
            return result;
        }
        /*#Ch30 Added Start*/
        public Int16 InvoiceCancellationV2()
        {
            XMLError = "<NewDataset><table></table></NewDataset>";  //#Ch05: Added
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[6];
            objSqlParam[0] = new SqlParameter("@cancelRemarks", Remarks);
            objSqlParam[1] = new SqlParameter("@InvoiceID", InvoiceID);
            objSqlParam[2] = new SqlParameter("@ModifiedBy", CreatedBy);
            objSqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[4].Direction = ParameterDirection.Output;
            /*(Start:#Ch05 - Added)*/
            objSqlParam[5] = new SqlParameter("@XML_PartError", SqlDbType.Xml);
            objSqlParam[5].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(XMLError, XmlNodeType.Document, null));
            objSqlParam[5].Direction = ParameterDirection.Output;
            /*(End:#Ch05 - Added)*/
            SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcInvoice_CancelV2", objSqlParam);
            result = Convert.ToInt16(objSqlParam[3].Value);
            Error = Convert.ToString(objSqlParam[4].Value);
            /*(Start:#Ch05 - Added)*/
            if (((System.Data.SqlTypes.SqlXml)objSqlParam[5].Value).IsNull != true)
            {
                XMLError = ((System.Data.SqlTypes.SqlXml)objSqlParam[5].Value).Value;
            }
            else
            {
                XMLError = string.Empty;
            }
            /*(End:#Ch05 - Added)*/
            return result;
        }
        /*#Ch30 Added End*/
        public Int16 InvoiceCancellationApproval()
        {
            XMLError = "<NewDataset><table></table></NewDataset>";
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[7];
            objSqlParam[0] = new SqlParameter("@cancelRemarks", Remarks);
            objSqlParam[1] = new SqlParameter("@transactionid", TransactionId);
            objSqlParam[2] = new SqlParameter("@ModifiedBy", CreatedBy);
            objSqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[4].Direction = ParameterDirection.Output;
            objSqlParam[5] = new SqlParameter("@status", CancelStatus);
            objSqlParam[6] = new SqlParameter("@XML_PartError", SqlDbType.Xml);
            objSqlParam[6].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(XMLError, XmlNodeType.Document, null));
            objSqlParam[6].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcInvoiceRequest_Cancel", objSqlParam);
            result = Convert.ToInt16(objSqlParam[3].Value);
            Error = Convert.ToString(objSqlParam[4].Value);
            if (((System.Data.SqlTypes.SqlXml)objSqlParam[6].Value).IsNull != true)
            {
                XMLError = ((System.Data.SqlTypes.SqlXml)objSqlParam[6].Value).Value;
            }
            else
            {
                XMLError = null;
            }
            return result;
        }

        public DataTable SelectOrderFromDetails()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[2];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@loginentiyid", EntityId);
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "[prcFromOrder]", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[0].Value);


            return dtResult;
        }

        public DataTable SelectOrderTransferToDetails()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[2];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@orderid", OrderId);
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "[prcSelectOrderTransferToEntities]", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[0].Value);


            return dtResult;
        }
        public Int16 FillLaterStatus { get; set; }/*#CC18 Added*/
        public void InsertDispatchInfoForOrder()
        {
            try
            {
                int IntResultCount = 0;
                SqlParameter[] SqlParam = new SqlParameter[18];
                SqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 200);
                SqlParam[0].Direction = ParameterDirection.InputOutput;
                SqlParam[1] = new SqlParameter("@Remarks", Remarks);
                SqlParam[2] = new SqlParameter("@CreatedBy", CreatedBy);
                SqlParam[3] = new SqlParameter("@DispatchMode", DispatchMode);
                SqlParam[4] = new SqlParameter("@DocketDate", DocketDate);
                SqlParam[5] = new SqlParameter("@TransporterEntityID", TransporterId);
                SqlParam[6] = new SqlParameter("@TransporterOther", TransporterOther);
                SqlParam[7] = new SqlParameter("@VehicleNo", VehicleNo);
                SqlParam[8] = new SqlParameter("@TransporterMobileNo", TransporterMobileName);
                SqlParam[9] = new SqlParameter("@TransporterPersonName", TransportPersonName);
                SqlParam[10] = new SqlParameter("@DispatchNo", SqlDbType.NVarChar, 20);/*#CC51 changed 14 to 20*/
                SqlParam[10].Direction = ParameterDirection.Output;
                SqlParam[11] = new SqlParameter("@XMLPartDetailReturn", SqlDbType.Xml);
                SqlParam[11].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(strSalesInvoiceXML, XmlNodeType.Document, null));
                SqlParam[11].Direction = ParameterDirection.Output;
                SqlParam[12] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[12].Direction = ParameterDirection.Output;
                SqlParam[13] = new SqlParameter("@DocketNo", DocketNumber);
                SqlParam[14] = new SqlParameter("@CourierCompanyName", CourierCompanyName);
                SqlParam[15] = new SqlParameter("@allocationid", AllocationId);
                SqlParam[16] = new SqlParameter("@FillLaterStatus", FillLaterStatus); /*#CC18 Added*/
                SqlParam[17] = new SqlParameter("@DispatchType", DispatchType); /*#Ch24 Added*/
                IntResultCount = SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcDispatchOnInvoicedOrder", SqlParam);
                if (((System.Data.SqlTypes.SqlXml)SqlParam[11].Value).IsNull != true)
                {
                    strSalesInvoiceXML = ((System.Data.SqlTypes.SqlXml)SqlParam[11].Value).Value;

                }
                else
                {
                    strSalesInvoiceXML = null;
                }
                Error = SqlParam[0].Value.ToString();
                IntError = Convert.ToInt32(SqlParam[12].Value);
                DispatchNumber = SqlParam[10].Value.ToString();

            }

            catch (Exception ex)
            {

                throw ex;
            }

        }
        /*#Ch29 Added Start*/
        public Int64 InvoiceID { get; set; }
        public void InsertDispatchInfoForOrderV2()
        {
            try
            {
                int IntResultCount = 0;
                SqlParameter[] SqlParam = new SqlParameter[18];
                SqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 200);
                SqlParam[0].Direction = ParameterDirection.InputOutput;
                SqlParam[1] = new SqlParameter("@Remarks", Remarks);
                SqlParam[2] = new SqlParameter("@CreatedBy", CreatedBy);
                SqlParam[3] = new SqlParameter("@DispatchMode", DispatchMode);
                SqlParam[4] = new SqlParameter("@DocketDate", DocketDate);
                SqlParam[5] = new SqlParameter("@TransporterEntityID", TransporterId);
                SqlParam[6] = new SqlParameter("@TransporterOther", TransporterOther);
                SqlParam[7] = new SqlParameter("@VehicleNo", VehicleNo);
                SqlParam[8] = new SqlParameter("@TransporterMobileNo", TransporterMobileName);
                SqlParam[9] = new SqlParameter("@TransporterPersonName", TransportPersonName);
                SqlParam[10] = new SqlParameter("@DispatchNo", SqlDbType.NVarChar, 20);/*#CC51 changed 14 to 20*/
                SqlParam[10].Direction = ParameterDirection.Output;
                SqlParam[11] = new SqlParameter("@XMLPartDetailReturn", SqlDbType.Xml);
                SqlParam[11].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(strSalesInvoiceXML, XmlNodeType.Document, null));
                SqlParam[11].Direction = ParameterDirection.Output;
                SqlParam[12] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[12].Direction = ParameterDirection.Output;
                SqlParam[13] = new SqlParameter("@DocketNo", DocketNumber);
                SqlParam[14] = new SqlParameter("@CourierCompanyName", CourierCompanyName);
                SqlParam[15] = new SqlParameter("@InvoiceID", InvoiceID);
                SqlParam[16] = new SqlParameter("@FillLaterStatus", FillLaterStatus); /*#CC18 Added*/
                SqlParam[17] = new SqlParameter("@DispatchType", DispatchType); /*#Ch24 Added*/
                IntResultCount = SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcDispatchOnInvoicedOrderV2_InvoiceID", SqlParam);
                if (((System.Data.SqlTypes.SqlXml)SqlParam[11].Value).IsNull != true)
                {
                    strSalesInvoiceXML = ((System.Data.SqlTypes.SqlXml)SqlParam[11].Value).Value;

                }
                else
                {
                    strSalesInvoiceXML = null;
                }
                Error = SqlParam[0].Value.ToString();
                IntError = Convert.ToInt32(SqlParam[12].Value);
                DispatchNumber = SqlParam[10].Value.ToString();

            }

            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable GetInvoicedItemDetail()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[2];
            objSqlParam[0] = new SqlParameter("@InvoiceID", InvoiceID);
            objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 1000);
            objSqlParam[1].Direction = ParameterDirection.Output;

            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcGetInvoicedItemDetail", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            if (objSqlParam[1].Value != System.DBNull.Value)
            {
                Error = Convert.ToString(objSqlParam[1].Value);
            }
            else { Error = ""; }
            return dtResult;
        }
        /*#Ch29 Added End*/
        public DataTable SelectOrderForManualAllocation()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[13];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@toid", ToID);
            objSqlParam[2] = new SqlParameter("@fromid", FromID);
            if (FromDate != null && FromDate != "")
            {
                objSqlParam[3] = new SqlParameter("@fromdate", Convert.ToDateTime(FromDate));
            }
            else
            {
                objSqlParam[3] = new SqlParameter("@fromdate", FromDate);
            }
            if (ToDate != null && ToDate != "")
            {
                objSqlParam[4] = new SqlParameter("@todate", Convert.ToDateTime(ToDate));
            }
            else
            {
                objSqlParam[4] = new SqlParameter("@todate", ToDate);
            }
            objSqlParam[5] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[6] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[7] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[7].Direction = ParameterDirection.Output;
            objSqlParam[8] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[8].Direction = ParameterDirection.Output;
            objSqlParam[9] = new SqlParameter("@ordernumber", OrderNumber);
            objSqlParam[10] = new SqlParameter("@allocationstatus", OrderAllocationStatus);
            objSqlParam[11] = new SqlParameter("@loginentityid", EntityId);
            objSqlParam[12] = new SqlParameter("@loginentitytypeid", EntityTypeId);
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcOrderInformation_Select", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            TotalRecords = Convert.ToInt32(objSqlParam[8].Value);
            Error = Convert.ToString(objSqlParam[0].Value);

            return dtResult;
        }

        public DataTable SelectOrderForView()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[13];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@toid", ToID);
            objSqlParam[2] = new SqlParameter("@fromid", FromID);
            if (FromDate != null && FromDate != "")
            {
                objSqlParam[3] = new SqlParameter("@fromdate", Convert.ToDateTime(FromDate));
            }
            else
            {
                objSqlParam[3] = new SqlParameter("@fromdate", FromDate);
            }
            if (ToDate != null && ToDate != "")
            {
                objSqlParam[4] = new SqlParameter("@todate", Convert.ToDateTime(ToDate));
            }
            else
            {
                objSqlParam[4] = new SqlParameter("@todate", ToDate);
            }
            objSqlParam[5] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[6] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[7] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[7].Direction = ParameterDirection.Output;
            objSqlParam[8] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[8].Direction = ParameterDirection.Output;
            objSqlParam[9] = new SqlParameter("@ordernumber", OrderNumber);
            objSqlParam[10] = new SqlParameter("@allocationstatus", OrderAllocationStatus);
            objSqlParam[11] = new SqlParameter("@loginentityid", EntityId);
            objSqlParam[12] = new SqlParameter("@loginentitytypeid", EntityTypeId);
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcOrderInformation_SelectForView", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            TotalRecords = Convert.ToInt32(objSqlParam[8].Value);
            Error = Convert.ToString(objSqlParam[0].Value);

            return dtResult;
        }

        public Int16 InsertApprovalForCreditFailedOrders()
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[5];
            objSqlParam[0] = new SqlParameter("@remarks", Remarks);
            objSqlParam[1] = new SqlParameter("@orderid", OrderId);
            objSqlParam[2] = new SqlParameter("@createdby", CreatedBy);
            objSqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[4].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcFailedCreditCheckOrderApproval", objSqlParam);
            if (objSqlParam[3].Value.ToString() != "")
            {
                result = Convert.ToInt16(objSqlParam[3].Value);
            }
            Error = Convert.ToString(objSqlParam[4].Value);
            return result;
        }

        //public DataTable SelectOrderetailsForPrint()  //#Ch03:commented
        public DataSet SelectOrderetailsForPrint()  //#Ch03:Added
        {
            //DataTable dtResult = new DataTable(); //#Ch03:commented
            SqlParameter[] objSqlParam = new SqlParameter[2];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@orderid", OrderId);
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "[prcSalesOrder_SelectReport]", objSqlParam);
            //if (dsResult != null && dsResult.Tables.Count > 0)    //#Ch03:commented
            //    dtResult = dsResult.Tables[0];                    //#Ch03:commented
            Error = Convert.ToString(objSqlParam[0].Value);
            //return dtResult;  //#Ch03:commented
            return dsResult;    //#Ch03:Added

        }

        //public DataSet SelectPackingSlipForPrint()
        //{
        //    DataSet dssResult = new DataSet();
        //    SqlParameter[] objSqlParam = new SqlParameter[2];
        //    objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        //    objSqlParam[0].Direction = ParameterDirection.Output;
        //    objSqlParam[1] = new SqlParameter("@allocationid",AllocationId);
        //    DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcSelectPackingSlipForPrint", objSqlParam);
        //    //if (dsResult != null && dsResult.Tables.Count > 0)
        //    //    dtResult = dsResult.Tables[0];
        //    Error = Convert.ToString(objSqlParam[0].Value);
        //    return dsResult;
        //}

        //OLd One
        public DataTable SelectPackingSlipForPrint()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@allocationid", AllocationId);
            objSqlParam[2] = new SqlParameter("@Value", Value);
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcSelectPackingSlipForPrint", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[0].Value);
            return dtResult;
        }

        public DataTable SelectNonParentOrders()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[10];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@fromid", FromID);
            if (FromDate != null && FromDate != "")
            {
                objSqlParam[2] = new SqlParameter("@fromdate", Convert.ToDateTime(FromDate));
            }
            else
            {
                objSqlParam[2] = new SqlParameter("@fromdate", FromDate);
            }
            if (ToDate != null && ToDate != "")
            {
                objSqlParam[3] = new SqlParameter("@todate", Convert.ToDateTime(ToDate));
            }
            else
            {
                objSqlParam[3] = new SqlParameter("@todate", ToDate);
            }
            objSqlParam[4] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[5] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[6] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[6].Direction = ParameterDirection.Output;
            objSqlParam[7] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[7].Direction = ParameterDirection.Output;
            objSqlParam[8] = new SqlParameter("@ordernumber", OrderNumber);
            objSqlParam[9] = new SqlParameter("@loginentityid", EntityId);
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcNonParentalOrders_Select", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            TotalRecords = Convert.ToInt32(objSqlParam[7].Value);
            Error = Convert.ToString(objSqlParam[0].Value);

            return dtResult;
        }

        public Int16 ApproveNonParentalOrders()
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[6];
            objSqlParam[0] = new SqlParameter("@remarks", Remarks);
            objSqlParam[1] = new SqlParameter("@transactionid", TransactionId);
            objSqlParam[2] = new SqlParameter("@createdby", CreatedBy);
            objSqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[4].Direction = ParameterDirection.Output;
            objSqlParam[5] = new SqlParameter("@status", CancelStatus);
            SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "[prcNonParentalOrders_Approve]", objSqlParam);
            result = Convert.ToInt16(objSqlParam[3].Value);
            Error = Convert.ToString(objSqlParam[4].Value);
            return result;
        }

        public DataTable SelectSalesOrderCancelRequest()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[10];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@fromid", FromID);
            if (FromDate != null && FromDate != "")
            {
                objSqlParam[2] = new SqlParameter("@fromdate", Convert.ToDateTime(FromDate));
            }
            else
            {
                objSqlParam[2] = new SqlParameter("@fromdate", FromDate);
            }
            if (ToDate != null && ToDate != "")
            {
                objSqlParam[3] = new SqlParameter("@todate", Convert.ToDateTime(ToDate));
            }
            else
            {
                objSqlParam[3] = new SqlParameter("@todate", ToDate);
            }
            objSqlParam[4] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[5] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[6] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[6].Direction = ParameterDirection.Output;
            objSqlParam[7] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[7].Direction = ParameterDirection.Output;
            objSqlParam[8] = new SqlParameter("@ordernumber", OrderNumber);
            objSqlParam[9] = new SqlParameter("@loginentityid", EntityId);
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcSalesOrderCancelRequest_Select", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            TotalRecords = Convert.ToInt32(objSqlParam[7].Value);
            Error = Convert.ToString(objSqlParam[0].Value);

            return dtResult;
        }

        public Int16 ApproveSalesOrderCancelRequest()
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[6];
            objSqlParam[0] = new SqlParameter("@remarks", Remarks);
            objSqlParam[1] = new SqlParameter("@transactionid", TransactionId);
            objSqlParam[2] = new SqlParameter("@createdby", CreatedBy);
            objSqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[4].Direction = ParameterDirection.Output;
            objSqlParam[5] = new SqlParameter("@status", CancelStatus);
            SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "[prcSalesOrderCancelRequest_Approve]", objSqlParam);
            result = Convert.ToInt16(objSqlParam[3].Value);
            Error = Convert.ToString(objSqlParam[4].Value);
            return result;
        }

        public void InsertSerialBatchFIFOInfo()
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@SalesOrderAllocationID ", AllocationId);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "[prcSalesOrderAllocation_GetEligibleSerialBatchOnFIFO]", objSqlParam);
            IntError = Convert.ToInt32(objSqlParam[1].Value);
            Error = Convert.ToString(objSqlParam[2].Value);

        }

        public DataTable SelectOrderAdditionalDetails()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@SalesOrderId", OrderId);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcSalesOrder_AdditionalDetails", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[2].Value);
            return dtResult;
        }

        public DataTable SelectOrderItemDetails()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@SalesOrderId", OrderId);
            objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcSalesOrder_ItemDetails", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[2].Value);
            return dtResult;
        }


        public DataTable SelectOrderForView_Ver2()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[13];
            objSqlParam[0] = new SqlParameter("@OrderType", OrderType);
            objSqlParam[1] = new SqlParameter("@FromEntities", FromEntities);
            objSqlParam[2] = new SqlParameter("@ToEntities", ToEntities);
            objSqlParam[3] = new SqlParameter("@FromDate", SoFromDate);
            objSqlParam[4] = new SqlParameter("@ToDate", SoToDate);
            objSqlParam[5] = new SqlParameter("@OrderNumber", OrderNumber);
            objSqlParam[6] = new SqlParameter("@AllocationStatus", OrderAllocationStatus);
            objSqlParam[7] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[8] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[9] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[9].Direction = ParameterDirection.Output;
            objSqlParam[10] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[10].Direction = ParameterDirection.Output;
            objSqlParam[11] = new SqlParameter("@SOCreditCheckStatus", SoCreaditCheckStatus); /*#CC17:Added*/
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcOrderInformation", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            TotalRecords = Convert.ToInt32(objSqlParam[9].Value);
            Error = Convert.ToString(objSqlParam[10].Value);

            return dtResult;
        }
        /*#Ch01 added (start)*/
        public void UpdateSalesOrderInfo(DataTable Dt)
        {
            try
            {
                Result = 1;
                SqlParameter[] objSqlParam = new SqlParameter[15];
                objSqlParam[0] = new SqlParameter("@tvpSoOrder", SqlDbType.Structured);
                objSqlParam[0].Value = Dt;
                objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                objSqlParam[2].Direction = ParameterDirection.Output;
                objSqlParam[3] = new SqlParameter("@fromid", FromID);
                objSqlParam[4] = new SqlParameter("@toid", ToID);
                objSqlParam[5] = new SqlParameter("@createdby", CreatedBy);
                objSqlParam[6] = new SqlParameter("@StockUpdateError", SqlDbType.Xml);
                objSqlParam[6].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(strSalesInvoiceXML, XmlNodeType.Document, null));
                objSqlParam[6].Direction = ParameterDirection.Output;
                objSqlParam[7] = new SqlParameter("@frieghtstatus", FrieghtStatus);
                objSqlParam[8] = new SqlParameter("@bookingDetails", BookingDetails);
                objSqlParam[9] = new SqlParameter("@exciseregNo", ExciseRegNo);
                objSqlParam[10] = new SqlParameter("@cargoTot", cargoToT);
                objSqlParam[11] = new SqlParameter("@SalesOrderID", OrderId);
                objSqlParam[12] = new SqlParameter("@Status", Status);
                objSqlParam[13] = new SqlParameter("@Reason", Reason);
                objSqlParam[14] = new SqlParameter("@OtherDiscount", OtherDiscount);
                SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcSalesOrder_UpDate", objSqlParam);
                /*#CC01: Changed (start)*/
                Error = Convert.ToString(objSqlParam[1].Value);
                if (Convert.ToString(objSqlParam[1].Value) != "" && Convert.ToInt32(objSqlParam[2].Value) > 0)
                {
                    return;
                }
                /*#CC01: added (start)*/
                Result = Convert.ToInt16(objSqlParam[2].Value);
                /*#CC01: added (End)*/

                if (((System.Data.SqlTypes.SqlXml)objSqlParam[6].Value).IsNull != true)
                {
                    strSalesInvoiceXML = ((System.Data.SqlTypes.SqlXml)objSqlParam[6].Value).Value;

                }
                else
                {
                    strSalesInvoiceXML = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*#Ch01 added (End)*/

        public DataSet GetApproveSO()
        {
            SqlParameter[] objSqlParam = new SqlParameter[5];
            objSqlParam[0] = new SqlParameter("@tvpAllData", SqlDbType.Structured);
            objSqlParam[0].Value = DtGetApproveSo;
            objSqlParam[1] = new SqlParameter("@CreatedBy", CreatedBy);
            objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@XMLError", SqlDbType.Xml);
            objSqlParam[4].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(XMLError, XmlNodeType.Document, null));
            objSqlParam[4].Direction = ParameterDirection.Output;
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcApprovedSO_Insert", objSqlParam);
            //SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcApprovedSO_Insert", objSqlParam);
            Result = Convert.ToInt16(objSqlParam[2].Value);
            Error = Convert.ToString(objSqlParam[3].Value);
            if (((System.Data.SqlTypes.SqlXml)objSqlParam[4].Value).IsNull != true)
            {
                XMLError = ((System.Data.SqlTypes.SqlXml)objSqlParam[4].Value).Value;
            }
            else
            {
                XMLError = null;
            }
            return dsResult;
        }



        /*(Start:#Ch04 - Added)*/
        /// <summary>
        /// Get all selected schemes for selected parts.
        /// </summary>
        /// <returns></returns>
        public DataTable SelectAvailableSchemes()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[6];
            objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@PartDetails", DtPurschasedParts);
            objSqlParam[3] = new SqlParameter("@OrderFrom", FromID);
            objSqlParam[4] = new SqlParameter("@SelPromotions", SqlDbType.Structured);
            objSqlParam[4].Value = _dtSelectedPromotions;
            objSqlParam[5] = new SqlParameter("@OtherDiscount", SqlDbType.Decimal, 10);
            objSqlParam[5].Direction = ParameterDirection.Output;
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcScheme_Select", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[1].Value);
            if (Convert.ToString(objSqlParam[5].Value) != string.Empty)
            {
                OtherDiscount = Convert.ToDecimal(objSqlParam[5].Value);
            }

            return dtResult;
        }
        /*(End:#Ch04 - Added)*/

        /*(Start:#Ch08 - Added)*/
        /// <summary>
        /// Get all available schemes for selected parts.
        /// </summary>
        /// <returns></returns>
        public DataSet SelectAllSchemes()
        {
            SqlParameter[] objSqlParam = new SqlParameter[6];/*Ch22:increase*/
            objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@PartDetails", DtPurschasedParts);
            objSqlParam[3] = new SqlParameter("@OrderFrom", FromID);
            objSqlParam[4] = new SqlParameter("@OtherDiscount", SqlDbType.Decimal, 10);
            objSqlParam[4].Direction = ParameterDirection.Output;
            objSqlParam[5] = new SqlParameter("@OrderTo", ToID);/*Ch22:added*/
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcAvailablePromotion_Select", objSqlParam);
            Error = Convert.ToString(objSqlParam[1].Value);
            Result = Convert.ToInt16(objSqlParam[0].Value);
            OtherDiscount = Convert.ToString(objSqlParam[4].Value) != string.Empty ? Convert.ToDecimal(objSqlParam[4].Value) : 0;

            return dsResult;
        }
        /*(End:#Ch08 - Added)*/

        public DataTable SelectSuggestedDet()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[4];
            objSqlParam[0] = new SqlParameter("@SalesOrderID", OrderId);
            objSqlParam[1] = new SqlParameter("@Type", Type);
            objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[3].Direction = ParameterDirection.Output;
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcSalesOrderSuggQty_SelectBySOID", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[3].Value);

            return dtResult;
        }
        /*Ch12:added (start)*/
        public DataTable SelectSaleOrderPickList()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[13];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@OrderTo", ToID);
            objSqlParam[2] = new SqlParameter("@OrderFrom", FromID);
            if (FromDate != null && FromDate != "")
            {
                objSqlParam[3] = new SqlParameter("@FromDate", Convert.ToDateTime(FromDate));
            }
            else
            {
                objSqlParam[3] = new SqlParameter("@FromDate", FromDate);
            }
            if (ToDate != null && ToDate != "")
            {
                objSqlParam[4] = new SqlParameter("@ToDate", Convert.ToDateTime(ToDate));
            }
            else
            {
                objSqlParam[4] = new SqlParameter("@ToDate", ToDate);
            }
            objSqlParam[5] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[6] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[7] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[7].Direction = ParameterDirection.Output;
            objSqlParam[8] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[8].Direction = ParameterDirection.Output;
            objSqlParam[9] = new SqlParameter("@OrderNumber", OrderNumber);
            objSqlParam[10] = new SqlParameter("@SapPartCode", PartCode);
            objSqlParam[11] = new SqlParameter("@loginentityid", EntityId);
            objSqlParam[12] = new SqlParameter("@StockMode", StockMode);/* #CC39 Added*/
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcSaleOrderPick_Select", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            TotalRecords = Convert.ToInt32(objSqlParam[8].Value);
            Error = Convert.ToString(objSqlParam[0].Value);

            return dtResult;
        }

        public Int16 UpdateSalesOrderAllocationDetail(DataTable Dt)
        {
            try
            {
                /*#Ch32 Added Start*/
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                SqlConnection conStr = new SqlConnection(strConStr);
                /*#ch32 Added Start*/
                Result = 1;
                SqlParameter[] objSqlParam = new SqlParameter[14];/* #CC37 increased */
                objSqlParam[0] = new SqlParameter("@tvpSalesOrderAllocationDetail", SqlDbType.Structured);
                objSqlParam[0].Value = Dt;
                objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                objSqlParam[2].Direction = ParameterDirection.Output;
                objSqlParam[3] = new SqlParameter("@OrderFrom", FromID);
                objSqlParam[4] = new SqlParameter("@OrderTo", ToID);
                objSqlParam[5] = new SqlParameter("@CreatedBy", CreatedBy);
                objSqlParam[6] = new SqlParameter("@StockUpdateError", SqlDbType.Xml);
                objSqlParam[6].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(strSalesInvoiceXML, XmlNodeType.Document, null));
                objSqlParam[6].Direction = ParameterDirection.Output;
                objSqlParam[7] = new SqlParameter("@Type", Type);
                objSqlParam[8] = new SqlParameter("@FromDate", SoFromDate);/*CC19:change*/
                objSqlParam[9] = new SqlParameter("@ToDate", SoToDate);/*CC19:change*/
                objSqlParam[10] = new SqlParameter("@OrderNumber", OrderNumber);
                objSqlParam[11] = new SqlParameter("@PartCode", PartCode);
                objSqlParam[12] = new SqlParameter("@SaleOrderPickListNumber", SqlDbType.VarChar, 500); /*Ch20:added: SaleOrderPickList+ number added */
                objSqlParam[12].Direction = ParameterDirection.Output; /*Ch20:added */
                objSqlParam[13] = new SqlParameter("@loginentityid", EntityId);/* #CC37 Added */

                /*#ch32 Added Start */
                //SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcSaleOrderAllocationDetail_Update", objSqlParam);
                cmd = new SqlCommand("prcSaleOrderAllocationDetail_Update", conStr);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(objSqlParam);
                da.SelectCommand = cmd;

                DataSet dsResult = new DataSet();
                da.SelectCommand.CommandTimeout = 180;
                da.Fill(dsResult);
                /*#ch32 Added End */
                Error = Convert.ToString(objSqlParam[1].Value);
                Result = Convert.ToInt16(objSqlParam[2].Value);
                /*#CC15:added (start)*/
                if (((System.Data.SqlTypes.SqlXml)objSqlParam[6].Value).IsNull != true)
                {
                    strSalesInvoiceXML = ((System.Data.SqlTypes.SqlXml)objSqlParam[6].Value).Value;

                }
                if (Convert.ToString(objSqlParam[12].Value) != "") /*Ch20:added */
                    SalesOrderPickListNumber = Convert.ToString(objSqlParam[12].Value); /*Ch20:added */
                /*#CC15:added (end)*/
                return Result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*Ch12:added (end)*/

        /*#Ch25:added Start */

        public DataSet LoaddynCtrl()
        {
            SqlParameter[] objSqlParam = new SqlParameter[5];
            objSqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@UserID", UserDetailId);
            objSqlParam[3] = new SqlParameter("@Value", Pvalue);
            objSqlParam[4] = new SqlParameter("@TableName", TableName);
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcGetControlsinformation", objSqlParam);
            Error = Convert.ToString(objSqlParam[1].Value);
            Result = Convert.ToInt16(objSqlParam[0].Value);
            return dsResult;
        }
        /*#Ch25:added End */
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

        ~clsSalesOrder()
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


        public void InsertSalesOrderInfoBulk(DataTable Dt)
        {
            try
            {
                SqlParameter[] objSqlParam = new SqlParameter[20];
                objSqlParam[0] = new SqlParameter("@tvpSoOrderBulk", SqlDbType.Structured);
                objSqlParam[0].Value = Dt;
                objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                objSqlParam[2].Direction = ParameterDirection.Output;
                objSqlParam[3] = new SqlParameter("@orderdate", Convert.ToDateTime(Orderdate));
                objSqlParam[4] = new SqlParameter("@FromID", FromID);
                objSqlParam[5] = new SqlParameter("@ToID", ToID);
                objSqlParam[6] = new SqlParameter("@PONumber", PONumber);
                objSqlParam[7] = new SqlParameter("@remarks", Remarks);
                objSqlParam[8] = new SqlParameter("@createdby", CreatedBy);
                objSqlParam[9] = new SqlParameter("@ordernumber", SqlDbType.VarChar, 500);
                objSqlParam[9].Direction = ParameterDirection.Output;
                objSqlParam[10] = new SqlParameter("@documenttype", DocumentType);
                objSqlParam[11] = new SqlParameter("@istransfer", IsTransfer);
                objSqlParam[12] = new SqlParameter("@StockUpdateError", SqlDbType.Xml);
                objSqlParam[12].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(strSalesInvoiceXML, XmlNodeType.Document, null));
                objSqlParam[12].Direction = ParameterDirection.Output;
                objSqlParam[13] = new SqlParameter("@orderidout", SqlDbType.Int, 2);
                objSqlParam[13].Direction = ParameterDirection.Output;
                objSqlParam[14] = new SqlParameter("@frieghtstatus", FrieghtStatus);
                objSqlParam[15] = new SqlParameter("@bookingDetails", BookingDetails);
                objSqlParam[16] = new SqlParameter("@exciseregNo", ExciseRegNo);
                objSqlParam[17] = new SqlParameter("@cargoTot", cargoToT);
                objSqlParam[18] = new SqlParameter("@OrdertypeID", OrderTypeId); /*#Ch17:Added*/
                SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcBulkPOGeneration_Upload", objSqlParam);
                //Error = Convert.ToString(objSqlParam[2].Value);/*Ch23:code commented */
                Error = Convert.ToString(objSqlParam[1].Value);/*Ch23:code added */
                OrderNumber = Convert.ToString(objSqlParam[9].Value);
                Result = Convert.ToInt16(objSqlParam[2].Value);/*Ch23:code added */
                if (((System.Data.SqlTypes.SqlXml)objSqlParam[12].Value).IsNull != true)
                {
                    strSalesInvoiceXML = ((System.Data.SqlTypes.SqlXml)objSqlParam[12].Value).Value;

                }
                else
                {
                    strSalesInvoiceXML = null;
                    OrderId = Convert.ToInt32(objSqlParam[13].Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// This will fetch the retailer's list for the Van sales interface
        /// </summary>
        /// <returns></returns>
        public DataTable SelectDealer()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[6];
            objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[2] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@LoggedInEntityID", LoggedInEntityID);
            objSqlParam[5] = new SqlParameter("@LoggedInEntityTypeID", LoggedInEntityTypeID);
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcEntityMaster_SelectDealer", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            TotalRecords = Convert.ToInt32(objSqlParam[2].Value);
            Error = Convert.ToString(objSqlParam[3].Value);
            return dtResult;
        }
        /*Ch11:added (start)*/
        public DataTable SelectOrderType()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[5];/*Ch31:incresed*/
            objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@OrderFromEntityID", FromID);/*Ch31:added*/
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcSelect_OrderType", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[2].Value);
            return dtResult;
        }
        /*Ch11:added (end)*/

        /*Ch12:added (start)*/
        public DataTable SelectStockMode()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[4];
            objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[3].Direction = ParameterDirection.Output;
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcSelect_StockMode", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[2].Value);
            return dtResult;
        }
        /*Ch12:added (end)*/

        /*Ch14:added (start)*/
        public Int16 AutoAllocatePO()
        {
            /*#CC52:Added Start*/
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlConnection conStr = new SqlConnection(strConStr);

            /*#CC52:Added End*/

            Int16 result = 1;
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[5];/*Ch15:increase*/
            objSqlParam[0] = new SqlParameter("@createdby", CreatedBy);
            objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@FromEntityID", FromID);/*Ch15:added*/
            objSqlParam[4] = new SqlParameter("@ToEntityID", ToID);  /*Ch15:added*/

            /*SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcOrderAutoPartAllocateAll", objSqlParam);*/
            /*#CC52:Commented*/
            /*#CC52:Added Start*/

            cmd = new SqlCommand("prcOrderAutoPartAllocateAll", conStr);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(objSqlParam);
            da.SelectCommand = cmd;

            DataSet dsResult = new DataSet();
            da.SelectCommand.CommandTimeout = 200;
            da.Fill(dsResult);
            /*#CC52:Added End*/
            result = Convert.ToInt16(objSqlParam[2].Value);
            Error = Convert.ToString(objSqlParam[1].Value);
            return result;
        }
        /*Ch14:added (start)*/

        /*Ch25:added (start)*/
        public DataSet GetProcessSO()
        {
            SqlParameter[] objSqlParam = new SqlParameter[5];
            objSqlParam[0] = new SqlParameter("@tvpAllData", SqlDbType.Structured);
            objSqlParam[0].Value = DtGetApproveSo;
            objSqlParam[1] = new SqlParameter("@CreatedBy", CreatedBy);
            objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@XMLError", SqlDbType.Xml);
            objSqlParam[4].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(XMLError, XmlNodeType.Document, null));
            objSqlParam[4].Direction = ParameterDirection.Output;
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcProcessSO_Insert", objSqlParam);
            //SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcApprovedSO_Insert", objSqlParam);
            Result = Convert.ToInt16(objSqlParam[2].Value);
            Error = Convert.ToString(objSqlParam[3].Value);
            if (((System.Data.SqlTypes.SqlXml)objSqlParam[4].Value).IsNull != true)
            {
                XMLError = ((System.Data.SqlTypes.SqlXml)objSqlParam[4].Value).Value;
            }
            else
            {
                XMLError = null;
            }
            return dsResult;
        }
        /*Ch25:added (end)*/

        /*#CC35:added (start)*/
        public Int16 CheckSOCreditCheckFromAPI()
        {
            Int16 result = 0;
            SqlParameter[] objSqlParam = new SqlParameter[5];
            objSqlParam[0] = new SqlParameter("@FromEntityID", FromID);
            objSqlParam[1] = new SqlParameter("@CheckStatus", SqlDbType.TinyInt, 2);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@EntitySAPCode", SqlDbType.VarChar, 50);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@ToEntityId", ToID);
            SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcCheckSOCeditLimitFrom", objSqlParam);
            result = Convert.ToInt16(objSqlParam[1].Value);
            EntitySAPCode = Convert.ToString(objSqlParam[2].Value);
            return result;
        }
        public Int16 RequestedRaisedForAPISave(DataTable DtAllocationId)
        {
            Int16 result = 1;
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[10];
            objSqlParam[0] = new SqlParameter("@createdby", CreatedBy);
            objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@FromEntityID", FromID);
            objSqlParam[4] = new SqlParameter("@Amount", Amount);
            objSqlParam[5] = new SqlParameter("@TokenNo", SqlDbType.NVarChar, 50, TokenNo);
            objSqlParam[5].Direction = ParameterDirection.InputOutput;
            objSqlParam[6] = new SqlParameter("@CallingMode", CallingMode);
            objSqlParam[7] = new SqlParameter("@CheckStatus", CheckStatus);
            objSqlParam[8] = new SqlParameter("@InvoicingStatus", InvoicingStatus);
            objSqlParam[9] = new SqlParameter("@tvpAllocationid", DtAllocationId);
            SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcRequestedRaisedForAPI", objSqlParam);
            result = Convert.ToInt16(objSqlParam[2].Value);
            Error = Convert.ToString(objSqlParam[1].Value);
            TokenNo = Convert.ToString(objSqlParam[5].Value);
            return result;

        }

        public Int16 UpdateAPIResult()
        {
            Int16 result = 1;
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[10];
            objSqlParam[0] = new SqlParameter("@createdby", CreatedBy);
            objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@FromEntityID", FromID);
            objSqlParam[4] = new SqlParameter("@Amount", Amount);
            objSqlParam[5] = new SqlParameter("@TokenNo", TokenNo);
            objSqlParam[6] = new SqlParameter("@CallingMode", CallingMode);
            objSqlParam[7] = new SqlParameter("@CheckStatus", CheckStatus);
            objSqlParam[8] = new SqlParameter("@InvoicingStatus", InvoicingStatus);
            SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcUpdateAPIResult", objSqlParam);
            result = Convert.ToInt16(objSqlParam[2].Value);
            Error = Convert.ToString(objSqlParam[1].Value);
            return result;

        }
        /*#CC35:added (end)*/


        /*#CC36:added (start)*/

        public DataTable EntityNameForSalesOrderInvoice()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[10];
            objSqlParam[0] = new SqlParameter("@BusinessEventKeyword", BusinessEventKeyword);
            objSqlParam[1] = new SqlParameter("@entitytypebit", EntityTypeDescription);
            objSqlParam[2] = new SqlParameter("@ActiveMode", SelectionMode);
            objSqlParam[3] = new SqlParameter("@Keyword", Keyword);
            objSqlParam[4] = new SqlParameter("@IsParent", IsParent);
            objSqlParam[5] = new SqlParameter("@Type", Type);
            objSqlParam[6] = new SqlParameter("@SelectedEntityID", ParentSelectedValue);
            objSqlParam[7] = new SqlParameter("@LogedInEntityID", LoggedInEntityID);
            objSqlParam[8] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[8].Direction = ParameterDirection.Output;
            objSqlParam[9] = new SqlParameter("@TotalRecord", SqlDbType.BigInt);
            objSqlParam[9].Direction = ParameterDirection.Output;

            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcEntityNameForSalesOrderInvoiceCreation", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[8].Value);
            return dtResult;
        }


        /*#CC36:added (end)*/

        /*#CC38:Added start here*/

        public DataTable FromEntityName()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[10];
            objSqlParam[0] = new SqlParameter("@BusinessEventKeyword", BusinessEventKeyword);
            objSqlParam[1] = new SqlParameter("@entitytypebit", EntityTypeDescription);
            objSqlParam[2] = new SqlParameter("@ActiveMode", SelectionMode);
            objSqlParam[3] = new SqlParameter("@Keyword", Keyword);
            objSqlParam[4] = new SqlParameter("@IsParent", IsParent);
            objSqlParam[5] = new SqlParameter("@Type", Type);
            objSqlParam[6] = new SqlParameter("@SelectedEntityID", ParentSelectedValue);
            objSqlParam[7] = new SqlParameter("@LogedInEntityID", LoggedInEntityID);
            objSqlParam[8] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[8].Direction = ParameterDirection.Output;
            objSqlParam[9] = new SqlParameter("@TotalRecord", SqlDbType.BigInt);
            objSqlParam[9].Direction = ParameterDirection.Output;

            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcFromEntityName", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[8].Value);
            return dtResult;
        }
        public DataTable DirctSaleOrderUploadInsert()
        {
            try
            {
                DataTable dtResult = new DataTable();
                int result = -1;
                SqlParameter[] objSqlParam = new SqlParameter[12]; /*#CC53: Added increase length from 11 to 12*/
                objSqlParam[0] = new SqlParameter("@TvpDirectSaleUpload", SqlDbType.Structured);
                objSqlParam[0].Value = dtSoDetail;
                objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                objSqlParam[2].Direction = ParameterDirection.Output;
                objSqlParam[3] = new SqlParameter("@createdby", CreatedBy);
                objSqlParam[4] = new SqlParameter("@StockUpdateError", SqlDbType.Xml, 5);
                objSqlParam[4].Direction = ParameterDirection.Output;
                objSqlParam[5] = new SqlParameter("@Salefromid", Salefromid);
                objSqlParam[6] = new SqlParameter("@StockModeId", StockModeId);
                objSqlParam[7] = new SqlParameter("@ReferenceNo", ReferenceNo);
                objSqlParam[8] = new SqlParameter("@remarks", Remarks);
                objSqlParam[9] = new SqlParameter("@UploadedfileName", UploadedfileName);
                objSqlParam[10] = new SqlParameter("@ProcessFileName", ProcessFileName);
                objSqlParam[11] = new SqlParameter("@ReceiveStockMode", StockMode);/*#CC53:Added*/
                DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcInsertDirectSalesUpload", objSqlParam);
                Error = Convert.ToString(objSqlParam[1].Value);
                result = Convert.ToInt32(objSqlParam[2].Value);
                if (dsResult != null && dsResult.Tables.Count > 0)
                    dtResult = dsResult.Tables[0];
                if (objSqlParam[4].Value != System.DBNull.Value)
                    strSalesInvoiceXML = Convert.ToString(objSqlParam[4].Value);
                else
                    strSalesInvoiceXML = null;
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*#CC38:Added end here*/

        #endregion

        /* #CC39 Added start */
        #region SalesOrderPickList-Bulk-upload code

        public DataSet BindStockMode()
        {
            SqlParameter[] objSqlParam = new SqlParameter[1];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcGetStockMode", objSqlParam);
            Error = Convert.ToString(objSqlParam[0].Value);
            return dsResult;
        }

        public Int16 UploadSalesOrderAllocationDetail(DataTable Dt)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                SqlConnection conStr = new SqlConnection(strConStr);
                Result = 1;
                SqlParameter[] objSqlParam = new SqlParameter[8];
                objSqlParam[0] = new SqlParameter("@tvpSalesOrderAllocationDetail", SqlDbType.Structured);
                objSqlParam[0].Value = Dt;
                objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                objSqlParam[2].Direction = ParameterDirection.Output;
                objSqlParam[3] = new SqlParameter("@CreatedBy", CreatedBy);
                objSqlParam[4] = new SqlParameter("@StockUpdateError", SqlDbType.Xml);
                objSqlParam[4].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(strSalesInvoiceXML, XmlNodeType.Document, null));
                objSqlParam[4].Direction = ParameterDirection.Output;
                objSqlParam[5] = new SqlParameter("@SaleOrderPickListNumber", SqlDbType.VarChar, 500);
                objSqlParam[5].Direction = ParameterDirection.Output;
                objSqlParam[6] = new SqlParameter("@LogedInEntityID", LoggedInEntityID);
                objSqlParam[7] = new SqlParameter("@ToEntityID", ToID);
                //SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcSaleOrderAllocationDetail_Update", objSqlParam);
                cmd = new SqlCommand("prcSaleOrderAllocationDetailBulkUpload", conStr);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(objSqlParam);
                da.SelectCommand = cmd;

                DataSet dsResult = new DataSet();
                da.SelectCommand.CommandTimeout = 200;
                da.Fill(dsResult);
                Error = Convert.ToString(objSqlParam[1].Value);
                Result = Convert.ToInt16(objSqlParam[2].Value);
                strSalesInvoiceXML = null;
                if (((System.Data.SqlTypes.SqlXml)objSqlParam[4].Value).IsNull != true)
                {

                    strSalesInvoiceXML = ((System.Data.SqlTypes.SqlXml)objSqlParam[4].Value).Value;

                }
                if (Convert.ToString(objSqlParam[5].Value) != "")
                    SalesOrderPickListNumber = Convert.ToString(objSqlParam[5].Value);
                return Result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion
        /*#CC39 Added end */

        /*#CC42 Added start */
        public DataTable GetAllPartGroupName()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[1];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcPartGroup_Select", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[0].Value);
            return dtResult;
        }
        /*#CC42 Added end */

        /* #CC43 Added Start */
        public DataTable GetDealerDetails()
        {
            DataTable dtResult = new DataTable();
            DataSet dsResult = new DataSet();
            SqlParameter[] objSqlParam = new SqlParameter[2];
            objSqlParam[0] = new SqlParameter("@EntityTypeID", SelectedEntityTypeID);

            dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcGetDealerDetails", objSqlParam);

            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];

            return dtResult;
        }
        /* #CC43 Added End */

        /* #CC44 Added Start */
        public DataSet ProductPartDownLoadAll()
        {
            SqlParameter[] objSqlParam = new SqlParameter[2];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@OrderFromId", FromID);
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcProductPartMappingDownloadActive", objSqlParam);

            Error = Convert.ToString(objSqlParam[0].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }

            return dsResult;
        }
        /* #CC44 Added End */



        /* #CC45 Added Start */
        public DataTable DirectPartSalesInsert()
        {
            try
            {
                DataTable dtResult = new DataTable();
                //int result = -1;
                SqlParameter[] objSqlParam = new SqlParameter[11];
                objSqlParam[0] = new SqlParameter("@Tvp_SalesPart", SqlDbType.Structured);
                objSqlParam[0].Value = dtSoDetail;
                objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                objSqlParam[2].Direction = ParameterDirection.Output;
                objSqlParam[3] = new SqlParameter("@createdby", CreatedBy);

                objSqlParam[4] = new SqlParameter("@ToEntityId", ToID);
                objSqlParam[5] = new SqlParameter("@FromEntityId", FromID);
                objSqlParam[6] = new SqlParameter("@StockModeMasterID", StockModeId);
                objSqlParam[7] = new SqlParameter("@RefNumber", ReferenceNo);
                objSqlParam[8] = new SqlParameter("@remarks", Remarks);
                if (dtCustDetail != null && dtCustDetail.Rows.Count > 0)
                {
                    objSqlParam[9] = new SqlParameter("@TVP_CustomerDetails", SqlDbType.Structured);
                    objSqlParam[9].Value = dtCustDetail;
                }

                objSqlParam[10] = new SqlParameter("@ReceiptNo", SqlDbType.VarChar, 500);
                objSqlParam[10].Direction = ParameterDirection.Output;

                DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcDirectPartSales", objSqlParam);
                Error = Convert.ToString(objSqlParam[1].Value);
                Result = Convert.ToInt16(objSqlParam[2].Value);

                if (dsResult != null && dsResult.Tables.Count > 0)
                {
                    dtResult = dsResult.Tables[0];
                    ReceiptNo = Convert.ToString(objSqlParam[10].Value);
                }

                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /* #CC45 Added End */
        /* #CC46 Add Start */
        public DataSet GenerateMultiStoresalesOrder()
        {
            // Int16 result = 1;
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[5];
            objSqlParam[0] = new SqlParameter("@GroupEntityID", GroupEntityID);
            objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@SessionId", SessionId);
            objSqlParam[4] = new SqlParameter("@UserId", CreatedBy);
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcGenerateMultiStoreSaleorder", objSqlParam);
            Result = Convert.ToInt16(objSqlParam[2].Value);
            Error = Convert.ToString(objSqlParam[1].Value);
            return dsResult;
        }
        /* #CC46 Add End */

        /*#CC55:added (start)*/
        public DataTable SelectPurposeofMaterial()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[6];
            objSqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[1] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[2].Direction = ParameterDirection.Output;
            objSqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@OrderFromEntityID", FromID);
            objSqlParam[5] = new SqlParameter("@OrderTOEntityID", ToID);
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcSelect_OrderPurpose", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[2].Value);
            return dtResult;
        }
        /*#CC55:added (end)*/

        /*#CC56:Added Start*/

        public DataTable EditSalesInvoiceUpload()
        {
            try
            {
                DataTable dtResult = new DataTable();
                SqlParameter[] objSqlParam = new SqlParameter[6];
                objSqlParam[0] = new SqlParameter("@CreatedBy", CreatedBy);
                objSqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                objSqlParam[1].Direction = ParameterDirection.Output;
                objSqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
                objSqlParam[2].Direction = ParameterDirection.Output;
                objSqlParam[3] = new SqlParameter("@TVPEditSalesInvoiceUpload", SqlDbType.Structured);
                objSqlParam[3].Value = EditSalesInvoice;
                objSqlParam[4] = new SqlParameter("@OrignalfileName", OrignalfileName);
                objSqlParam[5] = new SqlParameter("@DiscountType", DiscountType);
                DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcEditSalesInvoiceBulkUpload_Insert", objSqlParam);
                if (dsResult != null && dsResult.Tables.Count > 0)
                    dtResult = dsResult.Tables[0];
                OutParam = Convert.ToInt16(objSqlParam[1].Value);
                Error = Convert.ToString(objSqlParam[2].Value);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet InvoiceReferenceData()
        {
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@InvoiceNo", InvoiceNumber);
            objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[2].Direction = ParameterDirection.Output;
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcSaleInvoiceReferenceData", objSqlParam);
            OutParam = Convert.ToInt16(objSqlParam[2].Value);
            Error = Convert.ToString(objSqlParam[1].Value);
            return dsResult;
        }

        public DataSet GetSKUList()
        {

            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcGetSKUList");
            return dsResult;
        }

        /*#CC56:Added End*/


    }
}
