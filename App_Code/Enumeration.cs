/*
 ====================================================================================================
Modification On       Modified By          Change Code        Modification 
---------------      -----------          -----------------   --------------------------------------------  
04-oct-2011         Amit agarwal            CC#-01           modified   EnumBatteryQCLocation
09-Nov-2011         amit agarwal            CC#-02          modified    EnumSelectionMode
09-Nov-2011         Rakesh Goel             CC#-03          modified    EnumDispatchType
08-dec-2011         Amit Agarwal            CC#-04          Added       AnchorType, EntityTypeKeyword
17-March-2012       Pankaj Dhingra          CC#-04          Added       AnchorType, EntityTypeKeyword
27-March-2014       Shilpi Sharma          CC#-05          Added         increase value in EnumJobsheetPartRequiredStatus tag
11-Junep-2014       Ajeet Mishra           cc#-06          Added         increase value in EnumBatteryTestType tag      
03-July-2014       Shilpi Sharma           cc#-07          Added       increase key in EntityTypeKeyword tag "STOCKTRANSFER"
28-Aug-2014        Ajeet Mishra            CC#-08          Added      Added Transaction name
9-july-2015        Poonam                  CC#-09          Remove     I have removed luminous form  EnumCustomerProductSearchType and EnumCustomerSearchType.
08-May-2018        Shashikant Singh        CC#-10          Added      Jobsheet Closure status for firstdata
 */

namespace ZedAxis.ZedEBS.Enums
{
    public enum EnumCustomerType
    {
        NONE = 0,
        CUSTOMER = 1,
        DEALER = 2
    }

    public enum EnumUserDetailType
    {
        NONE = 0,
        APPLICATIONUSER = 1,
        CUSTOMER = 2,
        COMPANY = 3,
        ENTITY = 4,
        DEALER = 5
    }
    public enum JobType
    {
        Internal = 1,
        External = 2
    }

    public enum EnumCustomerSearchType
    {
        AMC_about_to_expire_expired = 1,
        Out_of_waranty_customer = 2,
        /*CC#-09 Commented start Luminous_Inverter_one_year_old_customer_with_any_battery = 3,CC#-09 Commented end*/
        /*CC#-09 Added start*/
        Inverter_one_year_old_customer_with_any_battery = 3,
        /*CC#-09 Added start*/
        New_Luminous_battery_or_Inverter_owner = 4,
        Call_Now = 5,
        Call_Later = 6,
        Re_Appoinment_Calls = 7,
    }

    public enum EnumStockStatus
    {
        Good = 1,
        Defective = 2,
        Scrap = 3
    }

    public enum EnumCustomerSourceType
    {
        COMPLAINT = 1,
        WARRANTY = 2
    }
    public enum EnumFeedBack
    {
        //Important Note: entries should exist in AMCLeadStatusMaster Table as well
        //Manual to be extended in ViewOutboundCallHistory.aspx as well in ddcallLeadStatus
        Interested = 6,
        Not_Interested = 2,
        Wrong_Number = 3,
        Call_Later = 4,
        No_Response = 5,
        AMC_Already_Collected = 8,
    }

    public enum EnumFeedBackForAllocatedCalls
    {
        //Select=1,
        //Feedback_Awaited=1,
        //Important Note: entries should exist in AMCCollectionStatusMaster Table as well
        House_Lock = 2,
        No_Responsible_Person = 3,
        Visit_Later = 4,
        Could_Not_Visit = 5,
        System_out_of_Order = 6,
        Refused_to_Buy = 7,
        AMC_Sold_Out = 8

    }
    public enum EnumEntityType
    {
        None = 0,
        Distributor = 1,
        Service_Center = 2,
        Dealer = 3,
        Call_Center = 4,
        Transport = 5,
        RTB_Battery = 6,
        Sales_Branch = 7,
        Admin = 8,
        Parts_Vendor = 9,
        Head_Office = 10,
        Service_Region = 11,
        Service_Zone = 12,
        Service_Branch = 13,
        Sales_Region = 15
    }

    public enum EnumInteractionType
    {
        NONE = 0,
        INBOUND = 1,
        OUTBOUND = 2,
        LRRECEIVE = 3,
        WALKIN = 4,
        AUTOPROCESS = 5,
        DATAENTRY_WARRANTYREGISTER = 6
    }

    public enum EnumJobSheetType
    {
        REPAIR = 1,
        WARRANTY = 2,
        AMC = 3
    }

    public enum EnumJobSheetSource
    {
        None = 0,
        Call = 1,
        Walkin = 2,
        Transport = 3
    }

    public enum EnumCityPowerZone
    {
        A = 0,
        B = 1,
        C = 2,
        D = 4
    }

    public enum EnumCityGroup
    {
        OwnServiceCenter = 1,
        EngineerAvailable = 2,
        EngineerVisit = 3,
        EngineerVisitOnCall = 4
    }

    public enum EnumWeekDays
    {
        Sunday = 1,
        Monday = 2,
        Tuesday = 3,
        Wednesday = 4,
        Thursday = 5,
        Friday = 6,
        Saturday = 7
    }

    public enum EnumPartStatus
    {
        PartRequested = 1,
        PartComsumed = 2,
        PartNotRequired = 3
    }

    public enum EnumDSRStatusMode
    {
        Unproductive = 0,
        Productive = 1,
        RTB = 2
    }

    public enum EnumDSRStatus
    {
        NONE = 0,
        NOT_ATTEMPTED = 1,
        CREATED = 2,
        ELECTRICITY_NOT_AVAIALABLE = 3,
        DOOR_LOCK = 4,
        CUSTOMER_NOT_READY_TO_PAY = 5,
        PERSON_NOT_AT_HOME = 6,
        SKILL_SET_NOT_MATCHED = 7,
        PREVENTIVE_MAINTENANCE = 8,
        ELECTRICITY_FAULT = 9,
        RTB_INVERTER = 10,
        RTB_BATTERY = 11,
        PARTS_NOT_AVAILABLE = 12,
        REPAIRED_WITH_PART = 13,
        REPAIRED_WITHOUT_PART = 14
    }
    public enum EnumStandByUnitStatus
    {
        NOT_ISSUED = 0,
        ISSUED = 1,
        RECEIVED_BACK = 2,
        STANDBY_REQUIRED = 3
    }

    public enum EnumBrandType
    {
        //Service_AMC_done=1,
        //Only_AMC_done=2,
        //NoService_AMC_done=3
        Serviced_By_Company = 1,
        Maintained_By_Company = 2,
        Not_Serviced_By_Company = 3,
        Part_Vendor_Brand = 4               /* added by Prashant Chitransh on 12-Aug-2011 */
    }

    public enum VirtualEntityType
    {
        CallCenter = 1,
        ServiceCenter = 2,
        Rtb = 3
    }
    public enum EnumUploadPageName
    {
        StockAdjustment = 0

    }

    public enum EnumCustomerProductSearchType
    {
        AMC_Expired = 1,
        Out_of_warranty_Customers = 2,
        /*CC#-09 added start*/
        Inverter_Customer_with_any_battery = 3,
        /*CC#-09 added End*/
        /*CC#-09 Commented Start*/
        //Luminous_Inverter_Customer_with_any_battery = 3,
        /*CC#-09 Commented End*/
        New_Luminous_battery = 4

    }
    public enum EnumEngineerType
    {
        FieldEngineer = 2,
        InhouseEngineer = 3
    }
    public enum EnumBatteryTestType
    {

        Field_Process = 0,   //#cc#-06 Added
        GRN = 1,
        BENCH_CHARGING = 2,
        INVERTOR_LOAD = 3
    }
    public enum EnumTaxType
    {
        Tax_Added_on_Value = 1,
        Tax_Added_on_Tax_Value = 2,
        Tax_Deduction = 3
    }

    public enum EnumTaxApplicableOn
    {
        Default_for_Finished_Good_FG = 0,
        Accessory = 1,
        Part = 2,
        Labour = 3

    }

    public enum EnumApplicableForType
    {
        ProductCategory = 1,
        Product = 2,
        All = 0
    }
    public enum EnumProductCategory
    {
        None = 0,
        Inverter = 1,
        Battery = 2,
        Gas_Geyser = 3,
        Inverter_Cum_UPS = 4,
        LPG_Geyser = 5,
        Off_Line_UPS = 6,
        On_Line_UPS = 7,
        Water_Purifier = 8,
        Fan = 16
    }

    public enum EnumIsPartRequired
    {
        Default = 0,
        Part_info_required_but_part_not_consumed = 2,
        Part_info_required_and_part_consumed = 1,
        Part_info_not_required = 3,
        Part_pending = 4

    }

    public enum EnumIsRTB
    {
        No = 0,
        Yes = 1
    }

    public enum EnumRTBUnitStatus
    {
        RTB_NotRequired = 0,
        Pickup_Required = 1,
        Picked = 2,
        Received_ByRTBCenter_Pickup = 3,
        Customer_Drop = 4,
        Received_ByRTBCenter = 5
    }

    public enum EnumApplicationUsedServicedBy
    {
        self = 1,
        incorporate = 2
    }

    public enum EnumWarrantyStatus
    {
        Inwarranty = 0,
        AMC_Warranty = 1,
        Prorata_warranty = 2,
        Out_of_Warranty = 3,
        Undefined = 4,
        Unsold = 5                      /* Added By Prashant on 03-Aug-2011 for TRCDefectiveReceiving */
    }
    /*CC#-10:Added Start*/
    public enum EnumJobsheetClosureType
    {
        STP = 0,
        NSTP = 1,
        WT = 2,
        BT = 3,
        NA = 4
    }

    /*CC#-10:Added End*/

    public enum EnumIndentStatus
    {
        Pending = 0,
        Approved = 1,
        Reject = 2
    }
    public enum BindBucket
    {
        BySearch = 0,
        ByTree = 1,
        ByExport = 2
    }

    public enum PaymentMode
    {
        Cash = 1,
        Cheque = 2,
        TDS_Deducted = 3,
        RTGS__NEFT = 4, /*__ means / */
        Demand_Draft = 5
    }

    public enum PaymentClearenceStatus
    {
        Under_Process = 1,
        Cleared = 2,
        Not_Cleared = 3
    }

    public enum EnumDiscrepancyCheckType
    {
        At_time_of_GRN = 1,
        Reported_in_BTR = 2
    }

    public enum EnumDiscrepancyCheckCategory
    {
        Undefined = 0,
        System = 1,
        Document = 2,
        Commercial = 3,
        Physical = 4,
        BTR = 5
    }

    public enum EnumPaymentType
    {
        Invoice = 1,
        Jobsheet = 2
    }

    public enum FreightStatus
    {
        Paid = 0,
        To_Pay = 1
    }

    public enum EnumDSRVerificationStatus
    {
        //0 - Unverified, 1- Verified, 2 - Verified with mismatch
        Unverified = 0,
        Verified = 1,
        Verified_with_mismatch = 2
    }

    public enum EnumProductVerified
    {
        //0 - Unverified, 1- Verified, 2 - Partially Verified
        Unverified = 0,
        Verified = 1,
        Partially_Verified = 2
    }
    public enum EnumCartStatus
    {
        //0 - Unverified, 1- Verified, 2 - Partially Verified
        Valid = 0,
        AlreadyAdded = 1,
        MultipleCustomersProduct = 2
    }
    public enum EnumKpiChartType
    {
        //0 - Unverified, 1- Verified, 2 - Partially Verified
        None = 0,
        Battery = 1,
        NonBattery = 2
    }
    public enum EnumReportType
    {
        NONE = 0,
        DAILY_PERFORMANCE = 1,
        FIELD_TAT = 2,
        INHOUSE_BATTERY_TAT = 3,
        PAYMENT_COLLECTION = 4,
        DAILY_PERFORMANCE_Open = 5

    }
    public enum EnumEmailOutboundTrans
    {
        None = 0,
        Nightly_Reports = 1
    }
    #region Enum for SAP Service
    public enum EnumSAPModuleName
    {
        SAPItemMaster = 0,
        Indent = 1,
        Invoice = 2,
        ChequeCollection = 3,
        StockTransferWithInvoice = 4,
        StockTransferWithoutInvoice = 5,
        GrnFrmstockofEntityReceive = 6,
        PartConsumption = 7,
        DefectiveReceived = 8,
        RepairInvoice = 9,
        AMCInvoice = 10,
        SapCashCollection = 11,
        GenerateAutoBMTJobSheet = 12,
        CalculateAverageConsumptionForROL = 13
    }

    public enum EnumSAPLogType
    {
        SuccessWithData = 1,
        Failure = 2,
        Error = 3,
        SuccessWithoutData = 4
    }
    public enum EnumSAPLogTypeForSMS
    {
        Pending = 1,
        Sucesss = 2,
        Failure = 3,
        Invalid = 4,// can be for blank text
        Error = 5
        //0=Pending,1=Sucesss, 2=Failure,3=Invalid,4=Error
    }

    public enum EnumTypeForOutBoundSMS
    {
        CA = 1,
        PR = 2,
        LR = 3,
        LRPick = 4,// can be for blank text
        ClmAcptRjt = 5,
        ClamAccDis = 6
        //0=Pending,1=Sucesss, 2=Failure,3=Invalid,4=Error
    }
    public enum EnumSMSTransName
    {
        select = 0,
        Call_Allocation = 1,
        Payment_Received = 2,
        Customer_LR_Received = 3,
        LR_GRN_Done = 4,
        Battery_Claim_Accept_Reject = 5,
        Battery_Claim_Accepted_and_Dispatched = 6,
        /*#CC#-08  added  Start*/
        NewCall = 7,
        CallCreated = 8,
        CallClosed = 9,
        EnquiryCreation = 10,
        InternalApproval = 11,
        THmodifyOrder = 12,
        OrderStatus = 13,
        MarkDamage_hort_Batch_Receive = 14
    /*#CC#-08  added  end*/

    }

    public enum EnumJobsheetPartRequiredStatus
    {
        Default = 0,
        Part_InfoRequired_Consumed = 1,
        Part_InfoRequired_Not_Consumed = 2,
        Part_InfoNotRequired = 3,
        Part_Pending = 4,
        PartPendingPartInfoNotRequired = 5,
        //Product_Accessory_Consumed = 5,/*CC#-05:added*/
        Product_Swap = 6,/*CC#-05:added*/
        Product_Box_Swap = 7/*CC#-05:added*/
        //0-Default; 1-part info required and part consumed, 
        //2-part info required but part not consumed (part cleaning), 3- Part info not required, 
        //4 = part pending
    }

    public enum EnumLetterGenerateStatus
    {

        Letter_Not_Generated = 3,
        Letter_Generated = 4
    }
    #endregion

    public enum EnumHelpdeskTicketSource
    {
        None = 0,
        Call = 1,
        Email = 2
    }

    public enum EnumTargetFor
    {
        Engineer = 1,
        AMC_Telecaller = 2,
        Field_Collection_Executive = 3      /* added on 30-06-2011 by Prashant Chitransh */
    }

    public enum EnumMonths
    {
        Janaury = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }

    public enum EnumtargetApprovalStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }

    public enum EnumBatteryQCStatus
    {
        NotRequired = 0,
        Pending = 1,
        Done = 2
    }

    public enum EnumBatteryTestCategory
    {
        Undifined = 0,
        Design = 1,
        Manufacturing = 2,
        Service = 3
    }

    public enum EnumBatteryQCLocation
    {
        Undifined = 0,
        //   Factory = 1,    // commented  CC#-01
        Factory = 17, //  added CC#-01
        ServiceCenter = 2
    }

    public enum EnumTaxGroup
    {
        VAT = 1,
        Service_Tax = 2,
        TDS = 3,
        CST = 4
    }

    public enum EnumTDSCertificatereceiveStatus
    {
        Received = 1,
        Pending = 2
    }

    public enum EnumDispatchType
    {
        Service_Center_Transfer = 1,
        Direct_GRN = 2,                     //CC#-03 added
        Against_Indent = 3,
        Pick_Up_GRN = 4,
        Walkin_GRN = 5,
        Transport_GRN = 6,
        Stand_by_GRN = 7,
        Stand_by_GRN_On_Pickup_Cancel = 8,  //CC#-03 added
        Dealer_defective = 9               /* Added by Prashant on 06-Aug-2011 */
    }

    public enum EnumHolidayType
    {
        Entity_Specific = 1,
        For_All_Entity = 2
    }

    /* Added by Prashant on 05-Aug-2011 */
    public enum EnumModeOfDefectiveReceive
    {
        By_hand = 1,
        By_Courier = 4
    }

    public enum EnumInvoiceStatus
    {
        Generated = 1,
        Not_Generated = 0
    }

    public enum EnumSelectionMode // #CC-02
    {
        // Summary:
        //     Only one list box item can be selected at once.
        Single = 0,
        //
        // Summary:
        //     Multiple items can be selected within the edtior by clicking list items while
        //     pressing Ctrl (to add an individual item) or Shift (to select a range of
        //     items).
        Multiple = 1,
        //
        // Summary:
        //     Multiple items can be selected within the editor by clicking specific check
        //     boxes or list items (the Shift key can also be used in this mode to select
        //     a range of items).
        CheckColumn = 2,
    }
    public enum AnchorType
    {
        Service = 1,
        Sales = 2,
        Other = 3
    }


    public enum EntityTypeKeyword
    {
        //FWDLOG,
        //REVLOG,
        //DIRECTGRN,
        //SERVICETREE,
        //SALETREE,
        //SALECHANNEL,
        //BILLFROM,
        //RTBALLPRD,
        //RTBBATTERY,
        //CHILDSCV

        FWDLOG,
        REVLOG,
        DIRECTGRN,
        ORGTREE,
        SALECHANNEL,
        CHANNELGROUP,
        BILLFROM,
        RTBALLPRD,
        RTBBATTERY,
        PJP,
        EscalateJob,
        DOAREVLog,
        STOCKTRANSFER/*cc#-07    :added*/
    }
    public enum EnumPartType
    {
        Fast_Moving = 1,
        Slow_Moving = 2,
        Non_Moving = 3
    }

    public enum EnumSerialized
    {
        No = 0,
        Yes = 1,
        BatchCode = 2
    }

    public enum EnumOwnership
    {
        NotSpecified = 0,
        Manufactured = 1,
        Purchased = 2
    }

    public enum EnumApplicationWorkingMode
    {
        Not_Defined = 0,
        Online = 1,
        Excel_Import = 2,
        Tally_Import = 3
    }
    public enum EnumGroupType
    {
        No_Group = 0,
        //Head_Of_Group = 1,          //Due to change in the Requirement this would be removed
        Child_Of_Group = 2
    }

    public enum BusinessEventsKeyword
    {
        SRFSC = 1,
        SRFCC = 2,
        SOGENERATE = 3,
        SOCRCHECK = 4,
        SOALLOCATE = 5,
        Consigner = 6,
        Consignee = 7,
        PAYIN = 8,
        DRCRNOTE = 9,
        SOTRANSFER = 10,
        RETURN = 11,
        SOPARENTCHECK = 12,
        SOCANCELAPPROVAL = 14
    }

    public enum EntityTypeDescription
    {
        ExcludingEntityType = 1,        
        IncludingEntityType = 2  //Pankaj Dhingra
    }
    public enum eTargetTemplateType
    {
        eSummary = 1,
        eSKUWise = 2
    };          //for Target
    public enum eTemplateCount
    {
        ePrimarysales1 = 2,
        ePrimarysales2 = 2,
        eSecondary = 2,
        eRetailer = 3,
        ePrice = 1,
        eScheme = 2,
        eTarget = 4

    };
    public enum eUploadExcelValidationType
    {

        eRetailerUpload = 0,
        eSales = 1,
        ePriceUpload = 2,
        eScheme = 3,
        eTarget = 4

    };


    /* Added by Prashant on 05-Mar-2012 for untested unit process */
    public enum EnumDecisionRequire
    {
        Replace_the_Unit = 1,
        Test_the_Unit_in_Field = 2,
        Test_the_Unit_on_Desk = 3//,
        //Close_the_Job = 4
    }

    /* Added by Prashant on 07-Mar-2012 to set usercontrol display mode. */
    public enum EnumPageMode
    {
        Insert = 1,
        Update = 2,
        View = 3,
        UpdateView = 4
    }
    /* CC#-05 Add Start*/
    public enum EnumPartMasterSysGroup
    {

        Charger = 1,
        Document = 2
    }
    public enum EnumPartMasterIsSerialized
    {
        NonSerialized = 0,
        Serialized = 1,
        BatchNo = 2

    }
    public enum EnumPartCategory
    {
        A,
        B,
        C,
        D
    }
    public enum EnumVehicle
    {
        Bike=1,
        Car=2
    }
    /* CC#-05 Add End*/


}