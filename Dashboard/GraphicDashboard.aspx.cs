using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Web.Services;


public partial class Dashboard_GraphicDashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if(Convert.ToString(BussinessLogic.PageBase.UserId)=="")
        {
            Response.Redirect(ConfigurationManager.AppSettings["siteurl"] + "/logout.aspx", false);
        }
        SetAPiValues();
    }

    private void SetAPiValues()
    {
        try
        {
            hdnConstr.Value = Convert.ToString(ConfigurationManager.AppSettings["APIConnStringKey"]);
            hdnAPIURL.Value = Convert.ToString(ConfigurationManager.AppSettings["APIurl"]);
            hdnUserID.Value = Convert.ToString(BussinessLogic.PageBase.UserId);
            hdnRoleID.Value = Convert.ToString(BussinessLogic.PageBase.RoleID);

            lnkBootstrap.Attributes.Add("href", PageBase.siteURL  + Convert.ToString(ConfigurationManager.AppSettings["AssetsPath"]) +"/css/bootstrap.min.css");
            lnkGraphics.Attributes.Add("href", PageBase.siteURL + Convert.ToString(ConfigurationManager.AppSettings["AssetsPath"]) + "/css/GraphicDashboard.css");
        }
        catch (Exception ex)
        {            

            throw ex;
        }
    }


    #region GetSaleData

    public class clsSaleData
    {
        public string SaleType { get; set; }
        public Int64 MTD { get; set; }
        public Int64 LMTD { get; set; }
        public Int64 LMSale { get; set; }
    }
    public class clsSaleDataList
    {
        public List<clsSaleData> ItemLists = new List<clsSaleData>();
    }

    [WebMethod()]
    public static object GetSaleData()
    {
        DataTable objdata = new DataTable();
        DataSet dsResult = new DataSet();
        //API.Models.Masters.ResponseStatus ObjResponse = new API.Models.Masters.ResponseStatus();
        clsSaleDataList objSaleDataList = new clsSaleDataList();
        try
        {
            SqlParameter[] SqlParam = new SqlParameter[3];
            //dsResult = SqlHelper.ExecuteDataset(clsBase.strPbConString1, CommandType.StoredProcedure, "prcGetSaleData", SqlParam);
            SqlParam[0] = new SqlParameter("@UserId", PageBase.UserId);
            SqlParam[1] = new SqlParameter("@RoleID", PageBase.RoleID);
            dsResult = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.StoredProcedure, "prcGetSaleData", SqlParam);

            if (dsResult != null && dsResult.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                {
                    clsSaleData ObjclsSaleData = new clsSaleData();
                    ObjclsSaleData.SaleType = Convert.ToString(dsResult.Tables[0].Rows[i]["SaleType"]);
                    ObjclsSaleData.MTD = Convert.ToInt64(dsResult.Tables[0].Rows[i]["MTD"]);
                    ObjclsSaleData.LMTD = Convert.ToInt64(dsResult.Tables[0].Rows[i]["LMTD"]);
                    ObjclsSaleData.LMSale = Convert.ToInt64(dsResult.Tables[0].Rows[i]["LMSale"]);
                    objSaleDataList.ItemLists.Add(ObjclsSaleData);
                }
            }
            else
            {
                /*ObjResponse.status = "1";
                ObjResponse.message = "No Record Found.";
                return ObjResponse;*/
            }
            return objSaleDataList;
           
        }
        catch (Exception ex)
        {
            
            throw ex;
        }


    }
#endregion  

    #region StateWiseMTD

    public class clsStateWiseMTD
    {
        public string State { get; set; }
        public Int64 Qty { get; set; }
    }
    public class clsStateWiseMTDList
    {
        public List<clsStateWiseMTD> ItemLists = new List<clsStateWiseMTD>();
    }
      [WebMethod()]
    public static object GetStateWiseMTDData()
    {
        DataTable objdata = new DataTable();
        DataSet dsResult = new DataSet();
       // API.Models.Masters.ResponseStatus ObjResponse = new API.Models.Masters.ResponseStatus();
        clsStateWiseMTDList objItemList = new clsStateWiseMTDList();
        try
        {
          

            SqlParameter[] SqlParam = new SqlParameter[3];
            
            SqlParam[0] = new SqlParameter("@UserId", PageBase.UserId);
            SqlParam[1] = new SqlParameter("@RoleID", PageBase.RoleID);
            dsResult = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.StoredProcedure, "prcGetStateMTDSale", SqlParam);
            if (dsResult != null && dsResult.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                {
                    clsStateWiseMTD ObjclsStateWiseMTD = new clsStateWiseMTD();
                    ObjclsStateWiseMTD.State = Convert.ToString(dsResult.Tables[0].Rows[i]["StateName"]);
                    ObjclsStateWiseMTD.Qty = Convert.ToInt64(dsResult.Tables[0].Rows[i]["qty"]);
                    objItemList.ItemLists.Add(ObjclsStateWiseMTD);
                }
            }
            else
            {
                //ObjResponse.status = "1";
                //ObjResponse.message = "No Record Found.";
                //return ObjResponse;
            }
            return objItemList;
            //}
        }
        catch (Exception ex)
        {
           
            throw ex;
        }


    }
    #endregion
    #region TargetVsAchievement

    public class clsAchievement
    {
        public string TargetName { get; set; }
        public string TargetBase { get; set; }
        public Int64 TargetAchieve { get; set; }

    }
    public class clsAchievementList
    {
        public List<clsAchievement> ItemLists = new List<clsAchievement>();
    }

     [WebMethod()]
    public static object GetTargetAchievementData()
    {
        DataTable objdata = new DataTable();
        DataSet dsResult = new DataSet();
        //API.Models.Masters.ResponseStatus ObjResponse = new API.Models.Masters.ResponseStatus();
        clsAchievementList objTargetAchievement = new clsAchievementList();
        try
        {
            SqlParameter[] SqlParam = new SqlParameter[3];
            //dsResult = SqlHelper.ExecuteDataset(clsBase.strPbConString1, CommandType.StoredProcedure, "prcGetTargetVsAchievement", SqlParam);
            SqlParam[0] = new SqlParameter("@UserId", PageBase.UserId);
            SqlParam[1] = new SqlParameter("@RoleID", PageBase.RoleID);
            dsResult = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.StoredProcedure, "prcGetTargetVsAchievement", SqlParam);

            if (dsResult != null && dsResult.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                {
                    clsAchievement ObjclsAchievement = new clsAchievement();
                    ObjclsAchievement.TargetName = Convert.ToString(dsResult.Tables[0].Rows[i]["TargetName"]);
                    ObjclsAchievement.TargetBase = Convert.ToString(dsResult.Tables[0].Rows[i]["TargetBase"]);
                    ObjclsAchievement.TargetAchieve = Convert.ToInt64(dsResult.Tables[0].Rows[i]["TargetAchieve"]);
                    objTargetAchievement.ItemLists.Add(ObjclsAchievement);
                }
            }
            else
            {
                //ObjResponse.status = "1";
                //ObjResponse.message = "No Record Found.";
                //return ObjResponse;
            }
            return objTargetAchievement;
            //}
        }
        catch (Exception ex)
        {
            ////if (AccountId != 0)
            ////{
            ////    if (intwritelog == 0)
            ////        clsBase.WriteLogToTextFile(ex.ToString(), UserId);
            ////    clsBase.Exception(System.Net.HttpStatusCode.InternalServerError, clsBase.ErrorMessage("500", clsBase.InternalServerError));
            ////}
            ////else
            //{
            //    clsBase.Exception(System.Net.HttpStatusCode.Unauthorized, clsBase.ErrorMessage("401", clsBase.strReturnToken));
            //}
            throw ex;
        }


    }
    #endregion
    #region GetTopModels

    public class clsTopModels
    {
        public string ModelName { get; set; }
        public Int64 SalePercentage { get; set; }
    }
    public class clsTopModelsList
    {
        public List<clsTopModels> ItemLists = new List<clsTopModels>();
    }

    [WebMethod()]
    public static object GetTopModels()
    {
        DataTable objdata = new DataTable();
        DataSet dsResult = new DataSet();
        //API.Models.Masters.ResponseStatus ObjResponse = new API.Models.Masters.ResponseStatus();
        clsTopModelsList objTopModelsList = new clsTopModelsList();
        try
        {
            SqlParameter[] SqlParam = new SqlParameter[3];
            // dsResult = SqlHelper.ExecuteDataset(clsBase.strPbConString1, CommandType.StoredProcedure, "prcGetTopModels", SqlParam);
            SqlParam[0] = new SqlParameter("@UserId", PageBase.UserId);
            SqlParam[1] = new SqlParameter("@RoleID", PageBase.RoleID);
            dsResult = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.StoredProcedure, "prcGetTopModels", SqlParam);

            if (dsResult != null && dsResult.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                {
                    clsTopModels ObjclsTopModels = new clsTopModels();
                    ObjclsTopModels.ModelName = Convert.ToString(dsResult.Tables[0].Rows[i]["ModelName"]);
                    ObjclsTopModels.SalePercentage = Convert.ToInt64(dsResult.Tables[0].Rows[i]["SalePercentage"]);
                    objTopModelsList.ItemLists.Add(ObjclsTopModels);
                }
            }
            else
            {
                //ObjResponse.status = "1";
                //ObjResponse.message = "No Record Found.";
                //return ObjResponse;
            }
            return objTopModelsList;
            //}
        }
        catch (Exception ex)
        {
            //if (AccountId != 0)
            //{
            //    if (intwritelog == 0)
            //        clsBase.WriteLogToTextFile(ex.ToString(), UserId);
            //    clsBase.Exception(System.Net.HttpStatusCode.InternalServerError, clsBase.ErrorMessage("500", clsBase.InternalServerError));
            //}
            //else
            //{
            //    clsBase.Exception(System.Net.HttpStatusCode.Unauthorized, clsBase.ErrorMessage("401", clsBase.strReturnToken));
            //}
            throw ex;
        }


    }
    #endregion
    #region GetLastDayStock

    public class clsLastDayStock
    {
        public string SaleChannelType { get; set; }
        public string EntityType { get; set; }
        public Int64 StockPercantage { get; set; }
        public Int64 StockQty { get; set; }
    }
    public class clsLastDayStockList
    {
        public List<clsLastDayStock> ItemLists = new List<clsLastDayStock>();
    }

    [WebMethod()]
    public static object GetLastDayStock()
    {
        DataTable objdata = new DataTable();
        DataSet dsResult = new DataSet();
       // API.Models.Masters.ResponseStatus ObjResponse = new API.Models.Masters.ResponseStatus();
        clsLastDayStockList objLastDayStockList = new clsLastDayStockList();
        try
        {
            SqlParameter[] SqlParam = new SqlParameter[3];
            //dsResult = SqlHelper.ExecuteDataset(clsBase.strPbConString1, CommandType.StoredProcedure, "prcGetLastDayStock", SqlParam);

            SqlParam[0] = new SqlParameter("@UserId", PageBase.UserId);
            SqlParam[1] = new SqlParameter("@RoleID", PageBase.RoleID);
            dsResult = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.StoredProcedure, "prcGetLastDayStock", SqlParam);

            if (dsResult != null && dsResult.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                {
                    clsLastDayStock ObjclsLastDayStock = new clsLastDayStock();
                    ObjclsLastDayStock.SaleChannelType = Convert.ToString(dsResult.Tables[0].Rows[i]["SaleChannelType"]);
                    ObjclsLastDayStock.StockPercantage = Convert.ToInt64(dsResult.Tables[0].Rows[i]["StockPercantage"]);
                    ObjclsLastDayStock.EntityType = Convert.ToString(dsResult.Tables[0].Rows[i]["EntityType"]);
                    ObjclsLastDayStock.StockQty = Convert.ToInt64(dsResult.Tables[0].Rows[i]["StockQty"]);
                    objLastDayStockList.ItemLists.Add(ObjclsLastDayStock);
                }
            }
            else
            {
                //ObjResponse.status = "1";
                //ObjResponse.message = "No Record Found.";
                //return ObjResponse;
            }
            return objLastDayStockList;
            //}
        }
        catch (Exception ex)
        {
            //if (AccountId != 0)
            //{
            //    if (intwritelog == 0)
            //        clsBase.WriteLogToTextFile(ex.ToString(), UserId);
            //    clsBase.Exception(System.Net.HttpStatusCode.InternalServerError, clsBase.ErrorMessage("500", clsBase.InternalServerError));
            //}
            //else
            //{
            //    clsBase.Exception(System.Net.HttpStatusCode.Unauthorized, clsBase.ErrorMessage("401", clsBase.strReturnToken));
            //}
            throw ex;
        }


    }
    #endregion
    #region GetTopDistributors

    public class clsTopDistributor
    {
        public string DistName { get; set; }
        public Int64 TertSale { get; set; }
    }
    public class clsTopDistributorList
    {
        public List<clsTopDistributor> ItemLists = new List<clsTopDistributor>();
    }

    [WebMethod()]
    public static object GetTopDistributor()
    {
        DataTable objdata = new DataTable();
        DataSet dsResult = new DataSet();
       // API.Models.Masters.ResponseStatus ObjResponse = new API.Models.Masters.ResponseStatus();
        clsTopDistributorList objTopDistributorList = new clsTopDistributorList();
        try
        {
            SqlParameter[] SqlParam = new SqlParameter[3];
            // dsResult = SqlHelper.ExecuteDataset(clsBase.strPbConString1, CommandType.StoredProcedure, "prcGetTopDistributor", SqlParam);

            SqlParam[0] = new SqlParameter("@UserId", PageBase.UserId);
            SqlParam[1] = new SqlParameter("@RoleID", PageBase.RoleID);
            dsResult = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.StoredProcedure, "prcGetTopDistributor", SqlParam);

            if (dsResult != null && dsResult.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                {
                    clsTopDistributor ObjclsTopDistributor = new clsTopDistributor();
                    ObjclsTopDistributor.DistName = Convert.ToString(dsResult.Tables[0].Rows[i]["DistName"]);
                    ObjclsTopDistributor.TertSale = Convert.ToInt64(dsResult.Tables[0].Rows[i]["TertSale"]);
                    objTopDistributorList.ItemLists.Add(ObjclsTopDistributor);
                }
            }
            else
            {
                //ObjResponse.status = "1";
                //ObjResponse.message = "No Record Found.";
                //return ObjResponse;
            }
            return objTopDistributorList;
            //}
        }
        catch (Exception ex)
        {
            //if (AccountId != 0)
            //{
            //    if (intwritelog == 0)
            //        clsBase.WriteLogToTextFile(ex.ToString(), UserId);
            //    clsBase.Exception(System.Net.HttpStatusCode.InternalServerError, clsBase.ErrorMessage("500", clsBase.InternalServerError));
            //}
            //else
            //{
            //    clsBase.Exception(System.Net.HttpStatusCode.Unauthorized, clsBase.ErrorMessage("401", clsBase.strReturnToken));
            //}
            throw ex;
        }


    }
    #endregion
    #region GetRegionalSales

    public class clsRegionalSale
    {
        public string RegionName { get; set; }
        public decimal Sale { get; set; }
    }
    public class clsRegionalSaleList
    {
        public List<clsRegionalSale> ItemLists = new List<clsRegionalSale>();
    }

    [WebMethod()]
    public static object GetRegionalSale()
    {
        DataTable objdata = new DataTable();
        DataSet dsResult = new DataSet();
       // API.Models.Masters.ResponseStatus ObjResponse = new API.Models.Masters.ResponseStatus();
        clsRegionalSaleList objRegionalSaleList = new clsRegionalSaleList();
        try
        {
            SqlParameter[] SqlParam = new SqlParameter[3];
            // dsResult = SqlHelper.ExecuteDataset(clsBase.strPbConString1, CommandType.StoredProcedure, "prcGetRegionalSale", SqlParam);

            SqlParam[0] = new SqlParameter("@UserId", PageBase.UserId);
            SqlParam[1] = new SqlParameter("@RoleID", PageBase.RoleID);
            dsResult = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.StoredProcedure, "prcGetRegionalSale", SqlParam);

            if (dsResult != null && dsResult.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                {
                    clsRegionalSale ObjclsRegionalSale = new clsRegionalSale();
                    ObjclsRegionalSale.RegionName = Convert.ToString(dsResult.Tables[0].Rows[i]["Region"]);
                    ObjclsRegionalSale.Sale = Convert.ToDecimal(dsResult.Tables[0].Rows[i]["SalePercent"]);
                    objRegionalSaleList.ItemLists.Add(ObjclsRegionalSale);
                }
            }
            else
            {
                //ObjResponse.status = "1";
                //ObjResponse.message = "No Record Found.";
                //return ObjResponse;
            }
            return objRegionalSaleList;
            //}
        }
        catch (Exception ex)
        {
            //if (AccountId != 0)
            //{
            //    if (intwritelog == 0)
            //        clsBase.WriteLogToTextFile(ex.ToString(), UserId);
            //    clsBase.Exception(System.Net.HttpStatusCode.InternalServerError, clsBase.ErrorMessage("500", clsBase.InternalServerError));
            //}
            //else
            //{
            //    clsBase.Exception(System.Net.HttpStatusCode.Unauthorized, clsBase.ErrorMessage("401", clsBase.strReturnToken));
            //}
            throw ex;
        }


    }
    #endregion
    
    
    #region GetWODLastMonths

    public class clsWODLastMonths
    {
        public string MonthYear { get; set; }
        public Int64 OldWOD { get; set; }
        public Int64 NewWOD { get; set; }
    }
    public class clsWODLastMonthsList
    {
        public List<clsWODLastMonths> ItemLists = new List<clsWODLastMonths>();
    }

    [WebMethod()]
    public static object GetWODLastMonths()
    {
        DataTable objdata = new DataTable();
        DataSet dsResult = new DataSet();
       // API.Models.Masters.ResponseStatus ObjResponse = new API.Models.Masters.ResponseStatus();
        clsWODLastMonthsList objWODLastMonthsList = new clsWODLastMonthsList();
        try
        {
            SqlParameter[] SqlParam = new SqlParameter[3];
            // dsResult = SqlHelper.ExecuteDataset(clsBase.strPbConString1, CommandType.StoredProcedure, "prcGetWODLastMonths", SqlParam);
            SqlParam[0] = new SqlParameter("@UserId", PageBase.UserId);
            SqlParam[1] = new SqlParameter("@RoleID", PageBase.RoleID);
            dsResult = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.StoredProcedure, "prcGetWODLastMonths", SqlParam);

            if (dsResult != null && dsResult.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                {
                    clsWODLastMonths ObjclsWODLastMonths = new clsWODLastMonths();
                    ObjclsWODLastMonths.MonthYear = Convert.ToString(dsResult.Tables[0].Rows[i]["MonthYear"]);
                    ObjclsWODLastMonths.OldWOD = Convert.ToInt64(dsResult.Tables[0].Rows[i]["OldWOD"]);
                    ObjclsWODLastMonths.NewWOD = Convert.ToInt64(dsResult.Tables[0].Rows[i]["NewWOD"]);
                    objWODLastMonthsList.ItemLists.Add(ObjclsWODLastMonths);
                }
            }
            else
            {
                //ObjResponse.status = "1";
                //ObjResponse.message = "No Record Found.";
                //return ObjResponse;
            }
            return objWODLastMonthsList;
            //}
        }
        catch (Exception ex)
        {
            
            throw ex;
        }


    }
    #endregion
    
    #region GetCurrentVsLastMonthSale

    public class clsCurrentVsLastMonthSale
    {
        public string SaleDate { get; set; }
        public Int16 DayValue { get; set; }
        public Int16 MonthValue { get; set; }
        public Int64 YearValue { get; set; }
        public Int64 LastMonthSale { get; set; }
        public Int64 CurrentMonthSale { get; set; }

    }
    public class clsCurrentVsLastMonthSaleList
    {
        public List<clsCurrentVsLastMonthSale> ItemLists = new List<clsCurrentVsLastMonthSale>();
    }

    [WebMethod()]
    public static object GetCurrentVsLastMonthSale()
    {
        DataTable objdata = new DataTable();
        DataSet dsResult = new DataSet();
        //API.Models.Masters.ResponseStatus ObjResponse = new API.Models.Masters.ResponseStatus();
        clsCurrentVsLastMonthSaleList objCurrentVsLastMonthSaleList = new clsCurrentVsLastMonthSaleList();
        try
        {
            SqlParameter[] SqlParam = new SqlParameter[3];
            // dsResult = SqlHelper.ExecuteDataset(clsBase.strPbConString1, CommandType.StoredProcedure, "prcGetCurrentVsLastMonthSale", SqlParam);
            SqlParam[0] = new SqlParameter("@UserId", PageBase.UserId);
            SqlParam[1] = new SqlParameter("@RoleID", PageBase.RoleID);
            dsResult = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.StoredProcedure, "prcGetCurrentVsLastMonthSale", SqlParam);

            if (dsResult != null && dsResult.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                {
                    clsCurrentVsLastMonthSale ObjclsCurrentVsLastMonthSale = new clsCurrentVsLastMonthSale();
                    ObjclsCurrentVsLastMonthSale.SaleDate = Convert.ToString(dsResult.Tables[0].Rows[i]["SaleDate"]);
                    ObjclsCurrentVsLastMonthSale.DayValue = Convert.ToInt16(dsResult.Tables[0].Rows[i]["DayValue"]);
                    ObjclsCurrentVsLastMonthSale.MonthValue = Convert.ToInt16(dsResult.Tables[0].Rows[i]["MonthValue"]);
                    ObjclsCurrentVsLastMonthSale.YearValue = Convert.ToInt64(dsResult.Tables[0].Rows[i]["YearValue"]);
                    ObjclsCurrentVsLastMonthSale.LastMonthSale = Convert.ToInt64(dsResult.Tables[0].Rows[i]["LastMonthSale"]);
                    ObjclsCurrentVsLastMonthSale.CurrentMonthSale = Convert.ToInt64(dsResult.Tables[0].Rows[i]["CurrentMonthSale"]);
                    objCurrentVsLastMonthSaleList.ItemLists.Add(ObjclsCurrentVsLastMonthSale);
                }
            }
            else
            {
                //ObjResponse.status = "1";
                //ObjResponse.message = "No Record Found.";
                //return ObjResponse;
            }
            return objCurrentVsLastMonthSaleList;
            //}
        }
        catch (Exception ex)
        {
            //if (AccountId != 0)
            //{
            //    if (intwritelog == 0)
            //        clsBase.WriteLogToTextFile(ex.ToString(), UserId);
            //    clsBase.Exception(System.Net.HttpStatusCode.InternalServerError, clsBase.ErrorMessage("500", clsBase.InternalServerError));
            //}
            //else
            //{
            //    clsBase.Exception(System.Net.HttpStatusCode.Unauthorized, clsBase.ErrorMessage("401", clsBase.strReturnToken));
            //}
            throw ex;
        }


    }
    #endregion
    #region GetCurrentVsLastYearSale

    public class clsCurrentVsLastYearSale
    {
        public string SaleDate { get; set; }
        //public DateTime SaleDate { get; set; }
        public Int64 LastYearSale { get; set; }
        public Int64 CurrentYearSale { get; set; }

    }
    public class clsCurrentVsLastYearSaleList
    {
        public List<clsCurrentVsLastYearSale> ItemLists = new List<clsCurrentVsLastYearSale>();
    }

    [WebMethod()]
    public static object GetCurrentVsLastYearSale()
    {
        DataTable objdata = new DataTable();
        DataSet dsResult = new DataSet();
       // API.Models.Masters.ResponseStatus ObjResponse = new API.Models.Masters.ResponseStatus();
        clsCurrentVsLastYearSaleList objCurrentVsLastYearSaleList = new clsCurrentVsLastYearSaleList();
        try
        {
            SqlParameter[] SqlParam = new SqlParameter[3];
            //  dsResult = SqlHelper.ExecuteDataset(clsBase.strPbConString1, CommandType.StoredProcedure, "prcGetCurrentVsLastYearSale", SqlParam);
            SqlParam[0] = new SqlParameter("@UserId", PageBase.UserId);
            SqlParam[1] = new SqlParameter("@RoleID", PageBase.RoleID);
            dsResult = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.StoredProcedure, "prcGetCurrentVsLastYearSale", SqlParam);

            if (dsResult != null && dsResult.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                {
                    clsCurrentVsLastYearSale ObjclsCurrentVsLastYearSale = new clsCurrentVsLastYearSale();
                    ObjclsCurrentVsLastYearSale.SaleDate = Convert.ToString(dsResult.Tables[0].Rows[i]["SaleDate"]);
                    ObjclsCurrentVsLastYearSale.LastYearSale = Convert.ToInt64(dsResult.Tables[0].Rows[i]["LastYearSale"]);
                    ObjclsCurrentVsLastYearSale.CurrentYearSale = Convert.ToInt64(dsResult.Tables[0].Rows[i]["CurrentYearSale"]);
                    objCurrentVsLastYearSaleList.ItemLists.Add(ObjclsCurrentVsLastYearSale);
                }
            }
            else
            {
                //ObjResponse.status = "1";
                //ObjResponse.message = "No Record Found.";
                //return ObjResponse;
            }
            return objCurrentVsLastYearSaleList;
            //}
        }
        catch (Exception ex)
        {
            //if (AccountId != 0)
            //{
            //    if (intwritelog == 0)
            //        clsBase.WriteLogToTextFile(ex.ToString(), UserId);
            //    clsBase.Exception(System.Net.HttpStatusCode.InternalServerError, clsBase.ErrorMessage("500", clsBase.InternalServerError));
            //}
            //else
            //{
            //    clsBase.Exception(System.Net.HttpStatusCode.Unauthorized, clsBase.ErrorMessage("401", clsBase.strReturnToken));
            //}
            throw ex;
        }


    }
    #endregion
    #region GetAllSaleDetail

    public class clsAllSaleDetail
    {
        public string SaleDate { get; set; }
        //public Int16 DayValue { get; set; }
        //public Int16 MonthValue { get; set; }
        //public Int64 YearValue { get; set; }
        public Int64 PrimarySale { get; set; }
        public Int64 IntermediarySale { get; set; }
        public Int64 SecondarySale { get; set; }
        public Int64 TertiarySale { get; set; }

    }
    public class clsAllSaleDetailList
    {
        public List<clsAllSaleDetail> ItemLists = new List<clsAllSaleDetail>();
    }

    [WebMethod()]
    public static object GetAllSaleDetail()
    {
        DataTable objdata = new DataTable();
        DataSet dsResult = new DataSet();
        //API.Models.Masters.ResponseStatus ObjResponse = new API.Models.Masters.ResponseStatus();
        clsAllSaleDetailList objAllSaleDetailList = new clsAllSaleDetailList();
        try
        {
            SqlParameter[] SqlParam = new SqlParameter[3];
            // dsResult = SqlHelper.ExecuteDataset(clsBase.strPbConString1, CommandType.StoredProcedure, "prcGetAllSaleDetail", SqlParam);
            SqlParam[0] = new SqlParameter("@UserId", PageBase.UserId);
            SqlParam[1] = new SqlParameter("@RoleID", PageBase.RoleID);
            dsResult = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.StoredProcedure, "prcGetAllSaleDetail", SqlParam);

            if (dsResult != null && dsResult.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                {
                    clsAllSaleDetail ObjclsAllSaleDetail = new clsAllSaleDetail();
                    ObjclsAllSaleDetail.SaleDate = Convert.ToString(dsResult.Tables[0].Rows[i]["SaleDate"]);
                    ObjclsAllSaleDetail.PrimarySale = Convert.ToInt64(dsResult.Tables[0].Rows[i]["PrimarySale"]);
                    ObjclsAllSaleDetail.IntermediarySale = Convert.ToInt64(dsResult.Tables[0].Rows[i]["IntermediarySale"]);
                    ObjclsAllSaleDetail.SecondarySale = Convert.ToInt64(dsResult.Tables[0].Rows[i]["SecondarySale"]);
                    ObjclsAllSaleDetail.TertiarySale = Convert.ToInt64(dsResult.Tables[0].Rows[i]["TertiarySale"]);
                    objAllSaleDetailList.ItemLists.Add(ObjclsAllSaleDetail);
                }
            }
            else
            {
                //ObjResponse.status = "1";
                //ObjResponse.message = "No Record Found.";
                //return ObjResponse;
            }
            return objAllSaleDetailList;
            //}
        }
        catch (Exception ex)
        {
            //if (AccountId != 0)
            //{
            //    if (intwritelog == 0)
            //        clsBase.WriteLogToTextFile(ex.ToString(), UserId);
            //    clsBase.Exception(System.Net.HttpStatusCode.InternalServerError, clsBase.ErrorMessage("500", clsBase.InternalServerError));
            //}
            //else
            //{
            //    clsBase.Exception(System.Net.HttpStatusCode.Unauthorized, clsBase.ErrorMessage("401", clsBase.strReturnToken));
            //}
            throw ex;
        }


    }
    #endregion    
      #region GetLast6MonthsSale

    public class clsLast6MonthsSale
    {
        public string MonthYear { get; set; }
        public Int64 PrimarySale { get; set; }
        public Int64 IntermediarySale { get; set; }
        public Int64 SecondarySale { get; set; }
        public Int64 TertiarySale { get; set; }
        
    }
    public class clsLast6MonthsSaleList
    {
        public List<clsLast6MonthsSale> ItemLists = new List<clsLast6MonthsSale>();
    }

    [WebMethod()]
    public static object GetLast6MonthsSale()
    {
        DataTable objdata = new DataTable();
        DataSet dsResult = new DataSet();
       // API.Models.Masters.ResponseStatus ObjResponse = new API.Models.Masters.ResponseStatus();
        clsLast6MonthsSaleList objLast6MonthsSaleList = new clsLast6MonthsSaleList();
        try
        {
            SqlParameter[] SqlParam = new SqlParameter[3];
            // dsResult = SqlHelper.ExecuteDataset(clsBase.strPbConString1, CommandType.StoredProcedure, "prcGetLast6MonthsSale", SqlParam);
            SqlParam[0] = new SqlParameter("@UserId", PageBase.UserId);
            SqlParam[1] = new SqlParameter("@RoleID", PageBase.RoleID);
            dsResult = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.StoredProcedure, "prcGetLast6MonthsSale", SqlParam);

            if (dsResult != null && dsResult.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                {
                    clsLast6MonthsSale ObjclsLast6MonthsSale = new clsLast6MonthsSale();
                    ObjclsLast6MonthsSale.MonthYear = Convert.ToString(dsResult.Tables[0].Rows[i]["MonthYear"]);
                    ObjclsLast6MonthsSale.PrimarySale = Convert.ToInt64(dsResult.Tables[0].Rows[i]["PrimarySale"]);
                    ObjclsLast6MonthsSale.IntermediarySale = Convert.ToInt64(dsResult.Tables[0].Rows[i]["IntermediarySale"]);
                    ObjclsLast6MonthsSale.SecondarySale = Convert.ToInt64(dsResult.Tables[0].Rows[i]["SecondarySale"]);
                    ObjclsLast6MonthsSale.TertiarySale = Convert.ToInt64(dsResult.Tables[0].Rows[i]["TertiarySale"]);
                
                    objLast6MonthsSaleList.ItemLists.Add(ObjclsLast6MonthsSale);
                }
            }
            else
            {
                //ObjResponse.status = "1";
                //ObjResponse.message = "No Record Found.";
                //return ObjResponse;
            }
            return objLast6MonthsSaleList;
            //}
        }
        catch (Exception ex)
        {
            
            throw ex;
        }


    }
    #endregion
}