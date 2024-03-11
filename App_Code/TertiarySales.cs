using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml;
/*
 * Change Log
 * DD-MMM-YYY, Name, #CCXX, Description.
 * 19-09-2016, Sumit Maurya, #CC01, Method and properties copied from TempClass.
 */

namespace DataAccess
{
    public class TertiarySales : IDisposable
    {
        #region Private Class Variables
        private Int64 _intTertiarySalesID;
        private int _intSalesFromID;
        private string _strInvoiceNumber;
        private DateTime _dtInvoiceDate;
        private int _intTotalQuantity;
        private decimal _dcmTotalAmount;
        private bool _blnStatus;
        private int _intISPID;
        private DateTime _dtRecordCreationDate;
        private int _intCreatedBy;
        private DateTime _dtModifiedOn;
        private int _intModifiedBy;

        private string _strError;
        private Int32 _intPageIndex;
        private Int32 _intPageSize;
        private Int32 _intTotalRecords;
        DateTime? datefrom; /* #CC01 Added */
        #endregion

        #region Public Properties
        /* #CC01 Add Start */
        public DateTime? DateFrom
        {
            get { return datefrom; }
            set { datefrom = value; }
        }
        /* #CC01 Add End */
        public Int32 CompanyId { get; set; }
        public string SKUName { get; set; }
        public Int64 TertiarySalesID
        {
            get
            {
                return _intTertiarySalesID;
            }
            set
            {
                _intTertiarySalesID = value;
            }
        }
        public int SalesFromID
        {
            get
            {
                return _intSalesFromID;
            }
            set
            {
                _intSalesFromID = value;
            }
        }
        public string InvoiceNumber
        {
            get
            {
                return _strInvoiceNumber;
            }
            set
            {
                _strInvoiceNumber = value;
            }
        }
        public DateTime InvoiceDate
        {
            get
            {
                return _dtInvoiceDate;
            }
            set
            {
                _dtInvoiceDate = value;
            }
        }
        public int TotalQuantity
        {
            get
            {
                return _intTotalQuantity;
            }
            set
            {
                _intTotalQuantity = value;
            }
        }
        public decimal TotalAmount
        {
            get
            {
                return _dcmTotalAmount;
            }
            set
            {
                _dcmTotalAmount = value;
            }
        }
        public bool Status
        {
            get
            {
                return _blnStatus;
            }
            set
            {
                _blnStatus = value;
            }
        }
        public int ISPID
        {
            get
            {
                return _intISPID;
            }
            set
            {
                _intISPID = value;
            }
        }
        public DateTime RecordCreationDate
        {
            get
            {
                return _dtRecordCreationDate;
            }
            set
            {
                _dtRecordCreationDate = value;
            }
        }
        public int CreatedBy
        {
            get
            {
                return _intCreatedBy;
            }
            set
            {
                _intCreatedBy = value;
            }
        }
        public DateTime ModifiedOn
        {
            get
            {
                return _dtModifiedOn;
            }
            set
            {
                _dtModifiedOn = value;
            }
        }
        public int ModifiedBy
        {
            get
            {
                return _intModifiedBy;
            }
            set
            {
                _intModifiedBy = value;
            }
        }

        public string Error
        {
            get
            {
                return _strError;
            }
            private set
            {
                _strError = value;
            }
        }
        public Int32 PageIndex
        {
            private get
            {
                return _intPageIndex;
            }
            set
            {
                _intPageIndex = value;
            }
        }
        public Int32 PageSize
        {
            private get
            {
                return _intPageSize;
            }
            set
            {
                _intPageSize = value;
            }
        }
        public Int32 TotalRecords
        {
            get
            {
                return _intTotalRecords;
            }
            private set
            {
                _intTotalRecords = value;
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

        ~TertiarySales()
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



        //public Int32 InsertTeriarySales()
        //{
        //    try
        //    {
        //        SqlParam = new SqlParameter[3];
        //        SqlParam[0] = new SqlParameter("@PriceMasterID", PriceMasterID);
        //        SqlParam[1] = new SqlParameter("@Out_Param", SqlDbType.TinyInt, 2);
        //        SqlParam[1].Direction = ParameterDirection.Output;
        //        SqlParam[2] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 500);
        //        SqlParam[2].Direction = ParameterDirection.Output;
        //        DataAccess.Instance.DBInsertCommand("prcPriceMaster_Delete", SqlParam);
        //        IntResultCount = Convert.ToInt16(SqlParam[1].Value);
        //        error = Convert.ToString(SqlParam[2].Value);
        //        return IntResultCount;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}  
        private string strXMLList;
        public string XMLList
        {
            get { return strXMLList; }
            set { strXMLList = value; }
        }

        public string IEMI { get; set; }
        public string BatchCode { get; set; }
        public Int32 Quantity { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }


        public Int16 InsertTertairySalesForRetailer()
        {

            Int16 Result;
            SqlParameter[] SqlParam = new SqlParameter[17];
            SqlParam[0] = new SqlParameter("@SalesFromID", SalesFromID);
            SqlParam[1] = new SqlParameter("@InvoiceNumber", null);
            SqlParam[2] = new SqlParameter("@InvoiceDate", InvoiceDate);
            SqlParam[3] = new SqlParameter("@ISPID", ISPID);
            SqlParam[4] = new SqlParameter("@CreatedBy", CreatedBy);
            SqlParam[5] = new SqlParameter("@IEMI", IEMI);
            SqlParam[6] = new SqlParameter("@FirstName", FirstName);
            SqlParam[7] = new SqlParameter("@MiddleName", MiddleName);
            SqlParam[8] = new SqlParameter("@LastName", LastName);
            SqlParam[9] = new SqlParameter("@Mobile", Mobile);
            SqlParam[10] = new SqlParameter("@Out_Error", SqlDbType.VarChar, 200);
            SqlParam[10].Direction = ParameterDirection.Output;
            SqlParam[11] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
            SqlParam[11].Direction = ParameterDirection.Output;

            SqlParam[12] = new SqlParameter("@Out_Param", SqlDbType.Int);
            SqlParam[12].Direction = ParameterDirection.Output;
            SqlParam[13] = new SqlParameter("@BatchCode", BatchCode);
            SqlParam[14] = new SqlParameter("@Quantity", Quantity);
            SqlParam[15] = new SqlParameter("@CompanyId", CompanyId);
            SqlParam[16] = new SqlParameter("@SKUName", SKUName);
            DataAccess.Instance.DBInsertCommand("prcTertiarySales_Insert", SqlParam);
            if (SqlParam[11].Value.ToString() != "")
            {
                XMLList = SqlParam[11].Value.ToString();
            }
            else
            {
                XMLList = null;
            }
            Error = Convert.ToString(SqlParam[10].Value);
            Result = Convert.ToInt16(SqlParam[12].Value);
            return Result;
        }



        public DataTable GetTertiarySalesByRetailer()
        {
            try
            {

                SqlParameter[] SqlParam = new SqlParameter[2];
                SqlParam[0] = new SqlParameter("@SalesFromID", SalesFromID);
                SqlParam[1] = new SqlParameter("@RecordCreationDate", RecordCreationDate);
                return DataAccess.Instance.GetTableFromDatabase("prcGetTertiarySales_Select", CommandType.StoredProcedure, SqlParam);

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetTertiarySalesByRetailerForReport()
        {
            try
            {

                SqlParameter[] SqlParam = new SqlParameter[1];
                SqlParam[0] = new SqlParameter("@SalesFromID", SalesFromID);
                return DataAccess.Instance.GetTableFromDatabase("prcGetTertiarySales_Retailer", CommandType.StoredProcedure, SqlParam);

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        /* #CC01 Add Start */
        public DataSet TertiaryCount()/*Karam */
        {
            try
            {
                DataSet dsResult = new DataSet();
                SqlParameter[] SqlParam = new SqlParameter[6];
                SqlParam[0] = new SqlParameter("@Date", DateFrom);
                dsResult = DataAccess.Instance.GetDataSetFromDatabase("prcTertiaryCount", CommandType.StoredProcedure, SqlParam);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /* #CC01 Add Start */

    }
}
