using System;
using System.IO;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Collections;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;
using System.Xml;
using clsException;
using DataAccess;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using ZedService;



//using Tamir.SharpSsh;
//using Tamir.SharpSsh.jsch;
/*Change Log:
 * 23-Jun-14, Rakesh Goel, #CC01 - Mechanism for redirecting user to login page modified on session expiry. 
 * Response.Code 301 is problematic
 * 09-Jul-14, Rakesh Goel, #CC02 - File name generation code changed from time basis to guid as duplicate file name is possible with time approach
 * 05-Aug-14, Rakesh Goel, #CC03 - Changes for transaction module restriction 
 * 22-Dec-14, Karam Chand Sharma, #CC04 - Read config value for bulk upload GRNand promary
 * 08-Jan-15, Sumit Kumar, #CC05 - Read config value PRIMARYSALESBCP,SALESMANOPTIONAL for Primary Upload and Add Retailer
 * 24-Jan-15, Karam Chand Sharma, #CC06 - Add nes session value form "DirectSaleReturn" config key
 * 10-Mar-15, Karam Chand Sharma, #CC07 - Read config value AREAOPTIONAL for add retailer interface
 * 27-May-2015,Karam Chand Sharma, #CC08, Add some properties and store menu in hashtable (for zedcontrol functionality)
 * 09-Jun-2015,Karam Chand Sharma, #CC09, Add new key in session for retailer default setting
 * 28-oct-2015,Karam Chand Sharma, #CC10, Add new key in session for retailer bank  default setting name with RetailerBankDetail
 * 17-Mar-2016, Sumit Maurya, #CC11, New key in session added for Counter Potential Value display and mandatory.
 * 20-Mar-2016, Karam Chand Sharma , #CC12, New new key in session for Added for TehsillDisplayMode.
 * 18-Apr-2016, Sumit Maurya, #CC13, New key in session added to allow multiple login in application according to configvalue .
 * 23-May-2016, Sumit Maurya, #CC14, New Key(s)  
 *                            "ALLOWEDITRETAILER" Added in session to make Name , State and City columns editable.
 *                            "STPARENTCHECK" added to allow parent channel should be same or not.
 * 31-May-2016, Karan Chand Sharma, #CC15, New Keys Added "SHOWCARTONSIZE" to show counter size
 *                                                        "REQCARTONSIZE" to apply validation  on Countersize.
 * 07-Jun-2016, Sumit Maurya, #CC16, New Key added "ChangeTinLabel" to change TIN No as VAT No.
 * 14-Jun-2016, Sumit Maurya, #CC17, New Key added "LOCATIONMAPPING".
 * 08-Aug-2016, Sumit Maurya, #CC18, New check added to display Retailer Excel upload interface according to column value.
 * 09-Aug-2016, Sumit Maurya, #CC19, New Key added "BRANDWISEBULLETIN".
 * 04-Nov-2016, Karam Chand Sharma, #CC20, New Key added "NDHIERARCHYMAPPING".
 * 26-Dec-2016, Karam Chand Sharma, #CC21, New Key added "ISLOGINEDITABLE".
 *  12-July-2017, Vijay Kumar Prajapati, #CC22, Fill CheckBox List For ValidateFinanceRequestType.aspx page".
 *  17-Jan-2018, Sumit Maurya, #CC23, Previous code was not working for sales.
 *  * 26-Feb-2018,Vijay Kumar Prajapati,#CC24,Add/Check Configration for send mail to parent organization Hierarchy if configration =1 then send email to parent or cofigaration=0 then not send mail to organization hierarchy.
 *  30-Mar-2018, Sumit Maurya, #CC25, New Key added "RETHERARCHYPARENTMANDATORY".
 *  23-Apr-2018, Sumit Maurya, #CC26, New Key added "SalesChannelInActive" (Done for Lemon).
 *  21-May-2018, Rajnish Kumar, #CC27, New Key added "RetailerCheckUpload and WhatsAppMobileNumber" (Done for Karbon).
 *  23-May-2018,Vijay Kumar Prajapati,#CC28,New Key added "UserLogingUsingOTP"(Done for Karbon).
 *  30-May-2018,Vijay Kumar Prajapati,#CC29,New Key added "SecondarySalesReturnApproval" and "IntermediarySalesReturnApproval" for salesreturnapproval.(Done for Karbonn.)
 *  18-June-2018,Rajnish Kumar,#CC30,Region Label Name according to RSM Configuration Value
 *  3-July-2018, Rakesh Raj, #CC31, Export to CSV Feature Added
 *  5-July-2018, Rakesh Raj, #CC32, Sales Channel PanNo Mandotory from database table -Configkey 'ISPANNOMANDATORY'
 *  06-July-2018,Vijay Kumar Prajapati,#CC33,Retailer Multiplemapping in view retailer hide some column using config for karbonn mobile.
 *  06-July-2018, Rakesh Raj, #CC34, Export to ExportInExcel(DataSet DsExport, string strFileName) added
 *  04-AUG-2018,Vijay Kumar Prajapati,#CC35,Added Number of BackDaysSalesReurn.
 *  02-Oct-2018, Sumit Maurya, #CC36, New propertiy, config added with session value for "ISPUniqueMobile"( Done for motorola).
 *  14-Nov-2018, Sumit Maurya, #CC37, New config added with session value for "EDITRETAILERNAME"(Done for motorola).
 *  12-Dec-2018, Balram Jha, #CC38- Response end on invalid menu access
 *  04-Jan-2019, Balram Jha, #CC39 - changed for Billing process
 *  30-May-2019,Vijay Kumar Prajapati,#CC40--Added for ExpiryDate When Added SKU (Done for shivalik.)
 *  30-Jan-2020,Vijay Kumar Prajapati,#CC41--Added PhysicalPath for SAPIntegration(Done for Inone).
 *  13-March-2020,Vijay Kumar Prajapati,#CC42--Added UserOtherDetail For AKAI.
 *  31-March-2020,Vijay Kumar Prajapati,#CC43--Added ClientId For Zedsales2.0
 */

namespace BussinessLogic
{
    public class PageBase : System.Web.UI.Page
    {

        static string _strPhysicalPath = string.Empty;      //not in use
        static string _strPhysicalPathUpload = string.Empty; //Not in use
        static string _strVirtualPath = string.Empty;  //Not in use
        public static string strExcelVirtualPath = string.Empty;
        public static string strExcelPhysicalDownloadPathSB = string.Empty;
        public static string strExcelPhysicalUploadPathSB = string.Empty;
        public static string strExcelPhysicalBlankTemplatePathSB = string.Empty;
        public static string strExcelPhysicalBulkGRNPrimary = string.Empty;
        public static string strExcelBulkUploadPath = string.Empty;/*#CC04 ADDED*/
        public static string strExcelBulkUploadPSIInfoPath = string.Empty;/*#CC41 ADDED*/
        public static string strExcelBulkUploadPSIInvoiceInfoPath = string.Empty;
        /*#CC08 START ADDED */
        private Int32 intMenuID;
        public Int32 MenuID
        {
            get { return intMenuID; }
            set { intMenuID = value; }
        }
        /*#CC08 START END */
        public static string PhysicalPath
        {
            get
            {
                if (HttpContext.Current.Session["PhysicalPath"] != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["PhysicalPath"]);
                }
                return "";
            }
            set
            {
                _strPhysicalPath = value;
            }
        }
        public static string PhysicalPathUpload
        {
            get
            {
                if (HttpContext.Current.Session["PhysicalPathUpload"] != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["PhysicalPathUpload"]);
                }
                return "";
            }
            set
            {
                _strPhysicalPathUpload = value;
            }
        }


        public static string VirtualPath
        {
            get
            {
                if (HttpContext.Current.Session["VirtualPath"] != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["VirtualPath"]);
                }
                return "";
            }
            set
            {
                _strVirtualPath = value;
            }
        }
        //#region Global Variables|Culture Settings
        // public static string strpbGlobalErrormessage = ConfigurationManager.AppSettings["errormessage"].ToString();
        public static string strGlobalDownloadExcelPathRoot = GetWebConfigKey("ExcelDownloadPath");
        public static string strGlobalBlankExportTemplateName = GetWebConfigKey("ExcelBlankTemlateName");
        public static string strGlobalUploadExcelPathRoot = GetWebConfigKey("ExcelUploadPath");
        public static string strAssets = GetWebConfigKey("AssetsPath");
        public static string strApplicationTitle = GetWebConfigKey("ApplicationTitle");
        public static string siteURL = GetWebConfigKey("siteurl");
        public static string redirectURL = GetWebConfigKey("RedirectUrl");
        public static string MailFrom = GetWebConfigKey("ApplicationTitle");
        public static string EmailIDFrom = GetWebConfigKey("mailfrom");
        public static string ForgotPasswordSubject = GetWebConfigKey("passwordsubject");
        public static String ConStr = ConfigurationManager.ConnectionStrings["AppConString"].ConnectionString;
        public static string KeyStr = GetWebConfigKey("KeyValue");  //For encrypt decrypt
        public static string GlobalErrorMsg = GetWebConfigKey("GlobalErrorMsg");  //For show error msg global =0
        public static string SapDirectoryPath = GetWebConfigKey("SapDirectoryPath");  //To Get the Path For the sap File upload
        public static string ValidExcelLength = GetWebConfigKey("ValidExcelLength");  //To Get the  file size (KB) allow user
        public static string ValidExcelRows = GetWebConfigKey("ValidExcelRows");  //To Get the number of rows allow to user for upload excel at one atttempt
        public static Int16 intBackDaysAllowForSS = Convert.ToInt16(GetWebConfigKey("BackDaysAllowForSS"));  //SS:Days relaxation for giving stock insertion
        public static Int16 intBackDaysAllowForTD = Convert.ToInt16(GetWebConfigKey("BackDaysAllowForTD"));  //TD:Days relaxation for giving stock insertion
        /////// Sap Integration FTP Global Variable
        public static string sFtpServerIP = GetWebConfigKey("FtpServerIP");  //To Get Server IP      
        public static string sFtpServerUserName = GetWebConfigKey("FtpServerUserName");  //To Get Server UserName
        public static string sFtpServerPassword = GetWebConfigKey("FtpServerPassword");  //To Get Server Password
        public static string sFtpServerRemoteDir = GetWebConfigKey("FtpServerRemoteDir");  //To Get Remote DirectoryName
        public static string Client = GetWebConfigKey("Client");
        public static string strBCPFilePath = GetWebConfigKey("BCPFilePath");  //To Get File Path of BCP File Export folder
        public static string PageSize = GetWebConfigKey("PageSize");
        public static string strExcelTemplatePathSB = GetWebConfigKey("ExcelTemplatePathSB");

        public static string strDefaultPassword = GetWebConfigKey("DefaultPassword");


        public static string ConfigKeysError;



        public static string GetWebConfigKey(string Key)
        {
            try
            {


                // ConfigurationManager.AppSettings[Key].ToString()
                return ConfigurationManager.AppSettings[Key].ToString();
            }
            catch (Exception ex)
            {

                ConfigKeysError = ex.Message + "Key:" + Key + " not found in webconfig.";
                // ConfigKeysError = ex.Message + "Key:" + Key + " not found in webconfig.";
                return ex.Message;
            }

        }


        protected void Page_Init(object sender, System.EventArgs e)
        {
            strExcelPhysicalDownloadPathSB = Server.MapPath(ConfigurationManager.AppSettings["ExcelPhysicalDownloadPathSB"].ToString());  //To get Excel physical path for Download
            strExcelVirtualPath = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + "/";
            strExcelPhysicalUploadPathSB = Server.MapPath(ConfigurationManager.AppSettings["ExcelPhysicalUploadPathSB"].ToString());  //To get Excel physical path for Upload
            strExcelBulkUploadPath = Server.MapPath(ConfigurationManager.AppSettings["ExcelBulkUploadPath"].ToString());  /*#CC04 ADDED*/
            strExcelPhysicalBlankTemplatePathSB = Server.MapPath(ConfigurationManager.AppSettings["ExcelPhysicalBlankTemlateNameSB"].ToString());  //To get Excel physical path for Upload
            strExcelBulkUploadPSIInfoPath = Server.MapPath(ConfigurationManager.AppSettings["PSIBulkUploadPath"].ToString());  /*#CC41 ADDED*/
            strExcelBulkUploadPSIInvoiceInfoPath = Server.MapPath(ConfigurationManager.AppSettings["DNInvoiceUploadPath"].ToString());  /*#CC41 ADDED*/
            strExcelPhysicalBulkGRNPrimary = Server.MapPath("~/Excel/Upload/BulkGRNPrimary/");
            GetvalidSession();
        }


        #region PropertyToGetRootPathForExportToExecel


        private static string strRootFilePath;
        private static string strPageHeading;
        private static EnumData.eControlRequestTypeForEntry eRequestType;
        public static EnumData.eControlRequestTypeForEntry RequestType
        {
            get { return eRequestType; }
            set { eRequestType = value; }
        }

        public static string RootFilePath
        {
            get { return strRootFilePath; }
            set { strRootFilePath = value; }
        }
        public static string PageHeading
        {
            get { return strPageHeading; }
            set { strPageHeading = value; }
        }

        public static string Fromdate = DateTime.Now.Month.ToString() + "/01/" + DateTime.Now.Year.ToString();
        public static string ToDate = DateTime.Now.ToShortDateString();


        #endregion


        #region CODE SNIPPET Added by Pradeep Kumar on 28.3.2011 to check page referesh from F5

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
            }

            ViewState["CheckRefresh"] = Session["CheckRefresh"];
        }
        public bool IsPageRefereshed
        {
            get
            {

                if (Session["CheckRefresh"].ToString() != ViewState["CheckRefresh"].ToString())
                {
                    return true;
                }
                else
                {

                    Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
                    return false;
                }

            }
        }

        #endregion
        #region Session Properties
        public static int UserId
        {
            get
            {
                if (HttpContext.Current.Session["UserID"] != null)
                    return Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
                else
                    return 0;

            }
        }
        /*#CC43 Added Started*/
        public static int ClientId
        {
            get
            {
                return Convert.ToInt32(HttpContext.Current.Session["CompanyId"].ToString());

            }
        }
        public static int RetailerEntityTypeID
        {
            get
            {
                return Convert.ToInt32(HttpContext.Current.Session["RetailerEntityTypeID"].ToString());

            }
        }
        /*#CC43 Added End*/
        //public static int OtherEntityType
        //{
        //    get
        //    {
        //        return Convert.ToInt32(HttpContext.Current.Session["OtherEntityType"].ToString());

        //    }
        //}
        public static int BaseEntityTypeID
        {
            get
            {
                return Convert.ToInt32(HttpContext.Current.Session["BaseEntityTypeID"].ToString());

            }
        }

        public static int EntityTypeID
        {
            get
            {
                return Convert.ToInt32(HttpContext.Current.Session["EntityTypeID"].ToString());

            }
        }
        public static int ApprovalLevel2
        {
            get
            {
                return Convert.ToInt32(HttpContext.Current.Session["ApprovalLevel2"].ToString());

            }
        }
         public static int PasswordExp
        {
            get
            {
                return Convert.ToInt32(HttpContext.Current.Session["PWDEXPY"].ToString());
            }
         }
        public static string DisplayName
        {
            get
            {
                return HttpContext.Current.Session["DisplayName"].ToString();
            }
        }


        public static int NumberofBackDaysAllowed           //Pankaj Dhingra
        {
            get
            {
                return Convert.ToInt32(HttpContext.Current.Session["NumberOfBackDaysAllowed"].ToString());
            }
        }
        /*#CC35 Added Started*/
        public static int BackDaysAllowedForSaleReturn           //Pankaj Dhingra
        {
            get
            {
                return Convert.ToInt32(HttpContext.Current.Session["BackDaysAllowedForSaleReturn"].ToString());
            }
        }
        /*#CC35 Added End*/
        public static Int32 HierarchyLevelID
        {
            get
            {
                if (HttpContext.Current.Session["HierarchyLevelID"].ToString() == "" || HttpContext.Current.Session["HierarchyLevelID"] == null)
                    return 0;
                else
                    return Convert.ToInt32(HttpContext.Current.Session["HierarchyLevelID"].ToString());
            }
        }//= 0;
        public static Int16 SalesChannelLevel
        {
            get
            {
                if (HttpContext.Current.Session["SalesChannelLevel"].ToString() == "" || HttpContext.Current.Session["SalesChannelLevel"] == null)
                    return 0;
                else
                    return Convert.ToInt16(HttpContext.Current.Session["SalesChannelLevel"].ToString());
            }
        }//= 0;
        public static bool AllowAllHierarchy
        {
            get
            {
                if (HttpContext.Current.Session["AllowAllHierarchy"].ToString() == "" || HttpContext.Current.Session["AllowAllHierarchy"] == null)
                    return false;
                else
                    return Convert.ToBoolean(HttpContext.Current.Session["AllowAllHierarchy"].ToString());
            }
        }//= 0;
        public static Int16 RoleID
        {
            get { return Convert.ToInt16(HttpContext.Current.Session["RoleID"].ToString()); }
        }//= 0;
        public static Int32 SalesChanelTypeID
        {
            get
            {
                if (HttpContext.Current.Session["SalesChanelTypeID"].ToString() == "" || HttpContext.Current.Session["SalesChanelTypeID"] == null)
                    return 0;
                else
                    return Convert.ToInt32(HttpContext.Current.Session["SalesChanelTypeID"].ToString()); ;
            }
        }// = 0;
        public static DateTime? SalesChannelOpeningStockDate
        {
            get
            {
                if (HttpContext.Current.Session["OpeningStockdate"].ToString() == "" || HttpContext.Current.Session["OpeningStockdate"] == null)
                    return null;
                else
                    return Convert.ToDateTime(HttpContext.Current.Session["OpeningStockdate"].ToString()); ;
            }
        }// = 0;
        public static DateTime? UserPasswordExpiredDate
        {
            get
            {
                if (HttpContext.Current.Session["PasswordExpired"].ToString() == "" || HttpContext.Current.Session["PasswordExpired"] == null)
                    return null;
                else
                    return Convert.ToDateTime(HttpContext.Current.Session["PasswordExpired"].ToString()); ;
            }
        }// = 0;
        public static Int32 SalesChanelID
        {
            get
            {
                if (Convert.ToString( HttpContext.Current.Session["SalesChannelID"]) == "" || HttpContext.Current.Session["SalesChannelID"] == null)
                    return 0;
                else
                    return Convert.ToInt32(HttpContext.Current.Session["SalesChannelID"].ToString()); ;
            }
        }// = 0;
        public static bool IsSuperAdmin
        {
            get
            {
                if (HttpContext.Current.Session["IsSuperAdmin"] == null || Convert.ToBoolean(HttpContext.Current.Session["IsSuperAdmin"].ToString()) == false || HttpContext.Current.Session["IsSuperAdmin"] == null)
                    return false;
                else
                    return true;
            }
        }// = 0;
        public static String SalesChanelCode
        {
            get
            {
                if (HttpContext.Current.Session["SalesChannelCode"] != null)
                    return Convert.ToString(HttpContext.Current.Session["SalesChannelCode"].ToString());
                else
                    return "";
                
            }
        }// = 0;
        public static Int32 ParentHierarchyLevelID
        {
            get { 
                if(HttpContext.Current.Session["ParentHierarchyLevelID"] !=null)
                return Convert.ToInt32(HttpContext.Current.Session["ParentHierarchyLevelID"].ToString()); 
                else
                    return 0;
            }
        } //= 0;

        public static Int32 Brand
        {

            get        //Pankaj Dhingra
            {
                if (HttpContext.Current.Session["Brand"].ToString() == "" || HttpContext.Current.Session["Brand"] == null)
                    return 0;
                else
                { return Convert.ToInt32(HttpContext.Current.Session["Brand"].ToString()); }
            }

        }

        //public static string MultipleBrandName
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Session["MultipleBrandName"].ToString() == "" || HttpContext.Current.Session["MultipleBrandName"] == null)
        //            return "";
        //        else
        //        { return (HttpContext.Current.Session["MultipleBrandName"].ToString()); }
        //    }

        //}
        public static string MultipleBrandName
        {
            get
            {
                if (Convert.ToString(HttpContext.Current.Session["MultipleBrandName"])== "" || HttpContext.Current.Session["MultipleBrandName"] == null)
                    return "";
                else
                { return (HttpContext.Current.Session["MultipleBrandName"].ToString()); }
            }

        }
        public static Int16 BackDaysAllowedOrder
        {
            get
            {
                if (HttpContext.Current.Session["BackDaysAllowedOrder"].ToString() == "" || HttpContext.Current.Session["BackDaysAllowedOrder"] == null)
                    return 0;
                else
                    return Convert.ToInt16(HttpContext.Current.Session["BackDaysAllowedOrder"]);
            }
        }
        public static Int16 BackDaysAllowedOpeningStock
        {
            get
            {
                if (HttpContext.Current.Session["BackDaysAllowedOpeningStock"].ToString() == "" || HttpContext.Current.Session["BackDaysAllowedOpeningStock"] == null)
                    return 0;
                else
                    return Convert.ToInt16(HttpContext.Current.Session["BackDaysAllowedOpeningStock"]);
            }
        }
        public static Int16 BackDaysAllowedBeforeOpening
        {
            get
            {
                if (HttpContext.Current.Session["BackDaysAllowedBeforeOpening"].ToString() == "" || HttpContext.Current.Session["BackDaysAllowedBeforeOpening"] == null)
                    return 0;
                else
                    return Convert.ToInt16(HttpContext.Current.Session["BackDaysAllowedBeforeOpening"]);
            }
        }


        public static Int16 IsPrimaryOrderNoAutogenerate
        {
            get
            {
                if (HttpContext.Current.Session["IsPrimaryOrderNoAutogenerate"].ToString() == "" || HttpContext.Current.Session["IsPrimaryOrderNoAutogenerate"] == null)
                    return 0;
                else
                    return Convert.ToInt16(HttpContext.Current.Session["IsPrimaryOrderNoAutogenerate"]);
            }
        }
        public static Int16 IsIntermediaryOrderNoAutogenerate
        {
            get
            {
                if (HttpContext.Current.Session["IsIntermediaryOrderNoAutogenerate"].ToString() == "" || HttpContext.Current.Session["IsIntermediaryOrderNoAutogenerate"] == null)
                    return 0;
                else
                    return Convert.ToInt16(HttpContext.Current.Session["IsIntermediaryOrderNoAutogenerate"]);
            }
        }
        public static Int16 IsRetailerStockTrack
        {
            get
            {
                if (HttpContext.Current.Session["RETAILERSTOCKTRACK"].ToString() == "" || HttpContext.Current.Session["RETAILERSTOCKTRACK"] == null)
                    return 0;
                else
                    return Convert.ToInt16(HttpContext.Current.Session["RETAILERSTOCKTRACK"]);
            }
        }

        /*#CC27 start*/
        public static int WhatsAppMobileNumber           //Pankaj Dhingra
        {
            get
            {
                return Convert.ToInt32(HttpContext.Current.Session["WhatsAppMobileNumber"].ToString());
            }
        }

        public static int RetailerCheckUpload           //Pankaj Dhingra
        {
            get
            {
                return Convert.ToInt32(HttpContext.Current.Session["RetailerCheckUpload"].ToString());
            }
        }
        public static int RetailerVisitingCardandshopImageUpload            //Pankaj Dhingra
        {
            get
            {
                return Convert.ToInt32(HttpContext.Current.Session["RetailerVisitingCardandshopImageUpload  "].ToString());
            }
        }
        
        /*#CC27 end*/

        #endregion
        public static void Errorhandling(Exception exce)
        {
            try
            {
                if (UserId != null)
                    clsHandleException.fncHandleException(exce, "User Id- '" + UserId + "'");
                else
                    clsHandleException.fncHandleException(exce, "");
            }
            catch (Exception ex)
            {
                clsHandleException.fncHandleException(exce, ex.Message);
            }




        }

        public static byte DefaultRedirectionFlag
        {
            get { return Convert.ToByte(HttpContext.Current.Session["DefaultRedirectionFlag"].ToString()); }
        }

        /* Default Redirection,1: Sales Channel Opening Default, 2: Retailer Default, 3: Retailer Opening Stock Default Redirection */


        public static void ExportToExeclUsingOPENXMLV2(DataTable Dt, string FilenameToExport)
        {

            ExcelExport objExcel = new ExcelExport();
            string filename = strRootFilePath + strGlobalDownloadExcelPathRoot;
            string strExportFileName = filename + "Excel" + importExportExcelFileName;

            objExcel.ExportDataTable(Dt, strExportFileName);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + FilenameToExport + ".xlsx");
            HttpContext.Current.Response.ContentType = "application/vnd.xlsx";
            ClearBuffer();
            HttpContext.Current.Response.WriteFile(strExportFileName);
            HttpContext.Current.Response.End();
        }

        public static void ExportToExecl(DataSet Dt, string FilenameToExport)
        {

            string[] SheetName = null;
            OpenXMLExcel xl = new OpenXMLExcel();
            string ExportFileLocation = HttpContext.Current.Server.MapPath("~") + "/Excel/Download/";
            string TempLocation = HttpContext.Current.Server.MapPath("~") + "/Excel/Templates/";
            string strExcelFileName = FilenameToExport + System.DateTime.Now.Ticks + ".xlsx";
            //DeleteFiles(ExportFileLocation);
            xl.ExportDataTableV2(Dt, ExportFileLocation, TempLocation + "BlankExportTemplate.xlsx", strExcelFileName, true, 1, SheetName);
        }

        public static void ExportToExeclEPPTemplate(DataSet Dt, string FilenameToExport, string strTemplateFilename)
        {

            OpenXMLExcel objExcel = new OpenXMLExcel();

            string sfilepath = strTemplateFilename;

            string filename = strExcelPhysicalDownloadPathSB;
            string strExportFileName = FilenameToExport + importExportExcelFileName;
            //objExcel.ExportDataTableEPPwithTemplate(Dt, filename, sfilepath, strExportFileName, true, 1);
            objExcel.ExportDataTable(Dt, filename, sfilepath, strExportFileName, true, 1);
                        
            ClearBuffer();

        }

        public static void ExportToExeclV2(DataSet Dt, string FilenameToExport)
        {
            FilenameToExport = FilenameToExport + ".xlsx";
            OpenXMLExcel objExcel = new OpenXMLExcel();
            string sfilepath = strExcelPhysicalBlankTemplatePathSB;
            string filename = strExcelPhysicalDownloadPathSB;
            //string sfilepath = strRootFilePath + strGlobalBlankExportTemplateName;
            //string filename = strRootFilePath + strGlobalDownloadExcelPathRoot;
            //            objExcel.ExportDataTable(Dt, filename, sfilepath, FilenameToExport, true, 1);//29-04-2013

            objExcel.ExportDataTable(Dt, filename, sfilepath, FilenameToExport, true, 1);
            //filename = filename + FilenameToExport;
            //HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.Charset = "";
            //HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + FilenameToExport + ".xlsx");
            //HttpContext.Current.Response.ContentType = "application/vnd.xlsx";
            //ClearBuffer();
            //HttpContext.Current.Response.WriteFile(filename);
            //HttpContext.Current.Response.End();
        }

        public static void ExportToExeclV2(DataSet Dt, string FilenameToExport, int intSheetCount)
        {
            FilenameToExport = FilenameToExport + ".xlsx";
            OpenXMLExcel objExcel = new OpenXMLExcel();
            string sfilepath = strExcelPhysicalBlankTemplatePathSB;
            string filename = strExcelPhysicalDownloadPathSB;
            objExcel.ExportDataTable(Dt, filename, sfilepath, FilenameToExport, true, intSheetCount);
        }


        public static void ExportToExeclV3(DataSet Dt, string FilenameToExport, int intSheetCount)
        {
            FilenameToExport = FilenameToExport + ".xlsx";
            OpenXMLExcel objExcel = new OpenXMLExcel();
            string sfilepath = strExcelPhysicalBlankTemplatePathSB;
            string filename = strExcelBulkUploadPSIInfoPath;
            objExcel.ExportDataTable(Dt, filename, sfilepath, FilenameToExport, true, intSheetCount);
        }

        public static void ExportToExecl(DataSet Dt, string FilenameToExport, EnumData.eTemplateCount TemplateCount)
        {

            OpenXMLExcel objExcel = new OpenXMLExcel();
            // string sfilepath = strRootFilePath + strGlobalBlankExportTemplateName;
            // string filename = strRootFilePath + strGlobalDownloadExcelPathRoot;
            string sfilepath = strExcelPhysicalBlankTemplatePathSB;
            //string filename1 = strRootFilePath + strGlobalDownloadExcelPathRoot;
            string filename = strExcelPhysicalDownloadPathSB;
            //string strExportFileName = "Excel" + importExportExcelFileName;
            string strExportFileName = FilenameToExport + importExportExcelFileName;
            //objExcel.ExportDataTable(Dt, filename, sfilepath, strExportFileName, true, Convert.ToInt16(TemplateCount));
           /*  objExcel.ExportDataTable(Dt, filename, sfilepath, strExportFileName, true, Convert.ToInt16(TemplateCount)); #CC23 Commented */
            objExcel.ExportDataTableV2(Dt, filename, sfilepath, strExportFileName, true, Convert.ToInt16(TemplateCount)); /* #CC23 Added */
            //filename = filename + strExportFileName;
            //HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.Charset = "";
            //HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + FilenameToExport + ".xlsx");
            //HttpContext.Current.Response.ContentType = "application/vnd.xlsx";
            ClearBuffer();
            //HttpContext.Current.Response.WriteFile(filename);
            //HttpContext.Current.Response.End();
            
        }


        public static void ExportToExeclEpp(DataSet Dt, string FilenameToExport, EnumData.eTemplateCount TemplateCount)
        {

            OpenXMLExcel objExcel = new OpenXMLExcel();
            // string sfilepath = strRootFilePath + strGlobalBlankExportTemplateName;
            // string filename = strRootFilePath + strGlobalDownloadExcelPathRoot;
            string sfilepath = strExcelPhysicalBlankTemplatePathSB;
            //string filename1 = strRootFilePath + strGlobalDownloadExcelPathRoot;
            string filename = strExcelPhysicalDownloadPathSB;
            //string strExportFileName = "Excel" + importExportExcelFileName;
            string strExportFileName = FilenameToExport + importExportExcelFileName;
            //objExcel.ExportDataTable(Dt, filename, sfilepath, strExportFileName, true, Convert.ToInt16(TemplateCount));
            objExcel.ExportDataTable(Dt, filename, sfilepath, strExportFileName, true, Convert.ToInt16(TemplateCount));
            //filename = filename + strExportFileName;
            //HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.Charset = "";
            //HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + FilenameToExport + ".xlsx");
            //HttpContext.Current.Response.ContentType = "application/vnd.xlsx";
            //ClearBuffer();
            //HttpContext.Current.Response.WriteFile(filename);
            //HttpContext.Current.Response.End();
        }

        //#CC34
        public static void ExportInExcel(DataSet DsExport, string strFileName)
        {
            try
            {
                if (DsExport != null && DsExport.Tables.Count > 0)
                {
                    PageBase.ExportToExeclV2(DsExport, strFileName, DsExport.Tables.Count);
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }



        public static void ConvertXMLIntoDataset(string strxml, ref DataSet ds)
        {

            StringReader myStreamReader = new StringReader(strxml);
            XmlDataDocument myXmlDataDocument = new XmlDataDocument();
            myXmlDataDocument.DataSet.ReadXml(myStreamReader);
            ds = null;
            ds = myXmlDataDocument.DataSet;
            myXmlDataDocument = null;
            myStreamReader = null;

        }

        public static string ImageChange(Int16 IsActive)
        {
            string imgUrl = siteURL + "/" + strAssets + "/CSS/images/decative.png";
            if (IsActive == 1)
            { imgUrl = siteURL + "/" + strAssets + "/CSS/images/active.png"; }
            return imgUrl;
        }
        public static string NoImageFound()
        {
            string strURL = siteURL + "/" + strAssets + "/CSS/Images/no-image.png";
            return strURL;
        }
        public static string ToolTipeChange(Int16 IsActive)
        {
            string ToolTip = "Active";
            if (IsActive == 0)
            { ToolTip = "Inactive"; }
            return ToolTip;
        }

        #region Session Check and Values
        public void GetvalidSession()
        {
            if (HttpContext.Current.Session["LoginName"] == null)
            {
                //HttpContext.Current.ApplicationInstance.Server.Transfer("~/Logout.aspx");
                Response.Redirect("~/Logout.aspx", true);

                /*
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.StatusCode = 301;
                string[] strAssets = PageBase.strAssets.Split(new char[] { '/' });
                string path = strAssets[1].ToString();
                HttpContext.Current.Response.AppendHeader("location", siteURL + "Login/" + path + "/Login.aspx");
                HttpContext.Current.Response.End();
                */
            }
        }
        

        /*#CC01 COMMENTED public static void UsertrackingAndRequestValidate()*/
        public void UsertrackingAndRequestValidate()/*#CC01 ADDED*/
        {

            string RedirectURL = PageBase.DefaultRedirection(PageBase.DefaultRedirectionFlag);
            if (PageBase.DefaultRedirectionFlag == 1 | PageBase.DefaultRedirectionFlag == 3)
                HttpContext.Current.Response.Redirect(RedirectURL, false);

            string[] strURL = HttpContext.Current.Request.Params["URL"].Split(new char[] { '/' });
            int count = strURL.Length;
            string serverIP = string.Empty;
            if (HttpContext.Current.Request.ServerVariables.Get("HTTP_X_FORWARDED_FOR") != null)
            {
                serverIP = HttpContext.Current.Request.ServerVariables.Get("HTTP_X_FORWARDED_FOR").ToString();
            }
            using (UserData objCommon = new UserData())
            {
                objCommon.UserID = UserId;
                objCommon.UserIP = serverIP;
                objCommon.MenuDescription = strURL.GetValue(count - 1).ToString();


                if (objCommon.MenuDescription.ToLower().Contains("default") == true)
                {
                    PageHeading = string.Empty;
                    return;
                }
                objCommon.UserServerIP = HttpContext.Current.Request.Params["REMOTE_ADDR"].ToString();
                objCommon.InsertUpdateUserTracking();

                MenuID = objCommon.ReturnMenuID;/*#CC01 ADDED*/

                //if (objCommon.isMenuRequestValid == false)  //#CC03 commented
                if (objCommon.isMenuRequestValid == 0)  //#CC03 added
                {
                    //HttpContext.Current.Response.Redirect(siteURL + "Default.aspx?err=1", false);   //#CC03 added err in querystring #CC38-comented
                    HttpContext.Current.Response.Redirect(siteURL + "Default.aspx?err=1", false);//#CC38 added
                    return;
                }
                //#CC03 add start
                if (objCommon.isMenuRequestValid == 2)  //#CC03 added
                {
                    HttpContext.Current.Response.Redirect(siteURL + "Default.aspx?err=2", false);
                    return;
                }

                //#CC03 add end
                if (objCommon.MenuDescription != "" || objCommon.MenuDescription != string.Empty)
                {
                    PageHeading = objCommon.MenuDescription;
                }
                if (objCommon.ErrorMessage != null && objCommon.ErrorMessage != string.Empty)
                {
                    throw new ArgumentException(objCommon.ErrorMessage);
                    return;
                }

            }
        }

        #endregion

        public static string importExportExcelFileName
        {
            //get { return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".xlsx"; }  //#CC02 commented
            get { return Guid.NewGuid().ToString("N") + ".xlsx"; }  //#CC02 added
        }
        public static string importExportCSVFileName
        {
            //get { return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".csv"; }  //#CC02 commented
            get { return Guid.NewGuid().ToString("N") + ".csv"; }  //#CC02 added

        }
        public static void ClearBuffer()
        {
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
            HttpContext.Current.Response.Cache.SetExpires(System.DateTime.Now.AddSeconds(-1));
            HttpContext.Current.Response.Cache.SetNoStore();
            HttpContext.Current.Response.AppendHeader("Pragma", "no-cache");

        }



        public string GetMonthName(short MonthNo)
        {
            string sMonthName = string.Empty;
            switch (MonthNo)
            {
                case 1: sMonthName = "Jan"; break;
                case 2: sMonthName = "Feb"; break;
                case 3: sMonthName = "Mar"; break;
                case 4: sMonthName = "Apr"; break;
                case 5: sMonthName = "May"; break;
                case 6: sMonthName = "Jun"; break;
                case 7: sMonthName = "Jul"; break;
                case 8: sMonthName = "Aug"; break;
                case 9: sMonthName = "Sep"; break;
                case 10: sMonthName = "Oct"; break;
                case 11: sMonthName = "Nov"; break;
                case 12: sMonthName = "Dec"; break;
            }
            return sMonthName;

        }

        #region Return Different type of data after filtering
        /// <summary>
        /// Return datatable after filter
        /// </summary>
        /// <param name="dtfilter"></param>
        /// <param name="strSearchCondition"></param>
        /// <returns></returns>
        //public static DataTable FilterTable(DataTable dtfilter, string strSearchCondition)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        dtfilter.Select(strSearchCondition).CopyToDataTable(dt, System.Data.LoadOption.OverwriteChanges);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (dt != null)
        //        {
        //            dt.Dispose();
        //        }
        //    }

        //}

        //public static DataTable FilterTable(DataTable dtfilter, string strSearchCondition, string strSoting)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        dtfilter.Select(strSearchCondition, strSoting).CopyToDataTable(dt, System.Data.LoadOption.OverwriteChanges);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (dt != null)
        //        {
        //            dt.Dispose();
        //        }
        //    }

        //}



        #endregion
        #region DropDown Binding using Different Collection
        public static void DropdownBinding(ref DropDownList drpRef, DataTable dtSource, params string[] strDisplayColumn)
        {
            try
            {
                if (drpRef.Items.Count > 0) drpRef.Items.Clear();
                if (dtSource != null && dtSource.Rows.Count > 0)
                {
                    drpRef.DataSource = dtSource;
                    drpRef.DataValueField = strDisplayColumn[0].ToString();
                    drpRef.DataTextField = strDisplayColumn[1].ToString();
                    drpRef.DataBind();
                    drpRef.Items.Insert(0, new ListItem("Select", "0"));
                }
                else
                {
                    drpRef.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void DropdownBinding(ref ComboBox drpRef, DataTable dtSource, params string[] strDisplayColumn)
        {
            try
            {
                if (drpRef.Items.Count > 0) drpRef.Items.Clear();
                if (dtSource != null && dtSource.Rows.Count > 0)
                {
                    drpRef.DataSource = dtSource;
                    drpRef.DataValueField = strDisplayColumn[0].ToString();
                    drpRef.DataTextField = strDisplayColumn[1].ToString();
                    drpRef.DataBind();
                    drpRef.Items.Insert(0, new ListItem("Select", "0"));
                }

                else
                {
                    drpRef.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static void DropdownBinding(ref ComboBox drpRef, DataTable dtSource, bool IsComboAll, params string[] strDisplayColumn)
        {
            try
            {
                if (drpRef.Items.Count > 0) drpRef.Items.Clear();
                if (dtSource != null && dtSource.Rows.Count > 0)
                {
                    drpRef.DataSource = dtSource;
                    drpRef.DataValueField = strDisplayColumn[0].ToString();
                    drpRef.DataTextField = strDisplayColumn[1].ToString();
                    drpRef.DataBind();
                    drpRef.Items.Insert(0, new ListItem("All", "0"));
                }

                else
                {
                    drpRef.Items.Insert(0, new ListItem("All", "0"));
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void DropdownBinding(ref DropDownList drpRef, Hashtable dtSource)
        {
            try
            {
                if (drpRef.Items.Count > 0) drpRef.Items.Clear();
                drpRef.DataSource = dtSource;
                drpRef.DataValueField = "Key";
                drpRef.DataTextField = "Value";
                drpRef.DataBind();
                drpRef.Items.Insert(0, new ListItem("Select", "0"));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        #endregion
        #region Reset method for Page

        public static void ResetPageControl(ControlCollection Contrls)
        {
            try
            {
                foreach (Control ctrlparent in Contrls)
                {
                    if (ctrlparent.HasControls())
                    {
                        ResetPageControl(ctrlparent.Controls);
                    }
                    else
                    {
                        if (ctrlparent.GetType() == typeof(TextBox))
                        {
                            ((TextBox)ctrlparent).Text = "";
                        }
                        else if (ctrlparent.GetType() == typeof(DropDownList))
                        {
                            if (((DropDownList)ctrlparent).Items.Count > 0) ((DropDownList)ctrlparent).ClearSelection();
                        }
                        else if (ctrlparent.GetType() == typeof(RadioButton))
                        {
                            ((RadioButton)ctrlparent).Checked = false;
                        }
                        else if (ctrlparent.GetType() == typeof(RadioButtonList))
                        {
                            if (((RadioButtonList)ctrlparent).Items.Count > 0) ((RadioButtonList)ctrlparent).ClearSelection();
                        }
                        else if (ctrlparent.GetType() == typeof(CheckBox))
                        {
                            ((CheckBox)ctrlparent).Checked = false;
                        }
                        else if (ctrlparent.GetType() == typeof(CheckBoxList))
                        {
                            if (((CheckBoxList)ctrlparent).Items.Count > 0) ((CheckBoxList)ctrlparent).ClearSelection();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region "Code for Grid with no row"

        public static void EmptyGridFix(GridView grdView)
        {
            try
            {
                // normally executes after a grid load method
                if (grdView.Rows.Count == 0 && grdView.DataSource != null)
                {
                    DataTable dt = null;

                    // need to clone sources otherwise it will be indirectly adding to 
                    // the original source

                    if (grdView.DataSource is DataSet)
                    {
                        dt = ((DataSet)grdView.DataSource).Tables[0].Clone();
                    }
                    else if (grdView.DataSource is DataTable)
                    {
                        dt = ((DataTable)grdView.DataSource).Clone();
                    }

                    if (dt == null)
                    {
                        return;
                    }

                    dt.Rows.Add(dt.NewRow()); // add empty row
                    grdView.DataSource = dt;
                    grdView.DataBind();

                    // hide row
                    grdView.Rows[0].Visible = false;
                    grdView.Rows[0].Controls.Clear();
                }

                // normally executes at all postbacks
                if (grdView.Rows.Count == 1 && grdView.DataSource == null)
                {
                    bool bIsGridEmpty = true;

                    // check first row that all cells empty
                    for (int i = 0; i < grdView.Rows[0].Cells.Count; i++)
                    {
                        if (grdView.Rows[0].Cells[i].Text != string.Empty)
                        {
                            bIsGridEmpty = false;
                        }
                    }
                    // hide row
                    if (bIsGridEmpty)
                    {
                        grdView.Rows[0].Visible = false;
                        grdView.Rows[0].Controls.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                Errorhandling(ex);
                ////clsException.clsHandleException.fncHandleException(ex, "User Id- '" + Session["webuserid"] + "' User - '" + Session["person_name"] + "'");
            }
        }

        #endregion

        public static void MergeXmlInDataTable(ref DataRow drSourceRow, string xmlString)
        {
            DataSet dtXml = new DataSet();
            StringReader myStreamReader = new StringReader(xmlString);
            XmlDataDocument myXmlDataDocument = new XmlDataDocument();
            string strMerageRow = string.Empty;
            myXmlDataDocument.DataSet.ReadXml(myStreamReader);
            dtXml = myXmlDataDocument.DataSet.Copy();
            myXmlDataDocument.DataSet.Dispose();
            myStreamReader.Dispose();

            if (dtXml.Tables.Count > 0 && dtXml.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drow in dtXml.Tables[0].Rows)
                {
                    strMerageRow = (strMerageRow == "" ? drow[1].ToString() : "" + Environment.NewLine + drow[1].ToString());
                }
                drSourceRow[1] = strMerageRow;
            }
        }

        public static void ConvertColumnsIntoRows(ref DataTable dtSource, byte ColumnStartingNumber)
        {
            DataTable DtNew = new DataTable();
            DataRow dNewRow;
            try
            {
                DtNew.Columns.Add("RetailerCode", System.Type.GetType("System.String"));
                DtNew.Columns.Add("BaseID", System.Type.GetType("System.String"));
                DtNew.Columns.Add("Quantity", System.Type.GetType("System.Int32"));

                foreach (DataRow drow in dtSource.Rows)
                {
                    for (int iCol = ColumnStartingNumber; iCol < dtSource.Columns.Count; iCol++)
                    {
                        dNewRow = DtNew.NewRow();
                        dNewRow["RetailerCode"] = drow[0];
                        dNewRow["BaseID"] = dtSource.Columns[iCol].ColumnName;
                        dNewRow["Quantity"] = drow[dtSource.Columns[iCol].ColumnName];

                        DtNew.Rows.Add(dNewRow);
                    }
                }
                DtNew.AcceptChanges();
                dtSource = DtNew.Copy();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (DtNew != null)
                {
                    DtNew.Dispose();
                }

            }

        }

        public static void createHTMLTable(DataTable dtSource, Int16 intStartingDigit, ref Table HtmlTable)
        {
            TableRow htmlRow;
            try
            {
                htmlRow = new TableRow();
                for (int iCol = 0; iCol < dtSource.Columns.Count; iCol++)
                {
                    TableCell htmlcell = new TableCell();
                    htmlcell.Text = dtSource.Columns[iCol].ColumnName;
                    htmlRow.Cells.Add(htmlcell);
                    htmlcell.Dispose();
                }

                HtmlTable.Rows.Add(htmlRow);
                for (int iRow = 0; iRow < dtSource.Rows.Count; iRow++)
                {
                    htmlRow = new TableRow();
                    addTextBox(ref htmlRow, dtSource, intStartingDigit, dtSource.Rows[iRow][0].ToString());
                    HtmlTable.Rows.Add(htmlRow);


                }

                HtmlTable.CssClass = "gridrow";


            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        private static void addTextBox(ref TableRow htmlrow, DataTable dtColTable, Int16 intstartCol, string firstCol)
        {
            TableCell htmlcell;
            htmlcell = new TableCell();
            Label lblretailer = new Label();
            lblretailer.ID = "lbl" + firstCol;
            lblretailer.Text = firstCol;
            htmlcell.Controls.Add(lblretailer);
            htmlrow.Cells.Add(htmlcell);
            lblretailer.Dispose();
            htmlcell = null;


            for (int iCol = intstartCol; iCol < dtColTable.Columns.Count; iCol++)
            {
                TextBox txt = new TextBox();
                htmlcell = new TableCell();
                txt.ID = "txt" + dtColTable.Columns[iCol].ColumnName + firstCol;
                txt.Text = "0";
                htmlcell.Controls.Add(txt);
                htmlrow.Cells.Add(htmlcell);
                txt.CssClass = "form_input5";

            }


        }
        public static void FillReportDate(ref string Datefrom, ref string DateTo)
        {
            Datefrom = DateTime.Now.Month.ToString() + "/01/" + DateTime.Now.Year.ToString();
            DateTo = DateTime.Now.ToShortDateString();


        }
        public static bool GlobalErrorDisplay()
        {
            if (GlobalErrorMsg == "1")
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public struct SshConnectionInfo
        {
            public string Host;
            public string User;
            public string Pass;
            public string IdentityFile;
            public string proto;

        }
        public static Boolean GetConnectWithFTP(string strHost, string strUser, string strpassword)
        {
            ////SshTransferProtocolBase sshCp;
            ////sshCp = new Scp(strHost, strUser, strpassword);
            ////sshCp.Connect();
            //while (true)
            //{
            //    string direction = "from";
            //    string rfile = GetArg("Enter remote file ['Enter to cancel']");
            //    if (rfile == "") break;
            //    string lpath = GetArg("Enter local path ['Enter to cancel']");
            //    if (lpath == "") break;
            //    sshCp.Get(rfile, lpath);
            //}
            return true;
        }

        /// <summary>
        /// Convert 2003 version file into 2007 version
        /// </summary>
        /// <param name="strRootPath"></param>
        /// <param name="strUploadedFileName"></param>
        public static void ConvertXLStoXLSX(string strRootPath, ref string strUploadedFileName)
        {
            try
            {
                //if (System.IO.Path.GetExtension(strUploadedFileName).ToLower() == ".xls")
                //{
                //    Microsoft.Office.Interop.Excel.ApplicationClass app = new Microsoft.Office.Interop.Excel.ApplicationClass();
                //    Microsoft.Office.Interop.Excel.Workbook workBook = app.Workbooks.Open((strRootPath + strUploadedFileName), 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false,
                //    false, 0, true, 1, 0);
                //    string retval = strRootPath + strUploadedFileName.Replace(".xls", ".xlsx"); //System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + "_tmp_" + ".xlsx";
                //    strUploadedFileName = retval;
                //    workBook.SaveAs(retval, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook, null, null, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, false, false, null,
                //    null, null);

                //    workBook.Close(null, null, null);
                //    workBook = null;
                //    app.Quit();         //Pankaj Dhingra
                //}
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public static DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();
            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;
            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow 
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }
                DataRow dr = dtReturn.NewRow();
                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        public static void SetSessionForLoginUser(DataSet dtUser)
        {
            /* #CC18 Add Start */
            if (dtUser.Tables[0] != null && dtUser.Tables[0].Rows.Count > 0)
            {
                if (dtUser.Tables[0].Rows[0]["RetailerExcelUpload"] != null)
                {
                    HttpContext.Current.Session["RetailerExcelUpload"] = Convert.ToInt32(dtUser.Tables[0].Rows[0]["RetailerExcelUpload"].ToString());
                }
            }

            /* #CC18 Add End */

            /* #CC30 Add Start */
            if (dtUser!=null && dtUser.Tables.Count>3 &&  dtUser.Tables[3] != null && dtUser.Tables[3].Rows.Count > 0)
            {
                if (dtUser.Tables[3].Rows[0]["RSMLevelName"] != null)
                {
                    HttpContext.Current.Session["RSMLevelName"] = dtUser.Tables[3].Rows[0]["RSMLevelName"].ToString();
                }
            }/* #CC30 Add end */

            if (dtUser.Tables[1] != null && dtUser.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow Dr in dtUser.Tables[1].Rows)
                {

                    if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "OPDATE")
                    {
                        if (dtUser.Tables[0].Rows[0]["SalesChannelTypeID"].ToString() != "")
                        {
                            // line: added by amit agarwal
                            HttpContext.Current.Session["IsOpeningStockEntered"] = Convert.ToBoolean(dtUser.Tables[0].Rows[0]["IsOpeningStockEntered"].ToString());



                            if (Convert.ToBoolean(dtUser.Tables[0].Rows[0]["IsOpeningStockEntered"]) == false)
                            {
                                HttpContext.Current.Session["BackDaysAllowedOpeningStock"] = Dr["ConfigValue"].ToString();
                            }
                            else
                            {
                                //HttpContext.Current.Session["BackDaysAllowedOpeningStock"] = "43";
                                HttpContext.Current.Session["BackDaysAllowedOpeningStock"] = Dr["ConfigValue"].ToString();
                            }
                        }
                        else
                            HttpContext.Current.Session["BackDaysAllowedOpeningStock"] = Dr["ConfigValue"].ToString();//Now if any user whether it is saleschannel user or not
                        //before this Ho was not able to insert the Opening Stock info while making the saleschannel

                    }
                    
                    else if
                    (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "PRETRN")
                    {
                        HttpContext.Current.Session["BackDaysAllowedBeforeOpening"] = Dr["ConfigValue"].ToString();
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "ORDATE")
                    {
                        HttpContext.Current.Session["BackDaysAllowedOrder"] = Dr["ConfigValue"].ToString();
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "PONOAUTO")
                    {
                        if (dtUser.Tables[0].Rows[0]["SalesChannelLevel"].ToString() != null && dtUser.Tables[0].Rows[0]["SalesChannelLevel"].ToString() == "2")
                        {
                            HttpContext.Current.Session["IsPrimaryOrderNoAutogenerate"] = Dr["ConfigValue"].ToString();
                        }
                        else
                        {
                            HttpContext.Current.Session["IsPrimaryOrderNoAutogenerate"] = null;
                        }


                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "IONOAUTO")
                    {
                        if (dtUser.Tables[0].Rows[0]["SalesChannelLevel"].ToString() != null && dtUser.Tables[0].Rows[0]["SalesChannelLevel"].ToString() == "3")
                        {
                            HttpContext.Current.Session["IsIntermediaryOrderNoAutogenerate"] = Dr["ConfigValue"].ToString();
                        }
                        else
                        {
                            HttpContext.Current.Session["IsIntermediaryOrderNoAutogenerate"] = null;
                        }
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "OPVALIDATE")
                    {
                        HttpContext.Current.Session["ValidateSerialNo"] = Dr["ConfigValue"].ToString();
                        //Pankaj Dhingra 04052012
                    }

                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "RETAILERLOGIN")
                    {
                        HttpContext.Current.Session["RetailerLogin"] = Dr["ConfigValue"].ToString();
                        //Pankaj Dhingra 25052012
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "ISDLOGIN")
                    {
                        HttpContext.Current.Session["ISDLogin"] = Dr["ConfigValue"].ToString();
                        //Amit Agarwal
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "RETAILERBACKDATESALE")
                    {
                        HttpContext.Current.Session["RETAILERBACKDATESALE"] = Dr["ConfigValue"].ToString();
                        //Amit Agarwal
                    }

                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "RETAILERHIERLVLID")
                    {
                        HttpContext.Current.Session["RETAILERHIERLVLID"] = Dr["ConfigValue"].ToString();
                        //Pankaj Dhingra 25052012
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "PRIMARYINGRNMODE")
                    {
                        HttpContext.Current.Session["PRIMARYINGRNMODE"] = Dr["ConfigValue"].ToString();
                        //Pankaj Dhingra 13062012
                    }

                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "SERIALLENGTHMIN")
                    {
                        HttpContext.Current.Session["SERIALLENGTHMIN"] = Dr["ConfigValue"].ToString();
                        //Amit agarwal
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "SERIALLENGTHMAX")
                    {
                        HttpContext.Current.Session["SERIALLENGTHMAX"] = Dr["ConfigValue"].ToString();
                        //Amit agarwal
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "BATCHLENGTHMIN")
                    {
                        HttpContext.Current.Session["BATCHLENGTHMIN"] = Dr["ConfigValue"].ToString();
                        //Amit agarwal
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "BATCHLENGTHMAX")
                    {
                        HttpContext.Current.Session["BATCHLENGTHMAX"] = Dr["ConfigValue"].ToString();
                        //Amit agarwal
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "SKUCOLORREQD")
                    {
                        HttpContext.Current.Session["SKUCOLORREQD"] = Dr["ConfigValue"].ToString();
                        //Amit agarwal
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "RETAILERCODEAUTO")
                    {
                        HttpContext.Current.Session["RETAILERCODEAUTO"] = Dr["ConfigValue"].ToString();
                        //Pankaj Dhingra
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "HIERARCHYADMIN")
                    {
                        HttpContext.Current.Session["HIERARCHYADMIN"] = Dr["ConfigValue"].ToString();
                        //Pankaj Dhingra
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "DefaultOpStockDate")
                    {
                        HttpContext.Current.Session["DefaultOpStockDate"] = Dr["ConfigValue"].ToString();
                        //Pankaj Dhingra
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "SalesReturnBinAsking")
                    {
                        HttpContext.Current.Session["SalesReturnBinAsking"] = Dr["ConfigValue"].ToString();
                        //Pankaj Dhingra
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "RETAILERSTOCKTRACK")
                    {
                        HttpContext.Current.Session["RETAILERSTOCKTRACK"] = Dr["ConfigValue"].ToString();
                        //Rakesh Goel 26-Mar-14
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "DirctSralSaleIntefce")
                    {
                        HttpContext.Current.Session["DirctSralSaleIntefce"] = Dr["ConfigValue"].ToString();
                        //Pankaj Dhingra 11-Oct-14
                    }

                        //#CC05 Added Start
                    /*#CC06 START ADDED*/
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "DirectSaleReturn")
                    {
                        HttpContext.Current.Session["DirectSaleReturn"] = Dr["ConfigValue"].ToString();
                    }
                    /*#CC06 START END*/
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "PRIMARYSALESBCP")
                    {
                        HttpContext.Current.Session["PRIMARYSALESBCP"] = Dr["ConfigValue"].ToString();
                        //Sumit Kumar 05-Jan-14
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "SALESMANOPTIONAL")
                    {
                        HttpContext.Current.Session["SALESMANOPTIONAL"] = Dr["ConfigValue"].ToString();
                        //Sumit Kumar 08-Jan-14
                    }
                    //#CC05 Added End
                    /*#CC07 START ADDED*/
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "AREAOPTIONAL")
                    {
                        HttpContext.Current.Session["AREAOPTIONAL"] = Dr["ConfigValue"].ToString();
                    }
                    /*#CC07 START END*/
                    /*#CC09 START ADDED*/
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "RetailerOpStockDate")
                    {
                        HttpContext.Current.Session["RetailerOpStockDate"] = Dr["ConfigValue"].ToString();
                    }
                    /*#CC09 START END*/
                    /*#CC09 START ADDED*/
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "RetailerApproval")
                    {
                        HttpContext.Current.Session["RetailerApproval"] = Dr["ConfigValue"].ToString();
                    }
                    /*#CC09 START END*/
                    /*#CC10 ADDED START*/
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "RetailerBankDetail")
                    {
                        HttpContext.Current.Session["RetailerBankDetail"] = Dr["ConfigValue"].ToString();
                    }
                    /*#CC10 ADDED END*/

                     /*#CC11 ADDED START*/
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "PotentialVolDisplay")
                    {
                        HttpContext.Current.Session["PotentialVolDisplay"] = Dr["ConfigValue"].ToString();
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "PotentialVolMandatry")
                    {
                        HttpContext.Current.Session["PotentialVolMandatry"] = Dr["ConfigValue"].ToString();
                    }
                    /*#CC11 ADDED END*/
                    /*#CC12 START ADDED*/
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "TehsillDisplayMode")
                    {
                        HttpContext.Current.Session["TehsillDisplayMode"] = Dr["ConfigValue"].ToString();
                    }
                    /*#CC12 START END*/

                    /*#CC13 Add Start*/
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString().ToLower() == "multilogin")
                    {
                        HttpContext.Current.Session["Multilogin"] = Dr["ConfigValue"].ToString();
                    }
                    /*#CC13 Add End*/

                    /*#CC14 Add Start*/
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString().ToLower() == "alloweditretailer")
                    {
                        HttpContext.Current.Session["ALLOWEDITRETAILER"] = Dr["ConfigValue"].ToString();
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString().ToLower() == "stparentcheck")
                    {
                        HttpContext.Current.Session["STPARENTCHECK"] = Dr["ConfigValue"].ToString();
                    }
                    /*#CC14 Add End*/
                    /*#CC15 Add Start*/
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString().ToLower() == "showcartonsize")
                    {
                        HttpContext.Current.Session["SHOWCARTONSIZE"] = Dr["ConfigValue"].ToString();
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString().ToLower() == "reqcartonsize")
                    {
                        HttpContext.Current.Session["REQCARTONSIZE"] = Dr["ConfigValue"].ToString();
                    }
                    /*#CC15 Add End*/
                    /*#CC16 Add Start*/
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString().ToLower() == "changetinlabel")
                    {
                        HttpContext.Current.Session["ChangeTinLabel"] = Dr["ConfigValue"].ToString();
                    }

                    /*#CC16 Add End*/
                    /*#CC17 Add Start*/
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString().ToLower() == "locationmapping")
                    {
                        HttpContext.Current.Session["LOCATIONMAPPING"] = Dr["ConfigValue"].ToString();
                    }

                    /*#CC17 Add End*/
                    /*#CC19 Add Start*/
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString().ToLower() == "brandwisebulletin")
                    {
                        HttpContext.Current.Session["BRANDWISEBULLETIN"] = Dr["ConfigValue"].ToString();
                    }
                    /*#CC19 Add End*/
                    /*#CC20 Add Start*/
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "NDHIERARCHYMAPPING")
                    {
                        HttpContext.Current.Session["NDHIERARCHYMAPPING"] = Dr["ConfigValue"].ToString();
                    }
                    /*#CC20 Add END*/
                    /*#CC21 START ADDED*/
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "ISLOGINEDITABLE")
                    {
                        HttpContext.Current.Session["ISLOGINEDITABLE"] = Dr["ConfigValue"].ToString();
                    }
                    /*#CC21 START END*/
                    /*#CC24 START ADDED*/
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "RETAILERCREATIONAPPROVALMAIL")
                    {
                        HttpContext.Current.Session["RETAILERCREATIONAPPROVALMAIL"] = Dr["ConfigValue"].ToString();
                    }
                    /*#CC24 START END*/
                    
                    /* #CC25 Add Start  */
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "RETHERARCHYPARENTMANDATORY")
                    {
                        HttpContext.Current.Session["RETHERARCHYPARENTMANDATORY"] = Dr["ConfigValue"].ToString();
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "PWDEXPY")
                    {
                        HttpContext.Current.Session["PWDEXPY"] = Dr["ConfigValue"].ToString();
                    }
                    /* #CC25 Add End  */
                    /* #CC26 Add Start  */
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "SALESCHANNELINACTIVE")
                    {
                        HttpContext.Current.Session["SALESCHANNELINACTIVE"] = Dr["ConfigValue"].ToString();
                    }
                    /* #CC26 Add End  */
                    /*#CC27 Start*/ else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "WhatsAppMobileNumber")
                    {
                        HttpContext.Current.Session["WhatsAppMobileNumber"] = Dr["ConfigValue"].ToString();
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "RetailerCheckUpload")
                    {
                        HttpContext.Current.Session["RetailerCheckUpload"] = Dr["ConfigValue"].ToString();
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "RetailerVisitingCardandshopImageUpload")
                    {
                        HttpContext.Current.Session["RetailerVisitingCardandshopImageUpload"] = Dr["ConfigValue"].ToString();
                    } /*#CC27 end*/
                    else if/*#CC28 Added*/ (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "UserLogingUsingOTP")
                    {
                        HttpContext.Current.Session["UserLogingUsingOTP"] = Dr["ConfigValue"].ToString();
                    }/*#CC28 Added End*/
                    else if/*#CC29 Added Started*/ (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "SecondarySalesReturnApproval")
                    {
                        HttpContext.Current.Session["SecondarySalesReturnApproval"] = Dr["ConfigValue"].ToString();
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "IntermediarySalesReturnApproval")
                    {
                        HttpContext.Current.Session["IntermediarySalesReturnApproval"] = Dr["ConfigValue"].ToString();
                    }/*#CC29 Added End*/
                  
                    ///*#CC32 Add Started*/
                    //else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "ISPANNOMANDATORY")
                    //{
                    //    HttpContext.Current.Session["ISPANNOMANDATORY"] = Dr["ConfigValue"].ToString();
                    //}
                     /*#CC32 Add End*/
                    /*#CC33 Added Started*/
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "RetailerMultipleMapping")
                    {
                        HttpContext.Current.Session["RetailerMultipleMapping"] = Dr["ConfigValue"].ToString();
                    }
                    /*#CC33 Added End*/

                    /* #CC36 Add Start */
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString().ToLower() == "ispuniquemobile")
                    {
                        HttpContext.Current.Session["ISPUNIQUEMOBILE"] = Dr["ConfigValue"].ToString();
                    }
                    /* #CC36 Add End */

                     /* #CC37 Add Start */
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString().ToLower() == "editretailername")
                    {
                        HttpContext.Current.Session["EDITRETAILERNAME"] = Dr["ConfigValue"].ToString();
                    }
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "SalesManLOGIN")
                    {
                        HttpContext.Current.Session["SalesManLOGIN"] = Dr["ConfigValue"].ToString();
                    }
                    /* #CC37 Add End */
                    /*#CC40 Added Started*/
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "SKUExpiryDate")
                    {
                        HttpContext.Current.Session["SKUExpiryDate"] = Dr["ConfigValue"].ToString();
                    }
                    /*#CC40 Added End*/

                     /*#CC42 Added Started*/
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "UserOtherDetail")
                    {
                        HttpContext.Current.Session["UserOtherDetail"] = Dr["ConfigValue"].ToString();
                    }
                    /*#CC42 Added End*/
                    else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString().ToLower() == "retailerentitytypeid")//#CC43 Added
                    {
                        HttpContext.Current.Session["RetailerEntityTypeID"] = Dr["ConfigValue"].ToString();
                    }
                    
                }

            }
            HttpContext.Current.Session["PasswordExpired"] = dtUser.Tables[0].Rows[0]["PasswordExpiredOn"].ToString();
            HttpContext.Current.Session["UserID"] = dtUser.Tables[0].Rows[0]["UserID"].ToString();
            HttpContext.Current.Session["RoleID"] = dtUser.Tables[0].Rows[0]["RoleID"].ToString();
            HttpContext.Current.Session["HierarchyLevelID"] = dtUser.Tables[0].Rows[0]["HierarchyLevelID"].ToString();
            HttpContext.Current.Session["SalesChanelTypeID"] = dtUser.Tables[0].Rows[0]["SalesChannelTypeID"].ToString();
            HttpContext.Current.Session["ParentHierarchyLevelID"] = dtUser.Tables[0].Rows[0]["ParentHierarchyLevelID"].ToString();
            HttpContext.Current.Session["LoginName"] = dtUser.Tables[0].Rows[0]["LoginName"].ToString();
            HttpContext.Current.Session["SalesChannelID"] = dtUser.Tables[0].Rows[0]["SalesChannelID"].ToString();
            HttpContext.Current.Session["DisplayName"] = System.Web.HttpContext.Current.Server.HtmlEncode(dtUser.Tables[0].Rows[0]["DisplayName"].ToString().Length > 0 ? dtUser.Tables[0].Rows[0]["DisplayName"].ToString() : dtUser.Tables[0].Rows[0]["Name"].ToString());
            HttpContext.Current.Session["RoleName"] = dtUser.Tables[0].Rows[0]["RoleName"].ToString();
            HttpContext.Current.Session["SalesChannelLevel"] = dtUser.Tables[0].Rows[0]["SalesChannelLevel"].ToString();
            HttpContext.Current.Session["SalesChannelCode"] = dtUser.Tables[0].Rows[0]["SalesChannelCode"].ToString();
            HttpContext.Current.Session["NumberofBackDaysAllowed"] = dtUser.Tables[0].Rows[0]["NumberOfBackDays"].ToString();
            HttpContext.Current.Session["BackDaysAllowedForSaleReturn"] = dtUser.Tables[0].Rows[0]["BackDaysAllowedForSaleReturn"].ToString();/*#CC35 Added*/
            HttpContext.Current.Session["IsSuperAdmin"] = dtUser.Tables[0].Rows[0]["IsSuperAdmin"].ToString();
            HttpContext.Current.Session["AllowAllHierarchy"] = dtUser.Tables[0].Rows[0]["AllowAllHierarchy"].ToString();
            HttpContext.Current.Session["OpeningStockdate"] = dtUser.Tables[0].Rows[0]["OpeningStockdate"].ToString();
            HttpContext.Current.Session["Brand"] = 0;               //Default Value it will be initialized from default page  //Pankaj Dhingra
            HttpContext.Current.Session["MultipleBrandName"] = ""; //Default Value it will be initialized from default page  //Pankaj Dhingra

            //HttpContext.Current.Session["OtherEntityType"] = dtUser.Tables[0].Rows[0]["otherEntityType"].ToString();
            HttpContext.Current.Session["BaseEntityTypeID"] = dtUser.Tables[0].Rows[0]["BaseEntityTypeID"].ToString();
            HttpContext.Current.Session["EntityTypeID"] = dtUser.Tables[0].Rows[0]["EntityTypeID"].ToString();

            HttpContext.Current.Session["DefaultRedirectionFlag"] = dtUser.Tables[0].Rows[0]["DefaultRedirectionFlag"].ToString();


            HttpContext.Current.Session["ISP_RetailerName"] = dtUser.Tables[0].Rows[0]["ISP_RetailerName"].ToString();
            //HttpContext.Current.Session["ApprovalLevel2"] = dtUser.Tables[0].Rows[0]["ApprovalLevel2"].ToString();
            HttpContext.Current.Session["CompanyId"] = dtUser.Tables[0].Rows[0]["CompanyId"].ToString(); /*#CC43 Added*/
            

            /*#CC01 START ADDED*/
            Hashtable haspage = new Hashtable();
            DataTable dtAccessTable = dtUser.Tables[2];
            for (int i = 0; i < dtAccessTable.Rows.Count; i++)
            {
                string key = Convert.ToString(dtAccessTable.Rows[i]["MenuID"]).Trim();
                string value = Convert.ToString(dtAccessTable.Rows[i]["TagValue"]).Trim();
                haspage.Add(key, value);
            }
            HttpContext.Current.Session["PageAccess"] = haspage;
            /*#CC01 START END*/
        }


        /*#CC37 Add Start */

        public static string EDITRETAILERNAME
        {
            get
            {
                if (HttpContext.Current.Session["EDITRETAILERNAME"] != null)
                    return Convert.ToString(HttpContext.Current.Session["EDITRETAILERNAME"]);
                else
                    return "";
            }
        }

        /* #CC37 Add End */

        public static string ISP_RetailerName
        {
            get {
                if (HttpContext.Current.Session["ISP_RetailerName"] != null)
                    return Convert.ToString(HttpContext.Current.Session["ISP_RetailerName"]);
                else
                    return "";
            }

        }

        public static int SerialNoLength_Min
        {
            get
            {
                if (HttpContext.Current.Session["SERIALLENGTHMIN"] != null)
                    return Convert.ToInt32(HttpContext.Current.Session["SERIALLENGTHMIN"].ToString());
                else
                    return 0;

            }
        }
        public static int SerialNoLength_Max
        {
            get
            {
                if (HttpContext.Current.Session["SERIALLENGTHMAX"] != null)
                    return Convert.ToInt32(HttpContext.Current.Session["SERIALLENGTHMAX"].ToString());
                else
                    return 0;

            }
        }
        public static int BatchNoLength_Min
        {
            get
            {
                if (HttpContext.Current.Session["BATCHLENGTHMIN"] != null)
                    return Convert.ToInt32(HttpContext.Current.Session["BATCHLENGTHMIN"].ToString());
                else
                    return 0;

            }
        }
        public static int BatchNoLength_Max
        {
            get
            {
                if (HttpContext.Current.Session["BATCHLENGTHMAX"] != null)
                    return Convert.ToInt32(HttpContext.Current.Session["BATCHLENGTHMAX"].ToString());
                else
                    return 0;

            }
        }
        //#CC05 Added Start
        public static string PRIMARYSALESBCP
        {
            get
            {
                if (HttpContext.Current.Session["PRIMARYSALESBCP"] != null)
                    return Convert.ToString(HttpContext.Current.Session["PRIMARYSALESBCP"]);
                else
                    return "";
            }
        }

        public static string SALESMANOPTIONAL
        {
            get
            {
                if (HttpContext.Current.Session["SALESMANOPTIONAL"] != null)
                    return Convert.ToString(HttpContext.Current.Session["SALESMANOPTIONAL"]);
                else
                    return "";
            }
        }
        //#CC05 Added End
        /*#CC07 START ADDED*/
        public static string AREAOPTIONAL
        {
            get {
                if (HttpContext.Current.Session["AREAOPTIONAL"] != null)
                    return Convert.ToString(HttpContext.Current.Session["AREAOPTIONAL"]);
                else
                    return "";
            }
        }
        /*#CC07 START END*/
        /*#CC12 START ADDED*/
        public static string TehsillDisplayMode
        {
            get { return Convert.ToString(HttpContext.Current.Session["TehsillDisplayMode"]); }
        }
        /*#CC12 START END*/

        /* #CC13 Add Start */
        public static string MultiLogin
        {
            get { return Convert.ToString(HttpContext.Current.Session["MultiLogin"]); }
        }
        /* #CC13 Add End */

        /* #CC14 Add Start */
        public static string ALLOWEDITRETAILER
        {
            get { return Convert.ToString(HttpContext.Current.Session["ALLOWEDITRETAILER"]); }
        }

        public static int STPARENTCHECK
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["STPARENTCHECK"]); }
        }
        /* #CC14 Add End */

        /* #CC15 Add Start */
        public static int SHOWCARTONSIZE
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["SHOWCARTONSIZE"]); }
        }

        public static int REQCARTONSIZE
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["REQCARTONSIZE"]); }
        }
        /* #CC15 Add End */

        /* #CC16 Add Start */
        public static int ChangeTinLabel
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["ChangeTinLabel"]); }
        }
        /* #CC16 Add End */

        /* #CC17 Add Start */
        public static int LOCATIONMAPPING
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["LOCATIONMAPPING"]); }
        }
        /* #CC17 Add End */
        /* #CC18 Add Start */
        public static int RetailerExcelUpload
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["RetailerExcelUpload"]); }
        }
        /* #CC18 Add End */
        /* #CC19 Add Start */
        public static int BRANDWISEBULLETIN
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["BRANDWISEBULLETIN"]); }
        }
        /* #CC19 Add End */
        /*#CC20 Add Start*/
        public static string NDHIERARCHYMAPPING
        {
            get { return Convert.ToString(HttpContext.Current.Session["NDHIERARCHYMAPPING"]); }
        }
        /*#CC20 Add END*/
        /*#CC21 START ADDED*/
        public static string ISLOGINEDITABLE
        {
            get { return Convert.ToString(HttpContext.Current.Session["ISLOGINEDITABLE"]); }
        }
        /*#CC21 START END*/

        /*#CC40 Added Started*/
        public static int SHOWExpiryDate
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["SKUExpiryDate"]); }
        }
        /*#CC40 Added End*/

        /*#CC42 Added Started*/
        public static int UserOtherDetail
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["UserOtherDetail"]); }
        }
        /*#CC42 Added End*/
        private static bool _IsWEB = true;

        public static bool IsWEB
        {
            get { return _IsWEB; }
            set { _IsWEB = value; }
        }

        public static DataTable GetStockUpdateTable()
        {
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn("SKUCode", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("Quantity", typeof(int));
            dt.Columns.Add(dc);
            dc = new DataColumn("StockBinTypeCode_From", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("StockBinTypeCode_To", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("SerialNo", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("BatchNo", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("GRNDate_From", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("GRNDate_To", typeof(DateTime));
            dt.Columns.Add(dc);
            return dt;
        }


        public static string DefaultRedirection(byte DefalutRedirectionFlag)
        {

            string ret = "~/Default.aspx";
            if (DefalutRedirectionFlag == 0)
            {
                ret = "~/Default.aspx";
            }
            else if (DefalutRedirectionFlag == 1)
            {
                ret = "~/Transactions/CommanSerial/ManageOpeningStockSerialWise.aspx";
            }
            else if (DefalutRedirectionFlag == 2)
            {
                if (IsWEB)
                    ret = "~/Retailer/specific/sonydefault.aspx";
                else
                    ret = "~/Home.aspx";
            }
            else if (DefalutRedirectionFlag == 3)
            {
                ret = "~/Retailer/Common/RetailerOpeningStock.aspx";
            }
            else if (DefalutRedirectionFlag == 4)
            {
                if (IsWEB)
                    ret = "~/Retailer/specific/sonydefault.aspx";
                else
                    ret = "~/Home.aspx";
            }
            return ret;
        }

        public static string CheckSerialNo(string getstr)
        {
            string SerialNo = getstr;
            string ErrorString = string.Empty;
            SerialNo = SerialNo.Replace("\r\n", ",");
            string[] strArray = SerialNo.Split(',');
            foreach (string objstr in strArray)
            {
                if (objstr.Length > SerialNoLength_Max || objstr.Length < SerialNoLength_Min)
                {
                    ErrorString = ErrorString + "," + objstr;
                }
            }
            if (ErrorString.StartsWith(","))
            {
                ErrorString = ErrorString.Remove(0, 1);
            }
            return ErrorString;
        }
        public static string CheckBatchNo(string getstr)
        {
            string BatchNo = getstr;
            string ErrorString = string.Empty;
            BatchNo = BatchNo.Replace("\r\n", ",");
            string[] strArray = BatchNo.Split(',');
            foreach (string objstr in strArray)
            {
                if (objstr.Length > BatchNoLength_Max || objstr.Length < BatchNoLength_Min)
                {
                    ErrorString = ErrorString + "," + objstr;

                }
            }
            if (ErrorString.StartsWith(","))
            {
                ErrorString = ErrorString.Remove(0, 1);
            }
            return ErrorString;
        }
        public DataTable AddDataColumns(string _DtColumnNames, string _DtColumnTypes)
        {
            DataTable _dtMyData = new DataTable();
            // Assuming that both Column name and Column type contains equal length.
            string[] dcNames = { };
            if (_DtColumnNames.Contains(","))
            {
                dcNames = _DtColumnNames.Split(',');
            }

            string[] dcTypes = { };
            if (_DtColumnTypes.Contains(","))
            {
                dcTypes = _DtColumnTypes.Split(',');
            }

            DataColumn dc = new DataColumn();
            for (int i = 0; (i < dcNames.Length && (dcNames.Length == dcTypes.Length)); i++)
            {
                if (dcTypes[i].ToLower() == "string")
                {
                    dc = new DataColumn(dcNames[i], typeof(string));
                }
                else if (dcTypes[i].ToLower() == "int32")
                {
                    dc = new DataColumn(dcNames[i], typeof(Int32));
                }
                else if (dcTypes[i].ToLower() == "double")
                {
                    dc = new DataColumn(dcNames[i], typeof(double));
                }
                else if (dcTypes[i].ToLower() == "boolean")
                {
                    dc = new DataColumn(dcNames[i], typeof(Boolean));
                }
                try
                {
                    _dtMyData.Columns.Add(dc);
                }
                catch { }
            }
            return _dtMyData;
        }

        /// <summary>
        /// List of All Dates from Date Range provided as Param
        /// </summary>
        /// <param name="FirstItemText">First Item Text</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public static List<ListItem> GetListByStartAndEndDate(string FirstItemText, DateTime StartDate, DateTime EndDate)
        {


            List<ListItem> DayList = new List<ListItem>();
            DayList.Add(new ListItem(FirstItemText, "0"));
            DateTime ThisMonth = EndDate;
            while (ThisMonth.Date >= StartDate.Date)
            {
                DayList.Add(new ListItem(ThisMonth.ToString("dd") + " " + ThisMonth.ToString("MMMM") + " " + ThisMonth.Year.ToString(), ThisMonth.ToShortDateString()));
                ThisMonth = ThisMonth.AddDays(-1);
            }

            return DayList;
        }


        public static bool IsOpeningStockEntered
        {
            get
            {
                if (HttpContext.Current.Session["IsOpeningStockEntered"] == null)
                    return false;
                else
                    return Convert.ToBoolean(HttpContext.Current.Session["IsOpeningStockEntered"].ToString());

            }
        }
        //Pankaj Dhingra
        public void ChangedExcelSheetNames(ref DataSet dsExcel, string[] strExcelSheetName, Int16 intSheetCount)
        {
            Int16 index;
            for (index = 0; index < intSheetCount; index++)
                dsExcel.Tables[index].TableName = strExcelSheetName[index];
            dsExcel.AcceptChanges();

        }
        //Pankaj Dhingra 11-Oct-2014
        public DataColumn AddColumn(string columnValue, string ColumnName, Type ColumnType)
        {
            DataColumn dcSession = new DataColumn();
            dcSession.ColumnName = ColumnName;

            if (ColumnType == typeof(int))
            {
                dcSession.DataType = typeof(System.Int32);
                dcSession.DefaultValue = Convert.ToInt32(columnValue);
            }
            if (ColumnType == typeof(System.String))
            {
                dcSession.DataType = typeof(System.String);
                dcSession.DefaultValue = columnValue;
            }
            return dcSession;
        }
        /*Added #CC22*/
        public static DataTable GetEnumByTableName(string FileName, string TableName)
        {
            System.Data.DataTable dt = new DataTable();
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

        public void MoveFileFromTemp(DataTable dataTable)
        {
            try
            {
                if (dataTable == null)
                    return;

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    bool uploadDone = false;
                    if (Resources.AppConfig.SaveFileOn.ToString() == "0")
                    {
                        string strImageConString = ConfigurationManager.AppSettings["AzureConnectionString"].ToString();
                        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(strImageConString);
                        Microsoft.WindowsAzure.StorageClient.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                        CloudBlobContainer container = blobClient.GetContainerReference(Resources.AppConfig.AzureContainerName);
                        container.CreateIfNotExist();
                        string[] Blog = dataTable.Rows[i]["ImageRelativePath"].ToString().ToLower().Split(':');
                        string NewBlob = Blog[1].ToLower();
                        CloudBlob blob = container.GetBlobReference(NewBlob);
                        container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                        using (var fileStream = System.IO.File.OpenRead(dataTable.Rows[i]["TempFileLocation"].ToString()))
                        {
                            if (fileStream != null && blob != null)
                            {
                                blob.UploadFromStream(fileStream);
                                uploadDone = true;
                            }
                        }
                    }
                    else
                    {
                        File.Move(dataTable.Rows[i]["TempFileLocation"].ToString(), dataTable.Rows[i]["ImageRelativePath"].ToString());
                    }
                   
                    if (uploadDone)
                        File.Delete(dataTable.Rows[i]["TempFileLocation"].ToString());
                }
            }
            catch (Exception ex)
            {
               
            }
          
        }
        public void MoveFileFromTemp(DataTable dataTable, int Server)
        {
            try
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    File.Move(dataTable.Rows[i]["TempFileLocation"].ToString(), dataTable.Rows[i]["ImageRelativePath"].ToString());
                }
            }
            catch (Exception ex)
            {
              
            }
        }
        public static byte[] ImageToBinary(string imagePath)
        {
            FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, (int)fileStream.Length);
            fileStream.Close();
            return buffer;
        }
        //#CC39 start
        public static void DownloadExportToExcelFile(string fullVirtualPath)
        { 
            try
            {
                
                if (fullVirtualPath.Contains("DownloadUpload"))
                {
                    string[] strpt = { "DownloadUpload" };
                    string[] array = fullVirtualPath.Split(strpt, 2, StringSplitOptions.None);
                    if (array.Length > 1)
                    {
                        fullVirtualPath = array[1];
                    }

                    fullVirtualPath = fullVirtualPath.Replace("/", "\\");
                    fullVirtualPath = Convert.ToString(ConfigurationManager.AppSettings["PhysicalPath"].ToString()) + "\\" + fullVirtualPath;
                    
                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                if (File.Exists(fullVirtualPath))
                {
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(fullVirtualPath));
                    HttpContext.Current.Response.ContentType = ReturnExtension(Path.GetExtension(fullVirtualPath).ToLower());
                    HttpContext.Current.Response.WriteFile(fullVirtualPath);
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.Close();
                    
                }
            }
            
        }
        private static string ReturnExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".doc":
                    return "application/ms-word";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".zip":
                    return "application/zip";
                case ".xls":
                case ".xlsx":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".wav":
                    return "audio/wav";
                case ".mp3":
                    return "audio/mpeg3";
                case ".mpg":
                case "mpeg":
                    return "video/mpeg";
                case ".rtf":
                    return "application/rtf";
                case ".asp":
                    return "text/asp";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".ppt":
                    return "application/mspowerpoint";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                case ".sdxl":
                    return "application/xml";
                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";
                default:
                    return "application/octet-stream";
            }
        }//#CC39 end
    }
    
    //#CC31
    public static class ExportToCSV
    {

        public static void ToCSV(this DataTable dtDataTable, string strFileName, string strFilePath)
        {
            try
            {
                string strExcelFileName = strFileName + System.DateTime.Now.Ticks + ".csv";

                string strServerPath = strFilePath + strExcelFileName;

                StreamWriter sw = new StreamWriter(strServerPath, false);
            //headers  
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(","))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();

                // Opening File/ Downloading File
            HttpContext.Current.Response.Redirect(PageBase.VirtualPath + PageBase.strGlobalDownloadExcelPathRoot + strExcelFileName);
            }

            catch (Exception)
            {

                throw;
            }
        }
    }



}
