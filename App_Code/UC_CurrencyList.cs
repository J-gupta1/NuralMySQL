using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Microsoft.ApplicationBlocks.Data;

namespace BussinessLogic
{
   public class UC_CurrencyList: UserControlsProperties
    {
       public String strConStr = System.Configuration.ConfigurationManager.ConnectionStrings["AppConString"].ConnectionString;
        public DataTable GetCurrency(Int16 Active)
        {
            DataTable dtResult = new DataTable();
            SqlParameter[] objSqlParam = new SqlParameter[2];
            objSqlParam[0] = new SqlParameter("@Out_Error", SqlDbType.NVarChar, 100);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new SqlParameter("@Active", Active);
            DataSet dsResult = SqlHelper.ExecuteDataset(strConStr, CommandType.StoredProcedure, "prcCurrencyMaster_Select", objSqlParam);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];

            Error = Convert.ToString(objSqlParam[0].Value);
            if (Error != string.Empty)
            {
                throw new ArgumentException(Error);
            }
            return dtResult;
        }
    }
}
