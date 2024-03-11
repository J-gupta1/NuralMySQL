﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections;
using BussinessLogic;
/*
 * 18 July 2016, Karam Chand Sharma, #CC01, Added StockBinType in datatable
 * 27-Jul-2016, Sumit Maurya, #CC02, new datarow added StockBinType.
 * * 30-Mar-2018, Rajnish Kumar, #CC03, To allow duplicate Retailer Name In Infocus.
 * 12/June/2018, #CC04, RAKESH RAJ, Used for Uploading Master Excel Sheet Data like Color Master Upload (TVP) 
 * 05-Dec-2018, #CC05, Sumit Maurya, properies and function created /modified for OutstandingAmountUpload (Done for Inovu).
 * 01-May-2019,#CC06,Vijay Kumar Prajapati,Added Table for tertiarysale Upload
 * 15-May-2020,#CC07,Vijay Kumar Prajapati,Added CompanyId in Method.
 */
namespace DataAccess
{
    public class CommonData : IDisposable
    {
        private string docXML, strSpName, strTableName , strError,  strErrorDetailXML;
        SqlParameter[] SqlParam;
        MySqlParameter[] MySqlParam;
        DataSet dsUploadschema;
        DataTable dtUploadSchema, dtCommon;
        Int32 IntResultCount = 0, intSalesCHannelID = 0, intBrandID = 0;
        private string strCategory;
        private Int32 intCategoryId;
        private bool blnStatus;
        public string DocXML
        {
            get { return docXML; }
            set { docXML = value; }
        }
        public string SPName
        {
            get
            {
                return strSpName;
            }
            set
            {
                strSpName = value;
            }
        }
        public Int32 SalesCHannelID
        {
            get
            {
                return intSalesCHannelID;
            }
            set
            {
                intSalesCHannelID = value;
            }
        }
        public Int32 BrandID
        {
            get
            {
                return intBrandID;
            }
            set
            {
                intBrandID = value;
            }
        }
        public EnumData.eUploadExcelValidationType UploadValidationType
        {
            get;
            set;
        }
        public EnumData.eTargetTemplateType TemplateType
        {
            get;
            set;
        }

        public int intOutParam
        {
            get;
            set;
        }

        public string ErrorDetailXML
        {
            get { return strErrorDetailXML; }
            set { strErrorDetailXML = value; }
        }

        public string Error
        {
            get { return strError; }
            set { strError = value; }
        }
        
        public string SessionID
        {
            get;
            set;
        }
        /* #CC02 Add Start */
        public int UserID { get; set; }
        /* #CC02 Add End */
        public string UploadTableName
        {
            get { return strTableName; }
            set { strTableName = value; }
        }
        public Int32 CompanyId { get; set; }/*#CC07 Added*/
        #region Get Schema By Matched  Excel File Template
        public DataSet InsertThroughUpload(ref int Result)
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@doc", docXML);
                SqlParam[1] = new SqlParameter("@OutResult", 0);
                SqlParam[1].Direction = ParameterDirection.Output;
                dsUploadschema = DataAccess.Instance.GetDataSetFromDatabase(strSpName, CommandType.StoredProcedure, SqlParam);
                Result = Convert.ToInt32(SqlParam[2].Value);
                return dsUploadschema;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public DataTable getSchemaForUpload()
        {
            MySqlParam = new MySqlParameter[2];
            MySqlParam[0] = new MySqlParameter("@p_UploadTableName", strTableName);
            MySqlParam[1] = new MySqlParameter("@p_CompanyId", CompanyId);
            dtUploadSchema = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetSchemaForUpload", CommandType.StoredProcedure,MySqlParam);

            if (UploadValidationType == EnumData.eUploadExcelValidationType.eTarget)
            {
                if (TemplateType == (EnumData.eTargetTemplateType.eSKUWise))
                {
                    dtUploadSchema.DefaultView.RowFilter = "TableColumnName<>'BrandCode'";
                    dtUploadSchema = dtUploadSchema.DefaultView.ToTable();
                }

                else
                {
                    dtUploadSchema.DefaultView.RowFilter = "TableColumnName<>'SKUCode'";
                    dtUploadSchema = dtUploadSchema.DefaultView.ToTable();
                }
            }
            CreateSchema(ref dtUploadSchema);
            return dtUploadSchema;
        }

        /*#CC07 Added Started*/
        public DataTable getSchemaForUploadWithCompanyId()
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@UploadTableName", strTableName);
            SqlParam[1] = new SqlParameter("@CompanyId", CompanyId);
            dtUploadSchema = DataAccess.Instance.GetTableFromDatabase("prcGetSchemaForUploadWithCompanyId", CommandType.StoredProcedure, SqlParam);

            if (UploadValidationType == EnumData.eUploadExcelValidationType.eTarget)
            {
                if (TemplateType == (EnumData.eTargetTemplateType.eSKUWise))
                {
                    dtUploadSchema.DefaultView.RowFilter = "TableColumnName<>'BrandCode'";
                    dtUploadSchema = dtUploadSchema.DefaultView.ToTable();
                }

                else
                {
                    dtUploadSchema.DefaultView.RowFilter = "TableColumnName<>'SKUCode'";
                    dtUploadSchema = dtUploadSchema.DefaultView.ToTable();
                }
            }
            CreateSchema(ref dtUploadSchema);
            return dtUploadSchema;
        }
        /*#CC07 Added End*/
        public DataTable getSchemaForUploadSecondarySales(DataSet Ds)
        {
            SqlParam = new SqlParameter[2];
            SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesCHannelID);
            SqlParam[1] = new SqlParameter("@BrandID", intBrandID);
            dtUploadSchema = DataAccess.Instance.GetTableFromDatabase("prcGetSchemaForSecondaryUpload", CommandType.StoredProcedure, SqlParam);
            return dtUploadSchema;


        }
        private void CreateSchema(ref DataTable dt)
        {
            if (dt.Rows.Count == 0)
            {
                throw new NullReferenceException("Schema not defined for table " + strTableName);
            }

            DataTable dtNewSchema = new DataTable(dt.Rows[0]["tableName"].ToString());
            DataColumn dtCol;
            ArrayList PrimaryCol = new ArrayList();
            Int16 iPrimaryCount = 0;
            foreach (DataRow drow in dt.Rows)
            {
                dtCol = new DataColumn(drow["ExcelSheetColumnName"].ToString(), System.Type.GetType(drow["ExcelSheetDataType"].ToString()));
                
                    try
                    {
                        //dtCol.AllowDBNull = !Convert.ToBoolean(drow["Validate"].ToString());
                        dtCol.AllowDBNull = !Convert.ToBoolean(drow["Validate"]);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
               
                
                //dtCol.ExtendedProperties.Add("Validate", Convert.ToBoolean(drow["Validate"].ToString()));
                dtCol.ExtendedProperties.Add("Validate", Convert.ToBoolean(drow["Validate"]));

                if (Convert.ToInt32(drow["MinLength"]) != 0)
                {
                    dtCol.ExtendedProperties.Add("MinLength", drow["MinLength"].ToString());
                }
                if (Convert.ToInt32(drow["MaxLength"].ToString()) > 0)
                {
                    dtCol.ExtendedProperties.Add("MaxLength", Convert.ToInt32(drow["MaxLength"].ToString()));
                }

                dtCol.ExtendedProperties.Add("ColumnConstraint", drow["ColumnConstraint"].ToString());



                if (drow["ColumnConstraint"].ToString().ToLower() == "primary")
                {
                    PrimaryCol.Add(dtCol.ColumnName);
                    dtCol.ExtendedProperties.Add("TableColumnName", drow["TableColumnName"].ToString());
                    dtCol.ExtendedProperties.Add("TableColumnDataType", drow["TableColumnDataType"].ToString());
                    iPrimaryCount++;
                }
                //else
                //{

                //}
                dtNewSchema.Columns.Add(dtCol);
            }


            DataColumn[] dtc = new DataColumn[PrimaryCol.Count];
            if (PrimaryCol.Count > 0)/*#CC03*/
            {
                for (int i = 0; i < PrimaryCol.Count; i++)
                {
                    if (PrimaryCol[i] != null)
                    {
                        dtc[i] = dtNewSchema.Columns[PrimaryCol[i].ToString()];
                    }
                }

                UniqueConstraint dcolUnique = new UniqueConstraint(dtc);
                dtNewSchema.Constraints.Add("PkKey", dtc, false);
                PrimaryCol = null;

                dt = null;
                dt = dtNewSchema;
            }
            dt = dtNewSchema;/*#CC03*/
        }

        
        #region GRN


        public DataTable GettvpTableGRNUpload()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("WareHouseCode");
            Detail.Columns.Add("GRNNumber");
            Detail.Columns.Add("GRNDate");
            Detail.Columns.Add("PONumber");
            Detail.Columns.Add("PODate");
            Detail.Columns.Add("InvoiceNumber");
            Detail.Columns.Add("InvoiceDate");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("SalesFromID");


            return Detail;
        }





        public DataTable GettvpTableSalesManUpload()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("SalesChannelCode");
            Detail.Columns.Add("SalesManName");
            Detail.Columns.Add("SalesManCode");
            Detail.Columns.Add("Address");
            Detail.Columns.Add("MobileNumber");


            return Detail;
        }

        public DataTable GettvpTableColorUpload()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("ColorName");
            Detail.Columns.Add("Active");
            return Detail;
        }

        public DataTable GettvpTableDistrictUpload()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("DistrictName");
            Detail.Columns.Add("DistrictCode");
            Detail.Columns.Add("StateName");
            Detail.Columns.Add("Active");
            return Detail;
        }

        public DataTable GettvpRetailerMap()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("RetailerID");
            return Detail;
        }




        #endregion




        #region Get Tvp Structure For PrimarySales1
        public DataTable GettvpTablePrimarySalesBasePOC()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("SalesChannelCode");
            Detail.Columns.Add("InvoiceNumber");
            Detail.Columns.Add("InvoiceDate");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("SalesFromID");
            Detail.Columns.Add("DocketNumber");
            Detail.Columns.Add("DocketDate");
            Detail.Columns.Add("CarrierName");
            Detail.Columns.Add("ItemPrice");
            Detail.Columns.Add("TotalInvoiceValue");
            Detail.Columns.Add("OrderNumber");
            return Detail;
        }

        public DataTable GettvpTablePrimarySales1()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("SalesChannelCode");
            Detail.Columns.Add("InvoiceNumber");
            Detail.Columns.Add("InvoiceDate");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("SalesFromID");
            return Detail;
        }
        public DataTable GettvpTablePrimarySalesPOC()       //Pankaj Dhingra For POC
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("SalesChannelCode");
            Detail.Columns.Add("InvoiceNumber");
            Detail.Columns.Add("InvoiceDate");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("SalesFromID");
            Detail.Columns.Add("NetAmountWithoutED");
            Detail.Columns.Add("TotalValue");
            return Detail;
        }

        public DataTable GettvpTablePrimarySalesNew()   //Pankaj Dhingra
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("SalesChannelCode");
            Detail.Columns.Add("InvoiceNumber");
            Detail.Columns.Add("InvoiceDate");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("SalesFromID");
            Detail.Columns.Add("WarehouseSalesChannelCode");
            Detail.Columns.Add("OrderNumber");
            return Detail;
        }
        public DataTable GettvpTableForIMEI()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("InvoiceNo");
            Detail.Columns.Add("PartNo");
            Detail.Columns.Add("Description");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("IMEINo");
            Detail.Columns.Add("IMEINo1");
            Detail.Columns.Add("IMEINo2");
            return Detail;
        }
        #endregion
        #region Get Tvp Structure For PrimarySales2
        public DataTable GettvpTablePrimarySales2()
        {
            DataTable Detail = new DataTable();

            Detail.Columns.Add("IntermediarySalesID");
            Detail.Columns.Add("SalesChannelCode");
            Detail.Columns.Add("InvoiceNumber");
            Detail.Columns.Add("InvoiceDate");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("Salesman");
            Detail.Columns.Add("SalesFromID");
            Detail.Columns.Add("IntermediaryDetailID");
            return Detail;
        }
        public DataTable GettvpTablePrimarySales2WitoutOrder()
        {
            DataTable Detail = new DataTable();

            Detail.Columns.Add("IntermediarySalesID");
            Detail.Columns.Add("SalesChannelCode");
            Detail.Columns.Add("InvoiceNumber");
            Detail.Columns.Add("InvoiceDate");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("Salesman");
            Detail.Columns.Add("SalesFromID");

            return Detail;
        }
        public DataTable GettvpTablePrimarySales2UploadWithoutOrder()
        {
            DataTable Detail = new DataTable();

            Detail.Columns.Add("IntermediarySalesID");
            Detail.Columns.Add("SalesChannelCode");
            Detail.Columns.Add("InvoiceNumber");
            Detail.Columns.Add("InvoiceDate");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("Salesman");
            Detail.Columns.Add("SalesFromID");

            return Detail;
        }
        public DataTable GettvpTablePrimarySales2Upload()
        {
            DataTable Detail = new DataTable();

            Detail.Columns.Add("IntermediarySalesID");
            Detail.Columns.Add("SalesChannelCode");
            Detail.Columns.Add("InvoiceNumber");
            Detail.Columns.Add("InvoiceDate");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("Salesman");
            Detail.Columns.Add("SalesFromID");
            Detail.Columns.Add("OrderNumber");
            return Detail;
        }
        public DataTable GettvpTablePrimarySalesNew1()
        {
            DataTable Detail = new DataTable();

            Detail.Columns.Add("PrimarySalesID");
            Detail.Columns.Add("SalesChannelCode");
            Detail.Columns.Add("InvoiceNumber");
            Detail.Columns.Add("InvoiceDate");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("Salesman");
            Detail.Columns.Add("SalesFromID");
            Detail.Columns.Add("FromSalesChannelCode");
            Detail.Columns.Add("PrimaryOrderDetailID");
            return Detail;

        }
        public DataTable GettvpTablePrimarySalesNew1WithoutOrder()
        {
            DataTable Detail = new DataTable();

            Detail.Columns.Add("PrimarySalesID");
            Detail.Columns.Add("SalesChannelCode");
            Detail.Columns.Add("InvoiceNumber");
            Detail.Columns.Add("InvoiceDate");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("Salesman");
            Detail.Columns.Add("SalesFromID");
            Detail.Columns.Add("FromSalesChannelCode");

            return Detail;
        }
        public DataTable GettvpTablePrimarySalesNewWitoutOrder()   //Pankaj Dhingra
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("SalesChannelCode");
            Detail.Columns.Add("InvoiceNumber");
            Detail.Columns.Add("InvoiceDate");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("SalesFromID");
            Detail.Columns.Add("WarehouseSalesChannelCode");

            return Detail;
        }
        public DataTable GetTVPTableStockTransfer()
        {
            DataTable Detail = new DataTable();

            Detail.Columns.Add("FromsaleschannelID");
            Detail.Columns.Add("ToSalesChannelID");
            Detail.Columns.Add("STNNumber");
            Detail.Columns.Add("DocketNumber");
            Detail.Columns.Add("TransferDate");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("SKUID");

            return Detail;
        }
        public DataTable GetTVPTableStockTransferV1()
        {
            DataTable Detail = new DataTable();

            Detail.Columns.Add("FromsaleschannelID");
            Detail.Columns.Add("ToSalesChannelID");
            Detail.Columns.Add("STNNumber");
            Detail.Columns.Add("DocketNumber");
            Detail.Columns.Add("TransferDate");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("SerialNo");
            Detail.Columns.Add("BatchCode");

            return Detail;
        }


        #endregion


        /*This Tvp is made for uploading the voucher of Made for Poc*/
        //Pankaj Dhingra
        public DataTable GettvpUploadVoucher()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("VoucherDate");
            Detail.Columns.Add("VoucherType");
            Detail.Columns.Add("DocNo");
            Detail.Columns.Add("PartyCode");
            Detail.Columns.Add("Amount");
            Detail.Columns.Add("Narration");
            Detail.Columns.Add("VoucherNumber");
            Detail.Columns.Add("SalesChannelID");
            Detail.Columns.Add("LoggedInUserID");
            return Detail;
        }
        /*End of TVP made*/
        #region Get Tvp Structure For Secondary sales
        public DataTable GettvpTableSecondarySales()
        {
            DataTable Detail = new DataTable();

            Detail.Columns.Add("SecondarySalesID");
            Detail.Columns.Add("SalesChannelCode");
            Detail.Columns.Add("InvoiceDate");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("Salesman");
            Detail.Columns.Add("SalesFromID");
            Detail.Columns.Add("SalesmanID");
            return Detail;
        }
        public DataTable GettvpTableSecondarySalesReturn()
        {
            DataTable Detail = new DataTable();


            Detail.Columns.Add("RetailerCode");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");

            Detail.Columns.Add("SalesReturnToID");
            Detail.Columns.Add("Salesman");
            Detail.Columns.Add("SalesReturnDate");
            Detail.Columns.Add("SalesmanID");
            return Detail;
        }
        #endregion
        #region Get Tvp Structure For Primary sales return
        public DataTable GettvpTablePrimarySalesReturn()
        {
            DataTable Detail = new DataTable();

            Detail.Columns.Add("PrimarySalesReturnID");
            Detail.Columns.Add("SalesChannelCode");
            Detail.Columns.Add("InvoiceNumber");
            Detail.Columns.Add("InvoiceDate");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("ReturnToID");
            Detail.Columns.Add("ReturnDate");
            return Detail;
        }
        public DataTable GettvpTablePrimarySalesReturnNew()         //Pankaj Dhingra(27-06-2011)
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("PrimarySalesReturnID");
            Detail.Columns.Add("SalesChannelCode");
            Detail.Columns.Add("InvoiceNumber");
            Detail.Columns.Add("InvoiceDate");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("ReturnToID");
            Detail.Columns.Add("ReturnDate");
            Detail.Columns.Add("FromSalesChannelCode");

            return Detail;
        }
        #endregion
        #region Get Tvp Structure For Stock Transfer
        public DataTable GettvpTableStockTransfer()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("FromSalesChannelID");
            Detail.Columns.Add("ToSalesChannelID");
            Detail.Columns.Add("STNNumber");
            Detail.Columns.Add("DocketNumber");
            Detail.Columns.Add("TransferDate");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("SKUID");
            return Detail;
        }
        #endregion

        #region Get Tvp Structure For Stock Adjustment
        public DataTable GettvpTableStockAdjustment()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("SalesChannelID");
            Detail.Columns.Add("AdjustmentDate");
            Detail.Columns.Add("SKUID");
            Detail.Columns.Add("Quantity");

            return Detail;
        }

        public DataTable GettvpOrderInformation()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("OrderFromSalesChannelID");
            Detail.Columns.Add("ApprovedQuantity");
            Detail.Columns.Add("SKUID");
            Detail.Columns.Add("OrderID");

            return Detail;
        }

        #endregion

        //added for orchid
        public DataTable GettvpTableOrder()
        {
            DataTable dtPrimaryOrder = new DataTable();
            dtPrimaryOrder.Columns.Add("PrimaryOrderID");
            dtPrimaryOrder.Columns.Add("OrderNumber");
            dtPrimaryOrder.Columns.Add("OrderDate");
            dtPrimaryOrder.Columns.Add("FromSalesChannelID");
            dtPrimaryOrder.Columns.Add("ToSalesChannelCode");
            dtPrimaryOrder.Columns.Add("SKUCode");
            dtPrimaryOrder.Columns.Add("Quantity");
            dtPrimaryOrder.Columns.Add("Amount");
            return dtPrimaryOrder;
        }

        public DataTable GettvpTablePrimarySales()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("PrimarySalesID");
            Detail.Columns.Add("SalesChannelID");
            Detail.Columns.Add("InvoiceNumber");
            Detail.Columns.Add("InvoiceDate");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("SalesFromID");
            Detail.Columns.Add("PrimaryOrderDetailID");
            //Detail.Columns.Add("OrderedQuantity");
            return Detail;
        }
        public DataTable GettvpTableUploadTarget()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("TargetFor");
            Detail.Columns.Add("BaseCode");
            Detail.Columns.Add("Target");
            Detail.Columns.Add("TargetType");
            Detail.Columns.Add("TargetName");
            Detail.Columns.Add("FinacialCalenderID");
            Detail.Columns.Add("TargetCategory");
            Detail.Columns.Add("TargetUserTypeID");
            Detail.Columns.Add("TargetUserType");
            Detail.Columns.Add("TargetBasedOn");

            return Detail;
        }
        public DataTable GettvpTableUploadScheme()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("BaseCode");
            Detail.Columns.Add("MinSlab");
            Detail.Columns.Add("MaxSlab");
            Detail.Columns.Add("RewardedQuantity");
            return Detail;
        }
        public DataTable GettvpTablePrimarySalesBatch()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("SalesChannelCode");
            Detail.Columns.Add("InvoiceNumber");
            Detail.Columns.Add("InvoiceDate");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("SalesFromID");
            Detail.Columns.Add("WarehouseSalesChannelCode");
            Detail.Columns.Add("BatchNumber");
            Detail.Columns.Add("OrderNumber");
            return Detail;
        }
        public DataTable GettvpTableOrderBatch()
        {
            DataTable dtPrimaryOrder = new DataTable();
            dtPrimaryOrder.Columns.Add("PrimaryOrderID");
            dtPrimaryOrder.Columns.Add("OrderNumber");
            dtPrimaryOrder.Columns.Add("OrderDate");
            dtPrimaryOrder.Columns.Add("FromSalesChannelID");
            dtPrimaryOrder.Columns.Add("ToSalesChannelCode");
            dtPrimaryOrder.Columns.Add("SKUCode");
            dtPrimaryOrder.Columns.Add("Quantity");
            dtPrimaryOrder.Columns.Add("Amount");
            return dtPrimaryOrder;
        }
        public DataTable GettvpTableFinacialCalender()
        {
            DataTable dtCalender = new DataTable();
            DataColumn ID = new DataColumn("Id", typeof(System.Int32));
            ID.AutoIncrement = true;
            ID.AutoIncrementSeed = 1;
            dtCalender.Columns.Add(ID);
            dtCalender.Columns.Add("Fortnight");
            dtCalender.Columns.Add("StartDate");
            dtCalender.Columns.Add("Quarter");
            return dtCalender;
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

        ~CommonData()
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

        public DataTable GettvpTableOpeningStockSB()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("Serial#1");
            Detail.Columns.Add("Serial#2");
            Detail.Columns.Add("Serial#3");
            Detail.Columns.Add("Serial#4");
            Detail.Columns.Add("BatchNo");
            Detail.Columns.Add("StockBinType");/* #CC02 Added */
            return Detail;
        }
        public DataTable GettvpTableGRNSB()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("WareHouseCode");
            Detail.Columns.Add("GRNNumber");
            Detail.Columns.Add("GRNDate");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("Serial#1");
            Detail.Columns.Add("Serial#2");
            Detail.Columns.Add("Serial#3");
            Detail.Columns.Add("Serial#4");
            Detail.Columns.Add("BatchNo");
            Detail.Columns.Add("StockBinType");/*#CC01 ADDED*/

            return Detail;
        }

        public DataSet GetUploadFileSchema(string ExcelName)
        {
            DataSet dsNew = new DataSet();
            SqlParameter[] objSqlParam = new SqlParameter[3];
            objSqlParam[0] = new SqlParameter("@ExcelName", ExcelName);
            objSqlParam[1] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new SqlParameter("@CompanyId", CompanyId);
             dsNew = DataAccess.Instance.GetDataSetFromDatabase("prcUploadFileSchema_Select", CommandType.StoredProcedure, objSqlParam);
            //Error = Convert.ToString(objSqlParam[1].Value);
            //if (Error != string.Empty)
            //{
            //    throw new ArgumentException(Error);
            //}
            return dsNew;
        }
        /*#CC07 Added*/
        public DataSet GetUploadFileSchemaWithCompanyId(string ExcelName, Int32 CompanyId)
        {
            DataSet dsNew = new DataSet();
            MySqlParameter[] objSqlParam = new MySqlParameter[4];/*#CC07 Added 2 to 3*/
            objSqlParam[0] = new MySqlParameter("@p_ExcelName", ExcelName);
            objSqlParam[1] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 500);
            objSqlParam[1].Direction = ParameterDirection.Output;
            objSqlParam[2] = new MySqlParameter("@p_CompanyId", CompanyId);/*#CC07 Added*/
            objSqlParam[3] = new MySqlParameter("@p_DebugMode", 0);/*#CC07 Added*/

            dsNew = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcUploadFileSchema_SelectWithCompanyId", CommandType.StoredProcedure, objSqlParam);
            //Error = Convert.ToString(objSqlParam[1].Value);
            //if (Error != string.Empty)
            //{
            //    throw new ArgumentException(Error);
            //}
            return dsNew;
        }
        /*#CC07 Added End*/
        public DataTable GettvpTablePrimarySales1SB()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("WareHouseCode");
            Detail.Columns.Add("SalesChannelCode");
            Detail.Columns.Add("OrderNumber");
            Detail.Columns.Add("InvoiceNumber");
            Detail.Columns.Add("InvoiceDate");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("Serial#1");
            Detail.Columns.Add("Serial#2");
            Detail.Columns.Add("Serial#3");
            Detail.Columns.Add("Serial#4");
            Detail.Columns.Add("BatchNo");
            Detail.Columns.Add("BinCode");
            return Detail;
        }
        public DataTable GettvpTablePrimarySalesReturn1SB() //Not in use
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("ReturnFromSalesChannelCode");
            Detail.Columns.Add("ReturnToSalesChannelCode");
            Detail.Columns.Add("InvoiceNumber");
            Detail.Columns.Add("InvoiceDate");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("Serial#1");
            Detail.Columns.Add("Serial#2");
            Detail.Columns.Add("Serial#3");
            Detail.Columns.Add("Serial#4");
            Detail.Columns.Add("BatchNo");
            return Detail;
        }
        public DataTable GettvpTableIntermediarySalesSB()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("FromSalesChannelCode");
            Detail.Columns.Add("ToSalesChannelCode");
            Detail.Columns.Add("OrderNumber");
            Detail.Columns.Add("InvoiceNumber");
            Detail.Columns.Add("InvoiceDate");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("Serial#1");
            Detail.Columns.Add("Serial#2");
            Detail.Columns.Add("Serial#3");
            Detail.Columns.Add("Serial#4");
            Detail.Columns.Add("BatchNo");
            Detail.Columns.Add("Rate");
            Detail.Columns.Add("Amount");
            return Detail;
        }
        /*#CC06 Added*/
        public DataTable GettvpTableTertiarySalesSB()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("RetailerCode");
            Detail.Columns.Add("InvoiceDate");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("Serial#1");
            Detail.Columns.Add("Serial#2");
            Detail.Columns.Add("Serial#3");
            Detail.Columns.Add("Serial#4");
            Detail.Columns.Add("BatchNo");
            Detail.Columns.Add("BinCode");
            Detail.Columns.Add("CustomerName");
            Detail.Columns.Add("MobileNo");
            return Detail;
        }
        /*#CC06 End*/
        //public DataTable GettvpTableActivation()
        //{
        //    DataTable Detail = new DataTable();
        //    Detail.Columns.Add("MobileNo");
        //    Detail.Columns.Add("Serial#1");
        //    Detail.Columns.Add("Serial#2");
        //    Detail.Columns.Add("Serial#3");
        //    Detail.Columns.Add("Serial#4");
        //    Detail.Columns.Add("ActivationDate");
        //    Detail.Columns.Add("ModelName");
        //    Detail.Columns.Add("Operator");
        //    Detail.Columns.Add("Circle");
        //    //Detail.Columns.Add("TransType");
        //    //Detail.Columns.Add("TransUploadSession");
        //    return Detail;
        //}
        public DataTable GettvpTableReturnSalesSB()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("ReturnFromSalesChannelCode");
            Detail.Columns.Add("ReturnToSalesChannelCode");
            Detail.Columns.Add("InvoiceNumber");
            Detail.Columns.Add("InvoiceDate");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("Serial#1");
            Detail.Columns.Add("Serial#2");
            Detail.Columns.Add("Serial#3");
            Detail.Columns.Add("Serial#4");
            Detail.Columns.Add("BatchNo");
            Detail.Columns.Add("BinCode");/*#CC01 ADDED*/
            return Detail;
        }

        public DataTable GettvpISPSalary()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("ISPCode");
            Detail.Columns.Add("ISPID");
            Detail.Columns.Add("SalaryAmt");
            Detail.Columns.Add("TransUploadSession");
            return Detail;
        }


        public DataTable GettvpInvoiceCancellationSB()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("FromSalesChannelCode");
            Detail.Columns.Add("InvoiceNumber");
            Detail.Columns.Add("ToSalesChannelCode");
            return Detail;
        }
        //#CC04 Start 
        /// <summary>
        /// Created By: Rakesh Raj
        /// Created On: 9/6/2018
        /// </summary> Used for Uploading Master Excel Sheet Data like Color Master Upload, 
        /// District Master Upload etc.
        /// <param name="Dt"></param>
        /// <returns></returns>

        public DataSet BulkUploadUsingTVP(DataTable Dt, string strStoredProcedureName, string tvpName)
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter(tvpName, SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;

                dsUploadschema = DataAccess.Instance.GetDataSetFromDatabase(strStoredProcedureName, CommandType.StoredProcedure, SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[1].Value);
                if (SqlParam[3].Value.ToString() != "")
                {
                    ErrorDetailXML = SqlParam[3].Value.ToString();
                }
                else
                {
                    ErrorDetailXML = null;
                }
                if (SqlParam[2].Value.ToString() != "")
                {
                    Error = Convert.ToString(SqlParam[2].Value);
                }

                return dsUploadschema;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Create By: Rakesh Raj
        /// Created On: 11/June/2018
        /// Used to Bulk Add/Update Excel Sheet Using BCP
        /// </summary>
        /// <param name="strStoredProcedureName"></param>
        /// <returns></returns>
        public DataSet BulkUploadExcel(string strStoredProcedureName)
        {
            try
            {

                SqlParam = new SqlParameter[3]; 
                SqlParam[0] = new SqlParameter("@SessionID", SessionID);
                SqlParam[1] = new SqlParameter("@OutParam", SqlDbType.Int);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@OutError", SqlDbType.NVarChar, 200);
                SqlParam[2].Direction = ParameterDirection.Output;
                dsUploadschema = DataAccess.Instance.GetDataSetFromDatabase(strStoredProcedureName, CommandType.StoredProcedure, SqlParam);
                intOutParam = Convert.ToInt16(SqlParam[1].Value);
                if (Convert.ToString(SqlParam[2].Value) != null && Convert.ToString(SqlParam[2].Value) != "")
                {
                    Error = Convert.ToString(SqlParam[2].Value);
                }
                return dsUploadschema;

            }
            catch (Exception)
            {

                throw;
            }
        }


        /* #CC05 Add Start */

        public DataSet BulkUploadOutStandingAmount()
        {
            try
            {

                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@SessionID", SessionID);
                SqlParam[1] = new SqlParameter("@OutParam", SqlDbType.Int);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@OutError", SqlDbType.NVarChar, 2000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@UserID", UserID);
                dsUploadschema = DataAccess.Instance.GetDataSetFromDatabase("prcBulkUploadOutStandingAmountV2", CommandType.StoredProcedure, SqlParam);
                intOutParam = Convert.ToInt16(SqlParam[1].Value);
                if (Convert.ToString(SqlParam[2].Value) != null && Convert.ToString(SqlParam[2].Value) != "")
                {
                    Error = Convert.ToString(SqlParam[2].Value);
                }
                return dsUploadschema;

            }
            catch (Exception)
            {

                throw;
            }
        } /* #CC05 Add End */


        
     /// <summary>
     ///  Create By: Rakesh Raj
     /// Created On: 12/June/2018
     /// </summary> Used to Populdate Excel Sheet Export based on refType parameter
     /// <param name="refType">1- Area Ref Code, 2- Model Ref Code</param>
     /// <returns></returns>
        public DataSet DownloadRefCodeExcel(Int16 refType)
        {
           try
            {                
                SqlParam = new SqlParameter[2]; /* #CC02 Length increased from 1 to 2 */
                SqlParam[0] = new SqlParameter("@reftype", refType);
                SqlParam[1] = new SqlParameter("@UserID", UserID); /*#CC02 Added */          
               dsUploadschema = DataAccess.Instance.GetDataSetFromDatabase("prcGetExcelRefCode", CommandType.StoredProcedure, SqlParam);
               
               return dsUploadschema;
             }
             catch (Exception ex)
             {
                 throw ex;
             }

        }


        //#CC04 End
        
    }
}
