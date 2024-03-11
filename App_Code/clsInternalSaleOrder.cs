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

namespace BussinessLogic
{
    public class clsInternalSaleOrder : IDisposable
    {
        #region Page Variables
        Int16 _intOrderType;
        DataTable _dtFromEntities;
        DataTable _dtToEntities;
        #endregion
        #region Properties
        public String strConStr = ConfigurationManager.ConnectionStrings["AppConString"].ConnectionString;
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

        public Int64 OrderId
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
        #endregion

        # region Functions

        public DataSet SelectPartInfoForOrder()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[12];
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
            }
            return dsResult;

        }

        public void InsertSalesOrderInfo(DataTable Dt)
        {
            try
            {
                Result = 1;
                SqlParameter[] objSqlParam = new SqlParameter[18];
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
                SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcSalesOrder_Insert", objSqlParam);
                /*#CC01: Changed (start)*/
                Error = Convert.ToString(objSqlParam[1].Value);
                if (Convert.ToString(objSqlParam[1].Value) != "" && Convert.ToInt32(objSqlParam[2].Value) > 0)
                {
                    return;
                }
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
                    OrderId = Convert.ToInt32(objSqlParam[13].Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


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

        public DataTable SelectSerializedPartsDetailsForInvoice()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[2];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@allocationId", AllocationId);
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcAllocationDetailForSerializedParts", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[0].Value);


            return dtResult;
        }

        public DataTable ExecuteprocedureforAutomaticAllocation()
        {
            DataTable dtResult = new DataTable();
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcAutoProcessCreditCheckStockAllocation");
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];

            return dtResult;
        }

        public DataTable SelectPackingSlip()
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
            objSqlParam[10] = new SqlParameter("@packingslipnumber", PackingSlipNumber);
            objSqlParam[11] = new SqlParameter("@invoicenumber", InvoiceNumber);
            objSqlParam[12] = new SqlParameter("@loginentityid", EntityId);
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcPackingSlipInformation_Select", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            TotalRecords = Convert.ToInt32(objSqlParam[8].Value);
            Error = Convert.ToString(objSqlParam[0].Value);

            return dtResult;
        }

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
                PackingSlipNumber = Convert.ToString(objSqlParam[4].Value);
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
            SqlParameter[] objSqlParam = new SqlParameter[6];
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

        public void InsertInvoiceDetails(DataTable Dt)
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
                objSqlParam[6] = new SqlParameter("@isSerial", IsSerial);
                objSqlParam[7] = new SqlParameter("@StockUpdateError", SqlDbType.Xml);
                objSqlParam[7].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(strSalesInvoiceXML, XmlNodeType.Document, null));
                objSqlParam[7].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcInvoiceOnPackingSlip", objSqlParam);
                Error = Convert.ToString(objSqlParam[1].Value);
                InvoiceNumber = Convert.ToString(objSqlParam[5].Value);
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
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[5];
            objSqlParam[0] = new SqlParameter("@cancelRemarks", Remarks);
            objSqlParam[1] = new SqlParameter("@allocationid", AllocationId);
            objSqlParam[2] = new SqlParameter("@ModifiedBy", CreatedBy);
            objSqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[4].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcInvoice_Cancel", objSqlParam);
            result = Convert.ToInt16(objSqlParam[3].Value);
            Error = Convert.ToString(objSqlParam[4].Value);
            return result;
        }

        public Int16 InvoiceCancellationApproval()
        {
            Int16 result = 1;
            SqlParameter[] objSqlParam = new SqlParameter[6];
            objSqlParam[0] = new SqlParameter("@cancelRemarks", Remarks);
            objSqlParam[1] = new SqlParameter("@transactionid", TransactionId);
            objSqlParam[2] = new SqlParameter("@ModifiedBy", CreatedBy);
            objSqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[3].Direction = ParameterDirection.Output;
            objSqlParam[4] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[4].Direction = ParameterDirection.Output;
            objSqlParam[5] = new SqlParameter("@status", CancelStatus);
            SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcInvoiceRequest_Cancel", objSqlParam);
            result = Convert.ToInt16(objSqlParam[3].Value);
            Error = Convert.ToString(objSqlParam[4].Value);
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

        public void InsertDispatchInfoForOrder()
        {
            try
            {
                int IntResultCount = 0;
                SqlParameter[] SqlParam = new SqlParameter[16];
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
                SqlParam[10] = new SqlParameter("@DispatchNo", SqlDbType.NVarChar, 14);
                SqlParam[10].Direction = ParameterDirection.Output;
                SqlParam[11] = new SqlParameter("@XMLPartDetailReturn", SqlDbType.Xml);
                SqlParam[11].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(strSalesInvoiceXML, XmlNodeType.Document, null));
                SqlParam[11].Direction = ParameterDirection.Output;
                SqlParam[12] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[12].Direction = ParameterDirection.Output;
                SqlParam[13] = new SqlParameter("@DocketNo", DocketNumber);
                SqlParam[14] = new SqlParameter("@CourierCompanyName", CourierCompanyName);
                SqlParam[15] = new SqlParameter("@allocationid", AllocationId);
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

        public DataTable SelectOrderetailsForPrint()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[2];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@orderid", OrderId);
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "[prcSalesOrder_SelectReport]", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            Error = Convert.ToString(objSqlParam[0].Value);
            return dtResult;
        }

       
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

        public DataTable SelectInternalSaleOrders()
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
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcInternalSaleOrders_Select", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            TotalRecords = Convert.ToInt32(objSqlParam[7].Value);
            Error = Convert.ToString(objSqlParam[0].Value);

            return dtResult;
        }

        public DataSet SelectInternalSaleOrders_ById()
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[7];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@SaleOrderId", OrderId);         
            objSqlParam[2] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[3] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[4] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
            objSqlParam[4].Direction = ParameterDirection.Output;
            objSqlParam[5] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[5].Direction = ParameterDirection.Output;           
            objSqlParam[6] = new SqlParameter("@loginentityid", EntityId);
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcInternalSaleOrders_SelectBYId", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            TotalRecords = Convert.ToInt32(objSqlParam[5].Value);
            Error = Convert.ToString(objSqlParam[0].Value);

            return dsResult;
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
            objSqlParam[3] = new SqlParameter("@FromDate", FromDate);
            objSqlParam[4] = new SqlParameter("@ToDate", ToDate);
            objSqlParam[5] = new SqlParameter("@OrderNumber", OrderNumber);
            objSqlParam[6] = new SqlParameter("@AllocationStatus", OrderAllocationStatus);
            objSqlParam[5] = new SqlParameter("@PageIndex", PageIndex);
            objSqlParam[6] = new SqlParameter("@PageSize", PageSize);
            objSqlParam[8] = new SqlParameter("@TotalRecord", SqlDbType.BigInt, 8);
            objSqlParam[8].Direction = ParameterDirection.Output;
            objSqlParam[9] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
            objSqlParam[9].Direction = ParameterDirection.Output;
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcOrderInformation_SelectForView_Ver2", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            TotalRecords = Convert.ToInt32(objSqlParam[8].Value);
            Error = Convert.ToString(objSqlParam[0].Value);

            return dtResult;
        }
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

        ~clsInternalSaleOrder()
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
                SqlParameter[] objSqlParam = new SqlParameter[18];
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
                SqlHelper.ExecuteNonQuery(strConStr, CommandType.StoredProcedure, "prcBulkPOGeneration_Upload", objSqlParam);
                Error = Convert.ToString(objSqlParam[2].Value);
                OrderNumber = Convert.ToString(objSqlParam[9].Value);
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
        #endregion
    }
}