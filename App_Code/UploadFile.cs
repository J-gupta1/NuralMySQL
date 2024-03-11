/* 
 * 28-Oct-2015, Sumit Maurya, #CC01, New Check Added if Session["RetailerBankDetail"].ToString() == "1" then it will not validate Retailer bank Detail .
 * 30-Oct-2015, Sumit Maurya, #CC02, New column PANNo validation added for RetailerBankDetail.
 * 18-Mar-2016, Sumit Maurya, #CC03, New column added CounterValue
 * 30-Mar-2016, Sumit Maurya, #CC04, New check added to modify TehsilName column from dataset according to config value.
 * 07-Jun-2016< Sumit Maurya, #CC05, Column name TinNumber/VATNumber Changed according to config.
 * 20-Oct-2016, Sumit Maurya ,#CC06, Spelling changed from "kinldy" to "kindly"
 * 11-Aug-2017, Sumit Maurya, #CC07, Column name chagended from VATNumber To GSTNumber
 * 09-Feb-2018,Vijay Kumar Prajapati,#CC08,new method ValidateExcelFile for DOA.
 * 22-May-2018,Rajnish Kumar ,#CC09,For WhatsApp Mobile Number.
 * 12-June-2018, Rakesh Raj, #CC10, For 'Cannot find table 0.' if blank sheet uploaded 
 * 29-June-2018, Rakesh Raj, #CC11, For Update feature from Excle Sheet
 * 11-July-2018, Rakesh Raj, #CC12, Blank Space Column Validation using Trim function/Exception thrown in case of blank space in column
 * 25-Jul-2018, Sumit Maurya, #CC13, Error message is now displayed from Resourcefile (Done for ComioV5).
 * 30-Jan-2020,Vijay Kumar Prajapati,#CC14,Added New method for SAPIntergation(Done for Inone).
 * 16-April-2020,Vijay Kumar Prajapati,#CC15,Added New Mathod with CompanyId
 */
using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using DataAccess;
using System.Web.UI;
using DocumentFormat;
using ZedService;
namespace BussinessLogic
{
    public class UploadFile
    {
        #region Variables and objects

        private string strMSg;
        private string strUploadedFileName = string.Empty;
        private string strRootFolerPath;
        private DataTable dtUpload;
        //        private static int counter = 1;
        private int counter = 1;
        private int intvalueForDate;
        public bool issaleschannel;

        #endregion




        #region Properties
        public int ValueForDate
        {
            get { return intvalueForDate; }
            set { intvalueForDate = value; }
        }

        public string RootFolerPath
        {
            get { return strRootFolerPath; }
            set { strRootFolerPath = value; }
        }
        public string UploadedFileName
        {
            get { return strUploadedFileName; }
            set { strUploadedFileName = value; }
        }
        public string Message
        {
            get { return strMSg; }
            set
            {
                strMSg = value;
            }
        }
        private EnumData.EnumSAPModuleName MODDataUpload;
        public EnumData.eUploadExcelValidationType UploadValidationType
        { get; set; }

        public EnumData.EnumSAPModuleName UploadCheckNegativeStock
        { get; set; }
        public EnumData.eSchemeTemplateType SchemeType
        { get; set; }
        public EnumData.eTargetTemplateType TemplateType
        {
            get;
            set;
        }


        public string IsSku
        {
            get;
            set;
        }
        private string _DSR_Path;
        public string DSR_Path
        {
            get { return _DSR_Path; }
            set { _DSR_Path = value; }
        }
        #endregion
        #region Public Methods
        public byte uploadValidExcel(ref DataSet Ds, string Tablename)
        {
            int rowCownt = 0;

            using (CommonData objc = new CommonData())
            {

                objc.UploadTableName = Tablename;
                objc.TemplateType = TemplateType;
                objc.UploadValidationType = UploadValidationType;
                objc.CompanyId = PageBase.ClientId;
                dtUpload = objc.getSchemaForUpload();

            }

            OpenXMLExcel objexcel = new OpenXMLExcel();
            rowCownt = objexcel.CountExcelRows(strRootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
            if (rowCownt >= Convert.ToInt64(PageBase.ValidExcelRows))
            {
                Message = "Maximum" + PageBase.ValidExcelRows + "rows to upload excel file";
                return 0;
            }
            Ds = objexcel.ImportExcelFileV2(strRootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
            //IsSku = Ds.Tables[0].Columns[0].ColumnName;
            //#CC10
            /// Was displaying message 'Cannot find table 0.' if blank sheet uploaded instead of  
            /// Please enter data in excel sheet message 
            /// Rakesh Raj/9/6/2018
            if (Ds.Tables.Count == 0)
            {
                Message = "Please enter data in excel sheet";
                return 0;
            }
            if (Ds.Tables[0].Columns.Count != dtUpload.Columns.Count)
            {
                Message = Resources.Messages.InvalidExcelColumnsDoesntMatch; /* #CC13 Added */ /*"Invalid excel sheet, columns doesn't match to required feilds"; #CC13 Commented */
                return 0;
            }
            for (int i = 0; i < dtUpload.Columns.Count; i++)
            {
                Ds.Tables[0].Columns[i].ColumnName = Ds.Tables[0].Columns[i].ColumnName.Trim().Replace(" ", "");
                if (dtUpload.Columns[i].ColumnName.ToLower() != Ds.Tables[0].Columns[i].ColumnName.ToLower())
                {
                    Message = "Invalid excel sheet," + Ds.Tables[0].Columns[i].ColumnName + " missing or not in correct sequense, kindly download template again."; /* #CC06 Spelling changed */
                    return 0;
                }
            }
            bool blnValidaData = true;
            if (Ds.Tables[0].Columns.Contains("Error") == false)
            {
                Ds.Tables[0].Columns.Add("Error", System.Type.GetType("System.String"));
            }

            for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
            {
                if (Ds.Tables[0].Columns[i].ColumnName.ToLower() == "dispatchqty")
                {
                    Ds.Tables[0].Columns[i].ColumnName = "Quantity";
                    break;
                }
            }
            if (UploadCheckNegativeStock == EnumData.EnumSAPModuleName.BTMDataUpload)
                UploadCheckNegativeStock = EnumData.EnumSAPModuleName.BTMDataUpload;
            else if (UploadCheckNegativeStock == EnumData.EnumSAPModuleName.GRNDataUpload)
                UploadCheckNegativeStock = EnumData.EnumSAPModuleName.GRNDataUpload;
            blnValidaData = ValidateExcel(ref Ds);
            if (blnValidaData == false)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        public byte uploadValidExcelRetailer(ref DataSet Ds, string Tablename)
        {
            int rowCownt = 0;

            using (CommonData objc = new CommonData())
            {

                objc.UploadTableName = Tablename;
                objc.TemplateType = TemplateType;
                objc.UploadValidationType = UploadValidationType;

                dtUpload = objc.getSchemaForUpload();



            }

            OpenXMLExcel objexcel = new OpenXMLExcel();
            rowCownt = objexcel.CountExcelRows(strRootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
            if (rowCownt >= Convert.ToInt64(PageBase.ValidExcelRows))
            {
                Message = "Maximum" + PageBase.ValidExcelRows + "rows to upload excel file";
                return 0;
            }
            Ds = objexcel.ImportExcelFileV2(strRootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
            if (Ds.Tables.Count == 0)
            {
                return 0;
            }


            HttpContext.Current.Session["RetailerUploadData"] = Ds.Copy();

            //IsSku = Ds.Tables[0].Columns[0].ColumnName;
            if (HttpContext.Current.Session["RETAILERLOGIN"] != null)
            {
                if (Convert.ToInt32(HttpContext.Current.Session["RETAILERLOGIN"]) == 1)
                {
                    if (Ds.Tables[0].Columns.Contains("RetailerUserName"))
                    {
                        Ds.Tables[0].Columns.Remove("RetailerUserName");
                    }
                }
            }
            if (HttpContext.Current.Session["RETAILERHIERLVLID"] != null)
            {
                if (Convert.ToInt32(HttpContext.Current.Session["RETAILERHIERLVLID"]) > 0)
                {
                    if (Ds.Tables[0].Columns.Contains("MappedOrgnhierarchy"))
                    {
                        Ds.Tables[0].Columns.Remove("MappedOrgnhierarchy");
                    }
                }
            }
            if (HttpContext.Current.Session["RETAILERCODEAUTO"] != null)
            {
                if (Convert.ToInt32(HttpContext.Current.Session["RETAILERCODEAUTO"]) == 0)
                {
                    if (Ds.Tables[0].Columns.Contains("RetailerCode"))
                    {
                        Ds.Tables[0].Columns.Remove("RetailerCode");
                    }
                }
            }

            //#CC11
            if (HttpContext.Current.Session["UPDRTLR"] != null)
            {
                if (Convert.ToInt32(HttpContext.Current.Session["UPDRTLR"]) == 1)
                {
                    if (Ds.Tables[0].Columns.Contains("RetailerCode"))
                    {
                        Ds.Tables[0].Columns.Remove("RetailerCode");
                    }

                    if (Ds.Tables[0].Columns.Contains("Active"))
                    {
                        Ds.Tables[0].Columns.Remove("Active");
                    }

                    if (dtUpload.Columns.Contains("SalesChannelCode"))
                    {
                        dtUpload.Columns.Remove("SalesChannelCode");
                    }

                    if (dtUpload.Columns.Contains("SalesmanName"))
                    { dtUpload.Columns.Remove("SalesmanName"); }


                }
            }

            if (HttpContext.Current.Session["RetailerOpStockDate"] != null)
            {
                if (HttpContext.Current.Session["RetailerOpStockDate"].ToString() == "0")
                {
                    if (Ds.Tables[0].Columns.Contains("StockOpeningDate"))
                    {
                        Ds.Tables[0].Columns.Remove("StockOpeningDate");
                    }
                }
            }
            /* #CC01 Add Start */
            if (HttpContext.Current.Session["RetailerBankDetail"] != null)
            {
                if (HttpContext.Current.Session["RetailerBankDetail"].ToString() == "1")
                {
                    if (Ds.Tables[0].Columns.Contains("BankName"))
                    {
                        Ds.Tables[0].Columns.Remove("BankName");
                    }
                    if (Ds.Tables[0].Columns.Contains("AccountHolderName"))
                    {
                        Ds.Tables[0].Columns.Remove("AccountHolderName");
                    }
                    if (Ds.Tables[0].Columns.Contains("BankAccountNumber"))
                    {
                        Ds.Tables[0].Columns.Remove("BankAccountNumber");
                    }
                    if (Ds.Tables[0].Columns.Contains("BranchLocation"))
                    {
                        Ds.Tables[0].Columns.Remove("BranchLocation");
                    }
                    if (Ds.Tables[0].Columns.Contains("IFSCCode"))
                    {
                        Ds.Tables[0].Columns.Remove("IFSCCode");
                    }
                    /* #CC02 Add Start */
                    if (Ds.Tables[0].Columns.Contains("PANNo"))
                    {
                        Ds.Tables[0].Columns.Remove("PANNo");
                    }
                    /* #CC02 Add End */
                }
            }
            /* #CC01 Add End */

            /* #CC03 Add Start */


            /*#CC09 start*/
            if (HttpContext.Current.Session["WhatsAppMobileNumber"] != null)
            {
                if (HttpContext.Current.Session["WhatsAppMobileNumber"].ToString() == "1")
                {
                    if (Ds.Tables[0].Columns.Contains("WhatsAppNumber"))
                    {
                        Ds.Tables[0].Columns.Remove("WhatsAppNumber");
                    }
                }
            }/*#CC09 end*/
            if (HttpContext.Current.Session["PotentialVolDisplay"] != null)
            {
                if (HttpContext.Current.Session["PotentialVolDisplay"].ToString() == "1")
                {
                    if (Ds.Tables[0].Columns.Contains("CounterPotentialValue"))
                    {
                        Ds.Tables[0].Columns.Remove("CounterPotentialValue");
                    }
                }
            }
            /* #CC03 Add End */

            /* #CC04 Add Start */
            if (HttpContext.Current.Session["TehsillDisplayMode"] != null)
            {
                if (PageBase.TehsillDisplayMode == "1")
                {
                    if (Ds.Tables[0].Columns.Contains("TehsilName"))
                    {
                        Ds.Tables[0].Columns.Remove("TehsilName");
                    }
                }
            }
            /* #CC04 Add End */

            /* #CC05 Add Start */
            if (PageBase.ChangeTinLabel == 1)
            {
                /*
                 #CC07 Comment Start
                 if (Ds.Tables[0].Columns.Contains("VATNumber"))
                {
                    Ds.Tables[0].Columns["VATNumber"].ColumnName = "TinNumber";
                }
                #CC07 Comment End */
                /* #CC07 Add Start */
                if (Ds.Tables[0].Columns.Contains("GSTNumber"))
                {
                    Ds.Tables[0].Columns["GSTNumber"].ColumnName = "TinNumber";
                }
                /* #CC07 Add End */
            }
            /* #CC05 Add End */

            if (Ds.Tables[0].Rows.Count == 0)
            {
                Message = "Please enter data in excel sheet";
                return 0;
            }
            if (Ds.Tables[0].Columns.Count != dtUpload.Columns.Count)
            {
                Message = Resources.Messages.InvalidExcelColumnsDoesntMatch; /* #CC13 Added */ /*"Invalid excel sheet, columns doesn't match to required feilds"; #CC13 Commented */
                return 0;
            }
            for (int i = 0; i < dtUpload.Columns.Count; i++)
            {
                Ds.Tables[0].Columns[i].ColumnName = Ds.Tables[0].Columns[i].ColumnName.Trim().Replace(" ", "");
                if (dtUpload.Columns[i].ColumnName.ToLower() != Ds.Tables[0].Columns[i].ColumnName.ToLower())
                {
                    Message = "Invalid excel sheet," + Ds.Tables[0].Columns[i].ColumnName + " missing or not in correct sequense, kindly download template again."; /* #CC06 Spelling changed */
                    return 0;
                }
            }
            bool blnValidaData = true;
            if (Ds.Tables[0].Columns.Contains("Error") == false)
            {
                Ds.Tables[0].Columns.Add("Error", System.Type.GetType("System.String"));
            }

            for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
            {
                if (Ds.Tables[0].Columns[i].ColumnName.ToLower() == "dispatchqty")
                {
                    Ds.Tables[0].Columns[i].ColumnName = "Quantity";
                    break;
                }
            }
            if (UploadCheckNegativeStock == EnumData.EnumSAPModuleName.BTMDataUpload)
                UploadCheckNegativeStock = EnumData.EnumSAPModuleName.BTMDataUpload;
            else if (UploadCheckNegativeStock == EnumData.EnumSAPModuleName.GRNDataUpload)
                UploadCheckNegativeStock = EnumData.EnumSAPModuleName.GRNDataUpload;
            blnValidaData = ValidateExcel(ref Ds);
            if (blnValidaData == false)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }
        public byte uploadValidExcelForSecondarySales(ref DataSet Ds)
        {
            OpenXMLExcel objexcel = new OpenXMLExcel();
            bool blnValidaData = true;
            //Ds = objexcel.ImportExcelFile(strRootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
            Ds = objexcel.ImportExcelFileV2(strRootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);

            if (Ds.Tables[0].Columns.Count < 4)// Column number from excel should be same as SKU Count or Brand Count depends on target Category.
            {
                Message = "Invalid excel sheet, columns missing";
                return 0;
            }


            using (CommonData objc = new CommonData())
            {
                objc.SalesCHannelID = PageBase.SalesChanelID;
                objc.BrandID = PageBase.Brand;

                dtUpload = objc.getSchemaForUploadSecondarySales(Ds);
                dtUpload.PrimaryKey = new DataColumn[] { dtUpload.Columns[0] };


                for (int i = 3; i < Ds.Tables[0].Columns.Count; i++)
                {

                    {
                        if (!dtUpload.Rows.Contains(Ds.Tables[0].Columns[i].ColumnName))
                        {
                            Message = "Invalid excel sheet," + Ds.Tables[0].Columns[i].ColumnName + " missing or not in correct sequence, kindly download the template again.";
                            return 0;
                        }
                    }
                }
            }
            if (Ds.Tables[0].Columns.Contains("Error") == false)
            {
                Ds.Tables[0].Columns.Add("Error", System.Type.GetType("System.String"));
            }

            for (int iRow = 0; iRow < Ds.Tables[0].Rows.Count; iRow++)
            {
                for (int iCol = 3; iCol < Ds.Tables[0].Columns.Count - 1; iCol++)
                {
                    if (ServerValidation.IsInteger(Ds.Tables[0].Rows[iRow][iCol], true) != 0)
                    {
                        Ds.Tables[0].Rows[iRow].BeginEdit();
                        Ds.Tables[0].Rows[iRow]["Error"] = (Ds.Tables[0].Rows[iRow]["Error"] == DBNull.Value ? "" : Environment.NewLine + Ds.Tables[0].Rows[iRow]["Error"]) + Ds.Tables[0].Columns[iCol].ColumnName + " Value should be numeric.";
                        Ds.Tables[0].Rows[iRow].EndEdit();
                    }
                }
            }

            Ds.AcceptChanges();

            if (Ds.Tables[0].Select("Error is not null").Length > 0)
            {
                blnValidaData = false;
            }

            if (blnValidaData == false)
            {
                return 2;
            }
            else
            {
                return 1;
            }


        }

        public DataSet ImportExcelFileMMX()
        {
            OpenXMLExcel objexcel = new OpenXMLExcel();
            DataSet Ds = new DataSet();
            Ds = objexcel.ImportExcelFileV2(strRootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
            Ds.AcceptChanges();
            return Ds;
        }



        public byte uploadValidExcelSecondarySales(ref DataSet Ds, string Tablename)
        {
            int rowCownt = 0;

            using (CommonData objc = new CommonData())
            {

                objc.UploadTableName = Tablename;
                objc.TemplateType = TemplateType;
                objc.UploadValidationType = UploadValidationType;
                dtUpload = objc.getSchemaForUpload();
            }

            OpenXMLExcel objexcel = new OpenXMLExcel();
            rowCownt = objexcel.CountExcelRows(strRootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
            if (rowCownt >= Convert.ToInt16(PageBase.ValidExcelRows))
            {
                Message = "Maximum" + PageBase.ValidExcelRows + "rows to upload excel file";
                return 0;
            }
            Ds = objexcel.ImportExcelFileV2(strRootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);

            if (Ds.Tables[0].Rows.Count == 0)
            {
                Message = "Please enter data in excel sheet";
                return 0;
            }
            if (Ds.Tables[0].Columns.Count != dtUpload.Columns.Count)
            {
                Message = Resources.Messages.InvalidExcelColumnsDoesntMatch; /* #CC13 Added */ /*"Invalid excel sheet, columns doesn't match to required feilds"; #CC13 Commented */
                return 0;
            }
            for (int i = 0; i < dtUpload.Columns.Count; i++)
            {
                Ds.Tables[0].Columns[i].ColumnName = Ds.Tables[0].Columns[i].ColumnName.Trim().Replace(" ", "");
                if (dtUpload.Columns[i].ColumnName.ToLower() != Ds.Tables[0].Columns[i].ColumnName.ToLower())
                {
                    Message = "Invalid excel sheet," + Ds.Tables[0].Columns[i].ColumnName + " missing or not in correct sequense, kindly download template again."; /* #CC06 Spelling changed */
                    return 0;
                }
            }
            bool blnValidaData = true;
            if (Ds.Tables[0].Columns.Contains("Error") == false)
            {
                Ds.Tables[0].Columns.Add("Error", System.Type.GetType("System.String"));
            }

            for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
            {
                if (Ds.Tables[0].Columns[i].ColumnName.ToLower() == "dispatchqty")
                {
                    Ds.Tables[0].Columns[i].ColumnName = "Quantity";
                    break;
                }
            }
            if (UploadCheckNegativeStock == EnumData.EnumSAPModuleName.BTMDataUpload)
                UploadCheckNegativeStock = EnumData.EnumSAPModuleName.BTMDataUpload;
            else if (UploadCheckNegativeStock == EnumData.EnumSAPModuleName.GRNDataUpload)
                UploadCheckNegativeStock = EnumData.EnumSAPModuleName.GRNDataUpload;
            blnValidaData = ValidateExcel(ref Ds);
            if (blnValidaData == false)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }
        public byte uploadValidExcelForBTM(ref DataSet Ds, string Tablename)
        {
            int rowCownt = 0;

            using (CommonData objc = new CommonData())
            {
                objc.UploadTableName = Tablename;
                dtUpload = objc.getSchemaForUpload();
            }


            OpenXMLExcel objexcel = new OpenXMLExcel();

            // condition is used when file extension is not xls then OLEDB used to get data from excel
            if (strUploadedFileName.Contains(".xlsx") == true)
                Ds = objexcel.ImportExcelFileV2(strRootFolerPath + strUploadedFileName);
            else
            {
                Message = "Please upload .xlsx file.";
                return 0;
            }

            if (Ds.Tables[0].Rows.Count == 0)
            {
                Message = "Please enter data in excel sheet";
                return 0;
            }
            if (Ds.Tables[0].Columns.Count != dtUpload.Columns.Count)
            {
                Message = Resources.Messages.InvalidExcelColumnsDoesntMatch; /* #CC13 Added */ /*"Invalid excel sheet, columns doesn't match to required feilds"; #CC13 Commented */
                return 0;
            }



            for (int i = 0; i < dtUpload.Columns.Count; i++)
            {
                Ds.Tables[0].Columns[i].ColumnName = Ds.Tables[0].Columns[i].ColumnName.Trim().Replace(" ", "");
                Ds.Tables[0].Columns[i].ColumnName = Ds.Tables[0].Columns[i].ColumnName.Trim().Replace("#", ".");

                Ds.Tables[0].Columns[i].ColumnName = Ds.Tables[0].Columns[i].ColumnName.Trim();
                if (dtUpload.Columns[i].ColumnName.ToLower() != Ds.Tables[0].Columns[i].ColumnName.ToLower())
                {
                    Message = "Invalid excel sheet," + Ds.Tables[0].Columns[i].ColumnName + " missing or not in correct sequense, kindly download template again."; /* #CC06 Spelling changed */
                    return 0;
                }
            }





            bool blnValidaData = true;
            if (Ds.Tables[0].Columns.Contains("Error") == false)
            {
                Ds.Tables[0].Columns.Add("Error", System.Type.GetType("System.String"));
            }

            for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
            {
                if (Ds.Tables[0].Columns[i].ColumnName.ToLower() == "dispatchqty")
                {
                    Ds.Tables[0].Columns[i].ColumnName = "Quantity";
                    break;
                }
            }


            if (UploadCheckNegativeStock == EnumData.EnumSAPModuleName.BTMDataUpload)
                UploadCheckNegativeStock = EnumData.EnumSAPModuleName.BTMDataUpload;
            else if (UploadCheckNegativeStock == EnumData.EnumSAPModuleName.GRNDataUpload)
                UploadCheckNegativeStock = EnumData.EnumSAPModuleName.GRNDataUpload;

            blnValidaData = ValidateExcelForSap(ref Ds, false);


            if (blnValidaData == false)
            {
                return 2;
            }
            else
            {
                return 1;
            }


        }
        public byte uploadValidExcelForGrnGfive(ref DataSet Ds, string Tablename)
        {
            int rowCownt = 0;

            using (CommonData objc = new CommonData())
            {
                objc.UploadTableName = Tablename;
                dtUpload = objc.getSchemaForUpload();
            }

            OpenXMLExcel objexcel = new OpenXMLExcel();
            //   rowCownt = objexcel.CountExcelRows(strRootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
            rowCownt = objexcel.CountExcelRows(strRootFolerPath + strUploadedFileName);
            if (rowCownt >= Convert.ToInt16(PageBase.ValidExcelRows))
            {
                Message = "Maximum" + PageBase.ValidExcelRows + "rows to upload excel file";
                return 0;
            }
            //  Ds = objexcel.ImportExcelFile(strRootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
            Ds = objexcel.ImportExcelFileV2(strRootFolerPath + strUploadedFileName);
            if (Ds.Tables[0].Rows.Count == 0)
            {
                Message = "Please enter data in excel sheet";
                return 0;
            }
            if (Ds.Tables[0].Columns.Count != dtUpload.Columns.Count)
            {
                Message = Resources.Messages.InvalidExcelColumnsDoesntMatch; /* #CC13 Added */ /*"Invalid excel sheet, columns doesn't match to required feilds"; #CC13 Commented */
                return 0;
            }



            for (int i = 0; i < dtUpload.Columns.Count; i++)
            {
                Ds.Tables[0].Columns[i].ColumnName = Ds.Tables[0].Columns[i].ColumnName.Trim().Replace(" ", "");    //Pankaj Kumar
                if (dtUpload.Columns[i].ColumnName.ToLower() != Ds.Tables[0].Columns[i].ColumnName.ToLower())
                {
                    Message = "Invalid excel sheet," + Ds.Tables[0].Columns[i].ColumnName + " missing or not in correct sequense, kindly download template again."; /* #CC06 Spelling changed */
                    return 0;
                }
            }





            bool blnValidaData = true;
            if (Ds.Tables[0].Columns.Contains("Error") == false)
            {
                Ds.Tables[0].Columns.Add("Error", System.Type.GetType("System.String"));
            }

            for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
            {
                if (Ds.Tables[0].Columns[i].ColumnName.ToLower() == "dispatchqty")
                {
                    Ds.Tables[0].Columns[i].ColumnName = "Quantity";
                    break;
                }
            }


            if (UploadCheckNegativeStock == EnumData.EnumSAPModuleName.BTMDataUpload)
                UploadCheckNegativeStock = EnumData.EnumSAPModuleName.BTMDataUpload;
            else if (UploadCheckNegativeStock == EnumData.EnumSAPModuleName.GRNDataUpload)
                UploadCheckNegativeStock = EnumData.EnumSAPModuleName.GRNDataUpload;

            blnValidaData = ValidateExcel(ref Ds, false);


            if (blnValidaData == false)
            {
                return 2;
            }
            else
            {
                return 1;
            }


        }
        public byte uploadValidExcelModForGfive(ref DataSet Ds, string Tablename)
        {
            int ErrorCounter = 1;
            int rowCownt = 0;
            DataTable dtFullRecord = new DataTable();
            DataSet dsNew = new DataSet();
            DataTable dtFilter;

            using (CommonData objc = new CommonData())
            {
                objc.UploadTableName = Tablename;
                dtUpload = objc.getSchemaForUpload();
            }

            OpenXMLExcel objexcel = new OpenXMLExcel();
            // rowCownt = objexcel.CountExcelRows(strRootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
            rowCownt = objexcel.CountExcelRows(strRootFolerPath + strUploadedFileName);
            if (rowCownt >= Convert.ToInt16(PageBase.ValidExcelRows))
            {
                Message = "Maximum" + PageBase.ValidExcelRows + "rows to upload excel file";
                return 0;
            }
            //   Ds = objexcel.ImportExcelFile(strRootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
            Ds = objexcel.ImportExcelFileV2(strRootFolerPath + strUploadedFileName);
            dtFullRecord = Ds.Tables[0].Clone();
            //dtFullRecord = dtUpload.Clone();
            dsNew = Ds;

            if (Ds.Tables[0].Rows.Count == 0)
            {
                Message = "Please enter data in excel sheet";
                return 0;
            }
            if (Ds.Tables[0].Columns.Count != dtUpload.Columns.Count)
            {
                Message = Resources.Messages.InvalidExcelColumnsDoesntMatch; /* #CC13 Added */ /*"Invalid excel sheet, columns doesn't match to required feilds"; #CC13 Commented */
                return 0;
            }



            for (int i = 0; i < dtUpload.Columns.Count; i++)
            {
                Ds.Tables[0].Columns[i].ColumnName = Ds.Tables[0].Columns[i].ColumnName.Trim().Replace(" ", "");
                if (dtUpload.Columns[i].ColumnName.ToLower() != Ds.Tables[0].Columns[i].ColumnName.ToLower())
                {
                    Message = "Invalid excel sheet," + Ds.Tables[0].Columns[i].ColumnName + " missing or not in correct sequense, kindly download template again."; /* #CC06 Spelling changed */
                    return 0;
                }
            }





            bool blnValidaData = true;
            if (Ds.Tables[0].Columns.Contains("Error") == false)
            {
                Ds.Tables[0].Columns.Add("Error", System.Type.GetType("System.String"));
            }

            for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
            {
                if (Ds.Tables[0].Columns[i].ColumnName.ToLower() == "total_qty")
                {
                    Ds.Tables[0].Columns[i].ColumnName = "Quantity";
                    break;
                }
            }
            for (int count = 0; count < 2; count++)
            {
                Ds = null;
                if (count == 0)
                {
                    dsNew.Tables[0].DefaultView.RowFilter = "Quantity>0";
                    dtFilter = dsNew.Tables[0].DefaultView.ToTable();
                    Ds = new DataSet();
                    Ds.Merge(dtFilter);
                }
                else
                {
                    dsNew.Tables[0].DefaultView.RowFilter = "Quantity<0";
                    dtFilter = dsNew.Tables[0].DefaultView.ToTable();
                    Ds = new DataSet();
                    Ds.Merge(dtFilter);
                }
                UploadCheckNegativeStock = EnumData.EnumSAPModuleName.MODDataUpload;
                if (Ds.Tables[0].Rows.Count > 0)
                    blnValidaData = ValidateExcel(ref Ds, true);
                if (blnValidaData == false)
                {
                    ErrorCounter = ErrorCounter + 1;
                }
                if (blnValidaData == true)
                {
                    foreach (DataRow dr1 in Ds.Tables[0].Rows)
                    {
                        dtFullRecord.ImportRow(dr1);
                    }
                }

            }


            if (UploadCheckNegativeStock == EnumData.EnumSAPModuleName.BTMDataUpload)
                UploadCheckNegativeStock = EnumData.EnumSAPModuleName.BTMDataUpload;
            else if (UploadCheckNegativeStock == EnumData.EnumSAPModuleName.GRNDataUpload)
                UploadCheckNegativeStock = EnumData.EnumSAPModuleName.GRNDataUpload;

            //blnValidaData = ValidateExcel(ref Ds,true);


            //if (blnValidaData == false)
            //{
            //    counter = counter + 1;
            //}
            //else
            //{
            //    foreach (DataRow dr1 in Ds.Tables[0].Rows)
            //    {
            //        dtFullRecord.ImportRow(dr1);
            //    }
            //}
            if (ErrorCounter >= 2)
            {
                ErrorCounter = 1;
                return 2;
            }
            else
            {
                Ds = null;
                Ds = new DataSet();
                Ds.Merge(dtFullRecord);
                return 1;
            }



        }
        public byte uploadValidExcelMoD(ref DataSet Ds, string Tablename)
        {
            int ErrorCounter = 1;
            DataSet dsNew = new DataSet();
            DataTable dtFullRecord = new DataTable();
            DataTable dtFilter;

            DataTable dtChanged = new DataTable();

            using (CommonData objc = new CommonData())
            {
                objc.UploadTableName = Tablename;
                dtUpload = objc.getSchemaForUpload();
            }

            OpenXMLExcel objexcel = new OpenXMLExcel();

            if (strUploadedFileName.Contains(".xlsx") == true)
                Ds = objexcel.ImportExcelFileV2(strRootFolerPath + strUploadedFileName);
            else
            {
                Message = "Please upload .xlsx file.";
                return 0;
            }

            for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
            {
                Ds.Tables[0].Columns[i].ColumnName = (Ds.Tables[0].Columns[i].ColumnName).Replace(" ", "");
                Ds.Tables[0].Columns[i].ColumnName = Ds.Tables[0].Columns[i].ColumnName.Trim().Replace("#", ".");    //Pankaj Kumar
            }
            ///Due to requirement changed  
            dsNew = Ds;
            ///End of Due to requirement changed  
            if (Ds.Tables[0].Rows.Count == 0)
            {
                Message = "Please enter data in excel sheet";
                return 0;
            }
            if (Ds.Tables[0].Columns.Count < dtUpload.Columns.Count)
            {
                Message = "Invalid excel sheet, columns missing";
                return 0;
            }
            for (int i = 0; i < dtUpload.Columns.Count; i++)
            {
                Ds.Tables[0].Columns[i].ColumnName = Ds.Tables[0].Columns[i].ColumnName.Trim().Replace(" ", "");    //Pankaj Kumar
                if (dtUpload.Columns[i].ColumnName != Ds.Tables[0].Columns[i].ColumnName)
                {
                    Message = "Invalid excel sheet," + Ds.Tables[0].Columns[i].ColumnName + " missing or not in correct sequense, kindly download template again."; /* #CC06 Spelling changed */
                    return 0;
                }
            }
            bool blnValidaData = true;
            if (Ds.Tables[0].Columns.Contains("Error") == false)
            {
                Ds.Tables[0].Columns.Add("Error", System.Type.GetType("System.String"));
            }
            dtFullRecord = Ds.Tables[0].Clone();
            for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
            {
                if (Ds.Tables[0].Columns[i].ColumnName.ToLower() == "dispatchqty")
                {
                    Ds.Tables[0].Columns[i].ColumnName = "Quantity";
                    break;
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////////////
            ///Due to requirement changed           //Pankaj Dhingra
            /*      if (Ds.Tables[0].Rows.Count > 0)
                  {
                      for (int i = 0; i <= Ds.Tables[0].Rows.Count - 1; i++)
                      {

                          if (Ds.Tables[0].Rows[i]["Quantity"].ToString().Contains("-"))
                          {
                              int intindex = Ds.Tables[0].Rows[i]["Quantity"].ToString().IndexOf('-');
                              if (intindex != 0)
                                  Ds.Tables[0].Rows[i]["Quantity"] = "-" + Ds.Tables[0].Rows[i]["Quantity"].ToString().Remove(intindex);
                              else
                                  Ds.Tables[0].Rows[i]["Quantity"] = Ds.Tables[0].Rows[i]["Quantity"].ToString().Trim();

                          }
                      
                      }
                  }
             Ds.Tables[0].AcceptChanges();
                  dtFullRecord = Ds.Tables[0].Clone();
                  dtFullRecord.Columns["Quantity"].DataType = typeof(System.Int32);
                  dtChanged = dtFullRecord.Clone();
             */

            // convert varchar data into integer
            /*      DataRow dNewRow;
                  foreach (DataRow dr1 in Ds.Tables[0].Rows)
                  {
                      dNewRow = dtChanged.NewRow();
                      foreach (DataColumn dCol in Ds.Tables[0].Columns)
                      {
                          if (dCol.ColumnName.ToLower() == "quantity")
                              dNewRow[dCol.ColumnName] = Convert.ToInt32(Convert.ToDouble(dr1[dCol.ColumnName]));
                          else
                              dNewRow[dCol.ColumnName] = dr1[dCol.ColumnName].ToString().Trim();      //Trimming the values
                      }
                      dtChanged.Rows.Add(dNewRow);
                  }
              dsNew = new DataSet();
                  dsNew.Merge(dtChanged);*/
            ////End of Due to requirement changed  
            //////////////////////////////////////////////////////////////////////////////////////////
            for (int count = 0; count < 2; count++)
            {
                Ds = null;
                if (count == 0)
                {
                    dsNew.Tables[0].DefaultView.RowFilter = "Quantity>0";
                    dtFilter = dsNew.Tables[0].DefaultView.ToTable();
                    Ds = new DataSet();
                    Ds.Merge(dtFilter);
                }
                else
                {
                    dsNew.Tables[0].DefaultView.RowFilter = "Quantity<0";
                    dtFilter = dsNew.Tables[0].DefaultView.ToTable();
                    dtFilter.AcceptChanges();
                    Ds = new DataSet();
                    Ds.Merge(dtFilter);
                }
                UploadCheckNegativeStock = EnumData.EnumSAPModuleName.MODDataUpload;
                if (Ds.Tables[0].Rows.Count > 0)
                    blnValidaData = ValidateExcel(ref Ds, true);
                if (blnValidaData == false)
                {
                    ErrorCounter = ErrorCounter + 1;
                }
                //if (blnValidaData == true)

                //{
                foreach (DataRow dr1 in Ds.Tables[0].Rows)
                {
                    dtFullRecord.ImportRow(dr1);
                }
                //}

            }
            Ds = null;
            Ds = new DataSet();
            Ds.Merge(dtFullRecord);

            if (ErrorCounter >= 2)
            {
                ErrorCounter = 1;
                return 2;
            }
            else
            {
                return 1;
            }
        }
        public Int16 IsExcelFile(System.Web.UI.WebControls.FileUpload UploadControl, ref string FileName)
        {
            Int16 MessageforValidation = 0;
            try
            {

                if (UploadControl.HasFile)
                {
                    if ((Path.GetExtension(UploadControl.FileName).ToLower() == ".xlsx") || (Path.GetExtension(UploadControl.FileName).ToLower() == ".xls"))
                    {
                        try
                        {
                            double dblMaxFileSize = Convert.ToDouble(PageBase.ValidExcelLength);
                            int intFileSize = UploadControl.PostedFile.ContentLength;  //file size is obtained in bytes
                            double dblFileSizeinKB = intFileSize / 1024.0; //convert the file size into kilobytes
                            if ((dblFileSizeinKB > dblMaxFileSize))
                            {
                                MessageforValidation = 4;
                                return MessageforValidation;
                            }
                            strUploadedFileName = PageBase.importExportExcelFileName;
                            //UploadControl.SaveAs(RootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
                            if (DSR_Path == "" || DSR_Path == null)
                            {

                                UploadControl.SaveAs(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);
                            }
                            else
                            {
                                UploadControl.SaveAs(DSR_Path + strUploadedFileName);
                            }

                            MessageforValidation = 1;
                            FileName = strUploadedFileName;
                            return MessageforValidation;
                        }
                        catch (Exception objEx)
                        {

                            MessageforValidation = 0;
                            throw new Exception(objEx.Message.ToString());
                            return MessageforValidation;

                        }
                    }
                    else
                    {


                        MessageforValidation = 2;
                        return MessageforValidation;
                    }
                }
                else
                {


                    MessageforValidation = 3;
                    return MessageforValidation;
                }
            }
            catch (HttpException objHttpException)
            {
                //return MessageforValidation;
                throw objHttpException;
            }
            catch (Exception ex)
            {
                //return MessageforValidation;
                throw ex;

            }


        }

        /// <summary>
        /// Its used in FTPUploadFile page and modification done due to path of storage modify // C001
        /// </summary>
        /// <param name="UploadControl"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public Int16 IsExcelSAPFile(System.Web.UI.WebControls.FileUpload UploadControl, ref string FileName)
        {
            Int16 MessageforValidation = 0;
            try
            {

                if (UploadControl.HasFile)
                {
                    //if (Path.GetExtension(UploadControl.FileName).ToLower() == ".xls" || Path.GetExtension(UploadControl.FileName).ToLower() == ".xlsx")
                    if (Path.GetExtension(UploadControl.FileName).ToLower() == ".xlsx")     //Pankaj Dhingra we only accept xlsx file from manual upload
                    {
                        try
                        {
                            double dblMaxFileSize = Convert.ToDouble(PageBase.ValidExcelLength);
                            int intFileSize = UploadControl.PostedFile.ContentLength;  //file size is obtained in bytes
                            double dblFileSizeinKB = intFileSize / 1024.0; //convert the file size into kilobytes
                            if ((dblFileSizeinKB > dblMaxFileSize))
                            {
                                MessageforValidation = 4;
                                return MessageforValidation;
                            }
                            strUploadedFileName = PageBase.importExportExcelFileName;
                            UploadControl.SaveAs(RootFolerPath + strUploadedFileName); //C001  remove PageBase.strGlobalUploadExcelPathRoot 
                            MessageforValidation = 1;
                            FileName = strUploadedFileName;
                            return MessageforValidation;
                        }
                        catch (Exception objEx)
                        {

                            MessageforValidation = 0;
                            throw new Exception(objEx.Message.ToString());
                            return MessageforValidation;

                        }
                    }
                    else
                    {


                        MessageforValidation = 2;
                        return MessageforValidation;
                    }
                }
                else
                {


                    MessageforValidation = 3;
                    return MessageforValidation;
                }
            }
            catch (HttpException objHttpException)
            {
                //return MessageforValidation;
                throw objHttpException;
            }
            catch (Exception ex)
            {
                //return MessageforValidation;
                throw ex;

            }


        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="AsyncFileUpload"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public Int16 IsExcelFile(AjaxControlToolkit.AsyncFileUpload AsyncFileUpload, ref string FileName)
        {
            Int16 MessageforValidation = 0;
            try
            {

                if (AsyncFileUpload.HasFile)
                {
                    if (Path.GetExtension(AsyncFileUpload.FileName).ToLower() == ".xlsx")
                    {
                        try
                        {
                            strUploadedFileName = PageBase.importExportExcelFileName;
                            AsyncFileUpload.SaveAs(RootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
                            MessageforValidation = 1;
                            FileName = strUploadedFileName;
                            return MessageforValidation;
                        }
                        catch (Exception objEx)
                        {

                            MessageforValidation = 0;

                            return MessageforValidation;
                            throw objEx;
                        }
                    }
                    else
                    {


                        MessageforValidation = 2;
                        return MessageforValidation;
                    }
                }
                else
                {


                    MessageforValidation = 3;
                    return MessageforValidation;
                }
            }
            catch (HttpException objHttpException)
            {
                return MessageforValidation;
                throw objHttpException;
            }
            catch (Exception ex)
            {
                return MessageforValidation;
                throw ex;

            }


        }

        #endregion

        #region Private Methods

        private bool ValidateExcel(ref DataSet dsExcel)
        {
            //DataRow[] Drow;
            DataRow dnew;
            DataTable dt;
            dt = dsExcel.Tables[0].Copy();
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new ArgumentException("No data for upload! ");

            }

            //switch (UploadValidationType)
            //{
            //   //case EnumData.eUploadExcelValidationType.eSales:


            //        //if (UploadCheckNegativeStock == EnumData.EnumSAPModuleName.MODDataUpload)
            //        //{  
            //        //    Drow = dt.Select("(isnull(Quantity,'')<>'')");

            //        //}
            //        //else
            //        //{

            //        //    if (dt.Select("(isnull(Quantity,'')='') or Quantity=''").Length == 0)
            //        //    {
            //        //        Drow = dt.Select("(isnull(cast(Quantity as bigint),0)<0)");
            //        //        if (Drow.Length > 0)
            //        //        {
            //        //            throw new ArgumentException("Quantity should not be negative,Please check excel sheet! ");
            //        //            return false;
            //        //        }
            //        //        Drow = null;

            //        //        Drow = dt.Select("(isnull(Quantity,'')<>'') and (isnull(Quantity,0)>0)");
            //        //        dsExcel.Tables[0].Rows.Clear();
            //        //    }
            //        //    else
            //        //    {
            //        //        throw new ArgumentException("Quantity should not be blank,Please check excel sheet! ");
            //        //        return false;
            //        //    }


            //        //}

            //      //  break;
            //    case EnumData.eUploadExcelValidationType.eRetailerUpload:
            //    case EnumData.eUploadExcelValidationType.ePriceUpload:
            //    default:
            dt = dsExcel.Tables[0].Copy();
            //Drow = dt.Select("1=1");
            dsExcel.Tables[0].Rows.Clear();
            //break;

            //}

            //if (UploadCheckNegativeStock == EnumData.EnumSAPModuleName.MODDataUpload)
            //{
            //}
            //else
            //{
            //    if (Drow == null || Drow.Length == 0)
            //    {

            //        throw new ArgumentException("Please enter quantity,Please check excel sheet");
            //        return false;
            //    }
            //}


            foreach (DataRow erow in dt.Rows)
            {
                dnew = dtUpload.NewRow();
                try
                {
                    for (int icol = 0; icol < dsExcel.Tables[0].Columns.Count - 1; icol++)
                    {

                        //if (dtUpload.Columns[icol].ExtendedProperties["ColumnConstraint"] != null)
                        //{


                        switch (dtUpload.Columns[icol].DataType.FullName.ToString())
                        {
                            case "System.Int64":
                                if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                                {
                                    if (ServerValidation.IsBigInt(erow[icol].ToString(), true) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be numeric format and mandatory.");
                                    }

                                }
                                else
                                {
                                    if (ServerValidation.IsBigInt(erow[icol].ToString(), false) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be numeric format ");
                                    }

                                }
                                break;
                            case "System.Int32":
                                if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                                {
                                    if (ServerValidation.IsInteger(erow[icol].ToString(), true) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be numeric format and mandatory.");
                                    }
                                }
                                else
                                {
                                    if (ServerValidation.IsInteger(erow[icol].ToString(), false) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be numeric format");
                                    }
                                }
                                break;
                            case "System.Int16":
                                if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                                {


                                    if (ServerValidation.IsSmallint(erow[icol].ToString(), true) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be numeric format and mandatory.");
                                    }

                                }
                                else
                                {

                                    if (ServerValidation.IsSmallint(erow[icol].ToString(), false) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be numeric format.");
                                    }

                                }
                                break;
                            case "System.Decimal":
                                if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                                {

                                    if (ServerValidation.IsDecimal(erow[icol].ToString(), true) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be decimal format and mandatory");
                                    }
                                    if (erow[icol].ToString().Contains("-") == true)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be negative");
                                    }
                                }
                                else
                                    if (ServerValidation.IsDecimal(erow[icol].ToString(), false) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be decimal format ");
                                    }
                                //#CC12
                                if (erow[icol].ToString().Trim() != "")
                                {
                                    if (erow[icol].ToString().Contains("-") == true)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be negative");
                                    }
                                }

                                break;
                            case "System.DateTime":
                                if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                                {

                                    if (ServerValidation.IsDate(erow[icol].ToString(), true) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be date format and mandatory");
                                    }
                                }
                                else
                                {
                                    if (ServerValidation.IsDate(erow[icol].ToString(), false) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be date format");
                                    }
                                }
                                break;

                        }
                        switch (dtUpload.Columns[icol].ColumnName.ToString().ToLower())
                        {


                            case "email":
                                //Need not any validation on the Email in Excel in Retailer upload
                                if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                                {

                                    if (ServerValidation.IsValidEmail(erow[icol].ToString(), true) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be proper email format and mandatory");
                                    }
                                }
                                else
                                {

                                    if (ServerValidation.IsValidEmail(erow[icol].ToString(), false) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be proper email format.");
                                    }
                                }
                                break;
                            case "pincode":
                                if (ServerValidation.IsPinCode(erow[icol].ToString(), false) != 0)
                                {
                                    throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be proper pin code format and mandatory");
                                }
                                break;
                            case "mobilenumber":
                                if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)      //Pankaj Dhingra
                                {
                                    if (ServerValidation.IsMobileNo(erow[icol].ToString(), true) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be proper mobile number format and mandatory");
                                    }
                                }
                                else
                                {
                                    if (ServerValidation.IsMobileNo(erow[icol].ToString(), false) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be proper mobile number format");
                                    }
                                }
                                break;
                            case "invoicedate":

                                if (Convert.ToDateTime(erow[icol].ToString()) > System.DateTime.Now)
                                {
                                    throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be less than current date and mandatory");
                                }

                                break;
                            case "quantity":
                                if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)      //Pankaj Dhingra
                                {
                                    if (((Convert.ToInt32(erow[icol].ToString()) < 0)))
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be negative");
                                    }
                                }
                                break;
                            case "externaltarget": //#CC12
                                if (dtUpload.Columns[icol].ToString().Trim() != null && dtUpload.Columns[icol].ToString().Trim() != "")
                                {
                                    if (((Convert.ToInt32(erow[icol].ToString()) < 0)))
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be negative");
                                    }
                                }
                                break;
                            case "internaltarget": ////#CC12
                                if (dtUpload.Columns[icol].ToString().Trim() != null && dtUpload.Columns[icol].ToString().Trim() != "")
                                {
                                    if (((Convert.ToInt32(erow[icol].ToString()) < 0)))
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be negative");
                                    }
                                }
                                break;
                        }
                        //}

                        if (dtUpload.Columns[icol].ExtendedProperties["MinLength"] != null)
                        {
                            //#CC12
                            if (erow[icol].ToString().Trim().Length < Convert.ToInt32(dtUpload.Columns[icol].ExtendedProperties["MinLength"]))
                            {
                                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be minimum " + dtUpload.Columns[icol].ExtendedProperties["MinLength"]);
                            }


                        }
                        if (dtUpload.Columns[icol].ExtendedProperties["MaxLength"] != null)
                        {
                            //#CC12
                            if (erow[icol].ToString().Trim().Length > Convert.ToInt32(dtUpload.Columns[icol].ExtendedProperties["MaxLength"]))
                            {
                                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be maximum " + dtUpload.Columns[icol].ExtendedProperties["MaxLength"]);
                            }


                        }
                        if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                        {
                            if (erow[icol].ToString().Trim() == "")//#CC12
                            {
                                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be blank");
                            }
                        }
                        //#CC12
                        if (erow[icol] != null && erow[icol].ToString().Trim() != "")
                        {
                            dnew[icol] = erow[icol];
                        }
                    }
                    dtUpload.Rows.Add(dnew);


                }
                catch (Exception ex)
                {
                    erow["Error"] = ex.Message;
                }

                dsExcel.Tables[0].LoadDataRow(erow.ItemArray, true);
            }

            dsExcel.Tables[0].AcceptChanges();
            if (dsExcel.Tables[0].Select("isnull(Error,'')<>''").Length > 0) { return false; }
            else
            {
                //if (UploadCheckNegativeStock == EnumData.EnumSAPModuleName.MODDataUpload)
                //{

                //    dsExcel.Tables.Clear();
                //    DataTable dtNew = new DataTable();
                //    dtNew = dtUpload.Copy();
                //    dtNew.TableName = "Table"+System.Guid.NewGuid();
                //    dtUpload.AcceptChanges();
                //    dsExcel.Tables.Add(dtNew);
                //    dtUpload.Rows.Clear(); 
                //    return true;
                //}
                dsExcel.Tables.Clear();
                dtUpload.AcceptChanges();
                dtUpload.TableName = "Table";
                dsExcel.Tables.Add(dtUpload);
                return true;

            }
        }
        private bool ValidateExcel(ref DataSet dsExcel, bool isFtpIntegration)
        {
            DataRow dnew;
            DataTable dt;
            dt = dsExcel.Tables[0].Copy();
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new ArgumentException("No data for upload! ");

            }
            dt = dsExcel.Tables[0].Copy();
            dsExcel.Tables[0].Rows.Clear();
            dtUpload.Rows.Clear();


            foreach (DataRow erow in dt.Rows)
            {
                dnew = dtUpload.NewRow();
                try
                {
                    for (int icol = 0; icol < dsExcel.Tables[0].Columns.Count - 1; icol++)
                    {
                        switch (dtUpload.Columns[icol].DataType.FullName.ToString())
                        {
                            case "System.Int64":
                                if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                                {
                                    if (ServerValidation.IsBigInt(erow[icol].ToString(), true) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be numeric format and mandatory.");
                                    }

                                }
                                else
                                {
                                    if (ServerValidation.IsBigInt(erow[icol].ToString(), false) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be numeric format ");
                                    }

                                }
                                break;
                            case "System.Int32":
                                if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                                {
                                    if (ServerValidation.IsInteger(erow[icol].ToString(), true) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be numeric format and mandatory.");
                                    }
                                }
                                else
                                {
                                    if (ServerValidation.IsInteger(erow[icol].ToString(), false) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be numeric format");
                                    }
                                }
                                break;
                            case "System.Int16":
                                if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                                {


                                    if (ServerValidation.IsSmallint(erow[icol].ToString(), true) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be numeric format and mandatory.");
                                    }

                                }
                                else
                                {

                                    if (ServerValidation.IsSmallint(erow[icol].ToString(), false) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be numeric format.");
                                    }

                                }
                                break;
                            case "System.Decimal":
                                if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                                {

                                    if (ServerValidation.IsDecimal(erow[icol].ToString(), true) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be decimal format and mandatory");
                                    }
                                    if (erow[icol].ToString().Contains("-") == true)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be negative");
                                    }
                                }
                                else
                                    if (ServerValidation.IsDecimal(erow[icol].ToString(), false) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be decimal format ");
                                    }
                                if (erow[icol].ToString() != "")
                                {
                                    if (erow[icol].ToString().Contains("-") == true)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be negative");
                                    }
                                }

                                break;
                            case "System.DateTime":
                                if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                                {

                                    if (ServerValidation.IsDate(erow[icol].ToString(), true) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be date format and mandatory");
                                    }
                                }
                                else
                                {
                                    if (ServerValidation.IsDate(erow[icol].ToString(), false) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be date format");
                                    }
                                }
                                break;

                        }
                        switch (dtUpload.Columns[icol].ColumnName.ToString().ToLower())
                        {


                            case "email":
                                //Need not any validation on the Email in Excel in Retailer upload
                                if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                                {

                                    if (ServerValidation.IsValidEmail(erow[icol].ToString(), true) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be proper email format and mandatory");
                                    }
                                }
                                else
                                {

                                    if (ServerValidation.IsValidEmail(erow[icol].ToString(), false) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be proper email format.");
                                    }
                                }
                                break;
                            case "pincode":
                                if (ServerValidation.IsPinCode(erow[icol].ToString(), true) != 0)
                                {
                                    throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be proper pin code format and mandatory");
                                }
                                break;
                            case "mobilenumber":
                                if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)      //Pankaj Dhingra
                                {
                                    if (ServerValidation.IsMobileNo(erow[icol].ToString(), true) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be proper mobile number format and mandatory");
                                    }
                                }
                                else
                                {
                                    if (ServerValidation.IsMobileNo(erow[icol].ToString(), false) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be proper mobile number format");
                                    }
                                }
                                break;
                            case "invoicedate":
                                if (isFtpIntegration != true)
                                {
                                    if (Convert.ToDateTime(erow[icol].ToString()) > System.DateTime.Now)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be less than current date and mandatory");
                                    }
                                }

                                break;
                            case "quantity":
                                if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)      //Pankaj Dhingra
                                {
                                    if (((Convert.ToInt32(erow[icol].ToString()) < 0)) && (isFtpIntegration == true))
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be negative");
                                    }
                                }
                                break;
                        }
                        //}

                        if (dtUpload.Columns[icol].ExtendedProperties["MinLength"] != null)
                        {
                            if (erow[icol].ToString().Length < Convert.ToInt32(dtUpload.Columns[icol].ExtendedProperties["MinLength"]))
                            {
                                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be minimum " + dtUpload.Columns[icol].ExtendedProperties["MinLength"]);
                            }


                        }
                        if (dtUpload.Columns[icol].ExtendedProperties["MaxLength"] != null)
                        {
                            if (erow[icol].ToString().Length > Convert.ToInt32(dtUpload.Columns[icol].ExtendedProperties["MaxLength"]))
                            {
                                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be maximum " + dtUpload.Columns[icol].ExtendedProperties["MaxLength"]);
                            }


                        }
                        if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                        {
                            if (erow[icol].ToString() == "")
                            {
                                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be blank");
                            }
                        }

                        if (erow[icol] != null && erow[icol].ToString() != "")
                        {
                            dnew[icol] = erow[icol];
                        }
                    }
                    dtUpload.Rows.Add(dnew);


                }
                catch (Exception ex)
                {
                    erow["Error"] = ex.Message;
                }

                dsExcel.Tables[0].LoadDataRow(erow.ItemArray, LoadOption.OverwriteChanges);
            }

            // dsExcel.Tables[0].AcceptChanges();
            if (dsExcel.Tables[0].Select("isnull(Error,'')<>''").Length > 0) { return false; }
            else
            {
                if (isFtpIntegration == true)
                {

                    dsExcel.Tables.Clear();
                    DataTable dtNew = new DataTable();
                    dtNew = dtUpload.Copy();
                    dtNew.TableName = "Table" + System.Guid.NewGuid();
                    dtUpload.AcceptChanges();
                    dsExcel.Tables.Add(dtNew);
                    dtUpload.Rows.Clear();
                    return true;
                }
                dsExcel.Tables.Clear();
                dtUpload.AcceptChanges();
                dtUpload.TableName = "Table";
                dsExcel.Tables.Add(dtUpload);
                return true;

            }
        }
        private bool ValidateExcelForSap(ref DataSet dsExcel, bool isFtpIntegration)
        {
            //DataRow[] Drow;
            DataRow dnew;
            DataTable dt;
            dt = dsExcel.Tables[0].Copy();
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new ArgumentException("No data for upload! ");

            }

            dt = dsExcel.Tables[0].Copy();
            dsExcel.Tables[0].Rows.Clear();
            foreach (DataRow erow in dt.Rows)
            {
                dnew = dtUpload.NewRow();
                try
                {
                    for (int icol = 0; icol < dsExcel.Tables[0].Columns.Count - 1; icol++)
                    {
                        erow[icol] = erow[icol].ToString().Trim();
                        switch (dtUpload.Columns[icol].DataType.FullName.ToString())
                        {
                            case "System.Int64":
                                if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                                {
                                    if (ServerValidation.IsBigInt(erow[icol].ToString(), true) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be numeric format and mandatory.");
                                    }

                                }
                                else
                                {
                                    if (ServerValidation.IsBigInt(erow[icol].ToString(), false) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be numeric format ");
                                    }

                                }
                                break;
                            case "System.Int32":
                                if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                                {
                                    if (ServerValidation.IsInteger(erow[icol].ToString(), true) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be numeric format and mandatory.");
                                    }
                                }
                                else
                                {
                                    if (ServerValidation.IsInteger(erow[icol].ToString(), false) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be numeric format");
                                    }
                                }
                                break;
                            case "System.Int16":
                                if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                                {


                                    if (ServerValidation.IsSmallint(erow[icol].ToString(), true) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be numeric format and mandatory.");
                                    }

                                }
                                else
                                {

                                    if (ServerValidation.IsSmallint(erow[icol].ToString(), false) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be numeric format.");
                                    }

                                }
                                break;
                            case "System.Decimal":
                                if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                                {

                                    if (ServerValidation.IsDecimal(erow[icol].ToString(), true) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be decimal format and mandatory");
                                    }
                                    if (erow[icol].ToString().Contains("-") == true)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be negative");
                                    }
                                }
                                else
                                    if (ServerValidation.IsDecimal(erow[icol].ToString(), false) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be decimal format ");
                                    }
                                if (erow[icol].ToString() != "")
                                {
                                    if (erow[icol].ToString().Contains("-") == true)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be negative");
                                    }
                                }

                                break;
                            case "System.DateTime":
                                if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                                {

                                    if (ServerValidation.IsDate(erow[icol].ToString(), true) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be date format and mandatory");
                                    }
                                }
                                else
                                {
                                    if (ServerValidation.IsDate(erow[icol].ToString(), false) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be date format");
                                    }
                                }
                                break;

                        }
                        switch (dtUpload.Columns[icol].ColumnName.ToString().ToLower())
                        {


                            case "email":
                                //Need not any validation on the Email in Excel in Retailer upload
                                if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                                {

                                    if (ServerValidation.IsValidEmail(erow[icol].ToString(), true) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be proper email format and mandatory");
                                    }
                                }
                                else
                                {

                                    if (ServerValidation.IsValidEmail(erow[icol].ToString(), false) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be proper email format.");
                                    }
                                }
                                break;
                            case "pincode":
                                if (ServerValidation.IsPinCode(erow[icol].ToString(), true) != 0)
                                {
                                    throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be proper pin code format and mandatory");
                                }
                                break;
                            case "mobilenumber":
                                if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)      //Pankaj Dhingra
                                {
                                    if (ServerValidation.IsMobileNo(erow[icol].ToString(), true) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be proper mobile number format and mandatory");
                                    }
                                }
                                else
                                {
                                    if (ServerValidation.IsMobileNo(erow[icol].ToString(), false) != 0)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be proper mobile number format");
                                    }
                                }
                                break;
                            case "invoicedate":
                                if (isFtpIntegration != true)
                                {
                                    if (Convert.ToDateTime(erow[icol].ToString()) > System.DateTime.Now)
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be less than current date and mandatory");
                                    }
                                }

                                break;
                            case "quantity":
                                if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)      //Pankaj Dhingra
                                {
                                    if (((Convert.ToInt32(erow[icol].ToString()) < 0)) && (UploadCheckNegativeStock != EnumData.EnumSAPModuleName.MODDataUpload))
                                    {
                                        throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be negative");
                                    }
                                }
                                break;
                        }
                        //}

                        if (dtUpload.Columns[icol].ExtendedProperties["MinLength"] != null)
                        {
                            if (erow[icol].ToString().Length < Convert.ToInt32(dtUpload.Columns[icol].ExtendedProperties["MinLength"]))
                            {
                                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should be minimum " + dtUpload.Columns[icol].ExtendedProperties["MinLength"]);
                            }


                        }
                        if (dtUpload.Columns[icol].ExtendedProperties["MaxLength"] != null)
                        {
                            if (erow[icol].ToString().Length > Convert.ToInt32(dtUpload.Columns[icol].ExtendedProperties["MaxLength"]))
                            {
                                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be maximum " + dtUpload.Columns[icol].ExtendedProperties["MaxLength"]);
                            }


                        }
                        if (Convert.ToBoolean(dtUpload.Columns[icol].ExtendedProperties["Validate"]) != false)
                        {
                            if (erow[icol].ToString() == "")
                            {
                                throw new ArgumentException(dsExcel.Tables[0].Columns[icol].ColumnName + " should not be blank");
                            }
                        }

                        if (erow[icol] != null && erow[icol].ToString() != "")
                        {
                            dnew[icol] = erow[icol];
                        }
                    }
                    dtUpload.Rows.Add(dnew);


                }
                catch (Exception ex)
                {
                    erow["Error"] = ex.Message;
                }

                dsExcel.Tables[0].LoadDataRow(erow.ItemArray, true);
            }

            dsExcel.Tables[0].AcceptChanges();
            if (dsExcel.Tables[0].Select("isnull(Error,'')<>''").Length > 0) { return false; }
            else
            {
                if (isFtpIntegration == true)
                {

                    dsExcel.Tables.Clear();
                    DataTable dtNew = new DataTable();
                    dtNew = dtUpload.Copy();
                    dtNew.TableName = "Table" + System.Guid.NewGuid();
                    dtUpload.AcceptChanges();
                    dsExcel.Tables.Add(dtNew);
                    dtUpload.Rows.Clear();
                    return true;
                }
                dsExcel.Tables.Clear();
                dtUpload.AcceptChanges();
                dtUpload.TableName = "Table";
                dsExcel.Tables.Add(dtUpload);
                return true;

            }
        }

        #endregion


        #region Public Methods for Target Upload

        public byte uploadValidTargetExcel(ref DataSet Ds, Int16 TargetCategory, Int32 BrandID)
        {
            // OpenXMLExcel objexcel = new OpenXMLExcel();
            bool blnValidaData = true;
            //Ds = objexcel.ImportExcelFile(strRootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);

            //if (Ds.Tables[0].Columns.Count < 2)// Column number from excel should be same as SKU Count or Brand Count depends on target Category.
            //{
            //    Message = "Invalid excel sheet, columns missing";
            //    return 0;
            //}

            //switch (UploadValidationType)
            //{
            //    case EnumData.eUploadExcelValidationType.ePurchase:
            //        break;
            //    case EnumData.eUploadExcelValidationType.eSales:
            //        break;
            //    case EnumData.eUploadExcelValidationType.eTarget:
            //        using (CommonMaster objc = new CommonMaster())
            //        {
            //            objc.BrandID = 0;
            //            dtUpload = objc.getSchemaForTargetUpload(TargetCategory);
            //            dtUpload.PrimaryKey = new DataColumn[] { dtUpload.Columns[0] };


            //            for (int i = 1; i < Ds.Tables[0].Columns.Count; i++)
            //            {
            //                if (!dtUpload.Rows.Contains(Ds.Tables[0].Columns[i].ColumnName))
            //                {
            //                    Message = "Invalid excel sheet," + Ds.Tables[0].Columns[i].ColumnName + " missing or not in correct sequense, kinldy download template again.";
            //                    return 0;
            //                }
            //            }
            //        }
            //        break;
            //    case EnumData.eUploadExcelValidationType.eAdjustment:
            //        using (CommonMaster objc = new CommonMaster())
            //        {
            //            objc.BrandID = BrandID;
            //            dtUpload = objc.getSchemaForTargetUpload(TargetCategory);
            //            dtUpload.PrimaryKey = new DataColumn[] { dtUpload.Columns[0] };


            //            for (int i = 1; i < Ds.Tables[0].Columns.Count; i++)
            //            {
            //                if (!dtUpload.Rows.Contains(Ds.Tables[0].Columns[i].ColumnName))
            //                {
            //                    Message = "Invalid excel sheet," + Ds.Tables[0].Columns[i].ColumnName + " missing or not in correct sequense, kinldy download template again.";
            //                    return 0;
            //                }
            //            }
            //        }
            //        break;
            //    case EnumData.eUploadExcelValidationType.eScheme:
            //        using (SchemeData objc = new SchemeData())
            //        {
            //            objc.SchemeType = SchemeType;
            //            dtUpload = objc.GetSchemeSchema();
            //            dtUpload.PrimaryKey = new DataColumn[] { dtUpload.Columns[0] };


            //            for (int i = 1; i < Ds.Tables[0].Columns.Count; i++)
            //            {
            //                if (!dtUpload.Columns.Contains(Ds.Tables[0].Columns[i].ColumnName))
            //                {
            //                    Message = "Invalid excel sheet," + Ds.Tables[0].Columns[i].ColumnName + " missing or not in correct sequense, kinldy download template again.";
            //                    return 0;
            //                }
            //            }

            //            if (Ds.Tables[0].Columns.Contains("Error") == false)
            //            {
            //                Ds.Tables[0].Columns.Add("Error", System.Type.GetType("System.String"));
            //            }

            //            int iColStartIndex;
            //            if (SchemeType == EnumData.eSchemeTemplateType.eSummary)
            //                iColStartIndex = 0;
            //            else
            //                iColStartIndex = 1;

            //            for (int iRow = 0; iRow < Ds.Tables[0].Rows.Count; iRow++)
            //            {
            //                for (int iCol = iColStartIndex; iCol < Ds.Tables[0].Columns.Count - 1; iCol++)
            //                {
            //                    if (ServerValidation.IsInteger(Ds.Tables[0].Rows[iRow][iCol], true) != 0)
            //                    {
            //                        Ds.Tables[0].Rows[iRow].BeginEdit();
            //                        Ds.Tables[0].Rows[iRow]["Error"] = (Ds.Tables[0].Rows[iRow]["Error"] == DBNull.Value ? "" : Environment.NewLine + Ds.Tables[0].Rows[iRow]["Error"]) + Ds.Tables[0].Columns[iCol].ColumnName + "Value should be numeric.";
            //                        Ds.Tables[0].Rows[iRow].EndEdit();
            //                    }
            //                }
            //            }

            //        }
            //        break;
            //    default:
            //        break;
            //}


            //Ds.AcceptChanges();


            //if (Ds.Tables[0].Columns.Contains("Error") == false)
            //{
            //    Ds.Tables[0].Columns.Add("Error", System.Type.GetType("System.String"));
            //}


            //if (Ds.Tables[0].Select("Error is not null").Length > 0)
            //{
            //    blnValidaData = false;
            //}

            if (blnValidaData == false)
            {
                return 2;
            }
            else
            {
                return 1;
            }


        }

        #endregion

        //Pankaj Dhingra for POC
        public byte uploadValidExcelMoDForScheme(ref DataSet Ds, string Tablename)
        {
            int ErrorCounter = 1;
            DataSet dsNew = new DataSet();
            DataTable dtFullRecord = new DataTable();
            DataTable dtFilter;

            DataTable dtChanged = new DataTable();

            using (CommonData objc = new CommonData())
            {
                objc.UploadTableName = Tablename;
                dtUpload = objc.getSchemaForUpload();
            }

            OpenXMLExcel objexcel = new OpenXMLExcel();

            if (strUploadedFileName.Contains(".xlsx") == true)
                Ds = objexcel.ImportExcelFileV2(strRootFolerPath + strUploadedFileName);
            else
            {
                Message = "Please upload .xlsx file.";
                return 0;
            }

            for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
            {
                Ds.Tables[0].Columns[i].ColumnName = (Ds.Tables[0].Columns[i].ColumnName).Replace(" ", "");
                Ds.Tables[0].Columns[i].ColumnName = Ds.Tables[0].Columns[i].ColumnName.Trim().Replace("#", ".");    //Pankaj Kumar
            }
            ///Due to requirement changed  
            dsNew = Ds;
            ///End of Due to requirement changed  
            if (Ds.Tables[0].Rows.Count == 0)
            {
                Message = "Please enter data in excel sheet";
                return 0;
            }
            if (Ds.Tables[0].Columns.Count < dtUpload.Columns.Count)
            {
                Message = "Invalid excel sheet, columns missing";
                return 0;
            }
            for (int i = 0; i < dtUpload.Columns.Count; i++)
            {
                Ds.Tables[0].Columns[i].ColumnName = Ds.Tables[0].Columns[i].ColumnName.Trim().Replace(" ", "");    //Pankaj Kumar
                if (dtUpload.Columns[i].ColumnName != Ds.Tables[0].Columns[i].ColumnName)
                {
                    Message = "Invalid excel sheet," + Ds.Tables[0].Columns[i].ColumnName + " missing or not in correct sequense, kindly download template again."; /* */
                    return 0;
                }
            }
            bool blnValidaData = true;
            if (Ds.Tables[0].Columns.Contains("Error") == false)
            {
                Ds.Tables[0].Columns.Add("Error", System.Type.GetType("System.String"));
            }
            dtFullRecord = Ds.Tables[0].Clone();
            for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
            {
                if (Ds.Tables[0].Columns[i].ColumnName.ToLower() == "dispatchqty")
                {
                    Ds.Tables[0].Columns[i].ColumnName = "Quantity";
                    break;
                }
            }

            for (int count = 0; count < 2; count++)
            {
                Ds = null;
                if (count == 0)
                {
                    dsNew.Tables[0].DefaultView.RowFilter = "Quantity>0";
                    dtFilter = dsNew.Tables[0].DefaultView.ToTable();
                    Ds = new DataSet();
                    Ds.Merge(dtFilter);
                }
                else
                {
                    dsNew.Tables[0].DefaultView.RowFilter = "Quantity<0";
                    dtFilter = dsNew.Tables[0].DefaultView.ToTable();
                    foreach (DataRow dr in dtFilter.Rows)
                    {
                        dr["NET(WITHOUTED)"] = Convert.ToDecimal(dr["NET(WITHOUTED)"].ToString().Replace("-", ""));
                        dr["TotalValue"] = Convert.ToDecimal(dr["TotalValue"].ToString().Replace("-", ""));
                    }
                    dtFilter.AcceptChanges();
                    Ds = new DataSet();
                    Ds.Merge(dtFilter);
                }
                UploadCheckNegativeStock = EnumData.EnumSAPModuleName.MODDataUpload;
                if (Ds.Tables[0].Rows.Count > 0)
                    blnValidaData = ValidateExcelForSap(ref Ds, true);
                if (blnValidaData == false)
                {
                    ErrorCounter = ErrorCounter + 1;
                }
                //if (blnValidaData == true)

                //{
                foreach (DataRow dr1 in Ds.Tables[0].Rows)
                {
                    dtFullRecord.ImportRow(dr1);
                }
                //}

            }
            Ds = null;
            Ds = new DataSet();
            Ds.Merge(dtFullRecord);

            if (ErrorCounter >= 2)
            {
                ErrorCounter = 1;
                return 2;
            }
            else
            {
                return 1;
            }
        }
        /*#CC08 Added Started*/
        public byte uploadValidExceluploadDOA(ref DataSet Ds, string Tablename)
        {
            int rowCownt = 0;
            bool blnValidaData = true;
            using (CommonData objc = new CommonData())
            {

                objc.UploadTableName = Tablename;
                objc.TemplateType = TemplateType;
                objc.UploadValidationType = UploadValidationType;

                dtUpload = objc.getSchemaForUpload();


            }
            OpenXMLExcel objexcel = new OpenXMLExcel();
            rowCownt = objexcel.CountExcelRows(strRootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
            if (rowCownt >= Convert.ToInt64(PageBase.ValidExcelRows))
            {
                Message = "Maximum" + PageBase.ValidExcelRows + "rows to upload excel file";
                return 0;
            }
            Ds = objexcel.ImportExcelFileV2(strRootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);


            if (Ds.Tables[0].Rows.Count == 0)
            {
                Message = "Please enter data in excel sheet";
                return 0;
            }
            if (Ds.Tables[0].Columns.Count != dtUpload.Columns.Count)
            {
                Message = Resources.Messages.InvalidExcelColumnsDoesntMatch; /* #CC13 Added */ /*"Invalid excel sheet, columns doesn't match to required feilds"; #CC13 Commented */
                return 0;
            }
            for (int i = 0; i < dtUpload.Columns.Count; i++)
            {
                Ds.Tables[0].Columns[i].ColumnName = Ds.Tables[0].Columns[i].ColumnName.Trim().Replace(" ", "");
                if (dtUpload.Columns[i].ColumnName.ToLower() != Ds.Tables[0].Columns[i].ColumnName.ToLower())
                {
                    Message = "Invalid excel sheet," + Ds.Tables[0].Columns[i].ColumnName + " missing or not in correct sequense, kindly download template again."; /* #CC06 Spelling changed */
                    return 0;
                }
            }

            if (Ds.Tables[0].Columns.Contains("Error") == false)
            {
                Ds.Tables[0].Columns.Add("Error", System.Type.GetType("System.String"));
            }

            //if (Ds.Tables[0].Select("Error is not null").Length > 0)
            //{
            //    blnValidaData = false;
            //}
            //if (blnValidaData == false)
            //{
            //    return 2;
            //}
            //else
            //{
            //    return 1;
            //}
            blnValidaData = ValidateExcel(ref Ds);
            if (blnValidaData == false)
            {
                return 2;
            }
            else
            {
                return 1;
            }

        }
        /*#CC08 Added End*/

        /*#CC14 Added Started*/
        public Int16 IsExcelFilePSISAPIntegration(System.Web.UI.WebControls.FileUpload UploadControl, ref string FileName, string SAPUploadInterfaceName)
        {
            Int16 MessageforValidation = 0;
            try
            {

                if (UploadControl.HasFile)
                {
                    if ((Path.GetExtension(UploadControl.FileName).ToLower() == ".xlsx") || (Path.GetExtension(UploadControl.FileName).ToLower() == ".xls"))
                    {
                        try
                        {
                            double dblMaxFileSize = Convert.ToDouble(PageBase.ValidExcelLength);
                            int intFileSize = UploadControl.PostedFile.ContentLength;  //file size is obtained in bytes
                            double dblFileSizeinKB = intFileSize / 1024.0; //convert the file size into kilobytes
                            if ((dblFileSizeinKB > dblMaxFileSize))
                            {
                                MessageforValidation = 4;
                                return MessageforValidation;
                            }
                            strUploadedFileName = PageBase.importExportExcelFileName;
                            //UploadControl.SaveAs(RootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
                            if (DSR_Path == "" || DSR_Path == null)
                            {
                                if (SAPUploadInterfaceName == "PSIInfoUpload")
                                {
                                    UploadControl.SaveAs(PageBase.strExcelBulkUploadPSIInfoPath + strUploadedFileName);
                                }
                                else if (SAPUploadInterfaceName == "InvoiceInfoUpload")
                                {
                                    UploadControl.SaveAs(PageBase.strExcelBulkUploadPSIInvoiceInfoPath + strUploadedFileName);
                                }

                            }
                            else
                            {
                                UploadControl.SaveAs(DSR_Path + strUploadedFileName);
                            }

                            MessageforValidation = 1;
                            FileName = strUploadedFileName;
                            return MessageforValidation;
                        }
                        catch (Exception objEx)
                        {

                            MessageforValidation = 0;
                            throw new Exception(objEx.Message.ToString());
                            return MessageforValidation;

                        }
                    }
                    else
                    {


                        MessageforValidation = 2;
                        return MessageforValidation;
                    }
                }
                else
                {


                    MessageforValidation = 3;
                    return MessageforValidation;
                }
            }
            catch (HttpException objHttpException)
            {
                //return MessageforValidation;
                throw objHttpException;
            }
            catch (Exception ex)
            {
                //return MessageforValidation;
                throw ex;

            }


        }

        /*#CC14 Added End*/

        /*#CC15 Added */
        public byte uploadValidExcelRetailerWithCompanyId(ref DataSet Ds, string Tablename)
        {
            int rowCownt = 0;

            using (CommonData objc = new CommonData())
            {

                objc.UploadTableName = Tablename;
                objc.TemplateType = TemplateType;
                objc.UploadValidationType = UploadValidationType;
                objc.CompanyId = PageBase.ClientId;/*#CC15 Added*/
                dtUpload = objc.getSchemaForUploadWithCompanyId();



            }

            OpenXMLExcel objexcel = new OpenXMLExcel();
            rowCownt = objexcel.CountExcelRows(strRootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
            if (rowCownt >= Convert.ToInt64(PageBase.ValidExcelRows))
            {
                Message = "Maximum" + PageBase.ValidExcelRows + "rows to upload excel file";
                return 0;
            }
            Ds = objexcel.ImportExcelFileV2(strRootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
            if (Ds.Tables.Count == 0)
            {
                return 0;
            }


            HttpContext.Current.Session["RetailerUploadData"] = Ds.Copy();

            //IsSku = Ds.Tables[0].Columns[0].ColumnName;
            if (HttpContext.Current.Session["RETAILERLOGIN"] != null)
            {
                if (Convert.ToInt32(HttpContext.Current.Session["RETAILERLOGIN"]) == 1)
                {
                    if (Ds.Tables[0].Columns.Contains("RetailerUserName"))
                    {
                        Ds.Tables[0].Columns.Remove("RetailerUserName");
                    }
                }
            }
            if (HttpContext.Current.Session["RETAILERHIERLVLID"] != null)
            {
                if (Convert.ToInt32(HttpContext.Current.Session["RETAILERHIERLVLID"]) > 0)
                {
                    if (Ds.Tables[0].Columns.Contains("MappedOrgnhierarchy"))
                    {
                        Ds.Tables[0].Columns.Remove("MappedOrgnhierarchy");
                    }
                }
            }
            if (HttpContext.Current.Session["RETAILERCODEAUTO"] != null)
            {
                if (Convert.ToInt32(HttpContext.Current.Session["RETAILERCODEAUTO"]) == 0)
                {
                    if (Ds.Tables[0].Columns.Contains("RetailerCode"))
                    {
                        Ds.Tables[0].Columns.Remove("RetailerCode");
                    }
                }
            }

            //#CC11
            if (HttpContext.Current.Session["UPDRTLR"] != null)
            {
                if (Convert.ToInt32(HttpContext.Current.Session["UPDRTLR"]) == 1)
                {
                    if (Ds.Tables[0].Columns.Contains("RetailerCode"))
                    {
                        Ds.Tables[0].Columns.Remove("RetailerCode");
                    }

                    if (Ds.Tables[0].Columns.Contains("Active"))
                    {
                        Ds.Tables[0].Columns.Remove("Active");
                    }

                    if (dtUpload.Columns.Contains("SalesChannelCode"))
                    {
                        dtUpload.Columns.Remove("SalesChannelCode");
                    }

                    if (dtUpload.Columns.Contains("SalesmanName"))
                    { dtUpload.Columns.Remove("SalesmanName"); }


                }
            }

            if (HttpContext.Current.Session["RetailerOpStockDate"] != null)
            {
                if (HttpContext.Current.Session["RetailerOpStockDate"].ToString() == "0")
                {
                    if (Ds.Tables[0].Columns.Contains("StockOpeningDate"))
                    {
                        Ds.Tables[0].Columns.Remove("StockOpeningDate");
                    }
                }
            }
            /* #CC01 Add Start */
            if (HttpContext.Current.Session["RetailerBankDetail"] != null)
            {
                if (HttpContext.Current.Session["RetailerBankDetail"].ToString() == "1")
                {
                    if (Ds.Tables[0].Columns.Contains("BankName"))
                    {
                        Ds.Tables[0].Columns.Remove("BankName");
                    }
                    if (Ds.Tables[0].Columns.Contains("AccountHolderName"))
                    {
                        Ds.Tables[0].Columns.Remove("AccountHolderName");
                    }
                    if (Ds.Tables[0].Columns.Contains("BankAccountNumber"))
                    {
                        Ds.Tables[0].Columns.Remove("BankAccountNumber");
                    }
                    if (Ds.Tables[0].Columns.Contains("BranchLocation"))
                    {
                        Ds.Tables[0].Columns.Remove("BranchLocation");
                    }
                    if (Ds.Tables[0].Columns.Contains("IFSCCode"))
                    {
                        Ds.Tables[0].Columns.Remove("IFSCCode");
                    }
                    /* #CC02 Add Start */
                    if (Ds.Tables[0].Columns.Contains("PANNo"))
                    {
                        Ds.Tables[0].Columns.Remove("PANNo");
                    }
                    /* #CC02 Add End */
                }
            }
            /* #CC01 Add End */

            /* #CC03 Add Start */


            /*#CC09 start*/
            if (HttpContext.Current.Session["WhatsAppMobileNumber"] != null)
            {
                if (HttpContext.Current.Session["WhatsAppMobileNumber"].ToString() == "1")
                {
                    if (Ds.Tables[0].Columns.Contains("WhatsAppNumber"))
                    {
                        Ds.Tables[0].Columns.Remove("WhatsAppNumber");
                    }
                }
            }/*#CC09 end*/
            if (HttpContext.Current.Session["PotentialVolDisplay"] != null)
            {
                if (HttpContext.Current.Session["PotentialVolDisplay"].ToString() == "1")
                {
                    if (Ds.Tables[0].Columns.Contains("CounterPotentialValue"))
                    {
                        Ds.Tables[0].Columns.Remove("CounterPotentialValue");
                    }
                }
            }
            /* #CC03 Add End */

            /* #CC04 Add Start */
            if (HttpContext.Current.Session["TehsillDisplayMode"] != null)
            {
                if (PageBase.TehsillDisplayMode == "1")
                {
                    if (Ds.Tables[0].Columns.Contains("TehsilName"))
                    {
                        Ds.Tables[0].Columns.Remove("TehsilName");
                    }
                }
            }
            /* #CC04 Add End */

            /* #CC05 Add Start */
            if (PageBase.ChangeTinLabel == 1)
            {
                /*
                 #CC07 Comment Start
                 if (Ds.Tables[0].Columns.Contains("VATNumber"))
                {
                    Ds.Tables[0].Columns["VATNumber"].ColumnName = "TinNumber";
                }
                #CC07 Comment End */
                /* #CC07 Add Start */
                if (Ds.Tables[0].Columns.Contains("GSTNumber"))
                {
                    Ds.Tables[0].Columns["GSTNumber"].ColumnName = "TinNumber";
                }
                /* #CC07 Add End */
            }
            /* #CC05 Add End */

            if (Ds.Tables[0].Rows.Count == 0)
            {
                Message = "Please enter data in excel sheet";
                return 0;
            }
            if (Ds.Tables[0].Columns.Count != dtUpload.Columns.Count)
            {
                Message = Resources.Messages.InvalidExcelColumnsDoesntMatch; /* #CC13 Added */ /*"Invalid excel sheet, columns doesn't match to required feilds"; #CC13 Commented */
                return 0;
            }
            for (int i = 0; i < dtUpload.Columns.Count; i++)
            {
                Ds.Tables[0].Columns[i].ColumnName = Ds.Tables[0].Columns[i].ColumnName.Trim().Replace(" ", "");
                if (dtUpload.Columns[i].ColumnName.ToLower() != Ds.Tables[0].Columns[i].ColumnName.ToLower())
                {
                    Message = "Invalid excel sheet," + Ds.Tables[0].Columns[i].ColumnName + " missing or not in correct sequense, kindly download template again."; /* #CC06 Spelling changed */
                    return 0;
                }
            }
            bool blnValidaData = true;
            if (Ds.Tables[0].Columns.Contains("Error") == false)
            {
                Ds.Tables[0].Columns.Add("Error", System.Type.GetType("System.String"));
            }

            for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
            {
                if (Ds.Tables[0].Columns[i].ColumnName.ToLower() == "dispatchqty")
                {
                    Ds.Tables[0].Columns[i].ColumnName = "Quantity";
                    break;
                }
            }
            if (UploadCheckNegativeStock == EnumData.EnumSAPModuleName.BTMDataUpload)
                UploadCheckNegativeStock = EnumData.EnumSAPModuleName.BTMDataUpload;
            else if (UploadCheckNegativeStock == EnumData.EnumSAPModuleName.GRNDataUpload)
                UploadCheckNegativeStock = EnumData.EnumSAPModuleName.GRNDataUpload;
            blnValidaData = ValidateExcel(ref Ds);
            if (blnValidaData == false)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }
        /*#CC15 Added End*/
    }
}
