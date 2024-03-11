﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Xml;
/*
 * 22 Jan 2015, Karam Chand Sharma, #CC01, Add some properties name with stockBinType and pass into function GetSerialNosByCodeForReturn()
 * 31 May 2016, Karam Chand Sharma, #CC02, Add some properties and change in sku intertion and updation, show sku and export in excel function.
 * 07-Jul-2016, Sumit Maurya, #CC03, New function and properties created for pricelist.
 * 26 July 2016, Karam Chand Sharma, #CC04, Pass stock type in GetSKUStockInHandBySalesChannelOrRetailer() function
 * 1 Aug 2016, Karam Chand Sharma, #CC05, Pass stock type in GetSerialNosByCode() function
 * 05-March-2019,Vijay Kumar Prajapati,#CC06,Added new paramater Keyword (done for brother)
 * 31-march-2020,Vijay Kumar Prajapati,#CC07,Added Companyid on this page.
 */

namespace DataAccess
{
    public class ProductData : IDisposable
    {

        DataTable d1;
        DataTable dtResult;
        DataSet DsResult;
        SqlParameter[] SqlParam;
        MySqlParameter[] MySqlParam;
        Int32 IntResultCount = 0;

        private string strXMLList;
        private bool? blnStatus;
        private Int32 intPriceListId;
        private Int64 intPriceMasterId;
        private DateTime? dtEffectiveDate;
        private DateTime? dtDateRange;
        private EnumData.eSearchConditions eSearchType;
        private Int32 intModelType;
        private Int32 intModelMode;
        public Int32 PageIndex { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public Int32 TotalCount { get; set; }
        public Int16 Value
        {
            get;
            set;
        }
        /*#CC01 START ADDED*/
        private int _intStockBinType;
        public int StockBinType
        {
            get { return _intStockBinType; }
            set { _intStockBinType = value; }
        }
        /*#CC01 START END*/
        public string error;
        public Int32 CompanyId {get;set;}
        public Int32 ClientId { get; set; }
        /* #CC03 Add Start */

        
        /* #CC03 Add End */

        # region pricelist

        private int pricelistid, intUserId; string pricelistname; int priceliststatus; int pricelistselectionmode;

        public int PriceListId
        {
            get { return pricelistid; }
            set { pricelistid = value; }
        }
        public int UserId
        {
            get { return intUserId; }
            set { intUserId = value; }
        }

        public Int64 PriceMasterID
        {
            get { return intPriceMasterId; }
            set { intPriceMasterId = value; }
        }

        public string PriceListName
        {
            get { return pricelistname; }
            set { pricelistname = value; }
        }


        public int PriceListStatus
        {
            get { return priceliststatus; }
            set { priceliststatus = value; }

        }

        public int PriceListSelectionMode
        {
            get { return pricelistselectionmode; }
            set { pricelistselectionmode = value; }

        }
        public EnumData.eSearchConditions SearchType
        {
            get { return eSearchType; }
            set { eSearchType = value; }
        }
        public Int32 ModelType
        {
            get { return intModelType; }
            set { intModelType = value; }
        }
        public Int32 ModelMode
        {
            get { return intModelMode; }
            set { intModelMode = value; }
        }

        public int ChequeNumber
        {
            get;
            set;
        }


        public string BankName
        {
            get;
            set;

        }
        public string AccountNo
        {
            get;
            set;

        }

        public int Checkfrom
        {
            get;
            set;

        }
        public int CheckTo
        {
            get;
            set;
        }

        public int ChequeStatus
        {
            get;
            set;
        }

        public string DateFrom
        {
            get;
            set;
        }

        public string DateTo
        {
            get;
            set;
        }


        public int AdvancedChequeID
        {
            get;
            set;
        }

        public decimal Amount
        {
            get;
            set;
        }


        public string DepositionDate
        {
            get;
            set;
        }


        public Int16 SelectionMode
        {
            get;
            set;
        }


        public string VendorName
        {
            get;
            set;
        }


        public string VendorCode
        {
            get;
            set;
        }

        public int VendorID
        {
            get;
            set;
        }

        public Int16 VendorStatus
        {
            get;
            set;
        }

        public Int16 VendorSelectionMode
        {
            get;
            set;
        }

        public string OrderNumber
        {
            get;
            set;
        }
        public Int16 TertiaryType
        {
            get;
            set;
        }
        public void InsUpdChequeDetails()
        {

            try
            {
                SqlParam = new SqlParameter[10];
                SqlParam[0] = new SqlParameter("@saleshannelid", SalesChannelID);
                SqlParam[1] = new SqlParameter("@bankname", BankName);
                SqlParam[2] = new SqlParameter("@accountno", AccountNo);
                SqlParam[3] = new SqlParameter("@chequefrom", Checkfrom);
                SqlParam[4] = new SqlParameter("@chequeto", CheckTo);
                SqlParam[5] = new SqlParameter("@amount", Amount);
                SqlParam[6] = new SqlParameter("@depositiondate", DepositionDate);
                SqlParam[7] = new SqlParameter("@chequeno", ChequeNumber);
                SqlParam[8] = new SqlParameter("@outerror", SqlDbType.NVarChar, 200);
                SqlParam[8].Direction = ParameterDirection.Output;
                SqlParam[9] = new SqlParameter("@ordernumber", OrderNumber);
                int r = DataAccess.Instance.DBInsertCommand("prcInsUpdChequeDetails", SqlParam);
                error = Convert.ToString(SqlParam[8].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectChequeInfo()
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@saleschannelid", SalesChannelID);
                SqlParam[1] = new SqlParameter("@checkstatus", ChequeStatus);
                SqlParam[2] = new SqlParameter("@datefrom", DateFrom);
                SqlParam[3] = new SqlParameter("@dateto", DateTo);
                SqlParam[4] = new SqlParameter("@checkno", ChequeNumber);
                d1 = DataAccess.Instance.GetTableFromDatabase("prcGetChequeDetails", CommandType.StoredProcedure, SqlParam);


                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void InsertPriceListInfo()
        {
            try
            {

                string outex = "";
                pricelistid = 0;
                MySqlParam = new MySqlParameter[5];
                MySqlParam[0] = new MySqlParameter("@p_pricelistname", pricelistname);
                MySqlParam[1] = new MySqlParameter("@p_status", priceliststatus);
                MySqlParam[2] = new MySqlParameter("@p_pricelistid", pricelistid);
                MySqlParam[3] = new MySqlParameter("@p_CompanyId", CompanyId);
                MySqlParam[4] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 200);
                MySqlParam[4].Direction = ParameterDirection.Output;
                int r = DataAccess.Instance.DBInsert_MySqlCommand("prcInsUpdPriceList", MySqlParam);
                error = Convert.ToString(MySqlParam[3].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePriceListInfo()
        {
            try
            {
                string outex = "";
                MySqlParam = new MySqlParameter[5];
                MySqlParam[0] = new MySqlParameter("@p_pricelistid", pricelistid);
                MySqlParam[1] = new MySqlParameter("@p_pricelistname", pricelistname);
                MySqlParam[2] = new MySqlParameter("@p_status", priceliststatus);
                MySqlParam[3] = new MySqlParameter("@p_CompanyId", CompanyId);
                MySqlParam[4] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 200);
                MySqlParam[4].Direction = ParameterDirection.Output;
                int r = DataAccess.Instance.DBInsert_MySqlCommand("prcInsUpdPriceList", MySqlParam);
                error = Convert.ToString(MySqlParam[4].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable SelectPriceListInfo()
        {
            try
            {


                MySqlParam = new MySqlParameter[4];
                MySqlParam[0] = new MySqlParameter("@p_pricelistname", PriceListName);
                MySqlParam[1] = new MySqlParameter("@p_pricelistid", pricelistid);
                MySqlParam[2] = new MySqlParameter("@p_selectionmode", pricelistselectionmode);
                MySqlParam[3] = new MySqlParameter("@p_CompanyId", CompanyId);

                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetPriceListDetails", CommandType.StoredProcedure, MySqlParam);


                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable SelectAllPriceListInfo()
        {

            try
            {


                pricelistselectionmode = 1;
                pricelistname = "";
                pricelistid = 0;
                
                MySqlParam = new MySqlParameter[4];
                MySqlParam[0] = new MySqlParameter("@p_pricelistname", pricelistname);
                MySqlParam[1] = new MySqlParameter("@p_pricelistid", pricelistid);
                MySqlParam[2] = new MySqlParameter("@p_selectionmode", pricelistselectionmode);
                MySqlParam[3] = new MySqlParameter("@p_CompanyId", CompanyId);

                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetPriceListDetails", CommandType.StoredProcedure, MySqlParam);


                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        # region sku

        private int skuid; string skucode; string skuname; int skustatus; int skuprodid;
        int skuprodcatid; int skubrandid; int skumodelid; int? skucolorid; int skuselectionmode;
        string skuattribute1; string skuattribute2;


        public int SKUId
        {
            get { return skuid; }
            set { skuid = value; }
        }
        public string XMLList
        {
            get { return strXMLList; }
            set { strXMLList = value; }
        }
        public DateTime? EffectiveDate
        {
            get { return dtEffectiveDate; }
            set { dtEffectiveDate = value; }
        }
        public DateTime? DateRange
        {
            get { return dtDateRange; }
            set { dtDateRange = value; }
        }
        public int PriceListID
        {
            get { return intPriceListId; }
            set { intPriceListId = value; }
        }
        public string SKUName
        {
            get { return skuname; }
            set { skuname = value; }
        }
        public bool? Status
        {
            get { return blnStatus; }
            set { blnStatus = value; }
        }

        public string SKUCode
        {
            get { return skucode; }
            set { skucode = value; }
        }
        public int SKUStatus
        {
            get { return skustatus; }
            set { skustatus = value; }

        }

        public int SKUProdId
        {
            get { return skuprodid; }
            set { skuprodid = value; }
        }

        public int SKUProdCatId
        {
            get { return skuprodcatid; }
            set { skuprodcatid = value; }
        }

        public int SKUBrandId
        {
            get { return skubrandid; }
            set { skubrandid = value; }
        }

        public int SKUModelId
        {
            get { return skumodelid; }
            set { skumodelid = value; }
        }

        public int? SKUColorId
        {
            get { return skucolorid; }
            set { skucolorid = value; }
        }

        public int SKUSelectionMode
        {
            get { return skuselectionmode; }
            set { skuselectionmode = value; }
        }

        public string SKUAttribute1
        {
            get { return skuattribute1; }
            set { skuattribute1 = value; }
        }


        public string SKUAttribute2
        {
            get { return skuattribute2; }
            set { skuattribute2 = value; }
        }
        private string _SKUDesc;
        public string KeyWord { get; set; }/*#CC06 Added*/
        public string SKUDesc
        {
            get { return _SKUDesc; }
            set { _SKUDesc = value; }
        }
        /*#CC02 START ADDED*/
        private int _CartonSIze;
        public int CartonSIze
        {
            get { return _CartonSIze; }
            set { _CartonSIze = value; }
        }
        /*#CC02 START END*/

        public int PageSize
        {
            get;
            set;
        }



        public int PageNo
        {
            get;
            set;
        }

        public int Elements
        {
            get;
            set;
        }

        public int ExcludedSelectionMode
        {
            get;
            set;
        }


        public DataTable SelectExcludedInfo()
        {
            try
            {
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@selectionmode", ExcludedSelectionMode);
                d1 = DataAccess.Instance.GetTableFromDatabase("[prcGetExcludedItems]", CommandType.StoredProcedure, SqlParam);

                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void InsertSKUInfo()
        {
            try
            {

                string outex = "";
                //skuid = 0;#CC06 comented
                SqlParam = new SqlParameter[13];
                SqlParam[0] = new SqlParameter("@skuname", skuname);
                SqlParam[1] = new SqlParameter("@status", skustatus);
                SqlParam[2] = new SqlParameter("@skucode", skucode);
                SqlParam[3] = new SqlParameter("@skuid", skuid);

                SqlParam[4] = new SqlParameter("@skuprodcatid", skuprodcatid);

                SqlParam[5] = new SqlParameter("@skumodelid", skumodelid);
                SqlParam[6] = new SqlParameter("@skucolorid", skucolorid);
                SqlParam[7] = new SqlParameter("@skuattribute1", skuattribute1);
                SqlParam[8] = new SqlParameter("@skuattribute2", skuattribute2);

                SqlParam[9] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 500);
                SqlParam[9].Direction = ParameterDirection.Output;
                SqlParam[10] = new SqlParameter("@skudesc", SKUDesc);
                SqlParam[11] = new SqlParameter("@CartonSIze", CartonSIze); /*#CC02 ADDED*/
                SqlParam[12] = new SqlParameter("@Keyword", KeyWord); /*#CC06 ADDED*/
                int r = DataAccess.Instance.DBInsertCommand("prcInsUpdSKU", SqlParam);
                error = Convert.ToString(SqlParam[9].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateSKUInfo()
        {
            try
            {

                string outex = "";
                SqlParam = new SqlParameter[12];
                SqlParam[0] = new SqlParameter("@skuname", skuname);
                SqlParam[1] = new SqlParameter("@status", skustatus);
                SqlParam[2] = new SqlParameter("@skucode", skucode);
                SqlParam[3] = new SqlParameter("@skuid", skuid);

                SqlParam[4] = new SqlParameter("@skuprodcatid", skuprodcatid);

                SqlParam[5] = new SqlParameter("@skumodelid", skumodelid);
                SqlParam[6] = new SqlParameter("@skucolorid", skucolorid);
                SqlParam[7] = new SqlParameter("@skuattribute1", skuattribute1);
                SqlParam[8] = new SqlParameter("@skuattribute2", skuattribute2);
                SqlParam[9] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
                SqlParam[9].Direction = ParameterDirection.Output;
                SqlParam[10] = new SqlParameter("@skudesc", SKUDesc);
                SqlParam[11] = new SqlParameter("@CartonSIze", CartonSIze); /*#CC02 ADDED*/
                int r = DataAccess.Instance.DBInsertCommand("prcInsUpdSKU", SqlParam);
                error = Convert.ToString(SqlParam[9].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectSKUInfo()
        {
            try
            {
                MySqlParam = new MySqlParameter[9];
                MySqlParam[0] = new MySqlParameter("@p_productid", productid);
                MySqlParam[1] = new MySqlParameter("@p_skuname", skuname);
                MySqlParam[2] = new MySqlParameter("@p_skucode", skucode);
                MySqlParam[3] = new MySqlParameter("@p_skuid", skuid);
                MySqlParam[4] = new MySqlParameter("@p_skuprodcatid", skuprodcatid);
                MySqlParam[5] = new MySqlParameter("@p_skumodelid", skumodelid);
                MySqlParam[6] = new MySqlParameter("@p_skucolorid", skucolorid);
                MySqlParam[7] = new MySqlParameter("@p_selectionmode", skuselectionmode);
                MySqlParam[8] = new MySqlParameter("@p_CompanyId", CompanyId);/*#CC07 Added*/
                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetSKUDetails", CommandType.StoredProcedure, MySqlParam);

                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable SelectAllSKUInfo()
        {

            try
            {


                skucode = "";
                skuname = "";
                skuid = 0;

                skuprodcatid = 0;

                skucolorid = 0;
                skumodelid = 0;
                skuselectionmode = 1;
                SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@skuname", skuname);
                SqlParam[1] = new SqlParameter("@skucode", skucode);
                SqlParam[2] = new SqlParameter("@skuid", skuid);
                SqlParam[3] = new SqlParameter("@skuprodcatid", skuprodcatid);

                SqlParam[4] = new SqlParameter("@skumodelid", skumodelid);
                SqlParam[5] = new SqlParameter("@skucolorid", skucolorid);
                SqlParam[6] = new SqlParameter("@selectionmode", skuselectionmode);
                d1 = DataAccess.Instance.GetTableFromDatabase("prcGetSKUDetails", CommandType.StoredProcedure, SqlParam);

                return d1;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectSKUModelwiseInfo()
        {
            try
            {
                d1 = DataAccess.Instance.GetTableFromDatabase("[prcGetSKUModelWiseDetails]", CommandType.StoredProcedure);

                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectSKUBlockwiseInfo()
        {
            try
            {
                SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@skuname", skuname);
                SqlParam[1] = new SqlParameter("@skucode", skucode);
                SqlParam[2] = new SqlParameter("@skuprodcatid", skuprodcatid);
                SqlParam[3] = new SqlParameter("@skumodelid", skumodelid);
                SqlParam[4] = new SqlParameter("@blockno", PageNo);
                SqlParam[5] = new SqlParameter("@pagesize", PageSize);
                SqlParam[6] = new SqlParameter("@elements", SqlDbType.Int);
                SqlParam[6].Direction = ParameterDirection.Output;
                d1 = DataAccess.Instance.GetTableFromDatabase("[prcGetBlockWiseSKU]", CommandType.StoredProcedure, SqlParam);
                Elements = Convert.ToInt32(SqlParam[6].Value);
                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        # endregion

        # region brand

        private int brandid; string brandcode; string brandname; int brandstatus; int brandselectionmode;
        
        public int BrandId
        {
            get { return brandid; }
            set { brandid = value; }
        }

        public string BrandName
        {
            get { return brandname; }
            set { brandname = value; }
        }

        public string BrandCode
        {
            get { return brandcode; }
            set { brandcode = value; }
        }
        public int BrandStatus
        {
            get { return brandstatus; }
            set { brandstatus = value; }

        }

        public int BrandSelectionMode
        {
            get { return brandselectionmode; }
            set { brandselectionmode = value; }

        }

        public void InsertBrandInfo()
        {
            try
            {

                string outex = "";
                brandid = 0;

                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@brandname", brandname);
                SqlParam[1] = new SqlParameter("@status", brandstatus);
                SqlParam[2] = new SqlParameter("@brandcode", brandcode);
                SqlParam[3] = new SqlParameter("@brandid", brandid);

                SqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
                SqlParam[4].Direction = ParameterDirection.Output;
                int r = DataAccess.Instance.DBInsertCommand("prcInsUpdBrand", SqlParam);
                error = Convert.ToString(SqlParam[4].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateBrandInfo()
        {
            try
            {

                string outex = "";
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@brandid", brandid);
                SqlParam[1] = new SqlParameter("@brandname", brandname);
                SqlParam[2] = new SqlParameter("@brandcode", brandcode);
                SqlParam[3] = new SqlParameter("@status", brandstatus);
                SqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
                SqlParam[4].Direction = ParameterDirection.Output;
                int r = DataAccess.Instance.DBInsertCommand("prcInsUpdBrand", SqlParam);
                error = Convert.ToString(SqlParam[4].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectBrandInfo()
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@brandname", brandname);
                SqlParam[1] = new SqlParameter("@brandid", brandid);
                SqlParam[2] = new SqlParameter("@brandcode", brandcode);
                SqlParam[3] = new SqlParameter("@selectionmode", brandselectionmode);
                SqlParam[4] = new SqlParameter("@CompanyId", CompanyId);
                d1 = DataAccess.Instance.GetTableFromDatabase("prcGetBrandDetails", CommandType.StoredProcedure, SqlParam);
                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable SelectAllBrandInfo()
        {

            try
            {
                brandselectionmode = 1;
                brandcode = "";
                brandname = "";
                brandid = 0;
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@brandname", brandname);
                SqlParam[1] = new SqlParameter("@brandid", brandid);
                SqlParam[2] = new SqlParameter("@brandcode", brandcode);
                SqlParam[3] = new SqlParameter("@selectionmode", brandselectionmode);
                SqlParam[4] = new SqlParameter("@CompanyId", CompanyId);
                d1 = DataAccess.Instance.GetTableFromDatabase("prcGetBrandDetails", CommandType.StoredProcedure, SqlParam);
                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #endregion

        # region model

        private int modelid; string modelcode; string modelname; int modelstatus; int modelprodid;
        int modelprodcatid; int modelbrandid; int modelselectionmode;

        public int ModelId
        {
            get { return modelid; }
            set { modelid = value; }
        }

        public string ModelName
        {
            get { return modelname; }
            set { modelname = value; }
        }

        public string ModelCode
        {
            get { return modelcode; }
            set { modelcode = value; }
        }
        public int ModelStatus
        {
            get { return modelstatus; }
            set { modelstatus = value; }

        }

        public int ModelProdId
        {
            get { return modelprodid; }
            set { modelprodid = value; }
        }

        public int ModelProdCatId
        {
            get { return modelprodcatid; }
            set { modelprodcatid = value; }
        }

        public int ModelBrandId
        {
            get { return modelbrandid; }
            set { modelbrandid = value; }
        }

        public int ModelSelectionMode
        {
            get { return modelselectionmode; }
            set { modelselectionmode = value; }
        }

        public void InsertModelInfo()
        {
            try
            {

                string outex = "";
                modelid = 0;

                SqlParam = new SqlParameter[10];
                SqlParam[0] = new SqlParameter("@modelname", modelname);
                SqlParam[1] = new SqlParameter("@status", modelstatus);
                SqlParam[2] = new SqlParameter("@modelcode", modelcode);
                SqlParam[3] = new SqlParameter("@modelid", modelid);
                SqlParam[4] = new SqlParameter("@modelprodid", modelprodid);
                SqlParam[5] = new SqlParameter("@modelprodcatid", modelprodcatid);
                SqlParam[6] = new SqlParameter("@modelbrandid", modelbrandid);
                SqlParam[7] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
                SqlParam[7].Direction = ParameterDirection.Output;
                SqlParam[8] = new SqlParameter("@ModelMode", ModelMode);    //Pankaj Dhingra
                SqlParam[9] = new SqlParameter("@ModelType", ModelType);       //Added two new parameters in the manage Model 
                int r = DataAccess.Instance.DBInsertCommand("prcInsUpdModel", SqlParam);
                error = Convert.ToString(SqlParam[7].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateModelInfo()
        {
            try
            {

                string outex = "";

                SqlParam = new SqlParameter[10];
                SqlParam[0] = new SqlParameter("@modelname", modelname);
                SqlParam[1] = new SqlParameter("@status", modelstatus);
                SqlParam[2] = new SqlParameter("@modelcode", modelcode);
                SqlParam[3] = new SqlParameter("@modelid", modelid);
                SqlParam[4] = new SqlParameter("@modelprodid", modelprodid);
                SqlParam[5] = new SqlParameter("@modelprodcatid", modelprodcatid);
                SqlParam[6] = new SqlParameter("@modelbrandid", modelbrandid);


                SqlParam[7] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
                SqlParam[7].Direction = ParameterDirection.Output;
                SqlParam[8] = new SqlParameter("@ModelMode", ModelMode);    //Pankaj Dhingra
                SqlParam[9] = new SqlParameter("@ModelType", ModelType);       //Added two new parameters in the manage Model 
                int r = DataAccess.Instance.DBInsertCommand("prcInsUpdModel", SqlParam);
                error = Convert.ToString(SqlParam[7].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public DataTable SelectModelInfo()
        {

            try
            {

                modelcode = "";
                modelname = "";

                MySqlParam = new MySqlParameter[8];
                MySqlParam[0] = new MySqlParameter("@p_modelname", modelname);
                MySqlParam[1] = new MySqlParameter("@p_modelcode", modelcode);
                MySqlParam[2] = new MySqlParameter("@p_modelid", modelid);
                MySqlParam[3] = new MySqlParameter("@p_modelprodid", modelprodid);
                MySqlParam[4] = new MySqlParameter("@p_modelprodcatid", modelprodcatid);
                MySqlParam[5] = new MySqlParameter("@p_modelbrandid", modelbrandid);
                MySqlParam[6] = new MySqlParameter("@p_selectionmode", modelselectionmode);
                MySqlParam[7] = new MySqlParameter("@p_CompanyId", CompanyId);
                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetModelDetails", CommandType.StoredProcedure, MySqlParam);

                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable SelectAllModelInfo()
        {

            try
            {


                modelcode = "";
                modelname = "";
                modelid = 0;
                modelprodid = 0;
                modelprodcatid = 0;
                modelbrandid = 0;
                modelselectionmode = 1;
                SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@modelname", modelname);

                SqlParam[1] = new SqlParameter("@modelcode", modelcode);
                SqlParam[2] = new SqlParameter("@modelid", modelid);
                SqlParam[3] = new SqlParameter("@modelprodid", modelprodid);
                SqlParam[4] = new SqlParameter("@modelprodcatid", modelprodcatid);
                SqlParam[5] = new SqlParameter("@modelbrandid", modelbrandid);
                SqlParam[6] = new SqlParameter("@selectionmode", modelselectionmode);
                d1 = DataAccess.Instance.GetTableFromDatabase("prcGetModelDetails", CommandType.StoredProcedure, SqlParam);

                return d1;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #endregion

        # region color

        private int colorid; string colorname; int colorstatus; int colorselectionmode;

        public int ColorId
        {
            get { return colorid; }
            set { colorid = value; }
        }

        public string ColorName
        {
            get { return colorname; }
            set { colorname = value; }
        }


        public int ColorStatus
        {
            get { return colorstatus; }
            set { colorstatus = value; }

        }

        public int ColorSelectionMode
        {
            get { return colorselectionmode; }
            set { colorselectionmode = value; }

        }

        public void InsertColorInfo()
        {
            try
            {

                string outex = "";
                colorid = 0;

                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@colorname", colorname);
                SqlParam[1] = new SqlParameter("@status", colorstatus);
                SqlParam[2] = new SqlParameter("@colorid", colorid);

                SqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
                SqlParam[3].Direction = ParameterDirection.Output;
                int r = DataAccess.Instance.DBInsertCommand("prcInsUpdColor", SqlParam);
                error = Convert.ToString(SqlParam[3].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateColorInfo()
        {
            try
            {

                string outex = "";


                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@colorid", colorid);
                SqlParam[1] = new SqlParameter("@colorname", colorname);

                SqlParam[2] = new SqlParameter("@status", colorstatus);
                SqlParam[3] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
                SqlParam[3].Direction = ParameterDirection.Output;
                int r = DataAccess.Instance.DBInsertCommand("prcInsUpdColor", SqlParam);
                error = Convert.ToString(SqlParam[3].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetAllBrandByParameters()
        {
            try
            {
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@SearchConditions", eSearchType);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetAllBrandByParameters", CommandType.StoredProcedure, SqlParam);

                return dtResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAllProductCategoryByParameters()//Pankaj Kumar
        {
            try
            {
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@SearchConditions", eSearchType);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetAllProductCategoryByParameters", CommandType.StoredProcedure, SqlParam);

                return dtResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectColorInfo()
        {

            try
            {



                MySqlParam = new MySqlParameter[4];
                MySqlParam[0] = new MySqlParameter("@p_colorname", colorname);
                MySqlParam[1] = new MySqlParameter("@p_colorid", colorid);
                MySqlParam[2] = new MySqlParameter("@p_selectionmode", colorselectionmode);
                MySqlParam[3] = new MySqlParameter("@p_CompanyId", CompanyId);
                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetColorDetails", CommandType.StoredProcedure, MySqlParam);


                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable SelectAllColorInfo()
        {

            try
            {
                colorname = "";
                colorid = 0;
                colorselectionmode = 1;
                MySqlParam = new MySqlParameter[4];
                MySqlParam[0] = new MySqlParameter("@p_colorname", colorname);
                MySqlParam[1] = new MySqlParameter("@p_colorid", colorid);
                MySqlParam[2] = new MySqlParameter("@p_selectionmode", colorselectionmode);
                MySqlParam[3] = new MySqlParameter("@p_CompanyId", CompanyId);/*#CC07 Added*/
                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetColorDetails", CommandType.StoredProcedure, MySqlParam);

                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        #endregion


        # region Product Category

        private int prodcatid; string prodcatcode; string prodcatname; int prodcatstatus; int prodcatselectionmode;

        public int ProdCatId
        {
            get { return prodcatid; }
            set { prodcatid = value; }
        }

        public string ProdCatName
        {
            get { return prodcatname; }
            set { prodcatname = value; }
        }

        public string ProdCatCode
        {
            get { return prodcatcode; }
            set { prodcatcode = value; }
        }
        public int ProdCatStatus
        {
            get { return prodcatstatus; }
            set { prodcatstatus = value; }

        }


        public int ProdCatSelectionMode
        {
            get { return prodcatselectionmode; }
            set { prodcatselectionmode = value; }

        }






        public void InsertProdCatInfo()
        {
            try
            {


                prodcatid = 0;

                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@prodcatname", prodcatname);
                SqlParam[1] = new SqlParameter("@status", prodcatstatus);
                SqlParam[2] = new SqlParameter("@prodcatcode", prodcatcode);
                SqlParam[3] = new SqlParameter("@prodcatid", prodcatid);

                SqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
                SqlParam[4].Direction = ParameterDirection.Output;
                int r = DataAccess.Instance.DBInsertCommand("prcInsUpdProductCategory", SqlParam);
                error = Convert.ToString(SqlParam[4].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateProdCatInfo()
        {
            try
            {




                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@prodcatid", prodcatid);
                SqlParam[1] = new SqlParameter("@prodcatname", prodcatname);
                SqlParam[2] = new SqlParameter("@prodcatcode", prodcatcode);
                SqlParam[3] = new SqlParameter("@status", prodcatstatus);

                SqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
                SqlParam[4].Direction = ParameterDirection.Output;
                int r = DataAccess.Instance.DBInsertCommand("prcInsUpdProductCategory", SqlParam);
                error = Convert.ToString(SqlParam[4].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable SelectProdCatInfo()
        {

            try
            {

                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@prodcatname", prodcatname);
                SqlParam[1] = new SqlParameter("@prodcatid", prodcatid);
                SqlParam[2] = new SqlParameter("@prodcatcode", prodcatcode);
                SqlParam[3] = new SqlParameter("@selectionmode", prodcatselectionmode);
                d1 = DataAccess.Instance.GetTableFromDatabase("prcGetProductCategoryDetails", CommandType.StoredProcedure, SqlParam);


                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable SelectAllProdCatInfo()
        {

            try
            {


                prodcatcode = "";
                prodcatname = "";
                prodcatid = 0;
                prodcatselectionmode = 1;
                MySqlParam = new MySqlParameter[5];
                MySqlParam[0] = new MySqlParameter("@p_prodcatname", prodcatname);
                MySqlParam[1] = new MySqlParameter("@p_prodcatid", prodcatid);
                MySqlParam[2] = new MySqlParameter("@p_prodcatcode", prodcatcode);
                MySqlParam[3] = new MySqlParameter("@p_selectionmode", prodcatselectionmode);
                MySqlParam[4] = new MySqlParameter("@p_CompanyId", CompanyId);
                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetProductCategoryDetails", CommandType.StoredProcedure, MySqlParam);


                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        # endregion

        # region Product

        private int productid; string productcode; string productname; int productstatus; int productselectionmode;


        public int ProductId
        {
            get { return productid; }
            set { productid = value; }
        }

        public string ProductName
        {
            get { return productname; }
            set { productname = value; }
        }

        public string ProductCode
        {
            get { return productcode; }
            set { productcode = value; }
        }
        public int ProductStatus
        {
            get { return productstatus; }
            set { productstatus = value; }

        }

        public int ProductSelectionMode
        {
            get { return productselectionmode; }
            set { productselectionmode = value; }

        }


        public void InsertVendorInfo()
        {
            try
            {

                string outex = "";
                productid = 0;

                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@vendorname", VendorName);
                SqlParam[1] = new SqlParameter("@status", VendorStatus);
                SqlParam[2] = new SqlParameter("@vendorcode", VendorCode);
                SqlParam[3] = new SqlParameter("@vendorid", VendorID);

                SqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
                SqlParam[4].Direction = ParameterDirection.Output;
                int r = DataAccess.Instance.DBInsertCommand("[prcInsUpdVendor]", SqlParam);
                error = Convert.ToString(SqlParam[4].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void UpdateVendorInfo()
        {
            try
            {

                string outex = "";


                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@vendorid", VendorID);
                SqlParam[1] = new SqlParameter("@vendorname", VendorName);
                SqlParam[2] = new SqlParameter("@vendorcode", VendorCode);
                SqlParam[3] = new SqlParameter("@status", VendorStatus);

                SqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
                SqlParam[4].Direction = ParameterDirection.Output;
                int r = DataAccess.Instance.DBInsertCommand("[prcInsUpdVendor]", SqlParam);
                error = Convert.ToString(SqlParam[4].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable SelectVendorInfo()
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@vendorname", VendorName);
                SqlParam[1] = new SqlParameter("@vendorid", VendorID);
                SqlParam[2] = new SqlParameter("@vendorcode", VendorCode);
                SqlParam[3] = new SqlParameter("@selectionmode", VendorSelectionMode);
                d1 = DataAccess.Instance.GetTableFromDatabase("[prcGetVendorDetails]", CommandType.StoredProcedure, SqlParam);


                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertProductInfo()
        {
            try
            {

                string outex = "";
                productid = 0;

                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@productid", productid);
                SqlParam[1] = new SqlParameter("@productname", productname);
                SqlParam[2] = new SqlParameter("@productcode", productcode);
                SqlParam[3] = new SqlParameter("@status", productstatus);


                SqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
                SqlParam[4].Direction = ParameterDirection.Output;
                int r = DataAccess.Instance.DBInsertCommand("[prcInsUpdProduct]", SqlParam);
                error = Convert.ToString(SqlParam[4].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateProductInfo()
        {
            try
            {

                string outex = "";


                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@productid", productid);
                SqlParam[1] = new SqlParameter("@productname", productname);
                SqlParam[2] = new SqlParameter("@productcode", productcode);
                SqlParam[3] = new SqlParameter("@status", productstatus);

                SqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
                SqlParam[4].Direction = ParameterDirection.Output;
                int r = DataAccess.Instance.DBInsertCommand("prcInsUpdProduct", SqlParam);
                error = Convert.ToString(SqlParam[4].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable SelectProductInfo()
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@productname", productname);
                SqlParam[1] = new SqlParameter("@productid", productid);
                SqlParam[2] = new SqlParameter("@productcode", productcode);
                SqlParam[3] = new SqlParameter("@selectionmode", productselectionmode);
                d1 = DataAccess.Instance.GetTableFromDatabase("prcGetProductDetails", CommandType.StoredProcedure, SqlParam);


                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable SelectAllProductInfo()
        {

            try
            {

                productselectionmode = 1;
                productcode = "";
                productname = "";
                productid = 0;
                productselectionmode = 1;
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@productname", productname);
                SqlParam[1] = new SqlParameter("@productid", productid);
                SqlParam[2] = new SqlParameter("@productcode", productcode);
                SqlParam[3] = new SqlParameter("@selectionmode", productselectionmode);

                d1 = DataAccess.Instance.GetTableFromDatabase("prcGetProductDetails", CommandType.StoredProcedure, SqlParam);

                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        # endregion


        #region SKUInformation

        public int SalesChannelID
        {
            get;
            set;
        }
        public int RequestType
        {
            get;
            set;
        }
        public int Condition
        {
            get;
            set;
        }

        public DataTable GetSKUInfo()
        {
            try
            {
                MySqlParam = new MySqlParameter[4];
                MySqlParam[0] = new MySqlParameter("@p_Status", Status);
                MySqlParam[1] = new MySqlParameter("@p_RequestType", RequestType);
                MySqlParam[2] = new MySqlParameter("@p_SalesChannelID", SalesChannelID);
                MySqlParam[3] = new MySqlParameter("@p_CompanyId", CompanyId);
                dtResult = DataAccess.Instance.GetTableFrom_MySqlDatabase("PrcGetSKUInfoByParameters", CommandType.StoredProcedure, MySqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable GetSKUInfoOffer()
        {
            try
            {
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@Date", DateFrom);
                dtResult = DataAccess.Instance.GetTableFromDatabase("PrcGetSKUInfoOffer", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        #endregion

        #region PriceListInformation
        public DataTable GetPriceListInfo()
        {
            try
            {
                MySqlParam = new MySqlParameter[2];
                MySqlParam[0] = new MySqlParameter("@p_Status", Status);
                MySqlParam[1] = new MySqlParameter("@p_CompanyId", CompanyId);
                dtResult = DataAccess.Instance.GetTableFrom_MySqlDatabase("PrcGetPriceListInfoByParameters", CommandType.StoredProcedure, MySqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetPriceListInfoV2()
        {
            try
            {
                MySqlParam = new MySqlParameter[4];
                MySqlParam[0] = new MySqlParameter("@p_Status", Status);
                MySqlParam[1] = new MySqlParameter("@p_UserID", UserId);
                MySqlParam[2] = new MySqlParameter("@p_Condition", Condition);
                MySqlParam[3] = new MySqlParameter("@p_ErrorMsg", MySqlDbType.VarChar, 200);
                MySqlParam[3].Direction = ParameterDirection.Output;
                dtResult = DataAccess.Instance.GetTableFrom_MySqlDatabase("PrcGetPriceListInfoByParametersV2", CommandType.StoredProcedure, MySqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Int32 InsertUpdatePriceInfo()
        {
            try
            {
                MySqlParam = new MySqlParameter[5];
                MySqlParam[0] = new MySqlParameter("@p_PriceListID", PriceListID);
                MySqlParam[1] = new MySqlParameter("@p_PriceListXML", SqlDbType.Xml);
                MySqlParam[1].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(XMLList, XmlNodeType.Document, null));
                MySqlParam[1].Direction = ParameterDirection.InputOutput;
                MySqlParam[2] = new MySqlParameter("@p_EffectiveDate", EffectiveDate);
                MySqlParam[3] = new MySqlParameter("@p_Status", Status);
                MySqlParam[4] = new MySqlParameter("@p_Out_Param", MySqlDbType.Int16, 2);
                MySqlParam[4].Direction = ParameterDirection.Output;
                DataAccess.Instance.DBInsert_MySqlCommand("PrcInsUpdPriceMasterInfo", MySqlParam);
                IntResultCount = Convert.ToInt16(MySqlParam[4].Value);
                if (((System.Data.SqlTypes.SqlXml)MySqlParam[1].Value).IsNull != true)
                {
                    XMLList = ((System.Data.SqlTypes.SqlXml)MySqlParam[1].Value).Value;
                }
                else
                {
                    XMLList = null;
                }
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Int32 InsertUpdateCustomPriceInfo()
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@PriceListID", PriceListID);
                SqlParam[1] = new SqlParameter("@PriceListXML", SqlDbType.Xml);
                SqlParam[1].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(XMLList, XmlNodeType.Document, null));
                SqlParam[1].Direction = ParameterDirection.InputOutput;
                SqlParam[2] = new SqlParameter("@EffectiveDate", EffectiveDate);
                SqlParam[3] = new SqlParameter("@Status", Status);
                SqlParam[4] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[4].Direction = ParameterDirection.Output;
                DataAccess.Instance.DBInsertCommand("PrcInsUpdCustomPriceMasterInfo", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[4].Value);
                if (((System.Data.SqlTypes.SqlXml)SqlParam[1].Value).IsNull != true)
                {
                    XMLList = ((System.Data.SqlTypes.SqlXml)SqlParam[1].Value).Value;
                }
                else
                {
                    XMLList = null;
                }
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetPriceInfo()
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@PriceListId", PriceListID);
                SqlParam[1] = new SqlParameter("@SKUId", SKUId);
                SqlParam[2] = new SqlParameter("@DateFrom", EffectiveDate);
                SqlParam[3] = new SqlParameter("@DateTo", DateRange);
                dtResult = DataAccess.Instance.GetTableFromDatabase("PrcGetPriceInfoByParameters", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable GetPriceInfoV2()
        {
            try
            {
                MySqlParam = new MySqlParameter[6];
                MySqlParam[0] = new MySqlParameter("@p_PriceListId", PriceListID);
                MySqlParam[1] = new MySqlParameter("@p_SKUId", SKUId);
                MySqlParam[2] = new MySqlParameter("@p_DateFrom", EffectiveDate);
                MySqlParam[3] = new MySqlParameter("@p_DateTo", DateRange);
                MySqlParam[4] = new MySqlParameter("@p_UserID", UserId);
                MySqlParam[5] = new MySqlParameter("@p_Condition", Condition);
                dtResult = DataAccess.Instance.GetTableFrom_MySqlDatabase("PrcGetPriceInfoByParametersV2", CommandType.StoredProcedure, MySqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        /* #CC03 Add Start */
        public DataTable GetPriceInfoV3()
        {
            try
            {
                MySqlParameter[] MySqlParam = new MySqlParameter[10];
                MySqlParam[0] = new MySqlParameter("@p_PriceListId", PriceListID);
                MySqlParam[1] = new MySqlParameter("@p_DateFrom", FromDate);
                MySqlParam[2] = new MySqlParameter("@p_DateTo", ToDate);
                MySqlParam[3] = new MySqlParameter("@p_SKUId", SKUId);
                MySqlParam[4] = new MySqlParameter("@p_PageIndex", PageIndex);
                MySqlParam[5] = new MySqlParameter("@p_PageSize", PageSize);
                MySqlParam[6] = new MySqlParameter("@p_TotalRecord", MySqlDbType.Int64, 8);
                MySqlParam[6].Direction = ParameterDirection.Output;
                MySqlParam[7] = new MySqlParameter("@p_OutError", MySqlDbType.VarChar, 500);
                MySqlParam[7].Direction = ParameterDirection.Output;
                MySqlParam[8] = new MySqlParameter("@p_UserId", UserId);
                MySqlParam[9] = new MySqlParameter("@p_Condition", Condition);
                dtResult = ((DataSet)DataAccess.Instance.GetDataSetFrom_MySqlDatabase("PrcGetPriceInfoByParametersV3", CommandType.StoredProcedure, MySqlParam)).Tables[0];
                error = Convert.ToString(MySqlParam[7].Value);
                TotalCount = Convert.ToInt32(MySqlParam[6].Value);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /* #CC03 Add End */

        public Int32 UpdateStatusPriceInfo()
        {
            try
            {
                MySqlParam = new MySqlParameter[1];
                MySqlParam[0] = new MySqlParameter("@p_PriceMasterID", PriceMasterID);
                IntResultCount = DataAccess.Instance.DBInsert_MySqlCommand("PrcUpdStatusPrice", MySqlParam);
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Int32 DeletePriceInfo()
        {
            try
            {
                MySqlParam = new MySqlParameter[3];
                MySqlParam[0] = new MySqlParameter("@p_PriceMasterID", PriceMasterID);
                MySqlParam[1] = new MySqlParameter("@p_Out_Param", MySqlDbType.Int16, 2);
                MySqlParam[1].Direction = ParameterDirection.Output;
                MySqlParam[2] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 500);
                MySqlParam[2].Direction = ParameterDirection.Output;
                DataAccess.Instance.DBInsert_MySqlCommand("prcPriceMaster_Delete", MySqlParam);
                IntResultCount = Convert.ToInt16(MySqlParam[1].Value);
                error = Convert.ToString(MySqlParam[2].Value);
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion
        #region PriceListInformation
        public DataTable GetPriceDropDate()
        {
            try
            {
                SqlParam = new SqlParameter[1];

                SqlParam[0] = new SqlParameter("@UserId", intUserId);

                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetPriceDropDate", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        #region SalesReturn
        public Int32 ReturnFromSalesChannelID
        {
            get;
            set;
        }
        #endregion

        public DataTable SelectUserBasedonSalesChannelID()      //Developed for Bliss POC       Pankaj Dhingra
        {
            try
            {
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@SalesChannelID", SalesChannelID);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetUserNameBasedonSaleschannelID", CommandType.StoredProcedure, SqlParam);

                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet SelectSkuDataBasedonType()      //Developed for Bliss POC       Pankaj Dhingra
        {
            try
            {
                DsResult = DataAccess.Instance.GetDataSetFromDatabase("prcSelectSkuDataBasedonType", CommandType.StoredProcedure);

                return DsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        ~ProductData()
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




        /*   added by amit agarwal on 7 may 2012 */
        /*New Parameter has been added for branding Purpose             Pankaj Dhingra*/
        public DataTable GetSKUInfoByCode()
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@SKUCode", skucode);
                SqlParam[1] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[2] = new SqlParameter("@BrandId", BrandId);
                SqlParam[3] = new SqlParameter("@SKUName", skuname);
                SqlParam[4] = new SqlParameter("@CompanyId", CompanyId);
                dtResult = DataAccess.Instance.GetTableFromDatabase("PrcGetSKUInfoBySKUCode", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public DataTable GetModelInfoByName()
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@ModelCode", modelcode);
                SqlParam[1] = new SqlParameter("@ModelName", ModelName);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@BrandId", BrandId);
                dtResult = DataAccess.Instance.GetTableFromDatabase("PrcGetModelInfoByModelName", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private string _strInvoiceNumber = string.Empty;
        public string InvoiceNumber
        {
            set
            {
                _strInvoiceNumber = value;
            }
        }
        public DateTime InvoiceDate
        {
            get;
            set;
        }


        private string _salesChannelCode = string.Empty;
        public string SalesChannelCode
        {
            set
            {
                _salesChannelCode = value;
            }
        }
        public DataTable GetSKUStockInHandAndIsSerializedByCode()
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@SKUCode", skucode);
                SqlParam[1] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[2] = new SqlParameter("@SalesChannelCode", _salesChannelCode);
                SqlParam[3] = new SqlParameter("@CompanyId", CompanyId);

                dtResult = DataAccess.Instance.GetTableFromDatabase("PrcGetSKUStockInHandAndIsSerializedBySKUCode", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public DataTable GetSKUStockInHandAndIsSerializedByName()
        {
            try
            {
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@SKUName", SKUName);
                SqlParam[1] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[2] = new SqlParameter("@SalesChannelCode", _salesChannelCode);

                dtResult = DataAccess.Instance.GetTableFromDatabase("PrcGetSKUStockInHandAndIsSerializedBySKUName", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public DataTable GetSKUSalesInformation()
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@SKUCode", skucode);
                SqlParam[1] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[2] = new SqlParameter("@SalesChannelCode", _salesChannelCode);
                SqlParam[3] = new SqlParameter("@InvoiceNumber", _strInvoiceNumber);
                SqlParam[4] = new SqlParameter("@InvoiceDate", InvoiceDate);
                SqlParam[5] = new SqlParameter("@Value", Value);
                dtResult = DataAccess.Instance.GetTableFromDatabase("PrcGetSKUSalesInformation", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        #region ReturnInterfaces

        public DataTable GetSerialNosByCodeForReturn()
        {
            try
            {
                SqlParam = new SqlParameter[8];
                SqlParam[0] = new SqlParameter("@SKUCode", skucode);
                SqlParam[1] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[2] = new SqlParameter("@SalesChannelCode", _salesChannelCode);
                SqlParam[3] = new SqlParameter("@InvoiceNumber", _strInvoiceNumber);
                SqlParam[4] = new SqlParameter("@InvoiceDate", InvoiceDate);
                SqlParam[5] = new SqlParameter("@ReturnFromSalesChannelID", ReturnFromSalesChannelID);
                SqlParam[6] = new SqlParameter("@Value", Value);
                SqlParam[7] = new SqlParameter("@StockBinTypeMasterID", StockBinType);/*#CC01 ADDED*/
                dtResult = DataAccess.Instance.GetTableFromDatabase("PrcGetSerialNoBySKUCodeForReturn", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable GetBatchNosByCodeForReturn()
        {
            try
            {
                SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@SKUCode", skucode);
                SqlParam[1] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[2] = new SqlParameter("@SalesChannelCode", _salesChannelCode);
                SqlParam[3] = new SqlParameter("@InvoiceNumber", _strInvoiceNumber);
                SqlParam[4] = new SqlParameter("@InvoiceDate", InvoiceDate);
                SqlParam[5] = new SqlParameter("@ReturnFromSalesChannelID", ReturnFromSalesChannelID);
                SqlParam[6] = new SqlParameter("@Value", Value);
                dtResult = DataAccess.Instance.GetTableFromDatabase("PrcGetBatchNosBySKUCodeForReturn", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        #endregion



        public DataTable GetSerialNosByCode()
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@SKUCode", skucode);
                SqlParam[1] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[2] = new SqlParameter("@SalesChannelCode", _salesChannelCode);
                SqlParam[3] = new SqlParameter("@TypeID", _TypeID);
                SqlParam[4] = new SqlParameter("@StockBinTypeMasterID", StockBinType);
                dtResult = DataAccess.Instance.GetTableFromDatabase("PrcGetSerialNoBySKUCode", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public DataTable GetBatchNosByCode()
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@SKUCode", skucode);
                SqlParam[1] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[2] = new SqlParameter("@SalesChannelCode", _salesChannelCode);
                SqlParam[3] = new SqlParameter("@TypeID", _TypeID);

                dtResult = DataAccess.Instance.GetTableFromDatabase("PrcGetBatchNosBySKUCode", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private short _isValidate = 0;
        private short _out_Param = 0;

        public string IMEI { get; set; }
        public string Batchcode { get; set; }
        public int RetailerID { get; set; }

        public short IsValidate { get { return _isValidate; } set { _isValidate = value; } }
        public short Out_Param { get { return _out_Param; } set { _out_Param = value; } }

        public DataTable GetIEMIInfoByRetilerID()
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@RetailerID", RetailerID);
                SqlParam[1] = new SqlParameter("@IMEI", IMEI);
                SqlParam[2] = new SqlParameter("@Out_Param", _out_Param);
                SqlParam[2].Direction = ParameterDirection.InputOutput;
                SqlParam[3] = new SqlParameter("@isValidate", _isValidate);
                dtResult = DataAccess.Instance.GetTableFromDatabase("PrcGetIEMIInfoByRetailerID", CommandType.StoredProcedure, SqlParam);

                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public DataTable GetBatchcodeInfoByRetilerID()
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@RetailerID", RetailerID);
                SqlParam[1] = new SqlParameter("@Batchcode", Batchcode);
                SqlParam[2] = new SqlParameter("@Out_Param", _out_Param);
                SqlParam[2].Direction = ParameterDirection.InputOutput;
                SqlParam[3] = new SqlParameter("@isValidate", _isValidate);
                dtResult = DataAccess.Instance.GetTableFromDatabase("PrcGetBatchCodeInfoByRetailerID", CommandType.StoredProcedure, SqlParam);

                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private byte _TypeID = 0;

        public byte TypeID
        {
            get { return _TypeID; }
            set { _TypeID = value; }
        }

        public DataTable GetSKUStockInHandBySalesChannelOrRetailer()
        {
            try
            {

                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@SKUCode", skucode);
                SqlParam[1] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[2] = new SqlParameter("@SalesChannelCode", _salesChannelCode);
                SqlParam[3] = new SqlParameter("@TypeID", _TypeID);
                SqlParam[4] = new SqlParameter("@StockBinTypeId", _intStockBinType);/*#CC04 ADDED*/

                dtResult = DataAccess.Instance.GetTableFromDatabase("PrcGetSKUStockInHandByLogin", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private int _ISPID = 0;

        public int ISPID
        {
            get { return _ISPID; }
            set { _ISPID = value; }
        }

        public DataTable GetIEMIInfoByISP()
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@ISPID", ISPID);
                SqlParam[1] = new SqlParameter("@IMEI", IMEI);
                SqlParam[2] = new SqlParameter("@Out_Param", _out_Param);
                SqlParam[2].Direction = ParameterDirection.InputOutput;
                SqlParam[3] = new SqlParameter("@UserID", UserId);
                dtResult = DataAccess.Instance.GetTableFromDatabase("PrcGetIEMIInfoByISPID", CommandType.StoredProcedure, SqlParam);
                Out_Param = Convert.ToInt16(SqlParam[2].Value);

                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public DataTable SelectModelList()
        {

            try
            {



                SqlParam = new SqlParameter[1];

                SqlParam[0] = new SqlParameter("@TertiaryType", TertiaryType);
                d1 = DataAccess.Instance.GetTableFromDatabase("prcgetModelList", CommandType.StoredProcedure, SqlParam);

                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}


