using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DataAccess 
{
  public  class FinacialCalender: IDisposable
    {
        SqlParameter[] SqlParam;
        DataTable DtCalender;
        private string strCalenderYear,strQuarterName, strErrorXML, strError,strFortnightName;
        private DateTime? dtFortnightEndDate, dtFortnightStartDate;
        private int intFinacialCalenderID;
        public string CalenderYear
        {
            get { return strCalenderYear; }
            set { strCalenderYear = value; }
        }
        public string FortnightName
        {
            get { return strFortnightName; }
            set { strFortnightName = value; }
        }
        public int FinacialCalenderID
        {
            get { return intFinacialCalenderID; }
            set { intFinacialCalenderID = value; }
        }
        public string ErrorXML
        {
            get { return strErrorXML; }
            set { strErrorXML = value; }
        }
             public string Error
        {
            get { return strError; }
            set { strError = value; }
        }
             public DateTime? FortnightStartDate
             {
                 get { return dtFortnightStartDate; }
                 set { dtFortnightStartDate = value; }
             }
             public DateTime? FortnightEndDate
             {
                 get { return dtFortnightEndDate; }
                 set { dtFortnightEndDate = value; }
             }
             public string QuarterName
             {
                 get { return strQuarterName; }
                 set { strQuarterName = value; }
             }
             //public void UploadCalender(DataTable Dt)
             //{
             //    try
             //    {
             //        SqlParam = new SqlParameter[5];
             //        SqlParam[0] = new SqlParameter("@tvpFinCalender", SqlDbType.Structured);
             //        SqlParam[0].Value = Dt;
             //        SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
             //        SqlParam[1].Direction = ParameterDirection.Output;
             //        SqlParam[2] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 8000);
             //        SqlParam[2].Direction = ParameterDirection.Output;
             //        SqlParam[3] = new SqlParameter("@CalenderYear", strCalenderYear);
             //        SqlParam[4] = new SqlParameter("@YearEndDate", dtFortnightEndDate);
             //        DataAccess.Instance.DBInsertCommand("PrcUploadCalender", SqlParam);
             //        Error = SqlParam[1].Value.ToString();
             //        if (SqlParam[2].Value != DBNull.Value)
             //        {
             //            ErrorXML = SqlParam[2].Value.ToString();
             //        }
             //        else
             //        {
             //            ErrorXML = null;
             //        }

             //    }
             //    catch (Exception ex)
             //    {
             //        throw ex;
             //    }
             //}
             public void InsertCalender()
             {
                 try
                 {
                     SqlParam = new SqlParameter[7];
                     SqlParam[0] = new SqlParameter("@CalenderYear",strCalenderYear);
                     SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                     SqlParam[1].Direction = ParameterDirection.Output;
                     SqlParam[2] = new SqlParameter("@QuarterName", strQuarterName);
                     SqlParam[3] = new SqlParameter("@FortnightStartDate", dtFortnightStartDate);
                     SqlParam[4] = new SqlParameter("@FortnightEndDate", dtFortnightEndDate);
                     SqlParam[5] = new SqlParameter("@FinancialCalenderID", intFinacialCalenderID);
                     SqlParam[6] = new SqlParameter("@FortnightName", strFortnightName);
                     DataAccess.Instance.DBInsertCommand("PrcInsertCalender", SqlParam);
                     Error = SqlParam[1].Value.ToString();
                    
                 }
                 catch (Exception ex)
                 {
                     throw ex;
                 }
             }
             public Boolean ISCalenderExists()
             {
                 bool blResult;
                 SqlParam = new SqlParameter[2];
                 SqlParam[0] = new SqlParameter("@CalenderYear", strCalenderYear);
                 SqlParam[1] = new SqlParameter("@IsCalUnique", SqlDbType.Bit, 2);
                 SqlParam[1].Direction = ParameterDirection.Output;

                 DataAccess.Instance.getSingleValues("PrcIsuniqueCal", SqlParam);

                 blResult = Convert.ToBoolean(SqlParam[1].Value);

                 return blResult;
             }
             public DataTable GetCalenderDetail()
             {

                 SqlParam = new SqlParameter[5];
                 SqlParam[0] = new SqlParameter("@CalenderYear", strCalenderYear);
                 SqlParam[1] = new SqlParameter("@FinacialCalenderID", intFinacialCalenderID);
                 SqlParam[2] = new SqlParameter("@FortnightName", strFortnightName);
                 SqlParam[3] = new SqlParameter("@FortnightStartDate", dtFortnightStartDate);
                 SqlParam[4] = new SqlParameter("@FortnightEndDate", dtFortnightEndDate);
                 DtCalender = DataAccess.Instance.GetTableFromDatabase("PrcGetCalenderDetailByParameters", CommandType.StoredProcedure, SqlParam);

                 return DtCalender;
             }
        public void DeleteCalenderInfo()
             {

                 SqlParam = new SqlParameter[2];
                 SqlParam[0] = new SqlParameter("@FinacialCalenderID", intFinacialCalenderID);
                 SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                 SqlParam[1].Direction = ParameterDirection.Output;
                 DataAccess.Instance.DBInsertCommand("PrcDeleteCalenderInfo", SqlParam);
                 Error = SqlParam[1].Value.ToString();
                 
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

        ~FinacialCalender()
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
