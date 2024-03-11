using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using Microsoft.ApplicationBlocks.Data;

namespace DataAccess
{
    public class POC : IDisposable
    {
        SqlParameter[] SqlParam;
        DataTable d1;
        DataSet ds;
        private Int32 intSalesChannelID;
        public string error;
        Int32 IntResultCount = 0, intOfferID, intOfferBasedOnDetailID;
        private string strErrorDetailXML, _xmlList;
        public string MyProperty1 { get; set; }
        public string GenServiceDocNo
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

        public int SalesFromId
        {
            get;
            set;
        }

        public int SalesToId
        {
            get;
            set;
        }

        public string InvoiceNumber
        {
            get;
            set;
        }

        public string InvoiceDate
        {
            get;
            set;
        }

        public string ErrorDetailXML
        {
            get { return strErrorDetailXML; }
            set { strErrorDetailXML = value; }
        }

        public int SKUID
        {
            get;
            set;
        }

        public Boolean Status
        {
            get;
            set;
        }
        public string BatchName
        {
            get;
            set;
        }
        public string BatchCode
        {
            get;
            set;
        }
        public string BatchStartDate
        {
            get;
            set;
        }
        public string BatchEndDate
        {
            get;
            set;
        }
        public int BatchID
        {
            get;
            set;
        }

        public Int16 BatchselectionMode
        {
            get;
            set;
        }

        public string Error
        {
            get;
            set;
        }
        public string XMLList
        {
            get
            {
                return _xmlList;
            }
            set
            {
                _xmlList = value;
            }
        }
        public Int32 OfferID
        {
            get { return intOfferID; }
            set { intOfferID = value; }
        }
        public Int32 OfferBasedOnDetailID
        {
            get { return intOfferBasedOnDetailID; }
            set { intOfferBasedOnDetailID = value; }
        }

        public Int32 SalesChannelID
        {
            get { return intSalesChannelID; }
            set { intSalesChannelID = value; }
        }
        public String SkuCode { get; set; }

        public int ProductID
        {
            get;
            set;
        }


        public int SalesChannelId
        {
            get;
            set;
        }

        public int UserId
        {
            set;
            get;
        }

        public string VendorName
        {
            get;
            set;
        }


        # region batchwise Product



        public DataTable SelectBatchWiseSKUInfo()
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@saleschannelid", SalesChannelId);
                SqlParam[1] = new SqlParameter("@productid", ProductID);
                d1 = DataAccess.Instance.GetTableFromDatabase("prcGetBatchSKUFromProduct", CommandType.StoredProcedure, SqlParam);
                return d1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public DataTable SelectBatchWiseProductInfo()
        {
            try
            {
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@saleschannelid", SalesChannelId);
                d1 = DataAccess.Instance.GetTableFromDatabase("prcGetBatchwiseProduct", CommandType.StoredProcedure, SqlParam);
                return d1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectBatchInfoFromSKU()
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@saleschannelid", SalesChannelId);
                SqlParam[1] = new SqlParameter("@skuid", SKUID);
                d1 = DataAccess.Instance.GetTableFromDatabase("prcGetBatchInfofromSKU", CommandType.StoredProcedure, SqlParam);
                return d1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        # endregion



        # region Btachwisesecondrysale


        public Int32 InsertBatchwiseSecondarySales(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[8];
                SqlParam[0] = new SqlParameter("@tvpsecondrysales", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@salesfromid", SalesFromId);
                SqlParam[2] = new SqlParameter("@invoicenumber", InvoiceNumber);
                SqlParam[3] = new SqlParameter("@invoicedate", InvoiceDate);
                SqlParam[4] = new SqlParameter("@salestoid", SalesToId);
                SqlParam[5] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[5].Direction = ParameterDirection.Output;
                SqlParam[6] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[6].Direction = ParameterDirection.Output;
                SqlParam[7] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[7].Direction = ParameterDirection.Output;
                DataAccess.Instance.DBInsertCommand("prcInsertBatchWiseSecondrySales", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[1].Value);
                if (SqlParam[7].Value.ToString() != "")
                {
                    ErrorDetailXML = SqlParam[7].Value.ToString();
                }
                else
                {
                    ErrorDetailXML = null;
                }
                Error = Convert.ToString(SqlParam[6].Value);

                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        # endregion

        public DataTable GettvpTableMaterialIssueBatchOrSerialWise()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("WareHouseID");
            Detail.Columns.Add("DocketNo");
            Detail.Columns.Add("DocketDate");
            Detail.Columns.Add("IssueToUserID");
            Detail.Columns.Add("Remarks");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("ModeofReceipt");
            Detail.Columns.Add("CourierName");
            Detail.Columns.Add("SkuCode");
            Detail.Columns.Add("SerialNumber");
            Detail.Columns.Add("BatchNumber");
            Detail.Columns.Add("LoggedInID");       //SalesChannelID
           

            return Detail;
        }
        public Int32 InsertInfoMaterialIssueBatchWiseOrSerialWise(DataTable Dt)     //Pankaj Dhingra
        {
            try
            {
                ////Changes to be done here  PKD
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@GRNBatchOrSerial", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 5000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                DataAccess.Instance.DBInsertCommand("PrcInsertInfoMaterialIssue", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[1].Value);
                if (SqlParam[3].Value.ToString() != "" && SqlParam[3].Value.ToString() != null )
                {
                    ErrorDetailXML = SqlParam[3].Value.ToString();
                }
                else
                {
                    ErrorDetailXML = null;
                }
                Error = Convert.ToString(SqlParam[2].Value);

                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #region GRNBatchWise
        public DataTable GettvpTableGRNUploadBatchWise()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("WareHouseID");
            Detail.Columns.Add("GRNNumber");
            Detail.Columns.Add("GRNDate");
            Detail.Columns.Add("InvoiceNumber");
            Detail.Columns.Add("InvoiceDate");
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("BatchNumber");
            Detail.Columns.Add("PONumber");
            Detail.Columns.Add("PODate");
            Detail.Columns.Add("Remarks");
            Detail.Columns.Add("LoggedInID");
            Detail.Columns.Add("SerialNumber");
            //SalesChannelID


            return Detail;
        }
        public Int32 InsertInfoGRNUploadBatchWise(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@GRNUploadBatchWise", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 5000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@vendorname",VendorName);
                DataAccess.Instance.DBInsertCommand("[PrcInsertInfoGRNUploadBatchOrSerialWise]", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[1].Value);
                if (SqlParam[3].Value.ToString() != "")
                {
                    ErrorDetailXML = SqlParam[3].Value.ToString();
                }
                else
                {
                    ErrorDetailXML = null;
                }
                Error = Convert.ToString(SqlParam[2].Value);

                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        #endregion

        #region Get Tvp Structure For Opening Stock Batch wise entry

        public DataTable GettvpTableCommomForStockBatchEntry()
        {
            DataTable Detail = new DataTable();
            Detail.Columns.Add("SKUCode");
            Detail.Columns.Add("Quantity");
            Detail.Columns.Add("BatchNumber");

            return Detail;
        }
        public void InsertOpeningStockBatch(DataTable Tvp)
        {
            SqlParam = new SqlParameter[4];
            SqlParam[0] = new SqlParameter("@SalesChannelId", intSalesChannelID);
            SqlParam[1] = new SqlParameter("@tvpBatchStockEntry", SqlDbType.Structured);
            SqlParam[1].Value = Tvp;
            SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[2].Direction = ParameterDirection.Output;
            SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
            SqlParam[3].Direction = ParameterDirection.Output;
            DataAccess.Instance.DBInsertCommand("prcInsBatchOpeningStock", SqlParam);
            if (SqlParam[3].Value.ToString() != "")
            {
                XMLList = SqlParam[3].Value.ToString();
            }
            else
            {
                XMLList = null;
            }
            Error = Convert.ToString(SqlParam[2].Value);
        }
        #endregion

        # region Batch Master
        public DataTable SelectBatchInfo()
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@batchid", BatchID);
                SqlParam[1] = new SqlParameter("@batchname", BatchName);
                SqlParam[2] = new SqlParameter("@batchcode", BatchCode);
                SqlParam[3] = new SqlParameter("@batchstartdate", BatchStartDate);
                SqlParam[4] = new SqlParameter("@batchenddate", BatchEndDate);
                SqlParam[5] = new SqlParameter("@selectionmode", BatchselectionMode);
                d1 = DataAccess.Instance.GetTableFromDatabase("prcGetBatchDetails", CommandType.StoredProcedure, SqlParam);


                return d1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsUpdBatchInfo()
        {
            try
            {
                SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@batchid", BatchID);
                SqlParam[1] = new SqlParameter("@batchname", BatchName);
                SqlParam[2] = new SqlParameter("@batchcode", BatchCode);
                SqlParam[3] = new SqlParameter("@batchstartdate", BatchStartDate);
                SqlParam[4] = new SqlParameter("@batchenddate", BatchEndDate);
                SqlParam[5] = new SqlParameter("@status", Status);
                SqlParam[6] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 200);
                SqlParam[6].Direction = ParameterDirection.Output;
                int r = DataAccess.Instance.DBInsertCommand("prcInsUpdBatch", SqlParam);
                error = Convert.ToString(SqlParam[6].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion


        # region batch Reports

        public DataTable GetBatchStockDetailInfo()
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@BatchDateFrom", DateFrom);
                SqlParam[1] = new SqlParameter("@BatchDateTo", DateTo);
                SqlParam[2] = new SqlParameter("@BatchNumber", BatchCode);
                SqlParam[3] = new SqlParameter("@SkuCode", SkuCode);
                SqlParam[4] = new SqlParameter("@SaleschannelID", SalesChannelId);
                SqlParam[5] = new SqlParameter("@UserId", UserId);
                d1 = DataAccess.Instance.GetTableFromDatabase("prcGetBatchStockDetailInfo", CommandType.StoredProcedure, SqlParam);
                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetBatchWiseStockReport()
        {
            try
            {
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@Datefrom", DateFrom);
                DataSet d2 = DataAccess.Instance.GetDataSetFromDatabase("PrcGetBatchWiseStockRpt", CommandType.StoredProcedure, SqlParam);
                return d2;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #endregion

        # region Onida Sap Service
        public DataSet GetUpdateSelectRawData(String strConnectionString, int value, int FromErrorOrNot)    //For onida sap integration
        {
            try
            {
                ds = new DataSet();
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@Value", value);
                SqlParam[1] = new SqlParameter("@FromErrorOrNot", FromErrorOrNot);
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@GenServiceDocNo", SqlDbType.NVarChar, 25); //Pankaj dhingra
                SqlParam[4].Direction = ParameterDirection.Output;
                ds = SqlHelper.ExecuteDataset(strConnectionString, CommandType.StoredProcedure, "prcGetUpdateSelectRawData", SqlParam);
                //IntResultCount = Convert.ToInt16(SqlParam[1].Value);
                if (SqlParam[4].Value.ToString() != "")
                    GenServiceDocNo = SqlParam[4].Value.ToString();
                else
                    GenServiceDocNo = null;
                             
                if (SqlParam[3].Value.ToString() != "")
                {
                    ErrorDetailXML = SqlParam[3].Value.ToString();
                }
                else
                {
                    ErrorDetailXML = null;
                }
                Error = Convert.ToString(SqlParam[2].Value);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetDetailwiseDataForSap(Int32 ModuleID)    //For onida sap integration
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@ServiceDocNo",GenServiceDocNo);
                SqlParam[1] = new SqlParameter("@ModuleID", ModuleID);
                dt = DataAccess.Instance.GetTableFromDatabase("prcGetDetailwiseDataForSap", CommandType.StoredProcedure, SqlParam);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        # endregion Onida
        # region POC OfferBased Order
        public DataTable GetOfferBySku()
        {
            try
            {
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@OrderDate", InvoiceDate);
                SqlParam[1] = new SqlParameter("@SKUID", SKUID);
                SqlParam[2] = new SqlParameter("@OfferID", OfferID);
                d1 = DataAccess.Instance.GetTableFromDatabase("prcGetOfferBySku", CommandType.StoredProcedure, SqlParam);
                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetOfferBySkuReward()
        {
            try
            {
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@OrderDate", InvoiceDate);
                SqlParam[1] = new SqlParameter("@SKUID", SKUID);
                SqlParam[2] = new SqlParameter("@OfferBasedOnDetailID", OfferBasedOnDetailID);
                d1 = DataAccess.Instance.GetTableFromDatabase("prcGetOfferBySkuReward", CommandType.StoredProcedure, SqlParam);
                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetSKUPrice()
        {
            try
            {
                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@OrderDate", InvoiceDate);
                SqlParam[1] = new SqlParameter("@SKUID", SKUID);
                d1 = DataAccess.Instance.GetTableFromDatabase("prcGetPriceOfSKU", CommandType.StoredProcedure, SqlParam);
                return d1;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable GetNetOfferAmount()
        {
            try
            {
               
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@OrderDate", InvoiceDate);
                d1 = DataAccess.Instance.GetTableFromDatabase("prcGetNetOfferAmount", CommandType.StoredProcedure, SqlParam);

                return d1;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        # endregion
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

        ~POC()
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
}
