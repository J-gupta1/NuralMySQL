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


public partial class Dashboard_GraphicDashboard2 : System.Web.UI.Page
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

            lnkBootstrap.Attributes.Add("href", PageBase.siteURL  + Convert.ToString(ConfigurationManager.AppSettings["AssetsPath"]) +"css/bootstrap.min.css");
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

    public object GetSaleData()
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

}