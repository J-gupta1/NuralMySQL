using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*08-Feb-2018,Vijay Kumar Prajapati  #CC01-Add Enum for download referencecode. 
08-July-2018,Rajnish Kumar  #CC02-Target Product Category wise. 
*19-Dec-2018,Rakesh Raj, #CC03- Imported from ZEDERP 
 *02-Mar-2019, Balram Jha, #CC04 - added enum for product key reference 
 *17-Nov-2022, Rinku Sharma, #CC05 - added enum for Template Category Refrence 
 */

namespace DataAccess
{
   public static class EnumData
    {
        /*#CC03*/
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
       /*#CC03*/

       public enum eSearchCondition
       {
           All = 0,
           Like = 1,
           DefaultCondition = 2,
           SpecificID = 3,
           Delete = 4,
           UpdateStatus = 5
       };
       public enum eAutoCompleteType
       {
           PurchaseInvoice = 0,
           PurchaseReturnNumber = 1
       };
       public enum eExcludedModels
       {
           SKU = 1,
           Model = 2,
           Brand = 3,
           Product = 4
       }

       public enum eSalesChannelLevel
       {
           Warehouse = 1,
           SDLevel = 2,
           TdLevel=3
         
       };
       public enum eSalesChannelType
       {
           SS = 6,
           TD=7,
           DirectTD=9
          

       };
       public enum eSearchConditions
       {
          Inactive=0,
          Active=1,
          All=2

       };
       public enum eUploadExcelValidationType
       {
           
           eRetailerUpload = 0,
           eSales = 1,
           ePriceUpload=2,
           eScheme = 3,
           eTarget = 4
          
       };

       public enum eControlRequestTypeForEntry
       {
           eGRN = 0,
           ePrimary1Sales = 1,
           ePrimary2Sales = 2,
           eSecondarySales = 3,
           eStockTransfer = 4,
           eRetailer=5,
           ePrice = 6,
           eOrder=7,
           eScheme = 8,
           eTarget = 9,
           eProductKey = 10,//#CC04 added
           eCurrentStock = 11// Current Stock upload
          
       };

       
       public enum eTemplateCount
       {
           ePrimarysales1 = 2,
           ePrimarysales2 = 2,
           eSecondary = 2,
           eRetailer=3,
           ePrice=1,
           eScheme = 2,
           eTarget= 4,
           eNothing=-1,
           eDoauploadReferencecode=2/*#CC01 Added*/

       };
      
       public enum eSchemeTemplateType
       {
           eSummary = 1,
           eSKUWise = 2
       };
       public enum eTargetTemplateType
       {
           eSummary = 1,
           eSKUWise = 2,
           eProductCategoryWise=3,
           eWOD = 4 /* #CC05 */
           /*#CC02*/
       };
       public enum eEntryType
       {
           eInterface = 1,
           eUpload = 2
       };
       public enum eReportType
       {
           PrimarySales = 1,
           PrimarySales2 = 2,
           SecondarySales = 3
       };
       public enum EnumSAPLogType
       {
           SuccessWithData = 1,
           Failure = 2,
           Error = 3,
           SuccessWithoutData = 4
       };
       public enum EnumSAPModuleName
       {
           BTMDataUpload = 1,
           MODDataUpload=2,
           IMEIDataUpload=3,
           GRNDataUpload=4,
           NoFileToUpload=0,
           ExceptionOccured=5

       };
       public enum EnumSAPMethodName
       {
           GRNData=1,
           MODData=2,
           IMEIData=3,
           BTMData=4,
           Downloading_Uploading=5

       };
     
       public enum SchemeComponentType
       {
           eTargetPurchaseQty = 10,
           eTargetPurchaseValue = 11,
           eTotalPurchaseQty = 12,
           eTotalPurchaseValue = 13,
           eSKUWisePurchaseQty = 14,
           eSKUWisePurchaseValue = 15,
           eTotalSalesQty = 16,
           eTotalSalesValue = 17,
           eSKUWiseSalesQty = 18,
           eSKUWiseSalesValue = 19

       };
       public enum eRequestforDeleteStatus
       {
           eStatus = 1,
           eDelete = 2
       };
       public enum ModelType
       {
          Saleable=1, 
          NonSaleable=2
       };
       public enum SerializedMode
       {
           No_SerialBatch_Number = 1,
           Batch_Coded = 2,
           Unique_Serial_Number=3
       };

       public enum RoleType
       {
           Retailer = 1,
           ISD = 2
       };
    
    }
}
