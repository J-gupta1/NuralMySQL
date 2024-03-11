#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*/
 * ================================================================================================
 * ================================================================================================
 * COPYRIGHT (c) 2016 Zed Axis Technologies (P) Ltd.
 * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART,
 * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
 * ================================================================================================
 * Created By : Sumit Maurya
 * Created On : 01-Sep-2016
 * Description : This is a copy of MasterData.cs
 * ================================================================================================
 * Change Log:
 * ------------- 
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 29-May-2017, Vijay Kumar Prajapati,#CC01,Add Method For Getlist For Region.
 * ====================================================================================================
 */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Data.SqlClient;
using System.Xml;
using MySql.Data.MySqlClient;
/*
 * 18 Mar 2015, Karam Chand Sharma, #CC01 , Create some new function to save , update and select tehsil data from master table
 * 23-May-2016, Sumit Maurya, #CC02, New Parameter supplied in method SelectStateInfo() to get state details according to parameter value.
 * 24-May-2016, Karam Chand Sharma, #CC03, As per client requriment tehsil will come under city now 
 * 22-May-2018,Vijay Kumar Prajapati,#CC04,Add method for zonemaster.
 * 20-July-2018,Vijay Kumar Prajapati,#CC05,Add method for BindRegion.
 * 05-May- 2023, Hema Thapa, #CC06, MySql connections
 */
namespace DataAccess
{
    public class MastersData : IDisposable
    {
        DataTable d1;
        string countryname; int countryid; short countrystatus; short countryselectionmode; short zonestatus/*#CC04 Added*/;

        int regionid; int activeid; string regionname; int stateregionid; /*Added #CC01*/

        SqlParameter[] SqlParam;
        MySqlParameter[] MySqlParam;
        public string error;
        Int32 IntResultCount = 0;

        # region country
        public Int32 Condition
        {
            get;
            set;
        }
        public Int32 UniqueID
        {
            get;
            set;
        }
        public Int16 Purpose
        {
            get;
            set;
        }

        public Int32 SalesChannelID
        {
            get;
            set;
        }
        public int CountryId
        {
            get { return countryid; }
            set { countryid = value; }
        }


        public string CountryName
        {
            get { return countryname; }
            set { countryname = value; }
        }

        public short CountryStatus
        {
            get { return countrystatus; }
            set { countrystatus = value; }

        }
        /*#CC04 Added*/
        public short ZoneStatus
        {
            get { return zonestatus; }
            set { zonestatus = value; }

        }
        /*#CC04 Added End*/
        public short CountrySelectionMode
        {
            get { return countryselectionmode; }
            set { countryselectionmode = value; }
        }
        /*Added #CC01*/
        public int RegionId
        {
            get { return regionid; }
            set { regionid = value; }
        }
        public string RegionName
        {
            get { return regionname; }
            set { regionname = value; }
        }
        public int State_Id
        {
            get { return stateid; }
            set { stateid = value; }
        }
        public string State_Name
        {
            get { return statename; }
            set { statename = value; }
        }
        public int StateRegionid
        {
            get { return stateregionid; }
            set { stateregionid = value; }
        }
        /*End #CC01*/
        /*#CC04 Added Started*/
        
        public int ZoneID { get; set; }
        public Int16 Active { get; set; }
        public int CountryID { get; set; }
        public int CompanyID { get; set; }
        

        /*#CC04 Added End*/
        public DataTable SelectCountryInfo()
        {
            try
            {
                /* #CC06 add start */
                MySqlParam = new MySqlParameter[4];
                MySqlParam[0] = new MySqlParameter("@p_countryid", countryid);
                MySqlParam[1] = new MySqlParameter("@p_countryname", countryname);
                MySqlParam[2] = new MySqlParameter("@p_selectionmode", countryselectionmode);
                MySqlParam[3] = new MySqlParameter("@p_CompanyId", CompanyId);
                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetCountryDetails", CommandType.StoredProcedure, MySqlParam);

                /* #CC06 add end */

                /* #CC06 comment start */
                //SqlParam = new SqlParameter[4];
                //SqlParam[0] = new SqlParameter("@countryid", countryid);
                //SqlParam[1] = new SqlParameter("@countryname", countryname);
                //SqlParam[2] = new SqlParameter("@selectionmode", countryselectionmode);
                //SqlParam[3] = new SqlParameter("@CompanyId", Cw"prcGetCountryDetails", CommandType.StoredProcedure, SqlParam);
                /* #CC06 comment start */
                return d1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet SelectSalesChannelAreainformation()
        {
            try
            {
                DataSet ds = new DataSet();
                MySqlParam = new MySqlParameter[4];
                MySqlParam[0] = new MySqlParameter("@p_countryid", countryid);
                MySqlParam[1] = new MySqlParameter("@p_countryname", countryname);
                MySqlParam[2] = new MySqlParameter("@p_selectionmode", countryselectionmode);
                MySqlParam[3] = new MySqlParameter("@p_SalesChannelId", SalesChannelID);
                ds = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcGetSalesChannelAreainformation", CommandType.StoredProcedure, MySqlParam);
                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void InsertCountryInfo()
        {
            try
            {


                stateid = 0;

                MySqlParam = new MySqlParameter[4];
                MySqlParam[0] = new MySqlParameter("@p_countryid", countryid);
                MySqlParam[1] = new MySqlParameter("@p_status", countrystatus);
                MySqlParam[2] = new MySqlParameter("@p_countryname", countryname);
                MySqlParam[3] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 200);
                MySqlParam[3].Direction = ParameterDirection.Output;
                int r = DataAccess.Instance.DBInsert_MySqlCommand("prcInsUpdCountry", MySqlParam);
                error = Convert.ToString(MySqlParam[3].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*Add #CC01*/
        public DataTable SelectRegionList()
        {
            MySqlParameter[] objSqlParam = new MySqlParameter[2];
            objSqlParam[0] = new MySqlParameter("@p_regionid", regionid);
            objSqlParam[1] = new MySqlParameter("@p_regionname", regionname);
            d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcRegionMaster_SelectList", CommandType.StoredProcedure, objSqlParam);
            return d1;
        }
       /*End*/

        /*Add #CC01*/
        public DataTable SelectStateList()
        {
            MySqlParameter[] objSqlParam = new MySqlParameter[2];
            objSqlParam[0] = new MySqlParameter("@p_stateid", stateid);
            objSqlParam[1] = new MySqlParameter("@p_statename", statename);
            d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetSchemeStateDetails", CommandType.StoredProcedure, objSqlParam);
            return d1;
        }
        /*End*/
        /*Add #CC01*/
        public DataTable SelectCityList()
        {
            MySqlParameter[] objSqlParam = new MySqlParameter[2];
            objSqlParam[0] = new MySqlParameter("@p_cityid", stateid);
            objSqlParam[1] = new MySqlParameter("@p_cityname", statename);
            d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetSchemeCityDetails", CommandType.StoredProcedure, objSqlParam);
            return d1;
        }
        /*End*/

        /*Add #CC01*/
        public DataSet SelectStateByRegionId()
        {
            DataSet ds = new DataSet();
            MySqlParameter[] objSqlParam = new MySqlParameter[3];
            objSqlParam[0] = new MySqlParameter("@p_stateid", stateid);
            objSqlParam[1] = new MySqlParameter("@p_statename", statename);
            objSqlParam[2] = new MySqlParameter("@p_regionid", stateregionid);
            ds = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcGetStateLocationDetails", CommandType.StoredProcedure, objSqlParam);

            return ds;
        }
        /*End*/
        /*#CC04 Added*/
        public DataTable SelectList()
        {
            DataTable dtResult = new DataTable();
            MySqlParameter[] objSqlParam = new MySqlParameter[3];
            objSqlParam[0] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 500);
            objSqlParam[0].Direction = ParameterDirection.Output;
            objSqlParam[1] = new MySqlParameter("@p_countryID", CountryID);
            objSqlParam[2] = new MySqlParameter("@p_active", Active);
            DataSet dsResult = DataAccess.Instance.GetDataSetFrom_MySqlDatabase("prcCountryMaster_SelectList", CommandType.StoredProcedure, objSqlParam);
         
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtResult = dsResult.Tables[0];
            return dtResult;
            
        }
        public static DataTable GetEnumbyTableName(string Filename, string TableName)
        {
            DataTable dt = new DataTable();
            using (DataSet ds = new DataSet())
            {
                string filename = HttpContext.Current.Server.MapPath("~/Assets/XML/" + Filename + ".xml");
                ds.ReadXml(filename);
                dt = ds.Tables[TableName];
                if (dt == null || dt.Rows.Count == 0)
                    return null;
            }
            try
            {
                dt = dt.Select("Active=1").CopyToDataTable();
                return dt;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /*#CC04 End*/
        /*#CC05 Added Started*/
        public DataTable SelectRegionInfo()
        {
            try
            {

                MySqlParam = new MySqlParameter[1];
                MySqlParam[0] = new MySqlParameter("@p_countryid", countryid);
                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetRegionName", CommandType.StoredProcedure, MySqlParam);
                return d1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable SelectZoneInfo()
        {
            try
            {

                MySqlParam = new MySqlParameter[1];
                MySqlParam[0] = new MySqlParameter("@p_countryid", countryid);
                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetZoneName", CommandType.StoredProcedure, MySqlParam);
                return d1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*#CC05 Added End*/
        #endregion

        #region excel sheetdata
        public string UploadSchemaXml
        {
            get;
            set;
        }

        public int TableID
        {
            get;
            set;
        }

        public string ErrorXmlDetail
        {
            get;
            set;
        }


        public void InsertUploadSchema()
        {
            try
            {
                SqlParam = new SqlParameter[4];
                SqlParam[0] = new SqlParameter("@UploadSchemaXML", SqlDbType.Xml);
                SqlParam[0].Value = new System.Data.SqlTypes.SqlXml(new XmlTextReader(UploadSchemaXml, XmlNodeType.Document, null));
                SqlParam[0].Direction = ParameterDirection.InputOutput;
                SqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 200);
                SqlParam[1].Direction = ParameterDirection.Output;
                SqlParam[3] = new SqlParameter("@ErrorXML", SqlDbType.Xml, 2);
                SqlParam[3].Direction = ParameterDirection.Output;
                SqlParam[2] = new SqlParameter("@tableno", TableID);
                DataAccess.Instance.DBInsertCommand("prcInsertMasters", SqlParam);
                if (SqlParam[3].Value.ToString() != "")
                {
                    ErrorXmlDetail = (SqlParam[3].Value).ToString();
                }
                else
                {
                    ErrorXmlDetail = null;
                }
                if (SqlParam[1].Value != DBNull.Value && SqlParam[1].Value.ToString() != "")
                {
                    error = (SqlParam[1].Value).ToString();
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }






        #endregion


        #region State


        int stateid; string statename; string statecode; string statepriceeffdate; int statestatus; int statepricelistid;

        int statepreviouspricelistid; int stateselectionmode; int statecountryid;


        public int StateId
        {
            get { return stateid; }
            set { stateid = value; }
        }

        public string StateName
        {
            get { return statename; }
            set { statename = value; }
        }

        public string StateCode
        {
            get { return statecode; }
            set { statecode = value; }
        }
        public int StateStatus
        {
            get { return statestatus; }
            set { statestatus = value; }

        }

        public string StatePriceEffDate
        {
            get { return statepriceeffdate; }
            set { statepriceeffdate = value; }
        }
        

        public int StatePriceListId
        {
            get { return statepricelistid; }
            set { statepricelistid = value; }

        }
        public int StatePreviousPriceListId
        {
            get { return statepreviouspricelistid; }
            set { statepreviouspricelistid = value; }

        }
        public int StateSelectionMode
        {
            get { return stateselectionmode; }
            set { stateselectionmode = value; }
        }

        public int StateCountryid
        {
            get { return statecountryid; }
            set { statecountryid = value; }
        }




        public DataTable SelectStateInfo()
        {
            try
            {
                /* #CC06 add start */
                MySqlParam = new MySqlParameter[9];
                MySqlParam[0] = new MySqlParameter("@p_stateid", stateid);
                MySqlParam[1] = new MySqlParameter("@p_statename", statename);
                MySqlParam[2] = new MySqlParameter("@p_statecode", statecode);
                MySqlParam[3] = new MySqlParameter("@p_pricelstid", statepricelistid);
                MySqlParam[4] = new MySqlParameter("@p_selectionmode", stateselectionmode);
                MySqlParam[5] = new MySqlParameter("@p_countryid", statecountryid);
                MySqlParam[6] = new MySqlParameter("@p_SalesChannelId", SalesChannelID);
                MySqlParam[7] = new MySqlParameter("@p_CompanyId", CompanyId);
                MySqlParam[8] = new MySqlParameter("@p_out_error", MySqlDbType.VarChar, 200);
                MySqlParam[8].Direction = ParameterDirection.Output;
                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetStateDetails", CommandType.StoredProcedure, MySqlParam);
                error = Convert.ToString(MySqlParam[8].Value);

                /* #CC06 add end*/

                /* #CC06 comment start */
                //SqlParam = new SqlParameter[8];
                //SqlParam[0] = new SqlParameter("@stateid", stateid);
                //SqlParam[1] = new SqlParameter("@statename", statename);
                //SqlParam[2] = new SqlParameter("@statecode", statecode);
                //SqlParam[3] = new SqlParameter("@pricelstid", statepricelistid);
                //SqlParam[4] = new SqlParameter("@selectionmode", stateselectionmode);
                //SqlParam[5] = new SqlParameter("@countryid", statecountryid);
                //SqlParam[6] = new SqlParameter("@SalesChannelID", SalesChannelID);
                //SqlParam[7] = new SqlParameter("@CompanyId", CompanyId);/*#CC06 Added*/
                //d1 = DataAccess.Instance.GetTableFromDatabase("prcGetStateDetails", CommandType.StoredProcedure, SqlParam);
                /* #CC06 comment end*/



                return d1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataTable SelectAllStateInfo()
        {
            try
            {
                stateid = 0;
                statename = "";
                statecode = "";
                statepricelistid = 0;
                stateselectionmode = 1;
                MySqlParam = new MySqlParameter[6];
                MySqlParam[0] = new MySqlParameter("@p_stateid", stateid);
                MySqlParam[1] = new MySqlParameter("@p_statename", statename);
                MySqlParam[2] = new MySqlParameter("@p_statecode", statecode);
                MySqlParam[3] = new MySqlParameter("@p_pricelstid", statepricelistid);
                MySqlParam[4] = new MySqlParameter("@p_selectionmode", stateselectionmode);
                MySqlParam[5] = new MySqlParameter("@p_countryid", statecountryid);
                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetStateDetails", CommandType.StoredProcedure, MySqlParam);

                return d1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public void InsertStateInfo()
        {
            try
            {

                string outex = "";
                stateid = 0;

                MySqlParam = new MySqlParameter[9];
                MySqlParam[0] = new MySqlParameter("@p_stateid", stateid);
                MySqlParam[1] = new MySqlParameter("@p_status", statestatus);
                MySqlParam[2] = new MySqlParameter("@p_statename", statename);
                MySqlParam[3] = new MySqlParameter("@p_statecode", statecode);
                MySqlParam[4] = new MySqlParameter("@p_pricelstid", statepricelistid);
                MySqlParam[5] = new MySqlParameter("@p_previouspricelistid", statepreviouspricelistid);
                MySqlParam[6] = new MySqlParameter("@p_priceeffdt", statepriceeffdate);
                MySqlParam[7] = new MySqlParameter("@p_countryid", statecountryid);
                MySqlParam[8] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 200);
                MySqlParam[8].Direction = ParameterDirection.Output;
                int r = DataAccess.Instance.DBInsert_MySqlCommand("prcInsUpdState", MySqlParam);
                error = Convert.ToString(MySqlParam[8].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void UpdateStateInfo()
        {
            try
            {

                string outex = "";


                MySqlParam = new MySqlParameter[9];
                MySqlParam[0] = new MySqlParameter("@p_stateid", stateid);
                MySqlParam[1] = new MySqlParameter("@p_status", statestatus);
                MySqlParam[2] = new MySqlParameter("@p_statename", statename);
                MySqlParam[3] = new MySqlParameter("@p_statecode", statecode);
                MySqlParam[4] = new MySqlParameter("@p_pricelstid", statepricelistid);
                MySqlParam[5] = new MySqlParameter("@p_previouspricelistid", statepreviouspricelistid);
                MySqlParam[6] = new MySqlParameter("@p_priceeffdt", statepriceeffdate);
                MySqlParam[7] = new MySqlParameter("@p_countryid", statecountryid);
                MySqlParam[8] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 200);
                MySqlParam[8].Direction = ParameterDirection.Output;
                int r = DataAccess.Instance.DBInsert_MySqlCommand("prcInsUpdState", MySqlParam);
                error = Convert.ToString(MySqlParam[8].Value);
                //if (error != null)
                //{
                //    throw new Exception(error);
                //}

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        #endregion
        #region NewStateMaster
        public Int32 InsertStatePriceLists()
        {
            try
            {
                MySqlParam = new MySqlParameter[8];
                MySqlParam[0] = new MySqlParameter("@p_StateID", stateid);
                MySqlParam[1] = new MySqlParameter("@p_PricelstID", statepricelistid);
                MySqlParam[2] = new MySqlParameter("@p_PricelstDate", StatePriceEffDate);
                //MySqlParam[2] = new MySqlParameter("@p_PricelstDate", statepriceeffdate);
                MySqlParam[3] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 200);
                MySqlParam[3].Direction = ParameterDirection.Output;
                MySqlParam[4] = new MySqlParameter("@p_Out_Param", MySqlDbType.Int16, 2);
                MySqlParam[4].Direction = ParameterDirection.Output;
                MySqlParam[5] = new MySqlParameter("@p_Purpose", Purpose);
                MySqlParam[6] = new MySqlParameter("@p_PriceListChangeLogID", UniqueID);
                MySqlParam[7] = new MySqlParameter("@p_CompanyId", CompanyId);
                int r = DataAccess.Instance.DBInsert_MySqlCommand("prcInsUpdStatePriceList", MySqlParam);
                error = Convert.ToString(MySqlParam[3].Value);
                IntResultCount = Convert.ToInt32(MySqlParam[4].Value);
                return IntResultCount;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsertStateInfoVer2()
        {
            try
            {

                string outex = "";
                stateid = 0;

                MySqlParam = new MySqlParameter[11];
                MySqlParam[0] = new MySqlParameter("@p_stateid", stateid);
                MySqlParam[1] = new MySqlParameter("@p_status", StateStatus);
                MySqlParam[2] = new MySqlParameter("@p_statename", statename);
                MySqlParam[3] = new MySqlParameter("@p_statecode", statecode);
                MySqlParam[4] = new MySqlParameter("@p_pricelstid", statepricelistid);
                MySqlParam[5] = new MySqlParameter("@p_previouspricelistid", statepreviouspricelistid);
                MySqlParam[6] = new MySqlParameter("@p_priceeffdt", statepriceeffdate);
                MySqlParam[7] = new MySqlParameter("@p_countryid", statecountryid);
                MySqlParam[8] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 200);
                MySqlParam[8].Direction = ParameterDirection.Output;
                MySqlParam[9] = new MySqlParameter("@p_RegionId", RegionId);/*#CC05 Added*/
                MySqlParam[10] = new MySqlParameter("@p_Companyid", CompanyId);
                int r = DataAccess.Instance.DBInsert_MySqlCommand("prcInsUpdStateVer2", MySqlParam);
                error = Convert.ToString(MySqlParam[8].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateStateInfoVer2()
        {
            try
            {

                string outex = "";


                MySqlParam = new MySqlParameter[10];
                MySqlParam[0] = new MySqlParameter("@p_stateid", stateid);
                MySqlParam[1] = new MySqlParameter("@p_status", statestatus);
                MySqlParam[2] = new MySqlParameter("@p_statename", statename);
                MySqlParam[3] = new MySqlParameter("@p_statecode", statecode);
                MySqlParam[4] = new MySqlParameter("@p_pricelstid", statepricelistid);
                MySqlParam[5] = new MySqlParameter("@p_previouspricelistid", statepreviouspricelistid);
                MySqlParam[6] = new MySqlParameter("@p_priceeffdt", statepriceeffdate);
                MySqlParam[7] = new MySqlParameter("@p_countryid", statecountryid);
                MySqlParam[8] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 200);
                MySqlParam[8].Direction = ParameterDirection.Output;
                MySqlParam[9] = new MySqlParameter("@p_RegionId", RegionId);
                int r = DataAccess.Instance.DBInsert_MySqlCommand("prcInsUpdStateVer2", MySqlParam);
                error = Convert.ToString(MySqlParam[8].Value);
                //if (error != null)
                //{
                //    throw new Exception(error);
                //}

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable SelectStateInfoVer2()
        {
            try
            {
                MySqlParam = new MySqlParameter[8];
                MySqlParam[0] = new MySqlParameter("@p_stateid", stateid);
                MySqlParam[1] = new MySqlParameter("@p_statename", statename);
                MySqlParam[2] = new MySqlParameter("@p_statecode", statecode);
                MySqlParam[3] = new MySqlParameter("@p_pricelstid", statepricelistid);
                MySqlParam[4] = new MySqlParameter("@p_selectionmode", stateselectionmode);
                MySqlParam[5] = new MySqlParameter("@p_countryid", statecountryid);
                MySqlParam[6] = new MySqlParameter("@p_UniquePriceListId", UniqueID);
                MySqlParam[7] = new MySqlParameter("@p_Companyid", CompanyId);
                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetStateDetailsVer2", CommandType.StoredProcedure, MySqlParam);


                return d1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Int32 DeleteStatePriceListInfo()
        {
            try
            {
                MySqlParam = new MySqlParameter[5];
                MySqlParam[0] = new MySqlParameter("@p_Condition", Condition);
                MySqlParam[1] = new MySqlParameter("@p_UniqueID", UniqueID);
                MySqlParam[2] = new MySqlParameter("@p_Out_Param", MySqlDbType.Int32);
                MySqlParam[2].Direction = ParameterDirection.Output;
                MySqlParam[3] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 500);
                MySqlParam[3].Direction = ParameterDirection.Output;
                MySqlParam[4] = new MySqlParameter("@p_StateID", StateId);
                int r = DataAccess.Instance.DBInsert_MySqlCommand("prcDeleteStatePriceListInfo", MySqlParam);
                error = Convert.ToString(MySqlParam[3].Value);
                return Convert.ToInt32(MySqlParam[2].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public Int32 SelectStatePriceListInfo()
        //{
        //    try
        //    {
        //        SqlParam = new SqlParameter[9];
        //        SqlParam[0] = new SqlParameter("@stateid", stateid);
        //        SqlParam[2] = new SqlParameter("@Condition", Condition);
        //        SqlParam[3] = new SqlParameter("@pricelstid", statepricelistid);
        //        SqlParam[6] = new SqlParameter("@UniqueID", UniqueID);
        //        int r = DataAccess.Instance.DBInsertCommand("prcSelectStatePriceListInfo", SqlParam);
        //        error = Convert.ToString(SqlParam[8].Value);
        //        return Convert.ToInt32(SqlParam[5].Value);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #endregion

        # region  District


        int districtid; string districtname; string districtcode;
        int districtstateid; int districtstatus; int districtselectionmode; int districtountryid;


        public int DistrictId
        {
            get { return districtid; }
            set { districtid = value; }
        }

        public string DistrictName
        {
            get { return districtname; }
            set { districtname = value; }
        }

        public string DistrictCode
        {
            get { return districtcode; }
            set { districtcode = value; }
        }
        public int DistrictStatus
        {
            get { return districtstatus; }
            set { districtstatus = value; }

        }

        public int DistrictStateId
        {
            get { return districtstateid; }
            set { districtstateid = value; }
        }


        public int DistrictCountryId
        {
            get { return districtountryid; }
            set { districtountryid = value; }
        }

        public int DistrictSelectionMode
        {
            get { return districtselectionmode; }
            set { districtselectionmode = value; }
        }


        public DataTable SelectAllDistrictInfo()
        {
            try
            {
                districtid = 0;
                districtname = "";
                districtstateid = 0;
                districtselectionmode = 1;
                MySqlParam = new MySqlParameter[6];
                MySqlParam[0] = new MySqlParameter("@p_districtid", districtid);
                MySqlParam[1] = new MySqlParameter("@p_districtname", districtname);
                MySqlParam[2] = new MySqlParameter("@p_districtstateid", districtstateid);
                MySqlParam[3] = new MySqlParameter("@p_districtcountryid", districtountryid);
                MySqlParam[4] = new MySqlParameter("@p_selectionmode", districtselectionmode);
                MySqlParam[5] = new MySqlParameter("@p_districtcode", districtcode);
                MySqlParam[6] = new MySqlParameter("@p_CompanyId", CompanyId);



                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetDistrictDetails", CommandType.StoredProcedure, MySqlParam);

                return d1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataTable SelectDistrictInfo()
        {
            try
            {
                /* #CC06 add start */
                
                MySqlParam = new MySqlParameter[7];
                MySqlParam[0] = new MySqlParameter("@p_districtid", districtid);
                MySqlParam[1] = new MySqlParameter("@p_districtname", districtname);
                MySqlParam[2] = new MySqlParameter("@p_districtstateid", districtstateid);
                MySqlParam[3] = new MySqlParameter("@p_districtcountryid", districtountryid);
                MySqlParam[4] = new MySqlParameter("@p_selectionmode", districtselectionmode);
                MySqlParam[5] = new MySqlParameter("@p_districtcode", districtcode);
                MySqlParam[6] = new MySqlParameter("@p_CompanyId", CompanyId);

                              
                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetDistrictDetails", CommandType.StoredProcedure, MySqlParam);
                

                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void InsertDistrictInfo()
        {
            try
            {
                string outex = "";
                districtid = 0;
                MySqlParam = new MySqlParameter[6];
                MySqlParam[0] = new MySqlParameter("@p_districtstateid", districtstateid);
                MySqlParam[1] = new MySqlParameter("@p_status", districtstatus);
                MySqlParam[2] = new MySqlParameter("@p_districtname", districtname);
                MySqlParam[3] = new MySqlParameter("@p_districtcode", districtcode);
                MySqlParam[4] = new MySqlParameter("@p_districtid", districtid);
                //SqlParam[5] = new SqlParameter("@districtcountryid", districtountryid);
                MySqlParam[5] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 200);
                MySqlParam[5].Direction = ParameterDirection.Output;
                int r = DataAccess.Instance.DBInsert_MySqlCommand("prcInsUpdDistrict", MySqlParam);
                error = Convert.ToString(MySqlParam[5].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateDistrictInfo()
        {
            try
            {

                string outex = "";


                MySqlParam = new MySqlParameter[6];
                MySqlParam[0] = new MySqlParameter("@p_districtid", districtid);
                MySqlParam[1] = new MySqlParameter("@p_districtstateid", districtstateid);
                MySqlParam[2] = new MySqlParameter("@p_status", districtstatus);
                MySqlParam[3] = new MySqlParameter("@p_districtname", districtname);
                MySqlParam[4] = new MySqlParameter("@p_districtcode", districtcode);
                //SqlParam[5] = new SqlParameter("@districtcountryid", districtountryid);
                MySqlParam[5] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 200);
                MySqlParam[5].Direction = ParameterDirection.Output;
                int r = DataAccess.Instance.DBInsert_MySqlCommand("prcInsUpdDistrict", MySqlParam);
                error = Convert.ToString(MySqlParam[5].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        #endregion

        #region City



        int cityid; string cityname; string citycode; int citystateid;
        int citydistrictid; int citystatus; int cityselectionmode; int citycountryid;


        public int CityId
        {
            get { return cityid; }
            set { cityid = value; }
        }

        public string CityName
        {
            get { return cityname; }
            set { cityname = value; }
        }

        public string CityCode
        {
            get { return citycode; }
            set { citycode = value; }
        }
        public int CityStatus
        {
            get { return citystatus; }
            set { citystatus = value; }

        }

        public int CityStateId
        {
            get { return citystateid; }
            set { citystateid = value; }
        }


        public int CityCountryId
        {
            get { return citycountryid; }
            set { citycountryid = value; }
        }


        public int CityDistrictId
        {
            get { return citydistrictid; }
            set { citydistrictid = value; }
        }

        public int CitySelectionMode
        {
            get { return cityselectionmode; }
            set { cityselectionmode = value; }
        }
        /*#CC01  START ADDED*/
        public int tehsillselectionmode
        {
            get;
            set;
        }
        public string tehsillcode
        {
            get;
            set;
        }
        public int tehsillcountryid
        {
            get;
            set;
        }
        public string tehsillname
        {
            get;
            set;
        }
        public int tehsillid
        {
            get;
            set;
        }
        public int tehsilldistrictid
        {
            get;
            set;
        }
        /*#CC03 START ADDED */
        public int tehsilCityId
        {
            get;
            set;
        }
        /*#CC03 START END */
        public int tehsillstateid
        {
            get;
            set;
        }
        public int tehsillstatus
        {
            get;
            set;
        }


        public DataTable SelectTahsillInfo()
        {
            try
            {

                MySqlParam = new MySqlParameter[9];
                MySqlParam[0] = new MySqlParameter("@p_TehsillStateId", tehsillstateid);
                MySqlParam[1] = new MySqlParameter("@p_TehsillDistrictId", tehsilldistrictid);
                MySqlParam[2] = new MySqlParameter("@p_TehsillId", tehsillid);
                MySqlParam[3] = new MySqlParameter("@p_TehsillName", tehsillname);
                MySqlParam[4] = new MySqlParameter("@p_TehsillCode", tehsillcode);
                MySqlParam[5] = new MySqlParameter("@p_selectionmode", tehsillselectionmode);
                MySqlParam[6] = new MySqlParameter("@p_countryid", tehsillcountryid);
                MySqlParam[7] = new MySqlParameter("@p_cityid", tehsilCityId);
                MySqlParam[8] = new MySqlParameter("@p_CompanyId", CompanyId);/*#CC06 Added*/
                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetTahsillDetails", CommandType.StoredProcedure, MySqlParam);
                return d1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void InsertTehsilInfo()
        {
            try
            {
                MySqlParam = new MySqlParameter[8];
                MySqlParam[0] = new MySqlParameter("@p_tehsilstateid", tehsillstateid);
                MySqlParam[1] = new MySqlParameter("@p_status", tehsillstatus);
                MySqlParam[2] = new MySqlParameter("@p_tehsilcode", tehsillcode);
                MySqlParam[3] = new MySqlParameter("@p_tehsilname", tehsillname);
                MySqlParam[4] = new MySqlParameter("@p_tehsilid", tehsillid);
                MySqlParam[5] = new MySqlParameter("@p_tehsildistrictid", tehsilldistrictid);
                MySqlParam[6] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 200);
                MySqlParam[6].Direction = ParameterDirection.Output;
                MySqlParam[7] = new MySqlParameter("@p_tehsilCityid", tehsilCityId);
                int r = DataAccess.Instance.DBInsert_MySqlCommand("prcInsUpdTehsill", MySqlParam);
                error = Convert.ToString(MySqlParam[6].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateTehsilInfo()
        {
            try
            {
                MySqlParam = new MySqlParameter[8];
                MySqlParam[0] = new MySqlParameter("@p_tehsilid", tehsillid);
                MySqlParam[1] = new MySqlParameter("@p_status", tehsillstatus);
                MySqlParam[2] = new MySqlParameter("@p_tehsilname", tehsillname);
                MySqlParam[3] = new MySqlParameter("@p_tehsilcode", tehsillcode);
                MySqlParam[4] = new MySqlParameter("@p_tehsilstateid", tehsillstateid);
                MySqlParam[5] = new MySqlParameter("@p_tehsildistrictid", tehsilldistrictid);
                MySqlParam[6] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 200);
                MySqlParam[6].Direction = ParameterDirection.Output;
                MySqlParam[7] = new MySqlParameter("@p_tehsilCityid", tehsilCityId);
                int r = DataAccess.Instance.DBInsert_MySqlCommand("prcInsUpdTehsill", MySqlParam);
                error = Convert.ToString(MySqlParam[6].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable SelectCityInfoTehsilWise()
        {
            try
            {
                /* #CC06 add start */
                MySqlParam = new MySqlParameter[9];
                MySqlParam[0] = new MySqlParameter("@p_citystateid", citystateid);
                MySqlParam[1] = new MySqlParameter("@p_citydistrictid", citydistrictid);
                MySqlParam[2] = new MySqlParameter("@p_cityid", cityid);
                MySqlParam[3] = new MySqlParameter("@p_cityname", cityname);
                MySqlParam[4] = new MySqlParameter("@p_citycode", citycode);
                MySqlParam[5] = new MySqlParameter("@p_selectionmode", cityselectionmode);
                MySqlParam[6] = new MySqlParameter("@p_countryid", citycountryid);
                MySqlParam[7] = new MySqlParameter("@p_TehsilId", tehsillid);
                MySqlParam[8] = new MySqlParameter("@p_out_error", MySqlDbType.VarChar, 200);
                MySqlParam[8].Direction = ParameterDirection.Output;
                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetCityDetails_ForTehsil", CommandType.StoredProcedure, MySqlParam);
                error = Convert.ToString(MySqlParam[8].Value);
                /* #CC06 add end*/

                /* #CC06 comment start */
                //SqlParam = new SqlParameter[8];
                //SqlParam[0] = new SqlParameter("@citystateid", citystateid);
                //SqlParam[1] = new SqlParameter("@citydistrictid", citydistrictid);
                //SqlParam[2] = new SqlParameter("@cityid", cityid);
                //SqlParam[3] = new SqlParameter("@cityname", cityname);
                //SqlParam[4] = new SqlParameter("@citycode", citycode);
                //SqlParam[5] = new SqlParameter("@selectionmode", cityselectionmode);
                //SqlParam[6] = new SqlParameter("@countryid", citycountryid);
                //SqlParam[7] = new SqlParameter("@TehsilId", tehsillid);
                //d1 = DataAccess.Instance.GetTableFromDatabase("prcGetCityDetails_ForTehsil", CommandType.StoredProcedure, SqlParam);
                /* #CC06 comment end*/

                return d1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*#CC01  START END*/
        public DataTable SelectCityInfo()
        {
            try
            {
                /* #CC06 add start */
                MySqlParam = new MySqlParameter[9];
                MySqlParam[0] = new MySqlParameter("@p_cityid", cityid);
                MySqlParam[1] = new MySqlParameter("@p_citycode", citycode);
                MySqlParam[2] = new MySqlParameter("@p_cityname", cityname);
                MySqlParam[3] = new MySqlParameter("@p_citystateid", citystateid);
                MySqlParam[4] = new MySqlParameter("@p_citydistrictid", citydistrictid);
                MySqlParam[5] = new MySqlParameter("@p_selectionmode", cityselectionmode);
                MySqlParam[6] = new MySqlParameter("@p_countryid", citycountryid);
                MySqlParam[7] = new MySqlParameter("@p_debugmode", 0);
                MySqlParam[8] = new MySqlParameter("@p_CompanyId", CompanyId);
                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetCityDetails", CommandType.StoredProcedure, MySqlParam);
                error = Convert.ToString(MySqlParam[8].Value);
                /* #CC06 add end*/

                /* #CC06 comment start */
                //SqlParam = new SqlParameter[8];
                //SqlParam[0] = new SqlParameter("@citystateid", citystateid);
                //SqlParam[1] = new SqlParameter("@citydistrictid", citydistrictid);
                //SqlParam[2] = new SqlParameter("@cityid", cityid);
                //SqlParam[3] = new SqlParameter("@cityname", cityname);
                //SqlParam[4] = new SqlParameter("@citycode", citycode);
                //SqlParam[5] = new SqlParameter("@selectionmode", cityselectionmode);
                //SqlParam[6] = new SqlParameter("@countryid", citycountryid);
                //SqlParam[7] = new SqlParameter("@CompanyId", CompanyId);/*#CC06 Added*/
                //d1 = DataAccess.Instance.GetTableFromDatabase("prcGetCityDetails", CommandType.StoredProcedure, SqlParam);
                /* #CC06 comment end*/


                return d1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable SelectAllCityInfo()
        {
            try
            {
                cityid = 0;
                cityname = "";
                citycode = "";
                citystateid = 0;
                citydistrictid = 0;
                cityselectionmode = 1;
                MySqlParam = new MySqlParameter[9];
                MySqlParam[0] = new MySqlParameter("@p_cityid", cityid);
                MySqlParam[1] = new MySqlParameter("@p_citycode", citycode);
                MySqlParam[2] = new MySqlParameter("@p_cityname", cityname);
                MySqlParam[3] = new MySqlParameter("@p_citystateid", citystateid);
                MySqlParam[4] = new MySqlParameter("@p_citydistrictid", citydistrictid);
                MySqlParam[5] = new MySqlParameter("@p_selectionmode", cityselectionmode);
                MySqlParam[6] = new MySqlParameter("@p_countryid", citycountryid);
                MySqlParam[7] = new MySqlParameter("@p_debugmode", 0);
                MySqlParam[8] = new MySqlParameter("@p_CompanyId", CompanyId);
                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetCityDetails", CommandType.StoredProcedure, MySqlParam);

                return d1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void InsertCityInfo()
        {
            try
            {
                string outex = "";

                int cityid = 0;

                MySqlParam = new MySqlParameter[8];
                MySqlParam[0] = new MySqlParameter("@p_citystateid", citystateid);
                MySqlParam[1] = new MySqlParameter("@p_status", citystatus);
                MySqlParam[2] = new MySqlParameter("@p_citycode", citycode);
                MySqlParam[3] = new MySqlParameter("@p_cityname", cityname);
                MySqlParam[4] = new MySqlParameter("@p_cityid", cityid);
                MySqlParam[5] = new MySqlParameter("@p_citydistrictid", citydistrictid);
                MySqlParam[6] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 200);
                MySqlParam[6].Direction = ParameterDirection.Output;
                int r = DataAccess.Instance.DBInsert_MySqlCommand("prcInsUpdCity", MySqlParam);
                error = Convert.ToString(SqlParam[6].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCityInfo()
        {
            try
            {

                string outex = "";


                MySqlParam = new MySqlParameter[8];
                MySqlParam[0] = new MySqlParameter("@p_cityid", cityid);
                MySqlParam[1] = new MySqlParameter("@p_status", citystatus);
                MySqlParam[2] = new MySqlParameter("@p_cityname", cityname);
                MySqlParam[3] = new MySqlParameter("@p_citycode", citycode);
                MySqlParam[4] = new MySqlParameter("@p_citystateid", citystateid);
                MySqlParam[5] = new MySqlParameter("@p_citydistrictid", citydistrictid);
                MySqlParam[6] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 200);
                MySqlParam[6].Direction = ParameterDirection.Output;
                int r = DataAccess.Instance.DBInsert_MySqlCommand("prcInsUpdCity", MySqlParam);
                error = Convert.ToString(SqlParam[6].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion



        #region area

        int areaid; string areaname; string areacode; int areastateid; int areacountryid;
        int areastatus; int areacityid; int areadistrictid; int areaselectionmode;

        public int AreaId
        {
            get { return areaid; }
            set { areaid = value; }
        }

        public string AreaName
        {
            get { return areaname; }
            set { areaname = value; }
        }

        public string AreaCode
        {
            get { return areacode; }
            set { areacode = value; }
        }
        public int AreaStatus
        {
            get { return areastatus; }
            set { areastatus = value; }

        }

        public int AreaStateId
        {
            get { return areastateid; }
            set { areastateid = value; }
        }

        public int AreaCityId
        {
            get { return areacityid; }
            set { areacityid = value; }
        }

        public int AreaDistrictId
        {
            get { return areadistrictid; }
            set { areadistrictid = value; }
        }

        public int AreaSelectionMode
        {
            get { return areaselectionmode; }
            set { areaselectionmode = value; }
        }

        public int AreaCountryId
        {
            get { return areacountryid; }
            set { areacountryid = value; }
        }

        public DataTable SelectAreaInfo()
        {
            try
            {

                MySqlParam = new MySqlParameter[9];
                MySqlParam[0] = new MySqlParameter("@p_areastateid", areastateid);
                MySqlParam[1] = new MySqlParameter("@p_areadistrictid", areadistrictid);
                MySqlParam[2] = new MySqlParameter("@p_areacityid", areacityid);
                MySqlParam[3] = new MySqlParameter("@p_areaname", areaname);
                MySqlParam[4] = new MySqlParameter("@p_areacode", areacode);
                MySqlParam[5] = new MySqlParameter("@p_areaid", areaid);
                MySqlParam[6] = new MySqlParameter("@p_countryid", areacountryid);
                MySqlParam[7] = new MySqlParameter("@p_selectionmode", areaselectionmode);
                MySqlParam[8] = new MySqlParameter("@p_areatehsilid", tehsillid);
                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetAreaDetails", CommandType.StoredProcedure, MySqlParam);
                return d1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectAllAreaInfo()
        {
            try
            {
                areaid = 0;
                areaname = "";
                areacode = "";
                areastateid = 0;
                areacityid = 0;
                areadistrictid = 0;
                areaselectionmode = 1;
                MySqlParam = new MySqlParameter[8];
                MySqlParam[0] = new MySqlParameter("@p_areacityid", areacityid);
                MySqlParam[1] = new MySqlParameter("@p_areacode", areacode);
                MySqlParam[2] = new MySqlParameter("@p_areadistrictid", areadistrictid);
                MySqlParam[3] = new MySqlParameter("@p_areaname", areaname);
                MySqlParam[4] = new MySqlParameter("@p_areastateid", areastateid);
                MySqlParam[5] = new MySqlParameter("@p_areaid", areaid);
                MySqlParam[6] = new MySqlParameter("@p_countryid", areacountryid);
                MySqlParam[7] = new MySqlParameter("@p_selectionmode", areaselectionmode);
                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetAreaDetails", CommandType.StoredProcedure, MySqlParam);
                return d1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertAreaInfo()
        {
            try
            {

                string outex = "";
                int areaid = 0;

                MySqlParam = new MySqlParameter[8];
                MySqlParam[0] = new MySqlParameter("@p_areastateid", areastateid);
                MySqlParam[1] = new MySqlParameter("@p_status", areastatus);
                MySqlParam[2] = new MySqlParameter("@p_areacode", areacode);
                MySqlParam[3] = new MySqlParameter("@p_areaname", areaname);

                MySqlParam[4] = new MySqlParameter("@p_areacityid", areacityid);


                MySqlParam[5] = new MySqlParameter("@p_areaid", areaid);


                MySqlParam[6] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 200);
                MySqlParam[6].Direction = ParameterDirection.Output;
                MySqlParam[7] = new MySqlParameter("@p_tehsilid", tehsillid);
                int r = DataAccess.Instance.DBInsert_MySqlCommand("prcInsUpdArea", MySqlParam);
                error = Convert.ToString(MySqlParam[6].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void UpdateAreaInfo()
        {
            try
            {

                string outex = "";


                MySqlParam = new MySqlParameter[8];
                MySqlParam[0] = new MySqlParameter("@p_areacityid", areacityid);
                MySqlParam[1] = new MySqlParameter("@p_status", areastatus);
                MySqlParam[2] = new MySqlParameter("@p_areaname", areaname);
                MySqlParam[3] = new MySqlParameter("@p_areacode", areacode);
                MySqlParam[4] = new MySqlParameter("@p_areastateid", areastateid);
                MySqlParam[5] = new MySqlParameter("@p_areaid", areaid);

                MySqlParam[6] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 200);
                MySqlParam[6].Direction = ParameterDirection.Output;
                MySqlParam[7] = new MySqlParameter("@p_tehsilid", tehsillid);
                int r = DataAccess.Instance.DBInsert_MySqlCommand("prcInsUpdArea", MySqlParam);
                error = Convert.ToString(MySqlParam[6].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        #endregion

        #region Category
        int categoryid; string categoryname; bool status; int categoryselectionmode;

        public int CategoryID
        {
            get { return categoryid; }
            set { categoryid = value; }
        }

        public string CategoryName
        {
            get { return categoryname; }
            set { categoryname = value; }
        }
        public bool Status
        {
            get { return status; }
            set { status = value; }
        }

        public int CategorySelectionMode
        {
            get { return categoryselectionmode; }
            set { categoryselectionmode = value; }

        }

        public DataTable SelectAllLocalityInfo()
        {
            try
            {

                /* #CC06 add start */
                MySqlParam = new MySqlParameter[6];
                MySqlParam[0] = new MySqlParameter("@p_stateid", StateId);
                MySqlParam[1] = new MySqlParameter("@p_DistrictId", DistrictId);
                MySqlParam[2] = new MySqlParameter("@p_CityId", CityId);
                MySqlParam[3] = new MySqlParameter("@p_countryid", CountryId);
                MySqlParam[4] = new MySqlParameter("@p_tehsillid", tehsillid);
                MySqlParam[5] = new MySqlParameter("@p_CompanyId", CompanyId);
                //MySqlParam[6] = new MySqlParameter("@p_out_error", MySqlDbType.VarChar, 200);
                //MySqlParam[6].Direction = ParameterDirection.Output;
                //string err = Convert.ToString(MySqlParam[6].Value);
                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetAllLocalityMasterDetails", CommandType.StoredProcedure, MySqlParam);
                //error = Convert.ToString(MySqlParam[6].Value);
                /* #CC06 add end*/

                /* #CC06 comment start */
                //SqlParam = new SqlParameter[6];
                //SqlParam[0] = new SqlParameter("@stateid", StateId);
                //SqlParam[1] = new SqlParameter("@DistrictId", DistrictId);
                //SqlParam[2] = new SqlParameter("@CityId", CityId);
                //SqlParam[3] = new SqlParameter("@countryid", CountryId);
                //SqlParam[4] = new SqlParameter("@tehsillid", tehsillid);
                //SqlParam[5] = new SqlParameter("@CompanyId", CompanyId);
                //d1 = DataAccess.Instance.GetTableFromDatabase("prcGetAllLocalityMasterDetails", CommandType.StoredProcedure, SqlParam);
                /* #CC06 comment end*/
                return d1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void InsertUpdateCategoryInfo()
        {
            try
            {
                string outex = "";


                MySqlParam = new MySqlParameter[6];
                MySqlParam[0] = new MySqlParameter("@p_categoryid", categoryid);
                // SqlParam[0].Direction = ParameterDirection.InputOutput;
                MySqlParam[1] = new MySqlParameter("@p_categoryname", categoryname);
                MySqlParam[2] = new MySqlParameter("@p_status", Status);

                MySqlParam[3] = new MySqlParameter("@p_Out_Error", MySqlDbType.VarChar, 200); ;
                MySqlParam[3].Direction = ParameterDirection.Output;
                MySqlParam[4] = new MySqlParameter("@p_UserID", UserId);
                MySqlParam[5] = new MySqlParameter("@p_CompanyId", CompanyId);

                int r = DataAccess.Instance.DBInsert_MySqlCommand("PrcInsUpdCategoryInfo", MySqlParam);
                //IntResultCount = Convert.ToInt32(SqlParam[0].Value);
                error = Convert.ToString(MySqlParam[3].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectCategoryInfo()
        {

            try
            {



                MySqlParam = new MySqlParameter[5];
                MySqlParam[0] = new MySqlParameter("@p_categoryid", categoryid);
                MySqlParam[1] = new MySqlParameter("@p_categoryname", categoryname);
                MySqlParam[2] = new MySqlParameter("@p_selectionmode", categoryselectionmode);
                MySqlParam[3] = new MySqlParameter("@p_UserID", UserId);
                MySqlParam[4] = new MySqlParameter("@p_CompanyId", CompanyId);


                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcSelectCategoryDetails", CommandType.StoredProcedure, MySqlParam);


                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectAllCategory()
        {
            try
            {

                categoryid = 0;
                categoryname = "";
                categoryselectionmode = 1;



                MySqlParam = new MySqlParameter[5];
                MySqlParam[0] = new MySqlParameter("@p_categoryid", categoryid);
                MySqlParam[1] = new MySqlParameter("@p_categoryname", categoryname);
                MySqlParam[2] = new MySqlParameter("@p_selectionmode", categoryselectionmode);
                MySqlParam[3] = new MySqlParameter("@p_UserID", UserId);
                MySqlParam[4] = new MySqlParameter("@p_CompanyId", CompanyId);


                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcSelectCategoryDetails", CommandType.StoredProcedure, MySqlParam);

                return d1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region SubCategory
        int subcategoryid; string subcategoryname; int catid; bool substatus; int subcategoryselectionmode;

        public int SubCategoryID
        {
            get { return subcategoryid; }
            set { subcategoryid = value; }
        }

        public string SubCategoryName
        {
            get { return subcategoryname; }
            set { subcategoryname = value; }
        }
        public bool SubStatus
        {
            get { return substatus; }
            set { substatus = value; }
        }

        public int CatID
        {
            get { return catid; }
            set { catid = value; }
        }

        public int SubCategorySelectionMode
        {
            get { return subcategoryselectionmode; }
            set { subcategoryselectionmode = value; }
        }

        public Int32 UserId { get; set; }
        public Int32 CompanyId { get; set; }
        public DataTable SelectAllSubCategoryInfo()
        {
            try
            {

                subcategoryid = 0;
                subcategoryname = "";
                catid = 0;
                subcategoryselectionmode = 1;


                MySqlParam = new MySqlParameter[6];
                MySqlParam[0] = new MySqlParameter("@p_subcategoryid", subcategoryid);

                MySqlParam[1] = new MySqlParameter("@p_subcategoryname", subcategoryname);
                MySqlParam[2] = new MySqlParameter("@p_catid", catid);
                MySqlParam[3] = new MySqlParameter("@p_selectionmode", subcategoryselectionmode);
                MySqlParam[4] = new MySqlParameter("@p_UserID", UserId);

                MySqlParam[5] = new MySqlParameter("@p_CompanyId", CompanyId);


                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcSelectSubCategoryDetails", CommandType.StoredProcedure, MySqlParam);


                return d1;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public DataTable SelectSubCategoryInfo()
        {
            try
            {


                MySqlParam = new MySqlParameter[6];
                MySqlParam[0] = new MySqlParameter("@p_subcategoryid", subcategoryid);
                MySqlParam[1] = new MySqlParameter("@p_catid", catid);

                MySqlParam[2] = new MySqlParameter("@p_subcategoryname", subcategoryname);
                MySqlParam[3] = new MySqlParameter("@p_selectionmode", subcategoryselectionmode);
                MySqlParam[4] = new MySqlParameter("@p_UserID", UserId);

                MySqlParam[5] = new MySqlParameter("@p_CompanyId", CompanyId);

                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcSelectSubCategoryDetails", CommandType.StoredProcedure, MySqlParam);

                return d1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAllBulletinSubCategory()
        {
            try
            {
                MySqlParam = new MySqlParameter[4];
                MySqlParam[0] = new MySqlParameter("@p_CategoryID", CategoryID);
                MySqlParam[1] = new MySqlParameter("@p_UserID", UserId);

                MySqlParam[2] = new MySqlParameter("@p_CompanyId", CompanyId);
                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetAllBulletinSubCategory", CommandType.StoredProcedure, MySqlParam);
                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetAllBulletinSubCategorybyCategoryId()
        {
            try
            {
                MySqlParam = new MySqlParameter[3];
                MySqlParam[0] = new MySqlParameter("@p_CategoryID", CategoryID);
                MySqlParam[1] = new MySqlParameter("@p_UserID", UserId);

                MySqlParam[2] = new MySqlParameter("@p_CompanyId", CompanyId);
                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetAllBulletinSubCategory", CommandType.StoredProcedure, MySqlParam);
                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetAllBulletinCategory()
        {
            try
            {
                MySqlParam = new MySqlParameter[3];
                
                MySqlParam[0] = new MySqlParameter("@p_UserID", UserId);

                MySqlParam[1] = new MySqlParameter("@p_CompanyId", CompanyId);
                d1 = DataAccess.Instance.GetTableFrom_MySqlDatabase("prcGetAllBulletinCategory", CommandType.StoredProcedure, MySqlParam);
                return d1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsertSubCategoryInfo()
        {
            try
            {
                subcategoryid = 0;

                string outex = "";



                MySqlParam = new MySqlParameter[7];
                MySqlParam[0] = new MySqlParameter("@p_catid", catid);
                MySqlParam[1] = new MySqlParameter("@p_substatus", substatus);
                MySqlParam[2] = new MySqlParameter("@p_subcategoryname", subcategoryname);
                MySqlParam[3] = new MySqlParameter("@p_subcategoryid", subcategoryid);

                MySqlParam[4] = new MySqlParameter("@p_Out_Error", outex);
                MySqlParam[4].Direction = ParameterDirection.Output;
                MySqlParam[5] = new MySqlParameter("@p_UserID", UserId);
                MySqlParam[6] = new MySqlParameter("@p_CompanyId", CompanyId);

                int r = DataAccess.Instance.DBInsert_MySqlCommand("prcInsertUpdSubCategory", MySqlParam);
                error = Convert.ToString(MySqlParam[4].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateSubCategoryInfo()
        {
            try
            {

                string outex = "";


                MySqlParam = new MySqlParameter[7];
                MySqlParam[0] = new MySqlParameter("@p_subcategoryid", subcategoryid);
                MySqlParam[1] = new MySqlParameter("@p_catid", catid);
                MySqlParam[2] = new MySqlParameter("@p_substatus", substatus);
                MySqlParam[3] = new MySqlParameter("@p_subcategoryname", subcategoryname);

                MySqlParam[4] = new MySqlParameter("@p_Out_Error", outex);
                MySqlParam[4].Direction = ParameterDirection.Output;
                MySqlParam[5] = new MySqlParameter("@p_UserID", UserId);
                MySqlParam[6] = new MySqlParameter("@p_CompanyId", CompanyId);
                int r = DataAccess.Instance.DBInsert_MySqlCommand("prcInsertUpdSubCategory", MySqlParam);
                error = Convert.ToString(MySqlParam[4].Value);

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

        ~MastersData()
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
