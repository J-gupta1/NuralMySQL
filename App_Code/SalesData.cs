#region Copyright(c) 2017 Zed-Axis Technologies All rights are reserved
/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2017 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Created By : Sumit Maurya
* Created On: 23-Feb-2017 
 * Description: This is a copy of Salesdata from DataAccess.
* ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 24-Mar-2017, Sumit Maurya, #CC01, New parameter supplied to method to get saleschanneldetail accordingly.
 ====================================================================================================
*/

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using DataAccess;
/*
 * 09-Jan-2014, Sumit Kumar, #CC01, Insert Bulk Excel data for Primary IMEI Ack
 * 30-May-2018,Vijay Kumar Prajapati,#CC02,Add New parameter returntype.
 * 19-Sept-2018,Vijay Kumar Prajapati,#CC03,Add New parameter for Primaryreturn method.
 * 26-Feb-2019, Balram Jha, #CC04- added method for Interface Sale fail record process for Brother
 *  * 01-May-2019,#CC05,Vijay Kumar Prajapati,Added Table for tertiarysale Upload
 *  07-May-2019,#CC06,Vijay Kumar Prajapati,Added method for upload Activation Referencecode.
 * 30-Oct-2019, #CC07, Balram Jha, Methods for Tally patch (interface) failur processing with master generation 
 * 18-Feb-2020, #CC08, Balram Jha, Sales Return Request additon for Motorola 
 */

namespace DataAccess
{
    public class SalesData : IDisposable
    {
        #region Private Properties

        private string strError, strInvoiceNumber, strSKUCode, strErrorDetailXML, strBTMSapDetailXML, strRetailerCode, strSalesChannelCode, strOrderNumber, strFromSalesChannelCode, strReturnToSalesChannelCode;
        private int intSalesChannelID, intSKUId, intRetailerID, intBrand, intUserID, _ReturnType/*#CC02 Added*/;
        private DateTime? dateInvoiceDate, dateReturnDate;
        private EnumData.eControlRequestTypeForEntry eSalesType;
        private EnumData.eEntryType eEntryType;
        private DateTime? dtFromdate, dtToDate;
        #endregion
        #region Public Variables

        DataTable dtResult;
        SqlParameter[] SqlParam;
        Int32 IntResultCount = 0, intDistributorSalesChannelID;
        DataSet dsResult;
        #endregion
        #region Public Properties
        public Int32 CompanyId { get; set; }
        public Int16 SalestypeId { get; set; }
        public Int16 Mastertype { get; set; }
        public string TransUploadSession
        {
            get;
            set;
        }
        public Int32 Brand
        {
            get { return intBrand; }
            set { intBrand = value; }
        }
        public string ErrorDetailXML
        {
            get { return strErrorDetailXML; }
            set { strErrorDetailXML = value; }
        }

        public string TemplateType
        {
            get;
            set;
        }
        public bool CanApprove//#CC08 added
        {
            get;
            set;
        }
        public EnumData.eControlRequestTypeForEntry SalesType
        {
            get { return eSalesType; }
            set { eSalesType = value; }
        }
        public Int16 SalesTypeCancellation
        {
            get;
            set;
        }
        public Int16 StockBinType
        {
            get;
            set;
        }
        public EnumData.eEntryType EntryType
        {
            get { return eEntryType; }
            set { eEntryType = value; }
        }
        public string Error
        {
            get { return strError; }
            set { strError = value; }
        }
        public int SalesChannelID
        {
            get { return intSalesChannelID; }
            set { intSalesChannelID = value; }
        }
        public string InvoiceNumber
        {
            get { return strInvoiceNumber; }
            set { strInvoiceNumber = value; }
        }
        public DateTime? InvoiceDate
        {
            get { return dateInvoiceDate; }
            set { dateInvoiceDate = value; }
        }
        public DateTime? ReturnDate
        {
            get { return dateReturnDate; }
            set { dateReturnDate = value; }
        }

        public DateTime? FromDate
        {
            get { return dtFromdate; }
            set { dtFromdate = value; }
        }
        public DateTime? ToDate
        {
            get;
            set;
        }

        public string SKUCode
        {
            get { return strSKUCode; }
            set { strSKUCode = value; }
        }

        public string SKUName
        {
            get;
            set;
        }
        public int ProductCategoryID
        {
            get;
            set;
        }

        public int ModelId
        {
            get;
            set;
        }

        public int SKUId
        {

            get { return intSKUId; }
            set { intSKUId = value; }
        }
        public int RetailerID
        {
            get { return intRetailerID; }
            set { intRetailerID = value; }
        }
        public string RetailerCode
        {
            get { return strRetailerCode; }
            set { strRetailerCode = value; }
        }
        public string SalesChannelCode
        {
            get { return strSalesChannelCode; }
            set { strSalesChannelCode = value; }
        }
        public string SalesChannelName { get; set; }
        /*#CC02 Added Started*/
        public int  ReturnType
        {
            get { return _ReturnType; }
            set { _ReturnType = value; }
        }
        /*#CC02 Added End*/
        public Int64 SecondarySalesReturnMainID
        {
            get;
            set;
        }
        public Int16 ApproveStatus
        {
            get;
            set;
        }
        //Pankaj Dhingra
        public string FromSalesChannelCode
        {
            get { return strFromSalesChannelCode; }
            set { strFromSalesChannelCode = value; }
        }
        public string ReturnToSalesChannelCode
        {
            get { return strReturnToSalesChannelCode; }
            set { strReturnToSalesChannelCode = value; }
        }
        public string OrderNumber
        {
            get { return strOrderNumber; }
            set { strOrderNumber = value; }
        }
        public Int32 DistributorSalesChannelID
        {
            get { return intDistributorSalesChannelID; }
            set { intDistributorSalesChannelID = value; }
        }
        public string BTMSapDetailXML
        {
            get { return strBTMSapDetailXML; }
            set { strBTMSapDetailXML = value; }
        }
        public int UserID
        {
            get { return intUserID; }
            set { intUserID = value; }
        }

        public int OrderId
        {
            get;
            set;
        }
        public int FlagForTable
        {
            get;
            set;
        }
        public int AckStatus
        {
            get;
            set;
        }
        public int SalesUniqueID
        {
            get;
            set;
        }
        public int value
        {
            get;
            set;
        }
        public int Decider
        {
            get;
            set;
        }
        public int OtherEntity
        {
            get;
            set;
        }
        public Int32 ReturnFromSalesChannelID
        {
            get;
            set;
        }
        public Int32 ComingFrom
        {
            get;
            set;
        }

        public int Status
        {
            get;
            set;
        }
        public string strOriginalFileName { get; set; }/*#CC03 Added*/
        public string strUploadedFileName { get; set; }/*#CC03 Added*/
        private int _PageIndex = 0;
        public int PageIndex
        {
            get
            {
                return _PageIndex;
            }
            set
            {
                _PageIndex = value;
            }
        }
        private int _PageSize = 10;
        public int PageSize
        {
            get
            {
                return _PageSize;
            }
            set
            {
                _PageSize = value;
            }
        }


        //Insert Bulk Excel data for Primary IMEI Ack//

        public DataTable dtIMEI
        {
            get;
            set;
        }
        public string IMEISessionID
        {
            get;
            set;
        }
        public string Remark//#CC08
        {
            get;
            set;
        }
        public int SalesChannelTypeID
        {
            get;
            set;
        }
        public Int32 TotalRecords
        {
            get;
            set;
        }
        public int intOutParam
        {
            get;
            set;
        }
        public string strFromDate
        {
            get;
            set;
        }
        public string strToDate
        {
            get;
            set;
        }
        /* #CC01 Add Start */
        public Int32 BilltoRetailer
        {
            get;
            set;
        }
        /* #CC01 Add End */
        #endregion
        #region UploadSapBtmData
        public void UploadBTMSapData()
        {
            try
            {

                SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@BTMSapDetailXML", SqlDbType.Xml);
                SqlParam[0].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(strBTMSapDetailXML, XmlNodeType.Document, null));
                SqlParam[0].Direction = ParameterDirection.InputOutput;
                SqlParam[1] = new SqlParameter("@ErrorMessage", "");
                SqlParam[1].Direction = ParameterDirection.InputOutput;
                IntResultCount = DataAccess.Instance.DBInsertCommand("", SqlParam);
                if (((System.Data.SqlTypes.SqlXml)SqlParam[0].Value).IsNull != true)
                {
                    strBTMSapDetailXML = ((System.Data.SqlTypes.SqlXml)SqlParam[0].Value).Value;

                }
                else
                {
                    strBTMSapDetailXML = null;
                }
                Error = SqlParam[1].Value.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }



        }
        #endregion
        #region GetOrderDetail
        public DataTable GetOrderDetail()
        {

            try
            {
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesChannelID);
                SqlParam[1] = new SqlParameter("@Brand", intBrand);
                SqlParam[2] = new SqlParameter("@OrderDate", dateInvoiceDate);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetOrderDetail", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion
        #region Insert Order p1/p2



        public void InsertPrimaryOrderInfo(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@PS2Type", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@uniquenumberout", SqlDbType.VarChar, 100);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@userid", UserID);
                SqlParam[4] = new SqlParameter("@orderid", SqlDbType.Int, 5);
                SqlParam[4].Direction = ParameterDirection.Output;
                DataAccess.Instance.DBInsertCommand("PrcInsertPrimaryOrderInfo", SqlParam);
                Error = Convert.ToString(SqlParam[1].Value);
                OrderNumber = Convert.ToString(SqlParam[2].Value);
                OrderId = Convert.ToInt32(SqlParam[4].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsertIntermediaryOrderInfo(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@PS2Type", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@userid", UserID);
                SqlParam[3] = new SqlParameter("@orderno1", SqlDbType.VarChar, 200);
                SqlParam[3].Direction = ParameterDirection.Output;
                DataAccess.Instance.DBInsertCommand("PrcInsertIntermediaryOrderInfo", SqlParam);
                OrderNumber = Convert.ToString(SqlParam[3].Value);
                Error = Convert.ToString(SqlParam[1].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Get Orderwise P1 Sales
        public DataSet GetPrimarySales1Add()
        {

            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesChannelID);
                SqlParam[1] = new SqlParameter("@InvoiceNumber", strInvoiceNumber);
                SqlParam[2] = new SqlParameter("@ToSalesChannelCode", strSalesChannelCode);
                SqlParam[3] = new SqlParameter("@Brand", intBrand);
                SqlParam[3] = new SqlParameter("@FromSalesChannelCode", FromSalesChannelCode);

                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetPrimarySales1DetailAdd", CommandType.StoredProcedure, SqlParam);

                return dsResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public DataSet GetPrimarySales1Edit()
        {

            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesChannelID);
                SqlParam[1] = new SqlParameter("@InvoiceNumber", strInvoiceNumber);
                SqlParam[2] = new SqlParameter("@ToSalesChannelCode", strSalesChannelCode);
                SqlParam[3] = new SqlParameter("@Brand", intBrand);
                SqlParam[3] = new SqlParameter("@FromSalesChannelCode", FromSalesChannelCode);

                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetPrimarySales1DetailEdit", CommandType.StoredProcedure, SqlParam);

                return dsResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
        #region Insert/Update Orderwise P1 Sales
        public void InsertPrimarySales1InfoAdd(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@PSType", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@SalesEnrtyType", eEntryType);
                SqlParam[4] = new SqlParameter("@userid", UserID);
                DataAccess.Instance.DBInsertCommand("PrcInsertPrimarySales1Info", SqlParam);
                Error = Convert.ToString(SqlParam[1].Value);
                if (SqlParam[2].Value != DBNull.Value)
                {
                    strErrorDetailXML = (Convert.ToString(SqlParam[2].Value));
                }
                else
                {
                    strErrorDetailXML = null;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsertPrimarySales1InfoEdit(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@PSType", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
                SqlParam[2].Direction = ParameterDirection.Output;

                DataAccess.Instance.DBInsertCommand("PrcUpdatePrimarySales1Info", SqlParam);
                Error = Convert.ToString(SqlParam[1].Value);
                if (SqlParam[2].Value != DBNull.Value)
                {
                    strErrorDetailXML = (Convert.ToString(SqlParam[2].Value));
                }
                else
                {
                    strErrorDetailXML = null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UploadPrimarySalesInfo(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@PSType", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@SalesChannelId", intSalesChannelID);

                DataAccess.Instance.DBInsertCommand("PrcUploadPrimarySalesInfo", SqlParam);

                if (SqlParam[2].Value.ToString() != "")
                {
                    ErrorDetailXML = SqlParam[2].Value.ToString();
                }
                else
                {
                    ErrorDetailXML = null;
                }
                Error = Convert.ToString(SqlParam[1].Value);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public DataSet FillSaleDropDown()
        {


            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[1] = new SqlParameter("@UserId", UserID);
                SqlParam[2] = new SqlParameter("@CompanyId", CompanyId);
                SqlParam[3] = new SqlParameter("@SalesTypeId", SalestypeId);
                SqlParam[4] = new SqlParameter("@MasterType", Mastertype);

                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcFillSaleDropDown", CommandType.StoredProcedure, SqlParam);

                return dsResult;
            }

            catch (Exception ex)
            {

                throw ex;
            }
        }
        #region Get Orderwise P2 Sales
        public DataSet GetPrimarySales2Add()
        {


            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesChannelID);
                SqlParam[1] = new SqlParameter("@InvoiceNumber", strInvoiceNumber);
                SqlParam[2] = new SqlParameter("@ToSalesChannelCode", strSalesChannelCode);
                SqlParam[3] = new SqlParameter("@Brand", intBrand);

                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetPrimarySales2DetailAdd", CommandType.StoredProcedure, SqlParam);

                return dsResult;
            }

            catch (Exception ex)
            {

                throw ex;
            }
        }
        public DataSet GetPrimarySales2Edit()
        {

            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesChannelID);
                SqlParam[1] = new SqlParameter("@InvoiceNumber", strInvoiceNumber);
                SqlParam[2] = new SqlParameter("@ToSalesChannelCode", strSalesChannelCode);
                SqlParam[3] = new SqlParameter("@Brand", intBrand);

                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetPrimarySales2DetailEdit", CommandType.StoredProcedure, SqlParam);

                return dsResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
        #region Insert/Update Orderwise P2 Sales
        public void InsertPrimarySales2InfoAdd(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@PS2Type", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@SalesEnrtyType", eEntryType);
                SqlParam[4] = new SqlParameter("@userid", UserID);
                DataAccess.Instance.DBInsertCommand("PrcInsPrimarySales2Info", SqlParam);
                Error = Convert.ToString(SqlParam[1].Value);
                if (SqlParam[2].Value != DBNull.Value)
                {
                    strErrorDetailXML = (Convert.ToString(SqlParam[2].Value));
                }
                else
                {
                    strErrorDetailXML = null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsertPrimarySales2InfoEdit(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@PS2Type", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@SalesEnrtyType", eEntryType);

                DataAccess.Instance.DBInsertCommand("PrcUpdatePrimarySales2Info", SqlParam);
                Error = Convert.ToString(SqlParam[1].Value);
                if (SqlParam[2].Value != DBNull.Value)
                {
                    strErrorDetailXML = (Convert.ToString(SqlParam[2].Value));
                }
                else
                {
                    strErrorDetailXML = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UploadPrimarySales2Info(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@PS2Type", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
                SqlParam[2].Direction = ParameterDirection.Output;
                DataAccess.Instance.DBInsertCommand("PrcUploadPrimarySales2Info", SqlParam);
                Error = Convert.ToString(SqlParam[1].Value);
                if (SqlParam[2].Value != DBNull.Value)
                {
                    strErrorDetailXML = (Convert.ToString(SqlParam[2].Value));
                }
                else
                {
                    strErrorDetailXML = null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region insert update SecondarySales
        public Int16 InsertSecondarySalesIMEI(DataTable Dt)
        {
            Int16 IntResultCount = 0;
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@tvpSecondaryIMEI", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                SqlParam[5] = new SqlParameter("@SalesChannelID", SalesChannelID);
                DataAccess.Instance.DBInsertCommand("PrcInsertSecondarySalesIMEI", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[3].Value);
                if (SqlParam[2].Value != DBNull.Value)
                {
                    strErrorDetailXML = SqlParam[2].Value.ToString();
                }
                else
                {
                    strErrorDetailXML = null;
                }
                Error = SqlParam[1].Value.ToString();
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Int16 InsertUpdateSecondarySalesInfo(DataTable Dt)
        {
            Int16 IntResultCount = 0;
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@tvpSecondary", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@SalesEnrtyType", eEntryType);
                SqlParam[5] = new SqlParameter("@userid", UserID);
                DataAccess.Instance.DBInsertCommand("PrcInsUpdSecondarySalesInfo", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[3].Value);
                if (SqlParam[2].Value != DBNull.Value)
                {
                    strErrorDetailXML = SqlParam[2].Value.ToString();
                }
                else
                {
                    strErrorDetailXML = null;
                }
                if (SqlParam[1].Value != DBNull.Value)
                {
                    Error = SqlParam[1].Value.ToString();
                }
                else
                {
                    Error = null;
                }
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Int16 InsertUpdateSecondarySalesInfoUpload(DataTable Dt)
        {
            Int16 IntResultCount = 0;
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@tvpSecondary", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@SalesEnrtyType", eEntryType);
                DataAccess.Instance.DBInsertCommand("PrcInsUpdSecondarySalesInfoUpload", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[3].Value);
                if (SqlParam[2].Value != DBNull.Value)
                {
                    strErrorDetailXML = SqlParam[2].Value.ToString();
                }
                else
                {
                    strErrorDetailXML = null;
                }
                if (SqlParam[1].Value != DBNull.Value)
                {
                    Error = SqlParam[1].Value.ToString();
                }
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region SecondarySalesMicromax
        public void InsertSecondarySalesMicromax(DataTable Dt)
        {

            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@tvpSecondarySales", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[4] = new SqlParameter("@InvoiceDate", InvoiceDate);
                DataAccess.Instance.DBInsertCommand("PrcInsSecondarySalesInfoUploadMMX", SqlParam);

                if (SqlParam[2].Value != DBNull.Value)
                {
                    strErrorDetailXML = SqlParam[2].Value.ToString();
                }
                else
                {
                    strErrorDetailXML = null;
                }
                if (SqlParam[1].Value != DBNull.Value)
                {
                    Error = SqlParam[1].Value.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region SecondarySalesReturn
        public Int32 InsertSecondarySalesReturnInfo(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@SecondarySalesReturn", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", "");
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 1000);
                //SqlParam[3].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(ErrorDetailXML, XmlNodeType.Document, null));
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                DataAccess.Instance.DBInsertCommand("PrcInsertSecondarySalesReturnInfo", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[1].Value);
                if (SqlParam[3].Value.ToString() != "")
                {
                    ErrorDetailXML = SqlParam[3].Value.ToString();
                }
                else
                {
                    ErrorDetailXML = null;
                }
                Error = SqlParam[1].Value.ToString();
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Int32 InsertUpdateSecondarySalesReturnInfo(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@SecondarySalesReturn", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", "");
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 1000);

                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                DataAccess.Instance.DBInsertCommand("PrcInsUpdSecondarySalesReturnInfo", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[1].Value);
                if (SqlParam[3].Value.ToString() != "")
                {
                    ErrorDetailXML = SqlParam[3].Value.ToString();
                }
                else
                {
                    ErrorDetailXML = null;
                }
                Error = SqlParam[1].Value.ToString();
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Primary Sales
        public Int32 InsertPrimarySalesInfo(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@PSType", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@SalesChannelId", intSalesChannelID);       //For Mapping check Pankaj Dhingra      
                DataAccess.Instance.DBInsertCommand("PrcInsertPrimarySalesInfoNewWitoutOrder", SqlParam);
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
        public Int32 InsertPrimarySalesInfoMicromax(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@PSType", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                DataAccess.Instance.DBInsertCommand("PrcInsertPrimarySalesInfoMicromax", SqlParam);
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
        public Int32 InsertPrimarySalesInfoMODUpload(DataTable Dt, int value)
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@PSType", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                if (value == 1)
                    DataAccess.Instance.DBInsertCommand("PrcInsertPrimarySalesInfoMODSapUploadForGfive", SqlParam);
                else if (value == 2)        //Pankaj Dhingra For POC
                    DataAccess.Instance.DBInsertCommand("PrcInsertPrimarySalesInfoMODSapUploadForScheme", SqlParam);
                else
                {
                    if (System.Configuration.ConfigurationSettings.AppSettings["Client"].ToUpper() == "POC")
                    {
                        DataAccess.Instance.DBInsertCommand("PrcInsertPrimarySalesInfoMODSapUploadBasePOC", SqlParam);
                    }
                    else
                    {
                        DataAccess.Instance.DBInsertCommand("PrcInsertPrimarySalesInfoMODSapUpload", SqlParam);
                    }
                }
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
        public Int32 InsertPrimarySalesInfoMODUploadOnida(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@PSType", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                DataAccess.Instance.DBInsertCommand("PrcInsertPrimarySalesInfoMODSapUploadOnida", SqlParam);
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
        public Int32 InsertInfoGRNUpload(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@tvpgrn", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                DataAccess.Instance.DBInsertCommand("PrcGRNUpload", SqlParam);
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
        public Int32 InsertInfoGRNUploadOnida(DataTable Dt)     //For onida 
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@tvpgrn", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                DataAccess.Instance.DBInsertCommand("PrcGRNUploadOnida", SqlParam);
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
        public Int32 InsertInfoGRNUploadForGfive(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@tvpgrn", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                DataAccess.Instance.DBInsertCommand("PrcGRNUploadForGfive", SqlParam);
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
        public Int32 InsertInfoGRNSapUpload(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@PSType", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                DataAccess.Instance.DBInsertCommand("PrcInsertInfoGRNSapUpload", SqlParam);
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
        public Int32 InsertInfoIMEISapUpload(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@PSType", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                DataAccess.Instance.DBInsertCommand("PrcInsertInfoIMEISapUpload", SqlParam);
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
        public Int32 InsertPrimarySales2Info(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@PS2Type", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@SalesEnrtyType", eEntryType);
                SqlParam[5] = new SqlParameter("@userid", UserID);
                DataAccess.Instance.DBInsertCommand("PrcInsertPrimarySales2Info", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[1].Value);
                Error = Convert.ToString(SqlParam[2].Value);
                if (SqlParam[3].Value != DBNull.Value)
                {
                    strErrorDetailXML = (Convert.ToString(SqlParam[3].Value));
                }
                else
                {
                    strErrorDetailXML = null;
                }
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Int32 InsertInfoGRNUploadSB(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@tvpgrnSB", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                SqlParam[5] = new SqlParameter("@EntryType", eEntryType);
                DataAccess.Instance.DBInsertCommand("PrcGRNUploadSB", SqlParam);
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
        public Int32 InsertInfoGRNUploadSBBCP()
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@TransUploadSession", TransUploadSession);
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                SqlParam[5] = new SqlParameter("@EntryType", eEntryType);
                DataAccess.Instance.DBInsertCommand("PrcGRNUploadSBV5", SqlParam);
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
        public Int32 InsertInfoIMEISapBulkUpload(DataTable Dt)
        {
            //Panakj Dhingra(Newly added for Bulk upload of the serial Number)
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@PSType", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                DataAccess.Instance.DBInsertCommand("PrcInsertInfoIMEISapBulkUpload", SqlParam);
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
        #region Fetch PrimarySales1/ PrimarySales2 return data
        public DataTable GetPrimarySales2Return()
        {

            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesChannelID);
                SqlParam[1] = new SqlParameter("@InvoiceNumber", strInvoiceNumber);
                SqlParam[2] = new SqlParameter("@ToSalesChannelCode", strSalesChannelCode);
                SqlParam[3] = new SqlParameter("@ReturnDate", dateReturnDate);
                SqlParam[4] = new SqlParameter("@Brand", intBrand);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetPrimarySales2ReturnDetail", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable GetPrimarySales1Return()
        {
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesChannelID);
                SqlParam[1] = new SqlParameter("@InvoiceNumber", strInvoiceNumber);
                SqlParam[2] = new SqlParameter("@ToSalesChannelCode", strSalesChannelCode);
                SqlParam[3] = new SqlParameter("@ReturnDate", dateReturnDate);
                SqlParam[4] = new SqlParameter("@Brand", intBrand);
                SqlParam[5] = new SqlParameter("@WarehouseSalesChannelCode", ReturnToSalesChannelCode);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetPrimarySales1ReturnDetail", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion
        #region Fetch Secondary Sales data
        public DataTable GetSecondarySales()
        {

            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesChannelID);
                SqlParam[1] = new SqlParameter("@RetailerID", intRetailerID);
                SqlParam[2] = new SqlParameter("@InvoiceDate", dateInvoiceDate);
                SqlParam[3] = new SqlParameter("@RetailerCode", strRetailerCode);
                SqlParam[4] = new SqlParameter("@Brand", intBrand);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetSecondarySalesDetail", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable GetSecondarySalesReturn()
        {

            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesChannelID);
                //SqlParam[1] = new SqlParameter("@RetailerID", intRetailerID);

                SqlParam[1] = new SqlParameter("@RetailerCode", strRetailerCode);
                SqlParam[2] = new SqlParameter("@ReturnDate", dateReturnDate);
                SqlParam[3] = new SqlParameter("@Brand", intBrand);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetSecondarySalesReturnDetail ", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion
        #region Fetch Stock Inhand Check For sales
        public DataTable GetStockInHandForSales()
        {

            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesChannelID);
                SqlParam[1] = new SqlParameter("@InvoiceNumber", strInvoiceNumber);
                SqlParam[2] = new SqlParameter("@SKUId", intSKUId);
                SqlParam[3] = new SqlParameter("@InvoiceDate", dateInvoiceDate);
                SqlParam[4] = new SqlParameter("@SalesType", SalesType);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetStockInhandForSales", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion
        #region insert update Primary sales Return Info
        public Int16 InsertUpdatePrimarySalesReturnInfo(DataTable Dt)
        {
            Int16 IntResultCount = 0;
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@tvpPrimarySalesReturn", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@SalesEnrtyType", eEntryType);
                SqlParam[5] = new SqlParameter("@userid", UserID);
                DataAccess.Instance.DBInsertCommand("PrcInsUpdPrimarySalesReturnInfo", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[3].Value);
                if (SqlParam[2].Value != DBNull.Value)
                {
                    strErrorDetailXML = SqlParam[2].Value.ToString();
                }
                else
                {
                    strErrorDetailXML = null;
                }
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Int16 InsertUpdatePrimarySalesReturnInfoNew(DataTable Dt)        //Pankaj Dhingra(27-06-2011)
        {
            Int16 IntResultCount = 0;
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@tvpPrimarySalesReturn", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@SalesEnrtyType", eEntryType);
                SqlParam[5] = new SqlParameter("@UserID", UserID);
                DataAccess.Instance.DBInsertCommand("PrcInsUpdPrimarySalesReturnInfoNew", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[3].Value);
                if (SqlParam[2].Value != DBNull.Value)
                {
                    strErrorDetailXML = SqlParam[2].Value.ToString();
                }
                else
                {
                    strErrorDetailXML = null;
                }
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Int16 InsertUpdatePrimarySalesReturnInfoNewForMicromax(DataTable Dt)        //Pankaj Dhingra(27-06-2011)
        {
            Int16 IntResultCount = 0;
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@tvpPrimarySalesReturn", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@SalesEnrtyType", eEntryType);
                DataAccess.Instance.DBInsertCommand("PrcInsUpdPrimarySalesReturnInfoMicromax", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[3].Value);
                if (SqlParam[2].Value != DBNull.Value)
                {
                    strErrorDetailXML = SqlParam[2].Value.ToString();
                }
                else
                {
                    strErrorDetailXML = null;
                }
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Int16 InsertUpdatePrimarySalesReturnInfoInterface(DataTable Dt)
        {
            Int16 IntResultCount = 0;
            try
            {
                SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@tvpPrimarySalesReturn", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@SalesEnrtyType", eEntryType);
                SqlParam[5] = new SqlParameter("@userid", UserID);
                DataAccess.Instance.DBInsertCommand("PrcInsUpdPrimarySalesReturnInfoInterface", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[3].Value);
                if (SqlParam[2].Value != DBNull.Value)
                {
                    strErrorDetailXML = SqlParam[2].Value.ToString();
                }
                else
                {
                    strErrorDetailXML = null;
                }
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion
        #region InsertStockTransfer
        public Int32 InsertStockTransferInfo(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@tvpStocktransfer", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@outParam", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@OutError", SqlDbType.VarChar, 200);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                DataAccess.Instance.DBInsertCommand("prcInsStockTransfer", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[1].Value);
                Error = Convert.ToString(SqlParam[2].Value);
                if (SqlParam[3].Value != DBNull.Value)
                {
                    strErrorDetailXML = (Convert.ToString(SqlParam[3].Value));
                }
                else
                {
                    strErrorDetailXML = null;
                }
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region InsertStockAdjustment
        public void InsertStockAdjustment(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@tvpStockAdjustment", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@UserID", intUserID);
                DataAccess.Instance.DBInsertCommand("PrcInsStockAdjustment", SqlParam);

                Error = Convert.ToString(SqlParam[1].Value);
                if (SqlParam[2].Value != DBNull.Value)
                {
                    strErrorDetailXML = (Convert.ToString(SqlParam[2].Value));
                }
                else
                {
                    strErrorDetailXML = null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region NotInUse
        public DataSet GetPrimarySales2Sales()
        {

            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesChannelID);
                SqlParam[1] = new SqlParameter("@InvoiceNumber", strInvoiceNumber);
                SqlParam[2] = new SqlParameter("@ToSalesChannelCode", strSalesChannelCode);
                SqlParam[3] = new SqlParameter("@Brand", intBrand);

                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetPrimarySales2Detail", CommandType.StoredProcedure, SqlParam);

                return dsResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataSet GetPrimarySales1Sales()
        {

            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesChannelID);
                SqlParam[1] = new SqlParameter("@InvoiceNumber", strInvoiceNumber);
                SqlParam[2] = new SqlParameter("@ToSalesChannelCode", strSalesChannelCode);
                SqlParam[3] = new SqlParameter("@Brand", intBrand);
                SqlParam[3] = new SqlParameter("@FromSalesChannelCode", FromSalesChannelCode);

                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetPrimarySales1Detail", CommandType.StoredProcedure, SqlParam);

                return dsResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public Int32 InsertPrimarySales1Info(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@PSType", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@SalesEnrtyType", eEntryType);
                SqlParam[4] = new SqlParameter("@userid", UserID);
                DataAccess.Instance.DBInsertCommand("[PrcInsertPrimarySales1InfoWithoutOrder]", SqlParam);
                Error = Convert.ToString(SqlParam[1].Value);
                if (SqlParam[2].Value != DBNull.Value)
                {
                    strErrorDetailXML = (Convert.ToString(SqlParam[2].Value));
                }
                else
                {
                    strErrorDetailXML = null;
                }
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetPrimarySalesInfo()  //Pankaj dhingra
        {

            try
            {
                SqlParam = new SqlParameter[3];
                SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesChannelID);
                SqlParam[1] = new SqlParameter("@InvoiceNumber", strInvoiceNumber);
                SqlParam[2] = new SqlParameter("@DistributorSalesChannelID", DistributorSalesChannelID);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetPrimaryOrderSalesDetail", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        //public DataTable GetPrimaryOrderInfoForReport()  //Pankaj dhingra
        //{

        //    try
        //    {
        //        SqlParam = new SqlParameter[1];
        //        SqlParam[0] = new SqlParameter("@orderid",OrderId);
        //        dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetPrimaryOrderInfoforReport", CommandType.StoredProcedure, SqlParam);
        //        return dtResult;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }

        //}

        public DataSet GetPrimaryOrderInfoForReport()
        {

            try
            {
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@orderid", OrderId);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetPrimaryOrderInfoforReport", CommandType.StoredProcedure, SqlParam);

                return dsResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public Int32 InsertPrimarySalesInfoDetail(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@PS2Type", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@SalesEnrtyType", eEntryType);
                DataAccess.Instance.DBInsertCommand("PrcInsertPrimarySalesInfoDetail", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[1].Value);
                Error = Convert.ToString(SqlParam[2].Value);
                if (SqlParam[3].Value != DBNull.Value)
                {
                    strErrorDetailXML = (Convert.ToString(SqlParam[3].Value));
                }
                else
                {
                    strErrorDetailXML = null;
                }
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region PrimarySalesBatchWise
        public void UploadPrimarySalesInfoBatch(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@TvpSales", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@SalesChannelId", intSalesChannelID);

                DataAccess.Instance.DBInsertCommand("PrcUploadPrimarySalesBatchInfo", SqlParam);

                if (SqlParam[2].Value.ToString() != "")
                {
                    ErrorDetailXML = SqlParam[2].Value.ToString();
                }
                else
                {
                    ErrorDetailXML = null;
                }
                Error = Convert.ToString(SqlParam[1].Value);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
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

        ~SalesData()
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


        public Int32 InsertStockTransferInfo_V1(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[5];
                SqlParam[0] = new SqlParameter("@tvpStocktransfer", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@outParam", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@OutError", SqlDbType.VarChar, 200);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                DataAccess.Instance.DBInsertCommand("prcInsStockTransferV1", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[1].Value);
                Error = Convert.ToString(SqlParam[2].Value);
                if (SqlParam[3].Value != DBNull.Value)
                {
                    strErrorDetailXML = (Convert.ToString(SqlParam[3].Value));
                }
                else
                {
                    strErrorDetailXML = null;
                }
                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int32 InsertPrimarySales1SBBCP()
        {
            try
            {
                SqlParam = new SqlParameter[8];
                SqlParam[0] = new SqlParameter("@TransUploadSession", TransUploadSession);
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                SqlParam[5] = new SqlParameter("@EntryType", eEntryType);
                SqlParam[6] = new SqlParameter("@SalesChannelId", SalesChannelID);
                SqlParam[7] = new SqlParameter("@ComingFrom", ComingFrom);
                DataAccess.Instance.DBInsertCommand("PrcPrimarySales1UploadSBV5", SqlParam);
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

        public Int32 InsertPrimarySales1SBBCP_V1()
        {
            try
            {
                SqlParam = new SqlParameter[8];
                SqlParam[0] = new SqlParameter("@TransUploadSession", TransUploadSession);
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                SqlParam[5] = new SqlParameter("@EntryType", eEntryType);
                SqlParam[6] = new SqlParameter("@SalesChannelId", SalesChannelID);
                SqlParam[7] = new SqlParameter("@ComingFrom", ComingFrom);
                DataAccess.Instance.DBInsertCommand("PrcPrimarySales1UploadSBV5_V1", SqlParam);
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

        public Int32 InsertPrimarySales1SB(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@tvpP1SB", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                SqlParam[5] = new SqlParameter("@EntryType", eEntryType);
                SqlParam[6] = new SqlParameter("@SalesChannelId", SalesChannelID);
                DataAccess.Instance.DBInsertCommand("PrcPrimarySales1UploadSB", SqlParam);
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
        public Int32 InsertPrimarySalesReturn1SB(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[9];
                SqlParam[0] = new SqlParameter("@tvpP1ReturnSB", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                SqlParam[5] = new SqlParameter("@EntryType", eEntryType);
                SqlParam[6] = new SqlParameter("@SalesChannelId", SalesChannelID);
                SqlParam[7] = new SqlParameter("@SalesReturnDate", ReturnDate);
                SqlParam[8] = new SqlParameter("@StockBinType", StockBinType);
                DataAccess.Instance.DBInsertCommand("PrcPrimarySalesReturn1UploadSB", SqlParam);
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

        public Int32 InsertPrimarySalesReturn1SBBCP()//23-Aug-2014
        {
            try
            {
                SqlParam = new SqlParameter[13];
                SqlParam[0] = new SqlParameter("@TransUploadSession", TransUploadSession);
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                SqlParam[5] = new SqlParameter("@EntryType", eEntryType);
                SqlParam[6] = new SqlParameter("@SalesChannelId", SalesChannelID);
                SqlParam[7] = new SqlParameter("@SalesReturnDate", ReturnDate);
                SqlParam[8] = new SqlParameter("@StockBinType", StockBinType);
                SqlParam[9] = new SqlParameter("@TemplateType", TemplateType);
                SqlParam[10] = new SqlParameter("@ComingFrom", ComingFrom);
                SqlParam[11] = new SqlParameter("@OriginalFileName", strOriginalFileName);
                SqlParam[12] = new SqlParameter("@UniqueFileName", strUploadedFileName);
                DataAccess.Instance.DBInsertCommand("PrcPrimarySalesReturn1UploadSBV5", SqlParam);
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


        public Int32 InsertIntermediarySalesReturnSB(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[9];
                SqlParam[0] = new SqlParameter("@tvpIntermediaryReturnSB", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                SqlParam[5] = new SqlParameter("@EntryType", eEntryType);
                SqlParam[6] = new SqlParameter("@SalesChannelId", SalesChannelID);
                SqlParam[7] = new SqlParameter("@SalesReturnDate", ReturnDate);
                SqlParam[8] = new SqlParameter("@StockBinType", StockBinType);
                DataAccess.Instance.DBInsertCommand("PrcIntermediarySalesReturnUploadSB", SqlParam);
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


        public Int32 InsertIntermediarySalesReturnSBBCP()
        {
            try
            {
                SqlParam = new SqlParameter[11];
                SqlParam[0] = new SqlParameter("@TransUploadSession", TransUploadSession);
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                SqlParam[5] = new SqlParameter("@EntryType", eEntryType);
                SqlParam[6] = new SqlParameter("@SalesChannelId", SalesChannelID);
                SqlParam[7] = new SqlParameter("@SalesReturnDate", ReturnDate);
                SqlParam[8] = new SqlParameter("@StockBinType", StockBinType);
                SqlParam[9] = new SqlParameter("@TemplateType", TemplateType);
                SqlParam[10] = new SqlParameter("@ComingFrom", ComingFrom);
                DataAccess.Instance.DBInsertCommand("PrcIntermediarySalesReturnUploadSBV5", SqlParam);
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

        public Int32 InsertIntermediarySalesSB(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@tvpIntermediarySalesSB", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                SqlParam[5] = new SqlParameter("@EntryType", eEntryType);
                SqlParam[6] = new SqlParameter("@SalesChannelId", SalesChannelID);
                DataAccess.Instance.DBInsertCommand("PrcIntermediarySalesUploadSB", SqlParam);
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



        public Int32 InsertBulkActivationBCP()
        {
            try
            {
                SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@TransUploadSession", TransUploadSession);
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                SqlParam[5] = new SqlParameter("@EntryType", eEntryType);
                SqlParam[6] = new SqlParameter("@SalesChannelId", SalesChannelID);
                DataAccess.Instance.DBInsertCommand("PrcInsertBulkActivationBCP", SqlParam);
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

        public Int32 InsertIntermediarySalesSBBCP()
        {
            try
            {
                SqlParam = new SqlParameter[8];
                SqlParam[0] = new SqlParameter("@TransUploadSession", TransUploadSession);
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                SqlParam[5] = new SqlParameter("@EntryType", eEntryType);
                SqlParam[6] = new SqlParameter("@SalesChannelId", SalesChannelID);
                SqlParam[7] = new SqlParameter("@ComingFrom", ComingFrom);
                DataAccess.Instance.DBInsertCommand("PrcIntermediarySalesUploadSBV5", SqlParam);
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

        public Int32 InsertSecondarySalesSB(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@tvpSecondarySalesSB", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                SqlParam[5] = new SqlParameter("@EntryType", eEntryType);
                SqlParam[6] = new SqlParameter("@SalesChannelId", SalesChannelID);
                //DataAccess.Instance.DBInsertCommand("PrcSecondarySalesUploadSBV2", SqlParam);
                DataAccess.Instance.DBInsertCommand("PrcSecondarySalesUploadSB", SqlParam);
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
        public Int32 InsertSecondarySalesSBBCP()
        {
            try
            {
                SqlParam = new SqlParameter[9];
                SqlParam[0] = new SqlParameter("@TransUploadSession", TransUploadSession);
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                SqlParam[5] = new SqlParameter("@EntryType", eEntryType);
                SqlParam[6] = new SqlParameter("@SalesChannelId", SalesChannelID);
                SqlParam[7] = new SqlParameter("@ComingFrom", ComingFrom);
                SqlParam[8] = new SqlParameter("@CompanyId", CompanyId);

                //DataAccess.Instance.DBInsertCommand("PrcSecondarySalesUploadSBV2", SqlParam);
                DataAccess.Instance.DBInsertCommand("PrcSecondarySalesUploadSBV5", SqlParam);
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

        public Int32 InsertSecondarySalesReturnSBBCP()
        {
            try
            {
                SqlParam = new SqlParameter[11];
                SqlParam[0] = new SqlParameter("@TransUploadSession", TransUploadSession);
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                SqlParam[5] = new SqlParameter("@EntryType", eEntryType);
                SqlParam[6] = new SqlParameter("@SalesChannelId", SalesChannelID);
                SqlParam[7] = new SqlParameter("@SalesReturnDate", ReturnDate);
                SqlParam[8] = new SqlParameter("@StockBinType", StockBinType);
                SqlParam[9] = new SqlParameter("@TemplateType", TemplateType);
                SqlParam[10] = new SqlParameter("@ComingFrom", ComingFrom);
                DataAccess.Instance.DBInsertCommand("PrcSecondarySalesReturnUploadSBV5", SqlParam);
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



        public Int32 InsertSecondarySalesReturnSB(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[9];
                SqlParam[0] = new SqlParameter("@tvpSecondarySalesReturnSB", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                SqlParam[5] = new SqlParameter("@EntryType", eEntryType);
                SqlParam[6] = new SqlParameter("@SalesChannelId", SalesChannelID);
                SqlParam[7] = new SqlParameter("@SalesReturnDate", ReturnDate);
                SqlParam[8] = new SqlParameter("@StockBinType", StockBinType);
                DataAccess.Instance.DBInsertCommand("PrcSecondarySalesReturnUploadSB", SqlParam);
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
        public DataTable GetDataForAcknowlegement()
        {

            try
            {
                SqlParam = new SqlParameter[9];
                SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesChannelID);
                SqlParam[1] = new SqlParameter("@InvoiceNumber", InvoiceNumber);
                SqlParam[2] = new SqlParameter("@InvoiceFromDate", FromDate);
                SqlParam[3] = new SqlParameter("@InvoiceToDate", ToDate);
                SqlParam[4] = new SqlParameter("@AckStatus", AckStatus);
                SqlParam[5] = new SqlParameter("@FlagForTable", FlagForTable);
                SqlParam[6] = new SqlParameter("@SalesUniqueID", SalesUniqueID);
                SqlParam[7] = new SqlParameter("@OtherEntity", OtherEntity);
                SqlParam[8] = new SqlParameter("@UserID", UserID);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetDataForAcknowlegement", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataTable GetDataForAcknowlegementExportToExcel()
        {

            try
            {
                SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesChannelID);
                SqlParam[1] = new SqlParameter("@InvoiceNumber", InvoiceNumber);
                SqlParam[2] = new SqlParameter("@InvoiceFromDate", FromDate);
                SqlParam[3] = new SqlParameter("@InvoiceToDate", ToDate);
                SqlParam[4] = new SqlParameter("@AckStatus", AckStatus);
                SqlParam[5] = new SqlParameter("@OtherEntity", OtherEntity);
                SqlParam[6] = new SqlParameter("@UserID", UserID);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetDataForAcknowlegementExportToExcel", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public DataTable GetInvoiceListForCancellation()
        {
            try
            {
                SqlParam = new SqlParameter[9];
                SqlParam[0] = new SqlParameter("@SalesChannelID", intSalesChannelID);
                SqlParam[1] = new SqlParameter("@InvoiceNumber", InvoiceNumber);
                SqlParam[2] = new SqlParameter("@InvoiceFromDate", FromDate);
                SqlParam[3] = new SqlParameter("@InvoiceToDate", ToDate);
                SqlParam[4] = new SqlParameter("@AckStatus", AckStatus);
                SqlParam[5] = new SqlParameter("@FlagForTable", FlagForTable);
                SqlParam[6] = new SqlParameter("@SalesUniqueID", SalesUniqueID);
                SqlParam[7] = new SqlParameter("@OtherEntity", OtherEntity);
                SqlParam[8] = new SqlParameter("@UserID", UserID);
                dtResult = DataAccess.Instance.GetTableFromDatabase("prcGetDataForCancellationBySeller", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public Int32 InsertAcknowlegeInformationSB()
        {
            try
            {
                SqlParam = new SqlParameter[10];
                //SqlParam[0] = new SqlParameter("@tvpSecondarySalesSB", SqlDbType.Structured);
                //SqlParam[0].Value = Dt;
                SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@userid", UserID);
                SqlParam[4] = new SqlParameter("@EntryType", eEntryType);
                SqlParam[5] = new SqlParameter("@SalesChannelId", SalesChannelID);
                SqlParam[6] = new SqlParameter("@SalesUniqueID", SalesUniqueID);
                SqlParam[7] = new SqlParameter("@value", value);
                SqlParam[8] = new SqlParameter("@Decider", Decider);
                SqlParam[9] = new SqlParameter("@OtherEntity", OtherEntity);
                DataAccess.Instance.DBInsertCommand("PrcInsertAcknowlegeInformationSB", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[0].Value);
                if (SqlParam[2].Value.ToString() != "")
                {
                    ErrorDetailXML = SqlParam[2].Value.ToString();
                }
                else
                {
                    ErrorDetailXML = null;
                }
                Error = Convert.ToString(SqlParam[1].Value);

                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        public Int32 InsertCancellationInformationSB()
        {
            try
            {
                SqlParam = new SqlParameter[10];
                //SqlParam[0] = new SqlParameter("@tvpSecondarySalesSB", SqlDbType.Structured);
                //SqlParam[0].Value = Dt;
                SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@userid", UserID);
                SqlParam[4] = new SqlParameter("@EntryType", eEntryType);
                SqlParam[5] = new SqlParameter("@SalesChannelId", SalesChannelID);
                SqlParam[6] = new SqlParameter("@SalesUniqueID", SalesUniqueID);
                SqlParam[7] = new SqlParameter("@value", value);
                SqlParam[8] = new SqlParameter("@Decider", Decider);
                SqlParam[9] = new SqlParameter("@OtherEntity", OtherEntity);
                DataAccess.Instance.DBInsertCommand("PrcInsertCancellationInformationSB", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[0].Value);
                if (SqlParam[2].Value.ToString() != "")
                {
                    ErrorDetailXML = SqlParam[2].Value.ToString();
                }
                else
                {
                    ErrorDetailXML = null;
                }
                Error = Convert.ToString(SqlParam[1].Value);

                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Int32 InsertCancellationInformationBulkSB(DataTable Dt)
        {
            try
            {
                SqlParam = new SqlParameter[10];
                SqlParam[0] = new SqlParameter("@tvpInvoiceCancellation", SqlDbType.Structured);
                SqlParam[0].Value = Dt;
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                SqlParam[5] = new SqlParameter("@EntryType", eEntryType);
                SqlParam[6] = new SqlParameter("@LoggedInSalesChannelId", SalesChannelID);
                SqlParam[7] = new SqlParameter("@Decider", Decider);
                SqlParam[8] = new SqlParameter("@OtherEntity", OtherEntity);
                SqlParam[9] = new SqlParameter("@SalesTypeCancellation", SalesTypeCancellation);
                DataAccess.Instance.DBInsertCommand("PrcInsertCancellationInformationBulkSB", SqlParam);
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
        public DataSet GetInvoiceListTobeCancelled()
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@UserID", UserID);
                SqlParam[1] = new SqlParameter("@LoggedInSalesChannelID", SalesChannelID);
                SqlParam[2] = new SqlParameter("@Decider", Decider);
                SqlParam[3] = new SqlParameter("@OtherEntity", OtherEntity);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetInvoiceListTobeCancelled", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetInvoiceList()
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@InvoiceNumber", InvoiceNumber);
                SqlParam[1] = new SqlParameter("@value", value);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@ReturnFromSalesChannelID", ReturnFromSalesChannelID);
                dtResult = DataAccess.Instance.GetTableFromDatabase("PrcGetInvoiceList", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public DataTable GetInvoiceListFull()
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@InvoiceNumber", InvoiceNumber);
                SqlParam[1] = new SqlParameter("@value", value);
                SqlParam[2] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[3] = new SqlParameter("@ReturnFromSalesChannelID", ReturnFromSalesChannelID);
                dtResult = DataAccess.Instance.GetTableFromDatabase("PrcGetInvoiceNumberListFull", CommandType.StoredProcedure, SqlParam);
                return dtResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        //Insert Bulk Excel data for Primary IMEI Ack//

        public bool Insert_BCP_IMEIAck(DataTable dt)
        {
            try
            {
                bool res = DataAccess.Instance.BCP_IMEIAck(dt);
                return res;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public Int32 InsertAcknowlegeInformationSB_V1()
        {
            try
            {
                SqlParam = new SqlParameter[11];
                //SqlParam[0] = new SqlParameter("@tvpSecondarySalesSB", SqlDbType.Structured);
                //SqlParam[0].Value = Dt;
                SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@userid", UserID);
                SqlParam[4] = new SqlParameter("@EntryType", eEntryType);
                SqlParam[5] = new SqlParameter("@SalesChannelId", SalesChannelID);
                SqlParam[6] = new SqlParameter("@SalesUniqueID", SalesUniqueID);
                SqlParam[7] = new SqlParameter("@value", value);
                SqlParam[8] = new SqlParameter("@Decider", Decider);
                SqlParam[9] = new SqlParameter("@OtherEntity", OtherEntity);
                SqlParam[10] = new SqlParameter("@IMEIAckSessionID", IMEISessionID);
                DataAccess.Instance.DBInsertCommand("PrcInsertAcknowlegeInformationSB_V1", SqlParam);
                IntResultCount = Convert.ToInt16(SqlParam[0].Value);
                if (SqlParam[2].Value.ToString() != "")
                {
                    ErrorDetailXML = SqlParam[2].Value.ToString();
                }
                else
                {
                    ErrorDetailXML = null;
                }
                Error = Convert.ToString(SqlParam[1].Value);

                return IntResultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet dsSecondarySalesReturnDropdownData()
        {
            try
            {
                SqlParam = new SqlParameter[6];/* #CC01 Length increased from 4 to 6 */
                SqlParam[0] = new SqlParameter("@OutError", SqlDbType.NVarChar, 2000);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@TotalRecords", SqlDbType.Int);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@SalesChannelTypeID", SalesChannelTypeID);
                SqlParam[3] = new SqlParameter("@UserID", UserID); /* #CC01 Added */
                SqlParam[4] = new SqlParameter("@BilltoRetailer", BilltoRetailer);
                SqlParam[5] = new SqlParameter("@SalesChannelCode", SalesChannelCode);

                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcSalesChannelAndTypeDetail", CommandType.StoredProcedure, SqlParam);
                TotalRecords = Convert.ToInt32(SqlParam[1].Value);
                if (SqlParam[0].Value != null && !string.IsNullOrEmpty(SqlParam[0].Value.ToString()))
                {
                    strError = SqlParam[0].Value.ToString();
                }
                return dsResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet dsSecondarySalesReturnApprovalData()
        {
            try
            {
                SqlParam = new SqlParameter[11];
                SqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
                SqlParam[1] = new SqlParameter("@PageSize", PageSize);
                SqlParam[2] = new SqlParameter("@TotalRecords", SqlDbType.Int, 10);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[4] = new SqlParameter("@SalesChannelCode", SalesChannelCode);
                SqlParam[5] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 2000);
                SqlParam[5].Direction = ParameterDirection.Output;
                SqlParam[6] = new SqlParameter("@Status", Status);
                SqlParam[7] = new SqlParameter("@InvoiceNumber", InvoiceNumber);
                SqlParam[8] = new SqlParameter("@FromDate", strFromDate);
                SqlParam[9] = new SqlParameter("@Todate", strToDate);
               
                SqlParam[10] = new SqlParameter("@SaleschannelType",SalesChannelTypeID);

                /*#CC02 Added Started*/
                if(ReturnType==1)
                {
                    dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcSecondarySalesReturnApprovalData", CommandType.StoredProcedure, SqlParam);
                }
                if(ReturnType==2)
                {
                    dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcIntermediarySAlesReturnApprovalData", CommandType.StoredProcedure, SqlParam);
                }
                /*#CC02 Added End*/
                TotalRecords = Convert.ToInt32(SqlParam[2].Value);
                if (SqlParam[5].Value != null && !string.IsNullOrEmpty(SqlParam[5].Value.ToString()))
                    strError = SqlParam[5].Value.ToString();
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet dsSecondarySalesReturnSerialData()
        {
            try
            {
                SqlParam = new SqlParameter[7];

                SqlParam[0] = new SqlParameter("@TotalRecords", SqlDbType.Int, 10);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@SecondarySalesReturnMainID", SecondarySalesReturnMainID);
                SqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 2000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@Status", Status);
                /*#CC02 Added Started*/
                if(ReturnType==1)
                {
                    dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcSecondarySalesReturnSerialData", CommandType.StoredProcedure, SqlParam);
                }
                if(ReturnType==2)
                {
                    dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcIntermediarySalesReturnSerialData", CommandType.StoredProcedure, SqlParam);
                }
                /*#CC02 Added End*/
                TotalRecords = Convert.ToInt32(SqlParam[1].Value);
                if (SqlParam[2].Value != null && !string.IsNullOrEmpty(SqlParam[2].Value.ToString()))
                    strError = SqlParam[2].Value.ToString();
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int ApproveRejectSecondarySalesReturn()
        {
            //Int16 Result;
            SqlParam = new SqlParameter[7];
            SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.Int);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@userid", UserID);
            SqlParam[3] = new SqlParameter("@SecondarySalesReturnMainID", SecondarySalesReturnMainID);
            //SqlParam[3].Direction = ParameterDirection.Output;
            SqlParam[4] = new SqlParameter("@ApprovalStatus", ApproveStatus);

            SqlParam[5] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
            SqlParam[5].Direction = ParameterDirection.Output;
            /*#CC02 Added Started*/
            if(ReturnType==1)
            {
                DataAccess.Instance.DBInsertCommand("PrcSecondarySalesReturnApprove", SqlParam);
            }
           if(ReturnType==2)
            {
                DataAccess.Instance.DBInsertCommand("PrcIntermediarySalesReturnApprove", SqlParam);
            }
           /*#CC02 Added End*/
            intOutParam = Convert.ToInt16(SqlParam[0].Value);
            if (SqlParam[1].Value != null && !string.IsNullOrEmpty(SqlParam[1].Value.ToString()))
                Error = SqlParam[1].Value.ToString();
            else
                Error = null;
            if (SqlParam[5].Value != null && !string.IsNullOrEmpty(SqlParam[5].Value.ToString()))
                ErrorDetailXML = (Convert.ToString(SqlParam[5].Value));
            else
                ErrorDetailXML = null;

            return intOutParam;
        }
        /*#CC04 add start*/
        public DataSet GetInterfaceSaleFailTrans()
        {
            try
            {
                SqlParam = new SqlParameter[6];

                SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@OutError", SqlDbType.VarChar, 500);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@userid",  UserID);
                SqlParam[3] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[4] = new SqlParameter("@FromDate", FromDate);
                SqlParam[5] = new SqlParameter("@ToDate", ToDate);
                
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcDownloadInterfaceSaleErrorRecord", CommandType.StoredProcedure, SqlParam);
                
                if (! string.IsNullOrEmpty( Convert.ToString(SqlParam[1].Value)))
                Error = Convert.ToString(SqlParam[1].Value);

                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int32 ProcessInterFaceFailTrans()
        {
            try
            {
                SqlParam = new SqlParameter[8];
                SqlParam[0] = new SqlParameter("@TransUploadSession", TransUploadSession);
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                SqlParam[5] = new SqlParameter("@OriginalFileName", strOriginalFileName);
                SqlParam[6] = new SqlParameter("@UniqueFileName", strUploadedFileName);
                SqlParam[7] = new SqlParameter("@SalesChannelId", SalesChannelID);
                

                DataAccess.Instance.DBInsertCommand("prcProcessInterFaceFailTrans", SqlParam);
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
        /*#CC04 end*/
        /*#CC07 add start*/
        public DataSet GetInterfaceSaleFailTransWithMaster()
        {
            try
            {
                SqlParam = new SqlParameter[7];

                SqlParam[0] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@OutError", SqlDbType.VarChar, 500);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@userid", UserID);
                SqlParam[3] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[4] = new SqlParameter("@FromDate", FromDate);
                SqlParam[5] = new SqlParameter("@ToDate", ToDate);
                SqlParam[6] = new SqlParameter("@DownloadAll", ReturnType);

                dsResult = DataAccess.Instance.GetDataSetFromDatabase("PrcDownloadInterfaceErrorWithMaster", CommandType.StoredProcedure, SqlParam);

                if (!string.IsNullOrEmpty(Convert.ToString(SqlParam[1].Value)))
                    Error = Convert.ToString(SqlParam[1].Value);

                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int32 ProcessInterFaceFailTransWithMaster()
        {
            try
            {
                SqlParam = new SqlParameter[8];
                SqlParam[0] = new SqlParameter("@TransUploadSession", TransUploadSession);
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                SqlParam[5] = new SqlParameter("@OriginalFileName", strOriginalFileName);
                SqlParam[6] = new SqlParameter("@UniqueFileName", strUploadedFileName);
                SqlParam[7] = new SqlParameter("@SalesChannelId", SalesChannelID);


                DataAccess.Instance.DBInsertCommand("prcProcessInterFaceFailTransWithMaster", SqlParam);
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
        /*#CC07 end*/
        /*#CC05 Added*/
        public Int32 InsertTertiarySalesSBBCP()
        {
            try
            {
                SqlParam = new SqlParameter[8];
                SqlParam[0] = new SqlParameter("@TransUploadSession", TransUploadSession);
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                SqlParam[5] = new SqlParameter("@EntryType", eEntryType);
                SqlParam[6] = new SqlParameter("@SalesChannelId", SalesChannelID);
                SqlParam[7] = new SqlParameter("@ComingFrom", ComingFrom);
                DataAccess.Instance.DBInsertCommand("PrcTertiarySalesUploadSBV5", SqlParam);
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
        /*#CC05 Added End*/
        /*#CC06 Added Started*/
        public DataSet GetAllTemplateData()
        {
            try
            {
                SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@UserID", UserID);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcGetSKUInfoForactivationData", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*#CC06 Added End*/
        /*#CC08 start*/
        public Int32 SalesReturnRequest()
        {
            try
            {
                SqlParam = new SqlParameter[7];
                SqlParam[0] = new SqlParameter("@TransUploadSession", TransUploadSession);
                SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 1000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[4] = new SqlParameter("@userid", UserID);
                SqlParam[5] = new SqlParameter("@SalesChannelId", SalesChannelID);
                SqlParam[6] = new SqlParameter("@RequestRemark", Remark);

                DataAccess.Instance.DBInsertCommand("PrcSalesReturnRequest", SqlParam);
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

        public DataSet dsSalesReturnRequestData()
        {
            try
            {
                SqlParam = new SqlParameter[12];
                SqlParam[0] = new SqlParameter("@PageIndex", PageIndex);
                SqlParam[1] = new SqlParameter("@PageSize", PageSize);
                SqlParam[2] = new SqlParameter("@TotalRecords", SqlDbType.Int, 10);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@SalesChannelID", SalesChannelID);
                SqlParam[4] = new SqlParameter("@SalesChannelCode", SalesChannelCode);
                SqlParam[5] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 2000);
                SqlParam[5].Direction = ParameterDirection.Output;
                SqlParam[6] = new SqlParameter("@RequestStatus", Status);
                SqlParam[7] = new SqlParameter("@SalesChannelName", SalesChannelName);
                SqlParam[8] = new SqlParameter("@FromDate", strFromDate);
                SqlParam[9] = new SqlParameter("@Todate", strToDate);

                SqlParam[10] = new SqlParameter("@UserID", UserID);
                SqlParam[11] = new SqlParameter("@ReturnType", ReturnType);//2 Intermediary 3= secondary

                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcSalesReturnRequestData", CommandType.StoredProcedure, SqlParam);
                
                TotalRecords = Convert.ToInt32(SqlParam[2].Value);
                if (SqlParam[5].Value != null && !string.IsNullOrEmpty(SqlParam[5].Value.ToString()))
                    strError = SqlParam[5].Value.ToString();
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet dsSalesReturnRequestDetailData()
        {
            try
            {
                SqlParam = new SqlParameter[7];

                SqlParam[0] = new SqlParameter("@TotalRecords", SqlDbType.Int, 10);
                SqlParam[0].Direction = ParameterDirection.Output;
                SqlParam[1] = new SqlParameter("@SalesReturnRequestID", SecondarySalesReturnMainID);
                SqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 2000);
                SqlParam[2].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@UserID", UserID);
                SqlParam[4] = new SqlParameter("@CanApprove", SqlDbType.Bit);
                SqlParam[4].Direction = ParameterDirection.Output;
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcSalesReturnDetailData", CommandType.StoredProcedure, SqlParam);
                
                TotalRecords = Convert.ToInt32(SqlParam[1].Value);
                if (SqlParam[2].Value != null && !string.IsNullOrEmpty(SqlParam[2].Value.ToString()))
                    strError = SqlParam[2].Value.ToString();
                CanApprove = Convert.ToBoolean(SqlParam[4].Value);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int ApproveRejectSalesReturnRequest()
        {
            //Int16 Result;
            SqlParam = new SqlParameter[6];
            SqlParam[0] = new SqlParameter("@OutParam", SqlDbType.Int);
            SqlParam[0].Direction = ParameterDirection.Output;
            SqlParam[1] = new SqlParameter("@OutError", SqlDbType.VarChar, 2000);
            SqlParam[1].Direction = ParameterDirection.Output;
            SqlParam[2] = new SqlParameter("@userid", UserID);
            SqlParam[3] = new SqlParameter("@SalesReturnRequestID", SecondarySalesReturnMainID);
            SqlParam[4] = new SqlParameter("@ApproveStatus", ApproveStatus);

            SqlParam[5] = new SqlParameter("@ApprovalRemark", Remark);
            
            DataAccess.Instance.DBInsertCommand("prcSalesReturnRequestApproval", SqlParam);
            
            intOutParam = Convert.ToInt16(SqlParam[0].Value);
            if (SqlParam[1].Value != null && !string.IsNullOrEmpty(SqlParam[1].Value.ToString()))
                Error = SqlParam[1].Value.ToString();
            else
                Error = null;
           

            return intOutParam;
        }
        /*#CC08 end*/
    }

}
